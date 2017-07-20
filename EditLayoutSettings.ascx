<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditLayoutSettings.ascx.vb" Inherits="Ventrian.PropertyAgent.EditLayoutSettings" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
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
<dnn:sectionhead id="dshLabelSettings" cssclass="Head" runat="server" text="Label Settings" section="tblLabelSettings"
	resourcekey="LabelSettings" includerule="True"></dnn:sectionhead>
<TABLE id="tblLabelSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Label Settings Design Table"
	border="0" runat="server">
	<TR>
		<td colSpan="3">
			<asp:label id="lblLabelSettingsHelp" cssclass="Normal" runat="server" resourcekey="LabelSettingsDescription"
				enableviewstate="False"></asp:label></td>
	</TR>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plMainLabel" runat="server" controlname="txtMainLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMainLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valMainLabel" Runat="server" ControlToValidate="txtMainLabel" Display="Dynamic"
				ResourceKey="valMainLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plPropertyLabel" runat="server" controlname="txtPropertyLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPropertyLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valPropertyLabel" Runat="server" ControlToValidate="txtPropertyLabel" Display="Dynamic"
				ResourceKey="valPropertyLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plPropertyPluralLabel" runat="server" controlname="txtPropertyPluralLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPropertyPluralLabel" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" />
			<asp:RequiredFieldValidator ID="valPropertyPluralLabel" Runat="server" ControlToValidate="txtPropertyPluralLabel"
				Display="Dynamic" ResourceKey="valPropertyPluralLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plPropertyTypeLabel" runat="server" controlname="txtPropertyTypeLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPropertyTypeLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valPropertyTypeLabel" Runat="server" ControlToValidate="txtPropertyTypeLabel"
				Display="Dynamic" ResourceKey="valPropertyTypeLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plPropertyTypePluralLabel" runat="server" controlname="txtPropertyTypePluralLabel"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtPropertyTypePluralLabel" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" />
			<asp:RequiredFieldValidator ID="valPropertyTypePluralLabel" Runat="server" ControlToValidate="txtPropertyTypePluralLabel"
				Display="Dynamic" ResourceKey="valPropertyTypePluralLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plLocationLabel" runat="server" controlname="txtLocationLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtLocationLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valLocationLabel" Runat="server" ControlToValidate="txtLocationLabel" Display="Dynamic"
				ResourceKey="valLocationLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plAgentLabel" runat="server" controlname="txtAgentLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtAgentLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valAgentLabel" Runat="server" ControlToValidate="txtAgentLabel" Display="Dynamic"
				ResourceKey="valAgentLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plAgentPluralLabel" runat="server" controlname="txtAgentPluralLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtAgentPluralLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valAgentPluralLabel" Runat="server" ControlToValidate="txtAgentPluralLabel" Display="Dynamic"
				ResourceKey="valAgentPluralLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plBrokerLabel" runat="server" controlname="txtBrokerLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtBrokerLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valBrokerLabel" Runat="server" ControlToValidate="txtBrokerLabel" Display="Dynamic"
				ResourceKey="valBrokerLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plBrokerPluralLabel" runat="server" controlname="txtBrokerPluralLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtBrokerPluralLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valBrokerPluralLabel" Runat="server" ControlToValidate="txtBrokerPluralLabel" Display="Dynamic"
				ResourceKey="valBrokerPluralLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plShortListLabel" runat="server" controlname="txtShortListLabel" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtShortListLabel" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valShortListLabel" Runat="server" ControlToValidate="txtShortListLabel" Display="Dynamic"
				ResourceKey="valShortListLabel.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshLangingPageSettings" cssclass="Head" runat="server" text="Landing Page Details"
	section="tblLangingPageSettings" resourcekey="LangingPageSettings" includerule="True"></dnn:sectionhead>
