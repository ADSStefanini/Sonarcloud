Imports coactivosyp.My.Resources

Public Class ErrorPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblError.Text = StringsResourse.MsgError
    End Sub

End Class