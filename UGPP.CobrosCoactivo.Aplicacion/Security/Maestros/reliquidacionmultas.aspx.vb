Imports System.Data.SqlClient
Imports System.IO

Partial Public Class reliquidacionmultas
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Sub mostrarMensaje(ByVal sMensaje As String)
        Dim script As String = String.Format("alert('{0}');", sMensaje)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Page_Load", script, True)
    End Sub

    'Calcular Intereses Parafiscales
    Private Function _Calcular(ByVal deuda As Double, ByVal tasa_interes As Integer, ByVal dias As Integer) As Double
        lblError.Text = ""
        Dim intereses As Double = 0

        Try

            'Calcular los interes 
            intereses = (deuda * (tasa_interes / 100) * dias) / 365

            'Redondear Intereses
            intereses = RedondearUnidades(-2, intereses)

            Return intereses

        Catch ex As Exception
            lblError.Text = "Error: por favor compruebe el archivo importado, es posible que el orden de las columnas no coinsidan con la plantilla de ejemplo o que esten campos vacios."
        End Try

    End Function

    Protected Sub cmdImportarcsv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImportarcsv.Click
        Dim filePath As String = String.Empty
        If upload.HasFile AndAlso upload.PostedFile.ContentType.Equals("application/vnd.ms-excel") Then
            Gridinteres.DataSource = DirectCast(ReadToEnd(upload.PostedFile.FileName), DataTable)
            Gridinteres.DataBind()

            Try

            

                For Each row As GridViewRow In Gridinteres.Rows
                    row.Cells(7).Text = Now()
                    If CDate(row.Cells(1).Text) > CDate(CDate(row.Cells(3).Text)) Then
                        lblError.Text = "Por favor, compruebe el archivo a procesar, la fecha de exigibilidad no puede ser superior a la fecha de pago."
                    Else
                        'Calcular dias de moras
                        row.Cells(4).Text = DateDiff(DateInterval.Day, CDate(CDate(row.Cells(1).Text)), CDate(CDate(row.Cells(3).Text))) + 1
                    End If

                    row.Cells(5).Text = _Calcular(CDbl(row.Cells(0).Text), CInt(row.Cells(2).Text), CInt(row.Cells(4).Text))

                    row.Cells(6).Text = CDbl(row.Cells(5).Text) + CDbl(row.Cells(0).Text)

                    'Redondear total Pago
                    row.Cells(6).Text = RedondearUnidades(-2, row.Cells(6).Text)
                Next
                lblError.Text = "Se detectaron " & Gridinteres.Rows.Count & " registro(s) en el archivo. Proceso terminado satisfactorio."
            Catch ex As Exception
                lblError.Text = "Error: Por favor, compruebe el tipo de archivo seleccionado..."
            End Try

        Else
            lblError.Text = "Por favor, compruebe el tipo de archivo seleccionado"
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

                'Agregar Columna de fecha del sys
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.DateTime")
                column.ColumnName = "FECHA_SYS"
                dtDataSource.Columns.Add(column)


                'Add row data
                For i As Integer = 1 To fileContent.Count() - 1
                    Dim rowData As String() = fileContent(i).Split(";"c)
                    dtDataSource.Rows.Add(rowData)
                Next

            End If


        Catch ex As Exception
            mostrarMensaje("Error: " & ex.ToString & "Compruebe el archivo .csv")
        End Try
        Return dtDataSource
    End Function

    Private Sub Save_Planilla(ByVal SUBSISTEMA As String, ByVal NIT_EMPRESA As String, ByVal RAZON_SOCIAL As String, ByVal INF As String, ByVal ANNO As Integer, ByVal MES As Integer, ByVal CEDULA As Integer, ByVal NOMBRE As String, ByVal IBC As Double, ByVal AJUSTE As Double, ByVal EXPEDIENTE As String, ByVal INTERESES As Double, ByVal FECHA_PAGO As Date, ByVal FECHA_EXIGIBILIDAD As Date, ByVal ID_CALCULO As Integer, ByVal DIA_HABIL_PAGO As Integer, ByVal LIQ_REC As Integer, ByVal TOTAL_PAGAR As Double, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

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
        command.Parameters.AddWithValue("@INTERESES", INTERESES)
        command.Parameters.AddWithValue("@FECHA_PAGO", FECHA_PAGO)
        command.Parameters.AddWithValue("@FECHA_EXIGIBILIDAD", FECHA_EXIGIBILIDAD)
        command.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        command.Parameters.AddWithValue("@DIA_HABIL_PAGO", DIA_HABIL_PAGO)
        command.Parameters.AddWithValue("@LIQ_REC", LIQ_REC)
        command.Parameters.AddWithValue("@TOTAL_PAGAR", TOTAL_PAGAR)
        command.ExecuteNonQuery()

    End Sub

    Private Sub Save_LogCalculo(ByVal ID_CALCULO As String, ByVal _trans As SqlTransaction, ByVal cn As SqlConnection)

        Dim command As New SqlCommand("save_LogCalculoInteresParafiscales", cn, _trans)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.AddWithValue("@ID_CALCULO", ID_CALCULO)
        command.ExecuteNonQuery()

    End Sub

    Public Function RoundI(ByVal x As Double, Optional ByVal d As Integer = 0) As Double
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
        'Gridinteres.PageIndex = e.NewPageIndex
        'LLenarDatos()
    End Sub

    Protected Sub cmdExportarExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdExportarExcel.Click

        If Gridinteres.Rows.Count > 0 Then
            Dim sb As StringBuilder = New StringBuilder()
            Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
            Dim Page As Page = New Page()
            Dim form As HtmlForm = New HtmlForm()
            Me.Gridinteres.EnableViewState = False
            Page.EnableEventValidation = False
            Page.DesignerInitialize()
            Page.Controls.Add(form)
            form.Controls.Add(Me.Gridinteres)
            Page.RenderControl(htw)
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=data.xls")
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(sb.ToString())
            Response.End()
        Else
            lblError.Text = "No se encontraron datos a exportar "
        End If
    End Sub


End Class