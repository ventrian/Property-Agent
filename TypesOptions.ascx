<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TypesOptions.ascx.vb" Inherits="Ventrian.PropertyAgent.TypesOptions" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="2" border="0" summary="Settings Design Table">
<tr>
	<td class="SubHead" width="150"><dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID"></dnn:label></td>
	<td valign="bottom">
		<asp:dropdownlist id="drpModuleID" Runat="server" Width="325" datavaluefield="ModuleID" datatextfield="ModuleTitle"
			CssClass="NormalTextBox" />
	</td>
</tr>
<tr>
	<td class="SubHead" width="150"><dnn:label id="plTypesSortBy" runat="server" controlname="drpTypesSortBy"
			suffix=":"></dnn:label></td>
	<td valign="bottom">
		<asp:DropDownList id="drpTypesSortBy" Runat="server" CssClass="NormalTextBox" />
	</td>
</tr>
</table>
<br />
<dnn:sectionhead id="dshFilterSettings" cssclass="Head" runat="server" text="Filter Settings" section="tblFilterSettings"
	resourcekey="FilterSettings" includerule="True"></dnn:sectionhead>
<table id="tblFilterSettings" cellspacing="0" cellpadding="2" width="100%" summary="Layout Settings Design Table"
	border="0" runat="server">
    <tr>
	    <td class="SubHead" width="150" valign="top"><dnn:label id="plAgents" runat="server" controlname="chkHideZero" suffix=":"></dnn:label></td>
	    <td valign="bottom">
	        <asp:Label ID="lblUsername" runat="server" ResourceKey="Username" CssClass="SubHead" />
	        <asp:TextBox ID="txtUsername" runat="server" CssClass="NormalTextBox" />
	        <asp:LinkButton ID="cmdAddAgent" runat="server" ResourceKey="cmdAddAgent" CssClass="CommandButton" /><br />
	        <asp:Repeater ID="rptAgents" runat="server">
	            <ItemTemplate>
	                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Images/delete.gif" CommandName="Delete" CommandArgument='<%#Eval("UserID")%>' ImageAlign="AbsMiddle" />
	                <span class="Normal"><%# Eval("Username") %></span>
	            </ItemTemplate>
	            <SeparatorTemplate><br /></SeparatorTemplate>
	        </asp:Repeater>
	        <asp:Label ID="lblNoAgentFitler" runat="server" CssClass="Normal" ResourceKey="NoAgentFilter" />
	    </td>
    </tr>
    <tr>
	    <td class="SubHead" width="150"><dnn:label id="plHideZero" runat="server" controlname="chkHideZero" suffix=":"></dnn:label></td>
	    <td valign="bottom">
		    <asp:CheckBox ID="chkHideZero" Runat="server" />
	    </td>
    </tr>
    <tr>
	    <td class="SubHead" width="150"><dnn:label id="plShowTopLevelOnly" runat="server" controlname="chkShowTopLevelOnly" suffix=":"></dnn:label></td>
	    <td valign="bottom">
		    <asp:CheckBox ID="chkShowTopLevelOnly" Runat="server" />
	    </td>
    </tr>
    <tr>
	    <td class="SubHead" width="150"><dnn:label id="plShowAllTypes" runat="server" controlname="chkShowAllTypes" suffix=":"></dnn:label></td>
	    <td valign="bottom">
		    <asp:CheckBox ID="chkShowAllTypes" Runat="server" AutoPostBack="true" />
	    </td>
    </tr>
    <tr id="trTypesFilter" visible="false">
	    <td class="SubHead" width="150"><dnn:label id="plTypesFilter" runat="server" controlname="rptTypesFilter" suffix=":"></dnn:label></td>
	    <td valign="bottom">
	        <asp:Repeater ID="rptTypesFilter" runat="server">
	            <ItemTemplate>
	                <div class="Normal"><asp:CheckBox ID="chkSelected" runat="server" /><%#DataBinder.Eval(Container.DataItem, "NameIndented")%></div>
	            </ItemTemplate>
	        </asp:Repeater>
	    </td>
    </tr>
</table>
<br />
<dnn:sectionhead id="dshLayoutSettings" cssclass="Head" runat="server" text="Layout Settings" section="tblLayoutSettings"
	resourcekey="LayoutSettings" includerule="True"></dnn:sectionhead>
<table id="tblLayoutSettings" cellspacing="0" cellpadding="2" width="100%" summary="Layout Settings Design Table"
	border="0" runat="server">
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plLayoutMode" runat="server" controlname="drpSortDirection" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstLayoutMode" Runat="server" CssClass="Normal" RepeatDirection="Horizontal"
				RepeatLayout="Flow" AutoPostBack="True" />
		</td>
	</tr>
	<tr id="trIncludeStylesheet" runat="server">
		<td class="SubHead" width="150"><dnn:label id="plIncludeStylesheet" runat="server" controlname="chkIncludeStylesheet"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox id="chkIncludeStylesheet" Runat="server" />
		</td>
	</tr>
	<tr id="trHeader" runat="Server">
		<td class="SubHead" width="150">
			<dnn:label id="plHeader" controlname="txtHeader" suffix=":" runat="server"></dnn:label></td>
		<td vAlign="bottom">
			<asp:TextBox id="txtHeader" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></td>
	</tr>
	<tr id="trItem" runat="Server">
		<td class="SubHead" width="150">
			<dnn:label id="plItem" controlname="txtItem" suffix=":" runat="server"></dnn:label></td>
		<td vAlign="bottom">
			<asp:TextBox id="txtItem" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></td>
	</tr>
	<tr id="trFooter" runat="Server">
		<td class="SubHead" width="150">
			<dnn:label id="plFooter" controlname="txtFooter" suffix=":" runat="server"></dnn:label></td>
		<td vAlign="bottom">
			<asp:TextBox id="txtFooter" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></td>
	</tr>
</table>
