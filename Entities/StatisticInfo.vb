Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent
    Public Class StatisticInfo
#Region " Private Members "

        Dim _propertyID As Integer
        Dim _dateCreated As DateTime
        Dim _userID As Integer
        Dim _remoteAddress As String

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

        Public Property DateCreated() As DateTime
            Get
                Return _dateCreated
            End Get
            Set(ByVal value As DateTime)
                _dateCreated = value
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
        Public Property RemoteAddress() As String
            Get
                Return _remoteAddress
            End Get
            Set(ByVal value As String)
                _remoteAddress = value
            End Set
        End Property
#End Region
    End Class
End Namespace

