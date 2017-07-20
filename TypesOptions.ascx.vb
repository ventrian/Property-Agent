Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports DotNetNuke.Entities.Users

Namespace Ventrian.PropertyAgent

    Partial Public Class TypesOptions
        Inherits ModuleSettingsBase

#Region " Private Members "

        Private _propertySettingsTypes As PropertySettingsTypes

#End Region

#Region " Private Properties "

        Private Property Agents() As String
            Get
                Return ViewState("PA-Agents-Filter").ToString()
            End Get
            Set(ByVal value As String)
                ViewState("PA-Agents-Filter") = value
            End Set
        End Property

        Public ReadOnly Property PropertySettingsTypes() As PropertySettingsTypes
            Get
                If (_propertySettingsTypes Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettingsTypes = New PropertySettingsTypes(Me.Settings)
                End If
                Return _propertySettingsTypes
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindAgents()

            Dim objAgents As New List(Of UserInfo)

            For Each ID As String In Agents.Split(","c)
                If (ID <> "") Then
                    Dim objUser As UserInfo = UserController.GetUser(Me.PortalId, ID, True)
                    If (objUser IsNot Nothing) Then
                        objAgents.Add(objUser)
                    End If
                End If
            Next

            If (objAgents.Count > 0) Then
                rptAgents.DataSource = objAgents
                rptAgents.DataBind()
                lblNoAgentFitler.Visible = False
                rptAgents.Visible = True
            Else
                lblNoAgentFitler.Visible = True
                rptAgents.Visible = False
            End If

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

        Private Sub BindModules()

            Dim objDesktopModuleController As New DesktopModuleController
            Dim objDesktopModuleInfo As DesktopModuleInfo = objDesktopModuleController.GetDesktopModuleByModuleName("PropertyAgent")

            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As ArrayList = objTabController.GetTabs(PortalId)
                For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs
                    If Not (objTab Is Nothing) Then
                        If (objTab.IsDeleted = False) Then
                            Dim objModules As New ModuleController
                            For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                Dim objModule As ModuleInfo = pair.Value
                                If (objModule.IsDeleted = False) Then
                                    If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                        If PortalSecurity.IsInRoles(objModule.AuthorizedEditRoles) = True And objModule.IsDeleted = False Then
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

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            Try

                If (Page.IsPostBack = False) Then

                    Dim moduleID As Integer = Null.NullInteger

                    BindModules()
                    BindLayoutMode()

                    For Each value As Integer In System.Enum.GetValues(GetType(PropertyTypeSortByType))
                        Dim li As New ListItem
                        li.Value = System.Enum.GetName(GetType(PropertyTypeSortByType), value)
                        li.Text = Localization.GetString(System.Enum.GetName(GetType(PropertyTypeSortByType), value), Me.LocalResourceFile)
                        drpTypesSortBy.Items.Add(li)
                    Next

                    If (Settings.Contains(Constants.TYPES_MODULE_ID_SETTING) And Settings.Contains(Constants.TYPES_TAB_ID_SETTING)) Then
                        If Not (drpModuleID.Items.FindByValue(Settings(Constants.TYPES_TAB_ID_SETTING).ToString() & "-" & Settings(Constants.TYPES_MODULE_ID_SETTING).ToString()) Is Nothing) Then
                            drpModuleID.SelectedValue = Settings(Constants.TYPES_TAB_ID_SETTING).ToString() & "-" & Settings(Constants.TYPES_MODULE_ID_SETTING).ToString()
                            moduleID = Convert.ToInt32(Settings(Constants.TYPES_MODULE_ID_SETTING).ToString())
                        End If
                    End If

                    Agents = Me.PropertySettingsTypes.AgentFilter
                    BindAgents()

                    If (moduleID <> Null.NullInteger) Then
                        Dim objTypeController As New PropertyTypeController()
                        rptTypesFilter.DataSource = objTypeController.ListAll(moduleID, True, PropertyTypeSortByType.Standard, "")
                        rptTypesFilter.DataBind()
                    End If

                    If Not (drpTypesSortBy.Items.FindByValue(Me.PropertySettingsTypes.TypesSortBy.ToString()) Is Nothing) Then
                        drpTypesSortBy.SelectedValue = Me.PropertySettingsTypes.TypesSortBy.ToString()
                    End If

                    If Not (lstLayoutMode.Items.FindByValue(Convert.ToInt32(Me.PropertySettingsTypes.LayoutMode).ToString()) Is Nothing) Then
                        lstLayoutMode.SelectedValue = Convert.ToInt32(Me.PropertySettingsTypes.LayoutMode).ToString()
                    End If
                    trIncludeStylesheet.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 0)
                    trHeader.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                    trItem.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)
                    trFooter.Visible = (Convert.ToInt32(lstLayoutMode.SelectedValue) = 1)

                    chkHideZero.Checked = Me.PropertySettingsTypes.HideZeroCount
                    chkShowTopLevelOnly.Checked = Me.PropertySettingsTypes.ShowTopLevelOnly
                    chkShowAllTypes.Checked = Me.PropertySettingsTypes.ShowAllTypes

                    If (chkShowAllTypes.Checked = False) Then
                        trTypesFilter.Visible = True
                    End If

                    chkIncludeStylesheet.Checked = Me.PropertySettingsTypes.IncludeStylesheet
                    txtHeader.Text = Me.PropertySettingsTypes.LayoutHeader
                    txtItem.Text = Me.PropertySettingsTypes.LayoutItem
                    txtFooter.Text = Me.PropertySettingsTypes.LayoutFooter

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
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TYPES_TAB_ID_SETTING, values(0))
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TYPES_MODULE_ID_SETTING, values(1))
                        End If
                    End If
                End If

                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_SORT_BY_SETTING, drpTypesSortBy.SelectedValue)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_AGENT_FILTER_SETTING, Agents)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_HIDE_ZERO_SETTING, chkHideZero.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_SHOW_TOP_LEVEL_ONLY_SETTING, chkShowTopLevelOnly.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_SHOW_ALL_SETTING, chkShowAllTypes.Checked.ToString())

                Dim types As New List(Of Integer)
                For Each ri As RepeaterItem In rptTypesFilter.Items
                    If (ri.ItemType = ListItemType.Item Or ri.ItemType = ListItemType.AlternatingItem) Then
                        Dim chkSelected As CheckBox = ri.FindControl("chkSelected")
                        If (chkSelected IsNot Nothing) Then
                            If (chkSelected.Checked) Then
                                types.Add(Convert.ToInt32(chkSelected.Attributes("TypeID").ToString()))
                            End If
                        End If
                    End If
                Next

                Dim typesString As String = ""
                For Each i As Integer In types
                    If (typesString = "") Then
                        typesString = i.ToString()
                    Else
                        typesString = typesString & "," & i.ToString()
                    End If
                Next
                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_FILTER_SETTING, typesString)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_MODE_SETTING, lstLayoutMode.SelectedValue)

                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_INCLUDE_STYLESHEET_SETTING, chkIncludeStylesheet.Checked.ToString())
                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_HEADER_SETTING, txtHeader.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_ITEM_SETTING, txtItem.Text)
                objModuleController.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_FOOTER_SETTING, txtFooter.Text)

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

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdAddAgent_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddAgent.Click

            Try

                If (txtUsername.Text.Length > 0) Then
                    Dim objUser As UserInfo = UserController.GetUserByName(Me.PortalId, txtUsername.Text)
                    If (objUser IsNot Nothing) Then
                        If (Agents = "") Then
                            Agents = objUser.UserID.ToString()
                        Else
                            Dim doAdd As Boolean = True
                            For Each ID As String In Agents.Split(","c)
                                If (ID = objUser.UserID.ToString()) Then
                                    doAdd = False
                                End If
                            Next
                            If (doAdd) Then
                                Agents = Agents & "," & objUser.UserID.ToString()
                            End If
                        End If
                    End If
                    BindAgents()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub rptTypesFilter_ItemDataBound(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptTypesFilter.ItemDataBound

            Try

                If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                    Dim objType As PropertyTypeInfo = CType(e.Item.DataItem, PropertyTypeInfo)
                    Dim chkSelected As CheckBox = e.Item.FindControl("chkSelected")

                    If (chkSelected IsNot Nothing) Then
                        chkSelected.Attributes("TypeID") = objType.PropertyTypeID.ToString()

                        If (Settings.Contains(Constants.TYPES_LAYOUT_FILTER_SETTING)) Then
                            Dim types As String = Settings(Constants.TYPES_LAYOUT_FILTER_SETTING).ToString()

                            For Each t As String In types.Split(","c)
                                If (t = objType.PropertyTypeID.ToString()) Then
                                    chkSelected.Checked = True
                                    Exit For
                                End If
                            Next
                        Else
                            chkSelected.Checked = True
                        End If
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub rptAgents_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptAgents.ItemCommand

        End Sub

        Protected Sub chkShowAllTypes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkShowAllTypes.CheckedChanged

            trTypesFilter.Visible = Not chkShowAllTypes.Checked

        End Sub

#End Region

    End Class

End Namespace
