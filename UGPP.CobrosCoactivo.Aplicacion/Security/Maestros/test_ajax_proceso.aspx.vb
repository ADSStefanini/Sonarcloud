Public Partial Class test_ajax_proceso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim resultado As Integer = Val(Request("valorCaja1")) + Val(Request("valorCaja2"))
        Response.Write(resultado)
    End Sub

End Class