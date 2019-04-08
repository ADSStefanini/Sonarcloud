Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos

Public Class TipoCarteraDAL
    Inherits AccesObject(Of TipoCartera)
    Dim _auditLog As LogAuditoria
    Dim db As UGPPEntities
    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    Public Function consultarTiposCartera(codProcedencia As Int32) As List(Of TipoCartera)
        Return ExecuteList("SP_TIPOS_CARTERA_POR_PROCEDENCIA", New SqlClient.SqlParameter("@COD_PROCEDENCIA", codProcedencia))
    End Function

End Class
