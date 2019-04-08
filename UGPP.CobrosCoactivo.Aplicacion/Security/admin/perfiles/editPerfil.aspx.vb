Public Class editPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Len(Request("idPerfil")) = False Then
                Response.Redirect(My.Resources.Formularios.urlPerfiles, True)
            End If
        End If
    End Sub

End Class