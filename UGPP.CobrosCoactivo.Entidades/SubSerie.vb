Public Class SubSerie
    Public Property Id_Subserie As Integer
    Public Property Id_Serie As Integer
    Public Property Codigo_Subserie As String
    Public Property Nombre_Subserie As String
    Public Property Activo As Boolean

    Public Property SERIE As Serie
    Public Property TIPO_DOCUMENTAL As ICollection(Of TipoDocumental) = New HashSet(Of TipoDocumental)
End Class
