Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security
Imports Ventrian.PropertyAgent.Mapping

Namespace Ventrian.PropertyAgent

    Partial Public Class Latest
        Inherits PropertyAgentLatestBase

#Region " Private Members "

        Private _objLayoutLatest As LayoutInfo
        Private _objLayoutLatestHeader As LayoutInfo
        Private _objLayoutLatestFooter As LayoutInfo
        Private _objLayoutLatestEmpty As LayoutInfo

        Private _propertyID As Integer = Null.NullInteger
        Private _propertyTypeID As Integer = Null.NullInteger

        Private _pageNumber As Integer = 1

        Private _searchValues As String = Null.NullString
        Private _agentType As String = Null.NullString
        Private _agentFilter As String = Null.NullString
        Private _propertyAgentID As Integer = Null.NullInteger
        Private _propertyAgentName As String = Null.NullInteger
        Private _propertyBrokerID As Integer = Null.NullInteger
        Private _location As String = Null.NullString
        Private _currentPage As Integer = 1
        Private _pageRecords As Integer = 10
        Private _customFieldIDs As String = Null.NullString
        Private _sortBy As String = ""
        Private _sortDirection As String = ""
        Private _totalRecords As Integer = 0

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
                    Return Me.PropertySettingsLatest.SortBy
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
                    Return Me.PropertySettingsLatest.SortByCustomField
                End Try
            End Get
        End Property

        Private ReadOnly Property SortDirection() As SortDirectionType
            Get
                Try
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), drpSortDirection.SelectedValue), SortDirectionType)
                Catch
                    Return Me.PropertySettingsLatest.SortByCustomField
                End Try
            End Get
        End Property

#End Region

