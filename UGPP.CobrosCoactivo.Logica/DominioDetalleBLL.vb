Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Public Class DominioDetalleBLL

    Private Property _DominioDetalleDAL As DominioDetalleDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _DominioDetalleDAL = New DominioDetalleDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _DominioDetalleDAL = New DominioDetalleDAL(_AuditEntity)
    End Sub

    Public Function consultarDominioPorIdDominio(dominioid As Int32) As List(Of DominioDetalle)
        Return _DominioDetalleDAL.consultarDominioPorIdDominio(dominioid)
    End Function

End Class
