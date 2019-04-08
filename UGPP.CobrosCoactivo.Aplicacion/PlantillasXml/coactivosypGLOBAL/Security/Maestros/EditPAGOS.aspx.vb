Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EditPAGOS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
                Dim sql As String = "select * from PAGOS where NroConsignacion = @NroConsignacion"
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
                    txtNroConsignacion.Text = Reader("NroConsignacion").ToString()
                    txtNroExp.Text = Reader("NroExp").ToString().Trim
                    txtFecSolverif.Text = Left(Reader("FecSolverif").ToString().Trim, 10)
                    txtFecVerificado.Text = Left(Reader("FecVerificado").ToString().Trim, 10)
                    cboestado.SelectedValue = Reader("estado").ToString()
                    txtpagFecha.Text = Left(Reader("pagFecha").ToString().Trim, 10)
                    txtpagFechaDeudor.Text = Left(Reader("pagFechaDeudor").ToString().Trim, 10)
                    txtpagNroTitJudicial.Text = Reader("pagNroTitJudicial").ToString()
                    txtpagCapital.Text = Reader("pagCapital").ToString()
                    txtpagAjusteDec1406.Text = Reader("pagAjusteDec1406").ToString()
                    txtpagInteres.Text = Reader("pagInteres").ToString()
                    txtpagGastosProc.Text = Reader("pagGastosProc").ToString()
                    txtpagExceso.Text = Reader("pagExceso").ToString()
                    txtpagTotal.Text = Reader("pagTotal").ToString()
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
                        MostrarEncabezado(NumExp)
                        MostrarEA(NumExp)
                        MostrarGestorResp(NumExp)
                        MostrarTitulo(NumExp)
                        MostrarDeudor(NumExp)
                    End If

                End If                
                Reader.Close()
                Connection.Close()                
            Else
                If Len(Request("pExpediente")) > 0 Then
                    Dim NumExp As String = Request("pExpediente").Trim
                    If NumExp <> "" Then
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
            Dim MTG As New MetodosGlobalesCobro
            lblNomPerfil.Text = MTG.GetNomPerfil(Session("mnivelacces"))

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

        End If
    End Sub

    Private Sub MostrarEncabezado(ByVal pExpediente As String)
        'Conexion a la base de datos
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        If Len(pExpediente) > 0 Then
            Dim sql As String = "SELECT EJEFISGLOBAL.*, ESTADOS_PROCESO.nombre AS NomEstadoProc " & _
                                               "FROM EJEFISGLOBAL " & _
                                               "LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " & _
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

            'Conexion a la base de datos
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            'Consultar el total de la deuda
            Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " & _
                                          "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pNumExpediente.Trim & "' GROUP BY MT_expediente"
            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'Dim n As Double = Convert.ToDouble(Reader("totaldeuda").ToString())
                'txtTotalDeudaEA.Text = n.ToString("N0")                
                txtTotalDeudaEA.Text = Convert.ToDouble(Reader("totaldeuda").ToString()).ToString("N0")
            Else
                txtTotalDeudaEA.Text = "0"
            End If
            Reader.Close()

            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital " & _
                                    "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' GROUP BY NroExp"

            Dim Command2 As New SqlCommand(sql2, Connection)
            Dim Reader2 As SqlDataReader = Command2.ExecuteReader
            If Reader2.Read Then
                If Reader2("pagcapital").ToString() = "" Then
                    txtPagosCapitalEA.Text = "0"
                Else
                    txtPagosCapitalEA.Text = Convert.ToDouble(Reader2("pagcapital").ToString()).ToString("N0")
                End If
            Else
                txtPagosCapitalEA.Text = "0"
            End If
            Reader2.Close()

            'Mostrar la diferencia entre Total deuda - Capital pagado
            Dim saldoEA As Double = Convert.ToDouble(txtTotalDeudaEA.Text) - Convert.ToDouble(txtPagosCapitalEA.Text)
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
            Dim sql As String = "SELECT MT.MT_nro_titulo, MT.MT_fec_expedicion_titulo, MT.MT_tipo_titulo, TT.nombre AS NomTipoTitulo " & _
                                " FROM MAESTRO_TITULOS MT  " & _
                                " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " & _
                                "WHERE MT.MT_expediente = '" & pNumExpediente & "'"

            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'txtTotalDeudaEA.Text = Convert.ToDouble(Reader("totaldeuda").ToString()).ToString("N0")
                txtNUMTITULOEJECUTIVO.Text = Reader("MT_nro_titulo").ToString()
                txtFECTITULO.Text = Left(Reader("MT_fec_expedicion_titulo").ToString(), 10)
                txtTIPOTITULO.Text = Reader("NomTipoTitulo").ToString()
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

            Dim cmd As String = "SELECT TOP 1 deudor, ED.ED_Nombre " & _
                                "FROM DEUDORES_EXPEDIENTES DxE " & _
                                "LEFT JOIN ENTES_DEUDORES ED ON DxE.deudor = ED.ED_Codigo_Nit " & _
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

        If Len(Request("ID")) > 0 Then
            ID = Request("ID")
        Else
            ID = txtNroConsignacion.Text.Trim
        End If

        Dim OperacionSQL As String = ""
        If Len(Request("pExpediente")) > 0 Then
            OperacionSQL = "ADD"
        Else
            OperacionSQL = "EDT"
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
        Dim InsertSQL As String = "INSERT INTO pagos (NroConsignacion, NroExp, FecSolverif, FecVerificado, estado, UserVerif, " & _
                                    "pagFecha, pagFechaDeudor, pagNroTitJudicial, pagCapital, pagAjusteDec1406, pagInteres, " & _
                                    "pagGastosProc, pagExceso, pagTotal, pagestadoprocfrp, pagFecExi, pagTasaIntApl, pagdiasmora, " & _
                                    "pagvalcuota, pagNumConPag" & _
                                    " ) VALUES ( @NroConsignacion, @NroExp, @FecSolverif, @FecVerificado, @estado, @UserVerif, " & _
                                    "@pagFecha, @pagFechaDeudor, @pagNroTitJudicial, @pagCapital, @pagAjusteDec1406, @pagInteres, " & _
                                    "@pagGastosProc, @pagExceso, @pagTotal, @pagestadoprocfrp, @pagFecExi, @pagTasaIntApl, @pagdiasmora, " & _
                                    "@pagvalcuota, @pagNumConPag)"

        Dim UpdateSQL As String = "Update PAGOS SET NroExp = @NroExp, FecSolverif = @FecSolverif, FecVerificado = @FecVerificado, estado = @estado, UserVerif = @UserVerif, " & _
                                    "pagFecha = @pagFecha, pagFechaDeudor = @pagFechaDeudor, pagNroTitJudicial = @pagNroTitJudicial, " & _
                                    "pagCapital = @pagCapital, pagAjusteDec1406 = @pagAjusteDec1406, pagInteres = @pagInteres, " & _
                                    "pagGastosProc = @pagGastosProc, pagExceso = @pagExceso, pagTotal = @pagTotal, " & _
                                    "pagestadoprocfrp = @pagestadoprocfrp, " & _
                                    "pagFecExi = @pagFecExi, " & _
                                    "pagTasaIntApl = @pagTasaIntApl, " & _
                                    "pagdiasmora = @pagdiasmora, " & _
                                    "pagvalcuota = @pagvalcuota, " & _
                                    "pagNumConPag = @pagNumConPag " & _
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
                CustomValidator1.Text = "Pago guardado satisfactoriamente..."
                CustomValidator1.IsValid = False
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
        Response.Redirect("PAGOS.aspx?pExpediente=" & txtNroExp.Text.Trim)
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

    Protected Sub cmdSolicitudCambioEstado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSolicitudCambioEstado.Click
        'Reenviar  la pagina de solicitudes de cambio de estado con un marco para navegacion
        Response.Redirect("EditPagCambioEstado.aspx?pExpediente=" & txtNroExpEnc.Text.Trim & "&ID=" & txtNroConsignacion.Text.Trim)
    End Sub

    Private Sub ConsultarPagosSaldos(ByVal pExpediente As String, ByRef efipagoscap As Double, ByRef efisaldocap As Double, ByVal Connection As SqlConnection, ByVal trans As SqlTransaction)
        If pExpediente <> "" Then
            Dim TotalDeuda As Double = 0
            Dim TotalPagos As Double = 0

            'Conexion a la base de datos
            ' Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            'Connection.Open()

            'Consultar el total de la deuda
            Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " & _
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
            Dim sql2 As String = "SELECT SUM(pagcapital) AS pagcapital FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' GROUP BY NroExp"
            Dim Command2 As New SqlCommand(sql2, Connection, trans)
            Dim Reader2 As SqlDataReader = Command2.ExecuteReader
            If Reader2.Read Then
                If Reader2("pagcapital").ToString() = "" Then
                    TotalPagos = 0
                Else
                    TotalPagos = Convert.ToDouble(Reader2("pagcapital").ToString())
                End If
            Else
                TotalPagos = 0
            End If
            Reader2.Close()

            'Mostrar la diferencia entre Total deuda - Capital pagado
            Dim saldoEA As Double = TotalDeuda - TotalPagos
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

        If Not IsNumeric(txtpagCapital.Text.Trim) Then
            CustomValidator1.Text = "Capital pagado no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
        ElseIf Not IsNumeric(txtpagAjusteDec1406.Text.Trim) Then
            CustomValidator1.Text = " Ajuste Decreto 1406 no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
        ElseIf Not IsNumeric(txtpagInteres.Text.Trim) Then
            CustomValidator1.Text = "Intereses pagados no es un numero valido, VERIFIQUE."
            CustomValidator1.IsValid = False
        Else
            total = CDbl(txtpagCapital.Text.Trim) + CDbl(txtpagAjusteDec1406.Text.Trim) + CDbl(txtpagInteres.Text.Trim)
        End If

        txtpagTotal.Text = total

        Return total
    End Function


End Class


