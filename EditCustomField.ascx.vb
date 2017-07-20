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

    Partial Public Class EditCustomField
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _customFieldID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("CustomFieldID") Is Nothing) Then
                _customFieldID = Convert.ToInt32(Request("CustomFieldID"))
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCustomFieldManager As New CrumbInfo
            objCustomFieldManager.Caption = Localization.GetString("CustomFieldManager", LocalResourceFile)
            objCustomFieldManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields")
            crumbs.Add(objCustomFieldManager)

            Dim objCustomFieldEdit As New CrumbInfo
            If (_customFieldID <> Null.NullInteger) Then
                objCustomFieldEdit.Caption = Localization.GetString("EditCustomField", LocalResourceFile)
            Else
                objCustomFieldEdit.Caption = Localization.GetString("AddCustomField", LocalResourceFile)
            End If
            objCustomFieldEdit.Url = Request.RawUrl
            crumbs.Add(objCustomFieldEdit)

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

        Private Sub BindFieldElementTypes()

            Dim objPropertyTypeController As New PropertyTypeController()
            rptFieldElementsType.DataSource = objPropertyTypeController.ListAll(Me.ModuleId, True, Me.PropertySettings.TypesSortBy, Null.NullString())
            rptFieldElementsType.DataBind()

            drpCustomFieldsDropDown.Items.Clear()
            For Each objCustomField As CustomFieldInfo In CustomFields
                If (objCustomField.FieldType = CustomFieldType.DropDownList And objCustomField.FieldElementType = FieldElementType.Standard And objCustomField.CustomFieldID <> _customFieldID) Then
                    drpCustomFieldsDropDown.Items.Add(New ListItem(objCustomField.Name, objCustomField.CustomFieldID))
                End If
            Next

            For Each value As Integer In System.Enum.GetValues(GetType(FieldElementType))
                Dim addField As Boolean = True
                If (value = 2) Then
                    addField = False
                    If (drpCustomFieldsDropDown.Items.Count > 0) Then
                        addField = True
                    End If
                End If
                If (addField) Then
                    Dim li As New ListItem
                    li.Value = System.Enum.GetName(GetType(FieldElementType), value)
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(FieldElementType), value), Me.LocalResourceFile)
                    If (value = FieldElementType.Standard) Then
                        li.Selected = True
                    End If
                    lstFieldElementType.Items.Add(li)
                End If
            Next

        End Sub

        Private Sub BindFieldTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(CustomFieldType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CustomFieldType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CustomFieldType), value), Me.LocalResourceFile)
                drpFieldType.Items.Add(li)
            Next

        End Sub

        Private Sub BindValidationTypes()

            For Each value As Integer In System.Enum.GetValues(GetType(CustomFieldValidationType))
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CustomFieldValidationType), value), Me.LocalResourceFile)
                drpValidationType.Items.Add(li)
            Next

        End Sub

        Private Sub BindSearchTypes()

            drpSearchType.Items.Clear()

            Dim objFieldType As CustomFieldType = CType(System.Enum.Parse(GetType(CustomFieldValidationType), drpFieldType.SelectedIndex.ToString()), CustomFieldType)

            For Each value As Integer In System.Enum.GetValues(GetType(SearchType))
                If (value = 0 Or value = 3 Or (value = 1 And (objFieldType = CustomFieldType.DropDownList Or objFieldType = CustomFieldType.RadioButton)) Or (value = 2 And (objFieldType = CustomFieldType.OneLineTextBox)) Or (value = 3 And (objFieldType = CustomFieldType.OneLineTextBox Or objFieldType = CustomFieldType.MultiCheckBox)) Or (value = 4 And (objFieldType = CustomFieldType.ListBox))) Then
                    Dim li As New ListItem
                    li.Value = value.ToString()
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(SearchType), value), Me.LocalResourceFile)
                    drpSearchType.Items.Add(li)

                    If (value = 2) Then
                        drpSearchType.AutoPostBack = True
                    End If
                Else
                    If (value = 2) Then
                        drpSearchType.AutoPostBack = False
                    End If
                End If
            Next

        End Sub

        Private Sub AdjustFieldElements()

            Dim objFieldType As CustomFieldType = CType(System.Enum.Parse(GetType(CustomFieldValidationType), drpFieldType.SelectedIndex.ToString()), CustomFieldType)

            Select Case objFieldType

                Case CustomFieldType.CheckBox
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = False
                    lstFieldElementType.Visible = False
                    trSortable.Visible = True
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.DropDownList
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = True
                    lstFieldElementType.Visible = True
                    trSortable.Visible = True
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = True
                    trHideZeroCount.Visible = True
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.ListBox
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = True
                    lstFieldElementType.Visible = False
                    trSortable.Visible = False
                    chkSortable.Checked = False
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.MultiCheckBox
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = True
                    lstFieldElementType.Visible = False
                    trSortable.Visible = False
                    chkSortable.Checked = False
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.MultiLineTextBox
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = False
                    lstFieldElementType.Visible = False
                    trSortable.Visible = False
                    chkSortable.Checked = False
                    trMaximumLength.Visible = True
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.OneLineTextBox
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = False
                    lstFieldElementType.Visible = False
                    trSortable.Visible = True
                    trMaximumLength.Visible = True
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.RadioButton
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = True
                    lstFieldElementType.Visible = False
                    trSortable.Visible = True
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.RichTextBox
                    phRequired.Visible = True
                    phSearch.Visible = True
                    trFieldElements.Visible = False
                    lstFieldElementType.Visible = False
                    trSortable.Visible = False
                    chkSortable.Checked = False
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.FileUpload
                    phRequired.Visible = True
                    phSearch.Visible = False
                    trFieldElements.Visible = False
                    lstFieldElementType.Visible = False
                    trSortable.Visible = False
                    chkSortable.Checked = False
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.Hyperlink
                    phRequired.Visible = True
                    phSearch.Visible = False
                    trFieldElements.Visible = False
                    lstFieldElementType.Visible = False
                    trSortable.Visible = True
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = False
                    phDisplayDetails.Visible = True

                Case CustomFieldType.Label
                    phRequired.Visible = False
                    phSearch.Visible = False
                    trFieldElements.Visible = False
                    lstFieldElementType.Visible = False
                    trSortable.Visible = False
                    trMaximumLength.Visible = False
                    trIncludeCount.Visible = False
                    trHideZeroCount.Visible = False
                    phLabelDetails.Visible = True
                    phDisplayDetails.Visible = False

            End Select

            pnlFieldElements.Visible = (lstFieldElementType.SelectedIndex = 0 Or lstFieldElementType.SelectedValue = "SqlQuery")
            pnlFieldElementsType.Visible = (lstFieldElementType.SelectedValue = "LinkedToPropertyType")
            pnlFieldElementsDropDown.Visible = (lstFieldElementType.SelectedValue = "LinkedToDropdown")

            lblFieldElementHelp.Visible = Not (lstFieldElementType.SelectedIndex = 0 Or lstFieldElementType.SelectedValue = "SqlQuery")
            lblFieldElementHelpStandard.Visible = (lstFieldElementType.SelectedIndex = 0 Or lstFieldElementType.SelectedValue = "SqlQuery")

        End Sub

        Private Sub AdjustSearchType()

            If (drpSearchType.SelectedValue = "2") Then
                trFieldElementsFrom.Visible = True
                trFieldElementsTo.Visible = True
            Else
                trFieldElementsFrom.Visible = False
                trFieldElementsTo.Visible = False
            End If

        End Sub

        Private Sub AdjustValidationType()

            If (drpValidationType.SelectedValue = CType(CustomFieldValidationType.Regex, Integer).ToString()) Then
                trRegex.Visible = True
            Else
                trRegex.Visible = False
            End If

        End Sub

        Private Sub BindCustomField()

            If (_customFieldID = Null.NullInteger) Then

                AdjustFieldElements()
                BindSearchTypes()
                AdjustSearchType()
                AdjustValidationType()
                cmdDelete.Visible = False

                trEditRoles.Visible = False

            Else

                Dim objCustomFieldController As New CustomFieldController
                Dim objCustomFieldInfo As CustomFieldInfo = objCustomFieldController.Get(_customFieldID)

                If Not (objCustomFieldInfo Is Nothing) Then

                    txtName.Text = objCustomFieldInfo.Name
                    txtCaption.Text = objCustomFieldInfo.Caption
                    txtCaptionHelp.Text = objCustomFieldInfo.CaptionHelp
                    If Not (drpFieldType.Items.FindByValue(objCustomFieldInfo.FieldType.ToString()) Is Nothing) Then
                        drpFieldType.SelectedValue = objCustomFieldInfo.FieldType.ToString()
                    End If

                    If Not (lstFieldElementType.Items.FindByValue(objCustomFieldInfo.FieldElementType.ToString()) Is Nothing) Then
                        lstFieldElementType.SelectedValue = objCustomFieldInfo.FieldElementType.ToString()
                    End If

                    AdjustFieldElements()

                    If (drpCustomFieldsDropDown.Items.FindByValue(objCustomFieldInfo.FieldElementDropDown.ToString()) IsNot Nothing) Then
                        drpCustomFieldsDropDown.SelectedValue = objCustomFieldInfo.FieldElementDropDown.ToString()

                        For Each objCustomField As CustomFieldInfo In CustomFields
                            If (objCustomField.CustomFieldID.ToString() = drpCustomFieldsDropDown.SelectedValue) Then
                                rptFieldElementDropDown.DataSource = objCustomField.FieldElements.Split("|"c)
                                rptFieldElementDropDown.DataBind()
                            End If
                        Next
                    End If

                    Select Case lstFieldElementType.SelectedValue
                        Case "Standard"
                            txtFieldElements.Text = objCustomFieldInfo.FieldElements.Replace("|", vbCrLf)
                            Exit Select

                        Case "LinkedToPropertyType"
                            Dim count As Integer = 0
                            For Each objItem As RepeaterItem In rptFieldElementsType.Items
                                If (objItem.ItemType = ListItemType.Item Or objItem.ItemType = ListItemType.AlternatingItem) Then
                                    Dim txtFieldElementTypes As TextBox = objItem.FindControl("txtFieldElementTypes")
                                    Dim elements() As String = objCustomFieldInfo.FieldElements.Split(vbCrLf)
                                    If (count < elements.Length) Then
                                        txtFieldElementTypes.Text = elements(count)
                                    End If
                                    count = count + 1
                                End If
                            Next
                            Exit Select

                        Case "LinkedToDropdown"
                            Dim count As Integer = 0
                            For Each objItem As RepeaterItem In rptFieldElementDropDown.Items
                                If (objItem.ItemType = ListItemType.Item Or objItem.ItemType = ListItemType.AlternatingItem) Then
                                    Dim txtFieldElementDropDown As TextBox = objItem.FindControl("txtFieldElementDropDown")
                                    Dim elements() As String = objCustomFieldInfo.FieldElements.Split(vbCrLf)
                                    If (count < elements.Length) Then
                                        txtFieldElementDropDown.Text = elements(count)
                                    End If
                                    count = count + 1
                                End If
                            Next
                            Exit Select

                        Case "SqlQuery"
                            txtFieldElements.Text = objCustomFieldInfo.FieldElements
                            Exit Select

                    End Select

                    txtDefaultValue.Text = objCustomFieldInfo.DefaultValue
                    chkPublished.Checked = objCustomFieldInfo.IsPublished
                    If (objCustomFieldInfo.Length <> Null.NullInteger) Then
                        txtMaximumLength.Text = objCustomFieldInfo.Length
                    End If

                    chkRequired.Checked = objCustomFieldInfo.IsRequired
                    If Not (drpValidationType.Items.FindByValue(CType(objCustomFieldInfo.ValidationType, Integer).ToString()) Is Nothing) Then
                        drpValidationType.SelectedValue = CType(objCustomFieldInfo.ValidationType, Integer).ToString()
                    End If
                    txtRegex.Text = objCustomFieldInfo.RegularExpression
                    AdjustValidationType()

                    chkSearchable.Checked = objCustomFieldInfo.IsSearchable

                    BindSearchTypes()
                    If Not (drpSearchType.Items.FindByValue(CType(objCustomFieldInfo.SearchType, Integer).ToString()) Is Nothing) Then
                        drpSearchType.SelectedValue = CType(objCustomFieldInfo.SearchType, Integer).ToString()
                    End If
                    txtFieldElementsFrom.Text = objCustomFieldInfo.FieldElementsFrom
                    txtFieldElementsTo.Text = objCustomFieldInfo.FieldElementsTo
                    chkIncludeCount.Checked = objCustomFieldInfo.IncludeCount
                    chkHideZeroCount.Checked = objCustomFieldInfo.HideZeroCount
                    AdjustSearchType()

                    chkSortable.Checked = objCustomFieldInfo.IsSortable
                    chkFeatured.Checked = objCustomFieldInfo.IsFeatured
                    chkListing.Checked = objCustomFieldInfo.IsInListing
                    chkManager.Checked = objCustomFieldInfo.IsInManager
                    chkCaptionHidden.Checked = objCustomFieldInfo.IsCaptionHidden
                    chkHidden.Checked = objCustomFieldInfo.IsHidden
                    chkLockDown.Checked = objCustomFieldInfo.IsLockDown

                    If (objCustomFieldInfo.FieldType = CustomFieldType.Label) Then
                        txtLabelDetails.Text = objCustomFieldInfo.FieldElements
                    End If

                    chkInheritSecurity.Checked = objCustomFieldInfo.InheritSecurity
                    trEditRoles.Visible = Not chkInheritSecurity.Checked

                    If (Settings.Contains(Constants.PERMISSION_CUSTOM_FIELD_SETTING & objCustomFieldInfo.CustomFieldID.ToString())) Then
                        Dim editRoles As String = Settings(Constants.PERMISSION_CUSTOM_FIELD_SETTING & objCustomFieldInfo.CustomFieldID.ToString()).ToString()

                        For Each role As String In editRoles.Split(";"c)

                            For Each item As ListItem In lstEditRoles.Items
                                If (item.Text.ToLower() = role.ToLower()) Then
                                    item.Selected = True
                                End If
                            Next

                        Next

                    End If

                End If

            End If

        End Sub

        Private Sub BindRoles()

            Dim objRoleController As New RoleController()

            lstEditRoles.DataSource = objRoleController.GetPortalRoles(Me.PortalId)
            lstEditRoles.DataBind()

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

                    BindFieldElementTypes()
                    BindFieldTypes()
                    BindRoles()
                    BindValidationTypes()
                    BindCustomField()

                    Page.SetFocus(txtName)
                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")

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

                    Dim objCustomFieldController As New CustomFieldController
                    Dim objCustomFieldInfo As New CustomFieldInfo

                    objCustomFieldInfo.ModuleID = Me.ModuleId

                    objCustomFieldInfo.Name = txtName.Text
                    objCustomFieldInfo.Caption = txtCaption.Text
                    objCustomFieldInfo.CaptionHelp = txtCaptionHelp.Text
                    objCustomFieldInfo.FieldType = CType(System.Enum.Parse(GetType(CustomFieldType), drpFieldType.SelectedValue.ToString()), CustomFieldType)
                    objCustomFieldInfo.FieldElements = txtFieldElements.Text.Replace(vbCrLf, "|")
                    objCustomFieldInfo.FieldElementType = CType(System.Enum.Parse(GetType(FieldElementType), lstFieldElementType.SelectedValue.ToString()), FieldElementType)
                    objCustomFieldInfo.FieldElementDropDown = Null.NullInteger

                    If (objCustomFieldInfo.FieldElementType = FieldElementType.SqlQuery) Then
                        objCustomFieldInfo.FieldElements = txtFieldElements.Text
                    End If

                    If (objCustomFieldInfo.FieldType = CustomFieldType.DropDownList And objCustomFieldInfo.FieldElementType = FieldElementType.LinkedToPropertyType) Then
                        Dim fieldElements As String = ""
                        For Each objItem As RepeaterItem In rptFieldElementsType.Items
                            If (objItem.ItemType = ListItemType.Item Or objItem.ItemType = ListItemType.AlternatingItem) Then
                                Dim txtFieldElementTypes As TextBox = objItem.FindControl("txtFieldElementTypes")
                                If (fieldElements.Length = 0) Then
                                    fieldElements = txtFieldElementTypes.Text.Replace(vbCrLf, "") & vbCrLf
                                Else
                                    fieldElements = fieldElements & txtFieldElementTypes.Text.Replace(vbCrLf, "") & vbCrLf
                                End If
                            End If
                        Next
                        objCustomFieldInfo.FieldElements = fieldElements
                    End If

                    If (objCustomFieldInfo.FieldType = CustomFieldType.DropDownList And objCustomFieldInfo.FieldElementType = FieldElementType.LinkedToDropdown) Then
                        If (drpCustomFieldsDropDown.Items.Count > 0) Then
                            objCustomFieldInfo.FieldElementDropDown = Convert.ToInt32(drpCustomFieldsDropDown.SelectedValue)
                        End If

                        Dim fieldElements As String = ""
                        For Each objItem As RepeaterItem In rptFieldElementDropDown.Items
                            If (objItem.ItemType = ListItemType.Item Or objItem.ItemType = ListItemType.AlternatingItem) Then
                                Dim txtFieldElementDropDown As TextBox = objItem.FindControl("txtFieldElementDropDown")
                                If (fieldElements.Length = 0) Then
                                    fieldElements = txtFieldElementDropDown.Text.Replace(vbCrLf, "") & vbCrLf
                                Else
                                    fieldElements = fieldElements & txtFieldElementDropDown.Text.Replace(vbCrLf, "") & vbCrLf
                                End If
                            End If
                        Next
                        objCustomFieldInfo.FieldElements = fieldElements
                    End If

                    objCustomFieldInfo.DefaultValue = txtDefaultValue.Text
                    objCustomFieldInfo.IsPublished = chkPublished.Checked
                    If (txtMaximumLength.Text.Trim() = "") Then
                        objCustomFieldInfo.Length = Null.NullInteger
                    Else
                        objCustomFieldInfo.Length = Convert.ToInt32(txtMaximumLength.Text)
                        If (objCustomFieldInfo.Length <= 0) Then
                            objCustomFieldInfo.Length = Null.NullInteger
                        End If
                    End If

                    objCustomFieldInfo.IsRequired = chkRequired.Checked
                    objCustomFieldInfo.ValidationType = CType(System.Enum.Parse(GetType(CustomFieldValidationType), drpValidationType.SelectedIndex.ToString()), CustomFieldValidationType)
                    objCustomFieldInfo.RegularExpression = txtRegex.Text

                    objCustomFieldInfo.IsSearchable = chkSearchable.Checked
                    objCustomFieldInfo.SearchType = CType(System.Enum.Parse(GetType(SearchType), drpSearchType.SelectedValue.ToString()), SearchType)
                    objCustomFieldInfo.FieldElementsFrom = txtFieldElementsFrom.Text
                    objCustomFieldInfo.FieldElementsTo = txtFieldElementsTo.Text
                    objCustomFieldInfo.IncludeCount = chkIncludeCount.Checked
                    objCustomFieldInfo.HideZeroCount = chkHideZeroCount.Checked

                    objCustomFieldInfo.IsSortable = chkSortable.Checked
                    objCustomFieldInfo.IsFeatured = chkFeatured.Checked
                    objCustomFieldInfo.IsInListing = chkListing.Checked
                    objCustomFieldInfo.IsInManager = chkManager.Checked
                    objCustomFieldInfo.IsCaptionHidden = chkCaptionHidden.Checked
                    objCustomFieldInfo.IsHidden = chkHidden.Checked
                    objCustomFieldInfo.IsLockDown = chkLockDown.Checked

                    objCustomFieldInfo.InheritSecurity = chkInheritSecurity.Checked

                    If (objCustomFieldInfo.FieldType = CustomFieldType.Label) Then
                        objCustomFieldInfo.FieldElements = txtLabelDetails.Text
                    End If

                    If (_customFieldID = Null.NullInteger) Then

                        Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleId, Null.NullBoolean)

                        If (objCustomFields.Count = 0) Then
                            objCustomFieldInfo.SortOrder = 0
                        Else
                            objCustomFieldInfo.SortOrder = CType(objCustomFields(objCustomFields.Count - 1), CustomFieldInfo).SortOrder + 1
                        End If

                        objCustomFieldInfo.CustomFieldID = objCustomFieldController.Add(objCustomFieldInfo)

                    Else

                        Dim objCustomFieldInfoOld As CustomFieldInfo = objCustomFieldController.Get(_customFieldID)

                        objCustomFieldInfo.SortOrder = objCustomFieldInfoOld.SortOrder
                        objCustomFieldInfo.CustomFieldID = _customFieldID
                        objCustomFieldController.Update(objCustomFieldInfo)

                    End If

                    Dim objModules As New DotNetNuke.Entities.Modules.ModuleController

                    If (objCustomFieldInfo.InheritSecurity = False) Then

                        Dim editRoles As String = ""

                        For Each li As ListItem In lstEditRoles.Items

                            If (li.Selected) Then

                                If (editRoles = "") Then
                                    editRoles = li.Text
                                Else
                                    editRoles = editRoles & ";" & li.Text
                                End If

                            End If

                        Next

                        objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_CUSTOM_FIELD_SETTING & objCustomFieldInfo.CustomFieldID.ToString(), editRoles)

                    Else

                        objModules.UpdateModuleSetting(ModuleId, Constants.PERMISSION_CUSTOM_FIELD_SETTING & objCustomFieldInfo.CustomFieldID.ToString(), Null.NullString())

                    End If

                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objCustomFieldController As New CustomFieldController
                objCustomFieldController.Delete(_customFieldID)

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpFieldType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpFieldType.SelectedIndexChanged

            Try

                AdjustFieldElements()
                BindSearchTypes()
                AdjustSearchType()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpSearchType.SelectedIndexChanged

            Try

                AdjustSearchType()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpValidationType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpValidationType.SelectedIndexChanged

            Try

                AdjustValidationType()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valFieldElementsFrom_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valFieldElementsFrom.ServerValidate

            Try

                If (trFieldElementsFrom.Visible = False) Then
                    args.IsValid = True
                    Return
                End If

                If (txtFieldElementsFrom.Text = "") Then
                    args.IsValid = True
                    Return
                End If

                Dim objValidationType As CustomFieldValidationType = CType(System.Enum.Parse(GetType(CustomFieldValidationType), drpValidationType.SelectedIndex.ToString()), CustomFieldValidationType)
                args.IsValid = True

                Dim values As String() = txtFieldElementsFrom.Text.Split(Convert.ToChar("|"))
                For Each value As String In values
                    Select Case objValidationType
                        Case CustomFieldValidationType.Currency
                            If (IsNumeric(value) = False) Then
                                args.IsValid = False
                            End If
                        Case CustomFieldValidationType.Date
                            If (IsDate(value) = False) Then
                                args.IsValid = False
                            End If
                        Case CustomFieldValidationType.Double
                            If (IsNumeric(value) = False) Then
                                args.IsValid = False
                            End If
                        Case CustomFieldValidationType.Integer
                            If (IsNumeric(value) = False) Then
                                args.IsValid = False
                            End If
                    End Select
                Next

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valFieldElementsTo_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valFieldElementsTo.ServerValidate

            Try

                If (trFieldElementsTo.Visible = False) Then
                    args.IsValid = True
                    Return
                End If

                If (txtFieldElementsTo.Text = "") Then
                    args.IsValid = True
                    Return
                End If

                Dim objValidationType As CustomFieldValidationType = CType(System.Enum.Parse(GetType(CustomFieldValidationType), drpValidationType.SelectedIndex.ToString()), CustomFieldValidationType)
                args.IsValid = True

                Dim values As String() = txtFieldElementsTo.Text.Split(Convert.ToChar("|"))
                For Each value As String In values
                    Select Case objValidationType
                        Case CustomFieldValidationType.Currency
                            If (IsNumeric(value) = False) Then
                                args.IsValid = False
                            End If
                        Case CustomFieldValidationType.Date
                            If (IsDate(value) = False) Then
                                args.IsValid = False
                            End If
                        Case CustomFieldValidationType.Double
                            If (IsNumeric(value) = False) Then
                                args.IsValid = False
                            End If
                        Case CustomFieldValidationType.Integer
                            If (IsNumeric(value) = False) Then
                                args.IsValid = False
                            End If
                    End Select
                Next

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub lstFieldElementType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstFieldElementType.SelectedIndexChanged

            pnlFieldElements.Visible = (lstFieldElementType.SelectedIndex = 0 Or lstFieldElementType.SelectedValue = "SqlQuery")
            pnlFieldElementsType.Visible = (lstFieldElementType.SelectedIndex = 1)
            pnlFieldElementsDropDown.Visible = (lstFieldElementType.SelectedValue = "LinkedToDropdown")

            lblFieldElementHelp.Visible = Not (lstFieldElementType.SelectedIndex = 0 Or lstFieldElementType.SelectedValue = "SqlQuery")
            lblFieldElementHelpStandard.Visible = (lstFieldElementType.SelectedIndex = 0 Or lstFieldElementType.SelectedValue = "SqlQuery")

            If (pnlFieldElementsDropDown.Visible) Then
                For Each objCustomField As CustomFieldInfo In CustomFields
                    If (objCustomField.CustomFieldID.ToString() = drpCustomFieldsDropDown.SelectedValue) Then
                        rptFieldElementDropDown.DataSource = objCustomField.FieldElements.Split("|"c)
                        rptFieldElementDropDown.DataBind()
                    End If
                Next
            End If

        End Sub

        Protected Sub drpCustomFieldsDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpCustomFieldsDropDown.SelectedIndexChanged

            For Each objCustomField As CustomFieldInfo In CustomFields
                If (objCustomField.CustomFieldID.ToString() = drpCustomFieldsDropDown.SelectedValue) Then
                    rptFieldElementDropDown.DataSource = objCustomField.FieldElements.Split("|"c)
                    rptFieldElementDropDown.DataBind()
                End If
            Next

        End Sub

        Protected Sub chkInheritSecurity_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkInheritSecurity.CheckedChanged

            Try

                trEditRoles.Visible = Not chkInheritSecurity.Checked

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace