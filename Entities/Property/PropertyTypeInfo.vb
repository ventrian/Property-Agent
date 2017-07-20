Namespace Ventrian.PropertyAgent

    Public Class PropertyTypeInfo

#Region " Private Members "

        Dim _propertyTypeID As Integer
        Dim _parentID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _nameIndented As String
        Dim _description As String
        Dim _imageFile As String
        Dim _sortOrder As Integer
        Dim _isPublished As Boolean
        Dim _allowProperties As Boolean
        Dim _propertyCount As Integer
        Dim _propertyTypeCount As Integer

#End Region

#Region " Public Properties "

        Public Property PropertyTypeID() As Integer
            Get
                Return _propertyTypeID
            End Get
            Set(ByVal Value As Integer)
                _propertyTypeID = Value
            End Set
        End Property

        Public Property ParentID() As Integer
            Get
                Return _parentID
            End Get
            Set(ByVal Value As Integer)
                _parentID = Value
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

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property

        Public ReadOnly Property NameWithCount() As String
            Get
                Return _name & " (" & _propertyCount.ToString() & ")"
            End Get
        End Property

        Public Property NameIndented() As String
            Get
                Return _nameIndented
            End Get
            Set(ByVal Value As String)
                _nameIndented = Value
            End Set
        End Property

        Public ReadOnly Property NameIndentedWithCount() As String
            Get
                Return _nameIndented & " (" & _propertyCount.ToString() & ")"
            End Get
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal Value As String)
                _description = Value
            End Set
        End Property

        Public Property ImageFile() As String
            Get
                Return _imageFile
            End Get
            Set(ByVal Value As String)
                _imageFile = Value
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

        Public Property IsPublished() As Boolean
            Get
                Return _isPublished
            End Get
            Set(ByVal Value As Boolean)
                _isPublished = Value
            End Set
        End Property

        Public Property AllowProperties() As Boolean
            Get
                Return _allowProperties
            End Get
            Set(ByVal Value As Boolean)
                _allowProperties = Value
            End Set
        End Property

        Public Property PropertyCount() As Integer
            Get
                Return _propertyCount
            End Get
            Set(ByVal Value As Integer)
                _propertyCount = Value
            End Set
        End Property

        Public Property PropertyTypeCount() As Integer
            Get
                Return _propertyTypeCount
            End Get
            Set(ByVal Value As Integer)
                _propertyTypeCount = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
