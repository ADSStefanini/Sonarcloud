Imports UGPP.CobrosCoactivo.Entidades

Public Class HistoricoClasificacionManualDAL

    Dim _Auditoria As LogAuditoria
    Dim db As UGPPEntities

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        db = New UGPPEntities()
    End Sub


    Public Sub New(ByVal auditLog As LogAuditoria)
        db = New UGPPEntities()
        _Auditoria = auditLog
    End Sub


    ''' <summary>
    ''' guarda el historico en la base de datos
    ''' </summary>
    ''' <returns></returns>
    Public Function Salvar(ByVal historico As Datos.HISTORICO_CLASIFICACION_MANUAL) As Boolean
        Try
            db.HISTORICO_CLASIFICACION_MANUAL.Add(historico)
            Utils.salvarDatos(db)
            Dim array As ArrayList = New ArrayList
            Dim list As List(Of Char) = historico.ToString.ToList
            For Each item In list
                array.Add(item)
            Next
            Utils.ValidaLog(_Auditoria, "INSERT INTO HISTORICO_CLASIFICACION_MANUAL ", array)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Retorna una lista de historico por un idexpediente
    ''' </summary>
    ''' <param name="idExpediente"></param>
    ''' <returns></returns>
    Public Function ObtenerHistoricoPorIdExpediente(ByVal idExpediente As String) As List(Of Datos.HISTORICO_CLASIFICACION_MANUAL)
        Return db.HISTORICO_CLASIFICACION_MANUAL.Where(Function(x) x.ID_EXPEDIENTE = idExpediente).ToList
    End Function
End Class
