Imports System.Data.SqlClient
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Configuration

Partial Public Class EJEFISGLOBALREPARTIDOR
    Inherits System.Web.UI.Page
    Dim NumRegs As Integer = 0

    'Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '11/mzo/2014. Validar sesion
        Dim MTG As New MetodosGlobalesCobro
        Dim SesionExpirada As Boolean = MTG.IsSessionTimedOut()

        If SesionExpirada Then
            CerrarSesion()
            Response.Redirect("../../login.aspx")
        End If

        ContarSolicitudesCambioEstado()
        ContarMsjNoLeidos()

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            'Loads elements from the codigo table to be searched on
            LoadcboSearchEFIUSUASIG()
            'Loads elements from the codigo table to be searched on
            LoadcboSearchEFIESTADO()

            'Combo de la paginacion
            LoadcboNumExp()

            If Session("Paginacion") = Nothing Then
                Session("Paginacion") = 10
            End If

            lblNomPerfil.Text = GetNomPerfil(Session("sscodigousuario"))

            PaginacionEjefisglobal = Session("Paginacion")
            cboNumExp.SelectedValue = PaginacionEjefisglobal


            'Puts the previous state of the txtSearchEFINROEXP field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchEFINROEXP.Text = Session("EJEFISGLOBAL.txtSearchEFINROEXP")

            'Puts the previous state of the txtSearchEFINUMMEMO field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchEFINUMMEMO.Text = Session("EJEFISGLOBAL.txtSearchEFINUMMEMO")

            'Puts the previous state of the txtSearchEFINIT field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchEFINIT.Text = Session("EJEFISGLOBAL.txtSearchEFINIT")

            'Puts the previous state of the txtSearchED_NOMBRE field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchED_NOMBRE.Text = Session("EJEFISGLOBAL.txtSearchED_NOMBRE")

            'Puts the previous state of the cboSearchEFIUSUASIG field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            cboSearchEFIUSUASIG.SelectedValue = Session("EJEFISGLOBAL.cboSearchEFIUSUASIG")

            'Puts the previous state of the cboSearchEFIESTADO field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            cboSearchEFIESTADO.SelectedValue = Session("EJEFISGLOBAL.cboSearchEFIESTADO")
            BindGrid()

            'Instanciar clase de metodos globales
            'Dim MTG As New MetodosGlobalesCobro

            'Mostrar estadisticas de expedientes vencidos y por vencer
            'lnkNumExpVencidos.Text = MTG.MostrarVencidos(Session("sscodigousuario")).ToString.Trim
            'lnkNumExpVencer.Text = MTG.MostrarxVencer(Session("sscodigousuario")).ToString.Trim

            'Mostrar mensajes NO leidos y total mensajes
            'lnkMsjNoLeidos.Text = MTG.MostrarNumMsjNoLeidos(Session("sscodigousuario")).ToString.Trim
            'lblTotalMsj.Text = MTG.MostrarNumMensajes(Session("sscodigousuario")).ToString.Trim

        End If
    End Sub

    Private Sub ContarSolicitudesCambioEstado()

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT COUNT(*) AS NumSol FROM SOLICITUDCAMBIOESTADO WHERE nivel_escalamiento = 3"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Session("ssNumSolicitudesCE") = Reader("NumSol").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
    End Sub

    Private Sub ContarMsjNoLeidos()
        Dim NumMensajes As Integer = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT COUNT(idunico) AS NumMensajes FROM mensajes WHERE " & _
            " (UsuDestino = '" & Session("sscodigousuario") & "') AND " & _
            "(leido = 0 OR leido IS NULL)"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NumMensajes = Reader("NumMensajes").ToString()
        End If
        Reader.Close()
        Connection.Close()

        'Return NumMensajes
        Session("ssNumMsgNoLeidos") = NumMensajes
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
        Session("EJEFISGLOBAL.RecordsFound") = 0
        If Len(Session("EJEFISGLOBAL.CurrentPage")) = 0 Then
            Session("EJEFISGLOBAL.CurrentPage") = 1

        End If
        If Len(Session("EJEFISGLOBAL.SortExpression")) = 0 Then
            Session("EJEFISGLOBAL.SortExpression") = "EFINROEXP"
            Session("EJEFISGLOBAL.SortDirection") = "ASC"
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
        Command.Parameters.AddWithValue("@EFINROEXP", "%" & txtSearchEFINROEXP.Text.Trim & "%")
        Command.Parameters.AddWithValue("@ED_NOMBRE", "%" & txtSearchED_NOMBRE.Text.Trim & "%")
        Command.Parameters.AddWithValue("@EFINUMMEMO", "%" & txtSearchEFINUMMEMO.Text.Trim)
        Command.Parameters.AddWithValue("@EFINIT", "%" & txtSearchEFINIT.Text.Trim & "%")
        Command.Parameters.AddWithValue("@EFIUSUASIG", "%" & cboSearchEFIUSUASIG.SelectedValue.Trim)
        Command.Parameters.AddWithValue("@EFIESTADO", "%" & cboSearchEFIESTADO.SelectedValue.Trim)

        'Llenar dataTable
        Dim Adaptador As New SqlDataAdapter(Command)
        Dim dtProcesos As New DataTable
        Adaptador.Fill(dtProcesos)
        NumRegs = ContarRegistros(dtProcesos)

        Dim MTG As New MetodosGlobalesCobro
        MTG.AjustarTerminos(dtProcesos, "EJEFISGLOBALREPARTIDOR")

        'Probando filtrado
        Dim SortOrder As String = Session("EJEFISGLOBAL.SortExpression")
        Dim PosicionPunto As Integer
        PosicionPunto = InStr(SortOrder, ".") + 1
        SortOrder = Mid(SortOrder, PosicionPunto)

        If SortOrder = "ED_Nombre" Then
            SortOrder = "ENTES_DEUDORESEFINITED_Nombre"
        ElseIf SortOrder = "nombre" Then
            SortOrder = "USUARIOSEFIUSUASIGnombre"
        End If

        Dim dtProcesos2 As New DataTable
        dtProcesos2 = FiltrarDataTable(dtProcesos, "termino LIKE '%" & txtTermino.Text.Trim & "%'", SortOrder)

        'grd.DataSource = dtProcesos
        grd.DataSource = dtProcesos2

        grd.DataBind()


        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("EJEFISGLOBAL.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("EJEFISGLOBAL.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False
        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PaginacionEjefisglobal * Session("EJEFISGLOBAL.CurrentPage") - PaginacionEjefisglobal + 1
        Dim StopRecord As Long = StartRecord + PaginacionEjefisglobal

        'Columnas
        Dim Columns As String = "[EJEFISGLOBAL].*, ENTES_DEUDORESEFINIT.ED_Nombre as ENTES_DEUDORESEFINITED_Nombre, ENTES_DEUDORESEFINIT.ED_Codigo_Nit, USUARIOSEFIUSUASIG.nombre as USUARIOSEFIUSUASIGnombre, ESTADOS_PROCESOEFIESTADO.nombre as ESTADOS_PROCESOEFIESTADOnombre, 'OK' AS termino, '      ' AS explicacion, '                    ' AS PictureURL "

        'Tablas y JOINs
        Dim Table As String = "(([EJEFISGLOBAL] left join [ENTES_DEUDORES] ENTES_DEUDORESEFINIT on [EJEFISGLOBAL].EFINIT = ENTES_DEUDORESEFINIT.ED_Codigo_Nit )  left join [USUARIOS] USUARIOSEFIUSUASIG on [EJEFISGLOBAL].EFIUSUASIG = USUARIOSEFIUSUASIG.codigo )  left join [ESTADOS_PROCESO] ESTADOS_PROCESOEFIESTADO on [EJEFISGLOBAL].EFIESTADO = ESTADOS_PROCESOEFIESTADO.codigo "

        'Where
        Dim WhereClause As String = ""
        If txtSearchEFINROEXP.Text.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFINROEXP] like @EFINROEXP"

        End If

        If txtSearchEFINUMMEMO.Text.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFINUMMEMO] like @EFINUMMEMO"
        End If

        If txtSearchED_NOMBRE.Text.Length > 0 Then
            WhereClause = WhereClause & " and [ENTES_DEUDORESEFINIT].[ED_NOMBRE] like @ED_NOMBRE"
        End If

        If txtSearchEFINIT.Text.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFINIT] like @EFINIT"
        End If

        If cboSearchEFIUSUASIG.SelectedValue.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFIUSUASIG] like @EFIUSUASIG"
        End If

        If cboSearchEFIESTADO.SelectedValue.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFIESTADO] like @EFIESTADO"
        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If
        Dim SortOrder As String = Session("EJEFISGLOBAL.SortExpression") & " " & Session("EJEFISGLOBAL.SortDirection")
        Dim sql As String = "WITH EJEFISGLOBALRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause

        End If
        sql = sql & " ),"
        sql = sql & " EJEFISGLOBALRecordCount AS ( SELECT * FROM EJEFISGLOBALRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM EJEFISGLOBALRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM EJEFISGLOBALRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        ModoAddEditRepartidor = "ADICIONAR"
        Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx")
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("EJEFISGLOBAL.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("EJEFISGLOBAL.txtSearchEFINROEXP") = txtSearchEFINROEXP.Text.Trim
        Session("EJEFISGLOBAL.txtSearchED_NOMBRE") = txtSearchED_NOMBRE.Text
        Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = txtSearchEFINUMMEMO.Text.Trim
        Session("EJEFISGLOBAL.txtSearchEFINIT") = txtSearchEFINIT.Text.Trim
        Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = cboSearchEFIUSUASIG.SelectedValue.Trim
        Session("EJEFISGLOBAL.cboSearchEFIESTADO") = cboSearchEFIESTADO.SelectedValue.Trim
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand        
        If e.CommandName = "" Then
            ModoAddEditRepartidor = "EDITAR"
            Dim EFINROEXP As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx?ID=" & EFINROEXP)
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("EJEFISGLOBAL.SortDirection"))
            Case "ASC"
                Session("EJEFISGLOBAL.SortDirection") = "DESC"
            Case "DESC"
                Session("EJEFISGLOBAL.SortDirection") = "ASC"
            Case Else
                Session("EJEFISGLOBAL.SortDirection") = "ASC"
        End Select

        Session("EJEFISGLOBAL.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            'Session("EJEFISGLOBAL.RecordsFound") = grd.DataSource("RecordSetCount")
            'NumRegs
            Session("EJEFISGLOBAL.RecordsFound") = NumRegs
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("EJEFISGLOBAL.RecordsFound") / PaginacionEjefisglobal)
        Dim PartialRecordCount As Long = Session("EJEFISGLOBAL.RecordsFound") Mod PaginacionEjefisglobal
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1

        End If
        If WholePageCount = 0 Then
            WholePageCount = 1

        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("EJEFISGLOBAL.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("EJEFISGLOBAL.CurrentPage") = Session("EJEFISGLOBAL.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("EJEFISGLOBAL.CurrentPage") > 1 Then
            Session("EJEFISGLOBAL.CurrentPage") = Session("EJEFISGLOBAL.CurrentPage") - 1
        Else
            Session("EJEFISGLOBAL.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("EJEFISGLOBAL.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Expedientes encontrados " & Session("EJEFISGLOBAL.RecordsFound")
        lblPageNumber.Text = "Página " & Session("EJEFISGLOBAL.CurrentPage") & " de " & GetPageCount()
    End Sub

    Protected Sub LoadcboSearchEFIESTADO()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()

        'Select statement that loads the combo box for searching the EFIESTADO column
        Dim sql As String = "select codigo, nombre from [ESTADOS_PROCESO] order by nombre"

        'Set the Command variable to a new instance of a SqlCommand object
        'Initialize it with the sql and Connection
        Dim Command As New SqlCommand(sql, Connection)

        'Set the DataTextField to nombre
        'The DataTextField linked to the field that is to be displayed in the combo box.
        cboSearchEFIESTADO.DataTextField = "nombre"

        'Set the DataValueField to codigo
        'The DataTextField linked to the field that will be returned when an item is selected.
        cboSearchEFIESTADO.DataValueField = "codigo"
        cboSearchEFIESTADO.DataSource = Command.ExecuteReader()
        cboSearchEFIESTADO.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub


    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click        
        CerrarSesion()
        Response.Redirect("../../login.aspx")
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
        MTG.ExportDataSetToExcel(ds, "GridRepartidor.xls")

    End Sub


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

    'Protected Sub AInformes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AInformes.Click        
    '    Response.Redirect("../FrmGrupoReportes.aspx")
    'End Sub

    Private Function ContarRegistros(ByVal dtTabla As DataTable) As Integer
        Dim NumRegistros As Integer = 0
        If dtTabla.Rows.Count > 0 Then
            NumRegistros = dtTabla.Rows(0).Item("RecordSetCount").ToString
        End If
        Return NumRegistros
    End Function


    Public Function FiltrarDataTable(ByVal pDataTable As DataTable, ByVal psFiltro As String, Optional ByVal psOrder As String = "EFINROEXP") As DataTable
        Dim loRows As DataRow()
        Dim loNuevoDataTable As DataTable

        If psOrder = "" Then
            psOrder = "EFINROEXP"
        End If

        ' Copio la estructura del DataTable original
        loNuevoDataTable = pDataTable.Clone()
        ' Establezco el filtro y el orden
        If psOrder = "" Then
            loRows = pDataTable.Select(psFiltro)
        Else
            loRows = pDataTable.Select(psFiltro, psOrder)
        End If

        ' Cargo el nuevo DataTable con los datos filtrados
        For Each ldrRow As DataRow In loRows
            loNuevoDataTable.ImportRow(ldrRow)
        Next

        ' Retorno el nuevo DataTable
        Return loNuevoDataTable
    End Function

    Protected Sub ACambio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ACambio.Click
        Response.Redirect("SOLICITUDCAMBIOESTADO.aspx")
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("MENSAJES.aspx")
    End Sub

    Protected Sub LoadcboSearchEFIUSUASIG()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()

        'Select statement that loads the combo box for searching the EFIUSUASIG column
        Dim sql As String = "select codigo, nombre from [USUARIOS] order by nombre"

        'Set the Command variable to a new instance of a SqlCommand object
        'Initialize it with the sql and Connection
        Dim Command As New SqlCommand(sql, Connection)

        'Set the DataTextField to nombre
        'The DataTextField linked to the field that is to be displayed in the combo box.
        cboSearchEFIUSUASIG.DataTextField = "nombre"

        'Set the DataValueField to codigo
        'The DataTextField linked to the field that will be returned when an item is selected.
        cboSearchEFIUSUASIG.DataValueField = "codigo"
        cboSearchEFIUSUASIG.DataSource = Command.ExecuteReader()
        cboSearchEFIUSUASIG.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        'Limpiar los cuadros de texto de busqueda
        'Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
        'Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
        'Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
        'Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
        'Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""
        'Session("Paginacion") = 10
        'Session("EJEFISGLOBAL.CurrentPage") = 1
        'Session("EJEFISGLOBAL.SortDirection") = "ASC"
        'Session.Remove("EJEFISGLOBAL.SortExpression")
        Session.RemoveAll()
    End Sub

End Class
