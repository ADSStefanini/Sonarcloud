Imports AutoMapper
Imports UGPP.CobrosCoactivo.Entidades
Public Class TraductorAuditoriaBLL
    Private Property _DiccionarioDal As Datos.DiccionarioAuditoriaDAL
    Private Property _Auditoria As Entidades.LogAuditoria
    Public Sub New(ByVal auditoriaLog As Entidades.LogAuditoria)
        _DiccionarioDal = New Datos.DiccionarioAuditoriaDAL
        _Auditoria = auditoriaLog
    End Sub
    Public Sub New()
        _DiccionarioDal = New Datos.DiccionarioAuditoriaDAL
    End Sub
    ''' <summary>
    ''' Obtiene el diccionario para la auditoria
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Public Function obtenerDiccionarioPorLlave(ByVal llave As String) As List(Of DiccionarioAditoria)
        Dim resultadoDAL = _DiccionarioDal.obtenerDiccionarioPorLlave(llave)

        Dim resultado As New List(Of DiccionarioAditoria)
        For Each item In resultadoDAL
            resultado.Add(New DiccionarioAditoria With {.ACTIVO = item.ACTIVO, .VALOR_DESTINO = item.VALOR_DESTINO, .VALOR_ORIGINAL = item.VALOR_ORIGINAL})
        Next
        Return resultado
    End Function
    ''' <summary>
    ''' Obtiene los registros de diccionario segun un like
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Public Function obtenerDiccionarioLike(ByVal llave As String) As List(Of DiccionarioAditoria)
        Dim resultadoDAL = _DiccionarioDal.obtenerDiccionarioLike(llave)
        Dim resultado As New List(Of DiccionarioAditoria)
        For Each item In resultadoDAL
            resultado.Add(New DiccionarioAditoria With {.ACTIVO = item.ACTIVO, .VALOR_DESTINO = item.VALOR_DESTINO, .VALOR_ORIGINAL = item.VALOR_ORIGINAL})
        Next
        Return resultado
    End Function
    ''' <summary>
    ''' Obtiene todas las llaves del disicionario
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerDiccionario() As List(Of DiccionarioAditoria)
        Dim resultadoDAL = _DiccionarioDal.obtenerDiccionario()
        Dim resultado As New List(Of DiccionarioAditoria)
        For Each item In resultadoDAL
            resultado.Add(New DiccionarioAditoria With {.ACTIVO = item.ACTIVO, .VALOR_DESTINO = item.VALOR_DESTINO, .VALOR_ORIGINAL = item.VALOR_ORIGINAL})
        Next
        Return resultado
    End Function
    ''' <summary>
    ''' Guarda o actualiza el diccionario
    ''' </summary>
    ''' <param name="diccionario"></param>
    ''' <returns></returns>
    Public Function salvarDiccionario(ByVal diccionario As DiccionarioAditoria) As Boolean
        Dim diccionarioData As New Datos.DICCIONARIO_AUDITORIA
        diccionarioData.ACTIVO = diccionario.ACTIVO
        diccionarioData.VALOR_DESTINO = diccionario.VALOR_DESTINO
        diccionarioData.VALOR_ORIGINAL = diccionario.VALOR_ORIGINAL
        Dim data As New Datos.DiccionarioAuditoriaDAL(_Auditoria)
        If Me.obtenerDiccionarioPorLlave(diccionario.VALOR_ORIGINAL).FirstOrDefault() IsNot Nothing Then
            data.actualizarDiccionario(diccionarioData)
        Else
            data.guardarDiccionario(diccionarioData)
        End If
        Return True
    End Function
End Class
