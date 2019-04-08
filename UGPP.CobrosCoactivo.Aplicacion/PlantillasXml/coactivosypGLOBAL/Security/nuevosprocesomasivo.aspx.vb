Imports System.Data.SqlClient
Imports CrystalDecisions.Shared

Partial Public Class nuevosprocesomasivo
    Inherits System.Web.UI.Page

    Private Sub fechoy()
        Dim fecha As DateTime = Date.Now
        Dim fechaFormateada As String = FormatDateTime(fecha, DateFormat.LongDate)
        lblfechahoy.Text = fechaFormateada
    End Sub

    Function SelectDataTable(ByVal dt As DataTable, ByVal filter As String, ByVal sort As String) As DataTable
        Dim rows As DataRow()
        Dim dtNew As DataTable
        ' copy table structure
        dtNew = dt.Clone()
        ' sort and filter data
        rows = dt.Select(filter, sort)
        ' fill dtNew with selected rows
        Dim x As Integer
        For Each dr As DataRow In rows
            x += 1
            dtNew.ImportRow(dr)
            If x = 200 Then
                Exit For
            End If
        Next
        ' return filtered dt
        Return dtNew
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Using mytable As New Schema.Masivos_Pendientes_formDataTable
                Dim clasemas As New procesos_tributario.cobronuevo.cobronuevo
                clasemas.Impuesto_ente = Session("ssimpuesto")
                clasemas.dedures_no_procesos(mytable, Funciones.CadenaConexionUnion, Nothing)

                Dim dtNew As Schema.Masivos_Pendientes_formDataTable
                dtNew = SelectDataTable(mytable, "TOTALPAGAR > 0", "TOTALPAGAR DESC")

                GridView.DataSource = dtNew
                GridView.DataBind()
                ejDetalle.Text = UCase("Se detectaron " & Funciones.Num2Text(mytable.Rows.Count) & " registros.")
                lblnroregistros.Text = mytable.Rows.Count

                Me.ViewState("datos_fun") = CType(dtNew, Schema.Masivos_Pendientes_formDataTable)

                Dim valor As String = String.Format("{0:C}", mytable.Compute("sum(TOTALPAGAR)", "TOTALPAGAR > 0"))
                lbltotal.Text = valor
                Dim sumObject As Object = mytable.Compute("MIN(ANNIOMIN)  + ' - ' +  MAX(ANNIOMAX)  ", "TOTALPAGAR > 0")
                lblvigencias.Text = valorNull(sumObject, "Null")

                Call ultimo()
                Call fechoy()
            End Using
        End If
    End Sub

    Protected Sub dtgViewActos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgViewActos.SelectedIndexChanged
        With Me
            If txtValor.Text = Nothing Then
                txtValor.Text = 0
            End If

            Dim Mytb As DataTable = CType(.ViewState("datos"), DataTable)
            Dim index As Integer = dtgViewActos.SelectedIndex
            Dim Acto As String = Mytb.Rows(index).Item("DEP_CODACTO")
            lblacto.Text = Acto
            Dim MytableRepo As Boolean
            Dim DtsDatos As Reportes_Admistratiivos = New Reportes_Admistratiivos
            Dim Ado As New SqlConnection(Funciones.CadenaConexionUnion)
            Dim AdoME As New SqlConnection(Funciones.CadenaConexion)

            Dim myBaseClass As New procesos_tributario.cobronuevo.cobronuevo
            myBaseClass.Impuesto_ente = Session("ssimpuesto")
            myBaseClass.Impuesto = Session("ssimpuesto")
            myBaseClass.Impuesto_Literal = Session("ssCodimpadm")
            myBaseClass.Datos_Ente = DtsDatos.CAT_CLIENTES
            myBaseClass.Datos_Informe = DtsDatos.Mandaniento_Pago
            myBaseClass.Ente = Session("mcobrador")
            myBaseClass.Conexion = Ado
            myBaseClass.ConexionME = AdoME
            myBaseClass.ActoNuevoPro = Acto
            myBaseClass.MaxVigen = Val(txtVigencias.Text)
            myBaseClass.TotaValor = CType(txtValor.Text, Double)
            myBaseClass.Estrato = Val(txtEstrato.Text)

            MytableRepo = myBaseClass.Prima_EjecucionFiscal_Masivos(True, CType((txtValor.Text), Double), Session("ssCodimpadm"), 2)

            If DtsDatos.Mandaniento_Pago.Rows.Count > 0 Then
                Dim cr As CrystalDecisions.CrystalReports.Engine.ReportDocument = myBaseClass.QueReporte(Acto)
                Dim Test As String = ""

                Test = doc_Documento(6)
                If Test = "no" Then
                    Validator.Text = "Consulta satisfactoria. <br /> Nota: No se pude  generar el documento, verificar permisos de sesión. </b>"
                    Me.Validator.IsValid = False
                    Exit Sub
                End If
                '
                'datogen.InnerHtml = "Generando archivo ""PDF"""
                lblcodigo.Text = Test
                totexp.InnerHtml = DtsDatos.Mandaniento_Pago.Rows.Count & " de expedientes impresos"
                ViewState("Arch") = Funciones.Exportar_Reportes_Masivos(Me, cr, DtsDatos, Test & ".pdf", "")

                LinkArchivo_expediente.Text = "Descargar archivo aquí (" & Test & ".pdf" & ")"

                Validator.Text = "Consulta satisfactoria. <br /> <b>IMPRIMIO : (" & Mytb.Rows(index).Item("DEP_CODACTO") & ") " & Mytb.Rows(index).Item("DEP_NOMBREPPAL") & "</b>"
                Me.Validator.IsValid = False
                datos_base(DtsDatos, Test)
            Else
                Validator.Text = "No hay datos para mostrar."
                Me.Validator.IsValid = False
            End If
        End With
    End Sub

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
                Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Error de base de dato  o la  sesión caduco. ."
                Me.Validator.IsValid = False
            End Try

            Call ultimo()
        End Using
    End Sub

    Private Sub ultimo()
        Dim myadapter As New SqlDataAdapter("select * from MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 6", Session("ConexionServer").ToString)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1
        lblcodigo.Text = Format(Con, "00000000")
    End Sub

    Private Function archivo_grande(ByVal doc As String) As Boolean
        Dim bool As Boolean = False
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            Dim da As New SqlDataAdapter("SELECT * FROM DOCUMENTO_MASIVO_HEAD WHERE DELL_DOCUMENTO = @MAN_DOCUMENTO", con)
            da.SelectCommand.Parameters.Add("@MAN_DOCUMENTO", SqlDbType.Char, 8).Value = doc

            Dim cb As New SqlCommandBuilder(da)
            da.InsertCommand = cb.GetInsertCommand()
            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction
            da.InsertCommand.Transaction = tran
            Dim mitabladestino As New DatasetForm.DOCUMENTO_MASIVO_HEADDataTable
            Dim selectcod As String = lblacto.Text
            mitabladestino.AddDOCUMENTO_MASIVO_HEADRow(doc, Date.Now, "Expediente masivo generado automáticamente <--> FECHA: " & lblfechahoy.Text.ToUpper.Trim & " <--> HORA: " & DateTime.Now.ToString("hh:mm:ss ttt"), Session("sscodigousuario"), Session("mcobrador"), Session("ssimpuesto"), False, selectcod)

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

    Public Function doc_Documento(ByVal consecutivo As Integer) As String
        Using con As New SqlConnection(Session("ConexionServer").ToString)
            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction

            Dim proximo_numero As String = "00000000"
            Dim mycommand As New SqlCommand("update MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = @conse", con)
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
                Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Error de base de dato  o la  sesión caduco. ."
                Me.Validator.IsValid = False
                proximo_numero = "no"
            End Try

            Return proximo_numero
        End Using
    End Function

    Protected Sub LinkArchivo_expediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkArchivo_expediente.Click
        If Me.ViewState("Arch") <> Nothing Then
            Response.Redirect("cuadros/download.ashx?documento=" & ViewState("Arch"))
        Else
            Validator.Text = "Error : Archivo no encontrado."
            Me.Validator.IsValid = False
        End If
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Try
            Dim ultomo As String = ViewState("UltimoPaso")
            Dim sql As String = "SELECT top 1 (DEP_DEPENDENCIA + '-' + DEP_DESCRIPCION) ULTIMO,DEP_CODACTO,DEP_NOMBREPPAL,DEP_TERMINO FROM DEPENDENCIA_ACTUACIONES"
            Dim myadapter As New SqlClient.SqlDataAdapter(sql, Funciones.CadenaConexion)
            Dim mytb As New DataTable
            myadapter.Fill(mytb)

            If mytb.Rows.Count > 0 Then
                ActoAdmind.InnerHtml = mytb.Rows(0).Item("ULTIMO")
                dtgViewActos.DataSource = mytb
                dtgViewActos.DataBind()

                Me.ViewState("datos") = CType(mytb, DataTable)
            Else

            End If
            Me.ModalPopupExtender2.Show()
        Catch ex As Exception
            'Validator.Text = "Error : " & ex.Message
            'Me.Validator.IsValid = False
        End Try
    End Sub
   
    Private Sub GridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView.PageIndexChanging
        Dim grilla As GridView = CType(sender, GridView)
        With grilla
            .PageIndex = e.NewPageIndex()
        End With

        GridView.DataSource = CType(Me.ViewState("datos_fun"), Schema.Masivos_Pendientes_formDataTable)
        GridView.DataBind()
    End Sub

    Protected Sub btnSeperar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSeperar.Click
        Ejecutarjavascript(Me, "<script ""text/javascript"">window.open('cuadros/SepararEquipos.aspx?documento=" & ViewState("Arch") & "','Graph1','status=0,left=170,top=120,width=850,height=330,scrollbars=auto');</script>", "Reporte")
    End Sub

    Protected Sub Linkvalor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Linkvalor.Click
        Using mytable As New Schema.Masivos_Pendientes_formDataTable
            Dim SQL As String = Nothing

            If txtValor.Text = Nothing Then
                txtValor.Text = 0
            End If

            Dim clasemas As New procesos_tributario.cobronuevo.cobronuevo
            clasemas.Impuesto_ente = Session("ssimpuesto")
            clasemas.MaxVigen = Val(txtVigencias.Text)
            clasemas.TotaValor = CType(txtValor.Text, Double)
            clasemas.Estrato = Val(txtEstrato.Text)
            Me.ViewState("datos_fun") = Nothing
            clasemas.dedures_no_procesos(mytable, Funciones.CadenaConexionUnion, SQL)
            Dim valor As String = String.Format("{0:C}", mytable.Compute("sum(TOTALPAGAR)", "TOTALPAGAR > 0"))
            Dim sumObject As Object = mytable.Compute("MIN(ANNIOMIN)  + ' - ' +  MAX(ANNIOMAX)  ", "TOTALPAGAR > 0")

            lblvigencias.Text = valorNull(sumObject, "Null")

            lbltotal.Text = valor
            GridView.DataSource = mytable
            GridView.DataBind()

            ejDetalle.Text = UCase("Se detectaron " & Funciones.Num2Text(mytable.Rows.Count) & " registros.")
            lblnroregistros.Text = mytable.Rows.Count
            Me.ViewState("datos_fun") = CType(mytable, Schema.Masivos_Pendientes_formDataTable)
        End Using
    End Sub
End Class