Imports System.Runtime.Serialization
<DataContract([Namespace]:="ctaCobro")>
Public Structure CuentaCobro
    <DataMember>
    Public Property idCuentaCobro As String
    <DataMember>
    Public Property fecFechaCtaCobro As DateTime
    <DataMember>
    Public Property valValorCtaCobro As Decimal
End Structure
