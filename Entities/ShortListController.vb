Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class ShortListController

#Region " Public Methods "

        Public Sub Add(ByVal objShortlist As ShortListInfo)

            DataProvider.Instance().AddShortlist(objShortlist.PropertyID, objShortlist.UserID, objShortlist.CreateDate, objShortlist.UserKey)

        End Sub

        Public Sub Delete(ByVal propertyID As Integer, ByVal userID As Integer, ByVal userKey As String)

            DataProvider.Instance().DeleteShortlist(propertyID, userID, userKey)

        End Sub

        Public Function [Get](ByVal propertyID As Integer, ByVal userID As Integer, ByVal userKey As String) As ShortListInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetShortlist(propertyID, userID, userKey), GetType(ShortListInfo)), ShortListInfo)

        End Function

#End Region

    End Class

End Namespace


