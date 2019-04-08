Imports System.Data.SqlClient
Imports System.Globalization
Imports coactivosyp.My.Resources
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoativo

Partial Public Class EditMAESTRO_TITULOS_AORIGEN
    Inherits PaginaBase

    Dim documentoTipoTituloBLL As DocumentoTipoTituloBLL
    Dim tipoTituloBLL As TipoTituloBLL
    Dim tipoCarteraBLL As TipoCarteraBLL
    Dim tipoAreaOrigenBLL As TipoAreaOrigenBLL
    Dim obtenDatosValoresBLL As ObtenDatosValoresBLL
    Dim tareaAsignadaBLL As TareaAsignadaBLL
    Dim tareaAsignadaObject As TareaAsignada
    Dim almacenamientoTemporalBLL As AlmacenamientoTemporalBLL
    Dim dominioDetalleBLL As DominioDetalleBLL
    Dim observacionCumpleBLL As ObservacionCumpleBLL

    Protected Overrides Sub OnInit(e As EventArgs)
        documentoTipoTituloBLL = New DocumentoTipoTituloBLL()
        tipoTituloBLL = New TipoTituloBLL()
        tipoCarteraBLL = New TipoCarteraBLL()
        tipoAreaOrigenBLL = New TipoAreaOrigenBLL()
        obtenDatosValoresBLL = New ObtenDatosValoresBLL()
        Dim ExtensionesValidas As String() = StringsResourse.Documentos_extenciones.Split(",")
        ViewState("ExtValidas") = ExtensionesValidas
        HdnExtValidas.Value = StringsResourse.Documentos_extenciones
        tareaAsignadaBLL = New TareaAsignadaBLL()
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL()
        dominioDetalleBLL = New DominioDetalleBLL()
        observacionCumpleBLL = New ObservacionCumpleBLL()
    End Sub
    Public Sub VistaValidarCumpleYObservaciones(EstadoOperativo As Int32)
        Dim MostrarObserva = 0
        'Mostrar control cumple 
        Select Case EstadoOperativo
            Case 8, 5, 7, 3, 4, 9 ' Subsanar, Retorno, Stand By, Engestión, EnSolicitud, Asignado
                MostrarObserva = 1
                PnlCNCOb.Visible = True
        End Select
        Dim _expedienteBLL As New ExpedienteBLL()
        HdnMostrarObser.Value = MostrarObserva
    End Sub
    Public Sub OcultarCumpleGeneralTituloExpediente()
        PnlCNCOb.Enabled = False
        BtnEnviarCalificacion.Visible = False
        BtnGuardar1.Visible = False
        BtnGuardar2.Visible = False
        BtnGuardar3.Visible = False
    End Sub
    Public Sub VistaCargarControles()
        CommonsCobrosCoactivos.poblarTipificaciones(ChkBltsTipifica)
        LoadcboMT_tiposentencia()
        LoadcboMT_for_notificacion_titulo()
        LoadcboMT_for_not_reso_resu_reposicion()
        LoadcboMT_for_not_reso_apela_recon()
        LoadcboPROCEDENCIA()
        LoadTipo_Cartera()
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'BtnFinalizaC.Visible = False
            If Len(Request("Edit")) > 0 Then
                ABack.Visible = True
            End If
            If Session("mnivelacces") = 10 Then
                nombreModulo = "ESTUDIO_TITULOS"
            End If

            If Session("mnivelacces") = 11 Then
                nombreModulo = "AREA_ORIGEN"
                ValidaCheckEjecutoria() 'Se deshabilita la fecha de ejecutoria
            End If
            VistaCargarControles()
            btnSuspenderTitulo.Visible = False
            'Activar/Inactivar controles en funcion del usuario logueado
            ActivarControles()
            Dim tituloEjecutivoObj As TituloEjecutivoExt
            BtnEliminar.Visible = False
            BtnEnviarTitulo.Visible = False
            BtnEnviarCalificacion.Visible = False
            'BtnVerInfo.Visible = False
            If Session("mnivelacces") = 11 Then
                BtnEnviarTitulo.Visible = True
            End If
            If Len(Request("ID_TASK")) > 0 Then
                'Si ID_TASK tiene valor se carga valida el item
                tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(Request("ID_TASK").ToString()))

                If tareaAsignadaObject.ID_UNICO_TITULO.HasValue And tareaAsignadaObject.ID_UNICO_TITULO <> 0 Then
                    HdnIdunico.Value = tareaAsignadaObject.ID_UNICO_TITULO
                    If Session("mnivelacces") = 10 And tareaAsignadaObject.COD_ESTADO_OPERATIVO = 5 Then
                        btnSuspenderTitulo.Visible = True
                    End If
                    lblNroTitulo.Text = tareaAsignadaObject.ID_UNICO_TITULO
                Else
                    divTitleNumber.Visible = False
                End If
                If tareaAsignadaObject.COD_ESTADO_OPERATIVO = 8 Then
                    cboMT_tipo_titulo.Enabled = False
                    cboTipo_Cartera.Enabled = False
                End If
                If Session("mnivelacces") = 10 Then
                    BtnEnviarCalificacion.Visible = True
                End If
                'Acciones solo de AreaOrigen
                If Session("mnivelacces") = 11 Then
                    BtnEliminar.Visible = True
                    PnlCNCOb.Visible = False
                End If
                HdnIdTask.Value = Request("ID_TASK")
                HdnIdEstadoOperativo.Value = tareaAsignadaObject.COD_ESTADO_OPERATIVO
                Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(tareaAsignadaObject.ID_TAREA_ASIGNADA)
                If almacenamientoTemportalItem Is Nothing Then
                    almacenamientoTemportalItem = New AlmacenamientoTemporal()
                    almacenamientoTemportalItem.ID_TAREA_ASIGNADA = tareaAsignadaObject.ID_TAREA_ASIGNADA
                    almacenamientoTemportalItem.JSON_OBJ = String.Empty
                    almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("tareaasignada=" + almacenamientoTemportalItem.ID_TAREA_ASIGNADA.ToString() & "jsonobj=" + almacenamientoTemportalItem.JSON_OBJ))
                    almacenamientoTemporalBLL.InsertarAlmacenamiento(almacenamientoTemportalItem)
                End If

                If String.IsNullOrEmpty(HdnIdunico.Value) = False And String.IsNullOrEmpty(almacenamientoTemportalItem.JSON_OBJ) Then
                    HdnIdunico.Value = tareaAsignadaObject.ID_UNICO_TITULO
                    Dim MetodoSel As New ValidadorBLL()
                    tituloEjecutivoObj = MetodoSel.ConsultarTituloEjecutivo(HdnIdunico.Value)
                    If tituloEjecutivoObj Is Nothing Then
                        redirectBandejaCorrespondiente()
                    End If
                    'Se guarda para el guardado parcial de estudio de titulos
                    almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(tituloEjecutivoObj)
                    almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("jsonobj=" + almacenamientoTemportalItem.JSON_OBJ))
                    almacenamientoTemporalBLL.actualizarAlmacenamiento(almacenamientoTemportalItem)
                Else
                    'Si TareaAsignadaObject.ID_UNICO_TITULO esta vacio o ya tiene json guardado realiza el cargue del objeto parcial
                    tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
                End If

                If tituloEjecutivoObj IsNot Nothing Then
                    HdnAutomatico.Value = tituloEjecutivoObj.TituloEjecutivo.Automatico.ToString()
                    If tituloEjecutivoObj.TituloEjecutivo.Automatico Then
                        HdnAutomaticoCargue.Value = HabilitarCargue(tituloEjecutivoObj.TituloEjecutivo.IdunicoTitulo).ToString()
                    End If

                    CargarTitulo(tituloEjecutivoObj.TituloEjecutivo, tituloEjecutivoObj.ObservacionTitulo, tituloEjecutivoObj.LstTipificaciones, tituloEjecutivoObj.LstDocumentos)
                    LstNotificaciones.DataSource = tituloEjecutivoObj.LstNotificacion
                    LstNotificaciones.DataBind()
                    loadItemsPorTipoObligacion()
                    If tituloEjecutivoObj.TituloEjecutivo.Automatico Then
                        loadDocumentos()
                    End If
                End If
                VistaValidarCumpleYObservaciones(tareaAsignadaObject.COD_ESTADO_OPERATIVO)
            Else
                HdnAutomaticoCargue.Value = False
                HdnIdEstadoOperativo.Value = 1
                loadItemsPorTipoObligacion()
                VistaValidarCumpleYObservaciones(1) 'EnCreacion
                divTitleNumber.Visible = False
                If Session("mnivelacces") = 11 Then
                    BtnNuevo.Visible = True
                End If
                'si no hay id de la tarea es creacion del titulo
                Inicializardeuda()
            End If
            If String.IsNullOrEmpty(HdnAutomaticoCargue.Value.ToString()) Then
                HdnAutomaticoCargue.Value = False
            End If
#Region "EstudioTitulo"
            'Validaciones estudio de títulos 
            If Session("mnivelacces") = 10 Then
                Dim ocultarEstudio As Boolean = False
                'Se verifica si el título esta asignado al gestor que está ingresando 
                If (tareaAsignadaObject.VAL_USUARIO_NOMBRE <> Session("ssloginusuario")) Then
                    lblErrorUsuarioAsignado.Text = My.Resources.Titulos.lblUsuarioAsignado
                    lblErrorUsuarioAsignado.Visible = True
                    ocultarEstudio = True
                End If
                'Se verifica si es un título
                If Not ocultarEstudio AndAlso tareaAsignadaObject.COD_TIPO_OBJ <> 4 Then
                    lblErrorNoEsTitulo.Text = My.Resources.Titulos.lblErrorNoEsTitulo
                    lblErrorNoEsTitulo.Visible = True
                    ocultarEstudio = True
                End If
                'Se verifica si los estados operativos que se solicitan son los autorizados
                Dim codEstadoOperativo = tareaAsignadaObject.COD_ESTADO_OPERATIVO
                If Not ocultarEstudio AndAlso (codEstadoOperativo <> 5 AndAlso codEstadoOperativo <> 4 AndAlso codEstadoOperativo <> 7 AndAlso codEstadoOperativo <> 9) Then
                    lblErrorEstadoOperativoErroneo.Text = My.Resources.Titulos.lblErrorEstadoOperativoErroneo
                    lblErrorEstadoOperativoErroneo.Visible = True
                    ocultarEstudio = True
                End If
                'Se verifica si el título tiene la prioridad requerida
                If Not ocultarEstudio AndAlso (tareaAsignadaObject.VAL_PRIORIDAD <> 1 AndAlso tareaAsignadaObject.IND_TITULO_PRIORIZADO <> 1) Then
                    lblErrorPrioridad.Text = My.Resources.Titulos.lblErrorPrioridad
                    lblErrorPrioridad.Visible = True
                    ocultarEstudio = True
                End If

                If Not ocultarEstudio Then
                    'Consulto el título en la base de datos
                    Dim _maestroTitulosBLL As New MaestroTitulosBLL()
                    Dim titulo = _maestroTitulosBLL.consultarTituloPorID(tareaAsignadaObject.ID_UNICO_TITULO)
                    'y verifico que no tenga tenga expediente
                    If Not IsNothing(titulo.MT_expediente) Then
                        If Session("mnivelacces") = 11 Then
                            btnShowTerminar.Visible = (tareaAsignadaObject.COD_ESTADO_OPERATIVO = 8)
                        End If
                        'Si el expediente ya se encuentra clasificado se retorna a la bandeja de estudio de títulos
                        Dim _expedienteBLL As New ExpedienteBLL()
                        Dim expediente = _expedienteBLL.obtenerExpedientePorId(titulo.MT_expediente)
                        HdnExpediente.Value = expediente.EFINROEXP
                        'Si tiene expediente asignado se debe bloquear el cumple no cumple
                        If Not IsNothing(expediente.EFIESTADO) And expediente.EFIESTADO <> 13 Then
                            lblErrorTituloClasificado.Text = My.Resources.Titulos.lblErrorTituloClasificado
                            lblErrorTituloClasificado.Visible = True
                            ocultarEstudio = True
                        ElseIf expediente.EFIESTADO <> 13 Then
                            MostrarMensajeClasificacion()
                            OcultarCumpleGeneralTituloExpediente()
                        End If
                    End If
                End If

                If ocultarEstudio Then
                    tabs1.Visible = False
                    tabs2.Visible = False
                    tabs3.Visible = False
                    tabs4.Visible = False
                    tblGralCumpleNoCumple.Visible = False
                    ModalValidacionInicial.Show()
                    'Exit Sub
                End If
                If Boolean.Parse(HdnAutomatico.Value) Then
                    lsvListaDocumentos.Visible = False
                    PnlDocAutomatico.Visible = True
                    'HdnMostrarObser.Value = "0"
                    'PnlCNCOb.Visible = False
                    'PnlCNCOb.Enabled = False
                    'BtnEnviarCalificacion.Visible = False
                    'BtnGuardar1.Visible = False
                    'BtnGuardar2.Visible = False
                    'BtnGuardar3.Visible = True
                    'BtnFinalizaC.Visible = True
                End If
            End If
