Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class RatingController

#Region " Public Methods "

        Public Function Add(ByVal objRating As RatingInfo) As Integer

            Return CType(DataProvider.Instance().AddRating(objRating.PropertyID, objRating.UserID, objRating.CommentID, objRating.ReviewID, objRating.Rating, objRating.CreateDate), Integer)

        End Function

        Public Sub Delete(ByVal propertyID As Integer, ByVal reviewID As Integer)

            DataProvider.Instance().DeleteRating(propertyID, reviewID)

        End Sub

        Public Function [Get](ByVal propertyID As Integer, ByVal userID As Integer) As RatingInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetRating(propertyID, userID), GetType(RatingInfo)), RatingInfo)

        End Function

#End Region

    End Class

End Namespace