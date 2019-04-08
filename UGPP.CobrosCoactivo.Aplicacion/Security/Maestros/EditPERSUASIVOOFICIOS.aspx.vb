Imports System.Data.SqlClient

Partial Public Class EditPERSUASIVOOFICIOS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            LoadcboTipificacion()

            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            If Len(Request("numero")) > 0 Then
                Dim sql As String = "SELECT * FROM PERSUASIVOOFICIOS WHERE NroExp = @NroExp AND numero = @numero"
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
                Command.Parameters.AddWithValue("@numero", Request("numero"))

                Dim Reader As SqlDataReader = Command.ExecuteReader
                If Reader.Read Then
                    'txtnumero.Text = Reader("numero").ToString()
                    txtNroOfi.Text = Reader("NroOfi").ToString()
                    txtFecOfi.Text = Left(Reader("FecOfi").ToString().Trim, 10)
                    txtFecEnvOfi.Text = Left(Reader("FecEnvOfi").ToString().Trim, 10)
                    txtNoGuiaEnt.Text = Reader("NoGuiaEnt").ToString()
                    txtFecAcuseO.Text = Left(Reader("FecAcuseO").ToString().Trim, 10)
                    txtdirenvio.Text = Reader("direnvio").ToString()
                    txtemail.Text = Reader("email").ToString()
                    cboTipificacion.Text = Reader("Tipificacion").ToString()
                    txtObservaciones.Text = Reader("Observaciones").ToString()
                End If
                Reader.Close()
                Connection.Close()

                If Not (Session("mnivelacces") = 1 Or Session("mnivelacces") = 3 Or Session("mnivelacces") = 8) Then
                    cmdDelete.Visible = False
                    cmdSave.Visible = False
                    imgBtnBorraFecOfi.Visible = False
                    imgBtnBorraFecEnvOfi.Visible = False
                    imgBtnBorraFecAcuseO.Visible = False
                    '
                    cboTipificacion.Enabled = False
                    txtNroOfi.Enabled = False
                    txtFecOfi.Enabled = False
                    txtFecEnvOfi.Enabled = False
                    txtNoGuiaEnt.Enabled = False
                    txtFecAcuseO.Enabled = False
                    txtdirenvio.Enabled = False
                    txtemail.Enabled = False
                    txtObservaciones.Enabled = False
                    '
                    CustomValidator1.Text = "Solo los revisores y gestores de información pueden editar este registro"
                    CustomValidator1.IsValid = False
                End If

            Else
                cmdDelete.Visible = False
                'txtnumero.Text = getNextConsec()
            End If
            'txtnumero.ReadOnly = True

        End If
    End Sub

    Protected Sub LoadcboTipificacion()
        'Llenar el combo de PERSUASIVOTIPIFICACION

        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM PERSUASIVOTIPIFICACIONOFICIO ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtPERSUASIVOTIPIFICACION As New DataTable
        Adaptador.Fill(dtPERSUASIVOTIPIFICACION)
        If dtPERSUASIVOTIPIFICACION.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaPERSUASIVOTIPIFICACION As DataRow = dtPERSUASIVOTIPIFICACION.NewRow()
            filaPERSUASIVOTIPIFICACION("codigo") = 0
            filaPERSUASIVOTIPIFICACION("nombre") = " "
            dtPERSUASIVOTIPIFICACION.Rows.Add(filaPERSUASIVOTIPIFICACION)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaPERSUASIVOTIPIFICACION As DataView = New DataView(dtPERSUASIVOTIPIFICACION)
            vistaPERSUASIVOTIPIFICACION.Sort = "codigo"
            '--------------------------------------------------------------------
            cboTipificacion.DataSource = vistaPERSUASIVOTIPIFICACION
            cboTipificacion.DataTextField = "nombre"
            cboTipificacion.DataValueField = "codigo"
            cboTipificacion.DataBind()
        End If
    End Sub

    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "delete from PERSUASIVOOFICIOS where NroExp = @NroExp AND numero = @numero"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Command.Parameters.AddWithValue("@numero", Request("numero"))

        Command.ExecuteNonQuery()

        Connection.Close()
        Response.Redirect("PERSUASIVOOFICIOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim numero As String = Request("numero")

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand

        Dim InsertSQL As String = "INSERT INTO PERSUASIVOOFICIOS (NroExp, numero, NroOfi, FecOfi, FecEnvOfi, NoGuiaEnt, FecAcuseO, direnvio, email, Tipificacion, Observaciones ) VALUES ( @NroExp, @numero, @NroOfi, @FecOfi, @FecEnvOfi, @NoGuiaEnt, @FecAcuseO, @direnvio, @email, @Tipificacion, @Observaciones ) "

        Dim UpdateSQL As String = "UPDATE PERSUASIVOOFICIOS SET NroOfi = @NroOfi, FecOfi = @FecOfi, FecEnvOfi = @FecEnvOfi, NoGuiaEnt = @NoGuiaEnt, FecAcuseO = @FecAcuseO, direnvio = @direnvio, email = @email, Tipificacion = @Tipificacion, Observaciones = @Observaciones WHERE NroExp = @NroExp AND numero = @numero "

        If String.IsNullOrEmpty(numero) Then
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
            Command.Parameters.AddWithValue("@numero", getNextConsec())
        Else
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
            Command.Parameters.AddWithValue("@numero", numero)
        End If



        Command.Parameters.AddWithValue("@NroOfi", txtNroOfi.Text)

        If IsDate(Left(txtFecOfi.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecOfi", Left(txtFecOfi.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecOfi", DBNull.Value)
        End If

        If IsDate(Left(txtFecEnvOfi.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecEnvOfi", Left(txtFecEnvOfi.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecEnvOfi", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NoGuiaEnt", txtNoGuiaEnt.Text)

        If IsDate(Left(txtFecAcuseO.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecAcuseO", Left(txtFecAcuseO.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecAcuseO", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@direnvio", txtdirenvio.Text)

        Command.Parameters.AddWithValue("@email", txtemail.Text)

        If cboTipificacion.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Tipificacion", cboTipificacion.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Tipificacion", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@Observaciones", txtObservaciones.Text)

        Try
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de oficios persuasivos", "No. Expediente " & Request("pExpediente").Trim, Command)

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
            Return

        End Try

        Connection.Close()
        Response.Redirect("PERSUASIVOOFICIOS.aspx?pExpediente=" & Request("pExpediente"))

    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("PERSUASIVOOFICIOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub imgBtnBorraFecOfi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecOfi.Click
        txtFecOfi.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecEnvOfi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecEnvOfi.Click
        txtFecEnvOfi.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecAcuseO_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecAcuseO.Click
        txtFecAcuseO.Text = ""
    End Sub

    Private Function getNextConsec() As Integer
        Dim NewConsec As Integer = 0
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT ISNULL(MAX(numero),0) + 1 AS MaxConsec FROM PERSUASIVOOFICIOS WHERE NroExp = @NroExp"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))

        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NewConsec = Reader("MaxConsec").ToString()
        End If

        Reader.Close()
        Connection.Close()

        Return NewConsec

    End Function
End Class