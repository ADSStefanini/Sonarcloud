Imports System.Data.SqlClient
Partial Public Class capturanotificacionpersona
    Inherits System.Web.UI.Page

    Private Sub FechaRac()
        Dim ACTO As String = ViewState("ActoPre").ToString
        Dim NombreActoPre As String = ViewState("NombreActoPre")

        Using myadapter As New SqlDataAdapter("select * from documentos where docexpediente=@doc_expediente and idacto=@idacto", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@doc_expediente", SqlDbType.VarChar).Value = Request("expediente")
            myadapter.SelectCommand.Parameters.Add("@idacto", SqlDbType.VarChar).Value = ACTO
            Dim myTable As New DataTable
            myadapter.Fill(myTable)
            If myTable.Rows.Count > 0 Then
                lblFechaRac.Text = CDate(myTable.Rows(0).Item("fecharadic")).ToString
                ViewState("fecharadic") = CDate(myTable.Rows(0).Item("fecharadic"))

                Validator.Text = "..."
                Me.Validator.IsValid = False
            Else
                Validator.Text = "Documento invalido - No se detecto " & NombreActoPre.ToUpper & " asociado a este expediente."
                btnImprimir.Enabled = False
                Me.Validator.IsValid = False
            End If
        End Using
    End Sub

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

        myadapter.SelectCommand.CommandTimeout = 60000
        Using myTable As New DataTable
            myadapter.Fill(myTable)
            ViewState("datosMandamiento") = myTable
            If myTable.Rows.Count > 0 Then
                lblLiqtotal.Text = FormatCurrency(myTable.Rows(0).Item("MAN_VALORMANDA"), 2)
                If Not myTable.Rows(0).IsNull("MAN_INTERESES") Then
                    lblLiqInteres.Text = FormatCurrency(myTable.Rows(0).Item("MAN_INTERESES"), 2)
                    ViewState("MAN_INTERESES") = myTable.Rows(0).Item("MAN_INTERESES")
                Else
                    ViewState("MAN_INTERESES") = 0
                    lblLiqInteres.Text = 0
                End If
                If Not myTable.Rows(0).IsNull("MAN_PAGOS") Then
                    lblLiqAbono.Text = FormatCurrency(myTable.Rows(0).Item("MAN_PAGOS"), 2)
                    ViewState("MAN_PAGOS") = myTable.Rows(0).Item("MAN_PAGOS")
                Else
                    lblLiqAbono.Text = 0
                    ViewState("MAN_PAGOS") = 0
                End If

                ViewState("MAN_VALORMANDA") = myTable.Rows(0).Item("MAN_VALORMANDA")
            Else
                lblLiqtotal.Text = "ERROR"
                lblLiqInteres.Text = "ERROR"
                lblLiqAbono.Text = "ERROR"
            End If
        End Using

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
        End If

        If Not Me.Page.IsPostBack Then
            If Request("acto") = "017" Then
                UpdateActoPre()
            ElseIf Request("acto") = "023" Then
                UpdateActoPre023()
            End If
            ACTOPRE()
            NumResolucion()
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            lblDeudor.Text = Request("cedula")
            lblExpediente.Text = Request("expediente")
            acto.InnerHtml = Request("acto") & "-" & Request("nomacto")

            Dim fecha As Date = Today.Date
            txtFechaPresentacion.Text = fecha.ToString("dd/MM/yyyy")
            txtCedulanit.Focus()
            Call FechaRac()
            Call datosextras()
            Dim sihaydoc As Boolean = chekhay(lblExpediente.Text.Trim, Request("acto"))
            If sihaydoc = True Then
                Validator.Text = "Este archivo fue generado con anterioridad, si desea imprimirlo presione el boton imprimir."
                Dim myTable As New DataTable
                myTable = CType(ViewState("datos"), DataTable)
                If myTable.Rows(0).Item("doc_tipoapo") = "Pern" Then
                    rApoderado.SelectedIndex = 0
                ElseIf myTable.Rows(0).Item("doc_tipoapo") = "perju" Then
                    rApoderado.SelectedIndex = 1
                ElseIf myTable.Rows(0).Item("doc_tipoapo") = "perapo" Then
                    rApoderado.SelectedIndex = 2
                End If

                txtFechaPresentacion.Text = CDate(myTable.Rows(0).Item("doc_fechaNotificacion")).ToString("dd-MM-yyyy")
                txtCedulanit.Text = myTable.Rows(0).Item("doc_apocc")
                txtFechaExpedicion.Text = CDate(myTable.Rows(0).Item("doc_apofechaexpcc")).ToString("dd-MM-yyyy")
                txtExpedicion.Text = UCase(myTable.Rows(0).Item("doc_apolugarexpcc"))
                txtNombre.Text = UCase(myTable.Rows(0).Item("doc_aponombre"))
                txtTarjetaAbogado.Text = myTable.Rows(0).Item("doc_apotarjetapro")
                mtext.Value = UCase(myTable.Rows(0).Item("doc_memo"))

            End If
            Me.Validator.IsValid = False
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

            Dim apo As String = rApoderado.SelectedValue
            Dim tabla As DataTable = CType(ViewState("datosMandamiento"), DataTable)

            Try
                command.Parameters.Add("@TOTAL", SqlDbType.Float).Value = ViewState("MAN_VALORMANDA")
                command.Parameters.Add("@INTERES", SqlDbType.Float).Value = ViewState("MAN_INTERESES")
                command.Parameters.Add("@PAGOS", SqlDbType.Float).Value = ViewState("MAN_PAGOS")
                command.Parameters.Add("@fechaNotificacion", SqlDbType.Date).Value = FechaWebLocal(txtFechaPresentacion.Text)
                command.Parameters.Add("@fecharac", SqlDbType.Date).Value = FechaWebLocal(ViewState("fecharadic"))
                command.Parameters.Add("@IMPUESTO", SqlDbType.VarChar).Value = Session("ssCodimpadm")
                command.Parameters.Add("@apo", SqlDbType.VarChar).Value = apo
                command.Parameters.Add("@actoadmin", SqlDbType.VarChar).Value = Request("acto")
                command.Parameters.Add("@hora", SqlDbType.VarChar).Value = Date.Now.Hour
                command.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = lblExpediente.Text.Trim
                command.Parameters.Add("@DETALLE1", SqlDbType.VarChar).Value = Listadetalle.SelectedItem.Text
                command.Parameters.Add("@cedula", SqlDbType.VarChar).Value = cedula
                command.Parameters.Add("@deunom", SqlDbType.VarChar).Value = deunom
                command.Parameters.Add("@doc_apocc", SqlDbType.VarChar).Value = txtCedulanit.Text
                command.Parameters.Add("@doc_apofechaexpcc", SqlDbType.Date).Value = FechaWebLocal(txtFechaExpedicion.Text)
                command.Parameters.Add("@doc_apolugarexpcc", SqlDbType.VarChar).Value = txtExpedicion.Text.Trim
                command.Parameters.Add("@doc_deudirest", SqlDbType.VarChar).Value = tabla.Rows(0).Item(6).ToString

                If actualizar = 1 Then
                    command.CommandText = UCase("UPDATE entra_documentoma SET doc_fechaNotificacion=@fechaNotificacion,doc_horanotif=@hora,doc_deudorcedula=@cedula,doc_deudornombre=@deunom,doc_deudirest=@doc_deudirest,doc_apocc=@doc_apocc,doc_apofechaexpcc=@doc_apofechaexpcc,doc_apolugarexpcc=@doc_apolugarexpcc,doc_aponombre='" & txtNombre.Text & "',doc_apotarjetapro='" & txtTarjetaAbogado.Text & "',doc_expediente='" & lblExpediente.Text & "',doc_tipoapo=@apo,doc_memo='" & mtext.Value & "',doc_fecharac=@fecharac,doc_actoadministrativo='" & Request("acto") & "',doc_liqtot=@TOTAL,doc_liqint=@INTERES,doc_liqabo=@PAGOS,doc_impuesto=@IMPUESTO,doc_detalle1=@DETALLE1 WHERE doc_expediente ='" & lblExpediente.Text.Trim & "' and doc_actoadministrativo = @actoadmin")
                    command.ExecuteNonQuery()
                Else
                    command.CommandText = UCase("INSERT INTO entra_documentoma (RESOLUCION, doc_fechaNotificacion,doc_horanotif,doc_deudorcedula,doc_deudornombre,doc_deudirest,doc_apocc,doc_apofechaexpcc,doc_apolugarexpcc,doc_aponombre,doc_apotarjetapro,doc_expediente,doc_tipoapo,doc_memo,doc_fecharac,doc_actoadministrativo,doc_liqtot,doc_liqint,doc_liqabo,doc_impuesto,doc_detalle1) VALUES('" & txtResolucion.Text & "', @fechaNotificacion,@hora,@cedula,@deunom,@doc_deudirest,@doc_apocc,@doc_apofechaexpcc,@doc_apolugarexpcc,'" & txtNombre.Text & "','" & txtTarjetaAbogado.Text & "','" & lblExpediente.Text.Trim & "',@apo,'" & mtext.Value & "',@fecharac,'" & Request("acto") & "',@TOTAL,@INTERES,@PAGOS,@IMPUESTO,@DETALLE1" & ")")
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

    Private Sub ACTOPRE()
        Dim Adap As New SqlClient.SqlDataAdapter("SELECT DXI_ACTO_PREVIO, B.nombre FROM DOCUMENTO_INFORMEXIMPUESTO A , ACTUACIONES B WHERE DXI_ACTO_PREVIO = codigo AND DXI_ACTO = @acto AND DXI_IMPUESTOVALUE = @IMP", Funciones.CadenaConexion)
        Adap.SelectCommand.Parameters.Add("@IMP", SqlDbType.VarChar).Value = Session("ssimpuesto")
        Adap.SelectCommand.Parameters.Add("@acto", SqlDbType.VarChar).Value = Request("acto")
        Dim _tabla As New DataTable
        Adap.Fill(_tabla)

        If _tabla.Rows.Count > 0 Then
            ViewState("ActoPre") = _tabla.Rows(0).Item("DXI_ACTO_PREVIO").ToString
            ViewState("NombreActoPre") = _tabla.Rows(0).Item("nombre").ToString
        Else
            ViewState("ActoPre") = ""
            ViewState("NombreActoPre") = ""
        End If
    End Sub

    Private Sub NumResolucion()
        Dim ACTO As String = ViewState("ActoPre").ToString
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT *  FROM DOCUMENTOS_GENERADOS WHERE DG_EXPEDIENTE = @expediente and DG_COD_ACTO=@acto", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@expediente", SqlDbType.VarChar).Value = Request("expediente")
            myadapter.SelectCommand.Parameters.Add("@acto", SqlDbType.VarChar).Value = ACTO

            Using myTable As New DataTable
                myadapter.Fill(myTable)
                If myTable.Rows.Count > 0 Then
                    txtResolucion.Text = myTable.Rows(0).Item(3).ToString
                    txtFechaExpedicion.Text = myTable.Rows(0).Item(4).ToString
                Else
                    txtResolucion.Text = "NO APLICA"

                End If
            End Using
        End Using
    End Sub

    Private Sub reporte()
        Response.Redirect("cobranzas2.aspx?cedula=" & Request("cedula") & "&expediente=" & Request("expediente") & "&acto=" & Request("acto") & "&nomacto=" & Request("nomacto") & "&tipo=1&reporte=1", True)
    End Sub

    Private Sub UpdateActoPre()
        Dim CMD As New SqlCommand
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT docActoPre FROM DOCUMENTOS WHERE DOCEXPEDIENTE = @expediente AND docimpuesto = @IMP AND idacto = '122'", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@expediente", SqlDbType.VarChar).Value = Request("expediente")
            myadapter.SelectCommand.Parameters.Add("@imp", SqlDbType.VarChar).Value = Session("ssimpuesto")

            Using myTable As New DataTable
                myadapter.Fill(myTable)
                cn.Open()
                If myTable.Rows.Count > 0 Then
                    CMD = New SqlCommand("UPDATE DOCUMENTO_INFORMEXIMPUESTO SET DXI_ACTO_PREVIO = '" & myTable.Rows(0).Item("docActoPre").ToString & "' WHERE DXI_IMPUESTOVALUE = " & Session("ssimpuesto") & " AND DXI_ACTO = '017' ", cn)
                Else
                    CMD = New SqlCommand("UPDATE DOCUMENTO_INFORMEXIMPUESTO SET DXI_ACTO_PREVIO = '122' WHERE DXI_IMPUESTOVALUE = " & Session("ssimpuesto") & " AND DXI_ACTO = '017' ", cn)
                End If
                CMD.ExecuteNonQuery()
                cn.Close()
            End Using
        End Using
    End Sub

    Private Sub UpdateActoPre023()
        Dim CMD As New SqlCommand
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT docActoPre FROM DOCUMENTOS WHERE DOCEXPEDIENTE = @expediente AND docimpuesto = @IMP AND idacto = '035'", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@expediente", SqlDbType.VarChar).Value = Request("expediente")
            myadapter.SelectCommand.Parameters.Add("@imp", SqlDbType.VarChar).Value = Session("ssimpuesto")

            Using myTable As New DataTable
                myadapter.Fill(myTable)
                cn.Open()
                If myTable.Rows.Count > 0 Then
                    CMD = New SqlCommand("UPDATE DOCUMENTO_INFORMEXIMPUESTO SET DXI_ACTO_PREVIO = '" & myTable.Rows(0).Item("docActoPre").ToString & "' WHERE DXI_IMPUESTOVALUE = " & Session("ssimpuesto") & " AND DXI_ACTO = '023' ", cn)
                Else
                    CMD = New SqlCommand("UPDATE DOCUMENTO_INFORMEXIMPUESTO SET DXI_ACTO_PREVIO = '035' WHERE DXI_IMPUESTOVALUE = " & Session("ssimpuesto") & " AND DXI_ACTO = '023' ", cn)
                End If
                CMD.ExecuteNonQuery()
                cn.Close()
            End Using
        End Using
    End Sub



End Class