Public Partial Class consultardocumentos2
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        If Not Me.Page.IsPostBack Then
            Dim fecha As Date = Today.Date
            Dim fechaFormateada As String
            fechaFormateada = fecha.ToString("dd/MM/yyyy")

            txtFechaRad.Text = fechaFormateada
            txtFechaRad2.Text = fechaFormateada

            If Session("mcobrador") Is Nothing Then
                Me.Validator.ErrorMessage = "<font color='#8A0808' >Error : La sesión ha caducado <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
                Me.Validator.IsValid = False
                Exit Sub
            End If
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
        End If
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        consulta("rango")
    End Sub
    Protected Sub btnConsultarHoy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultarHoy.Click
        consulta("hoy")
    End Sub
    Protected Sub btnUltimos90_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUltimos90.Click
        consulta("ultimos90")
    End Sub

    Private Sub xtabla(ByVal Table As DataTable)
        Me.contenidogrids.InnerHtml = ""
        Dim row As DataRow
        Dim col As DataColumn

        Dim Msg As String = ""

        If Table.Rows.Count > 0 Then
            Msg &= "<table  style=""width:1500px"" class=""servicesT"">" _
            & "<tr><th colspan='2'>Usuario</th><th>Fech. System</th><th colspan='2'>Deudor</th><th colspan='2'>Acto Administrativo</th><th>Predio</th><th>Expediente</th><th>Pag.</th><th>Archivo</th></tr>"
            For Each row In Table.Rows
                Msg &= "<tr>"
                For Each col In Table.Columns
                    If (col.ColumnName <> "ruta") And (col.ColumnName <> "id") And (col.ColumnName <> "fecharadic") Then
                        If col.ColumnName = "nomarchivo" Then
                            Msg &= "<td class=""servHd"">" & ArmarHREF(row, 1) & "</td>"
                        ElseIf col.ColumnName = "docusuario" Then
                            Msg &= "<td class=""servHd " & col.ColumnName & """>" & ArmarHREF(row, 2) & "</td>"
                        Else
                            Msg &= "<td class='" & col.ColumnName & "'>" & valorNull(row(col), "&nbsp") & "</td>"
                        End If
                    End If
                Next
                Msg &= "</tr>"
            Next
            Msg &= "<tr><th colspan='2'>Usuario</th><th>Fech. System</th><th colspan='2'>Deudor</th><th colspan='2'>Acto Administrativo</th><th>Predio</th><th>Expediente</th><th>Pag.</th><th>Archivo</th></tr>"
            Msg &= "</table>"
        Else
            Msg = ""
            Me.contenidogrids.InnerHtml = "<br /> &nbsp No hay actos o expedientes según la consulta..."
        End If

        Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + Msg
    End Sub

    Private Function ArmarHREF(ByVal row As DataRow, ByVal op As Byte) As String
        Dim reto As String = ""
        Select Case op
            Case 1
                reto = "<a href=""javascript:;""  onclick=""window.open('TiffViewer.aspx?nomente=" & row("nomente") & "&idente=" & row("entidad") & "&F=" & row("nomarchivo") & "&totimg=" & row("paginas") & "&acto=" & row("nomactuacion") & "&idacto=" & row("idacto") & "&folder=&Enabled=false', '', 'fullscreen=yes, scrollbars=auto')"">" & row("nomarchivo") & "</a>"
            Case 2
                reto = "<a href=""javascript:;""  onclick=""window.open('cuadros/userauditoria.aspx?cod=" & row("docusuario") & "&fechasystem=" & row("docfechasystem") & "&F=" & row("nombre") & "', 'mywindow', 'location=0,status=0,scrollbars=1, width=950,height=370')"">" & row("docusuario") & "</a>"
        End Select
        Return reto
    End Function

    Private Sub consulta(ByVal opcion As String)
        Dim cmd As String = ""
        Dim fecharadic As Date
        Dim fecharadic2 As Date

        If opcion = "rango" Then
            fecharadic = FechaWebLocal(txtFechaRad.Text)
            fecharadic2 = FechaWebLocal(txtFechaRad2.Text)

            Me.Validator.ErrorMessage = fecharadic
            Me.Validator.IsValid = False

            cmd = "SELECT top 1000 documentos.docusuario, usuarios.nombre,documentos.docfechasystem, documentos.entidad, entesdbf.nombre AS nomente, documentos.idacto,actuaciones.nombre AS nomactuacion ,documentos.docpredio_refecatrastal,documentos.docexpediente ,documentos.paginas, documentos.nomarchivo,  documentos.ruta,  documentos.id, documentos.fecharadic " & _
            "FROM documentos inner join usuarios on documentos.docusuario = usuarios.codigo, entesdbf, actuaciones " & _
            "WHERE RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit) AND " & _
            "RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND " & _
            "docfechasystem BETWEEN '" & fecharadic & "' AND '" & fecharadic2 & " 11:59:59.998 PM' " & _
            "AND RTRIM(documentos.cobrador) = '" & Session("mcobrador") & "' " & _
            " ORDER BY docfechasystem desc,entidad, idacto, nomarchivo"
        ElseIf opcion = "hoy" Then
            fecharadic = FechaWebLocal(Date.Today.ToString("dd/MM/yyyy"))

            cmd = "SELECT top 1000 documentos.docusuario, usuarios.nombre,documentos.docfechasystem,documentos.entidad, entesdbf.nombre AS nomente, documentos.idacto,actuaciones.nombre AS nomactuacion ,documentos.docpredio_refecatrastal,documentos.docexpediente ,documentos.paginas, documentos.nomarchivo,  documentos.ruta,  documentos.id, documentos.fecharadic " & _
            "FROM documentos inner join usuarios on documentos.docusuario = usuarios.codigo, entesdbf, actuaciones " & _
            "WHERE RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit) AND " & _
            "RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND " & _
            "docfechasystem BETWEEN '" & fecharadic & "' AND '" & fecharadic & " 11:59:59.998 PM' " & _
            "AND RTRIM(documentos.cobrador) = '" & Session("mcobrador") & "' " & _
            " ORDER BY docfechasystem DESC, entidad, idacto, nomarchivo"
        ElseIf opcion = "ultimos90" Then
            cmd = "SELECT top 90 documentos.docusuario, usuarios.nombre,documentos.docfechasystem,documentos.entidad, entesdbf.nombre AS nomente, documentos.idacto,actuaciones.nombre AS nomactuacion ,documentos.docpredio_refecatrastal,documentos.docexpediente ,documentos.paginas, documentos.nomarchivo,  documentos.ruta,  documentos.id, documentos.fecharadic " & _
                  "FROM documentos inner join usuarios on documentos.docusuario = usuarios.codigo, entesdbf, actuaciones " & _
                  "WHERE RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit) AND " & _
                  "RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND " & _
                  "docfechasystem <= Getdate() " & _
                  "AND RTRIM(documentos.cobrador) = '" & Session("mcobrador") & "' " & _
                  " ORDER BY docfechasystem desc"
        End If

        Using Ado As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Using oDtAdapterSql As New SqlClient.SqlDataAdapter(cmd, Ado)
                Using myDatatable As New DataTable
                    oDtAdapterSql.Fill(myDatatable)
                    Call xtabla(myDatatable)
                End Using
            End Using
        End Using

        'Me.Validator.ErrorMessage = "<font color='#8A0808' >Error :" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
        'Me.Validator.IsValid = False
    End Sub
 
    Private Function FechaWebLocal(ByVal fecha As String) As Date
        Dim TipoConexion, dia, mes, anio As String
        Dim fechaserver As Date
        Dim fechacon As Date

        TipoConexion = ConfigurationManager.AppSettings("tipoconexion") 'local o web
        If TipoConexion = "web" Then
            dia = Left(fecha, 2)
            mes = Mid(fecha, 4, 2)
            anio = Mid(fecha, 7, 4)

            fechaserver = mes & "/" & dia & "/" & anio
            'fechaserver = Format(CDate(fecha), "MM/dd/yyyy")
            fechacon = CDate(fechaserver)
        Else
            fechacon = CDate(fecha)
        End If

        Return fechacon
    End Function
    
    Private Sub Ejecutarjavascript(ByVal script As String, ByVal NomScript As String)
        Dim csname1 As [String] = NomScript
        Dim cstype As Type = Me.[GetType]()

        Dim cs As ClientScriptManager = Page.ClientScript

        If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As String = script
            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())
        End If
    End Sub
End Class