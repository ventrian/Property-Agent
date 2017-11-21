<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LandingPage.ascx.vb" Inherits="Ventrian.PropertyAgent.LandingPage" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td align="left" valign="middle">
	</td>
	<td align="right">
		<Agent:Options id="Options1" runat="server" />
	</td>
</tr>
<tr>
	<td colspan="2"></td>
</tr>
</table>
<asp:Panel ID="pnlSearch" runat="server">
    <asp:PlaceHolder ID="phProperty" Runat="server" />
</asp:Panel>