Imports System.Data.SqlClient

Partial Public Class EditOTRASRESOLUCIONES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboFormaNotif()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion())

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM OTRASRESOLUCIONES WHERE IdUnico = @IdUnico"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @IdUnico parameter in the Command select query
                Command.Parameters.AddWithValue("@IdUnico", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtNroExp.Text = Reader("NroExp").ToString()
                    txtNroResol.Text = Reader("NroResol").ToString()
                    txtFechaResol.Text = Left(Reader("FechaResol").ToString().Trim, 10)

                    cboFormaNotif.SelectedValue = Reader("FormaNotif").ToString()

                    txtFechaNotif.Text = Left(Reader("FechaNotif").ToString().Trim, 10)
                    txtNombreTipo.Text = Reader("NombreTipo").ToString()
                    txtObservaciones.Text = Reader("observaciones").ToString()
                End If

                Reader.Close()
                Connection.Close()
            Else
                'cmdDelete.Visible = False
                'INSERT
                Dim NroExp As String = ""
                NroExp = Request("pExpediente")
                txtNroExp.Text = NroExp.Trim

                'Manejo del consecutivo de resoluciones
                Dim NuevoConsec As Int64
                GenerarConsecutivoResoluciones(NuevoConsec)
                txtNroResol.Text = NuevoConsec
                txtFechaResol.Text = Date.Now.ToString("dd/MM/yyyy")
            End If
            ' 
            'end of ID > 0

        End If
    End Sub

    Protected Sub LoadcboFormaNotif()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion())

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [FORMAS_NOTIFICACION] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboFormaNotif.DataTextField = "nombre"
        cboFormaNotif.DataValueField = "codigo"
        cboFormaNotif.DataSource = Command.ExecuteReader()
        cboFormaNotif.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    ' 
    'Event handler for Delete clicks 
    'Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

    '    'Create a new connection to the database
    '    Dim Connection As New SqlConnection(Funciones.CadenaConexion())

    '    'Opens a connection to the database.
    '    Connection.Open()

    '    Dim sql As String = "delete from [dbo].[OTRASRESOLUCIONES] where [IdUnico] = @IdUnico"
    '    ' 
    '    'Declare SQLCommand Object named Command
    '    'Create a new Command object with a delete statement that will removed the current item being edited
    '    Dim Command As New SqlCommand(sql, Connection)
    '    Command.Parameters.AddWithValue("@IdUnico", Request("ID"))
    '    ' 
    '    'Run the statement
    '    Command.ExecuteNonQuery()


    '    'Close the Connection Object 
    '    Connection.Close()
    '    Response.Redirect("OTRASRESOLUCIONES.aspx")
    'End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        'Validar el tipo de resolucion
        If txtNombreTipo.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el tipo de resolución, por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        Dim ID As String = Request("ID")
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion())
        Connection.Open()
        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        ' 
        'Declare string SQL 
        Dim InsertSQL As String = "Insert into OTRASRESOLUCIONES ([NroExp], [NroResol], [FechaResol], [FormaNotif], [FechaNotif], [NombreTipo], [observaciones] ) VALUES ( @NroExp, @NroResol, @FechaResol, @FormaNotif, @FechaNotif, @NombreTipo, @observaciones ) "
        Dim UpdateSQL As String = "Update OTRASRESOLUCIONES set [FormaNotif] = @FormaNotif, [FechaNotif] = @FechaNotif, [NombreTipo] = @NombreTipo, [observaciones] = @observaciones where [IdUnico] = @IdUnico "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' INSERT          
            Command = New SqlCommand(InsertSQL, Connection)

            Dim NuevoConsec As Int64
            GenerarConsecutivoResoluciones(NuevoConsec)
            Command.Parameters.AddWithValue("@NroExp", txtNroExp.Text)
            Command.Parameters.AddWithValue("@NroResol", NuevoConsec.ToString.Trim)
            Command.Parameters.AddWithValue("@FechaResol", Date.Now.ToString("dd/MM/yyyy"))
        Else
            ' UPDATE            
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@IdUnico", ID)

        End If

        'Command.Parameters.AddWithValue("@NroExp", txtNroExp.Text)

        'Command.Parameters.AddWithValue("@NroResol", txtNroResol.Text)

        'If IsDate(txtFechaResol.Text) Then
        '    Command.Parameters.AddWithValue("@FechaResol", txtFechaResol.Text)
        'Else
        '    Command.Parameters.AddWithValue("@FechaResol", DBNull.Value)

        'End If

        If cboFormaNotif.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@FormaNotif", cboFormaNotif.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@FormaNotif", DBNull.Value)
        End If

        If IsDate(txtFechaNotif.Text) Then
            Command.Parameters.AddWithValue("@FechaNotif", txtFechaNotif.Text)
        Else
            Command.Parameters.AddWithValue("@FechaNotif", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NombreTipo", txtNombreTipo.Text)
        Command.Parameters.AddWithValue("@observaciones", txtObservaciones.Text)

        Try
            'Run the statement
            Command.ExecuteNonQuery()
            Connection.Close()

            If String.IsNullOrEmpty(ID) Then
                ActualizarConsecutivoResoluciones()
            End If

            'Go to the Summary page 
            Response.Redirect("OTRASRESOLUCIONES.aspx?pExpediente=" & txtNroExp.Text.Trim)
        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            'Return
        End Try

    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("OTRASRESOLUCIONES.aspx?pExpediente=" & txtNroExp.Text.Trim)
    End Sub

    Private Sub GenerarConsecutivoResoluciones(ByRef pNuevoConsec As Int64)
        'Dim Consecutivo As Int64 = 0
        pNuevoConsec = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT CON_USER FROM MAESTRO_CONSECUTIVOS WHERE CON_IDENTIFICADOR = 999"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            pNuevoConsec = Convert.ToInt64(Reader("CON_USER").ToString().Trim) + 1
            '
            'txtNroResol.Text = Consecutivo
            'txtFechaResol.Text = Date.Now.ToString("dd/MM/yyyy")
        End If
        Reader.Close()
        Connection.Close()
    End Sub

    Private Sub ActualizarConsecutivoResoluciones()
        ' Incrementar en uno el consecutivo
        Dim Connection As New SqlConnection(Funciones.CadenaConexion())
        Connection.Open()
        Dim UpdateSQL As String = "UPDATE MAESTRO_CONSECUTIVOS SET CON_USER = CON_USER + 1 WHERE CON_IDENTIFICADOR = 999 "

        Dim Command As SqlCommand
        Command = New SqlCommand(UpdateSQL, Connection)
        'Try           
        Command.ExecuteNonQuery()
        Connection.Close()
        'Catch ex As Exception
        '    CustomValidator1.Text = ex.Message
        '    CustomValidator1.IsValid = False
        '    'Return
        'End Try
    End Sub
End Class