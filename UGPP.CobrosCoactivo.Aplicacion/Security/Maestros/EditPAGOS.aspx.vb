Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Partial Public Class EditPAGOS
    Inherits System.Web.UI.Page

    Protected Shared tipoTitulo As String
    Protected Shared DatosImportado As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        finsession()

        'Evaluates to true when the page is loaded for the first time.
        tipoTitulo = Tipo_Titulo(Request("pExpediente"))
        If IsPostBack = False Then


            Loadcboestado()
            Loadcbopagestadoprocfrp()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert            
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM PAGOS where NroConsignacion = @NroConsignacion"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @NroConsignacion parameter in the Command select query
                Command.Parameters.AddWithValue("@NroConsignacion", HttpUtility.UrlDecode(Request("ID").ToString.Replace("+", "%2B")))

                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtNroConsignacion.Text = Reader("NroConsignacion").ToString()
                    txtNroRadicadoSalida.Text = Reader("pagNroRadicadoSalida").ToString()
                    txtNroExp.Text = Reader("NroExp").ToString().Trim
                    txtFecSolverif.Text = Left(Reader("FecSolverif").ToString().Trim, 10)
                    txtFecVerificado.Text = Left(Reader("FecVerificado").ToString().Trim, 10)
                    cboestado.SelectedValue = Reader("estado").ToString()
                    txtpagFecha.Text = Left(Reader("pagFecha").ToString().Trim, 10)
                    txtpagFechaDeudor.Text = Left(Reader("pagFechaDeudor").ToString().Trim, 10)
                    txtpagNroTitJudicial.Text = Reader("pagNroTitJudicial").ToString()
                    If IsDBNull(Reader("pagCapital")) Then
                        txtpagCapital.Text = ""
                    ElseIf Reader("pagCapital").ToString().Length = 1 Then
                        txtpagCapital.Text = Reader("pagCapital").ToString()
                    Else
                        txtpagCapital.Text = Reader("pagCapital")
                    End If
                    If IsDBNull(Reader("pagAjusteDec1406")) Then
                        txtpagAjusteDec1406.Text = ""
                    ElseIf Reader("pagAjusteDec1406").ToString().Length = 1 Then
                        txtpagAjusteDec1406.Text = Reader("pagAjusteDec1406").ToString()
                    Else
                        txtpagAjusteDec1406.Text = Reader("pagAjusteDec1406")
                    End If

                    '/-----------------------------------------------------------------  
                    'ID _HU:  015
                    'Nombre HU   : Error en Cálculo del IPC
                    'Empresa: UT TECHNOLOGY 
                    'Autor: Jeisson Gómez 
                    'Fecha: 06-01-2017 - 30-05-2017
                    'Objetivo : Realizar la desagregación de las obligaciones, 
                    ' se deja como estaba para que se muestre el valor del IPC
                    '------------------------------------------------------------------/

                    If IsDBNull(Reader("vlripc")) Or IsDBNull(Reader("pagInteres")) Then
                        txtpagInteres.Text = ""
                    ElseIf tipoTitulo = "07" And Reader("vlripc").ToString().Length = 1 Then
                        txtpagInteres.Text = Reader("vlripc").ToString()
                    ElseIf tipoTitulo <> "07" And Reader("pagInteres").ToString().Length = 1 Then
                        txtpagInteres.Text = Reader("pagInteres").ToString()
                    Else
                        txtpagInteres.Text = Reader("vlripc")
                    End If
                    If IsDBNull(Reader("pagGastosProc")) Then
                        txtpagGastosProc.Text = ""
                    ElseIf Reader("pagGastosProc").ToString().Length = 1 Then
                        txtpagGastosProc.Text = Reader("pagGastosProc").ToString()
                    Else
                        txtpagGastosProc.Text = Reader("pagGastosProc")
                    End If
                    If IsDBNull(Reader("pagExceso")) Then
                        txtpagExceso.Text = ""
                    ElseIf Reader("pagExceso").ToString().Length = 1 Then
                        txtpagExceso.Text = Reader("pagExceso").ToString()
                    Else
                        txtpagExceso.Text = Reader("pagExceso")
                    End If
                    If IsDBNull(Reader("pagTotal")) Then
                        txtpagTotal.Text = ""
                    ElseIf Reader("pagTotal").ToString().Length = 1 Then
                        txtpagTotal.Text = Reader("pagTotal").ToString()
                    Else
                        txtpagTotal.Text = Reader("pagTotal")
                    End If
                    cbopagestadoprocfrp.SelectedValue = Reader("pagestadoprocfrp").ToString()
                    'Datos de facilidades / acuerdos de pago
                    txtpagFecExi.Text = Left(Reader("pagFecExi").ToString().Trim, 10)
                    txtpagTasaIntApl.Text = Reader("pagTasaIntApl").ToString()
                    txtpagdiasmora.Text = Reader("pagdiasmora").ToString()
                    If IsDBNull(Reader("pagvalcuota")) Then
                        txtpagvalcuota.Text = ""
                    ElseIf Reader("pagvalcuota").ToString().Length = 1 Then
                        txtpagvalcuota.Text = Reader("pagvalcuota").ToString()
                    Else
                        txtpagvalcuota.Text = Reader("pagvalcuota")
                    End If
                    txtpagNumConPag.Text = Reader("pagNumConPag").ToString()

                    '23/JUL/2014. Mostrar datos del encabezado
                    Dim NumExp As String = Reader("NroExp").ToString().Trim
                    If NumExp <> "" Then
                        interesesIPC(NumExp)
                        MostrarEncabezado(NumExp)
                        'MostrarEA(NumExp)
                        MostrarGestorResp(NumExp)
                        MostrarTitulo(NumExp)
                        MostrarDeudor(NumExp)
                    End If

                End If
                Reader.Close()

                Dim sql2 As String = "SELECT EJEFISGLOBAL.*  " &
                                    "FROM EJEFISGLOBAL " &
                                    " " &
                                    "WHERE EJEFISGLOBAL.EFINROEXP = @EFINROEXP "
                '              
                Dim Command2 As New SqlCommand(sql2, Connection)
                Command2.Parameters.AddWithValue("@EFINROEXP", txtNroExp.Text)
                Dim Reader2 As SqlDataReader = Command2.ExecuteReader

                'Si se encontro el expediente
                If Reader2.Read Then
                    'Mostrar informacion general del expediente
                    txtExpOrigen.Text = Reader2("EFIEXPORIGEN").ToString()
                    txtExpDocumentic.Text = Reader2("EFIEXPDOCUMENTIC").ToString()
                End If
                Reader2.Close()

                Connection.Close()
            Else
                If Len(Request("pExpediente")) > 0 Then
                    Dim NumExp As String = Request("pExpediente").Trim
                    If NumExp <> "" Then
                        interesesIPC(NumExp)
                        MostrarEncabezado(NumExp)
                        MostrarEA(NumExp)
                        MostrarGestorResp(NumExp)
                        MostrarTitulo(NumExp)
                        MostrarDeudor(NumExp)
                        '
                        txtNroExp.Text = NumExp
                    End If


                    Dim sql2 As String = "SELECT EJEFISGLOBAL.*  " &
                                 "FROM EJEFISGLOBAL " &
                                 " " &
                                 "WHERE EJEFISGLOBAL.EFINROEXP = @EFINROEXP "
                    '              
                    Dim Command2 As New SqlCommand(sql2, Connection)
                    Command2.Parameters.AddWithValue("@EFINROEXP", txtNroExp.Text)
                    Dim Reader2 As SqlDataReader = Command2.ExecuteReader

                    'Si se encontro el expediente
                    If Reader2.Read Then
                        'Mostrar informacion general del expediente
                        txtExpOrigen.Text = Reader2("EFIEXPORIGEN").ToString()
                        txtExpDocumentic.Text = Reader2("EFIEXPDOCUMENTIC").ToString()
                    End If
                    Reader2.Close()

                    Connection.Close()

                End If

            End If
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)

            If Session("mnivelacces") <> 6 Then
                cmdSave.Visible = False
                'Desactivar controles
                txtNroRadicadoSalida.Enabled = False
                txtFecVerificado.Enabled = False
                cboestado.Enabled = False
                txtpagFecha.Enabled = False
                txtpagFechaDeudor.Enabled = False
                txtpagNroTitJudicial.Enabled = False
                txtpagCapital.Enabled = False
                txtpagAjusteDec1406.Enabled = False
                txtpagInteres.Enabled = False
                txtpagGastosProc.Enabled = False
                txtpagExceso.Enabled = False
                txtpagTotal.Enabled = False
                cbopagestadoprocfrp.Enabled = False
                txtpagFecExi.Enabled = False
                txtpagTasaIntApl.Enabled = False
                txtpagdiasmora.Enabled = False
                txtpagvalcuota.Enabled = False
                txtpagNumConPag.Enabled = False

            End If

            Tpgado()

        End If
    End Sub

    Private Sub finsession()
        Dim MTG As New MetodosGlobalesCobro
        If MTG.IsSessionTimedOut Then
            FormsAuthentication.SignOut()
            'Limpiar los cuadros de texto de busqueda
            Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
            Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
            Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
            Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
            Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""
            Session("Paginacion") = 10
            Response.Redirect("../../login.aspx")
        End If
    End Sub

    Private Sub MostrarEncabezado(ByVal pExpediente As String)
        'Conexion a la base de datos
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        If Len(pExpediente) > 0 Then
            Dim sql As String = "SELECT EJEFISGLOBAL.*, ESTADOS_PROCESO.nombre AS NomEstadoProc " &
                                               "FROM EJEFISGLOBAL " &
                                               "LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " &
                                               "WHERE EJEFISGLOBAL.EFINROEXP = @EFINROEXP"
            '              
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@EFINROEXP", pExpediente)
            Dim Reader As SqlDataReader = Command.ExecuteReader

            'Si se encontro el expediente
            If Reader.Read Then
                'Mostrar informacion general del expediente
                txtNroExpEnc.Text = Reader("EFINROEXP").ToString()
                txtNombreEstado.Text = Reader("NomEstadoProc").ToString().Trim
                txtFECENTREGAGESTOR.Text = Left(Reader("EFIFECENTGES").ToString(), 10)
            End If

            'Cerrar el Data Reader
            Reader.Close()

        End If

        'Cerrar conexion
        Connection.Close()
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
            'Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda,ISNULL(sum(valorRevoca),0) valorRevoca  " &
            '                              "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pNumExpediente.Trim & "' GROUP BY MT_expediente"

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
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' AND SnContabilizar = 1 GROUP BY NroExp"

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

            '/-----------------------------------------------------------------  
            'ID _HU:  015
            'Nombre HU   : Error en Cálculo del IPC
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 30-05-2017
            'Objetivo : Se realiza la suma de los valores del IPC, sin pagos y 
            ' con pagos como se hace en el método MostrarEA_LiquidacionSancion
            '------------------------------------------------------------------/

            'txtValorIPC.Text = CDbl(CDbl(Ipcsinpagos) + CDbl(ipc)).ToString("N0")
            txtValorIPC.Text = CDec(CDec(Ipcsinpagos)).ToString("N0")
            'Mostrar la diferencia entre Total deuda - Capital pagado
            Dim saldoEA As Decimal
            If tipoTitulo = "07" Then
                lblInte.InnerText = "Valor IPC"
                txtpagInteresLiq.Text = txtValorIPC.Text
                saldoEA = Convert.ToDecimal(txtTotalDeudaEA.Text) - Convert.ToDecimal(txtValorRevocatoria.Text) - Convert.ToDecimal(txtPagosCapitalEA.Text) + Convert.ToDecimal(txtValorIPC.Text)
            Else
                saldoEA = Convert.ToDecimal(txtTotalDeudaEA.Text) - Convert.ToDecimal(txtPagosCapitalEA.Text)
                txtValorIPC.Text = "NO APLICA"
            End If

            txtSaldoEA.Text = saldoEA.ToString("N0")

            Connection.Close()
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

            If tipoTitulo = "08" Then
                'Veneno
                MostrarEA_LiquidacionSancion(txtNroExp.Text.Trim)
            Else
                'Veneno
                MostrarEA(txtNroExp.Text.Trim)
                MostrarSancion(False)
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

    Protected Sub Loadcboestado()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM estados_pago ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEstados_pago As New DataTable
        Adaptador.Fill(dtEstados_pago)
        If dtEstados_pago.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtEstados_pago.NewRow()
            filaEstado("codigo") = "00"
            filaEstado("nombre") = " "
            dtEstados_pago.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaEstados_Pago As DataView = New DataView(dtEstados_pago)
            vistaEstados_Pago.Sort = "codigo"
            '--------------------------------------------------------------------

            cboestado.DataSource = vistaEstados_Pago
            cboestado.DataTextField = "nombre"
            cboestado.DataValueField = "codigo"
            cboestado.DataBind()
        End If
    End Sub

    Protected Sub Loadcbopagestadoprocfrp()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM ESTADOS_PROCESO ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtESTADOS_PROCESO As New DataTable
        Adaptador.Fill(dtESTADOS_PROCESO)
        If dtESTADOS_PROCESO.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtESTADOS_PROCESO.NewRow()
            filaEstado("codigo") = "00"
            filaEstado("nombre") = " "
            dtESTADOS_PROCESO.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaESTADOS_PROCESO As DataView = New DataView(dtESTADOS_PROCESO)
            vistaESTADOS_PROCESO.Sort = "codigo"
            '--------------------------------------------------------------------

            cbopagestadoprocfrp.DataSource = vistaESTADOS_PROCESO
            cbopagestadoprocfrp.DataTextField = "nombre"
            cbopagestadoprocfrp.DataValueField = "codigo"
            cbopagestadoprocfrp.DataBind()
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String
        Dim OperacionSQL As String = ""
        If Len(Request("ID")) > 0 Then
            ID = Request("ID")
            OperacionSQL = "EDT"
        Else
            ID = txtNroConsignacion.Text.Trim
            If Len(Request("pExpediente")) > 0 Then
                OperacionSQL = "ADD"
            End If

        End If

        'Validaciones
        If txtNroConsignacion.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el número de consignación por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        If txtNroRadicadoSalida.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el número de radicado de salida por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Opens a connection to the database.
        Connection.Open()
        Dim trans As SqlTransaction
        trans = Connection.BeginTransaction

        Dim Command As SqlCommand

        'En esta ventana no existe INSERT porque quien crea los registros es el abogado / gestor --- NO
        '02/SEP/2014.
        'Ahora si va a haber INSERT
        ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
        ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago.  
        Dim InsertSQL As String = "INSERT INTO pagos (NroConsignacion, NroExp, FecSolverif, FecVerificado, estado, UserVerif, " &
                                    "pagFecha, pagFechaDeudor, pagNroTitJudicial, pagCapital, pagAjusteDec1406, pagInteres, " &
                                    "pagGastosProc, pagExceso, pagTotal, pagestadoprocfrp, pagFecExi, pagTasaIntApl, pagdiasmora, " &
                                    "pagvalcuota, pagNumConPag,vlripc, SnContabilizar" &
                                    " ) VALUES ( @NroConsignacion, @NroExp, @FecSolverif, @FecVerificado, @estado, @UserVerif, " &
                                    "@pagFecha, @pagFechaDeudor, @pagNroTitJudicial, @pagCapital, @pagAjusteDec1406, @pagInteres, " &
                                    "@pagGastosProc, @pagExceso, @pagTotal, @pagestadoprocfrp, @pagFecExi, @pagTasaIntApl, @pagdiasmora, " &
                                    "@pagvalcuota, @pagNumConPag, @vlripc, 1, @NroRadicadoSalida)"

        Dim UpdateSQL As String = "Update PAGOS SET NroExp = @NroExp, FecSolverif = @FecSolverif, FecVerificado = @FecVerificado, estado = @estado, UserVerif = @UserVerif, " &
                                    "pagFecha = @pagFecha, pagFechaDeudor = @pagFechaDeudor, pagNroTitJudicial = @pagNroTitJudicial, " &
                                    "pagCapital = @pagCapital, pagAjusteDec1406 = @pagAjusteDec1406, pagInteres = @pagInteres, " &
                                    "pagGastosProc = @pagGastosProc, pagExceso = @pagExceso, pagTotal = @pagTotal, " &
                                    "pagestadoprocfrp = @pagestadoprocfrp, " &
                                    "pagFecExi = @pagFecExi, " &
                                    "pagTasaIntApl = @pagTasaIntApl, " &
                                    "pagdiasmora = @pagdiasmora, " &
                                    "pagvalcuota = @pagvalcuota, " &
                                    "pagNumConPag = @pagNumConPag, vlripc = @vlripc, pagNroRadicadoSalida = @NroRadicadoSalida " &
                                    "WHERE NroConsignacion = @NroConsignacion "

        If OperacionSQL = "ADD" Then
            Command = New SqlCommand(InsertSQL, Connection, trans)
            Command.Parameters.AddWithValue("@NroConsignacion", txtNroConsignacion.Text.Trim)
        Else
            Command = New SqlCommand(UpdateSQL, Connection, trans)
            Command.Parameters.AddWithValue("@NroConsignacion", ID)
        End If

        'If CDbl(Tpgado()) = 0 Then
        '    CustomValidator1.Text = "No se ha ingresado ningun pago, Por favor verifique."
        '    CustomValidator1.IsValid = False
        '    Exit Sub
        'End If


        'Parametros
        Command.Parameters.AddWithValue("@NroExp", txtNroExp.Text.Trim)

        If IsNumeric(txtNroRadicadoSalida.Text.Trim) Then
            Command.Parameters.AddWithValue("@NroRadicadoSalida", txtNroRadicadoSalida.Text.Trim)
        Else
            CustomValidator1.Text = "El valor del campo de número de radicado de salida debe ser numérico"
            CustomValidator1.IsValid = False
            Return
        End If

        Command.Parameters.AddWithValue("@UserVerif", Session("sscodigousuario"))

        If IsDate(txtFecSolverif.Text.Trim) Then
            Dim parameter As New SqlParameter("@FecSolverif", SqlDbType.DateTime, 10)
            parameter.Value = Date.Parse(txtFecSolverif.Text.Trim)
            parameter.IsNullable = True
            Command.Parameters.Add(parameter)
            ' Command.Parameters.AddWithValue("@FecSolverif", txtFecSolverif.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecSolverif", DBNull.Value)
        End If

        If IsDate(txtFecVerificado.Text.Trim) Then
            Dim parameter As New SqlParameter("@FecVerificado", SqlDbType.DateTime, 10)
            parameter.Value = Date.Parse(txtFecVerificado.Text.Trim)
            parameter.IsNullable = True
            Command.Parameters.Add(parameter)
            ' Command.Parameters.AddWithValue("@FecVerificado", txtFecVerificado.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecVerificado", DBNull.Value)
        End If

        If cboestado.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@estado", cboestado.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@estado", DBNull.Value)
        End If

        If IsDate(Left(txtpagFecha.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@pagFecha", Left(txtpagFecha.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@pagFecha", DBNull.Value)
        End If

        If IsDate(Left(txtpagFechaDeudor.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@pagFechaDeudor", Left(txtpagFechaDeudor.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@pagFechaDeudor", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@pagNroTitJudicial", txtpagNroTitJudicial.Text.Trim)

        Dim capitalPagado = txtpagCapital.Text.Replace(",", "") '.Replace(".", ",")
        If IsNumeric(capitalPagado) Then
            Command.Parameters.AddWithValue("@pagCapital", capitalPagado.Trim)
        Else
            Command.Parameters.AddWithValue("@pagCapital", DBNull.Value)
        End If

        Dim ajusteDecreto1406 = txtpagAjusteDec1406.Text.Replace(",", "") '.Replace(".", ",")
        If IsNumeric(ajusteDecreto1406) Then
            Command.Parameters.AddWithValue("@pagAjusteDec1406", ajusteDecreto1406.Trim)
        Else
            Command.Parameters.AddWithValue("@pagAjusteDec1406", DBNull.Value)
        End If

        Dim interesesPagados = txtpagInteres.Text.Replace(",", "") '.Replace(".", ",")
        If IsNumeric(interesesPagados) Then
            If tipoTitulo = "07" Then
                Command.Parameters.AddWithValue("@vlripc", interesesPagados.Trim)
                Command.Parameters.AddWithValue("@pagInteres", "0")
            Else
                Command.Parameters.AddWithValue("@vlripc", "0")
                Command.Parameters.AddWithValue("@pagInteres", interesesPagados.Trim)
            End If

        Else
            Command.Parameters.AddWithValue("@pagInteres", DBNull.Value)
            Command.Parameters.AddWithValue("@vlripc", DBNull.Value)
        End If

        Dim gastosProceso = txtpagGastosProc.Text.Replace(",", "") '.Replace(".", ",")
        If IsNumeric(gastosProceso) Then
            Command.Parameters.AddWithValue("@pagGastosProc", gastosProceso.Trim)
        Else
            Command.Parameters.AddWithValue("@pagGastosProc", DBNull.Value)
        End If

        Dim pagosExceso = txtpagExceso.Text.Replace(",", "") '.Replace(".", ",")
        If IsNumeric(pagosExceso) Then
            Command.Parameters.AddWithValue("@pagExceso", pagosExceso.Trim)
        Else
            Command.Parameters.AddWithValue("@pagExceso", DBNull.Value)
        End If

        Tpgado()

        Dim totalPagado = txtpagTotal.Text.Replace(",", "") '.Replace(".", ",")
        If IsNumeric(totalPagado) Then
            Command.Parameters.AddWithValue("@pagTotal", totalPagado.Trim)
        Else
            Command.Parameters.AddWithValue("@pagTotal", DBNull.Value)
        End If

        If cbopagestadoprocfrp.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@pagestadoprocfrp", cbopagestadoprocfrp.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@pagestadoprocfrp", DBNull.Value)
        End If

        '---------------------------------------------------------------------------------------------
        'Parametros de la sección de facilidades de pago
        '
        'Campo fecha
        If IsDate(Left(txtpagFecExi.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@pagFecExi", Left(txtpagFecExi.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@pagFecExi", DBNull.Value)
        End If

        'Campo texto
        Command.Parameters.AddWithValue("@pagTasaIntApl", txtpagTasaIntApl.Text.Trim)

        'Campo numerico
        If IsNumeric(txtpagdiasmora.Text) Then
            Command.Parameters.AddWithValue("@pagdiasmora", txtpagdiasmora.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@pagdiasmora", DBNull.Value)
        End If

        'Campo numerico
        Dim valorCuota = txtpagvalcuota.Text.Replace(",", "") '.Replace(".", ",")
        If IsNumeric(valorCuota) Then
            Command.Parameters.AddWithValue("@pagvalcuota", valorCuota.Trim)
        Else
            Command.Parameters.AddWithValue("@pagvalcuota", DBNull.Value)
        End If

        'Campo texto
        Command.Parameters.AddWithValue("@pagNumConPag", txtpagNumConPag.Text.Trim)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog2(Session("ssloginusuario"), "Registro de pagos", "No. Consignación " & ID.Trim, Command, Connection, trans)

            '-------------------------------------------------------------------------------------------------------------------------------------
            '27/oct/2014. Actualizar los campos EFIPAGOSCAP y EFISALDOCAP en la tabla EJEFISGLOBAL        
            Dim efipagoscap As Double = 0
            Dim efisaldocap As Double = 0
            Dim expediente As String = txtNroExp.Text.Trim

            ConsultarPagosSaldos(expediente, efipagoscap, efisaldocap, Connection, trans)

            UpdateSQL = "UPDATE ejefisglobal SET efipagoscap = @efipagoscap, efisaldocap = (efivaldeu - @efipagoscap) WHERE efinroexp = '" & expediente & "'"
            Dim Command2 As SqlCommand
            Command2 = New SqlCommand(UpdateSQL, Connection, trans)

            Command2.Parameters.AddWithValue("@efipagoscap", efipagoscap)
            'Command2.Parameters.AddWithValue("@efisaldocap", efisaldocap)
            Command2.ExecuteNonQuery()

            LogProc.SaveLog2(Session("ssloginusuario"), "Registro de pagos", "No. Consignación " & ID.Trim, Command2, Connection, trans)

            Dim VerificarAcuerdo As Boolean = VerificarAcuerdoVigente(expediente)
            Dim mensajeAcuerdo As String = ""
            If OperacionSQL = "ADD" Then
                If VerificarAcuerdo Then

                    Dim VerificarCuotas As DataTable = VerificarCuotasAcuerdoVigente(expediente)
                    If VerificarCuotas.Rows.Count > 0 Then

                        If CDbl(VerificarCuotas.Rows(0).Item("VALOR_CUOTA")) = CDbl(txtpagTotal.Text.Trim) Then
                            UpdateCuotaFacilidadPago(CDbl(txtpagTotal.Text.Trim), CDate(txtpagFechaDeudor.Text), VerificarCuotas.Rows(0).Item("CUOTA_NUMERO"), CStr(VerificarCuotas.Rows(0).Item("DOCUMENTO")), Connection, trans, LogProc)
                        Else
                            mensajeAcuerdo = "(EXPEDIENTE EN FACILIDAD DE PAGO). El total pagado no coincide con el valor de la cuota..."
                        End If
                    Else
                        mensajeAcuerdo = "El expediente está en facilidad de pago y ha pagado la totalidad de las cuotas."
                    End If
                End If
            End If

            If mensajeAcuerdo <> "" Then
                trans.Rollback()
                CustomValidator1.Text = mensajeAcuerdo
                CustomValidator1.IsValid = False
            Else
                trans.Commit()
                CustomValidator1.Text = "Pago guardado satisfactoriamente..."
                CustomValidator1.IsValid = False


                If tipoTitulo = "07" Then
                    'If Year(CDate(txtpagFecha.Text)) < Year(CDate(Now)) Then
                    Dim ejecutarIpc As New capturarinteresesmulta
                    DatosImportado = New DataTable
                    DatosImportado = ejecutarIpc.LogicaIpc(txtNroExp.Text.Trim, CDbl(txtTotalDeudaEA.Text), True)
                    MostrarEA(txtNroExp.Text.Trim)
                    CustomValidator1.Text = "Pago guardado satisfactoriamente, IPC recalculado Click en  desacargar relacion IPC "
                    CustomValidator1.IsValid = False
                    'End If
                Else
                    MostrarEA(txtNroExp.Text.Trim)
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), Me.ClientID, String.Format("window.parent.location = window.parent.location", "Server"), True)
            End If

            '-------------------------------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            trans.Rollback()
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Return
        Finally
            Connection.Close()
        End Try

        'Try
        '    Command2.ExecuteNonQuery()
        'Catch ex As Exception
        '    Dim msg As String = ex.Message

        'End Try
        'Connection.Close()

        'Response.Redirect("PAGOS.aspx?pExpediente=" & txtNroExp.Text.Trim)
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'Response.Redirect("PAGOS.aspx")
        'Response.Redirect("PAGOS.aspx?pExpediente=" & txtNroExp.Text.Trim)
        Response.Redirect("PAGOSEXPEDIENTE.aspx?pExpediente=" & txtNroExp.Text.Trim)
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()

        Response.Redirect("../../login.aspx")
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        Session.RemoveAll()
    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("PAGOS.aspx?pExpediente=" & txtNroExp.Text.Trim)
    End Sub

    Protected Sub AHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AHome.Click
        Response.Redirect("EJEFISGLOBAL.aspx")
    End Sub

    Protected Sub cmdSolicitudCambioEstado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSolicitudCambioEstado.Click
        'Reenviar  la pagina de solicitudes de cambio de estado con un marco para navegacion
        Response.Redirect("EditPagCambioEstado.aspx?pExpediente=" & txtNroExpEnc.Text.Trim & "&ID=" & txtNroConsignacion.Text.Trim)
    End Sub

    Private Sub ConsultarPagosSaldos(ByVal pExpediente As String, ByRef efipagoscap As Double, ByRef efisaldocap As Double, ByVal Connection As SqlConnection, ByVal trans As SqlTransaction)
        If pExpediente <> "" Then
            Dim TotalDeuda As Double = 0
            Dim TotalPagos As Double = 0
            Dim Totalipc As Double = 0

            'Conexion a la base de datos
            ' Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            'Connection.Open()

            'Consultar el total de la deuda
            Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " &
                                          "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pExpediente.Trim & "' GROUP BY MT_expediente"

            'Dim Command As New SqlCommand(sql)
            Dim Command As New SqlCommand(sql, Connection, trans)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                TotalDeuda = Convert.ToDouble(Reader("totaldeuda").ToString())
            Else
                TotalDeuda = 0
            End If
            Reader.Close()

            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor).
            Dim sql2 As String = "SELECT SUM(pagcapital) AS pagcapital,sum(vlripc) AS vlripc FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' GROUP BY NroExp"
            Dim Command2 As New SqlCommand(sql2, Connection, trans)
            Dim Reader2 As SqlDataReader = Command2.ExecuteReader
            If Reader2.Read Then
                If Reader2("pagcapital").ToString() = "" Then
                    TotalPagos = 0
                Else
                    TotalPagos = Convert.ToDouble(Reader2("pagcapital").ToString())
                End If


                If Reader2("vlripc").ToString() = "" Then
                    Totalipc = "0"
                Else
                    Totalipc = Convert.ToDouble(Reader2("vlripc").ToString()).ToString("N0")
                End If

            Else
                TotalPagos = 0
                Totalipc = 0
            End If
            Reader2.Close()

            Dim saldoEA As Double
            If tipoTitulo = "07" Then
                saldoEA = TotalDeuda - TotalPagos + Totalipc
            Else
                saldoEA = TotalDeuda - TotalPagos
            End If


            'Mostrar la diferencia entre Total deuda - Capital pagado
            'Connection.Close()

            efipagoscap = TotalPagos
            efisaldocap = saldoEA
        End If
    End Sub

    Private Function VerificarAcuerdoVigente(ByVal expediente As String) As Boolean
        Dim sw As Boolean = False
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT * FROM MAESTRO_ACUERDOS WHERE EXPEDIENTE = '" & expediente & "'")

        If tb.Rows.Count > 0 Then
            sw = True
        End If

        Return sw
    End Function

    Private Function VerificarCuotasAcuerdoVigente(ByVal expediente As String) As DataTable
        Dim sw As Boolean = False
        Dim tb As DataTable = Funciones.RetornaCargadatos("select TOP 1  B.* from MAESTRO_ACUERDOS A, DETALLES_ACUERDO_PAGO B WHERE A.DOCUMENTO = B.DOCUMENTO AND A.EXPEDIENTE = '" & expediente & "' AND B.VALOR_CUOTA > B.VALOR_PAGADO ORDER BY B.CUOTA_NUMERO ")

        Return tb
    End Function

    Private Sub UpdateCuotaFacilidadPago(ByVal pValorPagado As Double, ByVal pFechaPago As Date, ByVal pNumeroCuota As Integer, ByVal pNumeroAcuerdo As String, ByVal Connection As SqlConnection, ByVal trans As SqlTransaction, ByVal LogProc As LogProcesos)

        Dim UpdateSQL As String = "UPDATE DETALLES_ACUERDO_PAGO SET FECHA_PAGO = @FECHA_PAGO, VALOR_PAGADO = @VALOR_PAGADO WHERE CUOTA_NUMERO = @CUOTA_NUMERO AND DOCUMENTO = @DOCUMENTO "
        Dim Command As SqlCommand
        Command = New SqlCommand(UpdateSQL, Connection, trans)

        Command.Parameters.AddWithValue("@FECHA_PAGO", pFechaPago)
        Command.Parameters.AddWithValue("@VALOR_PAGADO", pValorPagado)
        Command.Parameters.AddWithValue("@CUOTA_NUMERO", pNumeroCuota)
        Command.Parameters.AddWithValue("@DOCUMENTO", pNumeroAcuerdo)


        Command.ExecuteNonQuery()
        LogProc.SaveLog2(Session("ssloginusuario"), "Registro de pagos", "No. Consignación " & txtNroConsignacion.Text.Trim, Command, Connection, trans)


    End Sub
    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Tpgado()
    End Sub

    Private Function Tpgado() As Decimal
        Dim total As Decimal = 0
        If tipoTitulo = "07" Then
            txtpagInteres.Text = 0
        End If

        'Se eliminan las comas de los separadoes de miles y se pone coma como separador decimal para que se realicen los calculos de forma correcta en el vb
        txtpagCapital.Text = txtpagCapital.Text.Replace(",", "").Replace(".", ",")
        txtpagAjusteDec1406.Text = txtpagAjusteDec1406.Text.Replace(",", "").Replace(".", ",")
        txtpagInteres.Text = txtpagInteres.Text.Replace(",", "").Replace(".", ",")

        If Not IsNumeric(txtpagCapital.Text.Trim) Then
            CustomValidator1.Text = "Capital pagado no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
            Exit Function
        ElseIf Not IsNumeric(txtpagAjusteDec1406.Text.Trim) Then
            CustomValidator1.Text = " Ajuste Decreto 1406 no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
            Exit Function
        ElseIf Not IsNumeric(txtpagInteres.Text.Trim) Then
            CustomValidator1.Text = "Intereses pagados no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
            Exit Function
        ElseIf Not IsDate(txtpagFecha.Text) Then
            CustomValidator1.Text = "Por digite una fecha de pago valida, VERIFIQUE."
            CustomValidator1.IsValid = False
            Exit Function
        Else

            If tipoTitulo = "07" Then
                txtpagInteres.Text = 0
                total = CDec(txtpagCapital.Text.Trim) + CDec(txtpagAjusteDec1406.Text.Trim)
            Else
                total = CDec(txtpagCapital.Text.Trim) + CDec(txtpagAjusteDec1406.Text.Trim) + CDec(txtpagInteres.Text.Trim)
            End If

        End If

        ''Calcular IPC
        If tipoTitulo = "07" Then
            btnExp.Enabled = True
            Dim CalcularIpc As New CalcularIPCinduvidual
            Dim _DatosRetorno As New CalcularIPCinduvidual._DatosRetorno
            Dim saldo As Decimal = 0

            saldo = CDec(txtCapitalInicial.Text) - CDec(txtValorRevocatoria.Text) - CDec(txtPagosCapitalEA.Text) + RetornarIpc(txtpagFecha.Text, Request("pExpediente"))

            'If saldo > 0 And (CDate(txtpagFecha.Text) >= CDate(Now.ToString("dd/MM/yyyy"))) Then
            If saldo > 0 And (Year(CDate(txtpagFecha.Text)) > Year(CDate(txtFechaExigTitulo.Text))) Then
                _DatosRetorno = CalcularIpc.CalculoIpc(Request("pExpediente"), txtpagFecha.Text, saldo, Session("sscodigousuario"))
            Else
                _DatosRetorno.VALORIPC = 0
                _DatosRetorno.CALCULO = Nothing
            End If
            If _DatosRetorno._Error = "" Then
                txtpagInteres.Text = _DatosRetorno.VALORIPC
            Else
                CustomValidator1.Text = _DatosRetorno._Error
                CustomValidator1.IsValid = False
            End If
            DatosImportado = _DatosRetorno.CALCULO
        End If

        ' Se remplazan las commas por puntos para correcto funcionamiento con la librería number js
        txtpagCapital.Text = txtpagCapital.Text.Replace(",", ".")
        txtpagAjusteDec1406.Text = txtpagAjusteDec1406.Text.Replace(",", ".")
        txtpagInteres.Text = txtpagInteres.Text.Replace(",", ".")
        txtpagTotal.Text = total
        txtpagTotal.Text = txtpagTotal.Text.Replace(",", ".")

        Return total
    End Function

    Private Sub interesesIPC(ByVal pExpediente As String)
        If tipoTitulo = "07" Then
            intIPC.InnerText = "Valor IPC"
            txtpagInteres.ReadOnly = True
        Else
            intIPC.InnerText = "Intereses pagados"
            txtpagInteres.ReadOnly = False
        End If

    End Sub

    Private Function Tipo_Titulo(ByVal pExpediente As String) As String
        Dim salida As String = ""
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT MT_TIPO_TITULO FROM MAESTRO_TITULOS WHERE mt_EXPEDIENTE = '" & pExpediente & "'")
        If tb.Rows.Count > 0 Then
            salida = tb.Rows(0).Item("MT_TIPO_TITULO").ToString
        End If
        Return salida
    End Function

    Private Sub descargafichero(ByVal dtTemp As DataTable, ByVal fname As String, ByVal forceDownload As Boolean, Optional ByVal plantilla As String = "")

        Dim ext As String = Path.GetExtension(fname)
        Dim type As String = ""
        ' mirar las extensiones conocidas	
        If ext IsNot Nothing Then
            Select Case ext.ToLower()
                Case ".htm", ".html"
                    type = "text/HTML"
                    Exit Select

                Case ".txt"
                    type = "text/plain"
                    Exit Select

                Case ".doc", ".rtf"
                    type = "Application/msword"
                    Exit Select

                Case ".xls"
                    type = "Application/vnd.ms-excel"
                    Exit Select

            End Select
        End If


        Response.Clear()
        Response.Buffer = True

        If forceDownload Then
            Response.AppendHeader("content-disposition", "attachment; filename=" & fname)
        End If

        If type <> "" Then
            Response.ContentType = type
        End If

        If plantilla <> "" Then
            Response.Write(plantilla)
        Else
            Dim sb As StringBuilder = New StringBuilder()
            Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
            Dim dg As New DataGrid()

            dg.DataSource = dtTemp
            dg.DataBind()

            dg.RenderControl(htw)
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(SW)
        End If



        Response.End()

    End Sub

    Protected Sub btnExp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExp.Click
        If DatosImportado.Rows.Count > 0 Then
            descargafichero(DatosImportado, Now.ToString("ddMMyyyyhhssmm") & Request("pExpediente") & ".xls", True)
        Else
            CustomValidator1.Text = "No hay datos a exportar"
            CustomValidator1.IsValid = False
        End If

    End Sub

    Protected Sub imgBtnBorra_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorra.Click
        txtpagInteres.Text = 0
        Tpgado()
    End Sub

    Private Sub MostrarSancion(ByVal mostar As Boolean)
        trsancion.Visible = mostar
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
            'Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda,ISNULL(sum(mt_totalSancion),0) as totalsancion,ISNULL(sum(valorRevoca),0) valorRevoca " &
            '                              "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pNumExpediente.Trim & "' GROUP BY MT_expediente"

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
                txtTotalDeudaEA.Text = Convert.ToDecimal(Reader("totaldeuda").ToString()).ToString("N0")
                txtCapitalInicial.Text = Convert.ToDecimal(Reader("capitalInicial").ToString()).ToString("NO")
                txtTotalDeudaSancion.Text = Convert.ToDecimal(Reader("totalsancion").ToString()).ToString("N0")
                txtValorRevocatoria.Text = Convert.ToDecimal(Reader("valorRevoca").ToString()).ToString("N0")
            Else
                txtTotalDeudaEA.Text = "0"
                txtCapitalInicial.Text = "0"
                txtTotalDeudaSancion.Text = "0"
                txtValorRevocatoria.Text = "0"
            End If
            Reader.Close()

            'Pagos de la liquidacion oficial
            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital, SUM(COALESCE(paginteres,0)) pagoInteres " &
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' and pagLiqSan = 0 GROUP BY NroExp"

            Dim Command2 As New SqlCommand(sql2, Connection)
            Dim Reader2 As SqlDataReader = Command2.ExecuteReader
            If Reader2.Read Then
                If Reader2("pagcapital").ToString() = "" Then
                    txtPagosCapitalEA.Text = "0"
                Else
                    txtPagosCapitalEA.Text = Convert.ToDecimal(Reader2("pagcapital").ToString()).ToString("N0")
                End If

                If Reader2("pagoInteres").ToString() = "" Then
                    txtpagInteresLiq.Text = "0"
                Else
                    txtpagInteresLiq.Text = Convert.ToDecimal(Reader2("pagoInteres").ToString()).ToString("N0")
                End If

                txttotalpagLiq.Text = Convert.ToDecimal(CDec(txtPagosCapitalEA.Text) + CDec(txtpagInteresLiq.Text)).ToString("N0")

            Else
                txtpagInteresLiq.Text = "0"
                txtPagosCapitalEA.Text = "0"
                txttotalpagLiq.Text = "0"
            End If
            Reader2.Close()

            'Estado actual de la deuda
            'txtEstadoActualDeuda.Text = DeterminarEstadoActualDeuda(txtPagosCapitalEA.Text, txtCapitalInicial.Text)

            'Pagos de la Sancion
            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            Dim sql3 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital, sum(vlripc) AS vlripc,MAX(pagFecha) pagFecha " &
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' and pagLiqSan = 1 GROUP BY NroExp"

            Dim Command3 As New SqlCommand(sql3, Connection)
            Dim Reader3 As SqlDataReader = Command3.ExecuteReader
            If Reader3.Read Then
                txtUltPag.Text = Left(Reader3("pagFecha").ToString(), 10)
                If Reader3("pagcapital").ToString() = "" Then
                    txtPagosCapitalSancion.Text = "0"
                Else
                    txtPagosCapitalSancion.Text = Convert.ToDecimal(Reader3("pagcapital").ToString()).ToString("N0")
                End If

                If Reader3("vlripc").ToString() = "" Then
                    ipc = "0"
                Else
                    ipc = Convert.ToDecimal(Reader3("vlripc").ToString())
                End If
            Else
                txtPagosCapitalSancion.Text = "0"
                ipc = "0"
                txtUltPag.Text = "Sin pagos"
            End If
            Reader3.Close()

            txtValorIPC.Text = CDec(CDec(Ipcsinpagos) + CDec(ipc)).ToString("N0")
            'Mostrar la diferencia entre Total deuda - Capital pagado

            txtsandoactualSancion.Text = Convert.ToDecimal(txtTotalDeudaSancion.Text) - Convert.ToDecimal(txtPagosCapitalSancion.Text) + Convert.ToDecimal(txtValorIPC.Text)
            txtSaldoEA.Text = Convert.ToDecimal(txtTotalDeudaEA.Text) - Convert.ToDecimal(txtValorRevocatoria.Text) - Convert.ToDecimal(txtPagosCapitalEA.Text)

            txtSaldoEA.Text = CDec(txtSaldoEA.Text).ToString("N0")
            txtsandoactualSancion.Text = CDec(txtsandoactualSancion.Text).ToString("N0")

            Connection.Close()
        End If

    End Sub

    Private Function ObtenerValorIPC(ByVal pExpediente As String) As String
        Dim ValorIPC As String = String.Empty
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT EFIIPC FROM EJEFISGLOBAL WHERE EFINROEXP  = '" & pExpediente & "'")
        If tb.Rows.Count > 0 Then
            ValorIPC = IIf(IsDBNull(tb.Rows(0).Item("EFIIPC")), 0, tb.Rows(0).Item("EFIIPC"))
        End If
        Return ValorIPC
    End Function

    Private Function ExistePago(ByRef pNroTitulo As String) As Boolean
        Dim Anio As Int16 = Year(Date.Now())
        Dim blnExistePago As Boolean = False
        Dim Sql As String = String.Empty

        Sql = " SELECT YEAR(MAX(pagFecha)) ANIO FROM PAGOS P WHERE P.NroExp = @NroExp "
        Using Cn As New SqlConnection(Funciones.CadenaConexion)
            Cn.Open()
            Using Cmd As New SqlCommand(Sql, Cn)
                Cmd.Parameters.AddWithValue("@NroExp", pNroTitulo)
                Dim reader As SqlDataReader = Cmd.ExecuteReader
                While (reader.Read())
                    blnExistePago = IIf(Not IsDBNull(reader("ANIO")) AndAlso reader("ANIO") = Anio, True, False)
                End While
            End Using
        End Using
        Return blnExistePago
    End Function

    Private Function RetornarIpc(ByVal strFecha As String, ByVal strNroExp As String) As Decimal
        Dim decSaldoIpc As Decimal = 0
        Dim sql As String = String.Empty

        sql = " SELECT SUM(vlripc) vlripc FROM PAGOS WHERE pagFecha < @fecha AND NroExp = @nroExp"

        Using Cn As New SqlConnection(Funciones.CadenaConexion)
            Cn.Open()
            Dim cmd As SqlCommand = New SqlCommand(sql, Cn)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("@fecha", strFecha)
            cmd.Parameters.AddWithValue("@nroExp", strNroExp)
            Using Reader = cmd.ExecuteReader()
                If Reader.Read() Then
                    decSaldoIpc = IIf(Not IsDBNull(Reader("vlripc")), Reader("vlripc"), 0)
                End If
            End Using
            Cn.Close()
        End Using
        Return decSaldoIpc
    End Function

End Class