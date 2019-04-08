Imports System.Data.SqlClient
Namespace procesos_tributario.cobronuevo
    Public Class cobronuevo
        Inherits procesos_tributario.ProcesosConfig

        Private varActoNuevoPro As String
        Private varImpuesto As String
        Private varTotaValor As Double
        Private varEstrato As String
        Private varMaxVigen As String
        Private varSQLIndividual As String

        Public Property TotaValor() As Double
            Get
                Return vartotavalor
            End Get
            Set(ByVal value As Double)
                vartotavalor = value
            End Set
        End Property

        Public Property Impuesto_ente() As String
            Get
                Return varImpuesto
            End Get
            Set(ByVal value As String)
                varImpuesto = value
            End Set
        End Property

        Public Property Estrato() As String
            Get
                Return varEstrato
            End Get
            Set(ByVal value As String)
                varEstrato = value
            End Set
        End Property

        Public Property MaxVigen() As String
            Get
                Return varMaxVigen
            End Get
            Set(ByVal value As String)
                varMaxVigen = value
            End Set
        End Property

        Public Property ActoNuevoPro() As String
            Get
                Return varActoNuevoPro
            End Get
            Set(ByVal value As String)
                varActoNuevoPro = value
            End Set
        End Property

        Public Property SQLIndividual() As String
            Get
                Return varSQLIndividual
            End Get
            Set(ByVal value As String)
                varSQLIndividual = value
            End Set
        End Property

        Public Sub New(ByVal Conexion As System.Data.SqlClient.SqlConnection)
            MyBase.New(Conexion)
        End Sub

        Public Sub New()

        End Sub
  
        Public Sub dedures_no_procesos(ByVal mytable As Schema.Masivos_Pendientes_formDataTable, ByVal xtConexion As String, ByVal SQL As String)
            Using connection As New SqlConnection(xtConexion)
                Dim sqlConsulta As String

                If SQL = Nothing Then
                    sqlConsulta = consultas(1, Impuesto)
                Else
                    sqlConsulta = SQL

                    If TotaValor = Nothing Then
                        TotaValor = 1
                    End If

                    If MaxVigen = Nothing Then
                        MaxVigen = 1700
                    End If

                    If Estrato = Nothing Then
                        Estrato = 1
                    End If
                End If

                If Impuesto_ente Is Nothing Then
                    Throw New Exception("No se detecto el impuesto (Acto Administrativo)")
                End If

                Dim command As SqlCommand = New SqlCommand(sqlConsulta, connection)
                connection.Open()

                command.Parameters.Add("@MaxVigen", SqlDbType.VarChar).Value = MaxVigen
                command.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = TotaValor
                command.Parameters.Add("@ESTRATO", SqlDbType.Int).Value = Estrato
                command.Parameters.Add("@IMPMODCOD", SqlDbType.Int).Value = Impuesto_ente
                command.CommandTimeout = 60000


                Dim reader As SqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
                mytable.Load(reader)
                reader.Close()
            End Using
        End Sub

        Public Overrides Function Prima_EjecucionFiscal(ByVal Impuesto As String, ByVal TipoRep As Integer) As Boolean

        End Function

        Private Function consultas(ByVal con As Integer, ByVal varImpuesto As String) As String
            Dim stringSQL As String = Nothing
            Dim concat As String = Nothing

            If TotaValor = Nothing Then
                TotaValor = 1
            End If

            If MaxVigen = Nothing Then
                MaxVigen = 1700
            End If

            If Estrato = Nothing Then
                Estrato = 1
            End If

            Dim vigen As String = " AND A.ANNIOMIN BETWEEN @MaxVigen AND " & Date.Now.Year & " AND A.PRECARVAL >= @ESTRATO AND A.TOTALPAGAR >= @VALOR AND IMPMODCOD = @IMPMODCOD "

            Select Case con
                Case 1
                    stringSQL = "SELECT A.PRENUM AS LIQGEN,A.PRECARVAL,A.PREPRSDOC,A.PREPRSNOM,(CONVERT(VARCHAR,A.ANNIOMIN) + ' - ' + CONVERT(VARCHAR, A.ANNIOMAX)) AS VIGENCIAS,A.ANNIOMIN,A.ANNIOMAX,A.TOTALPAGAR FROM PROCESOS_DETALPREDIO A WHERE A.PRENUM NOT IN (SELECT EFIGEN FROM EJEFIS) " & vigen & " ORDER BY TOTALPAGAR DESC"
                Case 2
                    stringSQL = "SELECT  @IMP AS MAN_IMPUESTO,A.PRENUM AS LIQGEN,A.PRECARVAL,A.PREPRSDOC,A.PREPRSNOM,(CONVERT(VARCHAR,A.ANNIOMIN) + ' - ' + CONVERT(VARCHAR, A.ANNIOMAX)) AS VIGENCIAS,ANNIOMIN, A.ANNIOMAX,A.TOTALPAGAR,A.LIQINT,A.LIQTOT,A.PREDIR FROM PROCESOS_DETALPREDIO A WHERE A.PRENUM NOT IN (SELECT EFIGEN FROM EJEFIS) " & vigen & " ORDER BY TOTALPAGAR DESC"
            End Select

            Return stringSQL
        End Function

        '<Cobro de Nuevos Procesos> <predio o placa sin expedientes > <Arranque>
        Public Overrides Function Prima_EjecucionFiscal_Masivos(ByVal Valor As Boolean, ByVal ValValor As Double, ByVal Impuesto As String, ByVal TipoRep As Integer) As Boolean
            'Prueba
            Me.Load_Configuracion()

            Dim tabla_Deudores_sin_procesos As Schema.Masivos_PendientesDataTable
            tabla_Deudores_sin_procesos = New Schema.Masivos_PendientesDataTable
            'Predios sin expedientes a procesar
            recolecta_datos_nuevos_cobros(tabla_Deudores_sin_procesos)

            'Asegurar Consecutivo ---- 
            Dim conse As ArrayList
            conse = consecutivo_procesar(tabla_Deudores_sin_procesos)
            Dim addconse As Integer = conse(0)



            Dim row As DataRow
            For Each row In tabla_Deudores_sin_procesos.Rows
                addconse += 1
                Dim NewRows As DataRow = addnewRows_nuevo_proceso(row, Me.Datos_Informe, tabla_Deudores_sin_procesos.Rows.Count, addconse)
                Me.Datos_Informe.Rows.Add(NewRows)
            Next
        End Function

        Private Sub recolecta_datos_nuevos_cobros(ByVal myTable As Schema.Masivos_PendientesDataTable)
            Dim sqlConsulta As String
            If varSQLIndividual = Nothing Then
                'Procesos masivos
                sqlConsulta = consultas(2, Impuesto)
            Else
                'Procesos individuales
                sqlConsulta = varSQLIndividual
            End If

            Dim myadpater As New SqlDataAdapter(sqlConsulta, Conexion)

            If (Impuesto_ente Is Nothing) Or (Impuesto_Literal Is Nothing) Then
                Throw New Exception("No se detecto el impuesto (Acto Administrativo)")
            End If

            myadpater.SelectCommand.Parameters.Add("@IMP", SqlDbType.VarChar, 255).Value = Impuesto_Literal
            myadpater.SelectCommand.Parameters.Add("@MaxVigen", SqlDbType.VarChar).Value = MaxVigen
            myadpater.SelectCommand.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = TotaValor
            myadpater.SelectCommand.Parameters.Add("@ESTRATO", SqlDbType.Int).Value = Estrato
            myadpater.SelectCommand.Parameters.Add("@IMPMODCOD", SqlDbType.Int).Value = Impuesto_ente
            myadpater.SelectCommand.CommandTimeout = 60000

            myadpater.Fill(myTable)
        End Sub

        Private Function consecutivo_procesar(ByVal tabla_Deudores_sin_procesos As Schema.Masivos_PendientesDataTable) As ArrayList
            Dim conse As Integer
            Dim Primer_antes_conse As Integer
            Dim retorno As New ArrayList

            Using connection As New SqlConnection(MyConexion.ConnectionString)
                connection.Open()

                Dim command As SqlCommand = connection.CreateCommand()
                Dim transaction As SqlTransaction
                transaction = connection.BeginTransaction("SampleTransaction")
                command.Connection = connection
                command.Transaction = transaction

                Try
                    Dim nroNumFectar As Integer = tabla_Deudores_sin_procesos.Rows.Count
                    nroNumFectar = tabla_Deudores_sin_procesos.Rows.Count
                    command.CommandText = "UPDATE NUMERADO SET  @proximo_numero = NumUltNro = NumUltNro + @numafectar, @numerador = NumUltNro WHERE numid = 'ex1'"
                    command.Parameters.Add("@proximo_numero", SqlDbType.Int)
                    command.Parameters.Add("@numerador", SqlDbType.Int)
                    command.Parameters.Add("@numafectar", SqlDbType.Int).Value = nroNumFectar
                    command.Parameters("@proximo_numero").Direction = ParameterDirection.Output
                    command.Parameters("@numerador").Direction = ParameterDirection.Output

                    command.ExecuteNonQuery()
                    conse = CType(command.Parameters("@proximo_numero").Value, Integer)
                    Primer_antes_conse = CType(command.Parameters("@numerador").Value, Integer)

                    retorno.Add(Primer_antes_conse)
                    retorno.Add(conse)

                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                End Try
            End Using

            Return retorno
        End Function

        Private Function addnewRows_nuevo_proceso(ByVal xrow As DataRow, ByVal xDatos As DataTable, ByVal numero_datos_afectados As Integer, ByVal expediente As Integer) As DataRow
            Dim row As Reportes_Admistratiivos.Mandaniento_PagoRow
            row = xDatos.NewRow

            row("MAN_DEUSDOR") = xrow("PREPRSDOC")
            row("MAN_IMPUESTO") = Funciones.valorNull(Impuesto_Literal, Impuesto)
            row("MAN_VALORMANDA") = xrow("TOTALPAGAR")
            row("MAN_NOMDEUDOR") = xrow("PREPRSNOM")
            row("MAN_DIRECCION") = xrow("PREDIR")
            row("MAN_DIR_ESTABL") = xrow("PREDIR")
            row("MAN_REFCATRASTAL") = xrow("LIQGEN")
            row("MAN_VIGENCIA") = Nothing
            row("MAN_CONCEPTOCDG") = Nothing
            row("MAN_ESTRATOCD") = Nothing
            row("MAN_DESTINOCD2") = Nothing
            row("MAN_BASEGRAVABLE") = Nothing
            row("MAN_TARIFA") = DBNull.Value
            row("MAN_CAPITAL") = DBNull.Value
            row("MAN_INTERESES") = xrow("LIQINT")
            row("MAN_TOTAL") = xrow("LIQTOT")
            row("MAN_EXPEDIENTE") = expediente
            row("MAN_FECHADOC") = DBNull.Value
            row("MAN_EFIPERDES") = xrow("ANNIOMIN")
            row("MAN_EFIPERHAS") = xrow("ANNIOMAX")
            row("MAN_PAGOS") = DBNull.Value
            row("MAN_FECHARAC") = Date.Now
            row("MAN_MATINMOB") = DBNull.Value

            Return row
        End Function
    End Class
End Namespace

