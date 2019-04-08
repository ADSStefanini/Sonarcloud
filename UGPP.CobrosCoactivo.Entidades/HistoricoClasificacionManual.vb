Public Class HistoricoClasificacionManual
    Public Property ID_REGISTRO_CLASIFICACION_MANUAL As Integer
    Public Property ID_EXPEDIENTE As String
    Public Property ID_USUARIO As String
    Public Property FECHA As Date
    Public Property PERSONA_JURIDICA As Nullable(Of Boolean)
    Public Property PERSONA_NATURAL As Nullable(Of Boolean)
    Public Property PERSONA_VIVA As Nullable(Of Boolean)
    Public Property MATRICULA_MERCANTIL As Nullable(Of Boolean)
    Public Property ID_MTD_DOCUMENTO As Nullable(Of Long)
    Public Property PROCESO_ESPECIAL As Nullable(Of Boolean)
    Public Property TIPO_PROCESO As Nullable(Of Integer)
    Public Property BENEFICIO_TRIBUTARIO As Nullable(Of Boolean)
    Public Property PAGOS_DEUDOR As Nullable(Of Boolean)
    Public Property NUMERO_RADICADO As Nullable(Of Integer)
    Public Property OBSERVACIONES As String
    Public Property VALOR_MENOR_UVT As Nullable(Of Boolean)

    Public Overridable Property EJEFISGLOBAL As EJEFISGLOBAL
End Class
