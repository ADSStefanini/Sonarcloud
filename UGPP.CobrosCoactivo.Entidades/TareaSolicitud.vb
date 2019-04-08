Public Class TareaSolicitud
    Public Property ID_TAREA_SOLICITUD As Long
    Public Property ID_TAREA_ASIGNADA As Long
    Public Property VAL_USUARIO_SOLICITANTE As String
    Public Property VAL_USUARIO_APROBADOR As String
    Public Property VAL_USUARIO_DESTINO As String
    Public Property VAL_TIPO_SOLICITUD As Integer
    Public Property COD_SOLICITUD_CAMBIO_ESTADO As Nullable(Of Long)
    Public Property VAL_TIPOLOGIA As String
    Public Property ID_TAREA_OBSERVACION As Nullable(Of Integer)
    Public Property IND_SOLICITUD_PROCESADA As Boolean
    Public Property COD_ESTADO_SOLICITUD As Nullable(Of Integer)
    Public Property FEC_SOLICITUD As Nullable(Of Date)

    Public Property SOLICITUDES_CAMBIOESTADO As Solicitudes_CambioEstado
    Public Property SOLICITUDES_CAMBIOESTADO1 As Solicitudes_CambioEstado
    Public Property TAREA_ASIGNADA As TareaAsignada
    Public Property TAREA_OBSERVACION As TareaObservacion
End Class
