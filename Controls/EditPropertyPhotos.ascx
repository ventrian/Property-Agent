<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditPropertyPhotos.ascx.vb" Inherits="Ventrian.PropertyAgent.Controls.EditPropertyPhotos" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<dnn:sectionhead id="dshAssignedPhotos" cssclass="Head" runat="server" text="Assigned Photos" section="tblAssignedPhotos"
	resourcekey="AssignedPhotos" includerule="True"></dnn:sectionhead>
<table id="tblAssignedPhotos" cellspacing="0" cellpadding="2" width="100%" summary="Assigned Photos Design Table"
	border="0" runat="server">
	<tr>
		<td colspan="3" class="Normal">
			<asp:label id="lblAssignedPhotosHelp" cssclass="Normal" runat="server"
				enableviewstate="False"></asp:label>
			&nbsp;(<asp:LinkButton ID="cmdSortPhotos" runat="server" Text="Click here to sort photos" CssClass="CommandButton" />)
        </td>
	</tr>
	<tr>
		<td colspan="2">
            <asp:PlaceHolder ID="phEditing" runat="server">
		    <asp:DataList CellPadding="4" CellSpacing="0" ID="dlPhotos" Runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="PhotoID">
				<ItemStyle CssClass="Normal" HorizontalAlign="Center" />
				<ItemTemplate>
					<img src='<%# PropertyAgentBase.GetPhotoPathCropped(Container.DataItem) %>' alt="Photo"><br />
					<b><%# DataBinder.Eval(Container.DataItem, "Title") %></b><%#GetCategory(DataBinder.Eval(Container.DataItem, "Category").ToString())%>
					<br />
					<asp:ImageButton ID="btnEdit" Runat="server" CommandName="Edit" CausesValidation="False" ImageUrl="~/Images/edit.gif" />
					<asp:ImageButton ID="btnDelete" Runat="server" CommandName="Delete" CausesValidation="False" ImageUrl="~/Images/delete.gif" />
					<asp:ImageButton ID="btnDown" Runat="server" ImageUrl="~/Images/dn.gif" />
					<asp:ImageButton ID="btnUp" Runat="server" ImageUrl="~/Images/up.gif" />
					
				</ItemTemplate>
				<EditItemTemplate>
					<img src='<%# PropertyAgentBase.GetPhotoPathCropped(Container.DataItem) %>' alt="Photo"><br />
					<asp:TextBox id="txtTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title")  %>' Width="125" />
                    <asp:DropDownList id="drpCategory" runat="server" Width="125" />
                    <br />
                    <asp:LinkButton id="btnUpdate" runat="server" Text="Update" 
                        CommandName="Update" ResourceKey="cmdUpdate" CausesValidation="false" />
                    <asp:LinkButton id="btnCancel" runat="server" Text="Cancel" 
                        CommandName="Cancel" ResourceKey="cmdCancel" CausesValidation="false" />
				</EditItemTemplate>
			</asp:DataList>
			<div align="center"><asp:Label ID="lblNoPhotos" Runat="server" CssClass="Normal" ResourceKey="NoPhotos" /></div>
            </asp:PlaceHolder>
		</td>
	</tr>
</table>

<asp:PlaceHolder ID="phSorting" runat="server" Visible="false">
<asp:Label ID="lblSortingInstructions" runat="server" CssClass="Normal" Text="<br />Drag and drop the photos to sort..." />
<asp:Repeater ID="rptPhotos" runat="server">
    <HeaderTemplate><ul id="sortable" class="sortablePhoto"></HeaderTemplate>
    <ItemTemplate><li class="ui-state-default" id='<%# Eval("PhotoID") %>'><img src='<%# PropertyAgentBase.GetPhotoPathCropped(Container.DataItem) %>' alt="Photo"></li></ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>

<script type="text/javascript">

    jQuery(document).ready(function() {
        jQuery('#sortable').sortable({
            placeholder: 'ui-state-highlight',
            update: OnSortableUpdate
        });
        jQuery('#sortable').disableSelection();
        
        function OnSortableUpdate(event, ui) {
             var order = jQuery('#sortable').sortable('toArray').join(',').replace(/id_/gi, '');
             
             jQuery.ajax({
                    type: 'POST',
                    url: '<%= ResolveUrl("Sortable.asmx/UpdateItemsOrder") %>',
                    data: '{itemOrder: \'' + order + '\'}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
        }
    });
    
</script>

<div style="clear: both" />
</asp:PlaceHolder>
