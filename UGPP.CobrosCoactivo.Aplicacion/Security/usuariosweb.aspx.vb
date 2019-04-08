Imports System.Data.SqlClient
Partial Public Class usuariosweb2
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Private Sub cargeUser()
        Dim Table As DatasetForm.usuariosDataTable = LoadDatos()
        Me.ViewState("DatosUser") = Table
        dtgUsuarios.DataSource = Table
        dtgUsuarios.DataBind()
    End Sub

    Private Function LoadDatos(Optional ByVal codigo As String = "") As DataTable
        Dim cnn As String = Session("ConexionServer")
        Dim MyAdapter As New SqlDataAdapter
        If codigo = "" Then
            MyAdapter = New SqlDataAdapter("SELECT codigo ,nombre ,documento ,clave ,nivelacces ,cobrador ,apppredial ,appvehic ,appcuotasp ,appindycom ,login,conuser, useractivo ,useremail ,usercamclave, superior FROM usuarios", cnn)
        Else
            MyAdapter = New SqlDataAdapter("SELECT codigo ,nombre ,documento ,clave ,nivelacces ,cobrador ,apppredial ,appvehic ,appcuotasp ,appindycom ,login,conuser, useractivo ,useremail ,usercamclave, superior FROM usuarios where codigo = '" & codigo & "'", cnn)
        End If

        Dim Mytb As New DatasetForm.usuariosDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

    Private Function ultimo() As String
        Dim myadapter As New SqlDataAdapter("select * from MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 1", Session("ConexionServer").ToString)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1
        Return Format(Con, "0000")
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function GetData() As String
        Dim form As New usuariosweb2
        Return form.ultimo()
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("usuario") <> True Then
                Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha  intentado una Entrada inválida.<br />" _
                                      & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
                                      & "Este usuario no tiene permisos para ingresar a esta ventana, favor consulte con su administrador. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

                ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel_()"
                menssageError(amsbgox)
                Exit Sub
            End If

            Try

                CommonsCobrosCoactivos.poblarPerfiles(lstNivel)
                Call cargeUser()
                txtCodigo.Text = ultimo()
                lblCobrador.InnerHtml = Session("mcobrador") & "::" & Session("mnombcobrador")
            Catch ex As Exception
                Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Acceso invalido o conexión no habilitada.<br />" _
                & "<br /><hr /><h2>ERROR TÉCNICO</h2>"

                Messenger.InnerHtml = "<font color='#8A0808' >Error:" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
                menssageError(amsbgox & Messenger.InnerHtml)
            End Try

            'Cargar combo de superiorres
            LoadcboSuperior()
        End If
    End Sub

    Private Sub ActualizarSuperior(ByVal pUsuario As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        Dim UpdateSQL As String = "UPDATE usuarios SET superior = '" & cboSuperior.SelectedValue.Trim & "' WHERE codigo ='" & pUsuario & "'"
        Command = New SqlCommand(UpdateSQL, Connection)
        'Parametros
        'Command.Parameters.AddWithValue("@superior", cboSuperior.SelectedValue)
        'Command.Parameters.AddWithValue("@codigo", pUsuario.Trim)
        'Ejecutar
        Command.ExecuteNonQuery()

        'Después de cada GRABAR hay que llamar al log de auditoria
        Dim LogProc As New LogProcesos
        LogProc.SaveLog(Session("ssloginusuario"), "Actualización de superior de un gestor o usuario ", "Código " & pUsuario, Command)

        Connection.Close()

    End Sub

    Private Sub LoadcboSuperior()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "SELECT codigo, nombre FROM usuarios"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboSuperior.DataTextField = "nombre"
        'cboSuperior.DataValueField = "codigo"
        'cboSuperior.DataSource = Command.ExecuteReader()
        'cboSuperior.DataBind()
        'Connection.Close()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM usuarios"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtUsuarios As New DataTable
        Adaptador.Fill(dtUsuarios)
        If dtUsuarios.Rows.Count > 0 Then
            '------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtUsuarios.NewRow()
            filaEstado("codigo") = ""
            filaEstado("nombre") = "  "
            dtUsuarios.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaEstados_Proceso As DataView = New DataView(dtUsuarios)
            vistaEstados_Proceso.Sort = "codigo"
            '--------------------------------------------------------------------
            cboSuperior.DataSource = vistaEstados_Proceso
            cboSuperior.DataTextField = "nombre"
            cboSuperior.DataValueField = "codigo"
            cboSuperior.DataBind()
        End If
    End Sub

    Private Sub dtgUsuarios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dtgUsuarios.PageIndexChanging
        dtgUsuarios.PageIndex = e.NewPageIndex
        'cargeUser()
    End Sub


    Private Function GetSuperior(ByVal pUsuario As String)
        Dim superior As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT superior from usuarios WHERE codigo = @codigo"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@codigo", pUsuario.Trim)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            superior = Reader("superior").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return superior
    End Function

    Protected Sub dtgUsuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgUsuarios.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.usuariosDataTable = CType(ViewState("DatosUser"), DatasetForm.usuariosDataTable)
            With Mytb.Item(.dtgUsuarios.SelectedIndex)

                Try
                    Messenger.InnerHtml = "" : cancelar()

                    txtCodigo.Text = .codigo
                    txtNombre.Text = .nombre.ToUpper.Trim
                    txtLogin.Text = .login.ToUpper.Trim
                    chkactivo.Checked = .useractivo

                    '28/ene/2014. Superior
                    'cboSuperior
                    cboSuperior.SelectedValue = GetSuperior(.codigo.Trim)

                    If Not .IsclaveNull Then txtClave.Attributes.Add("Value", .clave.Trim) : txtClave.Enabled = False
                    If Not .IsdocumentoNull Then txtCedula.Text = .documento.Trim
                    If Not .IsuseremailNull Then txtemail.Text = .useremail

                    txtConfirmarClave.Text = .clave.Trim
                    txtConfirmarClave.Attributes.Add("Value", .clave.Trim) : txtConfirmarClave.Enabled = False
                    lstNivel.SelectedValue = .nivelacces

                    Me.ViewState("seleccionado") = .codigo.ToString
                    Me.ViewState("useractivo") = .useractivo
                    Div1.InnerHtml = "<font color='#FFFFFF'>(<b>Usuario seleccionado : </b>" & .nombre.ToUpper.Trim & ")</font>"
                Catch ex As Exception
                    Messenger.InnerHtml = "<span style='color:yellow;'>Error : " & ex.Message & "</span>"

                    Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Acceso invalido o conexión no habilitada.<br />" _
                         & "<br /><hr /><h2>ERROR TÉCNICO</h2>"

                    menssageError(amsbgox & "<span style='color:red;'>Error : " & ex.Message & "</span>")
                End Try
            End With
        End With
    End Sub

    Private Sub cancelar()
        txtCedula.Text = ""
        txtClave.Text = ""
        txtClave.Attributes.Remove("Value")
        txtLogin.Text = ""
        txtNombre.Text = ""
        txtConfirmarClave.Text = ""
        txtConfirmarClave.Attributes.Remove("Value")
        txtemail.Text = ""
    End Sub

    Function siLogin(ByVal codigo As String) As Boolean
        Using Command As New System.Data.SqlClient.SqlCommand("SELECT LOGIN FROM USUARIOS WHERE LOGIN = @CODIGO", New SqlConnection(Funciones.CadenaConexion))
            Command.Parameters.AddWithValue("@CODIGO", codigo)
            Command.CommandTimeout = 60000

            Dim Adapt As New SqlDataAdapter(Command)
            Dim tb As New DataTable
            Adapt.Fill(tb)
            If tb.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    Private Sub btnAceptar_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim SwGrabar As Boolean = False
        Try
            If Not txtemail.Text = Nothing Then
                Dim bool As String = Funciones.validar_Mail(txtemail.Text)
                If Not bool = True Then
                    Messenger.InnerHtml = "<span style='color:#dee359'><b>Lo sentimos debe suministrar una cuenta E-Mail valida.</b></span>"
                    Exit Sub
                End If
            End If
            If txtLogin.Text = Nothing Then
                Messenger.InnerHtml = "<span style='color:#dee359'><b>Lo sentimos debe suministrar un Login valido.</b></span>"
                Exit Sub
            End If

            Using con As New SqlConnection(Funciones.CadenaConexion)
                With Me

                    Dim proximo_numero As String = "0000"
                    Dim da As New SqlDataAdapter("SELECT TOP 1 * FROM USUARIOS", con)
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                    ' Creamos los comandos con el CommandBuilder
                    Dim cb As New SqlCommandBuilder(da)

                    da.InsertCommand = cb.GetInsertCommand()
                    da.UpdateCommand = cb.GetUpdateCommand()
                    'da.DeleteCommand = cb.GetDeleteCommand()

                    Dim LogProc As New LogProcesos

                    con.Open()
                    Dim tran As SqlTransaction = con.BeginTransaction

                    da.InsertCommand.Transaction = tran
                    da.UpdateCommand.Transaction = tran
                    'da.DeleteCommand.Transaction = tran

                    Dim Mytb As DatasetForm.usuariosDataTable = CType(.ViewState("DatosUser"), DatasetForm.usuariosDataTable)



                    If Mytb.Select("CODIGO='" & .txtCodigo.Text & "'").Length > 0 Then

                        'En esta parate se procede a actualizar el registro si existe 
                        Dim Row As DatasetForm.usuariosRow = Mytb.Select("CODIGO='" & .txtCodigo.Text & "'")(0)
                        If Not Row Is Nothing Then
                            Row.nombre = .txtNombre.Text
                            Row.documento = .txtCedula.Text
                            Row.login = .txtLogin.Text
                            Row.nivelacces = lstNivel.SelectedValue
                            Row.useremail = txtemail.Text
                            Row.useractivo = chkactivo.Checked
                        End If
                    Else
                        Dim siLo As Boolean = siLogin(txtLogin.Text.Trim.ToUpper)
                        If siLo = True Then
                            Messenger.InnerHtml = "<span style='color:#dee359;'>No puede usar un <b>Login</b> que ya existe .....</span>"
                            Exit Sub
                        End If
                        'Esta parte se ingresa un registro nuevo
                        'Actualizar el consecutivo
                        Using mycommand As New SqlCommand("update MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = 1", con)
                            mycommand.Parameters.Add("@proximo_numero", SqlDbType.Int)
                            mycommand.Parameters("@proximo_numero").Direction = ParameterDirection.Output
                            mycommand.Transaction = tran
                            mycommand.ExecuteNonQuery()

                            'Después de cada GRABAR hay que llamar al log de auditoria
                            'Dim LogProc As New LogProcesos
                            LogProc.SaveLog(Session("ssloginusuario"), "Actualización de consecutivos ", "CON_IDENTIFICADOR = 1 ", mycommand)
                            '-------

                            Dim conse As Integer = CType(mycommand.Parameters("@proximo_numero").Value, Integer)
                            proximo_numero = Format(conse, "0000")
                            Dim pass As String = Encrypt(txtClave.Text)
                            Mytb.AddusuariosRow(txtCodigo.Text, .txtNombre.Text.ToUpper, .txtCedula.Text, pass, .lstNivel.SelectedValue, Session("mcobrador"), "S", "S", "S", "S", txtLogin.Text.ToUpper, True, txtemail.Text.ToUpper, False)
                            OpMenu(proximo_numero, con, tran)
                        End Using
                    End If

                    Try
                        ' Add handler
                        AddHandler da.RowUpdating, New SqlRowUpdatingEventHandler(AddressOf da_RowUpdating)

                        'Actualizamos los datos de la tabla
                        da.Update(Mytb)
                        tran.Commit()
                        'Mytb.AcceptChanges()

                        ' Remove handler
                        RemoveHandler da.RowUpdating, New SqlRowUpdatingEventHandler(AddressOf da_RowUpdating)

                        'Después de cada GRABAR hay que llamar al log de auditoria    
                        If TipoComandoUsado = "INSERT" Then
                            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de usuarios", "codigo, nombre", da.InsertCommand)
                        Else
                            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de usuarios", "codigo, nombre", da.UpdateCommand)
                        End If


                        Messenger.InnerHtml = "<span style='color:#dee359'><b> Se han guardado los datos</b></span>"

                        SwGrabar = True
                    Catch ex As Exception
                        ' Si hay error, desahacemos lo que se haya hecho
                        tran.Rollback()
                        Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
                    End Try
                    con.Close()
                End With
            End Using
        Catch ex As SqlException
            Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Acceso invalido o conexión no habilitada.<br />" _
                         & "<br /><hr /><h2>ERROR TÉCNICO</h2>"
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error:</b> " & ex.Message & "</font>"
            menssageError(amsbgox & Messenger.InnerHtml)
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try

        If SwGrabar Then
            ActualizarSuperior(txtCodigo.Text.Trim)

            Call cancelar()
            Call cargeUser()
            txtCodigo.Text = ultimo()
            txtClave.Enabled = True : txtConfirmarClave.Enabled = True
            Response.Redirect("menu4.aspx")
        End If

    End Sub

    Private Shared Sub da_RowUpdating(ByVal sender As Object, ByVal e As SqlRowUpdatingEventArgs)
        If e.Command IsNot Nothing Then
            TipoComandoUsado = e.StatementType.ToString.ToUpper
        End If
    End Sub

    Private Sub OpMenu(ByVal Cod As String, ByVal con As SqlConnection, ByVal Tran As SqlTransaction)
        Dim Permiso As String = lstNivel.SelectedValue
        If Permiso = 1 Then
            Permiso = "ADMIN"
        ElseIf Permiso = 2 Then
            Permiso = "ABOGADO"
        Else
            Permiso = "CONSULTA"
        End If

        Dim command As New SqlCommand("INSERT INTO USUARIO_PPALMENU SELECT @CODIGOID AS ME_USUARIO ,ME_OPCIONMENU,ME_PERMISO,ME_OPCIONINDEX, ME_MENU,ME_DETALLEOPCION FROM USUARIO_PPALMENU WHERE ME_USUARIO = @PERMISO", con)
        command.Parameters.Add("@CODIGOID", SqlDbType.VarChar).Value = Cod
        command.Parameters.Add("@PERMISO", SqlDbType.VarChar).Value = Permiso
        command.Transaction = Tran
        command.ExecuteNonQuery()

        'Después de cada GRABAR hay que llamar al log de auditoria
        'Dim LogProc As New LogProcesos
        'LogProc.SaveLog(Session("ssloginusuario"), "Actualización de USUARIO_PPALMENU ", "ME_USUARIO = " & Permiso, command)
        '-------

    End Sub

    Protected Sub btnRecargarUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRecargarUser.Click
        Try
            Call cargeUser()
            txtCodigo.Text = ultimo()

            Ejecutarjavascript("<script type=""text/javascript""> $(document).ready(function() { $(""#btnCancelar"").click(); });</script>", "Cancelar")
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808' >Error :" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
        End Try
    End Sub

    Protected Sub btnCambioClave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCambioClave.Click

        If Me.ViewState("seleccionado") = Nothing Then
            Div1.InnerHtml = "<font color='#8A0808'><b>Error :</b> Seleccione un usuario para continuar </font>"

            Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Seleccione un usuario de la lista para continuar." _
                        & "<br /><hr /><h2>ERROR TÉCNICO</h2>Seguir los pasos  adecuados para continuar."
            menssageError(amsbgox)
        Else
            Response.Redirect("usuariosweb_cambioclave.aspx?codigo=" & txtCodigo.Text.ToString & "", False)
        End If
    End Sub

   

    Private Sub btnSi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSi.Click
        Try
            If Me.ViewState("useractivo") Is Nothing Then
                Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b> Seleccione un usuario </font>"
                Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Seleccione un usuario de la lista para continuar." _
                        & "<br /><hr /><h2>ERROR TÉCNICO</h2>Seguir los pasos  adecuados para continuar."
                menssageError(amsbgox)
            Else
                Dim activodes As String = Me.ViewState("useractivo")
                Dim conexcion As New SqlConnection(Session("ConexionServer").ToString)
                Try
                    Dim mycmd As New SqlCommand
                    mycmd.CommandType = CommandType.Text
                    conexcion.Open()
                    mycmd.Connection = conexcion


                    If activodes = True Then
                        mycmd.CommandText = "UPDATE USUARIOS SET USERACTIVO = " & 0 & " WHERE CODIGO = '" & Me.ViewState("seleccionado").ToString & "'"
                        Me.ViewState("useractivo") = False
                    Else
                        mycmd.CommandText = "UPDATE USUARIOS SET USERACTIVO = " & 1 & " WHERE CODIGO = '" & Me.ViewState("seleccionado").ToString & "'"
                        Me.ViewState("useractivo") = True
                        'Response.Write("Desea activar el usuario.")
                    End If
                    mycmd.ExecuteNonQuery()

                    'Después de cada GRABAR hay que llamar al log de auditoria
                    Dim LogProc As New LogProcesos
                    LogProc.SaveLog(Session("ssloginusuario"), "Actualización de usuarios", "ME_USUARIO = " & Me.ViewState("seleccionado").ToString.Trim, mycmd)

                    Call cargeUser()
                    txtCodigo.Text = ultimo()
                    Ejecutarjavascript("<script type='text/javascript'>$(document).ready(function(){ $('#btnCancelar').click(); }); </script>", "Mesaje_1")
                Catch ex As Exception
                    Me.ViewState("useractivo") = activodes
                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Ejecutarjavascript(ByVal script As String, ByVal NomScript As String)
        Dim csname1 As [String] = NomScript
        Dim cstype As Type = Me.[GetType]()

        Dim cs As ClientScriptManager = Page.ClientScript

        If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As String = script
            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())
        End If
    End Sub

    'Protected Sub HyperLinkImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles HyperLinkImprimir.Click
    '    Dim cr As New rptuser
    '    Dim DtsDatos As DatasetForm = New DatasetForm
    '    Dim DatosConsultasTablas As New DatosConsultasTablas
    '    DatosConsultasTablas.Load_Configuracion(Session("mcobrador"), DtsDatos.CAT_CLIENTES)
    '    DatosConsultasTablas.Tipear_Tabla(CType(Me.ViewState("DatosUser"), DatasetForm.usuariosDataTable), DtsDatos.usuarios)

    '    Dim List As New List(Of ItemParams)
    '    For Each Par As CrystalDecisions.Shared.ParameterField In cr.ParameterFields
    '        Select Case Par.Name
    '            Case "EnteCobrador"
    '                List.Add(New ItemParams("EnteCobrador", Session("mnombcobrador")))
    '            Case Else
    '                List.Add(New ItemParams(Par.Name, "Sin Valor"))
    '        End Select
    '    Next

    '    Funciones.Exportar(Me, cr, DtsDatos, "usuarios" & Now().ToString("dd/MM/yyyy") & ".Pdf", "", List)
    'End Sub

    Private Sub userLO(ByVal data As DataTable)
        data = CType(Me.ViewState("DatosUser"), DatasetForm.usuariosDataTable)
    End Sub

    Protected Sub btnDesactivar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDesactivar.Click
        Dim SQL As String = "UPDATE usuarios SET useractivo = 1 WHERE LOGIN IN ("

        Dim presentado As CheckBox
        Dim sw As Boolean = False
        For Each row As GridViewRow In dtgUsuarios.Rows
            presentado = dtgUsuarios.Rows(row.RowIndex).FindControl("chkSelec")
            If presentado.Checked Then
                SQL += "'" & row.Cells(1).Text & "',"
                sw = True
                'Else
                'sw = False
            End If
        Next
        If sw Then
            Me.ViewState("useractivo") = 1
            SQL = SQL.Substring(0, SQL.Length - 1) & ")"
            Dim cmd As New SqlCommand(SQL, New SqlConnection(Funciones.CadenaConexion))
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Actualización de usuarios", "Actualización en bloque ", cmd)

            cmd.Connection.Close()
            Messenger.InnerHtml = "<span style='color:#dee359'><b>Usuarios habilitados correctamente...</b></span>"
        Else
            Messenger.InnerHtml = "<span style='color:#dee359'><b>No ha chekeado ningun usuario.</b></span>"
        End If
    End Sub

    Protected Sub chkselccion_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkselccion.CheckedChanged
        Dim presentado As CheckBox
        If dtgUsuarios.Rows.Count > 0 Then
            If chkselccion.Checked Then
                For Each row As GridViewRow In dtgUsuarios.Rows
                    presentado = dtgUsuarios.Rows(row.RowIndex).FindControl("chkSelec")
                    presentado.Checked = True
                Next
            Else
                For Each row As GridViewRow In dtgUsuarios.Rows
                    presentado = dtgUsuarios.Rows(row.RowIndex).FindControl("chkSelec")
                    presentado.Checked = False
                Next
            End If

        End If


    End Sub

    Protected Sub btndesausu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btndesausu.Click
        Dim SQL As String = "UPDATE usuarios SET useractivo = 0 WHERE LOGIN IN ("

        Dim presentado As CheckBox
        Dim sw As Boolean = False
        For Each row As GridViewRow In dtgUsuarios.Rows
            presentado = dtgUsuarios.Rows(row.RowIndex).FindControl("chkSelec")
            If presentado.Checked Then
                SQL += "'" & row.Cells(1).Text & "',"
                sw = True
                'Else
                'sw = False
            End If
        Next
        If sw Then
            Me.ViewState("useractivo") = 2
            SQL = SQL.Substring(0, SQL.Length - 1) & ")"

            Dim cmd As New SqlCommand(SQL, New SqlConnection(Funciones.CadenaConexion))
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Actualización de usuarios", "Actualización en bloque ", cmd)

            cmd.Connection.Close()
            Messenger.InnerHtml = "<span style='color:#dee359'><b>Usuarios deshabilitados correctamente...</b></span>"
        Else
            Messenger.InnerHtml = "<span style='color:#dee359'><b>No ha chekeado ningun usuario.</b></span>"
        End If
    End Sub
End Class