Imports System.Data.SqlClient
Partial Public Class salfav01
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load      
        Dim cnx As String = Funciones.CadenaConexionUnion

        If Not Page.IsPostBack Then
            LoadGridSaldosFav()
        End If
    End Sub

    Private Sub LoadGridSaldosFav()
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de Predio Matriz
        cmd = "SELECT SalNum, SalVig, SalRecGen, SalResTipG, SalResVigG, SalResNumG, SalFav, SalCon, SalEst, SalFec, " & _
            "SalCodUsu, SalRecApl, SalResTipA, SalResVigA, SalResNumA, MunCod, SalPreNum AS SalPreNum, ModCod, " & _
            "SalIndNum AS SalIndNum " & _
            "FROM salfavco"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtSaldosFav As New DataTable
        Adaptador.Fill(dtSaldosFav)
        GridSaldosFav.DataSource = dtSaldosFav
        GridSaldosFav.DataBind()
    End Sub

    Private Sub GridSaldosFav_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridSaldosFav.PageIndexChanging
        GridSaldosFav.PageIndex = e.NewPageIndex
        LoadGridSaldosFav()
    End Sub
End Class