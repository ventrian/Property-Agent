<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditLayoutFiles.ascx.vb" Inherits="Ventrian.PropertyAgent.EditLayoutFiles" %>
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
<div align="Center"><asp:Label ID="lblLayoutFilesUpdated" Runat="server" ResourceKey="LayoutFilesUpdated" EnableViewState="False" CssClass="NormalBold" Visible="False" /></div>
<dnn:sectionhead id="dshEditLayoutFiles" cssclass="Head" runat="server" text="Edit Layout Files" section="tblEditLayoutFiles"
	resourcekey="EditLayoutFiles" includerule="True"></dnn:sectionhead>
<TABLE id="tblEditLayoutFiles" cellSpacing="0" cellPadding="2" width="100%" summary="Edit Layout Files Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblEditLayoutFilesHelp" cssclass="Normal" runat="server" resourcekey="EditLayoutFilesDescription"
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
	<tr runat="server" id="trHeader">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plHeader" runat="server" controlname="txtHeader" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtHeader" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
	<tr runat="server" id="trItem">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plItem" runat="server" controlname="txtItem" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtItem" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
	<tr runat="server" id="trAlternate">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plAlternate" runat="server" controlname="txtAlternate" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtAlternate" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
	<tr runat="server" id="trSeparator">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plSeparator" runat="server" controlname="txtItem" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSeparator" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
	<tr runat="server" id="trFooter">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plFooter" runat="server" controlname="txtFooter" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtFooter" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
	<tr runat="server" id="trPhotoFirst">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plPhotoFirst" runat="server" controlname="txtPhotoFirst" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPhotoFirst" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
	<tr runat="server" id="trPhoto">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plPhoto" runat="server" controlname="txtPhoto" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPhoto" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
		</td>
	</tr>
	<tr runat="server" id="trPageTitle">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plPageTitle" runat="server" controlname="txtPageTitle" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPageTitle" width="350" cssclass="NormalTextBox" runat="server" MaxLength="255" />
		</td>
	</tr>
	<tr runat="server" id="trPageDescription">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plPageDescription" runat="server" controlname="txtPageDescription" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPageDescription" width="350" cssclass="NormalTextBox" runat="server" MaxLength="255" />
		</td>
	</tr>
	<tr runat="server" id="trPageKeywords">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plPageKeywords" runat="server" controlname="txtPageKeywords" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPageKeywords" width="350" cssclass="NormalTextBox" runat="server" MaxLength="255" />
		</td>
	</tr>
	<tr runat="server" id="trPageHeader">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="150"><dnn:label id="plPageHeader" runat="server" controlname="txtPageHeader" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPageHeader" width="350" cssclass="NormalTextBox" runat="server" textmode="MultiLine" rows="10" />
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
