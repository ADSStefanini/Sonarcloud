Imports System.Data.SqlClient
Partial Public Class capturarecibosdepagos
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            menssageError(amsbgox)
            Exit Sub
        End If

        If Not Me.Page.IsPostBack Then
            If Session("ConexionServer") Is Nothing Then
                Session("ConexionServer") = Funciones.CadenaConexion
            End If

            txtExpediente.Focus()

        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim actualizar As Byte = 0
        Dim myTable As New DataTable
        myTable = CType(ViewState("datos"), DataTable)
        If Not myTable Is Nothing Then
            If myTable.Rows.Count > 0 Then
                actualizar = 1
            End If
        End If

        'entra_documentoma
        Using connection As New SqlConnection(IIf(Session("ConexionServer").ToString = Nothing, Funciones.CadenaConexion, Session("ConexionServer")))
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction

            transaction = connection.BeginTransaction()
            command.Connection = connection
            command.Transaction = transaction
            Try

                '--DATOS DEL CONTRIBUYENTE
                command.Parameters.AddWithValue("@rp_nro", txtnro.Text)
                command.Parameters.AddWithValue("@rp_fecha", CDate(txtfecha.Text))
                command.Parameters.AddWithValue("@rp_valor", txtvalor.Text)
                '--- datos del acto-----
                command.Parameters.AddWithValue("@rp_expediente", txtExpediente.Text.Trim)
                command.Parameters.AddWithValue("@rp_impuesto", Session("ssCodimpadm"))


                If actualizar = 1 Then
                    command.CommandText = UCase(" UPDATE MAESTRO_RECIBOS_PAGOS			" & _
                                                "    SET rp_nro = @rp_nro               " & _
                                                "       ,rp_fecha = @rp_fecha           " & _
                                                "       ,rp_valor = @rp_valor           " & _
                                                "       ,rp_expediente = @rp_expediente " & _
                                                "       ,rp_impuesto = @rp_impuesto     " & _
                                                "       ,rp_acto = @rp_acto             " & _
                                                "  WHERE rp_expediente = @rp_expediente " & _
                                                "  AND   rp_acto = @rp_acto             ")

                Else
                    command.CommandText = UCase(" INSERT INTO MAESTRO_RECIBOS_PAGOS " & _
                                                "            (rp_nro                " & _
                                                "            ,rp_fecha              " & _
                                                "            ,rp_valor              " & _
                                                "            ,rp_expediente         " & _
                                                "            ,rp_impuesto)          " & _
                                                "      VALUES                       " & _
                                                "            (@rp_nro               " & _
                                                "            ,@rp_fecha             " & _
                                                "            ,@rp_valor             " & _
                                                "            ,@rp_expediente        " & _
                                                "            ,@rp_impuesto)         ")

                End If


                Try
                    command.ExecuteNonQuery()
                    transaction.Commit()
                    Validator.Text = "Datos guardados con éxito."
                    Me.Validator.IsValid = False
                Catch ex As Exception
                    transaction.Rollback()
                    Validator.Text = ex.Message
                    Me.Validator.IsValid = False
                End Try
            Catch ex2 As Exception
                Validator.Text = "No se guardaron los datos. <br />" & ex2.Message
                Me.Validator.IsValid = False
            End Try
        End Using
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        txtnro.Text = ""
        txtfecha.Text = ""
        txtvalor.Text = ""
        txtnro.Focus()

    End Sub

    Protected Sub txtExpediente_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtExpediente.TextChanged
        Dim sihaydoc As Boolean = chekhay(txtExpediente.Text.Trim)
        If sihaydoc = True Then
            Validator.Text = "Un registro encontrado..."
            Dim myTable As New DataTable
            myTable = CType(ViewState("datos"), DataTable)
            txtnro.Text = myTable.Rows(0).Item("rp_nro")
            txtfecha.Text = myTable.Rows(0).Item("rp_fechar")
            txtvalor.Text = myTable.Rows(0).Item("rp_valor")
            Me.Validator.IsValid = False
        End If
    End Sub

    Private Function chekhay(ByVal expediente As String) As Boolean
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT *  FROM MAESTRO_RECIBOS_PAGOS WHERE rp_expediente = @rp_expediente ", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@rp_expediente", SqlDbType.VarChar).Value = expediente
            Using myTable As New DataTable
                myadapter.Fill(myTable)
                If myTable.Rows.Count > 0 Then
                    ViewState("datos") = CType(myTable, DataTable)
                    Return True
                Else
                    Return False
                End If
            End Using
        End Using
    End Function

End Class