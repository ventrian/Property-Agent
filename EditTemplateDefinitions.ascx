<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditTemplateDefinitions.ascx.vb" Inherits="Ventrian.PropertyAgent.EditTemplateDefinitions" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td align="left">
		<asp:Repeater ID="rptBreadCrumbs" Runat="server" EnableViewState="False">
			<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
			<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
		</asp:Repeater>
	</td>
	<td align="right">
		<Agent:Options id="Options1" runat="server" />
	</td>
</tr>
<tr>
	<td height="5px" colspan="2"></td>
</tr>
</table>
<asp:datagrid id="grdDefinitions" BorderWidth="0" BorderStyle="None" CellPadding="4" cellspacing="4"
	AutoGenerateColumns="false" EnableViewState="false" runat="server" summary="Module Defs Design Table"
	GridLines="None" Width="100%">
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Width="20px"></ItemStyle>
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# GetEditUrl(DataBinder.Eval(Container.DataItem,"TemplateID").ToString()) %>' Visible="<%# IsTemplateEditable %>" runat="server" ID="Hyperlink1">
					<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" Visible="<%# IsEditable %>" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/>
				</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="Title" HeaderText="Title">
			<HeaderStyle Wrap="False" CssClass="NormalBold"></HeaderStyle>
			<ItemStyle Wrap="False" CssClass="Normal" Width="150px"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Description" HeaderText="Description">
			<HeaderStyle CssClass="NormalBold"></HeaderStyle>
			<ItemStyle CssClass="Normal"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="IsPremium" HeaderText="Premium">
			<HeaderStyle HorizontalAlign="Center" CssClass="NormalBold"></HeaderStyle>
			<ItemStyle HorizontalAlign="Center" CssClass="Normal" Width="100px"></ItemStyle>
		</asp:BoundColumn>
	</Columns>
</asp:datagrid>
<div align="center"><asp:Label ID="lblNoTemplates" Runat="server" CssClass="Normal" ResourceKey="NoTemplates" EnableViewState="False" Visible="False" /></div>
<p align=center>
	<asp:linkbutton id="cmdImportNewTemplate" resourcekey="cmdImportNewTemplate" runat="server" cssclass="CommandButton" text="Import New Template"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdChangeCurrentTemplate" resourcekey="cmdChangeCurrentTemplate" runat="server" cssclass="CommandButton" text="Change Current Template"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdExportCurrentTemplate" resourcekey="cmdExportCurrentTemplate" runat="server" cssclass="CommandButton" text="Export Current Template"
		borderstyle="none" />
</p>
