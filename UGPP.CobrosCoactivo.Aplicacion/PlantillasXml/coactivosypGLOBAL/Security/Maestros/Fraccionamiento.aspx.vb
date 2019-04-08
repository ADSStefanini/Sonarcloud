Imports System.Data.SqlClient
Imports coactivosyp.WordReport
Imports System.Math
Public Class WebForm1
    Inherits System.Web.UI.Page
    Dim Expediente, Deposito, Idembargo As String

    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub

    Private Sub SendReport(ByVal NombreArchivo As String, ByVal Plantilla As String)
        ''        Dim nav As String = Request.Browser.Browser
        Response.ContentType = "application/msword"
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.doc", NombreArchivo))
        Response.Write(Plantilla)
        Response.End()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Expediente = Request("Expediente")
        Deposito = Request("Deposito")
        Idembargo = Request("ID")
        txtNroTituloPrincipal.Text = Request("Titulo_Principal")
        If Funciones.getResolucion_anterior(Expediente, 364)(0) = Nothing Then
            Alert("no se puede fraccionar sin resolucion de credito y costas ")
            Exit Sub

        End If
        txtValorTDJPrincipal.Text = String.Format("{0:C0}", CDbl(Request("Valor_Principal")))
        txt_Liquidacion.Text = String.Format("{0:C0}", CDbl(GetLiquidacion(Expediente.Trim, Funciones.getResolucion_anterior(Expediente, "233")(0))))
        llenargrid(txtNroTituloPrincipal.Text.Trim)






    End Sub

    Protected Sub cmd_fraccionamiento_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmd_fraccionamiento.Click
        exportar(1)
    End Sub
    Private Sub exportar(ByVal op As String, Optional ByVal ref As String = "", Optional ByVal ref1 As String = "", Optional ByVal ref3 As String = "")
        ''generador_expedientes de reportes
        Dim worddoc As New WordReport
        Dim worddocresult As String = ""
        Dim DTR As DataTable = getdatosbasicos()

        Select Case op
            Case 1 'fraccionar titulo de deposito judicial'
                Dim datos_individual(13) As WordReport.Marcadores_Adicionales
                '' Dim tdj() As String = GetTituloPrincipal(Expediente.Trim)
                Dim resolucion() As String = getResolucion_anterior(Expediente.Trim, "230")
                Dim fraccionamiento() As Double
                datos_individual(1).Marcador = "letras"
                ''datos_individual(1).Valor = Num2Text(GetLiquidacion(Expediente.Trim))

                If resolucion(0) <> Nothing Then
                    datos_individual(2).Marcador = "fecha_anterior"
                    datos_individual(2).Valor = resolucion(1)

                    datos_individual(3).Marcador = "resolucion_anterior"
                    datos_individual(3).Valor = resolucion(0)


                Else
                    Alert("NO HAY RESOLUCION DE (233)APROBACION DE LIQUIDACION DE PARA GENERAR EL DOCUMENTO ")
                    Exit Sub
                End If

                Dim guardar() As String = saveResolucion(Expediente, "364")

                datos_individual(0).Marcador = "nro_resolucion"
                datos_individual(0).Valor = guardar(0)
                datos_individual(4).Marcador = "fecha_actual"
                datos_individual(4).Valor = guardar(1)

                datos_individual(5).Marcador = "Total_Liquidacion"
                ''datos_individual(5).Valor = String.Format("{0:C0}", GetLiquidacion(Expediente.Trim))
                datos_individual(6).Marcador = "nro_titulo"
                datos_individual(6).Valor = txtNroTituloPrincipal.Text
                datos_individual(7).Marcador = "Ltotal"
                datos_individual(7).Valor = String.Format("{0:C0}", CDbl(txtValorTDJPrincipal.Text))
                datos_individual(8).Marcador = "Ltitulo"
                datos_individual(8).Valor = Num2Text(txtValorTDJPrincipal.Text)
                fraccionamiento = generarfraccionamiento(Deposito, 0, txtNroTituloPrincipal.Text, guardar(0), Now.Date)
                datos_individual(9).Marcador = "titulo1"
                datos_individual(9).Valor = String.Format("{0:C0}", fraccionamiento(0))
                datos_individual(10).Marcador = "lprincipal"
                datos_individual(10).Valor = Num2Text(fraccionamiento(0))
                datos_individual(11).Marcador = "ltitulo2"
                datos_individual(11).Valor = Num2Text(fraccionamiento(1))
                datos_individual(12).Marcador = "titulo2"
                datos_individual(12).Valor = String.Format("{0:C0}", fraccionamiento(1))
                datos_individual(13).Marcador = "fecha_emision"
                datos_individual(13).Valor = guardar(3)
                worddocresult = worddoc.CreateReport(DTR, Reportes.FraccionamientoDepositoJudicial, datos_individual)

                If worddocresult = "" Then
                    ''mensaje no informe
                Else

                    Dim nombre As String = Replace("fraccionamiento de titulo de deposito judicial", " ", ".")
                    SendReport(nombre & Expediente.Trim & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                End If


            Case 2 'aplicar titulo'

                Dim datos_individual(12) As WordReport.Marcadores_Adicionales
                Dim tdj() As String = GetTituloPrincipal(Expediente.Trim)

                Dim resolucion_liquidacion() As String = getResolucion_anterior(Expediente.Trim, "364")
                Dim resolucion() As String = getResolucion_anterior(Expediente.Trim, "365")
                Dim tbl1 As DataTable = Getvalores(txtNroTituloPrincipal.Text)
                Dim resolucion_Cobro() As String = getResolucion_anterior(Expediente.Trim, resolucion_liquidacion(2))



                If resolucion(0) <> Nothing Then
                    datos_individual(2).Marcador = "fecha_anterior"
                    datos_individual(2).Valor = resolucion(1)

                    datos_individual(3).Marcador = "resolucion_anterior"
                    datos_individual(3).Valor = resolucion(0)


                Else
                    Alert("NO HAY RESOLUCION DE (364)FRACCIONAMIENTO  DE TITULO DE DEPOSITO JUDICIAL DE PARA GENERAR EL DOCUMENTO ")
                    Exit Sub
                End If

                Dim guardar() As String = saveResolucion(Expediente, "365")

                datos_individual(0).Marcador = "nro_resolucion"
                datos_individual(0).Valor = guardar(0)

                datos_individual(1).Marcador = "fecha_actual"
                datos_individual(1).Valor = guardar(1)

                actualizar(guardar(0), guardar(1), txtNroTituloPrincipal.Text.Trim, 1, ref3)
                datos_individual(4).Marcador = "letras_liquidacion"
                datos_individual(4).Valor = Num2Text(GetLiquidacion(Expediente.Trim, resolucion_Cobro(0)))

                datos_individual(5).Marcador = "Total_Liquidacion"
                datos_individual(5).Valor = String.Format("{0:C0}", GetLiquidacion(Expediente.Trim, resolucion_Cobro(0)))

                datos_individual(6).Marcador = "titulo_principal"
                datos_individual(6).Valor = txtNroTituloPrincipal.Text.Trim
                datos_individual(7).Marcador = "resolucion_L"
                datos_individual(7).Valor = resolucion_liquidacion(0)
                datos_individual(8).Marcador = "fecha_L"
                datos_individual(8).Valor = resolucion_liquidacion(1)

                datos_individual(9).Marcador = "titulo_judicial"
                datos_individual(9).Valor = ref
                datos_individual(10).Marcador = "vtitulo"
                datos_individual(10).Valor = ref1
                datos_individual(11).Marcador = "fecha_corte"
                datos_individual(11).Valor = resolucion_Cobro(1)
                datos_individual(12).Marcador = "fecha_emision"
                datos_individual(12).Valor = guardar(3)
                worddocresult = worddoc.CreateReportMultiTable(DTR, Reportes.AplicacionDepositoJudicial, datos_individual, tbl1, 0, False, Nothing, 0, False, Nothing, 0, False)

                If worddocresult = "" Then
                    ''mensaje no informe
                Else

                    Dim nombre As String = Replace("APLICACION DE TITULO DE DEPOSITO JUDICIAL", " ", ".")
                    SendReport(nombre & Expediente.Trim & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                End If


            Case 3 'Devolucion de Titulo Deposito Judicial'


                Dim datos_individual(17) As WordReport.Marcadores_Adicionales
                Dim inicial() As String = getResolucion_anterior(Expediente.Trim, "232")
                Dim Desembargo() As String = getResolucion_anterior(Expediente.Trim, inicial(2))
                Dim Embargo() As String = getResolucion_anterior(Expediente.Trim, Desembargo(2))
                Dim Anterior() As String = getResolucion_anterior(Expediente.Trim, Embargo(2))
                Dim Valores() As String = GetActo(Expediente.Trim, "013")
                Dim representante() As String = cargar_representante(Expediente.Trim)
                Dim guardar() As String = saveResolucion(Expediente.Trim, "232")
                Dim tbl1 As DataTable = GetTituloPrincipal2(Expediente.Trim)
                Dim tbl2 As DataTable = GetTituloPrincipal2(Expediente.Trim, 1)

                Dim vc_datos() As String
                vc_datos = overloadresolucion(Expediente.Trim, homologo("316"))
                If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                    inicial(0) = vc_datos(0).ToUpper
                    inicial(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If

                If inicial(0) <> Nothing Then
                    datos_individual(0).Marcador = "terminacion"
                    datos_individual(0).Valor = inicial(0)
                    datos_individual(1).Marcador = "fecha_resolucion"
                    datos_individual(1).Valor = inicial(1)
                Else
                    Alert("NO SE ENCUENTRA UNA (316)TERMINACION DE PROCESO ")
                    Exit Sub
                End If


                vc_datos = overloadresolucion(Expediente.Trim, "229")
                If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                    Desembargo(0) = vc_datos(0).ToUpper
                    Desembargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If

                If Desembargo(0) <> Nothing Then
                    datos_individual(2).Marcador = "nro_desembargo"
                    datos_individual(2).Valor = Desembargo(0)
                    datos_individual(3).Marcador = "fecha_desembargo"
                    datos_individual(3).Valor = Desembargo(1)
                Else
                    Alert("NO SE ENCUENTRA UNA RESOLUCION DE DESEMBARGO ")
                    Exit Sub
                End If

                vc_datos = overloadresolucion(Expediente.Trim, "319")
                If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                    Embargo(0) = vc_datos(0).ToUpper
                    Embargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If


                If Embargo(0) <> Nothing Then

                    datos_individual(4).Marcador = "nro_embargo"
                    datos_individual(4).Valor = Embargo(0)

                    datos_individual(5).Marcador = "fecha_embargo"
                    datos_individual(5).Valor = Embargo(1)
                Else
                    Alert("NO SE ENCUENTRA UNA RESOLUCION DE EMBARGO")
                    Exit Sub
                End If

                vc_datos = overloadresolucion(Expediente.Trim, "013")
                If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                    Anterior(0) = vc_datos(0).ToUpper
                    Anterior(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If


                If Anterior(0) <> Nothing Then
                    datos_individual(6).Marcador = "resolución_antes"
                    datos_individual(6).Valor = Anterior(0)

                    datos_individual(7).Marcador = "fecha_anterior"
                    datos_individual(7).Valor = Anterior(1)
                Else
                    Alert("NO SE ENCUENTRA UNA RESOLUCON DE MANDAMIENTO DE PAGO ")
                    Exit Sub
                End If
                datos_individual(8).Marcador = "replegal"

                If representante(0) <> Nothing Then
                    datos_individual(8).Marcador = "replegal"
                    datos_individual(8).Valor = representante(0)
                    datos_individual(9).Marcador = "rep_nit"
                    datos_individual(9).Valor = representante(1)
                Else
                    datos_individual(8).Marcador = "replegal"
                    datos_individual(8).Valor = DTR.Rows.Item(0)("ED_Nombre")
                    datos_individual(9).Marcador = "rep_nit"
                    datos_individual(9).Valor = DTR.Rows.Item(0)("ED_Codigo_Nit")
                End If
                datos_individual(10).Marcador = "Nsalario"
                datos_individual(10).Valor = Round((DTR.Rows.Item(0)("totaldeuda") / 616000), 0)
                datos_individual(11).Marcador = "numerosalario"
                datos_individual(11).Valor = Num2Text(datos_individual(10).Valor)
                datos_individual(12).Marcador = "tipo_noti"
                datos_individual(12).Valor = Valores(1)
                datos_individual(13).Marcador = "acta_nro"
                datos_individual(13).Valor = Valores(2)
                datos_individual(14).Marcador = "fecha_acta"
                datos_individual(14).Valor = (CDate(Valores(0)).ToString("'del' dd 'de' MMMM 'de' yyy"))

                datos_individual(15).Marcador = "Nro_Resolucion"
                datos_individual(15).Valor = guardar(0)

                datos_individual(16).Marcador = "fecha_actual"
                datos_individual(16).Valor = guardar(1)
                datos_individual(17).Marcador = "fecha_emision"
                datos_individual(17).Valor = guardar(3)
                actualizar(guardar(0), guardar(1), txtNroTituloPrincipal.Text.Trim, 2, ref3)

                worddocresult = worddoc.CreateReportMultiTable(DTR, Reportes.DevolucionTitulo2, datos_individual, tbl1, 0, False, tbl2, 1, False, Nothing, 0, False)
                If worddocresult = "" Then
                    ''mensaje no informe
                Else
                    Dim nombre As String = Replace(Replace("Devolucion de Titulo de Deposito Judicial", " ", "."), "-", ".")
                    SendReport(nombre & "-" & Expediente.Trim & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                End If


        End Select


    End Sub

    Private Function getdatosbasicos() As DataTable
        Dim sqldatatable As New DataTable
        Dim sqladapter As SqlDataAdapter
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New SqlCommand( _
                                                               " SELECT TOP 1 A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                                                               " L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                                                               " c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP                                                                                                                                                                                                                                 " & _
                                                               " FROM  EJEFISGLOBAL A,                                                                                                                                                                                                                     " & _
                                                               " 	  ENTES_DEUDORES B,                                                                                                                                                                                                                     " & _
                                                               " 	  DIRECCIONES C ,                                                                                                                                                                                                                       " & _
                                                               " 	  DEUDORES_EXPEDIENTES D,                                                                                                                                                                                                               " & _
                                                               " 	  MAESTRO_TITULOS E,                                                                                                                                                                                                                    " & _
                                                               " 	  TIPOS_TITULO F,                                                                                                                                                                                                                       " & _
                                                               " 	  TIPOS_IDENTIFICACION G,                                                                                                                                                                                                               " & _
                                                               " 	  DEPARTAMENTOS H,                                                                                                                                                                                                                      " & _
                                                               " 	  MUNICIPIOS I,                                                                                                                                                                                                                         " & _
                                                               " 	  FORMAS_NOTIFICACION J,                                                                                                                                                                                                                " & _
                                                               " 	  USUARIOS K,                                                                                                                                                                                                                           " & _
                                                               " 	  USUARIOS L                                                                                                                                                                                                                            " & _
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                               " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                           " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                           " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                        " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                          " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                              " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                            " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                  " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                              " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                              " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                               " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE                                                                                                                                                                                                             " & _
                                                               " ORDER BY TIPO", sqlconfig)
        sqlmanager.Parameters.AddWithValue("@EXPEDIENTE", expediente.Trim)
        sqladapter = New SqlDataAdapter(sqlmanager)
        sqladapter.Fill(sqldatatable)

        If sqldatatable.Rows.Count > 0 Then
            Return sqldatatable
        Else
            Return Nothing
        End If
    End Function

    Private Function generarfraccionamiento(ByVal nro_deposito As String, ByVal nrotituloJ As String, ByVal nrotituloprincipal As String, ByVal nro_resolucion As String, ByVal fecha_resolucion As Date) As Double()
        Dim primero, segundo As Double
        Dim fraccionamiento(1) As Double
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldata As New SqlCommand
        Dim sqldatatble As New DataTable
        Dim sqladapter As SqlDataAdapter
        primero = txt_Liquidacion.Text
        segundo = txtValorTDJPrincipal.Text - txt_Liquidacion.Text

        sqladapter = New SqlDataAdapter("select nrotituloj as 'nro titulo' ,valortdj as 'valor titulo',b.nombre as estado,('RCC-' +nroresolges )as resolucion ,fecresolges as 'fecha de resolucion' " & _
                                        " from TDJ A,TIPOS_RESOLTDJ  B " & _
                                        "where NroTituloPrincipal ='" & nrotituloprincipal.Trim & "'" & _
                                        "and  A.TipoResolGes=B.codigo ", sqlconfig)

        sqladapter.Fill(sqldatatble)
        If sqldatatble.Rows.Count > 0 Then
            'mensaje de que existe fraccionamiento'
        Else
            sqldata = New SqlCommand(" update TDJ set TipoResolGes ='3' ,NroResolGes = @nro_resolucion ,FecResolGes =@fec_resolucion " & _
                                     " where IdEmbargo =@id_embargo and NroDeposito = @nrodeposito ", sqlconfig)

            sqldata.Parameters.AddWithValue("@nro_resolucion", nro_resolucion)
            sqldata.Parameters.AddWithValue("@fec_resolucion", fecha_resolucion)
            sqldata.Parameters.AddWithValue("@id_embargo", Idembargo)
            sqldata.Parameters.AddWithValue("@nrodeposito", Deposito)

            If sqlconfig.State = ConnectionState.Open Then
                sqlconfig.Close()

            End If
            sqlconfig.Open()
            sqldata.ExecuteNonQuery()


            sqldata = New SqlCommand("insert into TDJ (NroDeposito,NroTituloJ,NroTituloPrincipal,NroResolGes,FecResolGes,valorTDJ,TipoResolGes,IdEmbargo)values (@nro_deposito,@nrotituloJ,@nrotituloprincipal,@nroresolges,@fecresolges,@valorTDJ,@TipoResolGes,@IdEmbargo)", sqlconfig)
            sqldata.Parameters.AddWithValue("@nro_deposito", nro_deposito & "-1")
            sqldata.Parameters.AddWithValue("@nrotituloJ", nrotituloJ)
            sqldata.Parameters.AddWithValue("@nrotituloprincipal", nrotituloprincipal)
            sqldata.Parameters.AddWithValue("@nroresolges", "Sin Movimiento")
            sqldata.Parameters.AddWithValue("@fecresolges", Now.Date)
            sqldata.Parameters.AddWithValue("@valorTDJ", primero)
            sqldata.Parameters.AddWithValue("@TipoResolGes", 4)
            sqldata.Parameters.AddWithValue("@IdEmbargo", Idembargo)
            If sqlconfig.State = ConnectionState.Open Then
                sqlconfig.Close()

            End If
            sqlconfig.Open()
            sqldata.ExecuteNonQuery()

            sqldata = New SqlCommand("insert into TDJ (NroDeposito,NroTituloJ,NroTituloPrincipal,NroResolGes,FecResolGes,valorTDJ,TipoResolGes,IdEmbargo)values (@nro_deposito,@nrotituloJ,@nrotituloprincipal,@nroresolges,@fecresolges,@valorTDJ,@TipoResolGes,@IdEmbargo)", sqlconfig)
            sqldata.Parameters.AddWithValue("@nro_deposito", nro_deposito & "-2")
            sqldata.Parameters.AddWithValue("@nrotituloJ", nrotituloJ)
            sqldata.Parameters.AddWithValue("@nrotituloprincipal", nrotituloprincipal)
            sqldata.Parameters.AddWithValue("@nroresolges", "Sin Movimiento")
            sqldata.Parameters.AddWithValue("@fecresolges", Now.Date)
            sqldata.Parameters.AddWithValue("@valorTDJ", segundo)
            sqldata.Parameters.AddWithValue("@TipoResolGes", 4)
            sqldata.Parameters.AddWithValue("@IdEmbargo", Idembargo)
            If sqlconfig.State = ConnectionState.Open Then
                sqlconfig.Close()

            End If
            sqlconfig.Open()
            sqldata.ExecuteNonQuery()

        End If


        fraccionamiento(0) = primero
        fraccionamiento(1) = segundo

        Return fraccionamiento

    End Function


    Public Sub llenargrid(ByVal tituloprincipal As String)
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqltable As New DataTable
        Dim sqladapter As SqlDataAdapter

        sqladapter = New SqlDataAdapter(" select NroDeposito as 'Nro Deposito',nrotituloj as 'nro titulo' ,'$ '+CAST(CONVERT(varchar, CAST(valortdj AS money), 1) AS varchar)  as 'valor titulo',b.nombre as estado,('RCC-' +nroresolges )as resolucion ,fecresolges as 'fecha de resolucion' " & _
                                        " from TDJ A,TIPOS_RESOLTDJ  B " & _
                                        "where NroTituloPrincipal ='" & tituloprincipal.Trim & "'" & _
                                        "and  A.TipoResolGes=B.codigo ", sqlconfig)
        sqladapter.Fill(sqltable)
        grid_fraccionamiento.DataSource = sqltable
        grid_fraccionamiento.DataBind()




    End Sub



    Private Sub grid_fraccionamiento_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grid_fraccionamiento.RowCommand

        Select Case e.CommandName

            Case "Aplicar"
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = grid_fraccionamiento.Rows(index)
                If row.Cells.Item(3).Text = "APLICACION" Or row.Cells.Item(3).Text = "SIN DATOS" Then
                    exportar(2, row.Cells.Item(1).Text, row.Cells.Item(2).Text, row.Cells.Item(0).Text)
                Else
                    Alert("ESTE TITULO YA FUE APLICADO Y/O DEVUELTO ")
                    Exit Sub
                End If


            Case "Devolver"
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = grid_fraccionamiento.Rows(index)
                If row.Cells.Item(3).Text = "SIN DATOS" Or row.Cells.Item(3).Text = "DEVOLUCION" Then
                    exportar(3, row.Cells.Item(1).Text, row.Cells.Item(2).Text, row.Cells.Item(0).Text)
                Else
                    Alert("ESTE TITULO YA FUE APLICADO Y/O DEVUELTO ")
                    Exit Sub
                End If


            Case "Editar"
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = grid_fraccionamiento.Rows(index)
                If row.Cells.Item(3).Text = "APLICACION" Or row.Cells.Item(3).Text = "DEVOLUCION" Then
                    Response.Redirect("EditTDJ.aspx?ID=" & row.Cells.Item(0).Text & "&pExpediente=" & Expediente.Trim & "&IdEmbargo=" & Idembargo)
                Else
                    Alert("ESTE TITULO YA FUE APLICADO Y/O DEVUELTO POR LO TANTO NO SE PUEDE EDITAR")
                    Exit Sub
                End If

            Case Else
                Alert("no ha seleccionado opciones")


        End Select


    End Sub
    Private Function Getvalores(ByVal TITULO_PRINCIPAL As String) As DataTable
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqltable As New DataTable
        Dim sqladapter As SqlDataAdapter

        sqladapter = New SqlDataAdapter(" select nrotituloj as 'NRO_TITULO' ,' ' AS 'FECHA COSTITUCION',( SELECT A.ED_Nombre FROM ENTES_DEUDORES A,EJEFISGLOBAL B " & _
                                         " WHERE B.EFINIT = A.ED_Codigo_Nit " & _
                                         " AND B.EFINROEXP ='" & Expediente.Trim & "') AS DEPOSITANTE ,D.BAN_NOMBRE AS CONSIGNANTE ,'$ '+CAST(CONVERT(varchar, CAST(valortdj AS money), 1) AS varchar)  as 'valor titulo' " & _
                                         " from TDJ A,TIPOS_RESOLTDJ  B, MAESTRO_BANCOS D  " & _
                                         " where NroTituloPrincipal ='" & TITULO_PRINCIPAL & "'  " & _
                                         " and  A.TipoResolGes=B.codigo " & _
                                         " AND ISNULL(A.Banco,'00')=BAN_CODIGO ", sqlconfig)

        sqladapter.Fill(sqltable)
        If sqltable.Rows.Count > 0 Then
            Return sqltable
        Else
            Return Nothing
        End If

    End Function
    Public Function cargar_representante(ByVal expediente As Integer) As String()
        Dim Ado As New SqlConnection(Funciones.CadenaConexion)
        Dim rep(2) As String
        Dim tblrep As New DataTable
        Dim sqldata As New SqlDataAdapter(" SELECT A.ED_NOMBRE ,A.ED_CODIGO_NIT" & _
                                          " FROM  ENTES_DEUDORES A, " & _
                                          " DEUDORES_EXPEDIENTES B, " & _
                                          " EJEFISGLOBAL C " & _
                                          " WHERE  C.EFINROEXP =B.NROEXP  " & _
                                          " AND A.ED_CODIGO_NIT = B.DEUDOR " & _
                                          " AND TIPO =3 and NroExp = '" & expediente & "'", Ado)
        sqldata.Fill(tblrep)



        If tblrep.Rows.Count > 0 Then
            For i = 0 To tblrep.Rows.Count - 1
                rep(0) = tblrep.Rows.Item(0)("ED_NOMBRE")
                rep(1) = tblrep.Rows.Item(0)("ED_CODIGO_NIT")
            Next

        End If
        Return rep
    End Function

    Public Sub actualizar(ByVal resolucion As String, ByVal fecharesolucion As String, ByVal titulo_principal As String, ByVal tipores As Integer, ByVal nro_deposito As String)
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlcommand As New SqlCommand(" update TDJ set NroResolGes =@resolucion ,FecResolGes =@fecha_resolucion ,TipoResolGes =@tipores" & _
                                         " where NroDeposito = @nro_deposito              " & _
                                         " and IdEmbargo = @idembargo                     " & _
                                         " and NroTituloPrincipal =@nro_titulo_principal  ")
        sqlcommand.Connection = sqlconfig
        sqlcommand.Parameters.AddWithValue("@resolucion", resolucion)
        sqlcommand.Parameters.AddWithValue("@fecha_resolucion", Today.Date) ''CDate(fecharesolucion))
        sqlcommand.Parameters.AddWithValue("@nro_deposito", nro_deposito)
        sqlcommand.Parameters.AddWithValue("@nro_titulo_principal", titulo_principal)
        sqlcommand.Parameters.AddWithValue("@tipores", tipores)
        sqlcommand.Parameters.AddWithValue("@idembargo", Idembargo)

        If sqlcommand.Connection.State = ConnectionState.Open Then
            sqlcommand.Connection.Close()
        End If
        sqlcommand.Connection.Open()
        sqlcommand.ExecuteNonQuery()
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Response.Redirect("EditTDJ.aspx?ID=" & Deposito.Trim & "&pExpediente=" & Expediente.Trim & "&IdEmbargo=" & Idembargo)
    End Sub

    Protected Sub cmd_adicionar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmd_adicionar.Click
        Dim suma_titulos As Integer
        For i = 0 To grid_fraccionamiento.Rows.Count - 1
            Dim row As GridViewRow = grid_fraccionamiento.Rows(i)
            If suma_titulos = 0 Then

            Else
                suma_titulos = suma_titulos + CInt(row.Cells.Item(2).Text)
            End If


        Next
        If suma_titulos > txtValorTDJPrincipal.Text Then
            Alert("NO SE  PUDE ADICIONAR MAS TITULOS DE DEPOSITO JUDICIAL   ,DADO QUE SOBREPASA EL VALOR DE LA LIQUIDACION APROBADA ")
        Else
            Response.Redirect("EditTDJ.aspx?pExpediente=" & Expediente.Trim & "&IdEmbargo=" & Idembargo & "&titulo_principal=" & txtNroTituloPrincipal.Text.Trim)
        End If


    End Sub


End Class
