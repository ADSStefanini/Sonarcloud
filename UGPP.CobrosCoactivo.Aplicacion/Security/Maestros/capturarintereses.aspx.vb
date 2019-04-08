Imports System.Data.SqlClient
Imports System.IO

Partial Public Class capturarintereses
    Inherits System.Web.UI.Page

    Protected Shared Expxls As New DataTable
    Protected Shared DatosImportado As New DataTable


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ultima_tasas()
        End If
    End Sub

    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub

    'Calcular Intereses Parafiscales
    Private Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click

        Dim tiempoInicial As String = Now().ToLongTimeString
        Dim tiempoFinal As String = ""
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim _trans As SqlTransaction

        'Datos del sql en la db de coactivo
        Dim saldoSQlInterno As Double = 0.0
        Dim SQlInterno As DataTable = VerificarsQL(Request("pExpediente"))
        If SQlInterno.Rows.Count > 0 Then
            saldoSQlInterno = SQlInterno.Compute("SUM(AJUSTE)", String.Empty)
        End If

        'Informacion del expediente
        Dim datosExp As DataTable = DatosExpediente(Request("pExpediente"))
        If datosExp.Rows.Count = 0 Then
            lblError.Text = "El expediente no existe, por favor verificar."
            Exit Sub
        End If
        'Asignar valor fijos
        Dim expediente As String = datosExp.Rows(0).Item("EFINROEXP").ToString
        Dim capitalInicial As Double = CDbl(datosExp.Rows(0).Item("EFIVALDEU"))
        Dim nit As String = datosExp.Rows(0).Item("EFINIT").ToString


        'Traer saldo del expediente-------------------------------------------

        Dim WsEncabezado As New WSLO.opConsultarSaldoIMPLClient
        Dim WsResultadoEncabezado As New WSLO.consultarSaldo


        WsResultadoEncabezado.expediente = expediente
        WsResultadoEncabezado.nit = nit
        WsResultadoEncabezado.valor = capitalInicial
        WsResultadoEncabezado.transaccion = "2"
        WsResultadoEncabezado = WsEncabezado.oConsultarSaldoCAB(WsResultadoEncabezado)
        Dim saldo As Double = CDbl(WsResultadoEncabezado.valor) ' Obtener saldo actual de la liquidación
        'Fin--------------------------------------------------------------------
        If saldo = "-1" Then
            lblError.Text = "El expediente no está en verificación."
            Exit Sub
        End If

        If CDbl(saldo) <> CDbl(saldoSQlInterno) Then

            'Borrar el detallado del Sql en 
            'la db de ciactivo para incluir 
            'el saldo actualizado
            BorrarDetalladoSQl(expediente)
            '--------------------------------

            'Detalle del SQL
            Dim WsDetalle As New WSLO.opConsultarSaldoIMPLClient
            Dim WsRespuestaDetalle(WsResultadoEncabezado.numero_Registros) As WSLO.consultarSaldoRespuesta
            Dim WsResultadoDetalle As New WSLO.consultarSaldo
            WsResultadoDetalle.expediente = expediente
            WsResultadoDetalle.nit = nit
            WsResultadoDetalle.valor = capitalInicial
            WsResultadoDetalle.transaccion = "2"
            WsRespuestaDetalle = WsDetalle.oConsultarSaldo(WsResultadoDetalle)


            cn.Open()
            _trans = cn.BeginTransaction
            Try

                For Each i As WSLO.consultarSaldoRespuesta In WsRespuestaDetalle
                    Save_SQL(expediente, 0, i.inf, i.subsistema, WsResultadoEncabezado.nit, WsResultadoEncabezado.razon_Social, i.anio, i.mes, i.cedula, i.nombreTrabajador, i.ibc, i.ajuste, Nothing, i.tipo, _trans, cn)
                Next


                _trans.Commit()


            Catch ex As Exception
                _trans.Rollback()
                lblError.Text = "Error: " & ex.ToString
                Exit Sub
            Finally
                cn.Close()
            End Try

            'tiempoFinal = Now().ToLongTimeString
            'lblError.Text = "Ok, Tiempo Inicial " & tiempoInicial & ",  Tiempo Final " & tiempoFinal

        Else
            'tiempoFinal = Now().ToLongTimeString
            'lblError.Text = "Ok, Tiempo Inicial " & tiempoInicial & ",  Tiempo Final " & tiempoFinal
            'Exit Sub
        End If


        If VerificarsQL(Request("pExpediente")).Rows.Count = 0 Then
            lblError.Text = "No se ha destectado detalle de la deuda (SQL) asociado a este expediente, consulte con el administrador del sistema. "
            Exit Sub
        End If

        If txtFechaPago.Text = "" Then
            lblError.Text = "Por Favor digite una fecha de pago válida."
            Exit Sub

        End If

        cn.Open()
        _trans = cn.BeginTransaction

        Try

            If DatosImportado.Rows.Count > 0 Then
                'Validar seleccion de un tipo de aportante
                If cboCAPORTE.SelectedValue <> "0" Then

                    Dim consecutivoInteres As Integer = LoadConsecutivoInteres(cn, _trans) 'Consultar consecutivo de intereses 
                    Dim nitcc As String = DatosImportado.Rows(0).Item("NIT_EMPRESA").ToString ' nit o cedula de deudor
                    Dim diaPago As Integer = CInt(lbldiaPago.Text)
                    'ModificarPlanilla(DatosImportado.Rows(0).Item("EXPEDIENTE"), _trans, cn)
                    For row As Integer = 0 To DatosImportado.Rows.Count - 1 ' recorro la grilla para determinar los intereses por cada ajuste
                        Dim deudaCapital As Double = CDbl(DatosImportado.Rows(row).Item("AJUSTE"))

                        'DatosImportado.Rows(row).Item("AJUSTE") = String.Format("{0:C2}", CDbl(DatosImportado.Rows(row).Item("AJUSTE")))

                        Dim fechaPago As Date = CDate(txtFechaPago.Text).ToString("dd/MM/yyyy")
                        Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, DatosImportado.Rows(row).Item("ANNO"), DatosImportado.Rows(row).Item("MES"), cboCAPORTE.SelectedValue, DatosImportado.Rows(row).Item("SUBSISTEMA"))

                        'Alterar grid colocar datos faltantes
                        DatosImportado.Rows(row).Item("DIA_HABIL_PAGO") = diaPago
                        DatosImportado.Rows(row).Item("FECHA_EXIGIBILIDAD") = CDate(fechaExigibilidad).ToString("dd/MM/yyyy")

                        'Intereses sin redondear
                        DatosImportado.Rows(row).Item("INTERESES_NORMAL") = FuncionesInteresesParafiscales._CalcularIntereses(deudaCapital, fechaExigibilidad, fechaPago, CDec(ViewState("trimestral")))
                        'DatosImportado.Rows(row).Item("INTERESES_NORMAL") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("INTERESES_NORMAL")))


                        DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL") = deudaCapital + CDbl(DatosImportado.Rows(row).Item("INTERESES_NORMAL"))
                        'DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL")))


                        'Intereses redondeado
                        DatosImportado.Rows(row).Item("INTERESES_AJUSTADO") = RedondearUnidades(-2, CDbl(DatosImportado.Rows(row).Item("INTERESES_NORMAL")))
                        'DatosImportado.Rows(row).Item("INTERESES_AJUSTADO") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("INTERESES_AJUSTADO")))

                        DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO") = deudaCapital + CDbl(DatosImportado.Rows(row).Item("INTERESES_AJUSTADO"))
                        'DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO")))


                        'Dias de mora
                        DatosImportado.Rows(row).Item("DIAS_MORA") = DateDiff(DateInterval.Day, fechaExigibilidad, fechaPago)


                        DatosImportado.Rows(row).Item("ID_CALCULO") = consecutivoInteres
                        DatosImportado.Rows(row).Item("FECHA_SYS") = Now()

                        Save_Planilla(DatosImportado.Rows(row).Item("SUBSISTEMA"), nitcc, DatosImportado.Rows(row).Item("RAZON_SOCIAL"), DatosImportado.Rows(row).Item("INF"), DatosImportado.Rows(row).Item("ANNO"), DatosImportado.Rows(row).Item("MES"), DatosImportado.Rows(row).Item("CEDULA"), DatosImportado.Rows(row).Item("NOMBRE"), DatosImportado.Rows(row).Item("IBC"), DatosImportado.Rows(row).Item("AJUSTE"), DatosImportado.Rows(row).Item("EXPEDIENTE"), DatosImportado.Rows(row).Item("INTERESES_NORMAL"), fechaPago, fechaExigibilidad, consecutivoInteres, diaPago, DatosImportado.Rows(row).Item("LIQ_REC"), DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL"), DatosImportado.Rows(row).Item("INTERESES_AJUSTADO"), DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO"), DatosImportado.Rows(row).Item("DIAS_MORA"), _trans, cn)
                    Next
                    Expxls = Save_LogCalculo(consecutivoInteres, _trans, cn)
                    CargarTotales(consecutivoInteres, cn, _trans)
                    _trans.Commit()

                    _Gridinteres.DataSource = DatosImportado
                    _Gridinteres.DataBind()

                    tiempoFinal = Now().ToLongTimeString
                    lblError.Text = "Proceso terminado satisfactoriamente.. Tiempo Inicial: " & tiempoInicial & " Tiempo FInal: " & tiempoFinal
                Else
                    lblError.Text = "Por favor, Seleccione el tipo de aportante"
                    lblError.Visible = True
                End If
            Else
                lblError.Text = "Por favor, Seleccione el archivo .CSV a procesar y luego oprima el boton Importar... "
                lblError.Visible = True
            End If
        Catch ex As Exception
            _trans.Rollback()
            cn.Close()
            Alert("Error: " & ex.ToString)
            lblError.Text = "Por favor compruebe el archivo importado, es posible que el orden de las columnas no coinsidan con la plantilla de ejemplo o que esten campos vacios. " & tiempoFinal
            lblError.Visible = True
        Finally
            cn.Close()
        End Try


    End Sub

    Private Sub Save_Planilla(ByVal SUBSISTEMA As String, ByVal NIT_EMPRESA As String, ByVal RAZON_SOCIAL As String, ByVal INF As String, ByVal ANNO As Integer, ByVal MES As Integer, ByVal CEDULA As String, ByVal NOMBRE As String, ByVal IBC As Double, ByVal AJUSTE As Double, ByVal EXPEDIENTE As String, ByVal INTERESES As Double, ByVal FECHA_PAGO As Date, ByVal FECHA_EXIGIBILIDAD As Date, ByVal ID_CALCULO As Integer, ByVal DIA_HABIL_PAGO As Integer, ByVal LIQ_REC As Integer, ByVal TOTAL_PAGAR As Double, ByVal INTERESES_AJUSTADO As Double, ByVal TOTAL_PAGAR_AJUSTADO As Double, ByVal DIAS_MORA As Integer, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

        Dim command As New SqlCommand("Save_Planilla", cn, _trans)
        command.CommandType = CommandType.StoredProcedure
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
        command.Parameters.AddWithValue("@INTERESES_AJUSTADO", INTERESES_AJUSTADO)
        command.Parameters.AddWithValue("@TOTAL_PAGAR_AJUSTADO", TOTAL_PAGAR_AJUSTADO)
        command.Parameters.AddWithValue("@DIAS_MORA", DIAS_MORA)
        command.ExecuteNonQuery()

    End Sub

    Private Function Save_LogCalculo(ByVal ID_CALCULO As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection) As DataTable
        Dim tb As New DataTable
        Dim command As New SqlCommand("save_LogCalculoInteresParafiscales", cn, _trans)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        command.ExecuteNonQuery()
        Dim ad As New SqlDataAdapter(command)
        ad.Fill(tb)
        Return tb
    End Function

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

    Private Function CargarTotales(ByVal ID_CALCULO As Integer, ByVal cn As SqlConnection, ByVal _trans As SqlTransaction) As DataTable
        Dim Table As New DataTable
        'Consultar consecutivo
        Dim _Adapter As New SqlDataAdapter("SELECT SUBSISTEMA, SUM(AJUSTE) AS CAPITAL,SUM(INTERESES_AJUSTADO) AS INTERESES,SUM(AJUSTE +INTERESES_AJUSTADO )AS GRANTOTAL  FROM PLANILLA WHERE ID_CALCULO = @ID_CALCULO  GROUP BY SUBSISTEMA", cn)
        _Adapter.SelectCommand.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        _Adapter.SelectCommand.Transaction = _trans
        _Adapter.Fill(Table)

        'Iniciar campos en 0
        txtSaludInteres.Text = 0
        txtSaludCapital.Text = 0
        txtTotalSalud.Text = 0
        txtPensionCapital.Text = 0
        txtPensionInteres.Text = 0
        txtTotalPension.Text = 0
        txtCcfCapital.Text = 0
        txtCcfInteres.Text = 0
        txtTotalCCF.Text = 0
        txtArlCapital.Text = 0
        txtArlInteres.Text = 0
        txtTotalARL.Text = 0
        txtFspCapital.Text = 0
        txtFspInteres.Text = 0
        txtTotalFSP.Text = 0
        txtIcbfCapital.Text = 0
        txtIcbfInteres.Text = 0
        txtTotalICBF.Text = 0
        txtSenaCapital.Text = 0
        txtSenaInteres.Text = 0
        txtTotalSena.Text = 0
        'Fin

        'Calcular totales
        For Each row As DataRow In Table.Rows
            If row("SUBSISTEMA") = "SALUD" Then
                txtSaludCapital.Text = row("CAPITAL")
                txtSaludCapital.Text = FormatCurrency(txtSaludCapital.Text, 0, TriState.True) 'Formato moneda
                txtSaludInteres.Text = row("INTERESES")
                txtSaludInteres.Text = FormatCurrency(txtSaludInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalSalud.Text = row("GRANTOTAL")
                txtTotalSalud.Text = FormatCurrency(txtTotalSalud.Text, 0, TriState.True) 'Formato moneda
            End If
            If row("SUBSISTEMA") = "PENSION" Then
                txtPensionCapital.Text = row("CAPITAL")
                txtPensionCapital.Text = FormatCurrency(txtPensionCapital.Text, 0, TriState.True) 'Formato moneda
                txtPensionInteres.Text = row("INTERESES")
                txtPensionInteres.Text = FormatCurrency(txtPensionInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalPension.Text = row("GRANTOTAL")
                txtTotalPension.Text = FormatCurrency(txtTotalPension.Text, 0, TriState.True) 'Formato moneda
            End If
            If row("SUBSISTEMA") = "CCF" Then
                txtCcfCapital.Text = row("CAPITAL")
                txtCcfCapital.Text = FormatCurrency(txtCcfCapital.Text, 0, TriState.True) 'Formato moneda
                txtCcfInteres.Text = row("INTERESES")
                txtCcfInteres.Text = FormatCurrency(txtCcfInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalCCF.Text = row("GRANTOTAL")
                txtTotalCCF.Text = FormatCurrency(txtTotalCCF.Text, 0, TriState.True) 'Formato moneda
            End If

            If row("SUBSISTEMA") = "ARL" Then
                txtArlCapital.Text = row("CAPITAL")
                txtArlCapital.Text = FormatCurrency(txtArlCapital.Text, 0, TriState.True) 'Formato moneda
                txtArlInteres.Text = row("INTERESES")
                txtArlInteres.Text = FormatCurrency(txtArlInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalARL.Text = row("GRANTOTAL")
                txtTotalARL.Text = FormatCurrency(txtTotalARL.Text, 0, TriState.True) 'Formato moneda
            End If

            If row("SUBSISTEMA") = "FSP" Then
                txtFspCapital.Text = row("CAPITAL")
                txtFspCapital.Text = FormatCurrency(txtFspCapital.Text, 0, TriState.True) 'Formato moneda
                txtFspInteres.Text = row("INTERESES")
                txtFspInteres.Text = FormatCurrency(txtFspInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalFSP.Text = row("GRANTOTAL")
                txtTotalFSP.Text = FormatCurrency(txtTotalFSP.Text, 0, TriState.True) 'Formato moneda
            End If

            If row("SUBSISTEMA") = "ICBF" Then
                txtIcbfCapital.Text = row("CAPITAL")
                txtIcbfCapital.Text = FormatCurrency(txtIcbfCapital.Text, 0, TriState.True) 'Formato moneda
                txtIcbfInteres.Text = row("INTERESES")
                txtIcbfInteres.Text = FormatCurrency(txtIcbfInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalICBF.Text = row("GRANTOTAL")
                txtTotalICBF.Text = FormatCurrency(txtTotalICBF.Text, 0, TriState.True) 'Formato moneda
            End If
            If row("SUBSISTEMA") = "SENA" Then
                txtSenaCapital.Text = row("CAPITAL")
                txtSenaCapital.Text = FormatCurrency(txtSenaCapital.Text, 0, TriState.True) 'Formato moneda
                txtSenaInteres.Text = row("INTERESES")
                txtSenaInteres.Text = FormatCurrency(txtSenaInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalSena.Text = row("GRANTOTAL")
                txtTotalSena.Text = FormatCurrency(txtTotalSena.Text, 0, TriState.True) 'Formato moneda
            End If
        Next
        'Fin 
        Return Table

    End Function

    Private Sub cboCAPORTE_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboCAPORTE.SelectedIndexChanged

        Dim nitcc As String = Request("pNitCedula").ToString
        lbldiaPago.Text = FuncionesInteresesParafiscales.ObtenerDiaPago(nitcc, cboCAPORTE.SelectedValue) ' dia de pago del deudor
        
    End Sub

    Private Function RoundI(ByVal x As Double, Optional ByVal d As Integer = 0) As Double
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

    Private Sub LLenarDatos()
        Dim DataTable As DataTable = CType(ViewState("datos"), DataTable)
        Gridinteres.DataSource = DataTable
        Gridinteres.DataBind()
    End Sub

    Private Sub Gridinteres_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridinteres.PageIndexChanging
        Gridinteres.PageIndex = e.NewPageIndex
        _Gridinteres.DataSource = DatosImportado
        _Gridinteres.DataBind()
    End Sub

    Private Sub cmdExportarExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdExportarExcel.Click

        If Gridinteres.Rows.Count > 0 Then

            descargafichero(Expxls, "informacion_calculo_intereses_expediente" & Request("pExpediente") & ".xls", True)
        Else
            lblError.Text = "Error: no se detecto registros a exportar..."


            'Dim sb As StringBuilder = New StringBuilder()
            'Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
            'Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
            'Dim Page As Page = New Page()
            'Dim form As HtmlForm = New HtmlForm()
            'Me.Gridinteres.EnableViewState = False
            'Page.EnableEventValidation = False
            'Page.DesignerInitialize()
            'Page.Controls.Add(form)
            'form.Controls.Add(Me.Gridinteres)
            'Page.RenderControl(htw)
            'Response.Clear()
            'Response.Buffer = True
            'Response.ContentType = "application/vnd.ms-excel"
            'Response.AddHeader("Content-Disposition", "attachment;filename=data.xls")
            'Response.Charset = "UTF-8"
            'Response.ContentEncoding = Encoding.Default
            'Response.Write(sb.ToString())
            'Response.End()

        End If
    End Sub

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

    Private Sub ultima_tasas()
        Dim table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT TOP 1 * FROM dbo.CALCULO_INTERESES_PARAFISCALES where p_trimestral > 0 ORDER BY  CONSEC DESC", Funciones.CadenaConexion)
        _Adap.Fill(table)

        ViewState("diaria") = table.Rows(0).Item(4)
        ViewState("trimestral") = table.Rows(0).Item(1)
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

    Private Sub ModificarPlanilla(ByVal pExpediente As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)
        Dim command As New SqlCommand("DELETE FROM PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE ", cn, _trans)
        command.Parameters.AddWithValue("@EXPEDIENTE", pExpediente)
        command.ExecuteNonQuery()
    End Sub

    Public Function VerificarsQL(ByVal pExpediente As String) As DataTable

        Dim sql As String = "update SQL_PLANILLA set ANNO = 2012 WHERE EXPEDIENTE = '82299'"
        Dim cmd As New SqlCommand(sql, New SqlConnection(Funciones.CadenaConexion))
        cmd.Connection.Open()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()

        DatosImportado = New DataTable
        DatosImportado = RetornaCargadatos("SELECT EXPEDIENTE,LIQ_REC,INF,SUBSISTEMA,NIT_EMPRESA,RAZON_SOCIAL,ANNO,MES,CEDULA,NOMBRE,IBC,(AJUSTE - ISNULL(MONTO_PAGO,0) ) AJUSTE, ISNULL( ID_GRUPO,0)ID_GRUPO FROM  SQL_PLANILLA WHERE EXPEDIENTE = " & pExpediente)
        'Dim sql As String = "SELECT EXPEDIENTE,LIQ_REC,INF,SUBSISTEMA,NIT_EMPRESA,RAZON_SOCIAL,ANNO,MES,CEDULA,NOMBRE,IBC,(AJUSTE - ISNULL(MONTO_PAGO,0) ) AJUSTE, ISNULL( ID_GRUPO,0)ID_GRUPO FROM  SQL_PLANILLA WHERE EXPEDIENTE = @EXP "
        'Dim cmd As New SqlCommand(sql, New SqlConnection(Funciones.CadenaConexion))
        'cmd.Parameters.AddWithValue("@EXP", pExpediente)
        'Dim adap As New SqlDataAdapter(cmd)
        'adap.Fill(DatosImportado)

        Dim column As DataColumn
        'Agregar Columna de DIA_HABIL_PAGO
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.Int32")
        column.ColumnName = "DIA_HABIL_PAGO"
        DatosImportado.Columns.Add(column)

        'Agregar Columna de fecha de exigibilidad
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.DateTime")
        column.ColumnName = "FECHA_EXIGIBILIDAD"
        DatosImportado.Columns.Add(column)

        'Agregar Columna de interes sin ajustar
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = "INTERESES_NORMAL"
        DatosImportado.Columns.Add(column)

        'Agregar Columna de total pago sin ajustar
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = "TOTAL_PAGAR_NORMAL"
        DatosImportado.Columns.Add(column)

        'Agregar Columna de interes ajustado
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = "INTERESES_AJUSTADO"
        DatosImportado.Columns.Add(column)

        'Agregar Columna de total pago ajustado
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = "TOTAL_PAGAR_AJUSTADO"
        DatosImportado.Columns.Add(column)

        'Agregar Columna de DIAS DE MORA
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.Int32")
        column.ColumnName = "DIAS_MORA"
        DatosImportado.Columns.Add(column)


        'Agregar Columna de id calculo
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.Int32")
        column.ColumnName = "ID_CALCULO"
        DatosImportado.Columns.Add(column)

        'Agregar Columna de fecha del sys
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.DateTime")
        column.ColumnName = "FECHA_SYS"
        DatosImportado.Columns.Add(column)
        Return DatosImportado
    End Function


    Private Function DatosExpediente(ByVal pexpediente As String) As DataTable
        Dim table As New DataTable
        Dim SQL As String = "SELECT * FROM EJEFISGLOBAL WHERE EFINROEXP = @EXPEDIENTE"
        Dim cmd As New SqlCommand(SQL, New SqlConnection(Funciones.CadenaConexion))
        cmd.Parameters.AddWithValue("@EXPEDIENTE", pexpediente.Trim)
        Dim _Adap As New SqlDataAdapter(cmd)
        _Adap.Fill(table)

        Return table
    End Function

    Private Sub BorrarDetalladoSQl(ByVal pExpediente As String)
        Dim sql As String = "DELETE FROM SQL_PLANILLA WHERE EXPEDIENTE =@EXP"
        Dim cmd As New SqlCommand(sql, New SqlConnection(Funciones.CadenaConexion))
        cmd.Parameters.AddWithValue("@exp", pExpediente)
        cmd.Connection.Open()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()
    End Sub

    Private Sub Save_SQL(ByVal EXPEDIENTE As String, ByVal LIQ_REC As Integer, ByVal INF As String, ByVal SUBSISTEMA As String, ByVal NIT_EMPRESA As String, ByVal RAZON_SOCIAL As String, ByVal ANNO As Integer, ByVal MES As Integer, ByVal CEDULA As String, ByVal NOMBRE As String, ByVal IBC As Double, ByVal AJUSTE As Double, ByVal FECHA_PAGO As Date, ByVal ID_GRUPO As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

        Dim command As New SqlCommand("Save_SQL", cn, _trans)
        command.CommandType = CommandType.StoredProcedure
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
        command.Parameters.AddWithValue("@FECHA_PAGO", Now())
        command.Parameters.AddWithValue("@LIQ_REC", LIQ_REC)
        command.Parameters.AddWithValue("@ID_GRUPO", ID_GRUPO)
        command.Parameters.AddWithValue("@MONTO_PAGO", 0)
        command.ExecuteNonQuery()

    End Sub


End Class