Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Framework
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.UserControls

Imports System.IO
Imports System.Globalization
Imports Ventrian.PropertyAgent.Social

Namespace Ventrian.PropertyAgent

    Partial Public Class EditProperty
        Inherits PropertyAgentBase

#Region " Controls "

#End Region

#Region " Private Members "

        Private _propertyID As Integer = Null.NullInteger
        Private _propertyTypeID As Integer = Null.NullInteger
        Private _returnUrl As String = Null.NullString

        Private _property As PropertyInfo
        Private _richTextValues As New NameValueCollection

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            Dim propertyParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyParam) = "") Then
                propertyParam = "PropertyID"
            End If
            If Not (Request(propertyParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyParam))
            End If

            If Not (Request("ReturnUrl") Is Nothing) Then
                _returnUrl = Server.UrlDecode(Request("ReturnUrl"))
            End If

            If (Page.IsPostBack) Then
                If (IsNumeric(Request(drpTypes.ClientID.ToString().Replace("_", "$")))) Then
                    _propertyTypeID = Convert.ToInt32(Request(drpTypes.ClientID.ToString().Replace("_", "$")))
                End If
            End If

        End Sub

        Private Function StripNonAlphaNumericCharacters(ByVal Text As String) As String
            Dim sb As New System.Text.StringBuilder(Text.Length)
            Dim chr As Char
            For Each chr In Text
                If Char.IsLetterOrDigit(chr) Then
                    sb.Append(chr)
                End If
            Next chr
            Return sb.ToString()
        End Function

        Private Function RenderControlAsString(ByVal objControl As Control) As String

            Dim sb As New StringBuilder
            Dim tw As New StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            objControl.RenderControl(hw)

            Return sb.ToString()

        End Function

        Private Sub CheckSecurity()

            If (Request.IsAuthenticated = False) Then
                ' Only authenticated people can edit. 
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=AccessDenied"), True)
            End If

            If (IsEditable = False And PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = False And PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = False) Then
                If (_propertyID = Null.NullInteger Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=AccessDenied"), True)
                End If
            End If

            If (IsEditable = False And (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True Or PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True) And PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False) Then
                If (_propertyID <> Null.NullInteger) Then
                    Dim objPropertyController As New PropertyController
                    Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)

                    If Not (objProperty Is Nothing) Then
                        If (objProperty.AuthorID <> UserId And objProperty.BrokerID <> UserId) Then
                            Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=AccessDenied"), True)
                        End If
                    Else
                        Response.Redirect(NavigateURL(), True)
                    End If
                End If
            End If

            If (_propertyID = Null.NullInteger And CheckLimit() = False) Then
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager"), True)
            End If

        End Sub

        Private Sub BindPropertyTypes()

            Dim objPropertyTypeController As New PropertyTypeController

            drpTypes.DataSource = objPropertyTypeController.ListAll(Me.ModuleId, True, PropertySettings.TypesSortBy, Null.NullString())
            drpTypes.DataBind()

            drpTypes.Items.Insert(0, New ListItem(GetResourceString("SelectType"), "-1"))

            If (_propertyTypeID <> Null.NullInteger) Then
                If (drpTypes.Items.FindByValue(_propertyTypeID.ToString()) IsNot Nothing) Then
                    drpTypes.SelectedValue = _propertyTypeID.ToString()
                End If
            End If

            For Each objCustomField As CustomFieldInfo In CustomFields
                If (objCustomField.IsPublished AndAlso objCustomField.FieldType = CustomFieldType.DropDownList AndAlso objCustomField.FieldElementType = FieldElementType.LinkedToPropertyType) Then
                    drpTypes.AutoPostBack = True
                End If
            Next

        End Sub

        Private Sub BindDetails()

            trOwner.Visible = IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove)
            If (trOwner.Visible = False) Then
                If (PortalSecurity.IsInRoles(PropertySettings.PermissionBroker)) Then
                    Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)
                    Dim objAgents As ArrayList = objAgentController.ListSelected(PortalId, ModuleId, Me.UserId)
                    If (objAgents.Count > 0) Then
                        trOwner.Visible = True
                    End If
                End If
            End If
            phAuthorDetails.Visible = Not Me.PropertySettings.PropertyManagerHideAuthorDetails
            phPublishingDetails.Visible = IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionPublishDetail)

            If (PropertySettings.AgentDropdownDefault) Then
                lblOwner.Visible = False
                cmdChange.Visible = False
                drpOwner.Visible = True

                PopulateOwnerList()
            End If

            If _propertyID <> Null.NullInteger Then

                Dim objPropertyController As New PropertyController
                _property = objPropertyController.Get(_propertyID)

                If (_property Is Nothing) Then
                    Response.Redirect(NavigateURL(), True)
                End If

                If (Page.IsPostBack = False) Then

                    cmdClone.Visible = PortalSecurity.IsInRoles(PropertySettings.PermissionApprove)

                    If (PropertySettings.AgentDropdownDefault) Then
                        If (drpOwner.Items.FindByValue(_property.AuthorID.ToString()) IsNot Nothing) Then
                            drpOwner.SelectedValue = _property.AuthorID.ToString()
                        End If
                    End If

                    If Not (drpTypes.Items.FindByValue(_property.PropertyTypeID.ToString()) Is Nothing) Then
                        drpTypes.SelectedValue = _property.PropertyTypeID.ToString()
                    End If

                    txtCreationDate.Text = _property.DateCreated.ToShortDateString()
                    drpCreationTimeHour.SelectedValue = _property.DateCreated.Hour.ToString()
                    drpCreationTimeMinute.SelectedValue = _property.DateCreated.Minute.ToString()

                    If (_property.Latitude <> Null.NullDouble) Then
                        txtLatitude.Text = _property.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat)
                    End If

                    If (_property.Longitude <> Null.NullDouble) Then
                        txtLongitude.Text = _property.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)
                    End If

                    If (txtLatitude.Text <> "" And txtLongitude.Text <> "") Then

                        phMapLoad.Visible = True
                    Else
                        phMapLoad.Visible = False
                    End If

                    If (_property.DatePublished <> Null.NullDate) Then
                        txtPublishDate.Text = _property.DatePublished.ToShortDateString()
                        drpPublishTimeHour.SelectedValue = _property.DatePublished.Hour.ToString()
                        drpPublishTimeMinute.SelectedValue = _property.DatePublished.Minute.ToString()
                    Else
                        drpPublishTimeHour.SelectedValue = "-"
                        drpPublishTimeMinute.SelectedValue = "-"
                    End If

                    If (_property.DateExpired <> Null.NullDate) Then
                        txtExpiryDate.Text = _property.DateExpired.ToShortDateString()
                        If Not (drpExpiryTimeHour.Items.FindByValue(_property.DateExpired.Hour.ToString()) Is Nothing) Then
                            drpExpiryTimeHour.SelectedValue = _property.DateExpired.Hour.ToString()
                        End If
                        If Not (drpExpiryTimeMinute.Items.FindByValue(_property.DateExpired.Minute.ToString()) Is Nothing) Then
                            drpExpiryTimeMinute.SelectedValue = _property.DateExpired.Minute.ToString()
                        End If
                    Else
                        drpExpiryTimeHour.SelectedValue = "-"
                        drpExpiryTimeMinute.SelectedValue = "-"
                    End If

                    Select Case _property.Status

                        Case StatusType.Draft
                            chkPublished.Checked = False
                            chkApproved.Checked = False

                        Case StatusType.AwaitingApproval
                            chkPublished.Checked = True
                            chkApproved.Checked = False

                        Case StatusType.Published
                            chkPublished.Checked = True
                            chkApproved.Checked = True

                    End Select

                    If (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) And (IsEditable = False Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False)) Then
                        chkApproved.Enabled = False
                        chkApproved.Checked = False
                    End If

                    If (PortalSecurity.IsInRoles(PropertySettings.PermissionAutoApprove) And (IsEditable = False Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False)) Then
                        chkApproved.Enabled = False
                        chkApproved.Checked = True
                    End If

                    If (IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove)) Then
                        chkApproved.Enabled = True
                    End If

                    chkFeatured.Checked = _property.IsFeatured
                    chkOnlyForAuthenticated.Checked = _property.OnlyForAuthenticated

                    If (_property.Username <> "") Then
                        lblOwner.Text = _property.DisplayName & " (" & _property.Username & ")"
                        If (_property.BrokerID <> Null.NullInteger) Then
                            lblOwner.Text = _property.Username & " (" & PropertyUtil.FormatPropertyLabel(Localization.GetString("Broker", Me.LocalResourceFile), Me.PropertySettings) & " " & _property.BrokerUsername & ")"
                        End If
                    Else
                        lblOwner.Text = Localization.GetString("None_Specified")
                    End If

                    lblUsername.Text = _property.Username
                    lblDisplayName.Text = _property.DisplayName
                    lblEmail.Text = "<a href='mailto:" & _property.Email & "'>" + _property.Email + "</a>"

                End If

            Else

                phMapLoad.Visible = False

                txtCreationDate.Text = DateTime.Now.ToShortDateString()
                drpCreationTimeHour.SelectedValue = DateTime.Now.Hour.ToString()
                drpCreationTimeMinute.SelectedValue = DateTime.Now.Minute.ToString()

                txtPublishDate.Text = DateTime.Now.ToShortDateString()
                drpPublishTimeHour.SelectedValue = DateTime.Now.Hour.ToString()
                drpPublishTimeMinute.SelectedValue = DateTime.Now.Minute.ToString()

                If (PropertySettings.DefaultExpiration <> Null.NullInteger) Then
                    Dim expirationDate As DateTime = DateTime.Now

                    Select Case PropertySettings.DefaultExpirationPeriod
                        Case "D"
                            expirationDate = expirationDate.AddDays(Convert.ToInt32(PropertySettings.DefaultExpiration))
                            Exit Select
                        Case "M"
                            expirationDate = expirationDate.AddMonths(Convert.ToInt32(PropertySettings.DefaultExpiration))
                            Exit Select
                        Case "Y"
                            expirationDate = expirationDate.AddYears(Convert.ToInt32(PropertySettings.DefaultExpiration))
                            Exit Select
                        Case Else

                    End Select

                    txtExpiryDate.Text = expirationDate.ToShortDateString()

                    If Not (drpExpiryTimeHour.Items.FindByValue(expirationDate.Hour.ToString()) Is Nothing) Then
                        drpExpiryTimeHour.SelectedValue = expirationDate.Hour.ToString()
                    End If
                    If Not (drpExpiryTimeMinute.Items.FindByValue(expirationDate.Minute.ToString()) Is Nothing) Then
                        drpExpiryTimeMinute.SelectedValue = expirationDate.Minute.ToString()
                    End If
                End If

                If (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit)) Then
                    chkApproved.Enabled = False
                    chkApproved.Checked = False
                End If

                If (PortalSecurity.IsInRoles(PropertySettings.PermissionAutoApprove)) Then
                    chkApproved.Enabled = False
                    chkApproved.Checked = True
                End If

                If (IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove)) Then
                    chkApproved.Enabled = True
                    chkApproved.Checked = True
                End If

                If (PortalSecurity.IsInRoles(PropertySettings.PermissionAutoFeature)) Then
                    chkFeatured.Enabled = True
                    chkFeatured.Checked = True
                End If

                chkPublished.Checked = True

                phAuthorDetails.Visible = False

                lblOwner.Text = Me.UserInfo.DisplayName & " (" & Me.UserInfo.Username & ")"

                If (drpTypes.Items.Count = 2) Then
                    drpTypes.SelectedIndex = 1
                End If

                cmdClone.Visible = False

                If (PropertySettings.AgentDropdownDefault) Then
                    If (drpOwner.Items.FindByValue(DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID.ToString) IsNot Nothing) Then
                        drpOwner.SelectedValue = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID.ToString
                    End If
                End If

            End If

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            If (Request.IsAuthenticated) Then
                Dim objCrumbPropertyManager As New CrumbInfo
                objCrumbPropertyManager.Caption = GetResourceString("PropertyManager")
                objCrumbPropertyManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager")
                crumbs.Add(objCrumbPropertyManager)
            End If

            Dim objCrumbProperty As New CrumbInfo
            If (_propertyID <> Null.NullInteger) Then
                objCrumbProperty.Caption = GetResourceString("EditProperty")
                objCrumbProperty.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditProperty", PropertySettings.SEOPropertyID & "=" & _propertyID.ToString())
            Else
                objCrumbProperty.Caption = GetResourceString("AddNewProperty")
                objCrumbProperty.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditProperty")
            End If
            crumbs.Add(objCrumbProperty)

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

            rptDetails.DataSource = Me.CustomFields
            rptDetails.DataBind()

        End Sub

        Private Function FormatDefaultValue(ByVal defaultValue As String) As String

            'val = val.Replace("[DISPLAYNAME]", Me.UserInfo.DisplayName)
            'val = val.Replace("[FIRSTNAME]", Me.UserInfo.FirstName)
            'val = val.Replace("[EMAIL]", Me.UserInfo.Email)
            'val = val.Replace("[LASTNAME]", Me.UserInfo.LastName)
            'val = val.Replace("[USERID]", Me.UserInfo.UserID)
            'val = val.Replace("[USERNAME]", Me.UserInfo.Username)

            Dim objPlaceHolder As New PlaceHolder

            Dim delimStr As String = "[]"
            Dim delimiter As Char() = delimStr.ToCharArray()

            Dim layoutArray As String() = defaultValue.Split(delimiter)

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Controls.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "DISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = UserInfo.DisplayName
                            objPlaceHolder.Controls.Add(objLiteral)

                        Case "EMAIL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = UserInfo.Email
                            objPlaceHolder.Controls.Add(objLiteral)

                        Case "FIRSTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = UserInfo.FirstName
                            objPlaceHolder.Controls.Add(objLiteral)

                        Case "LASTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = UserInfo.LastName
                            objPlaceHolder.Controls.Add(objLiteral)

                        Case "USERID"
                            Dim objLiteral As New Literal
                            objLiteral.Text = UserInfo.UserID
                            objPlaceHolder.Controls.Add(objLiteral)

                        Case "USERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = UserInfo.Username
                            objPlaceHolder.Controls.Add(objLiteral)

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("PROFILE:")) Then
                                Dim field As String = layoutArray(iPtr + 1).Substring(8, layoutArray(iPtr + 1).Length - 8)

                                Dim objLiteral As New Literal
                                objLiteral.Text = UserInfo.Profile.GetPropertyValue(field)
                                objPlaceHolder.Controls.Add(objLiteral)
                            End If

                    End Select
                End If
            Next

            Return RenderControlAsString(objPlaceHolder)

        End Function

        Private Sub SetLockDown()

            If Me.PropertySettings.LockDownPropertyType AndAlso Not PortalSecurity.IsInRoles(PropertySettings.PermissionLockDown) Then
                drpTypes.Enabled = False
            End If

            If Me.PropertySettings.LockDownPropertyDates AndAlso Not PortalSecurity.IsInRoles(PropertySettings.PermissionLockDown) Then
                ' Creation date
                drpCreationTimeHour.Enabled = False
                drpCreationTimeMinute.Enabled = False
                txtCreationDate.Enabled = False
                cmdCreationDate.Enabled = False
                ' Start date
                drpPublishTimeHour.Enabled = False
                drpPublishTimeMinute.Enabled = False
                txtPublishDate.Enabled = False
                cmdPublishDate.Enabled = False
                ' End date
                drpExpiryTimeHour.Enabled = False
                drpExpiryTimeMinute.Enabled = False
                txtExpiryDate.Enabled = False
                cmdExpiryDate.Enabled = False
            End If

            If Me.PropertySettings.LockDownFeatured AndAlso Not PortalSecurity.IsInRoles(PropertySettings.PermissionLockDown) Then
                chkFeatured.Enabled = False
            End If

        End Sub

        Private Sub LocalizeLabels()

            CType(dshPropertyDetails, SectionHeadControl).Text = GetResourceString("PropertyDetails")
            lblPropertyDetailsHelp.Text = GetResourceString("PropertyDetailsDescription")

            CType(plType, LabelControl).Text = GetResourceString("Type") & "*:"
            CType(plType, LabelControl).HelpText = GetResourceString("Type.Help")
            valPropertyType.ErrorMessage = GetResourceString("valTypeRequired")

            lblPublishDetails.Text = GetResourceString("PublishDetailsDescription")
            CType(plPublishDate, LabelControl).HelpText = GetResourceString("PublishDate.Help")
            CType(plExpiryDate, LabelControl).HelpText = GetResourceString("ExpiryDate.Help")

            lblAuthorDetails.Text = GetResourceString("AuthorDetailsDescription")

            CType(plOwner, LabelControl).Text = GetResourceString("Agent")
            CType(plOwner, LabelControl).HelpText = GetResourceString("AgentHelp")

            cmdChange.Text = GetResourceString("cmdChangeOwner")

            valPropertyTypeSubmission.ErrorMessage = GetResourceString("valPropertyTypeSubmission.ErrorMessage")

        End Sub

        Private Sub PopulateOwnerList()

            If (IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove)) Then

                Dim objUsers As ArrayList = UserController.GetUsers(PortalId)
                For Each objUser As UserInfo In objUsers
                    drpOwner.Items.Add(New ListItem(objUser.DisplayName & " (" & objUser.Username & ")", objUser.UserID.ToString()))
                Next

                'drpOwner.DataSource = UserController.GetUsers(PortalId)
                'drpOwner.DataBind()

                'Dim objSuperUser As DotNetNuke.Entities.Users.UserInfo
                'For Each objSuperUser In UserController.GetUsers(Null.NullInteger)
                '    drpOwner.Items.Insert(0, New System.Web.UI.WebControls.ListItem(objSuperUser.DisplayName & " (" & objSuperUser.Username & ")", objSuperUser.UserID.ToString))
                'Next

                drpOwner.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("None_Specified"), "-1"))
            Else

                Dim objAgentController As New AgentController(PortalSettings, PropertySettings, PortalId)

                Dim objUsers As ArrayList = objAgentController.ListSelected(PortalId, ModuleId, UserId)
                For Each objUser As UserInfo In objUsers
                    drpOwner.Items.Add(New ListItem(objUser.DisplayName & " (" & objUser.Username & ")", objUser.UserID.ToString()))
                Next

                'drpOwner.DataSource = objAgentController.ListSelected(PortalId, ModuleId, UserId)
                'drpOwner.DataBind()

                drpOwner.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Me.UserInfo.DisplayName & " (" & Me.UserInfo.Username & ")", Me.UserId))
                drpOwner.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("None_Specified"), "-1"))

                If Not (drpOwner.Items.FindByValue(Me.UserId.ToString()) Is Nothing) Then
                    drpOwner.SelectedValue = Me.UserId.ToString()
                End If
            End If

        End Sub

        Private Sub SetVisibility()

            cmdDelete.Visible = (_propertyID <> Null.NullInteger)

            If (cmdDelete.Visible) Then
                cmdDelete.Visible = (IsEditor Or PortalSecurity.IsInRoles(PropertySettings.PermissionDelete))
            End If

        End Sub

        Private Sub Update()
            Dim hostSettings As Dictionary(Of String, String) = DotNetNuke.Entities.Controllers.HostController.Instance.GetSettingsDictionary()
            If (_propertyID = Null.NullInteger) Then
                _property = New PropertyInfo
                _property.ModuleID = Me.ModuleId
            End If

            _property.DateModified = DateTime.Now
            _property.PropertyTypeID = Convert.ToInt32(drpTypes.SelectedValue)

            Dim dateCreated As DateTime = DateTime.Parse(txtCreationDate.Text)
            dateCreated = dateCreated.AddHours(Convert.ToInt32(drpCreationTimeHour.SelectedValue))
            dateCreated = dateCreated.AddMinutes(Convert.ToInt32(drpCreationTimeMinute.SelectedValue))
            _property.DateCreated = dateCreated

            If (txtPublishDate.Text.Length > 0) Then
                Dim datePublished As DateTime = DateTime.Parse(txtPublishDate.Text)
                If (drpPublishTimeHour.SelectedValue <> "-") Then
                    datePublished = datePublished.AddHours(Convert.ToInt32(drpPublishTimeHour.SelectedValue))
                End If
                If (drpPublishTimeMinute.SelectedValue <> "-") Then
                    datePublished = datePublished.AddMinutes(Convert.ToInt32(drpPublishTimeMinute.SelectedValue))
                End If
                _property.DatePublished = datePublished
            Else
                _property.DatePublished = Null.NullDate
            End If

            If (txtExpiryDate.Text.Length > 0) Then
                Dim dateExpiry As DateTime = DateTime.Parse(txtExpiryDate.Text)
                If (drpExpiryTimeHour.SelectedValue <> "-") Then
                    dateExpiry = dateExpiry.AddHours(Convert.ToInt32(drpExpiryTimeHour.SelectedValue))
                End If
                If (drpExpiryTimeMinute.SelectedValue <> "-") Then
                    dateExpiry = dateExpiry.AddMinutes(Convert.ToInt32(drpExpiryTimeMinute.SelectedValue))
                End If
                _property.DateExpired = dateExpiry
            Else
                _property.DateExpired = Null.NullDate
            End If

            Dim objStatusType As StatusType = StatusType.Draft
            If (chkPublished.Checked = True And chkApproved.Checked = False) Then
                objStatusType = StatusType.AwaitingApproval
            End If
            If (chkPublished.Checked And chkApproved.Checked) Then
                objStatusType = StatusType.Published
            End If

            _property.Status = objStatusType
            _property.IsFeatured = chkFeatured.Checked
            _property.OnlyForAuthenticated = chkOnlyForAuthenticated.Checked
            _property.ModifiedID = Me.UserId

            If (txtLatitude.Text <> "") Then
                If (IsNumeric(txtLatitude.Text)) Then
                    _property.Latitude = Double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture.NumberFormat)
                Else
                    If (IsNumeric(txtLatitude.Text.Replace("."c, ","c))) Then
                        _property.Latitude = txtLatitude.Text.Replace("."c, ","c)
                    Else
                        _property.Latitude = Null.NullDouble
                    End If
                End If
            Else
                _property.Latitude = Null.NullDouble
            End If

            If (txtLongitude.Text <> "") Then
                If (IsNumeric(txtLongitude.Text)) Then
                    _property.Longitude = Double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture.NumberFormat)
                Else
                    If (IsNumeric(txtLongitude.Text.Replace("."c, ","c))) Then
                        _property.Longitude = txtLongitude.Text.Replace("."c, ","c)
                    Else
                        _property.Longitude = Null.NullDouble
                    End If
                End If
            Else
                _property.Longitude = Null.NullDouble
            End If

            Dim addJournal As Boolean = False
            Dim objPropertyController As New PropertyController

            If (_propertyID <> Null.NullInteger) Then
                If drpOwner.Visible Then
                    If drpOwner.SelectedValue <> "" Then
                        _property.AuthorID = Convert.ToInt32(drpOwner.SelectedValue)
                    Else
                        _property.AuthorID = Null.NullInteger
                    End If
                Else
                    ' User never clicked "change", leave authorid as is
                End If
                objPropertyController.Update(_property)
            Else
                _property.AuthorID = Me.UserId
                If drpOwner.Visible Then
                    If drpOwner.SelectedValue <> "" Then
                        _property.AuthorID = Convert.ToInt32(drpOwner.SelectedValue)
                    Else
                        _property.AuthorID = Null.NullInteger
                    End If
                Else
                    ' User never clicked "change", leave authorid as is
                End If
                _propertyID = objPropertyController.Add(_property)
                _property = objPropertyController.Get(_propertyID)

                addJournal = True

                If (PropertySettings.ImagesEnabled AndAlso ((IsEditable = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionAddImages) = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = True))) Then
                    If (PropertySettings.UploadPlacement = UploadPlacementType.InlineTop) Then
                        Dim objPhotoController As New PhotoController
                        Dim objPhotos As ArrayList = objPhotoController.List(Null.NullInteger, CType(phTop.Controls(1), Controls.EditPropertyPhotos).PropertyGuid)

                        For Each objPhoto As PhotoInfo In objPhotos
                            objPhoto.PropertyID = _property.PropertyID
                            objPhoto.PropertyGuid = Null.NullString
                            objPhotoController.Update(objPhoto)
                        Next
                    End If
                    If (PropertySettings.UploadPlacement = UploadPlacementType.InlineBottom) Then
                        Dim objPhotoController As New PhotoController
                        Dim objPhotos As ArrayList = objPhotoController.List(Null.NullInteger, CType(phBottom.Controls(1), Controls.EditPropertyPhotos).PropertyGuid)

                        For Each objPhoto As PhotoInfo In objPhotos
                            objPhoto.PropertyID = _property.PropertyID
                            objPhoto.PropertyGuid = Null.NullString
                            objPhotoController.Update(objPhoto)
                        Next
                    End If
                End If

            End If

            If (_property.Status = StatusType.AwaitingApproval) Then
                If Not (IsEditable Or PortalSecurity.IsInRoles(PropertySettings.PermissionApprove)) Then
                    ' Send Approval Email
                    Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabId, ModuleId, ModuleKey)
                    ' Get the layout for subject and body
                    Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Submission_Subject_Html)
                    Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Submission_Body_Html)

                    ' Get the processed layout for subject and body
                    Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, _property, CustomFields, Nothing, False)
                    Dim subject As String = RenderControlAsString(phProperty)
                    phProperty = New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, _property, CustomFields, Nothing, False)
                    Dim body As String = RenderControlAsString(phProperty)
                    phProperty = Nothing

                    If (Me.PropertySettings.NotificationEmail <> "") Then
                        Try
                            DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, Me.PropertySettings.NotificationEmail, "", "",
                               DotNetNuke.Services.Mail.MailPriority.Normal,
                               subject,
                               DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                               "", hostSettings("SMTPServer"), hostSettings("SMTPAuthentication"), hostSettings("SMTPUsername"), hostSettings("SMTPPassword"))
                        Catch
                        End Try
                    End If

                    If (Me.PropertySettings.NotificationNotifyApprovers) Then

                        Dim emails As New Hashtable
                        Dim objAgentController As New AgentController(Me.PortalSettings, Me.PropertySettings, Me.PortalId)
                        Dim objApprovers As ArrayList = objAgentController.ListApprovers()

                        For Each objApprover As UserInfo In objApprovers
                            If (emails.ContainsKey(objApprover.Email) = False) Then
                                emails.Add(objApprover.Email, objApprover.Email)
                            End If
                        Next

                        For Each item As DictionaryEntry In emails
                            Try
                                DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, item.Value.ToString(), "", "",
                                  DotNetNuke.Services.Mail.MailPriority.Normal,
                                  subject,
                                  DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                                  "", hostSettings("SMTPServer"), hostSettings("SMTPAuthentication"), hostSettings("SMTPUsername"), hostSettings("SMTPPassword"))
                            Catch
                            End Try
                        Next

                    End If

                End If
            End If

            If (Me.PropertySettings.NotificationNotifyOwner) Then
                If (Request.IsAuthenticated AndAlso Me.UserId <> _property.AuthorID) Then

                    Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabId, ModuleId, ModuleKey)

                    Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactOwner_Subject_Html)
                    Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactOwner_Body_Html)

                    Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, _property, CustomFields, Nothing, False)
                    Dim subject As String = RenderControlAsString(phProperty)
                    phProperty = New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, _property, CustomFields, Nothing, False)
                    Dim body As String = RenderControlAsString(phProperty)
                    phProperty = Nothing

                    If (_property.Email <> "") Then
                        Try
                            DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, _property.Email, "", "",
                        DotNetNuke.Services.Mail.MailPriority.Normal,
                        subject,
                        DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                        "", hostSettings("SMTPServer"), hostSettings("SMTPAuthentication"), hostSettings("SMTPUsername"), hostSettings("SMTPPassword"))

                        Catch
                        End Try
                    End If
                End If
            End If

            If (Me.PropertySettings.NotificationNotifyBroker) Then
                If (Request.IsAuthenticated AndAlso Me.UserId <> _property.BrokerID) Then

                    Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabId, ModuleId, ModuleKey)

                    Dim objLayoutSubject As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactBroker_Subject_Html)
                    Dim objLayoutBody As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactBroker_Body_Html)

                    Dim phProperty As New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutSubject.Tokens, _property, CustomFields, Nothing, False)
                    Dim subject As String = RenderControlAsString(phProperty)
                    phProperty = New System.Web.UI.WebControls.PlaceHolder

                    objLayoutController.ProcessItem(phProperty.Controls, objLayoutBody.Tokens, _property, CustomFields, Nothing, False)
                    Dim body As String = RenderControlAsString(phProperty)
                    phProperty = Nothing

                    If (_property.BrokerEmail <> "") Then
                        Try
                            DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, _property.BrokerEmail, "", "",
                       DotNetNuke.Services.Mail.MailPriority.Normal,
                       subject,
                       DotNetNuke.Services.Mail.MailFormat.Text, System.Text.Encoding.UTF8, body,
                       "", hostSettings("SMTPServer"), hostSettings("SMTPAuthentication"), hostSettings("SMTPUsername"), hostSettings("SMTPPassword"))
                        Catch
                        End Try
                    End If
                End If
            End If

            PropertyTypeController.RemoveCache(Me.ModuleId)

            Dim fieldsToUpdate As New Hashtable

            Dim objCustomFields As List(Of CustomFieldInfo) = Me.CustomFields

            For Each item As RepeaterItem In rptDetails.Items
                Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)

                If Not (phValue Is Nothing) Then
                    If (phValue.Controls.Count > 0) Then

                        Dim objControl As System.Web.UI.Control = phValue.Controls(0)
                        Dim customFieldID As Integer = Convert.ToInt32(objControl.ID.Split("_")(0))

                        For Each objCustomField As CustomFieldInfo In objCustomFields
                            If (objCustomField.CustomFieldID = customFieldID) Then
                                Select Case objCustomField.FieldType

                                    Case CustomFieldType.OneLineTextBox
                                        Dim objTextBox As TextBox = CType(objControl, TextBox)
                                        If objTextBox.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)
                                            If (PropertySettings.CustomFieldExpiration <> Null.NullInteger) Then
                                                If (objCustomField.ValidationType = CustomFieldValidationType.Date) Then
                                                    If (objCustomField.CustomFieldID = PropertySettings.CustomFieldExpiration) Then
                                                        If (objTextBox.Text <> "") Then
                                                            Try

                                                                Dim expirationDate As DateTime = Convert.ToDateTime(objTextBox.Text)

                                                                If (PropertySettings.DefaultExpiration <> Null.NullInteger And PropertySettings.DefaultExpirationPeriod <> "") Then

                                                                    Select Case PropertySettings.DefaultExpirationPeriod
                                                                        Case "D"
                                                                            expirationDate = expirationDate.AddDays(Convert.ToInt32(PropertySettings.DefaultExpiration))
                                                                            Exit Select
                                                                        Case "M"
                                                                            expirationDate = expirationDate.AddMonths(Convert.ToInt32(PropertySettings.DefaultExpiration))
                                                                            Exit Select
                                                                        Case "Y"
                                                                            expirationDate = expirationDate.AddYears(Convert.ToInt32(PropertySettings.DefaultExpiration))
                                                                            Exit Select
                                                                    End Select

                                                                End If

                                                                _property.DateExpired = expirationDate
                                                                objPropertyController.Update(_property)
                                                            Catch
                                                            End Try
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If


                                    Case CustomFieldType.MultiLineTextBox
                                        Dim objTextBox As TextBox = CType(objControl, TextBox)
                                        If objTextBox.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)
                                        End If

                                    Case CustomFieldType.RichTextBox
                                        Try
                                            'If is a TextEditor, is not read-only
                                            Dim objTextBox As TextEditor = CType(objControl, TextEditor)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)
                                        Catch
                                            'When LockDown, it's a Label instead of a TextEditor
                                            Dim objTextBox As Label = CType(objControl, Label)
                                            If objTextBox.Enabled Then
                                                'Only if could be modified - not read-only (lockdown)
                                                fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)
                                            End If
                                        End Try

                                    Case CustomFieldType.DropDownList
                                        Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                        If objDropDownList.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            If (objDropDownList.SelectedValue = "-1") Then
                                                fieldsToUpdate.Add(customFieldID.ToString(), "")
                                            Else
                                                fieldsToUpdate.Add(customFieldID.ToString(), objDropDownList.SelectedValue)
                                            End If
                                        End If

                                    Case CustomFieldType.CheckBox
                                        Dim objCheckBox As CheckBox = CType(objControl, CheckBox)
                                        If objCheckBox.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objCheckBox.Checked.ToString())
                                        End If

                                    Case CustomFieldType.MultiCheckBox
                                        Dim objCheckBoxList As CheckBoxList = CType(objControl, CheckBoxList)
                                        If objCheckBoxList.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            Dim values As String = ""
                                            For Each objCheckBox As ListItem In objCheckBoxList.Items
                                                If (objCheckBox.Selected) Then
                                                    If (values = "") Then
                                                        values = objCheckBox.Value
                                                    Else
                                                        values = values & "|" & objCheckBox.Value
                                                    End If
                                                End If
                                            Next
                                            fieldsToUpdate.Add(customFieldID.ToString(), values)
                                        End If

                                    Case CustomFieldType.RadioButton
                                        Dim objRadioButtonList As RadioButtonList = CType(objControl, RadioButtonList)
                                        If objRadioButtonList.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objRadioButtonList.SelectedValue)
                                        End If

                                    Case CustomFieldType.FileUpload
                                        Dim objFileUpload As HtmlInputFile = CType(objControl, HtmlInputFile)
                                        If Not (objFileUpload.PostedFile Is Nothing) Then

                                            ' Delete old one
                                            Dim objPropertyValueController As New PropertyValueController
                                            Dim objPropertyValue As PropertyValueInfo = objPropertyValueController.GetByCustomField(_propertyID, Convert.ToInt32(customFieldID.ToString()), Me.ModuleId)

                                            If Not (objPropertyValue Is Nothing) Then
                                                Dim fileToDelete = PortalSettings.HomeDirectoryMapPath & objPropertyValue.CustomValue
                                                If (File.Exists(fileToDelete)) Then
                                                    File.Delete(fileToDelete)
                                                End If
                                                objPropertyValueController.Delete(_propertyID, objPropertyValue.PropertyValueID)
                                            End If

                                            If (objFileUpload.PostedFile.ContentLength > 0) Then
                                                ' Upload new one
                                                Dim filePath As String = PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & ModuleId.ToString() & "\Files\" & _propertyID.ToString() & "\"
                                                Dim fileName As String = Path.GetFileName(objFileUpload.PostedFile.FileName)
                                                Dim relativePath As String = "PropertyAgent\" & ModuleId.ToString() & "\Files\" & _propertyID.ToString() & "\" & fileName
                                                If (Directory.Exists(filePath) = False) Then
                                                    Directory.CreateDirectory(filePath)
                                                End If
                                                objFileUpload.PostedFile.SaveAs(filePath & fileName)
                                                fieldsToUpdate.Add(customFieldID.ToString(), relativePath)
                                            End If

                                        End If

                                    Case CustomFieldType.Hyperlink
                                        Dim objTextBox As TextBox = CType(objControl, TextBox)
                                        If objTextBox.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            fieldsToUpdate.Add(customFieldID.ToString(), objTextBox.Text)
                                        End If

                                    Case CustomFieldType.ListBox
                                        Dim objListBox As ListBox = CType(objControl, ListBox)
                                        If objListBox.Enabled Then
                                            'Only if could be modified - not read-only (lockdown)
                                            Dim values As String = ""
                                            For Each objItem As ListItem In objListBox.Items
                                                If (objItem.Selected) Then
                                                    If (values = "") Then
                                                        values = objItem.Value
                                                    Else
                                                        values = values & "|" & objItem.Value
                                                    End If
                                                End If
                                            Next
                                            fieldsToUpdate.Add(customFieldID.ToString(), values)
                                        End If

                                End Select

                                Exit For
                            End If
                        Next

                    End If
                End If
            Next

            Dim objSecurity As New PortalSecurity

            For Each key As String In fieldsToUpdate.Keys
                Dim val As String = fieldsToUpdate(key).ToString()

                Dim objPropertyValueController As New PropertyValueController
                Dim objPropertyValue As PropertyValueInfo = objPropertyValueController.GetByCustomField(_propertyID, Convert.ToInt32(key), Me.ModuleId)

                If (objPropertyValue Is Nothing) Then
                    objPropertyValue = New PropertyValueInfo
                    objPropertyValue.CustomFieldID = Convert.ToInt32(key)
                    If (PropertySettings.ProtectXSS) Then
                        objPropertyValue.CustomValue = objSecurity.InputFilter(val, PortalSecurity.FilterFlag.NoScripting)
                    Else
                        objPropertyValue.CustomValue = val
                    End If
                    objPropertyValue.PropertyID = _propertyID
                    objPropertyValueController.Add(objPropertyValue)
                Else
                    objPropertyValue.CustomFieldID = Convert.ToInt32(key)
                    If (PropertySettings.ProtectXSS) Then
                        objPropertyValue.CustomValue = objSecurity.InputFilter(val, PortalSecurity.FilterFlag.NoScripting)
                    Else
                        objPropertyValue.CustomValue = val
                    End If
                    objPropertyValue.PropertyID = _propertyID
                    objPropertyValueController.Update(objPropertyValue)
                End If
            Next

            If (addJournal) Then
                Dim summary As String = GetResourceString("JournalAddProperty")

                Dim objLayoutController As New LayoutController(PortalSettings, PropertySettings, Page, Nothing, False, TabId, ModuleId, ModuleKey)
                summary = summary.Replace("[LINK]", objLayoutController.GetExternalLink(objLayoutController.GetPropertyLink(_property, objCustomFields)))
                summary = summary.Replace("[PROPERTYLABEL]", PropertySettings.PropertyLabel)
                Journal.AddPropertyToJournal(_property, PortalId, TabId, UserId, Null.NullInteger, objLayoutController.GetExternalLink(objLayoutController.GetPropertyLink(_property, objCustomFields)), summary)
            End If


        End Sub
