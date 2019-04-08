Public Class Pagos
    Public Property NroConsignacion As String
    Public Property NroExp As String
    Public Property FecSolverif As Nullable(Of Date)
    Public Property FecVerificado As Nullable(Of Date)
    Public Property estado As String
    Public Property UserSolicita As String
    Public Property UserVerif As String
    Public Property pagFecha As Nullable(Of Date)
    Public Property pagFechaDeudor As Nullable(Of Date)
    Public Property pagNroTitJudicial As String
    Public Property pagCapital As Nullable(Of Double)
    Public Property pagAjusteDec1406 As Nullable(Of Double)
    Public Property pagInteres As Nullable(Of Double)
    Public Property pagGastosProc As Nullable(Of Double)
    Public Property pagExceso As Nullable(Of Double)
    Public Property pagTotal As Nullable(Of Double)
    Public Property pagestadoprocfrp As String
    Public Property pagFecExi As Nullable(Of Date)
    Public Property pagTasaIntApl As String
    Public Property pagdiasmora As Nullable(Of Integer)
    Public Property pagvalcuota As Nullable(Of Double)
    Public Property pagNumConPag As String
    Public Property NroConsigOri As String
    Public Property pagLiqSan As Nullable(Of Boolean)
    Public Property vlripc As Nullable(Of Double)
    Public Property SnContabilizar As Boolean
    Public Property pagNroRadicadoSalida As Nullable(Of Double)
End Class
