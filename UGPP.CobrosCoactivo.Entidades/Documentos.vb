Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Class Documentos

    <DataMember(Name:="valNombreDocumento")>
    Public Property valNombreDocumento As String
    <DataMember(Name:="codDocumentic")>
    Public Property codDocumentic As String
    <DataMember(Name:="codTipoDocumento")>
    Public Property codTipoDocumento As String

End Class
