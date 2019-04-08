Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml.Xsl
Imports System.Xml
Imports System.Math

Partial Public Class cobranzas2
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub


    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub


    Private Sub ValidateUserExp()
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#validateUser').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub


    Private Sub Alert(ByVal Menssage As String, ByVal Rtable As Reportes_Admistratiivos)
        Dim TableBuilder As New StringBuilder
        TableBuilder.Append("<br /><table class=""xdt"">")
        If Rtable.Mandaniento_Pago.Rows.Count > 0 Then
            For Each row As Reportes_Admistratiivos.Mandaniento_PagoRow In Rtable.Mandaniento_Pago.Rows
                For Each Column As DataColumn In Rtable.Mandaniento_Pago.Columns
                    Select Case Column.Caption
                        Case "MAN_EXPEDIENTE"
                            TableBuilder.Append("<tr><th>EXPEDIENTE</th><td>")
                            TableBuilder.Append(row.MAN_EXPEDIENTE)
                            TableBuilder.Append("</td></tr>")
                        Case "MAN_IMPUESTO"
                            TableBuilder.Append("<tr><th>IMPUESTO</th><td>")
                            TableBuilder.Append(row.MAN_IMPUESTO)
                            TableBuilder.Append("</td></tr>")
                    End Select
                Next
            Next
        End If
        TableBuilder.Append("</table>")
        ViewState("message") = Menssage & TableBuilder.ToString
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({width: 480,hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SqlDataSource1.ConnectionString = Funciones.CadenaConexion
        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            menssageError(amsbgox)
            Exit Sub
        End If
        If Not Me.Page.IsPostBack Then
            Page.Form.Attributes.Add("autocomplete", "off")
            If Session("ConexionServer") = Nothing Then
                Session("ConexionServer") = Funciones.CadenaConexion
            End If

            Call cargar_objeto(Lista, "SELECT [DXI_ACTO], ('(' + [DXI_ACTO] + ') ' + [DXI_NOMBREACTO]) AS DXI_NOMBREACTO FROM [DOCUMENTO_INFORMEXIMPUESTO] A, ACTUACIONES B WHERE  A.DXI_ACTO = B.CODIGO AND (([DXI_HISTORIAL] = @DXI_HISTORIAL) AND ([DXI_IMPUESTOVALUE] = @DXI_IMPUESTOVALUE)) AND B.IDETAPA = @IDETAPA ORDER BY 1", "DXI_ACTO", "DXI_NOMBREACTO", Request("etapa"))

            Call CargarDatoas(Session("mcobrador"))
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            Dim tipo As String = Request("tipo")
            If tipo <> Nothing Then
                txtEnte.Text = Request("cedula")
                lblExpediente.Text = Request("expediente")
                Lista.DataBind()
                Lista.SelectedValue = Request("acto")
                Dim reporte As String = Request("reporte")
            End If
        End If
    End Sub


    Private Sub CargarDatoas(ByVal Ente As String)
        Dim ConsultaTable As New DatosConsultasTablas
        Dim MyTable As New DatasetForm.entescobradoresDataTable
        MyTable = ConsultaTable.EntesCobradores(Session("mcobrador"))
        ViewState("DatosEnte") = MyTable
    End Sub


    Private Sub CargarExepciones(ByVal Expediente As String)
        Dim ConsultaTable As New DatosConsultasTablas
        Dim MyTable As New DatasetForm.entescobradoresDataTable
        MyTable = ConsultaTable.EntesCobradores(Session("mcobrador"))
        ViewState("DatosExepciones") = MyTable
    End Sub


    Public Function ValidarExepciones(ByVal expedientes As String) As DataTable
        Dim data As New DataTable
        Dim Sql = "SELECT IMP_NOMBRE,IMP_VALUES,IMP_ENTECOBRADOR,IMP_CAMPOCLAVEID,IMP_ID FROM DOCUMENTO_IMPUESTO WHERE IMP_ENTECOBRADOR = @IMP_ENTECOBRADOR"
        Using adap As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
            adap.SelectCommand.Parameters.Add("@IMP_ENTECOBRADOR", SqlDbType.VarChar).Value = expedientes
            adap.Fill(data)
            Return data
        End Using
    End Function


    Private Function validateReportes() As DataTable
        Dim Sql As String = "SELECT * FROM EJEFISGLOBAL     	" & _
                            " WHERE EFINROEXP = @Expediente    	" & _
                            " AND   EfiModCod = @Impuesto  	    "


        Dim myadapter As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
        myadapter.SelectCommand.Parameters.Add("@Expediente", SqlDbType.VarChar).Value = lblExpediente.Text
        myadapter.SelectCommand.Parameters.Add("@Impuesto", SqlDbType.VarChar).Value = Session("ssimpuesto")
        Dim myTable As New DataTable
        myadapter.Fill(myTable)
        Return myTable
    End Function

    Private Function ManualEspecial_reporte(ByVal Acto As String, ByVal tipo As Byte) As Boolean
        If Acto = "005" OrElse Acto = "023" OrElse Acto = "017" OrElse Acto = "097" OrElse Acto = "162" Then
            Response.Redirect("capturanotificacionpersona.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Return False
        ElseIf Acto = "007" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "campo_variable", "$(function() {var Modulo_dlgd = jQuery('#dialog-variables-report-parametro').dialog({hide: 'fold',width: 550,autoOpen: true,modal: true}); Modulo_dlgd.parent().appendTo(jQuery('form:first'));});", True)
            Return False
        ElseIf Acto = "024" Then
            Response.Redirect("capturainformesvarios.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Return False
        ElseIf Acto = "072" Then
            If validateReportes.Rows.Count > 0 Then
                Response.Redirect("capturaposecionsecuestre.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Else
                Validator.Text = "No hay datos para mostrar."
                Alert(Validator.Text)
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If
            Return False
        ElseIf Acto = "205" Then
            If validateReportes.Rows.Count > 0 Then
                Response.Redirect("capturaperito.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Else
                Validator.Text = "No hay datos para mostrar."
                Alert(Validator.Text)
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If
            Return False
        ElseIf Acto = "209" Then
            If validateReportes.Rows.Count > 0 Then
                Response.Redirect("capturadatosempresa.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Else
                Validator.Text = "No hay datos para mostrar."
                Alert(Validator.Text)
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If
            Return False
        ElseIf Acto = "164" Then
            Response.Redirect("capturafechafallecimiento.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Return False
        ElseIf Acto = "071" Then
            Response.Redirect("capturaoficiocomunicadesembargo.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Return False
        ElseIf Acto = "073" Then
            Dim Sql As String = UCase("SELECT * FROM entra_documentoma WHERE DOC_EXPEDIENTE = @Expediente and doc_actoadministrativo = '072'")
            Dim myadapter As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@Expediente", SqlDbType.VarChar).Value = lblExpediente.Text
            Dim myTable As New DataTable
            myadapter.Fill(myTable)
            If myTable.Rows.Count > 0 Then
                Imprimir("...")
            Else
                Validator.Text = "No hay datos para mostrar..."
                Alert(Validator.Text)
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If
            Return False
        ElseIf Acto = "204" Then
            Dim Sql As String = UCase("SELECT * FROM entra_documentoma WHERE DOC_EXPEDIENTE = @Expediente and doc_actoadministrativo = '205' ")
            Dim myadapter As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@Expediente", SqlDbType.VarChar).Value = lblExpediente.Text
            Dim myTable As New DataTable
            myadapter.Fill(myTable)
            If myTable.Rows.Count > 0 Then
                Imprimir("...")
            Else
                Validator.Text = "No hay datos para mostrar..."
                Alert(Validator.Text)
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If
            Return False
        ElseIf Acto = "028" Then
            Response.Redirect("capturaliquidacioncredito.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Return False
        ElseIf Acto = "206" Then
            Response.Redirect("capturatrasladoavaluo.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Return False
        ElseIf Acto = "213" Then
            Response.Redirect("capturaempresa.aspx?cedula=" & Me.txtEnte.Text & "&expediente=" & lblExpediente.Text & "&acto=" & Lista.SelectedValue & "&nomacto=" & Lista.SelectedItem.Text, True)
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        If (lblExpediente.Text <> "000000") OrElse (lblExpediente.Text <> Nothing) Then
            If (txtEnte.Text <> Nothing) OrElse (Not Session("UsuarioValido") Is Nothing) Then
                Dim p As Boolean
                If Lista.SelectedValue = Nothing Then
                    Validator.Text = "Debe elegir un reporte para continuar."
                    Me.Validator.IsValid = False
                    mensup.InnerHtml = Validator.Text
                    Exit Sub
                ElseIf Not validarExpedientes(lblExpediente.Text.Trim) And Session("mnivelacces") <> 1 Then
                    ValidateUserExp()

                Else

                    'Casos especiales en el cual se le pide datos manuales para continuar...
                    'Ejemplo Notificaciones...
                    p = ManualEspecial_reporte(Lista.SelectedValue, 0)
                    If p = False Then
                        Exit Sub
                    Else
                        Imprimir()
                    End If
                End If
            Else
                Validator.Text = "Para continuar debe digitar el deudor a procesar y por consecuente un expediente."
                Alert(Validator.Text)
                Me.Validator.IsValid = False
                mensup.InnerHtml = "Tal vez este reporte este ocupado intente otra vez."
            End If
        Else
            Validator.Text = "Expediente no valido...."
            Me.Validator.IsValid = False
            Alert(Validator.Text)
        End If

    End Sub

    Function valida_Repo() As String
        If Request("acto") = Nothing Then
            Return Lista.SelectedValue
        Else
            If Request("acto") = Lista.SelectedValue Then
                Return Request("acto")
            Else
                Return Lista.SelectedValue
            End If
        End If
    End Function

    'Private Sub bntEjeRepoVariable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bntEjeRepoVariable.Click
    '    Imprimir()
    'End Sub

    Private Sub Imprimir(Optional ByVal cod_ActoPrevio As String = Nothing)


        Try
            'variable que genera el reporte winword  '
            Dim worddoc As New WordReport
            Dim worddocresult As String = ""
            Dim fecha, fecha2 As Date
            Dim idEntidad As String = Nothing
            idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
            Dim selectcod As String
            If cod_ActoPrevio = "" Then
                selectcod = valida_Repo()
            End If

            Dim MytableRepo As Boolean
            Dim Ado As New SqlConnection(Funciones.CadenaConexion)
            Dim DtsDatos As New Reportes_Admistratiivos
            Dim myBaseClass As New procesos_tributario.sqlQuery.sqlQuery(Ado) 'CONEXION EJECUTA EL SELECT SEGUN LA CONSULTA ... EN LA BASE DE DATOS 
            myBaseClass.Impuesto = Session("ssimpuesto")
            myBaseClass.Expedinete = lblExpediente.Text.Trim
            myBaseClass.Repo = selectcod
            myBaseClass.Datos_Ente = DtsDatos.CAT_CLIENTES
            myBaseClass.Datos_Informe = DtsDatos.Mandaniento_Pago
            myBaseClass.Datos_Acto_Previo = DtsDatos.ENTRA_DOCUMENTOMA
            myBaseClass.Datos_Conceptos_Deuda = DtsDatos.CONCEPTOS_DEUDA
            myBaseClass.Datos_Conceptos_Deuda_Indus = DtsDatos.CONCEPTOS_DEUDA_INDUS
            myBaseClass.Load_Acto_Previo = IIf(cod_ActoPrevio = Nothing, False, True)
            myBaseClass.Ente = Session("mcobrador")
            myBaseClass.ConexionME = Ado
            myBaseClass.mnivelacces = Session("mnivelacces")
            myBaseClass.sscodigousuario = Session("sscodigousuario")
            MytableRepo = myBaseClass.Prima_EjecucionFiscal(Session("ssCodimpadm"), 1)

            If DtsDatos.Mandaniento_Pago.Rows.Count > 0 Then

                'RESOLUCION Y FECHA DEL ACTO PREVIO
                Dim tipo As String = ""
                Dim Load_Acto_Previo As Boolean = GeneraDoc(selectcod)
                If Load_Acto_Previo = True Then
                    tipo = "AND D.TIPO = 1"
                    resolucion_ActoPrevio(myBaseClass.Expedinete, myBaseClass.Repo, myBaseClass.Impuesto, DtsDatos.RESOLUCION_ACTOPREVIO)
                    If DtsDatos.RESOLUCION_ACTOPREVIO.Rows.Count = 0 Then
                        Validator.Text = "<b> No se ha generado la resolucion para el acto  " & Lista.SelectedItem.Text & "</b>"
                        mensup.InnerHtml = Validator.Text
                        'Exit Sub
                    End If
                End If


                Select Case selectcod
                    Case "228", "013", "233", "354", "355", "356", "226", "363", "230", "214", "235", "315", "319", "320", "321", "353", "355", "352", "368", "351", "366", "367", "359", "360", "370", "350"
                        tipo = "AND D.TIPO = 1"
                    Case Else

                End Select


                Dim AdapterMT As SqlClient.SqlDataAdapter

                'CARGAR TABLA DE TITULOS 
                Select Case selectcod

                    Case "228", "013", "233", "354", "355", "356", "226", "363", "230", "214", "235", "315", "319", "320", "321", "353", "355", "352", "368", "316", "366", "367", "351", "352", "359", "360", "370", "350"
                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT TOP 1 A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,F.codigo as codigotitulo,                                                                                                                                                                                                              " & _
                                                               " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP, E.MT_FEC_EXI_LIQ                                                                                                                                                                                                                                 " & _
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
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                              " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                          " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                          " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                       " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                         " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                             " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                           " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                 " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & tipo & "                                                                                                                                                                                               " & _
                                                               " ORDER BY TIPO", Ado)

                    Case "349", "358", "380"
                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                                                               " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP,M.BAN_DIRECCION AS DIRECCIONBANCARIA , M.BAN_TELEFONO AS TELEFONOBANCARIO , M.BAN_NOMBRE AS NOMBREBANCARIO, F.codigo as codigotitulo                       " & _
                                                               " FROM  EJEFISGLOBAL A,                                                                                                                                                                                                                      " & _
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
                                                               " 	  USUARIOS L, MAESTRO_BANCOS M                                                                                                                                                                                                          " & _
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                               " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                           " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                           " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                        " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                          " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                              " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                            " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                  " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & tipo & "                                                                                                                                                                                               " & _
                                                               " ORDER BY TIPO", Ado)

                    Case "217", "219", "221", "222", "301", "307", "329"

                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT A.EFINROEXP AS MAN_EXPEDIENTE, " & _
                        " G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                        " B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                        " C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,  " & _
                        " E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo,  " & _
                        " E.MT_fec_expedicion_titulo, " & _
                        " ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo, '' AS MT_for_notificacion_titulo , " & _
                        " '$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda, " & _
                        " E.totaldeuda,                                                              " & _
                        " CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento," & _
                        " CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio, " & _
                        " k.nombre as Proyecto, L.nombre as Revisor,ED_TipoPersona as DOCUMENTO, " & _
                     " D.TIPO as Tipo_Deudor, c.idunico as IdDireccion, " & _
                     " A.EFINUMMEMO,E.MT_fecha_ejecutoria,a.EFIFECHAEXP, F.codigo as codigotitulo " & _
                     " FROM  EJEFISGLOBAL A,                " & _
                      " ENTES_DEUDORES B,  DIRECCIONES C , " & _
                      " DEUDORES_EXPEDIENTES D, MAESTRO_TITULOS E, " & _
                      " TIPOS_TITULO F, TIPOS_IDENTIFICACION G, " & _
                      " DEPARTAMENTOS H, MUNICIPIOS I, " & _
                      " USUARIOS K, USUARIOS L " & _
                      " WHERE A.EFINROEXP = D.NROEXP " & _
                      " AND   D.DEUDOR = B.ED_Codigo_Nit " & _
                      " AND   B.ED_Codigo_Nit = C.deudor  " & _
                      " AND   E.MT_expediente = A.EFINROEXP " & _
                      " AND   F.codigo = E.MT_tipo_titulo " & _
                      " AND   G.codigo  = B.ED_TipoId " & _
                      " AND   H.codigo = C.Departamento " & _
                      " AND   I.codigo = C.Ciudad " & _
                      " AND   K.codigo = A.EFIUSUASIG " & _
                      " AND   L.codigo = A.EFIUSUREV " & _
                      " AND   A.EFINROEXP = @EXPEDIENTE " & tipo & " " & _
                      " ORDER BY TIPO ", Ado)
                    Case Else
                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT  A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                                                               " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP, F.codigo as codigotitulo                                                                                                                                                       " & _
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
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                              " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                          " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                          " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                       " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                         " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                             " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                           " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                 " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & tipo & "                                                                                                                                                                                               " & _
                                                               " ORDER BY TIPO", Ado)

                End Select

                AdapterMT.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                Try
                    DtsDatos.DATOS_REPORTES.Clear()
                    AdapterMT.Fill(DtsDatos.DATOS_REPORTES)
                    If DtsDatos.DATOS_REPORTES.Rows.Count = 0 Then
                        Alert("  SE RECOMIENDA VERIFICAR LOS DATOS DE PARAMETRIACION " & vbNewLine & _
                              "  NO SE ENCUENTRAN, " & vbNewLine & _
                              "  DEBE VERIFICAR LA FECHA DE EXPEDICION DE TITULO, " & vbNewLine & _
                              "  LA FECHA DE EJECUTORIA , Y SI POSEE EL TITULO EJECUTIVO " & vbNewLine & _
                              "  EN LA PESTAÑA DE INFORMACION GENERAL " & vbNewLine & _
                              "  EN LA PESTAÑA TITULO EJECUTIVO " & vbNewLine & _
                              "  VERIFICAR QUE ESTE INGRESADO  LA FORMA DE NOTIFICACION " & vbNewLine & _
                              "  DEL TITULO EN LA PESTAÑA INFORMACION GENERAL " & vbNewLine & _
                              "  EN LA PESTAÑA TITULO EJECUTIVO " & vbNewLine & _
                              "  DEUDORES  EN LA PESTAÑA INFORMACION GENERAL EN LA PESTAÑA DEUDOR ")

                        Exit Sub
                    End If
                Catch ex As Exception
                    Alert(ex.Message)
                End Try
                'saveResolucion(lblExpediente.Text.Trim, selectcod, DtsDatos)


                'Reportes administrativos Cobro Coactivo
                If selectcod = "013" Then

                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fecha_ejecutoria
                    If IsDBNull(fecha2) Then
                        Alert("NO SE HA INGRESADO FECHA DE EJECUTORIA DEL TITULO ")
                        Exit Sub
                    End If

                    Dim parametros(6) As WordReport.Marcadores_Adicionales

                    Dim resolucion() As String
                    'CARGA RESOLUCION VIEJA ' O revisa la resolucion si existe 
                    resolucion = saveResolucion(lblExpediente.Text, selectcod)
                    SaveTable(lblExpediente.Text, selectcod)
                    resolucion = saveResolucion(lblExpediente.Text, selectcod)


                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "Nro_Resolucion"
                    parametros(0).Valor = resolucion(0).ToUpper
                    parametros(1).Marcador = "fecha_actual"
                    parametros(1).Valor = resolucion(1).ToUpper
                    parametros(2).Marcador = "letras"
                    parametros(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    parametros(3).Marcador = "total_deudas"
                    parametros(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).Total_deuda
                    parametros(4).Marcador = "fecha1"
                    parametros(4).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(5).Marcador = "fecha_ejecutoria"
                    parametros(5).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(6).Marcador = "fecha_emision"
                    parametros(6).Valor = resolucion(3)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.MandamientoPagoPorPila, parametros)

                    ''SaveTable(lblExpediente.Text, selectcod)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        '' ModalPopupExtender4.Show()

                    End If


                End If

                If selectcod = "056" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        datos(1).Marcador = "nro_resolucion"
                        datos(1).Valor = resolucion(0).ToUpper
                        datos(2).Marcador = "fecha_reg"
                        datos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA IMPRIMIR EL DOCUMENTO" & Lista.SelectedItem.Text.ToUpper)
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionPorCorreo, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)



                    End If



                End If

                If selectcod = "228" Then

                    Dim datos_valor(1) As WordReport.Marcadores_Adicionales
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_valor(1).Marcador = "NRO"
                    datos_valor(1).Valor = DtsDatos.DATOS_REPORTES.Rows.Count

                    For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                        DtsDatos.DATOS_REPORTES.Item(i).Municipio = DtsDatos.DATOS_REPORTES.Item(i).MT_fec_expedicion_titulo.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        DtsDatos.DATOS_REPORTES.Item(i).MT_tipo_titulo = DtsDatos.DATOS_REPORTES.Item(i).MT_tipo_titulo & " " & DtsDatos.DATOS_REPORTES.Item(i).MT_nro_titulo
                        DtsDatos.DATOS_REPORTES.Item(i).Departamento = ""
                    Next

                    worddocresult = worddoc.CreateReportWithTable(DtsDatos.DATOS_REPORTES, Reportes.DevolucionTituloTesoreria, "MT_tipo_titulo,Municipio,ED_Nombre,Departamento,totaldeuda", datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If

                If selectcod = "229" Then

                    Dim buscar As String

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        buscar = 319
                    Else
                        buscar = 320
                    End If
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    If embargos <= 0 Then
                        Alert("Verificar si posee porcentaje de embargo para continuar con la generacion del reporte ")
                        Exit Sub
                    End If

                    Dim datos_valor(4) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = overloadresolucion(lblExpediente.Text.Trim, buscar)

                    For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                        datos_valor(0).Marcador = "ED_Codigo_Nit"
                        datos_valor(0).Valor = DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId & DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit
                        'DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit = DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId & DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit
                    Next

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    'Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "315")
                    'If vc_datos(0) <> Nothing Then
                    '    resolucion(0) = vc_datos(0)
                    '    resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy")
                    'Else

                    'End If
                    If resolucion(0) <> Nothing Then
                        datos_valor(1).Marcador = "XXXXXXXX"
                        datos_valor(1).Valor = resolucion(0)
                        datos_valor(2).Marcador = "XXXXXXXXX"
                        datos_valor(2).Valor = resolucion(1)
                    Else
                        Alert("NO SE HA GENERADO RESOLUCION DE EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO  ")
                        Exit Sub
                    End If
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, selectcod)
                    Dim guardar(3) As String
                    If vc_datos(0) = Nothing Or vc_datos(0).ToString = "" Or vc_datos(0).ToString.ToUpper.Trim = "SIN DATOS" Then
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    Else
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    End If


                    datos_valor(3).Marcador = "fecha_actual"
                    datos_valor(3).Valor = guardar(1).ToUpper
                    datos_valor(4).Marcador = "Nro_resolucion"
                    datos_valor(4).Valor = guardar(0).ToUpper
                    DtsDatos.DATOS_REPORTES.Item(0).MT_tipo_titulo = "RCC-" & resolucion(0).ToUpper & " " & resolucion(1)

                    DtsDatos.DATOS_REPORTES.Item(0).Total_deuda = String.Format("{0:C0}", (CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda) * (embargos / 100)))
                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, selectcod)

                    End If

                    worddocresult = worddoc.CreateReportWithTable(DtsDatos.DATOS_REPORTES, Reportes.LevantamientodeMedidaCautelar, "ED_NOMBRE,ED_Codigo_Nit,MT_tipo_titulo,Total_Deuda", datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "230" Or selectcod = "368" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo

                        Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, selectcod)
                        Dim guardar(3) As String
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            guardar(0) = vc_datos(0)
                            If vc_datos(1).ToString <> "" Then
                                guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                guardar(2) = vc_datos(1)
                                guardar(3) = vc_datos(2)
                            Else
                                guardar(1) = "SIN DATOS"
                                guardar(2) = "SIN DATOS"
                                guardar(3) = "SIN DATOS"

                            End If

                        Else
                            guardar = saveResolucion(lblExpediente.Text, selectcod)
                        End If

                        If vc_datos(0) = Nothing Then
                            SaveTable(lblExpediente.Text, selectcod)
                        End If

                        'datos detallado de la deuda
                        Dim tbl2 As New DataTable
                        tbl2.Columns.AddRange(New DataColumn() {New DataColumn("SUBSISTEMA", GetType(String)), _
                                                 New DataColumn("CAPITAL_1", GetType(String)), _
                                                 New DataColumn("INTERESES_1", GetType(String)), _
                                                 New DataColumn("TOTAL_A_PAGAR_1", GetType(String)), _
                                                 New DataColumn("CAPITAL", GetType(Double)), _
                                                 New DataColumn("INTERESES", GetType(Double)), _
                                                 New DataColumn("TOTAL_A_PAGAR", GetType(Double))})


                        Dim tb1 As New DataTable

                        Dim ad As New SqlDataAdapter("SELECT SUBSISTEMA, '' AS AJUSTE, '' AS INTERESES_MORATORIOS, '' AS SALDO_TOTAL, SUM( AJUSTE) AS CAPITAL ,A.MES,A.ANNO FROM  SQL_PLANILLA A , MAESTRO_MES B WHERE A.MES = B.ID_MES AND A.EXPEDIENTE = @EXPEDIENTE GROUP BY SUBSISTEMA, ANNO, B.NOMBRE,B.ID_MES,A.MES ORDER BY SUBSISTEMA,ANNO,B.ID_MES", Funciones.CadenaConexion)
                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)

                        ad.Fill(tb1)
                        If tb1.Rows.Count > 0 Then


                            'Obterner dia habiles de pago
                            Dim tb2 As New DataTable

                            ad = New SqlDataAdapter("SELECT A.DEUDOR,D.CODIGO FROM DEUDORES_EXPEDIENTES A , TIPOS_ENTES B, ENTES_DEUDORES C,TIPOS_APORTANTES D WHERE A.DEUDOR = C.ED_CODIGO_NIT AND A.TIPO = B.CODIGO AND C.ED_TIPOAPORTANTE = D.CODIGO AND B.CODIGO = '1' AND A.NROEXP = @EXPEDIENTE ", Funciones.CadenaConexion)
                            ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                            ad.Fill(tb2)


                            Dim diaPago As String = FuncionesInteresesParafiscales.ObtenerDiaPago(tb2.Rows(0).Item("DEUDOR").ToString, CInt(tb2.Rows(0).Item("CODIGO")))

                            Dim subsistema As String = ""
                            Dim subsistemaSig As String = ""
                            Dim c, i, t, x, sw As Double
                            c = 0
                            i = 0
                            t = 0
                            x = 0
                            sw = 0
                            For Each row As DataRow In tb1.Rows
                                subsistemaSig = row("SUBSISTEMA")
                                sw = sw + 1
                                Dim deudaCapital As Double = CDbl(row("CAPITAL"))
                                Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, row("ANNO"), row("MES"), CInt(tb2.Rows(0).Item("CODIGO")), row("SUBSISTEMA")).ToString("dd/MM/yyyy")
                                'Alterar grid colocar datos faltantes
                                row("INTERESES_MORATORIOS") = FuncionesInteresesParafiscales._CalcularIntereses(CDbl(row("CAPITAL")), fechaExigibilidad.ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"), CDec(ViewState("diaria")))
                                row("SALDO_TOTAL") = CDbl(row("INTERESES_MORATORIOS")) + deudaCapital


                                If subsistema = subsistemaSig Then
                                    x = 0
                                End If

                                'Agregar acumulado a la tabla que se va a mostrar en la minuta.
                                If x > 0 And subsistema <> subsistemaSig Then
                                    tbl2.Rows.Add(subsistema, "", "", "", c, i, t)
                                    c = 0
                                    i = 0
                                    t = 0
                                End If

                                ' Acumular montos 
                                c = c + row("CAPITAL")
                                i = i + row("INTERESES_MORATORIOS")
                                t = t + row("SALDO_TOTAL")
                                x = 1
                                subsistema = row("SUBSISTEMA")

                                'Agregar ultimo acumulado a la tabla que se va a mostrar en la minuta.
                                If sw = tb1.Rows.Count Then
                                    tbl2.Rows.Add(subsistema, "", "", "", c, i, t)
                                End If

                            Next
                            ''For n = 0 To tbl2.Rows.Count - 1

                            ''Next

                            'Agregar totales a la tabla
                            SaveTable(guardar(0), lblExpediente.Text, "Liquidacion Oficial", tbl2.Compute("SUM(CAPITAL)", String.Empty), tbl2.Compute("SUM(INTERESES)", String.Empty), tbl2.Compute("SUM(TOTAL_A_PAGAR)", String.Empty), guardar(2))
                            tbl2.Rows.Add("TOTALES", "", "", "", tbl2.Compute("SUM(CAPITAL)", String.Empty), tbl2.Compute("SUM(INTERESES)", String.Empty), tbl2.Compute("SUM(TOTAL_A_PAGAR)", String.Empty))

                            'Formatear datos a tipo moneda
                            For Each row As DataRow In tbl2.Rows
                                If IsDBNull(row("INTERESES")) = True Then
                                    row("INTERESES_1") = String.Format("{0:C0}", CDbl(0))
                                Else
                                    row("INTERESES_1") = String.Format("{0:C0}", CDbl(row("INTERESES")))
                                End If

                                If IsDBNull(row("TOTAL_A_PAGAR")) = True Then
                                    row("TOTAL_A_PAGAR_1") = String.Format("{0:C0}", CDbl(0))
                                Else
                                    row("TOTAL_A_PAGAR_1") = String.Format("{0:C0}", CDbl(row("TOTAL_A_PAGAR")))
                                End If

                                If IsDBNull(row("CAPITAL")) = True Then
                                    row("CAPITAL_1") = String.Format("{0:C0}", CDbl(0))
                                Else
                                    row("CAPITAL_1") = String.Format("{0:C0}", CDbl(row("CAPITAL")))
                                End If

                            Next
                        Else
                            Alert("NO HAY SQL PARA GENERAR EL REPORTE SOLICITADO ")
                            Exit Sub

                        End If


                        Dim datos(6) As WordReport.Marcadores_Adicionales
                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "fecha2"
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(2).Marcador = "fecha_actual"
                        datos(2).Valor = guardar(1).ToUpper
                        datos(3).Marcador = "letras"
                        datos(3).Valor = Num2Text(CInt(tbl2.Rows.Item(tbl2.Rows.Count - 1)(6))).ToUpper
                        'Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        datos(4).Marcador = "Nro_Resolucion"
                        datos(4).Valor = guardar(0)
                        datos(5).Marcador = "total"
                        datos(5).Valor = String.Format("{0:C0}", CDbl(tbl2.Rows.Item(tbl2.Rows.Count - 1)(6)))
                        datos(6).Marcador = "fecha_emision"
                        datos(6).Valor = guardar(3)

                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                        ''    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.LiquidacionCreditoCosta, datos)
                        worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.LiquidacionCreditoCosta, datos, tbl2, 0, False, Nothing, 0, False, Nothing, 0, False)
                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                        End If


                    Else

                        '---- DIFERENTES A LIQUIDACIONES OFICIALES...
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo

                        Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, selectcod)
                        Dim guardar(3) As String
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            guardar(0) = vc_datos(0)
                            If vc_datos(1) <> Nothing Or vc_datos(1).ToString <> "" Or vc_datos(1).ToString.ToUpper.Trim <> "SIN DATOS" Then
                                guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                guardar(2) = vc_datos(1)
                                guardar(3) = vc_datos(2)
                            Else
                                guardar(1) = "SIN DATOS"
                                guardar(2) = "SIN DATOS"
                                guardar(3) = "SIN DATOS"
                            End If

                        Else
                            guardar = saveResolucion(lblExpediente.Text, selectcod)
                        End If

                        If vc_datos(0) = Nothing Then
                            SaveTable(lblExpediente.Text, selectcod)
                        End If

                        Dim datos(9) As WordReport.Marcadores_Adicionales
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "fecha2"
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(2).Marcador = "fecha_actual"
                        datos(2).Valor = guardar(1).ToUpper
                        datos(3).Marcador = "letras"
                        datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                        datos(4).Marcador = "Nro_Resolucion"
                        datos(4).Valor = guardar(0)
                        datos(5).Marcador = "total"
                        datos(5).Valor = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda))

                        Dim diasMora As Integer = FuncionesInteresesMultas.CalcularDiasMoras(CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_exi_liq).ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"))
                        Dim intereses As Double = FuncionesInteresesMultas.CalcularInteresesMoras(CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda), diasMora)

                        datos(6).Marcador = "1_COLUMN"
                        datos(6).Valor = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda))
                        datos(7).Marcador = "2_COLUMN"
                        datos(7).Valor = String.Format("{0:C0}", CDbl(intereses))
                        datos(8).Marcador = "3_COLUMN"
                        datos(8).Valor = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda) + CDbl(intereses))
                        datos(9).Marcador = "fecha_emision"
                        datos(9).Valor = guardar(3)

                        SaveTable(guardar(0), lblExpediente.Text, "MULTA", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda), CDbl(intereses), CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda) + CDbl(intereses), guardar(2))


                        ''    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.LiquidacionCreditoCosta, datos)
                        worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.liquidaciondecreditomulta, datos, Nothing, 0, False, Nothing, 0, False, Nothing, 0, False)
                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                        End If

                    End If
                End If




                If selectcod = "233" Then

                    Dim datos_valor(8) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos(2) As String
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_valor(1).Marcador = "fecha2"
                    datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_valor(2).Marcador = "letras"

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    vc_datos = overloadresolucion(lblExpediente.Text, "230")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper

                        If vc_datos(1) = "" Then
                            resolucion(1) = "01 DE ENERO DEL 1900 "
                        Else
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper()


                        End If

                    Else

                    End If
                    datos_valor(2).Valor = Num2Text(GetLiquidacion(lblExpediente.Text.Trim, resolucion(0))).ToUpper
                    If resolucion(0) <> Nothing Then
                        datos_valor(3).Marcador = "nro_resolucion2"
                        datos_valor(3).Valor = resolucion(0).ToUpper
                        datos_valor(4).Marcador = "fecha_reg"
                        datos_valor(4).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY (230) RESOLUCION DE LIQUIDACION DE CREDITO Y COSTAS PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If
                    vc_datos = overloadresolucion(lblExpediente.Text, selectcod)
                    Dim guardar(3) As String
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0)
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy")
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If

                    datos_valor(5).Marcador = "fecha_actual"
                    datos_valor(5).Valor = guardar(1).ToUpper
                    datos_valor(6).Marcador = "Nro_resolucion"
                    datos_valor(6).Valor = guardar(0).ToUpper
                    datos_valor(7).Marcador = "Total_Liquidacion"
                    datos_valor(7).Valor = String.Format("{0:C0}", GetLiquidacion(lblExpediente.Text.Trim, resolucion(0)))
                    datos_valor(8).Marcador = "fecha_emision"
                    datos_valor(8).Valor = guardar(3)


                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, selectcod)

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.Aprobaciónliquidacióndelcrédito, datos_valor)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If


                If selectcod = "314" Then
                    Dim parametros(7) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    Dim Excepciones() As String = GetExcepcion(lblExpediente.Text.Trim)
                    If Excepciones(0) = "" Then
                        Alert("NO HAY RADICADO DE PARA GENERAR INFORME DE EXCEPCIONES ")
                        Exit Sub
                    End If
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "fecha1"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(1).Marcador = "Letras"
                    parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    parametros(2).Marcador = "fecha2"
                    parametros(2).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper


                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper


                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametros(3).Marcador = "nro_resolucion"
                        parametros(3).Valor = resolucion(0).ToUpper
                        parametros(4).Marcador = "fecha_reg"
                        parametros(4).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO  PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, selectcod)
                    Dim guardar(3) As String
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If


                    parametros(5).Marcador = "Nro_res"
                    parametros(5).Valor = guardar(0).ToUpper
                    parametros(6).Marcador = "fecha_actual"
                    parametros(6).Valor = guardar(1).ToUpper
                    parametros(7).Marcador = "fecha_emision"
                    parametros(7).Valor = guardar(3)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AperturaDePruebasLiquidacion, parametros)
                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, selectcod)

                    End If
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "315" Then
                    Dim parametros(6) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "fecha1"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(1).Marcador = "Letras"
                    parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    parametros(2).Marcador = "fecha2"
                    parametros(2).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    vc_datos = overloadresolucion(lblExpediente.Text, "230")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametros(3).Marcador = "resolucion_anterior"
                        parametros(3).Valor = resolucion(0).ToUpper
                        parametros(4).Marcador = "fecha_anterior"
                        parametros(4).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION (230) LIQUIDACION CREDITO Y COSTAS DE PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    vc_datos = overloadresolucion(lblExpediente.Text, selectcod)
                    Dim guardar(3) As String
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If

                    parametros(5).Marcador = "fecha_actual"
                    parametros(5).Valor = guardar(1).ToUpper
                    parametros(6).Marcador = "Nro_resolucion"
                    parametros(6).Valor = guardar(0).ToUpper
                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, selectcod)

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AprobacionLiquidacionCreditoLiquidacionOficial, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "317" Then
                    Dim parametro(1) As WordReport.Marcadores_Adicionales
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_resolucion"
                        parametro(0).Valor = resolucion(0).ToUpper
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CitacionMandamientoPago, parametro)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "319" Then

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        Dim tbResoluciones As DataTable
                        Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                        If datos.Rows.Count > 0 Then

                            tbResoluciones = datos
                        Else
                            tbResoluciones = GetList_Embargos2("320", lblExpediente.Text.Trim)
                        End If


                        If tbResoluciones.Rows.Count > 0 Then
                            list_embargos.Items.Clear()
                            For Each row As DataRow In tbResoluciones.Rows
                                list_embargos.Items.Add(row("NRORESOLEM"))
                            Next


                            ModalPopupExtender2.Show()
                        Else
                            Limite.Text = "200"
                            ModalPopupExtender3.Show()
                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS EJECUTIVOS DE LIQUIDACION OFICIAL")
                        Exit Sub
                    End If
                    Exit Sub
                End If



                If selectcod = "320" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        Dim tbResoluciones As DataTable
                        Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                        If datos.Rows.Count > 0 Then

                            tbResoluciones = datos
                        Else
                            tbResoluciones = GetList_Embargos2("320", lblExpediente.Text.Trim)
                        End If

                        If tbResoluciones.Rows.Count > 0 Then
                            list_embargos.Items.Clear()
                            For Each row As DataRow In tbResoluciones.Rows
                                list_embargos.Items.Add(row("NRORESOLEM"))
                            Next

                            ModalPopupExtender2.Show()
                        Else
                            Limite.Text = "150"
                            ModalPopupExtender3.Show()

                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS EJECUTIVOS DE MULTA ")
                        Exit Sub
                    End If
                    Exit Sub


                End If


                If selectcod = "321" Then


                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        Dim parametros(5) As WordReport.Marcadores_Adicionales
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                        Dim vc_datos() As String
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        parametros(0).Marcador = "fecha_reg"
                        parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        parametros(1).Marcador = "Letras"
                        parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion(0) <> Nothing Then
                            parametros(2).Marcador = "nro_resolucion"
                            parametros(2).Valor = resolucion(0).ToUpper
                            parametros(3).Marcador = "fecha_anterior"
                            parametros(3).Valor = resolucion(1).ToUpper
                        Else
                            parametros(2).Marcador = "nro_resolucion"
                            parametros(2).Valor = ""
                            parametros(3).Marcador = "fecha_anterior"
                            parametros(3).Valor = ""
                        End If

                        vc_datos = overloadresolucion(lblExpediente.Text, selectcod)
                        Dim guardar(1) As String
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            guardar(0) = vc_datos(0).ToUpper
                            guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else
                            guardar = saveResolucion(lblExpediente.Text, selectcod)
                        End If

                        parametros(4).Marcador = "fecha_actual"
                        parametros(4).Valor = guardar(1).ToUpper
                        parametros(5).Marcador = "Nro_resolucion"
                        parametros(5).Valor = guardar(0).ToUpper
                        SaveTable(lblExpediente.Text.Trim, selectcod, guardar(0), CDate(guardar(2)), DtsDatos.DATOS_REPORTES.Item(0).totaldeuda, DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.EmbargoCuentaBancariaMultaHospitales, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS JUDICIALES DE MULTA ")
                        Exit Sub
                    End If


                End If
                If selectcod = "322" Then
                    Dim parametros(1) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    vc_datos = overloadresolucion(lblExpediente.Text, "314")
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametros(0).Marcador = "nro_resolucion"
                        parametros(0).Valor = resolucion(0).ToUpper
                        parametros(1).Marcador = "fecha_reg"
                        parametros(1).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE ABRE A PRUEBAS-LIQUIDACION PARA IMPRIMIR EL DOCUMENTO ")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.MemorandoIntegracionPruebas, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "323" Then
                    Dim parametros(5) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)

                    Dim titulo() As String = GetTituloPrincipal(lblExpediente.Text.Trim)
                    If titulo(0) = "" Then
                        Alert("NO HAY TITULO DE DEPOSITO JUDICIAL Y/O RESOLUCION DE FRACCIONAMIENTO DE TITULO")

                        Exit Sub
                    End If

                    Dim titulo2() As Double = getfraccionado(titulo(0))
                    If titulo2(0) = "" Then
                        Alert("NO HAY TITULO DE DEPOSITO JUDICIAL Y/O RESOLUCION DE FRACCIONAMIENTO DE TITULO")
                        Exit Sub
                    End If
                    parametros(0).Marcador = "resolucion_anterior"
                    parametros(0).Valor = resolucion(0).ToUpper
                    parametros(1).Marcador = "fecha_anterior"
                    parametros(1).Valor = resolucion(1).ToUpper
                    parametros(2).Marcador = "titulo_principal"
                    parametros(2).Valor = titulo(0).ToUpper
                    parametros(3).Marcador = "titulo1"
                    parametros(3).Valor = Num2Text(titulo2(0)) & " " & String.Format("{0:C0}", titulo2(0)).ToUpper
                    parametros(4).Marcador = "titulo2"
                    parametros(4).Valor = Num2Text(titulo2(1)) & " " & String.Format("{0:C0}", titulo2(1)).ToUpper
                    parametros(5).Marcador = "Total_Titulo"
                    parametros(5).Valor = String.Format("{0:C0}", CDbl(titulo(1)))
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.FraccionamientoTituloDepositoJudicial, parametros)

                    If worddocresult = "" Then

                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "324" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    Dim parametros(4) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    parametros(0).Marcador = "fecha1"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    If resolucion(0) <> Nothing Then
                        parametros(1).Marcador = "nro_resolucion"
                        parametros(1).Valor = resolucion(0).ToUpper
                        parametros(2).Marcador = "fecha_reg"
                        parametros(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE TERMINACION DE PROCESO PARA GENERAR LA DEVOLUCION DE TITULO  " & Lista.Text & "")
                        Exit Sub
                    End If
                    parametros(3).Marcador = "numerofolio"
                    parametros(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Count).ToUpper
                    parametros(4).Marcador = "folios"
                    parametros(4).Valor = DtsDatos.DATOS_REPORTES.Count
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.MemorandoDevolucionTitulo, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "325" Then
                    Dim sdatos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    sdatos(0).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        sdatos(0).Valor = representante(0)
                    Else
                        sdatos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "314")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion(0) <> Nothing Then
                        sdatos(1).Marcador = "nro_resolucion"
                        sdatos(1).Valor = resolucion(0).ToUpper
                        sdatos(2).Marcador = "fecha_reg"
                        sdatos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION ABRE A PRUEBAS  DE PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NOTIFICACIONABREAPRUEBAS, sdatos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "326" Then
                    Dim sdatos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    sdatos(0).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        sdatos(0).Valor = representante(0)
                    Else
                        sdatos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "315")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        If vc_datos(1) = "" Then
                            resolucion(1) = "01 DE ENERO DEL 1900 "
                        Else
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper()
                        End If


                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        sdatos(1).Marcador = "nro_resolucion"
                        sdatos(1).Valor = resolucion(0).ToUpper
                        sdatos(2).Marcador = "fecha_reg"
                        sdatos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE APROBACIÓN DE LIQUIDACIÓN DEL CRÉDITO - LIQUIDACIÓN OFICIAL PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NOTIFICACIONAPROBACIÓNLIQUIDACIÓNDELCRÉDITO, sdatos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "327" Then

                    Dim sdatos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    sdatos(0).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        sdatos(0).Valor = representante(0)
                    Else
                        sdatos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "316")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        sdatos(1).Marcador = "nro_resolucion"
                        sdatos(1).Valor = resolucion(0).ToUpper
                        sdatos(2).Marcador = "fecha_reg"
                        sdatos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE  TERMINACION Y ARCHIVO PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NOTIFICACIÓNDETERMINACIÓNDELPROCESO, sdatos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "328" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    datos(0).Marcador = "ED_Rep"
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    If representante(0) <> Nothing Then
                        datos(0).Valor = representante(0).ToUpper
                    Else
                        datos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre.ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    'Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "320")
                    'If vc_datos(0) <> Nothing Then
                    '    resolucion(0) = vc_datos(0)
                    '    resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    'Else

                    'End If


                    If resolucion(0) <> Nothing Then
                        datos(1).Marcador = "nro_resolucion"
                        datos(1).Valor = resolucion(0).ToUpper
                        datos(2).Marcador = "fecha_reg"
                        datos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE  (230) LIQUIDACION CREDITO Y COSTAS PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionLiquidacionCredito, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "346" Then

                    Dim datos(1) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "351")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        datos(0).Marcador = "nro_resolucion"
                        datos(0).Valor = resolucion(0).ToUpper
                        datos(1).Marcador = "fecha_reg"
                        datos(1).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE  ORDEN DE EJECUCIÓN MULTA  Ò ORDEN DE EJECUCIÓN PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionOrdenEjecucion, datos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "347" Then
                    Dim datos(1) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "359")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion(0) <> Nothing Then
                        datos(0).Marcador = "nro_resolucion"
                        datos(0).Valor = resolucion(0).ToUpper
                        datos(1).Marcador = "fecha_reg"
                        datos(1).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE RESUELVE EXCEPCIONES PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionResuelveExepciones, datos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "348" Then

                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                    Dim tbl1 As DataTable = GetTituloPrincipal3(lblExpediente.Text.Trim)
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    datos(0).Marcador = "ED_Rep"

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "348")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        datos(2).Marcador = "nro_devolucion"
                        datos(2).Valor = resolucion(0).ToUpper
                        datos(3).Marcador = "fecha_devolucion"
                        datos(3).Valor = resolucion(1).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE DEVOLUCION DE TITULO DE DEPOSITO JUDICIAL PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    If representante(0) <> Nothing Then
                        datos(0).Valor = representante(0).ToUpper
                    Else
                        datos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre.ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    datos(1).Marcador = "fecha1"
                    datos(1).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.OficioComunicacionDevolucionTituloDepositoJudicial, datos, tbl1, 0, False, Nothing, 0, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "349" Then
                    ''   If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then


                    Dim tbResoluciones As DataTable
                    Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                    If datos.Rows.Count > 0 Then

                        tbResoluciones = datos
                    Else
                        tbResoluciones = GetList_Embargos2("319", lblExpediente.Text.Trim)
                    End If


                    If tbResoluciones.Rows.Count > 0 Then
                        list_embargos.Items.Clear()
                        For Each row As DataRow In tbResoluciones.Rows
                            list_embargos.Items.Add(row("NRORESOLEM"))
                        Next
                        ModalPopupExtender2.Show()
                    Else
                        ''ModalPopupExtender3.Show()
                    End If
                    Exit Sub
                Else
                    ''End If

                End If

                If selectcod = "380" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo <> "01" Then

                        Dim tbResoluciones As DataTable
                        Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                        If datos.Rows.Count > 0 Then

                            tbResoluciones = datos
                        Else
                            tbResoluciones = GetList_Embargos2("320", lblExpediente.Text.Trim)
                        End If


                        If tbResoluciones.Rows.Count > 0 Then
                            list_embargos.Items.Clear()
                            For Each row As DataRow In tbResoluciones.Rows
                                list_embargos.Items.Add(row("NRORESOLEM"))
                            Next
                            ModalPopupExtender2.Show()
                        Else
                            ''ModalPopupExtender3.Show()
                        End If
                        Exit Sub
                    Else

                    End If


                End If




                If selectcod = "350" Then
                    Dim parametros(5) As WordReport.Marcadores_Adicionales
                    Dim busqueda As String
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = 1 Then
                        busqueda = "319"
                    Else
                        busqueda = "320"

                    End If

                    Dim resolucion() As String = overloadresolucion(lblExpediente.Text, busqueda)

                    Dim vc_datos() As String
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "fecha_reg"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    parametros(1).Marcador = "Letras"
                    parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    If resolucion(0) <> Nothing Then
                        parametros(2).Marcador = "nro_resolucion"
                        parametros(2).Valor = resolucion(0).ToUpper
                        parametros(3).Marcador = "fecha_resolucion"
                        parametros(3).Valor = resolucion(1).ToUpper
                    Else
                        parametros(2).Marcador = "nro_resolucion"
                        parametros(2).Valor = ""
                        parametros(3).Marcador = "fecha_resolucion"
                        parametros(3).Valor = ""
                    End If
                    vc_datos = overloadresolucion(lblExpediente.Text, selectcod)
                    Dim guardar(3) As String
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If
                    parametros(4).Marcador = "Nro_Resolucion"
                    parametros(4).Valor = guardar(0).ToUpper

                    parametros(5).Marcador = "fecha_actual"
                    parametros(5).Valor = guardar(1).ToUpper

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ReduccionDeEmbargosPorPagoParcial, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "351" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        Dim parametros(5) As WordReport.Marcadores_Adicionales
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                        Dim vc_datos() As String
                        parametros(0).Marcador = "letras"
                        parametros(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If

                        If resolucion(0) <> Nothing Then
                            parametros(1).Marcador = "fecha_anterior"
                            parametros(1).Valor = resolucion(1).ToUpper
                            parametros(2).Marcador = "resolucion_anterior"
                            parametros(2).Valor = resolucion(0).ToUpper
                        Else
                            Alert("NO HAY RESOLUCION DE (013) MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO")
                            Exit Sub

                        End If

                        vc_datos = overloadresolucion(lblExpediente.Text, selectcod)
                        Dim guardar(3) As String
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            guardar(0) = vc_datos(0)
                            guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy")
                            guardar(2) = vc_datos(1)
                            guardar(3) = vc_datos(2)
                        Else
                            guardar = saveResolucion(lblExpediente.Text, selectcod)
                        End If

                        parametros(3).Marcador = "Nro_Resolucion"
                        parametros(3).Valor = guardar(0).ToUpper

                        parametros(4).Marcador = "fecha_actual"
                        parametros(4).Valor = guardar(1).ToUpper

                        parametros(5).Marcador = "fecha_emision"
                        parametros(5).Valor = guardar(3)
                        If vc_datos(0) = Nothing Then
                            SaveTable(lblExpediente.Text, selectcod)

                        End If
                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.OrdenEjecucionLiquidacionOficial, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If

                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS EJECUTIVOS DE LIQUIDACION OFICIAL")
                        Exit Sub
                    End If
                End If

                If selectcod = "352" Then

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                        Dim parametros(10) As WordReport.Marcadores_Adicionales
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                        Dim vc_datos() As String
                        parametros(2).Marcador = "letras"
                        parametros(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If

                        If resolucion(0) <> Nothing Then
                            parametros(3).Marcador = "fecha_anterior"
                            parametros(3).Valor = resolucion(1).ToUpper

                            parametros(4).Marcador = "resolucion_anterior"
                            parametros(4).Valor = resolucion(0).ToUpper
                        Else
                            Alert("NO REGISTRADA RESOLUCION DE (013) MANDAMIENTO DE PAGO PARA GENERAR ESTE DOCUMENTO")
                        End If

                        vc_datos = overloadresolucion(lblExpediente.Text, selectcod)

                        Dim guardar(3) As String
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            guardar(0) = vc_datos(0)
                            guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy")
                            guardar(2) = vc_datos(1)
                            guardar(3) = vc_datos(2)
                        Else
                            guardar = saveResolucion(lblExpediente.Text, selectcod)
                        End If


                        parametros(0).Marcador = "Nro_Resolucion"
                        parametros(0).Valor = guardar(0).ToUpper

                        parametros(1).Marcador = "fecha_actual"
                        parametros(1).Valor = guardar(1).ToUpper

                        If Valores(2) <> "" Then
                            parametros(5).Marcador = "acta_numero"
                            parametros(5).Valor = Valores(2).ToUpper

                            parametros(6).Marcador = "tipo_notificacion"
                            parametros(6).Valor = Valores(1).ToUpper

                            parametros(7).Marcador = "acta_dia"
                            parametros(7).Valor = Valores(0).ToUpper
                        Else
                            parametros(5).Marcador = "acta_numero"
                            parametros(5).Valor = ""

                            parametros(6).Marcador = "tipo_notificacion"
                            parametros(6).Valor = ""

                            parametros(7).Marcador = "acta_dia"
                            parametros(7).Valor = ""
                        End If
                        parametros(8).Marcador = "Nsalario"
                        parametros(8).Valor = Num2Text(Round((DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0))
                        parametros(9).Marcador = "nsalario_minimo"
                        parametros(9).Valor = Round((DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                        parametros(10).Marcador = "fecha_emision"
                        parametros(10).Valor = guardar(3)

                        If vc_datos(0) = Nothing Then
                            SaveTable(lblExpediente.Text, selectcod)

                        End If
                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.OrdenEjecucionMulta, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS JUDICIALES DE MULTA ")
                        Exit Sub
                    End If

                End If

                If selectcod = "353" Then
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim datos_individual(4) As WordReport.Marcadores_Adicionales
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, homologo(selectcod))
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0).ToUpper
                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    datos_individual(2).Marcador = "salario"
                    datos_individual(2).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = guardar(1).ToUpper
                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = guardar(3)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConPagoEnFosygaYPila, datos_individual)

                    SaveTable(lblExpediente.Text.Trim, homologo(selectcod))


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "354" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    Dim datos_individual(4) As WordReport.Marcadores_Adicionales
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, selectcod)
                    Dim guardar(3) As String
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0).ToUpper
                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    datos_individual(2).Marcador = "fecha_actual"
                    datos_individual(2).Valor = guardar(1).ToUpper

                    datos_individual(3).Marcador = "fecha1"
                    datos_individual(3).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = guardar(3)
                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, selectcod)

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConPagoEnFosyga, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "355" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim datos_individual(4) As WordReport.Marcadores_Adicionales
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, homologo(selectcod))
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0).ToUpper

                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    datos_individual(2).Marcador = "fecha1"
                    datos_individual(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = guardar(1).ToUpper

                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = guardar(3)

                    SaveTable(lblExpediente.Text.Trim, homologo(selectcod))


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConRecursoModificatorio, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "356" Then
                    
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, homologo(selectcod))
                    Dim datos_individual(7) As WordReport.Marcadores_Adicionales
                    '---- DATOS DE CUADRO_1
                    Dim tbl_deudores As New DataTable
                    Dim sql_deuda As SqlCommand
                    sql_deuda = New SqlCommand(" SELECT 0 AS CUENTA, " & _
                                " ED_NOMBRE,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP =@EXPEDIENTE AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)

                    sql_deuda.CommandType = CommandType.Text
                    sql_deuda.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)

                    Dim sql_deudores As New SqlDataAdapter(sql_deuda)
                    sql_deudores.Fill(tbl_deudores)
                    If tbl_deudores.Rows.Count > 0 Then
                        For I = 0 To tbl_deudores.Rows.Count - 1
                            tbl_deudores.Rows(I).Item(0) = I + 1

                        Next
                    Else
                        Alert("NO HAY DEUDORES SOLIDARIOS PARA GENERAR EL INFORME")
                        Exit Sub
                    End If
                    '--- DATOS DE CUADRO_2
                    Dim tbl_deudores2 As New DataTable
                    Dim sql_deuda2 As SqlCommand
                    sql_deuda2 = New SqlCommand(" SELECT 0 AS CUENTA, " & _
                                " ED_NOMBRE,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION , '' AS VALORES,'' AS LETRAS  FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP =@EXPEDIENTE AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)


                    sql_deuda2.CommandType = CommandType.Text
                    sql_deuda2.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                    Dim sumavalores As Double
                    Dim sql_deudores2 As New SqlDataAdapter(sql_deuda2)
                    sql_deudores2.Fill(tbl_deudores2)
                    If tbl_deudores2.Rows.Count > 0 Then
                        For I = 0 To tbl_deudores2.Rows.Count - 1
                            tbl_deudores2.Rows(I).Item("CUENTA") = I + 1
                            tbl_deudores2.Rows(I).Item("VALORES") = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (tbl_deudores2.Rows(I).Item("PARTICIPACION") / 100)))
                            tbl_deudores2.Rows(I).Item("LETRAS") = Num2Text(tbl_deudores2.Rows(I).Item("VALORES"))
                            sumavalores = sumavalores + CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (tbl_deudores2.Rows(I).Item("PARTICIPACION") / 100))

                        Next
                    Else
                        Alert("NO HAY DEUDORES SOLIDARIOS PARA GENERAR EL INFORME")
                        Exit Sub
                    End If

                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0).ToUpper

                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    datos_individual(2).Marcador = "fecha1"
                    datos_individual(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = guardar(1).ToUpper

                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = guardar(3)

                    datos_individual(5).Marcador = "fecha_ejecutoria"
                    datos_individual(5).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fecha_ejecutoria).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    SaveTable(lblExpediente.Text.Trim, homologo(selectcod))

                    datos_individual(6).Marcador = "total_socios"
                    datos_individual(6).Valor = String.Format("{0:C0}", sumavalores)
                    datos_individual(7).Marcador = "Socios"
                    datos_individual(7).Valor = Num2Text(sumavalores)

                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text.Trim, "363")
                    If vc_datos(0) = "" And vc_datos(0) = Nothing Then
                        Dim sqltrass As SqlTransaction
                        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
                        If sqlconfig.State = ConnectionState.Open Then
                            sqlconfig.Close()
                        End If
                        sqlconfig.Open()

                        sqltrass = sqlconfig.BeginTransaction
                        Dim sqldatas As New SqlCommand

                        Try
                            sqldatas = New SqlCommand("INSERT INTO DOCUMENTOS_GENERADOS(DG_COD_ACTO,DG_FECHA_DOC,DG_NRO_DOC,DG_EXPEDIENTE,DG_ESTADO) VALUES(@ACTO,@FECHA,@DOCUMENTO,@EXPEDIENTE,1)", sqlconfig)
                            sqldatas.Transaction = sqltrass
                            sqldatas.Parameters.AddWithValue("@ACTO", "363")
                            sqldatas.Parameters.AddWithValue("@FECHA", guardar(2))
                            sqldatas.Parameters.AddWithValue("@DOCUMENTO", guardar(0))
                            sqldatas.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                            sqldatas.ExecuteNonQuery()

                            sqldatas = New SqlCommand("UPDATE COACTIVO SET RESVINDEUDORSOL = @DOCUMENTO , FECVINDEUDORSOL =@FECHA WHERE NROEXP =@EXPEDIENTE ", sqlconfig)
                            sqldatas.Transaction = sqltrass
                            sqldatas.Parameters.AddWithValue("@FECHA", guardar(2))
                            sqldatas.Parameters.AddWithValue("@DOCUMENTO", guardar(0))
                            sqldatas.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                            sqldatas.ExecuteNonQuery()

                            sqltrass.Commit()
                        Catch ex As Exception
                            sqltrass.Rollback()
                        End Try

                        sqlconfig.Close()

                    Else

                    End If


                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConSocios, datos_individual, tbl_deudores, 0, False, tbl_deudores2, 1, True, Nothing, 0, False)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "358" Then
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    Dim datos_individual(6) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    Dim embargo As String = resolucion(2)
                    Dim resolucion2() As String = getResolucion_anterior(lblExpediente.Text, embargo)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    vc_datos = overloadresolucion(lblExpediente.Text, "229")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "319")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion2(0) = vc_datos(0)
                        resolucion2(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then

                        datos_individual(0).Marcador = "nro_resolucion"
                        datos_individual(0).Valor = resolucion(0)
                        datos_individual(1).Marcador = "fecha_resolucion"
                        datos_individual(1).Valor = resolucion(1)
                        datos_individual(2).Marcador = "nro_embargo"
                        datos_individual(2).Valor = resolucion2(0)
                        datos_individual(3).Marcador = "fecha_embargo"
                        datos_individual(3).Valor = resolucion2(1)

                    Else
                        Alert("NO HAY RESOLUCION DE LEVANTAMIENTO DE MEDIDAS CAUTELARES PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    datos_individual(4).Marcador = "letras"
                    datos_individual(4).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargos / 100))
                    datos_individual(5).Marcador = "Total_Embargo"
                    datos_individual(5).Valor = String.Format("{0:C2}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargos / 100)))
                    datos_individual(6).Marcador = "embargabilidad"
                    datos_individual(6).Valor = embargos

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoPlanillaLevantamientoEmbargo, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "370" Then
                    Dim datos_individual(8) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                    Dim Excepciones() As String = GetExcepcion(lblExpediente.Text.Trim)
                    If Excepciones(0) = "" Then
                        Alert("NO HAY RADICADO DE PARA GENERAR INFORME DE EXCEPCIONES ")
                        Exit Sub
                    End If
                    Dim vc_datos(2) As String
                    vc_datos = overloadresolucion(lblExpediente.Text.Trim, "370")
                    If vc_datos(0) <> "" Then
                        guardar(0) = vc_datos(0)
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    End If

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    datos_individual(0).Marcador = "nro_resolucion"
                    datos_individual(0).Valor = guardar(0)

                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    vc_datos = overloadresolucion(lblExpediente.Text.Trim, "013")

                    If vc_datos(0) <> "" Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If


                    If resolucion(0) = "" Then
                        Alert("NO HAY REGISTRA MANDAMIENTO DE PAGO ")
                    Else

                    End If
                    datos_individual(2).Marcador = "fecha_anterior"
                    datos_individual(2).Valor = resolucion(1)

                    datos_individual(3).Marcador = "resolucion_anterior"
                    datos_individual(3).Valor = resolucion(0)
                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = guardar(1)
                    If Valores(2) <> Nothing Then

                        datos_individual(5).Marcador = "acta_numero"
                        datos_individual(5).Valor = Valores(2)

                        datos_individual(6).Marcador = "tipo_notificacion"
                        datos_individual(6).Valor = Valores(1)

                        datos_individual(7).Marcador = "acta_dia"
                        datos_individual(7).Valor = Valores(0).ToUpper

                    Else
                        datos_individual(5).Marcador = "acta_numero"
                        datos_individual(5).Valor = "0"

                        datos_individual(6).Marcador = "tipo_notificacion"
                        datos_individual(6).Valor = "0"


                        datos_individual(7).Marcador = "acta_dia"
                        datos_individual(7).Valor = "0"

                    End If

                    datos_individual(8).Marcador = "fecha_emision"
                    datos_individual(8).Valor = guardar(3)
                    SaveTable(lblExpediente.Text, selectcod)






                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ResuelveySuspendeproceso, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                '*********** FIN REPORTES DE COBRO COACTIVO ********************




                'Se devuelve el campo en vaciosi no posee el representante legal'***** solicitado Por Rafael 
                If selectcod = "217" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)


                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        datos(2).Marcador = "fecha1"
                        datos(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If
                    Else
                        Alert("NO SE ENCUENTRA TITULO EJECUTIVO  Y/O FECHA DEL TITULO ")
                        Exit Sub
                    End If

                    ''For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                    'DtsDatos.DATOS_REPORTES.Item(i).ED_TipoPersona = getTitulo(DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId, DtsDatos.DATOS_REPORTES.Item(i).Tipo_Deudor)
                    'DtsDatos.DATOS_REPORTES.Item(i).totaldeuda = Format(DtsDatos.DATOS_REPORTES.Item(i).totaldeuda, "#,##0.00")
                    'Next

                    datos(0).Marcador = "ED_Rep"
                    If representante(0) <> Nothing Then
                        datos(0).Valor = representante(0)
                    Else
                        datos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(1).Marcador = "Letras"
                    datos(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioDeCobroPersuasivoomisos, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe

                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        ''SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "218" Then
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo


                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else

                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioDeCobroPersuasivoomisos, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If

                If selectcod = "219" Then
                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        If DtsDatos.DATOS_REPORTES.Count > 0 Then
                            fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                            If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                                DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                                DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                            End If

                        Else
                            Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                            Exit Sub
                        End If


                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "ED_Rep"

                        If representante(0) <> Nothing Then
                            datos(1).Valor = representante(0)
                        Else
                            datos(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                            DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                        End If
                        datos(2).Marcador = "Letras"
                        datos(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        datos(3).Marcador = "fecha2"
                        If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                            fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                            datos(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        Else
                            datos(3).Valor = "NO REQUERIDO"
                        End If

                    Else
                        Alert("EL DOCUMENTO SOLO PUEDE SER GENERADO PARA MULTAS ")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioDeCobroPersuasivoFosiga, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If



                If selectcod = "220" Then
                    Dim datos(4) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        If DtsDatos.DATOS_REPORTES.Count > 0 Then

                            fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                            'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                            If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                                DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                                DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                            End If

                        Else
                            Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                            Exit Sub
                        End If

                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "fecha2"
                        If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                            fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                            datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        Else
                            datos(1).Valor = "NO REQUERIDO "
                        End If


                        datos(2).Marcador = "ED_Rep"

                        If representante(0) <> Nothing Then
                            datos(2).Valor = representante(0)
                        Else
                            datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                            DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                        End If
                        datos(3).Marcador = "Letras"
                        datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                        datos(4).Marcador = "Solicitados"

                        If DtsDatos.DATOS_REPORTES.Item(0).Documento = "00" Then

                            datos(4).Valor = ""
                        ElseIf DtsDatos.DATOS_REPORTES.Item(0).Documento = "01" Then

                            datos(4).Valor = "CERTIFICACIÓN DE INGRESOS DEBIDAMENTE EMITIDO POR PARTE DE UN CONTADOR TITULADO"
                        ElseIf DtsDatos.DATOS_REPORTES.Item(0).Documento = "02" Then

                            datos(4).Valor = "CERTIFICADO DE DISPONIBILIDAD PRESUPUESTAL"
                        ElseIf DtsDatos.DATOS_REPORTES.Item(0).Documento = "03" Then

                            datos(4).Valor = "LOS ESTADOS FINANCIEROS DEL ÚLTIMO AÑO GRAVABLE DE LA SOCIEDAD"


                        Else
                            Alert("ESTE DOCUMENTO SOLO SE PUEDE GENERAR PARA MULTAS ")
                            Exit Sub
                        End If
                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioDeCobroPersuasivoFosiga, datos)
                    End If



                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "221" Then
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo


                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO")
                        Exit Sub
                    End If

                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(1).Valor = representante(0)
                    Else
                        datos(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(2).Marcador = "Letras"
                    datos(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos(3).Marcador = "informante"
                    datos(3).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimeOficioPersuasivoMultaDirectoFosiga, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "223" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub

                    End If
                    Dim datos(4) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If



                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos(4).Marcador = "informante"
                    datos(4).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SolicitudDocumentosPago, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "224" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.VerificacionPagoAprobado, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "225" Then
                    Dim datos_individual(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    datos_individual(0).Marcador = "Fecha_Actual"
                    datos_individual(0).Valor = Today.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_individual(1).Marcador = "Ejecutor"
                    datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                    datos_individual(2).Marcador = "Cargo"
                    datos_individual(2).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                    datos_individual(3).Marcador = "Nro"
                    datos_individual(3).Valor = DtsDatos.DATOS_REPORTES.Rows.Count

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CambioEstadoExpedienteIndividual, datos_individual)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "226" Then

                    Dim datos_valor(12) As WordReport.Marcadores_Adicionales
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                    Dim oficio() As String = GETPERSUASIVO(lblExpediente.Text.Trim)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo


                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo > Nothing Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO "
                    End If

                    datos_valor(2).Marcador = "letras"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "fecha_actual"
                    datos_valor(3).Valor = guardar(1)
                    datos_valor(4).Marcador = "salario"

                    datos_valor(4).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    datos_valor(5).Marcador = "Nro_Resolucion"
                    datos_valor(5).Valor = guardar(0)
                    datos_valor(6).Marcador = "fecha3"
                    datos_valor(6).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).EFIFECHAEXP).ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(7).Marcador = "estados"

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        datos_valor(7).Valor = "Determino"
                    Else
                        datos_valor(7).Valor = "Impuso"
                    End If


                    datos_valor(8).Marcador = "oficio1"
                    datos_valor(8).Valor = oficio(0)

                    datos_valor(9).Marcador = "fechaof1"
                    datos_valor(9).Valor = oficio(1).ToUpper

                    datos_valor(10).Marcador = "oficio2"
                    If oficio(2) = "" Then
                        datos_valor(10).Valor = ""
                    Else

                        datos_valor(10).Valor = oficio(2)
                    End If


                    datos_valor(11).Marcador = "fechaof2"
                    If oficio(3) = "del 01 de enero de 1900" Then
                        datos_valor(11).Valor = "NO ENVIADO "
                    Else
                        datos_valor(11).Valor = oficio(3)

                    End If

                    datos_valor(12).Marcador = "fecha_emision"
                    datos_valor(12).Valor = guardar(3)

                    SaveTable(lblExpediente.Text, selectcod)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AutoterminacionyArchivo, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "244" Then
                    Dim parametro(1) As WordReport.Marcadores_Adicionales
                    If DtsDatos.DATOS_REPORTES.Rows.Count > 0 Then
                        parametro(0).Marcador = "Deuda_Letras"
                        parametro(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        parametro(1).Marcador = "Deuda_Valor"
                        parametro(1).Valor = FormatCurrency(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    Else
                        Alert("NO HAY DATOS ASOCIADOS A ESTE PROCESO ")
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PresentacionDelCreditoCuotasPartes, parametro)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "." & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "245" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo = Nothing Then
                        Alert("no hay fecha de expedicion de titulo")
                        Exit Sub
                    Else
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    End If



                    Dim parametro(2) As WordReport.Marcadores_Adicionales
                    parametro(0).Marcador = "Letras"
                    parametro(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    parametro(1).Marcador = "Deuda_Valor"
                    parametro(1).Valor = FormatCurrency(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    parametro(2).Marcador = "fecha1"
                    parametro(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PresentacionDelCreditoParafiscal, parametro)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                    End If
                End If


                If selectcod = "301" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(1).Valor = representante(0)
                    Else
                        datos(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    datos(2).Marcador = "Letras"
                    datos(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos(3).Marcador = "fecha2"
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(3).Valor = "No requerida"
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioDeCobroPersuasivo, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If
                If selectcod = "302" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then


                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioDeCobroPersuasivo, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "303" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioCondenaJudicial, datos_valor)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "304" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioCondenaJudicial, datos_valor)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "305" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        '    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioMulta1607, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "306" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then


                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"

                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioMulta1607, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "307" Then
                    Dim datos_valor(4) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO ")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    datos_valor(1).Marcador = "valorL"
                    datos_valor(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(2).Marcador = "ED_Rep"


                    If representante(0) <> Nothing Then
                        datos_valor(2).Valor = representante(0)
                    Else
                        datos_valor(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    datos_valor(3).Marcador = "fecha2"
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(3).Valor = "NO REQUERIDO"
                    End If
                    datos_valor(4).Marcador = "informante"
                    datos_valor(4).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioMulta1438, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "308" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        '' fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioMulta1438, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "310" Then
                    Dim datos_valor(5) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo

                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If


                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    If resolucion(0) <> Nothing Then
                        datos_valor(4).Marcador = "nro_resolucion"
                        datos_valor(4).Valor = resolucion(0)
                        datos_valor(5).Marcador = "resolución_fecha"
                        datos_valor(5).Valor = resolucion(1)
                    Else
                        Alert("NO SE ENCUENTRA DOCUMENTO DE RESOLUCION DE TERMINACION ")
                        Exit Sub
                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ComunicacionTerminacionyArchivo, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "311" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "letras"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.RespuestaSolicitudPazSalvo, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "312" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.TrasladoPorCompetenciasDireccionParafiscales, datos)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "313" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If
                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ContestacionRevocatoriaDirecta, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If



                If selectcod = "318" Then

                    Dim datosrlegal(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, homologo("013"))
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If


                    datosrlegal(0).Marcador = "rlegal"

                    datosrlegal(1).Marcador = "ED_NIT"

                    If representante(0) <> Nothing Then
                        datosrlegal(0).Valor = representante(0)
                        datosrlegal(1).Valor = representante(1)
                    Else
                        datosrlegal(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        datosrlegal(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit = ""
                    End If
                    Dim tb1 As New DataTable
                    Dim sql_vinculo1 As SqlDataAdapter = New SqlDataAdapter(" SELECT ED_NOMBRE ," & _
                                 " ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT " & _
                                 " FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                 " WHERE  NROEXP ='" & lblExpediente.Text.Trim & "' AND TIPO ='2' " & _
                                 " AND   DEUDOR = ED_CODIGO_NIT ", Ado)
                    sql_vinculo1.Fill(tb1)
                    'For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                    '    DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit = DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId & DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit
                    'Next
                    If tb1.Rows.Count = 0 Then
                        Alert("NO SE ENCONTRARON REGISTROS DE DEUDORES SOLIDARIOS ")
                        Exit Sub
                    End If


                    If resolucion(0) <> Nothing Then
                        datosrlegal(2).Marcador = "nro_resolucion"
                        datosrlegal(2).Valor = resolucion(0)
                        datosrlegal(3).Marcador = "fecha_reg"
                        datosrlegal(3).Valor = resolucion(1)
                    Else
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA IMPRIMIR EL DOCUMENTO PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.CitacionMandamientoPagoSocios, datosrlegal, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "329" Then
                    Dim datos_valor(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    datos_valor(1).Marcador = "letras"
                    datos_valor(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(2).Valor = representante(0)
                    Else
                        datos_valor(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioCuotasPartes, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                    End If
                End If
                If selectcod = "330" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"
                    datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(2).Marcador = "letras"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioCuotasPartes, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "222" Then
                    Dim parametros(2) As WordReport.Marcadores_Adicionales
                    parametros(0).Marcador = "tasa1"
                    parametros(0).Valor = getPorcentaje(1)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        parametros(1).Marcador = "fecha1"
                        parametros(1).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    End If
                    parametros(2).Marcador = "Informantes"
                    parametros(2).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.TrasladoMinsterio, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else

                        Alert("NO HAY DATOS PARA GENERA ESTE REPORTE DE TRASLADO ")
                        Exit Sub
                    End If

                    'worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.TrasladoMinsterio)

                End If

                Dim tbTitulo As DataTable = Funciones.RetornaCargadatos("SELECT TOP 1 c.codigo, c.nombre FROM EJEFISGLOBAL A , MAESTRO_TITULOS B, TIPOS_TITULO C  WHERE A.EFINROEXP = B.MT_expediente AND b.MT_tipo_titulo = c.codigo and EFINROEXP = '" & lblExpediente.Text.Trim & "'")

                If selectcod = "332" Then

                    If tbTitulo.Rows.Count > 0 Then
                        Dim tipoTitulo As String = tbTitulo.Rows(0).Item("codigo")
                        If (tipoTitulo = "01") Or (tipoTitulo = "02") Or (tipoTitulo = "04") Then


                            Dim tb As New DataTable
                            Dim Adap As New SqlDataAdapter("SELECT SUM(AJUSTE) AS DEUDA, SUM(MONTO_PAGO) AS PAGOS FROM SQL_PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE", Funciones.CadenaConexion)
                            Adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                            Adap.Fill(tb)

                            If tb.Rows.Count > 0 Then

                                If tb.Rows(0).Item("PAGOS") >= tb.Rows(0).Item("DEUDA") Then
                                    Dim paramentros(9) As WordReport.Marcadores_Adicionales
                                    Dim fecha3 As Date
                                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "331", 0)

                                    If dtsPlantilla.Rows.Count > 0 Then

                                        'ENVIAR NUMERO DE CUOTAS 
                                        Dim tb1 As New DataTable
                                        Dim ad As New SqlDataAdapter("SELECT A.CUOTA_NUMERO,convert(varchar(10),A.FECHA_CUOTA,103) AS FECHA_CUOTA , CONVERT (VARCHAR(50),CONVERT (MONEY,A.SALDO_CAPITAL),1) AS SALDO_CAPITAL, A.PERIODO,CONVERT (VARCHAR(50),CONVERT (MONEY,A.VALOR_CUOTA),1)AS APORTE_CAPITAL,A.SALDO_CAPITAL AS SALDO,A.VALOR_CUOTA AS CUOTA FROM DETALLES_ACUERDO_PAGO A , MAESTRO_ACUERDOS B WHERE A.DOCUMENTO = B.DOCUMENTO AND B.EXPEDIENTE = @EXPEDIENTE", Funciones.CadenaConexion)
                                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                                        ad.Fill(tb1)

                                        Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)

                                        paramentros(0).Marcador = "fecha_actual"
                                        paramentros(0).Valor = guardar(1)
                                        paramentros(1).Marcador = "letra"
                                        paramentros(1).Valor = Num2Text(tb1.Rows.Item(0)("CUOTA")).ToUpper
                                        paramentros(2).Marcador = "fecha_titulo"
                                        fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("FECHA_SIS"))
                                        paramentros(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                        paramentros(3).Marcador = "fecha_anterior"
                                        fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("FECHA_ANT"))
                                        paramentros(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                                        paramentros(4).Marcador = "Lcuotas"
                                        paramentros(4).Valor = Num2Text(tb1.Rows.Count).ToUpper
                                        paramentros(5).Marcador = "final_cuotas"
                                        paramentros(5).Valor = tb1.Rows.Count

                                        paramentros(6).Marcador = "Nro_Resolucion"
                                        paramentros(6).Valor = guardar(0).ToUpper
                                        paramentros(7).Marcador = "finalpago"
                                        fecha3 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("INICIO_ACUERDO"))
                                        paramentros(7).Valor = fecha3.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                        paramentros(8).Marcador = "valor_cuota"
                                        paramentros(8).Valor = String.Format("{0:C2}", CDbl(tb1.Rows.Item(0)("CUOTA")))
                                        paramentros(9).Marcador = "fecha_emision"
                                        paramentros(9).Valor = guardar(3)


                                        worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadesPagoDeclaraCumplidaParafiscales, paramentros, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)

                                        If worddocresult = "" Then
                                            ''mensaje no informe
                                        Else
                                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                                        End If
                                    Else
                                        Alert("No se ha concedido una facilidad de pago para este proceso...")
                                        Exit Sub
                                    End If

                                Else
                                    Alert("No se a podido generar la RESOLUCIÓN DECLARA CUMPLIDA FACILIDAD PARAFISCALES, saldo pendiente por pagar.")
                                    Exit Sub
                                End If


                            Else
                                Alert("Noy hay SQL asociado al expedidiente " & lblExpediente.Text.Trim & " ...")
                                Exit Sub
                            End If
                        Else
                            Validator.Text = "IMPOSIBLE GENERAR EL ACTO, VERIFIQUE EL TIPO DE TITULO..."
                            mensup.InnerHtml = Validator.Text
                            Me.Validator.IsValid = False
                            Alert(Validator.Text, DtsDatos)
                            Exit Sub
                        End If

                    End If

                End If


                If selectcod = "333" Then

                    If tbTitulo.Rows.Count > 0 Then
                        Dim tipoTitulo As String = tbTitulo.Rows(0).Item("codigo")
                        If (tipoTitulo = "01") Or (tipoTitulo = "02") Or (tipoTitulo = "04") Then


                            If DtsDatos.DATOS_REPORTES.Count > 0 Then
                                fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                            Else
                                Alert(" NO HAY DATOS PARA ESTA CONSULTA ")
                                Exit Sub
                            End If
                            fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                            Dim dtsPlantilla As DataTable = Funciones.GetDatosGenerales(lblExpediente.Text.Trim)
                            If dtsPlantilla.Rows.Count > 0 Then
                                Dim datos_individual(3) As WordReport.Marcadores_Adicionales

                                datos_individual(0).Marcador = "Ejecutor"
                                datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                                datos_individual(1).Marcador = "Cargo"
                                datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec

                                datos_individual(2).Marcador = "MT_fec_expedicion_titulo"
                                datos_individual(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                                datos_individual(3).Marcador = "Letras"
                                datos_individual(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper


                                worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.FacilidadesPagoRespuestaSolicitudParafiscales, datos_individual)
                                If worddocresult = "" Then
                                    ''mensaje no informe
                                Else
                                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                    SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                                End If
                            End If
                        Else
                            Validator.Text = "IMPOSIBLE GENERAR EL ACTO, VERIFIQUE EL TIPO DE TITULO..."
                            mensup.InnerHtml = Validator.Text
                            Me.Validator.IsValid = False
                            Alert(Validator.Text, DtsDatos)
                            Exit Sub
                        End If
                    End If
                End If


                If selectcod = "334" Then
                    If tbTitulo.Rows.Count > 0 Then
                        Dim tipoTitulo As String = tbTitulo.Rows(0).Item("codigo")
                        If (tipoTitulo = "01") Or (tipoTitulo = "02") Or (tipoTitulo = "04") Then


                            Dim dtsPlantilla As DataTable = GetDatosGenerales(lblExpediente.Text)

                            If dtsPlantilla.Rows.Count > 0 Then
                                Dim datos_individual(2) As WordReport.Marcadores_Adicionales

                                datos_individual(0).Marcador = "Ejecutor"
                                datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                                datos_individual(1).Marcador = "Cargo"
                                datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                                datos_individual(2).Marcador = "fecha_corte"
                                datos_individual(2).Valor = Now().ToString("'al' dd 'de' MMMM 'de' yyy").ToUpper



                                Dim tb1 As New DataTable

                                Dim ad As New SqlDataAdapter("SELECT SUBSISTEMA, '' AS AJUSTE,  CONVERT(varchar(10), ANNO) + ' - '+  B.NOMBRE AS PERIODOS, '' AS INTERESES_MORATORIOS, '' AS SALDO_TOTAL, SUM( AJUSTE) AS CAPITAL ,A.MES,A.ANNO FROM  SQL_PLANILLA A , MAESTRO_MES B WHERE A.MES = B.ID_MES AND A.EXPEDIENTE = @EXPEDIENTE GROUP BY SUBSISTEMA, ANNO, B.NOMBRE,B.ID_MES,A.MES HAVING (SUM( AJUSTE)>0) ORDER BY SUBSISTEMA,ANNO,B.ID_MES", Funciones.CadenaConexion)
                                ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                                ad.Fill(tb1)


                                'Obterner dia habiles de pago
                                Dim tb2 As New DataTable

                                ad = New SqlDataAdapter("SELECT A.DEUDOR,D.CODIGO FROM DEUDORES_EXPEDIENTES A , TIPOS_ENTES B, ENTES_DEUDORES C,TIPOS_APORTANTES D WHERE A.DEUDOR = C.ED_CODIGO_NIT AND A.TIPO = B.CODIGO AND C.ED_TIPOAPORTANTE = D.CODIGO AND B.CODIGO = '1' AND A.NROEXP = @EXPEDIENTE ", Funciones.CadenaConexion)
                                ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                                ad.Fill(tb2)


                                Dim diaPago As String = FuncionesInteresesParafiscales.ObtenerDiaPago(tb2.Rows(0).Item("DEUDOR").ToString, CInt(tb2.Rows(0).Item("CODIGO")))

                                For Each row As DataRow In tb1.Rows
                                    Dim deudaCapital As Double = CDbl(row("CAPITAL"))
                                    Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, row("ANNO"), row("MES"), CInt(tb2.Rows(0).Item("CODIGO")), row("SUBSISTEMA")).ToString("dd/MM/yyyy")
                                    'Alterar grid colocar datos faltantes
                                    row("INTERESES_MORATORIOS") = FuncionesInteresesParafiscales._CalcularIntereses(CDbl(row("CAPITAL")), fechaExigibilidad.ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"), CDec(ViewState("diaria")))
                                    row("SALDO_TOTAL") = CDbl(row("INTERESES_MORATORIOS")) + deudaCapital

                                    row("INTERESES_MORATORIOS") = String.Format("{0:C2}", CDbl(row("INTERESES_MORATORIOS")))
                                    row("SALDO_TOTAL") = String.Format("{0:C2}", CDbl(row("SALDO_TOTAL")))
                                    row("AJUSTE") = String.Format("{0:C2}", CDbl(row("CAPITAL")))
                                Next


                                worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.OficioInformaNoConcedidaFacilidadInvitaPago, datos_individual, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)


                                If worddocresult = "" Then
                                    ''mensaje no informe
                                Else
                                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                    SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                                End If
                            Else
                                Alert("No Hay Datos Para generar el documento")
                                Exit Sub
                            End If
                        Else
                            Validator.Text = "IMPOSIBLE GENERAR EL ACTO, VERIFIQUE EL TIPO DE TITULO..."
                            mensup.InnerHtml = Validator.Text
                            Me.Validator.IsValid = False
                            Alert(Validator.Text, DtsDatos)
                            Exit Sub
                        End If
                    End If
                End If


                If selectcod = "335" Then
                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "331")
                    If dtsPlantilla.Rows.Count > 0 Then
                        Dim datos_individual(6) As WordReport.Marcadores_Adicionales

                        datos_individual(0).Marcador = "Ejecutor"
                        datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                        datos_individual(1).Marcador = "Cargo"
                        datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                        datos_individual(2).Marcador = "SCuota"
                        datos_individual(2).Valor = Math.Round(dtsPlantilla.Rows.Item(0)("SALDO_INICIAL") / dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") * 100)
                        datos_individual(3).Marcador = "Cuota"
                        datos_individual(3).Valor = dtsPlantilla.Rows.Item(0)("NRO_CUOTAS")
                        datos_individual(4).Marcador = "letra"
                        datos_individual(4).Valor = Num2Text(dtsPlantilla.Rows.Item(0)("SALDO_INICIAL")).ToUpper
                        datos_individual(5).Marcador = "fechai"
                        fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("INICIO_ACUERDO"))
                        datos_individual(5).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos_individual(6).Marcador = "monto"
                        datos_individual(6).Valor = dtsPlantilla.Rows.Item(0)("SALDO_INICIAL_M")

                        worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.OficioSolicitaCumplimientoRequisitos, datos_individual)
                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                End If

                If selectcod = "336" Then

                    If tbTitulo.Rows.Count > 0 Then
                        Dim tipoTitulo As String = tbTitulo.Rows(0).Item("codigo")
                        If (tipoTitulo = "01") Or (tipoTitulo = "02") Or (tipoTitulo = "04") Then



                            Dim paramentros(3) As WordReport.Marcadores_Adicionales

                            Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "331")
                            If dtsPlantilla.Rows.Count > 0 Then

                                'Obterner dia habiles de pago
                                Dim tb1 As New DataTable

                                Dim ad As New SqlDataAdapter(" SELECT A.SUBSISTEMA,CONVERT(varchar(10), A.ANNO) + ' - '+  C.NOMBRE AS PERIODOS , CONVERT(varchar (50),convert(money, SUM(A.AJUSTE)),1) AS DEUDA FROM 																						" & _
                                                            " 	(SELECT CONVERT(VARCHAR(12), EXPEDIENTE) + '' + SUBSISTEMA+ ''+ CONVERT(VARCHAR(50), NIT_EMPRESA)+ ''+ CONVERT(VARCHAR(4), ANNO)+ '' + CONVERT(VARCHAR(4),MES)+ ''+ CONVERT(VARCHAR(50),CEDULA) AS ID , * FROM DETALLE_FACILIDAD_PAGO ) A , " & _
                                                            " 	(SELECT CONVERT(VARCHAR(12), EXPEDIENTE) + '' + SUBSISTEMA+ ''+ CONVERT(VARCHAR(50), NIT_EMPRESA)+ ''+ CONVERT(VARCHAR(4), ANNO)+ '' + CONVERT(VARCHAR(4),MES)+ ''+ CONVERT(VARCHAR(50),CEDULA) AS ID, * FROM SQL_PLANILLA )B,              " & _
                                                            " 	 MAESTRO_MES C                                                                                                                                                                                                                              " & _
                                                            " WHERE A.ID= B.ID                                                                                                                                                                                                                              " & _
                                                            " AND A.MES=C.ID_MES AND B.MES=C.ID_MES  AND (A.AJUSTE > B.MONTO_PAGO)  AND A.EXPEDIENTE = @EXPEDIENTE                                                                                                                                              " & _
                                                            " GROUP BY A.SUBSISTEMA, A.ANNO,C.NOMBRE , C.ID_MES ORDER BY A.SUBSISTEMA,A.ANNO,C.ID_MES                                                                                                                                                       ", Funciones.CadenaConexion)
                                ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                                ad.Fill(tb1)

                                If tb1.Rows.Count = 0 Then
                                    Alert("No hay sql asociado a este expediente..")
                                    Exit Sub
                                End If
                                Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)

                                paramentros(0).Marcador = "fecha_actual"
                                paramentros(0).Valor = guardar(1)
                                paramentros(1).Marcador = "fecha_anterior"
                                fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(6))
                                paramentros(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                                paramentros(2).Marcador = "Nro_Resolucion"
                                paramentros(2).Valor = guardar(0)
                                paramentros(3).Marcador = "fecha_emision"
                                paramentros(3).Valor = guardar(3)
                                worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.ResolucionDeclaraIncumplidaParafiscales, paramentros, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)

                                If worddocresult = "" Then
                                    ''mensaje no informe
                                Else
                                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                    SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                                End If
                            Else
                                Alert("No Hay Datos Para generar el documento")
                                Exit Sub
                            End If
                        Else
                            Validator.Text = "IMPOSIBLE GENERAR EL ACTO, VERIFIQUE EL TIPO DE TITULO..."
                            mensup.InnerHtml = Validator.Text
                            Me.Validator.IsValid = False
                            Alert(Validator.Text, DtsDatos)
                            Exit Sub
                        End If
                    End If
                End If


                If selectcod = "337" Then
                    Dim paramentros(10) As WordReport.Marcadores_Adicionales
                    ''Dim fecha3 As Date
                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, selectcod, 1)
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                    If dtsPlantilla.Rows.Count > 0 Then
                        paramentros(0).Marcador = "fecha_actual"
                        paramentros(0).Valor = guardar(1)
                        paramentros(1).Marcador = "letra"
                        paramentros(1).Valor = Num2Text(dtsPlantilla.Rows.Item(0)(9))
                        paramentros(2).Marcador = "fecha_titulo"
                        fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(1))
                        paramentros(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(3).Marcador = "fecha_anterior"
                        fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(6))
                        paramentros(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(4).Marcador = "Lcuotas"
                        paramentros(4).Valor = Num2Text(dtsPlantilla.Rows.Count)
                        paramentros(5).Marcador = "Nro_Resolucion"
                        paramentros(5).Valor = guardar(0)
                        paramentros(6).Marcador = "nrocuota"
                        paramentros(6).Valor = dtsPlantilla.Rows.Count
                        paramentros(7).Marcador = "Tanual"
                        paramentros(7).Valor = getPorcentaje(0)
                        paramentros(8).Marcador = "Tmensual"
                        paramentros(8).Valor = getPorcentaje(1)
                        paramentros(9).Marcador = "SCuota"
                        paramentros(9).Valor = Math.Round(dtsPlantilla.Rows.Item(0)(10) / dtsPlantilla.Rows.Item(0)(9) * 100)
                        paramentros(10).Marcador = "fecha_emision"
                        paramentros(10).Valor = guardar(3)
                        'For i = 0 To dtsPlantilla.Rows.Count - 1

                        'dtsPlantilla.Rows.Item(i)("INTERESES_TOTALES") = dtsPlantilla.Rows.Item(i)(10) * ((getPorcentaje(1) + 1) ^ dtsPlantilla.Rows.Count)
                        'dtsPlantilla.Rows.Item(i)("TOTAL_CON") = dtsPlantilla.Rows.Item(i)("INTERESES_TOTALES") + dtsPlantilla.Rows.Item(i)(9)

                        'Next

                        worddocresult = worddoc.CreateReportWithTable(dtsPlantilla, Reportes.FacilidadPagoResolucionConcedeMulta, "CUOTA_NUMERO,FECHA_CUOTA,SALDO_CAPITAL,APORTE_CAPITAL,INTERESES_TOTALES,TOTAL_CON", paramentros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                End If


                If selectcod = "338" Then

                    If tbTitulo.Rows.Count > 0 Then
                        Dim tipoTitulo As String = tbTitulo.Rows(0).Item("codigo")
                        If (tipoTitulo = "05") Or (tipoTitulo = "07") Then

                            ''SABER SI YA SE PAGO LA TOTALIDAD DE LA DEUDA
                            Dim pazysalvo As Integer = Funciones.PazySalvoFacilidadPagoMultas(lblExpediente.Text.Trim)

                            If pazysalvo = 1 Then
                                Dim paramentros(10) As WordReport.Marcadores_Adicionales
                                Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                                Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, selectcod)
                                If dtsPlantilla.Rows.Count > 0 Then

                                    Dim TFacilidad As DataTable = Funciones.RetornaCargadatos("SELECT A.CUOTA_NUMERO, CONVERT(VARCHAR(10), A.FECHA_CUOTA,3) AS FECHA_CUOTA,CONVERT(VARCHAR(10), A.FECHA_PAGO,3) AS FECHA_PAGO,A.SALDO_CAPITAL, A.VALOR_PAGADO - A.VALOR_INTERES AS APORTE_CAPITAL,A.VALOR_PAGADO ,a.VALOR_CUOTA ,C.DG_NRO_DOC, C.DG_FECHA_DOC FROM DETALLES_ACUERDO_PAGO A, MAESTRO_ACUERDOS B, DOCUMENTOS_GENERADOS C WHERE A.DOCUMENTO =  B.DOCUMENTO AND B.EXPEDIENTE = C.DG_EXPEDIENTE AND  B.EXPEDIENTE = '" & lblExpediente.Text.Trim & "' AND C.DG_COD_ACTO = '337'")

                                    paramentros(0).Marcador = "fecha_actual"
                                    paramentros(0).Valor = guardar(1)
                                    paramentros(1).Marcador = "letra"
                                    paramentros(1).Valor = Num2Text(TFacilidad.Rows(0).Item("VALOR_CUOTA"))
                                    paramentros(2).Marcador = "fecha_titulo"
                                    fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(1))
                                    paramentros(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                                    paramentros(3).Marcador = "FechaFaci"

                                    If CStr(TFacilidad.Rows(0).Item("DG_FECHA_DOC")) = "" Then
                                        paramentros(3).Valor = "XXXXXX"
                                    Else
                                        fecha2 = Convert.ToDateTime(TFacilidad.Rows(0).Item("DG_FECHA_DOC"))
                                        paramentros(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                                    End If

                                    paramentros(4).Marcador = "Lcuotas"
                                    paramentros(4).Valor = Num2Text(TFacilidad.Rows.Count)
                                    paramentros(5).Marcador = "final_cuotas"
                                    paramentros(5).Valor = TFacilidad.Rows.Count
                                    paramentros(6).Marcador = "Nro_Resolucion"
                                    paramentros(6).Valor = guardar(0)
                                    paramentros(7).Marcador = "finalpago"
                                    paramentros(7).Valor = dtsPlantilla.Rows.Item(0)(3)
                                    paramentros(8).Marcador = "fecha_emision"
                                    paramentros(8).Valor = guardar(3)
                                    paramentros(9).Marcador = "Nro_Facilidad"
                                    paramentros(9).Valor = TFacilidad.Rows(0).Item("DG_NRO_DOC") & " " & paramentros(3).Valor
                                    paramentros(10).Marcador = "Vcuotas"
                                    paramentros(10).Valor = TFacilidad.Rows(0).Item("VALOR_CUOTA")

                                    'Vcuotas

                                    'For I = 0 To dtsPlantilla.Rows.Count - 1

                                    '    If dtsPlantilla.Rows.Item(I)("CONFIRMACION") = False Then
                                    '        dtsPlantilla.Rows.Item(I)("CONFIRMACION") = "SIN PAGO"
                                    '    Else
                                    '        dtsPlantilla.Rows.Item(I)("CONFIRMACION") = "PAGO"
                                    '    End If

                                    'Next

                                    worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadPagoDeclaraCumplidaMulta, paramentros, TFacilidad, 0, False, Nothing, 0, False, Nothing, 0, False)

                                    If worddocresult = "" Then
                                        ''mensaje no informe
                                    Else
                                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                                    End If
                                Else
                                    Alert("No Hay Datos Para generar el documento")
                                    Exit Sub
                                End If
                            Else
                                Validator.Text = "IMPOSIBLE GENERAR EL ACTO <br/> <b/> NO SE HA PAGADO LA TOTALIDAD DE LA DEUDA"
                                mensup.InnerHtml = Validator.Text
                                Me.Validator.IsValid = False
                                Alert(Validator.Text, DtsDatos)
                                Exit Sub
                            End If
                        Else
                            Validator.Text = "IMPOSIBLE GENERAR EL ACTO, VERIFIQUE EL TIPO DE TITULO..."
                            mensup.InnerHtml = Validator.Text
                            Me.Validator.IsValid = False
                            Alert(Validator.Text, DtsDatos)
                            Exit Sub
                        End If
                    End If
                End If

                If selectcod = "339" Then

                    If tbTitulo.Rows.Count > 0 Then
                        Dim tipoTitulo As String = tbTitulo.Rows(0).Item("codigo")
                        If (tipoTitulo = "01") Or (tipoTitulo = "02") Or (tipoTitulo = "04") Then

                            If ValidarAcuerdo(lblExpediente.Text.Trim) Then
                                Validator.Text = "EL EXPEDIENTE NÚMERO " & lblExpediente.Text.Trim & " TIENE UNA FACILIDAD DE PAGO VIGENTE..."
                                mensup.InnerHtml = Validator.Text
                                Me.Validator.IsValid = False
                                Alert(Validator.Text)
                                Exit Sub
                            End If


                            Dim dtsPlantilla As DataTable = Funciones.GetDatosGenerales(lblExpediente.Text.Trim)
                            If dtsPlantilla.Rows.Count > 0 Then

                                Dim tb1 As New DataTable

                                Dim ad As New SqlDataAdapter("SELECT SUBSISTEMA, '' AS AJUSTE,  CONVERT(varchar(10), ANNO) + ' - '+  B.NOMBRE AS PERIODOS, '' AS INTERESES_MORATORIOS, '' AS SALDO_TOTAL, SUM( AJUSTE) AS CAPITAL ,A.MES,A.ANNO FROM  SQL_PLANILLA A , MAESTRO_MES B WHERE A.MES = B.ID_MES AND A.EXPEDIENTE = @EXPEDIENTE GROUP BY SUBSISTEMA, ANNO, B.NOMBRE,B.ID_MES,A.MES ORDER BY SUBSISTEMA,ANNO,B.ID_MES", Funciones.CadenaConexion)
                                ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                                ad.Fill(tb1)

                                If tb1.Rows.Count = 0 Then
                                    Alert("No hay sql asociado a este expediente..")
                                    Exit Sub
                                End If

                                'Obterner dia habiles de pago
                                Dim tb2 As New DataTable

                                ad = New SqlDataAdapter("SELECT A.DEUDOR,D.CODIGO FROM DEUDORES_EXPEDIENTES A , TIPOS_ENTES B, ENTES_DEUDORES C,TIPOS_APORTANTES D WHERE A.DEUDOR = C.ED_CODIGO_NIT AND A.TIPO = B.CODIGO AND C.ED_TIPOAPORTANTE = D.CODIGO AND B.CODIGO = '1' AND A.NROEXP = @EXPEDIENTE ", Funciones.CadenaConexion)
                                ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                                ad.Fill(tb2)


                                Dim datos_individual(2) As WordReport.Marcadores_Adicionales

                                datos_individual(0).Marcador = "Ejecutor"
                                datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                                datos_individual(1).Marcador = "Cargo"
                                datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                                datos_individual(2).Marcador = "fecha_corte"
                                datos_individual(2).Valor = Now().ToString("'al' dd 'de' MMMM 'de' yyy").ToUpper



                                Dim diaPago As String = FuncionesInteresesParafiscales.ObtenerDiaPago(tb2.Rows(0).Item("DEUDOR").ToString, CInt(tb2.Rows(0).Item("CODIGO")))

                                For Each row As DataRow In tb1.Rows
                                    Dim deudaCapital As Double = CDbl(row("CAPITAL"))
                                    Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, row("ANNO"), row("MES"), CInt(tb2.Rows(0).Item("CODIGO")), row("SUBSISTEMA")).ToString("dd/MM/yyyy")
                                    'Alterar grid colocar datos faltantes
                                    row("INTERESES_MORATORIOS") = FuncionesInteresesParafiscales._CalcularIntereses(CDbl(row("CAPITAL")), fechaExigibilidad.ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"), CDec(ViewState("diaria")))
                                    row("SALDO_TOTAL") = CDbl(row("INTERESES_MORATORIOS")) + deudaCapital

                                    row("INTERESES_MORATORIOS") = String.Format("{0:C2}", CDbl(row("INTERESES_MORATORIOS")))
                                    row("SALDO_TOTAL") = String.Format("{0:C2}", CDbl(row("SALDO_TOTAL")))
                                    row("AJUSTE") = String.Format("{0:C2}", CDbl(row("CAPITAL")))
                                Next

                                worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadOficioInformaNoconcedidaFacilidadMulta, datos_individual, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)

                                If worddocresult = "" Then
                                    ''mensaje no informe
                                Else
                                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                    SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                                End If
                            Else
                                Alert("No Hay Datos Para generar el documento")
                                Exit Sub
                            End If
                        Else
                            Validator.Text = "IMPOSIBLE GENERAR EL ACTO, VERIFIQUE EL TIPO DE TITULO..."
                            mensup.InnerHtml = Validator.Text
                            Me.Validator.IsValid = False
                            Alert(Validator.Text, DtsDatos)
                            Exit Sub
                        End If
                    End If
                End If


                If selectcod = "340" Then
                    If tbTitulo.Rows.Count > 0 Then
                        Dim tipoTitulo As String = tbTitulo.Rows(0).Item("codigo")
                        If (tipoTitulo = "05") Or (tipoTitulo = "07") Then


                            Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "337", 0)

                            If dtsPlantilla.Rows.Count > 0 Then
                                Dim datos_individual(3) As WordReport.Marcadores_Adicionales
                                datos_individual(0).Marcador = "Ejecutor"
                                datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                                datos_individual(1).Marcador = "Cargo"
                                datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec


                                Dim tb1 As DataTable = Funciones.RetornaCargadatos("SELECT A.CUOTA_NUMERO, '' AS VALOR_CUOTA1, convert(varchar(10),A.FECHA_CUOTA,103) AS FECHA_CUOTA, B.p_anual AS TASA_MORA , convert(varchar(10),A.FECHA_PAGO,103) AS FECHA_PAGO,  DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO)AS DIAS_MORA,'' AS INTERES1,''AS TOTAL_A_PAGAR1,'' AS TOTAL_PAGADO1, '' AS SALDO_PENDIENTE1, VALOR_CUOTA * ( B.p_anual/ 365) * DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO) AS INTERES,0 AS TOTAL_A_PAGAR,0 AS TOTAL_PAGADO, 0 AS SALDO_PENDIENTE, A.VALOR_CUOTA   FROM DETALLES_ACUERDO_PAGO A, PORCENTAJE_TASA_MULTA B ,MAESTRO_ACUERDOS C WHERE A.DOCUMENTO = C.DOCUMENTO AND C.EXPEDIENTE = '" & lblExpediente.Text.Trim() & "' AND DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO) >0 AND CUOTA_FIN = 0 ORDER BY A.CUOTA_FIN")
                                Dim tb2 As DataTable = Funciones.RetornaCargadatos("SELECT A.CUOTA_NUMERO, '' AS VALOR_CUOTA1, '' AS VALOR_INTERES_PAGAR1,'' AS TOTAL_A_PAGAR1, convert(varchar(10),A.FECHA_PAGO,103) as FECHA_PAGO ,  A.VALOR_CUOTA, 0 AS  VALOR_INTERES_PAGAR,0 AS TOTAL_A_PAGAR  FROM DETALLES_ACUERDO_PAGO A, PORCENTAJE_TASA_MULTA B ,MAESTRO_ACUERDOS C WHERE A.DOCUMENTO = C.DOCUMENTO AND C.EXPEDIENTE = '" & lblExpediente.Text.Trim() & "' AND DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO) >0 AND CUOTA_FIN = 1 ORDER BY A.CUOTA_FIN")

                                For Each row As DataRow In tb1.Rows
                                    row("TOTAL_A_PAGAR") = row("VALOR_CUOTA") + row("INTERES")
                                    row("TOTAL_A_PAGAR1") = String.Format("{0:C2}", CDbl(row("TOTAL_A_PAGAR")))

                                    row("SALDO_PENDIENTE") = row("TOTAL_A_PAGAR") - row("TOTAL_PAGADO")
                                    row("SALDO_PENDIENTE1") = String.Format("{0:C2}", CDbl(row("SALDO_PENDIENTE")))

                                    row("VALOR_CUOTA1") = String.Format("{0:C2}", CDbl(row("VALOR_CUOTA")))
                                    row("INTERES1") = String.Format("{0:C2}", CDbl(row("INTERES")))

                                Next

                                tb2.Rows(0).Item("VALOR_INTERES_PAGAR") = tb1.Compute("SUM(INTERES)", String.Empty)
                                tb2.Rows(0).Item("VALOR_INTERES_PAGAR1") = String.Format("{0:C2}", CDbl(tb2.Rows(0).Item("VALOR_INTERES_PAGAR")))

                                tb2.Rows(0).Item("TOTAL_A_PAGAR") = tb1.Compute("SUM(SALDO_PENDIENTE)", String.Empty) + tb2.Rows(0).Item("VALOR_CUOTA")
                                tb2.Rows(0).Item("TOTAL_A_PAGAR1") = String.Format("{0:C2}", CDbl(tb2.Rows(0).Item("TOTAL_A_PAGAR")))

                                tb2.Rows(0).Item("VALOR_CUOTA1") = String.Format("{0:C2}", CDbl(tb2.Rows(0).Item("VALOR_CUOTA")))

                                datos_individual(2).Marcador = "valor_cuota"
                                datos_individual(2).Valor = String.Format("{0:C2}", CDbl(tb1.Compute("SUM(SALDO_PENDIENTE)", String.Empty)))
                                datos_individual(3).Marcador = "letra_cuota"
                                datos_individual(3).Valor = Num2Text(tb1.Compute("SUM(SALDO_PENDIENTE)", String.Empty))

                                worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadesPagoFormatoAjusteUltimaCuota, datos_individual, tb1, 0, False, tb2, 1, False, Nothing, 0, False)

                                If worddocresult = "" Then
                                    ''mensaje no informe
                                Else
                                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                    SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                                End If
                            Else
                                Alert("No se ha generado facilidad de pago correspondiente a este proceso...")
                                Exit Sub
                            End If
                        Else
                            Validator.Text = "IMPOSIBLE GENERAR EL ACTO, VERIFIQUE EL TIPO DE TITULO..."
                            mensup.InnerHtml = Validator.Text
                            Me.Validator.IsValid = False
                            Alert(Validator.Text, DtsDatos)
                            Exit Sub
                        End If
                    End If
                End If


                If selectcod = "341" Then

                    Dim dtsPlantilla As DataTable

                    If GetDatoPlantillaWord(lblExpediente.Text, "").Rows.Count > 0 Then
                        dtsPlantilla = GetDatoPlantillaWord(lblExpediente.Text, "")
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                    Dim datos_individual(1) As WordReport.Marcadores_Adicionales

                    datos_individual(0).Marcador = "Ejecutor"
                    datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                    datos_individual(1).Marcador = "Cargo"
                    datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec

                    worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.OficioInformaNoConcedidaFacilidadInvitaPagoMulta, datos_individual)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "344" Then

                    Dim dtsPlantilla As DataTable

                    If GetDatoPlantillaWord(lblExpediente.Text, "").Rows.Count > 0 Then
                        dtsPlantilla = GetDatoPlantillaWord(lblExpediente.Text, "")
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If
                    Dim datos_individual(1) As WordReport.Marcadores_Adicionales

                    datos_individual(0).Marcador = "Ejecutor"
                    datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                    datos_individual(1).Marcador = "Cargo"
                    datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec

                    worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.OficioInformaNoConcedidaFacilidadInvitaPago, datos_individual)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If

                If selectcod = "345" Then

                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fecha_ejecutoria
                    If IsDBNull(fecha2) Then
                        Alert("NO SE HA INGRESADO FECHA DE EJECUTORIA DEL TITULO ")
                        Exit Sub
                    End If

                    Dim datos_individual(6) As WordReport.Marcadores_Adicionales
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0).ToUpper
                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    datos_individual(2).Marcador = "salario_M"
                    datos_individual(2).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    datos_individual(3).Marcador = "salario_L"
                    datos_individual(3).Valor = Num2Text(Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0))
                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = guardar(1).ToUpper
                    datos_individual(4).Marcador = "fecha1"
                    datos_individual(4).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_individual(5).Marcador = "fecha_ejecutoria"
                    datos_individual(5).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMpPagoMulta1438Nuevauenta, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "232" Then


                    Dim datos_individual(16) As WordReport.Marcadores_Adicionales

                    Dim inicial() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim Desembargo() As String
                    Dim Embargo() As String
                    Dim Anterior() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, homologo("316"))
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        inicial(0) = vc_datos(0).ToUpper
                        inicial(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If inicial(0).ToString = "" Then
                        Alert("NO SE DETECTO RESOLUCIÓN DE TERMINACIÓN DE PROCESO PARA ESTE EXPEDIENTE...")
                        Exit Sub
                    Else

                        Desembargo = getResolucion_anterior(lblExpediente.Text, inicial(2))
                        vc_datos = overloadresolucion(lblExpediente.Text, "229")

                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            Desembargo(0) = vc_datos(0).ToUpper
                            Desembargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If Desembargo(0).ToString = "" Then
                            Alert("NO SE DETECTO RESOLUCIÓN DE DESEMBARGO PARA ESTE EXPEDIENTE...")
                            Exit Sub
                        Else

                            Embargo = getResolucion_anterior(lblExpediente.Text, Desembargo(2))
                            vc_datos = overloadresolucion(lblExpediente.Text, homologo("319"))
                            If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                                Embargo(0) = vc_datos(0).ToUpper
                                Embargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                            Else

                            End If

                            If Embargo(0).ToString = "" Then
                                Alert("NO SE DETECTO RESOLUCIÓN DE EMBARGO PARA ESTE EXPEDIENTE...")
                                Exit Sub
                            Else
                                Anterior = getResolucion_anterior(lblExpediente.Text, Embargo(2))
                                vc_datos = overloadresolucion(lblExpediente.Text, "013")
                                If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                                    Anterior(0) = vc_datos(0).ToUpper
                                    Anterior(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                Else

                                End If

                                If Anterior(0).ToString = "" Then
                                    Alert("NO SE DETECTO RESOLUCIÓN DE MANDAMIENTO DE PAGO PARA ESTE EXPEDIENTE...")
                                    Exit Sub
                                Else

                                End If

                            End If
                        End If
                    End If


                    Dim Valores() As String = GetActo(lblExpediente.Text, "013")
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)

                    Dim tbl As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)
                    If tbl.Rows.Count < 0 Then
                        Alert("no hay titulo de deposito judicial ")
                        Exit Sub
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


                    If Desembargo(0) <> Nothing Then
                        datos_individual(2).Marcador = "nro_desembargo"
                        datos_individual(2).Valor = Desembargo(0)
                        datos_individual(3).Marcador = "fecha_desembargo"
                        datos_individual(3).Valor = Desembargo(1)
                    Else
                        Alert("NO SE ENCUENTRA UNA RESOLUCION DE DESEMBARGO ")
                        Exit Sub
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
                        datos_individual(8).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        datos_individual(9).Marcador = "rep_nit"
                        datos_individual(9).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit
                    End If
                    datos_individual(10).Marcador = "Nsalario"
                    datos_individual(10).Valor = Round((DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
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
                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.DevolucionTitulo2, datos_individual, tbl, 0, False, tbl, 1, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "316" Then

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    DtsDatos.DATOS_REPORTES.Item(0).RESOLUCION = "RESOLUCION"
                    Dim parametro(4) As WordReport.Marcadores_Adicionales
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    parametro(0).Marcador = "fecha_actual"
                    parametro(0).Valor = guardar(1).ToUpper

                    parametro(1).Marcador = "fecha1"
                    parametro(1).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    parametro(2).Marcador = "letras"
                    parametro(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    parametro(3).Marcador = "Nro_Resolucion"
                    parametro(3).Valor = guardar(0).ToUpper

                    parametro(4).Marcador = "fecha_emision"
                    parametro(4).Valor = guardar(3)
                    SaveTable(lblExpediente.Text, selectcod)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AutoTerminacionProcesoPagoTotal, parametro)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        Dim datosnumero As Integer = nombre.Length
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "358" Then

                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                    Dim datos_individual(2) As WordReport.Marcadores_Adicionales
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0)
                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_individual(2).Marcador = "salario"
                    datos_individual(2).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoPlanillaLevantamientoEmbargo, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        Dim datosnumero As Integer = nombre.Length
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If

                If selectcod = "359" Then

                    Dim datos_individual(12) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim Excepcion() As String = GetExcepcion(lblExpediente.Text)
                    Dim vc_datos() As String
                    Dim guardar(3) As String
                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If



                    If resolucion(0) <> Nothing Then
                        datos_individual(2).Marcador = "fecha_anterior"
                        datos_individual(2).Valor = resolucion(1).ToUpper

                        datos_individual(3).Marcador = "resolucion_anterior"
                        datos_individual(3).Valor = resolucion(0).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO  PARA GENERAR EL DOCUMENTO ")
                        Exit Sub
                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "359")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If


                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0).ToUpper
                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = guardar(1).ToUpper

                    datos_individual(5).Marcador = "acta_numero"
                    datos_individual(5).Valor = Valores(2).ToUpper

                    datos_individual(6).Marcador = "tipo_notificacion"
                    datos_individual(6).Valor = Valores(1).ToUpper

                    datos_individual(7).Marcador = "acta_dia"
                    datos_individual(7).Valor = Valores(0).ToUpper

                    If Excepcion(0) <> Nothing Then
                        datos_individual(8).Marcador = "resolucion_excepcion"
                        datos_individual(8).Valor = Excepcion(0).ToUpper

                        datos_individual(9).Marcador = "fecha_excepcion"
                        datos_individual(9).Valor = Excepcion(1).ToUpper

                        datos_individual(10).Marcador = "nro_radicado"
                        datos_individual(10).Valor = Excepcion(2).ToUpper

                        datos_individual(11).Marcador = "fecha_rad"
                        datos_individual(11).Valor = Excepcion(3).ToUpper

                    Else
                        Alert("NO HAY EXCEPCIONES PARA GENERAR EL DOCUMENTO  SOLICITADO ")
                        Exit Sub
                    End If
                    datos_individual(12).Marcador = "fecha_emision"
                    datos_individual(12).Valor = guardar(3)
                    SaveTable(lblExpediente.Text, selectcod)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.RechazaExcepcionesSigueAdelanteConEjecucion, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        Dim datosnumero As Integer = nombre.Length
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "360" Then

                    Dim datos_individual(12) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim Excepcion As String() = GetExcepcion(lblExpediente.Text)
                    Dim Resolucion As String() = getResolucion_anterior(lblExpediente.Text, selectcod)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim vc_datos() As String
                    Dim guardar(3) As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        Resolucion(0) = vc_datos(0).ToUpper
                        Resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If Resolucion(0) <> Nothing Then
                        datos_individual(0).Marcador = "fecha_anterior"
                        datos_individual(0).Valor = Resolucion(1).ToUpper

                        datos_individual(1).Marcador = "resolucion_anterior"
                        datos_individual(1).Valor = Resolucion(0).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO DE PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub

                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "360")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If


                    datos_individual(2).Marcador = "Nro_Resolucion"
                    datos_individual(2).Valor = guardar(0).ToUpper

                    datos_individual(3).Marcador = "letra"
                    datos_individual(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = guardar(1).ToUpper

                    datos_individual(5).Marcador = "acta_numero"
                    datos_individual(5).Valor = Valores(2).ToUpper

                    datos_individual(6).Marcador = "tipo_notificacion"
                    datos_individual(6).Valor = Valores(1).ToUpper

                    datos_individual(7).Marcador = "acta_dia"
                    datos_individual(7).Valor = Valores(0).ToUpper

                    If Excepcion(0) <> Nothing Then
                        datos_individual(8).Marcador = "resolucion_excepcion"
                        datos_individual(8).Valor = Excepcion(0).ToUpper

                        datos_individual(9).Marcador = "fecha_excepcion"
                        datos_individual(9).Valor = Excepcion(1).ToUpper

                        datos_individual(10).Marcador = "nro_radicado"
                        datos_individual(10).Valor = Excepcion(2).ToUpper

                        datos_individual(11).Marcador = "fecha_rad"
                        datos_individual(11).Valor = Excepcion(3).ToUpper

                    Else
                        Alert("NO HAY EXCEPCIONES PARA GENERAR EL DOCUMENTO  SOLICITADO ")
                        Exit Sub
                    End If
                    datos_individual(12).Marcador = "fecha_emision"
                    datos_individual(12).Valor = guardar(3)

                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, selectcod)

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.RecursoReposicionReposicionParcial, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "363" Then

                    Dim datos_individual(5) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    datos_individual(0).Marcador = "letras"
                    datos_individual(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else
                        Alert("NO HAY RESOLUCION (013) DE MANDAMIENTO DE PAGO GENERAR EL DOCUMENTO ")
                        Exit Sub
                    End If

                    If resolucion(0) <> Nothing Then
                        datos_individual(1).Marcador = "fecha_anterior"
                        datos_individual(1).Valor = resolucion(1).ToUpper

                        datos_individual(2).Marcador = "resolucion_anterior"
                        datos_individual(2).Valor = resolucion(0).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION (013) DE MANDAMIENTO DE PAGO GENERAR EL DOCUMENTO ")
                        Exit Sub
                    End If
                    Dim guardar(3) As String

                    vc_datos = overloadresolucion(lblExpediente.Text, "363")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        guardar(0) = vc_datos(0).ToUpper
                        guardar(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        guardar(2) = vc_datos(1)
                        guardar(3) = vc_datos(2)
                    Else
                        guardar = saveResolucion(lblExpediente.Text, selectcod)
                    End If
                    Dim tbl_vin1, tbl_vin2 As New DataTable
                    Dim sql_vinculo1, sql_vinculo2 As SqlDataAdapter

                    sql_vinculo1 = New SqlDataAdapter(" SELECT 0 AS CUENTA ,ED_NOMBRE ," & _
                                " ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP ='" & lblExpediente.Text.Trim & "' AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)

                    sql_vinculo2 = New SqlDataAdapter(" SELECT 0 AS CUENTA ,ED_NOMBRE,  " & _
                                " ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION , '' AS VALORES ,'' AS LETRAS FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP ='" & lblExpediente.Text.Trim & "' AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)

                    sql_vinculo1.Fill(tbl_vin1)
                    If tbl_vin1.Rows.Count > 0 Then
                        For i = 0 To tbl_vin1.Rows.Count - 1
                            tbl_vin1.Rows(i).Item("CUENTA") = i + 1
                        Next
                    Else
                        Alert("NO HAY DEUDORES SOLIDARIOS PARA GENERAR EL DOCUMENTO")
                        Exit Sub
                    End If
                    sql_vinculo2.Fill(tbl_vin2)
                    If tbl_vin2.Rows.Count > 0 Then
                        For I = 0 To tbl_vin2.Rows.Count - 1
                            tbl_vin2.Rows(I).Item("CUENTA") = I + 1
                            tbl_vin2.Rows(I).Item("VALORES") = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (tbl_vin2.Rows(I).Item("PARTICIPACION") / 100)))
                            tbl_vin2.Rows(I).Item("LETRAS") = Num2Text(tbl_vin2.Rows(I).Item("VALORES"))
                        Next


                    Else

                        Alert("NO HAY DATOS DEUDORES SOLIDARIOS PARA GENERAR EL DOCUMENTO")
                    End If



                    datos_individual(3).Marcador = "Nro_Resolucion"
                    datos_individual(3).Valor = guardar(0).ToUpper
                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = guardar(1).ToUpper
                    datos_individual(5).Marcador = "fecha_emision"
                    datos_individual(5).Valor = guardar(3)
                    If vc_datos(0) = Nothing And vc_datos(0) = "" Then
                        SaveTable(lblExpediente.Text, selectcod)
                    End If

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.VINCULACIONSOCIOSSOLIDARIOSLIQUIDACIÓNOFICIAL, datos_individual, tbl_vin1, 0, False, tbl_vin2, 1, True, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If


                If selectcod = "366" Then

                    Dim Parametros(10) As WordReport.Marcadores_Adicionales
                    Dim vc_datos() As String
                    Dim resolucion_E() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                    vc_datos = overloadresolucion(lblExpediente.Text, "319")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion_E(0) = vc_datos(0).ToUpper
                        resolucion_E(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion_E(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE  EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If

                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_E(2))
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If
                    Dim embargo_valor As String = getEmbargo(0, lblExpediente.Text.Trim)
                    If embargo_valor < 0 Or Nothing Then
                        Alert("NO HAY INGRESADO PORCENTAJE A EMBARGAR PARA PODER GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If

                    Dim embargo As Double = (DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargo_valor / 100))

                    Dim guardar() As String = saveResolucion(lblExpediente.Text.Trim, selectcod)
                    If guardar(0) = Nothing Then
                        Alert("ESTE REPORTE POSEE DESMARCADO EL CAMPO PARA GENERAR DOCUMENTACION")
                        Exit Sub
                    End If

                    Dim tbl1 As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)
                    If tbl1 Is Nothing = True Then
                        Alert("NO HAY  DATOS DE TITULO DE DEPOSITO JUDICIAL ")
                        Exit Sub
                    End If
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Parametros(0).Marcador = "resolucion_anterior"
                    Parametros(0).Valor = resolucion(0).ToUpper

                    Parametros(1).Marcador = "fecha_anterior"
                    Parametros(1).Valor = resolucion(1).ToUpper

                    Parametros(2).Marcador = "resolucion_embargo"
                    Parametros(2).Valor = resolucion_E(0).ToUpper

                    Parametros(3).Marcador = "fecha_resolucion"
                    Parametros(3).Valor = resolucion_E(1).ToUpper

                    Parametros(4).Marcador = "fecha_actual"
                    Parametros(4).Valor = guardar(1).ToUpper

                    Parametros(5).Marcador = "nro_resolucion"
                    Parametros(5).Valor = guardar(0).ToUpper

                    Parametros(6).Marcador = "Procedencia"
                    Parametros(6).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                    Parametros(7).Marcador = "letras_deuda"
                    Parametros(7).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)


                    Parametros(8).Marcador = "letras_embargo"
                    Parametros(8).Valor = Num2Text(embargo).ToUpper

                    Parametros(9).Marcador = "Total_Embargo"
                    Parametros(9).Valor = String.Format("{0:C0}", embargo)

                    Parametros(10).Marcador = "fecha1"
                    Parametros(10).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo).ToString("'del' dd 'de' MMMM 'de' yyy")

                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, "316")

                    End If

                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.TerminacionporTutela, Parametros, tbl1, 0, False, tbl1, 1, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If


                End If
                If selectcod = "367" Then

                    Dim Parametros(10) As WordReport.Marcadores_Adicionales
                    Dim resolucion_E() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "319")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion_E(0) = vc_datos(0).ToUpper
                        resolucion_E(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion_E(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE  EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If

                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_E(2))
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If

                    Dim guardar() As String = saveResolucion(lblExpediente.Text.Trim, selectcod)
                    If guardar(0) = Nothing Then
                        Alert("ESTE REPORTE POSEE DESMARCADO EL CAMPO PARA GENERAR DOCUMENTACION")
                        Exit Sub
                    End If

                    Dim tbl1 As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)

                    Dim tbl2 As DataTable = GetTituloPrincipal3(lblExpediente.Text.Trim)
                    Dim valores() As String = GetActo(lblExpediente.Text.Trim, "013")
                    Parametros(0).Marcador = "resolucion_anterior"
                    Parametros(0).Valor = resolucion(0)

                    Parametros(1).Marcador = "fecha_anterior"
                    Parametros(1).Valor = resolucion(1)

                    Parametros(2).Marcador = "resolucion_embargo"
                    Parametros(2).Valor = resolucion_E(0)

                    Parametros(3).Marcador = "fecha_resolucion"
                    Parametros(3).Valor = resolucion_E(1)

                    Parametros(4).Marcador = "fecha_actual"
                    Parametros(4).Valor = guardar(1).ToUpper

                    Parametros(5).Marcador = "nro_resolucion"
                    Parametros(5).Valor = guardar(0).ToUpper

                    Parametros(6).Marcador = "acta"
                    Parametros(6).Valor = valores(2)
                    Parametros(7).Marcador = "formanotificacion"
                    Parametros(7).Valor = valores(1)
                    Parametros(8).Marcador = "fecha_notificacion"
                    Parametros(8).Valor = valores(0)
                    Parametros(9).Marcador = "letras"

                    Parametros(9).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                    Parametros(10).Marcador = "fecha1"
                    Parametros(10).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo).ToString("'del' dd 'de' MMMM 'de' yyy")

                    If vc_datos(0) = Nothing Then
                        SaveTable(lblExpediente.Text, "316")

                    End If
                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.Terminaciondevoluciontitulodepositjudicial, Parametros, tbl1, 0, False, tbl2, 2, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If


                End If

                If selectcod = "369" Then

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then

                        Dim Parametros(14) As WordReport.Marcadores_Adicionales
                        Dim vc_datos() As String
                        Dim resolucion_Desembargo() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                        Dim resolucion_E() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_Desembargo(2))
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_E(2))

                        vc_datos = overloadresolucion(lblExpediente.Text, "229")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion_Desembargo(0) = vc_datos(0).ToUpper
                            resolucion_Desembargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion_Desembargo(0) = Nothing Then
                            Alert("NO HAY RESOLUCION DE  DESEMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If


                        vc_datos = overloadresolucion(lblExpediente.Text, "320")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion_E(0) = vc_datos(0).ToUpper
                            resolucion_E(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion_E(0) = Nothing Then
                            Alert("NO HAY RESOLUCION DE  EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If


                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion(0) = Nothing Then
                            Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If
                        Dim embargo_valor As String = getEmbargo(0, lblExpediente.Text.Trim)
                        If embargo_valor < 0 Or Nothing Then
                            Alert("NO HAY INGRESADO PORCENTAJE A EMBARGAR PARA PODER GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If

                        Dim embargo As Double = (DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargo_valor / 100))

                        Dim guardar() As String = saveResolucion(lblExpediente.Text.Trim, selectcod)
                        If guardar(0) = Nothing Then
                            Alert("ESTE REPORTE POSEE DESMARCADO EL CAMPO PARA GENERAR DOCUMENTACION")
                            Exit Sub
                        End If

                        Dim tbl1 As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)
                        If tbl1 Is Nothing = True Then
                            Alert("NO HAY  DATOS DE TITULO DE DEPOSITO JUDICIAL ")
                            Exit Sub
                        End If
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        Parametros(0).Marcador = "resolucion_anterior"
                        Parametros(0).Valor = resolucion(0).ToUpper

                        Parametros(1).Marcador = "fecha_anterior"
                        Parametros(1).Valor = resolucion(1).ToUpper

                        Parametros(2).Marcador = "resolucion_embargo"
                        Parametros(2).Valor = resolucion_E(0).ToUpper

                        Parametros(3).Marcador = "fecha_resolucion"
                        Parametros(3).Valor = resolucion_E(1).ToUpper

                        Parametros(4).Marcador = "fecha_actual"
                        Parametros(4).Valor = guardar(1).ToUpper

                        Parametros(5).Marcador = "nro_resolucion"
                        Parametros(5).Valor = guardar(0).ToUpper

                        Parametros(6).Marcador = "Procedencia"
                        Parametros(6).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                        Parametros(7).Marcador = "letras_deuda"
                        Parametros(7).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)


                        Parametros(8).Marcador = "letras_embargo"
                        Parametros(8).Valor = Num2Text(embargo).ToUpper

                        Parametros(9).Marcador = "Total_Embargo"
                        Parametros(9).Valor = String.Format("{0:C0}", embargo)

                        Parametros(10).Marcador = "fecha1"
                        Parametros(10).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo).ToString("'del' dd 'de' MMMM 'de' yyy")

                        Parametros(11).Marcador = "fecha_desembargo"
                        Parametros(11).Valor = resolucion_Desembargo(1)
                        Parametros(12).Marcador = "resolucion_desembargo"
                        Parametros(12).Valor = resolucion_Desembargo(0)
                        Parametros(13).Marcador = "letras"
                        Parametros(13).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        Parametros(14).Marcador = "fecha_emision"
                        Parametros(14).Valor = guardar(3)
                        'If vc_datos(0) = Nothing Then

                        SaveTable(lblExpediente.Text, "316")

                        'End If

                        worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.TerminacionProcesoCoactivo_Multa, Parametros, tbl1, 0, False, tbl1, 1, False, Nothing, 0, False)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else

                            Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                            SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("ESTE ACTO SOLO SE PUEDE PROCESAR PARA TITULOS DE MULTA ")
                        Exit Sub
                    End If

                End If



                Validator.Text = "Consulta satisfactoria. <br /> <b>IMPRIMIO : " & Lista.SelectedItem.Text & "</b>"
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                Alert(Validator.Text, DtsDatos)
            Else
                Validator.Text = "No hay datos para mostrar."
                Alert(Validator.Text)
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If
        Catch ex As Exception
            Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Es posible que el reporte este inhabilitado."
            Me.Validator.IsValid = False
            mensup.InnerHtml = Validator.Text
            ''Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "mensaje", "<script type=\'text/javascript\'> alert(\'Debe introducir texto en el campo\'); </script>", False)

        End Try

    End Sub

    Public Sub resolucion_ActoPrevio(ByVal Me_Expediente As String, ByVal Repo As String, ByVal Impuesto As Integer, ByVal Resolucion_ActoPrevioDatos As DataTable)
        Using connection As New SqlConnection(Funciones.CadenaConexion)
            Using Command As New System.Data.SqlClient.SqlCommand("SELECT DG_NRO_DOC AS RESOLUCION,DG_FECHA_DOC AS FECHA,DG_EXPEDIENTE AS EXPEDIENTE FROM DOCUMENTOS_GENERADOS A WHERE DG_EXPEDIENTE = @DOC_EXPEDIENTE AND DG_COD_ACTO = (SELECT DXI_ACTO_PREVIO FROM dbo.DOCUMENTO_INFORMEXIMPUESTO WHERE NOT DXI_ACTO_PREVIO IS NULL AND DXI_ACTO = @DOC_ACTOADMINISTRATIVO AND  DXI_IMPUESTOVALUE = @IMPUESTOVALUE)", connection)
                connection.Open()
                Command.CommandTimeout = 60000
                Command.Parameters.Add("@DOC_EXPEDIENTE", SqlDbType.VarChar).Value = Me_Expediente
                Command.Parameters.Add("@DOC_ACTOADMINISTRATIVO", SqlDbType.VarChar).Value = Repo
                Command.Parameters.Add("@IMPUESTOVALUE", SqlDbType.VarChar).Value = Impuesto

                Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                Resolucion_ActoPrevioDatos.Load(reader)
                reader.Close()
                connection.Close()
            End Using
        End Using
    End Sub

    Public Sub resolucion_Acto229(ByVal Me_Expediente As String, ByVal Repo As String, ByVal Impuesto As Integer, ByVal Dts As Reportes_Admistratiivos)
        Using connection As New SqlConnection(Funciones.CadenaConexion)
            Using Command As New System.Data.SqlClient.SqlCommand("SELECT DG_NRO_DOC AS RESOLUCION,DG_FECHA_DOC AS FECHA,DG_EXPEDIENTE AS EXPEDIENTE FROM DOCUMENTOS_GENERADOS A WHERE DG_EXPEDIENTE = @DOC_EXPEDIENTE AND DG_COD_ACTO = @DOC_ACTOADMINISTRATIVO", connection)
                connection.Open()
                Command.CommandTimeout = 60000
                Command.Parameters.Add("@DOC_EXPEDIENTE", SqlDbType.VarChar).Value = Me_Expediente
                Command.Parameters.Add("@DOC_ACTOADMINISTRATIVO", SqlDbType.VarChar).Value = "229"
                Command.Parameters.Add("@IMPUESTOVALUE", SqlDbType.VarChar).Value = Impuesto

                Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                Dts.RESOLUCION_ACTO229.Load(reader)
                reader.Close()
                connection.Close()
            End Using
        End Using
    End Sub

    Protected Sub txtEnte_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEnte.TextChanged
        If txtEnte.Text <> Nothing Or (Not Session("UsuarioValido") Is Nothing) Then
            'ListExpedientes.Items.Clear()
            Dim idEntidad As String
            idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
            'expediente(idEntidad)
            Dim cmd As String
            Dim cnn As String = Funciones.CadenaConexion
            cmd = "select distinct docexpediente from documentos where RTRIM(entidad) = @entidad AND RTRIM(documentos.cobrador) = @cobrador"
            Dim MyAdapter As New SqlClient.SqlDataAdapter(cmd, cnn)
            MyAdapter.SelectCommand.Parameters.Add("@entidad", SqlDbType.VarChar)
            MyAdapter.SelectCommand.Parameters("@entidad").Value = idEntidad
            MyAdapter.SelectCommand.Parameters.Add("@cobrador", SqlDbType.VarChar)
            MyAdapter.SelectCommand.Parameters("@cobrador").Value = Session("mcobrador")
            Dim myTable As New DataTable
            MyAdapter.Fill(myTable)
            If myTable.Rows.Count > 0 Then
                'Me.ModalPopupExtender1.Show()
                'pnlSeleccionarDatos.Visible = True
                Dim xt As Integer
                For xt = 0 To myTable.Rows.Count - 1
                    '    ListExpedientes.Items.Add(myTable.Rows(xt).Item("docexpediente"))
                Next
            ElseIf myTable.Rows.Count > 0 Then
                'Mostrar el historico de los hepedientes 
                'expediente(idEntidad, myTable.Rows(0).Item("docexpediente"))
                'examenExpediente(myTable.Rows(0).Item("docexpediente"), "")
                lblExpediente.Text = myTable.Rows(0).Item("docexpediente")
            Else
                Validator.Text = "No se detectaron expedientes."
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If
        Else
            Validator.Text = "Favor digitar la cédula, Nit  o nombre del deudor."
            mensup.InnerHtml = Validator.Text
            Me.Validator.IsValid = False
            mensup.InnerHtml = Validator.Text
        End If
    End Sub


    Public Function GeneraDoc(ByVal Acto As String) As Boolean
        Dim _Return As Boolean
        Using CommandTieneResol As New System.Data.SqlClient.SqlCommand("SELECT * FROM ACTUACIONES WHERE GENERADOC = 1 AND CODIGO = (SELECT DXI_ACTO_PREVIO FROM dbo.DOCUMENTO_INFORMEXIMPUESTO WHERE NOT DXI_ACTO_PREVIO IS NULL AND DXI_ACTO = @CODIGO AND  DXI_IMPUESTOVALUE = @IMPUESTOVALUE)", New SqlConnection(Funciones.CadenaConexion))
            CommandTieneResol.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = Acto
            CommandTieneResol.Parameters.Add("@IMPUESTOVALUE", SqlDbType.VarChar).Value = Session("ssimpuesto")
            Dim tbTieneResol As New DataTable
            CommandTieneResol.Connection.Open()
            Dim readerCommandTieneResol As System.Data.SqlClient.SqlDataReader = CommandTieneResol.ExecuteReader(CommandBehavior.CloseConnection)
            tbTieneResol.Load(readerCommandTieneResol)
            readerCommandTieneResol.Close()
            CommandTieneResol.Connection.Close()
            If tbTieneResol.Rows.Count > 0 Then
                _Return = True
            Else
                _Return = False
            End If
            Return _Return
        End Using
    End Function


    Private Sub Literal_Coceptos_de_la_Deuda(ByVal dts As Reportes_Admistratiivos, ByVal expediente As Integer)
        Dim literas As String = Funciones.Literal_Coceptos_de_la_Deuda(expediente)

        If dts.Tables("Mandaniento_Pago").Rows.Count > 0 Then
            '---Cambiar en Tabla de mandaniento de pago
            dts.Tables("Mandaniento_Pago").Rows(0).Item("MAN_LIQUIDACIONES_LITERAL") = literas
        End If


    End Sub


    Private Function validarExpedientes(ByVal exp As String) As Boolean
        Dim _Return As Boolean
        Using CommandTieneResol As New System.Data.SqlClient.SqlCommand("SELECT * FROM EJEFISGLOBAL WHERE EFIUSUASIG = @USUARIO AND EFINROEXP = @EXPEDIENTE AND EFIMODCOD = @IMPUESTOVALUE", New SqlConnection(Funciones.CadenaConexion))
            CommandTieneResol.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = exp
            CommandTieneResol.Parameters.Add("@IMPUESTOVALUE", SqlDbType.VarChar).Value = Session("ssimpuesto")
            CommandTieneResol.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = Session("sscodigousuario")
            Dim tbTieneResol As New DataTable
            CommandTieneResol.Connection.Open()
            Dim readerCommandTieneResol As System.Data.SqlClient.SqlDataReader = CommandTieneResol.ExecuteReader(CommandBehavior.CloseConnection)
            tbTieneResol.Load(readerCommandTieneResol)
            readerCommandTieneResol.Close()
            CommandTieneResol.Connection.Close()
            If tbTieneResol.Rows.Count > 0 Then
                _Return = True
            Else
                _Return = False
            End If
            Return _Return
        End Using

    End Function

    Public Sub cargar_objeto(ByVal _obje As Object, _
     ByVal sql As String, ByVal Captura As String, ByVal CampoMostrar As String, ByVal etapa As String)

        Dim cn As New SqlClient.SqlConnection(Funciones.CadenaConexion) 'Nueva conexión indicando al SqlConnection la cadena de conexión  
        Try
            Dim cmd As New SqlClient.SqlCommand(sql, cn) 'Pasar la consulta sql y la conexión al Sql Command   
            cmd.Parameters.AddWithValue("@IDETAPA", etapa)
            cmd.Parameters.AddWithValue("@DXI_IMPUESTOVALUE", Session("ssimpuesto"))
            cmd.Parameters.AddWithValue("@DXI_HISTORIAL", True)

            Dim da As New SqlClient.SqlDataAdapter(cmd) 'Inicializar un nuevo SqlDataAdapter   
            Dim tb As New DataTable 'Crear y Llenar una Tabla
            da.Fill(tb) 'llena la tabla dependiendo la consulta realizada

            _obje.DataSource = tb 'Asignar el DataSource al combobox  
            _obje.DataValueField = Captura 'Campo a capturar
            _obje.DataTextField = CampoMostrar 'Campo a mostar
            _obje.DataBind() 'Enlaza un origen de datos al control de servidor invocado y a todos sus controles secundarios.            

        Catch ex As Exception
            ex.ToString()  'Si hay alguna excepcion
        Finally
        End Try
    End Sub

    Private Sub SendReport(ByVal NombreArchivo As String, ByVal Plantilla As String)
        ''Dim nav As String = Request.Browser.Browser
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/msword"
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.doc", NombreArchivo))
        Response.Write(Plantilla)
        Response.Flush()





    End Sub

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



    Private Sub proyectar(Optional ByVal cod_ActoPrevio As String = Nothing)
        Try
            'variable que genera el reporte winword  '
            Dim worddoc As New WordReport
            Dim worddocresult As String = ""
            Dim fecha, fecha2 As Date
            Dim idEntidad As String = Nothing
            idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
            Dim selectcod As String
            If cod_ActoPrevio = "" Then
                selectcod = valida_Repo()
            End If

            Dim MytableRepo As Boolean
            Dim Ado As New SqlConnection(Funciones.CadenaConexion)
            Dim DtsDatos As New Reportes_Admistratiivos
            Dim myBaseClass As New procesos_tributario.sqlQuery.sqlQuery(Ado) 'CONEXION EJECUTA EL SELECT SEGUN LA CONSULTA ... EN LA BASE DE DATOS 
            myBaseClass.Impuesto = Session("ssimpuesto")
            myBaseClass.Expedinete = lblExpediente.Text.Trim
            myBaseClass.Repo = selectcod
            myBaseClass.Datos_Ente = DtsDatos.CAT_CLIENTES
            myBaseClass.Datos_Informe = DtsDatos.Mandaniento_Pago
            myBaseClass.Datos_Acto_Previo = DtsDatos.ENTRA_DOCUMENTOMA
            myBaseClass.Datos_Conceptos_Deuda = DtsDatos.CONCEPTOS_DEUDA
            myBaseClass.Datos_Conceptos_Deuda_Indus = DtsDatos.CONCEPTOS_DEUDA_INDUS
            myBaseClass.Load_Acto_Previo = IIf(cod_ActoPrevio = Nothing, False, True)
            myBaseClass.Ente = Session("mcobrador")
            myBaseClass.ConexionME = Ado
            myBaseClass.mnivelacces = Session("mnivelacces")
            myBaseClass.sscodigousuario = Session("sscodigousuario")
            MytableRepo = myBaseClass.Prima_EjecucionFiscal(Session("ssCodimpadm"), 1)

            If DtsDatos.Mandaniento_Pago.Rows.Count > 0 Then

                'RESOLUCION Y FECHA DEL ACTO PREVIO
                Dim tipo As String = ""
                Dim Load_Acto_Previo As Boolean = GeneraDoc(selectcod)
                If Load_Acto_Previo = True Then
                    tipo = "AND D.TIPO = 1"
                    resolucion_ActoPrevio(myBaseClass.Expedinete, myBaseClass.Repo, myBaseClass.Impuesto, DtsDatos.RESOLUCION_ACTOPREVIO)
                    If DtsDatos.RESOLUCION_ACTOPREVIO.Rows.Count = 0 Then
                        Validator.Text = "<b> No se ha generado la resolucion para el acto  " & Lista.SelectedItem.Text & "</b>"
                        mensup.InnerHtml = Validator.Text
                        'Exit Sub
                    End If
                End If


                Select Case selectcod
                    Case "228", "013", "233", "354", "355", "356", "226", "363", "230", "214", "235", "315", "319", "320", "321", "353", "355", "352", "368", "351", "366", "367", "359", "360", "370"
                        tipo = "AND D.TIPO = 1"
                    Case Else

                End Select


                Dim AdapterMT As SqlClient.SqlDataAdapter

                'CARGAR TABLA DE TITULOS 
                Select Case selectcod

                    Case "228", "013", "233", "354", "355", "356", "226", "363", "230", "214", "235", "315", "319", "320", "321", "353", "355", "352", "368", "316", "366", "367", "351", "352", "359", "360", "370", "350"
                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT TOP 1 A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,F.codigo as codigotitulo,                                                                                                                                                                                                              " & _
                                                               " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP, E.MT_FEC_EXI_LIQ                                                                                                                                                                                                                                 " & _
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
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                              " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                          " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                          " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                       " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                         " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                             " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                           " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                 " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & tipo & "                                                                                                                                                                                               " & _
                                                               " ORDER BY TIPO", Ado)

                    Case "349", "358", "380"
                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                                                               " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP,M.BAN_DIRECCION AS DIRECCIONBANCARIA , M.BAN_TELEFONO AS TELEFONOBANCARIO , M.BAN_NOMBRE AS NOMBREBANCARIO, F.codigo as codigotitulo                       " & _
                                                               " FROM  EJEFISGLOBAL A,                                                                                                                                                                                                                      " & _
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
                                                               " 	  USUARIOS L, MAESTRO_BANCOS M                                                                                                                                                                                                          " & _
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                               " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                           " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                           " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                        " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                          " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                              " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                            " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                  " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & tipo & "                                                                                                                                                                                               " & _
                                                               " ORDER BY TIPO", Ado)

                    Case "217", "219", "221", "222", "301", "307", "329"

                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT A.EFINROEXP AS MAN_EXPEDIENTE, " & _
                        " G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                        " B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                        " C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,  " & _
                        " E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo,  " & _
                        " E.MT_fec_expedicion_titulo, " & _
                        " ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo, '' AS MT_for_notificacion_titulo , " & _
                        " '$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda, " & _
                        " E.totaldeuda,                                                              " & _
                        " CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento," & _
                        " CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio, " & _
                        " k.nombre as Proyecto, L.nombre as Revisor,ED_TipoPersona as DOCUMENTO, " & _
                     " D.TIPO as Tipo_Deudor, c.idunico as IdDireccion, " & _
                     " A.EFINUMMEMO,E.MT_fecha_ejecutoria,a.EFIFECHAEXP, F.codigo as codigotitulo " & _
                     " FROM  EJEFISGLOBAL A,                " & _
                      " ENTES_DEUDORES B,  DIRECCIONES C , " & _
                      " DEUDORES_EXPEDIENTES D, MAESTRO_TITULOS E, " & _
                      " TIPOS_TITULO F, TIPOS_IDENTIFICACION G, " & _
                      " DEPARTAMENTOS H, MUNICIPIOS I, " & _
                      " USUARIOS K, USUARIOS L " & _
                      " WHERE A.EFINROEXP = D.NROEXP " & _
                      " AND   D.DEUDOR = B.ED_Codigo_Nit " & _
                      " AND   B.ED_Codigo_Nit = C.deudor  " & _
                      " AND   E.MT_expediente = A.EFINROEXP " & _
                      " AND   F.codigo = E.MT_tipo_titulo " & _
                      " AND   G.codigo  = B.ED_TipoId " & _
                      " AND   H.codigo = C.Departamento " & _
                      " AND   I.codigo = C.Ciudad " & _
                      " AND   K.codigo = A.EFIUSUASIG " & _
                      " AND   L.codigo = A.EFIUSUREV " & _
                      " AND   A.EFINROEXP = @EXPEDIENTE " & tipo & " " & _
                      " ORDER BY TIPO ", Ado)
                    Case Else
                        AdapterMT = New SqlClient.SqlDataAdapter(" SELECT  A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                                                               " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP, F.codigo as codigotitulo                                                                                                                                                       " & _
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
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                              " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                          " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                          " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                       " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                         " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                             " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                           " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                 " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & tipo & "                                                                                                                                                                                               " & _
                                                               " ORDER BY TIPO", Ado)

                End Select

                AdapterMT.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                Try
                    DtsDatos.DATOS_REPORTES.Clear()
                    AdapterMT.Fill(DtsDatos.DATOS_REPORTES)
                    If DtsDatos.DATOS_REPORTES.Rows.Count = 0 Then
                        Alert("  SE RECOMIENDA VERIFICAR LOS DATOS DE PARAMETRIACION " & vbNewLine & _
                              "  NO SE ENCUENTRAN, " & vbNewLine & _
                              "  DEBE VERIFICAR LA FECHA DE EXPEDICION DE TITULO, " & vbNewLine & _
                              "  LA FECHA DE EJECUTORIA , Y SI POSEE EL TITULO EJECUTIVO " & vbNewLine & _
                              "  EN LA PESTAÑA DE INFORMACION GENERAL " & vbNewLine & _
                              "  EN LA PESTAÑA TITULO EJECUTIVO " & vbNewLine & _
                              "  VERIFICAR QUE ESTE INGRESADO  LA FORMA DE NOTIFICACION " & vbNewLine & _
                              "  DEL TITULO EN LA PESTAÑA INFORMACION GENERAL " & vbNewLine & _
                              "  EN LA PESTAÑA TITULO EJECUTIVO " & vbNewLine & _
                              "  DEUDORES  EN LA PESTAÑA INFORMACION GENERAL EN LA PESTAÑA DEUDOR ")

                        Exit Sub
                    End If
                Catch ex As Exception
                    Alert(ex.Message)
                End Try
                'saveResolucion(lblExpediente.Text.Trim, selectcod, DtsDatos)


                'Reportes administrativos Cobro Coactivo
                If selectcod = "013" Then

                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fecha_ejecutoria
                    If IsDBNull(fecha2) Then
                        Alert("NO SE HA INGRESADO FECHA DE EJECUTORIA DEL TITULO ")
                        Exit Sub
                    End If

                    Dim parametros(6) As WordReport.Marcadores_Adicionales
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "Nro_Resolucion"
                    parametros(0).Valor = "XXXXXXXXX"
                    parametros(1).Marcador = "fecha_actual"
                    parametros(1).Valor = "XXXXXXXXX"
                    parametros(2).Marcador = "letras"
                    parametros(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    parametros(3).Marcador = "total_deudas"
                    parametros(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).Total_deuda
                    parametros(4).Marcador = "fecha1"
                    parametros(4).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(5).Marcador = "fecha_ejecutoria"
                    parametros(5).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(6).Marcador = "fecha_emision"
                    parametros(6).Valor = "XXXXXXXXX"
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.MandamientoPagoPorPila, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If


                End If

                If selectcod = "056" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper




                    datos(1).Marcador = "nro_resolucion"
                    datos(1).Valor = "XXXXXXXXX"
                    datos(2).Marcador = "fecha_reg"
                    datos(2).Valor = "XXXXXXXXX"

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionPorCorreo, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)



                    End If



                End If

                If selectcod = "228" Then

                    Dim datos_valor(1) As WordReport.Marcadores_Adicionales
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_valor(1).Marcador = "NRO"
                    datos_valor(1).Valor = DtsDatos.DATOS_REPORTES.Rows.Count

                    For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                        DtsDatos.DATOS_REPORTES.Item(i).Municipio = DtsDatos.DATOS_REPORTES.Item(i).MT_fec_expedicion_titulo.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        DtsDatos.DATOS_REPORTES.Item(i).MT_tipo_titulo = DtsDatos.DATOS_REPORTES.Item(i).MT_tipo_titulo & " " & DtsDatos.DATOS_REPORTES.Item(i).MT_nro_titulo
                        DtsDatos.DATOS_REPORTES.Item(i).Departamento = ""
                    Next

                    worddocresult = worddoc.CreateReportWithTable(DtsDatos.DATOS_REPORTES, Reportes.DevolucionTituloTesoreria, "MT_tipo_titulo,Municipio,ED_Nombre,Departamento,totaldeuda", datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If

                If selectcod = "229" Then

                    Dim buscar As String

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        buscar = 319
                    Else
                        buscar = 320
                    End If
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    If embargos <= 0 Then
                        Alert("Verificar si posee porcentaje de embargo para continuar con la generacion del reporte ")
                        Exit Sub
                    End If

                    Dim datos_valor(4) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = overloadresolucion(lblExpediente.Text.Trim, buscar)

                    For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                        datos_valor(0).Marcador = "ED_Codigo_Nit"
                        datos_valor(0).Valor = DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId & DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit
                        'DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit = DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId & DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit
                    Next

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    If resolucion(0) <> Nothing Then
                        datos_valor(1).Marcador = "XXXXXXXX"
                        datos_valor(1).Valor = resolucion(0)
                        datos_valor(2).Marcador = "XXXXXXXXX"
                        datos_valor(2).Valor = resolucion(1)
                    Else
                        Alert("NO SE HA GENERADO RESOLUCION DE EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO  ")
                        Exit Sub
                    End If

                    datos_valor(3).Marcador = "fecha_actual"
                    datos_valor(3).Valor = "XXXXXXXXXXXXXXX"
                    datos_valor(4).Marcador = "Nro_resolucion"
                    datos_valor(4).Valor = "XXXXXXXXXXXXXXX"
                    DtsDatos.DATOS_REPORTES.Item(0).MT_tipo_titulo = "RCC-" & resolucion(0).ToUpper & " " & resolucion(1)

                    DtsDatos.DATOS_REPORTES.Item(0).Total_deuda = String.Format("{0:C0}", (CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda) * (embargos / 100)))

                    worddocresult = worddoc.CreateReportWithTable(DtsDatos.DATOS_REPORTES, Reportes.LevantamientodeMedidaCautelar, "ED_NOMBRE,ED_Codigo_Nit,MT_tipo_titulo,Total_Deuda", datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "230" Or selectcod = "368" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo

                        Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, selectcod)

                        'datos detallado de la deuda
                        Dim tbl2 As New DataTable
                        tbl2.Columns.AddRange(New DataColumn() {New DataColumn("SUBSISTEMA", GetType(String)), _
                                                 New DataColumn("CAPITAL_1", GetType(String)), _
                                                 New DataColumn("INTERESES_1", GetType(String)), _
                                                 New DataColumn("TOTAL_A_PAGAR_1", GetType(String)), _
                                                 New DataColumn("CAPITAL", GetType(Double)), _
                                                 New DataColumn("INTERESES", GetType(Double)), _
                                                 New DataColumn("TOTAL_A_PAGAR", GetType(Double))})


                        Dim tb1 As New DataTable

                        Dim ad As New SqlDataAdapter("SELECT SUBSISTEMA, '' AS AJUSTE, '' AS INTERESES_MORATORIOS, '' AS SALDO_TOTAL, SUM( AJUSTE) AS CAPITAL ,A.MES,A.ANNO FROM  SQL_PLANILLA A , MAESTRO_MES B WHERE A.MES = B.ID_MES AND A.EXPEDIENTE = @EXPEDIENTE GROUP BY SUBSISTEMA, ANNO, B.NOMBRE,B.ID_MES,A.MES ORDER BY SUBSISTEMA,ANNO,B.ID_MES", Funciones.CadenaConexion)
                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)

                        ad.Fill(tb1)
                        If tb1.Rows.Count > 0 Then


                            'Obterner dia habiles de pago
                            Dim tb2 As New DataTable

                            ad = New SqlDataAdapter("SELECT A.DEUDOR,D.CODIGO FROM DEUDORES_EXPEDIENTES A , TIPOS_ENTES B, ENTES_DEUDORES C,TIPOS_APORTANTES D WHERE A.DEUDOR = C.ED_CODIGO_NIT AND A.TIPO = B.CODIGO AND C.ED_TIPOAPORTANTE = D.CODIGO AND B.CODIGO = '1' AND A.NROEXP = @EXPEDIENTE ", Funciones.CadenaConexion)
                            ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                            ad.Fill(tb2)


                            Dim diaPago As String = FuncionesInteresesParafiscales.ObtenerDiaPago(tb2.Rows(0).Item("DEUDOR").ToString, CInt(tb2.Rows(0).Item("CODIGO")))

                            Dim subsistema As String = ""
                            Dim subsistemaSig As String = ""
                            Dim c, i, t, x, sw As Double
                            c = 0
                            i = 0
                            t = 0
                            x = 0
                            sw = 0
                            For Each row As DataRow In tb1.Rows
                                subsistemaSig = row("SUBSISTEMA")
                                sw = sw + 1
                                Dim deudaCapital As Double = CDbl(row("CAPITAL"))
                                Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, row("ANNO"), row("MES"), CInt(tb2.Rows(0).Item("CODIGO")), row("SUBSISTEMA")).ToString("dd/MM/yyyy")
                                'Alterar grid colocar datos faltantes
                                row("INTERESES_MORATORIOS") = FuncionesInteresesParafiscales._CalcularIntereses(CDbl(row("CAPITAL")), fechaExigibilidad.ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"), CDec(ViewState("diaria")))
                                row("SALDO_TOTAL") = CDbl(row("INTERESES_MORATORIOS")) + deudaCapital


                                If subsistema = subsistemaSig Then
                                    x = 0
                                End If

                                'Agregar acumulado a la tabla que se va a mostrar en la minuta.
                                If x > 0 And subsistema <> subsistemaSig Then
                                    tbl2.Rows.Add(subsistema, "", "", "", c, i, t)
                                    c = 0
                                    i = 0
                                    t = 0
                                End If

                                ' Acumular montos 
                                c = c + row("CAPITAL")
                                i = i + row("INTERESES_MORATORIOS")
                                t = t + row("SALDO_TOTAL")
                                x = 1
                                subsistema = row("SUBSISTEMA")

                                'Agregar ultimo acumulado a la tabla que se va a mostrar en la minuta.
                                If sw = tb1.Rows.Count Then
                                    tbl2.Rows.Add(subsistema, "", "", "", c, i, t)
                                End If

                            Next
                            ''For n = 0 To tbl2.Rows.Count - 1

                            ''Next

                            'Agregar totales a la tabla
                            tbl2.Rows.Add("TOTALES", "", "", "", tbl2.Compute("SUM(CAPITAL)", String.Empty), tbl2.Compute("SUM(INTERESES)", String.Empty), tbl2.Compute("SUM(TOTAL_A_PAGAR)", String.Empty))

                            'Formatear datos a tipo moneda
                            For Each row As DataRow In tbl2.Rows
                                If IsDBNull(row("INTERESES")) = True Then
                                    row("INTERESES_1") = String.Format("{0:C0}", CDbl(0))
                                Else
                                    row("INTERESES_1") = String.Format("{0:C0}", CDbl(row("INTERESES")))
                                End If

                                If IsDBNull(row("TOTAL_A_PAGAR")) = True Then
                                    row("TOTAL_A_PAGAR_1") = String.Format("{0:C0}", CDbl(0))
                                Else
                                    row("TOTAL_A_PAGAR_1") = String.Format("{0:C0}", CDbl(row("TOTAL_A_PAGAR")))
                                End If

                                If IsDBNull(row("CAPITAL")) = True Then
                                    row("CAPITAL_1") = String.Format("{0:C0}", CDbl(0))
                                Else
                                    row("CAPITAL_1") = String.Format("{0:C0}", CDbl(row("CAPITAL")))
                                End If

                            Next
                        Else
                            Alert("NO HAY SQL PARA GENERAR EL REPORTE SOLICITADO ")
                            Exit Sub

                        End If


                        Dim datos(6) As WordReport.Marcadores_Adicionales
                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "fecha2"
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(2).Marcador = "fecha_actual"
                        datos(2).Valor = "XXXXXXXXXXXXXXXXX"
                        datos(3).Marcador = "letras"
                        datos(3).Valor = Num2Text(CInt(tbl2.Rows.Item(tbl2.Rows.Count - 1)(6))).ToUpper
                        'Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        datos(4).Marcador = "Nro_Resolucion"
                        datos(4).Valor = "XXXXXXXXXXXXXXXXXXX"
                        datos(5).Marcador = "total"
                        datos(5).Valor = String.Format("{0:C0}", CDbl(tbl2.Rows.Item(tbl2.Rows.Count - 1)(6)))
                        datos(6).Marcador = "fecha_emision"
                        datos(6).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.LiquidacionCreditoCosta, datos, tbl2, 0, False, Nothing, 0, False, Nothing, 0, False)
                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                        End If


                    Else

                        '---- DIFERENTES A LIQUIDACIONES OFICIALES...
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo

                        Dim datos(9) As WordReport.Marcadores_Adicionales
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "fecha2"
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(2).Marcador = "fecha_actual"
                        datos(2).Valor = "XXXXXXXXX"
                        datos(3).Marcador = "letras"
                        datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                        datos(4).Marcador = "Nro_Resolucion"
                        datos(4).Valor = "XXXXXXXXXXXXXXXXXX"
                        datos(5).Marcador = "total"
                        datos(5).Valor = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda))

                        Dim diasMora As Integer = FuncionesInteresesMultas.CalcularDiasMoras(CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_exi_liq).ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"))
                        Dim intereses As Double = FuncionesInteresesMultas.CalcularInteresesMoras(CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda), diasMora)

                        datos(6).Marcador = "1_COLUMN"
                        datos(6).Valor = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda))
                        datos(7).Marcador = "2_COLUMN"
                        datos(7).Valor = String.Format("{0:C0}", CDbl(intereses))
                        datos(8).Marcador = "3_COLUMN"
                        datos(8).Valor = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda) + CDbl(intereses))
                        datos(9).Marcador = "fecha_emision"
                        datos(9).Valor = "XXXXXXXXXXXXXXXXXXXX"


                        worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.liquidaciondecreditomulta, datos, Nothing, 0, False, Nothing, 0, False, Nothing, 0, False)
                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                        End If

                    End If
                End If




                If selectcod = "233" Then

                    Dim datos_valor(8) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos(2) As String
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_valor(1).Marcador = "fecha2"
                    datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_valor(2).Marcador = "letras"

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    vc_datos = overloadresolucion(lblExpediente.Text, "230")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper

                        If vc_datos(1) = "" Then
                            resolucion(1) = "01 DE ENERO DEL 1900 "
                        Else
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper()


                        End If

                    Else

                    End If
                    datos_valor(2).Valor = Num2Text(GetLiquidacion(lblExpediente.Text.Trim, resolucion(0))).ToUpper
                    If resolucion(0) <> Nothing Then
                        datos_valor(3).Marcador = "nro_resolucion2"
                        datos_valor(3).Valor = resolucion(0).ToUpper
                        datos_valor(4).Marcador = "fecha_reg"
                        datos_valor(4).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY (230) RESOLUCION DE LIQUIDACION DE CREDITO Y COSTAS PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    datos_valor(5).Marcador = "fecha_actual"
                    datos_valor(5).Valor = "XXXXXXXXXXX"
                    datos_valor(6).Marcador = "Nro_resolucion"
                    datos_valor(6).Valor = "XXXXXXXXXXXXXXX"
                    datos_valor(7).Marcador = "Total_Liquidacion"
                    datos_valor(7).Valor = String.Format("{0:C0}", GetLiquidacion(lblExpediente.Text.Trim, resolucion(0)))
                    datos_valor(8).Marcador = "fecha_emision"
                    datos_valor(8).Valor = "XXXXXXXXXXXXXXXXXX"

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.Aprobaciónliquidacióndelcrédito, datos_valor)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If


                If selectcod = "314" Then
                    Dim parametros(7) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    Dim Excepciones() As String = GetExcepcion(lblExpediente.Text.Trim)
                    If Excepciones(0) = "" Then
                        Alert("NO HAY RADICADO DE PARA GENERAR INFORME DE EXCEPCIONES ")
                        Exit Sub
                    End If
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "fecha1"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(1).Marcador = "Letras"
                    parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    parametros(2).Marcador = "fecha2"
                    parametros(2).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper


                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper


                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametros(3).Marcador = "nro_resolucion"
                        parametros(3).Valor = resolucion(0).ToUpper
                        parametros(4).Marcador = "fecha_reg"
                        parametros(4).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO  PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    parametros(5).Marcador = "Nro_res"
                    parametros(5).Valor = "XXXXXXXXXXX"
                    parametros(6).Marcador = "fecha_actual"
                    parametros(6).Valor = "XXXXXXXXXXXXXXX"
                    parametros(7).Marcador = "fecha_emision"
                    parametros(7).Valor = "XXXXXXXXXXXXXXX"
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AperturaDePruebasLiquidacion, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "315" Then
                    Dim parametros(6) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "fecha1"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    parametros(1).Marcador = "Letras"
                    parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    parametros(2).Marcador = "fecha2"
                    parametros(2).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    vc_datos = overloadresolucion(lblExpediente.Text, "230")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametros(3).Marcador = "resolucion_anterior"
                        parametros(3).Valor = resolucion(0).ToUpper
                        parametros(4).Marcador = "fecha_anterior"
                        parametros(4).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION (230) LIQUIDACION CREDITO Y COSTAS DE PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    parametros(5).Marcador = "fecha_actual"
                    parametros(5).Valor = "XXXXXXXXXX"
                    parametros(6).Marcador = "Nro_resolucion"
                    parametros(6).Valor = "XXXXXXXXXX"

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AprobacionLiquidacionCreditoLiquidacionOficial, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "317" Then
                    Dim parametro(1) As WordReport.Marcadores_Adicionales
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_resolucion"
                        parametro(0).Valor = resolucion(0).ToUpper
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CitacionMandamientoPago, parametro)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "319" Then

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        Dim tbResoluciones As DataTable
                        Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                        If datos.Rows.Count > 0 Then

                            tbResoluciones = datos
                        Else
                            tbResoluciones = GetList_Embargos2("320", lblExpediente.Text.Trim)
                        End If


                        If tbResoluciones.Rows.Count > 0 Then
                            list_embargos.Items.Clear()
                            For Each row As DataRow In tbResoluciones.Rows
                                list_embargos.Items.Add(row("NRORESOLEM"))
                            Next


                            ModalPopupExtender2.Show()
                        Else
                            Limite.Text = "200"
                            ModalPopupExtender3.Show()
                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS EJECUTIVOS DE LIQUIDACION OFICIAL")
                        Exit Sub
                    End If
                    Exit Sub
                End If



                If selectcod = "320" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        Dim tbResoluciones As DataTable
                        Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                        If datos.Rows.Count > 0 Then

                            tbResoluciones = datos
                        Else
                            tbResoluciones = GetList_Embargos2("320", lblExpediente.Text.Trim)
                        End If

                        If tbResoluciones.Rows.Count > 0 Then
                            list_embargos.Items.Clear()
                            For Each row As DataRow In tbResoluciones.Rows
                                list_embargos.Items.Add(row("NRORESOLEM"))
                            Next

                            ModalPopupExtender2.Show()
                        Else
                            Limite.Text = "150"
                            ModalPopupExtender3.Show()

                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS EJECUTIVOS DE MULTA ")
                        Exit Sub
                    End If
                    Exit Sub


                End If


                If selectcod = "321" Then


                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        Dim parametros(5) As WordReport.Marcadores_Adicionales
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                        Dim vc_datos() As String
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        parametros(0).Marcador = "fecha_reg"
                        parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        parametros(1).Marcador = "Letras"
                        parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion(0) <> Nothing Then
                            parametros(2).Marcador = "nro_resolucion"
                            parametros(2).Valor = resolucion(0).ToUpper
                            parametros(3).Marcador = "fecha_anterior"
                            parametros(3).Valor = resolucion(1).ToUpper
                        Else
                            parametros(2).Marcador = "nro_resolucion"
                            parametros(2).Valor = ""
                            parametros(3).Marcador = "fecha_anterior"
                            parametros(3).Valor = ""
                        End If


                        parametros(4).Marcador = "fecha_actual"
                        parametros(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        parametros(5).Marcador = "Nro_resolucion"
                        parametros(5).Valor = "XXXXXXXXXXXXXXXXXXXXXX"


                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.EmbargoCuentaBancariaMultaHospitales, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS JUDICIALES DE MULTA ")
                        Exit Sub
                    End If


                End If

                If selectcod = "322" Then
                    Dim parametros(1) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    vc_datos = overloadresolucion(lblExpediente.Text, "314")
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametros(0).Marcador = "nro_resolucion"
                        parametros(0).Valor = resolucion(0).ToUpper
                        parametros(1).Marcador = "fecha_reg"
                        parametros(1).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE ABRE A PRUEBAS PARA IMPRIMIR EL DOCUMENTO ")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.MemorandoIntegracionPruebas, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "323" Then
                    Dim parametros(5) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)

                    Dim titulo() As String = GetTituloPrincipal(lblExpediente.Text.Trim)
                    If titulo(0) = "" Then
                        Alert("NO HAY TITULO DE DEPOSITO JUDICIAL Y/O RESOLUCION DE FRACCIONAMIENTO DE TITULO")

                        Exit Sub
                    End If

                    Dim titulo2() As Double = getfraccionado(titulo(0))
                    If titulo2(0) = "" Then
                        Alert("NO HAY TITULO DE DEPOSITO JUDICIAL Y/O RESOLUCION DE FRACCIONAMIENTO DE TITULO")
                        Exit Sub
                    End If
                    parametros(0).Marcador = "resolucion_anterior"
                    parametros(0).Valor = resolucion(0).ToUpper
                    parametros(1).Marcador = "fecha_anterior"
                    parametros(1).Valor = resolucion(1).ToUpper
                    parametros(2).Marcador = "titulo_principal"
                    parametros(2).Valor = titulo(0).ToUpper
                    parametros(3).Marcador = "titulo1"
                    parametros(3).Valor = Num2Text(titulo2(0)) & " " & String.Format("{0:C0}", titulo2(0)).ToUpper
                    parametros(4).Marcador = "titulo2"
                    parametros(4).Valor = Num2Text(titulo2(1)) & " " & String.Format("{0:C0}", titulo2(1)).ToUpper
                    parametros(5).Marcador = "Total_Titulo"
                    parametros(5).Valor = String.Format("{0:C0}", CDbl(titulo(1)))
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.FraccionamientoTituloDepositoJudicial, parametros)

                    If worddocresult = "" Then

                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "324" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    Dim parametros(4) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    parametros(0).Marcador = "fecha1"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    If resolucion(0) <> Nothing Then
                        parametros(1).Marcador = "nro_resolucion"
                        parametros(1).Valor = resolucion(0).ToUpper
                        parametros(2).Marcador = "fecha_reg"
                        parametros(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE TERMINACION DE PROCESO PARA GENERAR LA DEVOLUCION DE TITULO  " & Lista.Text & "")
                        Exit Sub
                    End If
                    parametros(3).Marcador = "numerofolio"
                    parametros(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Count).ToUpper
                    parametros(4).Marcador = "folios"
                    parametros(4).Valor = DtsDatos.DATOS_REPORTES.Count
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.MemorandoDevolucionTitulo, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "325" Then
                    Dim sdatos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    sdatos(0).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        sdatos(0).Valor = representante(0)
                    Else
                        sdatos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "314")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion(0) <> Nothing Then
                        sdatos(1).Marcador = "nro_resolucion"
                        sdatos(1).Valor = resolucion(0).ToUpper
                        sdatos(2).Marcador = "fecha_reg"
                        sdatos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION ABRE A PRUEBAS  DE PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NOTIFICACIONABREAPRUEBAS, sdatos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "326" Then
                    Dim sdatos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    sdatos(0).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        sdatos(0).Valor = representante(0)
                    Else
                        sdatos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "315")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        If vc_datos(1) = "" Then
                            resolucion(1) = "01 DE ENERO DEL 1900 "
                        Else
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper()
                        End If


                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        sdatos(1).Marcador = "nro_resolucion"
                        sdatos(1).Valor = resolucion(0).ToUpper
                        sdatos(2).Marcador = "fecha_reg"
                        sdatos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE APROBACIÓN DE LIQUIDACIÓN DEL CRÉDITO - LIQUIDACIÓN OFICIAL PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NOTIFICACIONAPROBACIÓNLIQUIDACIÓNDELCRÉDITO, sdatos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "327" Then

                    Dim sdatos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    sdatos(0).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        sdatos(0).Valor = representante(0)
                    Else
                        sdatos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "316")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        sdatos(1).Marcador = "nro_resolucion"
                        sdatos(1).Valor = resolucion(0).ToUpper
                        sdatos(2).Marcador = "fecha_reg"
                        sdatos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE  TERMINACION Y ARCHIVO PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NOTIFICACIÓNDETERMINACIÓNDELPROCESO, sdatos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "328" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    datos(0).Marcador = "ED_Rep"
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    If representante(0) <> Nothing Then
                        datos(0).Valor = representante(0).ToUpper
                    Else
                        datos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre.ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    'Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "320")
                    'If vc_datos(0) <> Nothing Then
                    '    resolucion(0) = vc_datos(0)
                    '    resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    'Else

                    'End If


                    If resolucion(0) <> Nothing Then
                        datos(1).Marcador = "nro_resolucion"
                        datos(1).Valor = resolucion(0).ToUpper
                        datos(2).Marcador = "fecha_reg"
                        datos(2).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE  (230) LIQUIDACION CREDITO Y COSTAS PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionLiquidacionCredito, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "346" Then

                    Dim datos(1) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "351")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        datos(0).Marcador = "nro_resolucion"
                        datos(0).Valor = resolucion(0).ToUpper
                        datos(1).Marcador = "fecha_reg"
                        datos(1).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE  ORDEN DE EJECUCIÓN MULTA  Ò ORDEN DE EJECUCIÓN PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionOrdenEjecucion, datos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "347" Then
                    Dim datos(1) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "359")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion(0) <> Nothing Then
                        datos(0).Marcador = "nro_resolucion"
                        datos(0).Valor = resolucion(0).ToUpper
                        datos(1).Marcador = "fecha_reg"
                        datos(1).Valor = resolucion(1).ToUpper
                    Else
                        Alert("NO HAY RESOLUCION DE RESUELVE EXCEPCIONES PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.NotificacionResuelveExepciones, datos)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "348" Then

                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                    Dim tbl1 As DataTable = GetTituloPrincipal3(lblExpediente.Text.Trim)
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    datos(0).Marcador = "ED_Rep"

                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "348")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        datos(2).Marcador = "nro_devolucion"
                        datos(2).Valor = resolucion(0).ToUpper
                        datos(3).Marcador = "fecha_devolucion"
                        datos(3).Valor = resolucion(1).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE DEVOLUCION DE TITULO DE DEPOSITO JUDICIAL PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    If representante(0) <> Nothing Then
                        datos(0).Valor = representante(0).ToUpper
                    Else
                        datos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre.ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If

                    datos(1).Marcador = "fecha1"
                    datos(1).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.OficioComunicacionDevolucionTituloDepositoJudicial, datos, tbl1, 0, False, Nothing, 0, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "349" Then
                    ''   If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then


                    Dim tbResoluciones As DataTable
                    Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                    If datos.Rows.Count > 0 Then

                        tbResoluciones = datos
                    Else
                        tbResoluciones = GetList_Embargos2("319", lblExpediente.Text.Trim)
                    End If


                    If tbResoluciones.Rows.Count > 0 Then
                        list_embargos.Items.Clear()
                        For Each row As DataRow In tbResoluciones.Rows
                            list_embargos.Items.Add(row("NRORESOLEM"))
                        Next
                        ModalPopupExtender2.Show()
                    Else
                        ''ModalPopupExtender3.Show()
                    End If
                    Exit Sub
                Else
                    ''End If

                End If

                If selectcod = "380" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo <> "01" Then

                        Dim tbResoluciones As DataTable
                        Dim datos As DataTable = GetList_Emebargos(selectcod, lblExpediente.Text.Trim)
                        If datos.Rows.Count > 0 Then

                            tbResoluciones = datos
                        Else
                            tbResoluciones = GetList_Embargos2("320", lblExpediente.Text.Trim)
                        End If


                        If tbResoluciones.Rows.Count > 0 Then
                            list_embargos.Items.Clear()
                            For Each row As DataRow In tbResoluciones.Rows
                                list_embargos.Items.Add(row("NRORESOLEM"))
                            Next
                            ModalPopupExtender2.Show()
                        Else
                            ''ModalPopupExtender3.Show()
                        End If
                        Exit Sub
                    Else

                    End If


                End If




                If selectcod = "350" Then
                    Dim parametros(5) As WordReport.Marcadores_Adicionales
                    Dim busqueda As String
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = 1 Then
                        busqueda = "319"
                    Else
                        busqueda = "320"

                    End If

                    Dim resolucion() As String = overloadresolucion(lblExpediente.Text, busqueda)

                    Dim vc_datos() As String
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    parametros(0).Marcador = "fecha_reg"
                    parametros(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    parametros(1).Marcador = "Letras"
                    parametros(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    If resolucion(0) <> Nothing Then
                        parametros(2).Marcador = "nro_resolucion"
                        parametros(2).Valor = resolucion(0).ToUpper
                        parametros(3).Marcador = "fecha_resolucion"
                        parametros(3).Valor = resolucion(1).ToUpper
                    Else
                        parametros(2).Marcador = "nro_resolucion"
                        parametros(2).Valor = ""
                        parametros(3).Marcador = "fecha_resolucion"
                        parametros(3).Valor = ""
                    End If

                    parametros(4).Marcador = "Nro_Resolucion"
                    parametros(4).Valor = "XXXXXXXXXXXXXXX"

                    parametros(5).Marcador = "fecha_actual"
                    parametros(5).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ReduccionDeEmbargosPorPagoParcial, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "351" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        Dim parametros(5) As WordReport.Marcadores_Adicionales
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                        Dim vc_datos() As String
                        parametros(0).Marcador = "letras"
                        parametros(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If

                        If resolucion(0) <> Nothing Then
                            parametros(1).Marcador = "fecha_anterior"
                            parametros(1).Valor = resolucion(1).ToUpper
                            parametros(2).Marcador = "resolucion_anterior"
                            parametros(2).Valor = resolucion(0).ToUpper
                        Else
                            Alert("NO HAY RESOLUCION DE (013) MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO")
                            Exit Sub

                        End If
                        parametros(3).Marcador = "Nro_Resolucion"
                        parametros(3).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                        parametros(4).Marcador = "fecha_actual"
                        parametros(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                        parametros(5).Marcador = "fecha_emision"
                        parametros(5).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        If vc_datos(0) = Nothing Then
                            SaveTable(lblExpediente.Text, selectcod)

                        End If
                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.OrdenEjecucionLiquidacionOficial, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If

                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS EJECUTIVOS DE LIQUIDACION OFICIAL")
                        Exit Sub
                    End If
                End If

                If selectcod = "352" Then

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                        Dim parametros(10) As WordReport.Marcadores_Adicionales
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                        Dim vc_datos() As String
                        parametros(2).Marcador = "letras"
                        parametros(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If

                        If resolucion(0) <> Nothing Then
                            parametros(3).Marcador = "fecha_anterior"
                            parametros(3).Valor = resolucion(1).ToUpper

                            parametros(4).Marcador = "resolucion_anterior"
                            parametros(4).Valor = resolucion(0).ToUpper
                        Else
                            Alert("NO REGISTRADA RESOLUCION DE (013) MANDAMIENTO DE PAGO PARA GENERAR ESTE DOCUMENTO")
                        End If



                        parametros(0).Marcador = "Nro_Resolucion"
                        parametros(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                        parametros(1).Marcador = "fecha_actual"
                        parametros(1).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                        If Valores(2) <> "" Then
                            parametros(5).Marcador = "acta_numero"
                            parametros(5).Valor = Valores(2).ToUpper

                            parametros(6).Marcador = "tipo_notificacion"
                            parametros(6).Valor = Valores(1).ToUpper

                            parametros(7).Marcador = "acta_dia"
                            parametros(7).Valor = Valores(0).ToUpper
                        Else
                            parametros(5).Marcador = "acta_numero"
                            parametros(5).Valor = ""

                            parametros(6).Marcador = "tipo_notificacion"
                            parametros(6).Valor = ""

                            parametros(7).Marcador = "acta_dia"
                            parametros(7).Valor = ""
                        End If
                        parametros(8).Marcador = "Nsalario"
                        parametros(8).Valor = Num2Text(Round((DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0))
                        parametros(9).Marcador = "nsalario_minimo"
                        parametros(9).Valor = Round((DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                        parametros(10).Marcador = "fecha_emision"
                        parametros(10).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.OrdenEjecucionMulta, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)

                        End If
                    Else
                        Alert("ESTE DOCUMENTO SOLO APLICA PARA TITULOS JUDICIALES DE MULTA ")
                        Exit Sub
                    End If

                End If

                If selectcod = "353" Then
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim datos_individual(4) As WordReport.Marcadores_Adicionales
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    datos_individual(2).Marcador = "salario"
                    datos_individual(2).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = "XXXXXXXXXXXXXXXXXXX"
                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConPagoEnFosygaYPila, datos_individual)




                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If


                If selectcod = "354" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    Dim datos_individual(4) As WordReport.Marcadores_Adicionales

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    datos_individual(2).Marcador = "fecha_actual"
                    datos_individual(2).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(3).Marcador = "fecha1"
                    datos_individual(3).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConPagoEnFosyga, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "355" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim datos_individual(4) As WordReport.Marcadores_Adicionales
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    datos_individual(2).Marcador = "fecha1"
                    datos_individual(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConRecursoModificatorio, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "356" Then

                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim datos_individual(7) As WordReport.Marcadores_Adicionales
                    '---- DATOS DE CUADRO_1
                    Dim tbl_deudores As New DataTable
                    Dim sql_deuda As SqlCommand
                    sql_deuda = New SqlCommand(" SELECT 0 AS CUENTA, " & _
                                " ED_NOMBRE,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP =@EXPEDIENTE AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)

                    sql_deuda.CommandType = CommandType.Text
                    sql_deuda.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)

                    Dim sql_deudores As New SqlDataAdapter(sql_deuda)
                    sql_deudores.Fill(tbl_deudores)
                    If tbl_deudores.Rows.Count > 0 Then
                        For I = 0 To tbl_deudores.Rows.Count - 1
                            tbl_deudores.Rows(I).Item(0) = I + 1

                        Next
                    Else
                        Alert("NO HAY DEUDORES SOLIDARIOS PARA GENERAR EL INFORME")
                        Exit Sub
                    End If
                    '--- DATOS DE CUADRO_2
                    Dim tbl_deudores2 As New DataTable
                    Dim sql_deuda2 As SqlCommand
                    sql_deuda2 = New SqlCommand(" SELECT 0 AS CUENTA, " & _
                                " ED_NOMBRE,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION , '' AS VALORES,'' AS LETRAS  FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP =@EXPEDIENTE AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)


                    sql_deuda2.CommandType = CommandType.Text
                    sql_deuda2.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                    Dim sumavalores As Double
                    Dim sql_deudores2 As New SqlDataAdapter(sql_deuda2)
                    sql_deudores2.Fill(tbl_deudores2)
                    If tbl_deudores2.Rows.Count > 0 Then
                        For I = 0 To tbl_deudores2.Rows.Count - 1
                            tbl_deudores2.Rows(I).Item("CUENTA") = I + 1
                            tbl_deudores2.Rows(I).Item("VALORES") = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (tbl_deudores2.Rows(I).Item("PARTICIPACION") / 100)))
                            tbl_deudores2.Rows(I).Item("LETRAS") = Num2Text(tbl_deudores2.Rows(I).Item("VALORES"))
                            sumavalores = sumavalores + CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (tbl_deudores2.Rows(0).Item("PARTICIPACION") / 100))

                        Next
                    Else
                        Alert("NO HAY DEUDORES SOLIDARIOS PARA GENERAR EL INFORME")
                        Exit Sub
                    End If

                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    datos_individual(2).Marcador = "fecha1"
                    datos_individual(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(5).Marcador = "fecha_ejecutoria"
                    datos_individual(5).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fecha_ejecutoria).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    SaveTable(lblExpediente.Text.Trim, homologo(selectcod))

                    datos_individual(6).Marcador = "total_socios"
                    datos_individual(6).Valor = String.Format("{0:C0}", sumavalores)
                    datos_individual(7).Marcador = "Socios"
                    datos_individual(7).Valor = Num2Text(sumavalores)

                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMPConSocios, datos_individual, tbl_deudores, 0, False, tbl_deudores2, 1, True, Nothing, 0, False)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "358" Then
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    Dim datos_individual(6) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim vc_datos() As String
                    Dim embargo As String = resolucion(2)
                    Dim resolucion2() As String = getResolucion_anterior(lblExpediente.Text, embargo)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    vc_datos = overloadresolucion(lblExpediente.Text, "229")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    vc_datos = overloadresolucion(lblExpediente.Text, "319")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion2(0) = vc_datos(0)
                        resolucion2(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then

                        datos_individual(0).Marcador = "nro_resolucion"
                        datos_individual(0).Valor = resolucion(0)
                        datos_individual(1).Marcador = "fecha_resolucion"
                        datos_individual(1).Valor = resolucion(1)
                        datos_individual(2).Marcador = "nro_embargo"
                        datos_individual(2).Valor = resolucion2(0)
                        datos_individual(3).Marcador = "fecha_embargo"
                        datos_individual(3).Valor = resolucion2(1)

                    Else
                        Alert("NO HAY RESOLUCION DE LEVANTAMIENTO DE MEDIDAS CAUTELARES PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    datos_individual(4).Marcador = "letras"
                    datos_individual(4).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargos / 100))
                    datos_individual(5).Marcador = "Total_Embargo"
                    datos_individual(5).Valor = String.Format("{0:C2}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargos / 100)))
                    datos_individual(6).Marcador = "embargabilidad"
                    datos_individual(6).Valor = embargos

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoPlanillaLevantamientoEmbargo, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "370" Then
                    Dim datos_individual(8) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim Excepciones() As String = GetExcepcion(lblExpediente.Text.Trim)
                    Dim vc_datos(1) As String
                    If Excepciones(0) = "" Then
                        Alert("NO HAY RADICADO DE PARA GENERAR INFORME DE EXCEPCIONES ")
                        Exit Sub
                    End If

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)

                    datos_individual(0).Marcador = "nro_resolucion"
                    datos_individual(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    vc_datos = overloadresolucion(lblExpediente.Text.Trim, "013")

                    If vc_datos(0) <> "" Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If


                    If resolucion(0) = "" Then
                        Alert("NO HAY REGISTRA MANDAMIENTO DE PAGO ")
                    Else

                    End If
                    datos_individual(2).Marcador = "fecha_anterior"
                    datos_individual(2).Valor = resolucion(1)

                    datos_individual(3).Marcador = "resolucion_anterior"
                    datos_individual(3).Valor = resolucion(0)
                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    If Valores(2) <> Nothing Then

                        datos_individual(5).Marcador = "acta_numero"
                        datos_individual(5).Valor = Valores(2)

                        datos_individual(6).Marcador = "tipo_notificacion"
                        datos_individual(6).Valor = Valores(1)

                        datos_individual(7).Marcador = "acta_dia"
                        datos_individual(7).Valor = Valores(0).ToUpper

                    Else
                        datos_individual(5).Marcador = "acta_numero"
                        datos_individual(5).Valor = "0"

                        datos_individual(6).Marcador = "tipo_notificacion"
                        datos_individual(6).Valor = "0"


                        datos_individual(7).Marcador = "acta_dia"
                        datos_individual(7).Valor = "0"

                    End If

                    datos_individual(8).Marcador = "fecha_emision"
                    datos_individual(8).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    SaveTable(lblExpediente.Text, selectcod)






                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ResuelveySuspendeproceso, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                '*********** FIN REPORTES DE COBRO COACTIVO ********************




                'Se devuelve el campo en vaciosi no posee el representante legal'***** solicitado Por Rafael 
                If selectcod = "217" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)


                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        datos(2).Marcador = "fecha1"
                        datos(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If
                    Else
                        Alert("NO SE ENCUENTRA TITULO EJECUTIVO  Y/O FECHA DEL TITULO ")
                        Exit Sub
                    End If

                    ''For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                    'DtsDatos.DATOS_REPORTES.Item(i).ED_TipoPersona = getTitulo(DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId, DtsDatos.DATOS_REPORTES.Item(i).Tipo_Deudor)
                    'DtsDatos.DATOS_REPORTES.Item(i).totaldeuda = Format(DtsDatos.DATOS_REPORTES.Item(i).totaldeuda, "#,##0.00")
                    'Next

                    datos(0).Marcador = "ED_Rep"
                    If representante(0) <> Nothing Then
                        datos(0).Valor = representante(0)
                    Else
                        datos(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(1).Marcador = "Letras"
                    datos(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioDeCobroPersuasivoomisos, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe

                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        ''SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "218" Then
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo


                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else

                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioDeCobroPersuasivoomisos, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If

                If selectcod = "219" Then
                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        If DtsDatos.DATOS_REPORTES.Count > 0 Then
                            fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                            If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                                DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                                DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                            End If

                        Else
                            Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                            Exit Sub
                        End If


                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "ED_Rep"

                        If representante(0) <> Nothing Then
                            datos(1).Valor = representante(0)
                        Else
                            datos(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                            DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                        End If
                        datos(2).Marcador = "Letras"
                        datos(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        datos(3).Marcador = "fecha2"
                        If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                            fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                            datos(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        Else
                            datos(3).Valor = "NO REQUERIDO"
                        End If

                    Else
                        Alert("EL DOCUMENTO SOLO PUEDE SER GENERADO PARA MULTAS ")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioDeCobroPersuasivoFosiga, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If



                If selectcod = "220" Then
                    Dim datos(4) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then
                        If DtsDatos.DATOS_REPORTES.Count > 0 Then

                            fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                            'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                            If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                                DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                                DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                            End If

                        Else
                            Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                            Exit Sub
                        End If

                        datos(0).Marcador = "fecha1"
                        datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos(1).Marcador = "fecha2"
                        If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                            fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                            datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        Else
                            datos(1).Valor = "NO REQUERIDO "
                        End If


                        datos(2).Marcador = "ED_Rep"

                        If representante(0) <> Nothing Then
                            datos(2).Valor = representante(0)
                        Else
                            datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                            DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                        End If
                        datos(3).Marcador = "Letras"
                        datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                        datos(4).Marcador = "Solicitados"

                        If DtsDatos.DATOS_REPORTES.Item(0).Documento = "00" Then

                            datos(4).Valor = ""
                        ElseIf DtsDatos.DATOS_REPORTES.Item(0).Documento = "01" Then

                            datos(4).Valor = "CERTIFICACIÓN DE INGRESOS DEBIDAMENTE EMITIDO POR PARTE DE UN CONTADOR TITULADO"
                        ElseIf DtsDatos.DATOS_REPORTES.Item(0).Documento = "02" Then

                            datos(4).Valor = "CERTIFICADO DE DISPONIBILIDAD PRESUPUESTAL"
                        ElseIf DtsDatos.DATOS_REPORTES.Item(0).Documento = "03" Then

                            datos(4).Valor = "LOS ESTADOS FINANCIEROS DEL ÚLTIMO AÑO GRAVABLE DE LA SOCIEDAD"


                        Else
                            Alert("ESTE DOCUMENTO SOLO SE PUEDE GENERAR PARA MULTAS ")
                            Exit Sub
                        End If
                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioDeCobroPersuasivoFosiga, datos)
                    End If



                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "221" Then
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo


                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO")
                        Exit Sub
                    End If

                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(1).Valor = representante(0)
                    Else
                        datos(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(2).Marcador = "Letras"
                    datos(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos(3).Marcador = "informante"
                    datos(3).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimeOficioPersuasivoMultaDirectoFosiga, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "223" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub

                    End If
                    Dim datos(4) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If



                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos(4).Marcador = "informante"
                    datos(4).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SolicitudDocumentosPago, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "224" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.VerificacionPagoAprobado, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "225" Then
                    Dim datos_individual(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    datos_individual(0).Marcador = "Fecha_Actual"
                    datos_individual(0).Valor = Today.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_individual(1).Marcador = "Ejecutor"
                    datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                    datos_individual(2).Marcador = "Cargo"
                    datos_individual(2).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                    datos_individual(3).Marcador = "Nro"
                    datos_individual(3).Valor = DtsDatos.DATOS_REPORTES.Rows.Count

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CambioEstadoExpedienteIndividual, datos_individual)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "226" Then

                    Dim datos_valor(12) As WordReport.Marcadores_Adicionales
                    Dim oficio() As String = GETPERSUASIVO(lblExpediente.Text.Trim)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo


                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo > Nothing Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO "
                    End If

                    datos_valor(2).Marcador = "letras"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "fecha_actual"
                    datos_valor(3).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_valor(4).Marcador = "salario"

                    datos_valor(4).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    datos_valor(5).Marcador = "Nro_Resolucion"
                    datos_valor(5).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_valor(6).Marcador = "fecha3"
                    datos_valor(6).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_valor(7).Marcador = "estados"

                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "01" Then
                        datos_valor(7).Valor = "Determino"
                    Else
                        datos_valor(7).Valor = "Impuso"
                    End If


                    datos_valor(8).Marcador = "oficio1"
                    datos_valor(8).Valor = oficio(0)

                    datos_valor(9).Marcador = "fechaof1"
                    datos_valor(9).Valor = oficio(1).ToUpper

                    datos_valor(10).Marcador = "oficio2"
                    If oficio(2) = "" Then
                        datos_valor(10).Valor = ""
                    Else

                        datos_valor(10).Valor = oficio(2)
                    End If


                    datos_valor(11).Marcador = "fechaof2"
                    If oficio(3) = "del 01 de enero de 1900" Then
                        datos_valor(11).Valor = "NO ENVIADO "
                    Else
                        datos_valor(11).Valor = oficio(3)

                    End If

                    datos_valor(12).Marcador = "fecha_emision"
                    datos_valor(12).Valor = "XXXXXXXXXXXXXXXXXXXXXX"


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AutoterminacionyArchivo, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "244" Then
                    Dim parametro(1) As WordReport.Marcadores_Adicionales
                    If DtsDatos.DATOS_REPORTES.Rows.Count > 0 Then
                        parametro(0).Marcador = "Deuda_Letras"
                        parametro(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        parametro(1).Marcador = "Deuda_Valor"
                        parametro(1).Valor = FormatCurrency(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    Else
                        Alert("NO HAY DATOS ASOCIADOS A ESTE PROCESO ")
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PresentacionDelCreditoCuotasPartes, parametro)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "." & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "245" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo = Nothing Then
                        Alert("no hay fecha de expedicion de titulo")
                        Exit Sub
                    Else
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    End If



                    Dim parametro(2) As WordReport.Marcadores_Adicionales
                    parametro(0).Marcador = "Letras"
                    parametro(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    parametro(1).Marcador = "Deuda_Valor"
                    parametro(1).Valor = FormatCurrency(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    parametro(2).Marcador = "fecha1"
                    parametro(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PresentacionDelCreditoParafiscal, parametro)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                    End If
                End If


                If selectcod = "301" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(1).Valor = representante(0)
                    Else
                        datos(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    datos(2).Marcador = "Letras"
                    datos(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos(3).Marcador = "fecha2"
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(3).Valor = "No requerida"
                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioDeCobroPersuasivo, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If
                If selectcod = "302" Then

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then


                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    Dim datos(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    datos(3).Marcador = "Letras"
                    datos(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioDeCobroPersuasivo, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "303" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioCondenaJudicial, datos_valor)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "304" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioCondenaJudicial, datos_valor)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "305" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        '    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioMulta1607, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "306" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then


                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"

                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioMulta1607, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "307" Then
                    Dim datos_valor(4) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO ")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    datos_valor(1).Marcador = "valorL"
                    datos_valor(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(2).Marcador = "ED_Rep"


                    If representante(0) <> Nothing Then
                        datos_valor(2).Valor = representante(0)
                    Else
                        datos_valor(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    datos_valor(3).Marcador = "fecha2"
                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(3).Valor = "NO REQUERIDO"
                    End If
                    datos_valor(4).Marcador = "informante"
                    datos_valor(4).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioMulta1438, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "308" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        '' fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioMulta1438, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "310" Then
                    Dim datos_valor(5) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo

                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If


                    datos_valor(2).Marcador = "valorL"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    If resolucion(0) <> Nothing Then
                        datos_valor(4).Marcador = "nro_resolucion"
                        datos_valor(4).Valor = resolucion(0)
                        datos_valor(5).Marcador = "resolución_fecha"
                        datos_valor(5).Valor = resolucion(1)
                    Else
                        Alert("NO SE ENCUENTRA DOCUMENTO DE RESOLUCION DE TERMINACION ")
                        Exit Sub
                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ComunicacionTerminacionyArchivo, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "311" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos_valor(1).Valor = "NO REQUERIDO"
                    End If

                    datos_valor(2).Marcador = "letras"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.RespuestaSolicitudPazSalvo, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "312" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If

                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.TrasladoPorCompetenciasDireccionParafiscales, datos)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "313" Then
                    Dim datos(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        ''fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        If DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA" Then
                            DtsDatos.DATOS_REPORTES.Item(0).Municipio = "BOGOTA D.C"
                            DtsDatos.DATOS_REPORTES.Item(0).Departamento = ""
                        End If

                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    datos(0).Marcador = "fecha1"
                    datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos(1).Marcador = "fecha2"

                    If DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo <> "1900-01-01" Then
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                        datos(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    Else
                        datos(1).Valor = "NO REQUERIDO"
                    End If
                    datos(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos(2).Valor = representante(0)
                    Else
                        datos(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.ContestacionRevocatoriaDirecta, datos)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If



                If selectcod = "318" Then

                    Dim datosrlegal(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, homologo("013"))
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If


                    datosrlegal(0).Marcador = "rlegal"

                    datosrlegal(1).Marcador = "ED_NIT"

                    If representante(0) <> Nothing Then
                        datosrlegal(0).Valor = representante(0)
                        datosrlegal(1).Valor = representante(1)
                    Else
                        datosrlegal(0).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        datosrlegal(1).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit = ""
                    End If
                    Dim tb1 As New DataTable
                    Dim sql_vinculo1 As SqlDataAdapter = New SqlDataAdapter(" SELECT ED_NOMBRE ," & _
                                 " ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT " & _
                                 " FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                 " WHERE  NROEXP ='" & lblExpediente.Text.Trim & "' AND TIPO ='2' " & _
                                 " AND   DEUDOR = ED_CODIGO_NIT ", Ado)
                    sql_vinculo1.Fill(tb1)
                    'For i = 0 To DtsDatos.DATOS_REPORTES.Rows.Count - 1
                    '    DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit = DtsDatos.DATOS_REPORTES.Item(i).ED_TipoId & DtsDatos.DATOS_REPORTES.Item(i).ED_Codigo_Nit
                    'Next
                    If tb1.Rows.Count = 0 Then
                        Alert("NO SE ENCONTRARON REGISTROS DE DEUDORES SOLIDARIOS ")
                        Exit Sub
                    End If


                    If resolucion(0) <> Nothing Then
                        datosrlegal(2).Marcador = "nro_resolucion"
                        datosrlegal(2).Valor = resolucion(0)
                        datosrlegal(3).Marcador = "fecha_reg"
                        datosrlegal(3).Valor = resolucion(1)
                    Else
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA IMPRIMIR EL DOCUMENTO PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub
                    End If

                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.CitacionMandamientoPagoSocios, datosrlegal, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "329" Then
                    Dim datos_valor(2) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        'fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If
                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    datos_valor(1).Marcador = "letras"
                    datos_valor(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(2).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(2).Valor = representante(0)
                    Else
                        datos_valor(2).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""

                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.PrimerOficioCuotasPartes, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                    End If
                End If
                If selectcod = "330" Then
                    Dim datos_valor(3) As WordReport.Marcadores_Adicionales
                    Dim representante() As String = cargar_representante(lblExpediente.Text)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then

                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo
                    Else
                        Alert("NO HAY TITULO EJECUTIVO Y/O  NOTIFICACION GENERADA")
                        Exit Sub
                    End If

                    datos_valor(0).Marcador = "fecha1"
                    datos_valor(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(1).Marcador = "fecha2"
                    datos_valor(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                    datos_valor(2).Marcador = "letras"
                    datos_valor(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_valor(3).Marcador = "ED_Rep"

                    If representante(0) <> Nothing Then
                        datos_valor(3).Valor = representante(0)
                    Else
                        datos_valor(3).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre = ""
                    End If
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.SegundoOficioCuotasPartes, datos_valor)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If
                If selectcod = "222" Then
                    Dim parametros(2) As WordReport.Marcadores_Adicionales
                    parametros(0).Marcador = "tasa1"
                    parametros(0).Valor = getPorcentaje(1)
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                        parametros(1).Marcador = "fecha1"
                        parametros(1).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")

                    End If
                    parametros(2).Marcador = "Informantes"
                    parametros(2).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.TrasladoMinsterio, parametros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else

                        Alert("NO HAY DATOS PARA GENERA ESTE REPORTE DE TRASLADO ")
                        Exit Sub
                    End If

                    'worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.TrasladoMinsterio)

                End If



                If selectcod = "332" Then

                    Dim tb As New DataTable
                    Dim Adap As New SqlDataAdapter("SELECT SUM(AJUSTE) AS DEUDA, SUM(MONTO_PAGO) AS PAGOS FROM SQL_PLANILLA WHERE EXPEDIENTE = @EXPEDIENTE", Funciones.CadenaConexion)
                    Adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                    Adap.Fill(tb)

                    If tb.Rows.Count > 0 Then

                        If tb.Rows(0).Item("PAGOS") >= tb.Rows(0).Item("DEUDA") Then
                            Dim paramentros(9) As WordReport.Marcadores_Adicionales
                            Dim fecha3 As Date
                            Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "331", 0)

                            If dtsPlantilla.Rows.Count > 0 Then

                                'ENVIAR NUMERO DE CUOTAS 
                                Dim tb1 As New DataTable
                                Dim ad As New SqlDataAdapter("SELECT A.CUOTA_NUMERO,convert(varchar(10),A.FECHA_CUOTA,103) AS FECHA_CUOTA , CONVERT (VARCHAR(50),CONVERT (MONEY,A.SALDO_CAPITAL),1) AS SALDO_CAPITAL, A.PERIODO,CONVERT (VARCHAR(50),CONVERT (MONEY,A.VALOR_CUOTA),1)AS APORTE_CAPITAL,A.SALDO_CAPITAL AS SALDO,A.VALOR_CUOTA AS CUOTA FROM DETALLES_ACUERDO_PAGO A , MAESTRO_ACUERDOS B WHERE A.DOCUMENTO = B.DOCUMENTO AND B.EXPEDIENTE = @EXPEDIENTE", Funciones.CadenaConexion)
                                ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                                ad.Fill(tb1)

                                Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)

                                paramentros(0).Marcador = "fecha_actual"
                                paramentros(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                                paramentros(1).Marcador = "letra"
                                paramentros(1).Valor = Num2Text(tb1.Rows.Item(0)("CUOTA")).ToUpper
                                paramentros(2).Marcador = "fecha_titulo"
                                fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("FECHA_SIS"))
                                paramentros(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                paramentros(3).Marcador = "fecha_anterior"
                                fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("FECHA_ANT"))
                                paramentros(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                                paramentros(4).Marcador = "Lcuotas"
                                paramentros(4).Valor = Num2Text(tb1.Rows.Count).ToUpper
                                paramentros(5).Marcador = "final_cuotas"
                                paramentros(5).Valor = tb1.Rows.Count

                                paramentros(6).Marcador = "Nro_Resolucion"
                                paramentros(6).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                                paramentros(7).Marcador = "finalpago"
                                fecha3 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("INICIO_ACUERDO"))
                                paramentros(7).Valor = fecha3.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                paramentros(8).Marcador = "valor_cuota"
                                paramentros(8).Valor = String.Format("{0:C2}", CDbl(tb1.Rows.Item(0)("CUOTA")))
                                paramentros(9).Marcador = "fecha_emision"
                                paramentros(9).Valor = "XXXXXXXXXXXXXXXXXXXXXX"


                                worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadesPagoDeclaraCumplidaParafiscales, paramentros, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)

                                If worddocresult = "" Then
                                    ''mensaje no informe
                                Else
                                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                                    SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                                End If
                            Else
                                Alert("No se ha concedido una facilidad de pago para este proceso...")
                                Exit Sub
                            End If

                        Else
                            Alert("No se a podido generar la RESOLUCIÓN DECLARA CUMPLIDA FACILIDAD PARAFISCALES, saldo pendiente por pagar.")
                            Exit Sub
                        End If


                    Else
                        Alert("Noy hay SQL asociado al expedidiente " & lblExpediente.Text.Trim & " ...")
                        Exit Sub
                    End If
                End If


                If selectcod = "333" Then
                    If DtsDatos.DATOS_REPORTES.Count > 0 Then
                        fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    Else
                        Alert(" NO HAY DATOS PARA ESTA CONSULTA ")
                        Exit Sub
                    End If
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    Dim dtsPlantilla As DataTable = Funciones.GetDatosGenerales(lblExpediente.Text.Trim)
                    If dtsPlantilla.Rows.Count > 0 Then
                        Dim datos_individual(3) As WordReport.Marcadores_Adicionales

                        datos_individual(0).Marcador = "Ejecutor"
                        datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                        datos_individual(1).Marcador = "Cargo"
                        datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec

                        datos_individual(2).Marcador = "MT_fec_expedicion_titulo"
                        datos_individual(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                        datos_individual(3).Marcador = "Letras"
                        datos_individual(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper


                        worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.FacilidadesPagoRespuestaSolicitudParafiscales, datos_individual)
                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    End If
                End If


                If selectcod = "334" Then
                    Dim dtsPlantilla As DataTable = GetDatosGenerales(lblExpediente.Text)

                    If dtsPlantilla.Rows.Count > 0 Then
                        Dim datos_individual(2) As WordReport.Marcadores_Adicionales

                        datos_individual(0).Marcador = "Ejecutor"
                        datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                        datos_individual(1).Marcador = "Cargo"
                        datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                        datos_individual(2).Marcador = "fecha_corte"
                        datos_individual(2).Valor = Now().ToString("'al' dd 'de' MMMM 'de' yyy").ToUpper



                        Dim tb1 As New DataTable

                        Dim ad As New SqlDataAdapter("SELECT SUBSISTEMA, '' AS AJUSTE,  CONVERT(varchar(10), ANNO) + ' - '+  B.NOMBRE AS PERIODOS, '' AS INTERESES_MORATORIOS, '' AS SALDO_TOTAL, SUM( AJUSTE) AS CAPITAL ,A.MES,A.ANNO FROM  SQL_PLANILLA A , MAESTRO_MES B WHERE A.MES = B.ID_MES AND A.EXPEDIENTE = @EXPEDIENTE GROUP BY SUBSISTEMA, ANNO, B.NOMBRE,B.ID_MES,A.MES HAVING (SUM( AJUSTE)>0) ORDER BY SUBSISTEMA,ANNO,B.ID_MES", Funciones.CadenaConexion)
                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                        ad.Fill(tb1)


                        'Obterner dia habiles de pago
                        Dim tb2 As New DataTable

                        ad = New SqlDataAdapter("SELECT A.DEUDOR,D.CODIGO FROM DEUDORES_EXPEDIENTES A , TIPOS_ENTES B, ENTES_DEUDORES C,TIPOS_APORTANTES D WHERE A.DEUDOR = C.ED_CODIGO_NIT AND A.TIPO = B.CODIGO AND C.ED_TIPOAPORTANTE = D.CODIGO AND B.CODIGO = '1' AND A.NROEXP = @EXPEDIENTE ", Funciones.CadenaConexion)
                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                        ad.Fill(tb2)


                        Dim diaPago As String = FuncionesInteresesParafiscales.ObtenerDiaPago(tb2.Rows(0).Item("DEUDOR").ToString, CInt(tb2.Rows(0).Item("CODIGO")))

                        For Each row As DataRow In tb1.Rows
                            Dim deudaCapital As Double = CDbl(row("CAPITAL"))
                            Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, row("ANNO"), row("MES"), CInt(tb2.Rows(0).Item("CODIGO")), row("SUBSISTEMA")).ToString("dd/MM/yyyy")
                            'Alterar grid colocar datos faltantes
                            row("INTERESES_MORATORIOS") = FuncionesInteresesParafiscales._CalcularIntereses(CDbl(row("CAPITAL")), fechaExigibilidad.ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"), CDec(ViewState("diaria")))
                            row("SALDO_TOTAL") = CDbl(row("INTERESES_MORATORIOS")) + deudaCapital

                            row("INTERESES_MORATORIOS") = String.Format("{0:C2}", CDbl(row("INTERESES_MORATORIOS")))
                            row("SALDO_TOTAL") = String.Format("{0:C2}", CDbl(row("SALDO_TOTAL")))
                            row("AJUSTE") = String.Format("{0:C2}", CDbl(row("CAPITAL")))
                        Next


                        worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.OficioInformaNoConcedidaFacilidadInvitaPago, datos_individual, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)


                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If

                End If

                If selectcod = "335" Then
                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "331")
                    If dtsPlantilla.Rows.Count > 0 Then
                        Dim datos_individual(6) As WordReport.Marcadores_Adicionales

                        datos_individual(0).Marcador = "Ejecutor"
                        datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                        datos_individual(1).Marcador = "Cargo"
                        datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                        datos_individual(2).Marcador = "SCuota"
                        datos_individual(2).Valor = Math.Round(dtsPlantilla.Rows.Item(0)("SALDO_INICIAL") / dtsPlantilla.Rows.Item(0)("TOTAL_DEUDA") * 100)
                        datos_individual(3).Marcador = "Cuota"
                        datos_individual(3).Valor = dtsPlantilla.Rows.Item(0)("NRO_CUOTAS")
                        datos_individual(4).Marcador = "letra"
                        datos_individual(4).Valor = Num2Text(dtsPlantilla.Rows.Item(0)("SALDO_INICIAL")).ToUpper
                        datos_individual(5).Marcador = "fechai"
                        fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)("INICIO_ACUERDO"))
                        datos_individual(5).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        datos_individual(6).Marcador = "monto"
                        datos_individual(6).Valor = dtsPlantilla.Rows.Item(0)("SALDO_INICIAL_M")

                        worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.OficioSolicitaCumplimientoRequisitos, datos_individual)
                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                End If

                If selectcod = "336" Then
                    Dim paramentros(3) As WordReport.Marcadores_Adicionales

                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "331")
                    If dtsPlantilla.Rows.Count > 0 Then

                        'Obterner dia habiles de pago
                        Dim tb1 As New DataTable

                        Dim ad As New SqlDataAdapter(" SELECT A.SUBSISTEMA,CONVERT(varchar(10), A.ANNO) + ' - '+  C.NOMBRE AS PERIODOS , CONVERT(varchar (50),convert(money, SUM(A.AJUSTE)),1) AS DEUDA FROM 																						" & _
                                                    " 	(SELECT CONVERT(VARCHAR(12), EXPEDIENTE) + '' + SUBSISTEMA+ ''+ CONVERT(VARCHAR(50), NIT_EMPRESA)+ ''+ CONVERT(VARCHAR(4), ANNO)+ '' + CONVERT(VARCHAR(4),MES)+ ''+ CONVERT(VARCHAR(50),CEDULA) AS ID , * FROM DETALLE_FACILIDAD_PAGO ) A , " & _
                                                    " 	(SELECT CONVERT(VARCHAR(12), EXPEDIENTE) + '' + SUBSISTEMA+ ''+ CONVERT(VARCHAR(50), NIT_EMPRESA)+ ''+ CONVERT(VARCHAR(4), ANNO)+ '' + CONVERT(VARCHAR(4),MES)+ ''+ CONVERT(VARCHAR(50),CEDULA) AS ID, * FROM SQL_PLANILLA )B,              " & _
                                                    " 	 MAESTRO_MES C                                                                                                                                                                                                                              " & _
                                                    " WHERE A.ID= B.ID                                                                                                                                                                                                                              " & _
                                                    " AND A.MES=C.ID_MES AND B.MES=C.ID_MES  AND (A.AJUSTE > B.MONTO_PAGO)  AND A.EXPEDIENTE = @EXPEDIENTE                                                                                                                                              " & _
                                                    " GROUP BY A.SUBSISTEMA, A.ANNO,C.NOMBRE , C.ID_MES ORDER BY A.SUBSISTEMA,A.ANNO,C.ID_MES                                                                                                                                                       ", Funciones.CadenaConexion)
                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                        ad.Fill(tb1)

                        If tb1.Rows.Count = 0 Then
                            Alert("No hay sql asociado a este expediente..")
                            Exit Sub
                        End If
                        Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)

                        paramentros(0).Marcador = "fecha_actual"
                        paramentros(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        paramentros(1).Marcador = "fecha_anterior"
                        fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(6))
                        paramentros(1).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(2).Marcador = "Nro_Resolucion"
                        paramentros(2).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        paramentros(3).Marcador = "fecha_emision"
                        paramentros(3).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.ResolucionDeclaraIncumplidaParafiscales, paramentros, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd-MMyyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                End If

                If selectcod = "337" Then
                    Dim paramentros(10) As WordReport.Marcadores_Adicionales
                    ''Dim fecha3 As Date
                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, selectcod, 1)
                    If dtsPlantilla.Rows.Count > 0 Then
                        paramentros(0).Marcador = "fecha_actual"
                        paramentros(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        paramentros(1).Marcador = "letra"
                        paramentros(1).Valor = Num2Text(dtsPlantilla.Rows.Item(0)(9))
                        paramentros(2).Marcador = "fecha_titulo"
                        fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(1))
                        paramentros(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(3).Marcador = "fecha_anterior"
                        fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(6))
                        paramentros(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(4).Marcador = "Lcuotas"
                        paramentros(4).Valor = Num2Text(dtsPlantilla.Rows.Count)
                        paramentros(5).Marcador = "Nro_Resolucion"
                        paramentros(5).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        paramentros(6).Marcador = "nrocuota"
                        paramentros(6).Valor = dtsPlantilla.Rows.Count
                        paramentros(7).Marcador = "Tanual"
                        paramentros(7).Valor = getPorcentaje(0)
                        paramentros(8).Marcador = "Tmensual"
                        paramentros(8).Valor = getPorcentaje(1)
                        paramentros(9).Marcador = "SCuota"
                        paramentros(9).Valor = Math.Round(dtsPlantilla.Rows.Item(0)(10) / dtsPlantilla.Rows.Item(0)(9) * 100)
                        paramentros(10).Marcador = "fecha_emision"
                        paramentros(10).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        'For i = 0 To dtsPlantilla.Rows.Count - 1

                        'dtsPlantilla.Rows.Item(i)("INTERESES_TOTALES") = dtsPlantilla.Rows.Item(i)(10) * ((getPorcentaje(1) + 1) ^ dtsPlantilla.Rows.Count)
                        'dtsPlantilla.Rows.Item(i)("TOTAL_CON") = dtsPlantilla.Rows.Item(i)("INTERESES_TOTALES") + dtsPlantilla.Rows.Item(i)(9)

                        'Next

                        worddocresult = worddoc.CreateReportWithTable(dtsPlantilla, Reportes.FacilidadPagoResolucionConcedeMulta, "CUOTA_NUMERO,FECHA_CUOTA,SALDO_CAPITAL,APORTE_CAPITAL,INTERESES_TOTALES,TOTAL_CON", paramentros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                End If


                If selectcod = "338" Then
                    Dim paramentros(8) As WordReport.Marcadores_Adicionales
                    Dim fecha3 As Date
                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, selectcod)
                    If dtsPlantilla.Rows.Count > 0 Then
                        paramentros(0).Marcador = "fecha_actual"
                        paramentros(0).Valor = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXx"
                        paramentros(1).Marcador = "letra"
                        paramentros(1).Valor = Num2Text(dtsPlantilla.Rows.Item(0)(15))
                        paramentros(2).Marcador = "fecha_titulo"
                        fecha = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(1))
                        paramentros(2).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(3).Marcador = "fecha_anterior"
                        fecha2 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(6))
                        paramentros(3).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(4).Marcador = "Lcuotas"
                        paramentros(4).Valor = Num2Text(dtsPlantilla.Rows.Count)
                        paramentros(5).Marcador = "final_cuotas"
                        paramentros(5).Valor = dtsPlantilla.Rows.Count
                        paramentros(6).Marcador = "Nro_Resolucion"
                        paramentros(6).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                        paramentros(7).Marcador = "finalpago"
                        fecha3 = Convert.ToDateTime(dtsPlantilla.Rows.Item(0)(18))
                        paramentros(7).Valor = fecha3.ToString("'del' dd 'de' MMMM 'de' yyy")
                        paramentros(8).Marcador = "fecha_emision"
                        paramentros(8).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                        For I = 0 To dtsPlantilla.Rows.Count - 1

                            If dtsPlantilla.Rows.Item(I)("CONFIRMACION") = False Then
                                dtsPlantilla.Rows.Item(I)("CONFIRMACION") = "SIN PAGO"
                            Else
                                dtsPlantilla.Rows.Item(I)("CONFIRMACION") = "PAGO"
                            End If

                        Next
                        worddocresult = worddoc.CreateReportWithTable(dtsPlantilla, Reportes.FacilidadPagoDeclaraCumplidaMulta, "CUOTA_NUMERO,FECHA_CUOTA,FECHA_PAGO,SALDO_CAPITAL,PAGO_PERIODO,APORTE_CAPITAL,CONFIRMACION", paramentros)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                End If

                If selectcod = "339" Then
                    Dim dtsPlantilla As DataTable = Funciones.GetDatosGenerales(lblExpediente.Text.Trim)
                    If dtsPlantilla.Rows.Count > 0 Then

                        Dim tb1 As New DataTable

                        Dim ad As New SqlDataAdapter("SELECT SUBSISTEMA, '' AS AJUSTE,  CONVERT(varchar(10), ANNO) + ' - '+  B.NOMBRE AS PERIODOS, '' AS INTERESES_MORATORIOS, '' AS SALDO_TOTAL, SUM( AJUSTE) AS CAPITAL ,A.MES,A.ANNO FROM  SQL_PLANILLA A , MAESTRO_MES B WHERE A.MES = B.ID_MES AND A.EXPEDIENTE = @EXPEDIENTE GROUP BY SUBSISTEMA, ANNO, B.NOMBRE,B.ID_MES,A.MES ORDER BY SUBSISTEMA,ANNO,B.ID_MES", Funciones.CadenaConexion)
                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                        ad.Fill(tb1)

                        If tb1.Rows.Count = 0 Then
                            Alert("No hay sql asociado a este expediente..")
                            Exit Sub
                        End If

                        'Obterner dia habiles de pago
                        Dim tb2 As New DataTable

                        ad = New SqlDataAdapter("SELECT A.DEUDOR,D.CODIGO FROM DEUDORES_EXPEDIENTES A , TIPOS_ENTES B, ENTES_DEUDORES C,TIPOS_APORTANTES D WHERE A.DEUDOR = C.ED_CODIGO_NIT AND A.TIPO = B.CODIGO AND C.ED_TIPOAPORTANTE = D.CODIGO AND B.CODIGO = '1' AND A.NROEXP = @EXPEDIENTE ", Funciones.CadenaConexion)
                        ad.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
                        ad.Fill(tb2)


                        Dim datos_individual(2) As WordReport.Marcadores_Adicionales

                        datos_individual(0).Marcador = "Ejecutor"
                        datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                        datos_individual(1).Marcador = "Cargo"
                        datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec
                        datos_individual(2).Marcador = "fecha_corte"
                        datos_individual(2).Valor = Now().ToString("'al' dd 'de' MMMM 'de' yyy").ToUpper



                        Dim diaPago As String = FuncionesInteresesParafiscales.ObtenerDiaPago(tb2.Rows(0).Item("DEUDOR").ToString, CInt(tb2.Rows(0).Item("CODIGO")))

                        For Each row As DataRow In tb1.Rows
                            Dim deudaCapital As Double = CDbl(row("CAPITAL"))
                            Dim fechaExigibilidad As Date = FuncionesInteresesParafiscales.FechasHabiles(diaPago, row("ANNO"), row("MES"), CInt(tb2.Rows(0).Item("CODIGO")), row("SUBSISTEMA")).ToString("dd/MM/yyyy")
                            'Alterar grid colocar datos faltantes
                            row("INTERESES_MORATORIOS") = FuncionesInteresesParafiscales._CalcularIntereses(CDbl(row("CAPITAL")), fechaExigibilidad.ToString("dd/MM/yyyy"), Now().ToString("dd/MM/yyyy"), CDec(ViewState("diaria")))
                            row("SALDO_TOTAL") = CDbl(row("INTERESES_MORATORIOS")) + deudaCapital

                            row("INTERESES_MORATORIOS") = String.Format("{0:C2}", CDbl(row("INTERESES_MORATORIOS")))
                            row("SALDO_TOTAL") = String.Format("{0:C2}", CDbl(row("SALDO_TOTAL")))
                            row("AJUSTE") = String.Format("{0:C2}", CDbl(row("CAPITAL")))
                        Next

                        worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadOficioInformaNoconcedidaFacilidadMulta, datos_individual, tb1, 0, False, Nothing, 0, False, Nothing, 0, False)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                End If


                If selectcod = "340" Then

                    Dim dtsPlantilla As DataTable = GetDatoPlantillaWord(lblExpediente.Text, "337", 0)

                    If dtsPlantilla.Rows.Count > 0 Then
                        Dim datos_individual(3) As WordReport.Marcadores_Adicionales
                        datos_individual(0).Marcador = "Ejecutor"
                        datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                        datos_individual(1).Marcador = "Cargo"
                        datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec


                        Dim tb1 As DataTable = Funciones.RetornaCargadatos("SELECT A.CUOTA_NUMERO, '' AS VALOR_CUOTA1, convert(varchar(10),A.FECHA_CUOTA,103) AS FECHA_CUOTA, B.p_anual AS TASA_MORA , convert(varchar(10),A.FECHA_PAGO,103) AS FECHA_PAGO,  DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO)AS DIAS_MORA,'' AS INTERES1,''AS TOTAL_A_PAGAR1,'' AS TOTAL_PAGADO1, '' AS SALDO_PENDIENTE1, VALOR_CUOTA * ( B.p_anual/ 365) * DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO) AS INTERES,0 AS TOTAL_A_PAGAR,0 AS TOTAL_PAGADO, 0 AS SALDO_PENDIENTE, A.VALOR_CUOTA   FROM DETALLES_ACUERDO_PAGO A, PORCENTAJE_TASA_MULTA B ,MAESTRO_ACUERDOS C WHERE A.DOCUMENTO = C.DOCUMENTO AND C.EXPEDIENTE = '" & lblExpediente.Text.Trim() & "' AND DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO) >0 AND CUOTA_FIN = 0 ORDER BY A.CUOTA_FIN")
                        Dim tb2 As DataTable = Funciones.RetornaCargadatos("SELECT A.CUOTA_NUMERO, '' AS VALOR_CUOTA1, '' AS VALOR_INTERES_PAGAR1,'' AS TOTAL_A_PAGAR1, convert(varchar(10),A.FECHA_PAGO,103) as FECHA_PAGO ,  A.VALOR_CUOTA, 0 AS  VALOR_INTERES_PAGAR,0 AS TOTAL_A_PAGAR  FROM DETALLES_ACUERDO_PAGO A, PORCENTAJE_TASA_MULTA B ,MAESTRO_ACUERDOS C WHERE A.DOCUMENTO = C.DOCUMENTO AND C.EXPEDIENTE = '" & lblExpediente.Text.Trim() & "' AND DATEDIFF(DAY, FECHA_CUOTA, FECHA_PAGO) >0 AND CUOTA_FIN = 1 ORDER BY A.CUOTA_FIN")

                        For Each row As DataRow In tb1.Rows
                            row("TOTAL_A_PAGAR") = row("VALOR_CUOTA") + row("INTERES")
                            row("TOTAL_A_PAGAR1") = String.Format("{0:C2}", CDbl(row("TOTAL_A_PAGAR")))

                            row("SALDO_PENDIENTE") = row("TOTAL_A_PAGAR") - row("TOTAL_PAGADO")
                            row("SALDO_PENDIENTE1") = String.Format("{0:C2}", CDbl(row("SALDO_PENDIENTE")))

                            row("VALOR_CUOTA1") = String.Format("{0:C2}", CDbl(row("VALOR_CUOTA")))
                            row("INTERES1") = String.Format("{0:C2}", CDbl(row("INTERES")))


                        Next

                        tb2.Rows(0).Item("VALOR_INTERES_PAGAR") = tb1.Compute("SUM(INTERES)", String.Empty)
                        tb2.Rows(0).Item("VALOR_INTERES_PAGAR1") = String.Format("{0:C2}", CDbl(tb2.Rows(0).Item("VALOR_INTERES_PAGAR")))

                        tb2.Rows(0).Item("TOTAL_A_PAGAR") = tb1.Compute("SUM(SALDO_PENDIENTE)", String.Empty) + tb2.Rows(0).Item("VALOR_CUOTA")
                        tb2.Rows(0).Item("TOTAL_A_PAGAR1") = String.Format("{0:C2}", CDbl(tb2.Rows(0).Item("TOTAL_A_PAGAR")))

                        tb2.Rows(0).Item("VALOR_CUOTA1") = String.Format("{0:C2}", CDbl(tb2.Rows(0).Item("VALOR_CUOTA")))



                        datos_individual(2).Marcador = "valor_cuota"
                        datos_individual(2).Valor = String.Format("{0:C2}", CDbl(tb1.Compute("SUM(SALDO_PENDIENTE)", String.Empty)))
                        datos_individual(3).Marcador = "letra_cuota"
                        datos_individual(3).Valor = Num2Text(tb1.Compute("SUM(SALDO_PENDIENTE)", String.Empty))



                        worddocresult = worddoc.CreateReportMultiTable(dtsPlantilla, Reportes.FacilidadesPagoFormatoAjusteUltimaCuota, datos_individual, tb1, 0, False, tb2, 1, False, Nothing, 0, False)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else
                            Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                            SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                        End If
                    Else
                        Alert("No se ha generado facilidad de pago correspondiente a este proceso...")
                        Exit Sub
                    End If
                End If
                If selectcod = "341" Then
                    Dim dtsPlantilla As DataTable

                    If GetDatoPlantillaWord(lblExpediente.Text, "").Rows.Count > 0 Then
                        dtsPlantilla = GetDatoPlantillaWord(lblExpediente.Text, "")
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If


                    Dim datos_individual(1) As WordReport.Marcadores_Adicionales

                    datos_individual(0).Marcador = "Ejecutor"
                    datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                    datos_individual(1).Marcador = "Cargo"
                    datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec

                    worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.OficioInformaNoConcedidaFacilidadInvitaPagoMulta, datos_individual)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "344" Then
                    Dim dtsPlantilla As DataTable

                    If GetDatoPlantillaWord(lblExpediente.Text, "").Rows.Count > 0 Then
                        dtsPlantilla = GetDatoPlantillaWord(lblExpediente.Text, "")
                    Else
                        Alert("No Hay Datos Para generar el documento")
                        Exit Sub
                    End If
                    Dim datos_individual(1) As WordReport.Marcadores_Adicionales

                    datos_individual(0).Marcador = "Ejecutor"
                    datos_individual(0).Valor = DtsDatos.CAT_CLIENTES(0).ent_funcionario_ejec
                    datos_individual(1).Marcador = "Cargo"
                    datos_individual(1).Valor = DtsDatos.CAT_CLIENTES(0).ent_cargo_func_ejec

                    worddocresult = worddoc.CreateReport(dtsPlantilla, Reportes.OficioInformaNoConcedidaFacilidadInvitaPago, datos_individual)
                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If

                If selectcod = "345" Then
                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo
                    fecha2 = DtsDatos.DATOS_REPORTES.Item(0).MT_fecha_ejecutoria
                    If IsDBNull(fecha2) Then
                        Alert("NO SE HA INGRESADO FECHA DE EJECUTORIA DEL TITULO ")
                        Exit Sub
                    End If

                    Dim datos_individual(6) As WordReport.Marcadores_Adicionales
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, selectcod)
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = guardar(0).ToUpper
                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    datos_individual(2).Marcador = "salario_M"
                    datos_individual(2).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    datos_individual(3).Marcador = "salario_L"
                    datos_individual(3).Valor = Num2Text(Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0))
                    datos_individual(3).Marcador = "fecha_actual"
                    datos_individual(3).Valor = guardar(1).ToUpper
                    datos_individual(4).Marcador = "fecha1"
                    datos_individual(4).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    datos_individual(5).Marcador = "fecha_ejecutoria"
                    datos_individual(5).Valor = fecha2.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoMpPagoMulta1438Nuevauenta, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "232" Then

                    Dim datos_individual(16) As WordReport.Marcadores_Adicionales

                    Dim inicial() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim Desembargo() As String
                    Dim Embargo() As String
                    Dim Anterior() As String
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, homologo("316"))
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        inicial(0) = vc_datos(0).ToUpper
                        inicial(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If inicial(0).ToString = "" Then
                        Alert("NO SE DETECTO RESOLUCIÓN DE TERMINACIÓN DE PROCESO PARA ESTE EXPEDIENTE...")
                        Exit Sub
                    Else

                        Desembargo = getResolucion_anterior(lblExpediente.Text, inicial(2))
                        vc_datos = overloadresolucion(lblExpediente.Text, "229")

                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            Desembargo(0) = vc_datos(0).ToUpper
                            Desembargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If Desembargo(0).ToString = "" Then
                            Alert("NO SE DETECTO RESOLUCIÓN DE DESEMBARGO PARA ESTE EXPEDIENTE...")
                            Exit Sub
                        Else

                            Embargo = getResolucion_anterior(lblExpediente.Text, Desembargo(2))
                            vc_datos = overloadresolucion(lblExpediente.Text, homologo("319"))
                            If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                                Embargo(0) = vc_datos(0).ToUpper
                                Embargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                            Else

                            End If

                            If Embargo(0).ToString = "" Then
                                Alert("NO SE DETECTO RESOLUCIÓN DE EMBARGO PARA ESTE EXPEDIENTE...")
                                Exit Sub
                            Else
                                Anterior = getResolucion_anterior(lblExpediente.Text, Embargo(2))
                                vc_datos = overloadresolucion(lblExpediente.Text, "013")
                                If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                                    Anterior(0) = vc_datos(0).ToUpper
                                    Anterior(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                                Else

                                End If

                                If Anterior(0).ToString = "" Then
                                    Alert("NO SE DETECTO RESOLUCIÓN DE MANDAMIENTO DE PAGO PARA ESTE EXPEDIENTE...")
                                    Exit Sub
                                Else

                                End If

                            End If
                        End If
                    End If


                    Dim Valores() As String = GetActo(lblExpediente.Text, "013")
                    Dim representante() As String = cargar_representante(lblExpediente.Text)


                    Dim tbl As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)
                    If tbl.Rows.Count < 0 Then
                        Alert("no hay titulo de deposito judicial ")
                        Exit Sub
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


                    If Desembargo(0) <> Nothing Then
                        datos_individual(2).Marcador = "nro_desembargo"
                        datos_individual(2).Valor = Desembargo(0)
                        datos_individual(3).Marcador = "fecha_desembargo"
                        datos_individual(3).Valor = Desembargo(1)
                    Else
                        Alert("NO SE ENCUENTRA UNA RESOLUCION DE DESEMBARGO ")
                        Exit Sub
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
                        datos_individual(8).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Nombre
                        datos_individual(9).Marcador = "rep_nit"
                        datos_individual(9).Valor = DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit
                    End If
                    datos_individual(10).Marcador = "Nsalario"
                    datos_individual(10).Valor = Round((DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    datos_individual(11).Marcador = "numerosalario"
                    datos_individual(11).Valor = Num2Text(datos_individual(10).Valor)
                    datos_individual(12).Marcador = "tipo_noti"
                    datos_individual(12).Valor = Valores(1)
                    datos_individual(13).Marcador = "acta_nro"
                    datos_individual(13).Valor = Valores(2)
                    datos_individual(14).Marcador = "fecha_acta"
                    datos_individual(14).Valor = (CDate(Valores(0)).ToString("'del' dd 'de' MMMM 'de' yyy"))

                    datos_individual(15).Marcador = "Nro_Resolucion"
                    datos_individual(15).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(16).Marcador = "fecha_actual"
                    datos_individual(16).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.DevolucionTitulo2, datos_individual, tbl, 0, False, tbl, 1, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

                If selectcod = "316" Then
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    DtsDatos.DATOS_REPORTES.Item(0).RESOLUCION = "RESOLUCION"
                    Dim parametro(4) As WordReport.Marcadores_Adicionales

                    fecha = DtsDatos.DATOS_REPORTES.Item(0).MT_fec_expedicion_titulo

                    parametro(0).Marcador = "fecha_actual"
                    parametro(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    parametro(1).Marcador = "fecha1"
                    parametro(1).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper

                    parametro(2).Marcador = "letras"
                    parametro(2).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    parametro(3).Marcador = "Nro_Resolucion"
                    parametro(3).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    parametro(4).Marcador = "fecha_emision"
                    parametro(4).Valor = "XXXXXXXXXXXXXXXXXXXXXX"


                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.AutoTerminacionProcesoPagoTotal, parametro)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If

                End If

                If selectcod = "358" Then


                    Dim datos_individual(2) As WordReport.Marcadores_Adicionales
                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_individual(1).Marcador = "letra"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    datos_individual(2).Marcador = "salario"
                    datos_individual(2).Valor = Round(Convert.ToDouble(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda / 616000), 0)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.CoactivoPlanillaLevantamientoEmbargo, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                End If

                If selectcod = "359" Then
                    Dim datos_individual(12) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    Dim Excepcion() As String = GetExcepcion(lblExpediente.Text)
                    Dim vc_datos() As String
                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)




                    If resolucion(0) <> Nothing Then
                        datos_individual(2).Marcador = "fecha_anterior"
                        datos_individual(2).Valor = resolucion(1).ToUpper

                        datos_individual(3).Marcador = "resolucion_anterior"
                        datos_individual(3).Valor = resolucion(0).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO  PARA GENERAR EL DOCUMENTO ")
                        Exit Sub
                    End If



                    datos_individual(0).Marcador = "Nro_Resolucion"
                    datos_individual(0).Valor = "XXXXXXXXXXXXXXXXXXX"
                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXX"

                    datos_individual(5).Marcador = "acta_numero"
                    datos_individual(5).Valor = Valores(2).ToUpper

                    datos_individual(6).Marcador = "tipo_notificacion"
                    datos_individual(6).Valor = Valores(1).ToUpper

                    datos_individual(7).Marcador = "acta_dia"
                    datos_individual(7).Valor = Valores(0).ToUpper

                    If Excepcion(0) <> Nothing Then
                        datos_individual(8).Marcador = "resolucion_excepcion"
                        datos_individual(8).Valor = Excepcion(0).ToUpper

                        datos_individual(9).Marcador = "fecha_excepcion"
                        datos_individual(9).Valor = Excepcion(1).ToUpper

                        datos_individual(10).Marcador = "nro_radicado"
                        datos_individual(10).Valor = Excepcion(2).ToUpper

                        datos_individual(11).Marcador = "fecha_rad"
                        datos_individual(11).Valor = Excepcion(3).ToUpper

                    Else
                        Alert("NO HAY EXCEPCIONES PARA GENERAR EL DOCUMENTO  SOLICITADO ")
                        Exit Sub
                    End If
                    datos_individual(12).Marcador = "fecha_emision"
                    datos_individual(12).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    SaveTable(lblExpediente.Text, selectcod)
                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.RechazaExcepcionesSigueAdelanteConEjecucion, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "360" Then


                    Dim datos_individual(12) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim Excepcion As String() = GetExcepcion(lblExpediente.Text)
                    Dim Resolucion As String() = getResolucion_anterior(lblExpediente.Text, selectcod)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        Resolucion(0) = vc_datos(0).ToUpper
                        Resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If Resolucion(0) <> Nothing Then
                        datos_individual(0).Marcador = "fecha_anterior"
                        datos_individual(0).Valor = Resolucion(1).ToUpper

                        datos_individual(1).Marcador = "resolucion_anterior"
                        datos_individual(1).Valor = Resolucion(0).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO DE PARA IMPRIMIR EL DOCUMENTO")
                        Exit Sub


                    End If



                    datos_individual(2).Marcador = "Nro_Resolucion"
                    datos_individual(2).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(3).Marcador = "letra"
                    datos_individual(3).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda).ToUpper

                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                    datos_individual(5).Marcador = "acta_numero"
                    datos_individual(5).Valor = Valores(2).ToUpper

                    datos_individual(6).Marcador = "tipo_notificacion"
                    datos_individual(6).Valor = Valores(1).ToUpper

                    datos_individual(7).Marcador = "acta_dia"
                    datos_individual(7).Valor = Valores(0).ToUpper

                    If Excepcion(0) <> Nothing Then
                        datos_individual(8).Marcador = "resolucion_excepcion"
                        datos_individual(8).Valor = Excepcion(0).ToUpper

                        datos_individual(9).Marcador = "fecha_excepcion"
                        datos_individual(9).Valor = Excepcion(1).ToUpper

                        datos_individual(10).Marcador = "nro_radicado"
                        datos_individual(10).Valor = Excepcion(2).ToUpper

                        datos_individual(11).Marcador = "fecha_rad"
                        datos_individual(11).Valor = Excepcion(3).ToUpper

                    Else
                        Alert("NO HAY EXCEPCIONES PARA GENERAR EL DOCUMENTO  SOLICITADO ")
                        Exit Sub
                    End If
                    datos_individual(12).Marcador = "fecha_emision"
                    datos_individual(12).Valor = "XXXXXXXXXXXXXXXXXXXXXX"

                    worddocresult = worddoc.CreateReport(DtsDatos.DATOS_REPORTES, Reportes.RecursoReposicionReposicionParcial, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                If selectcod = "363" Then
                    Dim datos_individual(5) As WordReport.Marcadores_Adicionales
                    Dim Valores As String() = GetActo(lblExpediente.Text, "013")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, selectcod)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    datos_individual(0).Marcador = "letras"
                    datos_individual(0).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        datos_individual(1).Marcador = "fecha_anterior"
                        datos_individual(1).Valor = resolucion(1).ToUpper

                        datos_individual(2).Marcador = "resolucion_anterior"
                        datos_individual(2).Valor = resolucion(0).ToUpper

                    Else
                        Alert("NO HAY RESOLUCION (013) DE MANDAMIENTO DE PAGO GENERAR EL DOCUMENTO ")
                        Exit Sub
                    End If

                    Dim tbl_vin1, tbl_vin2 As New DataTable
                    Dim sql_vinculo1, sql_vinculo2 As SqlDataAdapter

                    sql_vinculo1 = New SqlDataAdapter(" SELECT 0 AS CUENTA ,ED_NOMBRE ," & _
                                " ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP ='" & lblExpediente.Text.Trim & "' AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)

                    sql_vinculo2 = New SqlDataAdapter(" SELECT 0 AS CUENTA ,ED_NOMBRE,  " & _
                                " ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DIGITOVERIFICACION,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_CODIGO_NIT, " & _
                                " PARTICIPACION , '' AS VALORES ,'' AS LETRAS FROM ENTES_DEUDORES ,DEUDORES_EXPEDIENTES  " & _
                                " WHERE  NROEXP ='" & lblExpediente.Text.Trim & "' AND TIPO ='2' " & _
                                " AND   DEUDOR = ED_CODIGO_NIT ", Ado)

                    sql_vinculo1.Fill(tbl_vin1)
                    If tbl_vin1.Rows.Count > 0 Then
                        For i = 0 To tbl_vin1.Rows.Count - 1
                            tbl_vin1.Rows(i).Item("CUENTA") = i + 1
                        Next
                    Else
                        Alert("NO HAY DEUDORES SOLIDARIOS PARA GENERAR EL DOCUMENTO")
                        Exit Sub
                    End If
                    sql_vinculo2.Fill(tbl_vin2)
                    If tbl_vin2.Rows.Count > 0 Then
                        For I = 0 To tbl_vin2.Rows.Count - 1
                            tbl_vin2.Rows(I).Item("CUENTA") = I + 1
                            tbl_vin2.Rows(I).Item("VALORES") = String.Format("{0:C0}", CDbl(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (tbl_vin2.Rows(I).Item("PARTICIPACION") / 100)))
                            tbl_vin2.Rows(I).Item("LETRAS") = Num2Text(tbl_vin2.Rows(I).Item("VALORES"))
                        Next


                    Else

                        Alert("NO HAY DATOS DEUDORES SOLIDARIOS PARA GENERAR EL DOCUMENTO")
                    End If



                    datos_individual(3).Marcador = "Nro_Resolucion"
                    datos_individual(3).Valor = "XXXXXXXXXXXXXXXXXXXXXX"
                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"
                    datos_individual(5).Marcador = "fecha_emision"
                    datos_individual(5).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.VINCULACIONSOCIOSSOLIDARIOSLIQUIDACIÓNOFICIAL, datos_individual, tbl_vin1, 0, False, tbl_vin2, 1, True, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)

                    End If
                End If
                If selectcod = "366" Then
                    Dim Parametros(10) As WordReport.Marcadores_Adicionales
                    Dim vc_datos() As String
                    Dim resolucion_E() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                    vc_datos = overloadresolucion(lblExpediente.Text, "319")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion_E(0) = vc_datos(0).ToUpper
                        resolucion_E(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion_E(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE  EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If

                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_E(2))
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If
                    Dim embargo_valor As String = getEmbargo(0, lblExpediente.Text.Trim)
                    If embargo_valor < 0 Or Nothing Then
                        Alert("NO HAY INGRESADO PORCENTAJE A EMBARGAR PARA PODER GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If

                    Dim embargo As Double = (DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargo_valor / 100))


                    Dim tbl1 As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)
                    If tbl1 Is Nothing = True Then
                        Alert("NO HAY  DATOS DE TITULO DE DEPOSITO JUDICIAL ")
                        Exit Sub
                    End If
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Parametros(0).Marcador = "resolucion_anterior"
                    Parametros(0).Valor = resolucion(0).ToUpper

                    Parametros(1).Marcador = "fecha_anterior"
                    Parametros(1).Valor = resolucion(1).ToUpper

                    Parametros(2).Marcador = "resolucion_embargo"
                    Parametros(2).Valor = resolucion_E(0).ToUpper

                    Parametros(3).Marcador = "fecha_resolucion"
                    Parametros(3).Valor = resolucion_E(1).ToUpper

                    Parametros(4).Marcador = "fecha_actual"
                    Parametros(4).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                    Parametros(5).Marcador = "nro_resolucion"
                    Parametros(5).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                    Parametros(6).Marcador = "Procedencia"
                    Parametros(6).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                    Parametros(7).Marcador = "letras_deuda"
                    Parametros(7).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)


                    Parametros(8).Marcador = "letras_embargo"
                    Parametros(8).Valor = Num2Text(embargo).ToUpper

                    Parametros(9).Marcador = "Total_Embargo"
                    Parametros(9).Valor = String.Format("{0:C0}", embargo)

                    Parametros(10).Marcador = "fecha1"
                    Parametros(10).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo).ToString("'del' dd 'de' MMMM 'de' yyy")


                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.TerminacionporTutela, Parametros, tbl1, 0, False, tbl1, 1, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If


                End If
                If selectcod = "367" Then
                    Dim Parametros(10) As WordReport.Marcadores_Adicionales
                    Dim resolucion_E() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                    DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                    Dim vc_datos() As String
                    vc_datos = overloadresolucion(lblExpediente.Text, "319")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion_E(0) = vc_datos(0).ToUpper
                        resolucion_E(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion_E(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE  EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If

                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_E(2))
                    vc_datos = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                        resolucion(0) = vc_datos(0).ToUpper
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) = Nothing Then
                        Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                        Exit Sub
                    End If


                    Dim tbl1 As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)

                    Dim tbl2 As DataTable = GetTituloPrincipal3(lblExpediente.Text.Trim)
                    Dim valores() As String = GetActo(lblExpediente.Text.Trim, "013")
                    Parametros(0).Marcador = "resolucion_anterior"
                    Parametros(0).Valor = resolucion(0)

                    Parametros(1).Marcador = "fecha_anterior"
                    Parametros(1).Valor = resolucion(1)

                    Parametros(2).Marcador = "resolucion_embargo"
                    Parametros(2).Valor = resolucion_E(0)

                    Parametros(3).Marcador = "fecha_resolucion"
                    Parametros(3).Valor = resolucion_E(1)

                    Parametros(4).Marcador = "fecha_actual"
                    Parametros(4).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                    Parametros(5).Marcador = "nro_resolucion"
                    Parametros(5).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                    Parametros(6).Marcador = "acta"
                    Parametros(6).Valor = valores(2)
                    Parametros(7).Marcador = "formanotificacion"
                    Parametros(7).Valor = valores(1)
                    Parametros(8).Marcador = "fecha_notificacion"
                    Parametros(8).Valor = valores(0)
                    Parametros(9).Marcador = "letras"

                    Parametros(9).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)

                    Parametros(10).Marcador = "fecha1"
                    Parametros(10).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo).ToString("'del' dd 'de' MMMM 'de' yyy")

                    worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.Terminaciondevoluciontitulodepositjudicial, Parametros, tbl1, 0, False, tbl2, 2, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                        SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If


                End If
                If selectcod = "369" Then
                    If DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "06" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "05" Or DtsDatos.DATOS_REPORTES.Item(0).codigotitulo = "07" Then

                        Dim Parametros(14) As WordReport.Marcadores_Adicionales
                        Dim vc_datos() As String
                        Dim resolucion_Desembargo() As String = getResolucion_anterior(lblExpediente.Text.Trim, selectcod)
                        Dim resolucion_E() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_Desembargo(2))
                        Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text.Trim, resolucion_E(2))

                        vc_datos = overloadresolucion(lblExpediente.Text, "229")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion_Desembargo(0) = vc_datos(0).ToUpper
                            resolucion_Desembargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion_Desembargo(0) = Nothing Then
                            Alert("NO HAY RESOLUCION DE  DESEMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If


                        vc_datos = overloadresolucion(lblExpediente.Text, "320")
                        If vc_datos(0) <> Nothing Or vc_datos(0).ToString <> "" Then
                            resolucion_E(0) = vc_datos(0).ToUpper
                            resolucion_E(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion_E(0) = Nothing Then
                            Alert("NO HAY RESOLUCION DE  EMBARGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If


                        vc_datos = overloadresolucion(lblExpediente.Text, "013")
                        If vc_datos(0) <> Nothing Then
                            resolucion(0) = vc_datos(0).ToUpper
                            resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If
                        If resolucion(0) = Nothing Then
                            Alert("NO HAY RESOLUCION DE MANDAMIENTO DE PAGO PARA GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If
                        Dim embargo_valor As String = getEmbargo(0, lblExpediente.Text.Trim)
                        If embargo_valor < 0 Or Nothing Then
                            Alert("NO HAY INGRESADO PORCENTAJE A EMBARGAR PARA PODER GENERAR EL DOCUMENTO SOLICITADO")
                            Exit Sub
                        End If

                        Dim embargo As Double = (DtsDatos.DATOS_REPORTES.Item(0).totaldeuda * (embargo_valor / 100))

                        Dim tbl1 As DataTable = GetTituloPrincipal2(lblExpediente.Text.Trim)
                        If tbl1 Is Nothing = True Then
                            Alert("NO HAY  DATOS DE TITULO DE DEPOSITO JUDICIAL ")
                            Exit Sub
                        End If
                        DtsDatos.DATOS_REPORTES.Item(0).Direccion = Load_multiDirecciones(DtsDatos.DATOS_REPORTES.Item(0).ED_Codigo_Nit, DtsDatos.DATOS_REPORTES.Item(0).ED_TipoId)
                        Parametros(0).Marcador = "resolucion_anterior"
                        Parametros(0).Valor = resolucion(0).ToUpper

                        Parametros(1).Marcador = "fecha_anterior"
                        Parametros(1).Valor = resolucion(1).ToUpper

                        Parametros(2).Marcador = "resolucion_embargo"
                        Parametros(2).Valor = resolucion_E(0).ToUpper

                        Parametros(3).Marcador = "fecha_resolucion"
                        Parametros(3).Valor = resolucion_E(1).ToUpper

                        Parametros(4).Marcador = "fecha_actual"
                        Parametros(4).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                        Parametros(5).Marcador = "nro_resolucion"
                        Parametros(5).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                        Parametros(6).Marcador = "Procedencia"
                        Parametros(6).Valor = GETPROCEDENCIA(lblExpediente.Text.Trim)

                        Parametros(7).Marcador = "letras_deuda"
                        Parametros(7).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)


                        Parametros(8).Marcador = "letras_embargo"
                        Parametros(8).Valor = Num2Text(embargo).ToUpper

                        Parametros(9).Marcador = "Total_Embargo"
                        Parametros(9).Valor = String.Format("{0:C0}", embargo)

                        Parametros(10).Marcador = "fecha1"
                        Parametros(10).Valor = CDate(DtsDatos.DATOS_REPORTES.Item(0).MT_fec_notificacion_titulo).ToString("'del' dd 'de' MMMM 'de' yyy")

                        Parametros(11).Marcador = "fecha_desembargo"
                        Parametros(11).Valor = resolucion_Desembargo(1)
                        Parametros(12).Marcador = "resolucion_desembargo"
                        Parametros(12).Valor = resolucion_Desembargo(0)
                        Parametros(13).Marcador = "letras"
                        Parametros(13).Valor = Num2Text(DtsDatos.DATOS_REPORTES.Item(0).totaldeuda)
                        Parametros(14).Marcador = "fecha_emision"
                        Parametros(14).Valor = "XXXXXXXXXXXXXXXXXXXXXXX"

                        worddocresult = worddoc.CreateReportMultiTable(DtsDatos.DATOS_REPORTES, Reportes.TerminacionProcesoCoactivo_Multa, Parametros, tbl1, 0, False, tbl1, 1, False, Nothing, 0, False)

                        If worddocresult = "" Then
                            ''mensaje no informe
                        Else

                            Dim nombre As String = Replace(Lista.SelectedItem.Text.Trim, " ", ".")
                            SendReport(nombre & lblExpediente.Text & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                        End If
                    Else
                        Alert("ESTE ACTO SOLO SE PUEDE PROCESAR PARA TITULOS DE MULTA ")
                        Exit Sub
                    End If

                End If



                Validator.Text = "Consulta satisfactoria. <br /> <b>IMPRIMIO : " & Lista.SelectedItem.Text & "</b>"
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                Alert(Validator.Text, DtsDatos)
            Else
                Validator.Text = "No hay datos para mostrar."
                Alert(Validator.Text)
                mensup.InnerHtml = Validator.Text
                Me.Validator.IsValid = False
                mensup.InnerHtml = Validator.Text
            End If


        Catch ex As Exception
            Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Es posible que el reporte este inhabilitado."
            Me.Validator.IsValid = False
            mensup.InnerHtml = Validator.Text
            ''Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "mensaje", "<script type=\'text/javascript\'> alert(\'Debe introducir texto en el campo\'); </script>", False)

        End Try




    End Sub



    Protected Sub btnProyectar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProyectar.Click
        proyectar()
    End Sub
    Private Function GetList_Emebargos(ByVal codigoActo As String, ByVal Expediente As String) As DataTable

        ''Dim cmd As SqlCommand
        Dim cn As New SqlClient.SqlConnection(Funciones.CadenaConexion) 'Nueva conexión indicando al SqlConnection la cadena de conexión  
        Dim tb As New DataTable 'Crear y Llenar una Tabla
        Dim cmd As New SqlClient.SqlCommand( _
            " SELECT DISTINCT  A.DG_COD_ACTO,NRORESOLEM,FECRESOLEM " & _
            " FROM DOCUMENTOS_GENERADOS A,EMBARGOS B     " & _
            " WHERE A.DG_EXPEDIENTE  = B.NROEXP          " & _
            " AND B.NROEXP =@EXPEDIENTE                  " & _
            " AND A.DG_COD_ACTO =@COD_ACTO               ", cn) 'Pasar la consulta sql y la conexión al Sql Command   
        cmd.Parameters.AddWithValue("@EXPEDIENTE", Expediente)
        cmd.Parameters.AddWithValue("@COD_ACTO", codigoActo)

        Dim da As New SqlClient.SqlDataAdapter(cmd) 'Inicializar un nuevo SqlDataAdapter   

        da.Fill(tb) 'llena la tabla dependiendo la consulta realizada

        Return tb
    End Function
    Private Function GetList_Embargos2(ByVal codigoActo As String, ByVal Expediente As String) As DataTable

        ''Dim cmd As SqlCommand
        Dim cn As New SqlClient.SqlConnection(Funciones.CadenaConexion) 'Nueva conexión indicando al SqlConnection la cadena de conexión  
        Dim tb As New DataTable 'Crear y Llenar una Tabla

        Dim cmd As New SqlClient.SqlCommand("SP_COACTIVOS_SELECT", cn) 'Pasar la consulta sql y la conexión al Sql Command   
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@EXPEDIENTE", Expediente)
        cmd.Parameters.AddWithValue("@ACTO", codigoActo)

        Dim da As New SqlClient.SqlDataAdapter(cmd) 'Inicializar un nuevo SqlDataAdapter   

        da.Fill(tb) 'llena la tabla dependiendo la consulta realizada

        Return tb
    End Function


    Private Function obtenerDatos(ByVal codigo As String, ByVal tipo_embargo As String, Optional ByVal DP As String = "0") As DataTable
        Dim TIPOS_EMBARGOS As String
        Dim consulta As String
        If tipo_embargo = 0 Then
            If DP = "349" Or DP = "380" Then
                TIPOS_EMBARGOS = " AND N.NroResolEm=@RESOLUCION"
            Else
                TIPOS_EMBARGOS = " AND M.NroResolEm=@RESOLUCION"
            End If

        Else
            TIPOS_EMBARGOS = ""
        End If

        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Select Case DP
            Case "349", "380"
                consulta = " SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_Codigo_Nit + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                                                               " 	  c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP,M.BAN_DIRECCION AS DIRECCIONBANCARIA , M.BAN_TELEFONO AS TELEFONOBANCARIO , M.BAN_NOMBRE AS NOMBREBANCARIO, F.codigo as codigotitulo,                       " & _
                                                               "      N.NroResolEm, N.FecResolEm ,N.LimiteEm              FROM  EJEFISGLOBAL A,                                                                                                                                                                                                                      " & _
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
                                                               " 	  USUARIOS L, MAESTRO_BANCOS M, EMBARGOS N                                                                                                                                                                                                          " & _
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                               " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                           " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                           " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                        " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                          " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                              " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                            " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                  " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & TIPOS_EMBARGOS & "                                                                                                                                                                                               " & _
                                                               " ORDER BY TIPO"
            Case Else
                consulta = " SELECT TOP 1 A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(E.totaldeuda  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(E.totaldeuda  AS money))-1)as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " 	   k.nombre as Proyecto,ED_TipoPersona as DOCUMENTO,                                                                                                                                                                                                                " & _
                                                               " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " 	   D.TIPO as Tipo_Deudor,F.codigo as codigotitulo,                                                                                                                                                                                                              " & _
                                                               " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP,M.NroResolEm,M.FecResolEm ,M.LimiteEm                                                                                                                                                                                                                                  " & _
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
                                                               " 	  USUARIOS L, EMBARGOS M                                                                                                                                                                                                                            " & _
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                              " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                          " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                          " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                       " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                         " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                             " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                           " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                 " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE " & TIPOS_EMBARGOS & "                                                                                                                                                                                       " & _
                                                               " ORDER BY TIPO"

        End Select

        Dim sqlcommand As New SqlCommand(consulta)
        Dim sqlmanager As SqlDataAdapter
        Dim sqltbl As New DataTable
        sqlcommand.Connection = sqlconfig
        sqlcommand.Parameters.AddWithValue("@EXPEDIENTE", lblExpediente.Text.Trim)
        If tipo_embargo = 0 Then
            sqlcommand.Parameters.AddWithValue("@RESOLUCION", codigo)

        End If

        sqlmanager = New SqlDataAdapter(sqlcommand)
        sqlmanager.Fill(sqltbl)

        If sqltbl.Rows.Count > 0 Then
            Return sqltbl
        Else
            Return Nothing
        End If

    End Function


    Private Sub btnSi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSi.Click

        Dim worddoc As New WordReport
        Dim worddocresult As String = ""
        Dim codigo As String = list_embargos.SelectedItem.Value
        Dim sel As String = valida_Repo()
        Dim DTSDATOS As DataTable = obtenerDatos(codigo, 0, sel)
        Dim Socios_solidarios() As String = loadMultisocios(lblExpediente.Text.Trim, DTSDATOS.Rows(0).Item("totaldeuda"))
        Dim Mostras_saldo As Double = CDbl(Mostrar_Saldo_Actual(lblExpediente.Text.Trim))
        Select Case sel
            Case "319"
                If Socios_solidarios(0) = "" Then
                    btnadicionar.Enabled = True

                    Dim parametro(7) As WordReport.Marcadores_Adicionales
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    Dim embargos_r As Integer = DTSDATOS.Rows(0).Item("LimiteEm")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = resolucion(0)
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1)

                    Else
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = ""
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = ""
                    End If


                    parametro(2).Marcador = "letras"
                    parametro(2).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))

                    If embargos_r > 0 Then

                        ''Dim guardar() As String = saveResolucion(lblExpediente.Text, sel)

                        parametro(3).Marcador = "fecha_actual"
                        parametro(3).Valor = CDate(DTSDATOS.Rows(0).Item("FecResolEm")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        parametro(4).Marcador = "Nro_resolucion"
                        parametro(4).Valor = DTSDATOS.Rows(0).Item("NroResolEm")
                        parametro(5).Marcador = "pxem"
                        parametro(5).Valor = embargos
                        parametro(6).Marcador = "Total_Embargo"
                        parametro(6).Valor = String.Format("{0:C0}", CDbl(embargos_r))
                        parametro(7).Marcador = "embargol"
                        parametro(7).Valor = Num2Text(Round(embargos_r, 0))
                    Else
                        'Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                        Exit Sub
                    End If

                    ''SaveTable(lblExpediente.Text.Trim, selectcod, parametro(6).Valor)
                    worddocresult = worddoc.CreateReport(DTSDATOS, Reportes.EmbargoBancarioLiquidacionOficial, parametro)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                Else

                    btnadicionar.Enabled = True

                    Dim parametro(7) As WordReport.Marcadores_Adicionales
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    Dim embargos_r As Integer = DTSDATOS.Rows(0).Item("LimiteEm")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")
                    Dim tbl As DataTable = tabladatosembargos(lblExpediente.Text.Trim)
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = resolucion(0)
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1)

                    Else
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = ""
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = ""
                    End If


                    parametro(2).Marcador = "letras"
                    parametro(2).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))

                    If embargos_r > 0 Then

                        ''Dim guardar() As String = saveResolucion(lblExpediente.Text, sel)

                        parametro(3).Marcador = "fecha_actual"
                        parametro(3).Valor = CDate(DTSDATOS.Rows(0).Item("FecResolEm")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        parametro(4).Marcador = "nro_resolucion"
                        parametro(4).Valor = DTSDATOS.Rows(0).Item("NroResolEm")
                        parametro(5).Marcador = "pxem"
                        parametro(5).Valor = embargos
                        parametro(6).Marcador = "Sociedad"
                        parametro(6).Valor = Socios_solidarios(0)
                        parametro(7).Marcador = "Valorsco"
                        parametro(7).Valor = Socios_solidarios(1)

                    Else
                        'Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                        Exit Sub
                    End If

                    For i = 0 To tbl.Rows.Count - 1

                        If tbl.Rows(i).Item("tipo") = 1 Then
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(DTSDATOS.Rows(0).Item("LimiteEm")))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        Else
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(((tbl.Rows(i).Item("PARTICIPACION") / 100) * DTSDATOS.Rows.Item(0)("totaldeuda")) * (embargos / 100)))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        End If

                    Next

                    worddocresult = worddoc.CreateReportMultiTable(DTSDATOS, Reportes.EmbargosSocios, parametro, tbl, 0, False, Nothing, 0, False, Nothing, 0, False)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                
            Case "320"
                btnadicionar.Enabled = True

                If Socios_solidarios(0) = "" Then
                    Dim parametros(7) As WordReport.Marcadores_Adicionales
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    Dim embargos_r As Integer = DTSDATOS.Rows(0).Item("LimiteEm")
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    parametros(0).Marcador = "letras"
                    parametros(0).Valor = Num2Text(DTSDATOS.Rows(0).Item("totaldeuda"))

                    parametros(1).Marcador = "fecha_actual"
                    parametros(1).Valor = CDate(DTSDATOS.Rows(0).Item("FecResolEm")).ToString("'del' dd 'de' MMMM 'de' yyy")
                    parametros(2).Marcador = "Nro_resolucion"
                    parametros(2).Valor = DTSDATOS.Rows(0).Item("NroResolEm")
                    ''If resolucion(0) <> Nothing Then
                    parametros(3).Marcador = "nro_anterior"
                    parametros(3).Valor = resolucion(0)
                    parametros(4).Marcador = "fecha_resolucion"
                    parametros(4).Valor = resolucion(1)
                    parametros(5).Marcador = "pxem"
                    parametros(5).Valor = embargos
                    parametros(6).Marcador = "Total_Embargo"
                    parametros(6).Valor = String.Format("{0:C0}", CDbl(embargos_r))
                    parametros(7).Marcador = "embargol"
                    parametros(7).Valor = Num2Text(Round(embargos_r, 0))
                    ''Else
                    ''Alert("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO  PARA IMPRIMIR EL DOCUMENTO")
                    ''Exit Sub
                    ''End If

                    worddocresult = worddoc.CreateReport(DTSDATOS, Reportes.EmbargoCuentaBancariaMulta, parametros)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                Else
                    btnadicionar.Enabled = True


                    Dim parametro(7) As WordReport.Marcadores_Adicionales
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    Dim embargos_r As Integer = DTSDATOS.Rows(0).Item("LimiteEm")
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")
                    Dim tbl As DataTable = tabladatosembargos(lblExpediente.Text.Trim)
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = resolucion(0)
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1)

                    Else
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = ""
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = ""
                    End If


                    parametro(2).Marcador = "letras"
                    parametro(2).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))

                    If embargos_r > 0 Then

                        ''Dim guardar() As String = saveResolucion(lblExpediente.Text, sel)

                        parametro(3).Marcador = "fecha_actual"
                        parametro(3).Valor = CDate(DTSDATOS.Rows(0).Item("FecResolEm")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        parametro(4).Marcador = "nro_resolucion"
                        parametro(4).Valor = DTSDATOS.Rows(0).Item("NroResolEm")
                        parametro(5).Marcador = "pxem"
                        parametro(5).Valor = embargos
                        parametro(6).Marcador = "Sociedad"
                        parametro(6).Valor = Socios_solidarios(0)
                        parametro(7).Marcador = "Valorsco"
                        parametro(7).Valor = Socios_solidarios(1)

                    Else
                        'Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                        Exit Sub
                    End If

                    For i = 0 To tbl.Rows.Count - 1
                        If tbl.Rows(i).Item("tipo") = 1 Then
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(DTSDATOS.Rows(0).Item("LimiteEm")))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        Else
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(((tbl.Rows(i).Item("PARTICIPACION") / 100) * DTSDATOS.Rows.Item(0)("totaldeuda")) * (embargos / 100)))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        End If

                    Next

                    worddocresult = worddoc.CreateReportMultiTable(DTSDATOS, Reportes.EmbargosSocios, parametro, tbl, 0, False, Nothing, 0, False, Nothing, 0, False)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
                

            Case "380"
                btnadicionar.Enabled = False
                Dim fecha As Date = DTSDATOS.Rows(0).Item("MT_fec_expedicion_titulo")
                Dim datos(2) As WordReport.Marcadores_Adicionales
                Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                Dim embargo As Double = getEmbargo(0, lblExpediente.Text.Trim)
                Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")
                If vc_datos(0) <> Nothing Then
                    resolucion(0) = vc_datos(0)
                    resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If

                If embargo < 0 Then
                    Alert("El porcentaje de embargo no ha sido ingresado  para poder genera la plantilla ")
                    Exit Sub
                End If

                datos(0).Marcador = "fecha1"
                datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                ''Dim tipotitulo As String


                If resolucion(0) <> Nothing Then
                    datos(1).Marcador = "rresolucion"
                    datos(1).Valor = resolucion(0).ToUpper
                    datos(2).Marcador = "rfecha"
                    datos(2).Valor = resolucion(1).ToUpper
                Else
                    Alert("NO HAY RESOLUCION DE EMBARGO PARA GENERAR LA PLANTILLA")
                    Exit Sub
                End If

                If embargo > 0 Then
                    DTSDATOS.Rows(0).Item("Total_deuda") = String.Format("{0:C0}", CDbl(DTSDATOS.Rows(0).Item("totaldeuda") * (embargo / 100)))

                Else
                    Alert("NO HAY INGRESADO PORCENTAJE DE EMBARGO ")
                End If

                worddocresult = worddoc.CreateReport(DTSDATOS, Reportes.PlanillaEmbargo, datos)

                If worddocresult = "" Then
                    ''mensaje no informe
                Else
                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                    SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                End If


            Case "349"
                btnadicionar.Visible = False
                Dim fecha As Date = DTSDATOS.Rows(0).Item("MT_fec_expedicion_titulo")
                Dim datos(2) As WordReport.Marcadores_Adicionales
                Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                Dim embargo As Double = getEmbargo(0, lblExpediente.Text.Trim)
                If embargo < 0 Then
                    Alert("El porcentaje de embargo no ha sido ingresado  para poder genera la plantilla ")
                    Exit Sub
                End If

                Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "319")

                If vc_datos(0) <> Nothing Then
                    resolucion(0) = vc_datos(0)
                    resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If

                datos(0).Marcador = "fecha1"
                datos(0).Valor = fecha.ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                ''Dim tipotitulo As String


                If resolucion(0) <> Nothing Then
                    datos(1).Marcador = "rresolucion"
                    datos(1).Valor = resolucion(0).ToUpper
                    datos(2).Marcador = "rfecha"
                    datos(2).Valor = resolucion(1).ToUpper
                Else
                    Alert("NO HAY RESOLUCION DE EMBARGO PARA GENERAR LA PLANTILLA")
                    Exit Sub
                End If

                If embargo > 0 Then
                    For i = 0 To DTSDATOS.Rows.Count - 1
                        DTSDATOS.Rows(i).Item("Total_deuda") = String.Format("{0:C0}", CDbl(DTSDATOS.Rows(i).Item("totaldeuda") * (embargo / 100)))
                    Next

                    'DTSDATOS.Rows(0).Item("Total_deuda") = String.Format("{0:C0}", CDbl(DTSDATOS.Rows(0).Item("totaldeuda") * (embargo / 100)))

                Else
                    Alert("NO HAY INGRESADO PORCENTAJE DE EMBARGO ")
                End If

                worddocresult = worddoc.CreateReport(DTSDATOS, Reportes.PlanillaEmbargo, datos)

                If worddocresult = "" Then
                    ''mensaje no informe
                Else
                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                    SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                End If
        End Select
        ModalPopupExtender2.Hide()

    End Sub

    Private Sub btnadicionar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadicionar.Click

        Dim worddoc As New WordReport
        Dim worddocresult As String = ""
        'Dim codigo As String = list_embargos.SelectedItem.Value
        Dim DTSDATOS As DataTable = obtenerDatos(0, 1)
        Dim sel As String = valida_Repo()
        Dim Socios_solidarios() As String = loadMultisocios(lblExpediente.Text.Trim, DTSDATOS.Rows(0).Item("totaldeuda"))
        'Cambio Solicitado Para generar los embargos en base al saldo y no al total de la deuda '
        Dim mostrar_saldo As Double = CDbl(Mostrar_Saldo_Actual(lblExpediente.Text.Trim))
        Select Case sel
            Case "319"
                If Socios_solidarios(0) = "" Then
                    Dim parametro(7) As WordReport.Marcadores_Adicionales
                    Dim embargos As Integer = getEmbargo(0, lblExpediente.Text.Trim)
                    If embargos = Nothing Then
                        If Limite.Text = "" Then
                            embargos = 200
                        Else
                            embargos = Limite.Text.Trim
                        End If

                    End If
                    Dim embargos_r As Double = ((embargos / 100) * mostrar_saldo)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = resolucion(0)
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1)

                    Else
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = ""
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = ""

                    End If
                    ''If embargos_r > 0 Then
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, sel, 1)

                    parametro(2).Marcador = "fecha_actual"
                    parametro(2).Valor = guardar(1)
                    parametro(3).Marcador = "Nro_resolucion"
                    parametro(3).Valor = guardar(0)
                    parametro(4).Marcador = "pxem"
                    parametro(4).Valor = embargos
                    parametro(5).Marcador = "Total_Embargo"
                    parametro(5).Valor = String.Format("{0:C0}", embargos_r)
                    parametro(6).Marcador = "embargol"
                    parametro(6).Valor = Num2Text(Round(embargos_r, 0))
                    parametro(7).Marcador = "letras"
                    parametro(7).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))
                    SaveTable(lblExpediente.Text.Trim, sel, guardar(0), CDate(guardar(2)), embargos_r, embargos)
                    'Else
                    'Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                    'Exit Sub
                    'End If

                    worddocresult = worddoc.CreateReport(DTSDATOS, Reportes.EmbargoBancarioLiquidacionOficial, parametro)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                Else
                    btnadicionar.Enabled = True

                    Dim parametro(7) As WordReport.Marcadores_Adicionales
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                    If embargos = Nothing Then
                        If Limite.Text = "" Then
                            embargos = 200
                        Else
                            embargos = Limite.Text.Trim
                        End If

                    End If

                    'Dim embargos_r As Integer = DTSDATOS.Rows(0).Item("LimiteEm")


                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")
                    Dim tbl As DataTable = tabladatosembargos(lblExpediente.Text.Trim)
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = resolucion(0)
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1)

                    Else
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = ""
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = ""
                    End If


                    parametro(2).Marcador = "letras"
                    parametro(2).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))

                    If embargos > 0 Then

                        Dim guardar() As String = saveResolucion(lblExpediente.Text, sel, 1)

                        parametro(3).Marcador = "fecha_actual"
                        parametro(3).Valor = guardar(1)
                        parametro(4).Marcador = "nro_resolucion"
                        parametro(4).Valor = guardar(0)
                        parametro(5).Marcador = "pxem"
                        parametro(5).Valor = embargos
                        parametro(6).Marcador = "Sociedad"
                        parametro(6).Valor = Socios_solidarios(0)
                        parametro(7).Marcador = "Valorsco"
                        parametro(7).Valor = Socios_solidarios(1)

                        SaveTable(lblExpediente.Text.Trim, sel, guardar(0), CDate(guardar(2)), ((embargos / 100) * mostrar_saldo), embargos)

                    Else
                        'Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                        Exit Sub
                    End If

                    For i = 0 To tbl.Rows.Count - 1
                        If tbl.Rows(i).Item("tipo") = 1 Then
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(((100 / 100) * mostrar_saldo) * (embargos / 100)))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        Else
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(((tbl.Rows(i).Item("PARTICIPACION") / 100) * mostrar_saldo) * (embargos / 100)))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        End If
                    Next

                    worddocresult = worddoc.CreateReportMultiTable(DTSDATOS, Reportes.EmbargosSocios, parametro, tbl, 0, False, Nothing, 0, False, Nothing, 0, False)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If
            Case "320"
                If Socios_solidarios(0) = "" Then
                    Dim parametro(7) As WordReport.Marcadores_Adicionales
                    Dim embargos As Integer = getEmbargo(0, lblExpediente.Text.Trim)

                    If embargos = Nothing Then
                        If Limite.Text = "" Then
                            embargos = 150
                        Else
                            embargos = Limite.Text.Trim
                        End If

                    End If
                    Dim embargos_r As Double = ((embargos / 100) * mostrar_saldo)
                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")

                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If



                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = resolucion(0)
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1)

                    Else
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = ""
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = ""

                    End If
                    'If embargos_r > 0 Then
                    Dim guardar() As String = saveResolucion(lblExpediente.Text, sel, 1)

                    parametro(2).Marcador = "fecha_actual"
                    parametro(2).Valor = guardar(1)
                    parametro(3).Marcador = "Nro_resolucion"
                    parametro(3).Valor = guardar(0)
                    parametro(4).Marcador = "pxem"
                    parametro(4).Valor = embargos
                    parametro(5).Marcador = "Total_Embargo"
                    parametro(5).Valor = String.Format("{0:C0}", embargos_r)
                    parametro(6).Marcador = "embargol"
                    parametro(6).Valor = Num2Text(Round(embargos_r, 0))
                    parametro(7).Marcador = "letras"
                    parametro(7).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))
                    SaveTable(lblExpediente.Text.Trim, sel, guardar(0), CDate(guardar(2)), embargos_r, embargos)
                    'Else
                    'Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                    'Exit Sub
                    'End If

                    worddocresult = worddoc.CreateReport(DTSDATOS, Reportes.EmbargoCuentaBancariaMulta, parametro)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                Else

                    btnadicionar.Enabled = True

                    Dim parametro(7) As WordReport.Marcadores_Adicionales
                    Dim embargos As Double = getEmbargo(0, lblExpediente.Text.Trim)
                   
                    If embargos = Nothing Then

                        If Limite.Text = "" Then
                            embargos = 150
                        Else
                            embargos = Limite.Text.Trim
                        End If

                    End If

                    'Dim embargos_r As Integer = DTSDATOS.Rows(0).Item("LimiteEm")


                    Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                    Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")
                    Dim tbl As DataTable = tabladatosembargos(lblExpediente.Text.Trim)
                    If vc_datos(0) <> Nothing Then
                        resolucion(0) = vc_datos(0)
                        resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = resolucion(0)
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = resolucion(1)

                    Else
                        parametro(0).Marcador = "nro_anterior"
                        parametro(0).Valor = ""
                        parametro(1).Marcador = "fecha_resolucion"
                        parametro(1).Valor = ""
                    End If


                    parametro(2).Marcador = "letras"
                    parametro(2).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))

                    If embargos > 0 Then

                        Dim guardar() As String = saveResolucion(lblExpediente.Text, sel, 1)

                        parametro(3).Marcador = "fecha_actual"
                        parametro(3).Valor = guardar(1)
                        parametro(4).Marcador = "nro_resolucion"
                        parametro(4).Valor = guardar(0)
                        parametro(5).Marcador = "pxem"
                        parametro(5).Valor = embargos
                        parametro(6).Marcador = "Sociedad"
                        parametro(6).Valor = Socios_solidarios(0)
                        parametro(7).Marcador = "Valorsco"
                        parametro(7).Valor = Socios_solidarios(1)

                        SaveTable(lblExpediente.Text.Trim, sel, guardar(0), CDate(guardar(2)), ((embargos / 100) * mostrar_saldo), embargos)

                    Else
                        'Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                        Exit Sub
                    End If

                    For i = 0 To tbl.Rows.Count - 1

                        If tbl.Rows(i).Item("tipo") = 1 Then
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(((100 / 100) * mostrar_saldo) * (embargos / 100)))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        Else
                            tbl.Rows(i).Item("limite") = String.Format("{0:C0}", CDbl(((tbl.Rows(i).Item("PARTICIPACION") / 100) * mostrar_saldo) * (embargos / 100)))
                            tbl.Rows(i).Item("porcentaje") = embargos
                        End If
                    Next

                    worddocresult = worddoc.CreateReportMultiTable(DTSDATOS, Reportes.EmbargosSocios, parametro, tbl, 0, False, Nothing, 0, False, Nothing, 0, False)


                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else
                        Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                        SendReport(nombre & lblExpediente.Text & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

            Case "321"
                Dim parametro(7) As WordReport.Marcadores_Adicionales
                Dim embargos As Integer = getEmbargo(0, lblExpediente.Text.Trim)
                If embargos = Nothing Then
                    If Limite.Text = "" Then
                        embargos = 150
                    Else
                        embargos = Limite.Text.Trim
                    End If

                End If
                Dim embargos_r As Double = ((embargos / 100) * mostrar_saldo)
                Dim resolucion() As String = getResolucion_anterior(lblExpediente.Text, sel)
                Dim vc_datos() As String = overloadresolucion(lblExpediente.Text, "013")

                If vc_datos(0) <> Nothing Then
                    resolucion(0) = vc_datos(0)
                    resolucion(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If



                If resolucion(0) <> Nothing Then
                    parametro(0).Marcador = "nro_anterior"
                    parametro(0).Valor = resolucion(0)
                    parametro(1).Marcador = "fecha_resolucion"
                    parametro(1).Valor = resolucion(1)

                Else
                    parametro(0).Marcador = "nro_anterior"
                    parametro(0).Valor = ""
                    parametro(1).Marcador = "fecha_resolucion"
                    parametro(1).Valor = ""

                End If
                '                If embargos_r > 0 Then
                Dim guardar() As String = saveResolucion(lblExpediente.Text, sel, 1)

                parametro(2).Marcador = "fecha_actual"
                parametro(2).Valor = guardar(1)
                parametro(3).Marcador = "Nro_resolucion"
                parametro(3).Valor = guardar(0)
                parametro(4).Marcador = "pxem"
                parametro(4).Valor = embargos
                parametro(5).Marcador = "Total_Embargo"
                parametro(5).Valor = String.Format("{0:C0}", embargos_r)
                parametro(6).Marcador = "embargol"
                parametro(6).Valor = Num2Text(Round(embargos_r, 0))
                parametro(7).Marcador = "letras"
                parametro(7).Valor = Num2Text(DTSDATOS.Rows.Item(0)("totaldeuda"))

                SaveTable(lblExpediente.Text.Trim, sel, guardar(0), CDate(guardar(2)), embargos_r, embargos)
                'Else
                ' Alert("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                'Exit Sub
                'End If

                worddocresult = worddoc.CreateReport(DTSDATOS, Reportes.EmbargoCuentaBancariaMultaHospitales, parametro)

                If worddocresult = "" Then
                    ''mensaje no informe
                Else
                    Dim nombre As String = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                    SendReport(nombre & "-" & lblExpediente.Text & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                End If


        End Select
    End Sub

    Private Sub btn_enviar_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_enviar_2.Click
        btnadicionar_Click(Nothing, Nothing)
        ModalPopupExtender3.Hide()
        Limite.Text = ""
    End Sub

    Private Function ValidarAcuerdo(ByVal expediente As String) As Boolean
        Dim sw As Boolean = False
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT * FROM MAESTRO_ACUERDOS WHERE EXPEDIENTE = '" & expediente & "'")

        If tb.Rows.Count > 0 Then
            sw = True
        End If

        Return sw
    End Function

End Class