<TABLE id="tblLangingPageSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Landing Page Details Design Table"
	border="0" runat="server">
	<TR>
		<td colSpan="2">
			<asp:label id="lblLangingPageSettingsHelp" cssclass="Normal" runat="server" resourcekey="LangingPageSettingsDescription"
				enableviewstate="False"></asp:label></td>
	</TR>
	<tr valign="top">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td>
		    <table cellspacing="0" cellpadding="2" border="0" summary="Landing Page Settings Design Table"
				id="tblLandingPage" runat="server" width="100%">
			<tr>
		        <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		        <td class="SubHead" width="150"><dnn:label id="plLandingPageType" runat="server" controlname="lstLandingPageType" suffix=":"></dnn:label></td>
		        <td valign="bottom">
		            <asp:RadioButtonList ID="lstLandingPageType" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" />
		        </td>
	        </tr>
	        </table>
		</td>
	</tr>
	<TR vAlign="top">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td>
			<dnn:sectionhead id="dshSortOrder" cssclass="Head" runat="server" text="Sort Order Settings" section="tblSortOrder"
				resourcekey="SortOrderSettings" isexpanded="True" includerule="True" />
			<table cellspacing="0" cellpadding="2" border="0" summary="Sort Order Settings Design Table"
				id="tblSortOrder" runat="server" width="100%">
				<TR>
					<td colSpan="3">
						<asp:label id="lblSortOrderSettingsHelp" cssclass="Normal" runat="server" resourcekey="SortOrderSettingsDescription"
							enableviewstate="False"></asp:label></td>
				</TR>
				<tr>
					<td><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td width="100%">
						<asp:datagrid id="grdLandingPageSortOrder" Border="0" CellPadding="4" CellSpacing="0" AutoGenerateColumns="false"
							runat="server" summary="Landing Page Sort Order Design Table" GridLines="None">
							<Columns>
								<asp:TemplateColumn>
									<HeaderTemplate>
										<asp:Label ID="lblSectionHeader" Runat="server" ResourceKey="Section.Header" CssClass="NormalBold" />
									</HeaderTemplate>
									<ItemStyle CssClass="Normal" Width="150px" />
									<ItemTemplate>
										<asp:Label ID="lblSection" Runat="server" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderTemplate>
										<asp:Label ID="lblSortOrder" Runat="server" ResourceKey="SortOrder.Header" CssClass="NormalBold" />
									</HeaderTemplate>
									<ItemStyle Width="60px" HorizontalAlign="Center" />
									<ItemTemplate>
										<table cellpadding="0" cellspacing="0">
											<tr>
												<td width="16px">
													<asp:ImageButton ID="btnDown" Runat="server" ImageUrl="~/Images/dn.gif" /></td>
												<td width="16px">
													<asp:ImageButton ID="btnUp" Runat="server" ImageUrl="~/Images/up.gif" /></td>
											</tr>
										</table>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
			</table>
			<dnn:sectionhead id="dshFeatured" cssclass="Head" runat="server" text="Featured Settings" section="tblFeatured"
				resourcekey="FeaturedSettings" isexpanded="True" includerule="True" />
			<table cellspacing="0" cellpadding="2" border="0" summary="Featured Settings Design Table"
				id="tblFeatured" runat="server" width="100%">
				<TR>
					<td colSpan="3">
						<asp:label id="lblFeaturedSettingsHelp" cssclass="Normal" runat="server" resourcekey="FeaturedSettingsDescription"
							enableviewstate="False"></asp:label></td>
				</TR>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plFeaturedEnabled" runat="server" controlname="chkFeaturedEnabled" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkFeaturedEnabled" Runat="server" />
					</td>
				</tr>
	            <tr>
		            <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		            <td class="SubHead" width="150"><dnn:label id="plLayoutTypeFeatured" runat="server" controlname="lstLayoutType" suffix=":"></dnn:label></td>
		            <td valign="bottom">
			            <asp:RadioButtonList ID="lstLayoutTypeFeatured" Runat="server" CssClass="Normal" RepeatDirection="Horizontal"
				            RepeatLayout="Flow" />
		            </td>
	            </tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plFeaturedMaxNumber" runat="server" controlname="txtFeaturedMaxNumber" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:TextBox ID="txtFeaturedMaxNumber" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
							maxlength="6" /><asp:RequiredFieldValidator ID="valFeaturedMaxNumber" Runat="server" ControlToValidate="txtFeaturedMaxNumber"
							Display="Dynamic" ResourceKey="valFeaturedMaxNumber.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valFeaturedMaxNumberIsNumber" Runat="server" ControlToValidate="txtFeaturedMaxNumber"
							Display="Dynamic" ResourceKey="valFeaturedMaxNumberIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plFeaturedItemsPerRow" runat="server" controlname="txtFeaturedItemsPerRow" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:TextBox ID="txtFeaturedItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250"
							columns="10" maxlength="6" /><asp:RequiredFieldValidator ID="valFeaturedItemsPerRow" Runat="server" ControlToValidate="txtFeaturedItemsPerRow"
							Display="Dynamic" ResourceKey="valFeaturedItemsPerRow.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valFeaturedItemsPerRowIsNumber" Runat="server" ControlToValidate="txtFeaturedItemsPerRow"
							Display="Dynamic" ResourceKey="valFeaturedItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plFeaturedSortBy" runat="server" controlname="drpFeaturedSortBy" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:DropDownList id="drpFeaturedSortBy" Runat="server" CssClass="NormalTextBox" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plFeaturedSortDirection" runat="server" controlname="drpFeaturedSortdirection"
							suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:DropDownList id="drpFeaturedSortdirection" Runat="server" CssClass="NormalTextBox" />
					</td>
				</tr>
			</table>
		</td>
	</TR>
	<TR vAlign="top">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td>
			<dnn:sectionhead id="dshSearch" cssclass="Head" runat="server" text="Search Settings" section="tblSearch"
				resourcekey="SearchSettings" isexpanded="True" IncludeRule="True" />
			<table cellspacing="0" cellpadding="2" border="0" summary="Search Settings Design Table"
				id="tblSearch" runat="server">
				<TR>
					<td colSpan="3">
						<asp:label id="lblSearchSettings" cssclass="Normal" runat="server" resourcekey="SearchSettingsDescription"
							enableviewstate="False"></asp:label></td>
				</TR>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchEnabled" runat="server" controlname="chkSearchEnabled" suffix=":"></dnn:label></td>
					<td valign="bottom" width="300">
						<asp:CheckBox ID="chkSearchEnabled" Runat="server" />
					</td>
				</tr>
	            <tr>
		            <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		            <td class="SubHead" width="150"><dnn:label id="plSearchHideHelpIcon" runat="server" controlname="chkSearchHideHelpIcon" suffix=":"></dnn:label></td>
		            <td valign="bottom">
			            <asp:CheckBox ID="chkSearchHideHelpIcon" Runat="server" />
		            </td>
	            </tr>
                <tr>
		            <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
	                <td class="SubHead" width="150"><dnn:label id="plSearchHideTypesCount" runat="server" controlname="chkSearchHideTypesCount" suffix=":"></dnn:label></td>
	                <td valign="bottom">
		                <asp:CheckBox ID="chkSearchHideTypesCount" Runat="server" />
	                </td>
                </tr>
                <tr>
		            <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
	                <td class="SubHead" width="150"><dnn:label id="plSearchHideZeroTypes" runat="server" controlname="chkSearchHideZeroTypes" suffix=":"></dnn:label></td>
	                <td valign="bottom">
		                <asp:CheckBox ID="chkSearchHideZeroTypes" Runat="server" />
	                </td>
                </tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchWildcard" runat="server" controlname="chkSearchWildard" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkSearchWildard" Runat="server" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchTypes" runat="server" controlname="chkSearchTypes" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkSearchTypes" Runat="server" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchLocation" runat="server" controlname="chkSearchLocation" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkSearchLocation" Runat="server" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchBrokers" runat="server" controlname="chkSearchBrokers" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkSearchBrokers" Runat="server" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchAgents" runat="server" controlname="chkSearchAgents" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkSearchAgents" Runat="server" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchWidth" runat="server" controlname="txtSearchWidth" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:TextBox ID="txtSearchWidth" Runat="server" CssClass="NormalTextBox" width="250"
							columns="10" maxlength="6" />
						<asp:RequiredFieldValidator ID="valSearchWidth" Runat="server" ControlToValidate="txtSearchWidth"
							Display="Dynamic" ResourceKey="valSearchWidth.ErrorMessage" CssClass="NormalRed" />
						<asp:CompareValidator ID="valSearchWidthIsNumber" Runat="server" ControlToValidate="txtSearchWidth"
							Display="Dynamic" ResourceKey="valSearchWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
					</td>
				</tr>
				<tr>
					<td width="25"><IMG height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
					<td class="SubHead" width="150"><dnn:label id="plSearchStyle" runat="server" controlname="txtSearchStyle" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:TextBox ID="txtSearchStyle" Runat="server" CssClass="NormalTextBox" width="250" />
						<asp:RequiredFieldValidator ID="valSearchStyle" Runat="server" ControlToValidate="txtSearchStyle"
							Display="Dynamic" ResourceKey="valSearchStyle.ErrorMessage" CssClass="NormalRed" />
					</td>
				</tr>
			</table>
		</td>
	</TR>
	<TR vAlign="top">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td>
			<dnn:sectionhead id="dshTypes" cssclass="Head" runat="server" text="Types Settings" section="tblTypes"
				resourcekey="TypesSettings" isexpanded="True" IncludeRule="True" />
			<table cellspacing="0" cellpadding="2" border="0" summary="Types Settings Design Table"
				id="tblTypes" runat="server">
				<TR>
					<td colSpan="3">
						<asp:label id="lblTypesSettingsHelp" cssclass="Normal" runat="server" resourcekey="TypesSettingsDescription"
							enableviewstate="False"></asp:label></td>
				</TR>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plTypesEnabled" runat="server" controlname="chkTypesEnabled" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkTypesEnabled" Runat="server" />
					</td>
				</tr>
	            <tr>
		            <td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		            <td class="SubHead" width="150"><dnn:label id="plLayoutTypeTypes" runat="server" controlname="lstLayoutTypeTypes" suffix=":"></dnn:label></td>
		            <td valign="bottom">
			            <asp:RadioButtonList ID="lstLayoutTypeTypes" Runat="server" CssClass="Normal" RepeatDirection="Horizontal"
				            RepeatLayout="Flow" />
		            </td>
	            </tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plTypesHideZero" runat="server" controlname="chkTypesHideZero" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:CheckBox ID="chkTypesHideZero" Runat="server" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plTypesItemsPerRow" runat="server" controlname="txtTypesItemsPerRow" suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:TextBox ID="txtTypesItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
							maxlength="6" /><asp:RequiredFieldValidator ID="valTypesItemsPerRow" Runat="server" ControlToValidate="txtTypesItemsPerRow"
							Display="Dynamic" ResourceKey="valTypesItemsPerRow.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valTypesItemsPerRowIsNumber" Runat="server" ControlToValidate="txtTypesItemsPerRow"
							Display="Dynamic" ResourceKey="valTypesItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plTypesRepeatDirection" runat="server" controlname="drpTypesRepeatdirection"
							suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:DropDownList id="drpTypesRepeatdirection" Runat="server" CssClass="NormalTextBox" />
					</td>
				</tr>
				<tr>
					<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
					<td class="SubHead" width="150"><dnn:label id="plTypesSortBy" runat="server" controlname="drpTypesSortBy"
							suffix=":"></dnn:label></td>
					<td valign="bottom">
						<asp:DropDownList id="drpTypesSortBy" Runat="server" CssClass="NormalTextBox" />
					</td>
				</tr>
			</table>
		</td>
	</TR>
