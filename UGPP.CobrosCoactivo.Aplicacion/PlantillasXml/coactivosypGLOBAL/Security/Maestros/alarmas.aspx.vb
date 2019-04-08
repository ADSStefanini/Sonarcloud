Imports System.Data.SqlClient

Partial Public Class alarmas
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            BindGrid()

            lblNomPerfil.Text = GetNomPerfil(Session("sscodigousuario"))
        End If
    End Sub

    Private Sub BindGrid()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""
        sql = sql & "select * from ALARMAS"
        Return sql

    End Function

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim codigo As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditALARMAS.aspx?ID=" & codigo)

        End If
    End Sub

    Protected Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click
        '
        Response.Redirect("../MenuMaestros.aspx")
    End Sub

    Private Function GetNomPerfil(ByVal pUsuario As String) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & Session("mnivelacces")

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        Return NomPerfil
    End Function

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()
        Response.Redirect("../../login.aspx")
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        Session.RemoveAll()
    End Sub

End Class