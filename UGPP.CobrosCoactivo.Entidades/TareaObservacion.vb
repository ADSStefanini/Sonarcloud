Public Class TareaObservacion
    Public Property COD_ID_TAREA_OBSERVACION As Integer
    Public Property OBSERVACION As String
    Public Property IND_ESTADO As Boolean
    Public Property FEC_CREACION As Date

    Public Property TAREA_ASIGNADA As ICollection(Of TareaAsignada) = New HashSet(Of TareaAsignada)
    Public Property TAREA_SOLICITUD As ICollection(Of TareaSolicitud) = New HashSet(Of TareaSolicitud)
End Class
