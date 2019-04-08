Imports System.Data.SqlClient
Partial Public Class historiaexpediente
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Protected Sub Gridexpedinete_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Gridexpedinete.SelectedIndexChanged
        With Me
            Dim Mytb As DataTable = CType(.ViewState("Predio_documento"), DataTable)

            Dim dato As String = ""
            dato = Mytb.Rows(.Gridexpedinete.SelectedIndex).Item("docexpediente")
            Dim idEntidad As String
            idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
            examenExpediente(dato, "")
            expediente(idEntidad, dato)
        End With
    End Sub

    Private Sub ExpedientesPredio(ByVal Predio As String)
        documento(Predio.Trim)
        Dim Mytb As DataTable = CType(Me.ViewState("Predio_documento"), DataTable)
        If Mytb.Rows.Count > 0 Then
            Divtota.InnerHtml = "<b>EXPEDIENTES DETECTADOS : " & Mytb.Rows.Count & "</b>"

            Gridexpedinete.DataSource = Mytb
            Gridexpedinete.DataBind()

            Me.ModalPopupExtender3.Show()
        Else
            'Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>NO POSEE EXPEDIENTE DIGITALIZADOS</font>"
        End If
    End Sub

    Function documento(ByVal Predio As String) As DataTable
        Dim datata As New DataTable
        Dim myadapa As New SqlDataAdapter("select distinct (docexpediente) as docexpediente from documentos where docpredio_refecatrastal = @docpredio_refecatrastal", Funciones.CadenaConexion)
        myadapa.SelectCommand.Parameters.Add("@docpredio_refecatrastal", SqlDbType.VarChar).Value = Predio
        myadapa.Fill(datata)

        Me.ViewState("Predio_documento") = datata
        Return datata
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel()"
            menssageError(amsbgox)
            Exit Sub
        End If

        If Not Me.Page.IsPostBack Then
            Page.Form.Attributes.Add("autocomplete", "off")
            If Not Request("ejecuciones") Is Nothing Then
                buscaexpediente.Attributes.Add("style", "display:none;")
                ssherramienta2.Attributes.Add("style", "display:none;")
            End If

            Dim impuesto As Byte = Session("ssimpuesto")

            'Detecta si el viene de ejecuciones fiscales 
            Dim tipo As String = Request("tipo")
            If tipo = 1 Then
                btnAceptar.Visible = False
                btnCancelar.Visible = False

                Dim repexpediente As String = Request("expediente")

                If repexpediente <> Nothing Then
                    Dim cedula As String = Request("cedula")
                    txtEnte.Enabled = False
                    ejeFiscales.Attributes.Add("style", "position:absolute;top:50px;height:105px;width:735px;left:23px;display:block;")
                    ejDeudor.Text = Request("cedula")
                    ejExpediente.Text = Request("expediente")
                    'ejPredio.InnerHtml = Request("refcatastral")
                    ejdeuNombre.Text = Request("deunom")
                    'ejvigen.Text = "Periodos del <b>" & Request("des") & "</b> hasta <b>" & Request("has") & "</b>"
                    EjecucionesFiscalesLink.Attributes.Add("href", "ejecucionesFiscales.aspx?opconsul=" & Request("opconsul") & "&cedula=" & Request("cedula") & "&deunom=" & Request("deunom") & "&expediente=" & Request("expediente") & "&tipo=1")
                    Anexo_adjuntados.Attributes.Add("style", "display:block;")
                    message_box.Attributes.Add("style", "display:none;")
                    'xLinkVigencias(refcatastral.Trim, Request("deunom") & "(" & Request("cedula") & ")", Request("des"), Request("has"), ejvigen.Text)
                    expediente(ejDeudor.Text, repexpediente.Trim)
                    examenExpediente(repexpediente, cedula)
                    If Request("acupp") = "si" Then
                        Validator.Text = "<a href='generador-expedientes.aspx'> Volver al menú <b>COBRANZAS</b></a>"
                        Me.Validator.IsValid = False
                    End If
                    txtEnte.Text = Request("deunom").Trim & "::" & Request("cedula").Trim

                    Call registra_clientPredio()
                End If
            ElseIf tipo = 2 Then
                txtEnte.Text = Request("deunom").Trim & "::" & Request("cedula").Trim
                expediente(Request("cedula"), Request("expediente"))
                examenExpediente(Request("expediente"), "")
                btnRegresar.Visible = True
            End If
        End If
    End Sub

    Private Sub registra_clientPredio()
        Dim scriptText As String = " jQuery(document).ready(function($) {"
        If Not Request.Browser.Browser.ToLower = "ie" Then
            scriptText += "$('#ejPredio').bt({ajaxPath: 'cuadros/detallespredio.aspx?predio=' + document.getElementById('HiddenPredio').value,centerPointY: .2,trigger: 'click', ajaxLoading : ""<img src='images/ajax-loader2.gif' height='24' width='220' style='margin: 7px 7px 7px 100px;' />"",positions: ['right', 'left'],padding: 0,width: 420,spikeGirth: 20,spikeLength: 30,cornerRadius: 10,fill: '#FFF',strokeStyle: '#3c5d9c',shadow: true,shadowBlur: 12,shadowOffsetX: 0,shadowOffsetY: 5,hoverIntentOpts: { interval: 800, timeout: 0 },cssStyles: {fontSize: '12px',fontFamily: 'arial,helvetica,sans-serif'}});"
        Else
            scriptText += "$('#ejPredio').click(function(evento) {var toLoad = 'cuadros/detallespredio.aspx?predio=' + document.getElementById('HiddenPredio').value;$('#xShow_ejPredio_Content').hide('fast', loadContent);function loadContent() {$('#xShow_ejPredio').load(toLoad, '', showNewContent())}function showNewContent() {$('#xShow_ejPredio_Content').fadeIn('show');}});"
        End If
        scriptText += "});"
        ClientScript.RegisterClientScriptBlock(Me.GetType(), _
                        "CounterScript", scriptText, True)
    End Sub

    'Private Sub xLinkVigencias(ByVal ReferVar As String, ByVal deudor As String, ByVal des As String, ByVal has As String, ByVal vigencia As String)
    '    'Analizar Vigencias 
    '    linkVigencias.Attributes("href") = "javascript:;"
    '    linkVigencias2.Attributes("onclick") = "window.open('cuadros/vigenciasEjecucionFiscal.aspx?refExp=" & ReferVar.Trim & "&des=" & des & "&has=" & has & "&deudor=" & deudor & "&expediente=" & Request("expediente") & "','xvigcal','width=750,height=325,left=270,top=180,scrollbars=yes')"

    '    linkVigencias2.Attributes("href") = "javascript:;"
    '    linkVigencias2.Attributes("onclick") = "window.open('cuadros/vigenciasEjecucionFiscal.aspx?refExp=" & ReferVar.Trim & "&des=" & des & "&has=" & has & "&deudor=" & deudor & "&expediente=" & Request("expediente") & "','xvigcal','width=750,height=325,left=270,top=180,scrollbars=yes')"
    'End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Call ejecutar_expediente()
    End Sub

    Private Sub ejecutar_expediente()
        ListExpedientes.Items.Clear()
        Dim idEntidad As String
        idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)

        Dim cmd As String
        Dim cnn As String = Session("ConexionServer")

        cmd = "select distinct docexpediente from documentos where RTRIM(entidad) = @entidad AND RTRIM(documentos.cobrador) = @cobrador"
        'cmd = "select distinct docexpediente from documentos where RTRIM(entidad) = @idEntidad AND RTRIM(documentos.cobrador) = '" & Session("mcobrador") & "'"
        Dim MyAdapter As New SqlClient.SqlDataAdapter(cmd, cnn)
        MyAdapter.SelectCommand.Parameters.Add("@entidad", SqlDbType.VarChar).Value = idEntidad
        MyAdapter.SelectCommand.Parameters.Add("@cobrador", SqlDbType.VarChar).Value = Session("mcobrador")

        Dim myTable As New DataTable
        MyAdapter.Fill(myTable)

        If myTable.Rows.Count > 1 Then
            Me.ModalPopupExtender1.Show()
            
            Dim xt As Integer = 0
            For xt = 0 To myTable.Rows.Count - 1
                ListExpedientes.Items.Add(myTable.Rows(xt).Item("docexpediente"))
            Next
        ElseIf myTable.Rows.Count > 0 Then
            'Mostrar el historico de los hepedientes 
            expediente(idEntidad, myTable.Rows(0).Item("docexpediente"))
            examenExpediente(myTable.Rows(0).Item("docexpediente"), "")
        Else
            Validator.Text = "No se detectaron expedientes."
            Me.Validator.IsValid = False
        End If
    End Sub

    Private Sub examenExpediente(ByVal expediente As String, ByVal Referencia_Catastral As String)
        Dim texto As String = "<a title='Visualice todos los registros del expediente sin tener en cuenta los formatos o restricciones del secuenciador.' href='javascript:;' class='xa' onclick = ""window.open('cuadros/expedinteexaminar.aspx?expediente=" & expediente & "&refcata=" & Referencia_Catastral & "&vsExpedienteAcu=" & ViewState("ssExamenAcumulado") & "','cal','width=730,height=355,left=270,top=180,scrollbars=yes')"">" & expediente & "</a>"
        lblExpediente.Text = texto
    End Sub

    Private Sub Etapas()
        Dim etapas As New DatosConsultasTablas
        Me.ViewState("DatosEtapas") = etapas.Etapas
    End Sub

    Private Sub DEPENDENCIA_ACTUACIONES()
        Dim historial As New DatosConsultasTablas
        Me.ViewState("Datoshistorial") = historial.DEPENDENCIA_ACTUACIONES
    End Sub

    Private Sub Actos()
        Dim Mytb As New DataTable
        Mytb = RetornaCargadatos("select * from actuaciones")
        Me.ViewState("DatosActos") = Mytb
    End Sub
   
    Private Function ssPredio(ByVal expedientet As String) As String
        Using Command As New System.Data.SqlClient.SqlCommand("SELECT * FROM DOCUMENTOS WHERE DOCEXPEDIENTE = @DOCEXPEDIENTE", New SqlConnection(Funciones.CadenaConexion))
            Command.Parameters.Add("@DOCEXPEDIENTE", SqlDbType.VarChar).Value = expedientet
            Dim tb As New DataTable
            Command.Connection.Open()
            Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
            tb.Load(reader)
            LinkssPredioExaminarExpedientes.Text = ""
            LinkvsExpedienteAcu.Text = ""
            reader.Close()
            Command.Connection.Close()
            If tb Is Nothing Then
                Return "Predio No identificado"
            ElseIf tb.Rows.Count = 0 Then
                ViewState("vsExpedienteAcu") = Nothing
                Return "<span style='font-size:15px;font-weight:bold;'>Expedinte: </span><span style='color:#3092CB'>" & expedientet & "</span><hr />" & "Este expediente virtual no está digitalizado."
            End If
            LinkssPredioExaminarExpedientes.Text = "Aqui"
            ViewState("ssPredioExamen") = tb.Rows(0).Item("docpredio_refecatrastal")
            Dim docacumulacio As String = valorNull(tb.Rows(0).Item("docacumulacio"), "0").Trim
            If (Not ChekaNull(docacumulacio)) And (docacumulacio <> "0") Then
                If IsDBNull(tb.Rows(0).Item("docacumulacio")) Then
                    ViewState("ssExamenAcumulado") = Nothing
                Else
                    ViewState("ssExamenAcumulado") = tb.Rows(0).Item("docacumulacio")
                End If
                LinkvsExpedienteAcu.Text = "Ver Mas..."
                ViewState("vsExpedienteAcu") = "Este expediente se encuentra acumulado con <span style='font-size:12px;font-weight:bold;color:#3092CB'><strong>" & tb.Rows(0).Item("docacumulacio") & "</strong> </span>"
                If tb.Rows(0).Item("docacumulacio") = expedientet Then
                    Validator.Text = "Expediente principal..."
                    Me.Validator.IsValid = False
                Else
                    Validator.Text = "Advertencia este expediente se encuentra acumulado. Revise las consultas."
                    Me.Validator.IsValid = False
                End If
            Else
                ViewState("ssExamenAcumulado") = Nothing
                ViewState("vsExpedienteAcu") = "Expediente sin acumular"
                LinkvsExpedienteAcu.Text = ""
            End If

            Return "Para buscar los expedientes asociados al predio <span style='font-size:12px;font-weight:bold;color:#3092CB'>" & tb.Rows(0).Item("docpredio_refecatrastal") & "</span> hacer click "
        End Using
    End Function

    Private Sub expediente(ByVal idEntidad As String, ByVal expedientet As String)
        ViewState("vsPredio") = ssPredio(expedientet)
        Dim CodigoEtapa As String
        Using Command As New System.Data.SqlClient.SqlCommand("SELECT CODIGO, NOMBRE FROM ETAPAS ORDER BY CODIGO", New SqlClient.SqlConnection(Funciones.CadenaConexion))
            Command.Connection.Open()
            Dim TablaEtapas As New DataTable
            Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
            TablaEtapas.Load(reader)
            reader.Close()
            Command.Connection.Close()
            Me.contenidogrids.InnerHtml = ""
            Dim Msg As New StringBuilder
            For Each Etaparow As DataRow In TablaEtapas.Rows
                Dim EtapaOrHtmlVarios As New StringBuilder
                CodigoEtapa = Trim(Etaparow("codigo"))
                EtapaOrHtmlVarios.Append(CodigoEtapa)
                EtapaOrHtmlVarios.Append(". ")
                EtapaOrHtmlVarios.Append(Etaparow("nombre"))

                Msg.Append("<div class='contenedortitulos'><div style='float:left;'>" + EtapaOrHtmlVarios.ToString + "</div><div style='float:right;'><a href='javascript:void(0)' title='Mostrar todos los registros de esta etapa administrativa omitiendo las restricciones del secuenciador.' class='actoclicksec' rel='" & expedientet & "$" & CodigoEtapa & "'><img src='images/icons/tecno_sitemap.png' width='16' height='16' alt='Sin secuenciador' /></a></div><div style='clear:both;'></div></div>")
                Dim Logic As Ejecucion_Expediente_examinar = New Ejecucion_Expediente_examinar(New SqlClient.SqlConnection(Funciones.CadenaConexion), CodigoEtapa)
                Dim Table As DataTable = Logic.ExamenCompleto(expedientet, CodigoEtapa)
                Dim row As DataRow
                Msg.Append("<div id='content")
                Msg.Append(CodigoEtapa)
                Msg.Append("' style='margin:0;padding:0;'>")
                Msg.Append(vbNewLine)
                If Table.Rows.Count > 0 Then
                    Msg.Append("<table width=""100%"" class=""servicesT""><tr><th colspan='2'>Acto Administrativo</th><th style='text-align:left;width:100px;'>F. Radicación</th><th style='text-align:center;width:70px;'>Exp PPAL</th><th style='text-align:center;width:70px;'>Usuario</th></tr>")
                    Msg.Append(vbNewLine)
                    For Each row In Table.Rows
                        Msg.Append("<tr><td style='text-align:left;width:15px;'>")
                        Msg.Append(valorNull(row("idacto"), "&nbsp"))
                        Msg.Append("</td><td class='servHd'>")
                        Msg.Append(ArmarHREF(row))
                        Msg.Append("</td><td>")
                        Msg.Append(valorNull(Format(CDate(row("fecharadic")), "dd/MM/yyyy"), "&nbsp"))
                        Msg.Append("</td><td style='text-align:center;'>")
                        Dim Noppal As String = "<a href='javascript:void(0)' title='Este expediente no se encuentra acumulado.'>X</a>"
                        Msg.Append(IIf(valorNull(row("docacumulacio"), Noppal) = "", Noppal, row("docacumulacio")))
                        Msg.Append("</td><td style='text-align:center;'>")
                        Msg.Append(valorNull(row("docusuario"), "&nbsp"))
                        Msg.Append("</td></tr>")
                        'ultimopaso = row("idacto")
                    Next
                    Msg.Append("</table></div>")
                    'ViewState("UltimoPaso") = ultimopaso
                Else
                    Msg.Append("<table width='100%' class='servicesT'><tr><td class='EservEHd'>NO EXISTEN ACTOS ADMINISTRATIVOS ASOCIADOS A ESTA <b>ETAPA</b>...</td></tr></table></div>")
                End If
            Next
            Me.contenidogrids.InnerHtml = Msg.ToString
        End Using
        Dim tipo As String = Request("tipo")
    End Sub

    Private Function ArmarHREF(ByVal row As DataRow) As String
        Dim reto As String = "<a title = '" & row("nombreacto") & "' href=""javascript:void(0)""  onclick=""window.open('TiffViewer.aspx?nomente=" & row("nombre") & "&idente=" & row("entidad") & "&F=" & row("nomarchivo") & "&totimg=" & row("paginas") & "&acto=" & row("nombreacto") & "&idacto=" & row("idacto") & "&folder=&Enabled=false&observacion=" & row("DOCOBSERVACIONES") & "&vsExpedienteAcu=', '', 'fullscreen=yes, scrollbars=auto')"">" & row("nombreacto") & "</a>"
        Return reto
    End Function

    Private Sub nuevoProcesoExpediente()
        examinar.InnerHtml = ""
    End Sub

    Protected Sub btnSi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSi.Click
        Dim idEntidad As String
        idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
        Dim expedientet As String
        expedientet = ListExpedientes.SelectedValue
        expediente(idEntidad, expedientet)
        examenExpediente(expedientet, "")
    End Sub

    Private Sub cancelartn(ByVal index As String)
        If index = 0 Then

        End If
    End Sub

