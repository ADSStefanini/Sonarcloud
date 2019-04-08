Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class DocumentoTituloTipoTituloBLL
    Private Property _DocumentoTituloTipoTituloDAL As DocumentoTituloTipoTituloDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        _DocumentoTituloTipoTituloDAL = New DocumentoTituloTipoTituloDAL()
    End Sub
    ''' <summary>
    ''' Sobre escritura del constructor para adicionar auditoria
    ''' </summary>
    ''' <param name="auditData"></param>
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _DocumentoTituloTipoTituloDAL = New DocumentoTituloTipoTituloDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Obtiene los tiposTitulo
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerTiposTitulo() As List(Of Entidades.TipoTitulo)
        Dim result As List(Of Datos.TIPOS_TITULO)
        result = _DocumentoTituloTipoTituloDAL.obtenerTiposTitulo()
        Dim response As New List(Of Entidades.TipoTitulo)
        For Each item In result
            response.Add(New Entidades.TipoTitulo With {.codigo = item.codigo, .nombre = item.nombre})
        Next
        Return response
    End Function
    ''' <summary>
    ''' Obtiene los documentostitulo
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerDocumentosTitulo() As List(Of Entidades.DocumentoTitulo)
        Dim result As List(Of Datos.DOCUMENTO_TITULO)
        result = _DocumentoTituloTipoTituloDAL.obtenerDocumentosTitulo()
        Dim response As New List(Of Entidades.DocumentoTitulo)
        For Each item In result
            response.Add(New Entidades.DocumentoTitulo With {.ID_DOCUMENTO_TITULO = item.ID_DOCUMENTO_TITULO, .NOMBRE_DOCUMENTO = item.NOMBRE_DOCUMENTO})
        Next
        Return response
    End Function
    ''' <summary>
    ''' Obtiene la tabla relación
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerDocumentoTituloTipoTitulo() As List(Of Entidades.DocumentoTituloTipoTitulo)
        Dim result As List(Of Entidades.DocumentoTituloTipoTitulo)
        result = _DocumentoTituloTipoTituloDAL.obtenerDocumentoTituloTipoTitulo()
        Return result
    End Function
    ''' <summary>
    ''' Guarda el objeto
    ''' </summary>
    ''' <param name="tipo"></param>
    ''' <returns></returns>
    Public Function salvar(ByVal tipo As Entidades.DocumentoTituloTipoTitulo) As Boolean
        Try
            Return _DocumentoTituloTipoTituloDAL.salvar(tipo)
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Obtiene los tipos de identificación
    ''' </summary>
    ''' <returns>lista de tipoidentificacion</returns>
    Public Function obtenerTiposIdentificaciones() As List(Of TipoIdentificacion)
        Dim resultData As List(Of TIPOS_IDENTIFICACION)
        resultData = _DocumentoTituloTipoTituloDAL.obtenerTiposIdentificacion()
        Dim result As New List(Of TipoIdentificacion)
        For Each item In resultData
            result.Add(New TipoIdentificacion With {.codigo = item.codigo, .nombre = item.nombre})
        Next
        Return result
    End Function
End Class
