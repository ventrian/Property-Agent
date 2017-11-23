Imports System.Web.UI.WebControls

Imports DotNetNuke.Entities.Modules
Imports Ventrian.PropertyAgent.Mapping

Namespace Ventrian.PropertyAgent

    Public Class PropertySettings

#Region " Private Members "

        Private _settings As Hashtable

#End Region

#Region " Constructors "

        Public Sub New(ByVal settings As Hashtable)
            _settings = settings
        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property Template() As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_SETTING)) Then
                    Return _settings(Constants.TEMPLATE_SETTING).ToString()
                Else
                    Return Constants.TEMPLATE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property Template(ByVal moduleID As Integer) As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_SETTING)) Then
                    Return _settings(Constants.TEMPLATE_SETTING).ToString()
                Else
                    Dim objModuleController As New ModuleController()
                    Dim objSettings As Hashtable = objModuleController.GetModuleSettings(moduleID)

                    If (objSettings.Contains(Constants.TEMPLATE_OLD_SETTING)) Then
                        objModuleController.UpdateModuleSetting(moduleID, Constants.TEMPLATE_SETTING, objSettings(Constants.TEMPLATE_OLD_SETTING))
                        Return objSettings(Constants.TEMPLATE_OLD_SETTING)
                    Else
                        objModuleController.UpdateModuleSetting(moduleID, Constants.TEMPLATE_SETTING, Constants.TEMPLATE_SETTING_DEFAULT)
                    End If
                    Return Constants.TEMPLATE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateInfo() As TemplateInfo
            Get
                Dim objTemplateController As New TemplateController
                Return objTemplateController.GetByFolder(Template)
            End Get
        End Property

        Public ReadOnly Property LandingPageSections() As String
            Get
                If (_settings.Contains(Constants.LANDING_PAGE_SORT_ORDER_SETTING)) Then
                    Return _settings(Constants.LANDING_PAGE_SORT_ORDER_SETTING).ToString()
                Else
                    Return Constants.LANDING_PAGE_SORT_ORDER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MainLabel() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_MAIN_LABEL_SETTING)) Then
                    Return _settings(Constants.PROPERTY_MAIN_LABEL_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_MAIN_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyLabel() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_LABEL_SETTING)) Then
                    Return _settings(Constants.PROPERTY_LABEL_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyPluralLabel() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_PLURAL_LABEL_SETTING)) Then
                    Return _settings(Constants.PROPERTY_PLURAL_LABEL_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_PLURAL_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyTypeLabel() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_TYPE_LABEL_SETTING)) Then
                    Return _settings(Constants.PROPERTY_TYPE_LABEL_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_TYPE_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyTypePluralLabel() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_TYPE_PLURAL_LABEL_SETTING)) Then
                    Return _settings(Constants.PROPERTY_TYPE_PLURAL_LABEL_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_TYPE_PLURAL_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LocationLabel() As String
            Get
                If (_settings.Contains(Constants.LOCATION_LABEL_SETTING)) Then
                    Return _settings(Constants.LOCATION_LABEL_SETTING).ToString()
                Else
                    Return Constants.LOCATION_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property AgentLabel() As String
            Get
                If (_settings.Contains(Constants.AGENT_LABEL_SETTING)) Then
                    Return _settings(Constants.AGENT_LABEL_SETTING).ToString()
                Else
                    Return Constants.AGENT_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property AgentPluralLabel() As String
            Get
                If (_settings.Contains(Constants.AGENT_PLURAL_LABEL_SETTING)) Then
                    Return _settings(Constants.AGENT_PLURAL_LABEL_SETTING).ToString()
                Else
                    Return Constants.AGENT_PLURAL_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property BrokerLabel() As String
            Get
                If (_settings.Contains(Constants.BROKER_LABEL_SETTING)) Then
                    Return _settings(Constants.BROKER_LABEL_SETTING).ToString()
                Else
                    Return Constants.BROKER_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property BrokerPluralLabel() As String
            Get
                If (_settings.Contains(Constants.BROKER_PLURAL_LABEL_SETTING)) Then
                    Return _settings(Constants.BROKER_PLURAL_LABEL_SETTING).ToString()
                Else
                    Return Constants.BROKER_PLURAL_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ShortListLabel() As String
            Get
                If (_settings.Contains(Constants.SHORTLIST_LABEL_SETTING)) Then
                    Return _settings(Constants.SHORTLIST_LABEL_SETTING).ToString()
                Else
                    Return Constants.SHORTLIST_LABEL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LandingPageMode() As LandingPageType
            Get
                If (_settings.Contains(Constants.LANDING_PAGE_MODE_SETTING)) Then
                    Try
                        Return CType(System.Enum.Parse(GetType(LandingPageType), _settings(Constants.LANDING_PAGE_MODE_SETTING).ToString()), LandingPageType)
                    Catch
                        Return Constants.LANDING_PAGE_MODE_SETTING_DEFAULT
                    End Try
                Else
                    Return Constants.LANDING_PAGE_MODE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CoreSearchEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.CORE_SEARCH_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CORE_SEARCH_ENABLED_SETTING).ToString())
                Else
                    Return Constants.CORE_SEARCH_ENABLED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CoreSearchTitle() As String
            Get
                If (_settings.Contains(Constants.CORE_SEARCH_TITLE_SETTING)) Then
                    Return _settings(Constants.CORE_SEARCH_TITLE_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property CoreSearchDescription() As String
            Get
                If (_settings.Contains(Constants.CORE_SEARCH_DESCRIPTION_SETTING)) Then
                    Return _settings(Constants.CORE_SEARCH_DESCRIPTION_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateIncludeStylesheet() As Boolean
            Get
                If (_settings.Contains(Constants.TEMPLATE_INCLUDE_STYLESHEET_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TEMPLATE_INCLUDE_STYLESHEET_SETTING).ToString())
                Else
                    Return Constants.TEMPLATE_INCLUDE_STYLESHEET_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.FEATURED_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.FEATURED_ENABLED_SETTING).ToString())
                Else
                    Return Constants.FEATURED_ENABLED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedLayoutType() As LatestLayoutType
            Get
                If (_settings.Contains(Constants.FEATURED_LAYOUT_TYPE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(LatestLayoutType), _settings(Constants.FEATURED_LAYOUT_TYPE_SETTING).ToString()), LatestLayoutType)
                Else
                    Return Constants.FEATURED_LAYOUT_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedMaxNumber() As Integer
            Get
                If (_settings.Contains(Constants.FEATURED_MAX_NUMBER_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.FEATURED_MAX_NUMBER_SETTING).ToString())
                Else
                    Return Constants.FEATURED_MAX_NUMBER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.FEATURED_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.FEATURED_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.FEATURED_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedSortBy() As SortByType
            Get
                If (_settings.Contains(Constants.FEATURED_SORT_BY_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortByType), _settings(Constants.FEATURED_SORT_BY_SETTING).ToString()), SortByType)
                Else
                    Return Constants.FEATURED_SORT_BY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedSortByCustomField() As Integer
            Get
                If (_settings.Contains(Constants.FEATURED_SORT_BY_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.FEATURED_SORT_BY_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return Constants.FEATURED_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FeaturedSortDirection() As SortDirectionType
            Get
                If (_settings.Contains(Constants.FEATURED_SORT_DIRECTION_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _settings(Constants.FEATURED_SORT_DIRECTION_SETTING).ToString()), SortDirectionType)
                Else
                    Return Constants.FEATURED_SORT_DIRECTION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_ENABLED_SETTING).ToString())
                Else
                    Return Constants.SEARCH_ENABLED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchHideHelpIcon() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_HIDE_HELP_ICON_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_HIDE_HELP_ICON_SETTING).ToString())
                Else
                    Return Constants.SEARCH_HIDE_HELP_ICON_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchHideTypesCount() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_HIDE_TYPES_COUNT_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_HIDE_TYPES_COUNT_SETTING).ToString())
                Else
                    Return Constants.SEARCH_HIDE_TYPES_COUNT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchHideZeroTypes() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_HIDE_ZERO_TYPES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_HIDE_ZERO_TYPES_SETTING).ToString())
                Else
                    Return Constants.SEARCH_HIDE_ZERO_TYPES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchWildcard() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_WILDCARD_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_WILDCARD_SETTING).ToString())
                Else
                    Return Constants.SEARCH_WILDCARD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchTypes() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_TYPES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_TYPES_SETTING).ToString())
                Else
                    Return Constants.SEARCH_TYPES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchLocation() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_LOCATION_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_LOCATION_SETTING).ToString())
                Else
                    Return Constants.SEARCH_LOCATION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchAgents() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_AGENTS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_AGENTS_SETTING).ToString())
                Else
                    Return Constants.SEARCH_AGENTS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchBrokers() As Boolean
            Get
                If (_settings.Contains(Constants.SEARCH_BROKERS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEARCH_BROKERS_SETTING).ToString())
                Else
                    Return Constants.SEARCH_BROKERS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchWidth() As Integer
            Get
                If (_settings.Contains(Constants.SEARCH_AREA_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SEARCH_AREA_WIDTH_SETTING).ToString())
                Else
                    Return Constants.SEARCH_AREA_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SearchStyle() As String
            Get
                If (_settings.Contains(Constants.SEARCH_STYLE_SETTING)) Then
                    Return _settings(Constants.SEARCH_STYLE_SETTING).ToString()
                Else
                    Return Constants.SEARCH_STYLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.TYPES_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPES_ENABLED_SETTING).ToString())
                Else
                    Return Constants.TYPES_ENABLED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesLayoutType() As LatestLayoutType
            Get
                If (_settings.Contains(Constants.TYPES_LAYOUT_TYPE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(LatestLayoutType), _settings(Constants.TYPES_LAYOUT_TYPE_SETTING).ToString()), LatestLayoutType)
                Else
                    Return Constants.TYPES_LAYOUT_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesHideZero() As Boolean
            Get
                If (_settings.Contains(Constants.TYPES_HIDE_ZERO_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPES_HIDE_ZERO_SETTING).ToString())
                Else
                    Return Constants.TYPES_HIDE_ZERO_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesShowImages() As Boolean
            Get
                If (_settings.Contains(Constants.TYPES_SHOW_IMAGES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPES_SHOW_IMAGES_SETTING).ToString())
                Else
                    Return Constants.TYPES_SHOW_IMAGES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.TYPES_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.TYPES_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.TYPES_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypesRepeatDirection() As RepeatDirection
            Get
                If (_settings.Contains(Constants.TYPES_REPEAT_DIRECTION_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(RepeatDirection), _settings(Constants.TYPES_REPEAT_DIRECTION_SETTING).ToString()), RepeatDirection)
                Else
                    Return Constants.TYPES_REPEAT_DIRECTION_SETTING_DEFAULT
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

        Public ReadOnly Property ListingItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.LISTING_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LISTING_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.LISTING_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingItemsPerPage() As Integer
            Get
                If (_settings.Contains(Constants.LISTING_ITEMS_PER_PAGE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LISTING_ITEMS_PER_PAGE_SETTING).ToString())
                Else
                    Return Constants.LISTING_ITEMS_PER_PAGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingLayoutType() As LatestLayoutType
            Get
                If (_settings.Contains(Constants.LISTING_LAYOUT_TYPE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(LatestLayoutType), _settings(Constants.LISTING_LAYOUT_TYPE_SETTING).ToString()), LatestLayoutType)
                Else
                    Return Constants.LISTING_LAYOUT_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingBubbleFeatured() As Boolean
            Get
                If (_settings.Contains(Constants.LISTING_BUBBLE_FEATURED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LISTING_BUBBLE_FEATURED_SETTING).ToString())
                Else
                    Return Constants.LISTING_BUBBLE_FEATURED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSearchSubTypes() As Boolean
            Get
                If (_settings.Contains(Constants.LISTING_SEARCH_SUB_TYPES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LISTING_SEARCH_SUB_TYPES_SETTING).ToString())
                Else
                    Return Constants.LISTING_SEARCH_SUB_TYPES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingPassSearchValues() As Boolean
            Get
                If (_settings.Contains(Constants.LISTING_PASS_SEARCH_VALUES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LISTING_PASS_SEARCH_VALUES_SETTING).ToString())
                Else
                    Return Constants.LISTING_PASS_SEARCH_VALUES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortBy() As SortByType
            Get
                If (_settings.Contains(Constants.LISTING_SORT_BY_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortByType), _settings(Constants.LISTING_SORT_BY_SETTING).ToString()), SortByType)
                Else
                    Return Constants.LISTING_SORT_BY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortByCustomField() As Integer
            Get
                If (_settings.Contains(Constants.LISTING_SORT_BY_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LISTING_SORT_BY_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return Constants.LISTING_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortDirection() As SortDirectionType
            Get
                If (_settings.Contains(Constants.LISTING_SORT_DIRECTION_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _settings(Constants.LISTING_SORT_DIRECTION_SETTING).ToString()), SortDirectionType)
                Else
                    Return Constants.LISTING_SORT_DIRECTION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortBy2() As SortByTypeSecondary
            Get
                If (_settings.Contains(Constants.LISTING_SORT_BY_2_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortByTypeSecondary), _settings(Constants.LISTING_SORT_BY_2_SETTING).ToString()), SortByTypeSecondary)
                Else
                    Return Constants.LISTING_SORT_BY_2_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortByCustomField2() As Integer
            Get
                If (_settings.Contains(Constants.LISTING_SORT_BY_2_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LISTING_SORT_BY_2_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return Constants.LISTING_SORT_BY_2_CUSTOM_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortDirection2() As SortDirectionType
            Get
                If (_settings.Contains(Constants.LISTING_SORT_DIRECTION_2_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _settings(Constants.LISTING_SORT_DIRECTION_2_SETTING).ToString()), SortDirectionType)
                Else
                    Return Constants.LISTING_SORT_DIRECTION_2_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortBy3() As SortByTypeSecondary
            Get
                If (_settings.Contains(Constants.LISTING_SORT_BY_3_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortByTypeSecondary), _settings(Constants.LISTING_SORT_BY_3_SETTING).ToString()), SortByTypeSecondary)
                Else
                    Return Constants.LISTING_SORT_BY_3_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortByCustomField3() As Integer
            Get
                If (_settings.Contains(Constants.LISTING_SORT_BY_3_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LISTING_SORT_BY_3_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return Constants.LISTING_SORT_BY_3_CUSTOM_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortDirection3() As SortDirectionType
            Get
                If (_settings.Contains(Constants.LISTING_SORT_DIRECTION_3_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _settings(Constants.LISTING_SORT_DIRECTION_3_SETTING).ToString()), SortDirectionType)
                Else
                    Return Constants.LISTING_SORT_DIRECTION_3_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingUserSortable() As Boolean
            Get
                If (_settings.Contains(Constants.LISTING_USER_SORTABLE_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LISTING_USER_SORTABLE_SETTING).ToString())
                Else
                    Return Constants.LISTING_USER_SORTABLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ListingSortFields() As String
            Get
                If (_settings.Contains(Constants.LISTING_SORT_FIELDS_SETTING)) Then
                    Return _settings(Constants.LISTING_SORT_FIELDS_SETTING).ToString()
                Else
                    Dim vals As String = ""
                    For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                        Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)
                        If (vals = "") Then
                            vals = System.Enum.GetName(GetType(SortByType), value)
                        Else
                            vals = vals & "," & System.Enum.GetName(GetType(SortByType), value)
                        End If
                    Next
                    Return vals
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyManagerHideAuthorDetails() As Boolean
            Get
                If (_settings.Contains(Constants.PROPERTY_MANAGER_HIDE_AUTHOR_DETAILS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.PROPERTY_MANAGER_HIDE_AUTHOR_DETAILS_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_MANAGER_HIDE_AUTHOR_DETAILS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyManagerHidePublishingDetails() As Boolean
            Get
                If (_settings.Contains(Constants.PROPERTY_MANAGER_HIDE_PUBLISHING_DETAILS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.PROPERTY_MANAGER_HIDE_PUBLISHING_DETAILS_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_MANAGER_HIDE_PUBLISHING_DETAILS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyManagerItemsPerPage() As Integer
            Get
                If (_settings.Contains(Constants.PROPERTY_MANAGER_ITEMS_PER_PAGE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.PROPERTY_MANAGER_ITEMS_PER_PAGE_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_MANAGER_ITEMS_PER_PAGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyManagerSortBy() As SortByType
            Get
                If (_settings.Contains(Constants.PROPERTY_MANAGER_SORT_BY_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortByType), _settings(Constants.PROPERTY_MANAGER_SORT_BY_SETTING).ToString()), SortByType)
                Else
                    Return Constants.PROPERTY_MANAGER_SORT_BY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyManagerSortByCustomField() As Integer
            Get
                If (_settings.Contains(Constants.PROPERTY_MANAGER_SORT_BY_CUSTOM_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.PROPERTY_MANAGER_SORT_BY_CUSTOM_FIELD_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_MANAGER_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyManagerSortDirection() As SortDirectionType
            Get
                If (_settings.Contains(Constants.PROPERTY_MANAGER_SORT_DIRECTION_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _settings(Constants.PROPERTY_MANAGER_SORT_DIRECTION_SETTING).ToString()), SortDirectionType)
                Else
                    Return Constants.PROPERTY_MANAGER_SORT_DIRECTION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertySubject() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_SUBJECT_SETTING)) Then
                    Return _settings(Constants.PROPERTY_SUBJECT_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_SUBJECT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyTemplateHeader() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_TEMPLATE_HEADER_SETTING)) Then
                    Return _settings(Constants.PROPERTY_TEMPLATE_HEADER_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_TEMPLATE_HEADER_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyTemplateItem() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_TEMPLATE_ITEM_SETTING)) Then
                    Return _settings(Constants.PROPERTY_TEMPLATE_ITEM_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_TEMPLATE_ITEM_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyTemplateFooter() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_TEMPLATE_FOOTER_SETTING)) Then
                    Return _settings(Constants.PROPERTY_TEMPLATE_FOOTER_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_TEMPLATE_FOOTER_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationTo() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_TO_SETTING)) Then
                    Return _settings(Constants.PROPERTY_NOTIFICATION_TO_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_NOTIFICATION_TO_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationBCC() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_BCC_SETTING)) Then
                    Return _settings(Constants.PROPERTY_NOTIFICATION_BCC_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_NOTIFICATION_BCC_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationScheduleID() As Integer
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_SCHEDULER_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.PROPERTY_NOTIFICATION_SCHEDULER_ID_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_NOTIFICATION_SCHEDULER_ID_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationSchedulerEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_SCHEDULER_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.PROPERTY_NOTIFICATION_SCHEDULER_ENABLED_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_NOTIFICATION_SCHEDULER_ENABLED_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationSchedulerTimeLapse() As Integer
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationSchedulerTimeLapseMeasurement() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_MEASUREMENT_SETTING)) Then
                    Return _settings(Constants.PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_MEASUREMENT_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_MEASUREMENT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationSchedulerRetryFrequency() As Integer
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_SETTING).ToString())
                Else
                    Return Constants.PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PropertyNotificationSchedulerRetryFrequencyMeasurement() As String
            Get
                If (_settings.Contains(Constants.PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_SETTING)) Then
                    Return _settings(Constants.PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_SETTING).ToString()
                Else
                    Return Constants.PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderPeriod() As Integer
            Get
                If (_settings.Contains(Constants.REMINDER_PERIOD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.REMINDER_PERIOD_SETTING).ToString())
                Else
                    Return Constants.REMINDER_PERIOD_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderPeriodMeasurement() As String
            Get
                If (_settings.Contains(Constants.REMINDER_PERIOD_MEASUREMENT_SETTING)) Then
                    Return _settings(Constants.REMINDER_PERIOD_MEASUREMENT_SETTING).ToString()
                Else
                    Return Constants.REMINDER_PERIOD_MEASUREMENT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderFrequency() As Integer
            Get
                If (_settings.Contains(Constants.REMINDER_FREQUENCY_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.REMINDER_FREQUENCY_SETTING).ToString())
                Else
                    Return Constants.REMINDER_FREQUENCY_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderFrequencyMeasurement() As String
            Get
                If (_settings.Contains(Constants.REMINDER_FREQUENCY_MEASUREMENT_SETTING)) Then
                    Return _settings(Constants.REMINDER_FREQUENCY_MEASUREMENT_SETTING).ToString()
                Else
                    Return Constants.REMINDER_FREQUENCY_MEASUREMENT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderSubject() As String
            Get
                If (_settings.Contains(Constants.REMINDER_SUBJECT_SETTING)) Then
                    Return _settings(Constants.REMINDER_SUBJECT_SETTING).ToString()
                Else
                    Return Constants.REMINDER_SUBJECT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderTemplate() As String
            Get
                If (_settings.Contains(Constants.REMINDER_TEMPLATE_SETTING)) Then
                    Return _settings(Constants.REMINDER_TEMPLATE_SETTING).ToString()
                Else
                    Return Constants.REMINDER_TEMPLATE_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderBCC() As String
            Get
                If (_settings.Contains(Constants.REMINDER_BCC_SETTING)) Then
                    Return _settings(Constants.REMINDER_BCC_SETTING).ToString()
                Else
                    Return Constants.REMINDER_BCC_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderScheduleID() As Integer
            Get
                If (_settings.Contains(Constants.REMINDER_SCHEDULER_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.REMINDER_SCHEDULER_ID_SETTING).ToString())
                Else
                    Return Constants.REMINDER_SCHEDULER_ID_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderSchedulerEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.REMINDER_SCHEDULER_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.REMINDER_SCHEDULER_ENABLED_SETTING).ToString())
                Else
                    Return Constants.REMINDER_SCHEDULER_ENABLED_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderSchedulerTimeLapse() As Integer
            Get
                If (_settings.Contains(Constants.REMINDER_SCHEDULER_TIME_LAPSE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.REMINDER_SCHEDULER_TIME_LAPSE_SETTING).ToString())
                Else
                    Return Constants.REMINDER_SCHEDULER_TIME_LAPSE_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderSchedulerTimeLapseMeasurement() As String
            Get
                If (_settings.Contains(Constants.REMINDER_SCHEDULER_TIME_LAPSE_MEASUREMENT_SETTING)) Then
                    Return _settings(Constants.REMINDER_SCHEDULER_TIME_LAPSE_MEASUREMENT_SETTING).ToString()
                Else
                    Return Constants.REMINDER_SCHEDULER_TIME_LAPSE_MEASUREMENT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderSchedulerRetryFrequency() As Integer
            Get
                If (_settings.Contains(Constants.REMINDER_SCHEDULER_RETRY_FREQUENCY_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.REMINDER_SCHEDULER_RETRY_FREQUENCY_SETTING).ToString())
                Else
                    Return Constants.REMINDER_SCHEDULER_RETRY_FREQUENCY_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReminderSchedulerRetryFrequencyMeasurement() As String
            Get
                If (_settings.Contains(Constants.REMINDER_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_SETTING)) Then
                    Return _settings(Constants.REMINDER_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_SETTING).ToString()
                Else
                    Return Constants.REMINDER_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ImagesEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.IMAGES_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.IMAGES_ENABLED_SETTING).ToString())
                Else
                    Return Constants.IMAGES_ENABLED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property HighQuality() As Boolean
            Get
                If (_settings.Contains(Constants.IMAGES_HIGH_QUALITY_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.IMAGES_HIGH_QUALITY_SETTING).ToString())
                Else
                    Return Constants.IMAGES_HIGH_QUALITY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property IncludejQuery() As Boolean
            Get
                If (_settings.Contains(Constants.IMAGES_INCLUDE_JQUERY_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.IMAGES_INCLUDE_JQUERY_SETTING).ToString())
                Else
                    Return Constants.IMAGES_INCLUDE_JQUERY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property HideTypes() As Boolean
            Get
                If (_settings.Contains(Constants.HIDE_TYPES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.HIDE_TYPES_SETTING).ToString())
                Else
                    Return Constants.HIDE_TYPES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ProtectXSS() As Boolean
            Get
                If (_settings.Contains(Constants.PROTECT_XSS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.PROTECT_XSS_SETTING).ToString())
                Else
                    Return Constants.PROTECT_XSS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property AgentDropdownDefault() As Boolean
            Get
                If (_settings.Contains(Constants.AGENT_DROPDOWN_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.AGENT_DROPDOWN_SETTING).ToString())
                Else
                    Return Constants.AGENT_DROPDOWN_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ButtonClass() As String
            Get
                If (_settings.Contains(Constants.BUTTON_CLASS_SETTING)) Then
                    Return _settings(Constants.BUTTON_CLASS_SETTING).ToString()
                Else
                    Return Constants.BUTTON_CLASS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CachePropertyValues() As Boolean
            Get
                If (_settings.Contains(Constants.CACHE_PROPERTY_VALUES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CACHE_PROPERTY_VALUES_SETTING).ToString())
                Else
                    Return Constants.CACHE_PROPERTY_VALUES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TypeParams() As Boolean
            Get
                If (_settings.Contains(Constants.TYPE_PARAMS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.TYPE_PARAMS_SETTING).ToString())
                Else
                    Return Constants.TYPE_PARAMS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LockDownPropertyType() As Boolean
            Get
                If (_settings.Contains(Constants.LOCKDOWN_PROPERTYTYPE_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LOCKDOWN_PROPERTYTYPE_SETTING).ToString())
                Else
                    Return Constants.LOCKDOWN_PROPERTYTYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LockDownPropertyDates() As Boolean
            Get
                If (_settings.Contains(Constants.LOCKDOWN_PROPERTYDATES_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LOCKDOWN_PROPERTYDATES_SETTING).ToString())
                Else
                    Return Constants.LOCKDOWN_PROPERTYDATES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LockDownFeatured() As Boolean
            Get
                If (_settings.Contains(Constants.LOCKDOWN_FEATURED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.LOCKDOWN_FEATURED_SETTING).ToString())
                Else
                    Return Constants.LOCKDOWN_FEATURED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RedirectPage() As Integer
            Get
                If (_settings.Contains(Constants.REDIRECT_PAGE_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.REDIRECT_PAGE_SETTING).ToString())
                Else
                    Return Constants.REDIRECT_PAGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RedirectType() As RedirectType
            Get
                If (_settings.Contains(Constants.REDIRECT_TYPE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(RedirectType), _settings(Constants.REDIRECT_TYPE_SETTING).ToString()), RedirectType)
                Else
                    Return Constants.REDIRECT_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UploadMode() As UploadType
            Get
                If (_settings.Contains(Constants.UPLOAD_MODE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(UploadType), _settings(Constants.UPLOAD_MODE_SETTING).ToString()), UploadType)
                Else
                    Return Constants.UPLOAD_MODE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property UploadPlacement() As UploadPlacementType
            Get
                If (_settings.Contains(Constants.UPLOAD_PLACEMENT_SETTING)) Then
                    Try
                        Return CType(System.Enum.Parse(GetType(UploadPlacementType), _settings(Constants.UPLOAD_PLACEMENT_SETTING).ToString()), UploadPlacementType)
                    Catch
                        Return Constants.UPLOAD_PLACEMENT_SETTING_DEFAULT
                    End Try
                Else
                    Return Constants.UPLOAD_PLACEMENT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SmallWidth() As Integer
            Get
                If (_settings.Contains(Constants.SMALL_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SMALL_WIDTH_SETTING).ToString())
                Else
                    Return Constants.SMALL_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SmallHeight() As Integer
            Get
                If (_settings.Contains(Constants.SMALL_HEIGHT_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.SMALL_HEIGHT_SETTING).ToString())
                Else
                    Return Constants.SMALL_HEIGHT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MediumWidth() As Integer
            Get
                If (_settings.Contains(Constants.MEDIUM_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.MEDIUM_WIDTH_SETTING).ToString())
                Else
                    Return Constants.MEDIUM_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MediumHeight() As Integer
            Get
                If (_settings.Contains(Constants.MEDIUM_HEIGHT_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.MEDIUM_HEIGHT_SETTING).ToString())
                Else
                    Return Constants.MEDIUM_HEIGHT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LargeWidth() As Integer
            Get
                If (_settings.Contains(Constants.LARGE_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LARGE_WIDTH_SETTING).ToString())
                Else
                    Return Constants.LARGE_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property LargeHeight() As Integer
            Get
                If (_settings.Contains(Constants.LARGE_HEIGHT_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.LARGE_HEIGHT_SETTING).ToString())
                Else
                    Return Constants.LARGE_HEIGHT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.IMAGES_WATERMARK_ENABLED_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.IMAGES_WATERMARK_ENABLED_SETTING).ToString())
                Else
                    Return Constants.IMAGES_WATERMARK_ENABLED_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkText() As String
            Get
                If (_settings.Contains(Constants.IMAGES_WATERMARK_TEXT_SETTING)) Then
                    Return _settings(Constants.IMAGES_WATERMARK_TEXT_SETTING).ToString()
                Else
                    Return Constants.IMAGES_WATERMARK_TEXT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkImage() As String
            Get
                If (_settings.Contains(Constants.IMAGES_WATERMARK_IMAGE_SETTING)) Then
                    Return _settings(Constants.IMAGES_WATERMARK_IMAGE_SETTING).ToString()
                Else
                    Return Constants.IMAGES_WATERMARK_IMAGE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkPosition() As WatermarkPosition
            Get
                If (_settings.Contains(Constants.IMAGES_WATERMARK_IMAGE_POSITION_SETTING)) Then
                    Try
                        Return CType(System.Enum.Parse(GetType(WatermarkPosition), _settings(Constants.IMAGES_WATERMARK_IMAGE_POSITION_SETTING).ToString()), WatermarkPosition)
                    Catch
                        Return Constants.IMAGES_WATERMARK_IMAGE_POSITION_SETTING_DEFAULT
                    End Try
                Else
                    Return Constants.IMAGES_WATERMARK_IMAGE_POSITION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ImageCategories() As String
            Get
                If (_settings.Contains(Constants.IMAGES_CATEGORIES_SETTING)) Then
                    Return _settings(Constants.IMAGES_CATEGORIES_SETTING).ToString()
                Else
                    Return Constants.IMAGES_CATEGORIES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property BreadcrumbPlacement() As BreadcrumbType
            Get
                If (_settings.Contains(Constants.BREADCRUMB_PLACEMENT_SETTING)) Then
                    Try
                        Return CType(System.Enum.Parse(GetType(BreadcrumbType), _settings(Constants.BREADCRUMB_PLACEMENT_SETTING).ToString()), BreadcrumbType)
                    Catch
                        Return Constants.BREADCRUMB_PLACEMENT_SETTING_DEFAULT
                    End Try
                Else
                    Return Constants.BREADCRUMB_PLACEMENT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CustomFieldExpiration() As Integer
            Get
                If (_settings.Contains(Constants.CUSTOM_FIELD_EXPIRATION_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.CUSTOM_FIELD_EXPIRATION_SETTING).ToString())
                Else
                    Return Constants.CUSTOM_FIELD_EXPIRATION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property DefaultExpiration() As Integer
            Get
                If (_settings.Contains(Constants.DEFAULT_EXPIRATION_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.DEFAULT_EXPIRATION_SETTING).ToString())
                Else
                    Return Constants.DEFAULT_EXPIRATION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property DefaultExpirationPeriod() As String
            Get
                If (_settings.Contains(Constants.DEFAULT_EXPIRATION_PERIOD_SETTING)) Then
                    Return _settings(Constants.DEFAULT_EXPIRATION_PERIOD_SETTING).ToString()
                Else
                    Return Constants.DEFAULT_EXPIRATION_PERIOD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FieldWidth() As Integer
            Get
                If (_settings.Contains(Constants.FIELD_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.FIELD_WIDTH_SETTING).ToString())
                Else
                    Return Constants.FIELD_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ImagesItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.IMAGES_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.IMAGES_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.IMAGES_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CheckBoxItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.CHECKBOX_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.CHECKBOX_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.CHECKBOX_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RadioButtonItemsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.RADIO_BUTTON_ITEMS_PER_ROW_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.RADIO_BUTTON_ITEMS_PER_ROW_SETTING).ToString())
                Else
                    Return Constants.RADIO_BUTTON_ITEMS_PER_ROW_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CommentWidth() As String
            Get
                If (_settings.Contains(Constants.COMMENT_WIDTH_SETTING)) Then
                    Return _settings(Constants.COMMENT_WIDTH_SETTING).ToString()
                Else
                    Return Constants.COMMENT_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CommentUseCaptcha() As Boolean
            Get
                If (_settings.Contains(Constants.COMMENT_USE_CAPTCHA_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.COMMENT_USE_CAPTCHA_SETTING).ToString())
                Else
                    Return Constants.COMMENT_USE_CAPTCHA_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CommentNotifyOwner() As Boolean
            Get
                If (_settings.Contains(Constants.COMMENT_NOTIFY_OWNER_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.COMMENT_NOTIFY_OWNER_SETTING).ToString())
                Else
                    Return Constants.COMMENT_NOTIFY_OWNER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CommentNotifyEmail() As String
            Get
                If (_settings.Contains(Constants.COMMENT_NOTIFY_EMAIL_SETTING)) Then
                    Return _settings(Constants.COMMENT_NOTIFY_EMAIL_SETTING).ToString()
                Else
                    Return Constants.COMMENT_NOTIFY_EMAIL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MapEnable() As Boolean
            Get
                If (_settings.Contains(Constants.MAP_ENABLE_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.MAP_ENABLE_SETTING).ToString())
                Else
                    Return Constants.MAP_ENABLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MapKey() As String
            Get
                If (_settings.Contains(Constants.MAP_KEY_SETTING)) Then
                    Return _settings(Constants.MAP_KEY_SETTING).ToString()
                Else
                    Return Constants.MAP_KEY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MapHeight() As Integer
            Get
                If (_settings.Contains(Constants.MAP_HEIGHT_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.MAP_HEIGHT_SETTING).ToString())
                Else
                    Return Constants.MAP_HEIGHT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MapWidth() As Integer
            Get
                If (_settings.Contains(Constants.MAP_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.MAP_WIDTH_SETTING).ToString())
                Else
                    Return Constants.MAP_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MapZoom() As Integer
            Get
                If (_settings.Contains(Constants.MAP_ZOOM_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.MAP_ZOOM_SETTING).ToString())
                Else
                    Return Constants.MAP_ZOOM_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property DistanceExpression() As String
            Get
                If (_settings.Contains(Constants.MAP_DISTANCE_EXPRESSION_SETTING)) Then
                    Return _settings(Constants.MAP_DISTANCE_EXPRESSION_SETTING).ToString()
                Else
                    Return Constants.MAP_DISTANCE_EXPRESSION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property DistanceType() As DistanceType
            Get
                If (_settings.Contains(Constants.MAP_DISTANCE_TYPE_SETTING)) Then
                    Try
                        Return CType(System.Enum.Parse(GetType(DistanceType), _settings(Constants.MAP_DISTANCE_TYPE_SETTING).ToString()), DistanceType)
                    Catch
                        Return Constants.MAP_DISTANCE_TYPE_SETTING_DEFAULT
                    End Try
                Else
                    Return Constants.MAP_DISTANCE_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactDestination() As DestinationType
            Get
                If (_settings.Contains(Constants.CONTACT_DESTINATION_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(DestinationType), _settings(Constants.CONTACT_DESTINATION_SETTING).ToString()), DestinationType)
                Else
                    Return Constants.CONTACT_DESTINATION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactCustomEmail() As String
            Get
                If (_settings.Contains(Constants.CONTACT_CUSTOM_EMAIL_SETTING)) Then
                    Return _settings(Constants.CONTACT_CUSTOM_EMAIL_SETTING).ToString()
                Else
                    Return Constants.CONTACT_CUSTOM_EMAIL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactReplyTo() As ReplyToType
            Get
                If (_settings.Contains(Constants.CONTACT_REPLY_TO_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(ReplyToType), _settings(Constants.CONTACT_REPLY_TO_SETTING).ToString()), ReplyToType)
                Else
                    Return Constants.CONTACT_REPLY_TO_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactBCC() As String
            Get
                If (_settings.Contains(Constants.CONTACT_BCC_SETTING)) Then
                    Return _settings(Constants.CONTACT_BCC_SETTING).ToString()
                Else
                    Return Constants.CONTACT_BCC_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactHideEmail() As Boolean
            Get
                If (_settings.Contains(Constants.CONTACT_HIDE_EMAIL_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CONTACT_HIDE_EMAIL_SETTING).ToString())
                Else
                    Return Constants.CONTACT_HIDE_EMAIL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactHideName() As Boolean
            Get
                If (_settings.Contains(Constants.CONTACT_HIDE_NAME_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CONTACT_HIDE_NAME_SETTING).ToString())
                Else
                    Return Constants.CONTACT_HIDE_NAME_SETTING_DEFAULT
                End If
            End Get
        End Property


        Public ReadOnly Property ContactHidePhone() As Boolean
            Get
                If (_settings.Contains(Constants.CONTACT_HIDE_PHONE_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CONTACT_HIDE_PHONE_SETTING).ToString())
                Else
                    Return Constants.CONTACT_HIDE_PHONE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactRequireEmail() As Boolean
            Get
                If (_settings.Contains(Constants.CONTACT_REQUIRE_EMAIL_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CONTACT_REQUIRE_EMAIL_SETTING).ToString())
                Else
                    Return Constants.CONTACT_REQUIRE_EMAIL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactRequireName() As Boolean
            Get
                If (_settings.Contains(Constants.CONTACT_REQUIRE_NAME_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CONTACT_REQUIRE_NAME_SETTING).ToString())
                Else
                    Return Constants.CONTACT_REQUIRE_NAME_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactRequirePhone() As Boolean
            Get
                If (_settings.Contains(Constants.CONTACT_REQUIRE_PHONE_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CONTACT_REQUIRE_PHONE_SETTING).ToString())
                Else
                    Return Constants.CONTACT_REQUIRE_PHONE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactField() As Integer
            Get
                If (_settings.Contains(Constants.CONTACT_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.CONTACT_FIELD_SETTING).ToString())
                Else
                    Return Constants.CONTACT_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactCustomField() As Integer
            Get
                If (_settings.Contains(Constants.CONTACT_CUSTOM_FIELD_ID_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.CONTACT_CUSTOM_FIELD_ID_SETTING).ToString())
                Else
                    Return Constants.CONTACT_CUSTOM_FIELD_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactLogField() As Integer
            Get
                If (_settings.Contains(Constants.CONTACT_LOG_FIELD_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.CONTACT_LOG_FIELD_SETTING).ToString())
                Else
                    Return Constants.CONTACT_LOG_FIELD_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactMessageLines() As Integer
            Get
                If (_settings.Contains(Constants.CONTACT_MESSAGE_LINES_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.CONTACT_MESSAGE_LINES_SETTING).ToString())
                Else
                    Return Constants.CONTACT_MESSAGE_LINES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactUseCaptcha() As Boolean
            Get
                If (_settings.Contains(Constants.CONTACT_USE_CAPTCHA_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CONTACT_USE_CAPTCHA_SETTING).ToString())
                Else
                    Return Constants.CONTACT_USE_CAPTCHA_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ContactWidth() As String
            Get
                If (_settings.Contains(Constants.CONTACT_WIDTH_SETTING)) Then
                    Return _settings(Constants.CONTACT_WIDTH_SETTING).ToString()
                Else
                    Return Constants.CONTACT_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property EuroType() As EuroType
            Get
                If (_settings.Contains(Constants.EURO_TYPE_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(EuroType), _settings(Constants.EURO_TYPE_SETTING).ToString()), EuroType)
                Else
                    Return Constants.EURO_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property Currency() As CurrencyType
            Get
                If (_settings.Contains(Constants.CURRENCY_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(CurrencyType), _settings(Constants.CURRENCY_SETTING).ToString()), CurrencyType)
                Else
                    Return Constants.CURRENCY_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CurrencyShowAll() As Boolean
            Get
                If (_settings.Contains(Constants.CURRENCY_SHOW_ALL_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.CURRENCY_SHOW_ALL_SETTING).ToString())
                Else
                    Return Constants.CURRENCY_SHOW_ALL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CurrencyAvailable() As String
            Get
                If (_settings.Contains(Constants.CURRENCY_AVAILABLE_SETTING)) Then
                    Return _settings(Constants.CURRENCY_AVAILABLE_SETTING).ToString()
                Else
                    Return Constants.CURRENCY_AVAILABLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property CurrencyDecimalPlaces() As Integer
            Get
                If (_settings.Contains(Constants.CURRENCY_DECIMAL_PLACES_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.CURRENCY_DECIMAL_PLACES_SETTING).ToString())
                Else
                    Return Constants.CURRENCY_DECIMAL_PLACES_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FriendBCC() As String
            Get
                If (_settings.Contains(Constants.FRIEND_BCC_SETTING)) Then
                    Return _settings(Constants.FRIEND_BCC_SETTING).ToString()
                Else
                    Return Constants.FRIEND_BCC_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FriendWidth() As Integer
            Get
                If (_settings.Contains(Constants.FRIEND_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.FRIEND_WIDTH_SETTING).ToString())
                Else
                    Return Constants.FRIEND_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property NotificationNotifyApprovers() As Boolean
            Get
                If (_settings.Contains(Constants.NOTIFICATION_NOTIFY_APPROVERS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.NOTIFICATION_NOTIFY_APPROVERS_SETTING).ToString())
                Else
                    Return Constants.NOTIFICATION_NOTIFY_APPROVERS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property NotificationEmail() As String
            Get
                If (_settings.Contains(Constants.NOTIFICATION_EMAIL_SETTING)) Then
                    Return _settings(Constants.NOTIFICATION_EMAIL_SETTING).ToString()
                Else
                    Return Constants.NOTIFICATION_EMAIL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property NotificationNotifyBroker() As Boolean
            Get
                If (_settings.Contains(Constants.NOTIFICATION_NOTIFY_BROKER_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.NOTIFICATION_NOTIFY_BROKER_SETTING).ToString())
                Else
                    Return Constants.NOTIFICATION_NOTIFY_BROKER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property NotificationNotifyOwner() As Boolean
            Get
                If (_settings.Contains(Constants.NOTIFICATION_NOTIFY_OWNER_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.NOTIFICATION_NOTIFY_OWNER_SETTING).ToString())
                Else
                    Return Constants.NOTIFICATION_NOTIFY_OWNER_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReviewWidth() As Integer
            Get
                If (_settings.Contains(Constants.REVIEW_WIDTH_SETTING)) Then
                    Return Convert.ToInt32(_settings(Constants.REVIEW_WIDTH_SETTING).ToString())
                Else
                    Return Constants.REVIEW_WIDTH_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReviewEmail() As String
            Get
                If (_settings.Contains(Constants.REVIEW_EMAIL_SETTING)) Then
                    Return _settings(Constants.REVIEW_EMAIL_SETTING).ToString()
                Else
                    Return Constants.REVIEW_EMAIL_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReviewModeration() As Boolean
            Get
                If (_settings.Contains(Constants.REVIEW_MODERATION_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.REVIEW_MODERATION_SETTING).ToString())
                Else
                    Return Constants.REVIEW_MODERATION_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ReviewAnonymous() As Boolean
            Get
                If (_settings.Contains(Constants.REVIEW_ANONYMOUS_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.REVIEW_ANONYMOUS_SETTING).ToString())
                Else
                    Return Constants.REVIEW_ANONYMOUS_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RssEnable() As Boolean
            Get
                If (_settings.Contains(Constants.RSS_ENABLE)) Then
                    Return Convert.ToBoolean(_settings(Constants.RSS_ENABLE).ToString())
                Else
                    Return Constants.RSS_ENABLE_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RssMaxRecords() As Integer
            Get
                If (_settings.Contains(Constants.RSS_MAX_RECORDS)) Then
                    Return Convert.ToInt32(_settings(Constants.RSS_MAX_RECORDS).ToString())
                Else
                    Return Constants.RSS_MAX_RECORDS_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RssTitleLatest(ByVal replaceTokens As Boolean) As String
            Get
                Dim title As String = ""
                If (_settings.Contains(Constants.RSS_TITLE_LATEST)) Then
                    title = _settings(Constants.RSS_TITLE_LATEST).ToString()
                Else
                    title = Constants.RSS_TITLE_LATEST_DEFAULT
                End If

                If (replaceTokens) Then
                    title = title.Replace("[PROPERTYLABEL]", PropertyLabel)
                    title = title.Replace("[PROPERTYPLURALLABEL]", PropertyPluralLabel)
                    title = title.Replace("[PROPERTYTYPELABEL]", PropertyTypeLabel)
                    title = title.Replace("[PROPERTYTYPEPLURALLABEL]", PropertyTypePluralLabel)
                End If

                Return title
            End Get
        End Property

        Public ReadOnly Property RssTitleType(ByVal type As String, ByVal replaceTokens As Boolean) As String
            Get
                Dim title As String = ""
                If (_settings.Contains(Constants.RSS_TITLE_TYPE)) Then
                    title = _settings(Constants.RSS_TITLE_TYPE).ToString()
                Else
                    title = Constants.RSS_TITLE_TYPE_DEFAULT
                End If

                If (replaceTokens) Then
                    title = title.Replace("[PROPERTYLABEL]", PropertyLabel)
                    title = title.Replace("[PROPERTYPLURALLABEL]", PropertyPluralLabel)
                    title = title.Replace("[PROPERTYTYPELABEL]", PropertyTypeLabel)
                    title = title.Replace("[PROPERTYTYPEPLURALLABEL]", PropertyTypePluralLabel)
                    title = title.Replace("[TYPE]", type)
                End If

                Return title
            End Get
        End Property

        Public ReadOnly Property RssTitleSearchResult(ByVal replaceTokens As Boolean) As String
            Get
                Dim title As String = ""
                If (_settings.Contains(Constants.RSS_TITLE_SEARCH_RESULT)) Then
                    title = _settings(Constants.RSS_TITLE_SEARCH_RESULT).ToString()
                Else
                    title = Constants.RSS_TITLE_SEARCH_RESULT_DEFAULT
                End If

                If (replaceTokens) Then
                    title = title.Replace("[PROPERTYLABEL]", PropertyLabel)
                    title = title.Replace("[PROPERTYPLURALLABEL]", PropertyPluralLabel)
                    title = title.Replace("[PROPERTYTYPELABEL]", PropertyTypeLabel)
                    title = title.Replace("[PROPERTYTYPEPLURALLABEL]", PropertyTypePluralLabel)
                End If

                Return title
            End Get
        End Property

        Public ReadOnly Property XmlEnable() As Boolean
            Get
                If (_settings.Contains(Constants.XML_ENABLE)) Then
                    Return Convert.ToBoolean(_settings(Constants.XML_ENABLE).ToString())
                Else
                    Return Constants.XML_ENABLE_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property XmlMaxRecords() As Integer
            Get
                If (_settings.Contains(Constants.XML_MAX_RECORDS)) Then
                    Return Convert.ToInt32(_settings(Constants.XML_MAX_RECORDS).ToString())
                Else
                    Return Constants.XML_MAX_RECORDS_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEOAgentType() As String
            Get
                If (_settings.Contains(Constants.SEO_AGENT_TYPE_SETTING)) Then
                    Return _settings(Constants.SEO_AGENT_TYPE_SETTING).ToString()
                Else
                    Return Constants.SEO_AGENT_TYPE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEORedirect() As Boolean
            Get
                If (_settings.Contains(Constants.SEO_REDIRECT_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEO_REDIRECT_SETTING).ToString())
                Else
                    Return Constants.SEO_REDIRECT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEOPropertyID() As String
            Get
                If (_settings.Contains(Constants.SEO_PROPERTY_ID_SETTING)) Then
                    Return _settings(Constants.SEO_PROPERTY_ID_SETTING).ToString()
                Else
                    Return Constants.SEO_PROPERTY_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEOViewPropertyTitle() As String
            Get
                If (_settings.Contains(Constants.SEO_VIEW_PROPERTY_TITLE_SETTING)) Then
                    Return _settings(Constants.SEO_VIEW_PROPERTY_TITLE_SETTING).ToString()
                Else
                    Return Constants.SEO_VIEW_PROPERTY_TITLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEOViewTypeTitle() As String
            Get
                If (_settings.Contains(Constants.SEO_VIEW_TYPE_TITLE_SETTING)) Then
                    Return _settings(Constants.SEO_VIEW_TYPE_TITLE_SETTING).ToString()
                Else
                    Return Constants.SEO_VIEW_TYPE_TITLE_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEOPropertyTypeID() As String
            Get
                If (_settings.Contains(Constants.SEO_PROPERTY_TYPE_ID_SETTING)) Then
                    Return _settings(Constants.SEO_PROPERTY_TYPE_ID_SETTING).ToString()
                Else
                    Return Constants.SEO_PROPERTY_TYPE_ID_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEOTitleReplacement() As TitleReplacementType
            Get
                If (_settings.Contains(Constants.SEO_TITLE_REPLACEMENT_SETTING)) Then
                    Return CType(System.Enum.Parse(GetType(TitleReplacementType), _settings(Constants.SEO_TITLE_REPLACEMENT_SETTING).ToString()), TitleReplacementType)
                Else
                    Return Constants.SEO_TITLE_REPLACEMENT_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SEOCanonicalLink() As Boolean
            Get
                If (_settings.Contains(Constants.SEO_CANONICAL_LINK_SETTING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SEO_CANONICAL_LINK_SETTING).ToString())
                Else
                    Return Constants.SEO_CANONICAL_LINK_SETTING_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionViewDetail() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_VIEW_DETAIL_SETTING)) Then
                    Return _settings(Constants.PERMISSION_VIEW_DETAIL_SETTING).ToString()
                Else
                    Return DotNetNuke.Common.Globals.glbRoleAllUsersName & ";" & DotNetNuke.Common.Globals.glbRoleUnauthUserName
                End If
            End Get
        End Property

        Public ReadOnly Property IsPermissionViewDetailSet() As Boolean
            Get
                Return _settings.Contains(Constants.PERMISSION_VIEW_DETAIL_SETTING)
            End Get
        End Property

        Public ReadOnly Property PermissionSubmit() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_SUBMIT_SETTING)) Then
                    Return _settings(Constants.PERMISSION_SUBMIT_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionBroker() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_BROKER_SETTING)) Then
                    Return _settings(Constants.PERMISSION_BROKER_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAddImages() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADD_IMAGES_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADD_IMAGES_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAddImagesLimit() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADD_IMAGES_LIMIT_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADD_IMAGES_LIMIT_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionApprove() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_APPROVE_SETTING)) Then
                    Return _settings(Constants.PERMISSION_APPROVE_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAutoApprove() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_AUTO_APPROVE_SETTING)) Then
                    Return _settings(Constants.PERMISSION_AUTO_APPROVE_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionDelete() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_DELETE_SETTING)) Then
                    Return _settings(Constants.PERMISSION_DELETE_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionFeature() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_FEATURE_SETTING)) Then
                    Return _settings(Constants.PERMISSION_FEATURE_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAutoFeature() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_AUTO_FEATURE_SETTING)) Then
                    Return _settings(Constants.PERMISSION_AUTO_FEATURE_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionExport() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_EXPORT_SETTING)) Then
                    Return _settings(Constants.PERMISSION_EXPORT_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionPublishDetail() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_PUBLISH_DETAIL_SETTING)) Then
                    Return _settings(Constants.PERMISSION_PUBLISH_DETAIL_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionLockDown() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_LOCKDOWN_SETTING)) Then
                    Return _settings(Constants.PERMISSION_LOCKDOWN_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionLimit() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_LIMIT_SETTING)) Then
                    Return _settings(Constants.PERMISSION_LIMIT_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionDetailUrl() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_DETAIL_URL_SETTING)) Then
                    Return _settings(Constants.PERMISSION_DETAIL_URL_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAdminCustomField() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADMIN_CUSTOM_FIELD_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADMIN_CUSTOM_FIELD_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAdminReviewField() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADMIN_REVIEW_FIELD_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADMIN_REVIEW_FIELD_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAdminEmailFiles() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADMIN_EMAIL_FILES_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADMIN_EMAIL_FILES_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAdminLayoutFiles() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADMIN_LAYOUT_FILES_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADMIN_LAYOUT_FILES_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAdminLayoutSettings() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADMIN_LAYOUT_SETTINGS_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADMIN_LAYOUT_SETTINGS_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property PermissionAdminTypes() As String
            Get
                If (_settings.Contains(Constants.PERMISSION_ADMIN_TYPES_SETTING)) Then
                    Return _settings(Constants.PERMISSION_ADMIN_TYPES_SETTING).ToString()
                Else
                    Return ""
                End If
            End Get
        End Property

#End Region

    End Class

End Namespace