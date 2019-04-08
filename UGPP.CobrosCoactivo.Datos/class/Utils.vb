Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Public Class Utils

    ''' <summary>
    ''' Metódo para guardar los datos modificados en la base de datos
    ''' </summary>
    ''' <returns>Verdadero si OK, falso si error</returns>
    Public Shared Function salvarDatos(ByVal db As UGPPEntities) As Boolean
        'Salvar datos
        Try
            'db.Database.Log = Function(s)
            '                      Debug.Print(s)
            '                  End Function
            'Dim logger = New MyLogger()
            'db.Database.Log = Function(s) logger.Log("", "")
            db.SaveChanges()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Sub ValidaLog(log As LogAuditoria, consulta As String, ParamArray propiedades As SqlParameter())
        For Each item As SqlParameter In propiedades
            If item.Value IsNot Nothing Then
                consulta = consulta & " " & item.ParameterName & " " & item.Value
            End If
            If item IsNot propiedades.Last() Then
                consulta = consulta & ", "
            End If
        Next
        If (log IsNot Nothing) Then
            Dim logProcess As New LogProcesoDAL
            logProcess.saveAuditoria(New LOG_AUDITORIA With {.LOG_APLICACION = log.LOG_APLICACION, .LOG_CONSULTA = consulta, .LOG_DOC_AFEC = log.LOG_DOC_AFEC, .LOG_FECHA = log.LOG_FECHA, .LOG_HOST = log.LOG_HOST,
            .LOG_IP = log.LOG_IP, .LOG_USER_CC = log.LOG_USER_CC, .LOG_MODULO = log.LOG_MODULO, .LOG_USER_ID = log.LOG_USER_ID})
        End If
    End Sub

    Public Shared Sub ValidaLog(log As LogAuditoria, consulta As String, ParamArray propiedades As ArrayList())
        For Each item As ArrayList In propiedades
            If item IsNot Nothing Then
                consulta = consulta & " " & item.ToString & ", "
            End If
        Next
        If (log IsNot Nothing) Then
            Dim logProcess As New LogProcesoDAL
            logProcess.saveAuditoria(New LOG_AUDITORIA With {.LOG_APLICACION = log.LOG_APLICACION, .LOG_CONSULTA = consulta, .LOG_DOC_AFEC = log.LOG_DOC_AFEC, .LOG_FECHA = log.LOG_FECHA, .LOG_HOST = log.LOG_HOST,
            .LOG_IP = log.LOG_IP, .LOG_USER_CC = log.LOG_USER_CC, .LOG_MODULO = log.LOG_MODULO, .LOG_USER_ID = log.LOG_USER_ID})
        End If
    End Sub
End Class

Public Class MyLogger
    Public Sub Log(ByVal component As String, ByVal message As String)
        Console.WriteLine("Component: {0} Message: {1} ", component, message)
    End Sub
End Class
