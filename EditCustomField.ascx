<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditCustomField.ascx.vb" Inherits="Ventrian.PropertyAgent.EditCustomField" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
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
<dnn:sectionhead id="dshCustomFieldDetails" cssclass="Head" runat="server" text="Custom Field Details" section="tblCustomFieldDetails"
	resourcekey="CustomFieldDetails" includerule="True"></dnn:sectionhead>
<TABLE id="tblCustomFieldDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Custom Field Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblCustomFieldDetailsHelp" cssclass="Normal" runat="server" resourcekey="CustomFieldDetailsDescription"
				enableviewstate="False"></asp:label></TD>
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
		<TD class="SubHead" noWrap width="150"><dnn:label id="plCaption" runat="server" resourcekey="Caption" suffix=":" controlname="txtCaption"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtCaption" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
			<asp:requiredfieldvalidator id="valCaption" cssclass="NormalRed" runat="server" resourcekey="valCaption" controltovalidate="txtCaption"
				errormessage="<br>You Must Enter a Valid Caption" display="Dynamic"></asp:requiredfieldvalidator>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plCaptionHelp" runat="server" resourcekey="CaptionHelp" suffix=":" controlname="txtCaptionHelp"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtCaptionHelp" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plFieldType" runat="server" resourcekey="FieldType" suffix=":" controlname="drpFieldType"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:dropdownlist id="drpFieldType" cssclass="NormalTextBox" runat="server" width="325" AutoPostBack="True"></asp:dropdownlist>
		</TD>
	</TR>
	<TR vAlign="top" runat="server" id="trFieldElements">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plFieldElements" runat="server" resourcekey="FieldElements" suffix=":" controlname="txtFieldElements"></dnn:label></TD>
		<TD align="left" width="100%">
		    <asp:RadioButtonList ID="lstFieldElementType" runat="server" CssClass="Normal" RepeatLayout="Table" RepeatDirection="Horizontal" AutoPostBack="true" />
			<asp:Panel ID="pnlFieldElements" runat="server">
			    <asp:textbox id="txtFieldElements" cssclass="NormalTextBox" runat="server" TextMode="MultiLine" Height="150" width="325"></asp:textbox>
			    <asp:requiredfieldvalidator id="valFieldElements" cssclass="NormalRed" runat="server" resourcekey="valFieldElements" controltovalidate="txtFieldElements"
				    errormessage="<br>You Must Enter a Valid Field Element" display="Dynamic"></asp:requiredfieldvalidator>
			</asp:Panel>
			<asp:Panel ID="pnlFieldElementsType" runat="server">
			    <asp:Repeater ID="rptFieldElementsType" runat="server">
			        <HeaderTemplate>
			            <table id="tblFieldElementTypes" cellpadding="0" cellspacing="0" width="325">
			        </HeaderTemplate>
			        <ItemTemplate>
			            <tr>
			                <td class="Normal" width="100"><%#Eval("NameIndented")%></td>
			                <td width="200"><asp:TextBox ID="txtFieldElementTypes" runat="server" CssClass="NormalTextBox" Width="225" /></td>
			            </tr>
			        </ItemTemplate>
			        <FooterTemplate>
			            </table>
			        </FooterTemplate>
			    </asp:Repeater>
			</asp:Panel>
			<asp:Panel ID="pnlFieldElementsDropDown" runat="server">
			    <table id="tblFieldElementDropDown" cellpadding="0" cellspacing="0" width="325">
			    <tr>
			        <td class="Normal" width="100">Custom Field</td>
			        <td width="225"><asp:DropDownList ID="drpCustomFieldsDropDown" runat="server" CssClass="NormalTextBox" DataTextField="Name" DataValueField="CustomFieldID" AutoPostBack="true" Width="225"></asp:DropDownList></td>
			    </tr>
			    </table>
			    <asp:Repeater ID="rptFieldElementDropDown" runat="server">
			        <HeaderTemplate>
			            <table id="tblFieldElementDropDown" cellpadding="0" cellspacing="0" width="325">
			        </HeaderTemplate>
			        <ItemTemplate>
			            <tr>
			                <td class="Normal" width="100"><%#Container.DataItem.ToString()%></td>
			                <td width="225"><asp:TextBox ID="txtFieldElementDropDown" runat="server" CssClass="NormalTextBox" Width="225" /></td>
			            </tr>
			        </ItemTemplate>
			        <FooterTemplate>
			            </table>
			        </FooterTemplate>
			    </asp:Repeater>
			</asp:Panel>
			<asp:Label ID="lblFieldElementHelp" Runat="server" ResourceKey="FieldElementHelp" CssClass="Normal" />
			<asp:Label ID="lblFieldElementHelpStandard" Runat="server" ResourceKey="FieldElementHelpStandard" CssClass="Normal" />
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plDefaultValue" runat="server" resourcekey="DefaultValue" suffix=":" controlname="txtDefaultValue"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtDefaultValue" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plPublished" runat="server" resourcekey="Published" suffix=":" controlname="chkPublished"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkPublished" cssclass="NormalTextBox" runat="server" Checked="True"></asp:checkbox>
		</TD>
	</TR>
    <tr id="trMaximumLength" runat="Server">
	    <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
	    <td class="SubHead" width="150"><dnn:label id="plMaximumLength" runat="server" controlname="txtMaximumLength" suffix=":"></dnn:label></td>
	    <td valign="bottom">
		    <asp:TextBox ID="txtMaximumLength" Runat="server" CssClass="NormalTextBox" width="250"
			    columns="10" maxlength="6" />
			<asp:CompareValidator ID="valMaximumLengthIsNumber" Runat="server" ControlToValidate="txtMaximumLength"
			    Display="Dynamic" ResourceKey="valMaximumLengthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
	    </td>
    </tr>