#Region "Siguinet paso"
    Private Sub LinksiguentePasoAdministrativo2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinksiguentePasoAdministrativo2.Click
        LinksiguentePasoAdministrativo_Click(Nothing, Nothing)
    End Sub

    Private Sub LinksiguentePasoAdministrativo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinksiguentePasoAdministrativo.Click
        Try
            Dim TableUltimo As New DataTable
            Using Command As New System.Data.SqlClient.SqlCommand("SELECT * FROM DOCUMENTO_ULTIMOACTO WHERE ULT_EXPEDIENTE = @DOCEXPEDIENTE", New SqlConnection(Funciones.CadenaConexion))
                Command.Parameters.Add("@DOCEXPEDIENTE", SqlDbType.VarChar).Value = Request("expediente")
                Command.Connection.Open()
                Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                TableUltimo.Load(reader)
                reader.Close()
                Command.Connection.Close()
            End Using

            Dim ultomo As String = Nothing
            If TableUltimo.Rows.Count > 0 Then
                ultomo = TableUltimo.Rows(0).Item("ULT_ACTO")
            End If

            If ultomo <> Nothing Then
                lblxcExpediente.InnerHtml = Request("expediente")
                lblxcdeudor.InnerHtml = ejDeudor.Text + " - " + ejdeuNombre.Text

                Dim sql As String = "SELECT (DEP_CODACTO + '-' + DEP_NOMBREPPAL) ULTIMO,DEP_DEPENDENCIA,DEP_DESCRIPCION,DEP_TERMINO FROM DEPENDENCIA_ACTUACIONES WHERE DEP_CODACTO = @utilpas"
                Dim myadapter As New SqlClient.SqlDataAdapter(sql, Funciones.CadenaConexion)
                myadapter.SelectCommand.Parameters.Add("@utilpas", SqlDbType.VarChar).Value = ultomo
                Dim mytb As New DataTable
                myadapter.Fill(mytb)

                If mytb.Rows.Count > 0 Then
                    ActoAdmind.InnerHtml = mytb.Rows(0).Item("ULTIMO")
                    dtgViewActos.DataSource = mytb
                    dtgViewActos.DataBind()

                    Me.ViewState("datos") = CType(mytb, DataTable)
                    Me.ModalPopupExtender2.Show()
                Else
                    Validator.Text = "<span style='color:#ffff7f;'>Atención :</span> No existe un secuenciador para este acto administrativo <i>(Llamar administrador del sistema)</i>."
                    Me.Validator.IsValid = False
                End If
            Else
                Validator.Text = "<span style='color:#ffff7f;'>Atención :</span> En la configuraciones de este expediente no se detecto el último acto administrativo ejecutado <i>(Llamar administrador del sistema)</i>."
                Me.Validator.IsValid = False
            End If
        Catch ex As Exception
            Validator.Text = "Error : " & ex.Message
            Me.Validator.IsValid = False
        End Try
    End Sub

    Protected Sub dtgViewActos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgViewActos.SelectedIndexChanged
        With Me
            Dim Mytb As DataTable = CType(.ViewState("datos"), DataTable)
            Dim index As Integer = dtgViewActos.SelectedIndex
            Response.Redirect("SiguientePaso.aspx?expediente=" & ejExpediente.Text & "&refcatastral=" & Request("refcatastral") & "&tipo=1&cedula=" & ejDeudor.Text & "&deunom=" & ejdeuNombre.Text & "&utilpas=" & Mytb.Rows(index).Item("ULTIMO") & "&pasoselect=" & Mytb.Rows(index).Item("DEP_DEPENDENCIA") & "&descripselect=" & Mytb.Rows(index).Item("DEP_DESCRIPCION"))
        End With
    End Sub
