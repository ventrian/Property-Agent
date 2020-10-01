Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports Ventrian.PropertyAgent



Namespace Ventrian.PropertyAgent.Controls

    Public Class UploadPhotoHTML5
        Inherits PropertyAgentControl


#Region " Public Properties "

        ReadOnly Property GetMaximumFileSize() As String
            Get
                Return "1024"
            End Get
        End Property



#End Region


        Protected Sub cmdAttachPhoto_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If (Me.txtExternalUrl.Text.Trim <> "") Then
                    Dim imageByUrl As Drawing.Image = Me.GetImageByUrl(Me.txtExternalUrl.Text.Trim)
                    If (Not imageByUrl Is Nothing) Then
                        Dim objPhoto As New PhotoInfo With {
                            .PropertyID = Me._propertyID
                        }
                        If (Me._propertyID = Null.NullInteger) Then
                            objPhoto.PropertyGuid = Me._propertyGuid
                        End If
                        objPhoto.DateCreated = DateTime.Now
                        objPhoto.Filename = "Temporary.jpg"
                        objPhoto.SortOrder = 0
                        objPhoto.Title = ""
                        objPhoto.Width = imageByUrl.Width
                        objPhoto.Height = imageByUrl.Height
                        objPhoto.PhotoType = PhotoType.External
                        objPhoto.ExternalUrl = Me.txtExternalUrl.Text.Trim
                        objPhoto.Category = Me.drpCategories.SelectedValue
                        If (Me.Photos.Count > 0) Then
                            objPhoto.SortOrder = (DirectCast(Me.Photos.Item((Me.Photos.Count - 1)), PhotoInfo).SortOrder + 1)
                        End If
                        Dim controller As New PhotoController
                        objPhoto.PhotoID = controller.Add(objPhoto)
                        Select Case Me._imageType.ToLower
                            Case "image/jpeg"
                                objPhoto.Filename = (objPhoto.PhotoID.ToString & ".jpg")
                                Exit Select
                            Case "image/gif"
                                objPhoto.Filename = (objPhoto.PhotoID.ToString & ".gif")
                                Exit Select
                            Case Else
                                objPhoto.Filename = (objPhoto.PhotoID.ToString & ".jpg")
                                Exit Select
                        End Select
                        controller.Update(objPhoto)
                        imageByUrl.Dispose()
                        Me.txtExternalUrl.Text = ""
                        If TypeOf Me.Parent Is EditPhotos Then
                            DirectCast(Me.Parent, EditPhotos).RefreshPhotos()
                        Else
                            DirectCast(Me.Parent.Parent, EditProperty).RefreshPhotos()
                        End If
                    End If
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(Me, exc)
                ProjectData.ClearProjectError()
            End Try
        End Sub

        Protected Sub cmdRefreshPhotos_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If TypeOf Me.Parent Is EditPhotos Then
                    DirectCast(Me.Parent, EditPhotos).RefreshPhotos()
                Else
                    DirectCast(Me.Parent.Parent, EditProperty).RefreshPhotos()
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(Me, exc)
                ProjectData.ClearProjectError()
            End Try
        End Sub

        Protected Function GetCategory() As String
            Return Me.drpCategories.SelectedValue
        End Function

        Public Function GetImageByUrl(ByVal url As String) As Drawing.Image
            Dim response As WebResponse = Nothing
            Dim responseStream As Stream = Nothing
            Dim reader As StreamReader = Nothing
            Try
                Dim request As WebRequest = WebRequest.Create(Globals.AddHTTP(url))
                If (Not request Is Nothing) Then
                    response = request.GetResponse
                    If (Not response Is Nothing) Then
                        responseStream = response.GetResponseStream
                        Dim str As String = response.Headers.Item("Content-type")
                        Select Case str
                            Case "image/jpeg", "image/jpg"
                                Me._imageType = "image/jpeg"
                                Exit Select
                            Case Else
                                If (str = "image/gif") Then
                                    Me._imageType = "image/gif"
                                Else
                                    Return Nothing
                                End If
                                Exit Select
                        End Select
                        reader = New StreamReader(responseStream)
                        Dim image2 As Drawing.Image = Drawing.Image.FromStream(responseStream)
                        If (image2 Is Nothing) Then
                            Return Nothing
                        End If
                        Return image2
                    End If
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            Finally
                If (Not response Is Nothing) Then
                    response.Close()
                End If
                If (Not responseStream Is Nothing) Then
                    responseStream.Close()
                End If
                If (Not reader Is Nothing) Then
                    reader.Close()
                End If
            End Try
            Return Nothing
        End Function



        Protected Function GetPostBackReference() As String
            Return Me.Page.ClientScript.GetPostBackEventReference(Me.cmdRefreshPhotos, "Refresh")
        End Function

        Protected Function GetPropertyID() As String
            Dim nullInteger As Integer = Null.NullInteger
            Dim sEOPropertyID As String = MyBase.PropertySettings.SEOPropertyID
            If (MyBase.Request.Item(sEOPropertyID) = "") Then
                sEOPropertyID = "PropertyID"
            End If
            If (Not MyBase.Request.Item(sEOPropertyID) Is Nothing) Then
                nullInteger = Convert.ToInt32(MyBase.Request.Item(sEOPropertyID))
            End If
            Return nullInteger.ToString
        End Function

        Protected Function GetUploadLimit() As String
            If (MyBase.PropertyAgentBase.GetLimit(MyBase.PropertySettings.PermissionAddImages, MyBase.PropertySettings.PermissionAddImagesLimit) = Null.NullInteger) Then
                Return "0"
            End If
            Return "1"
        End Function

        Protected Function GetUploadUrl() As String
            Return Me.Page.ResolveUrl(("~/DesktopModules/PropertyAgent/Controls/SWFUploader.ashx?PortalID=" & MyBase.PropertyAgentBase.PortalId.ToString))
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            ClientResourceManager.RegisterStyleSheet(Me.Page, MyBase.ResolveUrl("~/DesktopModules/PropertyAgent/JS/fileuploader/jquery.fileuploader.min.css"), 100)
            ClientResourceManager.RegisterStyleSheet(Me.Page, MyBase.ResolveUrl("~/DesktopModules/PropertyAgent/JS/fileuploader/font/font-fileuploader.css"), 100)
            ClientResourceManager.RegisterScript(Me.Page, MyBase.ResolveUrl("~/DesktopModules/PropertyAgent/JS/fileuploader/jquery.fileuploader.min.js"), 100)
            Try
                Me.lblSelectImages.Text = Localization.GetString("SelectImages", "~/DesktopModules/PropertyAgent/App_LocalResources/SharedResources.ascx.resx")
                Me.ReadQueryString()
                Me.litModuleID.Text = MyBase.PropertyAgentBase.ModuleId.ToString
                Me.litTabModuleID.Text = MyBase.PropertyAgentBase.TabModuleId.ToString
                If MyBase.Request.IsAuthenticated Then
                    Me.litTicketID.Text = MyBase.Request.Cookies.Item(FormsAuthentication.FormsCookieName).Value
                End If
                'Me.litPropertyGuid.Text = Me.PropertyGuid.ToString
                If Not MyBase.IsPostBack Then
                    If (MyBase.PropertyAgentBase.PropertySettings.ImageCategories <> "") Then
                        Dim separator As Char() = New Char() {";"c}
                        Dim str As String
                        For Each str In MyBase.PropertyAgentBase.PropertySettings.ImageCategories.Split(separator)
                            Me.drpCategories.Items.Add(New ListItem(str, str))
                            Me.drpCategoriesExternal.Items.Add(New ListItem(str, str))
                        Next
                        Me.drpCategories.Items.Insert(0, New ListItem("-- Unassigned --", ""))
                        Me.drpCategoriesExternal.Items.Insert(0, New ListItem("-- Unassigned --", ""))
                    Else
                        Me.trImageCategories.Visible = False
                        Me.trImageCategoriesExternal.Visible = False
                    End If
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(Me, exc)
                ProjectData.ClearProjectError()
            End Try
        End Sub

        Private Sub ReadQueryString()
            Dim sEOPropertyID As String = MyBase.PropertySettings.SEOPropertyID
            If ((MyBase.Request.Item(sEOPropertyID) <> "") AndAlso Versioned.IsNumeric(MyBase.Request.Item(sEOPropertyID))) Then
                Me._propertyID = Convert.ToInt32(MyBase.Request.Item(sEOPropertyID))
            End If
        End Sub


        ' Properties
        Protected Overridable Property cmdAttachPhoto As LinkButton
            <CompilerGenerated>
            Get
                Return Me._cmdAttachPhoto
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As LinkButton)
                Dim handler As EventHandler = New EventHandler(AddressOf Me.cmdAttachPhoto_Click)
                Dim button As LinkButton = Me._cmdAttachPhoto
                If (Not button Is Nothing) Then
                    RemoveHandler button.Click, handler
                End If
                Me._cmdAttachPhoto = WithEventsValue
                button = Me._cmdAttachPhoto
                If (Not button Is Nothing) Then
                    AddHandler button.Click, handler
                End If
            End Set
        End Property

        Protected Overridable Property cmdRefreshPhotos As RefreshControl
            <CompilerGenerated>
            Get
                Return Me._cmdRefreshPhotos
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As RefreshControl)
                Dim handler As EventHandler = New EventHandler(AddressOf Me.cmdRefreshPhotos_Click)
                Dim control As RefreshControl = Me._cmdRefreshPhotos
                If (Not control Is Nothing) Then
                    RemoveHandler control.Click, handler
                End If
                Me._cmdRefreshPhotos = WithEventsValue
                control = Me._cmdRefreshPhotos
                If (Not control Is Nothing) Then
                    AddHandler control.Click, handler
                End If
            End Set
        End Property

        Protected Overridable Property drpCategories As DropDownList
            <CompilerGenerated>
            Get
                Return Me._drpCategories
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As DropDownList)
                Me._drpCategories = WithEventsValue
            End Set
        End Property

        Protected Overridable Property drpCategoriesExternal As DropDownList
            <CompilerGenerated>
            Get
                Return Me._drpCategoriesExternal
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As DropDownList)
                Me._drpCategoriesExternal = WithEventsValue
            End Set
        End Property

        Protected Overridable Property dshExternalPhoto As UserControl
            <CompilerGenerated>
            Get
                Return Me._dshExternalPhoto
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UserControl)
                Me._dshExternalPhoto = WithEventsValue
            End Set
        End Property

        Protected Overridable Property dshUploadPhoto As UserControl
            <CompilerGenerated>
            Get
                Return Me._dshUploadPhoto
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UserControl)
                Me._dshUploadPhoto = WithEventsValue
            End Set
        End Property

        Protected Overridable Property lblSelectImages As Label
            <CompilerGenerated>
            Get
                Return Me._lblSelectImages
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Label)
                Me._lblSelectImages = WithEventsValue
            End Set
        End Property

        Protected Overridable Property lblUploadExternalPhotoHelp As Label
            <CompilerGenerated>
            Get
                Return Me._lblUploadExternalPhotoHelp
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Label)
                Me._lblUploadExternalPhotoHelp = WithEventsValue
            End Set
        End Property

        Protected Overridable Property lblUploadPhotoHelp As Label
            <CompilerGenerated>
            Get
                Return Me._lblUploadPhotoHelp
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Label)
                Me._lblUploadPhotoHelp = WithEventsValue
            End Set
        End Property

        Protected Overridable Property litModuleID As Literal
            <CompilerGenerated>
            Get
                Return Me._litModuleID
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Literal)
                Me._litModuleID = WithEventsValue
            End Set
        End Property

        Protected Overridable Property litPropertyGuid As Literal
            <CompilerGenerated>
            Get
                Return Me._litPropertyGuid
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Literal)
                Me._litPropertyGuid = WithEventsValue
            End Set
        End Property

        Protected Overridable Property litTabModuleID As Literal
            <CompilerGenerated>
            Get
                Return Me._litTabModuleID
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Literal)
                Me._litTabModuleID = WithEventsValue
            End Set
        End Property

        Protected Overridable Property litTicketID As Literal
            <CompilerGenerated>
            Get
                Return Me._litTicketID
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As Literal)
                Me._litTicketID = WithEventsValue
            End Set
        End Property

        Private ReadOnly Property Photos As ArrayList
            Get
                If (Me._photos Is Nothing) Then
                    Dim controller As New PhotoController
                    If (Me._propertyID = Null.NullInteger) Then
                        Me._photos = controller.List(Me._propertyID, Me._propertyGuid)
                    Else
                        Me._photos = controller.List(Me._propertyID)
                    End If
                End If
                Return Me._photos
            End Get
        End Property

        Protected Overridable Property plExternalUrl As UserControl
            <CompilerGenerated>
            Get
                Return Me._plExternalUrl
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UserControl)
                Me._plExternalUrl = WithEventsValue
            End Set
        End Property

        Protected Overridable Property plImage As UserControl
            <CompilerGenerated>
            Get
                Return Me._plImage
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UserControl)
                Me._plImage = WithEventsValue
            End Set
        End Property

        Protected Overridable Property plImageCategories As UserControl
            <CompilerGenerated>
            Get
                Return Me._plImageCategories
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UserControl)
                Me._plImageCategories = WithEventsValue
            End Set
        End Property

        Protected Overridable Property plImageCategoriesExternal As UserControl
            <CompilerGenerated>
            Get
                Return Me._plImageCategoriesExternal
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As UserControl)
                Me._plImageCategoriesExternal = WithEventsValue
            End Set
        End Property

        Public Property PropertyGuid As String
            Get
                Return Me._propertyGuid
            End Get
            Set(ByVal value As String)
                Me._propertyGuid = value
            End Set
        End Property

        Protected Overridable Property tblUploadExternalPhoto As HtmlTable
            <CompilerGenerated>
            Get
                Return Me._tblUploadExternalPhoto
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As HtmlTable)
                Me._tblUploadExternalPhoto = WithEventsValue
            End Set
        End Property

        Protected Overridable Property tblUploadPhoto As HtmlTable
            <CompilerGenerated>
            Get
                Return Me._tblUploadPhoto
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As HtmlTable)
                Me._tblUploadPhoto = WithEventsValue
            End Set
        End Property

        Protected Overridable Property trImageCategories As HtmlTableRow
            <CompilerGenerated>
            Get
                Return Me._trImageCategories
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As HtmlTableRow)
                Me._trImageCategories = WithEventsValue
            End Set
        End Property

        Protected Overridable Property trImageCategoriesExternal As HtmlTableRow
            <CompilerGenerated>
            Get
                Return Me._trImageCategoriesExternal
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As HtmlTableRow)
                Me._trImageCategoriesExternal = WithEventsValue
            End Set
        End Property

        Protected Overridable Property txtExternalUrl As TextBox
            <CompilerGenerated>
            Get
                Return Me._txtExternalUrl
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), CompilerGenerated>
            Set(ByVal WithEventsValue As TextBox)
                Me._txtExternalUrl = WithEventsValue
            End Set
        End Property


        ' Fields
        <CompilerGenerated, AccessedThroughProperty("cmdAttachPhoto")>
        Private _cmdAttachPhoto As LinkButton
        <CompilerGenerated, AccessedThroughProperty("cmdRefreshPhotos")>
        Private _cmdRefreshPhotos As RefreshControl
        <CompilerGenerated, AccessedThroughProperty("drpCategories")>
        Private _drpCategories As DropDownList
        <CompilerGenerated, AccessedThroughProperty("drpCategoriesExternal")>
        Private _drpCategoriesExternal As DropDownList
        <CompilerGenerated, AccessedThroughProperty("dshExternalPhoto")>
        Private _dshExternalPhoto As UserControl
        <CompilerGenerated, AccessedThroughProperty("dshUploadPhoto")>
        Private _dshUploadPhoto As UserControl
        Private _imageType As String
        <CompilerGenerated, AccessedThroughProperty("lblSelectImages")>
        Private _lblSelectImages As Label
        <CompilerGenerated, AccessedThroughProperty("lblUploadExternalPhotoHelp")>
        Private _lblUploadExternalPhotoHelp As Label
        <CompilerGenerated, AccessedThroughProperty("lblUploadPhotoHelp")>
        Private _lblUploadPhotoHelp As Label
        <CompilerGenerated, AccessedThroughProperty("litModuleID")>
        Private _litModuleID As Literal
        <CompilerGenerated, AccessedThroughProperty("litPropertyGuid")>
        Private _litPropertyGuid As Literal
        <CompilerGenerated, AccessedThroughProperty("litTabModuleID")>
        Private _litTabModuleID As Literal
        <CompilerGenerated, AccessedThroughProperty("litTicketID")>
        Private _litTicketID As Literal
        Private _photos As ArrayList
        <CompilerGenerated, AccessedThroughProperty("plExternalUrl")>
        Private _plExternalUrl As UserControl
        <CompilerGenerated, AccessedThroughProperty("plImage")>
        Private _plImage As UserControl
        <CompilerGenerated, AccessedThroughProperty("plImageCategories")>
        Private _plImageCategories As UserControl
        <CompilerGenerated, AccessedThroughProperty("plImageCategoriesExternal")>
        Private _plImageCategoriesExternal As UserControl
        Private _propertyGuid As String
        Private _propertyID As Integer
        <CompilerGenerated, AccessedThroughProperty("tblUploadExternalPhoto")>
        Private _tblUploadExternalPhoto As HtmlTable
        <CompilerGenerated, AccessedThroughProperty("tblUploadPhoto")>
        Private _tblUploadPhoto As HtmlTable
        <CompilerGenerated, AccessedThroughProperty("trImageCategories")>
        Private _trImageCategories As HtmlTableRow
        <CompilerGenerated, AccessedThroughProperty("trImageCategoriesExternal")>
        Private _trImageCategoriesExternal As HtmlTableRow
        <CompilerGenerated, AccessedThroughProperty("txtExternalUrl")>
        Private _txtExternalUrl As TextBox
    End Class
End Namespace
