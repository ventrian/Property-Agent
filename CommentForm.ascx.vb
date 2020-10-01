Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports System.IO
Imports Ventrian.PropertyAgent.Social

Namespace Ventrian.PropertyAgent

    Partial Public Class CommentForm
        Inherits PropertyAgentControl

#Region " Private Constants "

        Private Const _name As String = "PropertyAgent-Name"
        Private Const _email As String = "PropertyAgent-Email"
        Private Const _website As String = "PropertyAgent-Website"
        Private Const _remember As String = "PropertyAgent-Remember"

#End Region

#Region " Private Members "

        Dim _commentFormFields As ArrayList
        Dim _enableRatings As Boolean = False

#End Region

#Region " Private Properties"

        Public ReadOnly Property CommentFormFields() As ArrayList
            Get
                If (_commentFormFields Is Nothing) Then
                    _commentFormFields = New ArrayList()

                    For Each commentFormField As CommentFieldType In System.Enum.GetValues(GetType(CommentFieldType))

                        Select Case commentFormField
                            Case CommentFieldType.Name
                                Dim objCustomField As New CustomFieldInfo
                                objCustomField.CustomFieldID = CommentFieldType.Name
                                objCustomField.Name = "Name"
                                objCustomField.Caption = "Name"
                                objCustomField.CaptionHelp = "Name.Help"
                                objCustomField.IsCaptionHidden = False
                                objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                If (Request.Cookies(_name) Is Nothing) Then
                                    objCustomField.DefaultValue = String.Empty
                                Else
                                    objCustomField.DefaultValue = Request.Cookies(_name).Value
                                End If
                                objCustomField.IsRequired = True
                                objCustomField.IsHidden = False
                                objCustomField.ValidationType = CustomFieldValidationType.None
                                If (Page.User.Identity.IsAuthenticated = False) Then
                                    _commentFormFields.Add(objCustomField)
                                End If

                            Case CommentFieldType.Email
                                Dim objCustomField As New CustomFieldInfo
                                objCustomField.CustomFieldID = CommentFieldType.Email
                                objCustomField.Name = "Email"
                                objCustomField.Caption = "Email"
                                objCustomField.CaptionHelp = "Email.Help"
                                objCustomField.IsCaptionHidden = False
                                objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                If (Request.Cookies(_email) Is Nothing) Then
                                    objCustomField.DefaultValue = String.Empty
                                Else
                                    objCustomField.DefaultValue = Request.Cookies(_email).Value
                                End If
                                objCustomField.IsRequired = False
                                objCustomField.IsHidden = False
                                objCustomField.ValidationType = CustomFieldValidationType.Email
                                If (Page.User.Identity.IsAuthenticated = False) Then
                                    _commentFormFields.Add(objCustomField)
                                End If

                            Case CommentFieldType.Website
                                Dim objCustomField As New CustomFieldInfo
                                objCustomField.CustomFieldID = CommentFieldType.Website
                                objCustomField.Name = "Website"
                                objCustomField.Caption = "Website"
                                objCustomField.CaptionHelp = "Website.Help"
                                objCustomField.IsCaptionHidden = False
                                objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                If (Request.Cookies(_website) Is Nothing) Then
                                    objCustomField.DefaultValue = String.Empty
                                Else
                                    objCustomField.DefaultValue = Request.Cookies(_website).Value
                                End If
                                objCustomField.IsRequired = False
                                objCustomField.IsHidden = False
                                objCustomField.ValidationType = CustomFieldValidationType.None
                                If (Page.User.Identity.IsAuthenticated = False) Then
                                    _commentFormFields.Add(objCustomField)
                                End If

                            Case CommentFieldType.Comment
                                Dim objCustomField As New CustomFieldInfo
                                objCustomField.CustomFieldID = CommentFieldType.Comment
                                objCustomField.Name = "Comment"
                                objCustomField.Caption = "Comment"
                                objCustomField.CaptionHelp = "Comment.Help"
                                objCustomField.IsCaptionHidden = False
                                objCustomField.FieldType = CustomFieldType.MultiLineTextBox ' .RichTextBox
                                objCustomField.DefaultValue = String.Empty
                                objCustomField.IsRequired = True
                                objCustomField.IsHidden = False
                                objCustomField.ValidationType = CustomFieldValidationType.None
                                _commentFormFields.Add(objCustomField)

                            Case CommentFieldType.Remember
                                Dim objCustomField As New CustomFieldInfo
                                objCustomField.CustomFieldID = CommentFieldType.Remember
                                objCustomField.Name = "Remember"
                                objCustomField.Caption = "Remember"
                                objCustomField.CaptionHelp = "Remember.Help"
                                objCustomField.IsCaptionHidden = False
                                objCustomField.FieldType = CustomFieldType.CheckBox
                                objCustomField.DefaultValue = String.Empty
                                If (Request.Cookies(_remember) Is Nothing) Then
                                    objCustomField.DefaultValue = String.Empty
                                Else
                                    objCustomField.DefaultValue = Request.Cookies(_remember).Value
                                End If
                                objCustomField.IsRequired = True
                                objCustomField.IsHidden = False
                                objCustomField.ValidationType = CustomFieldValidationType.None
                                If (Page.User.Identity.IsAuthenticated = False) Then
                                    _commentFormFields.Add(objCustomField)
                                End If

                            Case CommentFieldType.Rating
                                Dim objCustomField As New CustomFieldInfo
                                objCustomField.CustomFieldID = CommentFieldType.Rating
                                objCustomField.Name = "Rating"
                                objCustomField.Caption = "Rating"
                                objCustomField.CaptionHelp = "Rating.Help"
                                objCustomField.IsCaptionHidden = False
                                objCustomField.FieldType = CustomFieldType.RadioButton ' .RichTextBox
                                objCustomField.DefaultValue = String.Empty
                                objCustomField.IsRequired = True
                                objCustomField.IsHidden = False
                                objCustomField.ValidationType = CustomFieldValidationType.None
                                If (EnableRatings) Then
                                    _commentFormFields.Insert(0, objCustomField)
                                End If

                        End Select
                    Next
                End If
                Return _commentFormFields
            End Get
        End Property

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/CommentForm"
            End Get
        End Property

