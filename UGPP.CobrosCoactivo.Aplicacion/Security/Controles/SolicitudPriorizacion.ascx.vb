Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class SolicitudPriorizacion
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' Variable que indica si hubo un error al guardar la priorización
    ''' </summary>
    ''' <returns></returns>
    Public Property errorPriorizacion As Boolean
    ''' <summary>
    ''' Variable que captura el identificador de la tarea asiganada que se va a procesar
    ''' </summary>
    ''' <returns></returns>
    Public Property idTareaAsignada As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            'Mensajes de error
            CausalPriorizacionValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            reqObservacionPriorizacion.Text = My.Resources.Formularios.errorCampoRequerido
            'Poblar opciones de causales de Priorización
            CommonsCobrosCoactivos.poblarCausalesPriorizacion(ddlCausalPriorizacion)
        End If
    End Sub

    ''' <summary>
    ''' Establece el ID de la tarea asignada sobre el cual se va a solicitar la priorización
    ''' </summary>
    ''' <param name="prmStrIdTareaAsiganada">Identificador de la tarea asiganada</param>
    Public Sub AsignarTareaAsiganada(ByVal prmStrIdTareaAsiganada As String)
        Me.idTareaAsignada = Convert.ToInt32(prmStrIdTareaAsiganada)
        hdnIdTareaAsiganada.Value = prmStrIdTareaAsiganada
    End Sub

    ''' <summary>
    ''' Inicia el formulario con los textos y las opiones por default, opciones vacias
    ''' </summary>
    Public Sub IniciarFormulario()

        'Oculto mensajes de error y de success
        lblErrorPriorizacion.Visible = False
        lblPriorizacionEnviada.Visible = False

        If Not tareaAsiganadaInicializada() Then
            Exit Sub
        End If

        If existeSolicitudPriorizacion() Then
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
        ddlCausalPriorizacion.SelectedIndex = 0
        txtObservacionPriorizacion.Text = ""
    End Sub

    ''' <summary>
    ''' Valida que exista una tarea asiganada para realizar la priorización
    ''' </summary>
    ''' <returns></returns>
    Public Function tareaAsiganadaInicializada()
        If IsNothing(Me.idTareaAsignada) OrElse Me.idTareaAsignada.Equals(0) Then
            'lblErrorPriorizacion.Text = My.Resources.bandejas.errorSinTareaAPriorizar
            lblErrorPriorizacion.Text = "No se ha encontrado ningún objeto a priorizar"
            lblErrorPriorizacion.Visible = True
            DeshabilitarFormulario()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Valida si la tarea ya cuenta con una  solicitud de priorización
    ''' </summary>
    ''' <returns>Verdadero si ya existe una solicitud de priorización en la tarea, falso en caso contrario</returns>
    Public Function existeSolicitudPriorizacion()
        Dim _tareaSolicitudBLL As New TareaSolicitudBLL()
        Dim _tareaSolicitud As TareaSolicitud = _tareaSolicitudBLL.obtenerTareaSolicitudPorTipoSolicitudNoProcesada(Me.idTareaAsignada, Enumeraciones.DominioDetalle.SolicitudPriorizacion)
        If Not IsNothing(_tareaSolicitud) Then
            'lblErrorPriorizacion.Text = My.Resources.bandejas.errorSinTareaAPriorizar 'Ya existe una solicitud
            lblErrorPriorizacion.Text = "Ya existe una solicitud de priorización pendiente de aprobación"
            lblErrorPriorizacion.Visible = True
            DeshabilitarFormulario()
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Deshabilita todas las opciones del formulario
    ''' </summary>
    Public Sub DeshabilitarFormulario()
        ddlCausalPriorizacion.Enabled = False
        txtObservacionPriorizacion.Enabled = False
        cmdPriorizar.Enabled = False
    End Sub

    ''' <summary>
    ''' Habilita todas las opciones del formulario
    ''' </summary>
    Public Sub HabilitarFormulario()
        ddlCausalPriorizacion.Enabled = True
        txtObservacionPriorizacion.Enabled = True
        cmdPriorizar.Enabled = True
    End Sub

    ''' <summary>
    ''' Abrir el modal
    ''' </summary>
    Public Sub MostrarModal()
        modalPriorizacion.Show()
    End Sub

    ''' <summary>
    ''' Cerrar el modal
    ''' </summary>
    Public Sub CerrarModal()
        modalPriorizacion.Hide()
    End Sub

    ''' <summary>
    ''' Evento para guaradar ña solicitud de priorización
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub cmdPriorizar_Click(sender As Object, e As EventArgs) Handles cmdPriorizar.Click

        Me.idTareaAsignada = Convert.ToInt32(hdnIdTareaAsiganada.Value)

        If Not tareaAsiganadaInicializada() Then
            Exit Sub
        End If

        If existeSolicitudPriorizacion() Then
            Exit Sub
        End If

        Try
            'Se crea una lista con la única tarea que se está priorizando
            Dim _tareasAsignadas As New List(Of Integer)
            _tareasAsignadas.Add(Me.idTareaAsignada)
            'Se crea el objeto que guarda los parámetros de salvado de datos
            Dim _solicitudTarea As New SolicitudTarea()
            _solicitudTarea.IdsTareasignadas = _tareasAsignadas
            _solicitudTarea.UsuarioSolicitante = Session("ssloginusuario")
            _solicitudTarea.TipoSolicitud = Convert.ToInt32(Enumeraciones.DominioDetalle.SolicitudPriorizacion)
            _solicitudTarea.TipologiaSolicitud = ddlCausalPriorizacion.SelectedValue
            _solicitudTarea.SolicitudTareaObservacion = txtObservacionPriorizacion.Text
            'Se llama el objeto que guarda la solicitud y se procesa la solicitud
            Dim _bandejaBLL As BandejaBLL = New BandejaBLL()
            _bandejaBLL.ProcesarSolicitudEnTarea(_solicitudTarea)
            errorPriorizacion = False

            lblPriorizacionEnviada.Visible = True
            DeshabilitarFormulario()
            'CerrarModal()
        Catch ex As Exception
            'TODO: Capturar excepción y guardar en el LOG
            'lblErrorPriorizacion.Text = My.Resources.bandejas.errorProcesandoSolicitud
            lblErrorPriorizacion.Text = "Ha ocurrido un error inesperado, por favor contacte con el administrador"
            lblErrorPriorizacion.Visible = True
            errorPriorizacion = True
        End Try
    End Sub

    Protected Sub btnClose2_Click(sender As Object, e As EventArgs) Handles btnClose2.Click
        CerrarModal()
    End Sub
End Class