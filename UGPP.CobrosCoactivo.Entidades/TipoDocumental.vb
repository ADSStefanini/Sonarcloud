Public Class TipoDocumental
    Public Property Id_Tipo_Documental As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Id_Serie As Integer
    Public Property Id_Subserie As Nullable(Of Integer)

    Public Property DOCUMENTO_TITULO As ICollection(Of DocumentoTitulo) = New HashSet(Of DocumentoTitulo)
    Public Property SERIE As Serie
    Public Property SUBSERIE As SubSerie
End Class
