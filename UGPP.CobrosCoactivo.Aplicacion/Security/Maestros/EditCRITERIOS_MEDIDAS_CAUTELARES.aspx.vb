Imports System.Data.SqlClient

Partial Public Class EditCRITERIOS_MEDIDAS_CAUTELARES
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
                Dim sql As String = "select * from [dbo].[CRITERIOS_MEDIDAS_CAUTELARES] where [codigo] = @codigo"
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

                Dim sql As String = "select MAX(codigo) codigo from CRITERIOS_MEDIDAS_CAUTELARES"
                Dim Command As New SqlCommand(sql, Connection)
                Dim Reader As SqlDataReader = Command.ExecuteReader
                If Reader.Read Then
                    If Reader("codigo").ToString() = "" Then
                        txtcodigo.Text = "01"
                    Else
                        txtcodigo.Text = CInt(Reader("codigo").ToString()) + 1
                        If Len(txtcodigo.Text) = 1 Then
                            txtcodigo.Text = "0" & txtcodigo.Text
                        End If
                    End If

                Else
                    txtcodigo.Text = "01"
                End If
                ' 
                'Close the Data Reader we are done with it.
                Reader.Close()

                'Close the Connection Object 
                Connection.Close()

                ' 
                'Since this is an insert then you can't delete it yet because it's not in the database.
                'cmdDelete.Visible = False
                ' 
                'end of if Reader.Read 

            End If

            ' 
            'end of ID > 0

        End If
    End Sub

    ' 
    'Event handler for Delete clicks 
    'Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

    '    'Create a new connection to the database
    '    Dim Connection As New SqlConnection(Funciones.CadenaConexion)

    '    'Opens a connection to the database.
    '    Connection.Open()

    '    Dim sql As String = "delete from CRITERIOS_MEDIDAS_CAUTELARES where [codigo] = @codigo"
    '    ' 
    '    'Declare SQLCommand Object named Command
    '    'Create a new Command object with a delete statement that will removed the current item being edited
    '    Dim Command As New SqlCommand(sql, Connection)
    '    Command.Parameters.AddWithValue("@codigo", Request("ID"))

    '    ' 
    '    Try
    '        Command.ExecuteNonQuery()
    '        'Después de cada GRABAR hay que llamar al log de auditoria
    '        Dim LogProc As New LogProcesos
    '        LogProc.SaveLog(Session("ssloginusuario"), "Tipos de títulos", "Código " & Request("ID"), Command)
    '    Catch ex As Exception

    '    End Try

    '    'Close the Connection Object 
    '    Connection.Close()
    '    Response.Redirect("CRITERIOS_MEDIDAS_CAUTELARES.aspx")
    'End Sub

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
        Dim InsertSQL As String = "Insert into [dbo].[CRITERIOS_MEDIDAS_CAUTELARES] ([codigo], [nombre] ) VALUES ( @codigo, @nombre ) "
        Dim UpdateSQL As String = "Update [dbo].[CRITERIOS_MEDIDAS_CAUTELARES] set [nombre] = @nombre where [codigo] = @codigo "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' insert
            Command = New SqlCommand(InsertSQL, Connection)
            ID = txtcodigo.Text.Trim
            Command.Parameters.AddWithValue("@codigo", ID)
        Else
            ' update            
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@codigo", ID)
        End If

        Command.Parameters.AddWithValue("@nombre", txtnombre.Text)

        ' 
        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "CRITERIOS MEDIDAS CAUTELARES", "Código " & ID, Command)

        Catch ex As Exception

        End Try


        'Close the Connection Object 
        Connection.Close()
        ' 
        'Go to the Summary page 
        Response.Redirect("CRITERIOS_MEDIDAS_CAUTELARES.aspx")


    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("CRITERIOS_MEDIDAS_CAUTELARES.aspx")
    End Sub
End Class
