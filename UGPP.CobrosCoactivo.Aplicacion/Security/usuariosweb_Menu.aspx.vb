Imports System.Data.SqlClient
Partial Public Class usuariosweb_Menu
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Private Sub CargarPPALMENU()
        Dim cmd As String = "SELECT * FROM PPALMENU"
        Dim myAdapter As New SqlDataAdapter(cmd, Session("ConexionServer").ToString)
        Dim mytable As New DataTable
        myAdapter.Fill(mytable)
        Me.ViewState("PPALMENU") = mytable
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                Dim codigo As String = Request("select").ToString
                Dim myclass_ As New DatosConsultasTablas
                Dim mytable As New DataTable
                myclass_.consultarUsuario(codigo, Trim(Session("mcobrador").ToString), mytable)

                If mytable.Rows.Count > 0 Then
                    lblcodigo.Text = codigo
                    lblNombre.Text = mytable.Rows(0).Item("nombre").ToString.Trim
                    lblcedula.Text = mytable.Rows(0).Item("documento").ToString.Trim

                    Dim detalle As String = ""
                    If mytable.Rows(0).Item("nivelacces").ToString.Trim = "1" Then
                        detalle = "1 (Administrador)"
                    ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "2" Then
                        detalle = "2 (Operador sin permiso de actualizaci&#243;n)"
                    ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "3" Then
                        detalle = "3 (Operador sin permiso de actualizaci&#243;n y de visualizaci&#243;n de documentos"
                    End If

                    lbldetalle.Text = detalle
                    Call CargarPPALMENU()
                    Call CargarPermisos(codigo.ToString)
                Else
                    Dim amsbgox As String = "<h2 class='err'>ERROR</h2> El usuario actual no se pudo examinar, intente salir y entrar al sistema." _
                      & "<br /><hr /><h2>ERROR TÉCNICO</h2>Seguir los pasos  adecuados para continuar."
                    menssageError(amsbgox)
                End If
            Catch ex As Exception
                Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Es posible haya espirado la sesión. Intente salir y entrar al sistema." _
                      & "<br /><hr /><h2>ERROR TÉCNICO</h2>Seguir los pasos  adecuados para continuar."
                menssageError(amsbgox)
            End Try
        End If
    End Sub

    Private Sub CargarPermisos(ByVal UserId As String)
        Try
            Dim conexion As String = Session("ConexionServer").ToString
            Dim myadapter As New SqlDataAdapter("select * from USUARIO_PPALMENU where me_usuario = '" & UCase(Trim(UserId)) & "' ORDER BY ME_CONTADORME", conexion)
            Dim mytb As New DatasetForm.USUARIO_PPALMENUDataTable
            myadapter.Fill(mytb)
            Me.ViewState("DatosUser") = mytb
            If mytb.Rows.Count > 0 Then
                chkConsultar_expedientes.Checked = valorNull(mytb.Rows(0).Item("ME_PERMISO"), False)
                chkActualizar_expedientes.Checked = valorNull(mytb.Rows(1).Item("ME_PERMISO"), False)
                chkConsulta_Diaria.Checked = valorNull(mytb.Rows(2).Item("ME_PERMISO"), False)
                chkGestion_Cobranza.Checked = valorNull(mytb.Rows(3).Item("ME_PERMISO"), False)
                chkGestion_usuario.Checked = valorNull(mytb.Rows(4).Item("ME_PERMISO"), False)
                chkCambio_clave.Checked = valorNull(mytb.Rows(5).Item("ME_PERMISO"), False)
                chkgestion_impActAdmin.Checked = valorNull(mytb.Rows(6).Item("ME_PERMISO"), False)

                chkgestion_ejecuActuac.Checked = valorNull(mytb.Rows(7).Item("ME_PERMISO"), False)
                chkgestion_salcontrires.Checked = valorNull(mytb.Rows(8).Item("ME_PERMISO"), False)
                chkGestion_datosbasicos.Checked = valorNull(mytb.Rows(9).Item("ME_PERMISO"), False)

                '-- Datos basicos
                chkGestion_meregisActos.Checked = valorNull(mytb.Rows(10).Item("ME_PERMISO"), False)
                chkGestion_meregisEtapa.Checked = valorNull(mytb.Rows(11).Item("ME_PERMISO"), False)
                chkGestion_meconfigimprimi.Checked = valorNull(mytb.Rows(12).Item("ME_PERMISO"), False)
                chkGestion_meregideu.Checked = valorNull(mytb.Rows(13).Item("ME_PERMISO"), False)
                chkGestion_mesecueacto.Checked = valorNull(mytb.Rows(14).Item("ME_PERMISO"), False)
                chkGestion_meregisEntes.Checked = valorNull(mytb.Rows(15).Item("ME_PERMISO"), False)
                chkGestion_mefestivoadd.Checked = valorNull(mytb.Rows(16).Item("ME_PERMISO"), False)
                'Predial
                chkgestion_meResolAcumulado.Checked = valorNull(mytb.Rows(17).Item("ME_PERMISO"), False)
            Else
                chkConsultar_expedientes.Checked = False
                chkActualizar_expedientes.Checked = False
                chkConsulta_Diaria.Checked = False
                chkGestion_Cobranza.Checked = False
                chkGestion_usuario.Checked = False
                chkCambio_clave.Checked = False
                chkgestion_impActAdmin.Checked = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        If Me.ViewState("DatosUser") Is Nothing Then
            Messenger.InnerHtml = "<font color='red'>Error : El usuario actual no se pudo examinar, intente salir y entrar al sistema.</font>"
        Else
            Try
                Using con As New SqlConnection(Session("ConexionServer").ToString)
                    With Me
                        Dim da As New SqlDataAdapter("select * from USUARIO_PPALMENU", con)
                        da.MissingSchemaAction = MissingSchemaAction.AddWithKey

                        'Creamos los comandos con el CommandBuilder
                        Dim cb As New SqlCommandBuilder(da)

                        da.InsertCommand = cb.GetInsertCommand()
                        da.UpdateCommand = cb.GetUpdateCommand()

                        con.Open()
                        Dim tran As SqlTransaction = con.BeginTransaction

                        da.InsertCommand.Transaction = tran
                        da.UpdateCommand.Transaction = tran

                        'Los menu existente en el aplicativo 
                        Dim ppalmenu As New DataTable
                        ppalmenu = Me.ViewState("PPALMENU")
                        Dim xt As Integer = 0

                        'Menu asignados o registrados del usuario
                        Dim Mytb As DatasetForm.USUARIO_PPALMENUDataTable = CType(.ViewState("DatosUser"), DatasetForm.USUARIO_PPALMENUDataTable)
                        Dim permiso As ArrayList = indexmenu()

                        For xt = 0 To ppalmenu.Rows.Count - 1
                            If Mytb.Select("me_usuario='" & Request("select").ToString & "' and ME_OPCIONMENU='" & ppalmenu.Rows(xt).Item("MU_MENUOPCION") & "'").Length > 0 Then
                                Dim Row As DatasetForm.USUARIO_PPALMENURow = Mytb.Select("me_usuario='" & Request("select").ToString & "' AND ME_OPCIONMENU='" & ppalmenu.Rows(xt).Item("MU_MENUOPCION") & "'")(0)
                                If Not Row Is Nothing Then
                                    Row.ME_OPCIONMENU = ppalmenu.Rows(xt).Item("MU_MENUOPCION")
                                    Row.ME_PERMISO = permiso(xt)
                                    Row.ME_OPCIONINDEX = ppalmenu.Rows(xt).Item("MU_CODIGO")
                                    Row.ME_MENU = ppalmenu.Rows(xt).Item("MU_MENU")
                                    Row.ME_DETALLEOPCION = ppalmenu.Rows(xt).Item("MU_DESCRIPCION")
                                End If
                            Else
                                'Actualizar el consecutivo
                                Mytb.AddUSUARIO_PPALMENURow(Request("select").ToString, ppalmenu.Rows(xt).Item("MU_MENUOPCION"), permiso(xt), xt, ppalmenu.Rows(xt).Item("MU_MENU"), ppalmenu.Rows(xt).Item("MU_DESCRIPCION"))
                            End If
                        Next

                        Try
                            ' Actualizamos los datos de la tabla
                            da.Update(Mytb)
                            tran.Commit()
                            Messenger.InnerHtml = "<font color='#FFFFFF'><b> Se han guardado los datos</b></font>"
                            Ejecutarjavascript("<script type=text/javascript> $(document).ready(function(){CronoID = setTimeout(""timerOper()"", 5000);}); </script>", "CancelarClient")
                            Response.Redirect("usuariosweb.aspx")
                        Catch ex As Exception
                            'Si hay error, desahacemos lo que se haya hecho
                            tran.Rollback()
                            Messenger.InnerHtml = "<font color='#8A0808'> " & ex.Message & "</font>"
                        End Try
                        con.Close()

                    End With

                End Using
            Catch ex As Exception
                Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Tecno Expedientes no  puede conectar o no tiene acceso  origen de datos configurado, puede que no tenga permiso para utilizar el recurso de datos. Si el problema persiste favor comuníquese  con el <b>administrador</b> de este servidor  para comprobar si tiene permisos de acceso.<br />" _
               & "<b>Nombre Servidor : </b>" & ConfigurationManager.AppSettings("ServerName") & "   <br /><hr /><h2>ERROR TÉCNICO</h2>"

                menssageError(amsbgox & ex.Message)
            End Try
        End If
    End Sub

    Private Function indexmenu() As ArrayList
        Dim ConExp As Boolean = chkConsultar_expedientes.Checked
        Dim ActExp As Boolean = chkActualizar_expedientes.Checked
        Dim ConDia As Boolean = chkConsulta_Diaria.Checked
        Dim OpPredial As Boolean = chkGestion_Cobranza.Checked
        Dim usuario As Boolean = chkGestion_usuario.Checked
        Dim caClave As Boolean = chkCambio_clave.Checked
        Dim impActAdmin As Boolean = chkgestion_impActAdmin.Checked
        Dim ejecuActuac As Boolean = chkgestion_ejecuActuac.Checked
        Dim salcontrires As Boolean = chkgestion_salcontrires.Checked
        Dim datosbasicos As Boolean = chkGestion_datosbasicos.Checked

        Dim meregisActos As Boolean = chkGestion_meregisActos.Checked
        Dim meregisEtapa As Boolean = chkGestion_meregisEtapa.Checked
        Dim meconfigimprimi As Boolean = chkGestion_meconfigimprimi.Checked
        Dim meregideu As Boolean = chkGestion_meregideu.Checked
        Dim mesecueacto As Boolean = chkGestion_mesecueacto.Checked
        Dim meregisEntes As Boolean = chkGestion_meregisEntes.Checked
        Dim mefestivoadd As Boolean = chkGestion_mefestivoadd.Checked
        Dim meResolAcumulado As Boolean = chkgestion_meResolAcumulado.Checked

        Dim OPCIONINDEX As New ArrayList
        'Ojo mismo orden de la base de datos...!!!

        OPCIONINDEX.Add(ConExp)
        OPCIONINDEX.Add(ActExp)
        OPCIONINDEX.Add(ConDia)
        OPCIONINDEX.Add(OpPredial)
        OPCIONINDEX.Add(usuario)
        OPCIONINDEX.Add(caClave)
        OPCIONINDEX.Add(impActAdmin)
        OPCIONINDEX.Add(ejecuActuac)
        OPCIONINDEX.Add(salcontrires)
        OPCIONINDEX.Add(datosbasicos)

        OPCIONINDEX.Add(meregisActos)
        OPCIONINDEX.Add(meregisEtapa)
        OPCIONINDEX.Add(meconfigimprimi)
        OPCIONINDEX.Add(meregideu)
        OPCIONINDEX.Add(mesecueacto)
        OPCIONINDEX.Add(meregisEntes)
        OPCIONINDEX.Add(mefestivoadd)
        OPCIONINDEX.Add(meResolAcumulado)

        Return OPCIONINDEX
    End Function
    Private Sub Ejecutarjavascript(ByVal script As String, ByVal NomScript As String)
        Dim csname1 As [String] = NomScript
        Dim cstype As Type = Me.[GetType]()

        Dim cs As ClientScriptManager = Page.ClientScript

        If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As String = script
            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())
        End If
    End Sub
End Class