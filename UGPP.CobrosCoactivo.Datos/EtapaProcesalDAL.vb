Imports System.Configuration
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Public Class EtapaProcesalDAL
    Inherits AccesObject(Of EtapaProcesal)

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
    ''' 
    ''' </summary>
    ''' <returns>Retorna lista de Etapa Procesal</returns>
    Public Function ObtenerEtapasProcesales() As List(Of Datos.ETAPA_PROCESAL)
        Dim EtapasProcesales = (From m In db.ETAPA_PROCESAL
                                Order By m.VAL_ETAPA_PROCESAL
                                Select m).ToList()
        Return EtapasProcesales
    End Function

    Public Function ObtenerEtapasProcesalesPorId(ByVal Id As String) As List(Of Datos.ETAPA_PROCESAL)
        Dim EtapasProcesales = (From m In db.ETAPA_PROCESAL Where m.codigo = Id
                                Order By m.VAL_ETAPA_PROCESAL
                                Select m).ToList()
        Return EtapasProcesales
    End Function
End Class
