Imports System.Data.SqlClient
Partial Public Class consulta_predial02
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Predio As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Predio = Request.QueryString("idpred")

        If Not Page.IsPostBack Then
            LoadGridAvaluos(Predio)
        End If
    End Sub
    Private Sub LoadGridAvaluos(ByVal pPredio As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de Avaluos
        cmd = "SELECT ModCod, PreNum, MunCod, PreCarVal, PerCod, CarCod " & _
            "FROM predios2 " & _
            "WHERE (MunCod = 1 and PreNum = '" & pPredio & "') AND (CarCod = 3) " & _
            "ORDER BY MunCod, PreNum, ModCod, PerCod, CarCod"
        Dim Adaptador2 As New SqlDataAdapter(cmd, cnx)
        Dim dtAvaluos As New DataTable
        Adaptador2.Fill(dtAvaluos)
        GridAvaluos.DataSource = dtAvaluos
        GridAvaluos.DataBind()

        txtPredial.Text = dtAvaluos.Rows(0).Item(1).ToString
    End Sub
End Class