Namespace Ventrian.PropertyAgent

    Public Class ContactFieldInfo

#Region " Private Members "

        Dim _ContactFieldID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _fieldType As ContactFieldType
        Dim _fieldElements As String
        Dim _defaultValue As String
        Dim _caption As String
        Dim _captionHelp As String
        Dim _sortOrder As Integer
        Dim _isRequired As Boolean
        Dim _length As Integer
        Dim _customFieldID As Integer

#End Region

#Region " Public Properties "

        Public Property ContactFieldID() As Integer
            Get
                Return _ContactFieldID
            End Get
            Set(ByVal Value As Integer)
                _ContactFieldID = Value
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

        Public Property FieldType() As ContactFieldType
            Get
                Return _fieldType
            End Get
            Set(ByVal Value As ContactFieldType)
                _fieldType = Value
            End Set
        End Property

        Public Property FieldElements() As String
            Get
                Return _fieldElements
            End Get
            Set(ByVal Value As String)
                _fieldElements = Value
            End Set
        End Property

        Public Property DefaultValue() As String
            Get
                Return _defaultValue
            End Get
            Set(ByVal Value As String)
                _defaultValue = Value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return _caption
            End Get
            Set(ByVal Value As String)
                _caption = Value
            End Set
        End Property

        Public Property CaptionHelp() As String
            Get
                Return _captionHelp
            End Get
            Set(ByVal Value As String)
                _captionHelp = Value
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

        Public Property IsRequired() As Boolean
            Get
                Return _isRequired
            End Get
            Set(ByVal Value As Boolean)
                _isRequired = Value
            End Set
        End Property

        Public Property Length() As Integer
            Get
                Return _length
            End Get
            Set(ByVal Value As Integer)
                _length = Value
            End Set
        End Property

        Public Property CustomFieldID() As Integer
            Get
                Return _customFieldID
            End Get
            Set(ByVal Value As Integer)
                _customFieldID = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
