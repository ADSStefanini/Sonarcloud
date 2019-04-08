Imports System.Runtime.Serialization

<DataContract([Namespace]:="causante")>
Public Structure Causante
    <DataMember>
    Public Property idTipoIdentificacion As String
    <DataMember>
    Public Property valNumeroIdentificacion As String
End Structure
