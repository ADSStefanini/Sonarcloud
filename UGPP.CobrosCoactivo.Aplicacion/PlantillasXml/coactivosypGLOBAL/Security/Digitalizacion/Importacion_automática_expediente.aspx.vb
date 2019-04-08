Public Partial Class Importacion_automática_expediente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnImpdatos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImpdatos.Click
        Threading.Thread.Sleep(2000)
    End Sub

End Class