Imports System.Data.SqlClient

Partial Public Class ejecucionesFiscales
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub
    Private Sub ValidateUserExp()
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#validateUser').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel()"
            menssageError(amsbgox)
            Exit Sub
        End If

        If Not IsPostBack Then
            Page.Form.Attributes.Add("autocomplete", "off")
            ActoImpuesto.InnerHtml = Session("ssCodimpadm")
            'Busqueda por Predio
            ViewState("opcamp") = 1

            Dim tipo As String = Request("tipo")
            If tipo <> Nothing Then
                If tipo = 1 Then
                    Dim opconsul As String = Request("opconsul")
                    If opconsul = "1" Then
                        'Deudor
                        ViewState("opcamp") = 1
                        txtEnte.Text = Request("deunom").Trim & "::" & Request("cedula").Trim
                        titulocap.InnerHtml = "En este momento está buscando por <b>deudor</b>."
                    ElseIf opconsul = "2" Then
                        'Predio
                        ViewState("opcamp") = 2
                        txtEnte.Text = Request("deunom").Trim & "::" & Request("refcatastral")
                        titulocap.InnerHtml = "En este momento está buscando por <b>" & GridView_datos.Columns(1).HeaderText & "</b>."
                    ElseIf opconsul = "3" Then
                        'Expediente
                        ViewState("opcamp") = 3
                        txtEnte.Text = Request("expediente").Trim
                        titulocap.InnerHtml = "En este momento está buscando por <b>expediente</b>."
                    ElseIf opconsul = "4" Then
                        'Acto
                        ViewState("opcamp") = 4
                        'txtEnte.Text = Request("expediente")
                        titulocap.InnerHtml = "En este momento está buscando por <b>acto administrativo</b>."
                    End If

                    btnConsultar_Click(Nothing, Nothing)
                End If
            Else
                Dim Mytb As New DataTable
                Dim sqlString As String = "SELECT EFINROEXP ,B.ED_Nombre, EFIFECHAEXP ,EFINUMMEMO ,EFIEXPORIGEN ,EFIFECCAD ,EFINIT ,EFIPAGOSCAP ,EFISALDOCAP ,EFINROTITULO ,EFIUSUASIG ,EFIDEVUELTO ,EFIDIFICILCOBRO ,EFICODESTEXP ,EFIESTUP,EFIULTPAS FROM EJEFISGLOBAL A, ENTES_DEUDORES B WHERE A.EFINIT = B.ED_Codigo_Nit AND EfiModCod = " & Session("ssimpuesto") & ""
                Dim MyAdapter As New SqlDataAdapter(sqlString, Funciones.CadenaConexion)
                MyAdapter.Fill(Mytb)
                Me.ViewState("datos") = Mytb
                cargar_datos(Mytb)
                GridView_datos.DataSource = Mytb
                GridView_datos.DataBind()
            End If
        End If
    End Sub

    Private Sub Autocomplete(ByVal ServicePatch As String, ByVal ServiceMethod As String)
        'titulocap
        AutoCompleteExtender1.ServicePath = ServicePatch
        AutoCompleteExtender1.ServiceMethod = ServiceMethod
        txtEnte.Text = ""
        txtEnte.Focus()
        btnDeuda.Enabled = False
    End Sub

    Private Sub LinkBuDeudor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkBuDeudor.Click
        Autocomplete("Servicios/Autocomplete.asmx", "ObtListaEtidades_Est")
        btnDeuda.Enabled = True
        'opcamp.SelectedIndex = 0
        'opcamp.Value = 1
        ViewState("opcamp") = 1
        titulocap.InnerHtml = "En este momento está buscando por <b>deudor</b>."
    End Sub

    Private Sub LinkExpedienteBus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkExpedienteBus.Click
        'opcamp.SelectedIndex = 2
        'opcamp.Value = 3
        ViewState("opcamp") = 3
        Autocomplete("Servicios/Autocomplete.asmx", "ObtListaEtidades_Expediente")
        titulocap.InnerHtml = "En este momento está buscando por <b>expediente</b>."
    End Sub

    Private Sub LinkBusActuaciones_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkBusActuaciones.Click
        Autocomplete("Servicios/Autocomplete.asmx", "ObtListaActuaciones")
        'opcamp.SelectedIndex = 3
        'opcamp.Value = 4
        ViewState("opcamp") = 4
        titulocap.InnerHtml = "En este momento está buscando por <b>acto administrativo</b>."
    End Sub

    Protected Sub btnHistorial_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHistorial.Click
        If Session("UsuarioValido") Is Nothing Then
            Call sessioncau()
            Exit Sub
        End If

        Try
            Dim index As Integer = GridView_datos.SelectedIndex
            If index < 0 Then
                messagesof.InnerHtml = "Por favor elija un expediente "
                Exit Sub
            End If
            Dim Mytb As DataTable = CType(ViewState("datos"), DataTable)
            If Not validarExpedientes(Mytb.Rows(index).Item("EfiNroExp").ToString) And Session("mnivelacces") <> 1 Then
                ValidateUserExp()
            Else
                Response.Redirect("historiaexpediente.aspx?expediente=" & Mytb.Rows(index).Item("EfiNroExp") & "&tipo=1&cedula=" & Mytb.Rows(index).Item("EfiNit") & "&deunom=" & Mytb.Rows(index).Item("ED_Nombre") & "&utilpas=" & Mytb.Rows(index).Item("EfiUltPas") & "&opconsul=" & opcamp.Value & "&ejecuciones=1")
            End If

        Catch ex As Exception
            messagesof.InnerHtml = "Error: " & ex.Message
        End Try
    End Sub

    Protected Sub btnDeuda_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeuda.Click
        If Session("UsuarioValido") Is Nothing Then
            Call sessioncau()
            Exit Sub
        End If

        Dim idEntidadx() As String = Split(Me.txtEnte.Text, "::")
        If idEntidadx.Length = 2 Then
            Dim deunom, cedula As String
            cedula = CType(idEntidadx(1), String)
            deunom = CType(idEntidadx(0), String)
            Response.Redirect("HistoricoDeuda.aspx?cedula=" & cedula & "&deunom=" & deunom & "&tipo=1", True)
        Else
            messagesof.InnerHtml = "Error: Favor digitar la cedula o nombre para continuar."
        End If
    End Sub

#Region "cuadro por valor"
    Protected Sub btnSiConsulDeuda_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSiConsulDeuda.Click
        Dim sqlString As String = ""
        If Val(TextBoxDeuda.Text) > 0 And IsNumeric(TextBoxDeuda.Text) Then
            If txtEnte.Text <> Nothing Then
                Dim idEntidadx As String
                idEntidadx = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)

                If opcamp.Value = 1 Then
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EfiNit] = '" & idEntidadx & "') AND (Efiest = 0) group by [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas], [EfiNroExp], [EfiValDeu], [EfiValInt], [EfiNom], [EfiNit] having ([EfiValDeu] + [EfiValInt]) >= " & Val(TextBoxDeuda.Text)
                ElseIf opcamp.Value = 2 Then
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EFIGEN] = '" & idEntidadx & "') AND (Efiest = 0) group by [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas], [EfiNroExp], [EfiValDeu], [EfiValInt], [EfiNom], [EfiNit] having ([EfiValDeu] + [EfiValInt]) >= " & Val(TextBoxDeuda.Text)
                ElseIf opcamp.Value = 3 Then
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EfiNroExp] = '" & idEntidadx & "') AND (Efiest = 0) group by [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas], [EfiNroExp], [EfiValDeu], [EfiValInt], [EfiNom], [EfiNit] having ([EfiValDeu] + [EfiValInt]) >= " & Val(TextBoxDeuda.Text)
                ElseIf opcamp.Value = 4 Then
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EfiUltPas] = '" & idEntidadx & "') AND (Efiest = 0) group by [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas], [EfiNroExp], [EfiValDeu], [EfiValInt], [EfiNom], [EfiNit] having ([EfiValDeu] + [EfiValInt]) >= " & Val(TextBoxDeuda.Text)
                End If
            Else
                If Val(TextBoxDeuda.Text) = 0 Then
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE AND (Efiest = 0) Group by [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas], [EfiNroExp], [EfiValDeu], [EfiValInt], [EfiNom], [EfiNit] having ([EfiValDeu] + [EfiValInt]) >= 500"
                Else
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE AND (Efiest = 0) Group by [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas], [EfiNroExp], [EfiValDeu], [EfiValInt], [EfiNom], [EfiNit] having ([EfiValDeu] + [EfiValInt]) >= " & Val(TextBoxDeuda.Text)
                End If
            End If
        Else
            Messenger1.InnerHtml = "<b>Digite un valor valido para continuar.</b>"
            Exit Sub
        End If

        Dim Mytb As New DataTable
        Dim MyAdapter As New SqlDataAdapter(sqlString, Funciones.CadenaConexion)
        MyAdapter.Fill(Mytb)
        Me.ViewState("datos") = Mytb
        cargar_datos(Mytb)
        GridView_datos.DataSource = Mytb
        GridView_datos.DataBind()
    End Sub
