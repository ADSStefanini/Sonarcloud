Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Partial Public Class subirexpedientes
    Inherits System.Web.UI.Page

    Dim SqlConnection1 As New SqlConnection

    Private Function LoadDatos() As DataTable
        Dim cnn As String = Session("ConexionServer").ToString
        Dim MyAdapter As New SqlDataAdapter("select actuaciones.codigo,actuaciones.nombre,actuaciones.idetapa,etapas.nombre as nometapa from actuaciones ,etapas where actuaciones.idetapa = etapas.codigo and actuaciones.codigo <> '000'", cnn)
        Dim Mytb As New DatasetForm.etapa_actoDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

    Private Sub cargeetapa_acto()
        Dim Table As DatasetForm.etapa_actoDataTable = LoadDatos()
        Me.ViewState("Datosetapa_acto") = Table
        dtgetapa_acto.DataSource = Table
        dtgetapa_acto.DataBind()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Try
                cargeetapa_acto()
                dtgetapa_acto.SelectedIndex = 0

                Dim Mytb As DatasetForm.etapa_actoDataTable = CType(ViewState("Datosetapa_acto"), DatasetForm.etapa_actoDataTable)
                With Mytb.Item(dtgetapa_acto.SelectedIndex)
                    Etapa.InnerHtml = "<span class=""wsrt""><b>" & .idetapa & "</b></span>"
                    DivEtapa.InnerHtml = .nometapa
                    Acto.InnerHtml = "<span span class=""wsrt""><b>" & .codigo & "</b></span>"
                    DivActo.InnerHtml = .nombre
                End With
            Catch ex As Exception
                Me.Validator.Text = "<font color='#8A0808' >Error :" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
                Me.Validator.IsValid = False
            End Try
        End If
    End Sub

    Private Sub dtgetapa_acto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgetapa_acto.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.etapa_actoDataTable = CType(.ViewState("Datosetapa_acto"), DatasetForm.etapa_actoDataTable)
            With Mytb.Item(.dtgetapa_acto.SelectedIndex)
                Try
                    Etapa.InnerHtml = "<span class=""wsrt""><b>" & .idetapa & "</b></span>"
                    DivEtapa.InnerHtml = .nometapa
                    Acto.InnerHtml = "<span span class=""wsrt""><b>" & .codigo & "</b></span>"
                    DivActo.InnerHtml = .nombre
                Catch ex As Exception
                    'Messenger.InnerHtml = "<font color='red'>Error :" & ex.Message & "</font>"
                End Try
            End With
        End With
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        SqlConnection1.ConnectionString = CType(Session("ConexionServer"), String)

        'Obtenemos la ruta donde el archivo se va a guardar en el servidor:: :: ruta del server de godaddy: d:\hosting\admglobalcorp\coactivosyp
        Dim RutaBase As String = Session("ssrutaexpediente") & "\expedientes\" & Session("mcobrador")

        'Dim CarpetaTrabajo As String = Me.txtEnte.Value.Trim()
        Dim idEntidad As String
        Dim formanombre As String = ""
        idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
        'Dim Destino As String = Server.MapPath("") & "\expedientes\" & Session("mcobrador") & "\" & idEntidad & "\" '& Path.GetFileName(imagen1.PostedFile.FileName)
        Dim Destino As String = Server.MapPath("") & "\expedientes\" & Session("mcobrador") & "\Disco\"
        'Si no se ha seleccionado ninguna carpeta mostrar un mensaje
        If idEntidad = "" Then
            Me.Validator.Text = "Seleccione un ente por favor "
            Me.Validator.IsValid = False
            Return
        End If

        ''Si no se existe la entidad mostrar un mensaje
        Dim cmd As String
        'cmd = "SELECT codigo_nit, nombre FROM entesdbf WHERE RTRIM(codigo_nit) = '" & idEntidad & "'"
        'WHERE cobrador = '" & Session("mcobrador") & "'"
        cmd = "SELECT codigo_nit, nombre FROM entesdbf WHERE RTRIM(codigo_nit) = '" & idEntidad & "' AND cobrador = '" & Session("mcobrador") & "'"

        Dim Dataset1 As DataSet = New DataSet

        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, Me.SqlConnection1)
        oDtAdapterSql1.Fill(Dataset1, "qEntes")

        If Dataset1.Tables("qEntes").Rows.Count = 0 Then
            Me.Validator.Text = "La entidad digitada no existe"
            Me.Validator.IsValid = False
            Return
        End If

        ' OJO con este codigo
        ' ''Si el directorio no existe => se crea
        'Dim dir As New System.IO.DirectoryInfo(RutaBase & "\" & idEntidad)
        'If Not dir.Exists Then 'En este caso existe el dir        
        '    'Crear la carpeta
        '    Directory.CreateDirectory(RutaBase & "/" & idEntidad)
        'End If

        'Preparar los datos que se van a insertar en la tabla de documentos
        Dim idacto, sw, dia, mes, anio, fechaserver, TipoConexion As String
        Dim fecharadic As Date
        TipoConexion = ConfigurationManager.AppSettings("tipoconexion") 'local o web

        If Me.txtFechaRad.Text = "" Then
            Me.Validator.Text = "Digite la fecha de radicación por favor"
            Me.Validator.IsValid = False
            Return
        End If

        Try
            If TipoConexion = "web" Then
                dia = Left(Me.txtFechaRad.Text.Trim(), 2)
                mes = Mid(Me.txtFechaRad.Text.Trim(), 4, 2)
                anio = Mid(Me.txtFechaRad.Text.Trim(), 7, 4)
                fechaserver = mes & "/" & dia & "/" & anio
                fecharadic = CDate(fechaserver)
            Else
                fecharadic = CDate(Me.txtFechaRad.Text)
            End If
        Catch ex As Exception
            Me.Validator.Text = ex.Message.ToString()
            Me.Validator.IsValid = False
        End Try

        Dim Mytbb As DatasetForm.etapa_actoDataTable = CType(ViewState("Datosetapa_acto"), DatasetForm.etapa_actoDataTable)
        With Mytbb.Item(dtgetapa_acto.SelectedIndex)
            idacto = .codigo
        End With


        'buscar si ya hay documentos de esta misma actuacion
        Dim idactoAdapter As New SqlDataAdapter("select count(*) from documentos where entidad= '" & idEntidad & "' and idacto ='" & idacto & "' and cobrador='" & Session("mcobrador").ToString & "'", Session("ConexionServer").ToString)
        Dim mytableidacto As New DataTable
        idactoAdapter.Fill(mytableidacto)
        Dim ultimodoc As Integer = mytableidacto.Rows(0).Item(0)

        ' barrer y gusrdar los input Command
        Dim uploads As HttpFileCollection
        uploads = HttpContext.Current.Request.Files

        'prepaparar dataset 
        Dim Mytb As New DatasetForm.documentosDataTable
        sw = "no"
        For i As Integer = 0 To (uploads.Count - 1)
            If (uploads(i).ContentLength > 0) Then
                Dim archivo1 As String = Trim(System.IO.Path.GetFileName(uploads(i).FileName)).ToLower
                Try
                    'uploads(i).SaveAs(Destino & c)
                    If archivo1 <> "" Then
                        Try
                            uploads(i).SaveAs(Destino & archivo1)

                            'cambiar nombre del archivo una vez guardado 
                            'formanombre = idacto & "-" & CType(((i + ultimodoc) + 1), String)
                            'Dim ddd As String = CambiarNombreArchivo(Destino & formanombre, Destino & archivo1)
                            'archivo1 = Trim(System.IO.Path.GetFileName(ddd)).ToLower

                            'Dim arch As String = Destino & archivo1
                            'System.IO.File.Move(arch, Destino & "001-20.jpg")

                            'Verificar que no existe el archivo 1
                            If Not Me.ExisteArchivo(idEntidad, archivo1, Session("mcobrador")) Then
                                Mytb.AdddocumentosRow(idEntidad, idacto, idEntidad, archivo1, txtNroPaginas.Text, FechaWebLocal(fecharadic), Session("mcobrador"), txtExpediente.Text, txtExpediente.Text, txtrefecatras.Text, "", FechaWebLocal(txtFechacreacion.Text), "", True, Session("sscodigousuario"), FechaWebLocal(Date.Now), "S", Session("ssimpuesto"), Nothing, Nothing)
                            End If
                        Catch Ex As Exception
                            Me.Validator.Text = "ERROR: " & Ex.Message.ToString()
                            sw = "si"
                        End Try
                    End If
                Catch Exp As Exception
                    Me.Validator.Text = Exp.Message
                    Me.Validator.IsValid = False
                End Try
            End If
        Next i

        'se guardan las imagenes con el respectivo documeto 
        If Mytb.Rows.Count > 0 Then
            Dim Adap As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter("select * from documentos", Session("ConexionServer").ToString)
            Dim Cb As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(Adap)
            Adap.InsertCommand = Cb.GetInsertCommand
            Adap.Update(Mytb)
        Else
            Me.Validator.Text = "No se guardo el expediente."
            Me.Validator.IsValid = False
            Return
        End If

        ' Si no hubo errrores mostrar mensaje de exito
        If sw = "no" Then
            Me.Validator.Text = "Expediente actualizado con éxito"

            'Actualizar el numero de paginas de la actuacion en el expediente actual
            If idacto <> "" Then
                Dim mTotimg, registros As Integer
                mTotimg = 0
                cmd = "SELECT count(*) AS NumPaginas FROM documentos WHERE entidad = '" & idEntidad & "' AND idacto = '" & idacto & "' AND cobrador = '" & Session("mcobrador") & "'"

                ' Se crea el SQLCommand
                Dim cmdsql As SqlCommand
                cmdsql = New SqlCommand(cmd, Me.SqlConnection1)
                cmdsql.CommandType = CommandType.Text

                ' Abrir la conexion
                Me.SqlConnection1.Open()

                ' Ejecutar la consulta escalar
                mTotimg = cmdsql.ExecuteScalar()

                ' Crear y ejecutar la sentencia de actualizacion del numero de paginas
                cmd = "UPDATE documentos SET paginas = " & mTotimg & " WHERE entidad = '" & idEntidad & "' AND idacto = '" & idacto & "' AND cobrador = '" & Session("mcobrador") & "'"
                cmdsql.CommandText = cmd
                registros = cmdsql.ExecuteNonQuery()

                'Cerrar la conexion
                Me.SqlConnection1.Close()
            End If
        End If

        Me.Validator.IsValid = False
    End Sub
    Private Function ExisteArchivo(ByVal pEntidad As String, ByVal pNomArchivo As String, ByVal pCobrador As String) As Boolean
        Dim cmd As String
        Dim NumeroImagenes As Integer
        SqlConnection1.ConnectionString = CType(Session("ConexionServer"), String)
        cmd = "SELECT COUNT(*) FROM documentos WHERE RTRIM(entidad) = '" & pEntidad & "' AND RTRIM(nomarchivo) = '" & pNomArchivo & "' AND RTRIM(cobrador) = '" & pCobrador & "'"

        ' Se crea el SQLCommand
        Dim cmdsql As SqlCommand
        cmdsql = New SqlCommand(cmd, Me.SqlConnection1)
        cmdsql.CommandType = CommandType.Text

        ' Abrir la conexion
        Me.SqlConnection1.Open()

        ' Ejecutar la consulta escalar
        NumeroImagenes = cmdsql.ExecuteScalar()

        'Cerrar la conexion
        Me.SqlConnection1.Close()

        If NumeroImagenes = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function CambiarNombreArchivo(ByVal archivo As String, ByVal ruta_guardada As String) As String
        Dim nombre_arch As String = archivo
        Dim StrFileToConvert As String = nombre_arch '"c:\nombre_foto"
        Dim Path As New Bitmap(ruta_guardada)
        Path.Save(StrFileToConvert + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
        Path.Dispose()
        System.IO.File.Delete(ruta_guardada)

        Return StrFileToConvert + ".jpg"
    End Function

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

    End Sub
End Class
