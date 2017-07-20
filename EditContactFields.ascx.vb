Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditContactFields
        Inherits PropertyAgentBase

#Region " Private Members "

        Dim _currentPage As Integer = 1
        Dim _pageRecords As Integer = Null.NullInteger
        Dim _ContactFields As List(Of ContactFieldInfo)

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

            If (Request.Cookies("PA-RFM-PageSize") IsNot Nothing) Then
                If Not (drpRecordsPerPage.Items.FindByValue(Request.Cookies("PA-RFM-PageSize").Value) Is Nothing) Then
                    drpRecordsPerPage.SelectedValue = Request.Cookies("PA-RFM-PageSize").Value
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
            objCustomFieldManager.Caption = Localization.GetString("ContactFieldManager", LocalResourceFile)
            objCustomFieldManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields")
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

        Private Sub BindContactFields()

            Dim PageSize As Integer = Convert.ToInt32(drpRecordsPerPage.SelectedItem.Value)

            Dim objContactFieldController As New ContactFieldController

            Localization.LocalizeDataGrid(grdContactFields, Me.LocalResourceFile)

            _ContactFields = objContactFieldController.List(Me.ModuleId)

            If (_ContactFields.Count > 1) Then
                ' Sort Order must be messed up, fix now..
                For i As Integer = 0 To _ContactFields.Count - 1
                    Dim objContactField As ContactFieldInfo = CType(_ContactFields(i), ContactFieldInfo)
                    objContactField.SortOrder = i
                    objContactFieldController.Update(objContactField)
                Next
            End If

            Dim objPagedDataSource As New PagedDataSource

            objPagedDataSource.AllowPaging = True
            objPagedDataSource.DataSource = _ContactFields
            objPagedDataSource.PageSize = PageSize
            objPagedDataSource.CurrentPageIndex = _currentPage - 1

            If (_ContactFields.Count > 0) Then

                grdContactFields.DataSource = objPagedDataSource
                grdContactFields.DataBind()

                grdContactFields.Visible = True
                lblNoContactFields.Visible = False
                ctlPagingControl.Visible = True

                ctlPagingControl.Visible = True
                ctlPagingControl.TotalRecords = _ContactFields.Count
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = _currentPage
                ctlPagingControl.QuerystringParams = PropertySettings.SEOAgentType & "=EditContactFields"

                If (drpRecordsPerPage.SelectedIndex <> 1) Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&PageRecords=" + drpRecordsPerPage.SelectedValue
                End If

                ctlPagingControl.TabID = TabId
            Else
                grdContactFields.Visible = False
                lblNoContactFields.Visible = True
                ctlPagingControl.Visible = False
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetContactFieldEditUrl(ByVal ContactFieldID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactField", "ContactFieldID=" & ContactFieldID)

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
                    BindContactFields()
                End If

                cmdAddContactField.CssClass = PropertySettings.ButtonClass
                cmdReturnToModule.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpRecordsPerPage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRecordsPerPage.SelectedIndexChanged

            Try

                _currentPage = 1
                Response.Cookies.Add(New HttpCookie("PA-RFM-PageSize", drpRecordsPerPage.SelectedValue))
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields", "PageRecords=" & drpRecordsPerPage.SelectedValue), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddCustomField_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddContactField.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactField"), True)

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

        Private Sub grdContactFields_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdContactFields.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim btnUp As ImageButton = CType(e.Item.FindControl("btnUp"), ImageButton)
                Dim btnDown As ImageButton = CType(e.Item.FindControl("btnDown"), ImageButton)

                Dim objContactField As ContactFieldInfo = CType(e.Item.DataItem, ContactFieldInfo)

                If Not (btnUp Is Nothing And btnDown Is Nothing) Then

                    If (objContactField.ContactFieldID = CType(_ContactFields(0), ContactFieldInfo).ContactFieldID) Then
                        btnUp.Visible = False
                    End If

                    If (objContactField.ContactFieldID = CType(_ContactFields(_ContactFields.Count - 1), ContactFieldInfo).ContactFieldID) Then
                        btnDown.Visible = False
                    End If

                    btnUp.CommandArgument = objContactField.ContactFieldID.ToString()
                    btnUp.CommandName = "Up"

                    btnDown.CommandArgument = objContactField.ContactFieldID.ToString()
                    btnDown.CommandName = "Down"

                End If

            End If

        End Sub

        Private Sub grdContactFields_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdContactFields.ItemCommand

            Dim objContactFieldController As New ContactFieldController
            _ContactFields = objContactFieldController.List(Me.ModuleId)

            Dim ContactFieldID As Integer = Convert.ToInt32(e.CommandArgument)

            For i As Integer = 0 To _ContactFields.Count - 1

                Dim objContactField As ContactFieldInfo = CType(_ContactFields(i), ContactFieldInfo)

                If (ContactFieldID = objContactField.ContactFieldID) Then
                    If (e.CommandName = "Up") Then

                        Dim objContactFieldToSwap As ContactFieldInfo = CType(_ContactFields(i - 1), ContactFieldInfo)

                        Dim sortOrder As Integer = objContactField.SortOrder
                        Dim sortOrderPrevious As Integer = objContactFieldToSwap.SortOrder

                        objContactField.SortOrder = sortOrderPrevious
                        objContactFieldToSwap.SortOrder = sortOrder

                        objContactFieldController.Update(objContactField)
                        objContactFieldController.Update(objContactFieldToSwap)

                    End If


                    If (e.CommandName = "Down") Then

                        Dim objContactFieldToSwap As ContactFieldInfo = CType(_ContactFields(i + 1), ContactFieldInfo)

                        Dim sortOrder As Integer = objContactField.SortOrder
                        Dim sortOrderNext As Integer = objContactFieldToSwap.SortOrder

                        objContactField.SortOrder = sortOrderNext
                        objContactFieldToSwap.SortOrder = sortOrder

                        objContactFieldController.Update(objContactField)
                        objContactFieldController.Update(objContactFieldToSwap)

                    End If

                End If

            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace