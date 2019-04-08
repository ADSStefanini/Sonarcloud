Imports System.Data.SqlClient

Partial Public Class EditESTADOS_PAGO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from ESTADOS_PAGO where [codigo] = @codigo"
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@codigo", Request("ID"))
                Dim Reader As SqlDataReader = Command.ExecuteReader
                If Reader.Read Then
                    txtcodigo.Text = Reader("codigo").ToString()
                    txtnombre.Text = Reader("nombre").ToString()
                End If
                Reader.Close()
                Connection.Close()
            Else
                cmdDelete.Visible = False
            End If

        End If
    End Sub


    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "delete from ESTADOS_PAGO where [codigo] = @codigo"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@codigo", Request("ID"))
        Command.ExecuteNonQuery()

        Connection.Close()
        Response.Redirect("ESTADOS_PAGO.aspx")
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand

        Dim InsertSQL As String = "Insert into ESTADOS_PAGO ([codigo], [nombre] ) VALUES ( @codigo, @nombre ) "

        Dim UpdateSQL As String = "Update ESTADOS_PAGO set [codigo] = @codigo, [nombre] = @nombre where [codigo] = @codigo "

        If String.IsNullOrEmpty(ID) Then
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@codigo", txtcodigo.Text.Trim)
        Else
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@codigo", ID)
        End If

        Command.Parameters.AddWithValue("@nombre", txtnombre.Text.Trim)

        Command.ExecuteNonQuery()
        Connection.Close()
        Response.Redirect("ESTADOS_PAGO.aspx")

    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("ESTADOS_PAGO.aspx")
    End Sub

End Class