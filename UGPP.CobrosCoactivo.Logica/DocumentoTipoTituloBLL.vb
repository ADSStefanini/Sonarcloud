Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos

Public Class DocumentoTipoTituloBLL
    Private Property _DocumentoTipoTituloDAL As DocumentoTipoTituloDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _DocumentoTipoTituloDAL = New DocumentoTipoTituloDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _DocumentoTipoTituloDAL = New DocumentoTipoTituloDAL(_AuditEntity)
    End Sub

    Public Function consultarDocumentosPorTipo(codTipoTitulo As String) As List(Of DocumentoTipoTitulo)
        Return _DocumentoTipoTituloDAL.consultarDocumentosPorTipo(codTipoTitulo)
    End Function

End Class
