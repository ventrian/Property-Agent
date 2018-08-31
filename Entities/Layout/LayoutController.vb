Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.Caching

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Common.Lists
Imports DotNetNuke.Security
Imports System.Net
Imports System.Xml
Imports DotNetNuke.Entities.Tabs
Imports System.Globalization
Imports System.Xml.XPath
Imports DotNetNuke.Web.Client.ClientResourceManagement

Namespace Ventrian.PropertyAgent

    Public Class LayoutController

#Region " Constructors "

        Public Sub New(ByVal portalSettings As PortalSettings, ByVal propertySettings As PropertySettings, ByVal objPage As Web.UI.Page, ByVal objControl As Control, ByVal isEditable As Boolean, ByVal tabID As Integer, ByVal moduleID As Integer, ByVal moduleKey As String)
            _portalSettings = portalSettings
            _propertySettings = propertySettings
            _objPage = objPage
            _objControl = objControl
            _isEditable = isEditable
            _tabID = tabID
            _moduleID = moduleID
            _portalID = portalSettings.PortalId
            _moduleKey = moduleKey
            _listingIndex = 1
        End Sub

#End Region

#Region " Private Members "

        Private _agent As UserInfo
        Private _modified As UserInfo
        Private _broker As UserInfo

        Private _objLayoutPhotoItem As LayoutInfo
        Private _propertySettings As PropertySettings
        Private _portalSettings As PortalSettings

        Private _objPage As System.Web.UI.Page
        Private _objControl As Control

        Private _isEditable As Boolean
        Private _moduleID As Integer
        Private _tabID As Integer
        Private _portalID As Integer
        Private _moduleKey As String
        Private _listingIndex As Integer

        Private _siblingSearchProperties As List(Of PropertyInfo)
        Private _siblingTypeProperties As List(Of PropertyInfo)
        Private _siblingSearchCount As Integer = Null.NullInteger
        Private _siblingTypeCount As Integer = Null.NullInteger
        Private _searchPosition As Integer = Null.NullInteger
        Private _typePosition As Integer = Null.NullInteger

        Private _profileProperties As ProfilePropertyDefinitionCollection

#End Region

#Region " Public Properties "

        Public Property ListingIndex() As Integer
            Get
                Return _listingIndex
            End Get
            Set(ByVal value As Integer)
                _listingIndex = value
            End Set
        End Property

        Public Property ModuleKey() As String
            Get
                Return _moduleKey
            End Get
            Set(ByVal value As String)
                _moduleKey = value
            End Set
        End Property

#End Region

#Region " Private Properties "

        Private ReadOnly Property NextSearchProperty(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As PropertyInfo
            Get
                Dim objProperties As List(Of PropertyInfo) = SiblingSearchProperties(propertyID, propertyTypeID)

                If (objProperties.Count = 3) Then
                    Return objProperties(2)
                End If

                If (objProperties.Count = 2) Then
                    If (propertyID = objProperties(0).PropertyID) Then
                        Return objProperties(1)
                    End If
                End If

                Return Nothing
            End Get
        End Property

        Private ReadOnly Property NextTypeProperty(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As PropertyInfo
            Get
                Dim objProperties As List(Of PropertyInfo) = SiblingTypeProperties(propertyID, propertyTypeID)

                If (objProperties.Count = 3) Then
                    Return objProperties(2)
                End If

                If (objProperties.Count = 2) Then
                    If (propertyID = objProperties(0).PropertyID) Then
                        Return objProperties(1)
                    End If
                End If

                Return Nothing
            End Get
        End Property

        Private ReadOnly Property PreviousSearchProperty(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As PropertyInfo
            Get
                Dim objProperties As List(Of PropertyInfo) = SiblingSearchProperties(propertyID, propertyTypeID)

                If (objProperties.Count = 3) Then
                    Return objProperties(0)
                End If

                If (objProperties.Count = 2) Then
                    If (propertyID = objProperties(1).PropertyID) Then
                        Return objProperties(0)
                    End If
                End If

                Return Nothing
            End Get
        End Property

        Private ReadOnly Property PreviousTypeProperty(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As PropertyInfo
            Get
                Dim objProperties As List(Of PropertyInfo) = SiblingTypeProperties(propertyID, propertyTypeID)

                If (objProperties.Count = 3) Then
                    Return objProperties(0)
                End If

                If (objProperties.Count = 2) Then
                    If (propertyID = objProperties(1).PropertyID) Then
                        Return objProperties(0)
                    End If
                End If

                Return Nothing
            End Get
        End Property

        Private ReadOnly Property ProfileProperties() As ProfilePropertyDefinitionCollection
            Get
                If (_profileProperties Is Nothing) Then
                    _profileProperties = ProfileController.GetPropertyDefinitionsByPortal(_portalID)
                End If
                Return _profileProperties
            End Get
        End Property

        Private ReadOnly Property SearchPosition(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As Integer
            Get
                Dim objTempProperties As List(Of PropertyInfo) = SiblingSearchProperties(propertyID, propertyTypeID)
                Return _searchPosition
            End Get
        End Property

        Private ReadOnly Property SiblingSearchCount(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As Integer
            Get
                Dim objTempProperties As List(Of PropertyInfo) = SiblingSearchProperties(propertyID, propertyTypeID)
                Return _siblingSearchCount
            End Get
        End Property

        Private ReadOnly Property SiblingSearchProperties(ByVal propertyID As Integer, ByVal currentPropertyTypeID As Integer) As List(Of PropertyInfo)
            Get
                If (_siblingSearchProperties Is Nothing) Then

                    Dim objRequest As HttpRequest = HttpContext.Current.Request

                    Dim customFieldIDs As String = ""
                    Dim searchValues As String = ""
                    Dim propertyBrokerID As Integer = Null.NullInteger
                    Dim propertyAgentID As Integer = Null.NullInteger
                    Dim propertyTypeID As Integer = Null.NullInteger
                    Dim _sortBy As String = ""
                    Dim _sortDirection As String = ""

                    If (objRequest("customFieldIDs") <> "") Then
                        customFieldIDs = objRequest("customFieldIDs")
                    End If

                    If (objRequest("SearchValues") <> "") Then
                        searchValues = objRequest("SearchValues")
                    End If

                    If (objRequest("PropertyBrokerID") <> "") Then
                        propertyBrokerID = Convert.ToInt32(objRequest("PropertyBrokerID"))
                    End If

                    If (objRequest("PropertyAgentID") <> "") Then
                        propertyAgentID = Convert.ToInt32(objRequest("PropertyAgentID"))
                    End If

                    If (objRequest("PropertyTypeID") <> "") Then
                        propertyTypeID = Convert.ToInt32(objRequest("PropertyTypeID"))
                    End If

                    If Not (objRequest("sortBy") Is Nothing) Then
                        _sortBy = objRequest("sortBy").Trim()
                    End If

                    If Not (objRequest("sortDir") Is Nothing) Then
                        _sortDirection = objRequest("sortDir").Trim()
                    End If

                    If (customFieldIDs = Null.NullString And searchValues = Null.NullString And propertyBrokerID = Null.NullInteger And propertyAgentID = Null.NullInteger And propertyTypeID = Null.NullInteger) Then
                        propertyTypeID = currentPropertyTypeID
                    End If

                    Dim objSortBy As SortByType = _propertySettings.ListingSortBy
                    Dim sortCustomFieldID As Integer = _propertySettings.ListingSortByCustomField
                    If (_sortBy <> "") Then
                        If (_sortBy.StartsWith("cf")) Then
                            objSortBy = SortByType.CustomField
                            sortCustomFieldID = Convert.ToInt32(_sortBy.Replace("cf", ""))
                        Else
                            objSortBy = CType(System.Enum.Parse(GetType(SortByType), _sortBy, True), SortByType)
                        End If
                    End If

                    Dim objSortDirection As SortDirection = _propertySettings.ListingSortDirection
                    If (_sortDirection <> "") Then
                        objSortDirection = CType(System.Enum.Parse(GetType(SortDirectionType), _sortDirection, True), SortDirectionType)
                    End If

                    Dim objPropertyController As New PropertyController
                    _siblingSearchProperties = objPropertyController.List(_moduleID, propertyTypeID, SearchStatusType.PublishedActive, propertyAgentID, propertyBrokerID, Null.NullBoolean, Null.NullBoolean, objSortBy, sortCustomFieldID, objSortDirection, customFieldIDs, searchValues, 1, 3, _siblingSearchCount, _propertySettings.ListingBubbleFeatured, _propertySettings.ListingSearchSubTypes, propertyID, _searchPosition)

                End If
                Return _siblingSearchProperties
            End Get
        End Property

        Private ReadOnly Property SiblingTypeCount(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As Integer
            Get
                Dim objTempProperties As List(Of PropertyInfo) = SiblingTypeProperties(propertyID, propertyTypeID)
                Return _siblingTypeCount
            End Get
        End Property

        Private ReadOnly Property SiblingTypeProperties(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As List(Of PropertyInfo)
            Get
                If (_siblingTypeProperties Is Nothing) Then

                    Dim objPropertyController As New PropertyController
                    _siblingTypeProperties = objPropertyController.List(_moduleID, propertyTypeID, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, Null.NullBoolean, Null.NullBoolean, _propertySettings.ListingSortBy, _propertySettings.ListingSortByCustomField, _propertySettings.ListingSortDirection, Null.NullString, Null.NullString, 1, 3, _siblingTypeCount, False, False, propertyID, _typePosition)

                End If
                Return _siblingTypeProperties
            End Get
        End Property

        Private ReadOnly Property TypePosition(ByVal propertyID As Integer, ByVal propertyTypeID As Integer) As Integer
            Get
                Dim objTempProperties As List(Of PropertyInfo) = SiblingTypeProperties(propertyID, propertyTypeID)
                Return _typePosition
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Function OnlyAlphaNumericChars(ByVal OrigString As String) As String

            '***********************************************************
            'INPUT:  Any String
            'OUTPUT: The Input String with all non-alphanumeric characters 
            '        removed
            'EXAMPLE Debug.Print OnlyAlphaNumericChars("Hello World!")
            'output = "HelloWorld")
            'NOTES:  Not optimized for speed and will run slow on long
            '        strings.  If you plan on using long strings, consider 
            '        using alternative method of appending to output string,
            '        such as the method at
            '        http://www.freevbcode.com/ShowCode.Asp?ID=154
            '***********************************************************
            Dim lLen As Integer
            Dim sAns As String = ""
            Dim lCtr As Integer
            Dim sChar As String

            OrigString = RemoveDiacritics(Trim(OrigString))

            lLen = Len(OrigString)
            For lCtr = 1 To lLen
                sChar = Mid(OrigString, lCtr, 1)
                If IsAlphaNumeric(Mid(OrigString, lCtr, 1)) Or Mid(OrigString, lCtr, 1) = "-" Or Mid(OrigString, lCtr, 1) = "_" Then
                    sAns = sAns & sChar
                End If
            Next

            If (_propertySettings.SEOTitleReplacement = TitleReplacementType.Dash) Then
                OnlyAlphaNumericChars = Replace(sAns, " ", "-")
            Else
                OnlyAlphaNumericChars = Replace(sAns, " ", "_")
            End If

        End Function

        Private Function RemoveDiacritics(ByVal s As String) As String
            s = s.Normalize(System.Text.NormalizationForm.FormD)
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            Dim i As Integer
            For i = 0 To s.Length - 1
                If s(i) = ChrW(305) Then
                    sb.Append("i"c)
                Else
                    If CharUnicodeInfo.GetUnicodeCategory(s(i)) <> UnicodeCategory.NonSpacingMark Then
                        sb.Append(s(i))
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        Private Function IsAlphaNumeric(ByVal sChr As String) As Boolean
            IsAlphaNumeric = sChr Like "[0-9A-Za-z ]"
        End Function

        Public Function GetExternalLink(ByVal link As String) As String
            If (link.ToLower().StartsWith("http://") Or link.ToLower().StartsWith("https://")) Then
                Return link
            End If

            If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & link)
            Else
                Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & link)
            End If
        End Function

        Private Function RenderControlToString(ByVal ctrl As Control) As String

            Dim sb As New StringBuilder()
            Dim sw As New StringWriter(sb)
            Dim hw As New HtmlTextWriter(sw)

            ctrl.RenderControl(hw)
            Return sb.ToString()

        End Function

        Public Function GetPropertyLink(ByVal objProperty As PropertyInfo, ByVal objCustomFields As List(Of CustomFieldInfo)) As String

            Dim params As New List(Of String)

            params.Add(_propertySettings.SEOAgentType & "=View")
            params.Add(_propertySettings.SEOPropertyID & "=" & objProperty.PropertyID.ToString())

            If (_propertySettings.ListingPassSearchValues) Then

                If (HttpContext.Current.Request("customFieldIDs") <> "") Then
                    params.Add("customFieldIDs=" & HttpContext.Current.Request("customFieldIDs"))
                End If

                If (HttpContext.Current.Request("SearchValues") <> "") Then
                    params.Add("SearchValues=" & HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request("SearchValues")).Replace("%2c", ","))
                End If

                If (HttpContext.Current.Request("PropertyBrokerID") <> "") Then
                    params.Add("PropertyBrokerID=" & HttpContext.Current.Request("PropertyBrokerID"))
                End If

                If (HttpContext.Current.Request("PropertyAgentID") <> "") Then
                    params.Add("PropertyAgentID=" & HttpContext.Current.Request("PropertyAgentID"))
                End If

                If (HttpContext.Current.Request("sortBy") <> "") Then
                    params.Add("sortBy=" & HttpContext.Current.Request("sortBy"))
                End If

                If (HttpContext.Current.Request("sortDir") <> "") Then
                    params.Add("sortDir=" & HttpContext.Current.Request("sortDir"))
                End If

                If (params.Count > 2 AndAlso HttpContext.Current.Request("PropertyTypeID") <> "") Then
                    params.Add("PropertyTypeID=" & HttpContext.Current.Request("PropertyTypeID"))
                End If

            End If

            If DotNetNuke.Entities.Host.HostSettings.GetHostSetting("UseFriendlyUrls") = "Y" Then

                Dim strURL As String = ApplicationURL(_tabID)
                Dim settings As PortalSettings = PortalController.GetCurrentPortalSettings

                For Each p As String In params
                    strURL = strURL & "&" & p
                Next

                Dim objTabController As New TabController
                Dim objTab As TabInfo = objTabController.GetTab(_tabID, settings.PortalId, False)

                Dim title As String = "Default.aspx"

                If (_propertySettings.SEOViewPropertyTitle <> "") Then

                    Dim delimStr As String = "[]"
                    Dim delimiter As Char() = delimStr.ToCharArray()

                    Dim phPageTitle As New PlaceHolder()
                    ProcessItem(phPageTitle.Controls, _propertySettings.SEOViewPropertyTitle.Split(delimiter), objProperty, objCustomFields)
                    title = OnlyAlphaNumericChars(RenderControlToString(phPageTitle))
                    If (title Is Nothing OrElse title.Trim() = "") Then
                        title = "Default.aspx"
                    Else
                        title = title.Replace("--", "-") & ".aspx"
                    End If
                End If

                Dim link As String = FriendlyUrl(objTab, strURL, title, settings)

                If (link.ToLower().StartsWith("http://") Or link.ToLower().StartsWith("https://")) Then
                    Return link
                Else
                    If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & link)
                    Else
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & link)
                    End If
                End If

            Else
                Return NavigateURL(_tabID, "", params.ToArray())
            End If

        End Function

        Private Function GetFieldValue(ByVal objCustomField As CustomFieldInfo, ByVal objProperty As PropertyInfo, ByVal showCaption As Boolean, ByVal showUrlOnly As Boolean) As String
            Return GetFieldValue(objCustomField, objProperty, showCaption, showUrlOnly, False)
        End Function

        Private Function GetFieldValue(ByVal objCustomField As CustomFieldInfo, ByVal objProperty As PropertyInfo, ByVal showCaption As Boolean, ByVal showUrlOnly As Boolean, ByVal doReplace As Boolean) As String

            Dim value As String = objProperty.PropertyList(objCustomField.CustomFieldID).ToString()
            If (objCustomField.FieldType = CustomFieldType.RichTextBox) Then
                value = _objPage.Server.HtmlDecode(objProperty.PropertyList(objCustomField.CustomFieldID).ToString())
            Else
                If (objCustomField.FieldType = CustomFieldType.MultiCheckBox Or objCustomField.FieldType = CustomFieldType.ListBox) Then
                    If (doReplace) Then
                        value = objProperty.PropertyList(objCustomField.CustomFieldID).ToString().Replace("|", ", ")
                    Else
                        value = objProperty.PropertyList(objCustomField.CustomFieldID).ToString()
                    End If
                End If
                If (objCustomField.FieldType = CustomFieldType.MultiLineTextBox) Then
                    value = objProperty.PropertyList(objCustomField.CustomFieldID).ToString().Replace(vbCrLf, "<br />")
                End If
                If (objCustomField.FieldType = CustomFieldType.FileUpload) Then
                    Dim filePath As String = objProperty.PropertyList(objCustomField.CustomFieldID).ToString()
                    If (filePath <> "") Then
                        If (showUrlOnly) Then
                            If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                                value = AddHTTP(HttpContext.Current.Request.Url.Host & _portalSettings.HomeDirectory & filePath.Replace("\"c, "/"c))
                            Else
                                value = AddHTTP(HttpContext.Current.Request.Url.Host & ":" & HttpContext.Current.Request.Url.Port.ToString() & _portalSettings.HomeDirectory & filePath.Replace("\"c, "/"c))
                            End If
                        Else
                            Dim fileName As String = Path.GetFileNameWithoutExtension(_portalSettings.HomeDirectoryMapPath & filePath)
                            value = "<a href='" & _portalSettings.HomeDirectory & filePath.Replace("\"c, "/"c) & "'>" & fileName & "</a>"
                        End If
                    End If
                End If
                If (objCustomField.FieldType = CustomFieldType.Hyperlink) Then
                    If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                        If (showUrlOnly) Then
                            value = AddHTTP(objProperty.PropertyList(objCustomField.CustomFieldID).ToString())
                        Else
                            value = "<a href='" & AddHTTP(objProperty.PropertyList(objCustomField.CustomFieldID).ToString()) & "' target=""_blank"">" & objProperty.PropertyList(objCustomField.CustomFieldID).ToString() & "</a>"
                        End If
                    End If
                End If
                If (value <> "" And objCustomField.ValidationType = CustomFieldValidationType.Date) Then
                    Try
                        value = DateTime.Parse(value).ToShortDateString()
                    Catch
                        value = objProperty.PropertyList(objCustomField.CustomFieldID).ToString()
                    End Try
                End If

                If (value <> "" And objCustomField.ValidationType = CustomFieldValidationType.Currency) Then
                    Try
                        Dim culture As String = "en-US"

                        Select Case _propertySettings.Currency

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
                                Select Case _propertySettings.EuroType
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

                        Dim userLocale As String = _propertySettings.Currency.ToString()
                        If (HttpContext.Current.Request.Cookies("PA-" & _moduleID.ToString() & "-Currency") IsNot Nothing) Then

                            If (HttpContext.Current.Request.Cookies("PA-" & _moduleID.ToString() & "-Currency").Value <> _propertySettings.Currency.ToString()) Then

                                Dim rate As Double = Null.NullDouble

                                Dim uri As New Uri("http://themoneyconverter.com/rss-feed/" & _propertySettings.Currency.ToString() & "/rss.xml")
                                If (uri.Scheme = uri.UriSchemeHttp) Then

                                    Dim objXml As XmlDocument = CType(DataCache.GetCache("PA-" & _moduleID.ToString() & "-RSS"), XmlDocument)
                                    If (objXml Is Nothing) Then
                                        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
                                        request.Method = WebRequestMethods.Http.Get
                                        Dim response As HttpWebResponse = request.GetResponse()
                                        Dim reader As New StreamReader(response.GetResponseStream())
                                        Dim tmp As String = reader.ReadToEnd()
                                        response.Close()
                                        objXml = New XmlDocument()
                                        objXml.LoadXml(tmp)
                                        DataCache.SetCache("PA-" & _moduleID.ToString() & "-RSS", objXml, DateTime.Now.AddMinutes(15))
                                    End If

                                    Dim rssItems As XmlNodeList = objXml.SelectNodes("rss/channel/item")
                                    For Each objNode As XmlNode In rssItems
                                        Dim objTitle As XmlNode = objNode.SelectSingleNode("title")
                                        If (objTitle IsNot Nothing) Then
                                            If (objTitle.InnerText.Contains(HttpContext.Current.Request.Cookies("PA-" & _moduleID.ToString() & "-Currency").Value)) Then
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

                                Select Case HttpContext.Current.Request.Cookies("PA-" & _moduleID.ToString() & "-Currency").Value.ToLower()

                                    Case "aud"
                                        culture = "en-AU"
                                        Exit Select

                                    Case "brl"
                                        culture = "pt-BR"
                                        Exit Select

                                    Case "cad"
                                        culture = "en-CA"
                                        Exit Select

                                    Case "chf"
                                        culture = "de-CH"
                                        Exit Select

                                    Case "cny"
                                        culture = "zh-CN"
                                        Exit Select

                                    Case "czk"
                                        culture = "cs-CZ"
                                        Exit Select

                                    Case "dkk"
                                        culture = "da-DK"
                                        Exit Select

                                    Case "eur"
                                        culture = "fr-FR"
                                        Select Case _propertySettings.EuroType
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

                                    Case "gbp"
                                        culture = "en-GB"
                                        Exit Select

                                    Case "jpy"
                                        culture = "ja-JP"
                                        Exit Select

                                    Case "usd"
                                        culture = "en-US"
                                        Exit Select

                                    Case "myr"
                                        culture = "en-MY"
                                        Exit Select

                                    Case "nzd"
                                        culture = "en-NZ"
                                        Exit Select

                                    Case "nok"
                                        culture = "nb-NO"
                                        Exit Select

                                    Case "thb"
                                        culture = "th-TH"
                                        Exit Select

                                    Case "zar"
                                        culture = "en-ZA"
                                        Exit Select

                                End Select

                            End If

                        End If

                        Dim portalFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
                        Dim format As String = "{0:C" & _propertySettings.CurrencyDecimalPlaces.ToString() & "}"
                        value = String.Format(portalFormat.NumberFormat, format, Double.Parse(value))

                    Catch exc As Exception
                        value = objProperty.PropertyList(objCustomField.CustomFieldID).ToString()
                    End Try
                End If
            End If

            If (objCustomField.IsCaptionHidden = False And showCaption) Then
                value = "<b>" & objCustomField.Caption & "</b>:&nbsp;" & value
            End If

            Return value

        End Function

        Private Function GetReviewValue(ByVal reviewID As Integer, ByVal objReviewField As ReviewFieldInfo, ByVal objProperty As PropertyInfo) As String

            Dim value As String = objProperty.ReviewList(reviewID)(objReviewField.ReviewFieldID).ToString()

            If (objReviewField.FieldType = ReviewFieldType.MultiCheckBox) Then
                value = value.Replace("|", ", ")
            End If

            If (objReviewField.FieldType = ReviewFieldType.MultiLineTextBox) Then
                value = value.Replace(vbCrLf, "<br />")
            End If

            Return value

        End Function

        Private Function GetPhotoWidth(ByVal dataItem As Object, ByVal thumbnailType As ThumbnailType) As Integer
            Return GetPhotoWidth(dataItem, thumbnailType, False)
        End Function

        Private Function GetPhotoWidth(ByVal dataItem As Object, ByVal thumbnailType As ThumbnailType, ByVal isCropped As Boolean) As Integer

            Dim thumbWidth As Integer
            Dim thumbHeight As Integer

            Select Case thumbnailType

                Case thumbnailType.Small
                    thumbWidth = _propertySettings.SmallWidth
                    thumbHeight = _propertySettings.SmallHeight

                Case thumbnailType.Medium
                    thumbWidth = _propertySettings.MediumWidth
                    thumbHeight = _propertySettings.MediumHeight

                Case thumbnailType.Large
                    thumbWidth = _propertySettings.LargeWidth
                    thumbHeight = _propertySettings.LargeHeight

            End Select

            If (isCropped) Then
                Return thumbWidth
            End If

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > thumbWidth) Then
                    width = thumbWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > thumbHeight) Then
                    height = thumbHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return width
            Else
                Return thumbWidth
            End If

        End Function

        Private Function GetPhotoHeight(ByVal dataItem As Object, ByVal thumbnailType As ThumbnailType) As Integer
            Return GetPhotoHeight(dataItem, thumbnailType, False)
        End Function

        Private Function GetPhotoHeight(ByVal dataItem As Object, ByVal thumbnailType As ThumbnailType, ByVal isCropped As Boolean) As Integer

            Dim thumbWidth As Integer
            Dim thumbHeight As Integer

            Select Case thumbnailType

                Case thumbnailType.Small
                    thumbWidth = _propertySettings.SmallWidth
                    thumbHeight = _propertySettings.SmallHeight

                Case thumbnailType.Medium
                    thumbWidth = _propertySettings.MediumWidth
                    thumbHeight = _propertySettings.MediumHeight

                Case thumbnailType.Large
                    thumbWidth = _propertySettings.LargeWidth
                    thumbHeight = _propertySettings.LargeHeight

            End Select

            If (isCropped) Then
                Return thumbHeight
            End If

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > thumbWidth) Then
                    width = thumbWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > thumbHeight) Then
                    height = thumbHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return height
            Else
                Return thumbHeight
            End If

        End Function

        Private Function GetPhotoPath(ByVal dataItem As Object, ByVal width As Integer, ByVal height As Integer, ByVal isCropped As Boolean) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                If (objPhoto.PhotoType = PhotoType.Internal) Then
                    If (objPhoto.Width <> width And objPhoto.Height <> height) Then
                        If (isCropped) Then
                            Return _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & _objPage.Server.UrlEncode(_portalSettings.HomeDirectory & "/PropertyAgent/" & _moduleID.ToString() & "/Images") & "&fileName=" & _objPage.Server.UrlEncode(objPhoto.Filename) & "&portalid=" & _portalID.ToString() & "&i=" & objPhoto.PhotoID & "&s=1")
                        Else
                            Return _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & _objPage.Server.UrlEncode(_portalSettings.HomeDirectory & "/PropertyAgent/" & _moduleID.ToString() & "/Images") & "&fileName=" & _objPage.Server.UrlEncode(objPhoto.Filename) & "&portalid=" & _portalID.ToString() & "&i=" & objPhoto.PhotoID)
                        End If
                    Else
                        Return _portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Images/" & objPhoto.Filename
                    End If
                Else
                    If (objPhoto.Width <> width And objPhoto.Height <> height) Then
                        Return _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/ImageHandlerExt.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&i=" & objPhoto.PhotoID.ToString() & "&portalid=" & _portalID.ToString() & "&Q=" & _propertySettings.HighQuality.ToString())
                    Else
                        Return DotNetNuke.Common.AddHTTP(objPhoto.ExternalUrl)
                    End If
                End If
            Else
                Dim w As Integer = 600
                Dim h As Integer = 450

                If (w <> width And h <> height) Then
                    Return _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & _objPage.Server.UrlEncode(_objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/")) & "&fileName=" & _objPage.Server.UrlEncode("placeholder-600.jpg") & "&portalid=" & _portalID.ToString() & "&Q=" & _propertySettings.HighQuality.ToString())
                Else
                    Return _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/placeholder-600.jpg")
                End If
            End If

        End Function

        Private Function GetSharedResource(ByVal key As String) As String

            Dim path As String = "~/DesktopModules/PropertyAgent/" & DotNetNuke.Services.Localization.Localization.LocalResourceDirectory & "/" & DotNetNuke.Services.Localization.Localization.LocalSharedResourceFile
            Return DotNetNuke.Services.Localization.Localization.GetString(key, path)

        End Function

        Private Function HtmlEncode(ByVal html As String) As String
            Return HttpContext.Current.Server.HtmlEncode(html)
        End Function

        Private Sub ProcessPhotoToken(ByRef objPlaceHolder As ControlCollection, ByVal objPhoto As PhotoInfo, ByVal iPtr As String, ByVal thumbnailType As ThumbnailType, ByVal isCropped As Boolean)

            Dim objImage As New Image

            If Not objPhoto Is Nothing Then
                objImage.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())

                Dim width As Integer = GetPhotoWidth(objPhoto, thumbnailType)
                Dim height As Integer = GetPhotoHeight(objPhoto, thumbnailType)

                objImage.Width = Unit.Pixel(width)
                objImage.Height = Unit.Pixel(height)

                objImage.ImageUrl = GetPhotoPath(objPhoto, width, height, isCropped)
            Else
                objImage.ID = Globals.CreateValidID(_moduleKey & "-Photo--1-" & iPtr.ToString())
                Select Case thumbnailType

                    Case thumbnailType.Small
                        objImage.ImageUrl = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/ImageHandler.ashx?width=" & _propertySettings.SmallWidth.ToString() & "&height=" & _propertySettings.SmallHeight.ToString() & "&HomeDirectory=" & _objPage.Server.UrlEncode(_objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/")) & "&fileName=" & _objPage.Server.UrlEncode("placeholder-600.jpg") & "&portalid=" & _portalID.ToString())

                    Case thumbnailType.Medium
                        objImage.ImageUrl = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/ImageHandler.ashx?width=" & _propertySettings.MediumWidth.ToString() & "&height=" & _propertySettings.MediumHeight.ToString() & "&HomeDirectory=" & _objPage.Server.UrlEncode(_objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/")) & "&fileName=" & _objPage.Server.UrlEncode("placeholder-600.jpg") & "&portalid=" & _portalID.ToString())

                    Case thumbnailType.Large
                        objImage.ImageUrl = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/ImageHandler.ashx?width=" & _propertySettings.LargeWidth.ToString() & "&height=" & _propertySettings.LargeHeight.ToString() & "&HomeDirectory=" & _objPage.Server.UrlEncode(_objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/")) & "&fileName=" & _objPage.Server.UrlEncode("placeholder-600.jpg") & "&portalid=" & _portalID.ToString())

                End Select
            End If

            objPlaceHolder.Add(objImage)

        End Sub

        Private Sub ProcessPhotoHeightToken(ByRef objPlaceHolder As ControlCollection, ByVal objPhoto As PhotoInfo, ByVal iPtr As String, ByVal thumbnailType As ThumbnailType, ByVal isCropped As Boolean)

            Dim objLiteral As New Literal
            If Not (objPhoto Is Nothing) Then
                objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-PhotoHeight-" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
            End If
            Dim height As Integer = GetPhotoHeight(objPhoto, thumbnailType, isCropped)
            objLiteral.Text = height.ToString()
            objPlaceHolder.Add(objLiteral)

        End Sub

        Private Sub ProcessPhotoLinkToken(ByRef objPlaceHolder As ControlCollection, ByVal objPhoto As PhotoInfo, ByVal iPtr As String, ByVal thumbnailType As ThumbnailType, ByVal isCropped As Boolean)

            Dim objLiteral As New Literal
            If Not (objPhoto Is Nothing) Then
                objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-PhotoLink-" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
            End If
            Dim width As Integer = GetPhotoWidth(objPhoto, thumbnailType, isCropped)
            Dim height As Integer = GetPhotoHeight(objPhoto, thumbnailType, isCropped)
            objLiteral.Text = GetPhotoPath(objPhoto, width, height, isCropped)
            objPlaceHolder.Add(objLiteral)

        End Sub

        Private Sub ProcessPhotoWidthToken(ByRef objPlaceHolder As ControlCollection, ByVal objPhoto As PhotoInfo, ByVal iPtr As String, ByVal thumbnailType As ThumbnailType, ByVal isCropped As Boolean)

            Dim objLiteral As New Literal
            If Not (objPhoto Is Nothing) Then
                objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-PhotoWidth-" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
            End If
            Dim width As Integer = GetPhotoWidth(objPhoto, thumbnailType, isCropped)
            objLiteral.Text = width.ToString()
            objPlaceHolder.Add(objLiteral)

        End Sub

        Private Function TidyOutput(ByVal input As String) As String

            Return input.Replace(",", " ")

        End Function

        Private Sub dlPhotos_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)
                ProcessPhoto(e.Item.Controls, Me._objLayoutPhotoItem.Tokens, objPhoto, objPhoto.PhotoID)

            End If

        End Sub

        Private Sub rptPhotos_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)
                ProcessPhoto(e.Item.Controls, Me._objLayoutPhotoItem.Tokens, objPhoto, objPhoto.PhotoID)

            End If

        End Sub

        Public Shared Function IsMobileBrowser() As Boolean

            If (HttpContext.Current Is Nothing) Then
                Return False
            End If

            'GETS THE CURRENT USER CONTEXT
            Dim context As HttpContext = HttpContext.Current

            'FIRST TRY BUILT IN ASP.NT CHECK
            If context.Request.Browser.IsMobileDevice Then
                Return True
            End If
            'THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            If context.Request.ServerVariables("HTTP_X_WAP_PROFILE") IsNot Nothing Then
                Return True
            End If
            'THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            If context.Request.ServerVariables("HTTP_ACCEPT") IsNot Nothing AndAlso context.Request.ServerVariables("HTTP_ACCEPT").ToLower().Contains("wap") Then
                Return True
            End If
            'AND FINALLY CHECK THE HTTP_USER_AGENT 
            'HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            If context.Request.ServerVariables("HTTP_USER_AGENT") IsNot Nothing Then

                'Create a list of all mobile types
                Dim mobiles() As String = {"midp", "j2me", "avant", "docomo", "novarra", "palmos", _
                     "palmsource", "240x320", "opwv", "chtml", "pda", "windows ce", _
                     "mmp/", "blackberry", "mib/", "symbian", "wireless", "nokia", _
                     "hand", "mobi", "phone", "cdm", "up.b", "audio", _
                     "SIE-", "SEC-", "samsung", "HTC", "mot-", "mitsu", _
                     "sagem", "sony", "alcatel", "lg", "eric", "vx", _
                     "NEC", "philips", "mmm", "xx", "panasonic", "sharp", _
                     "wap", "sch", "rover", "pocket", "benq", "java", _
                     "pt", "pg", "vox", "amoi", "bird", "compal", _
                     "kg", "voda", "sany", "kdd", "dbt", "sendo", _
                     "sgh", "gradi", "jb", "dddi", "moto", "iphone"}

                'Loop through each item in the list created above 
                'and check if the header contains that text
                For Each s As String In mobiles
                    If context.Request.ServerVariables("HTTP_USER_AGENT").ToLower().Contains(s.ToLower()) Then
                        Return True
                    End If
                Next
            End If

            Return False

        End Function

        ' calculate the MD5 hash of a given string 
        ' the string is first converted to a byte array
        Public Function MD5CalcString(ByVal strData As String) As String

            Dim objMD5 As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim arrData() As Byte
            Dim arrHash() As Byte

            ' first convert the string to bytes (using UTF8 encoding for unicode characters)
            arrData = Text.Encoding.UTF8.GetBytes(strData)

            ' hash contents of this byte array
            arrHash = objMD5.ComputeHash(arrData)

            ' thanks objects
            objMD5 = Nothing

            ' return formatted hash
            Return ByteArrayToString(arrHash)

        End Function

        ' utility function to convert a byte array into a hex string
        Private Function ByteArrayToString(ByVal arrInput() As Byte) As String

            Dim strOutput As New System.Text.StringBuilder(arrInput.Length)

            For i As Integer = 0 To arrInput.Length - 1
                strOutput.Append(arrInput(i).ToString("X2"))
            Next

            Return strOutput.ToString().ToLower

        End Function

