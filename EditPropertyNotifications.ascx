<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditPropertyNotifications.ascx.vb" Inherits="Ventrian.PropertyAgent.EditPropertyNotifications" %>
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
<dnn:sectionhead id="dshPropertySettings" cssclass="Head" runat="server" text="Property Settings"
	section="tblPropertySettings" resourcekey="PropertySettings" includerule="True" />
<TABLE id="tblPropertySettings" cellSpacing="0" cellPadding="2" width="100%" summary="Property Settings Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblPropertySettingsHelp" cssclass="Normal" runat="server" resourcekey="PropertySettingsHelp"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="200"><dnn:label id="plSubject" runat="server" resourcekey="Subject" suffix=":" controlname="txtSubject"></dnn:label></td>
		<TD>
			<asp:textbox id="txtSubject" cssclass="NormalTextBox" width="300px" maxlength="100" runat="server" />
			<asp:requiredfieldvalidator id="valSubject" runat="server" CssClass="NormalRed" ControlToValidate="txtSubject"
				ResourceKey="valSubject.ErrorMessage" ErrorMessage="<br>Subject Is Required." Display="Dynamic"></asp:requiredfieldvalidator>
		</TD>
	</TR>
	<TR>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="200"><dnn:label id="plTemplateHeader" runat="server" suffix=":" controlname="txtTemplateHeader"></dnn:label></td>
		<TD>
			<asp:textbox id="txtTemplateHeader" cssclass="NormalTextBox" runat="server" width="300" rows="10" textmode="MultiLine"></asp:textbox>
		</TD>
	</TR>
	<TR>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="200"><dnn:label id="plTemplate" runat="server" suffix=":" controlname="txtTemplate"></dnn:label></td>
		<TD>
			<asp:textbox id="txtTemplate" cssclass="NormalTextBox" runat="server" width="300" rows="10" textmode="MultiLine"></asp:textbox>
			<asp:requiredfieldvalidator id="valTemplate" runat="server" CssClass="NormalRed" ControlToValidate="txtTemplate"
				ResourceKey="valTemplate.ErrorMessage" ErrorMessage="<br>Template Is Required." Display="Dynamic"></asp:requiredfieldvalidator>
		</TD>
	</TR>
	<TR>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="200"><dnn:label id="plTemplateFooter" runat="server" suffix=":" controlname="txtTemplateFooter"></dnn:label></td>
		<TD>
			<asp:textbox id="txtTemplateFooter" cssclass="NormalTextBox" runat="server" width="300" rows="10" textmode="MultiLine"></asp:textbox>
		</TD>
	</TR>
	<TR>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="200"><dnn:label id="plToEmail" runat="server" resourcekey="plToEmail" suffix=":" controlname="txtToEmail"></dnn:label></td>
		<TD>
			<asp:textbox id="txtToEmail" cssclass="NormalTextBox" runat="server" width="300"></asp:textbox>
			<asp:requiredfieldvalidator id="valToEmail" runat="server" CssClass="NormalRed" ControlToValidate="txtToEmail"
				ResourceKey="valToEmail.ErrorMessage" ErrorMessage="<br>To Email Is Required." Display="Dynamic"></asp:requiredfieldvalidator>
		</TD>
	</TR>
	<TR>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="200"><dnn:label id="plBCCEmail" runat="server" resourcekey="plBCCEmail" suffix=":" controlname="txtBCCEmail"></dnn:label></td>
		<TD>
			<asp:textbox id="txtBCCEmail" cssclass="NormalTextBox" runat="server" width="300"></asp:textbox>
		</TD>
	</TR>
</TABLE>
<br>
<dnn:sectionhead id="dshSchedulerSettings" cssclass="Head" runat="server" text="Scheduler Settings"
	section="tblSchedulerSettings" resourcekey="SchedulerSettings" includerule="True"></dnn:sectionhead>
