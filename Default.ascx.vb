'
' Property Agent for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2007
' by Ventrian Systems ( support@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security
Imports DotNetNuke.Web.Client.ClientResourceManagement

Namespace Ventrian.PropertyAgent

    Partial Public Class _Default
        Inherits PropertyAgentBase
        Implements IActionable

#Region " Private Members "

        Private _controlToLoad As String

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            Dim doRedirect As Boolean = False
            For Each key As String In Request.QueryString.AllKeys
                If (key IsNot Nothing) Then
                    If ((key.ToLower() = "agenttype" And PropertySettings.SEOAgentType.ToLower() <> "agenttype") Or (key.ToLower() = "propertyid" And PropertySettings.SEOPropertyID.ToLower() <> "propertyid") Or (key.ToLower() = "propertytypeid" And PropertySettings.SEOPropertyTypeID.ToLower() <> "propertytypeid")) Then
                        doRedirect = True
                    End If
                End If
            Next

            If (doRedirect) Then
                Dim tabID As Integer = Null.NullInteger
                Dim params As New List(Of String)
                For Each key As String In Request.QueryString.AllKeys
                    If (key.ToLower() = "tabid") Then
                        tabID = Convert.ToInt32(Request(key))
                    Else
                        If (key.ToLower() <> "language") Then
                            If (key.ToLower() = "agenttype" And PropertySettings.SEOAgentType.ToLower() <> "agenttype") Then
                                If (Request(key).ToLower() = "viewtype" Or Request(key).ToLower() = "view") Then
                                    doRedirect = False
                                End If
                                params.Add(PropertySettings.SEOAgentType & "=" & Request(key))
                            ElseIf (key.ToLower() = "propertyid" And PropertySettings.SEOPropertyID.ToLower() <> "propertyid") Then
                                params.Add(PropertySettings.SEOPropertyID & "=" & Request(key))
                            ElseIf (key.ToLower() = "propertytypeid" And PropertySettings.SEOPropertyTypeID.ToLower() <> "propertytypeid") Then
                                params.Add(PropertySettings.SEOPropertyTypeID & "=" & Request(key))
                            Else
                                params.Add(key & "=" & Request(key))
                            End If
                        End If
                    End If
                Next

                If (doRedirect) Then
                    If (PropertySettings.SEORedirect) Then
                        Response.Status = "301 Moved Permanently"
                        Response.AddHeader("Location", NavigateURL(tabID, Null.NullString, params.ToArray()))
                        Response.End()
                    End If
                End If
            End If

            Dim agentParameter As String = PropertySettings.SEOAgentType

            If (Request(agentParameter) = "") Then
                agentParameter = "agentType"
            End If

            If Not (Request(agentParameter) Is Nothing) Then

                ' Load the appropriate Control
                '
                Select Case Request(agentParameter).ToLower()

                    Case "accessdenied"
                        _controlToLoad = "AccessDenied.ascx"

                    Case "approvereviews"
                        If (PropertySettings.ReviewModeration = True) Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionApprove)) Then
                                _controlToLoad = "ApproveReviews.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "ApproveReviews.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "changetemplate"
                        If (Me.IsEditable) Then
                            _controlToLoad = "ChangeTemplate.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "contactlog"
                        If (Me.IsEditable) Then
                            _controlToLoad = "ViewContactLog.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "editbrokers"
                        If (Me.IsEditable) Then
                            _controlToLoad = "EditBrokers.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "editcontactfield"
                        If (PropertySettings.PermissionAdminCustomField <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminCustomField)) Then
                                _controlToLoad = "EditContactField.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditContactField.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editcontactfields"
                        If (PropertySettings.PermissionAdminCustomField <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminCustomField)) Then
                                _controlToLoad = "EditContactFields.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditContactFields.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editcustomfield"
                        If (PropertySettings.PermissionAdminCustomField <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminCustomField)) Then
                                _controlToLoad = "EditCustomField.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditCustomField.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editcustomfields"
                        If (PropertySettings.PermissionAdminCustomField <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminCustomField)) Then
                                _controlToLoad = "EditCustomFields.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditCustomFields.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editemailfiles"
                        If (PropertySettings.PermissionAdminEmailFiles <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminEmailFiles)) Then
                                _controlToLoad = "EditEmailFiles.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditEmailFiles.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editexpirynotifications"
                        If (Me.IsEditable) Then
                            _controlToLoad = "EditExpiryNotifications.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "editlayoutfiles"
                        If (PropertySettings.PermissionAdminLayoutFiles <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminLayoutFiles)) Then
                                _controlToLoad = "EditLayoutFiles.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditLayoutFiles.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editlayoutsettings"
                        If (PropertySettings.PermissionAdminLayoutSettings <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminLayoutSettings)) Then
                                _controlToLoad = "EditLayoutSettings.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditLayoutSettings.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editpropertynotifications"
                        If (Me.IsEditable) Then
                            _controlToLoad = "EditPropertyNotifications.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "editreviewfields"
                        If (PropertySettings.PermissionAdminCustomField <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminCustomField)) Then
                                _controlToLoad = "EditReviewFields.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditReviewFields.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editreviewfield"
                        If (PropertySettings.PermissionAdminCustomField <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminCustomField)) Then
                                _controlToLoad = "EditReviewField.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.UserInfo.IsSuperUser) Then
                                _controlToLoad = "EditReviewField.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editpermissions"
                        If (Me.IsEditable) Then
                            _controlToLoad = "EditPermissions.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "editphotos"
                        _controlToLoad = "EditPhotos.ascx"

                    Case "editproperty"
                        _controlToLoad = "EditProperty.ascx"

                    Case "editpropertytype"
                        If (PropertySettings.PermissionAdminTypes <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminTypes)) Then
                                _controlToLoad = "EditPropertyType.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.IsEditable) Then
                                _controlToLoad = "EditPropertyType.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "editpropertytypes"
                        If (PropertySettings.PermissionAdminTypes <> "") Then
                            If (PortalSecurity.IsInRoles(PropertySettings.PermissionAdminTypes)) Then
                                _controlToLoad = "EditPropertyTypes.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        Else
                            If (Me.IsEditable) Then
                                _controlToLoad = "EditPropertyTypes.ascx"
                            Else
                                _controlToLoad = "LandingPage.ascx"
                            End If
                        End If

                    Case "edittemplatedefinition"
                        If (Me.UserInfo.IsSuperUser) Then
                            _controlToLoad = "EditTemplateDefinition.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "edittemplatedefinitions"
                        If (Me.UserInfo.IsSuperUser) Then
                            _controlToLoad = "EditTemplateDefinitions.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "export"
                        _controlToLoad = "Export.ascx"

                    Case "exporttemplatedefinition"
                        If (Me.UserInfo.IsSuperUser) Then
                            _controlToLoad = "ExportTemplateDefinition.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "importtemplatedefinition"
                        If (Me.UserInfo.IsSuperUser) Then
                            _controlToLoad = "ImportTemplateDefinition.ascx"
                        Else
                            _controlToLoad = "LandingPage.ascx"
                        End If

                    Case "landingpage"
                        _controlToLoad = "LandingPage.ascx"

                    Case "pdfrender"
                        _controlToLoad = "PdfRender.ascx"

                    Case "propertymanager"
                        _controlToLoad = "EditProperties.ascx"

                    Case "rss"
                        _controlToLoad = "Rss.ascx"

                    Case "view"
                        _controlToLoad = "ViewProperty.ascx"

                    Case "viewfeatured"
                        _controlToLoad = "ViewType.ascx"

                    Case "viewsearch"
                        _controlToLoad = "ViewType.ascx"

                    Case "viewtype"
                        _controlToLoad = "ViewType.ascx"

                    Case "xml"
                        _controlToLoad = "Xml.ascx"

                    Case Else
                        _controlToLoad = "LandingPage.ascx"

                End Select

            Else

                If (Me.PropertySettings.LandingPageMode = LandingPageType.Standard) Then
                    _controlToLoad = "LandingPage.ascx"
                Else
                    _controlToLoad = "ViewType.ascx"
                End If

            End If

            If (Me.PropertySettings.Template <> "") Then
                If (LayoutController.CheckTemplate(PortalSettings, Me.ModuleId, Me.PropertySettings.Template, LayoutType.Listing_Item_Html) = False) Then
                    If (Me.IsEditor) Then
                        If (_controlToLoad <> "EditTemplateDefinitions.ascx" And _controlToLoad <> "ImportTemplateDefinition.ascx") Then
                            _controlToLoad = "ChangeTemplate.ascx"
                        End If
                    Else
                        _controlToLoad = "NoTemplate.ascx"
                    End If
                    Exit Sub
                End If
            End If

            If (Me.PropertySettings.Template(ModuleId) = "") Then
                If (Me.IsEditor) Then
                    If (_controlToLoad <> "EditTemplateDefinitions.ascx" And _controlToLoad <> "ImportTemplateDefinition.ascx") Then
                        _controlToLoad = "ChangeTemplate.ascx"
                    End If
                Else
                    _controlToLoad = "NoTemplate.ascx"
                End If
            End If

        End Sub

        Private Sub RegisterScripts()

            DotNetNuke.Framework.jQuery.RequestRegistration()
            ClientResourceManager.RegisterScript(Page, Me.ResolveUrl("js/lightbox/jquery.lightbox-0.4.pack.js"))

            'If (Me.PropertySettings.IncludejQuery And HttpContext.Current.Items("jquery_registered") Is Nothing And HttpContext.Current.Items("jQueryRequested") Is Nothing) Then
            '    If (HttpContext.Current.Items("PropertyAgent-jQuery-ScriptsRegistered") Is Nothing And HttpContext.Current.Items("SimpleGallery-ScriptsRegistered") Is Nothing) Then
            '        Dim litLink As New Literal
            '        litLink.Text = "" & vbCrLf _
            '            & "<script type=""text/javascript"" src='" & Me.ResolveUrl("js/jquery-1.7.1.min.js") & "'></script>" & vbCrLf
            '        Page.Header.Controls.Add(litLink)
            '        HttpContext.Current.Items.Add("PropertyAgent-jQuery-ScriptsRegistered", "true")
            '    End If
            'End If

            'If (HttpContext.Current.Items("PropertyAgent-ScriptsRegistered") Is Nothing) Then

            '    Dim litLink As New Literal
            '    litLink.Text = "" & vbCrLf _
            '        & "<script type=""text/javascript"" src='" & Me.ResolveUrl("js/lightbox/jquery.lightbox-0.4.pack.js") & "'></script>" & vbCrLf
            '    Me.Controls.Add(litLink)
            '    HttpContext.Current.Items.Add("PropertyAgent-ScriptsRegistered", "true")
            'End If

        End Sub

        Private Sub LoadControlType()

            Dim objPortalModuleBase As PortalModuleBase = CType(Me.LoadControl(_controlToLoad), PortalModuleBase)

            If Not (objPortalModuleBase Is Nothing) Then

                objPortalModuleBase.ModuleConfiguration = Me.ModuleConfiguration
                objPortalModuleBase.ID = System.IO.Path.GetFileNameWithoutExtension(_controlToLoad)
                phControls.Controls.Add(objPortalModuleBase)

            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                LoadControlType()

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                RegisterScripts()

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region " Optional Interfaces "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim Actions As New ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString("ChangeTemplate", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ChangeTemplate"), False, SecurityAccessLevel.Edit, True, False)
                If (Me.PropertySettings.Template <> "") Then

                    If (Me.PropertySettings.ReviewModeration = True) Then
                        If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionApprove)) Then
                            Actions.Add(GetNextActionID, PropertyUtil.FormatPropertyLabel(Localization.GetString("ApproveReviews", Me.LocalResourceFile), PropertySettings), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ApproveReviews"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If

                    If (Me.PropertySettings.PermissionAdminTypes = "") Then
                        Actions.Add(GetNextActionID, PropertyUtil.FormatPropertyLabel(Localization.GetString("EditPropertyTypes", Me.LocalResourceFile), PropertySettings), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes"), False, SecurityAccessLevel.Edit, True, False)
                    Else
                        If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionAdminTypes)) Then
                            Actions.Add(GetNextActionID, PropertyUtil.FormatPropertyLabel(Localization.GetString("EditPropertyTypes", Me.LocalResourceFile), PropertySettings), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyTypes"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If

                    Actions.Add(GetNextActionID, PropertyUtil.FormatPropertyLabel(Localization.GetString("EditBrokers", Me.LocalResourceFile), PropertySettings), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditBrokers"), False, SecurityAccessLevel.Edit, True, False)

                    If (Me.PropertySettings.Template <> "") Then
                        If (Me.PropertySettings.PermissionAdminReviewField = "") Then
                            Actions.Add(GetNextActionID, Localization.GetString("EditContactFields", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields"), False, SecurityAccessLevel.Host, True, False)
                        Else
                            If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionAdminReviewField)) Then
                                Actions.Add(GetNextActionID, Localization.GetString("EditContactFields", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditContactFields"), False, SecurityAccessLevel.View, True, False)
                            End If
                        End If
                    End If

                    If (Me.PropertySettings.PermissionAdminCustomField = "") Then
                        Actions.Add(GetNextActionID, Localization.GetString("EditCustomFields", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields"), False, SecurityAccessLevel.Host, True, False)
                    Else
                        If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionAdminCustomField)) Then
                            Actions.Add(GetNextActionID, Localization.GetString("EditCustomFields", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditCustomFields"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If

                    If (Me.PropertySettings.PermissionAdminEmailFiles = "") Then
                        Actions.Add(GetNextActionID, Localization.GetString("EditEmailFiles", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditEmailFiles"), False, SecurityAccessLevel.Host, True, False)
                    Else
                        If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionAdminEmailFiles)) Then
                            Actions.Add(GetNextActionID, Localization.GetString("EditEmailFiles", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditEmailFiles"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If
                    Actions.Add(GetNextActionID, Localization.GetString("EditExpiryNotifications", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditExpiryNotifications"), False, SecurityAccessLevel.Edit, True, False)

                    If (Me.PropertySettings.PermissionAdminLayoutFiles = "") Then
                        Actions.Add(GetNextActionID, Localization.GetString("EditLayoutFiles", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditLayoutFiles"), False, SecurityAccessLevel.Host, True, False)
                    Else
                        If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionAdminLayoutFiles)) Then
                            Actions.Add(GetNextActionID, Localization.GetString("EditLayoutFiles", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditLayoutFiles"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If

                    If (Me.PropertySettings.PermissionAdminLayoutSettings = "") Then
                        Actions.Add(GetNextActionID, Localization.GetString("EditLayoutSettings", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditLayoutSettings"), False, SecurityAccessLevel.Host, True, False)
                    Else
                        If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionAdminLayoutSettings)) Then
                            Actions.Add(GetNextActionID, Localization.GetString("EditLayoutSettings", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditLayoutSettings"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If

                End If
                Actions.Add(GetNextActionID, Localization.GetString("EditPermissions", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPermissions"), False, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, Localization.GetString("EditPropertyNotifications", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPropertyNotifications"), False, SecurityAccessLevel.Edit, True, False)

                If (Me.PropertySettings.Template <> "") Then
                    If (Me.PropertySettings.PermissionAdminReviewField = "") Then
                        Actions.Add(GetNextActionID, Localization.GetString("EditReviewFields", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields"), False, SecurityAccessLevel.Host, True, False)
                    Else
                        If (PortalSecurity.IsInRoles(Me.PropertySettings.PermissionAdminReviewField)) Then
                            Actions.Add(GetNextActionID, Localization.GetString("EditReviewFields", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditReviewFields"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If
                End If

                Actions.Add(GetNextActionID, Localization.GetString("EditTemplateDefinitions", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), False, SecurityAccessLevel.Host, True, False)
                Actions.Add(GetNextActionID, Localization.GetString("ViewContactLog", Me.LocalResourceFile), ModuleActionType.ContentOptions, "", "", NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ContactLog"), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace