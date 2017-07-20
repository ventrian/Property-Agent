Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security

Namespace Ventrian.PropertyAgent

    Public Class PropertyAgentTypesBase
        Inherits PortalModuleBase

#Region " Private Members "

        Private _propertySettings As PropertySettings
        Private _propertySettingsTypes As PropertySettingsTypes
        Private _customFields As List(Of CustomFieldInfo)

#End Region

#Region " Public Properties "

        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Protected ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Public ReadOnly Property CustomFields() As List(Of CustomFieldInfo)
            Get
                If (_customFields Is Nothing) Then
                    Dim objCustomFieldController As New CustomFieldController
                    _customFields = objCustomFieldController.List(Me.PropertySettingsTypes.PropertyAgentModuleID, True)
                End If

                Return _customFields
            End Get
        End Property

        Public ReadOnly Property PropertySettings() As PropertySettings
            Get
                If (_propertySettings Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettings = New PropertySettings(objModuleController.GetModuleSettings(Me.PropertySettingsTypes.PropertyAgentModuleID))
                End If
                Return _propertySettings
            End Get
        End Property

        Public ReadOnly Property PropertySettingsTypes() As PropertySettingsTypes
            Get
                If (_propertySettingsTypes Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettingsTypes = New PropertySettingsTypes(Me.Settings)
                End If
                Return _propertySettingsTypes
            End Get
        End Property

        Public ReadOnly Property ModuleKey() As String
            Get
                Return "PropertyAgentTypes-" & Me.TabModuleId
            End Get
        End Property

#End Region

    End Class

End Namespace