<TABLE id="tblSchedulerSettings" cellSpacing="0" cellPadding="2" width="100%" summary="Scheduler Settings Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblSchedulerSettingsHelp" cssclass="Normal" runat="server" resourcekey="SchedulerSettingsHelp"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr>
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<td class="SubHead" width="200"><dnn:label id="plSchedulerEnabled" text="Enabled?" runat="server" controlname="chkEnabled"></dnn:label></td>
		<td valign="top">
			<asp:CheckBox ID="chkEnabled" Runat="server" />
		</td>
	</tr>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" width="200">
			<dnn:label id="plTimeLapse" runat="server" controlname="txtTimeLapse" suffix=":" text="Time Lapse:"></dnn:label></TD>
		<TD class="Normal">
			<asp:textbox id="txtTimeLapse" runat="server" maxlength="10" width="50" cssclass="NormalTextBox"></asp:textbox>
			<asp:dropdownlist id="drpTimeLapseMeasurement" runat="server" cssclass="NormalTextBox">
				<asp:listitem resourcekey="Seconds" value="s">Seconds</asp:listitem>
				<asp:listitem resourcekey="Minutes" value="m">Minutes</asp:listitem>
				<asp:listitem resourcekey="Hours" value="h">Hours</asp:listitem>
				<asp:listitem resourcekey="Days" value="d">Days</asp:listitem>
			</asp:dropdownlist></TD>
	</TR>
	<TR vAlign="top">
		<TD width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></TD>
		<TD class="SubHead" width="200">
			<dnn:label id="plRetryTimeLapse" runat="server" controlname="txtRetryTimeLapse" suffix=":"
				text="Retry Frequency:"></dnn:label></TD>
		<TD class="Normal">
			<asp:textbox id="txtRetryTimeLapse" runat="server" maxlength="10" width="50" cssclass="NormalTextBox"></asp:textbox>
			<asp:dropdownlist id="drpRetryTimeLapseMeasurement" runat="server" cssclass="NormalTextBox">
				<asp:listitem resourcekey="Seconds" value="s">Seconds</asp:listitem>
				<asp:listitem resourcekey="Minutes" value="m">Minutes</asp:listitem>
				<asp:listitem resourcekey="Hours" value="h">Hours</asp:listitem>
				<asp:listitem resourcekey="Days" value="d">Days</asp:listitem>
			</asp:dropdownlist></TD>
	</TR>
</TABLE>
<p align="center">
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
<dnn:sectionhead id="dshSchedulerHistory" cssclass="Head" runat="server" text="Scheduler History"
	section="tblSchedulerHistory" resourcekey="SchedulerHistory" includerule="True"></dnn:sectionhead>
<TABLE id="tblSchedulerHistory" cellSpacing="0" cellPadding="2" width="100%" summary="Scheduler History Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblSchedulerHistoryHelp" cssclass="Normal" runat="server" resourcekey="SchedulerHistoryHelp"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr>
		<td>
			<asp:Label ID="lblNoHistory" Runat="server" CssClass="Normal" ResourceKey="NoHistory" />
			<asp:datagrid id="dgScheduleHistory" runat="server" autogeneratecolumns="false" cellpadding="4"
				cellspacing="2" datakeyfield="ScheduleID" enableviewstate="false" border="1" summary="This table shows the schedule of events for the portal."
				BorderColor="gray" BorderStyle="Solid" BorderWidth="1px" GridLines="Both">
				<Columns>
					<asp:TemplateColumn HeaderText="Description">
						<HeaderStyle CssClass="NormalBold" />
						<ItemStyle CssClass="Normal" VerticalAlign="Top" />
						<ItemTemplate>
							<table border="0" width="100%">
								<tr>
									<td nowrap Class="Normal">
										<i>
											<%# DataBinder.Eval(Container.DataItem,"TypeFullName")%>
										</i>
									</td>
								</tr>
							</table>
							<asp:Label runat=server visible='<%# DataBinder.Eval(Container.DataItem,"LogNotes")<>""%>' ID="Label1" NAME="Label1">
								<textarea rows="2" cols="65"><%# DataBinder.Eval(Container.DataItem,"LogNotes")%></textarea>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="ElapsedTime" HeaderText="Duration">
						<HeaderStyle CssClass="NormalBold"></HeaderStyle>
						<ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Succeeded" HeaderText="Succeeded">
						<HeaderStyle CssClass="NormalBold"></HeaderStyle>
						<ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Start/End/Next Start">
						<HeaderStyle CssClass="NormalBold"></HeaderStyle>
						<ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
						<ItemTemplate>
							S:&nbsp;<%# DataBinder.Eval(Container.DataItem,"StartDate")%>
							<br>
							E:&nbsp;<%# DataBinder.Eval(Container.DataItem,"EndDate")%>
							<br>
							N:&nbsp;<%# DataBinder.Eval(Container.DataItem,"NextStart")%>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid>
		</td>
	</tr>
</TABLE>