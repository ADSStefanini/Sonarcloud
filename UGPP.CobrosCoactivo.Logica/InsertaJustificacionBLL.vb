Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class InsertaJustificacionBLL

    Private Property _Inserta As InsertaJustificacionDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _Inserta = New InsertaJustificacionDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _Inserta = New InsertaJustificacionDAL(_AuditEntity)
    End Sub

    Public Sub InsertaJustificacionCierre(ByVal ID_UNICO_MT As Int64, ByVal DESC_JUSTIFICACION_CIERRE As String)
        _Inserta.InsertaJustificacionCierre(ID_UNICO_MT, DESC_JUSTIFICACION_CIERRE)
    End Sub
End Class
