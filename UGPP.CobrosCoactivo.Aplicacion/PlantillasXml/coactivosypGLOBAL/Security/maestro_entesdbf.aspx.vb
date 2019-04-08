Imports System.Data
Imports System.Data.SqlClient

Partial Public Class maestro_entesdbf
    Inherits System.Web.UI.Page


    Private Function LoadDatos(ByVal cedula As String) As DataTable
        Dim cnn As String = Session("ConexionServer").ToString
        Dim MyAdapter As New SqlDataAdapter("select * from entesdbf where codigo_nit='" & cedula.Trim & "' and  cobrador = '" & Session("mcobrador").ToString & "' order by NOMBRE asc", cnn)
        Dim Mytb As New DatasetForm.entesdbfDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function
    Private Sub cargeentesdbf(ByVal cedula As String)
        Dim Table As DatasetForm.entesdbfDataTable = LoadDatos(cedula)
        Me.ViewState("Datosetapa_acto") = Table
        'dtgentesdbf.DataSource = Table
        'dtgentesdbf.DataBind()
        'Dtal.InnerHtml = ("Se detectaron " & Num2Text(Table.Rows.Count) & " deudores").ToUpper
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Try
                Dim cobrador As String
                cobrador = Session("mcobrador").ToString
                cobradortv.InnerHtml = "Cobrador : " & cobrador & " - " & EntesCobradorPublic(cobrador)

                Dim tipo As String = Request("tipo")

                If tipo = 1 Then
                    txtCodigo.Text = Request("cedula").Trim
                    cargeentesdbf(txtCodigo.Text)
                    Call consultaDeu()
                End If
            Catch ex As Exception
                Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
            End Try
            'Call cargeentesdbf()
        End If
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Using con As New SqlConnection(Session("ConexionServer").ToString)
                With Me
                    Dim da As New SqlDataAdapter("select * from entesdbf WHERE  codigo_nit=@codigo", con)
                    da.SelectCommand.Parameters.Add("@codigo", SqlDbType.VarChar).Value = .txtCodigo.Text
                    cargeentesdbf(txtCodigo.Text.Trim)
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

                    Dim Mytb As New DatasetForm.entesdbfDataTable
                    Mytb = CType(.ViewState("Datosetapa_acto"), DatasetForm.entesdbfDataTable)

                    If Mytb Is Nothing Then
                        Mytb = New DatasetForm.entesdbfDataTable
                    End If

                    If Mytb.Select("codigo_nit='" & .txtCodigo.Text & "'").Length > 0 Then
                        'En esta parate se procede a actualizar el registro si existe 
                        Dim Row As DatasetForm.entesdbfRow = Mytb.Select("codigo_nit='" & .txtCodigo.Text & "'")(0)
                        If Not Row Is Nothing Then
                            Row.nombre = .txtNombre.Text
                            Row.codigo_nit = .txtCodigo.Text
                            Row.direccion = txtDireccion.Text
                            Row.telefono = .txtTelefono.Text
                        End If
                    Else
                        Mytb.AddentesdbfRow(txtCodigo.Text.Trim, .txtNombre.Text.Trim.ToUpper, Session("mcobrador").ToString, txtDireccion.Text.Trim.ToUpper, txtTelefono.Text.Trim.ToUpper, True)
                    End If

                    Try
                        ' Actualizamos los datos de la tabla
                        da.Update(Mytb)
                        tran.Commit()
                        'Mytb.AcceptChanges()
                        Messenger.InnerHtml = "<font color='#FFFFFF'><b> Se han guardado los datos</b></font>"

                        txtNombre.Text = ""
                        txtCodigo.Text = ""
                        txtDireccion.Text = ""
                        txtTelefono.Text = "" : txtCodigo.Focus()
                    Catch ex As Exception
                        ' Si hay error, desahacemos lo que se haya hecho
                        tran.Rollback()
                        Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
                    End Try
                    con.Close()
                End With
            End Using
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try
    End Sub
    Protected Sub txtCodigo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCodigo.TextChanged
        cargeentesdbf(txtCodigo.Text)
        Call consultaDeu()
    End Sub
    Private Sub consultaDeu()
        Dim Mytb As New DatasetForm.entesdbfDataTable
        Mytb = CType(Me.ViewState("Datosetapa_acto"), DatasetForm.entesdbfDataTable)

        If Mytb.Rows.Count > 0 Then
            txtNombre.Text = valorNull(Mytb.Rows(0).Item("nombre"), "")
            txtCodigo.Text = valorNull(Mytb.Rows(0).Item("codigo_nit"), "")
            txtDireccion.Text = valorNull(Mytb.Rows(0).Item("direccion"), "")
            txtTelefono.Text = valorNull(Mytb.Rows(0).Item("telefono"), "")
            txtNombre.Focus()
        End If
    End Sub
    Protected Sub btnSi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSi.Click
        Dim pad As String = Mid(Me.txtBuscarConsul.Text.Trim(), Me.txtBuscarConsul.Text.IndexOf(":") + 3)
        cargeentesdbf(pad)
        Call consultaDeu()
        txtBuscarConsul.Text = ""
    End Sub

    Protected Sub btnracExpediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnracExpediente.Click
        documento(txtCodigo.Text)
        Dim Mytb As DataTable = CType(Me.ViewState("documento"), DataTable)
        If Mytb.Rows.Count > 0 Then
            Divtota.InnerHtml = "<b>EXPEDIENTES DETECTADOS : " & Mytb.Rows.Count & "</b>"

            Gridexpedinete.DataSource = Mytb
            Gridexpedinete.DataBind()

            Me.ModalPopupExtender1.Show()
        Else
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>NO POSEE EXPEDIENTE DIGITALIZADOS</font>"
        End If
    End Sub

    Function documento(ByVal deudor As String) As DataTable
        Dim datata As New DataTable
        Dim myadapa As New SqlDataAdapter("select distinct (docexpediente) as docexpediente from documentos where entidad = @entidad", Funciones.CadenaConexion)
        myadapa.SelectCommand.Parameters.Add("@entidad", SqlDbType.VarChar).Value = deudor
        myadapa.Fill(datata)

        Me.ViewState("documento") = datata
        Return datata
    End Function
    Protected Sub Gridexpedinete_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Gridexpedinete.SelectedIndexChanged
        Try
            With Me
                Dim Mytb As DataTable = CType(.ViewState("documento"), DataTable)

                Dim dato As String = ""
                dato = Mytb.Rows(.Gridexpedinete.SelectedIndex).Item("docexpediente")
                Response.Redirect("historiaexpediente.aspx?cedula=" & txtCodigo.Text & "&expediente=" & dato & "&deunom=" & txtNombre.Text & "&tipo=2")
            End With
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub queExpediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles queExpediente.Click
        Dim Control As String
        Control = txtCodigo.Text

        Dim Logic As Ejecucion_Expediente_examinar = New Ejecucion_Expediente_examinar(New SqlClient.SqlConnection(Funciones.CadenaConexion), "02")
        Dim Table As DataTable = Logic.ExamenCompletocedula(Control)

        If Table.Rows.Count = 0 Then
            'No Trajo datos la aplicacion (expediente)

            Exit Sub
        End If

        Me.Repeater1.DataSource = Table
        Me.Repeater1.DataBind()
        Me.ModalPopupExtender2.Show()
    End Sub
End Class