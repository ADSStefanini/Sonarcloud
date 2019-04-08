Imports System.Data.SqlClient
Partial Public Class EditPRUEBAS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM PRUEBAS where NroRad = @NroRad AND NroAutoPru = @NroAutoPru"
                Dim Command As New SqlCommand(sql, Connection)
                'Parametros
                Command.Parameters.AddWithValue("@NroRad", Request("pRadicado"))
                Command.Parameters.AddWithValue("@NroAutoPru", Request("ID"))

                Dim Reader As SqlDataReader = Command.ExecuteReader

                'If at least one record was found
                If Reader.Read Then

                    txtNroAutoPru.Text = Reader("NroAutoPru").ToString()
                    txtFecAutoPru.Text = Left(Reader("FecAutoPru").ToString().Trim, 10)
                    txtFecIniTerPro.Text = Left(Reader("FecIniTerPro").ToString().Trim, 10)
                    txtFecFinTerPro.Text = Left(Reader("FecFinTerPro").ToString().Trim, 10)
                End If
                'Close the Data Reader we are done with it.
                Reader.Close()
                Connection.Close()
            Else

            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdSave.Visible = False

                'Desactivar controles
                txtNroAutoPru.Enabled = False
                txtFecAutoPru.Enabled = False
                txtFecIniTerPro.Enabled = False
                txtFecFinTerPro.Enabled = False
            End If
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If txtNroAutoPru.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el No. Auto que decreta pruebas por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        If txtFecAutoPru.Text.Trim = "" Then
            CustomValidator1.Text = "Digite la fecha auto que decreta pruebas por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        Dim ID As String = Request("ID") 'NroAutoPru
        Dim pRadicado As String = Request("pRadicado")


        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        Dim InsertSQL As String = "Insert into PRUEBAS ([NroRad], [NroAutoPru], [FecAutoPru], [FecIniTerPro], [FecFinTerPro] ) VALUES ( @NroRad, @NroAutoPru, @FecAutoPru, @FecIniTerPro, @FecFinTerPro ) "
        Dim UpdateSQL As String = "Update PRUEBAS set [FecAutoPru] = @FecAutoPru, [FecIniTerPro] = @FecIniTerPro, [FecFinTerPro] = @FecFinTerPro where NroRad = @NroRad AND  NroAutoPru = @NroAutoPru "

        If String.IsNullOrEmpty(ID) Then
            'insert
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroRad", pRadicado)
        Else
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroRad", pRadicado)
        End If

        Command.Parameters.AddWithValue("@NroAutoPru", txtNroAutoPru.Text)

        If IsDate(Left(txtFecAutoPru.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecAutoPru", Left(txtFecAutoPru.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecAutoPru", DBNull.Value)

        End If

        If IsDate(Left(txtFecIniTerPro.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecIniTerPro", Left(txtFecIniTerPro.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecIniTerPro", DBNull.Value)
        End If

        If IsDate(Left(txtFecFinTerPro.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecFinTerPro", Left(txtFecFinTerPro.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecFinTerPro", DBNull.Value)
        End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Registro de pruebas", "No. radicado " & pRadicado, Command)

        Catch ex As Exception

        End Try

        Connection.Close()
        Response.Redirect("PRUEBAS.aspx?pRadicado=" & Request("pRadicado") & "&pExpediente=" & Request("pExpediente"))
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("PRUEBAS.aspx?pRadicado=" & Request("pRadicado") & "&pExpediente=" & Request("pExpediente"))
    End Sub

End Class