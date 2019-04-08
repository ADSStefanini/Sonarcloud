Imports System.Data
Imports System.Data.SqlClient
Partial Public Class PAGOS
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            ContarMsjNoLeidos()

            'Loads elements from the codigo table to be searched on
            LoadcboSearchestado()

            LoadcboSearchUserSolicita()

            'Puts the previous state of the txtSearchNroConsignacion field done when the user has searched and moved to the EditPAGOS page and then came back
            txtSearchNroConsignacion.Text = Session("PAGOSNOMBRE.txtSearchNroConsignacion")

            'Faltaba este 
            txtSearchExpediente.Text = Session("PAGOSNOMBRE.txtSearchExpediente")

            'Puts the previous state of the cboSearchestado field done when the user has searched and moved to the EditPAGOS page and then came back
            cboSearchestado.SelectedValue = Session("PAGOSNOMBRE.cboSearchestado")

            'Puts the previous state of the cboSearchUserSolicita field done when the user has searched and moved to the EditPAGOS page and then came back
            cboSearchUserSolicita.SelectedValue = Session("PAGOSNOMBRE.cboSearchUserSolicita")
            BindGrid()

            'Instanciar clase de mensajes y estadisticas
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)

            '01/SEP/2014
            Dim pExpediente As String = ""
            pExpediente = Request("pExpediente")
            txtSearchExpediente.Text = pExpediente
            Buscar()

        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("PAGOSNOMBRE.RecordsFound") = 0
        If Len(Session("PAGOSNOMBRE.CurrentPage")) = 0 Then
            Session("PAGOSNOMBRE.CurrentPage") = 1

        End If
        If Len(Session("PAGOSNOMBRE.SortExpression")) = 0 Then
            Session("PAGOSNOMBRE.SortExpression") = "FecSolverif"
            Session("PAGOSNOMBRE.SortDirection") = "DESC"

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@NroConsignacion", "%" & txtSearchNroConsignacion.Text.Trim)
        '
        Command.Parameters.AddWithValue("@Expediente", "%" & txtSearchExpediente.Text.Trim)

        Command.Parameters.AddWithValue("@estado", "%" & cboSearchestado.SelectedValue)

        Command.Parameters.AddWithValue("@UserSolicita", "%" & cboSearchUserSolicita.SelectedValue)

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("PAGOSNOMBRE.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("PAGOSNOMBRE.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    'Private Function GetSQL() As String
    '    Dim StartRecord As Long = PageSize * Session("PAGOS.CurrentPage") - PageSize + 1
    '    Dim StopRecord As Long = StartRecord + PageSize
    '    Dim Columns As String = "PAGOS.*, ESTADOS_PAGOestado.nombre as ESTADOS_PAGOestadonombre, USUARIOSUserSolicita.nombre as USUARIOSUserSolicitanombre, ESTADOS_PROCESOpagestadoprocfrp.nombre as ESTADOS_PROCESOpagestadoprocfrpnombre"
    '    Dim Table As String = "((PAGOS left join [ESTADOS_PAGO] ESTADOS_PAGOestado on PAGOS.estado = ESTADOS_PAGOestado.codigo )  left join USUARIOS USUARIOSUserSolicita on PAGOS.UserSolicita = USUARIOSUserSolicita.codigo )  left join ESTADOS_PROCESO ESTADOS_PROCESOpagestadoprocfrp on PAGOS.pagestadoprocfrp = ESTADOS_PROCESOpagestadoprocfrp.codigo "
    '    Dim WhereClause As String = ""
    '    If txtSearchNroConsignacion.Text.Length > 0 Then
    '        WhereClause = WhereClause & " and PAGOS.NroConsignacion like @NroConsignacion"

    '    End If

    '    If txtSearchExpediente.Text.Length > 0 Then
    '        WhereClause = WhereClause & " and PAGOS.NroExp like @Expediente"

    '    End If

    '    If cboSearchestado.SelectedValue.Length > 0 Then
    '        WhereClause = WhereClause & " and PAGOS.estado like @estado"

    '    End If

    '    If cboSearchUserSolicita.SelectedValue.Length > 0 Then
    '        WhereClause = WhereClause & " and PAGOS.UserSolicita like @UserSolicita"

    '    End If

    '    If WhereClause.Length > 0 Then
    '        WhereClause = Replace(WhereClause, " and ", "", , 1)

    '    End If
    '    Dim SortOrder As String = Session("PAGOS.SortExpression") & " " & Session("PAGOS.SortDirection")
    '    Dim sql As String = "WITH PAGOSRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
    '    If Len(WhereClause) > 0 Then
    '        sql = sql & " where " & WhereClause

    '    End If
    '    sql = sql & " ),"
    '    sql = sql & " PAGOSRecordCount AS ( SELECT * FROM PAGOSRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PAGOSRecordSet) AS RC ) "
    '    sql = sql & "SELECT * FROM PAGOSRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
    '    Return sql

    'End Function

    Private Function GetSQL() As String
        '28/ago/2014
        'La vista PAGOSNOMBRE está contruida asi:
        'SELECT pagos.NroConsignacion, pagos.NroExp, PAGOS.FecSolverif, PAGOS.FecVerificado, PAGOS.pagFecha,
        '		PAGOS.pagFechaDeudor, PAGOS.pagTotal, PAGOS.estado,
        '		ESTADOS_PAGO.nombre AS NombreEstadoPago, 
        '		USUARIOS.nombre AS NombreUsuario, 
        '		ESTADOS_PROCESO.nombre AS NombreEstadoProc
        '	FROM pagos
        '		LEFT JOIN ESTADOS_PAGO ON PAGOS.estado = ESTADOS_PAGO.codigo   
        '		LEFT JOIN USUARIOS ON PAGOS.UserSolicita = USUARIOS.codigo   
        '		LEFT JOIN ESTADOS_PROCESO ON PAGOS.pagestadoprocfrp = ESTADOS_PROCESO.codigo 

        Dim StartRecord As Long = PageSize * Session("PAGOSNOMBRE.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "EJEFISGLOBAL.EFINROEXP, PAGOSNOMBRE.*"
        Dim Table As String = "EJEFISGLOBAL LEFT JOIN PAGOSNOMBRE ON EJEFISGLOBAL.EFINROEXP = PAGOSNOMBRE.NroExp "
        Dim WhereClause As String = ""

        If txtSearchNroConsignacion.Text.Length > 0 Then
            WhereClause = WhereClause & " and PAGOSNOMBRE.NroConsignacion like @NroConsignacion"

        End If

        If txtSearchExpediente.Text.Length > 0 Then
            WhereClause = WhereClause & " and PAGOSNOMBRE.NroExp like @Expediente"

        End If

        If cboSearchestado.SelectedValue.Length > 0 Then
            WhereClause = WhereClause & " and PAGOSNOMBRE.estado like @estado"

        End If

        If cboSearchUserSolicita.SelectedValue.Length > 0 Then
            WhereClause = WhereClause & " and PAGOSNOMBRE.UserSolicita like @UserSolicita"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)

        End If
        Dim SortOrder As String = Session("PAGOSNOMBRE.SortExpression") & " " & Session("PAGOSNOMBRE.SortDirection")
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
        Buscar()
    End Sub

    Private Sub Buscar()
        Session("PAGOSNOMBRE.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("PAGOSNOMBRE.txtSearchNroConsignacion") = txtSearchNroConsignacion.Text
        Session("PAGOSNOMBRE.txtSearchExpediente") = txtSearchExpediente.Text
        Session("PAGOSNOMBRE.cboSearchestado") = cboSearchestado.SelectedValue
        Session("PAGOSNOMBRE.cboSearchUserSolicita") = cboSearchUserSolicita.SelectedValue
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim NroConsignacion As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim Nroexpediente As String = grd.Rows(e.CommandArgument).Cells(2).Text
            Dim tipo As String = _TipoTitulo(Request("pExpediente"))
            '----------------------------------------------------------------
            'Nombre HU : Error al pasar el parámetro NroConsignación  
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 23-08-2017  
            'Objetivo: Para este caso puntual el + se usa como sustitución de espacios. 
            'En el caso del expediente 82989. Se usa HttpUtility.UrlEncode.
            '------------------------------------------------------------------/
            If tipo = "08" Then
                Response.Redirect("EditPAGOSLiquidacionSancion.aspx?ID=" & HttpUtility.UrlEncode(NroConsignacion) & "&pExpediente=" & Nroexpediente)
            Else
                Response.Redirect("EditPAGOS.aspx?ID=" & HttpUtility.UrlEncode(NroConsignacion) & "&pExpediente=" & Nroexpediente)
            End If

        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("PAGOSNOMBRE.SortDirection"))
            Case "ASC"
                Session("PAGOSNOMBRE.SortDirection") = "DESC"
            Case "DESC"
                Session("PAGOSNOMBRE.SortDirection") = "ASC"
            Case Else
                Session("PAGOSNOMBRE.SortDirection") = "ASC"
        End Select

        Session("PAGOSNOMBRE.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("PAGOSNOMBRE.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()
        End If

        ' Jeisson Gómez 03/09/2017 
        ' Inahabiliar el check de contabilización de pago, si el usuario que ingresa es difente a los perfiles 
        ' 3 REVISOR o 6 VERIFICADOR DE PAGOS.
        If e.Row.RowType = DataControlRowType.DataRow Then
            Try
                Dim Chk As CheckBox = DirectCast(e.Row.FindControl("chkSnContabilizar"), CheckBox)
                If (Session("mnivelacces") = "3" Or Session("mnivelacces") = "6") Then
                    Chk.Enabled = True
                Else
                    Chk.Enabled = False
                End If
            Catch ex As Exception

            End Try

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("PAGOSNOMBRE.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("PAGOSNOMBRE.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If
        If WholePageCount = 0 Then
            WholePageCount = 1
        End If
        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("PAGOSNOMBRE.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("PAGOSNOMBRE.CurrentPage") = Session("PAGOSNOMBRE.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("PAGOSNOMBRE.CurrentPage") > 1 Then
            Session("PAGOSNOMBRE.CurrentPage") = Session("PAGOSNOMBRE.CurrentPage") - 1
        Else
            Session("PAGOSNOMBRE.CurrentPage") = 1
        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("PAGOSNOMBRE.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Registros encontrados " & Session("PAGOSNOMBRE.RecordsFound")
        lblPageNumber.Text = "Página " & Session("PAGOSNOMBRE.CurrentPage") & " de " & GetPageCount()
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

    Private Sub LoadcboSearchUserSolicita()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM usuarios ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtUsuarios As New DataTable
        Adaptador.Fill(dtUsuarios)
        If dtUsuarios.Rows.Count > 0 Then
            '--------------------------------------------------------------------'
            '- Ingresar el valor en blanco (el valor queda al final)
            'Dim filaUsuarios As DataRow = dtUsuarios.NewRow()
            'filaUsuarios("codigo") = "00"
            'filaUsuarios("nombre") = "TODOS LOS USUARIOS"
            'dtUsuarios.Rows.Add(filaUsuarios)
            ''- Crear un dataview para ordenar los valore y "00" quede de primero
            'Dim vistaUsuarios As DataView = New DataView(dtUsuarios)
            'vistaUsuarios.Sort = "codigo"
            '--------------------------------------------------------------------'

            cboSearchUserSolicita.DataSource = dtUsuarios
            cboSearchUserSolicita.DataTextField = "nombre"
            cboSearchUserSolicita.DataValueField = "codigo"
            cboSearchUserSolicita.DataBind()
        End If
    End Sub

    Private Sub ContarMsjNoLeidos()
        Dim NumMensajes As Integer = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT COUNT(idunico) AS NumMensajes FROM mensajes WHERE " &
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

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()

        Response.Redirect("../../login.aspx")
    End Sub


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("MENSAJES.aspx")
    End Sub

    Private Sub lbtsubirpagos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtsubirpagos.Click
        Response.Redirect("subirVerificacionPago.aspx")
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        Session.RemoveAll()
    End Sub

    Protected Sub lnkSql_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkSql.Click
        Response.Redirect("subirSQL.aspx")
    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("EJEFISGLOBAL.aspx")
    End Sub

    Protected Sub cmdAdicionar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAdicionar.Click
        Dim tipo As String = _TipoTitulo(Request("pExpediente"))
        If tipo = "08" Then
            Response.Redirect("EditPAGOSLiquidacionSancion.aspx?pExpediente=" & Request("pExpediente"))
        Else
            Response.Redirect("EditPAGOS.aspx?pExpediente=" & Request("pExpediente"))
        End If
    End Sub

    Private Function _TipoTitulo(ByVal pNumExpediente As String) As String
        Dim tipotitulo As String = ""
        If pNumExpediente <> "" Then

            'Conexion a la base de datos
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            'Consultar el total de la deuda
            Dim sql As String = "SELECT MT.MT_nro_titulo, MT.MT_fec_expedicion_titulo, MT.MT_tipo_titulo, TT.nombre AS NomTipoTitulo " &
                                " FROM MAESTRO_TITULOS MT  " &
                                " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " &
                                "WHERE MT.MT_expediente = '" & pNumExpediente & "'"

            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                tipotitulo = Reader("MT_tipo_titulo").ToString()
            End If
            Reader.Close()
            Connection.Close()

            Return tipotitulo
        End If
    End Function

    Protected Sub cmdVerCambiosEstado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdVerCambiosEstado.Click
        Response.Redirect("ver_cambios_estado.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub chkSnContabilizar_CheckedChanged(sender As Object, e As EventArgs)
        '----------------------------------------------------------------
        'Nombre HU: HU_015 - Error ingresando el valor del pago.  
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 29-08-2017  
        'Objetivo: Cuando al ingresar el pago el perfil "VERIFICADOR DE PAGOS", 
        'en caso de ingresar un pago erróneamente, este check permite no 
        'tener en cuenta el pago. Lo anterior porque no se puede eliminar 
        'información del sistema. 
        '------------------------------------------------------------------/
        Dim chkEstado As CheckBox = DirectCast(sender, CheckBox)
        Dim row As GridViewRow = DirectCast(chkEstado.NamingContainer, GridViewRow)
        Dim NroConsignacion As String = grd.Rows(row.RowIndex).Cells(0).Text
        Dim NroExp As String = grd.Rows(row.RowIndex).Cells(2).Text
        Dim dcCapitalPagado As Decimal = CDec(grd.Rows(row.RowIndex).Cells(9).Text)

        Dim strQuery As String = "UPDATE PAGOS SET SnContabilizar = @SnContabilizar WHERE NroExp = @NroExp AND NroConsignacion = @NroConsignacion "
        Using Cn As New SqlConnection(Funciones.CadenaConexion)
            Dim Command As New SqlCommand(strQuery, Cn)

            Command.Parameters.Add("@SnContabilizar", SqlDbType.Bit).Value = chkEstado.Checked
            Command.Parameters.Add("@NroExp", SqlDbType.VarChar).Value = NroExp
            Command.Parameters.Add("@NroConsignacion", SqlDbType.VarChar).Value = NroConsignacion

            Cn.Open()
            Command.ExecuteNonQuery()
            Cn.Close()

        End Using

        ' Con base en la actualización realizada cuando se presenta un pago 
        ' UPDATE EJEFISGLOBAL Set efipagoscap = @efipagoscap, efisaldocap = (efivaldeu - @efipagoscap) WHERE efinroexp = '" & expediente & "'"
        ' Actualizamos la tabla EJEFISGLOBAL, según sea el caso 
        ' Si SnContabilizar = 1, entonces UPDATE ejefisglobal SET efipagoscap = (efipagoscap + dcCapitalPagado), efisaldocap = (efisaldocap + dcCapitalPagado) WHERE efinroexp = '" & expediente & "'"
        ' Si SnContabilizar = 0, entonces UPDATE ejefisglobal SET efipagoscap = (efipagoscap - dcCapitalPagado), efisaldocap = (efisaldocap - dcCapitalPagado) WHERE efinroexp = '" & expediente & "'"

        Dim strQueryUp As String = String.Empty

        If chkEstado.Checked Then
            strQueryUp = " UPDATE ejefisglobal SET efipagoscap = (efipagoscap + @dcCapitalPagado), efisaldocap = (efisaldocap + @dcCapitalPagado) WHERE efinroexp = @EfiNroExp "
        Else
            strQueryUp = " UPDATE ejefisglobal SET efipagoscap = (efipagoscap - @dcCapitalPagado), efisaldocap = (efisaldocap - @dcCapitalPagado) WHERE efinroexp = @EfiNroExp "
        End If

        Using Cnx As New SqlConnection(Funciones.CadenaConexion)
            Dim Cmd As New SqlCommand(strQueryUp, Cnx)

            Cmd.Parameters.Add("@dcCapitalPagado", SqlDbType.Money).Value = dcCapitalPagado
            Cmd.Parameters.Add("@EfiNroExp", SqlDbType.VarChar).Value = NroExp

            Cnx.Open()
            Cmd.ExecuteNonQuery()
            Cnx.Close()
        End Using

        BindGrid()
    End Sub
End Class