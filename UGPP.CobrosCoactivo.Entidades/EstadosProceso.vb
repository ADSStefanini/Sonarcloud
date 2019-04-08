Public Class EstadosProceso
    Public Property codigo As String
    Public Property nombre As String
    Public Property termino As Nullable(Of Integer)
    Public Property max_dias_gestion_amarillo As Nullable(Of Integer)
    Public Property max_dias_gestion_rojo As Nullable(Of Integer)

    Public Property ETAPA_PROCESAL As ICollection(Of EtapaProcesal) = New HashSet(Of EtapaProcesal)

End Class
