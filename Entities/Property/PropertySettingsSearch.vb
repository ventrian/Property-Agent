Imports DotNetNuke.Entities.Modules

Namespace Ventrian.PropertyAgent

    Public Class PropertySettingsSearch

#Region " Private Members "

        Private _settings As Hashtable

#End Region

#Region " Constructors "

        Public Sub New(ByVal settings As Hashtable)
            _settings = settings
        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property PropertyAgentModuleID() As Integer
            Get
                If (_settings.Contains(Constants.SEARCH_MODULE_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SEARCH_MODULE_ID_SETTING).ToString())
                Else
                    Return Constants.SEARCH_MODULE_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyAgentTabID() As Integer
            Get
                If (_settings.Contains(Constants.SEARCH_TAB_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SEARCH_TAB_ID_SETTING).ToString())
                Else
                    Return Constants.SEARCH_TAB_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property Width() As Integer
            Get
                If (_settings.Contains(Constants.SEARCH_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SEARCH_WIDTH_SETTING).ToString())
                Else
                    Return Constants.SEARCH_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RadioButtonItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.SEARCH_RADIO_BUTTON_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SEARCH_RADIO_BUTTON_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.SEARCH_RADIO_BUTTON_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CheckBoxItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.SEARCH_CHECKBOX_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SEARCH_CHECKBOX_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.SEARCH_CHECKBOX_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CustomFields() As String
            Get
                If (_settings.Contains(Constants.SEARCH_CUSTOM_FIELDS)) Then
                    Return _settings(Constants.SEARCH_CUSTOM_FIELDS).ToString()
                Else
                    Return Constants.SEARCH_CUSTOM_FIELDS_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property HideHelpIcon() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_HIDE_HELP_ICON_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_HIDE_HELP_ICON_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_HIDE_HELP_ICON_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property HideZeroCount() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_HIDE_ZERO_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_HIDE_ZERO_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_HIDE_ZERO_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property HideTypeCount() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_HIDE_TYPE_COUNT_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_HIDE_TYPE_COUNT_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_HIDE_TYPE_COUNT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutMode() As SearchLayoutMode
            Get
                If (_settings.Contains(Constants.SEARCH_LAYOUT_MODE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SearchLayoutMode), _settings(Constants.SEARCH_LAYOUT_MODE_SETTING).ToString()), SearchLayoutMode)
                Else
                    Return Constants.SEARCH_LAYOUT_MODE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchTemplate() As String
            Get
                If (_settings.Contains(Constants.SEARCH_LAYOUT_TEMPLATE_SETTING)) Then
                    Return _settings(Constants.SEARCH_LAYOUT_TEMPLATE_SETTING).ToString()
                Else
                    Return Constants.SEARCH_LAYOUT_TEMPLATE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchWildcard() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_WILDCARD_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_WILDCARD_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_WILDCARD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchTypes() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_TYPES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_TYPES_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_TYPES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchLocation() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_LOCATION_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_LOCATION_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_LOCATION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchAgents() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_AGENTS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_AGENTS_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_AGENTS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchBrokers() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_BROKERS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SMALL_BROKERS_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SMALL_BROKERS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchStyle() As String
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_STYLE_SETTING)) Then
                    Return _settings(Constants.SEARCH_SMALL_STYLE_SETTING).ToString()
                Else
                    Return Constants.SEARCH_SMALL_STYLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SplitRange() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_SPLIT_RANGE_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_SPLIT_RANGE_SETTING).ToString())
                Else
                    Return Constants.SEARCH_SPLIT_RANGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SortBy() As SortByType
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_SORT_BY_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortByType), _settings(Constants.SEARCH_SMALL_SORT_BY_SETTING).ToString()), SortByType)
                Else
                    Return SortByType.Published
                End If
            End Get
        End Property

        Public ReadOnly Property SortByCustomField() As Integer
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_SORT_BY_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SEARCH_SMALL_SORT_BY_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return -1
                End If
            End Get
        End Property

        Public ReadOnly Property SortDirection() As SortDirectionType
            Get
                If (_settings.Contains(Constants.SEARCH_SMALL_SORT_DIRECTION_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _settings(Constants.SEARCH_SMALL_SORT_DIRECTION_SETTING).ToString()), SortDirectionType)
                Else
                    Return SortDirectionType.Descending
                End If
            End Get
        End Property

#End Region

    End Class

End Namespace