#Region " Private Methods "

        Public Function CheckLimit() As Boolean

            Dim limit As Integer = GetLimit(PropertySettings.PermissionSubmit, PropertySettings.PermissionLimit)

            If (limit = Null.NullInteger) Then
                Return True
            End If

            Dim objPropertyController As New PropertyController
            Dim count As Integer = objPropertyController.Count(Me.ModuleId, Me.UserId)

            If (count >= limit) Then
                Return False
            Else
                Return True
            End If

        End Function

        Public Function GetLimit(ByVal permission As String, ByVal permissionLimit As String) As Integer

            Dim limit As Integer = Null.NullInteger

            For Each role As String In permission.Split(";"c)

                If (role <> "") Then

                    Dim actualRole As String = role

                    If (actualRole.Split(":"c).Length > 1) Then
                        actualRole = actualRole.Split(":"c)(0)
                    End If

                    If (PortalSecurity.IsInRole(actualRole)) Then

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

                End If

            Next

            Return limit

        End Function

        Private Sub InitializeTemplate()

            Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.PropertySettingsLatest.PropertyAgentTabID, Me.PropertySettingsLatest.PropertyAgentModuleID, Me.ModuleKey)

            If (Me.PropertySettingsLatest.LayoutMode = LatestLayoutMode.TemplateLayout) Then

                _objLayoutLatest = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Latest_Item_Html)
                _objLayoutLatestHeader = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Latest_Header_Html)
                _objLayoutLatestFooter = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Latest_Footer_Html)

            Else

                Dim delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                _objLayoutLatest = New LayoutInfo
                _objLayoutLatest.Template = Me.PropertySettingsLatest.LayoutItem
                _objLayoutLatest.Tokens = _objLayoutLatest.Template.Split(delimiter)

                _objLayoutLatestHeader = New LayoutInfo
                _objLayoutLatestHeader.Template = Me.PropertySettingsLatest.LayoutHeader
                _objLayoutLatestHeader.Tokens = _objLayoutLatestHeader.Template.Split(delimiter)

                _objLayoutLatestFooter = New LayoutInfo
                _objLayoutLatestFooter.Template = Me.PropertySettingsLatest.LayoutFooter
                _objLayoutLatestFooter.Tokens = _objLayoutLatestFooter.Template.Split(delimiter)

                _objLayoutLatestEmpty = New LayoutInfo
                _objLayoutLatestEmpty.Template = Me.PropertySettingsLatest.LayoutEmpty
                _objLayoutLatestEmpty.Tokens = _objLayoutLatestEmpty.Template.Split(delimiter)

            End If

            If (Me.PropertySettingsLatest.IncludeStylesheet) Then
                objLayoutController.LoadStyleSheet(Me.PropertySettings.Template, False)
            End If

        End Sub

        Protected Function GetPropertyID() As String

            Dim propertyID As Integer = Null.NullInteger
            Dim propertyIDParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyIDParam) = "") Then
                propertyIDParam = "PropertyID"
            End If
            If Not (Request(propertyIDParam) Is Nothing) Then
                propertyID = Convert.ToInt32(Request(propertyIDParam))
            End If

            Return propertyID.ToString()

        End Function

        Private Sub BindLatest()

            Dim customFieldFilters As String = PropertySettingsLatest.CustomFieldFilters
            Dim customFieldValues As String = PropertySettingsLatest.CustomFieldValues

            Dim objPropertyController As New PropertyController
            If (PropertySettingsLatest.ShowRelated) Then
                If (_propertyID <> Null.NullInteger) Then
                    Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)
                    If Not (objProperty Is Nothing) Then
                        If (Me.PropertySettingsLatest.RelatedCustomField <> Null.NullInteger) Then
                            If (objProperty.PropertyList.Contains(Me.PropertySettingsLatest.RelatedCustomField)) Then

                                customFieldFilters = Me.PropertySettingsLatest.RelatedCustomField.ToString()
                                customFieldValues = objProperty.PropertyList(Me.PropertySettingsLatest.RelatedCustomField).ToString()

                                If (objProperty.PropertyList(Me.PropertySettingsLatest.RelatedCustomField).ToString().Contains("|"c)) Then
                                    For Each v As String In objProperty.PropertyList(Me.PropertySettingsLatest.RelatedCustomField).ToString().Split("|"c)
                                        If (Request.Url.AbsolutePath.Contains(v)) Then
                                            customFieldValues = v
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        Else
                            _propertyTypeID = objProperty.PropertyTypeID
                        End If
                    End If
                End If
            Else
                _propertyTypeID = Null.NullInteger
            End If





            If (PropertySettingsLatest.TypeID <> Null.NullInteger) Then
                _propertyTypeID = PropertySettingsLatest.TypeID
            End If

            Dim startDate As DateTime = Null.NullDate

            If (Me.PropertySettingsLatest.MaxAge <> Null.NullInteger) Then
                startDate = DateTime.Today.AddDays(Me.PropertySettingsLatest.MaxAge * -1)
            Else
                If (Me.PropertySettingsLatest.StartDate <> Null.NullDate) Then
                    startDate = Me.PropertySettingsLatest.StartDate
                End If
            End If

            Dim agentID As Integer = Null.NullInteger

            Select Case Me.PropertySettingsLatest.UserFilter

                Case UserFilterType.Parameter
                    If (IsNumeric(Request(Me.PropertySettingsLatest.UserFilterParameter))) Then
                        agentID = Convert.ToInt32(Request(Me.PropertySettingsLatest.UserFilterParameter))
                    End If
                    Exit Select

                Case UserFilterType.Current
                    agentID = Me.UserId
                    Exit Select

                Case UserFilterType.Specific
                    agentID = Me.PropertySettingsLatest.UserFilterSpecific
                    Exit Select

            End Select

            Dim maxCount As Integer = Me.PropertySettingsLatest.MaxNumber
            If (Me.PropertySettingsLatest.EnablePager And Me.PropertySettingsLatest.PageSize > 0) Then
                If (Me.PropertySettingsLatest.PageSize < maxCount) Then
                    maxCount = Me.PropertySettingsLatest.PageSize
                End If
            End If

            Dim shortListID As String = Null.NullInteger.ToString()
            If (Me.PropertySettingsLatest.ShowShortList) Then
                If (agentID <> Null.NullInteger) Then
                    shortListID = agentID
                    agentID = Null.NullInteger
                Else
                    If (Request.IsAuthenticated) Then
                        shortListID = Me.UserInfo.UserID
                    Else
                        If (Request.Cookies("ShortList-PA-" & Me.PropertySettingsLatest.PropertyAgentModuleID.ToString()) IsNot Nothing) Then
                            shortListID = Request.Cookies("ShortList-PA-" & Me.PropertySettingsLatest.PropertyAgentModuleID.ToString()).Value
                        Else
                            shortListID = "-2"
                        End If
                    End If
                End If
            End If

            Dim latitude As Double = Null.NullDouble
            Dim longitude As Double = Null.NullDouble

            If (Me.SortBy = SortByType.Distance) Then
                Dim objGeoCodeInfo As GeocodeInfo = UserGeoLocator.GetUserLocation(Request.UserHostAddress)
                If (objGeoCodeInfo IsNot Nothing) Then
                    latitude = objGeoCodeInfo.Latitude
                    longitude = objGeoCodeInfo.Longitude
                End If
            End If





            Dim totalRecords As Integer = 0
            Dim OnlyForAuthenticated As Boolean = Null.NullBoolean
            Dim objProperties As New List(Of PropertyInfo)
            If Me.UserId = -1 Then OnlyForAuthenticated = True



            If (Me.PropertySettingsLatest.PropertyIDinURL) Then
                Dim PropID As Integer = GetPropertyID()
                If PropID <> -1 Then
                    objProperties.Add(objPropertyController.Get(PropID))
                End If
            ElseIf (Me.PropertySettingsLatest.SearchValuesInURL) Then
                ReadQueryStringTotal()
                objProperties = objPropertyController.List(Me.PropertySettingsLatest.PropertyAgentModuleID, _propertyTypeID, SearchStatusType.PublishedActive, _propertyAgentID, _propertyBrokerID, Me.PropertySettingsLatest.FeaturedOnly, OnlyForAuthenticated, Me.SortBy, Me.SortByCustomField, Me.SortDirection, Null.NullInteger, Null.NullInteger, SortDirectionType.Ascending, Null.NullInteger, Null.NullInteger, SortDirectionType.Ascending, _customFieldIDs, _searchValues, _currentPage - 1, _pageRecords, _totalRecords, Me.PropertySettingsLatest.Bubblefeatured, True, Null.NullInteger, Null.NullInteger, latitude, longitude, startDate, Null.NullString, shortListID)

            Else
                objProperties = objPropertyController.List(Me.PropertySettingsLatest.PropertyAgentModuleID, _propertyTypeID, SearchStatusType.PublishedActive, agentID, Null.NullInteger, Me.PropertySettingsLatest.FeaturedOnly, OnlyForAuthenticated, Me.SortBy, Me.SortByCustomField, Me.SortDirection, Null.NullInteger, Null.NullInteger, SortDirectionType.Ascending, Null.NullInteger, Null.NullInteger, SortDirectionType.Ascending, customFieldFilters, customFieldValues, _pageNumber - 1, maxCount, totalRecords, Me.PropertySettingsLatest.Bubblefeatured, True, Null.NullInteger, Null.NullInteger, latitude, longitude, startDate, Null.NullString, shortListID)

                If (PropertySettingsLatest.ShowRelated) Then
                    Dim match As Boolean = False
                    For Each objProperty As PropertyInfo In objProperties
                        If (objProperty.PropertyID = _propertyID) Then
                            match = True
                        End If
                    Next
                    If (match) Then
                        objProperties = objPropertyController.List(Me.PropertySettingsLatest.PropertyAgentModuleID, _propertyTypeID, SearchStatusType.PublishedActive, agentID, Null.NullInteger, Me.PropertySettingsLatest.FeaturedOnly, OnlyForAuthenticated, Me.SortBy, Me.SortByCustomField, Me.SortDirection, Null.NullInteger, Null.NullInteger, SortDirectionType.Ascending, Null.NullInteger, Null.NullInteger, SortDirectionType.Ascending, customFieldFilters, customFieldValues, _pageNumber - 1, maxCount + 1, totalRecords, Me.PropertySettingsLatest.Bubblefeatured, True, Null.NullInteger, Null.NullInteger, latitude, longitude, startDate, Null.NullString, shortListID)
                        Dim objPropertiesUpdated As New List(Of PropertyInfo)
                        For Each objProperty As PropertyInfo In objProperties
                            If (objProperty.PropertyID <> _propertyID) Then
                                objPropertiesUpdated.Add(objProperty)
                            End If
                        Next
                        objProperties = objPropertiesUpdated
                    End If
                End If

                If (objProperties.Count = 0) Then
                    If (Me.PropertySettingsLatest.LayoutMode = LatestLayoutMode.CustomLayout) Then
                        ProcessEmpty(phProperty.Controls, _objLayoutLatestEmpty.Tokens)
                    End If
                    Return
                End If
            End If



            ProcessHeaderFooter(phProperty.Controls, _objLayoutLatestHeader.Tokens)

            If (PropertySettingsLatest.LayoutType = LatestLayoutType.TableLayout) Then
                Dim objDataList As New System.Web.UI.WebControls.DataList
                Dim objHandler As New DataListItemEventHandler(AddressOf dlLatest_ItemDataBound)
                AddHandler objDataList.ItemDataBound, objHandler

                objDataList.CellPadding = 0
                objDataList.CellSpacing = 0

                objDataList.RepeatColumns = Me.PropertySettingsLatest.ItemsPerRow
                objDataList.RepeatDirection = RepeatDirection.Horizontal

                objDataList.ItemStyle.HorizontalAlign = HorizontalAlign.Center

                objDataList.DataSource = objProperties
                objDataList.DataBind()

                phProperty.Controls.Add(objDataList)
            Else
                Dim objRepeater As New System.Web.UI.WebControls.Repeater
                Dim objHandler As New RepeaterItemEventHandler(AddressOf rptLatest_ItemDataBound)
                AddHandler objRepeater.ItemDataBound, objHandler

                objRepeater.DataSource = objProperties
                objRepeater.DataBind()

                phProperty.Controls.Add(objRepeater)
            End If

            ProcessHeaderFooter(phProperty.Controls, _objLayoutLatestFooter.Tokens)

            If (totalRecords > Me.PropertySettingsLatest.MaxNumber) Then
                totalRecords = Me.PropertySettingsLatest.MaxNumber
            End If

            If (Me.PropertySettingsLatest.EnablePager And Me.PropertySettingsLatest.PageSize > 0 And totalRecords > 0) Then
                Dim pageCount As Integer = ((totalRecords - 1) \ Me.PropertySettingsLatest.PageSize) + 1

                If (pageCount > 1) Then
                    ctlPagingControl1.Visible = True
                    ctlPagingControl1.PageParam = "papg-" & Me.TabModuleId.ToString()
                    ctlPagingControl1.TotalRecords = totalRecords
                    ctlPagingControl1.PageSize = Me.PropertySettingsLatest.PageSize
                    ctlPagingControl1.CurrentPage = _pageNumber
                    ctlPagingControl1.TabID = TabId
                    ctlPagingControl1.EnableViewState = False
                    ctlPagingControl1.Title = "Default.aspx"
                Else
                    ctlPagingControl1.Visible = False
                End If
            Else
                ctlPagingControl1.Visible = False
            End If

        End Sub

        Private Sub ReadQueryStringTotal()

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

        Private Sub ProcessEmpty(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "ADDPROPERTY"
                            Dim objLink As New HyperLink
                            objLink.Text = PropertyUtil.FormatPropertyLabel(Localization.GetString("AddProperty", Me.LocalResourceFile), PropertySettings)
                            objLink.Visible = PropertySettings.Template <> "" AndAlso Page.Request.IsAuthenticated AndAlso (IsEditable OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) AndAlso CheckLimit()
                            objLink.CssClass = PropertySettings.ButtonClass
                            objLink.EnableViewState = False
                            objLink.NavigateUrl = NavigateURL(Me.PropertySettingsLatest.PropertyAgentTabID, "", PropertySettings.SEOAgentType & "=EditProperty")
                            objPlaceHolder.Add(objLink)

                        Case "ADDPROPERTYLINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = NavigateURL(Me.PropertySettingsLatest.PropertyAgentTabID, "", PropertySettings.SEOAgentType & "=EditProperty")
                            objLiteral.Visible = PropertySettings.Template <> "" AndAlso Request.IsAuthenticated AndAlso (IsEditable OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) AndAlso CheckLimit()
                            objLiteral.EnableViewState = False
                            objPlaceHolder.Add(objLiteral)

                        Case "HASADDPROPERTY"
                            If (PropertySettings.Template <> "" AndAlso Request.IsAuthenticated AndAlso (IsEditable OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) AndAlso CheckLimit()) = False Then
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
                            If (PropertySettings.Template <> "" AndAlso Request.IsAuthenticated AndAlso (IsEditable OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) AndAlso CheckLimit()) = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/HASPROPERTYMANAGER") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/HASPROPERTYMANAGER"
                            ' Do Nothing

                        Case "PROPERTYMANAGER"
                            Dim objLink As New HyperLink
                            objLink.Text = PropertyUtil.FormatPropertyLabel(Localization.GetString("AddProperty", Me.LocalResourceFile), PropertySettings)
                            objLink.Visible = PropertySettings.Template <> "" AndAlso Request.IsAuthenticated AndAlso (IsEditable OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) AndAlso CheckLimit()
                            objLink.CssClass = PropertySettings.ButtonClass
                            objLink.EnableViewState = False
                            objLink.NavigateUrl = NavigateURL(Me.PropertySettingsLatest.PropertyAgentTabID, "", PropertySettings.SEOAgentType & "=PropertyManager")
                            objPlaceHolder.Add(objLink)

                        Case "PROPERTYMANAGERLINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = NavigateURL(Me.PropertySettingsLatest.PropertyAgentTabID, "", PropertySettings.SEOAgentType & "=PropertyManager")
                            objLiteral.Visible = PropertySettings.Template <> "" AndAlso Request.IsAuthenticated AndAlso (IsEditable OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) AndAlso CheckLimit()
                            objLiteral.EnableViewState = False
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

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)
                        Case "QUADRA-APERTA"
                            Dim objLiteral As New Literal
                            objLiteral.Text = "["
                            objPlaceHolder.Add(objLiteral)
                        Case "QUADRA-CHIUSA"
                            Dim objLiteral As New Literal
                            objLiteral.Text = "]"
                            objPlaceHolder.Add(objLiteral)
                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = PropertySettings.PropertyLabel
                            objPlaceHolder.Add(objLiteral)

                    End Select
                End If
            Next

        End Sub

        Private Sub ReadQueryString()

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

            Dim propertyParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyParam) = "") Then
                propertyParam = "PropertyID"
            End If
            If Not (Request(propertyParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyParam))
            End If

            If (Request.QueryString("papg-" & Me.TabModuleId.ToString()) <> "") Then
                _pageNumber = Convert.ToInt32(Request.QueryString("papg-" & Me.TabModuleId.ToString()))
            End If

        End Sub

        Private Sub RegisterScripts()

            DotNetNuke.Framework.jQuery.RequestRegistration()

            'If (PropertySettings.IncludejQuery And HttpContext.Current.Items("jquery_registered") Is Nothing And HttpContext.Current.Items("jQueryRequested") Is Nothing) Then
            '    If (HttpContext.Current.Items("PropertyAgent-jQuery-ScriptsRegistered") Is Nothing And HttpContext.Current.Items("SimpleGallery-ScriptsRegistered") Is Nothing) Then
            '        Dim litLink As New Literal
            '        litLink.Text = "" & vbCrLf _
            '            & "<script type=""text/javascript"" src='" & Me.ResolveUrl("../PropertyAgent/js/jquery-1.7.1.min
            '                                                                       ") & "'></script>" & vbCrLf
            '        Page.Header.Controls.Add(litLink)
            '        HttpContext.Current.Items.Add("PropertyAgent-jQuery-ScriptsRegistered", "true")
            '    End If
            'End If

        End Sub

        Private Sub SetSortBy()

            divSort.Visible = PropertySettingsLatest.UserSortable

            BindSortBy()
            BindSortDirection()

        End Sub

        Private Sub BindSortBy()

            If (PropertySettingsLatest.UserSortableFields <> "") Then

                For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                    Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)

                    If (objSortByType = SortByType.CustomField) Then
                        For Each item As String In PropertySettingsLatest.UserSortableFields.Split(","c)
                            If (item = System.Enum.GetName(GetType(SortByType), value)) Then
                                Dim objCustomFieldController As New CustomFieldController
                                Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.PropertySettingsLatest.PropertyAgentModuleID, True)

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
                            For Each item As String In PropertySettingsLatest.UserSortableFields.Split(","c)
                                If (item = System.Enum.GetName(GetType(SortByType), value)) Then
                                    Dim objReviewFieldController As New ReviewFieldController
                                    Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(Me.PropertySettingsLatest.PropertyAgentModuleID)

                                    For Each objReviewField As ReviewFieldInfo In objReviewFields
                                        If (objReviewField.FieldType = ReviewFieldType.Rating) Then
                                            Dim li As New ListItem
                                            li.Value = "rf" & objReviewField.ReviewFieldID.ToString()
                                            li.Text = objReviewField.Caption
                                            drpSortBy.Items.Add(li)
                                        End If
                                    Next
                                End If
                            Next
                        Else
                            For Each item As String In PropertySettingsLatest.UserSortableFields.Split(","c)
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

                If (Me.PropertySettingsLatest.SortBy <> SortByType.CustomField And Me.PropertySettingsLatest.SortBy <> SortByType.ReviewField) Then
                    If Not (drpSortBy.Items.FindByValue(Me.PropertySettingsLatest.SortBy.ToString()) Is Nothing) Then
                        drpSortBy.SelectedValue = Me.PropertySettingsLatest.SortBy.ToString()
                    Else
                        For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                            If (Me.PropertySettingsLatest.SortBy.ToString() = System.Enum.GetName(GetType(SortByType), value)) Then
                                Dim li As New ListItem
                                li.Value = System.Enum.GetName(GetType(SortByType), value)
                                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                                li.Selected = True
                                drpSortBy.Items.Add(li)
                            End If
                        Next
                    End If
                Else
                    If (Me.PropertySettingsLatest.SortBy = SortByType.CustomField) Then
                        If Not (drpSortBy.Items.FindByValue("cf" & Me.PropertySettingsLatest.SortByCustomField.ToString()) Is Nothing) Then
                            drpSortBy.SelectedValue = "cf" & Me.PropertySettingsLatest.SortByCustomField.ToString()
                        Else
                            ' Sort by value not shown, now add it.
                            Dim objCustomFieldController As New CustomFieldController
                            Dim objCustomField As CustomFieldInfo = objCustomFieldController.Get(Convert.ToInt32(Me.PropertySettingsLatest.SortByCustomField.ToString()))

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

                    If (Me.PropertySettingsLatest.SortBy = SortByType.ReviewField) Then
                        If Not (drpSortBy.Items.FindByValue("rf" & Me.PropertySettingsLatest.SortByCustomField.ToString()) Is Nothing) Then
                            drpSortBy.SelectedValue = "rf" & Me.PropertySettingsLatest.SortByCustomField.ToString()
                        Else
                            ' Sort by value not shown, now add it.
                            Dim objReviewFieldController As New ReviewFieldController
                            Dim objReviewField As ReviewFieldInfo = objReviewFieldController.Get(Convert.ToInt32(Me.PropertySettingsLatest.SortByCustomField.ToString()))

                            If (objReviewField IsNot Nothing) Then
                                If (objReviewField.FieldType = ReviewFieldType.Rating) Then
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

                If (value = PropertySettingsLatest.SortDirection) Then
                    li.Selected = True
                End If
                drpSortDirection.Items.Add(li)
            Next

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (Me.PropertySettingsLatest.PropertyAgentModuleID = Null.NullInteger) Then
                    Dim objLabel As New Label
                    objLabel.Text = Localization.GetString("Configure", Me.LocalResourceFile)
                    objLabel.CssClass = "Normal"
                    phProperty.Controls.Add(objLabel)
                    divSort.Visible = False
                    Return
                End If

                If (Me.PropertySettings.Template = "") Then
                    If (Me.PropertySettings.Template(Me.PropertySettingsLatest.PropertyAgentModuleID) = "") Then
                        Dim objLabel As New Label
                        objLabel.Text = Localization.GetString("NoTemplate", Me.LocalResourceFile)
                        objLabel.CssClass = "Normal"
                        phProperty.Controls.Add(objLabel)
                        Return
                    End If
                End If

                If (Page.IsPostBack = False) Then
                    SetSortBy()
                End If

                ReadQueryString()
                InitializeTemplate()
                BindLatest()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                RegisterScripts()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlLatest_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objProperty As PropertyInfo = CType(e.Item.DataItem, PropertyInfo)

                'If an Agent (User/Role) has Submit privileges, they should see an Edit pencil icon 
                'next to their properties in both the Search List and the Latest Properties List
                Dim isMySubmittedProperty As Boolean = False
                If Request.IsAuthenticated AndAlso (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True Or PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True) Then
                    If Me.UserId = objProperty.AuthorID Then
                        isMySubmittedProperty = True
                    End If
                End If

                ' Check for broker access
                If (Request.IsAuthenticated AndAlso isMySubmittedProperty = False AndAlso Me.IsEditable = False) Then
                    If PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True Then
                        If (Me.UserId = objProperty.BrokerID) Then
                            isMySubmittedProperty = True
                        End If
                    End If
                End If

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, (Me.IsEditable Or isMySubmittedProperty), Me.PropertySettingsLatest.PropertyAgentTabID, Me.PropertySettingsLatest.PropertyAgentModuleID, Me.ModuleKey)
                objLayoutController.ListingIndex = e.Item.ItemIndex + 1
                objLayoutController.ProcessItem(e.Item.Controls, Me._objLayoutLatest.Tokens, objProperty, Me.CustomFields)
            End If

        End Sub

        Private Sub rptLatest_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objProperty As PropertyInfo = CType(e.Item.DataItem, PropertyInfo)

                'If an Agent (User/Role) has Submit privileges, they should see an Edit pencil icon 
                'next to their properties in both the Search List and the Latest Properties List
                Dim isMySubmittedProperty As Boolean = False
                If Request.IsAuthenticated AndAlso (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True Or PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True) Then
                    If Me.UserId = objProperty.AuthorID Then
                        isMySubmittedProperty = True
                    End If
                End If

                ' Check for broker access
                If (Request.IsAuthenticated AndAlso isMySubmittedProperty = False AndAlso Me.IsEditable = False) Then
                    If PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True Then
                        If (Me.UserId = objProperty.BrokerID) Then
                            isMySubmittedProperty = True
                        End If
                    End If
                End If

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, (Me.IsEditable Or isMySubmittedProperty), Me.PropertySettingsLatest.PropertyAgentTabID, Me.PropertySettingsLatest.PropertyAgentModuleID, Me.ModuleKey)
                objLayoutController.ListingIndex = e.Item.ItemIndex + 1
                objLayoutController.ProcessItem(e.Item.Controls, Me._objLayoutLatest.Tokens, objProperty, Me.CustomFields)
            End If

        End Sub

#End Region

    End Class

End Namespace
