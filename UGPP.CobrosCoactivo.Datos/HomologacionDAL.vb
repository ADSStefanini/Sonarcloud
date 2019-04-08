Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class HomologacionDAL
    Inherits AccesObject(Of Entidades.Homologacion)

    Dim _Auditoria As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _Auditoria = auditLog
    End Sub

    Public Function ConsultaDatValores(ByVal fuente As String, ByVal tipo As Integer) As List(Of Homologacion)
        Return ExecuteList("SP_OBTENER_HOMOLOGACION", New SqlClient.SqlParameter("@FUENTE", fuente), New SqlClient.SqlParameter("@TIPO_HOMOLOGACION", tipo))
    End Function
End Class
