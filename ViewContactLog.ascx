<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewContactLog.ascx.vb" Inherits="Ventrian.PropertyAgent.ViewContactLog" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
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
		<td height="5" colspan="2"></td>
	</tr>
</table>
<asp:Label ID="lblViewContactLog" runat="server" CssClass="SubHead" ResourceKey="ViewContactLog" /><br />

<table>
<tr align="top">
	<td class="SubHead" width="150"><dnn:label id="plStartDate" runat="server" resourcekey="StartDate" suffix=":" controlname="txtPublishDate"></dnn:label></td>
	<td>
		<asp:textbox id="txtStartDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
		<asp:hyperlink id="cmdStartDate" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
		<asp:comparevalidator id="valStartDate" cssclass="NormalRed" runat="server" controltovalidate="txtStartDate"
			errormessage="<br>Invalid start date!" operator="DataTypeCheck" type="Date" display="None" ResourceKey="valStartDate.ErrorMessage" SetFocusOnError="True"></asp:comparevalidator>
	</td>
</tr>
<tr align="top">
	<td class="SubHead" width="150"><dnn:label id="plEndDate" runat="server" resourcekey="EndDate" suffix=":" controlname="txtEndDate"></dnn:label></td>
	<td>
		<asp:textbox id="txtEndDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
		<asp:hyperlink id="cmdEndDate" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
		<asp:comparevalidator id="valEndDate" cssclass="NormalRed" runat="server" controltovalidate="txtEndDate"
			errormessage="<br>Invalid end date!" operator="DataTypeCheck" type="Date" display="None" ResourceKey="valEndDate.ErrorMessage" SetFocusOnError="True"></asp:comparevalidator>
	</td>
</tr>
<tr>
    <td></td>
    <td><asp:LinkButton ID="cmdSearch" runat="server" ResourceKey="cmdSearch" CssClass="CommandButton" /></td>
</tr>
</table>



<asp:datagrid id="grdContactLog" Border="0" CellPadding="4" width="100%" AutoGenerateColumns="true"
	runat="server" summary="Contact Log" BorderStyle="None" BorderWidth="0px"
	GridLines="None">
</asp:datagrid>
<div align="center"><asp:LinkButton id="cmdExport" runat="server" 
        onclick="cmdExport_Click" Text="Export to Excel" CssClass="Normal" Visible="false" /><asp:Label ID="lblNoContactLogs" Runat="server" CssClass="Normal" ResourceKey="NoContactLogs" EnableViewState="False" Visible="False" /></div>
