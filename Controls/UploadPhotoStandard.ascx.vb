Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions

Namespace Ventrian.PropertyAgent.Controls

    Partial Public Class UploadPhotoStandard
        Inherits PropertyAgentControl

#Region " Private Members "

        Private _photos As ArrayList
        Private _propertyID As Integer = Null.NullInteger

#End Region

#Region " Private Properties "

        Private ReadOnly Property Photos() As ArrayList
            Get
                If (_photos Is Nothing) Then
                    Dim objPhotoController As New PhotoController()
                    _photos = objPhotoController.List(_propertyID)
                End If
                Return _photos
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            Dim propertyIDParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyIDParam) = "") Then
                propertyIDParam = "PropertyID"
            End If
            If Not (Request(propertyIDParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyIDParam))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                ReadQueryString()
                cmdUploadPhoto.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUploadPhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUploadPhoto.Click

            If (Page.IsValid) Then

                Dim objPhoto As New PhotoInfo

                objPhoto.PropertyID = _propertyID
                objPhoto.DateCreated = DateTime.Now
                objPhoto.Filename = "Temporary.jpg"
                objPhoto.SortOrder = 0
                objPhoto.Title = txtTitle.Text
                objPhoto.PhotoType = PhotoType.Internal

                Dim maxWidth As Integer = PropertySettings.LargeWidth
                Dim maxHeight As Integer = PropertySettings.LargeHeight

                Dim photo As Drawing.Image = Drawing.Image.FromStream(txtPhoto.PostedFile.InputStream)
                Dim fileType As String = txtPhoto.PostedFile.ContentType

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

                Select Case fileType.ToLower()
                    Case "image/jpeg"
                        objPhoto.Filename = objPhoto.PhotoID.ToString() & ".jpg"

                    Case "image/gif"
                        objPhoto.Filename = objPhoto.PhotoID.ToString() & ".gif"

                    Case Else
                        'Shouldn't get to here because of validators.
                        objPhoto.Filename = objPhoto.PhotoID.ToString() & ".jpg"
                End Select

                Dim filePath As String = Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & PropertyAgentBase.ModuleId.ToString() & "\Images\"

                If Not (Directory.Exists(filePath)) Then
                    Directory.CreateDirectory(filePath)
                End If

                If (photo.Width < maxWidth And photo.Height < maxHeight And PropertySettings.WatermarkEnabled = False) Then
                    txtPhoto.PostedFile.SaveAs(filePath & objPhoto.Filename)
                Else
                    Dim bmp As New Bitmap(objPhoto.Width, objPhoto.Height)
                    Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

                    If (PropertySettings.HighQuality) Then
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic
                        g.SmoothingMode = SmoothingMode.HighQuality
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality
                        g.CompositingQuality = CompositingQuality.HighQuality
                    End If

                    g.DrawImage(photo, 0, 0, objPhoto.Width, objPhoto.Height)

                    If (PropertySettings.WatermarkEnabled And PropertySettings.WatermarkText <> "") Then
                        Dim crSize As SizeF = New SizeF
                        Dim brushColor As Brush = Brushes.Yellow
                        Dim fnt As Font = New Font("Verdana", 11, FontStyle.Bold)
                        Dim strDirection As StringFormat = New StringFormat

                        strDirection.Alignment = StringAlignment.Center
                        crSize = g.MeasureString(PropertySettings.WatermarkText, fnt)

                        Dim yPixelsFromBottom As Integer = Convert.ToInt32(Convert.ToDouble(objPhoto.Height) * 0.05)
                        Dim yPosFromBottom As Single = Convert.ToSingle((objPhoto.Height - yPixelsFromBottom) - (crSize.Height / 2))
                        Dim xCenterOfImage As Single = Convert.ToSingle((objPhoto.Width / 2))

                        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

                        Dim semiTransBrush2 As SolidBrush = New SolidBrush(Color.FromArgb(153, 0, 0, 0))
                        g.DrawString(PropertySettings.WatermarkText, fnt, semiTransBrush2, New PointF(xCenterOfImage + 1, yPosFromBottom + 1), strDirection)

                        Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(153, 255, 255, 255))
                        g.DrawString(PropertySettings.WatermarkText, fnt, semiTransBrush, New PointF(xCenterOfImage, yPosFromBottom), strDirection)
                    End If

                    If (PropertySettings.WatermarkEnabled And PropertySettings.WatermarkImage <> "") Then
                        Dim watermark As String = PortalSettings.HomeDirectoryMapPath & PropertySettings.WatermarkImage
                        If (File.Exists(watermark)) Then
                            Dim imgWatermark As Image = New Bitmap(watermark)
                            Dim wmWidth As Integer = imgWatermark.Width
                            Dim wmHeight As Integer = imgWatermark.Height

                            Dim objImageAttributes As New ImageAttributes()
                            Dim objColorMap As New ColorMap()
                            objColorMap.OldColor = Color.FromArgb(255, 0, 255, 0)
                            objColorMap.NewColor = Color.FromArgb(0, 0, 0, 0)
                            Dim remapTable As ColorMap() = {objColorMap}
                            objImageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)

                            Dim colorMatrixElements As Single()() = {New Single() {1.0F, 0.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 1.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 1.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.3F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.0F, 1.0F}}
                            Dim wmColorMatrix As New ColorMatrix(colorMatrixElements)
                            objImageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)

                            Dim xPosOfWm As Integer = ((objPhoto.Width - wmWidth) - 10)
                            Dim yPosOfWm As Integer = 10

                            Select Case PropertySettings.WatermarkPosition
                                Case WatermarkPosition.TopLeft
                                    xPosOfWm = 10
                                    yPosOfWm = 10
                                    Exit Select

                                Case WatermarkPosition.TopRight
                                    xPosOfWm = ((objPhoto.Width - wmWidth) - 10)
                                    yPosOfWm = 10
                                    Exit Select

                                Case WatermarkPosition.BottomLeft
                                    xPosOfWm = 10
                                    yPosOfWm = ((objPhoto.Height - wmHeight) - 10)

                                Case WatermarkPosition.BottomRight
                                    xPosOfWm = ((objPhoto.Width - wmWidth) - 10)
                                    yPosOfWm = ((objPhoto.Height - wmHeight) - 10)
                            End Select

                            g.DrawImage(imgWatermark, New Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight, _
                             GraphicsUnit.Pixel, objImageAttributes)
                            imgWatermark.Dispose()
                        End If
                    End If

                    photo.Dispose()

                    If (PropertySettings.HighQuality) Then

                        Select Case fileType.ToLower()
                            Case "image/jpeg"
                                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                                Dim encoderParameters As New EncoderParameters(1)
                                encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                                bmp.Save(filePath & objPhoto.Filename, info(1), encoderParameters)

                            Case "image/gif"
                                'Dim quantizer As New ImageQuantization.OctreeQuantizer(255, 8)
                                'Dim bmpQuantized As Bitmap = quantizer.Quantize(bmp)
                                'bmpQuantized.Save(filePath & objPhoto.Filename, ImageFormat.Gif)
                                ' Not working in medium trust.
                                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Gif)

                            Case Else
                                'Shouldn't get to here because of validators.                                
                                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                                Dim encoderParameters As New EncoderParameters(1)
                                encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                                bmp.Save(filePath & objPhoto.Filename, info(1), encoderParameters)
                        End Select

                    Else

                        Select Case fileType.ToLower()
                            Case "image/jpeg"
                                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Jpeg)

                            Case "image/gif"
                                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Gif)

                            Case Else
                                'Shouldn't get to here because of validators.
                                bmp.Save(filePath & objPhoto.Filename, ImageFormat.Jpeg)
                        End Select

                    End If

                    bmp.Dispose()
                End If

                objPhotoController.Update(objPhoto)

                txtTitle.Text = ""
                If (TypeOf Parent Is EditPhotos) Then
                    CType(Parent, EditPhotos).RefreshPhotos()
                Else
                    CType(Parent.Parent, EditProperty).RefreshPhotos()
                End If


            End If

        End Sub

        Private Sub valType_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valType.ServerValidate

            Try

                If Not (txtPhoto.PostedFile Is Nothing) Then
                    Dim fileType As String = Path.GetExtension(txtPhoto.PostedFile.FileName).ToLower()
                    If (fileType = ".jpg" OrElse fileType = ".jpeg" OrElse fileType = ".gif") Then
                        args.IsValid = True
                        Return
                    End If
                End If
                args.IsValid = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valPhoto_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valPhoto.ServerValidate

            Try

                If Not (txtPhoto.PostedFile Is Nothing) Then
                    If (txtPhoto.PostedFile.ContentLength > 0) Then
                        args.IsValid = True
                        Return
                    End If
                End If
                args.IsValid = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub valLimitExceeded_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valLimitExceeded.ServerValidate

            Try

                Dim limit As Integer = PropertyAgentBase.GetLimit(PropertySettings.PermissionAddImages, PropertySettings.PermissionAddImagesLimit)

                If (limit = Null.NullInteger) Then
                    args.IsValid = True
                    Return
                End If

                If (Photos.Count >= limit) Then
                    args.IsValid = False
                    Return
                Else
                    args.IsValid = True
                    Return
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
