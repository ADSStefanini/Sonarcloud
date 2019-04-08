Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica

Public Class SeleccionModulo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Page.Title = My.Resources.modulos.DefaultTitle
            lblPageTitle.Text = My.Resources.modulos.DefaultTitle
        End If
    End Sub

End Class