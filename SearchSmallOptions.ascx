<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchSmallOptions.ascx.vb" Inherits="Ventrian.PropertyAgent.SearchSmallOptions" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="2" border="0" summary="Settings Design Table">
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID"></dnn:label></td>
		<td valign="bottom">
			<asp:dropdownlist id="drpModuleID" Runat="server" Width="325" datavaluefield="ModuleID" datatextfield="ModuleTitle"
				CssClass="NormalTextBox" AutoPostBack="True"></asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plLayoutMode" runat="server" resourcekey="Module" suffix=":" controlname="lstLayoutMode"></dnn:label></td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstLayoutMode" Runat="server" CssClass="Normal" RepeatDirection="Horizontal"
				RepeatLayout="Flow" AutoPostBack="True" />
		</td>
	</tr>
	<asp:PlaceHolder ID="phLayoutStandard" runat="server">
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSearchWildcard" runat="server" controlname="chkSearchWildcard" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSearchWildcard" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSearchTypes" runat="server" controlname="chkSearchTypes" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSearchTypes" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSearchLocation" runat="server" controlname="chkSearchLocation" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSearchLocation" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSearchBrokers" runat="server" controlname="chkSearchBrokers" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSearchBrokers" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSearchAgents" runat="server" controlname="chkSearchAgents" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSearchAgents" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plCustomFields" runat="server" controlname="lstCustomFields" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBoxList ID="lstCustomFields" Runat="server" CssClass="NormalTextBox" DataTextField="Name" DataValueField="CustomFieldID" />
		</td>
	</tr>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="phLayoutCustom" runat="server">
	<tr>
		<td class="SubHead" width="150">
			<dnn:label id="plSearchItem" controlname="txtSearchItem" suffix=":" runat="server"></dnn:label></td>
		<td vAlign="bottom">
			<asp:TextBox id="txtSearchItem" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></td>
	</tr>
	</asp:PlaceHolder>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSortBy" runat="server" controlname="drpSortBy" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortBy" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSortDirection" runat="server" controlname="drpSortDirection" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortDirection" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshFormSettings" cssclass="Head" runat="server" text="Form Settings" section="tblFormSettings"
	resourcekey="FormSettings" includerule="True"></dnn:sectionhead>
<TABLE id="tblFormSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Form Settings Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="2">
			<asp:label id="lblFormSettingsHelp" cssclass="Normal" runat="server" resourcekey="FormSettingsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plHideHelpIcon" runat="server" controlname="chkHideHelpIcon" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHideHelpIcon" Runat="server" />
		</td>
	</tr>
    <tr>
	    <td class="SubHead" width="150"><dnn:label id="plHideTypeCount" runat="server" controlname="chkHideTypeCount" suffix=":"></dnn:label></td>
	    <td valign="bottom">
		    <asp:CheckBox ID="chkHideTypeCount" Runat="server" />
	    </td>
    </tr>
    <tr>
	    <td class="SubHead" width="150"><dnn:label id="plHideZero" runat="server" controlname="chkHideZero" suffix=":"></dnn:label></td>
	    <td valign="bottom">
		    <asp:CheckBox ID="chkHideZero" Runat="server" />
	    </td>
    </tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plWidth" runat="server" controlname="txtWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valWidth" Runat="server" ControlToValidate="txtWidth" Display="Dynamic" ResourceKey="valWidth.ErrorMessage"
				CssClass="NormalRed" />
			<asp:CompareValidator ID="valWidthIsNumber" Runat="server" ControlToValidate="txtWidth" Display="Dynamic"
				ResourceKey="valWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plRadioButtonItemsPerRow" runat="server" controlname="txtRadioButtonItemsPerRow"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtRadioButtonItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valRadioButtonItemsPerRow" Runat="server" ControlToValidate="txtRadioButtonItemsPerRow"
				Display="Dynamic" ResourceKey="valRadioButtonItemsPerRow.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valRadioButtonItemsPerRowIsNumber" Runat="server" ControlToValidate="txtRadioButtonItemsPerRow"
				Display="Dynamic" ResourceKey="valRadioButtonItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plCheckBoxListItemsPerRow" runat="server" controlname="txtCheckBoxListItemsPerRow"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtCheckBoxListItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valCheckBoxListItemsPerRow" Runat="server" ControlToValidate="txtCheckBoxListItemsPerRow"
				Display="Dynamic" ResourceKey="valCheckBoxListItemsPerRow.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valCheckBoxListItemsPerRowIsNumber" Runat="server" ControlToValidate="txtCheckBoxListItemsPerRow"
				Display="Dynamic" ResourceKey="valCheckBoxListItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSearchStyle" runat="server" controlname="txtSearchStyle" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSearchStyle" Runat="server" CssClass="NormalTextBox" width="250" />
			<asp:RequiredFieldValidator ID="valSearchStyle" Runat="server" ControlToValidate="txtSearchStyle" Display="Dynamic" ResourceKey="valSearchStyle.ErrorMessage"
				CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSplitRange" runat="server" controlname="chkSplitRange" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSplitRange" Runat="server" />
		</td>
	</tr>
</TABLE>
