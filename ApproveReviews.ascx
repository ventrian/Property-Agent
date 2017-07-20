<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ApproveReviews.ascx.vb" Inherits="Ventrian.PropertyAgent.ApproveReviews" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
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
	<td height="5px" colspan="2"></td>
</tr>
</table>
<script type="text/javascript">
function SelectAll(CheckBoxControl)
{
    if (CheckBoxControl.checked == true)
    {
        var i;
        for (i=0; i < document.forms[0].elements.length; i++)
        {
            if ((document.forms[0].elements[i].type == 'checkbox') &&
            (document.forms[0].elements[i].name.indexOf('rptApproveReviews') > -1))
            {
                document.forms[0].elements[i].checked = true;
            }
        }
    }
    else
    {
        var i;
        for (i=0; i < document.forms[0].elements.length; i++)
        {
            if ((document.forms[0].elements[i].type == 'checkbox') &&
            (document.forms[0].elements[i].name.indexOf('rptApproveReviews') > -1))
            {
                document.forms[0].elements[i].checked = false;
            }
        }
    }
}
</script>
<asp:Repeater ID="rptApproveReviews" runat="server">
    <HeaderTemplate>
        <table cellpadding="4" cellspacing="0" width="100%">
        <tr>
            <td width="15px">&nbsp;</td>
            <td width="25px"><input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)"></td>
            <td class="NormalBold"><asp:Label ID="lblName" runat="Server" ResourceKey="Name.Header" /></td>
            <td class="NormalBold"><asp:Label ID="lblDate" runat="Server" ResourceKey="Date.Header" /></td>
            <td class="NormalBold"><asp:Label ID="lblProperty" runat="Server" ResourceKey="Property.Header" /></td>
            <td class="NormalBold"><asp:Label ID="Label1" runat="Server" ResourceKey="Fields.Header" /></td>
       </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td width="15px">&nbsp;</td>
            <td valign="top"><asp:CheckBox id="chkSelected" runat="server" ></asp:CheckBox></td>
            <td class="Normal" valign="top"><span class="Normal"><%#DataBinder.Eval(Container.DataItem, "DisplayName")%></span></td>
            <td class="Normal" valign="top"><span class="Normal"><%#DataBinder.Eval(Container.DataItem, "CreateDate", "{0:d}")%></span></td>
            <td class="Normal" valign="top"><span class="Normal"><a href='<%#GetPropertyLink(DataBinder.Eval(Container.DataItem, "PropertyID").ToString())%>' target="_blank">Link</a></a></span></td>
            <td class="Normal" valign="top"><span class="Normal"><%#GetFields(DataBinder.Eval(Container.DataItem, "ReviewID").ToString())%></span></td>
        </tr>
        <td colspan="6"><hr /></td>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:label id="lblNoReviews" ResourceKey="NoReviews" Runat="server" CssClass="Normal" Visible="False"></asp:Label>
<p align="center">
	<asp:linkbutton id="cmdApprove" resourcekey="cmdApprove" runat="server" cssclass="CommandButton" text="Approve" causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdReject" resourcekey="cmdReject" runat="server" cssclass="CommandButton" text="Reject" borderstyle="none" />
</p>