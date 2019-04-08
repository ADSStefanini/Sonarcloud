Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class EntesDeudoresDAL
    Inherits AccesObject(Of Entidades.EntesDeudores)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _AuditLog = auditLog
    End Sub

    ''' <summary>
    ''' Retorna los deudores relacionados con un título
    ''' </summary>
    ''' <param name="prmIntTitulo">Número del título</param>
    ''' <returns>Lista de objetos del tipo Entidades.DeudoresExpediente)</returns>
    Public Function obtenerDeudoresPorTitulo(ByVal prmIntTitulo As Int32) As List(Of Entidades.EntesDeudores)
        Return ExecuteList("SP_OBTENER_DEUDORES_POR_TITULO", New SqlClient.SqlParameter("@ID_UNICO_TITULO", prmIntTitulo))
    End Function
End Class
