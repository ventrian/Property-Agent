<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Search.ascx.vb" Inherits="Ventrian.PropertyAgent.Search" %>
<%@ Register Assembly="Ventrian.PropertyAgent" Namespace="Ventrian.PropertyAgent" TagPrefix="cc1" %>
<asp:PlaceHolder ID="phSearch" runat="server" />

<asp:panel DefaultButton="cmdSearch" runat="server" ID="pnlSearch">
<table id="tblPropertySearch" cellSpacing="2" cellPadding="2" width="100%" summary="Property Design Table"
	border="0">
<TR vAlign="top" runat="server" id="trWildcard">
	<TD class="SubHead" width="150" valign="middle">
		<label id="Label2" runat="server">
			<asp:linkbutton id="cmdHelpSearch" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
				<asp:image id="imgHelpSearch" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
			</asp:linkbutton>
			<asp:label id="lblLabelSearch" runat="server" enableviewstate="False"></asp:label>
		</label>
		<asp:panel id="pnlHelpSearch" runat="server" cssClass="Help" enableviewstate="False">
			<asp:label id="lblHelpSearch" runat="server" enableviewstate="False"></asp:label>
		</asp:panel>
	</TD>
	<TD align="left">
		<asp:TextBox ID="txtWildcard" runat="server" CssClass="NormalTextBox" />
	</TD>
</TR>
<TR vAlign="top" runat="server" id="trTypes">
	<TD class="SubHead" width="150" valign="middle">
		<label id="lblTypes" runat="server">
			<asp:linkbutton id="cmdHelpTypes" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
				<asp:image id="imgHelpTypes" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
			</asp:linkbutton>
			<asp:label id="lblLabelTypes" runat="server" enableviewstate="False"></asp:label>
		</label>
		<asp:panel id="pnlHelpTypes" runat="server" cssClass="Help" enableviewstate="False">
			<asp:label id="lblHelpTypes" runat="server" enableviewstate="False"></asp:label>
		</asp:panel>
	</TD>
	<TD align="left">
		<asp:DropDownList ID="drpPropertyTypes" runat="server" CssClass="NormalTextBox" DataTextField="NameIndentedWithCount" DataValueField="PropertyTypeID" />
	</TD>
</TR>
<TR vAlign="top" runat="server" id="trLocation">
	<TD class="SubHead" width="150" valign="middle">
		<label id="Label1" runat="server">
			<asp:linkbutton id="cmdHelpLocation" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
				<asp:image id="imgHelpLocation" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
			</asp:linkbutton>
			<asp:label id="lblLabelLocation" runat="server" enableviewstate="False"></asp:label>
		</label>
		<asp:panel id="pnlHelpLocation" runat="server" cssClass="Help" enableviewstate="False">
			<asp:label id="lblHelpLocation" runat="server" enableviewstate="False"></asp:label>
		</asp:panel>
	</TD>
	<TD align="left">
		<asp:TextBox ID="txtLocation" runat="server" CssClass="NormalTextBox" />
	</TD>
</TR>
<TR id="trBrokers" vAlign="top" runat="server">
	<TD class="SubHead" vAlign="middle" width="150">
	        <label id="lblBrokers" runat="server">
	            <asp:linkbutton id="cmdHelpBrokers" tabIndex="-1" runat="server" enableviewstate="False" CausesValidation="False">
				<asp:image id="imgHelpBrokers" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
			</asp:linkbutton>
			<asp:label id="lblLabelBrokers" runat="server" enableviewstate="False"></asp:label>
		</label>
		<asp:panel id="pnlHelpBrokers" runat="server" enableviewstate="False" cssClass="Help">
			<asp:label id="lblHelpBrokers" runat="server" enableviewstate="False"></asp:label>
		</asp:panel>
	</TD>
	<TD align="left">
	    <asp:dropdownlist id="drpPropertyBrokers" runat="server" CssClass="NormalTextBox" AutoPostBack="True"></asp:dropdownlist>
	</TD>
</TR>
<TR id="trAgents" vAlign="top" runat="server">
	<TD class="SubHead" vAlign="middle" width="150">
	        <label id="lblAgents" runat="server">
	            <asp:linkbutton id="cmdHelpAgents" tabIndex="-1" runat="server" enableviewstate="False" CausesValidation="False">
				<asp:image id="imgHelpAgents" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
			</asp:linkbutton>
			<asp:label id="lblLabelAgents" runat="server" enableviewstate="False"></asp:label>
		</label>
		<asp:panel id="pnlHelpAgents" runat="server" enableviewstate="False" cssClass="Help">
			<asp:label id="lblHelpAgents" runat="server" enableviewstate="False"></asp:label>
		</asp:panel>
	</TD>
	<TD align="left">
	    <asp:dropdownlist id="drpPropertyAgents" runat="server" CssClass="NormalTextBox"></asp:dropdownlist>
	</TD>
</TR>
<asp:repeater id="rptDetails" Runat="server">
	<ItemTemplate>
		<TR vAlign="top">
			<TD class="SubHead" width="150" valign="middle">
				<label id=label runat="server">
					<asp:linkbutton id=cmdHelp tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
						<asp:image id="imgHelp" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
					</asp:linkbutton>
					<asp:label id=lblLabel runat="server" enableviewstate="False"></asp:label>
				</label>
				<asp:panel id=pnlHelp runat="server" cssClass="Help" enableviewstate="False">
					<asp:label id=lblHelp runat="server" enableviewstate="False"></asp:label>
				</asp:panel>
			</TD>
			<TD align="left">
				<asp:PlaceHolder ID="phValue" Runat="server" />
			</TD>
		</TR>
	</ItemTemplate>
</asp:repeater>
</table>
</asp:panel>
<p align="center">
	<cc1:LinkButtonDefault id="cmdSearch" runat="server" cssclass="CommandButton" text="Search"
		borderstyle="none" />
</p>