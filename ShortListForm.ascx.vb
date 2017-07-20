Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class ShortListForm
        Inherits PropertyAgentControl

#Region " Private Properties"

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/ShortListForm"
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

        Private ReadOnly Property UserKey() As String
            Get
                If (Request.IsAuthenticated) Then
                    Return ""
                Else
                    If (Request.Cookies("ShortList-PA-" & CurrentProperty.ModuleID.ToString()) Is Nothing) Then
                        Response.Cookies("ShortList-PA-" & CurrentProperty.ModuleID.ToString()).Value = Guid.NewGuid().ToString()
                    End If

                    Return Request.Cookies("ShortList-PA-" & CurrentProperty.ModuleID.ToString()).Value
                End If
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                Dim shortListLabel As String = Constants.SHORTLIST_LABEL_SETTING_DEFAULT
                If (PropertyAgentBase IsNot Nothing) Then
                    shortListLabel = PropertySettings.ShortListLabel
                    cmdAddShortList.CssClass = PropertySettings.ButtonClass
                    cmdRemoveShortList.CssClass = PropertySettings.ButtonClass
                Else
                    If (PropertyAgentLatestBase IsNot Nothing) Then
                        shortListLabel = PropertyAgentLatestBase.PropertySettings.ShortListLabel
                        cmdAddShortList.CssClass = PropertyAgentLatestBase.PropertySettings.ButtonClass
                        cmdRemoveShortList.CssClass = PropertyAgentLatestBase.PropertySettings.ButtonClass
                    End If
                End If

                cmdAddShortList.Text = Localization.GetString("cmdAddShortList", ResourceFile)
                cmdAddShortList.Text = cmdAddShortList.Text.Replace("[SHORTLIST]", shortListLabel)
                cmdRemoveShortList.Text = Localization.GetString("cmdRemoveShortList", ResourceFile)
                cmdRemoveShortList.Text = cmdRemoveShortList.Text.Replace("[SHORTLIST]", shortListLabel)

                Dim objShortListController As New ShortListController()
                Dim objShortList As ShortListInfo = objShortListController.Get(CurrentProperty.PropertyID, UserID, UserKey)

                If (objShortList Is Nothing) Then
                    ' Add Item
                    cmdRemoveShortList.Visible = False
                Else
                    ' Remove Item
                    cmdAddShortList.Visible = False
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddShortList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddShortList.Click

            Try

                Dim objShortList As New ShortListInfo()

                objShortList.CreateDate = DateTime.Now
                objShortList.PropertyID = CurrentProperty.PropertyID
                objShortList.UserID = UserID
                objShortList.UserKey = UserKey

                Dim objShortListController As New ShortListController()
                objShortListController.Add(objShortList)

                cmdRemoveShortList.Visible = True
                cmdAddShortList.Visible = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

            Response.Redirect(Request.RawUrl)

        End Sub

        Private Sub cmdRemoveShortList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveShortList.Click

            Try

                Dim objShortListController As New ShortListController()
                objShortListController.Delete(CurrentProperty.PropertyID, UserID, UserKey)

                cmdRemoveShortList.Visible = False
                cmdAddShortList.Visible = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

            Response.Redirect(Request.RawUrl)

        End Sub

#End Region

    End Class

End Namespace