</TABLE>
<br>
<dnn:sectionhead id="dshListingSettings" cssclass="Head" runat="server" text="Listing Settings" section="tblListingSettings"
	resourcekey="ListingSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Listing Settings Design Table"
	id="tblListingSettings" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plListingItemsPerRow" runat="server" controlname="txtListingItemsPerRow" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtListingItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valListingItemsPerRow" Runat="server" ControlToValidate="txtListingItemsPerRow"
				Display="Dynamic" ResourceKey="valListingItemsPerRow.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valListingItemsPerRowIsNumber" Runat="server" ControlToValidate="txtListingItemsPerRow"
				Display="Dynamic" ResourceKey="valListingItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plListingItemsPerPage" runat="server" controlname="txtListingItemsPerPage" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtListingItemsPerPage" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="6" /><asp:RequiredFieldValidator ID="valListingItemsPerPage" Runat="server" ControlToValidate="txtListingItemsPerPage"
				Display="Dynamic" ResourceKey="valListingItemsPerPage.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valListingItemsPerPageIsNumber" Runat="server" ControlToValidate="txtListingItemsPerPage"
				Display="Dynamic" ResourceKey="valListingItemsPerPageIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plLayoutType" runat="server" controlname="lstLayoutType" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstLayoutType" Runat="server" CssClass="Normal" RepeatDirection="Horizontal"
				RepeatLayout="Flow" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plBubbleFeatured" runat="server" controlname="chkBubbleFeatured" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkBubbleFeatured" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plSearchSubTypes" runat="server" controlname="chkSearchSubTypes" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSearchSubTypes" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plPassSearchValues" runat="server" controlname="chkPassSearchValues" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkPassSearchValues" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSortBy" runat="server" controlname="drpSortBy" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortBy" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSortDirection" runat="server" controlname="drpSortdirection" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortdirection" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSortBySecondary" runat="server" controlname="drpSortBySecondary" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortBySecondary" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSortDirectionSecondary" runat="server" controlname="drpSortDirectionSecondary" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortDirectionSecondary" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSortByTertiary" runat="server" controlname="drpSortByTertiary" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortByTertiary" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSortDirectionTertiary" runat="server" controlname="drpSortDirectionTertiary" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpSortDirectionTertiary" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plUserSortable" runat="server" controlname="chkUserSortable" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkUserSortable" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSortFields" runat="server" controlname="lstSortFields" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:CheckBoxList ID="lstSortFields" runat="Server" Repeatdirection="Vertical" CssClass="NormalTextBox" RepeatLayout="Flow" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshImage" cssclass="Head" runat="server" text="Image Settings" section="tblImage"
	resourcekey="ImageSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Image Settings Design Table"
	id="tblImage" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plImagesEnabled" runat="server" controlname="chkImagesEnabled" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkImagesEnabled" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plHighQuality" runat="server" controlname="chkHighQuality" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHighQuality" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plIncludejQuery" runat="server" controlname="chkIncludejQuery" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkIncludejQuery" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSmallWidth" runat="server" controlname="txtSmallWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSmallWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valSmallWidth" Runat="server" ControlToValidate="txtSmallWidth" Display="Dynamic"
				ResourceKey="valSmallWidth.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valSmallWidthIsNumber" Runat="server" ControlToValidate="txtSmallWidth" Display="Dynamic"
				ResourceKey="valSmallWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSmallHeight" runat="server" controlname="txtSmallWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSmallHeight" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valSmallHeight" Runat="server" ControlToValidate="txtSmallHeight" Display="Dynamic"
				ResourceKey="valSmallHeight.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valSmallHeightIsNumber" Runat="server" ControlToValidate="txtSmallHeight" Display="Dynamic"
				ResourceKey="valSmallHeightIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plMediumWidth" runat="server" controlname="txtMediumWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMediumWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valMediumWidth" Runat="server" ControlToValidate="txtMediumWidth" Display="Dynamic"
				ResourceKey="valMediumWidth.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valMediumWidthIsNumber" Runat="server" ControlToValidate="txtMediumWidth" Display="Dynamic"
				ResourceKey="valMediumWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plMediumHeight" runat="server" controlname="txtMediumHeight" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMediumHeight" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valMediumHeight" Runat="server" ControlToValidate="txtMediumHeight" Display="Dynamic"
				ResourceKey="valMediumHeight.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valMediumHeightIsNumber" Runat="server" ControlToValidate="txtMediumHeight"
				Display="Dynamic" ResourceKey="valMediumHeightIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plLargeWidth" runat="server" controlname="txtLargeWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtLargeWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valLargeWidth" Runat="server" ControlToValidate="txtLargeWidth" Display="Dynamic"
				ResourceKey="valLargeWidth.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valLargeWidthIsNumber" Runat="server" ControlToValidate="txtLargeWidth" Display="Dynamic"
				ResourceKey="valLargeWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plLargeHeight" runat="server" controlname="txtLargeHeight" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtLargeHeight" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" /><asp:RequiredFieldValidator ID="valLargeHeight" Runat="server" ControlToValidate="txtLargeHeight" Display="Dynamic"
				ResourceKey="valLargeHeight.ErrorMessage" CssClass="NormalRed" /><asp:CompareValidator ID="valLargeHeightIsNumber" Runat="server" ControlToValidate="txtLargeHeight" Display="Dynamic"
				ResourceKey="valLargeHeightIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plImagesItemsPerRow" runat="server" controlname="txtImagesItemsPerRow" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtImagesItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="6" />
			<asp:RequiredFieldValidator ID="valImagesItemsPerRow" Runat="server" ControlToValidate="txtImagesItemsPerRow"
				Display="Dynamic" ResourceKey="valImagesItemsPerRow.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valImagesItemsPerRowIsNumber" Runat="server" ControlToValidate="txtImagesItemsPerRow"
				Display="Dynamic" ResourceKey="valImagesItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
	    <td class="SubHead" width="150"><dnn:label id="plUseWatermark" runat="server" resourcekey="UseWatermark" suffix=":" controlname="chkUseWatermark"></dnn:label></td>
	    <td><asp:CheckBox id="chkUseWatermark" Runat="server" CssClass="NormalTextBox"></asp:CheckBox></td>
    </tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
	    <td class="SubHead" width="150"><dnn:label id="plWatermarkText" runat="server" resourcekey="WatermarkText" suffix=":" controlname="txtWatermarkText"></dnn:label></td>
	    <td><asp:textbox id="txtWatermarkText" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox></td>
    </tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
	    <td class="SubHead" width="150"><dnn:label id="plWatermarkImage" runat="server" resourcekey="WatermarkImage" suffix=":" controlname="ctlWatermarkImage"></dnn:label></td>
	    <td><dnn:url id="ctlWatermarkImage" runat="server" width="275" Required="False" showtrack="False" shownewwindow="False"
							    showlog="False" urltype="F" showUrls="False" showfiles="True" showtabs="False"></dnn:url></td>
    </tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
	    <td class="SubHead" width="150"><dnn:label id="plWatermarkPosition" runat="server" resourcekey="WatermarkPosition" suffix=":" controlname="drpWatermarkPosition"></dnn:label></td>
	    <td><asp:DropDownList id="drpWatermarkPosition" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
    </tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
	    <td class="SubHead" width="150"><dnn:label id="plImageCategories" runat="server" resourcekey="ImageCategories" suffix=":" controlname="txtImageCategories"></dnn:label></td>
	    <td>
	        <asp:textbox id="txtImageCategories" cssclass="NormalTextBox" runat="server" width="250px"></asp:textbox>
            <asp:Label ID="lblImageCategoriesHelp" runat="server" CssClass="Normal" ResourceKey="ImageCategoriesHelp" />
        </td>
    </tr>
