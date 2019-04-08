Imports System.Data.SqlClient
Partial Public Class EditVERIFICACIONESPAGO
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
                Dim sql As String = "SELECT * from PAGOS where [NroConsignacion] = @NroConsignacion"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @NroConsignacion parameter in the Command select query
                Command.Parameters.AddWithValue("@NroConsignacion", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtNroConsignacion.Text = Reader("NroConsignacion").ToString()
                    'txtNroExp.Text = Reader("NroExp").ToString()
                    'txtFecSolverif.Text = Reader("FecSolverif").ToString()
                    'txtFecVerificado.Text = Reader("FecVerificado").ToString()
                    'cboestado.SelectedValue = Reader("estado").ToString()
                    'cboUserSolicita.SelectedValue = Reader("UserSolicita").ToString()
                    'cboUserVerif.SelectedValue = Reader("UserVerif").ToString()

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

            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim MTG As New MetodosGlobalesCobro
            Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    cmdSave.Visible = False
                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False
                End If
                
            End If
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "Insert INTO PAGOS ([NroConsignacion], [NroExp], [FecSolverif], [estado], [UserSolicita] ) VALUES ( @NroConsignacion, @NroExp, @FecSolverif, '01', @UserSolicita ) "
        Dim UpdateSQL As String = "Update PAGOS set [NroExp] = @NroExp, [FecSolverif] = @FecSolverif, [UserSolicita] = @UserSolicita where [NroConsignacion] = @NroConsignacion "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            'insert 
            Command = New SqlCommand(InsertSQL, Connection)
            ID = txtNroConsignacion.Text.Trim
            Command.Parameters.AddWithValue("@NroConsignacion", ID)            
        Else
            'update             
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroConsignacion", ID)
        End If

        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Command.Parameters.AddWithValue("@FecSolverif", Date.Now)                    
        Command.Parameters.AddWithValue("@UserSolicita", Session("sscodigousuario"))
       
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de pagos", "No. Consignación " & ID, Command)

        Catch ex As Exception

        End Try

        Connection.Close()

        'Enviar un nuevo mensaje al VERIFICADOR
        'RegistrarMensaje(Request("pExpediente"), Session("sscodigousuario"), "0020", "Ud tiene una nueva consignación / planilla para revisar. Es la " & ID, DateTime.Now)

        'Enviar mensaje a todos los VERIFICADORES DE PAGO
        '----------------------------------------------------------------------------------------
        Dim IdVerificador As String = ""
        Connection.Open()
        Dim sql As String = "SELECT DISTINCT USUARIOS.codigo FROM USUARIOS WHERE nivelacces = 6"        
        Dim Command2 As New SqlCommand(sql, Connection)
       
        Dim Reader As SqlDataReader = Command2.ExecuteReader
        While Reader.Read
            IdVerificador = Reader("codigo").ToString()
            RegistrarMensaje(Request("pExpediente"), Session("sscodigousuario"), IdVerificador, "Ud tiene una nueva consignación / planilla para revisar. Es la " & ID, DateTime.Now, ID)
        End While
        Reader.Close()
        Connection.Close()
        '----------------------------------------------------------------------------------------

        Response.Redirect("VERIFICACIONESPAGO.aspx?pExpediente=" & Request("pExpediente"))
    End Sub



    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("VERIFICACIONESPAGO.aspx?pExpediente=" & Request("pExpediente"))
    End Sub


    Private Sub RegistrarMensaje(ByVal pNroExp As String, ByVal pUsuOrigen As String, ByVal pUsuDestino As String, ByVal pMensaje As String, ByVal pFecEnvio As DateTime, ByVal pNroConsignacion As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        'Comandos SQL
        Dim InsertSQL As String = "Insert into MENSAJES ([NroExp], [UsuOrigen], [UsuDestino], [Mensaje], [FecEnvio], [NroConsignacion]  ) VALUES ( @NroExp, @UsuOrigen, @UsuDestino, @Mensaje, @FecEnvio, @NroConsignacion  ) "

        ' insert             
        Command = New SqlCommand(InsertSQL, Connection)
        ' 
        Command.Parameters.AddWithValue("@NroExp", pNroExp)
        Command.Parameters.AddWithValue("@UsuOrigen", pUsuOrigen)
        Command.Parameters.AddWithValue("@UsuDestino", pUsuDestino)
        Command.Parameters.AddWithValue("@Mensaje", pMensaje)
        Command.Parameters.AddWithValue("@FecEnvio", pFecEnvio)
        Command.Parameters.AddWithValue("@NroConsignacion", pNroConsignacion)

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Registro de mensajes", "Expediente " & pNroExp, Command)
        Catch ex As Exception

        End Try

        Connection.Close()
    End Sub
End Class