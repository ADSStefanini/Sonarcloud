Imports System.Data.SqlClient
Imports System.IO
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class LiquidacionInteresesDTF
    Inherits System.Web.UI.Page

    Protected Shared Expxls As New DataTable
    Protected Shared DatosImportado As New DataTable

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilitan los campos para este perfil
                upload.Enabled = False
                cmdImportarcsv.Enabled = False
                txtfechaPago.Enabled = False
                cmdCalcularInteres.Enabled = False
                cmdExportarExcel.Enabled = False
            Else
                'Se habilitan los campos para usuarios con perfil diferente a Verificador de Pagos
                upload.Enabled = True
                cmdImportarcsv.Enabled = True
                txtfechaPago.Enabled = True
                cmdCalcularInteres.Enabled = True
                cmdExportarExcel.Enabled = True
            End If

            ultimoDTF()
            txtfechaPago.Text = Now().ToString("dd/MM/yyyy")
        End If
    End Sub

    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub

    'Calcular Intereses Parafiscales
    Private Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click
        'Dim cn As New SqlConnection(Funciones.CadenaConexion)
        'cn.Open()
        'Dim _trans As SqlTransaction
        '_trans = cn.BeginTransaction
        Try

            If txtfechaPago.Text = "" Then
                lblError.Text = "Por favor digite la fecha de pago"
                Alert(lblError.Text)
                Exit Sub
            End If

            If Not IsDate(txtfechaPago.Text) Then
                lblError.Text = "Por favor digite la fecha de pago válida"
                Alert(lblError.Text)
                Exit Sub
            End If

            If DatosImportado.Rows.Count > 0 Then
                Dim fechaPago As Date = CDate(txtfechaPago.Text).ToString("dd/MM/yyyy")
                Dim salida As New DataTable
                salida = DatosImportado.Copy()
                salida.Columns.Add("p")

                For Each X As DataRow In salida.Rows
                    If X.Item("FECHA_MESADA") <> "" Then
                        X.Item("p") = CInt(Year(X.Item("FECHA_MESADA")) & IIf(Len(Month(X.Item("FECHA_MESADA")).ToString) = 1, "0" & Month(X.Item("FECHA_MESADA")), Month(X.Item("FECHA_MESADA"))))
                    End If

                Next

                Dim dataView As New DataView(salida)
                dataView.Sort = "P DESC"
                salida = New DataTable
                salida = dataView.ToTable()
                Dim ultimaFecha As String = salida.Rows(0).Item("P").ToString
                'Dim _row() As DataRow = DatosImportado.Select(Year("FECHA_MESADA") & Month("FECHA_MESADA") = ultimaFecha, String.Empty)
                Dim _row() As DataRow = salida.Select("P = " & ultimaFecha, String.Empty)
                Dim diferencia As Integer = DateDiff(DateInterval.Month, CDate(_row(0).Item("FECHA_MESADA")), fechaPago)
                Dim dataRow As DataRow

                If CInt(ultimaFecha) < CInt(Year(fechaPago) & IIf(Len(Month(fechaPago).ToString) = 1, "0" & Month(fechaPago), Month(fechaPago))) Then

                    For i = 1 To diferencia - 1
                        dataRow = DatosImportado.NewRow
                        dataRow.Item("FECHA_MESADA") = DateAdd(DateInterval.Month, i, CDate(_row(0).Item("FECHA_MESADA"))).ToString("dd/MM/yyyy")
                        dataRow.Item("CAPITAL_ADEUDADO") = 0
                        dataRow.Item("DIAS_MORA") = ""
                        dataRow.Item("VALOR_ADEUDADO_ACUMULADO") = ""
                        dataRow.Item("INTERESES") = ""
                        dataRow.Item("SALDO_DEUDA") = ""
                        'dataRow.Item("FECHA_SYS") = Nothing
                        DatosImportado.Rows.Add(dataRow)
                    Next

                    dataRow = DatosImportado.NewRow
                    dataRow.Item("FECHA_MESADA") = fechaPago.ToString("dd/MM/yyyy")
                    dataRow.Item("CAPITAL_ADEUDADO") = 0
                    dataRow.Item("DIAS_MORA") = ""
                    dataRow.Item("VALOR_ADEUDADO_ACUMULADO") = ""
                    dataRow.Item("INTERESES") = ""
                    dataRow.Item("SALDO_DEUDA") = ""
                    'dataRow.Item("FECHA_SYS") = Nothing
                    DatosImportado.Rows.Add(dataRow)

                Else
                    lblError.Text = "La fecha de pago no puede ser inferior al ultimo periodo DTF."
                    Exit Sub
                End If

                'Validar seleccion de un tipo de aportante
                'Dim consecutivoInteres As Integer = LoadConsecutivoInteres(cn, _trans) 'Consultar consecutivo de intereses 

                Dim valorAcumulado As Double = 0

                Dim saldoDeuda As Double = 0
                Dim ultDTF As String = ""
                Dim perioPago As Integer = Year(CDate(txtfechaPago.Text)) & IIf(Len(Month(CDate(txtfechaPago.Text)).ToString) = 1, "0" & Month(CDate(txtfechaPago.Text)), Month(CDate(txtfechaPago.Text)))
                Dim diaPago As Integer = Day(CDate(txtfechaPago.Text))

                'ModificarPlanilla(DatosImportado.Rows(0).Item("EXPEDIENTE"), _trans, cn)
                For row As Integer = 0 To DatosImportado.Rows.Count - 1 ' recorro la grilla para determinar los intereses por cada ajuste

                    'If DatosImportado.Rows(row).Item("FECHA_MESADA") = "" Then

                    'End If
                    Dim fecha_mesada As String = DatosImportado.Rows(row).Item("FECHA_MESADA").ToString
                    If fecha_mesada <> "" Then

                        If CInt(Year(fecha_mesada) & IIf(Len(Month(CDate(fecha_mesada)).ToString) = 1, "0" & Month(CDate(fecha_mesada)), Month(CDate(fecha_mesada)))) = perioPago Then
                            DatosImportado.Rows(row).Item("DIAS_MORA") = diaPago
                        Else
                            DatosImportado.Rows(row).Item("DIAS_MORA") = 30
                        End If

                        fecha_mesada = DateAdd(DateInterval.Month, 1, CDate(fecha_mesada))
                        Dim periodo As String = Year(fecha_mesada) & IIf(Len(Month(CDate(fecha_mesada)).ToString) = 1, "0" & Month(CDate(fecha_mesada)), Month(CDate(fecha_mesada)))

                        Dim validarDTF() As DataRow = CType(ViewState("UltimoDTF"), DataTable).Select("PERIODO = " & periodo, String.Empty)
                        Dim dtf As DataRow
                        If validarDTF.Length > 0 Then
                            dtf = (From p In CType(ViewState("UltimoDTF"), DataTable) Where p.Item("PERIODO") = periodo Select p).Single
                        Else
                            dtf = Nothing
                        End If

                        '1ro Obtener saldo acumulado

                        Dim deudaCapital As String = DatosImportado.Rows(row).Item("CAPITAL_ADEUDADO")
                        'DatosImportado.Rows(row).Item("CAPITAL_ADEUDADO") = String.Format("{0:C0}", CDbl(DatosImportado.Rows(row).Item("CAPITAL_ADEUDADO")))
                        DatosImportado.Rows(row).Item("CAPITAL_ADEUDADO") = Format(CDbl(DatosImportado.Rows(row).Item("CAPITAL_ADEUDADO")), "##,##00.00")

                        If deudaCapital = "" Then
                            deudaCapital = 0
                        Else
                            deudaCapital = CDbl(deudaCapital)
                        End If

                        valorAcumulado += deudaCapital

                        DatosImportado.Rows(row).Item("VALOR_ADEUDADO_ACUMULADO") = valorAcumulado
                        DatosImportado.Rows(row).Item("VALOR_ADEUDADO_ACUMULADO") = RedondearUnidades(0, CDbl(DatosImportado.Rows(row).Item("VALOR_ADEUDADO_ACUMULADO")))
                        DatosImportado.Rows(row).Item("VALOR_ADEUDADO_ACUMULADO") = Format(CDbl(DatosImportado.Rows(row).Item("VALOR_ADEUDADO_ACUMULADO")), "##,##00")

                        'DTF

                        Dim intereses As Double = 0

                        If validarDTF.Length > 0 Then
                            ultDTF = dtf.Item("DTF")
                        Else
                            ultDTF = ultDTF
                        End If

                        DatosImportado.Rows(row).Item("DTF") = ultDTF

                        If CInt(DatosImportado.Rows(row).Item("DIAS_MORA")) = 30 Then
                            intereses = valorAcumulado * ((1 + CDbl(ultDTF)) ^ (1 / 12) - 1)
                        Else
                            intereses = (valorAcumulado * ((1 + CDbl(ultDTF)) ^ (1 / 360) - 1)) * CInt(DatosImportado.Rows(row).Item("DIAS_MORA"))
                        End If

                        'Intereses redondeado
                        DatosImportado.Rows(row).Item("INTERESES") = intereses
                        DatosImportado.Rows(row).Item("INTERESES") = RedondearUnidades(0, CDbl(DatosImportado.Rows(row).Item("INTERESES")))
                        DatosImportado.Rows(row).Item("INTERESES") = Format(CDbl(DatosImportado.Rows(row).Item("INTERESES")), "##,##00")

                        saldoDeuda = saldoDeuda + intereses + deudaCapital

                        DatosImportado.Rows(row).Item("SALDO_DEUDA") = saldoDeuda
                        DatosImportado.Rows(row).Item("SALDO_DEUDA") = RedondearUnidades(0, CDbl(DatosImportado.Rows(row).Item("SALDO_DEUDA")))
                        DatosImportado.Rows(row).Item("SALDO_DEUDA") = Format(CDbl(DatosImportado.Rows(row).Item("SALDO_DEUDA")), "##,##00")

                        ' DatosImportado.Rows(row).Item("ID_CALCULO") = consecutivoInteres
                        'DatosImportado.Rows(row).Item("FECHA_SYS") = Now().ToString
                    Else

                    End If
                Next
                'Expxls = Save_LogCalculo(consecutivoInteres, _trans, cn)
                '_trans.Commit()

                _Gridinteres.DataSource = DatosImportado
                _Gridinteres.DataBind()

                lblError.Text = "Proceso terminado satisfactoriamente.."
            Else
                lblError.Text = "Por favor, Seleccione el archivo .CSV a procesar y luego oprima el boton Importar... "
                lblError.Visible = True
            End If
        Catch ex As Exception
            '_trans.Rollback()
            'cn.Close()
            Alert("Error: " & ex.ToString)
            lblError.Text = "Por favor compruebe el archivo importado, es posible que el orden de las columnas no coinsidan con la plantilla de ejemplo o que esten campos vacios."
            lblError.Visible = True
        Finally
            'cn.Close()
        End Try
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
        Dim column As DataColumn

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

            'Agregar Columna de DIAS DE MORA
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "DIAS_MORA"
            MiTabla.Columns.Add(column)

            'DTF
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "DTF"
            MiTabla.Columns.Add(column)

            'Agregar Columna de total pago ajustado
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "VALOR_ADEUDADO_ACUMULADO"
            MiTabla.Columns.Add(column)

            'Agregar Columna de interes ajustado
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "INTERESES"
            MiTabla.Columns.Add(column)

            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "SALDO_DEUDA"
            MiTabla.Columns.Add(column)

            ''Agregar Columna de id calculo
            'column = New DataColumn()
            'column.DataType = System.Type.GetType("System.Int32")
            'column.ColumnName = "ID_CALCULO"
            'MiTabla.Columns.Add(column)

            'Agregar Columna de fecha del sys
            'column = New DataColumn()
            'column.DataType = System.Type.GetType("System.String")
            'column.ColumnName = "FECHA_SYS"
            'MiTabla.Columns.Add(column)

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

    Private Sub cmdImportarcsv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImportarcsv.Click
        Dim filePath As String = String.Empty
        If upload.HasFile AndAlso upload.PostedFile.ContentType.Equals("application/vnd.ms-excel") Then
            'Gridinteres.DataSource = DirectCast(ReadToEnd(upload.PostedFile.FileName), DataTable)
            DatosImportado = CType(ConstruirDatatable(upload.PostedFile.FileName, ";"), DataTable)

            Gridinteres.DataSource = DatosImportado
            Gridinteres.DataBind()
            lblError.Text = "Se detectaron " & DatosImportado.Rows.Count & " registro(s) en el archivo."
        Else
            lblError.Text = "Por favor, compruebe el tipo de archivo seleccionado"
            lblError.Visible = True
        End If
    End Sub

    Private Sub Save_Planilla(ByVal SUBSISTEMA As String, ByVal NIT_EMPRESA As String, ByVal RAZON_SOCIAL As String, ByVal INF As String, ByVal ANNO As Integer, ByVal MES As Integer, ByVal CEDULA As String, ByVal NOMBRE As String, ByVal IBC As Double, ByVal AJUSTE As Double, ByVal EXPEDIENTE As String, ByVal INTERESES As Double, ByVal FECHA_PAGO As Date, ByVal FECHA_EXIGIBILIDAD As Date, ByVal ID_CALCULO As Integer, ByVal DIA_HABIL_PAGO As Integer, ByVal LIQ_REC As Integer, ByVal TOTAL_PAGAR As Double, ByVal INTERESES_AJUSTADO As Double, ByVal TOTAL_PAGAR_AJUSTADO As Double, ByVal DIAS_MORA As Integer, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

        Dim command As New SqlCommand("Save_Planilla", cn, _trans)
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
        command.Parameters.AddWithValue("@INTERESES_NORMAL", INTERESES)
        command.Parameters.AddWithValue("@FECHA_PAGO", FECHA_PAGO)
        command.Parameters.AddWithValue("@FECHA_EXIGIBILIDAD", FECHA_EXIGIBILIDAD)
        command.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        command.Parameters.AddWithValue("@DIA_HABIL_PAGO", DIA_HABIL_PAGO)
        command.Parameters.AddWithValue("@LIQ_REC", LIQ_REC)
        command.Parameters.AddWithValue("@TOTAL_PAGAR_NORMAL", TOTAL_PAGAR)
        command.Parameters.AddWithValue("@INTERESES_AJUSTADO", INTERESES_AJUSTADO)
        command.Parameters.AddWithValue("@TOTAL_PAGAR_AJUSTADO", TOTAL_PAGAR_AJUSTADO)
        command.Parameters.AddWithValue("@DIAS_MORA", DIAS_MORA)
        command.ExecuteNonQuery()

    End Sub

    Private Function Save_LogCalculo(ByVal ID_CALCULO As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection) As DataTable
        Dim tb As New DataTable
        Dim command As New SqlCommand("save_LogCalculoInteresParafiscales", cn, _trans)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        command.ExecuteNonQuery()
        Dim ad As New SqlDataAdapter(command)
        ad.Fill(tb)
        Return tb
    End Function

    Private Function LoadConsecutivoInteres(ByVal cn As SqlConnection, ByVal _trans As SqlTransaction) As Integer
        Dim Table As New DataTable
        'Consultar consecutivo
        Dim _Adapter As New SqlDataAdapter("SELECT * FROM MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 16", cn)
        _Adapter.SelectCommand.Transaction = _trans
        _Adapter.Fill(Table)
        Dim Con As Integer
        Con = Table.Rows(0).Item("CON_USER") + 1

        'Actualizar Consultar consecutivo
        _Adapter = New SqlDataAdapter("UPDATE MAESTRO_CONSECUTIVOS SET CON_USER = CON_USER + 1 WHERE CON_IDENTIFICADOR = 16", cn)
        _Adapter.SelectCommand.Transaction = _trans
        _Adapter.SelectCommand.ExecuteNonQuery()

        Return Con
    End Function

    Private Function RoundI(ByVal x As Double, Optional ByVal d As Integer = 0) As Double
        Dim m As Double
        m = 10 ^ d
        If x < 0 Then
            RoundI = Fix(x * m - 0.5) / m
        Else
            RoundI = Fix(x * m + 0.5) / m
        End If
    End Function

    Private Function RedondearUnidades(ByVal pUnidad As Integer, ByVal pValor As Double) As Double
        Dim Unidad As String = pUnidad
        Dim resultado As Double = CDbl(pValor)
        Select Case Unidad
            Case 4 'DiezMilesima
                resultado = RoundI(CDbl(pValor), 4)
            Case 3 'Milesima
                resultado = RoundI(CDbl(pValor), 3)
            Case 2 'Centesima
                resultado = RoundI(CDbl(pValor), 2)
            Case 1 'Decima
                resultado = RoundI(CDbl(pValor), 1)
            Case 0 'Unidad
                resultado = RoundI(CDbl(pValor), 0)
            Case -1 'Decena
                resultado = RoundI(CDbl(pValor), -1)
            Case -2 'Centena
                resultado = RoundI(CDbl(pValor), -2)
            Case -3 'Mil
                resultado = RoundI(CDbl(pValor), -3)
            Case -4 'Diez Mil
                resultado = RoundI(CDbl(pValor), -4)
        End Select
        Return resultado
    End Function

    Private Sub LLenarDatos()
        Dim DataTable As DataTable = CType(ViewState("datos"), DataTable)
        Gridinteres.DataSource = DataTable
        Gridinteres.DataBind()
    End Sub

    Private Sub Gridinteres_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridinteres.PageIndexChanging
        Gridinteres.PageIndex = e.NewPageIndex
        _Gridinteres.DataSource = DatosImportado
        _Gridinteres.DataBind()
    End Sub

    Private Sub cmdExportarExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdExportarExcel.Click

        If Gridinteres.Rows.Count > 0 Then

            descargafichero(Expxls, "informacion_calculo_intereses_expediente" & Request("pExpediente") & ".xls", True)
        Else
            lblError.Text = "Error: no se detecto registros a exportar..."


            'Dim sb As StringBuilder = New StringBuilder()
            'Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
            'Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
            'Dim Page As Page = New Page()
            'Dim form As HtmlForm = New HtmlForm()
            'Me.Gridinteres.EnableViewState = False
            'Page.EnableEventValidation = False
            'Page.DesignerInitialize()
            'Page.Controls.Add(form)
            'form.Controls.Add(Me.Gridinteres)
            'Page.RenderControl(htw)
            'Response.Clear()
            'Response.Buffer = True
            'Response.ContentType = "application/vnd.ms-excel"
            'Response.AddHeader("Content-Disposition", "attachment;filename=data.xls")
            'Response.Charset = "UTF-8"
            'Response.ContentEncoding = Encoding.Default
            'Response.Write(sb.ToString())
            'Response.End()

        End If
    End Sub

    Private Sub descargafichero(ByVal dtTemp As DataTable, ByVal fname As String, ByVal forceDownload As Boolean, Optional ByVal plantilla As String = "")

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

        If plantilla <> "" Then
            Response.Write(plantilla)
        Else
            Dim sb As StringBuilder = New StringBuilder()
            Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
            Dim dg As New DataGrid()

            dg.DataSource = DatosImportado
            dg.DataBind()

            dg.RenderControl(htw)
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(SW)
        End If

        Response.End()

    End Sub

    Private Sub ultimoDTF()
        Dim table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT * FROM dbo.CONFIGURACION_DTF ORDER BY  ID ", Funciones.CadenaConexion)
        _Adap.Fill(table)
        ViewState("UltimoDTF") = table
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

    Private Sub ModificarPlanilla(ByVal pExpediente As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)
        Dim command As New SqlCommand("DELETE FROM PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE ", cn, _trans)
        command.Parameters.AddWithValue("@EXPEDIENTE", pExpediente)
        command.ExecuteNonQuery()
    End Sub

End Class