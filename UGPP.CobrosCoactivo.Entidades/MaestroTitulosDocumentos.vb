Public Class MaestroTitulosDocumentos

    Public Property ID_MAESTRO_TITULOS_DOCUMENTOS As Long
    Public Property ID_DOCUMENTO_TITULO As Nullable(Of Integer)
    Public Property ID_MAESTRO_TITULO As Nullable(Of Long)
    Public Property DES_RUTA_DOCUMENTO As String
    Public Property TIPO_RUTA As Nullable(Of Integer)
    Public Property COD_GUID As String
    Public Property COD_TIPO_DOCUMENTO_AO As String
    Public Property NOM_DOC_AO As String
    Public Property OBSERVA_LEGIBILIDAD As String
    Public Property NUM_PAGINAS As Nullable(Of Integer)
    Public Property IND_DOC_SINCRONIZADO As Nullable(Of Boolean)

    Public Property MAESTRO_TITULOS As MaestroTitulos
End Class
