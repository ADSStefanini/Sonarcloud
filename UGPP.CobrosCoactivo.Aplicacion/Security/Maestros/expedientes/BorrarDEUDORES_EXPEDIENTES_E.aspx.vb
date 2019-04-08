Imports System.Data
Imports System.Data.SqlClient

Partial Public Class BorrarDEUDORES_EXPEDIENTES_E
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            Label1.Text = "Realmente desea borrar a " & Request("pNombre") & " del expediente " & Request("pExpediente") & "?"
        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "DELETE FROM DEUDORES_EXPEDIENTES WHERE NroExp = '" & Request("pExpediente").Trim & "' AND deudor = '" & Request("pDeudor").Trim & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Command.ExecuteNonQuery()

        Connection.Close()
        Response.Redirect("ENTES_DEUDORES_E.aspx?pExpediente=" & Request("pExpediente") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))

    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Response.Redirect("ENTES_DEUDORES_E.aspx?pExpediente=" & Request("pExpediente") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))

    End Sub
End Class