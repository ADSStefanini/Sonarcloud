Imports System.Data.SqlClient

Partial Public Class MAESTRO_APODERADOS
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            'Puts the previous state of the txtSearchMAP_Codigo_Nit field done when the user has searched and moved to the EditMAESTRO_APODERADOS page and then came back
            txtSearchMAP_Codigo_Nit.Text = Session("MAESTRO_APODERADOS.txtSearchMAP_Codigo_Nit")

            'Puts the previous state of the txtSearchMAP_Nombre field done when the user has searched and moved to the EditMAESTRO_APODERADOS page and then came back
            txtSearchMAP_Nombre.Text = Session("MAESTRO_APODERADOS.txtSearchMAP_Nombre")

            'Puts the previous state of the txtSearchMAP_TarjetaProf field done when the user has searched and moved to the EditMAESTRO_APODERADOS page and then came back
            txtSearchMAP_TarjetaProf.Text = Session("MAESTRO_APODERADOS.txtSearchMAP_TarjetaProf")
            BindGrid()

            'End If - if IsPostBack equals false
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("MAESTRO_APODERADOS.RecordsFound") = 0
        If Len(Session("MAESTRO_APODERADOS.CurrentPage")) = 0 Then
            Session("MAESTRO_APODERADOS.CurrentPage") = 1

        End If
        If Len(Session("MAESTRO_APODERADOS.SortExpression")) = 0 Then
            Session("MAESTRO_APODERADOS.SortExpression") = "MAP_Codigo_Nit"
            Session("MAESTRO_APODERADOS.SortDirection") = "ASC"

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@MAP_Codigo_Nit", "%" & txtSearchMAP_Codigo_Nit.Text)

        Command.Parameters.AddWithValue("@MAP_Nombre", "%" & txtSearchMAP_Nombre.Text & "%")

        Command.Parameters.AddWithValue("@MAP_TarjetaProf", "%" & txtSearchMAP_TarjetaProf.Text)

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("MAESTRO_APODERADOS.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("MAESTRO_APODERADOS.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("MAESTRO_APODERADOS.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[dbo].[MAESTRO_APODERADOS].*"
        Dim Table As String = "[dbo].[MAESTRO_APODERADOS]"
        Dim WhereClause As String = ""
        If txtSearchMAP_Codigo_Nit.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[MAESTRO_APODERADOS].[MAP_Codigo_Nit] like @MAP_Codigo_Nit"

        End If

        If txtSearchMAP_Nombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[MAESTRO_APODERADOS].[MAP_Nombre] like @MAP_Nombre"

        End If

        If txtSearchMAP_TarjetaProf.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[MAESTRO_APODERADOS].[MAP_TarjetaProf] like @MAP_TarjetaProf"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)

        End If
        Dim SortOrder As String = Session("MAESTRO_APODERADOS.SortExpression") & " " & Session("MAESTRO_APODERADOS.SortDirection")
        Dim sql As String = "WITH MAESTRO_APODERADOSRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " MAESTRO_APODERADOSRecordCount AS ( SELECT * FROM MAESTRO_APODERADOSRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM MAESTRO_APODERADOSRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM MAESTRO_APODERADOSRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditMAESTRO_APODERADOS.aspx
        Response.Redirect("EditMAESTRO_APODERADOS.aspx")
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("MAESTRO_APODERADOS.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("MAESTRO_APODERADOS.txtSearchMAP_Codigo_Nit") = txtSearchMAP_Codigo_Nit.Text
        Session("MAESTRO_APODERADOS.txtSearchMAP_Nombre") = txtSearchMAP_Nombre.Text
        Session("MAESTRO_APODERADOS.txtSearchMAP_TarjetaProf") = txtSearchMAP_TarjetaProf.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim MAP_Codigo_Nit As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditMAESTRO_APODERADOS.aspx?ID=" & MAP_Codigo_Nit)

        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("MAESTRO_APODERADOS.SortDirection"))
            Case "ASC"
                Session("MAESTRO_APODERADOS.SortDirection") = "DESC"
            Case "DESC"
                Session("MAESTRO_APODERADOS.SortDirection") = "ASC"
            Case Else
                Session("MAESTRO_APODERADOS.SortDirection") = "ASC"
        End Select

        Session("MAESTRO_APODERADOS.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("MAESTRO_APODERADOS.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("MAESTRO_APODERADOS.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("MAESTRO_APODERADOS.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1

        End If
        If WholePageCount = 0 Then
            WholePageCount = 1

        End If


        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("MAESTRO_APODERADOS.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("MAESTRO_APODERADOS.CurrentPage") = Session("MAESTRO_APODERADOS.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("MAESTRO_APODERADOS.CurrentPage") > 1 Then
            Session("MAESTRO_APODERADOS.CurrentPage") = Session("MAESTRO_APODERADOS.CurrentPage") - 1
        Else
            Session("MAESTRO_APODERADOS.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("MAESTRO_APODERADOS.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Apoderados encontrados " & Session("MAESTRO_APODERADOS.RecordsFound")
        lblPageNumber.Text = "Página " & Session("MAESTRO_APODERADOS.CurrentPage") & " de " & GetPageCount()
    End Sub
End Class
