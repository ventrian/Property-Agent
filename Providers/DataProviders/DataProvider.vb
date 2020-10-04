Imports System

Imports DotNetNuke

Namespace Ventrian.PropertyAgent

    Public MustInherit Class DataProvider

#Region " Shared/Static Methods "

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "Ventrian.PropertyAgent", "Ventrian.PropertyAgent"), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region " Abstract Methods "

#Region " Agent Methods "

        Public MustOverride Function ListAgentActive(ByVal moduleID As Integer) As IDataReader

#End Region

#Region " Broker Methods "

        Public MustOverride Function ListBrokers(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal roles As String) As IDataReader
        Public MustOverride Function ListAgentAvailable(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal roles As String, ByVal brokerID As Integer) As IDataReader
        Public MustOverride Function ListAgentSelected(ByVal moduleID As Integer, ByVal brokerID As Integer) As IDataReader
        Public MustOverride Sub AddBroker(ByVal userID As Integer, ByVal brokerID As Integer, ByVal moduleID As Integer)
        Public MustOverride Sub DeleteBroker(ByVal userID As Integer, ByVal brokerID As Integer, ByVal moduleID As Integer)

#End Region

#Region " Comment Methods "

        Public MustOverride Function AddComment(ByVal propertyID As Integer, ByVal parentID As Integer, ByVal userID As Integer, ByVal comment As String, ByVal createDate As DateTime, ByVal name As String, ByVal email As String, ByVal website As String) As Integer
        Public MustOverride Function ListComment(ByVal propertyID As Integer) As IDataReader
        Public MustOverride Sub DeleteComment(ByVal commentID As Integer)

#End Region

#Region " Contact Log Methods "

        Public MustOverride Sub AddContactLog(ByVal moduleID As Integer, ByVal dateSent As DateTime, ByVal sentTo As String, ByVal sentFrom As String, ByVal subject As String, ByVal body As String, ByVal fieldValues As String, ByVal propertyID As Integer)
        Public MustOverride Function ListContactLog(ByVal moduleID As Integer, ByVal dateBegin As DateTime, ByVal dateEnd As DateTime) As IDataReader

#End Region

#Region " Review Field Methods "

        Public MustOverride Function GetContactField(ByVal contactFieldID As Integer) As IDataReader
        Public MustOverride Function ListContactField(ByVal moduleId As Integer) As IDataReader
        Public MustOverride Function AddContactField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer, ByVal customFieldID As Integer) As Integer
        Public MustOverride Sub UpdateContactField(ByVal contactFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer, ByVal customFieldID As Integer)
        Public MustOverride Sub DeleteContactField(ByVal contactFieldID As Integer)

#End Region

#Region " Custom Field Methods "

        Public MustOverride Function ExecuteQuery(ByVal query As String) As IDataReader
        Public MustOverride Function GetCustomField(ByVal customFieldID As Integer) As IDataReader
        Public MustOverride Function ListCustomField(ByVal moduleId As Integer, ByVal isPublishedOnly As Boolean) As IDataReader
        Public MustOverride Function AddCustomField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal fieldElementType As Integer, ByVal fieldElementDropDown As Integer, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isInManager As Boolean, ByVal isSortable As Boolean, ByVal isInListing As Boolean, ByVal isCaptionHidden As Boolean, ByVal isFeatured As Boolean, ByVal isPublished As Boolean, ByVal isHidden As Boolean, ByVal isSearchable As Boolean, ByVal IsLockDown As Boolean, ByVal searchType As Integer, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal validationType As Integer, ByVal fieldElementsFrom As String, ByVal fieldElementsTo As String, ByVal length As Integer, ByVal regularExpression As String, ByVal includeCount As Boolean, ByVal hideZeroCount As Boolean, ByVal inheritSecurity As Boolean) As Integer

        Friend Function GetTabModuleSettings(tabModuleId As Integer) As IDataReader
            Throw New NotImplementedException()
        End Function

        Public MustOverride Sub UpdateCustomField(ByVal customFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal fieldElementType As Integer, ByVal fieldElementDropDown As Integer, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isInManager As Boolean, ByVal isSortable As Boolean, ByVal isInListing As Boolean, ByVal isCaptionHidden As Boolean, ByVal isFeatured As Boolean, ByVal isPublished As Boolean, ByVal isHidden As Boolean, ByVal isSearchable As Boolean, ByVal IsLockDown As Boolean, ByVal searchType As Integer, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal validationType As Integer, ByVal fieldElementsFrom As String, ByVal fieldElementsTo As String, ByVal length As Integer, ByVal regularExpression As String, ByVal includeCount As Boolean, ByVal hideZeroCount As Boolean, ByVal inheritSecurity As Boolean)
        Public MustOverride Sub DeleteCustomField(ByVal customFieldID As Integer)

