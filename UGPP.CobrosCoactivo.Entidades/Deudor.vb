Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Class Deudor
    <DataMember(Name:="idTipoPersona")>
    Public Property tipoPersona As String

    <DataMember(Name:="NomtipoPersona")>
    Public Property NomtipoPersona As String

    <DataMember(Name:="valNombreDeudor")>
    Public Property nombreDeudor As String

    <DataMember(Name:="idTipoIdentificacion")>
    Public Property tipoIdentificacion As String

    <DataMember(Name:="valNumeroIdentificacion")>
    Public Property numeroIdentificacion As String

    <DataMember(Name:="NomTipoEnte")>
    Public Property NomTipoEnte As String

    <DataMember(Name:="TipoEnte")>
    Public Property TipoEnte As Int32

    <DataMember(Name:="TarjetaProfesional")>
    Public Property TarjetaProfesional As String

    <DataMember(Name:="MatriculaMercantil")>
    Public Property MatriculaMercantil As String

    <DataMember(Name:="PorcentajeParticipacion")>
    Public Property PorcentajeParticipacion As String

    <DataMember(Name:="NomTipoAportante")>
    Public Property NomTipoAportante As String

    <DataMember(Name:="TipoAportante")>
    Public Property TipoAportante As String

    <DataMember(Name:="NomEstadoPersona")>
    Public Property NomEstadoPersona As String

    <DataMember(Name:="EstadoPersona")>
    Public Property EstadoPersona As String

    <DataMember(Name:="digitoverificacion")>
    Public Property digitoVerificacion As String

    <DataMember(Name:="direccionUbicacion")>
    Public Property direccionUbicacion As DireccionUbicacion
End Class
