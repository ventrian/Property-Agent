Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class ViewProperty
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _propertyID As Integer = Null.NullInteger

        Private _layoutController As LayoutController

        Private _objLayout As LayoutInfo
        Private _objLayoutHeader As LayoutInfo
        Private _objLayoutFooter As LayoutInfo
        Private _objLayoutPageTitle As LayoutInfo
        Private _objLayoutPageDescription As LayoutInfo
        Private _objLayoutPageKeywords As LayoutInfo
        Private _objLayoutPageHeader As LayoutInfo

        Private _objLayoutPhotoItem As LayoutInfo

#End Region

#Region " Private Methods "

        Private Sub CheckSecurity()
            If (Request.IsAuthenticated) Then
                Dim objPropertyController As New PropertyController
                Dim objPropertyInfo As PropertyInfo = objPropertyController.Get(_propertyID)

                If Not (objPropertyInfo Is Nothing) Then
                    If (objPropertyInfo.AuthorID = Me.UserId Or objPropertyInfo.BrokerID = Me.UserId) Then
                        Return
                    End If
                Else
                    Response.Redirect(NavigateURL(), True)
                End If
            End If

            If (PortalSecurity.IsInRoles(PropertySettings.PermissionViewDetail) = False) Then
                If (PropertySettings.PermissionDetailUrl <> "") Then
                    Response.Redirect(AddHTTP(PropertySettings.PermissionDetailUrl), True)
                Else
                    Response.Redirect(NavigateURL(), True)
                End If
            End If

        End Sub

        Private Sub ReadQueryString()

            Dim propertyIDParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyIDParam) = "") Then
                propertyIDParam = "PropertyID"
            End If
            If Not (Request(propertyIDParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyIDParam))
            End If

        End Sub

        Private Sub InitializeTemplate()

            _layoutController = New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)

            _objLayout = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_Item_Html)
            _objLayoutHeader = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_Header_Html)
            _objLayoutFooter = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_Footer_Html)
            _objLayoutPageTitle = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageTitle_Html)
            _objLayoutPageDescription = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageDescription_Html)
            _objLayoutPageKeywords = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageKeywords_Html)
            _objLayoutPageHeader = _layoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageHeader_Html)
            _layoutController.LoadStyleSheet(Me.PropertySettings.Template)

        End Sub

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1).ToUpper()

                        Case "ISMOBILEDEVICE"
                            If LayoutController.IsMobileBrowser() = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISMOBILEDEVICE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISMOBILEDEVICE"
                            ' Do Nothing

                        Case "ISNOTMOBILEDEVICE"
                            If LayoutController.IsMobileBrowser() = True Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISNOTMOBILEDEVICE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTMOBILEDEVICE"
                            ' Do Nothing

                        Case "ISOWNER"
                            Dim isOwner As Boolean = False
                            If (Me.Page.User.Identity.IsAuthenticated) Then
                                Dim objPropertyController As New PropertyController
                                Dim objProperty As PropertyInfo = objPropertyController.Get(_propertyID)
                                If Not (objProperty Is Nothing) Then
                                    If (UserController.GetCurrentUserInfo.UserID = objProperty.AuthorID) Then
                                        isOwner = True
                                    End If
                                End If
                            End If

                            If isOwner = False Then
                                While (iPtr < layoutArray.Length - 1)
                                    If (layoutArray(iPtr + 1) = "/ISOWNER") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISOWNER"
                            ' Do Nothing

                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = PropertySettings.PropertyLabel
                            objPlaceHolder.Add(objLiteral)

                    End Select

                End If

            Next

        End Sub

        Private Sub BindProperty()

            Dim objPropertyController As New PropertyController
            Dim objPropertyInfo As PropertyInfo = objPropertyController.Get(_propertyID)
            If objPropertyInfo.OnlyForAuthenticated = False Then
                If Me.UserId = -1 Then
                    objPropertyInfo = Nothing
                End If
            End If
            If Not (objPropertyInfo Is Nothing) Then

                If (objPropertyInfo.Status <> StatusType.Published) Then
                    Response.Redirect(NavigateURL(), True)
                End If

                ' Check Redirect
                Dim propertyUrl As String = _layoutController.GetExternalLink(_layoutController.GetPropertyLink(objPropertyInfo, Me.CustomFields))

                'If (PropertySettings.SEORedirect) Then
                '    If (propertyUrl.ToLower() <> _layoutController.GetExternalLink(Request.RawUrl).ToLower()) Then
                '        Response.Status = "301 Moved Permanently"
                '        Response.AddHeader("Location", propertyUrl)
                '        Response.End()
                '    End If
                'End If

                If (PropertySettings.SEOCanonicalLink) Then
                    Dim litCanonical As New Literal
                    litCanonical.Text = "<link rel=""canonical"" href=""" & propertyUrl & """/>"
                    Me.BasePage.Header.Controls.Add(litCanonical)
                End If

                ' Record View
                Dim cookie As HttpCookie = Request.Cookies("Property" & _propertyID.ToString())

                If (cookie Is Nothing) Then

                    objPropertyInfo.ViewCount = objPropertyInfo.ViewCount + 1
                    objPropertyController.Update(objPropertyInfo)

                    objPropertyController.AddStatistic(objPropertyInfo.PropertyID, Me.UserId, Request.UserHostAddress)

                    cookie = New HttpCookie("Property" & _propertyID.ToString())
                    cookie.Value = "1"
                    cookie.Expires = DateTime.Now.AddMinutes(20)
                    Context.Response.Cookies.Add(cookie)

                End If

                Dim isMySubmittedProperty As Boolean = False
                If (PortalSecurity.IsInRoles(PropertySettings.PermissionSubmit) = True Or PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True) Then
                    If Me.UserId = objPropertyInfo.AuthorID Then
                        isMySubmittedProperty = True
                    End If
                End If

                ' Check for broker access
                If (Not objPropertyInfo Is Nothing AndAlso Request.IsAuthenticated AndAlso isMySubmittedProperty = False AndAlso Me.IsEditable = False) Then
                    If PortalSecurity.IsInRoles(PropertySettings.PermissionBroker) = True Then
                        If (Me.UserId = objPropertyInfo.BrokerID) Then
                            isMySubmittedProperty = True
                        End If
                    End If
                End If

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, (Me.IsEditable Or isMySubmittedProperty), Me.TabId, Me.ModuleId, Me.ModuleKey)

                If (objPropertyInfo.PhotoCount = 0) Then
                    phLightbox.Visible = False
                End If

                objLayoutController.ModuleKey = Me.ModuleKey & "-H"
                objLayoutController.ProcessItem(phProperty.Controls, _objLayoutHeader.Tokens, objPropertyInfo, CustomFields)
                objLayoutController.ModuleKey = Me.ModuleKey
                objLayoutController.ProcessItem(phProperty.Controls, _objLayout.Tokens, objPropertyInfo, CustomFields)
                objLayoutController.ModuleKey = Me.ModuleKey & "-F"
                objLayoutController.ProcessItem(phProperty.Controls, _objLayoutFooter.Tokens, objPropertyInfo, CustomFields)

                If (_objLayoutPageTitle.Template <> "") Then
                    Dim phPageTitle As New PlaceHolder()
                    objLayoutController.ProcessItem(phPageTitle.Controls, _objLayoutPageTitle.Tokens, objPropertyInfo, CustomFields)
                    Me.BasePage.Title = RenderControlToString(phPageTitle)
                End If

                If (_objLayoutPageDescription.Template <> "") Then
                    Dim phPageDescription As New PlaceHolder()
                    objLayoutController.ProcessItem(phPageDescription.Controls, _objLayoutPageDescription.Tokens, objPropertyInfo, CustomFields)
                    Me.BasePage.Description = RenderControlToString(phPageDescription)
                End If

                If (_objLayoutPageKeywords.Template <> "") Then
                    Dim phPageKeywords As New PlaceHolder()
                    objLayoutController.ProcessItem(phPageKeywords.Controls, _objLayoutPageKeywords.Tokens, objPropertyInfo, CustomFields)
                    Me.BasePage.KeyWords = RenderControlToString(phPageKeywords)
                End If

                If (_objLayoutPageHeader.Template <> "") Then
                    Dim phPageHeader As New PlaceHolder()
                    objLayoutController.ProcessItem(phPageHeader.Controls, _objLayoutPageHeader.Tokens, objPropertyInfo, CustomFields)
                    Me.BasePage.Header.Controls.Add(phPageHeader)
                End If

                Dim crumbs As New ArrayList
                Dim objCrumbMain As New CrumbInfo
                objCrumbMain.Caption = PropertySettings.MainLabel
                objCrumbMain.Url = NavigateURL()
                crumbs.Add(objCrumbMain)

                ' Show search details. 
                Dim params As New List(Of String)

                params.Add(PropertySettings.SEOAgentType & "=ViewSearch")

                If (Request("customFieldIDs") <> "") Then
                    params.Add("customFieldIDs=" & Request("customFieldIDs"))
                End If

                If (Request("SearchValues") <> "") Then
                    params.Add("SearchValues=" & Request("SearchValues"))
                End If

                If (Request("PropertyBrokerID") <> "") Then
                    params.Add("PropertyBrokerID=" & Request("PropertyBrokerID"))
                End If

                If (Request("PropertyAgentID") <> "") Then
                    params.Add("PropertyAgentID=" & Request("PropertyAgentID"))
                End If

                If (Request("sortBy") <> "") Then
                    params.Add("sortBy=" & Request("sortBy"))
                End If

                If (Request("sortDir") <> "") Then
                    params.Add("sortDir=" & Request("sortDir"))
                End If

                If (params.Count > 1 AndAlso HttpContext.Current.Request("PropertyTypeID") <> "") Then
                    params.Add(PropertySettings.SEOPropertyTypeID & "=" & HttpContext.Current.Request("PropertyTypeID"))
                End If

                If (params.Count = 1) Then

                    ' No search, show types list.

                    If (PropertySettings.HideTypes = False) Then
                        Dim objPropertyTypeController As New PropertyTypeController
                        Dim objPropertyType As PropertyTypeInfo = objPropertyTypeController.Get(Me.ModuleId, objPropertyInfo.PropertyTypeID)

                        If Not (objPropertyType Is Nothing) Then

                            Dim parentID As Integer = objPropertyType.ParentID

                            Do
                                Dim objCrumbType As New CrumbInfo
                                objCrumbType.Caption = objPropertyType.Name
                                objCrumbType.Url = objLayoutController.GetTypeLink(TabId, ModuleId, objPropertyType, Null.NullString(), Null.NullInteger)
                                If (objPropertyInfo.PropertyTypeID = objPropertyType.PropertyTypeID) Then
                                    crumbs.Add(objCrumbType)
                                Else
                                    crumbs.Insert(1, objCrumbType)
                                End If

                                objPropertyType = objPropertyTypeController.Get(Me.ModuleId, objPropertyType.ParentID)
                                If (objPropertyType Is Nothing) Then
                                    parentID = Null.NullInteger
                                Else
                                    parentID = objPropertyType.PropertyTypeID
                                End If
                            Loop While (parentID <> Null.NullInteger)

                        End If
                    End If

                Else

                    Dim objCrumbSearchResults As New CrumbInfo
                    objCrumbSearchResults.Caption = GetResourceString("SearchResults")
                    objCrumbSearchResults.Url = NavigateURL(Me.TabId, "", params.ToArray())
                    crumbs.Add(objCrumbSearchResults)

                End If

                Dim objCrumbProperty As New CrumbInfo
                objCrumbProperty.Caption = GetResourceString("PropertyDetails")
                objCrumbProperty.Url = Request.RawUrl
                crumbs.Add(objCrumbProperty)

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

            Else
                Response.Status = "301 Moved Permanently"
                Response.AddHeader("Location", NavigateURL())
                Response.End()
                ' Response.Redirect(NavigateURL(), True)
            End If

        End Sub

        Private Sub RegisterScripts()

            'If (HttpContext.Current.Items("PropertyAgent-ScriptsRegistered") Is Nothing) Then
            '    Dim objCSS As System.Web.UI.Control = BasePage.FindControl("CSS")

            '    If Not (objCSS Is Nothing) Then
            '        Dim litLink As New Literal
            '        litLink.Text = "" & vbCrLf _
            '            & "<script type=""text/javascript"" src='" & Me.ResolveUrl("js/lightbox/jquery.lightbox-0.4.pack.js") & "'></script>" & vbCrLf
            '        objCSS.Controls.Add(litLink)
            '    End If
            '    HttpContext.Current.Items.Add("PropertyAgent-ScriptsRegistered", "true")
            'End If

            'ClientScriptManager.

        End Sub

        Private Function RenderControlToString(ByVal ctrl As Control) As String

            Dim sb As New StringBuilder()
            Dim tw As New IO.StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            ctrl.RenderControl(hw)

            Return sb.ToString()

        End Function

#End Region

#Region " Protected Methods "

        Protected Function GetLocalizedValue(ByVal key As String) As String

            Return Localization.GetString(key, Me.LocalResourceFile)

        End Function

        Protected Function GetPropertyID() As String

            Dim propertyID As Integer = Null.NullInteger
            Dim propertyIDParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyIDParam) = "") Then
                propertyIDParam = "PropertyID"
            End If
            If Not (Request(propertyIDParam) Is Nothing) Then
                propertyID = Convert.ToInt32(Request(propertyIDParam))
            End If

            Return propertyID.ToString()

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                CheckSecurity()
                InitializeTemplate()
                BindProperty()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
