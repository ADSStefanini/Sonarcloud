Imports System.Data.SqlClient
Partial Public Class NuevoExpeIndividual
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Call Ter_Acto_Administrativo()
            ultimo()
            Validator.Text = "Para continuar genere un documento."

            If Session("ssimpuesto") = 1 Then
                LabelTexto.InnerHtml = "Predio"
            Else
                LabelTexto.InnerHtml = "Placa"
            End If
        End If
    End Sub

    Private Sub ultimo()
        Dim myadapter As New SqlDataAdapter("SELECT * FROM MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = 7", Funciones.CadenaConexion) ' Session("ConexionServer").ToString)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1
        lblcodigo.Text = Format(Con, "00000000")
    End Sub

    Private Sub Ter_Acto_Administrativo()
        Using connection As New SqlConnection(Funciones.CadenaConexion)
            Dim command As SqlCommand = New SqlCommand("SELECT TOP 1 (DEP_DEPENDENCIA + '-' + DEP_DESCRIPCION) ULTIMO,DEP_CODACTO,DEP_NOMBREPPAL,DEP_TERMINO FROM DEPENDENCIA_ACTUACIONES", connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
            Dim mytb As New DataTable
            mytb.Load(reader)

            If mytb.Rows.Count > 0 Then
                message.InnerHtml = "<img src='../images/icons/159.png' alt='Actos Administrativos' style='vertical-align: middle;outline:0' /> " & mytb.Rows(0).Item("DEP_CODACTO").ToString.ToUpper & " - " & mytb.Rows(0).Item("DEP_NOMBREPPAL").ToString.ToUpper
                Me.ViewState("datos_ultimo") = CType(mytb, DataTable)
            End If

            reader.Close()
        End Using
    End Sub

    Protected Sub bntGenerar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bntGenerar.Click
        SaveCaratula(txtVigenciaInicial.Text, txtVigenciaFinal.Text, Textbus.Text)
    End Sub

    Private Sub SaveCaratula(ByVal vigenciaDesde As String, ByVal vigenciaHasta As String, ByVal predio As String)
        'GlobalExpedintes.Mandaniento_Pago.Clear()
        Dim SQuery As String = ""
        If Not IsNumeric(txtVigenciaInicial.Text Or txtVigenciaFinal.Text) Then
            Validator.Text = "Digite un valor correcto en los rangos de fecha"
            Exit Sub
        End If

        If predio <> "" Then
            SQuery += "AND EDC_ID = '" & predio & "' AND EDC_VIGENCIA BETWEEN '" & vigenciaDesde & "' AND '" & vigenciaHasta & "' AND EDC_CODIGO_IMPUESTO = " & Session("ssimpuesto").ToString
        Else
            Validator.Text = "Error : Digite todo los datos."
            Exit Sub
        End If

        If CInt(vigenciaDesde) <= CInt(vigenciaHasta) Then
            Dim connection As SqlConnection
            Dim adapter As SqlDataAdapter
            Dim ArchiNomb As String = ""           
            Dim tb As New DataTable
            connection = New SqlConnection(Funciones.CadenaConexion.ToString)
            'connection.Open()
            Dim command As New SqlCommand("REGISTRAR_EXPEDIENTE_NUEVO", connection)
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.AddWithValue("@TOP", "")
            command.Parameters.Add("@COBRADOR", SqlDbType.NVarChar, 2).Value = Session("mcobrador")
            command.Parameters.AddWithValue("@USER", Session("sscodigousuario").ToString)
            command.Parameters.AddWithValue("@FILTRO", SQuery)

            adapter = New SqlDataAdapter(command)
            adapter.Fill(tb)
            If tb.Rows.Count > 0 Then
                Dim Test As String = ""
                'Dim cobranzasMasiva As New frmCobroMasivo
                Dim Mytbdocumento As New DatasetForm.documentosDataTable
                Dim myBaseClass As New procesos_tributario.sqlQuery.sqlQuery()
                myBaseClass.Impuesto = Session("ssimpuesto")
                myBaseClass.Impuesto_Literal = Session("ssCodimpadm")
                Dim cr As CrystalDecisions.CrystalReports.Engine.ReportDocument = myBaseClass.QueReporte("001")
                For Each row As DataRow In tb.Rows
                    Test = doc_Documento(6)
                    ArchiNomb = "TE" & Test & Date.Now.ToUniversalTime.ToString("ddMMyyyyHHmmss") & ".pdf"
                    Mytbdocumento.AdddocumentosRow(row("EFINIT"), "001", "ruta", ArchiNomb, 1, Date.Now.ToString("yyyy-MM-dd"), Session("mcobrador"), row("EFINROEXP"), lblcodigo.Text, row("EFIGEN"), Nothing, Date.Now, "Nuevo individual", 0, Session("sscodigousuario"), Date.Now, "S", Session("ssimpuesto"), Nothing, "001")
                    Exportar_Reportes_Masivos(Me.Page, cr, DtsDatosReturn(row), ArchiNomb, Session("ssrutaexpediente"))
                Next
                ViewState("Arch") = Session("ssrutaexpediente") & "\" & ArchiNomb
                Funciones.InsertDocDefinitivo(lblcodigo.Text, Mytbdocumento)
                Validator.Text = "El proceso ha terminado con exito..."
                LinkArchivo_expediente.Text = "DESCARGAR " & ArchiNomb
            Else
                Validator.Text = "No se afectaron registros"
            End If
        Else
            Validator.Text = "La vigencia inicial no puede ser mayor a la vigencia final"
        End If
    End Sub

    Private Function DtsDatosReturn(ByVal Rows As DataRow) As DataSet
        Dim DtsDatos As Reportes_Admistratiivos = New Reportes_Admistratiivos
        Load_Configuracion(Session("mcobrador"), DtsDatos.CAT_CLIENTES)
        Dim row As Reportes_Admistratiivos.Mandaniento_PagoRow
        row = DtsDatos.Mandaniento_Pago.NewRow
        row("MAN_DEUSDOR") = Rows("EFINIT")
        row("MAN_IMPUESTO") = Session("ssCodimpadm")
        row("MAN_VALORMANDA") = DBNull.Value
        row("MAN_NOMDEUDOR") = Nothing
        row("MAN_DIRECCION") = Nothing
        row("MAN_DIR_ESTABL") = Nothing
        row("MAN_REFCATRASTAL") = Nothing
        row("MAN_VIGENCIA") = Nothing
        row("MAN_CONCEPTOCDG") = Nothing
        row("MAN_ESTRATOCD") = Nothing
        row("MAN_DESTINOCD2") = Nothing
        row("MAN_BASEGRAVABLE") = Nothing
        row("MAN_TARIFA") = DBNull.Value
        row("MAN_CAPITAL") = DBNull.Value
        row("MAN_INTERESES") = DBNull.Value
        row("MAN_TOTAL") = DBNull.Value
        row("MAN_EXPEDIENTE") = Rows("EFINROEXP")
        row("MAN_FECHADOC") = DBNull.Value
        row("MAN_EFIPERDES") = DBNull.Value
        row("MAN_EFIPERHAS") = DBNull.Value
        row("MAN_FECHARAC") = DBNull.Value

        If Session("ssimpuesto") = 1 Then
            row("SUB_SERIE") = ultimo(11)
        ElseIf Session("ssimpuesto") = 2 Then
            row("SUB_SERIE") = ultimo(12)
        Else
            row("SUB_SERIE") = DBNull.Value
        End If

        DtsDatos.Mandaniento_Pago.AddMandaniento_PagoRow(row)
        Return DtsDatos
    End Function


    Public Function Load_Configuracion(ByVal Ente As String, ByVal DtsDatos As Reportes_Admistratiivos.CAT_CLIENTESDataTable) As Reportes_Admistratiivos.CAT_CLIENTESDataTable
        Using con As New SqlConnection(Funciones.CadenaConexion)
            Dim ESAdap As New SqlDataAdapter
            Dim ESCommand As SqlCommand
            ESCommand = New SqlCommand

            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            ESCommand.Connection = con
            ESCommand.CommandText = "SELECT codigo as ID_CLIENTE, nombre as NOMBRE,ent_foto as FOTO,ent_pref_exp,ent_pref_res,ent_tesorero, ent_firma,ent_foto2,ent_foto3 FROM entescobradores WHERE codigo = @Cobrador"
            ESCommand.Parameters.Add("@Cobrador", SqlDbType.VarChar)
            ESCommand.Parameters("@Cobrador").Value = Ente

            ESAdap.SelectCommand = ESCommand
            ESAdap.Fill(DtsDatos)

            Return DtsDatos
        End Using
    End Function

    Public Function doc_Documento(ByVal consecutivo As Integer) As String
        Using con As New SqlConnection(Funciones.CadenaConexion)
            con.Open()
            Dim tran As SqlTransaction = con.BeginTransaction

            Dim proximo_numero As String = "00000000"
            Dim mycommand As New SqlCommand("UPDATE MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = @conse", con)
            mycommand.Parameters.Add("@proximo_numero", SqlDbType.Int)
            mycommand.Parameters.Add("@conse", SqlDbType.Int).Value = consecutivo
            mycommand.Parameters("@proximo_numero").Direction = ParameterDirection.Output
            mycommand.Transaction = tran
            mycommand.ExecuteNonQuery()
            Dim conse As Integer = CType(mycommand.Parameters("@proximo_numero").Value, Integer)
            proximo_numero = Format(conse, "00000000")

            Try
                tran.Commit()
            Catch ex As Exception
                tran.Rollback()
                Validator.Text = "Error :" & ex.Message & "<br /><b>Nota: </b> Error de base de dato  o la  sesión caduco."
                proximo_numero = "no"
            End Try

            Return proximo_numero
        End Using
    End Function

    Protected Sub LinkArchivo_expediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkArchivo_expediente.Click
        If Me.ViewState("Arch") <> Nothing Then
            Response.Redirect("download.ashx?documento=" & ViewState("Arch"))
        Else
            Validator.Text = "Error : Archivo no encontrado."
        End If
    End Sub

    Protected Sub LinkConsultaNroPre_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkConsultaNroPre.Click
        Using connection As New SqlConnection(Funciones.CadenaConexion)
            Dim command As SqlCommand = New SqlCommand("SELECT TOP 100 EDC_NITCC,EDC_NOMBRE,COUNT(DISTINCT(EDC_ID)) AS NROPREDIO FROM EJEFISGLOBALLIQUIDAD WHERE EDC_EN_COBRO = 0 AND EDC_CODIGO_IMPUESTO = @IMP GROUP BY EDC_NITCC,EDC_NOMBRE ORDER BY NROPREDIO DESC", connection)
            command.Parameters.AddWithValue("@IMP", Session("ssimpuesto"))
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
            Dim mytb As New DataTable
            mytb.Load(reader)

            ViewState("ConsultaNroPr_datos") = CType(mytb, DataTable)
            GridPredios.DataSource = mytb
            GridPredios.DataBind()
            reader.Close()
        End Using
    End Sub

    Protected Sub GridPredios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridPredios.SelectedIndexChanged
        With Me
            Dim Mytb As DataTable = CType(.ViewState("ConsultaNroPr_datos"), DataTable)
            Dim index As Integer = GridPredios.SelectedIndex
            Dim Nitcc As String = Mytb.Rows(index).Item("EDC_NITCC")
            Dim Nombre As String = Mytb.Rows(index).Item("EDC_NOMBRE")

            Dim command As New SqlCommand("SELECT A.EDC_NITCC,A.EDC_NOMBRE,A.EDC_ID,EDC_DIRECCION,EDC_ESTRATO FROM EJEFISGLOBALLIQUIDAD A WHERE EDC_EN_COBRO = 0 AND A.EDC_NITCC = @EDC_NITCC AND A.EDC_NOMBRE = @EDC_NOMBRE GROUP BY A.EDC_NITCC,A.EDC_NOMBRE,EDC_ID,EDC_DIRECCION,EDC_ESTRATO", New SqlConnection(Funciones.CadenaConexion))
            command.Parameters.Add("@EDC_NOMBRE", SqlDbType.VarChar).Value = Nombre
            command.Parameters.Add("@EDC_NITCC", SqlDbType.VarChar).Value = Nitcc

            command.Connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
            Dim mytbNroExpediente As New DatasetForm
            mytbNroExpediente.NROPREDIOXDEUDOR.Load(reader)
            Logo_Load_Configuracion(mytbNroExpediente.CAT_CLIENTES, Session("mcobrador"))
            reader.Close()

            Dim cr As New rptNroPredio_x_Deudor
            Dim List As New List(Of ItemParams)
            For Each Par As CrystalDecisions.Shared.ParameterField In cr.ParameterFields
                 Select Par.Name
                    Case "IMPUESTO"
                        List.Add(New ItemParams("IMPUESTO", Session("ssCodimpadm")))
                End Select
            Next

            Funciones.Exportar(Me, cr, mytbNroExpediente, "Impresion.Pdf", "", List)
        End With
    End Sub

    Private Function ultimo(ByVal id As Integer) As Integer
        Dim myadapter As New SqlDataAdapter("select * from MAESTRO_CONSECUTIVOS where CON_IDENTIFICADOR = @id", Funciones.CadenaConexion)
        myadapter.SelectCommand.Parameters.AddWithValue("@id", id)
        Dim tbu As New DataTable
        myadapter.Fill(tbu)
        Dim Con As Integer
        Con = tbu.Rows(0).Item("CON_USER") + 1

        'actualizar consecutivo
        Dim cnn As New SqlConnection(Funciones.CadenaConexion)
        Dim cmd As New SqlCommand("UPDATE MAESTRO_CONSECUTIVOS SET CON_USER = CON_USER +1 WHERE CON_IDENTIFICADOR = @id", cnn)
        cmd.Parameters.AddWithValue("@id", id)
        cnn.Open()
        cmd.ExecuteNonQuery()
        cmd.Clone()

        Return Con
    End Function


End Class