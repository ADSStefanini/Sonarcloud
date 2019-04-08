Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
Imports Ec = coactivosyp.EnumClasificacion
Public Class ClasificacionManual2
    Inherits System.Web.UI.Page

    'Declaración de clases necesarias
    Dim clasificacionManual As New ClasificacionManualBLL
    Dim negocio As New HistoricoClasificacionManualBLL
    Dim dato As New HistoricoClasificacionManual
    Dim objTiposProcesoConcursalesb As New TiposProcesosConcursalesBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Se cargan mensajes de validación
            ddlTipoPersonaValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlMatriculaMercantilValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlPersonaVivaValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlProcesoEspecialJuridicaValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlTipoProcesoJuridicaValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlProcesoEspecialNaturalValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlTipoProcesoNaturalValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlBeneficioTributarioValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            ddlPagosDeudorValidator.Text = My.Resources.Formularios.errorSinOpcionSeleccionada
            reqNumeroRadicado.Text = My.Resources.Formularios.errorCampoRequerido

            'Se poblan dropdowns
            CommonsCobrosCoactivos.CargarComboJuridica(ddlTipoProcesoJuridica)
            CommonsCobrosCoactivos.CargarComboNatural(ddlTipoProcesoNatural)

            Dim idTask As Int32
            Dim expedienteID As String = Nothing
            Dim expediente As Entidades.EJEFISGLOBAL = Nothing

            If Len(Request.QueryString("ID_TASK")) Then
                'Se captira el ID de la tarea asiganada
                idTask = Convert.ToInt32(Request.QueryString("ID_TASK"))
                'Se trae la tarea de la base de datos
                Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                Dim _tareaAsiganada = _tareaAsignadaBLL.obtenerTareaAsignadaPorId(idTask) 'Validar que la tarea exista
                If Not IsNothing(_tareaAsiganada.ID_UNICO_TITULO) Then
                    newDoc.idTitulo = _tareaAsiganada.ID_UNICO_TITULO
                    hdnIdTitulo.Value = _tareaAsiganada.ID_UNICO_TITULO
                    'Se trae el tírulo de la DB
                    Dim _maestroTitulosBLL As New MaestroTitulosBLL() 'Validar que el tipo objeto sea 4
                    Dim _titulo = _maestroTitulosBLL.consultarTituloPorID(_tareaAsiganada.ID_UNICO_TITULO)
                    'Obtengo el ID del expediente
                    expedienteID = _titulo.MT_expediente
                    hdnExpediente.Value = _titulo.MT_expediente
                    Dim _expedienteBLL As New ExpedienteBLL()
                    expediente = _expedienteBLL.obtenerExpedientePorId(expedienteID)
                End If

            End If

            'Se muestra error de expediente no calificable
            If IsNothing(expedienteID) Or (Not IsNothing(expediente) AndAlso expediente.EFIESTADO = 13) Then
                lblNoClasificable.Text = My.Resources.ValExpedienteNoClasificable
                lblNoClasificable.Visible = True
                Panel1.Enabled = False
            Else
                'Se realizar la clasificación por cuantia
                If (CommonsCobrosCoactivos.ClasificacionUVT(idTask, expedienteID)) Then
                    hdnEPP.Value = My.Resources.ValEstadoProcesoDificilCobro
                    hdnEtapaEPP.Value = My.Resources.ValEtapaDificilCobro
                End If

                'Se selecciona automático el tipo de persona relacionada con el título
                Dim _entesDeudoresBLL As New EntesDeudoresBLL()
                Dim _deudores = _entesDeudoresBLL.obtenerDeudoresPorTitulo(hdnIdTitulo.Value)
                Dim _deudor = _deudores.FirstOrDefault()

                Dim _tipoPersona As String = Nothing

                If IsNothing(_deudor) Then
                    Dim almacenamientoTemporalBLL As New AlmacenamientoTemporalBLL
                    Dim tituloEjecutivoObj As TituloEjecutivoExt
                    Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(idTask)
                    tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
                    Dim _deudorGuardadoParcial = tituloEjecutivoObj.LstDeudores.FirstOrDefault()
                    _tipoPersona = _deudorGuardadoParcial.tipoPersona
                Else
                    _tipoPersona = _deudor.ED_TipoPersona
                End If

                If IsNothing(_tipoPersona) Then
                    lblNoClasificable.Text = "El título no cuenta con deudores asignados"
                    lblNoClasificable.Visible = True
                    Panel1.Enabled = False
                Else
                    If _tipoPersona = "01" Then
                        ddlTipoPersona.Items.FindByValue("2").Selected = True
                        ddlTipoPersona_SelectedIndexChanged(sender, e)
                    ElseIf (_tipoPersona <> "00") Then
                        ddlTipoPersona.Items.FindByValue("1").Selected = True
                        ddlTipoPersona_SelectedIndexChanged(sender, e)
                    End If
                End If

            End If

        End If
    End Sub

    Protected Sub ddlTipoPersona_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoPersona.SelectedIndexChanged
        'Ocultan las opciones siguientes 
        hdnEPP.Value = ""
        hdnEtapaEPP.Value = ""
        ' Se calcula la uvt
        Dim idTask As Int32
        Dim expedienteID As String = Nothing

        If Len(Request.QueryString("ID_TASK")) Then
            'Se captira el ID de la tarea asiganada
            idTask = Convert.ToInt32(Request.QueryString("ID_TASK"))
            'Se trae la tarea de la base de datos
            Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
            Dim _tareaAsiganada = _tareaAsignadaBLL.obtenerTareaAsignadaPorId(idTask) 'Validar que la tarea exista
            If Not IsNothing(_tareaAsiganada.ID_UNICO_TITULO) Then
                'Se trae el tírulo de la DB
                Dim _maestroTitulosBLL As New MaestroTitulosBLL() 'Validar que el tipo objeto sea 4
                Dim _titulo = _maestroTitulosBLL.consultarTituloPorID(_tareaAsiganada.ID_UNICO_TITULO)
                'Obtengo el ID del expediente
                expedienteID = _titulo.MT_expediente
                'Dim _expedienteBLL As New ExpedienteBLL()
            End If
        End If
        If (CommonsCobrosCoactivos.ClasificacionUVT(idTask, expedienteID)) Then
            hdnEPP.Value = My.Resources.ValEstadoProcesoDificilCobro
            hdnEtapaEPP.Value = My.Resources.ValEtapaDificilCobro
        End If
        VisibilidadNivel(Ec.TipoPersona)
        Dim _selected = ddlTipoPersona.SelectedValue
        If _selected = "0" Then
            Exit Sub
        End If
        'Se asigna el número del título
        newDoc.idTitulo = hdnIdTitulo.Value
        If (_selected = "1") Then
            'Juridica
            Me.newDoc.idDocumento = 52
            VisibilidadNivel(Ec.MatriculaMercantil)
        ElseIf (_selected = "2") Then
            'Natural
            VisibilidadNivel(Ec.PersonaViva)
            'Asignacón ID del documento vigencia de ciudadania
            Me.newDoc.idDocumento = 41
        End If
    End Sub


    Protected Sub ddlMatriculaMercantil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMatriculaMercantil.SelectedIndexChanged
        Dim _selected = ddlMatriculaMercantil.SelectedValue

        VisibilidadNivel(Ec.MatriculaMercantil)

        If (_selected = "1") Then
            VisibilidadNivel(Ec.UserControlCarga)
            VisibilidadNivel(Ec.JuridicaProceso)
        End If
        If (_selected = "2") Then
            hdnEPP.Value = My.Resources.ValEstadoProcesoCarteraIncobrable
            hdnEtapaEPP.Value = My.Resources.ValEtapaCarteraIncobrable
            VisibilidadNivel(Ec.UserControlCarga)
            VisibilidadNivel(Ec.BeneficioTributario)
        End If
        ddlProcesoEspecialJuridicaValidator.Enabled = (_selected <> "0")
    End Sub

    Protected Sub ddlProcesoEspecialJuridica_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProcesoEspecialJuridica.SelectedIndexChanged
        Dim _selected = ddlProcesoEspecialJuridica.SelectedValue
        VisibilidadNivel(Ec.JuridicaProceso)
        If (_selected = "1") Then
            VisibilidadNivel(Ec.JuridicaTipo)
        End If
        If (_selected = "2") Then
            VisibilidadNivel(Ec.BeneficioTributario)
        End If
        ddlProcesoEspecialJuridica.Enabled = (_selected <> "0")
    End Sub

    Protected Sub ddlTipoProcesoNatural_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoProcesoNatural.SelectedIndexChanged

        Dim _selected = ddlTipoProcesoNatural.SelectedValue
        VisibilidadNivel(Ec.NaturalTipo)
        If _selected <> "0" Then
            Dim TipoEstadoProcesal = objTiposProcesoConcursalesb.ObtenerTipoProcesoNatural().FirstOrDefault(Function(t) t.codigo = Int32.Parse(_selected))

            If TipoEstadoProcesal IsNot Nothing And String.IsNullOrEmpty(TipoEstadoProcesal.ESTADO_PROCESO_J) And TipoEstadoProcesal.ESTADO_PROCESO_J <> "0" Then
                hdnEPP.Value = TipoEstadoProcesal.ESTADO_PROCESO_N
            End If
            VisibilidadNivel(Ec.BeneficioTributario)
        End If

    End Sub

    Protected Sub ddlPersonaViva_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPersonaViva.SelectedIndexChanged
        Dim _selected = ddlPersonaViva.SelectedValue
        VisibilidadNivel(Ec.PersonaViva)

        If (_selected = "1") Then
            VisibilidadNivel(Ec.UserControlCarga)
            VisibilidadNivel(Ec.NaturalProceso)
        End If
        If (_selected = "2") Then
            hdnEPP.Value = My.Resources.ValEstadoProcesoDificilCobro
            hdnEtapaEPP.Value = My.Resources.ValEtapaDificilCobro
            VisibilidadNivel(Ec.UserControlCarga)
            VisibilidadNivel(Ec.Observaciones)
        End If
    End Sub

    Protected Sub ddlProcesoEspecialNatural_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProcesoEspecialNatural.SelectedIndexChanged

        Dim _selected = ddlProcesoEspecialNatural.SelectedValue
        VisibilidadNivel(Ec.NaturalProceso)
        If (_selected = "1") Then
            VisibilidadNivel(Ec.NaturalTipo)
        End If
        If (_selected = "2") Then
            VisibilidadNivel(Ec.BeneficioTributario)
        End If
    End Sub

    Protected Sub ddlTipoProcesoJuridica_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoProcesoJuridica.SelectedIndexChanged
        Dim _selected = ddlTipoProcesoJuridica.SelectedValue
        VisibilidadNivel(Ec.JuridicaTipo)
        If _selected <> "0" Then
            ddlTipoProcesoJuridicaValidator.Enabled = True
            VisibilidadNivel(Ec.BeneficioTributario)
            Dim TipoEstadoProcesal = objTiposProcesoConcursalesb.ObtenerTipoProcesoJuridica().FirstOrDefault(Function(t) t.codigo = Int32.Parse(_selected))

            If TipoEstadoProcesal IsNot Nothing And String.IsNullOrEmpty(TipoEstadoProcesal.ESTADO_PROCESO_J) And TipoEstadoProcesal.ESTADO_PROCESO_J <> "0" Then
                hdnEPP.Value = TipoEstadoProcesal.ESTADO_PROCESO_J
            End If
        End If
    End Sub

    Protected Sub ddlBeneficioTributario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBeneficioTributario.SelectedIndexChanged

        Dim _selected = ddlBeneficioTributario.SelectedValue
        VisibilidadNivel(Ec.BeneficioTributario)
        If _selected = "1" And hdnEPP.Value = My.Resources.ValEstadoProcesoConcursal Then
            hdnEtapaEPP.Value = My.Resources.ValEtapaBeneficioTributarioConcursal
        End If

        If _selected = "1" And hdnEPP.Value <> My.Resources.ValEstadoProcesoConcursal Then
            hdnEPP.Value = My.Resources.ValEstadoProcesoPersuasivo
            hdnEtapaEPP.Value = My.Resources.ValEtapaBeneficioTributarioPersuasivo
        End If
        VisibilidadNivel(Ec.PagoDeudor)
    End Sub

    Protected Sub ddlPagosDeudor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPagosDeudor.SelectedIndexChanged

        Dim _selected = ddlPagosDeudor.SelectedValue
        VisibilidadNivel(Ec.PagoDeudor)
        If (_selected = "1") Then
            VisibilidadNivel(Ec.NumeroRadicado)
            hdnEPP.Value = My.Resources.ValEstadoProcesoVerifPagos
            hdnEtapaEPP.Value = My.Resources.ValEtapaNormalizacion
        End If
        If (_selected = "2") And String.IsNullOrEmpty(hdnEPP.Value) Then
            hdnEPP.Value = My.Resources.ValEstadoProcesoPersuasivo
            hdnEtapaEPP.Value = My.Resources.ValEtapaDSIAC
        End If

        If (_selected <> "0") Then
            VisibilidadNivel(Ec.Observaciones)
        End If


    End Sub


    Protected Sub btnGuardar2_Click(sender As Object, e As EventArgs) Handles btnGuardar2.Click

        If Not Page.IsValid Then
            loadingClasificacion.Attributes("Class") = ""
            Exit Sub
        End If
        btnGuardar2.Enabled = False
        btnCancel.Enabled = False
        Dim docId = newDoc.obtenerIdMaestroDocumentoTitulo()
        If ddlTipoPersona.SelectedValue = "1" AndAlso (String.IsNullOrEmpty(docId) OrElse docId = "0") Then
            lblNoClasificable.Text = My.Resources.Formularios.errorDocumentoRequerido
            lblNoClasificable.Visible = True
            Exit Sub
        End If

        Dim valDdlMatriculaMercantil = ddlMatriculaMercantil.SelectedValue.ToString()
        Dim valDdlPersonaViva = ddlPersonaViva.SelectedValue.ToString()
        Dim valDdlTipoProcesoJuridica = ddlTipoProcesoJuridica.SelectedValue.ToString()
        Dim valDdlBeneficioTributario = ddlBeneficioTributario.SelectedValue.ToString()
        Dim valDdlTipoProcesoNatural = ddlTipoProcesoNatural.SelectedValue.ToString()
        Dim valDdlPagosDeudor = ddlPagosDeudor.SelectedValue.ToString()
        Dim valueDdlTipoPersona = ddlTipoPersona.SelectedValue.ToString()
        Dim valueDdlProcesoEspecialJuridica = ddlProcesoEspecialJuridica.SelectedValue.ToString()
        Dim valueDdlTipoProcesoJuridica = ddlTipoProcesoJuridica.SelectedValue.ToString()
        Dim valueDdlProcesoEspecialNatural = ddlProcesoEspecialNatural.SelectedValue.ToString()

        Dim idTask = Convert.ToInt32(Request.QueryString("ID_TASK"))
        Dim taskBLL As New TareaAsignadaBLL()
        Dim task = taskBLL.obtenerTareaAsignadaPorId(idTask)
        Dim _tituloBLL As New MaestroTitulosBLL()
        Dim titulo = _tituloBLL.consultarTituloPorID(task.ID_UNICO_TITULO)
        Dim idExpediente = titulo.MT_expediente
        Dim save As Boolean = False
        lblNoClasificable.Visible = False

        clasificacionManual.idTareaAsiganada = idTask

