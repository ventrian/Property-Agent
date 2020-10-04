Imports System
Imports System.Configuration
Imports System.Data
Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke

Namespace Ventrian.PropertyAgent

    Public Class SqlDataProvider

        Inherits DataProvider

#Region " Private Members "

        Private Const ProviderType As String = "data"

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region " Constructors "

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString()

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region " Properties "

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#End Region

#Region " Public Methods "

#Region " Agent Methods "

        Public Overrides Function ListAgentActive(ByVal moduleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_AgentListActive", moduleID), IDataReader)
        End Function

#End Region

#Region " Broker Methods "

        Public Overrides Function ListBrokers(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal roles As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_BrokerList", portalID, moduleID, roles), IDataReader)
        End Function

        Public Overrides Function ListAgentAvailable(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal roles As String, ByVal brokerID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_BrokerListAvailable", portalID, moduleID, roles, brokerID), IDataReader)
        End Function

        Public Overrides Function ListAgentSelected(ByVal moduleID As Integer, ByVal brokerID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_BrokerListSelected", moduleID, brokerID), IDataReader)
        End Function

        Public Overrides Sub AddBroker(ByVal userID As Integer, ByVal brokerID As Integer, ByVal moduleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_BrokerAddOwner", userID, brokerID, moduleID)
        End Sub

        Public Overrides Sub DeleteBroker(ByVal userID As Integer, ByVal brokerID As Integer, ByVal moduleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_BrokerDeleteOwner", userID, brokerID, moduleID)
        End Sub

#End Region

#Region " Comment Methods "

        Public Overrides Function AddComment(ByVal propertyID As Integer, ByVal parentID As Integer, ByVal userID As Integer, ByVal comment As String, ByVal createDate As DateTime, ByVal name As String, ByVal email As String, ByVal website As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CommentAdd", propertyID, GetNull(parentID), userID, GetNull(comment), createDate, GetNull(name), GetNull(email), GetNull(website)), Integer)
        End Function

        Public Overrides Function ListComment(ByVal propertyID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CommentList", propertyID), IDataReader)
        End Function

        Public Overrides Sub DeleteComment(ByVal commentID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CommentDelete", commentID)
        End Sub

#End Region

#Region " Contact Log Methods "

        Public Overrides Sub AddContactLog(ByVal moduleID As Integer, ByVal dateSent As DateTime, ByVal sentTo As String, ByVal sentFrom As String, ByVal subject As String, ByVal body As String, ByVal fieldValues As String, ByVal propertyID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ContactLogAdd", moduleID, dateSent, sentTo, sentFrom, subject, body, GetNull(fieldValues), GetNull(propertyID))
        End Sub

        Public Overrides Function ListContactLog(ByVal moduleID As Integer, ByVal dateBegin As DateTime, ByVal dateEnd As DateTime) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ContactLogList", moduleID, dateBegin, dateEnd), IDataReader)
        End Function

#End Region

#Region " Contact Methods "

        Public Overrides Function GetContactField(ByVal ContactFieldID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ContactFieldGet", ContactFieldID), IDataReader)
        End Function

        Public Overrides Function ListContactField(ByVal moduleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ContactFieldList", moduleId), IDataReader)
        End Function

        Public Overrides Function AddContactField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer, ByVal customFieldID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ContactFieldAdd", moduleID, name, fieldType, GetNull(fieldElements), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), sortOrder, isRequired, GetNull(length), GetNull(customFieldID)), Integer)
        End Function

        Public Overrides Sub UpdateContactField(ByVal contactFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer, ByVal customFieldID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ContactFieldUpdate", contactFieldID, moduleID, name, fieldType, GetNull(fieldElements), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), sortOrder, isRequired, GetNull(length), GetNull(customFieldID))
        End Sub

        Public Overrides Sub DeleteContactField(ByVal ContactFieldID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ContactFieldDelete", ContactFieldID)
        End Sub

#End Region

#Region " Custom Field Methods "

        Public Overrides Function ExecuteQuery(ByVal query As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, query)
        End Function

        Public Overrides Function GetCustomField(ByVal customFieldID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CustomFieldGet", customFieldID), IDataReader)
        End Function

        Public Overrides Function ListCustomField(ByVal moduleId As Integer, ByVal isPublishedOnly As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CustomFieldList", moduleId, isPublishedOnly), IDataReader)
        End Function

        Public Overrides Function AddCustomField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal fieldElementType As Integer, ByVal fieldElementDropDown As Integer, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isInManager As Boolean, ByVal isSortable As Boolean, ByVal isInListing As Boolean, ByVal isCaptionHidden As Boolean, ByVal isFeatured As Boolean, ByVal isPublished As Boolean, ByVal isHidden As Boolean, ByVal isSearchable As Boolean, ByVal IsLockDown As Boolean, ByVal searchType As Integer, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal validationType As Integer, ByVal fieldElementsFrom As String, ByVal FieldElementsTo As String, ByVal length As Integer, ByVal regularExpression As String, ByVal includeCount As Boolean, ByVal hideZeroCount As Boolean, ByVal inheritSecurity As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CustomFieldAdd", moduleID, name, fieldType, GetNull(fieldElements), fieldElementType, GetNull(fieldElementDropDown), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), isInManager, isSortable, isInListing, isCaptionHidden, isFeatured, isPublished, isHidden, isSearchable, IsLockDown, searchType, sortOrder, isRequired, validationType, GetNull(fieldElementsFrom), GetNull(FieldElementsTo), GetNull(length), GetNull(regularExpression), includeCount, hideZeroCount, inheritSecurity), Integer)
        End Function

        Public Overrides Sub UpdateCustomField(ByVal customFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal fieldElementType As Integer, ByVal fieldElementDropDown As Integer, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal isInManager As Boolean, ByVal isSortable As Boolean, ByVal isInListing As Boolean, ByVal isCaptionHidden As Boolean, ByVal isFeatured As Boolean, ByVal isPublished As Boolean, ByVal isHidden As Boolean, ByVal isSearchable As Boolean, ByVal IsLockDown As Boolean, ByVal searchType As Integer, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal validationType As Integer, ByVal fieldElementsFrom As String, ByVal fieldElementsTo As String, ByVal length As Integer, ByVal regularExpression As String, ByVal includeCount As Boolean, ByVal hideZeroCount As Boolean, ByVal inheritSecurity As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CustomFieldUpdate", customFieldID, moduleID, name, fieldType, GetNull(fieldElements), fieldElementType, GetNull(fieldElementDropDown), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), isInManager, isSortable, isInListing, isCaptionHidden, isFeatured, isPublished, isHidden, isSearchable, IsLockDown, searchType, sortOrder, isRequired, validationType, GetNull(fieldElementsFrom), GetNull(fieldElementsTo), GetNull(length), GetNull(regularExpression), includeCount, hideZeroCount, inheritSecurity)
        End Sub

        Public Overrides Sub DeleteCustomField(ByVal customFieldID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_CustomFieldDelete", customFieldID)
        End Sub

#End Region

#Region " Photo Methods "

        Public Overrides Function GetPhoto(ByVal photoID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PhotoGet", photoID), IDataReader)
        End Function

        Public Overrides Function ListPhoto(ByVal propertyID As Integer, ByVal propertyGuid As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PhotoList", propertyID, GetNull(propertyGuid)), IDataReader)
        End Function

        Public Overrides Function AddPhoto(ByVal propertyID As Integer, ByVal title As String, ByVal filename As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal sortOrder As Integer, ByVal propertyGuid As String, ByVal photoType As Integer, ByVal externalUrl As String, ByVal category As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PhotoAdd", propertyID, GetNull(title), filename, dateCreated, width, height, sortOrder, GetNull(propertyGuid), photoType, GetNull(externalUrl), GetNull(category)), Integer)
        End Function

        Public Overrides Sub UpdatePhoto(ByVal photoID As Integer, ByVal propertyID As Integer, ByVal title As String, ByVal filename As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal sortOrder As Integer, ByVal propertyGuid As String, ByVal photoType As Integer, ByVal externalUrl As String, ByVal category As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PhotoUpdate", photoID, propertyID, GetNull(title), filename, dateCreated, width, height, sortOrder, GetNull(propertyGuid), photoType, GetNull(externalUrl), GetNull(category))
        End Sub

        Public Overrides Sub DeletePhoto(ByVal photoID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PhotoDelete", photoID)
        End Sub

        Public Overrides Sub DeletePhotoByPropertyID(ByVal propertyID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PhotoDeleteByPropertyID", propertyID)
        End Sub

#End Region

#Region " Property Methods "

        Public Overrides Function CountProperty(ByVal moduleID As Integer, ByVal userID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyCount", moduleID, userID), Integer)
        End Function

        Public Overrides Function GetProperty(ByVal propertyID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyGet", propertyID), IDataReader)
        End Function

        Public Overrides Function ListProperty(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal status As Integer, ByVal authorID As Integer, ByVal brokerID As Integer, ByVal isActive As Boolean, ByVal isPending As Boolean, ByVal isExpired As Boolean, ByVal showFeaturedOnly As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal sortBy As Integer, ByVal sortByID As Integer, ByVal sortByIDType As Integer, ByVal sortOrder As Integer, ByVal sortBy2 As Integer, ByVal sortByID2 As Integer, ByVal sortByIDType2 As Integer, ByVal sortOrder2 As Integer, ByVal sortBy3 As Integer, ByVal sortByID3 As Integer, ByVal sortByIDType3 As Integer, ByVal sortOrder3 As Integer, ByVal customFieldIDs As String, ByVal searchValues As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal bubbleFeatured As Boolean, ByVal searchSubTypes As Boolean, ByVal propertyIDForNextPrev As Integer, ByVal latitude As Double, ByVal longitude As Double, ByVal startDate As DateTime, ByVal agentFilter As String, ByVal shortListID As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyList", moduleID, GetNull(propertyTypeID), GetNull(status), GetNull(authorID), GetNull(brokerID), GetNull(isActive), GetNull(isPending), GetNull(isExpired), GetNull(showFeaturedOnly), GetNull(OnlyForAuthenticated), sortBy, GetNull(sortByID), GetNull(sortByIDType), sortOrder, GetNull(sortBy2), GetNull(sortByID2), GetNull(sortByIDType2), sortOrder2, GetNull(sortBy3), GetNull(sortByID3), GetNull(sortByIDType3), sortOrder3, GetNull(customFieldIDs), GetNull(searchValues), pageNumber, pageSize, bubbleFeatured, searchSubTypes, GetNull(propertyIDForNextPrev), GetNull(latitude), GetNull(longitude), GetNull(startDate), GetNull(agentFilter), GetNull(shortListID)), IDataReader)
        End Function

        Public Overrides Function AddProperty(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal isFeatured As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal dateCreated As DateTime, ByVal dateModified As DateTime, ByVal datePublished As DateTime, ByVal dateExpired As DateTime, ByVal viewCount As Integer, ByVal status As Integer, ByVal authorID As Integer, ByVal modifiedID As Integer, ByVal latitude As Double, ByVal longitude As Double) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyAdd", moduleID, propertyTypeID, isFeatured, OnlyForAuthenticated, dateCreated, dateModified, GetNull(datePublished), GetNull(dateExpired), viewCount, status, authorID, modifiedID, GetNull(latitude), GetNull(longitude)), Integer)
        End Function

        Public Overrides Sub UpdateProperty(ByVal propertyID As Integer, ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal isFeatured As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal dateCreated As DateTime, ByVal dateModified As DateTime, ByVal datePublished As DateTime, ByVal dateExpired As DateTime, ByVal viewCount As Integer, ByVal status As Integer, ByVal authorID As Integer, ByVal modifiedID As Integer, ByVal latitude As Double, ByVal longitude As Double)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyUpdate", propertyID, moduleID, propertyTypeID, isFeatured, OnlyForAuthenticated, dateCreated, dateModified, GetNull(datePublished), GetNull(dateExpired), viewCount, status, authorID, modifiedID, GetNull(latitude), GetNull(longitude))
        End Sub


        Public Overrides Sub DeleteProperty(ByVal propertyID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyDelete", propertyID)
        End Sub

        Public Overrides Sub DeletePropertyByModuleID(ByVal moduleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyDeleteByModuleID", moduleID)
        End Sub

#End Region

#Region " Property Type Methods "

        Public Overrides Function ListPropertyTypeAll(ByVal moduleID As Integer, ByVal showPublishedOnly As Boolean, ByVal sortBy As Integer, ByVal agentFilter As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyTypeListAll", moduleID, -1, showPublishedOnly, sortBy, GetNull(agentFilter)), IDataReader)
        End Function

        Public Overrides Function AddPropertyType(ByVal parentID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal description As String, ByVal imageFile As String, ByVal sortOrder As Integer, ByVal isPublished As Boolean, ByVal allowProperties As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyTypeAdd", parentID, moduleID, name, GetNull(description), GetNull(imageFile), sortOrder, isPublished, allowProperties), Integer)
        End Function

        Public Overrides Sub UpdatePropertyType(ByVal propertyTypeID As Integer, ByVal parentID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal description As String, ByVal imageFile As String, ByVal sortOrder As Integer, ByVal isPublished As Boolean, ByVal allowProperties As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyTypeUpdate", propertyTypeID, parentID, moduleID, name, GetNull(description), GetNull(imageFile), sortOrder, isPublished, allowProperties)
        End Sub

        Public Overrides Sub DeletePropertyType(ByVal propertyTypeID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyTypeDelete", propertyTypeID)
        End Sub

#End Region

#Region " Property Value Methods "

        Public Overrides Function GetPropertyValue(ByVal propertyValueID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyValueGet", propertyValueID), IDataReader)
        End Function

        Public Overrides Function GetPropertyValueByField(ByVal propertyID As Integer, ByVal customFieldID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyValueGetByField", propertyID, customFieldID), IDataReader)
        End Function

        Public Overrides Function ListPropertyValue(ByVal propertyID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyValueList", propertyID), IDataReader)
        End Function

        Public Overrides Function ListPropertyValueByField(ByVal customFieldID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyValueListByField", customFieldID), IDataReader)
        End Function

        Public Overrides Function AddPropertyValue(ByVal propertyID As Integer, ByVal customFieldID As Integer, ByVal customValue As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyValueAdd", propertyID, customFieldID, customValue), Integer)
        End Function

        Public Overrides Sub UpdatePropertyValue(ByVal propertyValueID As Integer, ByVal propertyID As Integer, ByVal customFieldID As Integer, ByVal customValue As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyValueUpdate", propertyValueID, propertyID, customFieldID, customValue)
        End Sub

        Public Overrides Sub DeletePropertyValue(ByVal propertyValueID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_PropertyValueDelete", propertyValueID)
        End Sub

#End Region

#Region " Rating Methods "

        Public Overrides Function AddRating(ByVal propertyID As Integer, ByVal userID As Integer, ByVal commentID As String, ByVal reviewID As String, ByVal rating As Double, ByVal createDate As DateTime) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_RatingAdd", propertyID, userID, GetNull(commentID), GetNull(reviewID), rating, createDate), Integer)
        End Function

        Public Overrides Sub DeleteRating(ByVal propertyID As Integer, ByVal reviewID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_RatingDelete", propertyID, reviewID)
        End Sub

        Public Overrides Function GetRating(ByVal propertyID As Integer, ByVal userID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_RatingGet", propertyID, userID), IDataReader)
        End Function

#End Region

#Region " Review Methods "

        Public Overrides Function AddReview(ByVal propertyID As Integer, ByVal userID As Integer, ByVal createDate As DateTime, ByVal isApproved As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewAdd", propertyID, userID, createDate, isApproved), Integer)
        End Function

        Public Overrides Function AddReviewValue(ByVal reviewID As Integer, ByVal reviewFieldID As Integer, ByVal reviewValue As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewValueAdd", reviewID, reviewFieldID, reviewValue), Integer)
        End Function

        Public Overrides Sub DeleteReviewValue(ByVal reviewValueID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewValueDelete", reviewValueID)
        End Sub

        Public Overrides Sub DeleteReview(ByVal reviewID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewDelete", reviewID)
        End Sub

        Public Overrides Function ListReviewValue(ByVal reviewID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewValueList", reviewID), IDataReader)
        End Function

        Public Overrides Function ListReviews(ByVal moduleID As Integer, ByVal propertyID As Integer, ByVal isApproved As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewList", GetNull(moduleID), GetNull(propertyID), isApproved), IDataReader)
        End Function

        Public Overrides Function GetReview(ByVal propertyID As Integer, ByVal userID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewGet", propertyID, userID), IDataReader)
        End Function

        Public Overrides Function GetReviewField(ByVal reviewFieldID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewFieldGet", reviewFieldID), IDataReader)
        End Function

        Public Overrides Function ListReviewField(ByVal moduleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewFieldList", moduleId), IDataReader)
        End Function

        Public Overrides Function AddReviewField(ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewFieldAdd", moduleID, name, fieldType, GetNull(fieldElements), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), sortOrder, isRequired, GetNull(length)), Integer)
        End Function

        Public Overrides Sub UpdateReview(ByVal reviewID As Integer, ByVal isApproved As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewUpdate", reviewID, isApproved)
        End Sub

        Public Overrides Sub UpdateReviewField(ByVal customFieldID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal fieldType As Integer, ByVal fieldElements As String, ByVal defaultValue As String, ByVal caption As String, ByVal captionHelp As String, ByVal sortOrder As Integer, ByVal isRequired As Boolean, ByVal length As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewFieldUpdate", customFieldID, moduleID, name, fieldType, GetNull(fieldElements), GetNull(defaultValue), GetNull(caption), GetNull(captionHelp), sortOrder, isRequired, GetNull(length))
        End Sub

        Public Overrides Sub DeleteReviewField(ByVal reviewFieldID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ReviewFieldDelete", reviewFieldID)
        End Sub

#End Region

#Region " Shortlist Methods "

        Public Overrides Sub AddShortlist(ByVal propertyID As Integer, ByVal userID As Integer, ByVal createDate As DateTime, ByVal userKey As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ShortlistAdd", propertyID, userID, createDate, GetNull(userKey))
        End Sub

        Public Overrides Sub DeleteShortlist(ByVal propertyID As Integer, ByVal userID As Integer, ByVal userKey As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ShortlistDelete", propertyID, userID, GetNull(userKey))
        End Sub

        Public Overrides Function GetShortlist(ByVal propertyID As Integer, ByVal userID As Integer, ByVal userKey As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_ShortlistGet", propertyID, userID, GetNull(userKey)), IDataReader)
        End Function

#End Region

#Region " Statistic Methods "

        Public Overrides Sub AddStatistic(ByVal propertyID As Integer, ByVal userID As Integer, ByVal remoteAddress As String, ByVal moduleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_StatisticAdd", propertyID, GetNull(userID), GetNull(remoteAddress), moduleID)
        End Sub

        Public Overrides Function StatisticGet(ByVal propertyID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_StatisticGet", propertyID), IDataReader)
        End Function

        Public Overrides Function StatisticList(ByVal moduleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_StatisticList", moduleID), IDataReader)
        End Function

#End Region

#Region " Subscribe Methods "

        Public Overrides Sub AddSubscriber(ByVal propertyID As Integer, ByVal userID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_SubscribeAdd", propertyID, GetNull(userID))
        End Sub

        Public Overrides Sub DeleteSubscriber(ByVal propertyID As Integer, ByVal userID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_SubscribeDelete", propertyID, GetNull(userID))
        End Sub

        Public Overrides Function ListSubscribers(ByVal propertyId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_SubscribeList", propertyId), IDataReader)
        End Function

#End Region

#Region " Template Methods "

        Public Overrides Function GetTemplate(ByVal templateID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplateGet", templateID), IDataReader)
        End Function

        Public Overrides Function GetTemplateByFolder(ByVal folder As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplateGetByFolder", folder), IDataReader)
        End Function

        Public Overrides Function ListTemplate(ByVal portalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplateList", GetNull(portalID)), IDataReader)
        End Function

        Public Overrides Function AddTemplate(ByVal title As String, ByVal description As String, ByVal folder As String, ByVal isPremium As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplateAdd", title, GetNull(description), folder, isPremium), Integer)
        End Function

        Public Overrides Sub UpdateTemplate(ByVal templateID As Integer, ByVal title As String, ByVal description As String, ByVal folder As String, ByVal isPremium As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplateUpdate", templateID, title, GetNull(description), folder, isPremium)
        End Sub

        Public Overrides Sub DeleteTemplate(ByVal templateID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplateDelete", templateID)
        End Sub

#End Region

#Region " TemplatePortal Methods "

        Public Overrides Function ListTemplatePortal(ByVal portalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplatePortalList", portalID), IDataReader)
        End Function

        Public Overrides Function AddTemplatePortal(ByVal templateID As Integer, ByVal portalID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplatePortalAdd", templateID, portalID), Integer)
        End Function

        Public Overrides Sub DeleteTemplatePortal(ByVal templateID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_PropertyAgent_TemplatePortalDelete", templateID)
        End Sub

#End Region

#End Region

    End Class

End Namespace