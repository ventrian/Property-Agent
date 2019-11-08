Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Permissions

Namespace Ventrian.PropertyAgent

    Partial Public Class LatestOptions
        Inherits ModuleSettingsBase

#Region " Private Members "

        Private _customFields As List(Of CustomFieldInfo)
        Private _propertySettingsLatest As PropertySettingsLatest

#End Region

#Region " Private Properties "

        Private ReadOnly Property CustomFields() As List(Of CustomFieldInfo)
            Get
                If (_customFields Is Nothing) Then
                    Dim moduleID As Integer = Null.NullInteger

                    If (drpModuleID.SelectedValue <> "-1") Then
                        Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))
                        moduleID = Convert.ToInt32(values(1))
                    End If

                    Dim objCustomFieldController As New CustomFieldController
                    _customFields = objCustomFieldController.List(moduleID, True)
                End If
                Return _customFields
            End Get
        End Property

        Public ReadOnly Property PropertySettingsLatest() As PropertySettingsLatest
            Get
                If (_propertySettingsLatest Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettingsLatest = New PropertySettingsLatest(Me.Settings)
                End If
                Return _propertySettingsLatest
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindAgents()

            drpSpecificUser.DataSource = UserController.GetUsers(PortalId)
            drpSpecificUser.DataBind()

            drpSpecificUser.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("None_Specified"), "-1"))

        End Sub

        Private Sub BindModules()

            Dim objDesktopModuleController As New DesktopModuleController
            Dim objDesktopModuleInfo As DesktopModuleInfo = objDesktopModuleController.GetDesktopModuleByModuleName("PropertyAgent", PortalId)

            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As IList = objTabController.GetTabsByPortal(PortalId)
                For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs
                    If Not (objTab Is Nothing) Then
                        If (objTab.IsDeleted = False) Then
                            Dim objModules As New ModuleController
                            For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                Dim objModule As ModuleInfo = pair.Value
                                If (objModule.IsDeleted = False) Then
                                    If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                        If PortalSecurity.IsInRoles(ModulePermissionController.CanEditModuleContent(objModule)) = True And objModule.IsDeleted = False Then
                                            Dim strPath As String = objTab.TabName
                                            Dim objTabSelected As TabInfo = objTab
                                            While objTabSelected.ParentId <> Null.NullInteger
                                                objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, False)
                                                If (objTabSelected Is Nothing) Then
                                                    Exit While
                                                End If
                                                strPath = objTabSelected.TabName & " -> " & strPath
                                            End While

                                            Dim objListItem As New ListItem

                                            objListItem.Value = objModule.TabID.ToString() & "-" & objModule.ModuleID.ToString()
                                            objListItem.Text = strPath & " -> " & objModule.ModuleTitle

                                            drpModuleID.Items.Add(objListItem)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

                drpModuleID.Items.Insert(0, New ListItem(Localization.GetString("SelectModule", Me.LocalResourceFile), "-1"))

            End If
        End Sub

        Private Sub BindSortBy()

            drpSortBy.Items.Clear()

            For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)

                If (objSortByType = SortByType.CustomField) Then

                    Dim moduleID As Integer = Null.NullInteger
                    If (drpModuleID.SelectedValue <> "-1") Then
                        Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))
                        moduleID = Convert.ToInt32(values(1))
                    End If

                    Dim objCustomFieldController As New CustomFieldController
                    Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(moduleID, True)

                    For Each objCustomField As CustomFieldInfo In objCustomFields
                        If (objCustomField.IsSortable) Then
                            Dim li As New ListItem
                            li.Value = "cf" & objCustomField.CustomFieldID.ToString()
                            li.Text = objCustomField.Caption
                            drpSortBy.Items.Add(li)
                        End If
                    Next

                Else

                    If (objSortByType = SortByType.ReviewField) Then

                        Dim moduleID As Integer = Null.NullInteger
                        If (drpModuleID.SelectedValue <> "-1") Then
                            Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))
                            moduleID = Convert.ToInt32(values(1))
                        End If

                        Dim objReviewFieldController As New ReviewFieldController
                        Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(moduleID)

                        For Each objReviewField As ReviewFieldInfo In objReviewFields
                            Dim li As New ListItem
                            li.Value = "rf" & objReviewField.ReviewFieldID.ToString()
                            li.Text = objReviewField.Caption
                            drpSortBy.Items.Add(li)
                        Next

                    Else

                        Dim li As New ListItem
                        li.Value = System.Enum.GetName(GetType(SortByType), value)
                        li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                        drpSortBy.Items.Add(li)

                    End If
                End If
            Next

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

        End Sub

        Private Sub BindSortFields()

            For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)
                lstSortFields.Items.Add(New ListItem(Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile), System.Enum.GetName(GetType(SortByType), value)))
            Next

        End Sub

        Private Sub BindLayoutMode()

            For Each value As Integer In System.Enum.GetValues(GetType(LatestLayoutMode))
                Dim objLayoutMode As LatestLayoutMode = CType(System.Enum.Parse(GetType(LatestLayoutMode), value.ToString()), LatestLayoutMode)
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(LatestLayoutMode), value), Me.LocalResourceFile)
                lstLayoutMode.Items.Add(li)
            Next

            lstLayoutMode.Items(0).Selected = True

        End Sub

        Private Sub BindLayoutType()

            For Each value As Integer In System.Enum.GetValues(GetType(LatestLayoutType))
                Dim objLayoutMode As LatestLayoutMode = CType(System.Enum.Parse(GetType(LatestLayoutType), value.ToString()), LatestLayoutType)
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(LatestLayoutType), value), Me.LocalResourceFile)
                lstLayoutType.Items.Add(li)
            Next

            lstLayoutType.Items(1).Selected = True

        End Sub

        Private Sub BindTypes()

            drpTypes.Items.Clear()

            Dim objPropertyTypeController As New PropertyTypeController
            Dim moduleID As Integer = Null.NullInteger

            If (drpModuleID.SelectedValue <> "-1") Then
                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))
                moduleID = Convert.ToInt32(values(1))
            End If

            drpTypes.DataSource = objPropertyTypeController.ListAll(moduleID, True, PropertyTypeSortByType.Standard, Null.NullString())
            drpTypes.DataBind()

            drpTypes.Items.Insert(0, New ListItem(Localization.GetString("AllTypes", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub BindCustomFields()

            drpCustomFields.Items.Clear()

            txtCustomField.Visible = False
            drpCustomField.Visible = False
            chkCustomField.Visible = False
            chkCustomFieldList.Visible = False
            rdoCustomFieldList.Visible = False
            cmdAddCustomField.Visible = False

            Dim objCustomFieldController As New CustomFieldController()

            Dim moduleID As Integer = Null.NullInteger
            If (drpModuleID.SelectedValue <> "-1") Then
                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))
                moduleID = Convert.ToInt32(values(1))
            End If

            Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(moduleID, True)

            For Each objCustomField As CustomFieldInfo In objCustomFields
                If (objCustomField.FieldType <> CustomFieldType.FileUpload And objCustomField.FieldType <> CustomFieldType.Hyperlink And objCustomField.FieldType <> CustomFieldType.MultiLineTextBox And objCustomField.FieldType <> CustomFieldType.RichTextBox) Then
                    drpCustomFields.Items.Add(New ListItem(objCustomField.Name, objCustomField.CustomFieldID.ToString()))
                End If
            Next

            drpCustomFields.Items.Insert(0, New ListItem(Localization.GetString("SelectCustomField", Me.LocalResourceFile), "-1"))

            drpRelatedCustomFields.DataSource = objCustomFields
            drpRelatedCustomFields.DataBind()
            drpRelatedCustomFields.Items.Insert(0, New ListItem(Localization.GetString("SelectCustomField", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub BindCustomFilters()

            If (Me.PropertySettingsLatest.CustomFieldFilters <> "") Then
                grdCustomFilters.DataSource = Me.PropertySettingsLatest.CustomFieldFilters.Split(","c)
                grdCustomFilters.DataBind()
                grdCustomFilters.Visible = True
            Else
                grdCustomFilters.Visible = False
            End If

        End Sub

        Private Sub BindUserFilter()

            For Each value As Integer In System.Enum.GetValues(GetType(UserFilterType))
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(UserFilterType), value), Me.LocalResourceFile)
                lstUserFilter.Items.Add(li)
            Next

            lstUserFilter.Items(0).Selected = True

        End Sub

        Private Sub SaveCustomFilter(ByVal customFieldID As Integer, ByVal value As String)

            Dim objModuleController As New ModuleController

            If (Me.PropertySettingsLatest.CustomFieldFilters <> "") Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_CUSTOM_FIELD_FILTERS_SETTING, Me.PropertySettingsLatest.CustomFieldFilters & "," & customFieldID.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_CUSTOM_FIELD_VALUES_SETTING, Me.PropertySettingsLatest.CustomFieldValues & "," & value.Replace(","c, ""))
            Else
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_CUSTOM_FIELD_FILTERS_SETTING, customFieldID.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_CUSTOM_FIELD_VALUES_SETTING, value.Replace(","c, ""))
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetCustomFieldName(ByVal customFieldID As String) As String

            For Each objCustomField As CustomFieldInfo In CustomFields
                If (objCustomField.CustomFieldID.ToString() = customFieldID) Then
                    Return objCustomField.Name
                End If
            Next

            Return ""

        End Function

        Protected Function GetCustomFieldValue(ByVal fieldIndex As Integer) As String

            If ((Me.PropertySettingsLatest.CustomFieldValues.Split(","c).Length + 1) > fieldIndex) Then
                Return Me.PropertySettingsLatest.CustomFieldValues.Split(","c)(fieldIndex)
            End If

            Return ""

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                cmdStartDate.NavigateUrl = Calendar.InvokePopupCal(txtStartDate)

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpModuleID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpModuleID.SelectedIndexChanged

            Try

                BindSortBy()
                BindTypes()
                BindCustomFields()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub drpCustomFields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpCustomFields.SelectedIndexChanged

            txtCustomField.Visible = False
            drpCustomField.Visible = False
            chkCustomField.Visible = False
            chkCustomFieldList.Visible = False
            rdoCustomFieldList.Visible = False
            cmdAddCustomField.Visible = False

            If (drpCustomFields.SelectedValue <> "-1") Then

                Dim objCustomFieldController As New CustomFieldController
                Dim objCustomField As CustomFieldInfo = objCustomFieldController.Get(Convert.ToInt32(drpCustomFields.SelectedValue))

                If Not (objCustomField Is Nothing) Then
                    Select Case objCustomField.FieldType

                        Case CustomFieldType.OneLineTextBox
                            txtCustomField.Visible = True
                            cmdAddCustomField.Visible = True
                            Exit Select

                        Case CustomFieldType.DropDownList
                            drpCustomField.DataSource = objCustomField.FieldElements.Split("|"c)
                            drpCustomField.DataBind()
                            drpCustomField.Visible = True
                            cmdAddCustomField.Visible = True
                            Exit Select

                        Case CustomFieldType.CheckBox
                            chkCustomField.Visible = True
                            cmdAddCustomField.Visible = True
                            Exit Select

                        Case CustomFieldType.MultiCheckBox
                            chkCustomFieldList.DataSource = objCustomField.FieldElements.Split("|"c)
                            chkCustomFieldList.DataBind()
                            chkCustomFieldList.Visible = True
                            cmdAddCustomField.Visible = True
                            Exit Select

                        Case CustomFieldType.RadioButton
                            rdoCustomFieldList.DataSource = objCustomField.FieldElements.Split("|"c)
                            rdoCustomFieldList.DataBind()
                            rdoCustomFieldList.Visible = True
                            cmdAddCustomField.Visible = True
                            Exit Select

                    End Select
                End If

            End If

        End Sub

        Protected Sub cmdAddCustomField_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddCustomField.Click

            If (drpCustomFields.SelectedValue <> "-1") Then

                Dim objCustomFieldController As New CustomFieldController
                Dim objCustomField As CustomFieldInfo = objCustomFieldController.Get(Convert.ToInt32(drpCustomFields.SelectedValue))

                If Not (objCustomField Is Nothing) Then
                    Select Case objCustomField.FieldType

                        Case CustomFieldType.OneLineTextBox
                            If (txtCustomField.Text <> "") Then
                                SaveCustomFilter(objCustomField.CustomFieldID, txtCustomField.Text)
                            End If
                            Exit Select

                        Case CustomFieldType.DropDownList
                            If (drpCustomFields.Items.Count > 0) Then
                                SaveCustomFilter(objCustomField.CustomFieldID, drpCustomField.SelectedValue)
                            End If
                            Exit Select

                        Case CustomFieldType.CheckBox
                            SaveCustomFilter(objCustomField.CustomFieldID, chkCustomField.Checked.ToString())
                            Exit Select

                        Case CustomFieldType.MultiCheckBox
                            Dim values As String = ""
                            For Each objListItem As ListItem In chkCustomFieldList.Items
                                If (objListItem.Selected) Then
                                    If (values = "") Then
                                        values = objListItem.Text
                                    Else
                                        values = values & "|" & objListItem.Text
                                    End If
                                End If
                            Next
                            SaveCustomFilter(objCustomField.CustomFieldID, values)
                            Exit Select

                        Case CustomFieldType.RadioButton
                            If (rdoCustomFieldList.Items.Count > 0) Then
                                If Not (rdoCustomFieldList.SelectedItem Is Nothing) Then
                                    SaveCustomFilter(objCustomField.CustomFieldID, rdoCustomFieldList.SelectedValue)
                                End If
                            End If
                            Exit Select

                    End Select
                End If

                Dim objModuleController As New ModuleController
                Me.PropertySettingsLatest.SetSettings(objModuleController.GetModuleSettings(Me.ModuleId))
                BindCustomFilters()

            End If

        End Sub

        Protected Sub grdCustomFilters_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdCustomFilters.ItemCommand

            If (e.CommandName.ToLower() = "delete") Then

                Dim newCustomFieldIDs As String = ""
                Dim newCustomFieldValues As String = ""

                For i As Integer = 0 To Me.PropertySettingsLatest.CustomFieldFilters.Split(","c).Length - 1
                    If (i <> e.Item.ItemIndex) Then
                        If (newCustomFieldIDs = "") Then
                            newCustomFieldIDs = Me.PropertySettingsLatest.CustomFieldFilters.Split(","c)(i)
                            newCustomFieldValues = Me.PropertySettingsLatest.CustomFieldValues.Split(","c)(i)
                        Else
                            newCustomFieldIDs = newCustomFieldIDs & "," & Me.PropertySettingsLatest.CustomFieldFilters.Split(","c)(i)
                            newCustomFieldValues = newCustomFieldValues & "," & Me.PropertySettingsLatest.CustomFieldValues.Split(","c)(i)
                        End If
                    End If
                Next

                Dim objModuleController As New ModuleController
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_CUSTOM_FIELD_FILTERS_SETTING, newCustomFieldIDs)
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_CUSTOM_FIELD_VALUES_SETTING, newCustomFieldValues)
                Me.PropertySettingsLatest.SetSettings(objModuleController.GetModuleSettings(Me.ModuleId))
                BindCustomFilters()

            End If

        End Sub

        Protected Sub lstUserFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstUserFilter.SelectedIndexChanged

            Dim objUserFilterType As UserFilterType = CType(System.Enum.Parse(GetType(UserFilterType), lstUserFilter.SelectedValue), UserFilterType)

            trOwner.Visible = False
            trParameter.Visible = False
            drpSpecificUser.Items.Clear()

            Select Case objUserFilterType

                Case UserFilterType.None
                    ' Do Nothing
                    Exit Select

                Case UserFilterType.Current
                    ' Do Nothing yet
                    Exit Select

                Case UserFilterType.Parameter
                    trParameter.Visible = True
                    Exit Select

                Case UserFilterType.Specific
                    trOwner.Visible = True
                    BindAgents()
                    Exit Select

            End Select

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            Try
                If (Page.IsPostBack = False) Then

                    BindSortDirection()
                    BindLayoutMode()
                    BindLayoutType()
                    BindUserFilter()
                    BindModules()

                    If (Settings.Contains(Constants.LATEST_MODULE_ID_SETTING) And Settings.Contains(Constants.LATEST_TAB_ID_SETTING)) Then
                        If Not (drpModuleID.Items.FindByValue(Settings(Constants.LATEST_TAB_ID_SETTING).ToString() & "-" & Settings(Constants.LATEST_MODULE_ID_SETTING).ToString()) Is Nothing) Then
                            drpModuleID.SelectedValue = Settings(Constants.LATEST_TAB_ID_SETTING).ToString() & "-" & Settings(Constants.LATEST_MODULE_ID_SETTING).ToString()
                        End If
                    End If

                    BindSortBy()
                    BindCustomFields()
                    BindTypes()
                    BindSortFields()

                    ckkPropertyIDinURL.Checked = Me.PropertySettingsLatest.PropertyIDinURL

                    chkBubbleFeatured.Checked = Me.PropertySettingsLatest.Bubblefeatured

                    If Not (drpTypes.Items.FindByValue(Me.PropertySettingsLatest.TypeID.ToString()) Is Nothing) Then
                        drpTypes.SelectedValue = Me.PropertySettingsLatest.TypeID.ToString()
                    End If

                    chkShowFeaturedOnly.Checked = Me.PropertySettingsLatest.FeaturedOnly
                    chkShowShortListOnly.Checked = Me.PropertySettingsLatest.ShowShortList
                    txtItemsPerRow.Text = Me.PropertySettingsLatest.ItemsPerRow.ToString()
                    txtMaxNumber.Text = Me.PropertySettingsLatest.MaxNumber.ToString()
                    txtPageSize.Text = Me.PropertySettingsLatest.PageSize.ToString()
                    chkEnablePager.Checked = Me.PropertySettingsLatest.EnablePager
                    chkShowRelated.Checked = Me.PropertySettingsLatest.ShowRelated
                    If (drpRelatedCustomFields.Items.FindByValue(Me.PropertySettingsLatest.RelatedCustomField.ToString()) IsNot Nothing) Then
                        drpRelatedCustomFields.SelectedValue = Me.PropertySettingsLatest.RelatedCustomField.ToString()
                    End If

                    If (Me.PropertySettingsLatest.StartDate <> Null.NullDate) Then
                        txtStartDate.Text = Me.PropertySettingsLatest.StartDate.ToShortDateString()
                    End If
                    If (Me.PropertySettingsLatest.MinAge <> Null.NullInteger) Then
                        txtMinAge.Text = Me.PropertySettingsLatest.MinAge.ToString()
                    End If
                    If (Me.PropertySettingsLatest.MaxAge <> Null.NullInteger) Then
                        txtMaxAge.Text = Me.PropertySettingsLatest.MaxAge.ToString()
                    End If

                    If Not (lstUserFilter.Items.FindByValue(Convert.ToInt32(Me.PropertySettingsLatest.UserFilter).ToString()) Is Nothing) Then
                        lstUserFilter.SelectedValue = Convert.ToInt32(Me.PropertySettingsLatest.UserFilter).ToString()
                    End If

                    If (Me.PropertySettingsLatest.UserFilter = UserFilterType.Specific) Then
                        trOwner.Visible = True
                        BindAgents()
                        If (drpSpecificUser.Items.FindByValue(Me.PropertySettingsLatest.UserFilterSpecific.ToString()) IsNot Nothing) Then
                            drpSpecificUser.SelectedValue = Me.PropertySettingsLatest.UserFilterSpecific.ToString()
                        End If
                    End If

                    txtUserParameter.Text = Me.PropertySettingsLatest.UserFilterParameter

                    If (Me.PropertySettingsLatest.UserFilter = UserFilterType.Parameter) Then
                        trParameter.Visible = True
                    End If

                    If (Me.PropertySettingsLatest.SortBy = SortByType.CustomField) Then
                        If Not (drpSortBy.Items.FindByValue("cf" & Me.PropertySettingsLatest.SortByCustomField) Is Nothing) Then
                            drpSortBy.SelectedValue = "cf" & Me.PropertySettingsLatest.SortByCustomField
                        End If
                    Else
                        If (Me.PropertySettingsLatest.SortBy = SortByType.ReviewField) Then
                            If Not (drpSortBy.Items.FindByValue("rf" & Me.PropertySettingsLatest.SortByCustomField) Is Nothing) Then
                                drpSortBy.SelectedValue = "rf" & Me.PropertySettingsLatest.SortByCustomField
                            End If
                        Else
                            If Not (drpSortBy.Items.FindByValue(Me.PropertySettingsLatest.SortBy.ToString()) Is Nothing) Then
                                drpSortBy.SelectedValue = Me.PropertySettingsLatest.SortBy.ToString()
                            End If
                        End If
                    End If

                    If Not (drpSortDirection.Items.FindByValue(Me.PropertySettingsLatest.SortDirection.ToString()) Is Nothing) Then
                        drpSortDirection.SelectedValue = Me.PropertySettingsLatest.SortDirection.ToString()
                    End If

                    chkUserSortable.Checked = Me.PropertySettingsLatest.UserSortable
                    For Each li As ListItem In lstSortFields.Items
                        For Each item As String In PropertySettingsLatest.UserSortableFields.Split(","c)
                            If (li.Value = item) Then
                                li.Selected = True
                            End If
                        Next
                    Next

                    If Not (lstLayoutMode.Items.FindByValue(Convert.ToInt32(Me.PropertySettingsLatest.LayoutMode).ToString()) Is Nothing) Then
                        lstLayoutMode.SelectedValue = Convert.ToInt32(Me.PropertySettingsLatest.LayoutMode).ToString()
                    End If
                    trIncludeStylesheet.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 0)
                    trHeader.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                    trItem.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                    trFooter.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                    trEmpty.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)

                    If Not (lstLayoutType.Items.FindByValue(Convert.ToInt32(Me.PropertySettingsLatest.LayoutType).ToString()) Is Nothing) Then
                        lstLayoutType.SelectedValue = Convert.ToInt32(Me.PropertySettingsLatest.LayoutType).ToString()
                    End If
                    trItemsPerRow.Visible = (Convert.ToInt32(lstLayoutType.SelectedValue) = 1)

                    chkIncludeStylesheet.Checked = Me.PropertySettingsLatest.IncludeStylesheet
                    txtHeader.Text = Me.PropertySettingsLatest.LayoutHeader
                    txtItem.Text = Me.PropertySettingsLatest.LayoutItem
                    txtFooter.Text = Me.PropertySettingsLatest.LayoutFooter
                    txtEmpty.Text = Me.PropertySettingsLatest.LayoutEmpty

                    BindCustomFilters()

                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Public Overrides Sub UpdateSettings()

            Try

                Dim objModuleController As New ModuleController

                If (drpModuleID.Items.Count > 0) Then
                    If (drpModuleID.SelectedValue <> "-1") Then
                        Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))
                        If (values.Length = 2) Then
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_TAB_ID_SETTING, values(0))
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.LATEST_MODULE_ID_SETTING, values(1))
                        End If
                    End If
                End If

                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_PROPERTY_ID_IN_URL_SETTING, ckkPropertyIDinURL.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_BUBBLE_FEATURED_SETTING, chkBubbleFeatured.Checked.ToString())

                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_TYPE_ID_SETTING, drpTypes.SelectedValue)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_FEATURED_ONLY_SETTING, chkShowFeaturedOnly.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_SHOW_SHORTLIST_SETTING, chkShowShortListOnly.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_MAX_NUMBER_SETTING, txtMaxNumber.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_PAGE_SIZE_SETTING, txtPageSize.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_ENABLE_PAGER_SETTING, chkEnablePager.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_ITEMS_PER_ROW_SETTING, txtItemsPerRow.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_SHOW_RELATED_SETTING, chkShowRelated.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_RELATED_CUSTOM_FIELD_SETTING, drpRelatedCustomFields.SelectedValue.ToString())

                If (txtStartDate.Text <> "") Then
                    Dim objStartDate As DateTime = DateTime.Parse(txtStartDate.Text)
                    objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_START_DATE_SETTING, objStartDate.Year.ToString() + "-" + objStartDate.Month.ToString() + "-" + objStartDate.Day.ToString())
                Else
                    objModuleController.DeleteModuleSetting(ModuleId, Constants.LATEST_START_DATE_SETTING)
                End If
                If (txtMinAge.Text <> "") Then
                    objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_MIN_AGE_SETTING, txtMinAge.Text)
                Else
                    objModuleController.DeleteModuleSetting(ModuleId, Constants.LATEST_MIN_AGE_SETTING)
                End If
                If (txtMaxAge.Text <> "") Then
                    objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_MAX_AGE_SETTING, txtMaxAge.Text)
                Else
                    objModuleController.DeleteModuleSetting(ModuleId, Constants.LATEST_MAX_AGE_SETTING)
                End If
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_USER_FILTER_SETTING, lstUserFilter.SelectedValue)

                If (trOwner.Visible) Then
                    objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_USER_FILTER_SPECIFIC_SETTING, drpSpecificUser.SelectedValue)
                End If

                If (trParameter.Visible) Then
                    objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_USER_FILTER_PARAMETER_SETTING, txtUserParameter.Text)
                End If

                Dim objSortByType As SortByType = SortByType.CustomField
                Dim sortByID As Integer = Null.NullInteger
                If (drpSortBy.SelectedValue.StartsWith("cf")) Then
                    sortByID = Convert.ToInt32(drpSortBy.SelectedValue.Replace("cf", ""))
                Else
                    If (drpSortBy.SelectedValue.StartsWith("rf")) Then
                        sortByID = Convert.ToInt32(drpSortBy.SelectedValue.Replace("rf", ""))
                        objSortByType = SortByType.ReviewField
                    Else
                        objSortByType = CType(System.Enum.Parse(GetType(SortByType), drpSortBy.SelectedValue.ToString()), SortByType)
                    End If
                End If

                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_SORT_BY_SETTING, objSortByType.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_SORT_BY_CUSTOM_FIELD_SETTING, sortByID.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_SORT_DIRECTION_SETTING, drpSortDirection.SelectedValue)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_USER_SORTABLE_SETTING, chkUserSortable.Checked.ToString())
                Dim sortFields As String = ""
                For Each li As ListItem In lstSortFields.Items
                    If (li.Selected) Then
                        If (sortFields = "") Then
                            sortFields = li.Value
                        Else
                            sortFields = sortFields & "," & li.Value
                        End If
                    End If
                Next
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_USER_SORTABLE_FIELDS_SETTING, sortFields)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_LAYOUT_MODE_SETTING, lstLayoutMode.SelectedValue)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_LAYOUT_TYPE_SETTING, lstLayoutType.SelectedValue)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_LAYOUT_INCLUDE_STYLESHEET_SETTING, chkIncludeStylesheet.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_LAYOUT_HEADER_SETTING, txtHeader.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_LAYOUT_ITEM_SETTING, txtItem.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_LAYOUT_FOOTER_SETTING, txtFooter.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.LATEST_LAYOUT_EMPTY_SETTING, txtEmpty.Text)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub lstLayoutMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLayoutMode.SelectedIndexChanged

            Try

                trIncludeStylesheet.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 0)
                trHeader.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                trItem.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                trFooter.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                trEmpty.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub lstLayoutType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLayoutType.SelectedIndexChanged

            Try

                trItemsPerRow.Visible = (Convert.ToInt32(lstLayoutType.SelectedValue) = 1)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace