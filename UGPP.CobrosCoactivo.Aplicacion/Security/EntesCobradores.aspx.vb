'*****************************************************************************************
'* Leyton Manuel Espitia Diaz © (2011)                                                  **
'*****************************************************************************************
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Data.SqlClient
Partial Public Class EntesCobradores
    Inherits System.Web.UI.Page

#Region " Variables"
    Dim fsFoto As FileStream
    Dim curFileName As String = ""
    Dim Swt As Byte = 0
    Dim mas As Byte = 0
    Dim ancho As Integer
    Dim alto As Integer
#End Region

    Private Sub cargeCobrador()
        Dim Table As DatasetForm.entescobradoresDataTable = LoadDatos()
        Me.ViewState("DatosUser") = Table
        GridCobradores.DataSource = Table
        GridCobradores.DataBind()
    End Sub

    Private Function LoadDatos() As DataTable
        Dim cnn As String = Session("ConexionServer")
        Dim MyAdapter As New SqlDataAdapter("select * from  entescobradores", cnn)
        Dim Mytb As New DatasetForm.entescobradoresDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

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

        If Not IsPostBack Then
            If Session("ConexionServer") = Nothing Then
                Session("ConexionServer") = Funciones.CadenaConexion()
            End If
            cargeCobrador()
            Call ultimo()

            Dim tipo As String = Request("tipo")
            If tipo = "1" Then
                Dim file As String = Request("file")
                Dim con As String = Request("con")
                Dim nom As String = Request("nom")
                Dim rut As String = Request("rut")
                txtCodigo.Text = con
                txtNombre.Text = nom
                txtArchivadores.Text = rut

                titleimg.InnerHtml = "<table><tr><th>Imagen :</th><td>" & Extraer(file, "\") & "</td></tr></table>"
                ImagenesManager.InnerHtml = "<img src=""MostrarImagen.ashx?ImageFileName=" & file & "&Item=" & 0 & """ id=""myimg"" width=""1000"" height=""1500"" class=""disenno"" />" & "<br /><br />"
                ModalPopupError.OnCancelScript = "imgview()"
                Messenger.InnerHtml = "<font color='#ffffff'><b>Informacion : </b>Si desea continuar con otro proceso, prosiga presionando el botón cancelar.</font>"

                ModalPopupExtender2.Show()
            End If
        End If
    End Sub
    Function Extraer(ByVal Path As String, ByVal Caracter As String) As String
        Dim ret As String
        If Caracter = "." And InStr(Path, Caracter) = 0 Then
            Return ""
            Exit Function
        End If

        ret = Right(Path, Len(Path) - InStrRev(Path, Caracter))

        ' -- Retorna el valor  
        Extraer = ret
    End Function
    Private Sub ultimo()
        Dim myadapter As New SqlDataAdapter("select * from MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 5", Session("ConexionServer").ToString)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1
        txtCodigo.Text = Format(Con, "00")
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."
            ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel()"
            menssageError(amsbgox)
            Exit Sub
        End If

        Dim LogProc As New LogProcesos

        Try
            If txtNombre.Text <> Nothing And Directory.Exists(txtArchivadores.Text) Then
                Dim proximo_numero As String
                Dim ado As New SqlClient.SqlConnection(Funciones.CadenaConexion)
                If ado.State = ConnectionState.Open Then
                    ado.Close()
                End If
                ado.Open()

                Dim tran As SqlTransaction = ado.BeginTransaction

                Dim cmdFoto As New SqlCommand
                cmdFoto.Connection = ado

                Dim SqlData As New SqlDataAdapter("select * from  entescobradores WHERE (codigo = @codigo)", Funciones.CadenaConexion)
                SqlData.SelectCommand.Parameters.Add("@codigo", System.Data.SqlDbType.VarChar).Value = txtCodigo.Text
                '" & txtCodigo.Text & "'
                Dim tb As New DataTable
                SqlData.Fill(tb)
                If tb.Rows.Count = 0 Then
                    '-----------Consecutivo
                    Dim mycommand As New SqlCommand("update MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = 5", ado)
                    mycommand.Parameters.Add("@proximo_numero", SqlDbType.Int)
                    mycommand.Parameters("@proximo_numero").Direction = ParameterDirection.Output
                    mycommand.Transaction = tran
                    mycommand.ExecuteNonQuery()

                    '----------------
                    'Después de cada GRABAR hay que llamar al log de auditoria
                    'Dim LogProc As New LogProcesos
                    LogProc.SaveLog(Session("ssloginusuario"), "Actualización de consecutivos", "CON_IDENTIFICADOR = 5 ", mycommand)
                    '----------------

                    Dim conse As Integer = CType(mycommand.Parameters("@proximo_numero").Value, Integer)
                    proximo_numero = Format(conse, "00")

                    '----------Registro
                    cmdFoto.CommandText = "INSERT INTO entescobradores VALUES(@codigo,@nombre,@IMAGEN,@FIRMA,@Ruta,@RutaLocal,@ent_direccionlocalidad,@ent_telefonoslocalidad,@ent_localidad,@ent_foto2,@ent_foto3)"
                    cmdFoto.Parameters.Add("@codigo", System.Data.SqlDbType.VarChar).Value = proximo_numero
                    cmdFoto.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar).Value = txtNombre.Text.ToUpper
                    cmdFoto.Parameters.Add("@IMAGEN", System.Data.SqlDbType.Image).Value = System.Data.SqlTypes.SqlBinary.Null
                    cmdFoto.Parameters.Add("@FIRMA", System.Data.SqlDbType.Image).Value = System.Data.SqlTypes.SqlBinary.Null

                    cmdFoto.Parameters.Add("@ent_foto2", System.Data.SqlDbType.Image).Value = System.Data.SqlTypes.SqlBinary.Null
                    cmdFoto.Parameters.Add("@ent_foto3", System.Data.SqlDbType.Image).Value = System.Data.SqlTypes.SqlBinary.Null

                    cmdFoto.Parameters.Add("@Ruta", System.Data.SqlDbType.VarChar).Value = txtArchivadores.Text
                    cmdFoto.Parameters.Add("@RutaLocal", System.Data.SqlDbType.Bit).Value = System.Data.SqlTypes.SqlBinary.Null
                    cmdFoto.Parameters.Add("@ent_direccionlocalidad", System.Data.SqlDbType.VarChar).Value = Nothing
                    cmdFoto.Parameters.Add("@ent_telefonoslocalidad", System.Data.SqlDbType.VarChar).Value = Nothing
                    cmdFoto.Parameters.Add("@ent_localidad", System.Data.SqlDbType.VarChar).Value = Nothing
                    cmdFoto.Transaction = tran
                Else
                    cmdFoto.CommandText = "UPDATE entescobradores SET nombre=@nombre, ent_ruta=@Ruta WHERE (codigo = @codigo)"
                    '" & txtCodigo.Text.Trim & "'
                    cmdFoto.Parameters.Add("@nombre", SqlDbType.VarChar).Value = txtNombre.Text.Trim.ToUpper
                    cmdFoto.Parameters.Add("@codigo", SqlDbType.VarChar).Value = txtCodigo.Text.Trim
                    cmdFoto.Parameters.Add("@Ruta", System.Data.SqlDbType.VarChar).Value = txtArchivadores.Text
                    cmdFoto.Transaction = tran
                End If

                Try
                    cmdFoto.ExecuteNonQuery()

                    '----------------
                    'Después de cada GRABAR hay que llamar al log de auditoria
                    'Dim LogProc As New LogProcesos
                    LogProc.SaveLog(Session("ssloginusuario"), "Actualización de entes", "CODIGO = '01' ", cmdFoto)
                    '----------------

                    tran.Commit()
                    Session("ssrutaexpediente") = txtArchivadores.Text
                    Call cancelar()
                Catch ex As Exception
                    tran.Rollback()
                    Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
                End Try
            Else
                Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>Datos obligatorios para continuar. <br /> a) Favor digitar el nombre. b) Directorio valido o periférico con permisos.</font>"
            End If
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try
    End Sub

    Private Sub Ejecutar_Reporte(ByVal COD As String)
        Try
            Dim Ado As New SqlConnection(Funciones.CadenaConexion)
            Dim sqlData As New SqlDataAdapter("SELECT codigo as ID_CLIENTE, nombre as NOMBRE,ent_foto as FOTO, ent_firma,ent_foto2,ent_foto3 FROM entescobradores WHERE codigo ='" & COD & "'", Ado)
            Dim RepRotacion As New RptLogoEmpresa
            Dim xDato As New DatasetForm.CAT_CLIENTESDataTable
            sqlData.Fill(xDato)

            If xDato.Rows.Count > 0 Then
                Dim cr As New RptLogoEmpresa
                Funciones.Exportar(Me, cr, xDato, "Prueba.Pdf", "")
                Messenger.InnerHtml = "<font color='#424242'><b>Nota : </b>" & "Reporte Cod. " & COD & " (Satisfactorio)." & "</font>"
            Else
                Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & "No se detectaron dato por favor elegir un cobrador de la cuadricula para continuar… " & "</font>"
            End If
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Call cancelar()
    End Sub
    Private Sub cancelar()
        Call ultimo()
        Call cargeCobrador()

        lblCodigo.Text = ""
        txtNombre.Text = ""
        Messenger.InnerHtml = ""
        txtArchivadores.Text = ""
        medioarc.Attributes.Add("style", "display:none")
        txtNombre.Focus()
    End Sub

    Protected Sub btnImagena_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImagena.Click
        Try
            If upload.PostedFile.FileName = Nothing Or txtNombre.Text = Nothing Then
                Messenger.InnerHtml = "Elegir una imagen o seleccione un cobrador para continuar..."
                Exit Sub
            End If

            'Obtenemos la ruta donde el archivo se va a guardar en el servidor
            Dim TargetPath As String = HttpRuntime.AppDomainAppPath & "temp_arch\" & Path.GetFileName(upload.PostedFile.FileName)
            upload.PostedFile.SaveAs(TargetPath)
            ImgLogo.Src = "temp_arch/" & Path.GetFileName(upload.PostedFile.FileName)
            Messenger.InnerHtml = "Tu archivo se recibio correctamente en :: <b>" & TargetPath & "</b>"

            curFileName = TargetPath
            If curFileName = Nothing Or lblCodigo.Text = Nothing Then
                Messenger.InnerHtml = "Elegir una imagen o seleccione un cobrador para continuar..."
                Exit Sub
            End If

            Dim etapa As String = Mid(Me.lblCodigo.Text.Trim(), Me.lblCodigo.Text.IndexOf(":") + 3)

            Dim ado As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim SqlData As New SqlDataAdapter("select * from  entescobradores WHERE (codigo ='" & etapa.Trim & "')", ado)
            Dim tb As New DataTable
            fsFoto = New FileStream(curFileName, FileMode.Open)
            Dim fiFoto As FileInfo = New FileInfo(curFileName)
            Dim Temp As Long = fiFoto.Length
            Dim lung As Long = Convert.ToInt32(Temp)
            Dim picture(lung) As Byte
            fsFoto.Read(picture, 0, lung)
            fsFoto.Close()
            SqlData.Fill(tb)

            If ado.State = ConnectionState.Open Then
                ado.Close()
            End If
            ado.Open()

            Dim cmdFoto As New SqlCommand
            cmdFoto.Connection = ado
            If tb.Rows.Count = 0 Then
                cmdFoto.CommandText = "INSERT INTO entescobradores VALUES(@codigo,'" & txtNombre.Text & "',@IMAGEN,@FIRMA)"
                cmdFoto.Parameters.Add("@codigo", System.Data.SqlDbType.VarChar).Value = txtCodigo.Text
                cmdFoto.Parameters.Add("@IMAGEN", SqlDbType.Image)
                cmdFoto.Parameters("@IMAGEN").Value = picture
                cmdFoto.Parameters.Add("@FIRMA", SqlDbType.Image)
                cmdFoto.Parameters("@FIRMA").Value = picture
            Else
                If DropDownListTipo.SelectedValue = "Logo" Then
                    cmdFoto.CommandText = "UPDATE entescobradores SET ent_foto = @IMAGEN WHERE codigo = @codigo"

                ElseIf DropDownListTipo.SelectedValue = "Logo2" Then
                    cmdFoto.CommandText = "UPDATE entescobradores SET ent_foto2 = @IMAGEN WHERE codigo = @codigo"

                ElseIf DropDownListTipo.SelectedValue = "Logo3" Then
                    cmdFoto.CommandText = "UPDATE entescobradores SET ent_foto3 = @IMAGEN WHERE codigo = @codigo"

                Else
                    cmdFoto.CommandText = "UPDATE entescobradores SET ent_firma = @IMAGEN WHERE codigo = @codigo"
                End If

                cmdFoto.Parameters.Add("@IMAGEN", SqlDbType.Image).Value = picture
                cmdFoto.Parameters.Add("@codigo", SqlDbType.VarChar).Value = etapa.Trim
            End If

            cmdFoto.ExecuteNonQuery()
            '----------------
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Actualización de entes", "CODIGO = '01' ", cmdFoto)
            '----------------


            Call Ejecutar_Reporte(txtCodigo.Text)
            cmdFoto.Dispose()
            picture = Nothing
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try
    End Sub

    Protected Sub GridCobradores_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridCobradores.SelectedIndexChanged
        Try
            With Me
                Dim Mytb As DatasetForm.entescobradoresDataTable = CType(.ViewState("DatosUser"), DatasetForm.entescobradoresDataTable)
                With Mytb.Item(.GridCobradores.SelectedIndex)
                    Try
                        txtCodigo.Text = .codigo.Trim
                        txtNombre.Text = .nombre.Trim
                        txtArchivadores.Text = .ent_ruta
                        lblCodigo.Text = txtNombre.Text & "::" & txtCodigo.Text

                        ImgLogo.Src = "imagen.ashx?ImageFileName=" & txtCodigo.Text & "&tipo=" & DropDownListTipo.SelectedValue

                        Messenger.InnerHtml = "<font color='#FFFFFF'><b>Información Útil : <b>Para más  ayuda a la hora de configurar los archivadores, Presione el botón ruta (Ubicado a la izquierda de la etiqueta " & Chr(34) & "Archivadores" & Chr(34) & "). </b> </font>"
                    Catch ex As Exception
                        Messenger.InnerHtml = "<font color='red'>Error : " & ex.Message & "</font>"
                    End Try
                End With
            End With
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try
    End Sub

    Private Sub imagenes(ByVal COD As String)
        'Dim Mytb As New DatasetForm.CAT_CLIENTESDataTable
        'Mytb.AddCAT_CLIENTESRow("", "", "")
    End Sub
    
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Try
            Dim codigo As String = ""
            codigo = GridCobradores.Rows(GridCobradores.SelectedIndex).Cells(0).Text()
            If codigo = Nothing OrElse codigo = "" Then
                codigo = txtCodigo.Text
            End If
            Ejecutar_Reporte(codigo)
        Catch ex As Exception
            Dim amsbgox As String = "<h2 class='err'>MENSAJE DEL SISTEMA</h2>Operación invalida.<br /><b>Seleccione un ente para continuar.</b>" _
            & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
            & ex.Message
            menssageError(amsbgox)
        End Try
    End Sub

    Protected Sub LinkProbarRuta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkProbarRuta.Click
        If Not txtArchivadores.Text = Nothing Then
            If Directory.Exists(txtArchivadores.Text) Then
                Messenger.InnerHtml = "<font color='#FFFFFF'><b>Informe : </b>La carpeta existe <br /> <b>Prueba satisfactoria</b> </font>"

                Dim x As Integer = 0

                Table1.Attributes.Add("border", "0")
                Table1.Attributes.Add("cellpadding", "0")
                Table1.Attributes.Add("cellspacing", "0")

                Try
                    Dim archivos As String = txtArchivadores.Text.Trim
                    'For Each Archivo As String In My.Computer.FileSystem.GetFiles(archivos, FileIO.SearchOption.SearchAllSubDirectories, "*.tif")
                    For Each Archivo As String In My.Computer.FileSystem.GetFiles(archivos, FileIO.SearchOption.SearchTopLevelOnly, "*00000*.tif")
                        Dim r As New TableRow()
                        Dim c As New TableCell()
                        c.Controls.Add(New LiteralControl("<a href='EntesCobradores.aspx?file=" & Archivo & "&tipo=1&con=" & txtCodigo.Text.Trim & "&nom=" & txtNombre.Text & "&rut=" & txtArchivadores.Text & "' class='xa'>" & Archivo & "</a>"))
                        r.Cells.Add(c)
                        Table1.Rows.Add(r)

                        If x = 5 Then
                            Exit For
                        End If
                        x += 1
                    Next
                    medioarc.Attributes.Add("style", "display:block")
                Catch ex As Exception
                    Dim amsbgox As String = "<h2 class='err'>CARPETA O ARCHIVOS INACCESIBLES</h2> <img src='images/icons/Files_Download.png' height = '100' width = '100' />El sistema tiene permisos insuficientes. <br />Póngase en contacto con su administrador del sistema." _
                     & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
                     & "Desborde de dato, configurar el acceso a un archivo específico y la carpeta por tal que permiso de archivo o de red o no existe la carpeta especificada. <br /> Objeto : " _
                     & ex.Message
                    ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel_()"
                    menssageError(amsbgox)
                End Try
            Else
                Messenger.InnerHtml = "<font color='#FFFFFF'><b>Informe : </b>La carpeta no existe <br /> <b>Prueba desfavorable </b> </font>"
            End If
        Else
            Messenger.InnerHtml = "<font color='#FFFFFF'><b>Informe : </b>Operación invalida  <br /> <b>No es posible continuar, si no selecciona un ente de la cuadricula. </b> </font>"
        End If
    End Sub

    Protected Sub DropDownListTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListTipo.SelectedIndexChanged
        ImgLogo.Src = "imagen.ashx?ImageFileName=" & txtCodigo.Text & "&tipo=" & DropDownListTipo.SelectedValue

    End Sub
End Class