#End Region

#Region " Public Properties "

        Public Property EnableRatings() As Boolean
            Get
                Return _enableRatings
            End Get
            Set(ByVal value As Boolean)
                _enableRatings = value
            End Set
        End Property

#End Region

#Region " Private Methods "


        Private Sub BindDetails()

            rptDetails.DataSource = CommentFormFields
            rptDetails.DataBind()

            If PropertySettings.CommentWidth.Contains("%") Then
                tblCommentForm.Width = Unit.Percentage(PropertySettings.CommentWidth.Trim("%"))
            Else
                tblCommentForm.Width = Unit.Pixel(PropertySettings.CommentWidth.Trim("px"))
            End If
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

                BindDetails()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Not IsPostBack Then

                    cmdSubmit.ValidationGroup = ModuleKey & "-Comment"
                    cmdSubmit.CssClass = PropertySettings.ButtonClass

                End If

                trCaptcha.Visible = PropertySettings.CommentUseCaptcha
                'ctlCaptcha.ErrorMessage = Localization.GetString("ctlCaptcha.ErrorMessage", Me.ResourceFile)

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            Dim hostSettings As Dictionary(Of String, String) = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary()
            Try

                lblSubmitResults.Text = String.Empty

                If Page.IsValid AndAlso (PropertySettings.CommentUseCaptcha = False OrElse mlFlexCaptcha.mlValidate()) Then

                    Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabID, ModuleID, ModuleKey)
                    Dim objCustomFields As ArrayList = CommentFormFields

                    ' Assign values from from to mail custom field collection
                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then

                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.CustomFieldID = Convert.ToInt32(objControl.ID)) Then
                                        If (objCustomField.FieldType = CustomFieldType.OneLineTextBox OrElse objCustomField.FieldType = CustomFieldType.MultiLineTextBox) Then
                                            objCustomField.DefaultValue = CType(objControl, TextBox).Text
                                        End If
                                        If (objCustomField.FieldType = CustomFieldType.CheckBox) Then
                                            objCustomField.DefaultValue = CType(objControl, CheckBox).Checked.ToString()
                                        End If
                                        If (objCustomField.FieldType = CustomFieldType.RadioButton) Then
                                            objCustomField.DefaultValue = CType(objControl, RadioButtonList).SelectedValue
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    Dim objComment As New CommentInfo()

                    If (Me.Page.User.Identity.IsAuthenticated) Then
                        objComment.UserID = UserController.Instance.GetCurrentUserInfo.UserID
                    Else
                        objComment.UserID = Null.NullInteger
                    End If
                    objComment.PropertyID = CurrentProperty.PropertyID
                    objComment.ParentID = Null.NullInteger

                    If (Request("pid") <> "") Then
                        If (IsNumeric(Request("pid"))) Then
                            objComment.ParentID = Convert.ToInt32(Request("pid"))
                        End If
                    End If
                    objComment.CreateDate = DateTime.Now

                    Dim rememberMe As Boolean = False
                    Dim rating As Double = Null.NullDouble
                    For Each objCustomField As CustomFieldInfo In objCustomFields
                        If (objCustomField.CustomFieldID = Convert.ToInt32(CommentFieldType.Comment)) Then
                            objComment.Comment = objCustomField.DefaultValue
                        End If
                        If (objCustomField.CustomFieldID = Convert.ToInt32(CommentFieldType.Name)) Then
                            objComment.Name = objCustomField.DefaultValue
                        End If
                        If (objCustomField.CustomFieldID = Convert.ToInt32(CommentFieldType.Email)) Then
                            objComment.Email = objCustomField.DefaultValue
                        End If
                        If (objCustomField.CustomFieldID = Convert.ToInt32(CommentFieldType.Website)) Then
                            objComment.Website = objCustomField.DefaultValue
                        End If
                        If (objCustomField.CustomFieldID = Convert.ToInt32(CommentFieldType.Remember)) Then
                            rememberMe = Convert.ToBoolean(objCustomField.DefaultValue)
                        End If
                        If (objCustomField.CustomFieldID = Convert.ToInt32(CommentFieldType.Rating)) Then
                            rating = Convert.ToDouble(objCustomField.DefaultValue)
                        End If
                    Next

                    If (Me.Page.User.Identity.IsAuthenticated = False) Then
                        Response.Cookies(_remember).Value = rememberMe.ToString()
                        If (rememberMe) Then
                            Response.Cookies(_name).Value = objComment.Name
                            Response.Cookies(_email).Value = objComment.Email
                            Response.Cookies(_website).Value = objComment.Website
                        Else
                            If Not (Response.Cookies(_name) Is Nothing) Then
                                Response.Cookies.Remove(_name)
                            End If
                            If Not (Response.Cookies(_email) Is Nothing) Then
                                Response.Cookies.Remove(_email)
                            End If
                            If Not (Response.Cookies(_website) Is Nothing) Then
                                Response.Cookies.Remove(_website)
                            End If
                        End If
                    End If

                    Dim objCommentController As New CommentController()
                    objComment.CommentID = objCommentController.Add(objComment)


                    Dim objPropertyController As New PropertyController
                    Dim objProperty As PropertyInfo = objPropertyController.Get(objComment.PropertyID)

                    If (objProperty IsNot Nothing) Then

                        If (PropertyAgentBase.UserId <> Null.NullInteger) Then
                            Journal.AddCommentToJournal(objProperty, objComment, PropertyAgentBase.PortalId, TabID, PropertyAgentBase.UserId, objLayoutController.GetExternalLink(objLayoutController.GetPropertyLink(objProperty, PropertyAgentBase.CustomFields)), PropertySettings.PropertyLabel)
                        End If

                        If (PropertySettings.CommentNotifyOwner) Then
                            If (PropertyAgentBase.UserId <> objProperty.AuthorID) Then

                                Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Subject_Html)
                                Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Body_Html)

                                Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                                objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, objProperty, CustomFields, Nothing, False)
                                Dim subject As String = RenderControlAsString(phProperty).Replace("[COMMENT]", objComment.Comment)
                                phProperty = New System.Web.UI.WebControls.PlaceHolder

                                objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, objProperty, CustomFields, Nothing, False)
                                Dim body As String = RenderControlAsString(phProperty).Replace("[COMMENT]", objComment.Comment)
                                phProperty = Nothing

                                If (objProperty.Email <> "") Then
                                    Try
                                        DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objProperty.Email, "", "",
                                           DotNetNuke.Services.Mail.MailPriority.Normal,
                                           subject,
                                           DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                                           "", hostSettings("SMTPServer"), hostSettings("SMTPAuthentication"), hostSettings("SMTPUsername"), DotNetNuke.Entities.Host.Host.SMTPPassword)

                                    Catch
                                    End Try
                                End If

                            End If
                        End If

                        ' Subscribed People
                        Dim objSubscribeController As New SubscriberController()
                        Dim objSubscribers As List(Of UserInfo) = objSubscribeController.ListSubscribers(objProperty.PropertyID)

                        If (objSubscribers.Count > 0) Then

                            Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Subject_Html)
                            Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Body_Html)

                            Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                            objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, objProperty, CustomFields, Nothing, False)
                            Dim subject As String = RenderControlAsString(phProperty).Replace("[COMMENT]", objComment.Comment)
                            phProperty = New System.Web.UI.WebControls.PlaceHolder

                            objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, objProperty, CustomFields, Nothing, False)
                            Dim body As String = RenderControlAsString(phProperty).Replace("[COMMENT]", objComment.Comment)
                            phProperty = Nothing

                            For Each objSubscriber As UserInfo In objSubscribers
                                If (objSubscriber.Email <> "") Then
                                    Try
                                        DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objSubscriber.Email, "", "",
                                           DotNetNuke.Services.Mail.MailPriority.Normal,
                                           subject,
                                           DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                                           "", hostSettings("SMTPServer"), hostSettings("SMTPAuthentication"), hostSettings("SMTPUsername"), hostSettings("SMTPPassword"))

                                    Catch
                                    End Try
                                End If
                            Next

                        End If

                    End If

                    If (PropertySettings.CommentNotifyEmail <> "") Then

                        Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Subject_Html)
                        Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.CommentNotification_Body_Html)

                        Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                        objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, objProperty, CustomFields, Nothing, False)
                        Dim subject As String = RenderControlAsString(phProperty).Replace("[COMMENT]", objComment.Comment)
                        phProperty = New System.Web.UI.WebControls.PlaceHolder

                        objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, objProperty, CustomFields, Nothing, False)
                        Dim body As String = RenderControlAsString(phProperty).Replace("[COMMENT]", objComment.Comment)
                        phProperty = Nothing

                        For Each email As String In PropertySettings.CommentNotifyEmail.Split(","c)

                            If (email <> "") Then
                                Try
                                    DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, email, "", "",
                                       DotNetNuke.Services.Mail.MailPriority.Normal,
                                       subject,
                                       DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                                       "", hostSettings("SMTPServer"), hostSettings("SMTPAuthentication"), hostSettings("SMTPUsername"), hostSettings("SMTPPassword"))

                                Catch
                                End Try
                            End If

                        Next

                    End If

                    If (rating <> Null.NullDouble) Then
                        Dim objRating As New RatingInfo
                        objRating.CommentID = objComment.CommentID
                        objRating.PropertyID = objComment.PropertyID
                        objRating.CreateDate = objComment.CreateDate
                        objRating.UserID = objComment.UserID
                        objRating.Rating = rating

                        Dim objRatingController As New RatingController()
                        objRatingController.Add(objRating)
                    End If

                    lblSubmitResults.Text = Localization.GetString("CommentPosted.Text", Me.ResourceFile)
                    lblSubmitResults.CssClass = "Normal"

                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then

                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)

                                If (Convert.ToInt32(CommentFieldType.Comment) = Convert.ToInt32(objControl.ID)) Then
                                    CType(objControl, TextBox).Text = ""
                                End If
                            End If
                        End If
                    Next

                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then

                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                    If (objCustomField.CustomFieldID = Convert.ToInt32(objControl.ID)) Then
                                        If (objCustomField.FieldType = CustomFieldType.OneLineTextBox OrElse objCustomField.FieldType = CustomFieldType.MultiLineTextBox) Then
                                            objCustomField.DefaultValue = ""
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    Response.Redirect(Request.RawUrl, True)

                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Private Sub rptDetails_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDetails.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objCustomField As CustomFieldInfo = CType(e.Item.DataItem, CustomFieldInfo)
                Dim phValue As PlaceHolder = CType(e.Item.FindControl("phValue"), PlaceHolder)

                Dim cmdHelp As LinkButton = CType(e.Item.FindControl("cmdHelp"), LinkButton)
                Dim pnlHelp As Panel = CType(e.Item.FindControl("pnlHelp"), Panel)
                Dim lblLabel As Label = CType(e.Item.FindControl("lblLabel"), Label)
                Dim lblHelp As Label = CType(e.Item.FindControl("lblHelp"), Label)
                Dim imgHelp As Image = CType(e.Item.FindControl("imgHelp"), Image)

                If Not (phValue Is Nothing) Then

                    cmdHelp.Visible = Not PropertySettings.SearchHideHelpIcon

                    DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelp, pnlHelp, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                    lblLabel.Text = Localization.GetString(objCustomField.Caption, Me.ResourceFile) & ":"
                    lblHelp.Text = Localization.GetString(objCustomField.CaptionHelp, Me.ResourceFile)
                    imgHelp.AlternateText = Localization.GetString(objCustomField.CaptionHelp, Me.ResourceFile)

                    Select Case (objCustomField.FieldType)

                        Case CustomFieldType.OneLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.CustomFieldID.ToString()
                            If (objCustomField.DefaultValue <> "") Then
                                objTextBox.Text = objCustomField.DefaultValue
                            End If
                            objTextBox.Width = Unit.Percentage(100)
                            phValue.Controls.Add(objTextBox)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey & "-Comment"
                                phValue.Controls.Add(valRequired)
                            End If

                            If objCustomField.ValidationType = CustomFieldValidationType.Email Then
                                Dim valCompare As New RegularExpressionValidator
                                valCompare.ControlToValidate = objTextBox.ID
                                valCompare.ValidationExpression = "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                valCompare.ErrorMessage = Localization.GetString("valValidateEmail", ResourceFile)
                                valCompare.CssClass = "NormalRed"
                                valCompare.Display = ValidatorDisplay.Dynamic
                                valCompare.ValidationGroup = ModuleKey & "-Comment"
                                phValue.Controls.Add(valCompare)
                            Else
                                If (objCustomField.ValidationType <> CustomFieldValidationType.None) Then
                                    Dim valCompare As New CompareValidator
                                    valCompare.ControlToValidate = objTextBox.ID
                                    Select Case objCustomField.ValidationType

                                        Case CustomFieldValidationType.Currency
                                            valCompare.Type = ValidationDataType.Double
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateCurrency", Me.ResourceFile)

                                        Case CustomFieldValidationType.Date
                                            valCompare.Type = ValidationDataType.Date
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateDate", Me.ResourceFile)

                                        Case CustomFieldValidationType.Double
                                            valCompare.Type = ValidationDataType.Double
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateDecimal", Me.ResourceFile)

                                        Case CustomFieldValidationType.Integer
                                            valCompare.Type = ValidationDataType.Integer
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valValidateNumber", Me.ResourceFile)

                                    End Select
                                    valCompare.CssClass = "NormalRed"
                                    valCompare.Display = ValidatorDisplay.Dynamic
                                    valCompare.ValidationGroup = ModuleKey & "-Comment"
                                    phValue.Controls.Add(valCompare)
                                End If
                            End If

                        Case CustomFieldType.MultiLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.TextMode = TextBoxMode.MultiLine
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.CustomFieldID.ToString()
                            objTextBox.Rows = PropertySettings.ContactMessageLines
                            If (objCustomField.DefaultValue <> "") Then
                                objTextBox.Text = objCustomField.DefaultValue
                            End If
                            objTextBox.Width = Unit.Percentage(100)
                            phValue.Controls.Add(objTextBox)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey & "-Comment"
                                phValue.Controls.Add(valRequired)
                            End If

                        Case CustomFieldType.CheckBox

                            Dim objCheckBox As New CheckBox
                            objCheckBox.CssClass = "NormalTextBox"
                            objCheckBox.ID = objCustomField.CustomFieldID.ToString()
                            If (objCustomField.DefaultValue <> "") Then
                                Try
                                    objCheckBox.Checked = Convert.ToBoolean(objCustomField.DefaultValue)
                                Catch
                                End Try
                            End If
                            phValue.Controls.Add(objCheckBox)

                        Case CustomFieldType.RadioButton

                            Dim objRadioButtonList As New RadioButtonList
                            objRadioButtonList.CssClass = "NormalTextBox"
                            objRadioButtonList.RepeatDirection = RepeatDirection.Horizontal
                            objRadioButtonList.RepeatLayout = RepeatLayout.Flow
                            objRadioButtonList.ID = objCustomField.CustomFieldID.ToString()
                            objRadioButtonList.Items.Add("1")
                            objRadioButtonList.Items.Add("2")
                            objRadioButtonList.Items.Add("3")
                            objRadioButtonList.Items.Add("4")
                            objRadioButtonList.Items.Add("5")
                            phValue.Controls.Add(objRadioButtonList)

                            If (objCustomField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objRadioButtonList.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey & "-Comment"
                                phValue.Controls.Add(valRequired)
                            End If

                    End Select

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace