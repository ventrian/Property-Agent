Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class CurrencySelector
        Inherits PropertyAgentControl

        Private ReadOnly Property ResourceFile() As String
            Get
                Return "~/DesktopModules/PropertyAgent/App_LocalResources/EditLayoutSettings"
            End Get
        End Property

        Private Sub BindCurrency()

            For Each value As Integer In System.Enum.GetValues(GetType(CurrencyType))
                If (PropertySettings.CurrencyShowAll) Then
                    Dim li As New ListItem
                    li.Value = System.Enum.GetName(GetType(CurrencyType), value)
                    li.Text = Localization.GetString(System.Enum.GetName(GetType(CurrencyType), value), ResourceFile)
                    drpCurrency.Items.Add(li)
                Else
                    For Each currency As String In PropertySettings.CurrencyAvailable.Split(","c)
                        Dim li As New ListItem
                        li.Value = System.Enum.GetName(GetType(CurrencyType), value)
                        li.Text = Localization.GetString(System.Enum.GetName(GetType(CurrencyType), value), ResourceFile)
                        If (currency = li.Value) Then
                            drpCurrency.Items.Add(li)
                        End If
                    Next
                End If
            Next

            If (Request.Cookies("PA-" & ModuleID.ToString() & "-Currency") IsNot Nothing) Then
                If (drpCurrency.Items.FindByValue(Request.Cookies("PA-" & ModuleID.ToString() & "-Currency").Value) IsNot Nothing) Then
                    drpCurrency.SelectedValue = Request.Cookies("PA-" & ModuleID.ToString() & "-Currency").Value
                End If
            Else
                If (drpCurrency.Items.FindByValue(PropertySettings.Currency.ToString()) IsNot Nothing) Then
                    drpCurrency.SelectedValue = PropertySettings.Currency.ToString()
                End If
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (IsPostBack = False) Then
                BindCurrency()
            End If

        End Sub

        Protected Sub drpCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpCurrency.SelectedIndexChanged

            Response.Cookies("PA-" & ModuleID.ToString() & "-Currency").Value = drpCurrency.SelectedValue
            Response.Redirect(Request.RawUrl, True)

        End Sub

    End Class

End Namespace
