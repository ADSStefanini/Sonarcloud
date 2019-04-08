Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class VerSolicitud
    Inherits System.Web.UI.UserControl

    Property idTareaSolictud As Int32

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub AsoganrIdTareaSolicitud(ByVal prmIntIdTareaSolictud As Int32)
        Me.idTareaSolictud = prmIntIdTareaSolictud
        Me.hdnTareaSolicitud.Value = prmIntIdTareaSolictud
    End Sub

    Public Sub AbrirPopUp()
        modalVerSolicitud.Show()
    End Sub

    Public Sub CerrarPopUp()
        modalVerSolicitud.Hide()
    End Sub

    Public Sub PoblarDatosTareaSolicitud()
        If Not validarIdTareaSolicitud() Then
            Exit Sub
        End If

        lblErrorVerSolicitud.Visible = False
        Dim _tareaAsiganadaID = Convert.ToInt32(hdnTareaSolicitud.Value)
        Dim _tareaSolicitudBLL As New TareaSolicitudBLL()
        Dim _tareaSolicitud = _tareaSolicitudBLL.obternerTareaSolicitudPorId(_tareaAsiganadaID)
        If Not IsNothing(_tareaSolicitud.ID_TAREA_OBSERVACION) Then
            Dim _tareaObservacionBLL As New TareaObservacionBLL()
            Dim _tareaObservacion = _tareaObservacionBLL.obtenerTareaObservacionPorId(_tareaSolicitud.ID_TAREA_OBSERVACION)
            lblObservacionText.Text = _tareaObservacion.OBSERVACION
        Else
            pnlGestorSolicitado.Visible = False
            lblObservacionText.Text = "N/A"
        End If

        If _tareaSolicitud.VAL_TIPO_SOLICITUD = Enumeraciones.DominioDetalle.SolicitudResignacion Then
            pnlGestorSolicitado.Visible = True
            lblGestorSolicitadoText.Text = _tareaSolicitud.VAL_USUARIO_DESTINO
        Else
            pnlGestorSolicitado.Visible = False
        End If

    End Sub

    Private Function validarIdTareaSolicitud() As Boolean
        If String.IsNullOrEmpty(hdnTareaSolicitud.Value) OrElse hdnTareaSolicitud.Value = "" Then
            lblErrorVerSolicitud.Text = "No se ha encontrado tarea para mostrar"
            Return False
        End If
        Return True
    End Function

    Public Sub PoblarDatosSolicitudReasignacion(ByVal prmIntIdSolicitudReasignacion As Int32)
        Dim _solicitudCambiosEstadoBLL As New SolicitudCambiosEstadoBLL()
        Dim cambioEstado = _solicitudCambiosEstadoBLL.consultarSolictiudesCE(prmIntIdSolicitudReasignacion)
        pnlGestorSolicitado.Visible = False
        If Not IsNothing(cambioEstado.observac) Then
            lblObservacionText.Text = cambioEstado.observac
        Else
            lblObservacionText.Text = "N/A"
        End If
    End Sub
End Class