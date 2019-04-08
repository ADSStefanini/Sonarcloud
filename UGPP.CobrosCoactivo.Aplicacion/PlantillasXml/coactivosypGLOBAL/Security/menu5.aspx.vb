Public Partial Class menu5
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            menssageError(amsbgox)
            Exit Sub
        End If

        If Not IsPostBack Then
            If Session("datosbasicos") <> True Then
                'datosbasicos.Attributes.Add("class", "dialog_link")
            End If

            Dim codigo As String = Session("sscodigousuario").ToString
            Dim myclass_ As New DatosConsultasTablas
            Dim mytable As New DataTable
            myclass_.consultarUsuario(codigo, Trim(Session("mcobrador").ToString), mytable)

            If mytable.Rows.Count > 0 Then
                lblcodigo.Text = codigo.Trim
                lblNombre.Text = mytable.Rows(0).Item("nombre").ToString.Trim
                lblLogin.Text = mytable.Rows(0).Item("login").ToString.Trim
                lblcedula.Text = mytable.Rows(0).Item("documento").ToString.Trim

                Dim detalle As String = ""
                If mytable.Rows(0).Item("nivelacces").ToString.Trim = "1" Then
                    detalle = "1 (Administrador)"
                ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "2" Then
                    detalle = "2 (Operador sin permiso de actualizaci&#243;n)"
                ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "3" Then
                    detalle = "3 (Operador sin permiso de actualizaci&#243;n y de visualizaci&#243;n de documentos "
                End If

                lbldetalle.Text = detalle
            End If
        End If
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        Response.Redirect("../login.aspx")
    End Sub

End Class