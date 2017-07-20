Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules

Namespace Ventrian.PropertyAgent

    Public Class PropertySettingsLatest

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
                If (_settings.Contains(Constants.LATEST_MODULE_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_MODULE_ID_SETTING).ToString())
                Else
                    Return Constants.LATEST_MODULE_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyAgentTabID() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_TAB_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_TAB_ID_SETTING).ToString())
                Else
                    Return Constants.LATEST_TAB_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypeID() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_TYPE_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_TYPE_ID_SETTING).ToString())
                Else
                    Return Constants.LATEST_TYPE_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property Bubblefeatured() As Boolean
            Get
                If (_settings.Contains(Constants.LATEST_BUBBLE_FEATURED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LATEST_BUBBLE_FEATURED_SETTING).ToString())
                Else
                    Return Constants.LATEST_BUBBLE_FEATURED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedOnly() As Boolean
            Get
                If (_settings.Contains(Constants.LATEST_FEATURED_ONLY_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LATEST_FEATURED_ONLY_SETTING).ToString())
                Else
                    Return Constants.LATEST_FEATURED_ONLY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ShowRelated() As Boolean
            Get
                If (_settings.Contains(Constants.LATEST_SHOW_RELATED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LATEST_SHOW_RELATED_SETTING).ToString())
                Else
                    Return Constants.LATEST_SHOW_RELATED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RelatedCustomField() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_RELATED_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_RELATED_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return Constants.LATEST_RELATED_CUSTOM_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ShowShortList() As Boolean
            Get
                If (_settings.Contains(Constants.LATEST_SHOW_SHORTLIST_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LATEST_SHOW_SHORTLIST_SETTING).ToString())
                Else
                    Return Constants.LATEST_SHOW_SHORTLIST_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CustomFieldFilters() As String
            Get
                If (_settings.Contains(Constants.LATEST_CUSTOM_FIELD_FILTERS_SETTING)) Then
                    Return _settings(Constants.LATEST_CUSTOM_FIELD_FILTERS_SETTING).ToString()
                Else
                    Return Constants.LATEST_CUSTOM_FIELD_FILTERS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CustomFieldValues() As String
            Get
                If (_settings.Contains(Constants.LATEST_CUSTOM_FIELD_VALUES_SETTING)) Then
                    Return _settings(Constants.LATEST_CUSTOM_FIELD_VALUES_SETTING).ToString()
                Else
                    Return Constants.LATEST_CUSTOM_FIELD_VALUES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutMode() As LatestLayoutMode
            Get
                If (_settings.Contains(Constants.LATEST_LAYOUT_MODE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(LatestLayoutMode), _settings(Constants.LATEST_LAYOUT_MODE_SETTING).ToString()), LatestLayoutMode)
                Else
                    Return Constants.LATEST_LAYOUT_MODE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutType() As LatestLayoutType
            Get
                If (_settings.Contains(Constants.LATEST_LAYOUT_TYPE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(LatestLayoutType), _settings(Constants.LATEST_LAYOUT_TYPE_SETTING).ToString()), LatestLayoutType)
                Else
                    Return Constants.LATEST_LAYOUT_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UserFilter() As UserFilterType
            Get
                If (_settings.Contains(Constants.LATEST_USER_FILTER_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(UserFilterType), _settings(Constants.LATEST_USER_FILTER_SETTING).ToString()), UserFilterType)
                Else
                    Return Constants.LATEST_USER_FILTER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UserFilterSpecific() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_USER_FILTER_SPECIFIC_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_USER_FILTER_SPECIFIC_SETTING).ToString())
                Else
                    Return Constants.LATEST_USER_FILTER_SPECIFIC_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UserFilterParameter() As String
            Get
                If (_settings.Contains(Constants.LATEST_USER_FILTER_PARAMETER_SETTING)) Then
                    Return _settings(Constants.LATEST_USER_FILTER_PARAMETER_SETTING).ToString()
                Else
                    Return Constants.LATEST_USER_FILTER_PARAMETER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property IncludeStylesheet() As Boolean
            Get
                If (_settings.Contains(Constants.LATEST_LAYOUT_INCLUDE_STYLESHEET_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LATEST_LAYOUT_INCLUDE_STYLESHEET_SETTING).ToString())
                Else
                    Return Constants.LATEST_LAYOUT_INCLUDE_STYLESHEET_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutHeader() As String
            Get
                If (_settings.Contains(Constants.LATEST_LAYOUT_HEADER_SETTING)) Then
                    Return _settings(Constants.LATEST_LAYOUT_HEADER_SETTING).ToString()
                Else
                    Return Constants.LATEST_LAYOUT_HEADER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutItem() As String
            Get
                If (_settings.Contains(Constants.LATEST_LAYOUT_ITEM_SETTING)) Then
                    Return _settings(Constants.LATEST_LAYOUT_ITEM_SETTING).ToString()
                Else
                    Return Constants.LATEST_LAYOUT_ITEM_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutFooter() As String
            Get
                If (_settings.Contains(Constants.LATEST_LAYOUT_FOOTER_SETTING)) Then
                    Return _settings(Constants.LATEST_LAYOUT_FOOTER_SETTING).ToString()
                Else
                    Return Constants.LATEST_LAYOUT_FOOTER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LayoutEmpty() As String
            Get
                If (_settings.Contains(Constants.LATEST_LAYOUT_EMPTY_SETTING)) Then
                    Return _settings(Constants.LATEST_LAYOUT_EMPTY_SETTING).ToString()
                Else
                    Return Constants.LATEST_LAYOUT_EMPTY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MaxNumber() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_MAX_NUMBER_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_MAX_NUMBER_SETTING).ToString())
                Else
                    Return Constants.LATEST_MAX_NUMBER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PageSize() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_PAGE_SIZE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_PAGE_SIZE_SETTING).ToString())
                Else
                    Return Constants.LATEST_PAGE_SIZE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property EnablePager() As Boolean
            Get
                If (_settings.Contains(Constants.LATEST_ENABLE_PAGER_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LATEST_ENABLE_PAGER_SETTING).ToString())
                Else
                    Return Constants.LATEST_ENABLE_PAGER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property StartDate() As DateTime
            Get
                If (_settings.Contains(Constants.LATEST_START_DATE_SETTING)) Then
                    Return Convert.ToDateTime(_settings(Constants.LATEST_START_DATE_SETTING).ToString())
                Else
                    Return Null.NullDate
                End If
            End Get
        End Property

        Public ReadOnly Property MinAge() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_MIN_AGE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_MIN_AGE_SETTING).ToString())
                Else
                    Return Constants.LATEST_MIN_AGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MaxAge() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_MAX_AGE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_MAX_AGE_SETTING).ToString())
                Else
                    Return Constants.LATEST_MAX_AGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.LATEST_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SortBy() As SortByType
            Get
                If (_settings.Contains(Constants.LATEST_SORT_BY_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortByType), _settings(Constants.LATEST_SORT_BY_SETTING).ToString()), SortByType)
                Else
                    Return Constants.LATEST_SORT_BY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SortByCustomField() As Integer
            Get
                If (_settings.Contains(Constants.LATEST_SORT_BY_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LATEST_SORT_BY_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return Constants.LATEST_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SortDirection() As SortDirectionType
            Get
                If (_settings.Contains(Constants.LATEST_SORT_DIRECTION_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _settings(Constants.LATEST_SORT_DIRECTION_SETTING).ToString()), SortDirectionType)
                Else
                    Return Constants.LATEST_SORT_DIRECTION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UserSortable() As Boolean
            Get
                If (_settings.Contains(Constants.LATEST_USER_SORTABLE_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LATEST_USER_SORTABLE_SETTING).ToString())
                Else
                    Return Constants.LATEST_USER_SORTABLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UserSortableFields() As String
            Get
                If (_settings.Contains(Constants.LATEST_USER_SORTABLE_FIELDS_SETTING)) Then
                    Return _settings(Constants.LATEST_USER_SORTABLE_FIELDS_SETTING).ToString()
                Else
                    Return Constants.LATEST_USER_SORTABLE_FIELDS_SETTING_DEFAULT
                End If
            End Get
        End Property

#End Region

    End Class

End Namespace