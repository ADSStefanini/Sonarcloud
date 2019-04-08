Imports System.Data.SqlClient
Imports System.IO

Partial Public Class capturarinteresesmulta2
    Inherits System.Web.UI.Page

    Protected Shared DatosImportado As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MostrarEA(Request("pExpediente"))
            CargarTasas()
        End If

    End Sub

    Protected Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click
        'Response.Redirect("../cuadros/detalle_intereses.aspx?deuda=" & txtDeudaCap.Text.Trim & "&fecha_deuda=" & txtFecDeuda.Text.Trim & "&fecha_corte=" & txtFecPago.Text.Trim & "&expediente=" & Request("pExpediente"))
        Try
            If txtFecDeuda.Text = "" Then
                lblError.Text = "No se ha detectado fecha de Exigibilidad, Por favor verifique la información del titulo"
                Exit Sub
            End If

            If txtFecPago.Text = "" Then
                lblError.Text = "Por favor digite la fecha estimada de pago."
                Exit Sub
            End If

            Dim TotalDeuda As Double = 0
            Dim FechaInicioPago As Date = CDate(txtFecPago.Text)
            Dim DiasMora As Integer = FuncionesInteresesMultas.CalcularDiasMoras(txtFecDeuda.Text, FechaInicioPago.ToString("dd/MM/yyyy"))

            txtDiasdeMora.Text = DiasMora

            Dim InteresesMora As Integer = CInt(FuncionesInteresesMultas.CalcularInteresesMoras(txtDeudaCap.Text, DiasMora))

            txtInetesesMoraMulta.Text = InteresesMora
            txtInetesesMoraMulta.Text = FormatCurrency(txtInetesesMoraMulta.Text, 0, TriState.True)

            'Calcular Total Deuda
            TotalDeuda = Math.Round(CDbl(txtDeudaCap.Text) + CDbl(InteresesMora))
            txtDeudaMasIntereses.Text = FormatCurrency(TotalDeuda, 0, TriState.True)

            SaveLOG("IN", CDec(txtDeudaCap.Text), CDate(txtFecDeuda.Text), CDate(txtFecPago.Text), CInt(txtDiasdeMora.Text), CDec(InteresesMora), CDec(TotalDeuda), 0, 0, 0, 0, Session("sscodigousuario"))


            TableExport.Rows.Add(txtFecDeuda.Text, txtFecPago.Text, CDbl(txtDeudaCap.Text).ToString("N0"), DiasMora, txtTasaAnualMulta.Text, InteresesMora.ToString("N0"), TotalDeuda.ToString("N0"))


            'CargarTasas()


        Catch ex As Exception
            lblError.Text = "ADVERTENCIA: " & ex.ToString()
        End Try
    End Sub


    'Cargar Tasas de intereses
    Private Sub CargarTasas()
        'Cargar Tasas de intereses
        Dim tasas() As String = FuncionesInteresesMultas.CargarTasas

        txtTasaAnualMulta.Text = CDec(tasas(0))
        txtTasaMensualMulta.Text = CDec(tasas(1))
    End Sub


    'Calcular Cuota Inicial
    Private Function CalcularCoutaInicial(ByVal PValorTotalDeuda As Double) As Integer
        Dim cuotaInicial As Integer = 0

        cuotaInicial = PValorTotalDeuda * _Porcentaje(PValorTotalDeuda)

        Return cuotaInicial

    End Function

    'Calcular Porcentaje a cobrar
    Private Function _Porcentaje(ByVal pValorTotalDeuda As Double) As Double
        With Me
            Dim numPorc As Double = 0
            Dim Table As New DataTable
            Dim _Adapter As New SqlDataAdapter("SELECT * FROM ACUERDO_PORCENTAJE  WHERE APP_TIPO = 1", Funciones.CadenaConexion)
            _Adapter.Fill(Table)
            _Adapter.SelectCommand.Parameters.AddWithValue("@VALOR_DEUDA", pValorTotalDeuda)
            _Adapter.Fill(Table)
            If Table.Rows.Count > 0 Then
                numPorc = Math.Round(CDbl(Table.Rows(0).Item("APP_PORCENTAJE") / 100), 2)
            End If

            Return numPorc

        End With
    End Function

    Private Function _ProyectarInteresesMulta(ByVal pSaldoDiferir As Double, ByVal pFechaExigibilidad As Date, ByVal pFechaInicioPago As Date, ByVal pTasaMensual As Decimal, ByVal pNumCuota As Integer) As Integer
        Dim table As DataTable = New DataTable()
        Dim acumulado As Integer = 0
        Dim diasMora As Integer = 0
        Dim intereses As Integer = 0
        Dim fecInicioIntereses As Date = pFechaInicioPago.AddDays(1)

        table = FechasHabiles(pNumCuota, fecInicioIntereses)

        ' declarar DataColumn y DataRow variables. 
        Dim column As DataColumn

        ' Columna de porcentaje    
        column = New DataColumn()
        column.DataType = System.Type.GetType("System.Int32")
        column.ColumnName = "Dias"
        table.Columns.Add(column)

        ' columna de intereses
        column = New DataColumn()
        column.DataType = Type.GetType("System.Int32")
        column.ColumnName = "Interes"
        table.Columns.Add(column)

        ' columna de nuevo saldo
        column = New DataColumn()
        column.DataType = Type.GetType("System.Int32")
        column.ColumnName = "n_saldo"
        table.Columns.Add(column)

        ' columna de nuevo saldo cuota
        column = New DataColumn()
        column.DataType = Type.GetType("System.Int32")
        column.ColumnName = "n_saldo_cuota"
        table.Columns.Add(column)

        Dim sigFechaHabil As Date = Nothing
        Dim nSaldoCuota As Integer = 0
        Dim valorCuota As Integer = (pSaldoDiferir / pNumCuota)


        ' Alterar data table
        For Each row As DataRow In table.Rows

            'Calcular dias de moras
            If sigFechaHabil = Nothing Then
                diasMora = DateDiff(DateInterval.Day, CDate(fecInicioIntereses), CDate(row("fechasHabiles")))
            Else
                diasMora = DateDiff(DateInterval.Day, CDate(sigFechaHabil), CDate(row("fechasHabiles")))
            End If


            sigFechaHabil = row("fechasHabiles")

            'Acumular dias
            acumulado += diasMora

            'Alterar el dia de la tabla
            row("Dias") = diasMora

            'Nuevo Saldo a diferir
            If nSaldoCuota = 0 Then
                nSaldoCuota = pSaldoDiferir
            Else
                nSaldoCuota = nSaldoCuota - valorCuota
            End If

            row("n_saldo_cuota") = nSaldoCuota

            'Calcular los interes de los dias de moras
            row("Interes") = Math.Round(nSaldoCuota * (pTasaMensual / 30) * diasMora, 3)

            'sumatoria de intereses
            intereses += row("Interes")

            ' row("n_saldo") = pSaldoDiferir + intereses

        Next
        Return intereses

    End Function

    'Consultar Fechas habiles a partir de la fecha de pago
    Private Function FechasHabiles(ByVal pNumCuota As Integer, ByVal pFechaInicioAcuerdo As Date) As DataTable
        With Me
            Dim table As DataTable = New DataTable()
            ' declarar DataColumn y DataRow variables. 
            Dim column As DataColumn

            ' Columna de porcentaje    
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.DateTime")
            column.ColumnName = "fechasHabiles"
            table.Columns.Add(column)

            Dim x As Integer
            Dim FechaCuota As Date, FechaReal As Date
            Dim CuotaFin As Integer = 0
            FechaCuota = CDate(pFechaInicioAcuerdo)
            Dim rows As DataRow
            For x = 1 To pNumCuota
                If x + 1 = pNumCuota Then
                    CuotaFin = 1
                End If
                FechaReal = DateAdd(DateInterval.Month, x, FechaCuota)
                If FechaReal.DayOfWeek = DayOfWeek.Saturday Then
                    FechaReal = DateAdd(DateInterval.Day, 2, FechaReal)
                ElseIf FechaReal.DayOfWeek = DayOfWeek.Sunday Then
                    FechaReal = DateAdd(DateInterval.Day, 1, FechaReal)
                End If
                rows = table.NewRow
                rows.Item("fechasHabiles") = FechaReal
                table.Rows.Add(rows)
            Next

            Return table
        End With
    End Function

    Private Sub MostrarEA(ByVal pNumExpediente As String)
        Try

            If pNumExpediente <> "" Then
                Dim totalDeuda, pagos, saldo As Double

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
                    totalDeuda = Convert.ToDouble(Reader("totaldeuda").ToString())
                Else
                    totalDeuda = 0
                End If
                Reader.Close()

                '20/10/2014. Se incorporó el ajuste 1406 dentro del capital. xxxyyy
                'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
                Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital " & _
                                        "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' GROUP BY NroExp"

                Dim Command2 As New SqlCommand(sql2, Connection)
                Dim Reader2 As SqlDataReader = Command2.ExecuteReader
                If Reader2.Read Then
                    If Reader2("pagcapital").ToString() = "" Then
                        pagos = 0
                    Else
                        pagos = Convert.ToDouble(Reader2("pagcapital").ToString())

                    End If
                Else
                    pagos = 0
                End If
                Reader2.Close()

                'Mostrar la diferencia entre Total deuda - Capital pagado
                saldo = Convert.ToDouble(totalDeuda) - Convert.ToDouble(pagos)
                txtDeudaCap.Text = saldo

                If CInt(pagos) = 0 Then

                    'Consultar fecha exigibilidad del titulo.
                    Dim sql3 As String = "SELECT MT.MT_fec_exi_liq " & _
                                        " FROM MAESTRO_TITULOS MT  " & _
                                        " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " & _
                                        "WHERE MT.MT_expediente = '" & pNumExpediente & "'"

                    Dim Command3 As New SqlCommand(sql3, Connection)
                    Dim Reader3 As SqlDataReader = Command3.ExecuteReader
                    If Reader3.Read Then

                        If Reader3("MT_fec_exi_liq").ToString = "" Then
                            lblError.Text = "No se ha detectado fecha de Exigibilidad, Por favor verifique la información del titulo"
                        Else
                            txtFecDeuda.Text = Convert.ToDateTime(Reader3("MT_fec_exi_liq")).ToString("dd/MM/yyyy")
                        End If


                    Else
                        txtFecDeuda.Text = ""
                        lblError.Text = "No se ha detectado fecha de Exigibilidad, Por favor verifique la información del titulo"
                    End If
                    Reader3.Close()

                Else


                    '--***********************************************************************
                    'Consultar Fecha del ultimo pago
                    Dim sql4 As String = "select MAX(pagFecha ) pagfecha from PAGOS where NroExp = '" & pNumExpediente & "'"

                    Dim Command4 As New SqlCommand(sql4, Connection)
                    Dim Reader4 As SqlDataReader = Command4.ExecuteReader
                    If Reader4.Read Then

                        If Reader4("pagfecha").ToString = "" Then
                            txtFecDeuda.Text = ""
                            lblError.Text = "No se ha detectado fecha del pago, Por favor verifique"
                        Else
                            txtFecDeuda.Text = Convert.ToDateTime(Reader4("pagfecha")).ToString("dd/MM/yyyy")
                            txtFecDeuda.Text = DateAdd(DateInterval.Day, 1, CDate(txtFecDeuda.Text))
                        End If
                    End If
                    Reader4.Close()
                End If

                Connection.Close()
            End If
        Catch ex As Exception
            lblError.Text = "Error: " & ex.ToString
        End Try
    End Sub

    Private Function TableExport() As DataTable
        DatosImportado = New DataTable
        DatosImportado.Columns.AddRange(New DataColumn() {New DataColumn("FECHA_EXIG", GetType(String)), _
                                                New DataColumn("FECHA_PAGO", GetType(String)), _
                                                New DataColumn("DEUDA", GetType(String)), _
                                                New DataColumn("DIAS_MORA", GetType(String)), _
                                               New DataColumn("TASA", GetType(String)), _
                                               New DataColumn("INTERESES", GetType(String)), _
                                               New DataColumn("TOTAL_DEUDA", GetType(String))})

        Return DatosImportado

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


    Protected Sub cmdDescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDescargar.Click
        If DatosImportado.Rows.Count > 0 Then
            descargafichero(DatosImportado, Now.ToString("ddMMyyyyhhssmm") & Request("pExpediente") & ".xls", True)
        Else
            lblError.Text = "No hay información a descargar"
        End If
    End Sub

    Private Sub SaveLOG(ByVal TIPO As String, ByVal VALOR_CAPITAL As Decimal, ByVal FECHA_EXIG As Date, ByVal FECHA_PAGO As Date, ByVal DIAS_MORA As Integer, ByVal INTERESES As Decimal, ByVal TOTAL_DEUDA As Decimal, ByVal IPC As Decimal, ByVal VALOR_IPC As Decimal, ByVal ANIO_IPC As Integer, ByVal TASA As Decimal, ByVal USUARIO As String)
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim command As New SqlCommand("SP_LOG_IPC_INTERESES", cn)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.AddWithValue("@TIPO", TIPO)
        command.Parameters.AddWithValue("@VALOR_CAPITAL", VALOR_CAPITAL)
        command.Parameters.AddWithValue("@FECHA_EXIG", FECHA_EXIG)
        command.Parameters.AddWithValue("@FECHA_PAGO", IIf(FECHA_PAGO = "12:00:00 AM", DBNull.Value, FECHA_PAGO))
        command.Parameters.AddWithValue("@DIAS_MORA", DIAS_MORA)
        command.Parameters.AddWithValue("@INTERESES", INTERESES)
        command.Parameters.AddWithValue("@TOTAL_DEUDA", TOTAL_DEUDA)
        command.Parameters.AddWithValue("@IPC", IPC)
        command.Parameters.AddWithValue("@VALOR_IPC", VALOR_IPC)
        command.Parameters.AddWithValue("@ANIO_IPC", ANIO_IPC)
        command.Parameters.AddWithValue("@TASA", TASA)
        command.Parameters.AddWithValue("@USUARIO", USUARIO)
        cn.Open()
        command.ExecuteNonQuery()
        cn.Close()

    End Sub

End Class