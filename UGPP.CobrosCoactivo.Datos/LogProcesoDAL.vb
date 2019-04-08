Imports System.Text

Public Class LogProcesoDAL
    Dim db As UGPPEntities
    Private Property _DiccionarioDal As Datos.DiccionarioAuditoriaDAL
    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        db = New UGPPEntities()
        _DiccionarioDal = New DiccionarioAuditoriaDAL
    End Sub
    ''' <summary>
    ''' Salva la auditoria
    ''' </summary>
    ''' <returns></returns>
    Public Function saveAuditoria(ByVal auditoriaLog As LOG_AUDITORIA) As Boolean
        Try
            auditoriaLog.LOG_NEGOCIO = IIf(auditoriaLog.LOG_NEGOCIO Is Nothing, String.Empty, auditoriaLog.LOG_NEGOCIO)
            auditoriaLog.LOG_NEGOCIO = GenerarComandoNegocio(auditoriaLog.LOG_NEGOCIO)
            db.LOG_AUDITORIA.Add(auditoriaLog)
            Utils.salvarDatos(db)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Genera el comando de negocio
    ''' </summary>
    ''' <param name="comando"></param>
    ''' <returns></returns>
    Public Function GenerarComandoNegocio(ByVal comando As String) As String
        comando = comando.Replace("(", " ")
        comando = comando.Replace(")", " ")
        comando = comando.Replace("[", " ")
        comando = comando.Replace("]", " ")
        comando = comando.Replace(",", " ")
        comando = comando.Replace(";", " ")
        Dim arrayPalabras As String()
        arrayPalabras = comando.Split(" ")
        Dim resultado As New StringBuilder
        For Each word In arrayPalabras
            If word IsNot String.Empty Then
                resultado.Append(String.Concat(Me.Traductor(word), " "))
            End If
        Next
        Return resultado.ToString()
    End Function

    ''' <summary>
    ''' Realiza la traducción de los comandos a descripciones de negocio
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Public Function Traductor(ByVal llave As String) As String
        Dim resultado As String
        Dim entidad As New Datos.DICCIONARIO_AUDITORIA
        entidad = _DiccionarioDal.obtenerDiccionario().Where(Function(x) x.VALOR_ORIGINAL = llave.ToUpper).FirstOrDefault()
        If entidad IsNot Nothing Then
            resultado = entidad.VALOR_DESTINO
        Else
            resultado = llave
        End If
        Return resultado
    End Function
End Class
