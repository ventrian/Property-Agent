<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditPermissions.ascx.vb" Inherits="Ventrian.PropertyAgent.EditPermissions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="left" valign="middle">
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
<dnn:sectionhead id="dshPermissionSettings" cssclass="Head" runat="server" text="Permission Settings"
	section="tblPermissionSettings" resourcekey="PermissionSettings" includerule="True"></dnn:sectionhead>
<TABLE id="tblPermissionSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Permission Settings Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblPermissionSettingsHelp" cssclass="Normal" runat="server" resourcekey="PermissionSettingsDescription"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		<td class="SubHead" width="150"><dnn:label id="plPermissions" runat="server" controlname="grdPermissions" suffix=":"></dnn:label></td>
		<td></td>
	</tr>
	<tr>
		<td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td valign="bottom" align="left" colspan="2">
			<asp:DataGrid ID="grdPermissions" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
				ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
				HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
				BorderStyle="None" DataKeyField="Value">
				<Columns>
					<asp:TemplateColumn>
						<ItemStyle HorizontalAlign="Left" />
						<HeaderTemplate>
						</HeaderTemplate>
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "Text") %>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp;
							<asp:Label ID="lblSubmit" Runat="server" EnableViewState="False" ResourceKey="Submit" />&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkSubmit" Runat="server" />
							<asp:TextBox ID="txtAmount" Runat="server" Width="25px" CssClass="Normal" />
							<asp:Literal ID="litAmount" Runat="server">*</asp:Literal>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp;
							<asp:Label ID="lblAddImages" Runat="server" EnableViewState="False" ResourceKey="AddImages" />&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkAddImages" Runat="server" />
							<asp:TextBox ID="txtAddImagesLimit" Runat="server" Width="25px" CssClass="Normal" />
							<asp:Literal ID="litAddImagesLimit" Runat="server">*</asp:Literal>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp;
							<asp:Label ID="lblApprove" Runat="server" EnableViewState="False" ResourceKey="Approve" />&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkApprove" Runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp;
							<asp:Label ID="lblAutoApprove" Runat="server" EnableViewState="False" ResourceKey="AutoApprove" />&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkAutoApprove" Runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp;
							<asp:Label ID="lblDelete" Runat="server" EnableViewState="False" ResourceKey="Delete" />&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkDelete" Runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp;
							<asp:Label ID="lblFeature" Runat="server" EnableViewState="False" ResourceKey="Feature" />&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkFeature" Runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							&nbsp;
							<asp:Label ID="lblAutoFeature" Runat="server" EnableViewState="False" ResourceKey="AutoFeature" />&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkAutoFeature" Runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>		
				</Columns>
			</asp:DataGrid>
			<asp:Label ID="lblSubmitAmount" Runat="server" ResourceKey="SubmitAmount" CssClass="Normal" /><br /><br />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		<td colspan="2">
		    <dnn:sectionhead id="dshPermissionAdvancedSettings" cssclass="Head" runat="server" text="Advanced Settings"
	            section="tblPermissionAdvancedSettings" resourcekey="PermissionAdvancedSettings" includerule="True" IsExpanded="False"></dnn:sectionhead>
            <TABLE id="tblPermissionAdvancedSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Advanced Permission Settings Design Table"
	            border="0" runat="server">
	        <tr>
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <td class="SubHead" colspan="2"><dnn:label id="plPermissionsAdvanced" runat="server" controlname="grdPermissionsAdvanced" suffix=":"></dnn:label></td>
	        </tr>
	        <tr>
		        <td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <td valign="bottom" align="left" colspan="2">
			        <asp:DataGrid ID="grdPermissionsAdvanced" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
				        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
				        HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
				        BorderStyle="None" DataKeyField="Value">
				        <Columns>
					        <asp:TemplateColumn>
						        <ItemStyle HorizontalAlign="Left" />
						        <HeaderTemplate>
						        </HeaderTemplate>
						        <ItemTemplate>
							        <%# DataBinder.Eval(Container.DataItem, "Text") %>
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblViewDetail" Runat="server" EnableViewState="False" ResourceKey="ViewDetail" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkDetail" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblBroker" Runat="server" EnableViewState="False" ResourceKey="Broker" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkBroker" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblExport" Runat="server" EnableViewState="False" ResourceKey="Export" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkExport" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblLockDown" Runat="server" EnableViewState="False" ResourceKey="LockDown" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkLockDown" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>	
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblPublishDetail" Runat="server" EnableViewState="False" ResourceKey="PublishDetail" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkPublishDetail" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>						
				        </Columns>
			        </asp:DataGrid>
			        <asp:Label ID="lblSubmitAmount2" Runat="server" ResourceKey="SubmitAmount" CssClass="Normal" /><br /><br />
		        </td>
	        </tr>
	        <tr>
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <td class="SubHead" width="150"><dnn:label id="plDetailUrl" runat="server" controlname="txtPropertyLabel" suffix=":"></dnn:label></td>
		        <td valign="bottom">
			        <asp:TextBox ID="txtDetailUrl" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
		        </td>
	        </tr>
	        </TABLE>
		</td>
	</tr>
	<tr runat="server" id="trAdminSettings" runat="server" visible="false">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		<td colspan="2">
		    <dnn:sectionhead id="dshAdminSettings" cssclass="Head" runat="server" text="Advanced Settings"
	            section="tblAdminSettings" resourcekey="AdminSettings" includerule="True" IsExpanded="False"></dnn:sectionhead>
            <table id="tblAdminSettings" cellspacing="0" cellpadding="2" width="100%" summary="Admin Settings Design Table"
	            border="0" runat="server">
	       <tr>
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <td class="SubHead" colspan="2"><dnn:label id="plAdminPermissions" runat="server" controlname="grdPermissionsAdmin" suffix=":"></dnn:label></td>
	        </tr>
	        <tr>
		        <TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		        <td valign="bottom" colspan="2">
		            <asp:DataGrid ID="grdPermissionsAdmin" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
				        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
				        HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
				        BorderStyle="None" DataKeyField="Value">
				        <Columns>
					        <asp:TemplateColumn>
						        <ItemStyle HorizontalAlign="Left" />
						        <HeaderTemplate>
						        </HeaderTemplate>
						        <ItemTemplate>
							        <%# DataBinder.Eval(Container.DataItem, "Text") %>
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblCustomFields" Runat="server" EnableViewState="False" ResourceKey="CustomFields" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkCustomFields" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblReviewFields" Runat="server" EnableViewState="False" ResourceKey="ReviewFields" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkReviewFields" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblEmailFiles" Runat="server" EnableViewState="False" ResourceKey="EmailFiles" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkEmailFiles" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblLayoutFiles" Runat="server" EnableViewState="False" ResourceKey="LayoutFiles" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkLayoutFiles" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblLayoutSettings" Runat="server" EnableViewState="False" ResourceKey="LayoutSettings" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkLayoutSettings" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>		
					        <asp:TemplateColumn>
						        <HeaderTemplate>
							        &nbsp;
							        <asp:Label ID="lblTypes" Runat="server" EnableViewState="False" ResourceKey="Types" />&nbsp;
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:CheckBox ID="chkTypes" Runat="server" />
						        </ItemTemplate>
					        </asp:TemplateColumn>				
				        </Columns>
			        </asp:DataGrid>
		        </td>
	        </tr>
	        </table>
		</td>
	</tr>
</TABLE>
<p align="center">
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
