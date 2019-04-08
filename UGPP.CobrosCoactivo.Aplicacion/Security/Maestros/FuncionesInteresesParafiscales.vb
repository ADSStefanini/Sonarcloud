Imports System.Data.SqlClient

Module FuncionesInteresesParafiscales


    'Funcion para calcular la fecha habil de pago para los intereses de parafiscales
    Public Function FechasHabiles(ByVal numeroDiaHabil As Integer, ByVal anio As Integer, ByVal mes As Integer, ByVal tipoAportante As Integer, ByVal subsistema As String) As Date


        Dim FechaInicio As Date, FechaTemp As Date, FechaReal As Date

        FechaTemp = CDate("01/" & mes & "/" & anio).ToString("dd/MM/yyyy")

        If (subsistema = "SALUD" Or subsistema = "PENSION" Or subsistema = "CCF" Or subsistema = "ARL" Or subsistema = "SENA" Or subsistema = "ICBF") And (tipoAportante <> 1) Then
            FechaTemp = DateAdd(DateInterval.Month, 1, FechaTemp)
        End If

        'If (subsistema = "SALUD" Or subsistema = "PENSION" Or subsistema = "CCF" Or subsistema = "ARL" Or subsistema = "SENA" Or subsistema = "ICBF") Then
        '    FechaTemp = DateAdd(DateInterval.Month, 1, FechaTemp)
        'End If

        FechaInicio = FechaTemp.ToString("dd/MM/yyyy")
        FechaInicio = FechaInicio.ToString("dd/MM/yyyy")

        For x As Integer = 1 To numeroDiaHabil
            If x <= numeroDiaHabil Or numeroDiaHabil = 1 Then
                FechaReal = Nueva_Fecha_Acto(FechaInicio.ToString("dd/MM/yyyy"))
                If x < numeroDiaHabil Then
                    FechaInicio = DateAdd(DateInterval.Day, 1, FechaReal)
                Else
                    FechaInicio = FechaReal
                End If
            End If

           


        Next
        Return FechaInicio

    End Function

    Public Function Nueva_Fecha_Acto(ByVal Fecha As Date) As Date
        Dim Inc As Integer = 0
        Dim myTblDias As DataTable = Load_Dias_No_Habiles()
        Do

            Fecha = DateAdd(DateInterval.Day, Inc, Fecha)
            Fecha = Fecha.ToString("dd/MM/yyyy")
            If myTblDias.Select("FECHA= '" & Fecha.ToString("dd/MM/yyyy") & "'").Length > 0 Then
                Inc = 1
            ElseIf Fecha.DayOfWeek = DayOfWeek.Saturday Then
                Inc = 2
            ElseIf Fecha.DayOfWeek = DayOfWeek.Sunday Then
                Inc = 1
            Else
                Inc = 0
            End If
        Loop While Inc > 0

        Return Fecha
    End Function

    Private Function Load_Dias_No_Habiles() As DataTable
        Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT * FROM TDIAS_FESTIVOS ORDER BY FECHA", Funciones.CadenaConexion)
        Dim Table As DataTable = New DataTable
        Adap.Fill(Table)
        Return Table

    End Function

    'Calcular Intereses Parafiscales
    Public Function _CalcularIntereses(ByVal deuda As Double, ByVal fecha_deuda As Date, ByVal fecha_corte As Date, ByVal tasa_trimestral As Decimal) As Double
        Dim table As DataTable = New DataTable()
        Dim acumulado As Integer = 0
        Dim dias As Integer = 0
        Dim Intereses As Double = 0
        Dim AcumularIntereses As Double = 0

        Dim Con As Integer = 0

        fecha_deuda = fecha_deuda.ToString("dd/MM/yyyy")
        fecha_corte = fecha_corte.ToString("dd/MM/yyyy")
        Dim _Adap As New SqlDataAdapter("SELECT consec,p_trimestral, CONVERT(varchar, desde,105) as desde , CONVERT(varchar,hasta,105) hasta,t_diaria  FROM CALCULO_INTERESES_PARAFISCALES  WHERE consec >= (SELECT consec FROM CALCULO_INTERESES_PARAFISCALES  WHERE CONVERT(varchar, @fecha,105) BETWEEN desde AND HASTA ) ORDER BY CONSEC ASC", Funciones.CadenaConexion)
        _Adap.SelectCommand.Parameters.AddWithValue("@fecha", fecha_deuda)
        _Adap.Fill(table)

        ' Alterar data table


        For Each row As DataRow In table.Rows

            If fecha_deuda > CDate(row("hasta")) Then
                dias = 0
            Else
                'Congelar interes a la fecha actual
                If fecha_corte < row("hasta") Then
                    row("hasta") = fecha_corte
                End If

                If row("p_trimestral") = CDec(0) Then
                    row("p_trimestral") = tasa_trimestral
                End If

                'Calcular dias de moras
                dias = DateDiff(DateInterval.Day, fecha_deuda, row("hasta")) - acumulado

                'Calcular los interes para salud
                Intereses = (deuda * row("p_trimestral") * dias / 366)
                ' Intereses = RedondearUnidades(-2, Intereses)


                'Acumular dias
                acumulado += dias

                'Acumula Intereses
                AcumularIntereses += Intereses

            End If
        Next
        'AcumularIntereses = RedondearUnidades(-2, AcumularIntereses)
        Return AcumularIntereses

    End Function

    'Calcular Intereses Parafiscales
    Public Function _CalcularInteresesTasaDiaria(ByVal deuda As Double, ByVal fecha_deuda As Date, ByVal fecha_corte As Date, ByVal tasa_diaria As Decimal) As Double
        Dim table As DataTable = New DataTable()
        Dim acumulado As Integer = 0
        Dim dias As Integer = 0
        Dim Intereses As Double = 0
        Dim AcumularIntereses As Double = 0

        Dim Con As Integer = 0

        fecha_deuda = fecha_deuda.ToString("dd/MM/yyyy")
        fecha_corte = fecha_corte.ToString("dd/MM/yyyy")
        Dim _Adap As New SqlDataAdapter("SELECT consec,p_trimestral, CONVERT(varchar, desde,105) as desde , CONVERT(varchar,hasta,105) hasta,t_diaria  FROM CALCULO_INTERESES_PARAFISCALES  WHERE consec >= (SELECT consec FROM CALCULO_INTERESES_PARAFISCALES  WHERE '" & fecha_deuda & "' BETWEEN desde AND HASTA ) ORDER BY CONSEC ASC", Funciones.CadenaConexion)
        _Adap.Fill(table)

        ' Alterar data table


        For Each row As DataRow In table.Rows

            If fecha_deuda > CDate(row("hasta")) Then
                dias = 0
            Else
                'Congelar interes a la fecha actual
                If fecha_corte < row("hasta") Then
                    row("hasta") = fecha_corte
                End If

                If row("t_diaria") = CDec(0) Then
                    row("t_diaria") = tasa_diaria
                End If

                'Calcular dias de moras
                dias = DateDiff(DateInterval.Day, fecha_deuda, row("hasta")) - acumulado

                'Calcular los interes para salud
                Intereses = (deuda * row("t_diaria") * dias)
                ' Intereses = RedondearUnidades(-2, Intereses)


                'Acumular dias
                acumulado += dias

                'Acumula Intereses
                AcumularIntereses += Intereses

            End If
        Next
        'AcumularIntereses = RedondearUnidades(-2, AcumularIntereses)
        Return AcumularIntereses

    End Function


    'Verificar dia de pago
    Public Function ObtenerDiaPago(ByVal Nit_Cedula As String, ByVal tipoAportante As Integer) As String
        Dim diaPago As String = ""
        Dim _table As New DataTable
        Dim sFiltro As String = String.Empty, sSql As String = String.Empty
        Dim sNitCedula As String = Nit_Cedula.Substring(Nit_Cedula.Length - 2)
        Select Case tipoAportante
            Case 1
                sFiltro = sNitCedula & " BETWEEN  indepdesde AND  indephasta "
            Case 2
                sFiltro = sNitCedula & " BETWEEN empmas200desde AND empmas200hata "
            Case 3
                sFiltro = sNitCedula & " BETWEEN empmeno200desde AND empmeno200hasta "
        End Select

        sSql += "SELECT top 1 diahabil FROM FECHAS_PAGO_PILA WHERE " & sFiltro

        Try
            Dim _Adap As New SqlDataAdapter(sSql, Funciones.CadenaConexion)
            _Adap.Fill(_table)

            If _table.Rows.Count > 0 Then
                diaPago = _table.Rows(0).Item(0).ToString
            End If
        Catch ex As Exception
            ex.ToString()
        End Try

        Return diaPago
    End Function


End Module
