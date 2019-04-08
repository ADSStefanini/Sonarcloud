Imports System.Data.SqlClient

Partial Public Class EditPROMOTORES
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
                Dim sql As String = "select * from PROMOTORES where [cedula] = @cedula"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @cedula parameter in the Command select query
                Command.Parameters.AddWithValue("@cedula", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtcedula.Text = Reader("cedula").ToString()
                    txtnombre.Text = Reader("nombre").ToString()
                    txtdireccion.Text = Reader("direccion").ToString()
                    txttelefono.Text = Reader("telefono").ToString()
                    txtobservac.Text = Reader("observac").ToString()

                End If
                ' 
                'Close the Data Reader we are done with it.
                Reader.Close()
                'Close the Connection Object 
                Connection.Close()               
            Else                 
                'end of if Reader.Read 
            End If
            ' 
            'end of ID > 0
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
        Dim InsertSQL As String = "Insert into PROMOTORES ([cedula], [nombre], [direccion], [telefono], [observac] ) VALUES ( @cedula, @nombre, @direccion, @telefono, @observac ) "

        ' 
        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update PROMOTORES set [nombre] = @nombre, [direccion] = @direccion, [telefono] = @telefono, [observac] = @observac where [cedula] = @cedula "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' insert            
            Command = New SqlCommand(InsertSQL, Connection)
            ID = txtcedula.Text.Trim
            Command.Parameters.AddWithValue("@cedula", ID)
        Else
            '             
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@cedula", ID)
        End If

        Command.Parameters.AddWithValue("@nombre", txtnombre.Text.Trim.ToUpper)
        Command.Parameters.AddWithValue("@direccion", txtdireccion.Text.Trim.ToUpper)
        Command.Parameters.AddWithValue("@telefono", txttelefono.Text.Trim)
        Command.Parameters.AddWithValue("@observac", txtobservac.Text.Trim.ToUpper)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Registro de promotores", "Cédula / NIT " & ID.Trim, Command)

        Catch ex As Exception

        End Try


        Connection.Close()
        ' 
        Response.Redirect("PROMOTORES.aspx")
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("PROMOTORES.aspx")
    End Sub

End Class