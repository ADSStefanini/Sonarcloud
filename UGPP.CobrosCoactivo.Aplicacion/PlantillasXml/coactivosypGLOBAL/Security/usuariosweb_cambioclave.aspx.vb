Imports System.Data.SqlClient
Partial Public Class usuariosweb_cambioclave
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img style='background-color:#fff;' src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            menssageError(amsbgox)
            Exit Sub
        End If

        If Not IsPostBack Then
            Try
                Dim codigo As String = Session("sscodigousuario").ToString
                Dim myclass_ As New DatosConsultasTablas
                Dim mytable As New DataTable

                myclass_.consultarUsuario(codigo.Trim, Trim(Session("mcobrador").ToString), mytable)

                If mytable.Rows(0).Item("nivelacces").ToString.Trim = "1" Then
                    If Request("codigo") <> "" Or Request("codigo") IsNot Nothing Then
                        mytable = New DataTable
                        myclass_.consultarUsuario(Request("codigo"), Trim(Session("mcobrador").ToString), mytable)
                        lblcodigo.Text = Request("codigo")
                    Else
                        lblcodigo.Text = codigo
                    End If
                Else
                    lblcodigo.Text = codigo
                End If

                If mytable.Rows.Count > 0 Then

                    lblNombre.Text = mytable.Rows(0).Item("nombre").ToString.Trim
                    lblLogin.Text = mytable.Rows(0).Item("login").ToString.Trim
                    lblcedula.Text = mytable.Rows(0).Item("documento").ToString.Trim

                    Dim detalle As String = ""
                    If mytable.Rows(0).Item("nivelacces").ToString.Trim = "1" Then
                        detalle = "Nivel (Permisos) : 1 (Administrador)"
                    ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "2" Then
                        detalle = "Nivel (Permisos) : 2 (Abogado)"
                    ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "3" Then
                        detalle = "Nivel (Permisos) : 3 (Solo consulta)"
                    End If
                    ViewState("contrasennaanterior") = mytable.Rows(0).Item("clave").ToString.Trim

                    lbldetalle.Text = detalle
                End If

            Catch ex As Exception
                Messenger.InnerHtml = "<font color='#8A0808'>Error : " & ex.Message & "<br />Es posible haya espirado la sesión. Intente salir y entrar al sistema.  </font>"
            End Try
        End If
    End Sub

    Private Function vericlave(ByVal Clave As String, ByVal Cod As String) As Boolean
        Dim sql As String = "SELECT * FROM USUARIOS WHERE CODIGO = @CODIGO AND CLAVE = @CLAVE"
        Using conexion As New SqlConnection(Funciones.CadenaConexion)
            Using myadapter As New SqlDataAdapter(sql, conexion)
                myadapter.SelectCommand.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = Cod
                myadapter.SelectCommand.Parameters.Add("@CLAVE", SqlDbType.VarChar).Value = Encrypt(Clave)
                Dim tb As New DataTable
                myadapter.Fill(tb)
                If tb.Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Using
    End Function

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            If txtpwsNuevaContraseña.Text = txtpwsConfirmar.Text Then
                Dim con As New SqlConnection(Session("ConexionServer").ToString)
                con.Open()


                Dim lcUsuario As String = ""
                Dim Mycommand As New SqlCommand("UPDATE USUARIOS SET CLAVE = @clave WHERE CODIGO = @codigo", con)
                Mycommand.Parameters.Add("@clave", SqlDbType.VarChar).Value = Encrypt(txtpwsNuevaContraseña.Text.Trim)
                If Request("codigo") = Nothing Then
                    Mycommand.Parameters.Add("@codigo", SqlDbType.VarChar).Value = Session("sscodigousuario").ToString
                    lcUsuario = Session("sscodigousuario").ToString
                Else
                    Mycommand.Parameters.Add("@codigo", SqlDbType.VarChar).Value = Request("codigo").ToString
                    lcUsuario = Request("codigo").ToString
                End If


                Mycommand.ExecuteNonQuery()

                'Después de cada GRABAR hay que llamar al log de auditoria
                Dim LogProc As New LogProcesos
                LogProc.SaveLog(Session("ssloginusuario"), "Actualización de contraseñas ", "Usuario " & lcUsuario, Mycommand)
                '-----------------------------

                Messenger.InnerHtml = "<font color='#ffffff'><b style='text-decoration:underline;'>Contraseña : Su nueva contraseña fue guardada con éxito.</b></font>"
                Ejecutarjavascript("<script type=text/javascript> $(document).ready(function(){CronoID = setTimeout(""timerOper()"", 5000);}); </script>", "CancelarClient")

                Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - CONTRASEÑA</h2><img style='background-color:#fff;margin-right:2px;' id='imgcamclave' src='images/icons/SecurityON.png' height = '100' width = '100' />Su contraseña fue cambiada con éxito,  recuerde cambiarla periódicamente por criterios de seguridad.<br />" _
                & "<br /><hr /><h2>MODO SEGURO</h2>" _
                & "<div id='Messenger2'>Espere un momento.........</div>"

                ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel()"
                menssageError(amsbgox)

            Else
                Messenger.InnerHtml = "<span style='color:#8A0808;'>La clave anterior no coincide....</span>"
            End If
        Catch ex As Exception
            Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Acceso invalido o conexión no habilitada.<br />" _
                     & "<br /><hr /><h2>ERROR TÉCNICO</h2>"
            Messenger.InnerHtml = "<font color='#8A0808'>Error : " & ex.Message & "<br />Es posible haya espirado la sesión. Intente salir y entrar al sistema.  </font>"
            menssageError(amsbgox & Messenger.InnerHtml)
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
End Class