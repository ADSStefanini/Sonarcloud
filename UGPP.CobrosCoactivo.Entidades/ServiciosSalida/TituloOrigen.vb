Imports System.Runtime.Serialization
Imports UGPP.CobrosCoactivo.Entidades
<DataContract([Namespace]:="tituloOrigen")>
Public Class TituloOrigenContract
    <DataMember(Name:="idTipoTituloOrigen")>
    Public Property tipoTitulo As String
    <DataMember(Name:="idTipoCartera")>
    Public Property tipoCartera As Enumeraciones.TipoCartera
    <DataMember(Name:="valNumeroExpedienteOrigen")>
    Public Property numeroExpedienteOrigen As String
    <DataMember(Name:="idAreaOrigen")>
    Public Property areaOrigen As Enumeraciones.AreaOrigen
    <DataMember(Name:="valNumeroTituloOrigen")>
    Public Property numeroTitulo As String
    <DataMember(Name:="fecFechaTituloOrigen")>
    Public Property fechaTituloEjecutivo As DateTime
    <DataMember(Name:="valValorObligacion")>
    Public Property valorTitulo As Nullable(Of Decimal)
    <DataMember(Name:="valPartidaGlobal")>
    Public Property partidaGlobal As Nullable(Of Decimal)
    <DataMember(Name:="valSancionOmision")>
    Public Property sancionOmision As Nullable(Of Decimal)
    <DataMember(Name:="valSancionInexactitud")>
    Public Property sancionInexactitud As Nullable(Of Decimal)
    <DataMember(Name:="valtotalObligacions")>
    Public Property totalObligacion As Nullable(Of Decimal)
    <DataMember(Name:="idFormaNotificacion")>
    Public Property formaNotificacion As String
    <DataMember(Name:="fecFechaNotificacionTituloOrigen")>
    Public Property fechaNotificacion As DateTime
    <DataMember(Name:="esTituloEjecutoriado")>
    Public Property tituloEjecutoriado As Byte
    <DataMember(Name:="fecFechaEjecutoria")>
    Public Property fechaEjecutoria As DateTime?
    <DataMember(Name:="fecFechaExigibilidad")>
    Public Property fechaExigibilidad As DateTime?
    <DataMember(Name:="deudor")>
    Public Property deudor As DeudorContract
    <DataMember(Name:="documentos")>
    Public Property documentos As List(Of DocumentoContract)
    <DataMember(Name:="esPresentaRecursoReconsideracion")>
    Public Property presentaRecursoReconsideracion As Byte
    <DataMember(Name:="tituloEjecutivoRecursoReconsideracion")>
    Public Property tituloEjecutivoRecursoReconsideracion As Titulo?
    <DataMember(Name:="esPresentaRecursoReposicion")>
    Public Property presentaRecursoReposicion As Byte
    <DataMember(Name:="tituloEjecutivoRecursoReposicion")>
    Public Property tituloEjecutivoRecursoReposicion As Titulo?
    <DataMember(Name:="esExisteSentenciaSegundaInstancia")>
    Public Property existeSentenciaSegundaInstancia As Byte
    <DataMember(Name:="tituloEjecutivoSentenciaSegundaInstancia")>
    Public Property tituloEjecutivoSentenciaSegundaInstancia As Titulo?
    <DataMember(Name:="causante")>
    Public Property causante As Causante?
    <DataMember(Name:="cuentasCobros")>
    Public Property cuentasCobros As Nullable(Of CuentaCobro)
    <DataMember(Name:="esExisteFalloCasacion")>
    Public Property existeFalloCasacion As Byte?
End Class
