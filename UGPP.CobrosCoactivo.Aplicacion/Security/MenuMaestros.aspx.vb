Public Partial Class MenuMaestros
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
            If Session("meregisActos") <> True Then
                meregisActos.HRef = "javascript:alert('No tiene permiso');"
            End If
        End If

        If Not IsPostBack Then
            If Session("meregisEtapa") <> True Then
                meregisEtapa.HRef = "javascript:alert('No tiene permiso');"
            End If
        End If

        If Not IsPostBack Then
            If Session("meregideu") <> True Then
                'meregideu.HRef = "javascript:alert('No tiene permiso');"
            End If
        End If

        If Not IsPostBack Then
            If Session("mesecueacto") <> True Then
                mesecueacto.HRef = "javascript:alert('No tiene permiso');"
            End If
        End If

        If Not IsPostBack Then
            If Session("meregisEntes") <> True Then
                meregisEntes.HRef = "javascript:alert('No tiene permiso');"
            End If
        End If

        If Not IsPostBack Then
            If Session("mefestivoadd") <> True Then
                mefestivoadd.HRef = "javascript:alert('No tiene permiso');"
            End If
        End If

    End Sub

    Protected Sub A8_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A8.Click
        FormsAuthentication.SignOut()
        Session("UsuarioValido") = Nothing
        Response.Redirect("../login.aspx")
    End Sub
End Class