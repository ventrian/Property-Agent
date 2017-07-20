Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class ReviewFieldController

#Region " Public Methods "

        Public Function [Get](ByVal reviewFieldID As Integer) As ReviewFieldInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetReviewField(reviewFieldID), GetType(ReviewFieldInfo)), ReviewFieldInfo)

        End Function

        Public Function List(ByVal moduleID As Integer) As List(Of ReviewFieldInfo)

            Dim cacheKey As String = moduleID.ToString() & "-PropertyAgent-ReviewFieldList-All"

            Dim objReviewFields As List(Of ReviewFieldInfo) = CType(DataCache.GetCache(cacheKey), List(Of ReviewFieldInfo))

            If (objReviewFields Is Nothing) Then
                objReviewFields = CBO.FillCollection(Of ReviewFieldInfo)(DataProvider.Instance().ListReviewField(moduleID))
                DataCache.SetCache(cacheKey, objReviewFields)
            End If

            Return objReviewFields

        End Function

        Public Function Add(ByVal objReviewField As ReviewFieldInfo) As Integer

            DataCache.RemoveCache(objReviewField.ModuleID.ToString() & "-PropertyAgent-ReviewFieldList-All")
            Return CType(DataProvider.Instance().AddReviewField(objReviewField.ModuleID, objReviewField.Name, objReviewField.FieldType, objReviewField.FieldElements, objReviewField.DefaultValue, objReviewField.Caption, objReviewField.CaptionHelp, objReviewField.SortOrder, objReviewField.IsRequired, objReviewField.Length), Integer)

        End Function

        Public Sub Update(ByVal objReviewField As ReviewFieldInfo)

            DataCache.RemoveCache(objReviewField.ModuleID.ToString() & "-PropertyAgent-ReviewFieldList-All")
            DataProvider.Instance().UpdateReviewField(objReviewField.ReviewFieldID, objReviewField.ModuleID, objReviewField.Name, objReviewField.FieldType, objReviewField.FieldElements, objReviewField.DefaultValue, objReviewField.Caption, objReviewField.CaptionHelp, objReviewField.SortOrder, objReviewField.IsRequired, objReviewField.Length)

        End Sub

        Public Sub Delete(ByVal reviewFieldID As Integer)

            Dim objReviewField As ReviewFieldInfo = Me.Get(reviewFieldID)

            If Not (objReviewField Is Nothing) Then
                DataCache.RemoveCache(objReviewField.ModuleID.ToString() & "-PropertyAgent-ReviewFieldList-All")
            End If

            DataProvider.Instance().DeleteReviewField(reviewFieldID)

        End Sub

#End Region

    End Class

End Namespace