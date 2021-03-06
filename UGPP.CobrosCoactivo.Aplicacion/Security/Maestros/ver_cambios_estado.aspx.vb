﻿Imports System.Data
Imports System.Data.SqlClient

Partial Public Class ver_cambios_estado
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            BindGridCambioEstado()

            'Instanciar clase de mensajes y estadisticas
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)

        End If
    End Sub


    Private Sub BindGridCambioEstado()

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT CAMBIOS_ESTADO.*, USUARIOSrepartidor.nombre as USUARIOSrepartidornombre," &
             "USUARIOSabogado.nombre as USUARIOSabogadonombre, ESTADOS_PROCESOestado.nombre as ESTADOS_PROCESOestadonombre," &
             "ESTADOS_PAGOestadopago.nombre as ESTADOS_PAGOestadopagonombre " &
             "FROM CAMBIOS_ESTADO " &
              "left join USUARIOS USUARIOSrepartidor on CAMBIOS_ESTADO.repartidor = USUARIOSrepartidor.codigo " &
              "left join USUARIOS USUARIOSabogado on CAMBIOS_ESTADO.abogado = USUARIOSabogado.codigo " &
              "left join ESTADOS_PROCESO ESTADOS_PROCESOestado on CAMBIOS_ESTADO.estado = ESTADOS_PROCESOestado.codigo " &
              "left join ESTADOS_PAGO ESTADOS_PAGOestadopago on CAMBIOS_ESTADO.estadopago = ESTADOS_PAGOestadopago.codigo " &
             "WHERE CAMBIOS_ESTADO.NroExp = '" & Request("pExpediente") & "'"
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grdCambiosEstado.DataSource = Command.ExecuteReader()
        grdCambiosEstado.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grdCambiosEstado.Rows.Count
        Connection.Close()
    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("PAGOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()

        Response.Redirect("../../login.aspx")
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        Session.RemoveAll()
    End Sub

End Class