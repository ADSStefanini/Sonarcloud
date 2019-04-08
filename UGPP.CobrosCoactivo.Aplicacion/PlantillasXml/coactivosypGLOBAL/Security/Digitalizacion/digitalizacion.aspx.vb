Imports System.IO
Imports System.Drawing.Imaging
Imports System.Data.SqlClient

Partial Public Class digitalizacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Response.Redirect("~/login.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then
            Dim Data As New DataTable
            Dim DatosConsultasTablas As New DatosConsultasTablas
            Data = DatosConsultasTablas.Load_ImpuestosEnte(Session("mcobrador"), Data)

            DropDownListTipo.DataTextField = "IMP_NOMBRE"
            DropDownListTipo.DataValueField = "IMP_VALUES"
            DropDownListTipo.DataSource = Data
            DropDownListTipo.DataBind()

            Data = New DataTable
            Data = DatosConsultasTablas.consultarUsuario(Session("sscodigousuario"), Session("mcobrador"), Data)
            _user.InnerHtml = "(" & Data.Rows(0).Item("CODIGO").ToString.Trim & ") " & Data.Rows(0).Item("NOMBRE").ToString.Trim.ToUpper

            ImagenesManager.InnerHtml = "<img src=""../Imagenes/archivoerr.png"" id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
            ViewState("guardar") = "insert"
        End If
    End Sub

    Private Sub GridExp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridExp.SelectedIndexChanged
        'En caso que el usuario desee actualizar permite ver las imagenes asociadas a lo escaneado.
        Dim tb As DataTable = CType(Me.ViewState("DatosExpediente"), DataTable)
        If Not tb.Rows(GridExp.SelectedIndex).IsNull("DOCACUMULACIO") Then
            txtAcum.Value = tb.Rows(GridExp.SelectedIndex).Item("DOCACUMULACIO")
        Else
            txtAcum.Value = Nothing
        End If

        txtactadmin.Value = tb.Rows(GridExp.SelectedIndex).Item("IDACTO")
        fechRac.Value = tb.Rows(GridExp.SelectedIndex).Item("FECHARADIC")

        If Not tb.Rows(GridExp.SelectedIndex).IsNull("DOCFECHADOC") Then
            fechDoc.Value = tb.Rows(GridExp.SelectedIndex).Item("DOCFECHADOC")
        Else
            fechDoc.Value = Nothing
        End If

        CheckDeshabilitado.Checked = tb.Rows(GridExp.SelectedIndex).Item("DOCANULAR")
        RadioButtonEstado.SelectedValue = tb.Rows(GridExp.SelectedIndex).Item("DOCACTIVOTRIBUTARIO")

        ListBox.DataTextField = "NOMARCHIVO"
        ListBox.DataSource = tb
        ListBox.DataBind()

        paginas.Items.Clear()
        ListBox.SelectedIndex = GridExp.SelectedIndex

        NewViewImg(ListBox.SelectedValue, Session("ssrutaexpediente"), 0)
        ViewState("File") = Session("ssrutaexpediente")
        ViewState("guardar") = "update"
    End Sub

    Protected Sub txtExpediente_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtExpediente.TextChanged
        Dim DatosConsultasTablas As New DatosConsultasTablas
        Using tb As New DataTable
            DatosConsultasTablas.Expedientes_Deudor(Session("mcobrador"), "", txtExpediente.Text, tb)
            If tb.Rows.Count > 0 Then
                If Not validarExpedientes(txtExpediente.Text.Trim) And Session("mnivelacces") <> 1 Then
                    ValidateUserExp()
                Else
                    Cancelar()
                txtid.Value = tb.Rows(0).Item("ENTIDAD")
                txtpredio.Value = tb.Rows(0).Item("DOCPREDIO_REFECATRASTAL")
                'Si el campo expediente trajo datos
                ViewState("NroExp") = "Si"

                Using tabla As New DataTable
                    DatosConsultasTablas.Load_Deudor(tb.Rows(0).Item("ENTIDAD"), tabla)
                    If tabla.Rows.Count > 0 Then
                        txtNombred.Value = tabla.Rows(0).Item("NOMBRE")
                    End If
                End Using

                LinkExaminarExpediente.Text = "Expediente (" & tb.Rows.Count & ")"
                GridExp.DataSource = tb
                GridExp.DataBind()
                    ViewState("DatosExpediente") = CType(tb, DataTable)
                End If
            Else
                ViewState("NroExp") = "No"
                LinkExaminarExpediente.Text = "Expediente (0)"
                If ListBox.Items.Count = 0 Then
                    paginas.Items.Clear()
                End If
                Cancelar()
            End If
        End Using
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "focusObjtxtid", "$(document).ready(function(){$('#txtid').focus();window.scrollBy(0, 0);});", True)
    End Sub

    Private Sub Cancelar()
        txtactadmin.Value = ""
        txtAcum.Value = ""
        txtid.Value = ""
        txtNombred.Value = ""
        txtpredio.Value = ""
        CheckDeshabilitado.Checked = False
    End Sub

    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub

    Private Sub LinkExaminarExpediente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkExaminarExpediente.Click
        If Not ViewState("NroExp") Is Nothing Then
            If Not txtExpediente.Text <> Nothing Then
                Alert("<b>No a digitado un número de expediente </b><br /><br />Utilice este botón para examinar los registros que pertenecen al expediente antes digitado (cuadro expediente).")
            Else
                If ViewState("NroExp") <> "No" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType(), "Grid", "$(function() {$('#gridexpedientes').dialog({hide: 'fold',width: 1000,autoOpen: true,modal: true,buttons: {'Cancelar': function() {$( this ).dialog( 'close' );}}});});", True)
                End If
            End If
        End If
    End Sub

    Private Sub LinkListaArchivadores_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkListaArchivadores.Click
        Using Command As New System.Data.SqlClient.SqlCommand("SELECT * FROM DOCUMENTO_ARCHIVADORES WHERE ARC_USUARIO = @ARC_USUARIO", New SqlClient.SqlConnection(Funciones.CadenaConexion))
            Command.Parameters.Add("@ARC_USUARIO", SqlDbType.VarChar).Value = Session("sscodigousuario")
            Dim Tabla As New DataTable
            Command.Connection.Open()
            Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
            Tabla.Load(reader)

            If Tabla.Rows.Count > 0 Then
                GridArchivos.DataSource = Tabla
                GridArchivos.DataBind()
                ViewState("Datos") = Tabla
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "Grid_arch", "$(function() {var dlgd_Arch = jQuery('#dialog-Archivadores').dialog({minHeight:100,maxHeight:330,width:410,autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}}); dlgd_Arch.dialog('option', 'position', [280, 40]); });", True)
            Else
                Alert("No existen directorios credos para ejecutar esta operación.")
            End If

            reader.Close()
            Command.Connection.Close()
        End Using
    End Sub

    Private Sub btnguardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnguardar.Click
        If ListBox.Items.Count > 0 Then
            If Not (txtExpediente.Text = Nothing Or txtactadmin.Value = Nothing Or txtpredio.Value = Nothing Or txtid.Value = Nothing) Then
                If Not txtactadmin.Value.Length = 3 Then
                    Alert("El <b>Acto Administrativo</b> consta de tres dígitos. <br /> <br /> <b>001</b> - GENERACIÓN DE CARÁTULA")
                    Exit Sub
                End If

                Dim table As DatasetForm.documentosDataTable
                Dim adap As New DatasetFormTableAdapters.documentosTableAdapter
                Dim rows As DatasetForm.documentosRow

                table = adap.GetDataBy_docexpediente_digi(txtExpediente.Text, txtactadmin.Value)
                'table = adap.GetDataBy_docexpediente (txtExpediente.Text, txtactadmin.Value, txtid.Value, txtAcum.Value)

                If table.Rows.Count > 0 Then
                    rows = table.Rows(0)
                Else
                    rows = table.NewdocumentosRow
                End If

                rows.entidad = txtid.Value
                rows.idacto = txtactadmin.Value
                rows.ruta = "ruta"
                rows.nomarchivo = ListBox.SelectedValue
                rows.paginas = paginas.Items.Count
                rows.fecharadic = fechRac.Value
                rows.cobrador = Session("mcobrador")
                rows.docexpediente = txtExpediente.Text
                rows.docproceso = txtExpediente.Text
                rows.docpredio_refecatrastal = txtpredio.Value
                rows.docacumulacio = txtAcum.Value
                rows.docfechadoc = fechDoc.Value
                rows.docObservaciones = "EXPEDIENTE GUARDADO POR  " & _user.InnerHtml & " >>>>>>> Digitalizacion"
                rows.docanular = CheckDeshabilitado.Checked
                rows.docusuario = Session("sscodigousuario")
                rows.docfechasystem = FechaWebLocal(Date.Now)
                rows.docActivotributario = RadioButtonEstado.SelectedValue
                rows.docimpuesto = DropDownListTipo.SelectedValue
                rows.docResolucion = ""
                rows.docActoPre = ""

                If table.Rows.Count = 0 Then
                    table.AdddocumentosRow(rows)
                End If

                'adap.Update(table)
                '--13/08/2013: Actualizar a tabla documento_ultimoacto, campo ULT_ACTO si txtactadmin = "036"--'
                If adap.Update(table) >= 1 Then
                    If Me.txtactadmin.Value.Trim = "036" Then
                        Using cnx As New SqlConnection(Funciones.CadenaConexion)

                            Dim cmd As New SqlCommand("UPDATE documento_ultimoacto SET ult_acto = '036' WHERE ult_expediente = @pExp", cnx)
                            cnx.Open()
                            cmd.Parameters.AddWithValue("@pExp", Me.txtExpediente.Text.Trim)
                            Dim ok As Integer = cmd.ExecuteNonQuery()

                        End Using
                    End If
                End If
                '----------------------------------------------------------------------------------------------'


                If ViewState("guardar") = "insert" Then
                    'Mover el fichero. si existe lo sobreescribe  
                    Dim sArchivoOrigen As String = ViewState("File") & "\" & ListBox.SelectedValue
                    Cortar_TIFF(sArchivoOrigen, Session("ssrutaexpediente") & "\" & ListBox.SelectedValue)

                    'Verificar que se queria hacer aca
                    If ListBox.Items.Count > 0 Then
                        'Aca se reduce el numero de iems 
                        ListBox.Items.Remove(ListBox.SelectedValue) '... y puede llegar a cero
                        paginas.Items.Clear()

                        If ListBox.Items.Count > 0 Then
                            ListBox.SelectedIndex = 0
                            NewViewImg(ListBox.SelectedValue, ViewState("File"), 0)
                        Else
                            ViewState("guardar") = "insert"
                            ImagenesManager.InnerHtml = "<img src=""../Imagenes/archivoerr.png"" id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
                        End If
                    ElseIf ListBox.Items.Count = 0 Then
                        ViewState("guardar") = "insert"
                        ImagenesManager.InnerHtml = "<img src=""../Imagenes/archivoerr.png"" id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
                    End If
                End If

                txtactadmin.Value = ""
                fechDoc.Value = ""
                fechRac.Value = ""
                txtAcum.Value = ""
                CheckDeshabilitado.Checked = False

                Using tbExpedientes_Deudor As New DataTable
                    Dim DatosConsultasTablas As New DatosConsultasTablas
                    DatosConsultasTablas.Expedientes_Deudor(Session("mcobrador"), "", txtExpediente.Text, tbExpedientes_Deudor)
                    LinkExaminarExpediente.Text = "Expediente (" & tbExpedientes_Deudor.Rows.Count & ")"
                    GridExp.DataSource = tbExpedientes_Deudor
                    GridExp.DataBind()
                    ViewState("DatosExpediente") = CType(tbExpedientes_Deudor, DataTable)
                End Using
            Else
                Alert("Debe digitalizar los datos asociados a la imagen para  guardar.")
            End If
        Else
            Alert("Para guardar favor elegir un archivo de las carpetas o escriba un número de expediente  y seleccione un archivo. <br/> <br/> <ol><li>Expediente</li><li>CC/Nit</li><li>Nombre Deudor</li><li>Predio</li><li>Acto Administrativo</li><li>Fecha Expedición</li><li>Fecha Radicación</li><li>Acumulacion</li><li>Tipo</li></ol>")
        End If
    End Sub

    Private Sub Cortar_TIFF(ByVal Path As String, ByVal Path2 As String)
        If File.Exists(Path2) Then
            File.Delete(Path2)
        End If

        File.Move(Path, Path2)
    End Sub

    Private Sub GridArchivos_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridArchivos.RowDeleting
        Dim mytable As New DataTable
        mytable = CType(ViewState("Datos"), DataTable)
        If mytable.Rows.Count > 0 Then
            With mytable.Rows.Item(e.RowIndex)
                Dim targetPath As String = Server.MapPath("Upload" & "\" & .Item("ARC_NOMBRE"))

                If (System.IO.Directory.Exists(targetPath)) Then
                    System.IO.Directory.Delete(targetPath, True)
                End If
                Dim ado As New SqlConnection(Funciones.CadenaConexion)
                ado.Open()
                Dim cmd As New SqlCommand
                cmd.CommandText = "DELETE DOCUMENTO_ARCHIVADORES WHERE ARC_COD = @ARC_COD"
                cmd.Parameters.Add("@ARC_COD", SqlDbType.VarChar).Value = .Item("ARC_COD")
                cmd.Connection = ado
                cmd.ExecuteNonQuery()
                Alert("Registro borrado con exito..")
            End With
        End If
    End Sub

    Private Sub GridArchivos_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridArchivos.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub GridArchivos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridArchivos.SelectedIndexChanged
        Dim mytable As New DataTable
        mytable = CType(ViewState("Datos"), DataTable)
        If mytable.Rows.Count > 0 Then
            'Dim index As Integer = GridArchivos.SelectedIndex + (GridArchivos.PageIndex * GridArchivos.PageSize)
            With mytable.Rows.Item(GridArchivos.SelectedIndex)
                Dim RutaDelDirectorio As String = Server.MapPath("Upload" & "\" & .Item("ARC_NOMBRE"))
                If System.IO.Directory.Exists(RutaDelDirectorio) Then
                    ViewState("File") = RutaDelDirectorio
                    ViewState("guardar") = "insert"
                    Dim tFiles() As String
                    tFiles = System.IO.Directory.GetFiles(RutaDelDirectorio)

                    Dim values As New ArrayList()
                    Dim fileName As String

                    For Each fileName In tFiles
                        values.Add(System.IO.Path.GetFileName(fileName))
                    Next fileName

                    ListBox.DataTextField = Nothing
                    ListBox.DataSource = values
                    ListBox.DataBind()

                    Call selectTab2()
                    paginas.Items.Clear()
                    If values.Count > 0 Then
                        ListBox.SelectedIndex = 0
                        NewViewImg(ListBox.SelectedValue, RutaDelDirectorio, 0)
                    End If
                Else
                    Alert("Este Archivador no existe fisicamente en el disco duro.")
                End If
            End With
        End If
    End Sub

    Private Sub selectTab2()
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Tab", "$(document).ready(function(e) {  $('#selectTabs2').click(); });", True)
    End Sub

    Function Extraer(ByVal Path As String) As String
        Return System.IO.Path.GetExtension(Path)
    End Function

    Private Sub NewViewImg(ByVal xFile As String, ByVal Directorio As String, ByVal pagina As Integer)
        Dim ArchivoaBuscar As String = Directorio & "\" & xFile

        If Not File.Exists(ArchivoaBuscar) Then
            Alert("El sistema no puede encontrar el archivo físico en el disco duro.")
            ImagenesManager.InnerHtml = "<img src=""../Imagenes/archivoerr.png"" id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
            Exit Sub
        End If

        Dim tipo_extension As String
        tipo_extension = Extraer(ArchivoaBuscar).Trim
        If tipo_extension.ToLower = "pdf" OrElse tipo_extension.ToLower = ".pdf" Then
            'ArchivoaBuscar = Server.UrlEncode(ArchivoaBuscar)
            Dim objectPdf As New StringBuilder
            objectPdf.Append("<object type='application/pdf' data='pdfGenerator.ashx?ImageFileName=")
            objectPdf.Append(ArchivoaBuscar)
            objectPdf.Append("' width='1000' height='1500' standby='Cargando...' wmode='transparent'>")
            objectPdf.Append("<param name='src' value='pdfGenerator.ashx?ImageFileName=")
            objectPdf.Append(ArchivoaBuscar)
            objectPdf.Append("'><param name='wmode' value='transparent'><embed type='application/pdf' width='1000' height='1500' wmode='transparent' pluginspage='http://www.adobe.com/products/acrobat/readstep2.html' src='")
            objectPdf.Append(ArchivoaBuscar)
            objectPdf.Append("'><noembed>Su navegador no es compatible con los archivos PDF incrustados.</noembed></embed></object>")
            ImagenesManager.InnerHtml = objectPdf.ToString
            paginas.Items.Clear()
            paginas.Items.Add(0)
            Exit Sub
        End If

        ImagenesManager.InnerHtml = "<img src=""../MostrarImagen.ashx?ImageFileName=" & ArchivoaBuscar & "&Item=" & pagina & """ id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
        If paginas.Items.Count = 0 Then
            loadImage(ArchivoaBuscar)
        End If
    End Sub

    Private Sub ListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox.SelectedIndexChanged
        If Not ViewState("File") = Nothing Then
            paginas.Items.Clear()
            Dim img As String = ListBox.SelectedValue
            NewViewImg(img, ViewState("File"), 0)
            Call selectTab2()
        End If
    End Sub

    Private Sub loadImage(ByVal strFilePath As String)
        Dim totFrame As Integer
        Dim objImage As System.Drawing.Image

        paginas.Items.Clear()
        objImage = Drawing.Image.FromFile(strFilePath)
        Dim objGuid As Guid = (objImage.FrameDimensionsList(0))
        Dim objDimension As System.Drawing.Imaging.FrameDimension = New System.Drawing.Imaging.FrameDimension(objGuid)

        totFrame = objImage.GetFrameCount(objDimension)
        Dim i As Integer
        For i = 0 To totFrame - 1
            paginas.Items.Add(i + 1)
        Next
        objImage.Dispose()
        objImage = Nothing
    End Sub

    Private Sub LinkSiguiente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkSiguiente.Click
        If Not ViewState("File") = Nothing Then
            Dim img As String = ListBox.SelectedValue
            Dim nroItem As Integer = paginas.Items.Count
            Dim aoR As Integer = paginas.SelectedIndex
            aoR += 1
            If Not aoR = nroItem Then
                paginas.SelectedIndex = aoR
                NewViewImg(img, ViewState("File"), aoR)
            End If
            Call selectTab2()
        End If
    End Sub

    Private Sub LinkAntes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkAntes.Click
        If Not ViewState("File") = Nothing Then
            Dim img As String = ListBox.SelectedValue
            Dim aoR As Integer = paginas.SelectedIndex
            aoR = aoR - 1
            If Not aoR = -1 Then
                paginas.SelectedIndex = aoR
                NewViewImg(img, ViewState("File"), aoR)
            End If
            Call selectTab2()
        End If
    End Sub

    Private Sub paginas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles paginas.SelectedIndexChanged
        If Not ViewState("File") = Nothing Then
            Dim img As String = ListBox.SelectedValue
            NewViewImg(img, ViewState("File"), paginas.SelectedIndex)
            Call selectTab2()
        End If
    End Sub

    Private Function validarExpedientes(ByVal exp As String) As Boolean
        Dim _Return As Boolean
        Using CommandTieneResol As New System.Data.SqlClient.SqlCommand("SELECT * FROM EJEFISGLOBAL WHERE EFIUSUASIG = @USUARIO AND EFINROEXP = @EXPEDIENTE AND EFIMODCOD = @IMPUESTOVALUE", New SqlConnection(Funciones.CadenaConexion))
            CommandTieneResol.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = exp
            CommandTieneResol.Parameters.Add("@IMPUESTOVALUE", SqlDbType.VarChar).Value = Session("ssimpuesto")
            CommandTieneResol.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = Session("sscodigousuario")
            Dim tbTieneResol As New DataTable
            CommandTieneResol.Connection.Open()
            Dim readerCommandTieneResol As System.Data.SqlClient.SqlDataReader = CommandTieneResol.ExecuteReader(CommandBehavior.CloseConnection)
            tbTieneResol.Load(readerCommandTieneResol)
            readerCommandTieneResol.Close()
            CommandTieneResol.Connection.Close()
            If tbTieneResol.Rows.Count > 0 Then
                _Return = True
            Else
                _Return = False
            End If
            Return _Return
        End Using

    End Function

    Private Sub ValidateUserExp()
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#validateUser').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub
End Class