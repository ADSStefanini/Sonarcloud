Imports System.Data.SqlClient
Imports System.Linq
Imports System.Threading.Tasks
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class EJEFISGLOBAL
    Inherits coactivosyp.BasePage

    Dim NumRegs As Integer = 0
    Dim CacheKey As String = "dtProcesos"

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
            LoadcboSearchTERMINO()

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
            NomPerfil = CommonsCobrosCoactivos.getNomPerfil(Session)
            lblNomPerfil.Text = NomPerfil

            If NomPerfil = "REVISOR" Or NomPerfil = "SUPERVISOR" Then
                ContarSolicitudesCambioEstado()
            Else
                'Ocultar el icono de solicitudes de cambio de estado
                ACambio.Visible = False
                divCambioEstado.Visible = False
            End If

            'Actualización de la prioridad del título
            Dim _bandejaBLL As BandejaBLL = New BandejaBLL()
            _bandejaBLL.actualizarPriorizacionTitulo(Session("ssloginusuario"))

            'Poblado de datos para gestores que puedes ser solicitados en la solicitud de reasignación
            SolicitudReasignacionPanel.poblarGestorSolicitadoParaReasignacion(2)
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

        Dim dtProcesos As New DataTable
        'Crear las columnas 
        dtProcesos.Columns.Add("RecordSetID", GetType(Int64))
        dtProcesos.Columns.Add("EFINROEXP", GetType(String))
        dtProcesos.Columns.Add("EFIFECHAEXP", GetType(DateTime))
        dtProcesos.Columns.Add("EFINIT", GetType(String))
        dtProcesos.Columns.Add("EFIFECENTGES", GetType(DateTime))
        dtProcesos.Columns.Add("EFIFECCAD", GetType(DateTime))
        dtProcesos.Columns.Add("EFIPAGOSCAP", GetType(Decimal))
        dtProcesos.Columns.Add("EFISALDOCAP", GetType(Double))
        dtProcesos.Columns.Add("EFIESTADOCODIGO", GetType(String))
        dtProcesos.Columns.Add("EFIESTADO", GetType(String))
        dtProcesos.Columns.Add("EFIESTUP", GetType(String))
        dtProcesos.Columns.Add("FecEstiFin", GetType(DateTime))
        dtProcesos.Columns.Add("ED_NOMBRE", GetType(String))
        dtProcesos.Columns.Add("termino", GetType(String))
        dtProcesos.Columns.Add("explicacion", GetType(String))
        dtProcesos.Columns.Add("PictureURL", GetType(String))
        dtProcesos.Columns.Add("USUARIOSCODIGO", GetType(String))
        dtProcesos.Columns.Add("GESTOR", GetType(String))
        dtProcesos.Columns.Add("ESTADO_OPERATIVO", GetType(String))
        dtProcesos.Columns.Add("MT_TIPO_TITULO", GetType(String))
        dtProcesos.Columns.Add("NomTipoTitulo", GetType(String))
        dtProcesos.Columns.Add("RecordSetCount", GetType(Int64))
        dtProcesos.Columns.Add("ID_TAREA_ASIGANDA", GetType(Integer))
        dtProcesos.Columns.Add("VAL_PRIORIDAD", GetType(Integer))
        dtProcesos.Columns.Add("COD_ESTADO_OPERATIVO", GetType(Integer))
        dtProcesos.Columns.Add("COLORSUSPENSION", GetType(String))
        dtProcesos.Columns.Add("FECHALIMITE", GetType(DateTime))

        'Se inicia proceso para conultar los datos a partir del procedimiento almacenado, este consultando la tabla tarea_asiganda

        'Se valida el perfil del usuario logeado para saber si se muestran todos los expedientes o solo los asignados
        Dim usuario = String.Empty
        Dim usuarioNoIncluir = String.Empty
        Dim _usuarioBLL As New UsuariosBLL()
        'Si el usuario es gestor o si es revisor y se solicita ver la bandeja de espedientes asignados se asigna el combo con el gestor relacionado
        If (Session("mnivelacces") = 4 Or Session("mnivelacces") = 6) Or (Len(Request("filtro")) = 0 And Session("mnivelacces") = 3) Then
            usuario = Session("ssloginusuario")
            cboSearchEFIUSUASIG.Enabled = False
            cboSearchEFIUSUASIG.SelectedValue = Session("sscodigousuario")

        ElseIf cboSearchEFIUSUASIG.SelectedIndex <> 0 Then
            Dim usuarioCombo = cboSearchEFIUSUASIG.SelectedValue
            Dim usuarioConsulta = _usuarioBLL.obtenerUsuarioPorId(usuarioCombo)
            usuario = usuarioConsulta.login
        End If

        If Len(Request("filtro")) = 0 And Session("mnivelacces") = 3 Then
            usuarioNoIncluir = Session("ssloginusuario")
        End If

        If Len(Request("filtro")) = 1 AndAlso (Session("mnivelacces") = 3 Or Session("mnivelacces") = 2) Then
            'Se ocultan elementos que solo se muestran para los gestores que pueden realizar solicitudes
            grd.Columns(18).Visible = False
            grd.Columns(19).Visible = False
            btnSolicitarReasignacion.Visible = False
        End If

        'Se elimina lo que esta antes del punto en la variable de sesión EJEFISGLOBAL.SortExpression
        If Session("EJEFISGLOBAL.SortExpression") <> String.Empty Then
            Dim SortExpression = Session("EJEFISGLOBAL.SortExpression").Substring(Session("EJEFISGLOBAL.SortExpression").LastIndexOf(".") + 1)
            Session("EJEFISGLOBAL.SortExpression") = SortExpression
        End If

        ''Se crea el obgeto de consulta
        Dim _consultaExpedientes As New ConsultaExpedientes()
        _consultaExpedientes.StartRecord = PaginacionEjefisglobal * Session("EJEFISGLOBAL.CurrentPage") - PaginacionEjefisglobal + 1
        _consultaExpedientes.StopRecord = _consultaExpedientes.StartRecord + PaginacionEjefisglobal
        _consultaExpedientes.SortExpression = Session("EJEFISGLOBAL.SortExpression")
        _consultaExpedientes.SortDirection = Session("EJEFISGLOBAL.SortDirection")
        _consultaExpedientes.UsuarioLogin = usuario
        _consultaExpedientes.NumeroExpediente = txtSearchEFINROEXP.Text
        _consultaExpedientes.NombreDeudor = txtSearchED_NOMBRE.Text
        _consultaExpedientes.NumeroDocDeudor = txtSearchEFINIT.Text
        _consultaExpedientes.CodEstadoProcesal = If(cboEFIESTADO.SelectedIndex <> 0, cboEFIESTADO.SelectedValue, String.Empty)
        _consultaExpedientes.CodTipoTitulo = If(cboMT_tipo_titulo.SelectedIndex <> 0, cboMT_tipo_titulo.SelectedValue, String.Empty)
        _consultaExpedientes.FechaEntragaTitulo = txtSearchFECTITULO.Text
        _consultaExpedientes.FechaAsignacionGestor = txtSearchEFIFECENTGES.Text
        _consultaExpedientes.EstadoOperativo = 0
        _consultaExpedientes.UsuarioNoIncluir = usuarioNoIncluir

        'Se crea el objeto para consultas
        Dim _expedienteBLL As New ExpedienteBLL()
        dtProcesos = _expedienteBLL.obtenerExpedientesAsignados(_consultaExpedientes, dtProcesos)

        Dim MTG As New MetodosGlobalesCobro
        MTG.AjustarTerminos(dtProcesos, "EJEFISGLOBAL")

        '17/feb/2015. ajustar el capital pagado, sumandole el ajuste
        MTG.AjustarPagos(dtProcesos)

        NumRegs = ContarRegistros(dtProcesos)

        Dim dtProcesos2 As New DataTable
        'Jeisson Gómez 
        ' Cambio de combo con el fin que consulte todos los registros. 
        ' dtProcesos2 = FiltrarDataTable(dtProcesos, "termino Like '%" & txtTermino.Text.Trim & "%'", SortOrder)

        ' Reversar la HU_003 07/03/2017 Jeisson Gómez
        ' Jeisson Gómez HU_003 ALARMAS 23/04/2017 
        ' Se cambia por la descripción entregada por la UGPP 
        ' Se modifica HU_003 19/05/2017
        Dim strFiltro As String = String.Empty
        If Not cboSearchTERMINO.SelectedItem.Value.ToString().Equals("00") Then
            strFiltro = " termino LIKE '%" & cboSearchTERMINO.SelectedItem.Text.Trim & "%'"
        End If

        dtProcesos2 = FiltrarDataTable(dtProcesos, strFiltro, "")

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

        'Se recorre el gridview para habilitar o deshabilitar el botón de "Editar"
        DeshabilitarBotones()
    End Sub

    Protected Sub DeshabilitarBotones()
        'Si no cuenta con registros se sale del método
        If grd.Rows.Count = 0 Then
            Exit Sub
        End If

        For i As Integer = 0 To grd.Rows.Count - 1
            Dim row As GridViewRow = grd.Rows(i)
            Dim btnEditar As Button = CType(row.Cells(17).Controls(0), Button)
            Dim esPrioridad As String = row.Cells(21).Text
            Dim codEstadoOperativo As String = row.Cells(22).Text
            If esPrioridad = "1" Then
                'Se oculta el botón de priorizar para los expedientes que si se pueden gestionar
                Dim btnPriorizar As Button = CType(row.Cells(19).Controls(0), Button)
                btnPriorizar.Visible = False

                If codEstadoOperativo = "11" Then
                    btnEditar.Text = "Gestionar"
                ElseIf codEstadoOperativo = "15" Then
                    btnEditar.Text = "Continuar"
                ElseIf codEstadoOperativo = "14" OrElse codEstadoOperativo = "19" OrElse codEstadoOperativo = "12" Then
                    btnEditar.Text = "Retomar"
                End If
            Else
                btnEditar.Enabled = False
                btnEditar.Visible = False
            End If
            'Prioridad 21
            'Dim _prioridad
            Dim checkReasignación As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
            If codEstadoOperativo = "3" Or codEstadoOperativo = "13" Then
                checkReasignación.Visible = False
            End If

            If Len(Request("filtro")) = 1 AndAlso (Session("mnivelacces") = 3 Or Session("mnivelacces") = 2) Then
                btnEditar.Text = "Editar"
                btnEditar.Visible = True
            End If
        Next

    End Sub

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

        If e.CommandName = "cmdPriorizar" Then
            Dim codTareaAsiganada As String = grd.Rows(e.CommandArgument).Cells(20).Text
            SolicitudPriorizacionControl.AsignarTareaAsiganada(codTareaAsiganada)
            SolicitudPriorizacionControl.IniciarFormulario()
            SolicitudPriorizacionControl.MostrarModal()

        ElseIf e.CommandName = "" Then
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
                'Response.Redirect("PAGOS.aspx?pExpediente=" & EFINROEXP)
                Response.Redirect("EditEJEFISGLOBAL.aspx?ID=" & EFINROEXP)
            Else
                '/-----------------------------------------------------------------  
                'ID _HU:  005 
                'Nombre HU   : Redireccionamiento del botón Cancelar 
                'Empresa: UT TECHNOLOGY 
                'Autor: Jeisson Gómez 
                'Fecha: 20-01-2017  
                'Objetivo : Redireccionar a la página anterior en el botón cancelar, para cualquier 
                '           perfil de usuario en la aplicación.
                '------------------------------------------------------------------/
                Session("MenuPrincipal") = False
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
        'Jeisson Gómez 
        ' 06/07/2017 - Se comenta para que procese como estaba anteriormente.
        ' Me traje las variables StartRecord y StopRecord para la paginación.
        'Dim StartRecord As Long = PaginacionEjefisglobal * Session("EJEFISGLOBAL.CurrentPage") - PaginacionEjefisglobal + 1
        'Dim StopRecord As Long = StartRecord + PaginacionEjefisglobal - 1

        Dim loRows As DataRow()
        Dim loNuevoDataTable As DataTable

        ' Copio la estructura del DataTable original
        loNuevoDataTable = pDataTable.Clone()

        ' Establezco el filtro y el orden
        If psOrder = "" Then
            loRows = pDataTable.Select(psFiltro)
        Else
            loRows = pDataTable.Select(psFiltro, psOrder)
        End If

        For Each ldrRow As DataRow In loRows
            loNuevoDataTable.ImportRow(ldrRow)
        Next

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
        Dim cmd As String = "Select codigo, nombre FROM estados_proceso ORDER BY nombre"
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

    Protected Sub LoadcboSearchTERMINO()
        '/-----------------------------------------------------------------  
        'ID _HU:  003
        'Nombre HU   : Ajuste a la funcionalidad del Campo “Alerta - Termino”.
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 14-01-2017  
        'Objetivo : Llenar el combo con la consulta del campo nombre de la tabla ALARMAS
        '------------------------------------------------------------------/
        Dim MGC As MetodosGlobalesCobro = New MetodosGlobalesCobro

        cboSearchTERMINO.DataSource = MGC.DictAlarmas.OrderBy(Function(x) x.Value).ToList
        cboSearchTERMINO.DataValueField = "key"
        cboSearchTERMINO.DataTextField = "value"
        cboSearchTERMINO.DataBind()

        cboSearchTERMINO.Items.Insert(0, New ListItem("TODOS LOS TERMINOS", "00"))

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
        dt.Columns.Remove("Column1")
        dt.Columns.Remove("Column2")
        dt.Columns.Remove("Column3")
        dt.Columns.Remove("Column4")
        dt.Columns.Remove("Column5")
        dt.Columns.Remove("Column6")
        dt.Columns.Remove("Column7")
        dt.Columns.Remove("Column8")
        dt.Columns.Remove("Column9")
        dt.Columns.Remove("Column10")

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
        Dim sql As String = "Select COUNT(idunico) As NumMensajes FROM mensajes WHERE " &
            " (UsuDestino = '" & Session("sscodigousuario") & "') AND " &
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

        '/-----------------------------------------------------------------  
        'ID _HU:  001 
        'Nombre HU   : Usuarios en estado inactivos 
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 06-01-2017 
        'Objetivo : Se puso a la consulta la condición useractivo = 1. 
        '           Con el fin de traer solo los usuarios activos.
        '------------------------------------------------------------------/
        'Select statement that loads the combo box for searching the EFIUSUASIG column
        Dim sql As String = "select codigo, nombre from [USUARIOS] where useractivo = 1 order by nombre"

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
        'For Each de As DictionaryEntry In HttpContext.Current.Cache
        '    HttpContext.Current.Cache.Remove(DirectCast(de.Key, String))
        'Next
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

        Dim NomPerfil As String = CommonsCobrosCoactivos.getNomPerfil(Session)

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = ""

        If NomPerfil = "REVISOR" Then
            sql = "SELECT COUNT(*) AS NumSol FROM SOLICITUDCAMBIOESTADO WHERE nivel_escalamiento = 1 and accion = 'PENDIENTE' AND revisor = '" & Session("sscodigousuario") & "'"

        ElseIf NomPerfil = "SUPERVISOR" Then
            sql = "SELECT COUNT(*) AS NumSol FROM SOLICITUDCAMBIOESTADO WHERE nivel_escalamiento = 2 and accion = 'PENDIENTE'"

        Else
            sql = "SELECT COUNT(*) AS NumSol FROM SOLICITUDCAMBIOESTADO WHERE nivel_escalamiento = 1 and accion = 'PENDIENTE'"

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
        '/-----------------------------------------------------------------  
        'ID _HU:  005 
        'Nombre HU   : Redireccionamiento del botón Cancelar 
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 20-01-2017  
        'Objetivo : Redireccionar a la página anterior en el botón cancelar, para cualquier 
        '           perfil de usuario en la aplicación.
        '------------------------------------------------------------------/
        Session("MenuPrincipal") = True
        Response.Redirect("SOLICITUDCAMBIOESTADO.aspx")
    End Sub

    Protected Function InsertarCache(dt As DataTable) As DataTable
        '/-----------------------------------------------------------------  
        'ID _HU:  003 
        'Nombre HU   : Alarmas 
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 19-05-2017  
        'Objetivo : Para que se pueda navegar en la primera panatalla mas rapidamente y establecer el combo del filtro.
        '------------------------------------------------------------------/
        Dim CacheItem As Object = CType(Cache(CacheKey), DataTable)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            If CacheItem Is Nothing Then
                CacheItem = dt
                HttpContext.Current.Cache.Insert(CacheKey, CacheItem, Nothing, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, Nothing)
            End If
        End If

        Return CType(CacheItem, DataTable)

    End Function

    Protected Sub btnSolicitarReasignacion_Click(sender As Object, e As EventArgs) Handles btnSolicitarReasignacion.Click
        Dim _expedientesSeleccionados As New List(Of Integer)
        For i As Integer = 0 To grd.Rows.Count - 1
            Dim row As GridViewRow = grd.Rows(i)
            Dim chkReasignacion As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
            If (chkReasignacion.Checked) Then
                Dim codTareaAsiganada As String = row.Cells(20).Text
                _expedientesSeleccionados.Add(Convert.ToInt32(codTareaAsiganada))
            End If
        Next
        SolicitudReasignacionPanel.AsignarTareasAsiganadas(_expedientesSeleccionados)
        SolicitudReasignacionPanel.IniciarFormulario()
        SolicitudReasignacionPanel.MostrarModal()
    End Sub

    Protected Sub btnSolicitarCambioEstado_Click(sender As Object, e As EventArgs) Handles btnSolicitarCambioEstado.Click
        Dim _expedientesSeleccionados As New List(Of Integer)
        For i As Integer = 0 To grd.Rows.Count - 1
            Dim row As GridViewRow = grd.Rows(i)
            Dim chkReasignacion As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
            If (chkReasignacion.Checked) Then
                Dim codTareaAsiganada As String = row.Cells(20).Text
                _expedientesSeleccionados.Add(Convert.ToInt32(codTareaAsiganada))
            End If
        Next

        If (_expedientesSeleccionados.Count > 0) Then
            hdnExpedientes.Value = String.Join(",", _expedientesSeleccionados)
        End If

    End Sub

    Protected Sub ABack_Click(sender As Object, e As EventArgs) Handles ABack.Click
        Response.Redirect("~/Security/Modulos.aspx", True)
    End Sub
End Class