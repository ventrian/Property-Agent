Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security

Imports Ventrian.PropertyAgent.Mapping
Imports System.Globalization

Namespace Ventrian.PropertyAgent

    Partial Public Class ViewType
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _agentType As String = Null.NullString
        Private _agentFilter As String = Null.NullString

        Private _currentPage As Integer = 1
        Private _pageRecords As Integer = 10

        Private _propertyTypeID As Integer = Null.NullInteger
        Private _propertyAgentID As Integer = Null.NullInteger
        Private _propertyAgentName As String = Null.NullInteger
        Private _propertyBrokerID As Integer = Null.NullInteger
        Private _location As String = Null.NullString

        Private _layoutController As LayoutController

        Private _objLayout As LayoutInfo
        Private _objLayoutAlternating As LayoutInfo
        Private _objLayoutHeader As LayoutInfo
        Private _objLayoutFooter As LayoutInfo
        Private _objLayoutSeparator As LayoutInfo

        Private _objLayoutSearch As LayoutInfo
        Private _objLayoutSearchHeader As LayoutInfo
        Private _objLayoutSearchFooter As LayoutInfo

        Private _objLayoutType As LayoutInfo
        Private _objLayoutTypeHeader As LayoutInfo
        Private _objLayoutTypeFooter As LayoutInfo
        Private _objLayoutTypeTitle As LayoutInfo
        Private _objLayoutTypeDescription As LayoutInfo
        Private _objLayoutTypeKeyword As LayoutInfo
        Private _objLayoutTypePageHead As LayoutInfo

        Private _objMessage As LayoutInfo

        Private _objProperties As List(Of PropertyInfo)
        Private _totalRecords As Integer = 0

        Private _customFieldIDs As String = Null.NullString
        Private _searchValues As String = Null.NullString

        Private _sortBy As String = ""
        Private _sortDirection As String = ""

#End Region

