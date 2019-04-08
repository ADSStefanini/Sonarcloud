Imports System.Runtime.Serialization
<DataContract([Namespace]:="deudor")>
Public Class DeudorContract
    <DataMember>
    Public Property idTipoPersona As String
    <DataMember>
    Public Property valNombreDeudor As String
    <DataMember>
    Public Property idTipoIdentificacion As String
    <DataMember>
    Public Property valNumeroIdentificacion As String
    <DataMember>
    Public Property direccionesubicacion As DireccionUbicacionContract

End Class
