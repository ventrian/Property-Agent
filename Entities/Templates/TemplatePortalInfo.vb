Namespace Ventrian.PropertyAgent

    Public Class TemplatePortalInfo

#Region " Private Members "

        Dim _templatePortalID As Integer
        Dim _templateID As Integer
        Dim _portalID As Integer
        Dim _portalName As String

#End Region

#Region " Public Properties "

        Public Property TemplatePortalID() As Integer
            Get
                Return _templatePortalID
            End Get
            Set(ByVal Value As Integer)
                _templatePortalID = Value
            End Set
        End Property

        Public Property TemplateID() As Integer
            Get
                Return _templateID
            End Get
            Set(ByVal Value As Integer)
                _templateID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _portalID
            End Get
            Set(ByVal Value As Integer)
                _portalID = Value
            End Set
        End Property

        Public Property PortalName() As String
            Get
                Return _portalName
            End Get
            Set(ByVal Value As String)
                _portalName = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
