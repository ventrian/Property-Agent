Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports System.IO
Imports DotNetNuke.Security

Namespace Ventrian.PropertyAgent

    Partial Public Class ReviewForm
        Inherits PropertyAgentControl

#Region " Private Members "

        Dim _commentFormFields As ArrayList

#End Region

#Region " Private Properties"

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/ReviewForm"
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindDetails()

            Dim objReviewController As New ReviewFieldController

            rptDetails.DataSource = objReviewController.List(Me.ModuleID)
            rptDetails.DataBind()

            tblReviewForm.Width = Unit.Pixel(PropertySettings.ReviewWidth)
            cmdSubmit.Text = Localization.GetString("cmdSubmit", ResourceFile)

        End Sub

        Private Function RenderControlAsString(ByVal objControl As Control) As String

            Dim sb As New StringBuilder
            Dim tw As New StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            objControl.RenderControl(hw)

            Return sb.ToString()

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                If (PropertySettings.ReviewAnonymous = False And Request.IsAuthenticated = False) Then
                    Me.Visible = False
                    Return
                End If

                If (Request.IsAuthenticated) Then
                    Dim objReviewController As New ReviewController()
                    Dim objReview As ReviewInfo = objReviewController.GetReview(CurrentProperty.PropertyID, UserController.GetCurrentUserInfo.UserID)

                    If (objReview IsNot Nothing) Then
                        Me.Visible = False
                        Return
                    End If
                Else
                    ' Do CHECK
                    Dim cookie As HttpCookie = Request.Cookies("Review-" & Me.ModuleID.ToString() & "-" & CurrentProperty.PropertyID.ToString())
                    If (cookie IsNot Nothing) Then
                        Me.Visible = False
                        Return
                    End If
                End If

                BindDetails()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Not IsPostBack Then

                    cmdSubmit.ValidationGroup = ModuleKey & "-Review"
                    cmdSubmit.CssClass = PropertySettings.ButtonClass

                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

            Try

                lblSubmitResults.Text = String.Empty

                If Page.IsValid Then

                    Dim ratings As New List(Of Integer)

                    Dim objReviewController As New ReviewFieldController

                    Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabID, ModuleID, ModuleKey)
                    Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewController.List(Me.ModuleID)

                    ' Assign values from from to mail custom field collection
                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then

                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                                For Each objReviewField As ReviewFieldInfo In objReviewFields
                                    If (objReviewField.ReviewFieldID = Convert.ToInt32(objControl.ID)) Then
                                        If (objReviewField.FieldType = ReviewFieldType.OneLineTextBox OrElse objReviewField.FieldType = CustomFieldType.MultiLineTextBox) Then
                                            objReviewField.DefaultValue = CType(objControl, TextBox).Text
                                        End If
                                        If (objReviewField.FieldType = ReviewFieldType.CheckBox) Then
                                            objReviewField.DefaultValue = CType(objControl, CheckBox).Checked.ToString()
                                        End If
                                        If (objReviewField.FieldType = ReviewFieldType.DropDownList) Then
                                            objReviewField.DefaultValue = CType(objControl, DropDownList).SelectedValue
                                        End If
                                        If (objReviewField.FieldType = ReviewFieldType.MultiCheckBox) Then
                                            Dim objCheckBoxList As CheckBoxList = CType(objControl, CheckBoxList)
                                            For Each objListItem As ListItem In objCheckBoxList.Items
                                                If (objListItem.Selected) Then
                                                    If (objReviewField.DefaultValue <> "") Then
                                                        objReviewField.DefaultValue = objReviewField.DefaultValue & "|" & objListItem.Value
                                                    Else
                                                        objReviewField.DefaultValue = objListItem.Value
                                                    End If
                                                End If
                                            Next
                                        End If
                                        If (objReviewField.FieldType = ReviewFieldType.Rating) Then
                                            objReviewField.DefaultValue = CType(objControl, RadioButtonList).SelectedValue
                                            ratings.Add(Convert.ToInt32(CType(objControl, RadioButtonList).SelectedValue))
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    Dim requireReview As Boolean = False
                    If (PropertySettings.ReviewModeration = True And Request.IsAuthenticated = False) Then
                        requireReview = True
                    Else
                        If (PropertySettings.ReviewModeration = True And PropertyAgentBase.IsEditable = False And PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False) Then
                            requireReview = True
                        End If
                    End If

                    Dim objController As New ReviewController()
                    Dim reviewID As Integer = objController.Add(CurrentProperty.PropertyID, UserController.GetCurrentUserInfo.UserID, DateTime.Now, Not requireReview)

                    For Each objReviewField As ReviewFieldInfo In objReviewFields
                        objController.AddValue(reviewID, objReviewField.ReviewFieldID, objReviewField.DefaultValue)
                    Next

                    For Each objReviewField As ReviewFieldInfo In objReviewFields
                        objReviewField.DefaultValue = ""
                    Next

                    If (requireReview = False) Then

                        If (ratings.Count > 0) Then

                            Dim ratingValue As Integer = 0

                            For Each rating As Integer In ratings
                                ratingValue = ratingValue + rating
                            Next

                            Dim averageRating As Integer = ratingValue / ratings.Count

                            Dim objRating As New RatingInfo
                            objRating.CommentID = Null.NullInteger
                            objRating.ReviewID = reviewID
                            objRating.PropertyID = CurrentProperty.PropertyID
                            objRating.CreateDate = DateTime.Now
                            objRating.UserID = UserController.GetCurrentUserInfo.UserID
                            objRating.Rating = averageRating

                            Dim objRatingController As New RatingController()
                            objRatingController.Add(objRating)

                        End If

                        lblSubmitResults.Text = Localization.GetString("ReviewPosted.Text", Me.ResourceFile)
                    Else
                        lblSubmitResults.Text = Localization.GetString("ReviewModeration.Text", Me.ResourceFile)

                        Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ReviewNotification_Subject_Html)
                        Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ReviewNotification_Body_Html)

                        Dim delimStr As String = "[]"
                        Dim delimiter As Char() = delimStr.ToCharArray()
                        Dim link As String = objLayoutController.GetExternalLink(DotNetNuke.Common.NavigateURL(Me.TabID, "", PropertySettings.SEOAgentType & "=ApproveReviews"))
                        objLayoutBody.Tokens = objLayoutBody.Template.Replace("[LINK]", link).Split(delimiter)
                        
                        Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                        objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, CurrentProperty, CustomFields, Nothing, False)
                        Dim subject As String = RenderControlAsString(phProperty)
                        phProperty = New System.Web.UI.WebControls.PlaceHolder

                        objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, CurrentProperty, CustomFields, Nothing, False)
                        Dim body As String = RenderControlAsString(phProperty)
                        phProperty = Nothing

                        If (PropertySettings.ReviewEmail <> "") Then

                            For Each email As String In PropertySettings.ReviewEmail.Split(","c)
                                Try
                                    DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, email, "", "", _
                                       DotNetNuke.Services.Mail.MailPriority.Normal, _
                                       subject, _
                                       DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body, _
                                       "", PortalSettings.HostSettings("SMTPServer"), PortalSettings.HostSettings("SMTPAuthentication"), PortalSettings.HostSettings("SMTPUsername"), PortalSettings.HostSettings("SMTPPassword"))

                                Catch
                                End Try
                            Next

                        End If

                    End If

                    ' Record View
                    Dim cookie As HttpCookie = Request.Cookies("Review-" & Me.ModuleID.ToString() & "-" & CurrentProperty.PropertyID.ToString())
                    If (cookie Is Nothing) Then

                        cookie = New HttpCookie("Review-" & Me.ModuleID.ToString() & "-" & CurrentProperty.PropertyID.ToString())
                        cookie.Value = "1"
                        cookie.Expires = DateTime.Now.AddYears(1)
                        Context.Response.Cookies.Add(cookie)

                    End If

                    rptDetails.Visible = False
                    cmdSubmit.Visible = False

                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptDetails_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDetails.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objReviewField As ReviewFieldInfo = CType(e.Item.DataItem, ReviewFieldInfo)
                Dim phValue As PlaceHolder = CType(e.Item.FindControl("phValue"), PlaceHolder)

                Dim cmdHelp As LinkButton = CType(e.Item.FindControl("cmdHelp"), LinkButton)
                Dim pnlHelp As Panel = CType(e.Item.FindControl("pnlHelp"), Panel)
                Dim lblLabel As Label = CType(e.Item.FindControl("lblLabel"), Label)
                Dim lblHelp As Label = CType(e.Item.FindControl("lblHelp"), Label)
                Dim imgHelp As Image = CType(e.Item.FindControl("imgHelp"), Image)

                If Not (phValue Is Nothing) Then

                    cmdHelp.Visible = Not PropertySettings.SearchHideHelpIcon

                    DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelp, pnlHelp, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                    lblLabel.Text = objReviewField.Caption & ":"
                    lblHelp.Text = objReviewField.CaptionHelp
                    imgHelp.AlternateText = objReviewField.CaptionHelp

                    Select Case (objReviewField.FieldType)

                        Case ReviewFieldType.OneLineTextBox
                            Dim objTextBox As New TextBox
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objReviewField.ReviewFieldID.ToString()
                            If (objReviewField.DefaultValue <> "") Then
                                objTextBox.Text = objReviewField.DefaultValue
                            End If
                            objTextBox.Width = Unit.Percentage(100)
                            phValue.Controls.Add(objTextBox)

                            If (objReviewField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey & "-Review"
                                phValue.Controls.Add(valRequired)
                            End If

                        Case ReviewFieldType.MultiLineTextBox
                            Dim objTextBox As New TextBox
                            objTextBox.TextMode = TextBoxMode.MultiLine
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objReviewField.ReviewFieldID.ToString()
                            objTextBox.Rows = PropertySettings.ContactMessageLines
                            If (objReviewField.DefaultValue <> "") Then
                                objTextBox.Text = objReviewField.DefaultValue
                            End If
                            objTextBox.Width = Unit.Percentage(100)
                            phValue.Controls.Add(objTextBox)

                            If (objReviewField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey & "-Review"
                                phValue.Controls.Add(valRequired)
                            End If

                        Case ReviewFieldType.DropDownList

                            Dim objDropDownList As New DropDownList
                            objDropDownList.CssClass = "NormalTextBox"
                            objDropDownList.ID = objReviewField.ReviewFieldID.ToString()

                            Dim values As String() = objReviewField.FieldElements.Split(Convert.ToChar("|"))
                            For Each value As String In values
                                If (value <> "") Then
                                    objDropDownList.Items.Add(value)
                                End If
                            Next

                            Dim selectText As String = Localization.GetString("SelectValue", Me.ResourceFile)
                            selectText = selectText.Replace("[VALUE]", objReviewField.Caption)
                            objDropDownList.Items.Insert(0, New ListItem(selectText, "-1"))

                            If (objReviewField.DefaultValue <> "") Then
                                If Not (objDropDownList.Items.FindByValue(objReviewField.DefaultValue) Is Nothing) Then
                                    objDropDownList.SelectedValue = objReviewField.DefaultValue
                                End If
                            End If

                            objDropDownList.Width = Unit.Pixel(Me.PropertySettings.FieldWidth)
                            phValue.Controls.Add(objDropDownList)

                            If (objReviewField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objDropDownList.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey & "-Review"
                                valRequired.InitialValue = "-1"
                                phValue.Controls.Add(valRequired)
                            End If

                        Case ReviewFieldType.CheckBox
                            Dim objCheckBox As New CheckBox
                            objCheckBox.CssClass = "NormalTextBox"
                            objCheckBox.ID = objReviewField.ReviewFieldID.ToString()
                            If (objReviewField.DefaultValue <> "") Then
                                Try
                                    objCheckBox.Checked = Convert.ToBoolean(objReviewField.DefaultValue)
                                Catch
                                End Try
                            End If
                            phValue.Controls.Add(objCheckBox)

                        Case ReviewFieldType.MultiCheckBox
                            Dim objCheckBoxList As New CheckBoxList
                            objCheckBoxList.CssClass = "Normal"
                            objCheckBoxList.ID = objReviewField.ReviewFieldID.ToString()
                            objCheckBoxList.RepeatColumns = Me.PropertySettings.CheckBoxItemsPerRow
                            objCheckBoxList.RepeatDirection = RepeatDirection.Horizontal
                            objCheckBoxList.RepeatLayout = RepeatLayout.Table

                            Dim values As String() = objReviewField.FieldElements.Split(Convert.ToChar("|"))
                            For Each value As String In values
                                objCheckBoxList.Items.Add(value)
                            Next

                            phValue.Controls.Add(objCheckBoxList)

                        Case ReviewFieldType.Rating
                            Dim objRadioButtonList As New RadioButtonList
                            objRadioButtonList.CssClass = "NormalTextBox"
                            objRadioButtonList.RepeatDirection = RepeatDirection.Horizontal
                            objRadioButtonList.RepeatLayout = RepeatLayout.Flow
                            objRadioButtonList.ID = objReviewField.ReviewFieldID.ToString()
                            objRadioButtonList.Items.Add("1")
                            objRadioButtonList.Items.Add("2")
                            objRadioButtonList.Items.Add("3")
                            objRadioButtonList.Items.Add("4")
                            objRadioButtonList.Items.Add("5")
                            phValue.Controls.Add(objRadioButtonList)

                            If (objReviewField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objRadioButtonList.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey & "-Review"
                                phValue.Controls.Add(valRequired)
                            End If

                    End Select

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace