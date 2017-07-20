Imports System
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class CustomFieldController

#Region " Public Methods "

        Public Function [Get](ByVal customFieldID As Integer) As CustomFieldInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetCustomField(customFieldID), GetType(CustomFieldInfo)), CustomFieldInfo)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal isPublishedOnly As Boolean) As List(Of CustomFieldInfo)

            Dim cacheKey As String = moduleID.ToString() & "-PropertyAgent-CustomFieldList-All"

            If (isPublishedOnly) Then
                cacheKey = moduleID.ToString() & "-PropertyAgent-CustomFieldList"
            End If

            Dim objCustomFields As List(Of CustomFieldInfo) = CType(DataCache.GetCache(cacheKey), List(Of CustomFieldInfo))

            If (objCustomFields Is Nothing) Then
                objCustomFields = CBO.FillCollection(Of CustomFieldInfo)(DataProvider.Instance().ListCustomField(moduleID, isPublishedOnly))
                DataCache.SetCache(cacheKey, objCustomFields)
            End If

            Return objCustomFields

        End Function

        Public Function Add(ByVal objCustomField As CustomFieldInfo) As Integer

            DataCache.RemoveCache(objCustomField.ModuleID.ToString() & "-PropertyAgent-CustomFieldList")
            DataCache.RemoveCache(objCustomField.ModuleID.ToString() & "-PropertyAgent-CustomFieldList-All")
            Return CType(DataProvider.Instance().AddCustomField(objCustomField.ModuleID, objCustomField.Name, objCustomField.FieldType, objCustomField.FieldElements, objCustomField.FieldElementType, objCustomField.FieldElementDropDown, objCustomField.DefaultValue, objCustomField.Caption, objCustomField.CaptionHelp, objCustomField.IsInManager, objCustomField.IsSortable, objCustomField.IsInListing, objCustomField.IsCaptionHidden, objCustomField.IsFeatured, objCustomField.IsPublished, objCustomField.IsHidden, objCustomField.IsSearchable, objCustomField.IsLockDown, objCustomField.SearchType, objCustomField.SortOrder, objCustomField.IsRequired, objCustomField.ValidationType, objCustomField.FieldElementsFrom, objCustomField.FieldElementsTo, objCustomField.Length, objCustomField.RegularExpression, objCustomField.IncludeCount, objCustomField.HideZeroCount, objCustomField.InheritSecurity), Integer)

        End Function

        Public Sub Update(ByVal objCustomField As CustomFieldInfo)

            DataCache.RemoveCache(objCustomField.ModuleID.ToString() & "-PropertyAgent-CustomFieldList")
            DataCache.RemoveCache(objCustomField.ModuleID.ToString() & "-PropertyAgent-CustomFieldList-All")
            DataProvider.Instance().UpdateCustomField(objCustomField.CustomFieldID, objCustomField.ModuleID, objCustomField.Name, objCustomField.FieldType, objCustomField.FieldElements, objCustomField.FieldElementType, objCustomField.FieldElementDropDown, objCustomField.DefaultValue, objCustomField.Caption, objCustomField.CaptionHelp, objCustomField.IsInManager, objCustomField.IsSortable, objCustomField.IsInListing, objCustomField.IsCaptionHidden, objCustomField.IsFeatured, objCustomField.IsPublished, objCustomField.IsHidden, objCustomField.IsSearchable, objCustomField.IsLockDown, objCustomField.SearchType, objCustomField.SortOrder, objCustomField.IsRequired, objCustomField.ValidationType, objCustomField.FieldElementsFrom, objCustomField.FieldElementsTo, objCustomField.Length, objCustomField.RegularExpression, objCustomField.IncludeCount, objCustomField.HideZeroCount, objCustomField.InheritSecurity)

        End Sub

        Public Sub Delete(ByVal customFieldID As Integer)

            Dim objCustomField As CustomFieldInfo = Me.Get(customFieldID)

            If Not (objCustomField Is Nothing) Then
                DataCache.RemoveCache(objCustomField.ModuleID.ToString() & "-PropertyAgent-CustomFieldList")
                DataCache.RemoveCache(objCustomField.ModuleID.ToString() & "-PropertyAgent-CustomFieldList-All")
            End If

            DataProvider.Instance().DeleteCustomField(customFieldID)

        End Sub

#End Region

    End Class

End Namespace
