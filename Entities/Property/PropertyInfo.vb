Imports System.Collections.Specialized

Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class PropertyInfo

#Region " Private Members "

        Dim _propertyID As Integer = Null.NullInteger
        Dim _moduleID As Integer
        Dim _propertyTypeID As Integer
        Dim _isApproved As Boolean
        Dim _isFeatured As Boolean
        Dim _isPublished As Boolean
        Dim _isArchived As Boolean
        Dim _dateCreated As DateTime
        Dim _dateModified As DateTime
        Dim _datePublished As DateTime
        Dim _dateExpired As DateTime
        Dim _viewCount As Integer
        Dim _status As StatusType = StatusType.Draft
        Dim _authorID As Integer
        Dim _photoCount As Integer
        Dim _rating As Double
        Dim _ratingCount As Integer
        Dim _commentCount As Integer
        Dim _reviewCount As Integer = Null.NullInteger

        Dim _latitude As Double
        Dim _longitude As Double
        Dim _distance As Double

        Dim _username As String
        Dim _displayname As String
        Dim _fullname As String
        Dim _email As String

        Dim _brokerID As Integer = Null.NullInteger
        Dim _brokerUsername As String
        Dim _brokerDisplayName As String
        Dim _brokerEmail As String

        Dim _modifiedID As Integer
        Dim _modifiedUsername As String
        Dim _modifiedDisplayName As String
        Dim _modifiedEmail As String

        Dim _propertyTypeName As String
        Dim _propertyTypeDescription As String
        Dim _propertyList As Hashtable
        Dim _reviewList As Hashtable

        Dim _firstPhotoID As Integer
        Dim _firstPhoto As PhotoInfo

#End Region

