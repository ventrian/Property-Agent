<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditCustomFields.ascx.vb" Inherits="Ventrian.PropertyAgent.EditCustomFields" %>
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
	<td height="5px" colspan="2"></td>
</tr>
</table>
<table width="100%" border="0">
	<tr>
		<td align="left" class="Normal">
		</td>
		<td align="right" class="Normal">
			<asp:Label ID="lblRecordsPerPage" Runat="server" resourcekey="RecordsPage" CssClass="SubHead">Records Per Page:</asp:Label><br>
			<asp:DropDownList id="drpRecordsPerPage" Runat="server" AutoPostBack="True" CssClass="NormalTextBox">
				<asp:ListItem Value="10">10</asp:ListItem>
				<asp:ListItem Value="25" Selected="True">25</asp:ListItem>
				<asp:ListItem Value="50">50</asp:ListItem>
				<asp:ListItem Value="100">100</asp:ListItem>
				<asp:ListItem Value="250">250</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
</table>
<asp:datagrid id="grdCustomFields" Border="0" CellPadding="4" width="100%" AutoGenerateColumns="false"
	runat="server" summary="Custom Fields Table" BorderStyle="None" BorderWidth="0px"
	GridLines="None">
	<Columns>
		<asp:TemplateColumn ItemStyle-Width="20">
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# GetCustomFieldEditUrl(DataBinder.Eval(Container.DataItem,"CustomFieldID").ToString()) %>' runat="server" ID="Hyperlink1">
				<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn HeaderText="Name" DataField="Name" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />
		<asp:BoundColumn HeaderText="Caption" DataField="Caption" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
		<asp:BoundColumn HeaderText="FieldType" DataField="FieldType" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
		<asp:TemplateColumn>
			<HeaderTemplate>
				<asp:Label ID="lblFeatured" Runat="server" ResourceKey="Featured.Header" CssClass="NormalBold" />
			</HeaderTemplate>
			<ItemStyle Width="60px" HorizontalAlign="Center" />
			<ItemTemplate>
				<asp:ImageButton ID="btnFeatured" Runat="server" ImageUrl="~/Images/checked.gif" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<HeaderTemplate>
				<asp:Label ID="lblPublished" Runat="server" ResourceKey="Published.Header" CssClass="NormalBold" />
			</HeaderTemplate>
			<ItemStyle Width="60px" HorizontalAlign="Center" />
			<ItemTemplate>
				<asp:ImageButton ID="btnPublished" Runat="server" ImageUrl="~/Images/checked.gif" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<HeaderTemplate>
				<asp:Label ID="lblLockDown" Runat="server" ResourceKey="LockDown.Header" CssClass="NormalBold" />
			</HeaderTemplate>
			<ItemStyle Width="60px" HorizontalAlign="Center" />
			<ItemTemplate>
				<asp:ImageButton ID="btnLockDown" Runat="server" ImageUrl="~/Images/checked.gif" />
			</ItemTemplate>
		</asp:TemplateColumn>		
		<asp:TemplateColumn>
			<HeaderTemplate>
				<asp:Label ID="lblSortOrder" Runat="server" ResourceKey="SortOrder.Header" CssClass="NormalBold" />
			</HeaderTemplate>
			<ItemStyle Width="60px" HorizontalAlign="Center" />
			<ItemTemplate>
				<table cellpadding="0" cellspacing="0">
				<tr>
					<td width="16px"><asp:ImageButton ID="btnDown" Runat="server" ImageUrl="~/Images/dn.gif" /></td>
					<td width="16px"><asp:ImageButton ID="btnUp" Runat="server" ImageUrl="~/Images/up.gif" /></td>
				</tr>
				</table>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<div align="center"><asp:Label ID="lblNoCustomFields" Runat="server" CssClass="Normal" ResourceKey="NoCustomFields" EnableViewState="False" Visible="False" /></div>
<dnnsc:PagingControl id="ctlPagingControl" runat="server"></dnnsc:PagingControl>
<p align="center">
	<asp:linkbutton id="cmdAddCustomField" resourcekey="AddCustomField" runat="server" cssclass="CommandButton" text="Add Custom Field" causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdReturnToModule" resourcekey="ReturnToModule" runat="server" cssclass="CommandButton" text="Return To Module" causesvalidation="False" borderstyle="none" />
</p>
