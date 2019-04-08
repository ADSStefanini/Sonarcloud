Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class DeduoresExpedienteBLL
    Private Property _DeudoresExpedienteDAL As DeudoresExpedienteDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _DeudoresExpedienteDAL = New DeudoresExpedienteDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _DeudoresExpedienteDAL = New DeudoresExpedienteDAL(_AuditEntity)
    End Sub

    Public Function ConsultaDeudoresTituloExp(ByVal NumeroTitulo As Int64) As List(Of DeudoresExpediente)
        Return _DeudoresExpedienteDAL.ConsultaDeudoresTituloExp(NumeroTitulo)
    End Function
End Class
