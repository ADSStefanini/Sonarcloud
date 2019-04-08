Imports System.Configuration
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades
Public Class RelacionEP_EPDAL
    Inherits AccesObject(Of RELACION_ESTADO_ETAPA
        )
    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria
    Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    Public Function ConsultaEstadoEtapaPorID(ByVal codigo_estado As String, ByVal codigo_etapa As Int32) As Int32
        Dim retorno As Int32 = db.RELACION_ESTADO_ETAPA.Where(Function(x) (x.codigo_estado = codigo_estado And x.codigo_etapa = codigo_etapa)).Count
        Return retorno
    End Function
    Public Sub InsertarEstadoEtapa(ByVal codigo_estado As String, ByVal codigo_etapa As Int32)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim Insercion As RELACION_ESTADO_ETAPA = New RELACION_ESTADO_ETAPA
        Insercion.codigo_estado = codigo_estado
        Insercion.codigo_etapa = codigo_etapa
        db.RELACION_ESTADO_ETAPA.Add(Insercion)
        db.SaveChanges()
        Parameters.Add(New SqlClient.SqlParameter("@codigo_estado", codigo_estado))
        Parameters.Add(New SqlClient.SqlParameter("@codigo_etapa", codigo_etapa))
        Utils.ValidaLog(_auditLog, "INSERT RELACION_ESTADO_ETAPA ", Parameters.ToArray)
    End Sub
    Public Sub EliminarRegistro(ByVal codigo_estado As String, ByVal codigo_etapa As Int32)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim borrado As RELACION_ESTADO_ETAPA = db.RELACION_ESTADO_ETAPA.Where(Function(x) (x.codigo_estado = codigo_estado And x.codigo_etapa = codigo_etapa)).First
        db.RELACION_ESTADO_ETAPA.Remove(borrado)
        Parameters.Add(New SqlClient.SqlParameter("@codigo_estado", codigo_estado))
        Parameters.Add(New SqlClient.SqlParameter("@codigo_etapa", codigo_etapa))
        Utils.ValidaLog(_auditLog, "DELETE RELACION_ESTADO_ETAPA ", Parameters.ToArray)
        Utils.salvarDatos(db)
    End Sub
End Class
