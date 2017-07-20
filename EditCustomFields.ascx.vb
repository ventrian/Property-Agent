Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditCustomFields
        Inherits PropertyAgentBase

#Region " Private Members "

        Dim _currentPage As Integer = 1
        Dim _pageRecords As Integer = Null.NullInteger
        Dim _customFields As List(Of CustomFieldInfo)

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("CurrentPage") Is Nothing) Then
                _currentPage = Convert.ToInt32(Request("CurrentPage"))
            End If

            If Not (Request("PageRecords") Is Nothing) Then
                _pageRecords = Convert.ToInt32(Request("PageRecords"))
            End If

        End Sub

        Private Sub BindPageCount()

            If (Request.Cookies("PA-CFM-PageSize") IsNot Nothing) Then
                If Not (drpRecordsPerPage.Items.FindByValue(Request.Cookies("PA-CFM-PageSize").Value) Is Nothing) Then
                    drpRecordsPerPage.SelectedValue = Request.Cookies("PA-CFM-PageSize").Value
                    Return
                End If
            End If

            If (_pageRecords <> Null.NullInteger) Then
                If Not (drpRecordsPerPage.Items.FindByValue(_pageRecords.ToString()) Is Nothing) Then
                    drpRecordsPerPage.SelectedValue = _pageRecords.ToString()
                End If
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCustomFieldManager As New CrumbInfo
            objCustomFieldManager.Caption = Localization.GetString("CustomFieldManager", LocalResourceFile)
            objCustomFieldManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields")
            crumbs.Add(objCustomFieldManager)

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

        Private Sub BindCustomFields()

            Dim PageSize As Integer = Convert.ToInt32(drpRecordsPerPage.SelectedItem.Value)

            Dim objCustomFieldController As New CustomFieldController

            Localization.LocalizeDataGrid(grdCustomFields, Me.LocalResourceFile)

            _customFields = objCustomFieldController.List(Me.ModuleId, Null.NullBoolean)

            If (_customFields.Count > 1) Then
                ' Sort Order must be messed up, fix now..
                For i As Integer = 0 To _customFields.Count - 1
                    Dim objCustomField As CustomFieldInfo = CType(_customFields(i), CustomFieldInfo)
                    objCustomField.SortOrder = i
                    objCustomFieldController.Update(objCustomField)
                Next
            End If

            Dim objPagedDataSource As New PagedDataSource

            objPagedDataSource.AllowPaging = True
            objPagedDataSource.DataSource = _customFields
            objPagedDataSource.PageSize = PageSize
            objPagedDataSource.CurrentPageIndex = _currentPage - 1

            If (_customFields.Count > 0) Then

                grdCustomFields.DataSource = objPagedDataSource
                grdCustomFields.DataBind()

                grdCustomFields.Visible = True
                lblNoCustomFields.Visible = False
                ctlPagingControl.Visible = True

                ctlPagingControl.Visible = True
                ctlPagingControl.TotalRecords = _customFields.Count
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = _currentPage
                ctlPagingControl.QuerystringParams = PropertySettings.SEOAgentType & "=EditCustomFields"

                If (drpRecordsPerPage.SelectedIndex <> 1) Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&PageRecords=" + drpRecordsPerPage.SelectedValue
                End If

                ctlPagingControl.TabID = TabId
            Else
                grdCustomFields.Visible = False
                lblNoCustomFields.Visible = True
                ctlPagingControl.Visible = False
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetCustomFieldEditUrl(ByVal customFieldID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomField", "customFieldID=" & customFieldID)

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()

                If (Page.IsPostBack = False) Then
                    BindPageCount()
                    BindCustomFields()
                End If

                cmdAddCustomField.CssClass = PropertySettings.ButtonClass
                cmdReturnToModule.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpRecordsPerPage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRecordsPerPage.SelectedIndexChanged

            Try

                _currentPage = 1
                Response.Cookies.Add(New HttpCookie("PA-CFM-PageSize", drpRecordsPerPage.SelectedValue))
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields", "PageRecords=" & drpRecordsPerPage.SelectedValue), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddCustomField_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddCustomField.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomField"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdReturnToModule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturnToModule.Click

            Try

                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdCustomFields_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCustomFields.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim btnUp As ImageButton = CType(e.Item.FindControl("btnUp"), ImageButton)
                Dim btnDown As ImageButton = CType(e.Item.FindControl("btnDown"), ImageButton)

                Dim objCustomField As CustomFieldInfo = CType(e.Item.DataItem, CustomFieldInfo)

                If Not (btnUp Is Nothing And btnDown Is Nothing) Then

                    If (objCustomField.CustomFieldID = CType(_customFields(0), CustomFieldInfo).CustomFieldID) Then
                        btnUp.Visible = False
                    End If

                    If (objCustomField.CustomFieldID = CType(_customFields(_customFields.Count - 1), CustomFieldInfo).CustomFieldID) Then
                        btnDown.Visible = False
                    End If

                    btnUp.CommandArgument = objCustomField.CustomFieldID.ToString()
                    btnUp.CommandName = "Up"

                    btnDown.CommandArgument = objCustomField.CustomFieldID.ToString()
                    btnDown.CommandName = "Down"

                End If

                Dim btnPublished As ImageButton = CType(e.Item.FindControl("btnPublished"), ImageButton)

                If Not (btnPublished Is Nothing) Then
                    If (objCustomField.IsPublished) Then
                        btnPublished.ImageUrl = "~/images/checked.gif"
                        btnPublished.CommandName = "PublishedChecked"
                    Else
                        btnPublished.ImageUrl = "~/images/cancel.gif"
                        btnPublished.CommandName = "PublishedCancel"
                    End If

                    btnPublished.CommandArgument = objCustomField.CustomFieldID.ToString()
                End If


                Dim btnFeatured As ImageButton = CType(e.Item.FindControl("btnFeatured"), ImageButton)

                If Not (btnFeatured Is Nothing) Then
                    If (objCustomField.IsFeatured) Then
                        btnFeatured.ImageUrl = "~/images/checked.gif"
                        btnFeatured.CommandName = "FeaturedChecked"
                    Else
                        btnFeatured.ImageUrl = "~/images/cancel.gif"
                        btnFeatured.CommandName = "FeaturedCancel"
                    End If

                    btnFeatured.CommandArgument = objCustomField.CustomFieldID.ToString()
                End If

                Dim btnLockDown As ImageButton = CType(e.Item.FindControl("btnLockDown"), ImageButton)

                If Not (btnLockDown Is Nothing) Then
                    If (objCustomField.IsLockDown) Then
                        btnLockDown.ImageUrl = "~/images/checked.gif"
                        btnLockDown.CommandName = "LockDownChecked"
                    Else
                        btnLockDown.ImageUrl = "~/images/cancel.gif"
                        btnLockDown.CommandName = "LockDownCancel"
                    End If

                    btnLockDown.CommandArgument = objCustomField.CustomFieldID.ToString()
                End If

            End If

        End Sub

        Private Sub grdCustomFields_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdCustomFields.ItemCommand

            Dim objCustomFieldController As New CustomFieldController
            _customFields = objCustomFieldController.List(Me.ModuleId, Null.NullBoolean)

            Dim customFieldID As Integer = Convert.ToInt32(e.CommandArgument)

            For i As Integer = 0 To _customFields.Count - 1

                Dim objCustomField As CustomFieldInfo = CType(_customFields(i), CustomFieldInfo)

                If (customFieldID = objCustomField.CustomFieldID) Then
                    If (e.CommandName = "Up") Then

                        Dim objCustomFieldToSwap As CustomFieldInfo = CType(_customFields(i - 1), CustomFieldInfo)

                        Dim sortOrder As Integer = objCustomField.SortOrder
                        Dim sortOrderPrevious As Integer = objCustomFieldToSwap.SortOrder

                        objCustomField.SortOrder = sortOrderPrevious
                        objCustomFieldToSwap.SortOrder = sortOrder

                        objCustomFieldController.Update(objCustomField)
                        objCustomFieldController.Update(objCustomFieldToSwap)

                    End If


                    If (e.CommandName = "Down") Then

                        Dim objCustomFieldToSwap As CustomFieldInfo = CType(_customFields(i + 1), CustomFieldInfo)

                        Dim sortOrder As Integer = objCustomField.SortOrder
                        Dim sortOrderNext As Integer = objCustomFieldToSwap.SortOrder

                        objCustomField.SortOrder = sortOrderNext
                        objCustomFieldToSwap.SortOrder = sortOrder

                        objCustomFieldController.Update(objCustomField)
                        objCustomFieldController.Update(objCustomFieldToSwap)

                    End If

                    If (e.CommandName = "PublishedChecked") Then
                        objCustomField.IsPublished = False
                        objCustomFieldController.Update(objCustomField)
                    End If

                    If (e.CommandName = "PublishedCancel") Then
                        objCustomField.IsPublished = True
                        objCustomFieldController.Update(objCustomField)
                    End If

                    If (e.CommandName = "FeaturedChecked") Then
                        objCustomField.IsFeatured = False
                        objCustomFieldController.Update(objCustomField)
                    End If

                    If (e.CommandName = "FeaturedCancel") Then
                        objCustomField.IsFeatured = True
                        objCustomFieldController.Update(objCustomField)
                    End If

                    If (e.CommandName = "LockDownChecked") Then
                        objCustomField.IsLockDown = False
                        objCustomFieldController.Update(objCustomField)
                    End If

                    If (e.CommandName = "LockDownCancel") Then
                        objCustomField.IsLockDown = True
                        objCustomFieldController.Update(objCustomField)
                    End If

                End If

            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace