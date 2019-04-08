Imports System.Data.SqlClient
Partial Public Class iyc_declaraciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Placa As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Placa = Request.QueryString("idplaca")

        If Not Page.IsPostBack Then
            MostrarEncabezado(Placa)
            LoadGridDeclaraciones(Placa)
        End If
    End Sub
    Private Sub MostrarEncabezado(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        cmd = "SELECT MaeNum AS placa,MaeProCod AS nit_cedula,MaeProNom AS nombre_contribuyente," & _
                "MaeDir AS direccion_Establecimiento,MaeFecIni AS Fec_Ini_Act," & _
                "MaeDes AS 'Descrip. Actividad' " & _
                "FROM MAEIC WHERE MaeNum = '" & pPlaca & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEncabezado As New DataTable
        Adaptador.Fill(dtEncabezado)

        'Colocar los datos de los campos en los textbox
        txtIdPlaca.Text = CType(dtEncabezado.Rows(0).Item(0), Integer).ToString("000000")
        txtIdentificacion.Text = dtEncabezado.Rows(0).Item(1).ToString
        txtNombre.Text = dtEncabezado.Rows(0).Item(2).ToString
        txtDireccion.Text = dtEncabezado.Rows(0).Item(3).ToString
        txtActividad.Text = dtEncabezado.Rows(0).Item(5).ToString

        'txtFecIni.Text = dtEncabezado.Rows(0).Item(4).ToString
        Dim FecIni As Date = dtEncabezado.Rows(0).Item(4)
        txtFecIni.Text = FecIni.ToString("dd/MM/yyyy")

    End Sub

    Private Sub LoadGridDeclaraciones(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        cmd = "SELECT DepMatNum, DepVig, DepMaeNom, DepMaeDir, DepFecP, DepTip, DepNum " & _
                "FROM DECLARC WHERE DepMatNum = '" & pPlaca & "' " & _
                "ORDER BY DepMatNum, DepVig, DepTip, DepNum"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtDeclaraciones As New DataTable
        Adaptador.Fill(dtDeclaraciones)

        'Control PLACA
        'txtIdPlaca.Text = dtDeclaraciones.Rows(0).Item(0).ToString

        'GridPropietarios
        GridDeclaraciones.DataSource = dtDeclaraciones
        GridDeclaraciones.DataBind()
    End Sub

    Private Sub GridDeclaraciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDeclaraciones.PageIndexChanging
        Try
            GridDeclaraciones.PageIndex = e.NewPageIndex
            LoadGridDeclaraciones(txtIdPlaca.Text.Trim)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnDeclaraciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeclaraciones.Click
        Response.Redirect("iyc_ingresos_declarados.aspx?idplaca=" & txtIdPlaca.Text.Trim)
    End Sub
End Class