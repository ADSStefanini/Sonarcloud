﻿Imports System.Data.SqlClient
Namespace Security.Maestros

    Partial Public Class EditConfiguracionDTF
        Inherits System.Web.UI.Page

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Evaluates to true when the page is loaded for the first time.
            If IsPostBack = False Then


                'Create a new connection to the database
                Dim connection As New SqlConnection(Funciones.CadenaConexion())

                'Opens a connection to the database.
                connection.Open()
                ' 
                'if Request("ID") > 0 then this is an edit
                'if Request("ID") = 0 then this is an insert
                If Len(Request("ID")) > 0 Then
                    Const sql As String = "select * from CONFIGURACION_DTF where [PERIODO] = @PERIODO"
                    ' 
                    'Declare SQLCommand Object named Command
                    'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                    Dim command As New SqlCommand(sql, connection)
                    ' 
                    ' 'Set the @consec parameter in the Command select query
                    command.Parameters.AddWithValue("@PERIODO", Request("ID"))
                    ' 
                    'Declare a SqlDataReader Ojbect
                    'Load it with the Command's select statement
                    Dim reader As SqlDataReader = command.ExecuteReader
                    ' 
                    'If at least one record was found
                    If reader.Read Then
                        txtperiodo.Text = CInt(reader("PERIODO").ToString())
                        txtdtf.Text = CDec(reader("DTF").ToString())

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
            Const insertSql As String = "Insert into CONFIGURACION_DTF ([PERIODO],  [DTF] ) VALUES ( @PERIODO,  @DTF) "

            ' 
            'Declare String UpdateSQL 
            Const updateSql As String = "Update CONFIGURACION_DTF set [DTF] = @DTF where [PERIODO] = @PERIODO"

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
                'command.Parameters.AddWithValue("@ANIO", value)


            End If
            If IsNumeric(txtperiodo.Text) Then
                command.Parameters.AddWithValue("@PERIODO", CInt(txtperiodo.Text))
            Else
                lblError.Text = "El campo PERIODO no es un número valido..."
                Exit Sub
            End If

            If IsNumeric(txtdtf.Text) Then
                command.Parameters.AddWithValue("@DTF", CDec(txtdtf.Text))
            Else
                lblError.Text = "El campo DTF  no es un numero valido ..."
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
            Response.Redirect("ConfiguracionDTF.aspx")

            ' 
            'End event Save click 
        End Sub


        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Response.Redirect("ConfiguracionDTF.aspx")
        End Sub

    End Class
End Namespace