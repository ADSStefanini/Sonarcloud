Public Class Bandeja
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
        End If
    End Sub

    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        Response.Redirect("~/login.aspx", True)
    End Sub
End Class