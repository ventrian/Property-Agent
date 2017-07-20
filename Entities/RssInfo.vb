Namespace Ventrian.PropertyAgent

    Public Class RssInfo

#Region " Private Members "

        Dim _title As String
        Dim _description As String
        Dim _link As String

#End Region

#Region " Public Properties "

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return _link
            End Get
            Set(ByVal value As String)
                _link = value
            End Set
        End Property

#End Region

    End Class

End Namespace
