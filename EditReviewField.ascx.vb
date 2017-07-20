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

    Partial Public Class EditReviewField
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _reviewFieldID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub AdjustFieldElements()

            Dim objFieldType As ReviewFieldType = CType(System.Enum.Parse(GetType(ReviewFieldType), drpFieldType.SelectedIndex.ToString()), ReviewFieldType)

            Select Case objFieldType

                Case ReviewFieldType.CheckBox
                    phRequired.Visible = True
                    trFieldElements.Visible = False
                    trMaximumLength.Visible = False

                Case ReviewFieldType.DropDownList
                    phRequired.Visible = True
                    trFieldElements.Visible = True
                    trMaximumLength.Visible = False

                Case ReviewFieldType.MultiCheckBox
                    phRequired.Visible = True
                    trFieldElements.Visible = True
                    trMaximumLength.Visible = False

                Case ReviewFieldType.MultiLineTextBox
                    phRequired.Visible = True
                    trFieldElements.Visible = False
                    trMaximumLength.Visible = True

                Case ReviewFieldType.OneLineTextBox
                    phRequired.Visible = True
                    trFieldElements.Visible = False
                    trMaximumLength.Visible = True

                Case ReviewFieldType.Rating
                    phRequired.Visible = True
                    trFieldElements.Visible = False
                    trMaximumLength.Visible = False

            End Select

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("ReviewFieldID") Is Nothing) Then
                _reviewFieldID = Convert.ToInt32(Request("ReviewFieldID"))
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCustomFieldManager As New CrumbInfo
            objCustomFieldManager.Caption = Localization.GetString("ReviewFieldManager", LocalResourceFile)
            objCustomFieldManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields")
            crumbs.Add(objCustomFieldManager)

            Dim objReviewFieldEdit As New CrumbInfo
            If (_reviewFieldID <> Null.NullInteger) Then
                objReviewFieldEdit.Caption = Localization.GetString("EditReviewField", LocalResourceFile)
            Else
                objReviewFieldEdit.Caption = Localization.GetString("AddReviewField", LocalResourceFile)
            End If
            objReviewFieldEdit.Url = Request.RawUrl
            crumbs.Add(objReviewFieldEdit)

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

            For Each value As Integer In System.Enum.GetValues(GetType(ReviewFieldType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(ReviewFieldType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ReviewFieldType), value), Me.LocalResourceFile)
                drpFieldType.Items.Add(li)
            Next

        End Sub

        Private Sub BindReviewField()

            If (_reviewFieldID = Null.NullInteger) Then

                cmdDelete.Visible = False

            Else

                Dim objReviewFieldController As New ReviewFieldController
                Dim objReviewFieldInfo As ReviewFieldInfo = objReviewFieldController.Get(_reviewFieldID)

                If Not (objReviewFieldInfo Is Nothing) Then

                    txtName.Text = objReviewFieldInfo.Name
                    txtCaption.Text = objReviewFieldInfo.Caption
                    txtCaptionHelp.Text = objReviewFieldInfo.CaptionHelp
                    If Not (drpFieldType.Items.FindByValue(objReviewFieldInfo.FieldType.ToString()) Is Nothing) Then
                        drpFieldType.SelectedValue = objReviewFieldInfo.FieldType.ToString()
                    End If
                    AdjustFieldElements()

                    txtFieldElements.Text = objReviewFieldInfo.FieldElements

                    txtDefaultValue.Text = objReviewFieldInfo.DefaultValue
                    If (objReviewFieldInfo.Length <> Null.NullInteger) Then
                        txtMaximumLength.Text = objReviewFieldInfo.Length
                    End If

                    chkRequired.Checked = objReviewFieldInfo.IsRequired

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

                    BindFieldTypes()
                    BindReviewField()
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

                    Dim objReviewFieldController As New ReviewFieldController
                    Dim objReviewFieldInfo As New ReviewFieldInfo

                    objReviewFieldInfo.ModuleID = Me.ModuleId

                    objReviewFieldInfo.Name = txtName.Text
                    objReviewFieldInfo.Caption = txtCaption.Text
                    objReviewFieldInfo.CaptionHelp = txtCaptionHelp.Text
                    objReviewFieldInfo.FieldType = CType(System.Enum.Parse(GetType(ReviewFieldType), drpFieldType.SelectedIndex.ToString()), ReviewFieldType)
                    objReviewFieldInfo.FieldElements = txtFieldElements.Text

                    objReviewFieldInfo.DefaultValue = txtDefaultValue.Text
                    If (txtMaximumLength.Text.Trim() = "") Then
                        objReviewFieldInfo.Length = Null.NullInteger
                    Else
                        objReviewFieldInfo.Length = Convert.ToInt32(txtMaximumLength.Text)
                        If (objReviewFieldInfo.Length <= 0) Then
                            objReviewFieldInfo.Length = Null.NullInteger
                        End If
                    End If

                    objReviewFieldInfo.IsRequired = chkRequired.Checked

                    If (_reviewFieldID = Null.NullInteger) Then

                        Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(Me.ModuleId)

                        If (objReviewFields.Count = 0) Then
                            objReviewFieldInfo.SortOrder = 0
                        Else
                            objReviewFieldInfo.SortOrder = CType(objReviewFields(objReviewFields.Count - 1), ReviewFieldInfo).SortOrder + 1
                        End If

                        objReviewFieldInfo.ReviewFieldID = objReviewFieldController.Add(objReviewFieldInfo)

                    Else

                        Dim objReviewFieldInfoOld As ReviewFieldInfo = objReviewFieldController.Get(_reviewFieldID)

                        objReviewFieldInfo.SortOrder = objReviewFieldInfoOld.SortOrder
                        objReviewFieldInfo.ReviewFieldID = _reviewFieldID
                        objReviewFieldController.Update(objReviewFieldInfo)

                    End If

                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objReviewFieldController As New ReviewFieldController
                objReviewFieldController.Delete(_reviewFieldID)

                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields"), True)

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