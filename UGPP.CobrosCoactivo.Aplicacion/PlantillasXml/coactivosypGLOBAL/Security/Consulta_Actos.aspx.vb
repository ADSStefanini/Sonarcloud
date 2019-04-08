Imports System.Data.SqlClient
Partial Public Class Consulta_Actos
    Inherits System.Web.UI.Page


    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel()"
            menssageError(amsbgox)
            Exit Sub
        End If
    End Sub

    Private Function ArmarHREF(ByVal row As DataRow) As String
        Dim reto As String
        reto = "<a href=""javascript:;""  onclick=""window.open('TiffViewer.aspx?nomente=" & row("nombre") & "&idente=" & row("entidad") & "&F=" & row("NOMARCHIVO") & "&totimg=" & row("PAGINAS") & "&acto=" & row("DESCRIPCION") & "&idacto=" & row("ACTO") & "&folder=&Enabled=false&observacion=" & row("DOCOBSERVACIONES") & "&vsExpedienteAcu=" & ViewState("ssExamenAcumulado") & "', '', 'fullscreen=yes, scrollbars=auto')"">" & row("DESCRIPCION") & "</a>"
        Return reto
    End Function

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim Acto, CodigoEtapa, NombreEtapa As String
        Acto = Mid(Me.txtActo.Text.Trim(), Me.txtActo.Text.IndexOf(":") + 3)
        Me.contenidogrids.InnerHtml = ""
        Dim table As New DataTable
        Dim myAdacter As New SqlDataAdapter("select b.idetapa,c.nombre as nomacto,a.entidad,d.nombre,a.docexpediente,a.docpredio_refecatrastal,a.fecharadic,a.nomarchivo,a.paginas,a.docObservaciones,a.docacumulacio from documentos a inner join (actuaciones b inner join etapas c on b.idetapa = c.codigo) on a.idacto = b.codigo inner join entesdbf d on a.entidad = d.codigo_nit where idacto = @idacto ", Funciones.CadenaConexion)
        myAdacter.SelectCommand.Parameters.Add("@idacto", SqlDbType.Char, 3).Value = Acto
        myAdacter.Fill(table)
        'Dim row As DataRow
        'Dim col As DataColumn
        Dim Msg As String = ""
        If table.Rows.Count > 0 Then
            CodigoEtapa = Trim(table(0).Item("idetapa"))
            NombreEtapa = CodigoEtapa & ". " & table.Rows(0).Item("nomacto")
            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedortitulos'>" + NombreEtapa + "</div>"
            'Msg &= "<table  width=""100%"" class=""servicesT"">" _
            '     & "<tr><th colspan='2'>Acto Administrativo</th><th>F. Radicación</th><th>F. Proyección</th><th>Termino</th></tr>"
            'For Each row In table.Rows
            '    Msg &= "<tr>"
            '    For Each col In table.Columns
            '        If (col.ColumnName <> "entidad") And (col.ColumnName <> "nombre") And (col.ColumnName <> "docpredio_refecatrastal") And (col.ColumnName <> "fecharadic") And (col.ColumnName <> "nomarchivo") And (col.ColumnName <> "DOCOBSERVACIONES") Then
            '            If col.ColumnName = "nomarchivo" Then
            '                Msg &= "<td class=""servHd"">" & ArmarHREF(row) & "</td>"
            '            Else
            '                Msg &= "<td>" & valorNull(row(col), "&nbsp") & "</td>"
            '            End If
            '        End If
            '    Next
            '    Msg &= "</tr>"
            'Next
            'Msg &= "</table>"
            Grid_Datos.DataSource = table
            Grid_Datos.DataBind()
            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + Msg
            Me.ViewState("Datos") = CType(table, DataTable)

        Else
            Dim cmd As String
            cmd = "select b.codigo as codeta,b.nombre as nometa,a.codigo as codacto, a.nombre as nomacto from actuaciones a inner join etapas b on a.idetapa = b.codigo and a.codigo = @codigo"
            Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
            oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, Funciones.CadenaConexion)
            oDtAdapterSql1.SelectCommand.Parameters.Add("@codigo", SqlDbType.Char, 3).Value = Acto
            Dim Dataset1 As DataSet = New DataSet
            oDtAdapterSql1.Fill(Dataset1, "etapas")
            Me.contenidogrids.InnerHtml = ""
            If Dataset1.Tables("etapas").Rows.Count > 0 Then
                CodigoEtapa = Trim(Dataset1.Tables("etapas").Rows(0).Item("codeta"))
                NombreEtapa = CodigoEtapa & ". " & Dataset1.Tables("etapas").Rows(0).Item("nometa")
                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedortitulos'>" + NombreEtapa + "</div>"
                Msg &= "<table  width=""100%"" class=""servicesT"">" _
                & "<tr><th colspan='2'>Acto Administrativo</th><th>F. Radicación</th><th>F. Proyección</th><th>Termino</th></tr>"
                Msg &= "<tr>"
                Msg &= "<td colspan='5' class=""EservEHd"">No hay actos...</td>"
                Msg &= "</tr></table>"
                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + Msg
            Else
                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "No hay actos..."
            End If
        End If
    End Sub
    Private Sub Grid_Datos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Grid_Datos.PageIndexChanging
        Dim grilla As GridView = CType(sender, GridView)
        With grilla
            .PageIndex = e.NewPageIndex()
        End With

        Grid_Datos.DataSource = CType(Me.ViewState("Datos"), DataTable)
        Grid_Datos.DataBind()
    End Sub

    Protected Sub Grid_Datos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Grid_Datos.SelectedIndexChanged
        With Me
            Dim Mytb As DataTable = CType(.ViewState("Datos"), DataTable)
            Dim index As Integer = Grid_Datos.SelectedIndex + (Grid_Datos.PageIndex * Grid_Datos.PageSize)
            'Dim Row As DataRow = Mytb.Rows(index)
            contenidogrids_3.InnerHtml = "<b>Ultimo Seleecionado: </b>" & Mytb.Rows(index).Item("entidad") & " - <b style='color:ccc;'>" & Mytb.Rows(index).Item("nombre") & "</b> - " & Mytb.Rows(index).Item("docexpediente") & " - " & valorNull(Mytb.Rows(index).Item("docpredio_refecatrastal"), "&nbsp")
        End With
    End Sub
End Class