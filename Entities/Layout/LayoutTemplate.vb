Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace Ventrian.PropertyAgent

    Public Class LayoutTemplate
        Implements System.Web.UI.ITemplate

#Region " Private Members "

        Private _templateType As ListItemType
        Private _fieldID As String = ""
        Private _fieldTitle As String = ""

#End Region

#Region " Constructors "

        Sub New(ByVal type As ListItemType, ByVal fieldID As String, ByVal fieldTitle As String)

            _templateType = type
            _fieldID = fieldID
            _fieldTitle = fieldTitle

        End Sub

#End Region

#Region " Private Members "

        Sub BindStringColumn(ByVal sender As Object, ByVal e As EventArgs)

            Dim litItem As LiteralControl = CType(sender, LiteralControl)
            Dim Container As DataGridItem = CType(litItem.NamingContainer, DataGridItem)

            Dim objProperty As PropertyInfo = CType(Container.DataItem, PropertyInfo)
            litItem.Text = objProperty.PropertyList(Convert.ToInt32(_fieldID)).ToString()

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn

            Select Case _templateType

                Case ListItemType.Header

                    Dim litTitle As LiteralControl = New LiteralControl
                    litTitle.Text = _fieldTitle
                    container.Controls.Add(litTitle)

                Case ListItemType.Item

                    Dim litItem As LiteralControl = New LiteralControl
                    AddHandler litItem.DataBinding, AddressOf BindStringColumn
                    container.Controls.Add(litItem)

            End Select

        End Sub

#End Region

    End Class

End Namespace