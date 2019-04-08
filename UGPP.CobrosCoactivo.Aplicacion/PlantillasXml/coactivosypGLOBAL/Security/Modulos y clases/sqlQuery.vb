Imports System.Data.SqlClient
Namespace procesos_tributario.sqlQuery
    Public Class sqlQuery
        Inherits procesos_tributario.ProcesosConfig

        'Organizado por tablas 
#Region "Tabla [EJEFIS]"
        Protected myCommand As Data.SqlClient.SqlCommand


        Public Sub New(ByVal Conexion As System.Data.SqlClient.SqlConnection)
            MyBase.New(Conexion)
        End Sub

        Public Sub New()

        End Sub

        Public Overrides Function Prima_EjecucionFiscal(ByVal xImpuesto_Literal As String, ByVal TipoRep As Integer) As Boolean
            Dim selectCommand As SqlCommand = Load_Informe_EjecucionesFiscales(Expedinete, RefeCatastral, Repo, TipoRep)
            Dim adapter As New SqlDataAdapter(selectCommand)

            adapter.SelectCommand.Parameters.Add("@IMP", SqlDbType.VarChar, 255).Value = xImpuesto_Literal

            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey
            adapter.SelectCommand.CommandTimeout = 60000
            adapter.Fill(Me.Datos_Informe)
        End Function

        Public Function Load_Informe_EjecucionesFiscales(ByVal expediente As String, ByVal ReferCatrastal As String, ByVal Repo As String, ByVal TipoRep As Integer) As System.Data.SqlClient.SqlCommand
            Dim retornaDatos As String = ""
            'Parte manual 
            'Que clase se va consumir segun Impuesto
            'Clases
            If Impuesto = 1 Then ' Predial
                Dim controReport As New control_informe_x_impuesto.cobro_predial
                controReport.TipoReporte = TipoRep
                controReport.acto_paso_administrativo = Repo
                retornaDatos = controReport.Informe_retorno
            ElseIf Impuesto = 2 Then ' Industria y comercio
                Dim controReport As New control_informe_x_impuesto.cobro_industriaComercio
                controReport.TipoReporte = TipoRep
                controReport.acto_paso_administrativo = Repo
                retornaDatos = controReport.Informe_retorno
            ElseIf Impuesto = 3 Then ' Vehiculo 
                Dim controReport As New control_informe_x_impuesto.cobro_vehiculo
                controReport.TipoReporte = TipoRep
                controReport.acto_paso_administrativo = Repo
                retornaDatos = controReport.Informe_retorno
            ElseIf Impuesto = 4 Then ' PENSIONADOS
                Dim controReport As New control_informe_x_impuesto.cobro_pensiones
                controReport.TipoReporte = TipoRep
                controReport.acto_paso_administrativo = Repo
                retornaDatos = controReport.Informe_retorno
            Else
                Throw New Exception("No Existe un reporte habilitados para este impuesto")
            End If

            myCommand = New Data.SqlClient.SqlCommand
            myCommand.Connection = MyConexion

            Crear_Parametros("@Ente", SqlDbType.VarChar, Me.Ente)
            Crear_Parametros("@EnteDeudorPropietario", SqlDbType.VarChar, Me.EnteDeudorPropietario)
            Crear_Parametros("@Impuesto", SqlDbType.VarChar, Me.Impuesto)
            Crear_Parametros("@Refecatras", SqlDbType.VarChar, ReferCatrastal)
            Crear_Parametros("@Expediente", SqlDbType.VarChar, expediente)
            Crear_Parametros("@actoadmin", SqlDbType.VarChar, Repo)

            If Not retornaDatos Is Nothing Then
                Me.Load_Configuracion()
                myCommand.CommandText = retornaDatos
            Else
                Throw New Exception("No Existe un reporte habilitados para este proceso (Acto Administrativo)")
            End If

            If Load_Acto_Previo = True Then
                'Ejecutar fill de la tabla de actos_previos en la tabla de actos previos con la conexion me UTILIZANDO EL ACTO PREVIO
                Using connection As New SqlConnection(ConexionME.ConnectionString)
                    Using Command As New System.Data.SqlClient.SqlCommand("SELECT * FROM ENTRA_DOCUMENTOMA A WHERE DOC_EXPEDIENTE = @DOC_EXPEDIENTE AND DOC_ACTOADMINISTRATIVO = (SELECT DXI_ACTO_PREVIO FROM dbo.DOCUMENTO_INFORMEXIMPUESTO WHERE NOT DXI_ACTO_PREVIO IS NULL AND DXI_ACTO = @DOC_ACTOADMINISTRATIVO)", connection)
                        connection.Open()
                        Command.CommandTimeout = 60000
                        Command.Parameters.Add("@DOC_EXPEDIENTE", SqlDbType.VarChar).Value = Expedinete
                        Command.Parameters.Add("@DOC_ACTOADMINISTRATIVO", SqlDbType.VarChar).Value = Repo
                        Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                        Datos_Acto_Previo.Load(reader)
                        reader.Close()
                        connection.Close()
                    End Using
                End Using
            End If

            Return myCommand
        End Function

        Private Sub Crear_Parametros(ByVal ParameterName As String, ByVal ParameterType As System.Data.SqlDbType, ByVal Dato As Object)
            myCommand.Parameters.Add(ParameterName, ParameterType)
            If Dato = Nothing Then
                Dato = DBNull.Value
            End If

            myCommand.Parameters(ParameterName).Value = Dato
        End Sub

        Public Overloads Overrides Function Prima_EjecucionFiscal_Masivos(ByVal Valor As Boolean, ByVal ValValor As Double, ByVal xImpuesto_Literal As String, ByVal TipoRep As Integer) As Boolean
            Dim tempTable As New DataTable
            Dim pExpedinete, pRefeCatastral As String
            Dim myTableUnoaUno As DataTable
            tempTable = Estructura_dato_deuores_segun_secuencia()

            If tempTable.Rows.Count > 0 Then
                Dim x As Integer
                For x = 0 To tempTable.Rows.Count - 1
                    pExpedinete = tempTable.Rows(x).Item("ULT_EXPEDIENTE")
                    pRefeCatastral = Nothing 'oJO
                    Using command As SqlCommand = Load_Informe_EjecucionesFiscales(pExpedinete, pRefeCatastral, Repo, TipoRep)
                        'Dim command As SqlCommand = Load_Informe_EjecucionesFiscales(pExpedinete, pRefeCatastral, Repo, TipoRep)
                        command.Parameters.Add("@IMP", SqlDbType.VarChar, 255).Value = xImpuesto_Literal
                        command.Connection.Open()
                        Dim reader As SqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection)

                        myTableUnoaUno = New DataTable
                        myTableUnoaUno.Load(reader)

                        reader.Close()

                        Dim row As DataRow
                        For Each row In myTableUnoaUno.Rows
                            Dim NewRows As DataRow = addnewRows(row, Me.Datos_Informe)
                            Me.Datos_Informe.Rows.Add(NewRows)
                        Next
                    End Using
                Next
            End If
        End Function
        Public Sub Conceptos_de_la_Deuda(ByVal Expediente As String, ByVal Impuesto As Integer)
            Dim SQL As String
            If Impuesto = 1 Then
                SQL = "SELECT CONVERT(INT, EDC_LIQOFI) AS LIQOFI,CONVERT(CHAR(10), EDC_FECHLA_IQUIDACION, 20) AS FECHLA_IQUIDACION,EFIGEN AS PREDIO,EDC_VIGENCIA AS PERIODO,EDC_AVALUO AS AVALUO,EDC_TARIFA AS TRIFA_IPU, EDC_IPU AS IPU,EDC_TARIFAAREA AS TARIFA_AREA,EDC_SOBRETASAAREA AS SOBRETASA_AREA,EDC_TARIFACDMB AS TARIFA_CDMB, EDC_SAMBIENTAL AS SOBRETASA_CDMB, EDC_GASTOSSISTEMA AS SISTEMA,EDC_TOTLIQOFI AS TOTAL_IMPUESTO, EFINROEXP AS EXPEDIENTE FROM EJEFISGLOBALLIQUIDAD,EJEFISGLOBAL WHERE EDC_ID = EFIGEN AND EFINROEXP = @DOCEXPEDIENTE AND EDC_VIGENCIA BETWEEN EFIPERDES AND EFIPERHAS AND EDC_CODIGO_IMPUESTO = @IMPUESTO GROUP BY EDC_LIQOFI,EDC_FECHLA_IQUIDACION,EFIGEN,EDC_VIGENCIA,EDC_AVALUO,EDC_TARIFA,EDC_IPU,EDC_TARIFAAREA,EDC_SOBRETASAAREA,EDC_TARIFACDMB,EDC_SAMBIENTAL,EDC_GASTOSSISTEMA,EDC_TOTLIQOFI,EFINROEXP ORDER BY EDC_VIGENCIA ASC"
            ElseIf Impuesto = 2 Then
                SQL = " SELECT EDC_VIGENCIA AS ANNIOGRA,							" & _
                    " 	  'PRIV' AS TIPOLIQ,                                    " & _
                    " 	  EDC_FECHA_PRESENTACION AS FECHAPRESENTACION,          " & _
                    " 	  EFIGEN AS PLACA,                                      " & _
                    " 	  SUM(EDC_SANCION)AS SANCIONES,                         " & _
                    " 	  SUM(EDC_IMPUESTO) AS INDUSTRIA,                       " & _
                    " 	  SUM(EDC_AVISOS_TABLEROS) AS AVISOSYTABLEROS,          " & _
                    " 	  SUM(EDC_SOBRETASA_BOMBERIL) AS SOBRETASABOM,          " & _
                    " 	  SUM(EDC_GASTOSSISTEMA) AS GASTOSDESISTE,              " & _
                    " 	  SUM(EDC_OTROS) AS OTROS,                              " & _
                    " 	  CONVERT(CHAR(10),                                     " & _
                    " 	  EDC_FECHLA_IQUIDACION, 20) AS FECHLA_IQUIDACION,      " & _
                    " 	  EFINROEXP AS EXPEDIENTE                               " & _
                    " FROM EJEFISGLOBALLIQUIDAD,EJEFISGLOBAL                    " & _
                    " WHERE EDC_ID = EFIGEN                                     " & _
                    " AND EFINROEXP = @DOCEXPEDIENTE                            " & _
                    " AND EDC_VIGENCIA BETWEEN EFIPERDES AND EFIPERHAS          " & _
                    " AND EDC_CODIGO_IMPUESTO = @IMPUESTO                       " & _
                    " GROUP BY EDC_FECHLA_IQUIDACION,                           " & _
                    " EFIGEN,EDC_VIGENCIA,EDC_FECHA_PRESENTACION,EFINROEXP      " & _
                    " ORDER BY EDC_VIGENCIA ASC                                 "
            Else
                Throw New Exception("Impuesto sn configurar")
            End If

            Using Command As New System.Data.SqlClient.SqlCommand(SQL, New SqlConnection(Funciones.CadenaConexion))
                Command.Parameters.Add("@DOCEXPEDIENTE", SqlDbType.VarChar).Value = Expediente
                Command.Parameters.Add("@IMPUESTO", SqlDbType.VarChar).Value = Impuesto
                Command.Connection.Open()
                Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                Datos_Conceptos_Deuda.Load(reader)
                reader.Close()
                Command.Connection.Close()
            End Using
        End Sub


        Private Function Estructura_dato_deuores_segun_secuencia() As DataTable
            Using connection As New SqlConnection(MyConexionME.ConnectionString)
                Dim tempTable As DataTable
                Dim command As SqlCommand = New SqlCommand("SELECT * FROM DEPENDENCIA_ACTUACIONES WHERE DEP_DEPENDENCIA = @DEP_DEPENDENCIA", connection)
                command.Parameters.Add("@DEP_DEPENDENCIA", SqlDbType.VarChar, 3).Value = Repo
                connection.Open()

                Dim reader As SqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
                tempTable = New DataTable
                tempTable.Load(reader)

                reader.Close()
                Dim xSelect As String = "("

                If tempTable.Rows.Count > 0 Then
                    Dim x As Integer
                    For x = 0 To tempTable.Rows.Count - 1
                        xSelect += "ult_acto = @PARA" & x & " OR "
                        command.Parameters.Add("@PARA" & x, SqlDbType.VarChar, 3).Value = tempTable.Rows(x).Item("DEP_CODACTO")
                    Next

                    xSelect = Mid(xSelect, 1, xSelect.Length - 4)
                    xSelect += ")"

                    command.CommandText = "SELECT * FROM  documento_ultimoacto  WHERE " & xSelect
                    connection.Open()
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection)
                    tempTable = New DataTable
                    tempTable.Load(reader)
                    reader.Close()

                    Return tempTable
                Else
                    'Preguntar por codigo 065 y 066
                    If Repo = "065" Or Repo = "066" Or Repo = "217" Or Repo = "218" Or Repo = "219" Or Repo = "220" Or Repo = "221" Or Repo = "222" Or Repo = "223" Or Repo = "224" Or Repo = "225" Or Repo = "226" Or Repo = "227" Then
                        xSelect = ""
                        If mnivelacces = 2 Then
                            xSelect = xSelect & " AND EFIUSUASIG = @codusuario"
                            command.Parameters.Add("@codusuario", SqlDbType.VarChar, 4).Value = sscodigousuario
                        End If

                        Dim ACTO As String = ""
                        If Repo = "065" Then
                            ACTO = "014"
                        ElseIf Repo = "066" Then
                            ACTO = "065"
                        Else
                            ACTO = "001"
                        End If

                        command.CommandText = "SELECT * FROM DOCUMENTO_ULTIMOACTO,EJEFISGLOBAL WHERE ULT_EXPEDIENTE = EFINROEXP AND EFIMODCOD = @EFIMODCOD  AND ULT_ACTO = '" & ACTO & "' " & xSelect
                        command.Parameters.Add("@EFIMODCOD", SqlDbType.Int).Value = Impuesto

                        connection.Open()
                        reader = command.ExecuteReader(CommandBehavior.CloseConnection)
                        tempTable = New DataTable
                        tempTable.Load(reader)
                        reader.Close()

                        Return tempTable
                    Else
                        Throw New Exception("Este acto no tiene una ramificación o dependencia")
                    End If
                End If
            End Using
        End Function
#End Region
    End Class
End Namespace

