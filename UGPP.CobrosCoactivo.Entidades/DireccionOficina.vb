Public Class DireccionOficina
    Public Property Id_Direccion_Oficina As Integer
    Public Property Descripcion_Oficina As String
    Public Property Activo As String
    Public Property Id_Nivel As Nullable(Of Integer)
    Public Property Cod_Oficina As String

    Public Property Serie As ICollection(Of Serie) = New HashSet(Of Serie)
End Class
