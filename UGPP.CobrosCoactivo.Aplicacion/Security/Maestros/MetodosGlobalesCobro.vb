Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration

Public Class MetodosGlobalesCobro

    Public DictAlarmas As New Dictionary(Of Integer, String) From
        {{1, "REPARTO"}, {2, "CAMBIO DE ESTADO"}, {3, "DILIGENCIAMIENTO DE LA OBLIGACION"},
        {4, "REMISION BASE CALL"}, {5, "PRIMER OFICION PERSUASIVO"}, {6, "SEGUNDO OFICIO PERSUASIVO"}, {7, "CAMBIAR A COACTIVO"},
        {8, "MANDAMIENTO DE PAGO"}, {9, "ORDEN DE EJECUCION"}, {14, "EXCEPCIONES"}, {15, "REPOSICION"},
        {16, "LIQUIDACION CREDITO"}, {17, "OBJECIONES"}, {18, "APRUEBA CREDITO"}, {20, "INFO CONCURSAL"},
        {21, "PRESENTACION CREDITO"}, {26, "INCUMPLIMIENTO FACILIDAD"}, {29, "CUMPLIMIENTO FACILIDAD"}, {60, "EN TERMINO"},
        {61, "NOTIFICACION MANDAMIENTO PAGO"}, {62, "TERMINADO"}}

    Public Function RestarDiasHabiles(ByVal FecIni As Date, ByVal FecFin As Date) As Integer
        'Declarar variables
        Dim DiasCalendario, DiasFinSemana, DiasFestivos, DiasHabiles As Integer

        'Asignar valores
        DiasCalendario = DateDiff(DateInterval.Day, FecIni, FecFin)
        DiasFinSemana = ContarSabadosyDomingos(FecIni, FecFin)
        DiasFestivos = ContarFestivos(FecIni, FecFin)

        'Calculo. Version 1
        DiasHabiles = DiasCalendario - DiasFinSemana - DiasFestivos

        Return DiasHabiles

    End Function

    Public Function ContarSabadosyDomingos(ByVal FecIni As Date, ByVal Fecfin As Date) As Integer
        Dim Cant As Integer = 0

        If FecIni >= Fecfin Then
            Return Cant
        End If

        While FecIni <> Fecfin
            If (Weekday(FecIni) = 1) Or (Weekday(FecIni) = 7) Then
                Cant = Cant + 1
            End If
            FecIni = DateAdd("D", 1, FecIni)
        End While
        Return Cant
    End Function

    Public Function ContarFestivos(ByVal FecIni As Date, ByVal Fecfin As Date) As Integer
        Dim Cant As Integer = 0
        Dim Adaptador As New SqlDataAdapter("SELECT fecha FROM TDIAS_FESTIVOS", Funciones.CadenaConexion)
        Dim dtFestivos As New DataTable
        Adaptador.Fill(dtFestivos)
        Dim Buscar_Fila() As DataRow

        If FecIni >= Fecfin Then
            Return Cant
        End If

        While FecIni <> Fecfin
            Buscar_Fila = dtFestivos.Select(String.Format("#{0}# = fecha", FecIni.ToString("yyyy-MM-dd")))

            If Buscar_Fila.Length > 0 Then
                If (Weekday(FecIni) = 1) Or (Weekday(FecIni) = 7) Then
                    ' Si es sabado o domingo NO contar, porque ya se incluyo en la formula de ContarSabadosyDomingos
                Else
                    Cant = Cant + 1
                End If
            End If
            FecIni = DateAdd("D", 1, FecIni)
        End While
        Return Cant

    End Function

    'Mostrar los mensajes NO leidos de un usuario
    Public Function MostrarNumMsjNoLeidos(ByVal pCodigoUsuario As String) As Integer
        Dim NumMensajes As Integer = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT COUNT(idunico) AS NumMensajes FROM mensajes WHERE " & _
            " (UsuDestino = '" & pCodigoUsuario & "') AND " & _
            "(leido = 0 OR leido IS NULL)"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NumMensajes = Reader("NumMensajes").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return NumMensajes
    End Function

    'Mostrar total de mensajes
    Public Function MostrarNumMensajes(ByVal pCodigoUsuario As String) As Integer
        Dim NumMensajes As Integer = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT COUNT(idunico) AS NumMensajes FROM mensajes WHERE " & _
            " (UsuOrigen = '" & pCodigoUsuario & "' OR UsuDestino = '" & pCodigoUsuario & "') "

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NumMensajes = Reader("NumMensajes").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return NumMensajes
    End Function

    'Mostrar expedientes vencidos
    Public Function MostrarVencidos(ByVal pCodigoUsuario As String) As Integer
        Dim NumExpedientes As Integer = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "WITH tmpVencimientos AS ( " & _
            "SELECT EJEFISGLOBAL.EFINROEXP, " & _
            "TERMINO = " & _
            "	CASE WHEN ESTADOS_PROCESO.nombre <> 'PERSUASIVO' THEN 'OK: OTRA ETAPA' " & _
            "	ELSE " & _
            "		CASE WHEN FecEstiFin > GETDATE() THEN 'POR VENCER' " & _
            "		ELSE 'VENCIDO' " & _
            "		END " & _
            "	END " & _
            "FROM EJEFISGLOBAL " & _
            "LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " & _
            "LEFT JOIN PERSUASIVO ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp " & _
            "WHERE EJEFISGLOBAL.EFIUSUASIG = '" & pCodigoUsuario & "') " & _
            "SELECT COUNT(tmpVencimientos.EFINROEXP) AS NumVencidos " & _
            "FROM tmpVencimientos WHERE  tmpVencimientos.termino = 'VENCIDO' " & _
            "GROUP BY tmpVencimientos.termino"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NumExpedientes = Reader("NumVencidos").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return NumExpedientes
    End Function

    Public Function MostrarxVencer(ByVal pCodigoUsuario As String) As Integer
        Dim NumExpedientes As Integer = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "WITH tmpVencimientos AS ( " & _
            "SELECT EJEFISGLOBAL.EFINROEXP, " & _
            "TERMINO = " & _
            "	CASE WHEN ESTADOS_PROCESO.nombre <> 'PERSUASIVO' THEN 'OK: OTRA ETAPA' " & _
            "	ELSE " & _
            "		CASE WHEN FecEstiFin > GETDATE() THEN 'POR VENCER' " & _
            "		ELSE 'VENCIDO' " & _
            "		END " & _
            "	END " & _
            "FROM EJEFISGLOBAL " & _
            "LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " & _
            "LEFT JOIN PERSUASIVO ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp " & _
            "WHERE EJEFISGLOBAL.EFIUSUASIG = '" & pCodigoUsuario & "') " & _
            "SELECT COUNT(tmpVencimientos.EFINROEXP) AS NumxVencer " & _
            "FROM tmpVencimientos WHERE  tmpVencimientos.termino = 'POR VENCER' " & _
            "GROUP BY tmpVencimientos.termino"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NumExpedientes = Reader("NumxVencer").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return NumExpedientes
    End Function

    Public Function GetNomPerfil(ByVal pCodPerfil As String) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & pCodPerfil

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()

        Return NomPerfil
    End Function

    'Funcion para convertr un Gridview a DataTable
    Public Function GridviewToDataTable(ByVal gv As GridView) As DataTable
        Dim dt As New DataTable
        For Each col As DataControlField In gv.Columns
            dt.Columns.Add(col.HeaderText)
        Next
        For Each row As GridViewRow In gv.Rows
            Dim nrow As DataRow = dt.NewRow
            Dim z As Integer = 0
            For Each col As DataControlField In gv.Columns
                nrow(z) = HttpUtility.HtmlDecode(row.Cells(z).Text.Replace("&nbsp;", ""))
                z += 1
            Next
            dt.Rows.Add(nrow)
        Next
        Return dt
    End Function

    'Exportar Dataset a Excel
    Public Sub ExportDataSetToExcel(ByVal ds As DataSet, ByVal filename As String)
        '/---------------------------------------------------------------------------  
        'Incidente  :  222523
        'Nombre HU  :  REPORTE 040 - REPORTE TRAZABILIDAD DE EXPEDIENTES. 
        'Empresa    : UT TECHNOLOGY 
        'Autor      : Jeisson Gómez 
        'Fecha      : 13-01-2018 
        'Objetivo : Para poder visualizar correctamente los datos del reporte.
        ' Se agrega un espacio en html, luego de intentar con los formatos 
        ' mso-number-format:"\@"
        ' mso-number-format:"0" 
        ' REPORTE: 40. REPORTE TRAZABILIDAD EXPEDIENTES
        ' Sin obtener los resultados correctamente en la columna EXPEDIENTE ORIGEN
        '----------------------------------------------------------------------------/  
        '/--------------------------------------------------------------------------  
        'Incidente  :  225262
        'Nombre HU  :  REPORTE 001 - 01. GESTOR INFORMACIÓN GENERAL Y REPARTO
        'Empresa    : UT TECHNOLOGY 
        'Autor      : Jeisson Gómez 
        'Fecha      : 28-01-2018 
        'Objetivo : Para poder visualizar correctamente los datos del reporte.
        ' Se agrega un espacio en html, luego de intentar con los formatos 
        ' mso-number-format:"\@"
        ' mso-number-format:"0" 
        ' REPORTE: 01. GESTOR INFORMACIÓN GENERAL Y REPARTO
        ' Sin obtener los resultados correctamente en la columna No. Expediente origen
        '----------------------------------------------------------------------------/  
        Dim response As HttpResponse = HttpContext.Current.Response
        Dim strSpace As String = "&nbsp;"

        Dim gv As New GridView()
        gv.DataSource = ds.Tables(0)
        gv.DataBind()

        For Each gvRow As GridViewRow In gv.Rows
            For IntCell As Integer = 0 To gvRow.Cells.Count - 1
                If gv.HeaderRow.Cells(IntCell).Text = "EXPEDIENTE ORIGEN" Or gv.HeaderRow.Cells(IntCell).Text = "No. Expediente origen" Then
                    gvRow.Cells(IntCell).Text = strSpace + gvRow.Cells(IntCell).Text
                End If
            Next
        Next

        'Clean response object
        response.Clear()
        response.Charset = "UTF-8"

        'Set response header
        response.ContentType = "application/vnd.ms-excel"
        response.AddHeader("Content-Disposition", "attachment;filename=""" & filename & """")
        ''        
        'response.ContentEncoding = System.Text.Encoding.Unicode
        'response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())

        'Create StringWriter and use to create CSV
        Using sw As New StringWriter()
            Using htw As New HtmlTextWriter(sw)
                'Instantiate DataGrid
                'Dim dg As New DataGrid()
                'dg.DataSource = ds.Tables(0)
                'dg.DataBind()
                'dg.RenderControl(htw)
                gv.RenderControl(htw)
                response.Write(sw.ToString())
                response.[End]()
            End Using
        End Using
    End Sub

    Public Sub ExportDataSetToExcelActuaciones(ByVal ds As DataSet, ByVal filename As String)

        '/-----------------------------------------------------------------  
        'ID _HU:  009 
        'Nombre HU : REPORTE GENERAL DE ACTUACIONES
        'Empresa: UT TECHNOLOGY 
        'Autor: Jeisson Gómez 
        'Fecha: 15-06-2017 
        'Objetivo : Para poder visualizar correctamente los datos del reporte.
        ' Se agrega un espacio en html, luego de intentar con los formatos 
        ' mso-number-format:"\@"
        ' mso-number-format:"0" 
        ' Sin obtener los resultados correctamente en las columnas 
        ' EXPEDIENTE_O_CARPETA_DOCUMENTIC y RADICACION_EXCEPCIONES
        '------------------------------------------------------------------/  

        Dim response As HttpResponse = HttpContext.Current.Response
        Dim strSpace As String = "&nbsp;"


        Dim gv As New GridView()
        gv.DataSource = ds.Tables(0)
        gv.DataBind()

        For Each gvRow As GridViewRow In gv.Rows
            For IntCell As Integer = 0 To gvRow.Cells.Count - 1
                If gv.HeaderRow.Cells(IntCell).Text = "EXPEDIENTE_O_CARPETA_DOCUMENTIC" Or gv.HeaderRow.Cells(IntCell).Text = "RADICACION_EXCEPCIONES" Then
                    gvRow.Cells(IntCell).Text = strSpace + gvRow.Cells(IntCell).Text
                End If
            Next
        Next

        response.Clear()
        response.Buffer = True
        response.Charset = String.Empty
        response.ContentType = "application/vnd.ms-excel"
        response.AddHeader("Content-Disposition", "attachment;filename=""" & filename & """")
        response.ContentEncoding = System.Text.Encoding.Unicode
        response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())

        Using sw As New StringWriter()
            Using htw As New HtmlTextWriter(sw)
                gv.RenderControl(htw)
                response.Write(sw.ToString())
                response.Flush()
                response.[End]()
            End Using
        End Using

    End Sub

    Public Function GetEstadoExpediente(ByVal pExpediente As String) As String
        Dim EstadoExp As String = ""
        NomEstadoProceso = "" ' Esta es una variable publica. Se declara en ModuloVariables.vb

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        'Dim sql As String = "SELECT EFIESTADO FROM EJEFISGLOBAL WHERE EFINROEXP = '" & pExpediente & "'"
        Dim sql As String = "SELECT EJEFISGLOBAL.EFIESTADO, ESTADOS_PROCESO.nombre AS NomEstado " & _
                            "FROM EJEFISGLOBAL " & _
                            "	LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " & _
                            "WHERE EJEFISGLOBAL.EFINROEXP = '" & pExpediente.Trim & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            EstadoExp = Reader("EFIESTADO").ToString().Trim
            NomEstadoProceso = Reader("NomEstado").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()

        'Devolver el estado del expediente
        Return EstadoExp
    End Function

    Public Function GetSuperior(ByVal pUsuario As String, Optional ByRef pNomSuperior As String = "")
        Dim superior As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT codigo, nombre FROM usuarios WHERE codigo = (SELECT superior FROM usuarios WHERE codigo = @codigo)"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@codigo", pUsuario.Trim)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            superior = Reader("codigo").ToString()
            pNomSuperior = Reader("nombre").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return superior
    End Function


    Public Function IsSessionTimedOut() As Boolean
        Dim ctx As HttpContext = HttpContext.Current
        If ctx Is Nothing Then
            Throw New Exception("Este método sólo se puede usar en una aplicación Web")
        End If

        'Comprobamos que haya sesión en primer lugar 
        '(por ejemplo si por ejemplo EnableSessionState=false)
        If ctx.Session Is Nothing Then
            Return False
        End If
        'Si no hay sesión, no puede caducar
        'Se comprueba si se ha generado una nueva sesión en esta petición
        If Not ctx.Session.IsNewSession Then
            Return False
        End If
        'Si no es una nueva sesión es que no ha caducado
        Dim objCookie As HttpCookie = ctx.Request.Cookies("ASP.NET_SessionId")
        'Esto en teoría es imposible que pase porque si hay una 
        'nueva sesión debería existir la cookie, pero lo compruebo porque
        'IsNewSession puede dar True sin ser cierto (más en el post)
        If objCookie Is Nothing Then
            Return False
        End If

        'Si hay un valor en la cookie es que hay un valor de sesión previo, pero como la sesión 
        'es nueva no debería estar, por lo que deducimos que la sesión anterior ha caducado
        If Not String.IsNullOrEmpty(objCookie.Value) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetDtAlarmas() As DataTable
        'Jeisson Gómez 
        'Se realizar el cambio con el fin que se obtenga el nombre de la tabla ALARMAS 
        'Dim Adaptador As New SqlDataAdapter("SELECT codigo, textoactuarpronto, textosinaccion, diasactuarprontoINI, diasactuarprontoFIN, diassinaccionINI, diassinaccionFIN, diasescalamiento FROM ALARMAS", Funciones.CadenaConexion)
        'Jeisson Gómez  HU_003 Alarmas
        '19/05/2017
        ' Se cambia la forma de como se llena el DataTable 
        'Dim Adaptador As New SqlDataAdapter("SELECT codigo, nombre, textoactuarpronto, textosinaccion, diasactuarprontoINI, diasactuarprontoFIN, diassinaccionINI, diassinaccionFIN, diasescalamiento FROM ALARMAS", Funciones.CadenaConexion)
        'Dim dtAlarm As New DataTable
        'Adaptador.Fill(dtAlarm)
        'Return dtAlarm
        Dim strSql As String = "SELECT codigo, nombre, textoactuarpronto, textosinaccion, diasactuarprontoINI, diasactuarprontoFIN, diassinaccionINI, diassinaccionFIN, diasescalamiento FROM ALARMAS"
        Dim dtAlarm As New DataTable
        Using cnn As New SqlConnection(Funciones.CadenaConexion)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dtAlarm)
            End Using
            cnn.Close()
        End Using
        Return dtAlarm

    End Function

    Public Sub AlertarEscalar(ByVal pNumAlarma As Integer, ByVal pNumdias As Integer, ByVal pUsuarioOrigen As String, ByVal pNroExp As String, ByRef pTermino As String, ByRef pExplicacion As String, ByRef pPictureURL As String, ByVal dtAlarmas As DataTable, ByVal pComparador1 As String, ByVal pComparador2 As String, ByRef dtProcs As DataTable, ByVal IntNumReg As Integer)
        Dim FilaAlarma() As DataRow
        FilaAlarma = dtAlarmas.Select("codigo = " & pNumAlarma)

        pTermino = FnRetornarExplicacion(pNumAlarma)


        If FilaAlarma.Length > 0 Then
            Dim DiasActuarProntoIni, DiasActuarProntoFin, DiasSinAccionIni, DiasSinAccionFin, DiasEscalamiento As Integer
            Dim TextoActuarpronto, TextoSinAccion As String
            Dim Condicion12 As Boolean = True

            '' Jeisson Gómez se comenta 
            'DiasActuarProntoIni = FilaAlarma(0)(3)
            'DiasActuarProntoFin = FilaAlarma(0)(4)
            'DiasSinAccionIni = FilaAlarma(0)(5)
            'DiasSinAccionFin = FilaAlarma(0)(6)
            'DiasEscalamiento = FilaAlarma(0)(7)
            '' Jeisson Gómez se comenta 
            'TextoActuarpronto = FilaAlarma(0)(1)
            'TextoSinAccion = FilaAlarma(0)(2)
            '' Jeisson Gómez se comenta 

            DiasActuarProntoIni = FilaAlarma(0).Item("diasactuarprontoINI")
            DiasActuarProntoFin = FilaAlarma(0).Item("diasactuarprontoFIN")
            DiasSinAccionIni = FilaAlarma(0).Item("diassinaccionINI")
            DiasSinAccionFin = FilaAlarma(0).Item("diassinaccionFIN")
            DiasEscalamiento = FilaAlarma(0).Item("diasescalamiento")

            TextoActuarpronto = FilaAlarma(0).Item("textoactuarpronto")
            TextoSinAccion = FilaAlarma(0).Item("textosinaccion")

            ' Si DiasActuarProntoFin = 0, la condicion (2) de la primera comparacion (1) debe ser verdadera
            If DiasActuarProntoFin = 0 Then
                Condicion12 = True
            Else
                Condicion12 = Comparar(pNumdias, DiasActuarProntoFin, " <= ")
            End If


            If Comparar(pNumdias, DiasActuarProntoIni, pComparador1) And Condicion12 Then
                'Jeisson Gómez 
                ' Se cambia a que tome el valor del campo nombre de la tabla ALARMAS 
                ' se comenta la línea pTermino = "AE" & pNumAlarma.ToString.Trim & ". ACTUAR PRONTO"
                ' Jeisson Gómez 
                ' Se revierte la HU_003 ALARMAS 09/03/2017 
                ' pTermino = dtAlarmas.Rows(0).Item("nombre") 
                ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                ' Se cambia por el valor entregado por la UGPP 
                'pTermino = "AE" & pNumAlarma.ToString.Trim & ". ACTUAR PRONTO"
                ' Jeisson Gómez 26/04/2017
                ' Se agrega el parámetro dtRow para hacer que funcione el filtro. 
                dtProcs.Rows(IntNumReg).Item("termino") = pTermino
                'dtProcs.AcceptChanges()
                pTermino = pTermino
                pExplicacion = TextoActuarpronto
                pPictureURL = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")

            ElseIf Comparar(pNumdias, DiasSinAccionIni, pComparador2) Then
                'Jeisson Gómez 
                ' Se cambia a que tome el valor del campo nombre de la tabla ALARMAS 
                ' se comenta la línea pTermino = "AE" & pNumAlarma.ToString.Trim & ". SIN ACCION"
                ' Jeisson Gómez 
                ' Se revierte la HU_003 ALARMAS 09/03/2017 
                ' pTermino = dtAlarmas.Rows(0).Item("nombre")
                ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                ' Se cambia por el valor entregado por la UGPP 
                ' pTermino = "AE" & pNumAlarma.ToString.Trim & ". SIN ACCION"
                ' Jeisson Gómez 26/04/2017
                ' Se agrega el parámetro dtRow para hacer que funcione el filtro. 
                dtProcs.Rows(IntNumReg).Item("termino") = pTermino
                'dtProcs.AcceptChanges()
                pTermino = pTermino
                pExplicacion = TextoSinAccion
                pPictureURL = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

            Else
                'Jeisson Gómez 
                ' Se cambia a que tome el valor del campo nombre de la tabla ALARMAS 
                ' se comenta la línea pTermino = "AE" & pNumAlarma.ToString.Trim & ". EN TERMINO"
                ' Jeisson Gómez 
                ' Se revierte la HU_003 ALARMAS 09/03/2017 
                ' pTermino = dtAlarmas.Rows(0).Item("nombre")
                ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                ' Se cambia porSe cambia por el valor entregado por la UGPP 
                ' pTermino = "AE" & pNumAlarma.ToString.Trim & ". EN TERMINO"
                ' Jeisson Gómez 26/04/2017
                ' Se agrega el parámetro dtRow para hacer que funcione el filtro. 
                dtProcs.Rows(IntNumReg).Item("termino") = pTermino
                'dtProcs.AcceptChanges()
                pTermino = "EN TERMINO" + " " + FnRetornarExplicacion(pNumAlarma)
                pExplicacion = "AE" & pNumAlarma.ToString.Trim & ". EN TERMINO"
                pPictureURL = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
            End If

            If Comparar(pNumdias, DiasEscalamiento, ">=") Then
                'Registrar escalamiento: envio de mensaje a supervisor / revisor
                RegistrarEscalamiento(pNroExp, pNumAlarma, pUsuarioOrigen)
            End If

        End If

    End Sub

    Public Function Comparar(ByVal pValor1 As Integer, ByVal pValor2 As Integer, ByVal pComparador As String) As Boolean
        Dim respuesta As Boolean = False

        If pComparador = "=" Then
            If pValor1 = pValor2 Then
                respuesta = True
            End If

        ElseIf pComparador = ">" Then
            If pValor1 > pValor2 Then
                respuesta = True
            End If

        ElseIf pComparador = ">=" Then
            If pValor1 >= pValor2 Then
                respuesta = True
            End If

        ElseIf pComparador = "<=" Then
            If pValor1 <= pValor2 Then
                respuesta = True
            End If

        ElseIf pComparador = "<" Then
            If pValor1 < pValor2 Then
                respuesta = True
            End If

        End If

        '
        Return respuesta
    End Function

    ' 17/feb/2015. Ajustar el valor capital, sumando el campo pagos.ajuste 
    Public Sub AjustarPagos(ByRef dtProcs As DataTable)
        Dim PagoyAjuste As Double = 0

        'Conexion a la base de datos
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        For x = 0 To dtProcs.Rows.Count - 1
            PagoyAjuste = GetPagoyAjuste(dtProcs.Rows(x).Item("EFINROEXP").ToString(), Connection)
            dtProcs.Rows(x).Item("EFIPAGOSCAP") = PagoyAjuste
            Dim EFIVALDEU = dtProcs.Rows(x).Item("EFIVALDEU").ToString()
            dtProcs.Rows(x).Item("EFISALDOCAP") = Convert.ToDouble(If(String.IsNullOrEmpty(EFIVALDEU), 0, EFIVALDEU)) - PagoyAjuste
        Next

        Connection.Close()

    End Sub

    Private Function GetPagoyAjuste(ByVal pExpediente As String, ByVal Connection As SqlConnection) As Double
        Dim PagoyAjuste As Double = 0

        Dim sql2 As String = "Select SUM(COALESCE(pagcapital, 0) + COALESCE(pagAjusteDec1406,0)) As pagcapital " &
                                    "FROM pagos WHERE NroExp = '" & pExpediente.Trim & "' GROUP BY NroExp"

        Dim Command2 As New SqlCommand(sql2, Connection)
        Dim Reader2 As SqlDataReader = Command2.ExecuteReader
        If Reader2.Read Then
            If Reader2("pagcapital").ToString() = "" Then
                'txtPagosCapitalEA.Text = "0"
                PagoyAjuste = 0
            Else
                PagoyAjuste = Convert.ToDouble(Reader2("pagcapital").ToString()).ToString("N0")
            End If
        Else
            PagoyAjuste = 0
        End If
        Reader2.Close()

        Return PagoyAjuste
    End Function

    Public Sub AjustarTerminos(ByRef dtProcs As DataTable, ByVal emisor As String)
        ' -------------------------------------------------------------------------------------------- '
        '- Este es el metodo que se encarga de gestionar las alertas y los escalamientos
        ' -------------------------------------------------------------------------------------------- '

        Dim dtAlarmas As DataTable
        dtAlarmas = GetDtAlarmas()

        Dim x, DiasHabiles, NumAlarma As Integer
        Dim NombreEstado As String = ""
        Dim FecRecepTitulo As DateTime = Date.MinValue 'Fecha de recepcion del titulo ejecutivo
        Dim FechaActual As Date = Date.Now().Date
        Dim Expediente As String = ""
        Dim HaySolCambioE As Boolean = False
        Dim FecSolCambioE As Date = Date.MinValue
        Dim NumTitulo As String = ""

        '28/03/2014. El usuario origen depende el expediente 
        Dim pUsuarioOrigen As String = ""

        ' Instanciar metodos globales
        Dim MTG As New MetodosGlobalesCobro

        For x = 0 To dtProcs.Rows.Count - 1
            Expediente = dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim

            'If Expediente = "80127" Then
            '    Dim textttt As String = ""
            'End If

            pUsuarioOrigen = GetIDGestorResp(Expediente)
            If pUsuarioOrigen = "" Then
                pUsuarioOrigen = GetRepartidor(Expediente)
            End If

            If emisor = "EJEFISGLOBALREPARTIDOR" Then
                NombreEstado = dtProcs.Rows(x).Item("ESTADOS_PROCESOEFIESTADOnombre").ToString()
            Else
                NombreEstado = dtProcs.Rows(x).Item("EFIESTADO").ToString()
            End If

            '2. ALARMA DE CAMBIO DE ESTADO
            NumAlarma = 2
            HaySolCambioE = ExisteSolCambioEstado(Expediente, FecSolCambioE) 'FecSolCambioE Se llena por referencia
            If HaySolCambioE Then
                'Dias Habiles
                DiasHabiles = MTG.RestarDiasHabiles(FecSolCambioE, FechaActual)

                ' 04/feb/2015. Probando metodo de alarmas con base de datos
                'MTG.AlertarEscalar(NumAlarma, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"))

                ' Jeisson Gómez HU_003 26/04/2017
                ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro 
                AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, "=", ">=", dtProcs, x)

                'If DiasHabiles = 4 Then
                '    dtProcs.Rows(x).Item("termino") = "AE2. ACTUAR PRONTO"
                '    dtProcs.Rows(x).Item("explicacion") = "AE2. Este expediente tiene una solicitud de cambio de estado pendiente. Se han contado 4 días aún no se ha efectuado"
                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")

                'ElseIf DiasHabiles >= 5 Then
                '    dtProcs.Rows(x).Item("termino") = "AE2. SIN ACCION"
                '    dtProcs.Rows(x).Item("explicacion") = "AE2. Este expediente tiene una solicitud de cambio de estado pendiente. Se han contado 5 o más días y aún no se ha efectuado"
                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                '    RegistrarEscalamiento(dtProcs.Rows(x).Item("EFINROEXP").ToString(), 2, pUsuarioOrigen)

                'Else
                '    dtProcs.Rows(x).Item("termino") = "AE2. EN TERMINO"
                '    dtProcs.Rows(x).Item("explicacion") = "AE2. EN TERMINO"
                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                'End If

                ' pasar al siguiente registro
                Continue For
            End If

            If NombreEstado = "REPARTO" Then

                '1. ALARMA DE REPARTO. El repartidor tiene 6 días hábiles para repartir (ALARMA ROJA) 
                '   Al quinto día hábil: alarma amarilla 
                NumAlarma = 1

                FecRecepTitulo = dtProcs.Rows(x).Item("EFIFECHAEXP").ToString()
                'Dias Habiles
                DiasHabiles = MTG.RestarDiasHabiles(FecRecepTitulo, FechaActual)

                ' Jeisson Gómez HU_003 26/04/2017
                ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, "=", ">=", dtProcs, x)

                'If DiasHabiles = 5 Then
                '    dtProcs.Rows(x).Item("termino") = "AE1. ACTUAR PRONTO"
                '    dtProcs.Rows(x).Item("explicacion") = "AE1.  Este expediente ya tiene 5 días en estado de REPARTO y aún no tiene un gestor asignado"
                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")

                'ElseIf DiasHabiles >= 6 Then
                '    dtProcs.Rows(x).Item("termino") = "AE1. SIN ACCION"
                '    dtProcs.Rows(x).Item("explicacion") = "AE1. Este expediente ya tiene 6 o más días en estado de REPARTO y aún no tiene un gestor asignado"
                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                '    RegistrarEscalamiento(Expediente, 1, pUsuarioOrigen)

                'Else
                '    dtProcs.Rows(x).Item("termino") = "AE1. EN TERMINO"
                '    dtProcs.Rows(x).Item("explicacion") = "AE1. EN TERMINO"
                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                'End If

                ' Colocar despues aqui las reglas para suspender las alarmas 
                'dtProcs.Rows(x).Item("termino") = "SUSPENDIDO POR ..."
                ' Verificacion de pago o PAGO
                ' Solicitud de facilidad de pago
                ' Excepciones (Verificacion de pruebas)

            ElseIf NombreEstado = "TERMINADO" Then
                'Jeisson Gómez 
                ' Se cambia al valor retornado por el campo nombre de la tabla ALARMAS se comenta 
                ' la línea  dtProcs.Rows(x).Item("termino") = "TERMINADO"
                ' Jeisson Gómez 
                ' Se revierte la HU_003 ALARMAS 09/03/2017 
                ' dtProcs.Rows(x).Item("termino") = dtAlarmas.Rows(0).Item("nombre")
                ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                ' Se cambia por la descripción entregada por la UGPP 
                ' Se deja igual dtProcs.Rows(x).Item("termino") = "TERMINADO".
                dtProcs.Rows(x).Item("termino") = "TERMINADO"
                dtProcs.Rows(x).Item("explicacion") = "TERMINADO"
                dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/accept.png")


            ElseIf NombreEstado = "PERSUASIVO" Then

                Dim FechaEntregaPersuasivo As Date

                If dtProcs.Rows(x).Item("EFIFECENTGES").ToString() <> "" Then
                    Dim DeudaExpediente As Double = 0

                    '25/sep/2014. Correccion para nulos
                    If dtProcs.Rows(x).Item("EFIVALDEU").ToString() = Nothing Then
                        DeudaExpediente = 0
                    Else
                        DeudaExpediente = dtProcs.Rows(x).Item("EFIVALDEU").ToString()
                    End If

                    FechaEntregaPersuasivo = dtProcs.Rows(x).Item("EFIFECENTGES").ToString()

                    ' 3. PERSUASIVO - ALARMA DE DILIGENCIMIENTO DE LA OBLIGACION (DEUDA) DEL EXPEDIENTE
                    NumAlarma = 3

                    If DeudaExpediente = 0 Then
                        'Dias Habiles
                        DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaPersuasivo, FechaActual)

                        ' Jeisson Gómez HU_003 26/04/2017
                        ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                        AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, "=", ">=", dtProcs, x)

                        'If DiasHabiles = 3 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE3. ACTUAR PRONTO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE3. Contando 3 días y el expediente aún NO tiene diligenciada la información de la obligación por parte del gestor de la etapa persuasiva"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                        '    '
                        '    Continue For
                        'ElseIf DiasHabiles >= 4 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE3. SIN ACCION"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE3. Contando 4 o más días y el expediente aún NO tiene diligenciada la información de la obligación por parte del gestor de la etapa persuasiva"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                        '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                        '    RegistrarEscalamiento(Expediente, 3, pUsuarioOrigen)
                        '    '
                        '    Continue For
                        'Else
                        '    dtProcs.Rows(x).Item("termino") = "AE3. EN TERMINO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE3. EN TERMINO"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        'End If
                    Else
                        'Si llega aca es porque ya el gestor diligenció la información de la oblicación del titulo ejecutivo

                        ' 4. PERSUASIVO - REMISION BASE CONSOLIDADA AL CALL CENTER 
                        NumAlarma = 4

                        Dim FechaEnvioCallC As Date
                        FechaEnvioCallC = GetFechaEnvioCasoCallCenter(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                        If FechaEnvioCallC = Nothing Then
                            'DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaPersuasivo, FechaActual)
                            Dim FechaEditTitulo As Date = Nothing 'Fecha de diligenciamiento de obligacion

                            NumTitulo = ""
                            NumTitulo = GetNumTitulo(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                            If NumTitulo <> "" Then
                                FechaEditTitulo = GetFechaEditTitulo(NumTitulo)
                            End If

                            If FechaEditTitulo = Nothing Then
                                DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaPersuasivo, FechaActual)
                            Else
                                DiasHabiles = MTG.RestarDiasHabiles(FechaEditTitulo, FechaActual)
                            End If

                            ' Jeisson Gómez HU_003 26/04/2017
                            ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                            AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, "=", ">=", dtProcs, x)

                            'If DiasHabiles = 4 Then
                            '    dtProcs.Rows(x).Item("termino") = "AE4. ACTUAR PRONTO"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE4. Contando 4 días y aún NO se ha hecho la remisión de la base consolidada al call center"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                            '    '
                            '    Continue For
                            'ElseIf DiasHabiles >= 5 Then
                            '    dtProcs.Rows(x).Item("termino") = "AE4. SIN ACCION"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE4. Contando 4 o más días y aún NO se ha hecho la remisión de la base consolidada al call center"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                            '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                            '    RegistrarEscalamiento(Expediente, 4, pUsuarioOrigen)
                            '    '
                            '    Continue For
                            'Else
                            '    dtProcs.Rows(x).Item("termino") = "AE4. EN TERMINO"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE4. EN TERMINO"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                            'End If
                        End If
                    End If

                    ' 5. PERSUASIVO - ALARMA DE PRIMER OFICIO DE COBRO  
                    NumAlarma = 5

                    Dim FechaPrimerPersuasivo As Date
                    FechaPrimerPersuasivo = GetFechaPrimerPersuasivo(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                    DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaPersuasivo, FechaActual)

                    If FechaPrimerPersuasivo = Nothing Then

                        ' Jeisson Gómez HU_003 26/04/2017
                        ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                        AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                        'If DiasHabiles >= 10 And DiasHabiles <= 12 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE5. ACTUAR PRONTO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE5. Contando 10 A 12 días y aún NO se ha enviado el primer oficio de cobro persuasivo"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                        '    Continue For
                        'ElseIf DiasHabiles >= 13 Then ' And DiasHabiles <= 15
                        '    dtProcs.Rows(x).Item("termino") = "AE5. SIN ACCION"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE5. Contando 13 o más días y aún NO se ha enviado el primer oficio de cobro persuasivo"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                        '    If DiasHabiles >= 15 Then
                        '        'Registrar escalamiento: envio de mensaje a supervisor / revisor
                        '        RegistrarEscalamiento(Expediente, 5, pUsuarioOrigen)
                        '    End If

                        '    Continue For
                        'Else
                        '    'dtProcs.Rows(x).Item("termino") = "EN TERMINO"
                        '    'dtProcs.Rows(x).Item("explicacion") = "EN TERMINO"
                        '    'dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        'End If
                    Else
                        'Fecha primer persuasivo tiene un dato => validar segundo persuasivo

                        ' 6. PERSUASIVO - ALARMA DE SEGUNDO OFICIO DE COBRO
                        NumAlarma = 6

                        Dim FechaSegundoPersuasivo As Date
                        FechaSegundoPersuasivo = GetFechaSegundoPersuasivo(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                        If FechaSegundoPersuasivo = Nothing Then

                            ' Jeisson Gómez HU_003 26/04/2017
                            ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                            AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                            'If DiasHabiles >= 20 And DiasHabiles <= 22 Then
                            '    dtProcs.Rows(x).Item("termino") = "AE6. ACTUAR PRONTO"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE6. Contando 20 A 22 días y aún NO se ha enviado el segundo oficio de cobro persuasivo"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")

                            '    Continue For
                            'ElseIf DiasHabiles >= 23 Then 'And DiasHabiles <= 25
                            '    dtProcs.Rows(x).Item("termino") = "AE6. SIN ACCION"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE6. Contando 13 o más días y aún NO se ha enviado el segundo oficio de cobro persuasivo"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                            '    If DiasHabiles >= 25 Then
                            '        'Registrar escalamiento: envio de mensaje a supervisor / revisor
                            '        RegistrarEscalamiento(Expediente, 6, pUsuarioOrigen)
                            '    End If

                            '    Continue For
                            'Else
                            '    dtProcs.Rows(x).Item("termino") = "AE6. EN TERMINO"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE6. EN TERMINO"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                            'End If
                        Else
                            'Jeisson Gómez 
                            ' Se cambia al valor retornado por el campo nombre de la tabla ALARMAS se comenta 
                            ' la línea dtProcs.Rows(x).Item("termino") = "AE6. EN TERMINO"
                            ' Jeisson Gómez 
                            ' Se revierte la HU_003 ALARMAS 09/03/2017 
                            ' dtProcs.Rows(x).Item("termino") = dtAlarmas.Rows(0).Item("nombre")
                            ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                            ' Se cambia por la descripción entregada por la UGPP 
                            ' Se cambia dtProcs.Rows(x).Item("termino") = "AE6. EN TERMINO" por 
                            ' dtProcs.Rows(x).Item("termino") = "EN TERMINO"
                            dtProcs.Rows(x).Item("termino") = "EN TERMINO SEGUNDO OFICIO"
                            dtProcs.Rows(x).Item("explicacion") = "AE6. EN TERMINO"
                            dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        End If
                    End If

                    ' 7. PERSUASIVO - ALARMA DE MEMORANDO DE CAMBIO DE ESTADO (LA ETAPA PERSUASIVA TIENE UN TIEMPO)
                    NumAlarma = 7

                    HaySolCambioE = ExisteSolCambioEstado2(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, "06") '"06" = "ESTADO PERSUASIVO"
                    If Not HaySolCambioE Then
                        DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaPersuasivo, FechaActual)

                        ' Jeisson Gómez HU_003 26/04/2017
                        ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                        AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                        'If DiasHabiles >= 30 And DiasHabiles <= 32 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE7. ACTUAR PRONTO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE7. Contando 30 A 32 días y aún NO existe la solicitud de cambio de estado"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")

                        '    Continue For
                        'ElseIf DiasHabiles >= 33 Then ' And DiasHabiles <= 35
                        '    dtProcs.Rows(x).Item("termino") = "AE7. SIN ACCION"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE7. Contando 33 a 35 días y aún NO existe la solicitud de cambio de estado"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                        '    If DiasHabiles >= 35 Then
                        '        'Registrar escalamiento: envio de mensaje a supervisor / revisor
                        '        RegistrarEscalamiento(Expediente, 7, pUsuarioOrigen)
                        '    End If

                        '    Continue For
                        'Else
                        '    dtProcs.Rows(x).Item("termino") = "AE7. EN TERMINO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE7. EN TERMINO"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        'End If

                    End If
                End If

            ElseIf NombreEstado = "COACTIVO" Then
                ' 8. COACTIVO - ALARMA PARA PROFERIR MANDAMIENTO DE PAGO 
                NumAlarma = 8

                Dim FechaEntregaCoactivo As Date
                Dim ExisteMP As Boolean = False
                FechaEntregaCoactivo = dtProcs.Rows(x).Item("EFIFECENTGES").ToString()
                ExisteMP = ExisteMandamientoPago(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                Dim ExisteResOrdEjec As Boolean = False
                ExisteResOrdEjec = ExisteResolOrdenaEjec(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)

                If Not ExisteMP Then
                    DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaCoactivo, FechaActual)

                    ' Jeisson Gómez HU_003 26/04/2017
                    ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                    AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, "=", ">=", dtProcs, x)

                    'If DiasHabiles = 1 Then
                    '    dtProcs.Rows(x).Item("termino") = "AE8. ACTUAR PRONTO"
                    '    dtProcs.Rows(x).Item("explicacion") = "AE8. Contando 1 día y aún NO se ha proferido el Mandamiento de pago"
                    '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")

                    '    Continue For
                    'ElseIf DiasHabiles >= 2 Then
                    '    dtProcs.Rows(x).Item("termino") = "AE8. SIN ACCION"
                    '    dtProcs.Rows(x).Item("explicacion") = "AE8. Contando 2 o más días y aún NO se ha proferido el Mandamiento de pago"
                    '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")

                    '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                    '    RegistrarEscalamiento(Expediente, 8, pUsuarioOrigen)

                    '    Continue For
                    'Else
                    '    dtProcs.Rows(x).Item("termino") = "AE8. EN TERMINO"
                    '    dtProcs.Rows(x).Item("explicacion") = "AE8. EN TERMINO"
                    '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                    'End If

                Else
                    'EXISTE Mandamiento de Pago (ExisteMP)
                    ' 9. COACTIVO - ALARMA PARA RESOLUCIÓN QUE ORDENA SEGUIR ADELANTE CON LA EJECUCIÓN
                    NumAlarma = 9

                    Dim FechaNotifMP As Date = Nothing
                    FechaNotifMP = GetFechaNotificacionMP(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                    If FechaNotifMP = Nothing Then
                        'Inconsistencia en el proceso: OJO NO han notificado por ningun metodo al deudor. NO SE GENERA ALARMA
                        'Jeisson Gómez 
                        ' Se cambia al valor retornado por el campo nombre de la tabla ALARMAS se comenta 
                        ' la línea dtProcs.Rows(x).Item("termino") = "AE9."
                        ' Jeisson Gómez 
                        ' Se revierte la HU_003 ALARMAS 09/03/2017 
                        ' dtProcs.Rows(x).Item("termino") = dtAlarmas.Rows(0).Item("nombre")
                        ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                        ' Se cambia dtProcs.Rows(x).Item("termino") = "AE9." por dtProcs.Rows(x).Item("termino") = "NOTIFICACION MANDAMIENTO PAGO"
                        dtProcs.Rows(x).Item("termino") = "NOTIFICACION MANDAMIENTO PAGO"
                        dtProcs.Rows(x).Item("explicacion") = "AE9. NO han notificado por ningún método al deudor del Mandamiento de Pago"
                        dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                    Else
                        Dim DiaSigNotificacionMP As Date
                        DiaSigNotificacionMP = FechaNotifMP.AddDays(1)

                        'Si no existe la resolucion que ordena llevar adelante la ejecucion => Alarma 9
                        If Not ExisteResOrdEjec Then
                            DiasHabiles = MTG.RestarDiasHabiles(DiaSigNotificacionMP, FechaActual)

                            ' Jeisson Gómez HU_003 26/04/2017
                            ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                            AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                            'If DiasHabiles >= 15 Then
                            '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                            '    RegistrarEscalamiento(Expediente, 9, pUsuarioOrigen)
                            'End If
                            'If DiasHabiles >= 15 And DiasHabiles <= 18 Then
                            '    If DiasHabiles >= 16 And DiasHabiles <= 18 Then
                            '        dtProcs.Rows(x).Item("termino") = "AE9. ACTUAR PRONTO"
                            '        dtProcs.Rows(x).Item("explicacion") = "AE9. Contando 16 A 18 días y aún NO se ha generado la Resolución que ordena llevar adelante la ejecución"
                            '        dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                            '    End If
                            '    Continue For

                            'ElseIf DiasHabiles >= 19 Then 'And DiasHabiles <= 21 Then
                            '    dtProcs.Rows(x).Item("termino") = "AE9. SIN ACCION"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE9. Contando 19 o más días y aún NO se ha generado la Resolución que ordena llevar adelante la ejecución"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                            '    Continue For
                            'Else
                            '    dtProcs.Rows(x).Item("termino") = "AE9. EN TERMINO"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE9. EN TERMINO"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                            'End If

                        Else
                            'Existe la Resolucion que Ordena llevar adelante la Ejecucion
                            'Jeisson Gómez 
                            ' Se cambia al valor retornado por el campo nombre de la tabla ALARMAS se comenta 
                            ' la línea dtProcs.Rows(x).Item("termino") = "AE9. EN TERMINO"
                            ' Se revierte la HU_003 06/03/2017 Jeisson Gómez
                            ' dtProcs.Rows(x).Item("termino") = dtAlarmas.Rows(0).Item("nombre")
                            ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                            ' Se cambia dtProcs.Rows(x).Item("termino") = "AE9. EN TERMINO" por dtProcs.Rows(x).Item("termino") = "EN TERMINO"
                            dtProcs.Rows(x).Item("termino") = "EN TERMINO ORDEN DE EJECUCION"
                            dtProcs.Rows(x).Item("explicacion") = "AE9. EN TERMINO"
                            dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        End If

                    End If

                End If

                ' 14. COACTIVO - RESOLUCIÓN QUE RESUELVE EXCEPCIONES
                NumAlarma = 14

                Dim FechaRadicExcepciones As Date = Nothing
                Dim NroResResuelve As String = ""
                FechaRadicExcepciones = GetFechaRadicExcepciones(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, NroResResuelve)
                Dim DiasCalendario As Integer

                If FechaRadicExcepciones <> Nothing Then
                    ' Si hay una fecha de radicacion de excepciones, debe existir la reolucion que la resuelve
                    If NroResResuelve = "" Then
                        ' Si entra aca es porque no existe la resolucion que resuelve
                        Dim DiaSigRadicExcepciones As Date = Nothing

                        DiaSigRadicExcepciones = FechaRadicExcepciones.AddDays(1)
                        DiasCalendario = DateDiff(DateInterval.Day, DiaSigRadicExcepciones, Date.Now)

                        ' Jeisson Gómez HU_003 26/04/2017
                        ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                        AlertarEscalar(NumAlarma, DiasCalendario, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                        'If DiasCalendario >= 20 And DiasCalendario <= 25 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE14. ACTUAR PRONTO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE14. Contando 20 A 25 días y aún NO se ha generado la Resolución que resuelve excepciones"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                        '    '
                        '    Continue For

                        'ElseIf DiasHabiles >= 26 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE14. SIN ACCION"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE14. Contando 26 o más días y aún NO se ha generado la Resolución que resuelve excepciones"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                        '    If DiasHabiles >= 30 Then
                        '        RegistrarEscalamiento(Expediente, 14, pUsuarioOrigen)
                        '    End If
                        '    Continue For
                        'Else
                        '    dtProcs.Rows(x).Item("termino") = "AE14. EN TERMINO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE14. EN TERMINO"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        'End If

                    Else
                        'Si entra aca es porque hay una resolucion que resuelve excepciones y es posible que exista un recurso contra ella

                        ' 15. COACTIVO - RESOLUCIÓN QUE RESUELVE REPOSICIÓN CONTRA LA RESOLUCIÓN QUE RESUELVE EXCEPCIONES
                        NumAlarma = 15

                        Dim FechaRadicRecursoRepvsResExcep As Date = Nothing 'Fecha de radicacion de recurso de reposicion contra la resolucion que resuelve excepciones
                        Dim RecursoRepvsResExcep As String = ""

                        FechaRadicRecursoRepvsResExcep = GetFechaRadicRecursoRepvsResExcep(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, RecursoRepvsResExcep)
                        'Si esta fecha existe => tiene que existir la resolucion quel a resuelve
                        If FechaRadicRecursoRepvsResExcep <> Nothing Then
                            If RecursoRepvsResExcep = "" Then
                                'Si el recurso no existe => contar dias
                                DiasCalendario = DateDiff(DateInterval.Day, FechaRadicRecursoRepvsResExcep, Date.Now)

                                ' Jeisson Gómez HU_003 26/04/2017
                                ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                                AlertarEscalar(NumAlarma, DiasCalendario, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                                'If DiasCalendario >= 20 And DiasCalendario <= 25 Then
                                '    dtProcs.Rows(x).Item("termino") = "AE15. ACTUAR PRONTO"
                                '    dtProcs.Rows(x).Item("explicacion") = "AE15. Contando 20 A 25 días y aún NO se ha generado la Resolución que resuelve reposición contra la Resolución que resuelve excepciones"
                                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                                '    '
                                '    Continue For

                                'ElseIf DiasHabiles >= 26 Then
                                '    dtProcs.Rows(x).Item("termino") = "AE15. SIN ACCION"
                                '    dtProcs.Rows(x).Item("explicacion") = "AE15. Contando 26 o más días y aún NO se ha generado Resolución que resuelve reposición contra la Resolución que resuelve excepciones"
                                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                                '    If DiasHabiles >= 30 Then
                                '        RegistrarEscalamiento(Expediente, 15, pUsuarioOrigen)
                                '    End If
                                '    Continue For
                                'Else
                                '    dtProcs.Rows(x).Item("termino") = "AE15. EN TERMINO"
                                '    dtProcs.Rows(x).Item("explicacion") = "AE15. EN TERMINO"
                                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                                'End If
                            End If

                        End If

                    End If
                End If

                ' 16. COACTIVO - RESOLUCIÓN DE LIQUIDACIÓN DEL CRÉDITO 
                NumAlarma = 16

                If ExisteResOrdEjec Then
                    Dim FechaNotifAdelanteEjec As Date = Nothing
                    Dim NroResLiqCred As String = ""

                    FechaNotifAdelanteEjec = GetFechaNotifOrdenaEjec(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, NroResLiqCred)
                    If FechaNotifAdelanteEjec <> Nothing Then
                        Dim DiaSigNotifAdelanteEjec As Date = Nothing
                        DiaSigNotifAdelanteEjec = FechaNotifAdelanteEjec.AddDays(1)
                        If NroResLiqCred = "" Then
                            'Contar dias habiles
                            DiasHabiles = MTG.RestarDiasHabiles(DiaSigNotifAdelanteEjec, FechaActual)

                            ' Jeisson Gómez HU_003 26/04/2017
                            ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                            AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                            'If DiasHabiles >= 5 And DiasHabiles <= 7 Then
                            '    dtProcs.Rows(x).Item("termino") = "AE16. ACTUAR PRONTO"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE16. Contando 5 A 7 días y aún NO se ha generado la Resolución de Liquidación de Crédito"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                            '    Continue For

                            'ElseIf DiasHabiles >= 8 Then
                            '    dtProcs.Rows(x).Item("termino") = "AE16. SIN ACCION"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE16. Contando 8 o más días y aún NO se ha generado la Resolución de Liquidación de Crédito"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                            '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                            '    If DiasHabiles >= 10 Then
                            '        RegistrarEscalamiento(Expediente, 16, pUsuarioOrigen)
                            '    End If
                            '    Continue For
                            'Else
                            '    dtProcs.Rows(x).Item("termino") = "AE16. EN TERMINO"
                            '    dtProcs.Rows(x).Item("explicacion") = "AE16. EN TERMINO"
                            '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                            'End If

                        End If
                    End If
                End If

                ' 17. COACTIVO - RESOLUCIÓN QUE RESUELVE OBJECIONES CONTRA LA LIQUIDACIÓN DEL CRÉDITO Y APRUEBA LA LIQUIDACIÓN
                NumAlarma = 17

                Dim FechaRadObjContraLiqCredito As Date = Nothing
                Dim NroResApLiq As String = ""
                FechaRadObjContraLiqCredito = GetFechaRadObjContraLiqCredito(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, NroResApLiq)
                If FechaRadObjContraLiqCredito <> Nothing Then
                    If NroResApLiq = "" Then
                        'Contar dias habiles
                        DiasHabiles = MTG.RestarDiasHabiles(FechaRadObjContraLiqCredito, FechaActual)

                        ' Jeisson Gómez HU_003 26/04/2017
                        ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                        AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                        'If DiasHabiles >= 5 And DiasHabiles <= 7 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE17. ACTUAR PRONTO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE17. Contando 5 A 7 días y aún NO se ha generado la Resolución que resuelve objeciones contra la Liquidación del Crédito y aprueba la liquidación"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                        '    Continue For

                        'ElseIf DiasHabiles >= 8 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE17. SIN ACCION"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE17. Contando 8 o más días y aún NO se ha generado la Resolución que resuelve objeciones contra la Liquidación del Crédito y aprueba la liquidación"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                        '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                        '    If DiasHabiles >= 10 Then
                        '        RegistrarEscalamiento(Expediente, 17, pUsuarioOrigen)
                        '    End If
                        '    Continue For
                        'Else
                        '    dtProcs.Rows(x).Item("termino") = "AE17. EN TERMINO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE17. EN TERMINO"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        'End If

                    End If
                End If

                ' 18. COACTIVO - RESOLUCIÓN QUE APRUEBA LIQUIDACIÓN DEL CRÉDITO 
                NumAlarma = 18

                Dim FecNotCorLC As Date = Nothing
                Dim DiasSigFecNotCorLC As Date = Nothing

                FecNotCorLC = GetFecNotCorLC(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, NroResApLiq)
                If FecNotCorLC <> Nothing Then
                    If NroResApLiq = "" Then
                        'Contar dias habiles
                        DiasSigFecNotCorLC = FecNotCorLC.AddDays(3)
                        DiasHabiles = MTG.RestarDiasHabiles(DiasSigFecNotCorLC, FechaActual)

                        ' Jeisson Gómez HU_003 26/04/2017
                        ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                        AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                        'If DiasHabiles >= 5 And DiasHabiles <= 7 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE18. ACTUAR PRONTO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE18. Contando 5 A 7 días y aún NO se ha generado la Resolución que aprueba liquidación del crédito"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                        '    Continue For

                        'ElseIf DiasHabiles >= 8 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE18. SIN ACCION"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE18. Contando 8 o más días y aún NO se ha generado la Resolución que aprueba liquidación del crédito"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                        '    'Registrar escalamiento: envio de mensaje a supervisor / revisor
                        '    If DiasHabiles >= 10 Then
                        '        RegistrarEscalamiento(Expediente, 18, pUsuarioOrigen)
                        '    End If
                        '    Continue For
                        'Else
                        '    dtProcs.Rows(x).Item("termino") = "AE18. EN TERMINO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE18. EN TERMINO"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        'End If

                    End If
                End If

            ElseIf NombreEstado = "CONCURSAL" Then
                Dim FechaEntregaConcursal As Date
                Dim ExisteInfoConsursal As Boolean = False
                FechaEntregaConcursal = dtProcs.Rows(x).Item("EFIFECENTGES").ToString()

                ' 20. CONCURSAL - DILIGENCIAMIENTO DE INFORMACIÓN EN EL GESTOR DE COBRO DE LA OBLIGACIÓN
                NumAlarma = 20

                ExisteInfoConsursal = ExisteInformacionConsursal(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                If Not ExisteInfoConsursal Then
                    'contar dias habiles
                    DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaConcursal, FechaActual)

                    ' Jeisson Gómez HU_003 26/04/2017
                    ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                    AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                    'If DiasHabiles >= 3 And DiasHabiles <= 4 Then
                    '    dtProcs.Rows(x).Item("termino") = "AE20. ACTUAR PRONTO"
                    '    dtProcs.Rows(x).Item("explicacion") = "AE20. Contando 3 A 4 días y aún NO se ha diligenciado la información por parte del gestor asignado al estado consursal"
                    '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                    '    Continue For

                    'ElseIf DiasHabiles >= 5 Then
                    '    dtProcs.Rows(x).Item("termino") = "AE20. SIN ACCION"
                    '    dtProcs.Rows(x).Item("explicacion") = "AE20. Contando 8 o más días y aún NO se ha diligenciado la información por parte del gestor asignado al estado consursal"
                    '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                    '    'Registrar escalamiento: envio de mensaje a supervisor / revisor                        
                    '    RegistrarEscalamiento(Expediente, 20, pUsuarioOrigen)

                    '    Continue For
                    'Else
                    '   dtProcs.Rows(x).Item("termino") = "AE20. EN TERMINO"
                    '   dtProcs.Rows(x).Item("explicacion") = "AE20. EN TERMINO"
                    '   dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                    'End If

                End If

                ' 21. CONCURSAL - PRESENTACIÓN DEL CRÉDITO O REQUERIMIENTO PARA EL PAGO
                NumAlarma = 21

                Dim ExisteOfiPres As Boolean = False
                ExisteOfiPres = ExisteOficioPresentacion(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                If Not ExisteOfiPres Then
                    'contar dias habiles
                    DiasHabiles = MTG.RestarDiasHabiles(FechaEntregaConcursal, FechaActual)

                    ' Jeisson Gómez HU_003 26/04/2017
                    ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                    AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                    'If DiasHabiles >= 5 And DiasHabiles <= 7 Then
                    '    dtProcs.Rows(x).Item("termino") = "AE21. ACTUAR PRONTO"
                    '    dtProcs.Rows(x).Item("explicacion") = "AE21. Contando 5 A 7 días y aún NO se ha diligenciado el oficio de Presentación del crédito o requerimiento para el pago"
                    '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                    '    Continue For

                    'ElseIf DiasHabiles >= 8 Then
                    '    dtProcs.Rows(x).Item("termino") = "AE21. SIN ACCION"
                    '    dtProcs.Rows(x).Item("explicacion") = "AE21. Contando 8 o más días y aún NO se ha diligenciado el oficio de Presentación del crédito o requerimiento para el pago"
                    '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                    '    If DiasHabiles >= 10 Then
                    '        'Registrar escalamiento: envio de mensaje a supervisor / revisor                        
                    '        RegistrarEscalamiento(Expediente, 20, pUsuarioOrigen)
                    '    End If
                    '    Continue For
                    'Else
                    ' dtProcs.Rows(x).Item("termino") = "AE21. EN TERMINO"
                    ' dtProcs.Rows(x).Item("explicacion") = "AE21. EN TERMINO"
                    ' dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                    'End If

                End If

            ElseIf NombreEstado = "FACILIDAD DE PAGO" Then
                ' 26. FACILIDAD DE PAGO - ALARMA DE REQUERIMIENTO POR INCUMPLIMIENTO DE CUOTA
                NumAlarma = 26

                Dim FecCuotaRec As Date = Nothing
                FecCuotaRec = GetFechaCuotaReciente(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                If FecCuotaRec <> Nothing Then
                    Dim FecPagoRec As Date = Nothing
                    FecPagoRec = GetFechaPagoRec(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, FecCuotaRec.ToString("dd/MM/yyyy"))
                    ' Si la fecha de pago de la cuota esta vacia
                    If FecPagoRec = Nothing Then
                        Dim DiaSigIncump As Date
                        DiaSigIncump = FecCuotaRec.AddDays(1)

                        'Machete para hacer pruebas borrar                        
                        'DiaSigIncump = Date.ParseExact("15/06/2013", "dd/MM/yyyy", Nothing)

                        'contar dias habiles
                        DiasHabiles = MTG.RestarDiasHabiles(DiaSigIncump, FechaActual)

                        ' Jeisson Gómez HU_003 26/04/2017
                        ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro
                        AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                        'If DiasHabiles >= 5 And DiasHabiles <= 7 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE26. ACTUAR PRONTO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE26. Contando 5 A 7 días del incumlimiento de la cuota de la facilidad"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                        '    Continue For

                        'ElseIf DiasHabiles >= 8 Then
                        '    dtProcs.Rows(x).Item("termino") = "AE26. SIN ACCION"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE26. Contando 8 o más días del incumplimiento de la cuota de la facilidad"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                        '    If DiasHabiles >= 10 Then
                        '        'Registrar escalamiento: envio de mensaje a supervisor / revisor                        
                        '        RegistrarEscalamiento(Expediente, 26, pUsuarioOrigen)
                        '    End If
                        '    Continue For
                        'Else
                        '    dtProcs.Rows(x).Item("termino") = "AE26. EN TERMINO"
                        '    dtProcs.Rows(x).Item("explicacion") = "AE26. EN TERMINO"
                        '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                        'End If

                    Else
                        'Jeisson Gómez 
                        ' Se cambia al valor retornado por el campo nombre de la tabla ALARMAS se comenta 
                        ' la línea dtProcs.Rows(x).Item("termino") = "AE26. EN TERMINO"
                        ' Se revierte la HU_003 06/03/2017 Jeisson Gómez 
                        ' dtProcs.Rows(x).Item("termino") = dtAlarmas.Rows(0).Item("nombre")
                        ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                        ' Se cambia dtProcs.Rows(x).Item("termino") = "AE26. EN TERMINO" por dtProcs.Rows(x).Item("termino") = "EN TERMINO"
                        dtProcs.Rows(x).Item("termino") = "EN TERMINO INCUMPLIMIENTO FACILIDAD"
                        dtProcs.Rows(x).Item("explicacion") = "AE26. EN TERMINO"
                        dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")

                    End If
                Else
                    'Jeisson Gómez 
                    ' Se cambia al valor retornado por el campo nombre de la tabla ALARMAS se comenta 
                    ' la línea dtProcs.Rows(x).Item("termino") = "AE26. EN TERMINO"
                    ' Se revierte la HU_003 06/03/2017 Jeisson Gómez
                    ' dtProcs.Rows(x).Item("termino") = dtAlarmas.Rows(0).Item("nombre")
                    ' Jeisson Gómez HU_003 ALARMAS 19/04/2017 
                    ' Se cambia dtProcs.Rows(x).Item("termino") = "AE26. EN TERMINO" por dtProcs.Rows(x).Item("termino") = "EN TERMINO"
                    dtProcs.Rows(x).Item("termino") = "EN TERMINO INCUMPLIMIENTO FACILIDAD"
                    dtProcs.Rows(x).Item("explicacion") = "AE26. EN TERMINO"
                    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")

                End If

                ' 29. FACILIDAD DE PAGO - ALARMA DE FECHA DE CUMPLIMIENTO DE LA ÚLTIMA CUOTA DE PAGO
                NumAlarma = 29

                Dim FechaUltimaCuota As Date = Nothing
                FechaUltimaCuota = GetFechaUltimaCuota(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim)
                'MACHETE PARA PRUEBAS
                FechaUltimaCuota = Date.ParseExact("15/06/2013", "dd/MM/yyyy", Nothing)


                If FechaUltimaCuota <> Nothing Then
                    If FechaUltimaCuota > Date.Now Then
                        ' Aun no ha llegado la fecha de la ultima cuota
                    Else
                        ' Si llega aca es porque ya llego o paso por la fecha de la ultima cuota => preguntar si hay dato de la fecha de pago
                        Dim FecPagoFinal As Date = Nothing
                        FecPagoFinal = GetFechaPagoRec(dtProcs.Rows(x).Item("EFINROEXP").ToString().Trim, FechaUltimaCuota.ToString("dd/MM/yyyy"))
                        If FecPagoFinal <> Nothing Then
                            'Hay fecha de pago de la ultima cuota => preguntar si existe la reolucion que declara cumplida la failidad
                            Dim swExisteResFac As Boolean = False
                            swExisteResFac = ExisteResFac(dtProcs.Rows(x).Item("EFINROEXP").ToString())

                            If Not swExisteResFac Then
                                'Si no existe la resolucion de facilidad => realizar conteo de dias habiles
                                DiasHabiles = MTG.RestarDiasHabiles(FecPagoFinal, FechaActual)

                                ' Jeisson Gómez HU_003 26/04/2017
                                ' Se agrega la fila dtProcs.Rows(x) para que funcione el filtro ojo aquí es 
                                AlertarEscalar(NumAlarma, DiasHabiles, pUsuarioOrigen, dtProcs.Rows(x).Item("EFINROEXP").ToString(), dtProcs.Rows(x).Item("termino"), dtProcs.Rows(x).Item("explicacion"), dtProcs.Rows(x).Item("PictureURL"), dtAlarmas, ">=", ">=", dtProcs, x)

                                'If DiasHabiles >= 5 And DiasHabiles <= 7 Then
                                '    dtProcs.Rows(x).Item("termino") = "AE29. ACTUAR PRONTO"
                                '    dtProcs.Rows(x).Item("explicacion") = "AE29. Contando 5 A 7 días y aún NO se ha generado la Resolución que declara cumplida la facilidad de pago"
                                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alert.png")
                                '    Continue For

                                'ElseIf DiasHabiles >= 8 Then
                                '    dtProcs.Rows(x).Item("termino") = "AE29. SIN ACCION"
                                '    dtProcs.Rows(x).Item("explicacion") = "AE29. Contando 8 o más días y aún NO se ha generado la Resolución que declara cumplida la facilidad de pago"
                                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/alarm16x16.gif")
                                '    If DiasHabiles >= 10 Then
                                '        'Registrar escalamiento: envio de mensaje a supervisor / revisor                        
                                '        RegistrarEscalamiento(Expediente, 29, pUsuarioOrigen)
                                '    End If
                                '    Continue For
                                'Else
                                '    dtProcs.Rows(x).Item("termino") = "AE29. EN TERMINO"
                                '    dtProcs.Rows(x).Item("explicacion") = "AE29. EN TERMINO"
                                '    dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
                                'End If
                            End If
                        End If
                    End If
                End If

            Else
                'Por el momento aca se manejan todos los demas estados
                dtProcs.Rows(x).Item("termino") = ""
                dtProcs.Rows(x).Item("explicacion") = ""
                dtProcs.Rows(x).Item("PictureURL") = VirtualPathUtility.ToAbsolute("~/security/images/icons/160.png")
            End If

        Next
    End Sub

    Public Function GetSupervisor()
        Dim supervisor As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT codigo FROM USUARIOS WHERE nivelacces = 2 AND useractivo = 1"
        Dim Command As New SqlCommand(sql, Connection)

        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            supervisor = Reader("codigo").ToString()
        End If
        Reader.Close()
        Connection.Close()

        Return supervisor

    End Function

    Public Sub RegistrarEscalamiento(ByVal pExpediente As String, ByVal pNumAlarma As Integer, ByVal pUsuarioOrigen As String)
        ' Si no se ha enviado mensaje a supervisor / revisor => enviar
        Dim revisor, supervisor As String
        revisor = GetSuperior(pUsuarioOrigen)

        '25/nov/2014.xxxx:: Edicion de linea para hacer lamado a funcion que obtiene el supervisor real
        ' supervisor = "0018" 'Funcion para traer supervisor 
        supervisor = GetSupervisor()

        If pNumAlarma = 1 Then
            If Not FueEnviadoMsg(pExpediente, 1, supervisor) Then
                Dim msg As String = "AE1. Coactivo SyP ha detectado que el expediente " & pExpediente & " se encuentra de REPARTO después de 6 días y aún no tiene un gestor asignado"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, supervisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 2 Then
            If Not FueEnviadoMsg(pExpediente, 2, supervisor) Then
                Dim msg As String = "AE2. Coactivo SyP ha detectado que el expediente " & pExpediente & " tiene una solicitud de cambio de estado pendiente y después de 5 días aún no se ha efectuado"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, supervisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 3 Then
            If Not FueEnviadoMsg(pExpediente, 3, revisor) Then
                Dim msg As String = "AE3. Coactivo SyP ha detectado que el expediente " & pExpediente & " aún NO tiene diligenciada la información de la obligación por parte del gestor de la etapa persuasiva"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 4 Then
            If Not FueEnviadoMsg(pExpediente, 4, supervisor) Then
                Dim msg As String = "AE4. Coactivo SyP ha detectado que el expediente " & pExpediente & " aún NO ha sido remitido al Call Center"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, supervisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 5 Then
            If Not FueEnviadoMsg(pExpediente, 5, revisor) Then
                Dim msg As String = "AE5. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha enviado el primer oficio de cobro persuasivo"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 6 Then
            If Not FueEnviadoMsg(pExpediente, 6, revisor) Then
                Dim msg As String = "AE6. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha enviado el segundo oficio de cobro persuasivo"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 7 Then
            If Not FueEnviadoMsg(pExpediente, 7, supervisor) Then
                Dim msg As String = "AE7. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha enviado el segundo oficio de cobro persuasivo"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, supervisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 8 Then
            If Not FueEnviadoMsg(pExpediente, 8, revisor) Then
                Dim msg As String = "AE8. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha proferido el Mandamiento de pago"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 9 Then
            If Not FueEnviadoMsg(pExpediente, 9, revisor) Then
                Dim msg As String = "AE9. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha proferido la Resolución que ordena llevar adelante la ejecución"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 14 Then
            If Not FueEnviadoMsg(pExpediente, 14, revisor) Then
                Dim msg As String = "AE14. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha proferido la Resolución que resuelve excepciones"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 15 Then
            If Not FueEnviadoMsg(pExpediente, 15, revisor) Then
                Dim msg As String = "AE15. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha proferido la Resolución que resuelve reposición contra la Resolución que resuelve excepciones"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 16 Then
            'Resolución de Liquidación de Crédito
            If Not FueEnviadoMsg(pExpediente, 16, revisor) Then
                Dim msg As String = "AE16. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha proferido la Resolución de Liquidación de Crédito"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 17 Then
            'Resolución que resuelve objeciones contra la Liquidación del Crédito y aprueba la liquidación
            If Not FueEnviadoMsg(pExpediente, 17, revisor) Then
                Dim msg As String = "AE17. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha proferido la Resolución que resuelve objeciones contra la Liquidación del Crédito y aprueba la liquidación"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 18 Then
            'Resolución que resuelve objeciones contra la Liquidación del Crédito y aprueba la liquidación
            If Not FueEnviadoMsg(pExpediente, 18, revisor) Then
                Dim msg As String = "AE18. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha proferido la Resolución que aprueba liquidación del crédito"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 20 Then
            'NO se ha diligenciado la información por parte del gestor asignado al estado consursal
            If Not FueEnviadoMsg(pExpediente, 20, revisor) Then
                Dim msg As String = "AE20. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha diligenciado la información por parte del gestor asignado al estado consursal"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 21 Then
            'NO se ha diligenciado el oficio de Presentación del crédito o requerimiento para el pago
            If Not FueEnviadoMsg(pExpediente, 21, revisor) Then
                Dim msg As String = "AE21. Coactivo SyP ha detectado que en el expediente " & pExpediente & " aún NO se ha diligenciado el oficio de Presentación del crédito o requerimiento para el pago"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 26 Then
            'Se ha incumplido la cuota de facilidad
            If Not FueEnviadoMsg(pExpediente, 26, revisor) Then
                Dim msg As String = "AE26. Coactivo SyP ha detectado que en el expediente " & pExpediente & " se ha incumplido la cuota de facilidad"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        ElseIf pNumAlarma = 29 Then
            'RESOLUCIÓN QUE DECLARA CUMPLIDA LA FACILIDAD DE PAGO
            If Not FueEnviadoMsg(pExpediente, 29, revisor) Then
                Dim msg As String = "AE29. Coactivo SyP ha detectado que en el expediente " & pExpediente & " no se ha generado la Resolución que declara cumplida la facilidad de pago"
                RegistrarMensaje(pExpediente, pUsuarioOrigen, revisor, msg, Date.Now, True)
            End If

        End If

    End Sub

    Public Function GetNumTitulo(ByVal pExpediente As String) As String
        Dim NumTitulo As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT MT_nro_titulo FROM MAESTRO_TITULOS WHERE MT_expediente = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("MT_nro_titulo").ToString().Trim <> "" Then
                NumTitulo = Reader("MT_nro_titulo").ToString()
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return NumTitulo
    End Function

    'Existe Resolucion de Facilidad
    Public Function ExisteResFac(ByVal pExpediente As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT TOP 1 DG_ID FROM DOCUMENTOS_GENERADOS " &
                                "	WHERE DG_EXPEDIENTE = '" & pExpediente & "' AND " &
                                "		(DG_COD_ACTO = '332' OR DG_COD_ACTO = '338') "
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Public Function ExisteOficioPresentacion(ByVal pExpediente As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT NroOfiPres, FecOfiPres FROM CONCURSALES WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("NroOfiPres").ToString().Trim <> "" And
                Reader("NroOfiPres").ToString().Trim <> "sin datos" And
                Reader("NroOfiPres").ToString().Trim <> "NA" And
                Reader("NroOfiPres").ToString().Trim <> "No hay reg" Then

                sw = True
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Public Function ExisteInformacionConsursal(ByVal pExpediente As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT TipoProcCon FROM CONCURSALES WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("TipoProcCon").ToString().Trim <> "" Then
                sw = True
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Public Function GetFecNotCorLC(ByVal pExpediente As String, ByRef pNroResApLiq As String) As Date
        Dim FecNotCorLC As Date = Nothing

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT FecNotCorLC, NroResApLiq FROM coactivo WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'Fecha de notificacion personal
            If Reader("FecNotCorLC").ToString().Trim <> "" Then
                FecNotCorLC = Reader("FecNotCorLC").ToString()
                FecNotCorLC = FecNotCorLC.Date
                pNroResApLiq = Reader("NroResApLiq").ToString()

                If pNroResApLiq = "Sin Datos" Then
                    pNroResApLiq = ""
                End If
            End If

        End If
        Reader.Close()
        Connection.Close()
        '
        Return FecNotCorLC
    End Function

    Public Function GetFechaRadObjContraLiqCredito(ByVal pExpediente As String, ByRef pNroResApLiq As String) As Date
        Dim FecRadObjContraLiqCredito As Date = Nothing

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT FecRadObj, NroResApLiq FROM coactivo WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'Fecha de notificacion personal
            If Reader("FecRadObj").ToString().Trim <> "" Then
                FecRadObjContraLiqCredito = Reader("FecRadObj").ToString()
                FecRadObjContraLiqCredito = FecRadObjContraLiqCredito.Date
                pNroResApLiq = Reader("NroResApLiq").ToString()

                If pNroResApLiq = "Sin Datos" Then
                    pNroResApLiq = ""
                End If
            End If

        End If
        Reader.Close()
        Connection.Close()
        '
        Return FecRadObjContraLiqCredito
    End Function

    Public Function GetFechaNotifOrdenaEjec(ByVal pExpediente As String, ByRef pNroResLiquiCred As String) As Date
        Dim FechaNotifOrdenaEjec As Date = Nothing

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT NroResLiquiCred, FecNotifCor FROM coactivo WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'Fecha de notificacion personal
            If Reader("FecNotifCor").ToString().Trim <> "" Then
                FechaNotifOrdenaEjec = Reader("FecNotifCor").ToString()
                FechaNotifOrdenaEjec = FechaNotifOrdenaEjec.Date
                pNroResLiquiCred = Reader("NroResLiquiCred").ToString()

                If pNroResLiquiCred = "Sin Datos" Then
                    pNroResLiquiCred = ""
                End If
            End If

        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaNotifOrdenaEjec
    End Function

    Public Function GetFechaRadicRecursoRepvsResExcep(ByVal pExpediente As String, ByRef pNroResResuelve As String) As Date
        Dim FecRadRRRE As Date = Nothing

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT NroResolRes, FecRadRecurso FROM coactivo WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'Fecha de notificacion personal
            If Reader("FecRadRecurso").ToString().Trim <> "" Then
                FecRadRRRE = Reader("FecRadRecurso").ToString()
                FecRadRRRE = FecRadRRRE.Date
                pNroResResuelve = Reader("NroResolRes").ToString()
            End If

        End If
        Reader.Close()
        Connection.Close()
        '
        Return FecRadRRRE
    End Function

    Public Function GetFechaRadicExcepciones(ByVal pExpediente As String, ByRef pNroResResuelve As String) As Date
        Dim FecRadExcepciones As Date = Nothing

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT NroResResuelve, FecRad FROM EXCEPCIONES WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'Fecha de notificacion personal
            If Reader("FecRad").ToString().Trim <> "" Then
                FecRadExcepciones = Reader("FecRad").ToString()
                FecRadExcepciones = FecRadExcepciones.Date
                pNroResResuelve = Reader("NroResResuelve").ToString()
            End If

        End If
        Reader.Close()
        Connection.Close()
        '
        Return FecRadExcepciones
    End Function

    Public Function GetFechaNotificacionMP(ByVal pExpediente As String) As Date
        Dim FechaNotificacionMP As Date = Nothing
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT FecNotiPers, FecNotiCor, FecNotAvi FROM MANDAMIENTOS_PAGO WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            'Fecha de notificacion personal
            If Reader("FecNotiPers").ToString().Trim <> "" Then
                FechaNotificacionMP = Reader("FecNotiPers").ToString()
                FechaNotificacionMP = FechaNotificacionMP.Date
            End If

            'Fecha de notificacion por correo
            If Reader("FecNotiCor").ToString().Trim <> "" Then
                FechaNotificacionMP = Reader("FecNotiCor").ToString()
                FechaNotificacionMP = FechaNotificacionMP.Date
            End If

            'Fecha de notificacion por aviso
            If Reader("FecNotAvi").ToString().Trim <> "" Then
                FechaNotificacionMP = Reader("FecNotAvi").ToString()
                FechaNotificacionMP = FechaNotificacionMP.Date
            End If

        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaNotificacionMP
    End Function


    Public Function GetFechaPagoRec(ByVal pExpediente As String, ByVal pFecCuota As String) As Date
        Dim FechaPagoReciente As Date = Nothing
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT FECHA_PAGO " &
                            "	FROM DETALLES_ACUERDO_PAGO " &
                            "	WHERE DOCUMENTO = (SELECT DOCUMENTO FROM MAESTRO_ACUERDOS WHERE expediente = '" & pExpediente & "' AND ESTADO = 'I') AND " &
                            "		FECHA_CUOTA =  CONVERT(DATETIME, '" & pFecCuota & "', 103)"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("FECHA_PAGO").ToString().Trim <> "" Then
                FechaPagoReciente = Reader("FECHA_PAGO").ToString()
                FechaPagoReciente = FechaPagoReciente.Date
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaPagoReciente
    End Function

    Public Function GetFechaUltimaCuota(ByVal pExpediente As String) As Date
        Dim FecUltimaCuota As Date = Nothing
        Dim Hoy As String = Date.Now.ToString("dd/MM/yyyy")

        'OJO ESTA FECHA FIJA ES PARA UNA PRUEBA
        'Hoy = "15/06/2014"

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT MAX(FECHA_CUOTA) AS UltimaCuota " &
                            "	FROM DETALLES_ACUERDO_PAGO " &
                            "	WHERE DOCUMENTO = (SELECT DOCUMENTO FROM MAESTRO_ACUERDOS " &
                            "						WHERE expediente = '" & pExpediente & "' AND ESTADO = 'I') "
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("UltimaCuota").ToString().Trim <> "" Then
                FecUltimaCuota = Reader("UltimaCuota").ToString()
                FecUltimaCuota = FecUltimaCuota.Date
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return FecUltimaCuota
    End Function

    Public Function GetFechaCuotaReciente(ByVal pExpediente As String) As Date
        Dim FechaCuotaReciente As Date = Nothing
        Dim Hoy As String = Date.Now.ToString("dd/MM/yyyy")

        'OJO ESTA FECHA FIJA ES PARA UNA PRUEBA
        'Hoy = "15/06/2014"

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT MAX(FECHA_CUOTA) AS FecCuoRecent " &
                            "	FROM DETALLES_ACUERDO_PAGO " &
                            "	WHERE DOCUMENTO = (SELECT DOCUMENTO FROM MAESTRO_ACUERDOS WHERE expediente = '" & pExpediente & "' AND ESTADO = 'I') AND " &
                            "		FECHA_CUOTA <  CONVERT(DATETIME, '" & Hoy & "', 103)"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("FecCuoRecent").ToString().Trim <> "" Then
                FechaCuotaReciente = Reader("FecCuoRecent").ToString()
                FechaCuotaReciente = FechaCuotaReciente.Date
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaCuotaReciente
    End Function

    Public Function GetFechaEditTitulo(ByVal pTitulo As String) As Date
        Dim FechaEditTitulo As Date = Nothing
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT LOG_FECHA FROM LOG_AUDITORIA WHERE LOG_CONSE = (SELECT MIN(LOG_CONSE) FROM LOG_AUDITORIA WHERE LOG_MODULO = 'Gestión de títulos ejecutivos' AND LOG_DOC_AFEC = 'No. título " & pTitulo & "')"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("LOG_FECHA").ToString().Trim <> "" Then
                FechaEditTitulo = Reader("LOG_FECHA").ToString()
                FechaEditTitulo = FechaEditTitulo.Date
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaEditTitulo
    End Function

    Public Function GetFechaEnvioCasoCallCenter(ByVal pExpediente As String) As Date
        Dim FechaEnvioCasoCallCenter As Date = Nothing
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT FecEnvioCC FROM PERSUASIVO WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("FecEnvioCC").ToString().Trim <> "" Then
                FechaEnvioCasoCallCenter = Reader("FecEnvioCC").ToString()
                FechaEnvioCasoCallCenter = FechaEnvioCasoCallCenter.Date
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaEnvioCasoCallCenter
    End Function

    Public Function GetFechaPrimerPersuasivo(ByVal pExpediente As String) As Date
        Dim FechaPrimPersuasivo As Date = Nothing
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT FecEnvOfi1 FROM PERSUASIVO WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("FecEnvOfi1").ToString().Trim <> "" Then
                FechaPrimPersuasivo = Reader("FecEnvOfi1").ToString()
                FechaPrimPersuasivo = FechaPrimPersuasivo.Date
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaPrimPersuasivo
    End Function

    Public Function GetFechaSegundoPersuasivo(ByVal pExpediente As String) As Date
        Dim FechaSegPersuasivo As Date = Nothing
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT FecEnvOfi2 FROM PERSUASIVO WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            If Reader("FecEnvOfi2").ToString().Trim <> "" Then
                FechaSegPersuasivo = Reader("FecEnvOfi2").ToString()
                FechaSegPersuasivo = FechaSegPersuasivo.Date
            End If
        End If
        Reader.Close()
        Connection.Close()
        '
        Return FechaSegPersuasivo
    End Function

    Public Function ExisteResolOrdenaEjec(ByVal pExpediente As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT NroResolEjec FROM coactivo WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Public Function ExisteMandamientoPago(ByVal pExpediente As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT NroResolMP FROM MANDAMIENTOS_PAGO WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Public Function ExisteSolCambioEstado(ByVal pExpediente As String, ByRef FecCamEst As Date) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT fecha FROM SOLICITUDES_CAMBIOESTADO WHERE NroExp = '" & pExpediente & "' AND estadosol = 1"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
            FecCamEst = Reader("fecha").ToString().Trim
            FecCamEst = FecCamEst.Date
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Public Function ExisteSolCambioEstado2(ByVal pExpediente As String, ByVal pEstadoActual As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM SOLICITUDES_CAMBIOESTADO WHERE NroExp = '" & pExpediente & "' AND estadoactual = '" & pEstadoActual & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
        End If

        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Public Function FueEnviadoMsg(ByVal pExpediente As String, ByVal pNumAlarma As Integer, ByVal pUsuDestino As String) As Boolean
        Dim Respuesta As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT * FROM mensajes WHERE NroExp = '" & pExpediente & "' AND UsuDestino = '" & pUsuDestino & "' AND Mensaje LIKE 'AE" & pNumAlarma.ToString & "%'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Respuesta = True
        End If
        Reader.Close()
        Connection.Close()
        '
        Return Respuesta

    End Function


    Public Sub RegistrarMensaje(ByVal pNroExp As String, ByVal pUsuOrigen As String, ByVal pUsuDestino As String, ByVal pMensaje As String, ByVal pFecEnvio As DateTime, Optional ByVal MsgSistema As Boolean = False)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        'Comandos SQL
        Dim InsertSQL As String = "INSERT INTO mensajes (NroExp, UsuOrigen, UsuDestino, Mensaje, FecEnvio) " &
                                  "VALUES (@NroExp, @UsuOrigen, @UsuDestino, @Mensaje, @FecEnvio)"

        ' insert             
        Command = New SqlCommand(InsertSQL, Connection)
        ' 
        Command.Parameters.AddWithValue("@NroExp", pNroExp)
        Command.Parameters.AddWithValue("@UsuOrigen", pUsuOrigen)
        Command.Parameters.AddWithValue("@UsuDestino", pUsuDestino)
        Command.Parameters.AddWithValue("@Mensaje", pMensaje)
        Command.Parameters.AddWithValue("@FecEnvio", pFecEnvio)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            If MsgSistema Then
                LogProc.SaveLog("SYSTEM", "Registro de mensajes", "Expediente " & pNroExp, Command)
            Else
                LogProc.SaveLog(pUsuOrigen, "Registro de mensajes", "Expediente " & pNroExp, Command)
            End If

        Catch ex As Exception

        End Try

        Connection.Close()
    End Sub

    Public Function GetRepartidor(ByVal pExpediente As String) As String
        Dim idRepartidor As String = ""

        If pExpediente <> "" Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT repartidor FROM CAMBIOS_ESTADO WHERE idunico = (SELECT MAX(idunico) FROM CAMBIOS_ESTADO WHERE NroExp = @EFINROEXP)"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@EFINROEXP", pExpediente)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            'Si se encontro el expediente
            If Reader.Read Then
                idRepartidor = Reader("repartidor").ToString()
            End If
            Reader.Close()
            Connection.Close()
        End If

        Return idRepartidor
    End Function

    Public Function GetIDGestorResp(ByVal pExpediente As String) As String
        Dim idGestor As String = ""

        If pExpediente <> "" Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT EFIUSUASIG FROM EJEFISGLOBAL WHERE EFINROEXP = @EFINROEXP"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@EFINROEXP", pExpediente)
            Dim Reader As SqlDataReader = Command.ExecuteReader
            'Si se encontro el expediente
            If Reader.Read Then
                idGestor = Reader("EFIUSUASIG").ToString()
            End If
            Reader.Close()
            Connection.Close()
        End If

        Return idGestor
    End Function

    Public Function GetConsecutivoUsuario() As String
        Dim NuevoConsecutivo As String = ""
        '-----------------------------------------
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT con_user FROM MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 1"
        Dim Command As New SqlCommand(sql, Connection)

        Dim Reader As SqlDataReader = Command.ExecuteReader
        'Si se encontro el expediente
        If Reader.Read Then
            'NuevoConsecutivo = Reader("con_user").ToString()
            NuevoConsecutivo = Format(Reader("con_user"), "0000")
        End If
        Reader.Close()
        Connection.Close()
        '-----------------------------------------
        Return NuevoConsecutivo
    End Function

    Public Function RegistrarUsuario(ByVal pIdUsuario As String, ByVal pNivel As Integer, Optional ByVal pSuperior As String = "") As String
        Dim msg As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        'Comandos SQL
        Dim InsertSQL As String = "INSERT INTO usuarios (codigo, nombre, documento, clave, nivelacces, cobrador, apppredial, appvehic, appcuotasp, appindycom, login, useractivo, useremail, usercamclave, superior) " &
                                  "VALUES (@codigo, @nombre, @documento, @clave, @nivelacces, @cobrador, @apppredial, @appvehic, @appcuotasp, @appindycom, @login, @useractivo, @useremail, @usercamclave, @superior)"

        ' insert             
        Command = New SqlCommand(InsertSQL, Connection)

        'variables
        Dim consecutivo As String
        consecutivo = GetConsecutivoUsuario()

        ' parametros
        Command.Parameters.AddWithValue("@codigo", consecutivo)
        Command.Parameters.AddWithValue("@nombre", pIdUsuario)
        Command.Parameters.AddWithValue("@documento", "123")
        Command.Parameters.AddWithValue("@clave", "202cb962ac59075b964b07152d234b70") '123 EN LOCAL
        Command.Parameters.AddWithValue("@nivelacces", pNivel)
        Command.Parameters.AddWithValue("@cobrador", "01")
        Command.Parameters.AddWithValue("@apppredial", "S")
        Command.Parameters.AddWithValue("@appvehic", "S")
        Command.Parameters.AddWithValue("@appcuotasp", "S")
        Command.Parameters.AddWithValue("@appindycom", "S")
        Command.Parameters.AddWithValue("@login", pIdUsuario)
        Command.Parameters.AddWithValue("@useractivo", True)
        Command.Parameters.AddWithValue("@useremail", pIdUsuario.Trim & "@hotmail.com")
        Command.Parameters.AddWithValue("@usercamclave", True)
        Command.Parameters.AddWithValue("@superior", pSuperior)

        Try
            Command.ExecuteNonQuery()
            IncrementarConsecutivoUsuarios()

        Catch ex As Exception
            msg = ex.Message.Trim

        End Try

        Connection.Close()

        Return msg
    End Function

    Public Sub IncrementarConsecutivoUsuarios()
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        'Comandos SQL
        Dim UpdateSQL As String = "update MAESTRO_CONSECUTIVOS set con_user = con_user + 1 where CON_IDENTIFICADOR = 1"

        ' Update             
        Command = New SqlCommand(UpdateSQL, Connection)

        Try
            Command.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Connection.Close()
    End Sub
    '/-----------------------------------------------------------------  
    'ID _HU:  003
    'Nombre HU: Ajuste a la funcionalidad del Campo “Alerta - Termino”
    'Empresa: UT TECHNOLOGY 
    'Autor: Jeisson Gómez 
    'Fecha: 20-04-2017  
    'Objetivo : Se cambia por la homologación entregada por la UGPP. 
    '           Último cambio para la entrega. 
    '------------------------------------------------------------------/

    Public Function FnRetornarExplicacion(ByVal intAlarma As Integer) As String
        Dim strExplicacion As String = String.Empty

        If DictAlarmas.ContainsKey(intAlarma) Then
            strExplicacion = DictAlarmas.Item(intAlarma)
        End If

        Return strExplicacion
    End Function

End Class
