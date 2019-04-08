Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class ESTADO_PROCESO_GESTOR
    Inherits System.Web.UI.Page
    ''' <summary>
    ''' Load de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not Page.IsPostBack) Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
            dataBindGrid(New EstadoProcesoGestorBLL().ObtenerEstadoProcesoGestor)
            cargarDropEstados()
            If (Request.QueryString("Edit") = 1) Then
                ddlEstado.SelectedValue = Request.QueryString("Estado").ToString
                ddlEstado_SelectedIndexChanged(Nothing, Nothing)
                ddlEtapa.SelectedValue = Request.QueryString("Etapa").ToString
                txtUsuario.Text = Request.QueryString("User").ToString
                cbState.Checked = Convert.ToBoolean(Request.QueryString("Activo").ToString)
                mp2.Show()
            End If
        End If

    End Sub
    ''' <summary>
    ''' Carga el drop dowwn de estados
    ''' </summary>
    Private Sub cargarDropEstados()
        Dim dataEstado As New EstadosProcesoBLL()
        ddlEstado.DataSource = dataEstado.obtenerEstadosProcesos()
        ddlEstado.DataBind()
        ddlEstado.Items.Add(New ListItem("--Seleccione--", "0"))
        ddlEstado.SelectedValue = "0"
    End Sub

    ''' <summary>
    ''' Databind del gridview
    ''' </summary>
    Private Sub dataBindGrid(ByVal data As List(Of EstadoProcesoGestor))
        If data.Count() > 0 Then
            Dim etapaBll = New EtapaProcesalBLL()
            Dim EtapasEstado = etapaBll.ObtenerEtapaProcesal()
            For Each item As EstadoProcesoGestor In data
                If item.ETAPA_PROCESAL IsNot Nothing Then
                    item.NOMBRE_ETAPA = EtapasEstado.FirstOrDefault(Function(x) (x.ID_ETAPA_PROCESAL = item.ETAPA_PROCESAL)).VAL_ETAPA_PROCESAL.ToUpper()
                End If
            Next
            gwEstados.DataSource = data
        Else
            gwEstados.DataSource = Nothing
        End If
        gwEstados.DataBind()
    End Sub
    ''' <summary>
    ''' logout
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        Session.Clear()
        Session.RemoveAll()
        Response.Redirect("../../login.aspx")
    End Sub
    ''' <summary>
    ''' atras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ABackRep_Click(sender As Object, e As EventArgs) Handles ABackRep.Click
        Response.Redirect("../Modulos.aspx")
    End Sub

    '''
    ''' <summary>
    ''' Evento clic de buscar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim result As List(Of EstadoProcesoGestor)
        If (txtSearch.Text = String.Empty) Then
            result = New EstadoProcesoGestorBLL().ObtenerEstadoProcesoGestor()
        Else
            result = obtenerEstadosPorUsuario(txtSearch.Text)
        End If
        dataBindGrid(result)
    End Sub
    ''' <summary>
    ''' Obtiene los resultados pertenecientes a un usuario
    ''' </summary>
    ''' <param name="usuario"></param>
    ''' <returns></returns>
    Private Function obtenerEstadosPorUsuario(ByVal usuario As String) As List(Of EstadoProcesoGestor)
        Return New EstadoProcesoGestorBLL().ObtenerEstadoProcesoGestor().FindAll(Function(x) x.VAL_USUARIO.IndexOf(usuario, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList()
    End Function
    ''' <summary>
    ''' Evento adcionar el cual lanza el modal popup
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Clear()
        mp2.Show()
    End Sub
    ''' <summary>
    ''' Evento aceptar boton modal
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub cmdAceptar_Click(sender As Object, e As EventArgs)
        Dim estadoBll As New EstadoProcesoGestorBLL
        Dim dataEstado As New EstadoProcesoGestor
        dataEstado.COD_ID_ESTADOS_PROCESOS = ddlEstado.SelectedValue
        dataEstado.VAL_USUARIO = txtUsuario.Text.Trim
        dataEstado.ETAPA_PROCESAL = ddlEtapa.SelectedValue
        dataEstado.USUARIO_ACTUALIZACION = Session("ssloginusuario")
        dataEstado.FECHA_ACTUALIZACION = Date.Now
        dataEstado.IND_ESTADO = cbState.Checked
        If estadoBll.Guardar(dataEstado) Then
            mp2.Hide()
        End If
        Clear()
        dataBindGrid(estadoBll.ObtenerEstadoProcesoGestor())
        Response.Redirect("ESTADO_PROCESO_GESTOR.aspx")
    End Sub
    ''' <summary>
    ''' Limpia los campos del crud
    ''' </summary>
    Private Sub Clear()
        txtUsuario.Text = String.Empty
        ddlEstado.SelectedValue = "0"
    End Sub

    ''' <summary>
    ''' Evento row command de la grilla de datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gwEstados_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gwEstados.RowCommand
        Dim usuario As String
        Dim estado As String
        Dim etapa As String
        Dim activo As CheckBox
        Dim rowIndex As Integer = Integer.Parse(e.CommandArgument.ToString())
        usuario = gwEstados.Rows(e.CommandArgument).Cells(0).Text
        estado = gwEstados.DataKeys(rowIndex)(0)
        etapa = gwEstados.Rows(e.CommandArgument).Cells(2).Text
        activo = gwEstados.Rows(e.CommandArgument).Cells(5).FindControl("checkActivo")
        If e.CommandName = "edit" Then
            Server.Transfer("ESTADO_PROCESO_GESTOR.aspx?Edit=1&User=" & usuario & "&Estado=" & estado & "&Etapa=" & estado & "&Activo=" & activo.Checked.ToString) ' & "&Activo=" = cbState.Checked.ToString)
        End If
    End Sub

    Protected Sub ddlEstado_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim etapaBll = New EtapaProcesalBLL()
        Dim EtapasEstado = etapaBll.ObtenerEtapaProcesal().Where(Function(x) (x.codigo = ddlEstado.SelectedValue))
        If EtapasEstado IsNot Nothing And EtapasEstado.Count > 0 Then
            ddlEtapa.Enabled = True
            ddlEtapa.DataSource = etapaBll.ObtenerEtapaProcesal().Where(Function(x) (x.codigo = ddlEstado.SelectedValue))
            ddlEtapa.DataBind()
        Else
            ddlEtapa.Items.Clear()
            ddlEtapa.DataSource = Nothing
            ddlEtapa.DataBind()
            ddlEtapa.Enabled = False
        End If
    End Sub

    Protected Sub PaginadorGridView_EventActualizarGrid(sender As Object, e As EventArgs)
        btnSearch_Click(Nothing, Nothing)
    End Sub
End Class