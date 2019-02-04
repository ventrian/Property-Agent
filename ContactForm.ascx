<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ContactForm.ascx.vb" Inherits="Ventrian.PropertyAgent.ContactForm" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="Menulab.FlexCaptcha" Namespace="Menulab" TagPrefix="ml" %>
 
<asp:PlaceHolder id="phContactForm" runat="server" Visible="True">
	<asp:Table id="tblContactForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table id="tblContactFormDesign" cellSpacing="2" cellPadding="2" summary="ContactForm Design Table"
					border="0" width="100%">
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
			        <tr valign="top" id="trCaptcha" runat="Server">
				        <td class="SubHead">
					        <dnn:label id="plCaptcha" resourcekey="Captcha" runat="server" controlname="ctlCaptcha" suffix=":"></dnn:label>
				        </td>
				        <td align="left">
					        <ml:FlexCaptcha ID="mlFlexCaptcha" runat="server" />
				        </td>
			        </tr>
					<tr valign="top">
						<td colspan="2" align="center">
							<asp:LinkButton id="cmdSubmit" runat="server" CssClass="CommandButton" BorderStyle="None">Submit</asp:LinkButton><br />
							<asp:label id="lblRequireFields" runat="server" CssClass="Normal" ResourceKey="RequiredFields"></asp:label>
						</td>
					</tr>
					<tr valign="top">
						<td colspan="2" align="center">
							<asp:label id="lblSubmitResults" runat="server" CssClass="NormalRed"></asp:label>
						</td>
					</tr>
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</asp:PlaceHolder>
