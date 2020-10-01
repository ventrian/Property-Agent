<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LatestOptions.ascx.vb" Inherits="Ventrian.PropertyAgent.LatestOptions" %>
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
		<td class="SubHead" width="150">Enable ?PropertyID= in URL</td>
		<td valign="bottom">
			<asp:CheckBox ID="ckkPropertyIDinURL" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plBubbleFeatured" runat="server" controlname="chkBubbleFeatured" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkBubbleFeatured" Runat="server" />
		</td>
	</tr>
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
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plUserSortable" runat="server" controlname="chkUserSortable" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkUserSortable" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plSortFields" runat="server" controlname="lstSortFields" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:CheckBoxList ID="lstSortFields" runat="Server" Repeatdirection="Vertical" CssClass="NormalTextBox" RepeatLayout="Flow" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshFilterSettings" cssclass="Head" runat="server" text="Filter Settings" section="tblFilterSettings"
	resourcekey="FilterSettings" includerule="True"></dnn:sectionhead>
<TABLE id="tblFilterSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Filter Settings Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="2">
			<asp:label id="plFilterSettingsHelp" cssclass="Normal" runat="server" resourcekey="FilterSettingsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr align="top">
		<td class="SubHead" width="150"><dnn:label id="plType" runat="server" resourcekey="Type" suffix=":" controlname="drpTypes"></dnn:label></td>
		<td>
			<asp:dropdownlist id="drpTypes" DataValueField="PropertyTypeID" DataTextField="NameIndented" Runat="server"></asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plShowFeaturedOnly" resourcekey="ShowFeaturedOnly" runat="server" controlname="chkShowFeaturedOnly"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox id="chkShowFeaturedOnly" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plShowShortListOnly" resourcekey="ShowShortListOnly" runat="server" controlname="chkShowShortListOnly"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox id="chkShowShortListOnly" Runat="server" />
		</td>
	</tr>
	<tr align="top">
		<td class="SubHead" width="150" valign="top"><dnn:label id="plCustomFields" runat="server" resourcekey="CustomFields" suffix=":" controlname="drpCustomFields"></dnn:label></td>
		<td>
			<asp:dropdownlist id="drpCustomFields" DataValueField="CustomFieldID" DataTextField="Name" Runat="server" AutoPostBack="True"></asp:dropdownlist><br />
			<asp:TextBox id="txtCustomField" runat="server" width="250" cssclass="NormalTextBox" Visible="False"></asp:TextBox>
			<asp:DropDownList ID="drpCustomField" runat="Server" Width="250" CssClass="NormalTextBox" Visible="False"></asp:DropDownList>
			<asp:CheckBox ID="chkCustomField" runat="Server" CssClass="NormalTextBox" Visible="False"></asp:CheckBox>
			<asp:CheckBoxList ID="chkCustomFieldList" runat="Server" CssClass="NormalTextBox" Visible="False" RepeatDirection="Horizontal" RepeatColumns="3" RepeatLayout="Flow"></asp:CheckBoxList>
			<asp:RadioButtonList ID="rdoCustomFieldList" runat="Server" CssClass="NormalTextBox" Visible="False" RepeatDirection="Horizontal" RepeatColumns="3" RepeatLayout="Flow"></asp:RadioButtonList>
			<asp:LinkButton ID="cmdAddCustomField" runat="Server" CssClass="CommandButton" ResourceKey="cmdAddFilter" Visible="False" />
            <asp:datagrid id="grdCustomFilters" Border="0" CellPadding="4" CellSpacing="0" Width="300" AutoGenerateColumns="false"
	            runat="server" summary="Custom Filters Design Table" GridLines="None">
	            <Columns>
		            <asp:TemplateColumn ItemStyle-Width="150px" ItemStyle-CssClass="Normal">
			            <HeaderTemplate>
				            <asp:Label ID="lblName" runat="server" CssClass="NormalBold" ResourceKey="CustomField.Header" EnableViewState="False" />
			            </HeaderTemplate>
			            <ItemTemplate>
				            <%#GetCustomFieldName(Container.DataItem)%>
			            </ItemTemplate>
		            </asp:TemplateColumn>
		            <asp:TemplateColumn ItemStyle-Width="150px" ItemStyle-CssClass="Normal">
			            <HeaderTemplate>
				            <asp:Label ID="lblValue" runat="server" CssClass="NormalBold" ResourceKey="Value.Header" EnableViewState="False" />
			            </HeaderTemplate>
			            <ItemTemplate>
				            <%#GetCustomFieldValue(Container.DataSetIndex)%>
			            </ItemTemplate>
		            </asp:TemplateColumn>
		            <asp:ButtonColumn Text="Delete" ItemStyle-CssClass="CommandButton" CommandName="Delete" />
	            </Columns>
            </asp:datagrid>
		</td>
	</tr>
    <tr valign="top">
        <td class="SubHead" width="150">
	        <dnn:label id="plStartDate" runat="server" resourcekey="StartDate" suffix=":" controlname="txtStartDate"></dnn:label></td>
        <td align="left" width="325">
	        <asp:textbox id="txtStartDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
	        <asp:hyperlink id="cmdStartDate" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
	        <asp:comparevalidator id="valStartDate" cssclass="NormalRed" runat="server" controltovalidate="txtStartDate"
		        errormessage="<br>Invalid start date!" operator="DataTypeCheck" type="Date" display="Dynamic" ResourceKey="valStartDate.ErrorMessage"></asp:comparevalidator>
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead" width="150">
	        <dnn:label id="plMinAge" runat="server" resourcekey="MinAge" suffix=":" controlname="txtMinAge"></dnn:label></td>
        <td align="left" width="325">
	        <asp:textbox id="txtMinAge" Runat="server" Width="50" CssClass="NormalTextBox"></asp:textbox>&nbsp;<asp:Label ID="lblMinAge" Runat="server" EnableViewState="False" ResourceKey="MinAge2.Help"
		        CssClass="Normal" />
	        <asp:CompareValidator id="valMinAgeType" runat="server" ResourceKey="valMinAgeType.ErrorMessage" ErrorMessage="<br>* Must be a Number"
		        Display="Dynamic" ControlToValidate="txtMinAge" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead" width="150">
	        <dnn:label id="plMaxAge" runat="server" resourcekey="MaxAge" suffix=":" controlname="txtMaxAge"></dnn:label></td>
        <td align="left" width="325">
	        <asp:textbox id="txtMaxAge" Runat="server" Width="50" CssClass="NormalTextBox"></asp:textbox>&nbsp;<asp:Label ID="lblMaxAge" Runat="server" EnableViewState="False" ResourceKey="MaxAge2.Help"
		        CssClass="Normal" />
	        <asp:CompareValidator id="valMaxAgeType" runat="server" ResourceKey="valMaxAgeType.ErrorMessage" ErrorMessage="<br>* Must be a Number"
		        Display="Dynamic" ControlToValidate="txtMaxAge" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
        </td>
    </tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plUserFilter" runat="server" controlname="lstUser" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstUserFilter" Runat="server" CssClass="Normal" RepeatDirection="Vertical"
				RepeatLayout="Flow" AutoPostBack="True" />
		</td>
	</tr>
	<tr runat="server" id="trOwner" visible="false">
		<td class="SubHead" width="150"><dnn:label id="plSpecificUser" runat="server" controlname="lstUser" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:dropdownlist id="drpSpecificUser" runat="server" Width="300px" DataTextField="FullName" DataValueField="UserID"></asp:dropdownlist>
		</td>
	</tr>
    <tr runat="server" id="trParameter" visible="false">
        <td class="SubHead" width="150">
	        <dnn:label id="plUserParameter" runat="server" suffix=":" controlname="txtUserParameter"></dnn:label></td>
        <td align="left" width="325">
	        <asp:textbox id="txtUserParameter" Runat="server" Width="50" CssClass="NormalTextBox"></asp:textbox>
        </td>
    </tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plShowRelated" resourcekey="plShowRelated" runat="server" controlname="chkShowRelated"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox id="chkShowRelated" Runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plRelatedCustomFields" resourcekey="plRelatedCustomFields" runat="server" controlname="drpRelatedCustomFields"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList ID="drpRelatedCustomFields" runat="Server" Width="250" DataValueField="CustomFieldID" DataTextField="Name"></asp:DropDownList>
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshLayoutSettings" cssclass="Head" runat="server" text="Layout Settings" section="tblLayoutSettings"
	resourcekey="LayoutSettings" includerule="True"></dnn:sectionhead>
