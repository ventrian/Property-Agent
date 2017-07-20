Imports DotNetNuke.Common.Utilities

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Web
Imports System.Net
Imports System.IO
Imports Ventrian.PropertyAgent

Public Class ImageHandlerExt
    Implements System.Web.IHttpHandler

#Region " Private Members "

    Private _width As Integer = 100
    Private _height As Integer = 100
    Private _photoID As String = Null.NullInteger
    Private _imageType As String = ""
    Private _quality As Boolean = Null.NullBoolean

#End Region

#Region " Private Methods "

    Public Function GetImageByUrl(ByVal url As String) As System.Drawing.Image

        Dim response As WebResponse = Nothing
        Dim remoteStream As Stream = Nothing
        Dim readStream As StreamReader = Nothing
        Try
            Dim request As WebRequest = WebRequest.Create(DotNetNuke.Common.AddHTTP(url))
            If request IsNot Nothing Then
                response = request.GetResponse()
                If response IsNot Nothing Then
                    remoteStream = response.GetResponseStream()

                    Dim content_type As String = response.Headers("Content-type")

                    Dim imageType As String = ""

                    If content_type = "image/jpeg" OrElse content_type = "image/jpg" Then
                        _imageType = "image/jpeg"
                    ElseIf content_type = "image/gif" Then
                        _imageType = "image/gif"
                    Else
                        Return Nothing
                    End If

                    readStream = New StreamReader(remoteStream)

                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(remoteStream)

                    If img Is Nothing Then
                        Return Nothing
                    End If

                    ' img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg)
                    Return img
                End If
            End If
        Finally
            If response IsNot Nothing Then
                response.Close()
            End If
            If remoteStream IsNot Nothing Then
                remoteStream.Close()
            End If
            If readStream IsNot Nothing Then
                readStream.Close()
            End If
        End Try

        Return Nothing

    End Function

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

        If Not (context.Request("i") Is Nothing) Then
            Try
                _photoID = context.Server.UrlDecode(context.Request("i"))
            Catch
            End Try
        End If

        If Not (context.Request("Q") Is Nothing) Then
            Try
                _quality = Convert.ToBoolean(context.Request("Q"))
            Catch
            End Try
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
            context.Response.Cache.VaryByParams("i") = True
            context.Response.Cache.VaryByParams("Width") = True
            context.Response.Cache.VaryByParams("Height") = True
            context.Response.Cache.VaryByParams("Q") = True

            ' Temporary fix for DNN 4.4.0
            context.Items.Add("httpcompress.attemptedinstall", "true")

            ReadQueryString(context)

            Dim objPhotoController As New PhotoController()
            Dim objPhotoItem As PhotoInfo = objPhotoController.Get(_photoID)

            If (objPhotoItem IsNot Nothing) Then

                If (objPhotoItem.PhotoType = PhotoType.External) Then

                    Dim photo As Drawing.Image = GetImageByUrl(objPhotoItem.ExternalUrl)

                    Dim bmp As New Bitmap(_width, _height)
                    Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.SmoothingMode = SmoothingMode.HighQuality
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality
                    g.CompositingQuality = CompositingQuality.HighQuality

                    g.DrawImage(photo, 0, 0, _width, _height)
                    photo.Dispose()

                    If (_quality) Then

                        Select Case _imageType.ToLower()
                            Case "image/jpeg"
                                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                                Dim encoderParameters As New EncoderParameters(1)
                                encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                                bmp.Save(context.Response.OutputStream, info(1), encoderParameters)

                            Case "image/gif"
                                'Dim quantizer As New ImageQuantization.OctreeQuantizer(255, 8)
                                'Dim bmpQuantized As Bitmap = quantizer.Quantize(bmp)
                                'bmpQuantized.Save(filePath & objPhoto.Filename, ImageFormat.Gif)
                                ' Not working in medium trust.
                                bmp.Save(context.Response.OutputStream, ImageFormat.Gif)

                            Case Else
                                'Shouldn't get to here because of validators.                                
                                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                                Dim encoderParameters As New EncoderParameters(1)
                                encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                                bmp.Save(context.Response.OutputStream, info(1), encoderParameters)
                        End Select

                    Else

                        Select Case _imageType.ToLower()
                            Case "image/jpeg"
                                bmp.Save(context.Response.OutputStream, ImageFormat.Jpeg)

                            Case "image/gif"
                                bmp.Save(context.Response.OutputStream, ImageFormat.Gif)

                            Case Else
                                'Shouldn't get to here because of validators.
                                bmp.Save(context.Response.OutputStream, ImageFormat.Jpeg)
                        End Select

                    End If

                    bmp.Dispose()

                End If

            End If

        Catch
        End Try

    End Sub

#End Region


End Class