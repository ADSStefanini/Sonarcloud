Imports System.Data.SqlClient
Partial Public Class AjaxGetEstadoProceso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim IdEstadoExpediente As String = " "

        If Len(Request("pExpediente")) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT efiestado FROM ejefisglobal WHERE efinroexp = @efinroexp"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@efinroexp", Request("pExpediente").Trim)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                IdEstadoExpediente = Reader("efiestado").ToString().Trim
            End If
            Reader.Close()
            Connection.Close()
        End If

        Response.Write(IdEstadoExpediente)
    End Sub

End Class