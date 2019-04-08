Imports System.Configuration

Public Class UvtDAL
    Inherits AccesObject(Of UVT)


    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>Retorna valor año actual UVT</returns>
    Public Function ObtenerUvtAnioActual() As List(Of Datos.UVT)
        Dim anioActual = (From m In db.UVT
                          Where m.Activo = "1"
                          Order By m.Anio
                          Select m).ToList
        Return anioActual
    End Function
End Class
