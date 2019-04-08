Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class paginas1
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Mensajes de validación
            reqNombrePagina.Text = My.Resources.Formularios.errorCampoRequerido
            reqrdoEstado.Text = My.Resources.Formularios.errorSinOpcionSeleccionada

            Dim paginaBLL As PaginaBLL = New PaginaBLL()

            'Poblado de dropdown de páginas padres
            ddlPadre.Items.Add(New ListItem("", 0))
            For Each opcionPagina As UGPP.CobrosCoactivo.Entidades.Pagina In paginaBLL.obtenerPaginasActivas()
                ddlPadre.Items.Add(New ListItem(opcionPagina.val_nombre, opcionPagina.pk_codigo))
            Next

            'Poblado de datos de páginas padre
            If Len(Request("idPagina")) > 0 Then
                'Verifica que sea un ID númerico
                If CommonsCobrosCoactivos.checkIIsNumber(Request("idPagina")) = False Then
                    Response.Redirect(My.Resources.Formularios.urlPaginas, True)
                End If

                'Eliminar la página que se esta editando de las páginas de opciones de la página padre
                Dim item As ListItem = ddlPadre.Items.FindByValue(Request("idPagina"))
                If (IsNothing(item) = False) Then
                    ddlPadre.Items.Remove(item)
                End If

                'Cargar datos del formulario a partir de la variable GET
                Dim pagina As UGPP.CobrosCoactivo.Entidades.Pagina = paginaBLL.obtenerPaginaPorId(Convert.ToInt32(Request("idPagina")))

                If (pagina.pk_codigo = 0) Then
                    Response.Redirect(My.Resources.Formularios.urlPaginas, True)
                End If

                txtNombrePagina.Text = pagina.val_nombre
                txtUrlPagina.Text = pagina.val_url
                If (pagina.fk_padre <> 0) Then
                    ddlPadre.Items.FindByValue(pagina.fk_padre).Selected = True
                End If

                rdoEstado.SelectedValue = Convert.ToInt32(pagina.ind_estado).ToString()
                rdoPaginaInterna.SelectedValue = Convert.ToInt32(pagina.ind_pagina_interna).ToString()
                cmdSave.Text = My.Resources.Formularios.TextoActualizar
            Else
                cmdSave.Text = My.Resources.Formularios.TextoGuardar
            End If

        End If
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        If Page.IsValid Then

            Dim res As Boolean = True
            Dim paginaBLL = New PaginaBLL()

            Dim nombrePagina As String = txtNombrePagina.Text
            Dim urlPagina As String = txtUrlPagina.Text
            Dim padre As Int32 = ddlPadre.SelectedItem.Value
            Dim estado As Boolean = Convert.ToBoolean(Convert.ToInt32(rdoEstado.SelectedItem.Value))
            Dim pagina As New Entidades.Pagina
            If Len(Request("idPagina")) > 0 Then
                paginaBLL = New PaginaBLL(llenarAuditoria("codigo=" & Request("idPagina").ToString() & ",nombre=" & nombrePagina & ",url" & ",padre=" & padre.ToString() & ",estado=" & estado.ToString()))
                pagina = paginaBLL.actualizarPerfil(Convert.ToInt32(Request("idPagina")), nombrePagina, urlPagina, padre, estado)
            Else
                paginaBLL = New PaginaBLL(llenarAuditoria("nombre=" & nombrePagina & ",url" & ",padre=" & padre.ToString() & ",estado=" & estado.ToString()))
                pagina = paginaBLL.guardarPagina(nombrePagina, urlPagina, padre, estado)
            End If

            If (pagina.pk_codigo = 0) Then
                res = False
            End If


            If res Then
                Response.Redirect(My.Resources.Formularios.urlPaginas, True)
            End If

            cValErrorAlGuardar.Text = My.Resources.Formularios.errorAlGuardarDatos
            cValErrorAlGuardar.Attributes.Add("class", "invalid")
        End If
    End Sub

    Protected Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Response.Redirect(My.Resources.Formularios.urlPaginas, True)
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