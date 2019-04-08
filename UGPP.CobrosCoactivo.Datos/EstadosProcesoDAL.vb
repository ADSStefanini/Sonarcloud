Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class EstadosProcesoDAL
    Inherits AccesObject(Of Entidades.EstadosProceso)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _AuditLog = auditLog
    End Sub

    ''' <summary>
    ''' Retorna la lista de los estados procesales
    ''' </summary>
    ''' <returns>Lista de objetos Datos.ESTADOS_PROCESO</returns>
    Public Function obtenerEstadosProcesos() As List(Of Datos.ESTADOS_PROCESO)
        Dim estadosProceso = (From ep In db.ESTADOS_PROCESO
                              Order By ep.nombre
                              Select ep).ToList()
        Return estadosProceso
    End Function

End Class
