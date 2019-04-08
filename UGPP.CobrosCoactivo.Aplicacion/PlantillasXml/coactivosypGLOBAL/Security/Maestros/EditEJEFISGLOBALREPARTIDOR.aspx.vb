Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EditEJEFISGLOBALREPARTIDOR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            BindGrid()

            'Gestor, revisor y repartidor
            LoadcboEFIUSUASIG()
            LoadcboRepartidor()
            LoadcboEFIESTADO()
            LoadcboEFIESTADOPAGO()

            ' Combos de la pestaña de clasificacion
            LoadcboTitEjecAntig()
            LoadcboCaducidad()
            LoadcboEstadoPersona()
            LoadcboProcesoCurso()
            LoadcboAcuerdoPago()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM EJEFISGLOBAL where EFINROEXP = @EFINROEXP"
                ' 
                'Declare SQLCommand Object named Command                
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                Command.Parameters.AddWithValue("@EFINROEXP", Request("ID").Trim)
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtEFINROEXP.Text = Reader("EFINROEXP").ToString()
                    txtEFIFECHAEXP.Text = Left(Reader("EFIFECHAEXP").ToString().Trim, 10)
                    txtEFINUMMEMO.Text = Reader("EFINUMMEMO").ToString()
                    txtEFIEXPORIGEN.Text = Reader("EFIEXPORIGEN").ToString()
                    txtEFIFECCAD.Text = Left(Reader("EFIFECCAD").ToString().Trim, 10)
                    cboEFIUSUASIG.SelectedValue = Reader("EFIUSUASIG").ToString()
                    cboEFIESTADO.SelectedValue = Reader("EFIESTADO").ToString()
                    cboEFIESTADOPAGO.SelectedValue = Reader("EFIESTADOPAGO").ToString()
                    txtRevisor.Text = getNombreRevisor(Reader("EFIUSUREV").ToString.Trim)
                    txtEFIFECENTGES.Text = Left(Reader("EFIFECENTGES").ToString().Trim, 10)

                    MostrarRepartidor(Reader("EFINROEXP").ToString().Trim)
                End If
                ' 
                Reader.Close()
                Connection.Close()
                '
                cmdGestionar.Visible = True
            Else
                txtEFINROEXP.Text = GenConsecutivoExp()
                AShowExp.Visible = False

                'Ocultar controles
                cmdSave2.Visible = False
                cboRepartidor.Enabled = False
                txtRevisor.Enabled = False
                cboEFIUSUASIG.Enabled = False
                cboEFIESTADO.Enabled = False
                cboEFIESTADOPAGO.Enabled = False
                '
                cmdGestionar.Visible = False
            End If
            Dim MTG As New MetodosGlobalesCobro
            lblNomPerfil.Text = MTG.GetNomPerfil(Session("mnivelacces"))

            'Variable de sesion para el expediente actual
            Session("ssExpedienteActual") = txtEFINROEXP.Text.Trim

            'Mostrar clasificacion de cartera
            MostrarClasificacionCartera(txtEFINROEXP.Text.Trim)
        End If
    End Sub

    Private Sub RepartirExpedientes()

    End Sub

    Private Function getNombreRevisor(ByVal pIdRevisor As String) As String
        Dim NomSuperior As String = ""

        If Len(pIdRevisor) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT nombre FROM usuarios WHERE codigo = @codigo"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", pIdRevisor.Trim.Trim)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                NomSuperior = Reader("nombre").ToString().Trim
            End If
            Reader.Close()
            Connection.Close()
        End If

        Return NomSuperior
    End Function

    Private Function getIdRevisor(ByVal pIdGestor As String) As String
        Dim IdRevisor As String = ""

        If Len(pIdGestor) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT superior FROM usuarios WHERE codigo = @codigo"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", pIdGestor.Trim.Trim)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                IdRevisor = Reader("superior").ToString().Trim
            End If
            Reader.Close()
            Connection.Close()
        End If

        Return IdRevisor
    End Function

    Protected Sub LoadcboEFIUSUASIG()
        Dim cnx As String = Funciones.CadenaConexion
        'Dim cmd As String = "SELECT codigo, nombre FROM usuarios ORDER BY nombre"
        Dim cmd As String = "SELECT USUARIOS.codigo, USUARIOS.nombre " & _
                             "FROM USUARIOS " & _
                             "WHERE useractivo = 1 " & _
                               "ORDER BY nivelacces "

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtUsuarios As New DataTable
        Adaptador.Fill(dtUsuarios)
        If dtUsuarios.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtUsuarios.NewRow()
            filaEstado("codigo") = " "
            filaEstado("nombre") = "SELECCIONE UN GESTOR..."
            dtUsuarios.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaUsuarios As DataView = New DataView(dtUsuarios)
            vistaUsuarios.Sort = "codigo"
            '--------------------------------------------------------------------
            cboEFIUSUASIG.DataSource = vistaUsuarios
            cboEFIUSUASIG.DataTextField = "nombre"
            cboEFIUSUASIG.DataValueField = "codigo"
            cboEFIUSUASIG.DataBind()
        End If
    End Sub

    Protected Sub LoadcboRepartidor()
        Dim cnx As String = Funciones.CadenaConexion
        'Dim cmd As String = "SELECT codigo, nombre FROM usuarios ORDER BY nombre"
        Dim cmd As String = "SELECT DISTINCT USUARIOS.codigo, USUARIOS.login, USUARIOS.nombre, USUARIOS.nivelacces AS perfil, " & _
                                "perfiles.nombre AS NomPerfil " & _
                             "FROM USUARIOS " & _
                               "LEFT JOIN PERFILES ON USUARIOS.nivelacces = PERFILES.codigo " & _
                               "WHERE nivelacces = 2 AND useractivo = 1 " & _
                               "ORDER BY nivelacces "

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtUsuarios As New DataTable
        Adaptador.Fill(dtUsuarios)
        If dtUsuarios.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtUsuarios.NewRow()
            filaEstado("codigo") = " "
            filaEstado("nombre") = "SELECCIONE FUNCIONARIO QUE REALIZA REPARTO..."
            dtUsuarios.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaUsuarios As DataView = New DataView(dtUsuarios)
            vistaUsuarios.Sort = "codigo"
            '--------------------------------------------------------------------
            cboRepartidor.DataSource = vistaUsuarios
            cboRepartidor.DataTextField = "nombre"
            cboRepartidor.DataValueField = "codigo"
            cboRepartidor.DataBind()
        End If
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
            filaEstado("codigo") = " "
            filaEstado("nombre") = "SELECCIONE ESTADO..."
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

    Protected Sub LoadcboEFIESTADOPAGO()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM estados_pago ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEstados_Pago As New DataTable
        Adaptador.Fill(dtEstados_Pago)
        If dtEstados_Pago.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtEstados_Pago.NewRow()
            filaEstado("codigo") = " "
            filaEstado("nombre") = "SELECCIONE ESTADO DEL PAGO..."
            dtEstados_Pago.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaEstados_Pago As DataView = New DataView(dtEstados_Pago)
            vistaEstados_Pago.Sort = "codigo"
            '--------------------------------------------------------------------
            cboEFIESTADOPAGO.DataSource = vistaEstados_Pago
            cboEFIESTADOPAGO.DataTextField = "nombre"
            cboEFIESTADOPAGO.DataValueField = "codigo"
            cboEFIESTADOPAGO.DataBind()
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim Operacion As String = ""
        Dim ID As String = Request("ID")
        Dim idRevisor As String = getIdRevisor(cboEFIUSUASIG.SelectedValue.Trim)

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        '
        Dim trans As SqlTransaction
        trans = Connection.BeginTransaction


        Dim Command As SqlCommand
        'Comandos SQL
        Dim InsertSQL As String = "Insert into [EJEFISGLOBAL] ([EFINROEXP], [EFIFECHAEXP], [EFINUMMEMO], [EFIEXPORIGEN], [EFIMODCOD], [EFIESTADO] ) VALUES ( @EFINROEXP, @EFIFECHAEXP, @EFINUMMEMO, @EFIEXPORIGEN, @EFIMODCOD, @EFIESTADO) "
        Dim UpdateSQL As String = "Update [EJEFISGLOBAL] set [EFIFECHAEXP] = @EFIFECHAEXP, [EFINUMMEMO] = @EFINUMMEMO, [EFIEXPORIGEN] = @EFIEXPORIGEN, [EFIESTADO] = @EFIESTADO  WHERE [EFINROEXP] = @EFINROEXP "
        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' insert             
            Command = New SqlCommand(InsertSQL, Connection, trans)
            ' 
            'Generar consecutivo 
            'ID = GenConsecutivoExp().ToString.Trim
            ID = txtEFINROEXP.Text.Trim
            Command.Parameters.AddWithValue("@EFINROEXP", ID)
            Operacion = "INSERT"
        Else
            'Set the command object with the update sql and connection. 
            Command = New SqlCommand(UpdateSQL, Connection, trans)
            ' 
            'Set the @EFINROEXP field for updates. 
            Command.Parameters.AddWithValue("@EFINROEXP", ID)
            Operacion = "UPDATE"
        End If

        ' Validar Fecha recepción título ejecutivo 
        If IsDate(Left(txtEFIFECHAEXP.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@EFIFECHAEXP", Left(txtEFIFECHAEXP.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@EFIFECHAEXP", DBNull.Value)
            CustomValidator1.ErrorMessage = "Fecha recepción título ejecutivo "
            CustomValidator1.IsValid = False
            Connection.Close()
            Return
        End If

        ' Validar número del memorando
        If txtEFINUMMEMO.Text.Trim = "" Then
            CustomValidator1.ErrorMessage = "Digite el número del memorando por favor"
            CustomValidator1.IsValid = False
            Connection.Close()
            Return
        Else
            Command.Parameters.AddWithValue("@EFINUMMEMO", txtEFINUMMEMO.Text.Trim)
        End If

        ' Validar No. Expediente origen 
        If txtEFIEXPORIGEN.Text.Trim = "" Then
            CustomValidator1.ErrorMessage = "Digite el No. Expediente origen  por favor"
            CustomValidator1.IsValid = False
            Connection.Close()
            Return
        Else
            Command.Parameters.AddWithValue("@EFIEXPORIGEN", txtEFIEXPORIGEN.Text.Trim)
        End If
        ' Impuesto No. 4 = "parafiscales / pensiones"
        Command.Parameters.AddWithValue("@EFIMODCOD", 4)

        'Colocar inicialmente el estado '09' = REPARTO
        Command.Parameters.AddWithValue("@EFIESTADO", "09")        

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog2(Session("ssloginusuario"), "Módulo de creación de expedientes", "Expediente " & ID, Command, Connection, trans)

            'Actualizar consecutivo de expedientes
            If Operacion = "INSERT" Then
                If ActConsecutivoExp(Connection, trans) Then
                    trans.Commit()
                    CustomValidator1.ErrorMessage = "Expediente guardado con éxito"
                    CustomValidator1.IsValid = False
                    '
                    cmdGestionar.Visible = True
                Else
                    trans.Rollback()
                End If
            Else
                trans.Commit()
            End If

        Catch ex As Exception
            trans.Rollback()
            CustomValidator1.ErrorMessage = ex.Message
            CustomValidator1.IsValid = False

        End Try

        'Close the Connection Object 
        Connection.Close()

        'Go to the Summary page 
        'Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
        
    End Sub

    'Private Sub RegistrarMensaje(ByVal pNroExp As String, ByVal pUsuOrigen As String, ByVal pUsuDestino As String, ByVal pMensaje As String, ByVal pFecEnvio As DateTime)
    '    Dim Connection As New SqlConnection(Funciones.CadenaConexion)
    '    Connection.Open()
    '    Dim Command As SqlCommand
    '    'Comandos SQL
    '    Dim InsertSQL As String = "Insert into MENSAJES ([NroExp], [UsuOrigen], [UsuDestino], [Mensaje], [FecEnvio]) VALUES (@NroExp, @UsuOrigen, @UsuDestino, @Mensaje, @FecEnvio  ) "

    '    ' insert             
    '    Command = New SqlCommand(InsertSQL, Connection)
    '    ' 
    '    Command.Parameters.AddWithValue("@NroExp", pNroExp)
    '    Command.Parameters.AddWithValue("@UsuOrigen", pUsuOrigen)
    '    Command.Parameters.AddWithValue("@UsuDestino", pUsuDestino)
    '    Command.Parameters.AddWithValue("@Mensaje", pMensaje)
    '    Command.Parameters.AddWithValue("@FecEnvio", pFecEnvio)

    '    Try
    '        Command.ExecuteNonQuery()

    '        'Después de cada GRABAR hay que llamar al log de auditoria
    '        Dim LogProc As New LogProcesos
    '        LogProc.SaveLog(Session("ssloginusuario"), "Registro de mensajes", "Expediente " & pNroExp, Command)
    '    Catch ex As Exception

    '    End Try

    '    Connection.Close()
    'End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
    End Sub

    'Generar consecutivos de expedientes
    Private Function GenConsecutivoExp() As Integer
        Dim TmpConsec As Integer = 0
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT con_user FROM maestro_consecutivos WHERE con_identificador = 10"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtConsecutivos As New DataTable
        Adaptador.Fill(dtConsecutivos)
        If dtConsecutivos.Rows.Count > 0 Then
            TmpConsec = dtConsecutivos.Rows(0).Item(0)
        End If
        Return TmpConsec + 1

    End Function

    'Actualizar consecutivo
    Private Function ActConsecutivoExp(ByVal cnx As SqlConnection, ByVal transac As SqlTransaction) As Boolean
        Dim sw As Boolean = True
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        Dim Command As SqlCommand = New SqlCommand("UPDATE maestro_consecutivos SET con_user = con_user + 1 WHERE con_identificador = 10", cnx, transac)

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog2(Session("ssloginusuario"), "Módulo de actualización de consecutivos", "con_identificador = 10 ", Command, cnx, transac)
        Catch ex As Exception
            sw = False
        End Try

        'Connection.Close()
        Return sw

    End Function

    Private Sub RegistrarCambioEstado(ByVal pNumExpediente As String, ByVal pRepartidor As String, ByVal pRevisor As String, ByVal pAbogado As String, ByVal pFecha As Date, ByVal pEstado As String, ByVal pEstadoPago As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        'Declare string InsertSQL 
        Dim InsertSQL As String = "INSERT INTO CAMBIOS_ESTADO (NroExp, repartidor, revisor, abogado, fecha, estado, estadopago) VALUES (@NroExp, @repartidor, @revisor, @abogado, @fecha, @estado, @estadopago)"
        Dim Command As SqlCommand = New SqlCommand(InsertSQL, Connection)

        'Parametros
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Command.Parameters.AddWithValue("@repartidor", pRepartidor)
        Command.Parameters.AddWithValue("@revisor", pRevisor)
        Command.Parameters.AddWithValue("@abogado", pAbogado)
        Command.Parameters.AddWithValue("@fecha", pFecha)
        Command.Parameters.AddWithValue("@estado", pEstado)
        Command.Parameters.AddWithValue("@estadopago", pEstadoPago)

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Registro de cambios de estado", "Expediente " & pNumExpediente, Command)
        Catch ex As Exception

        End Try


        Connection.Close()
    End Sub

    Private Function HayCambioEstado(ByVal pNumExpediente As String, ByVal pRepartidor As String, ByVal pRevisor As String, ByVal pAbogado As String, ByVal pEstado As String, ByVal pEstadoPago As String, ByRef HayCambioGestor As Boolean) As Boolean
        Dim Respuesta As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp AND idunico = (SELECT MAX(idunico) AS UltimoCambio FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp)"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        ' 
        'If at least one record was found
        If Reader.Read Then
            Dim RepartidorActual As String = Reader("repartidor").ToString()
            Dim RevisorActual As String = Reader("revisor").ToString()
            Dim AbogadoActual As String = Reader("abogado").ToString()
            Dim EstadoActual As String = Reader("estado").ToString()
            Dim EstadoPagoActual As String = Reader("estadopago").ToString()

            'Preguntar si algun campo es diferente
            If RepartidorActual <> pRepartidor Or AbogadoActual <> pAbogado Or EstadoActual <> pEstado Or EstadoPagoActual <> pEstadoPago Or RevisorActual <> pRevisor Then
                Respuesta = True
            End If

            'Si el abogado es diferente, tambien guardo la respuesta en otra variable
            If AbogadoActual <> pAbogado Then
                HayCambioGestor = True
            End If

        Else
            ' Si no encontro el registro => cambio de estado
            Respuesta = True
            HayCambioGestor = True
        End If
        ' 
        'Close the Data Reader we are done with it.
        Reader.Close()

        'Close the Connection Object 
        Connection.Close()

        'Devolver resultado de la funcion
        Return Respuesta
    End Function

    'Private Function HayCambioGestor(ByVal pNumExpediente As String, ByVal pAbogado As String) As Boolean
    '    Dim Respuesta As Boolean = False

    '    Dim Connection As New SqlConnection(Funciones.CadenaConexion)
    '    Connection.Open()
    '    Dim sql As String = "SELECT * FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp AND idunico = (SELECT MAX(idunico) AS UltimoCambio FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp)"
    '    Dim Command As New SqlCommand(sql, Connection)
    '    Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
    '    Dim Reader As SqlDataReader = Command.ExecuteReader
    '    ' 
    '    'If at least one record was found
    '    If Reader.Read Then            
    '        Dim AbogadoActual As String = Reader("abogado").ToString()            
    '        'Preguntar si abogado es diferente
    '        If AbogadoActual <> pAbogado Then
    '            Respuesta = True
    '        End If
    '    Else
    '        ' Si no encontro el registro => cambio de estado
    '        Respuesta = True
    '    End If

    '    Reader.Close()
    '    Connection.Close()

    '    'Devolver resultado de la funcion
    '    Return Respuesta
    'End Function

    Private Sub MostrarClasificacionCartera(ByVal pNumExpediente As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM [CLASIF_CARTERA_REGISTRO] WHERE NroExp = @NroExp"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then            
            'cboRepartidor.SelectedValue = RepartidorActual
            cboTitEjecAntig.SelectedValue = Reader("TitEjecAntig").ToString()
            cboCaducidad.SelectedValue = Reader("Caducidad").ToString()
            cboEstadoPersona.SelectedValue = Reader("EstadoPersona").ToString()            
            cboProcesoCurso.SelectedValue = Reader("ProcesoCurso").ToString()
            cboAcuerdoPago.SelectedValue = Reader("AcuerdoPago").ToString()
            txtExtadoInicial.Text = Reader("EstadoInicial").ToString()
        End If
        'Close the Data Reader we are done with it.
        Reader.Close()
        Connection.Close()
    End Sub

    Private Sub MostrarRepartidor(ByVal pNumExpediente As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT repartidor FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp AND idunico = (SELECT MAX(idunico) AS UltimoCambio FROM CAMBIOS_ESTADO WHERE NroExp = @NroExp)"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            Dim RepartidorActual As String = Reader("repartidor").ToString()
            cboRepartidor.SelectedValue = RepartidorActual
        End If
        'Close the Data Reader we are done with it.
        Reader.Close()
        Connection.Close()
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()
        Response.Redirect("../../login.aspx")
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()     
        Session.RemoveAll()
    End Sub

    Protected Sub cboEFIUSUASIG_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboEFIUSUASIG.SelectedIndexChanged
        'txtRevisor.Text = cboEFIUSUASIG.SelectedValue.Trim
        Dim IdRevisor As String = getIdRevisor(cboEFIUSUASIG.SelectedValue.Trim)
        txtRevisor.Text = getNombreRevisor(IdRevisor)
    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
    End Sub

    'Combos relacionados con la clasificacion
    Private Sub LoadcboTitEjecAntig()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'TIPO PERSONA' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTitAnt As New DataTable
        Adaptador.Fill(dtTitAnt)
        If dtTitAnt.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtTitAnt.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtTitAnt.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTitAnt As DataView = New DataView(dtTitAnt)
            vistaTitAnt.Sort = "codigo"
            'Enlazar combo 
            cboTitEjecAntig.DataSource = vistaTitAnt
            cboTitEjecAntig.DataTextField = "nombre"
            cboTitEjecAntig.DataValueField = "codigo"
            cboTitEjecAntig.DataBind()
        End If

    End Sub
    '
    Private Sub LoadcboCaducidad()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'CADUCIDAD' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtCaducidad As New DataTable
        Adaptador.Fill(dtCaducidad)
        If dtCaducidad.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtCaducidad.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtCaducidad.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaCaducidad As DataView = New DataView(dtCaducidad)
            vistaCaducidad.Sort = "codigo"
            'Enlazar combo 
            cboCaducidad.DataSource = vistaCaducidad
            cboCaducidad.DataTextField = "nombre"
            cboCaducidad.DataValueField = "codigo"
            cboCaducidad.DataBind()
        End If
    End Sub

    Private Sub LoadcboEstadoPersona()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'ESTADO PERSONA NATURAL O JURÍDICA' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEstado As New DataTable
        Adaptador.Fill(dtEstado)
        If dtEstado.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtEstado.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtEstado.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaEstado As DataView = New DataView(dtEstado)
            vistaEstado.Sort = "codigo"
            'Enlazar combo 
            cboEstadoPersona.DataSource = vistaEstado
            cboEstadoPersona.DataTextField = "nombre"
            cboEstadoPersona.DataValueField = "codigo"
            cboEstadoPersona.DataBind()
        End If
    End Sub


    Private Sub LoadcboProcesoCurso()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'OTROS PROCESOS' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtProcesoCurso As New DataTable
        Adaptador.Fill(dtProcesoCurso)
        If dtProcesoCurso.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtProcesoCurso.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtProcesoCurso.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaProcesoCurso As DataView = New DataView(dtProcesoCurso)
            vistaProcesoCurso.Sort = "codigo"
            'Enlazar combo 
            cboProcesoCurso.DataSource = vistaProcesoCurso
            cboProcesoCurso.DataTextField = "nombre"
            cboProcesoCurso.DataValueField = "codigo"
            cboProcesoCurso.DataBind()
        End If
    End Sub

    Private Sub LoadcboAcuerdoPago()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'ACUERDOS DE PAGO' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtAcuerdos As New DataTable
        Adaptador.Fill(dtAcuerdos)
        If dtAcuerdos.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtAcuerdos.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtAcuerdos.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaAcuerdos As DataView = New DataView(dtAcuerdos)
            vistaAcuerdos.Sort = "codigo"
            'Enlazar combo 
            cboAcuerdoPago.DataSource = vistaAcuerdos
            cboAcuerdoPago.DataTextField = "nombre"
            cboAcuerdoPago.DataValueField = "codigo"
            cboAcuerdoPago.DataBind()
        End If
    End Sub

    Private Function GetEstadoInicialProceso() As Boolean
        Dim Respuesta As Boolean = False
        Dim I, J, K, M, N, O As Integer
        Dim B, C, D, F, G, BuscarV As String

        BuscarV = ""
        '
        'I = cboTitEjecAntig.SelectedValue
        'I = Mid(cboTitEjecAntig.SelectedValue, InStr(cboTitEjecAntig.SelectedValue, "V") + 1)
        'CInt(Int(val(I)))
        I = CInt(Int(Val(Mid(cboTitEjecAntig.SelectedValue, InStr(cboTitEjecAntig.SelectedValue, "V") + 1))))

        B = cboTitEjecAntig.SelectedItem.Text.Trim 'cboTitEjecAntig.SelectedItem.ToString
        If I = 0 Then
            txtExtadoInicial.Text = "Indique si es un título ejecutivo a cargo de la nación por favor"
            Return Respuesta
        End If

        'J = cboCaducidad.SelectedValue
        J = CInt(Int(Val(Mid(cboCaducidad.SelectedValue, InStr(cboCaducidad.SelectedValue, "V") + 1))))
        C = cboCaducidad.SelectedItem.Text.Trim
        If J = 0 Then
            txtExtadoInicial.Text = "Seleccione un tipo de caducidad por favor"
            Return Respuesta
        End If

        'K = cboEstadoPersona.SelectedValue
        K = CInt(Int(Val(Mid(cboEstadoPersona.SelectedValue, InStr(cboEstadoPersona.SelectedValue, "V") + 1))))
        D = cboEstadoPersona.SelectedItem.Text.Trim
        If K = 0 Then
            txtExtadoInicial.Text = "Indique el tipo de persona por favor"
            Return Respuesta
        End If

        'L = CInt(Int(Val(Mid(cboCausalesInc.SelectedValue, InStr(cboCausalesInc.SelectedValue, "V") + 1))))
        'E = cboCausalesInc.SelectedItem.Text.Trim

        'M = cboProcesoCurso.SelectedValue
        M = CInt(Int(Val(Mid(cboProcesoCurso.SelectedValue, InStr(cboProcesoCurso.SelectedValue, "V") + 1))))
        F = cboProcesoCurso.SelectedItem.Text.Trim
        If M = 0 Then
            txtExtadoInicial.Text = "Indique si hay un proceso de cobro coactivo en curso por favor"
            Return Respuesta
        End If

        'N = cboAcuerdoPago.SelectedValue
        N = CInt(Int(Val(Mid(cboAcuerdoPago.SelectedValue, InStr(cboAcuerdoPago.SelectedValue, "V") + 1))))
        G = cboAcuerdoPago.SelectedItem.Text.Trim
        If N = 0 Then
            txtExtadoInicial.Text = "Indique si el deudor tiene acuerdo de pago por favor"
            Return Respuesta
        End If

        'txtExtadoInicial.Text = ""

        'Asignar a O el valor menor
        Dim numeros() As Integer = {I, J, K, M, N}
        Array.Sort(numeros)

        O = numeros(0)

        If O = I Then
            BuscarV = B

        ElseIf O = J Then
            BuscarV = C

        ElseIf O = K Then
            BuscarV = D

        ElseIf O = M Then
            BuscarV = F

        Else
            BuscarV = G
        End If

        'txtExtadoInicial.Text = BuscarV

        ' Buscar BuscarV en la tabla 
        Dim Resultado As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT TOP 1 resultado FROM CLASIFICACION_CARTERA " & _
                            "WHERE concepto = '" & BuscarV & "' ORDER BY idunico"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Resultado = Reader("resultado").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()

        'Resultado calsificacion XXX
        txtExtadoInicial.Text = Resultado
        Dim IdResultado As String = ""
        'Session("ResultadoClasificacion") = Resultado
        'Pasar nombre a codigo
        If Resultado = "COACTIVO" Then
            IdResultado = "02"

        ElseIf Resultado = "CONCURSAL" Then
            IdResultado = "03"

        ElseIf Resultado = "INCOBRABLE" Then
            IdResultado = "01"

        Else
            'Persuasivo
            IdResultado = "06"

        End If
        '??
        cboEFIESTADO.SelectedValue = IdResultado

        Respuesta = True

        Return Respuesta
    End Function



    Protected Sub btnObtenerEstado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnObtenerEstado.Click
        If GetEstadoInicialProceso() Then
            'Grabar
            GrabarClasificacioCartera(txtEFINROEXP.Text.Trim)

            'Si el expediente aun no existe => actualizar el control cboEFIESTADO
            If Not ExisteExpediente(txtEFINROEXP.Text.Trim) Then
                ActualizarEstadoExpediente(txtEFINROEXP.Text.Trim)
            End If
        End If
    End Sub

    Private Sub ActualizarEstadoExpediente(ByVal pExpediente As String)
        '-----------------------
        Dim Resp As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()        
        Dim sql As String = "SELECT codigo, nombre FROM ESTADOS_PROCESO WHERE nombre = '" & txtExtadoInicial.Text.Trim & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            cboEFIESTADO.SelectedValue = Reader("codigo").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        '-----------------------
    End Sub

    Public Function ExisteExpediente(ByVal pExpediente As String) As Boolean
        Dim Resp As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        'Dim sql As String = "SELECT EFIESTADO FROM EJEFISGLOBAL WHERE EFINROEXP = '" & pExpediente & "'"
        Dim sql As String = "SELECT EFINROEXP FROM EJEFISGLOBAL WHERE EFINROEXP = '" & pExpediente.Trim & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Resp = True
        End If
        Reader.Close()
        Connection.Close()

        Return Resp
    End Function

    Private Sub GrabarClasificacioCartera(ByVal pExpediente As String)
        Dim sql As String = ""
        'Si el registro del expediente no existe en la tabla CLASIF_CARTERA_REGISTRO => insert, sino => update    
        If ExisteClasificacionCartera(pExpediente) Then
            'update
            sql = "UPDATE CLASIF_CARTERA_REGISTRO SET " & _
                    "TitEjecAntig = @TitEjecAntig, Caducidad = @Caducidad, EstadoPersona = @EstadoPersona, " & _
                    "ProcesoCurso = @ProcesoCurso, AcuerdoPago = @AcuerdoPago, EstadoInicial = @EstadoInicial " & _
                    "WHERE NroExp = @NroExp"
        Else
            'insert
            sql = "INSERT INTO CLASIF_CARTERA_REGISTRO (NroExp, TitEjecAntig, Caducidad, EstadoPersona, ProcesoCurso, AcuerdoPago, EstadoInicial) " & _
                                            "VALUES (@NroExp, @TitEjecAntig, @Caducidad, @EstadoPersona, @ProcesoCurso, @AcuerdoPago, @EstadoInicial)"
        End If

        '------------------------------------------------------------------------
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Asignar el tipo de comando (insert o update)
        Command = New SqlCommand(sql, Connection)

        'Parametros
        Command.Parameters.AddWithValue("@NroExp", pExpediente)
        Command.Parameters.AddWithValue("@TitEjecAntig", cboTitEjecAntig.SelectedValue.Trim)
        Command.Parameters.AddWithValue("@Caducidad", cboCaducidad.SelectedValue.Trim)
        Command.Parameters.AddWithValue("@EstadoPersona", cboEstadoPersona.SelectedValue.Trim)
        Command.Parameters.AddWithValue("@ProcesoCurso", cboProcesoCurso.SelectedValue.Trim)
        Command.Parameters.AddWithValue("@AcuerdoPago", cboAcuerdoPago.SelectedValue.Trim)
        Command.Parameters.AddWithValue("@EstadoInicial", txtExtadoInicial.Text.Trim)

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Clasificación de cartera", "Expediente " & pExpediente, Command)
        Catch ex As Exception

        End Try
        '------------------------------------------------------------------------
    End Sub

    Private Function ExisteClasificacionCartera(ByVal pExpediente As String) As Boolean
        Dim Respuesta As Boolean = False
        '------------------------------------------------------------------------------------
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT * FROM CLASIF_CARTERA_REGISTRO WHERE NroExp = @NroExp"

        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", pExpediente)
        '                 
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        If Reader.Read Then
            Respuesta = True
        End If
        Reader.Close()
        Connection.Close()
        '------------------------------------------------------------------------------------
        Return Respuesta

    End Function


    Protected Sub AShowExp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AShowExp.Click
        'EditEJEFISGLOBAL.aspx?ID=80003
        Response.Redirect("EditEJEFISGLOBAL.aspx?ID=" & Request("ID"))
    End Sub

    Protected Sub cmdSave2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave2.Click
        Dim Operacion As String = ""
        Dim ID As String = Request("ID")
        Dim idRevisor As String = getIdRevisor(cboEFIUSUASIG.SelectedValue.Trim)

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL        
        Dim UpdateSQL As String = "Update [EJEFISGLOBAL] set [EFIFECCAD] = @EFIFECCAD, [EFIUSUASIG] = @EFIUSUASIG, [EFIESTADO] = @EFIESTADO, [EFIESTADOPAGO] = @EFIESTADOPAGO, [EFIUSUREV] = @EFIUSUREV, EFIFECENTGES = @EFIFECENTGES  WHERE [EFINROEXP] = @EFINROEXP "
        '         
        'Set the command object with the update sql and connection. 
        Command = New SqlCommand(UpdateSQL, Connection)        
        Command.Parameters.AddWithValue("@EFINROEXP", ID)
        Operacion = "UPDATE"

        ' Validar la Fecha entrega al CAD para registro 	
        If IsDate(Left(txtEFIFECCAD.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@EFIFECCAD", Left(txtEFIFECCAD.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@EFIFECCAD", DBNull.Value)
            CustomValidator2.ErrorMessage = "Indique la Fecha entrega al CAD para registro por favor"
            CustomValidator2.IsValid = False
            Return
        End If

        'Validar que el repartidor actual NO sea nulo
        If cboRepartidor.SelectedValue.Length > 0 Then
            If cboRepartidor.SelectedValue.Trim = "" Then
                CustomValidator2.ErrorMessage = "Seleccione el funcionario repartidor actual por favor"
                CustomValidator2.IsValid = False
                Return
            End If
        End If

        'Validar que el gestor actual del pago NO sea nulo
        If cboEFIUSUASIG.SelectedValue.Length > 0 Then
            If cboEFIUSUASIG.SelectedValue.Trim = "" Then
                CustomValidator2.ErrorMessage = "Seleccione el gestor actual por favor"
                CustomValidator2.IsValid = False
                Return
            Else
                Command.Parameters.AddWithValue("@EFIUSUASIG", cboEFIUSUASIG.SelectedValue)
            End If

            'Si hay gestor => debe haber revisor            
            If idRevisor = "" Then
                Command.Parameters.AddWithValue("@EFIUSUREV", DBNull.Value)
                idRevisor = Nothing
            Else
                Command.Parameters.AddWithValue("@EFIUSUREV", idRevisor)
            End If
        Else
            Command.Parameters.AddWithValue("@EFIUSUASIG", DBNull.Value)
            Command.Parameters.AddWithValue("@EFIUSUREV", DBNull.Value)
        End If

        'Validar que el estado actual NO sea nulo 
        If cboEFIESTADO.SelectedValue.Length > 0 Then
            If cboEFIESTADO.SelectedValue.Trim = "" Then
                CustomValidator2.ErrorMessage = "Seleccione el estado actual del proceso por favor"
                CustomValidator2.IsValid = False
                Return
            Else
                Command.Parameters.AddWithValue("@EFIESTADO", cboEFIESTADO.SelectedValue)
            End If
        Else
            Command.Parameters.AddWithValue("@EFIESTADO", DBNull.Value)
        End If

        'Validar que el estado del pago NO sea nulo
        If cboEFIESTADOPAGO.SelectedValue.Length > 0 Then
            If cboEFIESTADOPAGO.SelectedValue.Trim = "" Then
                CustomValidator2.ErrorMessage = "Seleccione el estado del pago por favor"
                CustomValidator2.IsValid = False
                Return
            Else
                Command.Parameters.AddWithValue("@EFIESTADOPAGO", cboEFIESTADOPAGO.SelectedValue)
            End If
        Else
            Command.Parameters.AddWithValue("@EFIESTADOPAGO", DBNull.Value)
        End If

        'Fecha de entrega al gestor
        If IsDate(Left(txtEFIFECENTGES.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@EFIFECENTGES", Left(txtEFIFECENTGES.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@EFIFECENTGES", DBNull.Value)
            CustomValidator2.ErrorMessage = "Indique la fecha de entrega al gestor por favor"
            CustomValidator2.IsValid = False
            Return
        End If

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de creación de expedientes", "Expediente " & ID, Command)
        Catch ex As Exception

        End Try


        'Close the Connection Object 
        Connection.Close()

        'Operacion = "UPDATE"
        Dim HayCambioGestor As Boolean = False
        If HayCambioEstado(ID, cboRepartidor.SelectedValue, idRevisor, cboEFIUSUASIG.SelectedValue, cboEFIESTADO.SelectedValue, cboEFIESTADOPAGO.SelectedValue, HayCambioGestor) Then
            '02/09/2014. Se cambia la siguiente linea para que NO tome la fecha del sistema, sino la que indica txtEFIFECENTGES.text
            'RegistrarCambioEstado(ID, cboRepartidor.SelectedValue, idRevisor, cboEFIUSUASIG.SelectedValue, DateTime.Now(), cboEFIESTADO.SelectedValue, cboEFIESTADOPAGO.SelectedValue)            
            Dim FechaEntregaGestor As Date
            FechaEntregaGestor = Date.ParseExact(txtEFIFECENTGES.Text.Trim, "dd/MM/yyyy", Nothing)
            RegistrarCambioEstado(ID, cboRepartidor.SelectedValue, idRevisor, cboEFIUSUASIG.SelectedValue, FechaEntregaGestor, cboEFIESTADO.SelectedValue, cboEFIESTADOPAGO.SelectedValue)

            'Registrar el cambio en la tabla de solicitud de cambio de estado
            Dim sql As String
            sql = "UPDATE SOLICITUDES_CAMBIOESTADO SET estadosol = 2 WHERE NroExp = '" & ID & "'"
            Dim Command2 As SqlCommand
            Command2 = New SqlCommand(sql, Connection)
            Connection.Open()
            Try
                Command2.ExecuteNonQuery()
                'Después de cada GRABAR hay que llamar al log de auditoria
                Dim LogProc As New LogProcesos
                LogProc.SaveLog(Session("ssloginusuario"), "Actualización de solicitudes de cambio de estado", "Expediente " & ID, Command2)
            Catch ex As Exception
                CustomValidator2.Text = ex.Message
                CustomValidator2.IsValid = False
            End Try
            Connection.Close()

            '1. Enviar mensaje al abogado indicandole que tiene un nuevo proceso asignado.            
            Dim MTG As New MetodosGlobalesCobro
            Dim msg As String = "Ud tiene un nuevo expediente asignado. Es el " & ID
            'Esto solo se debe enviar cuando haya un cambio de abogado
            If HayCambioGestor Then
                MTG.RegistrarMensaje(ID, Session("sscodigousuario"), cboEFIUSUASIG.SelectedValue.Trim, msg, Date.Now)
            End If

            'Registrar fecha estimada de terminación persuasivo 
            RegistrarFecEstiFin(ID)

        End If

        'Go to the Summary page 
        Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
    End Sub

    Private Sub RegistrarFecEstiFin(ByVal pExpediente As String)
        Dim swExisteRegistro As Boolean = False
        Dim FecEstiFinDB As Date = Nothing
        Dim FechaEstiFin As Date = Nothing
        Dim sql As String

        'Registrar fecha estimada de terminación persuasivo si no la tiene
        Dim cnx As New SqlConnection(Funciones.CadenaConexion)
        cnx.Open()
        sql = "SELECT FecEstiFin FROM PERSUASIVO WHERE NroExp = '" & pExpediente & "'"
        Dim command As New SqlCommand(sql, cnx)
        Dim reader As SqlDataReader = command.ExecuteReader()
        If reader.Read Then
            FecEstiFinDB = reader("FecEstiFin").ToString()
            swExisteRegistro = True
        End If
        reader.Close()
        cnx.Close()

        ' Hacer insert o update en tabla persuasivo
        FechaEstiFin = Date.ParseExact(txtEFIFECENTGES.Text.Trim, "dd/MM/yyyy", Nothing) 'variable calculada
        FechaEstiFin = FechaEstiFin.AddMonths(1)

        If swExisteRegistro Then
            'Actualizar PERSUASIVO.FecEstiFin si esta vacio
            If FecEstiFinDB = Nothing Then 'Dato de la base de datos
                sql = "UPDATE persuasivo SET FecEstiFin = @FecEstiFin WHERE NroExp = '" & pExpediente & "'"
            End If
        Else
            'Insertar registro en tabla de persuasivo
            sql = "INSERT INTO persuasivo (NroExp, FecEstiFin) VALUES ('" & pExpediente & "', @FecEstiFin)"
        End If

        Dim Command2 As New SqlCommand(sql, cnx)
        Command2.Parameters.AddWithValue("@FecEstiFin", FechaEstiFin)
        cnx.Open()
        Try
            Command2.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Registro inicial de persuasivo", "Expediente " & pExpediente, Command2)

        Catch ex As Exception
            CustomValidator2.Text = ex.Message
            CustomValidator2.IsValid = False

        End Try
        cnx.Close()

    End Sub

    Protected Sub cmdGestionar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdGestionar.Click
        Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx?ID=" & txtEFINROEXP.Text.Trim)
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""
        ' OJO SOLICITUDCAMBIOESTADO NO ES UNA TABLA, SINO UNA VISTA 
        ' 21/NOV/2014. Al Request("ID") NO se le debe colocar .Trim() .ToString, ni nada adicional
        ' debido a que el boton de adicionar envia ese parametro como vacio, y por consiguiente 
        ' Request("ID") llega como null
        sql = sql & "SELECT * FROM SOLICITUDCAMBIOESTADO WHERE NroExp = '" & Request("ID") & "'"

        Return sql

    End Function

    Private Sub BindGrid()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql        

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim idunico As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim expediente As String = grd.Rows(e.CommandArgument).Cells(1).Text

            Dim MTG As New MetodosGlobalesCobro

            Response.Redirect("EditSOLICITUDES_CAMBIOESTADO.aspx?ID=" & idunico & "&pExpediente=" & expediente)

        End If
    End Sub

End Class
