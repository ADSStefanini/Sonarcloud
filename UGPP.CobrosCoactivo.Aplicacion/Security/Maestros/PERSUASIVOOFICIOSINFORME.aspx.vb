Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class PERSUASIVOOFICIOSINFORME
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilitan los campos para este perfil
                txtSearchFechaInicial.Enabled = False
                txtSearchFechaFinal.Enabled = False
                cmdSearch.Enabled = False
                cmdExportar.Enabled = False
            Else
                'Se habilitan los campos para usuarios con perfil diferente a Verificador de Pagos
                txtSearchFechaInicial.Enabled = True
                txtSearchFechaFinal.Enabled = True
                cmdSearch.Enabled = True
                cmdExportar.Enabled = True
            End If

            txtSearchFechaInicial.Text = Session("PERSUASIVOOFICIOS.txtToFecha")
            txtSearchFechaFinal.Text = Session("PERSUASIVOOFICIOS.txtFromFecha")
        End If
    End Sub

    Protected Sub imgBtnBorraFechaIni_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaIni.Click
        txtSearchFechaInicial.Text = ""
    End Sub

    Protected Sub imgBtnBorraFechaFin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaFin.Click
        txtSearchFechaFinal.Text = ""
    End Sub

    Private Sub BindGrid()
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql

        If txtSearchFechaInicial.Text.Length > 0 Then
            Dim parameter As New SqlParameter("@FromFecOfi", SqlDbType.DateTime, 10)
            parameter.Value = Date.Parse(txtSearchFechaInicial.Text)
            parameter.IsNullable = True
            Command.Parameters.Add(parameter)
            ' Command.Parameters.Add(New SqlParameter("@FromFecha", txtSearchFechaInicial.Text))
            ' Command.Parameters.AddWithValue("@FromFecha", txtSearchFechaInicial.Text)
        End If

        If txtSearchFechaFinal.Text.Length > 0 Then
            Dim parameter As New SqlParameter("@ToFecOfi", SqlDbType.DateTime, 10)
            parameter.Value = Date.Parse(txtSearchFechaFinal.Text)
            parameter.IsNullable = True
            Command.Parameters.Add(parameter)
            ' Command.Parameters.AddWithValue("@ToFecha", txtSearchFechaFinal.Text)
        End If

        If String.IsNullOrEmpty(Request("pExpediente")) Then
            Command.Parameters.AddWithValue("@numExpediente", String.Empty)
        Else
            Command.Parameters.AddWithValue("@numExpediente", Request("pExpediente"))
        End If

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        lblRecordsFound.Text = "Registros encontrados " & grd.Rows.Count
        Connection.Close()
    End Sub

    Private Function GetSQL() As String

        Dim sql As String = ""
        sql = sql & "SELECT * FROM PERSUASIVOOFICIOS "
        Dim WhereClause As String = ""

        If txtSearchFechaInicial.Text.Length > 0 Then
            WhereClause = WhereClause & " and PERSUASIVOOFICIOS.FecOfi >= @FromFecOfi"
        End If

        If txtSearchFechaFinal.Text.Length > 0 Then
            WhereClause = WhereClause & " and PERSUASIVOOFICIOS.FecOfi <= @ToFecOfi"
        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
            sql = sql & "where " & WhereClause
        End If

        ' Solo en el expediente que se esta trabajando
        sql = sql & " AND PERSUASIVOOFICIOS.NroExp = CASE WHEN @numExpediente IS NOT NULL THEN @numExpediente ELSE PERSUASIVOOFICIOS.NroExp END "

        If Len(Session("PERSUASIVOOFICIOS.SortExpression")) > 0 Then
            sql = sql & " order by " & Session("PERSUASIVOOFICIOS.SortExpression") & " " & Session("PERSUASIVOOFICIOS.SortDirection")
        End If

        Return sql

    End Function

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        If String.IsNullOrEmpty(txtSearchFechaInicial.Text) = False And String.IsNullOrEmpty(txtSearchFechaFinal.Text) = False Then
            BindGrid()

            Session("PERSUASIVOOFICIOS.txtToFecha") = txtSearchFechaInicial.Text
            Session("PERSUASIVOOFICIOS.txtFromFecha") = txtSearchFechaFinal.Text
        End If
    End Sub

    Protected Sub cmdExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdExportar.Click
        'Instanciar clase de metodos globales
        Dim MTG As New MetodosGlobalesCobro

        'Convertir Gridview a DataTable
        Dim dt As DataTable = MTG.GridviewToDataTable(grd)

        '"Convertir" datatable a dataset
        Dim ds As New DataSet
        ds.Merge(dt)

        'Exportar el dataset anterior a Excel 
        MTG.ExportDataSetToExcel(ds, "InformeOFICIOSPersuasivo.xls")
    End Sub

End Class