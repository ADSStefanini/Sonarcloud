Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Partial Public Class cobranzasMasiva
    Inherits System.Web.UI.Page

    Private Sub fechoy()
        Dim fecha As DateTime = Date.Now
        Dim fechaFormateada As String = FormatDateTime(fecha, DateFormat.LongDate)
        lblfechahoy.Text = fechaFormateada
    End Sub

    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SqlDataSource1.ConnectionString = Funciones.CadenaConexion
        If Not Me.Page.IsPostBack Then
            Page.Form.Attributes.Add("autocomplete", "off")
            If Session("ConexionServer") = Nothing Then
                Session("ConexionServer") = Funciones.CadenaConexion
            End If
            Call CargarDatoas(Session("mcobrador"))

            Call ultimo()
            Call fechoy()

            Dim tipo As String
            tipo = Request("tipo")
            If tipo <> Nothing Then
                txtValor.Text = Request("val")
                chkVista.Checked = True
            End If
        End If
    End Sub

    Private Sub CargarDatoas(ByVal Ente As String)
        Dim ConsultaTable As New DatosConsultasTablas
        Dim MyTable As New DatasetForm.entescobradoresDataTable
        MyTable = ConsultaTable.EntesCobradores(Session("mcobrador"))
        ViewState("DatosEnte") = MyTable
    End Sub

    Private Sub ultimo()
        Dim myadapter As New SqlDataAdapter("SELECT * FROM MAESTRO_CONSECUTIVOS WHERE CON_IDENTIFICADOR = 6", Session("ConexionServer").ToString)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1
        lblcodigo.Text = Format(Con, "00000000")
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        If chkVista.Checked Then
            If IsNumeric(txtValor.Text) Then
                Response.Redirect("deudoresactivos.aspx?tipo=1&val=" & txtValor.Text.Trim)
            Else

            End If
        Else
            Try
                Dim selectcod As String = Lista.SelectedValue

                Dim MytableRepo As Boolean
                Dim DtsDatos As Reportes_Admistratiivos = New Reportes_Admistratiivos

                Dim Ado As New SqlConnection(Funciones.CadenaConexionUnion)
                Dim AdoME As New SqlConnection(Funciones.CadenaConexion)

                Dim myBaseClass As New procesos_tributario.sqlQuery.sqlQuery(Ado)

                'myBaseClass.Expedinete = lblExpediente.Text
                myBaseClass.Impuesto = Session("ssimpuesto")
                myBaseClass.Repo = selectcod
                myBaseClass.Datos_Ente = DtsDatos.CAT_CLIENTES
                myBaseClass.Datos_Informe = DtsDatos.Mandaniento_Pago
                myBaseClass.Ente = Session("mcobrador")
                myBaseClass.ConexionME = AdoME

                MytableRepo = myBaseClass.Prima_EjecucionFiscal_Masivos(True, Val(txtValor.Text), Session("ssCodimpadm"), 2)

                If DtsDatos.Mandaniento_Pago.Rows.Count > 0 Then
                    Dim cr As CrystalDecisions.CrystalReports.Engine.ReportDocument = myBaseClass.QueReporte(selectcod)

                    Dim MyTableEnte As New DatasetForm.entescobradoresDataTable
                    MyTableEnte = CType(ViewState("DatosEnte"), DatasetForm.entescobradoresDataTable)

                    Dim List As New List(Of ItemParams)
                    For Each Par As CrystalDecisions.Shared.ParameterField In cr.ParameterFields
                        Select Case Par.Name
                            Case "Localidad"
                                List.Add(New ItemParams("Localidad", MyTableEnte.Rows(0).Item("ent_localidad")))
                            Case "DirLocalidad"
                                List.Add(New ItemParams("DirLocalidad", MyTableEnte.Rows(0).Item("ent_direccionlocalidad")))
                            Case Else
                                List.Add(New ItemParams(Par.Name, "Sin Valor"))
                        End Select
                    Next

                    Dim Test As String = ""

                    Test = doc_Documento(7)
                    If Test = "no" Then
                        Validator.Text = "Consulta satisfactoria. <br /> Nota: No se pude  generar el documento, verificar permisos de sesión. </b>"
                        Me.Validator.IsValid = False
                        Exit Sub
                    End If
                    '
                    'datogen.InnerHtml = "Generando archivo ""PDF"""
                    lblcodigo.Text = Test
                    totexp.InnerHtml = DtsDatos.Mandaniento_Pago.Rows.Count & " de expedientes impresos"
                    ViewState("Arch") = Exportar_Reportes_Masivos(Me, cr, DtsDatos, Test & ".pdf", "")

                    Validator.Text = "Consulta satisfactoria. <br /> <b>IMPRIMIO : (" & selectcod & ") " & Lista.SelectedItem.Text & "</b>"
                    Me.Validator.IsValid = False
                    Alert(Validator.Text)

                    datos_base(DtsDatos, Test)
                Else
                    Validator.Text = "No hay datos para mostrar."
                    Me.Validator.IsValid = False
                    Alert(Validator.Text)
                End If
            Catch ex As Exception
                Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Es posible que el reporte este inhabilitado."
                Me.Validator.IsValid = False
                Alert(Validator.Text)
            End Try
        End If
    End Sub

    Public Function doc_Documento(ByVal consecutivo As Integer) As String
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction

            Dim proximo_numero As String = "00000000"
            Dim mycommand As New SqlCommand("UPDATE MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = @conse", con)
            mycommand.Parameters.Add("@proximo_numero", SqlDbType.Int)
            mycommand.Parameters.Add("@conse", SqlDbType.Int).Value = consecutivo
            mycommand.Parameters("@proximo_numero").Direction = ParameterDirection.Output
            mycommand.Transaction = tran
            mycommand.ExecuteNonQuery()
            Dim conse As Integer = CType(mycommand.Parameters("@proximo_numero").Value, Integer)
            proximo_numero = Format(conse, "00000000")

            Try
                tran.Commit()
            Catch ex As Exception
                tran.Rollback()
                Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Error de base de dato  o la  sesión caduco."
                Me.Validator.IsValid = False
                proximo_numero = "no"
            End Try

            Return proximo_numero
        End Using
    End Function

    Private Function archivo_grande(ByVal doc As String) As Boolean
        Dim bool As Boolean = False
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            Dim da As New SqlDataAdapter("SELECT * FROM DOCUMENTO_MASIVO_HEAD WHERE DELL_DOCUMENTO = @MAN_DOCUMENTO", con)
            da.SelectCommand.Parameters.Add("@MAN_DOCUMENTO", SqlDbType.VarChar, 8).Value = doc

            Dim cb As New SqlCommandBuilder(da)
            da.InsertCommand = cb.GetInsertCommand()
            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction
            da.InsertCommand.Transaction = tran
            Dim mitabladestino As New DatasetForm.DOCUMENTO_MASIVO_HEADDataTable
            Dim selectcod As String = Lista.SelectedValue
            mitabladestino.AddDOCUMENTO_MASIVO_HEADRow(doc, Date.Now, "Expediente masivo generado automáticamente <--> " & lblfechahoy.Text.ToUpper.Trim, Session("sscodigousuario"), Session("mcobrador"), Session("ssimpuesto"), False, selectcod)

            da.Update(mitabladestino)

            ViewState("DOCUMENTO_MASIVO_HEAD") = CType(mitabladestino, DatasetForm.DOCUMENTO_MASIVO_HEADDataTable)

            Try
                tran.Commit()
                bool = True
            Catch ex As Exception
                tran.Rollback()
                Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Error de base de dato  o la  sesión caduco. ."
                Me.Validator.IsValid = False
                bool = False
            End Try
        End Using

        Return bool
    End Function

    Private Sub datos_base(ByVal dtb As Reportes_Admistratiivos, ByVal doc As String)
        Dim bool As Boolean
        bool = archivo_grande(doc)
        If bool = False Then
            Exit Sub
        End If

        Using con As New SqlConnection(Session("ConexionServer").ToString)
            Dim proximo_numero As String = lblcodigo.Text
            Dim da As New SqlDataAdapter("SELECT * FROM DOCUMENTO_MASIVO WHERE MAN_DOCUMENTO = @MAN_DOCUMENTO", con)
            da.SelectCommand.Parameters.Add("@MAN_DOCUMENTO", SqlDbType.VarChar, 8).Value = lblcodigo.Text
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey

            Dim cb As New SqlCommandBuilder(da)
            da.InsertCommand = cb.GetInsertCommand()
            da.UpdateCommand = cb.GetUpdateCommand()
            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction
            da.InsertCommand.Transaction = tran
            da.UpdateCommand.Transaction = tran

            Dim row As DataRow
            Dim mitabladestino As New DatasetForm.DOCUMENTO_MASIVODataTable

            For Each row In dtb.Mandaniento_Pago
                mitabladestino.AddDOCUMENTO_MASIVORow(valorNull(row("MAN_DEUSDOR"), Nothing), valorNull(row("MAN_IMPUESTO"), Nothing), valorNull(row("MAN_VALORMANDA"), Nothing), valorNull(row("MAN_NOMDEUDOR"), Nothing), valorNull(row("MAN_DIRECCION"), Nothing), valorNull(row("MAN_DIR_ESTABL"), Nothing), valorNull(row("MAN_REFCATRASTAL"), Nothing), valorNull(row("MAN_VIGENCIA"), Nothing), valorNull(row("MAN_CONCEPTOCDG"), Nothing), valorNull(row("MAN_ESTRATOCD"), Nothing), valorNull(row("MAN_DESTINOCD2"), Nothing), valorNull(row("MAN_BASEGRAVABLE"), Nothing), valorNull(row("MAN_TARIFA"), Nothing), valorNull(row("MAN_CAPITAL"), Nothing), valorNull(row("MAN_INTERESES"), Nothing), valorNull(row("MAN_TOTAL"), Nothing), valorNull(row("MAN_EXPEDIENTE"), Nothing), valorNull(row("MAN_FECHADOC"), Nothing), valorNull(row("MAN_EFIPERDES"), Nothing), valorNull(row("MAN_EFIPERHAS"), Nothing), valorNull(row("MAN_PAGOS"), Nothing), valorNull(row("MAN_FECHARAC"), FechaWebLocal(Date.Now)), proximo_numero, False, (lblfechahoy.Text & "(Documento masivo automático)").ToUpper, Session("mcobrador"))
            Next

            Try
                da.Update(mitabladestino)
                tran.Commit()
            Catch ex As Exception
                tran.Rollback()
                Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Error de base de dato  o la  sesión caduco..."
                Me.Validator.IsValid = False
            End Try

            Call ultimo()
        End Using
    End Sub

    Protected Sub LinkArchivo_expediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkArchivo_expediente.Click
        If Me.ViewState("Arch") <> Nothing Then
            Response.Redirect("cuadros/download.ashx?documento=" & ViewState("Arch"))
        Else
            Validator.Text = "Error : Archivo no encontrado."
            Me.Validator.IsValid = False
        End If
    End Sub

    Private Function Exportar_Reportes_Masivos(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
                   ByVal Datos As Object, ByVal NameFile As String, ByVal Path As String) As String
        Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New PdfRtfWordFormatOptions

        ExportOpts = Report.ExportOptions
        ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile
        DiskOpts.DiskFileName = Page.MapPath(Path) & "\temp_arch\reportes_masivos\" & NameFile
        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        Report.Close()
        Report.Dispose()
        Report = Nothing

        LinkArchivo_expediente.Text = "Descargar archivo aquí (" & NameFile & ")"

        Return DiskOpts.DiskFileName
    End Function

    Protected Sub btnSeperar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSeperar.Click
        Ejecutarjavascript(Me, "<script ""text/javascript"">window.open('cuadros/SepararEquipos.aspx?documento=" & ViewState("Arch") & "','Graph1','status=0,left=170,top=120,width=850,height=330,scrollbars=auto');</script>", "Reporte")
    End Sub

    Protected Sub btnImprnew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprnew.Click
        Response.Redirect("nuevosprocesomasivo.aspx")
    End Sub

End Class