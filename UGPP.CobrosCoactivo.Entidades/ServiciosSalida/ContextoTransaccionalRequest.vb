Imports System.Runtime.Serialization
Imports System.Xml.Serialization
<DataContract([Namespace]:="contextoTransaccionalRequest")>
Public Class ContextoTransaccionalRequest
    <DataMember(Name:="contextoTransaccionalTipo")>
    Public Property contextoTransaccionalTipo As ContextoTransaccionalTipo
    <DataMember(Name:="titulosOrigens")>
    Public Property tituloOrigen As List(Of TituloOrigenContract)
End Class
