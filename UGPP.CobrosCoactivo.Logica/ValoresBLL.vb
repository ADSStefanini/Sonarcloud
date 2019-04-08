Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades


Public Class ValoresBLL
    Private Property _ValoresDAL As ObtenDatosValoresDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _ValoresDAL = New ObtenDatosValoresDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _ValoresDAL = New ObtenDatosValoresDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Consulta la tabla de Parametrizacion de Valores con el fin de poder cargar la grilla Valores
    ''' </summary>
    ''' <returns></returns>
    Public Function ConsultarDatosValores() As List(Of Valores)
        Return _ValoresDAL.ConsultaDatValores()
    End Function

    ''' <summary>
    ''' Envia las variables de la parametrización de Valores
    ''' </summary>
    ''' <param name="ID_TIPO_OBLIGACION_VALORES"></param>
    ''' <param name="VALOR_OBLIGACION"></param>
    ''' <param name="PARTIDA_GLOBAL"></param>
    ''' <param name="SANCION_OMISION"></param>
    ''' <param name="SANCION_INEXACTITUD"></param>
    ''' <param name="SANCION_MORA"></param>
    Public Sub InsertDatValores(ByVal ID_TIPO_OBLIGACION_VALORES As String, ByVal VALOR_OBLIGACION As Boolean, ByVal PARTIDA_GLOBAL As Boolean, ByVal SANCION_OMISION As Boolean, ByVal SANCION_INEXACTITUD As Boolean, ByVal SANCION_MORA As Boolean)
        _ValoresDAL.InsertaDatValores(ID_TIPO_OBLIGACION_VALORES, VALOR_OBLIGACION, PARTIDA_GLOBAL, SANCION_OMISION, SANCION_INEXACTITUD, SANCION_MORA)
    End Sub
End Class

