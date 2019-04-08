Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports System.Configuration

Public Class FuenteDireccionDAL
    Inherits AccesObject(Of FuenteDireccion)

    Dim _Auditoria As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _Auditoria = auditLog
    End Sub

    Public Function ConsultarFuentes() As List(Of FuenteDireccion)
        Return ExecuteList("SP_FUENTE_DIRECCIONES")
    End Function
End Class
