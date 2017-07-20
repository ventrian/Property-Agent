<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Latest.ascx.vb" Inherits="Ventrian.PropertyAgent.Latest" %>
<%@ Register TagPrefix="ventrian" Namespace="Ventrian.PropertyAgent" Assembly="Ventrian.PropertyAgent" %>
<div align="right" id="divSort" runat="server">
		<asp:Label ID="lblSortBy" Runat="server" resourcekey="SortBy" CssClass="SubHead" EnableViewState="False">Sort By:</asp:Label>
		<asp:DropDownList id="drpSortBy" Runat="server" CssClass="NormalTextBox" AutoPostBack="True" />
		<asp:DropDownList id="drpSortDirection" Runat="server" CssClass="NormalTextBox" AutoPostBack="True" />
</div>
<asp:PlaceHolder ID="phProperty" Runat="server" />
<ventrian:pagingcontrol id="ctlPagingControl1" runat="server" Visible="false"></ventrian:pagingcontrol>

