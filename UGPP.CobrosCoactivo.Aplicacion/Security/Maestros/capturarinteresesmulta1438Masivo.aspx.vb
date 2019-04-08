Imports System.Data.SqlClient
Imports System.IO

Partial Public Class capturarinteresesmulta1438Masivo
    Inherits System.Web.UI.Page

    Protected Shared DatosImportado As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTasas()
            grdExpe.DataSource = SearchExpdiente()
            grdExpe.DataBind()
            lblCount.Text = "Se detectaron: " & grdExpe.Rows.Count & " registro(s)"
            lblCount.ForeColor = Drawing.Color.Red
        End If

    End Sub

    Protected Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click
        lblError.Text = ""
        Dim exp As String = ""
        Try
            TableExport()
            Dim seleccionado As CheckBox
            Dim sw As Boolean = False
            For Each row As GridViewRow In grdExpe.Rows
                seleccionado = grdExpe.Rows(row.RowIndex).FindControl("chkSeleccion")
                If seleccionado.Checked Then
                    sw = True
                    exp = row.Cells(2).Text
                    Dim datos As _DatosRetorno = MostrarEA(exp)
                    If datos._Error <> "" Then
                        DatosImportado.Rows.Add(exp, datos._Error, "", 0, 0, 0, 0, 0)
                    ElseIf datos.Deuda <= 0 Then
                        DatosImportado.Rows.Add(exp, "No existe deuda.", "", 0, 0, 0, 0, 0)
                    Else
                        Dim TotalDeuda As Double = 0
                        Dim FechaInicioPago As Date = CDate(Now())
                        Dim DiasMora As Integer = FuncionesInteresesMultas.CalcularDiasMoras(CDate(datos.FechaExig).ToString("dd/MM/yyyy"), FechaInicioPago.ToString("dd/MM/yyyy"))
                        Dim InteresesMora As Integer = CInt(FuncionesInteresesMultas.CalcularInteresesMoras(datos.Deuda, DiasMora))
                        'Calcular Total Deuda
                        TotalDeuda = Math.Round(CDbl(datos.Deuda) + CDbl(InteresesMora))
                        SaveLOG("IN", CDec(datos.Deuda), CDate(datos.FechaExig), CDate(FechaInicioPago), CInt(DiasMora), CDec(InteresesMora), CDec(TotalDeuda), 0, 0, 0, 0, Session("sscodigousuario"))
                        DatosImportado.Rows.Add(exp, CDate(datos.FechaExig).ToString("dd/MM/yyyy"), FechaInicioPago.ToString("dd/MM/yyyy"), datos.Deuda.ToString("N0"), DiasMora, ViewState("anual"), InteresesMora.ToString("N0"), TotalDeuda.ToString("N0"))
                    End If

                End If
            Next

            'Dim DatosExpediente As DataTable = SearchExpdiente()
            If sw Then
                lblError.Text = "Proceso terminado satisfactoriamente, click en descargar para ver detalle."
                'CargarTasas()
            Else
                lblError.Text = "Seleccione por lo menos un expediente, para realizar el cálculo de intereses."
            End If


        Catch ex As Exception
            lblError.Text = "ADVERTENCIA: " & exp & " " & ex.ToString()
        End Try
    End Sub
    'Cargar Tasas de intereses
    Private Sub CargarTasas()
        'Cargar Tasas de intereses
        Dim tasas() As String = FuncionesInteresesMultas.CargarTasas

        ViewState("anual") = CDec(tasas(0))
        ViewState("mensual") = CDec(tasas(1))
    End Sub

    Private Function MostrarEA(ByVal pNumExpediente As String) As _DatosRetorno
        Dim datos As New _DatosRetorno
        Dim fecha As String = ""
        Dim valor As Double
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()


        Dim totalDeuda, pagos, saldo As Double

        'Conexion a la base de datos

        'Consultar el total de la deuda
        Dim sql As String = "SELECT SUM(totaldeuda) AS totaldeuda " & _
                                      "FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pNumExpediente.Trim & "' GROUP BY MT_expediente"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'Dim n As Double = Convert.ToDouble(Reader("totaldeuda").ToString())
            'txtTotalDeudaEA.Text = n.ToString("N0")                
            totalDeuda = Convert.ToDouble(Reader("totaldeuda").ToString())
        Else
            totalDeuda = 0
        End If
        Reader.Close()

        '20/10/2014. Se incorporó el ajuste 1406 dentro del capital. xxxyyy
        'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
        Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital " & _
                                "FROM pagos WHERE NroExp = '" & pNumExpediente.Trim & "' GROUP BY NroExp"

        Dim Command2 As New SqlCommand(sql2, Connection)
        Dim Reader2 As SqlDataReader = Command2.ExecuteReader
        If Reader2.Read Then
            If Reader2("pagcapital").ToString() = "" Then
                pagos = 0
            Else
                pagos = Convert.ToDouble(Reader2("pagcapital").ToString())

            End If
        Else
            pagos = 0
        End If
        Reader2.Close()

        'Mostrar la diferencia entre Total deuda - Capital pagado
        saldo = Convert.ToDouble(totalDeuda) - Convert.ToDouble(pagos)
        valor = saldo

        If CDbl(pagos) = 0 Then
            'Consultar fecha exigibilidad del titulo.
            Dim sql3 As String = "SELECT MT.MT_fec_exi_liq " & _
                                " FROM MAESTRO_TITULOS MT  " & _
                                " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " & _
                                "WHERE MT.MT_expediente = '" & pNumExpediente & "'"

            Dim Command3 As New SqlCommand(sql3, Connection)
            Dim Reader3 As SqlDataReader = Command3.ExecuteReader
            If Reader3.Read Then

                If Reader3("MT_fec_exi_liq").ToString = "" Then
                    datos._Error = "No se ha detectado fecha de Exigibilidad, Por favor verifique la información del titulo"
                Else
                    fecha = Convert.ToDateTime(Reader3("MT_fec_exi_liq")).ToString("dd/MM/yyyy")
                End If
            Else
                fecha = ""
                datos._Error = "No se ha detectado fecha de Exigibilidad, Por favor verifique la información del titulo"
            End If
            Reader3.Close()

        Else


            '--***********************************************************************
            'Consultar Fecha del ultimo pago
            Dim sql4 As String = "select MAX(pagFecha ) pagfecha from PAGOS where NroExp = '" & pNumExpediente & "'"

            Dim Command4 As New SqlCommand(sql4, Connection)
            Dim Reader4 As SqlDataReader = Command4.ExecuteReader
            If Reader4.Read Then

                If Reader4("pagfecha").ToString = "" Then
                    fecha = ""
                    datos._Error = "No se ha detectado fecha del pago, Por favor verifique"
                Else
                    fecha = Convert.ToDateTime(Reader4("pagfecha")).ToString("dd/MM/yyyy")
                    fecha = DateAdd(DateInterval.Day, 1, CDate(fecha))
                End If
            End If
            Reader4.Close()
        End If

        datos.FechaExig = fecha
        datos.Deuda = valor

        Connection.Close()

        Return datos

    End Function

    Private Function TableExport() As DataTable
        DatosImportado = New DataTable
        DatosImportado.Columns.AddRange(New DataColumn() {New DataColumn("EXPEDIENTE", GetType(String)), _
                                                New DataColumn("FECHA_EXIG", GetType(String)), _
                                                New DataColumn("FECHA_PAGO", GetType(String)), _
                                                New DataColumn("DEUDA", GetType(String)), _
                                                New DataColumn("DIAS_MORA", GetType(String)), _
                                               New DataColumn("TASA", GetType(String)), _
                                               New DataColumn("INTERESES", GetType(String)), _
                                               New DataColumn("TOTAL_DEUDA", GetType(String))})

        Return DatosImportado

    End Function

    Public Function SearchExpdiente() As DataTable
        Dim Table As New DataTable
        'Dim _Adap As New SqlDataAdapter("select EJEFISGLOBAL.EFINROEXP, SUM(EFISALDOCAP) EFISALDOCAP  from maestro_titulos,EJEFISGLOBAL where mt_expediente = efinroexp and mt_tipo_titulo = '07' and efiestado not in ('07','08','04') and efinroexp = '81857'  group by EFINROEXP having (SUM(EFISALDOCAP)> 0) ORDER BY EFINROEXP", Funciones.CadenaConexion)
        Dim _Adap As New SqlDataAdapter("SELECT deudor ID, ED.ED_Nombre deudor,MT.MT_EXPEDIENTE EFINROEXP,SUM(EFIVALDEU) EFIVALDEU FROM DEUDORES_EXPEDIENTES DxE LEFT JOIN ENTES_DEUDORES ED ON DxE.deudor = ED.ED_Codigo_Nit INNER JOIN (select mt_expediente,mt_tipo_titulo , sum(totaldeuda) deuda   from maestro_titulos  group by mt_expediente,mt_tipo_titulo) MT ON DXE.NroExp = MT.MT_EXPEDIENTE INNER JOIN EJEFISGLOBAL EJ ON EJ.EFINROEXP = MT.MT_EXPEDIENTE WHERE mt_tipo_titulo = '05' and efiestado not in ('07','08','04') AND TIPO = 1 group by  deudor , ED.ED_Nombre ,MT.MT_EXPEDIENTE having (SUM(EFISALDOCAP)> 0) ORDER BY MT.MT_EXPEDIENTE", Funciones.CadenaConexion)
        _Adap.Fill(Table)
        Return Table
    End Function

    Public Function SearchExpdiente(ByVal pExpediente As String) As DataTable
        Dim Table As New DataTable
        'Dim _Adap As New SqlDataAdapter("select EJEFISGLOBAL.EFINROEXP, SUM(EFISALDOCAP) EFISALDOCAP  from maestro_titulos,EJEFISGLOBAL where mt_expediente = efinroexp and mt_tipo_titulo = '07' and efiestado not in ('07','08','04') and efinroexp = '81857'  group by EFINROEXP having (SUM(EFISALDOCAP)> 0) ORDER BY EFINROEXP", Funciones.CadenaConexion)
        Dim sql As String = ""
        Dim _Adap As New SqlDataAdapter

        If pExpediente = "" Then
            sql = "select EJEFISGLOBAL.EFINROEXP,  SUM(EFIVALDEU) EFIVALDEU  from maestro_titulos,EJEFISGLOBAL where mt_expediente = efinroexp and mt_tipo_titulo = '05' and efiestado not in ('07','08','04')   group by EFINROEXP having (SUM(EFISALDOCAP)> 0) ORDER BY EFINROEXP"
            _Adap = New SqlDataAdapter(sql, Funciones.CadenaConexion)
        Else
            sql = "select EJEFISGLOBAL.EFINROEXP,  SUM(EFIVALDEU) EFIVALDEU  from maestro_titulos,EJEFISGLOBAL where mt_expediente = efinroexp and mt_tipo_titulo = '05' and efiestado not in ('07','08','04') and efinroexp = @exp   group by EFINROEXP having (SUM(EFISALDOCAP)> 0) ORDER BY EFINROEXP"
            _Adap = New SqlDataAdapter(sql, Funciones.CadenaConexion)
            _Adap.SelectCommand.Parameters.AddWithValue("@exp", pExpediente)
        End If



        _Adap.Fill(Table)
        Return Table
    End Function


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

    Protected Sub cmdDescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDescargar.Click
        If DatosImportado.Rows.Count > 0 Then
            descargafichero(DatosImportado, Now.ToString("ddMMyyyyhhssmm") & Request("pExpediente") & ".xls", True)
        Else
            lblError.Text = "No hay información a descargar"
        End If
    End Sub

    Private Sub SaveLOG(ByVal TIPO As String, ByVal VALOR_CAPITAL As Decimal, ByVal FECHA_EXIG As Date, ByVal FECHA_PAGO As Date, ByVal DIAS_MORA As Integer, ByVal INTERESES As Decimal, ByVal TOTAL_DEUDA As Decimal, ByVal IPC As Decimal, ByVal VALOR_IPC As Decimal, ByVal ANIO_IPC As Integer, ByVal TASA As Decimal, ByVal USUARIO As String)
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim command As New SqlCommand("SP_LOG_IPC_INTERESES", cn)
        command.CommandType = CommandType.StoredProcedure
        command.Parameters.AddWithValue("@TIPO", TIPO)
        command.Parameters.AddWithValue("@VALOR_CAPITAL", VALOR_CAPITAL)
        command.Parameters.AddWithValue("@FECHA_EXIG", FECHA_EXIG)
        command.Parameters.AddWithValue("@FECHA_PAGO", IIf(FECHA_PAGO = "12:00:00 AM", DBNull.Value, FECHA_PAGO))
        command.Parameters.AddWithValue("@DIAS_MORA", DIAS_MORA)
        command.Parameters.AddWithValue("@INTERESES", INTERESES)
        command.Parameters.AddWithValue("@TOTAL_DEUDA", TOTAL_DEUDA)
        command.Parameters.AddWithValue("@IPC", IPC)
        command.Parameters.AddWithValue("@VALOR_IPC", VALOR_IPC)
        command.Parameters.AddWithValue("@ANIO_IPC", ANIO_IPC)
        command.Parameters.AddWithValue("@TASA", TASA)
        command.Parameters.AddWithValue("@USUARIO", USUARIO)
        cn.Open()
        command.ExecuteNonQuery()
        cn.Close()

    End Sub

    <Serializable()> _
Public Class _DatosRetorno
        Public Deuda As Double
        Public FechaExig As String
        Public CALCULO As DataTable
        Public _Error As String = ""
    End Class

    Protected Sub chkMarcar_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkMarcar.CheckedChanged
        Dim presentado As CheckBox
        If grdExpe.Rows.Count > 0 Then
            If chkMarcar.Checked Then
                For Each row As GridViewRow In grdExpe.Rows
                    presentado = grdExpe.Rows(row.RowIndex).FindControl("chkSeleccion")
                    presentado.Checked = True
                Next
            Else
                For Each row As GridViewRow In grdExpe.Rows
                    presentado = grdExpe.Rows(row.RowIndex).FindControl("chkSeleccion")
                    presentado.Checked = False
                Next
            End If

        End If

    End Sub
End Class