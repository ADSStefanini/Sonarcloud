Imports System.Configuration

Public Class TiposProcesosConcursalesDAL
    Inherits AccesObject(Of TIPOS_PROCESOS_CONCURSALES)


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
    ''' <returns>Retorna lista de TiposProcesosConcursales</returns>
    Public Function ObtenerTiposProcesosJuridico() As List(Of Datos.TIPOS_PROCESOS_CONCURSALES)
        Dim TiposProcesosConcursales = (From m In db.TIPOS_PROCESOS_CONCURSALES
                                        Where (m.PROCESO_JURIDICO = 1)
                                        Order By m.nombre
                                        Select m).ToList()
        Return TiposProcesosConcursales
    End Function

    Public Function ObtenerTiposProcesosNatural() As List(Of Datos.TIPOS_PROCESOS_CONCURSALES)
        Dim TiposProcesosConcursalesNatural = (From m In db.TIPOS_PROCESOS_CONCURSALES
                                               Where (m.PROCESO_NATURAL = 1)
                                               Order By m.nombre
                                               Select m).ToList()
        Return TiposProcesosConcursalesNatural
    End Function
End Class