</TABLE>
<br />
<asp:PlaceHolder ID="phRequired" runat="Server">
<dnn:sectionhead id="dshRequiredDetails" cssclass="Head" runat="server" text="Required Details" section="tblRequiredDetails"
	resourcekey="RequiredDetails" includerule="True"></dnn:sectionhead>
<TABLE id="tblRequiredDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Required Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblRequiredDetails" cssclass="Normal" runat="server" resourcekey="RequiredDetailsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plRequired" runat="server" resourcekey="Required" suffix=":" controlname="chkRequired"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkRequired" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plValidationType" runat="server" resourcekey="ValidationType" suffix=":" controlname="drpValidationType"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:dropdownlist id="drpValidationType" cssclass="NormalTextBox" runat="server" width="325" AutoPostBack="true"></asp:dropdownlist>
		</TD>
	</TR>
	<tr valign="top" runat="server" id="trRegex">
		<td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" noWrap width="150"><dnn:label id="plRegex" runat="server" resourcekey="plRegex" suffix=":" controlname="txtRegex"></dnn:label></td>
		<td align="left" width="100%">
			<asp:textbox id="txtRegex" cssclass="NormalTextBox" runat="server" maxlength="500" columns="30"
				width="325"></asp:textbox>
		</td>
	</tr>
</table>
<br />
</asp:PlaceHolder>
<asp:PlaceHolder ID="phLabelDetails" runat="Server">
<dnn:sectionhead id="dshLabelDetails" cssclass="Head" runat="server" text="Label Details" section="tblLabelDetails"
	resourcekey="LabelDetails" includerule="True"></dnn:sectionhead>
<table id="tblLabelDetails" cellspacing="0" cellpadding="2" width="100%" summary="Label Details Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:label id="lblLabelDetailsDescription" cssclass="Normal" runat="server" resourcekey="LabelDetailsDescription"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr valign="top">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td align="left" width="100%" colspan="2">
			<dnn:texteditor id="txtLabelDetails" runat="server" height="400" width="450"></dnn:texteditor>
		</td>
	</tr>
</table>
<br />
</asp:PlaceHolder>
<asp:PlaceHolder ID="phSearch" runat="Server">
<dnn:sectionhead id="dshSearchDetails" cssclass="Head" runat="server" text="Search Details" section="tblSearchDetails"
	resourcekey="SearchDetails" includerule="True"></dnn:sectionhead>
