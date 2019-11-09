Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Data
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Security.Principal
Imports System.Web
Imports System.Web.Security
Imports Ventrian.ImageResizer
Imports Ventrian.PropertyAgent

Namespace Ventrian.PropertyAgent.Controls
    Public Class SWFUploader
        Implements IHttpHandler

        ' Fields
        Private _category As String = Null.NullString
        Private _context As HttpContext
        Private _moduleID As Integer = Null.NullInteger
        Private _photos As ArrayList
        Private _portalID As Integer = Null.NullInteger
        Private _propertyGuid As String = Null.NullString
        Private _propertyID As Integer = Null.NullInteger
        Private _propertySettings As PropertySettings
        Private _settings As Hashtable
        Private _tabID As Integer = Null.NullInteger
        Private _tabModuleID As Integer = Null.NullInteger
        Private _ticket As String = Null.NullString
        Private _userID As Integer = Null.NullInteger
        ' Methods
        Private Sub AuthenticateUserFromTicket()
            If (Me._ticket <> "") Then
                Dim identity As New FormsIdentity(FormsAuthentication.Decrypt(Me._ticket))
                Dim roles As String() = Nothing
                HttpContext.Current.User = New GenericPrincipal(identity, roles)
                Dim userByName As UserInfo = UserController.GetUserByName(Me._portalID, HttpContext.Current.User.Identity.Name)
                If (Not userByName Is Nothing) Then
                    Me._userID = userByName.UserID
                    HttpContext.Current.Items.Item("UserInfo") = userByName
                    Dim userRoles As New List(Of UserRoleInfo)
                    Dim str As String = ""
                    userRoles = DirectCast(New RoleController().GetUserRoles(UserController.GetUserById(Me._portalID, Me._userID), True), List(Of UserRoleInfo))
                    Using enumerator As IEnumerator(Of UserRoleInfo) = userRoles.GetEnumerator
                        Do While enumerator.MoveNext
                            Dim current As UserRoleInfo = enumerator.Current
                            str = (str & str & ";")
                        Loop
                    End Using
                    Me._context.Items.Add("UserRoles", (";" & str & ";"))
                End If
            End If
        End Sub

        Public Function CheckLimit(ByVal context As HttpContext, ByVal permission As String, ByVal permissionLimit As String) As Boolean
            Try
                Dim objUser As UserInfo = UserController.Instance.GetCurrentUserInfo()
                'Dim userById As UserInfo = UserController.GetUserById(Me._portalID, Me._userID)
                Dim userRoles As List(Of UserRoleInfo) = DirectCast(New RoleController().GetUserRoles(objUser, True), List(Of UserRoleInfo))
                Dim nullInteger As Integer = Null.NullInteger
                Dim separator As Char() = New Char() {";"c}
                Dim str As String
                For Each str In permission.Split(separator)
                    If (str <> "") Then
                        Dim str2 As String = str
                        Dim chArray2 As Char() = New Char() {":"c}
                        If (str2.Split(chArray2).Length > 1) Then
                            Dim chArray3 As Char() = New Char() {":"c}
                            str2 = str2.Split(chArray3)(0)
                        End If
                        Dim info2 As UserRoleInfo
                        For Each info2 In userRoles
                            If (str.ToLower = info2.RoleName.ToLower) Then
                                Dim flag2 As Boolean = False
                                If (permissionLimit = "") Then
                                    Return (Null.NullInteger > 0)
                                End If
                                Dim chArray4 As Char() = New Char() {";"c}
                                Dim str3 As String
                                For Each str3 In permissionLimit.Split(chArray4)
                                    Dim chArray5 As Char() = New Char() {":"c}
                                    Dim str4 As String = str3.Split(chArray5)(0)
                                    Dim chArray6 As Char() = New Char() {":"c}
                                    Dim num4 As Integer = Convert.ToInt32(str3.Split(chArray6)(1))
                                    If (str2 = str4) Then
                                        If (nullInteger < num4) Then
                                            nullInteger = num4
                                        End If
                                        flag2 = True
                                    End If
                                Next
                                If Not flag2 Then
                                    Return (Null.NullInteger > 0)
                                End If
                            End If
                        Next
                    End If
                Next
                If ((nullInteger <> Null.NullInteger) AndAlso (New PhotoController().List(Me._propertyID, Me._propertyGuid).Count >= nullInteger)) Then
                    Return False
                End If
                Return True

            Catch ex As Exception
                Return False
            End Try

        End Function

        Private Function GetTabModuleSettings(ByVal TabModuleId As Integer, ByVal settings As Hashtable) As Hashtable
            Dim tabModuleSettings As IDataReader = DataProvider.Instance.GetTabModuleSettings(TabModuleId)
            Do While tabModuleSettings.Read
                If Not tabModuleSettings.IsDBNull(1) Then
                    settings.Item(tabModuleSettings.GetString(0)) = tabModuleSettings.GetString(1)
                Else
                    settings.Item(tabModuleSettings.GetString(0)) = ""
                End If
            Loop
            tabModuleSettings.Close()
            Return settings
        End Function

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest



            Me._context = context

            context.Response.ContentType = "text/plain"
            Try
                'issue to check the first time PropertySettins is called give an exception
                Dim largeWidth2 As Integer = Me.PropertySettings.LargeWidth
            Catch ex As Exception

            End Try

            Me.ReadQueryString(Me.PropertySettings.SEOPropertyID)
                Me.AuthenticateUserFromTicket()

                If Not Me._context.Request.IsAuthenticated Then
                    Me._context.Response.Write("-2")
                    Me._context.Response.End()
                End If
                Dim file As HttpPostedFile = Nothing
                Dim files As HttpFileCollection = Me._context.Request.Files
                If (files.Count > 0) Then
                    file = files.Item(0)
                End If
                If (file Is Nothing) Then
                    Me._context.Response.Write("420")
                    Me._context.Response.End()
                Else
                    If Not New PortalController().HasSpaceAvailable(Me._portalID, CLng(file.ContentLength)) Then
                        Me._context.Response.Write("-1")
                        Me._context.Response.End()
                    End If
                    Dim name As String = Me._context.User.Identity.Name
                    If Not Me.CheckLimit(context, Me.PropertySettings.PermissionAddImages, Me.PropertySettings.PermissionAddImagesLimit) Then
                        Me._context.Response.Write("-4")
                        Me._context.Response.End()
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
                    objPhoto.Category = Me._category
                    Dim largeWidth As Integer = Me.PropertySettings.LargeWidth
                    Dim largeHeight As Integer = Me.PropertySettings.LargeHeight
                    Dim image As Image = Image.FromStream(file.InputStream)
                    Dim contentType As String = file.ContentType
                    objPhoto.Width = image.Width
                    objPhoto.Height = image.Height
                    If (objPhoto.Width > largeWidth) Then
                        objPhoto.Width = largeWidth
                        objPhoto.Height = Convert.ToInt32(CDbl((CDbl(objPhoto.Height) / (CDbl(image.Width) / CDbl(largeWidth)))))
                    End If
                    If (objPhoto.Height > largeHeight) Then
                        objPhoto.Height = largeHeight
                        objPhoto.Width = Convert.ToInt32(CDbl((CDbl(image.Width) / (CDbl(image.Height) / CDbl(largeHeight)))))
                    End If
                    If (Me.Photos.Count > 0) Then
                        objPhoto.SortOrder = (DirectCast(Me.Photos.Item((Me.Photos.Count - 1)), PhotoInfo).SortOrder + 1)
                    End If
                    Dim controller As New PhotoController
                    objPhoto.PhotoID = controller.Add(objPhoto)
                    If file.FileName.ToLower.EndsWith(".jpg") Then
                        contentType = "image/jpeg"
                    End If
                    If file.FileName.ToLower.EndsWith(".gif") Then
                        contentType = "image/gif"
                    End If
                    If file.FileName.ToLower.EndsWith(".png") Then
                        contentType = "image/png"
                    End If
                    Select Case contentType.ToLower
                        Case "image/jpeg"
                            objPhoto.Filename = (objPhoto.PhotoID.ToString & ".jpg")
                            Exit Select
                        Case "image/gif"
                            objPhoto.Filename = (objPhoto.PhotoID.ToString & ".gif")
                            Exit Select
                        Case "image/png"
                            objPhoto.Filename = (objPhoto.PhotoID.ToString & ".png")
                            Exit Select
                        Case Else
                            objPhoto.Filename = (objPhoto.PhotoID.ToString & ".jpg")
                            Exit Select
                    End Select
                    Dim currentPortalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
                    Dim path As String = (currentPortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & Me._moduleID.ToString & "\Images\")
                    If Not Directory.Exists(path) Then
                        Directory.CreateDirectory(path)
                    End If
                    Dim queryString As New NameValueCollection
                    queryString.Add("maxwidth", Me.PropertySettings.LargeWidth.ToString)
                    queryString.Add("maxheight", Me.PropertySettings.LargeHeight.ToString)
                    file.SaveAs((path & objPhoto.Filename))
                    Dim watermark As New WatermarkSettings(HttpContext.Current.Request.QueryString)
                    If (Me.PropertySettings.WatermarkEnabled And (Operators.CompareString(Me.PropertySettings.WatermarkText, "", False) > 0)) Then
                        watermark.WatermarkText = Me.PropertySettings.WatermarkText
                    End If
                    If (Me.PropertySettings.WatermarkEnabled And (Operators.CompareString(Me.PropertySettings.WatermarkImage, "", False) > 0)) Then
                        watermark.WatermarkImagePath = (currentPortalSettings.HomeDirectoryMapPath & Me.PropertySettings.WatermarkImage)
                        watermark.WatermarkImagePosition = DirectCast(Me.PropertySettings.WatermarkPosition, WatermarkPosition)
                    End If
                    Dim targetFile As String = (path & objPhoto.Filename)
                    ImageManager.getBestInstance.BuildImage((path & objPhoto.Filename), targetFile, queryString, watermark)
                    controller.Update(objPhoto)
                    Me._context.Response.Write("200")
                    Me._context.Response.End()
                End If

        End Sub

        Private Sub ReadQueryString(ByVal SEOPropertyID As String)
            Try


                'If (Me._context.Request.Item("ModuleID") <> "") Then
                '    Me._moduleID = Convert.ToInt32(Me._context.Request.Item("ModuleID"))
                'End If
                'If (Me._context.Request.Item("PortalID") <> "") Then
                '    Me._portalID = Convert.ToInt32(Me._context.Request.Item("PortalID"))
                'End If
                'Dim sEOPropertyID As String = Me.PropertySettings.SEOPropertyID
                'If (Me._context.Request.Item(sEOPropertyID) = "") Then
                '    sEOPropertyID = "PropertyID"
                'End If

                'If ((Me._context.Request.Item(sEOPropertyID) <> "") AndAlso Versioned.IsNumeric(Me._context.Request.Item(sEOPropertyID))) Then
                '    Me._propertyID = Convert.ToInt32(Me._context.Request.Item(sEOPropertyID))
                'End If
                If (_context.Request("ModuleID") <> "") Then
                    _moduleID = Convert.ToInt32(_context.Request("ModuleID"))
                End If

                If (_context.Request("PortalID") <> "") Then
                    _portalID = Convert.ToInt32(_context.Request("PortalID"))
                End If

                Dim propertyIDParam As String = SEOPropertyID
                If (_context.Request(propertyIDParam) = "") Then
                    propertyIDParam = "PropertyID"
                End If
                If (_context.Request(propertyIDParam) <> "") Then
                    If (IsNumeric(_context.Request(propertyIDParam))) Then
                        _propertyID = Convert.ToInt32(_context.Request(propertyIDParam))
                    End If
                End If
                If (Me._context.Request.Item("TabModuleID") <> "") Then
                    Me._tabModuleID = Convert.ToInt32(Me._context.Request.Item("TabModuleID"))
                End If
                If (Me._context.Request.Item("TabID") <> "") Then
                    Me._tabID = Convert.ToInt32(Me._context.Request.Item("TabID"))
                End If
                If (Me._context.Request.Item("Ticket") <> "") Then
                    Me._ticket = Me._context.Request.Item("Ticket")
                End If
                If (Me._context.Request.Item("PropertyGuid") <> "") Then
                    Me._propertyGuid = Me._context.Request.Item("PropertyGuid")
                End If
                If (Me._context.Request.Item("Category") <> "") Then
                    Me._category = Me._context.Request.Item("Category")
                End If
            Catch ex As Exception

            End Try
        End Sub


        ' Properties
        Public ReadOnly Property IsReusable As Boolean
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property Photos As ArrayList
            Get
                If (Me._photos Is Nothing) Then
                    Dim controller As New PhotoController
                    If (Me._propertyID = Null.NullInteger) Then
                        Me._photos = controller.List(Me._propertyID, Me._propertyGuid)
                    Else
                        Me._photos = controller.List(Me._propertyID)
                    End If
                End If
                Return Me._photos
            End Get
        End Property

        Public ReadOnly Property PropertySettings() As PropertySettings
            Get
                If (_propertySettings Is Nothing) Then
                    _propertySettings = New PropertySettings(Me.Settings)
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



        Private ReadOnly Property IHttpHandler_IsReusable As Boolean Implements IHttpHandler.IsReusable
            Get
                Throw New NotImplementedException()
            End Get
        End Property



    End Class
End Namespace
