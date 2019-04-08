Imports System.Data.SqlClient

Partial Public Class capturarinteresesmulta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CargarTasas()
        Dim MTG As New MetodosGlobalesCobro
        lblNomPerfil.Text = MTG.GetNomPerfil(Session("mnivelacces"))
    End Sub

    Protected Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click
        'Response.Redirect("../cuadros/detalle_intereses.aspx?deuda=" & txtDeudaCap.Text.Trim & "&fecha_deuda=" & txtFecDeuda.Text.Trim & "&fecha_corte=" & txtFecPago.Text.Trim & "&expediente=" & Request("pExpediente"))
        Try

            Dim FechaInicioPago As Date = CDate(txtFecPago.Text)
            Dim DiasMora As Integer = FuncionesInteresesMultas.CalcularDiasMoras(txtFecDeuda.Text, FechaInicioPago.ToString("dd/MM/yyyy"))

            txtDiasdeMora.Text = DiasMora

            Dim InteresesMora As Integer = CInt(FuncionesInteresesMultas.CalcularInteresesMoras(txtDeudaCap.Text, DiasMora))

            txtInetesesMoraMulta.Text = InteresesMora
            txtInetesesMoraMulta.Text = FormatCurrency(txtInetesesMoraMulta.Text, 0, TriState.True)

            'Calcular Total Deuda
            txtDeudaMasIntereses.Text = Math.Round(CDbl(txtDeudaCap.Text) + CDbl(InteresesMora))
            txtDeudaMasIntereses.Text = FormatCurrency(txtDeudaMasIntereses.Text, 0, TriState.True)

            CargarTasas()

        Catch ex As Exception
            ex.ToString()
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

    Protected Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click

        'Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
        Dim Perfil As String = GetNomPerfil(Session("sscodigousuario"))

        If Perfil = "REPARTIDOR" Then
            Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")

        ElseIf Perfil = "VERIFICADOR DE PAGOS" Then
            Response.Redirect("PAGOS.aspx")

        ElseIf Perfil = "GESTOR - ABOGADO" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "REVISOR" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "SUPERVISOR" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        Else
            ' SUPER ADMINISTRADOR
            Response.Redirect("EJEFISGLOBAL.aspx")
        End If
    End Sub
End Class