Imports DotNetNuke.Common.Utilities

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Web

Imports Ventrian.ImageResizer
Imports System.IO

Public Class ImageHandler
    Implements System.Web.IHttpHandler

#Region " Private Members "

    Private _width As Integer = 100
    Private _height As Integer = 100
    Private _homeDirectory As String = Null.NullString
    Private _fileName As String = Null.NullString
    Private _quality As Boolean = Null.NullBoolean
    Private _cropped As Boolean = False

#End Region

#Region " Private Methods "

    Private Sub ReadQueryString(ByVal context As HttpContext)

        If Not (context.Request("Width") Is Nothing) Then
            If (IsNumeric(context.Request("Width"))) Then
                _width = Convert.ToInt32(context.Request("Width"))
            End If
        End If

        If Not (context.Request("Height") Is Nothing) Then
            If (IsNumeric(context.Request("Height"))) Then
                _height = Convert.ToInt32(context.Request("Height"))
            End If
        End If

        If Not (context.Request("HomeDirectory") Is Nothing) Then
            _homeDirectory = context.Server.UrlDecode(context.Request("HomeDirectory"))
        End If

        If Not (context.Request("FileName") Is Nothing) Then
            _fileName = context.Server.UrlDecode(context.Request("FileName"))
        End If

        If Not (context.Request("S") Is Nothing) Then
            If (context.Request("S") = "1") Then
                _cropped = True
            End If
        End If

    End Sub

#End Region

#Region " Properties "

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

#End Region

#Region " Event Handlers "

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Try

            ' Set up the response settings
            context.Response.ContentType = "image/jpeg"

            ' Caching 
            context.Response.Cache.SetCacheability(HttpCacheability.Public)
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(1))
            context.Response.Cache.VaryByParams("FileName") = True
            context.Response.Cache.VaryByParams("HomeDirectory") = True
            context.Response.Cache.VaryByParams("Width") = True
            context.Response.Cache.VaryByParams("Height") = True

            ' Temporary fix for DNN 4.4.0
            context.Items.Add("httpcompress.attemptedinstall", "true")

            ReadQueryString(context)

            Dim path As String = ""
            If _fileName = "placeholder-600.jpg" Then
                path = "Images/placeholder-600.jpg"
            Else
                path = _homeDirectory & "/" & _fileName
            End If

            Dim mapPath As String = context.Server.MapPath(path)

            Dim objQueryString As New NameValueCollection()

            For Each key As String In context.Request.QueryString.Keys
                Dim values() As String = context.Request.QueryString.GetValues(key)
                For Each value As String In values
                    objQueryString.Add(key.Replace("width", "maxwidth").Replace("height", "maxheight"), value)
                    If (key = "width" Or key = "height") Then
                        objQueryString.Add(key, value)
                    End If
                Next
            Next

            If (_cropped) Then
                objQueryString.Add("crop", "auto")
            End If

            'Dim objImage As Bitmap = ImageManager.getBestInstance().BuildImage(mapPath, objQueryString, New WatermarkSettings(objQueryString))
            'Dim ios As ImageOutputSettings = New ImageOutputSettings(ImageOutputSettings.GetImageFormatFromPhysicalPath(mapPath), objQueryString)
            'ios.SaveImage(context.Response.OutputStream, objImage)

            Dim objImage As Bitmap = ImageManager.getBestInstance().BuildImage(context.Server.MapPath(path), objQueryString, New WatermarkSettings(objQueryString))
            If (path.ToLower().EndsWith("jpg")) Then
                objImage.Save(context.Response.OutputStream, ImageFormat.Jpeg)
            Else
                If (path.ToLower().EndsWith("gif")) Then
                    context.Response.ContentType = "image/gif"
                    Dim ios As ImageOutputSettings = New ImageOutputSettings(ImageOutputSettings.GetImageFormatFromPhysicalPath(context.Server.MapPath(path)), objQueryString)
                    ios.SaveImage(context.Response.OutputStream, objImage)
                Else
                    If (path.ToLower().EndsWith("png")) Then
                        Dim objMemoryStream As New MemoryStream()
                        context.Response.ContentType = "image/png"
                        objImage.Save(objMemoryStream, ImageFormat.Png)
                        objMemoryStream.WriteTo(context.Response.OutputStream)
                    Else
                        objImage.Save(context.Response.OutputStream, ImageFormat.Jpeg)
                    End If
                End If
            End If


            'Dim photo As Drawing.Image = Drawing.Image.FromFile(mapPath)
            'Dim extension As String = System.IO.Path.GetExtension(mapPath)

            'Dim bmp As New Bitmap(_width, _height)
            'Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

            ''If (_quality) Then
            'g.InterpolationMode = InterpolationMode.HighQualityBicubic
            'g.SmoothingMode = SmoothingMode.HighQuality
            'g.PixelOffsetMode = PixelOffsetMode.HighQuality
            'g.CompositingQuality = CompositingQuality.HighQuality
            ''End If

            'g.DrawImage(photo, 0, 0, _width, _height)
            'photo.Dispose()

            'If (_quality) Then

            '    Select Case extension.ToLower()
            '        Case ".jpeg"
            '            Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
            '            Dim encoderParameters As New EncoderParameters(1)
            '            encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
            '            bmp.Save(context.Response.OutputStream, info(1), encoderParameters)

            '        Case ".gif"
            '            'Dim quantizer As New ImageQuantization.OctreeQuantizer(255, 8)
            '            'Dim bmpQuantized As Bitmap = quantizer.Quantize(bmp)
            '            'bmpQuantized.Save(filePath & objPhoto.Filename, ImageFormat.Gif)
            '            ' Not working in medium trust.
            '            bmp.Save(context.Response.OutputStream, ImageFormat.Gif)

            '        Case Else
            '            'Shouldn't get to here because of validators.                                
            '            Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
            '            Dim encoderParameters As New EncoderParameters(1)
            '            encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
            '            bmp.Save(context.Response.OutputStream, info(1), encoderParameters)
            '    End Select

            'Else

            '    Select Case extension.ToLower()
            '        Case ".jpeg"
            '            bmp.Save(context.Response.OutputStream, ImageFormat.Jpeg)

            '        Case ".gif"
            '            bmp.Save(context.Response.OutputStream, ImageFormat.Gif)

            '        Case Else
            '            'Shouldn't get to here because of validators.
            '            bmp.Save(context.Response.OutputStream, ImageFormat.Jpeg)
            '    End Select

            'End If

            'bmp.Dispose()

        Catch
        End Try

    End Sub

#End Region


End Class