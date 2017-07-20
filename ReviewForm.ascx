<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReviewForm.ascx.vb" Inherits="Ventrian.PropertyAgent.ReviewForm" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:Table id="tblReviewForm" runat="server">
	<asp:TableRow>
		<asp:TableCell>
            <table id="tblReviewFormDesign" cellspacing="2" cellpadding="2" summary="ReviewForm Design Table" border="0" width="100%">
                <asp:repeater id="rptDetails" Runat="server">
	                <ItemTemplate>
		                <tr valign="top">
			                <td class="SubHead" valign="middle" nowrap>
				                <label id="label" runat="server">
					                <asp:linkbutton id="cmdHelp" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
						                <asp:image id="imgHelp" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
					                </asp:linkbutton>
					                <asp:label id="lblLabel" runat="server" enableviewstate="False"></asp:label>
				                </label>
				                <asp:panel id="pnlHelp" runat="server" cssClass="Help" enableviewstate="False">
					                <asp:label id="lblHelp" runat="server" enableviewstate="False"></asp:label>
				                </asp:panel>
			                </td>
			                <td align="left" width="100%">
				                <asp:PlaceHolder ID="phValue" Runat="server" />
			                </td>
		                </tr>
	                </ItemTemplate>
                </asp:repeater>
                <tr valign="top">
	                <td colspan="2" align="center">
		                <asp:LinkButton id="cmdSubmit" runat="server" CssClass="CommandButton" BorderStyle="None">Submit</asp:LinkButton>
	                </td>
                </tr>
                <tr valign="top">
	                <td colspan="2" align="center">
		                <asp:label id="lblSubmitResults" runat="server" CssClass="NormalBold"></asp:label>
	                </td>
                </tr>
            </table>
        </asp:TableCell>
	</asp:TableRow>
</asp:Table>