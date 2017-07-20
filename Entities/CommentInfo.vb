Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class CommentInfo

#Region " Private Members "

        Dim _commentID As Integer
        Dim _parentID As Integer
        Dim _propertyID As Integer
        Dim _userID As Integer
        Dim _comment As String
        Dim _createDate As DateTime
        Dim _name As String
        Dim _email As String
        Dim _website As String
        Dim _username As String
        Dim _rating As Double

#End Region

#Region " Public Properties "

        Public Property CommentID() As Integer
            Get
                Return _commentID
            End Get
            Set(ByVal value As Integer)
                _commentID = value
            End Set
        End Property

        Public Property ParentID() As Integer
            Get
                Return _parentID
            End Get
            Set(ByVal value As Integer)
                _parentID = value
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

        Public Property Comment() As String
            Get
                Return _comment
            End Get
            Set(ByVal value As String)
                _comment = value
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

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property Website() As String
            Get
                Return _website
            End Get
            Set(ByVal value As String)
                _website = value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return _username
            End Get
            Set(ByVal value As String)
                _username = value
            End Set
        End Property

        Public Property Rating() As Double
            Get
                If (_rating >= 0) Then
                    Return _rating
                End If
                Return Null.NullDouble
            End Get
            Set(ByVal value As Double)
                _rating = value
            End Set
        End Property

#End Region

    End Class

End Namespace