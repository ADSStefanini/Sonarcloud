Partial Public Class MENU_GESTORES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Pagina As String = "Opciones para gestores"
        Pagina = Request.QueryString("pag")        
        titulo.InnerHtml = Pagina
    End Sub

End Class