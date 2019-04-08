Imports System.Data.SqlClient
Partial Public Class Festivos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Call LoadDatos()
        End If
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If txtFecha.Text <> Nothing Then
                'Dim dia, mes, anio, Descripcion As String

                'dia = Left(Me.txtFecha.Text.Trim(), 2)
                'mes = Mid(Me.txtFecha.Text.Trim(), 4, 2)
                'anio = Mid(Me.txtFecha.Text.Trim(), 7, 4)
                Dim Descripcion As String
                Dim command As New SqlCommand
                Dim ado As New SqlConnection(Session("ConexionServer").ToString)

                ado.Open()
                If txtDescripcion.Text = Nothing Then
                    Descripcion = "DIA DE FIESTA"
                Else
                    Descripcion = txtDescripcion.Text
                End If

                command.Connection = ado
                If Me.ViewState("seleccionado") Is Nothing Then
                    command.CommandText = "delete TDIAS_FESTIVOS where FECHA = '" & txtFecha.Text.Trim() & "'"
                    command.ExecuteNonQuery()

                    command.CommandText = "INSERT INTO TDIAS_FESTIVOS VALUES ('" & txtFecha.Text.Trim() & "','" & Descripcion & "')"
                Else
                    command.CommandText = "UPDATE TDIAS_FESTIVOS SET FECHA = '" & txtFecha.Text.Trim() & "',DESCRIPCION = '" & Descripcion & "' WHERE ID_DNL=" & Me.ViewState("seleccionado").ToString.Trim
                End If

                command.ExecuteNonQuery()
                Me.ViewState("seleccionado") = Nothing
                ado.Close()
                Call Cancelar()
                Messenger.InnerHtml = "<font color='#FFFF00'><b>Dato guardado satisfactoriamente.</b></font>"
                Call LoadDatos()
            Else
                Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & "No puede guardar sin una fecha" & " <br /><b>NOTA :</b> Intentalo otra vez</font>"
            End If
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & " <br /><b>NOTA :</b> Intentalo otra vez</font>"
        End Try
        'fechaserver = mes & "/" & dia & "/" & anio
    End Sub

    Private Sub LoadDatos()
        Dim cnn As String = Session("ConexionServer").ToString
        Dim consulta As String

        If chkAnnio.Checked = True Then
            consulta = "select ID_DNL,DESCRIPCION, FECHA  FROM TDIAS_FESTIVOS where year(fecha) = '" & ListAnnios.Text.ToString.Trim & "' ORDER BY FECHA DESC "
        Else
            consulta = "select ID_DNL,DESCRIPCION, FECHA  FROM TDIAS_FESTIVOS ORDER BY FECHA DESC"
        End If

        Dim MyAdapter As New SqlDataAdapter(consulta, cnn)
        Dim Mytb As New DatasetForm.TDIAS_FESTIVOS_formDataTable
        MyAdapter.Fill(Mytb)

        Me.ViewState("DatoFestivos") = Mytb
        dtfecha.DataSource = Mytb
        dtfecha.DataBind()


        'Años detecatados en la base de datos.
        If chkAnnio.Checked = False Then
            MyAdapter = New SqlDataAdapter("SELECT DISTINCT year(fecha) as ANNIO  FROM TDIAS_FESTIVOS ORDER BY ANNIO DESC", cnn)
            Dim Mytb2 As New DataTable

            Mytb2.Columns.Add(New DataColumn("ANNIO", GetType(String)))
            Mytb2.Rows.Add(CreateRow("Todos", Mytb2))
            MyAdapter.Fill(Mytb2)
            If Mytb2.Rows.Count > 0 Then
                ListAnnios.DataSource = Mytb2
                ListAnnios.DataTextField = "ANNIO"
                ListAnnios.DataValueField = "ANNIO"

                ListAnnios.DataBind()
            End If
        End If
    End Sub
    Function CreateRow(ByVal Value As String, ByVal dt As DataTable) As DataRow
        Dim dr As DataRow = dt.NewRow()
        dr(0) = Value
        Return dr
    End Function
    Protected Sub dtfecha_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtfecha.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.TDIAS_FESTIVOS_formDataTable = CType(.ViewState("DatoFestivos"), DatasetForm.TDIAS_FESTIVOS_formDataTable)
            With Mytb.Item(.dtfecha.SelectedIndex)
                Try
                    txtFecha.Text = .FECHA
                    txtDescripcion.Text = .DESCRIPCION
                    'Messenger.InnerHtml = ""

                    Me.ViewState("seleccionado") = .ID_DNL.ToString
                    lbleditar.Text = "Editando Cod. " & .ID_DNL.ToString
                    Panel1.Visible = True
                    'Me.ViewState("useractivo") = .useractivo
                Catch ex As Exception
                    Messenger.InnerHtml = "<font color='red'>Error : " & ex.Message & "</font>"
                End Try
            End With
        End With
    End Sub
  
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Call Cancelar()
    End Sub
    Private Sub Cancelar()
        Me.ViewState("seleccionado") = Nothing
        txtDescripcion.Text = ""
        txtFecha.Text = ""
        lbleditar.Text = ""
        txtFecha.Focus()
        Panel1.Visible = False
    End Sub

    Protected Sub ListAnnios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListAnnios.SelectedIndexChanged
        Dim cnn As String = Session("ConexionServer").ToString
        If ListAnnios.Text = "Todos" Then
            Call LoadDatos()
        Else
            Dim MyAdapter As New SqlDataAdapter("select ID_DNL,DESCRIPCION, FECHA  from TDIAS_FESTIVOS WHERE year(fecha) = '" & ListAnnios.Text & "' ORDER BY year(fecha) DESC", cnn)
            Dim Mytb As New DatasetForm.TDIAS_FESTIVOS_formDataTable
            MyAdapter.Fill(Mytb)

            Me.ViewState("DatoFestivos") = Mytb
            dtfecha.DataSource = Mytb
            dtfecha.DataBind()
        End If
    End Sub

    Protected Sub LinkEliminarFestivo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkEliminarFestivo.Click

        Try
            Dim dia, mes, anio As String

            dia = Left(Me.txtFecha.Text.Trim(), 2)
            mes = Mid(Me.txtFecha.Text.Trim(), 4, 2)
            anio = Mid(Me.txtFecha.Text.Trim(), 7, 4)

            Dim command As New SqlCommand
            Dim ado As New SqlConnection(Session("ConexionServer").ToString)
            ado.Open()

            command.Connection = ado
            command.CommandText = "delete TDIAS_FESTIVOS where FECHA = '" & txtFecha.Text.Trim() & "'"
            command.ExecuteNonQuery()

            Me.ViewState("seleccionado") = Nothing
            ado.Close()
            Call Cancelar()

            Messenger.InnerHtml = "<font color='#FFFF00'><b>Dato eliminado satisfactoriamente .</b></font>"
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & " <br /><b>NOTA :</b> Intentalo otra vez</font>"
        End Try
    End Sub
End Class