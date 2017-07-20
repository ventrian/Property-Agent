Imports DotNetNuke.Entities.Portals

Namespace Ventrian.PropertyAgent

    Public Class PropertyAgentControl
        Inherits System.Web.UI.UserControl

#Region " Private Properties "

        Dim _objProperty As PropertyInfo
        Dim _objComment As CommentInfo
        Dim _objReview As ReviewInfo

        Dim _moduleID As Integer
        Dim _tabID As Integer
        Dim _moduleKey As String

#End Region

#Region " Protected Properties "

        Protected ReadOnly Property PropertyAgentBase() As PropertyAgentBase
            Get
                Dim objParent As Control = Parent

                While objParent IsNot Nothing
                    If (TypeOf objParent Is PropertyAgentBase) Then
                        Return CType(objParent, PropertyAgentBase)
                    Else
                        objParent = objParent.Parent
                    End If
                End While

                Return Nothing
            End Get
        End Property

        Protected ReadOnly Property PropertyAgentLatestBase() As PropertyAgentLatestBase
            Get
                Dim objParent As Control = Parent

                While objParent IsNot Nothing
                    If (TypeOf objParent Is PropertyAgentLatestBase) Then
                        Return CType(objParent, PropertyAgentLatestBase)
                    Else
                        objParent = objParent.Parent
                    End If
                End While

                Return Nothing
            End Get
        End Property

        Protected ReadOnly Property CustomFields() As List(Of CustomFieldInfo)
            Get
                Return PropertyAgentBase.CustomFields
            End Get
        End Property

        Protected ReadOnly Property PortalSettings() As PortalSettings
            Get
                Return PropertyAgentBase.PortalSettings
            End Get
        End Property

        Protected ReadOnly Property PropertySettings() As PropertySettings
            Get
                Return PropertyAgentBase.PropertySettings
            End Get
        End Property

#End Region

#Region " Public Properties "

        Public Property CurrentProperty() As PropertyInfo
            Get
                Return _objProperty
            End Get
            Set(ByVal Value As PropertyInfo)
                _objProperty = Value
            End Set
        End Property

        Public Property CurrentComment() As CommentInfo
            Get
                Return _objComment
            End Get
            Set(ByVal Value As CommentInfo)
                _objComment = Value
            End Set
        End Property

        Public Property CurrentReview() As ReviewInfo
            Get
                Return _objReview
            End Get
            Set(ByVal Value As ReviewInfo)
                _objReview = Value
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

        Public Property TabID() As Integer
            Get
                Return _tabID
            End Get
            Set(ByVal Value As Integer)
                _tabID = Value
            End Set
        End Property

        Public Property ModuleKey() As String
            Get
                Return _moduleKey
            End Get
            Set(ByVal Value As String)
                _moduleKey = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
