Imports System.Data.SqlClient
Partial Public Class predialvariable089
    Inherits System.Web.UI.Page

    Private sw As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not IsPostBack Then
        End If
    End Sub

    Protected Sub txtNumExp_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtNumExp.TextChanged
        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        Dim cmd As String
        cmd = "SELECT TOP 1 docexpediente FROM documentos WHERE docexpediente = '" & txtNumExp.Text.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, conec)
        Dim dtDocumentos As New DataTable
        Adaptador.Fill(dtDocumentos)
        If dtDocumentos.Rows.Count = 0 Then
            LBLRES.Text = "El expediente digitado no existe en la base de datos"
        Else
            'CustomValidator1.Text = ""
            LBLRES.Text = ""
        End If
        '2. Buscar el expediente en la tabla Registro_excepciones. Si existe=> mostrar campos, sino dejarlos en blanco
        cmd = "SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='089'"
        Dim Adaptador2 As New SqlDataAdapter(cmd, conec)
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)
        If dtRegistro_excepciones.Rows.Count > 0 Then
            'Mostrar campos
            txtCalendario.Text = dtRegistro_excepciones.Rows(0).Item(1)
            TextArea1.Value = dtRegistro_excepciones.Rows(0).Item(5).ToString
        Else
            'Dejar controles en blanco
            txtCalendario.Text = ""
            TextArea1.Value = ""
        End If
    End Sub

    Protected Sub Link_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Link.Click
        'GUARDAR INFORMACION ------
        sw = save()
        'FIN DEL GUARDADO


        'INICIAR PROCESO DEL REPORTE
        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        Dim MANDANIENTO_PAGO As New DataTable("MANDANIENTO_PAGO")
        Dim CAT_CLIENTES As New DataTable("CAT_CLIENTES")
        Dim Registro_excepciones As New DataTable("Registro_excepciones")
        Dim adap As New SqlClient.SqlDataAdapter
        Dim Dts As New DataSet("Reportes_Admistratiivos")




        Dim cmd As New SqlCommand("SELECT codigo AS ID_CLIENTE, nombre AS NOMBRE,ent_foto AS FOTO, ent_firma,ent_pref_exp,ent_pref_res,ent_tesorero,ent_foto2,ent_foto3 FROM entescobradores  WHERE codigo = '01'", conec)
        adap = New SqlDataAdapter(cmd)
        adap.Fill(CAT_CLIENTES)
        Dts.Tables.Add(CAT_CLIENTES)

        Dim cmd1 As New SqlCommand(" SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,E.EFIGEN AS MAN_REFCATRASTAL, 												     " & _
                                                            " E.EfiMatInm AS MAN_MATINMOB,  E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,   	                                                     " & _
                                                            " E.EFIDIR AS MAN_DIR_ESTABL,   E.EFIPERDES AS MAN_EFIPERDES, E.EfiSubDes  AS MAN_EFISUBDES,E.EFIPERHAS   AS MAN_EFIPERHAS,	                 " & _
                                                            " E.EfiSubHas AS MAN_EFISUBHAS, SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,   SUM(L.EDC_TOTALABO) AS MAN_PAGOS, SUM(L.EDC_INTERES) AS MAN_INTERESES,   " & _
                                                            " SUM(L.EDC_TOTALDEUDA) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE                                                            " & _
                                                            " FROM EJEFISGLOBAL E,  EJEFISGLOBALLIQUIDAD L                                                                                               " & _
                                                            " WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente    	                     " & _
                                                            " AND E.EfiModCod = @Impuesto  	                                                                                                             " & _
                                                            " GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EFISUBHAS                    ", conec)

        cmd1.Parameters.AddWithValue("@Expediente", txtNumExp.Text)
        cmd1.Parameters.AddWithValue("@Impuesto", Session("ssimpuesto"))
        cmd1.Parameters.AddWithValue("@IMP", Session("ssCodimpadm"))

        adap = New SqlDataAdapter(cmd1)
        adap.Fill(MANDANIENTO_PAGO)
        Dts.Tables.Add(MANDANIENTO_PAGO)



        Dim cmd2 As New SqlCommand("SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='089'", conec)

        adap = New SqlDataAdapter(cmd2)
        adap.Fill(Registro_excepciones)
        Dts.Tables.Add(Registro_excepciones)


        Dim Reporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Reporte = New resolucion_ordena_la_suspension_del_proceso_089


        SharedFunctions.Reports.ReportSetDataSource(Reporte, Dts)
        SharedFunctions.Reports.Exportar(Me, Reporte, "ordena_la_suspension_del_proceso(89).pdf")
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        save()
    End Sub

    Private Function save() As Boolean

        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        Dim cmd As String
        cmd = "SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='089'"
        Dim Adaptador2 As New SqlDataAdapter(cmd, conec)
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)

        conec.Open()

        Try

        
            Dim cmd2 As SqlCommand
            If dtRegistro_excepciones.Rows.Count > 0 Then
                'UPDATE
                cmd2 = New SqlCommand("UPDATE Registro_excepciones SET tipo_excepcion=@codigo,fecha_de_acto=@fecha,usuario=@user,consideraciones=@texto WHERE numero_expediente=@codigoExpediente AND idacto='089'", conec)
            Else
                'INSERT            
                cmd2 = New SqlCommand("INSERT INTO Registro_excepciones(tipo_excepcion,fecha_de_acto,numero_expediente,usuario,consideraciones,idacto,IMPUESTO) VALUES (@codigo,@fecha,@codigoExpediente,@user,@texto,'089',@IMPUESTO)", conec)
                sw = True
            End If
            cmd2.Parameters.AddWithValue("@codigo", Me.DropCodigoActo.Text)
            cmd2.Parameters.AddWithValue("@texto", Me.TextArea1.Value)
            cmd2.Parameters.AddWithValue("@user", Session("sscodigousuario"))
            cmd2.Parameters.AddWithValue("@fecha", Me.txtCalendario.Text)
            cmd2.Parameters.AddWithValue("@codigoExpediente", Me.txtNumExp.Text.Trim)
            cmd2.Parameters.AddWithValue("@IMPUESTO", Session("ssimpuesto"))

            cmd2.ExecuteNonQuery()
            conec.Close()

            reporte()
        Catch ex As Exception
            ex.ToString()
        End Try

        Return sw
    End Function



    Private Sub reporte()
        Response.Redirect("cobranzas2.aspx?expediente=" & txtNumExp.Text.Trim & "&acto=089&nomacto=RESOLUCIÓN ORDENA SUSPENSIÓN DE PROCESO COACTIVO&tipo=1&reporte=1", True)
    End Sub


End Class