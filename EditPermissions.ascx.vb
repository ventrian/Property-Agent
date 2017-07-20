Imports DotNetNuke.Common
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditPermissions
        Inherits PropertyAgentBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCrumbPermissions As New CrumbInfo
            objCrumbPermissions.Caption = Localization.GetString("EditPermissions", Me.LocalResourceFile)
            objCrumbPermissions.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPermissions")
            crumbs.Add(objCrumbPermissions)

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

        Private Sub BindSettings()

            Dim objRoleController As New RoleController
            Dim availableRoles As New ArrayList
            Dim objRoles As ArrayList = objRoleController.GetPortalRoles(Me.PortalId)

            For Each objRole As RoleInfo In objRoles
                availableRoles.Add(New ListItem(objRole.RoleName, objRole.RoleName))
            Next

            availableRoles.Add(New ListItem(GetResourceString("UnauthenticatedUsers"), glbRoleUnauthUserName))

            grdPermissions.DataSource = availableRoles
            grdPermissions.DataBind()

            grdPermissionsAdvanced.DataSource = availableRoles
            grdPermissionsAdvanced.DataBind()

            grdPermissionsAdmin.DataSource = availableRoles
            grdPermissionsAdmin.DataBind()

            txtDetailUrl.Text = PropertySettings.PermissionDetailUrl

        End Sub

        Private Function GetLimitValue(ByVal role As String, ByVal limitSetting As String) As String

            Try

                For Each setting As String In limitSetting.Split(";"c)

                    If (role = setting.Split(":"c)(0)) Then
                        Return setting.Split(":"c)(1)
                    End If

                Next

            Catch
            End Try

            Return ""

        End Function

        Private Function IsInRole(ByVal roleName As String, ByVal roles As String()) As Boolean

            For Each role As String In roles
                If (roleName = role) Then
                    Return True
                End If
            Next

            Return False

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                BindCrumbs()

                trAdminSettings.Visible = (Me.UserInfo.IsSuperUser)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (Page.IsPostBack = False) Then
                    BindSettings()
                End If

                cmdUpdate.CssClass = PropertySettings.ButtonClass
                cmdCancel.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                lblSubmitAmount.Text = GetResourceString("SubmitAmount")

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Dim viewDetailRoles As String = ""
                    Dim submitRoles As String = ""
                    Dim limit As String = ""
                    Dim addImageRoles As String = ""
                    Dim addImagesLimit As String = ""
                    Dim approveRoles As String = ""
                    Dim autoApproveRoles As String = ""
                    Dim deleteRoles As String = ""
                    Dim featureRoles As String = ""
                    Dim autoFeatureRoles As String = ""
                    Dim brokerRoles As String = ""
                    Dim exportRoles As String = ""
                    Dim lockdownRoles As String = ""
                    Dim publishDetailRoles As String = ""

                    Dim customFieldRoles As String = ""
                    Dim reviewFieldRoles As String = ""
                    Dim emailFilesRoles As String = ""
                    Dim layoutFilesRoles As String = ""
                    Dim layoutSettingsRoles As String = ""
                    Dim typesRoles As String = ""

                    For Each item As DataGridItem In grdPermissions.Items

                        Dim role As String = grdPermissions.DataKeys(item.ItemIndex).ToString()

                        Dim chkSubmit As CheckBox = CType(item.FindControl("chkSubmit"), CheckBox)
                        Dim txtAmount As TextBox = CType(item.FindControl("txtAmount"), TextBox)
                        Dim chkAddImages As CheckBox = CType(item.FindControl("chkAddImages"), CheckBox)
                        Dim txtAddImagesLimit As TextBox = CType(item.FindControl("txtAddImagesLimit"), TextBox)
                        Dim chkApprove As CheckBox = CType(item.FindControl("chkApprove"), CheckBox)
                        Dim chkAutoApprove As CheckBox = CType(item.FindControl("chkAutoApprove"), CheckBox)
                        Dim chkDelete As CheckBox = CType(item.FindControl("chkDelete"), CheckBox)
                        Dim chkFeature As CheckBox = CType(item.FindControl("chkFeature"), CheckBox)
                        Dim chkAutoFeature As CheckBox = CType(item.FindControl("chkAutoFeature"), CheckBox)

                        If (chkSubmit.Checked) Then
                            If (submitRoles = "") Then
                                submitRoles = role
                            Else
                                submitRoles = submitRoles & ";" & role
                            End If

                            If (txtAmount.Text.Length > 0) Then
                                If (IsNumeric(txtAmount.Text)) Then
                                    If (limit = "") Then
                                        limit = role & ":" & Convert.ToInt32(txtAmount.Text).ToString()
                                    Else
                                        limit = limit & ";" & role & ":" & Convert.ToInt32(txtAmount.Text).ToString()
                                    End If
                                End If
                            End If
                        End If

                        If (chkAddImages.Checked) Then
                            If (addImageRoles = "") Then
                                addImageRoles = role
                            Else
                                addImageRoles = addImageRoles & ";" & role
                            End If

                            If (txtAddImagesLimit.Text.Length > 0) Then
                                If (IsNumeric(txtAddImagesLimit.Text)) Then
                                    If (addImagesLimit = "") Then
                                        addImagesLimit = role & ":" & Convert.ToInt32(txtAddImagesLimit.Text).ToString()
                                    Else
                                        addImagesLimit = addImagesLimit & ";" & role & ":" & Convert.ToInt32(txtAddImagesLimit.Text).ToString()
                                    End If
                                End If
                            End If
                        End If

                        If (chkApprove.Checked) Then
                            If (approveRoles = "") Then
                                approveRoles = role
                            Else
                                approveRoles = approveRoles & ";" & role
                            End If
                        End If

                        If (chkAutoApprove.Checked) Then
                            If (autoApproveRoles = "") Then
                                autoApproveRoles = role
                            Else
                                autoApproveRoles = autoApproveRoles & ";" & role
                            End If
                        End If

                        If (chkDelete.Checked) Then
                            If (deleteRoles = "") Then
                                deleteRoles = role
                            Else
                                deleteRoles = deleteRoles & ";" & role
                            End If
                        End If


                        If (chkFeature.Checked) Then
                            If (featureRoles = "") Then
                                featureRoles = role
                            Else
                                featureRoles = featureRoles & ";" & role
                            End If
                        End If

                        If (chkAutoFeature.Checked) Then
                            If (autoFeatureRoles = "") Then
                                autoFeatureRoles = role
                            Else
                                autoFeatureRoles = autoFeatureRoles & ";" & role
                            End If
                        End If

                    Next

                    For Each item As DataGridItem In grdPermissionsAdvanced.Items

                        Dim role As String = grdPermissions.DataKeys(item.ItemIndex).ToString()

                        Dim chkDetail As CheckBox = CType(item.FindControl("chkDetail"), CheckBox)
                        Dim chkBroker As CheckBox = CType(item.FindControl("chkBroker"), CheckBox)
                        Dim chkExport As CheckBox = CType(item.FindControl("chkExport"), CheckBox)
                        Dim chkLockDown As CheckBox = CType(item.FindControl("chkLockDown"), CheckBox)
                        Dim chkPublishDetail As CheckBox = CType(item.FindControl("chkPublishDetail"), CheckBox)

                        If (chkDetail.Checked) Then
                            If (viewDetailRoles = "") Then
                                viewDetailRoles = role
                            Else
                                viewDetailRoles = viewDetailRoles & ";" & role
                            End If
                        End If

                        If (chkBroker.Checked) Then
                            If (brokerRoles = "") Then
                                brokerRoles = role
                            Else
                                brokerRoles = brokerRoles & ";" & role
                            End If
                        End If

                        If (chkExport.Checked) Then
                            If (exportRoles = "") Then
                                exportRoles = role
                            Else
                                exportRoles = exportRoles & ";" & role
                            End If
                        End If

                        If (chkLockDown.Checked) Then
                            If (lockdownRoles = "") Then
                                lockdownRoles = role
                            Else
                                lockdownRoles = lockdownRoles & ";" & role
                            End If
                        End If

                        If (chkPublishDetail.Checked) Then
                            If (publishDetailRoles = "") Then
                                publishDetailRoles = role
                            Else
                                publishDetailRoles = publishDetailRoles & ";" & role
                            End If
                        End If

                    Next

                    For Each item As DataGridItem In grdPermissionsAdmin.Items

                        Dim role As String = grdPermissions.DataKeys(item.ItemIndex).ToString()

                        Dim chkCustomFields As CheckBox = CType(item.FindControl("chkCustomFields"), CheckBox)
                        Dim chkReviewFields As CheckBox = CType(item.FindControl("chkReviewFields"), CheckBox)
                        Dim chkEmailFiles As CheckBox = CType(item.FindControl("chkEmailFiles"), CheckBox)
                        Dim chkLayoutFiles As CheckBox = CType(item.FindControl("chkLayoutFiles"), CheckBox)
                        Dim chkLayoutSettings As CheckBox = CType(item.FindControl("chkLayoutSettings"), CheckBox)
                        Dim chkTypes As CheckBox = CType(item.FindControl("chkTypes"), CheckBox)

                        If (chkCustomFields.Checked) Then
                            If (customFieldRoles = "") Then
                                customFieldRoles = role
                            Else
                                customFieldRoles = customFieldRoles & ";" & role
                            End If
                        End If

                        If (chkReviewFields.Checked) Then
                            If (reviewFieldRoles = "") Then
                                reviewFieldRoles = role
                            Else
                                reviewFieldRoles = reviewFieldRoles & ";" & role
                            End If
                        End If

                        If (chkEmailFiles.Checked) Then
                            If (emailFilesRoles = "") Then
                                emailFilesRoles = role
                            Else
                                emailFilesRoles = emailFilesRoles & ";" & role
                            End If
                        End If

                        If (chkLayoutFiles.Checked) Then
                            If (layoutFilesRoles = "") Then
                                layoutFilesRoles = role
                            Else
                                layoutFilesRoles = layoutFilesRoles & ";" & role
                            End If
                        End If

                        If (chkLayoutSettings.Checked) Then
                            If (layoutSettingsRoles = "") Then
                                layoutSettingsRoles = role
                            Else
                                layoutSettingsRoles = layoutSettingsRoles & ";" & role
                            End If
                        End If

                        If (chkTypes.Checked) Then
                            If (typesRoles = "") Then
                                typesRoles = role
                            Else
                                typesRoles = typesRoles & ";" & role
                            End If
                        End If

                    Next

                    Dim objModules As New DotNetNuke.Entities.Modules.ModuleController

                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_VIEW_DETAIL_SETTING, viewDetailRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_SUBMIT_SETTING, submitRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_LIMIT_SETTING, limit)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADD_IMAGES_SETTING, addImageRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADD_IMAGES_LIMIT_SETTING, addImagesLimit)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_APPROVE_SETTING, approveRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_AUTO_APPROVE_SETTING, autoApproveRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_DELETE_SETTING, deleteRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_FEATURE_SETTING, featureRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_AUTO_FEATURE_SETTING, autoFeatureRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_BROKER_SETTING, brokerRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_EXPORT_SETTING, exportRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_LOCKDOWN_SETTING, lockdownRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_PUBLISH_DETAIL_SETTING, publishDetailRoles)

                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_DETAIL_URL_SETTING, txtDetailUrl.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADMIN_CUSTOM_FIELD_SETTING, customFieldRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADMIN_REVIEW_FIELD_SETTING, reviewFieldRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADMIN_EMAIL_FILES_SETTING, emailFilesRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADMIN_LAYOUT_FILES_SETTING, layoutFilesRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADMIN_LAYOUT_SETTINGS_SETTING, layoutSettingsRoles)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_ADMIN_TYPES_SETTING, typesRoles)

                End If

                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdPermissions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdPermissions.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                If Not (objListItem Is Nothing) Then

                    Dim role As String = CType(e.Item.DataItem, ListItem).Value

                    Dim chkSubmit As CheckBox = CType(e.Item.FindControl("chkSubmit"), CheckBox)
                    Dim txtAmount As TextBox = CType(e.Item.FindControl("txtAmount"), TextBox)
                    Dim litAmount As Literal = CType(e.Item.FindControl("litAmount"), Literal)

                    Dim chkAddImages As CheckBox = CType(e.Item.FindControl("chkAddImages"), CheckBox)
                    Dim txtAddImagesLimit As TextBox = CType(e.Item.FindControl("txtAddImagesLimit"), TextBox)
                    Dim litAddImagesLimit As Literal = CType(e.Item.FindControl("litAddImagesLimit"), Literal)

                    Dim chkApprove As CheckBox = CType(e.Item.FindControl("chkApprove"), CheckBox)
                    Dim chkAutoApprove As CheckBox = CType(e.Item.FindControl("chkAutoApprove"), CheckBox)
                    Dim chkDelete As CheckBox = CType(e.Item.FindControl("chkDelete"), CheckBox)
                    Dim chkFeature As CheckBox = CType(e.Item.FindControl("chkFeature"), CheckBox)
                    Dim chkAutoFeature As CheckBox = CType(e.Item.FindControl("chkAutoFeature"), CheckBox)

                    If (objListItem.Value = glbRoleUnauthUserName) Then

                        chkSubmit.Enabled = False
                        chkAddImages.Enabled = False
                        chkAutoApprove.Enabled = False
                        chkApprove.Enabled = False
                        chkAutoFeature.Enabled = False
                        chkDelete.Enabled = False
                        chkFeature.Enabled = False
                        txtAmount.Visible = False
                        litAmount.Visible = False
                        txtAddImagesLimit.Visible = False
                        litAddImagesLimit.Visible = False

                    End If

                    If (objListItem.Value = PortalSettings.AdministratorRoleName.ToString()) Then

                        chkSubmit.Enabled = False
                        txtAmount.Visible = False
                        litAmount.Visible = False
                        chkAddImages.Enabled = False
                        txtAddImagesLimit.Visible = False
                        litAddImagesLimit.Visible = False
                        chkApprove.Enabled = False
                        chkAutoApprove.Enabled = False
                        chkDelete.Enabled = False
                        chkFeature.Enabled = False
                        chkAutoFeature.Enabled = True

                        chkSubmit.Checked = True
                        chkAddImages.Checked = True
                        chkApprove.Checked = True
                        chkAutoApprove.Checked = True
                        chkDelete.Checked = True
                        chkFeature.Checked = True
                        chkAutoFeature.Checked = IsInRole(PortalSettings.AdministratorRoleName, PropertySettings.PermissionAutoFeature.Split(";"c))

                    Else

                        chkSubmit.Checked = IsInRole(role, PropertySettings.PermissionSubmit.Split(";"c))
                        If (objListItem.Value = glbRoleUnauthUserName) Then
                            chkSubmit.Checked = False
                        End If

                        txtAmount.Text = GetLimitValue(role, PropertySettings.PermissionLimit)

                        chkAddImages.Checked = IsInRole(role, PropertySettings.PermissionAddImages.Split(";"c))
                        txtAddImagesLimit.Text = GetLimitValue(role, PropertySettings.PermissionAddImagesLimit)
                        If (objListItem.Value = glbRoleUnauthUserName) Then
                            chkAddImages.Checked = False
                        End If

                        chkApprove.Checked = IsInRole(role, PropertySettings.PermissionApprove.Split(";"c))
                        chkAutoApprove.Checked = IsInRole(role, PropertySettings.PermissionAutoApprove.Split(";"c))
                        chkDelete.Checked = IsInRole(role, PropertySettings.PermissionDelete.Split(";"c))
                        chkFeature.Checked = IsInRole(role, PropertySettings.PermissionFeature.Split(";"c))
                        chkAutoFeature.Checked = IsInRole(role, PropertySettings.PermissionAutoFeature.Split(";"c))

                    End If

                End If

            End If

        End Sub

        Private Sub grdPermissionsAdvanced_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdPermissionsAdvanced.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                If Not (objListItem Is Nothing) Then

                    Dim role As String = CType(e.Item.DataItem, ListItem).Value

                    Dim chkDetail As CheckBox = CType(e.Item.FindControl("chkDetail"), CheckBox)
                    Dim chkBroker As CheckBox = CType(e.Item.FindControl("chkBroker"), CheckBox)
                    Dim chkExport As CheckBox = CType(e.Item.FindControl("chkExport"), CheckBox)
                    Dim chkLockDown As CheckBox = CType(e.Item.FindControl("chkLockDown"), CheckBox)
                    Dim chkPublishDetail As CheckBox = CType(e.Item.FindControl("chkPublishDetail"), CheckBox)

                    If (objListItem.Value = glbRoleUnauthUserName) Then

                        chkDetail.Enabled = True
                        chkBroker.Enabled = False
                        chkExport.Enabled = True
                        chkLockDown.Enabled = False
                        chkPublishDetail.Enabled = False

                    End If

                    If (objListItem.Value = PortalSettings.AdministratorRoleName) Then

                        chkDetail.Enabled = False
                        chkBroker.Enabled = True
                        chkExport.Enabled = True
                        chkLockDown.Enabled = False
                        chkPublishDetail.Enabled = False

                        chkDetail.Checked = True
                        chkBroker.Checked = IsInRole(PortalSettings.AdministratorRoleName, PropertySettings.PermissionViewDetail.Split(";"c))
                        chkExport.Checked = IsInRole(PortalSettings.AdministratorRoleName, PropertySettings.PermissionExport.Split(";"c))
                        chkLockDown.Checked = True
                        chkPublishDetail.Checked = True

                    Else

                        chkDetail.Checked = (PropertySettings.IsPermissionViewDetailSet = False Or IsInRole(role, PropertySettings.PermissionViewDetail.Split(";"c)))
                        chkBroker.Checked = IsInRole(role, PropertySettings.PermissionBroker.Split(";"c))
                        chkExport.Checked = IsInRole(role, PropertySettings.PermissionExport.Split(";"c))
                        chkLockDown.Checked = IsInRole(role, PropertySettings.PermissionLockDown.Split(";"c))
                        chkPublishDetail.Checked = IsInRole(role, PropertySettings.PermissionPublishDetail.Split(";"c))

                    End If

                End If

            End If

        End Sub

        Private Sub grdPermissionsAdmin_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdPermissionsAdmin.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                If Not (objListItem Is Nothing) Then

                    Dim role As String = CType(e.Item.DataItem, ListItem).Value

                    Dim chkCustomFields As CheckBox = CType(e.Item.FindControl("chkCustomFields"), CheckBox)
                    Dim chkReviewFields As CheckBox = CType(e.Item.FindControl("chkReviewFields"), CheckBox)
                    Dim chkEmailFiles As CheckBox = CType(e.Item.FindControl("chkEmailFiles"), CheckBox)
                    Dim chkLayoutFiles As CheckBox = CType(e.Item.FindControl("chkLayoutFiles"), CheckBox)
                    Dim chkLayoutSettings As CheckBox = CType(e.Item.FindControl("chkLayoutSettings"), CheckBox)
                    Dim chkTypes As CheckBox = CType(e.Item.FindControl("chkTypes"), CheckBox)

                    If (objListItem.Value = glbRoleUnauthUserName) Then

                        chkCustomFields.Enabled = False
                        chkReviewFields.Enabled = False
                        chkEmailFiles.Enabled = False
                        chkLayoutFiles.Enabled = False
                        chkLayoutSettings.Enabled = False
                        chkTypes.Enabled = False

                    Else

                        chkCustomFields.Checked = IsInRole(role, PropertySettings.PermissionAdminCustomField.Split(";"c))
                        chkReviewFields.Checked = IsInRole(role, PropertySettings.PermissionAdminReviewField.Split(";"c))
                        chkEmailFiles.Checked = IsInRole(role, PropertySettings.PermissionAdminEmailFiles.Split(";"c))
                        chkLayoutFiles.Checked = IsInRole(role, PropertySettings.PermissionAdminLayoutFiles.Split(";"c))
                        chkLayoutSettings.Checked = IsInRole(role, PropertySettings.PermissionAdminLayoutSettings.Split(";"c))
                        chkTypes.Checked = IsInRole(role, PropertySettings.PermissionAdminTypes.Split(";"c))

                    End If

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace