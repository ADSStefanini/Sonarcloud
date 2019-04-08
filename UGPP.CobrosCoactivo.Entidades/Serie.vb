Public Class Serie
    Public Property Id_Serie As Integer
    Public Property Id_Direccion_Oficina As Nullable(Of Integer)
    Public Property Codigo_Serie As String
    Public Property Nombre_Serie As String
    Public Property Activo As Boolean

    Public Property DIRECCION_OFICINA As DireccionOficina
    Public Property SUBSERIE As ICollection(Of SubSerie) = New HashSet(Of SubSerie)
    Public Property TIPO_DOCUMENTAL As ICollection(Of TipoDocumental) = New HashSet(Of TipoDocumental)
End Class
