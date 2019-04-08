Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class EditPAGOSLiquidacionSancion
    Inherits System.Web.UI.Page

    Protected Shared tipoTitulo As Boolean = False
    Protected Shared DatosImportado As New DataTable

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        finsession()
        'Evaluates to true when the page is loaded for the first time.
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
                Dim sql As String = "select * from PAGOS where NroConsignacion = @NroConsignacion "
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @NroConsignacion parameter in the Command select query
                Command.Parameters.AddWithValue("@NroConsignacion", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    Try
                        If CBool(Reader("pagLiqSan")) Then
                            tipoTitulo = True
                            txtNroConsignacionSancion.Text = Reader("NroConsignacion").ToString()
                            txtpagFechaSancion.Text = Left(Reader("pagFecha").ToString().Trim, 10)
                            txtpagFechaDeudorSancion.Text = Left(Reader("pagFechaDeudor").ToString().Trim, 10)
                            txtpagCapitalSancion.Text = Reader("pagCapital").ToString()
                            txtIpcSancion.Text = Reader("vlripc").ToString()
                            txtpagExcesoSancion.Text = Reader("pagExceso").ToString()
                            txtPagTotalSancion.Text = Reader("pagTotal").ToString()
                        Else
                            txtNroConsignacion.Text = Reader("NroConsignacion").ToString()
                            txtpagFecha.Text = Left(Reader("pagFecha").ToString().Trim, 10)
                            txtpagFechaDeudor.Text = Left(Reader("pagFechaDeudor").ToString().Trim, 10)
                            txtpagCapital.Text = Reader("pagCapital").ToString()
                            txtpagInteres.Text = Reader("pagInteres").ToString()
                            txtpagExceso.Text = Reader("pagExceso").ToString()
                            txtpagTotal.Text = Reader("pagTotal").ToString()
                        End If
                    Catch ex As Exception

                        txtNroConsignacion.Text = Reader("NroConsignacion").ToString()
                        txtpagFecha.Text = Left(Reader("pagFecha").ToString().Trim, 10)
                        txtpagFechaDeudor.Text = Left(Reader("pagFechaDeudor").ToString().Trim, 10)
                        txtpagCapital.Text = Reader("pagCapital").ToString()
                        txtpagInteres.Text = Reader("pagInteres").ToString()
                        txtpagExceso.Text = Reader("pagExceso").ToString()
                        txtpagTotal.Text = Reader("pagTotal").ToString()

                    End Try

                    txtNroExp.Text = Reader("NroExp").ToString().Trim
                    txtFecSolverif.Text = Left(Reader("FecSolverif").ToString().Trim, 10)
                    txtFecVerificado.Text = Left(Reader("FecVerificado").ToString().Trim, 10)
                    cboestado.SelectedValue = Reader("estado").ToString()
                    txtpagNroTitJudicial.Text = Reader("pagNroTitJudicial").ToString()
                    txtpagAjusteDec1406.Text = Reader("pagAjusteDec1406").ToString()
                    txtpagGastosProc.Text = Reader("pagGastosProc").ToString()
                    cbopagestadoprocfrp.SelectedValue = Reader("pagestadoprocfrp").ToString()

                    'Datos de facilidades / acuerdos de pago
                    txtpagFecExi.Text = Left(Reader("pagFecExi").ToString().Trim, 10)
                    txtpagTasaIntApl.Text = Reader("pagTasaIntApl").ToString()
                    txtpagdiasmora.Text = Reader("pagdiasmora").ToString()
                    txtpagvalcuota.Text = Reader("pagvalcuota").ToString()
                    txtpagNumConPag.Text = Reader("pagNumConPag").ToString()

                    '23/JUL/2014. Mostrar datos del encabezado
                    Dim NumExp As String = Reader("NroExp").ToString().Trim
                    If NumExp <> "" Then
                        'interesesIPC(NumExp)

                        MostrarEncabezado(NumExp)
                        MostrarEA(NumExp)
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
                        'interesesIPC(NumExp)
                        MostrarEncabezado(NumExp)
                        MostrarEA(NumExp)
                        MostrarGestorResp(NumExp)
                        MostrarTitulo(NumExp)
                        MostrarDeudor(NumExp)
                        '
                        txtNroExp.Text = NumExp
                    End If
                End If

            End If
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)

            If Session("mnivelacces") <> 6 Then
                cmdSave.Visible = False
                'Desactivar controles
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

            If Len(Request("tab")) > 0 Then
                encabezado.Visible = False
                infoexpediente.Visible = False
            End If
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
                txtValorIPC.Text = "0"
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
            'Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda,ISNULL(sum(mt_totalSancion),0) as totalsancion,ISNULL(sum(valorRevoca),0) valorRevoca " & _
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
                txtTotalDeudaSancion.Text = Convert.ToDecimal(Reader("totalsancion").ToString()).ToString("N0")
                txtValorRevocatoria.Text = Convert.ToDecimal(Reader("valorRevoca").ToString()).ToString("N0")
                txtCapitalInicial.Text = Convert.ToDecimal(Reader("capitalInicial").ToString()).ToString("N0")
            Else
                txtValorRevocatoria.Text = "0"
                txtTotalDeudaEA.Text = "0"
                txtTotalDeudaSancion.Text = "0"
                txtCapitalInicial.Text = "0"
            End If
            Reader.Close()

            'Pagos de la liquidacion oficial
            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
            ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. 
            Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital, SUM(COALESCE(paginteres,0)) pagoInteres " &
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' and pagLiqSan = 0 AND SnContabilizar = 1 GROUP BY NroExp"

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
                    txtPagosCapitalSancion.Text = Convert.ToDecimal(Reader3("pagcapital").ToString()).ToString("N0")
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

            txtsandoactualSancion.Text = CDec(txtTotalDeudaSancion.Text) - CDec(txtPagosCapitalSancion.Text) + CDec(txtValorIPC.Text)
            txtSaldoEA.Text = CDec(txtTotalDeudaEA.Text) - CDec(txtValorRevocatoria.Text) - CDec(txtPagosCapitalEA.Text) - CDec(txtPagosCapitalSancion.Text) + CDec(txtValorIPC.Text)

            txtSaldoEA.Text = CDec(txtSaldoEA.Text).ToString("N0")
            txtsandoactualSancion.Text = CDec(txtsandoactualSancion.Text).ToString("N0")

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
            Dim sql As String = "SELECT MT.MT_nro_titulo, MT.MT_fec_expedicion_titulo, MT.MT_tipo_titulo, TT.nombre AS NomTipoTitulo,MT_fec_exi_liq,MT_fecha_ejecutoria " &
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

            Else
                'txtTotalDeudaEA.Text = "0"

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
        ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
        ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. Se agrega el 1, al ingresar el pago. 
        Dim InsertSQL As String = "INSERT INTO pagos (NroConsignacion, NroExp, FecSolverif, FecVerificado, estado, UserVerif, " &
                                    "pagFecha, pagFechaDeudor, pagNroTitJudicial, pagCapital, pagAjusteDec1406, pagInteres, " &
                                    "pagGastosProc, pagExceso, pagTotal, pagestadoprocfrp, pagFecExi, pagTasaIntApl, pagdiasmora, " &
                                    "pagvalcuota, pagNumConPag,vlripc, pagLiqSan" &
                                    " ) VALUES ( @NroConsignacion, @NroExp, @FecSolverif, @FecVerificado, @estado, @UserVerif, " &
                                    "@pagFecha, @pagFechaDeudor, @pagNroTitJudicial, @pagCapital, @pagAjusteDec1406, @pagInteres, " &
                                    "@pagGastosProc, @pagExceso, @pagTotal, @pagestadoprocfrp, @pagFecExi, @pagTasaIntApl, @pagdiasmora, " &
                                    "@pagvalcuota, @pagNumConPag,@vlripc,0,1)"

        Dim UpdateSQL As String = "Update PAGOS SET NroExp = @NroExp, FecSolverif = @FecSolverif, FecVerificado = @FecVerificado, estado = @estado, UserVerif = @UserVerif, " &
                                    "pagFecha = @pagFecha, pagFechaDeudor = @pagFechaDeudor, pagNroTitJudicial = @pagNroTitJudicial, " &
                                    "pagCapital = @pagCapital, pagAjusteDec1406 = @pagAjusteDec1406, pagInteres = @pagInteres, " &
                                    "pagGastosProc = @pagGastosProc, pagExceso = @pagExceso, pagTotal = @pagTotal, " &
                                    "pagestadoprocfrp = @pagestadoprocfrp, " &
                                    "pagFecExi = @pagFecExi, " &
                                    "pagTasaIntApl = @pagTasaIntApl, " &
                                    "pagdiasmora = @pagdiasmora, " &
                                    "pagvalcuota = @pagvalcuota, " &
                                    "pagNumConPag = @pagNumConPag, vlripc = @vlripc " &
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
        Command.Parameters.AddWithValue("@UserVerif", Session("sscodigousuario"))

        If IsDate(txtFecSolverif.Text.Trim) Then
            Command.Parameters.AddWithValue("@FecSolverif", txtFecSolverif.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecSolverif", DBNull.Value)
        End If

        If IsDate(txtFecVerificado.Text.Trim) Then
            Command.Parameters.AddWithValue("@FecVerificado", txtFecVerificado.Text.Trim)
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

        If IsNumeric(txtpagCapital.Text) Then
            Command.Parameters.AddWithValue("@pagCapital", txtpagCapital.Text)
        Else
            Command.Parameters.AddWithValue("@pagCapital", DBNull.Value)
        End If

        If IsNumeric(txtpagAjusteDec1406.Text) Then
            Command.Parameters.AddWithValue("@pagAjusteDec1406", txtpagAjusteDec1406.Text)
        Else
            Command.Parameters.AddWithValue("@pagAjusteDec1406", DBNull.Value)
        End If

        If IsNumeric(txtpagInteres.Text) Then
            Command.Parameters.AddWithValue("@pagInteres", txtpagInteres.Text)
        Else
            Command.Parameters.AddWithValue("@pagInteres", DBNull.Value)
        End If

        If IsNumeric(txtpagGastosProc.Text) Then
            Command.Parameters.AddWithValue("@pagGastosProc", txtpagGastosProc.Text)
        Else
            Command.Parameters.AddWithValue("@pagGastosProc", DBNull.Value)
        End If

        If IsNumeric(txtpagExceso.Text) Then
            Command.Parameters.AddWithValue("@pagExceso", txtpagExceso.Text)
        Else
            Command.Parameters.AddWithValue("@pagExceso", DBNull.Value)
        End If

        If IsNumeric(txtpagTotal.Text) Then
            Command.Parameters.AddWithValue("@pagTotal", txtpagTotal.Text)
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
        If IsNumeric(txtpagvalcuota.Text) Then
            Command.Parameters.AddWithValue("@pagvalcuota", txtpagvalcuota.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@pagvalcuota", DBNull.Value)
        End If

        'Campo texto
        Command.Parameters.AddWithValue("@pagNumConPag", txtpagNumConPag.Text.Trim)

        '/-----------------------------------------------------------------  
        'ID _HU:  015
        'Nombre HU : Error en Cálculo del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 31-05-2017  
        'Objetivo : Se agrega el parámetro para grabar el valor del IPC
        ' sólo para la parte de sanción del título 
        '------------------------------------------------------------------/

        If IsNumeric(txtIpcSancion.Text) Then
            Command.Parameters.AddWithValue("@vlripc", txtIpcSancion.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@vlripc", DBNull.Value)
        End If

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

            UpdateSQL = "UPDATE ejefisglobal SET efipagoscap = @efipagoscap, efisaldocap =(efivaldeu - @efipagoscap) WHERE efinroexp = '" & expediente & "'"
            Dim Command2 As SqlCommand
            Command2 = New SqlCommand(UpdateSQL, Connection, trans)

            Command2.Parameters.AddWithValue("@efipagoscap", efipagoscap)
            '    Command2.Parameters.AddWithValue("@efisaldocap", efisaldocap)
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
                MostrarEA(txtNroExp.Text.Trim)
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

    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'Response.Redirect("PAGOS.aspx")
        If Len(Request("tab")) > 0 Then
            Response.Redirect("PAGOSEXPEDIENTE.aspx?pExpediente=" & txtNroExp.Text.Trim)
        Else
            Response.Redirect("PAGOS.aspx?pExpediente=" & txtNroExp.Text.Trim)
        End If

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
        Response.Redirect("EditPagCambioEstado.aspx?pExpediente=" & txtNroExpEnc.Text.Trim & "&ID=" & HttpUtility.HtmlAttributeEncode(txtNroConsignacion.Text.Trim.Replace("+", "%2B")))
    End Sub

    Private Sub ConsultarPagosSaldos(ByVal pExpediente As String, ByRef efipagoscap As Double, ByRef efisaldocap As Double, ByVal Connection As SqlConnection, ByVal trans As SqlTransaction)
        If pExpediente <> "" Then
            Dim TotalDeuda As Double = 0
            Dim TotalPagos As Double = 0
            Dim Totalipc As Double = 0
            Dim ValorRevoca As Double = 0

            '/-----------------------------------------------------------------  
            'ID _HU:  015
            'Nombre HU : Error en Cálculo del IPC
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 31-05-2017  
            'Objetivo : Se agrega la variable ValorRevoca.
            '------------------------------------------------------------------/


            'Conexion a la base de datos
            ' Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            'Connection.Open()

            'Consultar el total de la deuda
            Dim sql As String = " SELECT SUM(totaldeuda) AS totaldeuda, ISNULL(SUM(valorRevoca), 0) AS valorRevoca " &
                                " FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pExpediente.Trim & "' GROUP BY MT_expediente"

            'Dim Command As New SqlCommand(sql)
            Dim Command As New SqlCommand(sql, Connection, trans)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                TotalDeuda = Convert.ToDouble(Reader("totaldeuda").ToString())
                ValorRevoca = Convert.ToDouble(Reader("valorRevoca").ToString())
            Else
                TotalDeuda = 0
            End If
            Reader.Close()

            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor).
            ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
            ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. 
            Dim sql2 As String = "SELECT SUM(pagcapital) AS pagcapital,sum(vlripc) AS vlripc FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' AND SnContabilizar = 1 GROUP BY NroExp"
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

            '/-----------------------------------------------------------------  
            'ID _HU:  015
            'Nombre HU : Error en Cálculo del IPC
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 30-05-2017  
            'Objetivo : Se realiza el calculo del saldo.
            ' SaldoEA = Capital Inicial + Capital Inicial Sanción - Valor Revocatoria 
            '           - Saldo Actual Sanción - Total Pagados. 
            '------------------------------------------------------------------/


            Dim saldoEA As Double
            If tipoTitulo = "07" Then
                saldoEA = TotalDeuda - TotalPagos + Totalipc
            ElseIf txtTIPOTITULO.Text.Equals("LIQUIDACIÓN OFICIAL / SANCIÓN") Then
                saldoEA = TotalDeuda - TotalPagos - ValorRevoca + Totalipc
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

    Private Function Tpgado() As Double
        Dim total As Double = 0
        If tipoTitulo = "07" Then
            txtpagInteres.Text = 0
        End If


        '/-----------------------------------------------------------------  
        'ID _HU:  015
        'Nombre HU : Error en Cálculo del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 30-05-2017  
        'Objetivo : Se valida que el campo txtpagInteres.Text tenga un valor
        ' sino se le asinga 0. Para poder proseguir con el calculo del IPC.
        '------------------------------------------------------------------/

        If String.IsNullOrWhiteSpace(txtpagInteres.Text) Then
            txtpagInteres.Text = 0
        End If

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
            total = CDbl(txtpagCapital.Text.Trim) + CDbl(txtpagAjusteDec1406.Text.Trim) + CDbl(txtpagInteres.Text.Trim)
        End If

        '/-----------------------------------------------------------------  
        'ID _HU:  015
        'Nombre HU : Error en Cálculo del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 05-07-2017  
        'Objetivo : Realizar el cálculo del IPC, para la parte de 
        ' liquidación  del títlo
        '------------------------------------------------------------------/
        If txtTIPOTITULO.Text.Trim.Equals("LIQUIDACIÓN OFICIAL / SANCIÓN") Then
            Dim CalcularIpc As New CalcularIPCinduvidual
            Dim _DatosRetorno As New CalcularIPCinduvidual._DatosRetorno
            Dim saldo As String = CDbl(txtCapitalInicial.Text)

            If saldo > 0 And (Year(CDate(txtpagFecha.Text)) > Year(CDate(txtFechaExigTitulo.Text))) Then
                _DatosRetorno = CalcularIpc.CalculoIpc(Request("pExpediente"), txtpagFecha.Text, saldo, Session("sscodigousuario"))
            Else
                _DatosRetorno.VALORIPC = 0
                _DatosRetorno.CALCULO = Nothing
            End If
            If _DatosRetorno._Error = "" Then
                txtIpcSancion.Text = _DatosRetorno.VALORIPC
            Else
                CustomValidator1.Text = _DatosRetorno._Error
                CustomValidator1.IsValid = False
            End If
            DatosImportado = _DatosRetorno.CALCULO
        End If

        txtpagTotal.Text = total
        Return total
    End Function

    'Private Sub interesesIPC(ByVal pExpediente As String)
    '    If tipoTitulo = "07" Then
    '        intIPC.InnerText = "Valor IPC"
    '        txtpagInteres.ReadOnly = True
    '    Else
    '        intIPC.InnerText = "Intereses pagados"
    '        txtpagInteres.ReadOnly = False
    '    End If

    'End Sub

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

    Protected Sub cmdSaveSancion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveSancion.Click
        Dim ID As String
        Dim OperacionSQL As String = ""
        If Len(Request("ID")) > 0 Then
            ID = Request("ID")
            OperacionSQL = "EDT"
        Else
            ID = txtNroConsignacionSancion.Text.Trim
            If Len(Request("pExpediente")) > 0 Then
                OperacionSQL = "ADD"
            End If

        End If


        'Validaciones
        If txtNroConsignacionSancion.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el número de consignación para el pago de la sanción por favor"
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
        Dim InsertSQL As String = "INSERT INTO pagos (NroConsignacion, NroExp, FecSolverif, FecVerificado, estado, UserVerif, " &
                                    "pagFecha, pagFechaDeudor, pagNroTitJudicial, pagCapital, pagAjusteDec1406, pagInteres, " &
                                    "pagGastosProc, pagExceso, pagTotal, pagestadoprocfrp, pagFecExi, pagTasaIntApl, pagdiasmora, " &
                                    "pagvalcuota, pagNumConPag,vlripc, pagLiqSan" &
                                    " ) VALUES ( @NroConsignacion, @NroExp, @FecSolverif, @FecVerificado, @estado, @UserVerif, " &
                                    "@pagFecha, @pagFechaDeudor, @pagNroTitJudicial, @pagCapital, @pagAjusteDec1406, @pagInteres, " &
                                    "@pagGastosProc, @pagExceso, @pagTotal, @pagestadoprocfrp, @pagFecExi, @pagTasaIntApl, @pagdiasmora, " &
                                    "@pagvalcuota, @pagNumConPag,@vlripc,1)"

        Dim UpdateSQL As String = "Update PAGOS SET NroExp = @NroExp, FecSolverif = @FecSolverif, FecVerificado = @FecVerificado, estado = @estado, UserVerif = @UserVerif, " &
                                    "pagFecha = @pagFecha, pagFechaDeudor = @pagFechaDeudor, pagNroTitJudicial = @pagNroTitJudicial, " &
                                    "pagCapital = @pagCapital, pagAjusteDec1406 = @pagAjusteDec1406, pagInteres = @pagInteres, " &
                                    "pagGastosProc = @pagGastosProc, pagExceso = @pagExceso, pagTotal = @pagTotal, " &
                                    "pagestadoprocfrp = @pagestadoprocfrp, " &
                                    "pagFecExi = @pagFecExi, " &
                                    "pagTasaIntApl = @pagTasaIntApl, " &
                                    "pagdiasmora = @pagdiasmora, " &
                                    "pagvalcuota = @pagvalcuota, " &
                                    "pagNumConPag = @pagNumConPag, vlripc = @vlripc, pagLiqSan = 1 " &
                                    "WHERE NroConsignacion = @NroConsignacion "

        If OperacionSQL = "ADD" Then
            Command = New SqlCommand(InsertSQL, Connection, trans)
            Command.Parameters.AddWithValue("@NroConsignacion", txtNroConsignacionSancion.Text.Trim)
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
        Command.Parameters.AddWithValue("@UserVerif", Session("sscodigousuario"))

        If IsDate(txtFecSolverif.Text.Trim) Then
            Command.Parameters.AddWithValue("@FecSolverif", txtFecSolverif.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecSolverif", DBNull.Value)
        End If

        If IsDate(txtFecVerificado.Text.Trim) Then
            Command.Parameters.AddWithValue("@FecVerificado", txtFecVerificado.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecVerificado", DBNull.Value)
        End If

        If cboestado.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@estado", cboestado.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@estado", DBNull.Value)
        End If

        If IsDate(Left(txtpagFechaSancion.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@pagFecha", Left(txtpagFechaSancion.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@pagFecha", DBNull.Value)
        End If

        If IsDate(Left(txtpagFechaDeudorSancion.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@pagFechaDeudor", Left(txtpagFechaDeudorSancion.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@pagFechaDeudor", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@pagNroTitJudicial", txtpagNroTitJudicial.Text.Trim)

        If IsNumeric(txtpagCapitalSancion.Text) Then
            Command.Parameters.AddWithValue("@pagCapital", txtpagCapitalSancion.Text)
        Else
            Command.Parameters.AddWithValue("@pagCapital", DBNull.Value)
        End If


        Command.Parameters.AddWithValue("@pagAjusteDec1406", 0)
        Command.Parameters.AddWithValue("@pagInteres", 0)


        If IsNumeric(txtIpcSancion.Text) Then
            Command.Parameters.AddWithValue("@vlripc", txtIpcSancion.Text)
        Else
            Command.Parameters.AddWithValue("@vlripc", DBNull.Value)
        End If


        Command.Parameters.AddWithValue("@pagGastosProc", 0)

        If IsNumeric(txtpagExcesoSancion.Text) Then
            Command.Parameters.AddWithValue("@pagExceso", txtpagExcesoSancion.Text)
        Else
            Command.Parameters.AddWithValue("@pagExceso", DBNull.Value)
        End If

        If IsNumeric(txtPagTotalSancion.Text) Then
            Command.Parameters.AddWithValue("@pagTotal", txtPagTotalSancion.Text)
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
        If IsNumeric(txtpagvalcuota.Text) Then
            Command.Parameters.AddWithValue("@pagvalcuota", txtpagvalcuota.Text.Trim)
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

            UpdateSQL = "UPDATE ejefisglobal SET efipagoscap = @efipagoscap, efisaldocap = @efisaldocap WHERE efinroexp = '" & expediente & "'"
            Dim Command2 As SqlCommand
            Command2 = New SqlCommand(UpdateSQL, Connection, trans)

            Command2.Parameters.AddWithValue("@efipagoscap", efipagoscap)
            Command2.Parameters.AddWithValue("@efisaldocap", efisaldocap)
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
                CustomValidator1.Text = "Pago de Sanción guardado satisfactoriamente..."
                CustomValidator1.IsValid = False

                'If Year(CDate(txtpagFechaSancion.Text)) < Year(CDate(Now)) Then
                Dim ejecutarIpc As New GenerarIpcLiquidacionSancion
                DatosImportado = New DataTable
                DatosImportado = ejecutarIpc.LogicaIpc(txtNroExp.Text.Trim, CDbl(txtTotalDeudaSancion.Text), True)
                MostrarEA(txtNroExpEnc.Text)


                CustomValidator1.Text = "Pago de Sanción guardado satisfactoriamente, IPC recalculado Click en desacargar relacion IPC."
                CustomValidator1.IsValid = False
                'Else
                MostrarEA(txtNroExp.Text.Trim)
                'End If


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



    End Sub

    Protected Sub ImgTotalSancion_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgTotalSancion.Click
        Dim total As Double = 0

        '/-----------------------------------------------------------------  
        'ID _HU:  015
        'Nombre HU : Error en Cálculo del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 31-05-2017  
        'Objetivo : Se cambia y valida que ponga en 0 el valor siempre y cuando la 
        ' la propiedad text del campo este nula, vacía o sea espacios en blanco.
        '------------------------------------------------------------------/

        If String.IsNullOrWhiteSpace(txtIpcSancion.Text) Then
            txtIpcSancion.Text = 0
        End If

        If Not IsNumeric(txtpagCapitalSancion.Text.Trim) Then
            CustomValidator1.Text = "Capital pagado para la sanción no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
            Exit Sub
        ElseIf Not IsNumeric(txtpagAjusteDec1406.Text.Trim) Then
            CustomValidator1.Text = " Ajuste Decreto 1406 no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
            Exit Sub
        ElseIf Not IsDate(txtpagFechaSancion.Text) Then
            CustomValidator1.Text = "Por digite una fecha de pago valida para la sancion, VERIFIQUE."
            CustomValidator1.IsValid = False
            Exit Sub
        Else
            total = CDbl(txtpagCapitalSancion.Text.Trim)
        End If

        '/-----------------------------------------------------------------  
        'ID _HU:  015
        'Nombre HU : Error en Cálculo del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 30-05-2017  
        'Objetivo : Realizar el cálculo del IPC, para la parte de 
        ' sanción del títlo
        '------------------------------------------------------------------/
        If txtTIPOTITULO.Text.Trim.Equals("LIQUIDACIÓN OFICIAL / SANCIÓN") Then
            Dim CalcularIpc As New CalcularIPCinduvidual
            Dim _DatosRetorno As New CalcularIPCinduvidual._DatosRetorno
            Dim ipcPagos As Decimal = 0

            ' Jeisson Gómez 
            ' Si tiene pagos a capital, se retorna el ipc de los pagos, para que sea del saldo. 
            ' Jeisson Gómez 31/08/2017 HU_015 Error ingresando el pago. Se adiciona la condición AND SnContabilizar = 1, con el fin de contabilizar los pagos que esten 
            ' con el SnContabilizar = 1. Cuando la marca es SnContabilizar = 0, no debe tenerse en cuenta este pago. 
            Dim Sql As String = " SELECT ISNULL(SUM(vlripc), 0) ipcPagos FROM PAGOS P WHERE P.pagLiqSan = 1 AND P.NroExp = @NroExp AND P.SnContabilizar = 1 "
            Using Cn As New SqlConnection(Funciones.CadenaConexion)
                Cn.Open()
                Using Command As New SqlCommand(Sql, Cn)
                    Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
                    Using Reader = Command.ExecuteReader()
                        If Reader.Read() Then
                            ipcPagos = CDec(Reader("ipcPagos").ToString())
                        End If
                    End Using
                End Using
                Cn.Close()
            End Using


            Dim saldo As Decimal = CDec(txtTotalDeudaSancion.Text) - CDec(txtPagosCapitalSancion.Text) + CDec(ipcPagos)

            If saldo > 0 And (Year(CDate(txtpagFechaSancion.Text)) > Year(CDate(txtFechaExigTitulo.Text))) Then
                _DatosRetorno = CalcularIpc.CalculoIpc(Request("pExpediente"), txtpagFechaSancion.Text, saldo, Session("sscodigousuario"))
            Else
                _DatosRetorno.VALORIPC = 0
                _DatosRetorno.CALCULO = Nothing
            End If
            If _DatosRetorno._Error = "" Then
                txtIpcSancion.Text = _DatosRetorno.VALORIPC
            Else
                CustomValidator1.Text = _DatosRetorno._Error
                CustomValidator1.IsValid = False
            End If
            DatosImportado = _DatosRetorno.CALCULO
        End If

        txtPagTotalSancion.Text = total
    End Sub
End Class