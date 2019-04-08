Imports System.Data.SqlClient
Partial Public Class VERIFICACIONESPAGO
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            'Loads elements from the codigo table to be searched on
            LoadcboSearchestado()

            'Puts the previous state of the txtSearchNroConsignacion field done when the user has searched and moved to the EditVERIFICACIONESPAGO page and then came back
            txtSearchNroConsignacion.Text = Session("VERIFICACIONESPAGO.txtSearchNroConsignacion")

            'Puts the previous state of the cboSearchestado field done when the user has searched and moved to the EditVERIFICACIONESPAGO page and then came back
            cboSearchestado.SelectedValue = Session("VERIFICACIONESPAGO.cboSearchestado")

            BindGrid()

            '--------------
            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                cmdAddNew.Visible = False
            End If
            '--------------
        End If
    End Sub

  

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("VERIFICACIONESPAGO.RecordsFound") = 0
        If Len(Session("VERIFICACIONESPAGO.CurrentPage")) = 0 Then
            Session("VERIFICACIONESPAGO.CurrentPage") = 1

        End If
        If Len(Session("VERIFICACIONESPAGO.SortExpression")) = 0 Then
            Session("VERIFICACIONESPAGO.SortExpression") = "NroConsignacion"
            Session("VERIFICACIONESPAGO.SortDirection") = "ASC"

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

        If Session("VERIFICACIONESPAGO.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("VERIFICACIONESPAGO.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("VERIFICACIONESPAGO.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[dbo].[PAGOS].*, ESTADOS_PAGOestado.nombre as ESTADOS_PAGOestadonombre, USUARIOSUserSolicita.nombre as USUARIOSUserSolicitanombre, USUARIOSUserVerif.nombre as USUARIOSUserVerifnombre"
        Dim Table As String = "(([dbo].[PAGOS] left join [ESTADOS_PAGO] ESTADOS_PAGOestado on [dbo].[PAGOS].estado = ESTADOS_PAGOestado.codigo )  left join [USUARIOS] USUARIOSUserSolicita on [dbo].[PAGOS].UserSolicita = USUARIOSUserSolicita.codigo )  left join [USUARIOS] USUARIOSUserVerif on [dbo].[PAGOS].UserVerif = USUARIOSUserVerif.codigo "
        Dim WhereClause As String = ""

        WhereClause = WhereClause & " and PAGOS.NroExp = '" & Request("pExpediente") & "'"

        If txtSearchNroConsignacion.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PAGOS].[NroConsignacion] like @NroConsignacion"
        End If

        If cboSearchestado.SelectedValue.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[PAGOS].[estado] like @estado"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)

        End If
        Dim SortOrder As String = Session("VERIFICACIONESPAGO.SortExpression") & " " & Session("VERIFICACIONESPAGO.SortDirection")
        Dim sql As String = "WITH VERIFICACIONESPAGORecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " VERIFICACIONESPAGORecordCount AS ( SELECT * FROM VERIFICACIONESPAGORecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM VERIFICACIONESPAGORecordSet) AS RC ) "
        sql = sql & "SELECT * FROM VERIFICACIONESPAGORecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditVERIFICACIONESPAGO.aspx?pExpediente=" & Request("pExpediente"))
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("VERIFICACIONESPAGO.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("VERIFICACIONESPAGO.txtSearchNroConsignacion") = txtSearchNroConsignacion.Text
        Session("VERIFICACIONESPAGO.cboSearchestado") = cboSearchestado.SelectedValue
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim NroConsignacion As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditVERIFICACIONESPAGO.aspx?ID=" & NroConsignacion.Trim & "&pExpediente=" & Request("pExpediente"))
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("VERIFICACIONESPAGO.SortDirection"))
            Case "ASC"
                Session("VERIFICACIONESPAGO.SortDirection") = "DESC"
            Case "DESC"
                Session("VERIFICACIONESPAGO.SortDirection") = "ASC"
            Case Else
                Session("VERIFICACIONESPAGO.SortDirection") = "ASC"
        End Select

        Session("VERIFICACIONESPAGO.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("VERIFICACIONESPAGO.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("VERIFICACIONESPAGO.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("VERIFICACIONESPAGO.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1

        End If
        If WholePageCount = 0 Then
            WholePageCount = 1

        End If


        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("VERIFICACIONESPAGO.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("VERIFICACIONESPAGO.CurrentPage") = Session("VERIFICACIONESPAGO.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("VERIFICACIONESPAGO.CurrentPage") > 1 Then
            Session("VERIFICACIONESPAGO.CurrentPage") = Session("VERIFICACIONESPAGO.CurrentPage") - 1
        Else
            Session("VERIFICACIONESPAGO.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("VERIFICACIONESPAGO.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Registros encontrados " & Session("VERIFICACIONESPAGO.RecordsFound")
        lblPageNumber.Text = "Página " & Session("VERIFICACIONESPAGO.CurrentPage") & " de " & GetPageCount()
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