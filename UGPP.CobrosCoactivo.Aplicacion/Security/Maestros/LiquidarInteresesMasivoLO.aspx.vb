Imports System.Data.SqlClient
Imports System.IO

Partial Public Class LiquidarInteresesMasivoLO
    Inherits System.Web.UI.Page

    Protected Shared DatosImportado As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            grdExpe.DataSource = SearchExpdiente()
            grdExpe.DataBind()
            lblCount.Text = "Se detectaron: " & grdExpe.Rows.Count & " registro(s)"
            lblCount.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click

        Try
            Dim exp As String = ""
            Dim Vdeuda As Double = 0
            lblError.Text = ""
            TableExport()
            Dim seleccionado As CheckBox
            Dim funcion As New capturarintereses
            Dim sw As Boolean = False
            For Each row As GridViewRow In grdExpe.Rows
                seleccionado = grdExpe.Rows(row.RowIndex).FindControl("chkSeleccion")
                If seleccionado.Checked Then
                    sw = True
                    exp = row.Cells(2).Text

                    DatosImportado = New DataTable
                    DatosImportado = funcion.VerificarsQL(exp)

                    If DatosImportado.Rows.Count > 0 Then

                    End If

                End If
            Next

            If sw Then
                lblError.Text = "Proceso finalizado, click el descargar para ver el detalle del proceso."
            Else
                lblError.Text = "Seleccione por lo menos un expediente, para realizar el cálculo de intereses."
            End If

        Catch ex As Exception
            lblError.Text = "Error interno, por favor volver a darle click en el botón calcular."
        End Try


    End Sub

    Private Function TableExport() As DataTable
        DatosImportado = New DataTable
        DatosImportado.Columns.AddRange(New DataColumn() {New DataColumn("EXPEDIENTE", GetType(String)), _
                                               New DataColumn("LIQ_REC", GetType(String)), _
                                               New DataColumn("INF", GetType(String)), _
                                               New DataColumn("SUBSISTEMA", GetType(String)), _
                                               New DataColumn("NIT_EMPRESA", GetType(String)), _
                                               New DataColumn("RAZON_SOCIAL", GetType(String)), _
                                               New DataColumn("ANNO", GetType(String)), _
                                               New DataColumn("MES", GetType(String)), _
                                               New DataColumn("CEDULA", GetType(String)), _
                                               New DataColumn("NOMBRE", GetType(String)), _
                                               New DataColumn("IBC", GetType(String)), _
                                               New DataColumn("AJUSTE", GetType(String)), _
                                               New DataColumn("ID_GRUPO", GetType(String)), _
                                               New DataColumn("DIA_HABIL_PAGO", GetType(String)), _
                                               New DataColumn("FECHA_EXIGIBILIDAD", GetType(String)), _
                                               New DataColumn("INTERESES_NORMAL", GetType(String)), _
                                               New DataColumn("TOTAL_PAGAR_NORMAL", GetType(String)), _
                                               New DataColumn("INTERESES_AJUSTADO", GetType(String)), _
                                               New DataColumn("TOTAL_PAGAR_AJUSTADO", GetType(String)), _
                                               New DataColumn("DIAS_MORA", GetType(String)), _
                                               New DataColumn("FECHA_SYS", GetType(DateTime))})
        Return DatosImportado

    End Function

    Protected Sub cmdDescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDescargar.Click
        If DatosImportado.Rows.Count > 0 Then
            descargafichero(DatosImportado, Now.ToString("ddMMyyyyhhssmm") & ".xls", True)
        Else
            lblError.Text = "No hay información a descargar"
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

    Private Function FechaExigibilidadTitulo(ByVal pExpediente As String) As String
        'Conexion a la base de datos

        Dim txtFecExig As String = ""
        Dim tb As New DataTable
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Consultar fecha exigibilidad del titulo.
        Dim sql3 As String = "SELECT MT.MT_fec_exi_liq " & _
                            " FROM MAESTRO_TITULOS MT  " & _
                            " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " & _
                            "WHERE MT.MT_expediente = @EXP"

        Dim Command3 As New SqlCommand(sql3, Connection)
        Command3.Parameters.AddWithValue("@EXP", pExpediente)
        Command3.CommandTimeout = 6000000
        Dim _adap As New SqlDataAdapter(Command3)
        _adap.Fill(tb)

        If tb.Rows.Count > 0 Then
            If tb.Rows(0).Item("MT_fec_exi_liq").ToString = "" Then
                txtFecExig = ""
            Else
                txtFecExig = CDate(tb.Rows(0).Item("MT_fec_exi_liq")).ToString("dd/MM/yyyy")
            End If

        End If
        Return txtFecExig
    End Function

    'Cargar Tasas de intereses
    Public Function SearchExpdiente() As DataTable
        Dim Table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT deudor ID, ED.ED_Nombre deudor,MT.MT_EXPEDIENTE EFINROEXP,SUM(EFIVALDEU) EFIVALDEU FROM DEUDORES_EXPEDIENTES DxE LEFT JOIN ENTES_DEUDORES ED ON DxE.deudor = ED.ED_Codigo_Nit INNER JOIN maestro_titulos MT ON DXE.NroExp = MT.MT_EXPEDIENTE INNER JOIN EJEFISGLOBAL EJ ON EJ.EFINROEXP = MT.MT_EXPEDIENTE WHERE mt_tipo_titulo IN('01','02','04') and efiestado not in ('07','08','04') AND TIPO = 1 group by  deudor , ED.ED_Nombre ,MT.MT_EXPEDIENTE having (SUM(EFISALDOCAP)> 0) AND MT.MT_EXPEDIENTE IN (SELECT DISTINCT EXPEDIENTE FROM SQL_PLANILLA ) ORDER BY MT.MT_EXPEDIENTE", Funciones.CadenaConexion)
        'Dim _Adap As New SqlDataAdapter("select EJEFISGLOBAL.EFINROEXP,  SUM(EFIVALDEU) EFIVALDEU  from maestro_titulos,EJEFISGLOBAL where mt_expediente = efinroexp and mt_tipo_titulo = '07' and efiestado not in ('07','08','04')   group by EFINROEXP having (SUM(EFISALDOCAP)> 0) ORDER BY EFINROEXP", Funciones.CadenaConexion)
        _Adap.Fill(Table)
        Return Table
    End Function

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

    <Serializable()> _
  Public Class Valores
        Public Deuda As String
        Public DeudaAcumulada As String
    End Class


End Class