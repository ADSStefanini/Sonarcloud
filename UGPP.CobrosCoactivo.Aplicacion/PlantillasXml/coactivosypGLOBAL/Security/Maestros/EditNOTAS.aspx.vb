Imports System.Data.SqlClient

Partial Public Class EditNOTAS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            ' 
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM notas WHERE IdUnico = @IdUnico"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@IdUnico", Request("ID"))
                ' 
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtObservaciones.Text = Reader("Observaciones").ToString()
                    txtFecha.Text = Left(Reader("Fecha").ToString().Trim, 10)
                End If
                ' 
                Reader.Close()
                Connection.Close()
            Else
                txtFecha.Text = Left(Date.Now.ToString, 10)
            End If
            ' 
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")
        Dim Connection As New SqlConnection(CadenaConexion())

        'Opens a connection to the database.
        Connection.Open()
        Dim Command As SqlCommand
        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into [dbo].[NOTAS] ([NroExp], [Gestor], [Observaciones], [Modulo], [Fecha] ) VALUES ( @NroExp, @Gestor, @Observaciones, @Modulo, @Fecha ) "
        Dim UpdateSQL As String = "Update [dbo].[NOTAS] set [NroExp] = @NroExp, [Gestor] = @Gestor, [Observaciones] = @Observaciones, [Modulo] = @Modulo, [Fecha] = @Fecha where [IdUnico] = @IdUnico "
        ' 
        If String.IsNullOrEmpty(ID) Then
            Command = New SqlCommand(InsertSQL, Connection)
        Else
            Command = New SqlCommand(UpdateSQL, Connection)
            ' 
            Command.Parameters.AddWithValue("@IdUnico", ID)

        End If
        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente").Trim)
        Command.Parameters.AddWithValue("@Gestor", Session("sscodigousuario"))
        Command.Parameters.AddWithValue("@Observaciones", txtObservaciones.Text)
        Command.Parameters.AddWithValue("@Modulo", Request("pModulo").Trim)

        If IsDate(txtFecha.Text.Trim) Then
            Command.Parameters.AddWithValue("@Fecha", Left(txtFecha.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@Fecha", DBNull.Value)

        End If
        ' 
        Command.ExecuteNonQuery()
        Connection.Close()
        ' 
        'Go to the Summary page 
        Response.Redirect("NOTAS.aspx?pExpediente=" & Request("pExpediente") & "&pModulo=" & Request("pModulo"))

    End Sub
End Class