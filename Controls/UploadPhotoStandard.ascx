<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadPhotoStandard.ascx.vb" Inherits="Ventrian.PropertyAgent.Controls.UploadPhotoStandard" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<dnn:sectionhead id="dshUploadPhoto" cssclass="Head" runat="server" text="Upload Photo" section="tblUploadPhoto"
	resourcekey="UploadPhoto" includerule="True"></dnn:sectionhead>
<table id="tblUploadPhoto" cellspacing="0" cellpadding="2" width="100%" summary="Property Photos Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:label id="lblUploadPhotoHelp" cssclass="Normal" runat="server" resourcekey="UploadPhotoDescription"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr>
		<td width="25"></td>
		<td colspan="2">
			<table id="tblPropertyDetail" cellspacing="2" cellpadding="2" width="100%" summary="Property Design Table"
				border="0">
				<tr valign="top">
					<td class="SubHead" width="150"><dnn:label id="plTitle" runat="server" resourcekey="Title" suffix=":" controlname="txtTitle"></dnn:label></td>
					<td>
						<asp:TextBox ID="txtTitle" Runat="server" CssClass="NormalTextBox" Width="350px" />
					</td>
				</tr>
				<tr valign="top">
					<td class="SubHead" width="150"><dnn:label id="plPhoto" runat="server" resourcekey="Photo" suffix=":" controlname="txtPhoto"></dnn:label></td>
					<td>
						<input id="txtPhoto" type="file" size="50" name="txtPhoto" runat="server" />
						<asp:CustomValidator id="valPhoto" runat="server" resourcekey="Photo.ErrorMessage" CssClass="NormalRed"
							ErrorMessage="You Must Upload A File" Display="Dynamic" ValidationGroup="Upload"></asp:CustomValidator>
						<asp:CustomValidator id="valType" runat="server" resourcekey="ValidTypeSingle.ErrorMessage" CssClass="NormalRed"
							ErrorMessage="You must upload a file that is a JPG" Display="Dynamic" ValidationGroup="Upload"></asp:CustomValidator>
						<asp:CustomValidator id="valLimitExceeded" runat="server" resourcekey="valLimitExceeded.ErrorMessage" CssClass="NormalRed"
							ErrorMessage="You are not allowed to upload any more images to this item." Display="Dynamic" ValidationGroup="Upload"></asp:CustomValidator>
						<asp:Label ID="lblPhoto" Runat="server" CssClass="Normal" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<p align="center">
				<asp:linkbutton id="cmdUploadPhoto" BorderStyle="none" Text="Upload Photo" CssClass="CommandButton"
					runat="server" resourcekey="cmdUploadPhoto" ValidationGroup="Upload"></asp:linkbutton>
			</p>
		</td>
	</tr>
</table>
