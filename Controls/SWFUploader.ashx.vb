Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports DotNetNuke.Security

Imports Ventrian.ImageResizer

Namespace Ventrian.PropertyAgent.Controls

    Public Class SWFUploader
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _propertyID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _tabID As Integer = Null.NullInteger
        Private _tabModuleID As Integer = Null.NullInteger
        Private _portalID As Integer = Null.NullInteger
        Private _ticket As String = Null.NullString
        Private _userID As Integer = Null.NullInteger
        Private _propertyGuid As String = Null.NullString
        Private _category As String = Null.NullString

        Private _photos As ArrayList
        Private _propertySettings As PropertySettings
        Private _settings As Hashtable

        Private _context As HttpContext

#End Region

#Region " Private Methods "

        Private Sub AuthenticateUserFromTicket()

            If (_ticket <> "") Then

                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(_ticket)
                Dim fi As FormsIdentity = New FormsIdentity(ticket)

                Dim roles As String() = Nothing
                HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(fi, roles)

                Dim objUser As UserInfo = UserController.GetUserByName(_portalID, HttpContext.Current.User.Identity.Name)

                If Not (objUser Is Nothing) Then
                    _userID = objUser.UserID
                    HttpContext.Current.Items("UserInfo") = objUser

                    Dim objRoleController As New RoleController
                    roles = objRoleController.GetRolesByUser(_userID, _portalID)

                    Dim strPortalRoles As String = Join(roles, New Char() {";"c})
                    _context.Items.Add("UserRoles", ";" + strPortalRoles + ";")
                End If

            End If

        End Sub

        Private Function GetTabModuleSettings(ByVal TabModuleId As Integer, ByVal settings As Hashtable) As Hashtable

            Dim dr As IDataReader = DotNetNuke.Data.DataProvider.Instance().GetTabModuleSettings(TabModuleId)

            While dr.Read()

                If Not dr.IsDBNull(1) Then
                    settings(dr.GetString(0)) = dr.GetString(1)
                Else
                    settings(dr.GetString(0)) = ""
                End If

            End While

            dr.Close()

            Return settings

        End Function

        Private Sub ReadQueryString()

            If (_context.Request("ModuleID") <> "") Then
                _moduleID = Convert.ToInt32(_context.Request("ModuleID"))
            End If

            If (_context.Request("PortalID") <> "") Then
                _portalID = Convert.ToInt32(_context.Request("PortalID"))
            End If

            Dim propertyIDParam As String = PropertySettings.SEOPropertyID
            If (_context.Request(propertyIDParam) = "") Then
                propertyIDParam = "PropertyID"
            End If
            If (_context.Request(propertyIDParam) <> "") Then
                If (IsNumeric(_context.Request(propertyIDParam))) Then
                    _propertyID = Convert.ToInt32(_context.Request(propertyIDParam))
                End If
            End If

            If (_context.Request("TabModuleID") <> "") Then
                _tabModuleID = Convert.ToInt32(_context.Request("TabModuleID"))
            End If

            If (_context.Request("TabID") <> "") Then
                _tabID = Convert.ToInt32(_context.Request("TabID"))
            End If

            If (_context.Request("Ticket") <> "") Then
                _ticket = _context.Request("Ticket")
            End If

            If (_context.Request("PropertyGuid") <> "") Then
                _propertyGuid = _context.Request("PropertyGuid")
            End If

            If (_context.Request("Category") <> "") Then
                _category = _context.Request("Category")
            End If

        End Sub

#End Region

#Region " Private Properties "

        Private ReadOnly Property Photos() As ArrayList
            Get
                If (_photos Is Nothing) Then
                    Dim objPhotoController As New PhotoController()
                    If (_propertyID = Null.NullInteger) Then
                        _photos = objPhotoController.List(_propertyID, _propertyGuid)
                    Else
                        _photos = objPhotoController.List(_propertyID)
                    End If
                End If
                Return _photos
            End Get
        End Property

        Private ReadOnly Property PropertySettings() As PropertySettings
            Get
                If _propertySettings Is Nothing Then
                    _propertySettings = New Ventrian.PropertyAgent.PropertySettings(Settings)
                End If
                Return _propertySettings
            End Get
        End Property

        Private ReadOnly Property Settings() As Hashtable
            Get
                If _settings Is Nothing Then
                    Dim objModuleController As New ModuleController
                    _settings = objModuleController.GetModuleSettings(_moduleID)
                    _settings = GetTabModuleSettings(_tabModuleID, _settings)
                End If
                Return _settings
            End Get
        End Property

