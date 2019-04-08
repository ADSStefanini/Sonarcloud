Imports System.Data.SqlClient

Partial Public Class EditMAESTRO_REPRESENTANTESLEGALES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboMRL_TipoId()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from [dbo].[MAESTRO_REPRESENTANTESLEGALES] where [MRL_Codigo_Nit] = @MRL_Codigo_Nit"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @MRL_Codigo_Nit parameter in the Command select query
                Command.Parameters.AddWithValue("@MRL_Codigo_Nit", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    cboMRL_TipoId.SelectedValue = Reader("MRL_TipoId").ToString()
                    txtMRL_Codigo_Nit.Text = Reader("MRL_Codigo_Nit").ToString()
                    txtMRL_Nombre.Text = Reader("MRL_Nombre").ToString()

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

    Protected Sub LoadcboMRL_TipoId()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [TIPOS_IDENTIFICACION] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboMRL_TipoId.DataTextField = "nombre"
        cboMRL_TipoId.DataValueField = "codigo"
        cboMRL_TipoId.DataSource = Command.ExecuteReader()
        cboMRL_TipoId.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub


    ' 
    'Event handler for Delete clicks 
    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()

        Dim sql As String = "delete from [dbo].[MAESTRO_REPRESENTANTESLEGALES] where [MRL_Codigo_Nit] = @MRL_Codigo_Nit"
        ' 
        'Declare SQLCommand Object named Command
        'Create a new Command object with a delete statement that will removed the current item being edited
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@MRL_Codigo_Nit", Request("ID"))
        ' 
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Representantes legales", "Código " & Request("ID"), Command)
        Catch ex As Exception

        End Try



        'Close the Connection Object 
        Connection.Close()
        Response.Redirect("MAESTRO_REPRESENTANTESLEGALES.aspx")
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ''''Handles cmdSave.Click

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
        Dim InsertSQL As String = "Insert into [dbo].[MAESTRO_REPRESENTANTESLEGALES] ([MRL_TipoId], [MRL_Codigo_Nit], [MRL_Nombre] ) VALUES ( @MRL_TipoId, @MRL_Codigo_Nit, @MRL_Nombre ) "

        ' 
        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update [dbo].[MAESTRO_REPRESENTANTESLEGALES] set [MRL_TipoId] = @MRL_TipoId, [MRL_Codigo_Nit] = @MRL_Codigo_Nit, [MRL_Nombre] = @MRL_Nombre where [MRL_Codigo_Nit] = @MRL_Codigo_Nit "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' 
            'Create a new Command object for inserting a new record. 
            Command = New SqlCommand(InsertSQL, Connection)
            ' 
            'Set the @MRL_Codigo_Nit field for updates. 
            ID = txtMRL_Codigo_Nit.Text.Trim
            Command.Parameters.AddWithValue("@MRL_Codigo_Nit", ID)
            ' 
            'We are doing an insert 
        Else
            ' 
            'Set the command object with the update sql and connection. 
            Command = New SqlCommand(UpdateSQL, Connection)
            ' 
            'Set the @MRL_Codigo_Nit field for updates. 
            Command.Parameters.AddWithValue("@MRL_Codigo_Nit", ID)

        End If
        If cboMRL_TipoId.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MRL_TipoId", cboMRL_TipoId.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MRL_TipoId", DBNull.Value)

        End If


        Command.Parameters.AddWithValue("@MRL_Nombre", txtMRL_Nombre.Text)

        ' 
        'Run the sql command against the database with no return values 
        ' 
        'Run the statement
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Representantes legales", "Código " & ID, Command)

            'Close the Connection Object 
            Connection.Close()
            'Go to the Summary page 
            Response.Redirect("MAESTRO_REPRESENTANTESLEGALES.aspx")
        Catch ex As Exception
            Dim MensajeValidacion As String = ex.Message
            MostrarMensaje()
            'Dim script As String = "<script type='text/javascript'>alert('" & MensajeValidacion & "');</script>"
            'ScriptManager.RegisterStartupScript(Me, GetType(Page), "alerta", script, False)
            'Close the Connection Object 
            Connection.Close()
        End Try
    End Sub

    Private Sub MostrarMensaje()
        'test
        Dim MsjValidacion As String = "No se pudo guardar la información del representante legal actual."
        Dim script As String = "<script type='text/javascript'>alert('" & MsjValidacion & "');</script>"
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "alerta", script, False)
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("MAESTRO_REPRESENTANTESLEGALES.aspx")
    End Sub
End Class
