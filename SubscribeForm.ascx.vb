Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Public Class SubscribeForm
        Inherits PropertyAgentControl

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/SubscribeForm"
            End Get
        End Property

        Private ReadOnly Property UserID() As Integer
            Get
                Dim ID As Integer = Null.NullInteger
                If (Request.IsAuthenticated) Then
                    Dim objUser As UserInfo = UserController.GetCurrentUserInfo()
                    If (objUser IsNot Nothing) Then
                        ID = objUser.UserID
                    End If
                End If
                Return ID
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                If (Request.IsAuthenticated = False) Then
                    Me.Visible = False
                    Return
                End If

                If (IsPostBack = False) Then

                    chkSubscriber.Text = Localization.GetString("Subscribe", ResourceFile)


                    Dim objSubscriberController As New SubscriberController
                    Dim objSubscribers As List(Of UserInfo) = objSubscriberController.ListSubscribers(CurrentProperty.PropertyID)

                    For Each objSubscriber As UserInfo In objSubscribers
                        If (objSubscriber.UserID = UserID) Then
                            chkSubscriber.Checked = True
                            chkSubscriber.Text = Localization.GetString("Unsubscribe", ResourceFile)
                        End If
                    Next

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Protected Sub chkSubscriber_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSubscriber.CheckedChanged

            Try

                Dim objSubscriberController As New SubscriberController

                If (chkSubscriber.Checked) Then
                    ' Add
                    objSubscriberController.AddSubscriber(CurrentProperty.PropertyID, UserID)
                    chkSubscriber.Text = Localization.GetString("Unsubscribe", ResourceFile)
                Else
                    ' Remove
                    objSubscriberController.DeleteSubscriber(CurrentProperty.PropertyID, UserID)
                    chkSubscriber.Text = Localization.GetString("Subscribe", ResourceFile)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

    End Class

End Namespace