Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class DocumentoTipoTituloDAL
    Inherits AccesObject(Of DocumentoTipoTitulo)

    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _AuditLog = auditLog
    End Sub

    Public Function consultarDocumentosPorTipo(codTipoTitulo As String) As List(Of DocumentoTipoTitulo)
        Return ExecuteList("SP_DOCUMENTOS_TIPO_TITULO", New SqlClient.SqlParameter("@COD_TIPO_TITULO", codTipoTitulo))
    End Function

End Class
