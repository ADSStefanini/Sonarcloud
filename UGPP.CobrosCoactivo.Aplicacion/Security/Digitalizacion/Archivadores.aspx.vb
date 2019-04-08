Imports System.Data.SqlClient
Imports System.IO
Partial Public Class Archivadores
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Private Sub LinkGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkGuardar.Click
        If Session("UsuarioValido") Is Nothing Then
            Messenger.Attributes.Add("style", "display:block;")
            Messenger_contenido.InnerHtml = "Ha caducado el tiempo de sesión, cierre esta ventana y vuelva a ejecutar este proceso."
            Exit Sub
        End If

        newArchivador()
    End Sub

    Private Function newArchivador() As Boolean
        If txtArchivador.Value <> Nothing Then
            Dim proximo_numero As String
            Dim xArchivador As String
            xArchivador = ""
            Using ado As New SqlConnection(Funciones.CadenaConexion)
                Dim myTrans As SqlTransaction
                If ado.State = ConnectionState.Open Then
                    ado.Close()
                End If
                ado.Open()
                myTrans = ado.BeginTransaction()
                Dim queryString As String = "UPDATE MAESTRO_CONSECUTIVOS SET @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = 8"
                Using command As New SqlCommand(queryString, ado)
                    Try
                        command.Parameters.Add("@proximo_numero", SqlDbType.Int)
                        command.Parameters("@proximo_numero").Direction = ParameterDirection.Output
                        command.Transaction = myTrans
                        command.ExecuteNonQuery()

                        Dim conse As Integer = CType(command.Parameters("@proximo_numero").Value, Integer)
                        proximo_numero = Format(conse, "000000000")

                        command.CommandText = "INSERT INTO DOCUMENTO_ARCHIVADORES (ARC_COD,ARC_NOMBRE,ARC_FECHA,ARC_USUARIO,ARC_COBRADOR) VALUES (@ARC_COD,@ARC_NOMBRE,@ARC_FECHA,@ARC_USUARIO,@ARC_COBRADOR)"
                        command.Parameters.Add("@ARC_COD", SqlDbType.VarChar).Value = proximo_numero
                        xArchivador = txtArchivador.Value & "(" & Date.Now.ToString("ddMMyyyy") & ")"
                        command.Parameters.Add("@ARC_NOMBRE", SqlDbType.VarChar).Value = xArchivador
                        command.Parameters.Add("@ARC_FECHA", SqlDbType.DateTime).Value = Date.Now
                        command.Parameters.Add("@ARC_USUARIO", SqlDbType.VarChar).Value = Session("sscodigousuario")
                        command.Parameters.Add("@ARC_COBRADOR", SqlDbType.VarChar).Value = Session("mcobrador")
                        command.ExecuteNonQuery()

                        myTrans.Commit()
                        idmessage.Attributes.Add("style", "display:none;")
                        custom_demo.Attributes.Add("style", "display:block;")
                        Messenger.Attributes.Add("style", "display:none;")
                    Catch ex As Exception
                        myTrans.Rollback()
                        Messenger.Attributes.Add("style", "display:block;")
                        Messenger_contenido.InnerHtml = ex.Message & "<br /> <br />Consulte con el administrador del sistema."
                    Finally
                        ado.Close()
                    End Try
                End Using
            End Using

            Try
                Directory.CreateDirectory(Server.MapPath("Upload\" & xArchivador))
                ViewState("archivador") = xArchivador
            Catch ex As Exception
                Messenger.Attributes.Add("style", "display:block;")
                Messenger_contenido.InnerHtml = ex.Message & "<br /> <br />Consulte con el administrador del sistema."
                Response.Write(ex.Message)
            End Try
        Else
            txtArchivador.Focus()
            _Error.InnerHtml = "< Digite un nombre de archivador valido >"
        End If
    End Function
End Class