Imports System.Configuration
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Public Class DeudoresExpedienteDAL
    Inherits AccesObject(Of DeudoresExpediente)

    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _AuditLog = auditLog
    End Sub

    ''' <summary>
    ''' Trae los deudores de un expediente o titulo 
    ''' </summary>
    ''' <returns></returns>
    Public Function ConsultaDeudoresTituloExp(Optional ByVal NumeroTitulo As Int64 = Nothing) As List(Of DeudoresExpediente)
        Return ExecuteList("SP_ConsultarDeudoresExpedienteTitulo",
                New SqlClient.SqlParameter("@NROTITULO", NumeroTitulo)
                )
    End Function
End Class
