Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Framework
Imports System.Xml.XPath
Imports Ventrian.PropertyAgent.Mapping

Namespace Ventrian.PropertyAgent

    Partial Public Class LandingPage
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _objLayoutController As LayoutController

        Private _objLayoutFeatured As LayoutInfo
        Private _objLayoutFeaturedHeader As LayoutInfo
        Private _objLayoutFeaturedFooter As LayoutInfo

        Private _objLayoutSearch As LayoutInfo
        Private _objLayoutSearchHeader As LayoutInfo
        Private _objLayoutSearchFooter As LayoutInfo

        Private _objLayoutType As LayoutInfo
        Private _objLayoutTypeHeader As LayoutInfo
        Private _objLayoutTypeFooter As LayoutInfo

        Private _objLayoutRSS As LayoutInfo
        Private _objLayoutRSSHeader As LayoutInfo
        Private _objLayoutRSSFooter As LayoutInfo

        Dim _customFieldIDs As String = Null.NullString
        Dim _propertyTypeID As String = Null.NullString
        Dim _propertyAgentID As String = Null.NullString
        Dim _propertyBrokerID As String = Null.NullString
        Dim _location As String = Null.NullString
        Dim _searchValues As String = Null.NullString

#End Region

#Region " Private Properties "

        Private ReadOnly Property EditPropertyResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty"
            End Get
        End Property

        Private ReadOnly Property LayoutController() As LayoutController
            Get
                Return LayoutController(Nothing)
            End Get
        End Property

        Private ReadOnly Property LayoutController(ByVal objProperty As PropertyInfo) As LayoutController
            Get
                Dim isMySubmittedProperty As Boolean = False
                If Not objProperty Is Nothing Then
                    If (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True Or PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True) Then
                        If Me.UserId = objProperty.AuthorID Then
                            isMySubmittedProperty = True
                        End If
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

                Dim isApprover As Boolean = False
                ' Check for approvers
                If (Not objProperty Is Nothing AndAlso Request.IsAuthenticated AndAlso isMySubmittedProperty = False AndAlso Me.IsEditable = False) Then
                    If PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = True Then
                        isApprover = True
                    End If
                End If

                Return New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, (Me.IsEditable Or isMySubmittedProperty Or isApprover), Me.TabId, Me.ModuleId, Me.ModuleKey)
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub InitializeTemplate()

            _objLayoutController = New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)

            _objLayoutFeatured = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Featured_Item_Html)
            _objLayoutFeaturedHeader = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Featured_Header_Html)
            _objLayoutFeaturedFooter = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Featured_Footer_Html)

            _objLayoutSearch = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Item_Html)
            _objLayoutSearchHeader = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Header_Html)
            _objLayoutSearchFooter = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Footer_Html)

            _objLayoutType = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Item_Html)
            _objLayoutTypeHeader = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Header_Html)
            _objLayoutTypeFooter = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Footer_Html)

            _objLayoutRSS = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Item_Html)
            _objLayoutRSSHeader = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Header_Html)
            _objLayoutRSSFooter = _objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Footer_Html)

        End Sub

        Private Sub BindLandingPage()

            Dim sections As String() = Me.PropertySettings.LandingPageSections.Split(","c)

            For Each section As String In sections
                Select Case section

                    Case "Featured"
                        BindFeatured()

                    Case "Search"
                        BindSearch()

                    Case "Types"
                        BindTypes()

                End Select
            Next

        End Sub

        Private Sub BindFeatured()

            If (Me.PropertySettings.FeaturedEnabled) Then

                Dim latitude As Double = Null.NullInteger
                Dim longitude As Double = Null.NullInteger

                If (Me.PropertySettings.FeaturedSortBy = SortByType.Distance) Then
                    Dim objGeoCodeInfo As GeocodeInfo = UserGeoLocator.GetUserLocation(Request.UserHostAddress)
                    If (objGeoCodeInfo IsNot Nothing) Then
                        latitude = objGeoCodeInfo.Latitude
                        longitude = objGeoCodeInfo.Longitude
                    End If
                End If

                Dim objPropertyController As New PropertyController
                Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, True, Me.PropertySettings.FeaturedSortBy, Me.PropertySettings.FeaturedSortByCustomField, Me.PropertySettings.FeaturedSortDirection, Null.NullString, Null.NullString, 0, Me.PropertySettings.FeaturedMaxNumber, Null.NullInteger, False, True, Null.NullInteger, Null.NullInteger, latitude, longitude, Null.NullDate)

                If (objProperties.Count = 0) Then
                    Return
                End If

                ProcessHeaderFooter(phProperty.Controls, _objLayoutFeaturedHeader.Tokens)

                If (PropertySettings.FeaturedLayoutType = LatestLayoutType.TableLayout) Then

                    Dim objDataList As New System.Web.UI.WebControls.DataList
                    Dim objHandler As New DataListItemEventHandler(AddressOf dlFeatured_ItemDataBound)
                    AddHandler objDataList.ItemDataBound, objHandler

                    objDataList.CellPadding = 0
                    objDataList.CellSpacing = 0

                    objDataList.RepeatColumns = Me.PropertySettings.FeaturedItemsPerRow
                    objDataList.RepeatDirection = RepeatDirection.Horizontal

                    objDataList.ItemStyle.HorizontalAlign = HorizontalAlign.Center

                    objDataList.DataSource = objProperties
                    objDataList.DataBind()

                    phProperty.Controls.Add(objDataList)
                Else
                    Dim objRepeater As New System.Web.UI.WebControls.Repeater
                    Dim objHandler As New RepeaterItemEventHandler(AddressOf rptFeatured_ItemDataBound)
                    AddHandler objRepeater.ItemDataBound, objHandler

                    objRepeater.DataSource = objProperties
                    objRepeater.DataBind()

                    phProperty.Controls.Add(objRepeater)
                End If



                ProcessHeaderFooter(phProperty.Controls, _objLayoutFeaturedFooter.Tokens)

            End If

        End Sub

        Private Sub BindSearch()

            If (Me.PropertySettings.SearchEnabled) Then
                ProcessHeaderFooter(phProperty.Controls, _objLayoutSearchHeader.Tokens)
                ProcessSearch(phProperty.Controls, _objLayoutSearch.Tokens)
                ProcessHeaderFooter(phProperty.Controls, _objLayoutSearchFooter.Tokens)
            End If

        End Sub

        Private Sub BindTypes()

            If (Me.PropertySettings.TypesEnabled) Then

                ProcessHeaderFooter(phProperty.Controls, _objLayoutTypeHeader.Tokens)

                Dim objPropertyTypeController As New PropertyTypeController
                Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.List(Me.ModuleId, True, PropertySettings.TypesSortBy, Null.NullString(), Null.NullInteger)

                Dim objTypesSelected As New List(Of PropertyTypeInfo)

                For Each objType As PropertyTypeInfo In objTypes
                    If (PropertySettings.TypesHideZero) Then
                        If (objType.PropertyCount > 0) Then
                            objTypesSelected.Add(objType)
                        End If
                    Else
                        objTypesSelected.Add(objType)
                    End If
                Next

                If (PropertySettings.TypesLayoutType = LatestLayoutType.TableLayout) Then

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

            End If

        End Sub

        Private Function BuildCustomFieldIDs(ByVal customFieldIDs As String, ByVal value As String) As String

            If (customFieldIDs.Length = 0) Then
                Return value
            Else
                Return customFieldIDs & "," & value
            End If

        End Function

        Private Function BuildSearchValues(ByVal searchValues As String, ByVal value As String) As String

            If (searchValues.Length = 0) Then
                Return Server.UrlEncode(value.Replace(",", "^"))
            Else
                Return searchValues & "," & Server.UrlEncode(value.Replace(",", "^"))
            End If

        End Function

        Private Function GetSearchValue(ByVal customFieldID As Integer) As String

            If (_customFieldIDs <> Null.NullString And _searchValues <> Null.NullString) Then
                Dim IDs As String() = _customFieldIDs.Split(","c)
                Dim searchValues As String() = _searchValues.Split(","c)

                If Not (IDs Is Nothing) Then
                    For i As Integer = 0 To IDs.Length - 1
                        Dim ID As String = IDs(i)
                        If (ID = customFieldID.ToString()) Then
                            Return searchValues(i).Replace("^", ",")
                        End If
                    Next
                End If

                Return ""
            Else
                Return ""
            End If

        End Function

        Private Function GetSearchValueRange(ByVal customFieldID As Integer, ByVal isFrom As Boolean) As String

            Dim val As String = GetSearchValue(customFieldID)

            If (val.Split("|"c).Length > 1) Then
                If (isFrom) Then
                    Return val.Split("|"c)(0)
                Else
                    Return val.Split("|"c)(1)
                End If
            End If

            Return ""

        End Function

        Public Function GetTypeLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal propertyTypeID As Integer) As String

            Dim objTypesParam As New List(Of String)

            objTypesParam.Add(PropertySettings.SEOAgentType & "=ViewType")
            objTypesParam.Add(PropertySettings.SEOPropertyTypeID & "=" & propertyTypeID.ToString())

            If (PropertySettings.TypeParams) Then
                Dim types As New List(Of String)

                Dim objPropertyTypeController As New PropertyTypeController
                Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(moduleID, True, PropertyTypeSortByType.Standard, Null.NullString())

                For Each objType As PropertyTypeInfo In objTypes
                    If (objType.PropertyTypeID = propertyTypeID) Then
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

            Return NavigateURL(tabID, "", objTypesParam.ToArray())

        End Function

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "FEATUREDLINK"
                            Dim objLiteral As New Literal
                            objLiteral.Text = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ViewFeatured")
                            objPlaceHolder.Add(objLiteral)

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

                                        ProcessHeaderFooter(objPlaceHolder, _objLayoutRSSHeader.Tokens)
                                        Dim i As Integer = 1
                                        For Each objRssItem As RssInfo In objRssItems
                                            If (count = Null.NullInteger Or (count <> Null.NullInteger And i < (count + 1))) Then
                                                _objLayoutController.ProcessRssReaderItem(objPlaceHolder, _objLayoutRSS.Tokens, objRssItem, iPtr.ToString() & "-" & i.ToString())
                                                i = i + 1
                                            End If
                                        Next
                                        ProcessHeaderFooter(objPlaceHolder, _objLayoutRSSFooter.Tokens)

                                    End If

                                Catch
                                End Try

                            End If

                    End Select
                End If
            Next

        End Sub

        Private Sub ProcessSearch(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            Dim htFields As New Hashtable

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "AGENT"
                            Dim drpPropertyAgent As New DropDownList
                            drpPropertyAgent.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString() & "-Agent")
                            drpPropertyAgent.CssClass = "NormalTextBox"
                            drpPropertyAgent.Width = PropertySettings.SearchWidth
                            drpPropertyAgent.DataTextField = "DisplayName"
                            drpPropertyAgent.DataValueField = "UserId"

                            Dim objAgentController As New AgentController(PortalSettings, _
                                                                                          PropertySettings, _
                                                                                          PortalId)

                            Dim arrAgents As ArrayList
                            arrAgents = objAgentController.ListActive(PortalId, ModuleId)

                            drpPropertyAgent.DataSource = arrAgents
                            drpPropertyAgent.DataBind()

                            Dim selectText As String = GetResourceString("SelectAgent", "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty", PropertySettings)
                            drpPropertyAgent.Items.Insert(0, New ListItem(selectText, "-1"))

                            If Not (drpPropertyAgent.Items.FindByValue(_propertyAgentID.ToString()) Is Nothing) Then
                                drpPropertyAgent.SelectedValue = _propertyAgentID.ToString()
                            End If

                            objPlaceHolder.Add(drpPropertyAgent)

                        Case "BROKER"
                            Dim drpPropertyBrokers As New DropDownList
                            drpPropertyBrokers.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString() & "-Broker")

                            Dim objAgentController As New AgentController(PortalSettings, _
                                                              PropertySettings, _
                                                              PortalId)
                            Dim arrBrokers As ArrayList = objAgentController.ListOwners(PortalId, ModuleId, PropertySettings.PermissionBroker)

                            drpPropertyBrokers.AutoPostBack = True
                            drpPropertyBrokers.CssClass = "NormalTextBox"
                            drpPropertyBrokers.Width = PropertySettings.SearchWidth
                            drpPropertyBrokers.DataSource = arrBrokers
                            drpPropertyBrokers.DataTextField = "DisplayName"
                            drpPropertyBrokers.DataValueField = "UserId"
                            drpPropertyBrokers.DataBind()

                            AddHandler drpPropertyBrokers.SelectedIndexChanged, AddressOf drpPropertyBrokers_SelectedIndexChanged

                            Dim selectText As String = GetResourceString("SelectBroker", "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty", PropertySettings)
                            drpPropertyBrokers.Items.Insert(0, New ListItem(selectText, "-1"))

                            If Not (drpPropertyBrokers.Items.FindByValue(_propertyBrokerID.ToString()) Is Nothing) Then
                                drpPropertyBrokers.SelectedValue = _propertyBrokerID.ToString()
                            End If

                            objPlaceHolder.Add(drpPropertyBrokers)

                        Case "LOCATION"
                            Dim txtLocation As New TextBox
                            txtLocation.CssClass = "NormalTextBox"
                            txtLocation.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString() & "-Location")
                            txtLocation.EnableViewState = False
                            objPlaceHolder.Add(txtLocation)

                        Case "SEARCH"
                            Dim objSearch As PropertyAgent.Search = CType(Me.LoadControl("Search.ascx"), PropertyAgent.Search)
                            objPlaceHolder.Add(objSearch)

                        Case "SEARCHBUTTON"
                            Dim btnSearch As New Button
                            btnSearch.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString())
                            btnSearch.Text = Localization.GetString("cmdSearch", Me.LocalResourceFile)
                            btnSearch.EnableViewState = False
                            AddHandler btnSearch.Click, AddressOf Search_Click
                            objPlaceHolder.Add(btnSearch)

                        Case "SEARCHLINK"
                            Dim cmdSearch As New LinkButton
                            cmdSearch.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString())
                            cmdSearch.Text = Localization.GetString("cmdSearch", Me.LocalResourceFile)
                            cmdSearch.EnableViewState = False
                            cmdSearch.CssClass = PropertySettings.SearchStyle
                            cmdSearch.BorderStyle = BorderStyle.None
                            AddHandler cmdSearch.Click, AddressOf Search_Click
                            objPlaceHolder.Add(cmdSearch)

                        Case "TYPE"
                            Dim drpPropertyTypes As New DropDownList
                            drpPropertyTypes.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString() & "-Type")
                            drpPropertyTypes.EnableViewState = False
                            drpPropertyTypes.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)

                            Dim objPropertyTypeController As New PropertyTypeController
                            drpPropertyTypes.CssClass = "NormalTextBox"
                            drpPropertyTypes.DataTextField = "NameIndentedWithCount"
                            drpPropertyTypes.DataValueField = "PropertyTypeID"

                            If (Me.PropertySettings.SearchHideTypesCount) Then
                                drpPropertyTypes.DataTextField = "NameIndented"
                            End If
                            Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(ModuleId, True, PropertySettings.TypesSortBy, Null.NullString())
                            If (Me.PropertySettings.SearchHideZeroTypes) Then
                                Dim objTypesSelected As New List(Of PropertyTypeInfo)

                                For Each objType As PropertyTypeInfo In objTypes
                                    If (objType.PropertyCount > 0) Then
                                        objTypesSelected.Add(objType)
                                    End If
                                Next
                                drpPropertyTypes.DataSource = objTypesSelected
                                drpPropertyTypes.DataBind()
                            Else
                                drpPropertyTypes.DataSource = objTypes
                                drpPropertyTypes.DataBind()
                            End If

                            For Each objCustomField As CustomFieldInfo In CType(Parent.Parent, PropertyAgentBase).CustomFields
                                If (objCustomField.IsPublished AndAlso objCustomField.FieldType = CustomFieldType.DropDownList AndAlso objCustomField.FieldElementType = FieldElementType.LinkedToPropertyType) Then
                                    drpPropertyTypes.AutoPostBack = True
                                    AddHandler drpPropertyTypes.SelectedIndexChanged, AddressOf drpPropertyType_SelectedIndexChanged
                                End If
                            Next

                            Dim selectText As String = GetResourceString("SelectType", "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty", PropertySettings)
                            drpPropertyTypes.Items.Insert(0, New ListItem(selectText, "-1"))
                            objPlaceHolder.Add(drpPropertyTypes)

                            If Not (drpPropertyTypes.Items.FindByValue(_propertyTypeID.ToString()) Is Nothing) Then
                                drpPropertyTypes.SelectedValue = _propertyTypeID.ToString()
                            End If

                        Case "TYPECASCADE"

                            Dim objPropertyTypeController As New PropertyTypeController
                            Dim objTypesAll As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(ModuleId, True, Me.PropertySettings.TypesSortBy, Null.NullString())

                            Dim maxLevel As Integer = 1
                            For Each objPropertyType As PropertyTypeInfo In objTypesAll

                                Dim currentLevel = 1
                                Dim typeIndented As String = objPropertyType.NameIndented

                                While typeIndented.StartsWith("..")
                                    currentLevel = currentLevel + 1
                                    typeIndented = typeIndented.Substring(2, typeIndented.Length - 2)
                                End While

                                If (currentLevel > maxLevel) Then
                                    maxLevel = currentLevel
                                End If

                            Next

                            Dim typesListParam As New List(Of Integer)
                            If (IsPostBack = False And (_propertyTypeID <> Null.NullString)) Then

                                Dim objType As PropertyTypeInfo = objPropertyTypeController.Get(ModuleId, _propertyTypeID)

                                If (objType IsNot Nothing) Then
                                    typesListParam.Add(objType.PropertyTypeID)
                                    While objType.ParentID <> Null.NullInteger
                                        objType = objPropertyTypeController.Get(ModuleId, objType.ParentID)
                                        If (objType Is Nothing) Then
                                            Exit While
                                        End If
                                        typesListParam.Insert(0, objType.PropertyTypeID)
                                    End While
                                End If

                            End If

                            Dim bReset As Boolean = False
                            Dim typesListCurrent As New Hashtable

                            For i As Integer = 1 To maxLevel

                                Dim drpPropertyTypesCascade As New DropDownList

                                drpPropertyTypesCascade.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString() & "-TypeCascade-" & i.ToString())
                                drpPropertyTypesCascade.EnableViewState = False
                                drpPropertyTypesCascade.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                                drpPropertyTypesCascade.CssClass = "NormalTextBox"
                                drpPropertyTypesCascade.DataTextField = "NameWithCount"
                                drpPropertyTypesCascade.DataValueField = "PropertyTypeID"

                                If (Me.PropertySettings.SearchHideTypesCount) Then
                                    drpPropertyTypesCascade.DataTextField = "Name"
                                End If

                                If (i = 1) Then

                                    Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.List(ModuleId, True, Me.PropertySettings.TypesSortBy, Null.NullString(), Null.NullInteger)

                                    If (Me.PropertySettings.SearchHideZeroTypes) Then
                                        Dim objTypesSelected As New List(Of PropertyTypeInfo)

                                        For Each objType As PropertyTypeInfo In objTypes
                                            If (objType.PropertyCount > 0) Then
                                                objTypesSelected.Add(objType)
                                            End If
                                        Next
                                        drpPropertyTypesCascade.DataSource = objTypesSelected
                                        drpPropertyTypesCascade.DataBind()
                                    Else
                                        drpPropertyTypesCascade.DataSource = objTypes
                                        drpPropertyTypesCascade.DataBind()
                                    End If

                                    drpPropertyTypesCascade.AutoPostBack = True

                                    If (typesListParam.Count > 0) Then
                                        If Not (drpPropertyTypesCascade.Items.FindByValue(typesListParam(0).ToString()) Is Nothing) Then
                                            drpPropertyTypesCascade.SelectedValue = typesListParam(0).ToString()
                                        End If
                                    End If

                                    For Each li As ListItem In drpPropertyTypesCascade.Items
                                        typesListCurrent.Add(li.Value, li.Value)
                                    Next

                                Else

                                    Dim parentID As Integer = Null.NullInteger

                                    If (IsPostBack) Then

                                        If (bReset = False) Then
                                            Dim id As String = "dnn$ctr" & ModuleId.ToString() & "$Default$LandingPage$" & Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString() & "-TypeCascade-" & (i - 1).ToString())

                                            If (Request(id) <> "") Then
                                                If (IsNumeric(Request(id))) Then
                                                    If (Convert.ToInt32(Request(id)) <> -1) Then
                                                        parentID = Convert.ToInt32(Request(id))
                                                        If (id = Page.Request.Params.Get("__EVENTTARGET")) Then
                                                            bReset = True
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If

                                    Else

                                        If (typesListParam.Count > 0) Then
                                            If ((i - 2) < (typesListParam.Count)) Then
                                                parentID = typesListParam(i - 2)
                                            End If
                                        End If

                                    End If

                                    If (parentID <> Null.NullInteger) Then

                                        Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.List(ModuleId, True, Me.PropertySettings.TypesSortBy, Null.NullString(), parentID)

                                        If (Me.PropertySettings.SearchHideZeroTypes) Then
                                            Dim objTypesSelected As New List(Of PropertyTypeInfo)

                                            For Each objType As PropertyTypeInfo In objTypes
                                                If (objType.PropertyCount > 0) Then
                                                    objTypesSelected.Add(objType)
                                                End If
                                            Next
                                            drpPropertyTypesCascade.DataSource = objTypesSelected
                                            drpPropertyTypesCascade.DataBind()
                                        Else
                                            drpPropertyTypesCascade.DataSource = objTypes
                                            drpPropertyTypesCascade.DataBind()
                                        End If

                                        If (typesListParam.Count >= i) Then
                                            If Not (drpPropertyTypesCascade.Items.FindByValue(typesListParam(i - 1).ToString()) Is Nothing) Then
                                                drpPropertyTypesCascade.SelectedValue = typesListParam(i - 1).ToString()
                                            End If
                                        End If

                                        For Each li As ListItem In drpPropertyTypesCascade.Items
                                            typesListCurrent.Add(li.Value, li.Value)
                                        Next

                                    End If

                                    If (i < maxLevel) Then
                                        drpPropertyTypesCascade.AutoPostBack = True
                                    End If

                                    If (drpPropertyTypesCascade.Items.Count = 0) Then
                                        drpPropertyTypesCascade.Enabled = False
                                    End If

                                End If

                                Dim selectText As String = GetResourceString("SelectType" + i.ToString(), "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty", PropertySettings)
                                If (selectText = ("RESX:" & "SelectType" & i.ToString() & ".Text") Or selectText = "" Or selectText = "-1") Then
                                    selectText = GetResourceString("SelectType", "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty", PropertySettings)
                                End If

                                drpPropertyTypesCascade.Items.Insert(0, New ListItem(selectText, "-1"))
                                objPlaceHolder.Add(drpPropertyTypesCascade)

                            Next

                        Case "WILDCARD"
                            Dim txtWildcard As New TextBox
                            txtWildcard.CssClass = "NormalTextBox"
                            txtWildcard.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString() & "-Wildcard")
                            txtWildcard.EnableViewState = False
                            objPlaceHolder.Add(txtWildcard)

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CAPTION:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8)
                                For Each objCustomField As CustomFieldInfo In CustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID(ModuleKey & "-Search-" & iPtr.ToString())
                                        objLiteral.Text = objCustomField.Caption
                                        objLiteral.EnableViewState = False
                                        objPlaceHolder.Add(objLiteral)
                                        Exit For
                                    End If
                                Next
                            End If

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7)

                                For Each objCustomField As CustomFieldInfo In CustomFields
                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                        If (htFields.Contains(objCustomField.CustomFieldID) = False) Then
                                            RenderCustomField(phProperty.Controls, objCustomField)
                                            htFields.Add(objCustomField.CustomFieldID, objCustomField.CustomFieldID)
                                        End If
                                        Exit For
                                    End If
                                Next

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

                    End Select
                End If
            Next

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("CustomFieldIDs") Is Nothing) Then
                _customFieldIDs = Request("CustomFieldIDs")
            End If

            Dim propertyTypeIDParam As String = PropertySettings.SEOPropertyTypeID
            If (Request(propertyTypeIDParam) = "") Then
                propertyTypeIDParam = "PropertyTypeID"
            End If
            If Not (Request(propertyTypeIDParam) Is Nothing) Then
                _propertyTypeID = Request(propertyTypeIDParam)
            End If

            If Not (Request("PropertyAgentID") Is Nothing) Then
                _propertyAgentID = Request("PropertyAgentID")
            End If

            If Not (Request("PropertyBrokerID") Is Nothing) Then
                _propertyBrokerID = Request("PropertyBrokerID")
            End If

            If Not (Request("Location") Is Nothing) Then
                _location = Server.UrlDecode(Request("Location"))
            End If

            If Not (Request("SearchValues") Is Nothing) Then
                _searchValues = Request("SearchValues")
            End If

        End Sub

        Private Sub RenderCustomField(ByRef objPlaceHolder As ControlCollection, ByVal objCustomField As CustomFieldInfo)

            Select Case (objCustomField.FieldType)

                Case CustomFieldType.OneLineTextBox

                    Select Case objCustomField.SearchType

                        Case SearchType.Default
                            Dim objTextBox As New TextBox
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.CustomFieldID.ToString()
                            objTextBox.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                            objPlaceHolder.Add(objTextBox)

                            objTextBox.Text = GetSearchValue(objCustomField.CustomFieldID)

                        Case SearchType.DropDown
                            Dim objDropDown As New DropDownList
                            objDropDown.CssClass = "NormalTextBox"
                            objDropDown.ID = objCustomField.CustomFieldID.ToString()
                            objDropDown.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                            Dim objPropertyValueController As New PropertyValueController
                            Dim objPropertyValues As List(Of PropertyValueInfo) = objPropertyValueController.ListByCustomField(objCustomField.CustomFieldID)
                            For Each objPropertyValue As PropertyValueInfo In objPropertyValues
                                objDropDown.Items.Add(New ListItem(objPropertyValue.CustomValue, objPropertyValue.CustomValue))
                            Next
                            objDropDown.Items.Insert(0, New ListItem(Localization.GetString("SelectItem", Me.EditPropertyResourceFile), "-1"))
                            objPlaceHolder.Add(objDropDown)

                        Case SearchType.Range
                            If (objCustomField.FieldElementsFrom <> "") Then
                                Dim objDropDownFrom As New DropDownList
                                objDropDownFrom.CssClass = "NormalTextBox"
                                objDropDownFrom.ID = "From" & objCustomField.CustomFieldID.ToString()
                                Dim values As String() = objCustomField.FieldElementsFrom.Split(Convert.ToChar("|"))
                                For Each value As String In values
                                    Dim item As New ListItem
                                    item.Value = value
                                    item.Text = value
                                    Select Case objCustomField.ValidationType
                                        Case CustomFieldValidationType.Currency
                                            Try
                                                Dim culture As String = "en-US"

                                                Select Case PropertySettings.Currency

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

                                                    Case CurrencyType.CZK
                                                        culture = "cs-CZ"
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

                                                Dim portalFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
                                                Dim format As String = "{0:C" & PropertySettings.CurrencyDecimalPlaces.ToString() & "}"
                                                item.Text = String.Format(portalFormat.NumberFormat, format, Double.Parse(value))
                                            Catch
                                            End Try
                                        Case CustomFieldValidationType.Date
                                            Try
                                                item.Text = DateTime.Parse(value).ToShortDateString()
                                            Catch
                                            End Try
                                    End Select
                                    objDropDownFrom.Items.Add(item)
                                Next
                                objDropDownFrom.Width = Unit.Pixel(150)
                                objPlaceHolder.Add(objDropDownFrom)

                                objDropDownFrom.Items.Insert(0, New ListItem(Localization.GetString("SelectFrom", Me.EditPropertyResourceFile), "-1"))

                                If Not (objDropDownFrom.Items.FindByValue(GetSearchValueRange(objCustomField.CustomFieldID, True)) Is Nothing) Then
                                    objDropDownFrom.SelectedValue = GetSearchValueRange(objCustomField.CustomFieldID, True)
                                End If
                            Else
                                Dim objTextBoxFrom As New TextBox
                                objTextBoxFrom.CssClass = "NormalTextBox"
                                objTextBoxFrom.ID = "From" & objCustomField.CustomFieldID.ToString()
                                objTextBoxFrom.Width = Unit.Pixel(100)
                                objPlaceHolder.Add(objTextBoxFrom)

                                If (objCustomField.ValidationType <> CustomFieldValidationType.None) Then
                                    Dim valCompare As New CompareValidator
                                    valCompare.ControlToValidate = objTextBoxFrom.ID
                                    Select Case objCustomField.ValidationType

                                        Case CustomFieldValidationType.Currency
                                            valCompare.Type = ValidationDataType.Integer
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateCurrency", Me.EditPropertyResourceFile)

                                        Case CustomFieldValidationType.Date
                                            valCompare.Type = ValidationDataType.Date
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateDate", Me.EditPropertyResourceFile)

                                        Case CustomFieldValidationType.Double
                                            valCompare.Type = ValidationDataType.Double
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateDecimal", Me.EditPropertyResourceFile)

                                        Case CustomFieldValidationType.Integer
                                            valCompare.Type = ValidationDataType.Integer
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateNumber", Me.EditPropertyResourceFile)

                                    End Select
                                    valCompare.CssClass = "NormalRed"
                                    valCompare.Display = ValidatorDisplay.Dynamic
                                    objPlaceHolder.Add(valCompare)
                                End If

                                objTextBoxFrom.Text = GetSearchValueRange(objCustomField.CustomFieldID, True)
                            End If

                            If (objCustomField.FieldElementsFrom = "" Or objCustomField.FieldElementsTo = "") Then
                                Dim objLabelTo As New Label
                                objLabelTo.CssClass = "Normal"
                                objLabelTo.Text = Localization.GetString("To", Me.EditPropertyResourceFile)
                                objPlaceHolder.Add(objLabelTo)
                            End If

                            If (objCustomField.FieldElementsTo <> "") Then
                                Dim objDropDownTo As New DropDownList
                                objDropDownTo.CssClass = "NormalTextBox"
                                objDropDownTo.ID = "To" & objCustomField.CustomFieldID.ToString()
                                Dim values As String() = objCustomField.FieldElementsTo.Split(Convert.ToChar("|"))
                                For Each value As String In values
                                    Dim item As New ListItem
                                    item.Value = value
                                    item.Text = value
                                    Select Case objCustomField.ValidationType
                                        Case CustomFieldValidationType.Currency
                                            Try
                                                Dim culture As String = "en-US"

                                                Select Case PropertySettings.Currency

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

                                                    Case CurrencyType.CZK
                                                        culture = "cs-CZ"
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

                                                Dim portalFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
                                                Dim format As String = "{0:C" & PropertySettings.CurrencyDecimalPlaces.ToString() & "}"
                                                item.Text = String.Format(portalFormat.NumberFormat, format, Double.Parse(value))
                                            Catch
                                            End Try
                                        Case CustomFieldValidationType.Date
                                            Try
                                                item.Text = DateTime.Parse(value).ToShortDateString()
                                            Catch
                                            End Try
                                    End Select
                                    objDropDownTo.Items.Add(item)
                                Next
                                objDropDownTo.Width = Unit.Pixel(150)
                                objPlaceHolder.Add(objDropDownTo)

                                objDropDownTo.Items.Insert(0, New ListItem(Localization.GetString("SelectTo", Me.EditPropertyResourceFile), "-1"))

                                If Not (objDropDownTo.Items.FindByValue(GetSearchValueRange(objCustomField.CustomFieldID, False)) Is Nothing) Then
                                    objDropDownTo.SelectedValue = GetSearchValueRange(objCustomField.CustomFieldID, False)
                                End If
                            Else

                                Dim objTextBoxTo As New TextBox
                                objTextBoxTo.CssClass = "NormalTextBox"
                                objTextBoxTo.ID = "To" & objCustomField.CustomFieldID.ToString()
                                objTextBoxTo.Width = Unit.Pixel(100)
                                objPlaceHolder.Add(objTextBoxTo)

                                If (objCustomField.ValidationType <> CustomFieldValidationType.None) Then
                                    Dim valCompare As New CompareValidator
                                    valCompare.ControlToValidate = objTextBoxTo.ID
                                    Select Case objCustomField.ValidationType

                                        Case CustomFieldValidationType.Currency
                                            valCompare.Type = ValidationDataType.Integer
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateCurrency", Me.EditPropertyResourceFile)

                                        Case CustomFieldValidationType.Date
                                            valCompare.Type = ValidationDataType.Date
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateDate", Me.EditPropertyResourceFile)

                                        Case CustomFieldValidationType.Double
                                            valCompare.Type = ValidationDataType.Double
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateDecimal", Me.EditPropertyResourceFile)

                                        Case CustomFieldValidationType.Integer
                                            valCompare.Type = ValidationDataType.Integer
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateNumber", Me.EditPropertyResourceFile)

                                    End Select
                                    valCompare.CssClass = "NormalRed"
                                    valCompare.Display = ValidatorDisplay.Dynamic
                                    objPlaceHolder.Add(valCompare)
                                End If

                                objTextBoxTo.Text = GetSearchValueRange(objCustomField.CustomFieldID, False)
                            End If

                    End Select

                Case CustomFieldType.MultiLineTextBox

                    Dim objTextBox As New TextBox
                    objTextBox.CssClass = "NormalTextBox"
                    objTextBox.ID = objCustomField.CustomFieldID.ToString()
                    objTextBox.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                    objPlaceHolder.Add(objTextBox)

                    objTextBox.Text = GetSearchValue(objCustomField.CustomFieldID)

                Case CustomFieldType.RichTextBox

                    Dim objTextBox As New TextBox
                    objTextBox.CssClass = "NormalTextBox"
                    objTextBox.ID = objCustomField.CustomFieldID.ToString()
                    objTextBox.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                    objPlaceHolder.Add(objTextBox)

                    objTextBox.Text = GetSearchValue(objCustomField.CustomFieldID)

                Case CustomFieldType.DropDownList

                    Dim objListControl As ListControl

                    If (objCustomField.SearchType = SearchType.Multiple) Then
                        objListControl = New ListBox
                        CType(objListControl, ListBox).SelectionMode = ListSelectionMode.Multiple
                    Else
                        objListControl = New DropDownList
                    End If
                    objListControl.CssClass = "NormalTextBox"
                    objListControl.ID = objCustomField.CustomFieldID.ToString()
                    objListControl.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)

                    If (objCustomField.FieldElementType = FieldElementType.Standard) Then
                        Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                        For Each value As String In values
                            If (value.Trim() <> "") Then
                                If (objCustomField.IncludeCount) Then
                                    Dim objPropertyController As New PropertyController()
                                    Dim count As Integer = 0
                                    objPropertyController.List(ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                    If (objCustomField.HideZeroCount) Then
                                        If (count > 0) Then
                                            objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                        End If
                                    Else
                                        objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                    End If
                                Else
                                    If (objCustomField.HideZeroCount) Then
                                        Dim objPropertyController As New PropertyController()
                                        Dim count As Integer = 0
                                        objPropertyController.List(ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                        If (count > 0) Then
                                            objListControl.Items.Add(value)
                                        End If
                                    Else
                                        objListControl.Items.Add(value)
                                    End If
                                End If
                            End If
                        Next

                        For Each objCustomFieldDropDown As CustomFieldInfo In CustomFields
                            If (objCustomFieldDropDown.IsPublished AndAlso objCustomFieldDropDown.IsSearchable AndAlso objCustomFieldDropDown.FieldType = CustomFieldType.DropDownList AndAlso objCustomFieldDropDown.FieldElementType = FieldElementType.LinkedToDropdown AndAlso objCustomFieldDropDown.FieldElementDropDown = objCustomField.CustomFieldID) Then
                                objListControl.AutoPostBack = True
                                AddHandler objListControl.SelectedIndexChanged, AddressOf drpLinked_SelectedIndexChanged
                            End If
                        Next
                    End If

                    If (objCustomField.FieldElementType = FieldElementType.SqlQuery) Then
                        For Each value As String In objCustomField.FieldElementsSql
                            If (value.Trim() <> "") Then
                                If (objCustomField.IncludeCount) Then
                                    Dim objPropertyController As New PropertyController()
                                    Dim count As Integer = 0
                                    objPropertyController.List(ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                    If (objCustomField.HideZeroCount) Then
                                        If (count > 0) Then
                                            objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                        End If
                                    Else
                                        objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                    End If
                                Else
                                    If (objCustomField.HideZeroCount) Then
                                        Dim objPropertyController As New PropertyController()
                                        Dim count As Integer = 0
                                        objPropertyController.List(ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                        If (count > 0) Then
                                            objListControl.Items.Add(value)
                                        End If
                                    Else
                                        objListControl.Items.Add(value)
                                    End If
                                End If
                            End If
                        Next
                    End If

                    If (objCustomField.FieldElementType = FieldElementType.LinkedToPropertyType) Then
                        objListControl.Attributes.Add("CustomFieldID", objCustomField.CustomFieldID.ToString())
                        objListControl.ID = objListControl.ID & "-linkedtype"

                        If (_propertyTypeID <> "") Then
                            Dim objPropertyTypeController As New PropertyTypeController
                            Dim types As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(ModuleId, True, PropertySettings.TypesSortBy, Null.NullString())

                            Dim i As Integer = 0
                            For Each objType As PropertyTypeInfo In types
                                If (objType.PropertyTypeID.ToString() = _propertyTypeID) Then
                                    If (i < objCustomField.FieldElements.Split(vbCrLf).Length) Then
                                        Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(i).Split(Convert.ToChar("|"))
                                        For Each value As String In values
                                            If (value.Trim() <> "") Then
                                                If (objCustomField.IncludeCount) Then
                                                    Dim objPropertyController As New PropertyController()
                                                    Dim count As Integer = 0
                                                    objPropertyController.List(ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                    If (objCustomField.HideZeroCount) Then
                                                        If (count > 0) Then
                                                            objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                        End If
                                                    Else
                                                        objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                    End If
                                                Else
                                                    If (objCustomField.HideZeroCount) Then
                                                        Dim objPropertyController As New PropertyController()
                                                        Dim count As Integer = 0
                                                        objPropertyController.List(ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                        If (count > 0) Then
                                                            objListControl.Items.Add(value.Replace(vbLf, ""))
                                                        End If
                                                    Else
                                                        objListControl.Items.Add(value.Replace(vbLf, ""))
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                    Exit For
                                End If
                                i = i + 1
                            Next
                        End If
                    End If

                    If (objCustomField.FieldElementType = FieldElementType.LinkedToDropdown) Then
                        objListControl.Attributes.Add("CustomFieldID", objCustomField.CustomFieldID.ToString())
                        objListControl.ID = objListControl.ID & "-linkeddropdown"

                        Dim found As Boolean = False
                        If (found = False) Then
                            Dim fieldVal As String = GetSearchValue(objCustomField.FieldElementDropDown)
                            If (fieldVal <> "") Then

                                For Each objCustomFieldLinked As CustomFieldInfo In CustomFields
                                    If (objCustomFieldLinked.CustomFieldID = objCustomField.FieldElementDropDown) Then
                                        found = True
                                        Dim i As Integer = 0
                                        For Each valItem As String In objCustomFieldLinked.FieldElements.Split("|")
                                            If (valItem.ToLower() = fieldVal.ToLower()) Then
                                                Dim valueItems As String() = objCustomField.FieldElements.Split(vbCrLf)(i).Split(Convert.ToChar("|"))
                                                For Each value As String In valueItems
                                                    If (value.Trim() <> "") Then
                                                        If (objCustomField.IncludeCount) Then
                                                            Dim objPropertyController As New PropertyController()
                                                            Dim count As Integer = 0
                                                            objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                            If (objCustomField.HideZeroCount) Then
                                                                If (count > 0) Then
                                                                    objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                End If
                                                            Else
                                                                objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                            End If
                                                        Else
                                                            If (objCustomField.HideZeroCount) Then
                                                                Dim objPropertyController As New PropertyController()
                                                                Dim count As Integer = 0
                                                                objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                If (count > 0) Then
                                                                    objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                End If
                                                            Else
                                                                objListControl.Items.Add(value.Replace(vbLf, ""))
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            End If
                                            i = i + 1
                                        Next
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If

                    If (objCustomField.SearchType = SearchType.Multiple) Then
                        Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                        For Each val As String In vals
                            For Each item As ListItem In objListControl.Items
                                If (item.Value = val) Then
                                    item.Selected = True
                                End If
                            Next
                        Next
                    Else
                        Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                        selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                        objListControl.Items.Insert(0, New ListItem(selectText, "-1"))

                        Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                        If (val <> "") Then
                            If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                                objListControl.SelectedValue = val
                            End If
                        End If
                    End If

                    objPlaceHolder.Add(objListControl)

                Case CustomFieldType.ListBox

                    Dim objListControl As ListControl

                    If (objCustomField.SearchType = SearchType.Default) Then
                        objListControl = New ListBox
                        CType(objListControl, ListBox).SelectionMode = ListSelectionMode.Multiple
                    Else
                        objListControl = New DropDownList
                    End If
                    objListControl.CssClass = "NormalTextBox"
                    objListControl.ID = objCustomField.CustomFieldID.ToString()
                    objListControl.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)

                    Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                    For Each value As String In values
                        If (value.Trim() <> "") Then
                            objListControl.Items.Add(value)
                        End If
                    Next

                    If (objCustomField.SearchType = SearchType.Default) Then
                        Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                        For Each val As String In vals
                            For Each item As ListItem In objListControl.Items
                                If (item.Value = val) Then
                                    item.Selected = True
                                End If
                            Next
                        Next
                    Else
                        Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                        selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                        objListControl.Items.Insert(0, New ListItem(selectText, "-1"))

                        Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                        If (val <> "") Then
                            If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                                objListControl.SelectedValue = val
                            End If
                        End If
                    End If

                    objPlaceHolder.Add(objListControl)

                Case CustomFieldType.CheckBox

                    Dim objCheckBox As New CheckBox
                    objCheckBox.CssClass = "Normal"
                    objCheckBox.ID = objCustomField.CustomFieldID.ToString()
                    objPlaceHolder.Add(objCheckBox)

                    Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                    If (val <> "") Then
                        Try
                            objCheckBox.Checked = Boolean.Parse(val)
                        Catch

                        End Try
                    End If

                Case CustomFieldType.MultiCheckBox

                    If (objCustomField.SearchType = SearchType.DropDown) Then

                        Dim objListControl = New DropDownList
                        objListControl.CssClass = "NormalTextBox"
                        objListControl.ID = objCustomField.CustomFieldID.ToString()
                        objListControl.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)

                        Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                        For Each value As String In values
                            If (value <> "") Then
                                objListControl.Items.Add(value)
                            End If
                        Next

                        Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                        selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                        objListControl.Items.Insert(0, New ListItem(selectText, "-1"))

                        Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                        If (val <> "") Then
                            If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                                objListControl.SelectedValue = val
                            End If
                        End If

                        objPlaceHolder.Add(objListControl)

                    Else
                        Dim objCheckBoxList As New CheckBoxList
                        objCheckBoxList.CssClass = "Normal"
                        objCheckBoxList.ID = objCustomField.CustomFieldID.ToString()
                        objCheckBoxList.RepeatColumns = CType(Parent.Parent, PropertyAgentBase).PropertySettings.CheckBoxItemsPerRow
                        objCheckBoxList.RepeatDirection = RepeatDirection.Horizontal

                        Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                        For Each value As String In values
                            If (value <> "") Then
                                objCheckBoxList.Items.Add(value)
                            End If
                        Next

                        Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                        For Each val As String In vals
                            For Each item As ListItem In objCheckBoxList.Items
                                If (item.Value = val) Then
                                    item.Selected = True
                                End If
                            Next
                        Next

                        objPlaceHolder.Add(objCheckBoxList)
                    End If

                Case CustomFieldType.RadioButton

                    Dim objListControl As ListControl

                    If (objCustomField.SearchType = SearchType.Multiple) Then
                        objListControl = New CheckBoxList
                    Else
                        objListControl = New RadioButtonList
                    End If

                    objListControl.CssClass = "Normal"
                    objListControl.ID = objCustomField.CustomFieldID.ToString()

                    If (objCustomField.SearchType = SearchType.Multiple) Then
                        CType(objListControl, CheckBoxList).RepeatDirection = RepeatDirection.Horizontal
                        CType(objListControl, CheckBoxList).RepeatLayout = RepeatLayout.Table
                        CType(objListControl, CheckBoxList).RepeatColumns = CType(Parent.Parent, PropertyAgentBase).PropertySettings.CheckBoxItemsPerRow
                    Else
                        CType(objListControl, RadioButtonList).RepeatDirection = RepeatDirection.Horizontal
                        CType(objListControl, RadioButtonList).RepeatLayout = RepeatLayout.Table
                        CType(objListControl, RadioButtonList).RepeatColumns = CType(Parent.Parent, PropertyAgentBase).PropertySettings.RadioButtonItemsPerRow
                    End If

                    Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                    For Each value As String In values
                        If (value <> "") Then
                            objListControl.Items.Add(value)
                        End If
                    Next

                    If (objCustomField.SearchType = SearchType.Multiple) Then
                        Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                        For Each val As String In vals
                            For Each item As ListItem In objListControl.Items
                                If (item.Value = val) Then
                                    item.Selected = True
                                End If
                            Next
                        Next
                    Else
                        Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                        If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                            objListControl.SelectedValue = val
                        End If
                    End If

                    objPlaceHolder.Add(objListControl)

            End Select

        End Sub

        Private Sub RegisterScripts()

            'If (HttpContext.Current.Items("PropertyAgent-ScriptsRegistered") Is Nothing) Then
            '    Dim objCSS As System.Web.UI.Control = BasePage.FindControl("CSS")

            '    If Not (objCSS Is Nothing) Then
            '        Dim litLink As New Literal
            '        litLink.Text = "" & vbCrLf _
            '            & "<script type=""text/javascript"" src='" & Me.ResolveUrl("js/lightbox/jquery.lightbox-0.4.pack.js") & "'></script>" & vbCrLf
            '        objCSS.Controls.Add(litLink)
            '    End If
            '    HttpContext.Current.Items.Add("PropertyAgent-ScriptsRegistered", "true")
            'End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                'If (AJAX.IsInstalled) Then
                '    AJAX.RegisterScriptManager()
                '    AJAX.WrapUpdatePanelControl(pnlSearch, True)
                'End If

                If (PropertySettings.RssEnable) Then
                    If (HttpContext.Current.Items.Contains("RSS-PropertyAgent") = False) Then
                        HttpContext.Current.Items.Add("RSS-PropertyAgent", NavigateURL(Me.TabId, "", "agentType=rss", "title=" & Server.UrlEncode(PropertySettings.RssTitleLatest(True))))
                    End If
                    If (HttpContext.Current.Items.Contains("RSS-PropertyAgent-Title") = False) Then
                        HttpContext.Current.Items.Add("RSS-PropertyAgent-Title", PropertySettings.RssTitleLatest(True))
                    End If
                End If

                InitializeTemplate()
                ReadQueryString()
                BindLandingPage()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                _objLayoutController.LoadStyleSheet(Me.PropertySettings.Template)

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim customFieldIDs As String = ""
            Dim location As String = ""
            Dim searchValues As String = ""
            Dim propertyTypeIDs As String = ""
            Dim propertyAgentIDs As String = ""
            Dim propertyBrokerIDs As String = ""

            For Each objControl As Control In phProperty.Controls
                If (objControl.ID <> "") Then

                    If (objControl.ID.ToLower().EndsWith("agent")) Then
                        Dim drpPropertyAgents As DropDownList = CType(objControl, DropDownList)
                        If (drpPropertyAgents.SelectedValue <> "-1") Then
                            propertyAgentIDs = "PropertyAgentID=" & drpPropertyAgents.SelectedValue
                        End If
                    End If

                    If (objControl.ID.ToLower().EndsWith("broker")) Then
                        Dim drpPropertyBrokers As DropDownList = CType(objControl, DropDownList)
                        If (drpPropertyBrokers.SelectedValue <> "-1") Then
                            propertyBrokerIDs = "PropertyBrokerID=" & drpPropertyBrokers.SelectedValue
                        End If
                    End If

                    If (objControl.ID.ToLower().EndsWith("type") And Not objControl.ID.ToLower().EndsWith("linkedtype")) Then
                        Dim drpPropertyTypes As DropDownList = CType(objControl, DropDownList)
                        If (drpPropertyTypes.SelectedValue <> "-1") Then
                            propertyTypeIDs = PropertySettings.SEOPropertyTypeID & "=" & drpPropertyTypes.SelectedValue
                        End If
                    End If

                    If (objControl.ID.ToLower().Contains("typecascade")) Then
                        Dim drpPropertyTypes As DropDownList = CType(objControl, DropDownList)
                        If (drpPropertyTypes.SelectedValue <> "-1") Then
                            propertyTypeIDs = PropertySettings.SEOPropertyTypeID & "=" & drpPropertyTypes.SelectedValue
                        End If
                    End If

                    If (objControl.ID.ToLower().EndsWith("wildcard")) Then
                        Dim txtWildcard As TextBox = CType(objControl, TextBox)
                        If (txtWildcard.Text.Trim() <> "") Then
                            customFieldIDs = "-1"
                            searchValues = BuildSearchValues(searchValues, txtWildcard.Text.Trim())
                        End If
                    End If

                    If (objControl.ID.ToLower().EndsWith("location")) Then
                        Dim txtLocation As TextBox = CType(objControl, TextBox)
                        If (txtLocation.Text.Trim() <> "") Then
                            location = "Location=" & Server.UrlEncode(txtLocation.Text.Trim())
                        End If
                    End If

                    If (IsNumeric(objControl.ID.Replace("From", "").Replace("-linkedtype", "").Replace("-linkeddropdown", ""))) Then
                        ' Numeric, must be a Custom Field ID

                        Dim ID As Integer = Convert.ToInt32(objControl.ID.Replace("From", "").Replace("To", "").Replace("-linkedtype", "").Replace("-linkeddropdown", ""))
                        Dim customFieldID As Integer = ID

                        For Each objCustomField As CustomFieldInfo In CustomFields
                            If (objCustomField.CustomFieldID = customFieldID) Then

                                Select Case objCustomField.FieldType

                                    Case CustomFieldType.OneLineTextBox

                                        Select Case objCustomField.SearchType

                                            Case SearchType.Default
                                                Dim objTextBox As TextBox = CType(objControl, TextBox)
                                                If (objTextBox.Text <> "") Then
                                                    customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                    searchValues = BuildSearchValues(searchValues, objTextBox.Text)
                                                End If

                                            Case SearchType.DropDown
                                                Dim objDropDown As DropDownList = CType(objControl, DropDownList)
                                                If (objDropDown.SelectedValue <> "-1") Then
                                                    customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                    searchValues = BuildSearchValues(searchValues, objDropDown.SelectedValue)
                                                End If

                                            Case SearchType.Range
                                                Dim fromValue As String = ""
                                                If (objCustomField.FieldElementsFrom <> "") Then
                                                    Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                                    If (objDropDownList.SelectedValue <> "-1") Then
                                                        fromValue = objDropDownList.SelectedValue
                                                    End If
                                                Else
                                                    Dim objTextBoxFrom As TextBox = CType(objControl, TextBox)
                                                    fromValue = objTextBoxFrom.Text
                                                End If

                                                Dim toValue As String = ""
                                                If (objCustomField.FieldElementsTo <> "") Then
                                                    Dim objDropDownList As DropDownList = CType(phProperty.FindControl("To" & customFieldID.ToString()), DropDownList)
                                                    If (objDropDownList.SelectedValue <> "-1") Then
                                                        toValue = objDropDownList.SelectedValue
                                                    End If
                                                Else
                                                    Dim objTextBoxTo As TextBox = CType(phProperty.FindControl("To" & customFieldID.ToString()), TextBox)
                                                    toValue = objTextBoxTo.Text
                                                End If

                                                Dim searchValue As String = "|"

                                                If (fromValue <> "") Then
                                                    searchValue = fromValue & searchValue
                                                End If

                                                If (toValue <> "") Then
                                                    searchValue = searchValue & toValue
                                                End If

                                                If (searchValue <> "|") Then
                                                    customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                    searchValues = BuildSearchValues(searchValues, searchValue)
                                                End If

                                        End Select

                                    Case CustomFieldType.MultiLineTextBox
                                        Dim objTextBox As TextBox = CType(objControl, TextBox)
                                        If (objTextBox.Text <> "") Then
                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                            searchValues = BuildSearchValues(searchValues, objTextBox.Text)
                                        End If

                                    Case CustomFieldType.RichTextBox
                                        Dim objTextBox As TextBox = CType(objControl, TextBox)
                                        If (objTextBox.Text <> "") Then
                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                            searchValues = BuildSearchValues(searchValues, objTextBox.Text)
                                        End If

                                    Case CustomFieldType.DropDownList

                                        If (objCustomField.SearchType = SearchType.Multiple) Then
                                            Dim objListBox As ListBox = CType(objControl, ListBox)
                                            Dim values As String = ""
                                            For Each objItem As ListItem In objListBox.Items
                                                If (objItem.Selected) Then
                                                    If (values = "") Then
                                                        values = objItem.Value
                                                    Else
                                                        values = values & "|" & objItem.Value
                                                    End If
                                                End If
                                            Next
                                            If (values <> "") Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, values)
                                            End If
                                        Else
                                            Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                            If (objDropDownList.SelectedValue <> "-1") Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, objDropDownList.SelectedValue.ToString())
                                            End If
                                        End If

                                    Case CustomFieldType.ListBox

                                        If (objCustomField.SearchType = SearchType.Default) Then
                                            Dim objListBox As ListBox = CType(objControl, ListBox)
                                            Dim values As String = ""
                                            For Each objItem As ListItem In objListBox.Items
                                                If (objItem.Selected) Then
                                                    If (values = "") Then
                                                        values = objItem.Value
                                                    Else
                                                        values = values & "|" & objItem.Value
                                                    End If
                                                End If
                                            Next
                                            If (values <> "") Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, values)
                                            End If
                                        Else
                                            Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                            If (objDropDownList.SelectedValue <> "-1") Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, objDropDownList.SelectedValue.ToString())
                                            End If
                                        End If

                                    Case CustomFieldType.CheckBox
                                        Dim objCheckBox As CheckBox = CType(objControl, CheckBox)
                                        If (objCheckBox.Checked) Then
                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                            searchValues = BuildSearchValues(searchValues, objCheckBox.Checked.ToString())
                                        End If

                                    Case CustomFieldType.MultiCheckBox
                                        If (objCustomField.SearchType = SearchType.DropDown) Then
                                            Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                            If (objDropDownList.SelectedValue <> "-1") Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, objDropDownList.SelectedValue)
                                            End If
                                        Else
                                            Dim objCheckBoxList As CheckBoxList = CType(objControl, CheckBoxList)
                                            Dim values As String = ""
                                            For Each objCheckBox As ListItem In objCheckBoxList.Items
                                                If (objCheckBox.Selected) Then
                                                    If (values = "") Then
                                                        values = objCheckBox.Value
                                                    Else
                                                        values = values & "|" & objCheckBox.Value
                                                    End If
                                                End If
                                            Next
                                            If (values <> "") Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, values)
                                            End If
                                        End If

                                    Case CustomFieldType.RadioButton

                                        If (objCustomField.SearchType = SearchType.Multiple) Then
                                            Dim objCheckBoxList As CheckBoxList = CType(objControl, CheckBoxList)
                                            Dim values As String = ""
                                            For Each objCheckBox As ListItem In objCheckBoxList.Items
                                                If (objCheckBox.Selected) Then
                                                    If (values = "") Then
                                                        values = objCheckBox.Value
                                                    Else
                                                        values = values & "|" & objCheckBox.Value
                                                    End If
                                                End If
                                            Next
                                            If (values <> "") Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, values)
                                            End If
                                        Else
                                            Dim objRadioButtonList As RadioButtonList = CType(objControl, RadioButtonList)
                                            If Not (objRadioButtonList.SelectedItem Is Nothing) Then
                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                searchValues = BuildSearchValues(searchValues, objRadioButtonList.SelectedValue)
                                            End If
                                        End If

                                End Select

                                Exit For
                            End If
                        Next

                    End If

                End If
            Next

            Dim objParams As New ArrayList()
            If (propertyTypeIDs <> "" And propertyAgentIDs = "" And propertyBrokerIDs = "" And customFieldIDs = "" And searchValues = "" And location = "") Then
                Response.Redirect(GetTypeLink(TabId, ModuleId, Convert.ToInt32(propertyTypeIDs.Split("="c)(1))), True)
            Else
                objParams.Add(PropertySettings.SEOAgentType & "=ViewSearch")
                If (customFieldIDs <> "" And searchValues <> "") Then
                    objParams.Add("CustomFieldIDs=" & customFieldIDs)
                    objParams.Add("SearchValues=" & searchValues)
                End If
                objParams.Add(propertyTypeIDs)
                objParams.Add(propertyAgentIDs)
                objParams.Add(propertyBrokerIDs)
                objParams.Add(location)
                Response.Redirect(NavigateURL(TabId, "", CType(objParams.ToArray(GetType(String)), String())), True)
            End If

        End Sub

        Private Sub dlFeatured_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objProperty As PropertyInfo = CType(e.Item.DataItem, PropertyInfo)
                LayoutController(objProperty).ProcessItem(e.Item.Controls, Me._objLayoutFeatured.Tokens, objProperty, Me.CustomFields)
            End If

        End Sub

        Private Sub rptFeatured_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objProperty As PropertyInfo = CType(e.Item.DataItem, PropertyInfo)
                LayoutController(objProperty).ProcessItem(e.Item.Controls, Me._objLayoutFeatured.Tokens, objProperty, Me.CustomFields)
            End If

        End Sub

        Private Sub dlTypes_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objType As PropertyTypeInfo = CType(e.Item.DataItem, PropertyTypeInfo)
                LayoutController.ProcessType(e.Item.Controls, Me._objLayoutType.Tokens, objType, Null.NullString)
            End If

        End Sub

        Private Sub rptTypes_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objType As PropertyTypeInfo = CType(e.Item.DataItem, PropertyTypeInfo)
                LayoutController.ProcessType(e.Item.Controls, Me._objLayoutType.Tokens, objType, Null.NullString)
            End If

        End Sub

        Protected Sub drpPropertyBrokers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Try
                Dim drpPropertyBrokers As DropDownList = CType(sender, DropDownList)

                For Each objControl As Control In phProperty.Controls
                    If (objControl.ID <> "") Then
                        If (objControl.ID.ToLower().EndsWith("agent")) Then

                            Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)
                            Dim arrAgents As ArrayList
                            If (drpPropertyBrokers.SelectedValue <> "-1") Then
                                arrAgents = objAgentController.ListSelected(PortalId, ModuleId, Convert.ToInt32(drpPropertyBrokers.SelectedValue))
                            Else
                                arrAgents = objAgentController.ListActive(PortalId, ModuleId)
                            End If

                            Dim drpPropertyAgent As DropDownList = CType(objControl, DropDownList)
                            drpPropertyAgent.DataSource = arrAgents
                            drpPropertyAgent.DataBind()

                            Dim selectText As String = GetResourceString("SelectAgent", "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty", PropertySettings)
                            drpPropertyAgent.Items.Insert(0, New ListItem(selectText, "-1"))

                        End If
                    End If
                Next

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(CType(Parent.Parent, PropertyAgentBase), exc)
            End Try

        End Sub

        Protected Sub drpPropertyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Try
                Dim drpPropertyTypes As DropDownList = CType(sender, DropDownList)

                For Each objControl As Control In phProperty.Controls
                    If (objControl.ID <> "") Then
                        If (objControl.ID.ToLower().EndsWith("linkedtype")) Then

                            Dim val As String = ""
                            If (CType(objControl, ListControl).SelectedValue <> "") Then
                                val = CType(objControl, ListControl).SelectedValue
                            End If

                            CType(objControl, ListControl).Items.Clear()

                            If (drpPropertyTypes.SelectedValue <> "-1") Then
                                For Each objCustomField As CustomFieldInfo In CustomFields
                                    If (objCustomField.CustomFieldID = CType(objControl, WebControl).Attributes("CustomFieldID")) Then
                                        Dim index As Integer = 0
                                        Dim objPropertyTypeController As New PropertyTypeController
                                        Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(Me.ModuleId, True, Me.PropertySettings.TypesSortBy, Null.NullString())

                                        For Each objType As PropertyTypeInfo In objTypes
                                            If (objType.PropertyTypeID.ToString() = drpPropertyTypes.SelectedValue) Then
                                                Exit For
                                            End If
                                            index = index + 1
                                        Next
                                        If (index < objCustomField.FieldElements.Split(vbCrLf).Length) Then
                                            Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(index).Split(Convert.ToChar("|"))
                                            For Each value As String In values
                                                If (value.Trim() <> "") Then
                                                    If (objCustomField.IncludeCount) Then
                                                        Dim objPropertyController As New PropertyController()
                                                        Dim count As Integer = 0
                                                        objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                        If (objCustomField.HideZeroCount) Then
                                                            If (count > 0) Then
                                                                CType(objControl, ListControl).Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                            End If
                                                        Else
                                                            CType(objControl, ListControl).Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                        End If
                                                    Else
                                                        If (objCustomField.HideZeroCount) Then
                                                            Dim objPropertyController As New PropertyController()
                                                            Dim count As Integer = 0
                                                            objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                            If (count > 0) Then
                                                                CType(objControl, ListControl).Items.Add(value.Replace(vbLf, ""))
                                                            End If
                                                        Else
                                                            CType(objControl, ListControl).Items.Add(value.Replace(vbLf, ""))
                                                        End If
                                                    End If
                                                End If
                                            Next
                                        End If

                                        If (objCustomField.SearchType <> SearchType.Multiple) Then
                                            Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                                            selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                                            CType(objControl, ListControl).Items.Insert(0, New ListItem(selectText, "-1"))
                                        End If

                                        If (CType(objControl, ListControl).Items.FindByValue(val) IsNot Nothing) Then
                                            CType(objControl, ListControl).SelectedValue = val
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If

                            Exit For

                        End If
                    End If
                Next

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub drpLinked_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Try

                Dim drpLinked As DropDownList = CType(sender, DropDownList)

                For Each objControl As Control In phProperty.Controls
                    If (objControl.ID <> "") Then
                        If (objControl.ID.ToLower().EndsWith("linkeddropdown")) Then

                            Dim val As String = ""
                            If (CType(objControl, ListControl).SelectedValue <> "") Then
                                val = CType(objControl, ListControl).SelectedValue
                            End If

                            CType(objControl, ListControl).Items.Clear()

                            If (drpLinked.SelectedValue <> "-1") Then
                                For Each objCustomField As CustomFieldInfo In CustomFields
                                    If (objCustomField.CustomFieldID = CType(objControl, WebControl).Attributes("CustomFieldID")) Then
                                        If ((drpLinked.SelectedIndex - 1) < objCustomField.FieldElements.Split(vbCrLf).Length) Then
                                            Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(drpLinked.SelectedIndex - 1).Split(Convert.ToChar("|"))
                                            For Each value As String In values
                                                If (value.Trim() <> "") Then
                                                    If (objCustomField.IncludeCount) Then
                                                        Dim objPropertyController As New PropertyController()
                                                        Dim count As Integer = 0
                                                        objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                        If (objCustomField.HideZeroCount) Then
                                                            If (count > 0) Then
                                                                CType(objControl, ListControl).Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                            End If
                                                        Else
                                                            CType(objControl, ListControl).Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                        End If
                                                    Else
                                                        If (objCustomField.HideZeroCount) Then
                                                            Dim objPropertyController As New PropertyController()
                                                            Dim count As Integer = 0
                                                            objPropertyController.List(Me.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                            If (count > 0) Then
                                                                CType(objControl, ListControl).Items.Add(value.Replace(vbLf, ""))
                                                            End If
                                                        Else
                                                            CType(objControl, ListControl).Items.Add(value.Replace(vbLf, ""))
                                                        End If
                                                    End If
                                                End If
                                            Next
                                        End If

                                        If (objCustomField.SearchType <> SearchType.Multiple) Then
                                            Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                                            selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                                            CType(objControl, ListControl).Items.Insert(0, New ListItem(selectText, "-1"))
                                        End If

                                        If (CType(objControl, ListControl).Items.FindByValue(val) IsNot Nothing) Then
                                            CType(objControl, ListControl).SelectedValue = val
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If

                            Exit For

                        End If
                    End If
                Next

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace