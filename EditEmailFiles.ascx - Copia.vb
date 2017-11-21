Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditEmailFiles
        Inherits PropertyAgentBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objLayoutFiles As New CrumbInfo
            objLayoutFiles.Caption = Localization.GetString("EditEmailFiles", LocalResourceFile)
            objLayoutFiles.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditEmailFiles")
            crumbs.Add(objLayoutFiles)

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

        Private Sub BindLayoutGroups()

            For Each value As Integer In System.Enum.GetValues(GetType(LayoutGroupType))
                If (value = LayoutGroupType.Contact Or value = LayoutGroupType.Submission Or value = LayoutGroupType.ContactBroker Or value = LayoutGroupType.ContactOwner Or value = LayoutGroupType.SendToFriend Or value = LayoutGroupType.CommentNotification Or value = LayoutGroupType.ReviewNotification) Then
                    Dim li As New ListItem
                    li.Value = System.Enum.GetName(GetType(LayoutGroupType), value)
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(LayoutGroupType), value), Me.LocalResourceFile)
                    drpLayoutGroups.Items.Add(li)
                End If
            Next

        End Sub

        Private Sub BindDetails()

            lblCurrentTemplate.Text = Me.PropertySettings.Template
            lblLayoutGroup.Text = Localization.GetString(drpLayoutGroups.SelectedValue & "Description", Me.LocalResourceFile)

            Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)
            Dim objLayoutGroup As LayoutGroupType = CType(System.Enum.Parse(GetType(LayoutGroupType), drpLayoutGroups.SelectedValue), LayoutGroupType)

            Select Case objLayoutGroup

                Case LayoutGroupType.CommentNotification

                    trSubject.Visible = True
                    trBody.Visible = True

                    txtSubject.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Subject_Html).Template
                    txtBody.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Body_Html).Template

                Case LayoutGroupType.Contact

                    trSubject.Visible = True
                    trBody.Visible = True

                    txtSubject.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Subject_Html).Template
                    txtBody.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Body_Html).Template

                Case LayoutGroupType.ContactBroker

                    trSubject.Visible = True
                    trBody.Visible = True

                    txtSubject.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactBroker_Subject_Html).Template
                    txtBody.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactBroker_Body_Html).Template

                Case LayoutGroupType.ContactOwner

                    trSubject.Visible = True
                    trBody.Visible = True

                    txtSubject.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactOwner_Subject_Html).Template
                    txtBody.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactOwner_Body_Html).Template

                Case LayoutGroupType.ReviewNotification

                    trSubject.Visible = True
                    trBody.Visible = True

                    txtSubject.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ReviewNotification_Subject_Html).Template
                    txtBody.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ReviewNotification_Body_Html).Template

                Case LayoutGroupType.SendToFriend

                    trSubject.Visible = True
                    trBody.Visible = True

                    txtSubject.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.SendToFriendEmail_Subject_Html).Template
                    txtBody.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.SendToFriendEmail_Body_Html).Template

                Case LayoutGroupType.Submission

                    trSubject.Visible = True
                    trBody.Visible = True

                    txtSubject.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Submission_Subject_Html).Template
                    txtBody.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Submission_Body_Html).Template

            End Select

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
                    BindLayoutGroups()
                    BindDetails()
                End If

                cmdUpdate.CssClass = PropertySettings.ButtonClass
                cmdCancel.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpTemplateGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpLayoutGroups.SelectedIndexChanged

            Try

                BindDetails()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)
                Dim objLayoutGroup As LayoutGroupType = CType(System.Enum.Parse(GetType(LayoutGroupType), drpLayoutGroups.SelectedValue), LayoutGroupType)

                Select Case objLayoutGroup

                    Case LayoutGroupType.CommentNotification
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Subject_Html, txtSubject.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Body_Html, txtBody.Text)

                    Case LayoutGroupType.Contact
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Subject_Html, txtSubject.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Body_Html, txtBody.Text)

                    Case LayoutGroupType.ContactBroker
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactBroker_Subject_Html, txtSubject.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactBroker_Body_Html, txtBody.Text)

                    Case LayoutGroupType.ContactOwner
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactOwner_Subject_Html, txtSubject.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactOwner_Body_Html, txtBody.Text)

                    Case LayoutGroupType.ReviewNotification
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ReviewNotification_Subject_Html, txtSubject.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ReviewNotification_Body_Html, txtBody.Text)

                    Case LayoutGroupType.SendToFriend
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.SendToFriendEmail_Subject_Html, txtSubject.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.SendToFriendEmail_Body_Html, txtBody.Text)

                    Case LayoutGroupType.Submission
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Submission_Subject_Html, txtSubject.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Submission_Body_Html, txtBody.Text)

                End Select

                lblEmailFilesUpdated.Visible = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL, True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace