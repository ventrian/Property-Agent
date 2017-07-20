Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent.Mapping

    Public Class GeocodeInfo

#Region " Private Members "

        Private _latitude As Double = Null.NullDouble
        Private _longitude As Double = Null.NullDouble

#End Region

#Region " Public Properties "

        Public Property Latitude() As Double
            Get
                Return _latitude
            End Get
            Set(ByVal value As Double)
                _latitude = value
            End Set
        End Property

        Public Property Longitude() As Double
            Get
                Return _longitude
            End Get
            Set(ByVal value As Double)
                _longitude = value
            End Set
        End Property

#End Region

    End Class

End Namespace
