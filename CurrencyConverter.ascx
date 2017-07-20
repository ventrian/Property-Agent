<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CurrencyConverter.ascx.vb" Inherits="Ventrian.PropertyAgent.CurrencyConverter" %>
<asp:DropDownList ID="drpCurrency" runat="server" CssClass="NormalTextBox" onchange="javascript:showCurrency(this.options[this.selectedIndex].value);" />
<span id="posting:Currency" /> 

<script language="JavaScript" type="text/javascript"> 
function showCurrency(val) 
{ 
    if( val != "-1" )
    {
        document.getElementById("posting:Currency").innerHTML = val;
    }
    else
    {
        document.getElementById("posting:Currency").innerHTML = '';
    }
} 
</script> 
