Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class DeleteReview
        Inherits PropertyAgentControl

#Region " Private Properties "

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/DeleteReview"
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (Page.IsPostBack = False) Then

                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", ResourceFile) & "');")

            End If

            cmdDelete.CssClass = PropertySettings.ButtonClass

        End Sub

        Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objReviewController As New ReviewController
                Dim objReviewValues As List(Of ReviewValueInfo) = objReviewController.ListValue(CurrentReview.ReviewID)

                For Each objReviewValue As ReviewValueInfo In objReviewValues
                    objReviewController.DeleteValue(objReviewValue.ReviewValueID)
                Next

                objReviewController.Delete(CurrentReview.ReviewID)

                Dim objRatingController As New RatingController()
                objRatingController.Delete(CurrentReview.PropertyID, CurrentReview.ReviewID)

                Response.Redirect(Request.RawUrl, True)

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
