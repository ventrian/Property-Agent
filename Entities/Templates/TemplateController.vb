Imports System
Imports System.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework

Namespace Ventrian.PropertyAgent

    Public Class TemplateController

#Region " Public Methods "

        Public Function [Get](ByVal templateID As Integer) As TemplateInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetTemplate(templateID), GetType(TemplateInfo)), TemplateInfo)

        End Function

        Public Function [GetByFolder](ByVal folder As String) As TemplateInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetTemplateByFolder(folder), GetType(TemplateInfo)), TemplateInfo)

        End Function

        Public Function List(ByVal portalID As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListTemplate(portalID), GetType(TemplateInfo))

        End Function

        Public Function Add(ByVal objTemplate As TemplateInfo) As Integer

            Return CType(DataProvider.Instance().AddTemplate(objTemplate.Title, objTemplate.Description, objTemplate.Folder, objTemplate.IsPremium), Integer)

        End Function

        Public Sub Update(ByVal objTemplate As TemplateInfo)

            DataProvider.Instance().UpdateTemplate(objTemplate.TemplateID, objTemplate.Title, objTemplate.Description, objTemplate.Folder, objTemplate.IsPremium)

        End Sub

        Public Sub Delete(ByVal templateID As Integer)

            DataProvider.Instance().DeleteTemplate(templateID)

        End Sub

#End Region

    End Class

End Namespace
