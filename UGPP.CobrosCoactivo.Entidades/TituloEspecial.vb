Imports System.Runtime.Serialization

Public Class TituloEspecial
    <DataMember(Name:="valNumeroTituloOrigen")>
    Public Property numeroTitulo As String
    <DataMember(Name:="fecFechaTituloOrigen")>
    Public Property fechaTituloEjecutivo As DateTime?
    <DataMember(Name:="idFormaNotificacion")>
    Public Property formaNotificacion As String
    <DataMember(Name:="fecFechaNotificacionTituloOrigen")>
    Public Property fechaNotificacion As DateTime?
End Class
