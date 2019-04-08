Imports System.Data.SqlClient
Partial Public Class EditCUOTASPARTES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboEstadoEnte()

            Dim Connection As New SqlConnection(CadenaConexion())
            Connection.Open()
            ' 
            If Len(Request("pExpediente")) > 0 Then
                Dim sql As String = "select * from CUOTASPARTES where [NroExp] = @NroExp"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @NroExp parameter in the Command select query
                Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    cboEstadoEnte.SelectedValue = Reader("EstadoEnte").ToString().Trim

                    txtFechaLey550.Text = Left(Reader("FechaLey550").ToString().Trim, 10)
                    txtFechaOfiConsulta.Text = Left(Reader("FechaOfiConsulta").ToString().Trim, 10)
                    txtFecReciOfiConsul.Text = Left(Reader("FecReciOfiConsul").ToString().Trim, 10)
                    txtFechaEscritoObj.Text = Left(Reader("FechaEscritoObj").ToString().Trim, 10)
                    txtFechaAcepCP.Text = Left(Reader("FechaAcepCP").ToString().Trim, 10)
                    txtFechaFormalSilPos.Text = Left(Reader("FechaFormalSilPos").ToString().Trim, 10)
                    txtFechaDocResObj.Text = Left(Reader("FechaDocResObj").ToString().Trim, 10)

                    txtNoResolCP.Text = Reader("NoResolCP").ToString().Trim
                    txtFecResolCP.Text = Left(Reader("FecResolCP").ToString().Trim, 10)
                    txtNoCuentaCobro.Text = Reader("NoCuentaCobro").ToString().Trim
                    txtFechaCuentaCobro.Text = Left(Reader("FechaCuentaCobro").ToString().Trim, 10)
                    txtFecEntregaCtaCobro.Text = Left(Reader("FecEntregaCtaCobro").ToString().Trim, 10)
                    txtObservaciones.Text = Reader("Observaciones").ToString().Trim
                    '
                    cmdEditPensionados.Visible = True
                Else
                    '
                    cmdEditPensionados.Visible = False
                End If
                ' 
                Reader.Close()
                Connection.Close()

            End If
        End If
    End Sub

    Protected Sub LoadcboEstadoEnte()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(CadenaConexion())

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "SELECT codigo, nombre FROM ESTADOS_ENTES order by codigo"
        Dim Command As New SqlCommand(sql, Connection)
        cboEstadoEnte.DataTextField = "nombre"
        cboEstadoEnte.DataValueField = "codigo"
        cboEstadoEnte.DataSource = Command.ExecuteReader()
        cboEstadoEnte.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub


    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim Expediente As String = Request("pExpediente").ToString.Trim

        'Create a new connection to the database
        Dim Connection As New SqlConnection(CadenaConexion())
        Connection.Open()
        Dim Command As SqlCommand

        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into CUOTASPARTES ([NroExp], [EstadoEnte], [FechaLey550], [FechaOfiConsulta], [FecReciOfiConsul], [FechaEscritoObj], [FechaAcepCP], [FechaFormalSilPos], [FechaDocResObj], [NoResolCP], [FecResolCP], [NoCuentaCobro], [FechaCuentaCobro], [FecEntregaCtaCobro], [Observaciones] ) VALUES ( @NroExp, @EstadoEnte, @FechaLey550, @FechaOfiConsulta, @FecReciOfiConsul, @FechaEscritoObj, @FechaAcepCP, @FechaFormalSilPos, @FechaDocResObj, @NoResolCP, @FecResolCP, @NoCuentaCobro, @FechaCuentaCobro, @FecEntregaCtaCobro, @Observaciones ) "
        Dim UpdateSQL As String = "Update CUOTASPARTES set [NroExp] = @NroExp, [EstadoEnte] = @EstadoEnte, [FechaLey550] = @FechaLey550, [FechaOfiConsulta] = @FechaOfiConsulta, [FecReciOfiConsul] = @FecReciOfiConsul, [FechaEscritoObj] = @FechaEscritoObj, [FechaAcepCP] = @FechaAcepCP, [FechaFormalSilPos] = @FechaFormalSilPos, [FechaDocResObj] = @FechaDocResObj, [NoResolCP] = @NoResolCP, [FecResolCP] = @FecResolCP, [NoCuentaCobro] = @NoCuentaCobro, [FechaCuentaCobro] = @FechaCuentaCobro, [FecEntregaCtaCobro] = @FecEntregaCtaCobro, [Observaciones] = @Observaciones where [NroExp] = @NroExp "
        '
        If Not ExisteExpedienteCuotaParte(Expediente) Then
            ' INSERT
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", Expediente)

        Else
            ' UPDATE
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroExp", Expediente)

        End If

        If cboEstadoEnte.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@EstadoEnte", cboEstadoEnte.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@EstadoEnte", DBNull.Value)

        End If

        If IsDate(txtFechaLey550.Text) Then
            Command.Parameters.AddWithValue("@FechaLey550", txtFechaLey550.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FechaLey550", DBNull.Value)

        End If

        If IsDate(txtFechaOfiConsulta.Text) Then
            Command.Parameters.AddWithValue("@FechaOfiConsulta", txtFechaOfiConsulta.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FechaOfiConsulta", DBNull.Value)

        End If

        If IsDate(txtFecReciOfiConsul.Text) Then
            Command.Parameters.AddWithValue("@FecReciOfiConsul", txtFecReciOfiConsul.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecReciOfiConsul", DBNull.Value)

        End If

        If IsDate(txtFechaEscritoObj.Text) Then
            Command.Parameters.AddWithValue("@FechaEscritoObj", txtFechaEscritoObj.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FechaEscritoObj", DBNull.Value)

        End If

        If IsDate(txtFechaAcepCP.Text.Trim) Then
            Command.Parameters.AddWithValue("@FechaAcepCP", txtFechaAcepCP.Text)
        Else
            Command.Parameters.AddWithValue("@FechaAcepCP", DBNull.Value)

        End If

        If IsDate(txtFechaFormalSilPos.Text.Trim) Then
            Command.Parameters.AddWithValue("@FechaFormalSilPos", txtFechaFormalSilPos.Text)
        Else
            Command.Parameters.AddWithValue("@FechaFormalSilPos", DBNull.Value)

        End If

        If IsDate(txtFechaDocResObj.Text.Trim) Then
            Command.Parameters.AddWithValue("@FechaDocResObj", txtFechaDocResObj.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FechaDocResObj", DBNull.Value)

        End If

        Command.Parameters.AddWithValue("@NoResolCP", txtNoResolCP.Text)

        If IsDate(txtFecResolCP.Text) Then
            Command.Parameters.AddWithValue("@FecResolCP", txtFecResolCP.Text)
        Else
            Command.Parameters.AddWithValue("@FecResolCP", DBNull.Value)

        End If

        Command.Parameters.AddWithValue("@NoCuentaCobro", txtNoCuentaCobro.Text.Trim)

        If IsDate(txtFechaCuentaCobro.Text) Then
            Command.Parameters.AddWithValue("@FechaCuentaCobro", txtFechaCuentaCobro.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FechaCuentaCobro", DBNull.Value)

        End If

        If IsDate(txtFecEntregaCtaCobro.Text.Trim) Then
            Command.Parameters.AddWithValue("@FecEntregaCtaCobro", txtFecEntregaCtaCobro.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@FecEntregaCtaCobro", DBNull.Value)

        End If
        Command.Parameters.AddWithValue("@Observaciones", txtObservaciones.Text)

        ' 
        'Run the sql command against the database with no return values 
        Try
            Command.ExecuteNonQuery()
            Connection.Close()
            'Response.Redirect("CUOTASPARTES.aspx?pExpediente=" & Expediente)
            CustomValidator1.Text = "Datos guardados con éxito"
            CustomValidator1.IsValid = False

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()

        End Try

    End Sub

    Public Function ExisteExpedienteCuotaParte(ByVal pExpediente As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(CadenaConexion())
        Connection.Open()
        Dim sql As String = "SELECT NroExp FROM cuotaspartes WHERE NroExp = '" & pExpediente & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Protected Sub cmdEditPensionados_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEditPensionados.Click
        Response.Redirect("CUOTASPARTESDETALLE.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub imgBtnBorraFecReciOfiConsul_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecReciOfiConsul.Click
        txtFecReciOfiConsul.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecescritoObj_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecescritoObj.Click
        txtFechaEscritoObj.Text = ""
    End Sub

    Protected Sub imgBtnBorraFechaAcepCP_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaAcepCP.Click
        txtFechaAcepCP.Text = ""
    End Sub

    Protected Sub imgBtnBorraFechaFormalSilPos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaFormalSilPos.Click
        txtFechaFormalSilPos.Text = ""
    End Sub

    Protected Sub imgBtnBorraFechaDocResObj_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaDocResObj.Click
        txtFechaDocResObj.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecResolCP_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecResolCP.Click
        txtFecResolCP.Text = ""
    End Sub

    Protected Sub imgBtnBorraFechaCuentaCobro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaCuentaCobro.Click
        txtFechaCuentaCobro.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecEntregaCtaCobro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecEntregaCtaCobro.Click
        txtFecEntregaCtaCobro.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecOfiConsulta_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecOfiConsulta.Click
        txtFechaOfiConsulta.Text = ""
    End Sub

    Protected Sub imgBtnBorraFecLey550_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecLey550.Click
        txtFechaLey550.Text = ""
    End Sub
End Class