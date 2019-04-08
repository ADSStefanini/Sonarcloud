Imports System.Data.SqlClient
Imports System.IO

Partial Public Class subirSQL
    Inherits System.Web.UI.Page

    Protected Shared expxls As New DataTable
    Protected Shared DatosImportado As New DataTable

    Protected Sub cmdImportarcsv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImportarcsv.Click


        Dim filePath As String = String.Empty
        If upload.HasFile AndAlso upload.PostedFile.ContentType.Equals("application/vnd.ms-excel") Then
            'Gridinteres.DataSource = DirectCast(ReadToEnd(upload.PostedFile.FileName), DataTable)
            DatosImportado = New DataTable
            DatosImportado = CType(ConstruirDatatable(upload.PostedFile.FileName, ";"), DataTable)
            Dim mensajeValidacion As String = ""
            Try


                If DatosImportado.Rows.Count > 0 Then
                    Dim TotalAjuste As Double = 0
                    For row As Integer = 0 To DatosImportado.Rows.Count - 1
                        TotalAjuste = TotalAjuste + CDbl(DatosImportado.Rows(row).Item("AJUSTE"))
                    Next

                    mensajeValidacion = Validaciones(DatosImportado, CStr(DatosImportado.Rows(0).Item("EXPEDIENTE")), TotalAjuste)
                Else
                    lblError.Text = "No se detectó informacion a subir"
                    Exit Sub
                End If
            Catch ex As Exception
                lblError.Text = "Error: " & ex.ToString
            End Try

            If mensajeValidacion = "" Then

                Dim cn As New SqlConnection(Funciones.CadenaConexion)
                cn.Open()
                Dim _trans As SqlTransaction
                _trans = cn.BeginTransaction

                Dim ER As String = ""
                Try
                    ModificarSQLPLanilla(CStr(DatosImportado.Rows(0).Item("EXPEDIENTE")), _trans, cn)
                    For row As Integer = 0 To DatosImportado.Rows.Count - 1
                        ER = " - 2. Error en la fila " & row
                        Save_SQL(CStr(DatosImportado.Rows(row).Item("EXPEDIENTE")), CDbl(DatosImportado.Rows(row).Item("LIQ_REC")), CDbl(DatosImportado.Rows(row).Item("INF")), CStr(DatosImportado.Rows(row).Item("SUBSISTEMA")), CStr(DatosImportado.Rows(row).Item("NIT_EMPRESA")), CStr(DatosImportado.Rows(row).Item("RAZON_SOCIAL")), CDbl(DatosImportado.Rows(row).Item("ANNO")), CDbl(DatosImportado.Rows(row).Item("MES")), CStr(DatosImportado.Rows(row).Item("CEDULA")), CStr(DatosImportado.Rows(row).Item("NOMBRE")), CDbl(DatosImportado.Rows(row).Item("IBC")), CDbl(DatosImportado.Rows(row).Item("AJUSTE")), Nothing, CDbl(DatosImportado.Rows(row).Item("ID_GRUPO")), _trans, cn)

                    Next
                    Gridinteres.DataSource = DatosImportado
                    Gridinteres.DataBind()
                    _trans.Commit()

                    lblError.Text = DatosImportado.Rows.Count & " registro(s) almacenados."
                Catch ex As Exception
                    _trans.Rollback()
                    lblError.Text = "Error: por favor compruebe la información del archivo, 1. Es posible que hallan filas en blanco ó falten columnas a importar " & ER

                Finally
                    cn.Close()
                End Try
            Else
                lblError.Text = mensajeValidacion
            End If

        Else
            lblError.Text = "Por favor, compruebe el tipo de archivo seleccionado"
            lblError.Visible = True
        End If
    End Sub

    Private Function ReadToEnd(ByVal filePath As String) As Object
        Dim dtDataSource As New DataTable()
        Dim TargetPath As String = HttpRuntime.AppDomainAppPath & "temp_arch\" & Path.GetFileName(filePath)
        upload.PostedFile.SaveAs(TargetPath)

        Dim fileContent As String() = File.ReadAllLines(TargetPath)
        Try


            If fileContent.Count() > 0 Then
                'Agregar Columnas de archivo
                Dim columns As String() = fileContent(0).Split(";"c)
                For i As Integer = 0 To columns.Count() - 1
                    dtDataSource.Columns.Add(columns(i))
                Next
                'agregar filas.
                For i As Integer = 1 To fileContent.Count() - 1
                    Dim rowData As String() = fileContent(i).Split(";"c)
                    dtDataSource.Rows.Add(rowData)
                Next

            End If
        Catch ex As Exception
            lblError.Text = "Error: " & ex.ToString & "Compruebe el archivo .csv"
        End Try
        Return dtDataSource
    End Function

    Private Sub Save_SQL(ByVal EXPEDIENTE As String, ByVal LIQ_REC As Integer, ByVal INF As String, ByVal SUBSISTEMA As String, ByVal NIT_EMPRESA As String, ByVal RAZON_SOCIAL As String, ByVal ANNO As Integer, ByVal MES As Integer, ByVal CEDULA As String, ByVal NOMBRE As String, ByVal IBC As Double, ByVal AJUSTE As Double, ByVal FECHA_PAGO As Date, ByVal ID_GRUPO As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

        Dim command As New SqlCommand("Save_SQL", cn, _trans)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.AddWithValue("@SUBSISTEMA", SUBSISTEMA)
        command.Parameters.AddWithValue("@NIT_EMPRESA", NIT_EMPRESA)
        command.Parameters.AddWithValue("@RAZON_SOCIAL", RAZON_SOCIAL)
        command.Parameters.AddWithValue("@INF", INF)
        command.Parameters.AddWithValue("@ANNO", ANNO)
        command.Parameters.AddWithValue("@MES", MES)
        command.Parameters.AddWithValue("@CEDULA", CEDULA)
        command.Parameters.AddWithValue("@NOMBRE", NOMBRE)
        command.Parameters.AddWithValue("@IBC", IBC)
        command.Parameters.AddWithValue("@AJUSTE", AJUSTE)
        command.Parameters.AddWithValue("@EXPEDIENTE", EXPEDIENTE)
        command.Parameters.AddWithValue("@FECHA_PAGO", Now())
        command.Parameters.AddWithValue("@LIQ_REC", LIQ_REC)
        command.Parameters.AddWithValue("@ID_GRUPO", ID_GRUPO)
        command.Parameters.AddWithValue("@MONTO_PAGO", 0)
        command.ExecuteNonQuery()

    End Sub

    Protected Sub cmdExportarExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdExportarExcel.Click


        If Gridinteres.Rows.Count > 0 Then

            If rbsqlinteres.Checked = False And rbVerificacionpago.Checked = False And rbsql.Checked = False Then
                lblError.Text = "Por favor elija el formato a exportar..."
                Exit Sub
            End If

            If DatosImportado.Rows.Count > 0 Then
                Dim _tb As DataTable = TableExport(CStr(DatosImportado.Rows(0).Item("EXPEDIENTE")), False)
                Dim nombre As String = "SQL" & CStr(DatosImportado.Rows(0).Item("EXPEDIENTE")) & ".xls"
                descargafichero(_tb, nombre, True)
                lblError.Text = nombre & " exportado con exito..."
            Else
                lblError.Text = "Por favor consulte el proceso a exportar..."
            End If

        ElseIf _rbsql.Checked Then
            Dim _tb As DataTable = TableExport("")
            Const nombre As String = "SQL.xls"

            descargafichero(_tb, nombre, True)

            lblError.Text = nombre & " exportado con exito..."
        Else
            lblError.Text = "No se encontraron datos a exportar "
        End If
    End Sub

    Private Sub descargafichero(ByVal dtTemp As DataTable, ByVal fname As String, ByVal forceDownload As Boolean)

        Dim ext As String = Path.GetExtension(fname)
        Dim type As String = ""
        ' mirar las extensiones conocidas	
        If ext IsNot Nothing Then
            Select Case ext.ToLower()
                Case ".htm", ".html"
                    type = "text/HTML"
                    Exit Select

                Case ".txt"
                    type = "text/plain"
                    Exit Select

                Case ".doc", ".rtf"
                    type = "Application/msword"
                    Exit Select

                Case ".xls"
                    type = "Application/vnd.ms-excel"
                    Exit Select

            End Select
        End If

        Response.Clear()
        Response.Buffer = True

        If forceDownload Then
            Response.AppendHeader("content-disposition", "attachment; filename=" & fname)
        End If
        If type <> "" Then
            Response.ContentType = type
        End If

        Dim sb As StringBuilder = New StringBuilder()
        Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
        Dim dg As New DataGrid()

        dg.DataSource = dtTemp
        dg.DataBind()

        dg.RenderControl(htw)
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(SW)
        Response.End()

    End Sub

    Private Function TableExport(ByVal expediente As String, Optional ByVal buscar As Boolean = False) As DataTable
        Dim dt As New DataTable()
        Dim sql As String = ""
        If rbsql.Checked Or buscar = True Then
            dt.Columns.AddRange(New DataColumn() {New DataColumn("EXPEDIENTE", GetType(String)),
                                             New DataColumn("LIQ_REC", GetType(Integer)),
                                             New DataColumn("INF", GetType(String)),
                                             New DataColumn("SUBSISTEMA", GetType(String)),
                                             New DataColumn("NIT_EMPRESA", GetType(String)),
                                             New DataColumn("RAZON_SOCIAL", GetType(String)),
                                             New DataColumn("ANNO", GetType(Integer)),
                                             New DataColumn("MES", GetType(Integer)),
                                             New DataColumn("CEDULA", GetType(String)),
                                             New DataColumn("NOMBRE", GetType(String)),
                                             New DataColumn("IBC", GetType(Double)),
                                             New DataColumn("AJUSTE", GetType(Double)),
                                             New DataColumn("ID_GRUPO", GetType(String))})
            If buscar Then
                sql = "SELECT EXPEDIENTE,LIQ_REC ,INF,SUBSISTEMA,NIT_EMPRESA,RAZON_SOCIAL,ANNO,MES,CEDULA,NOMBRE,IBC,AJUSTE,ID_GRUPO FROM SQL_PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE ORDER BY ANNO,MES"
            Else
                sql = "SELECT TOP 1 '' AS EXPEDIENTE,0 AS LIQ_REC ,'' AS INF,'' AS SUBSISTEMA,'' AS NIT_EMPRESA,'' AS RAZON_SOCIAL,0 AS ANNO,0 AS MES,'' AS CEDULA,'' AS NOMBRE,0 AS IBC,0 AS AJUSTE,'1-OMISOS,2- INEXACTOS, 3-MORA' AS ID_GRUPO FROM SQL_PLANILLA "
            End If


        ElseIf rbsqlinteres.Checked Then

            dt.Columns.AddRange(New DataColumn() {New DataColumn("EXPEDIENTE", GetType(String)),
                                             New DataColumn("LIQ_REC", GetType(Integer)),
                                             New DataColumn("INF", GetType(String)),
                                             New DataColumn("SUBSISTEMA", GetType(String)),
                                             New DataColumn("NIT_EMPRESA", GetType(String)),
                                             New DataColumn("RAZON_SOCIAL", GetType(String)),
                                             New DataColumn("ANNO", GetType(Integer)),
                                             New DataColumn("MES", GetType(Integer)),
                                             New DataColumn("CEDULA", GetType(String)),
                                             New DataColumn("NOMBRE", GetType(String)),
                                             New DataColumn("IBC", GetType(Double)),
                                             New DataColumn("AJUSTE", GetType(Double)),
                                             New DataColumn("FECHA_PAGO", GetType(Date))})

            sql = "SELECT EXPEDIENTE,LIQ_REC ,INF,SUBSISTEMA,NIT_EMPRESA,RAZON_SOCIAL,ANNO,MES,CEDULA,NOMBRE,IBC,AJUSTE,FECHA_PAGO FROM SQL_PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE ORDER BY ANNO,MES"

        ElseIf rbVerificacionpago.Checked Then

            dt.Columns.AddRange(New DataColumn() {New DataColumn("EXPEDIENTE", GetType(String)),
                                             New DataColumn("NRO_CONSIGNACION_O_NRO_PLANILLA", GetType(String)),
                                             New DataColumn("NIT_EMPRESA", GetType(String)),
                                             New DataColumn("ANNO", GetType(String)),
                                             New DataColumn("MES", GetType(String)),
                                             New DataColumn("CEDULA", GetType(String)),
                                             New DataColumn("SUBSISTEMA", GetType(String)),
                                             New DataColumn("AJUSTE", GetType(String)),
                                             New DataColumn("FECHA_DE_PAGO", GetType(String)),
                                             New DataColumn("FECHA_DE_REPORTE_DEL_PAGO_POR_EL_DEUDOR", GetType(String)),
                                             New DataColumn("FECHA_DE_VERIFICACION_DEL_PAGO", GetType(String)),
                                             New DataColumn("TITULO_DE_DEPOSITO_JUDICIAL", GetType(String)),
                                             New DataColumn("CAPITAL_PAGADO", GetType(String)),
                                             New DataColumn("AJUSTE_DEC_1406", GetType(String)),
                                             New DataColumn("INTERESES_PAGADOS", GetType(String)),
                                             New DataColumn("GASTOS_DEL_PROCESO", GetType(String)),
                                             New DataColumn("PAGOS_EN_EXCESO", GetType(String))})

            sql = "SELECT EXPEDIENTE, '' AS NRO_CONSIGNACION_O_NRO_PLANILLA, NIT_EMPRESA, ANNO,MES,CEDULA, SUBSISTEMA, AJUSTE,'' AS FECHA_DE_PAGO,'' AS FECHA_DE_REPORTE_DEL_PAGO_POR_EL_DEUDOR, '' AS FECHA_DE_VERIFICACION_DEL_PAGO, '' AS TITULO_DE_DEPOSITO_JUDICIAL, '' AS CAPITAL_PAGADO , '' AS AJUSTE_DEC_1406,'' AS INTERESES_PAGADOS ,'' AS GASTOS_DEL_PROCESO,'' AS PAGOS_EN_EXCESO FROM SQL_PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE ORDER BY ANNO,MES"
        End If

        Dim _Adap As New SqlDataAdapter
        _Adap = New SqlDataAdapter(sql, Funciones.CadenaConexion)
        If buscar Or rbsqlinteres.Checked Or rbVerificacionpago.Checked Then
            _Adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
        End If


        _Adap.Fill(dt)

        Return dt

    End Function

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click

        DatosImportado = TableExport(txtExpediente.Text, True)

        If DatosImportado.Rows.Count > 0 Then
            Gridinteres.DataSource = DatosImportado
        Else
            lblError.Text = "No se encontro registro asociado al expediente <strong>" & txtExpediente.Text & " </strong>"
            Gridinteres.DataSource = Nothing
        End If

        Gridinteres.DataBind()
    End Sub

    Private Function GetNomPerfil(ByVal pUsuario As String) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & Session("mnivelacces")

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        Return NomPerfil
    End Function

    Protected Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click

        'Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
        Dim Perfil As String = GetNomPerfil(Session("sscodigousuario"))

        If Perfil = "REPARTIDOR" Then
            Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")

        ElseIf Perfil = "VERIFICADOR DE PAGOS" Then
            Response.Redirect("PAGOS.aspx")

        ElseIf Perfil = "GESTOR - ABOGADO" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "REVISOR" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "SUPERVISOR" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        Else
            ' SUPER ADMINISTRADOR
            Response.Redirect("EJEFISGLOBAL.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        finsession()
        If Not IsPostBack Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
        End If
    End Sub

    Private Function ConstruirDatatable(ByVal RutaCompletaArchivo As String, ByVal Separador As Char) As DataTable

        'declaramos la Tabla donde añadiremos los datos y la fila correspondiente
        Dim MiTabla As DataTable = New DataTable("MyTable")
        Dim MiFila As DataRow
        'declaramos el resto de variables que nos harán falta
        Dim pos As Boolean = False
        Dim i As Integer
        Dim fieldValues As String()
        Dim miReader As IO.StreamReader

        Try
            Dim TargetPath As String = HttpRuntime.AppDomainAppPath & "temp_arch\" & Path.GetFileName(RutaCompletaArchivo)
            upload.PostedFile.SaveAs(TargetPath)

            'Abrimos el fichero y leemos la primera linea con el fin de determinar cuantos campos tenemos
            miReader = File.OpenText(TargetPath)
            fieldValues = miReader.ReadLine().Split(Separador)
            'Creamos las columnas de la cabecera
            For i = 0 To fieldValues.Length() - 1
                MiTabla.Columns.Add(New DataColumn(fieldValues(i).ToString()))
            Next

            'Continuamos leyendo el resto de filas y añadiendolas a la tabla
            While miReader.Peek() <> -1
                fieldValues = miReader.ReadLine().Split(Separador)
                MiFila = MiTabla.NewRow
                For i = 0 To fieldValues.Length() - 1
                    MiFila.Item(i) = fieldValues(i).ToString
                Next


                MiTabla.Rows.Add(MiFila)
            End While
            'Cerramos el reader
            miReader.Close()
        Catch ex As Exception
            'Gestionamos las excepciones, Aqui cada uno puede hacer lo que crea conveniente: Mostrar un error en Javascript en este caso y devolver el Datatable vacío
            Dim mensaje As String
            mensaje = "alert ('Ha ocurrido el siguiente error al importar el archivo: " + ex.ToString + "');"
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Me.GetType(), "ErrorConstruirDatabale", mensaje, True)
            Return New DataTable("Empty")
        Finally
            'Si queremos ejecutar algo exista excepción o no
        End Try
        'Devolvemos el DataTable si todo ha ido bien
        Return MiTabla
    End Function

    Private Sub Gridinteres_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridinteres.PageIndexChanging
        Gridinteres.PageIndex = e.NewPageIndex
        _Gridinteres.DataSource = DatosImportado
        _Gridinteres.DataBind()
    End Sub

    Protected Sub cmdVerSql_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdVerSql.Click
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT DISTINCT EXPEDIENTE, RAZON_SOCIAL  FROM SQL_PLANILLA")
        Gridinteres.DataSource = tb
        Gridinteres.DataBind()
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        'Limpiar los cuadros de texto de busqueda
        Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
        Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
        Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
        Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
        Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""
        Session("Paginacion") = 10

        Response.Redirect("../../login.aspx")
    End Sub

    Private Sub finsession()
        Dim MTG As New MetodosGlobalesCobro
        If MTG.IsSessionTimedOut Then
            FormsAuthentication.SignOut()
            'Limpiar los cuadros de texto de busqueda
            Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
            Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
            Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
            Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
            Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""
            Session("Paginacion") = 10
            Response.Redirect("../../login.aspx")
        End If
    End Sub

    Private Function Validaciones(ByVal dt As DataTable, ByVal expediente As String, ByVal tAjuste As Double) As String

        Dim Mensaje As String = ""
        Dim SearhExp As DataTable = Funciones.RetornaCargadatos("SELECT * FROM EJEFISGLOBAL WHERE EFINROEXP = '" & expediente & "' ")


        'Verificar si el expediente existe.
        If SearhExp.Rows.Count = 0 Then
            Mensaje = "No se pudo subir el SQL. No se detectó el expediente. ."
        Else
            'Verificar si el archivo a subir tiene expedientes distintos
            Dim distinctExp = From row In dt.AsEnumerable()
                              Select row.Field(Of String)("EXPEDIENTE") Distinct
            If distinctExp.Count > 1 Then
                Mensaje = "No se pudo subir el SQL. Se detectó expedientes distintos en el archivo a subir por favor verifique."
            Else

                'Buscar el expediente en la tabla de titulo para comparar el valor ingresado por el repartidor en la columna totalrepartidor
                Dim SearchMontoTitulo As DataTable = Funciones.RetornaCargadatos("SELECT * FROM MAESTRO_TITULOS WHERE MT_expediente = '" & expediente & "'")
                If SearchMontoTitulo.Rows.Count > 0 Then
                    'Total del titulo ingresado por el repartidor
                    Dim TotalTitulo As Double = SearchMontoTitulo.Rows(0).Item("totalrepartidor")
                    If tAjuste > TotalTitulo Or tAjuste < TotalTitulo Then
                        Mensaje = "No se pudo subir el SQL. El valor total del ajuste no coincide con el valor ingresado en el proceso de reparto."
                    Else
                        'Verificar si el sql tiene pago detallado
                        Dim SearhExpSQL As DataTable = Funciones.RetornaCargadatos("SELECT SUM(MONTO_PAGO) AS PAGO FROM SQL_PLANILLA WHERE EXPEDIENTE = '" & expediente & "' ")
                        If CInt(SearhExpSQL.Rows(0).Item("PAGO")) > 0 Then
                            Mensaje = "No se pudo reemplazar el SQL. Se detectó que tiene pagos detallado."
                        End If
                    End If
                End If

            End If

        End If

        Return Mensaje
    End Function

    Private Sub ModificarSQLPLanilla(ByVal EXPEDIENTE As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

        Dim command As New SqlCommand("DELETE FROM SQL_PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE", cn, _trans)
        command.Parameters.AddWithValue("@EXPEDIENTE", EXPEDIENTE)
        command.ExecuteNonQuery()

    End Sub


End Class