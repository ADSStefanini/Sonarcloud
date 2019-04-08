Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class BandejaAprobacionReasignacionPriorizacion
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Se llenan los gestores dependiendo del gestor legeado
            'poblarGestores()
            'Se establecen los estados de la solicitud
            CommonsCobrosCoactivos.PoblarEstadoSolicitud(ddlEstadoSolicitud)
            'Se llena la grilla
            'BinGrid()
            'oculta checks de tareas ya gestionadas
            'ocultarChecksByEstadoSolicitud()
        End If
    End Sub

    ''' <summary>
    ''' Establece el tipo de solicitud
    ''' </summary>
    ''' <param name="prmIntTipoSolicitud">{8:SolicitudPriorizacion, 9:SolicitudResignacion}</param>
    Public Sub AsignarTipoSolicitud(ByVal prmIntTipoSolicitud As Int32)
        hdnTipoSolicitud.Value = prmIntTipoSolicitud
    End Sub

    ''' <summary>
    ''' Llena dropdown con los gestores relacionados al Revisor logeado (Coordinador - Superior de Superior)
    ''' </summary>
    Public Sub poblarGestores(Optional ByVal prmBoolTodosLosGestores As Boolean = False)
        If prmBoolTodosLosGestores Then
            CommonsCobrosCoactivos.poblarGestoresTodos(ddlGestor)
            Exit Sub
        End If

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

    ''' <summary>
    ''' Valida que el tipo de solicitud este definido
    ''' </summary>
    ''' <returns></returns>
    Protected Function ValidarTipoSolicitud() As Boolean
        If hdnTipoSolicitud.Value = "" OrElse String.IsNullOrEmpty(hdnTipoSolicitud.Value) Then
            lblRecordsFound.Text = "No se ha encontrado ningún registro para mostrar"
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Llena la bandeja con las solicitudes de reasignación realizadas al cordinador logeado
    ''' </summary>
    Public Sub BinGrid()

        If Not ValidarTipoSolicitud() Then
            Exit Sub
        End If

        'Se establece la jerarquía a validar
        Dim _tipoSolicitud = If(hdnTipoSolicitud.Value = "9", Enumeraciones.JerarquiaUsuario.Cordinador, Enumeraciones.JerarquiaUsuario.Superior)
        'Valida si el usuario tiene el nivel necesario para acceder
        If Not CommonsCobrosCoactivos.VerificaSiUsuarioPerteneceAJerarquia(Session("ssloginusuario"), _tipoSolicitud) Then
            lblSinPermisos.Text = My.Resources.txtSinPermisos
            lblSinPermisos.Visible = True
            OcultarElementos()
            Exit Sub
        End If

        Dim TipoSolicitud As Int32 = Convert.ToInt32(hdnTipoSolicitud.Value)
        Dim USULOG As String = If(TipoSolicitud <> 8, Session("ssloginusuario"), "")
        Dim NroExpediente As String = txtSearchEFINROEXP.Text
        Dim IdUnicoTitulo As String = txtNoTitulo.Text
        Dim LogUsuSolicitante As String = ddlGestor.SelectedValue
        Dim estadoSolicitud As Integer = If(ddlEstadoSolicitud.SelectedIndex = 0, 0, Convert.ToUInt32(ddlEstadoSolicitud.SelectedValue))

        Try
            Dim _bandejaBLL As New BandejaBLL()
            Dim _dataSource = _bandejaBLL.obtenerSolicitudesPorTipoSolicitud(USULOG, TipoSolicitud, NroExpediente, IdUnicoTitulo, LogUsuSolicitante, estadoSolicitud)
            If _dataSource.Rows.Count = 0 Then
                lblNoSolicitudes.Text = My.Resources.txtNoSolicitudes
                lblNoSolicitudes.Visible = True
                OcultarElementos()
                Exit Sub
            End If
            grdBandejaReasignacion.DataSource = _dataSource
            grdBandejaReasignacion.DataBind()
            lblRecordsFound.Text = _dataSource.Rows.Count() & " Registros Encontrados"
            ocultarChecksByEstadoSolicitud()
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Se ocultan elementos cuando no tiene permiso o cuando no tiene solicitudes asignadas
    ''' </summary>
    Private Sub OcultarElementos()
        grdBandejaReasignacion.Visible = False
        PaginadorGridView.Visible = False
        btnAprobar.Visible = False
    End Sub

    ''' <summary>
    ''' Oculta los checks de la bandeja para las solicitudes que ya fueron procesadas
    ''' </summary>
    Protected Sub ocultarChecksByEstadoSolicitud()
        For i As Integer = 0 To grdBandejaReasignacion.Rows.Count - 1
            Dim row As GridViewRow = grdBandejaReasignacion.Rows(i)
            Dim _codEstadoSolicitud = row.Cells(13).Text
            If _codEstadoSolicitud <> "16" Then
                Dim chkpriorizacion As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
                chkpriorizacion.Visible = False
            End If
        Next
    End Sub

    Protected Sub PaginadorGridView_EventActualizarGrid()
        BinGrid()
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        BinGrid()
    End Sub

    Protected Sub grdBandejaReasignacion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdBandejaReasignacion.RowCommand
        Dim codTareaSolicitud As String = grdBandejaReasignacion.Rows(e.CommandArgument).Cells(12).Text
        If e.CommandName = "cmdView" Then
            VerSolicitudUC.AsoganrIdTareaSolicitud(codTareaSolicitud)
            VerSolicitudUC.PoblarDatosTareaSolicitud()
            VerSolicitudUC.AbrirPopUp()
        End If
    End Sub

    Protected Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
        Dim _SolicitudesSeleccionadss As New List(Of Integer)
        For i As Integer = 0 To grdBandejaReasignacion.Rows.Count - 1
            Dim row As GridViewRow = grdBandejaReasignacion.Rows(i)
            Dim chkReasignacion As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
            If (chkReasignacion.Checked) Then
                Dim codTareaAsiganada As String = row.Cells(12).Text
                _SolicitudesSeleccionadss.Add(Convert.ToInt32(codTareaAsiganada))
            End If
        Next
        ResSolicitudUC.OcultarMensajeError()
        ResSolicitudUC.AsignarTareasSolicitud(_SolicitudesSeleccionadss)
        ResSolicitudUC.AsignarTipoSolicitud(hdnTipoSolicitud.Value)
        ResSolicitudUC.HabilitarFormulario()
        ResSolicitudUC.AbrirModal()
    End Sub
End Class