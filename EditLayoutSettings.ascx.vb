Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.PropertyAgent.Mapping

Namespace Ventrian.PropertyAgent

    Partial Public Class EditLayoutSettings
        Inherits PropertyAgentBase

#Region " Private Members "

        Dim _propertySettings As PropertySettings

#End Region

#Region " Private Methods "

        Private Sub BindReplyTo()

            For Each value As Integer In System.Enum.GetValues(GetType(ReplyToType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(ReplyToType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ReplyToType), value), Me.LocalResourceFile)

                If (value = ReplyToType.PortalAdmin) Then
                    li.Selected = True
                End If
                lstContactReply.Items.Add(li)
            Next

        End Sub

        Private Sub BindLandingPageType()

            For Each value As Integer In System.Enum.GetValues(GetType(LandingPageType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(LandingPageType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(LandingPageType), value), Me.LocalResourceFile)

                If (value = ReplyToType.PortalAdmin) Then
                    li.Selected = True
                End If
                lstLandingPageType.Items.Add(li)
            Next

        End Sub

        Private Sub BindLayoutType()

            For Each value As Integer In System.Enum.GetValues(GetType(LatestLayoutType))
                Dim objLayoutMode As LatestLayoutMode = CType(System.Enum.Parse(GetType(LatestLayoutType), value.ToString()), LatestLayoutType)
                Dim li As New ListItem
                li.Value = value.ToString()
                li.Text = Localization.GetString(System.Enum.GetName(GetType(LatestLayoutType), value), Me.LocalResourceFile)
                lstLayoutType.Items.Add(li)
                lstLayoutTypeFeatured.Items.Add(New ListItem(li.Text, li.Value))
                lstLayoutTypeTypes.Items.Add(New ListItem(li.Text, li.Value))
            Next

            lstLayoutType.Items(1).Selected = True
            lstLayoutTypeFeatured.Items(1).Selected = True
            lstLayoutTypeTypes.Items(1).Selected = True

        End Sub

        Private Sub BindBreadcrumbPlacement()

            For Each value As Integer In System.Enum.GetValues(GetType(BreadcrumbType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(BreadcrumbType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(BreadcrumbType), value), Me.LocalResourceFile)
                lstBreadcrumbPlacement.Items.Add(li)
            Next

        End Sub

        Private Sub BindWatermarkPosition()

            For Each value As Integer In System.Enum.GetValues(GetType(WatermarkPosition))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(WatermarkPosition), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(WatermarkPosition), value), Me.LocalResourceFile)
                drpWatermarkPosition.Items.Add(li)
            Next

        End Sub

        Private Sub BindContactFields()

            Dim objContactFieldController As New ContactFieldController()

            Dim objContactFields As List(Of ContactFieldInfo) = objContactFieldController.List(ModuleId)

            drpContactLogField.DataSource = objContactFields
            drpContactLogField.DataBind()
            drpContactLogField.Items.Insert(0, New ListItem(Localization.GetString("NotSpecified", Me.LocalResourceFile), -1))

            drpContactField.DataSource = CustomFields
            drpContactField.DataBind()
            drpContactField.Items.Insert(0, New ListItem(Localization.GetString("NotSpecified", Me.LocalResourceFile), -1))

            drpContactCustomField.DataSource = Me.CustomFields
            drpContactCustomField.DataBind()
            drpContactCustomField.Items.Insert(0, New ListItem(Localization.GetString("NotSpecified", Me.LocalResourceFile), -1))

        End Sub

        Private Sub BindCurrency()

            For Each value As Integer In System.Enum.GetValues(GetType(CurrencyType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CurrencyType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CurrencyType), value), Me.LocalResourceFile)

                If (value = CurrencyType.USD) Then
                    li.Selected = True
                End If
                drpCurrency.Items.Add(li)
                chkCurrencyAvailableList.Items.Add(New ListItem(li.Text, li.Value))
            Next

        End Sub

        Private Sub BindEuroType()

            For Each value As Integer In System.Enum.GetValues(GetType(EuroType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(EuroType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(EuroType), value), Me.LocalResourceFile)

                If (value = EuroType.French) Then
                    li.Selected = True
                End If
                lstEuroFormat.Items.Add(li)
            Next

        End Sub

        Private Sub BindDestination()

            For Each value As Integer In System.Enum.GetValues(GetType(DestinationType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(DestinationType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(DestinationType), value), Me.LocalResourceFile)

                If (value = DestinationType.PropertyOwner) Then
                    li.Selected = True
                End If
                lstContactDestination.Items.Add(li)
            Next

        End Sub

        Private Sub BindDistance()

            For Each value As Integer In System.Enum.GetValues(GetType(DistanceType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(DistanceType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(DistanceType), value), Me.LocalResourceFile)

                If (value = DistanceType.Miles) Then
                    li.Selected = True
                End If
                lstDistanceType.Items.Add(li)
            Next

        End Sub

        Private Sub BindExpiration()

            drpDefaultExpiration.Items.Add(New ListItem(Localization.GetString("Day", Me.LocalResourceFile), "D"))
            drpDefaultExpiration.Items.Add(New ListItem(Localization.GetString("Month", Me.LocalResourceFile), "M"))
            drpDefaultExpiration.Items.Add(New ListItem(Localization.GetString("Year", Me.LocalResourceFile), "Y"))

            For Each objCustomField As CustomFieldInfo In CustomFields
                If (objCustomField.FieldType = CustomFieldType.OneLineTextBox) Then
                    If (objCustomField.ValidationType = CustomFieldValidationType.Date) Then
                        drpBindExpiry.Items.Add(New ListItem(objCustomField.Caption, objCustomField.CustomFieldID.ToString()))
                    End If
                End If
            Next
            drpBindExpiry.Items.Insert(0, New ListItem(Localization.GetString("NotSpecified", Me.LocalResourceFile), -1))
            
        End Sub

        Private Sub BindPages()

            drpRedirectPage.DataSource = TabController.GetPortalTabs(PortalId, TabId, True, False)
            drpRedirectPage.DataBind()

        End Sub

        Private Sub BindSortOrderGrid()

            grdLandingPageSortOrder.DataSource = Me.PropertySettings(True).LandingPageSections.Split(","c)
            grdLandingPageSortOrder.DataBind()

        End Sub

        Private Sub BindRedirect()

            For Each value As Integer In System.Enum.GetValues(GetType(RedirectType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(RedirectType), value)
                li.Text = Localization.GetString("Redirect" & System.Enum.GetName(GetType(RedirectType), value), Me.LocalResourceFile)
                If (value = RedirectType.PropertyManager) Then
                    li.Selected = True
                End If
                lstRedirectType.Items.Add(li)
            Next

        End Sub

        Private Sub BindSortBy()

            For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)

                If (objSortByType = SortByType.CustomField) Then
                    Dim objCustomFieldController As New CustomFieldController
                    Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleId, True)

                    For Each objCustomField As CustomFieldInfo In objCustomFields
                        If (objCustomField.IsSortable) Then
                            Dim li As New ListItem
                            li.Value = "cf" & objCustomField.CustomFieldID.ToString()
                            li.Text = objCustomField.Caption
                            drpSortBy.Items.Add(li)
                            drpFeaturedSortBy.Items.Add(New ListItem(li.Text, li.Value))
                            drpPropertyManagerSortBy.Items.Add(New ListItem(li.Text, li.Value))
                        End If
                    Next
                Else
                    If (objSortByType = SortByType.ReviewField) Then
                        Dim objReviewFieldController As New ReviewFieldController
                        Dim objReviewFields As List(Of ReviewFieldInfo) = objReviewFieldController.List(Me.ModuleId)

                        For Each objReviewField As ReviewFieldInfo In objReviewFields
                            If (objReviewField.FieldType = ReviewFieldType.Rating) Then
                                Dim li As New ListItem
                                li.Value = "rf" & objReviewField.ReviewFieldID.ToString()
                                li.Text = objReviewField.Caption
                                drpSortBy.Items.Add(li)
                                drpFeaturedSortBy.Items.Add(New ListItem(li.Text, li.Value))
                                drpPropertyManagerSortBy.Items.Add(New ListItem(li.Text, li.Value))
                            End If
                        Next
                    Else
                        Dim li As New ListItem
                        li.Value = System.Enum.GetName(GetType(SortByType), value)
                        li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                        drpSortBy.Items.Add(li)
                        drpFeaturedSortBy.Items.Add(New ListItem(li.Text, li.Value))
                        drpPropertyManagerSortBy.Items.Add(New ListItem(li.Text, li.Value))
                    End If
                End If
            Next

            For Each value As Integer In System.Enum.GetValues(GetType(PropertyTypeSortByType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(PropertyTypeSortByType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(PropertyTypeSortByType), value), Me.LocalResourceFile)
                drpTypesSortBy.Items.Add(li)
            Next


            For Each value As Integer In System.Enum.GetValues(GetType(SortByTypeSecondary))
                Dim objSortByType As SortByTypeSecondary = CType(System.Enum.Parse(GetType(SortByTypeSecondary), value.ToString()), SortByTypeSecondary)

                If (objSortByType = SortByType.CustomField) Then
                    Dim objCustomFieldController As New CustomFieldController
                    Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleId, True)

                    For Each objCustomField As CustomFieldInfo In objCustomFields
                        If (objCustomField.IsSortable) Then
                            Dim li As New ListItem
                            li.Value = "cf" & objCustomField.CustomFieldID.ToString()
                            li.Text = objCustomField.Caption
                            drpSortBySecondary.Items.Add(li)
                            drpSortByTertiary.Items.Add(New ListItem(li.Text, li.Value))
                        End If
                    Next
                Else
                    Dim li As New ListItem
                    li.Value = System.Enum.GetName(GetType(SortByType), value)
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile)
                    drpSortBySecondary.Items.Add(li)
                    drpSortByTertiary.Items.Add(New ListItem(li.Text, li.Value))
                End If
            Next


        End Sub

        Private Sub BindSortDirection()

            For Each value As Integer In System.Enum.GetValues(GetType(SortDirectionType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortDirectionType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortDirectionType), value), Me.LocalResourceFile)

                If (value = SortDirectionType.Descending) Then
                    li.Selected = True
                End If
                drpSortDirection.Items.Add(li)
                drpFeaturedSortdirection.Items.Add(New ListItem(li.Text, li.Value))
                drpPropertyManagerSortDirection.Items.Add(New ListItem(li.Text, li.Value))
                drpSortDirectionSecondary.Items.Add(New ListItem(li.Text, li.Value))
                drpSortDirectionTertiary.Items.Add(New ListItem(li.Text, li.Value))
            Next

        End Sub

        Private Sub BindRepeatDirection()

            For Each value As Integer In System.Enum.GetValues(GetType(RepeatDirection))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(RepeatDirection), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(RepeatDirection), value), Me.LocalResourceFile)

                If (value = RepeatDirection.Horizontal) Then
                    li.Selected = True
                End If
                drpTypesRepeatDirection.Items.Add(li)
            Next

        End Sub

        Private Sub BindUpload()

            For Each value As Integer In System.Enum.GetValues(GetType(UploadType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(UploadType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(UploadType), value), Me.LocalResourceFile)

                If (value = UploadType.Flash) Then
                    li.Selected = True
                End If
                lstUploadMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindUploadPlacement()

            For Each value As Integer In System.Enum.GetValues(GetType(UploadPlacementType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(UploadPlacementType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(UploadPlacementType), value), Me.LocalResourceFile)

                If (value = UploadPlacementType.SeparatePage) Then
                    li.Selected = True
                End If
                lstUploadPlacement.Items.Add(li)
            Next

        End Sub

        Private Sub BindTitleReplacement()

            For Each value As Integer In System.Enum.GetValues(GetType(TitleReplacementType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(TitleReplacementType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(TitleReplacementType), value), Me.LocalResourceFile)
                lstSEOTitleReplacement.Items.Add(li)
            Next

        End Sub

        Private Sub BindSettings()

            BindReplyTo()
            BindLandingPageType()
            BindLayoutType()
            BindBreadcrumbPlacement()
            BindWatermarkPosition()
            BindCurrency()
            BindDestination()
            BindDistance()
            BindEuroType()
            BindPages()
            BindRedirect()
            BindSortFields()
            BindSortOrderGrid()
            BindSortBy()
            BindSortDirection()
            BindRepeatDirection()
            BindExpiration()
            BindUpload()
            BindUploadPlacement()
            BindTitleReplacement()
            BindContactFields()

            txtMainLabel.Text = Me.PropertySettings.MainLabel
            txtPropertyLabel.Text = Me.PropertySettings.PropertyLabel
            txtPropertyPluralLabel.Text = Me.PropertySettings.PropertyPluralLabel
            txtPropertyTypeLabel.Text = Me.PropertySettings.PropertyTypeLabel
            txtPropertyTypePluralLabel.Text = Me.PropertySettings.PropertyTypePluralLabel
            txtLocationLabel.Text = Me.PropertySettings.LocationLabel
            txtAgentLabel.Text = Me.PropertySettings.AgentLabel
            txtAgentPluralLabel.Text = Me.PropertySettings.AgentPluralLabel
            txtBrokerLabel.Text = Me.PropertySettings.BrokerLabel
            txtBrokerPluralLabel.Text = Me.PropertySettings.BrokerPluralLabel
            txtShortListLabel.Text = Me.PropertySettings.ShortListLabel

            If (lstLandingPageType.Items.FindByValue(PropertySettings.LandingPageMode.ToString()) IsNot Nothing) Then
                lstLandingPageType.SelectedValue = PropertySettings.LandingPageMode.ToString()
            End If

            chkFeaturedEnabled.Checked = Me.PropertySettings.FeaturedEnabled
            If Not (lstLayoutTypeFeatured.Items.FindByValue(Convert.ToInt32(Me.PropertySettings.FeaturedLayoutType).ToString()) Is Nothing) Then
                lstLayoutTypeFeatured.SelectedValue = Convert.ToInt32(Me.PropertySettings.FeaturedLayoutType).ToString()
            End If
            txtFeaturedItemsPerRow.Text = Me.PropertySettings.FeaturedItemsPerRow.ToString()
            txtFeaturedMaxNumber.Text = Me.PropertySettings.FeaturedMaxNumber.ToString()

            If (Me.PropertySettings.FeaturedSortBy = SortByType.CustomField) Then
                If Not (drpFeaturedSortBy.Items.FindByValue("cf" & Me.PropertySettings.FeaturedSortByCustomField) Is Nothing) Then
                    drpFeaturedSortBy.SelectedValue = "cf" & Me.PropertySettings.FeaturedSortByCustomField
                End If
            Else
                If (Me.PropertySettings.FeaturedSortBy = SortByType.ReviewField) Then
                    If Not (drpFeaturedSortBy.Items.FindByValue("rf" & Me.PropertySettings.FeaturedSortByCustomField) Is Nothing) Then
                        drpFeaturedSortBy.SelectedValue = "rf" & Me.PropertySettings.FeaturedSortByCustomField
                    End If
                Else
                    If Not (drpFeaturedSortBy.Items.FindByValue(Me.PropertySettings.FeaturedSortBy.ToString()) Is Nothing) Then
                        drpFeaturedSortBy.SelectedValue = Me.PropertySettings.FeaturedSortBy.ToString()
                    End If
                End If
            End If


            If Not (drpFeaturedSortdirection.Items.FindByValue(Me.PropertySettings.FeaturedSortDirection.ToString()) Is Nothing) Then
                drpFeaturedSortdirection.SelectedValue = Me.PropertySettings.FeaturedSortDirection.ToString()
            End If

            chkSearchEnabled.Checked = Me.PropertySettings.SearchEnabled
            chkSearchHideHelpIcon.Checked = Me.PropertySettings.SearchHideHelpIcon
            chkSearchHideTypesCount.Checked = Me.PropertySettings.SearchHideTypesCount
            chkSearchHideZeroTypes.Checked = Me.PropertySettings.SearchHideZeroTypes
            chkSearchWildard.Checked = Me.PropertySettings.SearchWildcard
            chkSearchTypes.Checked = Me.PropertySettings.SearchTypes
            chkSearchLocation.Checked = Me.PropertySettings.SearchLocation
            chkSearchAgents.Checked = Me.PropertySettings.SearchAgents
            chkSearchBrokers.Checked = Me.PropertySettings.SearchBrokers
            txtSearchWidth.Text = Me.PropertySettings.SearchWidth.ToString()
            txtSearchStyle.Text = Me.PropertySettings.SearchStyle


            If Not (lstLayoutTypeTypes.Items.FindByValue(Convert.ToInt32(Me.PropertySettings.TypesLayoutType).ToString()) Is Nothing) Then
                lstLayoutTypeTypes.SelectedValue = Convert.ToInt32(Me.PropertySettings.TypesLayoutType).ToString()
            End If
            chkTypesEnabled.Checked = Me.PropertySettings.TypesEnabled
            chkTypesHideZero.Checked = Me.PropertySettings.TypesHideZero
            txtTypesItemsPerRow.Text = Me.PropertySettings.TypesItemsPerRow.ToString()

            If Not (drpTypesRepeatdirection.Items.FindByValue(Me.PropertySettings.TypesRepeatDirection.ToString()) Is Nothing) Then
                drpTypesRepeatdirection.SelectedValue = Me.PropertySettings.TypesRepeatDirection.ToString()
            End If

            If Not (drpTypesSortBy.Items.FindByValue(Me.PropertySettings.TypesSortBy.ToString()) Is Nothing) Then
                drpTypesSortBy.SelectedValue = Me.PropertySettings.TypesSortBy.ToString()
            End If

            txtListingItemsPerRow.Text = Me.PropertySettings.ListingItemsPerRow.ToString()
            txtListingItemsPerPage.Text = Me.PropertySettings.ListingItemsPerPage.ToString()

            If Not (lstLayoutType.Items.FindByValue(Convert.ToInt32(Me.PropertySettings.ListingLayoutType).ToString()) Is Nothing) Then
                lstLayoutType.SelectedValue = Convert.ToInt32(Me.PropertySettings.ListingLayoutType).ToString()
            End If

            If (Me.PropertySettings.ListingSortBy = SortByType.CustomField) Then
                If Not (drpSortBy.Items.FindByValue("cf" & Me.PropertySettings.ListingSortByCustomField) Is Nothing) Then
                    drpSortBy.SelectedValue = "cf" & Me.PropertySettings.ListingSortByCustomField
                End If
            Else
                If (Me.PropertySettings.ListingSortBy = SortByType.CustomField) Then
                    If Not (drpSortBy.Items.FindByValue("rf" & Me.PropertySettings.ListingSortByCustomField) Is Nothing) Then
                        drpSortBy.SelectedValue = "rf" & Me.PropertySettings.ListingSortByCustomField
                    End If
                Else
                    If Not (drpSortBy.Items.FindByValue(Me.PropertySettings.ListingSortBy.ToString()) Is Nothing) Then
                        drpSortBy.SelectedValue = Me.PropertySettings.ListingSortBy.ToString()
                    End If
                End If
            End If

            If Not (drpSortdirection.Items.FindByValue(Me.PropertySettings.ListingSortDirection.ToString()) Is Nothing) Then
                drpSortdirection.SelectedValue = Me.PropertySettings.ListingSortDirection.ToString()
            End If

            If (Me.PropertySettings.ListingSortBy2 = SortByType.CustomField) Then
                If Not (drpSortBySecondary.Items.FindByValue("cf" & Me.PropertySettings.ListingSortByCustomField2) Is Nothing) Then
                    drpSortBySecondary.SelectedValue = "cf" & Me.PropertySettings.ListingSortByCustomField2
                End If
            Else
                If (Me.PropertySettings.ListingSortBy2 = SortByType.CustomField) Then
                    If Not (drpSortBySecondary.Items.FindByValue("rf" & Me.PropertySettings.ListingSortByCustomField2) Is Nothing) Then
                        drpSortBySecondary.SelectedValue = "rf" & Me.PropertySettings.ListingSortByCustomField2
                    End If
                Else
                    If Not (drpSortBySecondary.Items.FindByValue(Me.PropertySettings.ListingSortBy2.ToString()) Is Nothing) Then
                        drpSortBySecondary.SelectedValue = Me.PropertySettings.ListingSortBy2.ToString()
                    End If
                End If
            End If

            If Not (drpSortDirectionTertiary.Items.FindByValue(Me.PropertySettings.ListingSortDirection2.ToString()) Is Nothing) Then
                drpSortDirectionSecondary.SelectedValue = Me.PropertySettings.ListingSortDirection2.ToString()
            End If

            If (Me.PropertySettings.ListingSortBy3 = SortByType.CustomField) Then
                If Not (drpSortByTertiary.Items.FindByValue("cf" & Me.PropertySettings.ListingSortByCustomField3) Is Nothing) Then
                    drpSortByTertiary.SelectedValue = "cf" & Me.PropertySettings.ListingSortByCustomField3
                End If
            Else
                If (Me.PropertySettings.ListingSortBy3 = SortByType.CustomField) Then
                    If Not (drpSortByTertiary.Items.FindByValue("rf" & Me.PropertySettings.ListingSortByCustomField3) Is Nothing) Then
                        drpSortByTertiary.SelectedValue = "rf" & Me.PropertySettings.ListingSortByCustomField3
                    End If
                Else
                    If Not (drpSortByTertiary.Items.FindByValue(Me.PropertySettings.ListingSortBy3.ToString()) Is Nothing) Then
                        drpSortByTertiary.SelectedValue = Me.PropertySettings.ListingSortBy3.ToString()
                    End If
                End If
            End If

            If Not (drpSortDirectionTertiary.Items.FindByValue(Me.PropertySettings.ListingSortDirection3.ToString()) Is Nothing) Then
                drpSortDirectionTertiary.SelectedValue = Me.PropertySettings.ListingSortDirection3.ToString()
            End If

            chkBubbleFeatured.Checked = Me.PropertySettings.ListingBubbleFeatured
            chkSearchSubTypes.Checked = Me.PropertySettings.ListingSearchSubTypes
            chkPassSearchValues.Checked = Me.PropertySettings.ListingPassSearchValues
            chkUserSortable.Checked = Me.PropertySettings.ListingUserSortable

            For Each li As ListItem In lstSortFields.Items
                For Each item As String In PropertySettings.ListingSortFields.Split(","c)
                    If (li.Value = item) Then
                        li.Selected = True
                    End If
                Next
            Next

            chkHideAuthorDetails.Checked = Me.PropertySettings.PropertyManagerHideAuthorDetails
            chkHidePublishingDetails.Checked = Me.PropertySettings.PropertyManagerHidePublishingDetails

            If Not (drpPropertyManagerRecordsPerPage.Items.FindByValue(Me.PropertySettings.PropertyManagerItemsPerPage.ToString()) Is Nothing) Then
                drpPropertyManagerRecordsPerPage.SelectedValue = Me.PropertySettings.PropertyManagerItemsPerPage.ToString()
            End If

            If (Me.PropertySettings.PropertyManagerSortBy = SortByType.CustomField) Then
                If Not (drpPropertyManagerSortBy.Items.FindByValue("cf" & Me.PropertySettings.PropertyManagerSortByCustomField) Is Nothing) Then
                    drpPropertyManagerSortBy.SelectedValue = "cf" & Me.PropertySettings.PropertyManagerSortByCustomField
                End If
            Else
                If (Me.PropertySettings.PropertyManagerSortBy = SortByType.CustomField) Then
                    If Not (drpPropertyManagerSortBy.Items.FindByValue("rf" & Me.PropertySettings.PropertyManagerSortByCustomField) Is Nothing) Then
                        drpPropertyManagerSortBy.SelectedValue = "rf" & Me.PropertySettings.PropertyManagerSortByCustomField
                    End If
                Else
                    If Not (drpPropertyManagerSortBy.Items.FindByValue(Me.PropertySettings.PropertyManagerSortBy.ToString()) Is Nothing) Then
                        drpPropertyManagerSortBy.SelectedValue = Me.PropertySettings.PropertyManagerSortBy.ToString()
                    End If
                End If
            End If

            If Not (drpPropertyManagerSortDirection.Items.FindByValue(Me.PropertySettings.PropertyManagerSortDirection.ToString()) Is Nothing) Then
                drpPropertyManagerSortDirection.SelectedValue = Me.PropertySettings.PropertyManagerSortDirection.ToString()
            End If

            chkImagesEnabled.Checked = Me.PropertySettings.ImagesEnabled
            chkHighQuality.Checked = Me.PropertySettings.HighQuality
            chkIncludejQuery.Checked = Me.PropertySettings.IncludejQuery
            txtSmallWidth.Text = Me.PropertySettings.SmallWidth.ToString()
            txtSmallHeight.Text = Me.PropertySettings.SmallHeight.ToString()
            txtMediumWidth.Text = Me.PropertySettings.MediumWidth.ToString()
            txtMediumHeight.Text = Me.PropertySettings.MediumHeight.ToString()
            txtLargeWidth.Text = Me.PropertySettings.LargeWidth.ToString()
            txtLargeHeight.Text = Me.PropertySettings.LargeHeight.ToString()
            txtImagesItemsPerRow.Text = Me.PropertySettings.ImagesItemsPerRow.ToString()

            chkUseWatermark.Checked = Me.PropertySettings.WatermarkEnabled
            txtWatermarkText.Text = Me.PropertySettings.WatermarkText
            ctlWatermarkImage.Url = Me.PropertySettings.WatermarkImage
            If Not (drpWatermarkPosition.Items.FindByValue(PropertySettings.WatermarkPosition.ToString()) Is Nothing) Then
                drpWatermarkPosition.SelectedValue = PropertySettings.WatermarkPosition.ToString()
            End If
            txtImageCategories.Text = Me.PropertySettings.ImageCategories

            If (lstBreadcrumbPlacement.Items.FindByValue(PropertySettings.BreadcrumbPlacement.ToString()) IsNot Nothing) Then
                lstBreadcrumbPlacement.SelectedValue = PropertySettings.BreadcrumbPlacement.ToString()
            End If
            If (Me.PropertySettings.CustomFieldExpiration <> Null.NullInteger) Then
                If (drpBindExpiry.Items.FindByValue(Me.PropertySettings.CustomFieldExpiration.ToString()) IsNot Nothing) Then
                    drpBindExpiry.SelectedValue = Me.PropertySettings.CustomFieldExpiration.ToString()
                End If
            End If
            If (Me.PropertySettings.DefaultExpiration <> Null.NullInteger) Then
                txtdefaultExpiration.Text = Me.PropertySettings.DefaultExpiration.ToString()
            End If
            If Not (drpDefaultExpiration.Items.FindByValue(Me.PropertySettings.DefaultExpirationPeriod) Is Nothing) Then
                drpDefaultExpiration.SelectedValue = Me.PropertySettings.DefaultExpirationPeriod
            End If
            txtFieldWidth.Text = Me.PropertySettings.FieldWidth.ToString()
            txtCheckBoxListItemsPerRow.Text = Me.PropertySettings.CheckBoxItemsPerRow.ToString()
            txtRadioButtonItemsPerRow.Text = Me.PropertySettings.RadioButtonItemsPerRow.ToString()

            txtButtonClass.Text = Me.PropertySettings.ButtonClass
            chkCachePropertyValues.Checked = Me.PropertySettings.CachePropertyValues
            chkHideTypes.Checked = Me.PropertySettings.HideTypes
            chkTypeParams.Checked = Me.PropertySettings.TypeParams
            chkLockDownPropertyType.Checked = Me.PropertySettings.LockDownPropertyType
            chkLockDownPropertyDates.Checked = Me.PropertySettings.LockDownPropertyDates
            chkLockDownFeatured.Checked = Me.PropertySettings.LockDownFeatured
            If Not (drpRedirectPage.Items.FindByValue(Me.PropertySettings.RedirectPage.ToString()) Is Nothing) Then
                drpRedirectPage.SelectedValue = Me.PropertySettings.RedirectPage.ToString()
            End If
            If Not (lstRedirectType.Items.FindByValue(Me.PropertySettings.RedirectType.ToString()) Is Nothing) Then
                lstRedirectType.SelectedValue = Me.PropertySettings.RedirectType.ToString()
            End If
            trRedirectPage.Visible = (PropertySettings.RedirectType = RedirectType.Page)
            If Not (lstUploadMode.Items.FindByValue(Me.PropertySettings.UploadMode.ToString()) Is Nothing) Then
                lstUploadMode.SelectedValue = Me.PropertySettings.UploadMode.ToString()
            End If
            If Not (lstUploadPlacement.Items.FindByValue(Me.PropertySettings.UploadPlacement.ToString()) Is Nothing) Then
                lstUploadPlacement.SelectedValue = Me.PropertySettings.UploadPlacement.ToString()
            End If
            chkProtectXSS.Checked = Me.PropertySettings.ProtectXSS
            chkAgentDropdown.Checked = Me.PropertySettings.AgentDropdownDefault
            txtMaxUploadLimit.Text = Me.PropertySettings.MaxUploadLimit

            If Not (lstContactdestination.Items.FindByValue(Me.PropertySettings.ContactDestination.ToString()) Is Nothing) Then
                lstContactdestination.SelectedValue = Me.PropertySettings.ContactDestination.ToString()
            End If
            trContactCustomEmail.Visible = (Me.PropertySettings.ContactDestination = DestinationType.CustomEmail)
            trContactField.Visible = (Me.PropertySettings.ContactDestination = DestinationType.CustomField)
            txtContactCustomEmail.Text = Me.PropertySettings.ContactCustomEmail
            If Not (lstContactReply.Items.FindByValue(Me.PropertySettings.ContactReplyTo.ToString()) Is Nothing) Then
                lstContactReply.SelectedValue = Me.PropertySettings.ContactReplyTo.ToString()
            End If
            txtContactBCC.Text = Me.PropertySettings.ContactBCC
            txtContactMessageLines.Text = Me.PropertySettings.ContactMessageLines.ToString()
            txtContactWidth.Text = Me.PropertySettings.ContactWidth.ToString()
            chkHideEmail.Checked = Me.PropertySettings.ContactHideEmail
            chkHideName.Checked = Me.PropertySettings.ContactHideName
            chkHidePhone.Checked = Me.PropertySettings.ContactHidePhone
            chkRequireEmail.Checked = Me.PropertySettings.ContactRequireEmail
            chkRequireName.Checked = Me.PropertySettings.ContactRequireName
            chkRequirePhone.Checked = Me.PropertySettings.ContactRequirePhone
            chkContactUseCaptcha.Checked = Me.PropertySettings.ContactUseCaptcha
            If (drpContactLogField.Items.FindByValue(Me.PropertySettings.ContactLogField.ToString()) IsNot Nothing) Then
                drpContactLogField.SelectedValue = Me.PropertySettings.ContactLogField.ToString()
            End If
            If (drpContactField.Items.FindByValue(Me.PropertySettings.ContactField.ToString()) IsNot Nothing) Then
                drpContactField.SelectedValue = Me.PropertySettings.ContactField.ToString()
            End If
            If (drpContactCustomField.Items.FindByValue(Me.PropertySettings.ContactCustomField.ToString()) IsNot Nothing) Then
                drpContactCustomField.SelectedValue = Me.PropertySettings.ContactCustomField.ToString()
            End If

            If (drpCurrency.Items.FindByValue(Me.PropertySettings.Currency.ToString()) IsNot Nothing) Then
                drpCurrency.SelectedValue = Me.PropertySettings.Currency.ToString()
            End If
            If (lstEuroFormat.Items.FindByValue(Me.PropertySettings.EuroType.ToString()) IsNot Nothing) Then
                lstEuroFormat.SelectedValue = Me.PropertySettings.EuroType.ToString()
            End If
            chkCurrencyShowAll.Checked = Me.PropertySettings.CurrencyShowAll
            chkCurrencyAvailableList.Visible = Not chkCurrencyShowAll.Checked
            txtCurrencyDecimalPlaces.Text = Me.PropertySettings.CurrencyDecimalPlaces.ToString()

            For Each currency As String In PropertySettings.CurrencyAvailable.Split(","c)
                For Each li As ListItem In chkCurrencyAvailableList.Items
                    If (li.Value = currency) Then
                        li.Selected = True
                    End If
                Next
            Next

            chkEnableMaps.Checked = Me.PropertySettings.MapEnable
            txtMapKey.Text = Me.PropertySettings.MapKey
            txtMapHeight.Text = Me.PropertySettings.MapHeight.ToString()
            txtMapWidth.Text = Me.PropertySettings.MapWidth.ToString()
            txtMapZoom.Text = Me.PropertySettings.MapZoom.ToString()
            txtDistanceExpression.Text = Me.PropertySettings.DistanceExpression
            If Not (lstDistanceType.Items.FindByValue(Me.PropertySettings.DistanceType.ToString()) Is Nothing) Then
                lstDistanceType.SelectedValue = Me.PropertySettings.DistanceType.ToString()
            End If

            chkNotificationNotifyApprovers.Checked = Me.PropertySettings.NotificationNotifyApprovers
            txtNotificationEmail.Text = Me.PropertySettings.NotificationEmail
            chkNotifyBroker.Checked = Me.PropertySettings.NotificationNotifyBroker
            chkNotifyOwner.Checked = Me.PropertySettings.NotificationNotifyOwner

            txtCommentWidth.Text = Me.PropertySettings.CommentWidth.ToString()
            chkUseCaptcha.Checked = Me.PropertySettings.CommentUseCaptcha
            chkCommentNotifyOwner.Checked = Me.PropertySettings.CommentNotifyOwner
            txtCommentEmail.Text = Me.PropertySettings.CommentNotifyEmail

            chkRssEnable.Checked = Me.PropertySettings.RssEnable
            txtRssMaxRecords.Text = Me.PropertySettings.RssMaxRecords.ToString()
            txtRssTitleLatest.Text = Me.PropertySettings.RssTitleLatest(False)
            txtRssTitleType.Text = Me.PropertySettings.RssTitleType("", False)
            txtRssTitleSearchResult.Text = Me.PropertySettings.RssTitleSearchResult(False)

            txtFriendBCC.Text = Me.PropertySettings.FriendBCC
            txtFriendWidth.Text = Me.PropertySettings.FriendWidth.ToString()

            chkCoreSearchEnable.Checked = Me.PropertySettings.CoreSearchEnabled
            txtCoreSearchTitle.Text = Me.PropertySettings.CoreSearchTitle
            txtCoreSearchDescription.Text = Me.PropertySettings.CoreSearchDescription

            txtReviewWidth.Text = Me.PropertySettings.ReviewWidth.ToString()
            chkReviewModeration.Checked = Me.PropertySettings.ReviewModeration.ToString()
            txtReviewEmail.Text = Me.PropertySettings.ReviewEmail.ToString()
            chkReviewAnonymous.Checked = Me.PropertySettings.ReviewAnonymous.ToString()

            chkSEORedirect.Checked = Me.PropertySettings.SEORedirect
            txtSEOAgentType.Text = Me.PropertySettings.SEOAgentType
            txtSEOPropertyID.Text = Me.PropertySettings.SEOPropertyID
            txtSEOPropertyTypeID.Text = Me.PropertySettings.SEOPropertyTypeID
            txtSEOViewPropertyTitle.Text = Me.PropertySettings.SEOViewPropertyTitle
            txtSEOViewTypeTitle.Text = Me.PropertySettings.SEOViewTypeTitle
            If (lstSEOTitleReplacement.Items.FindByValue(Me.PropertySettings.SEOTitleReplacement.ToString()) IsNot Nothing) Then
                lstSEOTitleReplacement.SelectedValue = Me.PropertySettings.SEOTitleReplacement.ToString()
            End If
            chkSEOCanonicalLink.Checked = Me.PropertySettings.SEOCanonicalLink

            chkTemplateIncludeStylesheet.Checked = Me.PropertySettings.TemplateIncludeStylesheet

            chkXmlEnable.Checked = Me.PropertySettings.XmlEnable
            txtXmlMaxRecords.Text = Me.PropertySettings.XmlMaxRecords.ToString()

            Dim xmlLink As String = NavigateURL(Me.TabId, "", "agentType=xml")
            If (xmlLink.ToLower().StartsWith("http")) Then
                lblXmlUrl.Text = xmlLink
            Else
                lblXmlUrl.Text = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & xmlLink)
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objLayoutSettings As New CrumbInfo
            objLayoutSettings.Caption = Localization.GetString("EditLayoutSettings", LocalResourceFile)
            objLayoutSettings.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditLayoutSettings")
            crumbs.Add(objLayoutSettings)

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

        Private Sub BindSortFields()

            For Each value As Integer In System.Enum.GetValues(GetType(SortByType))
                Dim objSortByType As SortByType = CType(System.Enum.Parse(GetType(SortByType), value.ToString()), SortByType)
                lstSortFields.Items.Add(New ListItem(Localization.GetString(System.Enum.GetName(GetType(SortByType), value), Me.LocalResourceFile), System.Enum.GetName(GetType(SortByType), value)))
            Next

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
                    BindSettings()
                End If

                cmdUpdate.CssClass = PropertySettings.ButtonClass
                cmdCancel.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try
                If (Page.IsValid) Then

                    Dim objModules As New DotNetNuke.Entities.Modules.ModuleController

                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_MAIN_LABEL_SETTING, txtMainLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_LABEL_SETTING, txtPropertyLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_PLURAL_LABEL_SETTING, txtPropertyPluralLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_TYPE_LABEL_SETTING, txtPropertyTypeLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_TYPE_PLURAL_LABEL_SETTING, txtPropertyTypePluralLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.LOCATION_LABEL_SETTING, txtLocationLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.AGENT_LABEL_SETTING, txtAgentLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.AGENT_PLURAL_LABEL_SETTING, txtAgentPluralLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.BROKER_LABEL_SETTING, txtBrokerLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.BROKER_PLURAL_LABEL_SETTING, txtBrokerPluralLabel.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SHORTLIST_LABEL_SETTING, txtShortListLabel.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.LANDING_PAGE_MODE_SETTING, lstLandingPageType.SelectedValue)

                    objModules.UpdateModuleSetting(ModuleId, Constants.FEATURED_ENABLED_SETTING, chkFeaturedEnabled.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.FEATURED_LAYOUT_TYPE_SETTING, lstLayoutTypeFeatured.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.FEATURED_MAX_NUMBER_SETTING, txtFeaturedMaxNumber.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.FEATURED_ITEMS_PER_ROW_SETTING, txtFeaturedItemsPerRow.Text)

                    Dim objFeaturedSortByType As SortByType = SortByType.CustomField
                    Dim featuredSortByID As Integer = Null.NullInteger
                    If (drpFeaturedSortBy.SelectedValue.StartsWith("cf")) Then
                        featuredSortByID = Convert.ToInt32(drpFeaturedSortBy.SelectedValue.Replace("cf", ""))
                    Else
                        If (drpFeaturedSortBy.SelectedValue.StartsWith("rf")) Then
                            objFeaturedSortByType = SortByType.ReviewField
                            featuredSortByID = Convert.ToInt32(drpFeaturedSortBy.SelectedValue.Replace("rf", ""))
                        Else
                            objFeaturedSortByType = CType(System.Enum.Parse(GetType(SortByType), drpFeaturedSortBy.SelectedValue.ToString()), SortByType)
                        End If
                    End If

                    objModules.UpdateModuleSetting(ModuleId, Constants.FEATURED_SORT_BY_SETTING, objFeaturedSortByType.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.FEATURED_SORT_BY_CUSTOM_FIELD_SETTING, featuredSortByID.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.FEATURED_SORT_DIRECTION_SETTING, drpFeaturedSortdirection.SelectedValue)

                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_ENABLED_SETTING, chkSearchEnabled.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_HIDE_HELP_ICON_SETTING, chkSearchHideHelpIcon.Checked)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_HIDE_TYPES_COUNT_SETTING, chkSearchHideTypesCount.Checked)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_HIDE_ZERO_TYPES_SETTING, chkSearchHideZeroTypes.Checked)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_WILDCARD_SETTING, chkSearchWildard.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_TYPES_SETTING, chkSearchTypes.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_LOCATION_SETTING, chkSearchLocation.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_AGENTS_SETTING, chkSearchAgents.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_BROKERS_SETTING, chkSearchBrokers.Checked.ToString())
                    If (Convert.ToInt32(txtSearchWidth.Text) > 0) Then
                        objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_AREA_WIDTH_SETTING, txtSearchWidth.Text.ToString())
                    End If
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEARCH_STYLE_SETTING, txtSearchStyle.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.TYPES_ENABLED_SETTING, chkTypesEnabled.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.TYPES_LAYOUT_TYPE_SETTING, lstLayoutTypeTypes.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.TYPES_HIDE_ZERO_SETTING, chkTypesHideZero.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.TYPES_ITEMS_PER_ROW_SETTING, txtTypesItemsPerRow.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.TYPES_REPEAT_DIRECTION_SETTING, drpTypesRepeatdirection.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.TYPES_SORT_BY_SETTING, drpTypesSortBy.SelectedValue)

                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_LAYOUT_TYPE_SETTING, lstLayoutType.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_ITEMS_PER_ROW_SETTING, txtListingItemsPerRow.Text)
                    If (Convert.ToInt32(txtListingItemsPerPage.Text) > 0) Then
                        objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_ITEMS_PER_PAGE_SETTING, txtListingItemsPerPage.Text)
                    End If
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_BUBBLE_FEATURED_SETTING, chkBubbleFeatured.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SEARCH_SUB_TYPES_SETTING, chkSearchSubTypes.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_PASS_SEARCH_VALUES_SETTING, chkPassSearchValues.Checked.ToString())

                    Dim objSortByType As SortByType = SortByType.CustomField
                    Dim sortByID As Integer = Null.NullInteger
                    If (drpSortBy.SelectedValue.StartsWith("cf")) Then
                        sortByID = Convert.ToInt32(drpSortBy.SelectedValue.Replace("cf", ""))
                    Else
                        If (drpSortBy.SelectedValue.StartsWith("rf")) Then
                            objSortByType = SortByType.ReviewField
                            sortByID = Convert.ToInt32(drpSortBy.SelectedValue.Replace("rf", ""))
                        Else
                            objSortByType = CType(System.Enum.Parse(GetType(SortByType), drpSortBy.SelectedValue.ToString()), SortByType)
                        End If
                    End If

                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_BY_SETTING, objSortByType.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_BY_CUSTOM_FIELD_SETTING, sortByID.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_DIRECTION_SETTING, drpSortdirection.SelectedValue)

                    Dim objSortByTypeSecondary As SortByType = SortByType.CustomField
                    Dim sortByIDSecondary As Integer = Null.NullInteger
                    If (drpSortBySecondary.SelectedValue.StartsWith("cf")) Then
                        sortByIDSecondary = Convert.ToInt32(drpSortBySecondary.SelectedValue.Replace("cf", ""))
                    Else
                        If (drpSortBySecondary.SelectedValue.StartsWith("rf")) Then
                            objSortByTypeSecondary = SortByType.ReviewField
                            sortByIDSecondary = Convert.ToInt32(drpSortBySecondary.SelectedValue.Replace("rf", ""))
                        Else
                            objSortByTypeSecondary = CType(System.Enum.Parse(GetType(SortByType), drpSortBySecondary.SelectedValue.ToString()), SortByType)
                        End If
                    End If
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_BY_2_SETTING, objSortByTypeSecondary.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_BY_2_CUSTOM_FIELD_SETTING, sortByIDSecondary.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_DIRECTION_2_SETTING, drpSortDirectionSecondary.SelectedValue)

                    Dim objSortByTypeTertiary As SortByType = SortByType.CustomField
                    Dim sortByIDTertiary As Integer = Null.NullInteger
                    If (drpSortByTertiary.SelectedValue.StartsWith("cf")) Then
                        sortByIDTertiary = Convert.ToInt32(drpSortByTertiary.SelectedValue.Replace("cf", ""))
                    Else
                        If (drpSortByTertiary.SelectedValue.StartsWith("rf")) Then
                            objSortByTypeTertiary = SortByType.ReviewField
                            sortByIDTertiary = Convert.ToInt32(drpSortByTertiary.SelectedValue.Replace("rf", ""))
                        Else
                            objSortByTypeTertiary = CType(System.Enum.Parse(GetType(SortByType), drpSortByTertiary.SelectedValue.ToString()), SortByType)
                        End If
                    End If
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_BY_3_SETTING, objSortByTypeTertiary.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_BY_3_CUSTOM_FIELD_SETTING, sortByIDTertiary.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_DIRECTION_3_SETTING, drpSortDirectionTertiary.SelectedValue)

                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_USER_SORTABLE_SETTING, chkUserSortable.Checked.ToString())

                    Dim sortFields As String = ""
                    For Each li As ListItem In lstSortFields.Items
                        If (li.Selected) Then
                            If (sortFields = "") Then
                                sortFields = li.Value
                            Else
                                sortFields = sortFields & "," & li.Value
                            End If
                        End If
                    Next
                    objModules.UpdateModuleSetting(ModuleId, Constants.LISTING_SORT_FIELDS_SETTING, sortFields)

                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_MANAGER_HIDE_AUTHOR_DETAILS_SETTING, chkHideAuthorDetails.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_MANAGER_HIDE_PUBLISHING_DETAILS_SETTING, chkHidePublishingDetails.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_MANAGER_ITEMS_PER_PAGE_SETTING, drpPropertyManagerRecordsPerPage.SelectedValue)
                    Dim objPropertyManagerSortByType As SortByType = SortByType.CustomField
                    Dim propertyManagerSortByID As Integer = Null.NullInteger
                    If (drpPropertyManagerSortBy.SelectedValue.StartsWith("cf")) Then
                        propertyManagerSortByID = Convert.ToInt32(drpPropertyManagerSortBy.SelectedValue.Replace("cf", ""))
                    Else
                        If (drpPropertyManagerSortBy.SelectedValue.StartsWith("rf")) Then
                            objPropertyManagerSortByType = SortByType.ReviewField
                            propertyManagerSortByID = Convert.ToInt32(drpPropertyManagerSortBy.SelectedValue.Replace("rf", ""))
                        Else
                            objPropertyManagerSortByType = CType(System.Enum.Parse(GetType(SortByType), drpPropertyManagerSortBy.SelectedValue.ToString()), SortByType)
                        End If
                    End If

                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_MANAGER_SORT_BY_SETTING, objPropertyManagerSortByType.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_MANAGER_SORT_BY_CUSTOM_FIELD_SETTING, propertyManagerSortByID.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROPERTY_MANAGER_SORT_DIRECTION_SETTING, drpPropertyManagerSortDirection.SelectedValue)

                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_ENABLED_SETTING, chkImagesEnabled.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_HIGH_QUALITY_SETTING, chkHighQuality.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_INCLUDE_JQUERY_SETTING, chkIncludejQuery.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.SMALL_WIDTH_SETTING, txtSmallWidth.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SMALL_HEIGHT_SETTING, txtSmallHeight.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.MEDIUM_WIDTH_SETTING, txtMediumWidth.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.MEDIUM_HEIGHT_SETTING, txtMediumHeight.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.LARGE_WIDTH_SETTING, txtLargeWidth.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.LARGE_HEIGHT_SETTING, txtLargeHeight.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_ITEMS_PER_ROW_SETTING, txtImagesItemsPerRow.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_WATERMARK_ENABLED_SETTING, chkUseWatermark.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_WATERMARK_TEXT_SETTING, txtWatermarkText.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_WATERMARK_IMAGE_SETTING, ctlWatermarkImage.Url)
                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_WATERMARK_IMAGE_POSITION_SETTING, drpWatermarkPosition.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.IMAGES_CATEGORIES_SETTING, txtImageCategories.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.BREADCRUMB_PLACEMENT_SETTING, lstBreadcrumbPlacement.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CUSTOM_FIELD_EXPIRATION_SETTING, drpBindExpiry.SelectedValue)
                    If (txtdefaultExpiration.Text.Length > 0) Then
                        If (Convert.ToInt32(txtdefaultExpiration.Text) > 0) Then
                            objModules.UpdateModuleSetting(ModuleId, Constants.DEFAULT_EXPIRATION_SETTING, txtdefaultExpiration.Text)
                        Else
                            objModules.UpdateModuleSetting(ModuleId, Constants.DEFAULT_EXPIRATION_SETTING, "-1")
                        End If
                    Else
                        objModules.UpdateModuleSetting(ModuleId, Constants.DEFAULT_EXPIRATION_SETTING, "-1")
                    End If
                    objModules.UpdateModuleSetting(ModuleId, Constants.DEFAULT_EXPIRATION_PERIOD_SETTING, drpDefaultExpiration.SelectedValue)
                    If (Convert.ToInt32(txtFieldWidth.Text) > 0) Then
                        objModules.UpdateModuleSetting(ModuleId, Constants.FIELD_WIDTH_SETTING, txtFieldWidth.Text)
                    End If
                    objModules.UpdateModuleSetting(ModuleId, Constants.RADIO_BUTTON_ITEMS_PER_ROW_SETTING, txtRadioButtonItemsPerRow.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CHECKBOX_ITEMS_PER_ROW_SETTING, txtCheckBoxListItemsPerRow.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.BUTTON_CLASS_SETTING, txtButtonClass.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CACHE_PROPERTY_VALUES_SETTING, chkCachePropertyValues.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.HIDE_TYPES_SETTING, chkHideTypes.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.TYPE_PARAMS_SETTING, chkTypeParams.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LOCKDOWN_PROPERTYTYPE_SETTING, chkLockDownPropertyType.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LOCKDOWN_PROPERTYDATES_SETTING, chkLockDownPropertyDates.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.LOCKDOWN_FEATURED_SETTING, chkLockDownFeatured.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.REDIRECT_TYPE_SETTING, lstRedirectType.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.REDIRECT_PAGE_SETTING, drpRedirectPage.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.UPLOAD_MODE_SETTING, lstUploadMode.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.UPLOAD_PLACEMENT_SETTING, lstUploadPlacement.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.PROTECT_XSS_SETTING, chkProtectXSS.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.AGENT_DROPDOWN_SETTING, chkAgentDropdown.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.MAX_UPLOAD_LIMIT_SETTING, txtMaxUploadLimit.Text.ToString())

                    objModules.UpdateModuleSetting(ModuleId, Constants.MAP_ENABLE_SETTING, chkEnableMaps.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.MAP_KEY_SETTING, txtMapKey.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.MAP_HEIGHT_SETTING, txtMapHeight.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.MAP_WIDTH_SETTING, txtMapWidth.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.MAP_ZOOM_SETTING, txtMapZoom.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.MAP_DISTANCE_EXPRESSION_SETTING, txtDistanceExpression.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.MAP_DISTANCE_TYPE_SETTING, lstDistanceType.SelectedValue)

                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_DESTINATION_SETTING, lstContactdestination.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_CUSTOM_EMAIL_SETTING, txtContactCustomEmail.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_FIELD_SETTING, drpContactField.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_REPLY_TO_SETTING, lstContactReply.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_BCC_SETTING, txtContactBCC.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_WIDTH_SETTING, txtContactWidth.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_LOG_FIELD_SETTING, drpContactLogField.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_MESSAGE_LINES_SETTING, txtContactMessageLines.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_HIDE_EMAIL_SETTING, chkHideEmail.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_HIDE_NAME_SETTING, chkHideName.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_HIDE_PHONE_SETTING, chkHidePhone.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_REQUIRE_EMAIL_SETTING, chkRequireEmail.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_REQUIRE_NAME_SETTING, chkRequireName.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_REQUIRE_PHONE_SETTING, chkRequirePhone.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_USE_CAPTCHA_SETTING, chkContactUseCaptcha.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CONTACT_CUSTOM_FIELD_ID_SETTING, drpContactCustomField.SelectedValue)

                    objModules.UpdateModuleSetting(ModuleId, Constants.CURRENCY_SETTING, drpCurrency.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.EURO_TYPE_SETTING, lstEuroFormat.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CURRENCY_SHOW_ALL_SETTING, chkCurrencyShowAll.Checked.ToString())

                    Dim currencyList As String = ""
                    For Each li As ListItem In chkCurrencyAvailableList.Items
                        If (li.Selected) Then
                            If (currencyList = "") Then
                                currencyList = li.Value
                            Else
                                currencyList = currencyList & "," & li.Value
                            End If
                        End If
                    Next
                    objModules.UpdateModuleSetting(ModuleId, Constants.CURRENCY_AVAILABLE_SETTING, currencyList)
                    If (Convert.ToInt32(txtCurrencyDecimalPlaces.Text) >= 0) Then
                        objModules.UpdateModuleSetting(ModuleId, Constants.CURRENCY_DECIMAL_PLACES_SETTING, txtCurrencyDecimalPlaces.Text)
                    End If

                    objModules.UpdateModuleSetting(ModuleId, Constants.NOTIFICATION_NOTIFY_APPROVERS_SETTING, chkNotificationNotifyApprovers.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.NOTIFICATION_EMAIL_SETTING, txtNotificationEmail.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.NOTIFICATION_NOTIFY_BROKER_SETTING, chkNotifyBroker.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.NOTIFICATION_NOTIFY_OWNER_SETTING, chkNotifyOwner.Checked.ToString())

                    objModules.UpdateModuleSetting(ModuleId, Constants.COMMENT_WIDTH_SETTING, txtCommentWidth.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.COMMENT_USE_CAPTCHA_SETTING, chkUseCaptcha.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.COMMENT_NOTIFY_OWNER_SETTING, chkCommentNotifyOwner.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.COMMENT_NOTIFY_EMAIL_SETTING, txtCommentEmail.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.RSS_ENABLE, chkRssEnable.Checked.ToString())
                    If (Convert.ToInt32(txtRssMaxRecords.Text) > 0) Then
                        objModules.UpdateModuleSetting(ModuleId, Constants.RSS_MAX_RECORDS, txtRssMaxRecords.Text)
                    End If

                    objModules.UpdateModuleSetting(ModuleId, Constants.RSS_TITLE_LATEST, txtRssTitleLatest.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.RSS_TITLE_TYPE, txtRssTitleType.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.RSS_TITLE_SEARCH_RESULT, txtRssTitleSearchResult.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.FRIEND_BCC_SETTING, txtFriendBCC.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.FRIEND_WIDTH_SETTING, txtFriendWidth.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.CORE_SEARCH_ENABLED_SETTING, chkCoreSearchEnable.Checked.ToString())
                    objModules.UpdateModuleSetting(ModuleId, Constants.CORE_SEARCH_TITLE_SETTING, txtCoreSearchTitle.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.CORE_SEARCH_DESCRIPTION_SETTING, txtCoreSearchDescription.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.REVIEW_WIDTH_SETTING, txtReviewWidth.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.REVIEW_MODERATION_SETTING, chkReviewModeration.Checked)
                    objModules.UpdateModuleSetting(ModuleId, Constants.REVIEW_ANONYMOUS_SETTING, chkReviewAnonymous.Checked)
                    objModules.UpdateModuleSetting(ModuleId, Constants.REVIEW_EMAIL_SETTING, txtReviewEmail.Text)

                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_REDIRECT_SETTING, chkSEORedirect.Checked)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_AGENT_TYPE_SETTING, txtSEOAgentType.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_PROPERTY_ID_SETTING, txtSEOPropertyID.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_PROPERTY_TYPE_ID_SETTING, txtSEOPropertyTypeID.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_VIEW_PROPERTY_TITLE_SETTING, txtSEOViewPropertyTitle.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_VIEW_TYPE_TITLE_SETTING, txtSEOViewTypeTitle.Text)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_TITLE_REPLACEMENT_SETTING, lstSEOTitleReplacement.SelectedValue)
                    objModules.UpdateModuleSetting(ModuleId, Constants.SEO_CANONICAL_LINK_SETTING, chkSEOCanonicalLink.Checked.ToString())

                    objModules.UpdateModuleSetting(ModuleId, Constants.TEMPLATE_INCLUDE_STYLESHEET_SETTING, chkTemplateIncludeStylesheet.Checked.ToString())

                    objModules.UpdateModuleSetting(ModuleId, Constants.XML_ENABLE, chkXmlEnable.Checked.ToString())
                    If (Convert.ToInt32(txtXmlMaxRecords.Text) > 0) Then
                        objModules.UpdateModuleSetting(ModuleId, Constants.XML_MAX_RECORDS, txtXmlMaxRecords.Text)
                    End If

                    Dim objTemplateController As New LayoutController(Me.PortalSettings, Me.PropertySettings, Me.Page, Me, Me.IsEditable, Me.TabId, Me.ModuleId, Me.ModuleKey)
                    objTemplateController.ClearCache(Me.ModuleId)

                    Response.Redirect(NavigateURL(Me.TabId), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(Me.TabId), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdLandingPageSortOrder_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdLandingPageSortOrder.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim btnUp As ImageButton = CType(e.Item.FindControl("btnUp"), ImageButton)
                Dim btnDown As ImageButton = CType(e.Item.FindControl("btnDown"), ImageButton)

                Dim section As String = CType(e.Item.DataItem, String)

                If Not (btnUp Is Nothing And btnDown Is Nothing) Then

                    If (section = Me.PropertySettings.LandingPageSections.Split(","c)(0)) Then
                        btnUp.Visible = False
                    End If

                    If (section = Me.PropertySettings.LandingPageSections.Split(","c)(Me.PropertySettings.LandingPageSections.Split(","c).Length - 1)) Then
                        btnDown.Visible = False
                    End If

                    btnUp.CommandArgument = section
                    btnUp.CommandName = "Up"

                    btnDown.CommandArgument = section
                    btnDown.CommandName = "Down"

                End If

                Dim lblSection As Label = CType(e.Item.FindControl("lblSection"), Label)

                If Not (lblSection Is Nothing) Then
                    lblSection.Text = Localization.GetString(section, Me.LocalResourceFile)
                End If

            End If

        End Sub

        Private Sub grdLandingPageSortOrder_ItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdLandingPageSortOrder.ItemCommand

            Dim newOrder As String() = Me.PropertySettings.LandingPageSections.Split(","c)
            Dim existingOrder As String() = Me.PropertySettings.LandingPageSections.Split(","c)

            For i As Integer = 0 To existingOrder.Length - 1

                Dim section As String = existingOrder(i)

                If (e.CommandArgument.ToString() = section) Then
                    If (e.CommandName = "Up") Then

                        Dim fieldVal As String = existingOrder(i - 1)
                        Dim fieldValToSwap As String = existingOrder(i)

                        newOrder(i - 1) = fieldValToSwap
                        newOrder(i) = fieldVal

                    Else

                        If (e.CommandName = "Down") Then

                            Dim fieldVal As String = existingOrder(i)
                            Dim fieldValToSwap As String = existingOrder(i + 1)

                            newOrder(i) = fieldValToSwap
                            newOrder(i + 1) = fieldVal

                        End If

                    End If

                End If

            Next

            Dim newValue As String = ""
            For i As Integer = 0 To newOrder.Length - 1
                If (i = 0) Then
                    newValue = newOrder(i)
                Else
                    newValue = newValue & "," & newOrder(i)
                End If
            Next

            Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
            objModules.UpdateModuleSetting(ModuleId, Constants.LANDING_PAGE_SORT_ORDER_SETTING, newValue)

            BindSortOrderGrid()

        End Sub

        Protected Sub lstContactDestination_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstContactdestination.SelectedIndexChanged

            trContactCustomEmail.Visible = (lstContactdestination.SelectedValue = DestinationType.CustomEmail.ToString())
            trContactField.Visible = (lstContactdestination.SelectedValue = DestinationType.CustomField.ToString())

        End Sub

        Protected Sub chkCurrencyShowAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCurrencyShowAll.CheckedChanged

            chkCurrencyAvailableList.Visible = Not chkCurrencyShowAll.Checked

        End Sub

        Protected Sub lstRedirectType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstRedirectType.SelectedIndexChanged

            trRedirectPage.Visible = (lstRedirectType.SelectedValue = RedirectType.Page.ToString())

        End Sub

#End Region

    End Class

End Namespace