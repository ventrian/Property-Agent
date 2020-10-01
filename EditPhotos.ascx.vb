Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Web.UI.WebControls
Imports Ventrian.PropertyAgent.Controls

Namespace Ventrian.PropertyAgent
    Public Class EditPhotos
        Inherits PropertyAgentBase
        ' Methods
        Public Sub New()
            AddHandler MyBase.Init, New EventHandler(AddressOf Me.Page_Init)
            Me._propertyID = Null.NullInteger
            Me._returnUrl = Null.NullString
        End Sub

        Private Sub BindCrumbs()
            Dim list As New ArrayList
            Dim info As New CrumbInfo With {
                .Caption = MyBase.PropertySettings.MainLabel,
                .Url = Globals.NavigateURL
            }
            list.Add(info)
            Dim info2 As New CrumbInfo With {
                .Caption = MyBase.GetResourceString("PropertyManager")
            }
            Dim additionalParameters As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=PropertyManager")}
            info2.Url = Globals.NavigateURL(MyBase.TabId, "", additionalParameters)
            list.Add(info2)
            If (Me._propertyID <> Null.NullInteger) Then
                Dim info4 As New CrumbInfo With {
                    .Caption = MyBase.GetResourceString("EditProperty")
                }
                Dim textArray2 As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=EditProperty"), (MyBase.PropertySettings.SEOPropertyID & "=" & Me._propertyID.ToString)}
                info4.Url = Globals.NavigateURL(MyBase.TabId, "", textArray2)
                list.Add(info4)
            Else
                Dim textArray3 As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=EditProperty")}
                MyBase.Response.Redirect(Globals.NavigateURL(MyBase.TabId, "", textArray3), True)
            End If
            Dim info3 As New CrumbInfo With {
                .Caption = Localization.GetString("EditPhotos", MyBase.LocalResourceFile)
            }
            Dim textArray4 As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=EditPhotos"), (MyBase.PropertySettings.SEOPropertyID & "=" & Me._propertyID.ToString)}
            info3.Url = Globals.NavigateURL(MyBase.TabId, "", textArray4)
            list.Add(info3)
            If (MyBase.PropertySettings.BreadcrumbPlacement = BreadcrumbType.Portal) Then
                Dim num As Integer = (list.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num)
                    Dim info5 As CrumbInfo = DirectCast(list.Item(i), CrumbInfo)
                    If (i > 0) Then
                        Dim info6 As New TabInfo With {
                            .TabID = (-8888 + i),
                            .TabName = info5.Caption,
                            .Url = info5.Url
                        }
                        MyBase.PortalSettings.ActiveTab.BreadCrumbs.Add(info6)
                    End If
                    i += 1
                Loop
            End If
            If (MyBase.PropertySettings.BreadcrumbPlacement = BreadcrumbType.Module) Then
                Me.rptBreadCrumbs.DataSource = list
                Me.rptBreadCrumbs.DataBind()
            End If
        End Sub

        Private Sub CheckSecurity()
            If ((Not MyBase.IsEditable And (Not PortalSecurity.IsInRoles(MyBase.PropertySettings.PermissionSubmit) OrElse Not PortalSecurity.IsInRoles(MyBase.PropertySettings.PermissionAddImages))) AndAlso ((Me._propertyID = Null.NullInteger) OrElse Not PortalSecurity.IsInRoles(MyBase.PropertySettings.PermissionApprove))) Then
                Dim additionalParameters As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=AccessDenied")}
                MyBase.Response.Redirect(Globals.NavigateURL(MyBase.TabId, "", additionalParameters), True)
            End If
            If (((Not MyBase.IsEditable And PortalSecurity.IsInRoles(MyBase.PropertySettings.PermissionSubmit)) And Not PortalSecurity.IsInRoles(MyBase.PropertySettings.PermissionApprove)) AndAlso (Me._propertyID <> Null.NullInteger)) Then
                Dim info As PropertyInfo = New PropertyController().Get(Me._propertyID)
                If (Not info Is Nothing) Then
                    If (info.AuthorID <> MyBase.UserId) Then
                        Dim textArray2 As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=AccessDenied")}
                        MyBase.Response.Redirect(Globals.NavigateURL(MyBase.TabId, "", textArray2), True)
                    End If
                Else
                    MyBase.Response.Redirect(Globals.NavigateURL, True)
                End If
            End If
        End Sub

        Private Sub cmdReturnToEditProperty_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If (Me._returnUrl <> "") Then
                    Dim additionalParameters As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=EditProperty"), (MyBase.PropertySettings.SEOPropertyID & "=" & Me._propertyID.ToString), ("ReturnUrl=" & MyBase.Server.UrlEncode(Me._returnUrl))}
                    MyBase.Response.Redirect(Globals.NavigateURL(MyBase.TabId, "", additionalParameters), True)
                Else
                    Dim textArray2 As String() = New String() {(MyBase.PropertySettings.SEOAgentType & "=EditProperty"), (MyBase.PropertySettings.SEOPropertyID & "=" & Me._propertyID.ToString)}
                    MyBase.Response.Redirect(Globals.NavigateURL(MyBase.TabId, "", textArray2), True)
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError()
            End Try
        End Sub

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.ReadQueryString()
                Me.CheckSecurity()
                Me.cmdReturnToEditProperty.Text = MyBase.GetResourceString("cmdReturnToEditProperty")
                Me.cmdReturnToEditProperty.CssClass = MyBase.PropertySettings.ButtonClass
                If Not Me.Page.IsPostBack Then
                    Me.BindCrumbs()
                    Me.EditPropertyPhotos1.BindPhotos()
                    Me.UploadPhotoStandard1.Visible = (MyBase.PropertySettings.UploadMode = UploadType.Standard)
                    Me.UploadPhotoSWF1.Visible = (MyBase.PropertySettings.UploadMode = UploadType.Flash)
                    Me.UploadPhotoHTML51.Visible = (MyBase.PropertySettings.UploadMode = UploadType.HTML5)
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError()
            End Try
        End Sub

        Private Sub ReadQueryString()
            Dim sEOPropertyID As String = MyBase.PropertySettings.SEOPropertyID
            If (MyBase.Request.Item(sEOPropertyID) = "") Then
                sEOPropertyID = "PropertyID"
            End If
            If (Not MyBase.Request.Item(sEOPropertyID) Is Nothing) Then
                Me._propertyID = Convert.ToInt32(MyBase.Request.Item(sEOPropertyID))
            End If
            If (Not MyBase.Request.Item("ReturnUrl") Is Nothing) Then
                Me._returnUrl = MyBase.Server.UrlDecode(MyBase.Request.Item("ReturnUrl"))
            End If
        End Sub

        Public Sub RefreshPhotos()
            Me.EditPropertyPhotos1.BindPhotos()
        End Sub


        ' Properties
        Protected Overridable Property cmdReturnToEditProperty As LinkButton
            <CompilerGenerated>
            Get
                Return Me._cmdReturnToEditProperty
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As LinkButton)
                Dim handler As EventHandler = New EventHandler(AddressOf Me.cmdReturnToEditProperty_Click)
                Dim button As LinkButton = Me._cmdReturnToEditProperty
                If (Not button Is Nothing) Then
                    RemoveHandler button.Click, handler
                End If
                Me._cmdReturnToEditProperty = WithEventsValue
                button = Me._cmdReturnToEditProperty
                If (Not button Is Nothing) Then
                    AddHandler button.Click, handler
                End If
            End Set
        End Property

        Protected Overridable Property EditPropertyPhotos1 As EditPropertyPhotos
            <CompilerGenerated>
            Get
                Return Me._EditPropertyPhotos1
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As EditPropertyPhotos)
                Me._EditPropertyPhotos1 = WithEventsValue
            End Set
        End Property

        Protected Overridable Property Options1 As Options
            <CompilerGenerated>
            Get
                Return Me._Options1
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Options)
                Me._Options1 = WithEventsValue
            End Set
        End Property

        Protected Overridable Property rptBreadCrumbs As Repeater
            <CompilerGenerated>
            Get
                Return Me._rptBreadCrumbs
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Repeater)
                Me._rptBreadCrumbs = WithEventsValue
            End Set
        End Property

        Protected Overridable Property UploadPhotoStandard1 As UploadPhotoStandard
            <CompilerGenerated>
            Get
                Return Me._UploadPhotoStandard1
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UploadPhotoStandard)
                Me._UploadPhotoStandard1 = WithEventsValue
            End Set
        End Property

        Protected Overridable Property UploadPhotoSWF1 As UploadPhotoSWF
            <CompilerGenerated>
            Get
                Return Me._UploadPhotoSWF1
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UploadPhotoSWF)
                Me._UploadPhotoSWF1 = WithEventsValue
            End Set
        End Property

        Protected Overridable Property UploadPhotoHTML51 As UploadPhotoHTML5
            <CompilerGenerated>
            Get
                Return Me._UploadPhotoHTML51
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UploadPhotoHTML5)
                Me._UploadPhotoHTML51 = WithEventsValue
            End Set
        End Property

        ' Fields
        <CompilerGenerated, AccessedThroughProperty("cmdReturnToEditProperty")>
        Private _cmdReturnToEditProperty As LinkButton
        <CompilerGenerated, AccessedThroughProperty("EditPropertyPhotos1")>
        Private _EditPropertyPhotos1 As EditPropertyPhotos
        <CompilerGenerated, AccessedThroughProperty("Options1")>
        Private _Options1 As Options
        Private _photos As ArrayList
        Private _propertyID As Integer
        Private _returnUrl As String
        <CompilerGenerated, AccessedThroughProperty("rptBreadCrumbs")>
        Private _rptBreadCrumbs As Repeater
        <CompilerGenerated, AccessedThroughProperty("UploadPhotoStandard1")>
        Private _UploadPhotoStandard1 As UploadPhotoStandard
        <CompilerGenerated, AccessedThroughProperty("UploadPhotoSWF1")>
        Private _UploadPhotoSWF1 As UploadPhotoSWF
        <CompilerGenerated, AccessedThroughProperty("UploadPhotoHTML51")>
        Private _UploadPhotoHTML51 As UploadPhotoHTML5
    End Class
End Namespace