#End Region

#Region " Photo Methods "

        Public MustOverride Function GetPhoto(ByVal photoID As Integer) As IDataReader
        Public MustOverride Function ListPhoto(ByVal propertyID As Integer, ByVal propertyGuid As String) As IDataReader
        Public MustOverride Function AddPhoto(ByVal propertyID As Integer, ByVal title As String, ByVal filename As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal sortOrder As Integer, ByVal propertyGuid As String, ByVal photoType As Integer, ByVal externalUrl As String, ByVal category As String) As Integer
        Public MustOverride Sub UpdatePhoto(ByVal photoID As Integer, ByVal propertyID As Integer, ByVal title As String, ByVal filename As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal sortOrder As Integer, ByVal propertyGuid As String, ByVal photoType As Integer, ByVal externalUrl As String, ByVal category As String)
        Public MustOverride Sub DeletePhoto(ByVal photoID As Integer)
        Public MustOverride Sub DeletePhotoByPropertyID(ByVal propertyID As Integer)

#End Region

#Region " Property Methods "

        Public MustOverride Function CountProperty(ByVal moduleID As Integer, ByVal userID As Integer) As Integer
        Public MustOverride Function GetProperty(ByVal propertyID As Integer) As IDataReader
        Public MustOverride Function ListProperty(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal status As Integer, ByVal authorID As Integer, ByVal brokerID As Integer, ByVal isActive As Boolean, ByVal isPending As Boolean, ByVal isExpired As Boolean, ByVal showFeaturedOnly As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal sortBy As Integer, ByVal sortByID As Integer, ByVal sortByIDType As Integer, ByVal sortOrder As Integer, ByVal sortBy2 As Integer, ByVal sortByID2 As Integer, ByVal sortByIDType2 As Integer, ByVal sortOrder2 As Integer, ByVal sortBy3 As Integer, ByVal sortByID3 As Integer, ByVal sortByIDType3 As Integer, ByVal sortOrder3 As Integer, ByVal customFieldIDs As String, ByVal searchValues As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal bubbleFeatured As Boolean, ByVal searchSubTypes As Boolean, ByVal propertyIDForNextPrev As Integer, ByVal latitude As Double, ByVal longitude As Double, ByVal startDate As DateTime, ByVal agentFilter As String, ByVal shortListID As String) As IDataReader
        Public MustOverride Function AddProperty(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal isFeatured As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal dateCreated As DateTime, ByVal dateModified As DateTime, ByVal datePublished As DateTime, ByVal dateExpired As DateTime, ByVal viewCount As Integer, ByVal status As Integer, ByVal authorID As Integer, ByVal modifiedID As Integer, ByVal latitude As Double, ByVal longitude As Double) As Integer
        Public MustOverride Sub UpdateProperty(ByVal propertyID As Integer, ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal isFeatured As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal dateCreated As DateTime, ByVal dateModified As DateTime, ByVal datePublished As DateTime, ByVal dateExpired As DateTime, ByVal viewCount As Integer, ByVal status As Integer, ByVal authorID As Integer, ByVal modifiedID As Integer, ByVal latitude As Double, ByVal longitude As Double)
        Public MustOverride Sub DeleteProperty(ByVal propertyID As Integer)
        Public MustOverride Sub DeletePropertyByModuleID(ByVal moduleID As Integer)

#End Region

#Region " Property Type Methods "

        Public MustOverride Function ListPropertyTypeAll(ByVal moduleID As Integer, ByVal showPublishedOnly As Boolean, ByVal sortBy As Integer, ByVal agentFilter As String) As IDataReader
        Public MustOverride Function AddPropertyType(ByVal parentID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal description As String, ByVal imageFile As String, ByVal sortOrder As Integer, ByVal isPublished As Boolean, ByVal allowProperties As Boolean) As Integer
        Public MustOverride Sub UpdatePropertyType(ByVal propertyTypeID As Integer, ByVal parentID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal description As String, ByVal imageFile As String, ByVal sortOrder As Integer, ByVal isPublished As Boolean, ByVal allowProperties As Boolean)
        Public MustOverride Sub DeletePropertyType(ByVal propertyTypeID As Integer)

#End Region

#Region " Property Value Methods "

        Public MustOverride Function GetPropertyValue(ByVal propertyValueID As Integer) As IDataReader
        Public MustOverride Function GetPropertyValueByField(ByVal propertyID As Integer, ByVal customFieldID As Integer) As IDataReader
        Public MustOverride Function ListPropertyValue(ByVal propertyID As Integer) As IDataReader
        Public MustOverride Function ListPropertyValueByField(ByVal customFieldID As Integer) As IDataReader
        Public MustOverride Function AddPropertyValue(ByVal propertyID As Integer, ByVal customFieldID As Integer, ByVal customValue As String) As Integer
        Public MustOverride Sub UpdatePropertyValue(ByVal propertyValueID As Integer, ByVal propertyID As Integer, ByVal customFieldID As Integer, ByVal customValue As String)
        Public MustOverride Sub DeletePropertyValue(ByVal propertyValueID As Integer)

#End Region

#Region " Rating Methods "

        Public MustOverride Function AddRating(ByVal propertyID As Integer, ByVal userID As Integer, ByVal commentID As String, ByVal reviewID As String, ByVal rating As Double, ByVal createDate As DateTime) As Integer
        Public MustOverride Sub DeleteRating(ByVal propertyID As Integer, ByVal ratingID As Integer)
        Public MustOverride Function GetRating(ByVal propertyID As Integer, ByVal userID As Integer) As IDataReader

#End Region

#Region " Review Field Methods "

        Public MustOverride Function AddReview(ByVal propertyID As Integer, ByVal userID As Integer, ByVal createDate As DateTime, ByVal isApproved As Boolean) As Integer
        Public MustOverride Function AddReviewValue(ByVal reviewID As Integer, ByVal reviewFieldID As Integer, ByVal reviewValue As String) As Integer
        Public MustOverride Sub DeleteReviewValue(ByVal reviewValueID As Integer)
        Public MustOverride Sub DeleteReview(ByVal reviewID As Integer)
        Public MustOverride Function ListReviews(ByVal moduleID As Integer, ByVal propertyID As Integer, ByVal isApproved As Boolean) As IDataReader
        Public MustOverride Function ListReviewValue(ByVal reviewID As Integer) As IDataReader
        Public MustOverride Function GetReview(ByVal propertyID As Integer, ByVal userID As Integer) As IDataReader
        Public MustOverride Function GetReviewField(ByVal reviewFieldID As Integer) As IDataReader
        Public MustOverride Function ListReviewField(ByVal moduleId As Integer) As IDataReader
        Public MustOverride Function AddReviewField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer) As Integer
        Public MustOverride Sub UpdateReview(ByVal reviewID As Integer, ByVal isApproved As Boolean)
        Public MustOverride Sub UpdateReviewField(ByVal reviewFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer)
        Public MustOverride Sub DeleteReviewField(ByVal reviewFieldID As Integer)

#End Region

#Region " Shortlist Methods "

        Public MustOverride Sub AddShortlist(ByVal propertyID As Integer, ByVal userID As Integer, ByVal createDate As DateTime, ByVal userKey As String)
        Public MustOverride Sub DeleteShortlist(ByVal propertyID As Integer, ByVal userID As Integer, ByVal userKey As String)
        Public MustOverride Function GetShortlist(ByVal propertyID As Integer, ByVal userID As Integer, ByVal userKey As String) As IDataReader

#End Region

#Region " Statistic Methods "

        Public MustOverride Sub AddStatistic(ByVal propertyID As Integer, ByVal userID As Integer, ByVal remoteAddress As String, ByVal moduleID As Integer)

        Public MustOverride Function StatisticGet(ByVal propertyID As Integer) As IDataReader

        Public MustOverride Function StatisticList(ByVal moduleID As Integer) As IDataReader

#End Region

#Region " Subscribe Methods "

        Public MustOverride Sub AddSubscriber(ByVal propertyID As Integer, ByVal userID As Integer)
        Public MustOverride Sub DeleteSubscriber(ByVal propertyID As Integer, ByVal userID As Integer)
        Public MustOverride Function ListSubscribers(ByVal propertyID As Integer) As IDataReader

#End Region

#Region " Template Methods "

        Public MustOverride Function GetTemplate(ByVal templateID As Integer) As IDataReader
        Public MustOverride Function GetTemplateByFolder(ByVal folder As String) As IDataReader
        Public MustOverride Function ListTemplate(ByVal portalID As Integer) As IDataReader
        Public MustOverride Function AddTemplate(ByVal title As String, ByVal description As String, ByVal folder As String, ByVal isPremium As Boolean) As Integer
        Public MustOverride Sub UpdateTemplate(ByVal templateID As Integer, ByVal title As String, ByVal description As String, ByVal folder As String, ByVal isPremium As Boolean)
        Public MustOverride Sub DeleteTemplate(ByVal templateID As Integer)

#End Region

#Region " TemplatePortal Methods "

        Public MustOverride Function ListTemplatePortal(ByVal templateID As Integer) As IDataReader
        Public MustOverride Function AddTemplatePortal(ByVal templateID As Integer, ByVal portalID As Integer) As Integer
        Public MustOverride Sub DeleteTemplatePortal(ByVal templateID As Integer)

#End Region

#End Region

    End Class

End Namespace