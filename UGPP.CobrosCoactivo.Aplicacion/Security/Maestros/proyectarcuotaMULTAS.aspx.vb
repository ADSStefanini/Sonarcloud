Imports System.Data.SqlClient
Imports System.IO
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class proyectarcuotaMULTAS
    Inherits System.Web.UI.Page

    Dim Conexion As SqlConnection
    Dim Documento As SqlCommand
    Dim Update_AP As SqlCommand
    Dim _Adapter As SqlDataAdapter
    Dim Plazo As SqlCommand
    Dim Porcentaje As SqlCommand

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        InitializeComponent()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
        End If

        If Not Me.Page.IsPostBack Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilitan los campos para este perfil
                btnNuevo.Enabled = False
                cmdCalcularInteres.Enabled = False
                btndescargar.Enabled = False
                btnGuardar.Enabled = False
                txtPorcentajeCuotaini.Enabled = False
                txtFechaExig.Enabled = False
                txtfechaPago.Enabled = False
                TxtNCuotas.Enabled = False
                chkexcluircuota.Enabled = False
                TxtNumAcuerdo.Enabled = False
                TxtNroProceso.Enabled = False
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
                txtPorcentajeCuotaini.Enabled = True
                txtFechaExig.Enabled = True
                txtfechaPago.Enabled = True
                TxtNCuotas.Enabled = True
                chkexcluircuota.Enabled = True
                TxtNumAcuerdo.Enabled = True
                TxtNroProceso.Enabled = True
                Txt_solicitante.Enabled = True
                TxtNom_Solicitante.Enabled = True
                CmbSolicitante.Enabled = True
                TxtGarante.Enabled = True
                TxtNom_garante.Enabled = True
                CmbGarante.Enabled = True
                TxtDescripcionGarantia.Enabled = True
            End If

            Nuevo(True)
            NewDocument()

            TxtNroProceso.Text = Request("pExpediente")
            TxtValorDeuda.Text = FormatCurrency(Request("pValorDeuda"), 0, TriState.True)
            llenarNroCuotas(60)
            llenarPorcentajes(100)
            txtPorcentajeCuotaini.SelectedValue = "30"
            CargarTasas()
        End If
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

    Protected Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click
        Try
            lkbdescargar.Text = ""
            lblError.Text = ""
            Procesar()
        Catch ex As Exception
            lblError.Text = "Error: " & ex.ToString
        End Try
    End Sub

    Private Sub Procesar()
        If TipoTitulo(TxtNroProceso.Text.Trim()) Then

            If Left(txtFechaExig.Text, 10) = "" Then
                lblError.Text = "Por favor digite la fecha de exigibilidad. "
                Exit Sub
            End If

            If Left(txtfechaPago.Text, 10) = "" Then
                lblError.Text = "Por favor digite la fecha de pago. "
                Exit Sub
            End If

            Dim FechaInicioPago As Date = CDate(Left(txtfechaPago.Text, 10)).ToString("dd/MM/yyyy")

            'Calcular los dias de moras
            Dim DiasMora As Integer = FuncionesInteresesMultas.CalcularDiasMoras(CDate(Left(txtFechaExig.Text, 10)), FechaInicioPago) + 1
            txtdiasmora.Text = DiasMora

            'Calcular los interes de mora
            Dim InteresesMora As Integer = CInt(CalcularInteresesMoras(TxtValorDeuda.Text, DiasMora))

            txtimorateresesmora.Text = FormatCurrency(InteresesMora, 0, TriState.True)

            'Calcular Total Deuda
            txtIntCap.Text = Math.Round(CDbl(TxtValorDeuda.Text) + CDbl(InteresesMora))
            txtIntCap.Text = FormatCurrency(txtIntCap.Text, 0, TriState.True)

            'Calcular Cuota Inicial
            If chkexcluircuota.Checked Then
                TxtCuotaI.Text = 0
            Else
                TxtCuotaI.Text = CalcularCoutaInicial(txtIntCap.Text)
                TxtCuotaI.Text = FormatCurrency(TxtCuotaI.Text, 0, TriState.True)
            End If

            'Calcular Saldo a diferir
            txtSaldoDiferir.Text = Math.Round(CInt(txtIntCap.Text) - CInt(TxtCuotaI.Text))
            txtSaldoDiferir.Text = FormatCurrency(txtSaldoDiferir.Text, 0, TriState.True)

            'Calcula Intereses
            Dim tasas() As String = FuncionesInteresesMultas.CargarTasas
            txtInteresesProyectado.Text = _ProyectarInteresesMulta(txtSaldoDiferir.Text, Left(txtFechaExig.Text, 10), FechaInicioPago, tasas(1), TxtNCuotas.SelectedValue)
            txtInteresesProyectado.Text = FormatCurrency(txtInteresesProyectado.Text, 0, TriState.True)

            With Me
                Dim tabla As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable

                .Liquidar(tabla)
                .LlenarGrid()

            End With

            lblError.Text = "Proyección realizada satisfactoriamente..."
        Else
            lblError.Text = "El título asociado al expediente no es una RESOLUCIÓN MULTA L1438/11 Ó RESOLUCIÓN MULTA L1607/12..."
        End If

    End Sub

    'Calcular Cuota Inicial
    Private Function CalcularCoutaInicial(ByVal PValorTotalDeuda As Double) As Integer
        Dim cuotaInicial As Integer = 0

        cuotaInicial = PValorTotalDeuda * _Porcentaje(PValorTotalDeuda)

        Return cuotaInicial
    End Function

    'Metodo para proyectar intereses
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
                nSaldoCuota = txtSaldoDiferir.Text
            Else
                nSaldoCuota = nSaldoCuota - valorCuota
            End If

            row("n_saldo_cuota") = nSaldoCuota

            'Calcular los interes de los dias de moras
            row("Interes") = nSaldoCuota * (pTasaMensual / 30) * diasMora

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

    'Calcular Porcentaje a cobrar
    Private Function _Porcentaje(ByVal pValorTotalDeuda As Double) As Double
        With Me
            Dim numPorc As Double = 0
            'Dim Table As New DataTable
            'Dim _Adapter As New SqlDataAdapter("SELECT * FROM ACUERDO_PORCENTAJE  WHERE APP_TIPO = 1", Funciones.CadenaConexion)
            '_Adapter.Fill(Table)
            '_Adapter.SelectCommand.Parameters.AddWithValue("@VALOR_DEUDA", pValorTotalDeuda)
            '_Adapter.Fill(Table)
            'If Table.Rows.Count > 0 Then
            'numPorc = Math.Round(CDbl(Table.Rows(0).Item("APP_PORCENTAJE") / 100), 2)
            numPorc = Math.Round(CInt(txtPorcentajeCuotaini.SelectedValue) / 100, 8)

            'End If

            Return numPorc
        End With
    End Function

    'Cargar Tasas de intereses
    Private Sub CargarTasas()
        Dim tasas() As String = FuncionesInteresesMultas.CargarTasas

        txttasainteres.Text = CDec(tasas(0))
        txttasamensual.Text = CDec(tasas(1))
    End Sub

    'Establecer numero de cuotas 
    Private Sub llenarNroCuotas(ByVal numero As Integer)

        TxtNCuotas.Items.Clear()
        For i As Integer = 1 To numero - 1
            TxtNCuotas.Items.Insert(i - 1, i + 1)
        Next

    End Sub

    'Establecer porcentaje de cuota inicial 
    Private Sub llenarPorcentajes(ByVal numero As Integer)

        txtPorcentajeCuotaini.Items.Clear()
        For i As Integer = 1 To numero - 1
            txtPorcentajeCuotaini.Items.Insert(i - 1, i - 1)
        Next

    End Sub

    Private Sub LlenarGrid()
        With Me
            Dim row As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
            Dim FilasAct As Integer

            If .ViewState("rowDetalle") Is Nothing Then
                FilasAct = 0
            Else
                row = CType(.ViewState("rowDetalle"), Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable)
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

    'Metodo para liquidar las cuotas otorgadas 
    Private Sub Liquidar(ByVal TableDetalle As Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable)

        Dim interes As Integer = CInt(txtInteresesProyectado.Text)

        Dim totaldeuda As Double = interes + CInt(txtSaldoDiferir.Text)
        Dim valoraplicado As Double = 0
        Dim saldocapital As Double = CInt(txtSaldoDiferir.Text)

        If Left(txtFechaExig.Text, 10) = "" Or Left(txtfechaPago.Text, 10) = "" Then
            lblError.Text = "Por favor digite la información requerida"
            Exit Sub
        End If

        With Me
            Dim rows As Reportes_Admistratiivos.DETALLES_ACUERDO_PAGORow
            If Val(TxtNCuotas.SelectedValue) = 0 Then
                lblError.Text = "Digite el numero de cuotas del la fecilidad de pago"
                Exit Sub
            End If

            If Not IsDate(Left(txtfechaPago.Text, 10)) OrElse CDate(Left(txtfechaPago.Text, 10)) < Date.Today Then
                lblError.Text = "La Fecha de pago no es Valida"

                Exit Sub
            End If

            If CDate(Left(txtFechaExig.Text, 10)) > CDate(Left(txtfechaPago.Text, 10)) Then
                lblError.Text = "La fecha de exigibilidad no puede ser mayor de la fecha de pago"
                Exit Sub
            End If

            Dim Valor_Cuota As Decimal, Cuota_Inicial As Decimal = CType(TxtCuotaI.Text, Decimal)
            Dim x As Integer, NCuotas As Integer = Val(.TxtNCuotas.SelectedValue)
            'Dim NRecibo As String = .TxtNumAcuerdo.Text & "-01"
            Dim FechaCuota As Date, FechaReal As Date
            Dim CuotaFin As Integer = 0
            FechaCuota = DateAdd(DateInterval.Day, 1, CDate(Left(txtfechaPago.Text, 10)))
            rows = TableDetalle.NewDETALLES_ACUERDO_PAGORow

            For x = 0 To NCuotas - 1
                If x = NCuotas Then
                    CuotaFin = 1
                End If
                Valor_Cuota = (totaldeuda / NCuotas)

                valoraplicado = Valor_Cuota - interes

                FechaReal = DateAdd(DateInterval.Month, x, FechaCuota)

                If FechaReal.DayOfWeek = DayOfWeek.Saturday Then
                    FechaReal = DateAdd(DateInterval.Day, 2, FechaReal)
                ElseIf FechaReal.DayOfWeek = DayOfWeek.Sunday Then
                    FechaReal = DateAdd(DateInterval.Day, 1, FechaReal)
                End If

                rows = TableDetalle.NewDETALLES_ACUERDO_PAGORow
                'rows.DOCUMENTO = "0" : rows.CUOTA_NUMERO = x + 1 : rows.FECHA_CUOTA = CDate(FechaReal).ToString("dd/MM/yyyy") : rows.IsFECHA_PAGONull() : rows.VALOR_CUOTA = Valor_Cuota : rows.PAGADO = 0 : rows.CUOTA_FIN = CuotaFin

                If x = 0 Then
                    rows.DOCUMENTO = TxtNumAcuerdo.Text : rows.CUOTA_NUMERO = x + 1 : rows.FECHA_CUOTA = CDate(FechaReal).ToString("dd/MM/yyyy") : rows.IsFECHA_PAGONull() : rows.VALOR_CUOTA = Valor_Cuota : rows.VALOR_PAGADO = 0 : rows.CUOTA_FIN = CuotaFin : rows.VALOR_INTERES = interes : rows.SALDO_CAPITAL = saldocapital
                ElseIf x = 1 Then
                    rows.DOCUMENTO = TxtNumAcuerdo.Text : rows.CUOTA_NUMERO = x + 1 : rows.FECHA_CUOTA = CDate(FechaReal).ToString("dd/MM/yyyy") : rows.IsFECHA_PAGONull() : rows.VALOR_CUOTA = Valor_Cuota : rows.VALOR_PAGADO = 0 : rows.CUOTA_FIN = CuotaFin : rows.VALOR_INTERES = 0 : rows.SALDO_CAPITAL = saldocapital - valoraplicado
                    saldocapital = rows.SALDO_CAPITAL
                Else
                    rows.DOCUMENTO = TxtNumAcuerdo.Text : rows.CUOTA_NUMERO = x + 1 : rows.FECHA_CUOTA = CDate(FechaReal).ToString("dd/MM/yyyy") : rows.IsFECHA_PAGONull() : rows.VALOR_CUOTA = Valor_Cuota : rows.VALOR_PAGADO = 0 : rows.CUOTA_FIN = CuotaFin : rows.VALOR_INTERES = 0 : rows.SALDO_CAPITAL = saldocapital - Valor_Cuota
                    saldocapital = rows.SALDO_CAPITAL
                End If

                TableDetalle.AddDETALLES_ACUERDO_PAGORow(rows)
            Next
            ViewState("rowDetalle") = TableDetalle

        End With
    End Sub

    Protected Sub btndescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btndescargar.Click

        If DtgAcuerdos IsNot Nothing Then
            Dim MTG As New MetodosGlobalesCobro

            '"Convertir" datatable a dataset
            Dim ds As New DataSet
            ds.Merge(MTG.GridviewToDataTable(DtgAcuerdos))

            'Exportar el dataset anterior a Excel 
            MTG.ExportDataSetToExcel(ds, "proyecciomultas.xls")
        Else
            lblError.Text = "Error: no se detecto registros a exportar..."
        End If

    End Sub

    Private Sub Nuevo(ByVal limpiarPlaca As Boolean)
        With Me
            If limpiarPlaca Then
                .TxtNroProceso.Text = ""
            End If

            .TxtNom_Solicitante.Text = ""
            .Txt_solicitante.Text = ""
            .TxtNom_garante.Text = ""
            .TxtGarante.Text = ""
            .txtIntCap.Text = FormatCurrency(0, 2, TriState.True)
            ViewState("rowDetalle") = Nothing
            ViewState("TablaMaestro") = Nothing
            .LlenarGrid()
            .DtgAcuerdos.DataBind()
            .NewDocument()
            txtfechaPago.Text = ""
            txtFechaExig.Text = ""
            .TxtCuotaI.Text = FormatCurrency(0, 2, TriState.True)
            TxtNCuotas.Items.Clear()
            .TxtNCuotas.Items.Insert(0, 0)
            lkbdescargar.Text = ""
        End With
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
        Nuevo(True)
    End Sub

    Private Sub _SaveMaestro(ByVal Mytrans As SqlTransaction, ByVal con As SqlConnection, ByVal expediente As String)

        Dim table As Reportes_Admistratiivos.MAESTRO_ACUERDOSDataTable
        Dim adap As New Reportes_AdmistratiivosTableAdapters.MAESTRO_ACUERDOSTableAdapter
        Dim rows As Reportes_Admistratiivos.MAESTRO_ACUERDOSRow
        adap.Transaction = Mytrans
        adap.Connection = con

        table = adap.GetDataDocumento(TxtNumAcuerdo.Text)

        If table.Rows.Count > 0 Then
            rows = table.Rows(0)
            ViewState("INSERT") = 1
        Else
            rows = table.NewMAESTRO_ACUERDOSRow
            ViewState("INSERT") = 0
        End If

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
                lblError.Text = "Por favor complete la información del garanate.. "
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
        rows.CUOTA_INI = Decimal.Round(CDbl(TxtCuotaI.Text))
        rows.TOTAL_DEUDA = Decimal.Round(CDbl(txtIntCap.Text))
        rows.FECHA_EXIGIBILIDAD = CDate(Left(txtFechaExig.Text, 10))

        If table.Rows.Count = 0 Then
            table.AddMAESTRO_ACUERDOSRow(rows)
        End If
        ViewState("TablaMaestro") = table
        adap.Update(table)
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click

        lkbdescargar.Text = ""
        If TipoTitulo(TxtNroProceso.Text.Trim()) Then
            Try
                Procesar()
            Catch ex As Exception
                lblError.Text = "Error: " & ex.ToString
                Exit Sub
            End Try

            If DtgAcuerdos.Rows.Count <> 0 Then

                If ValidarAcuerdo(TxtNroProceso.Text.Trim) = False Then

                    Dim dstAcuerdo As New Reportes_Admistratiivos
                    Dim TablaDetalle As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable
                    Dim adapDetalle As New Reportes_AdmistratiivosTableAdapters.DETALLES_ACUERDO_PAGOTableAdapter
                    Dim Transacc As SqlTransaction
                    Conexion.Open()
                    Transacc = Conexion.BeginTransaction
                    '_SaveMaestro(Transacc, Conexion, tbDatosGen.Rows(0).Item("MAN_EXPEDIENTE"))
                    _SaveMaestro(Transacc, Conexion, TxtNroProceso.Text.Trim)

                    Liquidar(TablaDetalle)
                    LlenarGrid()

                    Try
                        'Guardar en la tabla de detalle
                        adapDetalle.Transaction = Transacc
                        adapDetalle.Connection = Conexion

                        If ViewState("INSERT") = 1 Then
                            adapDetalle.Delete(TxtNumAcuerdo.Text)
                        Else
                            'Nuevo consecutivo de documento
                            Update_AP.Transaction = Transacc
                            Update_AP.ExecuteNonQuery()
                        End If

                        adapDetalle.Update(TablaDetalle)

                        Dim FechaInicio As Date = Left(txtfechaPago.Text, 10)

                        '22-05-2014 actualizar estado del expediente.
                        ActualizarEstadoProceso(TxtNroProceso.Text.Trim, Conexion, Transacc)
                        'Fin

                        Transacc.Commit()

                        lblError.Text = "Acuerdo de pago numero " & TxtNumAcuerdo.Text & " realizado satisfactoriamente.."
                        lkbdescargar.Text = "Click aqui para descargar la <b>RESOLUCIÓN<b/>"
                    Catch ex As Exception
                        Transacc.Rollback()
                        lblError.Text = "Error " & ex.Message
                    Finally
                        Conexion.Close()
                    End Try
                Else
                    lblError.Text = "El expediente " & TxtNroProceso.Text.Trim & " tiene un acuerdo de pago vigente..."
                    lkbdescargar.Text = "Click aqui para descargar la <b>RESOLUCIÓN<b/>"
                End If
            Else
                lblError.Text = "Por favor realize la proyección de la deuda..."
            End If
        Else
            lblError.Text = "El título asociado al expediente es una liquidación oficial..."
        End If

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

        Dim paramentros(16) As WordReport.Marcadores_Adicionales
        ''Dim fecha3 As Date
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
            paramentros(1).Valor = Num2Text(dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA"))
            paramentros(2).Marcador = "FECHA_REG"
            fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("FECHAEXI"))
            paramentros(2).Valor = fecha.ToString(" dd 'de' MMMM 'de' yyy")
            paramentros(3).Marcador = "fecha_antes"
            paramentros(3).Valor = dtsPlantilla.Rows.Item(0)("FECHA_ANT")

            paramentros(4).Marcador = "Nro_Resolucion"
            paramentros(4).Valor = resolucion

            paramentros(5).Marcador = "Tanual"
            paramentros(5).Valor = getPorcentaje(0)
            paramentros(6).Marcador = "Tmensual"
            paramentros(6).Valor = getPorcentaje(1)
            paramentros(7).Marcador = "SCuota"
            paramentros(7).Valor = Math.Round(dtsPlantilla.Rows.Item(0)("SALDO_INICIAL") / dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") * 100)

            'ENVIAR NUMERO DE CUOTAS 
            Dim tb1 As New DataTable
            Dim ad As New SqlDataAdapter("SELECT A.CUOTA_NUMERO,convert(varchar(10),A.FECHA_CUOTA,103) AS FECHA_CUOTA , CONVERT (VARCHAR(50),CONVERT (MONEY, ISNULL( A.SALDO_CAPITAL,0)),1) AS SALDO_CAPITAL, CONVERT (VARCHAR(50),CONVERT (MONEY,ISNULL( A.VALOR_CUOTA - A.VALOR_INTERES,0)),1) AS APORTE_CAPITAL,CONVERT (VARCHAR(50),CONVERT (MONEY,ISNULL( A.VALOR_INTERES,0)),1)AS APORTE_INTERES,CONVERT (VARCHAR(50),CONVERT (MONEY, ISNULL( A.VALOR_CUOTA,0)),1) AS VALOR_CUOTA FROM DETALLES_ACUERDO_PAGO A , MAESTRO_ACUERDOS B WHERE A.DOCUMENTO = B.DOCUMENTO AND B.EXPEDIENTE = @EXPEDIENTE", Funciones.CadenaConexion)
            ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            ad.Fill(tb1)

            paramentros(8).Marcador = "txt_cuota"
            paramentros(8).Valor = Num2Text(tb1.Rows.Count)
            paramentros(9).Marcador = "ncuota"
            paramentros(9).Valor = tb1.Rows.Count

            Dim tb2 As New DataTable
            Dim ad2 As New SqlDataAdapter("SELECT salario_minimo FROM ENTESCOBRADORES", Funciones.CadenaConexion)
            ad2.Fill(tb2)

            paramentros(10).Marcador = "txt_salario"
            paramentros(10).Valor = Num2Text(CInt(dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") / tb2.Rows(0).Item("salario_minimo")))
            paramentros(11).Marcador = "salarios"
            paramentros(11).Valor = CInt(dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") / tb2.Rows(0).Item("salario_minimo"))

            paramentros(12).Marcador = "totalvalor"
            paramentros(12).Valor = String.Format("{0:C0}", CInt(dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA")))

            paramentros(13).Marcador = "saldoinicial"
            paramentros(13).Valor = String.Format("{0:C0}", CInt(dtsPlantilla.Rows.Item(0)("SALDO_INICIAL")))

            paramentros(14).Marcador = "saldodiferir"
            paramentros(14).Valor = String.Format("{0:C0}", dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") - dtsPlantilla.Rows.Item(0)("SALDO_INICIAL"))

            paramentros(15).Marcador = "repLegal"
            paramentros(15).Valor = dtsPlantilla.Rows.Item(0)("NOMBRE_RL")

            paramentros(16).Marcador = "fecha_emision"
            paramentros(16).Valor = getfecha_tipada(Today.Date).ToUpper


            worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadPagoResolucionConcedeMulta, paramentros, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)

        End If

        Return worddocresult

    End Function

    Private Sub SendReport(ByVal NombreArchivo As String, ByVal Plantilla As String)
        Response.ContentType = "application/msword"
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.doc", NombreArchivo))
        Response.Write(Plantilla)
        Response.End()
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

            If (tipo = "05") Or (tipo = "07") Then
                sw = True
            End If

        End If

        Return sw
    End Function

    Private Function ValidarMandamiento(ByVal expediente As String) As Boolean
        Dim sw As Boolean = False
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT DG_NRO_DOC AS RESOLUCION,DG_FECHA_DOC AS FECHA,DG_EXPEDIENTE AS EXPEDIENTE FROM DOCUMENTOS_GENERADOS A WHERE DG_EXPEDIENTE = '" & expediente & "' AND DG_COD_ACTO = '013'")

        If tb.Rows.Count > 0 Then
            sw = True
        End If

        Return sw
    End Function

    Protected Sub lkbdescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lkbdescargar.Click
        Dim exp As String = TxtNroProceso.Text.Trim
        Dim resolucion As Integer = saveResolucion(TxtNroProceso.Text.Trim, "337")

        Dim dowload As String = Export(TxtNroProceso.Text.Trim, "337", resolucion)

        If dowload = "" Then
            ''mensaje no informe
            lblError.Text = "No se ha encontrado información para mostrar la RESOLUCION QUE CONCEDE FACILIDAD DE PAGO"
        Else
            SendReport("RESOLUCION_CONCEDE_FACILIDAD_MULTAS_" & exp & "_" & Today.ToString("dd.MM.yyyy"), dowload)
        End If
    End Sub

End Class