#End Region

            AsignarGestion()

            lblMalla.Text = My.Resources.errorGeneral

        End If
    End Sub

    ''' <summary>
    ''' Muestra el mensaje para proceder a la clasificación manual
    ''' </summary>
    Private Sub MostrarMensajeClasificacion()
        lblMensajeClasificacion.Text = My.Resources.Titulos.lblMensajeClasificacion
        ModalClasificacion.Show()
        BtnEnviarCalificacion.Visible = False
    End Sub

    ''' <summary>
    ''' Metodo para actualizar la vista apartir del objeto TituloEjecutivoExt
    ''' </summary>
    ''' <param name="tituloEjecutivoObj"></param>
    Public Sub CargarTitulo(tituloEjecutivoObj As TituloEjecutivo, ObservacionTitulo As ObservacionesCNC, LsvTipificaciones As List(Of TipificacionCNC), LsvDocumentosTitulo As List(Of DocumentoMaestroTitulo))
#Region "InformacionTitulo"
        'Mostrar datos del titulo ejecutivo
        If tituloEjecutivoObj.numeroExpedienteOrigen IsNot Nothing Then
            txtEFIEXPORIGEN.Text = tituloEjecutivoObj.numeroExpedienteOrigen.ToString()
        End If
        If tituloEjecutivoObj.Automatico Then
            txtAutomatico.Text = StringsResourse.LblTituloAutomatico
            'If Session("mnivelacces") = 10 Then
            '    BtnVerInfo.Visible = True
            'End If
            'Solo para los automáticos
            If tituloEjecutivoObj.Automatico Then
                Dim MetodoSel As New ValidadorBLL()
                Dim LstDocumentosMaestro As List(Of DocumentoMaestroTitulo) = MetodoSel.ConsultarDocumentos(tituloEjecutivoObj.IdunicoTitulo)
                grdDocAutomatico.DataSource = LstDocumentosMaestro.Where(Function(x) (x.TIPO_RUTA = 2)).ToList()
                grdDocAutomatico.DataBind()
            End If
        End If

        txtMT_nro_titulo.Text = tituloEjecutivoObj.numeroTitulo
        If tituloEjecutivoObj.areaOrigen.HasValue And tituloEjecutivoObj.areaOrigen > 0 Then
            cboPROCEDENCIA.SelectedValue = Convert.ToInt32(tituloEjecutivoObj.areaOrigen)
            LoadTipo_Cartera()
        End If
        If String.IsNullOrEmpty(tituloEjecutivoObj.tipoCartera) = False Then
            cboTipo_Cartera.SelectedValue = tituloEjecutivoObj.tipoCartera.ToString()
        End If
        If String.IsNullOrEmpty(tituloEjecutivoObj.tipoTitulo) = False Then
            cboMT_tipo_titulo.SelectedValue = tituloEjecutivoObj.tipoTitulo.ToString()
        End If

        If String.IsNullOrEmpty(tituloEjecutivoObj.CodTipSentencia) = False And tituloEjecutivoObj.CodTipSentencia > 0 Then
            cboMT_tiposentencia.SelectedValue = tituloEjecutivoObj.CodTipSentencia.ToString()
        End If




        'Procedencia del titulo

        If tituloEjecutivoObj.tituloEjecutivoFalloCasacion Is Nothing Then
            tituloEjecutivoObj.tituloEjecutivoFalloCasacion = New TituloEspecial()
        End If

        If tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion Is Nothing Then
            tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion = New TituloEspecial()
        End If

        If tituloEjecutivoObj.tituloEjecutivoRecursoReposicion Is Nothing Then
            tituloEjecutivoObj.tituloEjecutivoRecursoReposicion = New TituloEspecial()
        End If

        If tituloEjecutivoObj.tituloEjecutivoSentenciaSegundaInstancia Is Nothing Then
            tituloEjecutivoObj.tituloEjecutivoSentenciaSegundaInstancia = New TituloEspecial()
        End If

        txtMT_fec_expedicion_titulo.Text = ReturnStringDate(tituloEjecutivoObj.fechaTituloEjecutivo)
        txtMT_fec_notificacion_titulo.Text = ReturnStringDate(tituloEjecutivoObj.fechaNotificacion)
        If String.IsNullOrEmpty(tituloEjecutivoObj.formaNotificacion) = False Then
            cboMT_for_notificacion_titulo.SelectedValue = tituloEjecutivoObj.formaNotificacion.ToString()
        End If

        If String.IsNullOrEmpty(tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.formaNotificacion) = False Then
            cboMT_for_not_reso_resu_reposicion.SelectedValue = tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.formaNotificacion
        End If

        If String.IsNullOrEmpty(tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.formaNotificacion) = False Then
            cboMT_for_not_reso_apela_recon.SelectedValue = tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.formaNotificacion
        End If

        txtTotalObligacion.Attributes.Add("readonly", "readonly")
        txtTotalPartidaGlobal.Attributes.Add("readonly", "readonly")
        txtTotalSancion.Attributes.Add("readonly", "readonly")
        txtTotalDeuda.Attributes.Add("readonly", "readonly")

        If String.IsNullOrEmpty(tituloEjecutivoObj.MT_res_resuelve_reposicion) = False Then
            txtMT_res_resuelve_reposicion.Text = tituloEjecutivoObj.MT_res_resuelve_reposicion
        End If
        If String.IsNullOrEmpty(tituloEjecutivoObj.MT_res_resuelve_reposicion) Then
            txtMT_res_resuelve_reposicion.Text = tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.numeroTitulo
        End If

        txtMT_fec_expe_resolucion_reposicion.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo)
        txtMT_fec_not_reso_resu_reposicion.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.fechaNotificacion)

        If String.IsNullOrEmpty(tituloEjecutivoObj.MT_reso_resu_apela_recon) = False Then
            txtMT_reso_resu_apela_recon.Text = tituloEjecutivoObj.MT_reso_resu_apela_recon
        End If
        If String.IsNullOrEmpty(tituloEjecutivoObj.MT_reso_resu_apela_recon) Then
            txtMT_reso_resu_apela_recon.Text = tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.numeroTitulo
        End If

        txtMT_fec_exp_reso_apela_recon.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo)
        txtMT_fec_not_reso_apela_recon.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.fechaNotificacion)

        If tituloEjecutivoObj.tituloEjecutoriado Then
            chkTituloEjecutoriado.Checked = True
        End If
        ValidaCheckEjecutoria()

        txtMT_fecha_ejecutoriaObli.Text = ReturnStringDate(tituloEjecutivoObj.fechaEjecutoria)
        txtMT_fec_exi_liqObli.Text = ReturnStringDate(tituloEjecutivoObj.fechaExigibilidad)
        txtMT_fec_cad_presc.Text = ReturnStringDate(tituloEjecutivoObj.fechaCaducidadPrescripcion)

        '------------------------------------------------------------------/
        'Se cruza contra la nueva versión del formato de los miles, no se suman los valores ya que estos se cargan en el front
        txtValorObligacion.Text = formatoColombia((tituloEjecutivoObj.valorTitulo).ToString())
        txtPartidaGlobal.Text = formatoColombia((tituloEjecutivoObj.partidaGlobal).ToString())
        txtSancionOmision.Text = formatoColombia((tituloEjecutivoObj.sancionOmision).ToString())
        txtSancionMora.Text = formatoColombia((tituloEjecutivoObj.sancionMora).ToString())
        txtSancionInexactitud.Text = formatoColombia((tituloEjecutivoObj.sancionInexactitud).ToString())
#End Region
#Region "Cargue Cumple General"
        If HdnIdEstadoOperativo.Value <> 1 Then
            PnlInformacionDocumento.Enabled = False
            PnlValores.Enabled = False
            PnlDeudores.Enabled = False
        End If

        If Session("mnivelacces") = 11 And HdnIdEstadoOperativo.Value = 8 Then
            PnlInformacionDocumento.Enabled = True
            PnlValores.Enabled = True
            PnlDeudores.Enabled = True
        End If
        TxtobservaCumpleNoCumple.Text = ObservacionTitulo.OBSERVACIONES
        If LsvTipificaciones.Count() > 0 Then
            Dim LsvTipificacionesSelect As List(Of Int64) = LsvTipificaciones.Select(Function(t) (t.ID_TIPIFICACION)).ToList()
            Dim LsvTipificacionesBloq As List(Of Int64) = LsvTipificaciones.Where(Function(t) (t.HABILITADO)).Select(Function(t) (t.ID_TIPIFICACION)).ToList()
            'Validar tipificaciones
            For Each item As ListItem In ChkBltsTipifica.Items
                If LsvTipificacionesSelect.Contains(item.Value) Then
                    item.Selected = True
                End If
                If LsvTipificacionesBloq.Contains(item.Value) Then
                    item.Enabled = False
                End If
            Next
        End If
        If ObservacionTitulo.CUMPLE_NOCUMPLE Then
            RbtnSiNoCumple.SelectedIndex = 0
            trTipificaciones.Visible = False
        Else
            RbtnSiNoCumple.SelectedIndex = 1
        End If

        If Session("mnivelacces") = 11 Then
            RbtnSiNoCumple.Enabled = False
            ChkBltsTipifica.Enabled = False
        End If
#End Region
    End Sub
    ''' <summary>
    ''' Asigna un formato valido para colombia 
    ''' </summary>
    ''' <param name="valor"></param>
    ''' <returns></returns>
    Function formatoColombia(ByVal valor As String) As String
        If String.IsNullOrEmpty(valor) = False Then
            valor = String.Format("{0:#,0.#}", Decimal.Parse(valor))
        End If

        Return valor
    End Function

    ''' <summary>
    ''' Formato base para calculos y guardado
    ''' </summary>
    ''' <param name="valor"></param>
    ''' <returns></returns>
    Function formatoBase(ByVal valor As String) As String
        valor = valor.Replace(".", "")
        valor = valor.Replace(",", ".")
        Return valor
    End Function

    Public Function ValidDecimal(ByVal decimalItem As Decimal?) As Decimal
        If decimalItem.HasValue And Double.TryParse(decimalItem, 0) Then
            Dim res = Double.Parse(decimalItem, CultureInfo.InvariantCulture)
            Return res
        End If
        Return 0
    End Function

    Public Function RetornarDecimalDeString(ByVal val As String) As Decimal
        val = formatoBase(val)
        If String.IsNullOrEmpty(val) = False Then
            Return Double.Parse(val, CultureInfo.InvariantCulture)
        Else
            Return 0
        End If
    End Function


    Public Function ReturnStringDate(ByVal dateitem As Date?) As String
        If dateitem IsNot Nothing Then
            If dateitem <= Date.Parse("1/1/1900 12:00:00 AM") Then
                Return ""
            Else
                Return Left(dateitem.ToString().Trim, 10)
            End If
        End If
        Return ""
    End Function

    Private Sub Inicializardeuda()
        txtTotalObligacion.Attributes.Add("readonly", "readonly")
        txtTotalPartidaGlobal.Attributes.Add("readonly", "readonly")
        txtTotalSancion.Attributes.Add("readonly", "readonly")
        txtTotalDeuda.Attributes.Add("readonly", "readonly")
    End Sub
    Public Sub EliminarTareaYAlmacenamiento()
        If (String.IsNullOrEmpty(HdnIdTask.Value)) = False Then
            tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("TAREAASIGNADA.ID=" + HdnIdTask.Value.ToString))
            tareaAsignadaBLL.eliminarTareaYalmacenamiento(Int64.Parse(HdnIdTask.Value.ToString()))
        End If
    End Sub
    Protected Sub cmdCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancelar3.Click, BtnCancelar2.Click, BtnCancelar1.Click
        ControlSubExeptionLog(AddressOf Cancelar)
    End Sub
    Public Sub Cancelar()
        If Len(Request("Edit")) = 0 Then
            EliminarTareaYAlmacenamiento()
            resetFormulario()
        Else
            redirectBandejaCorrespondiente()
        End If
    End Sub

    Private Sub resetFormulario()
        Response.Redirect("~/Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx?AreaOrigenId=" & Session("usrAreaOrgen"), False)
    End Sub

    Public Sub redirectBandejaCorrespondiente()
        If Request("Edit").ToString() = "1" Then
            Response.Redirect("~/Security/Area_Origen/BandejaAreaOrigen.aspx", False)
        End If
        If Request("Edit").ToString() = "2" Then
            Response.Redirect("~/Security/Maestros/estudio-titulos/BandejaTitulos.aspx", False)
        End If
    End Sub

    Protected Sub cmdBuscar_Click(ByVal sender As Object, ByVal e As EventArgs)
        BuscarDocumentoDocumentic()
        PaginadorGridView.UpdateLabels()
    End Sub

    Public Sub guardarTitulo()
        If Len(Request("ID_TASK")) = 0 And String.IsNullOrEmpty(HdnIdTask.Value) Then
            crearTarea()
        End If
        Dim tituloEjecutivoObj As TituloEjecutivoExt
        Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(HdnIdTask.Value.ToString()))
        tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
#Region "ValidacionesGuardeParcial"

        tituloEjecutivoObj.TituloEjecutivo.numeroTitulo = txtMT_nro_titulo.Text
        tituloEjecutivoObj.TituloEjecutivo.tipoTitulo = cboMT_tipo_titulo.SelectedValue
        tituloEjecutivoObj.TituloEjecutivo.tipoCartera = CType(cboTipo_Cartera.SelectedValue, Enumeraciones.TipoCartera)
        tituloEjecutivoObj.TituloEjecutivo.CodTipSentencia = Int32.Parse(cboMT_tiposentencia.SelectedValue)

        If String.IsNullOrEmpty(cboPROCEDENCIA.SelectedValue) = False Then
            tituloEjecutivoObj.TituloEjecutivo.areaOrigen = Convert.ToInt32(cboPROCEDENCIA.SelectedValue)
        End If

        If String.IsNullOrEmpty(txtMT_fec_expedicion_titulo.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.fechaTituloEjecutivo = Date.Parse(txtMT_fec_expedicion_titulo.Text)
        Else
            tituloEjecutivoObj.TituloEjecutivo.fechaTituloEjecutivo = Nothing
        End If

        If String.IsNullOrEmpty(txtMT_fec_notificacion_titulo.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.fechaNotificacion = Date.Parse(txtMT_fec_notificacion_titulo.Text)
        Else
            tituloEjecutivoObj.TituloEjecutivo.fechaNotificacion = Nothing
        End If

        tituloEjecutivoObj.TituloEjecutivo.formaNotificacion = cboMT_for_notificacion_titulo.SelectedValue

        tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReposicion.numeroTitulo = txtMT_res_resuelve_reposicion.Text


        If String.IsNullOrEmpty(txtMT_fec_expe_resolucion_reposicion.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo = txtMT_fec_expe_resolucion_reposicion.Text
        Else
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo = Nothing
        End If

        If String.IsNullOrEmpty(txtMT_fec_not_reso_resu_reposicion.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion = txtMT_fec_not_reso_resu_reposicion.Text
        Else
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion = Nothing
        End If

        tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReposicion.formaNotificacion = cboMT_for_not_reso_resu_reposicion.SelectedValue

        tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.numeroTitulo = txtMT_reso_resu_apela_recon.Text


        If String.IsNullOrEmpty(txtMT_fec_exp_reso_apela_recon.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo = Date.Parse(txtMT_fec_exp_reso_apela_recon.Text)
        Else
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo = Nothing
        End If
        If String.IsNullOrEmpty(txtMT_fec_not_reso_apela_recon.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion = Date.Parse(txtMT_fec_not_reso_apela_recon.Text)
        Else
            tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion = Nothing
        End If
        tituloEjecutivoObj.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.formaNotificacion = cboMT_for_not_reso_apela_recon.SelectedValue

        tituloEjecutivoObj.TituloEjecutivo.tituloEjecutoriado = If(chkTituloEjecutoriado.Checked, 1, 0)
        If String.IsNullOrEmpty(txtMT_fecha_ejecutoriaObli.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.fechaEjecutoria = Date.Parse(txtMT_fecha_ejecutoriaObli.Text)
        Else
            tituloEjecutivoObj.TituloEjecutivo.fechaEjecutoria = Nothing
        End If
        If String.IsNullOrEmpty(txtMT_fec_exi_liqObli.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.fechaExigibilidad = Date.Parse(txtMT_fec_exi_liqObli.Text)
        Else
            tituloEjecutivoObj.TituloEjecutivo.fechaExigibilidad = Nothing
        End If
        If String.IsNullOrEmpty(txtMT_fec_cad_presc.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.fechaCaducidadPrescripcion = Date.Parse(txtMT_fec_cad_presc.Text)
        Else
            tituloEjecutivoObj.TituloEjecutivo.fechaCaducidadPrescripcion = Nothing
        End If

        tituloEjecutivoObj.TituloEjecutivo.valorTitulo = RetornarDecimalDeString(txtValorObligacion.Text)
        tituloEjecutivoObj.TituloEjecutivo.partidaGlobal = RetornarDecimalDeString(txtPartidaGlobal.Text)
        tituloEjecutivoObj.TituloEjecutivo.sancionOmision = RetornarDecimalDeString(txtSancionOmision.Text)
        tituloEjecutivoObj.TituloEjecutivo.sancionMora = RetornarDecimalDeString(txtSancionMora.Text)
        tituloEjecutivoObj.TituloEjecutivo.sancionInexactitud = RetornarDecimalDeString(txtSancionInexactitud.Text)
        tituloEjecutivoObj.TituloEjecutivo.totalObligacion = RetornarDecimalDeString(txtTotalObligacion.Text)
        tituloEjecutivoObj.TituloEjecutivo.partidaGlobal = RetornarDecimalDeString(txtTotalPartidaGlobal.Text)

        If String.IsNullOrEmpty(txtEFIEXPORIGEN.Text) = False Then
            tituloEjecutivoObj.TituloEjecutivo.numeroExpedienteOrigen = txtEFIEXPORIGEN.Text
        Else
            tituloEjecutivoObj.TituloEjecutivo.numeroExpedienteOrigen = Nothing
        End If
#End Region

        ' Notificaciones
        Dim lstNotificacionesItems As List(Of NotificacionTitulo)
        lstNotificacionesItems = New List(Of NotificacionTitulo)
        If LstNotificaciones.Items.Count() > 0 Then
            For Each item As ListViewItem In LstNotificaciones.Items
                Dim ObjectItems As NotificacionTitulo
                ObjectItems = New NotificacionTitulo()
                Dim HdnItemObj As HiddenField = CType(item.FindControl("HdnItemObj"), HiddenField)
                Dim Format = "dd/MM/yyyy"
                Dim dateTimeConverter = New IsoDateTimeConverter()
                dateTimeConverter.DateTimeFormat = Format

                lstNotificacionesItems.Add(JsonConvert.DeserializeObject(Of NotificacionTitulo)(HdnItemObj.Value, dateTimeConverter))
            Next
        End If
        tituloEjecutivoObj.LstNotificacion = lstNotificacionesItems
        'Documentos
        Dim lstDocumentosItems As List(Of DocumentoMaestroTitulo)
        lstDocumentosItems = New List(Of DocumentoMaestroTitulo)
        If lsvListaDocumentos.Items.Count() > 0 And Not tituloEjecutivoObj.TituloEjecutivo.Automatico Then
            For Each item As ListViewItem In lsvListaDocumentos.Items
                Dim HdnIdDoc As HiddenField = CType(item.FindControl("HdnIdDoc"), HiddenField)
                Dim HdnPathFile As HiddenField = CType(item.FindControl("HdnPathFile"), HiddenField)
                Dim HdnIdMaestroTitulos As HiddenField = CType(item.FindControl("HdnIdMaestroTitulos"), HiddenField)

                Dim HdnCodTipoDodocumentoAO As HiddenField = CType(item.FindControl("HdnCodTipoDodocumentoAO"), HiddenField)
                Dim HdnNomDocAO As HiddenField = CType(item.FindControl("HdnNomDocAO"), HiddenField)
                Dim HdnObservaLegibilidad As HiddenField = CType(item.FindControl("HdnNumPaginas"), HiddenField)
                Dim HdnNumPaginas As HiddenField = CType(item.FindControl("HdnNumPaginas"), HiddenField)
                Dim HdnCodGuid As HiddenField = CType(item.FindControl("HdnCodGuid"), HiddenField)
                Dim HdIndDocSincronizado As HiddenField = CType(item.FindControl("HdIndDocSincronizado"), HiddenField)

                If String.IsNullOrEmpty(HdnPathFile.Value) Then
                    Continue For
                End If
                Dim ObjectItemMaestro As DocumentoMaestroTitulo
                ObjectItemMaestro = New DocumentoMaestroTitulo()
                ObjectItemMaestro.ID_DOCUMENTO_TITULO = Int32.Parse(HdnIdDoc.Value)
                ObjectItemMaestro.DES_RUTA_DOCUMENTO = HdnPathFile.Value
                ObjectItemMaestro.TIPO_RUTA = 1
                ObjectItemMaestro.COD_TIPO_DOCUMENTO_AO = HdnCodTipoDodocumentoAO.Value
                ObjectItemMaestro.NOM_DOC_AO = HdnNomDocAO.Value
                ObjectItemMaestro.OBSERVA_LEGIBILIDAD = HdnObservaLegibilidad.Value
                ObjectItemMaestro.NUM_PAGINAS = If(String.IsNullOrEmpty(HdnNumPaginas.Value), 0, Int32.Parse(HdnNumPaginas.Value))
                ObjectItemMaestro.COD_GUID = HdnCodGuid.Value
                ObjectItemMaestro.IND_DOC_SINCRONIZADO = If(String.IsNullOrEmpty(HdIndDocSincronizado.Value), 0, HdIndDocSincronizado.Value)
                If String.IsNullOrEmpty(HdnIdMaestroTitulos.Value) = False Then
                    ObjectItemMaestro.ID_MAESTRO_TITULOS_DOCUMENTOS = HdnIdMaestroTitulos.Value
                End If

#Region "CumpleDoc"
                Dim RbtnSiCumpleDoc As RadioButton = CType(item.FindControl("RbtnSiCumpleDoc"), RadioButton)
                Dim BtnObservaciones As Button = CType(item.FindControl("BtnObservaciones"), Button)
                Dim HdnObservacionDoc As HiddenField = CType(item.FindControl("HdnObservacionDoc"), HiddenField)

                ObjectItemMaestro.Observacion = New ObservacionesCNCDoc()
                ObjectItemMaestro.Observacion.ID_OBSERVACIONESDOC = -1
                ObjectItemMaestro.Observacion.CUMPLE_NOCUMPLE = RbtnSiCumpleDoc.Checked
                If ObjectItemMaestro.Observacion.CUMPLE_NOCUMPLE Then
                    ObjectItemMaestro.Observacion.OBSERVACIONES = ""
                Else
                    ObjectItemMaestro.Observacion.OBSERVACIONES = HdnObservacionDoc.Value
                End If
#End Region
                lstDocumentosItems.Add(ObjectItemMaestro)
            Next
        ElseIf grdDocAutomatico.Rows.Count > 0 Then
            'Se integra el guardado parcial con los documentos automáticos
            For Each row As GridViewRow In grdDocAutomatico.Rows
                'Se obtiene el código GUID del grid
                Dim id As String = row.Cells(2).Text
                Dim RbtnSiCumpleDocServicio As RadioButton = CType(row.FindControl("RbtnSiCumpleDocServicio"), RadioButton)
                Dim RbtnNoCumpleDocServicio As RadioButton = CType(row.FindControl("RbtnSiCumpleDocServicio"), RadioButton)

                Dim ObjectItemMaestro As New DocumentoMaestroTitulo()
                ObjectItemMaestro.ID_MAESTRO_TITULOS_DOCUMENTOS = row.Cells(0).Text
                ObjectItemMaestro.ID_MAESTRO_TITULO = tituloEjecutivoObj.TituloEjecutivo.IdunicoTitulo
                ObjectItemMaestro.TIPO_RUTA = 2
                ObjectItemMaestro.COD_GUID = id
                ObjectItemMaestro.COD_TIPO_DOCUMENTO_AO = row.Cells(2).Text
                If RbtnSiCumpleDocServicio.Checked Or RbtnNoCumpleDocServicio.Checked Then
                    ObjectItemMaestro.Observacion = New ObservacionesCNCDoc With {
                    .ID_OBSERVACIONESDOC = -1,
                    .CUMPLE_NOCUMPLE = RbtnSiCumpleDocServicio.Checked
                }
                End If
                lstDocumentosItems.Add(ObjectItemMaestro)
            Next
        End If
        tituloEjecutivoObj.LstDocumentos = lstDocumentosItems
#Region "Cumple General"
        tituloEjecutivoObj.ObservacionTitulo = New ObservacionesCNC()
        tituloEjecutivoObj.ObservacionTitulo.CUMPLE_NOCUMPLE = RbtnSiNoCumple.Items(0).Selected
        tituloEjecutivoObj.ObservacionTitulo.OBSERVACIONES = TxtobservaCumpleNoCumple.Text
        tituloEjecutivoObj.LstTipificaciones = New List(Of TipificacionCNC)()
        For i = 0 To ChkBltsTipifica.Items.Count - 1
            If ChkBltsTipifica.Items(i).Selected = True Then 'SI ESTA SELECCIONADO
                Dim tipificacion = New TipificacionCNC()
                tipificacion.ID_TIPIFICACION = ChkBltsTipifica.Items(i).Value
                tituloEjecutivoObj.LstTipificaciones.Add(tipificacion)
            End If
        Next
#End Region
        almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(tituloEjecutivoObj)
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("jsonobj=" + almacenamientoTemportalItem.JSON_OBJ))
        almacenamientoTemporalBLL.actualizarAlmacenamiento(almacenamientoTemportalItem)
        loadDocumentos()
    End Sub
    Protected Sub cmdGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar3.Click, BtnGuardar2.Click, BtnGuardar1.Click, BtnEnviarCNCGral.Click
        ControlSubExeptionLog(AddressOf cmdGuardar, StringsResourse.MsgErrorAlGuardar)
    End Sub
    Protected Sub cmdGuardar()
        LsvResul.Visible = False
        lblMallaTitulo.Visible = False
        guardarTitulo()
        If Request("Edit") IsNot Nothing And Len(Request("Edit")) > 0 Then
            lblMalla.Text = StringsResourse.MsgGuardadoRegistroParcialTitulo
            mpMalla.Show()
        Else
            lblMalla.Text = StringsResourse.MsgNuevoRegistroParcialTitulo
            mpMalla.Show()
        End If
    End Sub

#Region "EventosCargueActualizacionVista"
    Private Sub LoadTipo_Cartera()
        cboTipo_Cartera.DataSource = tipoCarteraBLL.consultarTiposCartera(Int32.Parse(cboPROCEDENCIA.SelectedValue))
        cboTipo_Cartera.DataTextField = "DEC_TIPO_CARTERA"
        cboTipo_Cartera.DataValueField = "ID_TIPO_CARTERA"
        cboTipo_Cartera.DataBind()
        LoadcboMT_tipo_titulo(Int32.Parse(cboPROCEDENCIA.SelectedValue))
    End Sub
    Private Sub loadDocumentos()
        Dim LstDocumentos As List(Of DocumentoTipoTitulo) = documentoTipoTituloBLL.consultarDocumentosPorTipo(cboMT_tipo_titulo.SelectedValue)

        If String.IsNullOrEmpty(HdnIdTask.Value) = False Then
            Dim tituloEjecutivoObj As TituloEjecutivoExt
            'Si TareaAsignadaObject.ID_UNICO_TITULO esta vacio realiza el cargue del objeto parcial
            Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(HdnIdTask.Value))
            tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)

            If tituloEjecutivoObj.LstDocumentos.Count() > 0 Then
                For Each element As DocumentoTipoTitulo In LstDocumentos
                    Dim DocumentoMaestro = tituloEjecutivoObj.LstDocumentos.FirstOrDefault(Function(x) (x.ID_DOCUMENTO_TITULO) = element.ID_DOCUMENTO_TITULO)
                    If DocumentoMaestro IsNot Nothing Then
                        element.DES_RUTA_DOCUMENTO = DocumentoMaestro.DES_RUTA_DOCUMENTO
                        element.ID_MAESTRO_DOCUMENTO = DocumentoMaestro.ID_MAESTRO_TITULOS_DOCUMENTOS
                        element.COD_GUID = DocumentoMaestro.COD_GUID
                        element.COD_TIPO_DOCUMENTO_AO = DocumentoMaestro.COD_TIPO_DOCUMENTO_AO
                        element.NOM_DOC_AO = DocumentoMaestro.NOM_DOC_AO
                        element.OBSERVA_LEGIBILIDAD = DocumentoMaestro.OBSERVA_LEGIBILIDAD
                        element.NUM_PAGINAS = DocumentoMaestro.NUM_PAGINAS
                        element.IND_DOC_SINCRONIZADO = DocumentoMaestro.IND_DOC_SINCRONIZADO
                        element.Observacion = DocumentoMaestro.Observacion
                    End If
                Next
            End If
        End If
        lsvListaDocumentos.DataSource = LstDocumentos
        lsvListaDocumentos.DataBind()
    End Sub
    Private Sub loadValores()
        Dim ValoresObligacion As Valores

        ValoresObligacion = obtenDatosValoresBLL.ConsultaDatValores().Where(Function(x) (x.ID_TIPO_OBLIGACION_VALORES) = cboMT_tipo_titulo.SelectedValue).FirstOrDefault()

        If ValoresObligacion IsNot Nothing Then
            txtSancionInexactitud.Visible = ValoresObligacion.SANCION_INEXACTITUD
            txtSancionMora.Visible = ValoresObligacion.SANCION_MORA
            txtSancionOmision.Visible = ValoresObligacion.SANCION_OMISION
            txtPartidaGlobal.Visible = ValoresObligacion.PARTIDA_GLOBAL
            txtValorObligacion.Visible = ValoresObligacion.VALOR_OBLIGACION
            lblNombreTitulo.Text = cboMT_tipo_titulo.SelectedItem.ToString()

            'Se quitan los valores de los campos relacionados con los valores solo si el campo no se muestra
            txtSancionInexactitud.Text = If(ValoresObligacion.SANCION_INEXACTITUD, txtSancionInexactitud.Text, String.Empty)
            txtSancionMora.Text = If(ValoresObligacion.SANCION_MORA, txtSancionMora.Text, String.Empty)
            txtSancionOmision.Text = If(ValoresObligacion.SANCION_OMISION, txtSancionOmision.Text, String.Empty)
            txtPartidaGlobal.Text = If(ValoresObligacion.PARTIDA_GLOBAL, txtPartidaGlobal.Text, String.Empty)
            txtValorObligacion.Text = If(ValoresObligacion.VALOR_OBLIGACION, txtValorObligacion.Text, String.Empty)
        End If
    End Sub
    Private Sub loadItemsPorTipoObligacion()
        loadDocumentos()
        loadValores()
    End Sub
    Private Sub ActivarControles()
        Dim Habilitado As Boolean
        If Session("mnivelacces") <> 11 Then
            If Session("mnivelacces") = 5 Or Session("mnivelacces") = 8 Then
                ' Es el repartidor. Solo activar los siguientes campos 
                txtMT_nro_titulo.Enabled = True
                txtMT_fec_expedicion_titulo.Enabled = True
                'txtTotalRepartidor.Enabled = True

                cboMT_tipo_titulo.Enabled = True
                '           
                imgBtnBorraFecExpTit.Visible = True

                ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 

                If Session("mnivelacces") = 5 Then
                    'tblTotalRepartidor.Visible = True

                    Habilitado = False
                Else
                    Habilitado = True
                End If

                txtMT_fec_notificacion_titulo.Enabled = Habilitado
                cboMT_for_notificacion_titulo.Enabled = Habilitado
                txtMT_res_resuelve_reposicion.Enabled = Habilitado
                txtMT_fec_expe_resolucion_reposicion.Enabled = Habilitado
                txtMT_fec_not_reso_resu_reposicion.Enabled = Habilitado
                cboMT_for_not_reso_resu_reposicion.Enabled = Habilitado
                txtMT_reso_resu_apela_recon.Enabled = Habilitado
                txtMT_fec_exp_reso_apela_recon.Enabled = Habilitado
                txtMT_fec_not_reso_apela_recon.Enabled = Habilitado
                cboMT_for_not_reso_apela_recon.Enabled = Habilitado



                If (Session("mnivelacces") = 4) Then
                    txtMT_fecha_ejecutoriaObli.Enabled = True
                    txtMT_fec_exi_liqObli.Enabled = True
                    txtMT_fec_exi_liqObli.Enabled = False
                Else
                    txtMT_fecha_ejecutoriaObli.Enabled = True
                    txtMT_fec_exi_liqObli.Enabled = True
                End If


                txtMT_fec_cad_presc.Enabled = False

                imgBtnBorraFecNotTit.Visible = Habilitado
                imgBtnBorraFecExpRR.Visible = Habilitado
                imgBtnBorraFecNotRRR.Visible = Habilitado
                imgBtnBorraFecExpRAR.Visible = Habilitado
                imgBtnBorraFecNotRAR.Visible = Habilitado



            Else

                txtMT_nro_titulo.Enabled = False
                txtMT_fec_expedicion_titulo.Enabled = False


                cboMT_tipo_titulo.Enabled = False


                imgBtnBorraFecExpTit.Visible = False

                txtMT_fec_notificacion_titulo.Enabled = True
                cboMT_for_notificacion_titulo.Enabled = True
                txtMT_res_resuelve_reposicion.Enabled = True
                txtMT_fec_expe_resolucion_reposicion.Enabled = True
                txtMT_fec_not_reso_resu_reposicion.Enabled = True
                cboMT_for_not_reso_resu_reposicion.Enabled = True
                txtMT_reso_resu_apela_recon.Enabled = True
                txtMT_fec_exp_reso_apela_recon.Enabled = True
                txtMT_fec_not_reso_apela_recon.Enabled = True
                cboMT_for_not_reso_apela_recon.Enabled = True
                txtMT_fecha_ejecutoriaObli.Enabled = True

                txtMT_fec_cad_presc.Enabled = False
                '
                imgBtnBorraFecNotTit.Visible = True
                imgBtnBorraFecExpRR.Visible = True
                imgBtnBorraFecNotRRR.Visible = True
                imgBtnBorraFecExpRAR.Visible = True
                imgBtnBorraFecNotRAR.Visible = True

            End If
        Else
            ' SE DESABILITA PARA AREA ORIGEN
            txtMT_fec_cad_presc.Enabled = False
        End If

    End Sub
    Private Sub LoadcboPROCEDENCIA()
        cboPROCEDENCIA.Enabled = False
        cboPROCEDENCIA.DataSource = tipoAreaOrigenBLL.ConsultarAreaOrigen()
        cboPROCEDENCIA.DataTextField = "nombre"
        cboPROCEDENCIA.DataValueField = "codigo"
        cboPROCEDENCIA.DataBind()
        Dim prmAreaOrigen As Int32
        If IsNothing(Session("usrAreaOrgen")) Or String.IsNullOrEmpty(Session("usrAreaOrgen")) Then
            Session("usrAreaOrgen") = ConfigurationManager.AppSettings("AREA_DEFAULT")
        End If
        prmAreaOrigen = Int32.Parse(Session("usrAreaOrgen"))
        cboPROCEDENCIA.SelectedValue = prmAreaOrigen
    End Sub
    Protected Sub LoadcboMT_tiposentencia()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo,nombre FROM TIPO_SENTENCIA"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTabla As New DataTable
        Adaptador.Fill(dtTabla)
        If dtTabla.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTabla As DataRow = dtTabla.NewRow()
            filaTabla("codigo") = "0"
            filaTabla("nombre") = ""
            dtTabla.Rows.Add(filaTabla)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTabla As DataView = New DataView(dtTabla)
            vistaTabla.Sort = "codigo"
            '--------------------------------------------------------------------
            cboMT_tiposentencia.DataSource = vistaTabla
            cboMT_tiposentencia.DataTextField = "nombre"
            cboMT_tiposentencia.DataValueField = "codigo"
            cboMT_tiposentencia.DataBind()
        End If
    End Sub
    Protected Sub LoadcboMT_tipo_titulo(prmTipoArea As Int32)
        cboMT_tipo_titulo.DataSource = tipoTituloBLL.consultarTipoTitulo(prmTipoArea)
        cboMT_tipo_titulo.DataTextField = "nombre"
        cboMT_tipo_titulo.DataValueField = "codigo"
        cboMT_tipo_titulo.DataBind()
    End Sub
    Protected Sub LoadcboMT_for_notificacion_titulo()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String
        cmd = "select codigo, nombre from [FORMAS_NOTIFICACION] order by nombre"
        If Session("mnivelacces") = 11 Or Session("mnivelacces") = 10 Then
            cmd = "select codigo, nombre from [FORMAS_NOTIFICACION] where nombre<>'NO SE INTERPUSO' order by nombre"
        End If
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTabla As New DataTable
        Adaptador.Fill(dtTabla)
        If dtTabla.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTabla As DataRow = dtTabla.NewRow()
            filaTabla("codigo") = "-1"
            filaTabla("nombre") = "Seleccione.."
            dtTabla.Rows.Add(filaTabla)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTabla As DataView = New DataView(dtTabla)
            vistaTabla.Sort = "codigo"
            Session("FormasNotifiacionLts") = vistaTabla
            '--------------------------------------------------------------------
            cboMT_for_notificacion_titulo.DataSource = vistaTabla
            cboMT_for_notificacion_titulo.DataTextField = "nombre"
            cboMT_for_notificacion_titulo.DataValueField = "codigo"
            cboMT_for_notificacion_titulo.DataBind()
        End If
    End Sub
    Protected Sub LoadcboMT_for_not_reso_resu_reposicion()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String
        cmd = "select codigo, nombre from [FORMAS_NOTIFICACION] order by nombre"
        If Session("mnivelacces") = 11 Or Session("mnivelacces") = 10 Then
            cmd = "select codigo, nombre from [FORMAS_NOTIFICACION] where nombre<>'NO SE INTERPUSO' order by nombre"
        End If
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTabla As New DataTable
        Adaptador.Fill(dtTabla)
        If dtTabla.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTabla As DataRow = dtTabla.NewRow()
            filaTabla("codigo") = ""
            filaTabla("nombre") = ""
            dtTabla.Rows.Add(filaTabla)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTabla As DataView = New DataView(dtTabla)
            vistaTabla.Sort = "codigo"
            '--------------------------------------------------------------------
            cboMT_for_not_reso_resu_reposicion.DataSource = vistaTabla
            cboMT_for_not_reso_resu_reposicion.DataTextField = "nombre"
            cboMT_for_not_reso_resu_reposicion.DataValueField = "codigo"
            cboMT_for_not_reso_resu_reposicion.DataBind()
        End If
    End Sub
    Protected Sub LoadcboMT_for_not_reso_apela_recon()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String
        cmd = "select codigo, nombre from [FORMAS_NOTIFICACION] order by nombre"
        If Session("mnivelacces") = 11 Or Session("mnivelacces") = 10 Then
            cmd = "select codigo, nombre from [FORMAS_NOTIFICACION] where nombre<>'NO SE INTERPUSO' order by nombre"
        End If
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTabla As New DataTable
        Adaptador.Fill(dtTabla)
        If dtTabla.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTabla As DataRow = dtTabla.NewRow()
            filaTabla("codigo") = ""
            filaTabla("nombre") = ""
            dtTabla.Rows.Add(filaTabla)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTabla As DataView = New DataView(dtTabla)
            vistaTabla.Sort = "codigo"
            '--------------------------------------------------------------------
            cboMT_for_not_reso_apela_recon.DataSource = vistaTabla
            cboMT_for_not_reso_apela_recon.DataTextField = "nombre"
            cboMT_for_not_reso_apela_recon.DataValueField = "codigo"
            cboMT_for_not_reso_apela_recon.DataBind()
        End If
    End Sub
    Protected Sub imgBtnBorraFecNotTit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecNotTit.Click
        txtMT_fec_notificacion_titulo.Text = ""
    End Sub
    Protected Sub imgBtnBorraFecExpRR_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExpRR.Click
        txtMT_fec_expe_resolucion_reposicion.Text = ""
    End Sub
    Protected Sub imgBtnBorraFecNotRRR_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecNotRRR.Click
        txtMT_fec_not_reso_resu_reposicion.Text = ""
    End Sub
    Protected Sub imgBtnBorraFecExpRAR_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExpRAR.Click
        txtMT_fec_exp_reso_apela_recon.Text = ""
    End Sub
    Protected Sub imgBtnBorraFecNotRAR_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecNotRAR.Click
        txtMT_fec_not_reso_apela_recon.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecEjecObli_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecEjecObli.Click
        txtMT_fecha_ejecutoriaObli.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecExiLOBli_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExiLObli.Click
        txtMT_fec_exi_liqObli.Text = ""
    End Sub
    Protected Sub imgBtnBorraFecExpTit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExpTit.Click
        txtMT_fec_expedicion_titulo.Text = ""
    End Sub
    Private Sub cboMT_tipo_titulo_TextChanged(sender As Object, e As EventArgs) Handles cboMT_tipo_titulo.TextChanged
        loadItemsPorTipoObligacion()
    End Sub

    Protected Sub lsvListaDocuentos_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lsvListaDocumentos.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim lblNombreTituloItem As Label = CType(e.Item.FindControl("lblNameDoc"), Label)
            Dim HdnPathFile As HiddenField = CType(e.Item.FindControl("HdnPathFile"), HiddenField)
            Dim HdnIdMaestroTitulos As HiddenField = CType(e.Item.FindControl("HdnIdMaestroTitulos"), HiddenField)
            Dim LblArchivo As Label = CType(e.Item.FindControl("LblArchivo"), Label)
            Dim LblCumple As Label = CType(e.Item.FindControl("LblCumple"), Label)
            'CumpleNoCumple
            Dim RbtnNoCumpleDoc As RadioButton = CType(e.Item.FindControl("RbtnNoCumpleDoc"), RadioButton)
            Dim RbtnSiCumpleDoc As RadioButton = CType(e.Item.FindControl("RbtnSiCumpleDoc"), RadioButton)
            Dim BtnObservaciones As Button = CType(e.Item.FindControl("BtnObservaciones"), Button)
            Dim btnCargar As Button = CType(e.Item.FindControl("btnCargar"), Button)
            Dim btnBorrar As Button = CType(e.Item.FindControl("btnBorrar"), Button)
            Dim HdnObservacionDoc As HiddenField = CType(e.Item.FindControl("HdnObservacionDoc"), HiddenField)

            Dim HdnCodTipoDodocumentoAO As HiddenField = CType(e.Item.FindControl("HdnCodTipoDodocumentoAO"), HiddenField)
            Dim HdnNomDocAO As HiddenField = CType(e.Item.FindControl("HdnNomDocAO"), HiddenField)
            Dim HdnObservaLegibilidad As HiddenField = CType(e.Item.FindControl("HdnNumPaginas"), HiddenField)
            Dim HdnNumPaginas As HiddenField = CType(e.Item.FindControl("HdnNumPaginas"), HiddenField)
            Dim HdnCodGuid As HiddenField = CType(e.Item.FindControl("HdnCodGuid"), HiddenField)
            Dim HdIndDocSincronizado As HiddenField = CType(e.Item.FindControl("HdIndDocSincronizado"), HiddenField)

            HdnCodTipoDodocumentoAO.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).COD_TIPO_DOCUMENTO_AO
            HdnNomDocAO.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).NOM_DOC_AO
            HdnObservaLegibilidad.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).OBSERVA_LEGIBILIDAD
            HdnNumPaginas.Value = If(String.IsNullOrEmpty(CType(e.Item.DataItem, DocumentoTipoTitulo).NUM_PAGINAS), 0, Int32.Parse(CType(e.Item.DataItem, DocumentoTipoTitulo).NUM_PAGINAS))
            HdnCodGuid.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).COD_GUID
            HdIndDocSincronizado.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).IND_DOC_SINCRONIZADO

            RbtnNoCumpleDoc.Enabled = False
            RbtnSiCumpleDoc.Enabled = False
            BtnObservaciones.Style.Value = "display: none;"
            Dim ObservacionDoc = CType(e.Item.DataItem, DocumentoTipoTitulo).Observacion
            LblCumple.Visible = False
            RbtnSiCumpleDoc.Visible = False
            RbtnNoCumpleDoc.Visible = False
            HdnObservacionDoc.Value = ObservacionDoc.OBSERVACIONES
            If (Session("mnivelacces") = 10) And HdnAutomaticoCargue.Value = False Then 'Estudio de titulos, solo estudio de titulos puede editar el cumple
                RbtnNoCumpleDoc.Enabled = True
                RbtnSiCumpleDoc.Enabled = True
            End If

            If (Session("mnivelacces") = 10) And HdnAutomaticoCargue.Value = False Then
                LblCumple.Visible = True
                RbtnSiCumpleDoc.Visible = True
                RbtnNoCumpleDoc.Visible = True
                If ObservacionDoc.CUMPLE_NOCUMPLE = False Then
                    RbtnSiCumpleDoc.Checked = False
                    RbtnNoCumpleDoc.Checked = True
                    BtnObservaciones.Style.Value = ""
                End If
                If ObservacionDoc.CUMPLE_NOCUMPLE Then
                    RbtnSiCumpleDoc.Checked = True
                    RbtnNoCumpleDoc.Checked = False
                    BtnObservaciones.Style.Value = "display: none;"
                End If
            End If
            If (Session("mnivelacces") = 11) And HdnIdEstadoOperativo.Value = 8 Then 'Area Origen - Subsanar
                LblCumple.Visible = True
                RbtnSiCumpleDoc.Visible = True
                RbtnNoCumpleDoc.Visible = True
                'Si el documento cumple no se permite borrar ni editar
                If RbtnSiCumpleDoc.Checked Then
                    btnCargar.Enabled = False
                    btnBorrar.CssClass = "force-hide"
                End If
                Dim MostrarCumple = (ObservacionDoc.ID_OBSERVACIONESDOC <> 0)
                If MostrarCumple Then
                    LblCumple.Visible = MostrarCumple
                    If HdnIdEstadoOperativo.Value = 8 Then
                        btnCargar.Enabled = (ObservacionDoc.CUMPLE_NOCUMPLE = False)
                    End If
                    RbtnSiCumpleDoc.Visible = MostrarCumple
                    RbtnNoCumpleDoc.Visible = MostrarCumple
                End If
                If ObservacionDoc.CUMPLE_NOCUMPLE = False Then
                    BtnObservaciones.Style.Value = ""
                End If
                If ObservacionDoc.CUMPLE_NOCUMPLE = False Then
                    RbtnSiCumpleDoc.Checked = False
                    RbtnNoCumpleDoc.Checked = True
                    BtnObservaciones.Style.Value = ""
                End If
                If ObservacionDoc.CUMPLE_NOCUMPLE Then
                    RbtnSiCumpleDoc.Checked = True
                    RbtnNoCumpleDoc.Checked = False
                End If
            End If
            If String.IsNullOrEmpty(HdnExpediente.Value) = False Or Boolean.Parse(HdnAutomaticoCargue.Value) Then
                RbtnSiCumpleDoc.Visible = False
                RbtnNoCumpleDoc.Visible = False
                LblCumple.Visible = False
                btnCargar.Visible = False
            End If
            Dim HdnIdDoc As HiddenField = CType(e.Item.FindControl("HdnIdDoc"), HiddenField)
            HdnIdDoc.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).ID_DOCUMENTO_TITULO
            If String.IsNullOrEmpty(CType(e.Item.DataItem, DocumentoTipoTitulo).DES_RUTA_DOCUMENTO) = False Then
                HdnPathFile.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).DES_RUTA_DOCUMENTO
                LblArchivo.Text = HdnPathFile.Value.Split("\").Last()
            End If
            If CType(e.Item.DataItem, DocumentoTipoTitulo).ID_MAESTRO_DOCUMENTO <> 0 Then
                HdnIdMaestroTitulos.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).ID_MAESTRO_DOCUMENTO
            End If
            Dim controlDoc As DatosDocumento = CType(e.Item.FindControl("editDoc"), DatosDocumento)

            If Boolean.Parse(HdnAutomatico.Value) And Session("mnivelacces") = 10 And Boolean.Parse(HdnAutomaticoCargue.Value) Then
                btnCargar.Visible = True
                BtnObservaciones.Visible = False
            End If
            lblNombreTituloItem.Text = CType(e.Item.DataItem, DocumentoTipoTitulo).NOMBRE_DOCUMENTO

            If CType(e.Item.DataItem, DocumentoTipoTitulo).VAL_OBLIGATORIO Then
                lblNombreTituloItem.Text = CType(e.Item.DataItem, DocumentoTipoTitulo).NOMBRE_DOCUMENTO & " *"
            End If

            If Session("mnivelacces") <> 11 And Boolean.Parse(HdnAutomaticoCargue.Value) = False Then 'Solo área origen puede borrar los documentos cargados
                btnBorrar.CssClass = "force-hide"
            End If

            If Boolean.Parse(HdnAutomatico.Value) Then
                BtnObservaciones.Visible = False
            End If
        End If
    End Sub
    Protected Sub BtnAdiccionarNot_Click(sender As Object, e As EventArgs) Handles BtnAdiccionarNot.Click
        Dim lstNotificacionesItems As List(Of NotificacionTitulo)
        lstNotificacionesItems = New List(Of NotificacionTitulo)
        If LstNotificaciones.Items.Count() > 0 Then
            For Each item As ListViewItem In LstNotificaciones.Items
                Dim ObjectItems As NotificacionTitulo
                ObjectItems = New NotificacionTitulo()
                Dim HdnItemObj As HiddenField = CType(item.FindControl("HdnItemObj"), HiddenField)
                Dim Format = "dd/MM/yyyy"
                Dim dateTimeConverter = New IsoDateTimeConverter()
                dateTimeConverter.DateTimeFormat = Format
                lstNotificacionesItems.Add(JsonConvert.DeserializeObject(Of NotificacionTitulo)(HdnItemObj.Value, dateTimeConverter))
            Next
        End If
        Dim notificacionNueva = New NotificacionTitulo()
        notificacionNueva.COD_TIPO_NOTIFICACION = 1
        lstNotificacionesItems.Add(notificacionNueva)
        LstNotificaciones.DataSource = lstNotificacionesItems
        LstNotificaciones.DataBind()
    End Sub
    Protected Sub LstNotificaciones_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles LstNotificaciones.ItemDataBound
        If e.Item.DataItem IsNot Nothing And e.Item.ItemType = ListViewItemType.DataItem Then
            Dim cboTipoNotifiacion As DropDownList = CType(e.Item.FindControl("cboTipoNotifiacion"), DropDownList)
            Dim cboMT_for_not As DropDownList = CType(e.Item.FindControl("cboMT_for_not"), DropDownList)
            Dim txtMT_fechaItem As TextBox = CType(e.Item.FindControl("txtMT_fechaItem"), TextBox)
            Dim HdnItemObj As HiddenField = CType(e.Item.FindControl("HdnItemObj"), HiddenField)


            If cboTipoNotifiacion.Items.Count = 0 Then
                cboTipoNotifiacion.DataSource = dominioDetalleBLL.consultarDominioPorIdDominio(1)
                cboTipoNotifiacion.DataTextField = "DESC_DESCRIPCION"
                cboTipoNotifiacion.DataValueField = "ID_DOMINIO_DETALLE"
                cboTipoNotifiacion.DataBind()

                cboMT_for_not.DataSource = Session("FormasNotifiacionLts")
                cboMT_for_not.DataTextField = "nombre"
                cboMT_for_not.DataValueField = "codigo"
                cboMT_for_not.DataBind()
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "ClientScript" & txtMT_fechaItem.ClientID, "<script type="" text/javascript"">ElementoFecha('" & txtMT_fechaItem.ClientID & "');    </script>")

            End If

            If CType(e.Item.DataItem, NotificacionTitulo).COD_TIPO_NOTIFICACION <> 0 Then
                cboTipoNotifiacion.SelectedValue = CType(e.Item.DataItem, NotificacionTitulo).COD_TIPO_NOTIFICACION.ToString()
            End If
            If String.IsNullOrEmpty(CType(e.Item.DataItem, NotificacionTitulo).COD_FOR_NOT) = False Then
                cboMT_for_not.SelectedValue = CType(e.Item.DataItem, NotificacionTitulo).COD_FOR_NOT
            End If

            If CType(e.Item.DataItem, NotificacionTitulo).FEC_NOTIFICACION.HasValue Then
                txtMT_fechaItem.Text = CType(e.Item.DataItem, NotificacionTitulo).FEC_NOTIFICACION.Value.ToString("dd/MM/yyyy")
            End If
            HdnItemObj.Value = JsonConvert.SerializeObject(CType(e.Item.DataItem, NotificacionTitulo))
        End If
    End Sub
    'Envento solo para area origen
    Protected Sub BtnEnviarTitulo_Click(sender As Object, e As EventArgs) Handles BtnEnviarTitulo.Click
        guardarTitulo()
        LsvResul.Visible = False
        If String.IsNullOrEmpty(HdnIdTask.Value.ToString()) = False Then
            'Si ID_TASK tiene valor se carga valida el item
            tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(HdnIdTask.Value.ToString()))
            Dim tituloEjecutivoObj As TituloEjecutivoExt
            Dim validadorMalla As New ValidadorBLL()
            Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(tareaAsignadaObject.ID_TAREA_ASIGNADA)
            'Si TareaAsignadaObject.ID_UNICO_TITULO esta vacio realiza el cargue del objeto parcial
            tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)

            tituloEjecutivoObj.TituloEjecutivo.Automatico = False
            If tareaAsignadaObject.ID_UNICO_TITULO Is Nothing Then
                'Creacion del titulo 
                If tareaAsignadaObject.COD_ESTADO_OPERATIVO = 1 Then
                    validadorMalla.MallaValidadoraTituloEjecutivoGlobal(tituloEjecutivoObj.TituloEjecutivo, tituloEjecutivoObj.LstDocumentos, tituloEjecutivoObj.LstDeudores, tituloEjecutivoObj.LstDireccionUbicacion, tituloEjecutivoObj.LstNotificacion, Int64.Parse(HdnIdTask.Value))
                    If validadorMalla.Idunicotitulo = -1 Then
                        lblMalla.Text = StringsResourse.MsgErrorNoControladoMalla
                    End If
                    If validadorMalla.Idunicotitulo = 0 Then
                        lblMalla.Text = StringsResourse.MsgNoValidacionTitulo
                        If validadorMalla.respuesta.Count > 0 Then
                            lblMalla.Text = My.Resources.errorGeneral
                            LsvResul.DataSource = validadorMalla.respuesta
                            LsvResul.DataBind()

                            LsvResul.Visible = True
                            lblMallaTitulo.Visible = True
                        End If
                    End If
                    If validadorMalla.Idunicotitulo <> 0 And validadorMalla.Idunicotitulo <> -1 Then
                        lblMalla.Text = StringsResourse.MsgIngresoTituloExitoso + validadorMalla.Idunicotitulo.ToString()
                        LimpiarAlmacenamiento(almacenamientoTemportalItem)
                        Dim TareaActualizar = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(HdnIdTask.Value.ToString()))
                        TareaActualizar.FEC_ACTUALIZACION = Date.Now()
                        TareaActualizar.VAL_USUARIO_NOMBRE = String.Empty
                        TareaActualizar.COD_ESTADO_OPERATIVO = 2
                        TareaActualizar.ID_UNICO_TITULO = validadorMalla.Idunicotitulo
                        tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("fecactualizacion=" + TareaActualizar.FEC_ACTUALIZACION.ToString() + ",usunombre=" & TareaActualizar.VAL_USUARIO_NOMBRE + ",codestadoope=" & TareaActualizar.COD_ESTADO_OPERATIVO.ToString() + ",idtitulo=" & TareaActualizar.ID_UNICO_TITULO.ToString()))
                        tareaAsignadaBLL.actualizarTareaAsignadaDevolucion(TareaActualizar)
                    End If
                    mpMalla.Show()
                End If
            End If
            'Subsanar Titulo
            If tareaAsignadaObject.COD_ESTADO_OPERATIVO = 8 Then 'Subsanar
                If ValidarCumpleNoCumple(tituloEjecutivoObj) = False Then
                    Return
                End If
                'Se agrega validador para que no borre el número del expediente
                Dim expedienteId As String = Nothing
                'Se valida si el título tiene expediente
                If Not IsNothing(tituloEjecutivoObj.TituloEjecutivo.IdunicoTitulo) Then
                    Dim _maestroTitulosBLL As New MaestroTitulosBLL
                    Dim titulo As Entidades.MaestroTitulos = _maestroTitulosBLL.consultarTituloPorID(tituloEjecutivoObj.TituloEjecutivo.IdunicoTitulo)
                    expedienteId = titulo.MT_expediente
                End If
                validadorMalla.MallaValidadoraTituloEjecutivoGlobal(tituloEjecutivoObj.TituloEjecutivo, tituloEjecutivoObj.LstDocumentos, tituloEjecutivoObj.LstDeudores, tituloEjecutivoObj.LstDireccionUbicacion, tituloEjecutivoObj.LstNotificacion, Int64.Parse(HdnIdTask.Value), True, expedienteId)
                If validadorMalla.Idunicotitulo = -1 Then
                    lblMalla.Text = StringsResourse.MsgErrorNoControladoMalla
                End If
                If validadorMalla.Idunicotitulo = 0 Then
                    lblMalla.Text = StringsResourse.MsgNoValidacionTitulo
                    If validadorMalla.respuesta.Count > 0 Then
                        LsvResul.Visible = True
                        LsvResul.DataSource = validadorMalla.respuesta
                        LsvResul.DataBind()
                    End If
                End If
                If validadorMalla.Idunicotitulo = 1 And validadorMalla.Idunicotitulo <> 0 Then
                    lblMalla.Text = StringsResourse.MsgActualizacionTitulo
                    LimpiarAlmacenamiento(almacenamientoTemportalItem)
                    Dim EstadosOpertivosBuscar = New List(Of Integer)() From {5, 7, 9}
                    Dim Destinatario As String = tareaAsignadaBLL.ObtenerUsuarioDevolucion(tareaAsignadaObject.ID_TAREA_ASIGNADA, Session("ssloginusuario"), EstadosOpertivosBuscar)
                    IngresarObservaciones(tituloEjecutivoObj, Destinatario)
                    Dim TareaActualizar = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(HdnIdTask.Value.ToString()))
                    TareaActualizar.FEC_ACTUALIZACION = Date.Now()
                    TareaActualizar.VAL_USUARIO_NOMBRE = Destinatario
                    TareaActualizar.COD_ESTADO_OPERATIVO = 9
                    tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("fecactualizacion=" + TareaActualizar.FEC_ACTUALIZACION.ToString() + ",usunombre=" & TareaActualizar.VAL_USUARIO_NOMBRE + ",codestadope=" & TareaActualizar.COD_ESTADO_OPERATIVO.ToString()))
                    tareaAsignadaBLL.actualizarTareaAsignadaDevolucion(TareaActualizar)
                    'lblMalla.Text = StringsResourse.MsgExitosoObservacionTitulo

                End If
                mpMalla.Show()
            End If
        End If
    End Sub
    ' Esta accion se realiza cuando se transita de area para su posterior guardado parcial
    Public Sub LimpiarAlmacenamiento(almacenamientoTemportalItem As AlmacenamientoTemporal)
        almacenamientoTemportalItem.JSON_OBJ = String.Empty
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("jsonobjt=" + almacenamientoTemportalItem.JSON_OBJ))
        almacenamientoTemporalBLL.actualizarAlmacenamiento(almacenamientoTemportalItem)
    End Sub
    ''' <summary>
    ''' Metodo que guarda la justificación del cierre administrativo desde el boton Terminar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GuardarJustCierre(sender As Object, e As EventArgs) Handles BtnGuardarValidaJust.Click
        If TxtJustificacion.Text = "" Then
            lblValidaJust.Visible = True
        Else
            lblValidaJust.Visible = False
            Dim MetodoSel As InsertaJustificacionBLL = New InsertaJustificacionBLL()
            tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Request("ID_TASK").ToString())
            MetodoSel.InsertaJustificacionCierre(tareaAsignadaObject.ID_UNICO_TITULO, TxtJustificacion.Text)
            Response.Redirect("Security/Area_Origen/BandejaAreaOrigen.aspx", False)
        End If
    End Sub
    ''' <summary>
    ''' Metodo que guarda las observaciones del Cumple no Cumple del modal de observaciones de Documentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GuardarObservacionesCNCDoc(sender As Object, e As EventArgs) Handles btnEnviarCNC.Click
        If TxtComentarioCNC.Text = "" Then
            LblValidaComentarioCNC.Visible = True
            Return
        Else
            LblValidaComentarioCNC.Visible = False
        End If
        If lsvListaDocumentos.Items.Count() > 0 Then
            For Each item As ListViewItem In lsvListaDocumentos.Items
                Dim RbtnSiCumpleDoc As RadioButton = CType(item.FindControl("RbtnSiCumpleDoc"), RadioButton)
                Dim RbtnNoCumpleDoc As RadioButton = CType(item.FindControl("RbtnNoCumpleDoc"), RadioButton)
                Dim BtnObservaciones As Button = CType(item.FindControl("BtnObservaciones"), Button)
                Dim HdnObservacionDoc As HiddenField = CType(item.FindControl("HdnObservacionDoc"), HiddenField)
                Dim HdnIdMaestroTitulos As HiddenField = CType(item.FindControl("HdnIdMaestroTitulos"), HiddenField)

                If HdnIdMaestroTitulos.Value = HdnIdDocumento.Value Then
                    RbtnSiCumpleDoc.Checked = False
                    RbtnNoCumpleDoc.Checked = True
                    HdnObservacionDoc.Value = TxtComentarioCNC.Text
                End If
                If RbtnSiCumpleDoc.Checked Then
                    BtnObservaciones.Style.Value = "disabled: false;"
                End If
            Next
            guardarTitulo()
            'tblGralCumpleNoCumple.Style.Value = "display: none;"
        End If

    End Sub
    ''' <summary>
    ''' Metodo de cuando cambia el CheboxList de Tipificación
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ChangeTipificacion(sender As Object, e As EventArgs) Handles ChkBltsTipifica.SelectedIndexChanged
        Dim contador As Integer = 0
        For i = 0 To ChkBltsTipifica.Items.Count - 1
            If ChkBltsTipifica.Items(i).Selected Then 'SI ESTA SELECCIONADO
                contador = contador + 1
            End If
        Next
        If contador > 0 Then
            If RbtnSiNoCumple.SelectedIndex <> 1 Then
                RbtnSiNoCumple.SelectedIndex = 1
            End If
        End If
    End Sub

    Protected Sub LsvResul_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles LsvResul.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim lblCod As Label = CType(e.Item.FindControl("lblCod"), Label)
            Dim LblMensaje As Label = CType(e.Item.FindControl("LblMensaje"), Label)
            lblCod.Text = CType(e.Item.DataItem, RespuestaMallaValidacion).codigo
            LblMensaje.Text = CType(e.Item.DataItem, RespuestaMallaValidacion).respuesta
        End If
    End Sub

    ''' <summary>
    ''' Llena la gr
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>

    Protected Sub PintarGridObservaDocs(sender As Object, e As EventArgs) Handles BtnObservacionesModal2.Click
        TxtComentarioCNC.Text = ""
        grdHistoricoCNCDoc.DataSource = Nothing
        grdHistoricoCNCDoc.DataBind()
        Dim ObservaCNCDoc As ObservacionesCNCDocBLL = New ObservacionesCNCDocBLL()
        Dim ObservaCNCDocList As List(Of ObservacionesCNCDoc) = ObservaCNCDoc.obtenerObservacionesCNCDoc(HdnIdunico.Value, HdnIdDocumento.Value)
        grdHistoricoCNCDoc.DataSource = ObservaCNCDocList
        grdHistoricoCNCDoc.DataBind()
        Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(HdnIdTask.Value.ToString()))
        Dim tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
        If tituloEjecutivoObj.LstDocumentos.FirstOrDefault(Function(x) (x.ID_MAESTRO_TITULOS_DOCUMENTOS = Int32.Parse(HdnIdDocumento.Value))) IsNot Nothing Then
            TxtComentarioCNC.Text = tituloEjecutivoObj.LstDocumentos.FirstOrDefault(Function(x) (x.ID_MAESTRO_TITULOS_DOCUMENTOS = Int32.Parse(HdnIdDocumento.Value))).Observacion.OBSERVACIONES
        End If
        UpnlDocCNC.Update()
    End Sub
    Protected Sub BtnAccionCancelarTit(sender As Object, e As EventArgs) Handles BtnCancelarAccion.Click
        If lblMalla.Text.Contains(StringsResourse.MsgIngresoTituloExitoso) And Len(Request("Edit")) <> 0 Or lblMalla.Text.Contains(StringsResourse.MsgActualizacionTitulo) And Request("ID_TASK") IsNot Nothing And Len(Request("ID_TASK")) > 0 Then
            redirectBandejaCorrespondiente()
        End If
        LsvResul.Items.Clear()
    End Sub

    Protected Sub btnRedirect_Click(sender As Object, e As EventArgs) Handles btnRedirect.Click

    End Sub

    Protected Sub btnRedirect2_Click(sender As Object, e As EventArgs) Handles btnRedirect2.Click
        Response.Redirect("~/Security/Maestros/estudio-titulos/BandejaTitulos.aspx", True)
    End Sub

    Protected Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If Len(Request("Edit")) > 0 Then
            EliminarTareaYAlmacenamiento()
            redirectBandejaCorrespondiente()
        Else
            EliminarTareaYAlmacenamiento()
            resetFormulario()
        End If
    End Sub
    Public Sub BuscarDocumentoDocumentic()
        Try
            Dim service As New Servicios.SvcDocumento
            Dim result As UGPPSvcDocumento.resultadoIdentificadorDocumentos
            Dim valConsulta As UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta = New UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta()
            valConsulta.idCarpeta = "{" & txtNoExp.Text & "}"
            Lblresultados.Text = "Buscando.."
            result = service.OpBuscarPorCriteriosExpediente(valConsulta)
            BtnBuscar.Enabled = False
            If result.documento.ToList().Count() = 0 Then
                Lblresultados.Text = "Sin resultados"
            Else
                Lblresultados.Text = result.documento.ToList().Count() & " Resultados"
            End If
            grdBuscar.DataSource = result.documento.ToList()
            grdBuscar.DataBind()
            UpdBuscar.Update()
            ModalPopupBuscarDocumento.Show()
            BtnBuscar.Enabled = True
        Catch ex As Exception
            Lblresultados.Text = "Servicio no disponible"
            ModalPopupBuscarDocumento.Show()
            BtnBuscar.Enabled = True
        End Try

    End Sub
    Protected Sub paginador_EventActualizarGrid()
        BuscarDocumentoDocumentic()
    End Sub

    Protected Sub grdBuscar_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnbutton As Button = DirectCast(e.Row.Cells(6).Controls(0), Button)
            Dim id As String = e.Row.Cells(0).Text
            Dim RutaDocumentic As String = ConfigurationManager.AppSettings("Constante1") & id & ConfigurationManager.AppSettings("Constante2")
            btnbutton.Attributes.Add("onclick", "javascript:AbrirNuevaPagina('" & RutaDocumentic & "');")
        End If

    End Sub

    Protected Sub grdDocAutomatico_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDocAutomatico.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnbutton As Button = DirectCast(e.Row.Cells(4).Controls(0), Button)
            Dim id As String = e.Row.Cells(2).Text
            id = id.Replace("{", "")
            id = id.Replace("}", "")
            Dim RutaDocumentic As String = ConfigurationManager.AppSettings("Constante1") & id & ConfigurationManager.AppSettings("Constante2")
            btnbutton.Attributes.Add("onclick", "javascript:AbrirNuevaPagina('" & RutaDocumentic & "');")
            'Se capturan los botones radio
            Dim radioSiCumple As RadioButton = CType(e.Row.FindControl("RbtnSiCumpleDocServicio"), RadioButton)
            Dim radioNoCumple As RadioButton = CType(e.Row.FindControl("RbtnNoCumpleDocServicio"), RadioButton)
            'Se asigna su group name para que funcionen por fila
            radioSiCumple.GroupName = id
            radioNoCumple.GroupName = id

            Dim almacenamientoTemtalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(HdnIdTask.Value.ToString()))
            Dim tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemtalItem.JSON_OBJ)
            Dim DocumentoMaestro = tituloEjecutivoObj.LstDocumentos.FirstOrDefault(Function(x) (x.ID_MAESTRO_TITULOS_DOCUMENTOS = e.Row.Cells(0).Text))
            If (DocumentoMaestro IsNot Nothing) And (DocumentoMaestro.Observacion.ID_OBSERVACIONESDOC = -1) Then
                If DocumentoMaestro.Observacion.CUMPLE_NOCUMPLE Then
                    radioSiCumple.Checked = True
                Else
                    radioNoCumple.Checked = True
                End If
            End If
        End If
    End Sub

    Protected Sub btnCerrarSuspenderTitulo_Click(sender As Object, e As EventArgs) Handles btnCerrarSuspenderTitulo.Click
        txtObservacionSuspenderTitulo.Text = String.Empty
    End Sub

    Protected Sub cmdSuspenderTitulo_Click(sender As Object, e As EventArgs) Handles cmdSuspenderTitulo.Click
        'Se guarda la observación 
        Dim tareaObservacion As New Entidades.TareaObservacion
        tareaObservacion.OBSERVACION = txtObservacionSuspenderTitulo.Text
        tareaObservacion.IND_ESTADO = 1
        tareaObservacion.FEC_CREACION = DateTime.Now.ToString("dd/MM/yyyy")
        'Se guarda la observación
        Dim _tareaObservacionBLL As New TareaObservacionBLL()
        tareaObservacion = _tareaObservacionBLL.crearTareaObservacion(tareaObservacion)

        'Se obtiene la tarea asiganada
        Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
        Dim tareaAsignada = _tareaAsignadaBLL.consultarTareaPorId(Long.Parse(HdnIdTask.Value))

        'Se guarda la solicitud
        Dim tareaSolicitud As New Entidades.TareaSolicitud()
        tareaSolicitud.ID_TAREA_ASIGNADA = tareaAsignada.ID_TAREA_ASIGNADA
        tareaSolicitud.VAL_USUARIO_SOLICITANTE = Session("ssloginusuario")
        tareaSolicitud.VAL_TIPO_SOLICITUD = Entidades.Enumeraciones.DominioDetalle.SolicitudSuspension
        tareaSolicitud.VAL_TIPOLOGIA = Entidades.Enumeraciones.DominioDetalle.SolicitudSuspension
        tareaSolicitud.ID_TAREA_OBSERVACION = tareaObservacion.COD_ID_TAREA_OBSERVACION
        Dim _tareaSolicitudBLL As New TareaSolicitudBLL()
        _tareaSolicitudBLL.guardarTareaSolicitud(tareaSolicitud)

        'Se actualiza el estado operativo de la tarea asiganada
        _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(tareaAsignada.ID_TAREA_ASIGNADA, 7)
        'Se guarda avance de cumple no cumple 
        guardarTitulo()
        'Se redirecciona a la bandeja
        Response.Redirect("~/Security/Maestros/estudio-titulos/BandejaTitulos.aspx", True)
    End Sub

#End Region

#Region "Sincronización de expediente"
    ''' <summary>
    ''' Función que incia el consumo del servicio que crea un expediente en Documentic y sincroniza los documentos de un expediente cargados en la aplicación de cobros y coactivo
    ''' </summary>
    Private Sub sincronizarTitulo()
        Try
            'Se valida que el titulo tenga expediente
            Dim _tituloBLL As New MaestroTitulosBLL()
            Dim titulo = _tituloBLL.consultarTituloPorID(HdnIdunico.Value)
            If IsNothing(titulo.MT_expediente) Then
                Dim _expedientesUtils As New ExpedientesUtils(HdnIdunico.Value, Session("ssloginusuario"))
                _expedientesUtils.urlBase = Request.Url.Scheme + "://" + Request.Url.Authority & Request.ApplicationPath.TrimEnd("/")
                _expedientesUtils.basePath = Server.MapPath("~/")
                _expedientesUtils.iniciarInstaciaExpediente()
                _expedientesUtils.SincronizarDocumentosTitulos()
                Response.Redirect(Request.RawUrl, True)
            Else
                MostrarMensajeClasificacion()
            End If
        Catch ex As Exception
            lblMalla.Text = "Error en la conexión con el servicio para la creación de un expediente"
            mpMalla.Show()
        End Try


    End Sub

    'Evento Solo usado por estudio de titulos
    Protected Sub BtnEnviarCalificacion_Click(sender As Object, e As EventArgs) Handles BtnEnviarCalificacion.Click
        ControlSubExeptionLog(AddressOf EnviarCalificacion)
    End Sub

    Public Sub EnviarCalificacion()
        guardarTitulo()
        Dim CumpleExitoso As Boolean = False
        Dim almacenamientoTemtalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(HdnIdTask.Value.ToString()))
        Dim tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemtalItem.JSON_OBJ)
        'Si el título es automático se valida que todos los documentos cargados esten calificados
        If tituloEjecutivoObj.TituloEjecutivo.Automatico Then
            Dim respuesta As New List(Of Entidades.RespuestaMallaValidacion)
            If grdDocAutomatico.Rows.Count > 0 Then
                'Se valida que tenga documentos el título y se valida que estén calificados
                For Each row As GridViewRow In grdDocAutomatico.Rows
                    Dim RbtnSiCumpleDocServicio As RadioButton = CType(row.FindControl("RbtnSiCumpleDocServicio"), RadioButton)
                    Dim RbtnNoCumpleDocServicio As RadioButton = CType(row.FindControl("RbtnSiCumpleDocServicio"), RadioButton)
                    If Not RbtnSiCumpleDocServicio.Checked And Not RbtnNoCumpleDocServicio.Checked Then
                        Dim _respuestaMallaValidacion As New RespuestaMallaValidacion With {
                            .codigo = "017",
                            .respuesta = "Falta calificación del documento " & row.Cells(2).Text
                        }
                        respuesta.Add(_respuestaMallaValidacion)
                    End If
                Next
            Else
                Dim _respuestaMallaValidacion As New RespuestaMallaValidacion With {
                    .codigo = "028",
                    .respuesta = "Falta diligenciar datos de documentos--"
                }
                respuesta.Add(_respuestaMallaValidacion)
            End If

            If respuesta.Count > 0 Then
                lblMalla.Text = StringsResourse.MsgNoValidacionTitulo
                lblMalla.Text = My.Resources.errorGeneral
                LsvResul.DataSource = respuesta
                LsvResul.DataBind()

                LsvResul.Visible = True
                lblMallaTitulo.Visible = True
                mpMalla.Show()
                loading.Attributes("class") = ""
                Exit Sub
            End If
        End If
        ' Se valida si cumple de manera general y por documento
        CumpleExitoso = (tituloEjecutivoObj.ObservacionTitulo.CUMPLE_NOCUMPLE = True)
        'Se validan los documentos uno a uno para verificar si tiene cargado un archivo
        For Each doc As DocumentoMaestroTitulo In tituloEjecutivoObj.LstDocumentos
            If (tituloEjecutivoObj.TituloEjecutivo.Automatico) And Not String.IsNullOrEmpty(doc.COD_GUID) Then
                If doc.Observacion.CUMPLE_NOCUMPLE = False Then
                    CumpleExitoso = False
                    Exit For
                End If
            ElseIf Not String.IsNullOrEmpty(doc.DES_RUTA_DOCUMENTO) And Not IsNothing(doc.DES_RUTA_DOCUMENTO) Then
                If doc.Observacion.CUMPLE_NOCUMPLE = False Then
                    CumpleExitoso = False
                    Exit For
                End If
            End If
        Next

        Dim MetodoSel As New ObservacionCumpleBLL()
        If ValidarCumpleNoCumple(tituloEjecutivoObj) = False Then
            loading.Attributes("class") = ""
            Return
        End If

        If tituloEjecutivoObj.TituloEjecutivo.Automatico Then
            Dim servicAcutalizarInstancia As New Servicios.SrvIntCobros()
            Dim Response As New UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosResp()
            Dim Request As New UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosReq()

            'Consumir OpActualizarInstancia
            Request.fechaEvaluacion = DateTime.Now.ToShortDateString
            Request.idTipoCartera = tituloEjecutivoObj.TituloEjecutivo.tipoCartera
            Request.idTituloOrigen = tituloEjecutivoObj.TituloEjecutivo.numeroTitulo
            Request.observacionesEvaluacionTitulo = tituloEjecutivoObj.ObservacionTitulo.OBSERVACIONES
            Request.resultadoEvaluacion = If(CumpleExitoso, "00", "01")
            Request.usuarioEvaluador = Session("ssloginusuario")

            Dim documentoListaRespuesta As New List(Of UGPP.CobrosCoactivo.Entidades.UGPPSrvIntCobros.DocumentoCobros)
            'TODO: No se incluyen los documentos en la respuesta, en espera de validación con el cliente para confirmación de esto
            'For Each item As DocumentoMaestroTitulo In tituloEjecutivoObj.LstDocumentos
            '    Dim documento As New UGPP.CobrosCoactivo.Entidades.UGPPSrvIntCobros.DocumentoCobros()
            '    documento.codTipoDocumento = item.ID_DOCUMENTO_TITULO
            '    documento.codDocumentic = item.COD_GUID
            '    documento.observacionesDocumento = If(item.Observacion.CUMPLE_NOCUMPLE, StringsResourse.TxtObservacionDocAutomaticoSiCumple, StringsResourse.TxtObservacionDocAutomaticoNoCumple)
            '    documento.valNombreDocumento = item.NOM_DOC_AO
            '    documentoListaRespuesta.Add(documento)
            'Next

            Dim contextoBll As New ContextoTransaccionalBLL()
            Request.documentos = documentoListaRespuesta.ToArray()
            Try
                Response = servicAcutalizarInstancia.OpActualizarEstadoInstancia(Request, contextoBll.ObtenerContextoPorIdTitulo(tituloEjecutivoObj.TituloEjecutivo.IdunicoTitulo).ID_TX)
                If Response.ContextoRespuesta.contextoRespuesta.codEstadoTx <> "OK" Then
                    lblMalla.Text = StringsResourse.TxtServicioFallo
                    LsvResul.Visible = False
                    lblMallaTitulo.Visible = True
                    lblMallaTitulo.Visible = True
                    mpMalla.Show()
                    loading.Attributes("class") = ""
                    Exit Sub
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                LsvResul.Visible = False
                lblMalla.Text = StringsResourse.TxtServicioFallo
                lblMallaTitulo.Visible = True
                mpMalla.Show()
                loading.Attributes("class") = ""
                Exit Sub
            End Try

        End If
        If CumpleExitoso = False Then 'Se retorna a area origen
            'Revisa si tiene almenos una tipificacion para registrar (Nuevas adicciones)
            If tituloEjecutivoObj.LstTipificaciones.FirstOrDefault(Function(x) (x.HABILITADO = False)) IsNot Nothing Then
                For Each item As TipificacionCNC In tituloEjecutivoObj.LstTipificaciones
                    If item.HABILITADO = False Then
                        MetodoSel.InsertaCNCTipificacionC(item.ID_TIPIFICACION, HdnIdunico.Value, Session("ssloginusuario"))
                        item.HABILITADO = True
                    End If
                Next
            End If
            If tituloEjecutivoObj.TituloEjecutivo.Automatico Then
                Dim TareaActualizar = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(HdnIdTask.Value.ToString()))
                TareaActualizar.FEC_ACTUALIZACION = Date.Now()
                TareaActualizar.VAL_USUARIO_NOMBRE = "Devolucion Automatico"
                TareaActualizar.COD_ESTADO_OPERATIVO = 8
                tareaAsignadaBLL.actualizarTareaAsignadaDevolucion(TareaActualizar)
                lblMalla.Text = "Calificación guardada exitosamente"
                mpMalla.Show()
                loading.Attributes("class") = ""
            End If
            If tituloEjecutivoObj.TituloEjecutivo.Automatico = False Then
                'Se busca el anterior usuario asignado para fijar el destinatario  de Estudio a AreaOrigen
                Dim EstadosOpertivosBuscar As New List(Of Integer)() From {1, 4, 8}
                Dim Destinatario As String = tareaAsignadaBLL.ObtenerUsuarioDevolucion(almacenamientoTemtalItem.ID_TAREA_ASIGNADA, Session("ssloginusuario"), EstadosOpertivosBuscar)
                IngresarObservaciones(tituloEjecutivoObj, Destinatario)
                LimpiarAlmacenamiento(almacenamientoTemtalItem)
                Dim TareaActualizar = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(HdnIdTask.Value.ToString()))
                TareaActualizar.FEC_ACTUALIZACION = Date.Now()
                TareaActualizar.VAL_USUARIO_NOMBRE = Destinatario
                TareaActualizar.COD_ESTADO_OPERATIVO = 8
                tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria(TareaActualizar.FEC_ACTUALIZACION & TareaActualizar.VAL_USUARIO_NOMBRE & TareaActualizar.COD_ESTADO_OPERATIVO))
                tareaAsignadaBLL.actualizarTareaAsignadaDevolucion(TareaActualizar)
                lblMalla.Text = "Calificación guardada exitosamente"
                mpMalla.Show()
                loading.Attributes("class") = ""
            End If

        End If

        'Se consulta la información completa del título
        Dim _titulo As Entidades.MaestroTitulos = Nothing
        Try
            Dim _maestroTitulosBLL As New MaestroTitulosBLL
            _titulo = _maestroTitulosBLL.consultarTituloPorID(tituloEjecutivoObj.TituloEjecutivo.IdunicoTitulo)
            'Se verifica si el título tiene expediente
            If Not IsNothing(_titulo.MT_expediente) AndAlso CumpleExitoso Then
                'Se consulta la tarea del expediente
                Dim _tareaExpediente As Entidades.TareaAsignada = tareaAsignadaBLL.obtenerTareaAsignadaPorIdExpediente(_titulo.MT_expediente)
                _tareaExpediente.COD_ESTADO_OPERATIVO = 17 'Estado operativo retorno
                _tareaExpediente.FEC_ACTUALIZACION = DateTime.Now
                tareaAsignadaBLL.ActualizarTareaAsignada(_tareaExpediente)
                'Se establece CumpleExitoso a false para que no vuelva a crear un expediente
                CumpleExitoso = False
            End If
        Catch

        End Try

        If CumpleExitoso Then 'Valida si cumple el formulario
            sincronizarTitulo()
            'lblMalla.Text = "Calificación guardada exitosamente"
            'mpMalla.Show()
        End If
        loading.Attributes("class") = ""
    End Sub
