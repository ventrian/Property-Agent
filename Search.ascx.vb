Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security

Namespace Ventrian.PropertyAgent

    Partial Public Class Search
        Inherits PropertyAgentControl

#Region " Private Members "

        Dim _customFieldIDs As String = Null.NullString
        Dim _propertyTypeID As String = Null.NullString
        Dim _propertyAgentID As String = Null.NullString
        Dim _propertyBrokerID As String = Null.NullString
        Dim _searchValues As String = Null.NullString

        Private _objLayout As LayoutInfo

#End Region

#Region " Private Methods "

        Private Function BuildCustomFieldIDs(ByVal customFieldIDs As String, ByVal value As String) As String

            If (customFieldIDs.Length = 0) Then
                Return value
            Else
                Return customFieldIDs & "," & value
            End If

        End Function

        Private Function BuildSearchValues(ByVal searchValues As String, ByVal value As String) As String

            Dim objSecurity As New PortalSecurity

            If (searchValues.Length = 0) Then
                Return Server.UrlEncode(objSecurity.InputFilter(value, PortalSecurity.FilterFlag.NoScripting).Replace(vbCrLf, "").Replace(",", "^"))
            Else
                Return searchValues & "," & Server.UrlEncode(objSecurity.InputFilter(value, PortalSecurity.FilterFlag.NoScripting).Replace(",", "^"))
            End If

        End Function

        Private Sub BindDetails()

            trWildcard.Visible = PropertyAgentBase.PropertySettings(True).SearchWildcard

            If (PropertyAgentBase.PropertySettings.SearchWildcard) Then

                DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelpSearch, pnlHelpSearch, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                cmdHelpSearch.Visible = Not Me.PropertySettings.SearchHideHelpIcon
                lblLabelSearch.Text = PropertyAgentBase.GetResourceString("Search", Me.EditPropertyResourceFile, PropertySettings)
                lblHelpSearch.Text = PropertyAgentBase.GetResourceString("SearchHelp", Me.EditPropertyResourceFile, PropertySettings)
                imgHelpSearch.AlternateText = PropertyAgentBase.GetResourceString("SearchHelp", Me.EditPropertyResourceFile, PropertySettings)

                txtWildcard.Text = GetSearchValue(-1)
                txtWildcard.Width = PropertySettings.SearchWidth

            End If

            trTypes.Visible = PropertyAgentBase.PropertySettings(True).SearchTypes

            If (PropertyAgentBase.PropertySettings.SearchTypes) Then

                DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelpTypes, pnlHelpTypes, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                cmdHelpTypes.Visible = Not Me.PropertySettings.SearchHideHelpIcon
                lblLabelTypes.Text = PropertyAgentBase.GetResourceString("PropertyTypes", Me.EditPropertyResourceFile, PropertySettings)
                lblHelpTypes.Text = PropertyAgentBase.GetResourceString("PropertyTypesHelp", Me.EditPropertyResourceFile, PropertySettings)
                imgHelpTypes.AlternateText = PropertyAgentBase.GetResourceString("PropertyTypesHelp", Me.EditPropertyResourceFile, PropertySettings)

                drpPropertyTypes.Width = PropertySettings.SearchWidth
                If (Me.PropertySettings.SearchHideTypesCount) Then
                    drpPropertyTypes.DataTextField = "NameIndented"
                End If
                Dim objPropertyTypeController As New PropertyTypeController
                Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(PropertyAgentBase.ModuleId, True, PropertySettings.TypesSortBy, Null.NullString())
                If (Me.PropertySettings.SearchHideZeroTypes) Then
                    Dim objTypesSelected As New List(Of PropertyTypeInfo)

                    For Each objType As PropertyTypeInfo In objTypes
                        If (objType.PropertyCount > 0) Then
                            objTypesSelected.Add(objType)
                        End If
                    Next
                    drpPropertyTypes.DataSource = objTypesSelected
                    drpPropertyTypes.DataBind()
                Else
                    drpPropertyTypes.DataSource = objTypes
                    drpPropertyTypes.DataBind()
                End If

                Dim selectText As String = PropertyAgentBase.GetResourceString("SelectType", Me.EditPropertyResourceFile, PropertySettings)
                drpPropertyTypes.Items.Insert(0, New ListItem(selectText, "-1"))

                If Not (drpPropertyTypes.Items.FindByValue(_propertyTypeID.ToString()) Is Nothing) Then
                    drpPropertyTypes.SelectedValue = _propertyTypeID.ToString()
                End If

                For Each objCustomField As CustomFieldInfo In PropertyAgentBase.CustomFields
                    If (objCustomField.IsPublished AndAlso objCustomField.FieldType = CustomFieldType.DropDownList AndAlso objCustomField.FieldElementType = FieldElementType.LinkedToPropertyType) Then
                        drpPropertyTypes.AutoPostBack = True
                    End If
                Next

            End If

            trLocation.Visible = PropertyAgentBase.PropertySettings.SearchLocation

            If (PropertyAgentBase.PropertySettings.SearchLocation) Then

                DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelpLocation, pnlHelpLocation, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                cmdHelpLocation.Visible = Not Me.PropertySettings.SearchHideHelpIcon
                lblLabelLocation.Text = PropertyAgentBase.GetResourceString("Location", Me.EditPropertyResourceFile, PropertySettings)
                lblHelpLocation.Text = PropertyAgentBase.GetResourceString("LocationHelp", Me.EditPropertyResourceFile, PropertySettings)
                imgHelpLocation.AlternateText = PropertyAgentBase.GetResourceString("LocationHelp", Me.EditPropertyResourceFile, PropertySettings)

                txtLocation.Width = PropertySettings.SearchWidth

            End If

            trBrokers.Visible = PropertyAgentBase.PropertySettings(True).SearchBrokers

            If (PropertyAgentBase.PropertySettings.SearchBrokers) Then

                DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelpBrokers, pnlHelpBrokers, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                cmdHelpBrokers.Visible = Not Me.PropertySettings.SearchHideHelpIcon
                lblLabelBrokers.Text = PropertyAgentBase.GetResourceString("Broker", Me.EditPropertyResourceFile, PropertySettings)
                lblHelpBrokers.Text = PropertyAgentBase.GetResourceString("BrokerHelp", Me.EditPropertyResourceFile, PropertySettings)
                imgHelpBrokers.AlternateText = PropertyAgentBase.GetResourceString("BrokerHelp", Me.EditPropertyResourceFile, PropertySettings)

                Dim objAgentController As New AgentController(PortalSettings, _
                                                              PropertySettings, _
                                                              PropertyAgentBase.PortalId)
                Dim arrBrokers As ArrayList = objAgentController.ListOwners(PropertyAgentBase.PortalId, PropertyAgentBase.ModuleId, PropertySettings.PermissionBroker)

                If Not arrBrokers Is Nothing Then
                    drpPropertyBrokers.Width = PropertySettings.SearchWidth
                    drpPropertyBrokers.DataSource = arrBrokers
                    drpPropertyBrokers.DataTextField = "DisplayName"
                    drpPropertyBrokers.DataValueField = "UserId"
                    drpPropertyBrokers.DataBind()


                    Dim selectText As String = PropertyAgentBase.GetResourceString("SelectBroker", Me.EditPropertyResourceFile, PropertySettings)
                    drpPropertyBrokers.Items.Insert(0, New ListItem(selectText, "-1"))

                    If Not (drpPropertyBrokers.Items.FindByValue(_propertyBrokerID.ToString()) Is Nothing) Then
                        drpPropertyBrokers.SelectedValue = _propertyBrokerID.ToString()
                    End If
                End If

            End If

            BindAgents()

            cmdSearch.Text = Localization.GetString("cmdSearch.Text", PropertyAgentBase.LocalResourceFile)
            cmdSearch.CssClass = PropertySettings.SearchStyle
            rptDetails.DataSource = PropertyAgentBase.CustomFields
            rptDetails.DataBind()

        End Sub

        Private Sub BindAgents()

            trAgents.Visible = PropertyAgentBase.PropertySettings(True).SearchAgents

            If (PropertyAgentBase.PropertySettings.SearchAgents) Then

                DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelpAgents, pnlHelpAgents, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                cmdHelpAgents.Visible = Not Me.PropertySettings.SearchHideHelpIcon
                lblLabelAgents.Text = PropertyAgentBase.GetResourceString("Agent", Me.EditPropertyResourceFile, PropertySettings)
                lblHelpAgents.Text = PropertyAgentBase.GetResourceString("AgentHelp", Me.EditPropertyResourceFile, PropertySettings)
                imgHelpAgents.AlternateText = PropertyAgentBase.GetResourceString("AgentHelp", Me.EditPropertyResourceFile, PropertySettings)

                Dim objAgentController As New AgentController(PortalSettings, _
                                                              PropertySettings, _
                                                              PropertyAgentBase.PortalId)

                Dim arrAgents As ArrayList
                If (trBrokers.Visible AndAlso drpPropertyBrokers.SelectedValue <> "-1") Then
                    arrAgents = objAgentController.ListSelected(PropertyAgentBase.PortalId, PropertyAgentBase.ModuleId, Convert.ToInt32(drpPropertyBrokers.SelectedValue))
                Else
                    arrAgents = objAgentController.ListActive(PropertyAgentBase.PortalId, PropertyAgentBase.ModuleId)
                End If

                If Not arrAgents Is Nothing Then
                    drpPropertyAgents.Items.Clear()
                    drpPropertyAgents.Width = PropertySettings.SearchWidth
                    drpPropertyAgents.DataSource = arrAgents
                    drpPropertyAgents.DataTextField = "DisplayName"
                    drpPropertyAgents.DataValueField = "UserId"
                    drpPropertyAgents.DataBind()

                    Dim selectText As String = PropertyAgentBase.GetResourceString("SelectAgent", Me.EditPropertyResourceFile, PropertyAgentBase.PropertySettings)
                    drpPropertyAgents.Items.Insert(0, New ListItem(selectText, "-1"))

                    If Not (drpPropertyAgents.Items.FindByValue(_propertyAgentID.ToString()) Is Nothing) Then
                        drpPropertyAgents.SelectedValue = _propertyAgentID.ToString()
                    End If
                End If

            End If

        End Sub

        Private Function GetSearchValue(ByVal customFieldID As Integer) As String

            If (_customFieldIDs <> Null.NullString And _searchValues <> Null.NullString) Then
                Dim IDs As String() = _customFieldIDs.Split(","c)
                Dim searchValues As String() = _searchValues.Split(","c)

                If Not (IDs Is Nothing) Then
                    For i As Integer = 0 To IDs.Length - 1
                        Dim ID As String = IDs(i)
                        If (ID = customFieldID.ToString()) Then
                            Return searchValues(i).Replace("^", ",")
                        End If
                    Next
                End If

                Return ""
            Else
                Return ""
            End If

        End Function

        Private Function GetSearchValueRange(ByVal customFieldID As Integer, ByVal isFrom As Boolean) As String

            Dim val As String = GetSearchValue(customFieldID)

            If (val.Split("|"c).Length > 1) Then
                If (isFrom) Then
                    Return val.Split("|"c)(0)
                Else
                    Return val.Split("|"c)(1)
                End If
            End If

            Return ""

        End Function

        Public Function GetTypeLink(ByVal tabID As Integer, ByVal moduleID As Integer, ByVal propertyTypeID As Integer) As String

            Dim objTypesParam As New List(Of String)

            objTypesParam.Add(PropertySettings.SEOAgentType & "=ViewType")
            objTypesParam.Add(PropertySettings.SEOPropertyTypeID & "=" & propertyTypeID.ToString())

            If (PropertySettings.TypeParams) Then
                Dim types As New List(Of String)

                Dim objPropertyTypeController As New PropertyTypeController
                Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(moduleID, True, PropertyTypeSortByType.Standard, Null.NullString())

                For Each objType As PropertyTypeInfo In objTypes
                    If (objType.PropertyTypeID = propertyTypeID) Then
                        types.Add(objType.Name)

                        Dim i As Integer = 2
                        While objType.ParentID <> Null.NullInteger
                            For Each objParentType As PropertyTypeInfo In objTypes
                                If (objParentType.PropertyTypeID = objType.ParentID) Then
                                    types.Add(objParentType.Name)
                                    objType = objParentType
                                    i = i + 1
                                End If
                            Next
                        End While
                    End If
                Next

                Dim length As Integer = types.Count
                For Each t As String In types
                    objTypesParam.Insert(2, "Type" & length.ToString() & "=" & t)
                    length = length - 1
                Next
            End If

            Return NavigateURL(tabID, "", objTypesParam.ToArray())

        End Function

        Private Sub InitializeTemplate()

            Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, PropertyAgentBase.IsEditable, PropertyAgentBase.TabId, PropertyAgentBase.ModuleId, PropertyAgentBase.ModuleKey & "-Search")
            _objLayout = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_Item_Html)

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("CustomFieldIDs") Is Nothing) Then
                _customFieldIDs = Request("CustomFieldIDs")
            End If

            Dim propertyTypeIDParam As String = PropertySettings.SEOPropertyTypeID
            If (Request(propertyTypeIDParam) = "") Then
                propertyTypeIDParam = "PropertyTypeID"
            End If
            If Not (Request(propertyTypeIDParam) Is Nothing) Then
                _propertyTypeID = Request(propertyTypeIDParam)
            End If

            If Not (Request("PropertyAgentID") Is Nothing) Then
                _propertyAgentID = Request("PropertyAgentID")
            End If

            If Not (Request("PropertyBrokerID") Is Nothing) Then
                _propertyBrokerID = Request("PropertyBrokerID")
            End If

            If Not (Request("SearchValues") Is Nothing) Then
                _searchValues = Request("SearchValues")
            End If

            If (Page.IsPostBack) Then
                If (IsNumeric(Request(drpPropertyTypes.ClientID.ToString().Replace("_", "$")))) Then
                    _propertyTypeID = Convert.ToInt32(Request(drpPropertyTypes.ClientID.ToString().Replace("_", "$")))
                End If
            End If

        End Sub

