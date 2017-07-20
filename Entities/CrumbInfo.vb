Namespace Ventrian.PropertyAgent

    Public Class CrumbInfo

#Region " Private Members "

        Dim _caption As String
        Dim _url As String

#End Region

#Region " Public Properties "

        Public Property Caption() As String
            Get
                Return _caption
            End Get
            Set(ByVal Value As String)
                _caption = Value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal Value As String)
                _url = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
