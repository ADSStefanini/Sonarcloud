Option Explicit On
Imports System.IO
Imports System.Net
Imports System.Data.SqlClient
Partial Public Class TiffViewer
    Inherits System.Web.UI.Page

    Function Extraer(ByVal Path As String) As String
        Return System.IO.Path.GetExtension(Path)
    End Function

    Private Sub ObjetoImg(ByVal pagina As Integer, ByVal fileName As String, ByVal all As String)
        fileName = fileName
        Dim ArchivoaBuscar As String
        'ArchivoaBuscar = Session("ssrutaexpediente") & "\" '& Session("mcobrador") & "\Disco\" & fileName
        ArchivoaBuscar = Session("ssrutaexpediente") & "\" & fileName

        If Not File.Exists(ArchivoaBuscar) Then
            ImagenesManager.InnerHtml = "<img src=""Imagenes/archivoerr.png"" id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
            Exit Sub
        End If

        'Localize remoto
        Dim tipo_extension As String
        tipo_extension = Extraer(fileName).Trim

        If tipo_extension.ToLower = "pdf" OrElse tipo_extension.ToLower = ".pdf" Then
            Dim pdfPath As String = ArchivoaBuscar
            Dim client As New WebClient()
            Dim buffer As [Byte]() = client.DownloadData(pdfPath)
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-length", buffer.Length.ToString())
            Response.BinaryWrite(buffer)
            Exit Sub
        End If

        If all = "si" Then
            ImagenesManager.InnerHtml += "<img src=""MostrarImagen.ashx?ImageFileName=" & ArchivoaBuscar & "&Item=" & pagina & """ id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
        Else
            ImagenesManager.InnerHtml = "<img src=""MostrarImagen.ashx?ImageFileName=" & ArchivoaBuscar & "&Item=" & pagina & """ id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim NroPaginas As Integer = CType(Val(Request.QueryString("totimg")), Integer)

            Dim xxt As Integer = 1
            For xxt = 1 To NroPaginas
                paginas.Items.Add(xxt)
            Next

            ObjetoImg(0, Request.QueryString("F"), "no")
            ViewState("File") = Request.QueryString("F")
            'Detalle 
            Dim mArchivo, mNomEnte, mActo, mIdEnte, mIdActo, expediente, Observacion As String
            Dim ArchivoBase As String = ""

            mArchivo = Request.QueryString("F")
            mArchivo = mArchivo.ToLower()
            mNomEnte = Request.QueryString("nomente")
            mIdEnte = Request.QueryString("idente")
            mIdActo = Request.QueryString("idacto")
            mActo = Request.QueryString("acto")
            expediente = Request.QueryString("Expedinete")
            Observacion = Request.QueryString("observacion")
            Dim Enabled As String = Request.QueryString("Enabled")


            If Request.QueryString("vsExpedienteAcu") = Nothing Then
                LinkProceso.Enabled = False
                LinkProceso.ForeColor = Drawing.Color.Gray
            End If

            Dim extra As String = ""
            If Session("mnivelacces") = "1" Then
                extra = Extra_Admin()
            End If
            ActoAdmind.InnerHtml = mNomEnte & " (" & mIdEnte & ")"

            Dim tabla As String
            tabla = "<table class=""tabla"">" & vbNewLine
            tabla += "<table class=""tabla"">"
            tabla += "<tr><th colspan=""2"" style=""text-align: center;"">VISOR DE IMAGENES</th></tr>" & vbNewLine
            tabla += "<tr>" & vbNewLine
            tabla += "<th>ENTIDAD : </th><td>" & mNomEnte & " (" & mIdEnte & ")</td>" & vbNewLine
            tabla += "</tr>" & vbNewLine
            tabla += "<tr>" & vbNewLine
            tabla += "<th>ACTUACION : </th><td>" & mActo & "</td>" & vbNewLine
            tabla += "</tr>" & vbNewLine
            tabla += "<tr>" & vbNewLine
            tabla += "<th>NOMBRE DEL ARCHIVO : </th><td>" & mArchivo & "</td>" & vbNewLine
            tabla += "</tr>" & vbNewLine & extra
            tabla += "<tr>" & vbNewLine
            tabla += "<th>NUMERO DE PAGINA(S) : </th><td>" & NroPaginas & "</td>" & vbNewLine
            tabla += "</tr>" & vbNewLine
            tabla += "<tr>" & vbNewLine
            tabla += "<th>OBSERVACIÓN : </th><td>" & Observacion & "</td>" & vbNewLine
            tabla += "</tr>" & vbNewLine
            tabla += "</table>" & vbNewLine

            DetalleVisor.InnerHtml = tabla
        End If
    End Sub
    Private Function Extra_Admin() As String
        Dim var As String = ""

        Dim fileName As String = Request.QueryString("F")
        Dim ArchivoaBuscar As String
        ArchivoaBuscar = Session("ssrutaexpediente") & "\" & fileName

        var += "<tr>" & vbNewLine
        var += "<th>RUTA DEL ARCHIVO : </th><td>" & ArchivoaBuscar & "</td>" & vbNewLine
        var += "</tr>" & vbNewLine

        Return var
    End Function
    Protected Sub paginas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles paginas.SelectedIndexChanged
        Dim page As Integer = CType(paginas.SelectedValue, Integer) - 1
        Try
            ImagenesManager.InnerHtml = ""
            ObjetoImg(page, ViewState("File"), "no")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub LinkAntes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkAntes.Click
        If paginas.SelectedIndex > 0 Then
            paginas.SelectedIndex = paginas.SelectedIndex - 1
        End If

        paginas_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Protected Sub LinkSiguiente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkSiguiente.Click
        Dim NroPaginas As Integer = CType(Val(Request.QueryString("totimg")), Integer) - 1
        If paginas.SelectedIndex < NroPaginas Then
            paginas.SelectedIndex = paginas.SelectedIndex + 1
        End If

        paginas_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Protected Sub LinkProceso_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkProceso.Click
        Using con As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim Acumulado As String = Request.QueryString("vsExpedienteAcu")
            If (Acumulado <> Nothing) And (Acumulado <> "0") And (Acumulado <> "") Then
                Dim adapter As New SqlDataAdapter("select a.entidad,a.idacto,b.nombre,a.nomarchivo,a.paginas,a.docexpediente,a.docpredio_refecatrastal,a.fecharadic from DOCUMENTOS a, actuaciones b  where a.idacto = b.codigo and docacumulacio=@docacumulacio order by a.docexpediente", con)
                adapter.SelectCommand.Parameters.Add("@docacumulacio", SqlDbType.VarChar).Value = Acumulado
                Dim tb As New DataTable
                adapter.Fill(tb)
                If tb.Rows.Count Then
                    Me.ViewState("Datos") = Nothing
                    Me.ViewState("Datos") = CType(tb, DataTable)
                    Me.ViewState("nroacu") = tb.Rows.Count
                    dtgViewActos.DataSource = tb
                    dtgViewActos.DataBind()
                    Me.ModalPopupExtender2.Show()
                Else
                    ViewState("PpalM") = "No posee acumulado."
                    Me.ModalPopupExtender1.Show()
                End If
            Else
                ViewState("PpalM") = "No posee acumulado."
                Me.ModalPopupExtender1.Show()
            End If
        End Using
    End Sub
    Private Sub GridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dtgViewActos.PageIndexChanging
        Dim grilla As GridView = CType(sender, GridView)
        With grilla
            .PageIndex = e.NewPageIndex()
        End With
        Me.ModalPopupExtender2.Show()
        dtgViewActos.DataSource = CType(Me.ViewState("Datos"), DataTable)
        dtgViewActos.DataBind()
    End Sub
    Protected Sub dtgViewActos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgViewActos.SelectedIndexChanged
        Dim mytable As New DataTable
        mytable = CType(ViewState("Datos"), DataTable)
        If mytable.Rows.Count > 0 Then
            With mytable.Rows.Item(dtgViewActos.SelectedIndex)
                Dim fileName As String = .Item("nomarchivo")
                ViewState("File") = fileName

                ObjetoImg(0, fileName, "no")
            End With
        End If
    End Sub

    Protected Sub LinkTodoDoc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkTodoDoc.Click
        Dim NroPaginas As Integer = CType(Val(Request.QueryString("totimg")), Integer)
        ImagenesManager.InnerHtml = ""
        Dim xxt As Integer = 0
        For xxt = 0 To NroPaginas - 1
            ObjetoImg(xxt, ViewState("File"), "si")
        Next
    End Sub
End Class
