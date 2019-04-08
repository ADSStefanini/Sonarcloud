Imports System.Data.SqlClient
Partial Public Class iyc_cancel_traspasos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Placa As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Placa = Request.QueryString("idplaca")

        If Not Page.IsPostBack Then
            txtIdPlaca.Text = Placa
            LoadGridCancelaciones(Placa)
        End If
    End Sub

    Private Sub LoadGridCancelaciones(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        'SELECT MaeNum, CanTip, CanNroN, CanFecSol, CanFecTer, CanDirMat, CanEst FROM(CANTRAS) WHERE MaeNum = '032115' ORDER BY MaeNum, CanNroN
        'Grid de Cancelaciones
        cmd = "SELECT MaeNum, CASE WHEN CanTip = 1 THEN 'Cancelación' WHEN CanTip = 2 THEN 'Traspaso' WHEN CanTip = 3 THEN 'Cambio Razón' WHEN CanTip = 9 THEN 'No determinado' END AS tipo, CanNroN, CanFecSol, CanFecTer, CanDirMat, CASE WHEN CanEst = 1 THEN 'En estudio' WHEN CanEst = 2 THEN 'Cumple' WHEN CanEst = 3 THEN 'No cumple' END AS estado " & _
                "FROM CANTRAS WHERE MaeNum = '" & pPlaca & "' " & _
                "ORDER BY MaeNum,CanNroN"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtCancelaciones As New DataTable
        Adaptador.Fill(dtCancelaciones)

        If dtCancelaciones.Rows.Count > 0 Then
            'Control PLACA
            'txtIdPlaca.Text = dtCancelaciones.Rows(0).Item(0).ToString

            'GridPropietarios
            GridCancelaciones.DataSource = dtCancelaciones
            GridCancelaciones.DataBind()
        End If
    End Sub


    Private Sub GridCancelaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridCancelaciones.PageIndexChanging
        Try
            GridCancelaciones.PageIndex = e.NewPageIndex
            LoadGridCancelaciones(txtIdPlaca.Text.Trim)
        Catch ex As Exception

        End Try
    End Sub
End Class