Public Class ConsultaExpedientes
    'Propiedades para la páginación
    Public Property StartRecord As Integer
    Public Property StopRecord As Integer
    'Propiedades de orden
    Public Property SortExpression As String
    Public Property SortDirection As String
    'Propiedades de filtro
    Public Property UsuarioLogin As String 'Usuario Logeado
    Public Property NumeroExpediente As String
    Public Property NombreDeudor As String
    Public Property NumeroDocDeudor As String
    Public Property CodEstadoProcesal As String
    Public Property CodTipoTitulo As String
    Public Property FechaEntragaTitulo As String
    Public Property FechaAsignacionGestor As String
    Public Property EstadoOperativo As Integer
    Public Property UsuarioNoIncluir As String 'Si es un usuario revisor no se deben mostrar sus expedientes en la lista de todos los expedientes
End Class
