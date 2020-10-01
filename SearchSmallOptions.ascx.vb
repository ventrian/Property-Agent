Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security.Permissions

Namespace Ventrian.PropertyAgent

    Partial Public Class SearchSmallOptions
        Inherits ModuleSettingsBase

#Region " Private Members "

        Private _propertySettingsSearch As PropertySettingsSearch

#End Region

#Region " Private Properties "

        Public ReadOnly Property PropertySettingsSearch() As PropertySettingsSearch
            Get
                If (_propertySettingsSearch Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettingsSearch = New PropertySettingsSearch(Me.Settings)
                End If
                Return _propertySettingsSearch
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindModules()

            Dim objDesktopModuleController As New DesktopModuleController
            Dim objDesktopModuleInfo As DesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName("PropertyAgent", PortalId)

            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As TabCollection = TabController.Instance.GetTabsByPortal(PortalId)
                For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs.Values
                    If Not (objTab Is Nothing) Then
                        If (objTab.IsDeleted = False) Then
                            Dim objModules As New ModuleController
                            For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                Dim objModule As ModuleInfo = pair.Value
                                If (objModule.IsDeleted = False) Then
                                    If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                        If ModulePermissionController.CanEditModuleContent(objModule) = True And objModule.IsDeleted = False Then
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

        Private Sub BindCustomFields()

            Dim objCustomFieldController As New CustomFieldController
            If (drpModuleID.SelectedValue <> "-1") Then
                Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Convert.ToInt32(drpModuleID.SelectedValue.Split("-"c)(1)), True)

                Dim objSearchableFields As New ArrayList

                For Each objCustomField As CustomFieldInfo In objCustomFields
                    If (objCustomField.IsSearchable) Then
                        objSearchableFields.Add(objCustomField)
                    End If
                Next
                lstCustomFields.DataSource = objSearchableFields
                lstCustomFields.DataBind()
            End If

        End Sub

        Private Sub BindLayoutMode()

            For Each value As Integer In System.Enum.GetValues(GetType(SearchLayoutMode))
                Dim objLayoutMode As SearchLayoutMode = CType(System.Enum.Parse(GetType(SearchLayoutMode), value.ToString()), SearchLayoutMode)
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SearchLayoutMode), value), Me.LocalResourceFile)
                lstLayoutMode.Items.Add(li)
            Next

            lstLayoutMode.Items(0).Selected = True

        End Sub

        Private Sub BindSortBy()

            drpSortBy.Items.Clear()

            For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)

                If (objSortByType = SortByType.CustomField) Then

                    If (drpModuleID.SelectedValue <> "-1") Then
                        Dim objCustomFieldController As New CustomFieldController
                        Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Convert.ToInt32(drpModuleID.SelectedValue.Split("-"c)(1)), True)

                        For Each objCustomField As CustomFieldInfo In objCustomFields
                            If (objCustomField.IsSortable) Then
                                Dim li As New ListItem
                                li.Value = "cf" & objCustomField.CustomFieldID.ToString()
                                li.Text = objCustomField.Caption
                                drpSortBy.Items.Add(li)
                            End If
                        Next
                    End If

                Else
                    Dim li As New ListItem
                    li.Value = System.Enum.GetName(GetType(SortByType), value)
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                    drpSortBy.Items.Add(li)
                End If
            Next

            drpSortBy.Items.Insert(0, New ListItem(Localization.GetString("NotSpecified.Text", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub BindSortDirection()

            For Each value As Integer In System.Enum.GetValues(GetType(SortDirectionType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortDirectionType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortDirectionType), value), Me.LocalResourceFile)
                drpSortdirection.Items.Add(li)
            Next

            drpSortdirection.Items.Insert(0, New ListItem(Localization.GetString("NotSpecified.Text", Me.LocalResourceFile), "-1"))

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub drpModuleID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpModuleID.SelectedIndexChanged

            Try

                BindCustomFields()
                BindSortBy()

                drpSortBy.SelectedValue = "-1"
                drpSortDirection.SelectedValue = "-1"

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub lstLayoutMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLayoutMode.SelectedIndexChanged

            Try

                phLayoutStandard.Visible = (lstLayoutMode.SelectedValue = "0")
                phLayoutCustom.Visible = (lstLayoutMode.SelectedValue = "1")

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            Try

                If (Page.IsPostBack = False) Then

                    BindModules()
                    BindLayoutMode()

                    If (Settings.Contains(Constants.SEARCH_MODULE_ID_SETTING) And Settings.Contains(Constants.SEARCH_TAB_ID_SETTING)) Then
                        If Not (drpModuleID.Items.FindByValue(Settings(Constants.SEARCH_TAB_ID_SETTING).ToString() & "-" & Settings(Constants.SEARCH_MODULE_ID_SETTING).ToString()) Is Nothing) Then
                            drpModuleID.SelectedValue = Settings(Constants.SEARCH_TAB_ID_SETTING).ToString() & "-" & Settings(Constants.SEARCH_MODULE_ID_SETTING).ToString()
                        End If
                    End If

                    txtWidth.Text = Me.PropertySettingsSearch.Width.ToString()
                    txtCheckBoxListItemsPerRow.Text = Me.PropertySettingsSearch.CheckBoxItemsPerRow.ToString()
                    txtRadioButtonItemsPerRow.Text = Me.PropertySettingsSearch.RadioButtonItemsPerRow.ToString()

                    BindCustomFields()
                    BindSortBy()
                    BindSortDirection()

                    If (Settings.Contains(Constants.SEARCH_CUSTOM_FIELDS)) Then
                        For Each li As ListItem In lstCustomFields.Items
                            For Each item As String In Settings(Constants.SEARCH_CUSTOM_FIELDS).ToString().Split(","c)
                                If (item = li.Value) Then
                                    li.Selected = True
                                End If
                            Next
                        Next
                    End If

                    If (Settings.Contains(Constants.SEARCH_SMALL_SORT_BY_SETTING)) Then
                        If (Me.PropertySettingsSearch.SortBy = SortByType.CustomField) Then
                            If Not (drpSortBy.Items.FindByValue("cf" & Me.PropertySettingsSearch.SortByCustomField) Is Nothing) Then
                                drpSortBy.SelectedValue = "cf" & Me.PropertySettingsSearch.SortByCustomField
                            End If
                        Else
                            If Not (drpSortBy.Items.FindByValue(Me.PropertySettingsSearch.SortBy.ToString()) Is Nothing) Then
                                drpSortBy.SelectedValue = Me.PropertySettingsSearch.SortBy.ToString()
                            End If
                        End If
                    End If

                    If (Settings.Contains(Constants.SEARCH_SMALL_SORT_DIRECTION_SETTING)) Then
                        If (drpSortDirection.Items.FindByValue(Me.PropertySettingsSearch.SortDirection.ToString()) IsNot Nothing) Then
                            drpSortDirection.SelectedValue = Me.PropertySettingsSearch.SortDirection.ToString()
                        End If
                    End If

                    chkHideHelpIcon.Checked = Me.PropertySettingsSearch.HideHelpIcon
                    chkSearchWildcard.Checked = Me.PropertySettingsSearch.SearchWildcard
                    chkSearchTypes.Checked = Me.PropertySettingsSearch.SearchTypes
                    chkHideZero.Checked = Me.PropertySettingsSearch.HideZeroCount
                    chkHideTypeCount.Checked = Me.PropertySettingsSearch.HideTypeCount
                    chkSearchLocation.Checked = Me.PropertySettingsSearch.SearchLocation
                    chkSearchAgents.Checked = Me.PropertySettingsSearch.SearchAgents
                    chkSearchBrokers.Checked = Me.PropertySettingsSearch.SearchBrokers
                    txtSearchStyle.Text = Me.PropertySettingsSearch.SearchStyle
                    chkSplitRange.Checked = Me.PropertySettingsSearch.SplitRange

                    If Not (lstLayoutMode.Items.FindByValue(Convert.ToInt32(Me.PropertySettingsSearch.LayoutMode).ToString()) Is Nothing) Then
                        lstLayoutMode.SelectedValue = Convert.ToInt32(Me.PropertySettingsSearch.LayoutMode).ToString()
                    End If

                    phLayoutStandard.Visible = (lstLayoutMode.SelectedValue = "0")
                    phLayoutCustom.Visible = (lstLayoutMode.SelectedValue = "1")

                    txtSearchItem.Text = Me.PropertySettingsSearch.SearchTemplate

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
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_TAB_ID_SETTING, values(0))
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_MODULE_ID_SETTING, values(1))
                        End If
                    End If
                End If

                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_WIDTH_SETTING, txtWidth.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_RADIO_BUTTON_ITEMS_PER_ROW_SETTING, txtRadioButtonItemsPerRow.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_CHECKBOX_ITEMS_PER_ROW_SETTING, txtCheckBoxListItemsPerRow.Text)

                If (drpSortBy.SelectedValue = "-1") Then
                    objModuleController.DeleteModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_SORT_BY_SETTING)
                    objModuleController.DeleteModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_SORT_BY_CUSTOM_FIELD_SETTING)
                Else
                    Dim objSortByType As SortByType = SortByType.CustomField
                    Dim sortByID As Integer = Null.NullInteger
                    If (drpSortBy.SelectedValue.StartsWith("cf")) Then
                        sortByID = Convert.ToInt32(drpSortBy.SelectedValue.Replace("cf", ""))
                    Else
                        objSortByType = CType(System.Enum.Parse(GetType(SortByType), drpSortBy.SelectedValue.ToString()), SortByType)
                    End If

                    objModuleController.UpdateModuleSetting(ModuleId, Constants.SEARCH_SMALL_SORT_BY_SETTING, objSortByType.ToString())
                    objModuleController.UpdateModuleSetting(ModuleId, Constants.SEARCH_SMALL_SORT_BY_CUSTOM_FIELD_SETTING, sortByID.ToString())
                End If

                If (drpSortDirection.SelectedValue = "-1") Then
                    objModuleController.DeleteModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_SORT_DIRECTION_SETTING)
                Else
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_SORT_DIRECTION_SETTING, drpSortDirection.SelectedValue)
                End If

                Dim vals As String = ""
                For Each li As ListItem In lstCustomFields.Items
                    If (li.Selected) Then
                        If (vals.Length = 0) Then
                            vals = li.Value
                        Else
                            vals = vals & "," & li.Value
                        End If
                    End If
                Next

                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_CUSTOM_FIELDS, vals)
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_HIDE_HELP_ICON_SETTING, chkHideHelpIcon.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_WILDCARD_SETTING, chkSearchWildcard.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_TYPES_SETTING, chkSearchTypes.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_HIDE_ZERO_SETTING, chkHideZero.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_HIDE_TYPE_COUNT_SETTING, chkHideTypeCount.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_LOCATION_SETTING, chkSearchLocation.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_AGENTS_SETTING, chkSearchAgents.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_BROKERS_SETTING, chkSearchBrokers.Checked.ToString())
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SMALL_STYLE_SETTING, txtSearchStyle.Text)
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_SPLIT_RANGE_SETTING, chkSplitRange.Checked.ToString())

                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_LAYOUT_MODE_SETTING, lstLayoutMode.SelectedValue)
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SEARCH_LAYOUT_TEMPLATE_SETTING, txtSearchItem.Text)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