<TABLE id="tblLayoutSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Layout Settings Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="2">
			<asp:label id="lblLayoutSettingsHelp" cssclass="Normal" runat="server" resourcekey="LayoutSettingsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plMaxNumber" runat="server" controlname="txtMaxNumber" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMaxNumber" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valMaxNumber" Runat="server" ControlToValidate="txtMaxNumber" Display="Dynamic"
				ResourceKey="valMaxNumber.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valMaxNumberIsNumber" Runat="server" ControlToValidate="txtMaxNumber" Display="Dynamic"
				ResourceKey="valMaxNumberIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
    <tr>
        <td class="SubHead" width="150"><dnn:label id="plEnablePager" resourcekey="EnablePager" runat="server"
	            controlname="chkEnablePager"></dnn:label></td>
        <td valign="top"><asp:checkbox id="chkEnablePager" Runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
    </tr>
    <tr vAlign="top">
        <TD class="SubHead" width="150">
            <dnn:label id="plPageSize" runat="server" resourcekey="PageSize" suffix=":" controlname="txtPageSize"></dnn:label></TD>
        <TD align="left" width="325">
            <asp:textbox id="txtPageSize" Runat="server" Width="50" CssClass="NormalTextBox">10</asp:textbox>
            <asp:RequiredFieldValidator id="valPageSize" runat="server" ResourceKey="valPageSize.ErrorMessage" ErrorMessage="<br>* Required"
                Display="Dynamic" ControlToValidate="txtPageSize" CssClass="NormalRed"></asp:RequiredFieldValidator>
            <asp:CompareValidator id="valPageSizeIsNumber" runat="server" ResourceKey="valPageSizeIsNumber.ErrorMessage" ErrorMessage="<br>* Must be a Number"
                Display="Dynamic" ControlToValidate="txtPageSize" Type="Integer" Operator="DataTypeCheck" CssClass="NormalRed"></asp:CompareValidator>
        </TD>
    </tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plLayoutType" runat="server" controlname="lstLayoutType" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstLayoutType" Runat="server" CssClass="Normal" RepeatDirection="Horizontal"
				RepeatLayout="Flow" AutoPostBack="True" />
		</td>
	</tr>
	<tr id="trItemsPerRow" runat="server">
		<td class="SubHead" width="150"><dnn:label id="plItemsPerRow" runat="server" controlname="txtItemsPerRow" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valItemsPerRow" Runat="server" ControlToValidate="txtItemsPerRow" Display="Dynamic"
				ResourceKey="valItemsPerRow.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valItemsPerRowIsNumber" Runat="server" ControlToValidate="txtItemsPerRow" Display="Dynamic"
				ResourceKey="valItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
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
	<TR id="trHeader" runat="Server">
		<TD class="SubHead" width="150">
			<dnn:label id="plHeader" controlname="txtHeader" suffix=":" runat="server"></dnn:label></TD>
		<TD vAlign="bottom">
			<asp:TextBox id="txtHeader" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></TD>
	</TR>
	<TR id="trItem" runat="Server">
		<TD class="SubHead" width="150">
			<dnn:label id="plItem" controlname="txtItem" suffix=":" runat="server"></dnn:label></TD>
		<TD vAlign="bottom">
			<asp:TextBox id="txtItem" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></TD>
	</TR>
	<TR id="trFooter" runat="Server">
		<TD class="SubHead" width="150">
			<dnn:label id="plFooter" controlname="txtFooter" suffix=":" runat="server"></dnn:label></TD>
		<TD vAlign="bottom">
			<asp:TextBox id="txtFooter" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></TD>
	</TR>
	<TR id="trEmpty" runat="Server">
		<TD class="SubHead" width="150">
			<dnn:label id="plEmpty" controlname="txtEmpty" suffix=":" runat="server"></dnn:label></TD>
		<TD vAlign="bottom">
			<asp:TextBox id="txtEmpty" runat="server" width="300" rows="10" textmode="MultiLine" cssclass="NormalTextBox"></asp:TextBox></TD>
	</TR>
</TABLE>