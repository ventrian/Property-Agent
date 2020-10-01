<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ImportTemplateDefinition.ascx.vb" Inherits="Ventrian.PropertyAgent.ImportTemplateDefinition" %>
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
<dnn:sectionhead id="dshImportTemplate" cssclass="Head" runat="server" text="Import Template" section="tblImportTemplate"
	resourcekey="ImportTemplate" includerule="True"></dnn:sectionhead>
<asp:updatepanel id="UpdatePanel1" runat="server">
<contenttemplate>
<TABLE id="tblImportTemplate" cellSpacing="0" cellPadding="2" width="100%" summary="Import Template Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblImportTemplateHelp" cssclass="Normal" runat="server" resourcekey="ImportTemplateDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plFile" runat="server" resourcekey="File" suffix=":" controlname="txtFolder"></dnn:label></TD>
		<TD align="left" width="100%">
			        <INPUT id="cmdBrowse" type="file" size="50" name="cmdBrowse" runat="server">
                    
			<asp:CustomValidator id="valFile" runat="server" resourcekey="valFile.ErrorMessage" CssClass="NormalRed"
				ErrorMessage="You Must Upload A File" Display="Dynamic"></asp:CustomValidator>
			<asp:CustomValidator id="valType" runat="server" resourcekey="valType.ErrorMessage" CssClass="NormalRed"
				ErrorMessage="You must upload a file that is either a ZIP" Display="Dynamic"></asp:CustomValidator>
			<asp:CustomValidator id="valFolderAlreadyExists" runat="server" resourcekey="valFolderAlreadyExists.ErrorMessage" CssClass="NormalRed"
				ErrorMessage="A Template With The Same Folder Already Exists" Display="Dynamic"></asp:CustomValidator>
		</TD>
	</TR>
</TABLE>
<p align="center">
	<asp:linkbutton id="cmdUploadFile" resourcekey="cmdUploadFile" runat="server" cssclass="CommandButton"
		text="Upload File" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
</contenttemplate>
    <triggers>
        <asp:postbacktrigger controlid="cmdUploadFile">
        </asp:postbacktrigger>
    </triggers>
</asp:updatepanel>
