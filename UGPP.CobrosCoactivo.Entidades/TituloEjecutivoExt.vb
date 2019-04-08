Public Class TituloEjecutivoExt
    Public Sub New()
        TituloEjecutivo = New TituloEjecutivo()
        ObservacionTitulo = New ObservacionesCNC()
        TituloEjecutivo.tituloEjecutivoFalloCasacion = New TituloEspecial()
        TituloEjecutivo.tituloEjecutivoRecursoReconsideracion = New TituloEspecial()
        TituloEjecutivo.tituloEjecutivoRecursoReposicion = New TituloEspecial()
        TituloEjecutivo.tituloEjecutivoSentenciaSegundaInstancia = New TituloEspecial()
        TituloEjecutivo.tituloEjecutivoFalloCasacion = New TituloEspecial()
        LstDocumentos = New List(Of DocumentoMaestroTitulo)()
        LstDeudores = New List(Of Deudor)()
        LstDireccionUbicacion = New List(Of DireccionUbicacion)()
        LstNotificacion = New List(Of NotificacionTitulo)()
        LstTipificaciones = New List(Of TipificacionCNC)()
    End Sub
    Public Property TituloEjecutivo As TituloEjecutivo
    Public Property ObservacionTitulo As ObservacionesCNC
    Public Property LstDocumentos As List(Of DocumentoMaestroTitulo)
    Public Property LstDeudores As List(Of Deudor)
    Public Property LstDireccionUbicacion As List(Of DireccionUbicacion)
    Public Property LstNotificacion As List(Of NotificacionTitulo)
    Public Property LstTipificaciones As List(Of TipificacionCNC)

End Class
