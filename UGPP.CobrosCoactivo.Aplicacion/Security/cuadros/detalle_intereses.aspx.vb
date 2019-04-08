Imports System.Data.SqlClient
Partial Public Class detalle_intereses
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            txtInteres.Text = _CalcularIntereses(CDbl(Request("deuda")), CDate(Request("fecha_deuda")), CDate(Request("fecha_corte")))
            Txtexpediente.Text = Request("expediente")
            txtFechaDeuda.Text = CDate(Request("fecha_deuda"))
            txtFechaActual.Text = CDate(Request("fecha_corte"))
            cedula.Value = Request("cedula")
            deuda.Value = Request("deuda")
        End If
    End Sub
    Private Function _CalcularIntereses(ByVal deuda As Double, ByVal fecha_deuda As Date, ByVal fecha_corte As Date) As Integer
        Dim table As DataTable = New DataTable()
        Dim acumulado As Integer = 0
        Dim dias As Integer = 0
        Dim intereses As Integer = 0

        Dim Con As Integer = 0
        Dim _Adap As New SqlDataAdapter("SELECT * FROM CALCULO_INTERESES_PARAFISCALES WHERE DESDE >= '" & CDate(fecha_deuda) & "'", Funciones.CadenaConexion)
        _Adap.Fill(table)

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



        ' Alterar data table
        For Each row As DataRow In table.Rows


            If fecha_deuda > CDate(row("hasta")) Then
                dias = 0
            Else
                'Congelar interes a la fecha actual
                If fecha_corte < CDate(row("hasta")) Then
                    row("hasta") = fecha_corte
                End If

                'Calcular dias de moras
                dias = DateDiff(DateInterval.Day, CDate(fecha_deuda), CDate(row("hasta"))) - acumulado

                'Calcular los interes de los dias de moras
                row("Interes") = Math.Round(deuda * row("t_diaria") * dias)

                'Acumular dias
                acumulado += dias

                'Alterar el dia de la tabla
                row("Dias") = dias

                'sumatoria de intereses
                intereses += row("Interes")

                row("n_saldo") = deuda + intereses

            End If
        Next
        Gridinteres.DataSource = table
        Gridinteres.DataBind()



        Return intereses
    End Function
End Class