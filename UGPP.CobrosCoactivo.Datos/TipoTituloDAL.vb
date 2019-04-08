Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Public Class TipoTituloDAL
    Inherits AccesObject(Of TipoTitulo)
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria
    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    Public Function consultarTipoTitulo(codTipoArea As Int32) As List(Of TipoTitulo)
        Return ExecuteList("SP_TIPOS_TITULO_POR_AREA", New SqlClient.SqlParameter("@COD_PROCEDENCIA", codTipoArea))
    End Function

End Class
