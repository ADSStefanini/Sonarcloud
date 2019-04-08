Imports System.Data.SqlClient
Partial Public Class capturaliquidacioncredito
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
            Response.Redirect("~/login.aspx")
        End If

        If Not Me.Page.IsPostBack Then
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            lblDeudor.Text = Request("cedula")
            lblExpediente.Text = Request("expediente")
            acto.InnerHtml = Request("acto") & "-" & Request("nomacto")

            Dim fecha As Date = Today.Date
            txtFechaliq.Text = fecha.ToString("dd/MM/yyyy")
            txtFechatitulo.Text = fecha.ToString("dd/MM/yyyy")
            txtFechaliq.Focus()
            Dim sihaydoc As Boolean = chekhay(lblExpediente.Text.Trim, Request("acto"))
            If Session("ssimpuesto") = 1 Then
                impuesto.Text = "PREDIAL"
            ElseIf Session("ssimpuesto") = 2 Then
                impuesto.Text = "INDUSTRIA Y COMERCIO"
            End If
            If sihaydoc = True Then
                Validator.Text = "Este archivo fue generado con anterioridad, si desea imprimirlo presione el boton imprimir."
                Dim myTable As New DataTable
                myTable = CType(ViewState("datos"), DataTable)

                txtFechaliq.Text = CDate(myTable.Rows(0).Item("LC_FECHALIQ"))
                txtnrotitulo.Text = myTable.Rows(0).Item("LC_TITULO")
                txtFechatitulo.Text = CDate(myTable.Rows(0).Item("LC_FECHATITULO"))
                vigencia.Text = myTable.Rows(0).Item("LC_VIGENCIA")
                totaltitulo.Text = UCase(myTable.Rows(0).Item("LC_TITULOTOTAL"))
                transpdili.Text = myTable.Rows(0).Item("LC_TRANSDILIGENCIA")
                honcerr.Text = UCase(myTable.Rows(0).Item("LC_HONCERRAJERO"))
                honsec.Text = UCase(myTable.Rows(0).Item("LC_HONSECUESTRE"))
                totalhon.Text = UCase(myTable.Rows(0).Item("LC_HONTOTAL"))
                totcred.Text = UCase(myTable.Rows(0).Item("LC_TOTALCREDITO"))
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

        'entra_documentoma
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
                command.Parameters.AddWithValue("@LC_FECHALIQ", FechaWebLocal(txtFechaliq.Text))
                command.Parameters.AddWithValue("@LC_TITULO", txtnrotitulo.Text)
                command.Parameters.AddWithValue("@LC_FECHATITULO", FechaWebLocal(txtFechatitulo.Text))
                command.Parameters.AddWithValue("@LC_VIGENCIA", vigencia.Text)
                command.Parameters.AddWithValue("@LC_IMPUESTO", Session("ssimpuesto"))
                command.Parameters.AddWithValue("@LC_TITULOTOTAL", totaltitulo.Text)
                command.Parameters.AddWithValue("@LC_TRANSDILIGENCIA", CDbl(transpdili.Text))
                command.Parameters.AddWithValue("@LC_HONCERRAJERO", honcerr.Text)
                command.Parameters.AddWithValue("@LC_HONSECUESTRE", CDbl(honsec.Text))
                command.Parameters.AddWithValue("@LC_HONTOTAL", CDbl(totalhon.Text))
                command.Parameters.AddWithValue("@LC_TOTALCREDITO", CDbl(totcred.Text))
                command.Parameters.AddWithValue("@LC_NOMDEUDOR", UCase(deunom))
                command.Parameters.AddWithValue("@LC_CCDEUDOR", cedula)
                command.Parameters.AddWithValue("@LC_EXPEDIENTE", lblExpediente.Text.Trim)
                command.Parameters.AddWithValue("@LC_ACTOADMIN", Request("acto"))


                If actualizar = 1 Then
                    command.CommandText = UCase(" UPDATE LIQUIDACION_CREDITO						" & _
                                                "    SET LC_FECHALIQ = @LC_FECHALIQ                 " & _
                                                "       ,LC_TITULO = @LC_TITULO                     " & _
                                                "       ,LC_FECHATITULO = @LC_FECHATITULO           " & _
                                                "       ,LC_VIGENCIA = @LC_VIGENCIA                 " & _
                                                "       ,LC_IMPUESTO = @LC_IMPUESTO                 " & _
                                                "       ,LC_TITULOTOTAL = @LC_TITULOTOTAL           " & _
                                                "       ,LC_TRANSDILIGENCIA = @LC_TRANSDILIGENCIA   " & _
                                                "       ,LC_HONCERRAJERO = @LC_HONCERRAJERO         " & _
                                                "       ,LC_HONSECUESTRE = @LC_HONSECUESTRE         " & _
                                                "       ,LC_HONTOTAL = @LC_HONTOTAL                 " & _
                                                "       ,LC_TOTALCREDITO = @LC_TOTALCREDITO         " & _
                                                "       ,LC_NOMDEUDOR = @LC_NOMDEUDOR               " & _
                                                "       ,LC_CCDEUDOR = @LC_CCDEUDOR                 " & _
                                                "  WHERE LC_EXPEDIENTE = @LC_EXPEDIENTE             " & _
                                                "  AND   LC_ACTOADMIN = @LC_ACTOADMIN               ")

                Else
                    command.CommandText = UCase(" INSERT INTO LIQUIDACION_CREDITO " & _
                                                "            (LC_FECHALIQ         " & _
                                                "            ,LC_TITULO           " & _
                                                "            ,LC_FECHATITULO      " & _
                                                "            ,LC_VIGENCIA         " & _
                                                "            ,LC_IMPUESTO         " & _
                                                "            ,LC_TITULOTOTAL      " & _
                                                "            ,LC_TRANSDILIGENCIA  " & _
                                                "            ,LC_HONCERRAJERO     " & _
                                                "            ,LC_HONSECUESTRE     " & _
                                                "            ,LC_HONTOTAL         " & _
                                                "            ,LC_TOTALCREDITO     " & _
                                                "            ,LC_NOMDEUDOR        " & _
                                                "            ,LC_CCDEUDOR         " & _
                                                "            ,LC_EXPEDIENTE       " & _
                                                "            ,LC_ACTOADMIN)       " & _
                                                "      VALUES                     " & _
                                                "            (@LC_FECHALIQ        " & _
                                                "            ,@LC_TITULO          " & _
                                                "            ,@LC_FECHATITULO     " & _
                                                "            ,@LC_VIGENCIA        " & _
                                                "            ,@LC_IMPUESTO        " & _
                                                "            ,@LC_TITULOTOTAL     " & _
                                                "            ,@LC_TRANSDILIGENCIA " & _
                                                "            ,@LC_HONCERRAJERO    " & _
                                                "            ,@LC_HONSECUESTRE    " & _
                                                "            ,@LC_HONTOTAL        " & _
                                                "            ,@LC_TOTALCREDITO    " & _
                                                "            ,@LC_NOMDEUDOR       " & _
                                                "            ,@LC_CCDEUDOR        " & _
                                                "            ,@LC_EXPEDIENTE      " & _
                                                "            ,@LC_ACTOADMIN       )")

                End If

                command.ExecuteNonQuery()
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
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT *  FROM LIQUIDACION_CREDITO WHERE LC_EXPEDIENTE = @LC_EXPEDIENTE and LC_ACTOADMIN=@LC_ACTOADMIN", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@LC_EXPEDIENTE", SqlDbType.VarChar).Value = expediente
            myadapter.SelectCommand.Parameters.Add("@LC_ACTOADMIN", SqlDbType.VarChar).Value = acto
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
        Response.Redirect("cobranzas2.aspx?cedula=" & Request("cedula") & "&expediente=" & Request("expediente") & "&acto=" & Request("acto") & "&nomacto=" & Request("nomacto") & "&tipo=1&reporte=3", True)
    End Sub
End Class