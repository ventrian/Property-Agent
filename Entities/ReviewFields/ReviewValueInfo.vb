Namespace Ventrian.PropertyAgent

    Public Class ReviewValueInfo

#Region " Private Members "

        Dim _reviewValueID As Integer
        Dim _reviewID As Integer
        Dim _reviewFieldID As Integer
        Dim _reviewValue As String

#End Region

#Region " Public Properties "

        Public Property ReviewValueID() As Integer
            Get
                Return _reviewValueID
            End Get
            Set(ByVal Value As Integer)
                _reviewValueID = Value
            End Set
        End Property

        Public Property ReviewID() As Integer
            Get
                Return _reviewID
            End Get
            Set(ByVal Value As Integer)
                _reviewID = Value
            End Set
        End Property

        Public Property ReviewFieldID() As Integer
            Get
                Return _reviewFieldID
            End Get
            Set(ByVal Value As Integer)
                _reviewFieldID = Value
            End Set
        End Property

        Public Property ReviewValue() As String
            Get
                Return _reviewValue
            End Get
            Set(ByVal Value As String)
                _reviewValue = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
