<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SendToFriendForm.ascx.vb" Inherits="Ventrian.PropertyAgent.SendToFriendForm" %>

<script type="text/javascript" src="http://www.plaxo.com/css/m/js/util.js"></script>
<script type="text/javascript" src="http://www.plaxo.com/css/m/js/basic.js"></script>
<script type="text/javascript" src="http://www.plaxo.com/css/m/js/abc_launcher.js"></script>
<script type="text/javascript"><!--
function onABCommComplete() {
    // OPTIONAL: do something here after the new data has been populated in your text area
    var emails = document.getElementById('<asp:literal id="litEmailTo1" runat="server" />').value;
    emails = emails.match(/<([^>]+)>/g).join(', ');
    emails = emails.replace(/[<>]/g,'');
    document.getElementById('<asp:literal id="litEmailTo2" runat="server" />').value = emails; 
}
//--></script>

<asp:PlaceHolder id="phSendToFriendForm" runat="server" Visible="True">
	<asp:Table id="tblSendToFriendForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table id="tblSendToFriendFormDesign" cellSpacing="2" cellPadding="2" summary="SendToFriendForm Design Table"
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
									<asp:label id="lblRecipientMessage" runat="server" enableviewstate="False" Visible="false" CssClass="Normal"></asp:label>
								</TD>
								<TD align="left" width="100%">
									<asp:PlaceHolder ID="phValue" Runat="server" />
								</TD>
							</TR>
						</ItemTemplate>
					</asp:repeater>
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
</asp:PlaceHolder>
