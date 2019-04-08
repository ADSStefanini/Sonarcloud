Imports UGPP.CobrosCoactivo.Logica

Public Class modulos_list
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Len(Request("s")) > 0 Then
                txtSearchModulos.Text = Request("s")
            End If
            BindGrid(txtSearchModulos.Text)
        End If
        'Dim i As Int32 = gvwModulos.PageCount
        'paginadorView.setGridView(gvwModulos)
        'paginadorView.UpdateLabels()
    End Sub

    Public Sub BindGrid(ByVal prmStrTextoBusqueda As String)
        Dim ModuloBLL As ModuloBLL = New ModuloBLL()
        gvwModulos.DataSource = ModuloBLL.obtenerModulos(prmStrTextoBusqueda)
        gvwModulos.DataBind()
    End Sub

    Protected Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        Response.Redirect(My.Resources.Formularios.urlAddModulo, True)
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Response.Redirect(My.Resources.Formularios.urlModulos & "?s=" & txtSearchModulos.Text, True)
    End Sub

    Protected Sub gvwModulos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvwModulos.RowCommand
        If e.CommandName = "edit" Then
            Dim idModulo As String = gvwModulos.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect(My.Resources.Formularios.urlEditModulo & idModulo)
        End If
    End Sub
End Class