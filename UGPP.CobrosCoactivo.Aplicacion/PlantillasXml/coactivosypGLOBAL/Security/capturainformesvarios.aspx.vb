Imports System.Data.SqlClient
Partial Public Class capturainformesvarios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            lblDeudor.Text = Request("cedula")
            lblExpediente.Text = Request("expediente")
            acto.InnerHtml = Request("acto") & "-" & Request("nomacto")

            Call datosextras()
            Dim sihaydoc As Boolean = chekhay(lblExpediente.Text.Trim, Request("acto"))
            If sihaydoc = True Then
                Validator.Text = "Este archivo fue generado con anterioridad, si desea imprimirlo presione el boton imprimir."
                Dim myTable As New DataTable
                myTable = CType(ViewState("datos"), DataTable)

                mtext.Value = UCase(myTable.Rows(0).Item("doc_memo"))

                If myTable.Rows(0).IsNull("doc_fecfijacion") Then
                    txtFechaFijacion.Text = Nothing
                Else
                    txtFechaFijacion.Text = myTable.Rows(0).Item("doc_fecfijacion")
                End If

                If myTable.Rows(0).IsNull("doc_fecdesfijacion") Then
                    txtFechaDesfijacion.Text = Nothing
                Else
                    txtFechaDesfijacion.Text = myTable.Rows(0).Item("doc_fecdesfijacion")
                End If

                If myTable.Rows(0).IsNull("doc_horadesfijacion") Then
                    txthoraDesfijacion.Text = Nothing
                Else
                    txthoraDesfijacion.Text = myTable.Rows(0).Item("doc_horadesfijacion")
                End If
                If myTable.Rows(0).IsNull("doc_horafijacion") Then
                    txthoraFijacion.Text = Nothing
                Else
                    txthoraFijacion.Text = myTable.Rows(0).Item("doc_horafijacion")
                End If

                Me.Validator.IsValid = False
            End If
        End If
    End Sub

    Private Function chekhay(ByVal expediente As String, ByVal acto As String) As Boolean
        Dim myadapter As New SqlClient.SqlDataAdapter("SELECT *  FROM [entra_documentoma] WHERE doc_expediente = @doc_expediente and doc_actoadministrativo=@doc_actoadministrativo", Funciones.CadenaConexion)
        myadapter.SelectCommand.Parameters.Add("@doc_expediente", SqlDbType.VarChar).Value = expediente
        myadapter.SelectCommand.Parameters.Add("@doc_actoadministrativo", SqlDbType.VarChar).Value = acto
        Dim myTable As New DataTable
        myadapter.Fill(myTable)
        If myTable.Rows.Count > 0 Then
            ViewState("datos") = CType(myTable, DataTable)
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub datosextras()
        Dim Sql As String = "SELECT E.EFINROEXP AS MAN_EXPEDIENTE,E.EFIGEN AS MAN_REFCATRASTAL, 													" & _
