Imports System.Data.SqlClient

Namespace Security.Maestros
    Partial Public Class ConfiguracionInteresesSancion
        Inherits Page

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            'Evaluates to true when the page is loaded for the first time.
            If IsPostBack = False Then

                'Puts the previous state of the txtSearchdesde field done when the user has searched and moved to the EditCALCULO_INTERESES_PARAFISCALES page and then came back
                'txtSearchdesde.Text = Session("CALCULO_INTERESES.txtSearchdesde")
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
            command.Connection = connection
            command.CommandText = sql
            'command.Parameters.AddWithValue("@p_anual", txtSearchdesde.Text)

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
            sql = sql & "SELECT PORCENTAJE_TASA_MULTA.* FROM PORCENTAJE_TASA_MULTA "
            Dim whereClause As String = ""
            'If txtSearchdesde.Text.Length > 0 Then
            '    whereClause = whereClause & " and [dbo].[PORCENTAJE_TASA_MULTA].[p_anual] = @p_anual"

            'End If

            If whereClause.Length > 0 Then
                whereClause = Replace(whereClause, " and ", "", , 1)
                sql = sql & "where " & whereClause

            End If

            If Len(Session("ConfiguracionInteresesSancion.SortExpression")) > 0 Then
                sql = sql & " order by " & Session("ConfiguracionInteresesSancion.SortExpression") & " " & Session("CALCULOUnderScoreINTERESES.SortDirection")

            End If
            Return sql

        End Function

        'cmdAddNew_Click event is run when the user clicks the AddNew button
        'Private Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddNew.Click
        '    'Go to the page : EditCALCULO_INTERESES_PARAFISCALES.aspx
        '    Response.Redirect("EditConfiguracionInteresesSancion.aspx")
        'End Sub


        'Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        '    BindGrid()

        '    Session("CALCULO_INTERESES.txtSearchdesde") = txtSearchdesde.Text
        'End Sub


        Private Sub grd_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles grd.RowCommand
            If e.CommandName = "" Then
                Dim consec As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Response.Redirect("EditConfiguracionInteresesSancion.aspx?ID=" & consec)

            End If
        End Sub

        Private Sub grd_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles grd.PageIndexChanging
            grd.PageIndex = e.NewPageIndex
            BindGrid()
        End Sub

        Private Sub grd_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles grd.Sorting

            Select Case CStr(Session("ConfiguracionInteresesSancion.SortDirection"))
                Case "ASC"
                    Session("ConfiguracionInteresesSancion.SortDirection") = "DESC"
                Case "DESC"
                    Session("ConfiguracionInteresesSancion.SortDirection") = "ASC"
                Case Else
                    Session("ConfiguracionInteresesSancion.SortDirection") = "ASC"
            End Select

            Session("ConfiguracionInteresesSancion.SortExpression") = e.SortExpression

            BindGrid()

        End Sub

    End Class
End Namespace