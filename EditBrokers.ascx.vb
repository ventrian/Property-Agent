Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.UserControls

Namespace Ventrian.PropertyAgent

    Partial Public Class EditBrokers
        Inherits PropertyAgentBase

#Region " Private Properties "

        Private ReadOnly Property CurrentBrokerID() As Integer
            Get
                If Not drpBrokers.SelectedValue Is Nothing AndAlso drpBrokers.SelectedValue <> "" Then
                    Return Convert.ToInt32(drpBrokers.SelectedValue)
                End If
                Return Null.NullInteger
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objTemplateDefinitions As New CrumbInfo
            objTemplateDefinitions.Caption = PropertyUtil.FormatPropertyLabel(Localization.GetString("EditBrokers", LocalResourceFile), Me.PropertySettings)
            objTemplateDefinitions.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditBrokers")
            crumbs.Add(objTemplateDefinitions)

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

        Private Sub BindDetails()

            BindBrokers()

            If drpBrokers.Items.Count > 0 Then
                drpBrokers.Enabled = True
                drpBrokers.Visible = True
                lblNoBrokers.Visible = False
                lstAvailable.Enabled = True
                lstSelected.Enabled = True
                cmdAdd.Enabled = True
                cmdRemove.Enabled = True
                BindOwners()
            Else
                drpBrokers.Enabled = False
                drpBrokers.Visible = False
                lblNoBrokers.Visible = True
                lstAvailable.Enabled = False
                lstSelected.Enabled = False
                cmdAdd.Enabled = False
                cmdRemove.Enabled = False
            End If

        End Sub

        Private Sub BindBrokers()

            Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)
            drpBrokers.DataSource = objAgentController.ListBrokers()
            drpBrokers.DataBind()

        End Sub

        Private Sub BindOwners()

            If CurrentBrokerID <> Null.NullInteger Then

                Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)

                ' load available agents
                lstAvailable.DataSource = objAgentController.ListAvailable(PortalId, ModuleId, PropertySettings.PermissionSubmit, CurrentBrokerID)
                lstAvailable.DataValueField = "UserId"
                lstAvailable.DataTextField = "DisplayName" '"FullName"
                lstAvailable.DataBind()

                'load selected agents
                lstSelected.DataSource = objAgentController.ListSelected(PortalId, ModuleId, CurrentBrokerID)
                lstSelected.DataValueField = "UserId"
                lstSelected.DataTextField = "DisplayName" '"FullName"
                lstSelected.DataBind()

            Else
                lstAvailable.Items.Clear()
                lstSelected.Items.Clear()
            End If

        End Sub

        Private Sub LocalizeLabels()

            cmdAdd.ToolTip = Localization.GetString("Add", LocalResourceFile)
            cmdRemove.ToolTip = Localization.GetString("Remove", LocalResourceFile)
            lblNoBrokers.Text = Localization.GetString("NoBrokesFound", LocalResourceFile)

            CType(dshEditBrokers, SectionHeadControl).Text = PropertyUtil.FormatPropertyLabel(GetResourceString("EditBrokers"), Me.PropertySettings)
            lblEditBrokersHelp.Text = PropertyUtil.FormatPropertyLabel(GetResourceString("EditBrokersHelp"), Me.PropertySettings)

            CType(plBrokers, LabelControl).Text = PropertyUtil.FormatPropertyLabel(GetResourceString("Broker"), Me.PropertySettings)
            CType(plBrokers, LabelControl).HelpText = GetResourceString("Broker.Help")

            CType(plOwners, LabelControl).Text = PropertyUtil.FormatPropertyLabel(GetResourceString("Owners"), Me.PropertySettings)
            CType(plOwners, LabelControl).HelpText = GetResourceString("Owners.Help")

        End Sub

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

                If IsPostBack = False Then
                    BindDetails()
                    Page.SetFocus(drpBrokers)
                End If

                cmdReturn.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                LocalizeLabels()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Protected Sub cmdReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturn.Click

            Try

                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub drpBrokers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpBrokers.SelectedIndexChanged

            Try

                BindOwners()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdAdd.Click

            Dim objListItem As ListItem

            Dim objList As ArrayList = New ArrayList

            For Each objListItem In lstAvailable.Items
                objList.Add(objListItem)
            Next

            For Each objListItem In objList
                If objListItem.Selected Then

                    ' Update listboxes
                    lstAvailable.Items.Remove(objListItem)
                    lstSelected.Items.Add(objListItem)

                    ' Update database
                    Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)
                    objAgentController.AddBroker(Convert.ToInt32(objListItem.Value), CurrentBrokerID, ModuleId)

                End If
            Next

            BindOwners()

        End Sub

        Protected Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRemove.Click

            Dim objListItem As ListItem

            Dim objList As ArrayList = New ArrayList

            For Each objListItem In lstSelected.Items
                objList.Add(objListItem)
            Next

            For Each objListItem In objList
                If objListItem.Selected Then

                    ' Update listboxes
                    lstSelected.Items.Remove(objListItem)
                    lstAvailable.Items.Add(objListItem)

                    ' Update database
                    Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)
                    objAgentController.DeleteBroker(Convert.ToInt32(objListItem.Value), CurrentBrokerID, ModuleId)

                End If
            Next

            lstAvailable.ClearSelection()
            lstSelected.ClearSelection()

        End Sub

#End Region

    End Class

End Namespace