#End Region

#Region " Public Methods "

        Public Sub ClearCache(ByVal moduleID As Integer)

            For Each type As String In System.Enum.GetNames(GetType(LayoutType))
                Dim cacheKey As String = moduleID.ToString() & type.ToString()
                DataCache.RemoveCache(cacheKey)
            Next

        End Sub

        Public Function GetPhotoPath(ByVal dataItem As Object, ByVal isCropped As Boolean) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer = GetPhotoWidth(objPhoto, ThumbnailType.Small)
                Dim height As Integer = GetPhotoHeight(objPhoto, ThumbnailType.Small)

                Return GetPhotoPath(objPhoto, width, height, isCropped)
            End If

            Return ""

        End Function

        Public Function GetPhotoPath(ByVal dataItem As Object, ByVal isCropped As Boolean, ByVal width As Integer, ByVal height As Integer) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Return GetPhotoPath(objPhoto, width, height, isCropped)
            End If

            Return ""

        End Function

        Public Shared Function CheckTemplate(ByVal portalSettings As PortalSettings, ByVal moduleID As Integer, ByVal template As String, ByVal type As LayoutType) As Boolean

            Dim cacheKey As String = moduleID.ToString() & type.ToString()
            Dim objLayout As LayoutInfo = CType(DataCache.GetCache(cacheKey), LayoutInfo)

            If (objLayout IsNot Nothing) Then
                Return True
            End If

            Dim path As String = portalSettings.HomeDirectoryMapPath & "PropertyAgent\" & moduleID.ToString() & "\Templates\" & template & "\" & type.ToString().Replace("_", ".")

            If (File.Exists(path)) Then
                Return True
            End If

            Return False

        End Function

        Public Function GetLayout(ByVal template As String, ByVal type As LayoutType) As LayoutInfo

            Dim cacheKey As String = _moduleID.ToString() & type.ToString()
            Dim objLayout As LayoutInfo = CType(DataCache.GetCache(cacheKey), LayoutInfo)

            If (objLayout Is Nothing) Then
                Dim delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                objLayout = New LayoutInfo
                Dim path As String = _portalSettings.HomeDirectoryMapPath & "PropertyAgent\" & _moduleID.ToString() & "\Templates\" & template & "\" & type.ToString().Replace("_", ".")

                If (File.Exists(path) = False) Then
                    ' Need to find a default... 
                    Select Case type
                        Case LayoutType.Print_Header_Html
                            Return GetLayout(template, LayoutType.View_Header_Html)

                        Case LayoutType.Print_Item_Html
                            Return GetLayout(template, LayoutType.View_Item_Html)

                        Case LayoutType.Print_Footer_Html
                            Return GetLayout(template, LayoutType.View_Footer_Html)

                        Case LayoutType.CommentNotification_Subject_Html
                            objLayout.Template = Constants.COMMENT_EMAIL_SUBJECT_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.CommentNotification_Body_Html
                            objLayout.Template = Constants.COMMENT_EMAIL_BODY_HTML_DEFAULT
                            Exit Select

                        Case LayoutType.ReviewNotification_Subject_Html
                            objLayout.Template = Constants.REVIEW_EMAIL_SUBJECT_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.ReviewNotification_Body_Html
                            objLayout.Template = Constants.REVIEW_EMAIL_BODY_HTML_DEFAULT
                            Exit Select

                        Case LayoutType.ContactEmail_Subject_Html
                            objLayout.Template = Constants.CONTACT_EMAIL_SUBJECT_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.ContactEmail_Body_Html
                            objLayout.Template = Constants.CONTACT_EMAIL_BODY_HTML_DEFAULT
                            Exit Select

                        Case LayoutType.SendToFriendEmail_Subject_Html
                            objLayout.Template = Constants.SEND_TO_FRIEND_EMAIL_SUBJECT_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.SendToFriendEmail_Body_Html
                            objLayout.Template = Constants.SEND_TO_FRIEND_EMAIL_BODY_HTML_DEFAULT
                            Exit Select

                        Case LayoutType.ContactBroker_Subject_Html
                            objLayout.Template = Constants.CONTACT_BROKER_SUBJECT_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.ContactBroker_Body_Html
                            objLayout.Template = Constants.CONTACT_BROKER_BODY_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.ContactOwner_Subject_Html
                            objLayout.Template = Constants.CONTACT_OWNER_SUBJECT_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.ContactOwner_Body_Html
                            objLayout.Template = Constants.CONTACT_OWNER_BODY_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.Export_Header_Html
                            objLayout.Template = Constants.EXPORT_HEADER_DEFAULT
                            Exit Select
                        Case LayoutType.Export_Item_Html
                            objLayout.Template = Constants.EXPORT_ITEM_DEFAULT
                            Exit Select
                        Case LayoutType.RSS_Header_Html
                            objLayout.Template = Constants.RSS_HEADER_DEFAULT
                            Exit Select
                        Case LayoutType.RSS_Item_Html
                            objLayout.Template = Constants.RSS_ITEM_DEFAULT
                            Exit Select
                        Case LayoutType.RSS_Reader_Header_Html
                            objLayout.Template = Constants.RSS_READER_HEADER_DEFAULT
                            Exit Select
                        Case LayoutType.RSS_Reader_Item_Html
                            objLayout.Template = Constants.RSS_READER_ITEM_DEFAULT
                            Exit Select
                        Case LayoutType.RSS_Reader_Footer_Html
                            objLayout.Template = Constants.RSS_READER_FOOTER_DEFAULT
                            Exit Select
                        Case LayoutType.Submission_Subject_Html
                            objLayout.Template = Constants.NOTIFICATION_EMAIL_SUBJECT_HTML_DEFAULT
                            Exit Select
                        Case LayoutType.Submission_Body_Html
                            objLayout.Template = Constants.NOTIFICATION_EMAIL_BODY_HTML_DEFAULT
                            Exit Select

                        Case LayoutType.Types_Header_Html
                            objLayout.Template = Constants.TYPES_LAYOUT_HEADER_SETTING_DEFAULT
                            Exit Select
                        Case LayoutType.Types_Item_Html
                            objLayout.Template = Constants.TYPES_LAYOUT_ITEM_SETTING_DEFAULT
                            Exit Select
                        Case LayoutType.Types_Footer_Html
                            objLayout.Template = Constants.TYPES_LAYOUT_FOOTER_SETTING_DEFAULT
                            Exit Select

                        Case LayoutType.Option_Item_Html
                            objLayout.Template = Constants.OPTION_ITEM_DEFAULT
                            Exit Select
                        Case LayoutType.Pdf_Header_Html
                            objLayout.Template = Constants.PDF_HEADER_DEFAULT
                            Exit Select
                        Case LayoutType.Pdf_Item_Html
                            objLayout.Template = Constants.PDF_ITEM_DEFAULT
                            Exit Select
                        Case LayoutType.Pdf_Footer_Html
                            objLayout.Template = Constants.PDF_FOOTER_DEFAULT
                            Exit Select
                        Case LayoutType.XML_Header_Html
                            objLayout.Template = Constants.XML_HEADER_DEFAULT
                            Exit Select
                        Case LayoutType.XML_Item_Html
                            objLayout.Template = Constants.XML_ITEM_DEFAULT
                            Exit Select
                        Case Else
                            objLayout.Template = ""
                            Exit Select
                    End Select
                Else
                    Dim sr As System.IO.StreamReader = New System.IO.StreamReader(path)
                    Try
                        objLayout.Template = sr.ReadToEnd()
                    Catch
                        objLayout.Template = ""
                    Finally
                        If Not sr Is Nothing Then sr.Close()
                    End Try
                End If

                objLayout.Tokens = objLayout.Template.Split(delimiter)
                DataCache.SetCache(cacheKey, objLayout, New CacheDependency(path))

            End If

            Return objLayout

        End Function

        Public Function GetStylesheet(ByVal template As String) As String

            Dim value As String = ""

            Dim path As String = _portalSettings.HomeDirectoryMapPath & "PropertyAgent\" & _moduleID.ToString() & "\Templates\" & template & "\Template.css"

            If (File.Exists(path) = False) Then
                ' Need to find a default... 
            End If

            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(path)
            Try
                value = sr.ReadToEnd()
            Catch
                value = "<br>ERROR: UNABLE TO READ 'Template.css' TEMPLATE:"
            Finally
                If Not sr Is Nothing Then sr.Close()
            End Try

            Return value

        End Function

        Public Sub UpdateStylesheet(ByVal template As String, ByVal text As String)

            Dim path As String = _portalSettings.HomeDirectoryMapPath & "PropertyAgent\" & _moduleID.ToString() & "\Templates\" & template & "\Template.css"

            If (File.Exists(path)) Then
                Dim sw As StreamWriter = New StreamWriter(path)
                Try
                    sw.Write(text)
                Catch
                Finally
                    If Not sw Is Nothing Then sw.Close()
                End Try
            End If

        End Sub

        Public Sub UpdateLayout(ByVal template As String, ByVal type As LayoutType, ByVal text As String)

            Dim path As String = _portalSettings.HomeDirectoryMapPath & "PropertyAgent\" & _moduleID.ToString() & "\Templates\" & template & "\" & type.ToString().Replace("_", ".")
            Dim sw As New StreamWriter(path)
            Try
                sw.Write(text)
            Catch
            Finally
                If Not sw Is Nothing Then sw.Close()
            End Try

        End Sub

        Public Sub LoadStyleSheet(ByVal template As String, Optional ByVal checkStylesheet As Boolean = True)

            If (checkStylesheet) Then
                If (_propertySettings.TemplateIncludeStylesheet = False) Then
                    Return
                End If
            End If

            Dim objCSS As Control = _objPage.FindControl("CSS")

            If Not (objCSS Is Nothing) Then
                Dim objLiteral As New Literal()
                objLiteral.Text = "<link type=""text/css"" rel=""stylesheet"" href=""" & _portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Templates/" & template & "/Template.css" & """ />"

                'Dim objLink As HtmlGenericControl
                'objLink = New HtmlGenericControl("LINK")
                'objLink.ID = _moduleKey & "Template_" & _moduleID.ToString()
                'objLink.Attributes("rel") = "stylesheet"
                'objLink.Attributes("type") = "text/css"
                'objLink.Attributes("href") = _portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Templates/" & template & "/Template.css"

                For i As Integer = 0 To objCSS.Controls.Count - 1
                    If (TypeOf objCSS.Controls(i) Is HtmlControl) Then
                        Dim objChildLink As HtmlControl = CType(objCSS.Controls(i), HtmlControl)
                        If Not (objChildLink Is Nothing) Then
                            If Not (objChildLink.Attributes.Item("href") Is Nothing) Then
                                If (objChildLink.Attributes.Item("href") <> "") Then
                                    If (objChildLink.Attributes("href").ToLower() = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/module.css").ToLower().Replace("%20", " ")) Then
                                        objCSS.Controls.AddAt(i + 1, objLiteral)
                                        Return
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

                objCSS = CType(_objPage.FindControl("Head"), HtmlGenericControl)

                If Not (objCSS Is Nothing) Then
                    For i As Integer = 0 To objCSS.Controls.Count - 1
                        If (TypeOf objCSS.Controls(i) Is HtmlControl) Then
                            Dim objChildLink As HtmlControl = CType(objCSS.Controls(i), HtmlControl)
                            If Not (objChildLink Is Nothing) Then
                                If (objChildLink.Attributes("href") <> "") Then
                                    If (objChildLink.Attributes("href").ToLower() = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/module.css").ToLower().Replace("%20", " ")) Then
                                        objCSS.Controls.AddAt(i + 1, objLiteral)
                                        Return
                                    End If
                                End If
                            End If
                        End If
                    Next
                    objCSS.Controls.AddAt(0, objLiteral)
                Else
                    objCSS = _objPage.FindControl("CSS")
                    objCSS.Controls.AddAt(0, objLiteral)
                End If
            End If

        End Sub

        Private Function Agent(ByVal authorID As Integer) As UserInfo

            If (authorID = Null.NullInteger) Then
                Return Nothing
            End If

            If (_agent Is Nothing) Then
                _agent = UserController.GetUser(_portalID, authorID, True)
            Else
                If (_agent.UserID = authorID) Then
                    Return _agent
                Else
                    _agent = UserController.GetUser(_portalID, authorID, True)
                End If
            End If

            Return _agent

        End Function

        Private Function Modified(ByVal modifiedID As Integer) As UserInfo

            If (modifiedID = Null.NullInteger) Then
                Return Nothing
            End If

            If (_modified Is Nothing) Then
                _modified = UserController.GetUser(_portalID, modifiedID, True)
            Else
                If (_modified.UserID = modifiedID) Then
                    Return _modified
                Else
                    _modified = UserController.GetUser(_portalID, modifiedID, True)
                End If
            End If

            Return _modified

        End Function

        Private Function Broker(ByVal brokerID As Integer) As UserInfo

            If (brokerID = Null.NullInteger) Then
                Return Nothing
            End If

            If (_broker Is Nothing) Then
                _broker = UserController.GetUser(_portalID, brokerID, True)
            Else
                If (_broker.UserID = brokerID) Then
                    Return _broker
                Else
                    _broker = UserController.GetUser(_portalID, brokerID, True)
                End If
            End If

            Return _broker

        End Function

#Region " Comments "

        Protected Sub ProcessComment(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objComment As CommentInfo, ByVal isPrint As Boolean)

            Dim objProperty As PropertyInfo = Nothing

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString().Replace("~", DotNetNuke.Common.Globals.ApplicationPath)))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "COMMENT"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.Comment.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.CommentID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "CREATEDATE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.CreateDate.ToShortDateString()
                            objPlaceHolder.Add(objLiteral)

                        Case "DELETE"
                            If Not isPrint And _isEditable Then
                                Dim objCommentDelete As DeleteComment = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/DeleteComment.ascx"), DeleteComment)
                                objCommentDelete.CurrentComment = objComment
                                objCommentDelete.ModuleID = _moduleID
                                objCommentDelete.TabID = _tabID
                                objCommentDelete.ModuleKey = _moduleKey
                                objPlaceHolder.Add(objCommentDelete)
                            End If

                        Case "EMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.Email.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "GRAVATARURL"
                            Dim objLiteral As New Literal
                            If (objComment.UserID = Null.NullInteger) Then
                                objLiteral.Text = "http://www.gravatar.com/avatar/" & MD5CalcString(objComment.Email.ToLower())
                            Else
                                objLiteral.Text = "http://www.gravatar.com/avatar/" & MD5CalcString(objComment.Email.ToLower())
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "ISAGENT"
                            If (objProperty Is Nothing) Then
                                Dim objPropertyController As New PropertyController()
                                objProperty = objPropertyController.Get(objComment.PropertyID)

                                If (objProperty.AuthorID <> objComment.UserID) Then
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = "/ISAGENT") Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If
                        Case "/ISAGENT"
                            ' Do Nothing

                        Case "ISNOTAGENT"
                            If (objProperty Is Nothing) Then
                                Dim objPropertyController As New PropertyController()
                                objProperty = objPropertyController.Get(objComment.PropertyID)

                                If (objProperty.AuthorID = objComment.UserID) Then
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = "/ISNOTAGENT") Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                            End If
                        Case "/ISNOTAGENT"
                            ' Do Nothing

                        Case "ISANONYMOUS"
                            If (objComment.UserID <> Null.NullInteger) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISANONYMOUS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISANONYMOUS"
                            ' Do Nothing

                        Case "ISCHILD"
                            If (objComment.ParentID = Null.NullInteger) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISCHILD") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISCHILD"
                            ' Do Nothing

                        Case "ISCOMMENTOWNER"
                            If (objComment.UserID <> UserController.GetCurrentUserInfo().UserID) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISCOMMENTOWNER") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISCOMMENTOWNER"
                            ' Do Nothing

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
                            ' Do Nothing.

                        Case "ISREGISTERED"
                            If (objComment.UserID = Null.NullInteger) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISREGISTERED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISREGISTERED"
                            ' Do Nothing

                        Case "NAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.Name.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.PropertyID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "RATING"
                            If (objComment.Rating <> Null.NullDouble) Then
                                Dim rating As Integer = 0
                                Dim ratingPercentage As Integer = 0
                                If (objComment.Rating <> Null.NullDouble) Then
                                    rating = Math.Round(objComment.Rating)
                                    ratingPercentage = objComment.Rating * 20
                                End If

                                Dim script As String = "" _
                                    & "<ul id=""rating-" & objComment.PropertyID.ToString() & "-" & objComment.CommentID.ToString() & """ class=""pa-star-rating pa-has-rated"">" _
                                    & " <li class=""pa-current-rating"" style=""width:" & ratingPercentage.ToString() & "%;"">Currently " & rating.ToString() & "/5 Stars.</li>" _
                                    & " <li><a href=""#"" title=""1 star out of 5"" class=""pa-one-star"">1</a></li>" _
                                    & " <li><a href=""#"" title=""2 stars out of 5"" class=""pa-two-stars"">2</a></li>" _
                                    & " <li><a href=""#"" title=""3 stars out of 5"" class=""pa-three-stars"">3</a></li>" _
                                    & " <li><a href=""#"" title=""4 stars out of 5"" class=""pa-four-stars"">4</a></li>" _
                                    & " <li><a href=""#"" title=""5 stars out of 5"" class=""pa-five-stars"">5</a></li>" _
                                    & "</ul>"

                                Dim objLiteral As New Literal
                                objLiteral.Text = script
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "RATINGVALUE"
                            If (objComment.Rating <> Null.NullDouble) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = objComment.Rating.ToString()
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "REPLYLINK"
                            If (objProperty Is Nothing) Then
                                Dim objPropertyController As New PropertyController()
                                objProperty = objPropertyController.Get(objComment.PropertyID)
                            End If

                            Dim objCustomFieldController As New CustomFieldController
                            Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(objProperty.ModuleID, True)

                            Dim objLiteral As New Literal
                            objLiteral.Text = GetExternalLink(GetPropertyLink(objProperty, objCustomFields))

                            If (objLiteral.Text.Contains("?")) Then
                                objLiteral.Text = objLiteral.Text & "&pid=" & objComment.CommentID & "#CommentForm"
                            Else
                                objLiteral.Text = objLiteral.Text & "?pid=" & objComment.CommentID & "#CommentForm"
                            End If

                            objPlaceHolder.Add(objLiteral)

                        Case "USERID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.UserID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "WEBSITE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objComment.Website.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            Dim isRendered As Boolean = False

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CREATEDATE:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)
                                If (objComment.CreateDate <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = objComment.CreateDate.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo
                                Dim isLink As Boolean = False

                                If (objProperty Is Nothing) Then
                                    Dim objPropertyController As New PropertyController()
                                    objProperty = objPropertyController.Get(objComment.PropertyID)
                                End If

                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(objProperty.ModuleID, True)

                                If (field.EndsWith("link")) Then
                                    Dim fieldWithoutLink As String = field.Remove(field.Length - 4, 4)
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = fieldWithoutLink.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                            isLink = True
                                        End If
                                    Next
                                End If

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If
                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, isLink)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        objLiteral.Text = fieldValue
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If

                                isRendered = True

                            End If

                            If (isRendered = False) Then
                                Dim objLiteralOther As New Literal
                                objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                                objLiteralOther.EnableViewState = False
                                objPlaceHolder.Add(objLiteralOther)
                            End If

                    End Select
                End If

            Next

        End Sub

#End Region

#Region " Export Templates "

        Public Sub ProcessExportHeader(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objCustomFields As List(Of CustomFieldInfo))

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "CUSTOMFIELDS"
                            Dim i As Integer = 0
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString() & "-" & i.ToString())
                                If ((i + 1) = objCustomFields.Count) Then
                                    objLiteral.Text = TidyOutput(objCustomField.Name)
                                Else
                                    objLiteral.Text = TidyOutput(objCustomField.Name) & ","
                                End If
                                objPlaceHolder.Add(objLiteral)
                                i = i + 1
                            Next

                    End Select
                End If

            Next

        End Sub

        Public Sub ProcessExportItem(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objProperty As PropertyInfo, ByVal objCustomFields As List(Of CustomFieldInfo))

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "COMMENTCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.CommentCount.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "CUSTOMFIELDS"
                            Dim i As Integer = 0
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                If ((i + 1) = objCustomFields.Count) Then
                                    objLiteral.Text = TidyOutput(GetFieldValue(objCustomField, objProperty, False, True))
                                Else
                                    objLiteral.Text = TidyOutput(GetFieldValue(objCustomField, objProperty, False, True)) & ","
                                End If
                                objPlaceHolder.Add(objLiteral)
                                i = i + 1
                            Next

                        Case "DATECREATED"
                            If (objProperty.DateCreated <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = TidyOutput(objProperty.DateCreated.ToShortDateString())
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEMODIFIED"
                            If (objProperty.DateModified <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = TidyOutput(objProperty.DateModified.ToShortDateString())
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEPUBLISHED"
                            If (objProperty.DatePublished <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = TidyOutput(objProperty.DatePublished.ToShortDateString())
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEEXPIRED"
                            If (objProperty.DateExpired <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = TidyOutput(objProperty.DateExpired.ToShortDateString())
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.DisplayName)
                            objPlaceHolder.Add(objLiteral)

                        Case "EMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.Email)
                            objPlaceHolder.Add(objLiteral)

                        Case "FEATURED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.IsFeatured.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "FULLNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.DisplayName)
                            objPlaceHolder.Add(objLiteral)

                        Case "HITS"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.ViewCount.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "LINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=View", _propertySettings.SEOPropertyID & "=" & objProperty.PropertyID.ToString()))
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(_portalSettings.PortalId.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(_portalSettings.PortalName)
                            objPlaceHolder.Add(objLiteral)

                        Case "PRINTLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(ResolveUrl("~/DesktopModules/PropertyAgent/Print.aspx?ModuleID=" & _moduleID.ToString() & "&PortalID=" & _portalID.ToString() & "&TabID=" & _tabID.ToString() & "&PropertyID=" & objProperty.PropertyID.ToString() & "&Template=" & _propertySettings.Template))
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.PropertyID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(_propertySettings.PropertyLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(_propertySettings.PropertyPluralLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPELABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(_propertySettings.PropertyTypeLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPEPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(_propertySettings.PropertyTypePluralLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.PropertyID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "RATINGVALUE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.Rating.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "RATINGCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.RatingCount.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "TEMPLATEPATH"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(_portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Templates/" & _propertySettings.Template & "/")
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.PropertyTypeName.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.PropertyTypeID)
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEDESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.PropertyTypeDescription)
                            objPlaceHolder.Add(objLiteral)

                        Case "URL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(System.Web.HttpContext.Current.Request.Url.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "URLENCODED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(System.Web.HttpContext.Current.Server.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()))
                            objPlaceHolder.Add(objLiteral)

                        Case "USERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.Username)
                            objPlaceHolder.Add(objLiteral)

                        Case "USERID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TidyOutput(objProperty.AuthorID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo
                                Dim isLink As Boolean = False

                                If (field.EndsWith("link")) Then
                                    Dim fieldWithoutLink As String = field.Remove(field.Length - 4, 4)
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = fieldWithoutLink.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                            isLink = True
                                        End If
                                    Next
                                End If

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If

                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, isLink)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        objLiteral.Text = TidyOutput(fieldValue)
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If

                            End If

                    End Select
                End If
            Next

        End Sub

#End Region

#Region " Reviews "

        Protected Sub ProcessReview(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objReview As ReviewInfo, ByVal isPrint As Boolean)

            Dim objProperty As PropertyInfo = Nothing

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString().Replace("~", DotNetNuke.Common.Globals.ApplicationPath)))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "CREATEDATE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objReview.CreateDate.ToShortDateString()
                            objPlaceHolder.Add(objLiteral)

                        Case "DELETE"
                            If Not isPrint And _isEditable Then
                                Dim objReviewDelete As DeleteReview = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/DeleteReview.ascx"), DeleteReview)
                                objReviewDelete.CurrentReview = objReview
                                objReviewDelete.ModuleID = _moduleID
                                objReviewDelete.TabID = _tabID
                                objReviewDelete.ModuleKey = _moduleKey
                                objReviewDelete.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objReviewDelete)
                            End If

                        Case "DISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objReview.DisplayName
                            objPlaceHolder.Add(objLiteral)

                        Case "EMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objReview.Email
                            objPlaceHolder.Add(objLiteral)

                        Case "REVIEWID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objReview.ReviewID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "USERID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objReview.UserID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            Dim isRendered As Boolean = False

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CREATEDATE:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)
                                If (objReview.CreateDate <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objReview.CreateDate.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASFIELD:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)

                                If (objProperty Is Nothing) Then
                                    Dim objPropertyController As New PropertyController()
                                    objProperty = objPropertyController.Get(objReview.PropertyID)
                                End If

                                Dim objReviewFieldController As New ReviewFieldController
                                Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(_moduleID)

                                For Each objReviewField As ReviewFieldInfo In objReviewFields
                                    If (objReviewField.Name.ToLower() = field.ToLower()) Then
                                        If (objProperty.ReviewList(objReview.ReviewID).Contains(objReviewField.ReviewFieldID)) Then
                                            Dim fieldValue As String = GetReviewValue(objReview.ReviewID, objReviewField, objProperty)
                                            If (fieldValue.Trim() = "") Then
                                                Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                                While (iPtr < layoutArray.Length - 1)
                                                    If (layoutArray(iPtr + 1) = endToken) Then
                                                        Exit While
                                                    End If
                                                    iPtr = iPtr + 1
                                                End While
                                            End If
                                        End If
                                    End If
                                Next

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASFIELD:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("FIELD:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(6, layoutArray(iPtr + 1).Length - 6).ToLower()

                                Dim reviewFieldID As Integer = Null.NullInteger
                                Dim objReviewFieldSelected As New ReviewFieldInfo

                                If (objProperty Is Nothing) Then
                                    Dim objPropertyController As New PropertyController()
                                    objProperty = objPropertyController.Get(objReview.PropertyID)
                                End If

                                Dim objReviewFieldController As New ReviewFieldController
                                Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(objProperty.ModuleID)

                                For Each objReviewField As ReviewFieldInfo In objReviewFields
                                    If (objReviewField.Name.ToLower() = field.ToLower()) Then
                                        reviewFieldID = objReviewField.ReviewFieldID
                                        objReviewFieldSelected = objReviewField
                                    End If
                                Next

                                If (reviewFieldID <> Null.NullInteger) Then
                                    Dim i As Integer = 0
                                    If (objProperty.ReviewList(objReview.ReviewID).Contains(reviewFieldID)) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        Dim reviewValue As String = GetReviewValue(objReview.ReviewID, objReviewFieldSelected, objProperty)
                                        objLiteral.Text = reviewValue
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("RATING:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower()

                                Dim reviewFieldID As Integer = Null.NullInteger
                                Dim objReviewFieldSelected As New ReviewFieldInfo

                                If (objProperty Is Nothing) Then
                                    Dim objPropertyController As New PropertyController()
                                    objProperty = objPropertyController.Get(objReview.PropertyID)
                                End If

                                Dim objReviewFieldController As New ReviewFieldController
                                Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(objProperty.ModuleID)

                                For Each objReviewField As ReviewFieldInfo In objReviewFields
                                    If (objReviewField.Name.ToLower() = field.ToLower()) Then
                                        reviewFieldID = objReviewField.ReviewFieldID
                                        objReviewFieldSelected = objReviewField
                                    End If
                                Next

                                If (reviewFieldID <> Null.NullInteger) Then
                                    Dim i As Integer = 0
                                    If (objProperty.ReviewList(objReview.ReviewID).Contains(reviewFieldID)) Then
                                        Dim reviewValue As String = GetReviewValue(objReview.ReviewID, objReviewFieldSelected, objProperty)
                                        If (IsNumeric(reviewValue)) Then

                                            Dim rating As Integer = 0
                                            Dim ratingPercentage As Integer = 0
                                            If (objProperty.Rating <> Null.NullDouble) Then
                                                rating = Math.Round(Convert.ToInt32(reviewValue))
                                                ratingPercentage = Convert.ToInt32(reviewValue) * 20
                                            End If

                                            Dim script As String = "" _
                                                & "<ul id=""review-" & objProperty.PropertyID.ToString() & """ class=""pa-star-rating pa-has-rated" & """>" _
                                                & " <li class=""pa-current-rating"" style=""width:" & ratingPercentage.ToString() & "%;"">Currently " & rating.ToString() & "/5 Stars.</li>" _
                                                & " <li><a href=""#"" title=""1 star out of 5"" class=""pa-one-star"">1</a></li>" _
                                                & " <li><a href=""#"" title=""2 stars out of 5"" class=""pa-two-stars"">2</a></li>" _
                                                & " <li><a href=""#"" title=""3 stars out of 5"" class=""pa-three-stars"">3</a></li>" _
                                                & " <li><a href=""#"" title=""4 stars out of 5"" class=""pa-four-stars"">4</a></li>" _
                                                & " <li><a href=""#"" title=""5 stars out of 5"" class=""pa-five-stars"">5</a></li>" _
                                                & "</ul>"

                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                            objLiteral.Text = script
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                        End If

                                        i = i + 1
                                    End If
                                End If

                                isRendered = True

                            End If


                            If (isRendered = False) Then
                                Dim objLiteralOther As New Literal
                                objLiteralOther.ID = Globals.CreateValidID(_moduleKey & "-Review-" & objReview.ReviewID.ToString() & "-" & iPtr.ToString())
                                objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                                objLiteralOther.EnableViewState = False
                                objPlaceHolder.Add(objLiteralOther)
                            End If

                    End Select
                End If

            Next

        End Sub

