Imports DotNetNuke.Common.Utilities
Imports System

Namespace Ventrian.PropertyAgent

    Public Class PropertyTypeController

#Region " Public Methods "

        Public Function [Get](ByVal moduleID As Integer, ByVal propertyTypeID As Integer) As PropertyTypeInfo

            If (propertyTypeID = Null.NullInteger) Then
                Return Nothing
            End If

            Dim objPropertyTypes As List(Of PropertyTypeInfo) = ListAll(moduleID, False, PropertyTypeSortByType.Standard, Null.NullString())
            For Each objPropertyType As PropertyTypeInfo In objPropertyTypes
                If (objPropertyType.PropertyTypeID = propertyTypeID) Then
                    Return objPropertyType
                End If
            Next

            Return Nothing

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal showPublishedOnly As Boolean) As List(Of PropertyTypeInfo)

            Return List(moduleID, showPublishedOnly, PropertyTypeSortByType.Standard, Null.NullString, Null.NullInteger)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal showPublishedOnly As Boolean, ByVal sortBy As PropertyTypeSortByType, ByVal agentFilter As String, ByVal parentID As Integer) As List(Of PropertyTypeInfo)

            Dim objTypesSelected As New List(Of PropertyTypeInfo)

            Dim objPropertyTypes As List(Of PropertyTypeInfo) = ListAll(moduleID, showPublishedOnly, sortBy, Null.NullString)
            For Each objPropertyType As PropertyTypeInfo In objPropertyTypes
                If (objPropertyType.ParentID = parentID) Then
                    objTypesSelected.Add(objPropertyType)
                End If
            Next

            Return objTypesSelected

        End Function

        Public Function ListAll(ByVal moduleID As Integer, ByVal showPublishedOnly As Boolean, ByVal sortBy As PropertyTypeSortByType, ByVal agentFilter As String) As List(Of PropertyTypeInfo)

            If (agentFilter = "") Then
                Dim cacheKey As String = moduleID.ToString() & "-PropertyAgent-PropertyTypes-" & showPublishedOnly.ToString() & "-" & sortBy.ToString()

                Dim objPropertyTypes As List(Of PropertyTypeInfo) = CType(DataCache.GetCache(cacheKey), List(Of PropertyTypeInfo))

                If (objPropertyTypes Is Nothing) Then
                    objPropertyTypes = CBO.FillCollection(Of PropertyTypeInfo)(DataProvider.Instance().ListPropertyTypeAll(moduleID, showPublishedOnly, sortBy, agentFilter))
                    DataCache.SetCache(cacheKey, objPropertyTypes)
                End If

                Return objPropertyTypes
            Else
                Dim objPropertyTypes As List(Of PropertyTypeInfo) = CBO.FillCollection(Of PropertyTypeInfo)(DataProvider.Instance().ListPropertyTypeAll(moduleID, showPublishedOnly, sortBy, agentFilter))
                Return objPropertyTypes
            End If

        End Function

        Public Function Add(ByVal objPropertyType As PropertyTypeInfo) As Integer

            RemoveCache(objPropertyType.ModuleID.ToString())
            Return CType(DataProvider.Instance().AddPropertyType(objPropertyType.ParentID, objPropertyType.ModuleID, objPropertyType.Name, objPropertyType.Description, objPropertyType.ImageFile, objPropertyType.SortOrder, objPropertyType.IsPublished, objPropertyType.AllowProperties), Integer)

        End Function

        Public Sub Update(ByVal objPropertyType As PropertyTypeInfo)

            RemoveCache(objPropertyType.ModuleID.ToString())
            DataProvider.Instance().UpdatePropertyType(objPropertyType.PropertyTypeID, objPropertyType.ParentID, objPropertyType.ModuleID, objPropertyType.Name, objPropertyType.Description, objPropertyType.ImageFile, objPropertyType.SortOrder, objPropertyType.IsPublished, objPropertyType.AllowProperties)

        End Sub

        Public Sub Delete(ByVal moduleID As Integer, ByVal propertyTypeID As Integer)

            RemoveCache(moduleID)
            DataProvider.Instance().DeletePropertyType(propertyTypeID)

        End Sub

        Public Shared Sub RemoveCache(ByVal moduleID As Integer)

            DataCache.RemoveCache(moduleID.ToString() & "-PropertyAgent-PropertyTypes-" & True.ToString() & "-" & PropertyTypeSortByType.Name.ToString())
            DataCache.RemoveCache(moduleID.ToString() & "-PropertyAgent-PropertyTypes-" & False.ToString() & "-" & PropertyTypeSortByType.Name.ToString())
            DataCache.RemoveCache(moduleID.ToString() & "-PropertyAgent-PropertyTypes-" & True.ToString() & "-" & PropertyTypeSortByType.Standard.ToString())
            DataCache.RemoveCache(moduleID.ToString() & "-PropertyAgent-PropertyTypes-" & False.ToString() & "-" & PropertyTypeSortByType.Standard.ToString())

        End Sub

#End Region

    End Class

End Namespace
