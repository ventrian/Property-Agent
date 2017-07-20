Namespace Ventrian.PropertyAgent

    Public Class ContactLogInfo

#Region " Private Members "

        Dim _moduleID As Integer
        Dim _dateSent As DateTime
        Dim _sentTo As String
        Dim _sentFrom As String
        Dim _subject As String
        Dim _body As String
        Dim _fieldValues As String
        Dim _propertyID As Integer

#End Region

#Region " Public Properties "

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property DateSent() As DateTime
            Get
                Return _dateSent
            End Get
            Set(ByVal Value As DateTime)
                _dateSent = Value
            End Set
        End Property

        Public Property SentTo() As String
            Get
                Return _sentTo
            End Get
            Set(ByVal Value As String)
                _sentTo = Value
            End Set
        End Property

        Public Property SentFrom() As String
            Get
                Return _sentFrom
            End Get
            Set(ByVal Value As String)
                _sentFrom = Value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal Value As String)
                _subject = Value
            End Set
        End Property

        Public Property Body() As String
            Get
                Return _body
            End Get
            Set(ByVal Value As String)
                _body = Value
            End Set
        End Property

        Public Property FieldValues() As String
            Get
                Return _fieldValues
            End Get
            Set(ByVal Value As String)
                _fieldValues = Value
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

#End Region

    End Class

End Namespace
