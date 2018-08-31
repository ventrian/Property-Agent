Imports System.Net
Imports System.Xml
Imports System.Globalization
Imports System.IO

Namespace Ventrian.PropertyAgent.Mapping

    Public Class Geocoder
        ' Methods
        Public Shared Function GeoCode(ByVal location As String, ByVal objPropertySettings As PropertySettings) As GeocodeInfo
            Dim tmpKey As String = objPropertySettings.MapKey
            Dim tmpLocation As String = objPropertySettings.DistanceExpression.Replace("[LOCATION]", location)
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(("https://maps.googleapis.com/maps/api/geocode/xml?address=" & HttpUtility.UrlEncode(tmpLocation) & "&sensor=false&key=" & tmpKey)), HttpWebRequest)
            Dim response As HttpWebResponse = DirectCast(request.GetResponse, HttpWebResponse)
            Dim objGeoCodeInfo As New GeocodeInfo With {
                .Latitude = 0,
                .Longitude = 0
            }
            If (response.StatusCode = HttpStatusCode.OK) Then
                Dim reader As New StreamReader(response.GetResponseStream)
                Dim doc As New XmlDocument
                doc.Load(reader)
                Dim nsmgr As New XmlNamespaceManager(doc.NameTable)
                Dim root As XmlElement = doc.DocumentElement
                Dim status As XmlNode = doc.DocumentElement.SelectSingleNode("result/geometry/location/lat", nsmgr)
                If (doc.DocumentElement.SelectSingleNode("status", nsmgr).InnerText = "OK") Then
                    objGeoCodeInfo.Latitude = Convert.ToDouble(Double.Parse(doc.DocumentElement.SelectSingleNode("result/geometry/location/lat", nsmgr).InnerText, CultureInfo.InvariantCulture.NumberFormat))
                    objGeoCodeInfo.Longitude = Convert.ToDouble(Double.Parse(doc.DocumentElement.SelectSingleNode("result/geometry/location/lng", nsmgr).InnerText, CultureInfo.InvariantCulture.NumberFormat))
                End If
                reader.Close()
                reader = Nothing
            End If
            Return objGeoCodeInfo
        End Function

    End Class

    'Public Class Geocoder

    '    'http://code.google.com/apis/maps/documentation/reference.html
    '    Shared Function GeoCode(ByVal location As String, ByVal objPropertySettings As PropertySettings) As GeocodeInfo

    '        Dim tmpKey As String = objPropertySettings.MapKey

    '        Dim tmpLatLong As String = "0,0"
    '        Dim url As String = "https://maps.google.com/maps/geo?q=" & Replace(objPropertySettings.DistanceExpression.Replace("[LOCATION]", location), " ", "+") & "&output=xml&key=" & tmpKey

    '        Dim request As HttpWebRequest = WebRequest.Create(url)
    '        Dim response As HttpWebResponse = request.GetResponse

    '        'If status = 200 (OK), then load it up
    '        If response.StatusCode = HttpStatusCode.OK Then
    '            Dim reader As New IO.StreamReader(response.GetResponseStream())
    '            Dim doc As New XmlDocument()

    '            doc.Load(reader)

    '            'the status returned in the XML also needs to be 200
    '            'this is different from an HTTP status code
    '            'it tells if the geocoding was sucessful
    '            If doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(1).ChildNodes.Item(0).InnerText = "200" Then

    '                'go straight to the XML for the 
    '                tmpLatLong = doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(2).ChildNodes.Item(3).InnerText

    '            End If

    '            reader.Close()
    '            reader = Nothing
    '        End If

    '        If (tmpLatLong = "") Then
    '            tmpLatLong = "0,0"
    '        End If

    '        Dim objGeoCodeInfo As New GeocodeInfo
    '        objGeoCodeInfo.Latitude = Convert.ToDouble(Double.Parse(tmpLatLong.Split(","c)(1), CultureInfo.InvariantCulture.NumberFormat))
    '        objGeoCodeInfo.Longitude = Convert.ToDouble(Double.Parse(tmpLatLong.Split(","c)(0), CultureInfo.InvariantCulture.NumberFormat))

    '        Return objGeoCodeInfo

    '    End Function

    'End Class

End Namespace
