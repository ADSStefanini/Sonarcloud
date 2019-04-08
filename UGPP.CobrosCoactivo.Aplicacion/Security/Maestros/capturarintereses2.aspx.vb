Imports System.Data.SqlClient
Imports System.IO

Partial Public Class capturarintereses2
    Inherits System.Web.UI.Page

    Protected Shared Expxls As New DataTable
    Protected Shared DatosImportado As New DataTable


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ultima_tasas()
        End If
    End Sub
    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub


    'Calcular Intereses Parafiscales

    Private Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        cn.Open()
        Dim _trans As SqlTransaction
        _trans = cn.BeginTransaction
        Try

            If DatosImportado.Rows.Count > 0 Then
                'Validar seleccion de un tipo de aportante
                If cboCAPORTE.SelectedValue <> "0" Then

                    Dim consecutivoInteres As Integer = LoadConsecutivoInteres(cn, _trans) 'Consultar consecutivo de intereses 
                    Dim nitcc As String = DatosImportado.Rows(0).Item("NIT_EMPRESA").ToString ' nit o cedula de deudor
                    Dim diaPago As String = lbldiaPago.Text
                    ModificarPlanilla(DatosImportado.Rows(0).Item("EXPEDIENTE"), _trans, cn)
                    For row As Integer = 0 To DatosImportado.Rows.Count - 1 ' recorro la grilla para determinar los intereses por cada ajuste
                        Dim deudaCapital As Double = CDbl(DatosImportado.Rows(row).Item("AJUSTE"))

                        DatosImportado.Rows(row).Item("AJUSTE") = String.Format("{0:C2}", CDbl(DatosImportado.Rows(row).Item("AJUSTE")))

                        Dim fechaPago As Date = CDate(DatosImportado.Rows(row).Item("FECHA_PAGO")).ToString("dd/MM/yyyy")
                        Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, DatosImportado.Rows(row).Item("ANNO"), DatosImportado.Rows(row).Item("MES"), cboCAPORTE.SelectedValue, DatosImportado.Rows(row).Item("SUBSISTEMA"))

                        'Alterar grid colocar datos faltantes
                        DatosImportado.Rows(row).Item("DIA_HABIL_PAGO") = diaPago
                        DatosImportado.Rows(row).Item("FECHA_EXIGIBILIDAD") = CDate(fechaExigibilidad).ToString("dd/MM/yyyy")

                        'Intereses sin redondear
                        DatosImportado.Rows(row).Item("INTERESES_NORMAL") = FuncionesInteresesParafiscales._CalcularIntereses(deudaCapital, fechaExigibilidad, fechaPago, CDec(ViewState("trimestral")))
                        DatosImportado.Rows(row).Item("INTERESES_NORMAL") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("INTERESES_NORMAL")))


                        DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL") = deudaCapital + CDbl(DatosImportado.Rows(row).Item("INTERESES_NORMAL"))
                        DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL")))


                        'Intereses redondeado
                        DatosImportado.Rows(row).Item("INTERESES_AJUSTADO") = RedondearUnidades(-2, CDbl(DatosImportado.Rows(row).Item("INTERESES_NORMAL")))
                        DatosImportado.Rows(row).Item("INTERESES_AJUSTADO") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("INTERESES_AJUSTADO")))

                        DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO") = deudaCapital + CDbl(DatosImportado.Rows(row).Item("INTERESES_AJUSTADO"))
                        DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO") = String.Format("{0:C8}", CDbl(DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO")))


                        'Dias de mora
                        DatosImportado.Rows(row).Item("DIAS_MORA") = DateDiff(DateInterval.Day, fechaExigibilidad, fechaPago)


                        DatosImportado.Rows(row).Item("ID_CALCULO") = consecutivoInteres
                        DatosImportado.Rows(row).Item("FECHA_SYS") = Now()

                        Save_Planilla(DatosImportado.Rows(row).Item("SUBSISTEMA"), nitcc, DatosImportado.Rows(row).Item("RAZON_SOCIAL"), DatosImportado.Rows(row).Item("INF"), DatosImportado.Rows(row).Item("ANNO"), DatosImportado.Rows(row).Item("MES"), DatosImportado.Rows(row).Item("CEDULA"), DatosImportado.Rows(row).Item("NOMBRE"), DatosImportado.Rows(row).Item("IBC"), DatosImportado.Rows(row).Item("AJUSTE"), DatosImportado.Rows(row).Item("EXPEDIENTE"), DatosImportado.Rows(row).Item("INTERESES_NORMAL"), fechaPago, fechaExigibilidad, consecutivoInteres, diaPago, DatosImportado.Rows(row).Item("LIQ_REC"), DatosImportado.Rows(row).Item("TOTAL_PAGAR_NORMAL"), DatosImportado.Rows(row).Item("INTERESES_AJUSTADO"), DatosImportado.Rows(row).Item("TOTAL_PAGAR_AJUSTADO"), DatosImportado.Rows(row).Item("DIAS_MORA"), _trans, cn)
                    Next
                    Expxls = Save_LogCalculo(consecutivoInteres, _trans, cn)
                    CargarTotales(consecutivoInteres, cn, _trans)
                    _trans.Commit()

                    _Gridinteres.DataSource = DatosImportado
                    _Gridinteres.DataBind()

                    lblError.Text = "Proceso terminado satisfactoriamente.."
                Else
                    lblError.Text = "Por favor, Seleccione el tipo de aportante"
                    lblError.Visible = True
                End If
            Else
                lblError.Text = "Por favor, Seleccione el archivo .CSV a procesar y luego oprima el boton Importar... "
                lblError.Visible = True
            End If
        Catch ex As Exception
            _trans.Rollback()
            cn.Close()
            Alert("Error: " & ex.ToString)
            lblError.Text = "Por favor compruebe el archivo importado, es posible que el orden de las columnas no coinsidan con la plantilla de ejemplo o que esten campos vacios."
            lblError.Visible = True
        Finally
            cn.Close()
        End Try
    End Sub

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

    Private Function ReadToEnd(ByVal filePath As String) As Object
        Dim dtDataSource As New DataTable()
        Dim TargetPath As String = HttpRuntime.AppDomainAppPath & "temp_arch\" & Path.GetFileName(filePath)
        upload.PostedFile.SaveAs(TargetPath)
        Dim column As DataColumn

        Dim fileContent As String() = File.ReadAllLines(TargetPath)
        Try


            If fileContent.Count() > 0 Then
                'Agregar Columnas de archivo
                Dim columns As String() = fileContent(0).Split(";"c)
                For i As Integer = 0 To columns.Count() - 1
                    dtDataSource.Columns.Add(columns(i))
                Next

                'Agregar Columna de DIA_HABIL_PAGO
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.Int32")
                column.ColumnName = "DIA_HABIL_PAGO"
                dtDataSource.Columns.Add(column)

                'Agregar Columna de fecha de exigibilidad
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.DateTime")
                column.ColumnName = "FECHA_EXIGIBILIDAD"
                dtDataSource.Columns.Add(column)

                'Agregar Columna de interes sin ajustar
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.String")
                column.ColumnName = "INTERESES_NORMAL"
                dtDataSource.Columns.Add(column)

                'Agregar Columna de total pago sin ajustar
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.String")
                column.ColumnName = "TOTAL_PAGAR_NORMAL"
                dtDataSource.Columns.Add(column)

                'Agregar Columna de interes ajustado
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.String")
                column.ColumnName = "INTERESES_AJUSTADO"
                dtDataSource.Columns.Add(column)

                'Agregar Columna de total pago ajustado
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.String")
                column.ColumnName = "TOTAL_PAGAR_AJUSTADO"
                dtDataSource.Columns.Add(column)

                'Agregar Columna de DIAS DE MORA
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.Int32")
                column.ColumnName = "DIAS_MORA"
                dtDataSource.Columns.Add(column)


                'Agregar Columna de id calculo
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.Int32")
                column.ColumnName = "ID_CALCULO"
                dtDataSource.Columns.Add(column)

                'Agregar Columna de fecha del sys
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.DateTime")
                column.ColumnName = "FECHA_SYS"
                dtDataSource.Columns.Add(column)


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

    Private Function CargarTotales(ByVal ID_CALCULO As Integer, ByVal cn As SqlConnection, ByVal _trans As SqlTransaction) As DataTable
        Dim Table As New DataTable
        'Consultar consecutivo
        Dim _Adapter As New SqlDataAdapter("SELECT SUBSISTEMA, SUM(AJUSTE) AS CAPITAL,SUM(INTERESES_AJUSTADO) AS INTERESES,SUM(AJUSTE +INTERESES_AJUSTADO )AS GRANTOTAL  FROM PLANILLA WHERE ID_CALCULO = @ID_CALCULO  GROUP BY SUBSISTEMA", cn)
        _Adapter.SelectCommand.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        _Adapter.SelectCommand.Transaction = _trans
        _Adapter.Fill(Table)

        'Iniciar campos en 0
        txtSaludInteres.Text = 0
        txtSaludCapital.Text = 0
        txtTotalSalud.Text = 0
        txtPensionCapital.Text = 0
        txtPensionInteres.Text = 0
        txtTotalPension.Text = 0
        txtCcfCapital.Text = 0
        txtCcfInteres.Text = 0
        txtTotalCCF.Text = 0
        txtArlCapital.Text = 0
        txtArlInteres.Text = 0
        txtTotalARL.Text = 0
        txtFspCapital.Text = 0
        txtFspInteres.Text = 0
        txtTotalFSP.Text = 0
        txtIcbfCapital.Text = 0
        txtIcbfInteres.Text = 0
        txtTotalICBF.Text = 0
        txtSenaCapital.Text = 0
        txtSenaInteres.Text = 0
        txtTotalSena.Text = 0
        'Fin

        'Calcular totales
        For Each row As DataRow In Table.Rows
            If row("SUBSISTEMA") = "SALUD" Then
                txtSaludCapital.Text = row("CAPITAL")
                txtSaludCapital.Text = FormatCurrency(txtSaludCapital.Text, 0, TriState.True) 'Formato moneda
                txtSaludInteres.Text = row("INTERESES")
                txtSaludInteres.Text = FormatCurrency(txtSaludInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalSalud.Text = row("GRANTOTAL")
                txtTotalSalud.Text = FormatCurrency(txtTotalSalud.Text, 0, TriState.True) 'Formato moneda
            End If
            If row("SUBSISTEMA") = "PENSION" Then
                txtPensionCapital.Text = row("CAPITAL")
                txtPensionCapital.Text = FormatCurrency(txtPensionCapital.Text, 0, TriState.True) 'Formato moneda
                txtPensionInteres.Text = row("INTERESES")
                txtPensionInteres.Text = FormatCurrency(txtPensionInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalPension.Text = row("GRANTOTAL")
                txtTotalPension.Text = FormatCurrency(txtTotalPension.Text, 0, TriState.True) 'Formato moneda
            End If
            If row("SUBSISTEMA") = "CCF" Then
                txtCcfCapital.Text = row("CAPITAL")
                txtCcfCapital.Text = FormatCurrency(txtCcfCapital.Text, 0, TriState.True) 'Formato moneda
                txtCcfInteres.Text = row("INTERESES")
                txtCcfInteres.Text = FormatCurrency(txtCcfInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalCCF.Text = row("GRANTOTAL")
                txtTotalCCF.Text = FormatCurrency(txtTotalCCF.Text, 0, TriState.True) 'Formato moneda
            End If

            If row("SUBSISTEMA") = "ARL" Then
                txtArlCapital.Text = row("CAPITAL")
                txtArlCapital.Text = FormatCurrency(txtArlCapital.Text, 0, TriState.True) 'Formato moneda
                txtArlInteres.Text = row("INTERESES")
                txtArlInteres.Text = FormatCurrency(txtArlInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalARL.Text = row("GRANTOTAL")
                txtTotalARL.Text = FormatCurrency(txtTotalARL.Text, 0, TriState.True) 'Formato moneda
            End If

            If row("SUBSISTEMA") = "FSP" Then
                txtFspCapital.Text = row("CAPITAL")
                txtFspCapital.Text = FormatCurrency(txtFspCapital.Text, 0, TriState.True) 'Formato moneda
                txtFspInteres.Text = row("INTERESES")
                txtFspInteres.Text = FormatCurrency(txtFspInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalFSP.Text = row("GRANTOTAL")
                txtTotalFSP.Text = FormatCurrency(txtTotalFSP.Text, 0, TriState.True) 'Formato moneda
            End If

            If row("SUBSISTEMA") = "ICBF" Then
                txtIcbfCapital.Text = row("CAPITAL")
                txtIcbfCapital.Text = FormatCurrency(txtIcbfCapital.Text, 0, TriState.True) 'Formato moneda
                txtIcbfInteres.Text = row("INTERESES")
                txtIcbfInteres.Text = FormatCurrency(txtIcbfInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalICBF.Text = row("GRANTOTAL")
                txtTotalICBF.Text = FormatCurrency(txtTotalICBF.Text, 0, TriState.True) 'Formato moneda
            End If
            If row("SUBSISTEMA") = "SENA" Then
                txtSenaCapital.Text = row("CAPITAL")
                txtSenaCapital.Text = FormatCurrency(txtSenaCapital.Text, 0, TriState.True) 'Formato moneda
                txtSenaInteres.Text = row("INTERESES")
                txtSenaInteres.Text = FormatCurrency(txtSenaInteres.Text, 0, TriState.True) 'Formato moneda
                txtTotalSena.Text = row("GRANTOTAL")
                txtTotalSena.Text = FormatCurrency(txtTotalSena.Text, 0, TriState.True) 'Formato moneda
            End If
        Next
        'Fin 
        Return Table

    End Function

    Private Sub cboCAPORTE_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboCAPORTE.SelectedIndexChanged
        If Gridinteres.Rows.Count > 0 Then
            Dim nitcc As String = DatosImportado.Rows(0).Item("NIT_EMPRESA")
            lbldiaPago.Text = FuncionesInteresesParafiscales.ObtenerDiaPago(nitcc, cboCAPORTE.SelectedValue) ' dia de pago del deudor
        Else
            cboCAPORTE.SelectedValue = "0"
            lblError.Text = "Por favor, Seleccione el archivo a importar y luego oprima el boton Importar... "
        End If

    End Sub

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

            dg.DataSource = dtTemp
            dg.DataBind()

            dg.RenderControl(htw)
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(SW)
        End If



        Response.End()

    End Sub

    Private Sub ultima_tasas()
        Dim table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT TOP 1 * FROM dbo.CALCULO_INTERESES_PARAFISCALES where p_trimestral > 0 ORDER BY  CONSEC DESC", Funciones.CadenaConexion)
        _Adap.Fill(table)

        ViewState("diaria") = table.Rows(0).Item(4)
        ViewState("trimestral") = table.Rows(0).Item(1)
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
            'Agregar Columna de DIA_HABIL_PAGO
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.Int32")
            column.ColumnName = "DIA_HABIL_PAGO"
            MiTabla.Columns.Add(column)

            'Agregar Columna de fecha de exigibilidad
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.DateTime")
            column.ColumnName = "FECHA_EXIGIBILIDAD"
            MiTabla.Columns.Add(column)

            'Agregar Columna de interes sin ajustar
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "INTERESES_NORMAL"
            MiTabla.Columns.Add(column)

            'Agregar Columna de total pago sin ajustar
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "TOTAL_PAGAR_NORMAL"
            MiTabla.Columns.Add(column)

            'Agregar Columna de interes ajustado
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "INTERESES_AJUSTADO"
            MiTabla.Columns.Add(column)

            'Agregar Columna de total pago ajustado
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = "TOTAL_PAGAR_AJUSTADO"
            MiTabla.Columns.Add(column)

            'Agregar Columna de DIAS DE MORA
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.Int32")
            column.ColumnName = "DIAS_MORA"
            MiTabla.Columns.Add(column)


            'Agregar Columna de id calculo
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.Int32")
            column.ColumnName = "ID_CALCULO"
            MiTabla.Columns.Add(column)

            'Agregar Columna de fecha del sys
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.DateTime")
            column.ColumnName = "FECHA_SYS"
            MiTabla.Columns.Add(column)

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

    Private Function TableExport(ByVal expediente As String) As DataTable
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("EXPEDIENTE", GetType(String)), _
                                               New DataColumn("LIQ_REC", GetType(Integer)), _
                                               New DataColumn("INF", GetType(String)), _
                                               New DataColumn("SUBSISTEMA", GetType(String)), _
                                               New DataColumn("NIT_EMPRESA", GetType(String)), _
                                               New DataColumn("RAZON_SOCIAL", GetType(String)), _
                                               New DataColumn("ANNO", GetType(Integer)), _
                                               New DataColumn("MES", GetType(Integer)), _
                                               New DataColumn("CEDULA", GetType(String)), _
                                               New DataColumn("NOMBRE", GetType(String)), _
                                               New DataColumn("IBC", GetType(Double)), _
                                               New DataColumn("AJUSTE", GetType(Double)), _
                                               New DataColumn("FECHA_PAGO", GetType(Date)), _
                                               New DataColumn("ID_GRUPO", GetType(String))})


        Dim _Adap As New SqlDataAdapter
        Dim sqlAportante As String = "SELECT EXPEDIENTE,LIQ_REC ,INF,SUBSISTEMA,NIT_EMPRESA,RAZON_SOCIAL,ANNO,MES,CEDULA,NOMBRE,IBC,AJUSTE,ID_GRUPO,FECHA_PAGO FROM SQL_PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE ORDER BY ANNO,MES"

        _Adap = New SqlDataAdapter(sqlAportante, Funciones.CadenaConexion)
        _Adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", expediente)
        _Adap.Fill(dt)

        Return dt

    End Function

    Private Sub ModificarPlanilla(ByVal pExpediente As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)
        Dim command As New SqlCommand("DELETE FROM PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE ", cn, _trans)
        command.Parameters.AddWithValue("@EXPEDIENTE", pExpediente)
        command.ExecuteNonQuery()
    End Sub

End Class