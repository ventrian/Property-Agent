Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class CommentController

#Region " Public Methods "

        Public Function Add(ByVal objComment As CommentInfo) As Integer

            Return CType(DataProvider.Instance().AddComment(objComment.PropertyID, objComment.ParentID, objComment.UserID, objComment.Comment, objComment.CreateDate, objComment.Name, objComment.Email, objComment.Website), Integer)

        End Function

        Public Function List(ByVal propertyID As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListComment(propertyID), GetType(CommentInfo))

        End Function

        Public Sub Delete(ByVal commentID As Integer)

            DataProvider.Instance().DeleteComment(commentID)

        End Sub

#End Region

    End Class

End Namespace