#Region " Private Properties "

        Private ReadOnly Property SortBy() As SortByType
            Get
                Try
                    If (drpSortBy.SelectedValue.StartsWith("cf")) Then
                        Return SortByType.CustomField
                    Else

                        If (drpSortBy.SelectedValue.StartsWith("rf")) Then
                            Return SortByType.ReviewField
                        Else
                            Return CType(System.Enum.Parse(GetType(SortByType), drpSortBy.SelectedValue), SortByType)
                        End If
                    End If
                Catch
                    Return Me.PropertySettings.ListingSortBy
                End Try
            End Get
        End Property

        Private ReadOnly Property SortByCustomField() As Integer
            Get
                Try
                    If (SortBy = SortByType.CustomField Or SortBy = SortByType.ReviewField) Then
                        Return Convert.ToInt32(drpSortBy.SelectedValue.Replace("cf", "").Replace("rf", ""))
                    End If
                Catch
                    Return Me.PropertySettings.ListingSortByCustomField
                End Try
            End Get
        End Property

        Private ReadOnly Property SortDirection() As SortDirectionType
            Get
                Try
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), drpSortDirection.SelectedValue), SortDirectionType)
                Catch
                    Return Me.PropertySettings.ListingSortDirection
                End Try
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

            If (PropertySettings.SEOTitleReplacement = TitleReplacementType.Dash) Then
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

        Private Sub ReadQueryString()

            Dim agentTypeParam As String = PropertySettings.SEOAgentType
            If (Request(agentTypeParam) = "") Then
                agentTypeParam = "agentType"
            End If
            If Not (Request(agentTypeParam) Is Nothing) Then
                _agentType = Request(agentTypeParam)
            End If

            Dim propertyTypeIDParam As String = PropertySettings.SEOPropertyTypeID
            If (Request(propertyTypeIDParam) = "") Then
                propertyTypeIDParam = "PropertyTypeID"
            End If
            If Not (Request(propertyTypeIDParam) Is Nothing) Then
                Integer.TryParse(Request(propertyTypeIDParam), _propertyTypeID)
                If (_propertyTypeID = 0) Then
                    Response.Redirect(NavigateURL(Me.TabId), True)
                End If
            End If

            If Not (Request("AgentFilter") Is Nothing) Then
                _agentFilter = Server.UrlDecode(Request("AgentFilter"))
            End If

            If Not (Request("PropertyAgentID") Is Nothing) Then
                _propertyAgentID = Convert.ToInt32(Request("PropertyAgentID"))
            End If
            If Not (Request("PropertyAgentName") Is Nothing) Then
                _propertyAgentName = Request("PropertyAgentName")
            End If
            If Not (Request("PropertyBrokerID") Is Nothing) Then
                _propertyBrokerID = Convert.ToInt32(Request("PropertyBrokerID"))
            End If

            If Not (Request("Location") Is Nothing) Then
                _location = Server.UrlDecode(Request("Location"))
            End If

            If Not (Request("CurrentPage") Is Nothing) Then
                _currentPage = Convert.ToInt32(Request("CurrentPage"))
            End If

            If Not (Request("CustomFieldIDs") Is Nothing) Then
                _customFieldIDs = Request("CustomFieldIDs").Trim()
            End If

            If Not (Request("SearchValues") Is Nothing) Then
                _searchValues = Request("SearchValues").Trim()
            End If

            If Not (Request("sortBy") Is Nothing) Then
                _sortBy = Request("sortBy").Trim()
            End If

            If Not (Request("sortDir") Is Nothing) Then
                _sortDirection = Request("sortDir").Trim()
            End If

            _pageRecords = Me.PropertySettings.ListingItemsPerPage

        End Sub

        Private Sub InitializeTemplate()

            _layoutController = New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)

            _objLayout = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Item_Html)
            _objLayoutAlternating = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Alternate_Html)
            _objLayoutHeader = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Header_Html)
            _objLayoutFooter = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Footer_Html)
            _objLayoutSeparator = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Separator_Html)

            _objLayoutSearch = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Item_Html)
            _objLayoutSearchHeader = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Header_Html)
            _objLayoutSearchFooter = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Footer_Html)

            _objLayoutType = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Item_Html)
            _objLayoutTypeHeader = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Header_Html)
            _objLayoutTypeFooter = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Footer_Html)
            _objLayoutTypeTitle = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageTitle_Html)
            _objLayoutTypeDescription = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageDescription_Html)
            _objLayoutTypeKeyword = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageKeyword_Html)
            _objLayoutTypePageHead = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageHeader_Html)

            _objMessage = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Message_Item_Html)

        End Sub

        Private Sub InitializeStylesheet()

            _layoutController.LoadStyleSheet(Me.PropertySettings.Template)

        End Sub

        Private Sub BindSortBy()

            If (PropertySettings.ListingSortFields <> "") Then

                For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                    Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)

                    If (objSortByType = SortByType.CustomField) Then
                        For Each item As String In PropertySettings.ListingSortFields.Split(","c)
                            If (item = System.Enum.GetName(GetType(SortByType), value)) Then
                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleId, True)

                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.IsSortable) Then
                                        Dim li As New ListItem
                                        li.Value = "cf" & objCustomField.CustomFieldID.ToString()
                                        li.Text = objCustomField.Caption
                                        drpSortBy.Items.Add(li)
                                    End If
                                Next
                            End If
                        Next
                    Else
                        If (objSortByType = SortByType.ReviewField) Then
                            Dim objReviewFieldController As New ReviewFieldController
                            Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(Me.ModuleId)

                            For Each objReviewField As ReviewFieldInfo In objReviewFields
                                If (objReviewField.FieldType = ReviewFieldType.Rating) Then
                                    Dim li As New ListItem
                                    li.Value = "rf" & objReviewField.ReviewFieldID.ToString()
                                    li.Text = objReviewField.Caption
                                    drpSortBy.Items.Add(li)
                                End If
                            Next
                        Else
                            For Each item As String In PropertySettings.ListingSortFields.Split(","c)
                                If (item = System.Enum.GetName(GetType(SortByType), value)) Then
                                    Dim li As New ListItem
                                    li.Value = System.Enum.GetName(GetType(SortByType), value)
                                    li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                                    drpSortBy.Items.Add(li)
                                End If
                            Next
                        End If
                    End If
                Next

                If (_sortBy <> "") Then
                    Dim objSortBy As SortByType = Me.PropertySettings.ListingSortBy

                    If (_sortBy.StartsWith("cf")) Then
                        objSortBy = SortByType.CustomField
                        If Not (drpSortBy.Items.FindByValue(_sortBy) Is Nothing) Then
                            drpSortBy.SelectedValue = _sortBy
                        End If
                    Else
                        If (_sortBy.StartsWith("rf")) Then
                            objSortBy = SortByType.CustomField
                            If Not (drpSortBy.Items.FindByValue(_sortBy) Is Nothing) Then
                                drpSortBy.SelectedValue = _sortBy
                            End If
                        Else
                            objSortBy = CType(System.Enum.Parse(GetType(SortByType), _sortBy, True), SortByType)
                            If Not (drpSortBy.Items.FindByValue(objSortBy.ToString) Is Nothing) Then
                                drpSortBy.SelectedValue = objSortBy.ToString
                            End If
                        End If
                    End If
                Else
                    If (Me.PropertySettings.ListingSortBy <> SortByType.CustomField And Me.PropertySettings.ListingSortBy <> SortByType.ReviewField) Then
                        If Not (drpSortBy.Items.FindByValue(Me.PropertySettings.ListingSortBy.ToString()) Is Nothing) Then
                            drpSortBy.SelectedValue = Me.PropertySettings.ListingSortBy.ToString()
                        Else
                            For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                                If (Me.PropertySettings.ListingSortBy.ToString() = System.Enum.GetName(GetType(SortByType), value)) Then
                                    Dim li As New ListItem
                                    li.Value = System.Enum.GetName(GetType(SortByType), value)
                                    li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                                    li.Selected = True
                                    drpSortBy.Items.Add(li)
                                End If
                            Next
                        End If
                    Else
                        If (Me.PropertySettings.ListingSortBy = SortByType.CustomField) Then
                            If Not (drpSortBy.Items.FindByValue("cf" & Me.PropertySettings.ListingSortByCustomField.ToString()) Is Nothing) Then
                                drpSortBy.SelectedValue = "cf" & Me.PropertySettings.ListingSortByCustomField.ToString()
                            Else
                                ' Sort by value not shown, now add it.
                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomField As CustomFieldInfo = objCustomFieldController.Get(Convert.ToInt32(Me.PropertySettings.ListingSortByCustomField.ToString()))

                                If (objCustomField IsNot Nothing) Then
                                    If (objCustomField.IsSortable) Then
                                        Dim li As New ListItem
                                        li.Value = "cf" & objCustomField.CustomFieldID.ToString()
                                        li.Text = objCustomField.Caption
                                        li.Selected = True
                                        drpSortBy.Items.Add(li)
                                    End If
                                End If
                            End If
                        End If

                        If (Me.PropertySettings.ListingSortBy = SortByType.ReviewField) Then
                            If Not (drpSortBy.Items.FindByValue("rf" & Me.PropertySettings.ListingSortByCustomField.ToString()) Is Nothing) Then
                                drpSortBy.SelectedValue = "rf" & Me.PropertySettings.ListingSortByCustomField.ToString()
                            Else
                                ' Sort by value not shown, now add it.
                                Dim objReviewFieldController As New ReviewFieldController
                                Dim objReviewField As ReviewFieldInfo = objReviewFieldController.Get(Convert.ToInt32(Me.PropertySettings.ListingSortByCustomField.ToString()))

                                If (objReviewField IsNot Nothing) Then
                                    Dim li As New ListItem
                                    li.Value = "rf" & objReviewField.ReviewFieldID.ToString()
                                    li.Text = objReviewField.Caption
                                    li.Selected = True
                                    drpSortBy.Items.Add(li)
                                End If
                            End If
                        End If

                    End If
                End If

            End If

        End Sub

        Private Sub BindSortDirection()

            For Each value As Integer In System.Enum.GetValues(GetType(SortDirectionType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortDirectionType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortDirectionType), value), Me.LocalResourceFile)

                If (value = SortDirectionType.Descending) Then
                    li.Selected = True
                End If
                drpSortDirection.Items.Add(li)
            Next

            If (_sortDirection <> "") Then
                Dim objSortDirection As SortDirectionType = CType(System.Enum.Parse(GetType(SortDirectionType), _sortDirection, True), SortDirectionType)
                If Not (drpSortDirection.Items.FindByValue(objSortDirection.ToString()) Is Nothing) Then
                    drpSortDirection.SelectedValue = objSortDirection.ToString()
                Else
                    If Not (drpSortDirection.Items.FindByValue(Me.PropertySettings.ListingSortDirection.ToString()) Is Nothing) Then
                        drpSortDirection.SelectedValue = Me.PropertySettings.ListingSortDirection.ToString()
                    End If
                End If
            Else
                If Not (drpSortDirection.Items.FindByValue(Me.PropertySettings.ListingSortDirection.ToString()) Is Nothing) Then
                    drpSortDirection.SelectedValue = Me.PropertySettings.ListingSortDirection.ToString()
                End If
            End If

        End Sub

        Private Sub BindProperty()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim showMessage As Boolean = True

            Select Case _agentType.ToLower()

                Case "viewfeatured"
                    Dim objCrumbFeatured As New CrumbInfo
                    objCrumbFeatured.Caption = Localization.GetString("Featured", Me.LocalResourceFile)
                    objCrumbFeatured.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=" & _agentType)
                    crumbs.Add(objCrumbFeatured)

                Case "viewsearch"
                    Dim objCrumbSearch As New CrumbInfo
                    objCrumbSearch.Caption = Localization.GetString("SearchResults", Me.LocalResourceFile)
                    objCrumbSearch.Url = GetListingUrl(_currentPage)
                    crumbs.Add(objCrumbSearch)

                Case "viewtype"
                    Dim objPropertyTypeController As New PropertyTypeController
                    Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, _propertyTypeID)

                    If Not (objPropertyType Is Nothing) Then

                        Dim sortByParam As String = Null.NullString
                        Dim sortDirParam As String = Null.NullString

                        If (Me.PropertySettings.ListingUserSortable) Then
                            If (SortBy <> Me.PropertySettings.ListingSortBy) Then
                                sortByParam = drpSortBy.SelectedValue
                            End If
                        End If

                        If (SortDirection <> Me.PropertySettings.ListingSortDirection) Then
                            sortDirParam = drpSortDirection.SelectedValue
                        End If

                        Dim typeUrl As String = ""

                        If (_currentPage = Null.NullInteger Or _currentPage = 1) Then
                            typeUrl = _layoutController.GetExternalLink(_layoutController.GetTypeLink(Me.TabId, Me.ModuleId, objPropertyType, "", Null.NullInteger, sortByParam, sortDirParam))
                        Else
                            typeUrl = _layoutController.GetExternalLink(_layoutController.GetTypeLink(Me.TabId, Me.ModuleId, objPropertyType, "", _currentPage, sortByParam, sortDirParam))
                        End If

                        If (PropertySettings.SEORedirect) Then
                            If (typeUrl.ToLower() <> _layoutController.GetExternalLink(Request.RawUrl).ToLower()) Then
                                Response.Status = "301 Moved Permanently"
                                Response.AddHeader("Location", typeUrl)
                                Response.End()
                            End If
                        End If

                        Dim parentID As Integer = objPropertyType.ParentID

                        Do
                            Dim objCrumbType As New CrumbInfo
                            objCrumbType.Caption = objPropertyType.Name
                            objCrumbType.Url = _layoutController.GetExternalLink(_layoutController.GetTypeLink(Me.TabId, Me.ModuleId, objPropertyType, "", Null.NullInteger))
                            If (_propertyTypeID = objPropertyType.PropertyTypeID) Then
                                crumbs.Add(objCrumbType)
                            Else
                                crumbs.Insert(1, objCrumbType)
                            End If

                            objPropertyType = objPropertyTypeController.Get(Me.ModuleId, objPropertyType.ParentID)
                            If (objPropertyType Is Nothing) Then
                                parentID = Null.NullInteger
                            Else
                                parentID = objPropertyType.PropertyTypeID
                            End If
                        Loop While (parentID <> Null.NullInteger)

                    End If

                    Dim objPropertyTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.List(Me.ModuleId, True, PropertySettings.TypesSortBy, Null.NullString(), _propertyTypeID)

                    Dim objTypesSelected As New List(Of PropertyTypeInfo)

                    For Each objType As PropertyTypeInfo In objPropertyTypes
                        If (PropertySettings.TypesHideZero) Then
                            If (objType.PropertyCount > 0) Then
                                objTypesSelected.Add(objType)
                            End If
                        Else
                            objTypesSelected.Add(objType)
                        End If
                    Next

                    If (objTypesSelected.Count > 0) Then
                        showMessage = False
                        ProcessHeaderFooter(phProperty.Controls, _objLayoutTypeHeader.Tokens)

                        If (PropertySettings.ListingLayoutType = LatestLayoutType.TableLayout) Then
                            Dim objDataList As New System.Web.UI.WebControls.DataList
                            Dim objHandler As New DataListItemEventHandler(AddressOf dlTypes_ItemDataBound)
                            AddHandler objDataList.ItemDataBound, objHandler

                            objDataList.CellPadding = 0
                            objDataList.CellSpacing = 0

                            objDataList.RepeatColumns = Me.PropertySettings.TypesItemsPerRow
                            objDataList.RepeatDirection = Me.PropertySettings.TypesRepeatDirection

                            objDataList.DataSource = objTypesSelected
                            objDataList.DataBind()

                            phProperty.Controls.Add(objDataList)
                        Else
                            Dim objRepeater As New System.Web.UI.WebControls.Repeater
                            Dim objHandler As New RepeaterItemEventHandler(AddressOf rptTypes_ItemDataBound)
                            AddHandler objRepeater.ItemDataBound, objHandler

                            objRepeater.DataSource = objTypesSelected
                            objRepeater.DataBind()

                            phProperty.Controls.Add(objRepeater)
                        End If


                        ProcessHeaderFooter(phProperty.Controls, _objLayoutTypeFooter.Tokens)
                    Else
                        ProcessHeaderFooter(phProperty.Controls, _objLayoutTypeHeader.Tokens)
                    End If

            End Select

            If (PropertySettings.BreadcrumbPlacement = BreadcrumbType.Portal) Then
                For i As Integer = 0 To crumbs.Count - 1
                    Dim objCrumb As CrumbInfo = crumbs(i)
                    If (i > 0) Then
                        Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo
                        objTab.TabID = -8888 + i
                        objTab.TabName = objCrumb.Caption
                        objTab.Url = objCrumb.Url
                        PortalSettings.ActiveTab.BreadCrumbs.Add(objTab)
                    End If
                Next
            End If

            If (PropertySettings.BreadcrumbPlacement = BreadcrumbType.Module) Then
                rptBreadCrumbs.DataSource = crumbs
                rptBreadCrumbs.DataBind()
            End If

            Dim latitude As Double = Null.NullDouble
            Dim longitude As Double = Null.NullDouble

            If (_location <> "") Then
                Dim objGeoCodeInfo As GeocodeInfo = Geocoder.GeoCode(_location, PropertySettings)
                If (objGeoCodeInfo.Latitude <> 0 And objGeoCodeInfo.Longitude <> 0) Then
                    latitude = objGeoCodeInfo.Latitude
                    longitude = objGeoCodeInfo.Longitude
                End If
            End If

            If (SortBy = SortByType.Distance And latitude = Null.NullDouble And longitude = Null.NullDouble) Then
                Dim objGeoCodeInfo As GeocodeInfo = UserGeoLocator.GetUserLocation(Request.UserHostAddress)
                If (objGeoCodeInfo IsNot Nothing) Then
                    latitude = objGeoCodeInfo.Latitude
                    longitude = objGeoCodeInfo.Longitude
                End If
            End If

            Dim objPropertyController As New PropertyController
            Dim OnlyForAuthenticated As Boolean = Null.NullBoolean
            If Me.UserId = -1 Then OnlyForAuthenticated = True
            Select Case _agentType.ToLower()

                Case "viewfeatured"
                    divSearch.Visible = False
                    _objProperties = objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, True, OnlyForAuthenticated, SortBy, SortByCustomField, SortDirection, PropertySettings.ListingSortBy2, PropertySettings.ListingSortByCustomField2, PropertySettings.ListingSortDirection2, PropertySettings.ListingSortBy3, PropertySettings.ListingSortByCustomField3, PropertySettings.ListingSortDirection3, Null.NullString, Null.NullString, _currentPage - 1, _pageRecords, _totalRecords, False, False, Null.NullInteger, Null.NullInteger, latitude, longitude, Null.NullDate, Null.NullString, Null.NullInteger)

                Case "viewsearch"

                    If (SortBy = SortByType.Distance And latitude = Null.NullDouble And longitude = Null.NullDouble) Then
                        divSearch.Visible = False
                        divExport.Visible = False
                        lnkExport.Visible = False
                        _layoutController.ProcessMessage(phProperty.Controls, _objMessage.Tokens, Localization.GetString("DistanceSortError.Text", Me.LocalResourceFile))
                        Return
                    End If

                    If (_customFieldIDs.Trim() <> "" And _searchValues.Trim() <> "") Then
                        divSearch.Visible = True
                        lnkSearchAgain.NavigateUrl = NavigateURL(Me.TabId)
                        If (_customFieldIDs.Trim() <> "" And _searchValues.Trim() <> "") Or _propertyAgentID <> Null.NullInteger Or _propertyBrokerID <> Null.NullInteger Then
                            lnkNarrowThisSearch.NavigateUrl = NavigateURL(Me.TabId, "", GetAdditionalParams("", 1, ""))
                        Else
                            lnkNarrowThisSearch.Visible = False
                        End If
                    Else
                        divSearch.Visible = False
                    End If
                    If _propertyAgentName <> "-1" Then
                        _propertyAgentID = 0
                        Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)
                        Dim arrAgents As ArrayList
                        arrAgents = objAgentController.ListActive(PortalId, ModuleId)
                        For Each Agent As DotNetNuke.Entities.Users.UserInfo In arrAgents
                            If Agent.Username = _propertyAgentName Then
                                _propertyAgentID = Agent.UserID
                                Exit For
                            End If
                        Next
                        'Dim TheAgent As New DotNetNuke.Entities.Users.UserInfo
                        'TheAgent = Array.Find(TheAgent, Function(c As DotNetNuke.Entities.Users.UserInfo) c.Username = _propertyAgentName)
                    End If
                    _objProperties = objPropertyController.List(Me.ModuleId, _propertyTypeID, SearchStatusType.PublishedActive, _propertyAgentID, _propertyBrokerID, False, OnlyForAuthenticated, SortBy, SortByCustomField, SortDirection, PropertySettings.ListingSortBy2, PropertySettings.ListingSortByCustomField2, PropertySettings.ListingSortDirection2, PropertySettings.ListingSortBy3, PropertySettings.ListingSortByCustomField3, PropertySettings.ListingSortDirection3, _customFieldIDs, _searchValues, _currentPage - 1, _pageRecords, _totalRecords, PropertySettings.ListingBubbleFeatured, True, Null.NullInteger, Null.NullInteger, latitude, longitude, Null.NullDate, Null.NullString, Null.NullInteger)

                Case "viewtype"
                    divSearch.Visible = False
                    _objProperties = objPropertyController.List(Me.ModuleId, _propertyTypeID, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, OnlyForAuthenticated, SortBy, SortByCustomField, SortDirection, PropertySettings.ListingSortBy2, PropertySettings.ListingSortByCustomField2, PropertySettings.ListingSortDirection2, PropertySettings.ListingSortBy3, PropertySettings.ListingSortByCustomField3, PropertySettings.ListingSortDirection3, Null.NullString, Null.NullString, _currentPage - 1, _pageRecords, _totalRecords, PropertySettings.ListingBubbleFeatured, Me.PropertySettings.ListingSearchSubTypes, Null.NullInteger, Null.NullInteger, latitude, longitude, Null.NullDate, _agentFilter, Null.NullInteger)

                Case Else

                    If (SortBy = SortByType.Distance And latitude = Null.NullDouble And longitude = Null.NullDouble) Then
                        divSearch.Visible = False
                        divExport.Visible = False
                        lnkExport.Visible = False
                        _layoutController.ProcessMessage(phProperty.Controls, _objMessage.Tokens, Localization.GetString("DistanceSortError.Text", Me.LocalResourceFile))
                        Return
                    End If

                    divSearch.Visible = (PropertySettings.LandingPageMode = LandingPageType.Standard)
                    lnkSearchAgain.NavigateUrl = NavigateURL(Me.TabId)
                    If (_customFieldIDs.Trim() <> "" And _searchValues.Trim() <> "") Or _propertyAgentID <> Null.NullInteger Or _propertyBrokerID <> Null.NullInteger Then
                        lnkNarrowThisSearch.NavigateUrl = NavigateURL(Me.TabId, "", GetAdditionalParams("", 1, ""))
                    Else
                        lnkNarrowThisSearch.Visible = False
                    End If
                    _objProperties = objPropertyController.List(Me.ModuleId, _propertyTypeID, SearchStatusType.PublishedActive, _propertyAgentID, _propertyBrokerID, False, OnlyForAuthenticated, SortBy, SortByCustomField, SortDirection, PropertySettings.ListingSortBy2, PropertySettings.ListingSortByCustomField2, PropertySettings.ListingSortDirection2, PropertySettings.ListingSortBy3, PropertySettings.ListingSortByCustomField3, PropertySettings.ListingSortDirection3, _customFieldIDs, _searchValues, _currentPage - 1, _pageRecords, _totalRecords, PropertySettings.ListingBubbleFeatured, True, Null.NullInteger, Null.NullInteger, latitude, longitude, Null.NullDate, Null.NullString, Null.NullInteger)

            End Select

            lnkExport.NavigateUrl = NavigateURL(Me.TabId, "", GetAdditionalParams("Export", 1, ""))

            If (PropertySettings.RssEnable) Then
                If (_agentType.ToLower() = "viewtype") Then
                    Dim objPropertyTypeController As New PropertyTypeController
                    Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, _propertyTypeID)
                    If Not (objPropertyType Is Nothing) Then

                        HttpContext.Current.Items.Add("RSS-PropertyAgent", NavigateURL(Me.TabId, "", GetAdditionalParams("rss", 1, PropertySettings.RssTitleType(objPropertyType.Name, True))))
                        HttpContext.Current.Items.Add("RSS-PropertyAgent-Title", PropertySettings.RssTitleType(objPropertyType.Name, True))

                    Else
                        HttpContext.Current.Items.Add("RSS-PropertyAgent", NavigateURL(Me.TabId, "", GetAdditionalParams("rss", 1, "")))
                        HttpContext.Current.Items.Add("RSS-PropertyAgent-Title", PropertySettings.RssTitleType("", True))

                    End If
                Else
                    HttpContext.Current.Items.Add("RSS-PropertyAgent", NavigateURL(Me.TabId, "", GetAdditionalParams("rss", 1, PropertySettings.RssTitleSearchResult(True))))
                    HttpContext.Current.Items.Add("RSS-PropertyAgent-Title", PropertySettings.RssTitleSearchResult(True))

                End If
            End If

            If (_objProperties.Count > 0) Then

                If (PropertySettings.ListingLayoutType = LatestLayoutType.TableLayout) Then
                    dlProperty.RepeatColumns = Me.PropertySettings.ListingItemsPerRow
                    dlProperty.RepeatDirection = RepeatDirection.Horizontal
                    dlProperty.DataSource = _objProperties
                    dlProperty.DataBind()
                Else
                    rptProperty.DataSource = _objProperties
                    rptProperty.DataBind()
                End If
            Else
                divSort.Visible = False
                If (showMessage) Then
                    _layoutController.ProcessMessage(phProperty.Controls, _objMessage.Tokens, Localization.GetString("NoResults", Me.LocalResourceFile))
                End If
            End If

        End Sub

        Private Sub ProcessSearch(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "SEARCH"
                            Dim objSearch As PropertyAgent.Search = CType(Me.LoadControl("Search.ascx"), PropertyAgent.Search)
                            objPlaceHolder.Add(objSearch)

                    End Select
                End If
            Next

        End Sub

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            Dim searchValueQuery As String = ""
            Dim customFieldIDsQuery As String = ""

            If (_searchValues <> "") Then
                searchValueQuery = "SearchValues=" & _searchValues
            End If

            If (_customFieldIDs <> "") Then
                customFieldIDsQuery = "CustomFieldIDs=" & _customFieldIDs
            End If

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                Dim value As Double = CDbl(_totalRecords) / CDbl(_pageRecords)
                Dim count As Integer = Fix(value - (value \ 1 <> value))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)
                        Case "QUADRA-APERTA"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLiteral.Text = "["
                            objPlaceHolder.Add(objLiteral)
                        Case "QUADRA-CHIUSA"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLiteral.Text = "]"
                            objPlaceHolder.Add(objLiteral)
                        Case "CURRENTPAGE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLiteral.Text = (_currentPage).ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "HASMULTIPLEPAGES"
                            If (_totalRecords <= _pageRecords) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASMULTIPLEPAGES") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASMULTIPLEPAGES"
                            ' Do Nothing

                        Case "HASPAGING"
                            If (_totalRecords <= _pageRecords) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASPAGING") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASPAGING"
                            ' Do Nothing

                        Case "HASNEXTPAGE"
                            If (_currentPage = count) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASNEXTPAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASNEXTPAGE"
                            ' Do Nothing

                        Case "HASPREVPAGE"
                            If (_currentPage = 1) Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASPREVPAGE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASPREVPAGE"
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
                            ' Do Nothing

                        Case "HASTYPE"
                            If (_agentType.ToLower() <> "viewtype") Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASTYPE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/HASTYPE"
                            ' Do Nothing

                        Case "LINKFIRST"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLink.CssClass = "CommandButton"
                            objLink.Enabled = Not (_currentPage = 1)
                            objLink.NavigateUrl = GetListingUrl(1)
                            objLink.Text = Localization.GetString("First", Me.LocalResourceFile)
                            objPlaceHolder.Add(objLink)

                        Case "LINKLAST"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLink.CssClass = "CommandButton"
                            objLink.Enabled = Not (_currentPage = count)
                            objLink.NavigateUrl = GetListingUrl(count)
                            objLink.Text = Localization.GetString("Last", Me.LocalResourceFile)
                            objPlaceHolder.Add(objLink)

                        Case "LINKNEXT"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLink.CssClass = "CommandButton"
                            objLink.Enabled = Not (_currentPage = count)
                            objLink.NavigateUrl = GetListingUrl(_currentPage + 1)
                            objLink.Text = Localization.GetString("Next", Me.LocalResourceFile)
                            objPlaceHolder.Add(objLink)

                        Case "LINKPREVIOUS"
                            Dim objLink As New HyperLink
                            objLink.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLink.CssClass = "CommandButton"
                            objLink.Enabled = Not (_currentPage = 1)
                            objLink.NavigateUrl = GetListingUrl(_currentPage - 1)
                            objLink.Text = Localization.GetString("Previous", Me.LocalResourceFile)
                            objPlaceHolder.Add(objLink)

                        Case "LINKXML"
                            Dim objLiteral As New Literal
                            objLiteral.Text = GetXmlUrl()
                            objPlaceHolder.Add(objLiteral)

                        Case "MARKERS"
                            Dim objLiteral As New Literal
                            objLiteral.Text = "var paItems = [" & vbCrLf

                            Dim bAdded As Boolean = False
                            Dim i As Integer = 1
                            For Each objProperty As PropertyInfo In _objProperties
                                If (objProperty.Latitude <> Null.NullDouble And objProperty.Longitude <> Null.NullDouble) Then

                                    Dim objCustomFieldController As New CustomFieldController()
                                    Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(ModuleId, True)
                                    Dim fields As String = ""
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (fields = "") Then
                                            fields = ",'" & objProperty.PropertyList(objCustomField.CustomFieldID).ToString() & "'"
                                        Else
                                            fields = fields & ",'" & objProperty.PropertyList(objCustomField.CustomFieldID).ToString() & "'"
                                        End If
                                    Next
                                    objLiteral.Text = objLiteral.Text & "[" & objProperty.Latitude.ToString() & "," & objProperty.Longitude.ToString() & "," & i.ToString() & fields & "]," & vbCrLf
                                    bAdded = True
                                    i = i + 1
                                End If
                            Next
                            objLiteral.Text = objLiteral.Text & vbCrLf _
                                & "];"

                            If (bAdded) Then
                                objPlaceHolder.Add(objLiteral)
                            End If

                        Case "PAGECOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLiteral.Text = count.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "PAGER"
                            If (count > 1) Then
                                Dim ctlPagingControl As New Ventrian.PropertyAgent.PagingControl
                                ctlPagingControl.Visible = True
                                ctlPagingControl.TotalRecords = _totalRecords
                                ctlPagingControl.PageSize = _pageRecords
                                ctlPagingControl.CurrentPage = _currentPage
                                ctlPagingControl.PageParam = "currentpage"

                                Dim params() As String = GetAdditionalParams(_agentType, _currentPage, "")

                                For Each param As String In params
                                    If (param.ToLower().StartsWith("currentpage") = False) Then
                                        If (ctlPagingControl.QuerystringParams <> "") Then
                                            ctlPagingControl.QuerystringParams += "&" & param
                                        Else
                                            ctlPagingControl.QuerystringParams = param
                                        End If
                                    End If
                                Next
                                ctlPagingControl.TabID = TabId
                                ctlPagingControl.EnableViewState = False
                                ctlPagingControl.Title = "Default.aspx"
                                If (Request(PropertySettings.SEOAgentType) <> "") Then
                                    If (Request(PropertySettings.SEOAgentType).ToLower() = "viewtype") Then
                                        If (PropertySettings.SEOViewTypeTitle <> "") Then
                                            If DotNetNuke.Entities.Host.HostSettings.GetHostSetting("UseFriendlyUrls") = "Y" Then
                                                If (PropertySettings.SEOViewTypeTitle <> "") Then

                                                    Dim objTypeController As New PropertyTypeController()
                                                    Dim objType As PropertyTypeInfo = objTypeController.Get(Me.ModuleId, _propertyTypeID)

                                                    If (objType IsNot Nothing) Then
                                                        Dim delimStr As String = "[]"
                                                        Dim delimiter As Char() = delimStr.ToCharArray()
                                                        Dim phPageTitle As New PlaceHolder()
                                                        _layoutController.ProcessType(phPageTitle.Controls, PropertySettings.SEOViewTypeTitle.Split(delimiter), objType, Null.NullString)
                                                        Dim title As String = OnlyAlphaNumericChars(RenderControlToString(phPageTitle))
                                                        If (title Is Nothing OrElse title.Trim() = "") Then
                                                            ctlPagingControl.Title = "Default.aspx"
                                                        Else
                                                            ctlPagingControl.Title = title.Replace("--", "-") & ".aspx"
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                objPlaceHolder.Add(ctlPagingControl)
                            End If

                        Case "PAGES"
                            For i As Integer = 1 To count
                                Dim objLink As New HyperLink
                                objLink.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString() & "-" & i.ToString())
                                objLink.CssClass = "CommandButton"
                                If (i <> _currentPage) Then
                                    objLink.Text = i.ToString()
                                    objLink.Enabled = True
                                    objLink.NavigateUrl = GetListingUrl(i)
                                Else
                                    objLink.Text = "[" & i.ToString() & "]"
                                    objLink.Enabled = False
                                End If
                                objPlaceHolder.Add(objLink)

                                If (i < count) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString() & "-spacer-" & i.ToString())
                                    objLiteral.Text = "&nbsp;&nbsp;"
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            Next

                        Case "TOTALRECORDS"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                            objLiteral.Text = _totalRecords.ToString()
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEDESCRIPTION"
                            If (_agentType.ToLower() = "viewtype") Then
                                Dim objTypeController As New PropertyTypeController()
                                Dim objType As PropertyTypeInfo = objTypeController.Get(Me.ModuleId, _propertyTypeID)
                                If (objType IsNot Nothing) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                                    objLiteral.Text = System.Web.HttpUtility.HtmlEncode(objType.Description.ToString())
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If
                        Case "TYPE"
                            If (_agentType.ToLower() = "viewtype") Then
                                Dim objTypeController As New PropertyTypeController()
                                Dim objType As PropertyTypeInfo = objTypeController.Get(Me.ModuleId, _propertyTypeID)
                                If (objType IsNot Nothing) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                                    objLiteral.Text = System.Web.HttpUtility.HtmlEncode(objType.Name.ToString())
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If
                        Case "FULLTYPES"
                            If (_agentType.ToLower() = "viewtype") Then
                                Dim objTypeController As New PropertyTypeController()
                                Dim objType As PropertyTypeInfo = objTypeController.Get(Me.ModuleId, _propertyTypeID)
                                If (objType IsNot Nothing) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                                    objLiteral.Text = System.Web.HttpUtility.HtmlEncode(objType.Name.ToString())
                                    While objType.ParentID <> -1
                                        objType = objTypeController.Get(ModuleId, objType.ParentID)
                                        objLiteral.Text = objType.Name & " - " & objLiteral.Text
                                    End While
                                    objPlaceHolder.Add(objLiteral)
                                End If
                            End If
                        Case "TYPEIMAGELINK"
                            If (_agentType.ToLower() = "viewtype") Then
                                Dim objTypeController As New PropertyTypeController()
                                Dim objType As PropertyTypeInfo = objTypeController.Get(Me.ModuleId, _propertyTypeID)
                                If (objType IsNot Nothing) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("PropertyAgent-Header" & "-" & iPtr.ToString())
                                    If objType.ImageFile.ToString() <> "" Then
                                        objLiteral.Text = PortalSettings.HomeDirectory & System.Web.HttpUtility.HtmlEncode(objType.ImageFile.ToString())
                                        objPlaceHolder.Add(objLiteral)
                                    End If                                   
                                End If
                            End If

                        Case Else

                            Dim objLiteralOther As New Literal
                            objLiteralOther.Text = "[" & layoutArray(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            objPlaceHolder.Add(objLiteralOther)

                    End Select
                End If

            Next

        End Sub

        Private Function GetAdditionalParams(ByVal agentType As String, ByVal currentPage As Integer, ByVal title As String) As String()

            Dim params As New ArrayList

            If (agentType <> "") Then
                params.Add(PropertySettings.SEOAgentType & "=" & agentType)
            End If

            If (_propertyTypeID <> Null.NullInteger) Then
                params.Add(PropertySettings.SEOPropertyTypeID & "=" & _propertyTypeID.ToString())
            End If

            If (_propertyAgentID <> Null.NullInteger) Then
                params.Add("PropertyAgentID=" & _propertyAgentID.ToString())
            End If

            If (_propertyAgentName <> Null.NullString) Then
                params.Add("PropertyAgentName=" & _propertyAgentName.ToString())
            End If

            If (_propertyBrokerID <> Null.NullInteger) Then
                params.Add("PropertyBrokerID=" & _propertyBrokerID.ToString())
            End If

            If (_location <> Null.NullString) Then
                params.Add("Location=" & Server.UrlEncode(_location))
            End If

            If (currentPage <> 1) Then
                params.Add("CurrentPage=" & currentPage.ToString())
            End If

            If (_searchValues <> "") Then
                params.Add("SearchValues=" & _searchValues)
            End If

            If (_customFieldIDs <> "") Then
                params.Add("CustomFieldIDs=" & _customFieldIDs)
            End If

            If (Me.PropertySettings.ListingUserSortable) Then
                If (SortBy <> Me.PropertySettings.ListingSortBy) Then
                    params.Add("sortBy=" & drpSortBy.SelectedValue)
                Else
                    If (SortBy = SortByType.CustomField) Then
                        params.Add("sortBy=" & drpSortBy.SelectedValue)
                    End If
                End If
            Else
                If (Me.PropertySettings.ListingSortBy = SortByType.CustomField) Then
                    params.Add("sortBy=cf" & Me.PropertySettings.ListingSortByCustomField.ToString())
                Else
                    If (Me.PropertySettings.ListingSortBy = SortByType.ReviewField) Then
                        params.Add("sortBy=rf" & Me.PropertySettings.ListingSortByCustomField.ToString())
                    Else
                        params.Add("sortBy=" & Me.PropertySettings.ListingSortBy.ToString())
                    End If
                End If
            End If

            If (SortDirection <> Me.PropertySettings.ListingSortDirection) Then
                params.Add("sortDir=" & drpSortDirection.SelectedValue)
            End If

            If (title <> "") Then
                params.Add("title=" & Server.UrlEncode(title))
            End If

            Return CType(params.ToArray(GetType(String)), String())

        End Function

        Private Function GetListingUrl(ByVal currentPage As Integer) As String

            Return NavigateURL(Me.TabId, "", GetAdditionalParams(_agentType, currentPage, ""))

        End Function

        Private Function GetXmlUrl() As String

            Return NavigateURL(Me.TabId, "", GetAdditionalParams("xml", 1, ""))

        End Function

        Private Function RenderControlToString(ByVal ctrl As Control) As String

            Dim sb As New StringBuilder()
            Dim tw As New IO.StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            ctrl.RenderControl(hw)

            Return sb.ToString()

        End Function

        Public Sub ProcessPropertyType(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String(), ByVal objType As PropertyTypeInfo, ByVal key As String)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "TYPE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(Me.ModuleKey & key & "-" & iPtr.ToString())
                            objLiteral.Text = System.Web.HttpUtility.HtmlEncode(objType.Name.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(Me.ModuleKey & key & "-" & iPtr.ToString())
                            objLiteral.Text = System.Web.HttpUtility.HtmlEncode(objType.PropertyTypeID.ToString())
                            objPlaceHolder.Add(objLiteral)

                        Case "TYPEDESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID(Me.ModuleKey & key & "-" & iPtr.ToString())
                            objLiteral.Text = System.Web.HttpUtility.HtmlEncode(objType.Description.ToString())
                            objPlaceHolder.Add(objLiteral)

                    End Select
                End If

            Next

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                InitializeTemplate()
                ReadQueryString()
                BindSortBy()
                BindSortDirection()
                BindProperty()

                If (PropertySettings.PermissionExport <> "") Then
                    divExport.Visible = PortalSecurity.IsInRoles(PropertySettings.PermissionExport)
                Else
                    divExport.Visible = False
                End If
                divSort.Visible = Me.PropertySettings.ListingUserSortable
                If (PropertySettings.ListingSortFields = "") Then
                    divSort.Visible = False
                End If

                lnkExport.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                InitializeStylesheet()

                If _agentType.ToLower() = "viewtype" Then
                    Dim objTypeController As New PropertyTypeController()
                    Dim objType As PropertyTypeInfo = objTypeController.Get(ModuleId, _propertyTypeID)

                    If (objType IsNot Nothing) Then
                        If (_objLayoutTypeTitle.Template <> "") Then
                            Dim phPageTitle As New PlaceHolder()
                            ProcessPropertyType(phPageTitle.Controls, _objLayoutTypeTitle.Tokens, objType, "Title")
                            Me.BasePage.Title = RenderControlToString(phPageTitle)
                        End If
                        If (_objLayoutTypeDescription.Template <> "") Then
                            Dim phPageDescription As New PlaceHolder()
                            ProcessPropertyType(phPageDescription.Controls, _objLayoutTypeDescription.Tokens, objType, "Description")
                            Me.BasePage.Description = RenderControlToString(phPageDescription)
                        End If
                        If (_objLayoutTypeKeyword.Template <> "") Then
                            Dim phPageKeyword As New PlaceHolder()
                            ProcessPropertyType(phPageKeyword.Controls, _objLayoutTypeKeyword.Tokens, objType, "Keyword")
                            Me.BasePage.KeyWords = RenderControlToString(phPageKeyword)
                        End If
                        If (_objLayoutTypePageHead.Template <> "") Then
                            Dim phPageHead As New PlaceHolder()
                            ProcessPropertyType(phPageHead.Controls, _objLayoutTypePageHead.Tokens, objType, "PageHead")
                            Me.BasePage.Header.Controls.Add(phPageHead)
                        End If
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlProperty_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlProperty.ItemDataBound

            If (e.Item.ItemType = ListItemType.Header) Then
                ProcessHeaderFooter(e.Item.Controls, _objLayoutHeader.Tokens)
            End If

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objProperty As PropertyInfo = CType(e.Item.DataItem, PropertyInfo)

                Dim objPhotoController As New PhotoController
                Dim objPhoto As PhotoInfo = objPhotoController.Get(objProperty.FirstPhotoID)

                Dim currentLayout As LayoutInfo = _objLayout

                If (e.Item.ItemType = ListItemType.AlternatingItem) Then
                    currentLayout = _objLayoutAlternating
                End If

                'If an Agent (User/Role) has Submit privileges, they should see an Edit pencil icon 
                'next to their properties in both the Search List and the Latest Properties List
                Dim isMySubmittedProperty As Boolean = False
                If (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True Or PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True) Then
                    If Me.UserId = objProperty.AuthorID Then
                        isMySubmittedProperty = True
                    End If
                End If

                ' Check for broker access
                If (Not objProperty Is Nothing AndAlso Request.IsAuthenticated AndAlso isMySubmittedProperty = False AndAlso Me.IsEditable = False) Then
                    If PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True Then
                        If (Me.UserId = objProperty.BrokerID) Then
                            isMySubmittedProperty = True
                        End If
                    End If
                End If

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, (Me.IsEditable Or isMySubmittedProperty), Me.TabId, Me.ModuleId, Me.ModuleKey)
                objLayoutController.ListingIndex = e.Item.ItemIndex
                objLayoutController.ProcessItem(e.Item.Controls, currentLayout.Tokens, objProperty, Me.CustomFields)

            End If

            If (e.Item.ItemType = ListItemType.Separator) Then
                For iPtr As Integer = 0 To _objLayoutSeparator.Tokens.Length - 1 Step 2
                    e.Item.Controls.Add(New LiteralControl(_objLayoutSeparator.Tokens(iPtr).ToString()))
                Next
            End If

            If (e.Item.ItemType = ListItemType.Footer) Then
                ProcessHeaderFooter(e.Item.Controls, _objLayoutFooter.Tokens)
            End If

        End Sub

        Private Sub rptProperty_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptProperty.ItemDataBound

            If (e.Item.ItemType = ListItemType.Header) Then
                ProcessHeaderFooter(e.Item.Controls, _objLayoutHeader.Tokens)
            End If

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objProperty As PropertyInfo = CType(e.Item.DataItem, PropertyInfo)

                Dim objPhotoController As New PhotoController
                Dim objPhoto As PhotoInfo = objPhotoController.Get(objProperty.FirstPhotoID)

                Dim currentLayout As LayoutInfo = _objLayout

                If (e.Item.ItemType = ListItemType.AlternatingItem) Then
                    currentLayout = _objLayoutAlternating
                End If

                'If an Agent (User/Role) has Submit privileges, they should see an Edit pencil icon 
                'next to their properties in both the Search List and the Latest Properties List
                Dim isMySubmittedProperty As Boolean = False
                If (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True Or PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True) Then
                    If Me.UserId = objProperty.AuthorID Then
                        isMySubmittedProperty = True
                    End If
                End If

                ' Check for broker access
                If (Not objProperty Is Nothing AndAlso Request.IsAuthenticated AndAlso isMySubmittedProperty = False AndAlso Me.IsEditable = False) Then
                    If PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True Then
                        If (Me.UserId = objProperty.BrokerID) Then
                            isMySubmittedProperty = True
                        End If
                    End If
                End If

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, (Me.IsEditable Or isMySubmittedProperty), Me.TabId, Me.ModuleId, Me.ModuleKey)
                objLayoutController.ListingIndex = e.Item.ItemIndex
                objLayoutController.ProcessItem(e.Item.Controls, currentLayout.Tokens, objProperty, Me.CustomFields)

            End If

            If (e.Item.ItemType = ListItemType.Separator) Then
                For iPtr As Integer = 0 To _objLayoutSeparator.Tokens.Length - 1 Step 2
                    e.Item.Controls.Add(New LiteralControl(_objLayoutSeparator.Tokens(iPtr).ToString()))
                Next
            End If

            If (e.Item.ItemType = ListItemType.Footer) Then
                ProcessHeaderFooter(e.Item.Controls, _objLayoutFooter.Tokens)
            End If

        End Sub

        Private Sub drpSortBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpSortBy.SelectedIndexChanged

            Try

                Response.Redirect(GetListingUrl(1), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpSortDirection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpSortDirection.SelectedIndexChanged

            Try

                Response.Redirect(GetListingUrl(1), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlTypes_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objType As PropertyTypeInfo = CType(e.Item.DataItem, PropertyTypeInfo)
                _layoutController.ProcessType(e.Item.Controls, Me._objLayoutType.Tokens, objType, Null.NullString)
            End If

        End Sub

        Private Sub rptTypes_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.Header Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objType As PropertyTypeInfo = CType(e.Item.DataItem, PropertyTypeInfo)
                _layoutController.ProcessType(e.Item.Controls, Me._objLayoutType.Tokens, objType, Null.NullString)
            End If

        End Sub

#End Region

    End Class

End Namespace
