Imports UGPP.CobrosCoactivo.Logica

Public Class perfiles
    Inherits System.Web.UI.Page

    'Protected paginadorControl As paginador

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If Len(Request("s")) > 0 Then
                txtSearchPerfiles.Text = Request("s")
            End If
            BindGrid(txtSearchPerfiles.Text)
            'Dim i As Int32 = gvwPerfiles.PageCount
            'paginadorView.setGridView(gvwPerfiles)
            'paginadorView.UpdateLabels()
        End If
    End Sub

    Protected Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        Response.Redirect(My.Resources.Formularios.urlAddPerfiles, True)
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Response.Redirect(My.Resources.Formularios.urlPerfiles & "?s=" & txtSearchPerfiles.Text, True)
    End Sub

    Public Sub BindGrid(ByVal prmStrTextoBusqueda As String)
        Dim perfilBLL As PerfilBLL = New PerfilBLL()
        Dim perfilesGW = perfilBLL.obtenerPerfilesToodos(prmStrTextoBusqueda)
        gvwPerfiles.DataSource = perfilesGW
        gvwPerfiles.DataBind()
    End Sub

    Protected Sub gvwPerfiles_PageIndexChanged(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.gvwPerfiles.PageIndex = e.NewPageIndex
        BindGrid(txtSearchPerfiles.Text)
    End Sub

    Protected Sub gvwPerfiles_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvwPerfiles.RowCommand
        If e.CommandName = "" Then
            Dim idPerfil As String = gvwPerfiles.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("~/Security/admin/perfiles/editPerfil.aspx?idPerfil=" & idPerfil)
        End If
    End Sub
End Class