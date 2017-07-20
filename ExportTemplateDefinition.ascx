<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ExportTemplateDefinition.ascx.vb" Inherits="Ventrian.PropertyAgent.ExportTemplateDefinition" %>
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
		<td height="5" colspan="2"></td>
	</tr>
</table>
<dnn:sectionhead id="dshExportTemplate" cssclass="Head" runat="server" text="Export Template" section="tblExportTemplate"
	resourcekey="ExportTemplate" includerule="True"></dnn:sectionhead>
<TABLE id="tblExportTemplate" cellSpacing="0" cellPadding="2" width="100%" summary="Export Template Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblExportTemplateHelp" cssclass="Normal" runat="server" resourcekey="ExportTemplateDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plTitle" runat="server" resourcekey="Title" suffix=":" controlname="txtTitle"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtTitle" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
			<asp:requiredfieldvalidator id="valTitle" display="Dynamic" resourcekey="valTitle.ErrorMessage" errormessage="<br>You must enter a friendly name." controltovalidate="txtTitle" runat="server" CssClass="NormalRed" />
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plFolder" runat="server" resourcekey="Folder" suffix=":" controlname="txtFolder"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtFolder" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
			<asp:requiredfieldvalidator id="valFolder" display="Dynamic" resourcekey="valFolder.ErrorMessage" errormessage="<br>You must enter a folder." controltovalidate="txtFolder" runat="server" CssClass="NormalRed" />
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plDescription" runat="server" resourcekey="Description" suffix=":" controlname="txtDescription"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtDescription" cssclass="NormalTextBox" width="390" columns="30" textmode="MultiLine" rows="10" maxlength="2000" runat="server" />
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plIncludeTypes" runat="server" resourcekey="IncludeTypes" suffix=":" controlname="chkIncludeTypes"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:CheckBox id="chkIncludeTypes" runat="server" />
		</TD>
	</TR>
</table>
<p align=center>
	<asp:linkbutton id="cmdExportTemplate" resourcekey="cmdExportTemplate" runat="server" cssclass="CommandButton" text="Export Template"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
<p align="center">
	<asp:Label ID="lblExported" Runat="server" CssClass="Normal" />
</p>