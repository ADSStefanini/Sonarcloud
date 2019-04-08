Imports System.Data.SqlClient

Public Class CalcularIPCinduvidual
    Dim _datosIPC As DataTable = CargarIPC()
    Protected Shared DatosImportado As New DataTable
    'Cargar Tasas de intereses
    Private Function CargarIPC() As DataTable
        Dim Table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT * FROM IPC ", Funciones.CadenaConexion)
        _Adap.Fill(Table)
        Return Table
    End Function

    Private Function RoundI(ByVal x As Decimal, Optional ByVal d As Integer = 0) As Decimal
        Dim m As Decimal
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

    Private Function TableExport() As DataTable
        DatosImportado = New DataTable
        DatosImportado.Columns.AddRange(New DataColumn() {New DataColumn("VALOR_CAPITAL", GetType(String)), _
                                                New DataColumn("FECHA_EXIG", GetType(String)), _
                                                New DataColumn("CORTE_ACTUALIZACION", GetType(String)), _
                                                New DataColumn("DIAS_MORA", GetType(String)), _
                                               New DataColumn("IPC", GetType(String)), _
                                               New DataColumn("ANIO", GetType(String)), _
                                               New DataColumn("VALOR_IPC", GetType(String)), _
                                               New DataColumn("VALOR_ACTUALIZADO", GetType(String))})

        Return DatosImportado

    End Function

    'Protected Sub cmdDescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDescargar.Click
    '    If DatosImportado.Rows.Count > 0 Then
    '        descargafichero(DatosImportado, Now.ToString("ddMMyyyyhhssmm") & Request("pExpediente") & ".xls", True)
    '    Else
    '        lblError.Text = "No hay información a descargar"
    '    End If

    'End Sub

    'Private Sub descargafichero(ByVal dtTemp As DataTable, ByVal fname As String, ByVal forceDownload As Boolean, Optional ByVal plantilla As String = "")

    '    Dim ext As String = Path.GetExtension(fname)
    '    Dim type As String = ""
    '    ' mirar las extensiones conocidas	
    '    If ext IsNot Nothing Then
    '        Select Case ext.ToLower()
    '            Case ".htm", ".html"
    '                type = "text/HTML"
    '                Exit Select

    '            Case ".txt"
    '                type = "text/plain"
    '                Exit Select

    '            Case ".doc", ".rtf"
    '                type = "Application/msword"
    '                Exit Select

    '            Case ".xls"
    '                type = "Application/vnd.ms-excel"
    '                Exit Select

    '        End Select
    '    End If


    '    Response.Clear()
    '    Response.Buffer = True

    '    If forceDownload Then
    '        Response.AppendHeader("content-disposition", "attachment; filename=" & fname)
    '    End If

    '    If type <> "" Then
    '        Response.ContentType = type
    '    End If

    '    If plantilla <> "" Then
    '        Response.Write(plantilla)
    '    Else
    '        Dim sb As StringBuilder = New StringBuilder()
    '        Dim SW As System.IO.StringWriter = New System.IO.StringWriter(sb)
    '        Dim htw As HtmlTextWriter = New HtmlTextWriter(SW)
    '        Dim dg As New DataGrid()

    '        dg.DataSource = dtTemp
    '        dg.DataBind()

    '        dg.RenderControl(htw)
    '        Response.Charset = "UTF-8"
    '        Response.ContentEncoding = Encoding.Default
    '        Response.Write(SW)
    '    End If



    '    Response.End()

    'End Sub

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

    Private Function Pagos(ByVal pExpediente As String) As DataTable
        Dim tb As New DataTable
        Dim strNombreTitulo As String = "LIQUIDACIÓN OFICIAL / SANCIÓN"
        Dim blnLiqSan As Boolean = False
        'Conexion a la base de datos
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        '20/10/2014. Se incorporó el ajuste 1406 dentro del capital. xxxyyy
        'Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
        '
        '
        Dim sql As String = " SELECT	TT.codigo, TT.nombre	 
                              FROM		MAESTRO_TITULOS MT 
                              INNER JOIN	TIPOS_TITULO TT ON TT.codigo = MT.MT_tipo_titulo
                              WHERE		MT.MT_expediente = @NroExp "

        Using cnx As New SqlConnection(Funciones.CadenaConexion)
            cnx.Open()
            Using Command As New SqlCommand(sql, cnx)
                Command.Parameters.AddWithValue("@NroExp", pExpediente)
                Using Reader = Command.ExecuteReader()
                    If Reader.Read() Then
                        blnLiqSan = IIf(Reader("nombre").ToString() = strNombreTitulo, True, False)
                    End If
                End Using
            End Using
            cnx.Close()
        End Using

        Dim sql1 As String = " SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital,pagFecha,YEAR(pagFecha) ANIO " &
                             " FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' " &
                             " AND pagFecha IN (SELECT MAX(pagFecha) FROM pagos pg WHERE pg.NroExp = pagos.NroExp "

        Dim sql2 As String = " AND  pagLiqSan = 1 ) "

        Dim sql3 As String = " GROUP BY pagFecha order by pagFecha "

        sql1 += IIf(blnLiqSan, sql2 + sql3, " )" + sql3)

        Dim Command2 As New SqlCommand(sql1, Connection)
        Dim adap As New SqlDataAdapter(Command2)
        adap.Fill(tb)
        Return tb
    End Function

    Private Function FechaUltimoPagos(ByVal pExpediente As String) As String
        Dim tb As New DataTable
        Dim salida As String = ""
        'Conexion a la base de datos
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Dim sql2 As String = "SELECT MAX(pagFecha) pagFecha FROM PAGOS  WHERE NroExp ='" & pExpediente & "'"
        Dim Command2 As New SqlCommand(sql2, Connection)
        Dim adap As New SqlDataAdapter(Command2)
        adap.Fill(tb)
        If tb.Rows.Count > 0 Then
            salida = tb.Rows(0).Item("pagFecha").ToString
        End If
        Return salida
    End Function

    Private Function FechaExigibilidadTitulo(ByVal pExpediente As String) As String
        Dim fechaExig As String

        'Conexion a la base de datos
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        'Consultar fecha exigibilidad del titulo.
        Dim sql3 As String = "SELECT MT.MT_fec_exi_liq " & _
                            " FROM MAESTRO_TITULOS MT  " & _
                            " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " & _
                            "WHERE MT.MT_expediente = '" & pExpediente & "'"

        Dim Command3 As New SqlCommand(sql3, Connection)
        Dim Reader3 As SqlDataReader = Command3.ExecuteReader
        If Reader3.Read Then
            If Reader3("MT_fec_exi_liq").ToString = "" Then
                fechaExig = ""
                'txtCorte.Text = ""
                'lblError.Text = "No se ha detectado fecha de Exigibilidad, Por favor verifique"
            Else
                fechaExig = Convert.ToDateTime(Reader3("MT_fec_exi_liq")).ToString("dd/MM/yyyy")
                'txtCorte.Text = "31/12/" & Year(CDate(txtFecExig.Text))
            End If
        Else
            fechaExig = ""
            'txtCorte.Text = ""
            'lblError.Text = "No se ha detectado fecha de Exigibilidad, Por favor verifique"
        End If
        Reader3.Close()
        Connection.Close()

        Return fechaExig
    End Function

    Public Function CalculoIpc(ByVal pexpediente As String, ByVal pFechaPago As String, ByVal pDeudatotal As Decimal, ByVal pUsuario As String) As _DatosRetorno
        Dim datos As New _DatosRetorno
        Dim fechaExi As String = FechaExigibilidadTitulo(pexpediente)
        datos.VALORIPC = 0
        If Year(CDate(fechaExi)) < Year(CDate(pFechaPago)) Then
            Dim datosIpc As DataTable = CType(_datosIPC, DataTable)
            If datosIpc.Rows.Count = 0 Then
                datos._Error = "No está configurado el porcentaje IPC."
                Return datos
            End If

            '/-----------------------------------------------------------------  
            'ID _HU:  015
            'Nombre HU : Error en Cálculo del IPC
            'Empresa: UT TECHNOLOGY 
            'Autor: Jeisson Gómez 
            'Fecha: 26-05-2017  
            'Objetivo : Cambiar la pregunta, para permitir liquidar el IPC 
            ' 1. Si no existe el pago en un año mayor a la fecha de exigibilidad 
            ' 2. Si no existe un realiza no se realizado un pago. 
            '------------------------------------------------------------------/ Aquí Ojo Jeisson 
            TableExport()
            Dim datosPago As DataTable = Pagos(pexpediente)
            Dim intFechaUltPago As Integer = Year(fechaExi)
            If datosPago.Rows.Count > 0 Then
                intFechaUltPago = IIf(IsDBNull(datosPago.Rows(0).Item("pagFecha")), Year(fechaExi), Year(CDate(Format(datosPago.Rows(0).Item("pagFecha"), "dd/MM/yyyy"))))
                ' Jeisson Gómez 
                ' 12/07/2017. Si el título no tiene pagos, se envía la fecha inicial de exigibilidad del título 
                ' Caso contrario, se debe enviar la fecha del último pago + 1 día, la cual sería la nueva fecha de exibilidad. 
                fechaExi = DateAdd(DateInterval.Day, 1, (CDate(datosPago.Rows(0).Item("pagFecha")))).ToString("dd/MM/yyyy")
            End If



            If intFechaUltPago < Year(CDate(pFechaPago)) Then
                'datos = LogicaIPC(datosIpc, pDeudatotal, pFechaPago, pUsuario)
                datos = LogicaIPC(datosIpc, pDeudatotal, fechaExi, pFechaPago, pUsuario)
            Else
                datos.VALORIPC = 0
                datos.CALCULO = Nothing
            End If
        End If
        Return datos
    End Function

    Private Function LogicaIPC(ByVal pDatosIPC As DataTable, ByVal pDeuda As Decimal, ByVal pfechaExi As String, ByVal pFechaPago As Date, ByVal pUsuario As String) As _DatosRetorno
        Dim datosR As New _DatosRetorno
        Dim fechaCorte, diasMora, ipc, valorActualizado As String
        Dim valorIpc As Decimal = 0
        Dim intDiasAnio As Integer = 0

        Dim x As Integer = pDatosIPC.Select(" ANIO = " & Year(CDate(pfechaExi)), String.Empty).Length
        If x > 0 Then
            'For Each z As DataRow In pDatosIPC.Select(" ANIO = " & Year(CDate(pfechaExi)), String.Empty)
            '    ' Jeisson Gómez 
            '    ' Si es con base a la fecha de exigibilidad no se suma el día. 
            '    ' Por lo tanto está línea se comenta 
            '    'pfechaExi = DateAdd(DateInterval.Day, 1, CDate(pfechaExi)).ToString("dd/MM/yyyy")
            '    pfechaExi = CDate(pfechaExi).ToString("dd/MM/yyyy")
            '    fechaCorte = "31/12/" & Year(CDate(pfechaExi))

            '    diasMora = DateDiff(DateInterval.Day, CDate(pfechaExi), CDate(fechaCorte)) + 1
            '    ipc = CDec(z("TASA")) * 100
            '    valorActualizado = CDbl(pDeuda) * (1 + (CDec(z("TASA")) / 365)) ^ CInt(diasMora)
            '    valorActualizado = RedondearUnidades(0, CDbl(valorActualizado))
            '    valorIpc = CDbl(valorActualizado) - CDbl(pDeuda)

            '    SaveLOG("IP", CDec(pDeuda), CDate(pfechaExi), Nothing, CInt(diasMora), 0, CDec(valorActualizado), CDec(ipc), CDec(valorIpc), Year(CDate(pfechaExi)), 0, pUsuario)
            '    DatosImportado.Rows.Add(pDeuda, pfechaExi, fechaCorte, diasMora, ipc, Year(CDate(pfechaExi)), valorIpc, valorActualizado)
            '    pDeuda = valorActualizado
            'Next
            ' Jeisson Gómez 
            ' Cambio la forma de calcular, debido que no lo está haciendo para cuando la fecha de exibilidad debe calcular más de un año. 

            For i As Integer = Year(CDate(pfechaExi)) To Year(CDate(pFechaPago)) - 1
                Dim z As DataRow = pDatosIPC.Select(" ANIO = " & i).FirstOrDefault()
                pfechaExi = IIf(Year(CDate(pfechaExi)) = i, CDate(pfechaExi).ToString("dd/MM/yyyy"), "01/01/" & i)
                'Jeisson Gómez 
                ' HU_015 Error Cálculo IPC 
                ' 13/07/2017 
                ' Para cuando se presenta un año bisiesto es decir 366 días, no se tiene en cuenta el día de más de mora. 
                ' Es decir, para el año bisiesto los días son de 365. Definición de cobros UGPP. 
                fechaCorte = "31/12/" & i
                diasMora = IIf(DateDiff(DateInterval.Day, CDate(pfechaExi), CDate(fechaCorte)) + 1 = 366, 365, DateDiff(DateInterval.Day, CDate(pfechaExi), CDate(fechaCorte)) + 1)

                ipc = CDec(z("TASA")) * 100
                valorActualizado = CDec(pDeuda) * (1 + (CDec(z("TASA")) / 365)) ^ CInt(diasMora)
                valorActualizado = RedondearUnidades(0, CDec(valorActualizado))
                ' Se redondea el valor de la deuda Jeisson Gómez 30/07/2017 HU_015 Control de cambio.
                pDeuda = RedondearUnidades(0, pDeuda)
                valorIpc += CDec(valorActualizado) - CDec(pDeuda)

                SaveLOG("IP", CDec(pDeuda), CDate(pfechaExi), Nothing, CInt(diasMora), 0, CDec(valorActualizado), CDec(ipc), CDec(valorIpc), Year(CDate(pfechaExi)), 0, pUsuario)
                DatosImportado.Rows.Add(pDeuda, pfechaExi, fechaCorte, diasMora, ipc, Year(CDate(pfechaExi)), valorIpc, valorActualizado)
                pDeuda += CDec(valorIpc)
            Next

        Else
            datosR.VALORIPC = 0
            datosR.CALCULO = Nothing
            datosR._Error = "No esta configurado el IPC para el año " & Year(CDate(pfechaExi))
        End If

        datosR.VALORIPC = valorIpc
        datosR.CALCULO = DatosImportado

        Return datosR
    End Function

    <Serializable()> _
    Public Class _DatosRetorno
        Public VALORIPC As String
        Public CALCULO As DataTable
        Public _Error As String = ""
    End Class

End Class