#Region " Public Properties "

        Public Property PropertyID() As Integer
            Get
                Return _propertyID
            End Get
            Set(ByVal Value As Integer)
                _propertyID = Value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property PropertyTypeID() As Integer
            Get
                Return _propertyTypeID
            End Get
            Set(ByVal Value As Integer)
                _propertyTypeID = Value
            End Set
        End Property

        Public Property IsFeatured() As Boolean
            Get
                Return _isFeatured
            End Get
            Set(ByVal Value As Boolean)
                _isFeatured = Value
            End Set
        End Property

        Public Property DateCreated() As DateTime
            Get
                Return _dateCreated
            End Get
            Set(ByVal Value As DateTime)
                _dateCreated = Value
            End Set
        End Property

        Public Property DateModified() As DateTime
            Get
                Return _dateModified
            End Get
            Set(ByVal Value As DateTime)
                _dateModified = Value
            End Set
        End Property

        Public Property DatePublished() As DateTime
            Get
                Return _datePublished
            End Get
            Set(ByVal Value As DateTime)
                _datePublished = Value
            End Set
        End Property

        Public Property DateExpired() As DateTime
            Get
                Return _dateExpired
            End Get
            Set(ByVal Value As DateTime)
                _dateExpired = Value
            End Set
        End Property

        Public Property ViewCount() As Integer
            Get
                Return _viewCount
            End Get
            Set(ByVal Value As Integer)
                _viewCount = Value
            End Set
        End Property

        Public Property PropertyTypeName() As String
            Get
                Return _propertyTypeName
            End Get
            Set(ByVal Value As String)
                _propertyTypeName = Value
            End Set
        End Property

        Public Property PropertyTypeDescription() As String
            Get
                Return _propertyTypeDescription
            End Get
            Set(ByVal Value As String)
                _propertyTypeDescription = Value
            End Set
        End Property

        Public ReadOnly Property PropertyList() As Hashtable
            Get
                If (_propertyList Is Nothing) Then
                    InitializePropertyList()
                End If
                Return _propertyList
            End Get
        End Property

        Public ReadOnly Property ReviewList(ByVal reviewID As Integer) As Hashtable
            Get
                If (_reviewList Is Nothing) Then
                    InitializeReviewList(reviewID)
                End If
                Return _reviewList
            End Get
        End Property

        Public Property Status() As StatusType
            Get
                Return _status
            End Get
            Set(ByVal Value As StatusType)
                _status = Value
            End Set
        End Property

        Public Property PhotoCount() As Integer
            Get
                Return _photoCount
            End Get
            Set(ByVal Value As Integer)
                _photoCount = Value
            End Set
        End Property

        Public Property Rating() As Double
            Get
                If (_rating >= 0) Then
                    Return _rating
                End If
                Return Null.NullDouble
            End Get
            Set(ByVal Value As Double)
                _rating = Value
            End Set
        End Property

        Public Property RatingCount() As Integer
            Get
                Return _ratingCount
            End Get
            Set(ByVal Value As Integer)
                _ratingCount = Value
            End Set
        End Property

        Public Property CommentCount() As Integer
            Get
                Return _commentCount
            End Get
            Set(ByVal Value As Integer)
                _commentCount = Value
            End Set
        End Property

        Public ReadOnly Property ReviewCount() As Integer
            Get
                If (_propertyID = Null.NullInteger) Then
                    Return 0
                End If
                If (_reviewCount = Null.NullInteger) Then
                    Dim objReviewController As New ReviewController()
                    _reviewCount = objReviewController.List(Null.NullInteger, _propertyID, True).Count
                End If
                Return _reviewCount
            End Get
        End Property

        Public Property Latitude() As Double
            Get
                Return _latitude
            End Get
            Set(ByVal Value As Double)
                _latitude = Value
            End Set
        End Property

        Public Property Longitude() As Double
            Get
                Return _longitude
            End Get
            Set(ByVal Value As Double)
                _longitude = Value
            End Set
        End Property

        Public Property Distance() As Double
            Get
                Return _distance
            End Get
            Set(ByVal Value As Double)
                _distance = Value
            End Set
        End Property

        Public Property FirstPhotoID() As Integer
            Get
                Return _firstPhotoID
            End Get
            Set(ByVal Value As Integer)
                _firstPhotoID = Value
            End Set
        End Property

        Public ReadOnly Property FirstPhoto() As PhotoInfo
            Get
                If (_firstPhoto Is Nothing) Then
                    If (FirstPhotoID <> Null.NullInteger) Then
                        Dim objPhotoController As New PhotoController
                        _firstPhoto = objPhotoController.Get(FirstPhotoID)
                        Return _firstPhoto
                    Else
                        Return Nothing
                    End If
                Else
                    Return _firstPhoto
                End If
            End Get
        End Property

        Public Property AuthorID() As Integer
            Get
                Return _authorID
            End Get
            Set(ByVal Value As Integer)
                _authorID = Value
            End Set
        End Property

        Public Property DisplayName() As String
            Get
                Return _displayname
            End Get
            Set(ByVal Value As String)
                _displayname = Value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return _username
            End Get
            Set(ByVal Value As String)
                _username = Value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal Value As String)
                _email = Value
            End Set
        End Property

        Public Property BrokerID() As Integer
            Get
                Return _brokerID
            End Get
            Set(ByVal Value As Integer)
                _brokerID = Value
            End Set
        End Property

        Public Property BrokerDisplayName() As String
            Get
                Return _brokerDisplayName
            End Get
            Set(ByVal Value As String)
                _brokerDisplayName = Value
            End Set
        End Property

        Public Property BrokerUsername() As String
            Get
                Return _brokerUsername
            End Get
            Set(ByVal Value As String)
                _brokerUsername = Value
            End Set
        End Property

        Public Property BrokerEmail() As String
            Get
                Return _brokerEmail
            End Get
            Set(ByVal Value As String)
                _brokerEmail = Value
            End Set
        End Property

        Public Property ModifiedID() As Integer
            Get
                Return _modifiedID
            End Get
            Set(ByVal Value As Integer)
                _modifiedID = Value
            End Set
        End Property

        Public Property ModifiedDisplayName() As String
            Get
                Return _modifiedDisplayName
            End Get
            Set(ByVal Value As String)
                _modifiedDisplayName = Value
            End Set
        End Property

        Public Property ModifiedUsername() As String
            Get
                Return _modifiedUsername
            End Get
            Set(ByVal Value As String)
                _modifiedUsername = Value
            End Set
        End Property

        Public Property ModifiedEmail() As String
            Get
                Return _modifiedEmail
            End Get
            Set(ByVal Value As String)
                _modifiedEmail = Value
            End Set
        End Property

