Imports System
Imports System.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework

Namespace Ventrian.PropertyAgent

    Public Class TemplatePortalController

#Region " Public Methods "

        Public Function List(ByVal templateID As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListTemplatePortal(templateID), GetType(TemplatePortalInfo))

        End Function

        Public Function Add(ByVal objTemplatePortal As TemplatePortalInfo) As Integer

            Return CType(DataProvider.Instance().AddTemplatePortal(objTemplatePortal.TemplateID, objTemplatePortal.PortalID), Integer)

        End Function


        Public Sub Delete(ByVal templatePortalID As Integer)

            DataProvider.Instance().DeleteTemplatePortal(templatePortalID)

        End Sub

#End Region

    End Class

End Namespace
