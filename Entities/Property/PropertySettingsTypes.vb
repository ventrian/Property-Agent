Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules

Namespace Ventrian.PropertyAgent

    Public Class PropertySettingsTypes

#Region " Private Members "

        Private _settings As Hashtable

#End Region

#Region " Constructors "

        Public Sub New(ByVal settings As Hashtable)
            _settings = settings
        End Sub

#End Region

#Region " Public Methods "

        Public Sub SetSettings(ByVal settings As Hashtable)
            _settings = settings
        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property PropertyAgentModuleID() As Integer
            Get
                If (_settings.Contains(Constants.TYPES_MODULE_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.TYPES_MODULE_ID_SETTING).ToString())
                Else
                    Return Constants.TYPES_MODULE_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyAgentTabID() As Integer
            Get
                If (_settings.Contains(Constants.TYPES_TAB_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.TYPES_TAB_ID_SETTING).ToString())
                Else
                    Return Constants.TYPES_TAB_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property AgentFilter() As String
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_AGENT_FILTER_SETTING)) Then
                    Return _settings(Constants.TYPES_LAYOUT_AGENT_FILTER_SETTING).ToString()
                Else
                    Return Constants.TYPES_LAYOUT_AGENT_FILTER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutMode() As LatestLayoutMode
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_MODE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(LatestLayoutMode), _settings(Constants.TYPES_LAYOUT_MODE_SETTING).ToString()), LatestLayoutMode)
                Else
                    Return Constants.TYPES_LAYOUT_MODE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property HideZeroCount() As Boolean
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_HIDE_ZERO_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPES_LAYOUT_HIDE_ZERO_SETTING).ToString())
                Else
                    Return Constants.TYPES_LAYOUT_HIDE_ZERO_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ShowTopLevelOnly() As Boolean
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_SHOW_TOP_LEVEL_ONLY_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPES_LAYOUT_SHOW_TOP_LEVEL_ONLY_SETTING).ToString())
                Else
                    Return Constants.TYPES_LAYOUT_SHOW_TOP_LEVEL_ONLY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ShowAllTypes() As Boolean
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_SHOW_ALL_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPES_LAYOUT_SHOW_ALL_SETTING).ToString())
                Else
                    Return Constants.TYPES_LAYOUT_SHOW_ALL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesFilter() As String
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_FILTER_SETTING)) Then
                    Return _settings(Constants.TYPES_LAYOUT_FILTER_SETTING).ToString()
                Else
                    Return Constants.TYPES_LAYOUT_FILTER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property IncludeStylesheet() As Boolean
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_INCLUDE_STYLESHEET_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPES_LAYOUT_INCLUDE_STYLESHEET_SETTING).ToString())
                Else
                    Return Constants.TYPES_LAYOUT_INCLUDE_STYLESHEET_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutHeader() As String
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_HEADER_SETTING)) Then
                    Return _settings(Constants.TYPES_LAYOUT_HEADER_SETTING).ToString()
                Else
                    Return Constants.TYPES_LAYOUT_HEADER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutItem() As String
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_ITEM_SETTING)) Then
                    Return _settings(Constants.TYPES_LAYOUT_ITEM_SETTING).ToString()
                Else
                    Return Constants.TYPES_LAYOUT_ITEM_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutFooter() As String
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_FOOTER_SETTING)) Then
                    Return _settings(Constants.TYPES_LAYOUT_FOOTER_SETTING).ToString()
                Else
                    Return Constants.TYPES_LAYOUT_FOOTER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesSortBy() As PropertyTypeSortByType
            Get
                If (_settings.Contains(Constants.TYPES_SORT_BY_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(PropertyTypeSortByType), _settings(Constants.TYPES_SORT_BY_SETTING).ToString()), PropertyTypeSortByType)
                Else
                    Return Constants.TYPES_SORT_BY_SETTING_DEFAULT
                End If
            End Get
        End Property

#End Region

    End Class

End Namespace