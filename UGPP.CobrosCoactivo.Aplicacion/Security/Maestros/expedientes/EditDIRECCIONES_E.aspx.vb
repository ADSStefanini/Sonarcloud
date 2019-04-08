Imports System.Data
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class EditDIRECCIONES_E
    Inherits System.Web.UI.Page
    Dim fuenteDireccionBLL As FuenteDireccionBLL
    Protected Overrides Sub OnInit(e As EventArgs)
        fuenteDireccionBLL = New FuenteDireccionBLL()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboDepartamento()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("pIdUnico")) > 0 Then
                Dim sql As String = "select * from DIRECCIONES where [idunico] = @pIdunico"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @deudor parameter in the Command select query
                Command.Parameters.AddWithValue("@pIdunico", Request("pIdUnico"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    'txtdeudor.Text = Reader("deudor").ToString()
                    txtDireccion.Text = Reader("Direccion").ToString()
                    cboDepartamento.SelectedValue = Reader("Departamento").ToString()
                    'Carga y selección de la ciudad del deudor
                    LoadcboCiudad(cboDepartamento.SelectedValue)
                    cboCiudad.SelectedValue = Reader("Ciudad").ToString()

                    txtTelefono.Text = Reader("Telefono").ToString()
                    txtEmail.Text = Reader("Email").ToString()
                    txtMovil.Text = Reader("Movil").ToString()
                    txtpaginaweb.Text = Reader("paginaweb").ToString()

                    If String.IsNullOrEmpty(Reader("ID_FUENTE").ToString()) = False Then
                        CboFuente.SelectedValue = Reader("ID_FUENTE").ToString()
                    End If

                    If String.IsNullOrEmpty(Reader("OTRA_FUENTE").ToString()) = False Then
                        TxtOtraFuente.Text = Reader("OTRA_FUENTE").ToString()
                    End If
                End If
                'Close the Data Reader we are done with it.
                Reader.Close()

                'Close the Connection Object 
                Connection.Close()

            Else
                LoadcboCiudad(cboDepartamento.SelectedValue)
                'cmdDelete.Visible = False
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos                       
            If NomEstadoProceso = "DEVUELTO" Or NomEstadoProceso = "TERMINADO" Then
                cmdSave.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar datos"
                CustomValidator1.IsValid = False

                'Deshabilitar controles
                txtDireccion.Enabled = False
                cboDepartamento.Enabled = False
                cboCiudad.Enabled = False
                txtTelefono.Enabled = False
                txtEmail.Enabled = False
                txtMovil.Enabled = False
                txtpaginaweb.Enabled = False
                CboFuente.Enabled = False
                TxtOtraFuente.Enabled = False
            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim MTG As New MetodosGlobalesCobro
            Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    cmdSave.Visible = False
                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False
                End If

            End If

            If Len(Request("pIdUnico")) > 0 Then
                pnlDirecciones.Enabled = False
                cmdSave.Visible = False
                cmdCancel.Enabled = True
                cmdCancel.CssClass = "enabled"
            End If
        End If
    End Sub

    Protected Sub LoadcboDepartamento()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [DEPARTAMENTOS] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboDepartamento.DataTextField = "nombre"
        cboDepartamento.DataValueField = "codigo"
        cboDepartamento.DataSource = Command.ExecuteReader()
        cboDepartamento.DataBind()
        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboCiudad(ByVal prmStrCodigoDepartamento As String)

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [MUNICIPIOS] WHERE departamento = '" & prmStrCodigoDepartamento & "' order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboCiudad.DataTextField = "nombre"
        cboCiudad.DataValueField = "codigo"
        cboCiudad.DataSource = Command.ExecuteReader()
        cboCiudad.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim ID As String = Request("pIdUnico")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        ' 
        'Comandos SQL
        Dim InsertSQL As String = "Insert into DIRECCIONES ([deudor], [Direccion], [Departamento], [Ciudad], [Telefono], [Email], [Movil], [paginaweb] ) VALUES ( @deudor, @Direccion, @Departamento, @Ciudad, @Telefono, @Email, @Movil, @paginaweb ) "
        Dim UpdateSQL As String = "Update DIRECCIONES set [Direccion] = @Direccion, [Departamento] = @Departamento, [Ciudad] = @Ciudad, [Telefono] = @Telefono, [Email] = @Email, [Movil] = @Movil, [paginaweb] = @paginaweb where [idunico] = @idunico "
        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' insert
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@deudor", Request("ID"))
        Else
            ' update
            'Set the command object with the update sql and connection. 
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@idunico", ID)
        End If

        Command.Parameters.AddWithValue("@Direccion", txtDireccion.Text)

        If cboDepartamento.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Departamento", cboDepartamento.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Departamento", DBNull.Value)
        End If

        If cboCiudad.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Ciudad", cboCiudad.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Ciudad", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@Telefono", txtTelefono.Text)
        Command.Parameters.AddWithValue("@Email", txtEmail.Text)
        Command.Parameters.AddWithValue("@Movil", txtMovil.Text)
        Command.Parameters.AddWithValue("@paginaweb", txtpaginaweb.Text)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            If String.IsNullOrEmpty(ID) Then
                '"@deudor", Request("ID")
                LogProc.SaveLog(Session("ssloginusuario"), "Gestión de direcciones", "Deudor " & Request("ID"), Command)
            Else
                'UPDATE
                LogProc.SaveLog(Session("ssloginusuario"), "Gestión de direcciones", "IdUnico " & ID, Command)
            End If

        Catch ex As Exception

        End Try

        Connection.Close()
        'Go to the Summary page 
        Response.Redirect("DIRECCIONES_E.aspx?ID=" & Request("ID") & "&pIdUnico=" & Request("pIdUnico=") & "&pExpediente=" & Request("pExpediente"))

    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("DIRECCIONES_E.aspx?ID=" & Request("ID") & "&pIdUnico=" & Request("pIdUnico=") & "&pExpediente=" & Request("pExpediente"))
    End Sub

    Private Shared Function GetCiudades() As DataTable
        Dim cmd, cnx As String
        cnx = Funciones.CadenaConexion
        cmd = "SELECT codigo, nombre, departamento FROM municipios"

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtMunicipios As New DataTable
        Adaptador.Fill(dtMunicipios)

        Return dtMunicipios
    End Function

    Protected Sub cboDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDepartamento.SelectedIndexChanged
        LoadcboCiudad(cboDepartamento.SelectedValue)
    End Sub
End Class
