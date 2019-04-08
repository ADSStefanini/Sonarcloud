Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.Serialization
Imports System.Xml.Schema
Imports System.Xml.Serialization

<DataContract([Namespace]:="ContextoTransaccionalTipo")>
Public Class ContextoTransaccionalTipo
    <DataMember(IsRequired:=True)>
    <StringLength(36)>
    Public Property idTx As String
    <DataMember(IsRequired:=True)>
    <StringLength(48)>
    Public Property fechaInicioTx As DateTime
    <DataMember(IsRequired:=True)>
    <StringLength(48)>
    Public Property idDefinicionProceso As String
    <DataMember(IsRequired:=True)>
    <StringLength(48)>
    Public Property valNombreDefinicionProceso As String
    <DataMember()>
    <StringLength(48)>
    Public Property idInstanciaActividad As String
    <DataMember>
    <StringLength(100)>
    Public Property valNombreDefinicionActividad As String
    <DataMember(IsRequired:=True)>
    Public Property idUsuarioAplicacion As String
    <DataMember(IsRequired:=True)>
    Public Property valClaveUsuarioAplicacion As String
    <DataMember(IsRequired:=True)>
    Public Property idUsuario As String
    <DataMember(IsRequired:=True)>
    Public Property idEmisor As String
    <DataMember>
    Public Property valURL As String
    <DataMember>
    Public Property valTamPagina As Integer = 0
    <DataMember>
    Public Property valNumPagina As Integer = 0
End Class
