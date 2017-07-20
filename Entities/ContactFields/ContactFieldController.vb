Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class ContactFieldController

#Region " Public Methods "

        Public Function [Get](ByVal ContactFieldID As Integer) As ContactFieldInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetContactField(ContactFieldID), GetType(ContactFieldInfo)), ContactFieldInfo)

        End Function

        Public Function List(ByVal moduleID As Integer) As List(Of ContactFieldInfo)

            Dim cacheKey As String = moduleID.ToString() & "-PropertyAgent-ContactFieldList-All"

            Dim objContactFields As List(Of ContactFieldInfo) = CType(DataCache.GetCache(cacheKey), List(Of ContactFieldInfo))

            If (objContactFields Is Nothing) Then
                objContactFields = CBO.FillCollection(Of ContactFieldInfo)(DataProvider.Instance().ListContactField(moduleID))
                DataCache.SetCache(cacheKey, objContactFields)
            End If

            For Each objContactField As ContactFieldInfo In objContactFields
                objContactField.DefaultValue = ""
            Next

            Return objContactFields

        End Function

        Public Function Add(ByVal objContactField As ContactFieldInfo) As Integer

            DataCache.RemoveCache(objContactField.ModuleID.ToString() & "-PropertyAgent-ContactFieldList-All")
            Return CType(DataProvider.Instance().AddContactField(objContactField.ModuleID, objContactField.Name, objContactField.FieldType, objContactField.FieldElements, objContactField.DefaultValue, objContactField.Caption, objContactField.CaptionHelp, objContactField.SortOrder, objContactField.IsRequired, objContactField.Length, objContactField.CustomFieldID), Integer)

        End Function

        Public Sub Update(ByVal objContactField As ContactFieldInfo)

            DataCache.RemoveCache(objContactField.ModuleID.ToString() & "-PropertyAgent-ContactFieldList-All")
            DataProvider.Instance().UpdateContactField(objContactField.ContactFieldID, objContactField.ModuleID, objContactField.Name, objContactField.FieldType, objContactField.FieldElements, objContactField.DefaultValue, objContactField.Caption, objContactField.CaptionHelp, objContactField.SortOrder, objContactField.IsRequired, objContactField.Length, objContactField.CustomFieldID)

        End Sub

        Public Sub Delete(ByVal ContactFieldID As Integer)

            Dim objContactField As ContactFieldInfo = Me.Get(ContactFieldID)

            If Not (objContactField Is Nothing) Then
                DataCache.RemoveCache(objContactField.ModuleID.ToString() & "-PropertyAgent-ContactFieldList-All")
            End If

            DataProvider.Instance().DeleteContactField(ContactFieldID)

        End Sub

#End Region

    End Class

End Namespace