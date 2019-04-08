Imports System.Runtime.Serialization
<DataContract([Namespace]:="RspActualizacionEstadoProcesoCobros")>
Public Class ResponseContract
    <DataMember>
    Public Property resultadoEjecucion As String
    <DataMember>
    Public Property codigoError As String
    <DataMember>
    Public Property detalleError As String
    <DataMember(Name:="contextoRespuesta")>
    Public Property contextoRespuesta As ContextoRespuestaTipo_old
End Class
