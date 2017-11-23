Imports Ventrian.PropertyAgent.Mapping

Namespace Ventrian.PropertyAgent

    Public Class Constants

#Region " Constants "

        Public Const JAVASCRIPT_VERSION As String = "010700"

        Public Const TEMPLATE_SETTING As String = "PropertyAgentTemplate"
        Public Const TEMPLATE_OLD_SETTING As String = "Template"
        Public Const TEMPLATE_SETTING_DEFAULT As String = ""

        Public Const LANDING_PAGE_SORT_ORDER_SETTING As String = "LandingPageSortOrder"
        Public Const LANDING_PAGE_SORT_ORDER_SETTING_DEFAULT As String = "Featured,Search,Types"

        Public Const PROPERTY_MAIN_LABEL_SETTING As String = "MainLabel"
        Public Const PROPERTY_MAIN_LABEL_SETTING_DEFAULT As String = "Main"
        Public Const PROPERTY_LABEL_SETTING As String = "PropertyLabel"
        Public Const PROPERTY_LABEL_SETTING_DEFAULT As String = "Property"
        Public Const PROPERTY_PLURAL_LABEL_SETTING As String = "PropertyPluralLabel"
        Public Const PROPERTY_PLURAL_LABEL_SETTING_DEFAULT As String = "Properties"
        Public Const PROPERTY_TYPE_LABEL_SETTING As String = "PropertyTypeLabel"
        Public Const PROPERTY_TYPE_LABEL_SETTING_DEFAULT As String = "Property Type"
        Public Const PROPERTY_TYPE_PLURAL_LABEL_SETTING As String = "PropertyTypePluralLabel"
        Public Const PROPERTY_TYPE_PLURAL_LABEL_SETTING_DEFAULT As String = "Property Types"
        Public Const LOCATION_LABEL_SETTING As String = "LocationLabel"
        Public Const LOCATION_LABEL_SETTING_DEFAULT As String = "Location"
        Public Const AGENT_LABEL_SETTING As String = "AgentLabel"
        Public Const AGENT_LABEL_SETTING_DEFAULT As String = "Agent"
        Public Const AGENT_PLURAL_LABEL_SETTING As String = "AgentPluralLabel"
        Public Const AGENT_PLURAL_LABEL_SETTING_DEFAULT As String = "Agents"
        Public Const BROKER_LABEL_SETTING As String = "BrokerLabel"
        Public Const BROKER_LABEL_SETTING_DEFAULT As String = "Broker"
        Public Const BROKER_PLURAL_LABEL_SETTING As String = "BrokerPluralLabel"
        Public Const BROKER_PLURAL_LABEL_SETTING_DEFAULT As String = "Brokers"
        Public Const SHORTLIST_LABEL_SETTING As String = "ShortListLabel"
        Public Const SHORTLIST_LABEL_SETTING_DEFAULT As String = "Shortlist"

        Public Const LANDING_PAGE_MODE_SETTING As String = "LandingPageMode"
        Public Const LANDING_PAGE_MODE_SETTING_DEFAULT As LandingPageType = LandingPageType.Standard

        Public Const FEATURED_ENABLED_SETTING As String = "FeaturedEnabled"
        Public Const FEATURED_ENABLED_SETTING_DEFAULT As Boolean = True
        Public Const FEATURED_LAYOUT_TYPE_SETTING As String = "FeaturedLayoutType"
        Public Const FEATURED_LAYOUT_TYPE_SETTING_DEFAULT As LatestLayoutType = LatestLayoutType.TableLayout
        Public Const FEATURED_MAX_NUMBER_SETTING As String = "FeaturedMaxNumber"
        Public Const FEATURED_MAX_NUMBER_SETTING_DEFAULT As Integer = 3
        Public Const FEATURED_ITEMS_PER_ROW_SETTING As String = "FeaturedItemsPerRow"
        Public Const FEATURED_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 3
        Public Const FEATURED_SORT_BY_SETTING As String = "FeaturedSortBy"
        Public Const FEATURED_SORT_BY_SETTING_DEFAULT As SortByType = SortByType.Published
        Public Const FEATURED_SORT_BY_CUSTOM_FIELD_SETTING As String = "FeaturedSortByCustomField"
        Public Const FEATURED_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const FEATURED_SORT_DIRECTION_SETTING As String = "FeaturedSortDirection"
        Public Const FEATURED_SORT_DIRECTION_SETTING_DEFAULT As SortDirectionType = SortDirectionType.Descending

        Public Const SEARCH_ENABLED_SETTING As String = "SearchEnabled"
        Public Const SEARCH_ENABLED_SETTING_DEFAULT As Boolean = True
        Public Const SEARCH_HIDE_HELP_ICON_SETTING As String = "SearchHideHelpIcon"
        Public Const SEARCH_HIDE_HELP_ICON_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_HIDE_TYPES_COUNT_SETTING As String = "SearchHideTypesCount"
        Public Const SEARCH_HIDE_TYPES_COUNT_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_HIDE_ZERO_TYPES_SETTING As String = "SearchHideZeroTypes"
        Public Const SEARCH_HIDE_ZERO_TYPES_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_WILDCARD_SETTING As String = "SearchWildcard"
        Public Const SEARCH_WILDCARD_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_TYPES_SETTING As String = "SearchTypes"
        Public Const SEARCH_TYPES_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_LOCATION_SETTING As String = "SearchLocation"
        Public Const SEARCH_LOCATION_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_AGENTS_SETTING As String = "SearchAgents"
        Public Const SEARCH_AGENTS_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_BROKERS_SETTING As String = "SearchBrokers"
        Public Const SEARCH_BROKERS_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_AREA_WIDTH_SETTING As String = "SearchWidth"
        Public Const SEARCH_AREA_WIDTH_SETTING_DEFAULT As Integer = 300
        Public Const SEARCH_STYLE_SETTING As String = "SearchStyle"
        Public Const SEARCH_STYLE_SETTING_DEFAULT As String = "CommandButton"

        Public Const TYPES_ENABLED_SETTING As String = "TypesEnabled"
        Public Const TYPES_ENABLED_SETTING_DEFAULT As Boolean = True
        Public Const TYPES_LAYOUT_TYPE_SETTING As String = "TypesLayoutType"
        Public Const TYPES_LAYOUT_TYPE_SETTING_DEFAULT As LatestLayoutType = LatestLayoutType.TableLayout
        Public Const TYPES_HIDE_ZERO_SETTING As String = "TypesHideZero"
        Public Const TYPES_HIDE_ZERO_SETTING_DEFAULT As Boolean = False
        Public Const TYPES_SHOW_IMAGES_SETTING As String = "TypesShowImages"
        Public Const TYPES_SHOW_IMAGES_SETTING_DEFAULT As Boolean = False
        Public Const TYPES_ITEMS_PER_ROW_SETTING As String = "TypesItemsPerRow"
        Public Const TYPES_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 3
        Public Const TYPES_REPEAT_DIRECTION_SETTING As String = "TypesRepeatDirection"
        Public Const TYPES_REPEAT_DIRECTION_SETTING_DEFAULT As System.Web.UI.WebControls.RepeatDirection = Web.UI.WebControls.RepeatDirection.Horizontal
        Public Const TYPES_SORT_BY_SETTING As String = "TypesSortBy"
        Public Const TYPES_SORT_BY_SETTING_DEFAULT As PropertyTypeSortByType = PropertyTypeSortByType.Standard

        Public Const LISTING_LAYOUT_TYPE_SETTING As String = "ListingLayoutType"
        Public Const LISTING_LAYOUT_TYPE_SETTING_DEFAULT As LatestLayoutType = LatestLayoutType.TableLayout
        Public Const LISTING_ITEMS_PER_ROW_SETTING As String = "ListingItemsPerRow"
        Public Const LISTING_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 1
        Public Const LISTING_ITEMS_PER_PAGE_SETTING As String = "ListingItemsPerPage"
        Public Const LISTING_ITEMS_PER_PAGE_SETTING_DEFAULT As Integer = 10
        Public Const LISTING_BUBBLE_FEATURED_SETTING As String = "ListingBubbleFeatured"
        Public Const LISTING_BUBBLE_FEATURED_SETTING_DEFAULT As Boolean = False
        Public Const LISTING_SEARCH_SUB_TYPES_SETTING As String = "ListingSearchSubTypes"
        Public Const LISTING_SEARCH_SUB_TYPES_SETTING_DEFAULT As Boolean = True
        Public Const LISTING_PASS_SEARCH_VALUES_SETTING As String = "ListingPassSearchValues"
        Public Const LISTING_PASS_SEARCH_VALUES_SETTING_DEFAULT As Boolean = False
        Public Const LISTING_SORT_BY_SETTING As String = "ListingSortBy"
        Public Const LISTING_SORT_BY_SETTING_DEFAULT As SortByType = SortByType.Published
        Public Const LISTING_SORT_BY_CUSTOM_FIELD_SETTING As String = "ListingSortByCustomField"
        Public Const LISTING_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const LISTING_SORT_DIRECTION_SETTING As String = "ListingSortDirection"
        Public Const LISTING_SORT_DIRECTION_SETTING_DEFAULT As SortDirectionType = SortDirectionType.Descending
        Public Const LISTING_SORT_BY_2_SETTING As String = "ListingSortBy2"
        Public Const LISTING_SORT_BY_2_SETTING_DEFAULT As SortByTypeSecondary = SortByTypeSecondary.Published
        Public Const LISTING_SORT_BY_2_CUSTOM_FIELD_SETTING As String = "ListingSortByCustomField2"
        Public Const LISTING_SORT_BY_2_CUSTOM_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const LISTING_SORT_DIRECTION_2_SETTING As String = "ListingSortDirection2"
        Public Const LISTING_SORT_DIRECTION_2_SETTING_DEFAULT As SortDirectionType = SortDirectionType.Descending
        Public Const LISTING_SORT_BY_3_SETTING As String = "ListingSortBy3"
        Public Const LISTING_SORT_BY_3_SETTING_DEFAULT As SortByTypeSecondary = SortByTypeSecondary.Published
        Public Const LISTING_SORT_BY_3_CUSTOM_FIELD_SETTING As String = "ListingSortByCustomField3"
        Public Const LISTING_SORT_BY_3_CUSTOM_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const LISTING_SORT_DIRECTION_3_SETTING As String = "ListingSortDirection3"
        Public Const LISTING_SORT_DIRECTION_3_SETTING_DEFAULT As SortDirectionType = SortDirectionType.Descending
        Public Const LISTING_USER_SORTABLE_SETTING As String = "ListingUserSortable"
        Public Const LISTING_USER_SORTABLE_SETTING_DEFAULT As Boolean = True
        Public Const LISTING_SORT_FIELDS_SETTING As String = "ListingSortFields"

        Public Const IMAGES_ENABLED_SETTING As String = "ImagesEnabled"
        Public Const IMAGES_ENABLED_SETTING_DEFAULT As Boolean = True
        Public Const IMAGES_HIGH_QUALITY_SETTING As String = "ImagesHighQuality"
        Public Const IMAGES_HIGH_QUALITY_SETTING_DEFAULT As Boolean = False
        Public Const IMAGES_INCLUDE_JQUERY_SETTING As String = "IncludeJQuery"
        Public Const IMAGES_INCLUDE_JQUERY_SETTING_DEFAULT As Boolean = True
        Public Const SMALL_WIDTH_SETTING As String = "SmallWidth"
        Public Const SMALL_WIDTH_SETTING_DEFAULT As Integer = 125
        Public Const SMALL_HEIGHT_SETTING As String = "SmallHeight"
        Public Const SMALL_HEIGHT_SETTING_DEFAULT As Integer = 100
        Public Const MEDIUM_WIDTH_SETTING As String = "MediumWidth"
        Public Const MEDIUM_WIDTH_SETTING_DEFAULT As Integer = 320
        Public Const MEDIUM_HEIGHT_SETTING As String = "MediumHeight"
        Public Const MEDIUM_HEIGHT_SETTING_DEFAULT As Integer = 240
        Public Const LARGE_WIDTH_SETTING As String = "LargeWidth"
        Public Const LARGE_WIDTH_SETTING_DEFAULT As Integer = 700
        Public Const LARGE_HEIGHT_SETTING As String = "LargeHeight"
        Public Const LARGE_HEIGHT_SETTING_DEFAULT As Integer = 475
        Public Const IMAGES_ITEMS_PER_ROW_SETTING As String = "ImagesItemsPerRow"
        Public Const IMAGES_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 2
        Public Const IMAGES_WATERMARK_ENABLED_SETTING As String = "ImagesWatermarkEnabled"
        Public Const IMAGES_WATERMARK_ENABLED_SETTING_DEFAULT As Boolean = False
        Public Const IMAGES_WATERMARK_TEXT_SETTING As String = "ImagesWatermarkText"
        Public Const IMAGES_WATERMARK_TEXT_SETTING_DEFAULT As String = ""
        Public Const IMAGES_WATERMARK_IMAGE_SETTING As String = "ImagesWatermarkImage"
        Public Const IMAGES_WATERMARK_IMAGE_SETTING_DEFAULT As String = ""
        Public Const IMAGES_WATERMARK_IMAGE_POSITION_SETTING As String = "ImagesWatermarkImagePosition"
        Public Const IMAGES_WATERMARK_IMAGE_POSITION_SETTING_DEFAULT As WatermarkPosition = WatermarkPosition.BottomRight
        Public Const IMAGES_CATEGORIES_SETTING As String = "ImagesCategories"
        Public Const IMAGES_CATEGORIES_SETTING_DEFAULT As String = ""

        Public Const BREADCRUMB_PLACEMENT_SETTING As String = "BreadcrumbPlacement"
        Public Const BREADCRUMB_PLACEMENT_SETTING_DEFAULT As BreadcrumbType = BreadcrumbType.Module
        Public Const CUSTOM_FIELD_EXPIRATION_SETTING As String = "CustomFieldExpiration"
        Public Const CUSTOM_FIELD_EXPIRATION_SETTING_DEFAULT As Integer = -1
        Public Const DEFAULT_EXPIRATION_SETTING As String = "DefaultExpiration"
        Public Const DEFAULT_EXPIRATION_SETTING_DEFAULT As Integer = -1
        Public Const DEFAULT_EXPIRATION_PERIOD_SETTING As String = "DefaultExpirationPeriod"
        Public Const DEFAULT_EXPIRATION_PERIOD_SETTING_DEFAULT As String = "D"
        Public Const FIELD_WIDTH_SETTING As String = "FieldWidth"
        Public Const FIELD_WIDTH_SETTING_DEFAULT As Integer = 400
        Public Const CHECKBOX_ITEMS_PER_ROW_SETTING As String = "CheckBoxItemsPerRow"
        Public Const CHECKBOX_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 4
        Public Const RADIO_BUTTON_ITEMS_PER_ROW_SETTING As String = "RadioButtonItemsPerRow"
        Public Const RADIO_BUTTON_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 4

        Public Const BUTTON_CLASS_SETTING As String = "FormButtonClass"
        Public Const BUTTON_CLASS_SETTING_DEFAULT As String = "CommandButton"
        Public Const HIDE_TYPES_SETTING As String = "HideTypes"
        Public Const HIDE_TYPES_SETTING_DEFAULT As Boolean = False
        Public Const TYPE_PARAMS_SETTING As String = "TypeParams"
        Public Const TYPE_PARAMS_SETTING_DEFAULT As Boolean = False
        Public Const LOCKDOWN_PROPERTYTYPE_SETTING As String = "LockDownPropertyType"
        Public Const LOCKDOWN_PROPERTYTYPE_SETTING_DEFAULT As Boolean = False
        Public Const LOCKDOWN_PROPERTYDATES_SETTING As String = "LockDownPropertyDates"
        Public Const LOCKDOWN_PROPERTYDATES_SETTING_DEFAULT As Boolean = False
        Public Const LOCKDOWN_FEATURED_SETTING As String = "LockDownFeatured"
        Public Const LOCKDOWN_FEATURED_SETTING_DEFAULT As Boolean = False
        Public Const REDIRECT_TYPE_SETTING As String = "PARedirectType"
        Public Const REDIRECT_TYPE_SETTING_DEFAULT As RedirectType = RedirectType.PropertyManager
        Public Const REDIRECT_PAGE_SETTING As String = "PARedirectPage"
        Public Const REDIRECT_PAGE_SETTING_DEFAULT As Integer = -1
        Public Const UPLOAD_MODE_SETTING As String = "FormUploadMode"
        Public Const UPLOAD_MODE_SETTING_DEFAULT As UploadType = UploadType.Flash
        Public Const UPLOAD_PLACEMENT_SETTING As String = "FormUploadPlacement"
        Public Const UPLOAD_PLACEMENT_SETTING_DEFAULT As UploadPlacementType = UploadPlacementType.SeparatePage
        Public Const CACHE_PROPERTY_VALUES_SETTING As String = "CachePropertyValues"
        Public Const CACHE_PROPERTY_VALUES_SETTING_DEFAULT As Boolean = True
        Public Const PROTECT_XSS_SETTING As String = "ProtectXSS"
        Public Const PROTECT_XSS_SETTING_DEFAULT As Boolean = True
        Public Const AGENT_DROPDOWN_SETTING As String = "AgentDropdown"
        Public Const AGENT_DROPDOWN_SETTING_DEFAULT As Boolean = False

        Public Const MAP_ENABLE_SETTING As String = "MapEnable"
        Public Const MAP_ENABLE_SETTING_DEFAULT As Boolean = False
        Public Const MAP_KEY_SETTING As String = "MapKey"
        Public Const MAP_KEY_SETTING_DEFAULT As String = ""
        Public Const MAP_WIDTH_SETTING As String = "MapWidth"
        Public Const MAP_WIDTH_SETTING_DEFAULT As Integer = 500
        Public Const MAP_HEIGHT_SETTING As String = "MapHeight"
        Public Const MAP_HEIGHT_SETTING_DEFAULT As Integer = 300
        Public Const MAP_ZOOM_SETTING As String = "MapZoom"
        Public Const MAP_ZOOM_SETTING_DEFAULT As Integer = 16
        Public Const MAP_ENABLE_DISTANCE_SETTING As String = "MapEnableDistance"
        Public Const MAP_ENABLE_DISTANCE_SETTING_DEFAULT As Boolean = False
        Public Const MAP_DISTANCE_EXPRESSION_SETTING As String = "MapDistanceExpression"
        Public Const MAP_DISTANCE_EXPRESSION_SETTING_DEFAULT As String = "[LOCATION]"
        Public Const MAP_DISTANCE_TYPE_SETTING As String = "MapDistanceType"
        Public Const MAP_DISTANCE_TYPE_SETTING_DEFAULT As DistanceType = DistanceType.Miles

        Public Const CONTACT_DESTINATION_SETTING As String = "ContactDestination"
        Public Const CONTACT_DESTINATION_SETTING_DEFAULT As DestinationType = DestinationType.PropertyOwner
        Public Const CONTACT_CUSTOM_EMAIL_SETTING As String = "ContactCustomEmail"
        Public Const CONTACT_CUSTOM_EMAIL_SETTING_DEFAULT As String = ""
        Public Const CONTACT_FIELD_SETTING As String = "ContactField"
        Public Const CONTACT_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const CONTACT_REPLY_TO_SETTING As String = "ContactReplyTo"
        Public Const CONTACT_REPLY_TO_SETTING_DEFAULT As ReplyToType = ReplyToType.PortalAdmin
        Public Const CONTACT_BCC_SETTING As String = "ContactBCC"
        Public Const CONTACT_BCC_SETTING_DEFAULT As String = ""
        Public Const CONTACT_WIDTH_SETTING As String = "ContactWidth"
        Public Const CONTACT_WIDTH_SETTING_DEFAULT As String = "100%"
        Public Const CONTACT_LOG_FIELD_SETTING As String = "ContactLogField"
        Public Const CONTACT_LOG_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const CONTACT_MESSAGE_LINES_SETTING As String = "ContactMessageLines"
        Public Const CONTACT_MESSAGE_LINES_SETTING_DEFAULT As Integer = 5
        Public Const CONTACT_HIDE_EMAIL_SETTING As String = "ContactHideEmail"
        Public Const CONTACT_HIDE_EMAIL_SETTING_DEFAULT As Boolean = False
        Public Const CONTACT_HIDE_NAME_SETTING As String = "ContactHideName"
        Public Const CONTACT_HIDE_NAME_SETTING_DEFAULT As Boolean = False
        Public Const CONTACT_HIDE_PHONE_SETTING As String = "ContactHidePhone"
        Public Const CONTACT_HIDE_PHONE_SETTING_DEFAULT As Boolean = False
        Public Const CONTACT_REQUIRE_EMAIL_SETTING As String = "ContactRequireEmail"
        Public Const CONTACT_REQUIRE_EMAIL_SETTING_DEFAULT As Boolean = False
        Public Const CONTACT_REQUIRE_NAME_SETTING As String = "ContactRequireName"
        Public Const CONTACT_REQUIRE_NAME_SETTING_DEFAULT As Boolean = False
        Public Const CONTACT_REQUIRE_PHONE_SETTING As String = "ContactRequirePhone"
        Public Const CONTACT_REQUIRE_PHONE_SETTING_DEFAULT As Boolean = False
        Public Const CONTACT_USE_CAPTCHA_SETTING As String = "ContactUseCaptcha"
        Public Const CONTACT_USE_CAPTCHA_SETTING_DEFAULT As Boolean = False
        Public Const CONTACT_CUSTOM_FIELD_ID_SETTING As String = "ContactCustomFieldID"
        Public Const CONTACT_CUSTOM_FIELD_ID_SETTING_DEFAULT As Integer = -1

        Public Const CONTACT_EMAIL_SUBJECT_HTML_DEFAULT As String = "[PortalName] - [PROPERTYLABEL] Contact ([CONTACTFORM:Name])"
        Public Const CONTACT_EMAIL_BODY_HTML_DEFAULT As String = "" _
            & "** CONTACT INFORMATION**" & vbCrLf _
            & "From: [CONTACTFORM:Name]" & vbCrLf _
            & "Phone: [CONTACTFORM:Phone]" & vbCrLf _
            & "Email: [CONTACTFORM:Email]" & vbCrLf _
            & "Message: " & vbCrLf _
            & "[CONTACTFORM:Message]" & vbCrLf _
            & "** PROPERTY **" & vbCrLf _
            & "[LINK]" & vbCrLf _
            & "** AGENT **" & vbCrLf _
            & "[HASAGENT][AGENT:FirstName] [AGENT:LastName]" & vbCrLf _
            & "[AGENT:Website]" & vbCrLf _
            & "[AGENT:Country]  [AGENT:Region] [/HASAGENT]" & vbCrLf _
            & "[HASNOAGENT]No Listing Agent[/HASNOAGENT]"

        Public Const SEND_TO_FRIEND_EMAIL_SUBJECT_HTML_DEFAULT As String = "[PortalName] - [PROPERTYLABEL] Details"
        Public Const SEND_TO_FRIEND_EMAIL_BODY_HTML_DEFAULT As String = "" _
            & "Hi," & vbCrLf & vbCrLf _
            & "A friend ([FRIEND:FROMNAME]) has referred you to the [PROPERTYLABEL] located here:-" & vbCrLf & vbCrLf _
            & "[LINK]" & vbCrLf & vbCrLf _
            & "[MESSAGE]" & vbCrLf & vbCrLf _
            & "Thank you," & vbCrLf _
            & "[PORTALNAME]"

        Public Const CURRENCY_SETTING As String = "Currency"
        Public Const CURRENCY_SETTING_DEFAULT As CurrencyType = CurrencyType.USD
        Public Const EURO_TYPE_SETTING As String = "EuroType"
        Public Const EURO_TYPE_SETTING_DEFAULT As EuroType = EuroType.French
        Public Const CURRENCY_SHOW_ALL_SETTING As String = "CurrencyShowAll"
        Public Const CURRENCY_SHOW_ALL_SETTING_DEFAULT As Boolean = True
        Public Const CURRENCY_AVAILABLE_SETTING As String = "CurrencyAvailable"
        Public Const CURRENCY_AVAILABLE_SETTING_DEFAULT As String = ""
        Public Const CURRENCY_DECIMAL_PLACES_SETTING As String = "CurrencyDecimalPlaces"
        Public Const CURRENCY_DECIMAL_PLACES_SETTING_DEFAULT As Integer = 2

        Public Const NOTIFICATION_NOTIFY_APPROVERS_SETTING As String = "NotificationNotifyApprovers"
        Public Const NOTIFICATION_NOTIFY_APPROVERS_SETTING_DEFAULT As Boolean = True
        Public Const NOTIFICATION_EMAIL_SETTING As String = "NotificationEmail"
        Public Const NOTIFICATION_EMAIL_SETTING_DEFAULT As String = ""
        Public Const NOTIFICATION_NOTIFY_BROKER_SETTING As String = "NotificationNotifyBroker"
        Public Const NOTIFICATION_NOTIFY_BROKER_SETTING_DEFAULT As Boolean = False
        Public Const NOTIFICATION_NOTIFY_OWNER_SETTING As String = "NotificationNotifyOwner"
        Public Const NOTIFICATION_NOTIFY_OWNER_SETTING_DEFAULT As Boolean = False

        Public Const NOTIFICATION_EMAIL_SUBJECT_HTML_DEFAULT As String = "[PortalName] - [PROPERTYLABEL] requires approval."
        Public Const NOTIFICATION_EMAIL_BODY_HTML_DEFAULT As String = "" _
            & "A [PROPERTYLABEL] requires approval..." & vbCrLf _
            & "[APPROVALLINK]" & vbCrLf & vbCrLf _
            & "Thank you, " & vbCrLf _
            & "[PortalName]"

        Public Const CONTACT_BROKER_SUBJECT_HTML_DEFAULT As String = "[PortalName] - [PROPERTYLABEL] has been changed."
        Public Const CONTACT_BROKER_BODY_HTML_DEFAULT As String = "" _
            & "A [PROPERTYLABEL] has been changed..." & vbCrLf _
            & "[LINK]" & vbCrLf & vbCrLf _
            & "Thank you, " & vbCrLf _
            & "[PortalName]"

        Public Const CONTACT_OWNER_SUBJECT_HTML_DEFAULT As String = "[PortalName] - [PROPERTYLABEL] has been changed."
        Public Const CONTACT_OWNER_BODY_HTML_DEFAULT As String = "" _
            & "A [PROPERTYLABEL] has been changed..." & vbCrLf _
            & "[LINK]" & vbCrLf & vbCrLf _
            & "Thank you, " & vbCrLf _
            & "[PortalName]"

        Public Const EXPORT_HEADER_DEFAULT As String = "Name,Property ID,Date Published,Featured,Type,[CUSTOMFIELDS]"
        Public Const EXPORT_ITEM_DEFAULT As String = "[DISPLAYNAME],[PROPERTYID],[DATEPUBLISHED],[FEATURED],[TYPE],[CUSTOMFIELDS]"

        Public Const OPTION_ITEM_DEFAULT As String = "<div id=""PropertyAgentButtons"">[ADDPROPERTY]&nbsp;[PROPERTYMANAGER]&nbsp;[RSS]</div>"

        Public Const PROPERTY_EMAIL_SETTING As String = "PropertyEmail"
        Public Const PROPERTY_PORTAL_NAME_SETTING As String = "PropertyPortalName"
        Public Const PROPERTY_LAST_DATE_TIME_SETTING As String = "PropertyLastDateTime"

        Public Const PROPERTY_SUBJECT_SETTING As String = "PropertySubject"
        Public Const PROPERTY_SUBJECT_DEFAULT As String = "New [PROPERTYPLURALLABEL] have been posted."

        Public Const PROPERTY_TEMPLATE_HEADER_SETTING As String = "PropertyTemplateHeader"
        Public Const PROPERTY_TEMPLATE_HEADER_DEFAULT As String = "" _
            & "The following [PROPERTYPLURALLABEL] has been posted:" _
            & vbCrLf & vbCrLf

        Public Const PROPERTY_TEMPLATE_ITEM_SETTING As String = "PropertyTemplateItem"
        Public Const PROPERTY_TEMPLATE_ITEM_DEFAULT As String = "" _
            & vbCrLf _
            & "[PROPERTYLABEL]: #[PROPERTYID] [LINK]" & vbCrLf

        Public Const PROPERTY_TEMPLATE_FOOTER_SETTING As String = "PropertyTemplateFooter"
        Public Const PROPERTY_TEMPLATE_FOOTER_DEFAULT As String = ""

        Public Const PROPERTY_NOTIFICATION_TO_SETTING As String = "PropertyNotificationTo"
        Public Const PROPERTY_NOTIFICATION_TO_DEFAULT As String = ""

        Public Const PROPERTY_NOTIFICATION_BCC_SETTING As String = "PropertyNotificationBCC"
        Public Const PROPERTY_NOTIFICATION_BCC_DEFAULT As String = ""

        Public Const PROPERTY_NOTIFICATION_SCHEDULER_ID_SETTING As String = "PropertyScheduleID"
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_ID_DEFAULT As Integer = -1
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_ENABLED_SETTING As String = "PropertySchedulerEnabled"
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_ENABLED_DEFAULT As Boolean = False
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_SETTING As String = "PropertySchedulerTimeLapse"
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_DEFAULT As Integer = 1
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_MEASUREMENT_SETTING As String = "PropertySchedulerTimeLapseMeasurement"
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_TIME_LAPSE_MEASUREMENT_DEFAULT As String = "d"
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_SETTING As String = "PropertySchedulerRetryFrequency"
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_DEFAULT As Integer = 2
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_SETTING As String = "PropertySchedulerRetryFrequencyMeasurement"
        Public Const PROPERTY_NOTIFICATION_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_DEFAULT As String = "d"

        Public Const REMINDER_EMAIL_SETTING As String = "ReminderEmail"
        Public Const REMINDER_PORTAL_NAME_SETTING As String = "ReminderPortalName"

        Public Const REMINDER_PERIOD_SETTING As String = "ReminderPeriod"
        Public Const REMINDER_PERIOD_DEFAULT As Integer = 28
        Public Const REMINDER_PERIOD_MEASUREMENT_SETTING As String = "ReminderPeriodMeasurement"
        Public Const REMINDER_PERIOD_MEASUREMENT_DEFAULT As String = "d"
        Public Const REMINDER_FREQUENCY_SETTING As String = "ReminderFrequency"
        Public Const REMINDER_FREQUENCY_DEFAULT As Integer = 7
        Public Const REMINDER_FREQUENCY_MEASUREMENT_SETTING As String = "ReminderFrequencyMeasurement"
        Public Const REMINDER_FREQUENCY_MEASUREMENT_DEFAULT As String = "d"

        Public Const REMINDER_SUBJECT_SETTING As String = "ReminderSubject"
        Public Const REMINDER_SUBJECT_DEFAULT As String = "Your [PROPERTYLABEL] is about to expire."
        Public Const REMINDER_TEMPLATE_SETTING As String = "ReminderTemplate"
        Public Const REMINDER_TEMPLATE_DEFAULT As String = "" _
            & "Hi [FIRSTNAME] [LASTNAME]," & vbCrLf & vbCrLf _
            & "Your [PROPERTYLABEL] expires on the following date:-" & vbCrLf & vbCrLf _
            & "[EXPIRYDATE]" & vbCrLf & vbCrLf _
            & "You may view/edit the property here:-" & vbCrLf & vbCrLf _
            & "[LINK]" & vbCrLf & vbCrLf _
            & "Yours Sincerely," & vbCrLf & vbCrLf _
            & "Your Name Here"
        Public Const REMINDER_BCC_SETTING As String = "ReminderBCC"
        Public Const REMINDER_BCC_DEFAULT As String = ""

        Public Const REMINDER_SCHEDULER_ID_SETTING As String = "ScheduleID"
        Public Const REMINDER_SCHEDULER_ID_DEFAULT As Integer = -1
        Public Const REMINDER_SCHEDULER_ENABLED_SETTING As String = "SchedulerEnabled"
        Public Const REMINDER_SCHEDULER_ENABLED_DEFAULT As Boolean = False
        Public Const REMINDER_SCHEDULER_TIME_LAPSE_SETTING As String = "SchedulerTimeLapse"
        Public Const REMINDER_SCHEDULER_TIME_LAPSE_DEFAULT As Integer = 1
        Public Const REMINDER_SCHEDULER_TIME_LAPSE_MEASUREMENT_SETTING As String = "SchedulerTimeLapseMeasurement"
        Public Const REMINDER_SCHEDULER_TIME_LAPSE_MEASUREMENT_DEFAULT As String = "d"
        Public Const REMINDER_SCHEDULER_RETRY_FREQUENCY_SETTING As String = "SchedulerRetryFrequency"
        Public Const REMINDER_SCHEDULER_RETRY_FREQUENCY_DEFAULT As Integer = 2
        Public Const REMINDER_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_SETTING As String = "SchedulerRetryFrequencyMeasurement"
        Public Const REMINDER_SCHEDULER_RETRY_FREQUENCY_MEASUREMENT_DEFAULT As String = "d"

        Public Const RSS_ENABLE As String = "RssEnable"
        Public Const RSS_ENABLE_DEFAULT As Boolean = True
        Public Const RSS_MAX_RECORDS As String = "RssMaxRecords"
        Public Const RSS_MAX_RECORDS_DEFAULT As Integer = 25
        Public Const RSS_TITLE_LATEST As String = "RssTitleLatest"
        Public Const RSS_TITLE_LATEST_DEFAULT As String = "Latest [PROPERTYPLURALLABEL]"
        Public Const RSS_TITLE_TYPE As String = "RssTitleType"
        Public Const RSS_TITLE_TYPE_DEFAULT As String = "[PROPERTYTYPELABEL]: [TYPE]"
        Public Const RSS_TITLE_SEARCH_RESULT As String = "RssTitleSearchResult"
        Public Const RSS_TITLE_SEARCH_RESULT_DEFAULT As String = "[PROPERTYLABEL] results"

        Public Const RSS_HEADER_DEFAULT As String = "" _
            & "<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"">" & vbCrLf _
            & "<channel>" & vbCrLf _
                & "<title>[TITLE]</title>" & vbCrLf _
                & "<link>[PORTALURL]</link>" & vbCrLf _
                & "<description></description>" & vbCrLf _
                & "<pubDate>[NOW]</pubDate>" & vbCrLf _
                & "<lastBuildDate>[NOW]</lastBuildDate>" & vbCrLf _
                & "[ITEMS]" & vbCrLf _
            & "</channel>" & vbCrLf _
            & "</rss>"

        Public Const RSS_ITEM_DEFAULT As String = "" _
            & "<item>" & vbCrLf _
                & "<title>[PROPERTYLABEL] [PROPERTYID]</title>" & vbCrLf _
                & "<link>[LINK]</link>" & vbCrLf _
                & "<dc:creator>[DISPLAYNAME]</dc:creator>" & vbCrLf _
                & "<guid isPermaLink=""false"">[PROPERTYID]</guid>" & vbCrLf _
                & "<description>[CUSTOMFIELDS]</description>" & vbCrLf _
                & "<pubDate>[DATEPUBLISHED]</pubDate>" & vbCrLf _
            & "</item>"

        Public Const RSS_READER_HEADER_DEFAULT As String = "<ul>"
        Public Const RSS_READER_ITEM_DEFAULT As String = "<li><a href=""[LINK]"" target=""_blank"">[TITLE]</a></li>"
        Public Const RSS_READER_FOOTER_DEFAULT As String = "</ul>"

        Public Const PDF_HEADER_DEFAULT As String = ""
        Public Const PDF_ITEM_DEFAULT As String = "[CUSTOMFIELDS]"
        Public Const PDF_FOOTER_DEFAULT As String = ""

        Public Const XML_ENABLE As String = "XmlEnable"
        Public Const XML_ENABLE_DEFAULT As Boolean = False
        Public Const XML_MAX_RECORDS As String = "XmlMaxRecords"
        Public Const XML_MAX_RECORDS_DEFAULT As Integer = 25

        Public Const XML_HEADER_DEFAULT As String = "" _
            & "<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"">" & vbCrLf _
            & "<channel>" & vbCrLf _
                & "<title>[TITLE]</title>" & vbCrLf _
                & "<link>[PORTALURL]</link>" & vbCrLf _
                & "<description></description>" & vbCrLf _
                & "<pubDate>[NOW]</pubDate>" & vbCrLf _
                & "<lastBuildDate>[NOW]</lastBuildDate>" & vbCrLf _
                & "[ITEMS]" & vbCrLf _
            & "</channel>" & vbCrLf _
            & "</rss>"

        Public Const XML_ITEM_DEFAULT As String = "" _
            & "<item>" & vbCrLf _
                & "<title>[PROPERTYLABEL] [PROPERTYID]</title>" & vbCrLf _
                & "<link>[LINK]</link>" & vbCrLf _
                & "<dc:creator>[DISPLAYNAME]</dc:creator>" & vbCrLf _
                & "<guid isPermaLink=""false"">[PROPERTYID]</guid>" & vbCrLf _
                & "<description>[CUSTOMFIELDS]</description>" & vbCrLf _
                & "<pubDate>[DATEPUBLISHED]</pubDate>" & vbCrLf _
            & "</item>"

        Public Const FRIEND_BCC_SETTING As String = "FriendBCC"
        Public Const FRIEND_BCC_SETTING_DEFAULT As String = ""
        Public Const FRIEND_WIDTH_SETTING As String = "FriendWidth"
        Public Const FRIEND_WIDTH_SETTING_DEFAULT As Integer = 350

        Public Const SEO_REDIRECT_SETTING As String = "SEORedirect"
        Public Const SEO_REDIRECT_SETTING_DEFAULT As Boolean = True
        Public Const SEO_AGENT_TYPE_SETTING As String = "SEOAgentType"
        Public Const SEO_AGENT_TYPE_SETTING_DEFAULT As String = "agentType"
        Public Const SEO_PROPERTY_ID_SETTING As String = "SEOPropertyID"
        Public Const SEO_PROPERTY_ID_SETTING_DEFAULT As String = "PropertyID"
        Public Const SEO_PROPERTY_TYPE_ID_SETTING As String = "SEOPropertyTypeID"
        Public Const SEO_PROPERTY_TYPE_ID_SETTING_DEFAULT As String = "PropertyTypeID"
        Public Const SEO_VIEW_PROPERTY_TITLE_SETTING As String = "SEOViewPropertyTitle"
        Public Const SEO_VIEW_PROPERTY_TITLE_SETTING_DEFAULT As String = ""
        Public Const SEO_VIEW_TYPE_TITLE_SETTING As String = "SEOViewTypeTitle"
        Public Const SEO_VIEW_TYPE_TITLE_SETTING_DEFAULT As String = ""
        Public Const SEO_TITLE_REPLACEMENT_SETTING As String = "SEOTitleReplacement"
        Public Const SEO_TITLE_REPLACEMENT_SETTING_DEFAULT As TitleReplacementType = TitleReplacementType.Dash
        Public Const SEO_CANONICAL_LINK_SETTING As String = "SEOCanonicalLink"
        Public Const SEO_CANONICAL_LINK_SETTING_DEFAULT As Boolean = True

        Public Const COMMENT_WIDTH_SETTING As String = "CommentWidth"
        Public Const COMMENT_WIDTH_SETTING_DEFAULT As String = "100%"
        Public Const COMMENT_USE_CAPTCHA_SETTING As String = "CommentUseCaptcha"
        Public Const COMMENT_USE_CAPTCHA_SETTING_DEFAULT As Boolean = False
        Public Const COMMENT_NOTIFY_OWNER_SETTING As String = "CommentNotifyOwner"
        Public Const COMMENT_NOTIFY_OWNER_SETTING_DEFAULT As Boolean = False
        Public Const COMMENT_NOTIFY_EMAIL_SETTING As String = "CommentNotifyEmail"
        Public Const COMMENT_NOTIFY_EMAIL_SETTING_DEFAULT As String = ""

        Public Const COMMENT_EMAIL_SUBJECT_HTML_DEFAULT As String = "[PortalName] - [PROPERTYLABEL] comment notification."
        Public Const COMMENT_EMAIL_BODY_HTML_DEFAULT As String = "" _
            & "A [PROPERTYLABEL] has received a comment..." & vbCrLf _
            & "[COMMENT]" & vbCrLf & vbCrLf _
            & "[LINK]" & vbCrLf & vbCrLf _
            & "Thank you, " & vbCrLf _
            & "[PortalName]"

        Public Const REVIEW_EMAIL_SUBJECT_HTML_DEFAULT As String = "[PortalName] - [PROPERTYLABEL] review notification."
        Public Const REVIEW_EMAIL_BODY_HTML_DEFAULT As String = "" _
            & "A [PROPERTYLABEL] has received a review awaiting approval..." & vbCrLf _
            & "[LINK]" & vbCrLf & vbCrLf _
            & "Thank you, " & vbCrLf _
            & "[PortalName]"

        Public Const REVIEW_WIDTH_SETTING As String = "ReviewWidth"
        Public Const REVIEW_WIDTH_SETTING_DEFAULT As Integer = 350
        Public Const REVIEW_MODERATION_SETTING As String = "ReviewModeration"
        Public Const REVIEW_MODERATION_SETTING_DEFAULT As Boolean = False
        Public Const REVIEW_EMAIL_SETTING As String = "ReviewEmail"
        Public Const REVIEW_EMAIL_SETTING_DEFAULT As String = ""
        Public Const REVIEW_ANONYMOUS_SETTING As String = "ReviewAnonymous"
        Public Const REVIEW_ANONYMOUS_SETTING_DEFAULT As Boolean = False

        Public Const PROPERTY_MANAGER_HIDE_AUTHOR_DETAILS_SETTING As String = "PropertyManagerHideAuthorDetails"
        Public Const PROPERTY_MANAGER_HIDE_AUTHOR_DETAILS_SETTING_DEFAULT As Boolean = False
        Public Const PROPERTY_MANAGER_HIDE_PUBLISHING_DETAILS_SETTING As String = "PropertyManagerHidePublishingDetails"
        Public Const PROPERTY_MANAGER_HIDE_PUBLISHING_DETAILS_SETTING_DEFAULT As Boolean = False
        Public Const PROPERTY_MANAGER_ITEMS_PER_PAGE_SETTING As String = "PropertyManagerItemsPerPage"
        Public Const PROPERTY_MANAGER_ITEMS_PER_PAGE_SETTING_DEFAULT As Integer = 10
        Public Const PROPERTY_MANAGER_SORT_BY_SETTING As String = "PropertyManagerSortBy"
        Public Const PROPERTY_MANAGER_SORT_BY_SETTING_DEFAULT As SortByType = SortByType.Published
        Public Const PROPERTY_MANAGER_SORT_BY_CUSTOM_FIELD_SETTING As String = "PropertyManagerSortByCustomField"
        Public Const PROPERTY_MANAGER_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const PROPERTY_MANAGER_SORT_DIRECTION_SETTING As String = "PropertyManagerSortDirection"
        Public Const PROPERTY_MANAGER_SORT_DIRECTION_SETTING_DEFAULT As SortDirectionType = SortDirectionType.Descending

        Public Const PERMISSION_VIEW_DETAIL_SETTING As String = "PermissionViewDetail"
        Public Const PERMISSION_SUBMIT_SETTING As String = "PermissionSubmit"
        Public Const PERMISSION_ADD_IMAGES_SETTING As String = "PermissionAddImages"
        Public Const PERMISSION_ADD_IMAGES_LIMIT_SETTING As String = "PermissionAddImagesLimit"
        Public Const PERMISSION_APPROVE_SETTING As String = "PermissionApprove"
        Public Const PERMISSION_AUTO_APPROVE_SETTING As String = "PermissionAutoApprove"
        Public Const PERMISSION_DELETE_SETTING As String = "PermissionDelete"
        Public Const PERMISSION_FEATURE_SETTING As String = "PermissionFeature"
        Public Const PERMISSION_AUTO_FEATURE_SETTING As String = "PermissionAutoFeature"
        Public Const PERMISSION_BROKER_SETTING As String = "PermissionBroker"
        Public Const PERMISSION_EXPORT_SETTING As String = "PermissionExport"
        Public Const PERMISSION_LOCKDOWN_SETTING As String = "PermissionLockDown"
        Public Const PERMISSION_PUBLISH_DETAIL_SETTING As String = "PermissionPublishDetail"

        Public Const PERMISSION_DETAIL_URL_SETTING As String = "PermissionDetailUrl"
        Public Const PERMISSION_LIMIT_SETTING As String = "PermissionLimit"

        Public Const PERMISSION_ADMIN_CUSTOM_FIELD_SETTING As String = "PermissionAdminCustomField"
        Public Const PERMISSION_ADMIN_REVIEW_FIELD_SETTING As String = "PermissionAdminReviewField"
        Public Const PERMISSION_ADMIN_EMAIL_FILES_SETTING As String = "PermissionAdminEmailFiles"
        Public Const PERMISSION_ADMIN_LAYOUT_FILES_SETTING As String = "PermissionAdminLayoutFiles"
        Public Const PERMISSION_ADMIN_LAYOUT_SETTINGS_SETTING As String = "PermissionAdminLayoutSettings"
        Public Const PERMISSION_ADMIN_TYPES_SETTING As String = "PermissionAdminTypes"

        Public Const PERMISSION_CUSTOM_FIELD_SETTING As String = "PermissionCustomField-"

        Public Const CORE_SEARCH_ENABLED_SETTING As String = "CoreSearchEnabled"
        Public Const CORE_SEARCH_ENABLED_SETTING_DEFAULT As Boolean = False
        Public Const CORE_SEARCH_TITLE_SETTING As String = "CoreSearchTitle"
        Public Const CORE_SEARCH_DESCRIPTION_SETTING As String = "CoreSearchDescription"

        Public Const TEMPLATE_INCLUDE_STYLESHEET_SETTING As String = "TemplateIncludeStylesheet"
        Public Const TEMPLATE_INCLUDE_STYLESHEET_SETTING_DEFAULT As Boolean = True

        Public Const LATEST_MODULE_ID_SETTING As String = "LatestModuleID"
        Public Const LATEST_MODULE_ID_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_TAB_ID_SETTING As String = "LatestTabID"
        Public Const LATEST_TAB_ID_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_TYPE_ID_SETTING As String = "LatestTypeID"
        Public Const LATEST_TYPE_ID_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_BUBBLE_FEATURED_SETTING As String = "LatestBubbleFeatured"
        Public Const LATEST_BUBBLE_FEATURED_SETTING_DEFAULT As Boolean = False
        Public Const LATEST_FEATURED_ONLY_SETTING As String = "LatestFeaturedOnly"
        Public Const LATEST_FEATURED_ONLY_SETTING_DEFAULT As Boolean = False
        Public Const LATEST_SHOW_RELATED_SETTING As String = "LatestShowRelated"
        Public Const LATEST_SHOW_RELATED_SETTING_DEFAULT As Boolean = False
        Public Const LATEST_RELATED_CUSTOM_FIELD_SETTING As String = "LatestRelatedCustomField"
        Public Const LATEST_RELATED_CUSTOM_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_SHOW_SHORTLIST_SETTING As String = "LatestShowShortList"
        Public Const LATEST_SHOW_SHORTLIST_SETTING_DEFAULT As Boolean = False
        Public Const LATEST_MAX_NUMBER_SETTING As String = "LatestMaxNumber"
        Public Const LATEST_MAX_NUMBER_SETTING_DEFAULT As Integer = 5
        Public Const LATEST_PAGE_SIZE_SETTING As String = "LatestPageSize"
        Public Const LATEST_PAGE_SIZE_SETTING_DEFAULT As Integer = 10
        Public Const LATEST_ENABLE_PAGER_SETTING As String = "LatestEnablePager"
        Public Const LATEST_ENABLE_PAGER_SETTING_DEFAULT As Boolean = False
        Public Const LATEST_START_DATE_SETTING As String = "LatestStartDate"
        Public Const LATEST_MAX_AGE_SETTING As String = "LatestMaxAge"
        Public Const LATEST_MAX_AGE_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_MIN_AGE_SETTING As String = "LatestMinAge"
        Public Const LATEST_MIN_AGE_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_ITEMS_PER_ROW_SETTING As String = "LatestItemsPerRow"
        Public Const LATEST_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 1
        Public Const LATEST_SORT_BY_SETTING As String = "LatestSortBy"
        Public Const LATEST_SORT_BY_SETTING_DEFAULT As SortByType = SortByType.Published
        Public Const LATEST_SORT_BY_CUSTOM_FIELD_SETTING As String = "LatestSortByCustomField"
        Public Const LATEST_SORT_BY_CUSTOM_FIELD_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_SORT_DIRECTION_SETTING As String = "LatestSortDirection"
        Public Const LATEST_SORT_DIRECTION_SETTING_DEFAULT As SortDirectionType = SortDirectionType.Descending
        Public Const LATEST_USER_SORTABLE_SETTING As String = "LatestUserSortable"
        Public Const LATEST_USER_SORTABLE_SETTING_DEFAULT As Boolean = False
        Public Const LATEST_USER_SORTABLE_FIELDS_SETTING As String = "LatestUserSortableFields"
        Public Const LATEST_USER_SORTABLE_FIELDS_SETTING_DEFAULT As String = ""
        Public Const LATEST_USER_FILTER_SETTING As String = "LatestUserFilter"
        Public Const LATEST_USER_FILTER_SETTING_DEFAULT As UserFilterType = UserFilterType.None
        Public Const LATEST_USER_FILTER_SPECIFIC_SETTING As String = "LatestUserFilterSpecific"
        Public Const LATEST_USER_FILTER_SPECIFIC_SETTING_DEFAULT As Integer = -1
        Public Const LATEST_USER_FILTER_PARAMETER_SETTING As String = "LatestUserParameter"
        Public Const LATEST_USER_FILTER_PARAMETER_SETTING_DEFAULT As String = "ID"

        Public Const LATEST_CUSTOM_FIELD_FILTERS_SETTING As String = "LatestCustomFieldFilters"
        Public Const LATEST_CUSTOM_FIELD_FILTERS_SETTING_DEFAULT As String = ""
        Public Const LATEST_CUSTOM_FIELD_VALUES_SETTING As String = "LatestCustomFieldValues"
        Public Const LATEST_CUSTOM_FIELD_VALUES_SETTING_DEFAULT As String = ""

        Public Const LATEST_LAYOUT_MODE_SETTING As String = "LatestLayoutMode"
        Public Const LATEST_LAYOUT_MODE_SETTING_DEFAULT As LatestLayoutMode = LatestLayoutMode.TemplateLayout
        Public Const LATEST_LAYOUT_TYPE_SETTING As String = "LatestLayoutType"
        Public Const LATEST_LAYOUT_TYPE_SETTING_DEFAULT As LatestLayoutType = LatestLayoutType.TableLayout
        Public Const LATEST_LAYOUT_INCLUDE_STYLESHEET_SETTING As String = "LatestIncludeStylesheet"
        Public Const LATEST_LAYOUT_INCLUDE_STYLESHEET_SETTING_DEFAULT As Boolean = True
        Public Const LATEST_LAYOUT_HEADER_SETTING As String = "LatestLayoutHeader"
        Public Const LATEST_LAYOUT_HEADER_SETTING_DEFAULT As String = ""
        Public Const LATEST_LAYOUT_ITEM_SETTING As String = "LatestLayoutItem"
        Public Const LATEST_LAYOUT_ITEM_SETTING_DEFAULT As String = "<div class=""Normal"">[EDIT]<a href=""[LINK]"">[CUSTOM:TITLE]</a><br /></div>"
        Public Const LATEST_LAYOUT_FOOTER_SETTING As String = "LatestLayoutFooter"
        Public Const LATEST_LAYOUT_FOOTER_SETTING_DEFAULT As String = ""
        Public Const LATEST_LAYOUT_EMPTY_SETTING As String = "LatestLayoutEmpty"
        Public Const LATEST_LAYOUT_EMPTY_SETTING_DEFAULT As String = ""

        Public Const SEARCH_MODULE_ID_SETTING As String = "SearchModuleID"
        Public Const SEARCH_MODULE_ID_SETTING_DEFAULT As Integer = -1
        Public Const SEARCH_TAB_ID_SETTING As String = "SearchTabID"
        Public Const SEARCH_TAB_ID_SETTING_DEFAULT As Integer = -1
        Public Const SEARCH_SMALL_SORT_BY_SETTING As String = "SearchSmallSortBy"
        Public Const SEARCH_SMALL_SORT_BY_CUSTOM_FIELD_SETTING As String = "SearchSmallSortByCustomField"
        Public Const SEARCH_SMALL_SORT_DIRECTION_SETTING As String = "SearchSmallSortDirection"
        Public Const SEARCH_WIDTH_SETTING As String = "SearchWidth"
        Public Const SEARCH_WIDTH_SETTING_DEFAULT As Integer = 100
        Public Const SEARCH_CHECKBOX_ITEMS_PER_ROW_SETTING As String = "SearchCheckBoxItemsPerRow"
        Public Const SEARCH_CHECKBOX_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 4
        Public Const SEARCH_RADIO_BUTTON_ITEMS_PER_ROW_SETTING As String = "SearchRADIO_BUTTON_ITEMS_PER_ROW"
        Public Const SEARCH_RADIO_BUTTON_ITEMS_PER_ROW_SETTING_DEFAULT As Integer = 4
        Public Const SEARCH_CUSTOM_FIELDS As String = "SearchCustomFields"
        Public Const SEARCH_CUSTOM_FIELDS_DEFAULT As String = ""
        Public Const SEARCH_LAYOUT_MODE_SETTING As String = "SearchLayoutMode"
        Public Const SEARCH_LAYOUT_MODE_SETTING_DEFAULT As SearchLayoutMode = SearchLayoutMode.StandardLayout
        Public Const SEARCH_LAYOUT_TEMPLATE_SETTING As String = "SearchLayoutTemplate"
        Public Const SEARCH_LAYOUT_TEMPLATE_SETTING_DEFAULT As String = "[WILDCARD]<BR />[SEARCHBUTTON]"
        Public Const SEARCH_SMALL_HIDE_HELP_ICON_SETTING As String = "SearchSmallHideHelpIcon"
        Public Const SEARCH_SMALL_HIDE_HELP_ICON_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_WILDCARD_SETTING As String = "SearchSmallWildcard"
        Public Const SEARCH_SMALL_WILDCARD_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_TYPES_SETTING As String = "SearchSmallTypes"
        Public Const SEARCH_SMALL_TYPES_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_HIDE_ZERO_SETTING As String = "SearchSmallHideZero"
        Public Const SEARCH_SMALL_HIDE_ZERO_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_HIDE_TYPE_COUNT_SETTING As String = "SearchSmallHideTypeCount"
        Public Const SEARCH_SMALL_HIDE_TYPE_COUNT_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_LOCATION_SETTING As String = "SearchSmallLocation"
        Public Const SEARCH_SMALL_LOCATION_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_AGENTS_SETTING As String = "SearchSmallAgents"
        Public Const SEARCH_SMALL_AGENTS_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_BROKERS_SETTING As String = "SearchSmallBrokers"
        Public Const SEARCH_SMALL_BROKERS_SETTING_DEFAULT As Boolean = False
        Public Const SEARCH_SMALL_STYLE_SETTING As String = "SearchSmallStyle"
        Public Const SEARCH_SMALL_STYLE_SETTING_DEFAULT As String = "CommandButton"
        Public Const SEARCH_SPLIT_RANGE_SETTING As String = "SearchSmallSplitRange"
        Public Const SEARCH_SPLIT_RANGE_SETTING_DEFAULT As Boolean = True

        Public Const TYPES_MODULE_ID_SETTING As String = "TypesModuleID"
        Public Const TYPES_MODULE_ID_SETTING_DEFAULT As Integer = -1
        Public Const TYPES_TAB_ID_SETTING As String = "TypesTabID"
        Public Const TYPES_TAB_ID_SETTING_DEFAULT As Integer = -1

        Public Const TYPES_LAYOUT_MODE_SETTING As String = "TypesLayoutMode"
        Public Const TYPES_LAYOUT_MODE_SETTING_DEFAULT As LatestLayoutMode = LatestLayoutMode.TemplateLayout
        Public Const TYPES_LAYOUT_AGENT_FILTER_SETTING As String = "TypesLayoutAgentFilter"
        Public Const TYPES_LAYOUT_AGENT_FILTER_SETTING_DEFAULT As String = ""
        Public Const TYPES_LAYOUT_HIDE_ZERO_SETTING As String = "TypesLayoutHideZero"
        Public Const TYPES_LAYOUT_HIDE_ZERO_SETTING_DEFAULT As Boolean = False
        Public Const TYPES_LAYOUT_SHOW_TOP_LEVEL_ONLY_SETTING As String = "TypesLayoutShowTopLevelOnly"
        Public Const TYPES_LAYOUT_SHOW_TOP_LEVEL_ONLY_SETTING_DEFAULT As Boolean = False
        Public Const TYPES_LAYOUT_SHOW_ALL_SETTING As String = "TypesShowAll"
        Public Const TYPES_LAYOUT_SHOW_ALL_SETTING_DEFAULT As Boolean = True
        Public Const TYPES_LAYOUT_FILTER_SETTING As String = "TypesFilter"
        Public Const TYPES_LAYOUT_FILTER_SETTING_DEFAULT As String = ""
        Public Const TYPES_LAYOUT_INCLUDE_STYLESHEET_SETTING As String = "TypesIncludeStylesheet"
        Public Const TYPES_LAYOUT_INCLUDE_STYLESHEET_SETTING_DEFAULT As Boolean = True
        Public Const TYPES_LAYOUT_HEADER_SETTING As String = "TypesLayoutHeader"
        Public Const TYPES_LAYOUT_HEADER_SETTING_DEFAULT As String = ""
        Public Const TYPES_LAYOUT_ITEM_SETTING As String = "TypesLayoutItem"
        Public Const TYPES_LAYOUT_ITEM_SETTING_DEFAULT As String = "<div class=""Normal""><a href=""[TYPELINK]"">[TYPEINDENTED] ([TYPECOUNT])</a><br /></div>"
        Public Const TYPES_LAYOUT_FOOTER_SETTING As String = "TypesLayoutFooter"
        Public Const TYPES_LAYOUT_FOOTER_SETTING_DEFAULT As String = ""

#End Region

    End Class

End Namespace