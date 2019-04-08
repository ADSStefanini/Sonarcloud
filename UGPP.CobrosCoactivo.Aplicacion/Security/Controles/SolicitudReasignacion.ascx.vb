Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class SolicitudReasignación
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' Variable que indica si hubo un error al guardar la priorización
    ''' </summary>
    ''' <returns></returns>
    Public Property errorReasignacion As Boolean
    ''' <summary>
    ''' Variable que captura el identificador de la tarea asiganada que se va a procesar
    ''' </summary>
    ''' <returns></returns>
    Public Property idsTareasAsignadas As List(Of Integer)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ddlCausalReasignacionValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            reqReasignacionObservacion.Text = My.Resources.Formularios.errorCampoRequerido
            lblNoUsuarioSuperior.Text = My.Resources.erorUsuarioSuperior

            'Poblar opciones de causales de reasignación
            CommonsCobrosCoactivos.poblarCausalesReasignacion(ddlCausalReasignacion)
        End If
    End Sub

    ''' <summary>
    ''' Llena los usuarios que se pueden solicitar para la reasignación
    ''' </summary>
    ''' <param name="prmIntTipoGestor">{1: Gestores estudio de títulos; 2: Gestores expedientes}</param>
    Public Sub poblarGestorSolicitadoParaReasignacion(Optional ByVal prmIntTipoGestor As Int32 = Nothing)
        If Not IsNothing(prmIntTipoGestor) Then
            hdnTipoGestor.Value = prmIntTipoGestor
        End If
        'Si no se carga si se deben seleccionar gestores de estudio de títulos o expedientes se oculta
        If IsNothing(hdnTipoGestor.Value) OrElse hdnTipoGestor.Value = "" OrElse hdnTipoGestor.Value = "0" Then
            divGestorSolicitado.Visible = False
            Exit Sub
        End If

        CommonsCobrosCoactivos.PoblarUsuariosReasignacion(ddlGestorSolicitado, hdnTipoGestor.Value)
    End Sub

    ''' <summary>
    ''' Establece el ID de la tarea asignada sobre el cual se va a solicitar la priorización
    ''' </summary>
    ''' <param name="prmObjIdTareaAsiganada">Lista de identificadores de las tareas asiganadas</param>
    Public Sub AsignarTareasAsiganadas(ByVal prmObjIdTareaAsiganada As List(Of Integer))
        If prmObjIdTareaAsiganada.Count() = 0 Then
            Exit Sub
        End If
        hdnIdTareaAsiganada.Value = String.Join(",", prmObjIdTareaAsiganada)
        Me.idsTareasAsignadas = prmObjIdTareaAsiganada
    End Sub

    Public Function validarUsuarioSuperior() As Boolean
        Dim _usuarioSuperior = CommonsCobrosCoactivos.ObtenerUsuarioSuperiorSolicitud(Session("ssloginusuario"), 9)
        If String.IsNullOrEmpty(_usuarioSuperior.login) Then
            lblNoUsuarioSuperior.Visible = True
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Inicia el formulario con los textos y las opiones por default, opciones vacias
    ''' </summary>
    Public Sub IniciarFormulario()

        lblReasignacionEnviada.Visible = False
        lblErrorReasignacion.Visible = False
        lblNoUsuarioSuperior.Visible = False

        If Not tareaAsiganadaInicializada() Then
            Exit Sub
        End If

        If Not validarUsuarioSuperior() Then
            Exit Sub
        End If

        'Deja el formulario con los valores por defecto
        HabilitarFormulario()
        vaciarFormulario()
    End Sub

    ''' <summary>
    ''' Deja el formulario con los valores por defecto
    ''' </summary>
    Public Sub vaciarFormulario()
        ddlCausalReasignacion.SelectedIndex = 0
        ddlGestorSolicitado.SelectedIndex = 0
        txtReasignacionObservacion.Text = ""
    End Sub

    ''' <summary>
    ''' Valida que exista una tarea asiganada para realizar la priorización
    ''' </summary>
    ''' <returns></returns>
    Public Function tareaAsiganadaInicializada()
        If IsNothing(Me.idsTareasAsignadas) AndAlso String.IsNullOrEmpty(hdnIdTareaAsiganada.Value) Then
            'lblErrorPriorizacion.Text = My.Resources.bandejas.errorSinTareaAPriorizar
            lblErrorReasignacion.Text = "No se ha encontrado ningún objeto a reasignar"
            lblErrorReasignacion.Visible = True
            DeshabilitarFormulario()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Deshabilita todas las opciones del formulario
    ''' </summary>
    Public Sub DeshabilitarFormulario()
        ddlCausalReasignacion.Enabled = False
        ddlGestorSolicitado.Enabled = False
        txtReasignacionObservacion.Enabled = False
        cmdSolicitarReasignacion.Enabled = False
    End Sub

    ''' <summary>
    ''' Habilita todas las opciones del formulario
    ''' </summary>
    Public Sub HabilitarFormulario()
        ddlCausalReasignacion.Enabled = True
        ddlGestorSolicitado.Enabled = True
        txtReasignacionObservacion.Enabled = True
        cmdSolicitarReasignacion.Enabled = True
    End Sub

    ''' <summary>
    ''' Abrir el modal
    ''' </summary>
    Public Sub MostrarModal()
        modalReasignacion.Show()
    End Sub

    ''' <summary>
    ''' Cerrar el modal
    ''' </summary>
    Public Sub CerrarModal()
        modalReasignacion.Hide()
    End Sub

    Protected Sub cmdSolicitarReasignacion_Click(sender As Object, e As EventArgs) Handles cmdSolicitarReasignacion.Click

        lblReasignacionEnviada.Visible = False
        lblErrorReasignacion.Visible = False

        If Not tareaAsiganadaInicializada() Then
            Exit Sub
        End If

        Try
            'Se crea la lista de ids de tareas asignadas 
            Dim _tareasAsignadas = Split(hdnIdTareaAsiganada.Value, ",")
            'Se convierte la lista de strings a lista de enteros
            Dim idsTareasAsignadas As New List(Of Integer)
            For Each tarea As String In _tareasAsignadas
                idsTareasAsignadas.Add(Convert.ToInt32(tarea))
            Next

            'Se captura el usuario destino
            Dim _usuarioDestino = If(ddlGestorSolicitado.SelectedIndex <> 0, ddlGestorSolicitado.SelectedValue, Nothing)

            'Se crea objeto base para guardar la solicitud
            Dim _solicitudTarea As New SolicitudTarea()
            _solicitudTarea.IdsTareasignadas = idsTareasAsignadas
            _solicitudTarea.UsuarioSolicitante = Session("ssloginusuario")
            _solicitudTarea.TipoSolicitud = Convert.ToInt32(Enumeraciones.DominioDetalle.SolicitudResignacion)
            _solicitudTarea.TipologiaSolicitud = ddlCausalReasignacion.SelectedValue
            _solicitudTarea.UsuarioDestino = _usuarioDestino
            _solicitudTarea.SolicitudTareaObservacion = txtReasignacionObservacion.Text
            _solicitudTarea.CodNuevoEstado = 13

            'Se llama el objeto que guarda la solicitud y se procesa la solicitud
            Dim _bandejaBLL As New BandejaBLL()
            _bandejaBLL.ProcesarSolicitudEnTarea(_solicitudTarea)
            errorReasignacion = False

            lblReasignacionEnviada.Visible = True
            DeshabilitarFormulario()
            hdnReloadPage.Value = "1"
            btnClose.Text = "Cerrar"
        Catch ex As Exception
            'TODO: Capturar excepción y guardar en el LOG
            lblErrorReasignacion.Text = "Ha ocurrido un error inesperado"
            lblErrorReasignacion.Visible = True
            errorReasignacion = True
        End Try

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If hdnReloadPage.Value = "1" Then
            Response.Redirect(Request.RawUrl, True)
        End If
        CerrarModal()
    End Sub
End Class