</table>
<br />
<dnn:sectionhead id="dshComment" cssclass="Head" runat="server" text="Comment Settings" section="tblComment"
	resourcekey="CommentSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Comment Settings Design Table"
	id="tblComment" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plCommentWidth" runat="server" controlname="txtCommentWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtCommentWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" /> 
			<asp:RequiredFieldValidator ID="valCommentWidth" Runat="server" ControlToValidate="txtCommentWidth"
				Display="Dynamic" ResourceKey="valCommentWidth.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valCommentWidthIsNumber" Runat="server" ControlToValidate="txtCommentWidth"
				Display="Dynamic" ResourceKey="valCommentWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plUseCaptcha" runat="server" controlname="chkUseCaptcha" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkUseCaptcha" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plCommentNotifyOwner" runat="server" controlname="chkCommentNotifyOwner" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkCommentNotifyOwner" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plCommentEmail" runat="server" controlname="txtCommentEmail" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:TextBox ID="txtCommentEmail" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshContact" cssclass="Head" runat="server" text="Contact Settings" section="tblContact"
	resourcekey="ContactSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Contact Settings Design Table"
	id="tblContact" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plContactDestination" runat="server" controlname="lstContactdestination" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstContactdestination" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" />
		</td>
	</tr>
	<tr runat="Server" id="trContactCustomEmail" visible="False">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plContactCustomEmail" runat="server" controlname="txtContactCustomEmail" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:TextBox ID="txtContactCustomEmail" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" />
		</td>
	</tr>
	<tr runat="Server" id="trContactField" visible="False">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plContactField" runat="server" controlname="txtContactCustomEmail" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:DropDownList ID="drpContactField" runat="server" DataValueField="CustomFieldID" DataTextField="Name" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plContactReply" runat="server" controlname="lstContactReply" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstContactReply" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plContactBCC" runat="server" controlname="txtContactBCC" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:TextBox ID="txtContactBCC" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plContactMessageLines" runat="server" controlname="txtContactMessageLines" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtContactMessageLines" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valContactMessageLines" Runat="server" ControlToValidate="txtContactMessageLines"
				Display="Dynamic" ResourceKey="valContactMessageLines.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valContactMessageLinesIsNumber" Runat="server" ControlToValidate="txtContactMessageLines"
				Display="Dynamic" ResourceKey="valContactMessageLinesIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plContactWidth" runat="server" controlname="txtContactWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtContactWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" /> <!-- columns="10" maxlength="6" -->
			<asp:RequiredFieldValidator ID="valContactWidth" Runat="server" ControlToValidate="txtContactWidth"
				Display="Dynamic" ResourceKey="valContactWidth.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valContactWidthIsNumber" Runat="server" ControlToValidate="txtContactWidth"
				Display="Dynamic" ResourceKey="valContactWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plHideName" runat="server" controlname="chkHideName" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHideName" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plRequireName" runat="server" controlname="chkRequireName" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkRequireName" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plHideEmail" runat="server" controlname="chkHideEmail" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHideEmail" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plRequireEmail" runat="server" controlname="chkRequireEmail" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkRequireEmail" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plHidePhone" runat="server" controlname="chkHidePhone" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHidePhone" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plRequirePhone" runat="server" controlname="chkRequirePhone" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkRequirePhone" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plContactUseCaptcha" runat="server" controlname="chkContactUseCaptcha" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkContactUseCaptcha" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plContactLogField" runat="server" controlname="chkContactUseCaptcha" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:DropDownList ID="drpContactLogField" runat="server" DataValueField="ContactFieldID" DataTextField="Caption" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plContactCustomField" runat="server" controlname="drpContactCustomField" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:DropDownList ID="drpContactCustomField" runat="server" DataValueField="CustomFieldID" DataTextField="Name" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshCoreSearchSettings" cssclass="Head" runat="server" text="Search Settings"
	section="tblCoreSearchSettings" resourcekey="CoreSearchSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Rss Settings Design Table"
	id="tblCoreSearchSettings" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plCoreSearch" runat="server" controlname="chkCoreSearchEnable" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkCoreSearchEnable" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plCoreSearchTitle" runat="server" controlname="txtCoreSearchTitle" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtCoreSearchTitle" Runat="server" CssClass="NormalTextBox" width="250" MaxLength="100" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plCoreSearchDescription" runat="server" controlname="txtCoreSearchDescription" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtCoreSearchDescription" Runat="server" CssClass="NormalTextBox" width="250" MaxLength="100" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshCurrency" cssclass="Head" runat="server" text="Currency Settings" section="tblCurrency"
	resourcekey="CurrencySettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Currency Settings Design Table"
	id="tblCurrency" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plCurrency" runat="server" controlname="drpCurrency" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:DropDownList ID="drpCurrency" runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>	
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		<td class="SubHead" width="150">
			<dnn:label id="plEuroFormat" runat="server" controlname="lstEuroFormat" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstEuroFormat" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plCurrencyAvailable" runat="server" controlname="chkCurrencyAvailable" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:CheckBox ID="chkCurrencyShowAll" runat="server" AutoPostBack="true" ResourceKey="ShowAll" CssClass="Normal" />
		    <asp:CheckBoxList ID="chkCurrencyAvailableList" runat="server" CssClass="Normal" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plDecimalPlaces" runat="server" controlname="txtDecimalPlaces"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtCurrencyDecimalPlaces" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="1" />
			<asp:RequiredFieldValidator ID="valDecimalPlaces" Runat="server" ControlToValidate="txtCurrencyDecimalPlaces"
				Display="Dynamic" ResourceKey="valDecimalPlaces.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valDecimalPlacesIsNumber" Runat="server" ControlToValidate="txtCurrencyDecimalPlaces"
				Display="Dynamic" ResourceKey="valDecimalPlacesIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshForm" cssclass="Head" runat="server" text="Form Settings" section="tblForm"
	resourcekey="FormSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Form Settings Design Table"
	id="tblForm" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plButtonClass" runat="server" controlname="txtButtonClass"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtButtonClass" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="255" />
			<asp:RequiredFieldValidator ID="valButtonClass" Runat="server" ControlToValidate="txtButtonClass"
				Display="Dynamic" ResourceKey="valButtonClass.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plCachePropertyValues" runat="server" controlname="chkCachePropertyValues" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkCachePropertyValues" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plHideTypes" runat="server" controlname="chkHideTypes" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHideTypes" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plTypeParams" runat="server" controlname="chkTypeParams" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkTypeParams" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plBreadcrumbPlacement" runat="server" controlname="lstBreadcrumbPlacement" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstBreadcrumbPlacement" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plBindExpiry" runat="server" controlname="drpBindExpiry"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:DropDownList ID="drpBindExpiry" runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plDefaultExpiration" runat="server" controlname="txtdefaultExpiration"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtdefaultExpiration" Runat="server" CssClass="NormalTextBox" width="50" maxlength="6" />
			<asp:DropDownList id="drpDefaultExpiration" Runat="server" CssClass="NormalTextBox" />
			<asp:CompareValidator ID="valDefaultExpirationIsNumber" Runat="server" ControlToValidate="txtdefaultExpiration"
				Display="Dynamic" ResourceKey="valDefaultExpirationIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
			<asp:Label ID="lblDefaultExpiration" CssClass="Normal" runat="Server" ResourceKey="DefaultExpiration" />	
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plFieldWidth" runat="server" controlname="txtFieldWidth"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtFieldWidth" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valFieldWidth" Runat="server" ControlToValidate="txtFieldWidth"
				Display="Dynamic" ResourceKey="valFieldWidth.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valFieldWidthIsNumber" Runat="server" ControlToValidate="txtFieldWidth"
				Display="Dynamic" ResourceKey="valFieldWidthIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plRadioButtonItemsPerRow" runat="server" controlname="txtRadioButtonItemsPerRow"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtRadioButtonItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valRadioButtonItemsPerRow" Runat="server" ControlToValidate="txtRadioButtonItemsPerRow"
				Display="Dynamic" ResourceKey="valRadioButtonItemsPerRow.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valRadioButtonItemsPerRowIsNumber" Runat="server" ControlToValidate="txtRadioButtonItemsPerRow"
				Display="Dynamic" ResourceKey="valRadioButtonItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plCheckBoxListItemsPerRow" runat="server" controlname="txtCheckBoxListItemsPerRow"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtCheckBoxListItemsPerRow" Runat="server" CssClass="NormalTextBox" width="250"
				columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valCheckBoxListItemsPerRow" Runat="server" ControlToValidate="txtCheckBoxListItemsPerRow"
				Display="Dynamic" ResourceKey="valCheckBoxListItemsPerRow.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valCheckBoxListItemsPerRowIsNumber" Runat="server" ControlToValidate="txtCheckBoxListItemsPerRow"
				Display="Dynamic" ResourceKey="valCheckBoxListItemsPerRowIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plLockDownPropertyType" runat="server" controlname="chkLockDownPropertyType"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkLockDownPropertyType" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plLockDownPropertyDates" runat="server" controlname="chkLockDownPropertyDates"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkLockDownPropertyDates" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plLockDownFeatured" runat="server" controlname="chkLockDownFeatured"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkLockDownFeatured" Runat="server" />
		</td>
	</tr>	
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plRedirectType" runat="server" controlname="lstRedirectType" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstRedirectType" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" />
		</td>
	</tr>
	<tr id="trRedirectPage" runat="server">
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plRedirectPage" runat="server" controlname="drpRedirectPage" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:DropDownList ID="drpRedirectPage" runat="server" CssClass="NormalTextBox" DataValueField="TabID" DataTextField="TabName" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plUploadMode" runat="server" controlname="lstUploadMode" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstUploadMode" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plUploadPlacement" runat="server" controlname="lstUploadPlacement" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstUploadPlacement" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" />
		</td>
	</tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plProtectXSS" runat="server" controlname="chkProtectXSS" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkProtectXSS" Runat="server" />
		</td>
	</tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plAgentDropdown" runat="server" controlname="chkAgentDropdown" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkAgentDropdown" Runat="server" />
		</td>
	</tr>
