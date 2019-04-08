Imports System.Data.SqlClient

Namespace Security.Maestros
    Partial Public Class ConfiguracionInteresesParafiscales
        Inherits Page

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            'Evaluates to true when the page is loaded for the first time.
            If IsPostBack = False Then

                'Puts the previous state of the txtSearchdesde field done when the user has searched and moved to the EditCALCULO_INTERESES_PARAFISCALES page and then came back
                txtSearchdesde.Text = Session("CALCULO_INTERESES_PARAFISCALES.txtSearchdesde")
                BindGrid()

                Dim mtg As New MetodosGlobalesCobro
                lblNomPerfil.Text = Session("ssnombreusuario") & " ). ( " & mtg.GetNomPerfil(Session("mnivelacces")) & " )"

                'End If - if IsPostBack equals false
            End If
        End Sub


        'Display's the grid with the search criteria.
        Private Sub BindGrid()

            'Create a new connection to the database
            Dim connection As New SqlConnection(CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            Dim sql As String = GetSQL()
            Dim command As New SqlCommand()
            Command.Connection = Connection
            Command.CommandText = sql
            Command.Parameters.AddWithValue("@desde", txtSearchdesde.Text)

            Dim dataAdapter As New SqlDataAdapter(command)

            Dim dataSet As New DataSet

            DataAdapter.Fill(DataSet)
            grd.DataSource = DataSet
            grd.DataBind()

            'Close the Connection Object 
            Connection.Close()
        End Sub

        Private Function GetSql() As String
            Dim sql As String = ""
            sql = sql & "select [dbo].[CALCULO_INTERESES_PARAFISCALES].* from [dbo].[CALCULO_INTERESES_PARAFISCALES]"
            Dim whereClause As String = ""
            If txtSearchdesde.Text.Length > 0 Then
                WhereClause = WhereClause & " and [dbo].[CALCULO_INTERESES_PARAFISCALES].[desde] = @desde"

            End If

            If WhereClause.Length > 0 Then
                WhereClause = Replace(WhereClause, " and ", "", , 1)
                sql = sql & "where " & WhereClause

            End If

            If Len(Session("ConfiguracionInteresesParafiscales.SortExpression")) > 0 Then
                sql = sql & " order by " & Session("ConfiguracionInteresesParafiscales.SortExpression") & " " & Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortDirection")

            End If
            Return sql

        End Function

        'cmdAddNew_Click event is run when the user clicks the AddNew button
        Private Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddNew.Click
            'Go to the page : EditCALCULO_INTERESES_PARAFISCALES.aspx
            Response.Redirect("EditConfiguracionInteresesParafiscales.aspx")
        End Sub


        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            BindGrid()

            Session("CALCULO_INTERESES_PARAFISCALES.txtSearchdesde") = txtSearchdesde.Text
        End Sub


        Private Sub grd_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles grd.RowCommand
            If e.CommandName = "" Then
                Dim consec As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Response.Redirect("EditConfiguracionInteresesParafiscales.aspx?ID=" & consec)

            End If
        End Sub

        Private Sub grd_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles grd.PageIndexChanging
            grd.PageIndex = e.NewPageIndex
            BindGrid()
        End Sub

        Private Sub grd_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles grd.Sorting

            Select Case CStr(Session("ConfiguracionInteresesParafiscales.SortDirection"))
                Case "ASC"
                    Session("ConfiguracionInteresesParafiscales.SortDirection") = "DESC"
                Case "DESC"
                    Session("ConfiguracionInteresesParafiscales.SortDirection") = "ASC"
                Case Else
                    Session("ConfiguracionInteresesParafiscales.SortDirection") = "ASC"
            End Select

            Session("ConfiguracionInteresesParafiscales.SortExpression") = e.SortExpression

            BindGrid()

        End Sub

    End Class
End Namespace