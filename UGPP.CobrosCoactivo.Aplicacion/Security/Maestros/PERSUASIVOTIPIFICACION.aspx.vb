Imports System.Data.SqlClient

Partial Public Class PERSUASIVOTIPIFICACION
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            BindGrid()            
        End If
    End Sub

    Private Sub BindGrid()
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grd.Rows.Count
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""
        sql = sql & "select * FROM PERSUASIVOTIPIFICACION"
        Return sql

    End Function

    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditPERSUASIVOTIPIFICACION.aspx")
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim codigo As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditPERSUASIVOTIPIFICACION.aspx?ID=" & codigo)
        End If
    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
    End Sub

End Class