#End Region

        Public Function CheckLimit(ByVal context As HttpContext, ByVal permission As String, ByVal permissionLimit As String) As Boolean

            Dim objUser As UserInfo = UserController.GetCurrentUserInfo()

            Dim objRoleController As New RoleController
            Dim userRoles As String() = objRoleController.GetRolesByUser(objUser.UserID, objUser.PortalID)

            Dim limit As Integer = Null.NullInteger

            For Each role As String In permission.Split(";"c)

                If (role <> "") Then

                    Dim actualRole As String = role

                    If (actualRole.Split(":"c).Length > 1) Then
                        actualRole = actualRole.Split(":"c)(0)
                    End If

                    For Each userRole As String In userRoles
                        If (role.ToLower() = userRole.ToLower()) Then

                            Dim found As Boolean = False

                            If (permissionLimit = "") Then
                                Return Null.NullInteger
                            End If

                            For Each item As String In permissionLimit.Split(";"c)
                                Dim r As String = item.Split(":"c)(0)
                                Dim v As Integer = Convert.ToInt32(item.Split(":"c)(1))

                                If (actualRole = r) Then
                                    If (limit < v) Then
                                        limit = v
                                    End If
                                    found = True
                                End If
                            Next

                            If (found = False) Then
                                Return Null.NullInteger
                            End If

                        End If
                    Next

                End If

            Next

            If (limit <> Null.NullInteger) Then

                Dim objPhotoController As New PhotoController()
                Dim objPhotos As ArrayList = objPhotoController.List(_propertyID, _propertyGuid)

                If (objPhotos.Count >= limit) Then
                    Return False
                End If

            End If

            Return True

        End Function

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            _context = context
            context.Response.ContentType = "text/plain"

            ReadQueryString()
            AuthenticateUserFromTicket()

            If (_context.Request.IsAuthenticated = False) Then
                _context.Response.Write("-2")
                _context.Response.End()
            End If

            Dim objFile As HttpPostedFile = _context.Request.Files("Filedata")

            If Not (objFile Is Nothing) Then

                Dim objPortalController As New PortalController()
                If (objPortalController.HasSpaceAvailable(_portalID, objFile.ContentLength) = False) Then
                    _context.Response.Write("-1")
                    _context.Response.End()
                End If

                Dim username As String = _context.User.Identity.Name

                If (CheckLimit(context, PropertySettings.PermissionAddImages, PropertySettings.PermissionAddImagesLimit) = False) Then
                    _context.Response.Write("-4")
                    _context.Response.End()
                End If

                Dim objPhoto As New PhotoInfo

                objPhoto.PropertyID = _propertyID
                If (_propertyID = Null.NullInteger) Then
                    objPhoto.PropertyGuid = _propertyGuid
                End If
                objPhoto.DateCreated = DateTime.Now
                objPhoto.Filename = "Temporary.jpg"
                objPhoto.SortOrder = 0
                objPhoto.Title = ""
                objPhoto.PhotoType = PhotoType.Internal
                objPhoto.Category = _category

                Dim maxWidth As Integer = PropertySettings.LargeWidth
                Dim maxHeight As Integer = PropertySettings.LargeHeight

                Dim photo As Drawing.Image = Drawing.Image.FromStream(objFile.InputStream)
                Dim fileType As String = objFile.ContentType

                objPhoto.Width = photo.Width
                objPhoto.Height = photo.Height

                If (objPhoto.Width > maxWidth) Then
                    objPhoto.Width = maxWidth
                    objPhoto.Height = Convert.ToInt32(objPhoto.Height / (photo.Width / maxWidth))
                End If

                If (objPhoto.Height > maxHeight) Then
                    objPhoto.Height = maxHeight
                    objPhoto.Width = Convert.ToInt32(photo.Width / (photo.Height / maxHeight))
                End If

                If (Photos.Count > 0) Then
                    objPhoto.SortOrder = CType(Photos(Photos.Count - 1), PhotoInfo).SortOrder + 1
                End If

                Dim objPhotoController As New PhotoController()
                objPhoto.PhotoID = objPhotoController.Add(objPhoto)

                If (objFile.FileName.ToLower().EndsWith(".jpg")) Then
                    fileType = "image/jpeg"
                End If

                If (objFile.FileName.ToLower().EndsWith(".gif")) Then
                    fileType = "image/gif"
                End If

                If (objFile.FileName.ToLower().EndsWith(".png")) Then
                    fileType = "image/png"
                End If

                Select Case fileType.ToLower()
                    Case "image/jpeg"
                        objPhoto.Filename = objPhoto.PhotoID.ToString() & ".jpg"

                    Case "image/gif"
                        objPhoto.Filename = objPhoto.PhotoID.ToString() & ".gif"

                    Case "image/png"
                        objPhoto.Filename = objPhoto.PhotoID.ToString() & ".png"

                    Case Else
                        'Shouldn't get to here because of validators.
                        objPhoto.Filename = objPhoto.PhotoID.ToString() & ".jpg"
                End Select

                Dim objPortalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()
                Dim filePath As String = objPortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & _moduleID.ToString() & "\Images\"

                If Not (Directory.Exists(filePath)) Then
                    Directory.CreateDirectory(filePath)
                End If

                Dim objQueryString As New NameValueCollection()
                objQueryString.Add("maxwidth", PropertySettings.LargeWidth.ToString())
                objQueryString.Add("maxheight", PropertySettings.LargeHeight.ToString())

                objFile.SaveAs(filePath & objPhoto.Filename)

                Dim objWatermarkSettings As New WatermarkSettings(HttpContext.Current.Request.QueryString)
                If (PropertySettings.WatermarkEnabled And PropertySettings.WatermarkText <> "") Then
                    objWatermarkSettings.WatermarkText = PropertySettings.WatermarkText
                End If
                If (PropertySettings.WatermarkEnabled And PropertySettings.WatermarkImage <> "") Then
                    objWatermarkSettings.WatermarkImagePath = (objPortalSettings.HomeDirectoryMapPath & PropertySettings.WatermarkImage)
                    objWatermarkSettings.WatermarkImagePosition = PropertySettings.WatermarkPosition
                End If

                Dim target As String = filePath & objPhoto.Filename
                ImageManager.getBestInstance().BuildImage(filePath & objPhoto.Filename, target, objQueryString, objWatermarkSettings)

                'If (photo.Width < maxWidth And photo.Height < maxHeight And PropertySettings.WatermarkEnabled = False) Then
                '    ' Don't resize, just save.
                '    objFile.SaveAs(filePath & objPhoto.Filename)
                'Else
                '    Dim bmp As New Bitmap(objPhoto.Width, objPhoto.Height)
                '    Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

                '    If (PropertySettings.HighQuality) Then
                '        g.InterpolationMode = InterpolationMode.HighQualityBicubic
                '        g.SmoothingMode = SmoothingMode.HighQuality
                '        g.PixelOffsetMode = PixelOffsetMode.HighQuality
                '        g.CompositingQuality = CompositingQuality.HighQuality
                '    End If

                '    g.DrawImage(photo, 0, 0, objPhoto.Width, objPhoto.Height)

                '    If (PropertySettings.WatermarkEnabled And PropertySettings.WatermarkText <> "") Then
                '        Dim crSize As SizeF = New SizeF
                '        Dim brushColor As Brush = Brushes.Yellow
                '        Dim fnt As Font = New Font("Verdana", 11, FontStyle.Bold)
                '        Dim strDirection As StringFormat = New StringFormat

                '        strDirection.Alignment = StringAlignment.Center
                '        crSize = g.MeasureString(PropertySettings.WatermarkText, fnt)

                '        Dim yPixelsFromBottom As Integer = Convert.ToInt32(Convert.ToDouble(objPhoto.Height) * 0.05)
                '        Dim yPosFromBottom As Single = Convert.ToSingle((objPhoto.Height - yPixelsFromBottom) - (crSize.Height / 2))
                '        Dim xCenterOfImage As Single = Convert.ToSingle((objPhoto.Width / 2))

                '        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

                '        Dim semiTransBrush2 As SolidBrush = New SolidBrush(Color.FromArgb(153, 0, 0, 0))
                '        g.DrawString(PropertySettings.WatermarkText, fnt, semiTransBrush2, New PointF(xCenterOfImage + 1, yPosFromBottom + 1), strDirection)

                '        Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(153, 255, 255, 255))
                '        g.DrawString(PropertySettings.WatermarkText, fnt, semiTransBrush, New PointF(xCenterOfImage, yPosFromBottom), strDirection)
                '    End If

                '    If (PropertySettings.WatermarkEnabled And PropertySettings.WatermarkImage <> "") Then
                '        Dim watermark As String = objPortalSettings.HomeDirectoryMapPath & PropertySettings.WatermarkImage
                '        If (File.Exists(watermark)) Then
                '            Dim imgWatermark As Image = New Bitmap(watermark)
                '            Dim wmWidth As Integer = imgWatermark.Width
                '            Dim wmHeight As Integer = imgWatermark.Height

                '            Dim objImageAttributes As New ImageAttributes()
                '            Dim objColorMap As New ColorMap()
                '            objColorMap.OldColor = Color.FromArgb(255, 0, 255, 0)
                '            objColorMap.NewColor = Color.FromArgb(0, 0, 0, 0)
                '            Dim remapTable As ColorMap() = {objColorMap}
                '            objImageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)

                '            Dim colorMatrixElements As Single()() = {New Single() {1.0F, 0.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 1.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 1.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.3F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.0F, 1.0F}}
                '            Dim wmColorMatrix As New ColorMatrix(colorMatrixElements)
                '            objImageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)

                '            Dim xPosOfWm As Integer = ((objPhoto.Width - wmWidth) - 10)
                '            Dim yPosOfWm As Integer = 10

                '            Select Case PropertySettings.WatermarkPosition
                '                Case WatermarkPosition.TopLeft
                '                    xPosOfWm = 10
                '                    yPosOfWm = 10
                '                    Exit Select

                '                Case WatermarkPosition.TopRight
                '                    xPosOfWm = ((objPhoto.Width - wmWidth) - 10)
                '                    yPosOfWm = 10
                '                    Exit Select

                '                Case WatermarkPosition.BottomLeft
                '                    xPosOfWm = 10
                '                    yPosOfWm = ((objPhoto.Height - wmHeight) - 10)

                '                Case WatermarkPosition.BottomRight
                '                    xPosOfWm = ((objPhoto.Width - wmWidth) - 10)
                '                    yPosOfWm = ((objPhoto.Height - wmHeight) - 10)
                '            End Select

                '            g.DrawImage(imgWatermark, New Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight, _
                '             GraphicsUnit.Pixel, objImageAttributes)
                '            imgWatermark.Dispose()
                '        End If
                '    End If

                '    photo.Dispose()

                '    If (PropertySettings.HighQuality) Then

                '        Select Case fileType.ToLower()
                '            Case "image/jpeg"
                '                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                '                Dim encoderParameters As New EncoderParameters(1)
                '                encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                '                bmp.Save(filePath & objPhoto.Filename, info(1), encoderParameters)

                '            Case "image/gif"
                '                'Dim quantizer As New ImageQuantization.OctreeQuantizer(255, 8)
                '                'Dim bmpQuantized As Bitmap = quantizer.Quantize(bmp)
                '                'bmpQuantized.Save(filePath & objPhoto.Filename, ImageFormat.Gif)
                '                ' Not working in medium trust.


                '                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Gif)

                '            Case Else
                '                'Shouldn't get to here because of validators.                                
                '                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                '                Dim encoderParameters As New EncoderParameters(1)
                '                encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                '                bmp.Save(filePath & objPhoto.Filename, info(1), encoderParameters)
                '        End Select

                '    Else

                '        Select Case fileType.ToLower()
                '            Case "image/jpeg"
                '                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Jpeg)

                '            Case "image/gif"
                '                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Gif)

                '            Case Else
                '                'Shouldn't get to here because of validators.
                '                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Jpeg)
                '        End Select

                '    End If

                '    bmp.Dispose()
                'End If

                objPhotoController.Update(objPhoto)

            End If

            _context.Response.Write("0")
            _context.Response.End()


        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace
