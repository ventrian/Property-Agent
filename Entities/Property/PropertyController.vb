Imports System
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework
Imports DotNetNuke.Services.Search
Imports DotNetNuke.Entities.Modules

Namespace Ventrian.PropertyAgent

    Public Class PropertyController

        Implements ISearchable


#Region " Private Methods "

        Private Function FillPropertyCollection(ByVal dr As IDataReader, ByRef totalRecords As Integer, ByRef typePosition As Integer) As List(Of PropertyInfo)
            'Note:  the DataReader returned from this method should contain 2 result sets.  The first set
            '       contains the TotalRecords, that satisfy the filter, the second contains the page
            '       of data

            Dim arrProperties As New List(Of PropertyInfo)
            Dim objProperty As PropertyInfo
            While dr.Read
                objProperty = CType(CBO.FillObject(dr, GetType(PropertyInfo), False), PropertyInfo)
                arrProperties.Add(objProperty)
            End While

            Dim nextResult As Boolean = dr.NextResult()
            totalRecords = 0

            If dr.Read Then
                Try
                    totalRecords = Convert.ToInt32(dr("TotalRecords"))
                Catch ex As Exception
                    totalRecords = -1
                End Try
            End If

            If (dr.NextResult()) Then
                If (dr.Read()) Then
                    Try
                        typePosition = Convert.ToInt32(dr("TypePosition"))
                    Catch ex As Exception
                        typePosition = -1
                    End Try
                End If
            End If

            If Not dr Is Nothing Then
                dr.Close()
            End If

            Return arrProperties

        End Function

        Private Function GetCustomFields(ByVal moduleID As Integer) As List(Of CustomFieldInfo)

            Dim objCustomFieldController As New CustomFieldController
            Return objCustomFieldController.List(moduleID, True)

        End Function

#End Region

#Region " Public Methods "

        Public Function Count(ByVal moduleID As Integer, ByVal userID As Integer) As Integer

            Return CType(DataProvider.Instance().CountProperty(moduleID, userID), Integer)

        End Function

        Public Function [Get](ByVal propertyID As Integer) As PropertyInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetProperty(propertyID), GetType(PropertyInfo)), PropertyInfo)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal status As SearchStatusType, ByVal authorID As Integer, ByVal brokerID As Integer, ByVal showFeaturedOnly As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal sortBy As SortByType, ByVal sortByID As Integer, ByVal sortOrder As SortDirectionType, ByVal customFieldIDs As String, ByVal searchValues As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByVal searchSubTypes As Boolean) As List(Of PropertyInfo)

            Return List(moduleID, propertyTypeID, status, authorID, brokerID, showFeaturedOnly, OnlyForAuthenticated, sortBy, sortByID, sortOrder, customFieldIDs, searchValues, pageNumber, pageSize, Null.NullInteger, False, searchSubTypes, Null.NullInteger, Null.NullInteger)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal status As SearchStatusType, ByVal authorID As Integer, ByVal brokerID As Integer, ByVal showFeaturedOnly As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal sortBy As SortByType, ByVal sortByID As Integer, ByVal sortOrder As SortDirectionType, ByVal customFieldIDs As String, ByVal searchValues As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer, ByVal bubbleFeatured As Boolean, ByVal searchSubTypes As Boolean, ByVal propertyIDForNextPrev As Integer, ByRef typePosition As Integer) As List(Of PropertyInfo)

            Return List(moduleID, propertyTypeID, status, authorID, brokerID, showFeaturedOnly, OnlyForAuthenticated, sortBy, sortByID, sortOrder, customFieldIDs, searchValues, pageNumber, pageSize, totalRecords, bubbleFeatured, searchSubTypes, propertyIDForNextPrev, typePosition, Null.NullDouble, Null.NullDouble, Null.NullDate)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal status As SearchStatusType, ByVal authorID As Integer, ByVal brokerID As Integer, ByVal showFeaturedOnly As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal sortBy As SortByType, ByVal sortByID As Integer, ByVal sortOrder As SortDirectionType, ByVal customFieldIDs As String, ByVal searchValues As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer, ByVal bubbleFeatured As Boolean, ByVal searchSubTypes As Boolean, ByVal propertyIDForNextPrev As Integer, ByRef typePosition As Integer, ByVal latitude As Double, ByVal longitude As Double, ByVal startDate As DateTime) As List(Of PropertyInfo)

            Return List(moduleID, propertyTypeID, status, authorID, brokerID, showFeaturedOnly, OnlyForAuthenticated, sortBy, sortByID, sortOrder, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullInteger, customFieldIDs, searchValues, pageNumber, pageSize, totalRecords, bubbleFeatured, searchSubTypes, propertyIDForNextPrev, typePosition, latitude, longitude, startDate, Null.NullString, Null.NullInteger)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal status As SearchStatusType, ByVal authorID As Integer, ByVal brokerID As Integer, ByVal showFeaturedOnly As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal sortBy As SortByType, ByVal sortByID As Integer, ByVal sortOrder As SortDirectionType, ByVal customFieldIDs As String, ByVal searchValues As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer, ByVal bubbleFeatured As Boolean, ByVal searchSubTypes As Boolean, ByVal propertyIDForNextPrev As Integer, ByRef typePosition As Integer, ByVal latitude As Double, ByVal longitude As Double, ByVal startDate As DateTime, ByVal agentFilter As String) As List(Of PropertyInfo)

            Return List(moduleID, propertyTypeID, status, authorID, brokerID, showFeaturedOnly, False, sortBy, sortByID, sortOrder, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullInteger, customFieldIDs, searchValues, pageNumber, pageSize, totalRecords, bubbleFeatured, searchSubTypes, propertyIDForNextPrev, typePosition, latitude, longitude, startDate, agentFilter, Null.NullInteger)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal propertyTypeID As Integer, ByVal status As SearchStatusType, ByVal authorID As Integer, ByVal brokerID As Integer, ByVal showFeaturedOnly As Boolean, ByVal OnlyForAuthenticated As Boolean, ByVal sortBy As SortByType, ByVal sortByID As Integer, ByVal sortOrder As SortDirectionType, ByVal sortBy2 As SortByTypeSecondary, ByVal sortByID2 As Integer, ByVal sortOrder2 As SortDirectionType, ByVal sortBy3 As SortByTypeSecondary, ByVal sortByID3 As Integer, ByVal sortOrder3 As SortDirectionType, ByVal customFieldIDs As String, ByVal searchValues As String, ByVal pageNumber As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer, ByVal bubbleFeatured As Boolean, ByVal searchSubTypes As Boolean, ByVal propertyIDForNextPrev As Integer, ByRef typePosition As Integer, ByVal latitude As Double, ByVal longitude As Double, ByVal startDate As DateTime, ByVal agentFilter As String, ByVal shortListID As String) As List(Of PropertyInfo)

            Dim objCustomFields As List(Of CustomFieldInfo) = GetCustomFields(moduleID)

            Dim sortByIDType As Integer = Null.NullInteger

            If (sortByID <> Null.NullInteger) Then
                For Each objCustomField As CustomFieldInfo In objCustomFields
                    If (objCustomField.CustomFieldID = sortByID) Then
                        sortByIDType = CType(objCustomField.ValidationType, Integer)
                    End If
                Next
            End If

            Dim sortByIDType2 As Integer = Null.NullInteger

            If (sortByID2 <> Null.NullInteger) Then
                For Each objCustomField As CustomFieldInfo In objCustomFields
                    If (objCustomField.CustomFieldID = sortByID2) Then
                        sortByIDType2 = CType(objCustomField.ValidationType, Integer)
                    End If
                Next
            End If

            Dim sortByIDType3 As Integer = Null.NullInteger

            If (sortByID3 <> Null.NullInteger) Then
                For Each objCustomField As CustomFieldInfo In objCustomFields
                    If (objCustomField.CustomFieldID = sortByID3) Then
                        sortByIDType3 = CType(objCustomField.ValidationType, Integer)
                    End If
                Next
            End If

            Dim searchStatus As SearchStatusType = status

            Dim isActive As Boolean = Null.NullBoolean
            Dim isPending As Boolean = Null.NullBoolean
            Dim isExpired As Boolean = Null.NullBoolean

            Select Case status

                Case SearchStatusType.PublishedActive
                    isActive = True
                    Exit Select

                Case SearchStatusType.PublishedExpired
                    isExpired = True
                    searchStatus = SearchStatusType.PublishedActive
                    Exit Select

                Case SearchStatusType.PublishedPending
                    isPending = True
                    searchStatus = SearchStatusType.PublishedActive
                    Exit Select

            End Select

            Dim objPropertyValueController As New PropertyValueController
            Dim objPropertyList As List(Of PropertyInfo) = FillPropertyCollection(DataProvider.Instance().ListProperty(moduleID, propertyTypeID, searchStatus, authorID, brokerID, isActive, isPending, isExpired, showFeaturedOnly, OnlyForAuthenticated, sortBy, sortByID, sortByIDType, sortOrder, sortBy2, sortByID2, sortByIDType2, sortOrder2, sortBy3, sortByID3, sortByIDType3, sortOrder3, customFieldIDs, searchValues, pageNumber, pageSize, bubbleFeatured, searchSubTypes, propertyIDForNextPrev, latitude, longitude, startDate, agentFilter, shortListID), totalRecords, typePosition)
            Return objPropertyList

        End Function

        Public Function Add(ByVal objProperty As PropertyInfo) As Integer

            Return CType(DataProvider.Instance().AddProperty(objProperty.ModuleID, objProperty.PropertyTypeID, objProperty.IsFeatured, objProperty.OnlyForAuthenticated, objProperty.DateCreated, objProperty.DateModified, objProperty.DatePublished, objProperty.DateExpired, objProperty.ViewCount, objProperty.Status, objProperty.AuthorID, objProperty.ModifiedID, objProperty.Latitude, objProperty.Longitude), Integer)

        End Function

        Public Sub AddStatistic(ByVal propertyID As Integer, ByVal userID As Integer, ByVal remoteAddress As String, ByVal moduleID As Integer)

            DataProvider.Instance().AddStatistic(propertyID, userID, remoteAddress, moduleID)

        End Sub

        Public Function StatisticGet(ByVal propertyID As Integer) As List(Of StatisticInfo)

            Return CBO.FillCollection(Of StatisticInfo)(DataProvider.Instance().StatisticGet(propertyID))

        End Function

        Public Function StatisticList(ByVal moduleID As Integer) As List(Of StatisticInfo)

            Return CBO.FillCollection(Of StatisticInfo)(DataProvider.Instance().StatisticList(moduleID))

        End Function

        Public Sub Update(ByVal objProperty As PropertyInfo)

            DataProvider.Instance().UpdateProperty(objProperty.PropertyID, objProperty.ModuleID, objProperty.PropertyTypeID, objProperty.IsFeatured, objProperty.OnlyForAuthenticated, objProperty.DateCreated, objProperty.DateModified, objProperty.DatePublished, objProperty.DateExpired, objProperty.ViewCount, objProperty.Status, objProperty.AuthorID, objProperty.ModifiedID, objProperty.Latitude, objProperty.Longitude)

        End Sub

        Public Sub Delete(ByVal propertyID As Integer)

            DataProvider.Instance().DeleteProperty(propertyID)

        End Sub

        Public Sub DeleteByModuleID(ByVal moduleID As Integer)

            DataProvider.Instance().DeletePropertyByModuleID(moduleID)

        End Sub

