Imports System.Data.SqlClient
Partial Public Class consulta_predial03
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Predio As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Predio = Request.QueryString("idpred")

        If Not Page.IsPostBack Then
            LoadGridPagos(Predio)
        End If
    End Sub
    Private Sub LoadGridPagos(ByVal pPredio As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de Pagos
        cmd = "SELECT PagCtb, PagEst, TpaCod, PagSubHas, PagPerHas, PagSubDes, PagPerDes, PagTot, PagFecLiq, PagFec, PagNroRec, " & _
            "'verforpag.aspx?idpag=' + RTRIM(LTRIM(PagNroRec)) AS enlace FROM pagos " & _
            "WHERE PagCtb = '" & pPredio & "' ORDER BY PagCtb, PagFecLiq "
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtPagos As New DataTable
        Adaptador.Fill(dtPagos)
        GridAvaluos.DataSource = dtPagos
        GridAvaluos.DataBind()

        If dtPagos.Rows.Count = 0 Then
            txtPredial.Text = pPredio
            Validator.ErrorMessage = "El predio solicitado no registra pagos"
            Validator.IsValid = False
        Else
            txtPredial.Text = dtPagos.Rows(0).Item(0).ToString
        End If

    End Sub
End Class