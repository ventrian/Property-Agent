Namespace Ventrian.PropertyAgent

    Public Class PhotoInfo

#Region " Private Members "

        Dim _photoID As Integer
        Dim _photoType As PhotoType
        Dim _propertyID As Integer
        Dim _title As String
        Dim _filename As String
        Dim _dateCreated As DateTime
        Dim _width As Integer
        Dim _height As Integer
        Dim _sortOrder As Integer
        Dim _propertyGuid As String
        Dim _externalUrl As String
        Dim _category As String

#End Region

#Region " Public Properties "

        Public Property PhotoID() As Integer
            Get
                Return _photoID
            End Get
            Set(ByVal Value As Integer)
                _photoID = Value
            End Set
        End Property

        Public Property PhotoType() As PhotoType
            Get
                Return _photoType
            End Get
            Set(ByVal value As PhotoType)
                _photoType = value
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

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property

        Public Property Filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal Value As String)
                _filename = Value
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

        Public Property Width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal Value As Integer)
                _width = Value
            End Set
        End Property

        Public Property Height() As Integer
            Get
                Return _height
            End Get
            Set(ByVal Value As Integer)
                _height = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _sortOrder
            End Get
            Set(ByVal Value As Integer)
                _sortOrder = Value
            End Set
        End Property

        Public Property PropertyGuid() As String
            Get
                Return _propertyGuid
            End Get
            Set(ByVal Value As String)
                _propertyGuid = Value
            End Set
        End Property

        Public Property ExternalUrl() As String
            Get
                Return _externalUrl
            End Get
            Set(ByVal Value As String)
                _externalUrl = Value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return _category
            End Get
            Set(ByVal Value As String)
                _category = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
