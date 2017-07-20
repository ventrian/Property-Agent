Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditPropertyType
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _propertyTypeID As Integer = Null.NullInteger
        Private _parentID As Integer = Null.NullInteger

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

            If Not (Request("ParentID") Is Nothing) Then
                _parentID = Convert.ToInt32(Request("ParentID"))
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

            Dim objPropertyTypeEdit As New CrumbInfo
            If (_propertyTypeID <> Null.NullInteger) Then
                objPropertyTypeEdit.Caption = Localization.GetString("EditPropertyType", LocalResourceFile)
            Else
                objPropertyTypeEdit.Caption = Localization.GetString("AddPropertyType", LocalResourceFile)
            End If
            objPropertyTypeEdit.Url = Request.RawUrl
            crumbs.Add(objPropertyTypeEdit)

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

        Private Sub BindParentTypes()

            Dim objPropertyTypeController As New PropertyTypeController

            drpParentType.DataSource = objPropertyTypeController.ListAll(Me.ModuleId, True, Me.PropertySettings.TypesSortBy, Null.NullString())
            drpParentType.DataBind()

            drpParentType.Items.Insert(0, New ListItem(GetResourceString("NoneSpecified"), "-1"))

            If Not (drpParentType.Items.FindByValue(_parentID.ToString()) Is Nothing) Then
                drpParentType.SelectedValue = _parentID.ToString()
            End If

        End Sub

        Private Sub BindPropertyType()

            If (_propertyTypeID = Null.NullInteger) Then

                cmdDelete.Visible = False

            Else

                Dim objPropertyTypeController As New PropertyTypeController
                Dim objPropertyTypeInfo As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, _propertyTypeID)

                If Not (objPropertyTypeInfo Is Nothing) Then

                    If Not (drpParentType.Items.FindByValue(objPropertyTypeInfo.ParentID.ToString()) Is Nothing) Then
                        drpParentType.SelectedValue = objPropertyTypeInfo.ParentID.ToString()
                    End If

                    txtName.Text = objPropertyTypeInfo.Name
                    txtDescription.Text = objPropertyTypeInfo.Description
                    ctlImage.Url = objPropertyTypeInfo.ImageFile
                    chkIsPublished.Checked = objPropertyTypeInfo.IsPublished
                    chkAllowProperties.Checked = objPropertyTypeInfo.AllowProperties

                End If

            End If


        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ctlImage.FileFilter = glbImageFileTypes

                If (Page.IsPostBack = False) Then

                    Page.SetFocus(txtName)
                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")

                    BindParentTypes()
                    BindPropertyType()

                End If

                cmdUpdate.CssClass = PropertySettings.ButtonClass
                cmdCancel.CssClass = PropertySettings.ButtonClass
                cmdDelete.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Dim objPropertyTypeController As New PropertyTypeController
                    Dim objPropertyTypeInfo As New PropertyTypeInfo

                    objPropertyTypeInfo.ParentID = Convert.ToInt32(drpParentType.SelectedValue)
                    objPropertyTypeInfo.Name = txtName.Text
                    objPropertyTypeInfo.Description = txtDescription.Text
                    objPropertyTypeInfo.ImageFile = ctlImage.Url
                    objPropertyTypeInfo.IsPublished = chkIsPublished.Checked
                    objPropertyTypeInfo.AllowProperties = chkAllowProperties.Checked
                    objPropertyTypeInfo.ModuleID = Me.ModuleId

                    If (_propertyTypeID = Null.NullInteger) Then

                        Dim objPropertyTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.List(Me.ModuleId, False)

                        If (objPropertyTypes.Count = 0) Then
                            objPropertyTypeInfo.SortOrder = 0
                        Else
                            objPropertyTypeInfo.SortOrder = CType(objPropertyTypes(objPropertyTypes.Count - 1), PropertyTypeInfo).SortOrder + 1
                        End If

                        objPropertyTypeController.Add(objPropertyTypeInfo)

                    Else

                        Dim objPropertyTypeInfoOld As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, _propertyTypeID)

                        objPropertyTypeInfo.SortOrder = objPropertyTypeInfoOld.SortOrder
                        objPropertyTypeInfo.PropertyTypeID = _propertyTypeID
                        objPropertyTypeController.Update(objPropertyTypeInfo)

                    End If

                    If (drpParentType.SelectedValue <> "-1") Then
                        Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes", PropertySettings.SEOPropertyTypeID & "=" & drpParentType.SelectedValue), True)
                    Else
                        Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes"), True)
                    End If

                End If

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                If (_propertyTypeID <> Null.NullInteger) Then
                    Dim objPropertyTypeController As New PropertyTypeController
                    Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, _propertyTypeID)

                    If Not (objPropertyType Is Nothing) Then
                        If (objPropertyType.ParentID <> Null.NullInteger) Then
                            Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes", PropertySettings.SEOPropertyTypeID & "=" & objPropertyType.ParentID.ToString()), True)
                        End If
                    End If
                End If
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes"), True)

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objPropertyController As New PropertyController
                Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(Me.ModuleId, _propertyTypeID, SearchStatusType.Any, Null.NullInteger, Null.NullInteger, False, SortByType.Published, Null.NullInteger, SortDirectionType.Descending, Null.NullString, Null.NullString, 0, 100000, False)

                For Each objProperty As PropertyInfo In objProperties
                    Dim objPhotoController As New PhotoController
                    Dim objPhotos As ArrayList = objPhotoController.List(objProperty.PropertyID)

                    For Each objPhoto As PhotoInfo In objPhotos
                        System.IO.File.Delete(Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent/" & Me.ModuleId & "/" & objPhoto.Filename)
                    Next

                    objPhotoController.DeleteByPropertyID(objProperty.PropertyID)
                    objPropertyController.Delete(objProperty.PropertyID)
                Next

                Dim objPropertyTypeController As New PropertyTypeController

                Dim objSubPropertyTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.List(Me.ModuleId, True, PropertyTypeSortByType.Standard, Null.NullString(), _propertyTypeID)

                For Each objPropertyType As PropertyTypeInfo In objSubPropertyTypes
                    objPropertyType.ParentID = Null.NullInteger
                    objPropertyTypeController.Update(objPropertyType)
                Next

                If (_propertyTypeID <> Null.NullInteger) Then
                    Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, _propertyTypeID)

                    If Not (objPropertyType Is Nothing) Then
                        objPropertyTypeController.Delete(Me.ModuleId, _propertyTypeID)
                        If (objPropertyType.ParentID <> Null.NullInteger) Then
                            Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes", PropertySettings.SEOPropertyTypeID & "=" & objPropertyType.ParentID.ToString()), True)
                        End If
                    End If
                End If

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes"), True)

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valParentType_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valParentType.ServerValidate

            Try

                If (_propertyTypeID = Null.NullInteger) Then
                    args.IsValid = True
                    Return
                End If

                ' Loop through parent IDs to make should an infinite loop is not created.

                If (drpParentType.SelectedValue <> "-1") Then

                    Dim objPropertyTypeController As New PropertyTypeController

                    Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, Convert.ToInt32(drpParentType.SelectedValue))

                    If (objPropertyType.PropertyTypeID = _propertyTypeID) Then
                        args.IsValid = False
                        Return
                    End If

                    While Not (objPropertyType Is Nothing)
                        If (objPropertyType.PropertyTypeID = _propertyTypeID) Then
                            args.IsValid = False
                            Return
                        End If
                        objPropertyType = objPropertyTypeController.Get(Me.ModuleId, objPropertyType.ParentID)
                    End While

                Else
                    args.IsValid = True
                End If

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace