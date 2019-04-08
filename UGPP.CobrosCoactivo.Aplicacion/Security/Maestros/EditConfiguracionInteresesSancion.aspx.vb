Imports System.Data.SqlClient
Namespace Security.Maestros

    Partial Public Class EditConfiguracionInteresesSancion
        Inherits System.Web.UI.Page

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Evaluates to true when the page is loaded for the first time.
            If IsPostBack = False Then

                lblNomPerfil.Text = Session("ssnombreusuario") & " ). ( " & CommonsCobrosCoactivos.getNomPerfil(Session) & " )"

                'Create a new connection to the database
                Dim connection As New SqlConnection(Funciones.CadenaConexion())

                'Opens a connection to the database.
                connection.Open()
                ' 
                'if Request("ID") > 0 then this is an edit
                'if Request("ID") = 0 then this is an insert
                If Len(Request("ID")) > 0 Then
                    Const sql As String = "SELECT * FROM PORCENTAJE_TASA_MULTA where [id] = @id"
                    ' 
                    'Declare SQLCommand Object named Command
                    'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                    Dim command As New SqlCommand(sql, connection)
                    ' 
                    ' 'Set the @consec parameter in the Command select query
                    command.Parameters.AddWithValue("@id", Request("ID"))
                    ' 
                    'Declare a SqlDataReader Ojbect
                    'Load it with the Command's select statement
                    Dim reader As SqlDataReader = command.ExecuteReader
                    ' 
                    'If at least one record was found
                    If reader.Read Then
                        txtpanual.Text = CDec(reader("p_anual").ToString())
                        txtpmensual.Text = CDec(reader("p_mensual").ToString())

                    End If
                    ' 
                    'Close the Data Reader we are done with it.
                    reader.Close()

                    'Close the Connection Object 
                    connection.Close()
                    ' 
                    'The length of ID equals zero.
                    'This is an insert so don't preload any data.
                End If

                ' 
                'end of ID > 0

            End If
        End Sub

        Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

            Dim value As String = Request("ID")


            'Create a new connection to the database
            Dim connection As New SqlConnection(Funciones.CadenaConexion())

            'Opens a connection to the database.
            connection.Open()

            ' 
            'Declare SqlCommand Object named Command 
            'To be used to invoke the Update or Insert statements 
            Dim command As SqlCommand

            ' 
            'Declare string InsertSQL 
            Const insertSql As String = "Insert into [dbo].[PORCENTAJE_TASA_MULTA] ([p_anual],[p_mensual] ) VALUES ( @p_anual, @p_mensual ) "

            ' 
            'Declare String UpdateSQL 
            Const updateSql As String = "Update [dbo].[PORCENTAJE_TASA_MULTA] set [p_anual] = @p_anual,  [p_mensual] = @p_mensual where [id] = @id "

            ' 
            'if ID > 0 run the update 
            'if ID = 0 run the Insert
            If String.IsNullOrEmpty(value) Then
                ' 
                'Create a new Command object for inserting a new record. 
                command = New SqlCommand(insertSql, connection)
                ' 
                'We are doing an insert 
            Else
                ' 
                'Set the command object with the update sql and connection. 
                command = New SqlCommand(updateSql, connection)
                ' 
                'Set the @consec field for updates. 
                command.Parameters.AddWithValue("@id", value)


            End If
            If IsNumeric(txtpanual.Text) Then
                command.Parameters.AddWithValue("@p_anual", CDec(txtpanual.Text))
            Else
                lblError.Text = "El campo tasa anual no es un número valido..."
                Exit Sub
            End If

            If IsNumeric(txtpmensual.Text) Then
                command.Parameters.AddWithValue("@p_mensual", CDec(txtpmensual.Text))
            Else
                lblError.Text = "El campo tasa mensual no es un número valido..."
                Exit Sub
            End If

            ' 
            'Run the sql command against the database with no return values 
            ' 
            'Run the statement
            command.ExecuteNonQuery()

            'Close the Connection Object 
            connection.Close()
            ' 
            'Go to the Summary page 
            Response.Redirect("ConfiguracionInteresesSancion.aspx")

            ' 
            'End event Save click 
        End Sub


        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Response.Redirect("ConfiguracionInteresesSancion.aspx")
        End Sub

        Protected Sub txtpanual_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtpanual.TextChanged
            txtpmensual.Text = CDec(txtpanual.Text) / 12
        End Sub
    End Class
End Namespace