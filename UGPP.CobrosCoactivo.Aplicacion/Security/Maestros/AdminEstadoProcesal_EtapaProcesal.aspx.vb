Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo
Imports System.Data.SqlClient

Public Class AdminRelacion_EstadoProcesal_EtapaProcesal
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            CommonsCobrosCoactivos.poblarEstadosProcesalesTitulos(ddlEstados)

            Dim EstadoProcesoBLL As EstadosProcesoBLL = New EstadosProcesoBLL()
            Dim EstadosProceso As List(Of EstadosProceso) = EstadoProcesoBLL.obtenerEstadosProcesos()
            gvwMatriz.Enabled = True
        End If
    End Sub

    Protected Sub ddlEstados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEstados.SelectedIndexChanged
        Dim EtapaProcesal As EtapaProcesalBLL = New EtapaProcesalBLL()
        Dim _RelacionEP_EPBLL As Relacion_Estado_EtapaBLL = New Relacion_Estado_EtapaBLL()
        Dim etapas As List(Of EtapaProcesal) = EtapaProcesal.ObtenerEtapaProcesalPorId(ddlEstados.SelectedValue)
        Dim contador As Int32
        gvwMatriz.DataSource = etapas
        gvwMatriz.DataBind()
        For i As Integer = 0 To gvwMatriz.Rows.Count - 1
            Dim row As GridViewRow = gvwMatriz.Rows(i)
            Dim EtapaId As String = row.Cells(1).Text
            Dim codigo_etapa As Int32 = Convert.ToInt32(EtapaId)
            Dim chequeo = TryCast(row.FindControl("chkSelect"), CheckBox)
            contador = _RelacionEP_EPBLL.ConsultaEstadoEtapaPorID(ddlEstados.SelectedValue.ToString, codigo_etapa)
            If (contador > 0) Then
                chequeo.Checked = True
                row.Enabled = True
            End If

        Next
    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Dim _RelacionEP_EPBLL As Relacion_Estado_EtapaBLL = New Relacion_Estado_EtapaBLL()
        Dim contador As Int32
        Dim j As Int32 = 0
        Dim k As Int32 = 0
        Dim listado As List(Of String) = New List(Of String)
        Dim listadoborrado As List(Of String) = New List(Of String)
        For i As Integer = 0 To gvwMatriz.Rows.Count - 1
            Dim row As GridViewRow = gvwMatriz.Rows(i)
            Dim chequeo = TryCast(row.FindControl("chkSelect"), CheckBox)
            If (chequeo.Checked) Then
                Dim EtapaId As String = row.Cells(1).Text
                j = j + 1
                listado.Add(EtapaId)
            Else
                Dim EtapaId As String = row.Cells(1).Text
                listadoborrado.Add(EtapaId)
            End If
        Next
        If (ddlEstados.SelectedValue = "0") Then
            CustomValidator2.Text = "Seleccione un Estado de Proceso Por favor"
            CustomValidator2.IsValid = False
        ElseIf (j = 0) Then
            CustomValidator2.Text = "Seleccione alguna Etapa Procesal"
            CustomValidator2.IsValid = False
        Else
            For Each etapas As String In listado
                k = Convert.ToInt32(etapas)
                contador = _RelacionEP_EPBLL.ConsultaEstadoEtapaPorID(ddlEstados.SelectedValue.ToString, k)
                If (contador = 0) Then
                    _RelacionEP_EPBLL.InsertarEstadoEtapa(ddlEstados.SelectedValue.ToString, k)
                End If
            Next
            For Each eliminacion As String In listadoborrado
                k = Convert.ToInt32(eliminacion)
                contador = _RelacionEP_EPBLL.ConsultaEstadoEtapaPorID(ddlEstados.SelectedValue.ToString, k)
                If (contador > 0) Then
                    _RelacionEP_EPBLL.BorradoEstadoEtapa(ddlEstados.SelectedValue.ToString, k)
                End If
            Next
            CustomValidator2.Text = "Registro Exitoso"
                CustomValidator2.IsValid = False
            End If
    End Sub

    Protected Sub gvwMatriz_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvwMatriz.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chkbox As CheckBox = CType(e.Row.FindControl("chkSelect"), CheckBox)
            chkbox.Enabled = True
        End If
    End Sub
End Class