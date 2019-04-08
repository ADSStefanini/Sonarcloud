Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Class BandejaTitulosAreaOrigen
    Public Property IDUNICO As Int64
    Public Property NROTITULO As String
    Public Property FCHEXPEDICIONTITULO As String
    Public Property NOMBREDEUDOR As String
    Public Property NRONITCEDULA As String
    Public Property TIPOOBLIGACION As String
    Public Property TOTALOBLIGACION As Double
    Public Property FEC_ENTREGA_GESTOR As String
    Public Property FCHLIMITE As String
    Public Property JSON_ALMACENAMIENTO As String
    Public Property ID_ESTADO_OPERATIVOS As Int32
    Public Property ID_TAREA_ASIGNADA As Int64
    Public Property COLOR As String
End Class
