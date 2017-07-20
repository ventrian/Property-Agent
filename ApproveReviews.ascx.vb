Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class ApproveReviews
        Inherits PropertyAgentBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCustomFieldManager As New CrumbInfo
            objCustomFieldManager.Caption = Localization.GetString("ApproveReviews", LocalResourceFile)
            objCustomFieldManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ApproveReviews")
            crumbs.Add(objCustomFieldManager)

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

        Private Sub BindReviews()

            Dim objReviewController As New ReviewController
            Dim objReviews As List(Of ReviewInfo) = objReviewController.List(ModuleId, Null.NullInteger, False)

            rptApproveReviews.DataSource = objReviews
            rptApproveReviews.DataBind()

            If (objReviews.Count = 0) Then
                lblNoReviews.Visible = True
                rptApproveReviews.Visible = False
                cmdApprove.Visible = False
                cmdReject.Visible = False
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetFields(ByVal reviewID As String) As String

            Dim objReviewFieldController As New ReviewFieldController()
            Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(ModuleId)

            Dim objReviewController As New ReviewController()
            Dim objReviewValues As List(Of ReviewValueInfo) = objReviewController.ListValue(Convert.ToInt32(reviewID))

            Dim fields As String = ""
            For Each objReviewValue As ReviewValueInfo In objReviewValues
                Dim title As String = ""
                For Each objReviewField As ReviewFieldInfo In objReviewFields
                    If (objReviewField.ReviewFieldID = objReviewValue.ReviewFieldID) Then
                        title = objReviewField.Name
                    End If
                Next
                If (fields = "") Then
                    fields = "<b>" & title & "</b>:&nbsp;" & objReviewValue.ReviewValue
                Else
                    fields = fields & "<br />" & "<b>" & title & "</b>:&nbsp;" & objReviewValue.ReviewValue
                End If
            Next

            Return fields

        End Function

        Protected Function GetPropertyLink(ByVal propertyID As String) As String

            Return NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=View", PropertySettings.SEOPropertyID & "=" & propertyID)

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
                    BindReviews()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptApproveReviews_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptApproveReviews.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objReview As ReviewInfo = CType(e.Item.DataItem, ReviewInfo)
                If (objReview IsNot Nothing) Then
                    Dim chkSelected As CheckBox = CType(e.Item.FindControl("chkSelected"), CheckBox)
                    chkSelected.Attributes.Add("ReviewID", objReview.ReviewID.ToString())
                End If

            End If

        End Sub

        Protected Sub cmdReject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReject.Click

            For Each item As RepeaterItem In rptApproveReviews.Items
                If (item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem) Then
                    Dim chkSelected As CheckBox = CType(item.FindControl("chkSelected"), CheckBox)
                    If Not (chkSelected Is Nothing) Then
                        If (chkSelected.Checked) Then
                            Dim reviewID As Integer = Convert.ToInt32(chkSelected.Attributes("ReviewID").ToString())
                            Dim objReviewController As New ReviewController()
                            Dim objReviewValues As List(Of ReviewValueInfo) = objReviewController.ListValue(reviewID)
                            For Each objReviewValue As ReviewValueInfo In objReviewValues
                                objReviewController.DeleteValue(objReviewValue.ReviewValueID)
                            Next
                            objReviewController.Delete(reviewID)
                        End If
                    End If
                End If
            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

        Protected Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click

            For Each item As RepeaterItem In rptApproveReviews.Items
                If (item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem) Then
                    Dim chkSelected As CheckBox = CType(item.FindControl("chkSelected"), CheckBox)
                    If Not (chkSelected Is Nothing) Then
                        If (chkSelected.Checked) Then
                            Dim reviewID As Integer = Convert.ToInt32(chkSelected.Attributes("ReviewID").ToString())

                            Dim objReviewController As New ReviewController()
                            Dim objReviews As List(Of ReviewInfo) = objReviewController.List(ModuleId, Null.NullInteger, False)

                            For Each objReview As ReviewInfo In objReviews
                                If (objReview.ReviewID = reviewID) Then
                                    objReviewController.Update(objReview.ReviewID, True)

                                    Dim ratings As New List(Of Integer)
                                    Dim objReviewFieldController As New ReviewFieldController()
                                    Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(ModuleId)
                                    Dim objReviewValues As List(Of ReviewValueInfo) = objReviewController.ListValue(reviewID)

                                    For Each objReviewValue As ReviewValueInfo In objReviewValues
                                        For Each objReviewField As ReviewFieldInfo In objReviewFields
                                            If (objReviewValue.ReviewFieldID = objReviewField.ReviewFieldID) Then
                                                If (objReviewField.FieldType = ReviewFieldType.Rating) Then
                                                    ratings.Add(Convert.ToInt32(objReviewValue.ReviewValue))
                                                End If
                                            End If
                                        Next
                                    Next

                                    If (ratings.Count > 0) Then

                                        Dim ratingValue As Integer = 0

                                        For Each rating As Integer In ratings
                                            ratingValue = ratingValue + rating
                                        Next

                                        Dim averageRating As Integer = ratingValue / ratings.Count

                                        Dim objRating As New RatingInfo
                                        objRating.CommentID = Null.NullInteger
                                        objRating.ReviewID = reviewID
                                        objRating.PropertyID = objReview.PropertyID
                                        objRating.CreateDate = DateTime.Now
                                        objRating.UserID = objReview.UserID
                                        objRating.Rating = averageRating

                                        Dim objRatingController As New RatingController()
                                        objRatingController.Add(objRating)

                                    End If

                                End If
                            Next
                        End If
                    End If
                End If
            Next

            Response.Redirect(Request.RawUrl, True)

        End Sub

#End Region

    End Class

End Namespace