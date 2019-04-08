Imports System.Data.SqlClient
Partial Public Class capturaperito
    Inherits System.Web.UI.Page

    Private Sub datosextras()
        Dim Sql As String = " SELECT  E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, " _
                            & "E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, " _
                            & "E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, " _
                            & "SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +   SUM(L.LIQTOT)) AS MAN_VALORMANDA , e.EfiMatInm as MAN_MATINMOB " _
                            & "FROM EJEFIS E, LIQUIDAD L  " _
                            & "WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS " _
                            & "AND E.EFINROEXP = @Expediente " _
                            & "GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS,e.EfiMatInm"

        Dim myadapter As New SqlDataAdapter(Sql, Funciones.CadenaConexionUnion)
        myadapter.SelectCommand.Parameters.Add("@Expediente", SqlDbType.VarChar).Value = Request("expediente")
        Dim myTable As New DataTable
        myadapter.Fill(myTable)

        If myTable.Rows.Count > 0 Then

        End If
        ViewState("datos_mandamiento") = myTable
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
        End If

        If Not Me.Page.IsPostBack Then
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            lblDeudor.Text = Request("cedula")
            lblExpediente.Text = Request("expediente")
            acto.InnerHtml = Request("acto") & "-" & Request("nomacto")

            txtCedulanit.Focus()
            datosextras()
            Dim sihaydoc As Boolean = chekhay(lblExpediente.Text.Trim, Request("acto"))
            If sihaydoc = True Then
                Validator.Text = "Este archivo fue generado con anterioridad, si desea imprimirlo presione el boton imprimir."
                Dim myTable As New DataTable
                myTable = CType(ViewState("datos"), DataTable)


                txtCedulanit.Text = myTable.Rows(0).Item("doc_ccsecuestre")
                txtNomperito.Text = myTable.Rows(0).Item("doc_nombresecuestre")
                txtvalor.Text = myTable.Rows(0).Item("doc_liqtot")
                txtFecDiligencia.Text = myTable.Rows(0).Item("doc_fechadiligenciasecuestre")

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
            Dim tabla As New DataTable
            tabla = CType(ViewState("datos_mandamiento"), DataTable)

            Try

                command.Parameters.Add("@IMPUESTO", SqlDbType.VarChar).Value = Session("ssCodimpadm")
                command.Parameters.Add("@actoadmin", SqlDbType.VarChar).Value = Request("acto")
                command.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = lblExpediente.Text.Trim

                'Informacion de deudor
                command.Parameters.Add("@doc_deudorcedula", SqlDbType.VarChar).Value = cedula
                command.Parameters.Add("@doc_deudornombre", SqlDbType.VarChar).Value = deunom
                command.Parameters.Add("@doc_deudirest", SqlDbType.VarChar).Value = tabla.Rows(0).Item(4).ToString
                

                command.Parameters.Add("@cedulasecuestre", SqlDbType.VarChar).Value = txtCedulanit.Text
                command.Parameters.Add("@nombresecuestre", SqlDbType.VarChar).Value = txtNomperito.Text
                command.Parameters.Add("@doc_liqtot", SqlDbType.VarChar).Value = txtvalor.Text
                command.Parameters.Add("@fechadiligencia", SqlDbType.Date).Value = CDate(txtFecDiligencia.Text)


                If actualizar = 1 Then
                    command.CommandText = UCase("UPDATE entra_documentoma SET  doc_deudirest=@doc_deudirest,doc_deudornombre=@doc_deudornombre, doc_deudorcedula=@doc_deudorcedula,doc_ccsecuestre=@cedulasecuestre, doc_nombresecuestre=@nombresecuestre, doc_liqtot=@doc_liqtot, doc_fechadiligenciasecuestre=@fechadiligencia,  doc_expediente=@EXPEDIENTE,doc_actoadministrativo=@actoadmin,doc_impuesto=@IMPUESTO WHERE doc_expediente =@EXPEDIENTE and doc_actoadministrativo = @actoadmin")

                Else
                    command.CommandText = UCase("INSERT INTO entra_documentoma (doc_deudorcedula,doc_deudornombre,doc_deudirest,doc_expediente,doc_actoadministrativo,doc_impuesto,doc_ccsecuestre,doc_nombresecuestre,doc_liqtot,doc_fechadiligenciasecuestre) VALUES(@doc_deudorcedula,@doc_deudornombre,@doc_deudirest,@EXPEDIENTE,@actoadmin,@IMPUESTO, @cedulasecuestre,@nombresecuestre,@doc_liqtot,@fechadiligencia)")
                End If

                Try
                    command.ExecuteNonQuery()
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
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT *  FROM [entra_documentoma] WHERE doc_expediente = @doc_expediente and doc_actoadministrativo=@doc_actoadministrativo", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@doc_expediente", SqlDbType.VarChar).Value = expediente
            myadapter.SelectCommand.Parameters.Add("@doc_actoadministrativo", SqlDbType.VarChar).Value = acto
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
        Response.Redirect("cobranzas2.aspx?cedula=" & Request("cedula") & "&expediente=" & Request("expediente") & "&acto=" & Request("acto") & "&nomacto=" & Request("nomacto") & "&tipo=1&reporte=1", True)
    End Sub
End Class