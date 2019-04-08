Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EditDETALLE_EMBARGO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboTipoBien()
            LoadcboBanco()
            LoadcboEfectivo()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM DETALLE_EMBARGO WHERE idunico = @idunico"
                Dim Efectivo As String
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @idunico parameter in the Command select query
                Command.Parameters.AddWithValue("@idunico", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    cboTipoBien.SelectedValue = Reader("TipoBien").ToString()
                    cboBanco.SelectedValue = Reader("Banco").ToString().Trim
                    txtIdentifBien.Text = Reader("IdentifBien").ToString()
                    txtvalorbien.Text = Reader("valorbien").ToString()
                    cboEfectivo.SelectedValue = Reader("efectivo").ToString().Trim
                    'txtcausal.Text = Reader("causal").ToString()
                    If Reader("efectivo").ToString().Trim = "SI" Then
                        Efectivo = "SI"                        
                    Else
                        Efectivo = "NO"                        
                    End If
                    LoadcboCausal(Efectivo)
                    'Es posible que por la migracion las causales no coordinen entre el SI y el NO. Por eso se llama al siguiente metodo
                    'cboCausal.SelectedValue = Reader("causal").ToString().Trim
                    AsignarCausal(Reader("causal").ToString().Trim, Efectivo)
                Else
                    cboEfectivo.SelectedValue = "NO"
                    LoadcboCausal("NO")
                End If
                Reader.Close()
                Connection.Close()
            Else
                'Since this is an insert then you can't delete it yet because it's not in the database.
                cmdTitulo.Visible = False
                '
                cboEfectivo.SelectedValue = "NO"
                LoadcboCausal("NO")
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdSave.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten adicionar datos"
                CustomValidator1.IsValid = False

                'Desactivar controles
                cboTipoBien.Enabled = False
                cboBanco.Enabled = False
                txtIdentifBien.Enabled = False
                txtvalorbien.Enabled = False
                cboEfectivo.Enabled = False
                'txtcausal.Enabled = False
            End If

            ValidarTipoBien()
        End If
    End Sub

    Private Sub AsignarCausal(ByVal pValor As String, ByVal pEfectivo As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT codigo FROM CAUSALES_EFECTIVIDAD_EMBARGO WHERE codigo = '" & pValor & "' AND efectivo = '" & pEfectivo & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'sw = True            
            cboCausal.SelectedValue = pValor
        Else
            'No hay consistencia entre efectivo y valor => Forzar valor por defecto
            If pEfectivo = "SI" Then
                cboCausal.SelectedValue = "01"
            Else
                cboCausal.SelectedValue = "05"
            End If
        End If
        Reader.Close()
        Connection.Close()
        ''
    End Sub

    

    Protected Sub LoadcboCausal(ByVal pEfectivo As String)

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT codigo, nombre FROM CAUSALES_EFECTIVIDAD_EMBARGO WHERE efectivo = '" & pEfectivo & "' ORDER BY codigo"
        Dim Command As New SqlCommand(sql, Connection)
        cboCausal.DataTextField = "nombre"
        cboCausal.DataValueField = "codigo"
        cboCausal.DataSource = Command.ExecuteReader()
        cboCausal.DataBind()
        'Close the Connection Object 
        Connection.Close()
    End Sub


    Protected Sub LoadcboTipoBien()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from TIPOS_BIENES order by codigo"
        Dim Command As New SqlCommand(sql, Connection)
        cboTipoBien.DataTextField = "nombre"
        cboTipoBien.DataValueField = "codigo"
        cboTipoBien.DataSource = Command.ExecuteReader()
        cboTipoBien.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboBanco()

        ''Create a new connection to the database
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        ''Opens a connection to the database.
        'Connection.Open()
        'Dim sql As String = "select BAN_CODIGO, BAN_NOMBRE from [MAESTRO_BANCOS] order by BAN_NOMBRE"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboBanco.DataTextField = "BAN_NOMBRE"
        'cboBanco.DataValueField = "BAN_CODIGO"
        'cboBanco.DataSource = Command.ExecuteReader()
        'cboBanco.DataBind()

        ''Close the Connection Object 
        'Connection.Close()

        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select BAN_CODIGO, BAN_NOMBRE from [MAESTRO_BANCOS] order by BAN_CODIGO"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("BAN_CODIGO") = ""
            fila("BAN_NOMBRE") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaBancos As DataView = New DataView(dt1)
            vistaBancos.Sort = "BAN_CODIGO"
            '--------------------------------------------------------------------
            cboBanco.DataSource = vistaBancos
            cboBanco.DataTextField = "BAN_NOMBRE"
            cboBanco.DataValueField = "BAN_CODIGO"
            cboBanco.DataBind()
        End If

    End Sub

    Private Sub LoadcboEfectivo()
        Dim dt As DataTable = New DataTable("Tabla")

        dt.Columns.Add("Codigo")
        dt.Columns.Add("Descripcion")

        Dim dr As DataRow

        dr = dt.NewRow()
        dr("Codigo") = "NO"
        dr("Descripcion") = "NO"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Codigo") = "SI"
        dr("Descripcion") = "SI"
        dt.Rows.Add(dr)

        cboEfectivo.DataSource = dt
        cboEfectivo.DataTextField = "Descripcion"
        cboEfectivo.DataValueField = "Codigo"
        cboEfectivo.DataBind()
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If txtvalorbien.Text = "" Then
            CustomValidator1.Text = "Digite el valor del bien por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        'Si el tipo de bien es "BIEN INMUEBLE" o "VEHICULO" => Blanquear campo banco, sino, Exigir banco
        Dim NomTipoBien As String = cboTipoBien.SelectedItem.Text.Trim
        'If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "BIEN INMUEBLE" Then
        If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "VEH&#205;CULO" Or _
                 (Left(NomTipoBien, 3) = "VEH" And Right(NomTipoBien, 4) = "CULO") Or _
                 NomTipoBien = "BIEN INMUEBLE" Or NomTipoBien = "MUEBLE" Or NomTipoBien = "OTRO" Then

            cboBanco.SelectedValue = "" ' Este fue un valor que se incluyo en LoadcboBanco

        Else
            If cboBanco.SelectedValue = "" Then
                CustomValidator1.Text = "Seleccione un banco por favor"
                CustomValidator1.IsValid = False
                Return
            End If
        End If

        Dim ID As String = Request("ID")
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL
        Dim InsertSQL As String = "Insert into DETALLE_EMBARGO ([NroResolEm], [TipoBien], [Banco], [IdentifBien], [valorbien], [efectivo], [causal] ) VALUES ( @NroResolEm, @TipoBien, @Banco, @IdentifBien, @valorbien, @efectivo, @causal ) "
        Dim UpdateSQL As String = "Update DETALLE_EMBARGO set [NroResolEm]=@NroResolEm, [TipoBien] = @TipoBien, [Banco] = @Banco, [IdentifBien] = @IdentifBien, [valorbien] = @valorbien, [efectivo] = @efectivo, [causal] = @causal where [idunico] = @idunico "
        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            'Insert            
            Command = New SqlCommand(InsertSQL, Connection)
        Else
            'Update             
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@idunico", ID)
        End If

        'Parametros
        Dim pResolEm As String
        pResolEm = Request("pResolEm")
        Command.Parameters.AddWithValue("@NroResolEm", pResolEm.Trim)

        If cboTipoBien.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@TipoBien", cboTipoBien.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@TipoBien", DBNull.Value)
        End If

        'Combo de los bancos
        If cboBanco.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Banco", cboBanco.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Banco", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@IdentifBien", txtIdentifBien.Text.Trim)

        If IsNumeric(txtvalorbien.Text) Then
            Command.Parameters.AddWithValue("@valorbien", txtvalorbien.Text)
        Else
            Command.Parameters.AddWithValue("@valorbien", DBNull.Value)
        End If

        'Combo 'efectivo'
        If cboEfectivo.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@efectivo", cboEfectivo.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@efectivo", DBNull.Value)
        End If

        'Combo 'causal'
        If cboCausal.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@causal", cboCausal.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@causal", DBNull.Value)
        End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Detalle de embargo", "Resolución de embargo No. " & pResolEm.Trim, Command)
            Connection.Close()
            Response.Redirect("DETALLE_EMBARGO.aspx?pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim)
        Catch ex As Exception
            Dim MsgError As String = ex.Message
            CustomValidator1.Text = MsgError
            CustomValidator1.IsValid = False
            Connection.Close()
        End Try

    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim pResolEm As String
        pResolEm = Request("pResolEm")
        Response.Redirect("DETALLE_EMBARGO.aspx?pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim)
    End Sub

    Protected Sub cmdTitulo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdTitulo.Click
        Dim pResolEm As String
        Dim pNroDeposito As String = ""

        pResolEm = Request("pResolEm")

        If ExisteTDJ(Request("ID").Trim, pNroDeposito) Then
            Response.Redirect("EditTDJ.aspx?IdEmbargo=" & Request("ID") & "&ID=" & pNroDeposito.Trim & "&pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim)
        Else
            Response.Redirect("EditTDJ.aspx?IdEmbargo=" & Request("ID") & "&pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim)
        End If
    End Sub

    Private Function ExisteTDJ(ByVal pIdEmbargo As String, ByRef pNroDeposito As String) As Boolean
        Dim respuesta As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT NroDeposito FROM tdj WHERE idEmbargo = " & pIdEmbargo & " and NroTituloPrincipal IS null"
        Dim Command As New SqlCommand(sql, Connection)

        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            respuesta = True
            pNroDeposito = Reader("NroDeposito").ToString().Trim.Trim
        Else
            pNroDeposito = ""
        End If

        Connection.Close()

        Return respuesta
    End Function


    Protected Sub cboEfectivo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboEfectivo.SelectedIndexChanged
        LoadcboCausal(cboEfectivo.SelectedValue.Trim)
    End Sub

    Protected Sub cboTipoBien_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTipoBien.SelectedIndexChanged
        ValidarTipoBien()
    End Sub

    Private Sub ValidarTipoBien()
        Dim NomTipoBien As String = cboTipoBien.SelectedItem.Text.Trim
        'If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "BIEN INMUEBLE" Then
        'If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "VEH&#205;CULO" Or _
        '         (Left(NomTipoBien, 3) = "VEH" And Right(NomTipoBien, 4) = "CULO") Or _
        '         NomTipoBien = "BIEN INMUEBLE" Then

        If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "VEH&#205;CULO" Or _
                 (Left(NomTipoBien, 3) = "VEH" And Right(NomTipoBien, 4) = "CULO") Or _
                 NomTipoBien = "BIEN INMUEBLE" Or NomTipoBien = "MUEBLE" Or NomTipoBien = "OTRO" Then

            cboBanco.Visible = False
            cmdTitulo.Text = "Gestionar Secuestro, Avalúo y Remate"
        Else
            'Cuentas bancarias y similares
            cboBanco.Visible = True
            cmdTitulo.Text = "Gestionar Título de Depósito Judicial (TDJ)"
        End If
    End Sub


End Class