</table>
<br>
<dnn:sectionhead id="dshMapSettings" cssclass="Head" runat="server" text="Map Settings" section="tblMap"
	resourcekey="MapSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Map Settings Design Table" id="tblMap" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plEnableMaps" runat="server" controlname="chkEnableMaps"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkEnableMaps" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plMapKey" runat="server" controlname="txtMapKey"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMapKey" Runat="server" CssClass="NormalTextBox" width="250" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		<td class="SubHead" width="150"><dnn:label id="plMapWidth" runat="server" controlname="txtMapWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMapWidth" Runat="server" CssClass="NormalTextBox" width="250" maxlength="5" />
			<asp:RequiredFieldValidator ID="valMapWidth" Runat="server" ControlToValidate="txtMapWidth"
				Display="Dynamic" ResourceKey="valMapWidth.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valMapWidthIsNumber" Runat="server" ControlToValidate="txtMapWidth"
				Display="Dynamic" ResourceKey="valMapWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		<td class="SubHead" width="150"><dnn:label id="plMapHeight" runat="server" controlname="txtMapHeight" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMapHeight" Runat="server" CssClass="NormalTextBox" width="250" maxlength="5" />
			<asp:RequiredFieldValidator ID="valMapHeight" Runat="server" ControlToValidate="txtMapHeight"
				Display="Dynamic" ResourceKey="valMapHeight.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valMapHeightIsNumber" Runat="server" ControlToValidate="txtMapHeight"
				Display="Dynamic" ResourceKey="valMapHeightIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25" /></td>
		<td class="SubHead" width="150"><dnn:label id="plMapZoom" runat="server" controlname="txtMapZoom" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtMapZoom" Runat="server" CssClass="NormalTextBox" width="250" maxlength="5" />
			<asp:RequiredFieldValidator ID="valMapZoom" Runat="server" ControlToValidate="txtMapZoom"
				Display="Dynamic" ResourceKey="valMapZoom.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valMapZoomIsNumber" Runat="server" ControlToValidate="txtMapZoom"
				Display="Dynamic" ResourceKey="valMapZoomIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plDistanceExpression" runat="server" controlname="txtdistanceExpression"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtDistanceExpression" Runat="server" CssClass="NormalTextBox" width="250" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150">
			<dnn:label id="plDistanceType" runat="server" controlname="lstDistanceType" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:RadioButtonList ID="lstDistanceType" runat="Server" CssClass="Normal" Repeatdirection="Horizontal" RepeatLayout="Flow" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshNotificationSettings" cssclass="Head" runat="server" text="Notification Settings" section="tblNotification"
	resourcekey="NotificationSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Notification Settings Design Table" id="tblNotification" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plNotificationNotifyApprovers" runat="server" controlname="chkNotificationNotifyApprovers"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkNotificationNotifyApprovers" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plNotificationEmail" runat="server" controlname="txtNotificationEmail"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtNotificationEmail" Runat="server" CssClass="NormalTextBox" width="250" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plNotifyBroker" runat="server" controlname="chkNotifyBroker"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkNotifyBroker" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plNotifyOwner" runat="server" controlname="chkNotifyOwner"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkNotifyOwner" Runat="server" />
		</td>
	</tr>
