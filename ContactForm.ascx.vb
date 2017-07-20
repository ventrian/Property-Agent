Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports System.IO

Namespace Ventrian.PropertyAgent

    Partial Public Class ContactForm
        Inherits PropertyAgentControl

#Region " Private Members "

        Dim _contactFormFields As List(Of ContactFieldInfo)
        Dim _email As String = ""

#End Region

#Region " Private Properties "

        Public ReadOnly Property ContactFormFields() As List(Of ContactFieldInfo)
            Get
                If (_contactFormFields Is Nothing) Then

                    Dim objContactFieldController As New ContactFieldController()
                    _contactFormFields = objContactFieldController.List(Me.ModuleID)

                    If (_contactFormFields.Count = 0) Then
                        _contactFormFields = New List(Of ContactFieldInfo)

                        For Each contactFormField As ContactDefaultFieldType In System.Enum.GetValues(GetType(ContactDefaultFieldType))

                            Select Case contactFormField
                                Case ContactDefaultFieldType.Name
                                    Dim objCustomField As New ContactFieldInfo
                                    objCustomField.ContactFieldID = ContactDefaultFieldType.Name
                                    objCustomField.Name = "Name"
                                    objCustomField.Caption = "Name"
                                    objCustomField.CaptionHelp = "Name.Help"
                                    objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                    objCustomField.DefaultValue = String.Empty
                                    objCustomField.IsRequired = Me.PropertySettings.ContactRequireName
                                    If (Me.PropertySettings.ContactHideName = False) Then
                                        _contactFormFields.Add(objCustomField)
                                    End If

                                Case ContactDefaultFieldType.Phone
                                    Dim objCustomField As New ContactFieldInfo
                                    objCustomField.ContactFieldID = ContactDefaultFieldType.Phone
                                    objCustomField.Name = "Phone"
                                    objCustomField.Caption = "Phone"
                                    objCustomField.CaptionHelp = "Phone.Help"
                                    objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                    objCustomField.DefaultValue = String.Empty
                                    objCustomField.IsRequired = Me.PropertySettings.ContactRequirePhone
                                    If (Me.PropertySettings.ContactHidePhone = False) Then
                                        _contactFormFields.Add(objCustomField)
                                    End If

                                Case ContactDefaultFieldType.Email
                                    Dim objCustomField As New ContactFieldInfo
                                    objCustomField.ContactFieldID = ContactDefaultFieldType.Email
                                    objCustomField.Name = "Email"
                                    objCustomField.Caption = "Email"
                                    objCustomField.CaptionHelp = "Email.Help"
                                    objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                    objCustomField.DefaultValue = String.Empty
                                    objCustomField.IsRequired = Me.PropertySettings.ContactRequireEmail
                                    If (Me.PropertySettings.ContactHideEmail = False) Then
                                        _contactFormFields.Add(objCustomField)
                                    End If

                                Case ContactDefaultFieldType.Message
                                    Dim objCustomField As New ContactFieldInfo
                                    objCustomField.ContactFieldID = ContactDefaultFieldType.Message
                                    objCustomField.Name = "Message"
                                    objCustomField.Caption = "Message"
                                    objCustomField.CaptionHelp = "Message.Help"
                                    objCustomField.FieldType = CustomFieldType.MultiLineTextBox ' .RichTextBox
                                    objCustomField.DefaultValue = String.Empty
                                    objCustomField.IsRequired = True
                                    _contactFormFields.Add(objCustomField)

                            End Select
                        Next
                    End If
                End If

                Return _contactFormFields
            End Get
        End Property

        Private ReadOnly Property MailTo() As String
            Get
                If (_email <> "") Then
                    Return _email
                End If
                Select Case Me.PropertySettings.ContactDestination
                    Case DestinationType.PropertyOwner
                        If (CurrentProperty Is Nothing) Then
                            Return Null.NullString
                        Else
                            If CurrentProperty.AuthorID <> Null.NullInteger Then
                                If CurrentProperty.Email.Trim() <> "" Then
                                    Return CurrentProperty.Email.Trim()
                                Else
                                    Return PropertySettings.ContactBCC.Trim()
                                End If
                            Else
                                Return PropertySettings.ContactBCC.Trim()
                            End If
                        End If

                    Case DestinationType.PortalAdmin
                        Return PortalSettings.Email

                    Case DestinationType.CustomEmail
                        Return PropertySettings.ContactCustomEmail

                    Case DestinationType.CustomField
                        If (PropertySettings.ContactDestination = DestinationType.CustomField) Then
                            If (PropertySettings.ContactField <> Null.NullInteger) Then
                                For Each objCustomField As CustomFieldInfo In CustomFields
                                    If (objCustomField.CustomFieldID = PropertySettings.ContactField) Then
                                        If (CurrentProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                            Return CurrentProperty.PropertyList(objCustomField.CustomFieldID).ToString()
                                        Else
                                            Return PortalSettings.Email
                                        End If
                                    End If
                                Next
                            Else
                                Return PortalSettings.Email
                            End If
                        Else
                            Return PortalSettings.Email
                        End If

                End Select

                Return ""
            End Get
        End Property

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/ContactForm"
            End Get
        End Property

#End Region

#Region " Public Properties "

        Public Property EmailAddress() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindDetails()

            rptDetails.DataSource = ContactFormFields
            rptDetails.DataBind()

            tblContactForm.Width = Unit.Pixel(PropertySettings.ContactWidth)
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
                trCaptcha.Visible = PropertySettings.ContactUseCaptcha

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Not IsPostBack Then

                    If CurrentProperty Is Nothing OrElse (CurrentProperty.Email.Trim() = "" And PropertySettings.ContactDestination = DestinationType.PropertyOwner) Then
                        phContactForm.Visible = False
                    Else
                        phContactForm.Visible = True
                        cmdSubmit.ValidationGroup = ModuleKey
                        cmdSubmit.CssClass = PropertySettings.ButtonClass
                    End If

                End If

                trCaptcha.Visible = PropertySettings.ContactUseCaptcha
                ctlCaptcha.ErrorMessage = Localization.GetString("ctlCaptcha.ErrorMessage", Me.ResourceFile)

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

            Try

                lblSubmitResults.Text = String.Empty

                If Page.IsValid And (ctlCaptcha.IsValid Or PropertySettings.ContactUseCaptcha = False) Then

                    Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabID, ModuleID, ModuleKey)
                    Dim objMailCustomFields As List(Of ContactFieldInfo) = ContactFormFields

                    Dim email As String = ""

                    ' Assign values from from to mail custom field collection
                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then

                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                                For Each objContactField As ContactFieldInfo In objMailCustomFields
                                    If (objContactField.ContactFieldID = Convert.ToInt32(objControl.ID)) Then
                                        If (objContactField.FieldType = ContactFieldType.OneLineTextBox OrElse objContactField.FieldType = ContactFieldType.MultiLineTextBox) Then
                                            objContactField.DefaultValue = CType(objControl, TextBox).Text
                                            If (objContactField.Name = "Email") Then
                                                email = objContactField.DefaultValue
                                            End If
                                        End If
                                        If (objContactField.FieldType = ContactFieldType.DropDownList Or objContactField.FieldType = ContactFieldType.CustomField) Then
                                            objContactField.DefaultValue = CType(objControl, DropDownList).SelectedValue
                                        End If
                                        If (objContactField.FieldType = ContactFieldType.MultiCheckBox) Then
                                            Dim vals As String = ""
                                            For Each i As ListItem In (CType(objControl, CheckBoxList).Items)
                                                If (i.Selected) Then
                                                    If (vals = "") Then
                                                        vals = i.Value
                                                    Else
                                                        vals = vals & ", " & i.Value
                                                    End If
                                                End If
                                            Next
                                            objContactField.DefaultValue = vals
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    If (email = "" Or email.Contains("@") = False) Then
                        email = PortalSettings.Email
                    End If

                    ' Get the layout for subject and body
                    Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Subject_Html)
                    Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Body_Html)

                    ' Get the processed layout for subject and body
                    Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, CurrentProperty, CustomFields, objMailCustomFields, False)
                    Dim subject As String = RenderControlAsString(phProperty)
                    phProperty = New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, CurrentProperty, CustomFields, objMailCustomFields, False)
                    Dim body As String = RenderControlAsString(phProperty)
                    phProperty = Nothing

                    Dim replyTo As String = PortalSettings.Email
                    If (PropertySettings.ContactReplyTo = ReplyToType.ContactPerson AndAlso email <> "") Then
                        replyTo = email
                    End If

                    Dim contactBCC As String = PropertySettings.ContactBCC
                    If (contactBCC.StartsWith("[CUSTOM:")) Then
                        Dim customField As String = contactBCC.Replace("[CUSTOM:", "").TrimEnd("]"c)
                        contactBCC = ""
                        For Each objCustomField As CustomFieldInfo In CustomFields
                            If (objCustomField.Name.ToLower() = customField.ToLower()) Then
                                If (CurrentProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                    If (CurrentProperty.PropertyList(objCustomField.CustomFieldID) <> "") Then
                                        contactBCC = CurrentProperty.PropertyList(objCustomField.CustomFieldID)
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                    End If

                    Dim objContactLog As New ContactLogInfo
                    objContactLog.ModuleID = ModuleID
                    objContactLog.DateSent = DateTime.Now
                    objContactLog.SentFrom = replyTo
                    objContactLog.SentTo = MailTo
                    objContactLog.Subject = subject
                    objContactLog.Body = body
                    objContactLog.PropertyID = CurrentProperty.PropertyID

                    Dim fieldValues As String = ""
                    For Each field As ContactFieldInfo In objMailCustomFields
                        If (fieldValues = "") Then
                            fieldValues = field.Caption & ":::" & field.DefaultValue & "|||"
                        Else
                            fieldValues = fieldValues & field.Caption & ":::" & field.DefaultValue & "|||"
                        End If
                    Next
                    objContactLog.FieldValues = fieldValues

                    Dim objContactLogController As New ContactLogController()
                    objContactLogController.Add(objContactLog)

                    If Me.MailTo <> "" AndAlso replyTo <> "" Then
                        Try
                            DotNetNuke.Services.Mail.Mail.SendMail(replyTo, MailTo, "", contactBCC, _
                                                                        DotNetNuke.Services.Mail.MailPriority.Normal, _
                                                                        subject, _
                                                                        DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body, _
                                                                        "", PortalSettings.HostSettings("SMTPServer"), PortalSettings.HostSettings("SMTPAuthentication"), PortalSettings.HostSettings("SMTPUsername"), PortalSettings.HostSettings("SMTPPassword"))
                            lblSubmitResults.Text = Localization.GetString("EmailSent.Message", Me.ResourceFile)
                            lblSubmitResults.CssClass = "Normal"
                        Catch
                            lblSubmitResults.Text = Localization.GetString("EmailNotSent.Message", Me.ResourceFile)
                            lblSubmitResults.CssClass = "NormalRed"
                        End Try
                    Else
                        lblSubmitResults.Text = Localization.GetString("EmailNotSent.Message", Me.ResourceFile)
                        lblSubmitResults.CssClass = "NormalRed"
                    End If

                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then
                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                                For Each objContactField As ContactFieldInfo In objMailCustomFields
                                    If (objContactField.ContactFieldID = Convert.ToInt32(objControl.ID)) Then
                                        If (objContactField.FieldType = CustomFieldType.OneLineTextBox OrElse objContactField.FieldType = CustomFieldType.MultiLineTextBox) Then
                                            objContactField.DefaultValue = ""
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    For Each objContactField As ContactFieldInfo In objMailCustomFields
                        objContactField.DefaultValue = ""
                    Next

                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptDetails_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDetails.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objContactField As ContactFieldInfo = CType(e.Item.DataItem, ContactFieldInfo)
                Dim phValue As PlaceHolder = CType(e.Item.FindControl("phValue"), PlaceHolder)

                Dim cmdHelp As LinkButton = CType(e.Item.FindControl("cmdHelp"), LinkButton)
                Dim pnlHelp As Panel = CType(e.Item.FindControl("pnlHelp"), Panel)
                Dim lblLabel As Label = CType(e.Item.FindControl("lblLabel"), Label)
                Dim lblHelp As Label = CType(e.Item.FindControl("lblHelp"), Label)
                Dim imgHelp As Image = CType(e.Item.FindControl("imgHelp"), Image)

                If Not (phValue Is Nothing) Then

                    If (objContactField.ContactFieldID = ContactDefaultFieldType.Email) Then
                        e.Item.Visible = Not Me.PropertySettings.ContactHideEmail
                    End If

                    If (objContactField.ContactFieldID = ContactDefaultFieldType.Phone) Then
                        e.Item.Visible = Not Me.PropertySettings.ContactHidePhone
                    End If

                    cmdHelp.Visible = Not PropertySettings.SearchHideHelpIcon

                    DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelp, pnlHelp, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                    If (objContactField.IsRequired) Then
                        lblLabel.Text = objContactField.Caption & "*:"
                    Else
                        lblLabel.Text = objContactField.Caption & ":"
                    End If
                    lblHelp.Text = Localization.GetString(objContactField.CaptionHelp, Me.ResourceFile)
                    imgHelp.AlternateText = Localization.GetString(objContactField.CaptionHelp, Me.ResourceFile)

                    Select Case (objContactField.FieldType)

                        Case ContactFieldType.OneLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objContactField.ContactFieldID.ToString()
                            If (objContactField.DefaultValue <> "") Then
                                objTextBox.Text = objContactField.DefaultValue
                            End If
                            objTextBox.Width = Unit.Percentage(100)
                            phValue.Controls.Add(objTextBox)

                            If (objContactField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                            If (objContactField.Caption.ToLower() = "email") Then
                                Dim valRequiredEmail As New RegularExpressionValidator
                                valRequiredEmail.ControlToValidate = objTextBox.ID
                                valRequiredEmail.ErrorMessage = Localization.GetString("valValidateEmail", Me.ResourceFile)
                                valRequiredEmail.CssClass = "NormalRed"
                                valRequiredEmail.Display = ValidatorDisplay.Dynamic
                                valRequiredEmail.ValidationGroup = ModuleKey
                                valRequiredEmail.SetFocusOnError = True
                                valRequiredEmail.ValidationExpression = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
                                phValue.Controls.Add(valRequiredEmail)
                            End If

                        Case ContactFieldType.MultiLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.TextMode = TextBoxMode.MultiLine
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objContactField.ContactFieldID.ToString()
                            objTextBox.Rows = PropertySettings.ContactMessageLines
                            If (objContactField.DefaultValue <> "") Then
                                objTextBox.Text = objContactField.DefaultValue
                            End If
                            objTextBox.Width = Unit.Percentage(100)
                            phValue.Controls.Add(objTextBox)

                            If (objContactField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objTextBox.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.ValidationGroup = ModuleKey
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                        Case ContactFieldType.DropDownList

                            Dim objDropDownList As New DropDownList
                            objDropDownList.CssClass = "NormalTextBox"
                            objDropDownList.ID = objContactField.ContactFieldID.ToString()

                            Dim values As String() = objContactField.FieldElements.Split(Convert.ToChar("|"))
                            For Each value As String In values
                                If (value <> "") Then
                                    objDropDownList.Items.Add(value)
                                End If
                            Next

                            Dim selectText As String = Localization.GetString("SelectValue", Me.ResourceFile)
                            selectText = selectText.Replace("[VALUE]", objContactField.Caption)
                            objDropDownList.Items.Insert(0, New ListItem(selectText, "-1"))

                            If (objContactField.DefaultValue <> "") Then
                                If Not (objDropDownList.Items.FindByValue(objContactField.DefaultValue) Is Nothing) Then
                                    objDropDownList.SelectedValue = objContactField.DefaultValue
                                End If
                            End If

                            objDropDownList.Width = Unit.Percentage(100)
                            phValue.Controls.Add(objDropDownList)

                            If (objContactField.IsRequired) Then
                                Dim valRequired As New RequiredFieldValidator
                                valRequired.ControlToValidate = objDropDownList.ID
                                valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                'If (valRequired.ErrorMessage <> "") Then
                                '    valRequired.ErrorMessage = valRequired.ErrorMessage.Replace("[CUSTOMFIELD]", objContactField.Name)
                                'End If
                                valRequired.CssClass = "NormalRed"
                                valRequired.Display = ValidatorDisplay.Dynamic
                                valRequired.SetFocusOnError = True
                                valRequired.InitialValue = "-1"
                                valRequired.ValidationGroup = ModuleKey
                                phValue.Controls.Add(valRequired)
                            End If

                        Case ContactFieldType.MultiCheckBox

                            Dim objCheckBoxList As New CheckBoxList
                            objCheckBoxList.CssClass = "Normal"
                            objCheckBoxList.ID = objContactField.ContactFieldID.ToString()
                            objCheckBoxList.RepeatColumns = 1
                            objCheckBoxList.RepeatDirection = RepeatDirection.Vertical
                            objCheckBoxList.RepeatLayout = RepeatLayout.Flow

                            Dim values As String() = objContactField.FieldElements.Split(Convert.ToChar("|"))
                            For Each value As String In values

                                Dim objListItem As New ListItem
                                objListItem.Text = value
                                objListItem.Value = value
                                If (objContactField.DefaultValue <> "") Then
                                    For Each v As String In objContactField.DefaultValue.Split("|"c)
                                        If (v.Trim() = value.Trim()) Then
                                            objListItem.Selected = True
                                            Exit For
                                        End If
                                    Next
                                End If
                                objCheckBoxList.Items.Add(objListItem)
                            Next

                            phValue.Controls.Add(objCheckBoxList)

                        Case ContactFieldType.CustomField

                            If (CurrentProperty IsNot Nothing) Then

                                If (CurrentProperty.PropertyList.Contains(objContactField.CustomFieldID)) Then

                                    Dim objDropDownList As New DropDownList
                                    objDropDownList.CssClass = "NormalTextBox"
                                    objDropDownList.ID = objContactField.ContactFieldID.ToString()

                                    Dim values As String() = CurrentProperty.PropertyList(objContactField.CustomFieldID).Split(Convert.ToChar("|"))
                                    For Each value As String In values
                                        If (value <> "") Then
                                            objDropDownList.Items.Add(value)
                                        End If
                                    Next

                                    Dim selectText As String = Localization.GetString("SelectValue", Me.ResourceFile)
                                    selectText = selectText.Replace("[VALUE]", objContactField.Caption)
                                    objDropDownList.Items.Insert(0, New ListItem(selectText, "-1"))

                                    objDropDownList.Width = Unit.Percentage(100)
                                    phValue.Controls.Add(objDropDownList)

                                    If (objContactField.IsRequired And objDropDownList.Items.Count > 1) Then
                                        Dim valRequired As New RequiredFieldValidator
                                        valRequired.ControlToValidate = objDropDownList.ID
                                        valRequired.ErrorMessage = Localization.GetString("valRequired", Me.ResourceFile)
                                        'If (valRequired.ErrorMessage <> "") Then
                                        '    valRequired.ErrorMessage = valRequired.ErrorMessage.Replace("[CUSTOMFIELD]", objContactField.Name)
                                        'End If
                                        valRequired.CssClass = "NormalRed"
                                        valRequired.Display = ValidatorDisplay.Dynamic
                                        valRequired.SetFocusOnError = True
                                        valRequired.InitialValue = "-1"
                                        valRequired.ValidationGroup = ModuleKey
                                        phValue.Controls.Add(valRequired)
                                    End If


                                End If

                            End If


                    End Select

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace
