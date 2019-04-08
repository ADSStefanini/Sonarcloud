Imports System.Data.SqlClient
Partial Public Class consulta_industriaycomercio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadGrid("")
        End If
    End Sub
    Private Sub GridVPredios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridVPredios.PageIndexChanging
        Try
            GridVPredios.PageIndex = e.NewPageIndex
            LoadGrid(txtEnte.Text.Trim)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadGrid(ByVal pValor As String)
        Dim cmd, criterio, condicion As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        cmd = ""
        If pValor = "" Then            
            cmd = "SELECT TOP 50 MaeNum,MaeProCod,MaeProNom,MaeDir FROM MAEIC WHERE MaeProNom LIKE 'A%' ORDER BY MaeProNom"
        Else
            criterio = cmbBuscarPor.SelectedValue
            condicion = ""
            Select Case criterio
                Case "Placa"
                    cmd = "SELECT TOP 50 MaeNum,MaeProCod,MaeProNom,MaeDir FROM maeic " & _
                        "WHERE MaeNum LIKE '" & pValor & "%' ORDER BY MaeNum,MaeProNom"

                Case "Cedula"
                    cmd = "SELECT TOP 50 MaeNum,MaeProCod,MaeProNom,MaeDir FROM maeic " & _
                        "WHERE MaeProCod LIKE '" & pValor & "%' ORDER BY MaeNum,MaeProNom"

                Case "Direccion"
                    cmd = "SELECT TOP 50 MaeNum,MaeProCod,MaeProNom,MaeDir FROM maeic " & _
                        "WHERE MaeDir LIKE '" & pValor & "%' ORDER BY MaeNum,MaeProNom"

                Case "Nombre"
                    cmd = "SELECT TOP 50 MaeNum,MaeProCod,MaeProNom,MaeDir FROM maeic " & _
                        "WHERE MaeProNom LIKE '%" & pValor & "%' ORDER BY MaeNum,MaeProNom"
            End Select
        End If

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtIndCom As New DataTable
        Adaptador.Fill(dtIndCom)

        GridVPredios.DataSource = dtIndCom
        GridVPredios.DataBind()
        If dtIndCom.Rows.Count = 0 Then
            Validator.ErrorMessage = "No se han encontrado resultados para su búsqueda"
            Validator.IsValid = False
        End If

        ' Poner la etiqueta de placa seleccionada en Ninguna y quitar resaltado de GridView
        LabelPredioSel.Text = "Ninguna"
        Me.GridVPredios.SelectedIndex = -1
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        LoadGrid(Me.txtEnte.Text.Trim)
    End Sub
    Private Sub GridVPredios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridVPredios.RowCommand
        If e.CommandName.ToUpper = "SELECT" Then
            ' Convert the row index stored in the CommandArgument
            ' property to an Integer.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Get the last name of the selected author from the appropriate
            ' cell in the GridView control.
            Dim selectedRow As GridViewRow = GridVPredios.Rows(index)
            Dim CeldaPlaca As TableCell = selectedRow.Cells(2)
            'Dim Predio As String = CeldaPlaca.Text.Trim

            Dim Placa As String = CType(GridVPredios.Rows(index).Cells(0).Controls(0), LinkButton).Text 'ok

            ' Mostrar la placa actual.                        
            LabelPredioSel.Text = Placa
        End If
    End Sub

    Protected Sub btnInfGral_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInfGral.Click
        If LabelPredioSel.Text <> "Ninguna" Then
            Response.Redirect("iyc_infogral.aspx?idplaca=" & LabelPredioSel.Text.Trim)
        End If
    End Sub

    Protected Sub btnCancelTraspasos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelTraspasos.Click
        If LabelPredioSel.Text <> "Ninguna" Then
            Response.Redirect("iyc_cancel_traspasos.aspx?idplaca=" & LabelPredioSel.Text.Trim)
        End If
    End Sub

    Protected Sub btnDeclaraciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeclaraciones.Click
        If LabelPredioSel.Text <> "Ninguna" Then
            Response.Redirect("iyc_declaraciones.aspx?idplaca=" & LabelPredioSel.Text.Trim)
        End If
    End Sub

    Protected Sub btnPagos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPagos.Click
        If LabelPredioSel.Text <> "Ninguna" Then
            Response.Redirect("iyc_pagos.aspx?idplaca=" & LabelPredioSel.Text.Trim)
        End If
    End Sub

    Protected Sub btnSaldosFavCon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaldosFavCon.Click
        If LabelPredioSel.Text <> "Ninguna" Then
            'Validar matricula no existe, esta cancelada
            '------------------------------------------------------------------------------------------------'
            Dim cmd As String
            Dim cnx As String = Funciones.CadenaConexionUnion
            cmd = "SELECT MaeNum, MaeProNom, MaeProCod, MaeDir, MaeEstAct " & _
                    "FROM MAEIC WHERE MaeNum = '" & LabelPredioSel.Text.Trim & "' AND ([MaeEstAct] < 3) ORDER BY [MaeNum]"
            Dim Adaptador As New SqlDataAdapter(cmd, cnx)
            Dim dtEncabezado1 As New DataTable
            Adaptador.Fill(dtEncabezado1)
            If dtEncabezado1.Rows.Count = 0 Then
                'Mostrar mensaje
                Response.Write("<script>alert('Matrícula no existe, está cancelada');</script>")
            Else
                Response.Redirect("iyc_estado_sanciones.aspx?idplaca=" & LabelPredioSel.Text.Trim)
            End If
            '------------------------------------------------------------------------------------------------'
        End If
    End Sub

    Protected Sub btnEstadoCta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstadoCta.Click
        If LabelPredioSel.Text <> "Ninguna" Then
            Response.Redirect("iyc_estado_cuenta.aspx?idplaca=" & LabelPredioSel.Text.Trim)
        End If
    End Sub
End Class