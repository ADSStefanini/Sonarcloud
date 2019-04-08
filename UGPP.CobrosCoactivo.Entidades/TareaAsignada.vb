Public Class TareaAsignada
    Public Property ID_TAREA_ASIGNADA As Long
    Public Property VAL_USUARIO_NOMBRE As String
    Public Property COD_TIPO_OBJ As Integer
    Public Property ID_UNICO_TITULO As Nullable(Of Long)
    Public Property EFINROEXP_EXPEDIENTE As String
    Public Property FEC_ACTUALIZACION As Date
    Public Property FEC_ENTREGA_GESTOR As Nullable(Of Date)
    Public Property VAL_PRIORIDAD As Integer
    Public Property IND_TITULO_PRIORIZADO As Nullable(Of Boolean)
    Public Property COD_ESTADO_OPERATIVO As Integer
    Public Property ID_TAREA_OBSERVACION As Nullable(Of Integer)

    Public Overridable Property ALMACENAMIENTO_TEMPORAL As ICollection(Of AlmacenamientoTemporal) = New HashSet(Of AlmacenamientoTemporal)
    Public Overridable Property TAREA_SOLICITUD As ICollection(Of TareaSolicitud) = New HashSet(Of TareaSolicitud)
    Public Overridable Property TAREA_OBSERVACION As TareaObservacion

End Class
