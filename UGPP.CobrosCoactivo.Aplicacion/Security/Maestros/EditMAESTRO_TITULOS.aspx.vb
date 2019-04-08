Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class EditMAESTRO_TITULOS
    Inherits System.Web.UI.Page

    Dim documentoTipoTituloBLL As DocumentoTipoTituloBLL
    Dim tipoCarteraBLL As TipoCarteraBLL

    Protected Overrides Sub OnInit(e As EventArgs)
        tipoCarteraBLL = New TipoCarteraBLL()
        documentoTipoTituloBLL = New DocumentoTipoTituloBLL()
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            LoadcboMT_tipoRevocatoria()
            LoadcboMT_tiposentencia()
            LoadcboMT_tipo_titulo()
            LoadcboMT_for_notificacion_titulo()
            LoadcboMT_for_not_reso_resu_reposicion()
            LoadcboMT_for_not_reso_apela_recon()
            LoadcboPROCEDENCIA()
            LoadcboCausalDevol()

            If (Session("mnivelacces") = 4) Then ' Para el perfil gestor 
                Me.FechaEjecutoriaObligatoria.Visible = True
                Me.FechaExiLiqObligatoria.Visible = True


            Else


                Me.FechaEjecutoriaObligatoria.Visible = False
                Me.FechaEjecutoriaObligatoria.Visible = False

            End If


            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM MAESTRO_TITULOS where MT_nro_titulo = @MT_nro_titulo AND MT_expediente = @MT_expediente"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @MT_nro_titulo parameter in the Command select query
                Command.Parameters.AddWithValue("@MT_nro_titulo", Request("ID"))
                Command.Parameters.AddWithValue("@MT_expediente", Request("pExpediente"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    'Mostrar datos del titulo ejecutivo
                    txtMT_nro_titulo.Text = Reader("MT_nro_titulo").ToString()
                    cboMT_tipo_titulo.SelectedValue = Reader("MT_tipo_titulo").ToString()

                    'Procedencia del titulo
                    cboPROCEDENCIA.SelectedValue = Reader("MT_procedencia").ToString
                    HdnIdunico.Value = Reader("idunico").ToString
                    LoadTipo_Cartera()
                    loadDocumentos()
                    txtMT_titulo_acumulado.Text = Reader("MT_titulo_acumulado").ToString()
                    txtMT_fec_expedicion_titulo.Text = Left(Reader("MT_fec_expedicion_titulo").ToString().Trim, 10)
                    txtMT_fec_notificacion_titulo.Text = Left(Reader("MT_fec_notificacion_titulo").ToString().Trim, 10)
                    cboMT_for_notificacion_titulo.SelectedValue = Reader("MT_for_notificacion_titulo").ToString()
                    txtMT_res_resuelve_reposicion.Text = Reader("MT_res_resuelve_reposicion").ToString()
                    txtMT_fec_expe_resolucion_reposicion.Text = Left(Reader("MT_fec_expe_resolucion_reposicion").ToString().Trim, 10)
                    txtMT_fec_not_reso_resu_reposicion.Text = Left(Reader("MT_fec_not_reso_resu_reposicion").ToString().Trim, 10)
                    cboMT_for_not_reso_resu_reposicion.SelectedValue = Reader("MT_for_not_reso_resu_reposicion").ToString()
                    txtMT_reso_resu_apela_recon.Text = Reader("MT_reso_resu_apela_recon").ToString()
                    txtMT_fec_exp_reso_apela_recon.Text = Left(Reader("MT_fec_exp_reso_apela_recon").ToString().Trim, 10)
                    txtMT_fec_not_reso_apela_recon.Text = Left(Reader("MT_fec_not_reso_apela_recon").ToString().Trim, 10)
                    cboMT_for_not_reso_apela_recon.SelectedValue = Reader("MT_for_not_reso_apela_recon").ToString()
                    cboMT_for_not_reso_apela_recon.SelectedValue = Reader("MT_for_not_reso_apela_recon").ToString()
                    txtEFIEXPORIGEN.Text = Reader("NoExpedienteOrigen").ToString()

                    '/-----------------------------------------------------------------  
                    'ID _HU:  004
                    'Nombre HU   : Ajuste a los permisos del  perfil Revisor 
                    'Empresa: UT TECHNOLOGY 
                    'Autor: Jeisson Gómez 
                    'Fecha: 11-01-2017  
                    'Objetivo : Asignar el valor de las fechas ejecutoria y exigibilidad obligatoria. 
                    '------------------------------------------------------------------/
                    txtMT_fecha_ejecutoriaObli.Text = Left(Reader("MT_fecha_ejecutoria").ToString().Trim, 10)
                    txtMT_fec_exi_liqObli.Text = Left(Reader("MT_fec_exi_liq").ToString().Trim, 10)
                    ' Jeisson Gómez 

                    txtMT_fec_cad_presc.Text = Left(Reader("MT_fec_cad_presc").ToString().Trim, 10)

                    'Mostrar datos de la devolucion si existe
                    txtNumMemoDev.Text = Reader("NumMemoDev").ToString().Trim
                    txtFecMemoDev.Text = Left(Reader("FecMemoDev").ToString().Trim, 10)
                    cboCausalDevol.SelectedValue = Reader("CausalDevol").ToString().Trim
                    taObsDevol.InnerHtml = Reader("ObsDevol").ToString().Trim
                    cargarNotificaciones()
                    ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
                    ' Jeisson Gómez 17/02/2016
                    ' txtcapitalmulta.Text = Reader("capitalmulta").ToString

                    MostarDeuda(Reader("MT_tipo_titulo").ToString().Trim)
                    'Mostrar informacion de la deuda del titulo
                    Dim datosDeuda As DataTable = Informacion_deuda(Request("pExpediente"))

                    If datosDeuda.Rows.Count > 0 Then
                        Inicializardeuda()
                        For Each row As DataRow In datosDeuda.Rows
                            If row("ID_GRUPO") = "1" Then

                                ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
                                ' Jeisson Gómez 17/02/2016

                                '    txttotalomisos.Text = datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='1'")

                                '    If row("SUBSISTEMA") = "SENA" Then
                                '        txtomisossena.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "PENSION" Then
                                '        txtomisospensiones.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "ARL" Then
                                '        txtomisosarl.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "ICBF" Then
                                '        txtomisosicbf.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "CCF" Then
                                '        txtomisossubfamiliar.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "SALUD" Then
                                '        txtomisossalud.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "FSP" Then
                                '        txtomisosfondosolpen.Text = row("DEUDA")
                                '    End If

                                'ElseIf row("ID_GRUPO") = "2" Then
                                '    txttotalinexactos.Text = datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='2'")
                                '    If row("SUBSISTEMA") = "SENA" Then
                                '        txtinexactossena.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "PENSION" Then
                                '        txtinexactospensiones.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "ARL" Then
                                '        txtinexactosarl.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "ICBF" Then
                                '        txtinexactosicbf.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "CCF" Then
                                '        txtinexactossubfamiliar.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "SALUD" Then
                                '        txtinexactossalud.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "FSP" Then
                                '        txtinexactosfondosolpen.Text = row("DEUDA")
                                '    End If

                                'ElseIf row("ID_GRUPO") = "3" Then
                                '    txttotalmora.Text = datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='3'")
                                '    If row("SUBSISTEMA") = "SENA" Then
                                '        txtmorasena.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "PENSION" Then
                                '        txtmorapensiones.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "ARL" Then
                                '        txtmoraarl.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "ICBF" Then
                                '        txtmoraicbf.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "CCF" Then
                                '        txtmorasubfamiliar.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "SALUD" Then
                                '        txtmorasalud.Text = row("DEUDA")
                                '    ElseIf row("SUBSISTEMA") = "FSP" Then
                                '        txtmorafondosolpen.Text = row("DEUDA")
                                '    End If

                            End If
                        Next
                        txtTotalDeuda.Text = datosDeuda.Compute("SUM(DEUDA)", String.Empty)
                    Else
                        Inicializardeuda()
                        If Session("mnivelacces") <> 5 Then 'Nivel 5 = Repartidor
                            If cboMT_tipo_titulo.SelectedValue = "01" Or cboMT_tipo_titulo.SelectedValue = "04" Then
                                CustomValidator1.Text = "No se pudo cargar la información de la deuda ya que no se encontro el SQL asociado al expediente. "
                                CustomValidator1.IsValid = False
                            End If
                        End If
                        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
                        ' Jeisson Gómez 17/02/2016
                        'txtcapitalmulta.Text = Reader("capitalmulta").ToString
                        '/-----------------------------------------------------------------  
                        'ID _HU:  012
                        'Nombre HU   : Desagregación de Obligaciones 
                        'Empresa: UT TECHNOLOGY 
                        'Autor: Jeisson Gómez 
                        'Fecha: 06-01-2017  
                        'Objetivo : Realizar la desagregación de las obligaciones 
                        '------------------------------------------------------------------/
                        txtValorObligacion.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_valor_obligacion")), 0, Reader("MT_valor_obligacion")))
                        txtPartidaGlobal.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_partida_global")), 0, Reader("MT_partida_global")))
                        txtSancionOmision.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_sancion_omision")), 0, Reader("MT_sancion_omision")))  'IIf(IsDBNull(datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='1'")), 0, datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='1'"))
                        txtSancionMora.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_sancion_mora")), 0, Reader("MT_sancion_mora")))  'IIf(IsDBNull(datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='3'")), 0, datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='3'"))
                        txtSancionInexactitud.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_sancion_inexactitud")), 0, Reader("MT_sancion_inexactitud"))) 'IIf(IsDBNull(datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='2'")), 0, datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='2'"))
                        txtTotalSancion.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_sancion_omision")), 0, Reader("MT_sancion_omision"))) + Decimal.Parse(IIf(IsDBNull(Reader("MT_sancion_mora")), 0, Reader("MT_sancion_mora")) + Decimal.Parse(IIf(IsDBNull(Reader("MT_sancion_inexactitud")), 0, Reader("MT_sancion_inexactitud"))))
                        txtTotalObligacion.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_total_obligacion")), 0, Reader("MT_total_obligacion")))
                        txtTotalPartidaGlobal.Text = Decimal.Parse(IIf(IsDBNull(Reader("MT_total_partida_global")), 0, Reader("MT_total_partida_global")))
                        txtTotalDeuda.Text = Decimal.Parse(txtTotalSancion.Text) + Decimal.Parse(txtTotalObligacion.Text) + Decimal.Parse(txtTotalPartidaGlobal.Text) ' Reader("totaldeuda").ToString
                    End If

                    ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
                    ' Jeisson Gómez 17/02/2016
                    'Mostrar info de la deuda del titulo
                    'txtsentenciasjudiciales.Text = Reader("sentenciasjudiciales").ToString
                    'txtcuotaspartesacum.Text = Reader("cuotaspartesacum").ToString
                    'txttotalmultas.Text = Reader("totalmultas").ToString
                    'txttotalsentencias.Text = Reader("totalsentencias").ToString
                    'txtTotalRepartidor.Text = Reader("totalrepartidor").ToString
                    txtTotalSancion.Text = IIf(String.IsNullOrEmpty(Reader("MT_totalSancion").ToString), 0, Reader("MT_totalSancion").ToString)

                Else
                    Inicializardeuda()
                End If
                'Close the Data Reader we are done with it.
                Reader.Close()
                Connection.Close()
            Else
                Inicializardeuda()
            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            If Session("mnivelacces") <> 5 And Session("mnivelacces") <> 8 Then
                Dim mtg As New MetodosGlobalesCobro
                Dim idGestorResp As String = mtg.GetIDGestorResp(Request("pExpediente"))
                '/-----------------------------------------------------------------  
                'ID _HU:  012
                'Nombre HU   : Desagregación de Obligaciones 
                'Empresa: UT TECHNOLOGY 
                'Autor: Jeisson Gómez 
                'Objetivo : Se pone false la propiedad Visible del botón guardar según requerimiento 
                ' 9.	Validar que la modificación de los campos indicados en la Fig. 1 se realice únicamente en el Rol Repartidor.
                '/-----------------------------------------------------------------  
                cmdSave2.Visible = False
                If idGestorResp <> Session("sscodigousuario") Then
                    'Primer boton de guardar
                    cmdSave.Visible = False
                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten guardar datos"
                    CustomValidator1.IsValid = False
                    '
                    cmdSave2.Visible = False
                    CustomValidator2.Text = "Este expediente está a cargo de otro gestor. No permiten guardar datos"
                    CustomValidator2.IsValid = False
                    '
                    cmdSave3.Visible = False
                    CustomValidator3.Text = "Este expediente está a cargo de otro gestor. No permiten guardar datos"
                    CustomValidator3.IsValid = False

                    btnrevocatoria.Visible = False
                    CustomValidator4.Text = "Este expediente está a cargo de otro gestor. No permiten guardar datos"
                    CustomValidator4.IsValid = False

                End If
            End If

            'Activar/Inactivar controles en funcion del usuario logueado
            ActivarControles()
            If txtMT_nro_titulo.Text.Trim = "" Then
                txtMT_nro_titulo.Enabled = True
            Else
                txtMT_nro_titulo.Enabled = False
            End If
        End If
    End Sub
    Public Function ReturnStringDate(ByVal dateitem As Date?) As String
        If dateitem IsNot Nothing Then
            If dateitem <= Date.Parse("1/1/1753 12:00:00 AM") Then
                Return ""
            Else
                Return Left(dateitem.ToString().Trim, 10)
            End If
        End If
        Return ""
    End Function
    Private Sub cargarNotificaciones()
        Dim MetodoSel As ValidadorBLL = New ValidadorBLL()
        Dim tituloEjecutivoObj = MetodoSel.ConsultarTituloNotificacionestivo(HdnIdunico.Value).TituloEjecutivo
        txtMT_fec_expedicion_titulo.Text = ReturnStringDate(tituloEjecutivoObj.fechaTituloEjecutivo)
        txtMT_fec_notificacion_titulo.Text = ReturnStringDate(tituloEjecutivoObj.fechaNotificacion)
        If String.IsNullOrEmpty(tituloEjecutivoObj.formaNotificacion) = False Then
            cboMT_for_notificacion_titulo.SelectedValue = tituloEjecutivoObj.formaNotificacion.ToString()
        End If

        If String.IsNullOrEmpty(tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.formaNotificacion) = False Then
            cboMT_for_not_reso_resu_reposicion.SelectedValue = tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.formaNotificacion
        End If

        txtMT_fec_expe_resolucion_reposicion.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo)
        txtMT_fec_not_reso_resu_reposicion.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReposicion.fechaNotificacion)

        If String.IsNullOrEmpty(tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.formaNotificacion) = False Then
            cboMT_for_not_reso_apela_recon.SelectedValue = tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.formaNotificacion
        End If
        txtMT_fec_exp_reso_apela_recon.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo)
        txtMT_fec_not_reso_apela_recon.Text = ReturnStringDate(tituloEjecutivoObj.tituloEjecutivoRecursoReconsideracion.fechaNotificacion)

    End Sub

    Private Sub LoadTipo_Cartera()
        cboTipo_Cartera.DataSource = tipoCarteraBLL.consultarTiposCartera(Int32.Parse(cboPROCEDENCIA.SelectedValue))
        cboTipo_Cartera.DataTextField = "DEC_TIPO_CARTERA"
        cboTipo_Cartera.DataValueField = "ID_TIPO_CARTERA"
        cboTipo_Cartera.DataBind()
    End Sub
    Private Sub loadDocumentos()
        Dim LstDocumentos As List(Of DocumentoTipoTitulo) = documentoTipoTituloBLL.consultarDocumentosPorTipo(cboMT_tipo_titulo.SelectedValue)

        Dim MetodoSel As ValidadorBLL = New ValidadorBLL()
        Dim LstDocumentosMaestro As List(Of DocumentoMaestroTitulo) = MetodoSel.ConsultarDocumentos(HdnIdunico.Value)

        If LstDocumentosMaestro.Count() > 0 Then
            For Each element As DocumentoTipoTitulo In LstDocumentos
                Dim DocumentoMaestro = LstDocumentosMaestro.FirstOrDefault(Function(x) (x.ID_DOCUMENTO_TITULO) = element.ID_DOCUMENTO_TITULO)
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

        lsvListaDocumentos.DataSource = LstDocumentos
        lsvListaDocumentos.DataBind()
    End Sub
    Private Sub ActivarControles()
        Dim Habilitado As Boolean
        If Session("mnivelacces") = 5 Or Session("mnivelacces") = 8 Then
            ' Es el repartidor. Solo activar los siguientes campos 
            '--30/07/2014. Tambien activar si es el gestor de informacion

            txtMT_nro_titulo.Enabled = True
            txtMT_fec_expedicion_titulo.Enabled = True
            ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
            ' Jeisson Gómez 17/02/2016
            'txtTotalRepartidor.Enabled = True
            cboPROCEDENCIA.Enabled = True
            cboMT_tipo_titulo.Enabled = True
            '           
            imgBtnBorraFecExpTit.Visible = True

            ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
            ' Jeisson Gómez 17/02/2016
            'lblLeyendaRepartidor.Visible = True

            ' Los demas se deshabilitan  
            If Session("mnivelacces") = 5 Then
                'tblTotalRepartidor.Visible = True

                Habilitado = False
            Else
                Habilitado = True
            End If
            txtMT_titulo_acumulado.Enabled = Habilitado
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
            cboTipo_Cartera.Enabled = False
            '/-----------------------------------------------------------------  
            'ID _HU:  002 0
            'Nombre HU   : Obligatoriedad de Campos fecha de ejecutoria y fecha de exigibilidad oficial
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 06-01-2017  
            'Objetivo : Hacer obligatorios los campos de fecha ejecutoria y fecha de exigibilidad oficial,
            '           para el perfil Gestor - Abogado
            '------------------------------------------------------------------/

            If (Session("mnivelacces") = 4) Then
                txtMT_fecha_ejecutoriaObli.Enabled = True
                txtMT_fec_exi_liqObli.Enabled = True
                txtMT_fec_exi_liqObli.Enabled = False
            Else
                txtMT_fecha_ejecutoriaObli.Enabled = False
                txtMT_fec_exi_liqObli.Enabled = False
            End If

            '/-----------------------------------------------------------------  
            'ID _HU:  002 0
            'Nombre HU   : Obligatoriedad de Campos fecha de ejecutoria y fecha de exigibilidad oficial
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 06-01-2017  
            'Objetivo : Hacer obligatorios los campos de fecha ejecutoria y fecha de exigibilidad oficial,
            '           para el perfil Gestor - Abogado
            '------------------------------------------------------------------/

            txtMT_fec_cad_presc.Enabled = Habilitado
            'Imagenes de eliminar la fecha del calendario
            imgBtnBorraFecNotTit.Visible = Habilitado
            imgBtnBorraFecExpRR.Visible = Habilitado
            imgBtnBorraFecNotRRR.Visible = Habilitado
            imgBtnBorraFecExpRAR.Visible = Habilitado
            imgBtnBorraFecNotRAR.Visible = Habilitado

        Else
            ' Es un perfil <> de repartidor. Desactivar los siguientes campos
            txtMT_nro_titulo.Enabled = False
            txtMT_fec_expedicion_titulo.Enabled = False
            ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
            ' Jeisson Gómez 17/02/2016
            'txtTotalRepartidor.Enabled = False
            cboPROCEDENCIA.Enabled = False
            cboMT_tipo_titulo.Enabled = False

            ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
            ' Jeisson Gómez 17/02/2016
            ' lblLeyendaRepartidor.Visible = False
            '
            'Imagenes de eliminar la fecha del calendario
            imgBtnBorraFecExpTit.Visible = False

            ' Los demas se habilitan
            txtMT_titulo_acumulado.Enabled = True
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
            txtMT_fec_cad_presc.Enabled = True
            '
            imgBtnBorraFecNotTit.Visible = True
            imgBtnBorraFecExpRR.Visible = True
            imgBtnBorraFecNotRRR.Visible = True
            imgBtnBorraFecExpRAR.Visible = True
            imgBtnBorraFecNotRAR.Visible = True
        End If
    End Sub

    Private Sub LoadcboCausalDevol()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM CAUSALES_DEVOLUCION_TITULO"

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dt1.NewRow()
            filaEstado("codigo") = 0
            filaEstado("nombre") = ""
            dt1.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaCausales As DataView = New DataView(dt1)
            vistaCausales.Sort = "codigo"
            '--------------------------------------------------------------------
            cboCausalDevol.DataSource = vistaCausales
            cboCausalDevol.DataTextField = "nombre"
            cboCausalDevol.DataValueField = "codigo"
            cboCausalDevol.DataBind()
        End If
    End Sub

    Private Sub LoadcboPROCEDENCIA()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM PROCEDENCIA_TITULOS"

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dt1.NewRow()
            filaEstado("codigo") = 0
            filaEstado("nombre") = ""
            dt1.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaProcedencia As DataView = New DataView(dt1)
            vistaProcedencia.Sort = "codigo"
            '--------------------------------------------------------------------
            cboPROCEDENCIA.DataSource = vistaProcedencia
            cboPROCEDENCIA.DataTextField = "nombre"
            cboPROCEDENCIA.DataValueField = "codigo"
            cboPROCEDENCIA.DataBind()
        End If
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

    Protected Sub LoadcboMT_tipoRevocatoria()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT id codigo, descripcion nombre FROM TIPO_REVOCATORIA"
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
            cborevocatoria.DataSource = vistaTabla
            cborevocatoria.DataTextField = "nombre"
            cborevocatoria.DataValueField = "codigo"
            cborevocatoria.DataBind()
        End If

    End Sub

    Protected Sub LoadcboMT_tipo_titulo()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [TIPOS_TITULO] order by nombre"
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
            cboMT_tipo_titulo.DataSource = vistaTabla
            cboMT_tipo_titulo.DataTextField = "nombre"
            cboMT_tipo_titulo.DataValueField = "codigo"
            cboMT_tipo_titulo.DataBind()
        End If
    End Sub

    Protected Sub LoadcboMT_for_notificacion_titulo()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [FORMAS_NOTIFICACION] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTabla As New DataTable
        Adaptador.Fill(dtTabla)
        If dtTabla.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTabla As DataRow = dtTabla.NewRow()
            filaTabla("codigo") = ""
            filaTabla("nombre") = ""
            dtTabla.Rows.Add(filaTabla)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTabla As DataView = New DataView(dtTabla)
            vistaTabla.Sort = "codigo"
            '--------------------------------------------------------------------
            cboMT_for_notificacion_titulo.DataSource = vistaTabla
            cboMT_for_notificacion_titulo.DataTextField = "nombre"
            cboMT_for_notificacion_titulo.DataValueField = "codigo"
            cboMT_for_notificacion_titulo.DataBind()
        End If
    End Sub

    Protected Sub LoadcboMT_for_not_reso_resu_reposicion()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [FORMAS_NOTIFICACION] order by nombre"
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
        Dim cmd As String = "select codigo, nombre from [FORMAS_NOTIFICACION] order by nombre"
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

    Private Function HayDatosDevolucionValidos() As Boolean
        If txtNumMemoDev.Text.Trim = "" And txtFecMemoDev.Text.Trim = "" And cboCausalDevol.SelectedValue.Trim = "0" And taObsDevol.InnerHtml.Trim = "" Then
            Return True
        Else
            ' Hay al menos un dato de devolucion => exigirlos todos (los de devolucion)
            If txtNumMemoDev.Text.Trim = "" Or txtFecMemoDev.Text.Trim = "" Or cboCausalDevol.SelectedValue.Trim = "" Or cboCausalDevol.SelectedValue.Trim = "0" Or taObsDevol.InnerHtml.Trim = "" Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Function AplicaDevolucionExpediente(ByVal pExpediente As String, ByVal pTitulo As String) As Boolean
        Dim Respuesta As Boolean = False
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT MT_nro_titulo, NumMemoDev FROM MAESTRO_TITULOS WHERE MT_expediente = '" + pExpediente + "' AND (RTRIM(NumMemoDev) = '' OR NumMemoDev IS NULL)"

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count = 1 Then
            'Respuesta = True
            For Each row As DataRow In dt1.Rows
                If pTitulo.Trim = row("MT_nro_titulo").ToString.Trim Then
                    Respuesta = True
                Else
                    Respuesta = False
                End If
            Next row
        Else
            If dt1.Rows.Count > 1 Then
                'Recorrer el datatable?
                Respuesta = False
            Else
                ' cero titulos
                Respuesta = True
            End If
        End If
        '
        Return Respuesta
    End Function

    Private Function GetEstadoActualExpediente(ByVal pExpediente As String) As String
        Dim EstadoActualExpediente As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT efiestado FROM ejefisglobal WHERE efinroexp = @efinroexp"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@efinroexp", pExpediente)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            EstadoActualExpediente = Reader("efiestado").ToString()
        End If
        'Cerrar el Data Reader
        Reader.Close()
        Connection.Close()

        'Devolver el estado actual del expediente
        Return EstadoActualExpediente
    End Function

    Private Sub CambiarSoloEstado(ByVal pExpediente As String, ByVal pEstado As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        Dim sql As String = "UPDATE ejefisglobal SET efiestado = '" & pEstado & "' WHERE efinroexp = '" & pExpediente & "'"
        Command = New SqlCommand(sql, Connection)

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Actualizar estado de expediente ", "Expediente " & pExpediente, Command)

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
            Return
        End Try
        Connection.Close()
    End Sub

    Private Sub RegistrarCambioEstado(ByVal pNumExpediente As String, ByVal pRepartidor As String, ByVal pRevisor As String, ByVal pAbogado As String, ByVal pFecha As Date, ByVal pEstado As String, ByVal pEstadoPago As String, ByVal pEstadoOperativo As String, ByVal pEtapaProcesal As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        'Declare string InsertSQL 
        Dim InsertSQL As String = "INSERT INTO CAMBIOS_ESTADO (NroExp, repartidor, revisor, abogado, fecha, estado, estadopago, estadooperativo, etapaprocesal) VALUES (@NroExp, @repartidor, @revisor, @abogado, @fecha, @estado, @estadopago, @estadooperativo, @etapaprocesal)"
        Dim Command As SqlCommand = New SqlCommand(InsertSQL, Connection)

        'Parametros
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Command.Parameters.AddWithValue("@repartidor", pRepartidor)
        Command.Parameters.AddWithValue("@revisor", pRevisor)
        Command.Parameters.AddWithValue("@abogado", pAbogado)
        Command.Parameters.AddWithValue("@fecha", pFecha)
        Command.Parameters.AddWithValue("@estado", pEstado)
        Command.Parameters.AddWithValue("@estadopago", pEstadoPago)
        Command.Parameters.AddWithValue("@estadooperativo", pEstadoOperativo)
        Command.Parameters.AddWithValue("@etapaprocesal", pEtapaProcesal)

        'Insertar el dato en la tabla
        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Registro de cambios de estado ", "Expediente " & pNumExpediente, Command)
        Catch ex As Exception

        End Try

        Connection.Close()
    End Sub

    Private Function HayCambioEstado(ByVal pNumExpediente As String, ByVal pRepartidor As String, ByVal pRevisor As String, ByVal pAbogado As String, ByVal pEstado As String, ByVal pEstadoPago As String, ByVal pEstadoOperativo As String, ByVal pEtapaProcesal As String) As Boolean
        Dim Respuesta As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp AND idunico = (SELECT MAX(idunico) AS UltimoCambio FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp)"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        ' 
        'If at least one record was found
        If Reader.Read Then
            Dim RepartidorActual As String = Reader("repartidor").ToString()
            Dim RevisorActual As String = Reader("revisor").ToString()
            Dim AbogadoActual As String = Reader("abogado").ToString()
            Dim EstadoActual As String = Reader("estado").ToString()
            Dim EstadoPagoActual As String = Reader("estadopago").ToString()
            Dim EstadoOperativoActual As String = Reader("estadooperativo").ToString()
            Dim EtapaProcesalActual As String = Reader("etapaprocesal").ToString()

            'Preguntar si algun campo es diferente
            If RepartidorActual <> pRepartidor Or AbogadoActual <> pAbogado Or EstadoActual <> pEstado Or EstadoPagoActual <> pEstadoPago Or EstadoOperativoActual <> pEstadoOperativo Or EtapaProcesalActual <> pEtapaProcesal Or RevisorActual <> pRevisor Then
                Respuesta = True
            End If
        End If
        ' 
        'Close the Data Reader we are done with it.
        Reader.Close()

        'Close the Connection Object 
        Connection.Close()

        'Devolver resultado de la funcion
        Return Respuesta
    End Function

    Private Sub ActualizarTotalDeuda(ByVal pExpediente As String, ByVal pTotalDeuda As Int64)
        'Ojo este procedimiento actualiza la DEUDA (, mas no el SALDO) en la tabla EJEFISGLOBAL
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        ' 
        Dim Command As SqlCommand
        Dim UpdateSQL As String = "UPDATE EJEFISGLOBAL SET EFIVALDEU = @EFIVALDEU WHERE EFINROEXP = @EFINROEXP"
        ' update
        Command = New SqlCommand(UpdateSQL, Connection)
        Command.Parameters.AddWithValue("@EFIVALDEU", pTotalDeuda)
        Command.Parameters.AddWithValue("@EFINROEXP", pExpediente)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Actualizar deuda de expedientes ", "Expediente " & pExpediente, Command)

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
            Return
        End Try

        Connection.Close()
    End Sub

    Private Function ConsultarTotalDeuda(ByVal pExpediente As String) As Int64
        'Ojo este procedimiento consulta la DEUDA, mas no el SALDO
        Dim TotalSaldo As Int64 = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        'Consultar el total de la deuda

        '---------------------------------------------------------------------------------------------------------------
        ' Empresa  : UT - Technology  
        ' Autor    : Jeisson Gómez 
        ' Fecha    : 18/12/2017 
        ' Objetivo : Que el sistema muestre adecuadamente el saldo en la grilla inicial de ingreso al sistema. 
        '----------------------------------------------------------------------------------------------------------------
        Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " &
                                      "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pExpediente.Trim & "' GROUP BY MT_expediente"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            TotalSaldo = Convert.ToInt64(Reader("totaldeuda").ToString())
        End If
        Reader.Close()
        Connection.Close()

        Return TotalSaldo
    End Function

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
        'Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx?ID=" & Request("pExpediente"))

        ' Get a ClientScriptManager reference from the Page class. 
        'Dim cs As ClientScriptManager = Page.ClientScript
        'cs.RegisterClientScriptBlock(Me.[GetType](), "RedirectScript", "window.parent.location = 'EditEJEFISGLOBALREPARTIDOR.aspx?ID='" & Request("pExpediente"), True)
    End Sub

    Private Function Informacion_deuda(ByVal expediente As String) As DataTable

        Dim connection As New SqlConnection(Funciones.CadenaConexion)
        connection.Open()
        'Consultar el total de la deuda
        Const sql As String = "SELECT A.ID_GRUPO,A.SUBSISTEMA,SUM(A.AJUSTE) AS DEUDA FROM SQL_PLANILLA A, GRUPOS B WHERE A.ID_GRUPO = B.ID_GRUPO AND  EXPEDIENTE = @EXPEDIENTE GROUP BY A.SUBSISTEMA,A.ID_GRUPO"
        Dim command As New SqlCommand(sql, connection)
        command.Parameters.AddWithValue("@EXPEDIENTE", expediente)

        Dim dts As New SqlDataAdapter(command)
        Dim table As New DataTable
        dts.Fill((table))

        Return table

    End Function

    Private Sub Inicializardeuda()

        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        txtValorObligacion.Text = 0
        txtPartidaGlobal.Text = 0
        txtSancionOmision.Text = 0
        txtSancionInexactitud.Text = 0
        txtSancionMora.Text = 0

        txtTotalObligacion.Text = 0
        txtTotalObligacion.Attributes.Add("readonly", "readonly")
        txtTotalPartidaGlobal.Text = 0
        txtTotalPartidaGlobal.Attributes.Add("readonly", "readonly")
        txtTotalSancion.Text = 0
        txtTotalSancion.Attributes.Add("readonly", "readonly")
        txtTotalDeuda.Text = 0
        txtTotalDeuda.Attributes.Add("readonly", "readonly")

        'txtcapitalmulta.Text = 0
        'txtomisossalud.Text = 0
        'txtmorasalud.Text = 0
        'txtinexactossalud.Text = 0
        'txtomisospensiones.Text = 0
        'txtmorapensiones.Text = 0
        'txtinexactospensiones.Text = 0
        'txtomisosfondosolpen.Text = 0
        'txtmorafondosolpen.Text = 0
        'txtinexactosfondosolpen.Text = 0
        'txtomisosarl.Text = 0
        'txtmoraarl.Text = 0
        'txtinexactosarl.Text = 0
        'txtomisosicbf.Text = 0
        'txtmoraicbf.Text = 0
        'txtinexactosicbf.Text = 0
        'txtomisossena.Text = 0
        'txtmorasena.Text = 0
        'txtinexactossena.Text = 0
        'txtomisossubfamiliar.Text = 0
        'txtmorasubfamiliar.Text = 0
        'txtinexactossubfamiliar.Text = 0
        'txtsentenciasjudiciales.Text = 0
        'txtcuotaspartesacum.Text = 0
        'txttotalmultas.Text = 0
        'txttotalomisos.Text = 0
        'txttotalmora.Text = 0
        'txttotalinexactos.Text = 0
        'txttotalsentencias.Text = 0
        'txttotaldeuda.Text = 0
        'txtTotalRepartidor.Text = 0
        txtTotalSancion.Text = 0
    End Sub

    Protected Sub cmdCancel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel2.Click
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdCancel3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel3.Click
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub
    'Se cambia para que ActualizarDatosFinales utilice su propia Conexión Jeisson Gómez 24/03/2017
    Private Sub ActualizarDatosFinales()

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Dim TotalExp As Int64 = ConsultarTotalDeuda(Request("pExpediente"))
        ActualizarTotalDeuda(Request("pExpediente"), TotalExp)

        'Obtener los datos del expediente actual
        Dim repartidor, revisor, abogado, estadopago, estadooperativo, etapaprocesal As String
        repartidor = ""
        revisor = ""
        abogado = ""
        estadopago = ""
        estadooperativo = ""
        etapaprocesal = ""
        Dim sql As String = "SELECT repartidor, revisor, abogado, estadopago, estadooperativo, etapaprocesal FROM CAMBIOS_ESTADO WHERE idunico = (SELECT MAX(idunico) FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp)"
        Connection.Open()
        Dim Command2 As New SqlCommand(sql, Connection)
        Command2.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Dim Reader2 As SqlDataReader = Command2.ExecuteReader
        If Reader2.Read Then
            repartidor = Reader2("repartidor").ToString().Trim
            revisor = Reader2("revisor").ToString().Trim
            abogado = Reader2("abogado").ToString().Trim
            estadopago = Reader2("estadopago").ToString().Trim
            estadooperativo = Reader2("estadooperativo").ToString().Trim()
            etapaprocesal = Reader2("etapaprocesal").ToString().Trim()
        End If
        Reader2.Close()
        Connection.Close()


        'Validar si se ingresan datos de devolución del expediente 
        If txtNumMemoDev.Text.Trim <> "" Then

            '07/abril/2014. HACER TODO ESTO SI NumTitulos ACTIVOS = 1
            Dim DevolverExpediente As Boolean = False
            DevolverExpediente = AplicaDevolucionExpediente(Request("pExpediente").Trim, ID)

            If DevolverExpediente Then
                'Cambiar el estado del expediente a devuelto en la tabla CAMBIOS_ESTADO y en la tabla EJEFISGLOBAL
                'Cambio de estado en la tabla CAMBIOS_ESTADO
                If HayCambioEstado(Request("pExpediente"), repartidor, revisor, abogado, "04", estadopago, estadooperativo, etapaprocesal) Then
                    ' Colocar los datos ACTUALES del proceso, pero cambiar el estado a "04"=DEVUELTO en la fecha de hoy 
                    RegistrarCambioEstado(Request("pExpediente").Trim, repartidor, revisor, abogado, DateTime.Now(), "04", estadopago, estadooperativo, etapaprocesal)
                End If
                'Forzar cambio de estado en la tabla de expedientes (ejefisglobal)
                CambiarSoloEstado(Request("pExpediente").Trim, "04")
            End If


        Else
            'Si llega aqui es porque no hay datos de devolución (txtNumMemoDev.Text.Trim = "")
            'Pero es posible que el ultimo estado del expediente sea devuelto. Si esta asi, 
            'entonces el titulo que "revive" cambia el estado del expediente porque ya no puede quedar en estado devuelto
            If GetEstadoActualExpediente(Request("pExpediente").Trim) = "04" Then
                'Cambio de estado en la tabla de expedientes (ejefisglobal)
                CambiarSoloEstado(Request("pExpediente").Trim, "09")

                If HayCambioEstado(Request("pExpediente"), repartidor, revisor, abogado, "09", estadopago, estadooperativo, etapaprocesal) Then
                    ' Colocar los datos ACTUALES del proceso, pero cambiar el estado a "09"=REPARTO en la fecha de hoy 
                    RegistrarCambioEstado(Request("pExpediente").Trim, repartidor, revisor, abogado, DateTime.Now(), "09", estadopago, estadooperativo, etapaprocesal)
                End If
            End If
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        ' Duplicidad de informacion Titulos 
        ' Se agrega para habilitar el botón duplicidad información de titulos Jeisson Gómez 24/03/2017 
        cmdSave.Enabled = False

        'Este boton solo va a a guardar los campos de la pestaña general

        'Validar datos
        If txtMT_nro_titulo.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el número del título por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        If txtMT_fec_expedicion_titulo.Text.Trim = "" Then
            CustomValidator1.Text = "Digite la fecha de expedición del título por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        If cboMT_tipo_titulo.SelectedValue.Length <= 0 Then
            'If Session("mnivelacces") = 5 Then
            CustomValidator1.Text = "Seleccione el tipo de título por favor"
            CustomValidator1.IsValid = False
            Return
            'End If
        End If

        If cboPROCEDENCIA.SelectedValue = 0 Then
            CustomValidator1.Text = "Seleccione la procedencia del título por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        '/-----------------------------------------------------------------  
        'ID _HU:  002 0
        'Nombre HU   : Obligatoriedad de Campos fecha de ejecutoria y fecha de exigibilidad oficial
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 06-01-2017 
        'Objetivo : Hacer obligatorios los campos de fecha ejecutoria y fecha de exigibilidad oficial,
        '           para el perfil Gestor - Abogado
        '------------------------------------------------------------------/

        If (Session("mnivelacces") = 4) Then
            If String.IsNullOrWhiteSpace(txtMT_fecha_ejecutoriaObli.Text.Trim) Then
                CustomValidator1.Text = "Digite la fecha de Ejecutoria del título por favor"
                CustomValidator1.IsValid = False
                Return
            End If

            If String.IsNullOrWhiteSpace(txtMT_fec_exi_liqObli.Text.Trim) Then
                CustomValidator1.Text = "Digite la fecha de exigibilidad liquidación oficial del título por favor"
                CustomValidator1.IsValid = False
                Return
            End If
        End If

        '/-----------------------------------------------------------------  
        'ID _HU:  002 0
        'Nombre HU   : Obligatoriedad de Campos fecha de ejecutoria y fecha de exigibilidad oficial
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 06-01-2017  
        'Objetivo : Hacer obligatorios los campos de fecha ejecutoria y fecha de exigibilidad oficial,
        '           para el perfil Gestor - Abogado
        '------------------------------------------------------------------/

        Dim ID As String = Request("ID")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        ' Se comenta el Open de la conexión duplicidad información de titulos Jeisson Gómez 24/03/2017
        'Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into MAESTRO_TITULOS ([MT_nro_titulo], [MT_expediente], [MT_tipo_titulo], [MT_titulo_acumulado], [MT_fec_expedicion_titulo], [MT_fec_notificacion_titulo], [MT_for_notificacion_titulo], [MT_res_resuelve_reposicion], [MT_fec_expe_resolucion_reposicion], [MT_fec_not_reso_resu_reposicion], [MT_for_not_reso_resu_reposicion], [MT_reso_resu_apela_recon], [MT_fec_exp_reso_apela_recon], [MT_fec_not_reso_apela_recon], [MT_for_not_reso_apela_recon], [MT_fecha_ejecutoria], [MT_fec_exi_liq], [MT_fec_cad_presc], [MT_procedencia]) " &
                            "VALUES ( @MT_nro_titulo, @MT_expediente, @MT_tipo_titulo, @MT_titulo_acumulado, @MT_fec_expedicion_titulo, @MT_fec_notificacion_titulo, @MT_for_notificacion_titulo, @MT_res_resuelve_reposicion, @MT_fec_expe_resolucion_reposicion, @MT_fec_not_reso_resu_reposicion, @MT_for_not_reso_resu_reposicion, @MT_reso_resu_apela_recon, @MT_fec_exp_reso_apela_recon, @MT_fec_not_reso_apela_recon, @MT_for_not_reso_apela_recon, @MT_fecha_ejecutoria, @MT_fec_exi_liq, @MT_fec_cad_presc, @MT_procedencia) "

        Dim UpdateSQL As String = "Update MAESTRO_TITULOS set [MT_nro_titulo] = @MT_nro_titulo, [MT_tipo_titulo] = @MT_tipo_titulo, [MT_titulo_acumulado] = @MT_titulo_acumulado, [MT_fec_expedicion_titulo] = @MT_fec_expedicion_titulo, [MT_fec_notificacion_titulo] = @MT_fec_notificacion_titulo, [MT_for_notificacion_titulo] = @MT_for_notificacion_titulo, [MT_res_resuelve_reposicion] = @MT_res_resuelve_reposicion, [MT_fec_expe_resolucion_reposicion] = @MT_fec_expe_resolucion_reposicion, [MT_fec_not_reso_resu_reposicion] = @MT_fec_not_reso_resu_reposicion, [MT_for_not_reso_resu_reposicion] = @MT_for_not_reso_resu_reposicion, [MT_reso_resu_apela_recon] = @MT_reso_resu_apela_recon, [MT_fec_exp_reso_apela_recon] = @MT_fec_exp_reso_apela_recon, [MT_fec_not_reso_apela_recon] = @MT_fec_not_reso_apela_recon, [MT_for_not_reso_apela_recon] = @MT_for_not_reso_apela_recon, [MT_fecha_ejecutoria] = @MT_fecha_ejecutoria, [MT_fec_exi_liq] = @MT_fec_exi_liq, [MT_fec_cad_presc] = @MT_fec_cad_presc, [MT_procedencia] = @MT_procedencia " &
                            "WHERE idunico = @idunico "
        ' 
        'if ID > 0 run the update..... ModoAddEditTitulo = "EDITAR" 
        'if ID = 0 run the Insert..... ModoAddEditTitulo = "ADICIONAR"
        '/-----------------------------------------------------------------  
        'ID _HU:  007
        'Nombre HU: Duplicidad de informacion Titulos
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 23-03-2017  
        'Objetivo : Se cambia la forma de validar, para evitar duplicidad, 
        ' no se ha podido replicar este comportamiento en el ambiente de desarrollo.
        '------------------------------------------------------------------/
        'If ModoAddEditTitulo = "ADICIONAR" Then 'String.IsNullOrEmpty(ID) Then
        If ValidarTitulo(txtMT_nro_titulo.Text.Trim, Request("pExpediente")) Then
            ' insert
            Command = New SqlCommand(InsertSQL, Connection)
            ID = txtMT_nro_titulo.Text.Trim
            Command.Parameters.AddWithValue("@MT_nro_titulo", txtMT_nro_titulo.Text.Trim)
            Command.Parameters.AddWithValue("@MT_expediente", Request("pExpediente"))
        Else
            ' update
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@MT_nro_titulo", txtMT_nro_titulo.Text.Trim)
            Command.Parameters.AddWithValue("@idunico", Request("pIdUnico"))
        End If

        If cboMT_tipo_titulo.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MT_tipo_titulo", cboMT_tipo_titulo.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MT_tipo_titulo", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@MT_titulo_acumulado", txtMT_titulo_acumulado.Text)

        If IsDate(Left(txtMT_fec_expedicion_titulo.Text.Trim, 10)) Then
            'Command.Parameters.AddWithValue("@MT_fec_expedicion_titulo", Left(txtMT_fec_expedicion_titulo.Text.Trim, 10))
            '24/06/2014
            'Prueba para acabar con problemas de fecha 
            Command.Parameters.AddWithValue("@MT_fec_expedicion_titulo", Date.ParseExact(Left(txtMT_fec_expedicion_titulo.Text.Trim, 10), "dd/MM/yyyy", Nothing))

        Else
            Command.Parameters.AddWithValue("@MT_fec_expedicion_titulo", DBNull.Value)
        End If

        If IsDate(Left(txtMT_fec_notificacion_titulo.Text.Trim, 10)) Then
            'Command.Parameters.AddWithValue("@MT_fec_notificacion_titulo", Left(txtMT_fec_notificacion_titulo.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fec_notificacion_titulo", Date.ParseExact(Left(txtMT_fec_notificacion_titulo.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_notificacion_titulo", DBNull.Value)
        End If

        If cboMT_for_notificacion_titulo.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MT_for_notificacion_titulo", cboMT_for_notificacion_titulo.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MT_for_notificacion_titulo", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@MT_res_resuelve_reposicion", txtMT_res_resuelve_reposicion.Text)

        If IsDate(txtMT_fec_expe_resolucion_reposicion.Text.Trim) Then
            'Command.Parameters.AddWithValue("@MT_fec_expe_resolucion_reposicion", Left(txtMT_fec_expe_resolucion_reposicion.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fec_expe_resolucion_reposicion", Date.ParseExact(Left(txtMT_fec_expe_resolucion_reposicion.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_expe_resolucion_reposicion", DBNull.Value)

        End If

        If IsDate(txtMT_fec_not_reso_resu_reposicion.Text) Then
            'Command.Parameters.AddWithValue("@MT_fec_not_reso_resu_reposicion", Left(txtMT_fec_not_reso_resu_reposicion.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fec_not_reso_resu_reposicion", Date.ParseExact(Left(txtMT_fec_not_reso_resu_reposicion.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_not_reso_resu_reposicion", DBNull.Value)

        End If

        If cboMT_for_not_reso_resu_reposicion.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MT_for_not_reso_resu_reposicion", cboMT_for_not_reso_resu_reposicion.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MT_for_not_reso_resu_reposicion", DBNull.Value)

        End If

        Command.Parameters.AddWithValue("@MT_reso_resu_apela_recon", txtMT_reso_resu_apela_recon.Text)

        If IsDate(txtMT_fec_exp_reso_apela_recon.Text.Trim) Then
            'Command.Parameters.AddWithValue("@MT_fec_exp_reso_apela_recon", Left(txtMT_fec_exp_reso_apela_recon.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fec_exp_reso_apela_recon", Date.ParseExact(Left(txtMT_fec_exp_reso_apela_recon.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_exp_reso_apela_recon", DBNull.Value)

        End If

        If IsDate(txtMT_fec_not_reso_apela_recon.Text.Trim) Then
            'Command.Parameters.AddWithValue("@MT_fec_not_reso_apela_recon", Left(txtMT_fec_not_reso_apela_recon.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fec_not_reso_apela_recon", Date.ParseExact(Left(txtMT_fec_not_reso_apela_recon.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_not_reso_apela_recon", DBNull.Value)
        End If

        If cboMT_for_not_reso_apela_recon.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MT_for_not_reso_apela_recon", cboMT_for_not_reso_apela_recon.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MT_for_not_reso_apela_recon", DBNull.Value)
        End If


        If IsDate(txtMT_fecha_ejecutoriaObli.Text.Trim) Then
            'Command.Parameters.AddWithValue("@MT_fecha_ejecutoria", Left(txtMT_fecha_ejecutoria.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fecha_ejecutoria", Date.ParseExact(Left(txtMT_fecha_ejecutoriaObli.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fecha_ejecutoria", DBNull.Value)
        End If

        If IsDate(Left(txtMT_fec_exi_liqObli.Text.Trim, 10)) Then
            'Command.Parameters.AddWithValue("@MT_fec_exi_liq", Left(txtMT_fec_exi_liq.Text.Trim, 10))
            Command.Parameters.AddWithValue("@txtMT_fec_exi_liqObli", Date.ParseExact(Left(txtMT_fec_exi_liqObli.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@txtMT_fec_exi_liqObli", DBNull.Value)
        End If

        If IsDate(Left(txtMT_fec_cad_presc.Text.Trim, 10)) Then
            'Command.Parameters.AddWithValue("@MT_fec_cad_presc", Left(txtMT_fec_cad_presc.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fec_cad_presc", Date.ParseExact(Left(txtMT_fec_cad_presc.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_cad_presc", DBNull.Value)
        End If

        If cboPROCEDENCIA.SelectedValue.Length > 0 And cboPROCEDENCIA.SelectedValue.Trim <> 0 Then
            Command.Parameters.AddWithValue("@MT_procedencia", cboPROCEDENCIA.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MT_procedencia", DBNull.Value)
        End If

        Dim SwOK As Boolean = False
        Try
            ' Se deja el Open de la conexión aquí en el try duplicidad información titulos Jeisson Gómez 24/03/2017  
            Connection.Open()
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de títulos ejecutivos", "No. título " & txtMT_nro_titulo.Text.Trim, Command)

            SwOK = True
        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            'Connection.Close()
            Return
        Finally
            ' Se agrega el Finally para cerrar la conexión y limpiar las variables InsertSQL y UpdateSQL Jeisson Gómez 24/03/2017
            InsertSQL = String.Empty
            UpdateSQL = String.Empty
            Connection.Close()
        End Try
        'Close the Connection Object 
        ' Se comenta debido que el Finally cierra la conexión Jeisson Gómez 24/03/2017
        ' Connection.Close()

        If SwOK Then
            'Se cambia para que ActualizarDatosFinales utilice su propia Conexión Jeisson Gómez 24/03/2017
            'Actualizar datos finales
            ActualizarDatosFinales()
        End If

        ' Se agrega para habilitar el botón duplicidad información de titulos Jeisson Gómez 24/03/2017 
        cmdSave.Enabled = True
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdSave2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave2.Click

        cmdSave2.Enabled = False
        'Este boton solo va a a guardar los campos de la deuda

        ' HU_012 - Desagregación de Obligaciones
        ' Jeisson Gómez 
        ' Para los componentes totales de los títulos, si la sumatoria de cada uno de los componente no concuerda, 
        ' la aplicación no debe permitir grabar la información en la base de datos. 

        If String.IsNullOrEmpty(txtMT_nro_titulo.Text.Trim) Then
            CustomValidator2.Text = "El número del título no puede estar vacío o en blanco. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If String.IsNullOrEmpty(cboMT_tipo_titulo.SelectedValue) Then
            CustomValidator2.Text = "El tipo del título no puede estar vacío o en blanco. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If String.IsNullOrEmpty(txtMT_fec_expedicion_titulo.Text.Trim()) Then
            CustomValidator2.Text = "La fecha de expedición del título no puede estar vacía o en blanco. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If cboPROCEDENCIA.SelectedValue.Length > 0 And cboPROCEDENCIA.SelectedValue.Trim = "0" Then
            CustomValidator2.Text = "La procedencia del título no puede estar vacía o en blanco. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If Decimal.Parse(txtValorObligacion.Text) <> Decimal.Parse(txtValorObligacion.Text) Then
            CustomValidator2.Text = "El valor digitado de la obligación y el valor total de la oblibación no coinciden. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If Decimal.Parse(txtPartidaGlobal.Text) <> Decimal.Parse(txtTotalPartidaGlobal.Text) Then
            CustomValidator2.Text = "El valor digitado de la partida global y el valor total de la partida global no coinciden. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If Decimal.Parse(txtSancionOmision.Text) + Decimal.Parse(txtSancionInexactitud.Text) + Decimal.Parse(txtSancionMora.Text) <> Decimal.Parse(txtTotalSancion.Text) Then
            CustomValidator2.Text = "El valor digitado de las sanciones omisión, inexactitud y mora  contra el valor total de las sanciones no coinciden. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If Decimal.Parse(txtTotalObligacion.Text) + Decimal.Parse(txtTotalPartidaGlobal.Text) + Decimal.Parse(txtTotalSancion.Text) <> Decimal.Parse(txtTotalDeuda.Text) Then
            CustomValidator2.Text = "El valor total de la obligación, total de la partida global, total de la sanción contra el valor total de la deuda no coinciden. Favor verificar."
            CustomValidator2.IsValid = False
            Return
        End If

        If Session("mnivelacces") <> 5 Then
            ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
            ' Jeisson Gómez 17/02/2016 Aquí validar contra el valor txtTotalRepartidor.Text
            'If txtTotalDeuda.Text <> txtTotalRepartidor.Text Then
            '    CustomValidator2.Text = "El valor digitado por el repartidor y el gestor no coinciden. Favor verificar."
            '    CustomValidator2.IsValid = False
            '    Return
            'End If
        End If

        If Session("mnivelacces") <> 5 Then
            If IsNumeric(txtTotalDeuda.Text) Then
                If Convert.ToInt32(txtTotalDeuda.Text) = 0 Then
                    CustomValidator2.Text = "El valor total del título ejecutivo debe ser mayor a cero"
                    CustomValidator2.IsValid = False
                    Return
                End If
            End If
        End If


        Dim ID As String = Request("ID")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        ' Se comenta el Open de la conexión duplicidad información de titulos Jeisson Gómez 24/03/2017
        'Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        'Declare string InsertSQL 
        Dim InsertSQL As String = "INSERT INTO maestro_titulos (MT_nro_titulo, MT_expediente, MT_tipo_titulo, MT_fec_expedicion_titulo, MT_procedencia,  " &
                                        " capitalmulta, omisossalud, morasalud, inexactossalud, omisospensiones, " &
                                        "morapensiones, inexactospensiones, omisosfondosolpen, morafondosolpen, inexactosfondosolpen, " &
                                        "omisosarl, moraarl, inexactosarl, omisosicbf, moraicbf, inexactosicbf, omisossena, morasena, " &
                                        "inexactossena, omisossubfamiliar, morasubfamiliar, inexactossubfamiliar, sentenciasjudiciales, " &
                                        "cuotaspartesacum, totalmultas, totalomisos, totalmora, totalinexactos, totalsentencias, " &
                                        "totaldeuda, TotalRepartidor, MT_totalSancion, " &
                                        " MT_valor_obligacion, MT_partida_global, MT_sancion_omision, MT_sancion_mora, MT_sancion_inexactitud, " &
                                        " MT_total_obligacion, MT_total_partida_global) " &
                                   "VALUES (@MT_nro_titulo, @MT_expediente, @MT_tipo_titulo, @MT_fec_expedicion_titulo,  @MT_procedencia, " &
                                            " @capitalmulta, @omisossalud, @morasalud, @inexactossalud, @omisospensiones, " &
                                            " @morapensiones, @inexactospensiones, @omisosfondosolpen, @morafondosolpen, @inexactosfondosolpen, " &
                                            " @omisosarl, @moraarl, @inexactosarl, @omisosicbf, @moraicbf, @inexactosicbf, @omisossena, @morasena, " &
                                            " @inexactossena, @omisossubfamiliar, @morasubfamiliar, @inexactossubfamiliar, @sentenciasjudiciales, " &
                                            " @cuotaspartesacum, @totalmultas, @totalomisos, @totalmora, @totalinexactos, @totalsentencias, " &
                                            " @totaldeuda, @TotalRepartidor,@TotalSancion, " &
                                            " @MT_valor_obligacion, @MT_partida_global, @MT_sancion_omision, @MT_sancion_mora, @MT_sancion_inexactitud, " &
                                            " @MT_total_obligacion, @MT_total_partida_global)"

        Dim UpdateSQL As String = "UPDATE maestro_titulos SET capitalmulta = @capitalmulta, omisossalud = @omisossalud, morasalud = @morasalud, " &
                                        "inexactossalud = @inexactossalud, omisospensiones = @omisospensiones, morapensiones = @morapensiones, " &
                                        "inexactospensiones = @inexactospensiones, omisosfondosolpen = @omisosfondosolpen, " &
                                        "morafondosolpen = @morafondosolpen, inexactosfondosolpen = @inexactosfondosolpen, " &
                                        "omisosarl = @omisosarl, moraarl = @moraarl, inexactosarl = @inexactosarl, omisosicbf = @omisosicbf, " &
                                        "moraicbf = @moraicbf, inexactosicbf = @inexactosicbf, omisossena = @omisossena, morasena = @morasena," &
                                        "inexactossena = @inexactossena, omisossubfamiliar = @omisossubfamiliar, " &
                                        "morasubfamiliar = @morasubfamiliar, inexactossubfamiliar = @inexactossubfamiliar, " &
                                        "sentenciasjudiciales = @sentenciasjudiciales, cuotaspartesacum = @cuotaspartesacum, " &
                                        "totalmultas = @totalmultas, totalomisos = @totalomisos, totalmora = @totalmora, " &
                                        "totalinexactos = @totalinexactos, totalsentencias = @totalsentencias, totaldeuda = @totaldeuda, " &
                                        "TotalRepartidor = @TotalRepartidor,MT_totalSancion = @TotalSancion, " &
                                        "MT_valor_obligacion = @MT_valor_obligacion, MT_partida_global = @MT_partida_global, MT_sancion_omision = @MT_sancion_omision, " &
                                        "MT_sancion_mora = @MT_sancion_mora, MT_sancion_inexactitud = @MT_sancion_inexactitud, " &
                                        "MT_total_obligacion = @MT_total_obligacion, MT_total_partida_global = @MT_total_partida_global " &
                                        "WHERE idunico = @idunico "
        ' 
        'if ID > 0 run the update..... ModoAddEditTitulo = "EDITAR" 
        'if ID = 0 run the Insert..... ModoAddEditTitulo = "ADICIONAR"'/-----------------------------------------------------------------  
        'ID _HU:  007
        'Nombre HU: Duplicidad de informacion Titulos
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 23-03-2017  
        'Objetivo : Se cambia la forma de validar, para evitar duplicidad, 
        ' no se ha podido replicar este comportamiento en el ambiente de desarrollo.
        '------------------------------------------------------------------/
        'If ModoAddEditTitulo = "ADICIONAR" Then 'String.IsNullOrEmpty(ID) Then
        If ValidarTitulo(txtMT_nro_titulo.Text.Trim, Request("pExpediente")) Then
            ' insert
            Command = New SqlCommand(InsertSQL, Connection)
            ID = txtMT_nro_titulo.Text.Trim
            Command.Parameters.AddWithValue("@MT_nro_titulo", txtMT_nro_titulo.Text.Trim)
            Command.Parameters.AddWithValue("@MT_expediente", Request("pExpediente"))
        Else
            ' update
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@MT_nro_titulo", txtMT_nro_titulo.Text.Trim)
            Command.Parameters.AddWithValue("@idunico", Request("pIdUnico"))
        End If

        If cboMT_tipo_titulo.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MT_tipo_titulo", cboMT_tipo_titulo.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MT_tipo_titulo", DBNull.Value)
        End If

        If IsDate(Left(txtMT_fec_expedicion_titulo.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@MT_fec_expedicion_titulo", Date.ParseExact(Left(txtMT_fec_expedicion_titulo.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_expedicion_titulo", DBNull.Value)
        End If

        If cboPROCEDENCIA.SelectedValue.Length > 0 And cboPROCEDENCIA.SelectedValue.Trim <> 0 Then
            Command.Parameters.AddWithValue("@MT_procedencia", cboPROCEDENCIA.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MT_procedencia", DBNull.Value)
        End If

        '/-----------------------------------------------------------------  
        'ID _HU:  012
        'Nombre HU: Desagregación de Obligaciones 
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 17-02-2017  
        'Objetivo : Realizar la desagregación de las obligaciones 
        ' Se mapean los campos así 
        '  Campo definido PL   -    Campo Base de datos         -                   Tabla 
        '   Valor Obligacion        MT_valor_obligacion                         maestro_titulos
        '   Partida Global          MT_partida_global                           maestro_titulos
        '   Sanción Omisión         MT_sancion_omision                          maestro_titulos 
        '   Sanción Mora            MT_sancion_mora                             maestro_titulos 
        '   Sanción Inexactitud     MT_sancion_inexactitud                      maestro_titulos 
        '   Total de la Obligacion  totaldeuda - MT_total_obligacion            maestro_titulos 
        '   Total Partida Global    MT_total_partida_global                     maestro_titulos              
        '   Total Sanción           MT_totalSancion                             maestro_titulos 
        '   Total de la deuda       totaldeuda                                  maestro_titulos  
        '   Todos los campos se han creado excepto totaldeuda y MT_totalSancion que ya existía.    
        '----------------------------------------------------------------------------------------/


        IIf(IsNumeric(txtValorObligacion.Text), Command.Parameters.AddWithValue("@MT_valor_obligacion", txtValorObligacion.Text), 0)
        IIf(IsNumeric(txtPartidaGlobal.Text), Command.Parameters.AddWithValue("@MT_partida_global", txtPartidaGlobal.Text), 0)
        IIf(IsNumeric(txtSancionOmision.Text), Command.Parameters.AddWithValue("@MT_sancion_omision", txtSancionOmision.Text), 0)
        IIf(IsNumeric(txtSancionMora.Text), Command.Parameters.AddWithValue("@MT_sancion_mora", txtSancionMora.Text), 0)
        IIf(IsNumeric(txtSancionInexactitud.Text), Command.Parameters.AddWithValue("@MT_sancion_inexactitud", txtSancionInexactitud.Text), 0)
        IIf(IsNumeric(txtTotalObligacion.Text), Command.Parameters.AddWithValue("@MT_total_obligacion", txtTotalObligacion.Text), 0)
        IIf(IsNumeric(txtTotalPartidaGlobal.Text), Command.Parameters.AddWithValue("@MT_total_partida_global", txtPartidaGlobal.Text), 0)
        IIf(IsNumeric(txtTotalSancion.Text), Command.Parameters.AddWithValue("@TotalSancion", txtTotalSancion.Text), 0)
        IIf(IsNumeric(txtTotalDeuda.Text), Command.Parameters.AddWithValue("@totaldeuda", txtTotalDeuda.Text), 0)

        ' Se adiciona la información TotalDeuda al campo TotalRepartidor
        IIf(IsNumeric(txtTotalDeuda.Text), Command.Parameters.AddWithValue("@TotalRepartidor", txtTotalDeuda.Text), 0)

        Command.Parameters.AddWithValue("@capitalmulta", 0)
        Command.Parameters.AddWithValue("@omisossalud", 0)
        Command.Parameters.AddWithValue("@morasalud", 0)
        Command.Parameters.AddWithValue("@inexactossalud", 0)
        Command.Parameters.AddWithValue("@omisospensiones", 0)
        Command.Parameters.AddWithValue("@morapensiones", 0)
        Command.Parameters.AddWithValue("@inexactospensiones", 0)
        Command.Parameters.AddWithValue("@omisosfondosolpen", 0)
        Command.Parameters.AddWithValue("@morafondosolpen", 0)
        Command.Parameters.AddWithValue("@inexactosfondosolpen", 0)
        Command.Parameters.AddWithValue("@omisosarl", 0)
        Command.Parameters.AddWithValue("@moraarl", 0)
        Command.Parameters.AddWithValue("@inexactosarl", 0)
        Command.Parameters.AddWithValue("@omisosicbf", 0)
        Command.Parameters.AddWithValue("@moraicbf", 0)
        Command.Parameters.AddWithValue("@inexactosicbf", 0)
        Command.Parameters.AddWithValue("@omisossena", 0)
        Command.Parameters.AddWithValue("@morasena", 0)
        Command.Parameters.AddWithValue("@inexactossena", 0)
        Command.Parameters.AddWithValue("@omisossubfamiliar", 0)
        Command.Parameters.AddWithValue("@morasubfamiliar", 0)
        Command.Parameters.AddWithValue("@inexactossubfamiliar", 0)
        Command.Parameters.AddWithValue("@sentenciasjudiciales", 0)
        Command.Parameters.AddWithValue("@cuotaspartesacum", 0)
        Command.Parameters.AddWithValue("@totalmultas", 0)
        Command.Parameters.AddWithValue("@totalomisos", 0)
        Command.Parameters.AddWithValue("@totalmora", 0)
        Command.Parameters.AddWithValue("@totalinexactos", 0)
        Command.Parameters.AddWithValue("@totalsentencias", 0)

        '04/feb/2014
        'Parametros de la deuda

        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016

        'If IsNumeric(txtcapitalmulta.Text) Then
        '    Command.Parameters.AddWithValue("@capitalmulta", txtcapitalmulta.Text)
        'Else
        '    Command.Parameters.AddWithValue("@capitalmulta", DBNull.Value)
        'End If

        'If IsNumeric(txtomisossalud.Text) Then
        '    Command.Parameters.AddWithValue("@omisossalud", txtomisossalud.Text)
        'Else
        '    Command.Parameters.AddWithValue("@omisossalud", DBNull.Value)
        'End If

        'If IsNumeric(txtmorasalud.Text) Then
        '    Command.Parameters.AddWithValue("@morasalud", txtmorasalud.Text)
        'Else
        '    Command.Parameters.AddWithValue("@morasalud", DBNull.Value)
        'End If

        'If IsNumeric(txtinexactossalud.Text) Then
        '    Command.Parameters.AddWithValue("@inexactossalud", txtinexactossalud.Text)
        'Else
        '    Command.Parameters.AddWithValue("@inexactossalud", DBNull.Value)
        'End If

        'If IsNumeric(txtomisospensiones.Text) Then
        '    Command.Parameters.AddWithValue("@omisospensiones", txtomisospensiones.Text)
        'Else
        '    Command.Parameters.AddWithValue("@omisospensiones", DBNull.Value)
        'End If

        'If IsNumeric(txtmorapensiones.Text) Then
        '    Command.Parameters.AddWithValue("@morapensiones", txtmorapensiones.Text)
        'Else
        '    Command.Parameters.AddWithValue("@morapensiones", DBNull.Value)
        'End If

        'If IsNumeric(txtinexactospensiones.Text) Then
        '    Command.Parameters.AddWithValue("@inexactospensiones", txtinexactospensiones.Text)
        'Else
        '    Command.Parameters.AddWithValue("@inexactospensiones", DBNull.Value)
        'End If

        'If IsNumeric(txtomisosfondosolpen.Text) Then
        '    Command.Parameters.AddWithValue("@omisosfondosolpen", txtomisosfondosolpen.Text)
        'Else
        '    Command.Parameters.AddWithValue("@omisosfondosolpen", DBNull.Value)
        'End If

        'If IsNumeric(txtmorafondosolpen.Text) Then
        '    Command.Parameters.AddWithValue("@morafondosolpen", txtmorafondosolpen.Text)
        'Else
        '    Command.Parameters.AddWithValue("@morafondosolpen", DBNull.Value)
        'End If

        'If IsNumeric(txtinexactosfondosolpen.Text) Then
        '    Command.Parameters.AddWithValue("@inexactosfondosolpen", txtinexactosfondosolpen.Text)
        'Else
        '    Command.Parameters.AddWithValue("@inexactosfondosolpen", DBNull.Value)
        'End If

        'If IsNumeric(txtomisosarl.Text) Then
        '    Command.Parameters.AddWithValue("@omisosarl", txtomisosarl.Text)
        'Else
        '    Command.Parameters.AddWithValue("@omisosarl", DBNull.Value)
        'End If

        'If IsNumeric(txtmoraarl.Text) Then
        '    Command.Parameters.AddWithValue("@moraarl", txtmoraarl.Text)
        'Else
        '    Command.Parameters.AddWithValue("@moraarl", DBNull.Value)
        'End If

        'If IsNumeric(txtinexactosarl.Text) Then
        '    Command.Parameters.AddWithValue("@inexactosarl", txtinexactosarl.Text)
        'Else
        '    Command.Parameters.AddWithValue("@inexactosarl", DBNull.Value)
        'End If

        'If IsNumeric(txtomisosicbf.Text) Then
        '    Command.Parameters.AddWithValue("@omisosicbf", txtomisosicbf.Text)
        'Else
        '    Command.Parameters.AddWithValue("@omisosicbf", DBNull.Value)
        'End If

        'If IsNumeric(txtmoraicbf.Text) Then
        '    Command.Parameters.AddWithValue("@moraicbf", txtmoraicbf.Text)
        'Else
        '    Command.Parameters.AddWithValue("@moraicbf", DBNull.Value)

        'End If

        'If IsNumeric(txtinexactosicbf.Text) Then
        '    Command.Parameters.AddWithValue("@inexactosicbf", txtinexactosicbf.Text)
        'Else
        '    Command.Parameters.AddWithValue("@inexactosicbf", DBNull.Value)

        'End If

        'If IsNumeric(txtomisossena.Text) Then
        '    Command.Parameters.AddWithValue("@omisossena", txtomisossena.Text)
        'Else
        '    Command.Parameters.AddWithValue("@omisossena", DBNull.Value)

        'End If

        'If IsNumeric(txtmorasena.Text) Then
        '    Command.Parameters.AddWithValue("@morasena", txtmorasena.Text)
        'Else
        '    Command.Parameters.AddWithValue("@morasena", DBNull.Value)
        'End If

        'If IsNumeric(txtinexactossena.Text) Then
        '    Command.Parameters.AddWithValue("@inexactossena", txtinexactossena.Text)
        'Else
        '    Command.Parameters.AddWithValue("@inexactossena", DBNull.Value)

        'End If

        'If IsNumeric(txtomisossubfamiliar.Text) Then
        '    Command.Parameters.AddWithValue("@omisossubfamiliar", txtomisossubfamiliar.Text)
        'Else
        '    Command.Parameters.AddWithValue("@omisossubfamiliar", DBNull.Value)

        'End If

        'If IsNumeric(txtmorasubfamiliar.Text) Then
        '    Command.Parameters.AddWithValue("@morasubfamiliar", txtmorasubfamiliar.Text)
        'Else
        '    Command.Parameters.AddWithValue("@morasubfamiliar", DBNull.Value)

        'End If

        'If IsNumeric(txtinexactossubfamiliar.Text) Then
        '    Command.Parameters.AddWithValue("@inexactossubfamiliar", txtinexactossubfamiliar.Text)
        'Else
        '    Command.Parameters.AddWithValue("@inexactossubfamiliar", DBNull.Value)
        'End If

        'If IsNumeric(txtsentenciasjudiciales.Text) Then
        '    Command.Parameters.AddWithValue("@sentenciasjudiciales", txtsentenciasjudiciales.Text)
        'Else
        '    Command.Parameters.AddWithValue("@sentenciasjudiciales", DBNull.Value)
        'End If

        'If IsNumeric(txtcuotaspartesacum.Text) Then
        '    Command.Parameters.AddWithValue("@cuotaspartesacum", txtcuotaspartesacum.Text)
        'Else
        '    Command.Parameters.AddWithValue("@cuotaspartesacum", DBNull.Value)

        'End If

        'If IsNumeric(txttotalmultas.Text) Then
        '    Command.Parameters.AddWithValue("@totalmultas", txttotalmultas.Text)
        'Else
        '    Command.Parameters.AddWithValue("@totalmultas", DBNull.Value)

        'End If

        'If IsNumeric(txttotalomisos.Text) Then
        '    Command.Parameters.AddWithValue("@totalomisos", txttotalomisos.Text)
        'Else
        '    Command.Parameters.AddWithValue("@totalomisos", DBNull.Value)

        'End If

        'If IsNumeric(txttotalmora.Text) Then
        '    Command.Parameters.AddWithValue("@totalmora", txttotalmora.Text)
        'Else
        '    Command.Parameters.AddWithValue("@totalmora", DBNull.Value)

        'End If

        'If IsNumeric(txttotalinexactos.Text) Then
        '    Command.Parameters.AddWithValue("@totalinexactos", txttotalinexactos.Text)
        'Else
        '    Command.Parameters.AddWithValue("@totalinexactos", DBNull.Value)
        'End If

        'If IsNumeric(txttotalsentencias.Text) Then
        '    Command.Parameters.AddWithValue("@totalsentencias", txttotalsentencias.Text)
        'Else
        '    Command.Parameters.AddWithValue("@totalsentencias", DBNull.Value)
        'End If

        'If IsNumeric(txttotalsancion.Text) Then
        '    Command.Parameters.AddWithValue("@TotalSancion", txttotalsancion.Text)
        'Else
        '    Command.Parameters.AddWithValue("@TotalSancion", DBNull.Value)
        'End If

        'Dim TotalRepartidor As Double = 0
        'If IsNumeric(txtTotalRepartidor.Text) Then
        '    TotalRepartidor = CDbl(txtTotalRepartidor.Text)
        'Else
        '    TotalRepartidor = 0
        'End If


        'If IsNumeric(txttotaldeuda.Text) Then
        '    '07/oct/2014. A la hora de adicionar un nuevo titulo, totaldeuda = TotalRepartidor
        '    If ModoAddEditTitulo = "ADICIONAR" Then
        '        Command.Parameters.AddWithValue("@totaldeuda", TotalRepartidor)
        '    Else
        '        ' Edicion
        '        If Session("mnivelacces") = 5 Or Session("mnivelacces") = 8 Then
        '            'Para perfil repartidor o gestor de información 
        '            Command.Parameters.AddWithValue("@totaldeuda", txtTotalRepartidor.Text)
        '        Else
        '            ' Para abogados y demas perfiles
        '            Command.Parameters.AddWithValue("@totaldeuda", txttotaldeuda.Text)
        '        End If

        '    End If
        'Else
        '    Command.Parameters.AddWithValue("@totaldeuda", 0)
        'End If

        ''txtTotalRepartidor
        'Command.Parameters.AddWithValue("@TotalRepartidor", TotalRepartidor)


        Dim SwOK As Boolean = False
        Try
            ' Se deja el Open de la conexión aquí en el try duplicidad información titulos Jeisson Gómez 24/03/2017 
            Connection.Open()
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de títulos ejecutivos", "No. título " & txtMT_nro_titulo.Text.Trim, Command)

            SwOK = True

            '28/abril/2015. Yesid solicito que si se edita el valor del titulo => hay que afectar ejefisglobal
            'xxxyyy
            ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
            ' Jeisson Gómez 17/02/2016 
            'ActualizarValoresEjefisglobal(Request("pExpediente"), TotalRepartidor)
            ActualizarValoresEjefisglobal(Request("pExpediente"), txtTotalDeuda.Text)

        Catch ex As Exception
            CustomValidator2.Text = ex.Message
            CustomValidator2.IsValid = False
            Return
        Finally
            ' Se agrega el Finally para cerrar la conexión y limpiar las variables InsertSQL y UpdateSQL duplicidad información titulos Jeisson Gómez 24/03/2017
            InsertSQL = String.Empty
            UpdateSQL = String.Empty
            Connection.Close()
        End Try
        'Close the Connection Object 
        'Connection.Close()

        If SwOK Then
            'Se cambia para que ActualizarDatosFinales utilice su propia Conexión duplicidad información titulos Jeisson Gómez 24/03/2017
            'Actualizar datos finales
            ActualizarDatosFinales()
        End If
        '
        ' Se agrega para habilitar el botón duplicidad información de titulos Jeisson Gómez 24/03/2017 
        cmdSave2.Enabled = True
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))

    End Sub

    Protected Sub ActualizarValoresEjefisglobal(ByVal pExpediente As String, ByVal pValorTituloEjecutivo As Double)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        '
        Dim Command As SqlCommand
        Dim UpdateSQL As String = "UPDATE EJEFISGLOBAL SET EFIVALDEU = @ValorTituloEjecutivo, EFISALDOCAP = @ValorTituloEjecutivo - EFIPAGOSCAP WHERE EFINROEXP = @expediente"

        Command = New SqlCommand(UpdateSQL, Connection)
        Command.Parameters.AddWithValue("@ValorTituloEjecutivo", pValorTituloEjecutivo)
        Command.Parameters.AddWithValue("@expediente", pExpediente)

        Try
            Command.ExecuteNonQuery()

        Catch ex As Exception
            CustomValidator2.Text = ex.Message
            CustomValidator2.IsValid = False
            Connection.Close()
            Return
        End Try

        Connection.Close()
    End Sub

    Protected Sub cmdSave3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave3.Click

        cmdSave3.Enabled = False

        'Guardar datos de devolucion del titulo

        'Validar datos de devolución del título ejecutivo
        Dim DatosDevol As Boolean = False
        DatosDevol = HayDatosDevolucionValidos()
        If Not DatosDevol Then
            CustomValidator1.Text = "Si va a devolver el título ejecutivo diligencie todos los campos de la sección de devolución, en caso contrario, bórrelos todos por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        Dim ID As String = Request("ID")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        'Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into MAESTRO_TITULOS (NumMemoDev, FecMemoDev, CausalDevol, ObsDevol ) " &
             "VALUES ( @NumMemoDev, @FecMemoDev, @CausalDevol, @ObsDevol ) "

        Dim UpdateSQL As String = "Update MAESTRO_TITULOS set NumMemoDev = @NumMemoDev, FecMemoDev = @FecMemoDev, CausalDevol = @CausalDevol, ObsDevol = @ObsDevol " &
             "WHERE idunico = @idunico "
        ' 
        'if ID > 0 run the update..... ModoAddEditTitulo = "EDITAR" 
        'if ID = 0 run the Insert..... ModoAddEditTitulo = "ADICIONAR"
        If ModoAddEditTitulo = "ADICIONAR" Then 'String.IsNullOrEmpty(ID) Then
            ' insert
            Command = New SqlCommand(InsertSQL, Connection)
            ID = txtMT_nro_titulo.Text.Trim
            Command.Parameters.AddWithValue("@MT_nro_titulo", txtMT_nro_titulo.Text.Trim)
            Command.Parameters.AddWithValue("@MT_expediente", Request("pExpediente"))
        Else
            ' update
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@MT_nro_titulo", txtMT_nro_titulo.Text.Trim)
            Command.Parameters.AddWithValue("@idunico", Request("pIdUnico"))
        End If

        'Parametros de la devolucion :: NumMemoDev, FecMemoDev, CausalDevol, ObsDevol
        Command.Parameters.AddWithValue("@NumMemoDev", txtNumMemoDev.Text.Trim)
        If IsDate(Left(txtFecMemoDev.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecMemoDev", Left(txtFecMemoDev.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecMemoDev", DBNull.Value)
        End If
        If cboCausalDevol.SelectedValue.Trim <> "0" Then
            Command.Parameters.AddWithValue("@CausalDevol", cboCausalDevol.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@CausalDevol", DBNull.Value)
        End If
        Command.Parameters.AddWithValue("@ObsDevol", taObsDevol.InnerHtml)

        Dim SwOK As Boolean = False
        Try
            ' Se deja el Open de la conexión aquí en el try duplicidad información titulos Jeisson Gómez 24/03/2017 
            Connection.Open()
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de títulos ejecutivos", "No. título " & txtMT_nro_titulo.Text.Trim, Command)

            SwOK = True
        Catch ex As Exception
            CustomValidator2.Text = ex.Message
            CustomValidator2.IsValid = False
            Return
        Finally
            ' Se agrega el Finally para cerrar la conexión y limpiar las variables InsertSQL y UpdateSQL duplicidad información titulos Jeisson Gómez 24/03/2017
            InsertSQL = String.Empty
            UpdateSQL = String.Empty
            Connection.Close()
        End Try
        'Close the Connection Object 
        'Connection.Close()

        If SwOK Then
            'Se cambia para que ActualizarDatosFinales utilice su propia Conexión Jeisson Gómez 24/03/2017
            'Actualizar datos finales
            ActualizarDatosFinales()
        End If
        '
        ' Se agrega para habilitar el botón duplicidad información de titulos Jeisson Gómez 24/03/2017 
        cmdSave3.Enabled = True
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))

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


    '/-----------------------------------------------------------------  
    'ID _HU:  002 0
    'Nombre HU   : Obligatoriedad de Campos fecha de ejecutoria y fecha de exigibilidad oficial
    'Empresa: UT TECHNOLOGY 
    'Autor: Jeisson Gómez 
    'Fecha: 06-01-2017 
    'Objetivo : Hacer obligatorios los campos de fecha ejecutoria y fecha de exigibilidad oficial,
    '           para el perfil Gestor - Abogado
    '------------------------------------------------------------------/
    Protected Sub imgBtnBorraFecEjecObli_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecEjecObli.Click
        txtMT_fecha_ejecutoriaObli.Text = ""
    End Sub


    '/-----------------------------------------------------------------  
    'ID _HU:  002 0
    'Nombre HU   : Obligatoriedad de Campos fecha de ejecutoria y fecha de exigibilidad oficial
    'Empresa: UT TECHNOLOGY 
    'Autor: Jeisson Gómez 
    'Fecha: 06-01-2017 
    'Objetivo : Hacer obligatorios los campos de fecha ejecutoria y fecha de exigibilidad oficial,
    '           para el perfil Gestor - Abogado
    '------------------------------------------------------------------/
    Protected Sub imgBtnBorraFecExiLOBli_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExiLObli.Click
        txtMT_fec_exi_liqObli.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecExpTit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExpTit.Click
        txtMT_fec_expedicion_titulo.Text = ""
    End Sub

    Private Sub MostarDeuda(ByVal pTipoTitulo As String)
        ' 1. Retornar el nombre del título. 
        lblNombreTitulo.Text = obtenerNombreTitulo(pTipoTitulo)

        ' Para los títulos 
        ' 04 - REQUERIMIENTO PARA DECLARAR Y/O CORREGIR
        ' 08 - LIQUIDACIÓN OFICIAL / SANCIÓN
        ' No se oculta nada


        '/-----------------------------------------------------------------  
        'ID _HU:  012
        'Nombre HU: Desagregación Obligación  
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 16-06-2017  
        'Objetivo : Cambio criterio de aceptación 5. 
        ' Antes: 5.	Garantizar que la captura de los campos indicados en la Fig. 1, se implemente para el Rol Repartidor. 
        ' Ahora: 5.	Validar que el ajuste se aplique para todos los Roles. 
        ' Por lo tanto se comenta el If y la parte Else. 
        '------------------------------------------------------------------/

        'If Session("mnivelacces") = 5 Then

        Select Case pTipoTitulo
            ' Para el título 
            ' 01 - LIQUIDACIÓN OFICIAL
            Case "01"
                trSancionOmision.Style.Add("Display", "none")
                trSancionMora.Style.Add("Display", "none")
                trSancionInexactitud.Style.Add("Display", "none")
                trTotalSancion.Style.Add("Display", "none")

                ' Para los títulos 
                ' 07 SANCIÓN L1607/12
                ' 02 INFORME DE FISCALIZACIÓN
                ' 03 CUOTA PARTE PENSIONAL
                ' 05 RESOLUCIÓN MULTA L1438/11
                ' 06 SENTENCIA JUDICIAL
                ' 09 OTROS
                ' 10 SANCION DISCIPLINARIA
                ' 11 MAYORES VALORES POR COMPARTIBILIDAD
                ' 12 MAYORES VALORES PAGADOS POR FRAUDE
                ' 13 MAYOR VALOR PAGADO POR SOBREVIVIENTE
                ' 14 MAYOR VALOR PAGADO POR ORDEN JUDICIAL
                ' 15 MAYOR VALOR PAGADO
                ' 16 COSTAS PROCESALES
                ' 17 PLIEGO DE CARGOS
            Case "07", "02", "03", "05", "06", "09", "10", "11", "12", "13", "14", "15", "16", "17"
                trPartidaGlobal.Style.Add("Display", "none")
                trSancionOmision.Style.Add("Display", "none")
                trSancionMora.Style.Add("Display", "none")
                trSancionInexactitud.Style.Add("Display", "none")
                trTotalPartidaGlobal.Style.Add("Display", "none")
                trTotalSancion.Style.Add("Display", "none")
        End Select

        'Else
        '    txtValorObligacion.Enabled = False
        '    txtPartidaGlobal.Enabled = False
        '    txtSancionMora.Enabled = False
        '    txtSancionInexactitud.Enabled = False
        '    txtSancionOmision.Enabled = False

        'End If

        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'If pTipoTitulo = "01" Or pTipoTitulo = "02" Or pTipoTitulo = "04" Then ' 01=LIQUIDACIÓN OFICIAL; 02=INFORME DE FISCALIZACIÓN; 04=REQUERIMIENTO PARA DECLARAR Y/O CORREGIR
        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'infoSentencias.Visible = False
        'infoMulta.Visible = False
        'infoDetalleDeuda.Visible = True
        'trSancion.Visible = False
        'trMulta.Visible = False
        'trValorSentencia.Visible = False
        'trValorCuotasPartes.Visible = False

        'infoTotalesDeuda.Visible = True


        'trValorOmiso.Visible = True
        'trValorMora.Visible = True
        'trValorInexacto.Visible = True


        'ElseIf pTipoTitulo = "03" Then ' 03=CUOTA PARTE PENSIONAL
        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'tdmulta.InnerHtml = "Capital Cuotas Partes"
        'infoSentencias.Visible = False
        'infoMulta.Visible = False
        'infoDetalleDeuda.Visible = False

        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        ' Total Valores

        'trValorOmiso.Visible = False
        'trValorMora.Visible = False
        'trValorInexacto.Visible = False
        'trSancion.Visible = False
        'trMulta.Visible = False
        'trValorSentencia.Visible = False
        'trValorCuotasPartes.Visible = True

        'ElseIf pTipoTitulo = "05" Then ' 05=RESOLUCIÓN MULTA L1438/11
        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'infoSentencias.Visible = False
        'infoMulta.Visible = True
        'infoDetalleDeuda.Visible = False
        'trSancion.Visible = False

        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'Totales de valores
        'trMulta.Visible = False
        'trValorOmiso.Visible = False
        'trValorMora.Visible = False
        'trValorInexacto.Visible = False
        'trValorSentencia.Visible = False
        'trValorCuotasPartes.Visible = False

        'ElseIf pTipoTitulo = "06" Then ' 06=SENTENCIA JUDICIAL
        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'infoSentencias.Visible = True
        'infoMulta.Visible = False
        'infoDetalleDeuda.Visible = False

        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'Total Valores
        'trValorOmiso.Visible = False
        'trValorMora.Visible = False
        'trValorInexacto.Visible = False
        'trSancion.Visible = False
        'trMulta.Visible = False
        'trValorSentencia.Visible = False
        'trValorCuotasPartes.Visible = False

        'ElseIf pTipoTitulo = "07" Then '07=SANCIÓN L1607/12
        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'tdmulta.InnerHtml = "Capital Sanción"

        'infoSentencias.Visible = False
        'infoMulta.Visible = True
        'infoDetalleDeuda.Visible = False

        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'Totes de valores
        'trSancion.Visible = False
        'trMulta.Visible = False
        'trValorOmiso.Visible = False
        'trValorMora.Visible = False
        'trValorInexacto.Visible = False
        'trValorSentencia.Visible = False
        'trValorCuotasPartes.Visible = False

        'ElseIf pTipoTitulo = "08" Then ' 08=LIQUIDACIÓN OFICIAL / SANCIÓN
        ' Se comenta por HU_012 Adicionar campo saldo inicial y sanción 
        ' Jeisson Gómez 17/02/2016
        'tdmulta.InnerHtml = "Capital Sanción"
        'infoSentencias.Visible = False
        'infoMulta.Visible = True
        'infoDetalleDeuda.Visible = True
        'trSancion.Visible = True

        'trMulta.Visible = False
        'trValorSentencia.Visible = False
        'trValorCuotasPartes.Visible = False

        'trValorOmiso.Visible = True
        'trValorMora.Visible = True
        'trValorInexacto.Visible = True
        'Else
        '    infoSentencias.Visible = False
        '    infoMulta.Visible = True

        '    trValorOmiso.Visible = True
        '    trValorMora.Visible = True
        '    trValorInexacto.Visible = True

        '    infoDetalleDeuda.Visible = True
        '    trSancion.Visible = False
        '    trMulta.Visible = True
        '    trValorSentencia.Visible = False
        '    trValorCuotasPartes.Visible = False

        '    txtcapitalmulta.Enabled = True

        'End If

    End Sub

    Protected Sub btnrevocatoria_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnrevocatoria.Click

        Try

            If cborevocatoria.SelectedValue = "0" Then
                CustomValidator4.Text = "Digite un tipo de revocatoria..."
                CustomValidator4.IsValid = False
                Exit Sub
            End If

            If txtResolucionRevocaroria.Text = "" Then
                CustomValidator4.Text = "Digite un numero de resolución válida..."
                CustomValidator4.IsValid = False
                Exit Sub
            End If

            If Not IsDate(txtfecharevocatoria.Text) Then
                CustomValidator4.Text = "Digite una fecha válida..."
                CustomValidator4.IsValid = False
                Exit Sub
            End If

            txtvalorrevocatoria.Text = IIf(txtvalorrevocatoria.Text = "", "0", txtvalorrevocatoria.Text)
            If cborevocatoria.SelectedValue = "1" Or cborevocatoria.SelectedValue = "2" Then
                If Not IsNumeric(txtvalorrevocatoria.Text) Then
                    CustomValidator4.Text = "Digite un valor valido..."
                    CustomValidator4.IsValid = False
                    Exit Sub
                End If

                If txtvalorrevocatoria.Text = "0" Then
                    CustomValidator4.Text = "Digite el valor de la revocatoria..."
                    CustomValidator4.IsValid = False
                    Exit Sub
                End If

            Else
                If Not IsNumeric(txtvalorrevocatoria.Text) Then
                    CustomValidator4.Text = "Digite un valor valido..."
                    CustomValidator4.IsValid = False
                    Exit Sub
                Else
                    txtvalorrevocatoria.Text = "0"
                End If
            End If

            UpdateRevocatoria(cborevocatoria.SelectedValue, txtResolucionRevocaroria.Text, CDate(txtfecharevocatoria.Text), CDbl(txtvalorrevocatoria.Text), Request("pIdUnico"))


        Catch ex As Exception
            CustomValidator4.Text = ex.Message
            CustomValidator4.IsValid = False
        End Try

    End Sub

    Private Sub UpdateRevocatoria(ByVal revocatoria As String, ByVal nroResolRevoca As String, ByVal fechaRevoca As Date, ByVal valorRevoca As Double, ByVal idUnico As String)
        Dim Sql As String = "Update MAESTRO_TITULOS set revocatoria = @revocatoria, nroResolRevoca = @nroResolRevoca, fechaRevoca = @fechaRevoca, valorRevoca = @valorRevoca " &
            "WHERE idunico = @idunico "
        Dim cmd As New SqlClient.SqlCommand(Sql, New SqlClient.SqlConnection(Funciones.CadenaConexion))
        cmd.Parameters.AddWithValue("@revocatoria", revocatoria)
        cmd.Parameters.AddWithValue("@nroResolRevoca", nroResolRevoca)
        cmd.Parameters.AddWithValue("@fechaRevoca", fechaRevoca)
        cmd.Parameters.AddWithValue("@valorRevoca", valorRevoca)
        cmd.Parameters.AddWithValue("@idunico", idUnico)

        Try
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            CustomValidator4.Text = "Datos guardados satisfactoriamente... "
            CustomValidator4.IsValid = False


        Catch ex As Exception
            CustomValidator4.Text = ex.Message
            CustomValidator4.IsValid = False
        Finally

            cmd.Connection.Close()
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        cborevocatoria.SelectedValue = "0"
        txtResolucionRevocaroria.Text = ""
        txtfecharevocatoria.Text = ""
        txtvalorrevocatoria.Text = ""
        'txtvalorrevocatoria.Enabled = True
    End Sub

    Private Sub cboMT_tipo_titulo_TextChanged(sender As Object, e As EventArgs) Handles cboMT_tipo_titulo.TextChanged

        If Session("mnivelacces") = 5 Then 'Nivel 5 = Repartidor
            MostarDeuda(cboMT_tipo_titulo.SelectedValue)
        End If

    End Sub

    ' Se elabora como parte de la Historia de usuario  HU_012 Adicionar campo saldo inicial y sanción 
    ' Jeisson Gómez 20/02/2016
    Private Function obtenerNombreTitulo(ByVal strTipoTitulo As String)
        Dim result As String = String.Empty

        Try
            Using con = New SqlConnection(Funciones.CadenaConexion)
                Using cmd = New SqlCommand("SELECT nombre FROM TIPOS_TITULO WHERE codigo = @codigo", con)
                    cmd.Parameters.AddWithValue("@codigo", strTipoTitulo)
                    con.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader
                        If reader.Read Then
                            result = reader("nombre").ToString()
                        Else
                            result = String.Empty
                        End If
                    End Using
                    con.Close()
                End Using
            End Using

        Catch ex As Exception
            CustomValidator1.Text = "Ha ocurrido una excepción en el sistema " + ex.Message
            CustomValidator1.IsValid = False
        End Try
        Return result
    End Function

    Protected Function ValidarTitulo(ByVal nroTitulo As String, ByVal nroExpediente As String) As Boolean

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Dim sql As String = String.Empty
        Dim blnReturn As Boolean = False

        Try
            sql = "SELECT COUNT(1) As Cuenta FROM MAESTRO_TITULOS WHERE MT_nro_titulo = '" & nroTitulo & "' AND MT_expediente = '" & nroExpediente & "'"
            Dim Command As New SqlCommand(sql, Connection)
            Connection.Open()
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                blnReturn = IIf(Reader("Cuenta") > 0, False, True)
            End If
            Return blnReturn
        Catch ex As Exception
            CustomValidator1.Text = "Ha ocurrido una excepción en el sistema " + ex.Message
            CustomValidator1.IsValid = False
            Return blnReturn
        Finally
            Connection.Close()
        End Try
    End Function

    Protected Sub lsvListaDocuentos_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lsvListaDocumentos.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim lblNombreTituloItem As Label = CType(e.Item.FindControl("lblNameDoc"), Label)
            Dim HdnPathFile As HiddenField = CType(e.Item.FindControl("HdnPathFile"), HiddenField)
            Dim HdnIdMaestroTitulos As HiddenField = CType(e.Item.FindControl("HdnIdMaestroTitulos"), HiddenField)
            Dim LblArchivo As Label = CType(e.Item.FindControl("LblArchivo"), Label)
            'CumpleNoCumple

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

            Dim HdnIdDoc As HiddenField = CType(e.Item.FindControl("HdnIdDoc"), HiddenField)
            HdnIdDoc.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).ID_DOCUMENTO_TITULO
            If String.IsNullOrEmpty(CType(e.Item.DataItem, DocumentoTipoTitulo).DES_RUTA_DOCUMENTO) = False Then
                HdnPathFile.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).DES_RUTA_DOCUMENTO
                LblArchivo.Text = HdnPathFile.Value.Split("\").Last()
            End If
            If CType(e.Item.DataItem, DocumentoTipoTitulo).ID_MAESTRO_DOCUMENTO <> 0 Then
                HdnIdMaestroTitulos.Value = CType(e.Item.DataItem, DocumentoTipoTitulo).ID_MAESTRO_DOCUMENTO
            End If
            lblNombreTituloItem.Text = CType(e.Item.DataItem, DocumentoTipoTitulo).NOMBRE_DOCUMENTO
        End If
    End Sub
End Class