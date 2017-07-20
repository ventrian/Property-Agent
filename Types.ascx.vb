Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class Types
        Inherits PropertyAgentTypesBase

#Region " Private Members "

        Private _objLayoutTypes As LayoutInfo
        Private _objLayoutTypesHeader As LayoutInfo
        Private _objLayoutTypesFooter As LayoutInfo

#End Region

#Region " Private Methods "

        Private Sub BindTypes()

            Dim objPropertyTypeController As New PropertyTypeController
            Dim objTypes As List(Of PropertyTypeInfo)

            If (PropertySettingsTypes.ShowTopLevelOnly) Then

                Dim propertyTypeID As Integer = Null.NullInteger

                Dim propertyTypeIDParam As String = PropertySettings.SEOPropertyTypeID
                If (Request(propertyTypeIDParam) = "") Then
                    propertyTypeIDParam = "PropertyTypeID"
                End If
                If Not (Request(propertyTypeIDParam) Is Nothing) Then
                    Integer.TryParse(Request(propertyTypeIDParam), propertyTypeID)
                End If

                objTypes = objPropertyTypeController.List(Me.PropertySettingsTypes.PropertyAgentModuleID, True, PropertySettingsTypes.TypesSortBy, PropertySettingsTypes.AgentFilter, propertyTypeID)

                If (objTypes.Count = 0) Then

                    If (propertyTypeID <> Null.NullInteger) Then
                        Dim objTypeParent As PropertyTypeInfo = objPropertyTypeController.Get(Me.PropertySettingsTypes.PropertyAgentModuleID, propertyTypeID)
                        
                        If (objTypeParent IsNot Nothing) Then
                            objTypes = objPropertyTypeController.List(Me.PropertySettingsTypes.PropertyAgentModuleID, True, PropertySettingsTypes.TypesSortBy, PropertySettingsTypes.AgentFilter, objTypeParent.ParentID)
                        End If

                    End If


                End If
            Else
                objTypes = objPropertyTypeController.ListAll(Me.PropertySettingsTypes.PropertyAgentModuleID, True, PropertySettingsTypes.TypesSortBy, Me.PropertySettingsTypes.AgentFilter)
            End If

            Dim objTypesSelected As New List(Of PropertyTypeInfo)

            For Each objType As PropertyTypeInfo In objTypes
                If (PropertySettingsTypes.HideZeroCount) Then
                    If (objType.PropertyCount > 0) Then
                        objTypesSelected.Add(objType)
                    End If
                Else
                    objTypesSelected.Add(objType)
                End If
            Next

            ProcessHeaderFooter(phProperty.Controls, _objLayoutTypesHeader.Tokens)

            Dim objRepeater As New System.Web.UI.WebControls.Repeater
            Dim objHandler As New RepeaterItemEventHandler(AddressOf dlTypes_ItemDataBound)
            AddHandler objRepeater.ItemDataBound, objHandler

            objRepeater.DataSource = objTypesSelected
            objRepeater.DataBind()

            phProperty.Controls.Add(objRepeater)

            ProcessHeaderFooter(phProperty.Controls, _objLayoutTypesFooter.Tokens)

        End Sub

        Private Sub InitializeTemplate()

            Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.PropertySettingsTypes.PropertyAgentTabID, Me.PropertySettingsTypes.PropertyAgentModuleID, Me.ModuleKey)

            If (Me.PropertySettingsTypes.LayoutMode = LatestLayoutMode.TemplateLayout) Then

                _objLayoutTypes = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Types_Item_Html)
                _objLayoutTypesHeader = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Types_Header_Html)
                _objLayoutTypesFooter = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Types_Footer_Html)

            Else

                Dim delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                _objLayoutTypes = New LayoutInfo
                _objLayoutTypes.Template = Me.PropertySettingsTypes.LayoutItem
                _objLayoutTypes.Tokens = _objLayoutTypes.Template.Split(delimiter)

                _objLayoutTypesHeader = New LayoutInfo
                _objLayoutTypesHeader.Template = Me.PropertySettingsTypes.LayoutHeader
                _objLayoutTypesHeader.Tokens = _objLayoutTypesHeader.Template.Split(delimiter)

                _objLayoutTypesFooter = New LayoutInfo
                _objLayoutTypesFooter.Template = Me.PropertySettingsTypes.LayoutFooter
                _objLayoutTypesFooter.Tokens = _objLayoutTypesFooter.Template.Split(delimiter)

            End If

            If (Me.PropertySettingsTypes.IncludeStylesheet) Then
                objLayoutController.LoadStyleSheet(Me.PropertySettings.Template, False)
            End If

        End Sub

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "ISMOBILEDEVICE"
                            If LayoutController.IsMobileBrowser() = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISMOBILEDEVICE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISMOBILEDEVICE"
                            ' Do Nothing

                        Case "ISNOTMOBILEDEVICE"
                            If LayoutController.IsMobileBrowser() = True Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTMOBILEDEVICE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTMOBILEDEVICE"
                            ' Do Nothing

                    End Select
                End If
            Next

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                If (Me.PropertySettingsTypes.PropertyAgentModuleID = Null.NullInteger) Then
                    Dim objLabel As New Label
                    objLabel.Text = Localization.GetString("Configure", Me.LocalResourceFile)
                    objLabel.CssClass = "Normal"
                    phProperty.Controls.Add(objLabel)
                    Return
                End If

                InitializeTemplate()
                BindTypes()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlTypes_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objType As PropertyTypeInfo = CType(e.Item.DataItem, PropertyTypeInfo)

                Dim success As Boolean = False
                If (Me.PropertySettingsTypes.ShowAllTypes = False) Then
                    For Each t As String In Me.PropertySettingsTypes.TypesFilter.Split(","c)
                        If (t = objType.PropertyTypeID.ToString()) Then
                            success = True
                        End If
                    Next
                Else
                    success = True
                End If

                If (success) Then
                    Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, (Me.IsEditable), Me.PropertySettingsTypes.PropertyAgentTabID, Me.PropertySettingsTypes.PropertyAgentModuleID, Me.ModuleKey)
                    objLayoutController.ProcessType(e.Item.Controls, Me._objLayoutTypes.Tokens, objType, Me.PropertySettingsTypes.AgentFilter)
                End If
                
            End If

        End Sub

#End Region

    End Class

End Namespace