Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security

Namespace Ventrian.PropertyAgent

    Public Class PropertyAgentLatestBase
        Inherits PortalModuleBase
        Implements ICallbackEventHandler

#Region " Private Members "

        Private _propertySettings As PropertySettings
        Private _propertySettingsLatest As PropertySettingsLatest
        Private _customFields As List(Of CustomFieldInfo)

        Private _callbackReturnValue As String = ""

#End Region

#Region " Public Methods "

        Public Sub RegisterRatingCallback(ByVal callbackFunctionName As String, ByVal returnFunctionName As String)

            Dim cbReference As String = Page.ClientScript.GetCallbackEventReference(Me, "arg", returnFunctionName, "context")
            Dim callbackScript As String = vbCrLf & "function " & callbackFunctionName & "(arg, context) {" & cbReference & "} ;"
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), callbackFunctionName, callbackScript, True)

        End Sub

#End Region

#Region " Public Properties "

        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Protected ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Public ReadOnly Property CustomFields() As List(Of CustomFieldInfo)
            Get
                If (_customFields Is Nothing) Then
                    Dim objCustomFieldController As New CustomFieldController
                    _customFields = objCustomFieldController.List(Me.PropertySettingsLatest.PropertyAgentModuleID, True)
                End If

                Return _customFields
            End Get
        End Property

        Public ReadOnly Property PropertySettings() As PropertySettings
            Get
                If (_propertySettings Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettings = New PropertySettings(objModuleController.GetModuleSettings(Me.PropertySettingsLatest.PropertyAgentModuleID))
                End If
                Return _propertySettings
            End Get
        End Property

        Public ReadOnly Property PropertySettingsLatest() As PropertySettingsLatest
            Get
                If (_propertySettingsLatest Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    _propertySettingsLatest = New PropertySettingsLatest(Me.Settings)
                End If
                Return _propertySettingsLatest
            End Get
        End Property

        Public ReadOnly Property ModuleKey() As String
            Get
                Return "PropertyAgentLatest-" & Me.TabModuleId
            End Get
        End Property

#End Region

#Region " Callback Properties "

        Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

            Return _callbackReturnValue

        End Function

        Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

            If (eventArgument.Split("-"c).Length = 2) Then

                Dim objRating As New RatingInfo
                objRating.PropertyID = eventArgument.Split("-"c)(1)
                objRating.Rating = eventArgument.Split("-"c)(0)
                objRating.UserID = Me.UserId
                objRating.CommentID = Null.NullInteger
                objRating.CreateDate = DateTime.Now

                Dim objRatingController As New RatingController()
                objRatingController.Add(objRating)

                If (Page.User.Identity.IsAuthenticated = False) Then
                    Dim objHttpCookie As New HttpCookie("PropertyAgent-Rating-" & objRating.PropertyID.ToString(), "True")
                    Response.Cookies.Add(objHttpCookie)
                End If

                _callbackReturnValue = eventArgument

            End If

        End Sub

#End Region

    End Class

End Namespace