#End Region


    Private Function alexpe() As Boolean
        ListExpedientes.Items.Clear()
        Dim idEntidad As String
        idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
        Dim cmd As String
        Dim cnn As String = Funciones.CadenaConexion

        cmd = "SELECT DISTINCT EfiNit FROM ejefisglobal WHERE EfiNit = @cedula"
        Dim MyAdapter As New SqlClient.SqlDataAdapter(cmd, cnn)
        MyAdapter.SelectCommand.Parameters.Add("@cedula", SqlDbType.VarChar)
        MyAdapter.SelectCommand.Parameters("@cedula").Value = idEntidad
        Dim myTable As New DataTable
        MyAdapter.Fill(myTable)

        If myTable.Rows.Count > 1 Then
            pnlSeleccionarDatos.Visible = True

            Dim xt As Integer
            For xt = 0 To myTable.Rows.Count - 1
                ListExpedientes.Items.Add(myTable.Rows(xt).Item("EFIGEN"))
            Next

            Me.ModalPopupExtender1.Show()
            Return True
        ElseIf myTable.Rows.Count > 0 Then
            Dim Mytb As New DataTable
            Dim sqlString As String = "SELECT EFINROEXP ,B.ED_Nombre, EFIFECHAEXP ,EFINUMMEMO ,EFIEXPORIGEN ,EFIFECCAD ,EFINIT ,EFIPAGOSCAP ,EFISALDOCAP ,EFINROTITULO ,EFIUSUASIG ,EFIDEVUELTO ,EFIDIFICILCOBRO ,EFICODESTEXP ,EFIESTUP FROM EJEFISGLOBAL A, ENTES_DEUDORES B WHERE A.EFINIT = B.ED_Codigo_Nit AND ([EFINIT] = '" & myTable.Rows(0).Item("EFINIT") & "') AND (EfiModCod = " & Session("ssimpuesto") & ")"
            Dim MyAdapter_d As New SqlDataAdapter(sqlString, Funciones.CadenaConexion)
            MyAdapter_d.Fill(Mytb)
            Me.ViewState("datos") = Mytb
            cargar_datos(Mytb)
            GridView_datos.DataSource = Mytb
            GridView_datos.DataBind()
            Return True
        Else
            Messenger1.InnerHtml = UCase("<b>No se detectaron procesos, inténtelo otra vez</b>")
            Return False
        End If
    End Function

    Protected Sub btnSi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSi.Click
        Dim Mytb As New DataTable
        Dim sqlString As String

        If chktodopanelPredio.Checked = True Then
            Dim x As Integer
            Dim comulador As String = "([EFIGEN]='"
            For x = 0 To ListExpedientes.Items.Count - 1
                comulador = comulador + ListExpedientes.Items(x).Value & "' or [EFIGEN]='"
            Next
            comulador = Mid(comulador, 1, comulador.Length - 14) & ")"
            sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE  " & comulador & " AND (Efiest = 0) AND (EfiModCod = " & Session("ssimpuesto") & ")"
        Else
            sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EFIGEN] = '" & ListExpedientes.SelectedValue & "') AND (Efiest = 0) AND (EfiModCod = " & Session("ssimpuesto") & ")"
        End If
        Dim MyAdapter_d As New SqlDataAdapter(sqlString, Funciones.CadenaConexion)
        MyAdapter_d.Fill(Mytb)
        Me.ViewState("datos") = Mytb
        cargar_datos(Mytb)
        GridView_datos.DataSource = Mytb
        GridView_datos.DataBind()
    End Sub

    Private Sub sessioncau()
        Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
         & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
         & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

        ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel()"
        menssageError(amsbgox)
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultar.Click
        If Session("UsuarioValido") Is Nothing Then
            Call sessioncau()
            Exit Sub
        End If

        Dim Mytb As New DataTable
        Dim sqlString As String = ""

        Try
            If txtEnte.Text <> Nothing Then
                Messenger1.InnerHtml = ""
                Dim idEntidad As String
                idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)

                If ViewState("opcamp") = 1 Then
                    'deudor 
                    alexpe()
                    Exit Sub
                ElseIf ViewState("opcamp") = 2 Then
                    'predio
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EFIGEN] LIKE '%" & idEntidad & "') AND (Efiest = 0) AND (EfiModCod = " & Session("ssimpuesto") & ")"
                ElseIf ViewState("opcamp") = 3 Then
                    'expediente
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EfiNroExp] = '" & Val(txtEnte.Text) & "') AND (Efiest = 0) AND (EfiModCod = " & Session("ssimpuesto") & ")"
                ElseIf ViewState("opcamp") = 4 Then
                    'ultimo acto
                    sqlString = "SELECT  [EFIGEN], [EfiDiaFin], [EfiUltPas],[EfiPerDes], [EfiPerHas] , [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total , [EfiNom], [EfiNit] FROM [EJEFISGLOBAL] WHERE ([EfiUltPas] = '" & idEntidad & "') AND (Efiest = 0) AND (EfiModCod = " & Session("ssimpuesto") & ")"
                End If

                Dim MyAdapter As New SqlDataAdapter(sqlString, Funciones.CadenaConexion)
                MyAdapter.Fill(Mytb)
                Me.ViewState("datos") = Mytb
                cargar_datos(Mytb)
                GridView_datos.DataSource = Mytb
                GridView_datos.DataBind()
            Else
                Messenger1.InnerHtml = "<b>Favor digitar deudor, expediente, acto administrativo o predio para continuar.</b>"
                txtEnte.Focus()
            End If
        Catch ex As Exception
            Messenger1.InnerHtml = "<b>" & ex.Message & "</b>"
        End Try
    End Sub

    Private Sub cargar_datos(ByVal dt As DataTable)
        'EfiValDeu
        'Dim EfiValInt, EfiValDeu As Object
        'EfiValInt = dt.Compute("sum(EfiValInt)", "EfiValInt > 0")
        'EfiValDeu = dt.Compute("sum(EfiValDeu)", "EfiValDeu > 0")

        'lbldeuda.InnerHtml = FormatCurrency(valorNull(EfiValDeu), 2)
        'lblInteres.InnerHtml = FormatCurrency(valorNull(EfiValInt), 2)
        'lbltotal.InnerHtml = FormatCurrency(Val(valorNull(EfiValInt)) + Val(valorNull(EfiValDeu)), 2)

        If dt.Rows.Count = 0 Then
            Messenger1.InnerHtml = UCase("<b>No se detectaron procesos, inténtelo otra vez</b>")
            detalle.InnerHtml = UCase("No se detectaron procesos, inténtelo otra vez")
        ElseIf dt.Rows.Count = 1 Then
            detalle.InnerHtml = UCase("Se detectaron " & Num2Text(dt.Rows.Count) & " Proceso.")
        Else
            detalle.InnerHtml = UCase("Se detectaron " & Num2Text(dt.Rows.Count) & " Procesos.")
            Messenger1.InnerHtml = ""
        End If
    End Sub

    Protected Sub GridView_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView_datos.SelectedIndexChanged
        With Me
            Dim Mytb As DataTable = CType(.ViewState("datos"), DataTable)
            Dim index As Integer = GridView_datos.SelectedIndex
            messagesof.InnerHtml = "Expediente seleccionado " & Mytb.Rows(index).Item("EfiNroExp")

            'lblInteres.InnerHtml = FormatCurrency(Mytb.Rows(index).Item("EfiValInt"), 2)
            'lbltotal.InnerHtml = FormatCurrency(Val(Mytb.Rows(index).Item("EfiValDeu")) + Val(Mytb.Rows(index).Item("EfiValInt")), 2)
            'Debug.WriteLine("My second error message.")
        End With
    End Sub

    Protected Sub btnracExpediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnracExpediente.Click
        Response.Redirect("deudoresactivos.aspx")
    End Sub

    Protected Sub btnconsultaracum_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnconsultaracum.Click
        Response.Redirect("consultaracum.aspx")
    End Sub

    Protected Sub btnCobroIndividual_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCobroIndividual.Click
        If Session("UsuarioValido") Is Nothing Then
            Call sessioncau()
            Exit Sub
        End If

        Try
            Dim index As Integer = GridView_datos.SelectedIndex
            If index < 0 Then
                messagesof.InnerHtml = "Por favor elija un expediente "
                Exit Sub
            End If
            Dim Mytb As DataTable = CType(ViewState("datos"), DataTable)

            Response.Redirect("cobranzas2.aspx?expediente=" & Mytb.Rows(index).Item("EfiNroExp") & "&refcatastral=" & Mytb.Rows(index).Item("EfiNit") & "&tipo=1&cedula=" & Mytb.Rows(index).Item("ED_Nombre") & "::" & Mytb.Rows(index).Item("EfiNit") & "&deunom=" & Mytb.Rows(index).Item("ED_Nombre") & "&utilpas=" & Mytb.Rows(index).Item("EfiUltPas") & "&opconsul=" & opcamp.Value & "&ejecuciones=1&etapa=02")
        Catch ex As Exception
            messagesof.InnerHtml = "Error: " & ex.Message
        End Try
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
End Class