#End Region

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        txtEnte.Text = ""
        txtEnte.Focus()
    End Sub

    Protected Sub txtEnte_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEnte.TextChanged
        Call ejecutar_expediente()
    End Sub

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("maestro_entesdbf.aspx?cedula=" & Request("cedula").Trim & "&expediente=" & Request("expediente").Trim & "&deunom=" & Request("deunom").Trim & "&tipo=1")
    End Sub

    Private Sub LinkvsExpedienteAcu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkvsExpedienteAcu.Click
        examenExpediente(ViewState("ssExamenAcumulado"), "")
        expediente("", ViewState("ssExamenAcumulado"))
    End Sub

    Private Sub LinkssPredioExaminarExpedientes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkssPredioExaminarExpedientes.Click
        ExpedientesPredio(ViewState("ssPredioExamen"))
    End Sub

    Private Sub btnEnviar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnviar.Click
        Using con As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Using Command As New SqlClient.SqlCommand()
                con.Open()
                Command.Connection = con
                Command.CommandText = "SELECT DOCEXPEDIENTE,EFINIT AS ENTIDAD,EFINOM AS NOMBRE,IDACTO, FECHARADIC, NOMARCHIVO, DOCFECHADOC,PAGINAS,DOCOBSERVACIONES FROM DOCUMENTOS,EJEFISGLOBAL WHERE DOCEXPEDIENTE = @EXPEDIENTE AND EFINIT = ENTIDAD"
                Command.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = txtbuscaexpedientesimple.Text
                Dim myadapter As New SqlDataAdapter(Command)
                Dim tb As New DataTable
                myadapter.Fill(tb)

                If txtbuscaexpedientesimple.Text <> Nothing Then
                    If tb.Rows.Count Then
                        txtEnte.Text = tb.Rows(0).Item("NOMBRE").toupper.trim & "::" & tb.Rows(0).Item("ENTIDAD").trim

                        examenExpediente(txtbuscaexpedientesimple.Text, "")
                        expediente("", txtbuscaexpedientesimple.Text)
                        txtbuscaexpedientesimple.Text = ""

                        Validator.Text = ("Expediente " & txtbuscaexpedientesimple.Text & " Detectado").ToUpper
                        Me.Validator.IsValid = False
                    Else
                        Validator.Text = "No se detectaron expedientes."
                        Me.Validator.IsValid = False
                    End If
                Else
                    Validator.Text = "Digite un expedientes."
                    Me.Validator.IsValid = False
                End If
            End Using
        End Using
    End Sub

End Class