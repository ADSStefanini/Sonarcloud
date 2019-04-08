Imports System.Data.SqlClient

Partial Class EditPAGOSOBSERVACIONES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            cmdDelete.Visible = False
            cmdSave.Visible = False
            Loadcbogestor()
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM PAGOSOBSERVACIONES WHERE IdUnico = @IdUnico"
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@IdUnico", Request("ID"))
                Dim Reader As SqlDataReader = Command.ExecuteReader
                If Reader.Read Then
                    txtFecha.Text = Left(Reader("Fecha").ToString().Trim, 10)
                    cbogestor.SelectedValue = Reader("gestor").ToString()
                    txtObservaciones.Text = Reader("observaciones").ToString()

                End If
                Reader.Close()
                Connection.Close()
            Else
                ' ADICIONAR
                cmdSave.Visible = True
                txtFecha.Text = Left(Date.Now.ToString, 10)
                txtFecha.Enabled = False
                cbogestor.SelectedValue = Session("sscodigousuario")
            End If
            cbogestor.Enabled = False
            txtFecha.Enabled = False
            '    Response.Redirect("PAGOSOBSERVACIONES.aspx?pExpediente=" & Request("pExpediente").Trim)
        End If
        ' No se debe permitir eliminar la fecha de la observacion
        imgBtnBorraFecha.Visible = False

    End Sub

    Protected Sub Loadcbogestor()

        'Llenar el combo de usuarios
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM usuarios ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtUSUARIOS As New DataTable
        Adaptador.Fill(dtUSUARIOS)
        If dtUSUARIOS.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaUSUARIOS As DataRow = dtUSUARIOS.NewRow()
            filaUSUARIOS("codigo") = 0
            filaUSUARIOS("nombre") = " "
            dtUSUARIOS.Rows.Add(filaUSUARIOS)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaUSUARIOS As DataView = New DataView(dtUSUARIOS)
            vistaUSUARIOS.Sort = "codigo"
            '--------------------------------------------------------------------
            cbogestor.DataSource = vistaUSUARIOS
            cbogestor.DataTextField = "nombre"
            cbogestor.DataValueField = "codigo"
            cbogestor.DataBind()
        End If

    End Sub


    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "DELETE FROM PAGOSOBSERVACIONES WHERE IdUnico = @IdUnico"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@IdUnico", Request("ID"))
        Command.ExecuteNonQuery()

        Connection.Close()
        Response.Redirect("PAGOSOBSERVACIONES.aspx?pExpediente=" & Request("pExpediente").Trim)
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand

        Dim InsertSQL As String = "INSERT INTO PAGOSOBSERVACIONES ([NroExp], [Fecha], [gestor], [observaciones] ) VALUES ( @NroExp, @Fecha, @gestor, @observaciones ) "
        Dim UpdateSQL As String = "UPDATE PAGOSOBSERVACIONES SET NroExp = @NroExp, Fecha = @Fecha, gestor = @gestor, observaciones = @observaciones where IdUnico = @IdUnico "

        If String.IsNullOrEmpty(ID) Then
            Command = New SqlCommand(InsertSQL, Connection)
        Else
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@IdUnico", ID)
        End If

        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente").Trim)

        If IsDate(Left(txtFecha.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@Fecha", Left(txtFecha.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@Fecha", DBNull.Value)
        End If

        If cbogestor.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@gestor", cbogestor.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@gestor", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@observaciones", txtObservaciones.Text)

        Try
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de PAgos ", "No. Expediente " & Request("pExpediente").Trim, Command)

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
            Return

        End Try

        Connection.Close()
        Response.Redirect("PAGOSOBSERVACIONES.aspx?pExpediente=" & Request("pExpediente").Trim)

    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("PAGOSOBSERVACIONES.aspx?pExpediente=" & Request("pExpediente").Trim)
    End Sub

    Protected Sub imgBtnBorraFecha_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecha.Click
        txtFecha.Text = ""
    End Sub
End Class
