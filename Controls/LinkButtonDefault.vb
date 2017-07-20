Namespace Ventrian.PropertyAgent

    Public Class LinkButtonDefault
        Inherits LinkButton

        Protected Overloads Overrides Sub OnLoad(ByVal e As System.EventArgs)
            Page.ClientScript.RegisterStartupScript([GetType](), "addClickFunctionScript", _addClickFunctionScript, True)

            Dim script As String = [String].Format(_addClickScript, ClientID)
            Page.ClientScript.RegisterStartupScript([GetType](), "click_" + ClientID, script, True)
            MyBase.OnLoad(e)
        End Sub

        Private Const _addClickScript As String = "addClickFunction('{0}');"
        Private Const _addClickFunctionScript As String = "" _
            & "function addClickFunction(id) {{" _
            & "var b = document.getElementById(id);" _
            & "if (b && typeof(b.click) == 'undefined') b.click = function() {{" _
            & "var result = true; if (b.onclick) result = b.onclick();" _
            & " if (typeof(result) == 'undefined' || result) {{ eval(b.href); }}" _
            & "}}}};"

    End Class


End Namespace
