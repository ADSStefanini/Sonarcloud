Imports System.Data.SqlClient

Partial Public Class CRITERIOS_MEDIDAS_CAUTELARES
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            'Puts the previous state of the txtSearchcodigo field done when the user has searched and moved to the EditCRITERIOS_MEDIDAS_CAUTELARES page and then came back
            txtSearchcodigo.Text = Session("CRITERIOS_MEDIDAS_CAUTELARES.txtSearchcodigo")

            'Puts the previous state of the txtSearchnombre field done when the user has searched and moved to the EditCRITERIOS_MEDIDAS_CAUTELARES page and then came back
            txtSearchnombre.Text = Session("CRITERIOS_MEDIDAS_CAUTELARES.txtSearchnombre")
            BindGrid()
        End If
    End Sub


    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("CRITERIOS_MEDIDAS_CAUTELARES.RecordsFound") = 0
        If Len(Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage")) = 0 Then
            Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = 1

        End If
        If Len(Session("CRITERIOS_MEDIDAS_CAUTELARES.SortExpression")) = 0 Then
            Session("CRITERIOS_MEDIDAS_CAUTELARES.SortExpression") = "codigo"
            Session("CRITERIOS_MEDIDAS_CAUTELARES.SortDirection") = "ASC"

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@codigo", "%" & txtSearchcodigo.Text)

        Command.Parameters.AddWithValue("@nombre", "%" & txtSearchnombre.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[dbo].[CRITERIOS_MEDIDAS_CAUTELARES].*"
        Dim Table As String = "[dbo].[CRITERIOS_MEDIDAS_CAUTELARES]"
        Dim WhereClause As String = ""
        If txtSearchcodigo.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[CRITERIOS_MEDIDAS_CAUTELARES].[codigo] like @codigo"

        End If

        If txtSearchnombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[CRITERIOS_MEDIDAS_CAUTELARES].[nombre] like @nombre"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)

        End If
        Dim SortOrder As String = Session("CRITERIOS_MEDIDAS_CAUTELARES.SortExpression") & " " & Session("CRITERIOS_MEDIDAS_CAUTELARES.SortDirection")
        Dim sql As String = "WITH CRITERIOS_MEDIDAS_CAUTELARESRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " CRITERIOS_MEDIDAS_CAUTELARESRecordCount AS ( SELECT * FROM CRITERIOS_MEDIDAS_CAUTELARESRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM CRITERIOS_MEDIDAS_CAUTELARESRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM CRITERIOS_MEDIDAS_CAUTELARESRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditCRITERIOS_MEDIDAS_CAUTELARES.aspx
        Response.Redirect("EditCRITERIOS_MEDIDAS_CAUTELARES.aspx")
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("CRITERIOS_MEDIDAS_CAUTELARES.txtSearchcodigo") = txtSearchcodigo.Text
        Session("CRITERIOS_MEDIDAS_CAUTELARES.txtSearchnombre") = txtSearchnombre.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim codigo As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditCRITERIOS_MEDIDAS_CAUTELARES.aspx?ID=" & codigo)

        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("CRITERIOS_MEDIDAS_CAUTELARES.SortDirection"))
            Case "ASC"
                Session("CRITERIOS_MEDIDAS_CAUTELARES.SortDirection") = "DESC"
            Case "DESC"
                Session("CRITERIOS_MEDIDAS_CAUTELARES.SortDirection") = "ASC"
            Case Else
                Session("CRITERIOS_MEDIDAS_CAUTELARES.SortDirection") = "ASC"
        End Select

        Session("CRITERIOS_MEDIDAS_CAUTELARES.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("CRITERIOS_MEDIDAS_CAUTELARES.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("CRITERIOS_MEDIDAS_CAUTELARES.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("CRITERIOS_MEDIDAS_CAUTELARES.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1

        End If
        If WholePageCount = 0 Then
            WholePageCount = 1

        End If


        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") > 1 Then
            Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") - 1
        Else
            Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Registros encontrados " & Session("CRITERIOS_MEDIDAS_CAUTELARES.RecordsFound")
        lblPageNumber.Text = "Página " & Session("CRITERIOS_MEDIDAS_CAUTELARES.CurrentPage") & " de " & GetPageCount()
    End Sub
End Class
