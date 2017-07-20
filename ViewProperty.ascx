<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewProperty.ascx.vb" Inherits="Ventrian.PropertyAgent.ViewProperty" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<asp:PlaceHolder ID="phLightbox" runat="server">
<script type="text/javascript">
jQuery(function() {
	jQuery('a[rel*=lightbox<%= GetPropertyID() %>]').lightBox({
		imageLoading: '<%= Me.ResolveUrl("images/lightbox/lightbox-ico-loading.gif") %>',
		imageBlank: '<%= Page.ResolveUrl("~/images/spacer.gif") %>',
		txtImage: '<%= GetLocalizedValue("Image") %>',
		txtOf: '<%= GetLocalizedValue("Of") %>',
		next: '<%= GetLocalizedValue("Next") %>',
		previous: '<%= GetLocalizedValue("Previous") %>',
		close: '<%= GetLocalizedValue("Close") %>'
	});
});
</script>
</asp:PlaceHolder>
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
<div class="Normal">
	<asp:PlaceHolder ID="phProperty" Runat="server" />
</div>
<script type="text/javascript">
function ReceiveServerData(arg)
{
}
</script>
