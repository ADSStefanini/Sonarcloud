Imports System.Data.SqlClient
Imports System.DirectoryServices.Protocols

Partial Public Class sincronizarusuarios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnSincronizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSincronizar.Click
        'Instanciar clase MetodosGlobalesAD
        Dim MTG2 As New MetodosGlobalesAD
        MTG2.SincronizarUsuarios()

        Response.Write("fin")
    End Sub

End Class