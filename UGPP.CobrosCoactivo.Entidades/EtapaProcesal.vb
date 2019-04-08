Public Class EtapaProcesal
    Public Property ID_ETAPA_PROCESAL As Integer
    Public Property codigo As String
    Public Property VAL_ETAPA_PROCESAL As String

    Public Property CAMBIOS_ESTADO As ICollection(Of CambiosEstado) = New HashSet(Of CambiosEstado)
    Public Property ESTADOS_PROCESO As EstadosProceso

End Class
