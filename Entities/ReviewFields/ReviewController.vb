Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class ReviewController

#Region " Public Methods "

        Public Function Add(ByVal propertyID As Integer, ByVal userID As Integer, ByVal createDate As DateTime, ByVal isApproved As Boolean) As Integer

            Return CType(DataProvider.Instance().AddReview(propertyID, userID, createDate, isApproved), Integer)

        End Function

        Public Function GetReview(ByVal propertyID As Integer, ByVal userID As Integer) As ReviewInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetReview(propertyID, userID), GetType(ReviewInfo)), ReviewInfo)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal propertyID As Integer, ByVal isApproved As Boolean) As List(Of ReviewInfo)

            Return CBO.FillCollection(Of ReviewInfo)(DataProvider.Instance().ListReviews(moduleID, propertyID, isApproved))

        End Function

        Public Function ListValue(ByVal reviewID As Integer) As List(Of ReviewValueInfo)

            Return CBO.FillCollection(Of ReviewValueInfo)(DataProvider.Instance().ListReviewValue(reviewID))

        End Function

        Public Function AddValue(ByVal reviewID As Integer, ByVal reviewFieldID As Integer, ByVal value As String) As Integer

            Return CType(DataProvider.Instance().AddReviewValue(reviewID, reviewFieldID, value), Integer)

        End Function

        Public Sub DeleteValue(ByVal reviewValueID As Integer)

            DataProvider.Instance().DeleteReviewValue(reviewValueID)

        End Sub

        Public Sub Delete(ByVal reviewID As Integer)

            DataProvider.Instance().DeleteReview(reviewID)

        End Sub

        Public Sub Update(ByVal reviewID As Integer, ByVal isApproved As Boolean)

            DataProvider.Instance().UpdateReview(reviewID, isApproved)

        End Sub

#End Region

    End Class

End Namespace
