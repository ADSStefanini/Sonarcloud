Imports System.Data.SqlClient
Partial Public Class PAGOSEXPEDIENTE
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            'Loads elements from the codigo table to be searched on
            LoadcboSearchestado()

            'Puts the previous state of the txtSearchNroConsignacion field done when the user has searched and moved to the EditPAGOS page and then came back
            txtSearchNroConsignacion.Text = Session("PAGOS.txtSearchNroConsignacion")

            'Puts the previous state of the cboSearchestado field done when the user has searched and moved to the EditPAGOS page and then came back
            cboSearchestado.SelectedValue = Session("PAGOS.cboSearchestado")
            BindGrid()

            'End If - if IsPostBack equals false
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("PAGOS.RecordsFound") = 0
        If Len(Session("PAGOS.CurrentPage")) = 0 Then
            Session("PAGOS.CurrentPage") = 1

        End If
        If Len(Session("PAGOS.SortExpression")) = 0 Then
            Session("PAGOS.SortExpression") = "NroConsignacion"
            Session("PAGOS.SortDirection") = "ASC"

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@NroConsignacion", "%" & txtSearchNroConsignacion.Text)

        Command.Parameters.AddWithValue("@estado", "%" & cboSearchestado.SelectedValue)

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("PAGOS.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("PAGOS.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("PAGOS.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[dbo].[PAGOS].*, ESTADOS_PAGOestado.nombre as ESTADOS_PAGOestadonombre, ESTADOS_PROCESOpagestadoprocfrp.nombre as ESTADOS_PROCESOpagestadoprocfrpnombre"
        Dim Table As String = "([dbo].[PAGOS] left join [ESTADOS_PAGO] ESTADOS_PAGOestado on [dbo].[PAGOS].estado = ESTADOS_PAGOestado.codigo )  left join [ESTADOS_PROCESO] ESTADOS_PROCESOpagestadoprocfrp on [dbo].[PAGOS].pagestadoprocfrp = ESTADOS_PROCESOpagestadoprocfrp.codigo "
        Dim WhereClause As String = " "
        If txtSearchNroConsignacion.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PAGOS].[NroConsignacion] like @NroConsignacion"
        End If

        If cboSearchestado.SelectedValue.Length > 0 Then
            WhereClause = WhereClause & " and PAGOS.estado like @estado"
        End If

        'Expediente actual
        WhereClause = WhereClause & " and NroExp = '" & Request("pExpediente") & "' "

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If



        Dim SortOrder As String = Session("PAGOS.SortExpression") & " " & Session("PAGOS.SortDirection")
        Dim sql As String = "WITH PAGOSRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " PAGOSRecordCount AS ( SELECT * FROM PAGOSRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PAGOSRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM PAGOSRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("PAGOS.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("PAGOS.txtSearchNroConsignacion") = txtSearchNroConsignacion.Text
        Session("PAGOS.cboSearchestado") = cboSearchestado.SelectedValue
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim NroConsignacion As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("verPAGOS.aspx?ID=" & NroConsignacion & "&pExpediente=" & Request("pExpediente"))
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("PAGOS.SortDirection"))
            Case "ASC"
                Session("PAGOS.SortDirection") = "DESC"
            Case "DESC"
                Session("PAGOS.SortDirection") = "ASC"
            Case Else
                Session("PAGOS.SortDirection") = "ASC"
        End Select

        Session("PAGOS.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("PAGOS.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("PAGOS.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("PAGOS.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1

        End If
        If WholePageCount = 0 Then
            WholePageCount = 1

        End If


        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("PAGOS.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("PAGOS.CurrentPage") = Session("PAGOS.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("PAGOS.CurrentPage") > 1 Then
            Session("PAGOS.CurrentPage") = Session("PAGOS.CurrentPage") - 1
        Else
            Session("PAGOS.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("PAGOS.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Pagos encontrados " & Session("PAGOS.RecordsFound")
        lblPageNumber.Text = "Página " & Session("PAGOS.CurrentPage") & " de " & GetPageCount()
    End Sub

    Protected Sub LoadcboSearchestado()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()

        'Select statement that loads the combo box for searching the estado column
        Dim sql As String = "select codigo, nombre from [ESTADOS_PAGO] order by nombre"

        'Set the Command variable to a new instance of a SqlCommand object
        'Initialize it with the sql and Connection
        Dim Command As New SqlCommand(sql, Connection)

        'Set the DataTextField to nombre
        'The DataTextField linked to the field that is to be displayed in the combo box.
        cboSearchestado.DataTextField = "nombre"

        'Set the DataValueField to codigo
        'The DataTextField linked to the field that will be returned when an item is selected.
        cboSearchestado.DataValueField = "codigo"
        cboSearchestado.DataSource = Command.ExecuteReader()
        cboSearchestado.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

End Class