Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class ContactLogController

#Region " Public Methods "

        Public Sub Add(ByVal objContactLog As ContactLogInfo)

            DataProvider.Instance().AddContactLog(objContactLog.ModuleID, objContactLog.DateSent, objContactLog.SentTo, objContactLog.SentFrom, objContactLog.Subject, objContactLog.Body, objContactLog.FieldValues, objContactLog.PropertyID)

        End Sub

        Public Function List(ByVal moduleID As Integer, ByVal dateBegin As DateTime, ByVal dateEnd As DateTime) As List(Of ContactLogInfo)

            Return CBO.FillCollection(Of ContactLogInfo)(DataProvider.Instance().ListContactLog(moduleID, dateBegin, dateEnd))

        End Function

#End Region

    End Class

End Namespace