#End Region

#Region " RSS Templates "

        Public Sub ProcessRSSHeader(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objCustomFields As List(Of CustomFieldInfo), ByVal objProperties As List(Of PropertyInfo), ByVal objModuleConfiguration As ModuleInfo, ByVal objLayoutItem As LayoutInfo, ByVal objLayoutPhoto As LayoutInfo)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "ITEMS"
                            For Each objProperty As PropertyInfo In objProperties
                                ProcessRSSItem(objPlaceHolder, objLayoutItem.Tokens, objProperty, objCustomFields, objLayoutPhoto)
                            Next

                        Case "MODULENAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = objModuleConfiguration.ModuleTitle
                            objPlaceHolder.Add(objLiteral)

                        Case "NOW"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = DateTime.Now.ToUniversalTime().ToString("r")
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = _portalSettings.PortalId.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = _portalSettings.PortalName
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyPluralLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPELABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyTypeLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPEPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyTypePluralLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALURL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            If (_portalSettings.PortalAlias.HTTPAlias.IndexOf("http://") = -1) And (_portalSettings.PortalAlias.HTTPAlias.IndexOf("https://") = -1) Then
                                objLiteral.Text = AddHTTP(_portalSettings.PortalAlias.HTTPAlias)
                            Else
                                objLiteral.Text = _portalSettings.PortalAlias.HTTPAlias
                            End If
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLE"
                            If (HttpContext.Current.Request("Title") <> "") Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                Dim objSecurity As New PortalSecurity
                                objLiteral.Text = objSecurity.InputFilter(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request("Title")), PortalSecurity.FilterFlag.NoScripting)
                                objPlaceHolder.Add(objLiteral)
                            End If

                    End Select
                End If

            Next

        End Sub

        Public Sub ProcessRSSItem(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objProperty As PropertyInfo, ByVal objCustomFields As List(Of CustomFieldInfo), ByVal objLayoutPhoto As LayoutInfo)

            Dim objCommentController As New CommentController
            Dim objComments As ArrayList = objCommentController.List(objProperty.PropertyID)

            Dim objCommentFound As CommentInfo = Nothing
            For Each objComment As CommentInfo In objComments
                If (objComment.Rating >= 3) Then
                    objCommentFound = objComment
                End If
            Next

            photoItemIndex = 0
            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "COMMENTCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.CommentCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LASTCOMMENTDISPLAYNAME"
                            If (objCommentFound IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objCommentFound.Name
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "LASTCOMMENTRATING"
                            If (objCommentFound IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = (objCommentFound.Rating * 20).ToString()
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "LASTCOMMENTBODY"
                            If (objCommentFound IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objCommentFound.Comment
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "LASTCOMMENTBODYDATA"
                            If (objCommentFound IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = "<![CDATA[" & objCommentFound.Comment & "]]>"
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "CUSTOMFAX"
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.Name.ToLower() = "contact") Then
                                    Dim val As String = GetFieldValue(objCustomField, objProperty, False, False)

                                    For Each v As String In val.Split("<br />")
                                        If (v.ToLower().Contains("fax:")) Then
                                            If (v.ToLower().Contains("fax:")) Then
                                                v = v.ToLower().Split("fax:")(1)
                                                v = "F" & v
                                            End If
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                            objLiteral.Text = v.ToLower().Replace("fax:", "").TrimStart(" "c)
                                            objPlaceHolder.Add(objLiteral)
                                            Exit For
                                        End If
                                    Next
                                End If
                            Next

                        Case "CUSTOMPHONE"
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.Name.ToLower() = "contact") Then
                                    Dim val As String = GetFieldValue(objCustomField, objProperty, False, False)

                                    For Each v As String In val.Split("<br />")
                                        If (v.ToLower().StartsWith("tel:")) Then
                                            If (v.ToLower().Contains("fax:")) Then
                                                v = v.ToLower().Split("fax:")(0)
                                            End If
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                            objLiteral.Text = v.ToLower().Replace("tel: ", "")
                                            objPlaceHolder.Add(objLiteral)
                                            Exit For
                                        End If
                                    Next
                                End If
                            Next

                        Case "CUSTOMFIELDS"
                            Dim i As Integer = 0
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.IsHidden = False) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                    If (GetFieldValue(objCustomField, objProperty, False, True) <> "") Then
                                        objLiteral.Text = HttpContext.Current.Server.HtmlEncode(GetFieldValue(objCustomField, objProperty, True, False) & "<BR>")
                                        objPlaceHolder.Add(objLiteral)
                                    End If
                                    i = i + 1
                                End If
                            Next

                        Case "DATECREATED"
                            If (objProperty.DateCreated <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.DateCreated.ToUniversalTime.ToString("r")
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEMODIFIED"
                            If (objProperty.DateModified <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.DateModified.ToUniversalTime.ToString("r")
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEPUBLISHED"
                            If (objProperty.DatePublished <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = (objProperty.DatePublished.ToUniversalTime.ToString("r"))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEEXPIRED"
                            If (objProperty.DateExpired <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = (objProperty.DateExpired.ToUniversalTime.ToString("r"))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DISPLAYNAME"
                            If Not (Agent(objProperty.AuthorID) Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = HtmlEncode(Agent(objProperty.AuthorID).DisplayName)
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "EMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.Email)
                            objPlaceHolder.Add(objLiteral)

                        Case "FEATURED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.IsFeatured.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "FULLNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(objProperty.DisplayName)
                            objPlaceHolder.Add(objLiteral)

                        Case "HASCOMMENTS"
                            If (objProperty.CommentCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCOMMENTS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCOMMENTS"
                            ' Do Nothing

                        Case "HASCOORDINATES"
                            If (objProperty.Latitude = Null.NullDouble And objProperty.Longitude = Null.NullDouble) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCOORDINATES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCOORDINATES"
                            ' Do Nothing

                        Case "HASNOCOORDINATES"
                            If (objProperty.Latitude <> Null.NullDouble And objProperty.Longitude <> Null.NullDouble) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOCOORDINATES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOCOORDINATES"
                            ' Do Nothing

                        Case "HASREVIEWS"
                            If (objProperty.ReviewCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASREVIEWS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASREVIEWS"
                            ' Do Nothing

                        Case "HASNOREVIEWS"
                            If (objProperty.ReviewCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOREVIEWS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNOREVIEWS"
                            ' Do Nothing

                        Case "HITS"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.ViewCount.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "LINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetExternalLink(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=View", _propertySettings.SEOPropertyID & "=" & objProperty.PropertyID.ToString()))
                            objPlaceHolder.Add(objLiteral)

                        Case "LATITUDE"
                            If (objProperty.Latitude <> Null.NullDouble) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.Latitude.ToString().Replace(",", ".")
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "LONGITUDE"
                            If (objProperty.Longitude <> Null.NullDouble) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.Longitude.ToString().Replace(",", ".")
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PHOTOLINKSMALL"
                            If Not (objProperty.FirstPhoto Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                Dim width As Integer = GetPhotoWidth(objProperty.FirstPhoto, ThumbnailType.Small)
                                Dim height As Integer = GetPhotoHeight(objProperty.FirstPhoto, ThumbnailType.Small)
                                objLiteral.Text = HttpContext.Current.Server.HtmlEncode(GetExternalLink(GetPhotoPath(objProperty.FirstPhoto, width, height, False)))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PHOTOLINKMEDIUM"
                            If Not (objProperty.FirstPhoto Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                Dim width As Integer = GetPhotoWidth(objProperty.FirstPhoto, ThumbnailType.Medium)
                                Dim height As Integer = GetPhotoHeight(objProperty.FirstPhoto, ThumbnailType.Medium)
                                objLiteral.Text = HttpContext.Current.Server.HtmlEncode(GetExternalLink(GetPhotoPath(objProperty.FirstPhoto, width, height, False)))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PHOTOLINKLARGE"
                            If Not (objProperty.FirstPhoto Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                Dim width As Integer = GetPhotoWidth(objProperty.FirstPhoto, ThumbnailType.Large)
                                Dim height As Integer = GetPhotoHeight(objProperty.FirstPhoto, ThumbnailType.Large)
                                objLiteral.Text = HttpContext.Current.Server.HtmlEncode(GetExternalLink(GetPhotoPath(objProperty.FirstPhoto, width, height, False)))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PHOTOS"
                            If (objProperty.PhotoCount > 0) Then
                                Dim objPhotoController As New PhotoController
                                Dim objPhotos As ArrayList = objPhotoController.List(objProperty.PropertyID)
                                For Each objPhoto As PhotoInfo In objPhotos
                                    ProcessPhoto(objPlaceHolder, objLayoutPhoto.Tokens, objPhoto, iPtr)
                                Next
                            End If

                        Case "PORTALID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (_portalSettings.PortalId.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(_portalSettings.PortalName)
                            objPlaceHolder.Add(objLiteral)

                        Case "PRINTLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (ResolveUrl("~/DesktopModules/PropertyAgent/Print.aspx?ModuleID=" & _moduleID.ToString() & "&PortalID=" & _portalID.ToString() & "&TabID=" & _tabID.ToString() & "&PropertyID=" & objProperty.PropertyID.ToString() & "&Template=" & _propertySettings.Template))
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.PropertyID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(_propertySettings.PropertyLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(_propertySettings.PropertyPluralLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPELABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(_propertySettings.PropertyTypeLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPEPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(_propertySettings.PropertyTypePluralLabel)
                            objPlaceHolder.Add(objLiteral)

                        Case "RATINGVALUE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.Rating.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "RATINGCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.RatingCount.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "TEMPLATEPATH"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(_portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Templates/" & _propertySettings.Template & "/")
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(objProperty.PropertyTypeName.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.PropertyTypeID)
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEDESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(objProperty.PropertyTypeDescription)
                            objPlaceHolder.Add(objLiteral)

                        Case "URL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (System.Web.HttpContext.Current.Request.Url.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "URLENCODED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (System.Web.HttpContext.Current.Server.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()))
                            objPlaceHolder.Add(objLiteral)

                        Case "USERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = HtmlEncode(objProperty.Username)
                            objPlaceHolder.Add(objLiteral)

                        Case "USERID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (objProperty.AuthorID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            Dim isRendered As Boolean = False

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo
                                Dim isLink As Boolean = False

                                If (field.EndsWith("link")) Then
                                    Dim fieldWithoutLink As String = field.Remove(field.Length - 4, 4)
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = fieldWithoutLink.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                            isLink = True
                                        End If
                                    Next
                                End If

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If
                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, isLink)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        objLiteral.Text = HtmlEncode(fieldValue)
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOMDATA:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo
                                Dim isLink As Boolean = False

                                If (field.EndsWith("link")) Then
                                    Dim fieldWithoutLink As String = field.Remove(field.Length - 4, 4)
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = fieldWithoutLink.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                            isLink = True
                                        End If
                                    Next
                                End If

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If
                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, isLink)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        objLiteral.Text = "<![CDATA[" & fieldValue & "]]>"
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOMREPLACE:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(14, layoutArray(iPtr + 1).Length - 14).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo

                                Dim replace As String = Null.NullString
                                If (field.IndexOf(":"c) <> -1) Then
                                    replace = field.Split(":"c)(1)
                                    field = field.Split(":"c)(0)
                                End If

                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, False)
                                        If (replace <> Null.NullString) Then
                                            fieldValue = fieldValue.Replace(replace, "")
                                        End If
                                        objLiteral.Text = HtmlEncode(fieldValue)
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATECREATED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                If (objProperty.DateCreated <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DateCreated.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATECOMMENT:")) Then
                                If (objCommentFound IsNot Nothing) Then
                                    Dim formatExpression As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                    If (objCommentFound.CreateDate <> Null.NullDate) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                        objLiteral.Text = objCommentFound.CreateDate.ToString(formatExpression)
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                    End If
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATEEXPIRED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                If (objProperty.DateExpired <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DateExpired.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATEMODIFIED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(13, layoutArray(iPtr + 1).Length - 13)
                                If (objProperty.DateModified <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DateModified.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATEPUBLISHED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(14, layoutArray(iPtr + 1).Length - 14)
                                If (objProperty.DatePublished <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DatePublished.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EXPRESSION:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                Dim params As String() = field.Split(":"c)

                                If (params.Length <> 3) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Dim customField As String = params(0)
                                Dim customExpression As String = params(1)
                                Dim customValue As String = params(2)

                                Dim fieldValue As String = ""

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                        If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            fieldValue = GetFieldValue(objCustomField, objProperty, False, False)
                                        End If
                                    End If
                                Next

                                Dim isValid = False
                                Select Case customExpression
                                    Case "="
                                        If (customValue.ToLower() = fieldValue.ToLower()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                    Case "!="
                                        If (customValue.ToLower() <> fieldValue.ToLower()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                    Case "<"
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) < Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case "<="
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) <= Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case ">"
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) > Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case ">="
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) >= Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                End Select

                                If (isValid = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/EXPRESSION:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EXPRESSIONTYPE:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(15, layoutArray(iPtr + 1).Length - 15)

                                Dim params As String() = field.Split(":"c)

                                If (params.Length <> 2) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Dim propertyExpression As String = params(0)
                                Dim propertyType As String = params(1)

                                Dim isValid = False
                                Select Case propertyExpression
                                    Case "="
                                        If (propertyType.ToLower().ToString() = objProperty.PropertyTypeName.ToLower().ToString()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                End Select

                                If (isValid = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/EXPRESSIONTYPE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EXPRESSIONTYPEID:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(17, layoutArray(iPtr + 1).Length - 17)

                                Dim params As String() = field.Split(":"c)

                                If (params.Length <> 2) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Dim propertyExpression As String = params(0)
                                Dim propertyTypeID As String = params(1)

                                Dim isValid = False
                                Select Case propertyExpression
                                    Case "="
                                        If (propertyTypeID.ToString() = objProperty.PropertyTypeID.ToString()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                End Select

                                If (isValid = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/EXPRESSIONTYPEID:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASNOVALUE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim fieldValue As String = GetFieldValue(objCustomField, objProperty, False, False)
                                            If (fieldValue.Trim() <> "") Then
                                                Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                                While (iPtr < layoutArray.Length - 1)
                                                    If (layoutArray(iPtr + 1) = endToken) Then
                                                        Exit While
                                                    End If
                                                    iPtr = iPtr + 1
                                                End While
                                            End If
                                        End If
                                    End If
                                Next
                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASNOVALUE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASVALUE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim fieldValue As String = GetFieldValue(objCustomField, objProperty, False, True)
                                            If (fieldValue.Trim() = "") Then
                                                Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                                While (iPtr < layoutArray.Length - 1)
                                                    If (layoutArray(iPtr + 1) = endToken) Then
                                                        Exit While
                                                    End If
                                                    iPtr = iPtr + 1
                                                End While
                                            End If
                                        End If
                                    End If
                                Next

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASVALUE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("SPLITCUSTOM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo

                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then

                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, False)

                                        For Each val As String In fieldValue.Split(","c)
                                            Dim objLiteral As New Literal
                                            objLiteral.Text = "<attr name=""Serving"">" & val.Trim() & "</attr>"
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                        Next

                                        i = i + 1
                                    End If

                                End If
                                isRendered = True

                            End If


                            If (isRendered = False) Then
                                Dim objLiteralOther As New Literal
                                objLiteralOther.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                                objLiteralOther.EnableViewState = False
                                objPlaceHolder.Add(objLiteralOther)
                            End If

                    End Select
                End If
            Next

        End Sub

#End Region

#Region " RSS READER "

        Public Sub ProcessRssReaderItem(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objRssItem As RssInfo, ByVal key As String)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString().Replace("~", DotNetNuke.Common.Globals.ApplicationPath)))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "DESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-RssReader-" & key & "-" & iPtr.ToString())
                            objLiteral.Text = objRssItem.Description.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "LINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-RssReader-" & key & "-" & iPtr.ToString())
                            objLiteral.Text = objRssItem.Link.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TITLE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-RssReader-" & key & "-" & iPtr.ToString())
                            objLiteral.Text = objRssItem.Title.ToString()
                            objPlaceHolder.Add(objLiteral)

                    End Select
                End If
            Next

        End Sub

#End Region

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
                            ' Do Nothing.

                    End Select
                End If
            Next

        End Sub

        Public Sub ProcessItem(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objProperty As PropertyInfo, ByVal objCustomFields As List(Of CustomFieldInfo))

            ProcessItem(objPlaceHolder, layoutArray, objProperty, objCustomFields, Nothing, False)

        End Sub

        Public Sub ProcessItem(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objProperty As PropertyInfo, ByVal objCustomFields As List(Of CustomFieldInfo), ByVal objMailCustomFields As List(Of ContactFieldInfo), ByVal isPrint As Boolean)

            _siblingSearchProperties = Nothing
            _siblingTypeProperties = Nothing

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "APPROVALLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetExternalLink(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=EditProperty", _propertySettings.SEOPropertyID & "=" & objProperty.PropertyID.ToString()))
                            objPlaceHolder.Add(objLiteral)

                        Case "BROKERID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.BrokerID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "BROKERDISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.BrokerDisplayName
                            objPlaceHolder.Add(objLiteral)

                        Case "BROKEREMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.BrokerEmail
                            objPlaceHolder.Add(objLiteral)

                        Case "BROKERUSERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.BrokerUsername
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.CommentCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "COMMENTS"
                            Dim objHeader As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Comment_Header_Html)
                            Dim objItem As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Comment_Item_Html)
                            Dim objSeparator As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Comment_Separator_Html)
                            Dim objFooter As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Comment_Footer_Html)

                            Dim objCommentController As New CommentController
                            Dim objComments As ArrayList = objCommentController.List(objProperty.PropertyID)

                            ProcessHeaderFooter(objPlaceHolder, objHeader.Tokens)

                            Dim count As Integer = 0
                            For Each objComment As CommentInfo In objComments
                                ProcessComment(objPlaceHolder, objItem.Tokens, objComment, isPrint)
                                If (count + 1 < objComments.Count) Then
                                    ProcessHeaderFooter(objPlaceHolder, objSeparator.Tokens)
                                End If
                                count = count + 1
                            Next

                            ProcessHeaderFooter(objPlaceHolder, objFooter.Tokens)

                        Case "COMMENTFORM"
                            If Not isPrint Then
                                Dim objCommentForm As CommentForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/CommentForm.ascx"), CommentForm)
                                objCommentForm.CurrentProperty = objProperty
                                objCommentForm.ModuleID = _moduleID
                                objCommentForm.TabID = _tabID
                                objCommentForm.ModuleKey = _moduleKey
                                objCommentForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objCommentForm)
                            End If

                        Case "COMMENTFORMRATING"
                            If Not isPrint Then
                                Dim objCommentForm As CommentForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/CommentForm.ascx"), CommentForm)
                                objCommentForm.CurrentProperty = objProperty
                                objCommentForm.ModuleID = _moduleID
                                objCommentForm.TabID = _tabID
                                objCommentForm.ModuleKey = _moduleKey
                                objCommentForm.EnableRatings = True
                                objCommentForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objCommentForm)
                            End If

                        Case "CONTACTFORM"
                            If Not isPrint Then
                                Dim objContactForm As ContactForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/ContactForm.ascx"), ContactForm)
                                objContactForm.CurrentProperty = objProperty
                                objContactForm.ModuleID = _moduleID
                                objContactForm.TabID = _tabID
                                objContactForm.ModuleKey = _moduleKey
                                objContactForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objContactForm)
                            End If

                        Case "CURRENCY"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.Currency.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "CURRENCYSELECTOR"
                            If Not isPrint Then
                                Dim objCurrencySelector As CurrencySelector = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/CurrencySelector.ascx"), CurrencySelector)
                                objCurrencySelector.CurrentProperty = objProperty
                                objCurrencySelector.ModuleID = _moduleID
                                objCurrencySelector.TabID = _tabID
                                objCurrencySelector.ModuleKey = _moduleKey
                                objCurrencySelector.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objCurrencySelector)
                            End If

                        Case "CURRENTUSERID"
                            Dim objUser As UserInfo = UserController.GetCurrentUserInfo()
                            If (objUser IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objUser.UserID.ToString()
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "CUSTOMFIELDS"
                            Dim i As Integer = 0
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.IsHidden = False) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                    objLiteral.Text = GetFieldValue(objCustomField, objProperty, True, False) & "<BR>"
                                    If (objLiteral.Text <> "") Then
                                        objPlaceHolder.Add(objLiteral)
                                    End If
                                    i = i + 1
                                End If
                            Next

                        Case "CUSTOMFIELDSFEATURED"
                            Dim i As Integer = 0
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.IsFeatured AndAlso objCustomField.IsHidden = False) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                    objLiteral.Text = GetFieldValue(objCustomField, objProperty, True, False) & "<BR>"
                                    If (objLiteral.Text <> "") Then
                                        objPlaceHolder.Add(objLiteral)
                                    End If
                                    i = i + 1
                                End If
                            Next

                        Case "CUSTOMFIELDSLISTING"
                            Dim i As Integer = 0
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.IsInListing AndAlso objCustomField.IsHidden = False) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                    objLiteral.Text = GetFieldValue(objCustomField, objProperty, True, False) & "<BR>"
                                    If (objLiteral.Text <> "") Then
                                        objPlaceHolder.Add(objLiteral)
                                    End If
                                    i = i + 1
                                End If
                            Next

                        Case "DATECREATED"
                            If (objProperty.DateCreated <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.DateCreated.ToShortDateString()
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEMODIFIED"
                            If (objProperty.DateModified <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.DateModified.ToShortDateString()
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEPUBLISHED"
                            If (objProperty.DatePublished <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.DatePublished.ToShortDateString()
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DATEEXPIRED"
                            If (objProperty.DateExpired <> Null.NullDate) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.DateExpired.ToShortDateString()
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "DISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.DisplayName
                            objPlaceHolder.Add(objLiteral)

                        Case "DISTANCE"
                            If (objProperty.Distance <> Null.NullDouble AndAlso objProperty.Distance <> 0) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                If (_propertySettings.DistanceType = Mapping.DistanceType.Miles) Then
                                    objLiteral.Text = (objProperty.Distance * 0.621371192).ToString("#.00")
                                Else
                                    objLiteral.Text = objProperty.Distance.ToString("#.00")
                                End If
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "EDIT"
                            If _isEditable Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objHyperLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=EditProperty", _propertySettings.SEOPropertyID & "=" & objProperty.PropertyID.ToString(), "ReturnUrl=" & HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.RawUrl))
                                objHyperLink.ImageUrl = "~/images/edit.gif"
                                objHyperLink.EnableViewState = False
                                objPlaceHolder.Add(objHyperLink)
                            End If

                        Case "EDITLINK"
                            If _isEditable Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=EditProperty", _propertySettings.SEOPropertyID & "=" & objProperty.PropertyID.ToString(), "ReturnUrl=" & HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.RawUrl))
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "EMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.Email
                            objPlaceHolder.Add(objLiteral)

                        Case "FIRSTTYPELINK"
                            Dim objPropertyController As New PropertyController
                            Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(_moduleID, objProperty.PropertyTypeID, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, False, _propertySettings.ListingSortBy, _propertySettings.ListingSortByCustomField, _propertySettings.ListingSortDirection, _propertySettings.ListingSortBy2, _propertySettings.ListingSortByCustomField2, _propertySettings.ListingSortDirection2, _propertySettings.ListingSortBy3, _propertySettings.ListingSortByCustomField3, _propertySettings.ListingSortDirection3, Null.NullString, Null.NullString, 0, 1, 1, _propertySettings.ListingBubbleFeatured, _propertySettings.ListingSearchSubTypes, Null.NullInteger, Null.NullInteger, Null.NullDouble, Null.NullDouble, Null.NullDate, Null.NullString, Null.NullInteger)

                            If (objProperties.Count > 0) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = GetExternalLink(GetPropertyLink(objProperties(0), objCustomFields))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "FULLNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.DisplayName
                            objPlaceHolder.Add(objLiteral)

                        Case "GOOGLEAPIKEY"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.MapKey
                            objPlaceHolder.Add(objLiteral)

                        Case "HASAGENT"
                            If Agent(objProperty.AuthorID) Is Nothing Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASAGENT") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASAGENT"
                            ' Do Nothing

                        Case "HASBROKER"
                            If Broker(objProperty.BrokerID) Is Nothing Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASBROKER") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASBROKER"
                            ' Do Nothing

                        Case "HASCOMMENTS"
                            If (objProperty.CommentCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCOMMENTS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASCOMMENTS"
                            ' Do Nothing

                        Case "HASCOORDINATES"
                            If (objProperty.Latitude = Null.NullDouble And objProperty.Longitude = Null.NullDouble) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASCOORDINATES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASCOORDINATES"
                            ' Do Nothing

                        Case "HASNOCOORDINATES"
                            If (objProperty.Latitude <> Null.NullDouble And objProperty.Longitude <> Null.NullDouble) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOCOORDINATES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASNOCOORDINATES"
                            ' Do Nothing

                        Case "HASDISTANCE"
                            If (objProperty.Distance = Null.NullDouble OrElse objProperty.Distance = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASDISTANCE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASDISTANCE"
                            ' Do Nothing

                        Case "HASMODIFIED"
                            If Broker(objProperty.BrokerID) Is Nothing Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASMODIFIED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASMODIFIED"
                            ' Do Nothing

                        Case "HASNOCOMMENTS"
                            If (objProperty.CommentCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOCOMMENTS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNOCOMMENTS"
                            ' Do Nothing

                        Case "HASNOAGENT"
                            If Not Agent(objProperty.AuthorID) Is Nothing Then
                                ' Property Agent = AuthorId (null allowed!)
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOAGENT") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNOAGENT"
                            ' Do Nothing

                        Case "HASIMAGES"
                            If (objProperty.PhotoCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASIMAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASIMAGES"
                            ' Do Nothing

                        Case "HASIMAGESENABLED"
                            If (_propertySettings.ImagesEnabled = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASIMAGESENABLED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASIMAGESENABLED"
                            ' Do Nothing

                        Case "HASNEXTSEARCH"
                            If (NextSearchProperty(objProperty.PropertyID, objProperty.PropertyTypeID) Is Nothing) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNEXTSEARCH") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNEXTSEARCH"
                            ' Do Nothing

                        Case "HASNEXTTYPE"
                            If (NextTypeProperty(objProperty.PropertyID, objProperty.PropertyTypeID) Is Nothing) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNEXTTYPE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNEXTTYPE"
                            ' Do Nothing

                        Case "HASNOIMAGES"
                            If (objProperty.PhotoCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOIMAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNOIMAGES"
                            ' Do Nothing

                        Case "HASPREVIOUSSEARCH"
                            If (PreviousSearchProperty(objProperty.PropertyID, objProperty.PropertyTypeID) Is Nothing) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASPREVIOUSSEARCH") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASPREVIOUSSEARCH"
                            ' Do Nothing

                        Case "HASPREVIOUSTYPE"
                            If (PreviousTypeProperty(objProperty.PropertyID, objProperty.PropertyTypeID) Is Nothing) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASPREVIOUSTYPE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASPREVIOUSTYPE"
                            ' Do Nothing

                        Case "HASRATING"
                            If (objProperty.RatingCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASRATING") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASRATING"
                            ' Do Nothing

                        Case "HASNORATING"
                            If (objProperty.RatingCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNORATING") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNORATING"
                            ' Do Nothing

                        Case "HASREVIEWS"
                            If (objProperty.ReviewCount = 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASREVIEWS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASREVIEWS"
                            ' Do Nothing

                        Case "HASNOREVIEWS"
                            If (objProperty.ReviewCount > 0) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNOREVIEWS") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNOREVIEWS"
                            ' Do Nothing

                        Case "HITS"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.ViewCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "ISEDITABLE"
                            If (_isEditable = False) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISEDITABLE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISEDITABLE"
                            ' Do Nothing

                        Case "ISEVEN"
                            If (ListingIndex Mod 2) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISEVEN") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISEVEN"
                            ' Do Nothing

                        Case "ISODD"
                            If (ListingIndex Mod 2 <> 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISODD") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISODD"
                            ' Do Nothing

                        Case "ISFEATURED"
                            If objProperty.IsFeatured = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISFEATURED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISFEATURED"
                            ' Do Nothing

                        Case "ISNOTFEATURED"
                            If objProperty.IsFeatured = True Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTFEATURED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTFEATURED"
                            ' Do Nothing

                        Case "ISFIRST"
                            If _listingIndex > 1 Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISFIRST") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISFIRST"
                            ' Do Nothing

                        Case "ISMOBILEDEVICE"
                            If IsMobileBrowser() = False Then
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
                            If IsMobileBrowser() = True Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTMOBILEDEVICE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTMOBILEDEVICE"
                            ' Do Nothing

                        Case "ISNOTFIRST"
                            If _listingIndex <= 1 Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTFIRST") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTFIRST"
                            ' Do Nothing

                        Case "ISOWNER"
                            Dim isOwner As Boolean = False
                            If (_objPage.User.Identity.IsAuthenticated) Then
                                If (UserController.GetCurrentUserInfo.UserID = objProperty.AuthorID) Then
                                    isOwner = True
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

                        Case "LATITUDE"
                            If (objProperty.Latitude <> Null.NullDouble) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.Latitude.ToString().Replace(",", ".")
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "LONGITUDE"
                            If (objProperty.Longitude <> Null.NullDouble) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.Longitude.ToString().Replace(",", ".")
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "LINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetExternalLink(GetPropertyLink(objProperty, objCustomFields))
                            objPlaceHolder.Add(objLiteral)

                        Case "ITEMINDEX"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = (_listingIndex + 1).ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "MAP"
                            If (objProperty.Latitude <> Null.NullDouble And objProperty.Longitude <> Null.NullDouble) Then
                                Dim objImage As New Image
                                objImage.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objImage.AlternateText = ""
                                'objImage.ImageUrl = "https://maps.google.com/staticmap?center=" & objProperty.Latitude.ToString().Replace(","c, ".") & "," & objProperty.Longitude.ToString().Replace(","c, ".") & "&markers=" & objProperty.Latitude.ToString().Replace(","c, ".") & "," & objProperty.Longitude.ToString().Replace(","c, ".") & ",red&zoom=" & _propertySettings.MapZoom.ToString() & "&size=" & _propertySettings.MapWidth.ToString() & "x" & _propertySettings.MapHeight.ToString() & "&key=" & _propertySettings.MapKey
                                objImage.ImageUrl = "https://maps.google.com/maps/api/staticmap?center=" & objProperty.Latitude.ToString().Replace(","c, ".") & "," & objProperty.Longitude.ToString().Replace(","c, ".") & "&zoom=" & _propertySettings.MapZoom.ToString() & "&size=" & _propertySettings.MapWidth.ToString() & "x" & _propertySettings.MapHeight.ToString() & "&markers=size:mid|color:red|label:A|" & objProperty.Latitude.ToString().Replace(","c, ".") & "," & objProperty.Longitude.ToString().Replace(","c, ".") & "&sensor=false" & "&key=" & _propertySettings.MapKey
                                objPlaceHolder.Add(objImage)
                            End If

                        Case "MODIFIEDID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.ModifiedID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "MODIFIEDDISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.ModifiedDisplayName
                            objPlaceHolder.Add(objLiteral)

                        Case "MODIFIEDEMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.ModifiedEmail
                            objPlaceHolder.Add(objLiteral)

                        Case "MODIFIEDUSERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.ModifiedUsername
                            objPlaceHolder.Add(objLiteral)

                        Case "NEXTSEARCH"
                            Dim objNextProperty As PropertyInfo = NextSearchProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objNextProperty Is Nothing) Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objHyperLink.NavigateUrl = GetExternalLink(GetPropertyLink(objNextProperty, objCustomFields))
                                objHyperLink.Text = GetSharedResource("NextSearch")
                                objPlaceHolder.Add(objHyperLink)
                            End If

                        Case "NEXTSEARCHLINK"
                            Dim objNextProperty As PropertyInfo = NextSearchProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objNextProperty Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = GetExternalLink(GetPropertyLink(objNextProperty, objCustomFields))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "NEXTTYPE"
                            Dim objNextProperty As PropertyInfo = NextTypeProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objNextProperty Is Nothing) Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objHyperLink.NavigateUrl = GetExternalLink(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=View", _propertySettings.SEOPropertyID & "=" & objNextProperty.PropertyID.ToString()))
                                objHyperLink.Text = GetSharedResource("NextType")
                                objPlaceHolder.Add(objHyperLink)
                            End If

                        Case "NEXTTYPELINK"
                            Dim objNextProperty As PropertyInfo = NextTypeProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objNextProperty Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = GetExternalLink(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=View", _propertySettings.SEOPropertyID & "=" & objNextProperty.PropertyID.ToString()))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PDFLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetExternalLink(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=PdfRender", _propertySettings.SEOPropertyID & "=" & objProperty.PropertyID.ToString()))
                            objPlaceHolder.Add(objLiteral)

                        Case "PHOTOCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.PhotoCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PHOTOSMALL"
                            ProcessPhotoToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, False)

                        Case "PHOTOSMALLCROPPED"
                            ProcessPhotoToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, True)

                        Case "PHOTOMEDIUM"
                            ProcessPhotoToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, False)

                        Case "PHOTOMEDIUMCROPPED"
                            ProcessPhotoToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, True)

                        Case "PHOTOLARGE"
                            ProcessPhotoToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, False)

                        Case "PHOTOLARGECROPPED"
                            ProcessPhotoToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, True)

                        Case "PHOTOHEIGHTSMALL"
                            ProcessPhotoHeightToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, False)

                        Case "PHOTOHEIGHTSMALLCROPPED"
                            ProcessPhotoHeightToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, True)

                        Case "PHOTOHEIGHTMEDIUM"
                            ProcessPhotoHeightToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, False)

                        Case "PHOTOHEIGHTMEDIUMCROPPED"
                            ProcessPhotoHeightToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, True)

                        Case "PHOTOHEIGHTLARGE"
                            ProcessPhotoHeightToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, False)

                        Case "PHOTOHEIGHTLARGECROPPED"
                            ProcessPhotoHeightToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, True)

                        Case "PHOTOLINKSMALL"
                            ProcessPhotoLinkToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, False)

                        Case "PHOTOLINKSMALLCROPPED"
                            ProcessPhotoLinkToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, True)

                        Case "PHOTOLINKMEDIUM"
                            ProcessPhotoLinkToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, False)

                        Case "PHOTOLINKMEDIUMCROPPED"
                            ProcessPhotoLinkToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, True)

                        Case "PHOTOLINKLARGE"
                            ProcessPhotoLinkToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, False)

                        Case "PHOTOLINKLARGECROPPED"
                            ProcessPhotoLinkToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, True)

                        Case "PHOTOTITLE"
                            If Not (objProperty.FirstPhoto Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.FirstPhoto.Title
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PHOTOURL"
                            If Not (objProperty.FirstPhoto Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = objProperty.FirstPhoto.ExternalUrl
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PHOTOWIDTHSMALL"
                            ProcessPhotoWidthToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, False)

                        Case "PHOTOWIDTHSMALLCROPPED"
                            ProcessPhotoWidthToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Small, True)

                        Case "PHOTOWIDTHMEDIUM"
                            ProcessPhotoWidthToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, False)

                        Case "PHOTOWIDTHMEDIUMCROPPED"
                            ProcessPhotoWidthToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Medium, True)

                        Case "PHOTOWIDTHLARGE"
                            ProcessPhotoWidthToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, False)

                        Case "PHOTOWIDTHLARGECROPPED"
                            ProcessPhotoWidthToken(objPlaceHolder, objProperty.FirstPhoto, iPtr, ThumbnailType.Large, True)
                        Case "PHOTOURL"

                        Case "PHOTOS"
                            Dim objPhotoController As New PhotoController
                            Dim objPhotos As ArrayList = objPhotoController.List(objProperty.PropertyID)

                            If (objPhotos.Count > 0) Then

                                Dim objLayoutController As New LayoutController(_portalSettings, _propertySettings, _objPage, _objControl, _isEditable, _tabID, _moduleID, _moduleKey)
                                Dim objLayoutFirst As LayoutInfo = objLayoutController.GetLayout(_propertySettings.Template, LayoutType.Photo_First_Html)
                                _objLayoutPhotoItem = objLayoutController.GetLayout(_propertySettings.Template, LayoutType.Photo_Item_Html)

                                If (objLayoutFirst.Template <> "") Then
                                    Dim objPhoto As PhotoInfo = CType(objPhotos(0), PhotoInfo)
                                    ProcessPhoto(objPlaceHolder, objLayoutFirst.Tokens, objPhoto, iPtr)
                                    objPhotos.RemoveAt(0)
                                End If

                                If (objPhotos.Count > 0) Then

                                    Dim objDataList As New System.Web.UI.WebControls.DataList
                                    Dim objHandler As New DataListItemEventHandler(AddressOf dlPhotos_ItemDataBound)
                                    AddHandler objDataList.ItemDataBound, objHandler

                                    objDataList.CellPadding = 0
                                    objDataList.CellSpacing = 0

                                    objDataList.RepeatColumns = _propertySettings.ImagesItemsPerRow
                                    objDataList.RepeatDirection = RepeatDirection.Horizontal

                                    objDataList.Width = Unit.Percentage(100)
                                    objDataList.ItemStyle.HorizontalAlign = HorizontalAlign.Center

                                    objDataList.DataSource = objPhotos
                                    objDataList.DataBind()

                                    objPlaceHolder.Add(objDataList)

                                End If
                            End If

                        Case "PHOTOS2"
                            Dim objPhotoController As New PhotoController
                            Dim objPhotos As ArrayList = objPhotoController.List(objProperty.PropertyID)

                            If (objPhotos.Count > 0) Then

                                Dim objLayoutController As New LayoutController(_portalSettings, _propertySettings, _objPage, _objControl, _isEditable, _tabID, _moduleID, _moduleKey)
                                Dim objLayoutFirst As LayoutInfo = objLayoutController.GetLayout(_propertySettings.Template, LayoutType.Photo_First_Html)
                                _objLayoutPhotoItem = objLayoutController.GetLayout(_propertySettings.Template, LayoutType.Photo_Item_Html)

                                If (objLayoutFirst.Template <> "") Then
                                    Dim objPhoto As PhotoInfo = CType(objPhotos(0), PhotoInfo)
                                    ProcessPhoto(objPlaceHolder, objLayoutFirst.Tokens, objPhoto, iPtr)
                                    objPhotos.RemoveAt(0)
                                End If

                                If (objPhotos.Count > 0) Then

                                    Dim objRepeater As New System.Web.UI.WebControls.Repeater
                                    Dim objHandler As New RepeaterItemEventHandler(AddressOf rptPhotos_ItemDataBound)
                                    AddHandler objRepeater.ItemDataBound, objHandler

                                    objRepeater.DataSource = objPhotos
                                    objRepeater.DataBind()

                                    objPlaceHolder.Add(objRepeater)

                                End If
                            End If

                        Case "PORTALID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _portalSettings.PortalId.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PORTALNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _portalSettings.PortalName
                            objPlaceHolder.Add(objLiteral)

                        Case "PREVIOUSSEARCH"
                            Dim objPreviousProperty As PropertyInfo = PreviousSearchProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objPreviousProperty Is Nothing) Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objHyperLink.NavigateUrl = GetExternalLink(GetPropertyLink(objPreviousProperty, objCustomFields))
                                objHyperLink.Text = GetSharedResource("PreviousSearch")
                                objPlaceHolder.Add(objHyperLink)
                            End If

                        Case "PREVIOUSSEARCHLINK"
                            Dim objPreviousProperty As PropertyInfo = PreviousSearchProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objPreviousProperty Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = GetExternalLink(GetPropertyLink(objPreviousProperty, objCustomFields))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PREVIOUSTYPE"
                            Dim objPreviousProperty As PropertyInfo = PreviousTypeProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objPreviousProperty Is Nothing) Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objHyperLink.NavigateUrl = GetExternalLink(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=View", _propertySettings.SEOPropertyID & "=" & objPreviousProperty.PropertyID.ToString()))
                                objHyperLink.Text = GetSharedResource("PreviousType")
                                objPlaceHolder.Add(objHyperLink)
                            End If

                        Case "PREVIOUSTYPELINK"
                            Dim objPreviousProperty As PropertyInfo = PreviousTypeProperty(objProperty.PropertyID, objProperty.PropertyTypeID)
                            If Not (objPreviousProperty Is Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = GetExternalLink(NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=View", _propertySettings.SEOPropertyID & "=" & objPreviousProperty.PropertyID.ToString()))
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PRINT"
                            Dim objPrintLink As New HyperLink
                            objPrintLink.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objPrintLink.NavigateUrl = "~/DesktopModules/PropertyAgent/Print.aspx?ModuleID=" & _moduleID.ToString() & "&PortalID=" & _portalID.ToString() & "&TabID=" & _tabID.ToString() & "&PropertyID=" & objProperty.PropertyID.ToString() & "&Template=" & _propertySettings.Template
                            objPrintLink.ImageUrl = "~/images/print.gif"
                            objPrintLink.EnableViewState = False
                            objPrintLink.Target = "_blank"
                            objPrintLink.Attributes.Add("rel", "nofollow")
                            objPlaceHolder.Add(objPrintLink)

                        Case "PRINTLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = ResolveUrl("~/DesktopModules/PropertyAgent/Print.aspx?ModuleID=" & _moduleID.ToString() & "&PortalID=" & _portalID.ToString() & "&TabID=" & _tabID.ToString() & "&PropertyID=" & objProperty.PropertyID.ToString() & "&Template=" & _propertySettings.Template)
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.PropertyID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyPluralLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPELABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyTypeLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYTYPEPLURALLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _propertySettings.PropertyTypePluralLabel
                            objPlaceHolder.Add(objLiteral)

                        Case "PROPERTYID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.PropertyID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "RATING"
                            Dim rating As Integer = 0
                            Dim ratingPercentage As Integer = 0
                            If (objProperty.Rating <> Null.NullDouble) Then
                                rating = Math.Round(objProperty.Rating)
                                ratingPercentage = objProperty.Rating * 20
                            End If

                            Dim hasRated As Boolean = False
                            If (_objPage.User.Identity.IsAuthenticated) Then
                                Dim objRatingController As New RatingController()
                                Dim objRating As RatingInfo = objRatingController.Get(objProperty.PropertyID, UserController.GetCurrentUserInfo.UserID)
                                hasRated = Not (objRating Is Nothing)
                            Else
                                If Not (_objPage.Request.Cookies("PropertyAgent-Rating-" & objProperty.PropertyID.ToString()) Is Nothing) Then
                                    hasRated = True
                                End If
                            End If

                            Dim script As String = "" _
                                & "<ul id=""rating-" & objProperty.PropertyID.ToString() & "-" & _moduleKey & """ class=""pa-star-rating" & IIf(hasRated, " pa-has-rated", "") & """>" _
                                & " <li class=""pa-current-rating"" style=""width:" & ratingPercentage.ToString() & "%;"">Currently " & rating.ToString() & "/5 Stars.</li>" _
                                & " <li><a href=""#"" title=""1 star out of 5"" class=""pa-one-star"">1</a></li>" _
                                & " <li><a href=""#"" title=""2 stars out of 5"" class=""pa-two-stars"">2</a></li>" _
                                & " <li><a href=""#"" title=""3 stars out of 5"" class=""pa-three-stars"">3</a></li>" _
                                & " <li><a href=""#"" title=""4 stars out of 5"" class=""pa-four-stars"">4</a></li>" _
                                & " <li><a href=""#"" title=""5 stars out of 5"" class=""pa-five-stars"">5</a></li>" _
                                & "</ul>"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = script
                            objPlaceHolder.Add(objLiteral)

                            If hasRated = False Then
                                'If (_objPage.ClientScript.IsClientScriptIncludeRegistered("PropertyAgent-jQuery-Rating") = False) Then
                                '    _objPage.ClientScript.RegisterClientScriptInclude("PropertyAgent-jQuery-Rating", _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/js/rating/jquery.rating.js"))
                                'End If

                                ClientResourceManager.RegisterScript(_objPage, _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/js/rating/jquery.rating.js"))


                                Dim callbackFunctionName As String = "RateProperty_" & objProperty.PropertyID.ToString() & "_" & iPtr.ToString()
                                Dim callbackReturnFunctionName As String = "RateProperty_" & objProperty.PropertyID.ToString() & "_" & iPtr.ToString() & "_Callback"
                                _objPage.ClientScript.RegisterClientScriptBlock(Me.GetType(), callbackFunctionName, "jQuery(document).ready(function() {" & vbCrLf & "jQuery('#rating-" + objProperty.PropertyID.ToString() & "-" & _moduleKey & "').rating('" & callbackFunctionName & "', {propertyID: " & objProperty.PropertyID.ToString() & ", userID: " & UserController.GetCurrentUserInfo.UserID.ToString() & "});" & vbCrLf & "});", True)
                                _objPage.ClientScript.RegisterClientScriptBlock(Me.GetType(), callbackReturnFunctionName, "<script type=""text/javascript"">function " & callbackReturnFunctionName & "() {}</script>")

                                If (TypeOf _objControl Is PropertyAgentBase) Then
                                    CType(_objControl, PropertyAgentBase).RegisterRatingCallback(callbackFunctionName, callbackReturnFunctionName)
                                End If
                                If (TypeOf _objControl Is PropertyAgentLatestBase) Then
                                    CType(_objControl, PropertyAgentLatestBase).RegisterRatingCallback(callbackFunctionName, callbackReturnFunctionName)
                                End If
                            End If

                        Case "RATINGAVG"
                            Dim rating As Integer = 0
                            Dim ratingPercentage As Integer = 0
                            If (objProperty.Rating <> Null.NullDouble) Then
                                rating = Math.Round(objProperty.Rating)
                                ratingPercentage = objProperty.Rating * 20
                            End If

                            Dim hasRated As Boolean = True

                            Dim script As String = "" _
                                & "<ul id=""rating-" & objProperty.PropertyID.ToString() & """ class=""pa-star-rating" & IIf(hasRated, " pa-has-rated", "") & """>" _
                                & " <li class=""pa-current-rating"" style=""width:" & ratingPercentage.ToString() & "%;"">Currently " & rating.ToString() & "/5 Stars.</li>" _
                                & " <li><a href=""#"" title=""1 star out of 5"" class=""pa-one-star"">1</a></li>" _
                                & " <li><a href=""#"" title=""2 stars out of 5"" class=""pa-two-stars"">2</a></li>" _
                                & " <li><a href=""#"" title=""3 stars out of 5"" class=""pa-three-stars"">3</a></li>" _
                                & " <li><a href=""#"" title=""4 stars out of 5"" class=""pa-four-stars"">4</a></li>" _
                                & " <li><a href=""#"" title=""5 stars out of 5"" class=""pa-five-stars"">5</a></li>" _
                                & "</ul>"

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = script
                            objPlaceHolder.Add(objLiteral)

                        Case "RATINGVALUE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.Rating.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "RATINGCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.RatingCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "REVIEWCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.ReviewCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "REVIEWS"
                            Dim objHeader As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Review_Header_Html)
                            Dim objItem As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Review_Item_Html)
                            Dim objSeparator As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Review_Separator_Html)
                            Dim objFooter As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.Review_Footer_Html)

                            Dim objReviewController As New ReviewController
                            Dim objReviews As List(Of ReviewInfo) = objReviewController.List(Null.NullInteger, objProperty.PropertyID, True)

                            ProcessHeaderFooter(objPlaceHolder, objHeader.Tokens)

                            Dim count As Integer = 0
                            For Each objReview As ReviewInfo In objReviews
                                ProcessReview(objPlaceHolder, objItem.Tokens, objReview, isPrint)
                                If (count + 1 < objReviews.Count) Then
                                    ProcessHeaderFooter(objPlaceHolder, objSeparator.Tokens)
                                End If
                                count = count + 1
                            Next

                            ProcessHeaderFooter(objPlaceHolder, objFooter.Tokens)

                        Case "REVIEWFORM"
                            If Not isPrint Then
                                Dim objReviewForm As ReviewForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/ReviewForm.ascx"), ReviewForm)
                                objReviewForm.CurrentProperty = objProperty
                                objReviewForm.ModuleID = _moduleID
                                objReviewForm.TabID = _tabID
                                objReviewForm.ModuleKey = _moduleKey
                                objReviewForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objReviewForm)
                            End If

                        Case "SEARCHCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = SiblingSearchCount(objProperty.PropertyID, objProperty.PropertyTypeID)
                            objPlaceHolder.Add(objLiteral)

                        Case "SEARCHPOSITION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = SearchPosition(objProperty.PropertyID, objProperty.PropertyTypeID).ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "SENDTOFRIENDFORM"
                            If Not isPrint Then
                                Dim objSendToFriendForm As SendToFriendForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/SendToFriendForm.ascx"), SendToFriendForm)
                                objSendToFriendForm.CurrentProperty = objProperty
                                objSendToFriendForm.ModuleID = _moduleID
                                objSendToFriendForm.TabID = _tabID
                                objSendToFriendForm.ModuleKey = _moduleKey
                                objSendToFriendForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objSendToFriendForm)
                            End If

                        Case "SHORTLIST"
                            If Not isPrint Then
                                Dim objShortListForm As ShortListForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/ShortlistForm.ascx"), ShortListForm)
                                objShortListForm.CurrentProperty = objProperty
                                objShortListForm.ModuleID = _moduleID
                                objShortListForm.TabID = _tabID
                                objShortListForm.ModuleKey = _moduleKey
                                objShortListForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objShortListForm)
                            End If

                        Case "SUBSCRIBEFORM"
                            If Not isPrint Then
                                Dim objSubscribeForm As SubscribeForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/SubscribeForm.ascx"), SubscribeForm)
                                objSubscribeForm.CurrentProperty = objProperty
                                objSubscribeForm.ModuleID = _moduleID
                                objSubscribeForm.TabID = _tabID
                                objSubscribeForm.ModuleKey = _moduleKey
                                objSubscribeForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objPlaceHolder.Add(objSubscribeForm)
                            End If

                        Case "TEMPLATEPATH"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = _portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Templates/" & _propertySettings.Template & "/"
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.PropertyTypeName
                            objPlaceHolder.Add(objLiteral)

                        Case "FULLTYPES"

                            Dim objTypeController As New PropertyTypeController()
                            Dim objType As PropertyTypeInfo = objTypeController.Get(_moduleID, objProperty.PropertyTypeID)
                            If (objType IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                                objLiteral.Text = System.Web.HttpUtility.HtmlEncode(objType.Name.ToString())
                                While objType.ParentID <> -1
                                    objType = objTypeController.Get(_moduleID, objType.ParentID)
                                    objLiteral.Text = objType.Name & " - " & objLiteral.Text
                                End While
                                objPlaceHolder.Add(objLiteral)
                            End If


                        Case "TYPECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = SiblingTypeCount(objProperty.PropertyID, objProperty.PropertyTypeID)
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEDESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.PropertyTypeDescription
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEIMAGE"
                            Dim objTypeController As New PropertyTypeController()
                            Dim objType As PropertyTypeInfo = objTypeController.Get(_moduleID, objProperty.PropertyTypeID)
                            If (objType IsNot Nothing) Then
                                If (objType.ImageFile <> "") Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = "<img src=""" & _portalSettings.HomeDirectory & objType.ImageFile & """ border=""0"">"
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If

                        Case "TYPEIMAGELINK"
                            Dim objTypeController As New PropertyTypeController()
                            Dim objType As PropertyTypeInfo = objTypeController.Get(_moduleID, objProperty.PropertyTypeID)
                            If (objType IsNot Nothing) Then
                                If (objType.ImageFile <> "") Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = _portalSettings.HomeDirectory & objType.ImageFile
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If

                        Case "TYPEID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.PropertyTypeID
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPELINK"
                            Dim objPropertyTypeController As New PropertyTypeController()
                            Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(objProperty.ModuleID, objProperty.PropertyTypeID)
                            If (objPropertyType IsNot Nothing) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objProperty.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = GetTypeLink(_tabID, _moduleID, objPropertyType, Null.NullString(), Null.NullInteger)
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "TYPEPOSITION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = TypePosition(objProperty.PropertyID, objProperty.PropertyTypeID).ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "URL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = System.Web.HttpContext.Current.Request.Url.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "URLENCODED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = System.Web.HttpContext.Current.Server.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "USERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.Username
                            objPlaceHolder.Add(objLiteral)

                        Case "USERID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objProperty.AuthorID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            Dim isRendered As Boolean = False

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("AGENT:")) Then
                                If Agent(objProperty.AuthorID) Is Nothing Then
                                    ' Should use the [HASNOAGENT] [/HASNOAGENT] template
                                Else
                                    ' Should use the [HASAGENT] [/HASAGENT] template

                                    ' token to be processed
                                    Dim field As String = layoutArray(iPtr + 1).Substring(6, layoutArray(iPtr + 1).Length - 6).ToLower().Trim()
                                    Dim fieldWithoutLinkEnd As String = field
                                    Dim fieldNameEndsWithLink As Boolean = False
                                    If field.EndsWith("link") Then
                                        fieldNameEndsWithLink = True
                                        fieldWithoutLinkEnd = field.Remove(field.Length - 4, 4)
                                    End If
                                    'Gets the DNN profile property named like the token (field)
                                    Dim profilePropertyFound As Boolean = False
                                    Dim profilePropertyDataType As String = String.Empty
                                    Dim profilePropertyName As String = String.Empty
                                    Dim profilePropertyValue As String = String.Empty

                                    For Each objProfilePropertyDefinition As ProfilePropertyDefinition In ProfileProperties
                                        If (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = field) _
                                            Or (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = fieldWithoutLinkEnd) Then

                                            'Gets the dnn profile property's datatype
                                            Dim objListController As New ListController
                                            Dim definitionEntry As ListEntryInfo = objListController.GetListEntryInfo(objProfilePropertyDefinition.DataType)
                                            If Not definitionEntry Is Nothing Then
                                                profilePropertyDataType = definitionEntry.Value
                                            Else
                                                profilePropertyDataType = "Unknown"
                                            End If

                                            If objProfilePropertyDefinition.PropertyName.ToLower().Trim().EndsWith("link") Then
                                                If fieldNameEndsWithLink = True Then
                                                    'Exact match (and both with "link" ending)
                                                    profilePropertyFound = True
                                                End If
                                            Else
                                                If fieldNameEndsWithLink = True Then
                                                    If profilePropertyDataType = "PhotoUpload" Then
                                                        'Just ignore the "Link" part on the token for "PhotoUpload" properties
                                                        profilePropertyFound = True
                                                    End If
                                                Else
                                                    'Exact match (and both without "link" ending)
                                                    profilePropertyFound = True
                                                End If
                                            End If

                                            If profilePropertyFound = True Then
                                                'Gets the dnn profile property's name and current value for the given user (Agent = AuthorID)
                                                profilePropertyName = objProfilePropertyDefinition.PropertyName
                                                profilePropertyValue = Agent(objProperty.AuthorID).Profile.GetPropertyValue(profilePropertyName)
                                                Exit For
                                            End If

                                        End If
                                    Next

                                    If Not profilePropertyFound Then
                                        'Token ignored, no matching DNN Profile property

                                    Else
                                        'Id for the control to be created
                                        Dim controlID As String = Globals.CreateValidID(_moduleKey & profilePropertyName & "-" & iPtr.ToString()) ' & "-" & i.ToString())

                                        Select Case profilePropertyDataType.ToLower()
                                            Case "truefalse"
                                                Dim objTrueFalse As New CheckBox
                                                objTrueFalse.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objTrueFalse.Checked = False
                                                Else
                                                    objTrueFalse.Checked = CType(profilePropertyValue, Boolean)
                                                End If
                                                objTrueFalse.Enabled = False
                                                objTrueFalse.EnableViewState = False
                                                objPlaceHolder.Add(objTrueFalse)

                                            Case "photoupload"
                                                Dim photoURL As String
                                                If profilePropertyValue = String.Empty Then
                                                    photoURL = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/ImageNotAvailable.jpg")
                                                Else
                                                    photoURL = _objPage.ResolveUrl(profilePropertyValue)
                                                    If Not File.Exists(_objPage.MapPath(photoURL)) Then
                                                        photoURL = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/ImageNotAvailable.jpg")
                                                    End If
                                                End If
                                                If fieldNameEndsWithLink = True Then 'field.EndsWith("link") Then
                                                    Dim objLiteral As New Literal
                                                    objLiteral.ID = controlID
                                                    objLiteral.Text = photoURL ' just the url so, the template can customize the IMG tag
                                                    objLiteral.EnableViewState = False
                                                    objPlaceHolder.Add(objLiteral)
                                                Else
                                                    Dim objImage As New Image
                                                    objImage.ID = controlID
                                                    objImage.ImageUrl = photoURL
                                                    objImage.EnableViewState = False
                                                    objPlaceHolder.Add(objImage)
                                                End If

                                            Case "richtext"
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    objLiteral.Text = _objPage.Server.HtmlDecode(profilePropertyValue)
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case "list"
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                objLiteral.Text = profilePropertyValue
                                                Dim objListController As New ListController
                                                Dim objListEntryInfoCollection As ListEntryInfoCollection = objListController.GetListEntryInfoCollection(profilePropertyName)
                                                For Each objListEntryInfo As ListEntryInfo In objListEntryInfoCollection
                                                    If objListEntryInfo.Value = profilePropertyValue Then
                                                        objLiteral.Text = objListEntryInfo.Text
                                                        Exit For
                                                    End If
                                                Next
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case Else
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    If profilePropertyName.ToLower() = "website" Then
                                                        Dim url As String = profilePropertyValue
                                                        If url.ToLower.StartsWith("http://") Then
                                                            url = url.Substring(7) ' removes the "http://"
                                                        End If
                                                        objLiteral.Text = url
                                                    Else
                                                        objLiteral.Text = profilePropertyValue
                                                    End If
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                        End Select 'profilePropertyDataType

                                        isRendered = True

                                    End If ' DNN Profile property processing
                                End If ' hasAgent
                            End If ' "AGENT:" token

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("BROKER:")) Then
                                If Broker(objProperty.BrokerID) Is Nothing Then
                                    ' Should use the [HASNOAGENT] [/HASNOAGENT] template
                                Else
                                    ' Should use the [HASAGENT] [/HASAGENT] template

                                    ' token to be processed
                                    Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower().Trim()
                                    Dim fieldWithoutLinkEnd As String = field
                                    Dim fieldNameEndsWithLink As Boolean = False
                                    If field.EndsWith("link") Then
                                        fieldNameEndsWithLink = True
                                        fieldWithoutLinkEnd = field.Remove(field.Length - 4, 4)
                                    End If
                                    'Gets the DNN profile property named like the token (field)
                                    Dim profilePropertyFound As Boolean = False
                                    Dim profilePropertyDataType As String = String.Empty
                                    Dim profilePropertyName As String = String.Empty
                                    Dim profilePropertyValue As String = String.Empty

                                    For Each objProfilePropertyDefinition As ProfilePropertyDefinition In ProfileProperties
                                        If (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = field) _
                                            Or (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = fieldWithoutLinkEnd) Then

                                            'Gets the dnn profile property's datatype
                                            Dim objListController As New ListController
                                            Dim definitionEntry As ListEntryInfo = objListController.GetListEntryInfo(objProfilePropertyDefinition.DataType)
                                            If Not definitionEntry Is Nothing Then
                                                profilePropertyDataType = definitionEntry.Value
                                            Else
                                                profilePropertyDataType = "Unknown"
                                            End If

                                            If objProfilePropertyDefinition.PropertyName.ToLower().Trim().EndsWith("link") Then
                                                If fieldNameEndsWithLink = True Then
                                                    'Exact match (and both with "link" ending)
                                                    profilePropertyFound = True
                                                End If
                                            Else
                                                If fieldNameEndsWithLink = True Then
                                                    If profilePropertyDataType = "PhotoUpload" Then
                                                        'Just ignore the "Link" part on the token for "PhotoUpload" properties
                                                        profilePropertyFound = True
                                                    End If
                                                Else
                                                    'Exact match (and both without "link" ending)
                                                    profilePropertyFound = True
                                                End If
                                            End If

                                            If profilePropertyFound = True Then
                                                'Gets the dnn profile property's name and current value for the given user (Agent = AuthorID)
                                                profilePropertyName = objProfilePropertyDefinition.PropertyName
                                                profilePropertyValue = Broker(objProperty.BrokerID).Profile.GetPropertyValue(profilePropertyName)
                                                Exit For
                                            End If

                                        End If
                                    Next

                                    If Not profilePropertyFound Then
                                        'Token ignored, no matching DNN Profile property

                                    Else
                                        'Id for the control to be created
                                        Dim controlID As String = Globals.CreateValidID(_moduleKey & profilePropertyName & "-" & iPtr.ToString()) ' & "-" & i.ToString())

                                        Select Case profilePropertyDataType.ToLower()
                                            Case "truefalse"
                                                Dim objTrueFalse As New CheckBox
                                                objTrueFalse.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objTrueFalse.Checked = False
                                                Else
                                                    objTrueFalse.Checked = CType(profilePropertyValue, Boolean)
                                                End If
                                                objTrueFalse.Enabled = False
                                                objTrueFalse.EnableViewState = False
                                                objPlaceHolder.Add(objTrueFalse)

                                            Case "photoupload"
                                                Dim photoURL As String
                                                If profilePropertyValue = String.Empty Then
                                                    photoURL = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/ImageNotAvailable.jpg")
                                                Else
                                                    photoURL = _objPage.ResolveUrl(profilePropertyValue)
                                                    If Not File.Exists(_objPage.MapPath(photoURL)) Then
                                                        photoURL = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/ImageNotAvailable.jpg")
                                                    End If
                                                End If
                                                If fieldNameEndsWithLink = True Then 'field.EndsWith("link") Then
                                                    Dim objLiteral As New Literal
                                                    objLiteral.ID = controlID
                                                    objLiteral.Text = photoURL ' just the url so, the template can customize the IMG tag
                                                    objLiteral.EnableViewState = False
                                                    objPlaceHolder.Add(objLiteral)
                                                Else
                                                    Dim objImage As New Image
                                                    objImage.ID = controlID
                                                    objImage.ImageUrl = photoURL
                                                    objImage.EnableViewState = False
                                                    objPlaceHolder.Add(objImage)
                                                End If

                                            Case "richtext"
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    objLiteral.Text = _objPage.Server.HtmlDecode(profilePropertyValue)
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case "list"
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                objLiteral.Text = profilePropertyValue
                                                Dim objListController As New ListController
                                                Dim objListEntryInfoCollection As ListEntryInfoCollection = objListController.GetListEntryInfoCollection(profilePropertyName)
                                                For Each objListEntryInfo As ListEntryInfo In objListEntryInfoCollection
                                                    If objListEntryInfo.Value = profilePropertyValue Then
                                                        objLiteral.Text = objListEntryInfo.Text
                                                        Exit For
                                                    End If
                                                Next
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case Else
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    If profilePropertyName.ToLower() = "website" Then
                                                        Dim url As String = profilePropertyValue
                                                        If url.ToLower.StartsWith("http://") Then
                                                            url = url.Substring(7) ' removes the "http://"
                                                        End If
                                                        objLiteral.Text = url
                                                    Else
                                                        objLiteral.Text = profilePropertyValue
                                                    End If
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                        End Select 'profilePropertyDataType

                                        isRendered = True

                                    End If ' DNN Profile property processing
                                End If ' hasAgent
                            End If ' "AGENT:" token

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CREATEDATEGREATERTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(22, layoutArray(iPtr + 1).Length - 22))

                                If (objProperty.DateCreated >= DateTime.Now.AddHours(length * -1)) Then
                                    Dim endVal As String = layoutArray(iPtr + 1).ToUpper()
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = ("/" & endVal)) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/CREATEDATEGREATERTHAN:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CREATEDATELESSTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(19, layoutArray(iPtr + 1).Length - 19))

                                If (objProperty.DateCreated < DateTime.Now.AddHours(length * -1)) Then
                                    Dim endVal As String = layoutArray(iPtr + 1).ToUpper()
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = ("/" & endVal)) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/CREATEDATELESSTHAN:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("MODIFIEDDATEGREATERTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(24, layoutArray(iPtr + 1).Length - 24))

                                If (objProperty.DateModified >= DateTime.Now.AddHours(length * -1)) Then
                                    Dim endVal As String = layoutArray(iPtr + 1).ToUpper()
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = ("/" & endVal)) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/MODIFIEDDATEGREATERTHAN:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("MODIFIEDDATELESSTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(21, layoutArray(iPtr + 1).Length - 21))

                                If (objProperty.DateModified < DateTime.Now.AddHours(length * -1)) Then
                                    Dim endVal As String = layoutArray(iPtr + 1).ToUpper()
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = ("/" & endVal)) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/MODIFIEDDATELESSTHAN:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("MODIFIED:")) Then
                                If Broker(objProperty.BrokerID) Is Nothing Then
                                    ' Should use the [HASNOAGENT] [/HASNOAGENT] template
                                Else
                                    ' Should use the [HASAGENT] [/HASAGENT] template

                                    ' token to be processed
                                    Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9).ToLower().Trim()
                                    Dim fieldWithoutLinkEnd As String = field
                                    Dim fieldNameEndsWithLink As Boolean = False
                                    If field.EndsWith("link") Then
                                        fieldNameEndsWithLink = True
                                        fieldWithoutLinkEnd = field.Remove(field.Length - 4, 4)
                                    End If
                                    'Gets the DNN profile property named like the token (field)
                                    Dim profilePropertyFound As Boolean = False
                                    Dim profilePropertyDataType As String = String.Empty
                                    Dim profilePropertyName As String = String.Empty
                                    Dim profilePropertyValue As String = String.Empty

                                    For Each objProfilePropertyDefinition As ProfilePropertyDefinition In ProfileProperties
                                        If (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = field) _
                                            Or (objProfilePropertyDefinition.PropertyName.ToLower().Trim() = fieldWithoutLinkEnd) Then

                                            'Gets the dnn profile property's datatype
                                            Dim objListController As New ListController
                                            Dim definitionEntry As ListEntryInfo = objListController.GetListEntryInfo(objProfilePropertyDefinition.DataType)
                                            If Not definitionEntry Is Nothing Then
                                                profilePropertyDataType = definitionEntry.Value
                                            Else
                                                profilePropertyDataType = "Unknown"
                                            End If

                                            If objProfilePropertyDefinition.PropertyName.ToLower().Trim().EndsWith("link") Then
                                                If fieldNameEndsWithLink = True Then
                                                    'Exact match (and both with "link" ending)
                                                    profilePropertyFound = True
                                                End If
                                            Else
                                                If fieldNameEndsWithLink = True Then
                                                    If profilePropertyDataType = "PhotoUpload" Then
                                                        'Just ignore the "Link" part on the token for "PhotoUpload" properties
                                                        profilePropertyFound = True
                                                    End If
                                                Else
                                                    'Exact match (and both without "link" ending)
                                                    profilePropertyFound = True
                                                End If
                                            End If

                                            If profilePropertyFound = True Then
                                                'Gets the dnn profile property's name and current value for the given user (Agent = AuthorID)
                                                profilePropertyName = objProfilePropertyDefinition.PropertyName
                                                profilePropertyValue = Modified(objProperty.BrokerID).Profile.GetPropertyValue(profilePropertyName)
                                                Exit For
                                            End If

                                        End If
                                    Next

                                    If Not profilePropertyFound Then
                                        'Token ignored, no matching DNN Profile property

                                    Else
                                        'Id for the control to be created
                                        Dim controlID As String = Globals.CreateValidID(_moduleKey & profilePropertyName & "-" & iPtr.ToString()) ' & "-" & i.ToString())

                                        Select Case profilePropertyDataType.ToLower()
                                            Case "truefalse"
                                                Dim objTrueFalse As New CheckBox
                                                objTrueFalse.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objTrueFalse.Checked = False
                                                Else
                                                    objTrueFalse.Checked = CType(profilePropertyValue, Boolean)
                                                End If
                                                objTrueFalse.Enabled = False
                                                objTrueFalse.EnableViewState = False
                                                objPlaceHolder.Add(objTrueFalse)

                                            Case "photoupload"
                                                Dim photoURL As String
                                                If profilePropertyValue = String.Empty Then
                                                    photoURL = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/ImageNotAvailable.jpg")
                                                Else
                                                    photoURL = _objPage.ResolveUrl(profilePropertyValue)
                                                    If Not File.Exists(_objPage.MapPath(photoURL)) Then
                                                        photoURL = _objPage.ResolveUrl("~/DesktopModules/PropertyAgent/Images/ImageNotAvailable.jpg")
                                                    End If
                                                End If
                                                If fieldNameEndsWithLink = True Then 'field.EndsWith("link") Then
                                                    Dim objLiteral As New Literal
                                                    objLiteral.ID = controlID
                                                    objLiteral.Text = photoURL ' just the url so, the template can customize the IMG tag
                                                    objLiteral.EnableViewState = False
                                                    objPlaceHolder.Add(objLiteral)
                                                Else
                                                    Dim objImage As New Image
                                                    objImage.ID = controlID
                                                    objImage.ImageUrl = photoURL
                                                    objImage.EnableViewState = False
                                                    objPlaceHolder.Add(objImage)
                                                End If

                                            Case "richtext"
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    objLiteral.Text = _objPage.Server.HtmlDecode(profilePropertyValue)
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case "list"
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                objLiteral.Text = profilePropertyValue
                                                Dim objListController As New ListController
                                                Dim objListEntryInfoCollection As ListEntryInfoCollection = objListController.GetListEntryInfoCollection(profilePropertyName)
                                                For Each objListEntryInfo As ListEntryInfo In objListEntryInfoCollection
                                                    If objListEntryInfo.Value = profilePropertyValue Then
                                                        objLiteral.Text = objListEntryInfo.Text
                                                        Exit For
                                                    End If
                                                Next
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)

                                            Case Else
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = controlID
                                                If profilePropertyValue = String.Empty Then
                                                    objLiteral.Text = String.Empty
                                                Else
                                                    If profilePropertyName.ToLower() = "website" Then
                                                        Dim url As String = profilePropertyValue
                                                        If url.ToLower.StartsWith("http://") Then
                                                            url = url.Substring(7) ' removes the "http://"
                                                        End If
                                                        objLiteral.Text = url
                                                    Else
                                                        objLiteral.Text = profilePropertyValue
                                                    End If
                                                End If
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                        End Select 'profilePropertyDataType

                                        isRendered = True

                                    End If ' DNN Profile property processing
                                End If ' hasAgent
                            End If ' "AGENT:" token

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CONTACTFORMEMAIL:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(17, layoutArray(iPtr + 1).Length - 17).ToLower()
                                If (field <> "") Then
                                    If Not isPrint Then
                                        Dim email As String = ""
                                        For Each objCustomField As CustomFieldInfo In objCustomFields
                                            If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                                If (objProperty.PropertyList.ContainsKey(objCustomField.CustomFieldID)) Then
                                                    email = objProperty.PropertyList(objCustomField.CustomFieldID).ToString()
                                                End If
                                            End If
                                        Next
                                        If (email <> "") Then
                                            Dim objContactForm As ContactForm = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/ContactForm.ascx"), ContactForm)
                                            objContactForm.CurrentProperty = objProperty
                                            objContactForm.ModuleID = _moduleID
                                            objContactForm.TabID = _tabID
                                            objContactForm.ModuleKey = _moduleKey
                                            objContactForm.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                            objContactForm.EmailAddress = email
                                            objPlaceHolder.Add(objContactForm)
                                        End If
                                    End If
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo
                                Dim isLink As Boolean = False
                                Dim isDate As Boolean = False

                                If (field.EndsWith("link")) Then
                                    Dim fieldWithoutLink As String = field.Remove(field.Length - 4, 4)
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = fieldWithoutLink.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                            isLink = True
                                        End If
                                    Next
                                End If

                                Dim formatExpression As String = Null.NullString
                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        If (IsNumeric(field.Split(":"c)(1))) Then
                                            maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                        Else
                                            formatExpression = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).Split(":"c)(1)
                                        End If
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If

                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                            If (objCustomField.FieldType = CustomFieldType.OneLineTextBox And objCustomField.ValidationType = CustomFieldValidationType.Date) Then
                                                isDate = True
                                            End If
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, isLink)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        If (isDate And formatExpression <> Null.NullString) Then
                                            Try
                                                fieldValue = Convert.ToDateTime(fieldValue).ToString(formatExpression)
                                            Catch
                                            End Try
                                        End If
                                        objLiteral.Text = fieldValue
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOMLIST:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11).ToLower()

                                Dim objCustomFieldSelected As New CustomFieldInfo

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        objCustomFieldSelected = objCustomField
                                    End If
                                Next

                                If (objCustomFieldSelected IsNot Nothing) Then

                                    If (objCustomFieldSelected.FieldType = CustomFieldType.MultiCheckBox) Then
                                        Dim value As String = objProperty.PropertyList(objCustomFieldSelected.CustomFieldID).ToString()

                                        Dim html As String = ""
                                        If (value.Length > 0) Then
                                            html = html & "<ul>"
                                            For Each element As String In value.Split("|")
                                                html = html & "<li>" & element & "</li>"
                                            Next
                                            html = html & "</ul>"
                                        End If

                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & 0.ToString())
                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, False)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        objLiteral.Text = html
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                    Else
                                        Dim i As Integer = 0
                                        If (objProperty.PropertyList.Contains(objCustomFieldSelected.CustomFieldID)) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                            Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, False)
                                            If (maxLength <> Null.NullInteger) Then
                                                If (fieldValue.Length > maxLength) Then
                                                    fieldValue = fieldValue.Substring(0, maxLength)
                                                End If
                                            End If
                                            objLiteral.Text = fieldValue
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                            i = i + 1
                                        End If
                                    End If

                                End If
                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOMSEARCH:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(13, layoutArray(iPtr + 1).Length - 13).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If

                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then
                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then

                                        Dim fieldValue As String = GetFieldValue(objCustomFieldSelected, objProperty, False, False)
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If

                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        For Each val As String In fieldValue.Split(";"c)
                                            If (objLiteral.Text.Length = 0) Then
                                                objLiteral.Text = "<a href='" & NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "CustomFieldIDs=" & customFieldID.ToString(), "SearchValues=" & HttpContext.Current.Server.UrlEncode(val)) & "'>" & val & "</a>"
                                            Else
                                                objLiteral.Text = objLiteral.Text & ", " & "<a href='" & NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "CustomFieldIDs=" & customFieldID.ToString(), "SearchValues=" & HttpContext.Current.Server.UrlEncode(val)) & "'>" & val & "</a>"
                                            End If
                                        Next
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)

                                        i = i + 1
                                    End If
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CAPTION:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8)

                                Dim i As Integer = 0
                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                            objLiteral.Text = objCustomField.Caption
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                            i = i + 1
                                        End If
                                    End If
                                Next

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CONTACTFORM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)

                                If (objMailCustomFields IsNot Nothing) Then
                                    Dim i As Integer = 0
                                    For Each objCustomField As ContactFieldInfo In objMailCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                            objLiteral.Text = objCustomField.DefaultValue
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                            i = i + 1
                                        End If
                                    Next
                                End If

                                isRendered = True
                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CURRENCYCONVERTER:")) Then
                                If Not isPrint Then

                                    Dim field As String = layoutArray(iPtr + 1).Substring(18, layoutArray(iPtr + 1).Length - 18)

                                    Dim customFieldID As Integer = Null.NullInteger
                                    Dim objCustomFieldSelected As New CustomFieldInfo

                                    If (customFieldID = Null.NullInteger) Then
                                        For Each objCustomField As CustomFieldInfo In objCustomFields
                                            If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                                customFieldID = objCustomField.CustomFieldID
                                                objCustomFieldSelected = objCustomField
                                            End If
                                        Next
                                    End If

                                    If (customFieldID <> Null.NullInteger) Then

                                        If (objProperty.PropertyList.Contains(customFieldID)) Then
                                            Dim fieldValue As String = objProperty.PropertyList(customFieldID).ToString()

                                            If (IsNumeric(fieldValue)) Then
                                                Dim objCurrencyConverter As CurrencyConverter = CType(_objPage.LoadControl("~/DesktopModules/PropertyAgent/CurrencyConverter.ascx"), CurrencyConverter)
                                                objCurrencyConverter.CurrentProperty = objProperty
                                                objCurrencyConverter.ModuleID = _moduleID
                                                objCurrencyConverter.TabID = _tabID
                                                objCurrencyConverter.ModuleKey = _moduleKey
                                                objCurrencyConverter.FieldValue = fieldValue
                                                objCurrencyConverter.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                                objPlaceHolder.Add(objCurrencyConverter)
                                            End If
                                        End If
                                    End If

                                    isRendered = True

                                End If
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("FRIEND:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7)

                                Dim i As Integer = 0
                                For Each objCustomField As ContactFieldInfo In objMailCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        objLiteral.Text = objCustomField.DefaultValue
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                Next

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EXPRESSION:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                Dim params As String() = field.Split(":"c)

                                If (params.Length <> 3) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Dim customField As String = params(0)
                                Dim customExpression As String = params(1)
                                Dim customValue As String = params(2)

                                Dim isMultiCheckBox As Boolean = False

                                Dim fieldValue As String = ""

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                        If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            fieldValue = GetFieldValue(objCustomField, objProperty, False, False)
                                            If (objCustomField.FieldType = CustomFieldType.MultiCheckBox) Then
                                                isMultiCheckBox = True
                                            End If
                                        End If
                                    End If
                                Next

                                Dim isValid = False
                                Select Case customExpression
                                    Case "="
                                        If (isMultiCheckBox) Then
                                            Dim tempFieldValue As String = ""
                                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                                If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                                    If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                                        tempFieldValue = GetFieldValue(objCustomField, objProperty, False, False, False)
                                                        Exit For
                                                    End If
                                                End If
                                            Next
                                            For Each Val As String In tempFieldValue.ToLower().Trim().Split("|"c)
                                                If (customValue.ToLower() = Val.ToLower()) Then
                                                    isValid = True
                                                    Exit For
                                                End If
                                            Next
                                        Else
                                            If (customValue.ToLower() = fieldValue.ToLower()) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case "!="
                                        If (isMultiCheckBox) Then
                                            Dim tempFieldValue As String = ""
                                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                                If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                                    If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                                        tempFieldValue = GetFieldValue(objCustomField, objProperty, False, False, False)
                                                        Exit For
                                                    End If
                                                End If
                                            Next
                                            Dim match As Boolean = False
                                            For Each Val As String In tempFieldValue.ToLower().Trim().Split("|"c)
                                                If (customValue.ToLower() = Val.ToLower()) Then
                                                    match = True
                                                End If
                                            Next
                                            If (match = False) Then
                                                isValid = True
                                            End If
                                        Else
                                            If (customValue.ToLower() <> fieldValue.ToLower()) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case "<"
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) < Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case "<="
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) <= Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case ">"
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) > Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                    Case ">="
                                        If (IsNumeric(customValue) AndAlso IsNumeric(fieldValue)) Then
                                            If (Convert.ToInt32(fieldValue) >= Convert.ToInt32(customValue)) Then
                                                isValid = True
                                            End If
                                        End If
                                        Exit Select

                                End Select

                                If (isValid = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/EXPRESSION:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EXPRESSIONTYPE:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(15, layoutArray(iPtr + 1).Length - 15)

                                Dim params As String() = field.Split(":"c)

                                If (params.Length < 2 Or params.Length > 3) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Dim propertyExpression As String = params(0)
                                Dim propertyType As String = params(1)

                                Dim isValid = False
                                Select Case propertyExpression
                                    Case "="
                                        If (propertyType.ToLower().ToString() = objProperty.PropertyTypeName.ToLower().ToString()) Then
                                            isValid = True
                                        End If
                                        Exit Select
                                    Case "!="
                                        If (propertyType.ToLower().ToString() <> objProperty.PropertyTypeName.ToLower().ToString()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                End Select

                                If (params.Length = 3) Then
                                    isValid = False

                                    Dim level As String = params(2)

                                    Dim objTypeStack As New List(Of PropertyTypeInfo)

                                    Dim objPropertyTypeController As New PropertyTypeController()
                                    Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(_moduleID, objProperty.PropertyTypeID)

                                    While (objPropertyType IsNot Nothing)
                                        objTypeStack.Insert(0, objPropertyType)
                                        objPropertyType = objPropertyTypeController.Get(_moduleID, objPropertyType.ParentID)
                                    End While

                                    If (IsNumeric(level)) Then
                                        Dim count As Integer = 0
                                        For Each objType As PropertyTypeInfo In objTypeStack
                                            If (count = Convert.ToInt32(level)) Then
                                                Select Case propertyExpression
                                                    Case "="
                                                        If (propertyType.ToLower().ToString() = objType.Name.ToLower().ToString()) Then
                                                            isValid = True
                                                        End If
                                                        Exit Select
                                                    Case "!="
                                                        If (propertyType.ToLower().ToString() <> objType.Name.ToLower().ToString()) Then
                                                            isValid = True
                                                        End If
                                                        Exit Select
                                                End Select
                                            End If
                                            count = count + 1
                                        Next
                                    End If

                                End If

                                If (isValid = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/EXPRESSIONTYPE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EXPRESSIONTYPEID:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(17, layoutArray(iPtr + 1).Length - 17)

                                Dim params As String() = field.Split(":"c)

                                If (params.Length <> 2) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                Dim propertyExpression As String = params(0)
                                Dim propertyTypeID As String = params(1)

                                Dim isValid = False
                                Select Case propertyExpression
                                    Case "="
                                        If (propertyTypeID.ToString() = objProperty.PropertyTypeID.ToString()) Then
                                            isValid = True
                                        End If
                                        Exit Select

                                End Select

                                If (isValid = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/EXPRESSIONTYPEID:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("EMAILFORM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(10, layoutArray(iPtr + 1).Length - 10)

                                Dim i As Integer = 0
                                For Each objCustomField As ContactFieldInfo In objMailCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString() & "-" & i.ToString())
                                        objLiteral.Text = objCustomField.DefaultValue
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        i = i + 1
                                    End If
                                Next

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATECREATED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                If (objProperty.DateCreated <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DateCreated.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATEEXPIRED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                If (objProperty.DateExpired <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DateExpired.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATEMODIFIED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(13, layoutArray(iPtr + 1).Length - 13)
                                If (objProperty.DateModified <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DateModified.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("DATEPUBLISHED:")) Then
                                Dim formatExpression As String = layoutArray(iPtr + 1).Substring(14, layoutArray(iPtr + 1).Length - 14)
                                If (objProperty.DatePublished <> Null.NullDate) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objProperty.DatePublished.ToString(formatExpression)
                                    objLiteral.EnableViewState = False
                                    objPlaceHolder.Add(objLiteral)
                                End If

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASNOVALUE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim fieldValue As String = GetFieldValue(objCustomField, objProperty, False, False)
                                            If (fieldValue.Trim() <> "") Then
                                                Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                                While (iPtr < layoutArray.Length - 1)
                                                    If (layoutArray(iPtr + 1) = endToken) Then
                                                        Exit While
                                                    End If
                                                    iPtr = iPtr + 1
                                                End While
                                            End If
                                        End If
                                    End If
                                Next

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASNOVALUE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASVALUE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            Dim fieldValue As String = GetFieldValue(objCustomField, objProperty, False, True)
                                            If (fieldValue.Trim() = "") Then
                                                Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                                While (iPtr < layoutArray.Length - 1)
                                                    If (layoutArray(iPtr + 1) = endToken) Then
                                                        Exit While
                                                    End If
                                                    iPtr = iPtr + 1
                                                End While
                                            End If
                                        End If
                                    End If
                                Next

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASVALUE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISLOCALE:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)

                                If (CType(_objPage, DotNetNuke.Framework.PageBase).PageCulture.Name.ToLower() <> field.ToLower()) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISLOCALE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)

                                If (PortalSecurity.IsInRole(field) = False) Then

                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While

                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISINROLE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISNOTINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)

                                If (PortalSecurity.IsInRole(field)) Then

                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While

                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISNOTINROLE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISAGENTINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(14, layoutArray(iPtr + 1).Length - 14)

                                Dim objRoleController As New DotNetNuke.Security.Roles.RoleController
                                Dim objRoles() As String = objRoleController.GetRolesByUser(objProperty.AuthorID, _portalID)

                                Dim roleFound As Boolean = False
                                For Each role As String In objRoles
                                    If (role.ToLower() = field.ToLower()) Then
                                        roleFound = True
                                    End If
                                Next

                                If (roleFound = False) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISAGENTINROLE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISAGENTNOTINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(17, layoutArray(iPtr + 1).Length - 17)

                                Dim objRoleController As New DotNetNuke.Security.Roles.RoleController
                                Dim objRoles() As String = objRoleController.GetRolesByUser(objProperty.AuthorID, _portalID)

                                Dim roleFound As Boolean = False
                                For Each role As String In objRoles
                                    If (role.ToLower() = field.ToLower()) Then
                                        roleFound = True
                                    End If
                                Next

                                If (roleFound = True) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/ISAGENTNOTINROLE:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PUBLISHDATEGREATERTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(23, layoutArray(iPtr + 1).Length - 23))

                                If (objProperty.DatePublished >= DateTime.Now.AddHours(length * -1)) Then
                                    Dim endVal As String = layoutArray(iPtr + 1).ToUpper()
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = ("/" & endVal)) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/PUBLISHDATEGREATERTHAN:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PUBLISHDATELESSTHAN:")) Then
                                Dim length As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(20, layoutArray(iPtr + 1).Length - 20))

                                If (objProperty.DatePublished < DateTime.Now.AddHours(length * -1)) Then
                                    Dim endVal As String = layoutArray(iPtr + 1).ToUpper()
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = ("/" & endVal)) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/PUBLISHDATELESSTHAN:")) Then
                                Exit Select
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("RSSREADER:")) Then

                                Try

                                    Dim field As String = layoutArray(iPtr + 1).Substring(10, layoutArray(iPtr + 1).Length - 10)

                                    Dim count As Integer = Null.NullInteger
                                    Dim fieldParts As String() = field.Split(":"c)
                                    If (fieldParts.Length > 1) Then
                                        If (IsNumeric(fieldParts(fieldParts.Length - 1))) Then
                                            count = fieldParts(fieldParts.Length - 1)
                                            field = ""
                                            For i As Integer = 0 To fieldParts.Length - 2
                                                If (i > 0) Then
                                                    field = field & ":" & fieldParts(i)
                                                Else
                                                    field = field & fieldParts(i)
                                                End If
                                            Next
                                        End If
                                    End If

                                    Dim delimStr As String = "{}"
                                    Dim delimiter As Char() = delimStr.ToCharArray()
                                    Dim phRssUrl As New PlaceHolder
                                    ProcessItem(phRssUrl.Controls, field.Split(delimiter), objProperty, objCustomFields)
                                    field = RenderControlToString(phRssUrl)

                                    Dim doc As XPathDocument
                                    Dim navigator As XPathNavigator
                                    Dim nodes As XPathNodeIterator
                                    Dim node As XPathNavigator

                                    ' Create a new XmlDocument  
                                    doc = New XPathDocument(field)

                                    ' Create navigator  
                                    navigator = doc.CreateNavigator()

                                    ' Get forecast with XPath  
                                    nodes = navigator.Select("/rss/channel/item")

                                    Dim objRssItems As New List(Of RssInfo)

                                    While nodes.MoveNext()
                                        node = nodes.Current

                                        Dim nodeTitle As XPathNavigator
                                        Dim nodeDescription As XPathNavigator
                                        Dim nodeLink As XPathNavigator

                                        nodeTitle = node.SelectSingleNode("title")
                                        nodeDescription = node.SelectSingleNode("description")
                                        nodeLink = node.SelectSingleNode("link")

                                        Dim objRssItem As New RssInfo()

                                        objRssItem.Title = nodeTitle.Value
                                        objRssItem.Description = nodeDescription.Value
                                        objRssItem.Link = nodeLink.Value

                                        objRssItems.Add(objRssItem)
                                    End While

                                    If (objRssItems.Count > 0) Then

                                        Dim objHeader As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.RSS_Reader_Header_Html)
                                        Dim objItem As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.RSS_Reader_Item_Html)
                                        Dim objFooter As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.RSS_Reader_Footer_Html)

                                        ProcessHeaderFooter(objPlaceHolder, objHeader.Tokens)
                                        Dim i As Integer = 1
                                        For Each objRssItem As RssInfo In objRssItems
                                            If (count = Null.NullInteger Or (count <> Null.NullInteger And i < (count + 1))) Then
                                                ProcessRssReaderItem(objPlaceHolder, objItem.Tokens, objRssItem, iPtr.ToString() & "-" & i.ToString())
                                                i = i + 1
                                            End If
                                        Next
                                        ProcessHeaderFooter(objPlaceHolder, objFooter.Tokens)

                                    End If

                                Catch
                                End Try

                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PHOTOS:")) Then

                                Dim category As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7)

                                Dim objPhotoController As New PhotoController
                                Dim objPhotos As ArrayList = objPhotoController.List(objProperty.PropertyID)

                                Dim objPhotosSelected As New ArrayList()
                                For Each objPhoto As PhotoInfo In objPhotos
                                    If (objPhoto.Category = category) Then
                                        objPhotosSelected.Add(objPhoto)
                                    End If
                                Next

                                If (objPhotosSelected.Count > 0) Then

                                    Dim objLayoutController As New LayoutController(_portalSettings, _propertySettings, _objPage, _objControl, _isEditable, _tabID, _moduleID, _moduleKey)
                                    Dim objLayoutFirst As LayoutInfo = objLayoutController.GetLayout(_propertySettings.Template, LayoutType.Photo_First_Html)
                                    _objLayoutPhotoItem = objLayoutController.GetLayout(_propertySettings.Template, LayoutType.Photo_Item_Html)

                                    If (objLayoutFirst.Template <> "") Then
                                        Dim objPhoto As PhotoInfo = CType(objPhotosSelected(0), PhotoInfo)
                                        ProcessPhoto(objPlaceHolder, objLayoutFirst.Tokens, objPhoto, iPtr)
                                        objPhotosSelected.RemoveAt(0)
                                    End If

                                    If (objPhotosSelected.Count > 0) Then

                                        Dim objRepeater As New System.Web.UI.WebControls.Repeater
                                        Dim objHandler As New RepeaterItemEventHandler(AddressOf rptPhotos_ItemDataBound)
                                        AddHandler objRepeater.ItemDataBound, objHandler

                                        objRepeater.DataSource = objPhotosSelected
                                        objRepeater.DataBind()

                                        objPlaceHolder.Add(objRepeater)

                                    End If

                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("HASPHOTOS:")) Then

                                Dim category As String = layoutArray(iPtr + 1).Substring(10, layoutArray(iPtr + 1).Length - 10)

                                Dim objPhotoController As New PhotoController
                                Dim objPhotos As ArrayList = objPhotoController.List(objProperty.PropertyID)

                                Dim objPhotosSelected As New ArrayList()
                                For Each objPhoto As PhotoInfo In objPhotos
                                    If (objPhoto.Category = category) Then
                                        objPhotosSelected.Add(objPhoto)
                                    End If
                                Next

                                If (objPhotosSelected.Count = 0) Then
                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/HASPHOTOS:")) Then
                                isRendered = True
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("TYPE:")) Then

                                Try

                                    Dim objTypes As New List(Of PropertyTypeInfo)

                                    Dim objTypeController As New PropertyTypeController

                                    Dim objType As PropertyTypeInfo = objTypeController.Get(_moduleID, objProperty.PropertyTypeID)

                                    objTypes.Insert(0, objType)
                                    While objType.ParentID <> Null.NullInteger
                                        objType = objTypeController.Get(_moduleID, objType.ParentID)
                                        If (objType.PropertyTypeID <> objProperty.PropertyTypeID) Then
                                            objTypes.Insert(0, objType)
                                        End If
                                    End While

                                    Dim count As Integer = Convert.ToInt32(layoutArray(iPtr + 1).Substring(5, layoutArray(iPtr + 1).Length - 5))
                                    If (count >= 0) Then
                                        If (count < objTypes.Count) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                            objLiteral.Text = objTypes(count).Name
                                            objPlaceHolder.Add(objLiteral)
                                        End If
                                    End If

                                Catch
                                End Try

                                isRendered = True

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("/TYPE:")) Then
                                isRendered = True
                            End If

                            If (isRendered = False) Then
                                Dim objLiteralOther As New Literal
                                objLiteralOther.ID = Globals.CreateValidID(_moduleKey & objProperty.PropertyID.ToString() & "-" & iPtr.ToString())
                                objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                                objLiteralOther.EnableViewState = False
                                objPlaceHolder.Add(objLiteralOther)
                            End If

                    End Select
                End If

            Next

            _listingIndex = _listingIndex + 1

        End Sub

        Public Sub ProcessOptionItem(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "ADDPROPERTY"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLink.Text = PropertyUtil.FormatPropertyLabel(GetSharedResource("AddProperty"), _propertySettings)
                            objLink.Visible = _propertySettings.Template <> "" AndAlso _objPage.Request.IsAuthenticated AndAlso (CType(_objControl, PropertyAgentBase).IsEditable OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionBroker)) AndAlso CType(_objControl, PropertyAgentBase).CheckLimit()
                            objLink.CssClass = _propertySettings.ButtonClass
                            objLink.EnableViewState = False
                            objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=EditProperty")
                            objPlaceHolder.Add(objLink)

                        Case "ADDPROPERTYLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=EditProperty")
                            objLiteral.Visible = _propertySettings.Template <> "" AndAlso _objPage.Request.IsAuthenticated AndAlso (CType(_objControl, PropertyAgentBase).IsEditable OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionBroker)) AndAlso CType(_objControl, PropertyAgentBase).CheckLimit()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "HASADDPROPERTY"
                            If (_propertySettings.Template <> "" AndAlso _objPage.Request.IsAuthenticated AndAlso (CType(_objControl, PropertyAgentBase).IsEditable OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionBroker)) AndAlso CType(_objControl, PropertyAgentBase).CheckLimit()) = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASADDPROPERTY") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASADDPROPERTY"
                            ' Do Nothing

                        Case "HASPROPERTYMANAGER"
                            If (_propertySettings.Template <> "" AndAlso _objPage.Request.IsAuthenticated AndAlso (CType(_objControl, PropertyAgentBase).IsEditable OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionApprove) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionBroker)) AndAlso CType(_objControl, PropertyAgentBase).CheckLimit()) = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASPROPERTYMANAGER") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASPROPERTYMANAGER"
                            ' Do Nothing

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
                            ' Do Nothing.

                        Case "PROPERTYMANAGER"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLink.Text = PropertyUtil.FormatPropertyLabel(GetSharedResource("PropertyManager"), _propertySettings)
                            objLink.Visible = _propertySettings.Template <> "" AndAlso _objPage.Request.IsAuthenticated AndAlso (CType(_objControl, PropertyAgentBase).IsEditable OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionApprove) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionBroker)) AndAlso CType(_objControl, PropertyAgentBase).CheckLimit()
                            objLink.CssClass = _propertySettings.ButtonClass
                            objLink.EnableViewState = False
                            objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=PropertyManager")
                            objPlaceHolder.Add(objLink)

                        Case "PROPERTYMANAGERLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                            objLiteral.Text = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=PropertyManager")
                            objLiteral.Visible = _propertySettings.Template <> "" AndAlso _objPage.Request.IsAuthenticated AndAlso (CType(_objControl, PropertyAgentBase).IsEditable OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionApprove) OrElse PortalSecurity.IsInRoles(_propertySettings.PermissionBroker)) AndAlso CType(_objControl, PropertyAgentBase).CheckLimit()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "RSS"
                            If (HttpContext.Current.Items.Contains("RSS-PropertyAgent")) Then
                                Dim objLink As New HyperLink
                                objLink.ImageUrl = "~/DesktopModules/PropertyAgent/images/xml.gif"
                                objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                objLink.Text = HttpContext.Current.Items("RSS-PropertyAgent-Title").ToString()
                                objLink.CssClass = _propertySettings.ButtonClass
                                objLink.EnableViewState = False
                                objLink.NavigateUrl = HttpContext.Current.Items("RSS-PropertyAgent").ToString()
                                objLink.Style.Add("vertical-align", "middle")
                                objPlaceHolder.Add(objLink)
                            End If

                        Case "SORTRATING"
                            If (CType(_objControl, PropertyAgentBase).Request("sortBy") = "Rating") Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                objLiteral.Text = GetSharedResource("SortRating")
                                objLiteral.EnableViewState = False
                                objPlaceHolder.Add(objLiteral)
                            Else
                                Dim objLink As New HyperLink
                                objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                objLink.Text = GetSharedResource("SortRating")
                                objLink.CssClass = _propertySettings.ButtonClass
                                objLink.EnableViewState = False
                                If (CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs") <> "" And CType(_objControl, PropertyAgentBase).Request("SearchValues") <> "") Then

                                    If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                        objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "customFieldIDs=" & CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs"), "searchValues=" & CType(_objControl, PropertyAgentBase).Request("SearchValues"), "sortBy=Rating")
                                    Else
                                        objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "customFieldIDs=" & CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs"), "searchValues=" & CType(_objControl, PropertyAgentBase).Request("SearchValues"), "sortBy=Rating")
                                    End If
                                Else
                                    If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                        objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "sortBy=Rating")
                                    Else
                                        objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "sortBy=Rating")
                                    End If
                                End If
                                objPlaceHolder.Add(objLink)
                            End If

                        Case Else


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("FILTER:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7)

                                If (field.Contains(":")) Then

                                    Dim val As String = field.Split(":"c)(1)
                                    field = field.Split(":"c)(0)

                                    Dim objCustomFieldController As New CustomFieldController
                                    Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(CType(_objControl, PropertyAgentBase).ModuleId, True)

                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then

                                            Dim useLink As Boolean = True
                                            If (CType(_objControl, PropertyAgentBase).Request("customFieldIDs") <> "") Then
                                                Dim index As Integer = 1
                                                For Each item As String In CType(_objControl, PropertyAgentBase).Request("customFieldIDs").Split(",")
                                                    If (item = objCustomField.CustomFieldID.ToString()) Then
                                                        If (CType(_objControl, PropertyAgentBase).Request("searchValues") <> "") Then
                                                            If (CType(_objControl, PropertyAgentBase).Request("searchValues").Split(",").Length >= (index)) Then
                                                                If (val = CType(_objControl, PropertyAgentBase).Request("searchValues").Split(",")(index - 1)) Then
                                                                    useLink = False
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                    index = index + 1
                                                Next
                                            End If

                                            If (useLink = False) Then
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                                objLiteral.Text = val
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                            Else
                                                Dim objLink As New HyperLink
                                                objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                                objLink.Text = val
                                                objLink.CssClass = _propertySettings.ButtonClass
                                                objLink.EnableViewState = False
                                                If (CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs") <> "" And CType(_objControl, PropertyAgentBase).Request("SearchValues") <> "") Then
                                                    Dim arr As New List(Of String)
                                                    arr.Add(_propertySettings.SEOAgentType & "=ViewSearch")

                                                    If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                        arr.Add(_propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID))
                                                    End If

                                                    Dim customFieldIDs As String = ""
                                                    Dim searchValues As String = ""
                                                    Dim index = 0
                                                    For Each customFieldID As String In CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs").Split(","c)
                                                        If (customFieldID <> objCustomField.CustomFieldID.ToString()) Then

                                                            If (CType(_objControl, PropertyAgentBase).Request("SearchValues").Split(","c).Length > index) Then
                                                                If (customFieldIDs <> "") Then
                                                                    customFieldIDs = customFieldIDs & "," & customFieldID
                                                                Else
                                                                    customFieldIDs = customFieldID
                                                                End If

                                                                If (searchValues <> "") Then
                                                                    searchValues = searchValues & "," & CType(_objControl, PropertyAgentBase).Request("SearchValues").Split(","c)(index)
                                                                Else
                                                                    searchValues = CType(_objControl, PropertyAgentBase).Request("SearchValues").Split(","c)(index)
                                                                End If
                                                            End If

                                                        End If
                                                        index = index + 1
                                                    Next
                                                    If (customFieldIDs <> "" And searchValues <> "") Then
                                                        customFieldIDs = customFieldIDs & "," & objCustomField.CustomFieldID.ToString()
                                                        searchValues = searchValues & "," & val
                                                    Else
                                                        customFieldIDs = objCustomField.CustomFieldID.ToString()
                                                        searchValues = val
                                                    End If

                                                    arr.Add("customFieldIDs=" & customFieldIDs)
                                                    arr.Add("searchValues=" & searchValues)

                                                    If (CType(_objControl, PropertyAgentBase).Request("sortBy") <> "") Then
                                                        arr.Add("sortBy=" & CType(_objControl, PropertyAgentBase).Request("sortBy"))
                                                    End If

                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", arr.ToArray())
                                                Else
                                                    If (CType(_objControl, PropertyAgentBase).Request("sortBy") <> "") Then
                                                        If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                            objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "customFieldIDs=" & objCustomField.CustomFieldID.ToString(), "searchValues=" & val, "sortBy=" & CType(_objControl, PropertyAgentBase).Request("sortBy"))
                                                        Else
                                                            objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "customFieldIDs=" & objCustomField.CustomFieldID.ToString(), "searchValues=" & val, "sortBy=" & CType(_objControl, PropertyAgentBase).Request("sortBy"))
                                                        End If
                                                    Else
                                                        If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                            objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "customFieldIDs=" & objCustomField.CustomFieldID.ToString(), "searchValues=" & val)
                                                        Else
                                                            objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "customFieldIDs=" & objCustomField.CustomFieldID.ToString(), "searchValues=" & val)
                                                        End If
                                                    End If
                                                End If
                                                objPlaceHolder.Add(objLink)
                                            End If
                                            Exit For
                                        End If
                                    Next

                                End If

                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("FILTERCLEAR:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)

                                If (field.Contains(":")) Then

                                    Dim text As String = field.Split(":"c)(1)
                                    field = field.Split(":"c)(0)

                                    Dim objCustomFieldController As New CustomFieldController
                                    Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(CType(_objControl, PropertyAgentBase).ModuleId, True)

                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then

                                            Dim useLink As Boolean = False
                                            If (CType(_objControl, PropertyAgentBase).Request("customFieldIDs") <> "") Then
                                                Dim index As Integer = 1
                                                For Each item As String In CType(_objControl, PropertyAgentBase).Request("customFieldIDs").Split(",")
                                                    If (item = objCustomField.CustomFieldID.ToString()) Then
                                                        useLink = True
                                                    End If
                                                    index = index + 1
                                                Next
                                            End If

                                            If (useLink = False) Then
                                                Dim objLiteral As New Literal
                                                objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                                objLiteral.Text = text
                                                objLiteral.EnableViewState = False
                                                objPlaceHolder.Add(objLiteral)
                                            Else
                                                Dim objLink As New HyperLink
                                                objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                                objLink.Text = text
                                                objLink.CssClass = _propertySettings.ButtonClass
                                                objLink.EnableViewState = False
                                                If (CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs") <> "" And CType(_objControl, PropertyAgentBase).Request("SearchValues") <> "") Then
                                                    Dim arr As New List(Of String)
                                                    arr.Add(_propertySettings.SEOAgentType & "=ViewSearch")

                                                    If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                        arr.Add(_propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID))
                                                    End If

                                                    Dim customFieldIDs As String = ""
                                                    Dim searchValues As String = ""
                                                    Dim index = 0
                                                    For Each customFieldID As String In CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs").Split(","c)
                                                        If (customFieldID <> objCustomField.CustomFieldID.ToString()) Then

                                                            If (CType(_objControl, PropertyAgentBase).Request("SearchValues").Split(","c).Length > index) Then
                                                                If (customFieldIDs <> "") Then
                                                                    customFieldIDs = customFieldIDs & "," & customFieldID
                                                                Else
                                                                    customFieldIDs = customFieldID
                                                                End If

                                                                If (searchValues <> "") Then
                                                                    searchValues = searchValues & "," & CType(_objControl, PropertyAgentBase).Request("SearchValues").Split(","c)(index)
                                                                Else
                                                                    searchValues = CType(_objControl, PropertyAgentBase).Request("SearchValues").Split(","c)(index)
                                                                End If
                                                            End If

                                                        End If
                                                        index = index + 1
                                                    Next

                                                    If (customFieldIDs <> "" And searchValues <> "") Then
                                                        arr.Add("customFieldIDs=" & customFieldIDs)
                                                        arr.Add("searchValues=" & searchValues)
                                                    End If

                                                    If (CType(_objControl, PropertyAgentBase).Request("sortBy") <> "") Then
                                                        arr.Add("sortBy=" & CType(_objControl, PropertyAgentBase).Request("sortBy"))
                                                    End If

                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", arr.ToArray())
                                                End If
                                                objPlaceHolder.Add(objLink)
                                            End If

                                        End If
                                    Next

                                End If

                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)
                                If (PortalSecurity.IsInRole(field) = False) Then

                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While

                                End If

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISNOTINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                If (PortalSecurity.IsInRole(field) = True) Then

                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While

                                End If

                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("SORT:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(5, layoutArray(iPtr + 1).Length - 5)

                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(CType(_objControl, PropertyAgentBase).ModuleId, True)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (CType(_objControl, PropertyAgentBase).Request("sortBy") = "cf" & objCustomField.CustomFieldID.ToString()) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                            objLiteral.Text = GetSharedResource("SortBy")
                                            If (objLiteral.Text.Contains("{0}")) Then
                                                objLiteral.Text = objLiteral.Text.Replace("{0}", objCustomField.Caption)
                                            End If
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                        Else
                                            Dim objLink As New HyperLink
                                            objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                            objLink.Text = GetSharedResource("SortBy")
                                            If (objLink.Text.Contains("{0}")) Then
                                                objLink.Text = objLink.Text.Replace("{0}", objCustomField.Caption)
                                            End If
                                            objLink.CssClass = _propertySettings.ButtonClass
                                            objLink.EnableViewState = False
                                            If (CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs") <> "" And CType(_objControl, PropertyAgentBase).Request("SearchValues") <> "") Then
                                                If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "customFieldIDs=" & CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs"), "searchValues=" & CType(_objControl, PropertyAgentBase).Request("SearchValues"), "sortBy=cf" & objCustomField.CustomFieldID.ToString())
                                                Else
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "customFieldIDs=" & CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs"), "searchValues=" & CType(_objControl, PropertyAgentBase).Request("SearchValues"), "sortBy=cf" & objCustomField.CustomFieldID.ToString())
                                                End If
                                            Else
                                                If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "sortBy=cf" & objCustomField.CustomFieldID.ToString())
                                                Else
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "sortBy=cf" & objCustomField.CustomFieldID.ToString())
                                                End If
                                            End If
                                            objPlaceHolder.Add(objLink)
                                        End If
                                        Exit For
                                    End If
                                Next

                            End If


                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("SORTRATING:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(11, layoutArray(iPtr + 1).Length - 11)

                                Dim objReviewFieldController As New ReviewFieldController()
                                Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(CType(_objControl, PropertyAgentBase).ModuleId)

                                For Each objReviewField As ReviewFieldInfo In objReviewFields
                                    If (objReviewField.Name.ToLower() = field.ToLower()) Then
                                        If (CType(_objControl, PropertyAgentBase).Request("sortBy") = "rf" & objReviewField.ReviewFieldID.ToString()) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                            objLiteral.Text = GetSharedResource("SortBy")
                                            If (objLiteral.Text.Contains("{0}")) Then
                                                objLiteral.Text = objLiteral.Text.Replace("{0}", objReviewField.Caption)
                                            End If
                                            objLiteral.EnableViewState = False
                                            objPlaceHolder.Add(objLiteral)
                                        Else
                                            Dim objLink As New HyperLink
                                            objLink.ID = Globals.CreateValidID(_moduleKey & "-" & iPtr.ToString())
                                            objLink.Text = GetSharedResource("SortBy")
                                            If (objLink.Text.Contains("{0}")) Then
                                                objLink.Text = objLink.Text.Replace("{0}", objReviewField.Caption)
                                            End If
                                            objLink.CssClass = _propertySettings.ButtonClass
                                            objLink.EnableViewState = False
                                            If (CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs") <> "" And CType(_objControl, PropertyAgentBase).Request("SearchValues") <> "") Then
                                                If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "customFieldIDs=" & CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs"), "searchValues=" & CType(_objControl, PropertyAgentBase).Request("SearchValues"), "sortBy=rf" & objReviewField.ReviewFieldID.ToString())
                                                Else
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "customFieldIDs=" & CType(_objControl, PropertyAgentBase).Request("CustomFieldIDs"), "searchValues=" & CType(_objControl, PropertyAgentBase).Request("SearchValues"), "sortBy=rf" & objReviewField.ReviewFieldID.ToString())
                                                End If
                                            Else
                                                If (CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID) <> "") Then
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", _propertySettings.SEOPropertyTypeID & "=" & CType(_objControl, PropertyAgentBase).Request(_propertySettings.SEOPropertyTypeID), "sortBy=rf" & objReviewField.ReviewFieldID.ToString())
                                                Else
                                                    objLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=ViewSearch", "sortBy=rf" & objReviewField.ReviewFieldID.ToString())
                                                End If
                                            End If
                                            objPlaceHolder.Add(objLink)
                                        End If
                                    End If
                                Next

                            End If

                    End Select
                End If
            Next

        End Sub

        Private photoItemIndex As Integer = 0
        Protected Sub ProcessPhoto(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objPhoto As PhotoInfo, ByVal photoPtr As String)

            photoItemIndex = photoItemIndex + 1
            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString().Replace("~", DotNetNuke.Common.Globals.ApplicationPath)))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

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
                            ' Do Nothing.

                        Case "ITEMINDEX"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & photoPtr & "-" & iPtr.ToString())
                            objLiteral.Text = photoItemIndex.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PHOTOSMALL"
                            ProcessPhotoToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Small, False)

                        Case "PHOTOMEDIUM"
                            ProcessPhotoToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Medium, False)

                        Case "PHOTOLARGE"
                            ProcessPhotoToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Large, False)

                        Case "PHOTOHEIGHTSMALL"
                            ProcessPhotoHeightToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Small, False)

                        Case "PHOTOHEIGHTMEDIUM"
                            ProcessPhotoHeightToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Medium, False)

                        Case "PHOTOHEIGHTLARGE"
                            ProcessPhotoHeightToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Large, False)

                        Case "PHOTOLINKSMALL"
                            ProcessPhotoLinkToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Small, False)

                        Case "PHOTOLINKMEDIUM"
                            ProcessPhotoLinkToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Medium, False)

                        Case "PHOTOLINKLARGE"
                            ProcessPhotoLinkToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Large, False)

                        Case "PHOTOURL"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & photoPtr & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.ExternalUrl
                            objPlaceHolder.Add(objLiteral)

                        Case "PHOTOWIDTHSMALL"
                            ProcessPhotoWidthToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Small, False)

                        Case "PHOTOWIDTHMEDIUM"
                            ProcessPhotoWidthToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Medium, False)

                        Case "PHOTOWIDTHLARGE"
                            ProcessPhotoWidthToken(objPlaceHolder, objPhoto, photoPtr & "-" & iPtr.ToString(), ThumbnailType.Large, False)

                        Case "PROPERTY"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & photoPtr & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.PropertyID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PHOTOTITLE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & photoPtr & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.Title
                            objPlaceHolder.Add(objLiteral)

                        Case "PHOTOLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & photoPtr & "-" & iPtr.ToString())
                            objLiteral.Text = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & _portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Images/" & objPhoto.Filename)
                            objPlaceHolder.Add(objLiteral)

                        Case "TEMPLATEPATH"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & photoPtr & "-" & iPtr.ToString())
                            objLiteral.Text = _portalSettings.HomeDirectory & "PropertyAgent/" & _moduleID.ToString() & "/Templates/" & _propertySettings.Template & "/"
                            objPlaceHolder.Add(objLiteral)

                        Case Else


                            Dim objLiteralOther As New Literal
                            objLiteralOther.ID = Globals.CreateValidID(_moduleKey & objPhoto.PropertyID.ToString() & "-Photo-" & objPhoto.PhotoID.ToString() & "-" & photoPtr & "-" & iPtr.ToString())
                            objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            objPlaceHolder.Add(objLiteralOther)

                    End Select
                End If

            Next

        End Sub

        Public Function GetTypeLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal objTypeSelected As PropertyTypeInfo, ByVal agentFilter As String, ByVal currentPage As Integer) As String

            Return GetTypeLink(tabID, moduleID, objTypeSelected, agentFilter, currentPage, Null.NullString, Null.NullString)

        End Function

        Public Function GetTypeLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal objTypeSelected As PropertyTypeInfo, ByVal agentFilter As String, ByVal currentPage As Integer, ByVal sortBy As String, ByVal sortDir As String) As String

            Dim objTypesParam As New List(Of String)

            objTypesParam.Add(_propertySettings.SEOAgentType & "=ViewType")
            objTypesParam.Add(_propertySettings.SEOPropertyTypeID & "=" & objTypeSelected.PropertyTypeID.ToString())

            If (agentFilter <> "") Then
                objTypesParam.Add("AgentFilter=" & HttpContext.Current.Server.UrlEncode(agentFilter))
            End If

            If (sortBy <> Null.NullString) Then
                objTypesParam.Add("sortBy=" & sortBy)
            End If

            If (sortDir <> Null.NullString) Then
                objTypesParam.Add("sortDir=" & sortDir)
            End If

            If (currentPage <> Null.NullInteger) Then
                objTypesParam.Add("currentpage=" & currentPage.ToString())
            End If

            If (_propertySettings.TypeParams) Then
                Dim types As New List(Of String)

                Dim objPropertyTypeController As New PropertyTypeController
                Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(moduleID, True, PropertyTypeSortByType.Standard, Null.NullString())

                For Each objType As PropertyTypeInfo In objTypes
                    If (objType.PropertyTypeID = objTypeSelected.PropertyTypeID) Then
                        types.Add(objType.Name)

                        Dim i As Integer = 2
                        While objType.ParentID <> Null.NullInteger
                            For Each objParentType As PropertyTypeInfo In objTypes
                                If (objParentType.PropertyTypeID = objType.ParentID) Then
                                    types.Add(objParentType.Name)
                                    objType = objParentType
                                    i = i + 1
                                End If
                            Next
                        End While
                    End If
                Next

                Dim length As Integer = types.Count
                For Each t As String In types
                    objTypesParam.Insert(2, "Type" & length.ToString() & "=" & t)
                    length = length - 1
                Next
            End If

            If DotNetNuke.Entities.Host.HostSettings.GetHostSetting("UseFriendlyUrls") = "Y" Then

                Dim strURL As String = ApplicationURL(_tabID)
                Dim settings As PortalSettings = PortalController.GetCurrentPortalSettings

                For Each p As String In objTypesParam
                    strURL = strURL & "&" & p
                Next

                Dim objTabController As New TabController
                Dim objTab As TabInfo = objTabController.GetTab(_tabID, settings.PortalId, False)

                Dim title As String = "Default.aspx"

                If (_propertySettings.SEOViewTypeTitle <> "") Then

                    Dim delimStr As String = "[]"
                    Dim delimiter As Char() = delimStr.ToCharArray()

                    Dim phPageTitle As New PlaceHolder()
                    ProcessType(phPageTitle.Controls, _propertySettings.SEOViewTypeTitle.Split(delimiter), objTypeSelected, agentFilter)
                    title = OnlyAlphaNumericChars(RenderControlToString(phPageTitle))
                    If (title Is Nothing OrElse title.Trim() = "") Then
                        title = "Default.aspx"
                    Else
                        title = title.Replace("--", "-") & ".aspx"
                    End If
                End If

                Dim link As String = FriendlyUrl(objTab, strURL, title, settings)

                If (link.ToLower().StartsWith("http://") Or link.ToLower().StartsWith("https://")) Then
                    Return link
                Else
                    If (System.Web.HttpContext.Current.Request.Url.Port = 80) Then
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & link)
                    Else
                        Return AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & ":" & System.Web.HttpContext.Current.Request.Url.Port.ToString() & link)
                    End If
                End If

            Else
                Return NavigateURL(_tabID, "", objTypesParam.ToArray())
            End If

        End Function

        Public Sub ProcessType(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objType As PropertyTypeInfo, ByVal agentFilter As String)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "EDIT"
                            If _isEditable Then
                                Dim objHyperLink As New HyperLink
                                objHyperLink.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                                objHyperLink.NavigateUrl = NavigateURL(_tabID, "", _propertySettings.SEOAgentType & "=EditPropertyType", _propertySettings.SEOPropertyTypeID & "=" & objType.PropertyTypeID.ToString())
                                objHyperLink.ImageUrl = "~/images/edit.gif"
                                objHyperLink.EnableViewState = False
                                objPlaceHolder.Add(objHyperLink)
                            End If

                        Case "PIPPO"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = "Pippo"
                            objPlaceHolder.Add(objLiteral)

                        Case "FIRSTTYPELINK"
                            Dim objPropertyController As New PropertyController
                            Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(_moduleID, objType.PropertyTypeID, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, False, _propertySettings.ListingSortBy, _propertySettings.ListingSortByCustomField, _propertySettings.ListingSortDirection, _propertySettings.ListingSortBy2, _propertySettings.ListingSortByCustomField2, _propertySettings.ListingSortDirection2, _propertySettings.ListingSortBy3, _propertySettings.ListingSortByCustomField3, _propertySettings.ListingSortDirection3, Null.NullString, Null.NullString, 0, 1, 1, _propertySettings.ListingBubbleFeatured, _propertySettings.ListingSearchSubTypes, Null.NullInteger, Null.NullInteger, Null.NullDouble, Null.NullDouble, Null.NullDate, Null.NullString, Null.NullInteger)

                            If (objProperties.Count > 0) Then
                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(_moduleID, True)

                                Dim objLiteral As New Literal
                                objLiteral.Text = GetExternalLink(GetPropertyLink(objProperties(0), objCustomFields))
                                objPlaceHolder.Add(objLiteral)
                            End If

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

                        Case "SUBTYPES"
                            If (objType.PropertyTypeCount > 0) Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                                Dim objTypeController As New PropertyTypeController
                                Dim objTypes As List(Of PropertyTypeInfo) = objTypeController.List(_moduleID, True, _propertySettings.TypesSortBy, Null.NullString(), objType.PropertyTypeID)
                                For Each objSubType As PropertyTypeInfo In objTypes
                                    If (objLiteral.Text <> "") Then
                                        objLiteral.Text = objLiteral.Text & ", " & "<a href=""" & GetTypeLink(_tabID, _moduleID, objSubType, agentFilter, Null.NullInteger) & """>" & objSubType.Name & "</a>"
                                    Else
                                        objLiteral.Text = "<a href=""" & GetTypeLink(_tabID, _moduleID, objSubType, agentFilter, Null.NullInteger) & """>" & objSubType.Name & "</a>"
                                    End If
                                Next
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "SUBTYPESCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objType.PropertyTypeCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objType.Name
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objType.PropertyCount.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEDESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objType.Description
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEIMAGE"
                            If (objType.ImageFile <> "") Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = "<img src=""" & _portalSettings.HomeDirectory & objType.ImageFile & """ border=""0"">"
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "TYPEIMAGELINK"
                            If (objType.ImageFile <> "") Then
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = _portalSettings.HomeDirectory & objType.ImageFile
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "TYPEINDENTED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objType.NameIndented
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPELINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetTypeLink(_tabID, _moduleID, objType, agentFilter, Null.NullInteger)
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objType.PropertyTypeID.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("RSSREADER:")) Then

                                Try

                                    Dim field As String = layoutArray(iPtr + 1).Substring(10, layoutArray(iPtr + 1).Length - 10)

                                    Dim count As Integer = Null.NullInteger
                                    Dim fieldParts As String() = field.Split(":"c)
                                    If (fieldParts.Length > 1) Then
                                        If (IsNumeric(fieldParts(fieldParts.Length - 1))) Then
                                            count = fieldParts(fieldParts.Length - 1)
                                            field = ""
                                            For i As Integer = 0 To fieldParts.Length - 2
                                                If (i > 0) Then
                                                    field = field & ":" & fieldParts(i)
                                                Else
                                                    field = field & fieldParts(i)
                                                End If
                                            Next
                                        End If
                                    End If

                                    Dim delimStr As String = "{}"
                                    Dim delimiter As Char() = delimStr.ToCharArray()
                                    Dim phRssUrl As New PlaceHolder
                                    ProcessType(phRssUrl.Controls, field.Split(delimiter), objType, Null.NullString())
                                    field = RenderControlToString(phRssUrl)

                                    Dim doc As XPathDocument
                                    Dim navigator As XPathNavigator
                                    Dim nodes As XPathNodeIterator
                                    Dim node As XPathNavigator

                                    ' Create a new XmlDocument  
                                    doc = New XPathDocument(field)

                                    ' Create navigator  
                                    navigator = doc.CreateNavigator()

                                    ' Get forecast with XPath  
                                    nodes = navigator.Select("/rss/channel/item")

                                    Dim objRssItems As New List(Of RssInfo)

                                    While nodes.MoveNext()
                                        node = nodes.Current

                                        Dim nodeTitle As XPathNavigator
                                        Dim nodeDescription As XPathNavigator
                                        Dim nodeLink As XPathNavigator

                                        nodeTitle = node.SelectSingleNode("title")
                                        nodeDescription = node.SelectSingleNode("description")
                                        nodeLink = node.SelectSingleNode("link")

                                        Dim objRssItem As New RssInfo()

                                        objRssItem.Title = nodeTitle.Value
                                        objRssItem.Description = nodeDescription.Value
                                        objRssItem.Link = nodeLink.Value

                                        objRssItems.Add(objRssItem)
                                    End While

                                    If (objRssItems.Count > 0) Then

                                        Dim objHeader As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.RSS_Reader_Header_Html)
                                        Dim objItem As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.RSS_Reader_Item_Html)
                                        Dim objFooter As LayoutInfo = GetLayout(_propertySettings.Template, LayoutType.RSS_Reader_Footer_Html)

                                        ProcessHeaderFooter(objPlaceHolder, objHeader.Tokens)
                                        Dim i As Integer = 1
                                        For Each objRssItem As RssInfo In objRssItems
                                            If (count = Null.NullInteger Or (count <> Null.NullInteger And i < (count + 1))) Then
                                                ProcessRssReaderItem(objPlaceHolder, objItem.Tokens, objRssItem, iPtr.ToString() & "-" & i.ToString())
                                                i = i + 1
                                            End If
                                        Next
                                        ProcessHeaderFooter(objPlaceHolder, objFooter.Tokens)

                                    End If

                                Catch
                                End Try

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("SUBTYPES:")) Then
                                Dim count As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)
                                If (Int32.TryParse(count, Nothing)) Then
                                    Dim limit As Integer = Int32.Parse(count)

                                    Dim actual As Integer = 0
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("PropertyAgent" & objType.PropertyTypeID.ToString() & "-" & iPtr.ToString())
                                    Dim objTypeController As New PropertyTypeController
                                    Dim objTypes As List(Of PropertyTypeInfo) = objTypeController.List(_moduleID, True, _propertySettings.TypesSortBy, Null.NullString(), objType.PropertyTypeID)
                                    For Each objSubType As PropertyTypeInfo In objTypes
                                        If (objLiteral.Text <> "") Then
                                            objLiteral.Text = objLiteral.Text & ", " & "<a href=""" & GetTypeLink(_tabID, _moduleID, objSubType, agentFilter, Null.NullInteger) & """>" & objSubType.Name & "</a>"
                                        Else
                                            objLiteral.Text = "<a href=""" & GetTypeLink(_tabID, _moduleID, objSubType, agentFilter, Null.NullInteger) & """>" & objSubType.Name & "</a>"
                                        End If
                                        actual = actual + 1
                                        If (actual >= limit) Then
                                            Exit For
                                        End If
                                    Next
                                    objPlaceHolder.Add(objLiteral)

                                End If
                            End If

                    End Select
                End If

            Next

        End Sub

        Public Sub ProcessMessage(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal message As String)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

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
                            ' Do Nothing.

                        Case "MESSAGE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(_moduleKey & "-Message-" & iPtr.ToString())
                            objLiteral.Text = message
                            objPlaceHolder.Add(objLiteral)

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(9, layoutArray(iPtr + 1).Length - 9)
                                If (PortalSecurity.IsInRole(field) = False) Then

                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While

                                End If

                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("ISNOTINROLE:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(12, layoutArray(iPtr + 1).Length - 12)
                                If (PortalSecurity.IsInRole(field) = True) Then

                                    Dim endToken As String = "/" & layoutArray(iPtr + 1)
                                    While (iPtr < layoutArray.Length - 1)
                                        If (layoutArray(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While

                                End If

                            End If

                    End Select
                End If

            Next

        End Sub

#End Region

    End Class

End Namespace
