Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security

Imports System.IO

Namespace Ventrian.PropertyAgent

    Partial Public Class Xml
        Inherits PropertyAgentBase

#Region " Private Members "

        Private _agentType As String = Null.NullString

        Private _propertyTypeID As Integer = Null.NullInteger
        Private _propertyAgentID As Integer = Null.NullInteger
        Private _propertyBrokerID As Integer = Null.NullInteger
        Private _totalRecords As Integer = 0

        Private _customFieldIDs As String = Null.NullString
        Private _searchValues As String = Null.NullString

        Private _sortBy As String = ""
        Private _sortDirection As String = ""

#End Region

#Region " Private Properties "

        Private ReadOnly Property SortBy() As SortByType
            Get
                If (_sortBy <> "") Then
                    If (_sortBy.StartsWith("cf")) Then
                        Return SortByType.CustomField
                    Else
                        Return CType(System.Enum.Parse(GetType(SortByType), _sortBy, True), SortByType)
                    End If
                Else
                    Return Me.PropertySettings.ListingSortBy
                End If
            End Get
        End Property

        Private ReadOnly Property SortByCustomField() As Integer
            Get
                If (_sortBy <> "") Then
                    If (_sortBy.StartsWith("cf")) Then
                        Return Convert.ToInt32(_sortBy.Replace("cf", ""))
                    Else
                        Return Null.NullInteger
                    End If
                Else
                    Return Me.PropertySettings.ListingSortByCustomField
                End If
            End Get
        End Property

        Private ReadOnly Property SortDirection() As SortDirectionType
            Get
                If (_sortDirection <> "") Then
                    Return CType(System.Enum.Parse(GetType(SortDirectionType), _sortDirection, True), SortDirectionType)
                Else
                    Return Me.PropertySettings.ListingSortDirection
                End If
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            Dim agentTypeParam As String = PropertySettings.SEOAgentType
            If (Request(agentTypeParam) = "") Then
                agentTypeParam = "agentType"
            End If
            If Not (Request(agentTypeParam) Is Nothing) Then
                _agentType = Request(agentTypeParam)
            End If

            Dim propertyTypeIDParam As String = PropertySettings.SEOPropertyTypeID
            If (Request(propertyTypeIDParam) = "") Then
                propertyTypeIDParam = "PropertyTypeID"
            End If
            If Not (Request(propertyTypeIDParam) Is Nothing) Then
                Integer.TryParse(Request(propertyTypeIDParam), _propertyTypeID)
                If (_propertyTypeID = 0) Then
                    Response.Redirect(NavigateURL(Me.TabId), True)
                End If
            End If

            If Not (Request("PropertyAgentID") Is Nothing) Then
                Integer.TryParse(Request("PropertyAgentID"), _propertyAgentID)
                If (_propertyAgentID = 0) Then
                    Response.Redirect(NavigateURL(Me.TabId), True)
                End If
            End If

            If Not (Request("PropertyBrokerID") Is Nothing) Then
                Integer.TryParse(Request("PropertyBrokerID"), _propertyBrokerID)
                If (_propertyBrokerID = 0) Then
                    Response.Redirect(NavigateURL(Me.TabId), True)
                End If
            End If

            If Not (Request("CustomFieldIDs") Is Nothing) Then
                _customFieldIDs = Request("CustomFieldIDs").Trim()
            End If

            If Not (Request("SearchValues") Is Nothing) Then
                _searchValues = Request("SearchValues").Trim()
            End If

            If Not (Request("sortBy") Is Nothing) Then
                _sortBy = Request("sortBy").Trim()
            End If

            If Not (Request("sortDir") Is Nothing) Then
                _sortDirection = Request("sortDir").Trim()
            End If

        End Sub

        Private Sub BindXml()

            Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)

            Dim objLayoutHeader As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.XML_Header_Html)
            Dim objLayoutItem As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.XML_Item_Html)
            Dim objLayoutPhoto As LayoutInfo = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.XML_Photo_Html)

            Dim objPropertyController As New PropertyController
            Dim OnlyForAuthenticated As Boolean = Null.NullBoolean
            If Me.UserId = -1 Then OnlyForAuthenticated = True
            Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(Me.ModuleId, _propertyTypeID, SearchStatusType.PublishedActive, _propertyAgentID, _propertyBrokerID, False, OnlyForAuthenticated, SortBy, SortByCustomField, SortDirection, _customFieldIDs, _searchValues, 0, Me.PropertySettings.XmlMaxRecords, _totalRecords, PropertySettings.ListingBubbleFeatured, True, Null.NullInteger, Null.NullInteger)

            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "text/xml"
            Response.ContentEncoding = Encoding.UTF8
            Me.EnableViewState = False

            Dim objPlaceHolder As New PlaceHolder
            objLayoutController.ProcessRSSHeader(objPlaceHolder.Controls, objLayoutHeader.Tokens, CustomFields, objProperties, ModuleConfiguration, objLayoutItem, objLayoutPhoto)

            Dim output As String = RenderControlAsString(objPlaceHolder) & vbCrLf
            If (Request("mode") = "Generate") Then
                If (Directory.Exists(Server.MapPath("~/feed/google")) = False) Then
                    Directory.CreateDirectory(Server.MapPath("~/feed/google"))
                End If
                File.WriteAllText(Server.MapPath("~/feed/google/google-local.xml"), output)
            Else
                Response.Write(output)
            End If

            Response.End()

        End Sub

        Private Function RenderControlAsString(ByVal objControl As Control) As String

            Dim sb As New StringBuilder
            Dim tw As New StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)

            objControl.RenderControl(hw)

            Return sb.ToString()

        End Function

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (PropertySettings.XmlEnable = False) Then
                Response.Redirect(NavigateURL(), True)
            End If
            ReadQueryString()
            BindXml()

        End Sub

#End Region

    End Class

End Namespace
