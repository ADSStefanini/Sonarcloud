Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Public Class SiguientePaso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Page.Form.Attributes.Add("autocomplete", "off")
            Dim tipo As String = Request("tipo")
            ejeFiscales.Attributes.Add("style", "position:absolute;top:50px; height:105px; width: 717px; left:38px; display:block;")
            Dim repexpediente As String = Request("expediente")
            Dim refcatastral As String = Request("refcatastral")

            ejDeudor.Text = Request("cedula")
            ejExpediente.Text = Request("expediente")
            ejPredio.Text = Request("refcatastral")
            ejdeuNombre.Text = Request("deunom")
            ejutilpas.Text = Request("utilpas")
            ActoAdmind.InnerHtml = Request("pasoselect") & " - " & Request("descripselect")

            If tipo = 1 Then
                If repexpediente <> Nothing Then
                    linkHistorial.Attributes("href") = "historiaexpediente.aspx?expediente=" & ejExpediente.Text & "&refcatastral=" & ejPredio.Text & "&tipo=1&cedula=" & ejDeudor.Text & "&deunom=" & ejdeuNombre.Text & "&utilpas=" & ejutilpas.Text
                End If
            ElseIf tipo = 2 Then
                ejecucionf.Attributes.Add("style", "visibility: hidden;")
                linkHistorial.Attributes("href") = "prodeacumulado.aspx?expediente=" & ejExpediente.Text & "&refcatastral=" & ejPredio.Text & "&tipo=1&cedula=" & ejDeudor.Text & "&deunom=" & ejdeuNombre.Text & "&utilpas=" & ejutilpas.Text
                txtObservaciones.Text = ("Documento acumulado registrado manualmente").ToUpper
            End If

            Using CommandTieneResol As New System.Data.SqlClient.SqlCommand("SELECT * FROM ACTUACIONES WHERE GENERADOC = 1 AND CODIGO = @CODIGO", New SqlConnection(Funciones.CadenaConexion))
                CommandTieneResol.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = CType(Request("pasoselect"), String)
                Dim tbTieneResol As New DataTable
                CommandTieneResol.Connection.Open()
                Dim readerCommandTieneResol As System.Data.SqlClient.SqlDataReader = CommandTieneResol.ExecuteReader(CommandBehavior.CloseConnection)
                tbTieneResol.Load(readerCommandTieneResol)
                readerCommandTieneResol.Close()
                CommandTieneResol.Connection.Close()
                If tbTieneResol.Rows.Count > 0 Then
                    Using Command As New System.Data.SqlClient.SqlCommand("SELECT DG_ID,DG_EXPEDIENTE,DG_COD_ACTO,A.NOMBRE,DG_NRO_DOC,DG_FECHA_DOC,DG_ESTADO FROM DOCUMENTOS_GENERADOS, ACTUACIONES A WHERE DG_EXPEDIENTE = @DOCEXPEDIENTE AND DG_COD_ACTO = @DG_COD_ACTO AND A.CODIGO = DG_COD_ACTO AND GENERADOC = 1", New SqlConnection(Funciones.CadenaConexion))
                        Command.Parameters.Add("@DOCEXPEDIENTE", SqlDbType.VarChar).Value = ejExpediente.Text
                        Command.Parameters.Add("@DG_COD_ACTO", SqlDbType.VarChar).Value = CType(Request("pasoselect"), String)
                        Dim tb As New DataTable
                        Command.Connection.Open()
                        Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                        tb.Load(reader)
                        reader.Close()
                        Command.Connection.Close()

                        If tb.Rows.Count > 0 Then
                            If Not tb.Rows(0).IsNull("DG_NRO_DOC") Then
                                txtResolucion.Text = tb.Rows(0).Item("DG_NRO_DOC")
                                lblresolucion_ok.Text = "<img src='images/icons/accept.png' alt='resolucion' width='16' height='16' align='absmiddle' style='vertical-align:middle;' />"
                            End If
                        Else
                            txtResolucion.Text = "SIN GENERAR"
                            lblresolucion_ok.Text = "<img src='images/icons/sign_cacel.png' alt='resolucion' width='16' height='16' align='absmiddle' style='vertical-align:middle;' /> <span>Res. no encontrada.</span>"
                        End If
                    End Using
                Else
                    txtResolucion.Text = "NO APLICA"
                    lblresolucion_ok.Text = "<img src='images/icons/accept.png' alt='resolucion' width='16' height='16' align='absmiddle' style='vertical-align:middle;' /> <span>No Aplica Res.</span>"
                End If
            End Using
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            Dim archivo1 As Object
            Using con As New SqlConnection(Session("ConexionServer").ToString)
                With Me
                    Dim da As New SqlDataAdapter("select * from documentos WHERE docexpediente = @Documento", con)
                    da.SelectCommand.Parameters.Add("@Documento", SqlDbType.VarChar).Value = .ejExpediente.Text
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

                    If Mytb.Select("docexpediente='" & .ejExpediente.Text & "' and idacto='" & Request("pasoselect") & "'").Length > 0 Then
                        'En esta parate se procede a actualizar el registro si existe 
                        Dim Row As DatasetForm.documentosRow = Mytb.Select("docexpediente='" & .ejExpediente.Text & "' and idacto='" & Request("pasoselect") & "'")(0)
                        If Not Row Is Nothing Then
                            'Row.NOMBRE = .txtNombre.Text
                            'Row.CEDULA = .txtCodigo.Text
                            'Row.DIRECCION = txtDireccion.Text
                            'Row.TELEFONO = .txtTelefono.Text
                        End If
                    Else
                        Dim doc As String
                        If Request("pasoselect") = "057" Then 'Acto que identifica la acumulacion 
                            doc = Request("expediente")
                        Else
                            doc = ""
                        End If
                        archivo1 = Trim(NomArchivoTiff_or_Pdf(Path.GetExtension(imagen1.PostedFile.FileName)))
                        Mytb.AdddocumentosRow(ejDeudor.Text, Request("pasoselect"), "ruta", archivo1, txtnroPaginas.Text, txtFechaRad.Text, Session("mcobrador").ToString, ejExpediente.Text, ejExpediente.Text, ejPredio.Text, doc, txtFechacreacion.Text, txtObservaciones.Text, False, Session("sscodigousuario"), FechaWebLocal(Date.Now), "S", Session("ssimpuesto"), txtResolucion.Text, Request("utilpas").Substring(0, 3))
                        ''Mytb.AdddocumentosRow(ejDeudor.Text, Request("pasoselect"), "ruta", archivo1, txtnroPaginas.Text, txtFechaRad.Text, Session("mcobrador").ToString, ejExpediente.Text, ejExpediente.Text, ejPredio.Text, doc, txtFechacreacion.Text, txtObservaciones.Text, False, Session("sscodigousuario"), FechaWebLocal(Date.Now), "S", Session("ssimpuesto"), txtResolucion.Text, Request("utilpas"))
                    End If

                    Try
                        Dim Adap As New SqlDataAdapter("SELECT fecharadic FROM documentos where idacto = @idacto and docexpediente= @expediente", Funciones.CadenaConexion)
                        Adap.SelectCommand.Parameters.Add("@expediente", SqlDbType.VarChar).Value = .ejExpediente.Text
                        Adap.SelectCommand.Parameters.Add("@idacto", SqlDbType.VarChar).Value = ejutilpas.Text.Substring(0, 3)
                        Dim _Table As New DataTable
                        Adap.Fill(_Table)

                        If CDate(_Table.Rows(0).Item(0)).ToString("dd-MM-yyyy") <= CDate(txtFechaRad.Text).ToString("dd-MM-yyyy") And CDate(_Table.Rows(0).Item(0)).ToString("dd-MM-yyyy") <= CDate(txtFechacreacion.Text).ToString("dd-MM-yyyy") Then
                            ' Actualizamos los datos de la tabla
                            da.Update(Mytb)
                            tran.Commit()

                            Dim Destino As String = Session("ssrutaexpediente") & "\"
                            ' Directory  
                            If Not Directory.Exists(Destino) Then
                                Directory.CreateDirectory(Destino)
                            End If
                            'Si no se ha seleccionado ninguna carpeta mostrar un mensaje
                            imagen1.PostedFile.SaveAs(Destino & archivo1)
                            UpdateUltimoacto(ejExpediente.Text, Request("pasoselect"), Request("descripselect"))
                            archivoGenerado.InnerHtml = archivo1
                            Me.ModalPopupExtender2.Show()
                        Else
                            Validator.Text = "La fecha de radicación y la fecha de documento no pueden ser inferior a la fecha de radicación del ultimo acto"
                            Me.Validator.IsValid = False
                        End If
                    Catch ex As Exception
                        tran.Rollback()
                        'Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
                    End Try
                    con.Close()
                End With
            End Using
        Catch ex As Exception
            Validator.Text = "Error : " & ex.Message
            Me.Validator.IsValid = False
        End Try
    End Sub

    Private Sub UpdateUltimoacto(ByVal EXPEDIENTE As String, ByVal ULTIMOACTO As String, ByVal NOMBREULTIMOACTO As String)
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim Command As SqlCommand
            Command = New SqlCommand

            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            Command.Connection = con
            Command.CommandText = "UPDATE documento_ultimoacto SET  ULT_ACTO = @ULTIMOACTO , ULT_ACTODESCRIP = @NOMBREULTIMOACTO WHERE ULT_EXPEDIENTE = @EXPEDIENTE"
            Command.Parameters.AddWithValue("@EXPEDIENTE", EXPEDIENTE)
            Command.Parameters.AddWithValue("@ULTIMOACTO", ULTIMOACTO)
            Command.Parameters.AddWithValue("@NOMBREULTIMOACTO", NOMBREULTIMOACTO)
            Command.ExecuteNonQuery()
            con.Close()
        End Using
    End Sub

    Private Sub btnSi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSi.Click
        Dim tipo As String = Request("tipo")
        If tipo = "2" Then
            If ejPredio.Text = Nothing Then
                ejPredio.Text = Request("refcatastral")
            End If

            Response.Redirect("historiaexpediente.aspx?expediente=" & ejExpediente.Text & "&refcatastral=" & ejPredio.Text & "&tipo=1&cedula=" & ejDeudor.Text & "&deunom=" & ejdeuNombre.Text & "&utilpas=" & ejutilpas.Text & "&acupp=si")
            Exit Sub
        End If

        Response.Redirect("historiaexpediente.aspx?expediente=" & ejExpediente.Text & "&refcatastral=" & ejPredio.Text & "&tipo=1&cedula=" & ejDeudor.Text & "&deunom=" & ejdeuNombre.Text & "&utilpas=" & ejutilpas.Text)
    End Sub

    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
        Response.Redirect(linkHistorial.Attributes("href"))
    End Sub
End Class