Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports System.IO

Namespace Ventrian.PropertyAgent

    Partial Public Class SendToFriendForm
        Inherits PropertyAgentControl

#Region " Private Members "

        Dim _contactFormFields As List(Of ContactFieldInfo)

#End Region

#Region " Private Properties"

        Public ReadOnly Property SendToFriendFormFields() As List(Of ContactFieldInfo)
            Get
                If (_contactFormFields Is Nothing) Then
                    _contactFormFields = New List(Of ContactFieldInfo)

                    For Each sendToFriendField As SendToFriendFieldType In System.Enum.GetValues(GetType(SendToFriendFieldType))

                        Select Case sendToFriendField
                            Case SendToFriendFieldType.ToEmail
                                Dim objCustomField As New ContactFieldInfo
                                objCustomField.ContactFieldID = SendToFriendFieldType.ToEmail
                                objCustomField.Name = "ToEmail"
                                objCustomField.Caption = "ToEmail"
                                objCustomField.CaptionHelp = "ToEmail.Help"
                                objCustomField.FieldType = CustomFieldType.MultiLineTextBox
                                objCustomField.DefaultValue = String.Empty
                                objCustomField.IsRequired = True
                                _contactFormFields.Add(objCustomField)

                            Case SendToFriendFieldType.FromName
                                Dim objCustomField As New ContactFieldInfo
                                objCustomField.ContactFieldID = SendToFriendFieldType.FromName
                                objCustomField.Name = "FromName"
                                objCustomField.Caption = "FromName"
                                objCustomField.CaptionHelp = "FromName.Help"
                                objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                objCustomField.DefaultValue = String.Empty
                                objCustomField.IsRequired = True
                                _contactFormFields.Add(objCustomField)

                            Case SendToFriendFieldType.FromEmail
                                Dim objCustomField As New ContactFieldInfo
                                objCustomField.ContactFieldID = SendToFriendFieldType.FromEmail
                                objCustomField.Name = "FromEmail"
                                objCustomField.Caption = "FromEmail"
                                objCustomField.CaptionHelp = "FromEmail.Help"
                                objCustomField.FieldType = CustomFieldType.OneLineTextBox
                                objCustomField.DefaultValue = String.Empty
                                objCustomField.IsRequired = True
                                _contactFormFields.Add(objCustomField)

                            Case SendToFriendFieldType.Message
                                Dim objCustomField As New ContactFieldInfo
                                objCustomField.ContactFieldID = SendToFriendFieldType.Message
                                objCustomField.Name = "Message"
                                objCustomField.Caption = "Message"
                                objCustomField.CaptionHelp = "Message.Help"
                                objCustomField.FieldType = CustomFieldType.MultiLineTextBox ' .RichTextBox
                                objCustomField.DefaultValue = String.Empty
                                objCustomField.IsRequired = False
                                _contactFormFields.Add(objCustomField)

                        End Select
                    Next
                End If
                Return _contactFormFields
            End Get
        End Property

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/SendToFriendForm"
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindDetails()

            rptDetails.DataSource = SendToFriendFormFields
            rptDetails.DataBind()

            tblSendToFriendForm.Width = Unit.Pixel(PropertySettings.FriendWidth)
            cmdSubmit.Text = Localization.GetString("cmdSubmit", ResourceFile)
            cmdSubmit.CssClass = PropertySettings.ButtonClass

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

                    phSendToFriendForm.Visible = True
                    cmdSubmit.ValidationGroup = ModuleKey & "-Friend"

                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

            Try

                lblSubmitResults.Text = String.Empty

                If Page.IsValid Then

                    Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabID, ModuleID, ModuleKey)
                    Dim objMailCustomFields As List(Of ContactFieldInfo) = SendToFriendFormFields
                    Dim fromEmail As String = ""
                    Dim toEmail As String = ""
                    Dim message As String = ""

                    ' Assign values from from to mail custom field collection
                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then

                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                                For Each objCustomField As ContactFieldInfo In objMailCustomFields
                                    If (objCustomField.ContactFieldID = Convert.ToInt32(objControl.ID)) Then
                                        If (objCustomField.FieldType = CustomFieldType.OneLineTextBox OrElse objCustomField.FieldType = CustomFieldType.MultiLineTextBox) Then
                                            objCustomField.DefaultValue = CType(objControl, TextBox).Text
                                            If (objCustomField.Name = "FromEmail") Then
                                                fromEmail = objCustomField.DefaultValue
                                            End If
                                            If (objCustomField.Name = "ToEmail") Then
                                                toEmail = objCustomField.DefaultValue
                                            End If
                                            If (objCustomField.Name = "Message") Then
                                                message = objCustomField.DefaultValue
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    ' Get the layout for subject and body
                    Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.SendToFriendEmail_Subject_Html)
                    Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.SendToFriendEmail_Body_Html)

                    ' Get the processed layout for subject and body
                    Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, CurrentProperty, CustomFields, objMailCustomFields, False)
                    Dim subject As String = RenderControlAsString(phProperty).Replace("[MESSAGE]", message)
                    phProperty = New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, CurrentProperty, CustomFields, objMailCustomFields, False)
                    Dim body As String = RenderControlAsString(phProperty).Replace("[MESSAGE]", message)
                    phProperty = Nothing

                    Dim replyTo As String = ""
                    If (fromEmail <> "") Then
                        replyTo = fromEmail
                    Else
                        replyTo = PortalSettings.Email
                    End If

                    Dim contactBCC As String = PropertySettings.FriendBCC
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

                    Dim mailTo As String = toEmail
                    If mailTo <> "" AndAlso replyTo <> "" Then
                        For Each mailItem As String In mailTo.Split(","c)
                            DotNetNuke.Services.Mail.Mail.SendMail(replyTo, mailItem, "", contactBCC,
                                DotNetNuke.Services.Mail.MailPriority.Normal,
                                subject,
                                DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                                "", DotNetNuke.Entities.Host.Host.SMTPServer, DotNetNuke.Entities.Host.Host.SMTPAuthentication, DotNetNuke.Entities.Host.Host.SMTPUsername, DotNetNuke.Entities.Host.Host.SMTPPassword)
                        Next
                        lblSubmitResults.Text = Localization.GetString("EmailSent.Message", Me.ResourceFile)
                        lblSubmitResults.CssClass = "Normal"
                    Else
                        lblSubmitResults.Text = Localization.GetString("EmailNotSent.Message", Me.ResourceFile)
                        lblSubmitResults.CssClass = "NormalRed"
                    End If

                    For Each item As RepeaterItem In rptDetails.Items
                        Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)
                        If Not (phValue Is Nothing) Then
                            If (phValue.Controls.Count > 0) Then
                                Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                                For Each objCustomField As ContactFieldInfo In objMailCustomFields
                                    If (objCustomField.ContactFieldID = Convert.ToInt32(objControl.ID)) Then
                                        If (objCustomField.FieldType = CustomFieldType.OneLineTextBox OrElse objCustomField.FieldType = CustomFieldType.MultiLineTextBox) Then
                                            objCustomField.DefaultValue = ""
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptDetails_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDetails.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objCustomField As ContactFieldInfo = CType(e.Item.DataItem, ContactFieldInfo)
                Dim phValue As PlaceHolder = CType(e.Item.FindControl("phValue"), PlaceHolder)

                Dim cmdHelp As LinkButton = CType(e.Item.FindControl("cmdHelp"), LinkButton)
                Dim pnlHelp As Panel = CType(e.Item.FindControl("pnlHelp"), Panel)
                Dim lblLabel As Label = CType(e.Item.FindControl("lblLabel"), Label)
                Dim lblHelp As Label = CType(e.Item.FindControl("lblHelp"), Label)
                Dim imgHelp As Image = CType(e.Item.FindControl("imgHelp"), Image)
                Dim lblRecipientMessage As Label = CType(e.Item.FindControl("lblRecipientMessage"), Label)

                If Not (phValue Is Nothing) Then

                    If (objCustomField.ContactFieldID = ContactDefaultFieldType.Email) Then
                        e.Item.Visible = Not Me.PropertySettings.ContactHideEmail
                    End If

                    If (objCustomField.ContactFieldID = ContactDefaultFieldType.Phone) Then
                        e.Item.Visible = Not Me.PropertySettings.ContactHidePhone
                    End If

                    cmdHelp.Visible = Not PropertySettings.SearchHideHelpIcon

                    DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelp, pnlHelp, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                    lblLabel.Text = Localization.GetString(objCustomField.Caption, Me.ResourceFile) & ":"
                    lblHelp.Text = Localization.GetString(objCustomField.CaptionHelp, Me.ResourceFile)
                    imgHelp.AlternateText = Localization.GetString(objCustomField.CaptionHelp, Me.ResourceFile)

                    If (objCustomField.Name = "ToEmail") Then
                        lblRecipientMessage.Visible = True
                        lblRecipientMessage.Text = Localization.GetString("RecipientMessage", ResourceFile)
                    End If

                    Select Case (objCustomField.FieldType)

                        Case CustomFieldType.OneLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.ContactFieldID.ToString()
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
                                valRequired.ValidationGroup = ModuleKey & "-Friend"
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                        Case CustomFieldType.MultiLineTextBox

                            Dim objTextBox As New TextBox
                            objTextBox.TextMode = TextBoxMode.MultiLine
                            objTextBox.CssClass = "NormalTextBox"
                            objTextBox.ID = objCustomField.ContactFieldID.ToString()
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
                                valRequired.ValidationGroup = ModuleKey & "-Friend"
                                valRequired.SetFocusOnError = True
                                phValue.Controls.Add(valRequired)
                            End If

                            If (objCustomField.Name = "ToEmail") Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = "<div align=""center""><a href=""#"" onclick=""showPlaxoABChooser('" & objTextBox.ClientID & "', '" & Page.ResolveUrl("~/desktopmodules/propertyagent/includes/plaxo_cb.html") & "'); return false""><img src=""http://www.plaxo.com/images/abc/buttons/add_button.gif"" alt=""Add from my address book"" border=""0"" /></a></div>"
                                phValue.Controls.Add(objLiteral)
                                litEmailTo1.Text = objTextBox.ClientID
                                litEmailTo2.Text = objTextBox.ClientID
                            End If

                    End Select

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace
