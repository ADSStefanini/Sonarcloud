Public Class TipoTitulo
    Public Property codigo As String
    Public Property nombre As String
    Public Property ID_TIPO_CARTERA As Nullable(Of Integer)
    Public Property COD_PROCEDENCIA As Nullable(Of Integer)
    Public Property DIAS_MAX_GESTION_AMARILLO As Nullable(Of Integer)
    Public Property DIAS_MAX_GESTION_ROJO As Nullable(Of Integer)
    Public Property ANOS_FECHA_PRESCRIPCION As Nullable(Of Integer)

    Public Property MAESTRO_TITULOS As ICollection(Of MaestroTitulos) = New HashSet(Of MaestroTitulos)
End Class
