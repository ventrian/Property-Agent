'
' Property Agent for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2007
' by Ventrian Systems ( support@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Security
Imports DotNetNuke.Entities.Modules

Namespace Ventrian.PropertyAgent

    Partial Public Class Options
        Inherits PortalModuleBase

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If (LayoutController.CheckTemplate(CType(Parent, PropertyAgentBase).PortalSettings, CType(Parent, PropertyAgentBase).ModuleId, CType(Parent, PropertyAgentBase).PropertySettings.Template, LayoutType.Listing_Item_Html) = False) Then
                Return
            End If

            Dim objLayoutController As New LayoutController(CType(Parent, PropertyAgentBase).PortalSettings, CType(Parent, PropertyAgentBase).PropertySettings, Me.Page, CType(Parent, PropertyAgentBase), CType(Parent, PropertyAgentBase).IsEditable, CType(Parent, PropertyAgentBase).TabId, CType(Parent, PropertyAgentBase).ModuleId, CType(Parent, PropertyAgentBase).ModuleKey & "-Options")
            Dim objOptions As LayoutInfo = objLayoutController.GetLayout(CType(Parent, PropertyAgentBase).PropertySettings.Template, LayoutType.Option_Item_Html)

            objLayoutController.ProcessOptionItem(phOption.Controls, objOptions.Tokens)

        End Sub

#End Region

    End Class

End Namespace