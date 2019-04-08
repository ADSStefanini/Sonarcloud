Imports System.Data.SqlClient
Partial Public Class obtenerNombreRevisor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim NomSuperior As String = ""

        If Len(Request("pIdGestor")) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT nombre FROM usuarios WHERE codigo = (SELECT superior FROM usuarios WHERE codigo = @codigo)"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", Request("pIdGestor").Trim)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                NomSuperior = Reader("nombre").ToString().Trim
            End If
            Reader.Close()
            Connection.Close()
        End If
        
        Response.Write(NomSuperior)
    End Sub

End Class