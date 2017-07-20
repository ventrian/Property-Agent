<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditPropertyType.ascx.vb" Inherits="Ventrian.PropertyAgent.EditPropertyType" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="left">
			<asp:Repeater ID="rptBreadCrumbs" Runat="server">
				<ItemTemplate>
					<a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold">
						<%# DataBinder.Eval(Container.DataItem, "Caption") %>
					</a>
				</ItemTemplate>
				<SeparatorTemplate>
					&nbsp;&#187;&nbsp;
				</SeparatorTemplate>
			</asp:Repeater>
		</td>
		<td align="right">
			<Agent:Options id="Options1" runat="server" />
		</td>
	</tr>
	<tr>
		<td height="5" colspan="2"></td>
	</tr>
</table>
<dnn:sectionhead id="dshPropertyTypeDetails" cssclass="Head" runat="server" text="Property Type Details"
	section="tblPropertyTypeDetails" resourcekey="PropertyTypeDetails" includerule="True"></dnn:sectionhead>
<TABLE id="tblPropertyTypeDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Property Type Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblPropertyTypeDetailsHelp" cssclass="Normal" runat="server" resourcekey="PropertyTypeDetailsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plParentType" runat="server" resourcekey="ParentType" suffix=":" controlname="drpParentType"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:DropDownList ID="drpParentType" runat="server" CssClass="NormalTextBox" DataTextField="NameIndented"
				DataValueField="PropertyTypeID" />
			<asp:CustomValidator ID="valParentType" Runat="server" CssClass="NormalRed" ControlToValidate="drpParentType" ResourceKey="valParentType.ErrorMessage" />
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plName" runat="server" resourcekey="Name" suffix=":" controlname="txtName"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtName" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
			<asp:requiredfieldvalidator id="valName" cssclass="NormalRed" runat="server" resourcekey="valName" controltovalidate="txtName"
				errormessage="<br>You Must Enter a Valid Name" display="Dynamic"></asp:requiredfieldvalidator>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plDescription" runat="server" resourcekey="Description" suffix=":" controlname="txtDescription"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtDescription" cssclass="NormalTextBox" runat="server" maxlength="2000" columns="30"
				width="325" Rows="4"></asp:textbox>
		</TD>
	</TR>
	<tr valign="top">
		<td width="25"><img height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" noWrap="true" width="150"><dnn:label id="plIsPublished" runat="server" resourcekey="IsPublished" suffix=":" controlname="chkIsPublished"></dnn:label></td>
		<td align="left" width="100%">
			<asp:CheckBox ID="chkIsPublished" Runat="server" Checked="True" />
		</td>
	</tr>
	<tr valign="top">
		<td width="25"><img height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" noWrap="true" width="150"><dnn:label id="plAllowProperties" runat="server" suffix=":" controlname="chkAllowProperties" /></td>
		<td align="left" width="100%">
			<asp:CheckBox ID="chkAllowProperties" Runat="server" Checked="True" />
		</td>
	</tr>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plImage" runat="server" resourcekey="Image" suffix=":" controlname="ctlImage"></dnn:label></TD>
		<TD align="left" width="100%">
		    <dnn:URL ID="ctlImage" runat="server" Width="325" ShowUrls="False" ShowTabs="False"
                ShowLog="False" ShowTrack="False" Required="False"></dnn:URL>
		</TD>
	</TR>
</TABLE>
<p align="center">
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete"
		causesvalidation="False" borderstyle="none" />
</p>
