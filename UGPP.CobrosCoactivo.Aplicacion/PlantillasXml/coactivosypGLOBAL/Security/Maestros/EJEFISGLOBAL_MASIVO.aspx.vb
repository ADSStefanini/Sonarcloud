Imports System.Data.SqlClient

Partial Public Class EJEFISGLOBAL_MASIVO
    Inherits System.Web.UI.Page
    Dim NumRegs As Integer = 0
    'Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            ContarMsjNoLeidos()
            LoadcboEFIESTADO()
            'Combo de la paginacion
            LoadcboNumExp()

            LoadcboMT_tipo_titulo()

            If Session("Paginacion") = Nothing Then
                Session("Paginacion") = 10
            End If

            PaginacionEjefisglobal = Session("Paginacion")
            cboNumExp.SelectedValue = PaginacionEjefisglobal

            'Puts the previous state of the txtSearchFECTITULO field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchFECTITULO.Text = Session("MAESTRO_TITULOS.txtSearchFECTITULO")

            'Puts the previous state of the txtSearchEFIFECENTGES field done when the user has searched and moved to the EditEJEFISGLOBAL page and then came back
            txtSearchEFIFECENTGES.Text = Session("EJEFISGLOBAL.txtSearchEFIFECENTGES")

            'Estado
            cboEFIESTADO.SelectedValue = Session("EstadoMasivo")

            BindGrid()

            'Instanciar clase de mensajes y estadisticas
            Dim MTG As New MetodosGlobalesCobro
            lblNomPerfil.Text = MTG.GetNomPerfil(Session("mnivelacces"))

            'If cboMT_tipo_titulo.SelectedValue = "01" Then
            '    'Liquidacion Oficial
            '    cmdGenXLS.Visible = True
            'Else
            'cmdGenXLS.Visible = False
            'cmdGenXLS2.Visible = False
            'OpcionesXLS.Visible = False
            'End If

            If cboMT_tipo_titulo.SelectedValue = "01" Then
                cmdGenXLS.Visible = True
                cmdGenXLS2.Visible = False
                OpcionesXLS.Visible = False
            ElseIf cboMT_tipo_titulo.SelectedValue = "07" Then
                cmdGenXLS.Visible = False
                cmdGenXLS2.Visible = True
                OpcionesXLS.Visible = True
            Else
                cmdGenXLS.Visible = False
                cmdGenXLS2.Visible = False
                OpcionesXLS.Visible = False
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

        If IsNumeric(txtSearchEFIVALDEU.Text.Trim) Then
            Command.Parameters.AddWithValue("@EFIVALDEU", CDbl(txtSearchEFIVALDEU.Text.Trim))
        Else
            Command.Parameters.AddWithValue("@EFIVALDEU", "")
        End If
        Command.Parameters.AddWithValue("@ESTADOPROC", cboEFIESTADO.SelectedValue)
        Command.Parameters.AddWithValue("@MT_TIPO_TITULO", cboMT_tipo_titulo.SelectedValue)

        'Llenar dataTable
        Dim Adaptador As New SqlDataAdapter(Command)
        Dim dtProcesos As New DataTable
        Adaptador.Fill(dtProcesos)
        NumRegs = ContarRegistros(dtProcesos)

        Dim MTG As New MetodosGlobalesCobro
        MTG.AjustarTerminos(dtProcesos, Session("sscodigousuario"))
        'AjustarTerminos(dtProcesos)

        '17/feb/2015. ajustar el capital pagado, sumandole el ajuste
        MTG.AjustarPagos(dtProcesos)

        'grd.DataSource = Command.ExecuteReader()
        grd.DataSource = dtProcesos
        grd.DataBind()

        'Close the Connection Object 
        'cnx.Close()

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
                                    "'OK' AS termino,'      ' AS explicacion,'                    ' AS PictureURL," & _
                                    "COALESCE(TITULOSEJECUTIVOS.NomTipoTitulo,'') AS NomTipoTitulo "

        Dim Table As String = "EJEFISGLOBAL LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " & _
             "LEFT JOIN ESTADOS_PAGO ON EJEFISGLOBAL.EFIESTUP = ESTADOS_PAGO.codigo " & _
             "LEFT JOIN PERSUASIVO ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp " & _
             "LEFT JOIN ENTES_DEUDORES ON EJEFISGLOBAL.EFINIT = ENTES_DEUDORES.ED_Codigo_Nit " & _
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

        If txtSearchEFIVALDEU.Text.Length > 0 Then
            WhereClause = WhereClause & " and [EJEFISGLOBAL].[EFIVALDEU] >= @EFIVALDEU"
        End If

        '31/dic/2013: Validar el campo de los estados
        If Me.cboEFIESTADO.SelectedValue <> "00" Then
            WhereClause = WhereClause & " and [ESTADOS_PROCESO].[CODIGO] = @ESTADOPROC"
        End If

        '21/05/2014
        If Me.cboMT_tipo_titulo.SelectedValue <> "00" Then
            WhereClause = WhereClause & " and TITULOSEJECUTIVOS.MT_TIPO_TITULO = @MT_TIPO_TITULO"
        End If

        '05/ene/2014: Expedientes del abogado actual edt// 19/ene/2014
        If GetPerfil(Session("sscodigousuario")) = 4 Then   'GetPerfil(Session("sscodigousuario"))> 3
            WhereClause = WhereClause & " and EJEFISGLOBAL.EFIUSUASIG = '" & Session("sscodigousuario") & "'"
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
        Session("EJEFISGLOBAL.txtSearchEFIVALDEU") = txtSearchEFIVALDEU.Text
        Session("EstadoMasivo") = cboEFIESTADO.SelectedValue

        If cboMT_tipo_titulo.SelectedValue = "01" Then
            'Liquidacion Oficial
            cmdGenXLS.Visible = True
        Else
            cmdGenXLS.Visible = False
        End If

        If cboMT_tipo_titulo.SelectedValue = "07" Then
            'RESOLUCIÓN MULTA L1438/11 xxx
            cmdGenXLS2.Visible = True
            OpcionesXLS.Visible = True
        Else
            cmdGenXLS2.Visible = False
            OpcionesXLS.Visible = False
        End If

    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim EFINROEXP As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditEJEFISGLOBAL.aspx?ID=" & EFINROEXP)
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

    Private Function ContarRegistros(ByVal dtTabla As DataTable) As Integer
        Dim NumRegistros As Integer = 0
        If dtTabla.Rows.Count > 0 Then
            NumRegistros = dtTabla.Rows(0).Item("RecordSetCount").ToString
        End If
        Return NumRegistros
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
        dr("Codigo") = "50"
        dr("Descripcion") = "50"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Codigo") = "100"
        dr("Descripcion") = "100"
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
        FormsAuthentication.SignOut()

        'Limpiar cuadros de busqueda
        Session("MAESTRO_TITULOS.txtSearchFECTITULO") = ""
        Session("EJEFISGLOBAL.txtSearchEFIFECENTGES") = ""
        'Session("EJEFISGLOBAL.txtSearchEFINIT") = ""

        Response.Redirect("../../login.aspx")
    End Sub

    Protected Sub cboNumExp_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboNumExp.SelectedIndexChanged
        'PaginacionEjefisglobal = 30
        PaginacionEjefisglobal = cboNumExp.SelectedValue
        Session("Paginacion") = PaginacionEjefisglobal
        BindGrid()
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

    Protected Sub CheckHeader_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        For i As Integer = 0 To grd.Rows.Count - 1
            If Not grd.Rows(i).FindControl("EfiCheck") Is Nothing Then
                Dim cb As New CheckBox
                cb = grd.Rows(i).FindControl("EfiCheck")
                cb.Checked = CType(sender, CheckBox).Checked
            End If
        Next
    End Sub
    Protected Sub cmdMasivos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdMasivos.Click
        GenDTMasivos("../cobranzasMasiva2.aspx")
    End Sub

    Private Sub GenDTMasivos(ByVal RedirectPage As String)
        Dim cb As New CheckBox
        Dim Tb As New DataTable
        Tb.Columns.Add("exp")
        Tb.Columns.Add("Nom")
        For i As Integer = 0 To grd.Rows.Count - 1
            If Not grd.Rows(i).FindControl("EfiCheck") Is Nothing Then
                cb = grd.Rows(i).FindControl("EfiCheck")
                If cb.Checked = True Then
                    Tb.Rows.Add(New Object() {grd.Rows(i).Cells(1).Text, grd.Rows(i).Cells(1).Text & " - " & grd.Rows(i).Cells(3).Text})
                End If
            End If
        Next
        If Tb.Rows.Count >= 2 Then
            Session("ssimpuesto") = "4"
            Session("Dtexp") = Tb
            Response.Redirect(RedirectPage)
        Else
            Session("Dtexp") = Nothing
            menssageError("Para informes masivos debe seleccionar por lo menos 2 expedientes.")
        End If
    End Sub

    Private Sub menssageError(ByVal msn As String)
        'Me.ViewState("Erroruseractivo") = msn
        'ModalPopupError.Show()
        ViewState("message") = msn
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)

    End Sub

    Protected Sub cmdGenXLS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdGenXLS.Click
        Dim c As Integer = 0
        Dim ListaExpedientes As String = ""
        Dim cb As New CheckBox
        For i As Integer = 0 To grd.Rows.Count - 1
            If Not grd.Rows(i).FindControl("EfiCheck") Is Nothing Then
                cb = grd.Rows(i).FindControl("EfiCheck")
                If cb.Checked = True Then
                    c = c + 1
                    ListaExpedientes = ListaExpedientes & "'" & grd.Rows(i).Cells(1).Text.Trim & "',"
                End If
            End If
        Next
        If c >= 2 Then
            ' Hay 2 o mas registros seleccionados. Hacer un select de los exp seleccionados y generar el XLS
            ListaExpedientes = Left(ListaExpedientes, Len(ListaExpedientes) - 1)
            'menssageError(ListaExpedientes)

            ' Hacer consulta para crear datatable de la vista PERSUASIVOMASIVO solo con 'ListaExpedientes'
            Dim cnx As String = Funciones.CadenaConexion
            'Dim cmd As String = "SELECT *, dbo.CantidadConLetra([*VALOR_CIFRAS*]) AS '*VALOR_EN_LETRAS*' FROM PERSUASIVOMASIVO WHERE [*PROCESO*] IN (" & ListaExpedientes & ")"
            Dim cmd As String = "SELECT * FROM PERSUASIVOMASIVO3 WHERE [*PROCESO*] IN (" & ListaExpedientes & ")"
            Dim Adaptador As New SqlDataAdapter(cmd, cnx)
            Dim dtTabla As New DataTable
            Adaptador.Fill(dtTabla)
            If dtTabla.Rows.Count > 0 Then
                ' convertir datatable en excel

                'Instanciar clase de metodos globales
                Dim MTG As New MetodosGlobalesCobro

                '"Convertir" datatable a dataset
                Dim ds As New DataSet
                ds.Merge(dtTabla)

                'Exportar el dataset anterior a Excel 
                MTG.ExportDataSetToExcel(ds, "PersuasivoMasivoLiqOfi.xls")
            End If

        Else
            menssageError("Para informes masivos debe seleccionar por lo menos 2 expedientes.")
        End If
    End Sub

    Protected Sub cmdAceptarXLSMultas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAceptarXLSMultas.Click
        Dim c, i As Integer
        c = 0
        Dim ListaExpedientes As String = ""
        Dim cb As New CheckBox
        'Instanciar clase de metodos globales
        Dim MTG As New MetodosGlobalesCobro

        For i = 0 To grd.Rows.Count - 1
            If Not grd.Rows(i).FindControl("EfiCheck") Is Nothing Then
                cb = grd.Rows(i).FindControl("EfiCheck")
                If cb.Checked = True Then
                    c = c + 1
                    ListaExpedientes = ListaExpedientes & "'" & grd.Rows(i).Cells(1).Text.Trim & "',"
                End If
            End If
        Next
        If c >= 2 Then
            ' Hay 2 o mas registros seleccionados. Hacer un select de los exp seleccionados y generar el XLS
            ListaExpedientes = Left(ListaExpedientes, Len(ListaExpedientes) - 1)

            If txtFechaPrimerPago.Text.Trim = "" Or txtFechaSegundoPago.Text.Trim = "" Then
                menssageError("Digite las fechas de pago por favor")
                Exit Sub
            End If

            ' Hacer consulta para crear datatable de la vista PERSUASIVOMASIVO solo con 'ListaExpedientes'
            Dim cnx As String = Funciones.CadenaConexion
            'Dim cmd As String = "SELECT *, dbo.CantidadConLetra([*VALOR_CIFRAS*]) AS '*VALOR_EN_LETRAS*' FROM PERSUASIVOMASIVO WHERE [*PROCESO*] IN (" & ListaExpedientes & ")"
            Dim cmd As String = "SELECT * FROM PERSUASIVOMASIVO4 WHERE [*PROCESO*] IN (" & ListaExpedientes & ")"
            Dim Adaptador As New SqlDataAdapter(cmd, cnx)
            Dim dtTabla As New DataTable
            Adaptador.Fill(dtTabla)
            If dtTabla.Rows.Count > 0 Then
                'Declararvariables
                Dim Fecha1Pago, Fecha2Pago, FechaEjecutoria As Date
                Dim DiasMora1, DiasMora2 As Integer
                Dim valorCifra, Interes1, Interes2, Total1, Total2 As Int64

                'Recorrer el dataset y actualizar el campo *FECHA1PAGO*
                For i = 0 To dtTabla.Rows.Count - 1

                    'Convertir datos de string a date
                    FechaEjecutoria = Date.ParseExact(dtTabla.Rows(i).Item("*FECHADE EJECUTORIA*").ToString.Trim, "dd/MM/yyyy", Nothing)
                    Fecha1Pago = Date.ParseExact(txtFechaPrimerPago.Text.Trim, "dd/MM/yyyy", Nothing)
                    Fecha2Pago = Date.ParseExact(txtFechaSegundoPago.Text.Trim, "dd/MM/yyyy", Nothing)

                    'Restar dias
                    DiasMora1 = DateDiff(DateInterval.Day, FechaEjecutoria, Fecha1Pago) + 1
                    DiasMora2 = DateDiff(DateInterval.Day, FechaEjecutoria, Fecha2Pago) + 1

                    'Calculo de los intereses
                    valorCifra = dtTabla.Rows(i).Item("*VALOR_CIFRAS*")
                    Interes1 = (valorCifra * 0.06 * DiasMora1) / 365
                    Interes2 = (valorCifra * 0.06 * DiasMora2) / 365

                    'Calculo de los totales
                    Total1 = valorCifra + Interes1
                    Total2 = valorCifra + Interes2

                    'Llenar campo *DIASMORAP* y *DIASMORA2*
                    dtTabla.Rows(i).Item("*DIASMORAP*") = DiasMora1
                    dtTabla.Rows(i).Item("*DIASMORA2*") = DiasMora2

                    'Llenar el campo *FECHA1PAGO* y *FECHA2PAGO*
                    dtTabla.Rows(i).Item("*FECHA1PAGO*") = txtFechaPrimerPago.Text.Trim
                    dtTabla.Rows(i).Item("*FECHA2PAGO*") = txtFechaSegundoPago.Text.Trim

                    'Llenar el campo *INTERES* y *INTERESS*
                    dtTabla.Rows(i).Item("*INTERES*") = Interes1
                    dtTabla.Rows(i).Item("*INTERESS*") = Interes2

                    'Llenar el campo *TOTA1LPAGAR* y *TOTAL2*
                    dtTabla.Rows(i).Item("*TOTA1LPAGAR*") = Total1
                    dtTabla.Rows(i).Item("*TOTAL2*") = Total2

                Next

                '"Convertir" datatable a dataset
                Dim ds As New DataSet
                ds.Merge(dtTabla)

                'Exportar el dataset anterior a Excel 
                MTG.ExportDataSetToExcel(ds, "PersuasivoMasivoMulta.xls")
            End If

        Else
            menssageError("Para informes masivos debe seleccionar por lo menos 2 expedientes.")
        End If
    End Sub

    Protected Sub cmdGenXLS2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdGenXLS2.Click
        If cboMT_tipo_titulo.SelectedValue = "05" Then
            'RESOLUCIÓN MULTA L1438/11            
            OpcionesXLS.Visible = True
        Else            
            OpcionesXLS.Visible = False
        End If
    End Sub

    Protected Sub imgBtnBorraFechaRT_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaRT.Click
        txtSearchFECTITULO.Text = ""
        Session("EJEFISGLOBAL.txtSearchFECTITULO") = ""
    End Sub

    Protected Sub imgBtnBorraFechaEG_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaEG.Click
        txtSearchEFIFECENTGES.Text = ""
        Session("EJEFISGLOBAL.txtSearchEFIFECENTGES") = ""
    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("EJEFISGLOBAL.aspx")
    End Sub

    Protected Sub CambioEstadoMasivo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CambioEstadoMasivo.Click
        If Session("EstadoMasivo") = "00" Or Session("EstadoMasivo") = Nothing Then 'cboEFIESTADO.SelectedValue = "00" Then
            menssageError("Para los cambios de estado masivo, debe aplicar el filtro 'Estado actual'")
        Else
            GenDTMasivos("SolicitudCambioEstadoMasivo.aspx")
        End If
    End Sub
End Class
