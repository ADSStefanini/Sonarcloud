Imports System.Data.SqlClient
Imports System.Math


Partial Public Class EditTDJ
    Inherits System.Web.UI.Page
    ''Dim nrotituloprincipal As String = Request("titulo_principal")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboEstadoTDJ()
            LoadcboConsignante()
            LoadcboBanco()
            LoadcboTipoResolGes()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from TDJ where [NroDeposito] = @NroDeposito"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @NroDeposito parameter in the Command select query
                Command.Parameters.AddWithValue("@NroDeposito", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtNroDeposito.Text = Reader("NroDeposito").ToString()
                    txtNroTituloJ.Text = Reader("NroTituloJ").ToString()
                    txtValorTDJ.Text = Reader("ValorTDJ").ToString()
                    cboEstadoTDJ.SelectedValue = Reader("EstadoTDJ").ToString()

                    txtFecRecibido.Text = Left(Reader("FecRecibido").ToString().Trim, 10)
                    txtFecEmision.Text = Left(Reader("FecEmision").ToString().Trim, 10)

                    cboConsignante.SelectedValue = Reader("Consignante").ToString()
                    cboBanco.SelectedValue = Reader("Banco").ToString()
                    txtObservac.Text = Reader("Observac").ToString()
                    txtNroResolGes.Text = Reader("NroResolGes").ToString()

                    txtFecResolGes.Text = Left(Reader("FecResolGes").ToString().Trim, 10)
                    txtNroMemoGes.Text = Reader("NroMemoGes").ToString()
                    txtFecEnvioMemo.Text = Left(Reader("FecEnvioMemo").ToString().Trim, 10)
                    cboTipoResolGes.SelectedValue = Reader("TipoResolGes").ToString()
                    txtFecDevol.Text = Left(Reader("FecDevol").ToString().Trim, 10)

                    


                End If

               
                ' 
                'Close the Data Reader we are done with it.
                Reader.Close()

                'Close the Connection Object 
                Connection.Close()
                ' 
                'The length of ID equals zero.
                'This is an insert so don't preload any data.
            Else
                ' 
                'Since this is an insert then you can't delete it yet because it's not in the database.
                'cmdDelete.Visible = False
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdSave.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten adicionar datos"
                CustomValidator1.IsValid = False

                'Desactivar controles
                txtNroDeposito.Enabled = False
                txtNroTituloJ.Enabled = False
                txtValorTDJ.Enabled = False
                cboEstadoTDJ.Enabled = False
                txtFecRecibido.Enabled = False
                txtFecEmision.Enabled = False
                cboConsignante.Enabled = False
                cboBanco.Enabled = False
                txtObservac.Enabled = False
                txtNroResolGes.Enabled = False
                txtFecResolGes.Enabled = False
                txtNroMemoGes.Enabled = False
                txtFecEnvioMemo.Enabled = False
                cboTipoResolGes.Enabled = False
                txtFecDevol.Enabled = False

            End If

        End If

        If txtNroTituloJ.Text = "" Then
            Pnl_Admin.Visible = False
        Else
            Pnl_Admin.Visible = True

        End If
    End Sub

    Protected Sub LoadcboEstadoTDJ()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [ESTADOS_TDJ] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboEstadoTDJ.DataTextField = "nombre"
        cboEstadoTDJ.DataValueField = "codigo"
        cboEstadoTDJ.DataSource = Command.ExecuteReader()
        cboEstadoTDJ.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboConsignante()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = _
                             " select ED_Codigo_Nit as ccnit, ED_Nombre as nombre " & _
                             " FROM  ENTES_DEUDORES C ,DEUDORES_EXPEDIENTES A , " & _
                             " DEUDORES_EXPEDIENTES B " & _
                             " where " & _
                             " A.deudor = c.ED_Codigo_Nit " & _
                             " and b.deudor =c.ED_Codigo_Nit  " & _
                             " and a.NroExp =@expediente      "

        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@expediente", Request("pExpediente"))
        cboConsignante.DataTextField = "nombre"
        cboConsignante.DataValueField = "ccnit"
        cboConsignante.DataSource = Command.ExecuteReader()
        cboConsignante.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboBanco()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select BAN_CODIGO, BAN_NOMBRE from [MAESTRO_BANCOS] order by BAN_NOMBRE"
        Dim Command As New SqlCommand(sql, Connection)
        cboBanco.DataTextField = "BAN_NOMBRE"
        cboBanco.DataValueField = "BAN_CODIGO"
        cboBanco.DataSource = Command.ExecuteReader()
        cboBanco.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub


    Protected Sub LoadcboTipoResolGes()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [TIPOS_RESOLTDJ] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboTipoResolGes.DataTextField = "nombre"
        cboTipoResolGes.DataValueField = "codigo"
        cboTipoResolGes.DataSource = Command.ExecuteReader()
        cboTipoResolGes.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Public Function ExisteTDJ(ByVal pNroDeposito As String) As Boolean
        Dim sw As Boolean = False
        Dim IdEmbargo As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT IdEmbargo FROM TDJ WHERE NroDeposito = '" & pNroDeposito & "'"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
            IdEmbargo = Reader("IdEmbargo").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim nrotituloprincipal As String = Request("titulo_principal")
        If txtNroDeposito.Text.Trim = "" Or txtNroTituloJ.Text.Trim = "" Then
            CustomValidator1.Text = "No. de Depósito y No. de Título Judicial son campos obligatorios "
            CustomValidator1.IsValid = False
            Return
        End If

        If IsNumeric(txtValorTDJ.Text) Then
            Dim tmpValor As Int32 = 0
            tmpValor = CInt(txtValorTDJ.Text.Trim)
            If tmpValor = 0 Then
                CustomValidator1.Text = "El valor del título debe ser mayor a cero (0)"
                CustomValidator1.IsValid = False
                Return
            End If
        Else
            CustomValidator1.Text = "Diligencie correctamente el valor del título"
            CustomValidator1.IsValid = False
            Return
        End If

        '27/08/2014
        'Dim ID As String = Request("ID")
        Dim NroDeposito As String = txtNroDeposito.Text.Trim
        Dim mExisteTDJ As Boolean = False
        mExisteTDJ = ExisteTDJ(NroDeposito)

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        ' 
        'Comandos SQL
        Dim InsertSQL As String = "Insert into TDJ ([NroDeposito], [NroTituloJ], [IdEmbargo], [ValorTDJ], [EstadoTDJ], [FecRecibido], [FecEmision], [Consignante], [Banco], [Gestor], [Observac], [NroResolGes], [FecResolGes], [NroMemoGes], [FecEnvioMemo], [TipoResolGes], [FecDevol],[NroTituloPrincipal]) VALUES ( @NroDeposito, @NroTituloJ, @IdEmbargo, @ValorTDJ, @EstadoTDJ, @FecRecibido, @FecEmision, @Consignante, @Banco, @Gestor, @Observac, @NroResolGes, @FecResolGes, @NroMemoGes, @FecEnvioMemo, @TipoResolGes, @FecDevol,@Nro_TituloPrincipal) "
        Dim UpdateSQL As String = "Update TDJ set [NroTituloJ] = @NroTituloJ, [IdEmbargo] = @IdEmbargo, [ValorTDJ] = @ValorTDJ, [EstadoTDJ] = @EstadoTDJ, [FecRecibido] = @FecRecibido, [FecEmision] = @FecEmision, [Consignante] = @Consignante, [Banco] = @Banco, [Gestor] = @Gestor, [Observac] = @Observac, [NroResolGes] = @NroResolGes, [FecResolGes] = @FecResolGes, [NroMemoGes] = @NroMemoGes, [FecEnvioMemo] = @FecEnvioMemo, [TipoResolGes] = @TipoResolGes, [FecDevol] = @FecDevol where [NroDeposito] = @NroDeposito "
        ' 
        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If Not mExisteTDJ Then
            ' insert            
            Command = New SqlCommand(InsertSQL, Connection)
            ID = txtNroDeposito.Text.Trim
            Command.Parameters.AddWithValue("@NroDeposito", NroDeposito)
        Else
            ' update             
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroDeposito", NroDeposito)
        End If

        'Parametros
        Command.Parameters.AddWithValue("@NroTituloJ", txtNroTituloJ.Text.Trim)
        Command.Parameters.AddWithValue("@IdEmbargo", Request("IdEmbargo").Trim)

        If IsNumeric(txtValorTDJ.Text) Then
            Command.Parameters.AddWithValue("@ValorTDJ", txtValorTDJ.Text)
        Else
            Command.Parameters.AddWithValue("@ValorTDJ", DBNull.Value)
        End If

        If cboEstadoTDJ.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@EstadoTDJ", cboEstadoTDJ.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@EstadoTDJ", DBNull.Value)
        End If

        If IsDate(Left(txtFecRecibido.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecRecibido", Left(txtFecRecibido.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecRecibido", DBNull.Value)
        End If

        If IsDate(Left(txtFecEmision.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecEmision", Left(txtFecEmision.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecEmision", DBNull.Value)
        End If

        If cboConsignante.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Consignante", cboConsignante.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Consignante", DBNull.Value)
        End If

        If cboBanco.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Banco", cboBanco.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Banco", DBNull.Value)
        End If

        'if cboGestor.SelectedValue.Length > 0 then 
        '    Command.Parameters.AddWithValue("@Gestor", cboGestor.SelectedValue)
        'else
        '    Command.Parameters.AddWithValue("@Gestor", DBNull.Value )
        'End If

        Command.Parameters.AddWithValue("@Gestor", "0007")
        Command.Parameters.AddWithValue("@Observac", txtObservac.Text.Trim)
        Command.Parameters.AddWithValue("@NroResolGes", txtNroResolGes.Text.Trim)

        If IsDate(Left(txtFecResolGes.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResolGes", Left(txtFecResolGes.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResolGes", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@NroMemoGes", txtNroMemoGes.Text)

        If IsDate(Left(txtFecEnvioMemo.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecEnvioMemo", Left(txtFecEnvioMemo.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecEnvioMemo", DBNull.Value)
        End If

        If cboTipoResolGes.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@TipoResolGes", cboTipoResolGes.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@TipoResolGes", DBNull.Value)
        End If

        If IsDate(Left(txtFecDevol.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecDevol", Left(txtFecDevol.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecDevol", DBNull.Value)
        End If

        If nrotituloprincipal = "" Then

            Command.Parameters.AddWithValue("@Nro_TituloPrincipal", DBNull.Value)
        Else
            Command.Parameters.AddWithValue("@Nro_TituloPrincipal", nrotituloprincipal)
        End If
        Try
            Command.ExecuteNonQuery()
            guardardeposito()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Títulos de Depósito Judicial", "No. depósito " & ID, Command)

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False

        End Try

        Connection.Close()

        Response.Redirect("EditDETALLE_EMBARGO.aspx?ID=" & Request("IdEmbargo") & "&pResolEm=" & Request("pResolEm") & "&pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("EditDETALLE_EMBARGO.aspx?ID=" & Request("IdEmbargo") & "&pResolEm=" & Request("pResolEm") & "&pExpediente=" & Request("pExpediente"))
    End Sub


    

    Private Sub guardardeposito()
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlcommand As SqlCommand

        sqlcommand = New SqlCommand("update DETALLE_EMBARGO  set NroDepositoTDJ ='" & txtNroDeposito.Text & "' where idunico ='" & Request("IdEmbargo") & "' AND ISNULL(NRODEPOSITOTDJ,'') ='' ")
        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Connection = sqlconfig

        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()
        End If
        sqlconfig.Open()
        sqlcommand.ExecuteNonQuery()
    End Sub
    'reporteador '

    Private Sub exportacion(ByVal op As String, Optional ByVal ref As String = "", Optional ByVal ref1 As String = "", Optional ByVal ref3 As String = "")
        ''generador_expedientes de reportes
        
        Dim worddoc As New WordReport
        Dim worddocresult As String = ""
        Dim DTR As DataTable = getdatosbasicos()
        Dim Res_Dato(3) As String

        Select Case op
            Case 1 'fraccionar titulo de deposito judicial'
                Dim resolucion() As String = getResolucion_anterior(Request("pExpediente").Trim, "364")
                Dim resolucion_Cobro() As String = getResolucion_anterior(Request("pExpediente").Trim, resolucion(2))
                Dim liquidacion As Double = GetLiquidacion(Request("pExpediente").Trim, resolucion_Cobro(0))

                If txtValorTDJ.Text > liquidacion Then
                    Dim datos_individual(13) As WordReport.Marcadores_Adicionales
                    '' Dim tdj() As String = GetTituloPrincipal(Expediente.Trim)
                    ''Dim resolucion() As String = getResolucion_anterior(Request("pExpediente").Trim, "364")
                    Dim fraccionamiento() As Double

                    datos_individual(1).Marcador = "letras"
                    datos_individual(1).Valor = Num2Text(GetLiquidacion(Request("pExpediente").Trim, resolucion_Cobro(0)))

                    Res_Dato = overloadresolucion(Request("pExpediente").Trim, "233")
                    If Res_Dato(0).Trim <> "" And Res_Dato(0) <> Nothing Then
                        resolucion(0) = Res_Dato(0)
                        resolucion(1) = CDate(Res_Dato(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If

                    If resolucion(0) <> Nothing Then
                        datos_individual(2).Marcador = "fecha_anterior"
                        datos_individual(2).Valor = resolucion(1)

                        datos_individual(3).Marcador = "resolucion_anterior"
                        datos_individual(3).Valor = resolucion(0)


                    Else
                        Alert("NO HAY RESOLUCION DE (233)APROBACION DE LIQUIDACION DE PARA GENERAR EL DOCUMENTO ")
                        Exit Sub
                    End If

                    Dim guardar() As String = saveResolucion(Request("pExpediente"), "364")

                    datos_individual(0).Marcador = "nro_resolucion"
                    datos_individual(0).Valor = guardar(0)
                    datos_individual(4).Marcador = "fecha_actual"
                    datos_individual(4).Valor = guardar(1)

                    datos_individual(5).Marcador = "Total_Liquidacion"
                    datos_individual(5).Valor = String.Format("{0:C0}", liquidacion)
                    datos_individual(6).Marcador = "nro_titulo"
                    datos_individual(6).Valor = txtNroTituloJ.Text.Trim
                    datos_individual(7).Marcador = "Ltotal"
                    datos_individual(7).Valor = String.Format("{0:C0}", CDbl(txtValorTDJ.Text.Trim))
                    datos_individual(8).Marcador = "Ltitulo"
                    datos_individual(8).Valor = Num2Text(txtValorTDJ.Text.Trim)
                    fraccionamiento = generarfraccionamiento(txtNroDeposito.Text.Trim, 0, txtNroTituloJ.Text.Trim, guardar(0), Now.Date)
                    datos_individual(9).Marcador = "titulo1"
                    datos_individual(9).Valor = String.Format("{0:C0}", fraccionamiento(0))
                    datos_individual(10).Marcador = "lprincipal"
                    datos_individual(10).Valor = Num2Text(fraccionamiento(0))
                    datos_individual(11).Marcador = "ltitulo2"
                    datos_individual(11).Valor = Num2Text(fraccionamiento(1))
                    datos_individual(12).Marcador = "titulo2"
                    datos_individual(12).Valor = String.Format("{0:C0}", fraccionamiento(1))
                    datos_individual(13).Marcador = "fecha_emision"
                    datos_individual(13).Valor = guardar(3)
                    worddocresult = worddoc.CreateReport(DTR, Reportes.FraccionamientoDepositoJudicial, datos_individual)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace("fraccionamiento de titulo de deposito judicial", " ", ".")
                        SendReport(nombre & Request("pExpediente").Trim & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If
                Else
                    Alert("NO SE PUEDE REALIZAR FRACCIONAMIENTO DEL TITULO DEBIDO A QUE EL VALOR DEL TITULO DE DEPOSITO ES INFERIOR AL  VALOR DE LA LIQUIDACION ")

                End If


            Case 2 'aplicar titulo'
                Dim resolucion() As String = getResolucion_anterior(Request("pExpediente").Trim, "364")

                If resolucion(0) <> "" Then
                    Dim tdj(2) As String

                    If Request("titulo_principal") = "" Or Request("titulo_principal") = Nothing Then
                        tdj = BuscarTituloPrincipal(txtNroTituloJ.Text.Trim)
                    Else
                        tdj = BuscarTituloPrincipal(Request("titulo_principal"))
                    End If

                    If tdj(0) = Nothing Or tdj(0) = "" Then
                        tdj(0) = txtNroTituloJ.Text.Trim
                        tdj(1) = txtValorTDJ.Text.Trim
                    End If

                    Dim nrotituloprincipal As String = tdj(0)
                    Dim valortdj As Double = CDbl(tdj(1))
                    Dim datos_individual(12) As WordReport.Marcadores_Adicionales
                    Dim resolucion_liquidacion() As String = getResolucion_anterior(Request("pExpediente").Trim, "364")
                    Dim tbl1 As DataTable = Getvalores(nrotituloprincipal)
                    Dim resolucion_Cobro() As String = getResolucion_anterior(Request("pExpediente").Trim, resolucion_liquidacion(2))

                    Res_Dato = overloadresolucion(Request("pExpediente").Trim, "233")
                    If Res_Dato(0).Trim <> "" And Res_Dato(0) <> Nothing Then
                        resolucion(0) = Res_Dato(0)
                        resolucion(1) = CDate(Res_Dato(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If


                    If resolucion(0) <> Nothing Then
                        datos_individual(2).Marcador = "fecha_anterior"
                        datos_individual(2).Valor = resolucion(1)

                        datos_individual(3).Marcador = "resolucion_anterior"
                        datos_individual(3).Valor = resolucion(0)


                    Else
                        Alert("NO HAY RESOLUCION DE (364)FRACCIONAMIENTO  DE TITULO DE DEPOSITO JUDICIAL DE PARA GENERAR EL DOCUMENTO ")
                        Exit Sub
                    End If

                    Dim guardar() As String = saveResolucion(Request("pExpediente"), "365")

                    datos_individual(0).Marcador = "nro_resolucion"
                    datos_individual(0).Valor = guardar(0)

                    datos_individual(1).Marcador = "fecha_actual"
                    datos_individual(1).Valor = guardar(1)

                    actualizar(guardar(0), guardar(1), nrotituloprincipal.Trim, 1, ref3)

                    datos_individual(4).Marcador = "letras_liquidacion"
                    datos_individual(4).Valor = Num2Text(GetLiquidacion(Request("pExpediente").Trim, resolucion_Cobro(0)))

                    datos_individual(5).Marcador = "Total_Liquidacion"
                    datos_individual(5).Valor = String.Format("{0:C0}", GetLiquidacion(Request("pExpediente").Trim, resolucion_Cobro(0)))

                    datos_individual(6).Marcador = "nro_titulo"
                    If nrotituloprincipal = "" Or nrotituloprincipal = Nothing Then
                        nrotituloprincipal = txtNroTituloJ.Text.Trim
                    End If

                    datos_individual(6).Valor = nrotituloprincipal.Trim
                    datos_individual(7).Marcador = "resolucion_L"
                    datos_individual(7).Valor = resolucion_liquidacion(0)
                    datos_individual(8).Marcador = "fecha_L"
                    datos_individual(8).Valor = resolucion_liquidacion(1)
                    datos_individual(9).Marcador = "titulo_judicial"
                    datos_individual(9).Valor = ref
                    datos_individual(10).Marcador = "vtitulo"
                    datos_individual(10).Valor = String.Format("{0:C0}", CDbl(ref1))
                    datos_individual(11).Marcador = "Letras_Titulo"
                    datos_individual(11).Valor = Num2Text(ref1)
                    datos_individual(12).Marcador = "fecha_emision"
                    datos_individual(12).Valor = guardar(3)

                    guardar_doc(guardar(0), guardar(2), txtNroDeposito.Text.Trim, Request("IdEmbargo").Trim, 1)

                    worddocresult = worddoc.CreateReportMultiTable(DTR, Reportes.AplicacionDepositoJudicial, datos_individual, tbl1, 0, False, Nothing, 0, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace("APLICACION DE TITULO DE DEPOSITO JUDICIAL", " ", ".")
                        SendReport(nombre & Request("pExpediente").Trim & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                Else

                    Dim tdj(2) As String

                    If Request("titulo_principal") = "" Or Request("titulo_principal") = Nothing Then
                        tdj = BuscarTituloPrincipal(txtNroTituloJ.Text.Trim)
                    Else
                        tdj = BuscarTituloPrincipal(Request("titulo_principal"))
                    End If

                    If tdj(0) = Nothing Or tdj(0) = "" Then
                        tdj(0) = txtNroTituloJ.Text.Trim
                        tdj(1) = txtValorTDJ.Text.Trim
                    End If

                    Dim nrotituloprincipal As String = tdj(0)
                    Dim valortdj As Double = CDbl(tdj(1))
                    Dim datos_individual(9) As WordReport.Marcadores_Adicionales
                    Dim tbl1 As DataTable = Getvalores2(Request("pExpediente").Trim)

                    Res_Dato = overloadresolucion(Request("pExpediente").Trim, "013")

                    If Res_Dato(0).Trim <> "" And Res_Dato(0) <> Nothing Then
                        resolucion(0) = Res_Dato(0)
                        resolucion(1) = CDate(Res_Dato(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else
                    End If

                    If resolucion(0) <> Nothing Then
                        datos_individual(2).Marcador = "fecha_anterior"
                        datos_individual(2).Valor = resolucion(1)

                        datos_individual(3).Marcador = "resolucion_anterior"
                        datos_individual(3).Valor = resolucion(0)


                    Else

                    End If

                    Dim guardar() As String = saveResolucion(Request("pExpediente"), "365")
                    Dim embargar() As String = LoadMultiEmbargos(Request("pExpediente"))
                    Dim sociedad() As String = loadMultisocios(Request("pExpediente").Trim, CDbl(DTR.Rows(0).Item("totaldeuda")))

                    datos_individual(0).Marcador = "nro_resolucion"
                    datos_individual(0).Valor = guardar(0)

                    datos_individual(1).Marcador = "fecha_actual"
                    datos_individual(1).Valor = guardar(1)

                    datos_individual(4).Marcador = "fecha_emision"
                    datos_individual(4).Valor = guardar(3)

                    datos_individual(5).Marcador = "Sociedad"
                    datos_individual(5).Valor = sociedad(0)

                    datos_individual(6).Marcador = "Valorsco"
                    datos_individual(6).Valor = sociedad(1)

                    datos_individual(7).Marcador = "embargos"
                    datos_individual(7).Valor = embargar(0)

                    datos_individual(8).Marcador = "valoresems"
                    datos_individual(8).Valor = embargar(1)

                    datos_individual(9).Marcador = "Socios"
                    datos_individual(9).Valor = sociedad(2)

                    actualizar(guardar(0), guardar(1), nrotituloprincipal.Trim, 1, ref3)
                    guardar_doc(guardar(0), guardar(2), txtNroDeposito.Text.Trim, Request("IdEmbargo").Trim, 1)

                    worddocresult = worddoc.CreateReportMultiTable(DTR, Reportes.AplicaciondeTituloJudicialSin, datos_individual, tbl1, 0, False, tbl1, 2, False, Nothing, 0, False)

                    If worddocresult = "" Then
                        ''mensaje no informe
                    Else

                        Dim nombre As String = Replace("APLICACION DE TITULO DE DEPOSITO JUDICIAL", " ", ".")
                        SendReport(nombre & Request("pExpediente").Trim & "." & Today.ToString("dd.MM.yyyy"), worddocresult)
                    End If

                End If

            Case 3 'Devolucion de Titulo Deposito Judicial'
                Dim tdj(1) As String

                If Request("titulo_principal") = "" Or Request("titulo_principal") = Nothing Then
                    tdj = BuscarTituloPrincipal(txtNroTituloJ.Text.Trim)
                Else
                    tdj = BuscarTituloPrincipal(Request("titulo_principal"))
                End If

                If tdj(0) = Nothing And tdj(0) = "" Then
                    tdj(0) = txtNroTituloJ.Text.Trim
                    tdj(1) = txtValorTDJ.Text.Trim

                End If



                Dim nrotituloprincipal As String = tdj(0)
                Dim valortdj As Double = CDbl(tdj(1))
                Dim datos_individual(19) As WordReport.Marcadores_Adicionales

                Dim inicial() As String = getResolucion_anterior(Request("pExpediente").Trim, "232")
                Dim Desembargo() As String
                Dim Embargo() As String
                Dim Anterior() As String

                Dim vc_datos() As String
                vc_datos = overloadresolucion(Request("pExpediente").Trim, homologo("316"))
                If vc_datos(0) <> Nothing Then
                    inicial(0) = vc_datos(0).ToUpper
                    inicial(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                Else

                End If

                If inicial(0).ToString = "" Then
                    Alert("NO SE DETECTO RESOLUCIÓN DE TERMINACIÓN DE PROCESO PARA ESTE EXPEDIENTE...")
                    Exit Sub
                Else

                    Desembargo = getResolucion_anterior(Request("pExpediente").Trim, inicial(2))
                    vc_datos = overloadresolucion(Request("pExpediente").Trim, "229")

                    If vc_datos(0) <> Nothing And vc_datos(0).Trim.ToUpper <> "SIN DATOS" Then
                        Desembargo(0) = vc_datos(0).ToUpper
                        Desembargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                    Else

                    End If
                    If Desembargo(0).ToString = "" Then
                        Alert("NO SE DETECTO RESOLUCIÓN DE DESEMBARGO PARA ESTE EXPEDIENTE...")
                        Exit Sub
                    Else

                        Embargo = getResolucion_anterior(Request("pExpediente").Trim, Desembargo(2))
                        vc_datos = overloadresolucion(Request("pExpediente").Trim, homologo("319"))
                        If vc_datos(0) <> Nothing Then
                            Embargo(0) = vc_datos(0).ToUpper
                            Embargo(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                        Else

                        End If

                        If Embargo(0).ToString = "" Then
                            Alert("NO SE DETECTO RESOLUCIÓN DE EMBARGO PARA ESTE EXPEDIENTE...")
                            Exit Sub
                        Else
                            Anterior = getResolucion_anterior(Request("pExpediente").Trim, Embargo(2))
                            vc_datos = overloadresolucion(Request("pExpediente").Trim, "013")
                            If vc_datos(0) <> Nothing Then
                                Anterior(0) = vc_datos(0).ToUpper
                                Anterior(1) = CDate(vc_datos(1)).ToString("'del' dd 'de' MMMM 'de' yyy").ToUpper
                            Else

                            End If

                            If Anterior(0).ToString = "" Then
                                Alert("NO SE DETECTO RESOLUCIÓN DE MANDAMIENTO DE PAGO PARA ESTE EXPEDIENTE...")
                                Exit Sub
                            Else

                            End If

                        End If
                    End If
                End If
                ''              Dim inicial() As String = getResolucion_anterior(Request("pExpediente").Trim, "232")
                ''              Dim Desembargo() As String = getResolucion_anterior(Request("pExpediente").Trim, inicial(2))
                ''              Dim Embargo() As String = getResolucion_anterior(Request("pExpediente").Trim, Desembargo(2))
                ''              Dim Anterior() As String = getResolucion_anterior(Request("pExpediente").Trim, Embargo(2))

                Dim Valores() As String = GetActo(Request("pExpediente").Trim, "013")

                Dim representante() As String = cargar_representante(Request("pExpediente").Trim)

                Dim guardar() As String = saveResolucion(Request("pExpediente").Trim, "232")

                Dim tbl1 As DataTable = Getvalores2(Request("pExpediente").Trim)

                Dim tbl2 As DataTable = Getvalores2(Request("pExpediente").Trim)

                If inicial(0) <> Nothing Then
                    datos_individual(0).Marcador = "terminacion"
                    datos_individual(0).Valor = inicial(0)
                    datos_individual(1).Marcador = "fecha_resolucion"
                    datos_individual(1).Valor = inicial(1)
                Else
                    Alert("NO SE ENCUENTRA UNA TERMINACION DE PROCESO ")
                    Exit Sub
                End If


                If Desembargo(0) <> Nothing Then
                    datos_individual(2).Marcador = "nro_desembargo"
                    datos_individual(2).Valor = Desembargo(0)
                    datos_individual(3).Marcador = "fecha_desembargo"
                    datos_individual(3).Valor = Desembargo(1)
                Else
                    Alert("NO SE ENCUENTRA UNA RESOLUCION DE DESEMBARGO ")
                    Exit Sub
                End If
                If Embargo(0) <> Nothing Then

                    datos_individual(4).Marcador = "nro_embargo"
                    datos_individual(4).Valor = Embargo(0)

                    datos_individual(5).Marcador = "fecha_embargo"
                    datos_individual(5).Valor = Embargo(1)
                Else
                    Alert("NO SE ENCUENTRA UNA RESOLUCION DE EMBARGO")
                    Exit Sub
                End If

                If Anterior(0) <> Nothing Then
                    datos_individual(6).Marcador = "resolución_antes"
                    datos_individual(6).Valor = Anterior(0)

                    datos_individual(7).Marcador = "fecha_anterior"
                    datos_individual(7).Valor = Anterior(1)
                Else
                    Alert("NO SE ENCUENTRA UNA RESOLUCON DE MANDAMIENTO DE PAGO ")
                    Exit Sub
                End If
                datos_individual(8).Marcador = "replegal"

                If representante(0) <> Nothing Then
                    datos_individual(8).Marcador = "replegal"
                    datos_individual(8).Valor = representante(0)
                    datos_individual(9).Marcador = "rep_nit"
                    datos_individual(9).Valor = representante(1)
                Else
                    datos_individual(8).Marcador = "replegal"
                    datos_individual(8).Valor = DTR.Rows.Item(0)("ED_Nombre")
                    datos_individual(9).Marcador = "rep_nit"
                    datos_individual(9).Valor = DTR.Rows.Item(0)("ED_CODIGO_NIT")
                End If
                datos_individual(10).Marcador = "Nsalario"
                datos_individual(10).Valor = Round((DTR.Rows.Item(0)("totaldeuda") / 616000), 0)
                datos_individual(11).Marcador = "numerosalario"
                datos_individual(11).Valor = Num2Text(datos_individual(10).Valor)
                datos_individual(12).Marcador = "tipo_noti"
                datos_individual(12).Valor = Valores(1)
                datos_individual(13).Marcador = "acta_nro"
                datos_individual(13).Valor = Valores(2)
                datos_individual(14).Marcador = "fecha_acta"
                datos_individual(14).Valor = (CDate(Valores(3)).ToString("'del' dd 'de' MMMM 'de' yyy"))

                datos_individual(15).Marcador = "Nro_Resolucion"
                datos_individual(15).Valor = guardar(0)

                datos_individual(16).Marcador = "fecha_actual"
                datos_individual(16).Valor = guardar(1)
                datos_individual(17).Marcador = "Valor_Titulo"
                datos_individual(17).Valor = String.Format("{0:C0}", CDbl(valortdj))
                datos_individual(18).Marcador = "Letras"
                datos_individual(18).Valor = Num2Text(valortdj)
                datos_individual(19).Marcador = "fecha_emision"
                datos_individual(19).Valor = guardar(3)
                guardar_doc(guardar(0), guardar(2), txtNroDeposito.Text.Trim, Request("IdEmbargo").Trim, 2)
                'actualizar(guardar(0), guardar(1), nrotituloprincipal.Trim, 2, ref3)

                worddocresult = worddoc.CreateReportMultiTable(DTR, Reportes.DevolucionTitulo2, datos_individual, tbl1, 0, False, tbl2, 1, False, Nothing, 0, False)
                If worddocresult = "" Then
                    ''mensaje no informe
                Else
                    Dim nombre As String = Replace(Replace("Devolucion de Titulo de Deposito Judicial", " ", "."), "-", ".")
                    SendReport(nombre & "-" & Request("pExpediente").Trim & "  " & Today.ToString("dd.MM.yyyy"), worddocresult)
                End If


        End Select


    End Sub

    Public Function cargar_representante(ByVal expediente As Integer) As String()
        Dim Ado As New SqlConnection(Funciones.CadenaConexion)
        Dim rep(2) As String
        Dim tblrep As New DataTable
        Dim sqldata As New SqlDataAdapter(" SELECT A.ED_NOMBRE ,A.ED_CODIGO_NIT" & _
                                          " FROM  ENTES_DEUDORES A, " & _
                                          " DEUDORES_EXPEDIENTES B, " & _
                                          " EJEFISGLOBAL C " & _
                                          " WHERE  C.EFINROEXP =B.NROEXP  " & _
                                          " AND A.ED_CODIGO_NIT = B.DEUDOR " & _
                                          " AND TIPO =3 and NroExp = '" & expediente & "'", Ado)
        sqldata.Fill(tblrep)



        If tblrep.Rows.Count > 0 Then
            For i = 0 To tblrep.Rows.Count - 1
                rep(0) = tblrep.Rows.Item(0)("ED_NOMBRE")
                rep(1) = tblrep.Rows.Item(0)("ED_CODIGO_NIT")
            Next

        End If
        Return rep
    End Function

    Private Sub Alert(ByVal Menssage As String)
        ViewState("message") = Menssage
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub

    Private Function Getvalores(ByVal TITULO_PRINCIPAL As String) As DataTable
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqltable As New DataTable
        Dim sqladapter As SqlDataAdapter

        sqladapter = New SqlDataAdapter(" select nrotituloj as 'NRO_TITULO' ,' ' AS 'FECHA COSTITUCION',( SELECT A.ED_Nombre FROM ENTES_DEUDORES A,EJEFISGLOBAL B " & _
                                         " WHERE B.EFINIT = A.ED_Codigo_Nit " & _
                                         " AND B.EFINROEXP ='" & Request("pExpediente").Trim & "') AS DEPOSITANTE ,D.BAN_NOMBRE AS CONSIGNANTE ,'$ '+CAST(CONVERT(varchar, CAST(valortdj AS money), 1) AS varchar)  as 'valor titulo' " & _
                                         " from TDJ A,TIPOS_RESOLTDJ  B, MAESTRO_BANCOS D  " & _
                                         " where NroTituloPrincipal ='" & TITULO_PRINCIPAL & "'  " & _
                                         " and  A.TipoResolGes=B.codigo " & _
                                         " AND ISNULL(A.Banco,'00')=BAN_CODIGO ", sqlconfig)

        sqladapter.Fill(sqltable)
        If sqltable.Rows.Count > 0 Then
            Return sqltable
        Else
            Return Nothing
        End If

    End Function
    Private Function Getvalores2(ByVal TITULO_PRINCIPAL As String) As DataTable
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqltable As New DataTable
        Dim sqladapter As SqlDataAdapter

        sqladapter = New SqlDataAdapter(" select nrotituloj as 'NRO_TITULO' ,NroDeposito ,' ' AS 'FECHA COSTITUCION',( SELECT A.ED_Nombre FROM ENTES_DEUDORES A,EJEFISGLOBAL B " & _
                                         " WHERE B.EFINIT = A.ED_Codigo_Nit " & _
                                         " AND B.EFINROEXP ='" & Request("pExpediente").Trim & "') AS DEPOSITANTE  ,'$ '+substring (replace(CAST(CONVERT(varchar, CAST(valortdj  AS money), 1) AS varchar),',','.'),1,  LEN(CAST(valortdj  AS money))-1) as 'valor titulo' " & _
                                         " from TDJ A,TIPOS_RESOLTDJ  B, MAESTRO_BANCOS D  " & _
                                         " where nroexp='" & TITULO_PRINCIPAL & "'" & _
                                         " and  A.TipoResolGes=B.codigo " & _
                                         " AND ISNULL(A.Banco,'00')=BAN_CODIGO ", sqlconfig)

        sqladapter.Fill(sqltable)
        If sqltable.Rows.Count > 0 Then
            Return sqltable
        Else
            Return Nothing
        End If

    End Function


    Private Function getdatosbasicos() As DataTable
        Dim sqldatatable As New DataTable
        Dim sqladapter As SqlDataAdapter
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlmanager As New SqlCommand( _
                                                               " SELECT TOP 1 A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                                                               " G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                                                               " C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                                                               " E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,ISNULL(E.MT_fec_notificacion_titulo,'') AS MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                                                               " CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                                                               " CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                                                               " k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                                                               " L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                                                               " D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                                                               " c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,A.EFIFECHAEXP                                                                                                                                                                                                                                 " & _
                                                               " FROM  EJEFISGLOBAL A,                                                                                                                                                                                                                     " & _
                                                               " 	  ENTES_DEUDORES B,                                                                                                                                                                                                                     " & _
                                                               " 	  DIRECCIONES C ,                                                                                                                                                                                                                       " & _
                                                               " 	  DEUDORES_EXPEDIENTES D,                                                                                                                                                                                                               " & _
                                                               " 	  MAESTRO_TITULOS E,                                                                                                                                                                                                                    " & _
                                                               " 	  TIPOS_TITULO F,                                                                                                                                                                                                                       " & _
                                                               " 	  TIPOS_IDENTIFICACION G,                                                                                                                                                                                                               " & _
                                                               " 	  DEPARTAMENTOS H,                                                                                                                                                                                                                      " & _
                                                               " 	  MUNICIPIOS I,                                                                                                                                                                                                                         " & _
                                                               " 	  FORMAS_NOTIFICACION J,                                                                                                                                                                                                                " & _
                                                               " 	  USUARIOS K,                                                                                                                                                                                                                           " & _
                                                               " 	  USUARIOS L                                                                                                                                                                                                                            " & _
                                                               " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                               " & _
                                                               " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                           " & _
                                                               " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                           " & _
                                                               " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                        " & _
                                                               " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                          " & _
                                                               " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                              " & _
                                                               " AND   H.codigo = C.Departamento                                                                                                                                                                                                            " & _
                                                               " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                  " & _
                                                               " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                              " & _
                                                               " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                              " & _
                                                               " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                               " & _
                                                               " AND  A.EFINROEXP = @EXPEDIENTE                                                                                                                                                                                                             " & _
                                                               " ORDER BY TIPO", sqlconfig)
        sqlmanager.Parameters.AddWithValue("@EXPEDIENTE", Request("pExpediente").Trim)
        sqladapter = New SqlDataAdapter(sqlmanager)
        sqladapter.Fill(sqldatatable)

        If sqldatatable.Rows.Count > 0 Then
            Return sqldatatable
        Else
            Return Nothing
        End If
    End Function

    Private Sub SendReport(ByVal NombreArchivo As String, ByVal Plantilla As String)
        ''        Dim nav As String = Request.Browser.Browser
        Response.ContentType = "application/msword"
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.doc", NombreArchivo))
        Response.Write(Plantilla)
        Response.End()
    End Sub

    Protected Sub cmdAdministrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAdministrar.Click
        Response.Redirect("Fraccionamiento.aspx?Expediente=" & Request("pExpediente") & "&Valor_Principal=" & txtValorTDJ.Text & "&Titulo_Principal=" & txtNroTituloJ.Text & "&Deposito=" & txtNroDeposito.Text & "&ID=" & Request("IdEmbargo"))
    End Sub

    Public Sub actualizar(ByVal resolucion As String, ByVal fecharesolucion As String, ByVal titulo_principal As String, ByVal tipores As Integer, ByVal nro_deposito As String)
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqlcommand As New SqlCommand(" update TDJ set NroResolGes =@resolucion ,FecResolGes =@fecha_resolucion ,TipoResolGes =@tipores" & _
                                         " where NroDeposito = @nro_deposito              " & _
                                         " and IdEmbargo = @idembargo                     " & _
                                         " and NroTituloPrincipal =@nro_titulo_principal  ")
        sqlcommand.Connection = sqlconfig
        sqlcommand.Parameters.AddWithValue("@resolucion", resolucion)
        sqlcommand.Parameters.AddWithValue("@fecha_resolucion", Today.Date) ''CDate(fecharesolucion))
        sqlcommand.Parameters.AddWithValue("@nro_deposito", nro_deposito)
        sqlcommand.Parameters.AddWithValue("@nro_titulo_principal", titulo_principal)
        sqlcommand.Parameters.AddWithValue("@tipores", tipores)
        sqlcommand.Parameters.AddWithValue("@idembargo", Request("IdEmbargo").Trim)

        If sqlcommand.Connection.State = ConnectionState.Open Then
            sqlcommand.Connection.Close()
        End If
        sqlcommand.Connection.Open()
        sqlcommand.ExecuteNonQuery()
    End Sub

    Private Function generarfraccionamiento(ByVal nro_deposito As String, ByVal nrotituloJ As String, ByVal nrotituloprincipal As String, ByVal nro_resolucion As String, ByVal fecha_resolucion As Date) As Double()
        If txtNroDeposito.Text.Trim = "" Or txtNroTituloJ.Text.Trim = "" Then
            CustomValidator1.Text = "No. de Depósito y No. de Título Judicial son campos obligatorios "
            CustomValidator1.IsValid = False

        End If

        Dim tdj(1) As String
        If Request("titulo_principal") <> "" Then
            tdj = BuscarTituloPrincipal(Request("titulo_principal"))
        Else
            tdj(0) = 0
            tdj(1) = 0
        End If

        'Dim nrotituloprincipal As String = tdj(0)
        Dim valortdj As Double = CDbl(txtValorTDJ.Text.Trim)
        Dim primero, segundo As Double
        Dim fraccionamiento(1) As Double
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldata As New SqlCommand
        Dim sqldatatble As New DataTable
        Dim sqladapter As SqlDataAdapter
        Dim resolucion_liquidacion() As String = getResolucion_anterior(Request("pExpediente").Trim, "233")
        Dim liquidacion As Double = GetLiquidacion(Request("pExpediente").Trim, resolucion_liquidacion(0))

        primero = liquidacion
        segundo = valortdj - liquidacion

        sqladapter = New SqlDataAdapter("select nrotituloj as 'nro titulo' ,valortdj as 'valor titulo',b.nombre as estado,('RCC-' +nroresolges )as resolucion ,fecresolges as 'fecha de resolucion' " & _
                                        " from TDJ A,TIPOS_RESOLTDJ  B " & _
                                        "where NroTituloPrincipal ='" & nrotituloprincipal.Trim & "'" & _
                                        "and  A.TipoResolGes=B.codigo ", sqlconfig)

        sqladapter.Fill(sqldatatble)
        If sqldatatble.Rows.Count > 0 Then
            'mensaje de que existe fraccionamiento'
        Else
            sqldata = New SqlCommand(" update TDJ set TipoResolGes ='3' ,NroResolGes = @nro_resolucion ,FecResolGes =@fec_resolucion " & _
                                     " where IdEmbargo =@id_embargo and NroDeposito = @nrodeposito ", sqlconfig)

            sqldata.Parameters.AddWithValue("@nro_resolucion", nro_resolucion)
            sqldata.Parameters.AddWithValue("@fec_resolucion", fecha_resolucion)
            sqldata.Parameters.AddWithValue("@id_embargo", Request("IdEmbargo").Trim)
            sqldata.Parameters.AddWithValue("@nrodeposito", txtNroDeposito.Text)

            If sqlconfig.State = ConnectionState.Open Then
                sqlconfig.Close()

            End If
            sqlconfig.Open()
            sqldata.ExecuteNonQuery()

            'sqldata = New SqlCommand("insert into TDJ (NroDeposito,NroTituloJ,NroTituloPrincipal,NroResolGes,FecResolGes,valorTDJ,TipoResolGes,IdEmbargo)values (@nro_deposito,@nrotituloJ,@nrotituloprincipal,@nroresolges,@fecresolges,@valorTDJ,@TipoResolGes,@IdEmbargo)", sqlconfig)
            'sqldata.Parameters.AddWithValue("@nro_deposito", nro_deposito & "-1")
            'sqldata.Parameters.AddWithValue("@nrotituloJ", nrotituloJ)
            'sqldata.Parameters.AddWithValue("@nrotituloprincipal", nrotituloprincipal)
            'sqldata.Parameters.AddWithValue("@nroresolges", "Sin Movimiento")
            'sqldata.Parameters.AddWithValue("@fecresolges", Now.Date)
            'sqldata.Parameters.AddWithValue("@valorTDJ", primero)
            'sqldata.Parameters.AddWithValue("@TipoResolGes", 4)
            'sqldata.Parameters.AddWithValue("@IdEmbargo", Idembargo)
            'If sqlconfig.State = ConnectionState.Open Then
            '    sqlconfig.Close()

            'End If
            'sqlconfig.Open()
            'sqldata.ExecuteNonQuery()

            'sqldata = New SqlCommand("insert into TDJ (NroDeposito,NroTituloJ,NroTituloPrincipal,NroResolGes,FecResolGes,valorTDJ,TipoResolGes,IdEmbargo)values (@nro_deposito,@nrotituloJ,@nrotituloprincipal,@nroresolges,@fecresolges,@valorTDJ,@TipoResolGes,@IdEmbargo)", sqlconfig)
            'sqldata.Parameters.AddWithValue("@nro_deposito", nro_deposito & "-2")
            'sqldata.Parameters.AddWithValue("@nrotituloJ", nrotituloJ)
            'sqldata.Parameters.AddWithValue("@nrotituloprincipal", nrotituloprincipal)
            'sqldata.Parameters.AddWithValue("@nroresolges", "Sin Movimiento")
            'sqldata.Parameters.AddWithValue("@fecresolges", Now.Date)
            'sqldata.Parameters.AddWithValue("@valorTDJ", segundo)
            'sqldata.Parameters.AddWithValue("@TipoResolGes", 4)
            'sqldata.Parameters.AddWithValue("@IdEmbargo", Idembargo)
            'If sqlconfig.State = ConnectionState.Open Then
            '    sqlconfig.Close()

            'End If
            'sqlconfig.Open()
            'sqldata.ExecuteNonQuery()

        End If


        fraccionamiento(0) = primero
        fraccionamiento(1) = segundo

        Return fraccionamiento

    End Function

    Public Function BuscarTituloPrincipal(ByVal tituloprincipal As String) As String()
        Dim datos_titulo(1) As String
        Dim sqlconfig As New SqlConnection(Funciones.CadenaConexion)
        Dim sqldatamanger As New DataTable
        Dim sqladapter As SqlDataAdapter
        Dim sqlcommand As New SqlCommand("select NroTituloJ ,ValorTDJ  from tdj where NroTituloPrincipal =@tituloprincipal")
        sqlcommand.CommandType = CommandType.Text
        sqlcommand.Parameters.AddWithValue("@tituloprincipal", tituloprincipal)
        sqlcommand.Connection = sqlconfig
        If sqlconfig.State = ConnectionState.Open Then
            sqlconfig.Close()
        End If
        sqlconfig.Open()
        sqladapter = New SqlDataAdapter(sqlcommand)
        sqladapter.Fill(sqldatamanger)

        If sqldatamanger.Rows.Count > 0 Then
            datos_titulo(0) = sqldatamanger.Rows(0).Item("NroTituloJ")
            datos_titulo(1) = sqldatamanger.Rows(0).Item("ValorTDJ")
        Else
            datos_titulo(0) = ""
            datos_titulo(1) = ""
        End If

        sqlconfig.Close()
        Return datos_titulo

    End Function


    Protected Sub cmdAplicar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAplicar.Click
        If txtNroDeposito.Text.Trim = "" Or txtNroTituloJ.Text.Trim = "" Then
            CustomValidator1.Text = "No. de Depósito y No. de Título Judicial son datos obligatorios "
            CustomValidator1.IsValid = False
            Exit Sub
        Else
            exportacion(2, txtNroTituloJ.Text.Trim, txtValorTDJ.Text.Trim)
        End If


    End Sub

    Protected Sub cmdDevolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDevolver.Click
        If txtNroDeposito.Text.Trim = "" Or txtNroTituloJ.Text.Trim = "" Then
            CustomValidator1.Text = "No. de Depósito y No. de Título Judicial son datos obligatorios "
            CustomValidator1.IsValid = False
            Exit Sub
        Else
            exportacion(3, txtNroTituloJ.Text.Trim, txtValorTDJ.Text.Trim)
        End If


    End Sub
    Protected Sub cmdFraccionar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFraccionar.Click
        If txtNroDeposito.Text.Trim = "" Or txtNroTituloJ.Text.Trim = "" Then
            CustomValidator1.Text = "No. de Depósito y No. de Título Judicial son Datos obligatorios "
            CustomValidator1.IsValid = False
            Exit Sub
        Else
            exportacion(1)
        End If

    End Sub

    Private Sub guardar_doc(ByVal resolucion As String, ByVal fecha_resolucion As String, ByVal nrodeposito As String, ByVal embargo As String, ByVal tipo_resolucion As String)
        Dim sqldata As SqlCommand
        Dim sqlconexion As New SqlConnection(Funciones.CadenaConexion)
        sqldata = New SqlCommand("update TDJ set TipoResolGes =@tipo_resolucion ,NroResolGes = @nro_resolucion ,FecResolGes =@fec_resolucion " & _
                                     " where IdEmbargo =@id_embargo and NroDeposito = @nrodeposito ", sqlconexion)
        sqldata.CommandType = CommandType.Text
        sqldata.Parameters.AddWithValue("@tipo_resolucion", tipo_resolucion)
        sqldata.Parameters.AddWithValue("@nro_resolucion", resolucion)
        sqldata.Parameters.AddWithValue("@fec_resolucion", fecha_resolucion)
        sqldata.Parameters.AddWithValue("@id_embargo", embargo)
        sqldata.Parameters.AddWithValue("@nrodeposito", nrodeposito)
        If sqlconexion.State = ConnectionState.Closed Then
            sqlconexion.Open()
        End If
        sqldata.ExecuteNonQuery()


    End Sub
End Class