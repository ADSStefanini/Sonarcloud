Imports System.Data.SqlClient
Imports System.IO
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class proyectarcuotaparafiscales
    Inherits System.Web.UI.Page

    Protected Shared DetallesCuotas As New DataTable
    Protected Shared TotalDeudaSinintereses As New Integer
    Protected Shared DisminuirDeuda As New Integer
    Protected Shared TablaDetalle As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
    Protected Shared rowDetalle As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable

    Protected Shared LoadDatosDetalles As DataTable

    Dim Conexion As SqlConnection
    Dim Documento As SqlCommand
    Dim Update_AP As SqlCommand
    Dim _Adapter As SqlDataAdapter
    Dim Plazo As SqlCommand
    Dim Porcentaje As SqlCommand

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        InitializeComponent()
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        Conexion = New SqlConnection
        Documento = New SqlCommand
        Update_AP = New SqlCommand
        _Adapter = New SqlDataAdapter
        Plazo = New SqlCommand
        Porcentaje = New SqlCommand

        'Conexion

        Conexion.ConnectionString = Funciones.CadenaConexion

        'Documento

        Documento.CommandText = "SELECT * FROM MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 13"
        Documento.Connection = Conexion

        'PLAZO EN MEES

        Plazo.CommandText = "SELECT APL_DESDE,APL_HASTA, APL_PLAZO_MES FROM ACUERDO_PLAZOS WHERE (@VALOR_DEUDA >= APL_DESDE   AND @VALOR_DEUDA <= APL_HASTA )"
        Plazo.Connection = Conexion

        'ACUERDO PORCENTAJE

        Porcentaje.CommandText = "SELECT APP_DESDE,APP_HASTA, APP_PORCENTAJE FROM ACUERDO_PORCENTAJE WHERE (@VALOR_DEUDA >= APP_DESDE   AND @VALOR_DEUDA <= APP_HASTA )"
        Porcentaje.Connection = Conexion

        'Update_AP

        Update_AP.CommandText = "UPDATE MAESTRO_CONSECUTIVOS SET CON_USER = CON_USER + 1 WHERE CON_IDENTIFICADOR = 13"
        Update_AP.Connection = Conexion

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'Se deshabilitan los campos para los usuarios con perfil Verificador de Pagos
            DeshabilitarCampos()

            ultima_tasas()
            LimpiarAll()
            TxtNroProceso.Text = Request("pExpediente")
            llenarDropDownListCuotas(60, TxtNCuotas)
            llenarDropDownList(100, txtPorcentajeCuotaini)
            txtPorcentajeCuotaini.SelectedValue = "30"
            LlenarGrid()
        End If
    End Sub

    Private Sub DeshabilitarCampos()
        If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            'Se inhabilitan los campos para este perfil
            btnNuevo.Enabled = False
            cmdCalcularInteres.Enabled = False
            btndescargar.Enabled = False
            btnGuardar.Enabled = False
            cboCAPORTE.Enabled = False
            txtfechaPago.Enabled = False
            TxtNCuotas.Enabled = False
            txtPorcentajeCuotaini.Enabled = False
            chkexcluircuota.Enabled = False
            Txt_solicitante.Enabled = False
            TxtNom_Solicitante.Enabled = False
            CmbSolicitante.Enabled = False
            TxtGarante.Enabled = False
            TxtNom_garante.Enabled = False
            CmbGarante.Enabled = False
            TxtDescripcionGarantia.Enabled = False
        Else
            'Se habilitan los campos para usuarios con perfil diferente a Verificador de Pagos
            btnNuevo.Enabled = True
            cmdCalcularInteres.Enabled = True
            btndescargar.Enabled = True
            btnGuardar.Enabled = True
            cboCAPORTE.Enabled = True
            txtfechaPago.Enabled = True
            TxtNCuotas.Enabled = True
            txtPorcentajeCuotaini.Enabled = True
            chkexcluircuota.Enabled = True
            Txt_solicitante.Enabled = True
            TxtNom_Solicitante.Enabled = True
            CmbSolicitante.Enabled = True
            TxtGarante.Enabled = True
            TxtNom_garante.Enabled = True
            CmbGarante.Enabled = True
            TxtDescripcionGarantia.Enabled = True
        End If
    End Sub

    Protected Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click

        Try

            lkbdescargar.Text = ""
            If TipoTitulo(TxtNroProceso.Text.Trim()) Then
                realizarCalculos()
            Else
                lblError.Text = "El título asociado al expediente no es una LIQUIDACIÓN OFICIAL, INFORME DE FISCALIZACIÓN Ó REQUERIMIENTO PARA DECLARAR Y/O CORREGIR..."
            End If

        Catch ex As Exception
            lblError.Text = "Error: " & ex.ToString()

        End Try

    End Sub

    Private Sub realizarCalculos()

        If ValidarTextos() = True Then
            If _DatosDetalles(Request("pExpediente")) Then

                LimpiarAll(False)


                Dim LoadTableDetalle As DataTable = CType(LoadDatosDetalles, DataTable)


                If Not IsDate(Left(txtfechaPago.Text, 10)) Then
                    lblError.Text = "La fecha de pago no es valida, verifique por favor..."
                    Exit Sub
                End If

                Dim datosInteresesDetallado As DataTable = _Proyectar_Calcular_Intereses(Left(txtfechaPago.Text, 10), CInt(TxtNCuotas.SelectedValue), LoadTableDetalle)

                Dim LoadTable As DataTable = _LoadDatos(Request("pExpediente"))
                txttotalperiodos.Text = LoadTable.Rows.Count

                If chkexcluircuota.Checked Then
                    DtgAcuerdos.DataSource = _DividirCuotasRestantes(LoadTable, CInt(TxtNCuotas.SelectedValue), LoadTableDetalle)

                    txtperiodocuotainicial.Text = 0
                    txtperiodosrestante.Text = LoadTable.Rows.Count
                    txtCuotaInicial.Text = 0
                    txtInteresCuotaInicial.Text = 0
                    txtPorcentajeideal.Text = 0
                Else
                    Dim periodosrestantes As DataTable = _CuotaInicial(LoadTable, CInt(TxtNCuotas.SelectedValue), CInt(txtPorcentajeCuotaini.Text), datosInteresesDetallado)

                    DtgAcuerdos.DataSource = _DividirCuotasRestantes(periodosrestantes, CInt(TxtNCuotas.SelectedValue), LoadTableDetalle)
                End If



                DtgAcuerdos.DataBind()

            Else
                lblError.Text = "No se encontro una planilla asociada a el expediente " & Request("pExpediente")
            End If
        Else
            lblError.Text = "Por favor, digite los campos requeridos..."
        End If

    End Sub

    Private Function _CuotaInicial(ByVal datos As DataTable, ByVal nrocuotas As Integer, ByVal porcentajecuota As Integer, ByVal _TablaDetalle As DataTable) As DataTable

        'Dim datos As DataTable = CType(ViewState("LoadDatos"), DataTable)
        Dim datoscuotas As New DataTable
        datoscuotas = datos.Copy()

        Dim capital As Double = datos.Compute("SUM(VALORDEUDA)", String.Empty)
        TotalDeudaSinintereses = capital

        Dim periodoCuotainicial As Integer = 0
        Dim cuotainicial As Double = capital * (porcentajecuota / 100)
        Dim cuotainicialideal As Double = 0
        Dim interes As Double = 0


        If datos.Rows.Count > 0 Then
            If capital > 0 Then
                For Each x As DataRow In datos.Rows
                    'Registros restante al establecer el nuero de cuotas a pagar
                    datoscuotas.Rows.RemoveAt(0)

                    periodoCuotainicial += 1
                    cuotainicialideal += x("VALORDEUDA")
                    interes += x("INTERESES")

                    For Each Z As DataRow In _TablaDetalle.Select("ANNO =" & x("ANNO") & " AND MES = " & x("MES"))
                        Z("NRO_CUOTA") = 0
                    Next

                    If cuotainicialideal >= cuotainicial Then
                        For Each Z As DataRow In _TablaDetalle.Select("NRO_CUOTA =0")
                            Z("FECHA_CUOTA") = Left(txtfechaPago.Text, 10)
                            Z("VALOR_CUOTA") = RedondearUnidades(-2, cuotainicialideal)
                        Next
                        Exit For
                    End If
                Next
                txtperiodocuotainicial.Text = periodoCuotainicial
                txtperiodosrestante.Text = datoscuotas.Rows.Count
                txtCuotaInicial.Text = RedondearUnidades(-2, cuotainicialideal)
                txtCuotaInicial.Text = FormatCurrency(txtCuotaInicial.Text, 0, TriState.True)
                txtInteresCuotaInicial.Text = interes
                txtInteresCuotaInicial.Text = FormatCurrency(txtInteresCuotaInicial.Text, 0, TriState.True)
                txtPorcentajeideal.Text = Math.Round(((cuotainicialideal / capital)), 2) * 100

            Else
                lblError.Text = "El expediente no presenta deuda."
            End If
        Else
            lblError.Text = "Por favor cargue la planilla del aportante a proyectar."
        End If

        Return datoscuotas

    End Function

    Private Function _Proyectar_Calcular_Intereses(ByVal fechaPago As Date, ByVal nroCuotas As Integer, ByVal TablaDetalles As DataTable) As DataTable

        If TablaDetalles.Rows.Count > 0 Then


            Dim cn As New SqlConnection(Funciones.CadenaConexion)
            cn.Open()
            Dim _trans As SqlTransaction
            _trans = cn.BeginTransaction

            Try

                'Validar seleccion de un tipo de aportante
                If cboCAPORTE.SelectedValue <> "0" Then

                    Dim nitcc As String = Request("pNit") ' nit o cedula de deudor
                    Dim diaPago As String = lbldiaPago.Text

                    deleteTemp(Request("pExpediente"), _trans, cn)
                    For Each row As DataRow In TablaDetalles.Rows ' recorro la grilla para determinar los intereses por cada ajuste DETALLADO
                        Dim deudaCapital As Double = CDbl(row("AJUSTE"))
                        Dim fechaRealPago As Date = CDate(fechaPago.AddMonths(nroCuotas)).ToString("dd/MM/yyyy")
                        Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, row("ANNO"), row("MES"), cboCAPORTE.SelectedValue, row("SUBSISTEMA")).ToString("dd/MM/yyyy")
                        'Alterar grid colocar datos faltantes
                        row("INTERESES") = FuncionesInteresesParafiscales._CalcularIntereses(CDbl(row("AJUSTE")), fechaExigibilidad.ToString("dd/MM/yyyy"), fechaRealPago.ToString("dd/MM/yyyy"), CDec(ViewState("trimestral")))
                        row("TOTAL_PAGAR") = row("INTERESES") + deudaCapital
                        row("FECHA_EXIGIBILIDAD") = fechaExigibilidad
                        row("FECHA_PAGO") = fechaPago
                        Save_temp_Planilla(row("SUBSISTEMA"), row("NIT_EMPRESA"), row("RAZON_SOCIAL"), row("INF"), row("ANNO"), row("MES"), row("CEDULA"), row("NOMBRE"), row("IBC"), row("AJUSTE"), row("EXPEDIENTE"), row("INTERESES"), row("FECHA_PAGO"), row("FECHA_EXIGIBILIDAD"), "0", diaPago, "0", row("TOTAL_PAGAR"), _trans, cn)
                    Next

                    _trans.Commit()
                    lblError.Text = "Proceso terminado satisfactoriamente.."
                Else
                    lblError.Text = "Por favor, Seleccione el tipo de aportante"
                    lblError.Visible = True
                End If
            Catch ex As Exception
                _trans.Rollback()
                cn.Close()
                lblError.Text = "Error: " & ex.ToString
                lblError.Visible = True
            Finally
                cn.Close()
            End Try
        Else
            lblError.Text = "No se encontraron registros a procesar..."
            lblError.Visible = True
        End If


        Return TablaDetalles
    End Function

    Private Function _DividirCuotasRestantes(ByVal datoscuotas As DataTable, ByVal nrocuotasExacta As Integer, ByVal datosDetalle As DataTable) As Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
        ' Dim TablaDetalle As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
        TablaDetalle = New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable

        Dim nrocuotas As Integer = nrocuotasExacta
        Dim tableCuotas As New DataTable
        If datoscuotas.Rows.Count >= nrocuotasExacta Then
            Dim cuota As Double = (datoscuotas.Compute("SUM(VALORDEUDA)", String.Empty))
            DisminuirDeuda = cuota
            Dim dividircuotas As Double = 0

            Dim aculacuotas As Double = 0
            Dim aculaInteres As Double = 0
            Dim aculaPeriodo As Double = 0
            Dim contarcuotas As Integer = 0
            Dim fechaPago As Date

            For a As Integer = 1 To datoscuotas.Rows.Count
                If a = 1 Then
                    dividircuotas = (cuota / nrocuotas)
                    aculacuotas = 0
                    contarcuotas = 0
                    aculaInteres = 0
                    aculaPeriodo = 0
                End If

                aculacuotas += CDbl(datoscuotas.Rows(a - 1).Item("VALORDEUDA"))
                aculaInteres += CDbl(datoscuotas.Rows(a - 1).Item("INTERESES"))
                aculaPeriodo += 1

                For Each Z As DataRow In datosDetalle.Select("ANNO =" & datoscuotas.Rows(a - 1).Item("ANNO") & " AND MES = " & datoscuotas.Rows(a - 1).Item("MES"))
                    Z("VALOR_CUOTA") = "0"
                Next

                'Cerrar el número del periodos
                If (nrocuotasExacta = datoscuotas.Rows.Count) Then
                    'Agregar registro a la tabla y reiniciar contadores
                    contarcuotas += 1
                    fechaPago = Liquidar(TablaDetalle, contarcuotas, aculaPeriodo, aculacuotas, aculaInteres, Left(txtfechaPago.Text, 10), contarcuotas)
                    For Each Z As DataRow In datosDetalle.Select("VALOR_CUOTA ='0'")
                        Z("NRO_CUOTA") = contarcuotas
                        Z("FECHA_CUOTA") = fechaPago
                        Z("VALOR_CUOTA") = RedondearUnidades(-2, aculacuotas)
                    Next
                    aculacuotas = 0
                    aculaInteres = 0
                    aculaPeriodo = 0
                ElseIf aculacuotas >= dividircuotas Then
                    'Agregar registro a la tabla y reiniciar contadores
                    contarcuotas += 1
                    fechaPago = Liquidar(TablaDetalle, contarcuotas, aculaPeriodo, aculacuotas, aculaInteres, Left(txtfechaPago.Text, 10), contarcuotas)
                    For Each Z As DataRow In datosDetalle.Select("VALOR_CUOTA ='0'")
                        Z("NRO_CUOTA") = contarcuotas
                        Z("FECHA_CUOTA") = fechaPago
                        Z("VALOR_CUOTA") = RedondearUnidades(-2, aculacuotas)
                    Next
                    aculacuotas = 0
                    aculaInteres = 0
                    aculaPeriodo = 0

                End If

                If a = datoscuotas.Rows.Count Then
                    'Validar ultimo registro, agregar registro a la tabla y reiniciar contadores
                    contarcuotas += 1

                    If (nrocuotas <> datoscuotas.Rows.Count) Then
                        fechaPago = Liquidar(TablaDetalle, contarcuotas, aculaPeriodo, aculacuotas, aculaInteres, Left(txtfechaPago.Text, 10), contarcuotas)
                        For Each Z As DataRow In datosDetalle.Select("VALOR_CUOTA ='0'")
                            Z("NRO_CUOTA") = contarcuotas
                            Z("FECHA_CUOTA") = fechaPago
                            Z("VALOR_CUOTA") = RedondearUnidades(-2, aculacuotas)
                        Next
                        aculacuotas = 0
                        aculaInteres = 0
                        aculaPeriodo = 0
                    Else
                        fechaPago = excluirSabadoDomingo(Left(txtfechaPago.Text, 10), contarcuotas)
                        For Each Z As DataRow In datosDetalle.Select("VALOR_CUOTA ='0'")
                            Z("NRO_CUOTA") = contarcuotas
                            Z("FECHA_CUOTA") = fechaPago
                            Z("VALOR_CUOTA") = RedondearUnidades(-2, aculacuotas)
                        Next
                        aculacuotas = 0
                        aculaInteres = 0
                        aculaPeriodo = 0
                    End If
                    If (contarcuotas >= nrocuotasExacta) Or (nrocuotas > nrocuotasExacta) Then
                        DetallesCuotas = datosDetalle
                        Exit For
                    Else
                        nrocuotas += 1
                        a = 0
                        DisminuirDeuda = cuota
                        TablaDetalle = New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
                        DetallesCuotas = New DataTable

                    End If
                End If
            Next
        Else
            lblError.Text = "El número de cuotas es superior al número de periodos que adeuda el aportante."
        End If
        DisminuirDeuda = 0
        Return TablaDetalle

    End Function

    Sub mostrarMensaje(ByVal sMensaje As String)
        Dim script As String = String.Format("alert('{0}');", sMensaje)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Page_Load", script, True)
    End Sub

    Private Sub deleteTemp(ByVal EXPEDIENTE As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)
        Dim command As New SqlCommand

        Dim sql As String = "DELETE FROM TEMP_PROYECCION WHERE EXPEDIENTE = @EXPEDIENTE "
        command = New SqlCommand(sql, cn, _trans)
        command.Parameters.AddWithValue("@EXPEDIENTE", EXPEDIENTE)
        command.ExecuteNonQuery()


    End Sub

    Private Sub Save_temp_Planilla(ByVal SUBSISTEMA As String, ByVal NIT_EMPRESA As String, ByVal RAZON_SOCIAL As String, ByVal INF As String, ByVal ANNO As Integer, ByVal MES As Integer, ByVal CEDULA As String, ByVal NOMBRE As String, ByVal IBC As Double, ByVal AJUSTE As Double, ByVal EXPEDIENTE As String, ByVal INTERESES As Double, ByVal FECHA_PAGO As Date, ByVal FECHA_EXIGIBILIDAD As Date, ByVal ID_CALCULO As Integer, ByVal DIA_HABIL_PAGO As Integer, ByVal LIQ_REC As Integer, ByVal TOTAL_PAGAR As Double, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)
        Dim _sql As String = "INSERT INTO TEMP_PROYECCION (EXPEDIENTE,LIQ_REC,INF,SUBSISTEMA,NIT_EMPRESA,RAZON_SOCIAL,ANNO,MES,CEDULA,NOMBRE,IBC,AJUSTE,FECHA_PAGO,INTERESES_NORMAL,TOTAL_PAGAR_NORMAL,FECHA_EXIGIBILIDAD,ID_CALCULO,DIA_HABIL_PAGO,FECHA_SYS) " &
                            " VALUES (@EXPEDIENTE,@LIQ_REC,@INF,@SUBSISTEMA,@NIT_EMPRESA,@RAZON_SOCIAL,@ANNO,@MES,@CEDULA,@NOMBRE,@IBC,@AJUSTE,@FECHA_PAGO,@INTERESES_NORMAL,@TOTAL_PAGAR_NORMAL,@FECHA_EXIGIBILIDAD,@ID_CALCULO,@DIA_HABIL_PAGO,@FECHA_SYS) "

        Dim command As New SqlCommand

        command = New SqlCommand(_sql, cn, _trans)
        command.Parameters.AddWithValue("@SUBSISTEMA", SUBSISTEMA)
        command.Parameters.AddWithValue("@NIT_EMPRESA", NIT_EMPRESA)
        command.Parameters.AddWithValue("@RAZON_SOCIAL", RAZON_SOCIAL)
        command.Parameters.AddWithValue("@INF", INF)
        command.Parameters.AddWithValue("@ANNO", ANNO)
        command.Parameters.AddWithValue("@MES", MES)
        command.Parameters.AddWithValue("@CEDULA", CEDULA)
        command.Parameters.AddWithValue("@NOMBRE", NOMBRE)
        command.Parameters.AddWithValue("@IBC", IBC)
        command.Parameters.AddWithValue("@AJUSTE", AJUSTE)
        command.Parameters.AddWithValue("@EXPEDIENTE", EXPEDIENTE)
        command.Parameters.AddWithValue("@INTERESES_NORMAL", INTERESES)
        command.Parameters.AddWithValue("@FECHA_PAGO", FECHA_PAGO)
        command.Parameters.AddWithValue("@FECHA_EXIGIBILIDAD", FECHA_EXIGIBILIDAD)
        command.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        command.Parameters.AddWithValue("@DIA_HABIL_PAGO", DIA_HABIL_PAGO)
        command.Parameters.AddWithValue("@LIQ_REC", LIQ_REC)
        command.Parameters.AddWithValue("@TOTAL_PAGAR_NORMAL", TOTAL_PAGAR)
        command.Parameters.AddWithValue("@FECHA_SYS", Now())
        command.ExecuteNonQuery()

    End Sub

    Private Sub Save_LogCalculo(ByVal ID_CALCULO As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

        Dim command As New SqlCommand("save_LogCalculoInteresParafiscales", cn, _trans)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        command.ExecuteNonQuery()

    End Sub

    Private Function LoadConsecutivoInteres(ByVal cn As SqlConnection, ByVal _trans As SqlTransaction) As Integer
        Dim Table As New DataTable
        'Consultar consecutivo
        Dim _Adapter As New SqlDataAdapter("SELECT * FROM MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 16", cn)
        _Adapter.SelectCommand.Transaction = _trans
        _Adapter.Fill(Table)
        Dim Con As Integer
        Con = Table.Rows(0).Item("CON_USER") + 1

        'Actualizar Consultar consecutivo
        _Adapter = New SqlDataAdapter("UPDATE MAESTRO_CONSECUTIVOS SET CON_USER = CON_USER + 1 WHERE CON_IDENTIFICADOR = 16", cn)
        _Adapter.SelectCommand.Transaction = _trans
        _Adapter.SelectCommand.ExecuteNonQuery()

        Return Con

    End Function

    Protected Sub cboCAPORTE_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboCAPORTE.SelectedIndexChanged
        Dim nitcc As String = Request("pNit")
        lbldiaPago.Text = FuncionesInteresesParafiscales.ObtenerDiaPago(nitcc, cboCAPORTE.SelectedValue) ' dia de pago del deudor
    End Sub

    Public Function RoundI(ByVal x As Double, Optional ByVal d As Integer = 0) As Double
        Dim m As Double
        m = 10 ^ d
        If x < 0 Then
            RoundI = Fix(x * m - 0.5) / m
        Else
            RoundI = Fix(x * m + 0.5) / m
        End If
    End Function

    Private Function RedondearUnidades(ByVal pUnidad As Integer, ByVal pValor As Double) As Double
        Dim Unidad As String = pUnidad
        Dim resultado As Double = CDbl(pValor)
        Select Case Unidad
            Case 4 'DiezMilesima
                resultado = RoundI(CDbl(pValor), 4)
            Case 3 'Milesima
                resultado = RoundI(CDbl(pValor), 3)
            Case 2 'Centesima
                resultado = RoundI(CDbl(pValor), 2)
            Case 1 'Decima
                resultado = RoundI(CDbl(pValor), 1)
            Case 0 'Unidad
                resultado = RoundI(CDbl(pValor), 0)
            Case -1 'Decena
                resultado = RoundI(CDbl(pValor), -1)
            Case -2 'Centena
                resultado = RoundI(CDbl(pValor), -2)
            Case -3 'Mil
                resultado = RoundI(CDbl(pValor), -3)
            Case -4 'Diez Mil
                resultado = RoundI(CDbl(pValor), -4)
        End Select
        Return resultado
    End Function

    Protected Sub cmdExportarExcel_Click(ByVal sender As Object, ByVal e As EventArgs)

        If DtgAcuerdos.Rows.Count > 0 Then
            Dim sb As StringBuilder = New StringBuilder()
            Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
            Dim Page As Page = New Page()
            Dim form As HtmlForm = New HtmlForm()
            Me.DtgAcuerdos.EnableViewState = False
            Page.EnableEventValidation = False
            Page.DesignerInitialize()
            Page.Controls.Add(form)
            form.Controls.Add(Me.DtgAcuerdos)
            Page.RenderControl(htw)
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=data.xls")
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(sb.ToString())
            Response.End()
        Else
            lblError.Text = "No se encontraron datos a exportar "
        End If
    End Sub

    Private Sub ultima_tasas()
        Dim table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT TOP 1 * FROM dbo.CALCULO_INTERESES_PARAFISCALES where p_trimestral > 0 ORDER BY  CONSEC DESC", Funciones.CadenaConexion)
        _Adap.Fill(table)

        ViewState("diaria") = table.Rows(0).Item(4)
        ViewState("trimestral") = table.Rows(0).Item(1)
    End Sub

    Private Function _LoadDatos(ByVal expediente As String) As DataTable
        Dim _Adap As New SqlDataAdapter
        Dim _tabla As New DataTable
        Dim sqlAportante As String = "SELECT EXPEDIENTE , ANNO,MES, SUM(AJUSTE) AS VALORDEUDA, SUM(INTERESES_NORMAL) AS INTERESES FROM TEMP_PROYECCION WHERE EXPEDIENTE = @EXPEDIENTE GROUP BY ANNO, MES,EXPEDIENTE ORDER BY ANNO,MES"

        _Adap = New SqlDataAdapter(sqlAportante, Funciones.CadenaConexion)
        _Adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
        _Adap.Fill(_tabla)

        Return _tabla
    End Function

    Private Function Liquidar(ByVal TableDetalle As Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable, ByVal NRO_CUOTA As String, ByVal PERIODOS As String, ByVal CAPITAL As Double, ByVal INTERES As Double, ByVal fecha_cuota As Date, ByVal addmes As Integer) As Date

        With Me

            DisminuirDeuda = DisminuirDeuda - CAPITAL

            Dim rows As Reportes_Admistratiivos.DETALLES_ACUERDO_PAGORow

            Dim FechaCuota As Date, FechaReal As Date
            Dim CuotaFin As Integer = 0
            FechaCuota = CDate(fecha_cuota)
            rows = TableDetalle.NewDETALLES_ACUERDO_PAGORow


            FechaReal = DateAdd(DateInterval.Month, addmes, FechaCuota)

            If FechaReal.DayOfWeek = DayOfWeek.Saturday Then
                FechaReal = DateAdd(DateInterval.Day, 2, FechaReal)
            ElseIf FechaReal.DayOfWeek = DayOfWeek.Sunday Then
                FechaReal = DateAdd(DateInterval.Day, 1, FechaReal)
            End If
            rows.CUOTA_NUMERO = NRO_CUOTA : rows.PERIODO = PERIODOS : rows.FECHA_CUOTA = CDate(FechaReal).ToString("dd/MM/yyyy") : rows.VALOR_CUOTA = CAPITAL : rows.SALDO_CAPITAL = DisminuirDeuda : rows.DOCUMENTO = TxtNumAcuerdo.Text : rows.VALOR_PAGADO = 0
            TableDetalle.AddDETALLES_ACUERDO_PAGORow(rows)

            Return FechaReal

        End With
    End Function

    Private Function _DatosDetalles(ByVal expediente As String) As Boolean
        Dim sw As Boolean = False
        Try
            Dim table As DataTable = New DataTable()
            Dim _Adap As New SqlDataAdapter

            Dim sql As String = "SELECT A.EXPEDIENTE,A.LIQ_REC,A.INF,A.SUBSISTEMA,A.NIT_EMPRESA,A.RAZON_SOCIAL, A.ANNO, A.MES ,A.CEDULA,A.NOMBRE,A.IBC,(A.AJUSTE - A.MONTO_PAGO) AS AJUSTE  ,A.FECHA_PAGO, 0  AS INTERESES,0 AS TOTAL_PAGAR, CONVERT( DATE,'01/01/1990') AS FECHA_EXIGIBILIDAD FROM SQL_PLANILLA A WHERE EXPEDIENTE = @EXPEDIENTE ORDER BY ANNO,MES"

            _Adap = New SqlDataAdapter(sql, Funciones.CadenaConexion)

            _Adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            _Adap.Fill(table)
            If table.Rows.Count > 0 Then
                ' declarar DataColumn y DataRow variables. 
                Dim column As DataColumn

                'Agregar Columna de interes
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.Int32")
                column.ColumnName = "NRO_CUOTA"
                table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = System.Type.GetType("System.Int32")
                column.ColumnName = "VALOR_CUOTA"
                table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = System.Type.GetType("System.DateTime")
                column.ColumnName = "FECHA_CUOTA"
                table.Columns.Add(column)

                LoadDatosDetalles = table

                sw = True

            Else
                lblError.Text = "No se encontro una planilla asociada a el expediente " & expediente
            End If
        Catch ex As Exception
            lblError.Text = "Error: " & ex.ToString
        End Try
        Return sw
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

    Private Sub LimpiarAll(Optional ByVal sw As Boolean = True)

        If sw Then
            txtCuotaInicial.Text = "0"
            txtfechaPago.Text = ""
            txtInteresCuotaInicial.Text = "0"
            txtperiodocuotainicial.Text = "0"
            txtperiodosrestante.Text = "0"
            txtPorcentajeideal.Text = "0"
            txttotalperiodos.Text = "0"
            cboCAPORTE.SelectedValue = "0"
            lbldiaPago.Text = ""
            TxtNom_Solicitante.Text = ""
            Txt_solicitante.Text = ""
            TxtNom_garante.Text = ""
            TxtGarante.Text = ""
            TxtDescripcionGarantia.Text = ""
            NewDocument()
            TablaDetalle = New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
            rowDetalle = New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
            LlenarGrid()
            lblError.Text = ""
            TxtNCuotas.SelectedValue = "2"
            txtPorcentajeCuotaini.SelectedValue = "30"
            LoadDatosDetalles = New DataTable
            DetallesCuotas = New DataTable
            lkbdescargar.Text = ""
        Else
            DtgAcuerdos.DataSource = Nothing
        End If



    End Sub

    Private Function ValidarTextos() As Boolean
        Dim sw As Boolean = False
        If txtfechaPago.Text = "" Then
            sw = False
        ElseIf TxtNCuotas.SelectedValue = "0" Then
            sw = False
        ElseIf cboCAPORTE.SelectedValue = "00" Then
            sw = False
        ElseIf Txt_solicitante.Text.Trim = "" Then
            sw = False
        ElseIf TxtNom_Solicitante.Text.Trim = "" Then
            sw = False
        Else
            sw = True
        End If


        Return sw

    End Function

    Private Function excluirSabadoDomingo(ByVal fecha_cuota As Date, ByVal addmes As Integer) As Date

        Dim FechaCuota As Date, FechaReal As Date
        FechaCuota = CDate(fecha_cuota)

        FechaReal = DateAdd(DateInterval.Month, addmes, FechaCuota)

        If FechaReal.DayOfWeek = DayOfWeek.Saturday Then
            FechaReal = DateAdd(DateInterval.Day, 2, FechaReal)
        ElseIf FechaReal.DayOfWeek = DayOfWeek.Sunday Then
            FechaReal = DateAdd(DateInterval.Day, 1, FechaReal)
        End If
        Return FechaReal
    End Function

    Protected Sub btndescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btndescargar.Click

        If DetallesCuotas IsNot Nothing Then
            If DetallesCuotas.Rows.Count > 0 Then
                descargafichero(DetallesCuotas, "informacionproyectadoexpediente" & Request("pExpediente") & ".xls", True)
            Else
                lblError.Text = "Error: no se detecto registros a exportar..."
            End If
        Else

        End If

    End Sub

    Private Sub NewDocument()
        With Me
            Dim Table As New DataTable
            _Adapter.SelectCommand = .Documento
            _Adapter.Fill(Table)
            Dim Con As Integer
            Con = Table.Rows(0).Item("CON_USER") + 1
            .TxtNumAcuerdo.Text = Format(Con, "0000")
        End With
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        LimpiarAll(True)
    End Sub

    Private Sub _SaveMaestro(ByVal Mytrans As SqlTransaction, ByVal con As SqlConnection, ByVal expediente As String)

        Dim table As Reportes_Admistratiivos.MAESTRO_ACUERDOSDataTable
        Dim adap As New Reportes_AdmistratiivosTableAdapters.MAESTRO_ACUERDOSTableAdapter
        Dim rows As Reportes_Admistratiivos.MAESTRO_ACUERDOSRow
        adap.Transaction = Mytrans
        adap.Connection = con



        table = adap.GetDataDocumento(TxtNumAcuerdo.Text)


        rows = table.NewMAESTRO_ACUERDOSRow

        Dim FechaInicio As Date = Left(txtfechaPago.Text, 10)

        rows.DOCUMENTO = TxtNumAcuerdo.Text
        rows.EXPEDIENTE = TxtNroProceso.Text
        rows.FECHA_INICIO = FechaInicio
        rows.FECHA_SISTEMA = Date.Now
        rows.ESTADO = "I"
        rows.USERID = Session("sscodigousuario")
        rows.ID_SOLICITANTE = Txt_solicitante.Text
        rows.NOM_SOLICITANTE = TxtNom_Solicitante.Text
        If Not TxtGarante.Text = "" Then
            rows.ID_GARANTE = TxtGarante.Text
            If TxtNom_garante.Text = "" Or TxtDescripcionGarantia.Text = "" Or CmbGarante.SelectedValue = "00" Then
                lblError.Text = "Por favor complete la informacion del garanate.. "
            Else
                rows.NOM_GARANTE = TxtNom_garante.Text
            End If
        Else
            rows.SetID_GARANTENull()
            rows.SetNOM_GARANTENull()
        End If

        rows.TP_GARANTE = CmbGarante.SelectedValue
        rows.DESC_GARANTIA = TxtDescripcionGarantia.Text
        rows.TP_SOLICITANTE = CmbSolicitante.SelectedValue()
        rows.CUOTA_INI = Decimal.Round(CDbl(txtCuotaInicial.Text))
        rows.TOTAL_DEUDA = TotalDeudaSinintereses
        rows.FECHA_EXIGIBILIDAD = Nothing

        If table.Rows.Count = 0 Then
            table.AddMAESTRO_ACUERDOSRow(rows)
        End If
        adap.Update(table)
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        lkbdescargar.Text = ""
        If TipoTitulo(TxtNroProceso.Text.Trim()) Then
            Try
                realizarCalculos()
            Catch ex As Exception
                lblError.Text = "Error: " & ex.ToString
                Exit Sub
            End Try


            If TablaDetalle.Rows.Count <> 0 Then
                If ValidarAcuerdo(TxtNroProceso.Text.Trim) = False Then

                    Dim dstAcuerdo As New Reportes_Admistratiivos
                    Dim adapDetalle As New Reportes_AdmistratiivosTableAdapters.DETALLES_ACUERDO_PAGOTableAdapter

                    Dim Transacc As SqlTransaction
                    Conexion.Open()
                    Transacc = Conexion.BeginTransaction
                    '_SaveMaestro(Transacc, Conexion, tbDatosGen.Rows(0).Item("MAN_EXPEDIENTE"))
                    _SaveMaestro(Transacc, Conexion, TxtNroProceso.Text.Trim)

                    'realizarCalculos()

                    Try
                        'Guardar en la tabla de detalle
                        adapDetalle.Transaction = Transacc
                        adapDetalle.Connection = Conexion

                        'Eliminar detalles
                        adapDetalle.Delete(TxtNumAcuerdo.Text)

                        'Nuevo consecutivo de documento
                        Update_AP.Transaction = Transacc
                        Update_AP.ExecuteNonQuery()

                        adapDetalle.Update(TablaDetalle)

                        _SaveDetalladoCuotas(Transacc, Conexion, DetallesCuotas)

                        '22-05-2014 actualizar estado del expediente.
                        ActualizarEstadoProceso(TxtNroProceso.Text.Trim, Conexion, Transacc)
                        'Fin

                        Transacc.Commit()

                        lblError.Text = "Acuerdo de pago número " & TxtNumAcuerdo.Text & " realizado satisfactoriamente.."

                        lkbdescargar.Text = "Click aqui para descargar la <b>RESOLUCIÓN<b/>"


                    Catch ex As Exception
                        Transacc.Rollback()
                        lblError.Text = "Error " & ex.Message
                    Finally
                        Conexion.Close()
                    End Try

                Else
                    lblError.Text = "El expediente " & TxtNroProceso.Text.Trim & " tiene un acuerdo de pago vigente, solo se pueden realizar proyeccion de la deuda."
                    lkbdescargar.Text = "Click aqui para descargar la <b>RESOLUCIÓN<b/>"
                End If
            Else
                lblError.Text = "Por favor realize la proyección de la deuda..."
            End If
        Else
            lblError.Text = "El título asociado al expediente no es una liquidación oficial..."
        End If

    End Sub

    Private Sub _SaveDetalladoCuotas(ByVal Mytrans As SqlTransaction, ByVal con As SqlConnection, ByVal tbDetalle As DataTable)

        Dim _DetalleCuotas As New Reportes_Admistratiivos.DETALLE_FACILIDAD_PAGODataTable
        Dim rows As Reportes_Admistratiivos.DETALLE_FACILIDAD_PAGORow
        Dim adap As New Reportes_AdmistratiivosTableAdapters.DETALLE_FACILIDAD_PAGOTableAdapter
        adap.Transaction = Mytrans
        adap.Connection = con

        For Each x As DataRow In tbDetalle.Rows
            rows = _DetalleCuotas.NewDETALLE_FACILIDAD_PAGORow

            rows.EXPEDIENTE = x.Item("EXPEDIENTE")
            rows.LIQ_REC = x.Item("LIQ_REC")
            rows.INF = x.Item("INF")
            rows.SUBSISTEMA = x.Item("SUBSISTEMA")
            rows.NIT_EMPRESA = x.Item("NIT_EMPRESA")
            rows.RAZON_SOCIAL = x.Item("RAZON_SOCIAL")
            rows.ANNO = x.Item("ANNO")
            rows.MES = x.Item("MES")
            rows.CEDULA = CStr(x.Item("CEDULA"))
            rows.NOMBRE = x.Item("NOMBRE")
            rows.IBC = x.Item("IBC")
            rows.AJUSTE = x.Item("AJUSTE")
            rows.FECHA_PAGO = x.Item("FECHA_PAGO")
            rows.INTERESES = x.Item("INTERESES")
            rows.TOTAL_PAGAR = x.Item("TOTAL_PAGAR")
            rows.FECHA_EXIGIBILIDAD = x.Item("FECHA_EXIGIBILIDAD")
            rows.NRO_CUOTA = x.Item("NRO_CUOTA")
            rows.VALOR_CUOTA = x.Item("VALOR_CUOTA")
            rows.FECHA_CUOTA = x.Item("FECHA_CUOTA")
            rows.FECHA_SYS = Now()
            _DetalleCuotas.AddDETALLE_FACILIDAD_PAGORow(rows)
        Next
        adap.Update(_DetalleCuotas)
    End Sub

    Public Function saveResolucion(ByVal expediente As String, ByVal acto As String)
        Dim resolucion As DataTable = InsertReslucion(expediente, acto)
        If resolucion.Rows.Count > 0 Then
            Return resolucion.Rows.Item(0)(3)
        Else
            Return Nothing
        End If
    End Function

    Private Function Export(ByVal expediente As String, ByVal acto As String, ByVal resolucion As Integer) As String
        Dim worddoc As New WordReport
        Dim worddocresult As String = ""
        Dim fecha As Date
        Dim paramentros(14) As WordReport.Marcadores_Adicionales
        Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(expediente, "013", 0)

        Dim TbMandamientoPago As New DataTable
        Dim adapMandamientoPago As New SqlDataAdapter("SELECT DG_NRO_DOC AS RESOLUCION,DG_FECHA_DOC AS FECHA,DG_EXPEDIENTE AS EXPEDIENTE FROM DOCUMENTOS_GENERADOS A WHERE DG_EXPEDIENTE = @EXPEDIENTE AND DG_COD_ACTO = @ACTO", Funciones.CadenaConexion)
        adapMandamientoPago.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
        adapMandamientoPago.SelectCommand.Parameters.AddWithValue("@ACTO", "013")
        adapMandamientoPago.Fill(TbMandamientoPago)

        If dtsPlantilla.Rows.Count > 0 Then
            'Preguntar si el expediente tiene mandamiento de pago...
            If TbMandamientoPago.Rows.Count > 0 Then
                dtsPlantilla.Rows(0).Item("ACTO_ANTERIOR") = TbMandamientoPago.Rows(0).Item("RESOLUCION")
                dtsPlantilla.Rows(0).Item("FECHA_ANT") = CDate(TbMandamientoPago.Rows(0).Item("FECHA")).ToString(" dd 'de' MMMM 'de' yyy")
            Else
                dtsPlantilla.Rows(0).Item("ACTO_ANTERIOR") = "XXXXXX"
                dtsPlantilla.Rows(0).Item("FECHA_ANT") = "XXXXXX"
            End If

            paramentros(0).Marcador = "fecha_actual"
            paramentros(0).Valor = Today.ToString(" dd 'de' MMMM 'de' yyy")
            paramentros(1).Marcador = "letra"
            paramentros(1).Valor = Num2Text(dtsPlantilla.Rows.Item(0)(9))
            paramentros(2).Marcador = "FECHA_REG"
            fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(1))
            paramentros(2).Valor = fecha.ToString(" dd 'de' MMMM 'de' yyy")
            paramentros(3).Marcador = "fecha_antes"
            paramentros(3).Valor = dtsPlantilla.Rows.Item(0)("FECHA_ANT")

            'ENVIAR NUMERO DE CUOTAS 
            Dim tb As New DataTable
            Dim adap As New SqlDataAdapter("SELECT A.CUOTA_NUMERO,convert(varchar(10),A.FECHA_CUOTA,103) AS FECHA_CUOTA , CONVERT (VARCHAR(50),CONVERT (MONEY, ISNULL( A.SALDO_CAPITAL,0)),1) AS SALDO_CAPITAL, CONVERT (VARCHAR(50),CONVERT (MONEY,ISNULL( A.VALOR_CUOTA - A.VALOR_INTERES,0)),1) AS APORTE_CAPITAL,CONVERT (VARCHAR(50),CONVERT (MONEY,ISNULL( A.VALOR_INTERES,0)),1)AS APORTE_INTERES,CONVERT (VARCHAR(50),CONVERT (MONEY, ISNULL( A.VALOR_CUOTA,0)),1) AS VALOR_CUOTA FROM DETALLES_ACUERDO_PAGO A , MAESTRO_ACUERDOS B WHERE A.DOCUMENTO = B.DOCUMENTO AND B.EXPEDIENTE = @EXPEDIENTE", Funciones.CadenaConexion)
            adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            adap.Fill(tb)

            paramentros(4).Marcador = "SCuota"
            paramentros(4).Valor = Math.Round(dtsPlantilla.Rows.Item(0)("SALDO_INICIAL") / dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") * 100)

            paramentros(5).Marcador = "ncuota"
            paramentros(5).Valor = tb.Rows.Count

            paramentros(6).Marcador = "txt_cuota"
            paramentros(6).Valor = Num2Text(tb.Rows.Count)
            'Fin marcadores cuotas

            paramentros(7).Marcador = "Nro_Resolucion"
            paramentros(7).Valor = resolucion

            paramentros(8).Marcador = "saldodiferir"
            paramentros(8).Valor = String.Format("{0:C0}", dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") - dtsPlantilla.Rows.Item(0)("SALDO_INICIAL"))

            paramentros(9).Marcador = "tdeuda"
            paramentros(9).Valor = String.Format("{0:C0}", dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA"))
            paramentros(10).Marcador = "sinicial"
            paramentros(10).Valor = String.Format("{0:C0}", dtsPlantilla.Rows.Item(0)("SALDO_INICIAL"))

            ''Quien Revisa - Quien proyecta
            paramentros(11).Marcador = "qrevisa"
            paramentros(11).Valor = dtsPlantilla.Rows.Item(0)("REVISOR")

            paramentros(12).Marcador = "qproyecta"
            paramentros(12).Valor = dtsPlantilla.Rows.Item(0)("PROYECTO")

            paramentros(13).Marcador = "repLegal"
            paramentros(13).Valor = dtsPlantilla.Rows.Item(0)("NOMBRE_RL")

            paramentros(14).Marcador = "FECHA_EMISION"
            paramentros(14).Valor = getfecha_tipada(Today.Date).ToUpper




            ' Cargar consolidado de deuda por subsistema
            Dim tb1 As New DataTable
            Dim ad As New SqlDataAdapter("SELECT C.NOMBRE_GRUPO,  A.ANNO AS AÑO, B.NOMBRE AS MES, CONVERT (VARCHAR(50),CONVERT (MONEY, SUM( A.AJUSTE)),1) AS DEUDA FROM SQL_PLANILLA A , MAESTRO_MES B, GRUPOS C WHERE A.MES = B.ID_MES AND A.ID_GRUPO = C.ID_GRUPO AND A.EXPEDIENTE = @EXPEDIENTE GROUP BY C.NOMBRE_GRUPO, A.ANNO,B.NOMBRE,B.ID_MES HAVING SUM(A.AJUSTE) > 0 ORDER BY C.NOMBRE_GRUPO,A.ANNO,B.ID_MES", Funciones.CadenaConexion)
            ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            ad.Fill(tb1)


            'ENVIAR NUMERO DE CUOTAS 
            Dim tb2 As New DataTable
            ad = New SqlDataAdapter("SELECT A.CUOTA_NUMERO, CONVERT(VARCHAR(10), A.FECHA_CUOTA ,103) AS FECHA_CUOTA, CONVERT (VARCHAR(50),CONVERT (MONEY,A.SALDO_CAPITAL),1) AS SALDO_CAPITAL, A.PERIODO,CONVERT (VARCHAR(50),CONVERT (MONEY,A.VALOR_CUOTA),1)AS APORTE_CAPITAL FROM DETALLES_ACUERDO_PAGO A , MAESTRO_ACUERDOS B WHERE A.DOCUMENTO = B.DOCUMENTO AND B.EXPEDIENTE = @EXPEDIENTE", Funciones.CadenaConexion)
            ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            ad.Fill(tb2)

            worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadesPagoResolucionConcede, paramentros, tb1, 0, False, tb2, 1, False, Nothing, 0, False)
        End If


        Return worddocresult


    End Function

    Private Sub SendReport(ByVal NombreArchivo As String, ByVal Plantilla As String)
        Response.ContentType = "application/msword"
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.doc", NombreArchivo))
        Response.Write(Plantilla)
        Response.End()
    End Sub

    'Establecer número de cuotas       
    Private Sub llenarDropDownList(ByVal número As Integer, ByVal droplist As DropDownList)

        droplist.Items.Clear()
        For i As Integer = 1 To número - 1
            droplist.Items.Insert(i - 1, i - 1)
        Next

    End Sub

    Private Sub llenarDropDownListCuotas(ByVal número As Integer, ByVal droplist As DropDownList)

        droplist.Items.Clear()
        For i As Integer = 1 To número - 1
            droplist.Items.Insert(i - 1, i + 1)
        Next

    End Sub

    Private Sub LlenarGrid()
        With Me
            Dim row As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
            Dim FilasAct As Integer

            If rowDetalle Is Nothing Then
                FilasAct = 0
            Else
                row = CType(rowDetalle, Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable)
                FilasAct = row.Rows.Count
            End If

            Dim FilasAdd As Integer
            Dim FilasGrid As Integer = .DtgAcuerdos.PageSize
            Dim x As Integer
            Select Case FilasAct
                Case 0
                    FilasAdd = FilasGrid
                Case Is < FilasGrid
                    FilasAdd = FilasGrid - FilasAct
                Case Is > FilasGrid
                    Dim Residuo As Integer = FilasAct Mod FilasGrid
                    Dim PaginasGrid As Integer = Int(FilasAct / FilasGrid)
                    If Residuo <> 0 Then
                        PaginasGrid = PaginasGrid + 1
                    End If
                    FilasAdd = (PaginasGrid * FilasGrid) - FilasAct
            End Select
            For x = 1 To FilasAdd
                row.AddDETALLES_ACUERDO_PAGORow(Nothing, Nothing, Date.Today, Date.Today, Nothing, Nothing, Nothing, Nothing, Nothing, 0)
            Next
            DtgAcuerdos.DataSource = row
            .DtgAcuerdos.DataBind()
        End With
    End Sub

    Private Function ValidarAcuerdo(ByVal expediente As String) As Boolean
        Dim sw As Boolean = False
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT * FROM MAESTRO_ACUERDOS WHERE EXPEDIENTE = '" & expediente & "'")

        If tb.Rows.Count > 0 Then
            sw = True
        End If

        Return sw
    End Function

    Protected Sub chkexcluircuota_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkexcluircuota.CheckedChanged
        If chkexcluircuota.Checked Then
            cmdCalcularInteres.Text = "Reliquidar saldo"
        Else
            cmdCalcularInteres.Text = "Proyectar Cuotas"
        End If
    End Sub

    Private Sub ActualizarEstadoProceso(ByVal expediente As String, ByVal cn As SqlConnection, ByVal trans As SqlTransaction)
        Dim sw As Boolean = False
        Dim Sql As String = "UPDATE EJEFISGLOBAL  SET EFIESTADO = '05' WHERE EFINROEXP = @EXPEDIENTE "
        Dim cmd As New SqlCommand(Sql, cn, trans)
        cmd.Parameters.AddWithValue("@EXPEDIENTE", expediente)
        cmd.ExecuteNonQuery()


    End Sub

    Private Function TipoTitulo(ByVal expediente As String) As Boolean
        Dim sw As Boolean = False
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT TOP 1 c.codigo, c.nombre FROM EJEFISGLOBAL A , MAESTRO_TITULOS B, TIPOS_TITULO C  WHERE A.EFINROEXP = B.MT_expediente AND b.MT_tipo_titulo = c.codigo and EFINROEXP = '" & expediente & "'")

        If tb.Rows.Count > 0 Then
            Dim tipo As String = tb.Rows(0).Item("codigo")

            If (tipo = "01") Or (tipo = "02") Or (tipo = "04") Then
                sw = True
            End If

        End If
        Return sw
    End Function

    Protected Sub lkbdescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lkbdescargar.Click
        Dim exp As String = TxtNroProceso.Text.Trim
        Dim resolucion As Integer = saveResolucion(TxtNroProceso.Text.Trim, "331")
        Dim dowload As String = Export(exp, "331", resolucion)

        If dowload <> "" Then
            'SendReport(expediente, worddocresult)
            descargafichero(Nothing, "RESOLUCION_CONCEDE_FACILIDAD_PARAFISCALES_" & exp & "_" & Today.ToString("dd.MM.yyyy") & ".doc", True, dowload)
        Else
            lblError.Text = "No se ha encontrado información para mostrar la RESOLUCION QUE CONCEDE FACILIDAD DE PAGO"
        End If
    End Sub

End Class



