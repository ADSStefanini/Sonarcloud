Imports System.Runtime.Serialization
<DataContract([Namespace]:="direccionUbicacion")>
Public Class DireccionUbicacionContract
    <DataMember>
    Public Property valDireccionCompleta As String
    <DataMember>
    Public Property codDepartamento As String
    <DataMember>
    Public Property codCiudad As String
    <DataMember>
    Public Property valTelefono As String
    <DataMember>
    Public Property valCelular As String
    <DataMember>
    Public Property valemail As String
    <DataMember>
    Public Property idFuenteDireccion As String
End Class
