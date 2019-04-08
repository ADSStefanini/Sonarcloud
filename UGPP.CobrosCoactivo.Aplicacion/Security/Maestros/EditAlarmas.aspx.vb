Imports System.Data.SqlClient

Partial Public Class EditAlarmas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM alarmas WHERE codigo = @codigo"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @codigo parameter in the Command select query
                Command.Parameters.AddWithValue("@codigo", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtcodigo.Text = Reader("codigo").ToString()
                    txtnombre.Text = Reader("nombre").ToString()
                    txttextoactuarpronto.Text = Reader("textoactuarpronto").ToString()
                    txttextosinaccion.Text = Reader("textosinaccion").ToString()
                    txtdiasactuarprontoINI.Text = Reader("diasactuarprontoINI").ToString()
                    txtdiasactuarprontoFIN.Text = Reader("diasactuarprontoFIN").ToString()
                    txtdiassinaccionINI.Text = Reader("diassinaccionINI").ToString()
                    txtdiassinaccionFIN.Text = Reader("diassinaccionFIN").ToString()
                    txtdiasescalamiento.Text = Reader("diasescalamiento").ToString()
                    txtobservaciones.Text = Reader("observaciones").ToString()

                End If
                ' 
                'Close the Data Reader we are done with it.
                Reader.Close()

                'Close the Connection Object 
                Connection.Close()
                ' 
                'The length of ID equals zero.
                'This is an insert so don't preload any data.
            Else
                '
            End If

        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        ' 
        'Declare string InsertSQL 
        'Dim InsertSQL As String = "Insert into [dbo].[ALARMAS] ([codigo], [nombre], [textoactuarpronto], [textosinaccion], [diasactuarprontoINI], [diasactuarprontoFIN], [diassinaccionINI], [diassinaccionFIN], [diasescalamiento], [observaciones] ) VALUES ( @codigo, @nombre, @textoactuarpronto, @textosinaccion, @diasactuarprontoINI, @diasactuarprontoFIN, @diassinaccionINI, @diassinaccionFIN, @diasescalamiento, @observaciones ) "

        ' 
        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update ALARMAS set [nombre] = @nombre, [textoactuarpronto] = @textoactuarpronto, [textosinaccion] = @textosinaccion, [diasactuarprontoINI] = @diasactuarprontoINI, [diasactuarprontoFIN] = @diasactuarprontoFIN, [diassinaccionINI] = @diassinaccionINI, [diassinaccionFIN] = @diassinaccionFIN, [diasescalamiento] = @diasescalamiento, [observaciones] = @observaciones where [codigo] = @codigo "


        '---------------------------------------------------------------
        'Set the command object with the update sql and connection. 
        Command = New SqlCommand(UpdateSQL, Connection)
        ' 
        'Set the @codigo field for updates. 
        Command.Parameters.AddWithValue("@codigo", ID)
        '---------------------------------------------------------------

        Command.Parameters.AddWithValue("@nombre", txtnombre.Text)

        Command.Parameters.AddWithValue("@textoactuarpronto", txttextoactuarpronto.Text)

        Command.Parameters.AddWithValue("@textosinaccion", txttextosinaccion.Text)

        If IsNumeric(txtdiasactuarprontoINI.Text) Then
            Command.Parameters.AddWithValue("@diasactuarprontoINI", txtdiasactuarprontoINI.Text)
        Else
            Command.Parameters.AddWithValue("@diasactuarprontoINI", DBNull.Value)

        End If

        If IsNumeric(txtdiasactuarprontoFIN.Text) Then
            Command.Parameters.AddWithValue("@diasactuarprontoFIN", txtdiasactuarprontoFIN.Text)
        Else
            Command.Parameters.AddWithValue("@diasactuarprontoFIN", DBNull.Value)

        End If

        If IsNumeric(txtdiassinaccionINI.Text) Then
            Command.Parameters.AddWithValue("@diassinaccionINI", txtdiassinaccionINI.Text)
        Else
            Command.Parameters.AddWithValue("@diassinaccionINI", DBNull.Value)

        End If

        If IsNumeric(txtdiassinaccionFIN.Text) Then
            Command.Parameters.AddWithValue("@diassinaccionFIN", txtdiassinaccionFIN.Text)
        Else
            Command.Parameters.AddWithValue("@diassinaccionFIN", DBNull.Value)

        End If

        If IsNumeric(txtdiasescalamiento.Text) Then
            Command.Parameters.AddWithValue("@diasescalamiento", txtdiasescalamiento.Text)
        Else
            Command.Parameters.AddWithValue("@diasescalamiento", DBNull.Value)

        End If

        Command.Parameters.AddWithValue("@observaciones", txtobservaciones.Text)

        ' 
        'Run the sql command against the database with no return values 
        ' 
        'Run the statement
        Command.ExecuteNonQuery()

        'Close the Connection Object 
        Connection.Close()
        ' 
        'Go to the Summary page 
        Response.Redirect("ALARMAS.aspx")

        ' 
        'End event Save click 
    End Sub


    'Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    '    Response.Redirect("ALARMAS.aspx")
    'End Sub

End Class