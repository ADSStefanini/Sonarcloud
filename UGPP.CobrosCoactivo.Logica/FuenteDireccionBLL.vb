Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports System.Configuration

Public Class FuenteDireccionBLL
    Private Property _FuenteDireccionDAL As FuenteDireccionDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _FuenteDireccionDAL = New FuenteDireccionDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _FuenteDireccionDAL = New FuenteDireccionDAL(_AuditEntity)
    End Sub

    Public Function ConsultarFuentes() As List(Of FuenteDireccion)
        Return _FuenteDireccionDAL.ConsultarFuentes()
    End Function
End Class
