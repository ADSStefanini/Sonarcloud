Public Class DocumentoTitulo
    Public Property ID_DOCUMENTO_TITULO As Integer
    Public Property NOMBRE_DOCUMENTO As String
    Public Property ID_TIPO_DOCUMENTAL As Nullable(Of Integer)

    Public Overridable Property TIPO_DOCUMENTAL As TipoDocumental
End Class