#End Region

#Region " Protected Methods "

        Protected Function GetMapUrl() As String

            Return "https://maps.googleapis.com/maps/api/js?key=" & PropertySettings.MapKey & "&callback=load"
            'Return "https://maps.googleapis.com/maps/api/js?key=" & PropertySettings.MapKey & "&callback=initMap"

        End Function

#End Region

#Region " Public Methods "

        Public Sub RefreshPhotos()

            If (PropertySettings.UploadPlacement = UploadPlacementType.InlineTop) Then
                CType(phTop.Controls(1), Controls.EditPropertyPhotos).BindPhotos()
            End If

            If (PropertySettings.UploadPlacement = UploadPlacementType.InlineBottom) Then
                CType(phBottom.Controls(1), Controls.EditPropertyPhotos).BindPhotos()
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                Dim doAjax As Boolean = True
                For Each objCustomField As CustomFieldInfo In CustomFields
                    If (objCustomField.FieldType = CustomFieldType.FileUpload) Then
                        doAjax = False
                        Exit For
                    End If
                Next

                'If (AJAX.IsInstalled And doAjax) Then
                '    AJAX.RegisterScriptManager()
                '    AJAX.WrapUpdatePanelControl(pnlUpdate, True)
                'End If

                ReadQueryString()
                BindPropertyTypes()
                BindDetails()
                SetLockDown()

                If (PropertySettings.ImagesEnabled AndAlso ((IsEditable = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionAddImages) = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = True))) Then
                    If (PropertySettings.UploadPlacement = UploadPlacementType.InlineTop) Then
                        Select Case PropertySettings.UploadMode
                                'Case 0
                                'Standard
                                'NOT SUPPORTED IN INLINE MODE
                            Case 0
                                'Standard
                                phTop.Controls.Add(Me.LoadControl("Controls/UploadPhotoStandard.ascx"))

                            Case 1
                                'Flash
                                phTop.Controls.Add(Me.LoadControl("Controls/UploadPhotoSWF.ascx"))

                            Case 2
                                'HTML5
                                phTop.Controls.Add(Me.LoadControl("Controls/UploadPhotoHTML5.ascx"))

                        End Select
                        phTop.Controls.Add(Me.LoadControl("Controls/EditPropertyPhotos.ascx"))
                    ElseIf (PropertySettings.UploadPlacement = UploadPlacementType.InlineBottom) Then
                        Select Case PropertySettings.UploadMode
                            Case 0
                                'Standard
                                phBottom.Controls.Add(Me.LoadControl("Controls/UploadPhotoStandard.ascx"))

                            Case 1
                                'Flash
                                phBottom.Controls.Add(Me.LoadControl("Controls/UploadPhotoSWF.ascx"))

                            Case 2
                                'HTML5
                                phBottom.Controls.Add(Me.LoadControl("Controls/UploadPhotoHTML5.ascx"))

                        End Select
                        phBottom.Controls.Add(Me.LoadControl("Controls/EditPropertyPhotos.ascx"))
                    End If
                End If

                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & GetResourceString("Confirmation", LocalResourceFile, Me.PropertySettings) & "');")
                cmdClone.Attributes.Add("onClick", "javascript:return confirm('" & GetResourceString("ConfirmationClone", LocalResourceFile, Me.PropertySettings) & "');")

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                trType.Visible = Not PropertySettings.HideTypes
                CheckSecurity()

                If (PropertySettings.ImagesEnabled AndAlso ((IsEditable = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionAddImages) = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = True))) Then
                    If (PropertySettings.UploadPlacement = UploadPlacementType.InlineTop) Then
                        If (ViewState("PropertyAgentGuid") Is Nothing) Then
                            ViewState.Add("PropertyAgentGuid", Guid.NewGuid.ToString())
                        End If
                        If (phTop.Controls.Count = 2) Then
                            Select Case PropertySettings.UploadMode
                                Case 0
                                    CType(phTop.Controls(1), Controls.EditPropertyPhotos).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                Case 1
                                    CType(phTop.Controls(0), Controls.UploadPhotoSWF).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                    CType(phTop.Controls(1), Controls.EditPropertyPhotos).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                Case 2
                                    CType(phTop.Controls(0), Controls.UploadPhotoHTML5).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                    CType(phTop.Controls(1), Controls.EditPropertyPhotos).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                            End Select
                        End If
                    End If

                    If (PropertySettings.UploadPlacement = UploadPlacementType.InlineBottom) Then
                        If (ViewState("PropertyAgentGuid") Is Nothing) Then
                            ViewState.Add("PropertyAgentGuid", Guid.NewGuid.ToString())
                        End If
                        If (phBottom.Controls.Count = 2) Then
                            Select Case PropertySettings.UploadMode
                                Case 0
                                    CType(phBottom.Controls(1), Controls.EditPropertyPhotos).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                Case 1
                                    CType(phBottom.Controls(0), Controls.UploadPhotoSWF).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                    CType(phBottom.Controls(1), Controls.EditPropertyPhotos).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                Case 2
                                    CType(phBottom.Controls(0), Controls.UploadPhotoHTML5).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                                    CType(phBottom.Controls(1), Controls.EditPropertyPhotos).PropertyGuid = ViewState("PropertyAgentGuid").ToString()
                            End Select
                        End If
                    End If
                End If

                cmdCreationDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtCreationDate)
                cmdPublishDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtPublishDate)
                cmdExpiryDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtExpiryDate)

                If (_propertyID <> Null.NullInteger) Then
                    For Each key As String In _richTextValues
                        For Each item As RepeaterItem In rptDetails.Items
                            If Not (item.FindControl(key) Is Nothing) Then
                                Dim objTextBox As TextEditor = CType(item.FindControl(key), TextEditor)
                                objTextBox.Text = _richTextValues(key)
                                Exit For
                            End If
                        Next
                    Next
                End If

                If (Page.IsPostBack = False) Then

                    If (IsEditable = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionFeature) = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = True) Then
                        trFeatured.Visible = True
                        chkFeatured.Enabled = True
                    Else
                        trFeatured.Visible = False
                        chkFeatured.Enabled = False
                    End If

                    If (IsEditable = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionAddImages) = True OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = True) Then
                        cmdUpdateEditPhotos.Visible = (PropertySettings.ImagesEnabled AndAlso PropertySettings.UploadPlacement = UploadPlacementType.SeparatePage)
                    Else
                        cmdUpdateEditPhotos.Visible = False
                    End If

                    phLocation.Visible = Me.PropertySettings.MapEnable

                End If

                cmdCancel.CssClass = PropertySettings.ButtonClass
                cmdDelete.CssClass = PropertySettings.ButtonClass
                cmdClone.CssClass = PropertySettings.ButtonClass
                cmdUpdate.CssClass = PropertySettings.ButtonClass
                cmdUpdateEditPhotos.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                LocalizeLabels()
                SetVisibility()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub rptDetails_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDetails.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objCustomField As CustomFieldInfo = CType(e.Item.DataItem, CustomFieldInfo)

                Dim showItem As Boolean = True
                If (objCustomField.InheritSecurity = False) Then
                    If (Settings.Contains(Constants.PERMISSION_CUSTOM_FIELD_SETTING & objCustomField.CustomFieldID.ToString())) Then
                        Dim editRoles As String = Settings(Constants.PERMISSION_CUSTOM_FIELD_SETTING & objCustomField.CustomFieldID.ToString()).ToString()
                        If (editRoles = "") Then
                            showItem = False
                        Else
                            If (PortalSecurity.IsInRoles(editRoles) = False) Then
                                showItem = False
                            End If
                        End If
                    Else
                        showItem = False
                    End If
                End If

                If (showItem) Then

                    Dim phValue As PlaceHolder = CType(e.Item.FindControl("phValue"), PlaceHolder)
                    Dim phLabel As PlaceHolder = CType(e.Item.FindControl("phLabel"), PlaceHolder)

                    Dim cmdHelp As LinkButton = CType(e.Item.FindControl("cmdHelp"), LinkButton)
                    Dim pnlHelp As Panel = CType(e.Item.FindControl("pnlHelp"), Panel)
                    Dim lblLabel As Label = CType(e.Item.FindControl("lblLabel"), Label)
                    Dim lblHelp As Label = CType(e.Item.FindControl("lblHelp"), Label)
                    Dim imgHelp As Image = CType(e.Item.FindControl("imgHelp"), Image)

                    Dim trItem As HtmlControls.HtmlTableRow = CType(e.Item.FindControl("trItem"), HtmlControls.HtmlTableRow)
                    Dim trLabel As HtmlControls.HtmlTableRow = CType(e.Item.FindControl("trLabel"), HtmlControls.HtmlTableRow)

                    Dim isLockdown As Boolean = (objCustomField.IsLockDown) AndAlso Not PortalSecurity.IsInRoles(PropertySettings.PermissionLockDown)

                    If Not (phValue Is Nothing) Then

                        DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelp, pnlHelp, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                        If (objCustomField.IsRequired) Then
                            lblLabel.Text = objCustomField.Caption & "*:"
                        Else
                            lblLabel.Text = objCustomField.Caption & ":"
                        End If
                        lblHelp.Text = objCustomField.CaptionHelp
                        imgHelp.AlternateText = objCustomField.CaptionHelp

                        Select Case (objCustomField.FieldType)

                            Case CustomFieldType.OneLineTextBox

                                Dim objTextBox As New TextBox
                                objTextBox.CssClass = "NormalTextBox"
                                objTextBox.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                If (objCustomField.Length <> Null.NullInteger AndAlso objCustomField.Length > 0) Then
                                    objTextBox.MaxLength = objCustomField.Length
                                End If
                                If (objCustomField.DefaultValue <> "") Then
                                    objTextBox.Text = FormatDefaultValue(objCustomField.DefaultValue)
                                End If
                                objTextBox.Enabled = Not isLockdown
                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objTextBox.Enabled = False)) Then
                                        objTextBox.Text = _property.PropertyList(objCustomField.CustomFieldID).ToString()
                                    End If
                                End If
                                If (objCustomField.ValidationType = CustomFieldValidationType.Date AndAlso Me.PropertySettings.FieldWidth > 100) Then
                                    objTextBox.Width = Unit.Pixel(Me.PropertySettings.FieldWidth - 100)
                                Else
                                    objTextBox.Width = Unit.Pixel(Me.PropertySettings.FieldWidth)
                                End If
                                phValue.Controls.Add(objTextBox)

                                If (objCustomField.IsRequired) Then
                                    Dim valRequired As New RequiredFieldValidator
                                    valRequired.ControlToValidate = objTextBox.ID
                                    valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                    valRequired.CssClass = "NormalRed"
                                    valRequired.Display = ValidatorDisplay.None
                                    valRequired.Enabled = Not isLockdown
                                    valRequired.SetFocusOnError = True
                                    phValue.Controls.Add(valRequired)
                                End If

                                If (objCustomField.ValidationType <> CustomFieldValidationType.None) Then
                                    Dim valCompare As New CompareValidator
                                    valCompare.ControlToValidate = objTextBox.ID
                                    valCompare.CssClass = "NormalRed"
                                    valCompare.Display = ValidatorDisplay.None
                                    valCompare.Enabled = Not isLockdown
                                    valCompare.SetFocusOnError = True
                                    Select Case objCustomField.ValidationType

                                        Case CustomFieldValidationType.Currency
                                            valCompare.Type = ValidationDataType.Double
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valFieldCurrency", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                            phValue.Controls.Add(valCompare)

                                        Case CustomFieldValidationType.Date
                                            valCompare.Type = ValidationDataType.Date
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valFieldDate", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                            phValue.Controls.Add(valCompare)

                                            Dim objCalendar As New HyperLink
                                            objCalendar.CssClass = "CommandButton"
                                            objCalendar.Text = Localization.GetString("Calendar", Me.LocalResourceFile)
                                            objCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(objTextBox)
                                            phValue.Controls.Add(objCalendar)

                                        Case CustomFieldValidationType.Double
                                            valCompare.Type = ValidationDataType.Double
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valFieldDecimal", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                            phValue.Controls.Add(valCompare)

                                        Case CustomFieldValidationType.Integer
                                            valCompare.Type = ValidationDataType.Integer
                                            valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                            valCompare.ErrorMessage = Localization.GetString("valFieldNumber", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                            phValue.Controls.Add(valCompare)

                                        Case CustomFieldValidationType.Email
                                            Dim valRegular As New RegularExpressionValidator
                                            valRegular.ControlToValidate = objTextBox.ID
                                            valRegular.ValidationExpression = "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            valRegular.ErrorMessage = Localization.GetString("valFieldEmail", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                            valRegular.CssClass = "NormalRed"
                                            valRegular.Display = ValidatorDisplay.None
                                            valRegular.Enabled = Not isLockdown
                                            phValue.Controls.Add(valRegular)

                                        Case CustomFieldValidationType.Regex
                                            If (objCustomField.RegularExpression <> "") Then
                                                Dim valRegular As New RegularExpressionValidator
                                                valRegular.ControlToValidate = objTextBox.ID
                                                valRegular.ValidationExpression = objCustomField.RegularExpression
                                                valRegular.ErrorMessage = Localization.GetString("valFieldRegex", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                                valRegular.CssClass = "NormalRed"
                                                valRegular.Display = ValidatorDisplay.None
                                                valRegular.Enabled = Not isLockdown
                                                phValue.Controls.Add(valRegular)
                                            End If

                                    End Select
                                End If

                            Case CustomFieldType.MultiLineTextBox

                                Dim objTextBox As New TextBox
                                objTextBox.TextMode = TextBoxMode.MultiLine
                                objTextBox.CssClass = "NormalTextBox"
                                objTextBox.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                objTextBox.Rows = 4
                                If (objCustomField.Length <> Null.NullInteger AndAlso objCustomField.Length > 0) Then
                                    objTextBox.MaxLength = objCustomField.Length
                                End If
                                If (objCustomField.DefaultValue <> "") Then
                                    objTextBox.Text = objCustomField.DefaultValue
                                End If
                                objTextBox.Enabled = Not isLockdown
                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objTextBox.Enabled = False)) Then
                                        objTextBox.Text = _property.PropertyList(objCustomField.CustomFieldID).ToString()
                                    End If
                                End If
                                objTextBox.Width = Unit.Pixel(Me.PropertySettings.FieldWidth)
                                phValue.Controls.Add(objTextBox)

                                If (objCustomField.IsRequired) Then
                                    Dim valRequired As New RequiredFieldValidator
                                    valRequired.ControlToValidate = objTextBox.ID
                                    valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                    valRequired.CssClass = "NormalRed"
                                    valRequired.Display = ValidatorDisplay.None
                                    valRequired.Enabled = Not isLockdown
                                    valRequired.SetFocusOnError = True
                                    phValue.Controls.Add(valRequired)
                                End If

                            Case CustomFieldType.RichTextBox

                                If isLockdown Then
                                    'If locked, uses a Label
                                    Dim objTextBox As New Label
                                    objTextBox.CssClass = "NormalTextBox"
                                    'objTextBox.BackColor = Drawing.Color.White 'this does not works...
                                    objTextBox.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                    objTextBox.Enabled = False
                                    If Not (_property Is Nothing) Then
                                        If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objTextBox.Enabled = False)) Then
                                            objTextBox.Text = HttpUtility.HtmlDecode(_property.PropertyList(objCustomField.CustomFieldID).ToString())
                                        End If
                                    End If
                                    phValue.Controls.Add(objTextBox)

                                Else
                                    'If not locked, uses the RichTextBox
                                    Dim objTextBox As TextEditor = CType(Me.LoadControl("~/controls/TextEditor.ascx"), TextEditor)
                                    objTextBox.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                    If (objCustomField.DefaultValue <> "") Then
                                        objTextBox.Text = objCustomField.DefaultValue
                                    End If
                                    If Not (_property Is Nothing) Then
                                        If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And Page.IsPostBack = False) Then
                                            ' There is a problem assigned values at init with the RTE, using ArrayList to assign later.
                                            _richTextValues.Add(objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name), _property.PropertyList(objCustomField.CustomFieldID).ToString())
                                        End If
                                    End If
                                    objTextBox.Width = Unit.Pixel(Me.PropertySettings.FieldWidth)
                                    objTextBox.Height = Unit.Pixel(400)

                                    phValue.Controls.Add(objTextBox)

                                    If (objCustomField.IsRequired) Then
                                        Dim valRequired As New RequiredFieldValidator
                                        valRequired.ControlToValidate = objTextBox.ID
                                        valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                        valRequired.CssClass = "NormalRed"
                                        valRequired.SetFocusOnError = True
                                        phValue.Controls.Add(valRequired)
                                    End If

                                End If 'IsLockDown

                            Case CustomFieldType.DropDownList

                                Dim objDropDownList As New DropDownList
                                objDropDownList.CssClass = "NormalTextBox"
                                objDropDownList.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)

                                If (objCustomField.FieldElementType = FieldElementType.Standard) Then
                                    Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                    For Each value As String In values
                                        If (value <> "") Then
                                            objDropDownList.Items.Add(value)
                                        End If
                                    Next

                                    For Each objCustomFieldDropDown As CustomFieldInfo In Me.CustomFields
                                        If (objCustomFieldDropDown.IsPublished AndAlso objCustomFieldDropDown.FieldType = CustomFieldType.DropDownList AndAlso objCustomFieldDropDown.FieldElementType = FieldElementType.LinkedToDropdown AndAlso objCustomFieldDropDown.FieldElementDropDown = objCustomField.CustomFieldID) Then
                                            objDropDownList.AutoPostBack = True
                                        End If
                                    Next
                                End If

                                If (objCustomField.FieldElementType = FieldElementType.LinkedToPropertyType) Then
                                    If (drpTypes.Items.Count > 1) Then
                                        If (drpTypes.SelectedIndex > 0) Then
                                            If ((drpTypes.SelectedIndex - 1) < objCustomField.FieldElements.Split(vbCrLf).Length) Then
                                                Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(drpTypes.SelectedIndex - 1).Split(Convert.ToChar("|"))
                                                For Each value As String In values
                                                    If (value.Trim() <> "") Then
                                                        objDropDownList.Items.Add(value.Trim())
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If

                                If (objCustomField.FieldElementType = FieldElementType.LinkedToDropdown) Then
                                    Dim found As Boolean = False
                                    If Not (_property Is Nothing) Then
                                        If (_property.PropertyList.Contains(objCustomField.FieldElementDropDown) And (Page.IsPostBack = False Or objDropDownList.Enabled = False)) Then
                                            For Each objCustomFieldLinked As CustomFieldInfo In CustomFields
                                                If (objCustomFieldLinked.CustomFieldID = objCustomField.FieldElementDropDown) Then
                                                    found = True
                                                    Dim i As Integer = 0
                                                    For Each val As String In objCustomFieldLinked.FieldElements.Split("|")
                                                        If (val.ToLower() = _property.PropertyList(objCustomField.FieldElementDropDown).ToString().ToLower()) Then
                                                            Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(i).Split(Convert.ToChar("|"))
                                                            For Each value As String In values
                                                                If (value.Trim() <> "") Then
                                                                    objDropDownList.Items.Add(value.Replace(vbLf, ""))
                                                                End If
                                                            Next
                                                        End If
                                                        i = i + 1
                                                    Next
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If

                                    Dim postbackID As String = Page.Request.Params.Get("__EVENTTARGET")
                                    If (postbackID <> "") Then
                                        Dim arrItems As String() = postbackID.Split("$")

                                        If (arrItems.Length > 0) Then
                                            If (IsNumeric(arrItems(arrItems.Length - 1).Split("_")(0))) Then
                                                Dim customFieldID As Integer = Convert.ToInt32(arrItems(arrItems.Length - 1).Split("_")(0))
                                                If (objCustomField.FieldElementDropDown = customFieldID) Then
                                                    found = True
                                                    If (Request(postbackID) <> "") Then

                                                        For Each objCustomFieldLinked As CustomFieldInfo In CustomFields
                                                            If (objCustomFieldLinked.CustomFieldID = customFieldID) Then
                                                                Dim i As Integer = 0
                                                                For Each val As String In objCustomFieldLinked.FieldElements.Split("|")
                                                                    If (val.ToLower() = Request(postbackID).ToLower()) Then
                                                                        Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(i).Split(Convert.ToChar("|"))
                                                                        For Each value As String In values
                                                                            If (value.Trim() <> "") Then
                                                                                objDropDownList.Items.Add(value.Replace(vbLf, ""))
                                                                            End If
                                                                        Next
                                                                    End If
                                                                    i = i + 1
                                                                Next
                                                                Exit For
                                                            End If
                                                        Next
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If

                                    If (found = False) Then
                                        For Each key As String In Request.Form.AllKeys
                                            Dim keySplit As String = key
                                            If (keySplit.Contains("_")) Then
                                                keySplit = keySplit.Split("_"c)(0)
                                            End If
                                            If (keySplit IsNot Nothing AndAlso keySplit.ToLower().EndsWith("$" & objCustomField.FieldElementDropDown.ToString().ToLower())) Then
                                                If (key.ToLower().Contains("rptdetails") And keySplit.ToLower().Contains("editproperty")) Then

                                                    If (Request(key) <> "") Then

                                                        For Each objCustomFieldLinked As CustomFieldInfo In CustomFields
                                                            If (objCustomFieldLinked.CustomFieldID = objCustomField.FieldElementDropDown) Then
                                                                Dim i As Integer = 0
                                                                For Each val As String In objCustomFieldLinked.FieldElements.Split("|")
                                                                    If (val.ToLower() = Request(key).ToLower()) Then
                                                                        Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(i).Split(Convert.ToChar("|"))
                                                                        For Each value As String In values
                                                                            If (value.Trim() <> "") Then
                                                                                objDropDownList.Items.Add(value.Replace(vbLf, ""))
                                                                            End If
                                                                        Next
                                                                    End If
                                                                    i = i + 1
                                                                Next
                                                                Exit For
                                                            End If
                                                        Next
                                                    End If

                                                End If
                                            End If
                                        Next
                                    End If

                                End If

                                If (objCustomField.FieldElementType = FieldElementType.SqlQuery) Then
                                    For Each value As String In objCustomField.FieldElementsSql
                                        If (value <> "") Then
                                            objDropDownList.Items.Add(value)
                                        End If
                                    Next
                                End If

                                Dim selectText As String = Localization.GetString("SelectValue", Me.LocalResourceFile)
                                selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                                objDropDownList.Items.Insert(0, New ListItem(selectText, "-1"))

                                If (objCustomField.DefaultValue <> "") Then
                                    If Not (objDropDownList.Items.FindByValue(objCustomField.DefaultValue) Is Nothing) Then
                                        objDropDownList.SelectedValue = objCustomField.DefaultValue
                                    End If
                                End If

                                objDropDownList.Width = Unit.Pixel(Me.PropertySettings.FieldWidth)
                                objDropDownList.Enabled = Not isLockdown

                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objDropDownList.Enabled = False)) Then
                                        If Not (objDropDownList.Items.FindByValue(_property.PropertyList(objCustomField.CustomFieldID).ToString()) Is Nothing) Then
                                            objDropDownList.SelectedValue = _property.PropertyList(objCustomField.CustomFieldID).ToString()
                                        End If
                                    End If
                                End If

                                phValue.Controls.Add(objDropDownList)

                                If (objCustomField.IsRequired) Then
                                    Dim valRequired As New RequiredFieldValidator
                                    valRequired.ControlToValidate = objDropDownList.ID
                                    valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                    valRequired.CssClass = "NormalRed"
                                    valRequired.Display = ValidatorDisplay.None
                                    valRequired.Enabled = Not isLockdown
                                    valRequired.SetFocusOnError = True
                                    valRequired.InitialValue = "-1"
                                    phValue.Controls.Add(valRequired)
                                End If

                            Case CustomFieldType.CheckBox

                                Dim objCheckBox As New CheckBox
                                objCheckBox.CssClass = "Normal"
                                objCheckBox.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                If (objCustomField.DefaultValue <> "") Then
                                    Try
                                        objCheckBox.Checked = Convert.ToBoolean(objCustomField.DefaultValue)
                                    Catch
                                    End Try
                                End If

                                objCheckBox.Enabled = Not isLockdown

                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objCheckBox.Enabled = False)) Then
                                        If (_property.PropertyList(objCustomField.CustomFieldID).ToString() <> "") Then
                                            Try
                                                objCheckBox.Checked = Convert.ToBoolean(_property.PropertyList(objCustomField.CustomFieldID).ToString())
                                            Catch
                                            End Try
                                        End If
                                    End If
                                End If
                                phValue.Controls.Add(objCheckBox)

                            Case CustomFieldType.MultiCheckBox

                                Dim objCheckBoxList As New CheckBoxList
                                objCheckBoxList.CssClass = "Normal"
                                objCheckBoxList.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                objCheckBoxList.RepeatColumns = Me.PropertySettings.CheckBoxItemsPerRow
                                objCheckBoxList.RepeatDirection = RepeatDirection.Horizontal
                                objCheckBoxList.RepeatLayout = RepeatLayout.Table

                                Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                For Each value As String In values

                                    Dim objListItem As New ListItem
                                    objListItem.Text = value
                                    objListItem.Value = value
                                    If (objCustomField.DefaultValue <> "") Then
                                        For Each v As String In objCustomField.DefaultValue.Split("|"c)
                                            If (v.Trim() = value.Trim()) Then
                                                objListItem.Selected = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    objCheckBoxList.Items.Add(objListItem)
                                Next

                                objCheckBoxList.Enabled = Not isLockdown

                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objCheckBoxList.Enabled = False)) Then
                                        Dim vals As String() = _property.PropertyList(objCustomField.CustomFieldID).ToString().Split(Convert.ToChar("|"))
                                        For Each val As String In vals
                                            For Each item As ListItem In objCheckBoxList.Items
                                                If (item.Value = val) Then
                                                    item.Selected = True
                                                End If
                                            Next
                                        Next
                                    End If
                                End If

                                phValue.Controls.Add(objCheckBoxList)

                            Case CustomFieldType.RadioButton

                                Dim objRadioButtonList As New RadioButtonList
                                objRadioButtonList.CssClass = "Normal"
                                objRadioButtonList.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                objRadioButtonList.RepeatDirection = RepeatDirection.Horizontal
                                objRadioButtonList.RepeatLayout = RepeatLayout.Table
                                objRadioButtonList.RepeatColumns = Me.PropertySettings.RadioButtonItemsPerRow

                                Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                For Each value As String In values
                                    objRadioButtonList.Items.Add(value)
                                Next

                                If (objCustomField.DefaultValue <> "") Then
                                    If Not (objRadioButtonList.Items.FindByValue(objCustomField.DefaultValue) Is Nothing) Then
                                        objRadioButtonList.SelectedValue = objCustomField.DefaultValue
                                    End If
                                End If

                                objRadioButtonList.Enabled = Not isLockdown

                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objRadioButtonList.Enabled = False)) Then
                                        If Not (objRadioButtonList.Items.FindByValue(_property.PropertyList(objCustomField.CustomFieldID).ToString()) Is Nothing) Then
                                            objRadioButtonList.SelectedValue = _property.PropertyList(objCustomField.CustomFieldID).ToString()
                                        End If
                                    End If
                                End If

                                phValue.Controls.Add(objRadioButtonList)

                                If (objCustomField.IsRequired) Then
                                    Dim valRequired As New RequiredFieldValidator
                                    valRequired.ControlToValidate = objRadioButtonList.ID
                                    valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                    valRequired.CssClass = "NormalRed"
                                    valRequired.Display = ValidatorDisplay.None
                                    valRequired.Enabled = Not isLockdown
                                    valRequired.SetFocusOnError = True
                                    phValue.Controls.Add(valRequired)
                                End If


                            Case CustomFieldType.FileUpload

                                Dim hasValue As Boolean = False
                                Dim objHtmlInputFile As New HtmlInputFile
                                objHtmlInputFile.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                objHtmlInputFile.Disabled = isLockdown
                                phValue.Controls.Add(objHtmlInputFile)

                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) AndAlso _property.PropertyList(objCustomField.CustomFieldID).ToString() <> "") Then

                                        Dim fileName As String = Path.GetFileName(PortalSettings.HomeDirectoryMapPath & _property.PropertyList(objCustomField.CustomFieldID).ToString())
                                        Dim objLabel As New Label
                                        objLabel.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name) & "-Label"
                                        objLabel.Text = fileName & "<BR>"
                                        objLabel.CssClass = "Normal"
                                        phValue.Controls.Add(objLabel)

                                        Dim objDelete As New LinkButton
                                        objDelete.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name) & "-Button"
                                        objDelete.Text = Localization.GetString("cmdDelete")
                                        objDelete.CssClass = "CommandButton"
                                        objDelete.CommandArgument = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                        objDelete.Visible = Not isLockdown
                                        AddHandler objDelete.Click, AddressOf FileUploadButton_Click
                                        phValue.Controls.Add(objDelete)

                                        phValue.Controls(0).Visible = False
                                        hasValue = True
                                    End If
                                End If


                                If (objCustomField.IsRequired) Then
                                    Dim valRequired As New RequiredFieldValidator
                                    valRequired.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name) & "-Required"
                                    valRequired.ControlToValidate = objHtmlInputFile.ID
                                    valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                    valRequired.CssClass = "NormalRed"
                                    valRequired.Display = ValidatorDisplay.None
                                    valRequired.Enabled = Not isLockdown
                                    valRequired.Visible = Not hasValue
                                    valRequired.SetFocusOnError = True
                                    phValue.Controls.Add(valRequired)
                                End If

                            Case CustomFieldType.Hyperlink

                                Dim objTextBox As New TextBox
                                objTextBox.CssClass = "NormalTextBox"
                                objTextBox.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                If (objCustomField.DefaultValue <> "") Then
                                    objTextBox.Text = objCustomField.DefaultValue
                                End If
                                objTextBox.Enabled = Not isLockdown
                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objTextBox.Enabled = False)) Then
                                        objTextBox.Text = _property.PropertyList(objCustomField.CustomFieldID).ToString()
                                    End If
                                End If
                                objTextBox.Width = Unit.Pixel(Me.PropertySettings.FieldWidth)
                                phValue.Controls.Add(objTextBox)

                                If (objCustomField.IsRequired) Then
                                    Dim valRequired As New RequiredFieldValidator
                                    valRequired.ControlToValidate = objTextBox.ID
                                    valRequired.ErrorMessage = Localization.GetString("valFieldRequired", Me.LocalResourceFile).Replace("[CUSTOMFIELD]", objCustomField.Name)
                                    valRequired.CssClass = "NormalRed"
                                    valRequired.Display = ValidatorDisplay.None
                                    valRequired.Enabled = Not isLockdown
                                    valRequired.SetFocusOnError = True
                                    phValue.Controls.Add(valRequired)
                                End If

                            Case CustomFieldType.ListBox

                                Dim objListBox As New ListBox
                                objListBox.CssClass = "Normal"
                                objListBox.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                objListBox.SelectionMode = ListSelectionMode.Multiple

                                Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                For Each value As String In values
                                    objListBox.Items.Add(value)
                                Next

                                objListBox.Width = Unit.Pixel(Me.PropertySettings.FieldWidth)
                                objListBox.Enabled = Not isLockdown
                                objListBox.Rows = 6

                                If Not (_property Is Nothing) Then
                                    If (_property.PropertyList.Contains(objCustomField.CustomFieldID) And (Page.IsPostBack = False Or objListBox.Enabled = False)) Then
                                        Dim vals As String() = _property.PropertyList(objCustomField.CustomFieldID).ToString().Split(Convert.ToChar("|"))
                                        For Each val As String In vals
                                            For Each item As ListItem In objListBox.Items
                                                If (item.Value = val) Then
                                                    item.Selected = True
                                                End If
                                            Next
                                        Next
                                    End If
                                End If

                                phValue.Controls.Add(objListBox)

                            Case CustomFieldType.Label

                                trItem.Visible = False
                                trLabel.Visible = True

                                Dim objLiteral As New Literal

                                objLiteral.ID = objCustomField.CustomFieldID.ToString() & "_" & StripNonAlphaNumericCharacters(objCustomField.Name)
                                objLiteral.Text = Page.Server.HtmlDecode(objCustomField.FieldElements)

                                phLabel.Controls.Add(objLiteral)

                        End Select

                    End If

                Else

                    e.Item.Visible = False

                End If

            End If

        End Sub

        Public Sub FileUploadButton_Click(ByVal sender As Object, ByVal e As EventArgs)

            Dim objDelete As LinkButton = CType(sender, LinkButton)

            For Each objControl As Control In rptDetails.Controls
                Dim phValue As PlaceHolder = CType(objControl.FindControl("phValue"), PlaceHolder)

                If Not (phValue Is Nothing) Then

                    Dim objHtmlInputFile As HtmlInputFile = CType(phValue.FindControl(objDelete.CommandArgument), HtmlInputFile)
                    If Not (objHtmlInputFile Is Nothing) Then
                        objHtmlInputFile.Visible = True
                    End If

                    Dim valRequired As RequiredFieldValidator = CType(phValue.FindControl(objDelete.CommandArgument & "-Required"), RequiredFieldValidator)
                    If Not (valRequired Is Nothing) Then
                        valRequired.Visible = True
                    End If

                    Dim objLabel As Label = CType(phValue.FindControl(objDelete.CommandArgument & "-Label"), Label)
                    If Not (objLabel Is Nothing) Then
                        objLabel.Visible = False
                    End If

                    Dim objDeleteLink As LinkButton = CType(phValue.FindControl(objDelete.CommandArgument & "-Button"), LinkButton)
                    If Not (objDeleteLink Is Nothing) Then
                        objDeleteLink.Visible = False
                    End If
                End If
            Next

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Update()

                    If (_returnUrl <> "") Then
                        Response.Redirect(_returnUrl, True)
                    Else
                        Select Case PropertySettings.RedirectType

                            Case RedirectType.PropertyManager
                                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager"), True)
                                Exit Select

                            Case RedirectType.ViewProperty
                                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=View", "PropertyID=" & _propertyID.ToString()), True)
                                Exit Select

                            Case RedirectType.Page
                                If (Me.PropertySettings.RedirectPage <> Null.NullInteger) Then
                                    Response.Redirect(NavigateURL(Me.PropertySettings.RedirectPage), True)
                                Else
                                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager"), True)
                                End If
                                Exit Select

                        End Select
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdateEditPhotos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdateEditPhotos.Click

            Try

                If (Page.IsValid) Then

                    Update()

                    If (_returnUrl <> "") Then
                        Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPhotos", PropertySettings.SEOPropertyID & "=" & _propertyID.ToString(), "ReturnUrl=" & _returnUrl), True)
                    Else
                        Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPhotos", PropertySettings.SEOPropertyID & "=" & _propertyID.ToString()), True)
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager"), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdClone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClone.Click

            Try

                If (Page.IsValid) Then

                    Update()

                    Dim objPropertyController As New PropertyController()
                    Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)

                    If (objProperty IsNot Nothing) Then

                        objProperty.PropertyID = objPropertyController.Add(objProperty)

                        Dim objPropertyValueController As New PropertyValueController()
                        Dim objPropertyValues As List(Of PropertyValueInfo) = objPropertyValueController.List(_propertyID, ModuleId)

                        For Each objPropertyValue As PropertyValueInfo In objPropertyValues
                            objPropertyValue.PropertyID = objProperty.PropertyID
                            objPropertyValueController.Add(objPropertyValue)
                        Next

                        Dim objPhotoController As New PhotoController()
                        Dim objPhotos As ArrayList = objPhotoController.List(_propertyID)

                        For Each objPhoto As PhotoInfo In objPhotos
                            objPhoto.PropertyID = objProperty.PropertyID
                            If (objPhoto.PhotoType = PhotoType.Internal) Then
                                ' Copy Photo
                                Dim filePath As String = PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & ModuleId.ToString() & "\Images\" & objPhoto.Filename

                                If (File.Exists(filePath)) Then
                                    File.Copy(filePath, PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & ModuleId.ToString() & "\Images\" & objProperty.PropertyID.ToString() & "_" & objPhoto.Filename)
                                End If

                                objPhoto.Filename = objProperty.PropertyID.ToString() & "_" & objPhoto.Filename
                            End If
                            objPhotoController.Add(objPhoto)
                        Next

                        Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=View", "PropertyID=" & objProperty.PropertyID.ToString()), True)

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChange.Click

            Try

                lblOwner.Visible = False
                cmdChange.Visible = False
                drpOwner.Visible = True

                PopulateOwnerList()

                Dim objPropertyController As New PropertyController
                Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)

                Try
                    If objProperty Is Nothing Then
                        drpOwner.SelectedValue = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID.ToString
                    Else
                        drpOwner.SelectedValue = objProperty.AuthorID.ToString()
                    End If
                Catch exc As Exception
                    ' suppress error selecting owner user
                End Try

            Catch exc As Exception
                ' suppress error if the user no longer exists
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objPhotoController As New PhotoController
                Dim objPhotos As ArrayList = objPhotoController.List(_propertyID)

                For Each objPhoto As PhotoInfo In objPhotos
                    If (File.Exists(Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent/" & Me.ModuleId & "/Images/" & objPhoto.Filename)) Then
                        File.Delete(Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent/" & Me.ModuleId & "/Images/" & objPhoto.Filename)
                    End If
                Next

                objPhotoController.DeleteByPropertyID(_propertyID)

                PropertyTypeController.RemoveCache(Me.ModuleId)

                Dim objPropertyController As New PropertyController
                objPropertyController.Delete(_propertyID)

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager"), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub valPropertyTypeSubmission_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valPropertyTypeSubmission.ServerValidate

            If (trType.Visible) Then
                args.IsValid = False
                Dim objPropertyTypeController As New PropertyTypeController
                Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, Convert.ToInt32(drpTypes.SelectedValue))

                If (objPropertyType IsNot Nothing) Then
                    If (objPropertyType.AllowProperties) Then
                        args.IsValid = True
                    Else
                        args.IsValid = False
                    End If
                Else
                    args.IsValid = True
                End If
            Else
                args.IsValid = True
            End If

        End Sub

#End Region

    End Class

End Namespace