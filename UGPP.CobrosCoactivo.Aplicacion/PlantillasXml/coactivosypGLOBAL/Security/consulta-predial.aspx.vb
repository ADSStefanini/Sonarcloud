Imports System.Data.SqlClient
Partial Public Class consulta_predial
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
            cmd = "SELECT TOP 50 MunCod,PreDir,PreMatInm,PreCod,PreNum FROM predios WHERE PreDir NOT LIKE 'PRUEBA%' AND PreDir <> '' AND PreMatInm <> '' ORDER BY MunCod,PreNum"
        Else            
            criterio = cmbBuscarPor.SelectedValue
            condicion = ""
            Select Case criterio
                Case "NroPredial"
                    cmd = "SELECT TOP 50 MunCod,PreDir,PreMatInm,PreCod,PreNum FROM predios " & _
                        "WHERE PreNum LIKE '" & pValor & "%' ORDER BY MunCod,PreNum"
                    'Columna 2                    
                    CType(GridVPredios.Columns(1), BoundField).DataField = "PreCod"
                    GridVPredios.Columns(1).HeaderText = "Cod. Corto"
                    GridVPredios.Columns(1).Visible = True
                    'Columna 3
                    CType(GridVPredios.Columns(2), BoundField).DataField = "PreMatInm"
                    GridVPredios.Columns(2).HeaderText = "Matrícula Inmobiliaria"
                    GridVPredios.Columns(2).Visible = True

                Case "Cedula"                    
                    cmd = "SELECT TOP 50 T1.MunCod, T1.PrsCod, T1.PrePrsDoc, T3.PreDir, T2.PrsNom, T1.PreNum  " & _
                        "FROM ((PREDIOS3 T1 INNER JOIN PERSONAS T2 ON T2.PrsCod = T1.PrsCod) " & _
                        "INNER JOIN PREDIOS T3 ON T3.MunCod = T1.MunCod AND T3.PreNum = T1.PreNum) " & _
                        "WHERE T1.PrePrsDoc LIKE '%" & pValor & "' ORDER BY  T1.PrePrsDoc"
                    'Columna 2
                    CType(GridVPredios.Columns(1), BoundField).DataField = "PrsNom"
                    GridVPredios.Columns(1).HeaderText = "Nombre"
                    GridVPredios.Columns(1).Visible = True
                    'Columna 3
                    CType(GridVPredios.Columns(2), BoundField).DataField = "PrsCod"
                    GridVPredios.Columns(2).Visible = False

                Case "Nombre"
                    cmd = "SELECT TOP 50 T1.MunCod, T1.PrsCod, T1.PrePrsNom, T3.PreDir, T2.PrsNom, T1.PreNum " & _
                         "FROM ((PREDIOS3 T1 INNER JOIN PERSONAS T2 " & _
                         "ON T2.PrsCod = T1.PrsCod) INNER JOIN PREDIOS T3 " & _
                         "ON T3.MunCod = T1.MunCod AND T3.PreNum = T1.PreNum) " & _
                         "WHERE T1.PrePrsNom LIKE '" & pValor & "%' ORDER BY T1.PrePrsNom"
                    'Columna 2
                    CType(GridVPredios.Columns(1), BoundField).DataField = "PrsNom"
                    GridVPredios.Columns(1).HeaderText = "Nombre"
                    GridVPredios.Columns(1).Visible = True
                    'Columna 3
                    CType(GridVPredios.Columns(2), BoundField).DataField = "PrsCod"
                    GridVPredios.Columns(2).Visible = False

                Case "Direccion"
                    cmd = "SELECT TOP 50 MunCod, PreDir, PreNum FROM PREDIOS WHERE PreDir LIKE '" & pValor & "%' ORDER BY PreDir"
                    'Columna 1
                    GridVPredios.Columns(0).ItemStyle.Width = 80
                    'Columna 2
                    CType(GridVPredios.Columns(1), BoundField).DataField = "PreNum"
                    GridVPredios.Columns(1).Visible = False
                    'Columna 3
                    CType(GridVPredios.Columns(2), BoundField).DataField = "PreNum"
                    GridVPredios.Columns(2).Visible = False

                Case "MatInmobiliaria"
                    cmd = "SELECT TOP 50 MunCod, PreDir, PreMatInm, PreCod, PreNum FROM predios " & _
                        "WHERE PreMatInm LIKE '" & pValor & "%' ORDER BY PreMatInm"
                    'Columna 2                    
                    CType(GridVPredios.Columns(1), BoundField).DataField = "PreCod"
                    GridVPredios.Columns(1).HeaderText = "Cód. Corto"
                    GridVPredios.Columns(1).Visible = False
                    'Columna 3
                    CType(GridVPredios.Columns(2), BoundField).DataField = "PreMatInm"
                    GridVPredios.Columns(2).HeaderText = "Matrícula Inmobiliaria"
                    GridVPredios.Columns(2).Visible = True

                Case "CodCorto"
                    cmd = "SELECT TOP 50 MunCod, PreDir, PreMatInm, PreCod, PreNum FROM PREDIOS WHERE PreCod >= '" & pValor & "' ORDER BY PreCod"
                    'Columna 2                    
                    CType(GridVPredios.Columns(1), BoundField).DataField = "PreCod"
                    GridVPredios.Columns(1).HeaderText = "Cod. Corto"
                    GridVPredios.Columns(1).Visible = True
                    'Columna 3
                    CType(GridVPredios.Columns(2), BoundField).DataField = "PreMatInm"
                    GridVPredios.Columns(2).HeaderText = "Matrícula Inmobiliaria"
                    GridVPredios.Columns(1).Visible = True
            End Select
        End If

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtPredios As New DataTable
        Adaptador.Fill(dtPredios)

        GridVPredios.DataSource = dtPredios
        GridVPredios.DataBind()
        If dtPredios.Rows.Count = 0 Then
            Validator.ErrorMessage = "No se han encontrado resultados para su búsqueda"
            Validator.IsValid = False
        End If

        ' Poner la etiqueta de predio seleccionado en Ninguno y quitar resaltado de GridView
        LabelPredioSel.Text = "Ninguno"
        Me.GridVPredios.SelectedIndex = -1
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        LoadGrid(Me.txtEnte.Text.Trim)
    End Sub

    Protected Sub btnInfGral_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInfGral.Click
        If LabelPredioSel.Text <> "Ninguno" Then
            Response.Redirect("consulta-predial01.aspx?idpred=" & LabelPredioSel.Text.Trim)
        End If
    End Sub
    Protected Sub btnAvaluos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAvaluos.Click
        If LabelPredioSel.Text <> "Ninguno" Then
            Response.Redirect("consulta-predial02.aspx?idpred=" & LabelPredioSel.Text.Trim)
        End If
    End Sub
    Protected Sub btnPagos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPagos.Click
        If LabelPredioSel.Text <> "Ninguno" Then
            Response.Redirect("consulta-predial03.aspx?idpred=" & LabelPredioSel.Text.Trim)
        End If
    End Sub
    'Protected Sub btnEstadoCuenta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstadoCuenta.Click
    '    If LabelPredioSel.Text <> "Ninguno" Then
    '        Response.Redirect("estado-cuenta.aspx?idpred=" & LabelPredioSel.Text.Trim)
    '    End If
    'End Sub
    Protected Sub btnSaldosFavCon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaldosFavCon.Click
        Response.Redirect("salfav01.aspx")
    End Sub
    Private Sub GridVPredios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridVPredios.RowCommand
        If e.CommandName.ToUpper = "SELECT" Then
            ' Convert the row index stored in the CommandArgument
            ' property to an Integer.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Get the last name of the selected author from the appropriate
            ' cell in the GridView control.
            Dim selectedRow As GridViewRow = GridVPredios.Rows(index)
            Dim CeldaPredio As TableCell = selectedRow.Cells(2)
            'Dim Predio As String = CeldaPredio.Text.Trim

            Dim Predio As String = CType(GridVPredios.Rows(index).Cells(0).Controls(0), LinkButton).Text 'ok

            ' Mostrar el predio actual.                        
            LabelPredioSel.Text = Predio
        End If
    End Sub

    Protected Sub btnRunEXE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRunEXE.Click
        Dim Command As New Process
        Command.StartInfo.FileName = "c:\sas9\sas9.exe"
        'Command.StartInfo.Arguments = "/c miapp.exe /original=" & Chr(34) & "D:\temporal\original.docx" & Chr(34) & "  /modified=" & Chr(34) & "D:\temporal\modificado.docx" & Chr(34) & " /outfile=" & Chr(34) & "D:\temporal\resultado.rtf" & Chr(34) & " /RTF /V /S"
        Command.StartInfo.RedirectStandardError = True
        Command.StartInfo.RedirectStandardOutput = True
        Command.StartInfo.UseShellExecute = False
        Try
            If Command.Start() Then
                'Response.Write("OK")
            Else
                'Response.Write("KO")
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class