" E.EfiMatInm AS MAN_MATINMOB,  E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,   	                                                    " & _
" E.EFIDIR AS MAN_DIR_ESTABL,   E.EFIPERDES AS MAN_EFIPERDES, E.EfiSubDes  AS MAN_EFISUBDES,E.EFIPERHAS   AS MAN_EFIPERHAS,	                " & _
" E.EfiSubHas AS MAN_EFISUBHAS, SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,   SUM(L.EDC_TOTALABO) AS MAN_PAGOS, SUM(L.EDC_INTERES) AS MAN_INTERESES,	" & _
" SUM(L.EDC_TOTALDEUDA) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE    	                                                    " & _
" FROM EJEFISGLOBAL E,  EJEFISGLOBALLIQUIDAD L    	                                                                                        " & _
" WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente    	                    " & _
" AND E.EfiModCod = @Impuesto  	                                                                                                            " & _
" GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EfiSubHas	                "


        Dim myadapter As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
        myadapter.SelectCommand.Parameters.Add("@Expediente", SqlDbType.VarChar).Value = Request("expediente")
        myadapter.SelectCommand.Parameters.Add("@Impuesto", SqlDbType.VarChar).Value = Session("ssimpuesto")
        Dim myTable As New DataTable
        myadapter.Fill(myTable)

        If myTable.Rows.Count > 0 Then
            lblLiqtotal.Text = FormatCurrency(myTable.Rows(0).Item("MAN_VALORMANDA"), 2)

            If myTable.Rows(0).IsNull("MAN_INTERESES") Then
                lblLiqInteres.Text = "0,0"
            Else
                lblLiqInteres.Text = FormatCurrency(myTable.Rows(0).Item("MAN_INTERESES"), 2)
            End If

            If myTable.Rows(0).IsNull("MAN_PAGOS") Then
                lblLiqAbono.Text = "0,0"
            Else
                lblLiqAbono.Text = FormatCurrency(myTable.Rows(0).Item("MAN_PAGOS"), 2)
            End If


            ViewState("MAN_VALORMANDA") = myTable.Rows(0).Item("MAN_VALORMANDA")
            ViewState("MAN_INTERESES") = myTable.Rows(0).Item("MAN_INTERESES")
            ViewState("MAN_PAGOS") = myTable.Rows(0).Item("MAN_PAGOS")
            ViewState("MAN_DIR_ESTABL") = myTable.Rows(0).Item("MAN_DIR_ESTABL")
        Else
            lblLiqtotal.Text = "ERROR"
            lblLiqInteres.Text = "ERROR"
            lblLiqAbono.Text = "ERROR"
        End If

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
        Using connection As New SqlConnection(Session("ConexionServer").ToString)
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
            'Dim tabla As DataTable = CType(ViewState("datos"), DataTable)

            Try
                command.Parameters.Add("@doc_liqtot", SqlDbType.Float).Value = ViewState("MAN_VALORMANDA")
                command.Parameters.Add("@doc_liqint", SqlDbType.Float).Value = ViewState("MAN_INTERESES")
                command.Parameters.Add("@doc_liqabo", SqlDbType.Float).Value = ViewState("MAN_PAGOS")
                command.Parameters.Add("@doc_fecharac", SqlDbType.Date).Value = Date.Now
                command.Parameters.Add("@FECHARAC", SqlDbType.Date).Value = Date.Now
                command.Parameters.Add("@doc_impuesto", SqlDbType.VarChar).Value = Session("ssCodimpadm")
                command.Parameters.Add("@doc_actoadministrativo", SqlDbType.VarChar).Value = Request("acto")
                command.Parameters.Add("@doc_expediente", SqlDbType.VarChar).Value = lblExpediente.Text
                command.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = lblExpediente.Text
                command.Parameters.Add("@doc_memo", SqlDbType.VarChar).Value = mtext.Value

                'Informacion de deudor
                command.Parameters.Add("@cedula", SqlDbType.VarChar).Value = cedula
                command.Parameters.Add("@deunom", SqlDbType.VarChar).Value = deunom
                command.Parameters.Add("@doc_deudirest", SqlDbType.VarChar).Value = ViewState("MAN_DIR_ESTABL")

                '--Informacion de fijacion
                command.Parameters.AddWithValue("@doc_fecfijacion", CDate(txtFechaFijacion.Text))
                command.Parameters.AddWithValue("@doc_horafijacion", txthoraFijacion.Text)
                command.Parameters.AddWithValue("@doc_fecdesfijacion", CDate(txtFechaDesfijacion.Text))
                command.Parameters.AddWithValue("@doc_horadesfijacion", txthoraDesfijacion.Text)


                If actualizar = 1 Then
                    command.CommandText = UCase("UPDATE entra_documentoma SET doc_fecharac=@doc_fecharac,doc_fecdesfijacion=@doc_fecdesfijacion,doc_horadesfijacion=@doc_horadesfijacion, doc_fecfijacion=@doc_fecfijacion,doc_horafijacion=@doc_horafijacion,doc_deudorcedula=@cedula,doc_deudornombre=@deunom,doc_deudirest=@doc_deudirest,doc_expediente=@doc_expediente,doc_memo=@doc_memo,doc_actoadministrativo=@doc_actoadministrativo,doc_liqtot=@doc_liqtot,doc_liqint=@doc_liqint,doc_liqabo=@doc_liqabo,doc_impuesto=@doc_impuesto WHERE doc_expediente =@doc_expediente and doc_actoadministrativo = @doc_actoadministrativo")
                    command.ExecuteNonQuery()
                Else
                    command.CommandText = UCase("INSERT INTO entra_documentoma (doc_fecfijacion,doc_horafijacion,doc_fecdesfijacion,doc_horadesfijacion,doc_deudorcedula,doc_deudornombre,doc_deudirest,doc_expediente,doc_memo,doc_actoadministrativo,doc_liqtot,doc_liqint,doc_liqabo,doc_impuesto,doc_fecharac) VALUES(@doc_fecfijacion,@doc_horafijacion,@doc_fecdesfijacion,@doc_horadesfijacion,@cedula,@deunom,@doc_deudirest,@EXPEDIENTE,@doc_memo,@doc_actoadministrativo,@doc_liqtot,@doc_liqint,@doc_liqabo,@doc_impuesto,@doc_fecharac" & ")")
                    command.ExecuteNonQuery()
                End If

                Try
                    transaction.Commit()
                    Validator.Text = "Datos guardados con éxito."
                    Me.Validator.IsValid = False


                    reporte()
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
    Protected Sub LinkCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkCancelar.Click
        Response.Redirect("cobranzas2.aspx?cedula=" & Request("cedula") & "&expediente=" & Request("expediente") & "&acto=" & Request("acto") & "&nomacto=" & Request("nomacto") & "&tipo=1", True)
    End Sub
    Private Sub reporte()
        Response.Redirect("cobranzas2.aspx?cedula=" & Request("cedula") & "&expediente=" & Request("expediente") & "&acto=" & Request("acto") & "&nomacto=" & Request("nomacto") & "&tipo=1&reporte=1", True)
    End Sub
End Class