<TABLE id="tblSearchDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Search Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="plSearchDetailsDescription" cssclass="Normal" runat="server" resourcekey="SearchDetailsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plSearchable" runat="server" resourcekey="Searchable" suffix=":" controlname="chkSearchable"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkSearchable" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plSearchType" runat="server" resourcekey="SearchType" suffix=":" controlname="drpSearchType"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:dropdownlist id="drpSearchType" cssclass="NormalTextBox" runat="server" width="325"></asp:dropdownlist>
		</TD>
	</TR>
	<TR vAlign="top" runat="server" id="trFieldElementsFrom">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plFieldElementsFrom" runat="server" resourcekey="FieldElementsFrom" suffix=":" controlname="txtFieldElementsFrom"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtFieldElementsFrom" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
			<asp:CustomValidator id="valFieldElementsFrom" runat="server" resourcekey="valFieldElementsFrom.ErrorMessage" CssClass="NormalRed"
						ErrorMessage="Field elements invalid, unable to match 'validation type'." Display="Dynamic"></asp:CustomValidator>
			<asp:Label ID="lblFieldElementsTo" Runat="server" ResourceKey="FieldElementRangeHelp" CssClass="Normal" />
		</TD>
	</TR>
	<TR vAlign="top" runat="server" id="trFieldElementsTo">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plFieldElementsTo" runat="server" resourcekey="FieldElementsTo" suffix=":" controlname="txtFieldElementsTo"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:textbox id="txtFieldElementsTo" cssclass="NormalTextBox" runat="server" maxlength="255" columns="30"
				width="325"></asp:textbox>
			<asp:CustomValidator id="valFieldElementsTo" runat="server" resourcekey="valFieldElementsTo.ErrorMessage" CssClass="NormalRed"
						ErrorMessage="Field elements invalid, unable to match 'validation type'." Display="Dynamic"></asp:CustomValidator>
			<asp:Label ID="lblFieldElementsFrom" Runat="server" ResourceKey="FieldElementRangeHelp" CssClass="Normal" />
		</TD>
	</TR>
	<tr vAlign="top" runat="server" id="trIncludeCount">
		<td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" noWrap width="150"><dnn:label id="plIncludeCount" runat="server" suffix=":" controlname="chkIncludeCount"></dnn:label></td>
		<td align="left" width="100%">
			<asp:checkbox id="chkIncludeCount" runat="server" Checked="True"></asp:checkbox>
		</td>
	</TR>
	<tr vAlign="top" runat="server" id="trHideZeroCount">
		<td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" noWrap width="150"><dnn:label id="plHideZeroCount" runat="server" suffix=":" controlname="chkHideZeroCount"></dnn:label></td>
		<td align="left" width="100%">
			<asp:checkbox id="chkHideZeroCount" runat="server" Checked="True"></asp:checkbox>
		</td>
	</tr>
</table>
<br />
</asp:PlaceHolder>
<asp:PlaceHolder ID="phDisplayDetails" runat="Server">
<dnn:sectionhead id="dshDisplayDetails" cssclass="Head" runat="server" text="Display Details" section="tblDisplayDetails"
	resourcekey="DisplayDetails" includerule="True"></dnn:sectionhead>
<TABLE id="tblDisplayDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Display Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblDisplayDetails" cssclass="Normal" runat="server" resourcekey="DisplayDetailsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR vAlign="top" runat="server" id="trSortable">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plSortable" runat="server" resourcekey="Sortable" suffix=":" controlname="chkSortable"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkSortable" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plFeatured" runat="server" resourcekey="Featured" suffix=":" controlname="chkFeatured"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkFeatured" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plListing" runat="server" resourcekey="Listing" suffix=":" controlname="chkListing"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkListing" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plManager" runat="server" resourcekey="Manager" suffix=":" controlname="chkManager"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkManager" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plCaptionHidden" runat="server" resourcekey="CaptionHidden" suffix=":" controlname="chkCaptionHidden"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkCaptionHidden" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plHidden" runat="server" resourcekey="Hidden" suffix=":" controlname="chkHidden"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkHidden" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" noWrap width="150"><dnn:label id="plLockDown" runat="server" resourcekey="LockDown" suffix=":" controlname="chkLockDown"></dnn:label></TD>
		<TD align="left" width="100%">
			<asp:checkbox id="chkLockDown" cssclass="NormalTextBox" runat="server"></asp:checkbox>
		</TD>
	</TR>		
</table>
</asp:PlaceHolder>
<br />
<dnn:sectionhead id="dshSecurityDetails" cssclass="Head" runat="server" text="Security Details" section="tblSecurityDetails"
	resourcekey="SecurityDetails" includerule="True"></dnn:sectionhead>
<table id="tblSecurityDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Security Details Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:label id="lblSecurityDetails" cssclass="Normal" runat="server" resourcekey="SecurityDetailsDescription"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr valign="top">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150" nowrap="true"><dnn:label id="plInheritSecurity" runat="server" suffix=":" controlname="chkInheritSecurity" /></td>
		<td align="left" width="100%">
			<asp:checkbox id="chkInheritSecurity" cssclass="NormalTextBox" runat="server" AutoPostBack="true" Checked="true"></asp:checkbox>
		</td>
	</tr>
	<tr valign="top" id="trEditRoles" runat="server">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150" nowrap="true"><dnn:label id="plEditRoles" runat="server" suffix=":" controlname="lstEditRoles" /></td>
		<td align="left" width="100%">
			<asp:CheckBoxList id="lstEditRoles" cssclass="NormalTextBox" runat="server" DataTextField="RoleName" DataValueField="RoleID" />
		</td>
	</tr>
</table>
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
