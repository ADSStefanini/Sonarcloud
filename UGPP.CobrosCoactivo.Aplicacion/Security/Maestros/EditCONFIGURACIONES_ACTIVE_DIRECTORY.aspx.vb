Imports System.Data.SqlClient
Partial Public Class EditCONFIGURACIONES_ACTIVE_DIRECTORY
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()

            Dim sql As String = "select * from [dbo].[CONFIGURACIONES_ACTIVE_DIRECTORY] where [idunico] = 1"
            ' 
            'Declare SQLCommand Object named Command
            'Create a new Command object with a select statement that will open the row referenced by Request("ID")
            Dim Command As New SqlCommand(sql, Connection)
            '            
            ' 
            'Declare a SqlDataReader Ojbect
            'Load it with the Command's select statement
            Dim Reader As SqlDataReader = Command.ExecuteReader
            ' 
            'If at least one record was found
            If Reader.Read Then
                txtservidor.Text = Reader("servidor").ToString()
                txtpuerto.Text = Reader("puerto").ToString()
                txtUsuarioServicio.Text = Reader("UsuarioServicio").ToString()
                txtClaveUsuarioServ.Text = Reader("ClaveUsuarioServ").ToString()
                If IsDBNull(Reader("usarAD")) = False Then
                    chkusarAD.Checked = Reader("usarAD").ToString()

                End If
            End If
            ' 
            'Close the Data Reader we are done with it.
            Reader.Close()

            'Close the Connection Object 
            Connection.Close()
        End If
    End Sub


    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")


        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()

        Dim Command As SqlCommand

        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update [dbo].[CONFIGURACIONES_ACTIVE_DIRECTORY] set [servidor] = @servidor, [puerto] = @puerto, [UsuarioServicio] = @UsuarioServicio, [ClaveUsuarioServ] = @ClaveUsuarioServ, [usarAD] = @usarAD where [idunico] = 1 "

        'Set the command object with the update sql and connection. 
        Command = New SqlCommand(UpdateSQL, Connection)
        ' 


        Command.Parameters.AddWithValue("@servidor", txtservidor.Text)

        If IsNumeric(txtpuerto.Text) Then
            Command.Parameters.AddWithValue("@puerto", txtpuerto.Text)
        Else
            Command.Parameters.AddWithValue("@puerto", DBNull.Value)

        End If

        Command.Parameters.AddWithValue("@UsuarioServicio", txtUsuarioServicio.Text)

        Command.Parameters.AddWithValue("@ClaveUsuarioServ", txtClaveUsuarioServ.Text)

        Command.Parameters.AddWithValue("@usarAD", chkusarAD.Checked)

        ' 
        'Run the sql command against the database with no return values 
        ' 
        'Run the statement
        Command.ExecuteNonQuery()

        'Close the Connection Object 
        Connection.Close()
        ' 
    End Sub

End Class