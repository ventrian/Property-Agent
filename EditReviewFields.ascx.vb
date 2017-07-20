Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditReviewFields
        Inherits PropertyAgentBase

#Region " Private Members "

        Dim _currentPage As Integer = 1
        Dim _pageRecords As Integer = Null.NullInteger
        Dim _reviewFields As List(Of ReviewFieldInfo)

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
            objCustomFieldManager.Caption = Localization.GetString("ReviewFieldManager", LocalResourceFile)
            objCustomFieldManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields")
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

        Private Sub BindReviewFields()

            Dim PageSize As Integer = Convert.ToInt32(drpRecordsPerPage.SelectedItem.Value)

            Dim objReviewFieldController As New ReviewFieldController

            Localization.LocalizeDataGrid(grdReviewFields, Me.LocalResourceFile)

            _reviewFields = objReviewFieldController.List(Me.ModuleId)

            If (_reviewFields.Count > 1) Then
                ' Sort Order must be messed up, fix now..
                For i As Integer = 0 To _reviewFields.Count - 1
                    Dim objReviewField As ReviewFieldInfo = CType(_reviewFields(i), ReviewFieldInfo)
                    objReviewField.SortOrder = i
                    objReviewFieldController.Update(objReviewField)
                Next
            End If

            Dim objPagedDataSource As New PagedDataSource

            objPagedDataSource.AllowPaging = True
            objPagedDataSource.DataSource = _reviewFields
            objPagedDataSource.PageSize = PageSize
            objPagedDataSource.CurrentPageIndex = _currentPage - 1

            If (_reviewFields.Count > 0) Then

                grdReviewFields.DataSource = objPagedDataSource
                grdReviewFields.DataBind()

                grdReviewFields.Visible = True
                lblNoReviewFields.Visible = False
                ctlPagingControl.Visible = True

                ctlPagingControl.Visible = True
                ctlPagingControl.TotalRecords = _reviewFields.Count
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = _currentPage
                ctlPagingControl.QuerystringParams = PropertySettings.SEOAgentType & "=EditReviewFields"

                If (drpRecordsPerPage.SelectedIndex <> 1) Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&PageRecords=" + drpRecordsPerPage.SelectedValue
                End If

                ctlPagingControl.TabID = TabId
            Else
                grdReviewFields.Visible = False
                lblNoReviewFields.Visible = True
                ctlPagingControl.Visible = False
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetReviewFieldEditUrl(ByVal reviewFieldID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewField", "reviewFieldID=" & reviewFieldID)

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
                    BindReviewFields()
                End If

                cmdAddReviewField.CssClass = PropertySettings.ButtonClass
                cmdReturnToModule.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpRecordsPerPage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRecordsPerPage.SelectedIndexChanged

            Try

                _currentPage = 1
                Response.Cookies.Add(New HttpCookie("PA-RFM-PageSize", drpRecordsPerPage.SelectedValue))
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields", "PageRecords=" & drpRecordsPerPage.SelectedValue), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddCustomField_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddReviewField.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewField"), True)

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

        Private Sub grdReviewFields_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdReviewFields.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim btnUp As ImageButton = CType(e.Item.FindControl("btnUp"), ImageButton)
                Dim btnDown As ImageButton = CType(e.Item.FindControl("btnDown"), ImageButton)

                Dim objReviewField As ReviewFieldInfo = CType(e.Item.DataItem, ReviewFieldInfo)

                If Not (btnUp Is Nothing And btnDown Is Nothing) Then

                    If (objReviewField.ReviewFieldID = CType(_reviewFields(0), ReviewFieldInfo).ReviewFieldID) Then
                        btnUp.Visible = False
                    End If

                    If (objReviewField.ReviewFieldID = CType(_reviewFields(_reviewFields.Count - 1), ReviewFieldInfo).ReviewFieldID) Then
                        btnDown.Visible = False
                    End If

                    btnUp.CommandArgument = objReviewField.ReviewFieldID.ToString()
                    btnUp.CommandName = "Up"

                    btnDown.CommandArgument = objReviewField.ReviewFieldID.ToString()
                    btnDown.CommandName = "Down"

                End If

            End If

        End Sub

        Private Sub grdReviewFields_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdReviewFields.ItemCommand

            Dim objReviewFieldController As New ReviewFieldController
            _reviewFields = objReviewFieldController.List(Me.ModuleId)

            Dim reviewFieldID As Integer = Convert.ToInt32(e.CommandArgument)

            For i As Integer = 0 To _reviewFields.Count - 1

                Dim objReviewField As ReviewFieldInfo = CType(_reviewFields(i), ReviewFieldInfo)

                If (reviewFieldID = objReviewField.ReviewFieldID) Then
                    If (e.CommandName = "Up") Then

                        Dim objReviewFieldToSwap As ReviewFieldInfo = CType(_reviewFields(i - 1), ReviewFieldInfo)

                        Dim sortOrder As Integer = objReviewField.SortOrder
                        Dim sortOrderPrevious As Integer = objReviewFieldToSwap.SortOrder

                        objReviewField.SortOrder = sortOrderPrevious
                        objReviewFieldToSwap.SortOrder = sortOrder

                        objReviewFieldController.Update(objReviewField)
                        objReviewFieldController.Update(objReviewFieldToSwap)

                    End If


                    If (e.CommandName = "Down") Then

                        Dim objReviewFieldToSwap As ReviewFieldInfo = CType(_reviewFields(i + 1), ReviewFieldInfo)

                        Dim sortOrder As Integer = objReviewField.SortOrder
                        Dim sortOrderNext As Integer = objReviewFieldToSwap.SortOrder

                        objReviewField.SortOrder = sortOrderNext
                        objReviewFieldToSwap.SortOrder = sortOrder

                        objReviewFieldController.Update(objReviewField)
                        objReviewFieldController.Update(objReviewFieldToSwap)

                    End If

                End If

            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace