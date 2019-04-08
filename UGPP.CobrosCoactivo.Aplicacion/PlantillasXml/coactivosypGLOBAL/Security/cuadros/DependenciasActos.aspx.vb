Imports System.Data.SqlClient
Partial Public Class DependenciasActos
    Inherits System.Web.UI.Page

    Private Function Load_Secuencia_Actos(ByVal etapa As String) As DataTable
        Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT * FROM DEPENDENCIA_ACTUACIONES WHERE DEP_ETAPA = @ETAPA ORDER BY dep_orden ASC", CadenaConexion)
        Adap.SelectCommand.Parameters.Add("@ETAPA", SqlDbType.VarChar)
        Adap.SelectCommand.Parameters("@ETAPA").Value = etapa

        Dim Table As New DatasetForm.DEPENDENCIA_ACTUACIONESDataTable
        Adap.Fill(Table)

        Return Table
    End Function
    Private Sub RecargarTodo(ByVal codigo As String)
        Dim tb As New DatasetForm.DEPENDENCIA_ACTUACIONESDataTable
        tb = Load_Secuencia_Actos(codigo)
        GridDep.DataSource = tb
        GridDep.DataBind()
        resultado.InnerHtml = "<b>Existen " & Num2Text(tb.Rows.Count) & " Dependencias detectadas  </b>"
        Me.ViewState("DatosActos") = tb
        control.Value = Request.QueryString("textbox")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim etapa As String = Request.QueryString("etapa")
            If etapa = Nothing Then
                Response.Redirect("Etapas.aspx?textbox=" & Request.QueryString("textbox").Trim, True)
                Exit Sub
            End If

            Dim codigo As String = Mid(etapa.Trim(), etapa.IndexOf(":") + 3)
            xtitle.InnerHtml = etapa
            otraEtapa.InnerHtml = "<a href=Etapas.aspx?textbox=" & Request.QueryString("textbox").Trim & ">Seleccionar otra etapa</a>"
            RecargarTodo(codigo)
        End If



        'Dim row As DataRow
        'Dim col As DataColumn
        'Dim Msg As String = ""

        'If tb.Tables("Lista").Rows.Count > 0 Then
        '    Msg &= "<table>"
        '    Msg &= "<caption>Listas de precio.</caption>"
        '    Msg &= "<tr><th>Cod.</th><th>Descripcion</th><th>Cod. Dep.</th><th>Descripcion</th><th>Termino</th><th>Orden</th></tr>"
        '    For Each row In tb.Tables("Lista").Rows
        '        Msg &= "<tr>"
        '        For Each col In tb.Tables("Lista").Columns
        '            Msg &= "<td>" & valorNull(row(col), "&nbsp") & "</td>"
        '        Next
        '        Msg &= "</tr>"
        '    Next
        '    Msg &= "</table>"
        'End If

        ''


    End Sub

    Private Sub Ejecutarjavascript(ByVal script As String)
        Dim csname1 As [String] = "anything"
        Dim cstype As Type = Me.[GetType]()

        ' Get a ClientScriptManager reference from the Page class. 
        Dim cs As ClientScriptManager = Page.ClientScript

        If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As String = script
            cs.RegisterClientScriptBlock(cstype, csname1, cstext1.ToString())
        End If
    End Sub

    Protected Sub GridDep_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridDep.SelectedIndexChanged
        Try
            With Me
                Dim Mytb As DatasetForm.DEPENDENCIA_ACTUACIONESDataTable = CType(.ViewState("DatosActos"), DatasetForm.DEPENDENCIA_ACTUACIONESDataTable)
                With Mytb.Item(.GridDep.SelectedIndex)

                    Dim dato As String = ""
                    Dim termino As String
                    Dim tipotermino As String
                    'txtBuscar
                    dato = .DEP_CONMOV
                    termino = .DEP_TERMINO
                    tipotermino = .DEP_TIPOTERMINO
                    Dim strScript As String = "<script>window.opener.document.forms(0)." + control.Value + ".value = '"
                    strScript += dato.Trim
                    strScript += "';window.opener.document.forms(0).txtBuscar.value = '"
                    strScript += Request.QueryString("etapa")
                    'strScript += "';self.close()"
                    strScript += "';window.opener.document.location.href = '../configuracionActos.aspx?texto=" & dato.Trim & "&etapa=" & Request.QueryString("etapa") & "&termino=" & termino & "&tipotermino" & tipotermino & "';"
                    strScript += "</" + "script>"

                    'Dim strScript As String = "<script>window.opener.document.forms(0)." + control.Value + ".value = '"
                    'strScript += dato.Trim
                    ''strScript += "';self.close()"
                    'strScript += "';document.location.href = '../configuracionActos.aspx';"
                    'strScript += "</" + "script>"

                    Ejecutarjavascript(strScript)
                End With
            End With
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub Linkrecargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Linkrecargar.Click
        Dim etapa As String = Request.QueryString("etapa")
        Dim codigo As String = Mid(etapa.Trim(), etapa.IndexOf(":") + 3)
        RecargarTodo(codigo)
    End Sub

    Protected Sub LinkImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkImprimir.Click
        Dim cr As New rptDependenciasactos
        Dim ds As New DatasetForm.DEPENDENCIA_ACTUACIONESDataTable
        ds = CType(Me.ViewState("DatosActos"), DatasetForm.DEPENDENCIA_ACTUACIONESDataTable)

        Funciones.Exportar(Me, cr, ds, "Prueba.Pdf", "")
    End Sub
End Class