#End Region

#Region " Optional Interfaces "

        Public Function GetSearchItems(ByVal ModInfo As DotNetNuke.Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements DotNetNuke.Entities.Modules.ISearchable.GetSearchItems

            Dim objModuleController As New ModuleController
            Dim objSettings As Hashtable = objModuleController.GetModuleSettings(ModInfo.ModuleID)

            If (objSettings.Contains(Constants.CORE_SEARCH_ENABLED_SETTING)) Then
                If (Convert.ToBoolean(objSettings(Constants.CORE_SEARCH_ENABLED_SETTING).ToString())) Then

                    Dim title As String = ""
                    If (objSettings.Contains(Constants.CORE_SEARCH_TITLE_SETTING)) Then
                        title = objSettings(Constants.CORE_SEARCH_TITLE_SETTING).ToString()
                    End If

                    If (title <> "") Then

                        Dim objCustomFieldController As New CustomFieldController
                        Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(ModInfo.ModuleID, True)

                        Dim delimStr As String = "[]"
                        Dim delimiter As Char() = delimStr.ToCharArray()
                        Dim layoutArrayTitle As String() = title.Split(delimiter)

                        Dim description As String = ""
                        If (objSettings.Contains(Constants.CORE_SEARCH_DESCRIPTION_SETTING)) Then
                            description = objSettings(Constants.CORE_SEARCH_DESCRIPTION_SETTING).ToString()
                        End If

                        Dim SearchItemCollection As New SearchItemInfoCollection

                        Dim objPropertyController As New PropertyController
                        Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(ModInfo.ModuleID, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, Null.NullBoolean, Null.NullBoolean, SortByType.Published, Null.NullInteger, SortDirectionType.Descending, Null.NullString, Null.NullString, 0, 100000, Null.NullBoolean)

                        For Each objProperty As PropertyInfo In objProperties

                            Dim strTitle As String = ""

                            For iPtr As Integer = 0 To layoutArrayTitle.Length - 1 Step 2

                                strTitle = strTitle & layoutArrayTitle(iPtr).ToString()

                                If iPtr < layoutArrayTitle.Length - 1 Then
                                    Select Case layoutArrayTitle(iPtr + 1).ToUpper()

                                        Case Else
                                            If (layoutArrayTitle(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                                Dim field As String = layoutArrayTitle(iPtr + 1).Substring(7, layoutArrayTitle(iPtr + 1).Length - 7).ToLower()

                                                Dim objCustomFieldSelected As New CustomFieldInfo

                                                For Each objCustomField As CustomFieldInfo In objCustomFields
                                                    If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                                        objCustomFieldSelected = objCustomField
                                                    End If
                                                Next

                                                If (objCustomFieldSelected IsNot Nothing) Then
                                                    If (objProperty.PropertyList.Contains(objCustomFieldSelected.CustomFieldID)) Then
                                                        strTitle = strTitle & objProperty.PropertyList(objCustomFieldSelected.CustomFieldID).ToString()
                                                    End If
                                                End If
                                            End If

                                    End Select
                                End If

                                strTitle = HtmlUtils.Shorten(HtmlUtils.Clean(System.Web.HttpUtility.HtmlDecode(strTitle), False), 100, "...")

                            Next

                            Dim strDescription As String = ""

                            If (description <> "") Then

                                Dim layoutArrayDescription As String() = description.Split(delimiter)

                                For iPtr As Integer = 0 To layoutArrayDescription.Length - 1 Step 2

                                    strDescription = strDescription & layoutArrayDescription(iPtr).ToString()

                                    If iPtr < layoutArrayDescription.Length - 1 Then
                                        Select Case layoutArrayDescription(iPtr + 1).ToUpper()

                                            Case Else
                                                If (layoutArrayDescription(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then
                                                    Dim field As String = layoutArrayDescription(iPtr + 1).Substring(7, layoutArrayDescription(iPtr + 1).Length - 7).ToLower()

                                                    Dim objCustomFieldSelected As New CustomFieldInfo

                                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                                            objCustomFieldSelected = objCustomField
                                                        End If
                                                    Next

                                                    If (objCustomFieldSelected IsNot Nothing) Then
                                                        If (objProperty.PropertyList.Contains(objCustomFieldSelected.CustomFieldID)) Then
                                                            strDescription = strDescription & objProperty.PropertyList(objCustomFieldSelected.CustomFieldID).ToString()
                                                        End If
                                                    End If
                                                End If

                                        End Select
                                    End If

                                Next

                                strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(System.Web.HttpUtility.HtmlDecode(strDescription), False), 100, "...")

                            End If

                            Dim strContent As String = ""
                            For Each objCustomField As CustomFieldInfo In objCustomFields
                                If (objCustomField.IsPublished) Then
                                    If (objProperty.PropertyList.Contains(objCustomField.CustomFieldID)) Then
                                        strContent = strContent & objProperty.PropertyList(objCustomField.CustomFieldID).ToString() & vbCrLf
                                    End If
                                End If
                            Next

                            Dim SearchItem As New SearchItemInfo(strTitle, strDescription, Null.NullInteger, objProperty.DateModified, objProperty.ModuleID, objProperty.PropertyID.ToString(), strContent, "agentType=View&PropertyID=" & objProperty.PropertyID.ToString())
                            SearchItemCollection.Add(SearchItem)
                        Next

                        Return SearchItemCollection

                    End If
                End If
            End If

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
