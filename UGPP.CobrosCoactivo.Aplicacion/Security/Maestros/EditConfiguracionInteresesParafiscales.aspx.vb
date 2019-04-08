Imports System.Data.SqlClient
Namespace Security.Maestros

    Partial Public Class EditConfiguracionInteresesParafiscales
        Inherits System.Web.UI.Page

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Evaluates to true when the page is loaded for the first time.
            If IsPostBack = False Then


                'Create a new connection to the database
                Dim connection As New SqlConnection(Funciones.CadenaConexion())

                'Opens a connection to the database.
                Connection.Open()
                ' 
                'if Request("ID") > 0 then this is an edit
                'if Request("ID") = 0 then this is an insert
                If Len(Request("ID")) > 0 Then
                    Const sql As String = "select * from [dbo].[CALCULO_INTERESES_PARAFISCALES] where [consec] = @consec"
                    ' 
                    'Declare SQLCommand Object named Command
                    'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                    Dim command As New SqlCommand(sql, connection)
                    ' 
                    ' 'Set the @consec parameter in the Command select query
                    Command.Parameters.AddWithValue("@consec", Request("ID"))
                    ' 
                    'Declare a SqlDataReader Ojbect
                    'Load it with the Command's select statement
                    Dim reader As SqlDataReader = command.ExecuteReader
                    ' 
                    'If at least one record was found
                    If Reader.Read Then
                        txtptrimestral.Text = CDec(reader("p_trimestral").ToString())
                        txtdesde.Text = CDate(reader("desde").ToString()).ToString("dd/MM/yyyy")
                        txthasta.Text = CDate(reader("hasta").ToString()).ToString("dd/MM/yyyy")
                        txttdiaria.Text = CDec(reader("t_diaria").ToString())

                    End If
                    ' 
                    'Close the Data Reader we are done with it.
                    Reader.Close()

                    'Close the Connection Object 
                    Connection.Close()
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
            Const insertSql As String = "Insert into [dbo].[CALCULO_INTERESES_PARAFISCALES] ([p_trimestral], [desde], [hasta], [t_diaria] ) VALUES ( @p_trimestral, @desde, @hasta, @t_diaria ) "

            ' 
            'Declare String UpdateSQL 
            Const updateSql As String = "Update [dbo].[CALCULO_INTERESES_PARAFISCALES] set [p_trimestral] = @p_trimestral, [desde] = @desde, [hasta] = @hasta, [t_diaria] = @t_diaria where [consec] = @consec "

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
                command.Parameters.AddWithValue("@consec", value)


            End If
            If IsNumeric(txtptrimestral.Text) Then
                command.Parameters.AddWithValue("@p_trimestral", CDec(txtptrimestral.Text))
            Else
                lblError.Text = "El campo tasa trimestral no es un número valido..."
                Exit Sub
            End If

            If IsNumeric(txttdiaria.Text) Then
                command.Parameters.AddWithValue("@t_diaria", CDec(txttdiaria.Text))
            Else
                lblError.Text = "El campo tasa diaria no es un número valido..."
                Exit Sub
            End If

            If IsDate(txtdesde.Text) Then
                command.Parameters.AddWithValue("@desde", CDate(txtdesde.Text).ToString("dd/MM/yyyy"))
            Else
                lblError.Text = "El campo fecha inicial no es una fecha valida..."
                Exit Sub
            End If


            If IsDate(txthasta.Text) Then
                command.Parameters.AddWithValue("@hasta", CDate(txthasta.Text).ToString("dd/MM/yyyy"))
            Else
                lblError.Text = "El campo fecha final no es una fecha valida..."
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
            Response.Redirect("ConfiguracionInteresesParafiscales.aspx")

            ' 
            'End event Save click 
        End Sub


        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Response.Redirect("ConfiguracionInteresesParafiscales.aspx")
        End Sub

        Protected Sub txtptrimestral_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtptrimestral.TextChanged
            txttdiaria.Text = CDec(txtptrimestral.Text) / 366
        End Sub
    End Class
End Namespace