Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class AlmacenamientoTemporalDAL
    Inherits AccesObject(Of AlmacenamientoTemporal)

    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _AuditLog = auditLog
    End Sub

    Public Function consultarAlmacenamientoPorId(idAlamacenamiento As Long) As AlmacenamientoTemporal
        Return ExecuteList("SP_ALMACENAMIENTO_TEMPORAL_POR_IDTAREA", New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", idAlamacenamiento)).FirstOrDefault()
    End Function

    Public Sub actualizarAlmacenamiento(prmAlmacenamientoTemporal As AlmacenamientoTemporal)
        Dim parametros As SqlClient.SqlParameter() = New SqlClient.SqlParameter() {
            New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", prmAlmacenamientoTemporal.ID_TAREA_ASIGNADA),
            New SqlClient.SqlParameter("@JSON_OBJ", prmAlmacenamientoTemporal.JSON_OBJ)
        }
        ExecuteCommand("SP_ALMACENAMIENTO_TEMPORAL_ACTUALIZAR", parametros)
        Utils.ValidaLog(_AuditLog, "EXECUTE SP_ALMACENAMIENTO_TEMPORAL_ACTUALIZAR", parametros)
    End Sub

    Public Function InsertarAlmacenamiento(prmAlmacenamientoTemporal As AlmacenamientoTemporal) As AlmacenamientoTemporal
        Dim parametros As SqlClient.SqlParameter() = New SqlClient.SqlParameter() {
            New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", prmAlmacenamientoTemporal.ID_TAREA_ASIGNADA),
            New SqlClient.SqlParameter("@JSON_OBJ", prmAlmacenamientoTemporal.JSON_OBJ)
        }
        Return ExecuteList("SP_ALMACENAMIENTO_TEMPORAL_INSERTAR", parametros).FirstOrDefault()

        Utils.ValidaLog(_AuditLog, "EXECUTE SP_ALMACENAMIENTO_TEMPORAL_INSERTAR", parametros)
    End Function
End Class
