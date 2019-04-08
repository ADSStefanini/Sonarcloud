Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class adminAccesoPerfiles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            'Dim paginaBLL As PaginaBLL = New PaginaBLL()
            'Dim paginas As List(Of Pagina) = paginaBLL.obtenerPaginasOrdenadas(Convert.ToInt32(Session("mnivelacces")), 0)
        End If
    End Sub

End Class