Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class BandejaEstudioTitulos
    Inherits System.Web.UI.UserControl

    Private PageSize As Long = 10
    Private titulosSeleccionados As List(Of Integer)

    Public Property esEstudioTitulos As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Actualización de la prioridad del título
            Dim _bandejaBLL As BandejaBLL = New BandejaBLL(llenarAuditoria("usuarionombre=" + Session("ssloginusuario").ToString()))
            Try
                _bandejaBLL.actualizarPriorizacionTitulo(Session("ssloginusuario"))
            Catch ex As Exception

            End Try

            'Poblado de datos
            CommonsCobrosCoactivos.poblarEstadosOperativosTitulos(ddlEstadoOperativo)
            'Poblar GridView
            PintarGridBandejaTitulosEstudioTitulos()
            'Poblado de datos para gestores de estudio de títulos que pueden ser solicitados en la solicitud de reasignación
            SolicitudReasignacionPanel.poblarGestorSolicitadoParaReasignacion(1)
        End If
    End Sub

    ''' <summary>
    ''' Metodo que ejecuta el llenado y pinta el GridView de la bandeja de titulos
    ''' </summary>
    Protected Sub PintarGridBandejaTitulosEstudioTitulos()
        'Captura de filtros para la consulta
        Dim USULOG As String = Session("ssloginusuario")
        Dim NROTITULO As String = txtNoTitulo.Text
        Dim ESTADOPROCESAL As Int32 = Nothing 'ddlEstadoProcesal.SelectedValue
        Dim ESTADOSOPERATIVO As Int32 = ddlEstadoOperativo.SelectedValue
        Dim FCHENVIOCOBRANZADESDE As String = fecFechaEnvioTituloInicio.Text
        Dim FCHENVIOCOBRANZAHASTA As String = fecFechaEnvioTituloFin.Text
        Dim NROIDENTIFICACIONDEUDOR As String = txtNumIdentificacionDeudor.Text
        Dim NOMBREDEUDOR As String = txtNombreDeudor.Text
        'Inicialización de la instancia
        'Valor de la variable tomada a partir del tag del control
        If esEstudioTitulos = "1" Then
            noTitulosAsignados.Visible = False
            PaginadorGridView.Visible = True
            Dim bandejaBLL As New BandejaBLL()
            'Si la lista de títulos es para estudio de títulos se filtra de forma diferente que para área origen
            grdBandejaTituloAO.DataSource = bandejaBLL.obtenerTitulosEstudioTitulos(USULOG, NROTITULO, ESTADOPROCESAL, ESTADOSOPERATIVO, FCHENVIOCOBRANZADESDE, FCHENVIOCOBRANZAHASTA, NROIDENTIFICACIONDEUDOR, NOMBREDEUDOR)
            grdBandejaTituloAO.DataBind()
            If grdBandejaTituloAO.Rows.Count = 0 Then
                noTitulosAsignados.Visible = True
                txtSinExpedientesAsignados.Text = "Actualmente no cuenta con títulos asignados" 'My.Resources.bandejas.txtSinTitulosAsignados
                btnSolicitarReasignacion.Visible = False
                PaginadorGridView.Visible = False
            Else
                btnSolicitarReasignacion.Visible = True
                DeshabilitarBotones()
            End If
        End If
        PaginadorGridView.UpdateLabels()
    End Sub

    Protected Sub DeshabilitarBotones()
        For i As Integer = 0 To grdBandejaTituloAO.Rows.Count - 1
            Dim row As GridViewRow = grdBandejaTituloAO.Rows(i)
            Dim esPuedeEditar As String = row.Cells(12).Text
            Dim btnContinue As Button = CType(row.Cells(11).Controls(0), Button)
            Dim btnPriorizar As Button = CType(row.Cells(10).Controls(0), Button)
            Dim checkReasignación As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
            Dim estadoOperativo As String = row.Cells(14).Text

            If (esPuedeEditar <> "1") Then
                btnContinue.Enabled = False
            Else
                btnPriorizar.Enabled = False
                If estadoOperativo = "4" Then
                    btnContinue.Text = "Gestionar"
                ElseIf estadoOperativo = "7" Then
                    btnContinue.Text = "Continuar"
                ElseIf estadoOperativo = "9" Then
                    btnContinue.Text = "Retomar"
                End If
            End If

            If EstadoOperativo = "3" Or EstadoOperativo = "13" Then
                checkReasignación.Visible = False
            End If
        Next
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        PintarGridBandejaTitulosEstudioTitulos()
    End Sub

    Protected Sub grdBandejaTituloAO_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdBandejaTituloAO.RowCommand
        'Dim codTitulo As String = grdBandejaTituloAO.Rows(e.CommandArgument).Cells(1).Text
        Dim codTareaAsiganada As String = grdBandejaTituloAO.Rows(e.CommandArgument).Cells(13).Text
        If e.CommandName = "cmdPriorizar" Then
            SolicitudPriorizacionControl.AsignarTareaAsiganada(codTareaAsiganada)
            SolicitudPriorizacionControl.IniciarFormulario()
            SolicitudPriorizacionControl.MostrarModal()
        ElseIf e.CommandName = "cmdContinuar" Then
            Dim tareaAsiganadaBLL As TareaAsignadaBLL = New TareaAsignadaBLL()
            Dim tareaAsiganada = tareaAsiganadaBLL.consultarTareaPorId(codTareaAsiganada)
            Response.Redirect("~/Security/modulos/maestro-acceso.aspx?ID_TASK=" & tareaAsiganada.ID_TAREA_ASIGNADA & "&AreaOrigenId=" & Session("usrAreaOrgen") & "&Edit=2", True)
        End If
    End Sub

    Protected Sub btnSolicitarReasignacion_Click(sender As Object, e As EventArgs) Handles btnSolicitarReasignacion.Click

        Dim _expedientesSeleccionados As New List(Of Integer)
        For i As Integer = 0 To grdBandejaTituloAO.Rows.Count - 1
            Dim row As GridViewRow = grdBandejaTituloAO.Rows(i)
            Dim chkReasignacion As CheckBox = CType(row.FindControl("chkReasignar"), CheckBox)
            If (chkReasignacion.Checked) Then
                Dim codTareaAsiganada As String = row.Cells(13).Text
                _expedientesSeleccionados.Add(Convert.ToInt32(codTareaAsiganada))
            End If
        Next
        SolicitudReasignacionPanel.AsignarTareasAsiganadas(_expedientesSeleccionados)
        SolicitudReasignacionPanel.IniciarFormulario()
        SolicitudReasignacionPanel.MostrarModal()

    End Sub

    Protected Sub btnExportarGrid_Click(sender As Object, e As EventArgs) Handles btnExportarGrid.Click
        Dim bandejaBLL As New BandejaBLL()
        Dim USULOG As String = Session("ssloginusuario")
        Dim NROTITULO As String = txtNoTitulo.Text
        Dim ESTADOPROCESAL As Int32 = Nothing 'ddlEstadoProcesal.SelectedValue
        Dim ESTADOSOPERATIVO As Int32 = ddlEstadoOperativo.SelectedValue
        Dim FCHENVIOCOBRANZADESDE As String = fecFechaEnvioTituloInicio.Text
        Dim FCHENVIOCOBRANZAHASTA As String = fecFechaEnvioTituloFin.Text
        Dim NROIDENTIFICACIONDEUDOR As String = txtNumIdentificacionDeudor.Text
        Dim NOMBREDEUDOR As String = txtNombreDeudor.Text


        'Instanciar clase de metodos globales
        Dim MTG As New MetodosGlobalesCobro

        'Convertir Gridview a DataTable
        Dim dt As DataTable = bandejaBLL.obtenerTitulosEstudioTitulos(USULOG, NROTITULO, ESTADOPROCESAL, ESTADOSOPERATIVO, FCHENVIOCOBRANZADESDE, FCHENVIOCOBRANZAHASTA, NROIDENTIFICACIONDEUDOR, NOMBREDEUDOR)
        dt.Columns(0).ColumnName = "ID"
        dt.Columns(2).ColumnName = "No. Título"
        dt.Columns(3).ColumnName = "Fecha de expedición del título"
        dt.Columns(4).ColumnName = "Nombre del deudor"
        dt.Columns(5).ColumnName = "NIT / CC"
        dt.Columns(6).ColumnName = "Tipo de obligación"
        dt.Columns(7).ColumnName = "Total Deuda"
        dt.Columns(8).ColumnName = "Fecha entrega Estudio títulos"
        dt.Columns(9).ColumnName = "Fecha límite"

        dt.Columns.Remove("ID_TAREA_ASIGNADA")
        dt.Columns.Remove("COLOR")
        dt.Columns.Remove("ID_ESTADO_OPERATIVOS")
        dt.Columns.Remove("VAL_PRIORIDAD")

        '"Convertir" datatable a dataset
        Dim ds As New DataSet
        ds.Merge(dt)

        'Exportar el dataset anterior a Excel 
        MTG.ExportDataSetToExcel(ds, "TitulosEstudioTitulos.xls")
    End Sub

    Protected Sub PaginadorGridView_EventActualizarGrid()
        PintarGridBandejaTitulosEstudioTitulos()
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

    Protected Sub imgBtnBorraFechaTituloInicio_Click(sender As Object, e As ImageClickEventArgs) Handles imgBtnBorraFechaTituloInicio.Click
        fecFechaEnvioTituloInicio.Text = String.Empty
    End Sub

    Protected Sub imgBtnBorraFechaTituloFin_Click(sender As Object, e As ImageClickEventArgs) Handles imgBtnBorraFechaTituloFin.Click
        fecFechaEnvioTituloFin.Text = String.Empty
    End Sub
End Class