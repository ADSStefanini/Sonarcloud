Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Partial Public Class prodeacumulado
    Inherits System.Web.UI.Page

    Private Sub LoadDatos()
        'Dim cnn As String = Session("ConexionServer")
        Dim Mytb As New DatasetForm
        Mytb.ProcesoAcumulado.AddProcesoAcumuladoRow("...Exp", "##/##/####", "", "")
        Mytb.ProcesoAcumulado.AddProcesoAcumuladoRow("...Exp", "##/##/####", "", "")
        Mytb.ProcesoAcumulado.AddProcesoAcumuladoRow("...Exp", "##/##/####", "", "")
        dtgAcumuladoCual.DataSource = Mytb.ProcesoAcumulado
        dtgAcumuladoCual.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.Form.Attributes.Add("autocomplete", "off")
            LoadDatos()

            Dim tipo As String = Request("tipo")
            If tipo = 1 Then
                txtEnte.Text = Request("deunom") & "::" & Request("cedula")
                btnAceptar_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim idEntidad As String
        idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
        Dim myclassconsul As New DatosConsultasTablas

        Using con As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim mytable As DatasetForm.ProcesoAcumuladoDataTable = myclassconsul.consultarExpedinetedeldeudor(Session("mcobrador"), con, idEntidad.Trim)

            dtgAcumuladoCual.DataSource = mytable
            dtgAcumuladoCual.DataBind()
            Me.ViewState("AcuDatos") = CType(mytable, DatasetForm.ProcesoAcumuladoDataTable)

            If mytable.Rows.Count > 0 Then
                Dim ver As Boolean = ChekaNull(mytable.Rows(0).Item("fecharadic"))

                If ver Then
                    Validator.Text = "<b>Error: </b> Registro invalido."
                    Me.Validator.IsValid = False

                    Dim amsbgox As String
                    amsbgox = "<h2 class='err'>ERROR</h2> Falta fecha de radicación. <br />" _
                    & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
                    & "<font color='#8A0808'>La fecha de radicación no se ha digitalizado.<br /> <b>Nota : si esta seguro que los datos estan bien y el error persiste, intete salir y entrar al sistema. </b> </font>"
                    menssageError(amsbgox)


                    btnResolAcu.Enabled = False
                    btnResolAcuManual.Enabled = False
                    Exit Sub
                End If

                btnResolAcu.Enabled = True
                btnResolAcuManual.Enabled = True

                dtgAcumuladoCual.SelectedIndex = 0
                Dim Chk As CheckBox = CType(Me.dtgAcumuladoCual.Rows(0).Cells(3).Controls(1), CheckBox)
                Chk.Checked = True
                lblAcumulado.Text = mytable.Rows(0).Item("docexpediente").trim
                lblfecharadic.Text = mytable.Rows(0).Item("fecharadic").trim
                Me.ViewState("Predio") = mytable.Rows(0).Item("docpredio_refecatrastal").trim
                lbldetalle.Text = (Num2Text(mytable.Rows.Count) & " expedientes detectados").ToUpper
            Else
                Call LoadDatos()
                Validator.Text = "No se detectaron datos. <br /> <b>identificacion : " & idEntidad & "</b>"
                Me.Validator.IsValid = False
            End If
        End Using
    End Sub

    Protected Sub dtgAcumuladoCual_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgAcumuladoCual.SelectedIndexChanged
        If Not Me.ViewState("AcuDatos") Is Nothing Then
            Dim TblDatos As DatasetForm.ProcesoAcumuladoDataTable = CType(Me.ViewState("AcuDatos"), DatasetForm.ProcesoAcumuladoDataTable)
            If TblDatos.Rows.Count > 0 Then
                Dim x As Integer
                For x = 0 To TblDatos.Rows.Count - 1
                    Dim Chk As CheckBox = CType(Me.dtgAcumuladoCual.Rows(x).Cells(3).Controls(1), CheckBox)
                    Chk.Checked = False
                Next

                Dim dChk As CheckBox = CType(dtgAcumuladoCual.Rows(Me.dtgAcumuladoCual.SelectedIndex).Cells(3).Controls(1), CheckBox)
                dChk.Checked = True
            End If
        End If
    End Sub

    Protected Sub btnResolAcuManual_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResolAcuManual.Click
        Dim amsbgox As String
        If Not Me.ViewState("Predio") Is Nothing Then
            Dim idEntidadx() As String = Split(Me.txtEnte.Text, "::")
            If idEntidadx.Length = 2 Then
                Dim deunom, cedula As String
                cedula = CType(idEntidadx(1), String)
                deunom = CType(idEntidadx(0), String)
                Response.Redirect("SiguientePaso.aspx?expediente=" & lblAcumulado.Text & "&refcatastral=" & Me.ViewState("Predio") & "&tipo=2&cedula=" & cedula & "&deunom=" & deunom & "&utilpas=057&pasoselect=057&descripselect=RESOLUCIÓN DE ACUMULACIÓN")
            Else
                amsbgox = "<h2 class='err'>ERROR</h2> Identificación o deudor no valido, inténtelo otra vez.<br />" _
                   & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
                   & "<font color='#8A0808' >Seleccionar uno en particular con flecha abajo o con el mouse.<br /> <b>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
                menssageError(amsbgox)
            End If
        Else
            amsbgox = "<h2 class='err'>ERROR</h2> Acaba de ejecutar uno operación no valida.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "<font color='#8A0808' >Digite un deudor y prosiga presionar el botón CONSULTAR.<br /> <b>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
            menssageError(amsbgox)
        End If
    End Sub

    Protected Sub btnResolAcu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResolAcu.Click
        Try
            Using con As New SqlConnection(Session("ConexionServer").ToString)
                With Me
                    Dim da As New SqlDataAdapter("select * from documentos WHERE docexpediente = @Documento and fecharadic = @fecharadic", con)
                    da.SelectCommand.Parameters.Add("@Documento", SqlDbType.VarChar).Value = lblAcumulado.Text
                    da.SelectCommand.Parameters.Add("@fecharadic", SqlDbType.DateTime).Value = lblfecharadic.Text
                    Dim xMytb As New DatasetForm.documentosDataTable
                    da.Fill(xMytb)
                    Me.ViewState("datos") = xMytb


                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                    ' Creamos los comandos con el CommandBuilder
                    Dim cb As New SqlCommandBuilder(da)

                    da.InsertCommand = cb.GetInsertCommand()
                    da.UpdateCommand = cb.GetUpdateCommand()
                    'da.DeleteCommand = cb.GetDeleteCommand()

                    con.Open()
                    Dim tran As SqlTransaction = con.BeginTransaction

                    da.InsertCommand.Transaction = tran
                    da.UpdateCommand.Transaction = tran
                    'da.DeleteCommand.Transaction = tran

                    Dim Mytb As New DatasetForm.documentosDataTable
                    Mytb = CType(.ViewState("datos"), DatasetForm.documentosDataTable)

                    If Mytb Is Nothing Then
                        Mytb = New DatasetForm.documentosDataTable
                    End If
                    Dim idEntidad As String
                    idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)

                    If Mytb.Select("docexpediente='" & lblAcumulado.Text & "' and idacto='" & Request("pasoselect") & "'").Length > 0 Then
                        'En esta parate se procede a actualizar el registro si existe 
                        Dim Row As DatasetForm.documentosRow = Mytb.Select("docexpediente='" & lblAcumulado.Text & "' and idacto='" & Request("pasoselect") & "'")(0)
                        If Not Row Is Nothing Then
                            'Row.NOMBRE = .txtNombre.Text
                            'Row.CEDULA = .txtCodigo.Text
                            'Row.DIRECCION = txtDireccion.Text
                            'Row.TELEFONO = .txtTelefono.Text
                        End If
                    Else
                        Dim predio As String = xMytb.Rows(0).Item("docpredio_refecatrastal")
                        Mytb.AdddocumentosRow(idEntidad, "057", "ruta", "ACU" & lblAcumulado.Text & ".Pdf", "1", FechaWebLocal(Date.Now), Session("mcobrador").ToString, lblAcumulado.Text, lblAcumulado.Text, predio.Trim, lblAcumulado.Text, FechaWebLocal(Date.Now), "RESOLUCIÓN DE ACUMULACIÓN ", False, Session("sscodigousuario"), FechaWebLocal(Date.Now), "S", Session("ssimpuesto"), Nothing, Nothing)
                    End If

                    Try
                        ' Actualizamos los datos de la tabla
                        da.Update(Mytb)
                        tran.Commit()
                        reporte("057", lblAcumulado.Text, idEntidad)
                        Me.ViewState("Predio") = Nothing
                    Catch ex As Exception
                        tran.Rollback()
                    End Try
                    con.Close()
                End With
            End Using
        Catch ex As Exception
            Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Acceso invalido o conexión no habilitada.<br />" _
            & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
            & "<font color='#8A0808' >" & ex.Message & " <br /> <b>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
            menssageError(amsbgox)
        End Try
    End Sub

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Private Sub reporte(ByVal acto As String, ByVal expediente As String, ByVal idEntidad As String)
        Dim selectcod As String = acto

        Dim MytableRepo As Boolean
        Dim DtsDatos As Reportes_Admistratiivos = New Reportes_Admistratiivos

        'Dim Ado As New SqlConnection(Funciones.CadenaConexionUnion)
        Dim AdoME As New SqlConnection(Funciones.CadenaConexion)

        Dim myBaseClass As New procesos_tributario.sqlQuery.sqlQuery(AdoME)

        myBaseClass.Impuesto = Session("ssimpuesto")
        myBaseClass.Expedinete = expediente
        myBaseClass.Repo = selectcod
        myBaseClass.Datos_Ente = DtsDatos.CAT_CLIENTES
        myBaseClass.Datos_Informe = DtsDatos._Resolucion_acumulacion_
        myBaseClass.Ente = Session("mcobrador")
        myBaseClass.ConexionME = AdoME
        myBaseClass.EnteDeudorPropietario = idEntidad

        MytableRepo = myBaseClass.Prima_EjecucionFiscal(Session("ssCodimpadm"), 1)

        If DtsDatos._Resolucion_acumulacion_.Rows.Count > 0 Then
            Dim cr As CrystalDecisions.CrystalReports.Engine.ReportDocument = myBaseClass.QueReporte(selectcod)
            'Funciones.Exportar(Me, cr, DtsDatos, "Impresion.Pdf", "")
            Dim NameFile As String = Exportar_Reportes_Masivos(Me, cr, DtsDatos, "ACU" & expediente & ".Pdf", "")
            Funciones.ForzarMostraPDF(Me, NameFile)
            'Ejecutarjavascript(Page, "<script ""text/javascript"">window.open('" & NameFile & "','Graph','left=0,top=0,fullscreen=yes, scrollbars=auto');</script>", "Reporte")

            Validator.Text = "Consulta satisfactoria. <br /> <b>IMPRIMIO : (" & selectcod & ") " & selectcod & "</b>"
            Me.Validator.IsValid = False
        Else
            Validator.Text = "No hay datos para mostrar."
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
        DiskOpts.DiskFileName = Session("ssrutaexpediente") & "\" & NameFile
        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        Report.Close()
        Report.Dispose()
        Report = Nothing

        Return DiskOpts.DiskFileName
    End Function
End Class