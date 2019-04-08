Imports System.Data.SqlClient

Partial Public Class EditMAESTRO_APODERADOS
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
                Dim sql As String = "select * from [dbo].[MAESTRO_APODERADOS] where [MAP_Codigo_Nit] = @MAP_Codigo_Nit"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @MAP_Codigo_Nit parameter in the Command select query
                Command.Parameters.AddWithValue("@MAP_Codigo_Nit", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtMAP_Codigo_Nit.Text = Reader("MAP_Codigo_Nit").ToString()
                    txtMAP_Nombre.Text = Reader("MAP_Nombre").ToString()
                    txtMAP_TarjetaProf.Text = Reader("MAP_TarjetaProf").ToString()

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
                'Since this is an insert then you can't delete it yet because it's not in the database.
                cmdDelete.Visible = False
                ' 
                'end of if Reader.Read 

            End If
            ' 
            'end of ID > 0

        End If
    End Sub

    ' 
    'Event handler for Delete clicks 
    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()

        Dim sql As String = "delete from [dbo].[MAESTRO_APODERADOS] where [MAP_Codigo_Nit] = @MAP_Codigo_Nit"
        ' 
        'Declare SQLCommand Object named Command
        'Create a new Command object with a delete statement that will removed the current item being edited
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@MAP_Codigo_Nit", Request("ID"))
        ' 
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Apoderados (borrar)", "Código " & Request("ID"), Command)
        Catch ex As Exception

        End Try

        'Close the Connection Object 
        Connection.Close()
        Response.Redirect("MAESTRO_APODERADOS.aspx")
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
        Dim InsertSQL As String = "Insert into [dbo].[MAESTRO_APODERADOS] ([MAP_Codigo_Nit], [MAP_Nombre], [MAP_TarjetaProf] ) VALUES ( @MAP_Codigo_Nit, @MAP_Nombre, @MAP_TarjetaProf ) "

        ' 
        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update [dbo].[MAESTRO_APODERADOS] set [MAP_Codigo_Nit] = @MAP_Codigo_Nit, [MAP_Nombre] = @MAP_Nombre, [MAP_TarjetaProf] = @MAP_TarjetaProf where [MAP_Codigo_Nit] = @MAP_Codigo_Nit "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' Insert
            'Create a new Command object for inserting a new record. 
            Command = New SqlCommand(InsertSQL, Connection)
            ' 
            ID = txtMAP_Codigo_Nit.Text.Trim
            Command.Parameters.AddWithValue("@MAP_Codigo_Nit", ID)
        Else
            ' 
            'Set the command object with the update sql and connection. 
            Command = New SqlCommand(UpdateSQL, Connection)
            ' 
            'Set the @MAP_Codigo_Nit field for updates. 
            Command.Parameters.AddWithValue("@MAP_Codigo_Nit", ID)
        End If

        Command.Parameters.AddWithValue("@MAP_Nombre", txtMAP_Nombre.Text)

        Command.Parameters.AddWithValue("@MAP_TarjetaProf", txtMAP_TarjetaProf.Text)

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Apoderados", "Código " & ID, Command)
        Catch ex As Exception

        End Try


        'Close the Connection Object 
        Connection.Close()
        ' 
        'Go to the Summary page 
        Response.Redirect("MAESTRO_APODERADOS.aspx")

        ' 
        'End event Save click 
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("MAESTRO_APODERADOS.aspx")
    End Sub
End Class
