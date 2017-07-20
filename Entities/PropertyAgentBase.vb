Imports System.ComponentModel
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Public Class PropertyAgentBase
        Inherits PortalModuleBase
        Implements ICallbackEventHandler

#Region " Private Members "

        Private _customFields As List(Of CustomFieldInfo)
        Private _objTemplatePhotoItem As LayoutInfo
        Private _propertySettings As PropertySettings
        Private _layoutController As LayoutController

        Private _callbackReturnValue As String = ""

#End Region

#Region " Public Properties "

        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Protected ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

#End Region

#Region " Public Methods "

        Public Function GetPhotoPath(ByVal dataItem As Object) As String

            Dim objTemplateController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)
            Return objTemplateController.GetPhotoPath(dataItem, False)

        End Function

        Public Function GetPhotoPathCropped(ByVal dataItem As Object) As String

            Dim objTemplateController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)
            Return objTemplateController.GetPhotoPath(dataItem, True, 125, 125)

        End Function

        Public Sub RegisterRatingCallback(ByVal callbackFunctionName As String, ByVal returnFunctionName As String)

            Dim cbReference As String = Page.ClientScript.GetCallbackEventReference(Me, "arg", returnFunctionName, "context")
            Dim callbackScript As String = vbCrLf & "function " & callbackFunctionName & "(arg, context) {" & cbReference & "} ;"
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), callbackFunctionName, callbackScript, True)

        End Sub

#End Region

#Region " Protected Methods "

        Public Function GetResourceString(ByVal key As String) As String

            Return GetResourceString(key, Me.LocalResourceFile, PropertySettings)

        End Function

        Public Function GetResourceString(ByVal key As String, ByVal resourceFile As String, ByVal propertySettings As PropertySettings) As String

            Return PropertyUtil.FormatPropertyLabel(Localization.GetString(key, resourceFile), propertySettings)

        End Function

        Public Function CheckLimit() As Boolean

            Dim limit As Integer = GetLimit(PropertySettings.PermissionSubmit, PropertySettings.PermissionLimit)

            If (limit = Null.NullInteger) Then
                Return True
            End If

            Dim objPropertyController As New PropertyController
            Dim count As Integer = objPropertyController.Count(Me.ModuleId, Me.UserId)

            If (count >= limit) Then
                Return False
            Else
                Return True
            End If

        End Function

        Public Function GetLimit(ByVal permission As String, ByVal permissionLimit As String) As Integer

            Dim limit As Integer = Null.NullInteger

            For Each role As String In permission.Split(";"c)

                If (role <> "") Then

                    Dim actualRole As String = role

                    If (actualRole.Split(":"c).Length > 1) Then
                        actualRole = actualRole.Split(":"c)(0)
                    End If

                    If (PortalSecurity.IsInRole(actualRole)) Then

                        Dim found As Boolean = False

                        If (permissionLimit = "") Then
                            Return Null.NullInteger
                        End If

                        For Each item As String In permissionLimit.Split(";"c)
                            Dim r As String = item.Split(":"c)(0)
                            Dim v As Integer = Convert.ToInt32(item.Split(":"c)(1))

                            If (actualRole = r) Then
                                If (limit < v) Then
                                    limit = v
                                End If
                                found = True
                            End If
                        Next

                        If (found = False) Then
                            Return Null.NullInteger
                        End If

                    End If

                End If

            Next

            Return limit

        End Function

#End Region

#Region " Protected Properties "

        Public ReadOnly Property PropertySettings() As PropertySettings
            Get
                If (_propertySettings Is Nothing) Then
                    _propertySettings = New PropertySettings(Me.Settings)
                End If
                Return _propertySettings
            End Get
        End Property

        Public ReadOnly Property PropertySettings(ByVal forceRefresh As Boolean) As PropertySettings
            Get
                Dim objModuleController As New ModuleController
                _propertySettings = New PropertySettings(objModuleController.GetModuleSettings(Me.ModuleId))
                Return _propertySettings
            End Get
        End Property

        Public ReadOnly Property CustomFields() As List(Of CustomFieldInfo)
            Get
                If (_customFields Is Nothing) Then
                    Dim objCustomFieldController As New CustomFieldController
                    _customFields = (objCustomFieldController.List(ModuleId, True))
                End If

                Return _customFields
            End Get
        End Property

        Protected ReadOnly Property IsEditor() As Boolean
            Get
                Return _
                        (PortalSecurity.IsInRoles(Me.ModuleConfiguration.AuthorizedEditRoles) = True) Or _
                        (PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles) = True) Or _
                        (PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) = True)
            End Get
        End Property

        Public ReadOnly Property ModuleKey() As String
            Get
                Return "PropertyAgent-" & Me.TabModuleId
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
