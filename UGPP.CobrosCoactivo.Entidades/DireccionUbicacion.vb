Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Class DireccionUbicacion
    <DataMember(Name:="idunico")>
    Public Property idunico As Long?
    <DataMember(Name:="valDireccionCompleta")>
    Public Property direccionCompleta As String
    <DataMember(Name:="nombrefuente")>
    Public Property nombreFuente As String
    <DataMember(Name:="deudornombre")>
    Public Property deudornombre As String
    <DataMember(Name:="numeroidentificacionDeudor")>
    Public Property numeroidentificacionDeudor As String
    <DataMember(Name:="codDepartamento")>
    Public Property idDepartamento As String
    <DataMember(Name:="NombreDepartamento")>
    Public Property NombreDepartamento As String
    <DataMember(Name:="NombreMunicipio")>
    Public Property NombreMunicipio As String
    <DataMember(Name:="codCiudad")>
    Public Property idCiudad As String
    <DataMember(Name:="valTelefono")>
    Public Property telefono As String
    <DataMember(Name:="valCelular")>
    Public Property celular As String
    <DataMember(Name:="valemail")>
    Public Property email As String
    <DataMember(Name:="idFuenteDireccion")>
    Public Property fuentesDirecciones As Int32
    <DataMember(Name:="idOtrasFuentesDirecciones")>
    Public Property otrasFuentesDirecciones As String
    <DataMember(Name:="paginaweb")>
    Public Property paginaweb As String
End Class

