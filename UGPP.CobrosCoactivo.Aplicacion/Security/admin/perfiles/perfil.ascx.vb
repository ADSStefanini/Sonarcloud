Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class perfil
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            reqNombrePerfil.Text = My.Resources.Formularios.errorCampoRequerido
            reqGrupoLdap.Text = My.Resources.Formularios.errorCampoRequerido
            reqrdoEstado.Text = My.Resources.Formularios.errorSinOpcionSeleccionada

            If Len(Request("idPerfil")) > 0 Then
                'Verifica que sea un ID númerico
                If CommonsCobrosCoactivos.checkIIsNumber(Request("idPerfil")) = False Then
                    Response.Redirect(My.Resources.Formularios.urlPerfiles, True)
                End If

                'Cargar datos del formulario a partir de la variable GET
                Dim perfilBLL As PerfilBLL = New PerfilBLL()
                Dim perfil As UGPP.CobrosCoactivo.Entidades.Perfiles = perfilBLL.obternetPerfilPorId(Convert.ToInt32(Request("idPerfil")))

                If perfil.codigo = 0 Then
                    Response.Redirect(My.Resources.Formularios.urlPerfiles, True)
                End If

                txtNombrePerfil.Text = perfil.nombre_perfil
                txtGrupoLdap.Text = perfil.val_ldap_group

                rdoEstado.SelectedValue = Convert.ToInt32(perfil.ind_estado).ToString()
                cmdSave.Text = My.Resources.Formularios.TextoActualizar
            Else
                cmdSave.Text = My.Resources.Formularios.TextoGuardar
            End If
        End If
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        If Page.IsValid Then
            'Se capturan los datos del formulario
            Dim nombrePerfil As String = txtNombrePerfil.Text
            Dim grupoLdap As String = txtGrupoLdap.Text
            Dim estado As Boolean = Convert.ToBoolean(Convert.ToInt32(rdoEstado.SelectedItem.Value))

            Dim perfilBLL As PerfilBLL = New PerfilBLL()
            Dim res As Boolean = True

            If Len(Request("idPerfil")) > 0 Then
                perfilBLL = New PerfilBLL(llenarAuditoria("codigo=" + Request("idPerfil").ToString() + ",nombre=" + nombrePerfil + ",ldapgroup=" + grupoLdap + ",estado=" + estado.ToString()))
                res = perfilBLL.actualizarPerfil(Convert.ToInt32(Request("idPerfil")), nombrePerfil, grupoLdap, estado)
            Else
                perfilBLL = New PerfilBLL(llenarAuditoria("nombre=" + nombrePerfil + ",ldapgroup=" + grupoLdap + ",estado=" + estado.ToString()))
                res = perfilBLL.guardarPerfil(nombrePerfil, grupoLdap, estado)
            End If

            If res Then
                Response.Redirect(My.Resources.Formularios.urlPerfiles, True)
            End If

            cValErrorAlGuardar.Text = My.Resources.Formularios.errorAlGuardarDatos
            cValErrorAlGuardar.Attributes.Add("class", "invalid")
        End If
    End Sub

    Protected Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Response.Redirect(My.Resources.Formularios.urlPerfiles, True)
    End Sub

    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "Seguridad"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function
End Class