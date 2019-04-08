Public Class TestCargaDocumento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'editDoc.idTituloDocumento = "12"
        End If
    End Sub

    Protected Sub btnTestIdMaestroDoc_Click(sender As Object, e As EventArgs) Handles btnTestIdMaestroDoc.Click
        Dim id = newDoc.obtenerIdMaestroDocumentoTitulo()
    End Sub
End Class