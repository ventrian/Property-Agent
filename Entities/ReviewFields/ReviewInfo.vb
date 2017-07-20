Namespace Ventrian.PropertyAgent

    Public Class ReviewInfo

#Region " Private Members "

        Dim _reviewID As Integer
        Dim _propertyID As Integer
        Dim _userID As Integer
        Dim _createDate As DateTime
        Dim _isApproved As Boolean

        Dim _displayName As String
        Dim _email As String
        Dim _username As String

#End Region

#Region " Public Properties "

        Public Property ReviewID() As Integer
            Get
                Return _reviewID
            End Get
            Set(ByVal Value As Integer)
                _reviewID = Value
            End Set
        End Property

        Public Property PropertyID() As Integer
            Get
                Return _propertyID
            End Get
            Set(ByVal Value As Integer)
                _propertyID = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal Value As Integer)
                _userID = Value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return _createDate
            End Get
            Set(ByVal Value As DateTime)
                _createDate = Value
            End Set
        End Property

        Public Property IsApproved() As Boolean
            Get
                Return _isApproved
            End Get
            Set(ByVal Value As Boolean)
                _isApproved = Value
            End Set
        End Property

        Public Property DisplayName() As String
            Get
                Return _displayName
            End Get
            Set(ByVal Value As String)
                _displayName = Value
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

        Public Property Username() As String
            Get
                Return _username
            End Get
            Set(ByVal Value As String)
                _username = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
