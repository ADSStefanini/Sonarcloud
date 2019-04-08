Public Class Tabs
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
        Dim logSessionInit As New LogProcesos
        logSessionInit.SaveLog(Session("ssnombreusuario"), "Inicio Session", String.Empty, "")
    End Sub

    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        Response.Redirect("~/login.aspx", True)
    End Sub
End Class