#Region "Esenarios donde no se clasifica automaticamente"
        If valueDdlTipoPersona = "2" And valDdlPersonaViva = "2" Then
            FinalizarClasificacion(idExpediente)
            Return
        End If

        If (hdnEPP.Value = My.Resources.ValEstadoProcesoVerifPagos And hdnEtapaEPP.Value = My.Resources.ValEtapaNormalizacion) Then
            FinalizarClasificacion(idExpediente)
            Return
        End If

        If (String.IsNullOrEmpty(hdnEPP.Value) = False And valDdlPagosDeudor = "2" And hdnEPP.Value <> My.Resources.ValEstadoProcesoPersuasivo And hdnEtapaEPP.Value <> My.Resources.ValEtapaDSIAC) Then
            FinalizarClasificacion(idExpediente)
            Return
        End If
#End Region

#Region "Clasificacion Automatica"
        Dim fechaCalculo As DateTime = New DateTime()
        If titulo.MT_fecha_ejecutoria.HasValue Then
            fechaCalculo = titulo.MT_fecha_ejecutoria.Value
        End If
        If titulo.MT_fecha_ejecutoria.HasValue = False And titulo.MT_fec_notificacion_titulo.HasValue Then
            fechaCalculo = titulo.MT_fec_notificacion_titulo.Value
        End If

        If DateDiff(DateInterval.Month, fechaCalculo, Date.Now) <= My.Resources.ValMesesTerminoPrescripcion Then
            hdnEPP.Value = My.Resources.ValEstadoProcesoCoactivo
            hdnEtapaEPP.Value = My.Resources.ValEtapaMandamientoDePago
            FinalizarClasificacion(idExpediente)
            Return
        End If


        If titulo.MT_fec_exi_liq > task.FEC_ENTREGA_GESTOR Then
            hdnEPP.Value = My.Resources.ValEstadoProcesoFacilPago
            hdnEtapaEPP.Value = 21
            FinalizarClasificacion(idExpediente)
            Return
        End If

        Select Case titulo.MT_tipo_titulo
            Case My.Resources.ValTiTituloSentJudicial, My.Resources.ValTiTituloCuotaPartPension, My.Resources.ValTiTituloMaValPagadosPorFraude
                hdnEPP.Value = My.Resources.ValEstadoProcesoCoactivo
                hdnEtapaEPP.Value = My.Resources.ValEtapaMandamientoDePago
            Case My.Resources.ValTiTituloReqParaDecYOCorregir, My.Resources.ValTiTuloPliegoDeCargos, My.Resources.ValTiTituloInformeDeFiscalizacion
                hdnEPP.Value = My.Resources.ValEstadoProcesoConcursal
                hdnEtapaEPP.Value = 20
            Case Else
                hdnEPP.Value = My.Resources.ValEstadoProcesoPersuasivo
                hdnEtapaEPP.Value = My.Resources.ValEtapaDSIAC
        End Select
        FinalizarClasificacion(idExpediente)
        loadingClasificacion.Attributes("Class") = ""
