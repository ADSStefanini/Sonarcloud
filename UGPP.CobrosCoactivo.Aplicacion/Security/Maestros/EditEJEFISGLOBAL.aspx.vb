Imports System.Data
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class EditEJEFISGLOBAL
    Inherits coactivosyp.BasePage

    Public Property intObliFechaPresentacion() As Integer
        Get
            Return _intDias
        End Get
        Set(value As Integer)
            _intDias = value
        End Set
    End Property
    Protected Shared _intDias As Integer
    Protected Shared intDiasPresentacionConcursales As Integer = 10
    Dim cs As ClientScriptManager = Page.ClientScript

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If MyBase.Page_Load(sender, e, Nothing) Then
            Exit Sub
        End If

        If IsPostBack = False Then

            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            Dim msg As String = ""

            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilitan los campos de las pestañas Cobro persuasivo-Datos-Resumen Gestión Persuasiva para este perfil
                cboResCobCC.Enabled = False
                cboCriteriosMC.Enabled = False
                cboEstaFinPag.Enabled = False
                cboCausalFinPers.Enabled = False
                txtAutoTerm.Enabled = False
                txtFecTerm.Enabled = False
                imgBtnBorraFecTerm.Enabled = False
                cmdSavePersuasivo.Enabled = False

                'Se inhabilitan los campos de las pestañas Cobro coactivo-General para este perfil
                DesactivarCoactivo()
                cmdSaveCoactivo1.Enabled = False

                'Se inhabilitan los campos de las pestañas Cobro coactivo-Liquidación del crédito para este perfil
                txtLiqCredCapital.Enabled = False
                txtLiqCredInteres.Enabled = False
                txtLiqCredFecCorte.Enabled = False

                'Se inhabilitan los campos de las pestañas Procesos concursales-Datos para este perfil
                DesactivarConcursales()
                cmdSaveConcursal.Enabled = False

                'Se inhabilitan los campos de la pestaña Suspensión para este perfil
                DesactivarSuspension()
                taObsSuspension.Disabled = True
            Else
                'Se habilitan los campos de las pestañas Cobro persuasivo-Datos-Resumen Gestión Persuasiva para usuarios con perfil diferente a Verificador de Pagos
                cboResCobCC.Enabled = True
                cboCriteriosMC.Enabled = True
                cboEstaFinPag.Enabled = True
                cboCausalFinPers.Enabled = True
                txtAutoTerm.Enabled = True
                txtFecTerm.Enabled = True
                imgBtnBorraFecTerm.Enabled = True
                cmdSavePersuasivo.Enabled = True

                'Se habilitan los campos de las pestañas Cobro coactivo-General para usuarios con perfil diferente a Verificador de Pagos
                cmdSaveCoactivo1.Enabled = True

                'Se habilitan los campos de las pestañas Cobro coactivo-Liquidación del crédito para usuarios con perfil diferente a Verificador de Pagos
                txtLiqCredCapital.Enabled = True
                txtLiqCredInteres.Enabled = True
                txtLiqCredFecCorte.Enabled = True

                'Se habilitan los campos de las pestañas Procesos concursales-Datos para usuarios con perfil diferente a Verificador de Pagos
                cmdSaveConcursal.Enabled = True

                'Se habilitan los campos de la pestaña Suspensión para usuarios con perfil diferente a Verificador de Pagos
                cboCausalSusp.Enabled = True
                txtNroResSusp.Enabled = True
                txtFecResSusp.Enabled = True
                imgBtnBorraFecResSusp.Enabled = True
                taObsSuspension.Disabled = False
                cmdSaveSuspension.Enabled = True
            End If

            '09/abril/2015
            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim idGestorResp As String
            idGestorResp = MTG.GetIDGestorResp(Request("ID"))
            'If idGestorResp <> Session("sscodigousuario") Then
            '    If Session("mnivelacces") = 4 Or Session("mnivelacces") = 6 Or Session("mnivelacces") = 7 Then
            '        msg = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
            '        Exit Sub
            '        'CustomValidator1.IsValid = False
            '        'BloquearTotal(msg)
            '    End If
            'End If

            '18/02/2014
            'Esto solo va a operar para ventanas que dependan de EditEjefisGlobal.aspx
            Session("EstadoProcesoActual") = ""

            'Boton para regresar al escritorio del repartidor
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.REPARTIDOR) Then
                ABackRep.Visible = True
                ABack.Visible = False
            Else
                ABackRep.Visible = False
                ABack.Visible = True
            End If

            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)

            'Cargar valores de los combos            
            'LoadcboResConCC()
            LoadcboResCobCC()
            LoadcboCriteriosMC()

            LoadcboEstaFinPag()
            Loadcboextincion()
            LoadcboAlcance()
            'LoadcboEstTrans()
            LoadcboPromotor()
            LoadcboTipoProcCon()
            LoadcboEstadoTrasConcur()
            LoadcboDecreto()
            '
            LoadcboCausalSusp()

            'Combos pestaña coactivo            
            LoadcboTipoLevMC()

            LoadcboCausalFinPro()
            LoadcboCausalFinPers()
            LoadcboEstadoPagCoac()
            LoadcboEstTransCoac()

            'Cargar ficha del reparto inicial
            LoadRepartoInicial()

            'Cargar grid de cambios de estado
            BindGridCambioEstado()

            'Conexion a la base de datos
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit            
            If Len(Request("ID")) > 0 Then
                'Dim sql As String = "SELECT * FROM EJEFISGLOBAL WHERE EFINROEXP = @EFINROEXP"
                Dim sql As String = "SELECT EJEFISGLOBAL.*, ESTADOS_PROCESO.nombre AS NomEstadoProc, ISNULL (DATEDIFF(DAY, EFIFECENTGES, GETDATE()), 0) As ObliFechaPresentacion " &
                                    "FROM EJEFISGLOBAL " &
                                    "LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " &
                                    "WHERE EJEFISGLOBAL.EFINROEXP = @EFINROEXP"
                '              
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@EFINROEXP", Request("ID"))
                Dim Reader As SqlDataReader = Command.ExecuteReader

                'Si se encontro el expediente
                If Reader.Read Then
                    'Mostrar informacion general del expediente
                    txtNroExp.Text = Reader("EFINROEXP").ToString()
                    txtNRODocumentic.Text = Reader("EFIEXPDOCUMENTIC").ToString()

                    txtNombreEstado.Text = Reader("NomEstadoProc").ToString().Trim
                    txtFECENTREGAGESTOR.Text = Left(Reader("EFIFECENTGES").ToString(), 10)

                    intObliFechaPresentacion = CDec(Reader("ObliFechaPresentacion").ToString)

                    Dim cstype As Type = Me.GetType()
                    Dim strScript As String = IIf(intObliFechaPresentacion > intDiasPresentacionConcursales, "ShowHideRequired(true);", "ShowHideRequired(false);")
                    cs.RegisterStartupScript(Page.GetType(), "Script", strScript, True)

                    'Mostrar datos del Titulo Ejecutivo
                    MostrarTitulo(Request("ID").Trim)

                    'Mostrar datos de la ficha "Recepcion Titulo"
                    txtEFIFECHAEXP.Text = Left(Reader("EFIFECHAEXP").ToString().Trim, 10)
                    txtEFINUMMEMO.Text = Reader("EFINUMMEMO").ToString()
                    txtEFIEXPORIGEN.Text = Reader("EFIEXPORIGEN").ToString()
                    txtEFIFECCAD.Text = Left(Reader("EFIFECCAD").ToString().Trim, 10)
                    txtEFIEXPDOCUMENTIC.Text = Reader("EFIEXPDOCUMENTIC").ToString()

                    '18/02/2014
                    'Esto solo va a operar para ventanas que dependan de EditEjefisGlobal.aspx
                    Session("EstadoProcesoActual") = Reader("EFIESTADO").ToString().Trim

                    'Controlar la edicion de expedientes
                    HabilitarEdicionExpedientes(Reader("NomEstadoProc").ToString().Trim)

                    'Mostrar informacion del deudor asociado al expediente                    
                    MostrarDeudor(Reader("EFINROEXP").ToString().Trim)

                    'Mostrar informacion del titulo asociado al expediente
                    'cboMT_nro_titulo.SelectedValue = Reader("EFINROTITULO").ToString().Trim
                    'MostrarTitulo(Reader("EFINROTITULO").ToString().Trim)

                    'Mostrar informacion de la deuda (titulos acumulados x expediente)
                    'MostrarDeuda(Request("ID"))

                    'Mostrar informacion del persuasivo
                    MostrarPersuasivo(Request("ID"))

                    'Mostrar informacion coactivo
                    MostrarCoactivo(Request("ID"))

                    'Mostrar suspension
                    MostrarSuspension(Request("ID"))

                    'Mostrar informacion concursales
                    MostrarConcursales(Request("ID"))

                    'Mostrar informacion de facilidades de pago
                    MostrarFacilidades(Request("ID"))

                    'Mostrar la ficha del EA = Estado Actual
                    If lbltipoTotulo.Text = "08" Then
                        'Veneno
                        MostrarEA_LiquidacionSancion(Request("ID"))
                    Else
                        'Veneno
                        MostrarEA(Request("ID"))
                        MostrarSancion(False)
                    End If

                    'Mostrar el Gestor responsable
                    MostrarGestorResp(Request("ID"))

                    '17/jul/2014. Si hay excepciones => Ocultar panel de Resolución que ordena continuar ejecución 
                    'Dim NumExcepciones As Integer = 0
                    'NumExcepciones = ContarExcepciones(Request("ID").Trim)
                    'If NumExcepciones > 0 Then
                    '    OcultarResolContinuaEjecucion()
                    'End If

                End If
                ' 
                'Cerrar el Data Reader
                Reader.Close()
                'Cerrar conexion
                Connection.Close()
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente            
            IdEstadoExp = MTG.GetEstadoExpediente(Request("ID"))

            'Se obtiene el gestor responsable de la tabla TAREA_ASIGNADA
            Dim _tareaAsignadaBLL As New TareaAsignadaBLL
            Dim _tarea = _tareaAsignadaBLL.obtenerTareaAsignadaPorIdExpediente(Request("ID"))
            Dim _usuarioBll As New UsuariosBLL
            Dim _usarioResp = _usuarioBll.obtenerUsuarioPorLogin(_tarea.VAL_USUARIO_NOMBRE)
            'idGestorResp = MTG.GetIDGestorResp(Request("ID"))
            idGestorResp = _usarioResp.codigo

            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                msg = "Los expedientes en estado " & NomEstadoProceso & " no permiten guardar datos"
                BloquearTotal(msg)

                '06/01/2015. Permitir edición del combo cboCausalFinPers al gestor que posee el expediente
                If idGestorResp = Session("sscodigousuario") Then
                    cboCausalFinPers.Enabled = True
                    cmdSavePersuasivo.Visible = True
                    cmdSavePersuasivo.Enabled = True

                End If

            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    msg = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    'CustomValidator1.IsValid = False
                    BloquearTotal(msg)
                End If

            End If

            'Si el proceso esta en persuasivo => Desactivar coactivo y concursales
            If IdEstadoExp = "06" Then
                If Session("mnivelacces") <> 8 Then
                    cmdSaveCoactivo1.Visible = False
                    cmdSaveConcursal.Visible = False
                    DesactivarCoactivo()
                    DesactivarConcursales()

                    'Mensaje en pestaña de coactivo
                    CustomValidator2.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar información de coactivo"
                    CustomValidator2.IsValid = False

                    'Mensaje en pestaña de concursales
                    CustomValidator3.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar información de concursales"
                    CustomValidator3.IsValid = False
                End If

            End If

            'Si el proceso esta en coactivo => Desactivar persuasivo y concursales
            If IdEstadoExp = "02" Then
                If Session("mnivelacces") <> 8 Then
                    cmdSavePersuasivo.Visible = False
                    cmdSaveConcursal.Visible = False
                    DesactivarPersuasivo()
                    DesactivarConcursales()

                    'Mensaje en pestaña de persuasivo
                    CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar información de persuasivo"
                    CustomValidator1.IsValid = False

                    'Mensaje en pestaña de concursales
                    CustomValidator3.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar información de concursales"
                    CustomValidator3.IsValid = False
                End If

            End If

            'Si el proceso esta en concursal => Desactivar persuasivo y coactivo
            If IdEstadoExp = "03" Then
                If Session("mnivelacces") <> 8 Then
                    cmdSavePersuasivo.Visible = False
                    cmdSaveCoactivo1.Visible = False
                    DesactivarPersuasivo()
                    DesactivarCoactivo()
                    '
                    DesactivarSuspension()

                    'Mensaje en pestaña de persuasivo
                    CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar información de persuasivo"
                    CustomValidator1.IsValid = False

                    'Mensaje en pestaña de coactivo
                    CustomValidator2.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar información de coactivo"
                    CustomValidator2.IsValid = False
                End If

            End If

            ''03/02/2016. La politica persuasiva solo puede ser EDITADA por el revisor
            'If txtPoliticaPersuasivo.Text <> "" And txtFecIniCC.Text <> "" Then
            '    ' Los dos campos para calcular la fecha de terminación tienen datos
            '    ' => Impedir edicion para gestor, habilitar para revisor y gestor de información
            '    If Not (Session("mnivelacces") = 8 Or Session("mnivelacces") = 3) Then
            '        txtPoliticaPersuasivo.Enabled = False
            '        txtFecIniCC.Enabled = False
            '        'imgBtnBorraFecIniCC.Visible = False
            '    End If
            'End If

            ' Controles q siempre van a estar deshabilitados
            txtResVinDeudorSol.Enabled = False
            txtFecVinDeudorSol.Enabled = False
            '
            txtNroResolEjec.Enabled = False
            txtFecResolEjec.Enabled = False
            '
            txtNroResLiquiCred.Enabled = False
            txtFecResLiquiCred.Enabled = False
            '
            txtNroResFinPro.Enabled = False
            txtFecResFinPro.Enabled = False
            '            
            '
            txtNroResApLiq.Enabled = False
            txtFecResApLiq.Enabled = False
            '
            txtLiqCredTotal.Enabled = False

            'Estado actual de la deuda
            txtEstadoActualDeuda.Text = DeterminarEstadoActualDeuda(txtPagosCapitalEA.Text, txtCapitalInicial.Text)

            If Len(Request("ID")) > 0 Then
                'El botón de solicitar suspensión solo aparece si el estado operativo es en gestion
                'Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                Dim _tareaAsignada = _tareaAsignadaBLL.obtenerTareaAsignadaPorIdExpediente(Request("ID"))
                If _tareaAsignada.COD_ESTADO_OPERATIVO = 15 Then
                    lineaBotonPriorizar.Visible = True
                End If
            End If

            AsignarGestion()

        End If
    End Sub

    Private Sub OcultarResolContinuaEjecucion()
        'Resolución que ordena continuar ejecución 
        txtNroResolEjec.Visible = False
        txtFecResolEjec.Visible = False
        txtNroOfiCitaCor.Visible = False
        txtFecOfiCitaCor.Visible = False
        txtFecNotifCor.Visible = False
        txtFecPubAviso.Visible = False
        cmdSaveOrdenEjecucion.Visible = False
        '
        CustomValidatorOrdenEjecucion.Text = "Debe crear la Resolución que Ordena Continuar La Ejecución en la pestaña de excepciones."
        CustomValidatorOrdenEjecucion.IsValid = False
    End Sub

    Private Sub DesactivarSuspension()
        cboCausalSusp.Enabled = False
        txtNroResSusp.Enabled = False
        txtFecResSusp.Enabled = False
        cmdSaveSuspension.Enabled = False
        imgBtnBorraFecResSusp.Enabled = False
    End Sub

    Private Sub BloquearTotal(ByVal pMensaje As String)

        cmdSaveCoactivo1.Visible = False
        cmdSaveConcursal.Visible = False

        'Mensaje en pestaña de persuasivo
        If Session("mnivelacces") = 2 Or Session("mnivelacces") = 3 Then
            cmdSavePersuasivo.Visible = True
        Else
            CustomValidator1.Text = pMensaje
            CustomValidator1.IsValid = False
            cmdSavePersuasivo.Visible = False
        End If



        'Mensaje en pestaña de coactivo
        CustomValidator2.Text = pMensaje
        CustomValidator2.IsValid = False

        'Orden de Ejecución
        CustomValidatorOrdenEjecucion.Text = pMensaje
        CustomValidatorOrdenEjecucion.IsValid = False

        'Liquidación de crédito y costas 
        CustomValidatorLiquidacionCredito.Text = pMensaje
        CustomValidatorLiquidacionCredito.IsValid = False

        'Terminación del proceso 
        CustomValidatorTerminacionCoactivo.Text = pMensaje
        CustomValidatorTerminacionCoactivo.IsValid = False

        'Levantamiento de medidas cautelares 
        CustomValidator4.Text = pMensaje
        CustomValidator4.IsValid = False

        'Mensaje en pestaña de concursales
        CustomValidator3.Text = pMensaje
        CustomValidator3.IsValid = False

        'Desactivar persuasivo
        DesactivarPersuasivo()

        'Desactivar coactivo
        DesactivarCoactivo()

        'Desactivar concursales
        DesactivarConcursales()

        'Desactivar suspension
        DesactivarSuspension()

    End Sub

    Private Sub DesactivarConcursales()
        cboTipoProcCon.Enabled = False
        txtNroResApertura.Enabled = False
        txtFecRes.Enabled = False
        cboPromotor.Enabled = False
        txtFecadmis.Enabled = False
        txtFecFijAvisoAdm.Enabled = False
        txtFecDesfAvisoAdm.Enabled = False
        txtFecLimPresCred.Enabled = False
        txtNroOfiPres.Enabled = False
        txtFecOfiPres.Enabled = False
        txtFecPres.Enabled = False
        txtNroGuia.Enabled = False
        txtFecTrasProy.Enabled = False
        txtFecLimPreObj.Enabled = False
        txtNroOfiObj.Enabled = False
        txtFecOfiObj.Enabled = False
        txtFecPresObj.Enabled = False
        txtFecDecObj.Enabled = False
        txtNroOfiRep.Enabled = False
        txtFecRecRep.Enabled = False
        txtPorDerVoto.Enabled = False
        txtOfiDemSupInt.Enabled = False
        txtFecDemSupInt.Enabled = False
        '
        txtFecPresAcu.Enabled = False
        txtFecAudConf.Enabled = False
        txtFecTermAcu.Enabled = False
        txtNroResAcu.Enabled = False
        txtFecResAcu.Enabled = False
        txtFecIniPagAcu.Enabled = False
        txtFecFinPagAcu.Enabled = False
        '
        txtNroOfiDenInc.Enabled = False
        txtFecOfiIncump.Enabled = False
        txtFecPresOfiInc.Enabled = False
        txtFecAudInc.Enabled = False
        txtDecisionInc.Enabled = False
        txtFecPresDemSSInc.Enabled = False
        '
        txtFecFinConCursal.Enabled = False
        txtObservacConcursal.Enabled = False
        cboEstadoTrasConcur.Enabled = False
        txtFecTrasConcursal.Enabled = False
        txtFecInsCamCom.Enabled = False
        txtFecFinCobConcursal.Enabled = False

        '10/sep/2014
        'Botones para borrar fechas en la seccion de concursales
        imgBtnBorraFecRes.Visible = False
        imgBtnBorraFecadmis.Visible = False
        imgBtnBorraFecFijAvisoAdm.Visible = False
        imgBtnBorraFecDesfAvisoAdm.Visible = False
        imgBtnBorraFecLimPresCred.Visible = False
        imgBtnBorraFecOfiPres.Visible = False
        imgBtnBorraFecPres.Visible = False
        imgBtnBorraFecTrasProy.Visible = False
        imgBtnBorraFecLimPreObj.Visible = False
        imgBtnBorraFecOfiObj.Visible = False
        imgBtnBorraFecPresObj.Visible = False
        imgBtnBorraFecDecObj.Visible = False
        imgBtnBorraFecRecRep.Visible = False
        imgBtnBorraFecDemSupInt.Visible = False
        imgBtnBorraFecPresAcu.Visible = False
        imgBtnBorraFecAudConf.Visible = False
        imgBtnBorraFecTermAcu.Visible = False
        imgBtnBorraFecResAcu.Visible = False
        imgBtnBorraFecIniPagAcu.Visible = False
        imgBtnBorraFecFinPagAcu.Visible = False
        imgBtnBorraFecOfiIncump.Visible = False
        imgBtnBorraFecPresOfiInc.Visible = False
        imgBtnBorraFecAudInc.Visible = False
        imgBtnBorraFecPresDemSSInc.Visible = False
        imgBtnBorraFecFinConCursal.Visible = False
        '
        imgBtnBorraFecInsCamCom.Visible = False
        imgBtnBorraFecTrasConcursal.Visible = False
        imgBtnBorraFecFinCobConcursal.Visible = False
    End Sub

    Private Sub DesactivarCoactivo()
        txtFecEstiFinCoac.Enabled = False
        txtResAvocaCon.Enabled = False
        txtFecResAvoca.Enabled = False
        txtFecNotAvoca.Enabled = False
        txtResVinDeudorSol.Enabled = False
        txtFecVinDeudorSol.Enabled = False
        'txtDeudoresVinc.Enabled = False        
        '
        txtNroResolEjec.Enabled = False
        txtFecResolEjec.Enabled = False
        txtNroOfiCitaCor.Enabled = False
        txtFecOfiCitaCor.Enabled = False
        txtFecNotifCor.Enabled = False
        txtFecPubAviso.Enabled = False
        '
        txtNroResLiquiCred.Enabled = False
        txtFecResLiquiCred.Enabled = False
        txtNroOfiCitCorLC.Enabled = False
        txtFecOfiCitCorLC.Enabled = False
        txtFecNotCorLC.Enabled = False
        txtFecPubAvisoLC.Enabled = False
        '
        txtFecRadObj.Enabled = False
        txtNroRad.Enabled = False
        txtNroResApLiq.Enabled = False
        txtFecResApLiq.Enabled = False
        txtNroOfiCorLD.Enabled = False
        txtFecOfiCorLD.Enabled = False
        txtFecNotLD.Enabled = False
        txtFecPubLD.Enabled = False
        '
        'txtTipoBienSec.Enabled = False

        '
        txtAdjudicat.Enabled = False
        txtNroResAdj.Enabled = False
        txtFecResAdj.Enabled = False
        '

        '
        CboExtincion.Enabled = False
        CboDecreto.Enabled = False
        cboAlcance.Enabled = False
        txtPerIniExtin.Enabled = False
        txtPerFinExtin.Enabled = False
        txtValExtin.Enabled = False
        txtNroResExtin.Enabled = False
        txtFecResExtin.Enabled = False
        '
        txtNroResLevMC.Enabled = False
        txtFecResLevMC.Enabled = False
        cboTipoLevMC.Enabled = False
        '
        cboCausalFinPro.Enabled = False
        txtNroResFinPro.Enabled = False
        txtFecResFinPro.Enabled = False
        '
        txtObsReselGes.Enabled = False
        cboEstadoPagCoac.Enabled = False
        cboEstadoTras.Enabled = False
        txtFecTras.Enabled = False
        '
        cmdSaveOrdenEjecucion.Visible = False
        cmdSaveLiquidacionCredito.Visible = False
        cmdSaveTerminacionCoactivo.Visible = False
        cmdSaveLevantamiento.Visible = False

    End Sub

    Private Sub DesactivarPersuasivo()
        'Desactivar todos los controles
        'txtFecEstiFin.Enabled = False

        If Session("mnivelacces") = 2 Or Session("mnivelacces") = 3 Then
            txtFecEnvioCC.Enabled = True
            txtFecIniCC.Enabled = True
            txtFecFinCC.Enabled = True
            txtPoliticaPersuasivo.Enabled = True
        Else
            txtFecEnvioCC.Enabled = False
            txtFecIniCC.Enabled = False
            txtFecFinCC.Enabled = False
            txtPoliticaPersuasivo.Enabled = False
        End If

        'cboResConCC.Enabled = False
        cboResCobCC.Enabled = False
        cboCriteriosMC.Enabled = False
        'txtTelFijo.Enabled = False
        'txtTelMovil.Enabled = False
        'txtDirCorresp.Enabled = False
        'txtEmail.Enabled = False

        'txtCausalTerm.Enabled = False
        cboCausalFinPers.Enabled = False

        txtAutoTerm.Enabled = False
        txtFecTerm.Enabled = False
        cboEstaFinPag.Enabled = False

        'cboEstTrans.Enabled = False
    End Sub

    'Control de cambios
    Private Sub HabilitarEdicionExpedientes(ByVal pNomEstado As String)

        'Variable publica para que sea accedida por las otras paginas
        NomEstadoProceso = pNomEstado

        Select Case pNomEstado
            Case "PERSUASIVO"
                'Si el expediente se encuentra en persuasivo desactivar coactivo y demas estados
                If Session("mnivelacces") <> 8 Then
                    cmdSavePersuasivo.Enabled = True
                    cmdSaveCoactivo1.Enabled = False
                End If

            Case "COACTIVO"
                If Session("mnivelacces") <> 8 Then
                    cmdSavePersuasivo.Enabled = False
                    cmdSaveCoactivo1.Enabled = True
                End If

        End Select

        '' Si el estado no es persuasivo => deshabilitar boton de guardar de esa tab
        'If pNomEstado = "PERSUASIVO" Then
        '    cmdSavePersuasivo.Enabled = True
        'Else
        '    cmdSavePersuasivo.Enabled = False
        'End If
    End Sub

    Private Sub LoadcboEstadoTrasConcur()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from ESTADOS_PROCESO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEP As New DataTable
        Adaptador.Fill(dtEP)
        If dtEP.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEP As DataRow = dtEP.NewRow()
            filaEP("codigo") = ""
            filaEP("nombre") = ""
            dtEP.Rows.Add(filaEP)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaEP As DataView = New DataView(dtEP)
            vistaEP.Sort = "codigo"
            '--------------------------------------------------------------------

            cboEstadoTrasConcur.DataSource = vistaEP
            cboEstadoTrasConcur.DataTextField = "nombre"
            cboEstadoTrasConcur.DataValueField = "codigo"
            cboEstadoTrasConcur.DataBind()
        End If
    End Sub

    Private Sub LoadcboTipoProcCon()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from TIPOS_PROCESOS_CONCURSALES order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTPC As New DataTable
        Adaptador.Fill(dtTPC)
        If dtTPC.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTPC As DataRow = dtTPC.NewRow()
            filaTPC("codigo") = 0
            filaTPC("nombre") = ""
            dtTPC.Rows.Add(filaTPC)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTPC As DataView = New DataView(dtTPC)
            vistaTPC.Sort = "codigo"
            '--------------------------------------------------------------------

            cboTipoProcCon.DataSource = vistaTPC
            cboTipoProcCon.DataTextField = "nombre"
            cboTipoProcCon.DataValueField = "codigo"
            cboTipoProcCon.DataBind()
        End If
    End Sub

    Protected Sub LoadcboPromotor()

        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select cedula, nombre from PROMOTORES order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtPromotores As New DataTable
        Adaptador.Fill(dtPromotores)
        If dtPromotores.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaPromotores As DataRow = dtPromotores.NewRow()
            filaPromotores("cedula") = ""
            filaPromotores("nombre") = ""
            dtPromotores.Rows.Add(filaPromotores)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaPromotores As DataView = New DataView(dtPromotores)
            vistaPromotores.Sort = "cedula"
            '--------------------------------------------------------------------

            cboPromotor.DataSource = vistaPromotores
            cboPromotor.DataTextField = "nombre"
            cboPromotor.DataValueField = "cedula"
            cboPromotor.DataBind()
        End If
    End Sub

    'Protected Sub LoadcboResConCC()
    '    Dim cnx As String = Funciones.CadenaConexion
    '    Dim cmd As String = "select codigo, nombre from [RESCONTACTIBILIDAD] order by nombre"
    '    Dim Adaptador As New SqlDataAdapter(cmd, cnx)
    '    Dim dtRESCONTACTIBILIDAD As New DataTable
    '    Adaptador.Fill(dtRESCONTACTIBILIDAD)
    '    If dtRESCONTACTIBILIDAD.Rows.Count > 0 Then
    '        '--------------------------------------------------------------------
    '        '- Ingresar el valor en blanco (el valor queda al final)
    '        Dim filaRESCONTACTIBILIDAD As DataRow = dtRESCONTACTIBILIDAD.NewRow()
    '        filaRESCONTACTIBILIDAD("codigo") = 0
    '        filaRESCONTACTIBILIDAD("nombre") = " "
    '        dtRESCONTACTIBILIDAD.Rows.Add(filaRESCONTACTIBILIDAD)
    '        '- Crear un dataview para ordenar los valores y "00" quede de primero
    '        Dim vistaRESCONTACTIBILIDAD As DataView = New DataView(dtRESCONTACTIBILIDAD)
    '        vistaRESCONTACTIBILIDAD.Sort = "codigo"
    '        '--------------------------------------------------------------------
    '        cboResConCC.DataSource = vistaRESCONTACTIBILIDAD
    '        cboResConCC.DataTextField = "nombre"
    '        cboResConCC.DataValueField = "codigo"
    '        cboResConCC.DataBind()
    '    End If
    'End Sub

    Protected Sub LoadcboResCobCC()
        'Llenar el combo del cobro en call center
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [RESULTCOBROCALLCENTER] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtRESULTCOBROCALLCENTER As New DataTable
        Adaptador.Fill(dtRESULTCOBROCALLCENTER)
        If dtRESULTCOBROCALLCENTER.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaRESULTCOBROCALLCENTER As DataRow = dtRESULTCOBROCALLCENTER.NewRow()
            filaRESULTCOBROCALLCENTER("codigo") = 0
            filaRESULTCOBROCALLCENTER("nombre") = " "
            dtRESULTCOBROCALLCENTER.Rows.Add(filaRESULTCOBROCALLCENTER)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaRESULTCOBROCALLCENTER As DataView = New DataView(dtRESULTCOBROCALLCENTER)
            vistaRESULTCOBROCALLCENTER.Sort = "codigo"
            '--------------------------------------------------------------------
            cboResCobCC.DataSource = vistaRESULTCOBROCALLCENTER
            cboResCobCC.DataTextField = "nombre"
            cboResCobCC.DataValueField = "codigo"
            cboResCobCC.DataBind()
        End If
    End Sub

    Protected Sub LoadcboCriteriosMC()
        'Llenar el combo de criterios para ordenar medidas cautelares
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [CRITERIOS_MEDIDAS_CAUTELARES] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtRESULTCOBROCALLCENTER As New DataTable
        Adaptador.Fill(dtRESULTCOBROCALLCENTER)
        If dtRESULTCOBROCALLCENTER.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaRESULTCOBROCALLCENTER As DataRow = dtRESULTCOBROCALLCENTER.NewRow()
            filaRESULTCOBROCALLCENTER("codigo") = 0
            filaRESULTCOBROCALLCENTER("nombre") = " "
            dtRESULTCOBROCALLCENTER.Rows.Add(filaRESULTCOBROCALLCENTER)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaRESULTCOBROCALLCENTER As DataView = New DataView(dtRESULTCOBROCALLCENTER)
            vistaRESULTCOBROCALLCENTER.Sort = "codigo"
            '--------------------------------------------------------------------
            cboCriteriosMC.DataSource = vistaRESULTCOBROCALLCENTER
            cboCriteriosMC.DataTextField = "nombre"
            cboCriteriosMC.DataValueField = "codigo"
            cboCriteriosMC.DataBind()
        End If
    End Sub

    Protected Sub LoadcboEstaFinPag()
        'Llenar el combo de estado final de pago
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from ESTADOS_PAGO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtESTADOS_PAGO As New DataTable
        Adaptador.Fill(dtESTADOS_PAGO)
        If dtESTADOS_PAGO.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaESTADOS_PAGO As DataRow = dtESTADOS_PAGO.NewRow()
            filaESTADOS_PAGO("codigo") = 0
            filaESTADOS_PAGO("nombre") = " "
            dtESTADOS_PAGO.Rows.Add(filaESTADOS_PAGO)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaESTADOS_PAGO As DataView = New DataView(dtESTADOS_PAGO)
            vistaESTADOS_PAGO.Sort = "codigo"
            '--------------------------------------------------------------------
            cboEstaFinPag.DataSource = vistaESTADOS_PAGO
            cboEstaFinPag.DataTextField = "nombre"
            cboEstaFinPag.DataValueField = "codigo"
            cboEstaFinPag.DataBind()
        End If
    End Sub

    Private Sub LoadcboEstadoPagCoac()
        'Llenar el combo de estado final de pago en COACTIVO
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from ESTADOS_PAGO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtESTADOS_PAGO As New DataTable
        Adaptador.Fill(dtESTADOS_PAGO)
        If dtESTADOS_PAGO.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaESTADOS_PAGO As DataRow = dtESTADOS_PAGO.NewRow()
            filaESTADOS_PAGO("codigo") = 0
            filaESTADOS_PAGO("nombre") = " "
            dtESTADOS_PAGO.Rows.Add(filaESTADOS_PAGO)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaESTADOS_PAGO As DataView = New DataView(dtESTADOS_PAGO)
            vistaESTADOS_PAGO.Sort = "codigo"
            '--------------------------------------------------------------------
            cboEstadoPagCoac.DataSource = vistaESTADOS_PAGO
            cboEstadoPagCoac.DataTextField = "nombre"
            cboEstadoPagCoac.DataValueField = "codigo"
            cboEstadoPagCoac.DataBind()
        End If
    End Sub

    Private Sub LoadcboTipoLevMC()
        'Llenar el combo de tipo de levantamiento de medida cautelar
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre from TIPOS_LEVANTAMIENTO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTIPOS_LEVANTAMIENTO As New DataTable
        Adaptador.Fill(dtTIPOS_LEVANTAMIENTO)
        If dtTIPOS_LEVANTAMIENTO.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTIPOS_LEVANTAMIENTO As DataRow = dtTIPOS_LEVANTAMIENTO.NewRow()
            filaTIPOS_LEVANTAMIENTO("codigo") = 0
            filaTIPOS_LEVANTAMIENTO("nombre") = " "
            dtTIPOS_LEVANTAMIENTO.Rows.Add(filaTIPOS_LEVANTAMIENTO)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTIPOS_LEVANTAMIENTO As DataView = New DataView(dtTIPOS_LEVANTAMIENTO)
            vistaTIPOS_LEVANTAMIENTO.Sort = "codigo"
            '--------------------------------------------------------------------
            cboTipoLevMC.DataSource = vistaTIPOS_LEVANTAMIENTO
            cboTipoLevMC.DataTextField = "nombre"
            cboTipoLevMC.DataValueField = "codigo"
            cboTipoLevMC.DataBind()
        End If
    End Sub

    Private Sub LoadcboCausalFinPro()
        'Llenar el combo de causales de terminacion de proceso
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre from CAUSALES_TERMINACION_PROCESO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtCAUSALES_TERMINACION_PROCESO As New DataTable
        Adaptador.Fill(dtCAUSALES_TERMINACION_PROCESO)
        If dtCAUSALES_TERMINACION_PROCESO.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaCAUSALES_SUSPENSION_PROCESO As DataRow = dtCAUSALES_TERMINACION_PROCESO.NewRow()
            filaCAUSALES_SUSPENSION_PROCESO("codigo") = 0
            filaCAUSALES_SUSPENSION_PROCESO("nombre") = " "
            dtCAUSALES_TERMINACION_PROCESO.Rows.Add(filaCAUSALES_SUSPENSION_PROCESO)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaCAUSALES_TERMINACION_PROCESO As DataView = New DataView(dtCAUSALES_TERMINACION_PROCESO)
            vistaCAUSALES_TERMINACION_PROCESO.Sort = "codigo"
            '--------------------------------------------------------------------
            cboCausalFinPro.DataSource = vistaCAUSALES_TERMINACION_PROCESO
            cboCausalFinPro.DataTextField = "nombre"
            cboCausalFinPro.DataValueField = "codigo"
            cboCausalFinPro.DataBind()
        End If
    End Sub

    Private Sub LoadcboCausalFinPers()
        'Llenar el combo de causales de terminacion de proceso
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre from CAUSALES_TERMINACION_PROCESO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtCAUSALES_TERMINACION_PROCESO As New DataTable
        Adaptador.Fill(dtCAUSALES_TERMINACION_PROCESO)
        If dtCAUSALES_TERMINACION_PROCESO.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaCAUSALES_TERMINACION_PROCESO As DataRow = dtCAUSALES_TERMINACION_PROCESO.NewRow()
            filaCAUSALES_TERMINACION_PROCESO("codigo") = 0
            filaCAUSALES_TERMINACION_PROCESO("nombre") = " "
            dtCAUSALES_TERMINACION_PROCESO.Rows.Add(filaCAUSALES_TERMINACION_PROCESO)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaCAUSALES_TERMINACION_PROCESO As DataView = New DataView(dtCAUSALES_TERMINACION_PROCESO)
            vistaCAUSALES_TERMINACION_PROCESO.Sort = "codigo"
            '--------------------------------------------------------------------
            cboCausalFinPers.DataSource = vistaCAUSALES_TERMINACION_PROCESO
            cboCausalFinPers.DataTextField = "nombre"
            cboCausalFinPers.DataValueField = "codigo"
            cboCausalFinPers.DataBind()
        End If
    End Sub

    Private Sub LoadcboEstTransCoac()
        'Llenar el combo del estado al que se translada en COACTIVO
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from ESTADOS_PROCESO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtESTADOS_PROCESO As New DataTable
        Adaptador.Fill(dtESTADOS_PROCESO)
        If dtESTADOS_PROCESO.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaESTADOS_PROCESO As DataRow = dtESTADOS_PROCESO.NewRow()
            filaESTADOS_PROCESO("codigo") = "00"
            filaESTADOS_PROCESO("nombre") = " "
            dtESTADOS_PROCESO.Rows.Add(filaESTADOS_PROCESO)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaESTADOS_PROCESO As DataView = New DataView(dtESTADOS_PROCESO)
            vistaESTADOS_PROCESO.Sort = "codigo"
            '--------------------------------------------------------------------
            cboEstadoTras.DataSource = vistaESTADOS_PROCESO
            cboEstadoTras.DataTextField = "nombre"
            cboEstadoTras.DataValueField = "codigo"
            cboEstadoTras.DataBind()
        End If
    End Sub

    'Protected Sub LoadcboEstTrans()
    '    'Llenar el combo del estado al que se translada en PERSUASIVO
    '    Dim cnx As String = Funciones.CadenaConexion
    '    Dim cmd As String = "select codigo, nombre from ESTADOS_PROCESO order by nombre"
    '    Dim Adaptador As New SqlDataAdapter(cmd, cnx)
    '    Dim dtESTADOS_PROCESO As New DataTable
    '    Adaptador.Fill(dtESTADOS_PROCESO)
    '    If dtESTADOS_PROCESO.Rows.Count > 0 Then
    '        '--------------------------------------------------------------------
    '        '- Ingresar el valor en blanco (el valor queda al final)
    '        Dim filaESTADOS_PROCESO As DataRow = dtESTADOS_PROCESO.NewRow()
    '        filaESTADOS_PROCESO("codigo") = "00"
    '        filaESTADOS_PROCESO("nombre") = " "
    '        dtESTADOS_PROCESO.Rows.Add(filaESTADOS_PROCESO)
    '        '- Crear un dataview para ordenar los valores y "00" quede de primero
    '        Dim vistaESTADOS_PROCESO As DataView = New DataView(dtESTADOS_PROCESO)
    '        vistaESTADOS_PROCESO.Sort = "codigo"
    '        '--------------------------------------------------------------------
    '        cboEstTrans.DataSource = vistaESTADOS_PROCESO
    '        cboEstTrans.DataTextField = "nombre"
    '        cboEstTrans.DataValueField = "codigo"
    '        cboEstTrans.DataBind()
    '    End If
    'End Sub

    Private Sub LoadRepartoInicial()
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT CE.idunico, CE.NroExp, UR.nombre AS NomRepartidor, UA.nombre AS NomAbogado," &
           "CE.fecha, EP.nombre AS NomEstado, EPG.nombre AS NomEstadoPago " &
              "FROM CAMBIOS_ESTADO CE " &
              "LEFT JOIN USUARIOS UR ON CE.repartidor = UR.codigo " &
              "LEFT JOIN USUARIOS UA ON CE.abogado = UA.codigo " &
              "LEFT JOIN ESTADOS_PROCESO EP ON CE.estado = EP.codigo " &
              "LEFT JOIN ESTADOS_PAGO EPG ON CE.estadopago = EPG.codigo " &
              "WHERE CE.NroExp = '" & Request("ID") & "' AND " &
              "CE.idunico = (SELECT MIN(idunico) AS IdIni FROM CAMBIOS_ESTADO WHERE NroExp = '" & Request("ID") & "')"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        If Reader.Read Then
            txtrepartidor.Text = Reader("NomRepartidor").ToString()
            txtabogado.Text = Reader("NomAbogado").ToString()
            txtfecha.Text = Left(Reader("fecha").ToString(), 10)
            txtcboestado.Text = Reader("NomEstado").ToString()
            txtestadopago.Text = Reader("NomEstadoPago").ToString()
        End If
        Reader.Close()
        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Sub LoadcboCausalSusp()
        'Llenar el combo de causales de terminacion de proceso
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre from CAUSALES_SUSPENSION_PROCESO order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtCAUSALES_SUSPENSION_PROCESO As New DataTable
        Adaptador.Fill(dtCAUSALES_SUSPENSION_PROCESO)
        If dtCAUSALES_SUSPENSION_PROCESO.Rows.Count > 0 Then
            '------------------------------------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaCAUSALES_SUSPENSION_PROCESO As DataRow = dtCAUSALES_SUSPENSION_PROCESO.NewRow()
            filaCAUSALES_SUSPENSION_PROCESO("codigo") = 0
            filaCAUSALES_SUSPENSION_PROCESO("nombre") = " "
            dtCAUSALES_SUSPENSION_PROCESO.Rows.Add(filaCAUSALES_SUSPENSION_PROCESO)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaCAUSALES_SUSPENSION_PROCESO As DataView = New DataView(dtCAUSALES_SUSPENSION_PROCESO)
            vistaCAUSALES_SUSPENSION_PROCESO.Sort = "codigo"
            '------------------------------------------------------------------------------------------------
            cboCausalSusp.DataSource = vistaCAUSALES_SUSPENSION_PROCESO
            cboCausalSusp.DataTextField = "nombre"
            cboCausalSusp.DataValueField = "codigo"
            cboCausalSusp.DataBind()
        End If
    End Sub

    Private Sub MostrarGestorResp(ByVal pExpediente As String)
        If pExpediente <> "" Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT nombre FROM USUARIOS WHERE codigo = (SELECT EFIUSUASIG FROM EJEFISGLOBAL WHERE EFINROEXP = @EFINROEXP)"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@EFINROEXP", pExpediente)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            'Si se encontro el expediente
            If Reader.Read Then
                txtGestorResp.Text = Reader("nombre").ToString()
            End If
            Reader.Close()
            Connection.Close()
        End If
    End Sub

    Private Sub MostrarDeudor(ByVal pIdDeudor As String)
        If pIdDeudor <> "" Then
            Dim cnx As String = Funciones.CadenaConexion

            Dim cmd As String = "SELECT TOP 1 deudor, ED.ED_Nombre " &
                                "FROM DEUDORES_EXPEDIENTES DxE " &
                                "LEFT JOIN ENTES_DEUDORES ED ON DxE.deudor = ED.ED_Codigo_Nit " &
                                "WHERE DxE.NroExp = '" & pIdDeudor.Trim & "' ORDER BY tipo"

            Dim Adaptador As New SqlDataAdapter(cmd, cnx)
            Dim dtDeudores As New DataTable
            Adaptador.Fill(dtDeudores)
            If dtDeudores.Rows.Count > 0 Then
                'Mostrar datos                                      
                'Datos del panel superior
                txtIdDeudor.Text = dtDeudores.Rows(0).Item("deudor").ToString
                txtNombreDeudor.Text = dtDeudores.Rows(0).Item("ED_Nombre").ToString
            End If
        End If
    End Sub

    Private Sub MostrarEA_LiquidacionSancion(ByVal pNumExpediente As String)
        If pNumExpediente <> "" Then
            Dim Ipcsinpagos As String = "0"
            Dim ipc As String = "0"
            'Conexion a la base de datos
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            'Consultar el total de la deuda
            Dim sql0 As String = "select EFIIPC from ejefisglobal where EFINROEXP = '" & pNumExpediente.Trim & "'"
            Dim Command0 As New SqlCommand(sql0, Connection)
            Dim Reader0 As SqlDataReader = Command0.ExecuteReader
            If Reader0.Read Then
                'Dim n As Double = Convert.ToDouble(Reader("totaldeuda").ToString())
                'txtTotalDeudaEA.Text = n.ToString("N0")                
                Ipcsinpagos = Convert.ToDouble(IIf(Reader0("EFIIPC").ToString() = "", "0", Reader0("EFIIPC").ToString()))
                txtValorIPC.Text = Convert.ToDouble(IIf(Reader0("EFIIPC").ToString() = "", "0", Reader0("EFIIPC").ToString())).ToString("N0")
            Else
                Ipcsinpagos = "0"
            End If
            Reader0.Close()

            '/-----------------------------------------------------------------  
            'ID _HU:  012
            'Nombre HU: Desagregación de Obligaciones  
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 16-06-2017  
            'Objetivo : Como el valor del título se desagrego, ahora se debe
            ' consultar los campos, MT_total_obligacion y MT_total_partida_global. 
            '------------------------------------------------------------------/

            'Consultar el total de la deuda
            ' Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda,ISNULL(sum(mt_totalSancion),0) as totalsancion,ISNULL(sum(valorRevoca),0) valorRevoca " & _
            '                             "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pNumExpediente.Trim & "' GROUP BY MT_expediente"

            Dim sql As String = " SELECT ISNULL(SUM(totaldeuda),0) As totalDeuda," +
                                "        ISNULL(SUM(MT_totalSancion),0) As totalSancion, " +
                                "        ISNULL(SUM(MT_total_obligacion), 0 ) As totalObligacion," +
                                "        ISNULL(SUM(MT_total_partida_global), 0) as totalPartidaGlobal, " +
                                "        ISNULL(SUM(valorRevoca),0) As valorRevoca, " +
                                "        ISNULL(SUM(MT_total_obligacion),0 ) + ISNULL(SUM(MT_total_partida_global),0) as capitalInicial " +
                                " FROM  MAESTRO_TITULOS " +
                                " WHERE MT_expediente = '" & pNumExpediente.Trim & "'" +
                                " GROUP BY MT_expediente "

            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'Dim n As Double = Convert.ToDouble(Reader("totaldeuda").ToString())
                'txtTotalDeudaEA.Text = n.ToString("N0")   
                txtTotalDeudaEA.Text = Convert.ToDouble(Reader("totaldeuda").ToString()).ToString("N0")
                txtCapitalInicial.Text = Convert.ToDouble(Reader("capitalInicial").ToString()).ToString("N0")
                txtTotalDeudaSancion.Text = Convert.ToDouble(Reader("totalsancion").ToString()).ToString("N0")
                txtValorRevocatoria.Text = Convert.ToDouble(Reader("valorRevoca").ToString()).ToString("N0")
            Else
                txtTotalDeudaEA.Text = "0"
                txtCapitalInicial.Text = "0"
                txtTotalDeudaSancion.Text = "0"
                txtValorRevocatoria.Text = "0"
            End If
            Reader.Close()

            'Pagos de la liquidacion oficial
            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
            ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. 
            Dim sql2 As String = "Select SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) As pagcapital, SUM(COALESCE(paginteres,0)) pagoInteres " &
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' and pagLiqSan = 0 AND SnContabilizar = 1  GROUP BY NroExp"

            Dim Command2 As New SqlCommand(sql2, Connection)
            Dim Reader2 As SqlDataReader = Command2.ExecuteReader
            If Reader2.Read Then
                If Reader2("pagcapital").ToString() = "" Then
                    txtPagosCapitalEA.Text = "0"
                Else
                    txtPagosCapitalEA.Text = Convert.ToDouble(Reader2("pagcapital").ToString()).ToString("N0")
                End If

                If Reader2("pagoInteres").ToString() = "" Then
                    txtpagInteresLiq.Text = "0"
                Else
                    txtpagInteresLiq.Text = Convert.ToDouble(Reader2("pagoInteres").ToString()).ToString("N0")
                End If

                'txttotalpagLiq.Text = Convert.ToDouble(CDbl(txtPagosCapitalEA.Text) + CDbl(txtpagInteresLiq.Text)).ToString("N0")
            Else
                txtpagInteresLiq.Text = "0"
                txtPagosCapitalEA.Text = "0"
                txttotalpagLiq.Text = "0"
            End If
            Reader2.Close()

            'Pagos de la Sancion
            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
            ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. 
            Dim sql3 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital, sum(vlripc) AS vlripc,MAX(pagFecha) pagFecha, SUM(COALESCE(paginteres,0)) pagoInteres " &
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' and pagLiqSan = 1 AND SnContabilizar = 1 GROUP BY NroExp"

            Dim Command3 As New SqlCommand(sql3, Connection)
            Dim Reader3 As SqlDataReader = Command3.ExecuteReader
            If Reader3.Read Then
                txtUltPag.Text = Left(Reader3("pagFecha").ToString(), 10)
                If Reader3("pagcapital").ToString() = "" Then
                    txtPagosCapitalSancion.Text = "0"
                Else
                    txtPagosCapitalSancion.Text = Convert.ToDouble(Reader3("pagcapital").ToString()).ToString("N0")
                End If

                If Reader3("vlripc").ToString() = "" Then
                    ipc = "0"
                Else
                    ipc = Convert.ToDouble(Reader3("vlripc").ToString())
                End If

                txttotalpagLiq.Text = Convert.ToDouble(CDbl(txtPagosCapitalEA.Text) + CDbl(txtTotalDeudaSancion.Text) + CDbl(txtpagInteresLiq.Text) + CDbl(Reader3("pagoInteres").ToString())).ToString("N0")
            Else
                txtPagosCapitalSancion.Text = "0"
                ipc = "0"
                txtUltPag.Text = "Sin pagos"
            End If
            Reader3.Close()


            'txtValorIPC.Text = CDbl(CDbl(Ipcsinpagos) + CDbl(ipc)).ToString("N0")
            'Mostrar la diferencia entre Total deuda - Capital pagado
            '/-----------------------------------------------------------------  
            'ID _HU:  016 
            'Nombre HU : Error en Saldo Actual en los Expedientes de Cobro 
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 30-03-2017  
            'Objetivo : 
            'Para obligaciones diferentes a Liquidación Oficial/Sanción y Sanciones L1607/12: 
            ' Saldo Actual = Capital Inicial – Valor Revocatoria – Pago a Capital 
            'Pago a Para obligaciones de Sanciones L1607/12
            'Para la primera actualización: Saldo Actual = Capital Inicial – Valor Revocatoria  + Valor IPC – Pago a Capital 
            '------------------------------------------------------------------/  

            txtsandoactualSancion.Text = Convert.ToDecimal(txtTotalDeudaSancion.Text) - Convert.ToDecimal(txtPagosCapitalSancion.Text) + Convert.ToDecimal(txtValorIPC.Text)
            'txtSaldoEA.Text = Convert.ToDouble(txtTotalDeudaEA.Text) - Convert.ToDouble(txtPagosCapitalEA.Text)
            txtSaldoEA.Text = Convert.ToDecimal(txtTotalDeudaEA.Text) - Convert.ToDecimal(txtValorRevocatoria.Text) - Convert.ToDecimal(txtPagosCapitalEA.Text) - Convert.ToDecimal(txtPagosCapitalSancion.Text)
            txtSaldoEA.Text = CDec(txtSaldoEA.Text).ToString("N0")
            txtsandoactualSancion.Text = CDec(txtsandoactualSancion.Text).ToString("N0")


            Connection.Close()
        End If

    End Sub

    Private Sub MostrarEA(ByVal pNumExpediente As String)
        If pNumExpediente <> "" Then
            Dim Ipcsinpagos As String = "0"
            Dim ipc As String = "0"
            'Conexion a la base de datos
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            'Consultar el total de la deuda
            Dim sql0 As String = "select EFIIPC from ejefisglobal where EFINROEXP = '" & pNumExpediente.Trim & "'"
            Dim Command0 As New SqlCommand(sql0, Connection)
            Dim Reader0 As SqlDataReader = Command0.ExecuteReader
            If Reader0.Read Then
                'Dim n As Double = Convert.ToDouble(Reader("totaldeuda").ToString())
                'txtTotalDeudaEA.Text = n.ToString("N0")                
                Ipcsinpagos = Convert.ToDouble(IIf(Reader0("EFIIPC").ToString() = "", "0", Reader0("EFIIPC").ToString()))
                txtValorIPC.Text = Convert.ToDouble(IIf(Reader0("EFIIPC").ToString() = "", "0", Reader0("EFIIPC").ToString())).ToString("N0")
            Else
                Ipcsinpagos = "0"
            End If
            Reader0.Close()

            '/-----------------------------------------------------------------  
            'ID _HU:  012
            'Nombre HU: Desagregación de Obligaciones  
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 16-06-2017  
            'Objetivo : Como el valor del título se desagrego, ahora se debe
            ' consultar los campos, MT_total_obligacion y MT_total_partida_global. 
            '------------------------------------------------------------------/

            'Consultar el total de la deuda
            ' Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda,ISNULL(sum(mt_totalSancion),0) as totalsancion,ISNULL(sum(valorRevoca),0) valorRevoca " & _
            '                             "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pNumExpediente.Trim & "' GROUP BY MT_expediente"

            Dim sql As String = " SELECT ISNULL(SUM(totaldeuda),0) As totalDeuda," +
                                "        ISNULL(SUM(MT_totalSancion),0) As totalSancion, " +
                                "        ISNULL(SUM(MT_total_obligacion), 0 ) As totalObligacion," +
                                "        ISNULL(SUM(MT_total_partida_global), 0) as totalPartidaGlobal, " +
                                "        ISNULL(SUM(valorRevoca),0) As valorRevoca, " +
                                "        ISNULL(SUM(MT_total_obligacion),0 ) + ISNULL(SUM(MT_total_partida_global),0) as capitalInicial " +
                                " FROM  MAESTRO_TITULOS " +
                                " WHERE MT_expediente = '" & pNumExpediente.Trim & "'" +
                                " GROUP BY MT_expediente "

            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'Dim n As Double = Convert.ToDouble(Reader("totaldeuda").ToString())
                'txtTotalDeudaEA.Text = n.ToString("N0")                
                txtTotalDeudaEA.Text = Convert.ToDouble(Reader("totaldeuda").ToString()).ToString("N0")
                txtCapitalInicial.Text = Convert.ToDouble(Reader("capitalInicial").ToString()).ToString("N0")
                txtValorRevocatoria.Text = Convert.ToDouble(Reader("valorRevoca").ToString()).ToString("N0")
            Else
                txtTotalDeudaEA.Text = "0"
            End If
            Reader.Close()

            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
            ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. 
            Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital, sum(vlripc) AS vlripc,MAX(pagFecha) pagFecha, SUM(COALESCE(paginteres,0)) pagoInteres " &
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' AND SnContabilizar = 1  GROUP BY NroExp"

            Dim Command2 As New SqlCommand(sql2, Connection)
            Dim Reader2 As SqlDataReader = Command2.ExecuteReader
            If Reader2.Read Then
                txtUltPag.Text = Left(Reader2("pagFecha").ToString(), 10)
                If Reader2("pagcapital").ToString() = "" Then
                    txtPagosCapitalEA.Text = "0"
                Else
                    txtPagosCapitalEA.Text = Convert.ToDouble(Reader2("pagcapital").ToString()).ToString("N0")
                End If

                If Reader2("vlripc").ToString() = "" Then
                    ipc = "0"
                Else
                    ipc = Convert.ToDouble(Reader2("vlripc").ToString())
                End If

                If Reader2("pagoInteres").ToString() = "" Then
                    txtpagInteresLiq.Text = "0"
                Else
                    txtpagInteresLiq.Text = Convert.ToDouble(Reader2("pagoInteres").ToString()).ToString("N0")
                End If

                txttotalpagLiq.Text = Convert.ToDouble(CDbl(txtPagosCapitalEA.Text) + CDbl(txtpagInteresLiq.Text)).ToString("N0")

            Else
                txtPagosCapitalEA.Text = "0"
                ipc = "0"
                txtUltPag.Text = "Sin pagos"
                txttotalpagLiq.Text = "0"
                txtpagInteresLiq.Text = "0"
            End If
            Reader2.Close()


            'Pagos de la Sancion
            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
            ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. 
            Dim sql3 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital, sum(vlripc) AS vlripc,MAX(pagFecha) pagFecha " &
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' and pagLiqSan = 1 AND SnContabilizar = 1 GROUP BY NroExp"

            Dim Command3 As New SqlCommand(sql3, Connection)
            Dim Reader3 As SqlDataReader = Command3.ExecuteReader
            If Reader3.Read Then
                txtUltPag.Text = Left(Reader3("pagFecha").ToString(), 10)
                If Reader3("pagcapital").ToString() = "" Then
                    txtPagosCapitalSancion.Text = "0"
                Else
                    txtPagosCapitalSancion.Text = Convert.ToDouble(Reader3("pagcapital").ToString()).ToString("N0")
                End If

                If Reader3("vlripc").ToString() = "" Then
                    ipc = "0"
                Else
                    ipc = Convert.ToDouble(Reader3("vlripc").ToString())
                End If
            Else
                txtPagosCapitalSancion.Text = "0"
                ipc = "0"
                txtUltPag.Text = "Sin pagos"
            End If
            Reader3.Close()

            'Mostrar la diferencia entre Total deuda - Capital pagado

            '/-----------------------------------------------------------------  
            'ID _HU:  016 
            'Nombre HU : Error en Saldo Actual en los Expedientes de Cobro 
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 30-03-2017  
            'Objetivo : 
            'Para obligaciones diferentes a Liquidación Oficial/Sanción y Sanciones L1607/12: 
            ' Saldo Actual = Capital Inicial – Valor Revocatoria  – 
            'Pago a Para obligaciones de Sanciones L1607/12
            'Para la primera actualización: Saldo Actual = Capital Inicial – Valor Revocatoria  + Valor IPC – Pago a Capital 
            '------------------------------------------------------------------/  
            Dim saldoEA As Decimal
            If lbltipoTotulo.Text = "07" Then
                lblInte.InnerText = "Valor IPC"
                txtpagInteresLiq.Text = txtValorIPC.Text
                'saldoEA = Convert.ToDouble(txtTotalDeudaEA.Text) - Convert.ToDouble(txtPagosCapitalEA.Text) + Convert.ToDouble(ipc) + Convert.ToDouble(Ipcsinpagos)
                saldoEA = CDec(txtTotalDeudaEA.Text) - CDec(txtValorRevocatoria.Text) - CDec(txtPagosCapitalEA.Text) + CDec(txtpagInteresLiq.Text)
            Else
                'saldoEA = Convert.ToDouble(txtTotalDeudaEA.Text) - Convert.ToDouble(txtPagosCapitalEA.Text)
                If String.IsNullOrWhiteSpace(txtPagosCapitalSancion.Text) Then
                    txtPagosCapitalSancion.Text = "0"
                End If
                saldoEA = CDec(txtTotalDeudaEA.Text) - CDec(txtValorRevocatoria.Text) - CDec(txtPagosCapitalEA.Text) - CDec(txtPagosCapitalSancion.Text)
                txtValorIPC.Text = "NO APLICA"
            End If

            txtSaldoEA.Text = saldoEA.ToString("N0")

            Connection.Close()
        End If
    End Sub

    Private Sub MostrarTitulo(ByVal pNumExpediente As String)
        If pNumExpediente <> "" Then
            'Conexion a la base de datos
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            'Consultar el total de la deuda
            Dim sql As String = "SELECT MT.MT_nro_titulo, MT.MT_fec_expedicion_titulo, MT.MT_tipo_titulo, TT.nombre AS NomTipoTitulo,MT_fec_exi_liq,MT_fecha_ejecutoria, MT_fec_cad_presc, MT_reso_resu_apela_recon, MT_fec_exp_reso_apela_recon " &
                                " FROM MAESTRO_TITULOS MT  " &
                                " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " &
                                "WHERE MT.MT_expediente = '" & pNumExpediente & "'"

            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'txtTotalDeudaEA.Text = Convert.ToDouble(Reader("totaldeuda").ToString()).ToString("N0")
                txtNUMTITULOEJECUTIVO.Text = Reader("MT_nro_titulo").ToString()
                txtTIPOTITULO.Text = Reader("NomTipoTitulo").ToString()
                lbltipoTotulo.Text = Reader("MT_tipo_titulo").ToString()
                If Reader("MT_fec_expedicion_titulo").ToString = "" Then
                    txtFECTITULO.Text = "Sin fecha"
                Else
                    txtFECTITULO.Text = Left(Reader("MT_fec_expedicion_titulo").ToString(), 10)
                End If

                If Reader("MT_fec_cad_presc").ToString = "" Then
                    txtFechaPrescripcion.Text = "Sin fecha"
                Else
                    txtFechaPrescripcion.Text = Left(Reader("MT_fec_cad_presc").ToString(), 10)
                End If


                If Reader("MT_fec_exi_liq").ToString = "" Then
                    txtFechaExigTitulo.Text = "Sin fecha"
                Else
                    txtFechaExigTitulo.Text = Left(Reader("MT_fec_exi_liq").ToString(), 10)
                End If

                If Reader("MT_fecha_ejecutoria").ToString = "" Then
                    txtFechaEjecutoria.Text = "Sin fecha"
                Else
                    txtFechaEjecutoria.Text = Left(Reader("MT_fecha_ejecutoria").ToString(), 10)
                End If


                If Reader("MT_reso_resu_apela_recon").ToString = "" Then
                    txtresolucionApelacion.Text = "No tiene"
                Else
                    txtresolucionApelacion.Text = Left(Reader("MT_reso_resu_apela_recon").ToString(), 10)
                End If


                If Reader("MT_fec_exp_reso_apela_recon").ToString = "" Then
                    txtfechaapelacionreconsideracion.Text = "No tiene"
                Else
                    txtfechaapelacionreconsideracion.Text = Left(Reader("MT_fec_exp_reso_apela_recon").ToString(), 10)
                End If


            Else
                'txtTotalDeudaEA.Text = "0"

            End If
            Reader.Close()
            Connection.Close()

        End If
    End Sub

    'Private Sub MostrarDeuda(ByVal pNumExpediente As String)
    '    If pNumExpediente <> "" Then
    '        Dim cnx As String = Funciones.CadenaConexion

    '        Dim cmd As String = "SELECT  SUM(capitalmulta) AS capitalmulta,  SUM(omisossalud) AS omisossalud,  SUM(morasalud) AS morasalud,    " & _
    '                            "		SUM(inexactossalud) AS inexactossalud,  SUM(omisospensiones) AS omisospensiones,                       " & _
    '                            "		SUM(morapensiones) AS morapensiones,  SUM(inexactospensiones) AS inexactospensiones,                   " & _
    '                            "		SUM(omisosfondosolpen) AS omisosfondosolpen,  SUM(morafondosolpen) AS morafondosolpen,                 " & _
    '                            "		SUM(inexactosfondosolpen) AS inexactosfondosolpen,  SUM(omisosarl) AS omisosarl,                       " & _
    '                            "		SUM(moraarl) AS moraarl,  SUM(inexactosarl) AS inexactosarl,  SUM(omisosicbf) AS omisosicbf,           " & _
    '                            "		SUM(moraicbf) AS moraicbf,  SUM(inexactosicbf) AS inexactosicbf,  SUM(omisossena) AS omisossena,       " & _
    '                            "		SUM(morasena) AS morasena,  SUM(inexactossena) AS inexactossena,                                       " & _
    '                            "		SUM(omisossubfamiliar) AS omisossubfamiliar,  SUM(morasubfamiliar) AS morasubfamiliar,                 " & _
    '                            "		SUM(inexactossubfamiliar) AS inexactossubfamiliar,  SUM(sentenciasjudiciales) AS sentenciasjudiciales, " & _
    '                            "		SUM(cuotaspartesacum) AS cuotaspartesacum,  SUM(totalmultas) AS totalmultas,                           " & _
    '                            "		SUM(totalomisos) AS totalomisos,  SUM(totalmora) AS totalmora,  SUM(totalinexactos) AS totalinexactos, " & _
    '                            "		SUM(totalsentencias) AS totalsentencias,  SUM(totaldeuda) AS totaldeuda                                " & _
    '                            "FROM MAESTRO_TITULOS                                                                                          " & _
    '                            "WHERE mt_expediente = '" & pNumExpediente.Trim & "'"

    '        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
    '        Dim dtDeuda As New DataTable
    '        Adaptador.Fill(dtDeuda)
    '        If dtDeuda.Rows.Count > 0 Then
    '            'Mostrar datos
    '            'txtTotalDeudaEA.Text = Convert.ToDouble(Reader("totaldeuda").ToString()).ToString("N0")
    '            txtcapitalmulta.Text = dtDeuda.Rows(0).Item("capitalmulta").ToString
    '            txtomisossalud.Text = dtDeuda.Rows(0).Item("omisossalud").ToString
    '            txtmorasalud.Text = dtDeuda.Rows(0).Item("morasalud").ToString
    '            txtinexactossalud.Text = dtDeuda.Rows(0).Item("inexactossalud").ToString
    '            txtomisospensiones.Text = dtDeuda.Rows(0).Item("omisospensiones").ToString
    '            txtmorapensiones.Text = dtDeuda.Rows(0).Item("morapensiones").ToString
    '            txtinexactospensiones.Text = dtDeuda.Rows(0).Item("inexactospensiones").ToString
    '            txtomisosfondosolpen.Text = dtDeuda.Rows(0).Item("omisosfondosolpen").ToString
    '            txtmorafondosolpen.Text = dtDeuda.Rows(0).Item("morafondosolpen").ToString
    '            txtinexactosfondosolpen.Text = dtDeuda.Rows(0).Item("inexactosfondosolpen").ToString
    '            txtomisosarl.Text = dtDeuda.Rows(0).Item("omisosarl").ToString
    '            txtmoraarl.Text = dtDeuda.Rows(0).Item("moraarl").ToString
    '            txtinexactosarl.Text = dtDeuda.Rows(0).Item("inexactosarl").ToString
    '            txtomisosicbf.Text = dtDeuda.Rows(0).Item("omisosicbf").ToString
    '            txtmoraicbf.Text = dtDeuda.Rows(0).Item("moraicbf").ToString
    '            txtinexactosicbf.Text = dtDeuda.Rows(0).Item("inexactosicbf").ToString
    '            txtomisossena.Text = dtDeuda.Rows(0).Item("omisossena").ToString
    '            txtmorasena.Text = dtDeuda.Rows(0).Item("morasena").ToString
    '            txtinexactossena.Text = dtDeuda.Rows(0).Item("inexactossena").ToString
    '            txtomisossubfamiliar.Text = dtDeuda.Rows(0).Item("omisossubfamiliar").ToString
    '            txtmorasubfamiliar.Text = dtDeuda.Rows(0).Item("morasubfamiliar").ToString
    '            txtinexactossubfamiliar.Text = dtDeuda.Rows(0).Item("inexactossubfamiliar").ToString
    '            txtsentenciasjudiciales.Text = dtDeuda.Rows(0).Item("sentenciasjudiciales").ToString
    '            txtcuotaspartesacum.Text = dtDeuda.Rows(0).Item("cuotaspartesacum").ToString
    '            txttotalmultas.Text = dtDeuda.Rows(0).Item("totalmultas").ToString
    '            txttotalomisos.Text = dtDeuda.Rows(0).Item("totalomisos").ToString
    '            txttotalmora.Text = dtDeuda.Rows(0).Item("totalmora").ToString
    '            txttotalinexactos.Text = dtDeuda.Rows(0).Item("totalinexactos").ToString
    '            txttotalsentencias.Text = dtDeuda.Rows(0).Item("totalsentencias").ToString
    '            txttotaldeuda.Text = dtDeuda.Rows(0).Item("totaldeuda").ToString
    '        End If
    '    End If
    'End Sub

    Private Sub MostrarFacilidades(ByVal pNumExpediente As String)
        If pNumExpediente <> "" Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "select * from FACILIDADES_PAGO where NroExp = '" & pNumExpediente.Trim & "'"
            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'Mostrar datos                
                'txtNroResFac.Text = Reader("NroResFac").ToString().Trim
                'txtTipoGarantia.Text = Reader("TipoGarantia").ToString().Trim
                'txtNroGarantia.Text = Reader("NroGarantia").ToString().Trim
                'txtValGarantia.Text = Reader("ValGarantia").ToString().Trim
                'txtValTotAcu.Text = Reader("ValTotAcu").ToString().Trim
                'txtPorCuotaIni.Text = Reader("PorCuotaIni").ToString().Trim
                'txtValCuotaIni.Text = Reader("ValCuotaIni").ToString().Trim
                'txtNumCuotas.Text = Reader("NumCuotas").ToString().Trim
                'txtVencPrimCuo.Text = Reader("VencPrimCuo").ToString().Trim
                'txtVenUltCuo.Text = Reader("VenUltCuo").ToString().Trim

                ''Fecha
                ''txtFecRes.Text = Left(Reader("FecRes").ToString().Trim, 10)
                'txtFecResolFac.Text = Left(Reader("FecResolFac").ToString().Trim, 10)
                'txtFecSolFac.Text = Left(Reader("FecSolFac").ToString().Trim, 10)
                'txtFecNotif.Text = Left(Reader("FecNotif").ToString(), 10)
                'txtFecGarantia.Text = Left(Reader("FecGarantia").ToString(), 10)
                'txtFecPagCuoIni.Text = Left(Reader("FecPagCuoIni").ToString(), 10)

            End If
            Reader.Close()
            Connection.Close()
        End If
    End Sub

    Private Sub MostrarConcursales(ByVal pNumExpediente As String)
        If pNumExpediente <> "" Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "select * from CONCURSALES where NroExp = '" & pNumExpediente.Trim & "'"
            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'Mostrar datos                
                txtNroResApertura.Text = Reader("NroResApertura").ToString()
                txtNroOfiPres.Text = Reader("NroOfiPres").ToString()
                txtNroGuia.Text = Reader("NroGuia").ToString()
                txtNroOfiObj.Text = Reader("NroOfiObj").ToString()
                txtNroOfiRep.Text = Reader("NroOfiRep").ToString()
                txtNroResAcu.Text = Reader("NroResAcu").ToString()
                txtNroOfiDenInc.Text = Reader("NroOfiDenInc").ToString()
                txtDecisionInc.Text = Reader("DecisionInc").ToString()
                txtPorDerVoto.Text = Reader("PorDerVoto").ToString()
                txtOfiDemSupInt.Text = Reader("OfiDemSupInt").ToString()
                'Fecha
                txtFecRes.Text = Left(Reader("FecRes").ToString().Trim, 10)
                txtFecFijAvisoAdm.Text = Left(Reader("FecFijAvisoAdm").ToString().Trim, 10)
                txtFecDesfAvisoAdm.Text = Left(Reader("FecDesfAvisoAdm").ToString().Trim, 10)
                txtFecLimPresCred.Text = Left(Reader("FecLimPresCred").ToString().Trim, 10)
                txtFecOfiPres.Text = Left(Reader("FecOfiPres").ToString().Trim, 10)
                txtFecPres.Text = Left(Reader("FecPres").ToString().Trim, 10)
                txtFecTrasProy.Text = Left(Reader("FecTrasProy").ToString().Trim, 10)
                txtFecOfiObj.Text = Left(Reader("FecOfiObj").ToString().Trim, 10)
                txtFecPresObj.Text = Left(Reader("FecPresObj").ToString().Trim, 10)
                txtFecDecObj.Text = Left(Reader("FecDecObj").ToString().Trim, 10)
                txtFecRecRep.Text = Left(Reader("FecRecRep").ToString().Trim, 10)
                txtFecPresAcu.Text = Left(Reader("FecPresAcu").ToString().Trim, 10)
                txtFecAudConf.Text = Left(Reader("FecAudConf").ToString().Trim, 10)
                txtFecTermAcu.Text = Left(Reader("FecTermAcu").ToString().Trim, 10)
                txtFecResAcu.Text = Left(Reader("FecResAcu").ToString().Trim, 10)
                txtFecOfiIncump.Text = Left(Reader("FecOfiIncump").ToString().Trim, 10)
                txtFecPresOfiInc.Text = Left(Reader("FecPresOfiInc").ToString().Trim, 10)
                txtFecAudInc.Text = Left(Reader("FecAudInc").ToString().Trim, 10)
                txtFecPresDemSSInc.Text = Left(Reader("FecPresDemSSInc").ToString().Trim, 10)
                txtFecTrasConcursal.Text = Left(Reader("FecTras").ToString().Trim, 10)
                txtFecInsCamCom.Text = Left(Reader("FecInsCamCom").ToString().Trim, 10)
                txtFecadmis.Text = Left(Reader("Fecadmis").ToString().Trim, 10)
                txtFecLimPreObj.Text = Left(Reader("FecLimPreObj").ToString().Trim, 10)
                txtFecIniPagAcu.Text = Left(Reader("FecIniPagAcu").ToString().Trim, 10)
                txtFecFinPagAcu.Text = Left(Reader("FecFinPagAcu").ToString().Trim, 10)
                txtFecDemSupInt.Text = Left(Reader("FecDemSupInt").ToString().Trim, 10)
                txtFecFinConCursal.Text = Left(Reader("FecFinConCursal").ToString().Trim, 10)
                txtFecFinCobConcursal.Text = Left(Reader("FecFinCob").ToString().Trim, 10)
                'Combos
                cboPromotor.SelectedValue = Reader("Promotor").ToString().Trim
                cboTipoProcCon.SelectedValue = Reader("TipoProcCon").ToString().Trim
                cboEstadoTrasConcur.SelectedValue = Reader("EstadoTras").ToString().Trim
                'textarea
                txtObservacConcursal.Text = Reader("Observac").ToString()
            End If
            Reader.Close()
            Connection.Close()
        End If
    End Sub

    Private Sub MostrarSuspension(ByVal pNumExpediente As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        '
        Dim sql As String = "SELECT * FROM coactivo WHERE NroExp = @NroExp"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", Request("ID"))
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        If Reader.Read Then
            cboCausalSusp.SelectedValue = Reader("CausalSusp").ToString()
            txtNroResSusp.Text = Reader("NroResSusp").ToString()
            txtFecResSusp.Text = Left(Reader("FecResSusp").ToString(), 10)
            taObsSuspension.InnerHtml = Reader("ObservacSusp").ToString().Trim
        End If
        Reader.Close()
        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Sub MostrarPersuasivo(ByVal pNumExpediente As String)
        If pNumExpediente <> "" Then
            Dim cnx As String = Funciones.CadenaConexion
            Dim cmd As String = "SELECT NroExp,FecEstiFin,FecEnvioCC,FecIniCC,FecFinCC,ResConCC,ResCobCC,TelFijo,TelMovil,DirCorresp," &
                "Email,NroOfi1,FecOfi1,FecEnvOfi1,NoGuiaEnt1,FecAcuseO1,NroOfi2,FecOfi2,FecEnvOfi2,NoGuiaEnt2," &
                "FecAcuseO2,FecFinCob,ResConCob,CausalTerm,AutoTerm,FecTerm,Observac,EstaFinPag,EstTrans,FecEnGesCE, PoliticaPersuasivo, CriMedCau " &
                "FROM PERSUASIVO WHERE NroExp = '" & pNumExpediente.Trim & "'"

            Dim Adaptador As New SqlDataAdapter(cmd, cnx)
            Dim dtPersuasivo As New DataTable
            Adaptador.Fill(dtPersuasivo)
            If dtPersuasivo.Rows.Count > 0 Then
                'Mostrar datos

                txtFecEnvioCC.Text = Left(dtPersuasivo.Rows(0).Item("FecEnvioCC").ToString, 10)
                txtPoliticaPersuasivo.Text = dtPersuasivo.Rows(0).Item("PoliticaPersuasivo").ToString

                'Generar promer oficio persuasivo
                Dim pFeO, pFeL As String
                pFeO = ""
                pFeL = ""
                Dim pFO As DataTable = PrimerOficioPersuasivo(pNumExpediente)
                Dim pFL As DataTable = PrimeraLlamadaPersuasivo(pNumExpediente)
                If pFO.Rows.Count > 0 Then
                    pFeO = Left(pFO.Rows(0).Item("pFechaOficio").ToString, 10)
                End If
                If pFL.Rows.Count > 0 Then
                    pFeL = Left(pFL.Rows(0).Item("pFechaLlamada").ToString, 10)
                End If
                If pFeO = "" And pFeL = "" Then ' No hay fecha de oficio presuasivo
                    txtFecIniCC.Text = ""
                ElseIf pFeO = "" And pFeL <> "" Then ' No hay fecha de oficio presuasivo
                    txtFecIniCC.Text = Left(pFeL.ToString, 10)
                ElseIf pFeL = "" And pFeO <> "" Then ' No hay fecha de oficio presuasivo
                    txtFecIniCC.Text = Left(pFeO.ToString, 10)
                ElseIf (CDate(Left(pFeO.ToString, 10)) = CDate(Left(pFeL.ToString, 10))) Then
                    txtFecIniCC.Text = Left(pFeO.ToString, 10)
                ElseIf (CDate(Left(pFeO.ToString, 10)) > CDate(Left(pFeL.ToString, 10))) Then
                    txtFecIniCC.Text = Left(pFeL.ToString, 10)
                Else
                    txtFecIniCC.Text = Left(pFeO.ToString, 10)
                End If

                If txtFecIniCC.Text <> "" Then
                    txtFecEstiFin.Text = DateAdd(DateInterval.Month, CInt(IIf(txtPoliticaPersuasivo.Text = "", 0, txtPoliticaPersuasivo.Text)), CDate(txtFecIniCC.Text))
                End If


                Dim fFP As DataTable = FechafinPersuasivo(pNumExpediente)
                If fFP.Rows.Count > 0 Then
                    If fFP.Rows(0).Item("fechaUltiEstado").ToString = "" Then
                        txtFecFinCC.Text = ""
                    Else
                        If txtFecIniCC.Text = "" Then
                            txtFecFinCC.Text = ""
                        Else
                            txtFecFinCC.Text = Left(fFP.Rows(0).Item("fechaUltiEstado").ToString, 10)
                        End If

                    End If
                Else
                    txtFecFinCC.Text = ""
                End If


                'cboResConCC.Text = dtPersuasivo.Rows(0).Item("ResConCC").ToString
                'cboResCobCC.Text = dtPersuasivo.Rows(0).Item("ResCobCC").ToString
                'txtTelFijo.Text = dtPersuasivo.Rows(0).Item("TelFijo").ToString
                'txtTelMovil.Text = dtPersuasivo.Rows(0).Item("TelMovil").ToString
                'txtDirCorresp.Text = dtPersuasivo.Rows(0).Item("DirCorresp").ToString
                'txtEmail.Text = dtPersuasivo.Rows(0).Item("Email").ToString

                'cboResConCob.Text = dtPersuasivo.Rows(0).Item("ResConCob").ToString
                'txtCausalTerm.Text = dtPersuasivo.Rows(0).Item("CausalTerm").ToString
                cboCausalFinPers.SelectedValue = dtPersuasivo.Rows(0).Item("CausalTerm").ToString

                txtAutoTerm.Text = dtPersuasivo.Rows(0).Item("AutoTerm").ToString
                txtFecTerm.Text = Left(dtPersuasivo.Rows(0).Item("FecTerm").ToString, 10)
                txtEstaFinPag.Text = dtPersuasivo.Rows(0).Item("EstaFinPag").ToString

                'txtEstTrans.Text = dtPersuasivo.Rows(0).Item("EstTrans").ToString


                ' COMBOS                
                'cboResConCC.SelectedValue = dtPersuasivo.Rows(0).Item("ResConCC").ToString.Trim
                cboResCobCC.SelectedValue = dtPersuasivo.Rows(0).Item("ResCobCC").ToString.Trim
                cboCriteriosMC.SelectedValue = dtPersuasivo.Rows(0).Item("CriMedCau").ToString.Trim
                cboEstaFinPag.SelectedValue = dtPersuasivo.Rows(0).Item("EstaFinPag").ToString.Trim
                'cboEstTrans.SelectedValue = dtPersuasivo.Rows(0).Item("EstTrans").ToString.Trim
            Else
                RestriccionesPersuasivos()
            End If
            RestriccionesPersuasivos()
        End If
    End Sub

    Private Sub RestriccionesPersuasivos()
        If (txtFecEnvioCC.Text <> "" And (Session("mnivelacces") = 2 Or Session("mnivelacces") = 3)) Or (txtFecEnvioCC.Text = "") Then
            txtFecEnvioCC.Enabled = True
        Else
            txtFecEnvioCC.Enabled = False
            imgBtnBorraFecEnvioCC.Visible = False
        End If


        If (txtFecIniCC.Text <> "" And (Session("mnivelacces") = 2 Or Session("mnivelacces") = 3)) Then
            txtFecIniCC.Enabled = True
        Else
            txtFecIniCC.Enabled = False
        End If

        If (txtFecFinCC.Text <> "" And (Session("mnivelacces") = 2 Or Session("mnivelacces") = 3)) Then
            txtFecFinCC.Enabled = True
        Else
            txtFecFinCC.Enabled = False
            imgBtnBorraFecFinCC.Visible = False
        End If

        If (txtPoliticaPersuasivo.Text <> "" And (Session("mnivelacces") = 2 Or Session("mnivelacces") = 3)) Or (txtPoliticaPersuasivo.Text = "") Then
            txtPoliticaPersuasivo.Enabled = True
        Else
            txtPoliticaPersuasivo.Enabled = False
        End If
    End Sub

    Private Sub MostrarCoactivo(ByVal pNumExpediente As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        'if Request("ID") > 0 then this is an edit
        'if Request("ID") = 0 then this is an insert
        If Len(Request("ID")) > 0 Then
            Dim sql As String = "SELECT * FROM coactivo WHERE NroExp = @NroExp"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@NroExp", Request("ID"))
            Dim Reader As SqlDataReader = Command.ExecuteReader
            'If at least one record was found
            If Reader.Read Then
                'txtNroExpCoactivo.Text = Reader("NroExp").ToString()
                txtFecEstiFinCoac.Text = Left(Reader("FecEstiFin").ToString(), 10)
                txtResAvocaCon.Text = Reader("ResAvocaCon").ToString()
                txtFecResAvoca.Text = Left(Reader("FecResAvoca").ToString(), 10)
                txtFecNotAvoca.Text = Left(Reader("FecNotAvoca").ToString(), 10)
                txtResVinDeudorSol.Text = Reader("ResVinDeudorSol").ToString()
                txtFecVinDeudorSol.Text = Left(Reader("FecVinDeudorSol").ToString(), 10)
                'txtDeudoresVinc.Text = Reader("DeudoresVinc").ToString()                

                'Resolución que ordena continuar ejecución 
                txtNroResolEjec.Text = Reader("NroResolEjec").ToString().Trim
                txtFecResolEjec.Text = Left(Reader("FecResolEjec").ToString().Trim, 10)
                txtNroOfiCitaCor.Text = Reader("NroOfiCitaCor").ToString().Trim
                txtFecOfiCitaCor.Text = Left(Reader("FecOfiCitaCor").ToString().Trim, 10)
                txtFecNotifCor.Text = Left(Reader("FecNotifCor").ToString().Trim, 10)
                txtFecPubAviso.Text = Left(Reader("FecPubAviso").ToString().Trim, 10)

                'Datos de la liquidacion de credito --------------------------------------
                txtNroResLiquiCred.Text = Reader("NroResLiquiCred").ToString().Trim
                txtFecResLiquiCred.Text = Left(Reader("FecResLiquiCred").ToString(), 10)
                txtNroOfiCitCorLC.Text = Reader("NroOfiCitCorLC").ToString().Trim
                txtFecOfiCitCorLC.Text = Left(Reader("FecOfiCitCorLC").ToString(), 10)
                txtFecNotCorLC.Text = Left(Reader("FecNotCorLC").ToString(), 10)
                txtFecPubAvisoLC.Text = Left(Reader("FecPubAvisoLC").ToString(), 10)
                txtFecRadObj.Text = Left(Reader("FecRadObj").ToString(), 10)
                txtNroRad.Text = Reader("NroRad").ToString().Trim
                txtNroResApLiq.Text = Reader("NroResApLiq").ToString()
                txtFecResApLiq.Text = Left(Reader("FecResApLiq").ToString(), 10)
                txtNroOfiCorLD.Text = Reader("NroOfiCorLD").ToString()
                txtFecOfiCorLD.Text = Left(Reader("FecOfiCorLD").ToString(), 10)
                txtFecNotLD.Text = Left(Reader("FecNotLD").ToString(), 10)
                txtFecPubLD.Text = Left(Reader("FecPubLD").ToString(), 10)
                ' 15/jul/2014. Se agregaron los siguientes campos
                txtLiqCredCapital.Text = Reader("LiqCredCapital").ToString().Trim
                txtLiqCredInteres.Text = Reader("LiqCredInteres").ToString().Trim
                txtLiqCredTotal.Text = Reader("LiqCredTotal").ToString().Trim
                txtLiqCredFecCorte.Text = Left(Reader("LiqCredFecCorte").ToString().Trim, 10)
                If txtLiqCredCapital.Text = "" Then
                    txtLiqCredCapital.Text = "0"
                End If
                If txtLiqCredInteres.Text = "" Then
                    txtLiqCredInteres.Text = "0"
                End If
                If txtLiqCredTotal.Text = "" Then
                    txtLiqCredTotal.Text = "0"
                End If
                '--------------------------------------------------------------------------


                txtAdjudicat.Text = Reader("Adjudicat").ToString()
                txtNroResAdj.Text = Reader("NroResAdj").ToString()
                txtFecResAdj.Text = Left(Reader("FecResAdj").ToString(), 10)


                If Not IsDBNull(Reader("CausalExtin")) Then
                    CboExtincion.SelectedIndex = Reader("CausalExtin").ToString()
                End If

                If Not IsDBNull(Reader("DecretoExtin")) Then
                    CboDecreto.SelectedIndex = Reader("DecretoExtin").ToString()
                End If

                If Not IsDBNull(Reader("AlcanceExtin")) Then
                    cboAlcance.SelectedIndex = Reader("AlcanceExtin").ToString()
                End If

                txtPerIniExtin.Text = Reader("PerIniExtin").ToString()
                txtPerFinExtin.Text = Reader("PerFinExtin").ToString()
                txtValExtin.Text = Reader("ValExtin").ToString()
                txtNroResExtin.Text = Reader("NroResExtin").ToString()
                txtFecResExtin.Text = Left(Reader("FecResExtin").ToString(), 10)
                txtNroResLevMC.Text = Reader("NroResLevMC").ToString()
                txtFecResLevMC.Text = Left(Reader("FecResLevMC").ToString(), 10)
                If Not IsDBNull(Reader("TipoLevMC")) Then
                    cboTipoLevMC.SelectedValue = Reader("TipoLevMC").ToString().Trim
                End If

                If Not IsDBNull(Reader("CausalFinPro")) Then
                    cboCausalFinPro.SelectedValue = Reader("CausalFinPro").ToString().Trim
                End If
                txtNroResFinPro.Text = Reader("NroResFinPro").ToString()
                txtFecResFinPro.Text = Left(Reader("FecResFinPro").ToString(), 10)
                txtObsReselGes.Text = Reader("ObsReselGes").ToString()
                If Not IsDBNull(Reader("EstadoPagCoac")) Then
                    cboEstadoPagCoac.SelectedValue = Reader("EstadoPagCoac").ToString().Trim
                End If
                If Not IsDBNull(Reader("EstadoTras")) Then
                    cboEstadoTras.SelectedValue = Reader("EstadoTras").ToString()
                End If
                txtFecTras.Text = Left(Reader("FecTras").ToString(), 10)
                '

            End If
            'Close the Data Reader we are done with it.
            Reader.Close()

            'Close the Connection Object 
            Connection.Close()
        Else
            '             
        End If
    End Sub

    Private Function ExisteRegistroDeuda(ByVal pNumExpediente As String) As Boolean
        Dim cmd As String = "SELECT expediente FROM deudas WHERE expediente = '" & pNumExpediente.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, Funciones.CadenaConexion)
        Dim dtRegDeuda As New DataTable
        Adaptador.Fill(dtRegDeuda)
        If dtRegDeuda.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ExisteRegistroCoactivo(ByVal pNumExpediente As String) As Boolean
        Dim cmd As String = "SELECT NroExp FROM coactivo WHERE NroExp = '" & pNumExpediente.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, Funciones.CadenaConexion)
        Dim dtRegCoactivo As New DataTable
        Adaptador.Fill(dtRegCoactivo)
        If dtRegCoactivo.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ExisteRegistroConcursal(ByVal pNumExpediente As String) As Boolean
        Dim cmd As String = "SELECT NroExp FROM concursales WHERE NroExp = '" & pNumExpediente.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, Funciones.CadenaConexion)
        Dim dtConcursales As New DataTable
        Adaptador.Fill(dtConcursales)
        If dtConcursales.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ExisteRegistroFacilidad(ByVal pNumExpediente As String) As Boolean
        Dim cmd As String = "SELECT NroExp FROM FACILIDADES_PAGO WHERE NroExp = '" & pNumExpediente.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, Funciones.CadenaConexion)
        Dim dtFacilidad As New DataTable
        Adaptador.Fill(dtFacilidad)
        If dtFacilidad.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ExisteRegistroPersuasivo(ByVal pNumExpediente As String) As Boolean
        Dim cmd As String = "SELECT NroExp FROM persuasivo WHERE NroExp = '" & pNumExpediente.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, Funciones.CadenaConexion)
        Dim dtPersuasivo As New DataTable
        Adaptador.Fill(dtPersuasivo)
        If dtPersuasivo.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub BindGridCambioEstado()

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT CAMBIOS_ESTADO.*, USUARIOSrepartidor.nombre as USUARIOSrepartidornombre," &
             "USUARIOSabogado.nombre as USUARIOSabogadonombre, ESTADOS_PROCESOestado.nombre as ESTADOS_PROCESOestadonombre," &
             "ESTADOS_PAGOestadopago.nombre as ESTADOS_PAGOestadopagonombre, " &
             "ESTADOOPERATIVOestadooperativo.VAL_NOMBRE as ESTADOOPERATIVOestadooperativonombre, " &
             "ETAPAPROCESALetapaprocesal.VAL_ETAPA_PROCESAL as ETAPAPROCESALetapaprocesalnombre " &
             "FROM CAMBIOS_ESTADO " &
              "left join USUARIOS USUARIOSrepartidor on CAMBIOS_ESTADO.repartidor = USUARIOSrepartidor.codigo " &
              "left join USUARIOS USUARIOSabogado on CAMBIOS_ESTADO.abogado = USUARIOSabogado.codigo " &
              "left join ESTADOS_PROCESO ESTADOS_PROCESOestado on CAMBIOS_ESTADO.estado = ESTADOS_PROCESOestado.codigo " &
              "left join ESTADOS_PAGO ESTADOS_PAGOestadopago on CAMBIOS_ESTADO.estadopago = ESTADOS_PAGOestadopago.codigo " &
              "left join ESTADO_OPERATIVO ESTADOOPERATIVOestadooperativo on CAMBIOS_ESTADO.estadooperativo = ESTADOOPERATIVOestadooperativo.ID_ESTADO_OPERATIVOS " &
              "left join ETAPA_PROCESAL ETAPAPROCESALetapaprocesal on CAMBIOS_ESTADO.etapaprocesal = ETAPAPROCESALetapaprocesal.ID_ETAPA_PROCESAL " &
             "WHERE CAMBIOS_ESTADO.NroExp = '" & Request("ID") & "'"
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grdCambiosEstado.DataSource = Command.ExecuteReader()
        grdCambiosEstado.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grdCambiosEstado.Rows.Count
        Connection.Close()
    End Sub


    'Protected Sub cmdSaveFacilidades_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveFacilidades.Click
    'Dim ID As String = Request("ID")

    'If String.IsNullOrEmpty(ID) Then
    '    Exit Sub
    'End If

    'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
    'Connection.Open()
    'Dim Command As SqlCommand

    ''Comandos SQL para insertar y editar 
    'Dim InsertSQL As String = "Insert into FACILIDADES_PAGO ([NroExp], [NroResFac], [FecResolFac], [FecSolFac], [FecNotif], [TipoGarantia], [NroGarantia], [FecGarantia], [ValGarantia], [ValTotAcu], [PorCuotaIni], [ValCuotaIni], [NumCuotas], [FecPagCuoIni], [VencPrimCuo], [VenUltCuo] ) VALUES ( @NroExp, @NroResFac, @FecResolFac, @FecSolFac, @FecNotif, @TipoGarantia, @NroGarantia, @FecGarantia, @ValGarantia, @ValTotAcu, @PorCuotaIni, @ValCuotaIni, @NumCuotas, @FecPagCuoIni, @VencPrimCuo, @VenUltCuo ) "
    'Dim UpdateSQL As String = "Update FACILIDADES_PAGO set [FecResolFac] = @FecResolFac, [FecSolFac] = @FecSolFac, [FecNotif] = @FecNotif, [TipoGarantia] = @TipoGarantia, [NroGarantia] = @NroGarantia, [FecGarantia] = @FecGarantia, [ValGarantia] = @ValGarantia, [ValTotAcu] = @ValTotAcu, [PorCuotaIni] = @PorCuotaIni, [ValCuotaIni] = @ValCuotaIni, [NumCuotas] = @NumCuotas, [FecPagCuoIni] = @FecPagCuoIni, [VencPrimCuo] = @VencPrimCuo, [VenUltCuo] = @VenUltCuo where [NroExp] = @NroExp "

    ''Si el registro de facilidades esta en la tabla FACILIDADES_PAGO existe => UPDATE, sino INSERT
    'If ExisteRegistroFacilidad(ID) Then
    '    'update
    '    Command = New SqlCommand(UpdateSQL, Connection)
    '    Command.Parameters.AddWithValue("@NroExp", ID)
    'Else
    '    'insert 
    '    Command = New SqlCommand(InsertSQL, Connection)
    '    Command.Parameters.AddWithValue("@NroExp", ID)
    'End If

    ''Parametros
    'Command.Parameters.AddWithValue("@NroResFac", txtNroResFac.Text.Trim)

    'If IsDate(Left(txtFecResolFac.Text.Trim, 10)) Then
    '    Command.Parameters.AddWithValue("@FecResolFac", Left(txtFecResolFac.Text.Trim, 10))
    'Else
    '    Command.Parameters.AddWithValue("@FecResolFac", DBNull.Value)
    'End If

    'If IsDate(Left(txtFecSolFac.Text.Trim, 10)) Then
    '    Command.Parameters.AddWithValue("@FecSolFac", Left(txtFecSolFac.Text.Trim, 10))
    'Else
    '    Command.Parameters.AddWithValue("@FecSolFac", DBNull.Value)
    'End If

    'If IsDate(Left(txtFecNotif.Text.Trim, 10)) Then
    '    Command.Parameters.AddWithValue("@FecNotif", Left(txtFecNotif.Text.Trim, 10))
    'Else
    '    Command.Parameters.AddWithValue("@FecNotif", DBNull.Value)
    'End If

    'Command.Parameters.AddWithValue("@TipoGarantia", txtTipoGarantia.Text.Trim)
    'Command.Parameters.AddWithValue("@NroGarantia", txtNroGarantia.Text.Trim)

    'If IsDate(Left(txtFecGarantia.Text.Trim, 10)) Then
    '    Command.Parameters.AddWithValue("@FecGarantia", Left(txtFecGarantia.Text.Trim, 10))
    'Else
    '    Command.Parameters.AddWithValue("@FecGarantia", DBNull.Value)
    'End If

    'If IsNumeric(txtValGarantia.Text) Then
    '    Command.Parameters.AddWithValue("@ValGarantia", txtValGarantia.Text)
    'Else
    '    Command.Parameters.AddWithValue("@ValGarantia", DBNull.Value)
    'End If

    'If IsNumeric(txtValTotAcu.Text) Then
    '    Command.Parameters.AddWithValue("@ValTotAcu", txtValTotAcu.Text)
    'Else
    '    Command.Parameters.AddWithValue("@ValTotAcu", DBNull.Value)
    'End If

    'If IsNumeric(txtPorCuotaIni.Text) Then
    '    Command.Parameters.AddWithValue("@PorCuotaIni", txtPorCuotaIni.Text)
    'Else
    '    Command.Parameters.AddWithValue("@PorCuotaIni", DBNull.Value)
    'End If

    'If IsNumeric(txtValCuotaIni.Text) Then
    '    Command.Parameters.AddWithValue("@ValCuotaIni", txtValCuotaIni.Text)
    'Else
    '    Command.Parameters.AddWithValue("@ValCuotaIni", DBNull.Value)
    'End If

    'If IsNumeric(txtNumCuotas.Text) Then
    '    Command.Parameters.AddWithValue("@NumCuotas", txtNumCuotas.Text)
    'Else
    '    Command.Parameters.AddWithValue("@NumCuotas", DBNull.Value)
    'End If

    'If IsDate(Left(txtFecPagCuoIni.Text.Trim, 10)) Then
    '    Command.Parameters.AddWithValue("@FecPagCuoIni", Left(txtFecPagCuoIni.Text.Trim, 10))
    'Else
    '    Command.Parameters.AddWithValue("@FecPagCuoIni", DBNull.Value)
    'End If

    'If IsDate(Left(txtVencPrimCuo.Text.Trim, 10)) Then
    '    Command.Parameters.AddWithValue("@VencPrimCuo", Left(txtVencPrimCuo.Text.Trim, 10))
    'Else
    '    Command.Parameters.AddWithValue("@VencPrimCuo", DBNull.Value)
    'End If

    'If IsDate(Left(txtVenUltCuo.Text.Trim, 10)) Then
    '    Command.Parameters.AddWithValue("@VenUltCuo", Left(txtVenUltCuo.Text.Trim, 10))
    'Else
    '    Command.Parameters.AddWithValue("@VenUltCuo", DBNull.Value)
    'End If

    'Try
    '    Command.ExecuteNonQuery()

    '    'Después de cada GRABAR hay que llamar al log de auditoria
    '    Dim LogProc As New LogProcesos
    '    LogProc.SaveLog(Session("ssloginusuario"), "Gestión de facilidades de pago", "Expediente " & ID, Command)
    'Catch ex As Exception

    'End Try

    'Connection.Close()
    'End Sub

    Protected Sub cmdSaveConcursal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveConcursal.Click
        Dim ID As String = Request("ID")

        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        '/-----------------------------------------------------------------  
        'Nombre Requerimiento: Campos Obligatorios de información de los Procesos Concursales 
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 23-07-2017  
        'Objetivo : Establecer campos de obligtorio diligenciamiento en la pestaña "Procesos Concursles" del aplicativo de cobro.
        ' 1. Inicio del proceso.  
        '   1.1. Tipo de proceso concursal. 
        '   1.2. No. auto de apertura del proceso.
        '   1.3. Fecha auto. 
        ' 2. Presentación del crédito. 
        '   2.1. Fecha de admisión 
        '   2.2. Fecha límite para presentar el crédito. 
        '   2.3. No. oficio de presentación. 
        '   2.4. Fecha oficio de presentación. 
        '   2.5. Fecha de presentación. 
        '   2.6. No. De guía / Radicación. 
        ' 3. Se cambia por desición de la UGPP área Cobros & coactivos 
        '   Se dejan obligatorios los campos. 
        '   Fecha de Admisión
        '   Fecha Limite para presentar el crédito
        '   10 días despúes de recibido el expediente:
        '       No.Oficio de Presentación
        '       Fecha Oficio de presentación
        '------------------------------------------------------------------/
        Dim cstype As Type = Me.GetType()
        Dim strScript As String = IIf(intObliFechaPresentacion > intDiasPresentacionConcursales, "$(function() { ShowHideRequired(true);})", "$(function() { ShowHideRequired(false);})")
        cs.RegisterStartupScript(Page.GetType(), "Script", strScript, True)

        ' Inicio del proceso. 
        If cboTipoProcCon.SelectedItem.Value.ToString().Equals("0") Then
            CustomValidator3.Text = "Seleccione el tipo de proceso concursal. Por favor."
            CustomValidator3.IsValid = False
            Return
        End If

        If txtNroResApertura.Text = String.Empty Then
            CustomValidator3.Text = "Digite el número auto de apertura del proceso. Por favor."
            CustomValidator3.IsValid = False
            Return
        End If

        If txtFecRes.Text = String.Empty Then
            CustomValidator3.Text = "Diligencie la fecha auto. Por favor."
            CustomValidator3.IsValid = False
            Return
        End If

        ' Presentación del crédito 
        If txtFecadmis.Text = String.Empty Then
            CustomValidator3.Text = "Diligencie la fecha de admisión. Por favor."
            CustomValidator3.IsValid = False
            Return
        End If


        If txtFecLimPresCred.Text = String.Empty Then
            CustomValidator3.Text = "Diligencie la fecha límite para presentar el crédito. Por favor."
            CustomValidator3.IsValid = False
            Return
        End If

        If intObliFechaPresentacion > intDiasPresentacionConcursales Then
            If txtNroOfiPres.Text = String.Empty Then
                CustomValidator3.Text = "Diligencie el número de oficio de presentación. Por favor."
                CustomValidator3.IsValid = False
                Return
            End If

            If txtFecOfiPres.Text = String.Empty Then
                CustomValidator3.Text = "Diligencie la fecha oficio de presentación. Por favor."
                CustomValidator3.IsValid = False
                Return
            End If
        End If

        'If txtFecPres.Text = String.Empty Then
        '    CustomValidator3.Text = "Diligencie la fecha de presentación. Por favor."
        '    CustomValidator3.IsValid = False
        '    Return
        'End If

        'If txtNroGuia.Text = String.Empty Then
        '    CustomValidator3.Text = "Diligencie el número de guía / Radicación. Por favor."
        '    CustomValidator3.IsValid = False
        '    Return
        'End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL para insertar y editar 
        Dim InsertSQL As String = "INSERT INTO CONCURSALES ([NroExp], [TipoProcCon], [NroResApertura], [FecRes], [Promotor], [FecFijAvisoAdm], [FecDesfAvisoAdm], [FecLimPresCred], [NroOfiPres], [FecOfiPres], [FecPres], [NroGuia], [FecTrasProy], [NroOfiObj], [FecOfiObj], [FecPresObj], [FecDecObj], [NroOfiRep], [FecRecRep], [FecPresAcu], [FecAudConf], [FecTermAcu], [NroResAcu], [FecResAcu], [NroOfiDenInc], [FecOfiIncump], [FecPresOfiInc], [FecAudInc], [DecisionInc], [FecPresDemSSInc], [Observac], [EstadoTras], [FecTras], [FecInsCamCom], [Fecadmis], [FecLimPreObj], [PorDerVoto], [FecIniPagAcu], [FecFinPagAcu], [OfiDemSupInt], [FecDemSupInt], [FecFinConCursal], [FecFinCob] ) VALUES ( @NroExp, @TipoProcCon, @NroResApertura, @FecRes, @Promotor, @FecFijAvisoAdm, @FecDesfAvisoAdm, @FecLimPresCred, @NroOfiPres, @FecOfiPres, @FecPres, @NroGuia, @FecTrasProy, @NroOfiObj, @FecOfiObj, @FecPresObj, @FecDecObj, @NroOfiRep, @FecRecRep, @FecPresAcu, @FecAudConf, @FecTermAcu, @NroResAcu, @FecResAcu, @NroOfiDenInc, @FecOfiIncump, @FecPresOfiInc, @FecAudInc, @DecisionInc, @FecPresDemSSInc, @Observac, @EstadoTras, @FecTras, @FecInsCamCom, @Fecadmis, @FecLimPreObj, @PorDerVoto, @FecIniPagAcu, @FecFinPagAcu, @OfiDemSupInt, @FecDemSupInt, @FecFinConCursal, @FecFinCob ) "
        Dim UpdateSQL As String = "UPDATE CONCURSALES SET [TipoProcCon] = @TipoProcCon, [NroResApertura] = @NroResApertura, [FecRes] = @FecRes, [Promotor] = @Promotor, [FecFijAvisoAdm] = @FecFijAvisoAdm, [FecDesfAvisoAdm] = @FecDesfAvisoAdm, [FecLimPresCred] = @FecLimPresCred, [NroOfiPres] = @NroOfiPres, [FecOfiPres] = @FecOfiPres, [FecPres] = @FecPres, [NroGuia] = @NroGuia, [FecTrasProy] = @FecTrasProy, [NroOfiObj] = @NroOfiObj, [FecOfiObj] = @FecOfiObj, [FecPresObj] = @FecPresObj, [FecDecObj] = @FecDecObj, [NroOfiRep] = @NroOfiRep, [FecRecRep] = @FecRecRep, [FecPresAcu] = @FecPresAcu, [FecAudConf] = @FecAudConf, [FecTermAcu] = @FecTermAcu, [NroResAcu] = @NroResAcu, [FecResAcu] = @FecResAcu, [NroOfiDenInc] = @NroOfiDenInc, [FecOfiIncump] = @FecOfiIncump, [FecPresOfiInc] = @FecPresOfiInc, [FecAudInc] = @FecAudInc, [DecisionInc] = @DecisionInc, [FecPresDemSSInc] = @FecPresDemSSInc, [Observac] = @Observac, [EstadoTras] = @EstadoTras, [FecTras] = @FecTras, [FecInsCamCom] = @FecInsCamCom, [Fecadmis] = @Fecadmis, [FecLimPreObj] = @FecLimPreObj, [PorDerVoto] = @PorDerVoto, [FecIniPagAcu] = @FecIniPagAcu, [FecFinPagAcu] = @FecFinPagAcu, [OfiDemSupInt] = @OfiDemSupInt, [FecDemSupInt] = @FecDemSupInt, [FecFinConCursal] = @FecFinConCursal, [FecFinCob] = @FecFinCob where [NroExp] = @NroExp "

        'Si el registro del concursal en la tabla CONCURSALES existe => UPDATE, sino INSERT
        If ExisteRegistroConcursal(ID) Then
            'update
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If

        'Parametros
        If cboTipoProcCon.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@TipoProcCon", cboTipoProcCon.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@TipoProcCon", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroResApertura", txtNroResApertura.Text.Trim)

        If IsDate(Left(txtFecRes.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecRes", Left(txtFecRes.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecRes", DBNull.Value)
        End If

        If cboPromotor.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Promotor", cboPromotor.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Promotor", DBNull.Value)
        End If

        If IsDate(Left(txtFecFijAvisoAdm.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecFijAvisoAdm", Left(txtFecFijAvisoAdm.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecFijAvisoAdm", DBNull.Value)
        End If

        If IsDate(Left(txtFecDesfAvisoAdm.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecDesfAvisoAdm", Left(txtFecDesfAvisoAdm.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecDesfAvisoAdm", DBNull.Value)
        End If

        If IsDate(Left(txtFecLimPresCred.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecLimPresCred", Left(txtFecLimPresCred.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecLimPresCred", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroOfiPres", txtNroOfiPres.Text.Trim)

        If IsDate(Left(txtFecOfiPres.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecOfiPres", Left(txtFecOfiPres.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecOfiPres", DBNull.Value)
        End If

        If IsDate(Left(txtFecPres.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPres", txtFecPres.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecPres", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroGuia", txtNroGuia.Text.Trim)

        If IsDate(Left(txtFecTrasProy.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecTrasProy", Left(txtFecTrasProy.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecTrasProy", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroOfiObj", txtNroOfiObj.Text.Trim)

        If IsDate(Left(txtFecOfiObj.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecOfiObj", Left(txtFecOfiObj.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecOfiObj", DBNull.Value)
        End If

        If IsDate(Left(txtFecPresObj.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPresObj", Left(txtFecPresObj.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecPresObj", DBNull.Value)
        End If

        If IsDate(Left(txtFecDecObj.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecDecObj", Left(txtFecDecObj.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecDecObj", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroOfiRep", txtNroOfiRep.Text.Trim)

        If IsDate(Left(txtFecRecRep.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecRecRep", Left(txtFecRecRep.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecRecRep", DBNull.Value)
        End If

        If IsDate(Left(txtFecPresAcu.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPresAcu", Left(txtFecPresAcu.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecPresAcu", DBNull.Value)
        End If

        If IsDate(Left(txtFecAudConf.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecAudConf", Left(txtFecAudConf.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecAudConf", DBNull.Value)
        End If

        If IsDate(Left(txtFecTermAcu.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecTermAcu", Left(txtFecTermAcu.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecTermAcu", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroResAcu", txtNroResAcu.Text.Trim)

        If IsDate(Left(txtFecResAcu.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResAcu", Left(txtFecResAcu.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResAcu", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroOfiDenInc", txtNroOfiDenInc.Text)

        If IsDate(Left(txtFecOfiIncump.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecOfiIncump", Left(txtFecOfiIncump.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecOfiIncump", DBNull.Value)
        End If

        If IsDate(Left(txtFecPresOfiInc.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPresOfiInc", Left(txtFecPresOfiInc.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecPresOfiInc", DBNull.Value)
        End If

        If IsDate(Left(txtFecAudInc.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecAudInc", Left(txtFecAudInc.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecAudInc", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@DecisionInc", txtDecisionInc.Text.Trim)

        If IsDate(Left(txtFecPresDemSSInc.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPresDemSSInc", Left(txtFecPresDemSSInc.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecPresDemSSInc", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@Observac", txtObservacConcursal.Text.Trim)

        '16/02/2015. Estado al que se translada en la pagina de concursales 
        If cboEstadoTrasConcur.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@EstadoTras", cboEstadoTrasConcur.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@EstadoTras", DBNull.Value)
        End If

        ' inclusion de validacion 23/12/2014
        'If IsDate(Left(txtFecTrasConcursal.Text.Trim, 10)) Then
        '    Command.Parameters.AddWithValue("@FecTras", Left(txtFecTrasConcursal.Text.Trim, 10))
        'Else
        '    Command.Parameters.AddWithValue("@FecTras", DBNull.Value)
        'End If

        '-- validacion de fecha --'

        If IsDate(Left(txtFecTrasConcursal.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecTras", Left(txtFecTrasConcursal.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecTras", DBNull.Value)
        End If

        If IsDate(Left(txtFecInsCamCom.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecInsCamCom", Left(txtFecInsCamCom.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecInsCamCom", DBNull.Value)
        End If

        If IsDate(Left(txtFecadmis.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@Fecadmis", Left(txtFecadmis.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@Fecadmis", DBNull.Value)
        End If

        If IsDate(Left(txtFecLimPreObj.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecLimPreObj", Left(txtFecLimPreObj.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecLimPreObj", DBNull.Value)
        End If

        If IsNumeric(txtPorDerVoto.Text) Then
            Command.Parameters.AddWithValue("@PorDerVoto", txtPorDerVoto.Text)
        Else
            Command.Parameters.AddWithValue("@PorDerVoto", DBNull.Value)
        End If

        If IsDate(Left(txtFecIniPagAcu.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecIniPagAcu", Left(txtFecIniPagAcu.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecIniPagAcu", DBNull.Value)
        End If

        If IsDate(Left(txtFecFinPagAcu.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecFinPagAcu", Left(txtFecFinPagAcu.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecFinPagAcu", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@OfiDemSupInt", txtOfiDemSupInt.Text.Trim)

        If IsDate(Left(txtFecDemSupInt.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecDemSupInt", Left(txtFecDemSupInt.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecDemSupInt", DBNull.Value)
        End If

        If IsDate(Left(txtFecFinConCursal.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecFinConCursal", Left(txtFecFinConCursal.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecFinConCursal", DBNull.Value)
        End If

        If IsDate(Left(txtFecFinCobConcursal.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecFinCob", Left(txtFecFinCobConcursal.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecFinCob", DBNull.Value)
        End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de concursales", "Expediente " & ID, Command)
        Catch ex As Exception
            CustomValidator3.Text = ex.Message
            CustomValidator3.IsValid = False
        End Try

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub cmdSavePersuasivo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSavePersuasivo.Click

        '---------------------------------------------------------------------------------------------------------------
        ' VALIDACION 3: La fecha de envio caso a Call Center debe ser < a fecha FecIniPers en Call C 
        '               ...y fecha FecIniPers en Call C < fecha culminacion en Call C.
        '---------------------------------------------------------------------------------------------------------------

        'txtFecEnvioCC
        If txtFecEnvioCC.Text.Trim = "" Then
            CustomValidator1.Text = "Por favor diigite la Fecha envío a la DSIAC."
            CustomValidator1.IsValid = False
            Return
        End If

        ''txtFecIniCC
        'If txtFecIniCC.Text.Trim = "" Then
        '    CustomValidator1.Text = "Por favor diigite la fecha Primera Acción Persuasiva"
        '    CustomValidator1.IsValid = False
        '    Return
        'End If


        If txtPoliticaPersuasivo.Text.Trim = "" Then
            CustomValidator1.Text = "Por favor diigite la política de persuasivo."
            CustomValidator1.IsValid = False
            Return
        End If


        ''txtFecFinCC
        'If txtFecFinCC.Text.Trim = "" Then
        '    CustomValidator1.Text = "Por favor diigite la fecha culminación persuasivo en DSIAC"
        '    CustomValidator1.IsValid = False
        '    Return
        'End If

        'If FecEnvioCC <> Nothing And FecIniCC <> Nothing Then
        '    If FecEnvioCC >= FecIniCC Then
        '        CustomValidator1.Text = "La fecha envío del caso a call center no puede ser mayor o igual a la fecha de inicio persuasivo en Call Center"
        '        CustomValidator1.IsValid = False
        '        Return
        '    End If
        'End If

        If txtFecIniCC.Text <> "" And txtFecFinCC.Text <> "" Then
            If CDate(txtFecIniCC.Text) >= CDate(txtFecFinCC.Text) Then
                CustomValidator1.Text = "La Fecha Primera Acción Persuasiva no puede ser mayor o igual a la Fecha culminación persuasivo en DSIAC"
                CustomValidator1.IsValid = False
                Return
            End If
        End If


        '30 Marzo 2016 Cesar
        '-------------------------
        If txtFecIniCC.Text <> "" And txtFecEnvioCC.Text <> "" Then
            If CDate(txtFecIniCC.Text) < CDate(txtFecEnvioCC.Text) Then
                CustomValidator1.Text = "La fecha primera accion persuasiva no puede ser inferior a la fecha de envio a la DSIAC."
                CustomValidator1.IsValid = False
                Exit Sub
            End If
        End If

        If txtFecFinCC.Text <> "" And txtFecEnvioCC.Text <> "" Then
            If CDate(txtFecFinCC.Text) < CDate(txtFecEnvioCC.Text) Then
                CustomValidator1.Text = "La fecha culminación persuasivo en DSIAC no puede ser inferior a la fecha de envio a la DSIAC."
                CustomValidator1.IsValid = False
                Exit Sub
            End If
        End If
        '-----------------------




        Dim ID As String = Request("ID")

        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL para insertar y editar 
        Dim InsertSQL As String = "INSERT INTO PERSUASIVO (NroExp, FecEstiFin, FecEnvioCC,FecIniCC, FecFinCC, ResCobCC, CausalTerm, AutoTerm, FecTerm, EstaFinPag, PoliticaPersuasivo, CriMedCau) VALUES ( @NroExp,@FecEstiFin,@FecEnvioCC,@FecIniCC,@FecFinCC,@ResCobCC, @CausalTerm,@AutoTerm, @FecTerm, @EstaFinPag, @PoliticaPersuasivo,@CriMedCau ) "
        Dim UpdateSQL As String = "UPDATE PERSUASIVO SET FecEstiFin = @FecEstiFin, FecEnvioCC = @FecEnvioCC, FecIniCC = @FecIniCC, FecFinCC   = @FecFinCC  ,ResCobCC   = @ResCobCC, CausalTerm = @CausalTerm, AutoTerm = @AutoTerm, FecTerm = @FecTerm, EstaFinPag = @EstaFinPag, PoliticaPersuasivo = @PoliticaPersuasivo, CriMedCau = @CriMedCau WHERE NroExp = @NroExp "

        'Si el registro del persuasivo en la tabla PERSUASIVO existe => UPDATE, sino INSERT
        If ExisteRegistroPersuasivo(ID) Then
            'update
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If

        If IsDate(Left(txtFecEstiFin.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecEstiFin", Left(txtFecEstiFin.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecEstiFin", DBNull.Value)
        End If

        If IsDate(Left(txtFecEnvioCC.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecEnvioCC", Left(txtFecEnvioCC.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecEnvioCC", DBNull.Value)
        End If

        If IsDate(Left(txtFecIniCC.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecIniCC", Left(txtFecIniCC.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecIniCC", DBNull.Value)
        End If

        If IsDate(Left(txtFecFinCC.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecFinCC", Left(txtFecFinCC.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecFinCC", DBNull.Value)
        End If

        'If cboResConCC.SelectedValue.Length > 0 Then
        '    Command.Parameters.AddWithValue("@ResConCC", cboResConCC.SelectedValue)
        'Else
        '    Command.Parameters.AddWithValue("@ResConCC", DBNull.Value)
        'End If

        If cboResCobCC.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@ResCobCC", cboResCobCC.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@ResCobCC", DBNull.Value)
        End If

        If cboCriteriosMC.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@CriMedCau", cboCriteriosMC.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@CriMedCau", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@CausalTerm", cboCausalFinPers.SelectedValue.Trim)
        Command.Parameters.AddWithValue("@AutoTerm", txtAutoTerm.Text)

        If IsDate(Left(txtFecTerm.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecTerm", Left(txtFecTerm.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecTerm", DBNull.Value)
        End If

        If cboEstaFinPag.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@EstaFinPag", cboEstaFinPag.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@EstaFinPag", DBNull.Value)
        End If

        'txtPoliticaPersuasivo
        Command.Parameters.AddWithValue("@PoliticaPersuasivo", txtPoliticaPersuasivo.Text)

        Try
            Command.ExecuteNonQuery()

            RestriccionesPersuasivos()

            CustomValidator1.Text = "Datos guardados correctamente..."
            CustomValidator1.IsValid = False

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de persuasivos", "Expediente " & ID, Command)
        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
        End Try

        'Close the Connection Object 
        Connection.Close()


    End Sub

    Protected Sub cmdSaveCoactivo1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveCoactivo1.Click
        Dim ID As String = Request("ID")

        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp, FecEstiFin, ResAvocaCon, FecResAvoca, FecNotAvoca, ResVinDeudorSol, FecVinDeudorSol, Adjudicat, NroResAdj, FecResAdj, CausalExtin, DecretoExtin, AlcanceExtin, PerIniExtin, PerFinExtin, ValExtin, NroResExtin, FecResExtin, ObsReselGes, EstadoPagCoac, FecTras ) VALUES ( @NroExp, @FecEstiFin, @ResAvocaCon, @FecResAvoca, @FecNotAvoca, @ResVinDeudorSol, @FecVinDeudorSol, @Adjudicat, @NroResAdj, @FecResAdj, @CausalExtin, @DecretoExtin, @AlcanceExtin, @PerIniExtin, @PerFinExtin, @ValExtin, @NroResExtin, @FecResExtin, @ObsReselGes, @EstadoPagCoac, @FecTras) "
        Dim UpdateSQL As String = "UPDATE COACTIVO SET FecEstiFin = @FecEstiFin, ResAvocaCon = @ResAvocaCon, FecResAvoca = @FecResAvoca, FecNotAvoca = @FecNotAvoca, ResVinDeudorSol = @ResVinDeudorSol, FecVinDeudorSol = @FecVinDeudorSol, Adjudicat = @Adjudicat, NroResAdj = @NroResAdj, FecResAdj = @FecResAdj, CausalExtin = @CausalExtin, DecretoExtin = @DecretoExtin, AlcanceExtin = @AlcanceExtin, PerIniExtin = @PerIniExtin, PerFinExtin = @PerFinExtin, ValExtin = @ValExtin, NroResExtin = @NroResExtin, FecResExtin = @FecResExtin, ObsReselGes = @ObsReselGes, EstadoPagCoac = @EstadoPagCoac, FecTras = @FecTras WHERE NroExp = @NroExp "

        'Si el registro esta en la tabla COACTIVO existe => UPDATE, sino INSERT
        If ExisteRegistroCoactivo(ID) Then
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If

        If IsDate(Left(txtFecEstiFinCoac.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecEstiFin", Left(txtFecEstiFinCoac.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecEstiFin", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@ResAvocaCon", txtResAvocaCon.Text)

        If IsDate(Left(txtFecResAvoca.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResAvoca", Left(txtFecResAvoca.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResAvoca", DBNull.Value)
        End If

        If IsDate(Left(txtFecNotAvoca.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecNotAvoca", Left(txtFecNotAvoca.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecNotAvoca", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@ResVinDeudorSol", txtResVinDeudorSol.Text)

        If IsDate(Left(txtFecVinDeudorSol.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecVinDeudorSol", Left(txtFecVinDeudorSol.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecVinDeudorSol", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@Adjudicat", txtAdjudicat.Text)
        Command.Parameters.AddWithValue("@NroResAdj", txtNroResAdj.Text)

        If IsDate(Left(txtFecResAdj.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResAdj", Left(txtFecResAdj.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResAdj", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@CausalExtin", CboExtincion.SelectedIndex)
        Command.Parameters.AddWithValue("@DecretoExtin", CboDecreto.SelectedIndex)
        Command.Parameters.AddWithValue("@AlcanceExtin", cboAlcance.SelectedIndex)
        Command.Parameters.AddWithValue("@PerIniExtin", txtPerIniExtin.Text)
        Command.Parameters.AddWithValue("@PerFinExtin", txtPerFinExtin.Text)

        If IsNumeric(txtValExtin.Text) Then
            Command.Parameters.AddWithValue("@ValExtin", txtValExtin.Text)
        Else
            Command.Parameters.AddWithValue("@ValExtin", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroResExtin", txtNroResExtin.Text)

        If IsDate(txtFecResExtin.Text.Trim) Then
            Command.Parameters.AddWithValue("@FecResExtin", txtFecResExtin.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecResExtin", DBNull.Value)
        End If


        Command.Parameters.AddWithValue("@ObsReselGes", txtObsReselGes.Text)

        If IsDate(Left(txtFecTras.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecTras", Left(txtFecTras.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecTras", DBNull.Value)
        End If

        'Combos ---------------------------------------------------------------------------------


        '4. Estado del pago en Cobro Coactivo cboEstadoPagCoac
        If cboEstadoPagCoac.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@EstadoPagCoac", cboEstadoPagCoac.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@EstadoPagCoac", DBNull.Value)
        End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de coactivo", "Expediente " & ID, Command)
            CustomValidator2.Text = "Datos guardados con éxito"
            CustomValidator2.IsValid = False

        Catch ex As Exception
            Dim Msg As String
            Msg = ex.Message
            CustomValidator2.Text = Msg
            CustomValidator2.IsValid = False

        End Try

        Connection.Close()
    End Sub

    Private Function GetNomPerfil(ByVal pUsuario As String) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & Session("mnivelacces")

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        Return NomPerfil
    End Function

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()

        Response.Redirect("../../login.aspx")
    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("EJEFISGLOBAL.aspx")
    End Sub

    Protected Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click
        'EditEJEFISGLOBAL.aspx?ID=80003 
        Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx?ID=" & Request("ID"))
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        'Limpiar los cuadros de texto de busqueda
        'Limpiar cuadros de busqueda
        'Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
        'Session("EJEFISGLOBAL.txtSearchED_NOMBRE") = ""
        'Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
        'Session("EJEFISGLOBAL.cboEFIESTADO") = ""
        'Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
        'Session("Paginacion") = 10
        Session.RemoveAll()
    End Sub


    Private Sub Loadcboextincion()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select * from tipo_extincion order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTPC As New DataTable
        Adaptador.Fill(dtTPC)
        If dtTPC.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTPC As DataRow = dtTPC.NewRow()
            filaTPC("codigo") = 0
            filaTPC("nombre") = ""
            dtTPC.Rows.Add(filaTPC)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTPC As DataView = New DataView(dtTPC)
            vistaTPC.Sort = "codigo"
            '--------------------------------------------------------------------

            CboExtincion.DataSource = vistaTPC
            CboExtincion.DataTextField = "nombre"
            CboExtincion.DataValueField = "codigo"
            CboExtincion.DataBind()
        End If
    End Sub

    Private Sub LoadcboAlcance()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from TIPOS_LEVANTAMIENTO  order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTPC As New DataTable
        Adaptador.Fill(dtTPC)
        If dtTPC.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTPC As DataRow = dtTPC.NewRow()
            filaTPC("codigo") = 0
            filaTPC("nombre") = ""
            dtTPC.Rows.Add(filaTPC)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTPC As DataView = New DataView(dtTPC)
            vistaTPC.Sort = "codigo"
            '--------------------------------------------------------------------

            cboAlcance.DataSource = vistaTPC
            cboAlcance.DataTextField = "nombre"
            cboAlcance.DataValueField = "codigo"
            cboAlcance.DataBind()
        End If
    End Sub

    Private Sub LoadcboDecreto()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select * from TIPOS_DECRETO_COACTIVO"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTPC As New DataTable
        Adaptador.Fill(dtTPC)
        If dtTPC.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTPC As DataRow = dtTPC.NewRow()
            filaTPC("codigo") = 0
            filaTPC("nombre") = ""
            dtTPC.Rows.Add(filaTPC)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTPC As DataView = New DataView(dtTPC)
            vistaTPC.Sort = "codigo"
            '--------------------------------------------------------------------

            CboDecreto.DataSource = vistaTPC
            CboDecreto.DataTextField = "nombre"
            CboDecreto.DataValueField = "codigo"
            CboDecreto.DataBind()
        End If
    End Sub

    Protected Sub cmdSaveSuspension_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveSuspension.Click
        Dim ID As String = Request("ID")

        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp, CausalSusp, NroResSusp, FecResSusp, ObservacSusp VALUES @NroExp, @CausalSusp, @NroResSusp, @FecResSusp, @ObservacSusp) "
        Dim UpdateSQL As String = "UPDATE COACTIVO SET CausalSusp = @CausalSusp, NroResSusp = @NroResSusp, FecResSusp = @FecResSusp, ObservacSusp = @ObservacSusp WHERE NroExp = @NroExp "

        'Si el registro esta en la tabla COACTIVO existe => UPDATE, sino INSERT
        If ExisteRegistroCoactivo(ID) Then
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If


        If cboCausalSusp.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@CausalSusp", cboCausalSusp.SelectedValue.Trim)
        Else
            Command.Parameters.AddWithValue("@CausalSusp", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroResSusp", txtNroResSusp.Text)

        If IsDate(Left(txtFecResSusp.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResSusp", Left(txtFecResSusp.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResSusp", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@ObservacSusp", taObsSuspension.InnerHtml)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de suspensión de procesos", "Expediente " & ID, Command)
            CustomValidatorSUSPENSION.Text = "Datos de suspensión almacenados con éxito"
            CustomValidatorSUSPENSION.IsValid = False

        Catch ex As Exception
            Dim Msg As String
            Msg = ex.Message.Trim
            CustomValidatorSUSPENSION.Text = Msg
            CustomValidatorSUSPENSION.IsValid = False

        End Try

        Connection.Close()
    End Sub

    Protected Sub imgBtnBorraFecResSusp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecResSusp.Click
        txtFecResSusp.Text = ""
    End Sub

    Protected Sub cmdSaveTerminacionCoactivo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveTerminacionCoactivo.Click
        Dim ID As String = Request("ID")

        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        'Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp, CausalFinPro, NroResFinPro, FecResFinPro ) VALUES ( @NroExp, @CausalFinPro, @NroResFinPro, @FecResFinPro) "
        'Dim UpdateSQL As String = "UPDATE COACTIVO SET CausalFinPro = @CausalFinPro, NroResFinPro = @NroResFinPro, FecResFinPro = @FecResFinPro WHERE NroExp = @NroExp "

        Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp, CausalFinPro) VALUES ( @NroExp, @CausalFinPro) "
        Dim UpdateSQL As String = "UPDATE COACTIVO SET CausalFinPro = @CausalFinPro WHERE NroExp = @NroExp "

        'Si el registro esta en la tabla COACTIVO existe => UPDATE, sino INSERT
        If ExisteRegistroCoactivo(ID) Then
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If

        If cboCausalFinPro.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@CausalFinPro", cboCausalFinPro.SelectedValue.Trim)
        Else
            Command.Parameters.AddWithValue("@CausalFinPro", DBNull.Value)
        End If

        'Command.Parameters.AddWithValue("@NroResFinPro", txtNroResFinPro.Text.Trim)

        'If IsDate(Left(txtFecResFinPro.Text.Trim, 10)) Then
        '    Command.Parameters.AddWithValue("@FecResFinPro", Left(txtFecResFinPro.Text.Trim, 10))
        'Else
        '    Command.Parameters.AddWithValue("@FecResFinPro", DBNull.Value)
        'End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de coactivo", "Expediente " & ID, Command)
            CustomValidatorTerminacionCoactivo.Text = "Datos guardados con éxito"
            CustomValidatorTerminacionCoactivo.IsValid = False

        Catch ex As Exception
            Dim Msg As String
            Msg = ex.Message.Trim
            CustomValidatorTerminacionCoactivo.Text = Msg
            CustomValidatorTerminacionCoactivo.IsValid = False
        End Try

        Connection.Close()
    End Sub

    Protected Sub cmdSaveLiquidacionCredito_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveLiquidacionCredito.Click
        Dim ID As String = Request("ID")

        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp, NroResLiquiCred, FecResLiquiCred,LiqCredCapital, LiqCredInteres, LiqCredTotal, LiqCredFecCorte, NroOfiCitCorLC, FecOfiCitCorLC, FecNotCorLC, FecPubAvisoLC, FecRadObj, NroRad, NroResApLiq, FecResApLiq, NroOfiCorLD, FecOfiCorLD, FecNotLD, FecPubLD ) VALUES ( @NroExp, @NroResLiquiCred, @FecResLiquiCred, @LiqCredCapital, @LiqCredInteres, @LiqCredTotal, @LiqCredFecCorte, @NroOfiCitCorLC, @FecOfiCitCorLC, @FecNotCorLC, @FecPubAvisoLC, @FecRadObj, @NroRad, @NroResApLiq, @FecResApLiq, @NroOfiCorLD, @FecOfiCorLD, @FecNotLD, @FecPubLD) "
        Dim UpdateSQL As String = "UPDATE COACTIVO SET NroResLiquiCred = @NroResLiquiCred, FecResLiquiCred = @FecResLiquiCred, LiqCredCapital = @LiqCredCapital, LiqCredInteres = @LiqCredInteres, LiqCredTotal = @LiqCredTotal, LiqCredFecCorte = @LiqCredFecCorte, NroOfiCitCorLC = @NroOfiCitCorLC, FecOfiCitCorLC = @FecOfiCitCorLC, FecNotCorLC = @FecNotCorLC, FecPubAvisoLC = @FecPubAvisoLC, FecRadObj = @FecRadObj, NroRad = @NroRad, NroResApLiq = @NroResApLiq, FecResApLiq = @FecResApLiq, NroOfiCorLD = @NroOfiCorLD, FecOfiCorLD = @FecOfiCorLD, FecNotLD = @FecNotLD, FecPubLD = @FecPubLD WHERE NroExp = @NroExp "

        'Si el registro esta en la tabla COACTIVO existe => UPDATE, sino INSERT
        If ExisteRegistroCoactivo(ID) Then
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If

        Command.Parameters.AddWithValue("@NroResLiquiCred", txtNroResLiquiCred.Text)

        If IsDate(Left(txtFecResLiquiCred.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResLiquiCred", Left(txtFecResLiquiCred.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResLiquiCred", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroOfiCitCorLC", txtNroOfiCitCorLC.Text)

        If IsDate(Left(txtFecOfiCitCorLC.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecOfiCitCorLC", Left(txtFecOfiCitCorLC.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecOfiCitCorLC", DBNull.Value)
        End If

        If IsDate(Left(txtFecNotCorLC.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecNotCorLC", Left(txtFecNotCorLC.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecNotCorLC", DBNull.Value)
        End If

        If IsDate(Left(txtFecPubAvisoLC.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPubAvisoLC", Left(txtFecPubAvisoLC.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecPubAvisoLC", DBNull.Value)
        End If

        If IsDate(Left(txtFecRadObj.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecRadObj", Left(txtFecRadObj.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecRadObj", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroRad", txtNroRad.Text.Trim)
        Command.Parameters.AddWithValue("@NroResApLiq", txtNroResApLiq.Text.Trim)

        If IsDate(Left(txtFecResApLiq.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResApLiq", Left(txtFecResApLiq.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResApLiq", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroOfiCorLD", txtNroOfiCorLD.Text)

        If IsDate(Left(txtFecOfiCorLD.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecOfiCorLD", Left(txtFecOfiCorLD.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecOfiCorLD", DBNull.Value)
        End If

        If IsDate(Left(txtFecNotLD.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecNotLD", Left(txtFecNotLD.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecNotLD", DBNull.Value)
        End If

        If IsDate(Left(txtFecPubLD.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPubLD", Left(txtFecPubLD.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecPubLD", DBNull.Value)
        End If

        '15/jul/2014
        Command.Parameters.AddWithValue("@LiqCredCapital", txtLiqCredCapital.Text.Trim)
        Command.Parameters.AddWithValue("@LiqCredInteres", txtLiqCredInteres.Text.Trim)

        'El campo disabled no muestra el valor en el lado servidor
        Dim Total As Int64
        Total = CLng(txtLiqCredCapital.Text.Trim) + CLng(txtLiqCredInteres.Text.Trim)

        'Command.Parameters.AddWithValue("@LiqCredTotal", txtLiqCredTotal.Text.Trim)
        Command.Parameters.AddWithValue("@LiqCredTotal", Total)

        If IsDate(Left(txtLiqCredFecCorte.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@LiqCredFecCorte", Left(txtLiqCredFecCorte.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@LiqCredFecCorte", DBNull.Value)
        End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de coactivo", "Expediente " & ID, Command)
            CustomValidatorLiquidacionCredito.Text = "Datos guardados con éxito"
            CustomValidatorLiquidacionCredito.IsValid = False

        Catch ex As Exception
            Dim Msg As String
            Msg = ex.Message
            CustomValidatorLiquidacionCredito.Text = ex.Message
            CustomValidatorLiquidacionCredito.IsValid = False

        End Try

        Connection.Close()
    End Sub

    Protected Sub cmdSaveOrdenEjecucion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveOrdenEjecucion.Click

        If (txtNroResolEjec.Text.Trim = "" And txtFecResolEjec.Text.Trim = "") Or
            txtNroResolEjec.Text.Trim.ToUpper = "NA" Or txtNroResolEjec.Text.Trim.ToUpper = "NO APLICA" Or
            txtNroResolEjec.Text.Trim.ToUpper = "SIN DATOS" Then

            CustomValidatorOrdenEjecucion.Text = "No se puede almacenar ningún dato adicional hasta que genere la Resolución que Ordena Continuar La Ejecución."
            CustomValidatorOrdenEjecucion.IsValid = False
            Return
        End If


        Dim ID As String = Request("ID")

        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp, NroResolEjec, FecResolEjec, NroOfiCitaCor, FecOfiCitaCor, FecNotifCor, FecPubAviso) VALUES ( @NroExp, @NroResolEjec, @FecResolEjec, @NroOfiCitaCor, @FecOfiCitaCor, @FecNotifCor, @FecPubAviso)"
        Dim UpdateSQL As String = "UPDATE COACTIVO SET NroResolEjec = @NroResolEjec, FecResolEjec = @FecResolEjec, NroOfiCitaCor = @NroOfiCitaCor, FecOfiCitaCor = @FecOfiCitaCor, FecNotifCor = @FecNotifCor, FecPubAviso = @FecPubAviso WHERE NroExp = @NroExp"

        'Si el registro esta en la tabla COACTIVO existe => UPDATE, sino INSERT
        If ExisteRegistroCoactivo(ID) Then
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If

        Command.Parameters.AddWithValue("@NroResolEjec", txtNroResolEjec.Text)

        If IsDate(Left(txtFecResolEjec.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResolEjec", Left(txtFecResolEjec.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResolEjec", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroOfiCitaCor", txtNroOfiCitaCor.Text)

        If IsDate(Left(txtFecOfiCitaCor.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecOfiCitaCor", Left(txtFecOfiCitaCor.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecOfiCitaCor", DBNull.Value)
        End If

        If IsDate(Left(txtFecNotifCor.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecNotifCor", Left(txtFecNotifCor.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecNotifCor", DBNull.Value)
        End If

        If IsDate(Left(txtFecPubAviso.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecPubAviso", Left(txtFecPubAviso.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecPubAviso", DBNull.Value)
        End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de coactivo", "Expediente " & ID, Command)
            CustomValidatorOrdenEjecucion.Text = "Datos guardados con éxito"
            CustomValidatorOrdenEjecucion.IsValid = False

        Catch ex As Exception
            Dim Msg As String
            Msg = ex.Message
            CustomValidatorOrdenEjecucion.Text = ex.Message
            CustomValidatorOrdenEjecucion.IsValid = False

        End Try

        Connection.Close()
    End Sub

    Public Function ContarExcepciones(ByVal pExpediente As String) As Integer
        Dim NumExcepciones As Integer = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT COUNT(NroRad) AS NumExcepciones FROM excepciones " &
            "WHERE NroExp = '" & pExpediente & "'"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NumExcepciones = Reader("NumExcepciones").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return NumExcepciones

    End Function

    Protected Sub cmdSaveLevantamiento_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveLevantamiento.Click

        If (cboTipoLevMC.SelectedValue = "0") Then
            CustomValidator4.Text = "Seleccione el tipo de resolución por favor"
            CustomValidator4.IsValid = False
            Return

        End If

        Dim ID As String = Request("ID")
        If String.IsNullOrEmpty(ID) Then
            Exit Sub
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp,  TipoLevMC) VALUES ( @NroExp, @TipoLevMC) "
        Dim UpdateSQL As String = "UPDATE COACTIVO SET TipoLevMC = @TipoLevMC WHERE NroExp = @NroExp "

        'Si el registro esta en la tabla COACTIVO existe => UPDATE, sino INSERT
        If ExisteRegistroCoactivo(ID) Then
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        Else
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", ID)
        End If

        Command.Parameters.AddWithValue("@TipoLevMC", cboTipoLevMC.SelectedValue)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de Levntamiento de medidas cautelares", "Expediente " & ID, Command)
            CustomValidator4.Text = "Datos guardados con éxito"
            CustomValidator4.IsValid = False

        Catch ex As Exception
            Dim Msg As String
            Msg = ex.Message
            CustomValidator4.Text = ex.Message
            CustomValidator4.IsValid = False

        End Try

        Connection.Close()
    End Sub

    Protected Sub imgBtnBorraFecRes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecRes.Click
        txtFecRes.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecadmis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecadmis.Click
        txtFecadmis.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecFijAvisoAdm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecFijAvisoAdm.Click
        txtFecFijAvisoAdm.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecDesfAvisoAdm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecDesfAvisoAdm.Click
        txtFecDesfAvisoAdm.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecLimPresCred_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecLimPresCred.Click
        txtFecLimPresCred.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecOfiPres_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecOfiPres.Click
        txtFecOfiPres.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecPres_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecPres.Click
        txtFecPres.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecTrasProy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecTrasProy.Click
        txtFecTrasProy.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecLimPreObj_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecLimPreObj.Click
        txtFecLimPreObj.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecOfiObj_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecOfiObj.Click
        txtFecOfiObj.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecPresObj_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecPresObj.Click
        txtFecPresObj.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecDecObj_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecDecObj.Click
        txtFecDecObj.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecRecRep_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecRecRep.Click
        txtFecRecRep.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecDemSupInt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecDemSupInt.Click
        txtFecDemSupInt.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecPresAcu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecPresAcu.Click
        txtFecPresAcu.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecAudConf_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecAudConf.Click
        txtFecAudConf.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecTermAcu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecTermAcu.Click
        txtFecTermAcu.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecResAcu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecResAcu.Click
        txtFecResAcu.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecIniPagAcu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecIniPagAcu.Click
        txtFecIniPagAcu.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecFinPagAcu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecFinPagAcu.Click
        txtFecFinPagAcu.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecOfiIncump_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecOfiIncump.Click
        txtFecOfiIncump.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecPresOfiInc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecPresOfiInc.Click
        txtFecPresOfiInc.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecAudInc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecAudInc.Click
        txtFecAudInc.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecPresDemSSInc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecPresDemSSInc.Click
        txtFecPresDemSSInc.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecFinConCursal_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecFinConCursal.Click
        txtFecFinConCursal.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecTrasConcursal_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecTrasConcursal.Click
        txtFecTrasConcursal.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecInsCamCom_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecInsCamCom.Click
        txtFecInsCamCom.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecFinCobConcursal_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecFinCobConcursal.Click
        txtFecFinCobConcursal.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecEnvioCC_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecEnvioCC.Click
        txtFecEnvioCC.Text = ""
    End Sub

    'Protected Sub imgBtnBorraFecIniCC_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecIniCC.Click
    '    txtFecIniCC.Text = ""
    'End Sub

    Protected Sub imgBtnBorraFecFinCC_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecFinCC.Click
        txtFecFinCC.Text = ""
    End Sub

    Private Sub imgBtnBorraFecTerm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecTerm.Click
        txtFecTerm.Text = ""
    End Sub

    Private Sub MostrarSancion(ByVal mostar As Boolean)
        trsancion.Visible = mostar
    End Sub

    Private Function PrimerOficioPersuasivo(ByVal pExpediente As String) As DataTable
        Return Funciones.RetornaCargadatos("select   min(FecOfi) pFechaOficio from PERSUASIVOOFICIOS where nroexp = '" & pExpediente & "'")
    End Function

    Private Function PrimeraLlamadaPersuasivo(ByVal pExpediente As String) As DataTable
        Return Funciones.RetornaCargadatos("select min(fecha) pFechaLlamada   from persuasivollamadas where nroexp = '" & pExpediente & "'")
    End Function

    Private Function FechafinPersuasivo(ByVal pExpediente As String) As DataTable
        Return Funciones.RetornaCargadatos("SELECT  max(fecha) fechaUltiEstado from CAMBIOS_ESTADO where NROEXP = '" & pExpediente & "' and  estado in ('03','02') ")
    End Function

    Private Function DeterminarEstadoActualDeuda(ByVal pagoCapitalTxt As String, ByVal capitalInicialTxt As String) As String
        Dim pagoCapital = Convert.ToDouble(pagoCapitalTxt.Trim)
        Dim capitalInicial = Convert.ToDouble(capitalInicialTxt.Trim)

        If pagoCapital = capitalInicial Then
            Return "PAGO TOTAL"
        ElseIf pagoCapital < capitalInicial Then
            Return "PAGO PARCIAL"
        ElseIf pagoCapital = 0 Then
            Return "SIN PAGOS"
        Else
            Return "-"
        End If
    End Function

    Protected Sub btnSolicitarSuspension_Click(sender As Object, e As EventArgs) Handles btnSolicitarSuspension.Click
        mp2.Show()
    End Sub

    Protected Sub cmdSuspender_Click(sender As Object, e As EventArgs) Handles cmdSuspender.Click
        Dim observacion As String = txtObservacionSuspension.Text
        Dim pExpediente As String = ""
        Dim tareaAsignadaBLL As TareaAsignadaBLL = New TareaAsignadaBLL
        Dim tareaObservacionBLL As TareaObservacionBLL = New TareaObservacionBLL
        Dim registros As TareaAsignada = New TareaAsignada
        Dim tareaObservacion As TareaObservacion = New TareaObservacion
        Dim id_tarea As Int32
        If Len(Request("ID")) > 0 Then
            pExpediente = Request("ID").Trim
            registros = tareaAsignadaBLL.obtenerTareaAsignadaPorIdExpediente(pExpediente)
            id_tarea = registros.ID_TAREA_ASIGNADA
            tareaObservacion.COD_ID_TAREA_OBSERVACION = id_tarea
            tareaObservacion.OBSERVACION = observacion
            tareaObservacion.IND_ESTADO = 1
            tareaObservacion.FEC_CREACION = DateTime.Now
            tareaObservacionBLL.crearTareaObservacion(tareaObservacion)
            tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(id_tarea, 14)
            mp2.Hide()
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), False)
        End If
    End Sub

    Private Sub AsignarGestion()
        Dim pExpediente As String = ""
        Dim tareaAsignadaBLL As TareaAsignadaBLL = New TareaAsignadaBLL
        Dim registros As TareaAsignada = New TareaAsignada
        If Len(Request("ID")) > 0 Then
            pExpediente = Request("ID").Trim
            registros = tareaAsignadaBLL.obtenerTareaAsignadaPorIdExpediente(pExpediente)
            If registros IsNot Nothing Then
                If (registros.COD_ESTADO_OPERATIVO = 11 AndAlso registros.VAL_USUARIO_NOMBRE = Session("ssloginusuario")) Then
                    tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("idtareasig=" & registros.ID_TAREA_ASIGNADA & ",estadoope=15"))
                    tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(registros.ID_TAREA_ASIGNADA, 15)
                End If
            End If
        End If
    End Sub

    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "Tarea asignada"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function

End Class