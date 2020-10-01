<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadPhotoSWF.ascx.vb" Inherits="Ventrian.PropertyAgent.Controls.UploadPhotoSWF" %>
<%@ Register TagPrefix="Ventrian" Assembly="Ventrian.PropertyAgent" Namespace="Ventrian.PropertyAgent" %>

<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<script type="text/javascript" src='<%= ResolveUrl("~/DesktopModules/PropertyAgent/JS/SWFUpload/swfupload.01.07.33.js") %>'></script>
<script type="text/javascript" src='<%= ResolveUrl("~/DesktopModules/PropertyAgent/JS/SWFUpload/handlers.01.08.05.js") %>'></script>

<script type="text/javascript">
	jQuery(document).ready(function() {
		UploadPropertyAgentImages();
	});
	
	var swfu;
	
	function UploadPropertyAgentImages() {
		swfu = new SWFUpload({
			// Backend Settings
			upload_url: "<%= GetUploadUrl() %>",	// Relative to the SWF file
			post_params: {
				"PropertyID" : '<%= GetPropertyID() %>', 
				"TabID" : '<%= Request("TabID") %>', 
				"ModuleID" : '<asp:literal id="litModuleID" runat="server" />', 
				"TabModuleID" : '<asp:literal id="litTabModuleID" runat="server" />', 
				"Ticket" : '<asp:literal id="litTicketID" runat="server" />', 
				"PropertyGuid" : '<asp:literal id="litPropertyGuid" runat="server" />',
				"Category" : '<%= GetCategory() %>'
			},	// Relative to the SWF file
			// File Upload Settings
			file_size_limit : "<%= GetMaximumFileSize() %>",	
			file_types : "*.gif;*.jpg;*.png",
			file_types_description : "Common Web Image Formats (gif, jpg, png)",
			file_upload_limit : "<%= GetUploadLimit() %>",    // Zero means unlimited
			// Event Handler Settings - these functions as defined in Handlers.js
			//  The handlers are not part of SWFUpload but are part of my website and control how
			//  my website reacts to the SWFUpload events.
			file_queue_error_handler : fileQueueError,
			file_dialog_complete_handler : fileDialogComplete,
			upload_progress_handler : uploadProgress,
			upload_error_handler : uploadError,
			upload_success_handler : uploadSuccess,
			upload_complete_handler : uploadComplete,
			// Button Settings
			button_image_url : '<%= ResolveUrl("~/DesktopModules/PropertyAgent/Images/SWFUpload/XPButtonNoText_160x22.png") %>',	// Relative to the SWF file
			button_placeholder_id : "spanButtonPlaceholder",
			button_width: 160,
			button_height: 22,
			button_text : '<span class="button"><asp:Label ID="lblSelectImages" Runat="server" EnableViewState="False" /></span>',
			button_text_style : '.button { font-family: Tahoma,Arial,Helvetica; font-size: 11px; font-weight:bold; text-align: center; }',
			button_text_top_padding: 2,
			button_text_left_padding: 5,
			// Flash Settings
			flash_url : '<%= ResolveUrl("~/DesktopModules/PropertyAgent/JS/SWFUpload/swfupload.01.07.33.swf") %>',	// Relative to this file
			custom_settings : {
				upload_target : "pa_progress_container",
				image_path : '<%= ResolveUrl("~/DesktopModules/PropertyAgent/Images/SWFUpload/") %>'
			},
			
			// Debug Settings
			debug: false
		});
	}
</script>
<dnn:sectionhead id="dshUploadPhoto" cssclass="Head" runat="server" text="Upload Photo" section="tblUploadPhoto"
	resourcekey="UploadPhoto" includerule="True"></dnn:sectionhead>
<table id="tblUploadPhoto" cellspacing="0" cellpadding="2" width="100%" summary="Property Photos Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:label id="lblUploadPhotoHelp" cssclass="Normal" runat="server" resourcekey="UploadPhotoDescription2"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr>
		<td width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plImage" runat="server" suffix=":"></dnn:label></td>
		<td>
			<div id="pa_upload_container">
				<div>
					<span id="spanButtonPlaceholder"></span>
				</div>
			</div>
		</td>
	</tr>
	<tr id="trImageCategories" runat="server">
		<td width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plImageCategories" runat="server" suffix=":" controlname="drpCategories"></dnn:label></td>
		<td>
			<asp:DropDownList ID="drpCategories" runat="server" CssClass="NormalTextBox" Width="225px" AutoPostBack="true" />
		</td>
	</tr>
	<tr>
		<td width="25"></td>
		<td colspan="2">
			 <div id="pa_progress_container" class="Normal" style="margin-top: 10px;"></div>
		</td>
	</tr>
</table>    
<br />
<dnn:sectionhead id="dshExternalPhoto" cssclass="Head" runat="server" text="Upload External Photo" section="tblUploadExternalPhoto"
	resourcekey="UploadExternalPhoto" includerule="True"></dnn:sectionhead>
<table id="tblUploadExternalPhoto" cellspacing="0" cellpadding="2" width="100%" summary="Property Photos Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3">
			<asp:label id="lblUploadExternalPhotoHelp" cssclass="Normal" runat="server" resourcekey="UploadExternalPhotoDescription2"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr>
		<td width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plExternalUrl" runat="server" suffix=":" controlname="txtExternalUrl"></dnn:label></td>
		<td>
			<asp:TextBox ID="txtExternalUrl" runat="server" Width="225px" />
			<asp:LinkButton ID="cmdAttachPhoto" runat="server" ResourceKey="AttachPhoto" CssClass="CommandButton"  />
		</td>
	</tr>
	<tr id="trImageCategoriesExternal" runat="server">
		<td width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plImageCategoriesExternal" runat="server" suffix=":" controlname="drpTypes"></dnn:label></td>
		<td>
			<asp:DropDownList ID="drpCategoriesExternal" runat="server" CssClass="NormalTextBox" Width="225px" />
		</td>
	</tr>
</table>    
<br />
<script type="text/javascript">
	function agentQueueComplete() {
		eval("<%= GetPostBackReference() %>");
	}
</script>
<Ventrian:RefreshControl ID="cmdRefreshPhotos" runat="server" Text="Refresh Photos" Visible="false" CausesValidation="false" />