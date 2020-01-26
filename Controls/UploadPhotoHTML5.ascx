<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadPhotoHTML5.ascx.vb" Inherits="Ventrian.PropertyAgent.Controls.UploadPhotoHTML5" %>
<%@ Register TagPrefix="Ventrian" Assembly="Ventrian.PropertyAgent" Namespace="Ventrian.PropertyAgent" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>


<script type="text/javascript">
    jQuery(document).ready(function() {
        //UploadPropertyAgentImages();
        $('#propertyagentfiles').fileuploader({
            fileMaxSize: <%= GetMaximumFileSize() %>,
            extensions: ['jpg', 'jpeg', 'png', 'gif'],
            upload: {
                url: "<%= GetUploadUrl() %>",
                data: {
                    PropertyID : '<%= GetPropertyID() %>', 
                    TabID : '<%= Request("TabID") %>', 
                    ModuleID : '<asp:literal id="litModuleID" runat="server" />', 
                    TabModuleID : '<asp:literal id="litTabModuleID" runat="server" />', 
                    Ticket : '<asp:literal id="litTicketID" runat="server" />', 
                    PropertyGuid : '<asp:literal id="litPropertyGuid" runat="server" />',
                    Category : '<%= GetCategory() %>'
                },
                type: 'POST',
                enctype: 'multipart/form-data',
                start: true,
                synchron: true,
                chunk: false,
                beforeSend: function (item, listEl, parentEl, newInputEl, inputEl) {
                    return true;
                },
                onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                },
                onError: function (item, listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus, errorThrown) {
                },
                onProgress: function (data, item, listEl, parentEl, newInputEl, inputEl) {
                },
                onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                    location.reload(true);
                }
            }
        });
    });
</script>

<span class="button">
    <asp:Label ID="lblSelectImages" runat="server" EnableViewState="False" /></span>
<dnn:sectionhead id="dshUploadPhoto" cssclass="Head" runat="server" text="Upload Photo" section="tblUploadPhoto"
    resourcekey="UploadPhoto" includerule="True"></dnn:sectionhead>

<table id="tblUploadPhoto" cellspacing="0" cellpadding="2" width="100%" summary="Property Photos Design Table"
    border="0" runat="server">
    <tr>
        <td colspan="3">
            <asp:Label ID="lblUploadPhotoHelp" CssClass="Normal" runat="server" resourcekey="UploadPhotoDescription2"
                EnableViewState="False"></asp:Label></td>
    </tr>
    <tr>
        <td width="25"></td>
        <td class="SubHead" width="150">
            <dnn:label id="plImage" runat="server" suffix=":"></dnn:label>
        </td>
        <td>


            <div id="pa_upload_container">
                <div>
                    <input id="propertyagentfiles" type="file" name="files">
                    <span id="spanButtonPlaceholder"></span>
                </div>
            </div>

                
        </td>
    </tr>

    <tr id="trImageCategories" runat="server">
        <td width="25"></td>
        <td class="SubHead" width="150">
            <dnn:label id="plImageCategories" runat="server" suffix=":" controlname="drpCategories"></dnn:label>
        </td>
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
            <asp:Label ID="lblUploadExternalPhotoHelp" CssClass="Normal" runat="server" resourcekey="UploadExternalPhotoDescription2"
                EnableViewState="False"></asp:Label></td>
    </tr>
    <tr>
        <td width="25"></td>
        <td class="SubHead" width="150">
            <dnn:label id="plExternalUrl" runat="server" suffix=":" controlname="txtExternalUrl"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtExternalUrl" runat="server" Width="225px" />
            <asp:LinkButton ID="cmdAttachPhoto" runat="server" ResourceKey="AttachPhoto" CssClass="CommandButton" />
        </td>
    </tr>
    <tr id="trImageCategoriesExternal" runat="server">
        <td width="25"></td>
        <td class="SubHead" width="150">
            <dnn:label id="plImageCategoriesExternal" runat="server" suffix=":" controlname="drpTypes"></dnn:label>
        </td>
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
<ventrian:refreshcontrol id="cmdRefreshPhotos" runat="server" text="Refresh Photos" visible="false" causesvalidation="false" />