</table>
<br>
<dnn:sectionhead id="dshPropertyManager" cssclass="Head" runat="server" text="Property Manager Settings"
	section="tblPropertyManager" resourcekey="PropertyManagerSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Property Manager Settings Design Table"
	id="tblPropertyManager" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plHideAuthorDetails" runat="server" controlname="chkHideAuthorDetails" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHideAuthorDetails" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plHidePublishingDetails" runat="server" controlname="chkPublishingDetails" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHidePublishingDetails" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plPropertyManagerRecordsPerPage" runat="server" controlname="drpPropertyManagerRecordsPerPage"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpPropertyManagerRecordsPerPage" Runat="server" CssClass="NormalTextBox">
				<asp:ListItem Value="10">10</asp:ListItem>
				<asp:ListItem Value="25">25</asp:ListItem>
				<asp:ListItem Value="50">50</asp:ListItem>
				<asp:ListItem Value="100">100</asp:ListItem>
				<asp:ListItem Value="250">250</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plPropertyManagerSortBy" runat="server" controlname="drpPropertyManagerSortBy"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpPropertyManagerSortBy" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plPropertyManagerSortDirection" runat="server" controlname="drpPropertyManagerSortDirection"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:DropDownList id="drpPropertyManagerSortDirection" Runat="server" CssClass="NormalTextBox" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshReviewSettings" cssclass="Head" runat="server" text="Review Settings" section="tblReview"
	resourcekey="ReviewSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Review Settings Design Table"
	id="tblReview" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plReviewWidth" runat="server" controlname="txtReviewWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtReviewWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" /> 
			<asp:RequiredFieldValidator ID="valReviewWidth" Runat="server" ControlToValidate="txtReviewWidth"
				Display="Dynamic" ResourceKey="valReviewWidth.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valReviewWidthIsNumber" Runat="server" ControlToValidate="txtReviewWidth"
				Display="Dynamic" ResourceKey="valReviewWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plReviewModeration" runat="server" controlname="chkReviewModeration" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:CheckBox ID="chkReviewModeration" runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plReviewEmail" runat="server" controlname="txtReviewEmail" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:TextBox ID="txtReviewEmail" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plReviewAnonymous" runat="server" controlname="chkReviewAnonymous" suffix=":"></dnn:label></td>
		<td valign="bottom">
		    <asp:CheckBox ID="chkReviewAnonymous" runat="server" />
		</td>
	</tr>
