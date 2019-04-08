Imports System.Data.SqlClient
Partial Public Class EditMENSAJES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadCboUsuDestino()
            LoadcboExpediente()

            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                'Actualizar mensaje
                ActualizarMensaje(Request("ID"))

                'Dim sql As String = "select * from MENSAJES where [idunico] = @idunico"
                Dim sql As String = "select MENSAJES.idunico, MENSAJES.NroExp, MENSAJES.UsuOrigen, MENSAJES.UsuDestino, MENSAJES.Mensaje, " & _
                         "MENSAJES.FecEnvio, MENSAJES.FecRecibo, MENSAJES.leido, U1.nombre AS NomRemitente, " & _
                         "U2.nombre AS NomDestinatario " & _
                        "from MENSAJES " & _
                        "LEFT JOIN USUARIOS U1 ON MENSAJES.UsuOrigen = U1.codigo " & _
                        "LEFT JOIN USUARIOS U2 ON MENSAJES.UsuDestino = U2.codigo " & _
                        "WHERE idunico = @idunico"

                Dim Command As New SqlCommand(sql, Connection)
                ' 'Set the @idunico parameter in the Command select query
                Command.Parameters.AddWithValue("@idunico", Request("ID"))

                Dim Reader As SqlDataReader = Command.ExecuteReader

                'El mensaje existe => Lo estan leyendo
                If Reader.Read Then

                    txtUsuOrigen.Text = Reader("NomRemitente").ToString()

                    'Destinatario
                    cboUsuDestino.SelectedValue = Reader("UsuDestino").ToString().Trim
                    cboUsuDestino.Enabled = False
                    cboUsuDestino.ForeColor = Drawing.Color.Red
                    cboUsuDestino.BackColor = Drawing.Color.White

                    'Expediente (Si esta relacionado con alguno)
                    'cboExpediente
                    cboExpediente.SelectedValue = Reader("NroExp").ToString().Trim
                    cboExpediente.Enabled = False
                    cboExpediente.ForeColor = Drawing.Color.Red
                    cboExpediente.BackColor = Drawing.Color.White

                    'Mensaje
                    taMensaje.InnerHtml = Reader("Mensaje").ToString()
                    'taMensaje.Disabled = True
                    taMensaje.Attributes("readonly") = "readonly"

                    txtFecEnvio.Text = Reader("FecEnvio").ToString().Trim
                    txtFecRecibo.Text = Reader("FecRecibo").ToString().Trim
                    If IsDBNull(Reader("leido")) = False Then
                        chkleido.Checked = Reader("leido").ToString()
                    End If

                    'A la hora de leer el mensaje deshabilitar el boton guardar
                    cmdSave.Visible = False


                    '23/OCT/2014. Datos para responder / reenviar un mensaje
                    Session("MsgIdRemitente") = Reader("UsuOrigen").ToString()
                    Session("MsgIdExpediente") = Reader("NroExp").ToString().Trim
                    Session("MsgMensaje") = vbCrLf & _
                                            vbCrLf & "__________________________________________________________________" & _
                                            vbCrLf & "De: " & Reader("NomRemitente").ToString().Trim & _
                                            vbCrLf & "Para: " & cboUsuDestino.SelectedItem.ToString & _
                                            vbCrLf & "Enviado:" & Reader("FecEnvio").ToString() & _
                                            vbCrLf & _
                                            vbCrLf & Reader("Mensaje").ToString()

                End If
                Reader.Close()
                Connection.Close()
            Else
                'Nuevo registro

                txtFecEnvio.Text = Left(Date.Now().ToString.Trim, 10)
                'Usuario origen
                txtUsuOrigen.Text = Session("ssnombreusuario")

                'Boton de responder
                cmdResponder.Visible = False

                'Usuario destino             
                'El combo ya esta lleno con los usuarios disponibles => falta el valor x defecto
                If Session("mnivelacces") = 4 Or Session("mnivelacces") = 5 Or Session("mnivelacces") = 6 Then
                    ' Si el usuario actual es de nivel 4,5,6=> el destinatario x defecto es el superior 
                    'Session("superior")
                    cboUsuDestino.SelectedValue = Session("superior").ToString.Trim
                ElseIf Session("mnivelacces") = 1 Or Session("mnivelacces") = 2 Or Session("mnivelacces") = 3 Then
                    cboUsuDestino.SelectedValue = GetIdGestor(Request("pExpediente"))
                Else
                    cboUsuDestino.SelectedValue = ""
                End If
                ' 22/octubre/2014
                'Usuario destino (si se trata de una acccion de responder el correo)                
                If Len(Request("pAccion")) > 0 Then
                    ' pAccion = 2 = Responder
                    If Request("pAccion") = 2 Then
                        cboUsuDestino.SelectedValue = Session("MsgIdRemitente") ' Remitente se vuelve destinatario                        
                    Else
                        If Request("pAccion") = 3 Then
                            cboUsuDestino.SelectedValue = ""
                        End If
                    End If
                    If Request("pAccion") = 2 Or Request("pAccion") = 3 Then
                        taMensaje.InnerHtml = Session("MsgMensaje")
                        cboExpediente.SelectedValue = Session("MsgIdExpediente")
                        cboExpediente.Enabled = False
                        cboExpediente.ForeColor = Drawing.Color.Red
                        cboExpediente.BackColor = Drawing.Color.White
                    End If

                End If

                'Numero del expediente
                ' Si se manda el mensaje desde dentro del expediente => colocar el valor del expediente en el combo
                If Len(Request("pExpediente")) > 0 Then
                    cboExpediente.SelectedValue = Request("pExpediente").Trim
                    cboExpediente.Enabled = False
                    cboExpediente.ForeColor = Drawing.Color.Red
                    cboExpediente.BackColor = Drawing.Color.White
                End If

            End If
        End If
        txtFecEnvio.ReadOnly = True
        txtFecRecibo.ReadOnly = True
        chkleido.Enabled = False
    End Sub

    'Marcar fecha en el DESTINATARIO lee el mensaje
    Private Sub ActualizarMensaje(ByVal pIdMensaje As String)

        'Si el usuario LOGUEADO es el DETINATARIO del mensaje => marcar
        If Session("sscodigousuario") = GetDestinatario(pIdMensaje) Then

            'Si el mensaje no esta marcado como leido => marcarlo
            If Not MensajeEstaMarcado(pIdMensaje) Then
                Dim Connection As New SqlConnection(Funciones.CadenaConexion)
                Connection.Open()
                Dim Command As SqlCommand
                Dim UpdateSQL As String = "UPDATE mensajes SET FecRecibo = GETDATE(), leido = 1 WHERE idunico = @idunico"
                Command = New SqlCommand(UpdateSQL, Connection)
                'Parametros
                Command.Parameters.AddWithValue("@idunico", pIdMensaje.Trim)

                'Ejecutar
                Try
                    Command.ExecuteNonQuery()
                    'Después de cada GRABAR hay que llamar al log de auditoria
                    Dim LogProc As New LogProcesos
                    LogProc.SaveLog(Session("ssloginusuario"), "Registro de mensajes", "Id de Mensaje " & pIdMensaje, Command)
                Catch ex As Exception

                End Try

                Connection.Close()
            End If
        End If
    End Sub

    Private Function MensajeEstaMarcado(ByVal pIdMensaje As String) As Boolean
        Dim Marcado As String = ""
        Dim FecRecibo As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT leido, FecRecibo FROM mensajes " & _
             "WHERE idunico = '" & pIdMensaje.Trim & "'"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Marcado = Reader("leido").ToString()
            FecRecibo = Reader("FecRecibo").ToString().Trim
        End If

        If Marcado = "" Or FecRecibo = "" Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function GetDestinatario(ByVal pIdMensaje As String) As String
        Dim IdDestinatario As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT UsuDestino FROM mensajes " & _
             "WHERE idunico = '" & pIdMensaje.Trim & "'"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            IdDestinatario = Reader("UsuDestino").ToString().Trim
        End If
        Return IdDestinatario
    End Function

    Private Sub LoadcboExpediente()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT efinroexp AS codigo, RTRIM(efinroexp) + ' (' +usuarios.nombre + ')' AS nombre FROM EJEFISGLOBAL " & _
                                "LEFT JOIN usuarios ON EJEFISGLOBAL.efiusuasig = usuarios.codigo"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtExpedientes As New DataTable
        Adaptador.Fill(dtExpedientes)
        If dtExpedientes.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaExp As DataRow = dtExpedientes.NewRow()
            filaExp("codigo") = ""
            filaExp("nombre") = "  "
            dtExpedientes.Rows.Add(filaExp)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaExp As DataView = New DataView(dtExpedientes)
            vistaExp.Sort = "codigo"
            '--------------------------------------------------------------------
            cboExpediente.DataSource = vistaExp
            cboExpediente.DataTextField = "nombre"
            cboExpediente.DataValueField = "codigo"
            cboExpediente.DataBind()
        End If
    End Sub

    Private Sub LoadCboUsuDestino()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM usuarios"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtUsuarios As New DataTable
        Adaptador.Fill(dtUsuarios)
        If dtUsuarios.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtUsuarios.NewRow()
            filaEstado("codigo") = ""
            filaEstado("nombre") = "  "
            dtUsuarios.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaEstados_Proceso As DataView = New DataView(dtUsuarios)
            vistaEstados_Proceso.Sort = "codigo"
            '--------------------------------------------------------------------
            cboUsuDestino.DataSource = vistaEstados_Proceso
            cboUsuDestino.DataTextField = "nombre"
            cboUsuDestino.DataValueField = "codigo"
            cboUsuDestino.DataBind()
        End If
    End Sub

    'Obtener el nombre del gestor asociado al expediente
    Private Function GetNomGestor(ByVal pExpediente As String) As String
        Dim NomGestor As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT EFIUSUASIG, USUARIOS.nombre FROM EJEFISGLOBAL " & _
             "LEFT JOIN USUARIOS ON   EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo " & _
             "WHERE EJEFISGLOBAL.EFINROEXP = '" & Request("pExpediente") & "'"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomGestor = Reader("nombre").ToString()
        End If
        Return NomGestor
    End Function

    'Obtener el codigo del gestor asociado al expediente
    Private Function GetIdGestor(ByVal pExpediente As String) As String
        Dim IdGestor As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT EFIUSUASIG FROM EJEFISGLOBAL " & _
             "WHERE EFINROEXP = '" & Request("pExpediente") & "'"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            IdGestor = Reader("EFIUSUASIG").ToString()
        End If
        Return IdGestor
    End Function

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        'Validar destinatario
        If cboUsuDestino.SelectedValue = "" Then
            CustomValidator1.Text = "Seleccione un destinatario por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        'Validar contenido
        If taMensaje.InnerHtml = "" Then
            CustomValidator1.Text = "El mensaje NO puede quedar vacío"
            CustomValidator1.IsValid = False
            Return
        End If

        Dim ID As String = Request("ID")
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        Dim InsertSQL As String = "Insert into MENSAJES ([NroExp], [UsuOrigen], [UsuDestino], [Mensaje], [FecEnvio]  ) VALUES ( @NroExp, @UsuOrigen, @UsuDestino, @Mensaje, @FecEnvio  ) "

        If String.IsNullOrEmpty(ID) Then
            ' insert 
            Command = New SqlCommand(InsertSQL, Connection)

            'Parametros 
            If Len(Request("pExpediente")) > 0 Then
                Command.Parameters.AddWithValue("@NroExp", Request("pExpediente").Trim)
            Else
                Command.Parameters.AddWithValue("@NroExp", cboExpediente.SelectedValue.Trim)
            End If
            Command.Parameters.AddWithValue("@UsuOrigen", Session("sscodigousuario"))
            Command.Parameters.AddWithValue("@UsuDestino", cboUsuDestino.SelectedValue.Trim)
            Command.Parameters.AddWithValue("@Mensaje", taMensaje.InnerHtml)
            Command.Parameters.AddWithValue("@FecEnvio", Date.Now)

            'Cerrar objeto command y conexion
            Try
                Command.ExecuteNonQuery()

                'Después de cada GRABAR hay que llamar al log de auditoria
                Dim LogProc As New LogProcesos
                If Len(Request("pExpediente")) > 0 Then
                    LogProc.SaveLog(Session("ssloginusuario"), "Registro de mensajes", "Id de Mensaje " & Request("pExpediente").Trim, Command)
                Else
                    LogProc.SaveLog(Session("ssloginusuario"), "Registro de mensajes", "Id de Mensaje " & cboExpediente.SelectedValue.Trim, Command)
                End If

            Catch ex As Exception

            End Try

            Connection.Close()
        End If
        ' 
        Response.Redirect("MENSAJES.aspx?pExpediente=" & Request("pExpediente"))
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("MENSAJES.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdMostrarExpediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdMostrarExpediente.Click
        Dim Exp As String = ""    
        Dim NroConsignacion As String = "" ' Para mensajes al verificador de pagos

        Exp = cboExpediente.SelectedValue.Trim

        If Exp = "" Then
            CustomValidator1.Text = "Este mensaje no fue asociado directamente a un expediente"
            CustomValidator1.IsValid = False
        Else
            If Session("mnivelacces") = 5 Then
                ' Repartidor
                Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx?ID=" & Exp)

            ElseIf Session("mnivelacces") = 6 Then
                'Verificador de pagos
                NroConsignacion = GetNroConsignacion(Request("id"))
                'Response.Redirect("EditPAGOS.aspx?ID=" & NroConsignacion)
                Response.Redirect("PAGOS.aspx?pExpediente=" & Exp)

            Else
                Response.Redirect("EditEJEFISGLOBAL.aspx?ID=" & Exp)

            End If

        End If
    End Sub

    Private Function GetNroConsignacion(ByVal pIdMensaje As String) As String
        Dim NroConsig As String = ""

        Dim cnx As New SqlConnection(Funciones.CadenaConexion)
        cnx.Open()
        Dim sql As String = "SELECT nroConsignacion FROM MENSAJES WHERE idunico = " & pIdMensaje
        Dim cmd As New SqlCommand(sql, cnx)
        Dim reader As SqlDataReader = cmd.ExecuteReader
        If reader.Read Then
            NroConsig = reader("nroConsignacion").ToString.Trim
        End If

        Return NroConsig
    End Function

    Protected Sub cmdResponder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdResponder.Click
        ' Parametro (pAccion) 1 = Nuevo, 2 = Responder, 3 = Reenviar
        Response.Redirect("EditMENSAJES.aspx?pExpediente=" & Request("pExpediente") & "&pDestinatario=" & Session("MsgIdRemitente") & "&pAccion=2")
    End Sub

    Protected Sub cmdReenviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReenviar.Click
        ' Parametro (pAccion) 1 = Nuevo, 2 = Responder, 3 = Reenviar
        Response.Redirect("EditMENSAJES.aspx?pExpediente=" & Request("pExpediente") & "&pDestinatario=" & Session("MsgIdRemitente") & "&pAccion=3")
    End Sub
End Class