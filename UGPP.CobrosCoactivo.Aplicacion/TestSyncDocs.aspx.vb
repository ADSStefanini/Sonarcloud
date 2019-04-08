Public Class TestSyncDocs
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim _expedientesUtils As ExpedientesUtils
            _expedientesUtils = New ExpedientesUtils(15691, "UnitTest")
            _expedientesUtils.urlBase = Request.Url.Scheme + "://" + Request.Url.Authority & Request.ApplicationPath.TrimEnd("/")
            _expedientesUtils.basePath = Server.MapPath("~/")
            _expedientesUtils.SincronizarDocumentosTitulos()
        End If
    End Sub

End Class