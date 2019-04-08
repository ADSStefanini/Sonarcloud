Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class DocumentoTituloTipoTituloDAL
    Inherits AccesObject(Of DocumentoTituloTipoTitulo)

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
    ''' Obtiene la lista de tipos de titulo
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerTiposTitulo() As List(Of Datos.TIPOS_TITULO)
        Return db.TIPOS_TITULO.ToList()
    End Function

    ''' <summary>
    ''' obtiene los documentos titulo
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerDocumentosTitulo() As List(Of Datos.DOCUMENTO_TITULO)
        Return db.DOCUMENTO_TITULO.ToList()
    End Function
    ''' <summary>
    ''' Obtiene los tipos de identificación
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerTiposIdentificacion() As List(Of TIPOS_IDENTIFICACION)
        Return db.TIPOS_IDENTIFICACION.ToList()
    End Function

    ''' <summary>
    ''' Obtiene los tipos de identificación
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerTiposIdentificacionPorId(ByVal codigo As String) As TIPOS_IDENTIFICACION
        Return db.TIPOS_IDENTIFICACION.Where(Function(x) x.codigo = codigo).FirstOrDefault()
    End Function

    ''' <summary>
    ''' Obtiene toda las relaciones existentes
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerDocumentoTituloTipoTitulo() As List(Of Entidades.DocumentoTituloTipoTitulo)
        Return ExecuteList("SP_DOCUMENTOS_TITULO_TIPO_TITULO")
    End Function

    ''' <summary>
    ''' Persiste los datos en la tabla relación
    ''' </summary>
    ''' <param name="tipo"></param>
    ''' <returns></returns>
    Public Function salvar(ByVal tipo As DocumentoTituloTipoTitulo) As Boolean
        If (_auditLog IsNot Nothing) Then
            Dim logProcess As New LogProcesoDAL
            logProcess.saveAuditoria(New LOG_AUDITORIA With {.LOG_APLICACION = _auditLog.LOG_APLICACION, .LOG_CONSULTA = "EXECUTE SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO", .LOG_DOC_AFEC = _auditLog.LOG_DOC_AFEC, .LOG_FECHA = _auditLog.LOG_FECHA, .LOG_HOST = _auditLog.LOG_HOST,
                                     .LOG_IP = _auditLog.LOG_IP, .LOG_USER_CC = _auditLog.LOG_USER_CC, .LOG_MODULO = _auditLog.LOG_MODULO, .LOG_USER_ID = _auditLog.LOG_USER_ID})
        End If
        ExecuteCommand("SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO", New SqlClient.SqlParameter("COD_TIPO_TITULO", tipo.COD_TIPO_TITULO), New SqlClient.SqlParameter("ID_DOCUMENTO_TITULO", tipo.ID_DOCUMENTO_TITULO), New SqlClient.SqlParameter("VAL_ESTADO", tipo.VAL_ESTADO), New SqlClient.SqlParameter("VAL_OBLIGATORIO", tipo.VAL_OBLIGATORIO))
        Return True
    End Function
End Class
