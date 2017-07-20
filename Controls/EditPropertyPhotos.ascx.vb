Imports System.IO

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent.Controls

    Partial Public Class EditPropertyPhotos
        Inherits PropertyAgentControl

#Region " Private Members "

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

#Region " Private Methods "

        Private Function GetResourceString(ByVal key As String, ByVal resourceFile As String, ByVal propertySettings As PropertySettings) As String

            Return PropertyUtil.FormatPropertyLabel(Localization.GetString(key, resourceFile), propertySettings)

        End Function

        Private Sub ReadQueryString()

            Dim propertyParam As String = PropertyAgentBase.PropertySettings.SEOPropertyID
            If (Request(propertyParam) = "") Then
                propertyParam = "PropertyID"
            End If
            If Not (Request(propertyParam) Is Nothing) Then
                _propertyID = Convert.ToInt32(Request(propertyParam))
            End If

        End Sub

#End Region

#Region " Public Methods "

        Protected Function GetCategory(ByVal category As String) As String

            If (category <> "") Then
                Return " [" & category & "]"
            End If

            Return ""

        End Function

        Public Sub BindPhotos()

            Dim objPhotoController As New PhotoController

            If (_propertyID = Null.NullInteger) Then
                _photos = objPhotoController.List(_propertyID, _propertyGuid)
            Else
                _photos = objPhotoController.List(_propertyID)
            End If

            If (_photos.Count > 1) Then
                If (CType(_photos(0), PhotoInfo).SortOrder = 0 And CType(_photos(1), PhotoInfo).SortOrder = 0) Then
                    ' Sort Order must be messed up, fix now..
                    For i As Integer = 0 To _photos.Count - 1
                        Dim objPhoto As PhotoInfo = CType(_photos(i), PhotoInfo)
                        objPhoto.SortOrder = i
                        objPhotoController.Update(objPhoto)
                    Next
                End If
            End If

            dlPhotos.DataSource = _photos
            dlPhotos.DataBind()

            rptPhotos.DataSource = _photos
            rptPhotos.DataBind()

            If (dlPhotos.Items.Count > 0) Then
                dlPhotos.Visible = True
                lblNoPhotos.Visible = False
            Else
                dlPhotos.Visible = False
                lblNoPhotos.Visible = True
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                ReadQueryString()
                DotNetNuke.Framework.jQuery.RequestUIRegistration()

                If (Page.IsPostBack = False) Then

                    BindPhotos()

                    cmdSortPhotos.Text = GetResourceString("SortPhotos", "~/DesktopModules/PropertyAgent/App_LocalResources/EditPhotos.ascx.resx", Me.PropertySettings)

                End If

                lblAssignedPhotosHelp.Text = GetResourceString("AssignedPhotosDescription", "~/DesktopModules/PropertyAgent/App_LocalResources/EditPhotos.ascx.resx", Me.PropertySettings)
                lblSortingInstructions.Text = GetResourceString("SortingInstructions", "~/DesktopModules/PropertyAgent/App_LocalResources/EditPhotos.ascx.resx", Me.PropertySettings)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlPhotos_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlPhotos.ItemDataBound

            Try

                If (e.Item.ItemType = Web.UI.WebControls.ListItemType.Item Or e.Item.ItemType = Web.UI.WebControls.ListItemType.AlternatingItem) Then

                    Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)

                    Dim btnEdit As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnEdit"), System.Web.UI.WebControls.ImageButton)
                    Dim btnDelete As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnDelete"), System.Web.UI.WebControls.ImageButton)
                    Dim btnUp As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnUp"), System.Web.UI.WebControls.ImageButton)
                    Dim btnDown As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnDown"), System.Web.UI.WebControls.ImageButton)

                    If Not (btnDelete Is Nothing) Then
                        btnDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeletePhoto", PropertyAgentBase.LocalResourceFile) & "');")

                        If Not (objPhoto Is Nothing) Then
                            btnDelete.CommandArgument = objPhoto.PhotoID.ToString()
                        End If

                    End If

                    If Not (btnEdit Is Nothing) Then

                        If Not (objPhoto Is Nothing) Then
                            btnEdit.CommandArgument = objPhoto.PhotoID.ToString()
                        End If

                    End If

                    If Not (btnUp Is Nothing And btnDown Is Nothing) Then

                        If (objPhoto.PhotoID = CType(_photos(0), PhotoInfo).PhotoID) Then
                            btnUp.Visible = False
                        End If

                        If (objPhoto.PhotoID = CType(_photos(_photos.Count - 1), PhotoInfo).PhotoID) Then
                            btnDown.Visible = False
                        End If

                        btnUp.CommandArgument = objPhoto.PhotoID.ToString()
                        btnUp.CommandName = "Up"
                        btnUp.CausesValidation = False

                        btnDown.CommandArgument = objPhoto.PhotoID.ToString()
                        btnDown.CommandName = "Down"
                        btnDown.CausesValidation = False

                    End If

                End If

                If (e.Item.ItemType = Web.UI.WebControls.ListItemType.EditItem) Then

                    Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)
                    Dim drpCategory As DropDownList = e.Item.FindControl("drpCategory")

                    If (drpCategory IsNot Nothing) Then
                        If (PropertyAgentBase.PropertySettings.ImageCategories <> "") Then
                            For Each item As String In PropertyAgentBase.PropertySettings.ImageCategories.Split(";"c)
                                drpCategory.Items.Add(New ListItem(item, item))
                            Next
                            drpCategory.Items.Insert(0, New ListItem("-- Unassigned --", ""))

                            If (drpCategory.Items.FindByValue(objPhoto.Category) IsNot Nothing) Then
                                drpCategory.SelectedValue = objPhoto.Category
                            End If
                        Else
                            drpCategory.Visible = False
                        End If
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlPhotos_OnItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlPhotos.ItemCommand

            Try

                Dim objPhotoController As New PhotoController
                _photos = objPhotoController.List(_propertyID)

                If (e.CommandName = "Delete") Then

                    Dim objPhoto As PhotoInfo = objPhotoController.Get(Convert.ToInt32(e.CommandArgument))

                    If Not (objPhoto Is Nothing) Then
                        If (File.Exists(Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent/" & Me.PropertyAgentBase.ModuleId & "/Images/" & objPhoto.Filename)) Then
                            File.Delete(Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent/" & Me.PropertyAgentBase.ModuleId & "/Images/" & objPhoto.Filename)
                        End If
                        objPhotoController.Delete(Convert.ToInt32(e.CommandArgument))
                    End If

                End If

                If (e.CommandName = "Edit") Then

                    dlPhotos.EditItemIndex = e.Item.ItemIndex

                End If

                If (e.CommandName = "Up") Then

                    Dim photoID As Integer = Convert.ToInt32(e.CommandArgument)

                    For i As Integer = 0 To _photos.Count - 1
                        Dim objPhoto As PhotoInfo = CType(_photos(i), PhotoInfo)
                        If (photoID = objPhoto.PhotoID) Then

                            Dim objPhotoToSwap As PhotoInfo = CType(_photos(i - 1), PhotoInfo)

                            Dim sortOrder As Integer = objPhoto.SortOrder
                            Dim sortOrderPrevious As Integer = objPhotoToSwap.SortOrder

                            objPhoto.SortOrder = sortOrderPrevious
                            objPhotoToSwap.SortOrder = sortOrder

                            objPhotoController.Update(objPhoto)
                            objPhotoController.Update(objPhotoToSwap)

                        End If
                    Next

                End If

                If (e.CommandName = "Down") Then

                    Dim photoID As Integer = Convert.ToInt32(e.CommandArgument)

                    For i As Integer = 0 To _photos.Count - 1
                        Dim objPhoto As PhotoInfo = CType(_photos(i), PhotoInfo)
                        If (photoID = objPhoto.PhotoID) Then
                            Dim objPhotoToSwap As PhotoInfo = CType(_photos(i + 1), PhotoInfo)

                            Dim sortOrder As Integer = objPhoto.SortOrder
                            Dim sortOrderNext As Integer = objPhotoToSwap.SortOrder

                            objPhoto.SortOrder = sortOrderNext
                            objPhotoToSwap.SortOrder = sortOrder

                            objPhotoController.Update(objPhoto)
                            objPhotoController.Update(objPhotoToSwap)
                        End If
                    Next

                End If

                If (e.CommandName = "Cancel") Then

                    dlPhotos.EditItemIndex = -1

                End If

                If (e.CommandName = "Update") Then

                    Dim txtTitle As TextBox = e.Item.FindControl("txtTitle")
                    Dim drpCategory As DropDownList = e.Item.FindControl("drpCategory")

                    Dim objPhoto As PhotoInfo = objPhotoController.Get(Convert.ToInt32(dlPhotos.DataKeys(e.Item.ItemIndex)))

                    If Not (objPhoto Is Nothing) Then
                        objPhoto.Title = txtTitle.Text
                        If (drpCategory.Visible) Then
                            objPhoto.Category = drpCategory.SelectedValue
                        End If
                        objPhotoController.Update(objPhoto)
                    End If

                    dlPhotos.EditItemIndex = -1

                End If

                BindPhotos()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSortPhotos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSortPhotos.Click

            Try

                phEditing.Visible = Not phEditing.Visible
                phSorting.Visible = Not phSorting.Visible

                If (phSorting.Visible) Then
                    cmdSortPhotos.Text = GetResourceString("StopSorting", "~/DesktopModules/PropertyAgent/App_LocalResources/EditPhotos.ascx.resx", Me.PropertySettings)
                Else
                    cmdSortPhotos.Text = GetResourceString("SortPhotos", "~/DesktopModules/PropertyAgent/App_LocalResources/EditPhotos.ascx.resx", Me.PropertySettings)
                    BindPhotos()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace