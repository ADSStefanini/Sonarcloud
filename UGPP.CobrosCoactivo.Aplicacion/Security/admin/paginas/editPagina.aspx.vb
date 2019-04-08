Public Class editPagina
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Len(Request("idPagina")) = False Then
                Response.Redirect(My.Resources.Formularios.urlPaginas, True)
            End If
        End If
    End Sub

End Class