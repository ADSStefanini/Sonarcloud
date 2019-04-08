Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EditMAESTRO_TITULOS
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboMT_tipo_titulo()
            LoadcboMT_for_notificacion_titulo()
            LoadcboMT_for_not_reso_resu_reposicion()
            LoadcboMT_for_not_reso_apela_recon()
            LoadcboPROCEDENCIA()
            LoadcboCausalDevol()

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
                    txtMT_fecha_ejecutoria.Text = Left(Reader("MT_fecha_ejecutoria").ToString().Trim, 10)
                    txtMT_fec_exi_liq.Text = Left(Reader("MT_fec_exi_liq").ToString().Trim, 10)
                    txtMT_fec_cad_presc.Text = Left(Reader("MT_fec_cad_presc").ToString().Trim, 10)                    

                    'Mostrar datos de la devolucion si existe
                    txtNumMemoDev.Text = Reader("NumMemoDev").ToString().Trim
                    txtFecMemoDev.Text = Left(Reader("FecMemoDev").ToString().Trim, 10)
                    cboCausalDevol.SelectedValue = Reader("CausalDevol").ToString().Trim
                    taObsDevol.InnerHtml = Reader("ObsDevol").ToString().Trim

                    txtcapitalmulta.Text = Reader("capitalmulta").ToString

                    'Mostrar infor de la deuda del titulo
                    Dim datosDeuda As DataTable = Informacion_deuda(Request("pExpediente"))

                    If datosDeuda.Rows.Count > 0 Then
                        Inicializardeuda()
                        For Each row As DataRow In datosDeuda.Rows
                            If row("ID_GRUPO") = "1" Then

                                txttotalomisos.Text = datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='1'")
                                
                                If row("SUBSISTEMA") = "SENA" Then
                                    txtomisossena.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "PENSION" Then
                                    txtomisospensiones.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "ARL" Then
                                    txtomisosarl.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "ICBF" Then
                                    txtomisosicbf.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "CCF" Then
                                    txtomisossubfamiliar.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "SALUD" Then
                                    txtomisossalud.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "FSP" Then
                                    txtomisosfondosolpen.Text = row("DEUDA")
                                End If

                            ElseIf row("ID_GRUPO") = "2" Then
                                txttotalinexactos.Text = datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='2'")
                                If row("SUBSISTEMA") = "SENA" Then
                                    txtinexactossena.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "PENSION" Then
                                    txtinexactospensiones.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "ARL" Then
                                    txtinexactosarl.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "ICBF" Then
                                    txtinexactosicbf.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "CCF" Then
                                    txtinexactossubfamiliar.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "SALUD" Then
                                    txtinexactossalud.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "FSP" Then
                                    txtinexactosfondosolpen.Text = row("DEUDA")
                                End If

                            ElseIf row("ID_GRUPO") = "3" Then
                                txttotalmora.Text = datosDeuda.Compute("SUM(DEUDA)", "ID_GRUPO='3'")
                                If row("SUBSISTEMA") = "SENA" Then
                                    txtmorasena.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "PENSION" Then
                                    txtmorapensiones.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "ARL" Then
                                    txtmoraarl.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "ICBF" Then
                                    txtmoraicbf.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "CCF" Then
                                    txtmorasubfamiliar.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "SALUD" Then
                                    txtmorasalud.Text = row("DEUDA")
                                ElseIf row("SUBSISTEMA") = "FSP" Then
                                    txtmorafondosolpen.Text = row("DEUDA")
                                End If

                            End If
                        Next
                        txttotaldeuda.Text = datosDeuda.Compute("SUM(DEUDA)", String.Empty)
                    Else
                        Inicializardeuda()
                        If Session("mnivelacces") <> 5 Then 'Nivel 5 = Repartidor
                            If cboMT_tipo_titulo.SelectedValue = "01" Or cboMT_tipo_titulo.SelectedValue = "04" Then
                                CustomValidator1.Text = "No se pudo cargar la información de la deuda ya que no se encontro el SQL asociado al expediente. "
                                CustomValidator1.IsValid = False
                            End If
                        End If

                        txtcapitalmulta.Text = Reader("capitalmulta").ToString
                        txttotaldeuda.Text = Reader("totaldeuda").ToString

                    End If
                    'Mostrar info de la deuda del titulo
                    txtsentenciasjudiciales.Text = Reader("sentenciasjudiciales").ToString
                    txtcuotaspartesacum.Text = Reader("cuotaspartesacum").ToString
                    txttotalmultas.Text = Reader("totalmultas").ToString
                    txttotalsentencias.Text = Reader("totalsentencias").ToString
                    txtTotalRepartidor.Text = Reader("totalrepartidor").ToString

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

    Private Sub ActivarControles()
        Dim Habilitado As Boolean
        If Session("mnivelacces") = 5 Or Session("mnivelacces") = 8 Then
            ' Es el repartidor. Solo activar los siguientes campos 
            '--30/07/2014. Tambien activar si es el gestor de informacion

            txtMT_nro_titulo.Enabled = True
            txtMT_fec_expedicion_titulo.Enabled = True
            txtTotalRepartidor.Enabled = True
            cboPROCEDENCIA.Enabled = True
            cboMT_tipo_titulo.Enabled = True
            '           
            imgBtnBorraFecExpTit.Visible = True

            lblLeyendaRepartidor.Visible = True

            ' Los demas se deshabilitan  
            If Session("mnivelacces") = 5 Then
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
            txtMT_fecha_ejecutoria.Enabled = Habilitado
            txtMT_fec_exi_liq.Enabled = Habilitado
            txtMT_fec_cad_presc.Enabled = Habilitado
            'Imagenes de eliminar la fecha del calendario
            imgBtnBorraFecNotTit.Visible = Habilitado
            imgBtnBorraFecExpRR.Visible = Habilitado
            imgBtnBorraFecNotRRR.Visible = Habilitado
            imgBtnBorraFecExpRAR.Visible = Habilitado
            imgBtnBorraFecNotRAR.Visible = Habilitado
            imgBtnBorraFecEjec.Visible = Habilitado
            imgBtnBorraFecExiLO.Visible = Habilitado

        Else
            ' Es un perfil <> de repartidor. Desactivar los siguientes campos
            txtMT_nro_titulo.Enabled = False
            txtMT_fec_expedicion_titulo.Enabled = False
            txtTotalRepartidor.Enabled = False
            cboPROCEDENCIA.Enabled = False
            cboMT_tipo_titulo.Enabled = False

            lblLeyendaRepartidor.Visible = False
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
            txtMT_fecha_ejecutoria.Enabled = True
            txtMT_fec_exi_liq.Enabled = True
            txtMT_fec_cad_presc.Enabled = True
            '
            imgBtnBorraFecNotTit.Visible = True
            imgBtnBorraFecExpRR.Visible = True
            imgBtnBorraFecNotRRR.Visible = True
            imgBtnBorraFecExpRAR.Visible = True
            imgBtnBorraFecNotRAR.Visible = True
            imgBtnBorraFecEjec.Visible = True
            imgBtnBorraFecExiLO.Visible = True
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

    Private Sub RegistrarCambioEstado(ByVal pNumExpediente As String, ByVal pRepartidor As String, ByVal pRevisor As String, ByVal pAbogado As String, ByVal pFecha As Date, ByVal pEstado As String, ByVal pEstadoPago As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        'Declare string InsertSQL 
        Dim InsertSQL As String = "INSERT INTO CAMBIOS_ESTADO (NroExp, repartidor, revisor, abogado, fecha, estado, estadopago) VALUES (@NroExp, @repartidor, @revisor, @abogado, @fecha, @estado, @estadopago)"
        Dim Command As SqlCommand = New SqlCommand(InsertSQL, Connection)

        'Parametros
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Command.Parameters.AddWithValue("@repartidor", pRepartidor)
        Command.Parameters.AddWithValue("@revisor", pRevisor)
        Command.Parameters.AddWithValue("@abogado", pAbogado)
        Command.Parameters.AddWithValue("@fecha", pFecha)
        Command.Parameters.AddWithValue("@estado", pEstado)
        Command.Parameters.AddWithValue("@estadopago", pEstadoPago)

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

    Private Function HayCambioEstado(ByVal pNumExpediente As String, ByVal pRepartidor As String, ByVal pRevisor As String, ByVal pAbogado As String, ByVal pEstado As String, ByVal pEstadoPago As String) As Boolean
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

            'Preguntar si algun campo es diferente
            If RepartidorActual <> pRepartidor Or AbogadoActual <> pAbogado Or EstadoActual <> pEstado Or EstadoPagoActual <> pEstadoPago Or RevisorActual <> pRevisor Then
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
        Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " & _
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
        Connection.Open()
        'Consultar el total de la deuda
        Const sql As String = "SELECT A.ID_GRUPO,A.SUBSISTEMA,SUM(A.AJUSTE) AS DEUDA FROM SQL_PLANILLA A, GRUPOS B WHERE A.ID_GRUPO = B.ID_GRUPO AND  EXPEDIENTE = @EXPEDIENTE GROUP BY A.SUBSISTEMA,A.ID_GRUPO"
        Dim command As New SqlCommand(sql, connection)
        command.Parameters.AddWithValue("@EXPEDIENTE", expediente)

        Dim dts As New SqlDataAdapter(Command)
        Dim table As New DataTable
        dts.Fill((table))

        Return table

    End Function

    Private Sub Inicializardeuda()
        txtcapitalmulta.Text = 0
        txtomisossalud.Text = 0
        txtmorasalud.Text = 0
        txtinexactossalud.Text = 0
        txtomisospensiones.Text = 0
        txtmorapensiones.Text = 0
        txtinexactospensiones.Text = 0
        txtomisosfondosolpen.Text = 0
        txtmorafondosolpen.Text = 0
        txtinexactosfondosolpen.Text = 0
        txtomisosarl.Text = 0
        txtmoraarl.Text = 0
        txtinexactosarl.Text = 0
        txtomisosicbf.Text = 0
        txtmoraicbf.Text = 0
        txtinexactosicbf.Text = 0
        txtomisossena.Text = 0
        txtmorasena.Text = 0
        txtinexactossena.Text = 0
        txtomisossubfamiliar.Text = 0
        txtmorasubfamiliar.Text = 0
        txtinexactossubfamiliar.Text = 0
        txtsentenciasjudiciales.Text = 0
        txtcuotaspartesacum.Text = 0
        txttotalmultas.Text = 0
        txttotalomisos.Text = 0
        txttotalmora.Text = 0
        txttotalinexactos.Text = 0
        txttotalsentencias.Text = 0
        txttotaldeuda.Text = 0
        txtTotalRepartidor.Text = 0
    End Sub

    Protected Sub cmdCancel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel2.Click
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdCancel3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel3.Click
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Private Sub ActualizarDatosFinales(ByVal Connection As SqlConnection)
        Dim TotalExp As Int64 = ConsultarTotalDeuda(Request("pExpediente"))
        ActualizarTotalDeuda(Request("pExpediente"), TotalExp)

        'Obtener los datos del expediente actual
        Dim repartidor, revisor, abogado, estadopago As String
        repartidor = ""
        revisor = ""
        abogado = ""
        estadopago = ""
        Dim sql As String = "SELECT repartidor, revisor, abogado, estadopago FROM CAMBIOS_ESTADO WHERE idunico = (SELECT MAX(idunico) FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp)"
        Connection.Open()
        Dim Command2 As New SqlCommand(sql, Connection)
        Command2.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Dim Reader2 As SqlDataReader = Command2.ExecuteReader
        If Reader2.Read Then
            repartidor = Reader2("repartidor").ToString().Trim
            revisor = Reader2("revisor").ToString().Trim
            abogado = Reader2("abogado").ToString().Trim
            estadopago = Reader2("estadopago").ToString().Trim
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
                If HayCambioEstado(Request("pExpediente"), repartidor, revisor, abogado, "04", estadopago) Then
                    ' Colocar los datos ACTUALES del proceso, pero cambiar el estado a "04"=DEVUELTO en la fecha de hoy 
                    RegistrarCambioEstado(Request("pExpediente").Trim, repartidor, revisor, abogado, DateTime.Now(), "04", estadopago)
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

                If HayCambioEstado(Request("pExpediente"), repartidor, revisor, abogado, "09", estadopago) Then
                    ' Colocar los datos ACTUALES del proceso, pero cambiar el estado a "09"=REPARTO en la fecha de hoy 
                    RegistrarCambioEstado(Request("pExpediente").Trim, repartidor, revisor, abogado, DateTime.Now(), "09", estadopago)
                End If
            End If
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

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

        Dim ID As String = Request("ID")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into MAESTRO_TITULOS ([MT_nro_titulo], [MT_expediente], [MT_tipo_titulo], [MT_titulo_acumulado], [MT_fec_expedicion_titulo], [MT_fec_notificacion_titulo], [MT_for_notificacion_titulo], [MT_res_resuelve_reposicion], [MT_fec_expe_resolucion_reposicion], [MT_fec_not_reso_resu_reposicion], [MT_for_not_reso_resu_reposicion], [MT_reso_resu_apela_recon], [MT_fec_exp_reso_apela_recon], [MT_fec_not_reso_apela_recon], [MT_for_not_reso_apela_recon], [MT_fecha_ejecutoria], [MT_fec_exi_liq], [MT_fec_cad_presc], [MT_procedencia]) " & _
                            "VALUES ( @MT_nro_titulo, @MT_expediente, @MT_tipo_titulo, @MT_titulo_acumulado, @MT_fec_expedicion_titulo, @MT_fec_notificacion_titulo, @MT_for_notificacion_titulo, @MT_res_resuelve_reposicion, @MT_fec_expe_resolucion_reposicion, @MT_fec_not_reso_resu_reposicion, @MT_for_not_reso_resu_reposicion, @MT_reso_resu_apela_recon, @MT_fec_exp_reso_apela_recon, @MT_fec_not_reso_apela_recon, @MT_for_not_reso_apela_recon, @MT_fecha_ejecutoria, @MT_fec_exi_liq, @MT_fec_cad_presc, @MT_procedencia) "

        Dim UpdateSQL As String = "Update MAESTRO_TITULOS set [MT_nro_titulo] = @MT_nro_titulo, [MT_tipo_titulo] = @MT_tipo_titulo, [MT_titulo_acumulado] = @MT_titulo_acumulado, [MT_fec_expedicion_titulo] = @MT_fec_expedicion_titulo, [MT_fec_notificacion_titulo] = @MT_fec_notificacion_titulo, [MT_for_notificacion_titulo] = @MT_for_notificacion_titulo, [MT_res_resuelve_reposicion] = @MT_res_resuelve_reposicion, [MT_fec_expe_resolucion_reposicion] = @MT_fec_expe_resolucion_reposicion, [MT_fec_not_reso_resu_reposicion] = @MT_fec_not_reso_resu_reposicion, [MT_for_not_reso_resu_reposicion] = @MT_for_not_reso_resu_reposicion, [MT_reso_resu_apela_recon] = @MT_reso_resu_apela_recon, [MT_fec_exp_reso_apela_recon] = @MT_fec_exp_reso_apela_recon, [MT_fec_not_reso_apela_recon] = @MT_fec_not_reso_apela_recon, [MT_for_not_reso_apela_recon] = @MT_for_not_reso_apela_recon, [MT_fecha_ejecutoria] = @MT_fecha_ejecutoria, [MT_fec_exi_liq] = @MT_fec_exi_liq, [MT_fec_cad_presc] = @MT_fec_cad_presc, [MT_procedencia] = @MT_procedencia " & _
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

        If IsDate(txtMT_fecha_ejecutoria.Text.Trim) Then
            'Command.Parameters.AddWithValue("@MT_fecha_ejecutoria", Left(txtMT_fecha_ejecutoria.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fecha_ejecutoria", Date.ParseExact(Left(txtMT_fecha_ejecutoria.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fecha_ejecutoria", DBNull.Value)
        End If

        If IsDate(Left(txtMT_fec_exi_liq.Text.Trim, 10)) Then
            'Command.Parameters.AddWithValue("@MT_fec_exi_liq", Left(txtMT_fec_exi_liq.Text.Trim, 10))
            Command.Parameters.AddWithValue("@MT_fec_exi_liq", Date.ParseExact(Left(txtMT_fec_exi_liq.Text.Trim, 10), "dd/MM/yyyy", Nothing))
        Else
            Command.Parameters.AddWithValue("@MT_fec_exi_liq", DBNull.Value)
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
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de títulos ejecutivos", "No. título " & txtMT_nro_titulo.Text.Trim, Command)

            SwOK = True
        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
            Return
        End Try
        'Close the Connection Object 
        Connection.Close()

        If SwOK Then
            'Actualizar datos finales
            ActualizarDatosFinales(Connection)
        End If
        '
        Response.Redirect("MAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdSave2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave2.Click

        'Este boton solo va a a guardar los campos de la deuda

        If Session("mnivelacces") <> 5 Then
            If txttotaldeuda.Text <> txtTotalRepartidor.Text Then
                CustomValidator2.Text = "El valor digitado por el repartidor y el gestor no coinciden. Favor verificar."
                CustomValidator2.IsValid = False
                Return
            End If
        End If

        If Session("mnivelacces") <> 5 Then
            If IsNumeric(txttotaldeuda.Text) Then
                If Convert.ToInt32(txttotaldeuda.Text) = 0 Then
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
        Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        'Declare string InsertSQL 
        Dim InsertSQL As String = "INSERT INTO maestro_titulos (capitalmulta, omisossalud, morasalud, inexactossalud, omisospensiones, " & _
                                        "morapensiones, inexactospensiones, omisosfondosolpen, morafondosolpen, inexactosfondosolpen, " & _
                                        "omisosarl, moraarl, inexactosarl, omisosicbf, moraicbf, inexactosicbf, omisossena, morasena, " & _
                                        "inexactossena, omisossubfamiliar, morasubfamiliar, inexactossubfamiliar, sentenciasjudiciales, " & _
                                        "cuotaspartesacum, totalmultas, totalomisos, totalmora, totalinexactos, totalsentencias, " & _
                                        "totaldeuda, MT_procedencia, NumMemoDev, FecMemoDev, CausalDevol, ObsDevol, TotalRepartidor ) " & _
                                   "VALUES ( @capitalmulta, @omisossalud, @morasalud, @inexactossalud, @omisospensiones, @morapensiones, " & _
                                        "@inexactospensiones, @omisosfondosolpen, @morafondosolpen, @inexactosfondosolpen, @omisosarl, " & _
                                        "@moraarl, @inexactosarl, @omisosicbf, @moraicbf, @inexactosicbf, @omisossena, @morasena, " & _
                                        "@inexactossena, @omisossubfamiliar, @morasubfamiliar, @inexactossubfamiliar, " & _
                                        "@sentenciasjudiciales, @cuotaspartesacum, @totalmultas, @totalomisos, @totalmora, " & _
                                        "@totalinexactos, @totalsentencias, @totaldeuda, @TotalRepartidor ) "

        Dim UpdateSQL As String = "UPDATE maestro_titulos SET capitalmulta = @capitalmulta, omisossalud = @omisossalud, morasalud = @morasalud, " & _
                                        "inexactossalud = @inexactossalud, omisospensiones = @omisospensiones, morapensiones = @morapensiones, " & _
                                        "inexactospensiones = @inexactospensiones, omisosfondosolpen = @omisosfondosolpen, " & _
                                        "morafondosolpen = @morafondosolpen, inexactosfondosolpen = @inexactosfondosolpen, " & _
                                        "omisosarl = @omisosarl, moraarl = @moraarl, inexactosarl = @inexactosarl, omisosicbf = @omisosicbf, " & _
                                        "moraicbf = @moraicbf, inexactosicbf = @inexactosicbf, omisossena = @omisossena, morasena = @morasena," & _
                                        "inexactossena = @inexactossena, omisossubfamiliar = @omisossubfamiliar, " & _
                                        "morasubfamiliar = @morasubfamiliar, inexactossubfamiliar = @inexactossubfamiliar, " & _
                                        "sentenciasjudiciales = @sentenciasjudiciales, cuotaspartesacum = @cuotaspartesacum, " & _
                                        "totalmultas = @totalmultas, totalomisos = @totalomisos, totalmora = @totalmora, " & _
                                        "totalinexactos = @totalinexactos, totalsentencias = @totalsentencias, totaldeuda = @totaldeuda, " & _
                                        "TotalRepartidor = @TotalRepartidor " & _
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

        '04/feb/2014
        'Parametros de la deuda
        If IsNumeric(txtcapitalmulta.Text) Then
            Command.Parameters.AddWithValue("@capitalmulta", txtcapitalmulta.Text)
        Else
            Command.Parameters.AddWithValue("@capitalmulta", DBNull.Value)
        End If

        If IsNumeric(txtomisossalud.Text) Then
            Command.Parameters.AddWithValue("@omisossalud", txtomisossalud.Text)
        Else
            Command.Parameters.AddWithValue("@omisossalud", DBNull.Value)
        End If

        If IsNumeric(txtmorasalud.Text) Then
            Command.Parameters.AddWithValue("@morasalud", txtmorasalud.Text)
        Else
            Command.Parameters.AddWithValue("@morasalud", DBNull.Value)
        End If

        If IsNumeric(txtinexactossalud.Text) Then
            Command.Parameters.AddWithValue("@inexactossalud", txtinexactossalud.Text)
        Else
            Command.Parameters.AddWithValue("@inexactossalud", DBNull.Value)
        End If

        If IsNumeric(txtomisospensiones.Text) Then
            Command.Parameters.AddWithValue("@omisospensiones", txtomisospensiones.Text)
        Else
            Command.Parameters.AddWithValue("@omisospensiones", DBNull.Value)
        End If

        If IsNumeric(txtmorapensiones.Text) Then
            Command.Parameters.AddWithValue("@morapensiones", txtmorapensiones.Text)
        Else
            Command.Parameters.AddWithValue("@morapensiones", DBNull.Value)
        End If

        If IsNumeric(txtinexactospensiones.Text) Then
            Command.Parameters.AddWithValue("@inexactospensiones", txtinexactospensiones.Text)
        Else
            Command.Parameters.AddWithValue("@inexactospensiones", DBNull.Value)
        End If

        If IsNumeric(txtomisosfondosolpen.Text) Then
            Command.Parameters.AddWithValue("@omisosfondosolpen", txtomisosfondosolpen.Text)
        Else
            Command.Parameters.AddWithValue("@omisosfondosolpen", DBNull.Value)
        End If

        If IsNumeric(txtmorafondosolpen.Text) Then
            Command.Parameters.AddWithValue("@morafondosolpen", txtmorafondosolpen.Text)
        Else
            Command.Parameters.AddWithValue("@morafondosolpen", DBNull.Value)
        End If

        If IsNumeric(txtinexactosfondosolpen.Text) Then
            Command.Parameters.AddWithValue("@inexactosfondosolpen", txtinexactosfondosolpen.Text)
        Else
            Command.Parameters.AddWithValue("@inexactosfondosolpen", DBNull.Value)
        End If

        If IsNumeric(txtomisosarl.Text) Then
            Command.Parameters.AddWithValue("@omisosarl", txtomisosarl.Text)
        Else
            Command.Parameters.AddWithValue("@omisosarl", DBNull.Value)
        End If

        If IsNumeric(txtmoraarl.Text) Then
            Command.Parameters.AddWithValue("@moraarl", txtmoraarl.Text)
        Else
            Command.Parameters.AddWithValue("@moraarl", DBNull.Value)
        End If

        If IsNumeric(txtinexactosarl.Text) Then
            Command.Parameters.AddWithValue("@inexactosarl", txtinexactosarl.Text)
        Else
            Command.Parameters.AddWithValue("@inexactosarl", DBNull.Value)
        End If

        If IsNumeric(txtomisosicbf.Text) Then
            Command.Parameters.AddWithValue("@omisosicbf", txtomisosicbf.Text)
        Else
            Command.Parameters.AddWithValue("@omisosicbf", DBNull.Value)
        End If

        If IsNumeric(txtmoraicbf.Text) Then
            Command.Parameters.AddWithValue("@moraicbf", txtmoraicbf.Text)
        Else
            Command.Parameters.AddWithValue("@moraicbf", DBNull.Value)

        End If

        If IsNumeric(txtinexactosicbf.Text) Then
            Command.Parameters.AddWithValue("@inexactosicbf", txtinexactosicbf.Text)
        Else
            Command.Parameters.AddWithValue("@inexactosicbf", DBNull.Value)

        End If

        If IsNumeric(txtomisossena.Text) Then
            Command.Parameters.AddWithValue("@omisossena", txtomisossena.Text)
        Else
            Command.Parameters.AddWithValue("@omisossena", DBNull.Value)

        End If

        If IsNumeric(txtmorasena.Text) Then
            Command.Parameters.AddWithValue("@morasena", txtmorasena.Text)
        Else
            Command.Parameters.AddWithValue("@morasena", DBNull.Value)
        End If

        If IsNumeric(txtinexactossena.Text) Then
            Command.Parameters.AddWithValue("@inexactossena", txtinexactossena.Text)
        Else
            Command.Parameters.AddWithValue("@inexactossena", DBNull.Value)

        End If

        If IsNumeric(txtomisossubfamiliar.Text) Then
            Command.Parameters.AddWithValue("@omisossubfamiliar", txtomisossubfamiliar.Text)
        Else
            Command.Parameters.AddWithValue("@omisossubfamiliar", DBNull.Value)

        End If

        If IsNumeric(txtmorasubfamiliar.Text) Then
            Command.Parameters.AddWithValue("@morasubfamiliar", txtmorasubfamiliar.Text)
        Else
            Command.Parameters.AddWithValue("@morasubfamiliar", DBNull.Value)

        End If

        If IsNumeric(txtinexactossubfamiliar.Text) Then
            Command.Parameters.AddWithValue("@inexactossubfamiliar", txtinexactossubfamiliar.Text)
        Else
            Command.Parameters.AddWithValue("@inexactossubfamiliar", DBNull.Value)
        End If

        If IsNumeric(txtsentenciasjudiciales.Text) Then
            Command.Parameters.AddWithValue("@sentenciasjudiciales", txtsentenciasjudiciales.Text)
        Else
            Command.Parameters.AddWithValue("@sentenciasjudiciales", DBNull.Value)
        End If

        If IsNumeric(txtcuotaspartesacum.Text) Then
            Command.Parameters.AddWithValue("@cuotaspartesacum", txtcuotaspartesacum.Text)
        Else
            Command.Parameters.AddWithValue("@cuotaspartesacum", DBNull.Value)

        End If

        If IsNumeric(txttotalmultas.Text) Then
            Command.Parameters.AddWithValue("@totalmultas", txttotalmultas.Text)
        Else
            Command.Parameters.AddWithValue("@totalmultas", DBNull.Value)

        End If

        If IsNumeric(txttotalomisos.Text) Then
            Command.Parameters.AddWithValue("@totalomisos", txttotalomisos.Text)
        Else
            Command.Parameters.AddWithValue("@totalomisos", DBNull.Value)

        End If

        If IsNumeric(txttotalmora.Text) Then
            Command.Parameters.AddWithValue("@totalmora", txttotalmora.Text)
        Else
            Command.Parameters.AddWithValue("@totalmora", DBNull.Value)

        End If

        If IsNumeric(txttotalinexactos.Text) Then
            Command.Parameters.AddWithValue("@totalinexactos", txttotalinexactos.Text)
        Else
            Command.Parameters.AddWithValue("@totalinexactos", DBNull.Value)
        End If

        If IsNumeric(txttotalsentencias.Text) Then
            Command.Parameters.AddWithValue("@totalsentencias", txttotalsentencias.Text)
        Else
            Command.Parameters.AddWithValue("@totalsentencias", DBNull.Value)
        End If

        Dim TotalRepartidor As Double = 0
        If IsNumeric(txtTotalRepartidor.Text) Then
            TotalRepartidor = CDbl(txtTotalRepartidor.Text)
        Else
            TotalRepartidor = 0
        End If


        If IsNumeric(txttotaldeuda.Text) Then
            '07/oct/2014. A la hora de adicionar un nuevo titulo, totaldeuda = TotalRepartidor
            If ModoAddEditTitulo = "ADICIONAR" Then
                Command.Parameters.AddWithValue("@totaldeuda", TotalRepartidor)
            Else
                ' Edicion
                If Session("mnivelacces") = 5 Or Session("mnivelacces") = 8 Then
                    'Para perfil repartidor o gestor de información 
                    Command.Parameters.AddWithValue("@totaldeuda", txtTotalRepartidor.Text)
                Else
                    ' Para abogados y demas perfiles
                    Command.Parameters.AddWithValue("@totaldeuda", txttotaldeuda.Text)
                End If

            End If
        Else
            Command.Parameters.AddWithValue("@totaldeuda", 0)
        End If

        'txtTotalRepartidor
        Command.Parameters.AddWithValue("@TotalRepartidor", TotalRepartidor)


        Dim SwOK As Boolean = False
        Try
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de títulos ejecutivos", "No. título " & txtMT_nro_titulo.Text.Trim, Command)

            SwOK = True

            '28/abril/2015. Yesid solicito que si se edita el valor del titulo => hay que afectar ejefisglobal
            'xxxyyy
            ActualizarValoresEjefisglobal(Request("pExpediente"), TotalRepartidor)

        Catch ex As Exception
            CustomValidator2.Text = ex.Message
            CustomValidator2.IsValid = False
            Connection.Close()
            Return
        End Try
        'Close the Connection Object 
        Connection.Close()

        If SwOK Then
            'Actualizar datos finales
            ActualizarDatosFinales(Connection)
        End If
        '
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
        Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into MAESTRO_TITULOS (NumMemoDev, FecMemoDev, CausalDevol, ObsDevol ) " & _
             "VALUES ( @NumMemoDev, @FecMemoDev, @CausalDevol, @ObsDevol ) "

        Dim UpdateSQL As String = "Update MAESTRO_TITULOS set NumMemoDev = @NumMemoDev, FecMemoDev = @FecMemoDev, CausalDevol = @CausalDevol, ObsDevol = @ObsDevol " & _
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
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de títulos ejecutivos", "No. título " & txtMT_nro_titulo.Text.Trim, Command)

            SwOK = True
        Catch ex As Exception
            CustomValidator2.Text = ex.Message
            CustomValidator2.IsValid = False
            Connection.Close()
            Return
        End Try
        'Close the Connection Object 
        Connection.Close()

        If SwOK Then
            'Actualizar datos finales
            ActualizarDatosFinales(Connection)
        End If
        '
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

    Protected Sub imgBtnBorraFecEjec_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecEjec.Click
        txtMT_fecha_ejecutoria.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecExiLO_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExiLO.Click
        txtMT_fec_exi_liq.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecExpTit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecExpTit.Click
        txtMT_fec_expedicion_titulo.Text = ""
    End Sub
End Class