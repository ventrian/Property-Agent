Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports ExpertPdf.HtmlToPdf
Imports System.IO

Namespace Ventrian.PropertyAgent

    Partial Public Class PdfRender
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _propertyID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Function GetPdfConverter() As PdfConverter

            Dim objPdfConverter As New PdfConverter()

            objPdfConverter.LicenseKey = e.a

            objPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4
            objPdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait
            objPdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.NoCompression

            objPdfConverter.PdfDocumentOptions.LeftMargin = 20
            objPdfConverter.PdfDocumentOptions.RightMargin = 20
            objPdfConverter.PdfDocumentOptions.BottomMargin = 20
            objPdfConverter.PdfDocumentOptions.TopMargin = 20

            Return objPdfConverter

        End Function

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "ISMOBILEDEVICE"
                            If LayoutController.IsMobileBrowser() = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISMOBILEDEVICE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISMOBILEDEVICE"
                            ' Do Nothing

                        Case "ISNOTMOBILEDEVICE"
                            If LayoutController.IsMobileBrowser() = True Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTMOBILEDEVICE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTMOBILEDEVICE"
                            ' Do Nothing

                        Case "ISOWNER"
                            Dim isOwner As Boolean = False
                            If (Me.Page.User.Identity.IsAuthenticated) Then
                                Dim objPropertyController As New PropertyController
                                Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)
                                If Not (objProperty Is Nothing) Then
                                    If (UserController.GetCurrentUserInfo.UserID = objProperty.AuthorID) Then
                                        isOwner = True
                                    End If
                                End If
                            End If

                            If isOwner = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISOWNER") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISOWNER"
                            ' Do Nothing

                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = PropertySettings.PropertyLabel
                            objPlaceHolder.Add(objLiteral)

                    End Select

                End If

            Next

        End Sub

        Private Sub ReadQueryString()

            Dim propertyIDParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyIDParam) = "") Then
                propertyIDParam = "PropertyID"
            End If
            If Not (Request(propertyIDParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyIDParam))
            End If

        End Sub

        Private Function RenderControlAsString(ByVal objControl As Control) As String

            Dim sb As New StringBuilder
            Dim tw As New StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            objControl.RenderControl(hw)

            Return sb.ToString()

        End Function


#End Region

#Region " Event Handlers "

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ReadQueryString()

            If (_propertyID <> Null.NullInteger) Then

                Dim objPropertyController As New PropertyController()
                Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)

                If (objProperty IsNot Nothing) Then
                    Dim objPdfConverter As PdfConverter = GetPdfConverter()

                    Dim baseUrl As String = AddHTTP(Request.Url.Host)

                    Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)

                    Dim objLayoutHeader As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Pdf_Header_Html)
                    Dim objLayoutItem As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Pdf_Item_Html)
                    Dim objLayoutFooter As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Pdf_Footer_Html)

                    Dim objPlaceHolder As New PlaceHolder
                    ProcessHeaderFooter(objPlaceHolder.Controls, objLayoutHeader.Tokens)
                    objLayoutController.ProcessItem(objPlaceHolder.Controls, objLayoutItem.Tokens, objProperty, CustomFields)
                    ProcessHeaderFooter(objPlaceHolder.Controls, objLayoutFooter.Tokens)

                    Dim content As String = RenderControlAsString(objPlaceHolder)
                    content = content & "<link rel=""stylesheet"" type=""text/css"" href=""" & PortalSettings.HomeDirectory & "PropertyAgent/" & ModuleId.ToString() & "/Templates/" & Me.PropertySettings.Template & "/Template.css" & """ />"

                    Dim objBytes As Byte() = objPdfConverter.GetPdfBytesFromHtmlString(content, baseUrl)

                    Response.Clear()
                    Response.AddHeader("Accept-Header", objBytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.AddHeader("content-disposition", "attachment; filename=PDF-" + _propertyID.ToString())
                    Response.OutputStream.Write(objBytes, 0, objBytes.Length)
                    Response.Flush()
                    Response.End()
                End If

            End If


        End Sub

#End Region

    End Class

End Namespace