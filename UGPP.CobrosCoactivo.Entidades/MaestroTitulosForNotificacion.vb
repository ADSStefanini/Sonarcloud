Public Class MaestroTitulosForNotificacion
    Public Property ID_MAESTRO_TITULOS_FOR_NOTIFICACION As Long
    Public Property ID_UNICO_MAESTRO_TITULOS As Long
    Public Property FEC_NOTIFICACION As Nullable(Of Date)
    Public Property COD_FOR_NOT As String
    Public Property COD_TIPO_NOTIFICACION As Integer

    Public Property MAESTRO_TITULOS As MaestroTitulos
End Class
