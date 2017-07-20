Imports System.Net
Imports System.IO
Imports System.Xml

Namespace Ventrian.PropertyAgent.Mapping

    Public Class UserGeoLocator

        Private Const ApiUrl As String = "http://api.ipinfodb.com/v2/ip_query.php?ip={0}&key={1}"
        Private Const Key As String = "45d4da98209056c5daa084bd174bd3e7c44d62d7c92290fc3c97f5d1f91eaae7"

        Public Shared Function GetUserLocation(ByVal ipAddress As String) As GeocodeInfo
            If (HttpContext.Current.Session("PA-GeocodeInfo") IsNot Nothing) Then
                Return CType(HttpContext.Current.Session("PA-GeocodeInfo"), GeocodeInfo)
            End If
            If String.IsNullOrEmpty(ipAddress) Then
                Return Nothing
            End If
            Dim reqUrl As String = String.Format(ApiUrl, ipAddress, Key)
            Dim httpReq As HttpWebRequest = TryCast(HttpWebRequest.Create(reqUrl), HttpWebRequest)
            Try
                Dim result As String = String.Empty
                Dim response As HttpWebResponse = TryCast(httpReq.GetResponse(), HttpWebResponse)
                Using reader = New StreamReader(response.GetResponseStream())
                    result = reader.ReadToEnd()
                End Using
                Dim objGeocodeInfo As GeocodeInfo = ProcessResponse(result)
                Return objGeocodeInfo
            Catch ex As Exception
                HttpContext.Current.Session("PA-GeocodeInfo") = New GeocodeInfo()
                Return Nothing
            End Try
        End Function

        Private Shared Function ProcessResponse(ByVal strResp As String) As GeocodeInfo

            Dim respElement As New XmlDocument()
            respElement.LoadXml(strResp)

            Dim objLatitude As XmlNode = respElement.SelectSingleNode("/Response/Latitude")
            Dim objLongitude As XmlNode = respElement.SelectSingleNode("/Response/Longitude")

            Dim objGeocodeInfo As New GeocodeInfo

            objGeocodeInfo.Latitude = Convert.ToDouble(objLatitude.InnerText)
            objGeocodeInfo.Longitude = Convert.ToDouble(objLongitude.InnerText)

            Return objGeocodeInfo

        End Function
    End Class
End Namespace
