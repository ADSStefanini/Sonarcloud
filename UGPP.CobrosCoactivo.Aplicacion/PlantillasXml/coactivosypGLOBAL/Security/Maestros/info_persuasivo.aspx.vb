Imports System.Data.SqlClient

Partial Public Class info_persuasivo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            'Puts the previous state of the txtSearchNroExp field done when the user has searched and moved to the EditPERSUASIVO page and then came back
            txtSearchNroExp.Text = Session("PERSUASIVO.txtSearchNroExp")

            'Puts the previous state of the txtToFecOfi1 field done when the user has searched and moved to the EditPERSUASIVO page and then came back
            txtToFecOfi1.Text = Session("PERSUASIVO.txtToFecOfi1")

            'Puts the previous state of the txtToFecOfi2 field done when the user has searched and moved to the EditPERSUASIVO page and then came back
            txtToFecOfi2.Text = Session("PERSUASIVO.txtToFecOfi2")
 
            BindGrid()
            llenarcombo()

            'End If - if IsPostBack equals false
        End If
    End Sub


    'Display's the grid with the search criteria.
    Private Function BindGrid() As Reportes_Admistratiivos

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@NroExp", txtSearchNroExp.Text)

        If txtToFecOfi1.Text.Length > 0 Then
            Command.Parameters.AddWithValue("@ToFecOfi1", txtToFecOfi1.Text)

        End If

        If txtToFecOfi2.Text.Length > 0 Then
            Command.Parameters.AddWithValue("@ToFecOfi2", txtToFecOfi2.Text)

        End If
        Dim DataAdapter As New SqlDataAdapter(Command)

        Dim repo As New Reportes_Admistratiivos
        Dim DataSet As New DataSet


        DataAdapter.Fill(DataSet)
        DataAdapter.Fill(repo.PERSUASIVO)
        grd.DataSource = DataSet
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()
        Return repo
    End Function

    Private Function GetSQL() As String
        Dim sql As String = ""
        sql = sql & "select [dbo].[PERSUASIVO].* from [dbo].[PERSUASIVO]"
        Dim WhereClause As String = ""
        If txtSearchNroExp.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PERSUASIVO].[NroExp] = @NroExp"

        End If

        If txtToFecOfi1.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PERSUASIVO].[FecOfi1] = @ToFecOfi1"

        End If
        
        If txtToFecOfi2.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PERSUASIVO].[FecOfi2] = @ToFecOfi2"

        End If
        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
            sql = sql & "where " & WhereClause

        End If

        If Len(Session("PERSUASIVO.SortExpression")) > 0 Then
            sql = sql & " order by " & Session("PERSUASIVO.SortExpression") & " " & Session("PERSUASIVO.SortDirection")

        End If
        Return sql

    End Function

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        BindGrid()

        Session("PERSUASIVO.txtSearchNroExp") = txtSearchNroExp.Text
        Session("PERSUASIVO.txtToFecOfi1") = txtToFecOfi1.Text
        Session("PERSUASIVO.txtToFecOfi2") = txtToFecOfi2.Text
    End Sub


    Protected Sub grd_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grd.PageIndexChanging
        grd.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub llenarcombo()
        Dim dt As New DataTable
        dt.Columns.Add("Codigo", System.Type.GetType("System.String"))
        dt.Columns.Add("Descripcion", System.Type.GetType("System.String"))

        Dim dr As DataRow = dt.NewRow
        dr("Codigo") = 1
        dr("Descripcion") = "INFORME PRIMER OFICIO PERSUASIVO"
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("Codigo") = 2
        dr("Descripcion") = "INFORME SEGUNDO OFICIO PERSUASIVO"
        dt.Rows.Add(dr)

        ddlinfo.DataValueField = "Codigo"
        ddlinfo.DataTextField = "Descripcion"
        ddlinfo.DataSource = dt
        ddlinfo.DataBind()
    End Sub

    Protected Sub btnprocesar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnprocesar.Click
        Dim cr As CrystalDecisions.CrystalReports.Engine.ReportDocument

        If ddlinfo.SelectedValue = 1 Then
            cr = New INFORME_PRIMER_OFICIO
        Else
            cr = New INFORME_SEGUNDO_OFICIO
        End If

        Funciones.Exportar(Me, cr, BindGrid, "Infome.pdf", "", Nothing)
    End Sub
End Class
