Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports System.IO

Namespace Ventrian.PropertyAgent

    Partial Public Class EditProperties
        Inherits PropertyAgentBase

#Region " Private Members "

        Dim _currentPage As Integer = 1
        Dim _pageRecords As Integer = Null.NullInteger

        Dim _propertyTypeID As Integer = Null.NullInteger
        Dim _status As String = "-1"

        Dim _sortBy As String = ""
        Dim _sortDirection As String = ""

        Dim _customValue As String = ""
        Dim _customField As String = ""

#End Region

#Region " Private Methods "

        Private Sub CheckSecurity()

            If (Request.IsAuthenticated = False) Then
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=AccessDenied"), True)
            End If

            If (IsEditable = False And PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = False And PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = False And PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False) Then
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=AccessDenied"), True)
            End If

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("CurrentPage") Is Nothing) Then
                _currentPage = Convert.ToInt32(Request("CurrentPage"))
            End If

            If Not (Request("PageRecords") Is Nothing) Then
                _pageRecords = Convert.ToInt32(Request("PageRecords"))
            End If

            Dim propertyTypeIDParam As String = PropertySettings.SEOPropertyTypeID
            If (Request(propertyTypeIDParam) = "") Then
                propertyTypeIDParam = "PropertyType"
            End If
            If Not (Request(propertyTypeIDParam) Is Nothing) Then
                _propertyTypeID = Convert.ToInt32(Request(propertyTypeIDParam))
            End If

            If Not (Request("Status") Is Nothing) Then
                _status = Request("Status")
            End If

            If Not (Request("SortBy") Is Nothing) Then
                _sortBy = Request("SortBy")
            End If

            If Not (Request("SortDirection") Is Nothing) Then
                _sortDirection = Request("SortDirection")
            End If

            If Not (Request("CustomValue") Is Nothing) Then
                _customValue = Request("CustomValue")
            End If

            If Not (Request("CustomField") Is Nothing) Then
                _customField = Request("CustomField")
            End If

        End Sub

        Private Sub BindPageCount()

            If (_pageRecords <> Null.NullInteger) Then
                If Not (drpRecordsPerPage.Items.FindByValue(_pageRecords.ToString()) Is Nothing) Then
                    drpRecordsPerPage.SelectedValue = _pageRecords.ToString()
                Else
                    If Not (drpRecordsPerPage.Items.FindByValue(Me.PropertySettings.PropertyManagerItemsPerPage.ToString()) Is Nothing) Then
                        drpRecordsPerPage.SelectedValue = Me.PropertySettings.PropertyManagerItemsPerPage.ToString()
                    End If
                End If
            Else
                If Not (drpRecordsPerPage.Items.FindByValue(Me.PropertySettings.PropertyManagerItemsPerPage.ToString()) Is Nothing) Then
                    drpRecordsPerPage.SelectedValue = Me.PropertySettings.PropertyManagerItemsPerPage.ToString()
                End If
            End If

        End Sub

        Private Sub BindCustomFields()

            drpCustomFields.DataSource = CustomFields
            drpCustomFields.DataBind()

            drpCustomFields.Items.Insert(0, New ListItem(GetResourceString("SelectCustomField"), "-1"))

            If (_customValue <> "") Then
                txtCustomField.Text = _customValue
            End If

            If (_customField <> "") Then
                If Not (drpCustomFields.Items.FindByValue(_customField) Is Nothing) Then
                    drpCustomFields.SelectedValue = _customField
                End If
            End If

        End Sub

        Private Sub BindTypes()

            Dim objPropertyTypeController As New PropertyTypeController

            drpTypes.DataSource = objPropertyTypeController.ListAll(Me.ModuleId, True, PropertySettings.TypesSortBy, Null.NullString())
            drpTypes.DataBind()

            drpTypes.Items.Insert(0, New ListItem(GetResourceString("SelectType"), "-1"))

            If (_propertyTypeID <> Null.NullInteger) Then
                If Not (drpTypes.Items.FindByValue(_propertyTypeID.ToString()) Is Nothing) Then
                    drpTypes.SelectedValue = _propertyTypeID.ToString()
                End If
            End If

        End Sub

        Private Sub BindStatus()

            For Each value As Integer In System.Enum.GetValues(GetType(SearchStatusType))
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SearchStatusType), value), Me.LocalResourceFile)
                If (li.Value = "-1") Then
                    drpStatus.Items.Insert(0, li)
                Else
                    drpStatus.Items.Add(li)
                End If
            Next

            ' drpStatus.Items.Insert(0, New ListItem(Localization.GetString("AnyStatus", Me.LocalResourceFile), "-1"))

            If (_status <> "-1") Then
                If Not (drpStatus.Items.FindByValue(_status) Is Nothing) Then
                    drpStatus.SelectedValue = _status
                End If
            End If

        End Sub

        Private Sub BindSortBy()

            For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)

                If (objSortByType = SortByType.CustomField) Then
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

                Else
                    Dim li As New ListItem
                    li.Value = System.Enum.GetName(GetType(SortByType), value)
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                    drpSortBy.Items.Add(li)
                End If
            Next

            If (_sortBy <> "") Then
                Dim objSortBy As SortByType = Me.PropertySettings.PropertyManagerSortBy

                If (_sortBy.StartsWith("cf")) Then
                    objSortBy = SortByType.CustomField
                    If Not (drpSortBy.Items.FindByValue(_sortBy) Is Nothing) Then
                        drpSortBy.SelectedValue = _sortBy
                    End If
                Else
                    objSortBy = CType(System.Enum.Parse(GetType(SortByType), _sortBy), SortByType)
                    If Not (drpSortBy.Items.FindByValue(objSortBy.ToString) Is Nothing) Then
                        drpSortBy.SelectedValue = objSortBy.ToString
                    End If
                End If
            Else
                If (Me.PropertySettings.PropertyManagerSortBy <> SortByType.CustomField) Then
                    If Not (drpSortBy.Items.FindByValue(Me.PropertySettings.PropertyManagerSortBy.ToString()) Is Nothing) Then
                        drpSortBy.SelectedValue = Me.PropertySettings.PropertyManagerSortBy.ToString()
                    End If
                Else
                    If Not (drpSortBy.Items.FindByValue("cf" & Me.PropertySettings.PropertyManagerSortByCustomField.ToString()) Is Nothing) Then
                        drpSortBy.SelectedValue = "cf" & Me.PropertySettings.PropertyManagerSortByCustomField.ToString()
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

            If Not (drpSortDirection.Items.FindByValue(_sortDirection) Is Nothing) Then
                drpSortDirection.SelectedValue = _sortDirection
            End If

            If (_sortDirection <> "") Then
                Dim objSortDirection As SortDirectionType = CType(System.Enum.Parse(GetType(SortDirectionType), _sortDirection), SortDirectionType)
                If Not (drpSortDirection.Items.FindByValue(objSortDirection.ToString()) Is Nothing) Then
                    drpSortDirection.SelectedValue = objSortDirection.ToString()
                Else
                    If Not (drpSortDirection.Items.FindByValue(Me.PropertySettings.PropertyManagerSortDirection.ToString()) Is Nothing) Then
                        drpSortDirection.SelectedValue = Me.PropertySettings.PropertyManagerSortDirection.ToString()
                    End If
                End If
            Else
                If Not (drpSortDirection.Items.FindByValue(Me.PropertySettings.PropertyManagerSortDirection.ToString()) Is Nothing) Then
                    drpSortDirection.SelectedValue = Me.PropertySettings.PropertyManagerSortDirection.ToString()
                End If
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCrumbPropertyManager As New CrumbInfo
            objCrumbPropertyManager.Caption = GetResourceString("PropertyManager")
            objCrumbPropertyManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager")
            crumbs.Add(objCrumbPropertyManager)

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

        End Sub

        Private Sub BindProperties()

            Dim PageSize As Integer = Convert.ToInt32(drpRecordsPerPage.SelectedItem.Value)

            Dim objPropertyController As New PropertyController

            If (Page.IsPostBack = False) Then
                Localization.LocalizeDataGrid(grdProperties, Me.LocalResourceFile)
            End If

            Dim objProperties As List(Of PropertyInfo)

            Dim objSortByType As SortByType = SortByType.CustomField
            Dim sortByID As Integer = Null.NullInteger
            If (drpSortBy.SelectedValue.StartsWith("cf")) Then
                sortByID = Convert.ToInt32(drpSortBy.SelectedValue.Replace("cf", ""))
            Else
                objSortByType = CType(System.Enum.Parse(GetType(SortByType), drpSortBy.SelectedValue.ToString()), SortByType)
            End If

            Dim customFieldIDs As String = ""
            Dim searchValues As String = ""
            If (txtCustomField.Text <> "" And drpCustomFields.SelectedValue <> "-1") Then
                searchValues = txtCustomField.Text
                customFieldIDs = drpCustomFields.SelectedValue
            End If

            Dim objSortDirection As SortDirectionType = CType(System.Enum.Parse(GetType(SortDirectionType), drpSortDirection.SelectedValue.ToString()), SortDirectionType)

            Dim authorID As Integer = UserId
            If (IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = True) Then
                authorID = Null.NullInteger
            End If
            Dim brokerID As Integer = Null.NullInteger
            If (authorID <> Null.NullInteger And PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) Then
                brokerID = UserId
                authorID = Null.NullInteger
            End If

            Dim totalRecords As Integer = 0
            Dim OnlyForAuthenticated As Boolean = Null.NullBoolean
            If Me.UserId = -1 Then OnlyForAuthenticated = True
            Dim objStatusType As SearchStatusType = CType(System.Enum.Parse(GetType(SearchStatusType), drpStatus.SelectedValue), SearchStatusType)
            objProperties = objPropertyController.List(Me.ModuleId, Convert.ToInt32(drpTypes.SelectedValue), objStatusType, authorID, brokerID, Null.NullBoolean, OnlyForAuthenticated, objSortByType, sortByID, objSortDirection, customFieldIDs, searchValues, _currentPage - 1, PageSize, totalRecords, False, True, Null.NullInteger, Null.NullInteger)

            If (objProperties.Count > 0) Then

                grdProperties.Columns(grdProperties.Columns.Count - 2).Visible = (IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) Or PortalSecurity.IsInRoles(PropertySettings.PermissionAutoApprove))
                grdProperties.Columns(grdProperties.Columns.Count - 1).Visible = (IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionDelete))

                grdProperties.DataSource = objProperties
                grdProperties.DataBind()

                grdProperties.Visible = True
                lblNoProperties.Visible = False

                ctlPagingControl.Visible = True
                ctlPagingControl.TotalRecords = totalRecords
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = _currentPage
                ctlPagingControl.QuerystringParams = PropertySettings.SEOAgentType & "=PropertyManager"

                If (drpRecordsPerPage.SelectedValue <> Me.PropertySettings.PropertyManagerItemsPerPage.ToString()) Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&PageRecords=" + drpRecordsPerPage.SelectedValue
                End If

                If (drpTypes.SelectedIndex <> 0) Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&PropertyType=" + drpTypes.SelectedValue
                End If

                If (drpStatus.SelectedIndex <> 0) Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&Status=" + drpStatus.SelectedValue
                End If

                ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&SortBy=" + drpSortBy.SelectedValue
                ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&SortDirection=" + drpSortDirection.SelectedValue

                If (txtCustomField.Text <> "" And drpCustomFields.SelectedValue <> "-1") Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&CustomValue=" + txtCustomField.Text
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&CustomField=" + drpCustomFields.SelectedValue
                End If

                ctlPagingControl.TabID = TabId

                If (totalRecords <= PageSize) Then
                    ctlPagingControl.Visible = False
                End If
            Else
                grdProperties.Visible = False
                lblNoProperties.Visible = True
                ctlPagingControl.Visible = False
            End If

        End Sub

        Private Sub AddGridColumns()

            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleId, True)

            Dim addCount As Integer = 0
            For i As Integer = 0 To objCustomFields.Count - 1
                Dim objCustomField As CustomFieldInfo = CType(objCustomFields(i), CustomFieldInfo)
                If (objCustomField.IsInManager) Then
                    Dim objTemplateColumn As New TemplateColumn
                    objTemplateColumn.HeaderStyle.CssClass = "NormalBold"
                    objTemplateColumn.ItemStyle.CssClass = "Normal"
                    objTemplateColumn.HeaderTemplate = New LayoutTemplate(ListItemType.Header, objCustomField.CustomFieldID.ToString(), objCustomField.Caption)
                    objTemplateColumn.ItemTemplate = New LayoutTemplate(ListItemType.Item, objCustomField.CustomFieldID.ToString(), objCustomField.Caption)
                    grdProperties.Columns.AddAt(1 + addCount, objTemplateColumn)
                    addCount = addCount + 1
                End If
            Next
            grdProperties.Columns(1 + addCount).Visible = (Not Me.PropertySettings.HideTypes)

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetPropertyEditUrl(ByVal propertyID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditProperty", PropertySettings.SEOPropertyID & "=" & propertyID)

        End Function

        Protected Function GetLocalizedStatus(ByVal obj As Object) As String

            Dim objProperty As PropertyInfo = CType(obj, PropertyInfo)

            If Not (objProperty Is Nothing) Then
                Select Case objProperty.Status

                    Case StatusType.Published
                        If (objProperty.DatePublished > DateTime.Now) Then
                            Return Localization.GetString("PublishedPending", Me.LocalResourceFile)
                        End If

                        If (objProperty.DateExpired <> Null.NullDate And objProperty.DateExpired < DateTime.Now) Then
                            Return Localization.GetString("PublishedExpired", Me.LocalResourceFile)
                        End If

                        Return Localization.GetString(objProperty.Status.ToString(), Me.LocalResourceFile)

                    Case Else
                        Return Localization.GetString(objProperty.Status.ToString(), Me.LocalResourceFile)

                End Select
            End If

            Return ""

        End Function

        Protected Function GetPropertyValue(ByVal obj As Object, ByVal field As String) As String

            Dim objProperty As PropertyInfo = CType(obj, PropertyInfo)

            If Not (objProperty Is Nothing) Then
                Return objProperty.PropertyList(Convert.ToInt32(field)).ToString()
            End If

            Return ""

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                AddGridColumns()
                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                CheckSecurity()
                ReadQueryString()

                If (Page.IsPostBack = False) Then
                    Dim limit As Integer = GetLimit(PropertySettings.PermissionSubmit, PropertySettings.PermissionLimit)

                    If (limit <> Null.NullInteger) Then
                        Dim message As String = GetResourceString("Limit")
                        If (message.IndexOf("{0}") <> -1) Then
                            lblLimit.Text = String.Format(message, limit.ToString())
                        Else
                            lblLimit.Text = message
                        End If
                    Else
                        lblLimit.Visible = False
                    End If

                    BindPageCount()
                    BindCustomFields()
                    BindTypes()
                    BindStatus()
                    BindSortBy()
                    BindSortDirection()
                    BindProperties()
                End If

                cmdSearch.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdProperties_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdProperties.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objProperty As PropertyInfo = CType(e.Item.DataItem, PropertyInfo)

                Dim btnApproved As ImageButton = CType(e.Item.FindControl("btnApproved"), ImageButton)

                If Not (btnApproved Is Nothing) Then
                    If (objProperty.Status = StatusType.Published) Then
                        btnApproved.ImageUrl = "~/images/checked.gif"
                        btnApproved.CommandName = "ApprovedChecked"
                    Else
                        btnApproved.ImageUrl = "~/images/cancel.gif"
                        btnApproved.CommandName = "ApprovedCancel"
                    End If

                    btnApproved.CommandArgument = objProperty.PropertyID.ToString()
                End If

                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)

                If Not (btnDelete Is Nothing) Then
                    btnDelete.Attributes.Add("onClick", "javascript:return confirm('" & GetResourceString("Confirmation", LocalResourceFile, Me.PropertySettings) & "');")
                    btnDelete.CommandName = "Delete"
                    btnDelete.CommandArgument = objProperty.PropertyID.ToString()
                End If

            End If

        End Sub

        Private Sub grdProperties_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdProperties.ItemCommand

            Dim objPropertyController As New PropertyController
            Dim objProperty As PropertyInfo = objPropertyController.Get(Convert.ToInt32(e.CommandArgument))

            If (e.CommandName = "ApprovedChecked" Or e.CommandName = "ApprovedCancel") Then
                If (e.CommandName = "ApprovedChecked") Then
                    objProperty.Status = StatusType.Draft
                    objProperty.DatePublished = Null.NullDate
                    objPropertyController.Update(objProperty)
                End If

                If (e.CommandName = "ApprovedCancel") Then
                    objProperty.Status = StatusType.Published
                    objProperty.DatePublished = DateTime.Now
                    objPropertyController.Update(objProperty)
                End If
            End If

            If (e.CommandName = "Delete") Then

                Dim objPhotoController As New PhotoController
                Dim objPhotos As ArrayList = objPhotoController.List(objProperty.PropertyID)

                For Each objPhoto As PhotoInfo In objPhotos
                    If (File.Exists(Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent/" & Me.ModuleId & "/Images/" & objPhoto.Filename)) Then
                        File.Delete(Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent/" & Me.ModuleId & "/Images/" & objPhoto.Filename)
                    End If
                Next

                objPhotoController.DeleteByPropertyID(objProperty.PropertyID)
                PropertyTypeController.RemoveCache(Me.ModuleId)
                objPropertyController.Delete(objProperty.PropertyID)

            End If

            BindProperties()

        End Sub

        Protected Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

            _currentPage = 1
            BindProperties()

        End Sub

#End Region

    End Class

End Namespace
