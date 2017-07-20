Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditPropertyTypes
        Inherits PropertyAgentBase

#Region " Private Members "

        Dim _propertyTypeID As Integer = Null.NullInteger

        Dim _currentPage As Integer = 1
        Dim _pageRecords As Integer = Null.NullInteger

        Dim _propertyTypes As List(Of PropertyTypeInfo)

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            Dim propertyTypeIDParam As String = PropertySettings.SEOPropertyTypeID
            If (Request(propertyTypeIDParam) = "") Then
                propertyTypeIDParam = "PropertyTypeID"
            End If
            If Not (Request(propertyTypeIDParam) Is Nothing) Then
                _propertyTypeID = Convert.ToInt32(Request(propertyTypeIDParam))
            End If

            If Not (Request("CurrentPage") Is Nothing) Then
                _currentPage = Convert.ToInt32(Request("CurrentPage"))
            End If

            If Not (Request("PageRecords") Is Nothing) Then
                _pageRecords = Convert.ToInt32(Request("PageRecords"))
            End If

        End Sub

        Private Sub BindPageCount()

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

            Dim objPropertyTypeManager As New CrumbInfo
            objPropertyTypeManager.Caption = Localization.GetString("PropertyTypeManager", LocalResourceFile)
            objPropertyTypeManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes")
            crumbs.Add(objPropertyTypeManager)

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

        Private Sub BindPropertyTypes()

            Dim PageSize As Integer = Convert.ToInt32(drpRecordsPerPage.SelectedItem.Value)

            Localization.LocalizeDataGrid(grdPropertyTypes, Me.LocalResourceFile)

            Dim objPropertyTypeController As New PropertyTypeController
            _propertyTypes = objPropertyTypeController.List(Me.ModuleId, False, Me.PropertySettings.TypesSortBy, Null.NullString(), _propertyTypeID)

            If (Me.PropertySettings.TypesSortBy = PropertyTypeSortByType.Standard) Then
                If (_propertyTypes.Count > 1) Then
                    For i As Integer = 0 To _propertyTypes.Count - 1
                        Dim objPropertyType As PropertyTypeInfo = CType(_propertyTypes(i), PropertyTypeInfo)
                        objPropertyType.SortOrder = i
                        objPropertyTypeController.Update(objPropertyType)
                    Next
                End If
            End If

            Dim objPagedDataSource As New PagedDataSource

            objPagedDataSource.AllowPaging = True
            objPagedDataSource.DataSource = _propertyTypes
            objPagedDataSource.PageSize = PageSize
            objPagedDataSource.CurrentPageIndex = _currentPage - 1

            If (_propertyTypes.Count > 0) Then

                grdPropertyTypes.DataSource = objPagedDataSource
                grdPropertyTypes.DataBind()

                If (Me.PropertySettings.TypesSortBy = PropertyTypeSortByType.Name) Then
                    grdPropertyTypes.Columns(4).Visible = False
                End If

                grdPropertyTypes.Visible = True
                lblNoPropertyTypes.Visible = False
                ctlPagingControl.Visible = True

                ctlPagingControl.Visible = True
                ctlPagingControl.TotalRecords = _propertyTypes.Count
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = _currentPage
                If (_propertyTypeID = Null.NullInteger) Then
                    ctlPagingControl.QuerystringParams = PropertySettings.SEOAgentType & "=EditPropertyTypes"
                Else
                    ctlPagingControl.QuerystringParams = PropertySettings.SEOAgentType & "=EditPropertyTypes&" & PropertySettings.SEOPropertyTypeID & "=" & _propertyTypeID.ToString()
                End If

                If (drpRecordsPerPage.SelectedIndex <> 0) Then
                    ctlPagingControl.QuerystringParams = ctlPagingControl.QuerystringParams & "&PageRecords=" + drpRecordsPerPage.SelectedValue
                End If

                ctlPagingControl.TabID = TabId

            Else

                grdPropertyTypes.Visible = False
                lblNoPropertyTypes.Visible = True
                ctlPagingControl.Visible = False

            End If

            Dim crumbs As New ArrayList

            Dim objCrumb As New CrumbInfo
            objCrumb.Caption = GetResourceString("Top Level")
            objCrumb.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes")
            crumbs.Add(objCrumb)

            If (_propertyTypeID <> Null.NullInteger) Then

                Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, _propertyTypeID)

                If Not (objPropertyType Is Nothing) Then

                    Dim parentID As Integer = objPropertyType.ParentID

                    Do
                        Dim objCrumbType As New CrumbInfo
                        objCrumbType.Caption = objPropertyType.Name
                        objCrumbType.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes", PropertySettings.SEOPropertyTypeID & "=" & objPropertyType.PropertyTypeID)
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

            End If

            If (crumbs.Count > 1) Then
                rptTypeCrumbs.DataSource = crumbs
                rptTypeCrumbs.DataBind()
            End If

            If (ctlPagingControl.TotalRecords <= PageSize) Then
                ctlPagingControl.Visible = False
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetPropertyTypeEditUrl(ByVal propertyTypeID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyType", "propertyTypeID=" & propertyTypeID)

        End Function

        Protected Function GetSubPropertyTypeEditUrl(ByVal propertyTypeID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes", "propertyTypeID=" & propertyTypeID)

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
                    BindPropertyTypes()
                End If

                cmdAddPropertyType.CssClass = PropertySettings.ButtonClass
                cmdReturnToModule.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddPropertyType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddPropertyType.Click

            Try

                If (_propertyTypeID = Null.NullInteger) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyType"), True)
                Else
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyType", "parentID=" & _propertyTypeID.ToString()), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdReturnToModule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturnToModule.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpRecordsPerPage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRecordsPerPage.SelectedIndexChanged

            Try

                If (_propertyTypeID <> Null.NullInteger) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes", "PageRecords=" & drpRecordsPerPage.SelectedValue, PropertySettings.SEOPropertyTypeID & "=" & _propertyTypeID.ToString()), True)
                Else
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes", "PageRecords=" & drpRecordsPerPage.SelectedValue), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdPropertyTypes_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdPropertyTypes.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim btnUp As ImageButton = CType(e.Item.FindControl("btnUp"), ImageButton)
                Dim btnDown As ImageButton = CType(e.Item.FindControl("btnDown"), ImageButton)

                Dim objPropertyType As PropertyTypeInfo = CType(e.Item.DataItem, PropertyTypeInfo)

                If Not (btnUp Is Nothing And btnDown Is Nothing) Then

                    If (objPropertyType.PropertyTypeID = CType(_propertyTypes(0), PropertyTypeInfo).PropertyTypeID) Then
                        btnUp.Visible = False
                    End If

                    If (objPropertyType.PropertyTypeID = CType(_propertyTypes(_propertyTypes.Count - 1), PropertyTypeInfo).PropertyTypeID) Then
                        btnDown.Visible = False
                    End If

                    btnUp.CommandArgument = objPropertyType.PropertyTypeID.ToString()
                    btnUp.CommandName = "Up"

                    btnDown.CommandArgument = objPropertyType.PropertyTypeID.ToString()
                    btnDown.CommandName = "Down"

                End If

                Dim btnPublished As ImageButton = CType(e.Item.FindControl("btnPublished"), ImageButton)

                If Not (btnPublished Is Nothing) Then
                    If (objPropertyType.IsPublished) Then
                        btnPublished.ImageUrl = "~/images/checked.gif"
                        btnPublished.CommandName = "PublishedChecked"
                    Else
                        btnPublished.ImageUrl = "~/images/cancel.gif"
                        btnPublished.CommandName = "PublishedCancel"
                    End If

                    btnPublished.CommandArgument = objPropertyType.PropertyTypeID.ToString()
                End If

            End If

        End Sub

        Private Sub grdPropertyTypes_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdPropertyTypes.ItemCommand

            Dim objPropertyTypeController As New PropertyTypeController
            _propertyTypes = objPropertyTypeController.List(Me.ModuleId, False, PropertyTypeSortByType.Standard, Null.NullString(), _propertyTypeID)

            Dim propertyTypeID As Integer = Convert.ToInt32(e.CommandArgument)

            For i As Integer = 0 To _propertyTypes.Count - 1

                Dim objPropertyType As PropertyTypeInfo = CType(_propertyTypes(i), PropertyTypeInfo)

                If (propertyTypeID = objPropertyType.PropertyTypeID) Then
                    If (e.CommandName = "Up") Then

                        Dim objPropertyTypeToSwap As PropertyTypeInfo = CType(_propertyTypes(i - 1), PropertyTypeInfo)

                        Dim sortOrder As Integer = objPropertyType.SortOrder
                        Dim sortOrderPrevious As Integer = objPropertyTypeToSwap.SortOrder

                        objPropertyType.SortOrder = sortOrderPrevious
                        objPropertyTypeToSwap.SortOrder = sortOrder

                        objPropertyTypeController.Update(objPropertyType)
                        objPropertyTypeController.Update(objPropertyTypeToSwap)

                    End If


                    If (e.CommandName = "Down") Then

                        Dim objPropertyTypeToSwap As PropertyTypeInfo = CType(_propertyTypes(i + 1), PropertyTypeInfo)

                        Dim sortOrder As Integer = objPropertyType.SortOrder
                        Dim sortOrderNext As Integer = objPropertyTypeToSwap.SortOrder

                        objPropertyType.SortOrder = sortOrderNext
                        objPropertyTypeToSwap.SortOrder = sortOrder

                        objPropertyTypeController.Update(objPropertyType)
                        objPropertyTypeController.Update(objPropertyTypeToSwap)

                    End If

                    If (e.CommandName = "PublishedChecked") Then
                        objPropertyType.IsPublished = False
                        objPropertyTypeController.Update(objPropertyType)
                    End If

                    If (e.CommandName = "PublishedCancel") Then
                        objPropertyType.IsPublished = True
                        objPropertyTypeController.Update(objPropertyType)
                    End If

                End If

            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace