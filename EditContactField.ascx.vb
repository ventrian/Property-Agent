'
' Property Agent for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2007
' by Ventrian Systems ( support@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security.Roles

Namespace Ventrian.PropertyAgent

    Partial Public Class EditContactField
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _contactFieldID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub AdjustFieldElements()

            Dim objFieldType As ContactFieldType = CType(System.Enum.Parse(GetType(ContactFieldType), drpFieldType.SelectedValue.ToString()), ContactFieldType)

            Select Case objFieldType

                Case ContactFieldType.DropDownList
                    phRequired.Visible = True
                    trFieldElements.Visible = True
                    trMaximumLength.Visible = False
                    trCustomField.Visible = False

                Case ContactFieldType.MultiCheckBox
                    phRequired.Visible = True
                    trFieldElements.Visible = True
                    trMaximumLength.Visible = False
                    trCustomField.Visible = False

                Case ContactFieldType.MultiLineTextBox
                    phRequired.Visible = True
                    trFieldElements.Visible = False
                    trMaximumLength.Visible = True
                    trCustomField.Visible = False

                Case ContactFieldType.OneLineTextBox
                    phRequired.Visible = True
                    trFieldElements.Visible = False
                    trMaximumLength.Visible = True
                    trCustomField.Visible = False

                Case ContactFieldType.CustomField
                    phRequired.Visible = True
                    trFieldElements.Visible = False
                    trMaximumLength.Visible = False
                    trCustomField.Visible = True

            End Select

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("ContactFieldID") Is Nothing) Then
                _ContactFieldID = Convert.ToInt32(Request("ContactFieldID"))
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCustomFieldManager As New CrumbInfo
            objCustomFieldManager.Caption = Localization.GetString("ContactFieldManager", LocalResourceFile)
            objCustomFieldManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields")
            crumbs.Add(objCustomFieldManager)

            Dim objContactFieldEdit As New CrumbInfo
            If (_ContactFieldID <> Null.NullInteger) Then
                objContactFieldEdit.Caption = Localization.GetString("EditContactField", LocalResourceFile)
            Else
                objContactFieldEdit.Caption = Localization.GetString("AddContactField", LocalResourceFile)
            End If
            objContactFieldEdit.Url = Request.RawUrl
            crumbs.Add(objContactFieldEdit)

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

        Private Sub BindFieldTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(ContactFieldType))
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ContactFieldType), value), Me.LocalResourceFile)

                If (value = ContactFieldType.CustomField) Then
                    If (drpCustomField.Items.Count > 0) Then
                        drpFieldType.Items.Add(li)
                    End If
                Else
                    drpFieldType.Items.Add(li)
                End If
            Next

        End Sub

        Private Sub BindCustomFields()

            For Each objCustomField As CustomFieldInfo In CustomFields
                If (objCustomField.FieldType = CustomFieldType.DropDownList Or objCustomField.FieldType = CustomFieldType.ListBox) Then
                    Dim li As New ListItem
                    li.Value = objCustomField.CustomFieldID.ToString()
                    li.Text = objCustomField.Name
                    drpCustomField.Items.Add(li)
                End If
            Next

        End Sub

        Private Sub BindContactField()

            If (_ContactFieldID = Null.NullInteger) Then

                cmdDelete.Visible = False

            Else

                Dim objContactFieldController As New ContactFieldController
                Dim objContactFieldInfo As ContactFieldInfo = objContactFieldController.Get(_ContactFieldID)

                If Not (objContactFieldInfo Is Nothing) Then

                    txtName.Text = objContactFieldInfo.Name
                    txtCaption.Text = objContactFieldInfo.Caption
                    txtCaptionHelp.Text = objContactFieldInfo.CaptionHelp
                    If Not (drpFieldType.Items.FindByValue(Convert.ToInt32(objContactFieldInfo.FieldType).ToString()) Is Nothing) Then
                        drpFieldType.SelectedValue = Convert.ToInt32(objContactFieldInfo.FieldType).ToString()
                    End If
                    If (drpCustomField.Items.FindByValue(objContactFieldInfo.CustomFieldID.ToString()) IsNot Nothing) Then
                        drpCustomField.SelectedValue = objContactFieldInfo.CustomFieldID.ToString()
                    End If
                    AdjustFieldElements()

                    txtFieldElements.Text = objContactFieldInfo.FieldElements

                    txtDefaultValue.Text = objContactFieldInfo.DefaultValue
                    If (objContactFieldInfo.Length <> Null.NullInteger) Then
                        txtMaximumLength.Text = objContactFieldInfo.Length
                    End If

                    chkRequired.Checked = objContactFieldInfo.IsRequired

                End If

            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (Page.IsPostBack = False) Then

                    BindCustomFields()
                    BindFieldTypes()
                    BindContactField()
                    Page.SetFocus(txtName)
                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")
                    AdjustFieldElements()

                End If

                cmdUpdate.CssClass = PropertySettings.ButtonClass
                cmdCancel.CssClass = PropertySettings.ButtonClass
                cmdDelete.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Dim objContactFieldController As New ContactFieldController
                    Dim objContactFieldInfo As New ContactFieldInfo

                    objContactFieldInfo.ModuleID = Me.ModuleId

                    objContactFieldInfo.Name = txtName.Text
                    objContactFieldInfo.Caption = txtCaption.Text
                    objContactFieldInfo.CaptionHelp = txtCaptionHelp.Text
                    objContactFieldInfo.FieldType = CType(System.Enum.Parse(GetType(ContactFieldType), drpFieldType.SelectedValue.ToString()), ContactFieldType)
                    objContactFieldInfo.FieldElements = txtFieldElements.Text

                    objContactFieldInfo.CustomFieldID = Null.NullInteger
                    If (drpCustomField.Visible) Then
                        If (drpCustomField.Items.Count > 0) Then
                            objContactFieldInfo.CustomFieldID = Convert.ToInt32(drpCustomField.SelectedValue)
                        End If
                    End If

                    objContactFieldInfo.DefaultValue = txtDefaultValue.Text
                    If (txtMaximumLength.Text.Trim() = "") Then
                        objContactFieldInfo.Length = Null.NullInteger
                    Else
                        objContactFieldInfo.Length = Convert.ToInt32(txtMaximumLength.Text)
                        If (objContactFieldInfo.Length <= 0) Then
                            objContactFieldInfo.Length = Null.NullInteger
                        End If
                    End If

                    objContactFieldInfo.IsRequired = chkRequired.Checked

                    If (_contactFieldID = Null.NullInteger) Then

                        Dim objContactFields As List(Of ContactFieldInfo) = objContactFieldController.List(Me.ModuleId)

                        If (objContactFields.Count = 0) Then
                            objContactFieldInfo.SortOrder = 0
                        Else
                            objContactFieldInfo.SortOrder = CType(objContactFields(objContactFields.Count - 1), ContactFieldInfo).SortOrder + 1
                        End If

                        objContactFieldInfo.ContactFieldID = objContactFieldController.Add(objContactFieldInfo)

                    Else

                        Dim objContactFieldInfoOld As ContactFieldInfo = objContactFieldController.Get(_contactFieldID)

                        objContactFieldInfo.SortOrder = objContactFieldInfoOld.SortOrder
                        objContactFieldInfo.ContactFieldID = _contactFieldID
                        objContactFieldController.Update(objContactFieldInfo)

                    End If

                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objContactFieldController As New ContactFieldController
                objContactFieldController.Delete(_contactFieldID)

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpFieldType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpFieldType.SelectedIndexChanged

            Try

                AdjustFieldElements()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace