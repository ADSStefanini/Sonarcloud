Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Public Class PagosDAL
    Inherits AccesObject(Of Entidades.Pagos)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria
    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    ''' <summary>
    ''' Consulta pagos
    ''' </summary>
    ''' <param name="idExpediente">Id unico a consultar</param>
    ''' <returns>Lista de objetos del tipo Datos.PAGOS</returns>
    Public Function consultarPagosPorIdExpediente(ByVal idExpediente As String) As List(Of Datos.PAGOS)
        Dim pagos = (From p In db.PAGOS
                     Where p.NroExp = idExpediente
                     Select p).ToList()
        Return pagos
    End Function
End Class