</table>
<br>
<dnn:sectionhead id="dshRssSettings" cssclass="Head" runat="server" text="RSS Settings"
	section="tblRssSettings" resourcekey="RssSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Rss Settings Design Table"
	id="tblRssSettings" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plRssEnable" runat="server" controlname="chkRssEnable" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkRssEnable" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><IMG height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plRssMaxRecords" runat="server" controlname="txtRssMaxRecords"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtRssMaxRecords" Runat="server" CssClass="NormalTextBox" width="250" columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valRssMaxRecords" Runat="server" ControlToValidate="txtRssMaxRecords"
				Display="Dynamic" ResourceKey="valRssMaxRecords.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valRssMaxRecordsIsNumber" Runat="server" ControlToValidate="txtRssMaxRecords"
				Display="Dynamic" ResourceKey="valRssMaxRecordsIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><IMG height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plRssTitleLatest" runat="server" controlname="txtRssTitleLatest" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtRssTitleLatest" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valRssTitleLatest" Runat="server" ControlToValidate="txtRssTitleLatest"
				Display="Dynamic" ResourceKey="valRssTitleLatest.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><IMG height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plRssTitleType" runat="server" controlname="txtRssTitleType" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtRssTitleType" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valRssTitleType" Runat="server" ControlToValidate="txtRssTitleType"
				Display="Dynamic" ResourceKey="valRssTitleType.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><IMG height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plRssTitleSearchResult" runat="server" controlname="txtRssTitleSearchResult" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtRssTitleSearchResult" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valRssTitleSearchResult" Runat="server" ControlToValidate="txtRssTitleSearchResult"
				Display="Dynamic" ResourceKey="valRssTitleSearchResult.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshSendToFriendSettings" cssclass="Head" runat="server" text="Send To Friend Settings" section="tblSendToFriend"
	resourcekey="SendToFriendSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Send To Friend Settings Design Table"
	id="tblSendToFriend" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150">
			<dnn:label id="plFriendBCC" runat="server" controlname="txtFriendBCC" suffix=":"></dnn:label>
		</td>
		<td valign="bottom">
			<asp:TextBox ID="txtFriendBCC" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plFriendWidth" runat="server" controlname="txtFriendWidth" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtFriendWidth" Runat="server" CssClass="NormalTextBox" width="250" columns="10"
				maxlength="150" /> 
			<asp:RequiredFieldValidator ID="valFriendWidth" Runat="server" ControlToValidate="txtFriendWidth"
				Display="Dynamic" ResourceKey="valFriendWidth.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valFriendWidthIsNumber" Runat="server" ControlToValidate="txtFriendWidth"
				Display="Dynamic" ResourceKey="valFriendWidthIsNumber.ErrorMessage" CssClass="NormalRed" Operator="DataTypeCheck"
				Type="Integer" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshSEOSettings" cssclass="Head" runat="server" text="SEO Settings" section="tblSEOSettings"
	resourcekey="SEOSettings" includerule="True"></dnn:sectionhead>
