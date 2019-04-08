Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos

Public Class TipoCarteraBLL
    Private Property _TipoCarteraDAL As TipoCarteraDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _TipoCarteraDAL = New TipoCarteraDAL()
    End Sub
    Public Sub Auditoria(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _TipoCarteraDAL = New TipoCarteraDAL(_AuditEntity)
    End Sub
    Public Function consultarTiposCartera(codProcedencia As Int32) As List(Of TipoCartera)
        Return _TipoCarteraDAL.consultarTiposCartera(codProcedencia)
    End Function
End Class
