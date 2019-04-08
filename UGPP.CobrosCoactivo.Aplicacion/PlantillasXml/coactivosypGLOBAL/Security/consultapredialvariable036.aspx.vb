Imports System.Data.SqlClient
Partial Public Class consultapredialvariable036
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
        End If

        If Not Me.Page.IsPostBack Then
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            txtNumExp.Focus()

        End If
    End Sub

    Protected Sub LinkCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkCancelar.Click
        Response.Redirect("cobranzas2.aspx")
    End Sub


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        Dim cmd As String
        Dim user As String = Session("sscodigousuario")

        cmd = "SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='036'"
        Dim Adaptador2 As New SqlDataAdapter(cmd, conec)
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)

        conec.Open()
        Dim cmd2 As SqlCommand
        If dtRegistro_excepciones.Rows.Count > 0 Then
            'UPDATE
            cmd2 = New SqlCommand("UPDATE Registro_excepciones SET tipo_excepcion=@codigo,fecha_de_acto=@fecha,usuario=@user where numero_expediente='" & Me.txtNumExp.Text.Trim & "' and idacto='036'", conec)
        Else
            'INSERT
            cmd2 = New SqlCommand("INSERT INTO Registro_excepciones(tipo_excepcion,fecha_de_acto,numero_expediente,usuario,idacto) VALUES (@codigo,@fecha,@codigoExpediente,@user,'036')", conec)
        End If
        cmd2.Parameters.AddWithValue("@codigo", Me.DropNumero_expediente.Text.Trim)
        cmd2.Parameters.AddWithValue("@user", user)
        cmd2.Parameters.AddWithValue("@fecha", Me.txtCalendario.Text)
        cmd2.Parameters.AddWithValue("@codigoExpediente", Me.txtNumExp.Text.Trim)
        cmd2.ExecuteNonQuery()
        conec.Close()

        reporte()
    End Sub

    Protected Sub txtNumExp_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtNumExp.TextChanged
        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        Dim cmd As String
        cmd = "SELECT TOP 1 docexpediente FROM documentos WHERE docexpediente = '" & txtNumExp.Text.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, conec)
        Dim dtDocumentos As New DataTable
        Adaptador.Fill(dtDocumentos)
        If dtDocumentos.Rows.Count = 0 Then
            Validator.Text = "El expediente digitado no existe en la base de datos"
            Me.Validator.IsValid = False
        Else
            'CustomValidator1.Text = ""
            Validator.Text = ""
            Me.Validator.IsValid = True

        End If

        '2. Buscar el expediente en la tabla Registro_excepciones. Si existe=> mostrar campos, sino dejarlos en blanco
        cmd = "SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='036'"
        Dim Adaptador2 As New SqlDataAdapter(cmd, conec)
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)
        If dtRegistro_excepciones.Rows.Count > 0 Then
            'Mostrar campos
            txtCalendario.Text = dtRegistro_excepciones.Rows(0).Item(1)
            DropNumero_expediente.Text = dtRegistro_excepciones.Rows(0).Item(0).ToString
        Else
            'Dejar controles en blanco
            txtCalendario.Text = ""
            DropNumero_expediente.SelectedIndex = 0
        End If

    End Sub
    Private Sub reporte()
        Dim conec As New SqlConnection(Funciones.CadenaConexion)

        Dim cmd As New SqlCommand("SELECT CODIGO AS ID_CLIENTE, NOMBRE AS NOMBRE, ENT_FOTO AS FOTO,ent_pref_exp,ent_pref_res,ent_tesorero, ent_firma FROM ENTESCOBRADORES  WHERE codigo = '01'", conec)
        Dim cmd1 As New SqlCommand(" SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,E.EFIGEN AS MAN_REFCATRASTAL, 												     " & _
                                                            " E.EfiMatInm AS MAN_MATINMOB,  E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,   	                                                     " & _
                                                            " E.EFIDIR AS MAN_DIR_ESTABL,   E.EFIPERDES AS MAN_EFIPERDES, E.EfiSubDes  AS MAN_EFISUBDES,E.EFIPERHAS   AS MAN_EFIPERHAS,	                 " & _
                                                            " E.EfiSubHas AS MAN_EFISUBHAS, SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,   SUM(L.EDC_TOTALABO) AS MAN_PAGOS, SUM(L.EDC_INTERES) AS MAN_INTERESES,   " & _
                                                            " SUM(L.EDC_TOTALDEUDA) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE,'' AS RESOLUCION                                                            " & _
                                                            " FROM EJEFISGLOBAL E,  EJEFISGLOBALLIQUIDAD L                                                                                               " & _
                                                            " WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente    	                     " & _
                                                            " AND E.EfiModCod = @Impuesto  	                                                                                                             " & _
                                                            " GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EFISUBHAS                    ", conec)

        cmd1.Parameters.AddWithValue("@Expediente", txtNumExp.Text)
        cmd1.Parameters.AddWithValue("@Impuesto", Session("ssimpuesto"))
        cmd1.Parameters.AddWithValue("@IMP", Session("ssCodimpadm"))
        Dim cmd2 As New SqlCommand("SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='036'", conec)
        Dim Dts As New Reportes_Admistratiivos

        Dim adap As New SqlClient.SqlDataAdapter
        adap.SelectCommand = cmd1

        adap.Fill(Dts.Mandaniento_Pago)


        adap.SelectCommand = cmd
        adap.Fill(Dts.CAT_CLIENTES)


        adap.SelectCommand = (cmd2)
        adap.Fill(Dts.REGISTRO_EXCEPCIONES)


        Dim Reporte As New CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim imp As Integer = Session("ssimpuesto")
        Select Case imp
            Case 1
                Reporte = New predial036
            Case 2
                Reporte = New predial036
        End Select

        saveResolucion(txtNumExp.Text, "036", Dts)
        Funciones.Exportar(Me, Reporte, Dts, "reporte036.pdf", "", Nothing)

    End Sub

    Private Sub saveResolucion(ByVal expediente As String, ByVal acto As String, ByVal Dts As Reportes_Admistratiivos)
        Dim resolucion As DataTable = Funciones.InsertReslucion(expediente, acto)
        If resolucion.Rows.Count > 0 Then
            If Dts.Tables("MANDANIENTO_PAGO").Rows.Count > 0 Then
                '---Cambiar en Tabla de mandaniento de pago
                Dts.Tables("MANDANIENTO_PAGO").Rows(0).Item("RESOLUCION") = resolucion.Rows(0).Item(3)
            End If
        End If
    End Sub
End Class