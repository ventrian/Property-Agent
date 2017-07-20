<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChangeTemplate.ascx.vb" Inherits="Ventrian.PropertyAgent.ChangeTemplate" %>

<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

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
		<td height="5" colspan="2"></td>
	</tr>
</table>
<dnn:SectionHead id="dshChangeTemplate" cssclass="Head" runat="server" text="Change Template" section="tblChangeTemplate"
	includerule="True"></dnn:sectionhead>
<TABLE id="tblChangeTemplate" cellSpacing="0" cellPadding="2" width="100%" summary="Change Template Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblChangeTemplateHelp" cssclass="Normal" runat="server"></asp:label></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plSelectTemplate" runat="server" resourcekey="plSelectTemplate" suffix=":" controlname="drpTemplate"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:DropDownList ID="drpTemplate" Runat="server" DataTextField="Title" DataValueField="TemplateID" />
			<asp:requiredfieldvalidator id="valTemplate" cssclass="NormalRed" runat="server" resourcekey="valTemplate.ErrorMessage"
				controltovalidate="drpTemplate" errormessage="<br>You Must Select A Template" display="Dynamic" InitialValue="-1"></asp:requiredfieldvalidator>
		</TD>
	</TR>
	<TR vAlign="top" runat="server" id="trCopyFiles">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plCopyFiles" runat="server" resourcekey="plCopyFiles" suffix=":" controlname="chkCopyFiles"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:CheckBox ID="chkCopyFiles" Runat="server" />
		</TD>
	</TR>
</TABLE>
<p align="center">
	<asp:linkbutton id="cmdInitTemplate" resourcekey="cmdInitTemplate" runat="server" cssclass="CommandButton"
		text="Initialize Template" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
