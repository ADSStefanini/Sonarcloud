Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class LOG_AUDITORIA
    Inherits System.Web.UI.Page
    Dim NumRegs As Integer = 0
    'Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            Dim perfilPaginaBLL As PerfilPaginaBLL = New PerfilPaginaBLL()
            Dim permisosPerfil = perfilPaginaBLL.obtenerPermisosPorPagina(Convert.ToInt32(Session("mnivelacces")), 100)

            If permisosPerfil.ind_puede_ver = Enumeraciones.Estado.ACTIVO Then
                'Puede ver
            Else
                'No tiene permiso de acceso a la página
            End If

            If permisosPerfil.ind_puede_editar = Enumeraciones.Estado.INACTIVO Then
                'No puede editar, desabilitar campos de edición
            End If


            'Combo de la paginacion ---------------------------
            LoadcboNumExp()

            If Session("Paginacion") = Nothing Then
                Session("Paginacion") = 10
            End If

            PaginacionEjefisglobal = Session("Paginacion")
            cboNumExp.SelectedValue = PaginacionEjefisglobal
            '--------------------------------------------------

            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)

            'Puts the previous state of the txtSearchLOG_USER_ID field done when the user has searched and moved to the EditLOG_AUDITORIA page and then came back
            txtSearchLOG_USER_ID.Text = Session("LOG_AUDITORIA.txtSearchLOG_USER_ID")

            'Puts the previous state of the txtSearchLOG_HOST field done when the user has searched and moved to the EditLOG_AUDITORIA page and then came back
            txtSearchLOG_HOST.Text = Session("LOG_AUDITORIA.txtSearchLOG_HOST")

            'Puts the previous state of the txtSearchLOG_IP field done when the user has searched and moved to the EditLOG_AUDITORIA page and then came back
            txtSearchLOG_IP.Text = Session("LOG_AUDITORIA.txtSearchLOG_IP")

            'Puts the previous state of the txtSearchLOG_MODULO field done when the user has searched and moved to the EditLOG_AUDITORIA page and then came back
            txtSearchLOG_MODULO.Text = Session("LOG_AUDITORIA.txtSearchLOG_MODULO")

            'Puts the previous state of the txtSearchLOG_DOC_AFEC field done when the user has searched and moved to the EditLOG_AUDITORIA page and then came back
            txtSearchLOG_DOC_AFEC.Text = Session("LOG_AUDITORIA.txtSearchLOG_DOC_AFEC")
            BindGrid()

        End If
    End Sub

    Private Function ContarRegistros(ByVal dtTabla As DataTable) As Integer
        Dim NumRegistros As Integer = 0
        If dtTabla.Rows.Count > 0 Then
            NumRegistros = dtTabla.Rows(0).Item("RecordSetCount").ToString
        End If
        Return NumRegistros
    End Function

    Private Sub LoadcboNumExp()
        Dim dt As DataTable = New DataTable("TablaPaginacion")

        dt.Columns.Add("Codigo")
        dt.Columns.Add("Descripcion")

        Dim dr As DataRow

        dr = dt.NewRow()
        dr("Codigo") = "10"
        dr("Descripcion") = "10"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Codigo") = "20"
        dr("Descripcion") = "20"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Codigo") = "50"
        dr("Descripcion") = "50"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Codigo") = "1000000"
        dr("Descripcion") = "TODOS"
        dt.Rows.Add(dr)


        cboNumExp.DataSource = dt
        cboNumExp.DataValueField = "Codigo"
        cboNumExp.DataTextField = "Descripcion"
        cboNumExp.DataBind()
    End Sub

    Protected Sub cboNumExp_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboNumExp.SelectedIndexChanged
        'PaginacionEjefisglobal = 30
        PaginacionEjefisglobal = cboNumExp.SelectedValue
        Session("Paginacion") = PaginacionEjefisglobal
        BindGrid()
    End Sub

    Protected Sub btnExportarGrid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportarGrid.Click

        'Instanciar clase de metodos globales
        Dim MTG As New MetodosGlobalesCobro

        'Convertir Gridview a DataTable
        Dim dt As DataTable = MTG.GridviewToDataTable(grd)

        '"Convertir" datatable a dataset
        Dim ds As New DataSet
        ds.Merge(dt)

        'Exportar el dataset anterior a Excel 
        MTG.ExportDataSetToExcel(ds, "GridLogAuditotia.xls")

    End Sub

    Private Function GetNomPerfil(ByVal pUsuario As String) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & Session("mnivelacces")

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        Return NomPerfil
    End Function

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("LOG_AUDITORIA.RecordsFound") = 0
        If Len(Session("LOG_AUDITORIA.CurrentPage")) = 0 Then
            Session("LOG_AUDITORIA.CurrentPage") = 1

        End If
        If Len(Session("LOG_AUDITORIA.SortExpression")) = 0 Then
            Session("LOG_AUDITORIA.SortExpression") = "LOG_CONSE"
            Session("LOG_AUDITORIA.SortDirection") = "ASC"
        End If

        'Create a new connection to the database
        Dim cnx As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        'cnx.Open()
        Dim sql As String = GetSQL()

        'SQLCommand
        Dim Command As New SqlCommand()
        Command.Connection = cnx
        Command.CommandText = sql
        'Parametros
        Command.Parameters.AddWithValue("@LOG_USER_ID", "%" & txtSearchLOG_USER_ID.Text & "%")
        Command.Parameters.AddWithValue("@LOG_HOST", "%" & txtSearchLOG_HOST.Text & "%")
        Command.Parameters.AddWithValue("@LOG_IP", "%" & txtSearchLOG_IP.Text & "%")
        Command.Parameters.AddWithValue("@LOG_MODULO", "%" & txtSearchLOG_MODULO.Text & "%")
        Command.Parameters.AddWithValue("@LOG_DOC_AFEC", "%" & txtSearchLOG_DOC_AFEC.Text & "%")

        'Llenar dataTable
        Dim Adaptador As New SqlDataAdapter(Command)
        Dim dtLog As New DataTable
        Adaptador.Fill(dtLog)
        NumRegs = ContarRegistros(dtLog)

        'grd.DataSource = Command.ExecuteReader()
        grd.DataSource = dtLog
        grd.DataBind()

        'Close the Connection Object 
        'cnx.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("LOG_AUDITORIA.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("LOG_AUDITORIA.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PaginacionEjefisglobal * Session("LOG_AUDITORIA.CurrentPage") - PaginacionEjefisglobal + 1
        Dim StopRecord As Long = StartRecord + PaginacionEjefisglobal
        Dim Columns As String = "[dbo].[LOG_AUDITORIA].*"
        Dim Table As String = "[dbo].[LOG_AUDITORIA]"
        Dim WhereClause As String = ""
        If txtSearchLOG_USER_ID.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[LOG_AUDITORIA].[LOG_USER_ID] like @LOG_USER_ID"

        End If

        If txtSearchLOG_HOST.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[LOG_AUDITORIA].[LOG_HOST] like @LOG_HOST"

        End If

        If txtSearchLOG_IP.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[LOG_AUDITORIA].[LOG_IP] like @LOG_IP"

        End If

        If txtSearchLOG_MODULO.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[LOG_AUDITORIA].[LOG_MODULO] like @LOG_MODULO"

        End If

        If txtSearchLOG_DOC_AFEC.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[LOG_AUDITORIA].[LOG_DOC_AFEC] like @LOG_DOC_AFEC"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)

        End If
        Dim SortOrder As String = Session("LOG_AUDITORIA.SortExpression") & " " & Session("LOG_AUDITORIA.SortDirection")
        Dim sql As String = "WITH LOG_AUDITORIARecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " LOG_AUDITORIARecordCount AS ( SELECT * FROM LOG_AUDITORIARecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM LOG_AUDITORIARecordSet) AS RC ) "
        sql = sql & "SELECT * FROM LOG_AUDITORIARecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("LOG_AUDITORIA.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("LOG_AUDITORIA.txtSearchLOG_USER_ID") = txtSearchLOG_USER_ID.Text
        Session("LOG_AUDITORIA.txtSearchLOG_HOST") = txtSearchLOG_HOST.Text
        Session("LOG_AUDITORIA.txtSearchLOG_IP") = txtSearchLOG_IP.Text
        Session("LOG_AUDITORIA.txtSearchLOG_MODULO") = txtSearchLOG_MODULO.Text
        Session("LOG_AUDITORIA.txtSearchLOG_DOC_AFEC") = txtSearchLOG_DOC_AFEC.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim LOG_CONSE As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditLOG_AUDITORIA.aspx?ID=" & LOG_CONSE)

        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("LOG_AUDITORIA.SortDirection"))
            Case "ASC"
                Session("LOG_AUDITORIA.SortDirection") = "DESC"
            Case "DESC"
                Session("LOG_AUDITORIA.SortDirection") = "ASC"
            Case Else
                Session("LOG_AUDITORIA.SortDirection") = "ASC"
        End Select

        Session("LOG_AUDITORIA.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            'Session("LOG_AUDITORIA.RecordsFound") = grd.DataSource("RecordSetCount")
            'NumRegs
            Session("LOG_AUDITORIA.RecordsFound") = NumRegs
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("LOG_AUDITORIA.RecordsFound") / PaginacionEjefisglobal)
        Dim PartialRecordCount As Long = Session("LOG_AUDITORIA.RecordsFound") Mod PaginacionEjefisglobal
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If

        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("LOG_AUDITORIA.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("LOG_AUDITORIA.CurrentPage") = Session("LOG_AUDITORIA.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("LOG_AUDITORIA.CurrentPage") > 1 Then
            Session("LOG_AUDITORIA.CurrentPage") = Session("LOG_AUDITORIA.CurrentPage") - 1
        Else
            Session("LOG_AUDITORIA.CurrentPage") = 1
        End If

        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("LOG_AUDITORIA.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Registros encontrados " & Session("LOG_AUDITORIA.RecordsFound")
        lblPageNumber.Text = "Página " & Session("LOG_AUDITORIA.CurrentPage") & " de " & GetPageCount()
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        'Limpiar los cuadros de texto de busqueda
        Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
        Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
        Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
        Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
        Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""

        Response.Redirect("../../login.aspx")
    End Sub

    Protected Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click
        'EditEJEFISGLOBAL.aspx?ID=80003 
        Response.Redirect("../menu.aspx")
    End Sub
End Class