#End Region
    End Sub

    Public Sub FinalizarClasificacion(ByVal idExpediente As String)
        Try
            loadingClasificacion.Attributes("Class") = ""
            clasificacionManual.EstablecerEstadoProcesal(idExpediente, hdnEPP.Value, hdnEtapaEPP.Value)
            saveHitoricoClasificacionManual(idExpediente)
            Dim estadoBll = New EstadosProcesoBLL()
            Dim etapaBll = New EtapaProcesalBLL()
            pnlMensaje.Visible = True
            Dim estado = estadoBll.obtenerEstadosProcesos.FirstOrDefault(Function(t) t.codigo = hdnEPP.Value)
            If estado IsNot Nothing Then
                lblMensajeClasificacion.Text = " Se Clasifico el expediente " + idExpediente + " En el estado :" + estado.nombre
            End If

            Dim etapa = etapaBll.ObtenerEtapaProcesal().FirstOrDefault(Function(t) t.ID_ETAPA_PROCESAL = hdnEtapaEPP.Value)
            If etapa IsNot Nothing Then
                lblMensajeClasificacion.Text = lblMensajeClasificacion.Text + " y etapa " + etapa.VAL_ETAPA_PROCESAL
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnCerrarAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptarClasificacion.Click
        Response.Redirect("~/Security/Maestros/estudio-titulos/BandejaTitulos.aspx", True)
    End Sub
    ''' <summary>
    ''' permite realizar guardado en la tabla historico manual
    ''' </summary>
    ''' <param name="idExpediente"></param>
    Protected Sub saveHitoricoClasificacionManual(idExpediente)

        'variables
        Dim valDdlMatriculaMercantil = ddlMatriculaMercantil.SelectedValue.ToString()
        Dim valDdlPersonaViva = ddlPersonaViva.SelectedValue.ToString()

        Dim valDdlBeneficioTributario = ddlBeneficioTributario.SelectedValue.ToString()
        Dim valDdlTipoProcesoNatural = ddlTipoProcesoNatural.SelectedValue.ToString()
        Dim valDdlPagosDeudor = ddlPagosDeudor.SelectedValue.ToString()
        Dim valueDdlTipoPersona = ddlTipoPersona.SelectedValue.ToString()
        Dim valueDdlProcesoEspecialJuridica = ddlProcesoEspecialJuridica.SelectedValue.ToString()
        Dim valueDdlTipoProcesoJuridica = ddlTipoProcesoJuridica.SelectedValue.ToString()
        Dim valueDdlProcesoEspecialNatural = ddlProcesoEspecialNatural.SelectedValue.ToString()
        Dim valPersonaJ As Boolean
        Dim valPersonaN As Boolean
        Dim valPersonaV As Boolean
        Dim valMatriculaM As Boolean
        Dim valDdlTipoProceso As Int32
        Dim valDdlTipoProcesoEspecial As Boolean
        Dim valDdlBenTributario As Boolean
        Dim valPagosDeudor As Boolean
        Dim clasfificacionCuantia As Boolean
        Dim valNumRadicado = txtNumeroRadicado.Text
        Dim valObservacion = TxtAreaObservaciones.Text

        If valueDdlTipoPersona = My.Resources.ValPositivoddl Then
            valPersonaJ = True
        Else
            valPersonaJ = False
        End If
        If valueDdlTipoPersona = My.Resources.ValNegativoddl Then
            valPersonaN = True
        Else
            valPersonaN = False
        End If
        If valDdlPersonaViva = My.Resources.ValPositivoddl Then
            valPersonaV = True
        ElseIf valDdlPersonaViva = My.Resources.ValNegativoddl Then
            valPersonaV = False
        End If
        If valDdlMatriculaMercantil = My.Resources.ValPositivoddl Then
            valMatriculaM = True
        ElseIf valDdlMatriculaMercantil = My.Resources.ValNegativoddl Then
            valMatriculaM = False
        End If
        If valueDdlTipoProcesoJuridica > My.Resources.DefaultValueDdlTipoProceso Then
            valDdlTipoProceso = Convert.ToInt32(valueDdlTipoProcesoJuridica)
        ElseIf valDdlTipoProcesoNatural > My.Resources.DefaultValueDdlTipoProceso Then
            valDdlTipoProceso = Convert.ToInt32(valDdlTipoProcesoNatural)
        End If
        If valueDdlProcesoEspecialJuridica = My.Resources.ValPositivoddl Then
            valDdlTipoProcesoEspecial = True
        ElseIf valueDdlProcesoEspecialJuridica = My.Resources.ValNegativoddl Then
            valDdlTipoProcesoEspecial = False
        End If
        If valueDdlProcesoEspecialNatural = My.Resources.ValPositivoddl Then
            valDdlTipoProcesoEspecial = True
        ElseIf valueDdlProcesoEspecialNatural = My.Resources.ValNegativoddl Then
            valDdlTipoProcesoEspecial = False
        End If
        If valDdlBeneficioTributario = My.Resources.ValPositivoddl Then
            valDdlBenTributario = True
        ElseIf valDdlBeneficioTributario = My.Resources.ValNegativoddl Then
            valDdlBenTributario = False
        End If
        If valDdlPagosDeudor = My.Resources.ValPositivoddl Then
            valPagosDeudor = True
        ElseIf valDdlPagosDeudor = My.Resources.ValNegativoddl Then
            valPagosDeudor = False
        End If

        If valNumRadicado <> Nothing Then
            Convert.ToInt32(valNumRadicado)
        End If
        Dim idDoc = Me.newDoc.obtenerIdMaestroDocumentoTitulo()
        idDoc = If(idDoc = "0", Nothing, idDoc)

        dato.ID_EXPEDIENTE = Convert.ToString(idExpediente)
        dato.ID_USUARIO = Session("sscodigousuario")
        dato.FECHA = DateTime.Now
        dato.PERSONA_JURIDICA = valPersonaJ
        dato.PERSONA_NATURAL = valPersonaN
        dato.PERSONA_VIVA = valDdlPersonaViva
        dato.MATRICULA_MERCANTIL = valMatriculaM
        dato.ID_MTD_DOCUMENTO = If(IsNothing(idDoc), Nothing, idDoc)
        dato.PROCESO_ESPECIAL = valDdlTipoProcesoEspecial
        dato.TIPO_PROCESO = If(valDdlTipoProceso = 0, Nothing, valDdlTipoProceso)
        dato.BENEFICIO_TRIBUTARIO = valDdlBenTributario
        dato.PAGOS_DEUDOR = valPagosDeudor
        dato.NUMERO_RADICADO = If(String.IsNullOrEmpty(valNumRadicado), 0, Convert.ToInt32(valNumRadicado))
        dato.OBSERVACIONES = valObservacion
        dato.VALOR_MENOR_UVT = clasfificacionCuantia
        negocio.Salvar(dato)
    End Sub

    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "Seguridad"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function

    Public Sub VisibilidadNivel(ByVal ParamNivel As EnumClasificacion)
        btnGuardar2.Visible = False
