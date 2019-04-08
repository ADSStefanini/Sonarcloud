Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Class TitulosEjecutivos
    <DataMember(Name:="titulosEjecutivos")>
    Public Property titulosEjecutivos As List(Of TituloEjecutivo)
End Class
