Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos

Public Class DominioDetalleDAL
    Inherits AccesObject(Of DominioDetalle)

    Dim _Auditoria As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _Auditoria = auditLog
    End Sub

    Public Function consultarDominioPorIdDominio(dominioid As Int32) As List(Of DominioDetalle)
        Return ExecuteList("SP_DOMINIO_DETALLE_DOMINIOID", New SqlClient.SqlParameter("@ID_DOMINIO", dominioid))
    End Function

End Class
