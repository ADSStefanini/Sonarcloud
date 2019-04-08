Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo

Public Class AdminAccesoPerfilesPaginas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            CommonsCobrosCoactivos.poblarPerfiles(ddlPerfiles)
            bindGrid()
            setAllWrong()

            gvwPaginasAcceso.Enabled = False
        End If
    End Sub

    Private Sub bindGrid()
        Dim paginaBLL As New PaginaBLL()
        Dim paginas As List(Of Pagina) = paginaBLL.obtenerPaginasOrdenadas()
        gvwPaginasAcceso.DataSource = paginas
        gvwPaginasAcceso.DataBind()
    End Sub

    Private Sub setAllWrong()
        For i As Integer = 0 To gvwPaginasAcceso.Rows.Count - 1

            Dim imgCtrlVer As Image = CType(gvwPaginasAcceso.Rows(i).FindControl("imgEstadoPuedeVer"), Image)
            Dim imgCtrlEditar As Image = CType(gvwPaginasAcceso.Rows(i).FindControl("imgEstadoPuedeEditar"), Image)

            Dim btnCtrlVer As Button = CType(gvwPaginasAcceso.Rows(i).Cells(5).Controls(0), Button)
            Dim btnCtrlEditar As Button = CType(gvwPaginasAcceso.Rows(i).Cells(6).Controls(0), Button)

            imgCtrlVer.ImageUrl = My.Resources.bandejas.urlImgAlerta
            imgCtrlEditar.ImageUrl = My.Resources.bandejas.urlImgAlerta

            btnCtrlVer.Text = My.Resources.bandejas.txtAutorizar
            btnCtrlEditar.Text = My.Resources.bandejas.txtAutorizar

            gvwPaginasAcceso.Rows(i).Cells(7).Text = "0"
            gvwPaginasAcceso.Rows(i).Cells(8).Text = "0"
        Next
    End Sub

    Protected Sub gvwPaginasAcceso_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvwPaginasAcceso.RowCommand
        If txtIdPerfil.Text.Equals("0") Then
            Exit Sub
        End If

        Dim paginaBLL As New PaginaBLL()
        Dim intPerfilId As Int32 = CType(ddlPerfiles.SelectedValue, Int32)
        Dim intPaginaId As Int32 = CType(gvwPaginasAcceso.Rows(e.CommandArgument).Cells(0).Text, Int32)

        Dim boolPuedeVerText = gvwPaginasAcceso.Rows(e.CommandArgument).Cells(7).Text
        boolPuedeVerText = If(String.IsNullOrEmpty(boolPuedeVerText), "0", boolPuedeVerText)
        Dim boolPuedeVer As Boolean = False
        If (IsNumeric(boolPuedeVerText)) Then
            boolPuedeVer = Convert.ToBoolean(Convert.ToInt32(boolPuedeVerText))
        End If

        If e.CommandName = "cmdUpdateVer" Then
            boolPuedeVer = Not boolPuedeVer
        End If

        Dim boolPuedeEditarText = gvwPaginasAcceso.Rows(e.CommandArgument).Cells(8).Text
        boolPuedeEditarText = If(String.IsNullOrEmpty(boolPuedeEditarText), "0", boolPuedeEditarText)
        Dim boolPuedeEditar As Boolean = False
        If (IsNumeric(boolPuedeEditarText)) Then
            boolPuedeEditar = Convert.ToBoolean(Convert.ToInt32(boolPuedeEditarText))
        End If

        If e.CommandName = "cmdUpdateEditar" Then
            boolPuedeEditar = Not boolPuedeEditar
        End If

        Try
            paginaBLL = New PaginaBLL(llenarAuditoria("perfilid=" & intPerfilId.ToString() & ",paginaid=" & intPaginaId.ToString() & ",puedever=" + boolPuedeVer.ToString() + ",puedeeditar=" + boolPuedeEditar.ToString()))
            paginaBLL.actualizarAccesoPagina(intPerfilId, intPaginaId, boolPuedeVer, boolPuedeEditar)
            updatedGridByPerfil()
        Catch ex As Exception
            'TODO: mensaje de error al no lograr guardar y registro en log
        End Try

    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        bindGrid()
        updatedGridByPerfil()
    End Sub

    Private Sub updatedGridByPerfil()
        setAllWrong()
        Dim paginaBLL As New PaginaBLL()

        Dim intPefilId As Int32 = CType(ddlPerfiles.SelectedValue, Int32)
        Dim paginasPuedeVer As List(Of Entidades.Pagina) = paginaBLL.obtenerPaginasPorPerfilPuedeVer(intPefilId)
        Dim paginasPuedeEditar As List(Of Entidades.Pagina) = paginaBLL.obtenerPaginasPorPerfilPuedeEditar(intPefilId)
        txtIdPerfil.Text = intPefilId.ToString()
        gvwPaginasAcceso.Enabled = True

        For i As Integer = 0 To gvwPaginasAcceso.Rows.Count - 1
            Dim row As GridViewRow = gvwPaginasAcceso.Rows(i)
            Dim paginaId As String = row.Cells(0).Text
            Dim pagPuedeVer As Entidades.Pagina = paginasPuedeVer.Find(Function(c) c.pk_codigo = CType(paginaId, Int32))
            Dim pagPuedeEditar As Entidades.Pagina = paginasPuedeEditar.Find(Function(c) c.pk_codigo = CType(paginaId, Int32))

            If Not pagPuedeVer Is Nothing Then
                Dim imgCtrlVer As Image = CType(row.FindControl("imgEstadoPuedeVer"), Image)
                Dim btnCtrlVer As Button = CType(row.Cells(5).Controls(0), Button)
                imgCtrlVer.ImageUrl = My.Resources.bandejas.urlImgCheck
                btnCtrlVer.Text = My.Resources.bandejas.txtDenegar
                gvwPaginasAcceso.Rows(i).Cells(7).Text = "1"
            End If

            If Not pagPuedeEditar Is Nothing Then
                Dim imgCtrlEditar As Image = CType(row.FindControl("imgEstadoPuedeEditar"), Image)
                Dim btnCtrlEditar As Button = CType(row.Cells(6).Controls(0), Button)
                imgCtrlEditar.ImageUrl = My.Resources.bandejas.urlImgCheck
                btnCtrlEditar.Text = My.Resources.bandejas.txtDenegar
                gvwPaginasAcceso.Rows(i).Cells(8).Text = "1"
            End If

        Next
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