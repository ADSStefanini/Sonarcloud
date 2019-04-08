Imports System.Data.SqlClient

Module FuncionesInteresesMultas

    'Calcular dias de moras
    Public Function CalcularDiasMoras(ByVal pFechaExigibilidad As Date, ByVal pFechaPago As Date) As Integer
        Dim diasMora As Integer = 0

        diasMora = DateDiff(DateInterval.Day, CDate(pFechaExigibilidad), CDate(pFechaPago))

        Return diasMora

    End Function

    'Calcular Intereses de Mora
    Public Function CalcularInteresesMoras(ByVal PValorDeuda As Double, ByVal pDiasMoras As Integer) As Double
        Dim tasas() As String = CargarTasas()

        Dim InteresesMora As Double = 0
        InteresesMora = PValorDeuda * (CDec(tasas(0)) / 365) * pDiasMoras
        Return InteresesMora

    End Function


    'Cargar Tasas de intereses
    Public Function CargarTasas() As String()
        Dim Tasas(1) As String
        Dim Table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT * FROM PORCENTAJE_TASA_MULTA", Funciones.CadenaConexion)
        _Adap.Fill(Table)

        Tasas(0) = Math.Round(CDec(Table.Rows(0).Item("p_anual")), 4) '-- Tasa anual
        Tasas(1) = Math.Round(CDec(Table.Rows(0).Item("p_mensual")), 4) '-- Tasa Mensual

        Return Tasas

    End Function


End Module