<table id="tblSEOSettings" cellSpacing="0" cellPadding="2" width="100%" summary="SEO Settings Design Table"
	border="0" runat="server">
	<tr>
		<td colSpan="3">
			<asp:label id="lblSEOSettings" cssclass="Normal" runat="server" resourcekey="SEOSettingsDescription"
				enableviewstate="False"></asp:label></td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plSEORedirect" runat="server" controlname="chkSEORedirect" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSEORedirect" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSEOAgentType" runat="server" controlname="txtSEOAgentType" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSEOAgentType" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valSEOAgentType" Runat="server" ControlToValidate="txtSEOAgentType" Display="Dynamic"
				ResourceKey="valSEOAgentType.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSEOPropertyID" runat="server" controlname="txtSEOPropertyID" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSEOPropertyID" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valSEOPropertyID" Runat="server" ControlToValidate="txtSEOPropertyID" Display="Dynamic"
				ResourceKey="valSEOPropertyID.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSEOPropertyTypeID" runat="server" controlname="txtSEOPropertyTypeID" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSEOPropertyTypeID" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
			<asp:RequiredFieldValidator ID="valSEOPropertyTypeID" Runat="server" ControlToValidate="txtSEOPropertyTypeID" Display="Dynamic"
				ResourceKey="valSEOPropertyTypeID.ErrorMessage" CssClass="NormalRed" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSEOViewPropertyTitle" runat="server" controlname="txtSEOViewPropertyTitle" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSEOViewPropertyTitle" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
		<td class="SubHead" width="150"><dnn:label id="plSEOViewTypeTitle" runat="server" controlname="txtSEOViewTypeTitle" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtSEOViewTypeTitle" Runat="server" CssClass="NormalTextBox" width="250" columns="10" />
		</td>
	</tr>
    <tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
        <td class="SubHead" width="150"><dnn:label id="plSEOTitleReplacement" runat="server" suffix=":" controlname="lstSEOTitleReplacement"></dnn:label></td>
        <td valign="top"><asp:RadioButtonList ID="lstSEOTitleReplacement" Runat="server" CssClass="Normal" RepeatDirection="Horizontal" /></td>
    </tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plSEOCanonicalLink" runat="server" controlname="chkSEOCanonicalLink" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkSEOCanonicalLink" Runat="server" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshTemplateSettings" cssclass="Head" runat="server" text="Template Settings"
	section="tblTemplateSettings" resourcekey="TemplateSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Rss Settings Design Table" id="tblTemplateSettings" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plIncludeStylesheet" runat="server" controlname="chkTemplateIncludeStylesheet" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkTemplateIncludeStylesheet" Runat="server" />
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshXmlSettings" cssclass="Head" runat="server" text="Xml Settings"
	section="tblXmlSettings" resourcekey="XmlSettings" isexpanded="True" IncludeRule="True" />
<table cellspacing="0" cellpadding="2" border="0" summary="Xml Settings Design Table"
	id="tblXmlSettings" runat="server">
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plXmlEnable" runat="server" controlname="chkXmlEnable" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkXmlEnable" Runat="server" />
		</td>
	</tr>
	<tr>
		<td width="25"><IMG height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plXmlMaxRecords" runat="server" controlname="txtXmlMaxRecords"
				suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox ID="txtXmlMaxRecords" Runat="server" CssClass="NormalTextBox" width="250" columns="10" maxlength="6" />
			<asp:RequiredFieldValidator ID="valXmlMaxRecords" Runat="server" ControlToValidate="txtXmlMaxRecords"
				Display="Dynamic" ResourceKey="valXmlMaxRecords.ErrorMessage" CssClass="NormalRed" />
			<asp:CompareValidator ID="valXmlMaxRecordsIsNumber" Runat="server" ControlToValidate="txtXmlMaxRecords"
				Display="Dynamic" ResourceKey="valXmlMaxRecordsIsNumber.ErrorMessage" CssClass="NormalRed"
				Operator="DataTypeCheck" Type="Integer" />
		</td>
	</tr>
	<tr>
		<td width="25"><img height="1" src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width="25"></td>
		<td class="SubHead" width="150"><dnn:label id="plXmlUrl" runat="server" controlname="lblXmlUrl" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:Label ID="lblXmlUrl" Runat="server" CssClass="Normal" />
		</td>
	</tr>
</table>
<p align="center">
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
