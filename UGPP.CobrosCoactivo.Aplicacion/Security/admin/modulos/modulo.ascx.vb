Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades

Public Class modulo
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            reqNombreModulo.Text = My.Resources.Formularios.errorCampoRequerido
            reqUrlModulo.Text = My.Resources.Formularios.errorCampoRequerido
            reqrdoEstado.Text = My.Resources.Formularios.errorCampoRequerido

            If Len(Request("idModulo")) > 0 Then
                'Verifica que sea un ID númerico
                If CommonsCobrosCoactivos.checkIIsNumber(Request("idModulo")) = False Then
                    Response.Redirect(My.Resources.Formularios.urlPerfiles, True)
                End If

                'Cargar datos del formulario a partir de la variable GET
                Dim moduloBLL As ModuloBLL = New ModuloBLL()
                Dim modulo As Entidades.Modulo = moduloBLL.obtenerModuloPorId(Convert.ToInt32(Request("idModulo")))

                If modulo.pk_codigo = 0 Then
                    Response.Redirect(My.Resources.Formularios.urlModulos, True)
                End If

                txtModuloId.Text = modulo.pk_codigo
                txtNombreModulo.Text = modulo.val_nombre
                txtUrlModulo.Text = modulo.val_url
                txtUrlIconoModulo.Text = modulo.val_url_icono

                rdoEstado.SelectedValue = Convert.ToInt32(modulo.ind_estado).ToString()
                cmdSave.Text = My.Resources.Formularios.TextoActualizar
            Else
                cmdSave.Text = My.Resources.Formularios.TextoGuardar
            End If
        End If
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        If Page.IsValid Then
            'Se capturan los datos del formulario
            Dim nombreModulo As String = txtNombreModulo.Text
            Dim urlModulo As String = txtUrlModulo.Text
            Dim urlIconoModulo As String = txtUrlIconoModulo.Text
            Dim estado As Boolean = Convert.ToBoolean(Convert.ToInt32(rdoEstado.SelectedItem.Value))

            Dim moduloBLL As ModuloBLL = New ModuloBLL()
            Dim moduloRes As Entidades.Modulo = New Entidades.Modulo()

            Try
                If Len(Request("idModulo")) > 0 Then
                    moduloBLL = New ModuloBLL(llenarAuditoria("codigo=" & Request("idModulo").ToString() & ",nombre=" & nombreModulo + ",url=" + urlModulo + ",urlicono=" + urlIconoModulo + ",estado=" + estado.ToString()))
                    moduloRes = moduloBLL.actualizarModulo(Convert.ToInt32(Request("idModulo")), nombreModulo, urlModulo, urlIconoModulo, estado)
                Else
                    moduloBLL = New ModuloBLL(llenarAuditoria("nombre=" & nombreModulo & ",url=" & urlModulo & ",urlicono=" & urlIconoModulo & ",estado=" + estado.ToString()))
                    moduloRes = moduloBLL.guardarModulo(nombreModulo, urlModulo, urlIconoModulo, estado)
                End If

                Response.Redirect(My.Resources.Formularios.urlModulos, True)
            Catch
                cValErrorAlGuardar.Text = My.Resources.Formularios.errorAlGuardarDatos
                cValErrorAlGuardar.Attributes.Add("class", "invalid")
            End Try

        End If
    End Sub

    Protected Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Response.Redirect(My.Resources.Formularios.urlModulos, True)
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