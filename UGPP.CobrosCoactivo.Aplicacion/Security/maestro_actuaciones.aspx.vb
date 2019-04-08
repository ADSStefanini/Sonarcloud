Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Public Class maestro_actuaciones
    Inherits System.Web.UI.Page

    Dim SqlConnection1 As New SqlConnection
    Private Function LoadDatos() As DataTable
        Dim cnn As String = Funciones.CadenaConexion
        Dim MyAdapter As New SqlDataAdapter("select codigo, nombre from etapas", cnn)
        Dim Mytb As New DatasetForm.etapa_actoDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function
    Private Sub cargeetapa_acto()
        Dim Table As DatasetForm.etapa_actoDataTable = LoadDatos()
        Me.ViewState("Datosetapa_acto") = Table
        dtgetapa_acto.DataSource = Table
        dtgetapa_acto.DataBind()
    End Sub

    Private Function LoadActos() As DataTable
        Dim cnn As String = Funciones.CadenaConexion
        Dim MyAdapter As New SqlDataAdapter("select * from actuaciones where codigo <> '000'", cnn)
        Dim Mytb As New DatasetForm.actuacionesDataTable
        MyAdapter.Fill(Mytb)
        Dtal.InnerHtml = ("Se detectaron " & Num2Text(Mytb.Rows.Count) & " (" & Mytb.Rows.Count & ") Actuaciones").ToUpper
        Return Mytb
    End Function
    Private Sub cargeetapa_actoCreados()
        Dim Table As DatasetForm.actuacionesDataTable = LoadActos()
        Me.ViewState("Datosetapa_actoCreados") = Table
        dtgetapa_actoCreados.DataSource = Table
        dtgetapa_actoCreados.DataBind()
        Me.ViewState("todos") = 1
    End Sub
    Private Sub ultimo()
        Dim myadapter As New SqlDataAdapter("select * from MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 2", Funciones.CadenaConexion)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1
        txtCodigo.Text = Format(Con, "000")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Try
                If Session("ConexionServer") = Nothing Then
                    Session("ConexionServer") = Funciones.CadenaConexion 
                End If

                cargeetapa_acto()
                If Request.QueryString("codigo") = Nothing Then
                    dtgetapa_acto.SelectedIndex = 0
                Else
                    dtgetapa_acto.SelectedIndex = CType(Request("codigo"), Integer)
                End If

                Dim Mytb As DatasetForm.etapa_actoDataTable = CType(ViewState("Datosetapa_acto"), DatasetForm.etapa_actoDataTable)
                With Mytb.Item(dtgetapa_acto.SelectedIndex)
                    Etapa.InnerHtml = "<span class=""wsrt""><b>" & .codigo & "</b></span>"
                    DivEtapa.InnerHtml = .nombre

                    ViewState("Codigo") = .codigo
                End With

                cargeetapa_actoCreados()
                Call ultimo()
            Catch ex As Exception
                Messenger.InnerHtml = "<font color='#8A0808' >Error :" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
            End Try
        End If
    End Sub
    Private Sub dtgetapa_acto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgetapa_acto.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.etapa_actoDataTable = CType(.ViewState("Datosetapa_acto"), DatasetForm.etapa_actoDataTable)
            With Mytb.Item(.dtgetapa_acto.SelectedIndex)
                Try
                    Etapa.InnerHtml = "<span class=""wsrt""><b>" & .codigo & "</b></span>"
                    DivEtapa.InnerHtml = .nombre
                    ViewState("Codigo") = .codigo
                Catch ex As Exception
                    Messenger.InnerHtml = "<font color='red'>Error :" & ex.Message & "</font>"
                End Try
            End With
        End With

        Try
            If RadioButton.Items(1).Selected = True Then
                Dim cnn As String = Funciones.CadenaConexion
                Dim MyAdapter As New SqlDataAdapter("select * from dbo.actuaciones where codigo <> '000' and idetapa = '" & ViewState("Codigo") & "'", cnn)
                Dim Mytb As New DatasetForm.actuacionesDataTable
                MyAdapter.Fill(Mytb)

                Me.ViewState("Datosetapa_actoCreados") = Mytb
                dtgetapa_actoCreados.DataSource = Mytb
                dtgetapa_actoCreados.DataBind()
                ViewState("todos") = 0
                Dtal.InnerHtml = ("Se detectaron " & Num2Text(Mytb.Rows.Count) & " (" & Mytb.Rows.Count & ") Actuaciones").ToUpper
            Else
                If ViewState("todos") = 0 Then
                    Call cargeetapa_actoCreados()
                End If
            End If
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808' >Error :" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
        End Try
    End Sub
    Protected Sub dtgetapa_actoCreados_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgetapa_actoCreados.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.actuacionesDataTable = CType(.ViewState("Datosetapa_actoCreados"), DatasetForm.actuacionesDataTable)
            With Mytb.Item(.dtgetapa_actoCreados.SelectedIndex)
                Try
                    txtCodigo.Text = .codigo
                    txtNombre.Text = .nombre.TrimEnd
                    lblDetalle.Text = "EDITANDO ACTUACION " & .codigo.ToString & "::" & .nombre.ToString
                    ChkMasivos.Checked = .actMasivo
                    ChkHistorial.Checked = .historial
                    'ViewState("update") = 1
                Catch ex As Exception
                    Messenger.InnerHtml = "<font color='red'>Error :" & ex.Message & "</font>"
                End Try
            End With
        End With
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If txtNombre.Text = Nothing Then
                Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>Digite el nombre del acto para continuar</font>"
                Exit Sub
            End If

            Dim con As New SqlConnection(Funciones.CadenaConexion)

            With Me
                Dim proximo_numero As String = "000"
                Dim da As New SqlDataAdapter("select * from actuaciones", con)
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                ' Creamos los comandos con el CommandBuilder
                Dim cb As New SqlCommandBuilder(da)

                da.InsertCommand = cb.GetInsertCommand()
                da.UpdateCommand = cb.GetUpdateCommand()
                'da.DeleteCommand = cb.GetDeleteCommand()

                con.Open()
                Dim tran As SqlTransaction = con.BeginTransaction

                da.InsertCommand.Transaction = tran
                da.UpdateCommand.Transaction = tran
                'da.DeleteCommand.Transaction = tran

                Dim LogProc As New LogProcesos

                Dim Mytb As DatasetForm.actuacionesDataTable = CType(.ViewState("Datosetapa_actoCreados"), DatasetForm.actuacionesDataTable)

                If Mytb.Select("CODIGO='" & .txtCodigo.Text & "'").Length > 0 Then
                    'En esta parate se procede a actualizar el registro si existe 
                    Dim Row As DatasetForm.actuacionesRow = Mytb.Select("CODIGO='" & .txtCodigo.Text & "'")(0)
                    If Not Row Is Nothing Then
                        Row.idetapa = ViewState("Codigo").ToString.Trim
                        Row.nombre = .txtNombre.Text
                        Row.historial = ChkHistorial.Checked
                        Row.fechasys = Now()
                    End If
                Else
                    'en esta parte se ingresa un registro nuevo
                    'Actualizar el consecutivo
                    Dim mycommand As New SqlCommand("update MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = 2", con)
                    mycommand.Parameters.Add("@proximo_numero", SqlDbType.Int)
                    mycommand.Parameters("@proximo_numero").Direction = ParameterDirection.Output
                    mycommand.Transaction = tran
                    mycommand.ExecuteNonQuery()

                    '----------------
                    'Después de cada GRABAR hay que llamar al log de auditoria                    
                    LogProc.SaveLog(Session("ssloginusuario"), "Actualización de consecutivos", "CON_IDENTIFICADOR = 2 ", mycommand)
                    '----------------

                    Dim conse As Integer = CType(mycommand.Parameters("@proximo_numero").Value, Integer)
                    proximo_numero = Format(conse, "000")
                    Mytb.AddactuacionesRow(proximo_numero.Trim, .txtNombre.Text.Trim.ToUpper, ViewState("Codigo").ToString.Trim, 0, 0, Nothing, Nothing, Nothing, ChkHistorial.Checked, 0, ChkMasivos.Checked, Now())


                End If



                Try
                    ' Add handler
                    AddHandler da.RowUpdating, New SqlRowUpdatingEventHandler(AddressOf da_RowUpdating)

                    ' Actualizamos los datos de la tabla
                    da.Update(Mytb)
                    tran.Commit()
                    'Mytb.AcceptChanges()

                    ' Remove handler
                    RemoveHandler da.RowUpdating, New SqlRowUpdatingEventHandler(AddressOf da_RowUpdating)

                    'Después de cada GRABAR hay que llamar al log de auditoria    
                    If TipoComandoUsado = "INSERT" Then
                        LogProc.SaveLog(Session("ssloginusuario"), "Entrada de actuaciones", "codigo, nombre", da.InsertCommand)
                    Else
                        LogProc.SaveLog(Session("ssloginusuario"), "Entrada de actuaciones", "codigo, nombre", da.UpdateCommand)
                    End If

                    Messenger.InnerHtml = "<font color='#FFFFFF'><b> Se han guardado los datos</b></font>"

                    Call cargeetapa_actoCreados()
                    Call ultimo()
                    txtNombre.Text = "" : txtNombre.Focus() : lblDetalle.Text = ""
                Catch ex As Exception
                    ' Si hay error, desahacemos lo que se haya hecho
                    tran.Rollback()
                    Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
                End Try
                con.Close()
            End With

        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try
    End Sub

    Private Shared Sub da_RowUpdating(ByVal sender As Object, ByVal e As SqlRowUpdatingEventArgs)
        If e.Command IsNot Nothing Then
            TipoComandoUsado = e.StatementType.ToString.ToUpper
        End If
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Call cargeetapa_actoCreados()
        Call ultimo()
        Call cancelar()
    End Sub
    Private Sub cancelar()
        txtNombre.Text = ""
        lblDetalle.Text = ""
        txtBuscar.Text = ""
        txtNombre.Focus()
    End Sub
    Protected Sub btnRefrescar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefrescar.Click
        call cargeetapa_actoCreados()
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        Dim codigo As String
        codigo = Mid(Me.txtBuscar.Text.Trim(), Me.txtBuscar.Text.IndexOf(":") + 3)

        Dim cnn As String = Funciones.CadenaConexion
        Dim MyAdapter As New SqlDataAdapter("select * from actuaciones where codigo <> '000' and idetapa ='" & codigo & "'", cnn)
        Dim Mytb As New DatasetForm.actuacionesDataTable
        MyAdapter.Fill(Mytb)

        dtgetapa_actoCreados.DataSource = Mytb
        dtgetapa_actoCreados.DataBind()

        Me.ViewState("Datosetapa_actoCreados") = Mytb
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Dim cr As New rptActos_Administrativos
        Dim DtsDatos As DatasetForm = New DatasetForm
        Dim DatosConsultasTablas As New DatosConsultasTablas
        DatosConsultasTablas.Load_Configuracion(Session("mcobrador"), DtsDatos.CAT_CLIENTES)
        DatosConsultasTablas.Tipear_Tabla(Load_Reporte, DtsDatos.ACTOS_ADMINISTRATIVOS)

        Dim List As New List(Of ItemParams)
        For Each Par As CrystalDecisions.Shared.ParameterField In cr.ParameterFields
            Select Case Par.Name
                Case "EnteCobrador"
                    List.Add(New ItemParams("EnteCobrador", Session("mnombcobrador")))
                Case Else
                    List.Add(New ItemParams(Par.Name, "Sin Valor"))
            End Select
        Next

        Funciones.Exportar(Me, cr, DtsDatos, "Impresion.Pdf", "", List)
    End Sub

    Function Load_Reporte() As DatasetForm.ACTOS_ADMINISTRATIVOSDataTable
        Dim cnn As String = Funciones.CadenaConexion
        Using MyAdapter As New SqlDataAdapter("SELECT A.CODIGO,A.NOMBRE,A.IDETAPA,B.NOMBRE AS NOMBREETAPA FROM ACTUACIONES A INNER JOIN ETAPAS B ON A.IDETAPA = B.CODIGO AND A.CODIGO <> '000' ORDER BY A.CODIGO ASC", cnn)
            Using Mytb As New DatasetForm.ACTOS_ADMINISTRATIVOSDataTable
                MyAdapter.Fill(Mytb)
                Return Mytb
            End Using
        End Using
    End Function

End Class