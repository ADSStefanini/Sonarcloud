Imports System.Data.SqlClient
Partial Public Class iyc_ingresos_declarados
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
        'txtFecIni.Text = dtEncabezado.Rows(0).Item(4).ToString
        Dim FecIni As Date = dtEncabezado.Rows(0).Item(4)
        txtFecIni.Text = FecIni.ToString("dd/MM/yyyy")
        txtActividad.Text = dtEncabezado.Rows(0).Item(5).ToString
    End Sub

    Private Sub LoadGridDeclaraciones(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        cmd = "SELECT DecMaeNum, DecVig, TipLla, ActCod, DecIng, DecOtrIng, DecDed, DecTar, DecTip, DecNum " & _
                "FROM ACTDECL1 WHERE DecMaeNum = '" & pPlaca & "' " & _
                "ORDER BY DecMaeNum, DecVig, DecTip, DecNum, TipLla, ActCod"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtDeclaraciones As New DataTable
        Adaptador.Fill(dtDeclaraciones)

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
End Class