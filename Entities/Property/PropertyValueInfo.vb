Namespace Ventrian.PropertyAgent

    Public Class PropertyValueInfo

#Region " Private Members "

        Dim _propertyValueID As Integer
        Dim _propertyID As Integer
        Dim _customFieldID As Integer
        Dim _customValue As String

#End Region

#Region " Public Properties "

        Public Property PropertyValueID() As Integer
            Get
                Return _propertyValueID
            End Get
            Set(ByVal Value As Integer)
                _propertyValueID = Value
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

        Public Property CustomFieldID() As Integer
            Get
                Return _customFieldID
            End Get
            Set(ByVal Value As Integer)
                _customFieldID = Value
            End Set
        End Property

        Public Property CustomValue() As String
            Get
                Return _customValue
            End Get
            Set(ByVal Value As String)
                _customValue = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
