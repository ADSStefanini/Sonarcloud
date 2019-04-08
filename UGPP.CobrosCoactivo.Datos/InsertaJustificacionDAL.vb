Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class InsertaJustificacionDAL
    Inherits AccesObject(Of TituloEjecutivo)

    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _AuditLog = auditLog
    End Sub

    Public Function InsertaJustificacionCierre(ByVal ID_UNICO_MT As Int64, ByVal DESC_JUSTIFICACION_CIERRE As String)
        Dim parametros As SqlClient.SqlParameter() = New SqlClient.SqlParameter() {
            New SqlClient.SqlParameter("@ID_UNICO_MT", ID_UNICO_MT),
            New SqlClient.SqlParameter("@DESC_JUSTIFICACION_CIERRE", DESC_JUSTIFICACION_CIERRE)
        }
        Return ExecuteList("SP_InsertaJustificacionCierreTit", parametros)
        Utils.ValidaLog(_AuditLog, "EXECUTE SP_InsertaJustificacionCierreTit", parametros)
    End Function
End Class