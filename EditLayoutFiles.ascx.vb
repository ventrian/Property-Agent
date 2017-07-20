Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class EditLayoutFiles
        Inherits PropertyAgentBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objLayoutFiles As New CrumbInfo
            objLayoutFiles.Caption = Localization.GetString("EditLayoutFiles", LocalResourceFile)
            objLayoutFiles.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditLayoutFiles")
            crumbs.Add(objLayoutFiles)

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

        Private Sub BindLayoutGroups()

            For Each value As Integer In System.Enum.GetValues(GetType(LayoutGroupType))
                If (value = LayoutGroupType.Submission = False And value = LayoutGroupType.Contact = False And value = LayoutGroupType.ContactBroker = False And value = LayoutGroupType.ContactOwner = False And value = LayoutGroupType.SendToFriend = False And value = LayoutGroupType.CommentNotification = False) Then
                    Dim li As New ListItem
                    li.Value = System.Enum.GetName(GetType(LayoutGroupType), value)
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(LayoutGroupType), value), Me.LocalResourceFile)
                    drpLayoutGroups.Items.Add(li)
                End If
            Next

        End Sub

        Private Sub BindDetails()

            lblCurrentTemplate.Text = Me.PropertySettings.Template
            lblLayoutGroup.Text = Localization.GetString(drpLayoutGroups.SelectedValue & "Description", Me.LocalResourceFile)

            Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)
            Dim objLayoutGroup As LayoutGroupType = CType(System.Enum.Parse(GetType(LayoutGroupType), drpLayoutGroups.SelectedValue), LayoutGroupType)

            trHeader.Visible = False
            trItem.Visible = False
            trAlternate.Visible = False
            trSeparator.Visible = False
            trFooter.Visible = False
            trPhotoFirst.Visible = False
            trPhoto.Visible = False
            trPageTitle.Visible = False
            trPageDescription.Visible = False
            trPageKeywords.Visible = False
            trPageHeader.Visible = False

            txtHeader.Text = ""
            txtItem.Text = ""
            txtAlternate.Text = ""
            txtSeparator.Text = ""
            txtFooter.Text = ""
            txtPhotoFirst.Text = ""
            txtPhoto.Text = ""
            txtPageTitle.Text = ""
            txtPageDescription.Text = ""
            txtPageKeywords.Text = ""

            Select Case objLayoutGroup

                Case LayoutGroupType.Comment
                    trHeader.Visible = True
                    trItem.Visible = True
                    trSeparator.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Comment_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Comment_Item_Html).Template
                    txtSeparator.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Comment_Separator_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Comment_Footer_Html).Template

                Case LayoutGroupType.Contact
                    trHeader.Visible = True
                    trItem.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Subject_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Body_Html).Template

                Case LayoutGroupType.Export
                    trHeader.Visible = True
                    trItem.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Export_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Export_Item_Html).Template

                Case LayoutGroupType.Featured
                    trHeader.Visible = True
                    trItem.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Featured_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Featured_Item_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Featured_Footer_Html).Template

                Case LayoutGroupType.Latest
                    trHeader.Visible = True
                    trItem.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Latest_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Latest_Item_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Latest_Footer_Html).Template

                Case LayoutGroupType.Listing
                    trHeader.Visible = True
                    trItem.Visible = True
                    trAlternate.Visible = True
                    trSeparator.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Item_Html).Template
                    txtAlternate.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Alternate_Html).Template
                    txtSeparator.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Separator_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Listing_Footer_Html).Template

                Case LayoutGroupType.Message
                    trItem.Visible = True

                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Message_Item_Html).Template

                Case LayoutGroupType.[Option]
                    trItem.Visible = True

                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Option_Item_Html).Template

                Case LayoutGroupType.Pdf
                    trHeader.Visible = True
                    trItem.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Pdf_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Pdf_Item_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Pdf_Footer_Html).Template

                Case LayoutGroupType.Photo
                    trItem.Visible = True
                    trPhotoFirst.Visible = True

                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Photo_Item_Html).Template
                    txtPhotoFirst.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Photo_First_Html).Template

                Case LayoutGroupType.Print
                    trHeader.Visible = True
                    trItem.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Print_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Print_Item_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Print_Footer_Html).Template

                Case LayoutGroupType.Review
                    trHeader.Visible = True
                    trItem.Visible = True
                    trSeparator.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Review_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Review_Item_Html).Template
                    txtSeparator.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Review_Separator_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Review_Footer_Html).Template

                Case LayoutGroupType.RSS
                    trHeader.Visible = True
                    trItem.Visible = True
                    trPhoto.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Item_Html).Template
                    txtPhoto.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Photo_Html).Template

                Case LayoutGroupType.RSSReader
                    trHeader.Visible = True
                    trItem.Visible = True
                    trFooter.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Item_Html).Template
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Footer_Html).Template

                Case LayoutGroupType.Search
                    trHeader.Visible = True
                    trItem.Visible = True
                    trAlternate.Visible = False
                    trSeparator.Visible = False
                    trFooter.Visible = True
                    trPhotoFirst.Visible = False
                    trPageTitle.Visible = False
                    trPageDescription.Visible = False
                    trPageKeywords.Visible = False

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Item_Html).Template
                    txtAlternate.Text = ""
                    txtSeparator.Text = ""
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Search_Footer_Html).Template
                    txtPhotoFirst.Text = ""
                    txtPageTitle.Text = ""
                    txtPageDescription.Text = ""
                    txtPageKeywords.Text = ""


                Case LayoutGroupType.Stylesheet
                    trHeader.Visible = False
                    trItem.Visible = True
                    trAlternate.Visible = False
                    trSeparator.Visible = False
                    trFooter.Visible = False
                    trPhotoFirst.Visible = False
                    trPageTitle.Visible = False
                    trPageDescription.Visible = False
                    trPageKeywords.Visible = False

                    txtHeader.Text = ""
                    txtItem.Text = objLayoutController.GetStylesheet(Me.PropertySettings.Template)
                    txtAlternate.Text = ""
                    txtSeparator.Text = ""
                    txtFooter.Text = ""
                    txtPhotoFirst.Text = ""
                    txtPageTitle.Text = ""
                    txtPageDescription.Text = ""
                    txtPageKeywords.Text = ""

                Case LayoutGroupType.Type
                    trHeader.Visible = True
                    trItem.Visible = True
                    trAlternate.Visible = False
                    trSeparator.Visible = False
                    trFooter.Visible = True
                    trPhotoFirst.Visible = False
                    trPageTitle.Visible = True
                    trPageDescription.Visible = True
                    trPageKeywords.Visible = True
                    trPageHeader.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Item_Html).Template
                    txtAlternate.Text = ""
                    txtSeparator.Text = ""
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_Footer_Html).Template
                    txtPhotoFirst.Text = ""
                    txtPageTitle.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageTitle_Html).Template
                    txtPageDescription.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageDescription_Html).Template
                    txtPageKeywords.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageKeyword_Html).Template
                    txtPageHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Type_PageHeader_Html).Template

                Case LayoutGroupType.Types
                    trHeader.Visible = True
                    trItem.Visible = True
                    trAlternate.Visible = False
                    trSeparator.Visible = False
                    trFooter.Visible = True
                    trPhotoFirst.Visible = False
                    trPageTitle.Visible = False
                    trPageDescription.Visible = False
                    trPageKeywords.Visible = False

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Types_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Types_Item_Html).Template
                    txtAlternate.Text = ""
                    txtSeparator.Text = ""
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.Types_Footer_Html).Template
                    txtPhotoFirst.Text = ""
                    txtPageTitle.Text = ""
                    txtPageDescription.Text = ""
                    txtPageKeywords.Text = ""

                Case LayoutGroupType.View
                    trHeader.Visible = True
                    trItem.Visible = True
                    trAlternate.Visible = False
                    trSeparator.Visible = False
                    trFooter.Visible = True
                    trPhotoFirst.Visible = False
                    trPageTitle.Visible = True
                    trPageDescription.Visible = True
                    trPageKeywords.Visible = True
                    trPageHeader.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_Item_Html).Template
                    txtAlternate.Text = ""
                    txtSeparator.Text = ""
                    txtFooter.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_Footer_Html).Template
                    txtPhotoFirst.Text = ""
                    txtPageTitle.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageTitle_Html).Template
                    txtPageDescription.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageDescription_Html).Template
                    txtPageKeywords.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageKeywords_Html).Template
                    txtPageHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.View_PageHeader_Html).Template

                Case LayoutGroupType.Xml
                    trHeader.Visible = True
                    trItem.Visible = True
                    trPhoto.Visible = True

                    txtHeader.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.XML_Header_Html).Template
                    txtItem.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.XML_Item_Html).Template
                    txtPhoto.Text = objLayoutController.GetLayout(Me.PropertySettings.Template, LayoutType.XML_Photo_Html).Template

            End Select

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (Page.IsPostBack = False) Then
                    BindLayoutGroups()
                    BindDetails()
                End If

                cmdUpdate.CssClass = PropertySettings.ButtonClass
                cmdCancel.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpTemplateGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpLayoutGroups.SelectedIndexChanged

            Try

                BindDetails()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                Dim objLayoutController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)
                Dim objLayoutGroup As LayoutGroupType = CType(System.Enum.Parse(GetType(LayoutGroupType), drpLayoutGroups.SelectedValue), LayoutGroupType)

                Select Case objLayoutGroup

                    Case LayoutGroupType.Comment
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Comment_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Comment_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Comment_Separator_Html, txtSeparator.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Comment_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Contact
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Subject_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.ContactEmail_Body_Html, txtItem.Text)

                    Case LayoutGroupType.Export
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Export_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Export_Item_Html, txtItem.Text)

                    Case LayoutGroupType.Featured
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Featured_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Featured_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Featured_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Latest
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Latest_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Latest_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Latest_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Listing
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Listing_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Listing_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Listing_Alternate_Html, txtAlternate.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Listing_Separator_Html, txtSeparator.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Listing_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Message
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Message_Item_Html, txtItem.Text)

                    Case LayoutGroupType.[Option]
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Option_Item_Html, txtItem.Text)

                    Case LayoutGroupType.Pdf
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Pdf_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Pdf_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Pdf_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Photo
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Photo_First_Html, txtPhotoFirst.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Photo_Item_Html, txtItem.Text)

                    Case LayoutGroupType.Print
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Print_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Print_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Print_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Review
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Review_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Review_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Review_Separator_Html, txtSeparator.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Review_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.RSS
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.RSS_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.RSS_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.RSS_Photo_Html, txtPhoto.Text)

                    Case LayoutGroupType.RSSReader
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.RSS_Reader_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Search
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Search_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Search_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Search_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.Stylesheet
                        objLayoutController.UpdateStylesheet(Me.PropertySettings.Template, txtItem.Text)

                    Case LayoutGroupType.Type
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Type_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Type_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Type_Footer_Html, txtFooter.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Type_PageTitle_Html, txtPageTitle.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Type_PageDescription_Html, txtPageDescription.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Type_PageKeyword_Html, txtPageKeywords.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Type_PageHeader_Html, txtPageHeader.Text)

                    Case LayoutGroupType.Types
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Types_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Types_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.Types_Footer_Html, txtFooter.Text)

                    Case LayoutGroupType.View
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.View_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.View_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.View_Footer_Html, txtFooter.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.View_PageTitle_Html, txtPageTitle.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.View_PageDescription_Html, txtPageDescription.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.View_PageKeywords_Html, txtPageKeywords.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.View_PageHeader_Html, txtPageHeader.Text)

                    Case LayoutGroupType.XML
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.XML_Header_Html, txtHeader.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.XML_Item_Html, txtItem.Text)
                        objLayoutController.UpdateLayout(Me.PropertySettings.Template, LayoutType.XML_Photo_Html, txtPhoto.Text)

                End Select

                lblLayoutFilesUpdated.Visible = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL, True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace