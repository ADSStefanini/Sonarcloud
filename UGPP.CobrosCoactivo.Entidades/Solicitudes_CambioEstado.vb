Public Class Solicitudes_CambioEstado
    Public Property idunico As Long
    Public Property NroExp As String
    Public Property abogado As String
    Public Property fecha As Nullable(Of Date)
    Public Property estadoactual As String
    Public Property estado As String
    Public Property estadosol As Nullable(Of Integer)
    Public Property observac As String
    Public Property grupo As String
    Public Property revisor As String
    Public Property aprob_revisor As String
    Public Property nota_revisor As String
    Public Property fecha_aprob_revisor As Nullable(Of Date)
    Public Property ejecutor As String
    Public Property aprob_ejecutor As String
    Public Property nota_ejecutor As String
    Public Property fecha_aprob_ejecutor As Nullable(Of Date)
    Public Property nivel_escalamiento As Nullable(Of Integer)
    Public Property efietapaprocesal As Nullable(Of Integer)

    Public Property TAREA_SOLICITUD As ICollection(Of TareaSolicitud) = New HashSet(Of TareaSolicitud)
    Public Property TAREA_SOLICITUD1 As ICollection(Of TareaSolicitud) = New HashSet(Of TareaSolicitud)
End Class
