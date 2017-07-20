Namespace Ventrian.PropertyAgent

    Public Class TemplateInfo

#Region " Private Members "

        Dim _templateID As Integer
        Dim _title As String
        Dim _description As String
        Dim _folder As String
        Dim _isPremium As Boolean

#End Region

#Region " Public Properties "

        Public Property TemplateID() As Integer
            Get
                Return _templateID
            End Get
            Set(ByVal Value As Integer)
                _templateID = Value
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

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal Value As String)
                _description = Value
            End Set
        End Property

        Public Property Folder() As String
            Get
                Return _folder
            End Get
            Set(ByVal Value As String)
                _folder = Value
            End Set
        End Property

        Public Property IsPremium() As Boolean
            Get
                Return _isPremium
            End Get
            Set(ByVal Value As Boolean)
                _isPremium = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
