Imports System.Data.SqlClient
Partial Public Class consulta_predial01
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Predio As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Predio = Request.QueryString("idpred")        

        If Not Page.IsPostBack Then
            LoadGridPropietarios(Predio)
            LoadGridCaracteristicas(Predio)
            LoadGridMatriz(Predio)
            LoadGridResols(Predio)
        End If
    End Sub

    Private Sub LoadGridPropietarios(ByVal pPredio As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de Propietarios
        cmd = "SELECT T1.PreNum, T2.PrsNom, T1.PreEstPer, T2.PrsTipDoc, T2.PrsDoc, T1.PrsCod, " & _
                    "T1.MunCod, T3.PreMatInm, T3.PreDirCob, T3.PreRecUltP, T3.PreFecUltP, T3.PreValUltP, " & _
                    "T3.PrePerHas, T3.PreSubHas, T3.PrePerDes, T3.PreSubDes, T3.PrePerCan, " & _
                    "T3.PreSubCan, T3.PreEjeFis, T3.PreAcuPag, T3.PreEstChe, T3.PreEstExe, " & _
                    "T3.PreEstMun, T3.PreDir " & _
                "FROM ((PREDIOS3 T1 INNER JOIN PERSONAS T2 " & _
                "ON T2.PrsCod = T1.PrsCod) INNER JOIN PREDIOS T3 " & _
                "ON T3.MunCod = T1.MunCod AND T3.PreNum = T1.PreNum) " & _
                "WHERE T1.MunCod =   1 and T1.PreNum = '" & pPredio & "' " & _
                "ORDER BY T1.MunCod, T1.PreNum, T1.PrsCod"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtPropietarios As New DataTable
        Adaptador.Fill(dtPropietarios)

        'Colocar los datos de los campos en los textbox
        txtIdMunicipio.Text = CType(dtPropietarios.Rows(0).Item(6), Integer).ToString("000")
        txtPredial.Text = dtPropietarios.Rows(0).Item(0).ToString
        txtMatInm.Text = dtPropietarios.Rows(0).Item(7).ToString
        txtDireccion.Text = dtPropietarios.Rows(0).Item(23).ToString
        txtDirCobro.Text = dtPropietarios.Rows(0).Item(8).ToString
        txtAnioDesde.Text = dtPropietarios.Rows(0).Item(14).ToString
        txtMesDesde.Text = dtPropietarios.Rows(0).Item(15).ToString
        txtAnioPago.Text = dtPropietarios.Rows(0).Item(12).ToString
        txtMesPago.Text = dtPropietarios.Rows(0).Item(13).ToString

        'Campos del cuadro 2
        txtPredioMun.Text = dtPropietarios.Rows(0).Item(22).ToString
        TextBox1.Text = dtPropietarios.Rows(0).Item(18).ToString
        TextBox2.Text = dtPropietarios.Rows(0).Item(19).ToString
        TextBox4.Text = dtPropietarios.Rows(0).Item(20).ToString
        TextBox3.Text = dtPropietarios.Rows(0).Item(21).ToString
        'Predio cancelado
        TextBox5.Text = dtPropietarios.Rows(0).Item(16).ToString
        TextBox6.Text = dtPropietarios.Rows(0).Item(17).ToString

        'Campos del cuadro 3        
        TextBox7.Text = dtPropietarios.Rows(0).Item(10).ToString 'Fecha ultimo pago
        TextBox8.Text = dtPropietarios.Rows(0).Item(11).ToString 'valor ultimo pago
        TextBox9.Text = dtPropietarios.Rows(0).Item(9).ToString 'valor ultimo pago

        'GridPropietarios
        GridPropietarios.DataSource = dtPropietarios
        GridPropietarios.DataBind()
    End Sub
    Private Sub LoadGridCaracteristicas(ByVal pPredio As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de caracteristicas
        cmd = "SELECT T1.ModCod, T1.PreNum, T1.MunCod, T2.CarEst, T1.PrefecAct, T1.PreCarSub, " & _
                "T1.PreCarEst, T1.PreCarVal, T2.CarDes, '00'+CAST(t1.CarCod AS CHAR) AS CarCod, T1.PerCod " & _
                "FROM (PREDIOS2 T1 INNER JOIN CARACTER T2 " & _
                "ON T2.CarCod = T1.CarCod) " & _
                "WHERE (T1.MunCod = 1 and T1.PreNum = '" & pPredio & "') AND " & _
                "(T1.PerCod >= 2001) AND (T1.CarCod   > 0) " & _
                "ORDER BY T1.MunCod, T1.PreNum, T1.ModCod, T1.PerCod, T1.CarCod"
        Dim Adaptador2 As New SqlDataAdapter(cmd, cnx)
        Dim dtCaracteristicas As New DataTable
        Adaptador2.Fill(dtCaracteristicas)
        GridCaracteristicas.DataSource = dtCaracteristicas
        GridCaracteristicas.DataBind()
    End Sub

    Private Sub LoadGridMatriz(ByVal pPredio As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de Predio Matriz
        cmd = "SELECT PreNum, MunCod, MatFecApl, MatNumRes, MatVigRes, MatTipRes, MatPreNum " & _
            "FROM predios1 " & _
            "WHERE MunCod = 1 and PreNum = '" & pPredio & "' " & _
            "ORDER BY MunCod, PreNum, MatPreNum"
        Dim Adaptador3 As New SqlDataAdapter(cmd, cnx)
        Dim dtMatriz As New DataTable
        Adaptador3.Fill(dtMatriz)
        GridMatriz.DataSource = dtMatriz
        GridMatriz.DataBind()
    End Sub
    Private Sub LoadGridResols(ByVal pPredio As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de Resoluciones aplicadas
        cmd = "SELECT MunCod, PreResFecA, ResNum, ResVig, ResTip, PreNum " & _
            "FROM PREDIOS4 " & _
            "WHERE MunCod = 1 and PreNum = '" & pPredio & "' " & _
            "ORDER BY MunCod, PreNum, ResTip,   ResVig, ResNum"
        Dim Adaptador4 As New SqlDataAdapter(cmd, cnx)
        Dim dtResols As New DataTable
        Adaptador4.Fill(dtResols)
        GridResolucionesAplicadas.DataSource = dtResols
        GridResolucionesAplicadas.DataBind()
    End Sub

    Private Sub GridPropietarios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridPropietarios.PageIndexChanging
        GridPropietarios.PageIndex = e.NewPageIndex
        LoadGridPropietarios(txtPredial.Text.Trim)
    End Sub

    Private Sub GridCaracteristicas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridCaracteristicas.PageIndexChanging
        GridCaracteristicas.PageIndex = e.NewPageIndex
        LoadGridCaracteristicas(txtPredial.Text.Trim)
    End Sub

    Private Sub GridResolucionesAplicadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridResolucionesAplicadas.PageIndexChanging
        GridResolucionesAplicadas.PageIndex = e.NewPageIndex
        LoadGridResols(txtPredial.Text.Trim)
    End Sub
End Class