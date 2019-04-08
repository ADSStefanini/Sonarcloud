Public Partial Class cobranzatipo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Response.Redirect("~/Login.aspx")
        End If
    End Sub

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Protected Sub btnEjecutarultimoacto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEjecutarultimoacto.Click
        If Session("mnivelacces") = "1" Then
            Ejecutarjavascript(Me, "<script ""text/javascript"">window.open('ultimoActo.aspx?documento=" & ViewState("Arch") & "','Graph2','status=0,left=170,top=120,width=300,height=150,scrollbars=auto');</script>", "Reporte")
        Else
            Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Los sentimos, esta es una operación que solo un administrador del sistema puede ejecutar.<br />" _
            & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
            & "Derechos de acceso denegados, La función de seguridad de control de acceso de usuario UAC (User Control Access)  están activadas. Se recomienda llamar a su administrador."
            menssageError(amsbgox)
        End If
    End Sub

    Protected Sub btnconsulta_1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnconsulta_1.Click
        Response.Redirect("informepagos.aspx")
    End Sub

End Class