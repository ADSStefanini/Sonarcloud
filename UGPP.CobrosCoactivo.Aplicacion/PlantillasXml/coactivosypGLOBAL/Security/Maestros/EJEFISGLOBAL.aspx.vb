Imports System.Data.SqlClient

Partial Public Class EJEFISGLOBAL
    Inherits coactivosyp.BasePage


    Dim NumRegs As Integer = 0
    'Private PageSize As Long = 10
    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If MyBase.Page_Load(sender, e, ModalPopupError) Then
            Exit Sub
        End If

       
        'Evaluates to true when the page is loaded for the first time.
        If Not IsPostBack Then

            ContarMsjNoLeidos()


            LoadcboSearchEFIUSUASIG()
            LoadcboEFIESTADO()

            LoadcboMT_tipo_titulo()

            'Combo de la paginacion
            LoadcboNumExp()

            If Session("Paginacion") = Nothing Then
                Session("Paginacion") = 10
            End If

            PaginacionEjefisglobal = Session("Paginacion")
            cboNumExp.SelectedValue = PaginacionEjefisglobal

            'Puts the previous state of the txtSearchFECTITULO field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchFECTITULO.Text = Session("MAESTRO_TITULOS.txtSearchFECTITULO")

            'Puts the previous state of the txtSearchEFIFECENTGES field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchEFIFECENTGES.Text = Session("EJEFISGLOBAL.txtSearchEFIFECENTGES")

            'Puts the previous state of the txtSearchEFINROEXP field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchEFINROEXP.Text = Session("EJEFISGLOBAL.txtSearchEFINROEXP")

            'Puts the previous state of the txtSearchED_NOMBRE field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchED_NOMBRE.Text = Session("EJEFISGLOBAL.txtSearchED_NOMBRE")

            'Puts the previous state of the txtSearchEFINIT field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchEFINIT.Text = Session("EJEFISGLOBAL.txtSearchEFINIT")

            'Puts the previous state of the cboSearchEFIUSUASIG field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            cboSearchEFIUSUASIG.SelectedValue = Session("EJEFISGLOBAL.cboSearchEFIUSUASIG")

            'Estado
            cboEFIESTADO.SelectedValue = Session("EJEFISGLOBAL.cboEFIESTADO")


            If Session("mnivelacces") = 1 Or Session("mnivelacces") = 2 Then
                cmdMostrarEstadisticas.Visible = True
            Else
                cmdMostrarEstadisticas.Visible = False
            End If

            ' Solo los revisores, supervisores, superadmin y repartidor pueden ver todos los expedientes
            If Session("mnivelacces") = 4 Or Session("mnivelacces") = 6 Then
                cboSearchEFIUSUASIG.Enabled = False
                cboSearchEFIUSUASIG.SelectedValue = Session("sscodigousuario")
            End If


            BindGrid()

            'Instanciar clase de mensajes y estadisticas
            Dim MTG As New MetodosGlobalesCobro
            Dim NomPerfil As String = ""
            NomPerfil = MTG.GetNomPerfil(Session("mnivelacces"))
            lblNomPerfil.Text = NomPerfil

            If NomPerfil = "REVISOR" Or NomPerfil = "SUPERVISOR" Then
                ContarSolicitudesCambioEstado()
            Else
                'Ocultar el icono de solicitudes de cambio de estado
                ACambio.Visible = False
                divCambioEstado.Visible = False
            End If


        End If
    End Sub

    Protected Sub LoadcboMT_tipo_titulo()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [TIPOS_TITULO] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTabla As New DataTable
        Adaptador.Fill(dtTabla)
        If dtTabla.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaTabla As DataRow = dtTabla.NewRow()
            filaTabla("codigo") = "00"
            filaTabla("nombre") = "TODOS LOS TIPOS DE TITULO"
            dtTabla.Rows.Add(filaTabla)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaTabla As DataView = New DataView(dtTabla)
            vistaTabla.Sort = "codigo"
            '--------------------------------------------------------------------
            cboMT_tipo_titulo.DataSource = vistaTabla
            cboMT_tipo_titulo.DataTextField = "nombre"
            cboMT_tipo_titulo.DataValueField = "codigo"
            cboMT_tipo_titulo.DataBind()
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("EJEFISGLOBAL.RecordsFound") = 0
        If Len(Session("EJEFISGLOBAL.CurrentPage")) = 0 Then
            Session("EJEFISGLOBAL.CurrentPage") = 1

        End If
        If Len(Session("EJEFISGLOBAL.SortExpression")) = 0 Then
            Session("EJEFISGLOBAL.SortExpression") = "EFIFECENTGES"
            Session("EJEFISGLOBAL.SortDirection") = "DESC"
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
        Command.Parameters.AddWithValue("@FECTITULO", txtSearchFECTITULO.Text.Trim)
        Command.Parameters.AddWithValue("@FECENTGES", txtSearchEFIFECENTGES.Text.Trim)

        Command.Parameters.AddWithValue("@EFINROEXP", "%" & txtSearchEFINROEXP.Text.Trim & "%")
        Command.Parameters.AddWithValue("@ED_NOMBRE", "%" & txtSearchED_NOMBRE.Text.Trim & "%")
        Command.Parameters.AddWithValue("@EFINIT", "%" & txtSearchEFINIT.Text.Trim & "%")
        Command.Parameters.AddWithValue("@ESTADOPROC", cboEFIESTADO.SelectedValue)
        Command.Parameters.AddWithValue("@MT_TIPO_TITULO", cboMT_tipo_titulo.SelectedValue)
        Command.Parameters.AddWithValue("@EFIUSUASIG", "%" & cboSearchEFIUSUASIG.SelectedValue.Trim)

        'Llenar dataTable
        Dim Adaptador As New SqlDataAdapter(Command)
        Dim dtProcesos As New DataTable
        Adaptador.Fill(dtProcesos)
        NumRegs = ContarRegistros(dtProcesos)

        Dim MTG As New MetodosGlobalesCobro
        MTG.AjustarTerminos(dtProcesos, "EJEFISGLOBAL")

        '17/feb/2015. ajustar el capital pagado, sumandole el ajuste
        MTG.AjustarPagos(dtProcesos)

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

        SumarTotales(dtProcesos)
    End Sub


    Private Function GetSQL() As String
        Dim StartRecord As Long = PaginacionEjefisglobal * Session("EJEFISGLOBAL.CurrentPage") - PaginacionEjefisglobal + 1
        Dim StopRecord As Long = StartRecord + PaginacionEjefisglobal
        'Dim Columns As String = "[EJEFISGLOBAL].*"
        Dim Columns As String = "EJEFISGLOBAL.EFINROEXP,  EJEFISGLOBAL.EFIFECHAEXP, EJEFISGLOBAL.EFINIT, EJEFISGLOBAL.EFIFECENTGES," & _
                                    "EJEFISGLOBAL.EFIFECCAD, EJEFISGLOBAL.EFIVALDEU , EJEFISGLOBAL.EFIPAGOSCAP, EJEFISGLOBAL.EFISALDOCAP, " & _
                                    "ESTADOS_PROCESO.nombre AS EFIESTADO, ESTADOS_PAGO.nombre AS EFIESTUP, PERSUASIVO.FecEstiFin, " & _
                                    "ENTES_DEUDORES.ED_NOMBRE, " & _
                                    "'OK' AS termino, '      ' AS explicacion, '                    ' AS PictureURL,  " & _
                                    "USUARIOS.nombre AS GESTOR, " & _
                                    "COALESCE(TITULOSEJECUTIVOS.NomTipoTitulo,'') AS NomTipoTitulo, 0.00 AS PagoyAjuste "

        Dim Table As String = "EJEFISGLOBAL LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " & _
             "LEFT JOIN ESTADOS_PAGO ON EJEFISGLOBAL.EFIESTUP = ESTADOS_PAGO.codigo " & _
             "LEFT JOIN PERSUASIVO ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp " & _
             "LEFT JOIN ENTES_DEUDORES ON EJEFISGLOBAL.EFINIT = ENTES_DEUDORES.ED_Codigo_Nit " & _
             "LEFT JOIN USUARIOS ON EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo " & _
             "LEFT JOIN TITULOSEJECUTIVOS ON EJEFISGLOBAL.EFINROEXP = TITULOSEJECUTIVOS.MT_expediente"

        Dim WhereClause As String = ""

        If txtSearchFECTITULO.Text.Length > 0 Then
            'VALIDAR FECHA --WHERE CONVERT(DATE,EJEFISGLOBAL.EFIFECHAEXP) = CONVERT(DATE, @FECTITULO, 103)
            WhereClause = WhereClause & " and CONVERT(DATE,EJEFISGLOBAL.EFIFECHAEXP) = CONVERT(DATE, @FECTITULO, 103)"
        End If

        If txtSearchEFIFECENTGES.Text.Length > 0 Then
            'WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFIFECENTGES] like @FECENTGES"
            WhereClause = WhereClause & " and CONVERT(DATE,EJEFISGLOBAL.EFIFECENTGES) = CONVERT(DATE, @FECENTGES, 103)"
        End If

        If txtSearchEFINROEXP.Text.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFINROEXP] like @EFINROEXP"
        End If

        If txtSearchED_NOMBRE.Text.Length > 0 Then
            WhereClause = WhereClause & " and [ENTES_DEUDORES].[ED_NOMBRE] like @ED_NOMBRE"
        End If

        If txtSearchEFINIT.Text.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFINIT] like @EFINIT"
        End If

        '31/dic/2013: Validar el campo de los estados
        If Me.cboEFIESTADO.SelectedValue <> "00" Then
            WhereClause = WhereClause & " and [ESTADOS_PROCESO].[CODIGO] = @ESTADOPROC"
        End If

        If Me.cboMT_tipo_titulo.SelectedValue <> "00" Then
            WhereClause = WhereClause & " and TITULOSEJECUTIVOS.MT_TIPO_TITULO = @MT_TIPO_TITULO"
        End If

        '05/ene/2014: Expedientes del abogado actual edt// 19/ene/2014
        'If GetPerfil(Session("sscodigousuario")) = 4 Then   'GetPerfil(Session("sscodigousuario"))> 3
        '    WhereClause = WhereClause & " and EJEFISGLOBAL.EFIUSUASIG = '" & Session("sscodigousuario") & "'"
        'End If


        If cboSearchEFIUSUASIG.SelectedValue.Length > 0 Then
            '27/10/2014. Habilitar a los verificadores de pago para ver cualquier expediente, 
            'debido a que no deben tener restricciones para aplicar pagos  

            '04/12/2014. Contraorden, volver a dejar la restriccion
            'If GetPerfil(Session("sscodigousuario")) <> 6 Then
            ' El nivel o perfil es el de verificadores
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFIUSUASIG] like @EFIUSUASIG"
            'End If

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
        'sql = sql & "SELECT * FROM EJEFISGLOBALRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        sql = sql & _
                "SELECT MAX(RecordSetID) AS RecordSetID, EFINROEXP, MAX(EFIFECHAEXP) AS EFIFECHAEXP, MAX(EFINIT) AS EFINIT,                 " & _
                "			MAX(EFIFECENTGES) AS EFIFECENTGES, MAX(EFIFECCAD) AS EFIFECCAD, MAX(EFIVALDEU) AS EFIVALDEU,                    " & _
                "			MAX(EFIPAGOSCAP) AS EFIPAGOSCAP, MAX(EFISALDOCAP) AS EFISALDOCAP, MAX(EFIESTADO) AS EFIESTADO,                  " & _
                "			MAX(EFIESTUP) AS EFIESTUP, MAX(FecEstiFin) AS FecEstiFin, MAX(ED_NOMBRE) AS ED_NOMBRE, MAX(termino) AS termino,	" & _
                "			MAX(explicacion) AS explicacion, MAX(PictureURL) AS PictureURL, MAX(GESTOR) AS GESTOR,                          " & _
                "			MAX(NomTipoTitulo) AS NomTipoTitulo, MAX(RecordSetCount) AS RecordSetCount                                      " & _
                "		FROM EJEFISGLOBALRecordCount WHERE RecordSetID >= " & _
                StartRecord & " AND RecordSetID < " & StopRecord & " GROUP BY EFINROEXP"

        Return sql

    End Function

    Private Function GetPerfil(ByVal pUsuario As String) As String        
        Dim perfil As Integer = Session("mnivelacces")
        Return perfil
    End Function

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("EJEFISGLOBAL.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("MAESTRO_TITULOS.txtSearchFECTITULO") = txtSearchFECTITULO.Text
        Session("EJEFISGLOBAL.txtSearchEFIFECENTGES") = txtSearchEFIFECENTGES.Text

        Session("EJEFISGLOBAL.txtSearchEFINROEXP") = txtSearchEFINROEXP.Text
        Session("EJEFISGLOBAL.txtSearchED_NOMBRE") = txtSearchED_NOMBRE.Text
        Session("EJEFISGLOBAL.txtSearchEFINIT") = txtSearchEFINIT.Text
        Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = cboSearchEFIUSUASIG.SelectedValue.Trim
        Session("EJEFISGLOBAL.cboEFIESTADO") = cboEFIESTADO.SelectedValue
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim EFINROEXP As String = grd.Rows(e.CommandArgument).Cells(0).Text

            '12/jun/2014. Si el expediente esta en COACTIVO asegurar que tenga el regitro en la tabla COACTIVO
            Dim EstadoExpediente As String = grd.Rows(e.CommandArgument).Cells(6).Text.Trim
            If EstadoExpediente = "COACTIVO" Then
                If Not ExisteRegistroCoactivo(EFINROEXP) Then

                    Dim Connection As New SqlConnection(Funciones.CadenaConexion)
                    Connection.Open()
                    Dim Command As SqlCommand

                    'Comandos SQL 
                    Dim InsertSQL As String = "INSERT INTO COACTIVO (NroExp, CausalExtin, DecretoExtin, AlcanceExtin) VALUES (@NroExp, '00', '00', '00') "

                    'insert 
                    Command = New SqlCommand(InsertSQL, Connection)
                    Command.Parameters.AddWithValue("@NroExp", EFINROEXP)

                    Try
                        Command.ExecuteNonQuery()

                        'Después de cada GRABAR hay que llamar al log de auditoria
                        Dim LogProc As New LogProcesos
                        LogProc.SaveLog(Session("ssloginusuario"), "Módulo de coactivo", "Expediente " & EFINROEXP, Command)
                    Catch ex As Exception

                    End Try
                    Connection.Close()

                End If
            End If

            '01/SEP/2014. Si quien está logueado es verificador de pagos => mostrar la ventana de pagos
            If Session("mnivelacces") = 6 Then 'Verificador de pagos 
                Response.Redirect("PAGOS.aspx?pExpediente=" & EFINROEXP)
            Else
                Response.Redirect("EditEJEFISGLOBAL.aspx?ID=" & EFINROEXP)
            End If

        End If
    End Sub

    Private Function ExisteRegistroCoactivo(ByVal pNumExpediente As String) As Boolean
        Dim cmd As String = "SELECT NroExp FROM coactivo WHERE NroExp = '" & pNumExpediente.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, Funciones.CadenaConexion)
        Dim dtRegCoactivo As New DataTable
        Adaptador.Fill(dtRegCoactivo)
        If dtRegCoactivo.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

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

        psOrder = psOrder & " " & Session("EJEFISGLOBAL.SortDirection")

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
        '----------------------------------------------------------------'
        'Dim dt As New DataTable("tblEntTable")
        'dt.Columns.Add("ID", GetType(String))
        'dt.Columns.Add("amount", GetType(Decimal))
        'dt.Rows.Add(New Object() {"1", 100.51})
        'dt.Rows.Add(New Object() {"1", 200.52})
        'dt.Rows.Add(New Object() {"2", 500.24})
        'dt.Rows.Add(New Object() {"2", 400.31})
        'dt.Rows.Add(New Object() {"3", 600.88})
        'dt.Rows.Add(New Object() {"3", 700.11})

        'Dim result = (From orders In dt.AsEnumerable _
        '    Group orders By ID = orders.Field(Of String)("ID") Into g = Group _
        '    Select New With {Key ID, _
        '        .Amount = g.Sum(Function(r) r.Field(Of Decimal)("amount")) _
        '    }).OrderBy(Function(tkey) tkey.ID).ToList()
        '----------------------------------------------------------------'
        'Dim test2 = Table.AsEnumerable().GroupBy(Function(row) row.Item("Name1")).Select(Function(group) New With {.Grp = group.Key, .Sum = group.Sum(Function(r) Double.Parse(r.Item("Time").ToString()))})



        ' Retorno el nuevo DataTable
        Return loNuevoDataTable


    End Function

    Private Sub SumarTotales(ByVal dtTabla As DataTable)        
        If dtTabla.Rows.Count > 0 Then
            Dim objTotalDeuda As Object = dtTabla.Compute("Sum(EFIVALDEU)", "")
            Dim objTotalPagos As Object = dtTabla.Compute("Sum(EFIPAGOSCAP)", "")
            Dim objTotalSaldo As Object = dtTabla.Compute("Sum(EFISALDOCAP)", "")
            '
            Try
                txtTotalDeuda.Text = Convert.ToDouble(objTotalDeuda).ToString("N0")
            Catch ex1 As Exception
                txtTotalDeuda.Text = 0
            End Try
            Try
                txtTotalPagos.Text = Convert.ToDouble(objTotalPagos).ToString("N0")
            Catch ex2 As Exception
                txtTotalPagos.Text = 0
            End Try
            Try
                txtSaldoCapital.Text = Convert.ToDouble(objTotalSaldo).ToString("N0")
            Catch ex3 As Exception
                txtSaldoCapital.Text = 0
            End Try

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

    Protected Sub LoadcboEFIESTADO()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM estados_proceso ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEstados_Proceso As New DataTable
        Adaptador.Fill(dtEstados_Proceso)
        If dtEstados_Proceso.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtEstados_Proceso.NewRow()
            filaEstado("codigo") = "00"
            filaEstado("nombre") = "TODOS LOS ESTADOS"
            dtEstados_Proceso.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaEstados_Proceso As DataView = New DataView(dtEstados_Proceso)
            vistaEstados_Proceso.Sort = "codigo"
            '--------------------------------------------------------------------
            cboEFIESTADO.DataSource = vistaEstados_Proceso
            cboEFIESTADO.DataTextField = "nombre"
            cboEFIESTADO.DataValueField = "codigo"
            cboEFIESTADO.DataBind()
        End If
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()
        Response.Redirect("../../login.aspx")
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
        MTG.ExportDataSetToExcel(ds, "GridGestor.xls")

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

    Protected Sub lnkInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkInteres.Click
        Response.Redirect("capturarintereses.aspx")
    End Sub

    Protected Sub lnkInterMultas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkInterMultas.Click
        Response.Redirect("capturarinteresesmulta.aspx")
    End Sub

    Protected Sub lnkSql_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkSql.Click
        Response.Redirect("subirSQL.aspx")
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("MENSAJES.aspx")
    End Sub

    Protected Sub lnkConsultarPagos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkConsultarPagos.Click
        Response.Redirect("PAGOS.aspx")
    End Sub



    Protected Sub cmdMostrarEstadisticas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdMostrarEstadisticas.Click
        ' Si el perfil es 1 o 2 => Mostrar estadistica
        If Session("mnivelacces") = 1 Or Session("mnivelacces") = 2 Then
            Response.Redirect("EstadisticaxExpediente1.aspx")
        End If
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
        Session.RemoveAll()
    End Sub

    Protected Sub lnkInformes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkInformes.Click
        Response.Redirect("../FrmGrupoReportes.aspx")
    End Sub

    Protected Sub cmdMasivo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdMasivo.Click
        Response.Redirect("EJEFISGLOBAL_MASIVO.aspx")
    End Sub

    Protected Sub imgBtnBorraFechaRT_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaRT.Click
        txtSearchFECTITULO.Text = ""
        Session("EJEFISGLOBAL.txtSearchFECTITULO") = ""
    End Sub

    Protected Sub imgBtnBorraFechaEG_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaEG.Click
        txtSearchEFIFECENTGES.Text = ""
        Session("EJEFISGLOBAL.txtSearchEFIFECENTGES") = ""
    End Sub

    Private Sub ContarSolicitudesCambioEstado()

        Dim MTG As New MetodosGlobalesCobro
        Dim NomPerfil As String = MTG.GetNomPerfil(Session("mnivelacces"))

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = ""

        If NomPerfil = "REVISOR" Then
            sql = "SELECT COUNT(*) AS NumSol FROM SOLICITUDCAMBIOESTADO WHERE nivel_escalamiento = 1 AND revisor = '" & Session("sscodigousuario") & "'"

        ElseIf NomPerfil = "SUPERVISOR" Then
            sql = "SELECT COUNT(*) AS NumSol FROM SOLICITUDCAMBIOESTADO WHERE nivel_escalamiento = 2"

        Else
            sql = "SELECT COUNT(*) AS NumSol FROM SOLICITUDCAMBIOESTADO WHERE nivel_escalamiento = 1"

        End If

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Session("ssNumSolicitudesCE") = Reader("NumSol").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
    End Sub

    Protected Sub ACambio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ACambio.Click
        Response.Redirect("SOLICITUDCAMBIOESTADO.aspx")
    End Sub

End Class
