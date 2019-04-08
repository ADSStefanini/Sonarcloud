Public Class DocumentoTipoTitulo
    Public Sub New()
        Observacion = New ObservacionesCNCDoc()
    End Sub

    Public Property ID_DOCUMENTO_TITULO As Int32
    Public Property NOMBRE_DOCUMENTO As String
    Public Property COD_TIPO_TITULO As String
    Public Property VAL_ESTADO As Boolean
    Public Property VAL_OBLIGATORIO As Boolean
    'Propiedades solo usadas en la vista
    Public Property DES_RUTA_DOCUMENTO As String
    Public Property ID_MAESTRO_DOCUMENTO As Int64
    Public Property COD_GUID As String
    Public Property COD_TIPO_DOCUMENTO_AO As String
    Public Property NOM_DOC_AO As String
    Public Property OBSERVA_LEGIBILIDAD As String
    Public Property NUM_PAGINAS As Int32
    Public Property IND_DOC_SINCRONIZADO As Boolean
    Public Property Observacion As ObservacionesCNCDoc

End Class
