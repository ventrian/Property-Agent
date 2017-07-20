Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Namespace Ventrian.PropertyAgent

    Partial Public Class EditPhotos
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _propertyID As Integer = Null.NullInteger
        Private _returnUrl As String = Null.NullString

        Private _photos As ArrayList

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            Dim propertyParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyParam) = "") Then
                propertyParam = "PropertyID"
            End If
            If Not (Request(propertyParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyParam))
            End If

            If Not (Request("ReturnUrl") Is Nothing) Then
                _returnUrl = Server.UrlDecode(Request("ReturnUrl"))
            End If

        End Sub

        Private Sub CheckSecurity()

            If (IsEditable = False And (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = False OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionAddImages) = False)) Then
                If (_propertyID = Null.NullInteger OrElse PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=AccessDenied"), True)
                End If
            End If

            If (IsEditable = False And PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True And PortalSecurity.IsInRoles(PropertySettings.PermissionApprove) = False) Then
                If (_propertyID <> Null.NullInteger) Then
                    Dim objPropertyController As New PropertyController
                    Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)

                    If Not (objProperty Is Nothing) Then
                        If (objProperty.AuthorID <> UserId) Then
                            Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=AccessDenied"), True)
                        End If
                    Else
                        Response.Redirect(NavigateURL(), True)
                    End If
                End If
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objCrumbPropertyManager As New CrumbInfo
            objCrumbPropertyManager.Caption = GetResourceString("PropertyManager")
            objCrumbPropertyManager.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=PropertyManager")
            crumbs.Add(objCrumbPropertyManager)

            If (_propertyID <> Null.NullInteger) Then
                Dim objCrumbProperty As New CrumbInfo
                objCrumbProperty.Caption = GetResourceString("EditProperty")
                objCrumbProperty.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditProperty", PropertySettings.SEOPropertyID & "=" & _propertyID.ToString())
                crumbs.Add(objCrumbProperty)
            Else
                Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditProperty"), True)
            End If

            Dim objCrumbEditPhotos As New CrumbInfo
            objCrumbEditPhotos.Caption = Localization.GetString("EditPhotos", LocalResourceFile)
            objCrumbEditPhotos.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditPhotos", PropertySettings.SEOPropertyID & "=" & _propertyID.ToString())
            crumbs.Add(objCrumbEditPhotos)

            If (PropertySettings.BreadcrumbPlacement = BreadcrumbType.Portal) Then
                For i As Integer = 0 To crumbs.Count - 1
                    Dim objCrumb As CrumbInfo = crumbs(i)
                    If (i > 0) Then
                        Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo
                        objTab.TabID = -8888 + i
                        objTab.TabName = objCrumb.Caption
                        objTab.Url = objCrumb.Url
                        PortalSettings.ActiveTab.BreadCrumbs.Add(objTab)
                    End If
                Next
            End If

            If (PropertySettings.BreadcrumbPlacement = BreadcrumbType.Module) Then
                rptBreadCrumbs.DataSource = crumbs
                rptBreadCrumbs.DataBind()
            End If

        End Sub

#End Region

#Region " Public Methods "

        Public Sub RefreshPhotos()

            EditPropertyPhotos1.BindPhotos()

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                CheckSecurity()

                cmdReturnToEditProperty.Text = GetResourceString("cmdReturnToEditProperty")
                cmdReturnToEditProperty.CssClass = PropertySettings.ButtonClass

                If (Page.IsPostBack = False) Then

                    BindCrumbs()
                    EditPropertyPhotos1.BindPhotos()

                    UploadPhotoStandard1.Visible = (Me.PropertySettings.UploadMode = UploadType.Standard)
                    UploadPhotoSWF1.Visible = (Me.PropertySettings.UploadMode = UploadType.Flash)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdReturnToEditProperty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturnToEditProperty.Click

            Try

                If (_returnUrl <> "") Then
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditProperty", PropertySettings.SEOPropertyID & "=" & _propertyID.ToString(), "ReturnUrl=" & Server.UrlEncode(_returnUrl)), True)
                Else
                    Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditProperty", PropertySettings.SEOPropertyID & "=" & _propertyID.ToString()), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace