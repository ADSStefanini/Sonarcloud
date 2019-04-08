Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.IO

Module Funciones

    Public Function DateToLetters(ByVal Fecha As Date) As String
        Dim Mes() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
        Dim Letters As New System.Text.StringBuilder
        Letters.Append(IIf(Fecha.Day = 1, "PRIMERO", Num2Text(Fecha.Day)))
        Letters.Append(" (")
        Letters.Append(Fecha.Day)
        Letters.Append(") de ")
        Letters.Append(Mes(Fecha.Month - 1))
        Letters.Append(" de ")
        Letters.Append(Num2Text(Fecha.Year))
        Letters.Append(" (")
        Letters.Append(Fecha.Year)
        Letters.Append(")")

        Return Letters.ToString
    End Function

    Public Function NomArchivoTiff_or_Pdf(ByVal Extension_de_Archivo As String) As String
        'CONSECUTIVO NOMBRE TIFF
        Using con As New SqlConnection(Funciones.CadenaConexion)
            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction
            Dim proximo_numero As String = "00000000000"
            Dim command As New SqlCommand("UPDATE MAESTRO_CONSECUTIVOS SET @PROXIMO_NUMERO = CON_USER = CON_USER + 1 WHERE CON_IDENTIFICADOR = 9", con)
            command.Parameters.Add("@PROXIMO_NUMERO", SqlDbType.Int)
            command.Parameters("@PROXIMO_NUMERO").Direction = ParameterDirection.Output
            command.Transaction = tran
            command.ExecuteNonQuery()
            Dim conse As Integer = CType(command.Parameters("@PROXIMO_NUMERO").Value, Integer)
            proximo_numero = Format(conse, "00000000000")

            Try
                tran.Commit()
            Catch ex As Exception
                tran.Rollback()
            End Try

            Dim Nombre_Archivo As New StringBuilder
            Nombre_Archivo.Append("TE")
            Nombre_Archivo.Append(proximo_numero)
            Nombre_Archivo.Append(Date.Now.ToUniversalTime.ToString("ddMMyyyyHHmmss"))
            Nombre_Archivo.Append(proximo_numero)
            Nombre_Archivo.Append(Extension_de_Archivo.ToLower)

            Return Nombre_Archivo.ToString
        End Using
    End Function

    Public Function validar_Mail(ByVal sMail As String) As Boolean
        ' retorna true o false   
        Return Regex.IsMatch(sMail, _
                "^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$")
    End Function

    Public Function ChekaNull(ByVal MyVar As Object) As Boolean
        Dim MyCheck As Boolean
        MyCheck = IsDBNull(MyVar)
        Return MyCheck
    End Function

    Public Function valorNull(ByVal MyVar As Object) As String
        Dim MyCheck As Boolean
        MyCheck = IsDBNull(MyVar)

        If MyCheck = True Then
            Return 0
        Else
            Return MyVar
        End If
    End Function

    Public Function valorNull(ByVal MyVar As Object, ByVal valorRetorno As String) As String
        Dim MyCheck As Boolean
        MyCheck = IsDBNull(MyVar)

        If MyCheck = True Then
            Return valorRetorno
        Else
            Return MyVar
        End If
    End Function

    Public Function ultimoCon(ByVal consebus As Integer, ByVal Lencon As Integer) As String
        Dim xt As Integer = 0
        Dim ceros As String = ""
        For xt = 0 To Lencon - 1
            ceros = ceros + "0"
        Next

        Dim myadapter As New SqlDataAdapter("select * from MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR =" & CType(consebus, String), ConexionServer)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1
        Return Format(Con, ceros)
    End Function

    Private Function ConexionServer() As String
        Dim CadenaConexionUnionBuilder As New StringBuilder
        CadenaConexionUnionBuilder.Append("workstation id= ")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("ServerName"))
        CadenaConexionUnionBuilder.Append(";packet size=4096;user id=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_User"))
        CadenaConexionUnionBuilder.Append(";data source=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("ServerName"))
        CadenaConexionUnionBuilder.Append(";persist security info=True;initial catalog=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_name"))
        CadenaConexionUnionBuilder.Append(";password=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_pass"))

        Return CadenaConexionUnionBuilder.ToString
    End Function

    Public Function CadenaConexionUnion() As String
        Dim CadenaConexionUnionBuilder As New StringBuilder
        CadenaConexionUnionBuilder.Append("workstation id= ")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("ServerName2"))
        CadenaConexionUnionBuilder.Append(";packet size=4096;user id=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_User2"))
        CadenaConexionUnionBuilder.Append(";data source=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("ServerName2"))
        CadenaConexionUnionBuilder.Append(";persist security info=True;initial catalog=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_name2"))
        CadenaConexionUnionBuilder.Append(";password=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_pass2"))

        Return CadenaConexionUnionBuilder.ToString
    End Function

    Public Function Num2Text(ByVal value As Double) As String
        Select Case value
            Case 0 : Num2Text = "CERO"
            Case 1 : Num2Text = "UN"
            Case 2 : Num2Text = "DOS"
            Case 3 : Num2Text = "TRES"
            Case 4 : Num2Text = "CUATRO"
            Case 5 : Num2Text = "CINCO"
            Case 6 : Num2Text = "SEIS"
            Case 7 : Num2Text = "SIETE"
            Case 8 : Num2Text = "OCHO"
            Case 9 : Num2Text = "NUEVE"
            Case 10 : Num2Text = "DIEZ"
            Case 11 : Num2Text = "ONCE"
            Case 12 : Num2Text = "DOCE"
            Case 13 : Num2Text = "TRECE"
            Case 14 : Num2Text = "CATORCE"
            Case 15 : Num2Text = "QUINCE"
            Case Is < 20 : Num2Text = "DIECI" & Num2Text(value - 10)
            Case 20 : Num2Text = "VEINTE"
            Case Is < 30 : Num2Text = "VEINTI" & Num2Text(value - 20)
            Case 30 : Num2Text = "TREINTA"
            Case 40 : Num2Text = "CUARENTA"
            Case 50 : Num2Text = "CINCUENTA"
            Case 60 : Num2Text = "SESENTA"
            Case 70 : Num2Text = "SETENTA"
            Case 80 : Num2Text = "OCHENTA"
            Case 90 : Num2Text = "NOVENTA"
            Case Is < 100 : Num2Text = Num2Text(Int(value \ 10) * 10) & " Y " & Num2Text(value Mod 10)
            Case 100 : Num2Text = "CIEN"
            Case Is < 200 : Num2Text = "CIENTO " & Num2Text(value - 100)
            Case 200, 300, 400, 600, 800 : Num2Text = Num2Text(Int(value \ 100)) & "CIENTOS"
            Case 500 : Num2Text = "QUINIENTOS"
            Case 700 : Num2Text = "SETECIENTOS"
            Case 900 : Num2Text = "NOVECIENTOS"
            Case Is < 1000 : Num2Text = Num2Text(Int(value \ 100) * 100) & " " & Num2Text(value Mod 100)
            Case 1000 : Num2Text = "MIL"
            Case Is < 2000 : Num2Text = "MIL " & Num2Text(value Mod 1000)
            Case Is < 1000000 : Num2Text = Num2Text(Int(value \ 1000)) & " MIL"
                If value Mod 1000 Then Num2Text = Num2Text & " " & Num2Text(value Mod 1000)
            Case 1000000 : Num2Text = "UN MILLON"
            Case Is < 2000000 : Num2Text = "UN MILLON " & Num2Text(value Mod 1000000)
            Case Is < 1000000000000.0# : Num2Text = Num2Text(Int(value / 1000000)) & " MILLONES "
                If (value - Int(value / 1000000) * 1000000) Then Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000) * 1000000)
            Case 1000000000000.0# : Num2Text = "UN BILLON"
            Case Is < 2000000000000.0# : Num2Text = "UN BILLON " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
            Case Else : Num2Text = Num2Text(Int(value / 1000000000000.0#)) & " BILLONES"
                If (value - Int(value / 1000000000000.0#) * 1000000000000.0#) Then Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
        End Select
    End Function

    Public Function EntesCobradorPublic(ByVal cobrador As String) As String
        Dim adpater As New SqlDataAdapter("select * from entescobradores where codigo = '" & cobrador & "'", ConexionServer.ToString)
        Dim mytable As New DataTable
        adpater.Fill(mytable)

        cobrador = mytable.Rows(0).Item("nombre")
        Return cobrador
    End Function

    Public Function RetornaCargadatos(ByVal tran As String) As DataTable
        Dim cnn As String = CadenaConexion()
        Dim MyAdapter As New SqlDataAdapter(tran, cnn)
        Dim Mytb As New DataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

    Public Function CadenaConexion() As String
        Dim CadenaConexionUnionBuilder As New StringBuilder
        CadenaConexionUnionBuilder.Append("workstation id= ")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("ServerName"))
        CadenaConexionUnionBuilder.Append(";packet size=4096;user id=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_User"))
        CadenaConexionUnionBuilder.Append(";data source=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("ServerName"))
        CadenaConexionUnionBuilder.Append(";persist security info=True;initial catalog=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_name"))
        CadenaConexionUnionBuilder.Append(";password=")
        CadenaConexionUnionBuilder.Append(System.Configuration.ConfigurationManager.AppSettings("BD_pass"))

        Return CadenaConexionUnionBuilder.ToString
    End Function

    Public Sub Exportar(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
                       ByVal Datos As Object, ByVal NameFile As String, ByVal Path As String, Optional ByVal Params As List(Of ItemParams) = Nothing)
        If Report.Database.Tables.Count > 0 Then Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New PdfRtfWordFormatOptions

        ExportOpts = Report.ExportOptions
        ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile
        DiskOpts.DiskFileName = Page.MapPath(Path) & "\" & NameFile

        'DiskOpts.DiskFileName = Page.MapPath("~") & "\Crystal_Reports\" & NameFile
        'NameFile = DiskOpts.DiskFileName

        If Not Params Is Nothing Then
            For Each P As ParameterField In Report.ParameterFields
                Dim PV = From Par In Params Where Par.Parameter_Name = P.Name Select Par
                For Each Ip In PV
                    Report.SetParameterValue(P.Name, Ip.Parameter_Value)
                Next
            Next
        End If

        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        Report.Close()
        Report.Dispose()
        Report = Nothing

        Ejecutarjavascript(Page, "<script ""text/javascript"">window.open('" & NameFile & "','Graph','left=0,top=0,fullscreen=yes, scrollbars=auto');</script>", "Reporte")
    End Sub

    Public Sub Exportar_word(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
                       ByVal Datos As Object, ByVal NameFile As String, ByVal Path As String, Optional ByVal Params As List(Of ItemParams) = Nothing)
        If Report.Database.Tables.Count > 0 Then Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New PdfRtfWordFormatOptions

        ExportOpts = Report.ExportOptions
        ExportOpts.ExportFormatType = ExportFormatType.WordForWindows
        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile
        DiskOpts.DiskFileName = Page.MapPath(Path) & "\" & NameFile

        'DiskOpts.DiskFileName = Page.MapPath("~") & "\Crystal_Reports\" & NameFile
        'NameFile = DiskOpts.DiskFileName

        If Not Params Is Nothing Then
            For Each P As ParameterField In Report.ParameterFields
                Dim PV = From Par In Params Where Par.Parameter_Name = P.Name Select Par
                For Each Ip In PV
                    Report.SetParameterValue(P.Name, Ip.Parameter_Value)
                Next
            Next
        End If

        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        Report.Close()
        Report.Dispose()
        Report = Nothing

        Ejecutarjavascript(Page, "", "Reporte")
    End Sub

    Public Sub Exportar2(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
                       ByVal Datos As Object, ByVal NameFile As String, ByVal Path As String)
        Report.SetDataSource(Datos)
        Report.PrintOptions.PrinterName = "ImagePri"
        Report.PrintOptions.PaperSize = PaperSize.PaperLetter

        Report.SummaryInfo.KeywordsInReport = "Esto_es_una_prueba"
        Report.SummaryInfo.ReportAuthor = "Esto_es_una_prueba"
        Report.SummaryInfo.ReportComments = "Esto_es_una_prueba"
        Report.SummaryInfo.ReportSubject = "Esto_es_una_prueba"
        Report.SummaryInfo.ReportTitle = "Esto_es_una_prueba"
        Report.Refresh()

        Report.PrintToPrinter(1, False, 0, 0)
        'Report.OpenSubreport("Details").PrintToPrinter(1, True, 0, 0)
    End Sub

    Public Sub Exportar3(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
                    ByVal Datos As Object, ByVal NameFile As String, ByVal tipo As String, Optional ByVal Redirect As Boolean = True)
        Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        ExportOpts = Report.ExportOptions
        If tipo = "pdf" Then
            ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        ElseIf tipo = "doc" Then
            ExportOpts.ExportFormatType = ExportFormatType.WordForWindows
        End If

        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile
        DiskOpts.DiskFileName = Page.MapPath("Archivos Planos/") & NameFile
        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        If Redirect Then
            Page.Response.Redirect("Archivos Planos/" & NameFile, False)
        End If
    End Sub

    Public Sub Ejecutarjavascript(ByVal Page As Page, ByVal script As String, ByVal Nombrescript As String)
        'Response.Write(vari)
        ' Define the name and type of the client scripts on the page. 
        Dim csname1 As [String] = Nombrescript
        Dim cstype As Type = Page.[GetType]()

        'Get a ClientScriptManager reference from the Page class. 
        Dim cs As ClientScriptManager = Page.ClientScript

        If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As String = script
            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())
        End If
    End Sub

    Public Function TranformarFecha(ByVal item As String, ByVal TipoConexion As Byte) As Date
        Dim fechaserver As String
        If TipoConexion = "web" Then
            Dim dia As String
            Dim mes As String
            Dim anio As String

            dia = Left(item.Trim(), 2)
            mes = Mid(item.Trim(), 4, 2)
            anio = Mid(item.Trim(), 7, 4)

            fechaserver = mes & "/" & dia & "/" & anio
        Else
            fechaserver = CDate(item)
        End If

        Return CDate(fechaserver)
    End Function

    Public Function FechaWebLocal(ByVal fecha As String) As Date
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

    Public Function FechaWebLocal(ByVal fecha As Date) As Date
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

    Public Function ForzarMostraPDF(ByVal Page As Page, ByVal Path As String) As Boolean
        Dim pdfPath As String = Path
        Dim client As New WebClient()
        Dim buffer As [Byte]() = client.DownloadData(pdfPath)
        Page.Response.ContentType = "application/pdf"
        Page.Response.AddHeader("content-length", buffer.Length.ToString())
        Page.Response.BinaryWrite(buffer)
    End Function

    '<Ojo exportacion de los expedientes masivos e individuales>
    Public Function Exportar_Reportes_Masivos(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
               ByVal Datos As Object, ByVal NameFile As String, ByVal Path As String) As String

        Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New PdfRtfWordFormatOptions

        ExportOpts = Report.ExportOptions
        ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile
        'Directorio de imagenes cambiar ???????
        If Path <> Nothing Then
            DiskOpts.DiskFileName = Path & "\" & NameFile
        Else
            DiskOpts.DiskFileName = Page.MapPath(Path) & "\temp_arch\reportes_masivos\" & NameFile
        End If

        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        Report.Close()
        Report.Dispose()
        Report = Nothing

        Return DiskOpts.DiskFileName
    End Function

    Public Function Llenar_Fila_Ejecuciones_Fiscales(ByVal Row As DataSet1.EJEFISGLOBALRow, ByVal Ultimo_Paso_Expe As String, ByVal Impuesto As Integer) As DataSet1.EJEFISGLOBALRow
        Dim TblEjFis As New DataSet1.EJEFISGLOBALDataTable
        For Each Column As DataColumn In TblEjFis.Columns
            'If Column.AllowDBNull Then
            If Column.Caption = "EfiUltPas" Then
                Row(Column.Caption) = Ultimo_Paso_Expe
            ElseIf Column.Caption = "PefCod" Then ' ojo parapanpam
                Row(Column.Caption) = 2
            ElseIf Column.Caption = "EfiEst" Then ' ojo parapanpam
                Row(Column.Caption) = 0
            ElseIf Column.Caption = "EfiModCod" Then ' ojo parapanpam 
                Row(Column.Caption) = Impuesto
            Else
                If Row.IsNull(Column.Caption) Then
                    If Column.DataType.Name <> "String" Then
                        If Column.DataType.Name = "Boolean" Then
                            Row(Column.Caption) = False
                        ElseIf Column.DataType.Name = "DateTime" Then
                            Row(Column.Caption) = Date.Today
                        Else
                            Row(Column.Caption) = 0
                        End If
                    Else
                        Row(Column.Caption) = ""
                    End If
                End If
            End If
        Next
        Return Row
    End Function

    Public Function Column_Ejecucion_Fiscal(ByVal Name As String) As DataColumn
        Dim TblEjFis As New DataSet1.EJEFISGLOBALDataTable
        Dim Column As DataColumn = Nothing
        Select Case Name
            Case "MAN_REFCATRASTAL"
                Column = TblEjFis.Columns("EFIGEN")
            Case "MAN_EFIPERDES"
                Column = TblEjFis.Columns("EfiPerDes")
            Case "MAN_EFIPERHAS"
                Column = TblEjFis.Columns("EfiPerHas")
            Case "MAN_EXPEDIENTE"
                Column = TblEjFis.Columns("EfiNroExp")
            Case "MAN_TOTAL"
                Column = TblEjFis.Columns("EfiValDeu")
            Case "MAN_INTERESES"
                Column = TblEjFis.Columns("EfiValInt")
            Case "MAN_NOMDEUDOR"
                Column = TblEjFis.Columns("EfiNom")
            Case "MAN_DEUSDOR"
                Column = TblEjFis.Columns("EfiNit")
        End Select

        Return Column
    End Function

    Public Sub InsertDocEjecucionesFiscales(ByVal Mytbdocumento As DataSet1.EJEFISGLOBALDataTable)
        Using con As New SqlConnection(Funciones.CadenaConexionUnion)
            Dim da As New SqlDataAdapter("SELECT TOP 10 * FROM EJEFISGLOBAL", con)
            'da.SelectCommand.Parameters.Add("@Documento", SqlDbType.VarChar).Value = documento

            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            Dim cb As New SqlCommandBuilder(da)
            da.InsertCommand = cb.GetInsertCommand()

            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction
            da.InsertCommand.Transaction = tran

            Try
                da.Update(Mytbdocumento)
                tran.Commit()
            Catch ex As Exception
                tran.Rollback()
            End Try
            con.Close()
        End Using
    End Sub

    Public Sub InsertDocDefinitivo(ByVal documento As String, ByVal Mytbdocumento As DatasetForm.documentosDataTable)
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim da As New SqlDataAdapter("select * from documentos WHERE docexpediente = @Documento", con)
            da.SelectCommand.Parameters.Add("@Documento", SqlDbType.VarChar).Value = documento

            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            Dim cb As New SqlCommandBuilder(da)
            da.InsertCommand = cb.GetInsertCommand()

            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction
            da.InsertCommand.Transaction = tran

            Try
                da.Update(Mytbdocumento)
                tran.Commit()
                'Nota Reparar
                'UpdateUtilpaso(Mydocumento_ultimoacto)
            Catch ex As Exception
                tran.Rollback()
            End Try
            con.Close()
        End Using
    End Sub

    Public Function InsertReslucion(ByVal expediente As String, ByVal acto_administrativo As String) As DataTable
        Using con As New SqlConnection(Funciones.CadenaConexion)

            Dim adapter As SqlDataAdapter
            Dim tb As New DataTable
            Dim command As New SqlCommand("REGISTRAR_DOCUMENTO_GENERADO", con)
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            command.Parameters.AddWithValue("@ACTO", acto_administrativo)
            adapter = New SqlDataAdapter(command)
            adapter.Fill(tb)

            Return tb
        End Using
    End Function

    Public Function Logo_Load_Configuracion(ByVal Data As DataTable, ByVal Cobrador As String) As DataTable
        Dim ESCommand As New SqlCommand
        ESCommand.Connection = New SqlConnection(Funciones.CadenaConexion)
        ESCommand.CommandText = "SELECT codigo as ID_CLIENTE, nombre as NOMBRE,ent_foto as FOTO,ent_pref_exp,ent_pref_res,ent_tesorero, ent_firma,ent_foto2,ent_foto3,ent_funcionario_ejec,ent_cargo_func_ejec, salario_minimo  FROM entescobradores WHERE codigo = @Cobrador"
        ESCommand.Parameters.Add("@Cobrador", SqlDbType.VarChar).Value = Cobrador
        ESCommand.Connection.Open()
        Dim reader As SqlDataReader = ESCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Data.Load(reader)
        reader.Close()
        Return Data
    End Function

    Public Function Literal_Coceptos_de_la_Deuda(ByVal Expediente) As String
        Dim texto As New StringBuilder
        Using Command As New System.Data.SqlClient.SqlCommand("SELECT CONVERT(INT, EDC_LIQOFI) AS LIQOFI,CONVERT(CHAR(10), EDC_FECHLA_IQUIDACION, 20) AS FECHLA_IQUIDACION,EFIGEN AS PREDIO,EDC_VIGENCIA AS PERIODO,SUM(EDC_TOTALDEUDA) AS TOTAL_IMPUESTO, EFINROEXP AS EXPEDIENTE FROM EJEFISGLOBALLIQUIDAD,EJEFISGLOBAL WHERE EDC_ID = EFIGEN AND EFINROEXP = @DOCEXPEDIENTE AND EDC_VIGENCIA BETWEEN EFIPERDES AND EFIPERHAS GROUP BY EDC_LIQOFI,EDC_FECHLA_IQUIDACION,EFIGEN,EDC_VIGENCIA,EDC_TOTLIQOFI,EFINROEXP ORDER BY EDC_VIGENCIA ASC", New SqlConnection(Funciones.CadenaConexion))
            Command.Parameters.Add("@DOCEXPEDIENTE", SqlDbType.VarChar).Value = Expediente
            Command.Connection.Open()
            Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
            Dim tb As New DataTable
            tb.Load(reader)
            Dim con As Integer = tb.Rows.Count
            Dim cur As Integer = 0
            For Each row As DataRow In tb.Rows
                cur += 1

                texto.Append("No. ")
                texto.Append(row("LIQOFI"))
                texto.Append(" de ")
                texto.Append(CDate(row("FECHLA_IQUIDACION")).ToString("dd/MM/yyyy"))
                texto.Append(" que comprende la vigencia ")
                texto.Append(row("PERIODO"))
                texto.Append(" por un valor de ")
                texto.Append(Num2Text(row("TOTAL_IMPUESTO")))
                texto.Append(" 00/100 (")
                texto.Append(CDbl(row("TOTAL_IMPUESTO")).ToString(("C2")))
                texto.Append(")")

                If con > 1 Then
                    If con = cur Then
                        texto.Append(".")
                    Else
                        texto.Append(", ")
                    End If
                End If
            Next

            reader.Close()
            Command.Connection.Close()
        End Using
        Return texto.ToString
    End Function

    Public Function consultar(ByVal Sql As String, ByVal _Conexion As SqlConnection) As DataTable
        Dim _Table As New DataTable
        Dim _Adapter As SqlDataAdapter
        _Adapter = New SqlDataAdapter(Sql, _Conexion)
        _Adapter.Fill(_Table)
        Return _Table
    End Function

    Public Function GetDatoPlantillaWord(ByVal expediente As String, ByVal acto As String, Optional ByVal tipo As Integer = 0) As DataTable
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanager As New DataTable
            Dim sqlcommand As New SqlCommand("DATOS_WORD", con)
            sqlcommand.CommandType = CommandType.StoredProcedure
            sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            sqlcommand.Parameters.AddWithValue("@acto", acto)
            sqlcommand.Parameters.AddWithValue("@OP", tipo)
            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanager)

            Return sqldatamanager
        End Using
    End Function

    Public Function getPorcentaje(ByVal op As Integer) As Double
        Dim tasa As Double
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanger As New DataTable
            Dim sqlcommand As New SqlCommand("select * from  porcentaje_tasa_multa", con)
            sqlcommand.CommandType = CommandType.Text
            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanger)
            If sqldatamanger.Rows.Count > 0 Then


                If op = 1 Then
                    tasa = Math.Round(sqldatamanger.Rows.Item(0)(1), 3)
                Else
                    tasa = Math.Round(sqldatamanger.Rows.Item(0)(2), 3)
                End If

            End If

            Return tasa
        End Using



    End Function

    Public Function getResolucion_anterior(ByVal Expediente As String, ByVal acto_administrativo As String) As String()
        Dim resolucion(2) As String
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanger As New DataTable
            Dim sqlcommand As New SqlCommand("SELECT A.DXI_ACTO  AS ACTUACION , B.DG_FECHA_DOC AS FECHA_ANTERIOR , B.DG_NRO_DOC  AS RESOLUCION, A.DXI_ACTO_PREVIO AS ACTO_ANTERIOR  FROM  DOCUMENTO_INFORMEXIMPUESTO A, DOCUMENTOS_GENERADOS B WHERE  A.DXI_ACTO_PREVIO  = B.DG_COD_ACTO AND B.DG_EXPEDIENTE  = @EXPEDIENTE AND A.DXI_ACTO =@ACTO", con)
            sqlcommand.CommandType = CommandType.Text
            sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", Expediente)
            sqlcommand.Parameters.AddWithValue("@ACTO", acto_administrativo)
            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanger)
            If sqldatamanger.Rows.Count > 0 Then

                resolucion(0) = sqldatamanger.Rows.Item(0)("RESOLUCION")
                resolucion(1) = CDate(sqldatamanger.Rows.Item(0)("FECHA_ANTERIOR")).ToString("'del' dd 'de' MMMM 'de' yyy")
                resolucion(2) = sqldatamanger.Rows.Item(0)("ACTO_ANTERIOR")
            Else
                resolucion(0) = ""
                resolucion(1) = ""
                resolucion(2) = ""
            End If

            Return resolucion
        End Using

    End Function

    Public Function GetDatosGenerales(ByVal expediente As String) As DataTable
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanager As New DataTable
            Dim sqlcommand As New SqlCommand("GETDATOS_GENERALES", con)
            sqlcommand.CommandType = CommandType.StoredProcedure
            sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanager)

            Return sqldatamanager
        End Using
    End Function

    Public Function GetActo(ByVal Expediente As String, ByVal acto As String) As String()
        Dim Actos(3) As String
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanger As New DataTable
            Dim sqlcommand As SqlCommand
            Dim consulta As String
            Select Case acto
                Case "013"
                    consulta = " SELECT (CASE WHEN ISNULL(FECNOTIPERS,'')  = '' THEN '' ELSE FECNOTIPERS  END ) AS FECNOTIPERS, " & _
                                                                 " (CASE WHEN ISNULL(FECNOTICOR ,'')  = '' THEN '' ELSE FECNOTICOR   END ) AS FECNOTICOR,  " & _
                                                                 " (CASE WHEN ISNULL(FECNOTICOR ,'')  = '' THEN 'Personalmente'   ELSE ' Por Correo' END) AS TIPO_N, " & _
                                                                 " isnull(NroActaNotPer ,'') as NroActaNotPer, " & _
                                                                 " isnull (NroOfiNotCor ,'') as NroOfiNotCor   " & _
                                                                 " FROM MANDAMIENTOS_PAGO " & _
                                                                 " WHERE NROEXP  ='" & Expediente & "'"
                Case "223"


            End Select
            sqlcommand = New SqlCommand(consulta, con)

            sqlcommand.CommandType = CommandType.Text
            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanger)
            If sqldatamanger.Rows.Count > 0 Then
                If sqldatamanger.Rows.Item(0)("FECNOTIPERS") = "1900-01-01" Then
                    Actos(0) = CDate(sqldatamanger.Rows.Item(0)("FECNOTICOR")).ToString("'del' dd 'de' MMMM 'de' yyy")
                    Actos(1) = sqldatamanger.Rows.Item(0)("TIPO_N")
                    Actos(2) = sqldatamanger.Rows.Item(0)("NroOfiNotCor")
                    Actos(3) = CDate(sqldatamanger.Rows.Item(0)("FECNOTICOR"))
                Else
                    Actos(0) = CDate(sqldatamanger.Rows.Item(0)("FECNOTIPERS")).ToString("'del' dd 'de' MMMM 'de' yyy")
                    Actos(1) = sqldatamanger.Rows.Item(0)("TIPO_N")
                    Actos(2) = sqldatamanger.Rows.Item(0)("NroActaNotPer")
                    Actos(3) = CDate(sqldatamanger.Rows.Item(0)("FECNOTIPERS"))
                End If
            End If

            Return Actos

        End Using


    End Function

    Public Function GetExcepcion(ByVal Expediente As String) As String()
        Dim Acto(3) As String
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanger As New DataTable
            Dim sqlcommand As New SqlCommand( _
                        "SELECT  " & _
                        "(case when isnull (NRORESRESUELVE,'Sin Datos')  =  'Sin Datos'  then 'No Presento'  else                                            " & _
                        " case when isnull (NRORESRESUELVE,''         )  =  ''           then 'No Presento'  else NRORESRESUELVE end end ) as resolucion  ,  " & _
                        "(case when ISNULL (FECRESRESUELVE,''        )   =  ''           then '           '  else FECRESRESUELVE end ) as fecha_resolucion,  " & _
                        "(case when  ISNULL (NROOFINOTCOR  ,'Sin Datos')  =  'Sin Datos' then 'No Presento'  else                                            " & _
                        " case when  ISNULL (NROOFINOTCOR  ,'')           =  ''          then 'No Presento'  else NROOFINOTCOR  end end) as Num_notifica ,   " & _
                        "(case when  ISNULL (FECOFINOTCOR  ,'')           =  ''           then '           '  else FECOFINOTCOR  end ) as fecha_notifica     " & _
                        " FROM EXCEPCIONES WHERE NROEXP =@EXPEDIENTE", con)

            sqlcommand.CommandType = CommandType.Text
            sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", Expediente)
            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanger)
            If sqldatamanger.Rows.Count > 0 Then

                Acto(0) = (sqldatamanger.Rows.Item(0)("resolucion"))
                Acto(1) = CDate(sqldatamanger.Rows.Item(0)("fecha_resolucion")).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Acto(2) = (sqldatamanger.Rows.Item(0)("Num_notifica"))
                Acto(3) = CDate(sqldatamanger.Rows.Item(0)("fecha_notifica")).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

            End If

            Return Acto

        End Using
    End Function

    Public Function SaveTable(ByVal expediente As String, ByVal acto As String, Optional ByVal LIMITE As Double = 0)
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanger As New DataTable
            Dim insert As String
            Dim ops As Integer
            Dim sqlcommand As New SqlCommand

            Select Case acto
                Case "013", " 353", "354", "355", "356"
                    insert = "SP_MANDAMIENTOPAGO"

                Case "316", "229", "230", "359", "360", "363", "351", "352", "315", "368", "366", "233", "367", "370", "314", "382", "383"
                    insert = "SP_COACTIVOS_UPDATE"
                Case "226"
                    insert = "SP_PERSUASIVO_UPDATE"
                Case "319", "320", "321"
                    insert = "SP_EMBARGO"

            End Select

            'sqlcommand = New SqlCommand(insert, con)

            If insert = "SP_COACTIVOS_UPDATE" Then
                Select Case acto
                    Case "316", "366", "367"
                        ops = 3
                    Case "229"
                        ops = 2
                    Case "230", "368"
                        ops = 1
                    Case "351", "352"
                        ops = 4
                    Case "360"
                        ops = 5
                    Case "363"
                        ops = 6
                    Case "315", "233"
                        ops = 7
                    Case "359", "370"
                        ops = 8
                    Case "314"
                        ops = 9
                    Case "382", "383"
                        ops = 10
                End Select

                sqlcommand = New SqlCommand(insert, con)
                sqlcommand.CommandType = CommandType.StoredProcedure
                sqlcommand.Parameters.AddWithValue("@OP", ops)
                sqlcommand.Parameters.AddWithValue("@ACTO", acto)
                sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)

            ElseIf insert = "SP_PERSUASIVO_UPDATE" Then
                Select Case acto
                    Case "226"
                        ops = 1
                End Select

                sqlcommand = New SqlCommand(insert, con)
                sqlcommand.CommandType = CommandType.StoredProcedure
                sqlcommand.Parameters.AddWithValue("@OP", ops)
                sqlcommand.Parameters.AddWithValue("@ACTO", acto)
                sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            ElseIf insert = "SP_EMBARGO" Then
                sqlcommand = New SqlCommand(insert, con)
                sqlcommand.CommandType = CommandType.StoredProcedure
                sqlcommand.Parameters.AddWithValue("@LIMITE", LIMITE)
                sqlcommand.Parameters.AddWithValue("@ACTO", acto)
                sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)

            Else
                sqlcommand = New SqlCommand(insert, con)
                sqlcommand.CommandType = CommandType.StoredProcedure
                sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
                sqlcommand.Parameters.AddWithValue("@COD_ACTO", acto)


            End If

            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanger)



        End Using
    End Function


    Public Function savetable(ByVal resolucion As String, ByVal expediente As String, ByVal subsistema As String, ByVal Capital As Integer, ByVal Intereses As Integer, ByVal total As Integer, Optional ByVal fecha_ejercicio As Date = Nothing)
        If fecha_ejercicio = Nothing Then
            fecha_ejercicio = Today.Date
        End If
        Dim sqlmanager As SqlCommand
        Dim sqlconfig As SqlConnection = New SqlConnection(Funciones.CadenaConexion)
        Dim sqltranss As SqlTransaction

        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()
        End If
        sqlconfig.Open()
        sqltranss = sqlconfig.BeginTransaction
                Try

            ''--Insert Tabla_LiquidacionCredito

            sqlmanager = New SqlCommand("Sp_Liquidacion_Credito")
            sqlmanager.Transaction = sqltranss
            sqlmanager.Connection = sqlconfig
            sqlmanager.CommandType = CommandType.StoredProcedure
            sqlmanager.Parameters.AddWithValue("@CONSE", resolucion)
            sqlmanager.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            sqlmanager.Parameters.AddWithValue("@SUBSISTEMA", subsistema)
            sqlmanager.Parameters.AddWithValue("@CAPITAL", Capital)
            sqlmanager.Parameters.AddWithValue("@TOTAL", total)
            sqlmanager.Parameters.AddWithValue("@INTERESES", Intereses)
            sqlmanager.Parameters.AddWithValue("@FECHA_CORTE", fecha_ejercicio)
            sqlmanager.ExecuteNonQuery()

            ''--Insert Tabla_Coactivo

            sqlmanager = New SqlCommand( _
                                        " update COACTIVO set LiqCredCapital =@CAPITAL ,       " & _
                                        " LiqCredInteres = @INTERESES,LiqCredTotal= @TOTAL ,    " & _
                                        " LiqCredFecCorte  =@FECHA_CORTE                       " & _
                                        " where COACTIVO.NroResLiquiCred =@CONSE               " & _
                                        " and FecResLiquiCred =@FECHA_CORTE AND NroExp =@EXPEDIENTE                   ")
            sqlmanager.Transaction = sqltranss
            sqlmanager.Connection = sqlconfig
            sqlmanager.CommandType = CommandType.Text
            sqlmanager.Parameters.AddWithValue("@CONSE", resolucion)
            sqlmanager.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            sqlmanager.Parameters.AddWithValue("@CAPITAL", Capital)
            sqlmanager.Parameters.AddWithValue("@TOTAL", total)
            sqlmanager.Parameters.AddWithValue("@INTERESES", Intereses)
            sqlmanager.Parameters.AddWithValue("@FECHA_CORTE", fecha_ejercicio)
            sqlmanager.ExecuteNonQuery()

            sqltranss.Commit()



        Catch ex As Exception
            sqltranss.Rollback()
        End Try


    End Function

    Public Function getEmbargo(ByVal op As Integer, ByVal expediente As String) As Double
        Dim tasa As Double
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanger As New DataTable
            Dim sqlcommand As New SqlCommand("select porcentaje  from embargos where  nroExp=@expediente ", con)
            sqlcommand.CommandType = CommandType.Text
            sqlcommand.Parameters.AddWithValue("@expediente", expediente)
            sqladapter = New SqlDataAdapter(sqlcommand)
            sqladapter.Fill(sqldatamanger)
            If sqldatamanger.Rows.Count > 0 Then


                If op = 0 Then
                    tasa = Math.Round(sqldatamanger.Rows.Item(0)(0), 3)
                Else
                    tasa = Nothing
                End If

            End If

            Return tasa
        End Using


    End Function

    Public Function GetLiquidacion(ByVal expediente As String, ByVal resolucion As String) As Double
        Dim RETORNO As Double

        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim sqladapter As New SqlDataAdapter("select SUM(LCC_Total) AS TOTAL from LIQUIDACIONCREDITO where LCC_NroExp ='" & expediente & "' and LCC_CONSE='" & resolucion & "'", sqlconfig)
        sqladapter.Fill(sqlmanager)

        If sqlmanager.Rows.Count > 0 Then
            If sqlmanager.Rows(0).Item("TOTAL").ToString = "" Then
                RETORNO = 0
            Else
                RETORNO = CDbl(sqlmanager.Rows(0).Item("TOTAL"))
            End If
        Else
            RETORNO = 0
        End If


        Return RETORNO


    End Function

    Public Function GETPROCEDENCIA(ByVal expediente As String) As String
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim sqladapter As New SqlDataAdapter("SELECT NOMBRE FROM PROCEDENCIA_TITULOS ,MAESTRO_TITULOS WHERE MT_procedencia = codigo AND MT_expediente ='" & expediente & "'", sqlconfig)
        sqladapter.Fill(sqlmanager)

        If sqlmanager.Rows.Count > 0 Then
            Return sqlmanager.Rows.Item(0)(0)
        Else
            Return "SUBDIRECCION DE DETERMINACION"
        End If



    End Function

    Public Function GETPERSUASIVO(ByVal expediente As String) As String()
        Dim oficios(4) As String
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim sqladapter As New SqlDataAdapter(" SELECT (CASE WHEN NROOFI1 ='SIN DATOS' THEN ''ELSE NROOFI1 END ) AS NROOF1," & _
                                             " ISNULL(FECENVOFI1,'')AS FECHA1, " & _
                                             " (CASE WHEN NROOFI2 ='SIN DATOS' THEN ''ELSE NROOFI2 END ) AS NROOFI2," & _
                                             " ISNULL(FECENVOFI2,'')AS FECHA2 " & _
                                             " FROM PERSUASIVO    " & _
                                             " WHERE NROEXP ='" & expediente & "'   ", sqlconfig)

        sqladapter.Fill(sqlmanager)

        If sqlmanager.Rows.Count > 0 Then
            oficios(0) = (sqlmanager.Rows.Item(0)(0)).ToString.Trim
            oficios(1) = CDate(sqlmanager.Rows.Item(0)(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
            oficios(2) = (sqlmanager.Rows.Item(0)(2)).ToString.Trim
            oficios(3) = CDate(sqlmanager.Rows.Item(0)(3)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
        Else
            oficios(0) = Nothing
            oficios(1) = Nothing
            oficios(2) = Nothing
            oficios(3) = Nothing

        End If
        Return oficios


    End Function

    Public Function GetTituloPrincipal(ByVal expediente As String) As String()
        Dim TDJ(2) As String
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim sqladapter As New SqlDataAdapter("SELECT " & _
                                             "     A.NroResolEm   ,	 A.FecResolEm   ,    C.NroDeposito  , " & _
                                             "     C.NroTituloJ   ,	 C.ValorTDJ     ,	 C.NroResolGes  ,	 C.FecResolGes  ,  " & _
                                             "	 H.nombre         ,	 D.nombre       ,    A.LIMITEEM " & _
                                             "FROM   " & _
                                             "     EMBARGOS A       ,      DETALLE_EMBARGO B ,      TDJ C           , " & _
                                             "     TIPOS_RESOLTDJ D ,      ESTADOS_EMBARGO F ,      TIPOS_BIENES G  , " & _
                                             "     ESTADOS_TDJ H    ,      MAESTRO_BANCOS I  " & _
                                             "    WHERE " & _
                                             "     A.NroExp ='" & expediente & "' " & _
                                             "  AND A.NroResolEm       = B.NroResolEm  " & _
                                             "	AND B.NroDepositoTDJ   = C.NroDeposito  " & _
                                             "	AND C.IdEmbargo		   = B.idunico      " & _
                                             "	AND D.codigo		   = C.EstadoTDJ    " & _
                                             "	AND F.codigo           = A.EstadoEm     " & _
                                             "	AND G.codigo           = B.TipoBien     " & _
                                             "	AND H.codigo           = C.EstadoTDJ    " & _
                                             "	AND I.BAN_CODIGO       = B.Banco        ", sqlconfig)

        sqladapter.Fill(sqlmanager)
        If sqlmanager.Rows.Count > 0 Then
            For i = 0 To sqlmanager.Rows.Count - 1
                If sqlmanager.Rows.Item(0)("LIMITEEM") <= sqlmanager(i)("ValorTDJ") Then
                    TDJ(0) = sqlmanager(i)("NroTituloJ")
                    TDJ(1) = sqlmanager(i)("ValorTDJ")
                    TDJ(2) = sqlmanager(0)("LIMITEEM")
                End If

            Next
            
        Else
            TDJ(0) = ""
            TDJ(1) = ""
            TDJ(2) = ""

        End If
        Return TDJ
    End Function

    Public Function GetTituloPrincipal2(ByVal expediente As String, Optional ByVal tipo As Integer = 0) As DataTable
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim tipos As String
        Select Case tipo
            Case 1
                tipos = "AND TIPORESOLGES <>3 AND TIPORESOLGES <> 1"
            Case Else
                tipos = ""
        End Select

        Dim sqladapter As New SqlDataAdapter("SELECT   C.NroTituloJ   ,'' AS FECHA_COSTITUCION,J.ED_Nombre ,   I.BAN_NOMBRE  ,'$ '+CAST(CONVERT(varchar, CAST(C.ValorTDJ AS money), 1) AS varchar) AS CUANTIA " & _
            " FROM                                                                " & _
            " EMBARGOS A       ,      DETALLE_EMBARGO B ,      TDJ C            , " & _
            " TIPOS_RESOLTDJ D ,      ESTADOS_EMBARGO F ,      TIPOS_BIENES G   , " & _
            " ESTADOS_TDJ H    ,      MAESTRO_BANCOS I  ,      ENTES_DEUDORES J , " & _
            " EJEFISGLOBAL K                               " & _
            " WHERE                                        " & _
            " A.NroResolEm       = B.NroResolEm            " & _
            " AND B.NroDepositoTDJ   = C.NroDeposito       " & _
            " AND C.IdEmbargo		   = B.idunico         " & _
            " AND D.codigo		   = C.EstadoTDJ           " & _
            " AND F.codigo           = A.EstadoEm          " & _
            " AND G.codigo           = B.TipoBien          " & _
            " AND H.codigo           = C.EstadoTDJ         " & _
            " AND I.BAN_CODIGO       = B.Banco             " & _
            " AND A.NroExp           = K.EFINROEXP         " & _
            " AND K.EFINIT           = J.ED_Codigo_Nit     " & _
            " AND A.NroExp = '" & expediente & "'" & tipos & "          ", sqlconfig)

        sqladapter.Fill(sqlmanager)
        If sqlmanager.Rows.Count > 0 Then
            Return sqlmanager
        Else
            sqlmanager.Rows.Add("no hay registro")

            Return sqlmanager
        End If
    End Function

    Public Function GetTituloPrincipal3(ByVal expediente As String) As DataTable
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim sqladapter As New SqlDataAdapter("SELECT   C.NroTituloJ   ,'' AS FECHA_COSTITUCION, I.BAN_NOMBRE  ,'$ '+CAST(CONVERT(varchar, CAST(C.ValorTDJ AS money), 1) AS varchar) AS CUANTIA " & _
            " FROM                                                                " & _
            " EMBARGOS A       ,      DETALLE_EMBARGO B ,      TDJ C            , " & _
            " TIPOS_RESOLTDJ D ,      ESTADOS_EMBARGO F ,      TIPOS_BIENES G   , " & _
            " ESTADOS_TDJ H    ,      MAESTRO_BANCOS I  ,      ENTES_DEUDORES J , " & _
            " EJEFISGLOBAL K                               " & _
            " WHERE                                        " & _
            " A.NroResolEm       = B.NroResolEm            " & _
            " AND B.NroDepositoTDJ   = C.NroDeposito       " & _
            " AND C.IdEmbargo		   = B.idunico         " & _
            " AND D.codigo		   = C.EstadoTDJ           " & _
            " AND F.codigo           = A.EstadoEm          " & _
            " AND G.codigo           = B.TipoBien          " & _
            " AND H.codigo           = C.EstadoTDJ         " & _
            " AND I.BAN_CODIGO       = B.Banco             " & _
            " AND A.NroExp           = K.EFINROEXP         " & _
            " AND K.EFINIT           = J.ED_Codigo_Nit     " & _
            " AND A.NroExp = '" & expediente & "'          ", sqlconfig)

        sqladapter.Fill(sqlmanager)
        If sqlmanager.Rows.Count > 0 Then
            Return sqlmanager
        Else
            sqlmanager.Rows.Add("no hay registro")

            Return sqlmanager
        End If
    End Function

    Public Function homologo(ByVal acto As String) As String

        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim sqladapter As New SqlDataAdapter( _
                  " SELECT C.COD_ACTO  FROM ACTUACIONES A,DOCUMENTO_INFORMEXIMPUESTO B,HOMOLOGO_ACTUACIONES C ,DOCUMENTOS_GENERADOS D " & _
                  " WHERE A.codigo =C.ACTO_SECUNDARIO    " & _
                  " AND   B.DXI_ACTO = C.ACTO_SECUNDARIO " & _
                  " AND C.ACTO_SECUNDARIO=D.DG_COD_ACTO  " & _
                  " AND C.ACTO_SECUNDARIO='" & acto & "'", sqlconfig)


        sqladapter.Fill(sqlmanager)

        If sqlmanager.Rows.Count > 0 Then

            Return sqlmanager.Rows(0).Item(0)

        Else

            Return Nothing


        End If



    End Function

    Public Function saveResolucion(ByVal expediente As String, ByVal acto As String, Optional ByVal op As Integer = 0) As String()
        Dim resolucion As DataTable
        Dim guardarResolucion(3) As String
        Select Case op
            Case 0
                resolucion = InsertReslucion(expediente, acto)
            Case 1
                resolucion = InsertReslucion2(expediente, acto)
        End Select


        Select Case op

            Case 1
                If resolucion.Rows.Count > 0 Then
                    guardarResolucion(0) = resolucion.Rows.Item(resolucion.Rows.Count - 1)(3)
                    guardarResolucion(1) = CDate(resolucion.Rows.Item(resolucion.Rows.Count - 1)(4)).ToString("'del' dd 'de' MMMM 'de' yyy")
                    guardarResolucion(2) = resolucion.Rows.Item(resolucion.Rows.Count - 1)(4)
                    guardarResolucion(3) = getfecha_tipada(CDate(resolucion.Rows.Item(0)(4)))
                Else
                    Return Nothing

                End If


            Case 0
                If resolucion.Rows.Count > 0 Then
                    guardarResolucion(0) = resolucion.Rows.Item(0)(3)
                    guardarResolucion(1) = CDate(resolucion.Rows.Item(0)(4)).ToString("'del' dd 'de' MMMM 'de' yyy")
                    guardarResolucion(2) = resolucion.Rows.Item(0)(4)
                    guardarResolucion(3) = getfecha_tipada(CDate(resolucion.Rows.Item(0)(4)))
                Else
                    Return Nothing

                End If


        End Select

        Return guardarResolucion
    End Function

    Public Function getfraccionado(ByVal tituloprincipal As String) As Double()
        Dim vec(1) As Double
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New DataTable
        Dim sqladapter As New SqlDataAdapter(" select nrotituloj ,valortdj,estadotdj,nroresolges,fecresolges " & _
                                             " from TDJ where NroTituloPrincipal ='" & tituloprincipal & "'", sqlconfig)

        sqladapter.Fill(sqlmanager)
        If sqlmanager.Rows.Count > 0 Then
            vec(0) = sqlmanager(0).Item(1)
            vec(1) = sqlmanager(1).Item(1)

        Else
            vec(0) = Nothing
            vec(1) = Nothing
        End If

        Return vec
    End Function
    Public Function InsertReslucion2(ByVal expediente As String, ByVal acto_administrativo As String) As DataTable
        Using con As New SqlConnection(Funciones.CadenaConexion)

            Dim adapter As SqlDataAdapter
            Dim tb As New DataTable
            Dim command As New SqlCommand("REGISTRAR_DOCUMENTO_GENERADO2", con)
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.AddWithValue("@EXPEDIENTE", expediente)
            command.Parameters.AddWithValue("@ACTO", acto_administrativo)
            adapter = New SqlDataAdapter(command)
            adapter.Fill(tb)

            Return tb
        End Using
    End Function

    Public Function savetable(ByVal Expediente As String, ByVal Acto As String, ByVal Resolucion As String, ByVal Fecres As Date, ByVal limite As Double, ByVal porcentaje As String)
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        ''Dim sqldatamanger As New DataTable
        Dim sqlcommand As New SqlCommand("SP_EMBARGOS")
        sqlcommand.CommandType = CommandType.StoredProcedure
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", Expediente.Trim.ToUpper)
        sqlcommand.Parameters.AddWithValue("@ACTO", Acto.Trim.ToUpper)
        sqlcommand.Parameters.AddWithValue("@RESOLUCION", Resolucion.Trim.ToUpper)
        sqlcommand.Parameters.AddWithValue("@FECRES", Fecres)
        sqlcommand.Parameters.AddWithValue("@LIMITE", limite)
        sqlcommand.Parameters.AddWithValue("@PORCENTAJE", porcentaje)
        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()

        End If
        sqlconfig.Open()

        sqlcommand.ExecuteNonQuery()
    End Function
    Public Function overloadresolucion(ByVal expediente As String, ByVal acto As String) As String()
        Dim vc_overload(2) As String
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanger As New DataTable
        Dim sqlmanager As SqlDataAdapter
        Dim sqlcommand As New SqlCommand("SP_COACTIVOS_SELECT")

        sqlcommand.CommandType = CommandType.StoredProcedure
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", expediente.Trim.ToUpper)
        sqlcommand.Parameters.AddWithValue("@ACTO", acto.Trim.ToUpper)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()

        End If
        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanger)
        If sqldatamanger.Rows.Count > 0 Then
            If sqldatamanger.Rows(0).Item(0).ToString.Trim <> "" And sqldatamanger.Rows(0).Item(0).ToString.Trim <> "Sin Datos" Then
                vc_overload(0) = sqldatamanger.Rows(0).Item(0)
                vc_overload(1) = IIf(sqldatamanger.Rows(0).Item(1) Is Nothing, "", sqldatamanger.Rows(0).Item(1)).ToString
                vc_overload(2) = getfecha_tipada(CDate(IIf(sqldatamanger.Rows(0).Item(1) Is Nothing, "", sqldatamanger.Rows(0).Item(1)).ToString))
            Else
                vc_overload(0) = ""
                vc_overload(1) = ""
                vc_overload(2) = ""
            End If
        Else
            vc_overload(0) = ""
            vc_overload(1) = ""
            vc_overload(2) = ""
        End If
        Return vc_overload
    End Function


    Public Function Load_multiDirecciones(ByVal DEUDOR As String, ByVal tipo As String) As String
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanager As New DataTable
        Dim sqlmanager As SqlDataAdapter
        Dim sqlcommand As New SqlCommand("SELECT  A.DIRECCION   FROM  DIRECCIONES A  WHERE   A.DEUDOR = @DEUDOR GROUP BY A.DIRECCION")
        Dim direcciones As String = ""
        Dim RUP(1) As String
        Select Case tipo
            Case "NIT"
                ''DEUDOR = DEUDOR.Substring(0, 9)
                ''DEUDOR = DEUDOR.Replace("-", "").Substring(0, 9)
                RUP = DEUDOR.Split("-")
                DEUDOR = RUP(0)
            Case "C.C."

        End Select

        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@DEUDOR", DEUDOR)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()

        End If

        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanager)

        If sqldatamanager.Rows.Count > 0 Then
            For i = 0 To sqldatamanager.Rows.Count - 1
                If direcciones = "" Then
                    direcciones = sqldatamanager.Rows(i).Item(0)
                Else
                    direcciones = direcciones & " - " & sqldatamanager.Rows(i).Item(0)
                End If
            Next
            Return direcciones

        Else
            Return "No hay Direccion registrada"

        End If

    End Function

    Public Function getfecha_tipada(ByVal fecha As Date) As String
        Dim Mes() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
        Dim Letters As New System.Text.StringBuilder
        Letters.Append("El dia ")
        Letters.Append(IIf(fecha.Day = 1, "PRIMERO", Num2Text(fecha.Day)))
        Letters.Append(" (")
        Letters.Append(fecha.Day)
        Letters.Append(") del mes de ")
        Letters.Append(Mes(fecha.Month - 1))
        Letters.Append(" del ")
        Letters.Append(Num2Text(fecha.Year))
        Letters.Append(" (")
        Letters.Append(fecha.Year)
        Letters.Append(")")

        Return Letters.ToString.ToUpper

    End Function
    Public Function loadMultisocios(ByVal expedientes As String, ByVal deuda As Double) As String()
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanager As New DataTable
        Dim sqlmanager As SqlDataAdapter
        Dim sociedad As String = ""
        Dim valores As String = ""
        Dim socios(2) As String
        Dim socio As String = ""
        Dim sqlcommand As New SqlCommand("select A.deudor ,B.ED_Nombre ,C.nombre, A.Participacion   from DEUDORES_EXPEDIENTES A, ENTES_DEUDORES B , TIPOS_IDENTIFICACION  C " & _
                                         " WHERE A.deudor =B.ED_Codigo_Nit " & _
                                         " AND C.codigo =B.ED_TipoId  " & _
                                         " AND A.tipo =2  " & _
                                         " AND A.NroExp =@EXPEDIENTES ")


        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTES", expedientes)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()

        End If

        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanager)

        If sqldatamanager.Rows.Count > 0 Then
            For i = 0 To sqldatamanager.Rows.Count - 1
                If sociedad = "" Then
                    sociedad = sqldatamanager.Rows(i).Item("ED_NOMBRE") & " " & "identificado con " & sqldatamanager.Rows(i).Item("nombre") & sqldatamanager.Rows(i).Item("deudor")
                    valores = String.Format("{0:C0}", (deuda * (sqldatamanager.Rows(i).Item("Participacion") / 100)))
                    socio = sqldatamanager.Rows(i).Item("ED_NOMBRE")
                Else
                    sociedad = sociedad & " , " & sqldatamanager.Rows(i).Item("ED_NOMBRE") & " " & "identificado con " & sqldatamanager.Rows(i).Item("nombre") & sqldatamanager.Rows(i).Item("deudor")
                    valores = valores & "," & String.Format("{0:C0}", (deuda * (sqldatamanager.Rows(i).Item("Participacion") / 100)))
                    socio = socio & sqldatamanager.Rows(i).Item("ED_NOMBRE")
                End If

            Next
            socios(0) = sociedad
            socios(1) = valores
            socios(2) = socio

        Else

            socios(0) = ""
            socios(1) = ""
            socios(2) = ""
        End If

        Return socios
    End Function

    Public Function LoadMultiEmbargos(ByVal expediente As String) As String()
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanager As New DataTable
        Dim sqlmanager As SqlDataAdapter
        Dim sqlcommand As New SqlCommand("SELECT NroResolEm ,LimiteEm  FROM EMBARGOS where NroExp =@Expediente")
        Dim embargos(1) As String
        Dim valores As String = ""
        Dim resoluciones As String = ""
        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@Expediente", expediente)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()

        End If

        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanager)

        If sqldatamanager.Rows.Count > 0 Then
            For i = 0 To sqldatamanager.Rows.Count - 1
                If valores = "" Then
                    valores = String.Format("{0:C0}", CDbl(sqldatamanager.Rows(i).Item(1)))
                    resoluciones = sqldatamanager.Rows(i).Item(0)
                Else
                    valores = valores & " , " & String.Format("{0:C0}", CDbl(sqldatamanager.Rows(i).Item(1)))
                    resoluciones = resoluciones & "," & sqldatamanager.Rows(i).Item(0)
                End If
            Next

            embargos(0) = resoluciones
            embargos(1) = valores
        Else
            embargos(0) = ""
            embargos(1) = ""

        End If

        Return embargos
    End Function
    Public Function tabladatosembargos(ByVal expedientes As String) As DataTable
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanager As New DataTable
        Dim sqlmanager As SqlDataAdapter
        Dim sociedad As String = ""
        Dim valores As String = ""
        Dim socios(2) As String
        Dim socio As String = ""
        Dim sqlcommand As New SqlCommand("    SELECT B.NroExp as Proceso,A.ED_Nombre as Obligado, " & _
                                         "    A.ED_CODIGO_NIT + CASE WHEN ISNULL(A.ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN A.ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(A.ED_DIGITOVERIFICACION,'') END AS CEDULA,'' AS LIMITE,'' AS PORCENTAJE , B.PARTICIPACION,b.tipo AS TIPO" & _
                                         "    FROM ENTES_DEUDORES A,  " & _
                                         "	DEUDORES_EXPEDIENTES B, " & _
                                         "	TIPOS_IDENTIFICACION C  " & _
                                         "	WHERE A.ED_Codigo_Nit =B.deudor  " & _
                                         "	AND  C.codigo =B.tipo  and   B.NroExp =@EXPEDIENTES  and B.tipo <> '03'       ")


        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTES", expedientes)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()

        End If

        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanager)

        If sqldatamanager.Rows.Count > 0 Then
            Return sqldatamanager
        Else
            Return Nothing
        End If


    End Function

    Public Function RetornarDatatable(ByVal sql As String, ByVal coneccion As SqlConnection, Optional ByVal trans As SqlTransaction = Nothing) As DataTable
        Dim MyAdapter As New SqlDataAdapter
        Dim command As New SqlCommand
        If trans Is Nothing Then
            command = New SqlCommand(sql, coneccion)
            MyAdapter = New SqlDataAdapter(command)
        Else
            command = New SqlCommand(sql, coneccion, trans)
            MyAdapter = New SqlDataAdapter(command)
        End If

        Dim Mytb As New DataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function
    Public Function Mostrar_Saldo_Actual(ByVal Expediente As String) As String
        Dim Saldo_Actual, Total_Deuda, Total_Capital As String
        If Expediente <> "" Then

            'Conexion a la base de datos
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            'Consultar el total de la deuda
            Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " & _
                                          "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & Expediente.Trim & "' GROUP BY MT_expediente"
            Dim Command As New SqlCommand(sql, Connection)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                'Dim n As Double = Convert.ToDouble(Reader("totaldeuda").ToString())
                'txtTotalDeudaEA.Text = n.ToString("N0")                
                Total_Deuda = Convert.ToDouble(Reader("totaldeuda").ToString()).ToString("N0")
            Else
                Total_Deuda = "0"
            End If
            Reader.Close()

            '20/10/2014. Se incorporó el ajuste 1406 dentro del capital. xxxyyy
            'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
            Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital " & _
                                    "FROM pagos WHERE NroExp = '" & Expediente.Trim & "' GROUP BY NroExp"

            Dim Command2 As New SqlCommand(sql2, Connection)
            Dim Reader2 As SqlDataReader = Command2.ExecuteReader
            If Reader2.Read Then
                If Reader2("pagcapital").ToString() = "" Then
                    Total_Capital = "0"
                Else
                    Total_Capital = Convert.ToDouble(Reader2("pagcapital").ToString()).ToString("N0")
                End If
            Else
                Total_Capital = "0"
            End If
            Reader2.Close()

            'Mostrar la diferencia entre Total deuda - Capital pagado
            Dim saldoEA As Double = Convert.ToDouble(Total_Deuda) - Convert.ToDouble(Total_Capital)
            Saldo_Actual = saldoEA.ToString("N0")

            Connection.Close()

        End If
        Return Saldo_Actual
    End Function

    Public Function PazySalvoFacilidadPagoMultas(ByVal pExpediente As String) As Integer

        Dim tablaretorno As DataTable = RetornarDatatable("SELECT CASE WHEN SUM( ISNULL(B.VALOR_PAGADO,0))  >= sum(ISNULL(B.VALOR_CUOTA,0)) THEN 1 ELSE 0 END PAGO, SUM( ISNULL(B.VALOR_PAGADO,0))VALOR_PAGADO  , sum(ISNULL(B.VALOR_CUOTA,0)) VALOR_CUOTA FROM MAESTRO_ACUERDOS A, DETALLES_ACUERDO_PAGO B WHERE A.DOCUMENTO = B.DOCUMENTO AND   A.EXPEDIENTE = '" & pExpediente & "'", New SqlConnection(Funciones.CadenaConexion), Nothing)


        Return CInt(tablaretorno.Rows(0).Item("PAGO"))
    End Function


    Public Function GET_ESTADO_PROCESO(ByVal expedientes As String) As String
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanager As New DataTable
        Dim sqlmanager As SqlDataAdapter

        Dim sqlcommand As New SqlCommand("SELECT EP.NOMBRE   FROM ESTADOS_PROCESO EP ,EJEFISGLOBAL EJF WHERE EJF.EFIESTADO = EP.CODIGO AND EJF.EFINROEXP =@EXPEDIENTES")
        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTES", expedientes)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()

        End If

        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanager)

        If sqldatamanager.Rows.Count > 0 Then
            Return sqldatamanager.Rows(0).Item(0)
        Else
            Return Nothing
        End If

    End Function
    Public Function GET_RESOLUCION_APELACION(ByVal expedientes As String) As String()
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanager As New DataTable
        Dim sqlmanager As SqlDataAdapter
        Dim resolucion_apelacion(1) As String

        Dim sqlcommand As New SqlCommand("SELECT MT.MT_RESO_RESU_APELA_RECON, MT.MT_FEC_EXP_RESO_APELA_RECON  FROM MAESTRO_TITULOS MT WHERE MT_expediente =@EXPEDIENTES")
        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTES", expedientes)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()
        End If

        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanager)

        If sqldatamanager.Rows.Count > 0 And sqldatamanager.Rows(0).Item("MT_RESO_RESU_APELA_RECON") <> "sin datos" And sqldatamanager.Rows(0).Item("MT_RESO_RESU_APELA_RECON") <> "" Then
            resolucion_apelacion(0) = sqldatamanager.Rows(0).Item("MT_RESO_RESU_APELA_RECON") '' numero de resolucion 

            If IsDBNull(sqldatamanager.Rows(0).Item("MT_FEC_EXP_RESO_APELA_RECON")) Then
                resolucion_apelacion(1) = ""
            Else
                resolucion_apelacion(1) = getfecha_tipada(CDate(sqldatamanager.Rows(0).Item("MT_FEC_EXP_RESO_APELA_RECON"))) '' fecha de la resolucion
            End If


        Else
            resolucion_apelacion(0) = ""
            resolucion_apelacion(1) = ""
        End If
        Return resolucion_apelacion
    End Function

    Public Function Cambiar_Estado_Suspendido(ByVal Expedientes As String) As Boolean
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanager As New DataTable
        Dim sqlmanager As SqlDataAdapter

        Dim sqlcommand As New SqlCommand("UPDATE EJEFISGLOBAL SET EFIESTADO ='08' WHERE EFINROEXP =@EXPEDIENTES")
        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTES", Expedientes)

        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()
        End If

        sqlconfig.Open()
        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqldatamanager)

        If sqldatamanager.Rows.Count > 0 Then
            Return True
        Else
            Return False

        End If

    End Function

End Module