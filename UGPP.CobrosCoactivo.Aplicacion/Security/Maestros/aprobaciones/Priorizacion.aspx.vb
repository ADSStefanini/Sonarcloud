Imports UGPP.CobrosCoactivo.Entidades

Public Class Priorizacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            BandejaPriorizacion.AsignarTipoSolicitud(Enumeraciones.DominioDetalle.SolicitudPriorizacion)
            BandejaPriorizacion.poblarGestores(True)
            BandejaPriorizacion.BinGrid()
        End If
    End Sub

    Protected Sub ABack_Click(sender As Object, e As EventArgs) Handles ABack.Click
        Response.Redirect("~/Security/Modulos.aspx", True)
    End Sub
End Class