Imports System.Data.SqlClient

Namespace Security.Maestros
    Partial Public Class ConfiguracionIPC
        Inherits Page

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            'Evaluates to true when the page is loaded for the first time.
            If IsPostBack = False Then

                'Puts the previous state of the txtSearchdesde field done when the user has searched and moved to the EditCALCULO_INTERESES_PARAFISCALES page and then came back
                txtSearchanio.Text = Session("IPC.txtSearchanio")
                BindGrid()

                lblNomPerfil.Text = Session("ssnombreusuario") & " ). ( " & CommonsCobrosCoactivos.getNomPerfil(Session) & " )"

                'End If - if IsPostBack equals false
            End If
        End Sub


        'Display's the grid with the search criteria.
        Private Sub BindGrid()

            'Create a new connection to the database
            Dim connection As New SqlConnection(CadenaConexion)
            'Opens a connection to the database.
            connection.Open()
            Dim sql As String = GetSql()
            Dim command As New SqlCommand()
            command.CommandTimeout = 60000
            command.Connection = connection
            command.CommandText = sql
            command.Parameters.AddWithValue("@ANIO", txtSearchanio.Text)

            Dim dataAdapter As New SqlDataAdapter(command)

            Dim dataSet As New DataSet

            dataAdapter.Fill(dataSet)
            grd.DataSource = dataSet
            grd.DataBind()

            'Close the Connection Object 
            connection.Close()
        End Sub

        Private Function GetSql() As String
            Dim sql As String = ""
            sql = sql & "select IPC.* from IPC "
            Dim whereClause As String = ""
            If txtSearchanio.Text.Length > 0 Then
                whereClause = whereClause & " and IPC.ANIO = @ANIO"

            End If

            If whereClause.Length > 0 Then
                whereClause = Replace(whereClause, " and ", "", , 1)
                sql = sql & "where " & whereClause

            End If

            If Len(Session("ConfiguracionIPC.SortExpression")) > 0 Then
                sql = sql & " order by " & Session("ConfiguracionIPC.SortExpression") & " " & Session("IPC.SortDirection")

            End If
            Return sql

        End Function

        'cmdAddNew_Click event is run when the user clicks the AddNew button
        Private Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddNew.Click
            'Go to the page : EditCALCULO_INTERESES_PARAFISCALES.aspx
            Response.Redirect("EditConfiguracionIPC.aspx")
        End Sub


        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            BindGrid()

            Session("IPC.txtSearchanio") = txtSearchanio.Text
        End Sub


        Private Sub grd_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles grd.RowCommand
            If e.CommandName = "" Then
                Dim consec As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Response.Redirect("EditConfiguracionIPC.aspx?ID=" & consec)

            End If
        End Sub

        Private Sub grd_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles grd.PageIndexChanging
            grd.PageIndex = e.NewPageIndex
            BindGrid()
        End Sub

        Private Sub grd_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles grd.Sorting

            Select Case CStr(Session("ConfiguracionIPC.SortDirection"))
                Case "ASC"
                    Session("ConfiguracionIPC.SortDirection") = "DESC"
                Case "DESC"
                    Session("ConfiguracionIPC.SortDirection") = "ASC"
                Case Else
                    Session("ConfiguracionIPC.SortDirection") = "ASC"
            End Select

            Session("ConfiguracionIPC.SortExpression") = e.SortExpression

            BindGrid()

        End Sub

    End Class
End Namespace