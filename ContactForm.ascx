<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ContactForm.ascx.vb" Inherits="Ventrian.PropertyAgent.ContactForm" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="Menulab.FlexCaptcha" Namespace="Menulab" TagPrefix="ml" %>
<asp:Table id="tblContactForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>

<asp:PlaceHolder id="phContactForm" runat="server" Visible="True">
	<div class="row ContactFormBody">
		<asp:repeater id="rptDetails" Runat="server">
			<ItemTemplate>

		<div class="col-sm-4 ContactFormLabel">
			<label id="label" runat="server">
				<asp:linkbutton id="cmdHelp" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
				<asp:image id="imgHelp" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
				</asp:linkbutton>
				<asp:label id="lblLabel" runat="server" enableviewstate="False"></asp:label>
			</label>
			<asp:panel id="pnlHelp" runat="server" cssClass="Help" enableviewstate="False">
				<asp:label id="lblHelp" runat="server" enableviewstate="False"></asp:label>
			</asp:panel>
		</div>
		<div class="col-sm-8 ContactFormTextBox">
			<asp:PlaceHolder ID="phValue" Runat="server" />
		</div>
			</ItemTemplate>
		</asp:repeater>
	
	<table class="ContactFormCaptcha">
    <tr valign="top" id="trCaptcha" runat="Server">
		<td>
		<div>
			<dnn:label id="plCaptcha" resourcekey="Captcha" runat="server" controlname="ctlCaptcha" suffix=":"></dnn:label>
			<ml:FlexCaptcha ID="mlFlexCaptcha" runat="server" />
		</div>
		</td>
	</tr>
	</table>
	<div class="row ContactFormSubmit">
		<asp:LinkButton id="cmdSubmit" runat="server" CssClass="CommandButton" BorderStyle="None">Submit</asp:LinkButton><br />
		<asp:label id="lblRequireFields" runat="server" CssClass="Normal" ResourceKey="RequiredFields"></asp:label>
	</div>
	<div class="row ContactFormResult">
		<asp:label id="lblSubmitResults" runat="server" CssClass="NormalRed"></asp:label>
	</div>
	</div>
</asp:PlaceHolder>

			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>