Imports System.Data.SqlClient
Partial Public Class PROMOTORES
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            'Puts the previous state of the txtSearchcedula field done when the user has searched and moved to the EditPROMOTORES page and then came back
            txtSearchcedula.Text = Session("PROMOTORES.txtSearchcedula")

            'Puts the previous state of the txtSearchnombre field done when the user has searched and moved to the EditPROMOTORES page and then came back
            txtSearchnombre.Text = Session("PROMOTORES.txtSearchnombre")
            BindGrid()

            'End If - if IsPostBack equals false
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("PROMOTORES.RecordsFound") = 0
        If Len(Session("PROMOTORES.CurrentPage")) = 0 Then
            Session("PROMOTORES.CurrentPage") = 1

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@cedula", "%" & txtSearchcedula.Text)

        Command.Parameters.AddWithValue("@nombre", "%" & txtSearchnombre.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("PROMOTORES.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("PROMOTORES.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("PROMOTORES.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[dbo].[PROMOTORES].*"
        Dim Table As String = "[dbo].[PROMOTORES]"
        Dim WhereClause As String = ""
        If txtSearchcedula.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PROMOTORES].[cedula] like @cedula"

        End If

        If txtSearchnombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PROMOTORES].[nombre] like @nombre"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)

        End If
        Dim sql As String = "WITH PROMOTORESRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY cedula ASC) AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " PROMOTORESRecordCount AS ( SELECT * FROM PROMOTORESRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PROMOTORESRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM PROMOTORESRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditPROMOTORES.aspx
        Response.Redirect("EditPROMOTORES.aspx")
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("PROMOTORES.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("PROMOTORES.txtSearchcedula") = txtSearchcedula.Text
        Session("PROMOTORES.txtSearchnombre") = txtSearchnombre.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim cedula As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditPROMOTORES.aspx?ID=" & cedula)

        End If
    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("PROMOTORES.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("PROMOTORES.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("PROMOTORES.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1

        End If
        If WholePageCount = 0 Then
            WholePageCount = 1

        End If


        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("PROMOTORES.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("PROMOTORES.CurrentPage") = Session("PROMOTORES.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("PROMOTORES.CurrentPage") > 1 Then
            Session("PROMOTORES.CurrentPage") = Session("PROMOTORES.CurrentPage") - 1
        Else
            Session("PROMOTORES.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("PROMOTORES.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Promotores encontrados " & Session("PROMOTORES.RecordsFound")
        lblPageNumber.Text = "Página " & Session("PROMOTORES.CurrentPage") & " de " & GetPageCount()
    End Sub

End Class