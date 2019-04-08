Imports System.Data.SqlClient
Partial Public Class iyc_estado_cuenta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Placa As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Placa = Request.QueryString("idplaca")

        If Not Page.IsPostBack Then
            MostrarEncabezado(Placa)
            LoadGridLiquidacion(Placa)
        End If
    End Sub

    Private Sub MostrarEncabezado(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        cmd = "SELECT MaeEstAct, MaeNum, MaeFecIni, MaeProCod, MaeFecUltP, MaeProNom, MaeValUltP, MaeRecUltP, MAePerHas," & _
            "MaeSubHas, MaeActBaz, MaeMatMer, MaeFecCam, MaeDirNot, MaeDir, MaeDes, MaeExped, MaeCos, MaeMult, MaeEjeEst " & _
            "FROM MAEIC WITH (NOLOCK) " & _
            "WHERE (MaeNum = '" & pPlaca & "') AND (MaeEstAct < 3) ORDER BY MaeNum"         
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEncabezado As New DataTable
        Adaptador.Fill(dtEncabezado)

        'Colocar los datos de los campos en los textbox
        Dim Fecha As Date = Date.Now()
        txtFecha.Text = Fecha.ToString("dd/MM/yyyy")
        txtIdPlaca.Text = CType(dtEncabezado.Rows(0).Item(1), Integer).ToString("000000")
        txtIdentificacion.Text = dtEncabezado.Rows(0).Item(3).ToString
        txtNombre.Text = dtEncabezado.Rows(0).Item(5).ToString
        txtCamaraCom.Text = dtEncabezado.Rows(0).Item(11).ToString
        txtActividad.Text = dtEncabezado.Rows(0).Item(15).ToString
        txtDireccion.Text = dtEncabezado.Rows(0).Item(14).ToString
        txtDirNotif.Text = dtEncabezado.Rows(0).Item(13).ToString

        'txtFecIni.Text = dtEncabezado.Rows(0).Item(4).ToString
        Dim FecUltPag As Date = dtEncabezado.Rows(0).Item(4)
        txtUltPag.Text = FecUltPag.ToString("dd/MM/yyyy") & " " & dtEncabezado.Rows(0).Item(6).ToString & " " & dtEncabezado.Rows(0).Item(7).ToString

        Dim FechaIni As Date = dtEncabezado.Rows(0).Item(2)
        txtFechaIni.Text = FechaIni.ToString("dd/MM/yyyy")

        txtAnioPagHasta.Text = dtEncabezado.Rows(0).Item(8).ToString
        txtMesPagHasta.Text = dtEncabezado.Rows(0).Item(9).ToString
    End Sub

    Private Sub LoadGridLiquidacion(ByVal pPlaca As String)
        Dim cmd, cmd2 As String
        Dim X, Y As Integer
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Datatable de liquidacion
        cmd = "SELECT PerCod AS anio, MAX(PerSubCod) AS E, 'P' AS T, SUM(LiqTot) AS deuda, 99999999 AS interes, " & _
                    "99999999 AS sancion,99999999 AS saldopend,99999999 AS total,99999999 AS acumulado " & _
                    "FROM LIQUIDAD WITH (NOLOCK) " & _
                    "WHERE LiqModCod = 2 AND LiqGen = '002411' AND PerCod > 0 " & _
                    "GROUP BY PerCod ORDER BY PerCod"

        Dim Adaptador1 As New SqlDataAdapter(cmd, cnx)
        Dim dtLiquidacion As New DataTable
        Adaptador1.Fill(dtLiquidacion)

        'Colocar los valores de las columnas numericas en cero
        For X = 0 To dtLiquidacion.Rows.Count - 1
            dtLiquidacion.Rows(X).Item(4) = 0
            dtLiquidacion.Rows(X).Item(5) = 0
            dtLiquidacion.Rows(X).Item(6) = 0
            dtLiquidacion.Rows(X).Item(7) = 0
            dtLiquidacion.Rows(X).Item(8) = 0
        Next

        'Datatable de sanciones
        cmd2 = "SELECT T1.ModCod, T1.LiqNum, T2.MunCod, T2.LiqEst, T1.LiqVal, T2.PerCod, T1.ConCod, T2.LiqGen, T2.LiqModCod " & _
                "FROM (LIQUIDA2 T1 WITH (FASTFIRSTROW NOLOCK) INNER JOIN LIQUIDAD T2 WITH (NOLOCK) ON T2.LiqNum = T1.LiqNum) " & _
                "WHERE  (T2.LiqModCod = 2) AND (T2.LiqGen = '" & pPlaca & "') AND " & _
                "(T1.ConCod = 7 or T1.ConCod =   8 or T1.ConCod = 10 OR " & _
                "T1.ConCod = 12 or T1.ConCod = 13 or T1.ConCod = 14 OR T1.ConCod = 19) AND " & _
                "(T1.LiqVal > 0) AND (T2.MunCod = 1) ORDER BY T1.LiqNum, T1.ModCod, T1.ConCod"
        Dim Adaptador2 As New SqlDataAdapter(cmd2, cnx)
        Dim dtSanciones As New DataTable
        Adaptador2.Fill(dtSanciones)

        'Barrer datatable de sanciones y actualizar datatable de liquidacion 
        Dim anio As Integer
        Dim valor As Decimal
        For X = 0 To dtSanciones.Rows.Count - 1
            anio = dtSanciones.Rows(X).Item(5)
            valor = dtSanciones.Rows(X).Item(4)
            'Buscar el año en el datatable dtLiquidacion y actualizar el campo sancion
            For Y = 0 To dtLiquidacion.Rows.Count - 1
                If dtLiquidacion.Rows(Y).Item(0) = anio Then
                    dtLiquidacion.Rows(Y).Item(5) = valor
                    dtLiquidacion.Rows(Y).Item(3) = dtLiquidacion.Rows(Y).Item(3) - valor
                    Exit For
                End If
            Next
        Next

        GridLiquidacion.DataSource = dtLiquidacion
        GridLiquidacion.DataBind()
    End Sub

    Private Sub GridLiquidacion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridLiquidacion.PageIndexChanging
        Try
            GridLiquidacion.PageIndex = e.NewPageIndex
            LoadGridLiquidacion(txtIdPlaca.Text.Trim)
        Catch ex As Exception

        End Try
    End Sub
End Class