Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Class TituloEjecutivo

#Region "Titulo Ejecutivo"
    <DataMember(Name:="idTipoTituloOrigen")>
    Public Property tipoTitulo As String
    <DataMember(Name:="idtipoInteres")>
    Public Property tipoInteres As String
    <DataMember(Name:="idTipoCartera")>
    Public Property tipoCartera As Enumeraciones.TipoCartera
    '<DataMember(Name:="idTipoObligacion")>
    'Public Property tipoObligacion As Enumeraciones.TipoObligacion
    <DataMember(Name:="valNumeroExpedienteOrigen")>
    Public Property numeroExpedienteOrigen As string
    <DataMember(Name:="idAreaOrigen")>
    Public Property areaOrigen As Enumeraciones.AreaOrigen?
    <DataMember(Name:="valNumeroTituloOrigen")>
    Public Property numeroTitulo As String
    <DataMember(Name:="fecFechaTituloOrigen")>
    Public Property fechaTituloEjecutivo As DateTime
    <DataMember(Name:="valValorObligacion")>
    Public Property valorTitulo As Decimal?
    <DataMember(Name:="valPartidaGlobal")>
    Public Property partidaGlobal As Decimal?
    <DataMember(Name:="valSancionOmision")>
    Public Property sancionOmision As Decimal?
    <DataMember(Name:="valSancionInexactitud")>
    Public Property sancionMora As Decimal?
    <DataMember(Name:="valSancionMora")>
    Public Property TotalSancion As Decimal?
    <DataMember(Name:="valTotalSancion")>
    Public Property sancionInexactitud As Decimal?
    <DataMember(Name:="valtotalObligacions")>
    Public Property totalObligacion As Decimal?
    <DataMember(Name:="idFormaNotificacion")>
    Public Property formaNotificacion As String
    <DataMember(Name:="CodTipoNotificacion")>
    Public Property CodTipNotificacion As Int32
    <DataMember(Name:="CodTipoSentencia")>
    Public Property CodTipSentencia As Int32
    <DataMember(Name:="fecFechaNotificacionTituloOrigen")>
    Public Property fechaNotificacion As DateTime?
    <DataMember(Name:="esTituloEjecutoriado")>
    Public Property tituloEjecutoriado As Byte?
    <DataMember(Name:="fecFechaEjecutoria")>
    Public Property fechaEjecutoria As DateTime?
    <DataMember(Name:="fecFechaExigibilidad")>
    Public Property fechaExigibilidad As DateTime?
    <DataMember(Name:="fecFechaCaducidadPrescrip")>
    Public Property fechaCaducidadPrescripcion As DateTime?
    <DataMember(Name:="IdunicoTitulo")>
    Public Property IdunicoTitulo As Int32

    <DataMember(Name:="MT_res_resuelve_reposicion")>
    Public Property MT_res_resuelve_reposicion As String

    <DataMember(Name:="MT_reso_resu_apela_recon")>
    Public Property MT_reso_resu_apela_recon As String

    <DataMember(Name:="Automatico")>
    Public Property Automatico As Boolean


#End Region
#Region "Deudor"
    <DataMember(Name:="deudor")>
    Public Property deudor As Deudor
#End Region
#Region "Direccionubicacion"
    <DataMember(Name:="Direccion")>
    Public Property direccionubicacion As DireccionUbicacion
#End Region

#Region "Titulo Especial"
    <DataMember(Name:="esPresentaRecursoReconsideracion")>
    Public Property presentaRecursoReconsideracion As Byte?
    <DataMember(Name:="tituloEjecutivoRecursoReconsideracion")>
    Public Property tituloEjecutivoRecursoReconsideracion As TituloEspecial
    <DataMember(Name:="esPresentaRecursoReposicion")>
    Public Property presentaRecursoReposicion As Byte?
    <DataMember(Name:="tituloEjecutivoRecursoReposicion")>
    Public Property tituloEjecutivoRecursoReposicion As TituloEspecial
    <DataMember(Name:="esExisteSentenciaSegundaInstancia")>
    Public Property existeSentenciaSegundaInstancia As Byte?
    <DataMember(Name:="tituloEjecutivoSentenciaSegundaInstancia")>
    Public Property tituloEjecutivoSentenciaSegundaInstancia As TituloEspecial
    <DataMember(Name:="esExisteFalloCasacion")>
    Public Property existeFalloCasacion As Byte?
    <DataMember(Name:="tituloEjecutivoFalloCasacion")>
    Public Property tituloEjecutivoFalloCasacion As TituloEspecial
#End Region
#Region "Documentos"
    <DataMember(Name:="documento")>
    Public Property documento As DocumentoMaestroTitulo
#End Region

End Class
