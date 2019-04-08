Public Partial Class plantilla
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Pagina As String = "DEPARTAMENTOS"
        Pagina = Request.QueryString("pag")
        contenido.InnerHtml = "<iframe src='" & Pagina & ".aspx' width='780px' height='100%' scrolling='no' frameborder='0'></iframe>"
        titulo.InnerHtml = Pagina
    End Sub

End Class