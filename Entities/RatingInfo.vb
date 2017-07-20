Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class RatingInfo

#Region " Private Members "

        Dim _ratingID As Integer
        Dim _propertyID As Integer
        Dim _userID As Integer
        Dim _commentID As Integer
        Dim _reviewID As Integer = Null.NullInteger
        Dim _rating As Double
        Dim _createDate As DateTime

#End Region

#Region " Public Properties "

        Public Property RatingID() As Integer
            Get
                Return _ratingID
            End Get
            Set(ByVal value As Integer)
                _ratingID = value
            End Set
        End Property

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

        Public Property CommentID() As Integer
            Get
                Return _commentID
            End Get
            Set(ByVal value As Integer)
                _commentID = value
            End Set
        End Property

        Public Property ReviewID() As Integer
            Get
                Return _reviewID
            End Get
            Set(ByVal value As Integer)
                _reviewID = value
            End Set
        End Property

        Public Property Rating() As Double
            Get
                Return _rating
            End Get
            Set(ByVal value As Double)
                _rating = value
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

#End Region

    End Class

End Namespace