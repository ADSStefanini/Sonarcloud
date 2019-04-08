Imports System.Configuration
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades


Public Class ObtenDatosValoresBLL
    Private Property _ObtenDatosValoresDAL As ObtenDatosValoresDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _ObtenDatosValoresDAL = New ObtenDatosValoresDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _ObtenDatosValoresDAL = New ObtenDatosValoresDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Realiza el llamado al stored procedure que realiza el update de la parametrización de valores
    ''' </summary>
    ''' <param name="ID_TIPO_OBLIGACION_VALORES"></param>
    ''' <param name="VALOR_OBLIGACION"></param>
    ''' <param name="PARTIDA_GLOBAL"></param>
    ''' <param name="SANCION_OMISION"></param>
    ''' <param name="SANCION_INEXACTITUD"></param>
    ''' <param name="SANCION_MORA"></param>
    Public Sub InsertaDatValores(ByVal ID_TIPO_OBLIGACION_VALORES As String, ByVal VALOR_OBLIGACION As Boolean, ByVal PARTIDA_GLOBAL As Boolean, ByVal SANCION_OMISION As Boolean, ByVal SANCION_INEXACTITUD As Boolean, ByVal SANCION_MORA As Boolean)
        _ObtenDatosValoresDAL.InsertaDatValores(ID_TIPO_OBLIGACION_VALORES, VALOR_OBLIGACION, PARTIDA_GLOBAL, SANCION_OMISION, SANCION_INEXACTITUD, SANCION_MORA)
    End Sub

    Public Function ConsultaDatValores() As List(Of Valores)
        Return _ObtenDatosValoresDAL.ConsultaDatValores()
    End Function
End Class
