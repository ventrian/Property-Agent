<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditPhotos.ascx.vb" Inherits="Ventrian.PropertyAgent.EditPhotos" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register src="Controls/EditPropertyPhotos.ascx" tagname="EditPropertyPhotos" tagprefix="uc1" %>
<%@ Register src="Controls/UploadPhotoStandard.ascx" tagname="UploadPhotoStandard" tagprefix="uc1" %>
<%@ Register src="Controls/UploadPhotoSWF.ascx" tagname="UploadPhotoSWF" tagprefix="uc1" %>
<%@ Register src="Controls/UploadPhotoHTML5.ascx" tagname="UploadPhotoHTML5" tagprefix="uc1" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="left">
			<asp:Repeater ID="rptBreadCrumbs" Runat="server">
				<ItemTemplate>
					<a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold">
						<%# DataBinder.Eval(Container.DataItem, "Caption") %>
					</a>
				</ItemTemplate>
				<SeparatorTemplate>
					&nbsp;&#187;&nbsp;
				</SeparatorTemplate>
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
<uc1:UploadPhotoHTML5 ID="UploadPhotoHTML51" runat="server" />
<uc1:UploadPhotoSWF ID="UploadPhotoSWF1" runat="server" />
<uc1:UploadPhotoStandard ID="UploadPhotoStandard1" runat="server" Visible="false" />
<uc1:EditPropertyPhotos ID="EditPropertyPhotos1" runat="server" />
<p align="center">
	<asp:linkbutton id="cmdReturnToEditProperty" BorderStyle="none" Text="Return to Edit Property" CssClass="CommandButton"
		runat="server"></asp:linkbutton>
</p>