#Region "Visible"
        Select Case ParamNivel
            Case Ec.MatriculaMercantil
                MatriculaMercantil.Visible = True
            Case Ec.PersonaViva
                PersonaViva.Visible = True
            Case Ec.UserControlCarga
                div_upload_file.Visible = True
            Case Ec.JuridicaProceso
                pnlProcesoEspecialJuridica.Visible = True
            Case Ec.JuridicaTipo
                pnlTipoProcesoJuridica.Visible = True
            Case Ec.NaturalProceso
                pnlProcesoEspecialNatural.Visible = True
            Case Ec.NaturalTipo
                pnlTipoProcesoNatural.Visible = True
            Case Ec.BeneficioTributario
                pnlBeneficioTributario.Visible = True
            Case Ec.PagoDeudor
                pnlPagosDeudor.Visible = True
            Case Ec.NumeroRadicado
                pnlNumeroRadicado.Visible = True
            Case Ec.Observaciones
                pnlObservaciones.Visible = True
                btnGuardar2.Visible = True
        End Select
#End Region
#Region "OcultaSuperiores"
        If ParamNivel < Ec.MatriculaMercantil Then
            MatriculaMercantil.Visible = False
            ddlMatriculaMercantil.SelectedValue = "0"
        End If
        If ParamNivel < Ec.PersonaViva Then
            PersonaViva.Visible = False
            ddlPersonaViva.SelectedValue = "0"
        End If
        If ParamNivel < Ec.UserControlCarga Then
            div_upload_file.Visible = False
        End If
        If ParamNivel < Ec.JuridicaProceso Then
            pnlProcesoEspecialJuridica.Visible = False
            ddlProcesoEspecialJuridicaValidator.Enabled = False
        End If
        If ParamNivel < Ec.JuridicaTipo Then
            pnlTipoProcesoJuridica.Visible = False
            ddlTipoProcesoJuridicaValidator.Enabled = False
        End If
        If ParamNivel < Ec.NaturalProceso Then
            pnlProcesoEspecialNatural.Visible = False
            ddlProcesoEspecialNaturalValidator.Enabled = False
        End If
        If ParamNivel < Ec.NaturalTipo Then
            pnlTipoProcesoNatural.Visible = False
            ddlTipoProcesoNaturalValidator.Enabled = False
        End If
        If ParamNivel < Ec.BeneficioTributario Then
            pnlBeneficioTributario.Visible = False
            ddlBeneficioTributario.SelectedValue = "0"
            ddlBeneficioTributarioValidator.Enabled = False
        End If
        If ParamNivel < Ec.PagoDeudor Then
            pnlPagosDeudor.Visible = False
            ddlPagosDeudor.SelectedValue = "0"
        End If
        If ParamNivel < Ec.NumeroRadicado Then
            pnlNumeroRadicado.Visible = False
            reqNumeroRadicado.Enabled = True
            txtNumeroRadicado.Text = ""
        End If
        If ParamNivel < Ec.Observaciones Then
            pnlObservaciones.Visible = False
            TxtAreaObservaciones.Text = ""
        End If
#End Region
    End Sub
    Public Sub ClasificacionUvt()

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        VisibilidadNivel(Ec.TipoPersona)
    End Sub
End Class