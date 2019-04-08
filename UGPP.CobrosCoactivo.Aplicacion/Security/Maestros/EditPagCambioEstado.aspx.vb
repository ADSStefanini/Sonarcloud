Partial Public Class EditPagCambioEstado
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
        End If

    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("EditPAGOS.aspx?ID=" & HttpUtility.HtmlAttributeEncode(Request("ID")).ToString.Replace("+", "%2B"))
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()

        Response.Redirect("../../login.aspx")
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        Session.RemoveAll()
    End Sub

End Class