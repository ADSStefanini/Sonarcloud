Imports System.Data.SqlClient

Partial Public Class EditMAESTRO_DEUDORSOLIDARIO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboMDS_TipoId()
            LoadcboMDS_TipoPersona()
            LoadcboMDS_EstadoPersona()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from MAESTRO_DEUDORSOLIDARIO where [MDS_Codigo_Nit] = @MDS_Codigo_Nit"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @MDS_Codigo_Nit parameter in the Command select query
                Command.Parameters.AddWithValue("@MDS_Codigo_Nit", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    cboMDS_TipoId.SelectedValue = Reader("MDS_TipoId").ToString()
                    txtMDS_Codigo_Nit.Text = Reader("MDS_Codigo_Nit").ToString()
                    txtMDS_DigitoVerificacion.Text = Reader("MDS_DigitoVerificacion").ToString()
                    cboMDS_TipoPersona.SelectedValue = Reader("MDS_TipoPersona").ToString()
                    txtMDS_Nombre.Text = Reader("MDS_Nombre").ToString()
                    cboMDS_EstadoPersona.SelectedValue = Reader("MDS_EstadoPersona").ToString()

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

  
    Protected Sub LoadcboMDS_TipoId()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "SELECT codigo, nombre FROM [TIPOS_IDENTIFICACION] WHERE ind_estado = 1 order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboMDS_TipoId.DataTextField = "nombre"
        cboMDS_TipoId.DataValueField = "codigo"
        cboMDS_TipoId.DataSource = Command.ExecuteReader()
        cboMDS_TipoId.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboMDS_TipoPersona()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "SELECT codigo, nombre FROM [TIPOS_PERSONA] WHERE ind_estado = 1 ORDER BY nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboMDS_TipoPersona.DataTextField = "nombre"
        cboMDS_TipoPersona.DataValueField = "codigo"
        cboMDS_TipoPersona.DataSource = Command.ExecuteReader()
        cboMDS_TipoPersona.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboMDS_EstadoPersona()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [ESTADOS_PERSONA] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboMDS_EstadoPersona.DataTextField = "nombre"
        cboMDS_EstadoPersona.DataValueField = "codigo"
        cboMDS_EstadoPersona.DataSource = Command.ExecuteReader()
        cboMDS_EstadoPersona.DataBind()

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

        Dim sql As String = "delete from [dbo].[MAESTRO_DEUDORSOLIDARIO] where [MDS_Codigo_Nit] = @MDS_Codigo_Nit"
        ' 
        'Declare SQLCommand Object named Command
        'Create a new Command object with a delete statement that will removed the current item being edited
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@MDS_Codigo_Nit", Request("ID"))
        ' 
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Deudores solidarios", "Código " & Request("ID"), Command)
        Catch ex As Exception

        End Try



        'Close the Connection Object 
        Connection.Close()
        Response.Redirect("MAESTRO_DEUDORSOLIDARIO.aspx")
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
        Dim InsertSQL As String = "Insert into [dbo].[MAESTRO_DEUDORSOLIDARIO] ([MDS_TipoId], [MDS_Codigo_Nit], [MDS_DigitoVerificacion], [MDS_TipoPersona], [MDS_Nombre], [MDS_EstadoPersona] ) VALUES ( @MDS_TipoId, @MDS_Codigo_Nit, @MDS_DigitoVerificacion, @MDS_TipoPersona, @MDS_Nombre, @MDS_EstadoPersona ) "

        ' 
        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update [dbo].[MAESTRO_DEUDORSOLIDARIO] set [MDS_TipoId] = @MDS_TipoId, [MDS_Codigo_Nit] = @MDS_Codigo_Nit, [MDS_DigitoVerificacion] = @MDS_DigitoVerificacion, [MDS_TipoPersona] = @MDS_TipoPersona, [MDS_Nombre] = @MDS_Nombre, [MDS_EstadoPersona] = @MDS_EstadoPersona where [MDS_Codigo_Nit] = @MDS_Codigo_Nit "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' 
            'Create a new Command object for inserting a new record. 
            Command = New SqlCommand(InsertSQL, Connection)
            ' 
            'Set the @MDS_Codigo_Nit field for updates. 
            ID = txtMDS_Codigo_Nit.Text.Trim
            Command.Parameters.AddWithValue("@MDS_Codigo_Nit", ID)
            ' 
            'We are doing an insert 
        Else
            ' 
            'Set the command object with the update sql and connection. 
            Command = New SqlCommand(UpdateSQL, Connection)
            ' 
            'Set the @MDS_Codigo_Nit field for updates. 
            Command.Parameters.AddWithValue("@MDS_Codigo_Nit", ID)

        End If
        If cboMDS_TipoId.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MDS_TipoId", cboMDS_TipoId.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MDS_TipoId", DBNull.Value)

        End If


        Command.Parameters.AddWithValue("@MDS_DigitoVerificacion", txtMDS_DigitoVerificacion.Text)

        If cboMDS_TipoPersona.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MDS_TipoPersona", cboMDS_TipoPersona.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MDS_TipoPersona", DBNull.Value)

        End If

        Command.Parameters.AddWithValue("@MDS_Nombre", txtMDS_Nombre.Text)

        If cboMDS_EstadoPersona.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@MDS_EstadoPersona", cboMDS_EstadoPersona.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@MDS_EstadoPersona", DBNull.Value)

        End If

        ' 
        'Run the sql command against the database with no return values 
        ' 
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Deudores solidarios", "Código " & ID, Command)
        Catch ex As Exception

        End Try


        'Close the Connection Object 
        Connection.Close()
        ' 
        'Go to the Summary page 
        Response.Redirect("MAESTRO_DEUDORSOLIDARIO.aspx")

        ' 
        'End event Save click 
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("MAESTRO_DEUDORSOLIDARIO.aspx")
    End Sub
End Class
