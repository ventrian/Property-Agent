Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel


Namespace Ventrian.PropertyAgent.Controls

    <System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
    <System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <ToolboxItem(False)> _
    <System.Web.Script.Services.ScriptService()> _
    Public Class Sortable
        Inherits System.Web.Services.WebService

        <WebMethod()> _
        Public Sub UpdateItemsOrder(ByVal itemOrder As String)

            Dim objPhotoController As New PhotoController

            Dim order As Integer = 0

            For Each i As String In itemOrder.Split(","c)

                Dim objPhoto As PhotoInfo = objPhotoController.Get(Convert.ToInt32(i))

                If (objPhoto IsNot Nothing) Then
                    objPhoto.SortOrder = order
                    objPhotoController.Update(objPhoto)
                    order = order + 1
                End If

            Next

        End Sub

    End Class

End Namespace