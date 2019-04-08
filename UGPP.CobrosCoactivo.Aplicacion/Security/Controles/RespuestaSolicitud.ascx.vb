Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class AprobarSolicitud
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' Objeto para guardar la observación que se crea
    ''' </summary>
    ''' <returns></returns>
    Property tareaObservacion As Entidades.TareaObservacion
    ''' <summary>
    ''' Lista e identificadores de solicitudes que se seleccionan del check de las bandejas
    ''' </summary>
    ''' <returns></returns>
    Property ListaTareasObservacion As List(Of Int32)
    ''' <summary>
    ''' {17: Aprobado, 18: Rechazado}
    ''' </summary>
    Property CodEstadoSolicitud As Int32

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            reqRespuestaSolicitud.Text = My.Resources.Formularios.errorCampoRequerido
        End If
    End Sub

    Public Sub AsignarTareasSolicitud(ByVal prmObjIdTareasSolicitud As List(Of Integer))
        If prmObjIdTareasSolicitud.Count() = 0 Then
            hdnTareasSolicitud.Value = String.Empty
            Exit Sub
        End If
        hdnTareasSolicitud.Value = String.Join(",", prmObjIdTareasSolicitud)
    End Sub

    ''' <summary>
    ''' Establece el tipo de solicitud
    ''' </summary>
    ''' <param name="prmIntTipoSolicitud">{8:SolicitudPriorizacion, 9:SolicitudResignacion}</param>
    Public Sub AsignarTipoSolicitud(ByVal prmIntTipoSolicitud As Int32)
        hdnTipoSolicitud.Value = prmIntTipoSolicitud
    End Sub

    Public Sub AbrirModal()
        If Not validatTareasSolicitud() Then
            desabilitarFormulario()
            modalResSolicitud.Show()
            ddlGestorAsignado.Visible = False
            Exit Sub
        End If

        If hdnTipoSolicitud.Value = "9" Then
            CommonsCobrosCoactivos.poblarGestoresTodos(ddlGestorAsignado)
            ddlGestorAsignado.Visible = True
        Else
            pnlGestor.Enabled = False
            pnlGestor.Visible = False
        End If

        modalResSolicitud.Show()
    End Sub

    Public Sub CerrarModal()
        modalResSolicitud.Hide()
    End Sub

    Public Sub desabilitarFormulario()
        txtRespuestaSolicitud.Enabled = False
        btnAprobarSolicitud.Enabled = False
        btnDenegarSolicitud.Enabled = False
    End Sub

    Public Sub HabilitarFormulario()
        txtRespuestaSolicitud.Enabled = True
        txtRespuestaSolicitud.Text = ""
        btnAprobarSolicitud.Enabled = True
        btnDenegarSolicitud.Enabled = True
    End Sub

    Protected Function validatTareasSolicitud() As Boolean
        If String.IsNullOrEmpty(hdnTareasSolicitud.Value) OrElse hdnTareasSolicitud.Value = "" Then
            lblErrorRespuestaSolicitud.Text = "No se ha encontrado nada para aprobar"
            lblErrorRespuestaSolicitud.Visible = True
            Return False
        End If
        If String.IsNullOrEmpty(hdnTipoSolicitud.Value) OrElse hdnTipoSolicitud.Value = "" Then
            lblErrorRespuestaSolicitud.Text = "No se ha encontrado el tipo de solicitud"
            lblErrorRespuestaSolicitud.Visible = True
            Return False
        End If
        Return True
    End Function

    Protected Sub btnAprobarSolicitud_Click(sender As Object, e As EventArgs) Handles btnAprobarSolicitud.Click
        If Not validatTareasSolicitud() Then
            Exit Sub
        End If

        lblErrorRespuestaSolicitud.Visible = False

        ProcesarSolicitud(1)
    End Sub

    Protected Sub btnDenegarSolicitud_Click(sender As Object, e As EventArgs) Handles btnDenegarSolicitud.Click
        If Not validatTareasSolicitud() Then
            Exit Sub
        End If

        lblErrorRespuestaSolicitud.Visible = False

        ProcesarSolicitud(2)
    End Sub

    Protected Function CrearTareaObservacion() As Entidades.TareaObservacion
        Dim _tareaObservacion As New TareaObservacion()
        _tareaObservacion.OBSERVACION = txtRespuestaSolicitud.Text
        _tareaObservacion.IND_ESTADO = 1
        _tareaObservacion.FEC_CREACION = DateTime.Now

        Dim _tareaObservacionBLL As New TareaObservacionBLL()
        _tareaObservacion = _tareaObservacionBLL.crearTareaObservacion(_tareaObservacion)
        Return _tareaObservacion
    End Function

    Protected Function poblarListaSolicitudes() As Boolean
        'Se crea la lista de los ids de tarea asiganción que se pueden procesar
        Dim _ListaTareasObservacion = Split(hdnTareasSolicitud.Value, ",")
        'Se valida que contenga IDS
        If _ListaTareasObservacion.Count = 0 Then
            lblErrorRespuestaSolicitud.Text = "No se han encontrado solicitudes para procesar"
            lblErrorRespuestaSolicitud.Visible = False
            Return False
        End If
        'Se convierte la lista de strings a lista de enteros
        Dim idsTareasObservacion As New List(Of Integer)
        For Each tarea As String In _ListaTareasObservacion
            idsTareasObservacion.Add(Convert.ToInt32(tarea))
        Next
        Me.ListaTareasObservacion = idsTareasObservacion
        Return True
    End Function

    ''' <summary>
    ''' Procesa la solicitud que se esta realizando, 
    ''' </summary>
    ''' <param name="prmIntEstadoAprobacion">Tipo de respuesta {1: aprobado, 2:Rechazado}</param>
    Protected Sub ProcesarSolicitud(ByVal prmIntEstadoAprobacion As Int32)
        If Not poblarListaSolicitudes() Then
            Exit Sub
        End If

        If hdnTipoSolicitud.Value = "7" Then
            procesarCambioEstado(prmIntEstadoAprobacion)
            Exit Sub
        End If

        Try
            lblErrorRespuestaSolicitud.Visible = False
            lblRespuestaSolicitudEnviada.Visible = False

            'Se crea la observación común a todas las solicitudes
            Me.tareaObservacion = CrearTareaObservacion()

            'Se establece el estado de la solicitud segun el parametro
            Me.CodEstadoSolicitud = If(prmIntEstadoAprobacion = 1, 17, 18)

            'Se recorren la lista de solicitudes
            For Each tareaSolicitud As Int32 In Me.ListaTareasObservacion
                'Se inicializa variable para nuevo usuario en caso de reasignacion
                Dim nuevoUsuario As String = Nothing

                'Se obtiene la tarea Solicitud
                Dim _tareaSolicitudBLL As New TareaSolicitudBLL()
                Dim _tareaSolicitud = _tareaSolicitudBLL.obternerTareaSolicitudPorId(tareaSolicitud)
                'Se actualiza la solicitud
                _tareaSolicitud.ID_TAREA_OBSERVACION = Me.tareaObservacion.COD_ID_TAREA_OBSERVACION
                _tareaSolicitud.IND_SOLICITUD_PROCESADA = 1
                _tareaSolicitud.COD_ESTADO_SOLICITUD = Me.CodEstadoSolicitud
                _tareaSolicitud.FEC_SOLICITUD = DateTime.Now
                _tareaSolicitud.VAL_USUARIO_APROBADOR = Session("ssloginusuario")
                _tareaSolicitud = _tareaSolicitudBLL.ActualizarTareaSolicitud(_tareaSolicitud)

                'Se inicializa variable para tarea asiganda
                Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                Dim _tareaAsignada As New TareaAsignada()

                If Not IsNothing(_tareaSolicitud.ID_TAREA_ASIGNADA) Then
                    'Se obtiene la tarea asiganada
                    _tareaAsignada = _tareaAsignadaBLL.obtenerTareaAsignadaPorId(_tareaSolicitud.ID_TAREA_ASIGNADA)
                    _tareaAsignada.ID_TAREA_OBSERVACION = Me.tareaObservacion.COD_ID_TAREA_OBSERVACION
                    'Se actualiza estado operativo dependiendo del tipo de solicitud

                    '''''''''''''''''' REASIGNACIÓN ''''''''''''''''''
                    If _tareaSolicitud.VAL_TIPO_SOLICITUD = 9 AndAlso prmIntEstadoAprobacion = 1 Then
                        'Reasignacion aprobada
                        If ddlGestorAsignado.SelectedValue <> "0" Then
                            'Se asigna el usuario destino al título o expediente
                            _tareaAsignada.VAL_USUARIO_NOMBRE = ddlGestorAsignado.SelectedValue
                            'Se establece como asiganado dependiendo si es título  o expediente
                            If Not IsNothing(_tareaAsignada.ID_UNICO_TITULO) Then
                                _tareaAsignada.COD_ESTADO_OPERATIVO = 4 'Título asignado
                            Else
                                _tareaAsignada.COD_ESTADO_OPERATIVO = 11 'Expediente asignado
                            End If
                        ElseIf Not IsNothing(_tareaSolicitud.VAL_USUARIO_DESTINO) Then
                            'Se asigna el usuario destino al título o expediente
                            _tareaAsignada.VAL_USUARIO_NOMBRE = _tareaSolicitud.VAL_USUARIO_DESTINO
                            'Se establece como asiganado dependiendo si es título  o expediente
                            If Not IsNothing(_tareaAsignada.ID_UNICO_TITULO) Then
                                _tareaAsignada.COD_ESTADO_OPERATIVO = 4 'Título asignado
                            Else
                                _tareaAsignada.COD_ESTADO_OPERATIVO = 11 'Expediente asignado
                            End If
                        Else
                            'Se deja por repartir el título o expediente
                            _tareaAsignada.VAL_USUARIO_NOMBRE = ""
                            'Se establece po repartir dependiendo si es título  o expediente
                            If Not IsNothing(_tareaAsignada.ID_UNICO_TITULO) Then
                                _tareaAsignada.COD_ESTADO_OPERATIVO = 2 'Título por repartir
                            Else
                                _tareaAsignada.COD_ESTADO_OPERATIVO = 10 'Expediente por repartir
                            End If
                        End If

                    ElseIf _tareaSolicitud.VAL_TIPO_SOLICITUD = 9 AndAlso prmIntEstadoAprobacion = 2 Then
                        'Reasignación rechazada
                        _tareaAsignada = ActualizarEstadoOperativoAsignadoSolicitudRechazada(_tareaAsignada)
                    End If

                    '''''''''''''''''' PRIORIZACIÓN ''''''''''''''''''
                    If _tareaSolicitud.VAL_TIPO_SOLICITUD = 8 Then
                        'En la  priorización siempre se devuelve a tarea asignada
                        _tareaAsignada = ActualizarEstadoOperativoAsignadoSolicitudRechazada(_tareaAsignada)
                    End If
                    If _tareaSolicitud.VAL_TIPO_SOLICITUD = 8 AndAlso prmIntEstadoAprobacion = 1 Then
                        'Priorización aprobada: se actualiza el campo que indica que es un expediente priorizado
                        _tareaAsignada.IND_TITULO_PRIORIZADO = True
                    ElseIf _tareaSolicitud.VAL_TIPO_SOLICITUD = 8 AndAlso prmIntEstadoAprobacion = 2 Then
                        _tareaAsignada.IND_TITULO_PRIORIZADO = False
                    End If
                End If

                _tareaAsignada = _tareaAsignadaBLL.ActualizarTareaAsignada(_tareaAsignada)
                desabilitarFormulario()
                btnClose.Text = "Cerrar"
                hdnReload.Value = "1"
                If _tareaSolicitud.VAL_TIPO_SOLICITUD = 8 Then
                    lblRespuestaSolicitudEnviada.Text = "Priorización realizada correctamente"
                Else
                    lblRespuestaSolicitudEnviada.Text = "Reasignación realizada correctamente"
                End If
                lblRespuestaSolicitudEnviada.Visible = True
            Next
        Catch ex As Exception
            lblErrorRespuestaSolicitud.Text = "Lo sentimos, ha ocurrido un error inesperado, contacte con el administrador"
            lblErrorRespuestaSolicitud.Visible = True
        End Try
    End Sub

    Protected Function ActualizarEstadoOperativoAsignadoSolicitudRechazada(ByVal prmObjTareaAsiganada As TareaAsignada) As TareaAsignada
        'Se establece como asignado sin cambiar el usuario dependiendo si es título  o expediente
        If Not IsNothing(prmObjTareaAsiganada.ID_UNICO_TITULO) Then
            prmObjTareaAsiganada.COD_ESTADO_OPERATIVO = 4 'Título Asignado
        Else
            prmObjTareaAsiganada.COD_ESTADO_OPERATIVO = 11  'Expediente Asignado
        End If
        Return prmObjTareaAsiganada
    End Function

    ''' <summary>
    ''' Procesa la solicitud que se esta realizando de cambio de estado
    ''' </summary>
    ''' <param name="prmIntEstadoAprobacion">Tipo de respuesta {1: aprobado, 2:Rechazado}</param>
    Protected Sub procesarCambioEstado(ByVal prmIntEstadoAprobacion As Int32)
        Try
            lblErrorRespuestaSolicitud.Visible = False
            lblRespuestaSolicitudEnviada.Visible = False

            'Se establece el estado de la solicitud segun el parametro
            Me.CodEstadoSolicitud = If(prmIntEstadoAprobacion = 1, 17, 18)

            'Se crea el obgeto base de cambio de estado entidad
            Dim _Solicitudes_CambioEstado As New Entidades.Solicitudes_CambioEstado
            _Solicitudes_CambioEstado.ejecutor = Session("sscodigousuario") 'Usuario que realiza la aprobación
            _Solicitudes_CambioEstado.aprob_revisor = If(prmIntEstadoAprobacion = 1, "SI", "NO")
            _Solicitudes_CambioEstado.nota_revisor = txtRespuestaSolicitud.Text

            'Obgeto de negocio
            Dim _solicitudCambiosEstadoBLL As New SolicitudCambiosEstadoBLL()

            'Se recorren la lista de solicitudes
            For Each tareaSolicitudCE As Int32 In Me.ListaTareasObservacion
                'Se crea el objeto que actualiza el estado del cambio de estado
                Dim _Solicitudes_CambioEstadoLoop As New Entidades.Solicitudes_CambioEstado
                _Solicitudes_CambioEstadoLoop = _Solicitudes_CambioEstado
                _Solicitudes_CambioEstadoLoop.idunico = tareaSolicitudCE
                'Sa actualiza la solicitud
                _Solicitudes_CambioEstadoLoop = _solicitudCambiosEstadoBLL.actualizarEstadoAprobacionCambioEstado(_Solicitudes_CambioEstadoLoop)

                If _Solicitudes_CambioEstadoLoop.aprob_revisor = "SI" Then
                    'Si la solicitud es aprobada se asigna el nuevo estado y etapa procesal al expediente
                    Dim _ejefisglobalBLL As New EjefisglobalBLL
                    Dim NroExp = _Solicitudes_CambioEstadoLoop.NroExp
                    Dim NuevoEstado = _Solicitudes_CambioEstadoLoop.estado
                    Dim NuevaEtapa = _Solicitudes_CambioEstadoLoop.efietapaprocesal
                    _ejefisglobalBLL.ActualizarExpedienteEtapaProcesal(NroExp, NuevoEstado, If(IsNothing(NuevaEtapa), 0, NuevaEtapa))
                End If

                Dim tareaAsignadaBLL As New TareaAsignadaBLL()
                Dim _tareaAsignada = tareaAsignadaBLL.obtenerTareaAsignadaPorIdExpediente(_Solicitudes_CambioEstadoLoop.NroExp)
                Dim nuevoEstadoOperativo = If(_Solicitudes_CambioEstadoLoop.aprob_revisor = "SI", 10, 13) 'Se establece expediente a estado operativo por repartir si es aprobador, en caso contrario se deja como en gestión al mismo gestor
                'Se obtiene el ID del estado que representa la devolución a estudio de títulos
                Dim _dominioDetalleBLL As New DominioDetalleBLL
                Dim _dominioDetalle = _dominioDetalleBLL.consultarDominioPorIdDominio(Enumeraciones.Dominio.EstadoProcesalEstudioTitulos)
                Dim ExpedienteRetorno = _dominioDetalle.FirstOrDefault()
                If (_Solicitudes_CambioEstadoLoop.estado = ExpedienteRetorno.VAL_VALOR) And (_Solicitudes_CambioEstadoLoop.aprob_revisor = "SI") Then
                    nuevoEstadoOperativo = 13 'Si es una solicitud para cambio de estado procesal estudio de títulos se deja como pendiente
                    'Si es una devolución a estudio de títulos se retorna a estudio de títulos
                    'Se obtiene el título por el número del expediente
                    Dim _maestroTitulosBLL As New MaestroTitulosBLL
                    Dim titulo As Entidades.MaestroTitulos = _maestroTitulosBLL.obtenerTituloPorExpedienteId(_Solicitudes_CambioEstadoLoop.NroExp)
                    'Se llama la tarea para reactivarla al gestor
                    Dim _tareaAsignadaBLL As New TareaAsignadaBLL
                    Dim _tarea = _tareaAsignadaBLL.obtenerTareaAsignadaPorIdTitulo(titulo.idunico)
                    If Not IsNothing(_tarea) Then
                        _tarea.COD_ESTADO_OPERATIVO = 9 'Estado operativo devolución a un gestor de estudio de títulos
                        _tareaAsignadaBLL.ActualizarTareaAsignada(_tarea)
                    End If
                End If
                    tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(_tareaAsignada.ID_TAREA_ASIGNADA, nuevoEstadoOperativo)
            Next
            desabilitarFormulario()
            btnClose.Text = "Cerrar"
            hdnReload.Value = "1"
            lblRespuestaSolicitudEnviada.Text = "Cambio de estado realizado correctamente"
            lblRespuestaSolicitudEnviada.Visible = True
        Catch ex As Exception
            lblErrorRespuestaSolicitud.Text = "Lo sentimos, ha ocurrido un error inesperado, contacte con el administrador"
            lblErrorRespuestaSolicitud.Visible = True
        End Try
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If hdnReload.Value = "1" Then
            Response.Redirect(Request.RawUrl, True)
        End If
        CerrarModal()
    End Sub

    Public Sub OcultarMensajeError()
        lblErrorRespuestaSolicitud.Visible = False
    End Sub
End Class