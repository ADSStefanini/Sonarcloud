Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Partial Public Class SepararEquipos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Call initpage()
        End If
    End Sub

    Private Sub initpage()
        Dim SQL As String = "SELECT DELL_DOCUMENTO,DELL_ACTO, A.NOMBRE, DELL_FECHA ,E.NOMBRE AS NOMBREMUNI FROM DOCUMENTO_MASIVO_HEAD, ACTUACIONES A, ENTESCOBRADORES E WHERE DELL_ACTO = A.CODIGO AND E.CODIGO = DELL_COBRADOR AND DELL_SEPARADO <> 1 AND DELL_COBRADOR = @DELL_COBRADOR"
        Using con As New SqlConnection(Funciones.CadenaConexion.ToString)
            Using da As New SqlDataAdapter(SQL, con)
                da.SelectCommand.Parameters.Add("@DELL_COBRADOR", SqlDbType.Char, 3).Value = Session("mcobrador")
                Using mitabladestino As New DataTable
                    da.Fill(mitabladestino)

                    lbldetalle.Text = "Se detectaron " & Num2Text(mitabladestino.Rows.Count) & " documento(s)"
                    GriDatos.DataSource = mitabladestino
                    GriDatos.DataBind()
                    ViewState("Datos") = CType(mitabladestino, DataTable)
                    ViewState("Count") = mitabladestino.Rows.Count
                    lblProceso.Text = "Elija un documento y presione el botón ""Separar Archivos"""
                End Using
            End Using
        End Using
    End Sub

    Private Function Exportar_Reportes_Masivos(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
                 ByVal Datos As Reportes_Admistratiivos, ByVal NameFile As String, ByVal Path As String) As String

        Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New PdfRtfWordFormatOptions

        ExportOpts = Report.ExportOptions
        ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile

        If Path <> Nothing Then
            DiskOpts.DiskFileName = Path & "\" & NameFile
        Else
            DiskOpts.DiskFileName = Server.MapPath("~") & "\Security\temp_arch\reportes_masivos\" & NameFile
        End If

        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Report = Nothing

        Return DiskOpts.DiskFileName
    End Function

    Protected Function Load_Configuracion(ByVal Ente As String, ByVal DtsDatos As Reportes_Admistratiivos.CAT_CLIENTESDataTable) As Reportes_Admistratiivos.CAT_CLIENTESDataTable
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            Dim ESAdap As New SqlDataAdapter
            Dim ESCommand As SqlCommand
            ESCommand = New SqlCommand

            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            ESCommand.Connection = con
            ESCommand.CommandText = "SELECT codigo as ID_CLIENTE, nombre as NOMBRE,ent_foto as FOTO,ent_pref_exp,ent_pref_res,ent_tesorero, ent_firma,ent_foto2,ent_foto3 FROM entescobradores WHERE codigo = @Cobrador"
            ESCommand.Parameters.Add("@Cobrador", SqlDbType.VarChar)
            ESCommand.Parameters("@Cobrador").Value = Ente

            ESAdap.SelectCommand = ESCommand
            ESAdap.Fill(DtsDatos)

            Return DtsDatos
        End Using
    End Function

    Protected Sub btnSeperar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSeperar.Click
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            If ViewState("select") <> Nothing Then
                Dim Documento As String = ViewState("select").ToString.Trim

                Dim da As New SqlDataAdapter("SELECT * FROM DOCUMENTO_MASIVO WHERE MAN_DOCUMENTO = @MAN_DOCUMENTO", con)
                da.SelectCommand.Parameters.Add("@MAN_DOCUMENTO", SqlDbType.VarChar, 8).Value = Documento
                Dim mitabladestino As New DatasetForm.DOCUMENTO_MASIVODataTable
                da.Fill(mitabladestino)

                Dim myBaseClass As New procesos_tributario.sqlQuery.sqlQuery()
                myBaseClass.Impuesto = Session("ssimpuesto")
                myBaseClass.Impuesto_Literal = Session("ssCodimpadm")

                Dim selectcod As String = ViewState("selectacto")
                Dim acto As String = ViewState("selectacto")
                Dim nomActo As String = ViewState("selectactonombre")
                Dim Test As String = ""

                Dim cr As CrystalDecisions.CrystalReports.Engine.ReportDocument = myBaseClass.QueReporte(selectcod)
                Dim DtsDatos As Reportes_Admistratiivos = New Reportes_Admistratiivos
                Load_Configuracion(Session("mcobrador"), DtsDatos.CAT_CLIENTES)

                Dim cobranzasMasiva As New cobranzasMasiva
                Dim row As DataRow
                Dim Mytbdocumento As New DatasetForm.documentosDataTable
                Dim Mydocumento_ultimoacto As New DatasetForm.documento_ultimoactoDataTable
                Dim Datos_documento_ultimoacto As New DatasetForm.documento_ultimoactoDataTable
                Datos_ultimo_paso(Datos_documento_ultimoacto)

                If mitabladestino.Rows.Count > 0 Then
                    Dim TblEjFis As DataSet1.EJEFISGLOBALDataTable = New DataSet1.EJEFISGLOBALDataTable

                    For Each row In mitabladestino.Rows
                        'Registrar en ejecuciones fiscales
                        Dim RowEjFis As DataSet1.EJEFISGLOBALRow = TblEjFis.NewEJEFISGLOBALRow
                        For Each Column As DataColumn In mitabladestino.Columns ' TblEjFis.Columns
                            Dim Col As DataColumn = Funciones.Column_Ejecucion_Fiscal(Column.Caption)
                            If Not Col Is Nothing Then
                                RowEjFis(Col.Caption) = row(Column.Caption)
                            End If
                        Next
                        TblEjFis.Rows.Add(Llenar_Fila_Ejecuciones_Fiscales(RowEjFis, acto, Session("ssimpuesto")))

                        Test = cobranzasMasiva.doc_Documento(6)
                        DtsDatos.Mandaniento_Pago.Clear()
                        Dim NewRows As DataRow = addnewRows(row, DtsDatos.Mandaniento_Pago, "TE" & Test & Date.Now.ToUniversalTime.ToString("ddMMyyyyHHmmss") & ".pdf", acto, Mytbdocumento, Documento, nomActo)
                        DtsDatos.Mandaniento_Pago.Rows.Add(NewRows)

                        If Test = "no" Then
                            'Validator.Text = "Consulta satisfactoria. <br /> Nota: No se pude  generar el documento, verificar permisos de sesión. </b>"
                            'Me.Validator.IsValid = False
                            'Exit Sub
                        End If

                        ViewState("Arch") = Exportar_Reportes_Masivos(Me, cr, DtsDatos, "TE" & Test & Date.Now.ToUniversalTime.ToString("ddMMyyyyHHmmss") & ".pdf", Session("ssrutaexpediente"))
                    Next
                    'Registrar en ejecuciones fiscales Insert Into
                    Funciones.InsertDocEjecucionesFiscales(TblEjFis)

                    updateDoc(Session("mcobrador"), Documento, Mytbdocumento, Datos_documento_ultimoacto)

                    'recargar la pagina
                    initpage()
                    lblProceso.Attributes.Add("style", "color:#045FB4")
                    lblProceso.Text = "Archivo separado con éxito"
                Else
                    lblProceso.Attributes.Add("style", "color:#8A0808")
                    lblProceso.Text = "El documento esta generado, pero no hay archivos para separar."
                End If
            Else
                lblProceso.Attributes.Add("style", "color:#8A0808")
                lbldetalle.Text = "Se detectaron " & Num2Text(ViewState("Count")) & " documento(s) --------- Expediente :: Seleccione un exp. para comenzar."
            End If
        End Using
    End Sub

    Private Function addnewRows(ByVal xrow As DataRow, ByVal xDatos As DataTable, ByVal xArchivo As String, ByVal Acto As String, ByVal Mytbdocumento As DatasetForm.documentosDataTable, ByVal Documento As String, ByVal NomActo As String) As DataRow
        Dim row As Reportes_Admistratiivos.Mandaniento_PagoRow
        row = xDatos.NewRow

        row("MAN_DEUSDOR") = xrow("MAN_DEUSDOR")
        row("MAN_IMPUESTO") = xrow("MAN_IMPUESTO")
        row("MAN_VALORMANDA") = xrow("MAN_VALORMANDA")
        row("MAN_NOMDEUDOR") = xrow("MAN_NOMDEUDOR")
        row("MAN_DIRECCION") = Nothing
        row("MAN_DIR_ESTABL") = xrow("MAN_DIR_ESTABL")
        row("MAN_REFCATRASTAL") = xrow("MAN_REFCATRASTAL")
        row("MAN_VIGENCIA") = Nothing
        row("MAN_CONCEPTOCDG") = Nothing
        row("MAN_ESTRATOCD") = Nothing
        row("MAN_DESTINOCD2") = Nothing
        row("MAN_BASEGRAVABLE") = Nothing
        row("MAN_TARIFA") = DBNull.Value
        row("MAN_CAPITAL") = DBNull.Value
        row("MAN_INTERESES") = xrow("MAN_INTERESES")
        row("MAN_TOTAL") = xrow("MAN_TOTAL")
        row("MAN_EXPEDIENTE") = xrow("MAN_EXPEDIENTE")
        row("MAN_FECHADOC") = DBNull.Value
        row("MAN_EFIPERDES") = xrow("MAN_EFIPERDES")
        row("MAN_EFIPERHAS") = xrow("MAN_EFIPERHAS")
        row("MAN_PAGOS") = xrow("MAN_PAGOS")
        row("MAN_FECHARAC") = DBNull.Value

        'Registrar documento 
        Mytbdocumento.AdddocumentosRow(valorNull(xrow("MAN_DEUSDOR"), Nothing), Acto.Trim, "ruta", Trim(xArchivo).ToLower(), "1", FechaWebLocal(Date.Today), Session("mcobrador").ToString, valorNull(xrow("MAN_EXPEDIENTE"), Nothing), valorNull(xrow("MAN_EXPEDIENTE"), Nothing), valorNull(xrow("MAN_REFCATRASTAL"), Nothing), "0", FechaWebLocal(Date.Today), Documento, False, Session("sscodigousuario"), FechaWebLocal(Date.Now), "S", Session("ssimpuesto"), Nothing, Nothing)
        'Mydocumento_ultimoacto.Adddocumento_ultimoactoRow(valorNull(xrow("MAN_EXPEDIENTE"), Nothing), Acto, NomActo, FechaWebLocal(Date.Today), valorNull(xrow("MAN_DEUSDOR"), Nothing))

        Return row
    End Function
    Private Sub ModUtilActo(ByVal Datos_documento_ultimoacto As DatasetForm.documento_ultimoactoDataTable, ByVal Expedente As String, ByVal Acto As String)
        Dim Mytb As DatasetForm.documento_ultimoactoDataTable = Datos_documento_ultimoacto
        If Mytb.Select("ULT_EXPEDIENTE='" & Expedente.Trim & "'").Length > 0 Then
            'En esta parate se procede a actualizar el registro si existe 
            Dim Row As DatasetForm.documento_ultimoactoRow = Mytb.Select("ULT_EXPEDIENTE='" & Expedente.Trim & "'")(0)
            If Not Row Is Nothing Then
                Row.ULT_ACTO = Acto
            End If
        End If
    End Sub

    Private Function Datos_ultimo_paso(ByVal Datos_documento_ultimoacto As DatasetForm.documento_ultimoactoDataTable) As DatasetForm.documento_ultimoactoDataTable
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            Dim da As New SqlDataAdapter("select * from documento_ultimoacto", con)
            da.Fill(Datos_documento_ultimoacto)

            Return Datos_documento_ultimoacto
        End Using
    End Function

    Private Sub UpdateUtilpaso(ByVal Mydocumento_ultimoacto As DatasetForm.documento_ultimoactoDataTable)
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            With Me
                Dim da As New SqlDataAdapter("select * from documento_ultimoacto", con)

                da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                Dim cb As New SqlCommandBuilder(da)

                ' da.InsertCommand = cb.GetInsertCommand()
                da.UpdateCommand = New SqlCommand("UPDATE documento_ultimoacto SET ULT_EXPEDIENTE = @ULT_EXPEDIENTE, ULT_ACTO = @ULT_ACTO, ULT_ACTODESCRIP= @ULT_ACTODESCRIP, ULT_FECHA = @ULT_FECHA, ULT_DEUDOR = @ULT_DEUDOR WHERE ULT_EXPEDIENTE = @ULT_EXPEDIENTE", con)
                da.UpdateCommand.Parameters.Add("@ULT_EXPEDIENTE", SqlDbType.VarChar, 50, "ULT_EXPEDIENTE")
                da.UpdateCommand.Parameters.Add("@ULT_ACTO", SqlDbType.VarChar, 3, "ULT_ACTO")
                da.UpdateCommand.Parameters.Add("@ULT_ACTODESCRIP", SqlDbType.VarChar, 300, "ULT_ACTODESCRIP")
                da.UpdateCommand.Parameters.Add("@ULT_FECHA", SqlDbType.DateTime, 50, "ULT_FECHA")
                da.UpdateCommand.Parameters.Add("@ULT_DEUDOR", SqlDbType.VarChar, 50, "ULT_DEUDOR")
                'da.UpdateCommand.UpdatedRowSource = UpdateRowSource.None

                Dim row As DataRow
                For Each row In Mydocumento_ultimoacto.Rows
                    row("ULT_ACTO") = ViewState("selectacto")
                Next

                con.Open()
                Dim tran As SqlTransaction = con.BeginTransaction
                'da.InsertCommand.Transaction = tran
                da.UpdateCommand.Transaction = tran

                Try

                    da.Update(Mydocumento_ultimoacto)
                    tran.Commit()
                Catch ex As Exception
                    tran.Rollback()
                End Try
            End With
            con.Close()
        End Using
    End Sub

    Private Sub updateDoc(ByVal Cobrador As String, ByVal Documento As String, ByVal Mytbdocumento As DatasetForm.documentosDataTable, ByVal Mydocumento_ultimoacto As DatasetForm.documento_ultimoactoDataTable)
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            Dim ESCommand As SqlCommand
            ESCommand = New SqlCommand

            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            ESCommand.Connection = con
            ESCommand.CommandText = "UPDATE DOCUMENTO_MASIVO_HEAD SET DELL_SEPARADO = 1 WHERE DELL_DOCUMENTO = @Documento AND DELL_COBRADOR = @Cobrador"
            ESCommand.Parameters.Add("@Cobrador", SqlDbType.VarChar).Value = Cobrador
            ESCommand.Parameters.Add("@Documento", SqlDbType.VarChar).Value = Documento
            ESCommand.ExecuteNonQuery()

            Funciones.InsertDocDefinitivo(Documento, Mytbdocumento)
            'ultimo paso OJO
        End Using
    End Sub

    Protected Sub GriDatos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GriDatos.SelectedIndexChanged
        Dim mytable As New DataTable
        mytable = CType(ViewState("Datos"), DataTable)
        If mytable.Rows.Count > 0 Then
            Dim index As Integer = GriDatos.SelectedIndex
            ViewState("select") = mytable.Rows(index).Item("DELL_DOCUMENTO")
            ViewState("selectacto") = mytable.Rows(index).Item("DELL_ACTO")
            ViewState("selectactonombre") = mytable.Rows(index).Item("NOMBRE")
            lbldetalle.Text = "Se detectaron " & Num2Text(mytable.Rows.Count) & " documento(s) --------- <a href=""#"" class='rel'>Expediente :: " & ViewState("select") & "</b></a>"
            lblProceso.Attributes.Add("style", "color:#045FB4")
        End If
    End Sub

    Private Sub GriDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GriDatos.PageIndexChanging
        Dim grilla As GridView = CType(sender, GridView)
        With grilla
            .PageIndex = e.NewPageIndex()
        End With

        GriDatos.DataSource = CType(Me.ViewState("Datos"), DataTable)
        GriDatos.DataBind()
    End Sub



    

End Class