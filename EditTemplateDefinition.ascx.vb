Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.UserControls

Namespace Ventrian.PropertyAgent

    Partial Public Class EditTemplateDefinition
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _templateID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("TemplateID") Is Nothing) Then
                _templateID = Convert.ToInt32(Request("TemplateID"))
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objTemplateDefinitions As New CrumbInfo
            objTemplateDefinitions.Caption = Localization.GetString("EditTemplateDefinitions", LocalResourceFile)
            objTemplateDefinitions.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions")
            crumbs.Add(objTemplateDefinitions)

            Dim objTemplate As New CrumbInfo
            objTemplate.Caption = Localization.GetString("EditTemplateDefinition", LocalResourceFile)
            objTemplate.Url = Request.RawUrl.ToString()
            crumbs.Add(objTemplate)

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

            If (_templateID <> Null.NullInteger) Then

                Dim objTemplateController As New TemplateController

                Dim objTemplate As TemplateInfo = objTemplateController.Get(_templateID)

                If Not (objTemplate Is Nothing) Then

                    txtFolder.Text = objTemplate.Folder
                    txtTitle.Text = objTemplate.Title
                    txtDescription.Text = objTemplate.Description
                    chkPremium.Checked = objTemplate.IsPremium

                    Dim objPortals As New PortalController
                    Dim arrPortals As ArrayList = objPortals.GetPortals

                    Dim objTemplatePortalController As New TemplatePortalController
                    Dim arrTemplatePortals As ArrayList = objTemplatePortalController.List(_templateID)

                    For Each objTemplatePortal As TemplatePortalInfo In arrTemplatePortals
                        For Each objPortal As PortalInfo In arrPortals
                            If objPortal.PortalID = objTemplatePortal.PortalID Then
                                arrPortals.Remove(objPortal)
                                Exit For
                            End If
                        Next
                    Next

                    CType(ctlPortals, DualListControl).Available = arrPortals
                    CType(ctlPortals, DualListControl).Assigned = arrTemplatePortals
                    CType(ctlPortals, DualListControl).Visible = chkPremium.Checked

                    AdjustPortals()

                Else
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), True)
                End If

            Else
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), True)
            End If

        End Sub

        Private Sub AdjustPortals()

            If (chkPremium.Checked) Then
                ctlPortals.Visible = True
            Else
                ctlPortals.Visible = False
            End If

        End Sub

        Private Sub SaveDetails()

            Dim objTemplate As New TemplateInfo

            objTemplate.TemplateID = _templateID
            objTemplate.Folder = txtFolder.Text
            objTemplate.Title = txtTitle.Text
            objTemplate.Description = txtDescription.Text
            objTemplate.IsPremium = chkPremium.Checked

            Dim objTemplateController As New TemplateController
            objTemplateController.Update(objTemplate)

            Dim objTemplatePortalController As New TemplatePortalController
            objTemplatePortalController.Delete(_templateID)

            If (objTemplate.IsPremium) Then
                For Each objListItem As ListItem In CType(ctlPortals, DualListControl).Assigned
                    Dim objTemplatePortal As New TemplatePortalInfo

                    objTemplatePortal.PortalID = Integer.Parse(objListItem.Value)
                    objTemplatePortal.TemplateID = _templateID

                    objTemplatePortalController.Add(objTemplatePortal)
                Next
            End If

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

                ReadQueryString()

                If (Page.IsPostBack = False) Then
                    BindDetails()
                    Page.SetFocus(txtTitle)
                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")
                End If

                cmdCancel.CssClass = PropertySettings.ButtonClass
                cmdDelete.CssClass = PropertySettings.ButtonClass
                cmdUpdate.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub chkPremium_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPremium.CheckedChanged

            Try

                AdjustPortals()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then
                    SaveDetails()
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objTemplateController As New TemplateController
                Dim objTemplate As TemplateInfo = objTemplateController.Get(_templateID)

                If Not (objTemplate Is Nothing) Then
                    Try
                        System.IO.Directory.Delete(ApplicationMapPath & "\DesktopModules\PropertyAgent\Templates\" & objTemplate.Folder, True)
                    Catch
                    End Try
                End If

                Dim objTemplatePortalController As New TemplatePortalController
                objTemplatePortalController.Delete(_templateID)

                objTemplateController.Delete(_templateID)

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace