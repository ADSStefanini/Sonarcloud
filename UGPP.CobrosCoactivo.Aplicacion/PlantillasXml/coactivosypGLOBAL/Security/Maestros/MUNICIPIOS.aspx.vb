﻿Imports System.Data.SqlClient
Imports System.Data

Partial Public Class MUNICIPIOS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboDialogdepartamento()
            BindGrid()

            'End If - if IsPostBack equals false
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
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
        sql = sql & "select [dbo].[MUNICIPIOS].*, DEPARTAMENTOSdepartamento.nombre as DEPARTAMENTOSdepartamentonombre from [dbo].[MUNICIPIOS] left join [DEPARTAMENTOS] DEPARTAMENTOSdepartamento on [dbo].[MUNICIPIOS].departamento = DEPARTAMENTOSdepartamento.codigo "
        Dim WhereClause As String = ""
        If txtSearchnombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[MUNICIPIOS].[nombre] like @nombre"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
            sql = sql & "where " & WhereClause

        End If

        If Len(Session("MUNICIPIOS.SortExpression")) > 0 Then
            sql = sql & " order by " & Session("MUNICIPIOS.SortExpression") & " " & Session("MUNICIPIOS.SortDirection")

        End If
        Return sql

    End Function

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Me.ID.Text


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
        Dim InsertSQL As String = "Insert into [dbo].[MUNICIPIOS] ([codigo], [nombre], [departamento] ) VALUES ( @codigo, @nombre, @departamento ) "

        ' 
        'Declare String UpdateSQL 
        Dim UpdateSQL As String = "Update [dbo].[MUNICIPIOS] set [codigo] = @codigo, [nombre] = @nombre, [departamento] = @departamento where [codigo] = @codigo "

        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            ' 
            'Create a new Command object for inserting a new record. 
            Command = New SqlCommand(InsertSQL, Connection)
            ' 
            'Set the @codigo field for updates. 
            ID = txtcodigo.Text.Trim
            Command.Parameters.AddWithValue("@codigo", ID)
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

        If txtdepartamento.Text.Length > 0 Then
            Command.Parameters.AddWithValue("@departamento", txtdepartamento.Text)
        Else
            Command.Parameters.AddWithValue("@departamento", DBNull.Value)

        End If

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Registro de municipios", "Código " & ID.Trim, Command)
        Catch ex As Exception

        End Try




        'Close the Connection Object 
        Connection.Close()
        ' 
        'Go to the Summary page 
        BindGrid()
        ' 
        'End event Save click 
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
            Dim sql As String = "select * from [dbo].[MUNICIPIOS] where [codigo] = @codigo"
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
                cboDialogdepartamento.SelectedValue = Reader("departamento").ToString()

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

        Select Case CStr(Session("MUNICIPIOS.SortDirection"))
            Case "ASC"
                Session("MUNICIPIOS.SortDirection") = "DESC"
            Case "DESC"
                Session("MUNICIPIOS.SortDirection") = "ASC"
            Case Else
                Session("MUNICIPIOS.SortDirection") = "ASC"
        End Select

        Session("MUNICIPIOS.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cmd As New Button
            cmd.UseSubmitBehavior = False
            cmd.Text = "Editar"
            cmd.Attributes.Add("class", "GridEditButton")
            cmd.Attributes.Add("onclick", "GridClick('" & e.Row.Cells(0).Text & "'); return false;")
            e.Row.Cells(e.Row.Cells.Count - 1).Controls.Add(cmd)

        End If
    End Sub

    ' 
    'Event handler for Delete clicks 
    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        If Len(ID.Text) > 0 Then

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()

            Dim sql As String = "delete from [dbo].[MUNICIPIOS] where [codigo] = @codigo"
            ' 
            'Declare SQLCommand Object named Command
            'Create a new Command object with a delete statement that will removed the current item being edited
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", ID.Text)
            ' 
            'Run the statement
            Try
                Command.ExecuteNonQuery()

                'Después de cada GRABAR hay que llamar al log de auditoria
                Dim LogProc As New LogProcesos
                LogProc.SaveLog(Session("ssloginusuario"), "Registro de municipios", "Código " & ID.Text.Trim, Command)

            Catch ex As Exception

            End Try



            'Close the Connection Object 
            Connection.Close()

        End If
        BindGrid()
    End Sub

    Protected Sub LoadcboDialogdepartamento()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [DEPARTAMENTOS] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboDialogdepartamento.DataTextField = "nombre"
        cboDialogdepartamento.DataValueField = "codigo"
        cboDialogdepartamento.DataSource = Command.ExecuteReader()
        cboDialogdepartamento.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub
End Class
