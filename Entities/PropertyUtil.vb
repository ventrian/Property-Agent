Namespace Ventrian.PropertyAgent

    Public Class PropertyUtil

#Region " Public Methods "

        Public Shared Function FormatPropertyLabel(ByVal key As String, ByVal propertySettings As PropertySettings) As String

            Try

                If Not (propertySettings Is Nothing) Then

                    key = key.Replace("[PROPERTYLABEL]", propertySettings.PropertyLabel)
                    key = key.Replace("[PROPERTYPLURALLABEL]", propertySettings.PropertyPluralLabel)
                    key = key.Replace("[PROPERTYTYPELABEL]", propertySettings.PropertyTypeLabel)
                    key = key.Replace("[PROPERTYTYPEPLURALLABEL]", propertySettings.PropertyTypePluralLabel)
                    key = key.Replace("[LOCATIONLABEL]", propertySettings.LocationLabel)
                    key = key.Replace("[AGENTLABEL]", propertySettings.AgentLabel)
                    key = key.Replace("[AGENTPLURALLABEL]", propertySettings.AgentPluralLabel)
                    key = key.Replace("[BROKERLABEL]", propertySettings.BrokerLabel)
                    key = key.Replace("[BROKERPLURALLABEL]", propertySettings.BrokerPluralLabel)

                End If

            Catch
            End Try

            Return key

        End Function

#End Region

    End Class

End Namespace