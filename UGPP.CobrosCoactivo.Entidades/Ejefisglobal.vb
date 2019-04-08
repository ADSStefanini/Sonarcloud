'Clase de expediente
Public Class EJEFISGLOBAL
    Public Property EFINROEXP As String
    Public Property EFIFECHAEXP As Nullable(Of Date)
    Public Property EFINUMMEMO As String
    Public Property EFIEXPORIGEN As String
    Public Property EFIFECCAD As Nullable(Of Date)
    Public Property EFINIT As String
    Public Property EFIVALDEU As Nullable(Of Decimal)
    Public Property EFIVALINT As Nullable(Of Decimal)
    Public Property EFIPAGOSCAP As Nullable(Of Double)
    Public Property EFISALDOCAP As Nullable(Of Double)
    Public Property EFIMODCOD As Nullable(Of Integer)
    Public Property EFIUSUASIG As String
    Public Property EFIDEVUELTO As Nullable(Of Boolean)
    Public Property EFIDIFICILCOBRO As Nullable(Of Boolean)
    Public Property EFICODESTEXP As String
    Public Property EFIESTUP As String
    Public Property EFIANULAR As Nullable(Of Boolean)
    Public Property EFIESTADO As String
    Public Property EFIULTPAS As String
    Public Property EFIESTADOPAGO As String
    Public Property EFIUSUREV As String
    Public Property EFIFECENTGES As Nullable(Of Date)
    Public Property TitEjecAntig As String
    Public Property Caducidad As String
    Public Property EstadoPersona As String
    Public Property CausalesInc As String
    Public Property ProcesoCurso As String
    Public Property AcuerdoPago As String
    Public Property FecSistema As Nullable(Of Date)
    Public Property EFIIPC As Nullable(Of Double)
    Public Property EFIEXPDOCUMENTIC As String
    Public Property EFIETAPAPROCESAL As Nullable(Of Integer)

    Public Property ENTES_DEUDORES As EntesDeudores
    Public Property HISTORICO_CLASIFICACION_MANUAL As ICollection(Of HistoricoClasificacionManual) = New HashSet(Of HistoricoClasificacionManual)
    Public Property MAESTRO_TITULOS As ICollection(Of MaestroTitulos) = New HashSet(Of MaestroTitulos)

End Class
