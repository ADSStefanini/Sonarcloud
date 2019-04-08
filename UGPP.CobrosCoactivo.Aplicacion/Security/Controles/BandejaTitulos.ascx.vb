Imports System.Drawing
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica


Public Class BandejaTitulos
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            CommonsCobrosCoactivos.poblarEstadosOperativosTitulos(ddlEstadoOperativo)
            CommonsCobrosCoactivos.poblarEstadosProcesalesTitulos(ddlEstadoProcesal)
            PintarGridBandejaTitulosAO(Session("ssloginusuario"), "", "0", "0", "", "", "", "")
        End If
    End Sub

    Protected Sub Buscar_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        PintarGridBandejaTitulosAO(Session("ssloginusuario"), txtNoTitulo.Text, ddlEstadoProcesal.SelectedValue, ddlEstadoOperativo.SelectedValue, fecFechaEnvioTituloInicio.Text, fecFechaEnvioTituloFin.Text, txtNumIdentificacionDeudor.Text, txtNombreDeudor.Text)
    End Sub

    ''' <summary>
    ''' Metodo que ejecuta el llenado y pinta el GridView de la bandeja de titulos area origen
    ''' </summary>
    Protected Sub PintarGridBandejaTitulosAO(ByVal USULOG As String, ByVal NROTITULO As String, ByVal ESTADOPROCESAL As Int32, ByVal ESTADOSOPERATIVO As Int32, ByVal FCHENVIOCOBRANZADESDE As String, ByVal FCHENVIOCOBRANZAHASTA As String, ByVal NROIDENTIFICACIONDEUDOR As String, ByVal NOMBREDEUDOR As String)
        Dim MetodoSel As New BandejaAreaOrigenBLL()
        Dim DatosTraer As New List(Of BandejaTitulosAreaOrigen)
        Dim DatosTraerParcial As New List(Of BandejaTitulosAreaOrigen)
        DatosTraer = MetodoSel.ConsultarDatosGrillaBandejaAreaOrigen(USULOG, NROTITULO, ESTADOPROCESAL, ESTADOSOPERATIVO, FCHENVIOCOBRANZADESDE, FCHENVIOCOBRANZAHASTA, NROIDENTIFICACIONDEUDOR, NOMBREDEUDOR)
        grdBandejaTituloAO.DataSource = DatosTraer
        grdBandejaTituloAO.DataBind()
        PaginadorGridView.UpdateLabels()
    End Sub

    Protected Sub TextButton(sender As Object, e As GridViewRowEventArgs) Handles grdBandejaTituloAO.RowDataBound
        For i As Integer = 0 To grdBandejaTituloAO.Rows.Count - 1
            Dim LabelButton As Label = CType(grdBandejaTituloAO.Rows(i).FindControl("LblAcciones"), Label)
            Dim LabelColor As Label = CType(grdBandejaTituloAO.Rows(i).FindControl("LblColorAccion"), Label)
            Dim activeButton As Button = CType(grdBandejaTituloAO.Rows(i).FindControl("BtnAcciones"), Button)
            If LabelButton.Text = "1" Then
                activeButton.Text = "Continuar"
            Else
                activeButton.Text = "Subsanar"
            End If
            If LabelColor.Text = "ROJO" Then
                For j As Integer = 0 To grdBandejaTituloAO.Columns.Count - 1
                    grdBandejaTituloAO.Rows(i).Cells(j).BackColor = Color.Red
                Next
            End If
        Next
    End Sub

    Protected Sub Redirect(sender As Object, e As GridViewCommandEventArgs) Handles grdBandejaTituloAO.RowCommand
        If (e.CommandName = "ClickRedireccionar") Then
            Dim ID_TAREA_ASIGNADA As Int64 = Convert.ToString(e.CommandArgument.ToString())
            Response.Redirect("~/Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx?ID_TASK=" & ID_TAREA_ASIGNADA & "&AreaOrigenId=" & Session("usrAreaOrgen") & "&Edit=1")
        End If
    End Sub

    Protected Sub btnExportarGrid_Click(sender As Object, e As EventArgs) Handles btnExportarGrid.Click
        'Instanciar clase de metodos globales
        Dim MTG As New MetodosGlobalesCobro

        'Convertir Gridview a DataTable
        Dim dt As DataTable = MTG.GridviewToDataTable(grdBandejaTituloAO)
        dt.Columns.Remove("Column1")
        dt.Columns.Remove("Acciones")
        '"Convertir" datatable a dataset
        Dim ds As New DataSet
        ds.Merge(dt)

        'Exportar el dataset anterior a Excel 
        MTG.ExportDataSetToExcel(ds, "TitulosAreaOrigen.xls")
    End Sub

    Protected Sub PaginadorGridView_EventActualizarGrid()
        Buscar_Click(Nothing, Nothing)
    End Sub

    Protected Sub imgBtnBorraFechaTituloInicio_Click(sender As Object, e As ImageClickEventArgs) Handles imgBtnBorraFechaTituloInicio.Click
        fecFechaEnvioTituloInicio.Text = String.Empty
    End Sub

    Protected Sub imgBtnBorraFechaTituloFin_Click(sender As Object, e As ImageClickEventArgs) Handles imgBtnBorraFechaTituloFin.Click
        fecFechaEnvioTituloFin.Text = String.Empty
    End Sub
End Class
