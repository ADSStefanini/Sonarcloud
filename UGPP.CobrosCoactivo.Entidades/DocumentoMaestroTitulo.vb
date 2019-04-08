Public Class DocumentoMaestroTitulo
    Public Sub New()
        Observacion = New ObservacionesCNCDoc()
    End Sub
    Public Property ID_MAESTRO_TITULOS_DOCUMENTOS As Long
    Public Property ID_DOCUMENTO_TITULO As Int32
    Public Property DES_RUTA_DOCUMENTO As String
    Public Property TIPO_RUTA As Int32
    Public Property COD_GUID As String
    Public Property COD_TIPO_DOCUMENTO_AO As String
    Public Property NOM_DOC_AO As String
    Public Property OBSERVA_LEGIBILIDAD As String
    Public Property NUM_PAGINAS As Int32
    Public Property ID_MAESTRO_TITULO As Long?
    Public Property IND_DOC_SINCRONIZADO As Boolean
    'Propiedad solo de vista
    'Solo mapea la ultima obeservacion
    Public Property Observacion As ObservacionesCNCDoc

End Class