Imports UGPP.CobrosCoactivo.Logica

Public Class paginas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Len(Request("s")) > 0 Then
                txtSearchPages.Text = Request("s")
            End If
            BindGrid(txtSearchPages.Text)
            Dim i As Int32 = gwPaginas.PageCount
            'paginadorView.setGridView(gwPaginas)
            'paginadorView.UpdateLabels(gwPaginas)
        End If
    End Sub

    Public Sub BindGrid(ByVal prmStrTextoBusqueda As String)
        Dim paginaBLL As PaginaBLL = New PaginaBLL()
        Dim paginasGw = paginaBLL.obtenerPaginasTodas(prmStrTextoBusqueda)
        gwPaginas.DataSource = paginasGw
        gwPaginas.DataBind()
    End Sub

    Protected Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        Response.Redirect(My.Resources.Formularios.urlAddPagina, True)
    End Sub

    Protected Sub gwPaginas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gwPaginas.RowCommand
        If e.CommandName = "edit" Then
            Dim idPagina As String = gwPaginas.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect(My.Resources.Formularios.urlEditPagina & idPagina)
        End If
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Dim url As String = Request.Url.AbsolutePath
        If Not String.IsNullOrEmpty(txtSearchPages.Text) Then
            url = url & "?s=" & txtSearchPages.Text
        End If
        Response.Redirect(url)
    End Sub
End Class