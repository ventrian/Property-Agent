Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules

Imports System.IO

Namespace Ventrian.PropertyAgent

    Partial Public Class Print
        Inherits DotNetNuke.Framework.PageBase

#Region " Private Members "

        Private _propertyID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _tabID As Integer = Null.NullInteger
        Private _portalID As Integer = Null.NullInteger

        Private _objLayout As LayoutInfo
        Private _objLayoutHeader As LayoutInfo
        Private _objLayoutFooter As LayoutInfo

        Private _objLayoutPhotoItem As LayoutInfo

        Private _propertySettings As PropertySettings

#End Region

#Region " Private Methods "

        Private Sub ManageStyleSheets(ByVal PortalCSS As Boolean)

            ' initialize reference paths to load the cascading style sheets
            Dim objCSS As Control = Me.FindControl("CSS")
            Dim objLink As HtmlGenericControl
            Dim ID As String

            Dim objCSSCache As Hashtable = CType(DataCache.GetCache("CSS"), Hashtable)
            If objCSSCache Is Nothing Then
                objCSSCache = New Hashtable
            End If

            If Not objCSS Is Nothing Then

                If PortalCSS = False Then

                    Dim objLiteral As New Literal
                    objLiteral.ID = CreateValidID("PropertyAgentNoIndex")
                    objLiteral.Text = "<META NAME=""robots"" CONTENT=""NOFOLLOW,NOINDEX"">"
                    objCSS.Controls.Add(objLiteral)

                    ' module style sheet
                    ID = CreateValidID("PropertyAgent")
                    objLink = New HtmlGenericControl("LINK")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = Me.ResolveUrl("module.css")
                    objCSS.Controls.Add(objLink)

                    ' default style sheet ( required )
                    ID = CreateValidID(Globals.HostPath)
                    objLink = New HtmlGenericControl("LINK")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = Globals.HostPath & "default.css"
                    objCSS.Controls.Add(objLink)

                    ' skin package style sheet
                    ID = CreateValidID(PortalSettings.ActiveTab.SkinPath)
                    If objCSSCache.ContainsKey(ID) = False Then
                        If File.Exists(Server.MapPath(PortalSettings.ActiveTab.SkinPath) & "skin.css") Then
                            objCSSCache(ID) = PortalSettings.ActiveTab.SkinPath & "skin.css"
                        Else
                            objCSSCache(ID) = ""
                        End If
                        If Not Globals.PerformanceSetting = Globals.PerformanceSettings.NoCaching Then
                            DataCache.SetCache("CSS", objCSSCache)
                        End If
                    End If
                    If objCSSCache(ID).ToString <> "" Then
                        objLink = New HtmlGenericControl("LINK")
                        objLink.ID = ID
                        objLink.Attributes("rel") = "stylesheet"
                        objLink.Attributes("type") = "text/css"
                        objLink.Attributes("href") = objCSSCache(ID).ToString
                        objCSS.Controls.Add(objLink)
                    End If

                    ' skin file style sheet
                    ID = CreateValidID(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))
                    If objCSSCache.ContainsKey(ID) = False Then
                        If File.Exists(Server.MapPath(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))) Then
                            objCSSCache(ID) = Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css")
                        Else
                            objCSSCache(ID) = ""
                        End If
                        If Not Globals.PerformanceSetting = Globals.PerformanceSettings.NoCaching Then
                            DataCache.SetCache("CSS", objCSSCache)
                        End If
                    End If
                    If objCSSCache(ID).ToString <> "" Then
                        objLink = New HtmlGenericControl("LINK")
                        objLink.ID = ID
                        objLink.Attributes("rel") = "stylesheet"
                        objLink.Attributes("type") = "text/css"
                        objLink.Attributes("href") = objCSSCache(ID).ToString
                        objCSS.Controls.Add(objLink)
                    End If
                Else
                    ' portal style sheet
                    ID = CreateValidID(PortalSettings.HomeDirectory)
                    objLink = New HtmlGenericControl("LINK")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = PortalSettings.HomeDirectory & "portal.css"
                    objCSS.Controls.Add(objLink)
                End If

            End If

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("ModuleID") Is Nothing) Then
                _moduleID = Convert.ToInt32(Request("ModuleID"))
            End If

            If Not (Request("TabID") Is Nothing) Then
                _tabID = Convert.ToInt32(Request("TabID"))
            End If

            If Not (Request("PortalID") Is Nothing) Then
                _portalID = Convert.ToInt32(Request("PortalID"))
            End If

            Dim propertyParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyParam) = "") Then
                propertyParam = "PropertyID"
            End If
            If Not (Request(propertyParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyParam))
            End If

        End Sub

        Private Sub InitializeTemplate()

            Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Nothing, False, _tabID, _moduleID, "PropertyAgentPrint-" & _moduleID.ToString())
            _objLayout = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Print_Item_Html)
            _objLayoutHeader = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Print_Header_Html)
            _objLayoutFooter = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Print_Footer_Html)
            objLayoutController.LoadStyleSheet(Me.PropertySettings.Template)

        End Sub

        Private Sub BindProperty()

            Dim objPropertyController As New PropertyController
            Dim objPropertyInfo As PropertyInfo = objPropertyController.Get(_propertyID)

            If Not (objPropertyInfo Is Nothing) Then

                Dim objCustomFieldController As New CustomFieldController
                Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(_moduleID, True)

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Nothing, False, _tabID, _moduleID, "PropertyAgentPrint-" & _moduleID.ToString())

                ProcessHeaderFooter(phProperty.Controls, _objLayoutHeader.Tokens)
                objLayoutController.ProcessItem(phProperty.Controls, _objLayout.Tokens, objPropertyInfo, objCustomFields, Nothing, True)
                ProcessHeaderFooter(phProperty.Controls, _objLayoutFooter.Tokens)

            End If

        End Sub

        Private Sub ProcessHeaderFooter(ByRef objPlaceHolder As ControlCollection, ByVal layoutArray As String())

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2
                objPlaceHolder.Add(New LiteralControl(layoutArray(iPtr).ToString()))
                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "PROPERTYLABEL"
                            Dim objLiteral As New Literal
                            objLiteral.Text = PropertySettings.PropertyLabel
                            objPlaceHolder.Add(objLiteral)

                    End Select
                End If
            Next

        End Sub

#End Region

#Region " Private Properties "

        Public ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Public ReadOnly Property PropertySettings() As PropertySettings
            Get
                If (_propertySettings Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    Dim settings As Hashtable = objModuleController.GetModuleSettings(_moduleID)
                    _propertySettings = New PropertySettings(settings)
                End If
                Return _propertySettings
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            ManageStyleSheets(False)
            ManageStyleSheets(True)
            ReadQueryString()
            InitializeTemplate()
            BindProperty()

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        End Sub

#End Region

    End Class

End Namespace
