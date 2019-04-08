Imports System.Data.SqlClient

Partial Public Class MAESTRO_REPRESENTANTESLEGALES
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            'Puts the previous state of the txtSearchMRL_Codigo_Nit field done when the user has searched and moved to the EditMAESTRO_REPRESENTANTESLEGALES page and then came back
            txtSearchMRL_Codigo_Nit.Text = Session("MAESTRO_REPRESENTANTESLEGALES.txtSearchMRL_Codigo_Nit")

            'Puts the previous state of the txtSearchMRL_Nombre field done when the user has searched and moved to the EditMAESTRO_REPRESENTANTESLEGALES page and then came back
            txtSearchMRL_Nombre.Text = Session("MAESTRO_REPRESENTANTESLEGALES.txtSearchMRL_Nombre")
            BindGrid()

            'End If - if IsPostBack equals false
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("MAESTRO_REPRESENTANTESLEGALES.RecordsFound") = 0
        If Len(Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage")) = 0 Then
            Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = 1

        End If
        If Len(Session("MAESTRO_REPRESENTANTESLEGALES.SortExpression")) = 0 Then
            Session("MAESTRO_REPRESENTANTESLEGALES.SortExpression") = "MRL_Codigo_Nit"
            Session("MAESTRO_REPRESENTANTESLEGALES.SortDirection") = "ASC"

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@MRL_Codigo_Nit", "%" & txtSearchMRL_Codigo_Nit.Text)

        Command.Parameters.AddWithValue("@MRL_Nombre", "%" & txtSearchMRL_Nombre.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[dbo].[MAESTRO_REPRESENTANTESLEGALES].*"
        Dim Table As String = "[dbo].[MAESTRO_REPRESENTANTESLEGALES]"
        Dim WhereClause As String = ""
        If txtSearchMRL_Codigo_Nit.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[MAESTRO_REPRESENTANTESLEGALES].[MRL_Codigo_Nit] like @MRL_Codigo_Nit"

        End If

        If txtSearchMRL_Nombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[MAESTRO_REPRESENTANTESLEGALES].[MRL_Nombre] like @MRL_Nombre"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)

        End If
        Dim SortOrder As String = Session("MAESTRO_REPRESENTANTESLEGALES.SortExpression") & " " & Session("MAESTRO_REPRESENTANTESLEGALES.SortDirection")
        Dim sql As String = "WITH MAESTRO_REPRESENTANTESLEGALESRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " MAESTRO_REPRESENTANTESLEGALESRecordCount AS ( SELECT * FROM MAESTRO_REPRESENTANTESLEGALESRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM MAESTRO_REPRESENTANTESLEGALESRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM MAESTRO_REPRESENTANTESLEGALESRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditMAESTRO_REPRESENTANTESLEGALES.aspx
        Response.Redirect("EditMAESTRO_REPRESENTANTESLEGALES.aspx")
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("MAESTRO_REPRESENTANTESLEGALES.txtSearchMRL_Codigo_Nit") = txtSearchMRL_Codigo_Nit.Text
        Session("MAESTRO_REPRESENTANTESLEGALES.txtSearchMRL_Nombre") = txtSearchMRL_Nombre.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim MRL_Codigo_Nit As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditMAESTRO_REPRESENTANTESLEGALES.aspx?ID=" & MRL_Codigo_Nit)

        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("MAESTRO_REPRESENTANTESLEGALES.SortDirection"))
            Case "ASC"
                Session("MAESTRO_REPRESENTANTESLEGALES.SortDirection") = "DESC"
            Case "DESC"
                Session("MAESTRO_REPRESENTANTESLEGALES.SortDirection") = "ASC"
            Case Else
                Session("MAESTRO_REPRESENTANTESLEGALES.SortDirection") = "ASC"
        End Select

        Session("MAESTRO_REPRESENTANTESLEGALES.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("MAESTRO_REPRESENTANTESLEGALES.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("MAESTRO_REPRESENTANTESLEGALES.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("MAESTRO_REPRESENTANTESLEGALES.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1

        End If
        If WholePageCount = 0 Then
            WholePageCount = 1

        End If


        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") > 1 Then
            Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") - 1
        Else
            Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Representantes encontrados " & Session("MAESTRO_REPRESENTANTESLEGALES.RecordsFound")
        lblPageNumber.Text = "Página " & Session("MAESTRO_REPRESENTANTESLEGALES.CurrentPage") & " de " & GetPageCount()
    End Sub
End Class
