Imports System.Data.SqlClient
Partial Public Class capturaoficiocomunicadesembargo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
        End If

        If Not Me.Page.IsPostBack Then
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            lblDeudor.Text = Request("cedula")
            lblExpediente.Text = Request("expediente")
            acto.InnerHtml = Request("acto") & "-" & Request("nomacto")

            TextArea1.Focus()

            Dim sihaydoc As Boolean = chekhay(lblExpediente.Text.Trim, Request("acto"))
            If sihaydoc = True Then
                Validator.Text = "Este archivo fue generado con anterioridad, si desea imprimirlo presione el boton imprimir."
                Dim myTable As New DataTable
                myTable = CType(ViewState("datos"), DataTable)
                TextArea1.Value = myTable.Rows(0).Item("Articulo_variable")
                Me.Validator.IsValid = False
            End If
        End If
    End Sub

    Protected Sub LinkCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkCancelar.Click
        Response.Redirect("cobranzas2.aspx?cedula=" & Request("cedula") & "&expediente=" & Request("expediente") & "&acto=" & Request("acto") & "&nomacto=" & Request("nomacto") & "&tipo=1", True)
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Dim actualizar As Byte = 0
        Dim myTable As New DataTable
        myTable = CType(ViewState("datos"), DataTable)
        If Not myTable Is Nothing Then
            If myTable.Rows.Count > 0 Then
                actualizar = 1
            End If
        End If

        'REGISTRO DE EXCEPCIONES
        Using connection As New SqlConnection(IIf(Session("ConexionServer").ToString = Nothing, Funciones.CadenaConexion, Session("ConexionServer")))
            Dim idEntidadx() As String = Split(Request("cedula"), "::")
            Dim deunom, cedula As String
            If idEntidadx.Length = 2 Then
                cedula = CType(idEntidadx(1), String)
                deunom = CType(idEntidadx(0), String)
            Else
                Validator.Text = "Cédula o Nit del deudor invalida."
                Me.Validator.IsValid = False
                Exit Sub
            End If
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction

            transaction = connection.BeginTransaction()
            command.Connection = connection
            command.Transaction = transaction

            Try
                '--- datos del acto-----
                command.Parameters.Add("@IMPUESTO", SqlDbType.VarChar).Value = Session("ssCodimpadm")
                command.Parameters.Add("@actoadmin", SqlDbType.VarChar).Value = Request("acto")
                command.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = lblExpediente.Text.Trim
                command.Parameters.Add("@info", SqlDbType.VarChar).Value = TextArea1.Value

                If actualizar = 1 Then
                    command.CommandText = UCase("UPDATE Registro_excepciones SET  Articulo_variable=@info, numero_expediente=@EXPEDIENTE,idacto=@actoadmin WHERE numero_expediente =@EXPEDIENTE and idacto = @actoadmin")
                    command.ExecuteNonQuery()
                Else
                    command.CommandText = UCase("INSERT INTO Registro_excepciones (numero_expediente,idacto,Articulo_variable) VALUES(@EXPEDIENTE,@actoadmin, @info)")
                    command.ExecuteNonQuery()
                End If

                Try
                    transaction.Commit()
                    reporte()
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

    Private Function chekhay(ByVal expediente As String, ByVal acto As String) As Boolean
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT * FROM Registro_excepciones WHERE numero_expediente = @EXPEDIENTE and idacto=@idacto ", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = expediente
            myadapter.SelectCommand.Parameters.Add("@idacto", SqlDbType.VarChar).Value = acto
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

    Private Sub reporte()
        Response.Redirect("cobranzas2.aspx?cedula=" & Request("cedula") & "&expediente=" & Request("expediente") & "&acto=" & Request("acto") & "&nomacto=" & Request("nomacto") & "&tipo=1&reporte=2", False)
    End Sub
End Class