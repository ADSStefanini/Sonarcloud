Imports System.Data.SqlClient
Imports System.Web.HttpRequest
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo.Entidades
Imports System.IO
Imports log4net
Public Class LogProcesos
    Public ListaDiccionario As List(Of DiccionarioAditoria)
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Enum ErrorType
        ErrorLog = 1
        WarningLog = 2
        DebugLog = 3
        FatalLog = 4
    End Enum
    Public Sub New()
        ListaDiccionario = New TraductorAuditoriaBLL().obtenerDiccionario()
    End Sub

    Private mens As String
    ReadOnly Property ErrorMsg() As String
        Get
            Return mens
        End Get
    End Property

    Private Function CreateScript2(ByVal Commando As SqlClient.SqlCommand) As String
        Dim strParametros As String = "/* "
        For i As Integer = 0 To Commando.Parameters.Count - 1
            strParametros &= Commando.Parameters(i).ParameterName
            Select Case Commando.Parameters(i).DbType
                Case DbType.String, DbType.StringFixedLength, DbType.AnsiString, DbType.AnsiStringFixedLength
                    strParametros &= String.Format(" {0} = '{1}', ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value)
                Case DbType.Binary
                    strParametros &= String.Format(" {0} = '{1}', ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value)
                Case DbType.Boolean, DbType.Byte, DbType.SByte
                    strParametros &= String.Format(" {0} = {1}, ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value)
                Case DbType.Currency, DbType.Decimal, DbType.Double, DbType.Int16, DbType.Int32, DbType.Int64, DbType.Single, DbType.UInt16, DbType.UInt32, DbType.UInt64, DbType.VarNumeric
                    strParametros &= String.Format(" {0} = {1}, ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value)
                Case DbType.Date, DbType.DateTime, DbType.DateTime2, DbType.DateTimeOffset, DbType.Time
                    strParametros &= String.Format(" {0} = '{1}', ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value)
                Case DbType.Guid
                    strParametros &= String.Format(" {0} = '{1}', ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value)
                Case DbType.Object
                    strParametros &= String.Format(" {0} = '{1}', ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value.ToString)
                Case DbType.Xml
                    strParametros &= String.Format(" {0} = '{1}', ", Commando.Parameters(i).ParameterName, Commando.Parameters(i).Value)
            End Select
        Next
        strParametros &= "*/ " & Commando.CommandText
        Return strParametros
    End Function

    Private Function CreateScript(ByVal Commando As SqlClient.SqlCommand) As String
        Dim strComandtext As String = Commando.CommandText
        Dim strValue As String = "", strParaName As String = ""
        For i As Integer = 0 To Commando.Parameters.Count - 1
            Select Case Commando.Parameters(i).DbType
                Case DbType.String, DbType.StringFixedLength, DbType.AnsiString, DbType.AnsiStringFixedLength
                    strValue = String.Format("'{0}'", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
                Case DbType.Binary
                    strValue = String.Format("'{0}'", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
                Case DbType.Boolean, DbType.Byte, DbType.SByte
                    strValue = String.Format("{0}", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
                Case DbType.Currency, DbType.Decimal, DbType.Double, DbType.Int16, DbType.Int32, DbType.Int64, DbType.Single, DbType.UInt16, DbType.UInt32, DbType.UInt64, DbType.VarNumeric
                    strValue = String.Format("{0}", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
                Case DbType.Date, DbType.DateTime, DbType.DateTime2, DbType.DateTimeOffset, DbType.Time
                    strValue = String.Format("'{0}'", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
                Case DbType.Guid
                    strValue = String.Format("'{0}'", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
                Case DbType.Object
                    strValue = String.Format("'{0}'", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
                Case DbType.Xml
                    strValue = String.Format("'{0}'", Commando.Parameters(i).Value)
                    strParaName = String.Format("{0}", Commando.Parameters(i).ParameterName)
            End Select
            If strComandtext.Contains(strParaName) Then
                strComandtext = strComandtext.Replace(strParaName, strValue)
            End If
        Next
        Return strComandtext
    End Function

    Public Function ClientIpAddress() As String
        Try
            Dim currentRequest As HttpRequest = HttpContext.Current.Request
            Dim ipAddress As String = currentRequest.ServerVariables("HTTP_X_FORWARDED_FOR")
            If ipAddress Is Nothing OrElse ipAddress.ToLower() = "unknown" Then
                ipAddress = currentRequest.ServerVariables("REMOTE_ADDR")
            End If
            Return ipAddress
        Catch
            Return String.Empty
        End Try
    End Function
    Public Function ClientHostName() As String
        Try
            Dim hostN As String = System.Net.Dns.GetHostEntry(ClientIpAddress).HostName
            If hostN = Nothing Then
                Return String.Empty
            Else
                Return hostN
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    Public Function AplicationName() As String
        Try
            Dim resultado As String
            resultado = System.Configuration.ConfigurationManager.AppSettings("ApplicationName") & " " & System.Configuration.ConfigurationManager.AppSettings("ApplicationVersion")
            Return resultado
        Catch
            Return String.Empty
        End Try
    End Function

    Public Function SaveLog(ByVal UsuarioId As String, ByVal NombreModulo As String, ByVal Documento As String, ByVal Commando As SqlCommand) As Boolean
        Dim resultadoCommand As String
        Try
            Dim Connection As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            resultadoCommand = CreateScript(Commando)
            Dim cmd As New SqlCommand
            cmd.Connection = Connection
            cmd.CommandText = "INSERT INTO LOG_AUDITORIA " &
                            "([LOG_USER_ID], [LOG_USER_CC], [LOG_HOST], [LOG_IP], [LOG_FECHA], [LOG_APLICACION], [LOG_MODULO], [LOG_DOC_AFEC], [LOG_CONSULTA] ,[LOG_NEGOCIO]) VALUES " &
                            "(@LOG_USER_ID , @LOG_USER_CC , @LOG_HOST , @LOG_IP , @LOG_FECHA , @LOG_APLICACION , @LOG_MODULO , @LOG_DOC_AFEC , @LOG_CONSULTA, @LOG_NEGOCIO) "
            cmd.Parameters.Add("@LOG_USER_ID", SqlDbType.VarChar, 40).Value = UsuarioId
            cmd.Parameters.Add("@LOG_USER_CC", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@LOG_HOST", SqlDbType.VarChar, 50).Value = If((ClientHostName() Is Nothing), String.Empty, ClientHostName())
            cmd.Parameters.Add("@LOG_IP", SqlDbType.VarChar, 20).Value = If((ClientIpAddress() Is Nothing), String.Empty, ClientIpAddress())
            cmd.Parameters.Add("@LOG_FECHA", SqlDbType.DateTime).Value = Date.Now
            cmd.Parameters.Add("@LOG_APLICACION", SqlDbType.VarChar, 100).Value = If((AplicationName() Is Nothing), String.Empty, AplicationName())
            cmd.Parameters.Add("@LOG_MODULO", SqlDbType.VarChar, 100).Value = NombreModulo
            cmd.Parameters.Add("@LOG_DOC_AFEC", SqlDbType.VarChar, 50).Value = Documento
            cmd.Parameters.Add("@LOG_CONSULTA", SqlDbType.VarChar, 5000).Value = resultadoCommand
            cmd.Parameters.Add("@LOG_NEGOCIO", SqlDbType.VarChar, 150).Value = GenerarComandoNegocio(resultadoCommand)
            Connection.Open()
            cmd.ExecuteNonQuery()
            Connection.Close()
            Connection.Dispose()
            cmd.Dispose()
            Return True
        Catch ex As Exception
            mens = ex.Message
            Return False
        End Try
    End Function

    Public Function SaveLog2(ByVal UsuarioId As String, ByVal NombreModulo As String, ByVal Documento As String, ByVal Commando As SqlCommand, ByVal cnx As SqlConnection, ByVal transac As SqlTransaction) As Boolean
        Try
            'Dim Connection As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim comando As String
            comando = CreateScript(Commando)
            Dim cmd As New SqlCommand
            cmd.Connection = cnx
            cmd.CommandText = "INSERT INTO LOG_AUDITORIA " &
                            "([LOG_USER_ID], [LOG_USER_CC], [LOG_HOST], [LOG_IP], [LOG_FECHA], [LOG_APLICACION], [LOG_MODULO], [LOG_DOC_AFEC], [LOG_CONSULTA] ,[LOG_NEGOCIO]) VALUES " &
                            "(@LOG_USER_ID , @LOG_USER_CC , @LOG_HOST , @LOG_IP , @LOG_FECHA , @LOG_APLICACION , @LOG_MODULO , @LOG_DOC_AFEC , @LOG_CONSULTA, @LOG_NEGOCIO) "
            cmd.Parameters.Add("@LOG_USER_ID", SqlDbType.VarChar, 40).Value = UsuarioId
            cmd.Parameters.Add("@LOG_USER_CC", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@LOG_HOST", SqlDbType.VarChar, 50).Value = If((ClientHostName() Is Nothing), String.Empty, ClientHostName())
            cmd.Parameters.Add("@LOG_IP", SqlDbType.VarChar, 20).Value = If((ClientIpAddress() Is Nothing), String.Empty, ClientIpAddress())
            cmd.Parameters.Add("@LOG_FECHA", SqlDbType.DateTime).Value = Date.Now
            cmd.Parameters.Add("@LOG_APLICACION", SqlDbType.VarChar, 100).Value = If((AplicationName() Is Nothing), String.Empty, AplicationName())
            cmd.Parameters.Add("@LOG_MODULO", SqlDbType.VarChar, 100).Value = NombreModulo
            cmd.Parameters.Add("@LOG_DOC_AFEC", SqlDbType.VarChar, 50).Value = Documento
            cmd.Parameters.Add("@LOG_CONSULTA", SqlDbType.VarChar, 5000).Value = comando
            cmd.Parameters.Add("@LOG_NEGOCIO", SqlDbType.VarChar, 150).Value = GenerarComandoNegocio(comando)
            cmd.Transaction = transac
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            mens = ex.Message
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Sobre escritura de la función de auditoria
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function SaveLog(ByVal UsuarioId As String, ByVal NombreModulo As String, ByVal Documento As String, ByVal Commando As String) As Boolean
        Dim resultado As Boolean
        resultado = False
        Try
            Using Connection As New SqlClient.SqlConnection(Funciones.CadenaConexion)
                Using cmd As New SqlCommand
                    cmd.Connection = Connection
                    cmd.CommandText = "INSERT INTO LOG_AUDITORIA " &
                                    "([LOG_USER_ID], [LOG_USER_CC], [LOG_HOST], [LOG_IP], [LOG_FECHA], [LOG_APLICACION], [LOG_MODULO], [LOG_DOC_AFEC], [LOG_CONSULTA], [LOG_NEGOCIO]) VALUES " &
                                    "(@LOG_USER_ID , @LOG_USER_CC , @LOG_HOST , @LOG_IP , @LOG_FECHA , @LOG_APLICACION , @LOG_MODULO , @LOG_DOC_AFEC , @LOG_CONSULTA,  @LOG_NEGOCIO)  "
                    cmd.Parameters.Add("@LOG_USER_ID", SqlDbType.VarChar, 40).Value = UsuarioId
                    cmd.Parameters.Add("@LOG_USER_CC", SqlDbType.VarChar, 50).Value = ""
                    cmd.Parameters.Add("@LOG_HOST", SqlDbType.VarChar, 50).Value = If((ClientHostName() Is Nothing), String.Empty, ClientHostName())
                    cmd.Parameters.Add("@LOG_IP", SqlDbType.VarChar, 20).Value = If((ClientIpAddress() Is Nothing), String.Empty, ClientIpAddress())
                    cmd.Parameters.Add("@LOG_FECHA", SqlDbType.DateTime).Value = Date.Now
                    cmd.Parameters.Add("@LOG_APLICACION", SqlDbType.VarChar, 100).Value = If((AplicationName() Is Nothing), String.Empty, AplicationName())
                    cmd.Parameters.Add("@LOG_MODULO", SqlDbType.VarChar, 100).Value = NombreModulo
                    cmd.Parameters.Add("@LOG_DOC_AFEC", SqlDbType.VarChar, 50).Value = Documento
                    cmd.Parameters.Add("@LOG_CONSULTA", SqlDbType.VarChar, 5000).Value = Commando
                    cmd.Parameters.Add("@LOG_NEGOCIO", SqlDbType.VarChar, 150).Value = GenerarComandoNegocio(Commando)
                    Connection.Open()
                    cmd.ExecuteNonQuery()
                    Connection.Close()
                    Connection.Dispose()
                    cmd.Dispose()
                End Using
            End Using
            resultado = True
        Catch ex As Exception
            mens = ex.Message
        End Try
        Return resultado
    End Function
    ''' <summary>
    ''' Realiza la traducción de los comandos a descripciones de negocio
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Public Function Traductor(ByVal llave As String) As String
        Dim resultado As String
        Dim entidad As New UGPP.CobrosCoactivo.Entidades.DiccionarioAditoria
        entidad = ListaDiccionario.Where(Function(x) x.VALOR_ORIGINAL = llave.ToUpper).FirstOrDefault()
        If entidad IsNot Nothing Then
            resultado = entidad.VALOR_DESTINO
        Else
            resultado = llave
        End If
        Return resultado
    End Function
    ''' <summary>
    ''' Genera el comando de negocio
    ''' </summary>
    ''' <param name="comando"></param>
    ''' <returns></returns>
    Public Function GenerarComandoNegocio(ByVal comando As String) As String
        comando = comando.Replace("(", " ")
        comando = comando.Replace(")", " ")
        comando = comando.Replace("[", " ")
        comando = comando.Replace("]", " ")
        comando = comando.Replace(",", " ")
        comando = comando.Replace(";", " ")
        Dim arrayPalabras As String()
        arrayPalabras = comando.Split(" ")
        Dim resultado As New StringBuilder
        For Each word In arrayPalabras
            If word IsNot String.Empty Then
                resultado.Append(String.Concat(Me.Traductor(word), " "))
            End If
        Next
        Return resultado.ToString()
    End Function
    ''' <summary>
    ''' Salva los errores en un log tipo archivo
    ''' </summary>
    ''' <param name="mensaje">mensaje de error</param>
    ''' <param name="clase">clase en que ocurre el error</param>
    ''' <param name="modulo">modulo de donde ocurre</param>
    Public Sub ErrorLog(ByVal mensaje As String, ByVal clase As String, ByVal modulo As String, ByVal tipo As ErrorType, Optional ByVal StarkTrace As String = Nothing)
        Dim path As String
        path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("UgppLogFile"))
        log4net.Config.XmlConfigurator.Configure(New FileInfo(path))
        If StarkTrace IsNot Nothing Then
            mensaje = mensaje & StarkTrace
        End If
        Select Case tipo
            Case 1
                log.Error(String.Concat(clase, "::", modulo, "::", mensaje))
            Case 2
                log.Warn(String.Concat(clase, "::", modulo, "::", mensaje))
            Case 3
                log.Debug(String.Concat(clase, "::", modulo, "::", mensaje))
            Case 4
                log.Fatal(String.Concat(clase, "::", modulo, "::", mensaje))
            Case Else
                log.Error(String.Concat(clase, "::", modulo, "::", mensaje))
        End Select
    End Sub
End Class


