Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class ExportTemplateDefinition
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

            Dim objExpportTemplate As New CrumbInfo
            objExpportTemplate.Caption = Localization.GetString("ExportTemplateDefinition", LocalResourceFile)
            objExpportTemplate.Url = Request.RawUrl.ToString()
            crumbs.Add(objExpportTemplate)

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

        Private Sub BindTemplate()

            If Not (Me.PropertySettings.TemplateInfo Is Nothing) Then
                txtTitle.Text = Me.PropertySettings.TemplateInfo.Title
                txtFolder.Text = Me.PropertySettings.TemplateInfo.Folder
                txtDescription.Text = Me.PropertySettings.TemplateInfo.Description
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

                If (Page.IsPostBack = False) Then
                    BindTemplate()
                End If

                cmdCancel.CssClass = PropertySettings.ButtonClass
                cmdExportTemplate.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdExportTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExportTemplate.Click

            Try

                If (Page.IsValid) Then
                    Dim objTemplateWriter As New TemplateWriter(Me.PortalSettings, Me.PropertySettings.Template, Me.ModuleId, txtTitle.Text, txtFolder.Text, txtDescription.Text, chkIncludeTypes.Checked)
                    objTemplateWriter.CreateTemplate()

                    If (Localization.GetString("TemplateExported", Me.LocalResourceFile).IndexOf("{0}") <> -1) Then
                        lblExported.Text = String.Format(Localization.GetString("TemplateExported", Me.LocalResourceFile), HostPath & txtFolder.Text & ".zip")
                    Else
                        lblExported.Text = Localization.GetString("TemplateExported", Me.LocalResourceFile)
                    End If
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

#End Region

    End Class

End Namespace