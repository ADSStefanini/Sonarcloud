Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class AdminAccesoPerfilesModulos1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            ddlPerfilesValidator.Text = My.Resources.Formularios.errorCampoRequerido
            CommonsCobrosCoactivos.poblarPerfiles(ddlPerfiles)
            bindGrid()
            desabledAllGrid()
            setAllWrong()
        End If
    End Sub

    Private Sub bindGrid()
        Dim moduloBLL As New ModuloBLL()
        gvwModulosAcceso.DataSource = moduloBLL.obtenerModulosActivos()
        gvwModulosAcceso.DataBind()
    End Sub

    Private Sub updatedGridByPerfil()
        setAllWrong()
        Dim moduloBLL As New ModuloBLL()
        Dim intPefilId As Int32 = CType(ddlPerfiles.SelectedValue, Int32)
        Dim modulos As List(Of Entidades.Modulo) = moduloBLL.obtenerModulosPorPerfil(intPefilId)
        txtIdPerfil.Text = intPefilId.ToString()
        gvwModulosAcceso.Enabled = True

        Dim procesados As Int32 = 0
        If modulos.Count() > 0 Then
            For i As Integer = 0 To gvwModulosAcceso.Rows.Count - 1
                Dim row As GridViewRow = gvwModulosAcceso.Rows(i)
                Dim moduloId As String = row.Cells(0).Text
                Dim m As Entidades.Modulo = modulos.Find(Function(c) c.pk_codigo = moduloId)

                If m Is Nothing Then
                    Continue For
                End If

                procesados += 1

                If (procesados > modulos.Count()) Then
                    Exit For
                End If

                Dim imgCtrl As Image = CType(row.FindControl("imgEstadoAccesoModulo"), Image)
                Dim btnCtrl As Button = CType(row.Cells(3).Controls(0), Button)

                imgCtrl.ImageUrl = My.Resources.bandejas.urlImgCheck
                btnCtrl.Text = My.Resources.bandejas.txtDenegar
                row.Cells(4).Text = "1"
            Next
        Else
            setAllWrong()
        End If
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        If (Page.IsValid) Then
            bindGrid()
            updatedGridByPerfil()
        Else
            setAllWrong()
            desabledAllGrid()
            txtIdPerfil.Text = "0"
        End If
    End Sub

    Private Sub desabledAllGrid()
        gvwModulosAcceso.Enabled = False
    End Sub

    Private Sub setAllWrong()
        For i As Integer = 0 To gvwModulosAcceso.Rows.Count - 1
            Dim imgCtrl As Image = CType(gvwModulosAcceso.Rows(i).FindControl("imgEstadoAccesoModulo"), Image)
            Dim btnCtrl As Button = CType(gvwModulosAcceso.Rows(i).Cells(3).Controls(0), Button)

            imgCtrl.ImageUrl = My.Resources.bandejas.urlImgAlerta
            btnCtrl.Text = My.Resources.bandejas.txtAutorizar
            gvwModulosAcceso.Rows(i).Cells(4).Text = "0"
        Next
    End Sub

    Protected Sub gvwModulosAcceso_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvwModulosAcceso.RowCommand
        If e.CommandName = "cmdUpdateAccess" Then
            If txtIdPerfil.Text.Equals("0") Then
                Exit Sub
            End If

            Dim estado As String = gvwModulosAcceso.Rows(e.CommandArgument).Cells(4).Text
            estado = If(String.IsNullOrEmpty(estado), "0", estado)


            Dim moduloBLL As ModuloBLL = New ModuloBLL()
            Dim intPerfilId As Int32 = CType(ddlPerfiles.SelectedValue, Int32)
            Dim intModuloId As Int32 = CType(gvwModulosAcceso.Rows(e.CommandArgument).Cells(0).Text, Int32)
            Dim boolEstado As Boolean = CType(estado, Boolean)

            boolEstado = Not boolEstado
            Try
                moduloBLL = New ModuloBLL(llenarAuditoria("fk_perfil_id=" & intPerfilId.ToString() & ",fk_modulo_id=" & intModuloId.ToString() & ",ind_estado=" & boolEstado.ToString()))
                moduloBLL.actualizarAccesoModulo(intPerfilId, intModuloId, boolEstado)
                updatedGridByPerfil()
            Catch ex As Exception
                'TODO: mensaje de error al no lograr guardar y registro en log
            End Try

        End If
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