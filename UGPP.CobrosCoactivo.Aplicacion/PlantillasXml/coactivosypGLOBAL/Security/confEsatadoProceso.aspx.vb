Imports System.Data.SqlClient
Partial Public Class confEsatadoProceso
    Inherits System.Web.UI.Page

    Private Sub cargarDatos()
        Dim cnn As String = Session("ConexionServer").ToString
        Dim MyAdapter As New SqlDataAdapter("select * from actuaciones where codigo <> '000'", cnn)
        Dim Mytb As New DatasetForm.actuacionesDataTable
        MyAdapter.Fill(Mytb)

        dtgetapa_actoCreados.DataSource = Mytb
        dtgetapa_actoCreados.DataBind()

        ViewState("Datos") = Mytb
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Try
                Call cargarDatos()
                Call Chekear()
            Catch ex As Exception
                Messenger.InnerHtml = "<font color='#DF0101'>Error :" & ex.Message & "</font>"
            End Try
        End If
    End Sub
    Private Sub Chekear()
        Dim TblDatos As DatasetForm.actuacionesDataTable = CType(Me.ViewState("Datos"), DatasetForm.actuacionesDataTable)
        Dim x As Integer
        For x = 0 To TblDatos.Rows.Count - 1
            Dim Chk As CheckBox = CType(Me.dtgetapa_actoCreados.Rows(x).Cells(2).Controls(1), CheckBox)
            Dim ChkMasivo As CheckBox = CType(Me.dtgetapa_actoCreados.Rows(x).Cells(3).Controls(1), CheckBox)
            Chk.Checked = TblDatos.Rows(x).Item("historial")
            ChkMasivo.Checked = TblDatos.Rows(x).Item("actMasivo")
        Next
    End Sub
    Protected Sub dtgetapa_actoCreados_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgetapa_actoCreados.SelectedIndexChanged
        'With Me
        '    Dim Mytb As DatasetForm.actuacionesDataTable = CType(.ViewState("Datosetapa_actoCreados"), DatasetForm.actuacionesDataTable)
        '    With Mytb.Item(.dtgetapa_actoCreados.SelectedIndex)
        '        Try
        '            txtCodigo.Text = .codigo
        '            txtNombre.Text = .nombre.TrimEnd
        '            lblDetalle.Text = "EDITANDO ACTUACION " & .codigo.ToString & "::" & .nombre.ToString

        '            Dim tabla As String
        '            tabla = "<table class=""tabla"">" & vbNewLine
        '            tabla += "<table class=""tabla"">"
        '            tabla += "<tr><th colspan=""2"" style=""text-align: center;"">VISOR DE IMAGENES</th></tr>" & vbNewLine
        '            tabla += "<tr>" & vbNewLine
        '            tabla += "<th>ENTIDAD : </th><td>" & mNomEnte & " (" & mIdEnte & ")</td>" & vbNewLine
        '            tabla += "</tr>" & vbNewLine
        '            tabla += "<tr>" & vbNewLine
        '            tabla += "<th>ACTUACION : </th><td>" & mActo & "</td>" & vbNewLine
        '            tabla += "</tr>" & vbNewLine
        '            tabla += "<tr>" & vbNewLine
        '            tabla += "<th>NOMBRE DEL ARCHIVO : </th><td>" & mArchivo & "</td>" & vbNewLine
        '            tabla += "</tr>" & vbNewLine
        '            tabla += "<tr>" & vbNewLine
        '            tabla += "<th>NUMERO DE PAGINA(S) : </th><td>" & NroPaginas & "</td>" & vbNewLine
        '            tabla += "</tr>" & vbNewLine
        '            tabla += "</table>" & vbNewLine

        '            DetalleVisor.InnerHtml = tabla
        '            ViewState("update") = 1
        '        Catch ex As Exception
        '            Messenger.InnerHtml = "<font color='red'>Error :" & ex.Message & "</font>"
        '        End Try
        '    End With
        'End With
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        With Me
            Using con As New SqlConnection(Session("ConexionServer").ToString)
                With Me
                    Dim da As New SqlDataAdapter("select * from actuaciones where codigo <> '000'", con)
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                    ' Creamos los comandos con el CommandBuilder
                    Dim cb As New SqlCommandBuilder(da)

                    da.UpdateCommand = cb.GetUpdateCommand()
                    con.Open()
                    Dim tran As SqlTransaction = con.BeginTransaction
                    da.UpdateCommand.Transaction = tran
                    Dim x As Integer
                    Dim TblDatos As DatasetForm.actuacionesDataTable = CType(.ViewState("Datos"), DatasetForm.actuacionesDataTable)
                    For x = 0 To TblDatos.Rows.Count - 1
                        Dim Chk As CheckBox = CType(.dtgetapa_actoCreados.Rows(x).Cells(2).Controls(1), CheckBox)
                        Dim ChkMasivo As CheckBox = CType(.dtgetapa_actoCreados.Rows(x).Cells(3).Controls(1), CheckBox)

                        If TblDatos.Select("CODIGO='" & TblDatos.Rows(x).Item("CODIGO") & "'").Length > 0 Then
                            Dim Row As DatasetForm.actuacionesRow = TblDatos.Select("CODIGO='" & TblDatos.Rows(x).Item("CODIGO") & "'")(0)
                            If Not Row Is Nothing Then
                                Row("historial") = Chk.Checked
                                Row("actMasivo") = ChkMasivo.Checked
                            End If
                        End If
                    Next

                    Try
                        'Actualizamos los datos de la tabla
                        da.Update(TblDatos)
                        tran.Commit()
                        Messenger.InnerHtml = "<font color='#FFFFFF'><b>Se guardaron los datos satisfactoriamente.</b></font>"
                    Catch ex As Exception
                        ' Si hay error, desahacemos lo que se haya hecho
                        tran.Rollback()
                        Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
                    End Try
                    con.Close()
                End With
            End Using
        End With
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        Dim codigo As String
        codigo = Mid(Me.txtBuscar.Text.Trim(), Me.txtBuscar.Text.IndexOf(":") + 3)

        Dim cnn As String = Session("ConexionServer").ToString
        Dim MyAdapter As New SqlDataAdapter("select * from actuaciones where codigo <> '000' and idetapa ='" & codigo & "'", cnn)
        Dim Mytb As New DatasetForm.actuacionesDataTable
        MyAdapter.Fill(Mytb)

        dtgetapa_actoCreados.DataSource = Mytb
        dtgetapa_actoCreados.DataBind()

        ViewState("Datos") = Mytb
        Call Chekear()
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        txtBuscar.Text = ""
        Call cargarDatos()
        Call Chekear()
    End Sub
End Class