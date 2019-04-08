﻿Imports System.Data.SqlClient
Imports System.Data

Partial Public Class DEPARTAMENTOS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            BindGrid()


            'End If - if IsPostBack equals false
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()

        'Create a new connection to the database
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Dim conexion As String = Funciones.CadenaConexion
        Dim connection As New SqlConnection(Funciones.CadenaConexion)


        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@codigo", "%" & txtSearchcodigo.Text)

        Command.Parameters.AddWithValue("@nombre", "%" & txtSearchnombre.Text & "%")

        Dim DataAdapter As New SqlDataAdapter(Command)

        Dim DataSet As New DataSet

        DataAdapter.Fill(DataSet)
        grd.DataSource = DataSet
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""
        sql = sql & "select [dbo].[DEPARTAMENTOS].* from [dbo].[DEPARTAMENTOS]"
        Dim WhereClause As String = ""
        If txtSearchcodigo.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[DEPARTAMENTOS].[codigo] like @codigo"

        End If

        If txtSearchnombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[DEPARTAMENTOS].[nombre] like @nombre"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
            sql = sql & "where " & WhereClause

        End If

        If Len(Session("DEPARTAMENTOS.SortExpression")) > 0 Then
            sql = sql & " order by " & Session("DEPARTAMENTOS.SortExpression") & " " & Session("DEPARTAMENTOS.SortDirection")

        End If
        Return sql

    End Function

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Me.ID.Text


        'Create a new connection to the database
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Dim connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()

        ' 
        'Declare SqlCommand Object named Command 
        'To be used to invoke the Update or Insert statements 
        Dim Command As SqlCommand

        ' 
        'Declare string InsertSQL 
        Dim InsertSQL As String = "Insert into [dbo].[DEPARTAMENTOS] ([codigo], [nombre] ) VALUES ( @codigo, @nombre ) "

        ' 
        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update DEPARTAMENTOS set nombre = @nombre where [codigo] = @codigo "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' 
            'Create a new Command object for inserting a new record. 
            Command = New SqlCommand(InsertSQL, Connection)
            ' 
            'Set the @codigo field for updates. 
            'Command.Parameters.AddWithValue("@codigo", ID)
            Command.Parameters.AddWithValue("@codigo", txtcodigo.Text.Trim)
            ' 
            'We are doing an insert 
        Else
            ' 
            'Set the command object with the update sql and connection. 
            Command = New SqlCommand(UpdateSQL, Connection)
            ' 
            'Set the @codigo field for updates. 
            Command.Parameters.AddWithValue("@codigo", ID)

        End If

        Command.Parameters.AddWithValue("@nombre", txtnombre.Text)

        '
        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de maestro de departamentos", "Dpto " & ID, Command)
        Catch ex As Exception

        End Try


        'Close the Connection Object 
        Connection.Close()        
        BindGrid()

    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        BindGrid()
    End Sub


    Protected Sub cmdLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLoad.Click

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        ' 
        'if ID.Text > 0 then this is an edit
        'if ID.Text = 0 then this is an insert
        If Len(ID.Text) > 0 Then
            Dim sql As String = "select * from [dbo].[DEPARTAMENTOS] where [codigo] = @codigo"
            ' 
            'Declare SQLCommand Object named Command
            'Create a new Command object with a select statement that will open the row referenced by Request("ID")
            Dim Command As New SqlCommand(sql, Connection)
            ' 
            ' 'Set the @codigo parameter in the Command select query
            Command.Parameters.AddWithValue("@codigo", ID.Text)
            ' 
            'Declare a SqlDataReader Ojbect
            'Load it with the Command's select statement
            Dim Reader As SqlDataReader = Command.ExecuteReader
            ' 
            'If at least one record was found
            If Reader.Read Then
                txtDialogcodigo.Text = Reader("codigo").ToString()
                txtDialognombre.Text = Reader("nombre").ToString()

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
    End Sub

    Protected Sub grd_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grd.PageIndexChanging
        grd.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("DEPARTAMENTOS.SortDirection"))
            Case "ASC"
                Session("DEPARTAMENTOS.SortDirection") = "DESC"
            Case "DESC"
                Session("DEPARTAMENTOS.SortDirection") = "ASC"
            Case Else
                Session("DEPARTAMENTOS.SortDirection") = "ASC"
        End Select

        Session("DEPARTAMENTOS.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cmd As New Button
            cmd.UseSubmitBehavior = False
            cmd.Text = "Editar"
            cmd.Attributes.Add("class", "GridEditButton")
            cmd.Attributes.Add("onclick", "GridClick('" & e.Row.Cells(0).Text.Trim & "'); return false;")
            e.Row.Cells(e.Row.Cells.Count - 1).Controls.Add(cmd)

        End If
    End Sub

    'Event handler for Delete clicks 
    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        If Len(ID.Text) > 0 Then

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()

            Dim sql As String = "delete from [dbo].[DEPARTAMENTOS] where [codigo] = @codigo"
            ' 
            'Declare SQLCommand Object named Command
            'Create a new Command object with a delete statement that will removed the current item being edited
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", ID.Text)
            ' 
            'Run the statement
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de maestro de departamentos", "Dpto " & ID.Text.Trim, Command)

            'Close the Connection Object 
            Connection.Close()

        End If
        BindGrid()
    End Sub

End Class
