<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CommentForm.ascx.vb" Inherits="Ventrian.PropertyAgent.CommentForm" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:Table id="tblCommentForm" runat="server">
	<asp:TableRow>
		<asp:TableCell>
            <table id="tblCommentFormDesign" cellSpacing="2" cellPadding="2" summary="CommentForm Design Table"
                border="0" width="100%">
                <asp:repeater id="rptDetails" Runat="server">
	                <ItemTemplate>
		                <TR vAlign="top">
			                <TD class="SubHead" valign="middle" nowrap>
				                <label id="label" runat="server">
					                <asp:linkbutton id="cmdHelp" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
						                <asp:image id="imgHelp" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
					                </asp:linkbutton>
					                <asp:label id="lblLabel" runat="server" enableviewstate="False"></asp:label>
				                </label>
				                <asp:panel id="pnlHelp" runat="server" cssClass="Help" enableviewstate="False">
					                <asp:label id="lblHelp" runat="server" enableviewstate="False"></asp:label>
				                </asp:panel>
			                </TD>
			                <TD align="left" width="100%">
				                <asp:PlaceHolder ID="phValue" Runat="server" />
			                </TD>
		                </TR>
	                </ItemTemplate>
                </asp:repeater>
			    <tr valign="top" id="trCaptcha" runat="Server">
				    <td class="SubHead" nowrap>
					    <dnn:label id="plCaptcha" resourcekey="Captcha" runat="server" controlname="ctlCaptcha" suffix=":"></dnn:label>
				    </td>
				    <td align="left">
					    <dnn:captchacontrol id="ctlCaptcha" captchawidth="130" captchaheight="40" cssclass="Normal" runat="server" errorstyle-cssclass="NormalRed" />
				    </td>
			    </tr>
                <tr vAlign="top">
	                <td colspan="2" align="center">
		                <asp:LinkButton id="cmdSubmit" runat="server" CssClass="CommandButton" BorderStyle="None">Submit</asp:LinkButton>
	                </td>
                </tr>
                <tr vAlign="top">
	                <td colspan="2" align="center">
		                <asp:label id="lblSubmitResults" runat="server" CssClass="NormalRed"></asp:label>
	                </td>
                </tr>
            </table>
        </asp:TableCell>
	</asp:TableRow>
</asp:Table>