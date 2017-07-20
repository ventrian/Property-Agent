Imports System.ComponentModel

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Public Class PropertyAgentSearchBase
        Inherits PortalModuleBase

#Region " Private Members "

        Private _propertySettings As PropertySettings
        Private _propertySettingsSearch As PropertySettingsSearch
        Private _customFields As List(Of CustomFieldInfo)

#End Region

#Region " Protected Methods "

        Protected Function GetResourceString(ByVal key As String) As String

            Return GetResourceString(key, Me.LocalResourceFile, PropertySettings)

        End Function

        Protected Function GetResourceString(ByVal key As String, ByVal resourceFile As String, ByVal propertySettings As PropertySettings) As String

            Return PropertyUtil.FormatPropertyLabel(Localization.GetString(key, resourceFile), propertySettings)

        End Function

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
                    _customFields = objCustomFieldController.List(Me.PropertySettingsSearch.PropertyAgentModuleID, True)
                End If

                Return _customFields
            End Get
        End Property

        Public ReadOnly Property PropertySettings() As PropertySettings
            Get
                If (_propertySettings Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettings = New PropertySettings(objModuleController.GetModuleSettings(Me.PropertySettingsSearch.PropertyAgentModuleID))
                End If
                Return _propertySettings
            End Get
        End Property

        Public ReadOnly Property PropertySettingsSearch() As PropertySettingsSearch
            Get
                If (_propertySettingsSearch Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettingsSearch = New PropertySettingsSearch(Me.Settings)
                End If
                Return _propertySettingsSearch
            End Get
        End Property

        Public ReadOnly Property ModuleKey() As String
            Get
                Return "PropertyAgentLatest-" & Me.TabModuleId
            End Get
        End Property

#End Region

    End Class

End Namespace
