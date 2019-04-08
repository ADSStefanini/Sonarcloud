Imports System.Data.SqlClient
Imports System.IO

Partial Public Class capturarinteresesmulta
    Inherits System.Web.UI.Page

    Protected Shared DatosImportado As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'MostrarEA(Request("pExpediente"))
            grdExpe.DataSource = SearchExpdiente()
            grdExpe.DataBind()
            lblCount.Text = "Se detectaron: " & grdExpe.Rows.Count & " registro(s)"
            lblCount.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub cmdCalcularInteres_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCalcularInteres.Click

        Dim exp As String = String.Empty
        Dim Vdeuda As Decimal = 0

        Try
            lblError.Text = ""
            TableExport()
            Dim seleccionado As CheckBox
            Dim sw As Boolean = False
            For Each row As GridViewRow In grdExpe.Rows
                seleccionado = grdExpe.Rows(row.RowIndex).FindControl("chkSeleccion")
                If seleccionado.Checked Then
                    sw = True
                    exp = row.Cells(2).Text
                    Vdeuda = CDec(row.Cells(4).Text)
                    LogicaIpc(exp, Vdeuda)
                End If
            Next

            If sw Then
                lblError.Text = "Proceso finalizado, click el descargar para ver el detalle del proceso."
            Else
                lblError.Text = "Seleccione por lo menos un expediente, para realizar el cálculo de IPC."
            End If

        Catch ex As Exception
        lblError.Text = "Error interno, por favor volver a darle click en el botón calcular IPC masivo." + " En el expediente " + ex.Message() + " " + exp
        End Try


    End Sub

    'Cargar Tasas de intereses
    Public Function CargarIPC() As DataTable
        'Dim Table As New DataTable
        'Dim _Adap As New SqlDataAdapter("SELECT * FROM IPC ", Funciones.CadenaConexion)
        '_Adap.Fill(Table)
        'Return Table

        '/-----------------------------------------------------------------  
        'ID _HU:  015 
        'Nombre HU : Error en Cálculco del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 14-07-2017  
        'Objetivo : Con el fin de evitar el siguiente error. ' El período de tiempo de espera expiró antes de obtener una conexión del grupo. 
        ' Esto puede suceder porque todas las conexiones de la agrupación estaban en uso y se alcanzó el máximo tamaño del grupo. 
        '------------------------------------------------------------------/
        Dim Table As New DataTable
        Using cn As New SqlConnection(Funciones.CadenaConexion)
            cn.Open()
            Using Adaptador As New SqlDataAdapter("SELECT * FROM IPC ", cn)
                Adaptador.Fill(Table)
            End Using
            cn.Close()
        End Using
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

    Private Function RedondearUnidades(ByVal pUnidad As Integer, ByVal pValor As Decimal) As Decimal
        Dim Unidad As String = pUnidad
        Dim resultado As Decimal = CDec(pValor)
        Select Case Unidad
            Case 4 'DiezMilesima
                resultado = RoundI(CDec(pValor), 4)
            Case 3 'Milesima
                resultado = RoundI(CDec(pValor), 3)
            Case 2 'Centesima
                resultado = RoundI(CDec(pValor), 2)
            Case 1 'Decima
                resultado = RoundI(CDec(pValor), 1)
            Case 0 'Unidad
                resultado = RoundI(CDec(pValor), 0)
            Case -1 'Decena
                resultado = RoundI(CDec(pValor), -1)
            Case -2 'Centena
                resultado = RoundI(CDec(pValor), -2)
            Case -3 'Mil
                resultado = RoundI(CDec(pValor), -3)
            Case -4 'Diez Mil
                resultado = RoundI(CDec(pValor), -4)
        End Select
        Return resultado
    End Function

    Private Function TableExport() As DataTable
        DatosImportado = New DataTable
        DatosImportado.Columns.AddRange(New DataColumn() {New DataColumn("EXPEDIENTE", GetType(String)),
                                            New DataColumn("TIPO_TITULO", GetType(String)),
                                            New DataColumn("VALOR_CAPITAL", GetType(String)),
                                            New DataColumn("FECHA_EXIG", GetType(String)),
                                            New DataColumn("CORTE_ACTUALIZACION", GetType(String)),
                                            New DataColumn("DIAS_MORA", GetType(String)),
                                            New DataColumn("IPC", GetType(String)),
                                            New DataColumn("ANIO", GetType(String)),
                                            New DataColumn("VALOR_IPC", GetType(String)),
                                            New DataColumn("VALOR_ACTUALIZADO", GetType(String))})

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

    Private Function Pagos(ByVal pExpediente As String) As DataTable
        Dim tb As New DataTable
        'Conexion a la base de datos
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()

        ''20/10/2014. Se incorporó el ajuste 1406 dentro del capital. xxxyyy
        ''Hacer la sumatoria del "CAPITAL PAGADO" (columna I del gestor). No toma el "TOTAL PAGADO" (columna N del gestor)
        'Dim sql2 As String = "SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital,pagFecha,YEAR(pagFecha) ANIO,NROCONSIGNACION " & _
        '                        "FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' GROUP BY pagFecha,NROCONSIGNACION order by pagFecha"

        'Dim Command2 As New SqlCommand(sql2, Connection)
        'Dim adap As New SqlDataAdapter(Command2)
        'adap.Fill(tb)
        'Return tb

        '/-----------------------------------------------------------------  
        'ID _HU:  015 
        'Nombre HU : Error en Cálculco del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 14-07-2017  
        'Objetivo : Con el fin de evitar el siguiente error. El período de tiempo de espera expiró antes de obtener una conexión del grupo. 
        ' Esto puede suceder porque todas las conexiones de la agrupación estaban en uso y se alcanzó el máximo tamaño del grupo. 
        '------------------------------------------------------------------/ 
        Dim sql2 As String = " SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0)) AS pagcapital, pagFecha,YEAR(pagFecha) ANIO, NROCONSIGNACION " &
                             " FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' " &
                             " GROUP BY pagFecha, NROCONSIGNACION " &
                             " HAVING SUM(COALESCE(pagcapital,0)) > 0  " &
                             " ORDER BY pagFecha"

        Using Cn As New SqlConnection(Funciones.CadenaConexion)
            Cn.Open()
            Using Command As New SqlCommand(sql2, Cn)
                Using Adaptador As New SqlDataAdapter(Command)
                    Adaptador.Fill(tb)
                End Using
            End Using
            Cn.Close()
        End Using
        Return tb

    End Function

    Private Function FechaExigibilidadTitulo(ByVal pExpediente As String) As String
        'Conexion a la base de datos
        '/-----------------------------------------------------------------  
        'ID _HU:  015 
        'Nombre HU : Error en Cálculco del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 14-07-2017  
        'Objetivo : Con el fin de evitar el siguiente error. El período de tiempo de espera expiró antes de obtener una conexión del grupo. 
        ' Esto puede suceder porque todas las conexiones de la agrupación estaban en uso y se alcanzó el máximo tamaño del grupo. 
        '------------------------------------------------------------------/  

        Dim txtFecExig As String = String.Empty
        Dim tb As New DataTable
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Consultar fecha exigibilidad del titulo.
        'Dim sql3 As String = "SELECT MT.MT_fec_exi_liq " &
        '                    " FROM MAESTRO_TITULOS MT  " &
        '                    " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " &
        '                    "WHERE MT.MT_expediente = @EXP"

        'Dim Command3 As New SqlCommand(sql3, Connection)
        'Command3.Parameters.AddWithValue("@EXP", pExpediente)
        'Command3.CommandTimeout = 6000000
        'Dim _adap As New SqlDataAdapter(Command3)
        '_adap.Fill(tb)

        'If tb.Rows.Count > 0 Then
        '    If tb.Rows(0).Item("MT_fec_exi_liq").ToString = "" Then
        '       txtFecExig = ""
        '    Else
        '        txtFecExig = CDate(tb.Rows(0).Item("MT_fec_exi_liq")).ToString("dd/MM/yyyy")
        '    End If

        'End If

        Dim sql3 As String = "SELECT MT.MT_fec_exi_liq " &
                            " FROM MAESTRO_TITULOS MT  " &
                            " LEFT JOIN TIPOS_TITULO TT ON MT.MT_tipo_titulo = TT.codigo " &
                            "WHERE MT.MT_expediente = @EXP"

        Using cn As New SqlConnection(Funciones.CadenaConexion)
            cn.Open()
            Using Command As New SqlCommand(sql3, cn)
                Command.Parameters.AddWithValue("@EXP", pExpediente)
                Using Reader = Command.ExecuteReader()
                    If Reader.Read() Then
                        If Reader.IsDBNull(0) Then
                            txtFecExig = String.Empty
                        Else
                            txtFecExig = CDate(Reader("MT_fec_exi_liq")).ToString("dd/MM/yyyy")
                        End If
                    End If
                End Using
            End Using
            cn.Close()
        End Using

        Return txtFecExig
    End Function

    Public Function LogicaIpc(ByVal pexpediente As String, ByVal pdeudatotal As Decimal, Optional ByVal individual As Boolean = False) As DataTable

        Dim pagAnteExig As Decimal = 0
        Dim strTipoTitulo As String = String.Empty

        strTipoTitulo = ObtenerTitoTitulo(pexpediente)


        If individual Then
                TableExport()
            End If

            Dim fechaExi As String
            Dim datosPago As DataTable = Pagos(pexpediente)
            Dim datosIpc As DataTable = CargarIPC()


            If datosIpc.Rows.Count = 0 Then
                lblError.Text = "No está configurado el porcentaje IPC."
                Return Nothing
            End If

            fechaExi = FechaExigibilidadTitulo(pexpediente)

            If fechaExi = String.Empty Then
            DatosImportado.Rows.Add(pexpediente, strTipoTitulo, "", "No se ha detectado fecha de Exigibilidad, Por favor verifique", "", "", "", "", "", "")
            Return Nothing
            End If

        If datosPago.Rows.Count = 0 Then
            CalcularIPC(datosIpc, pdeudatotal, fechaExi, pexpediente) 'Calcular IPC cuando no hay pago

            'If fechaExi = String.Empty Then
            '    DatosImportado.Rows.Add(pexpediente, "",  "", "No se ha detectado fecha de Exigibilidad, Por favor verifique", "", "", "", "", "", "")
            'Else
            '    CalcularIPC(datosIpc, pdeudatotal, fechaExi, pexpediente) 'Calcular IPC cuando no hay pago
            'End If
        ElseIf CDec(datosPago.Compute("SUM(pagcapital)", String.Empty)) = 0 And datosPago.Rows.Count = 1 Then
            CalcularIPC(datosIpc, pdeudatotal, fechaExi, pexpediente) 'Calcular IPC cuando no hay pago
        Else

            'Verificar pagos anteriores a la fecha de exigibilidad.
            pagAnteExig = IIf(datosPago.Compute("sum(pagcapital)", "ANIO <" & Year(fechaExi)) Is DBNull.Value, 0, datosPago.Compute("sum(pagcapital)", "ANIO <" & Year(fechaExi)))

            pdeudatotal = pdeudatotal - pagAnteExig

            If pdeudatotal <= 0 Then
                DatosImportado.Rows.Add(pexpediente, strTipoTitulo, "", "Se detectaron pagos anterioes a la fecha de exigibilidad que ha cubierto la totalidad de la deuda., Por favor verifique", "", "", "", "", "", "")
            Else
                CalcularIPC(datosIpc, pdeudatotal, fechaExi, datosPago, pexpediente) 'Calcular Intereses cuando hay un pago
            End If
        End If

        Return DatosImportado
    End Function

    Private Function CalcularIPC(ByVal pDatosIPC As DataTable, ByVal pDeuda As String, ByVal fechaExi As String, ByVal pexpediente As String) As Decimal
        Dim fechaCorte, diasMora, ipc, valorActualizado, valorIpc, acuIPC As String
        valorIpc = 0
        acuIPC = 0
        valorActualizado = ""
        Dim strTipoTitulo As String = String.Empty

        strTipoTitulo = ObtenerTitoTitulo(pexpediente)

        For Each Z As DataRow In pDatosIPC.Select(" ANIO >= " & Year(CDate(fechaExi)) & " AND  ANIO <=" & Year(CDate(Now)) - 1, "ANIO")

            If Year(CDate(fechaExi)) < CInt(Z("ANIO")) Then
                fechaExi = "01/01/" & CInt(Z("ANIO"))
                fechaCorte = "31/12/" & Year(CDate(fechaExi))
                'Jeisson Gómez 
                ' HU_015 Error Cálculo IPC 
                ' 13/07/2017 
                ' Para cuando se presenta un año bisiesto es decir 366 días, no se tiene en cuenta el día de más de mora. 
                ' Es decir, para el año bisiesto los días son de 365. Definición de cobros UGPP. 
                diasMora = IIf(DateDiff(DateInterval.Day, CDate(fechaExi), CDate(fechaCorte)) + 1 = 366, 365, DateDiff(DateInterval.Day, CDate(fechaExi), CDate(fechaCorte)) + 1)
                ipc = CDec(Z("TASA")) * 100
                valorActualizado = CDec(pDeuda) * (1 + (CDec(Z("TASA")) / 365)) ^ CInt(diasMora)
                valorActualizado = RedondearUnidades(0, CDec(valorActualizado)).ToString("N0")
                ' Se redondea el valor de la deuda Jeisson Gómez 30/07/2017 HU_015 Control de cambio.
                pDeuda = RedondearUnidades(0, pDeuda)
                valorIpc = CDec(valorActualizado).ToString("N0") - CDec(pDeuda).ToString("N0")

                'Guarda log
                SaveLOG("IP", CDec(pDeuda), CDate(fechaExi), Nothing, CInt(diasMora), 0, CDec(valorActualizado), CDec(ipc), CDec(valorIpc), Year(CDate(fechaExi)), 0, Session("sscodigousuario"))
                DatosImportado.Rows.Add(pexpediente, strTipoTitulo, pDeuda, fechaExi, fechaCorte, diasMora, ipc, Year(CDate(fechaExi)), valorIpc, valorActualizado)

                'Guardar datos a exportar
                InsertIpc(pexpediente, pDeuda, valorActualizado, valorIpc)

                pDeuda = valorActualizado
                acuIPC = CDec(acuIPC) + CDec(valorIpc)
            Else

                fechaCorte = "31/12/" & Year(CDate(fechaExi))
                'Jeisson Gómez 
                ' HU_015 Error Cálculo IPC 
                ' 13/07/2017 
                ' Para cuando se presenta un año bisiesto es decir 366 días, no se tiene en cuenta el día de más de mora. 
                ' Es decir, para el año bisiesto los días son de 365. Definición de cobros UGPP. 
                diasMora = IIf(DateDiff(DateInterval.Day, CDate(fechaExi), CDate(fechaCorte)) + 1 = 366, 365, DateDiff(DateInterval.Day, CDate(fechaExi), CDate(fechaCorte)) + 1)
                ipc = CDec(Z("TASA")) * 100
                valorActualizado = CDec(pDeuda) * (1 + (CDec(Z("TASA")) / 365)) ^ CInt(diasMora)
                valorActualizado = RedondearUnidades(0, CDec(valorActualizado)).ToString("N0")
                ' Se redondea el valor de la deuda Jeisson Gómez 30/07/2017 HU_015 Control de cambio.
                pDeuda = RedondearUnidades(0, pDeuda)
                valorIpc = CDec(valorActualizado).ToString("N0") - CDec(pDeuda).ToString("N0")

                'Guarda log
                SaveLOG("IP", CDec(pDeuda), CDate(fechaExi), Nothing, CInt(diasMora), 0, CDec(valorActualizado), CDec(ipc), CDec(valorIpc), Year(CDate(fechaExi)), 0, Session("sscodigousuario"))
                DatosImportado.Rows.Add(pexpediente, strTipoTitulo, pDeuda, fechaExi, fechaCorte, diasMora, ipc, Year(CDate(fechaExi)), valorIpc, valorActualizado)

                'Guardar datos a exportar
                InsertIpc(pexpediente, pDeuda, valorActualizado, valorIpc)

                pDeuda = valorActualizado
                acuIPC = CDec(acuIPC) + CDec(valorIpc)
            End If
        Next

        'Actualizar tabla ejefisglobal columna IPC
        UpdateDeudaTitulo(pexpediente, acuIPC)

        Return CDec(pDeuda)
    End Function

    Private Function CalcularIPC(ByVal pDatosIPC As DataTable, ByVal pDeuda As String, ByVal pfechaExi As String, ByVal pDatosPagos As DataTable, ByVal pexpediente As String) As Decimal
        Dim acuIPC, IPCold, fechaExiOld As String
        acuIPC = 0
        IPCold = 0
        fechaExiOld = pfechaExi



        For Each Z As DataRow In pDatosIPC.Select(" ANIO >= " & Year(CDate(fechaExiOld)) & " AND  ANIO <=" & Year(CDate(Now)) - 1, "ANIO")
            '20-Ene-2016 Cesar Julio
            'Validar pagos en la fecha de exigibilidad 
            Dim x As Integer = pDatosPagos.Select("ANIO = " & CInt(Z("ANIO"))).Length
            If x = 0 Then
                '*-* Si no hay pagos se calcula el IPC tomando la fecha de exigibilidad del titulo hasta 31 de Diciembre 
                'del año de la fecha de exigibilidad.
                '*-* Si hay pagos no se calcula nada.
                Dim v As New Valores
                v = _IPC(pexpediente, Z("TASA"), pfechaExi, pDeuda, "", False)
                pfechaExi = "01/01/" & Year(CDate(pfechaExi)) + 1
                pDeuda = v.Deuda
                acuIPC = CDec(acuIPC) + CDec(v.DeudaAcumulada)
            Else
                For Each P As DataRow In pDatosPagos.Select("ANIO = " & CInt(Z("ANIO")), "pagFecha")
                    pDeuda = CDec(pDeuda) - CDec(P("pagcapital"))
                    If CDate(pfechaExi) < CDate(P("pagFecha")) Then
                        pfechaExi = CDate(P("pagFecha")).ToString("dd/MM/yyyy")
                    Else
                        pfechaExi = DateAdd(DateInterval.Day, -1, CDate(pfechaExi))
                    End If

                Next

                If CDec(pDeuda) > 0 Then
                    Dim v As New Valores
                    v = _IPC(pexpediente, Z("TASA"), pfechaExi, pDeuda, "", True)
                    pfechaExi = "01/01/" & Year(CDate(pfechaExi)) + 1
                    pDeuda = v.Deuda
                    acuIPC = CDec(acuIPC) + CDec(v.DeudaAcumulada)
                End If

            End If


        Next

        UpdateDeudaTitulo(pexpediente, acuIPC)

        Return CDec(pDeuda)
    End Function


    Private Function _IPC(ByVal pexpediente As String, ByVal tasaIPC As String, ByVal pfechaExi As String, ByVal pDeuda As String, ByVal pNroConsignacion As String, Optional ByVal pConPago As Boolean = False) As Valores
        Dim valor As New Valores
        Dim fechaCorte, diasMora, IPC, valorActualizado, valorIpc As String
        Dim strTipoTitulo As String = String.Empty

        strTipoTitulo = ObtenerTitoTitulo(pexpediente)


        If pConPago Then
            pfechaExi = DateAdd(DateInterval.Day, 1, CDate(pfechaExi)).ToString("dd/MM/yyyy")
        End If

        fechaCorte = "31/12/" & Year(CDate(pfechaExi))
        'Jeisson Gómez 
        ' HU_015 Error Cálculo IPC 
        ' 25/07/2017 
        ' Para cuando se presenta un año bisiesto es decir 366 días, no se tiene en cuenta el día de más de mora. 
        ' Es decir, para el año bisiesto los días son de 365. Definición de cobros UGPP. 
        'diasMora = DateDiff(DateInterval.Day, CDate(pfechaExi), CDate(fechaCorte)) + 1
        diasMora = IIf(DateDiff(DateInterval.Day, CDate(pfechaExi), CDate(fechaCorte)) + 1 = 366, 365, DateDiff(DateInterval.Day, CDate(pfechaExi), CDate(fechaCorte)) + 1)

        IPC = CDec(tasaIPC) * 100
        valorActualizado = CDec(pDeuda) * (1 + (CDec(tasaIPC) / 365)) ^ CInt(diasMora)
        valorActualizado = RedondearUnidades(0, CDec(valorActualizado)).ToString("N0")
        valorIpc = CDec(valorActualizado).ToString("N0") - CDec(pDeuda).ToString("N0")

        SaveLOG("IP", CDec(pDeuda), CDate(pfechaExi), Nothing, CInt(diasMora), 0, CDec(valorActualizado), CDec(IPC), CDec(valorIpc), Year(CDate(pfechaExi)), 0, Session("sscodigousuario"))
        DatosImportado.Rows.Add(pexpediente, strTipoTitulo, pDeuda, pfechaExi, fechaCorte, diasMora, IPC, Year(CDate(pfechaExi)), valorIpc, valorActualizado)

        InsertIpc(pexpediente, pDeuda, valorActualizado, valorIpc) 'Guardar Log.

        pDeuda = valorActualizado

        'If pConPago Then
        '    ' UpdateDeudaTituloPagos(pexpediente, valorIpc, pNroConsignacion)
        'Else

        ' End If
        valor.DeudaAcumulada = CDec(valorIpc)
        valor.Deuda = pDeuda

        Return valor
    End Function


    Private Sub InsertIpc(ByVal pExpediente As String, ByVal pvalor_viejo As String, ByVal pvalor_nuevo As String, ByVal pvaloripc As String)
        ' Jeisson Gómez 
        ' Se quitan los decimales para evitar problemas de inserción 
        pvalor_viejo = CStr(CDec(pvalor_viejo).ToString("N0")).Replace(".", String.Empty)
        pvalor_nuevo = CStr(CDec(pvalor_nuevo).ToString("N0")).Replace(".", String.Empty)
        pvaloripc = CStr(CDec(pvaloripc).ToString("N0")).Replace(".", String.Empty)

        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim command As New SqlCommand("INSERT INTO [ACTUALIZACION_IPC_SIN_PAGO]([expdediente],[valor_viejo],[valor_nuevo],[fecha],valoripc )VALUES(@expdediente,@valor_viejo,@valor_nuevo,@fecha,@valoripc)", cn)
        command.Parameters.AddWithValue("@expdediente", pExpediente)
        command.Parameters.AddWithValue("@valor_viejo", pvalor_viejo)
        command.Parameters.AddWithValue("@valor_nuevo", pvalor_nuevo)
        command.Parameters.AddWithValue("@valoripc", pvaloripc)
        command.Parameters.AddWithValue("@fecha", Now())
        cn.Open()
        command.ExecuteNonQuery()
        cn.Close()

    End Sub

    Private Sub UpdateIpc(ByVal pExpediente As String, ByVal pvalor_viejo As String, ByVal pvalor_nuevo As String, ByVal pvaloripc As String)
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim command As New SqlCommand("UPDATE [ACTUALIZACION_IPC_SIN_PAGO] SET [valor_viejo] = @valor_viejo,[valor_nuevo] = @valor_nuevo ,[fecha] = @fecha,valoripc=@valoripc WHERE [expdediente] = @expdediente", cn)
        command.Parameters.AddWithValue("@expdediente", pExpediente)
        command.Parameters.AddWithValue("@valor_viejo", pvalor_viejo)
        command.Parameters.AddWithValue("@valor_nuevo", pvalor_nuevo)
        command.Parameters.AddWithValue("@valoripc", pvaloripc)
        command.Parameters.AddWithValue("@fecha", Now())
        cn.Open()
        command.ExecuteNonQuery()
        cn.Close()

    End Sub

    'Cargar Tasas de intereses
    Public Function SearchIPC(ByVal pExpediente As String) As DataTable
        Dim Table As New DataTable
        Dim _Adap As New SqlDataAdapter("SELECT * FROM ACTUALIZACION_IPC_SIN_PAGO where expdediente = @expdediente ", Funciones.CadenaConexion)
        _Adap.SelectCommand.Parameters.AddWithValue("@expdediente", pExpediente)
        _Adap.Fill(Table)
        Return Table
    End Function

    'Cargar Tasas de intereses
    Public Function SearchExpdiente() As DataTable
        Dim Table As New DataTable
        '/----------------------------------------------------------------------------------------- 
        'ID _HU:  015
        'Nombre HU : Error en Cálculo del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 12-07-2017  
        'Objetivo : Incluir en la generación de los títulos el título 
        ' LIQUIDACIÓN OFICIAL / SANCIÓN. Sólo la parte de la Sanción. 
        ' 1. mt_tipo_titulo IN ('07', '08')  08 - Títulos de LIQUIDACIÓN OFICIAL / SANCIÓN.
        ' 2. AND efiestado not in ('07','08','04') a los estados de los procesos 
        '    07	TERMINADO
        '    08	SUSPENDIDO
        '    04	DEVUELTO
        ' 3. AND TIPO = 1  
        ' Se refiere al DEUDOR PRINCIPAL
        ' 4. AND MT.MT_fec_exi_liq < DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)
        '    La fecha de exigiblidad del título sea menor al al 01/01/ del año actual, debido que no se cobra IPC, para 
        '    el año actual, sino para cuando la fecha de exigibilidad tiene 
        '--------------------------------------------------------------------------------/ 
        'Dim _Adap As New SqlDataAdapter("SELECT deudor ID, ED.ED_Nombre deudor,MT.MT_EXPEDIENTE EFINROEXP,SUM(EFIVALDEU) EFIVALDEU FROM DEUDORES_EXPEDIENTES DxE LEFT JOIN ENTES_DEUDORES ED ON DxE.deudor = ED.ED_Codigo_Nit INNER JOIN  (select mt_expediente,mt_tipo_titulo , sum(totaldeuda) deuda   from maestro_titulos  group by mt_expediente,mt_tipo_titulo) MT ON DXE.NroExp = MT.MT_EXPEDIENTE INNER JOIN EJEFISGLOBAL EJ ON EJ.EFINROEXP = MT.MT_EXPEDIENTE WHERE mt_tipo_titulo = '07' and efiestado not in ('07','08','04') AND TIPO = 1 group by  deudor , ED.ED_Nombre ,MT.MT_EXPEDIENTE having (SUM(EFISALDOCAP)> 0 ) ORDER BY MT.MT_EXPEDIENTE", Funciones.CadenaConexion)
        Using cn As New SqlConnection(Funciones.CadenaConexion)
            cn.Open()
            Using Adaptador As New SqlDataAdapter(
                "   SELECT deudor ID, 
                    ED.ED_Nombre deudor, 
                    MT.MT_EXPEDIENTE EFINROEXP,
                    TT.nombre NOMBRETITULO,
                    CASE mt_tipo_titulo 			
			            WHEN '07' THEN 	SUM(ROUND(EFIVALDEU,0)) 
		                WHEN '08' THEN 	SUM(ROUND(sancion,0))
		            END  EFIVALDEU 
                    FROM DEUDORES_EXPEDIENTES DxE 
                        LEFT JOIN ENTES_DEUDORES ED ON DxE.deudor = ED.ED_Codigo_Nit 
                        INNER JOIN  (SELECT mt_expediente,mt_tipo_titulo, MT_fec_exi_liq,  ISNULL(SUM(totaldeuda),0) deuda , ISNULL(SUM(MT_totalSancion),0) sancion  
                                        FROM   maestro_titulos  group by mt_expediente,mt_tipo_titulo, MT_fec_exi_liq ) MT 
                        ON DXE.NroExp = MT.MT_EXPEDIENTE 
                        INNER JOIN EJEFISGLOBAL EJ ON EJ.EFINROEXP = MT.MT_EXPEDIENTE 
                        INNER JOIN TIPOS_TITULO TT ON TT.codigo = MT.MT_TIPO_TITULO    
                    WHERE   mt_tipo_titulo IN  ('07', '08')
                            AND efiestado not in ('07','08','04') 
                            AND TIPO = 1 
                            AND MT.MT_fec_exi_liq < DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)
                    GROUP BY deudor , ED.ED_Nombre ,MT.MT_EXPEDIENTE, mt_tipo_titulo, TT.nombre
                    HAVING (SUM(EFISALDOCAP)> 0 ) 
                    ORDER BY MT.MT_EXPEDIENTE", cn)
                'Dim _Adap As New SqlDataAdapter("select EJEFISGLOBAL.EFINROEXP,  SUM(EFIVALDEU) EFIVALDEU  from maestro_titulos,EJEFISGLOBAL where mt_expediente = efinroexp and mt_tipo_titulo = '07' and efiestado not in ('07','08','04')   group by EFINROEXP having (SUM(EFISALDOCAP)> 0) ORDER BY EFINROEXP", Funciones.CadenaConexion)
                Adaptador.Fill(Table)
            End Using
            cn.Close()
        End Using
        Return Table
    End Function

    Private Sub UpdateDeudaTitulo(ByVal pExpediente As String, ByVal pDeudaNueva As String)
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim command As New SqlCommand("UPDATE EJEFISGLOBAL SET EFIIPC = @TOTALDEUDA WHERE EFINROEXP = @expdediente", cn)
        command.Parameters.AddWithValue("@expdediente", pExpediente)
        command.Parameters.AddWithValue("@TOTALDEUDA", pDeudaNueva)
        cn.Open()
        command.ExecuteNonQuery()
        cn.Close()

    End Sub

    Private Sub DeleteRows(ByVal pExpediente As String, ByVal pDeudaNueva As String)
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim command As New SqlCommand("UPDATE EJEFISGLOBAL SET EFIIPC = @TOTALDEUDA WHERE EFINROEXP = @expdediente", cn)
        command.Parameters.AddWithValue("@expdediente", pExpediente)
        command.Parameters.AddWithValue("@TOTALDEUDA", pDeudaNueva)
        cn.Open()
        command.ExecuteNonQuery()
        cn.Close()

    End Sub


    Private Sub UpdateDeudaTituloPagos(ByVal pExpediente As String, ByVal pVlrIPC As String, ByVal pNroConsignacion As String)
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim command As New SqlCommand("UPDATE PAGOS SET VLRIPC = @IPC WHERE [NROEXP] = @expdediente AND NROCONSIGNACION = @CONSIG", cn)
        command.Parameters.AddWithValue("@expdediente", pExpediente)
        command.Parameters.AddWithValue("@IPC", pVlrIPC)
        command.Parameters.AddWithValue("@CONSIG", pNroConsignacion)
        cn.Open()
        command.ExecuteNonQuery()
        cn.Close()

    End Sub

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

    Protected Function ObtenerTitoTitulo(ByVal pExpediente As String) As String

        Dim sql As String = String.Empty
        Dim strTipoTitulo As String = String.Empty
        '/----------------------------------------------------------------------------  
        'ID _HU:  015 
        'Nombre HU : Error en Cálculco del IPC
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 30-07-2017  
        'Objetivo : Para el proceso masivo de liquidación, se solicitó que se 
        ' exportará la información actual, y se adicionará el campo tipo de título.
        '-----------------------------------------------------------------------------/  

        sql = " SELECT TT.codigo, TT.nombre " +
              " FROM  MAESTRO_TITULOS MT " +
              " INNER JOIN dbo.TIPOS_TITULO TT " +
              " ON MT.MT_tipo_titulo = TT.codigo " +
              " WHERE MT.MT_expediente = @NroExpediente "

        Using Cn As New SqlConnection(Funciones.CadenaConexion)
            Cn.Open()
            Dim cmd As SqlCommand = New SqlCommand(sql, Cn)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("@NroExpediente", pExpediente)
            Using Reader = cmd.ExecuteReader()
                If Reader.Read() Then
                    strTipoTitulo = IIf(Not IsDBNull(Reader("nombre")), Reader("nombre"), String.Empty)
                End If
            End Using
            Cn.Close()
        End Using
        Return strTipoTitulo

    End Function


    <Serializable()> _
  Public Class Valores
        Public Deuda As String
        Public DeudaAcumulada As String
    End Class


End Class