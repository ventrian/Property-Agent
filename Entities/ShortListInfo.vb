
Namespace Ventrian.PropertyAgent

    Public Class ShortListInfo

#Region " Private Members "

        Dim _propertyID As Integer
        Dim _userID As Integer
        Dim _createDate As DateTime
        Dim _userKey As String

#End Region

#Region " Public Properties "

        Public Property PropertyID() As Integer
            Get
                Return _propertyID
            End Get
            Set(ByVal value As Integer)
                _propertyID = value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal value As Integer)
                _userID = value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return _createDate
            End Get
            Set(ByVal value As DateTime)
                _createDate = value
            End Set
        End Property

        Public Property UserKey() As String
            Get
                Return _userKey
            End Get
            Set(ByVal value As String)
                _userKey = value
            End Set
        End Property

#End Region

    End Class

End Namespace
