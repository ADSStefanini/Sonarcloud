Public Class TipoIdentificacion
    Public Property codigo As String
    Public Property nombre As String
    Public Property ind_estado As Nullable(Of Boolean)

    Public Overridable Property ENTES_DEUDORES As ICollection(Of EntesDeudores) = New HashSet(Of EntesDeudores)
End Class
