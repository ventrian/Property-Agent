<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditBrokers.ascx.vb" Inherits="Ventrian.PropertyAgent.EditBrokers" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
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
<dnn:sectionhead id="dshEditBrokers" cssclass="Head" runat="server" text="Edit Brokers" section="tblEditBrokers"
	includerule="True"></dnn:sectionhead>
<TABLE id="tblEditBrokers" cellSpacing="0" cellPadding="2" width="100%" summary="Edit Brokers Design Table"
	border="0" runat="server">
<TR>
	<TD colSpan="3">
		<asp:label id="lblEditBrokersHelp" cssclass="Normal" runat="server" enableviewstate="False"></asp:label><br /><br /></TD>
</TR>
<TR>
    <TD colspan="3">
        <asp:Panel id="pnlAgents" runat="server">
        <table id="tblBrokerAgents" cellSpacing="0" cellPadding="2" width="100%" border="0" runat="server">
	    <TR>
		    <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		    <TD class="SubHead" noWrap width="90"><dnn:label id="plBrokers" runat="server" suffix=":" controlname="drpBrokers"></dnn:label></TD>
		    <TD align="left" width="100%">
		        <asp:dropdownlist id="drpBrokers" runat="server" CssClass="NormalTextBox" AutoPostBack="True" Width="200px" DataTextField="DisplayName" DataValueField="UserID"></asp:dropdownlist>
                <asp:Label ID="lblNoBrokers" runat="server" CssClass="Normal" EnableViewState="False" Visible="False"></asp:Label>
            </TD>
	    </TR>
	    <TR>
		    <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		    <TD class="SubHead" noWrap width="90"><dnn:label id="plOwners" runat="server" resourcekey="Owners" suffix=":" controlname="Owners"></dnn:label></TD>
		    <TD align="left" width="100%">
                <table cellspacing="0" cellpadding="2" border="0">
	                <tr>
		                <td align="center" class="NormalBold"><asp:Label id="lblAvailable" resourcekey="Available" runat="server" enableviewstate="False">Available</asp:Label></td>
		                <td align="center">&nbsp;</td>
		                <td align="center" class="NormalBold"><asp:Label id="lblSelected" resourcekey="Selected" runat="server" enableviewstate="False">Selected</asp:Label></td>
	                </tr>
	                <tr>
		                <td align="center" valign="top">
			                <asp:ListBox ID="lstAvailable" runat="server" class="NormalTextBox" SelectionMode="Multiple" Width="200" Height="150"></asp:ListBox>
		                </td>
		                <td align="center" valign="middle">
			                <table cellpadding="0" cellspacing="0" border="0">
				                <tr>
					                <td align="center" valign="top">
					                <asp:ImageButton id="cmdAdd" runat="server" cssclass="CommandButton"  enableviewstate="False" ImageUrl="~/images/rt.gif"/>
                                    </td>
				                </tr>
				                <tr>				                
					                <td align="center" valign="top">
					                  <br />
					                  <asp:ImageButton id="cmdRemove" runat="server" cssclass="CommandButton"  enableviewstate="False" ImageUrl="~/images/lt.gif"/>
					                </td>
				                </tr>
				                <tr>
					                <td>&nbsp;</td>
				                </tr>
			                </table>
		                </td>
		                <td align="center" valign="top">
			                <asp:listbox runat="server" ID="lstSelected" class="NormalTextBox" SelectionMode="Multiple" Width="200" Height="150"></asp:listbox>
		                </td>
	                </tr>
                </table>
                			
		    </TD>
	    </TR>
	    <tr>
	        <td colspan="3" align="center">
            <br /><asp:linkbutton id="cmdReturn" resourcekey="cmdReturn" runat="server" cssclass="CommandButton" text="Return" causesvalidation="False" borderstyle="none" />
                &nbsp;</td>
        </tr>	
	    </table>
        </asp:Panel>
	</TD>
</TR>		
</TABLE>
