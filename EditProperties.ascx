<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditProperties.ascx.vb" Inherits="Ventrian.PropertyAgent.EditProperties" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="left">
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
<asp:Panel runat="server" DefaultButton="cmdSearch">
<table width="100%" border="0">
	<tr>
		<td colspan="2" align="center">
			<asp:Label ID="lblLimit" Runat="server" CssClass="Normal" />
		</td>
	</tr>
	<tr>
	    <td align="left" class="Normal" colspan="2">
	       	<asp:Label ID="lblSearch" Runat="server" resourcekey="Search" CssClass="SubHead" EnableViewState="False">Search:</asp:Label><br>
		    <asp:TextBox ID="txtCustomField" runat="Server" CssClass="NormalTextBox" />
			<asp:dropdownlist id="drpCustomFields" DataValueField="CustomFieldID" DataTextField="Name" Runat="server"
				CssClass="NormalTextBox"></asp:dropdownlist>
			<asp:dropdownlist id="drpTypes" DataValueField="PropertyTypeID" DataTextField="NameIndented" Runat="server"
				CssClass="NormalTextBox"></asp:dropdownlist>
			<asp:dropdownlist id="drpStatus" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist>
	    </td>
	</tr>
	<tr>
		<td align="left" class="Normal">
			<asp:Label ID="lblSortBy" Runat="server" resourcekey="SortBy" CssClass="SubHead" EnableViewState="False">Sort By:</asp:Label><br>
			<asp:DropDownList id="drpSortBy" Runat="server" CssClass="NormalTextBox" />
			<asp:DropDownList id="drpSortDirection" Runat="server" CssClass="NormalTextBox" />
		</td>
		<td align="right" class="Normal">
			<asp:Label ID="lblRecordsPerPage" Runat="server" resourcekey="RecordsPage" CssClass="SubHead"
				EnableViewState="False">Records Per Page:</asp:Label><br>
			<asp:DropDownList id="drpRecordsPerPage" Runat="server" CssClass="NormalTextBox">
				<asp:ListItem Value="10">10</asp:ListItem>
				<asp:ListItem Value="25">25</asp:ListItem>
				<asp:ListItem Value="50">50</asp:ListItem>
				<asp:ListItem Value="100">100</asp:ListItem>
				<asp:ListItem Value="250">250</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
	    <td colspan="2" align="Center">
	        <asp:linkbutton id="cmdSearch" runat="server" cssclass="CommandButton" text="Filter Properties" ResourceKey="FilterProperties" causesvalidation="False" borderstyle="none" />
	    </td>
	</tr>
	<tr>
		<td colspan="2"></td>
	</tr>
</table>
</asp:Panel>

<asp:datagrid id="grdProperties" Border="0" CellPadding="4" width="100%" AutoGenerateColumns="false"
	runat="server" summary="Property Design Table" BorderStyle="None" BorderWidth="0px" GridLines="None" EnableViewState="True">
	<Columns>
		<asp:TemplateColumn ItemStyle-Width="20">
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# GetPropertyEditUrl(DataBinder.Eval(Container.DataItem,"PropertyID").ToString()) %>' runat="server" ID="Hyperlink1">
					<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image"
						resourcekey="Edit" />
				</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="Type" DataField="PropertyTypeName" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" />
		<asp:BoundColumn HeaderText="Modified" DataField="DateModified" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold"
			HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
		<asp:TemplateColumn ItemStyle-Width="100px">
			<HeaderStyle HorizontalAlign="Center" />
			<ItemStyle HorizontalAlign="Center" CssClass="Normal" />
			<HeaderTemplate>
				<asp:Label ID="lblStatus" Runat="server" ResourceKey="Status.Header" CssClass="NormalBold"></asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
				<%# GetLocalizedStatus(Container.DataItem) %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="Hits" DataField="ViewCount" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold"
			ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
		<asp:TemplateColumn>
			<HeaderTemplate>
				<asp:Label ID="lblApproved" Runat="server" ResourceKey="Approved.Header" CssClass="NormalBold" />
			</HeaderTemplate>
			<ItemStyle Width="60px" HorizontalAlign="Center" />
			<ItemTemplate>
				<asp:ImageButton ID="btnApproved" Runat="server" ImageUrl="~/Images/checked.gif" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<HeaderTemplate>
				<asp:Label ID="lblDelete" Runat="server" ResourceKey="Delete.Header" CssClass="NormalBold" />
			</HeaderTemplate>
			<ItemStyle Width="60px" HorizontalAlign="Center" />
			<ItemTemplate>
				<asp:ImageButton ID="btnDelete" Runat="server" ImageUrl="~/Images/delete.gif" CommandName="Delete" />
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<div align="center"><asp:Label ID="lblNoProperties" Runat="server" CssClass="Normal" ResourceKey="NoProperties"
		EnableViewState="False" /></div>
<dnnsc:PagingControl id="ctlPagingControl" runat="server" Visible="False"></dnnsc:PagingControl>