#End Region

#Region " Private Properties "

        Private ReadOnly Property EditPropertyResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/EditProperty"
            End Get
        End Property

        Private Overloads ReadOnly Property PropertySettings() As PropertySettings
            Get
                Return PropertyAgentBase.PropertySettings
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                BindDetails()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(PropertyAgentBase, exc)
            End Try

        End Sub

        Private Sub rptDetails_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDetails.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objCustomField As CustomFieldInfo = CType(e.Item.DataItem, CustomFieldInfo)

                If (objCustomField.IsSearchable) Then
                    Dim phValue As PlaceHolder = CType(e.Item.FindControl("phValue"), PlaceHolder)

                    Dim cmdHelp As LinkButton = CType(e.Item.FindControl("cmdHelp"), LinkButton)
                    Dim pnlHelp As Panel = CType(e.Item.FindControl("pnlHelp"), Panel)
                    Dim lblLabel As Label = CType(e.Item.FindControl("lblLabel"), Label)
                    Dim lblHelp As Label = CType(e.Item.FindControl("lblHelp"), Label)
                    Dim imgHelp As Image = CType(e.Item.FindControl("imgHelp"), Image)

                    If Not (phValue Is Nothing) Then

                        DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdHelp, pnlHelp, True, DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxPersistanceType.None)

                        cmdHelp.Visible = Not Me.PropertySettings.SearchHideHelpIcon
                        lblLabel.Text = objCustomField.Caption & ":"
                        lblHelp.Text = objCustomField.CaptionHelp
                        imgHelp.AlternateText = objCustomField.CaptionHelp

                        Select Case (objCustomField.FieldType)

                            Case CustomFieldType.OneLineTextBox

                                Select Case objCustomField.SearchType

                                    Case SearchType.Default
                                        Dim objTextBox As New TextBox
                                        objTextBox.CssClass = "NormalTextBox"
                                        objTextBox.ID = objCustomField.CustomFieldID.ToString()
                                        objTextBox.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                                        phValue.Controls.Add(objTextBox)

                                        objTextBox.Text = GetSearchValue(objCustomField.CustomFieldID)

                                    Case SearchType.DropDown
                                        Dim objDropDown As New DropDownList
                                        objDropDown.CssClass = "NormalTextBox"
                                        objDropDown.ID = objCustomField.CustomFieldID.ToString()
                                        objDropDown.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                                        Dim objPropertyValueController As New PropertyValueController
                                        Dim objPropertyValues As List(Of PropertyValueInfo) = objPropertyValueController.ListByCustomField(objCustomField.CustomFieldID)
                                        For Each objPropertyValue As PropertyValueInfo In objPropertyValues
                                            If (objCustomField.HideZeroCount) Then
                                                Dim objPropertyController As New PropertyController()
                                                Dim count As Integer = 0
                                                objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), objPropertyValue.CustomValue, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                If (count > 0) Then
                                                    objDropDown.Items.Add(New ListItem(objPropertyValue.CustomValue, objPropertyValue.CustomValue))
                                                End If
                                            Else
                                                If (objCustomField.IncludeCount) Then
                                                    Dim objPropertyController As New PropertyController()
                                                    Dim count As Integer = 0
                                                    objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), objPropertyValue.CustomValue, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                    If (objCustomField.HideZeroCount) Then
                                                        If (count > 0) Then
                                                            objDropDown.Items.Add(New ListItem(objPropertyValue.CustomValue & " (" & count.ToString() & ")", objPropertyValue.CustomValue))
                                                        End If
                                                    Else
                                                        objDropDown.Items.Add(New ListItem(objPropertyValue.CustomValue & " (" & count.ToString() & ")", objPropertyValue.CustomValue))
                                                    End If
                                                Else
                                                    If (objCustomField.HideZeroCount) Then
                                                        Dim objPropertyController As New PropertyController()
                                                        Dim count As Integer = 0
                                                        objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), objPropertyValue.CustomValue, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                        If (count > 0) Then
                                                            objDropDown.Items.Add(New ListItem(objPropertyValue.CustomValue, objPropertyValue.CustomValue))
                                                        End If
                                                    Else
                                                        objDropDown.Items.Add(New ListItem(objPropertyValue.CustomValue, objPropertyValue.CustomValue))
                                                    End If
                                                End If
                                            End If
                                        Next
                                        objDropDown.Items.Insert(0, New ListItem(Localization.GetString("SelectItem", Me.EditPropertyResourceFile), "-1"))
                                        phValue.Controls.Add(objDropDown)

                                    Case SearchType.Range
                                        If (objCustomField.FieldElementsFrom <> "") Then
                                            Dim objDropDownFrom As New DropDownList
                                            objDropDownFrom.CssClass = "NormalTextBox"
                                            objDropDownFrom.ID = "From" & objCustomField.CustomFieldID.ToString()
                                            Dim values As String() = objCustomField.FieldElementsFrom.Split(Convert.ToChar("|"))
                                            For Each value As String In values
                                                Dim item As New ListItem
                                                item.Value = value
                                                item.Text = value
                                                Select Case objCustomField.ValidationType
                                                    Case CustomFieldValidationType.Currency
                                                        Try
                                                            Dim culture As String = "en-US"

                                                            Select Case PropertySettings.Currency

                                                                Case CurrencyType.AUD
                                                                    culture = "en-AU"
                                                                    Exit Select

                                                                Case CurrencyType.BRL
                                                                    culture = "pt-BR"
                                                                    Exit Select

                                                                Case CurrencyType.CAD
                                                                    culture = "en-CA"
                                                                    Exit Select

                                                                Case CurrencyType.CHF
                                                                    culture = "de-CH"
                                                                    Exit Select

                                                                Case CurrencyType.CNY
                                                                    culture = "zh-CN"
                                                                    Exit Select

                                                                Case CurrencyType.CZK
                                                                    culture = "cs-CZ"
                                                                    Exit Select

                                                                Case CurrencyType.EUR
                                                                    culture = "fr-FR"
                                                                    Select Case PropertySettings.EuroType
                                                                        Case EuroType.Dutch
                                                                            culture = "nl-NL"
                                                                            Exit Select

                                                                        Case EuroType.English
                                                                            culture = "en-IE"
                                                                            Exit Select

                                                                        Case EuroType.French
                                                                            culture = "fr-FR"
                                                                            Exit Select
                                                                    End Select
                                                                    Exit Select

                                                                Case CurrencyType.GBP
                                                                    culture = "en-GB"
                                                                    Exit Select

                                                                Case CurrencyType.JPY
                                                                    culture = "ja-JP"
                                                                    Exit Select

                                                                Case CurrencyType.USD
                                                                    culture = "en-US"
                                                                    Exit Select

                                                                Case CurrencyType.MYR
                                                                    culture = "en-MY"
                                                                    Exit Select

                                                                Case CurrencyType.NZD
                                                                    culture = "en-NZ"
                                                                    Exit Select

                                                                Case CurrencyType.NOK
                                                                    culture = "nb-NO"
                                                                    Exit Select

                                                                Case CurrencyType.THB
                                                                    culture = "th-TH"
                                                                    Exit Select

                                                                Case CurrencyType.ZAR
                                                                    culture = "en-ZA"
                                                                    Exit Select

                                                            End Select

                                                            Dim portalFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
                                                            Dim format As String = "{0:C" & PropertySettings.CurrencyDecimalPlaces.ToString() & "}"
                                                            item.Text = String.Format(portalFormat.NumberFormat, format, Double.Parse(value))
                                                        Catch
                                                        End Try
                                                    Case CustomFieldValidationType.Date
                                                        Try
                                                            item.Text = DateTime.Parse(value).ToShortDateString()
                                                        Catch
                                                        End Try
                                                End Select
                                                objDropDownFrom.Items.Add(item)
                                            Next
                                            objDropDownFrom.Width = Unit.Pixel(125)
                                            phValue.Controls.Add(objDropDownFrom)

                                            objDropDownFrom.Items.Insert(0, New ListItem(Localization.GetString("SelectFrom", Me.EditPropertyResourceFile), "-1"))

                                            If Not (objDropDownFrom.Items.FindByValue(GetSearchValueRange(objCustomField.CustomFieldID, True)) Is Nothing) Then
                                                objDropDownFrom.SelectedValue = GetSearchValueRange(objCustomField.CustomFieldID, True)
                                            End If
                                        Else
                                            Dim objTextBoxFrom As New TextBox
                                            objTextBoxFrom.CssClass = "NormalTextBox"
                                            objTextBoxFrom.ID = "From" & objCustomField.CustomFieldID.ToString()
                                            objTextBoxFrom.Width = Unit.Pixel(100)
                                            phValue.Controls.Add(objTextBoxFrom)

                                            If (objCustomField.ValidationType <> CustomFieldValidationType.None) Then
                                                Dim valCompare As New CompareValidator
                                                valCompare.ControlToValidate = objTextBoxFrom.ID
                                                Select Case objCustomField.ValidationType

                                                    Case CustomFieldValidationType.Currency
                                                        valCompare.Type = ValidationDataType.Integer
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateCurrency", Me.EditPropertyResourceFile)

                                                    Case CustomFieldValidationType.Date
                                                        valCompare.Type = ValidationDataType.Date
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateDate", Me.EditPropertyResourceFile)

                                                    Case CustomFieldValidationType.Double
                                                        valCompare.Type = ValidationDataType.Double
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateDecimal", Me.EditPropertyResourceFile)

                                                    Case CustomFieldValidationType.Integer
                                                        valCompare.Type = ValidationDataType.Integer
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateNumber", Me.EditPropertyResourceFile)

                                                End Select
                                                valCompare.CssClass = "NormalRed"
                                                valCompare.Display = ValidatorDisplay.Dynamic
                                                phValue.Controls.Add(valCompare)
                                            End If

                                            objTextBoxFrom.Text = GetSearchValueRange(objCustomField.CustomFieldID, True)
                                        End If

                                        If (objCustomField.FieldElementsFrom = "" Or objCustomField.FieldElementsTo = "") Then
                                            Dim objLabelTo As New Label
                                            objLabelTo.CssClass = "Normal"
                                            objLabelTo.Text = Localization.GetString("To", Me.EditPropertyResourceFile)
                                            phValue.Controls.Add(objLabelTo)
                                        End If

                                        If (objCustomField.FieldElementsTo <> "") Then
                                            Dim objDropDownTo As New DropDownList
                                            objDropDownTo.CssClass = "NormalTextBox"
                                            objDropDownTo.ID = "To" & objCustomField.CustomFieldID.ToString()
                                            Dim values As String() = objCustomField.FieldElementsTo.Split(Convert.ToChar("|"))
                                            For Each value As String In values
                                                Dim item As New ListItem
                                                item.Value = value
                                                item.Text = value
                                                Select Case objCustomField.ValidationType
                                                    Case CustomFieldValidationType.Currency
                                                        Try
                                                            Dim culture As String = "en-US"

                                                            Select Case PropertySettings.Currency

                                                                Case CurrencyType.AUD
                                                                    culture = "en-AU"
                                                                    Exit Select

                                                                Case CurrencyType.BRL
                                                                    culture = "pt-BR"
                                                                    Exit Select

                                                                Case CurrencyType.CAD
                                                                    culture = "en-CA"
                                                                    Exit Select

                                                                Case CurrencyType.CHF
                                                                    culture = "de-CH"
                                                                    Exit Select

                                                                Case CurrencyType.CNY
                                                                    culture = "zh-CN"
                                                                    Exit Select

                                                                Case CurrencyType.CZK
                                                                    culture = "cs-CZ"
                                                                    Exit Select

                                                                Case CurrencyType.EUR
                                                                    culture = "fr-FR"
                                                                    Select Case PropertySettings.EuroType
                                                                        Case EuroType.Dutch
                                                                            culture = "nl-NL"
                                                                            Exit Select

                                                                        Case EuroType.English
                                                                            culture = "en-IE"
                                                                            Exit Select

                                                                        Case EuroType.French
                                                                            culture = "fr-FR"
                                                                            Exit Select
                                                                    End Select
                                                                    Exit Select

                                                                Case CurrencyType.GBP
                                                                    culture = "en-GB"
                                                                    Exit Select

                                                                Case CurrencyType.JPY
                                                                    culture = "ja-JP"
                                                                    Exit Select

                                                                Case CurrencyType.USD
                                                                    culture = "en-US"
                                                                    Exit Select

                                                                Case CurrencyType.MYR
                                                                    culture = "en-MY"
                                                                    Exit Select

                                                                Case CurrencyType.NZD
                                                                    culture = "en-NZ"
                                                                    Exit Select

                                                                Case CurrencyType.NOK
                                                                    culture = "nb-NO"
                                                                    Exit Select

                                                                Case CurrencyType.ZAR
                                                                    culture = "en-ZA"
                                                                    Exit Select

                                                            End Select

                                                            Dim portalFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
                                                            Dim format As String = "{0:C" & PropertySettings.CurrencyDecimalPlaces.ToString() & "}"
                                                            item.Text = String.Format(portalFormat.NumberFormat, format, Double.Parse(value))
                                                        Catch
                                                        End Try
                                                    Case CustomFieldValidationType.Date
                                                        Try
                                                            item.Text = DateTime.Parse(value).ToShortDateString()
                                                        Catch
                                                        End Try
                                                End Select
                                                objDropDownTo.Items.Add(item)
                                            Next
                                            objDropDownTo.Width = Unit.Pixel(125)
                                            phValue.Controls.Add(objDropDownTo)

                                            objDropDownTo.Items.Insert(0, New ListItem(Localization.GetString("SelectTo", Me.EditPropertyResourceFile), "-1"))

                                            If Not (objDropDownTo.Items.FindByValue(GetSearchValueRange(objCustomField.CustomFieldID, False)) Is Nothing) Then
                                                objDropDownTo.SelectedValue = GetSearchValueRange(objCustomField.CustomFieldID, False)
                                            End If
                                        Else

                                            Dim objTextBoxTo As New TextBox
                                            objTextBoxTo.CssClass = "NormalTextBox"
                                            objTextBoxTo.ID = "To" & objCustomField.CustomFieldID.ToString()
                                            objTextBoxTo.Width = Unit.Pixel(100)
                                            phValue.Controls.Add(objTextBoxTo)

                                            If (objCustomField.ValidationType <> CustomFieldValidationType.None) Then
                                                Dim valCompare As New CompareValidator
                                                valCompare.ControlToValidate = objTextBoxTo.ID
                                                Select Case objCustomField.ValidationType

                                                    Case CustomFieldValidationType.Currency
                                                        valCompare.Type = ValidationDataType.Integer
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateCurrency", Me.EditPropertyResourceFile)

                                                    Case CustomFieldValidationType.Date
                                                        valCompare.Type = ValidationDataType.Date
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateDate", Me.EditPropertyResourceFile)

                                                    Case CustomFieldValidationType.Double
                                                        valCompare.Type = ValidationDataType.Double
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateDecimal", Me.EditPropertyResourceFile)

                                                    Case CustomFieldValidationType.Integer
                                                        valCompare.Type = ValidationDataType.Integer
                                                        valCompare.Operator = ValidationCompareOperator.DataTypeCheck
                                                        valCompare.ErrorMessage = Localization.GetString("valValidateNumber", Me.EditPropertyResourceFile)

                                                End Select
                                                valCompare.CssClass = "NormalRed"
                                                valCompare.Display = ValidatorDisplay.Dynamic
                                                phValue.Controls.Add(valCompare)
                                            End If

                                            objTextBoxTo.Text = GetSearchValueRange(objCustomField.CustomFieldID, False)
                                        End If

                                End Select

                            Case CustomFieldType.MultiLineTextBox

                                Dim objTextBox As New TextBox
                                objTextBox.CssClass = "NormalTextBox"
                                objTextBox.ID = objCustomField.CustomFieldID.ToString()
                                objTextBox.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                                phValue.Controls.Add(objTextBox)

                                objTextBox.Text = GetSearchValue(objCustomField.CustomFieldID)

                            Case CustomFieldType.RichTextBox

                                Dim objTextBox As New TextBox
                                objTextBox.CssClass = "NormalTextBox"
                                objTextBox.ID = objCustomField.CustomFieldID.ToString()
                                objTextBox.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)
                                phValue.Controls.Add(objTextBox)

                                objTextBox.Text = GetSearchValue(objCustomField.CustomFieldID)

                            Case CustomFieldType.DropDownList

                                Dim objListControl As ListControl

                                If (objCustomField.SearchType = SearchType.Multiple) Then
                                    objListControl = New ListBox
                                    CType(objListControl, ListBox).SelectionMode = ListSelectionMode.Multiple
                                Else
                                    objListControl = New DropDownList
                                End If
                                objListControl.CssClass = "NormalTextBox"
                                objListControl.ID = objCustomField.CustomFieldID.ToString()
                                objListControl.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)

                                If (objCustomField.FieldElementType = FieldElementType.Standard) Then
                                    Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                    For Each value As String In values
                                        If (value.Trim() <> "") Then
                                            If (objCustomField.IncludeCount) Then
                                                Dim objPropertyController As New PropertyController()
                                                Dim count As Integer = 0
                                                objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                If (objCustomField.HideZeroCount) Then
                                                    If (count > 0) Then
                                                        objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                                    End If
                                                Else
                                                    objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                                End If
                                            Else
                                                If (objCustomField.HideZeroCount) Then
                                                    Dim objPropertyController As New PropertyController()
                                                    Dim count As Integer = 0
                                                    objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                    If (count > 0) Then
                                                        objListControl.Items.Add(value)
                                                    End If
                                                Else
                                                    objListControl.Items.Add(value)
                                                End If
                                            End If
                                        End If
                                    Next

                                    For Each objCustomFieldDropDown As CustomFieldInfo In PropertyAgentBase.CustomFields
                                        If (objCustomFieldDropDown.IsPublished AndAlso objCustomFieldDropDown.IsSearchable AndAlso objCustomFieldDropDown.FieldType = CustomFieldType.DropDownList AndAlso objCustomFieldDropDown.FieldElementType = FieldElementType.LinkedToDropdown AndAlso objCustomFieldDropDown.FieldElementDropDown = objCustomField.CustomFieldID) Then
                                            objListControl.AutoPostBack = True
                                        End If
                                    Next
                                End If

                                If (objCustomField.FieldElementType = FieldElementType.SqlQuery) Then
                                    For Each value As String In objCustomField.FieldElementsSql
                                        If (value.Trim() <> "") Then
                                            If (objCustomField.IncludeCount) Then
                                                Dim objPropertyController As New PropertyController()
                                                Dim count As Integer = 0
                                                objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                If (objCustomField.HideZeroCount) Then
                                                    If (count > 0) Then
                                                        objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                                    End If
                                                Else
                                                    objListControl.Items.Add(New ListItem(value & " (" & count.ToString() & ")", value))
                                                End If
                                            Else
                                                If (objCustomField.HideZeroCount) Then
                                                    Dim objPropertyController As New PropertyController()
                                                    Dim count As Integer = 0
                                                    objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                    If (count > 0) Then
                                                        objListControl.Items.Add(value)
                                                    End If
                                                Else
                                                    objListControl.Items.Add(value)
                                                End If
                                            End If
                                        End If
                                    Next
                                End If

                                If (objCustomField.FieldElementType = FieldElementType.LinkedToPropertyType) Then
                                    If (drpPropertyTypes.Items.Count > 1) Then
                                        If (drpPropertyTypes.SelectedIndex > 0) Then
                                            Dim index As Integer = 0
                                            Dim objPropertyTypeController As New PropertyTypeController
                                            Dim objTypes As List(Of PropertyTypeInfo) = objPropertyTypeController.ListAll(Me.PropertyAgentBase.ModuleId, True, Me.PropertySettings.TypesSortBy, Null.NullString())

                                            For Each objType As PropertyTypeInfo In objTypes
                                                If (objType.PropertyTypeID.ToString() = drpPropertyTypes.SelectedValue) Then
                                                    Exit For
                                                End If
                                                index = index + 1
                                            Next
                                            If (index < objCustomField.FieldElements.Split(vbCrLf).Length) Then
                                                Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(index).Split(Convert.ToChar("|"))
                                                For Each value As String In values
                                                    If (value.Trim() <> "") Then
                                                        If (objCustomField.IncludeCount) Then
                                                            Dim objPropertyController As New PropertyController()
                                                            Dim count As Integer = 0
                                                            objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                            If (objCustomField.HideZeroCount) Then
                                                                If (count > 0) Then
                                                                    objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                End If
                                                            Else
                                                                objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                            End If
                                                        Else
                                                            If (objCustomField.HideZeroCount) Then
                                                                Dim objPropertyController As New PropertyController()
                                                                Dim count As Integer = 0
                                                                objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                If (count > 0) Then
                                                                    objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                End If
                                                            Else
                                                                objListControl.Items.Add(value.Replace(vbLf, ""))
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If

                                If (objCustomField.FieldElementType = FieldElementType.LinkedToDropdown) Then

                                    Dim found As Boolean = False
                                    Dim postbackID As String = Page.Request.Params.Get("__EVENTTARGET")
                                    If (postbackID <> "") Then
                                        Dim arrItems As String() = postbackID.Split("$")
                                        If (arrItems.Length > 0) Then
                                            Try
                                                Dim customFieldID As Integer = Convert.ToInt32(arrItems(arrItems.Length - 1))
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
                                                                                If (objCustomField.IncludeCount) Then
                                                                                    Dim objPropertyController As New PropertyController()
                                                                                    Dim count As Integer = 0
                                                                                    objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                                    If (objCustomField.HideZeroCount) Then
                                                                                        If (count > 0) Then
                                                                                            objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                                        End If
                                                                                    Else
                                                                                        objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                                    End If
                                                                                Else
                                                                                    If (objCustomField.HideZeroCount) Then
                                                                                        Dim objPropertyController As New PropertyController()
                                                                                        Dim count As Integer = 0
                                                                                        objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                                        If (count > 0) Then
                                                                                            objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                                        End If
                                                                                    Else
                                                                                        objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                                    End If
                                                                                End If
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
                                            Catch
                                            End Try
                                        End If
                                    End If

                                    If (found = False) Then
                                        For Each key As String In Request.Form.AllKeys
                                            If (key IsNot Nothing AndAlso key.ToLower().EndsWith("$" & objCustomField.FieldElementDropDown.ToString().ToLower())) Then
                                                If (key.ToLower().Contains("rptdetails") And key.ToLower().Contains("landingpage")) Then

                                                    If (Request(key) <> "") Then

                                                        For Each objCustomFieldLinked As CustomFieldInfo In CustomFields
                                                            If (objCustomFieldLinked.CustomFieldID = objCustomField.FieldElementDropDown) Then
                                                                Dim i As Integer = 0
                                                                For Each val As String In objCustomFieldLinked.FieldElements.Split("|")
                                                                    If (val.ToLower() = Request(key).ToLower()) Then
                                                                        Dim values As String() = objCustomField.FieldElements.Split(vbCrLf)(i).Split(Convert.ToChar("|"))
                                                                        For Each value As String In values
                                                                            If (value.Trim() <> "") Then
                                                                                If (objCustomField.IncludeCount) Then
                                                                                    Dim objPropertyController As New PropertyController()
                                                                                    Dim count As Integer = 0
                                                                                    objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                                    If (objCustomField.HideZeroCount) Then
                                                                                        If (count > 0) Then
                                                                                            objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                                        End If
                                                                                    Else
                                                                                        objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                                    End If
                                                                                Else
                                                                                    If (objCustomField.HideZeroCount) Then
                                                                                        Dim objPropertyController As New PropertyController()
                                                                                        Dim count As Integer = 0
                                                                                        objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value, 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                                        If (count > 0) Then
                                                                                            objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                                        End If
                                                                                    Else
                                                                                        objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                                    End If
                                                                                End If
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

                                    If (found = False) Then

                                        Dim fieldVal As String = GetSearchValue(objCustomField.FieldElementDropDown)
                                        If (fieldVal <> "") Then

                                            For Each objCustomFieldLinked As CustomFieldInfo In CustomFields
                                                If (objCustomFieldLinked.CustomFieldID = objCustomField.FieldElementDropDown) Then
                                                    found = True
                                                    Dim i As Integer = 0
                                                    For Each valItem As String In objCustomFieldLinked.FieldElements.Split("|")
                                                        If (valItem.ToLower() = fieldVal.ToLower()) Then
                                                            Dim valueItems As String() = objCustomField.FieldElements.Split(vbCrLf)(i).Split(Convert.ToChar("|"))
                                                            For Each value As String In valueItems
                                                                If (value.Trim() <> "") Then
                                                                    If (objCustomField.IncludeCount) Then
                                                                        Dim objPropertyController As New PropertyController()
                                                                        Dim count As Integer = 0
                                                                        objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                        If (objCustomField.HideZeroCount) Then
                                                                            If (count > 0) Then
                                                                                objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                            End If
                                                                        Else
                                                                            objListControl.Items.Add(New ListItem(value.Replace(vbLf, "") & " (" & count.ToString() & ")", value.Replace(vbLf, "")))
                                                                        End If
                                                                    Else
                                                                        If (objCustomField.HideZeroCount) Then
                                                                            Dim objPropertyController As New PropertyController()
                                                                            Dim count As Integer = 0
                                                                            objPropertyController.List(Me.PropertyAgentBase.ModuleId, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, SortByType.Hits, Null.NullInteger, SortDirectionType.Descending, objCustomField.CustomFieldID.ToString(), value.Replace(vbLf, ""), 1, 10, count, False, False, Null.NullInteger, Null.NullInteger)
                                                                            If (count > 0) Then
                                                                                objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                            End If
                                                                        Else
                                                                            objListControl.Items.Add(value.Replace(vbLf, ""))
                                                                        End If
                                                                    End If
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

                                If (objCustomField.SearchType = SearchType.Multiple) Then
                                    Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                                    For Each val As String In vals
                                        For Each item As ListItem In objListControl.Items
                                            If (item.Value = val) Then
                                                item.Selected = True
                                            End If
                                        Next
                                    Next
                                Else
                                    Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                                    Try
                                        selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                                        objListControl.Items.Insert(0, New ListItem(selectText, "-1"))
                                    Catch
                                        objListControl.Items.Insert(0, New ListItem("Select Value", "-1"))
                                    End Try

                                    Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                                    If (val <> "") Then
                                        If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                                            objListControl.SelectedValue = val
                                        End If
                                    End If
                                End If

                                phValue.Controls.Add(objListControl)

                            Case CustomFieldType.ListBox

                                Dim objListControl As ListControl

                                If (objCustomField.SearchType = SearchType.Default) Then
                                    objListControl = New ListBox
                                    CType(objListControl, ListBox).SelectionMode = ListSelectionMode.Multiple
                                Else
                                    objListControl = New DropDownList
                                End If
                                objListControl.CssClass = "NormalTextBox"
                                objListControl.ID = objCustomField.CustomFieldID.ToString()
                                objListControl.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)

                                Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                For Each value As String In values
                                    If (value.Trim() <> "") Then
                                        objListControl.Items.Add(value)
                                    End If
                                Next

                                If (objCustomField.SearchType = SearchType.Default) Then
                                    Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                                    For Each val As String In vals
                                        For Each item As ListItem In objListControl.Items
                                            If (item.Value = val) Then
                                                item.Selected = True
                                            End If
                                        Next
                                    Next
                                Else
                                    Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                                    selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                                    objListControl.Items.Insert(0, New ListItem(selectText, "-1"))

                                    Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                                    If (val <> "") Then
                                        If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                                            objListControl.SelectedValue = val
                                        End If
                                    End If
                                End If

                                phValue.Controls.Add(objListControl)

                            Case CustomFieldType.CheckBox

                                Dim objCheckBox As New CheckBox
                                objCheckBox.CssClass = "Normal"
                                objCheckBox.ID = objCustomField.CustomFieldID.ToString()
                                phValue.Controls.Add(objCheckBox)

                                Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                                If (val <> "") Then
                                    Try
                                        objCheckBox.Checked = Boolean.Parse(val)
                                    Catch

                                    End Try
                                End If

                            Case CustomFieldType.MultiCheckBox

                                If (objCustomField.SearchType = SearchType.DropDown) Then

                                    Dim objListControl = New DropDownList
                                    objListControl.CssClass = "NormalTextBox"
                                    objListControl.ID = objCustomField.CustomFieldID.ToString()
                                    objListControl.Width = Unit.Pixel(Me.PropertySettings.SearchWidth)

                                    Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                    For Each value As String In values
                                        If (value <> "") Then
                                            objListControl.Items.Add(value)
                                        End If
                                    Next

                                    Dim selectText As String = Localization.GetString("SelectValue", Me.EditPropertyResourceFile)
                                    selectText = selectText.Replace("[VALUE]", objCustomField.Caption)
                                    objListControl.Items.Insert(0, New ListItem(selectText, "-1"))

                                    Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                                    If (val <> "") Then
                                        If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                                            objListControl.SelectedValue = val
                                        End If
                                    End If

                                    phValue.Controls.Add(objListControl)

                                Else
                                    Dim objCheckBoxList As New CheckBoxList
                                    objCheckBoxList.CssClass = "Normal"
                                    objCheckBoxList.ID = objCustomField.CustomFieldID.ToString()
                                    objCheckBoxList.RepeatColumns = PropertyAgentBase.PropertySettings.CheckBoxItemsPerRow
                                    objCheckBoxList.RepeatDirection = RepeatDirection.Horizontal

                                    Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                    For Each value As String In values
                                        If (value <> "") Then
                                            objCheckBoxList.Items.Add(value)
                                        End If
                                    Next

                                    Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                                    For Each val As String In vals
                                        For Each item As ListItem In objCheckBoxList.Items
                                            If (item.Value = val) Then
                                                item.Selected = True
                                            End If
                                        Next
                                    Next

                                    phValue.Controls.Add(objCheckBoxList)
                                End If

                            Case CustomFieldType.RadioButton

                                Dim objListControl As ListControl

                                If (objCustomField.SearchType = SearchType.Multiple) Then
                                    objListControl = New CheckBoxList
                                Else
                                    objListControl = New RadioButtonList
                                End If

                                objListControl.CssClass = "Normal"
                                objListControl.ID = objCustomField.CustomFieldID.ToString()

                                If (objCustomField.SearchType = SearchType.Multiple) Then
                                    CType(objListControl, CheckBoxList).RepeatDirection = RepeatDirection.Horizontal
                                    CType(objListControl, CheckBoxList).RepeatLayout = RepeatLayout.Table
                                    CType(objListControl, CheckBoxList).RepeatColumns = PropertyAgentBase.PropertySettings.CheckBoxItemsPerRow
                                Else
                                    CType(objListControl, RadioButtonList).RepeatDirection = RepeatDirection.Horizontal
                                    CType(objListControl, RadioButtonList).RepeatLayout = RepeatLayout.Table
                                    CType(objListControl, RadioButtonList).RepeatColumns = PropertyAgentBase.PropertySettings.RadioButtonItemsPerRow
                                End If

                                Dim values As String() = objCustomField.FieldElements.Split(Convert.ToChar("|"))
                                For Each value As String In values
                                    If (value <> "") Then
                                        objListControl.Items.Add(value)
                                    End If
                                Next

                                If (objCustomField.SearchType = SearchType.Multiple) Then
                                    Dim vals As String() = GetSearchValue(objCustomField.CustomFieldID).Split("|"c)
                                    For Each val As String In vals
                                        For Each item As ListItem In objListControl.Items
                                            If (item.Value = val) Then
                                                item.Selected = True
                                            End If
                                        Next
                                    Next
                                Else
                                    Dim val As String = GetSearchValue(objCustomField.CustomFieldID)

                                    If Not (objListControl.Items.FindByValue(val) Is Nothing) Then
                                        objListControl.SelectedValue = val
                                    End If
                                End If

                                phValue.Controls.Add(objListControl)

                            Case CustomFieldType.Hyperlink

                                e.Item.Visible = False

                            Case CustomFieldType.FileUpload

                                e.Item.Visible = False

                        End Select

                    End If
                Else
                    e.Item.Visible = False
                End If

            End If

        End Sub

        Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

            Try

                If (Page.IsValid) Then

                    Dim objSecurity As New PortalSecurity

                    Dim customFieldIDs As String = ""
                    Dim searchValues As String = ""
                    Dim propertyTypeIDs As String = ""
                    Dim location As String = ""
                    Dim propertyAgentIDs As String = ""
                    Dim propertyBrokerIDs As String = ""

                    Dim objCustomFields As List(Of CustomFieldInfo) = PropertyAgentBase.CustomFields

                    If (PropertyAgentBase.PropertySettings.SearchWildcard = True) Then

                        If (txtWildcard.Text.Trim() <> "") Then
                            customFieldIDs = "-1"
                            searchValues = objSecurity.InputFilter(txtWildcard.Text.Trim(), PortalSecurity.FilterFlag.NoScripting)
                        End If

                    End If

                    If (PropertyAgentBase.PropertySettings.SearchTypes = True) Then

                        If (drpPropertyTypes.SelectedValue <> "-1") Then
                            propertyTypeIDs = PropertySettings.SEOPropertyTypeID & "=" & drpPropertyTypes.SelectedValue
                        End If

                    End If

                    If (PropertyAgentBase.PropertySettings.SearchLocation = True) Then

                        If (txtLocation.Text.Trim() <> "") Then
                            location = "Location=" & Server.UrlEncode(txtLocation.Text.Trim())
                        End If

                    End If

                    If (PropertyAgentBase.PropertySettings.SearchAgents = True) Then

                        If (drpPropertyAgents.SelectedValue <> "-1") Then
                            propertyAgentIDs = "PropertyAgentID=" & drpPropertyAgents.SelectedValue
                        End If

                    End If

                    If (PropertyAgentBase.PropertySettings.SearchBrokers = True) Then

                        If (drpPropertyBrokers.SelectedValue <> "-1") Then
                            propertyBrokerIDs = "PropertyBrokerID=" & drpPropertyBrokers.SelectedValue
                        End If

                    End If

                    For Each item As RepeaterItem In rptDetails.Items

                        If (item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem) Then

                            Dim phValue As PlaceHolder = CType(item.FindControl("phValue"), PlaceHolder)

                            If Not (phValue Is Nothing) Then
                                If (phValue.Controls.Count > 0) Then

                                    Dim objControl As System.Web.UI.Control = phValue.Controls(0)

                                    Dim ID As Integer = Convert.ToInt32(objControl.ID.Replace("From", "").Replace("To", ""))
                                    Dim customFieldID As Integer = ID

                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.CustomFieldID = customFieldID) Then
                                            Select Case objCustomField.FieldType

                                                Case CustomFieldType.OneLineTextBox

                                                    Select Case objCustomField.SearchType

                                                        Case SearchType.Default
                                                            Dim objTextBox As TextBox = CType(objControl, TextBox)
                                                            If (objTextBox.Text <> "") Then
                                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                                searchValues = BuildSearchValues(searchValues, objTextBox.Text)
                                                            End If

                                                        Case SearchType.DropDown
                                                            Dim objDropDown As DropDownList = CType(objControl, DropDownList)
                                                            If (objDropDown.SelectedValue <> "-1") Then
                                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                                searchValues = BuildSearchValues(searchValues, objDropDown.SelectedValue)
                                                            End If

                                                        Case SearchType.Range
                                                            Dim fromValue As String = ""
                                                            If (objCustomField.FieldElementsFrom <> "") Then
                                                                Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                                                If (objDropDownList.SelectedValue <> "-1") Then
                                                                    fromValue = objDropDownList.SelectedValue
                                                                End If
                                                            Else
                                                                Dim objTextBoxFrom As TextBox = CType(objControl, TextBox)
                                                                fromValue = objTextBoxFrom.Text
                                                            End If

                                                            Dim toValue As String = ""
                                                            If (objCustomField.FieldElementsTo <> "") Then
                                                                Dim objDropDownList As DropDownList = CType(phValue.FindControl("To" & customFieldID.ToString()), DropDownList)
                                                                If (objDropDownList.SelectedValue <> "-1") Then
                                                                    toValue = objDropDownList.SelectedValue
                                                                End If
                                                            Else
                                                                Dim objTextBoxTo As TextBox = CType(phValue.FindControl("To" & customFieldID.ToString()), TextBox)
                                                                toValue = objTextBoxTo.Text
                                                            End If

                                                            Dim searchValue As String = "|"

                                                            If (fromValue <> "") Then
                                                                searchValue = fromValue & searchValue
                                                            End If

                                                            If (toValue <> "") Then
                                                                searchValue = searchValue & toValue
                                                            End If

                                                            If (searchValue <> "|") Then
                                                                customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                                searchValues = BuildSearchValues(searchValues, searchValue)
                                                            End If

                                                    End Select

                                                Case CustomFieldType.MultiLineTextBox
                                                    Dim objTextBox As TextBox = CType(objControl, TextBox)
                                                    If (objTextBox.Text <> "") Then
                                                        customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                        searchValues = BuildSearchValues(searchValues, objTextBox.Text)
                                                    End If

                                                Case CustomFieldType.RichTextBox
                                                    Dim objTextBox As TextBox = CType(objControl, TextBox)
                                                    If (objTextBox.Text <> "") Then
                                                        customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                        searchValues = BuildSearchValues(searchValues, objTextBox.Text)
                                                    End If

                                                Case CustomFieldType.DropDownList

                                                    If (objCustomField.SearchType = SearchType.Multiple) Then
                                                        Dim objListBox As ListBox = CType(objControl, ListBox)
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
                                                        If (values <> "") Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, values)
                                                        End If
                                                    Else
                                                        Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                                        If (objDropDownList.SelectedValue <> "-1") Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, objDropDownList.SelectedValue.ToString())
                                                        End If
                                                    End If

                                                Case CustomFieldType.ListBox

                                                    If (objCustomField.SearchType = SearchType.Default) Then
                                                        Dim objListBox As ListBox = CType(objControl, ListBox)
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
                                                        If (values <> "") Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, values)
                                                        End If
                                                    Else
                                                        Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                                        If (objDropDownList.SelectedValue <> "-1") Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, objDropDownList.SelectedValue.ToString())
                                                        End If
                                                    End If

                                                Case CustomFieldType.CheckBox
                                                    Dim objCheckBox As CheckBox = CType(objControl, CheckBox)
                                                    If (objCheckBox.Checked) Then
                                                        customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                        searchValues = BuildSearchValues(searchValues, objCheckBox.Checked.ToString())
                                                    End If

                                                Case CustomFieldType.MultiCheckBox
                                                    If (objCustomField.SearchType = SearchType.DropDown) Then
                                                        Dim objDropDownList As DropDownList = CType(objControl, DropDownList)
                                                        If (objDropDownList.SelectedValue <> "-1") Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, objDropDownList.SelectedValue)
                                                        End If
                                                    Else
                                                        Dim objCheckBoxList As CheckBoxList = CType(objControl, CheckBoxList)
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
                                                        If (values <> "") Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, values)
                                                        End If
                                                    End If

                                                Case CustomFieldType.RadioButton

                                                    If (objCustomField.SearchType = SearchType.Multiple) Then
                                                        Dim objCheckBoxList As CheckBoxList = CType(objControl, CheckBoxList)
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
                                                        If (values <> "") Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, values)
                                                        End If
                                                    Else
                                                        Dim objRadioButtonList As RadioButtonList = CType(objControl, RadioButtonList)
                                                        If Not (objRadioButtonList.SelectedItem Is Nothing) Then
                                                            customFieldIDs = BuildCustomFieldIDs(customFieldIDs, objCustomField.CustomFieldID.ToString())
                                                            searchValues = BuildSearchValues(searchValues, objRadioButtonList.SelectedValue)
                                                        End If
                                                    End If

                                            End Select

                                            Exit For
                                        End If
                                    Next

                                End If
                            End If
                        End If
                    Next

                    Dim objParams As New ArrayList()
                    If (propertyTypeIDs <> "" And propertyAgentIDs = "" And propertyBrokerIDs = "" And customFieldIDs = "" And searchValues = "" And location = "") Then
                        Response.Redirect(GetTypeLink(PropertyAgentBase.TabId, PropertyAgentBase.ModuleId, Convert.ToInt32(propertyTypeIDs.Split("="c)(1))), True)
                    Else
                        objParams.Add(PropertySettings.SEOAgentType & "=ViewSearch")
                        If (customFieldIDs <> "" And searchValues <> "") Then
                            objParams.Add("CustomFieldIDs=" & customFieldIDs)
                            objParams.Add("SearchValues=" & searchValues)
                        End If
                        objParams.Add(propertyTypeIDs)
                        objParams.Add(location)
                        objParams.Add(propertyAgentIDs)
                        objParams.Add(propertyBrokerIDs)
                        Response.Redirect(NavigateURL(PropertyAgentBase.TabId, "", CType(objParams.ToArray(GetType(String)), String())), True)
                    End If


                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(PropertyAgentBase, exc)
            End Try

        End Sub

        Protected Sub drpPropertyBrokers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpPropertyBrokers.SelectedIndexChanged

            Try

                BindAgents()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(PropertyAgentBase, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace