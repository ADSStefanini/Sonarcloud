Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Structure Titulo
    <DataMember(Name:="valNumeroTituloOrigen")>
    Public Property numeroTitulo As String
    <DataMember(Name:="fecFechaTituloOrigen")>
    Public Property fechaTituloEjecutivo As DateTime
    <DataMember(Name:="idFormaNotificacionTituloOrigen")>
    Public Property formaNotificacion As Enumeraciones.FormaNotificacion?
    <DataMember(Name:="fecFechaNotificacionTituloOrigen")>
    Public Property fechaNotificacion As DateTime?
End Structure
