<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditEmailFiles.ascx.vb" Inherits="Ventrian.PropertyAgent.EditEmailFiles" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
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
<div align="Center"><asp:Label ID="lblEmailFilesUpdated" Runat="server" ResourceKey="EmailFilesUpdated" EnableViewState="False" CssClass="NormalBold" Visible="False" /></div>
<dnn:sectionhead id="dshEditEmailFiles" cssclass="Head" runat="server" text="Edit Email Files" section="tblEditEmailFiles"
	resourcekey="EditEmailFiles" includerule="True"></dnn:sectionhead>
<TABLE id="tblEditEmailFiles" cellSpacing="0" cellPadding="2" width="100%" summary="Edit Layout Files Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblEditEmailFilesHelp" cssclass="Normal" runat="server" resourcekey="EditEmailFilesDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plCurrentTemplate" runat="server" controlname="lblCurrentTemplate" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:Label ID="lblCurrentTemplate" Runat="server" CssClass="Normal" />
		</td>
	</tr>
	<tr>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plLayoutGroups" runat="server" controlname="drpLayoutGroups" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList ID="drpLayoutGroups" Runat="server" AutoPostBack="True" />&nbsp;<asp:Label id="lblLayoutGroup" CssClass="Normal" runat="Server" />
		</td>
	</tr>
	<tr runat="server" id="trSubject">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plSubject" runat="server" controlname="txtSubject" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSubject" width="350" cssclass="NormalTextBox" runat="server" />
		</td>
	</tr>
	<tr runat="server" id="trBody">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plBody" runat="server" controlname="txtBody" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtBody" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
</table>
<p align=center>
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
