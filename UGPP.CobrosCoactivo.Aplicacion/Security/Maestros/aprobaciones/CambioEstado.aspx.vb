Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class CambioEstado
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If CommonsCobrosCoactivos.VerificaSiUsuarioPerteneceAJerarquia(Session("ssloginusuario"), Enumeraciones.JerarquiaUsuario.Revisor) Then
                poblarGestores()
                BinGrid()
            Else
                'Se muestra mensaje de no permiso
                lblSinPermisos.Text = My.Resources.txtSinPermisos
                lblSinPermisos.Visible = True
                OcultarElementos()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se ocultan elementos cuando no tiene permiso o cuando no tiene solicitudes asignadas
    ''' </summary>
    Private Sub OcultarElementos()
        grdBandejaAprobacion.Visible = False
        PaginadorGridView.Visible = False
        btnAprobar.Visible = False
    End Sub

    ''' <summary>
    ''' Llena dropdown con los gestores relacionados al Revisor logeado (Coordinador - Superior de Superior)
    ''' </summary>
    Public Sub poblarGestores()
        Dim _ususarioBLL As New UsuariosBLL()
        Dim usuarios = _ususarioBLL.obtenerUsuariosPorSuperiorDelSuperior(Session("sscodigousuario"))
        If usuarios.Count() > 0 Then
            For Each usuario As Entidades.Usuario In usuarios
                ddlGestor.Items.Add(New ListItem(usuario.nombre, usuario.login))
            Next
        Else
            ddlGestor.Enabled = False
        End If
    End Sub

    Public Sub BinGrid()
        Dim USULOG As String = Session("ssloginusuario")
        Dim NroExpediente As String = txtSearchEFINROEXP.Text
        Dim gestorSolicitud As String = If(ddlGestor.SelectedIndex = 0, "", ddlGestor.SelectedValue)

        Try
            Dim _bandejaBLL As New BandejaBLL()

            Dim _dataSource = _bandejaBLL.ObtenerSolicitudesCambioEstado(USULOG, NroExpediente, gestorSolicitud)

            If _dataSource.Rows.Count = 0 Then
                lblNoSolicitudes.Text = My.Resources.txtNoSolicitudes
                lblNoSolicitudes.Visible = True
                OcultarElementos()
                Exit Sub
            End If
            grdBandejaAprobacion.DataSource = _dataSource
            grdBandejaAprobacion.DataBind()

            lblRecordsFound.Text = _dataSource.Rows.Count() & " Registros Encontrados"
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub PaginadorGridView_EventActualizarGrid()
        BinGrid()
    End Sub

    Protected Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
        procesarSolicitud()
    End Sub

    Protected Sub procesarSolicitud()
        Dim _SolicitudesSeleccionadas As New List(Of Integer)
        For i As Integer = 0 To grdBandejaAprobacion.Rows.Count - 1
            Dim row As GridViewRow = grdBandejaAprobacion.Rows(i)
            Dim chkReasignacion As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
            If (chkReasignacion.Checked) Then
                Dim codSolicitudAprobacion As String = row.Cells(9).Text
                _SolicitudesSeleccionadas.Add(Convert.ToInt32(codSolicitudAprobacion))
            End If
        Next

        If _SolicitudesSeleccionadas.Count = 0 Then
            Exit Sub
        End If

        ResSolicitudUC.AsignarTareasSolicitud(_SolicitudesSeleccionadas)
        ResSolicitudUC.AsignarTipoSolicitud(Enumeraciones.DominioDetalle.SolicitudCambioEstado)
        ResSolicitudUC.HabilitarFormulario()
        ResSolicitudUC.AbrirModal()
    End Sub

    Protected Sub grdBandejaAprobacion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdBandejaAprobacion.RowCommand
        If e.CommandName = "cmdView" Then
            Dim idSolicitud = grdBandejaAprobacion.Rows(e.CommandArgument).Cells(9).Text
            VerSolicitudUC.AsoganrIdTareaSolicitud(Convert.ToInt32(idSolicitud))
            VerSolicitudUC.PoblarDatosSolicitudReasignacion(Convert.ToInt32(idSolicitud))
            VerSolicitudUC.AbrirPopUp()
        End If
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        BinGrid()
    End Sub

    Protected Sub ABack_Click(sender As Object, e As EventArgs) Handles ABack.Click
        Response.Redirect("~/Security/Modulos.aspx", True)
    End Sub
End Class