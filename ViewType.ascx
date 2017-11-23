<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewType.ascx.vb" Inherits="Ventrian.PropertyAgent.ViewType" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td align="left" valign="middle">
		<asp:Repeater ID="rptBreadCrumbs" Runat="server">
				<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
				<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
		</asp:Repeater>
	</td>
	<td align="right">
		<Agent:Options id="Options1" runat="server" />
	</td>
</tr>
<tr>
	<td colspan="2"></td>
</tr>
</table>
<div align="right" id="divSort" runat="server">
		<asp:Label ID="lblSortBy" Runat="server" resourcekey="SortBy" CssClass="SubHead" EnableViewState="False">Sort By:</asp:Label>
		<asp:DropDownList id="drpSortBy" Runat="server" CssClass="NormalTextBox" AutoPostBack="True" />
		<asp:DropDownList id="drpSortDirection" Runat="server" CssClass="NormalTextBox" AutoPostBack="True" />
</div>
<asp:PlaceHolder ID="phProperty" Runat="server" />
<asp:DataList ID="dlProperty" Runat="server" EnableViewState="False" Width="100%" CellPadding="0" CellSpacing="0">
	<HeaderTemplate />
	<ItemTemplate />
	<SeparatorTemplate />
	<FooterTemplate />
</asp:DataList>
<asp:Repeater ID="rptProperty" Runat="server" EnableViewState="False">
	<HeaderTemplate />
	<ItemTemplate />
	<SeparatorTemplate />
	<FooterTemplate />
</asp:Repeater>
<div align="center" id="divSearch" runat="server">
	<span class="Normal">
		<asp:HyperLink ID="lnkSearchAgain" Runat="server" CssClass="CommandButton" ResourceKey="SearchAgain" NavigateUrl="#" />&nbsp;
		<asp:HyperLink  ID="lnkNarrowThisSearch" Runat="server" CssClass="CommandButton" ResourceKey="NarrowThisSearch" NavigateUrl="#"  />
	</span>
</div>
<div align="center" id="divExport" runat="Server">
    <asp:HyperLink ID="lnkExport" Runat="server" CssClass="CommandButton" ResourceKey="Export" Target="_blank" />&nbsp;
</div>