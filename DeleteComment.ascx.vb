Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class DeleteComment
        Inherits PropertyAgentControl

#Region " Private Properties "

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/DeleteComment"
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (Page.IsPostBack = False) Then

                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", ResourceFile) & "');")
                cmdDelete.CssClass = PropertySettings.ButtonClass

            End If

        End Sub

        Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objCommentController As New CommentController
                objCommentController.Delete(CurrentComment.CommentID)
                Response.Redirect(Request.RawUrl, True)

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
