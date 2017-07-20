Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace Ventrian.PropertyAgent

    Public Class SubscriberController

        Public Sub AddSubscriber(ByVal propertyId As Integer, ByVal userId As Integer)

            DataProvider.Instance().AddSubscriber(propertyId, userId)

        End Sub

        Public Sub DeleteSubscriber(ByVal propertyId As Integer, ByVal userId As Integer)

            DataProvider.Instance().DeleteSubscriber(propertyId, userId)

        End Sub

        Public Function ListSubscribers(ByVal propertyId As Integer) As List(Of UserInfo)

            Return CBO.FillCollection(Of UserInfo)(DataProvider.Instance().ListSubscribers(propertyId))

        End Function

    End Class

End Namespace
