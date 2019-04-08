Imports System.Runtime.Serialization

<DataContract([Namespace]:="")>
Public Class Valores
    Public Property ID_TIPO_OBLIGACION_VALORES As String
    Public Property NOMBRETIPO As String
    Public Property VALOR_OBLIGACION As Boolean
    Public Property PARTIDA_GLOBAL As Boolean
    Public Property SANCION_OMISION As Boolean
    Public Property SANCION_INEXACTITUD As Boolean
    Public Property SANCION_MORA As Boolean
End Class
