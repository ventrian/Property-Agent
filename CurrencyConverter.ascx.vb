Imports DotNetNuke.Services.Localization
Imports System.Xml
Imports System.Net
Imports System.IO
Imports DotNetNuke.Common.Utilities
Imports System.Globalization

Namespace Ventrian.PropertyAgent

    Partial Public Class CurrencyConverter
        Inherits PropertyAgentControl

        Private _fieldValue As String = ""
        Public Property FieldValue() As String
            Get
                Return _fieldValue
            End Get
            Set(ByVal value As String)
                _fieldValue = value
            End Set
        End Property

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/EditLayoutSettings"
            End Get
        End Property

        Private Sub BindCurrency()

            For Each value As Integer In System.Enum.GetValues(GetType(CurrencyType))
                Dim li As New ListItem
                li.Value = ConvertCurrency(System.Enum.GetName(GetType(CurrencyType), value), FieldValue)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CurrencyType), value), ResourceFile)
                drpCurrency.Items.Add(li)
            Next

            drpCurrency.Items.Insert(0, New ListItem("-- Select Currency --", "-1"))

        End Sub

        Private Function ConvertCurrency(ByVal currencyConvertTo As String, ByVal value As String)

            Dim culture As String = "en-US"

            Select Case System.Enum.Parse(GetType(CurrencyType), currencyConvertTo)

                Case CurrencyType.AUD
                    culture = "en-AU"
                    Exit Select

                Case CurrencyType.BRL
                    culture = "pt-BR"
                    Exit Select

                Case CurrencyType.CAD
                    culture = "en-CA"
                    Exit Select

                Case CurrencyType.CHF
                    culture = "de-CH"
                    Exit Select

                Case CurrencyType.CNY
                    culture = "zh-CN"
                    Exit Select

                Case CurrencyType.CRC
                    culture = "es-CR"
                    Exit Select

                Case CurrencyType.CZK
                    culture = "cs-CZ"
                    Exit Select

                Case CurrencyType.DKK
                    culture = "da-DK"
                    Exit Select

                Case CurrencyType.EUR
                    culture = "fr-FR"
                    Select Case PropertySettings.EuroType
                        Case EuroType.Dutch
                            culture = "nl-NL"
                            Exit Select

                        Case EuroType.English
                            culture = "en-IE"
                            Exit Select

                        Case EuroType.French
                            culture = "fr-FR"
                            Exit Select
                    End Select
                    Exit Select

                Case CurrencyType.GBP
                    culture = "en-GB"
                    Exit Select

                Case CurrencyType.JPY
                    culture = "ja-JP"
                    Exit Select

                Case CurrencyType.USD
                    culture = "en-US"
                    Exit Select

                Case CurrencyType.MYR
                    culture = "en-MY"
                    Exit Select

                Case CurrencyType.NZD
                    culture = "en-NZ"
                    Exit Select

                Case CurrencyType.NOK
                    culture = "nb-NO"
                    Exit Select

                Case CurrencyType.THB
                    culture = "th-TH"
                    Exit Select

                Case CurrencyType.ZAR
                    culture = "en-ZA"
                    Exit Select

            End Select

            Dim userLocale As String = PropertySettings.Currency.ToString()

            If (currencyConvertTo <> PropertySettings.Currency.ToString()) Then

                Dim rate As Double = Null.NullDouble

                Dim uri As New Uri("http://www.themoneyconverter.com/rss-feed/" & PropertySettings.Currency.ToString() & "/rss.xml")
                If (uri.Scheme = uri.UriSchemeHttp) Then

                    Dim objXml As XmlDocument = CType(DataCache.GetCache("PA-" & ModuleID.ToString() & "-RSS"), XmlDocument)
                    If (objXml Is Nothing) Then
                        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
                        request.Method = WebRequestMethods.Http.Get
                        Dim response As HttpWebResponse = request.GetResponse()
                        Dim reader As New StreamReader(response.GetResponseStream())
                        Dim tmp As String = reader.ReadToEnd()
                        response.Close()
                        objXml = New XmlDocument()
                        objXml.LoadXml(tmp)
                        DataCache.SetCache("PA-" & ModuleID.ToString() & "-RSS", objXml, DateTime.Now.AddMinutes(15))
                    End If

                    Dim rssItems As XmlNodeList = objXml.SelectNodes("rss/channel/item")
                    For Each objNode As XmlNode In rssItems
                        Dim objTitle As XmlNode = objNode.SelectSingleNode("title")
                        If (objTitle IsNot Nothing) Then
                            If (objTitle.InnerText.Contains(currencyConvertTo)) Then
                                Dim objDescription As XmlNode = objNode.SelectSingleNode("description")
                                Dim arr1() As String = objDescription.InnerText.Split("="c)
                                If (arr1.Length = 2) Then
                                    Dim val As String = arr1(1).Trim()
                                    Dim arr2() As String = val.Split(" "c)
                                    If (arr2.Length > 1) Then
                                        rate = Convert.ToDouble(Double.Parse(arr2(0), CultureInfo.InvariantCulture.NumberFormat))
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If

                If (rate <> Null.NullDouble) Then
                    value = value * rate
                End If

            End If

            Dim portalFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
            Dim format As String = "{0:C" & PropertySettings.CurrencyDecimalPlaces.ToString() & "}"
            value = String.Format(portalFormat.NumberFormat, format, Double.Parse(value))

            Return value

        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (IsPostBack = False) Then
                BindCurrency()
            End If

        End Sub

    End Class

End Namespace
