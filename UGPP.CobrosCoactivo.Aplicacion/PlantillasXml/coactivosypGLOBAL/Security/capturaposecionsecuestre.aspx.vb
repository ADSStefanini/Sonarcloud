Imports System.Data.SqlClient
Partial Public Class capturaposecionsecuestre
    Inherits System.Web.UI.Page

    Private Sub datosextras()
        Dim Sql As String = "SELECT E.EFINROEXP AS MAN_EXPEDIENTE,E.EFIGEN AS MAN_REFCATRASTAL, 													" & _
                            " E.EfiMatInm AS MAN_MATINMOB,  E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,   	                                                    " & _
                            " E.EFIDIR AS MAN_DIR_ESTABL,   E.EFIPERDES AS MAN_EFIPERDES, E.EfiSubDes  AS MAN_EFISUBDES,E.EFIPERHAS   AS MAN_EFIPERHAS,	                " & _
                            " E.EfiSubHas AS MAN_EFISUBHAS, SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,   SUM(L.EDC_TOTALABO) AS MAN_PAGOS, SUM(L.EDC_INTERES) AS MAN_INTERESES,	" & _
                            " SUM(L.EDC_TOTALDEUDA) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE , L.EDC_PREDIO AS MAN_PRENUM  	                                                    " & _
                            " FROM EJEFISGLOBAL E,  EJEFISGLOBALLIQUIDAD L    	                                                                                        " & _
                            " WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente    	                    " & _
                            " AND E.EfiModCod = @Impuesto  	                                                                                                            " & _
                            " GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EfiSubHas,L.EDC_PREDIO	                "

        Dim myadapter As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
        myadapter.SelectCommand.Parameters.Add("@Expediente", SqlDbType.VarChar).Value = Request("expediente")
        myadapter.SelectCommand.Parameters.Add("@Impuesto", SqlDbType.VarChar).Value = Session("ssimpuesto")

        Dim myTable As New DataTable
        myadapter.Fill(myTable)
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
                txtNomSecuestre.Text = myTable.Rows(0).Item("doc_nombresecuestre")
                txtNroCarnet.Text = myTable.Rows(0).Item("doc_carnetsecuestre")
                txtFecDiligencia.Text = myTable.Rows(0).Item("doc_fechadiligenciasecuestre")
                txtHora.Text = myTable.Rows(0).Item("doc_horadiligenciasecuestre").ToString
                txtTelefono.Text = myTable.Rows(0).Item("doc_telefonosecuestre")
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
                command.Parameters.Add("@doc_deudirest", SqlDbType.VarChar).Value = tabla.Rows(0).Item(5).ToString
                command.Parameters.Add("@doc_refcatrastal", SqlDbType.VarChar).Value = tabla.Rows(0).Item(1).ToString
                command.Parameters.Add("@doc_matinm", SqlDbType.VarChar).Value = tabla.Rows(0).Item(2).ToString
                command.Parameters.Add("@doc_prenum", SqlDbType.VarChar).Value = tabla.Rows(0).Item(16).ToString


                command.Parameters.Add("@cedulasecuestre", SqlDbType.VarChar).Value = txtCedulanit.Text
                command.Parameters.Add("@nombresecuestre", SqlDbType.VarChar).Value = txtNomSecuestre.Text
                command.Parameters.Add("@carnet", SqlDbType.VarChar).Value = txtNroCarnet.Text
                command.Parameters.Add("@fechadiligencia", SqlDbType.Date).Value = CDate(txtFecDiligencia.Text)
                command.Parameters.Add("@horadiligencia", SqlDbType.VarChar).Value = txtHora.Text
                command.Parameters.Add("@telefono", SqlDbType.VarChar).Value = txtTelefono.Text


                If actualizar = 1 Then
                    command.CommandText = UCase("UPDATE entra_documentoma SET doc_prenum = @doc_prenum, doc_matinm=@doc_matinm, doc_refcatrastal=@doc_refcatrastal, doc_deudirest=@doc_deudirest,doc_deudornombre=@doc_deudornombre, doc_deudorcedula=@doc_deudorcedula,doc_ccsecuestre=@cedulasecuestre, doc_nombresecuestre=@nombresecuestre, doc_carnetsecuestre=@carnet, doc_fechadiligenciasecuestre=@fechadiligencia, doc_horadiligenciasecuestre=@horadiligencia,doc_telefonosecuestre=@telefono, doc_expediente=@EXPEDIENTE,doc_actoadministrativo=@actoadmin,doc_impuesto=@IMPUESTO WHERE doc_expediente =@EXPEDIENTE and doc_actoadministrativo = @actoadmin")
                    command.ExecuteNonQuery()
                Else
                    command.CommandText = UCase("INSERT INTO entra_documentoma (doc_prenum,doc_matinm,doc_deudorcedula,doc_deudornombre,doc_deudirest,doc_refcatrastal,doc_expediente,doc_actoadministrativo,doc_impuesto,doc_ccsecuestre,doc_nombresecuestre,doc_carnetsecuestre,doc_fechadiligenciasecuestre,doc_horadiligenciasecuestre,doc_telefonosecuestre) VALUES(@doc_prenum,@doc_matinm,@doc_deudorcedula,@doc_deudornombre,@doc_deudirest,@doc_refcatrastal,@EXPEDIENTE,@actoadmin,@IMPUESTO, @cedulasecuestre,@nombresecuestre,@carnet,@fechadiligencia,@horadiligencia,@telefono)")
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