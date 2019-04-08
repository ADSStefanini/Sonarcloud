Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Xml
Imports System.IO.Compression
Imports Ionic.Zip




Public Class WordReport
    'creacion de reportes'
    Public Function CreateReport(ByVal TablaDatos As Data.DataTable, ByVal NombrePlantilla As String, Optional ByVal Parametros() As Marcadores_Adicionales = Nothing) As String

        Dim carpeta As String = Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "PlantillasXml\"
        Dim xmlPlantilla As XmlDocument
        Dim xmlBody, xmlEncabezado As XmlNode
        Dim BodyInnerXml As String
        Try
            ''Compruebo que exista la plantilla
            If IO.File.Exists(carpeta & NombrePlantilla) Then

                'Generamos un documento Xml para cargar el documento
                xmlPlantilla = New XmlDocument()
                xmlPlantilla.Load(carpeta & NombrePlantilla)


                'compruebo que existe el nodo encabezado
                If xmlPlantilla.GetElementsByTagName("w:hdr").Count > 0 Then
                    For i As Integer = 0 To xmlPlantilla.GetElementsByTagName("w:hdr").Count - 1
                        xmlEncabezado = xmlPlantilla.GetElementsByTagName("w:hdr")(i)
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                    String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), _
                                    String.Format("{0}", TablaDatos.Rows(0).Item(Columna).ToString))
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                   String.Format("{0}", Parametros(IndexP).Marcador), _
                                   String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If
                    Next
                End If

                'compruebo que existe el nodo body
                If xmlPlantilla.GetElementsByTagName("w:body").Count > 0 Then
                    xmlBody = xmlPlantilla.GetElementsByTagName("w:body")(0)
                    BodyInnerXml = xmlBody.InnerXml
                    'Remplazo Contenido de Body segun los datos
                    For Fila As Integer = 0 To TablaDatos.Rows.Count - 1
                        Dim NuevoBody As String = BodyInnerXml
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            If NuevoBody.Contains(TablaDatos.Columns(Columna).ColumnName) Then
                                NuevoBody = NuevoBody.Replace( _
                                    String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), _
                                    String.Format("{0}", TablaDatos.Rows(Fila).Item(Columna).ToString))
                            End If
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                NuevoBody = NuevoBody.Replace( _
                                   String.Format("{0}", Parametros(IndexP).Marcador), _
                                   String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If
                                If Fila = 0 Then
                                    xmlBody.InnerXml = NuevoBody
                                Else
                                    xmlBody.InnerXml &= NuevoBody
                                End If
                            Next
                            Return xmlPlantilla.InnerXml
                        End If
                        Return ""
            End If
            Return ""
        Catch Ex As Exception
            MsgBox(Ex.Message)
            Return ""
        End Try
    End Function

    Public Function CreateReport(ByVal TablaDatos As Data.DataTable, ByVal codigoPlantilla As Reportes, Optional ByVal Parametros() As Marcadores_Adicionales = Nothing) As String

        Dim carpeta As String = Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "PlantillasXml\"
        Dim xmlPlantilla As XmlDocument
        Dim xmlBody, xmlEncabezado As XmlNode
        Dim BodyInnerXml As String
        Dim plantilla As String = GetNombrePlantilla(codigoPlantilla)


        Try
            ''Compruebo que exista la plantilla
            If IO.File.Exists(carpeta & plantilla) Then

                'Generamos un documento Xml para cargar el documento
                xmlPlantilla = New XmlDocument()
                xmlPlantilla.Load(carpeta & plantilla)

                'compruebo que existe el nodo encabezado
                If xmlPlantilla.GetElementsByTagName("w:hdr").Count > 0 Then
                    For i As Integer = 0 To xmlPlantilla.GetElementsByTagName("w:hdr").Count - 1
                        xmlEncabezado = xmlPlantilla.GetElementsByTagName("w:hdr")(i)
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                    String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), _
                                    String.Format("{0}", TablaDatos.Rows(0).Item(Columna).ToString))
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                   String.Format("{0}", Parametros(IndexP).Marcador), _
                                   String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If
                    Next
                End If


                'compruebo que existe el nodo body
                If xmlPlantilla.GetElementsByTagName("w:body").Count > 0 Then
                    xmlBody = xmlPlantilla.GetElementsByTagName("w:body")(0)
                    BodyInnerXml = xmlBody.InnerXml
                    'Remplazo Contenido de Body segun los datos
                    For Fila As Integer = 0 To TablaDatos.Rows.Count - 1
                        Dim NuevoBody As String = BodyInnerXml
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            If NuevoBody.Contains(TablaDatos.Columns(Columna).ColumnName) Then
                                NuevoBody = NuevoBody.Replace( _
                                    String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), _
                                    String.Format("{0}", TablaDatos.Rows(Fila).Item(Columna).ToString))
                            End If
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                NuevoBody = NuevoBody.Replace( _
                                   String.Format("{0}", Parametros(IndexP).Marcador), _
                                   String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If
                        If Fila = 0 Then
                            xmlBody.InnerXml = NuevoBody
                        Else
                            xmlBody.InnerXml &= NuevoBody
                        End If
                    Next
                    Return xmlPlantilla.InnerXml
                End If
                Return ""
            End If
            Return ""
        Catch Ex As Exception
            MsgBox(Ex.Message)
            Return ""
        End Try
    End Function

    Public Function CreateReportWithTable(ByVal TablaDatos As Data.DataTable, ByVal codigoPlantilla As Reportes, ByVal CamposTabla As String, Optional ByVal Parametros() As Marcadores_Adicionales = Nothing) As String
        Dim carpeta As String = Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "PlantillasXml\"
        Dim TableColumns() As String = CamposTabla.Split(",")
        Dim xmlPlantilla As XmlDocument
        Dim xmlBody, xmlEncabezado, xmlTable As XmlNode
        Dim xmlTableList As XmlNodeList
        Dim BodyInnerXml As String
        Dim TablaInnerXml As String = GetPlantillaTabla(codigoPlantilla)
        Dim plantilla As String = GetNombrePlantilla(codigoPlantilla)
        Try
            ''Compruebo que exista la plantilla
            If IO.File.Exists(carpeta & plantilla) Then

                'Generamos un documento Xml para cargar el documento
                xmlPlantilla = New XmlDocument()
                xmlPlantilla.Load(carpeta & plantilla)

                'compruebo que existe el nodo encabezado
                If xmlPlantilla.GetElementsByTagName("w:hdr").Count > 0 Then
                    For i As Integer = 0 To xmlPlantilla.GetElementsByTagName("w:hdr").Count - 1
                        xmlEncabezado = xmlPlantilla.GetElementsByTagName("w:hdr")(i)
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                    String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), _
                                    String.Format("{0}", TablaDatos.Rows(0).Item(Columna).ToString))
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                   String.Format("{0}", Parametros(IndexP).Marcador), _
                                   String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If
                    Next
                End If

                'compruebo que existe el nodo body
                If xmlPlantilla.GetElementsByTagName("w:body").Count > 0 Then

                    'Lleno la tabla
                    xmlTable = xmlPlantilla.GetElementsByTagName("w:tbl")(0)
                    xmlTableList = xmlPlantilla.GetElementsByTagName("w:tbl")
                    Dim NuevaFila As String = ""
                    For Fila2 As Integer = 0 To TablaDatos.Rows.Count - 1
                        NuevaFila = TablaInnerXml
                        For col As Integer = 0 To TableColumns.Length - 1
                            NuevaFila = NuevaFila.Replace( _
                                    String.Format("{0}", "COLUMNA_" & col + 1), _
                                    String.Format("{0}", TablaDatos.Rows(Fila2).Item(TableColumns(col)).ToString))
                        Next
                        '    xmlTable.InnerXml &= NuevaFila
                        For Each node As XmlNode In xmlTableList
                            node.InnerXml &= NuevaFila
                        Next
                    Next

                    xmlBody = xmlPlantilla.GetElementsByTagName("w:body")(0)
                    BodyInnerXml = xmlBody.InnerXml
                    'Remplazo Contenido de Body segun los datos

                    'For Fila As Integer = 0 To TablaDatos.Rows.Count - 1
                    Dim Fila As Integer = 0
                    Dim NuevoBody As String = BodyInnerXml
                    For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                        If NuevoBody.Contains(TablaDatos.Columns(Columna).ColumnName) Then
                            NuevoBody = NuevoBody.Replace( _
                                String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), _
                                String.Format("{0}", TablaDatos.Rows(Fila).Item(Columna).ToString))
                        End If
                        'Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                NuevoBody = NuevoBody.Replace( _
                                 String.Format("{0}", Parametros(IndexP).Marcador), _
                                   String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If
                        If Fila = 0 Then
                            xmlBody.InnerXml = NuevoBody
                        Else
                            xmlBody.InnerXml &= NuevoBody
                        End If
                    Next
                    Return xmlPlantilla.InnerXml
                End If
                Return ""
            End If
            Return ""
        Catch Ex As Exception
            MsgBox(Ex.Message)
            Return ""
        End Try

    End Function

    Public Function CreateReportMasivo(ByVal TablaDatos As Data.DataTable, ByVal codigoPlantilla As Reportes, Optional ByVal Parametros() As Marcadores_Adicionales = Nothing) As String

        Dim carpeta As String = Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "PlantillasXml\"
        Dim xmlPlantilla As XmlDocument
        Dim xmlBody, xmlEncabezado As XmlNode
        Dim BodyInnerXml As String
        Dim plantilla As String = GetNombrePlantilla(codigoPlantilla)

        'plantilla = "Inicio1.xml"
        Try
            ''Compruebo que exista la plantilla
            If IO.File.Exists(carpeta & plantilla) Then

                'Generamos un documento Xml para cargar el documento
                xmlPlantilla = New XmlDocument()
                xmlPlantilla.Load(carpeta & plantilla)
                xmlPlantilla.PreserveWhitespace = True

                'compruebo que existe el nodo encabezado
                If xmlPlantilla.GetElementsByTagName("w:hdr").Count > 0 Then
                    For i As Integer = 0 To xmlPlantilla.GetElementsByTagName("w:hdr").Count - 1
                        xmlEncabezado = xmlPlantilla.GetElementsByTagName("w:hdr")(i)
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace(String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), String.Format("<![CDATA[{0}]]>", TablaDatos.Rows(0).Item(Columna).ToString))
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace(String.Format("{0}", Parametros(IndexP).Marcador), String.Format("<![CDATA[{0}]]>", Parametros(IndexP).Valor))
                            Next
                        End If
                    Next
                End If

                'compruebo que existe el nodo body
                If xmlPlantilla.GetElementsByTagName("w:body").Count > 0 Then
                    xmlBody = xmlPlantilla.GetElementsByTagName("w:body")(0)
                    BodyInnerXml = xmlBody.InnerXml
                    'Remplazo Contenido de Body segun los datos
                    For Fila As Integer = 0 To TablaDatos.Rows.Count - 1
                        Dim NuevoBody As String = BodyInnerXml
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            NuevoBody = NuevoBody.Replace(String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), String.Format("<![CDATA[{0}]]>", TablaDatos.Rows(Fila).Item(Columna).ToString))
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                NuevoBody = NuevoBody.Replace(String.Format("{0}", Parametros(IndexP).Marcador), String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If
                        If Fila = 0 Then
                            xmlBody.InnerXml = NuevoBody
                        Else
                            xmlBody.InnerXml &= NuevoBody
                        End If

                    Next
                    Return xmlPlantilla.InnerXml
                End If
                Return ""
            End If
            Return ""
        Catch Ex As Exception
            MsgBox(Ex.Message)
            Return ""
        End Try
    End Function

    Public Function CreateReportMasivoResoluciones(ByVal TablaDatos As Data.DataTable, ByVal codigoPlantilla As Reportes, ByVal nombre_informe As String, ByVal USER As String, Optional ByVal Parametros() As Marcadores_Adicionales = Nothing) As String

        Dim carpeta As String = Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "PlantillasXml\"
        Dim xmlPlantilla As XmlDocument
        '' Dim xmlInner As String
        Dim xmlBody, xmlEncabezado As XmlNode
        Dim BodyInnerXml As String
        Dim plantilla As String = GetNombrePlantilla(codigoPlantilla)
        'plantilla = "Inicio1.xml"
        Try
            ''Compruebo que exista la plantilla
            If IO.File.Exists(carpeta & plantilla) Then

                'Generamos un documento Xml para cargar el documento
                xmlPlantilla = New XmlDocument()
                xmlPlantilla.Load(carpeta & plantilla)
                xmlPlantilla.PreserveWhitespace = True
                ''xmlInner = xmlPlantilla.InnerXml

                'compruebo que existe el nodo encabezado

                'compruebo que existe el nodo body
                If xmlPlantilla.GetElementsByTagName("w:body").Count > 0 Then
                    xmlBody = xmlPlantilla.GetElementsByTagName("w:body")(0)
                    BodyInnerXml = xmlBody.InnerXml

                    For Fila As Integer = 0 To TablaDatos.Rows.Count - 1
                        'xmlPlantilla.InnerXml = xmlInner
                        If xmlPlantilla.GetElementsByTagName("w:hdr").Count > 0 Then
                            For i As Integer = 0 To xmlPlantilla.GetElementsByTagName("w:hdr").Count - 1
                                xmlEncabezado = xmlPlantilla.GetElementsByTagName("w:hdr")(i)
                                For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                                    xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace(String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), String.Format("<![CDATA[{0}]]>", TablaDatos.Rows(i).Item(Columna).ToString))
                                Next
                                If Not Parametros Is Nothing Then
                                    For IndexP As Integer = 0 To Parametros.Length - 1
                                        xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace(String.Format("{0}", Parametros(IndexP).Marcador), String.Format("<![CDATA[{0}]]>", Parametros(IndexP).Valor))
                                    Next
                                End If
                            Next
                        End If
                        'Remplazo Contenido de Body segun los datos
                        Dim NuevoBody As String = BodyInnerXml
                        For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                            NuevoBody = NuevoBody.Replace(String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), String.Format("<![CDATA[{0}]]>", TablaDatos.Rows(Fila).Item(Columna).ToString))
                        Next
                        If Not Parametros Is Nothing Then
                            For IndexP As Integer = 0 To Parametros.Length - 1
                                NuevoBody = NuevoBody.Replace(String.Format("{0}", Parametros(IndexP).Marcador), String.Format("{0}", Parametros(IndexP).Valor))
                            Next
                        End If

                        'If Fila = 0 Then
                        xmlBody.InnerXml = NuevoBody
                        'Else
                        'xmlBody.InnerXml &= NuevoBody
                        'End If
                        xmlPlantilla.Save(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos\" & nombre_informe & CDate(Today.Date).ToString("dd.MM.yyyy") & "\" & TablaDatos.Rows(Fila).Item("MAN_EXPEDIENTE") & "." & CDate(Today.Date).ToString("dd.MM.yyyy") & ".doc")
                    Next
                    ComprimirFolder(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos\" & nombre_informe & CDate(Today.Date).ToString("dd.MM.yyyy"), nombre_informe & CDate(Today.Date).ToString("dd.MM.yyyy"))
                    ''deletegeneration(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos")
                End If



                Return "Listo"
            End If
            Return "Listo"
        Catch Ex As Exception
            MsgBox(Ex.Message)
            Return "Listo"
        End Try
    End Function

    Public Function CreateReportMultiTable(ByVal TablaDatos As Data.DataTable, ByVal codigoPlantilla As Reportes, ByVal Parametros() As Marcadores_Adicionales, ByVal Tabla1 As DataTable, ByVal PosTab1 As Integer, ByVal TotalTab1 As Boolean, ByVal Tabla2 As DataTable, ByVal PosTab2 As Integer, ByVal TotalTab2 As Boolean, ByVal Tabla3 As DataTable, ByVal PosTab3 As Integer, ByVal TotalTab3 As Boolean) As String
        Dim carpeta As String = Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "PlantillasXml\"
        Dim plantilla As String = GetNombrePlantilla(codigoPlantilla)
        Dim xmlPlantilla As XmlDocument
        Dim xmlBody, xmlEncabezado, xmlNodoTable As XmlNode
        Dim xmlTableList As XmlNodeList
        Dim BodyInnerXml, NuevoBody, FilaTabla, FilaTotal, NuevaFila As String
        Try

            ''Compruebo que exista la plantilla
            If IO.File.Exists(carpeta & plantilla) Then
                'Generamos un documento Xml para cargar los datos
                xmlPlantilla = New XmlDocument()
                xmlPlantilla.Load(carpeta & plantilla)
                '
                If TablaDatos.Rows.Count > 0 Then
                    'compruebo que existe el nodo encabezado
                    If xmlPlantilla.GetElementsByTagName("w:hdr").Count > 0 Then
                        For i As Integer = 0 To xmlPlantilla.GetElementsByTagName("w:hdr").Count - 1
                            xmlEncabezado = xmlPlantilla.GetElementsByTagName("w:hdr")(i)
                            For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                                xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                        String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), _
                                        String.Format("{0}", TablaDatos.Rows(0).Item(Columna).ToString))
                            Next
                            If Not Parametros Is Nothing Then
                                For IndexP As Integer = 0 To Parametros.Length - 1
                                    xmlEncabezado.InnerXml = xmlEncabezado.InnerXml.Replace( _
                                       String.Format("{0}", Parametros(IndexP).Marcador), _
                                       String.Format("{0}", Parametros(IndexP).Valor))
                                Next
                            End If
                        Next
                    End If

                    If xmlPlantilla.GetElementsByTagName("w:body").Count > 0 Then
                        xmlBody = xmlPlantilla.GetElementsByTagName("w:body")(0)
                        BodyInnerXml = xmlBody.InnerXml
                        xmlBody.InnerXml = ""
                        For Fila As Integer = 0 To TablaDatos.Rows.Count - 1
                            NuevoBody = BodyInnerXml
                            For Columna As Integer = 0 To TablaDatos.Columns.Count - 1
                                NuevoBody = NuevoBody.Replace(String.Format("{0}", TablaDatos.Columns(Columna).ColumnName), String.Format("{0}", TablaDatos.Rows(Fila).Item(Columna).ToString))
                            Next
                            If Not Parametros Is Nothing Then
                                For IndexP As Integer = 0 To Parametros.Length - 1
                                    NuevoBody = NuevoBody.Replace(String.Format("{0}", Parametros(IndexP).Marcador), String.Format("{0}", Parametros(IndexP).Valor))
                                Next
                            End If
                            xmlBody.InnerXml &= NuevoBody
                        Next
                    End If
                End If

                '
                If xmlPlantilla.GetElementsByTagName("w:tbl").Count > 0 Then
                    xmlTableList = xmlPlantilla.GetElementsByTagName("w:tbl")
                    ''Primera Tabla
                    If Not Tabla1 Is Nothing Then
                        If xmlTableList.Count > PosTab1 Then
                            xmlNodoTable = xmlTableList(PosTab1)
                            If TotalTab1 Then
                                FilaTotal = xmlNodoTable.LastChild.OuterXml
                                xmlNodoTable.RemoveChild(xmlNodoTable.LastChild)
                            End If
                            FilaTabla = xmlNodoTable.LastChild.OuterXml
                            xmlNodoTable.RemoveChild(xmlNodoTable.LastChild)

                            For Fila As Integer = 0 To Tabla1.Rows.Count - 1
                                NuevaFila = FilaTabla
                                For Columna As Integer = 0 To Tabla1.Columns.Count - 1
                                    NuevaFila = NuevaFila.Replace(String.Format("{0}", CStr(Columna + 1) & "_COLUMN"), String.Format("{0}", Tabla1.Rows(Fila).Item(Columna).ToString))
                                Next
                                xmlNodoTable.InnerXml &= NuevaFila
                            Next
                            If TotalTab1 Then
                                xmlNodoTable.InnerXml &= FilaTotal
                            End If
                        End If
                    End If
                    ''Segunda Tabla
                    If Not Tabla2 Is Nothing Then
                        If xmlTableList.Count >= PosTab2 Then
                            xmlNodoTable = xmlTableList(PosTab2)
                            If TotalTab2 Then
                                FilaTotal = xmlNodoTable.LastChild.OuterXml
                                xmlNodoTable.RemoveChild(xmlNodoTable.LastChild)
                            End If
                            FilaTabla = xmlNodoTable.LastChild.OuterXml
                            xmlNodoTable.RemoveChild(xmlNodoTable.LastChild)
                            For Fila As Integer = 0 To Tabla2.Rows.Count - 1
                                NuevaFila = FilaTabla
                                For Columna As Integer = 0 To Tabla2.Columns.Count - 1
                                    NuevaFila = NuevaFila.Replace(String.Format("{0}", CStr(Columna + 1) & "_COLUMN"), String.Format("{0}", Tabla2.Rows(Fila).Item(Columna).ToString))
                                Next
                                xmlNodoTable.InnerXml &= NuevaFila
                            Next
                            If TotalTab2 Then
                                xmlNodoTable.InnerXml &= FilaTotal
                            End If
                        End If
                    End If
                    ''Tercera Tabla
                    If Not Tabla3 Is Nothing Then
                        If xmlTableList.Count >= PosTab3 Then
                            xmlNodoTable = xmlTableList(PosTab3)
                            If TotalTab3 Then
                                FilaTotal = xmlNodoTable.LastChild.OuterXml
                                xmlNodoTable.RemoveChild(xmlNodoTable.LastChild)
                            End If
                            FilaTabla = xmlNodoTable.LastChild.OuterXml
                            xmlNodoTable.RemoveChild(xmlNodoTable.LastChild)
                            For Fila As Integer = 0 To Tabla3.Rows.Count - 1
                                NuevaFila = FilaTabla
                                For Columna As Integer = 0 To Tabla3.Columns.Count - 1
                                    NuevaFila = NuevaFila.Replace(String.Format("{0}", CStr(Columna + 1) & "_COLUMN"), String.Format("{0}", Tabla3.Rows(Fila).Item(Columna).ToString))
                                Next
                                xmlNodoTable.InnerXml &= NuevaFila
                            Next
                            If TotalTab3 Then
                                xmlNodoTable.InnerXml &= FilaTotal
                            End If
                        End If
                    End If
                End If
                Return xmlPlantilla.InnerXml
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function


    'Nombramiento del Reporte '
    Private Function GetNombrePlantilla(ByVal plantilla As Reportes) As String

        Select Case plantilla

            Case Reportes.PrimerOficioDeCobroPersuasivoomisos

                Return "217-Persuasivo.PrimerOficio.Pila.xml"
            Case Reportes.SegundoOficioDeCobroPersuasivoomisos
                Return "218-Persuasivo.SegundoOficio.Pila.xml"

            Case Reportes.PrimerOficioDeCobroPersuasivoFosiga
                Return "219-Persuasivo.PrimerOficio.Fosiga.xml"

            Case Reportes.SegundoOficioDeCobroPersuasivoFosiga
                Return "220-Persuasivo.SegundoOficio.Fosiga.xml"

            Case Reportes.PrimeOficioPersuasivoMultaDirectoFosiga
                Return "221-Persuasivo.PrimerOficio.Multa.Directo.Fosiga.xml"

            Case Reportes.SolicitudDocumentosPago
                Return "223-Persuasivo.Solicitud.Documentos.Pago.xml"

            Case Reportes.VerificacionPagoAprobado
                Return "224-Persuasivo.Verificacion.Pago.Aprobado.xml"

            Case Reportes.CambioEstadoExpedienteIndividual
                Return "225-Persuasivo.CambioEstado.Expediente.Individual.xml"

            Case Reportes.PrimerOficioDeCobroPersuasivo
                Return "301-Persuasivo.PrimerOficio.Pila.xml"

            Case Reportes.SegundoOficioDeCobroPersuasivo
                Return "302-Persuasivo.SegundoOficio.Pila.xml"

            Case Reportes.PrimerOficioCondenaJudicial
                Return "303-Persuasivo.PrimerOficio.CondenaJudicial.xml"

            Case Reportes.SegundoOficioCondenaJudicial
                Return "304-Persuasivo.SegundoOficio.CondenaJudicial.xml"

            Case Reportes.PrimerOficioMulta1607
                Return "305-Persuasivo.PrimerOficio.Multa1607.xml"

            Case Reportes.SegundoOficioMulta1607
                Return "306-Persuasivo.SegundoOficio.Multa1607.xml"

            Case Reportes.PrimerOficioMulta1438
                Return "307-Persuasivo.PrimerOficio.Multa1438.xml"

            Case Reportes.SegundoOficioMulta1438
                Return "308-Persuasivo.SegundoOficio.Multa1438.xml"

            Case Reportes.PlanillaSolicitudConceptosJuridica
                Return "309-Persuasivo.PlanillaSolicitud.Conceptos.Juridica.xml"

            Case Reportes.ComunicacionTerminacionyArchivo
                Return "310-Persuasivo.Comunicacion.TerminacionyArchivo.xml"

            Case Reportes.RespuestaSolicitudPazSalvo
                Return "311-Persuasivo.RespuestaSolicitudPaz.Salvo.xml"

            Case Reportes.TrasladoPorCompetenciasDireccionParafiscales
                Return "312-Persuasivo.Traslado Por Competencias.Direccion.Parafiscales.xml"

            Case Reportes.PresentacionDelCreditoParafiscal
                Return "245-Concursales.Formato.Presentacion Credito.Parafiscales.xml"

            Case Reportes.PresentacionDelCreditoCuotasPartes
                Return "244-Concursales.Formato.PresentacionDelCrédito.CuotasPartes.xml"

            Case Reportes.ContestacionRevocatoriaDirecta

                Return "313-Persuasivo.Contestacion.Revocatoria.Directa.xml"
            Case Reportes.Aprobaciónliquidacióndelcrédito

                Return "233-Coactivo.Aprobación.liquidación.crédito.xml"

            Case Reportes.AutoterminacionyArchivo

                Return "226-Persuasivo.AutoTerminacion.Archivo.xml"

            Case Reportes.NotificacionPorCorreo
                Return "056-Coactivo.Notificacion.Correo.MandamientoPago.xml"

            Case Reportes.DevolucionTituloTesoreria
                Return "228-Coactivo.Devolucion.Titulo.Tesoreria.xml"

            Case Reportes.LevantamientodeMedidaCautelar

                Return "229-Coactivo.LevantamientodeMedidaCautelar.xml"
            Case Reportes.LiquidacionCreditoCosta

                Return "230-Coactivo.Liquidacion.Credito.Costa.xml"
            Case Reportes.CitacionMandamientoPagoSocios

                Return "318-Coactivo.Citacion.MandamientoPagoSocios.xml"
            Case Reportes.PrimerOficioCuotasPartes

                Return "329-Persuasivo.Primer.Oficio.Cuotas.Partes.xml"
            Case Reportes.SegundoOficioCuotasPartes

                Return "330-Persuasivo.Segundo.Oficio.Cuotas.Partes.xml"
            Case Reportes.TrasladoMinsterio
                Return "222-Persuasivo.Traslado.Ministerio.xml"
            Case Reportes.FacilidadesPagoResolucionConcede
                Return "331-FacilidadesPago.Resolucion.Concede.xml"
            Case Reportes.FacilidadesPagoDeclaraCumplidaParafiscales
                Return "332-FacilidadesPago.Declara.Cumplida.Parafiscales.xml"

            Case Reportes.FacilidadesPagoRespuestaSolicitudParafiscales
                Return "333-FacilidadesPago.Respuesta.Solicitud.Parafiscales.xml"

            Case Reportes.OficioInformaNoConcedidaFacilidadInvitaPago, Reportes.OficioInformaNoConcedidaFacilidadInvitaPagoMulta
                Return "334-FacilidadPago.Oficio.Informa.No.Concedida.Facilidad.Invita.Pago.xml"

            Case Reportes.OficioSolicitaCumplimientoRequisitos, Reportes.OficioSolicitaCumplimientoRequisitosMulta

                Return "335-FacilidadPago.Oficio.Solicita.Cumplimiento.Requisitos.xml"
            Case Reportes.ResolucionDeclaraIncumplidaParafiscales

                Return "336-FacilidadesPago.Resolucion.Declara.Incumplida.Parafiscales.xml"

            Case Reportes.FacilidadPagoResolucionConcedeMulta
                Return "337-FacilidadPago.Resolucion.Concede.Multa.xml"

            Case Reportes.FacilidadPagoDeclaraCumplidaMulta

                Return "338-FacilidadPago.Declara.Cumplida.Multa.xml"

            Case Reportes.FacilidadOficioInformaNoconcedidaFacilidadMulta
                Return "339-Facilidad.Oficio.Informa.Noconcedida.Facilidad.Multa.xml"

            Case Reportes.FacilidadesPagoFormatoAjusteUltimaCuota
                Return "340-FacilidadPago.FormatoAjuste.UltimaCuota.xml"

            Case Reportes.NotificacionLiquidacionCredito
                Return "328-Coactivo.Notificacion.Liquidacion.Credito.xml"

            Case Reportes.EmbargoCuentaBancariaMultaHospitales
                Return "321-Coactivo.Embargo.Cuenta.Bancaria.Multa.Hospitales.xml"

            Case Reportes.EmbargoCuentaBancariaMulta
                Return "320-Coactivo.Embargo.Cuenta.Bancaria.Multa.xml"

            Case Reportes.AperturaDePruebasLiquidacion
                Return "314-Coactivo.Apertura.De.Pruebas.Liquidacion.xml"

            Case Reportes.MemorandoIntegracionPruebas
                Return "322-Coactivo.Memorando.Integracion.Pruebas.xml"

            Case Reportes.AprobacionLiquidacionCreditoLiquidacionOficial
                Return "315-Coactivo.Aprobacion.Liquidacion.Credito.Liquidacion.Oficial.xml"

            Case Reportes.CitacionMandamientoPago
                Return "317-Coactivo.Citacion.MandamientoPago.xml"

            Case Reportes.EmbargoBancarioLiquidacionOficial
                Return "319-Coactivo.Embargo.Bancario.Liquidacion.Oficial.xml"

            Case Reportes.FraccionamientoTituloDepositoJudicial
                Return "323-Coactivo.Fraccionamiento.Titulo.Deposito.Judicial.xml"

            Case Reportes.MemorandoDevolucionTitulo
                Return "324-Coactivo.Memorando.Devolucion.Titulo.xml"

            Case Reportes.NOTIFICACIONABREAPRUEBAS
                Return "325-Coactivo.Notificacion.Apertura.Pruebas.xml"

            Case Reportes.NOTIFICACIONAPROBACIÓNLIQUIDACIÓNDELCRÉDITO
                Return "326-Coactivo.Notificacion.Aprobacion.Liquidacion.Credito.xml"

            Case Reportes.NOTIFICACIÓNDETERMINACIÓNDELPROCESO
                Return "327.Coactivo.Notificacion.Terminacion.Proceso.xml"

            Case Reportes.NotificacionOrdenEjecucion
                Return "346-Coactivo.Notificacion.Orden.Ejecucion.xml"

            Case Reportes.NotificacionResuelveExepciones
                Return "347-Coactivo.Notificacion.Resuelve.Exepciones.xml"

            Case Reportes.OficioComunicacionDevolucionTituloDepositoJudicial
                Return "348-Coactivo.Oficio.Comunicacion.Devolucion.Titulo.Deposito.Judicial.xml"

            Case Reportes.PlanillaEmbargo
                Return "349- Coactivo.Planilla.Embargo.xml"

            Case Reportes.ReduccionDeEmbargosPorPagoParcial
                Return "350-Coactivo.Reduccion.De.Embargos.Por.Pago.Parcial.xml"
            Case Reportes.OrdenEjecucionLiquidacionOficial
                Return "351-Coactivo.Orden.Ejecucion.Liquidacion.Oficial.xml"
            Case Reportes.OrdenEjecucionMulta
                Return "352-Coactivo.Orden.Ejecucion.Multa.xml"
            Case Reportes.MandamientoPagoPorPila
                Return "013-Coactivo.MandamientoPago.Por.Pila.xml"
            Case Reportes.CoactivoMpPagoMulta1438Nuevauenta
                Return "345-Coactivo.Mp.Pago.Multa.1438.Nueva.Cuenta.xml"
            Case Reportes.CoactivoMPConPagoEnFosygaYPila
                Return "353-Coactivo.MP.Con.Pago.En.Fosyga.Y.Pila.xml"
            Case Reportes.CoactivoMPConPagoEnFosyga
                Return "354-Coactivo.MP.Con.Pago.En.Fosyga.xml"
            Case Reportes.CoactivoMPConRecursoModificatorio
                Return "355-Coactivo.MP.Con.Recurso.Modificatorio.xml"
            Case Reportes.CoactivoMPConSocios
                Return "356-Coactivo.MP.Con.Socios.xml"
            Case Reportes.CoactivoPlanillaLevantamientoEmbargo
                Return "358-Coactivo.Planilla.Levantamiento.Embargo.xml"
            Case Reportes.RechazaExcepcionesSigueAdelanteConEjecucion
                Return "359-Coactivo.Rechaza.Excepciones.Sigue.Adelante.Con.Ejecucion.xml"
            Case Reportes.RecursoReposicionReposicionParcial
                Return "360-Coactivo.Recurso.Reposicion.Reposicion.Parcial.xml"
            Case Reportes.DevolucionTitulo
                Return "232-Coactivo.Devolucion.Titulo.xml"
            Case Reportes.AutoTerminacionProcesoPagoTotal
                Return "316-Coactivo.Auto.Terminacion.Proceso.Pago.Total.xml"
            Case Reportes.VINCULACIONSOCIOSSOLIDARIOSLIQUIDACIÓNOFICIAL
                Return "363-Coactivo.VINCULACION.SOCIOS.SOLIDARIOS.LIQUIDACIÓN.OFICIAL.xml"

            Case Reportes.FraccionamientoDepositoJudicial
                Return "364-Coactivo.Fraccionamiento.Deposito.Judicial.xml"
            Case Reportes.TerminacionporTutela
                Return "366-Coactivo.Terminacion.Tutela.xml"
            Case Reportes.AplicacionDepositoJudicial
                Return "365-Coactivo.Aplicacion.Titulo.Fraccionamiento.xml"
            Case Reportes.Terminaciondevoluciontitulodepositjudicial
                Return "367-Coactivo.Terminacion.Pago.Total.Devolucion.Titulo.Judicial.xml"
            Case Reportes.DevolucionTitulo2
                Return "232-Coactivo.Devolucion.Titulo2.xml"
            Case Reportes.MandamientoPagoPorPilaMasivo
                Return "013-Coactivo.MandamientoPago.Por.Pila.Masivo.xml"
            Case Reportes.CoactivoMPConPagoEnFosygaMasivo
                Return "354-Coactivo.MP.Con.Pago.En.Fosyga.Masivo.xml"
            Case Reportes.CoactivoMPConRecursoModificatorioMasivo
                Return "355-Coactivo.MP.Con.Recurso.Modificatorio.Masivo.xml"
            Case Reportes.liquidaciondecreditomulta
                Return "368-Coactivo.Liquidacion.Credito.Costa2.xml"
            Case Reportes.TerminacionProcesoCoactivo_Multa
                Return "369-TERMINACIÓN POR PAGO TOTAL-MULTA.xml"

            Case Reportes.ResuelveySuspendeproceso
                Return "370-Resuelve Excepciones y suspende el proceso.xml"

            Case Reportes.AplicaciondeTituloJudicialSin
                Return "365-Coactivo.Aplicacion.Titulo.Deposito.Judicial.Sin.xml"

            Case Reportes.EmbargosSocios
                Return "381-Embargos_Con_Socios.xml"
            Case Else

                Return Nothing
        End Select



    End Function

    Private Function GetPlantillaTabla(ByVal plantilla As Reportes) As String
        Select Case plantilla
            Case Reportes.LevantamientodeMedidaCautelar
                Return "<w:tr w:rsidR=""007254F7"" w:rsidRPr=""007254F7"" w:rsidTr=""00973DF2""><w:trPr><w:trHeight w:val=""214""/><w:jc w:val=""center""/></w:trPr><w:tc><w:tcPr><w:tcW w:w=""2627"" w:type=""dxa""/><w:tcMar><w:top w:w=""0"" w:type=""dxa""/><w:left w:w=""70"" w:type=""dxa""/><w:bottom w:w=""0"" w:type=""dxa""/><w:right w:w=""70"" w:type=""dxa""/></w:tcMar><w:vAlign w:val=""center""/></w:tcPr><w:p w:rsidR=""007254F7"" w:rsidRPr=""007254F7"" w:rsidRDefault=""007A50A3"" w:rsidP=""007254F7""><w:pPr><w:pStyle w:val=""Standard""/><w:autoSpaceDE w:val=""0""/><w:jc w:val=""center""/><w:textAlignment w:val=""auto""/><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/><w:kern w:val=""0""/><w:sz w:val=""20""/><w:szCs w:val=""20""/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/><w:kern w:val=""0""/><w:sz w:val=""20""/><w:szCs w:val=""20""/></w:rPr><w:t>COLUMNA_1</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w=""2003"" w:type=""dxa""/><w:tcMar><w:top w:w=""0"" w:type=""dxa""/><w:left w:w=""70"" w:type=""dxa""/><w:bottom w:w=""0"" w:type=""dxa""/><w:right w:w=""70"" w:type=""dxa""/></w:tcMar><w:vAlign w:val=""center""/></w:tcPr><w:p w:rsidR=""007254F7"" w:rsidRPr=""007254F7"" w:rsidRDefault=""007A50A3"" w:rsidP=""007254F7""><w:pPr><w:pStyle w:val=""Standard""/><w:autoSpaceDE w:val=""0""/><w:jc w:val=""center""/><w:textAlignment w:val=""auto""/><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/><w:kern w:val=""0""/><w:sz w:val=""20""/><w:szCs w:val=""20""/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Times New Roman"" w:hAnsi=""Arial Narrow""/><w:color w:val=""1D1B11""/><w:sz w:val=""20""/><w:szCs w:val=""20""/></w:rPr><w:t>COLUMNA_2</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w=""2823"" w:type=""dxa""/><w:tcMar><w:top w:w=""0"" w:type=""dxa""/><w:left w:w=""70"" w:type=""dxa""/><w:bottom w:w=""0"" w:type=""dxa""/><w:right w:w=""70"" w:type=""dxa""/></w:tcMar><w:vAlign w:val=""center""/></w:tcPr><w:p w:rsidR=""007254F7"" w:rsidRPr=""007254F7"" w:rsidRDefault=""007A50A3"" w:rsidP=""007254F7""><w:pPr><w:pStyle w:val=""Standard""/><w:autoSpaceDE w:val=""0""/><w:jc w:val=""center""/><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Unicode MS"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/><w:sz w:val=""20""/><w:szCs w:val=""20""/><w:lang w:val=""es-ES"" w:eastAsia=""es-ES""/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Times New Roman"" w:hAnsi=""Arial Narrow""/><w:color w:val=""1D1B11""/><w:sz w:val=""20""/><w:szCs w:val=""20""/></w:rPr><w:t>COLUMNA_3</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w=""1686"" w:type=""dxa""/><w:tcMar><w:top w:w=""0"" w:type=""dxa""/><w:left w:w=""70"" w:type=""dxa""/><w:bottom w:w=""0"" w:type=""dxa""/><w:right w:w=""70"" w:type=""dxa""/></w:tcMar><w:vAlign w:val=""center""/></w:tcPr><w:p w:rsidR=""007254F7"" w:rsidRPr=""007254F7"" w:rsidRDefault=""007A50A3"" w:rsidP=""007A50A3""><w:pPr><w:pStyle w:val=""Standard""/><w:autoSpaceDE w:val=""0""/><w:spacing w:line=""276"" w:lineRule=""auto""/><w:jc w:val=""right""/><w:textAlignment w:val=""auto""/><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/><w:kern w:val=""0""/><w:sz w:val=""20""/><w:szCs w:val=""20""/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/><w:kern w:val=""0""/><w:sz w:val=""20""/><w:szCs w:val=""20""/></w:rPr><w:t>COLUMNA_4</w:t></w:r></w:p></w:tc> </w:tr>"""
            Case Reportes.DevolucionTituloTesoreria
                Return "<w:tr w:rsidR=""00EF3DBA"" w:rsidTr=""00E81220""> <w:trPr> <w:trHeight w:val=""223""/> <w:tblHeader/> <w:jc w:val=""center""/> </w:trPr> <w:tc> <w:tcPr> <w:tcW w:w=""1532"" w:type=""dxa""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> </w:tcBorders> <w:tcMar> <w:top w:w=""28"" w:type=""dxa""/> <w:left w:w=""28"" w:type=""dxa""/> <w:bottom w:w=""28"" w:type=""dxa""/> <w:right w:w=""28"" w:type=""dxa""/> </w:tcMar> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00EF3DBA"" w:rsidRPr=""00E81220"" w:rsidRDefault=""00E81220""> <w:pPr> <w:pStyle w:val=""TableContentsuser""/> <w:autoSpaceDE w:val=""0""/> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> </w:pPr> <w:r w:rsidRPr=""00E81220""> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> <w:t>COLUMNA_1</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1365"" w:type=""dxa""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> </w:tcBorders> <w:tcMar> <w:top w:w=""28"" w:type=""dxa""/> <w:left w:w=""28"" w:type=""dxa""/> <w:bottom w:w=""28"" w:type=""dxa""/> <w:right w:w=""28"" w:type=""dxa""/> </w:tcMar> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00EF3DBA"" w:rsidRPr=""00E81220"" w:rsidRDefault=""00E81220""> <w:pPr> <w:pStyle w:val=""TableContentsuser""/> <w:autoSpaceDE w:val=""0""/> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> </w:pPr> <w:r w:rsidRPr=""00E81220""> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> <w:t>COLUMNA_2</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""2402"" w:type=""dxa""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> </w:tcBorders> <w:tcMar> <w:top w:w=""28"" w:type=""dxa""/> <w:left w:w=""28"" w:type=""dxa""/> <w:bottom w:w=""28"" w:type=""dxa""/> <w:right w:w=""28"" w:type=""dxa""/> </w:tcMar> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00EF3DBA"" w:rsidRPr=""00E81220"" w:rsidRDefault=""00E81220""> <w:pPr> <w:pStyle w:val=""TableContentsuser""/> <w:autoSpaceDE w:val=""0""/> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> </w:pPr> <w:r w:rsidRPr=""00E81220""> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> <w:t>COLUMNA_3</w:t> </w:r> <w:bookmarkStart w:id=""0"" w:name=""_GoBack""/> <w:bookmarkEnd w:id=""0""/> <w:r w:rsidRPr=""00E81220""> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr></w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1104"" w:type=""dxa""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> </w:tcBorders> <w:tcMar> <w:top w:w=""28"" w:type=""dxa""/> <w:left w:w=""28"" w:type=""dxa""/> <w:bottom w:w=""28"" w:type=""dxa""/> <w:right w:w=""28"" w:type=""dxa""/> </w:tcMar> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00EF3DBA"" w:rsidRPr=""00E81220"" w:rsidRDefault=""00E81220"" w:rsidP=""00E81220""> <w:pPr> <w:pStyle w:val=""TableContentsuser""/> <w:autoSpaceDE w:val=""0""/> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> </w:pPr> <w:r w:rsidRPr=""00E81220""> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> <w:t>COLUMNA_4</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1247"" w:type=""dxa""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""000000""/> </w:tcBorders> <w:tcMar> <w:top w:w=""28"" w:type=""dxa""/> <w:left w:w=""28"" w:type=""dxa""/> <w:bottom w:w=""28"" w:type=""dxa""/> <w:right w:w=""28"" w:type=""dxa""/> </w:tcMar> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00EF3DBA"" w:rsidRPr=""00E81220"" w:rsidRDefault=""00E81220""> <w:pPr> <w:pStyle w:val=""TableContentsuser""/> <w:autoSpaceDE w:val=""0""/> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> </w:pPr> <w:r w:rsidRPr=""00E81220""> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""MS Mincho"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:b/> <w:bCs/> <w:caps/> </w:rPr> <w:t>COLUMNA_5</w:t> </w:r> </w:p> </w:tc> </w:tr> "
                '    'Case Reportes.CitacionMandamientoPagoSocios
                '    '    Return "<w:tr w:rsidR=""005F1469"" w:rsidTr=""005F1469""> <w:trPr> <w:trHeight w:val=""269""/> </w:trPr> <w:tc> <w:tcPr> <w:tcW w:w=""1851"" w:type=""dxa""/> </w:tcPr> <w:p w:rsidR=""005F1469"" w:rsidRDefault=""005F1469""> <w:pPr> <w:pStyle w:val=""Textbody""/> <w:spacing w:after=""0""/> <w:jc w:val=""both""/> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:lang w:val=""es-ES"" w:eastAsia=""es-ES""/> </w:rPr> </w:pPr> <w:r w:rsidRPr=""005F1469""> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:caps/> <w:lang w:val=""es-ES"" w:eastAsia=""es-ES""/> </w:rPr> <w:t>COLUMNA_1</w:t> </w:r>  </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""3273"" w:type=""dxa""/> </w:tcPr> <w:p w:rsidR=""005F1469"" w:rsidRDefault=""005F1469""> <w:pPr> <w:pStyle w:val=""Textbody""/> <w:spacing w:after=""0""/> <w:jc w:val=""both""/> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:lang w:val=""es-ES"" w:eastAsia=""es-ES""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:ascii=""Arial Narrow"" w:eastAsia=""Arial Narrow"" w:hAnsi=""Arial Narrow"" w:cs=""Arial Narrow""/> <w:lang w:val=""es-ES"" w:eastAsia=""es-ES""/> </w:rPr> <w:t>COLUMNA_2</w:t> </w:r> </w:p> </w:tc> </w:tr> "
            Case Reportes.FacilidadesPagoResolucionConcede
                Return "<w:tr w:rsidR=""0090792D"" w:rsidRPr=""009247CF"" w:rsidTr=""0090792D""> <w:trPr> <w:trHeight w:val=""444""/> <w:jc w:val=""center""/> </w:trPr> <w:tc> <w:tcPr> <w:tcW w:w=""639"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_1</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1409"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""nil""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_2</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""795"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_3</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1041"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidDel=""00D9439E"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_4</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1116"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_5</w:t> </w:r> </w:p> </w:tc> </w:tr>"
            Case Reportes.FacilidadesPagoDeclaraCumplidaParafiscales
                Return "<w:tr w:rsidR=""0090792D"" w:rsidRPr=""009247CF"" w:rsidTr=""0090792D""> <w:trPr> <w:trHeight w:val=""444""/> <w:jc w:val=""center""/> </w:trPr> <w:tc> <w:tcPr> <w:tcW w:w=""639"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_1</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1409"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""nil""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_2</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""795"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_3</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1041"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidDel=""00D9439E"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_4</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1116"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00683F8E"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""0090792D"" w:rsidP=""0090792D""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_5</w:t> </w:r> </w:p> </w:tc> </w:tr>"
            Case Reportes.FacilidadPagoResolucionConcedeMulta
                Return "<w:tr w:rsidR=""004E0FE0"" w:rsidRPr=""009247CF"" w:rsidTr=""009B4B79""> <w:trPr> <w:trHeight w:val=""459""/> </w:trPr> <w:tc> <w:tcPr> <w:tcW w:w=""482"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""004E0FE0"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""002456E2"" w:rsidP=""009B4B79""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_1</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1188"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""nil""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""004E0FE0"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""002456E2"" w:rsidP=""009B4B79""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_2</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""668"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""004E0FE0"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""002456E2"" w:rsidP=""009B4B79""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_3</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""786"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""004E0FE0"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""002456E2"" w:rsidP=""009B4B79""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_4</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""1123"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""nil""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""004E0FE0"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""002456E2"" w:rsidP=""009B4B79""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_5</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""753"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""004E0FE0"" w:rsidRPr=""00A55BAD"" w:rsidRDefault=""002456E2"" w:rsidP=""009B4B79""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:sz w:val=""22""/> <w:szCs w:val=""22""/> </w:rPr> <w:t>COLUMNA_6</w:t> </w:r> </w:p> </w:tc> </w:tr> "
            Case Reportes.FacilidadPagoDeclaraCumplidaMulta
                Return "<w:tr w:rsidR=""00FE25D2"" w:rsidRPr=""00513EA3"" w:rsidTr=""00FE25D2""> <w:trPr> <w:trHeight w:val=""780""/> </w:trPr> <w:tc> <w:tcPr> <w:tcW w:w=""599"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""nil""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> <w:hideMark/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""007B594A""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_1</w:t> </w:r> <w:r w:rsidRPr=""00A52954""> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""599"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""nil""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""nil""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""00FE25D2""> <w:pPr> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_2</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""599"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""nil""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""00C634E8""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_3</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""599"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""nil""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""00C634E8""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_4</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""612"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""00C634E8""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_5</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""619"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""nil""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""00C634E8""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_5</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""624"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:left w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:noWrap/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""00C634E8""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_6</w:t> </w:r> </w:p> </w:tc> <w:tc> <w:tcPr> <w:tcW w:w=""751"" w:type=""pct""/> <w:tcBorders> <w:top w:val=""nil""/> <w:left w:val=""nil""/> <w:bottom w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> <w:right w:val=""single"" w:sz=""8"" w:space=""0"" w:color=""auto""/> </w:tcBorders> <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto""/> <w:vAlign w:val=""center""/> </w:tcPr> <w:p w:rsidR=""00FE25D2"" w:rsidRPr=""00A52954"" w:rsidRDefault=""00FE25D2"" w:rsidP=""00C634E8""> <w:pPr> <w:jc w:val=""center""/> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> </w:pPr> <w:r> <w:rPr> <w:rFonts w:eastAsia=""Times New Roman""/> <w:color w:val=""000000""/> <w:sz w:val=""20""/> <w:szCs w:val=""20""/> <w:lang w:val=""es-CO"" w:eastAsia=""es-CO""/> </w:rPr> <w:t>COLUMNA_7</w:t> </w:r> </w:p> </w:tc> </w:tr>"
        End Select

    End Function

    'Parametros Marcadores'
    Public Structure Marcadores_Adicionales
        Dim Marcador As String
        Dim Valor As String
    End Structure

    Public Shared Function ComprimirFolder(ByVal Ruta As String, ByVal Nombre As String) As [Boolean]

        Try
            If System.IO.Directory.Exists(Ruta) Then

                Using zip As New ZipFile()
                    zip.AddDirectory(Ruta)
                    zip.Save(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos\" & Nombre & ".zip")
                End Using

            End If



            Return True
        Catch

            Return False
        End Try

    End Function

    Public Function deletegeneration(ByVal ruta As String)
        For Each s As String In System.IO.Directory.GetFiles(ruta)

            If s.Contains(".zip") = False Then
                System.IO.File.Delete(ruta)
            End If

        Next
        Return Nothing
    End Function

End Class

'Enumeracion de reportes'
Public Enum Reportes
    'Persuasivos'

    PrimerOficioDeCobroPersuasivoomisos = 217
    SegundoOficioDeCobroPersuasivoomisos = 218
    PrimerOficioDeCobroPersuasivoFosiga = 219
    SegundoOficioDeCobroPersuasivoFosiga = 220
    PrimeOficioPersuasivoMultaDirectoFosiga = 221
    SolicitudDocumentosPago = 223
    VerificacionPagoAprobado = 224
    CambioEstadoExpedienteIndividual = 225
    PrimerOficioDeCobroPersuasivo = 301
    SegundoOficioDeCobroPersuasivo = 302
    PrimerOficioCondenaJudicial = 303
    SegundoOficioCondenaJudicial = 304
    PrimerOficioMulta1607 = 305
    SegundoOficioMulta1607 = 306
    PrimerOficioMulta1438 = 307
    SegundoOficioMulta1438 = 308
    PlanillaSolicitudConceptosJuridica = 309
    ComunicacionTerminacionyArchivo = 310
    RespuestaSolicitudPazSalvo = 311
    TrasladoPorCompetenciasDireccionParafiscales = 312
    ContestacionRevocatoriaDirecta = 313
    AutoterminacionyArchivo = 226
    PrimerOficioCuotasPartes = 329
    SegundoOficioCuotasPartes = 330
    TrasladoMinsterio = 222

    'Concursales'

    PresentacionDelCreditoCuotasPartes = 244
    PresentacionDelCreditoParafiscal = 245

    'Coactivos'
    MandamientoPagoPorPila = 13
    MandamientoPagoPorPilaMasivo = 131

    Aprobaciónliquidacióndelcrédito = 233
    NotificacionPorCorreo = 56
    DevolucionTituloTesoreria = 228
    LevantamientodeMedidaCautelar = 229
    LiquidacionCreditoCosta = 230
    AperturaDePruebasLiquidacion = 314
    AprobacionLiquidacionCreditoLiquidacionOficial = 315
    CitacionMandamientoPago = 317
    CitacionMandamientoPagoSocios = 318
    EmbargoBancarioLiquidacionOficial = 319
    EmbargoCuentaBancariaMulta = 320
    EmbargoCuentaBancariaMultaHospitales = 321
    MemorandoIntegracionPruebas = 322
    FraccionamientoTituloDepositoJudicial = 323
    MemorandoDevolucionTitulo = 324
    NOTIFICACIONABREAPRUEBAS = 325
    NOTIFICACIONAPROBACIÓNLIQUIDACIÓNDELCRÉDITO = 326
    NOTIFICACIÓNDETERMINACIÓNDELPROCESO = 327
    NotificacionLiquidacionCredito = 328
    NotificacionOrdenEjecucion = 346
    NotificacionResuelveExepciones = 347
    OficioComunicacionDevolucionTituloDepositoJudicial = 348
    PlanillaEmbargo = 349
    ReduccionDeEmbargosPorPagoParcial = 350
    OrdenEjecucionLiquidacionOficial = 351
    OrdenEjecucionMulta = 352
    CoactivoMpPagoMulta1438Nuevauenta = 345
    CoactivoMPConPagoEnFosygaYPila = 353

    CoactivoMPConPagoEnFosyga = 354
    CoactivoMPConPagoEnFosygaMasivo = 3541
    CoactivoMPConRecursoModificatorio = 355
    CoactivoMPConRecursoModificatorioMasivo = 3551
    CoactivoMPConSocios = 356
    CoactivoPlanillaLevantamientoEmbargo = 358
    RechazaExcepcionesSigueAdelanteConEjecucion = 359
    RecursoReposicionReposicionParcial = 360
    DevolucionTitulo = 232
    AutoTerminacionProcesoPagoTotal = 316
    VINCULACIONSOCIOSSOLIDARIOSLIQUIDACIÓNOFICIAL = 363
    FraccionamientoDepositoJudicial = 364
    AplicacionDepositoJudicial = 365
    TerminacionporTutela = 366
    Terminaciondevoluciontitulodepositjudicial = 367
    DevolucionTitulo2 = 2321
    liquidaciondecreditomulta = 368
    TerminacionProcesoCoactivo_Multa = 369
    ResuelveySuspendeproceso = 370
    AplicaciondeTituloJudicialSin = 3651
    EmbargosSocios = 381
    'Facilidades de Pago'
    FacilidadesPagoResolucionConcede = 331
    FacilidadesPagoDeclaraCumplidaParafiscales = 332
    FacilidadesPagoRespuestaSolicitudParafiscales = 333
    OficioInformaNoConcedidaFacilidadInvitaPago = 334
    OficioSolicitaCumplimientoRequisitos = 335
    ResolucionDeclaraIncumplidaParafiscales = 336
    FacilidadPagoResolucionConcedeMulta = 337
    FacilidadPagoDeclaraCumplidaMulta = 338
    FacilidadOficioInformaNoconcedidaFacilidadMulta = 339
    OficioInformaNoConcedidaFacilidadInvitaPagoMulta = 341
    OficioSolicitaCumplimientoRequisitosMulta = 342
    FacilidadesPagoFormatoAjusteUltimaCuota = 340
End Enum