#End Region

#Region " Private Methods "

        Private Sub InitializePropertyList()

            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleID, True)

            Dim objPropertyValueController As New PropertyValueController
            Dim objPropertyValues As List(Of PropertyValueInfo) = objPropertyValueController.List(Me.PropertyID, Me.ModuleID)

            _propertyList = New Hashtable

            For Each objCustomField As CustomFieldInfo In objCustomFields
                Dim value As String = ""
                For Each objPropertyValue As PropertyValueInfo In objPropertyValues
                    If (objPropertyValue.CustomFieldID = objCustomField.CustomFieldID) Then
                        value = objPropertyValue.CustomValue
                    End If
                Next
                _propertyList.Add(objCustomField.CustomFieldID, value)
            Next

        End Sub

        Private Sub InitializeReviewList(ByVal reviewID As Integer)

            Dim objReviewFieldController As New ReviewFieldController
            Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(Me.ModuleID)

            Dim objReviewValueController As New ReviewController
            Dim objReviewValues As List(Of ReviewValueInfo) = objReviewValueController.ListValue(reviewID)

            _reviewList = New Hashtable

            For Each objReviewField As ReviewFieldInfo In objReviewFields
                Dim value As String = ""
                For Each objReviewValue As ReviewValueInfo In objReviewValues
                    If (objReviewValue.ReviewFieldID = objReviewField.ReviewFieldID) Then
                        value = objReviewValue.ReviewValue
                    End If
                Next
                _reviewList.Add(objReviewField.ReviewFieldID, value)
            Next

        End Sub

        Private Shared Function TidyOutput(ByVal input As String) As String

            Return input.Replace(",", " ")

        End Function

        Private Shared Function GetCustomListCSVHeader(ByVal moduleID As Integer) As String

            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(moduleID, True)

            Dim value As String = ""
            For Each objCustomField As CustomFieldInfo In objCustomFields
                If (value = "") Then
                    value = TidyOutput(objCustomField.Name) & ","
                Else
                    value = value & TidyOutput(objCustomField.Name) & ","
                End If
            Next

            Return value

        End Function

        Private Function GetCustomListCSV() As String

            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleID, True)

            Dim value As String = ""
            For Each objCustomField As CustomFieldInfo In objCustomFields
                If (PropertyList.ContainsKey(objCustomField.CustomFieldID)) Then
                    If (value = "") Then
                        value = TidyOutput(PropertyList(objCustomField.CustomFieldID).ToString()) & ","
                    Else
                        value = value & TidyOutput(PropertyList(objCustomField.CustomFieldID).ToString()) & ","
                    End If
                Else
                    If (value = "") Then
                        value = ","
                    Else
                        value = value & ","
                    End If
                End If
            Next

            Return value

        End Function

#End Region

#Region " Public Methods "

        Public Shared Function ToStringHeader(ByVal moduleID As Integer) As String

            Return "" _
                & "Name," _
                & "Property ID," _
                & "Date Published," _
                & "Featured," _
                & "Type," _
                & GetCustomListCSVHeader(moduleID) _
                & vbCrLf

        End Function

        Public Overrides Function ToString() As String

            Return "" _
                & TidyOutput(Me.DisplayName) & "," _
                & TidyOutput(Me.PropertyID.ToString()) & "," _
                & TidyOutput(Me.DatePublished.ToString()) & "," _
                & TidyOutput(Me.IsFeatured.ToString()) & "," _
                & TidyOutput(Me.PropertyTypeName.ToString()) & "," _
                & GetCustomListCSV() _
                & vbCrLf

        End Function
#End Region

    End Class

End Namespace
