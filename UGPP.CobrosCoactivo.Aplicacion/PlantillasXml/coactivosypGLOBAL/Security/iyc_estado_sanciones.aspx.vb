Imports System.Data.SqlClient
Partial Public Class iyc_estado_sanciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Placa As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Placa = Request.QueryString("idplaca")

        If Not Page.IsPostBack Then
            MostrarEncabezado(Placa)
            LoadGridSanciones(Placa)
        End If
    End Sub
    Private Sub MostrarEncabezado(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        cmd = "SELECT MaeNum, MaeProNom, MaeProCod, MaeDir, MaeEstAct " & _
                "FROM MAEIC WHERE MaeNum = '" & pPlaca & "' AND ([MaeEstAct] < 3) ORDER BY [MaeNum]"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEncabezado1 As New DataTable
        Adaptador.Fill(dtEncabezado1)

        'Colocar los datos de los campos en los textbox
        txtIdPlaca.Text = CType(dtEncabezado1.Rows(0).Item(0), Integer).ToString("000000")
        txtNombre.Text = dtEncabezado1.Rows(0).Item(1).ToString
        txtCedula.Text = dtEncabezado1.Rows(0).Item(2).ToString
        txtDireccion.Text = dtEncabezado1.Rows(0).Item(3).ToString

        'Emplazamiento por no declarar
        cmd = "SELECT MaeNum, FisFecIni, FisResSan, FisFecFin, FisFecImp, FisNro, FisVigSan, FisEst, FisSwiSan, FisCon FROM FISCAL " & _
                "WHERE MaeNum = '" & pPlaca & "' ORDER BY MaeNum, FisCon"
        Dim Adaptador2 As New SqlDataAdapter(cmd, cnx)
        Dim dtEncabezado2 As New DataTable
        Adaptador2.Fill(dtEncabezado2)

        'Si el campo FISCAL.FisResSan <> 0 => tiene sanciones
        'FISCAL.FisResSan = Nro. Resolucion Sancion
        'FISCAL.FisFecImp = Fecha de sanción
        'FISCAL.FisFecFin = Fecha Limite
        Dim fila As Integer = dtEncabezado2.Rows.Count - 1
        If dtEncabezado2.Rows.Count > 0 Then

            'txtEmplazNoDec.Text = "EMPLAZADO". Depende del campo Fiscal.FisESt
            Dim FisESt As String = dtEncabezado2.Rows(fila).Item(7).ToString
            If FisESt = "0" Then
                txtEmplazNoDec.Text = "EMPLAZADO"
            Else
                txtEmplazNoDec.Text = "NO EMPLAZADO"
            End If

            'txtSancionND.Text = "SANCIONADO". Depende del campo Fiscal.FisSwiSan
            Dim FisSwiSan As String = dtEncabezado2.Rows(fila).Item(8).ToString
            If FisSwiSan = "1" Or FisSwiSan = "2" Then
                txtSancionND.Text = "SANCIONADO"
            Else
                txtSancionND.Text = "NO SANCIONADO"
            End If

            'If dtEncabezado2.Rows(fila).Item(2).ToString <> 0 Then
            txtNumResolSan.Text = dtEncabezado2.Rows(fila).Item(2).ToString

            'txtFechaSancionND.Text = dtEncabezado2.Rows(0).Item(4).ToString
            Dim FechaSancionND As Date = dtEncabezado2.Rows(fila).Item(4)
            txtFechaSancionND.Text = FechaSancionND.ToString("dd/MM/yyyy")
            If Mid(txtFechaSancionND.Text, 7, 4) = "1753" Then
                txtFechaSancionND.Text = ""
            End If

            'txtFechaLim.Text = dtEncabezado2.Rows(0).Item(3).ToString
            Dim FechaLim As Date = dtEncabezado2.Rows(fila).Item(3)
            txtFechaLim.Text = FechaLim.ToString("dd/MM/yyyy")
            If Mid(txtFechaLim.Text, 7, 4) = "1753" Then
                txtFechaLim.Text = ""
            End If

            txtEmplazCorregir.Text = "NO EMPLAZADO"
            '-----------------------------------------------------------------------------------------------------------------------'
            txtNumResolEND.Text = dtEncabezado2.Rows(fila).Item(5).ToString

            'txtFecEmplazND.Text = dtEncabezado2.Rows(0).Item(1).ToString
            Dim FecEmplazND As Date = dtEncabezado2.Rows(fila).Item(1)
            txtFecEmplazND.Text = FecEmplazND.ToString("dd/MM/yyyy")

            'Detalle de las vigencias
            cmd = "SELECT MaeNum, FisVigEmp, FisCon FROM FISCAL1 WHERE MaeNum = '" & pPlaca & "' ORDER BY MaeNum, FisCon, FisVigEmp"
            Dim Adaptador3 As New SqlDataAdapter(cmd, cnx)
            Dim dtEncabezado3 As New DataTable
            Dim vigencias As String = ""
            Adaptador3.Fill(dtEncabezado3)
            If dtEncabezado3.Rows.Count > 0 Then
                For x = 0 To dtEncabezado3.Rows.Count - 1
                    vigencias = vigencias & " " & dtEncabezado3.Rows(x).Item(1).ToString
                Next
            End If
            txtVigenciasEND.Text = vigencias
            '-----------------------------------------------------------------------------------------------------------------------'
            'End If
        End If

    End Sub

    Private Sub LoadGridSanciones(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        cmd = "SELECT T1.ModCod, T1.LiqNum, T2.MunCod, T2.LiqEst, T1.LiqVal, T2.PerCod, T1.ConCod, T2.LiqGen, T2.LiqModCod " & _
                "FROM (LIQUIDA2 T1 WITH (FASTFIRSTROW NOLOCK) INNER JOIN LIQUIDAD T2 WITH (NOLOCK) ON T2.LiqNum = T1.LiqNum) " & _
                "WHERE  (T2.LiqModCod = 2) AND (T2.LiqGen = '" & pPlaca & "') AND " & _
                "(T1.ConCod = 7 or T1.ConCod =   8 or T1.ConCod = 10 OR " & _
                "T1.ConCod = 12 or T1.ConCod = 13 or T1.ConCod = 14 OR T1.ConCod = 19) AND " & _
                "(T1.LiqVal > 0) AND (T2.MunCod = 1) ORDER BY T1.LiqNum, T1.ModCod, T1.ConCod"

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtSanciones As New DataTable
        Adaptador.Fill(dtSanciones)

        GridSanciones.DataSource = dtSanciones
        GridSanciones.DataBind()
    End Sub


    Private Sub GridSanciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridSanciones.PageIndexChanging
        Try
            GridSanciones.PageIndex = e.NewPageIndex
            LoadGridSanciones(txtIdPlaca.Text.Trim)
        Catch ex As Exception

        End Try
    End Sub
End Class