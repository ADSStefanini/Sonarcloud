Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Public Class maestro_etapa
    Inherits System.Web.UI.Page

    Private Function LoadDatos() As DataTable
        Dim cnn As String = Session("ConexionServer").ToString
        Dim MyAdapter As New SqlDataAdapter("select * from etapas", cnn)
        Dim Mytb As New DatasetForm.etapasDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function
    Private Sub cargeetapa_acto()
        Dim Table As DatasetForm.etapasDataTable = LoadDatos()
        Me.ViewState("Datosetapa_acto") = Table
        dtgetapa_acto.DataSource = Table
        dtgetapa_acto.DataBind()

        Dtal.InnerHtml = ("Se detectaron " & Num2Text(Table.Rows.Count) & " (" & Table.Rows.Count & ") Etapas").ToUpper
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Try
                Call cargeetapa_acto()

                txtCodigo.Text = ultimoCon(3, 2).Trim
            Catch ex As Exception
                Messenger.InnerHtml = "<font color='#8A0808' >Error :" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
            End Try
        End If
    End Sub
    Protected Sub LinkIrActo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkIrActo.Click
        If Not ViewState("Codigo") Is Nothing Then
            Response.Redirect("maestro_actuaciones.aspx" & "?codigo=" & ViewState("Codigo").ToString)
        Else
            Response.Redirect("maestro_actuaciones.aspx")
        End If
    End Sub
    Protected Sub dtgetapa_acto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgetapa_acto.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.etapasDataTable = CType(.ViewState("Datosetapa_acto"), DatasetForm.etapasDataTable)
            With Mytb.Item(.dtgetapa_acto.SelectedIndex)
                Try
                    txtCodigo.Text = .codigo.Trim
                    txtNombre.Text = .nombre.Trim
                    ViewState("Codigo") = dtgetapa_acto.SelectedIndex
                    txtNombre.Focus()
                Catch ex As Exception
                    Messenger.InnerHtml = "<font color='red'>Error :" & ex.Message & "</font>"
                End Try
            End With
        End With
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Using con As New SqlConnection(Session("ConexionServer").ToString)
                With Me
                    Dim proximo_numero As String = "00"
                    Dim da As New SqlDataAdapter("select * from etapas", con)
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                    ' Creamos los comandos con el CommandBuilder
                    Dim cb As New SqlCommandBuilder(da)

                    da.InsertCommand = cb.GetInsertCommand()
                    da.UpdateCommand = cb.GetUpdateCommand()
                    'da.DeleteCommand = cb.GetDeleteCommand()

                    Dim LogProc As New LogProcesos

                    con.Open()
                    Dim tran As SqlTransaction = con.BeginTransaction

                    da.InsertCommand.Transaction = tran
                    da.UpdateCommand.Transaction = tran
                    'da.DeleteCommand.Transaction = tran


                    Dim Mytb As DatasetForm.etapasDataTable = CType(.ViewState("Datosetapa_acto"), DatasetForm.etapasDataTable)

                    If Mytb.Select("CODIGO='" & .txtCodigo.Text & "'").Length > 0 Then
                        'En esta parate se procede a actualizar el registro si existe 
                        Dim Row As DatasetForm.etapasRow = Mytb.Select("CODIGO='" & .txtCodigo.Text & "'")(0)
                        If Not Row Is Nothing Then
                            Row.nombre = .txtNombre.Text
                        End If
                    Else
                        'en esta parte se ingresa un registro nuevo
                        'Actualizar el consecutivo
                        Dim mycommand As New SqlCommand("update MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = 3", con)
                        mycommand.Parameters.Add("@proximo_numero", SqlDbType.Int)
                        mycommand.Parameters("@proximo_numero").Direction = ParameterDirection.Output
                        mycommand.Transaction = tran
                        mycommand.ExecuteNonQuery()

                        '----------------
                        'Después de cada GRABAR hay que llamar al log de auditoria
                        'Dim LogProc As New LogProcesos
                        LogProc.SaveLog(Session("ssloginusuario"), "Actualización de consecutivos", "CON_IDENTIFICADOR = 3 ", mycommand)
                        '----------------

                        Dim conse As Integer = CType(mycommand.Parameters("@proximo_numero").Value, Integer)
                        proximo_numero = Format(conse, "00")
                        Mytb.AddetapasRow(proximo_numero.Trim, .txtNombre.Text.Trim.ToUpper)
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
                            LogProc.SaveLog(Session("ssloginusuario"), "Entrada de etapas", "codigo, nombre", da.InsertCommand)
                        Else                            
                            LogProc.SaveLog(Session("ssloginusuario"), "Entrada de etapas", "codigo, nombre", da.UpdateCommand)
                        End If

                        Messenger.InnerHtml = "<font color='#FFFFFF'><b> Se han guardado los datos</b></font>"

                        Call cargeetapa_acto()
                        txtCodigo.Text = ultimoCon(3, 2).Trim
                        txtNombre.Text = "" : txtNombre.Focus()
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

    Private Shared Sub da_RowUpdating(ByVal sender As Object, ByVal e As SqlRowUpdatingEventArgs)
        If e.Command IsNot Nothing Then
            TipoComandoUsado = e.StatementType.ToString.ToUpper
        End If
    End Sub

    Private Sub cancelar()
        txtNombre.Text = ""
        txtNombre.Focus()
        'Call cargeetapa_acto()

        txtCodigo.Text = ultimoCon(3, 2).Trim
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Call cancelar()
    End Sub
End Class