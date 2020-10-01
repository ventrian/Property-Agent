Imports System.IO
Imports System.Net

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent.Controls

    Public Class UploadPhotoSWF
        Inherits PropertyAgentControl

#Region " Private Members "

        Private _imageType As String = ""
        Private _propertyID As Integer = Null.NullInteger
        Private _propertyGuid As String = Null.NullString

        Private _photos As ArrayList

#End Region

#Region " Public Properties "

        Public Property PropertyGuid() As String
            Get
                Return _propertyGuid
            End Get
            Set(ByVal value As String)
                _propertyGuid = value
            End Set
        End Property

#End Region

#Region " Private Properties "

        Private ReadOnly Property Photos() As ArrayList
            Get
                If (_photos Is Nothing) Then
                    Dim objPhotoController As New PhotoController()
                    If (_propertyID = Null.NullInteger) Then
                        _photos = objPhotoController.List(_propertyID, _propertyGuid)
                    Else
                        _photos = objPhotoController.List(_propertyID)
                    End If
                End If
                Return _photos
            End Get
        End Property
#End Region

#Region " Private Methods "

        Public Function GetImageByUrl(ByVal url As String) As System.Drawing.Image

            Dim response As WebResponse = Nothing
            Dim remoteStream As Stream = Nothing
            Dim readStream As StreamReader = Nothing
            Try
                Dim request As WebRequest = WebRequest.Create(DotNetNuke.Common.AddHTTP(url))
                If request IsNot Nothing Then
                    response = request.GetResponse()
                    If response IsNot Nothing Then
                        remoteStream = response.GetResponseStream()

                        Dim content_type As String = response.Headers("Content-type")

                        Dim imageType As String = ""

                        If content_type = "image/jpeg" OrElse content_type = "image/jpg" Then
                            _imageType = "image/jpeg"
                        ElseIf content_type = "image/gif" Then
                            _imageType = "image/gif"
                        Else
                            Return Nothing
                        End If

                        readStream = New StreamReader(remoteStream)

                        Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(remoteStream)

                        If img Is Nothing Then
                            Return Nothing
                        End If

                        ' img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg)
                        Return img
                    End If
                End If
            Catch

            Finally
                If response IsNot Nothing Then
                    response.Close()
                End If
                If remoteStream IsNot Nothing Then
                    remoteStream.Close()
                End If
                If readStream IsNot Nothing Then
                    readStream.Close()
                End If
            End Try

            Return Nothing

        End Function

        Private Sub ReadQueryString()

            Dim propertyIDParam As String = PropertySettings.SEOPropertyID
            If (Request(propertyIDParam) <> "") Then
                If (IsNumeric(Request(propertyIDParam))) Then
                    _propertyID = Convert.ToInt32(Request(propertyIDParam))
                End If
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetCategory() As String

            Return drpCategories.SelectedValue

        End Function

        Protected Function GetMaximumFileSize() As String

            Return "10240"

        End Function

        Protected Function GetPostBackReference() As String

            Return Page.ClientScript.GetPostBackEventReference(cmdRefreshPhotos, "Refresh")

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

        Protected Function GetUploadLimit() As String

            Dim limit As Integer = PropertyAgentBase.GetLimit(PropertySettings.PermissionAddImages, PropertySettings.PermissionAddImagesLimit)

            If (limit = Null.NullInteger) Then
                Return "0"
            Else
                Return "1"
            End If

        End Function

        Protected Function GetUploadUrl() As String

            Return Page.ResolveUrl("~/DesktopModules/PropertyAgent/Controls/SWFUploader.ashx?PortalID=" & PropertyAgentBase.PortalId.ToString())

        End Function

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                lblSelectImages.Text = Localization.GetString("SelectImages", "~/DesktopModules/PropertyAgent/App_LocalResources/SharedResources.ascx.resx")

                ReadQueryString()

                litModuleID.Text = Me.PropertyAgentBase.ModuleId.ToString()
                litTabModuleID.Text = Me.PropertyAgentBase.TabModuleId.ToString()
                If (Request.IsAuthenticated) Then
                    litTicketID.Text = Request.Cookies(System.Web.Security.FormsAuthentication.FormsCookieName()).Value
                End If
                litPropertyGuid.Text = PropertyGuid.ToString()

                If (IsPostBack = False) Then

                    If (PropertyAgentBase.PropertySettings.ImageCategories <> "") Then
                        For Each item As String In PropertyAgentBase.PropertySettings.ImageCategories.Split(";"c)
                            drpCategories.Items.Add(New ListItem(item, item))
                            drpCategoriesExternal.Items.Add(New ListItem(item, item))
                        Next
                        drpCategories.Items.Insert(0, New ListItem("-- Unassigned --", ""))
                        drpCategoriesExternal.Items.Insert(0, New ListItem("-- Unassigned --", ""))
                    Else
                        trImageCategories.Visible = False
                        trImageCategoriesExternal.Visible = False
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdRefreshPhotos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRefreshPhotos.Click

            Try

                If (TypeOf Parent Is EditPhotos) Then
                    CType(Parent, EditPhotos).RefreshPhotos()
                Else
                    CType(Parent.Parent, EditProperty).RefreshPhotos()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdAttachPhoto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAttachPhoto.Click

            Try

                If (txtExternalUrl.Text.Trim() <> "") Then

                    Dim objImage As System.Drawing.Image = GetImageByUrl(txtExternalUrl.Text.Trim())

                    If (objImage IsNot Nothing) Then

                        Dim objPhoto As New PhotoInfo

                        objPhoto.PropertyID = _propertyID
                        If (_propertyID = Null.NullInteger) Then
                            objPhoto.PropertyGuid = _propertyGuid
                        End If
                        objPhoto.DateCreated = DateTime.Now
                        objPhoto.Filename = "Temporary.jpg"
                        objPhoto.SortOrder = 0
                        objPhoto.Title = ""
                        objPhoto.Width = objImage.Width
                        objPhoto.Height = objImage.Height
                        objPhoto.PhotoType = PhotoType.External
                        objPhoto.ExternalUrl = txtExternalUrl.Text.Trim()
                        objPhoto.Category = drpCategories.SelectedValue

                        If (Photos.Count > 0) Then
                            objPhoto.SortOrder = CType(Photos(Photos.Count - 1), PhotoInfo).SortOrder + 1
                        End If

                        Dim objPhotoController As New PhotoController()
                        objPhoto.PhotoID = objPhotoController.Add(objPhoto)

                        Select Case _imageType.ToLower()
                            Case "image/jpeg"
                                objPhoto.Filename = objPhoto.PhotoID.ToString() & ".jpg"

                            Case "image/gif"
                                objPhoto.Filename = objPhoto.PhotoID.ToString() & ".gif"

                            Case Else
                                'Shouldn't get to here because of validators.
                                objPhoto.Filename = objPhoto.PhotoID.ToString() & ".jpg"
                        End Select

                        objPhotoController.Update(objPhoto)

                        objImage.Dispose()

                        txtExternalUrl.Text = ""

                        If (TypeOf Parent Is EditPhotos) Then
                            CType(Parent, EditPhotos).RefreshPhotos()
                        Else
                            CType(Parent.Parent, EditProperty).RefreshPhotos()
                        End If

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace