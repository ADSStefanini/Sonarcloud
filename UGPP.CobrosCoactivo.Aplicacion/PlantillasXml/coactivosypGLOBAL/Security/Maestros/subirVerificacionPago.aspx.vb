Imports System.Data.SqlClient
Imports System.IO

Namespace Security.Maestros

    Partial Public Class SubirVerificacionPago
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack = False Then
                lblNomPerfil.Text = GetNomPerfil(Session("sscodigousuario"))
            End If
        End Sub

        Private Function GetNomPerfil(ByVal pUsuario As String) As String
            Dim nomPerfil As String = ""
            Dim connection As SqlConnection = New SqlConnection(Funciones.CadenaConexion)
            connection.Open()

            Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & Session("mnivelacces")

            Dim command = New SqlCommand(sql, connection)
            Dim reader As SqlDataReader = Command.ExecuteReader
            If reader.Read Then
                nomPerfil = reader("nombre").ToString().Trim
            End If
            reader.Close()
            connection.Close()
            Return nomPerfil
        End Function

        Protected Sub cmdImportarcsv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImportarcsv.Click
            

            Dim dtPagosDetalle As New DataTable
            Dim filePath As String = String.Empty
            If upload.HasFile AndAlso upload.PostedFile.ContentType.Equals("application/vnd.ms-excel") Then


                Try
                    dtPagosDetalle = CType(ConstruirDatatable(upload.PostedFile.FileName, ";"), DataTable)
                    Dim validar As String = Validaciones(dtPagosDetalle)

                    If validar <> "" Then
                        lblError.Text = validar
                        Exit Sub
                    End If

                Catch ex As Exception
                    'Gestionamos las excepciones, Aqui cada uno puede hacer lo que crea conveniente: Mostrar un error en Javascript en este caso y devolver el Datatable vacío
                    Dim mensaje As String
                    mensaje = "alert ('Ha ocurrido el siguiente error al importar el archivo: " & ex.Message & "');"
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Me.GetType(), "ErrorConstruirDatabale", mensaje, True)
                    lblError.Text = "Ha ocurrido el siguiente error al importar el archivo: " & ex.Message
                    Exit Sub
                End Try


                Dim cn As New SqlConnection(Funciones.CadenaConexion)
                cn.Open()
                Dim trans As SqlTransaction
                trans = cn.BeginTransaction


                Try
                    Dim LogProc As New LogProcesos
                    Dim SumatoriaPagoTotal As Double = 0

                    Dim lcExpediente As String = ""

                    'Campos para detalle de pagos
                    Dim expediente As String = ""
                    Dim nroconsignacion As String = ""
                    Dim nitEmpresa As String
                    Dim anno As Double
                    Dim mes As Double
                    Dim cedula As String
                    Dim subsistema As String
                    Dim ajuste As Double
                    Dim fecpago As Date
                    Dim fecreppagdeudor As Date
                    Dim fecverifpago As Date
                    Dim tdj As String
                    Dim capitalpag As Double
                    Dim ajustedec1406 As Double
                    Dim interespag As Double
                    Dim gastosproc As Double
                    Dim pagosexceso As Double
                    Dim totalpag As Double
                    Dim estadopag As String
                    Dim estadoprocfrp As String = GetEstadoProceso(dtPagosDetalle.Rows(0).Item("EXPEDIENTE").ToString.Trim, cn, trans)

                    Dim gestorreppag As String
                    Dim fecreppaggr As Date

                    For row As Integer = 0 To dtPagosDetalle.Rows.Count - 1
                        expediente = dtPagosDetalle.Rows(row)("EXPEDIENTE").ToString.Trim
                        If lcExpediente = "" Then
                            lcExpediente = expediente
                        End If
                        nroconsignacion = dtPagosDetalle.Rows(row)("NRO_CONSIGNACION_O_NRO_PLANILLA").ToString.Trim
                        nitEmpresa = dtPagosDetalle.Rows(row)("NIT_EMPRESA").ToString.Trim
                        anno = dtPagosDetalle.Rows(row)("ANNO").ToString.Trim
                        mes = dtPagosDetalle.Rows(row)("MES").ToString.Trim
                        cedula = dtPagosDetalle.Rows(row)("CEDULA").ToString.Trim
                        subsistema = dtPagosDetalle.Rows(row)("SUBSISTEMA").ToString.Trim
                        ajuste = dtPagosDetalle.Rows(row)("AJUSTE").ToString.Trim
                        '
                        If Date.TryParse(dtPagosDetalle.Rows(row)("FECHA_DE_PAGO").ToString.Trim, New Date) Then
                            fecpago = IIf(dtPagosDetalle.Rows(row)("FECHA_DE_PAGO").ToString.Trim = "", Date.Now, Date.Parse(dtPagosDetalle.Rows(row)("FECHA_DE_PAGO").ToString.Trim, New System.Globalization.CultureInfo("es-ES", True)).ToString("dd/MM/yyyy"))
                        Else
                            fecpago = IIf(dtPagosDetalle.Rows(row)("FECHA_DE_PAGO").ToString.Trim = "", Date.Now, Date.Parse(dtPagosDetalle.Rows(row)("FECHA_DE_PAGO").ToString.Trim, New System.Globalization.CultureInfo("en-US", True)).ToString("dd/MM/yyyy"))
                        End If

                        If Date.TryParse(dtPagosDetalle.Rows(row)("FECHA_DE_REPORTE_DEL_PAGO_POR_EL_DEUDOR").ToString.Trim, New Date) Then
                            fecreppagdeudor = IIf(dtPagosDetalle.Rows(row)("FECHA_DE_REPORTE_DEL_PAGO_POR_EL_DEUDOR").ToString.Trim = "", Date.Now, Date.Parse(dtPagosDetalle.Rows(row)("FECHA_DE_REPORTE_DEL_PAGO_POR_EL_DEUDOR").ToString.Trim, New System.Globalization.CultureInfo("es-Es", True)).ToString("dd/MM/yyyy"))
                        Else
                            fecreppagdeudor = IIf(dtPagosDetalle.Rows(row)("FECHA_DE_REPORTE_DEL_PAGO_POR_EL_DEUDOR").ToString.Trim = "", Date.Now, Date.Parse(dtPagosDetalle.Rows(row)("FECHA_DE_REPORTE_DEL_PAGO_POR_EL_DEUDOR").ToString.Trim, New System.Globalization.CultureInfo("en-US", True)).ToString("dd/MM/yyyy"))
                        End If

                        If Date.TryParse(dtPagosDetalle.Rows(row)("FECHA_DE_VERIFICACION_DEL_PAGO").ToString.Trim, New Date) Then
                            fecverifpago = IIf(dtPagosDetalle.Rows(row)("FECHA_DE_VERIFICACION_DEL_PAGO").ToString.Trim = "", Date.Now, Date.Parse(dtPagosDetalle.Rows(row)("FECHA_DE_VERIFICACION_DEL_PAGO").ToString.Trim, New System.Globalization.CultureInfo("es-ES", True)).ToString("dd/MM/yyyy"))
                        Else
                            fecverifpago = IIf(dtPagosDetalle.Rows(row)("FECHA_DE_VERIFICACION_DEL_PAGO").ToString.Trim = "", Date.Now, Date.Parse(dtPagosDetalle.Rows(row)("FECHA_DE_VERIFICACION_DEL_PAGO").ToString.Trim, New System.Globalization.CultureInfo("en-US", True)).ToString("dd/MM/yyyy"))
                        End If


                        tdj = dtPagosDetalle.Rows(row)("TITULO_DE_DEPOSITO_JUDICIAL").ToString.Trim
                        capitalpag = IIf(dtPagosDetalle.Rows(row)("CAPITAL_PAGADO").ToString.Trim = "", 0, dtPagosDetalle.Rows(row)("CAPITAL_PAGADO").ToString.Trim)
                        ajustedec1406 = IIf(dtPagosDetalle.Rows(row)("AJUSTE_DEC_1406").ToString.Trim = "", 0, dtPagosDetalle.Rows(row)("AJUSTE_DEC_1406").ToString.Trim)
                        interespag = IIf(dtPagosDetalle.Rows(row)("INTERESES_PAGADOS").ToString.Trim = "", 0, dtPagosDetalle.Rows(row)("INTERESES_PAGADOS").ToString.Trim)
                        gastosproc = IIf(dtPagosDetalle.Rows(row)("GASTOS_DEL_PROCESO").ToString.Trim = "", 0, dtPagosDetalle.Rows(row)("GASTOS_DEL_PROCESO").ToString.Trim)
                        pagosexceso = IIf(dtPagosDetalle.Rows(row)("PAGOS_EN_EXCESO").ToString.Trim = "", 0, dtPagosDetalle.Rows(row)("PAGOS_EN_EXCESO").ToString.Trim)
                        totalpag = CInt(capitalpag) + CInt(ajustedec1406) + CInt(interespag)
                        estadopag = IIf(capitalpag >= ajuste, "04", IIf(capitalpag = 0, "05", "02"))

                        'estadoprocfrp = dtPagosDetalle.Rows(row)("ESTADO_DEL_PROCESO_EN_LA_FECHA_DE_REPORTE_DEL_PAGO").ToString.Trim
                        gestorreppag = Session("sscodigousuario")
                        fecreppaggr = Date.Now

                        If (capitalpag > 0) Then
                            SumatoriaPagoTotal = SumatoriaPagoTotal + totalpag
                            Save_PD(expediente, nroconsignacion, nitEmpresa, anno, mes, cedula, subsistema, ajuste, fecpago, fecreppagdeudor, fecverifpago, tdj, capitalpag, ajustedec1406, interespag, gastosproc, pagosexceso, totalpag, estadopag, estadoprocfrp, gestorreppag, fecreppaggr, trans, cn, LogProc)
                            '--ActualizarMontoSqlPlanilla(expediente, subsistema, nitEmpresa, anno, mes, cedula, capitalpag, trans, cn)
                        End If

                    Next

                    'Actualizar la tabla de pagos
                    ActualizarTablaPagos(lcExpediente, cn, trans, LogProc)

                    Dim VerificarAcuerdo As Boolean = VerificarAcuerdoVigente(expediente)
                    Dim mensajeAcuerdo As String = ""

                    '11-11-2014 Obtener el pago total 
                    Dim Ptotal As Double = SumatoriaPagoTotal

                    If Not VerificarNroPLantilla(nroconsignacion, cn, trans) Then

                        If VerificarAcuerdo Then

                            Dim VerificarCuotas As DataTable = VerificarCuotasAcuerdoVigente(expediente)
                            If VerificarCuotas.Rows.Count > 0 Then

                                If CDbl(VerificarCuotas.Rows(0).Item("VALOR_CUOTA")) = CDbl(Ptotal) Then
                                    UpdateCuotaFacilidadPago(expediente, CDbl(Ptotal), fecreppagdeudor, VerificarCuotas.Rows(0).Item("CUOTA_NUMERO"), CStr(VerificarCuotas.Rows(0).Item("DOCUMENTO")), cn, trans, LogProc)
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
                        lblError.Text = mensajeAcuerdo

                    Else
                        trans.Commit()
                        lblError.Text = String.Format("{0} registro(s) procesados...", dtPagosDetalle.Rows.Count)
                    End If
                    'lblError.Text = String.Format("{0} registro(s) procesados...", dtPagosDetalle.Rows.Count)
                Catch ex As Exception
                    trans.Rollback()
                    'lblError.Text = "Error: por favor compruebe la información del archivo, es posible que hallan filas en blanco ó falten columnas a importar"
                    lblError.Text = ex.Message
                Finally
                    cn.Close()
                End Try
            Else
                lblError.Text = "Por favor, compruebe el tipo de archivo seleccionado..."
                lblError.Visible = True
            End If
        End Sub

        Private Function ReadToEnd(ByVal filePath As String) As Object
            Dim dtDataSource As New DataTable()
            Dim targetPath As String = HttpRuntime.AppDomainAppPath & "temp_arch\" & Path.GetFileName(filePath)
            upload.PostedFile.SaveAs(targetPath)

            Dim fileContent As String() = File.ReadAllLines(targetPath)
            Try

                If fileContent.Any() Then
                    'Agregar Columnas de archivo
                    Dim columns As String() = fileContent(0).Split(";"c)
                    For i As Integer = 0 To columns.Count() - 1
                        dtDataSource.Columns.Add(columns(i))
                    Next
                    'agregar filas.
                    For i As Integer = 1 To fileContent.Count() - 1
                        Dim rowData As String() = fileContent(i).Split(";"c)
                        dtDataSource.Rows.Add(rowData)
                    Next

                End If
            Catch ex As Exception
                lblError.Text = String.Format("Error: {0}Compruebe el archivo .csv", ex.ToString)
            End Try
            Return dtDataSource
        End Function

        Private Sub ActualizarMontoSqlPlanilla(ByVal expediente As String, ByVal subsistema As String, ByVal nitEmpresa As String, ByVal anno As Double, ByVal mes As Double, ByVal cedula As String, ByVal montoPago As Double, ByVal trans As SqlTransaction, ByVal cn As SqlConnection)
            Dim command As New SqlCommand("save_SQL_PLANILLA", cn, trans)
            command.CommandType = CommandType.StoredProcedure
            'Campos
            command.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            command.Parameters.AddWithValue("@SUBSISTEMA", subsistema)
            command.Parameters.AddWithValue("@NIT_EMPRESA", nitEmpresa)
            command.Parameters.AddWithValue("@ANNO", anno)
            command.Parameters.AddWithValue("@MES", mes)
            command.Parameters.AddWithValue("@CEDULA", cedula)
            command.Parameters.AddWithValue("@MONTO_PAGO", montoPago)
            '
            command.ExecuteNonQuery()
        End Sub

        Private Sub Save_PD(ByVal expediente As String, ByVal nroconsignacion As String, ByVal nitEmpresa As String, ByVal anno As Double, ByVal mes As Double, ByVal cedula As String, ByVal subsistema As String, ByVal ajuste As Double, ByVal fecpago As Date, ByVal fecreppagdeudor As Date, ByVal fecverifpago As Date, ByVal tdj As String, ByVal capitalpag As Double, ByVal ajustedec1406 As Double, ByVal interespag As Double, ByVal gastosproc As Double, ByVal pagosexceso As Double, ByVal totalpag As Double, ByVal estadopag As String, ByVal estadoprocfrp As String, ByVal gestorreppag As String, ByVal fecreppaggr As Date, ByVal trans As SqlTransaction, ByVal cn As SqlConnection, ByVal LogProc As LogProcesos)

            Dim command As New SqlCommand("save_PAGOS_DETALLE", cn, trans)
            command.CommandType = CommandType.StoredProcedure
            'Campos
            command.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            command.Parameters.AddWithValue("@NROCONSIGNACION", nroconsignacion)
            command.Parameters.AddWithValue("@NIT_EMPRESA", nitEmpresa)
            command.Parameters.AddWithValue("@ANNO", anno)
            command.Parameters.AddWithValue("@MES", mes)
            command.Parameters.AddWithValue("@CEDULA", cedula)
            command.Parameters.AddWithValue("@SUBSISTEMA", subsistema)
            command.Parameters.AddWithValue("@AJUSTE", ajuste)
            command.Parameters.AddWithValue("@FECPAGO", fecpago)
            command.Parameters.AddWithValue("@FECREPPAGDEUDOR", fecreppagdeudor)
            command.Parameters.AddWithValue("@FECVERIFPAGO", fecverifpago)
            command.Parameters.AddWithValue("@TDJ", tdj)
            command.Parameters.AddWithValue("@CAPITALPAG", capitalpag)
            command.Parameters.AddWithValue("@AJUSTEDEC1406", ajustedec1406)
            command.Parameters.AddWithValue("@INTERESPAG", interespag)
            command.Parameters.AddWithValue("@GASTOSPROC", gastosproc)
            command.Parameters.AddWithValue("@PAGOSEXCESO", pagosexceso)
            command.Parameters.AddWithValue("@TOTALPAG", totalpag)
            command.Parameters.AddWithValue("@ESTADOPAG", estadopag)
            command.Parameters.AddWithValue("@ESTADOPROCFRP", estadoprocfrp)
            command.Parameters.AddWithValue("@GESTORREPPAG", gestorreppag)
            command.Parameters.AddWithValue("@FECREPPAGGR", fecreppaggr)

            command.ExecuteNonQuery()
            LogProc.SaveLog2(Session("ssloginusuario"), "Registro de pagos", "No. Expediente " & expediente, command, cn, trans)

        End Sub

        Private Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
            CerrarSesion()
            Response.Redirect("../../login.aspx")
        End Sub

        Private Sub CerrarSesion()
            FormsAuthentication.SignOut()
            'Limpiar los cuadros de texto de busqueda
            'Limpiar cuadros de busqueda
            Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
            Session("EJEFISGLOBAL.txtSearchED_NOMBRE") = ""
            Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
            Session("EJEFISGLOBAL.cboEFIESTADO") = ""
            Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
            Session("Paginacion") = 10
        End Sub

        Private Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click
            Response.Redirect("PAGOS.aspx")
        End Sub

        Private Sub ActualizarTablaPagos(ByVal pExpediente As String, ByVal connection As SqlConnection, ByVal tran As SqlTransaction, ByVal LogProc As LogProcesos)
            Dim sql As String
            sql = "SELECT NroConsignacion, MAX(Expediente) AS Expediente, MAX(FecRepPagGR) AS FecRepPagGR, " & _
                  "	MAX(FecVerifPago) AS FecVerifPago, MAX(FecRepPagDeudor) AS FecRepPagDeudor," & _
                  "	SUM(CapitalPag) AS CapitalPag, " & _
                  "	SUM(AjusteDec1406) AS AjusteDec1406, SUM(InteresPag) AS InteresPag," & _
                  "	SUM(gastosproc) AS gastosproc, SUM(PagosExceso) AS PagosExceso," & _
                  "	SUM(TotalPag) AS TotalPag," & _
                  "	MAX(EstadoPag) AS EstadoPag," & _
                  "	MAX(EstadoProcFRP) AS EstadoProcFRP " & _
                  "FROM pagos_detalle " & _
                  "WHERE Expediente = '" & pExpediente & "' " & _
                  "GROUP BY NroConsignacion"

            Dim command As SqlCommand = New SqlCommand(sql, connection, tran)
            Dim tb As New DataTable
            Dim ad As New SqlDataAdapter(command)
            ad.Fill(tb)

            'Variables para actualizar tabla de pagos
            Dim nroConsignacion As String = ""
            Dim expediente As String = ""
            Dim fecRepPagGr, fecVerifPago, fecRepPagDeudor As Date
            Dim capitalPag As Integer = 0
            Dim ajusteDec1406 As Double = 0
            Dim interesPag As Double = 0
            Dim gastosproc As Double = 0
            Dim pagosExceso As Double = 0
            Dim totalPag As Double = 0
            Dim estadoPag As String = ""
            Dim estadoProcFrp As String = ""

            For Each row As DataRow In tb.Rows
                nroConsignacion = row("NroConsignacion").ToString().Trim
                expediente = row("Expediente").ToString().Trim
                fecRepPagGr = row("FecRepPagGR").ToString()
                fecVerifPago = row("FecVerifPago").ToString()
                fecRepPagDeudor = row("FecRepPagDeudor").ToString()
                capitalPag = row("CapitalPag").ToString()
                ajusteDec1406 = row("AjusteDec1406").ToString()
                interesPag = row("InteresPag").ToString()
                gastosproc = row("gastosproc").ToString()
                pagosExceso = row("PagosExceso").ToString()
                totalPag = row("TotalPag").ToString()
                estadoPag = row("EstadoPag").ToString().Trim
                estadoProcFrp = row("EstadoProcFRP").ToString().Trim

                'Guardar pago
                GuardarPago(nroConsignacion, expediente, fecRepPagGr, fecVerifPago, fecRepPagDeudor, capitalPag, ajusteDec1406, interesPag, gastosproc, pagosExceso, totalPag, estadoPag, estadoProcFrp, connection, tran, LogProc)
            Next

            '07/oct/2014. Actualizar los campos EFIPAGOSCAP y EFISALDOCAP en la tabla EJEFISGLOBAL
            Dim efipagoscap As Double = 0
            Dim efisaldocap As Double = 0

            ConsultarPagosSaldos(expediente, efipagoscap, efisaldocap, connection, tran)

            Dim UpdateSQL As String = "UPDATE ejefisglobal SET efipagoscap = @efipagoscap, efisaldocap = @efisaldocap WHERE efinroexp = '" & expediente & "'"
            Dim Command2 As SqlCommand
            Command2 = New SqlCommand(UpdateSQL, connection, tran)
            Command2.Parameters.AddWithValue("@efipagoscap", efipagoscap)
            Command2.Parameters.AddWithValue("@efisaldocap", efisaldocap)
            Try
                Command2.ExecuteNonQuery()
                LogProc.SaveLog2(Session("ssloginusuario"), "Registro de pagos", "No. Expediente " & expediente, Command2, connection, tran)
            Catch ex As Exception
                Dim msg As String = ex.Message

            End Try


        End Sub

        Private Sub ConsultarPagosSaldos(ByVal pExpediente As String, ByRef efipagoscap As Double, ByRef efisaldocap As Double, ByVal connection As SqlConnection, ByVal tran As SqlTransaction)
            If pExpediente <> "" Then
                Dim TotalDeuda As Double = 0
                Dim TotalPagos As Double = 0

                'Conexion a la base de datos
                'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
                'Connection.Open()

                'Consultar el total de la deuda
                Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " & _
                                              "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pExpediente.Trim & "' GROUP BY MT_expediente"
                Dim Command As New SqlCommand(sql, connection, tran)
                Dim Reader As SqlDataReader = Command.ExecuteReader
                If Reader.Read Then
                    TotalDeuda = Convert.ToDouble(Reader("totaldeuda").ToString())
                Else
                    TotalDeuda = 0
                End If
                Reader.Close()

                'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor).
                Dim sql2 As String = "SELECT SUM(pagcapital) AS pagcapital FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' GROUP BY NroExp"
                Dim Command2 As New SqlCommand(sql2, connection, tran)
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

        Private Sub GuardarPago(ByVal nroConsignacion As String, ByVal expediente As String, ByVal fecRepPagGr As Date, ByVal fecVerifPago As Date, ByVal fecRepPagDeudor As Date, ByVal capitalPag As Double, ByVal ajusteDec1406 As Double, ByVal interesPag As Double, ByVal gastosproc As Double, ByVal pagosExceso As Double, ByVal totalPag As Double, ByVal estadoPag As String, ByVal estadoProcFrp As String, ByVal connection As SqlConnection, ByVal trans As SqlTransaction, ByVal LogProc As LogProcesos)
            Dim command As SqlCommand
            Const insertSql As String = "INSERT INTO pagos (NroConsignacion, NroExp, FecSolverif, FecVerificado, pagFechaDeudor, pagCapital, pagAjusteDec1406, pagInteres, pagGastosProc, pagExceso, pagTotal, estado, pagestadoprocfrp, UserVerif, pagFecha) " & _
                                        "VALUES " & _
                                        "(@NroConsignacion, @NroExp, @FecSolverif, @FecVerificado, @pagFechaDeudor, @pagCapital, @pagAjusteDec1406, @pagInteres, @pagGastosProc, @pagExceso, @pagTotal, @estado, @pagestadoprocfrp, @UserVerif, @pagFecha) "

            Const updateSql As String = "UPDATE PAGOS SET " & _
                                        "		NroConsignacion = @NroConsignacion, NroExp = @NroExp, FecSolverif = @FecSolverif, FecVerificado = @FecVerificado, " & _
                                        "		pagFechaDeudor = @pagFechaDeudor, pagCapital = @pagCapital, pagAjusteDec1406 = @pagAjusteDec1406, pagInteres = @pagInteres, " & _
                                        "		pagGastosProc = @pagGastosProc, pagExceso = @pagExceso, pagTotal = @pagTotal, estado = @estado, pagestadoprocfrp = @pagestadoprocfrp, " & _
                                        "		UserVerif = @UserVerif, pagFecha = @pagFecha " & _
                                        "WHERE NroConsignacion = @NroConsignacion AND NroExp = @NroExp "

            If ConsultarPago(nroConsignacion, expediente) Then
                'Si existe resgistro => Update
                command = New SqlCommand(updateSql, connection, trans)
            Else
                'Si NO existe resgistro => Insert
                command = New SqlCommand(insertSql, connection, trans)
            End If

            'Parametros
            command.Parameters.AddWithValue("@NroConsignacion", nroConsignacion)
            command.Parameters.AddWithValue("@NroExp", expediente)
            command.Parameters.AddWithValue("@FecSolverif", fecRepPagGr)
            command.Parameters.AddWithValue("@FecVerificado", fecVerifPago)
            command.Parameters.AddWithValue("@pagFechaDeudor", fecRepPagDeudor)
            command.Parameters.AddWithValue("@pagCapital", capitalPag)
            command.Parameters.AddWithValue("@pagAjusteDec1406", ajusteDec1406)
            command.Parameters.AddWithValue("@pagInteres", interesPag)
            command.Parameters.AddWithValue("@pagGastosProc", gastosproc)
            command.Parameters.AddWithValue("@pagExceso", pagosExceso)
            command.Parameters.AddWithValue("@pagTotal", totalPag)
            command.Parameters.AddWithValue("@estado", estadoPag)
            command.Parameters.AddWithValue("@pagestadoprocfrp", estadoProcFrp)
            command.Parameters.AddWithValue("@UserVerif", Session("sscodigousuario"))
            command.Parameters.AddWithValue("@pagFecha", Date.Now)

            command.ExecuteNonQuery()

            LogProc.SaveLog2(Session("ssloginusuario"), "Registro de pagos", "No. Expediente " & expediente, command, connection, trans)

        End Sub

        Private Function ConsultarPago(ByVal pNroConsignacion As String, ByVal pExpediente As String) As Boolean
            Dim resp As Boolean = False

            Dim sql As String
            sql = "SELECT * FROM pagos WHERE NroConsignacion = '" & pNroConsignacion & "' AND NroExp = '" & pExpediente & "'"
            Dim connection As New SqlConnection(Funciones.CadenaConexion())
            connection.Open()
            Dim command As SqlCommand = New SqlCommand(sql, connection)
            Dim reader As SqlDataReader = command.ExecuteReader
            If reader.Read() Then
                resp = True
            End If
            '
            Return resp
        End Function

        Private Function ConstruirDatatable(ByVal RutaCompletaArchivo As String, ByVal Separador As Char) As DataTable

            'declaramos la Tabla donde añadiremos los datos y la fila correspondiente
            Dim MiTabla As DataTable = New DataTable("MyTable")
            Dim MiFila As DataRow
            'declaramos el resto de variables que nos harán falta
            Dim pos As Boolean = False
            Dim i As Integer
            Dim fieldValues As String()
            Dim miReader As IO.StreamReader

            'Try
                Dim TargetPath As String = HttpRuntime.AppDomainAppPath & "temp_arch\" & Path.GetFileName(RutaCompletaArchivo)
                upload.PostedFile.SaveAs(TargetPath)

                'Abrimos el fichero y leemos la primera linea con el fin de determinar cuantos campos tenemos
                miReader = File.OpenText(TargetPath)
                fieldValues = miReader.ReadLine().Split(Separador)

                'Creamos las columnas de la cabecera
                For i = 0 To fieldValues.Length() - 1
                    MiTabla.Columns.Add(New DataColumn(fieldValues(i).ToString().Trim))
                Next

                'Continuamos leyendo el resto de filas y añadiendolas a la tabla
                While miReader.Peek() <> -1
                    fieldValues = miReader.ReadLine().Split(Separador)
                    MiFila = MiTabla.NewRow
                    For i = 0 To fieldValues.Length() - 1
                        MiFila.Item(i) = fieldValues(i).ToString.Trim
                    Next


                    MiTabla.Rows.Add(MiFila)
                End While
                'Cerramos el reader
                miReader.Close()
            'Catch ex As Exception
            '    'Gestionamos las excepciones, Aqui cada uno puede hacer lo que crea conveniente: Mostrar un error en Javascript en este caso y devolver el Datatable vacío
            '    Dim mensaje As String
            '    mensaje = "alert ('Ha ocurrido el siguiente error al importar el archivo: " + ex.ToString + "');"
            '    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Me.GetType(), "ErrorConstruirDatabale", mensaje, True)
            '    Return New DataTable("Empty")
            'Finally
            'Si queremos ejecutar algo exista excepción o no
            'End Try
            'Devolvemos el DataTable si todo ha ido bien
            Return MiTabla
        End Function

        Private Function Validaciones(ByVal dt As DataTable) As String


            Dim Mensaje As String = ""

            If dt.Rows.Count > 0 Then

                Dim SearhExp As DataTable = Funciones.RetornaCargadatos("SELECT * FROM EJEFISGLOBAL WHERE EFINROEXP = '" & dt.Rows(0).Item("EXPEDIENTE").ToString.Trim & "' ")

                'Verificar si el expediente existe.
                If SearhExp.Rows.Count = 0 Then
                    Mensaje = "No se pudo subir el SQL. No se detectó el expediente. ."
                Else

                    'Verificar si el archivo a subir tiene expedientes distintos
                    Dim distinctExp = From row In dt.AsEnumerable() _
                        Select row.Field(Of String)("EXPEDIENTE") Distinct
                    If distinctExp.Count > 1 Then
                        Mensaje = "No se pudo subir el SQL. Se detectó expedientes distintos en el archivo a subir por favor verifique."

                    Else
                        'Verificar numero de plani no null ni vacio
                        For Each r As DataRow In dt.Rows
                            If r("NRO_CONSIGNACION_O_NRO_PLANILLA") = "" Or r("NRO_CONSIGNACION_O_NRO_PLANILLA") = "NULL" Or r("NRO_CONSIGNACION_O_NRO_PLANILLA") = "null" Then
                                Mensaje = "Pro favor verifique el archivo, no puede haber Nro de consinación o Planilla vacias.."
                            End If
                        Next
                    End If

                End If
            Else
                Mensaje = "No se detectó pagos a procesar... "
            End If
            Return Mensaje
        End Function

        Private Function GetEstadoProceso(ByVal pExpediente As String, ByVal connection As SqlConnection, ByVal trans As SqlTransaction) As String
            Dim codEstado As String = ""

            Dim sql As String = "SELECT * FROM EJEFISGLOBAL WHERE EFINROEXP = @expediente"

            Dim command = New SqlCommand(sql, connection, trans)
            command.Parameters.AddWithValue("@expediente", pExpediente)
            Dim tb As New DataTable
            Dim ad As New SqlDataAdapter(command)
            ad.Fill(tb)

            If tb.Rows.Count > 0 Then
                codEstado = tb.Rows(0).Item("EFIESTADO").ToString().Trim
            End If
            Return codEstado
        End Function


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



        Private Sub UpdateCuotaFacilidadPago(ByVal expediente As String, ByVal pValorPagado As Double, ByVal pFechaPago As Date, ByVal pNumeroCuota As Integer, ByVal pNumeroAcuerdo As String, ByVal Connection As SqlConnection, ByVal trans As SqlTransaction, ByVal LogProc As LogProcesos)

            Dim UpdateSQL As String = "UPDATE DETALLES_ACUERDO_PAGO SET FECHA_PAGO = @FECHA_PAGO, VALOR_PAGADO = @VALOR_PAGADO WHERE CUOTA_NUMERO = @CUOTA_NUMERO AND DOCUMENTO = @DOCUMENTO "
            Dim Command As SqlCommand
            Command = New SqlCommand(UpdateSQL, Connection, trans)

            Command.Parameters.AddWithValue("@FECHA_PAGO", pFechaPago)
            Command.Parameters.AddWithValue("@VALOR_PAGADO", pValorPagado)
            Command.Parameters.AddWithValue("@CUOTA_NUMERO", pNumeroCuota)
            Command.Parameters.AddWithValue("@DOCUMENTO", pNumeroAcuerdo)

            Command.ExecuteNonQuery()
            LogProc.SaveLog2(Session("ssloginusuario"), "Registro de pagos", "No. Expediente " & expediente, Command, Connection, trans)

        End Sub

        Private Function VerificarNroPLantilla(ByVal pNroConsignacion As String, ByVal cn As SqlConnection, ByVal trans As SqlTransaction) As Boolean
            Dim sw As Boolean = False
            Dim tb As DataTable = Funciones.RetornarDatatable("SELECT * FROM PAGOS WHERE nroConsignacion = '" & pNroConsignacion & "'", cn, trans)

            If tb.Rows.Count > 0 Then
                sw = True
            End If

            Return sw
        End Function



    End Class

End Namespace