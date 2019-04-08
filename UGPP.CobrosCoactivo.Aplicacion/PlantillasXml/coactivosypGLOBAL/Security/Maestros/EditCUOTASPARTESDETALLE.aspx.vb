Imports System.Data.SqlClient
Partial Public Class EditCUOTASPARTESDETALLE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            'Create a new connection to the database
            Dim Connection As New SqlConnection(CadenaConexion())
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("pPensionado")) > 0 Then
                Dim sql As String = "SELECT * FROM CUOTASPARTESDETALLE WHERE NroExp = @NroExp AND Pensionado = @Pensionado"

                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
                Command.Parameters.AddWithValue("@Pensionado", Request("pPensionado"))

                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtPensionado.Text = Reader("Pensionado").ToString().Trim
                    txtNomPensionado.Text = Reader("NomPensionado").ToString().Trim
                    txtCapitalCP.Text = Reader("CapitalCP").ToString().Trim
                    'Fechas
                    txtPeriodoIniCob.Text = Left(Reader("PeriodoIniCob").ToString().Trim, 10)
                    txtPeriodoFinCob.Text = Left(Reader("PeriodoFinCob").ToString().Trim, 10)
                    txtFechaPresPerAnt.Text = Left(Reader("FechaPresPerAnt").ToString().Trim, 10)
                    '
                    txtFalloOrdenRecPen.Text = Reader("FalloOrdenRecPen").ToString().Trim
                    txtResolCumpleFallo.Text = Reader("ResolCumpleFallo").ToString().Trim
                    txtObservacion.Text = Reader("Observacion").ToString().Trim

                End If
                'Close the Data Reader we are done with it.
                Reader.Close()
                Connection.Close()
            Else
                'Since this is an insert then you can't delete it yet because it's not in the database.
                cmdDelete.Visible = False
            End If
        End If
    End Sub

    'Event handler for Delete clicks 
    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim pExpediente As String = Request("pExpediente")

        Dim Connection As New SqlConnection(CadenaConexion())
        Connection.Open()
        Dim sql As String = "DELETE FROM CUOTASPARTESDETALLE WHERE NroExp = @NroExp AND Pensionado = @Pensionado"

        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Command.Parameters.AddWithValue("@Pensionado", Request("pPensionado"))
        ' 
        Try
            Command.ExecuteNonQuery()
            'Close the Connection Object 
            Connection.Close()
            Response.Redirect("CUOTASPARTESDETALLE.aspx?pExpediente=" & pExpediente)
        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
        End Try

    End Sub


    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim pExpediente As String = Request("pExpediente")

        'Validar cedula y nombre del pensionado
        If txtPensionado.Text.Trim = "" Then
            CustomValidator1.Text = "Digite la identificación del Beneficiario / causante por favor"
            CustomValidator1.IsValid = False
            Return
        End If
        If txtNomPensionado.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el nombre del Beneficiario / causante por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(CadenaConexion())
        Connection.Open()

        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        ' 
        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into CUOTASPARTESDETALLE ([NroExp], [Pensionado], [NomPensionado], [CapitalCP], [PeriodoIniCob], [PeriodoFinCob], [FechaPresPerAnt], [FalloOrdenRecPen], [ResolCumpleFallo], [Observacion] ) VALUES ( @NroExp, @Pensionado, @NomPensionado, @CapitalCP, @PeriodoIniCob, @PeriodoFinCob, @FechaPresPerAnt, @FalloOrdenRecPen, @ResolCumpleFallo, @Observacion ) "
        Dim UpdateSQL As String = "Update CUOTASPARTESDETALLE set [NroExp] = @NroExp, [Pensionado] = @Pensionado, [NomPensionado] = @NomPensionado, [CapitalCP] = @CapitalCP, [PeriodoIniCob] = @PeriodoIniCob, [PeriodoFinCob] = @PeriodoFinCob, [FechaPresPerAnt] = @FechaPresPerAnt, [FalloOrdenRecPen] = @FalloOrdenRecPen, [ResolCumpleFallo] = @ResolCumpleFallo, [Observacion] = @Observacion where [Pensionado] = @Pensionado "
        ' 
        Dim pPensionado As String = txtPensionado.Text.Trim
        If Not ExistePensionadoEnExpediente(pExpediente, pPensionado) Then
            'We are doing an insert              
            Command = New SqlCommand(InsertSQL, Connection)
        Else
            'Set the @Pensionado field for updates.              
            Command = New SqlCommand(UpdateSQL, Connection)
        End If

        'Parametros
        Command.Parameters.AddWithValue("@NroExp", pExpediente)
        Command.Parameters.AddWithValue("@Pensionado", pPensionado)
        Command.Parameters.AddWithValue("@NomPensionado", txtNomPensionado.Text.Trim)
        If IsNumeric(txtCapitalCP.Text) Then
            Command.Parameters.AddWithValue("@CapitalCP", txtCapitalCP.Text.Trim)
        Else
            Command.Parameters.AddWithValue("@CapitalCP", DBNull.Value)
        End If

        If IsDate(txtPeriodoIniCob.Text.Trim) Then
            Command.Parameters.AddWithValue("@PeriodoIniCob", Left(txtPeriodoIniCob.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@PeriodoIniCob", DBNull.Value)
        End If

        If IsDate(txtPeriodoFinCob.Text.Trim) Then
            Command.Parameters.AddWithValue("@PeriodoFinCob", Left(txtPeriodoFinCob.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@PeriodoFinCob", DBNull.Value)
        End If
        If IsDate(txtFechaPresPerAnt.Text.Trim) Then
            Command.Parameters.AddWithValue("@FechaPresPerAnt", Left(txtFechaPresPerAnt.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FechaPresPerAnt", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@FalloOrdenRecPen", txtFalloOrdenRecPen.Text)
        Command.Parameters.AddWithValue("@ResolCumpleFallo", txtResolCumpleFallo.Text)
        Command.Parameters.AddWithValue("@Observacion", txtObservacion.Text)

        Try
            Command.ExecuteNonQuery()
            Connection.Close()
            Response.Redirect("CUOTASPARTESDETALLE.aspx?pExpediente=" & pExpediente)

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
        End Try


    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim pExpediente As String = Request("pExpediente")
        Response.Redirect("CUOTASPARTESDETALLE.aspx?pExpediente=" & pExpediente)
    End Sub

    Private Function ExistePensionadoEnExpediente(ByVal pExpediente As String, ByVal pPensionado As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(CadenaConexion())
        Connection.Open()
        Dim sql As String = "SELECT NroExp, Pensionado FROM cuotaspartesdetalle WHERE NroExp = '" & pExpediente & "' AND Pensionado = '" & pPensionado & "'"
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

    Protected Sub imgBtnBorraPeriodoIniCob_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraPeriodoIniCob.Click
        txtPeriodoIniCob.Text = ""
    End Sub

    Protected Sub imgBtnBorraFinCob_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFinCob.Click
        txtPeriodoFinCob.Text = ""
    End Sub

    Protected Sub imgBtnBorraFechaPresPerAnt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFechaPresPerAnt.Click
        txtFechaPresPerAnt.Text = ""
    End Sub
End Class