#End Region

    ''' <summary>
    '''  Metodo para la validacioones de observaciones 
    ''' </summary>
    ''' <param name="tituloEjecutivoObj"></param>
    ''' <returns></returns>
    Public Function ValidarCumpleNoCumple(tituloEjecutivoObj As TituloEjecutivoExt) As Boolean
        If tituloEjecutivoObj.ObservacionTitulo.CUMPLE_NOCUMPLE Then
            If String.IsNullOrEmpty(tituloEjecutivoObj.ObservacionTitulo.OBSERVACIONES) Then
                If Session("mnivelacces") = 10 Then
                    tituloEjecutivoObj.ObservacionTitulo.OBSERVACIONES = StringsResourse.MsgDefaultAceptacionInformacionGeneral
                End If
            End If
        End If
        'Se valida que la observacion general no este vacia
        If tituloEjecutivoObj.ObservacionTitulo.CUMPLE_NOCUMPLE = False Then
            If String.IsNullOrEmpty(tituloEjecutivoObj.ObservacionTitulo.OBSERVACIONES) Then
                'Estudio Titulos
                If Session("mnivelacces") = 10 Then
                    lblMalla.Text = StringsResourse.MsgNoObservacionGeneralEstudio
                    mpMalla.Show()
                    Return False
                End If
                'AreaOrigen
                If Session("mnivelacces") = 11 Then
                    lblMalla.Text = StringsResourse.MsgNoObservacionGeneralAreaOrigen
                    mpMalla.Show()
                    Return False
                End If
            End If
        End If
        'Se valida que las observaciones de documentos no este vacia
        For Each item As DocumentoMaestroTitulo In tituloEjecutivoObj.LstDocumentos
            If item.Observacion.CUMPLE_NOCUMPLE Then
                If String.IsNullOrEmpty(item.Observacion.OBSERVACIONES) Then
                    If Session("mnivelacces") = 10 Then
                        item.Observacion.OBSERVACIONES = StringsResourse.MsgDefaultAceptacionDocumento
                    End If
                End If
            End If
            If item.Observacion.CUMPLE_NOCUMPLE = False Then
                If (Not String.IsNullOrEmpty(item.DES_RUTA_DOCUMENTO)) AndAlso Session("mnivelacces") = 11 And String.IsNullOrEmpty(item.Observacion.OBSERVACIONES) Then
                    lblMalla.Text = StringsResourse.MsgNoObservacionDocumentoAreaOrigen
                    mpMalla.Show()
                    Return False
                End If
                If (Not String.IsNullOrEmpty(item.DES_RUTA_DOCUMENTO)) AndAlso (String.IsNullOrEmpty(item.Observacion.OBSERVACIONES) And Session("mnivelacces") = 10 AndAlso Not tituloEjecutivoObj.TituloEjecutivo.Automatico) Then
                    'Estudio Titulos
                    lblMalla.Text = StringsResourse.MsgNoObservacionDocumentoEstudio
                    mpMalla.Show()
                    Return False
                End If
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Se ingresa las observaciones
    ''' </summary>
    ''' <param name="tituloEjecutivoObj"></param>
    ''' <param name="Destinatario"></param>
    Public Sub IngresarObservaciones(tituloEjecutivoObj As TituloEjecutivoExt, Destinatario As String)
        Dim MetodoSel As ObservacionCumpleBLL = New ObservacionCumpleBLL()
        For Each item As DocumentoMaestroTitulo In tituloEjecutivoObj.LstDocumentos
            If item.Observacion.ID_OBSERVACIONESDOC = -1 And String.IsNullOrEmpty(item.Observacion.OBSERVACIONES) = False And item.ID_MAESTRO_TITULOS_DOCUMENTOS <> 0 Then
                MetodoSel.InsertaCNCComentarioDocC(HdnIdunico.Value, item.ID_MAESTRO_TITULOS_DOCUMENTOS, Session("ssloginusuario"), Destinatario, item.Observacion.CUMPLE_NOCUMPLE, item.Observacion.OBSERVACIONES)
            End If
        Next
        Dim CheckSINO As Boolean
        If RbtnSiNoCumple.SelectedValue = "Si" Then
            CheckSINO = True
        Else
            CheckSINO = False
        End If

        If Session("mnivelacces") = 11 Then
            If CheckSINO = False Then
                MetodoSel.InsertaCNCComentarioC(HdnIdunico.Value, Session("ssloginusuario"), tituloEjecutivoObj.ObservacionTitulo.OBSERVACIONES, Destinatario, CheckSINO)
            End If
        Else
            MetodoSel.InsertaCNCComentarioC(HdnIdunico.Value, Session("ssloginusuario"), tituloEjecutivoObj.ObservacionTitulo.OBSERVACIONES, Destinatario, CheckSINO)
        End If
    End Sub
    Protected Sub ABack_Click(sender As Object, e As EventArgs) Handles ABack.Click
        redirectBandejaCorrespondiente()
    End Sub

    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "TAREA ASIGNADA"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function

    Protected Sub PaginadorDocAutomatico_EventActualizarGrid()
        Dim tituloEjecutivoObj As TituloEjecutivoExt
        Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(HdnIdTask.Value.ToString()))
        tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
        grdDocAutomatico.DataSource = tituloEjecutivoObj.LstDocumentos.Where(Function(x) (x.TIPO_RUTA = 2))
        grdDocAutomatico.DataBind()
    End Sub

    Protected Sub lsvListaDocumentos_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lsvListaDocumentos.ItemCommand
        If (String.Equals(e.CommandName, "updateMetaData")) Then
            Try
                lblErrorDocUpdate.Visible = False
                lblDocUpdateEnviada.Visible = False

                hdnIdDocUpdate.Value = String.Empty
                hlinkViewDoc.NavigateUrl = String.Empty
                txtNumPaginas.Text = String.Empty
                txtObservacionLegibilidad.Text = String.Empty

                Dim HdnIdDoc As HiddenField = CType(e.Item.FindControl("HdnIdMaestroTitulos"), HiddenField)
                hdnIdDocUpdate.Value = HdnIdDoc.Value
                Dim idTituloDocumento = Convert.ToInt32(HdnIdDoc.Value)

                Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()
                Dim _maestroTitulosDocumentos = _maestroTitulosDocumentosBLL.obtenerMaestroTitulosDocumentosPorId(Convert.ToInt32(idTituloDocumento))

                hlinkViewDoc.Visible = True
                hlinkViewDoc.NavigateUrl = _maestroTitulosDocumentos.DES_RUTA_DOCUMENTO

                txtNumPaginas.Text = _maestroTitulosDocumentos.NUM_PAGINAS
                txtObservacionLegibilidad.Text = _maestroTitulosDocumentos.OBSERVA_LEGIBILIDAD

                modalVerDocMD.Show()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub cmdAsignarDocumentoMetaData_Click(sender As Object, e As EventArgs) Handles cmdAsignarDocumentoMetaData.Click
        Try
            'Se obtiene obgeto para actualizar
            Dim idTituloDocumento = Convert.ToInt32(hdnIdDocUpdate.Value)
            Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()
            Dim _maestroTitulosDocumentos = _maestroTitulosDocumentosBLL.obtenerMaestroTitulosDocumentosPorId(Convert.ToInt32(idTituloDocumento))

            'Se actualiza la entidad
            _maestroTitulosDocumentos.NUM_PAGINAS = txtNumPaginas.Text
            _maestroTitulosDocumentos.OBSERVA_LEGIBILIDAD = txtObservacionLegibilidad.Text

            'Se actualizan los datos
            _maestroTitulosDocumentosBLL.ActualizarMaestroTitulosDocumentos(_maestroTitulosDocumentos)

            lblDocUpdateEnviada.Text = "Datos actualizados correctamente"
            lblDocUpdateEnviada.Visible = True
        Catch ex As Exception
            lblErrorDocUpdate.Text = "Los sentimos, ha ocurrdo un error en la actualización"
            lblErrorDocUpdate.Visible = True
        End Try
    End Sub

    ''' <summary>
    ''' Evento que oculta o muestra las tipificación de error si el título no cumple para ser cobrable
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub RbtnSiNoCumple_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RbtnSiNoCumple.SelectedIndexChanged
        If RbtnSiNoCumple.SelectedValue = "Si" Then
            trTipificaciones.Visible = False
        Else
            trTipificaciones.Visible = True
        End If
    End Sub

    Private Sub AsignarGestion()
        Dim _tareaAsiganada As String = ""
        Dim tareaAsignadaBLL As New TareaAsignadaBLL
        Dim registros As New TareaAsignada
        If Len(Request("ID_TASK")) > 0 Then
            _tareaAsiganada = Request("ID_TASK").Trim
            registros = tareaAsignadaBLL.obtenerTareaAsignadaPorId(_tareaAsiganada)
            If registros IsNot Nothing Then
                If (registros.COD_ESTADO_OPERATIVO = 4 AndAlso registros.VAL_USUARIO_NOMBRE = Session("ssloginusuario")) Then
                    tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("idtareasig=" & registros.ID_TAREA_ASIGNADA & ",estadoope=5"))
                    tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(registros.ID_TAREA_ASIGNADA, 5)
                End If
            End If
        End If
    End Sub

    Protected Sub chkTituloEjecutoriado_CheckedChanged(sender As Object, e As EventArgs) Handles chkTituloEjecutoriado.CheckedChanged
        ValidaCheckEjecutoria()
    End Sub

    Protected Sub btnCloseVerDocMD_Click(sender As Object, e As EventArgs) Handles btnCloseVerDocMD.Click
        modalVerDocMD.Hide()
    End Sub

    Protected Sub ValidaCheckEjecutoria()
        If (chkTituloEjecutoriado.Checked) Then
            txtMT_fecha_ejecutoriaObli.Enabled = True
            'If cboTipo_Cartera.SelectedValue = "1" Then
            '    txtMT_fec_exi_liqObli.Enabled = False
            'Else
            txtMT_fec_exi_liqObli.Enabled = True
            'End If
        Else
            txtMT_fecha_ejecutoriaObli.Enabled = False
            txtMT_fecha_ejecutoriaObli.Text = String.Empty
            txtMT_fec_exi_liqObli.Enabled = False
            txtMT_fec_exi_liqObli.Text = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Funcion que valida si ya se guardaron los documentos del tiulo 
    ''' </summary>
    ''' <param name="idTitulo"></param>
    ''' <returns></returns>
    Private Function HabilitarCargue(ByVal idTitulo As Integer) As Boolean
        Dim MetodoSel As ValidadorBLL = New ValidadorBLL()
        Dim LstDocumentosMaestro As List(Of DocumentoMaestroTitulo) = MetodoSel.ConsultarDocumentos(idTitulo)
        Return LstDocumentosMaestro.Where(Function(t) t.TIPO_RUTA = 1).Count() <= 0
    End Function

    'Protected Sub BtnFinalizaC_Click(sender As Object, e As EventArgs) Handles BtnFinalizaC.Click
    '    guardarTitulo()
    '    LsvResul.Visible = False
    '    'Si ID_TASK tiene valor se carga valida el item
    '    tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(HdnIdTask.Value.ToString()))
    '    Dim tituloEjecutivoObj As TituloEjecutivoExt
    '    Dim validadorMalla As New ValidadorBLL()
    '    Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(tareaAsignadaObject.ID_TAREA_ASIGNADA)
    '    'Si TareaAsignadaObject.ID_UNICO_TITULO esta vacio realiza el cargue del objeto parcial
    '    tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
    '    If Boolean.Parse(HdnAutomaticoCargue.Value) Then
    '        validadorMalla.MallaValidadoraTituloAutomaticoDocumentos(tituloEjecutivoObj.TituloEjecutivo, tituloEjecutivoObj.LstDocumentos)
    '        If validadorMalla.Idunicotitulo = -1 Then
    '            lblMalla.Text = StringsResourse.MsgErrorNoControladoMalla
    '        End If
    '        If validadorMalla.Idunicotitulo = 0 Then
    '            lblMalla.Text = StringsResourse.MsgNoValidacionTitulo
    '            If validadorMalla.respuesta.Count > 0 Then
    '                LsvResul.Visible = True
    '                LsvResul.DataSource = validadorMalla.respuesta
    '                LsvResul.DataBind()
    '            End If
    '        End If
    '        If validadorMalla.Idunicotitulo <> 0 Then
    '            LimpiarAlmacenamiento(almacenamientoTemportalItem)
    '            lblMalla.Text = StringsResourse.MsgActualizacionTitulo
    '        End If
    '        mpMalla.Show()
    '    End If

    Public Sub crearTarea()
        'Si ID_TASKno tiene valor se crea una tarea
        tareaAsignadaObject = New TareaAsignada()
        tareaAsignadaObject.COD_TIPO_OBJ = 4 ' TITULO
        tareaAsignadaObject.VAL_USUARIO_NOMBRE = Session("ssloginusuario")
        tareaAsignadaObject.COD_ESTADO_OPERATIVO = 1
        tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("codtipo=" + tareaAsignadaObject.COD_TIPO_OBJ.ToString() + ",usunombre=" & tareaAsignadaObject.VAL_USUARIO_NOMBRE + ",codestadoope=" & tareaAsignadaObject.COD_ESTADO_OPERATIVO.ToString()))
        tareaAsignadaObject = tareaAsignadaBLL.registrarTarea(tareaAsignadaObject)
        Dim almacenamientoTemportalItem As AlmacenamientoTemporal = New AlmacenamientoTemporal()
        almacenamientoTemportalItem.ID_TAREA_ASIGNADA = tareaAsignadaObject.ID_TAREA_ASIGNADA
        almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(New TituloEjecutivoExt())
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("idtareaasig=" + almacenamientoTemportalItem.ID_TAREA_ASIGNADA.ToString() + ",jsonobj=" & almacenamientoTemportalItem.JSON_OBJ))
        almacenamientoTemporalBLL.InsertarAlmacenamiento(almacenamientoTemportalItem)
        HdnIdTask.Value = tareaAsignadaObject.ID_TAREA_ASIGNADA
        HdnIdEstadoOperativo.Value = tareaAsignadaObject.COD_ESTADO_OPERATIVO
    End Sub

    Protected Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        resetFormulario()
    End Sub
End Class
