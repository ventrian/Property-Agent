Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditTemplateDefinitions
        Inherits PropertyAgentBase

#Region " Private Methods "

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

        Private Sub BindDefinitions()

            Dim objTemplateController As New TemplateController

            Localization.LocalizeDataGrid(grdDefinitions, Me.LocalResourceFile)

            grdDefinitions.DataSource = objTemplateController.List(Null.NullInteger)
            grdDefinitions.DataBind()

            If (grdDefinitions.Items.Count = 0) Then
                grdDefinitions.Visible = False
                lblNoTemplates.Visible = True
            Else
                grdDefinitions.Visible = True
                lblNoTemplates.Visible = False
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetEditUrl(ByVal templateID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinition", "TemplateID=" & templateID)

        End Function

        Protected Function IsTemplateEditable() As Boolean

            Return Me.IsEditable And Me.PropertySettings.Template <> ""

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

                If (Page.IsPostBack = False) Then

                    cmdExportCurrentTemplate.Visible = (Me.PropertySettings.Template <> "")
                    BindDefinitions()

                End If

                cmdChangeCurrentTemplate.CssClass = PropertySettings.ButtonClass
                cmdExportCurrentTemplate.CssClass = PropertySettings.ButtonClass
                cmdImportNewTemplate.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Private Sub cmdImportNewTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImportNewTemplate.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ImportTemplateDefinition"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdExportCurrentTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExportCurrentTemplate.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ExportTemplateDefinition"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdChangeCurrentTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeCurrentTemplate.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ChangeTemplate"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace