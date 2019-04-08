Imports System.Data.SqlClient
Imports System.Web.HttpRequest
Public Class LogProcesos
    Public Sub New()

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

    Private Function ClientIpAddress() As String
        Dim currentRequest As HttpRequest = HttpContext.Current.Request
        Dim ipAddress As String = currentRequest.ServerVariables("HTTP_X_FORWARDED_FOR")
        If ipAddress Is Nothing OrElse ipAddress.ToLower() = "unknown" Then
            ipAddress = currentRequest.ServerVariables("REMOTE_ADDR")
        End If
        Return ipAddress
    End Function
    Private Function ClientHostName() As String
        Dim hostN As String = System.Net.Dns.GetHostEntry(ClientIpAddress).HostName
        If hostN = Nothing Then
            Return ""
        Else
            Return hostN
        End If
    End Function

    Private Function AplicationName() As String
        Return System.Configuration.ConfigurationManager.AppSettings("ApplicationName") & " " & System.Configuration.ConfigurationManager.AppSettings("ApplicationVersion")
    End Function

    Public Function SaveLog(ByVal UsuarioId As String, ByVal NombreModulo As String, ByVal Documento As String, ByVal Commando As SqlCommand) As Boolean
        Try
            Dim Connection As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim cmd As New SqlCommand
            cmd.Connection = Connection
            cmd.CommandText = "INSERT INTO LOG_AUDITORIA " & _
                            "([LOG_USER_ID], [LOG_USER_CC], [LOG_HOST], [LOG_IP], [LOG_FECHA], [LOG_APLICACION], [LOG_MODULO], [LOG_DOC_AFEC], [LOG_CONSULTA]) VALUES " & _
                            "(@LOG_USER_ID , @LOG_USER_CC , @LOG_HOST , @LOG_IP , @LOG_FECHA , @LOG_APLICACION , @LOG_MODULO , @LOG_DOC_AFEC , @LOG_CONSULTA) "
            cmd.Parameters.Add("@LOG_USER_ID", SqlDbType.VarChar, 40).Value = UsuarioId
            cmd.Parameters.Add("@LOG_USER_CC", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@LOG_HOST", SqlDbType.VarChar, 50).Value = ClientHostName()
            cmd.Parameters.Add("@LOG_IP", SqlDbType.VarChar, 20).Value = ClientIpAddress()
            cmd.Parameters.Add("@LOG_FECHA", SqlDbType.DateTime).Value = Date.Now
            cmd.Parameters.Add("@LOG_APLICACION", SqlDbType.VarChar, 100).Value = AplicationName()
            cmd.Parameters.Add("@LOG_MODULO", SqlDbType.VarChar, 100).Value = NombreModulo
            cmd.Parameters.Add("@LOG_DOC_AFEC", SqlDbType.VarChar, 50).Value = Documento
            cmd.Parameters.Add("@LOG_CONSULTA", SqlDbType.VarChar, 5000).Value = CreateScript(Commando)
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
            Dim cmd As New SqlCommand
            cmd.Connection = cnx
            cmd.CommandText = "INSERT INTO LOG_AUDITORIA " & _
                            "([LOG_USER_ID], [LOG_USER_CC], [LOG_HOST], [LOG_IP], [LOG_FECHA], [LOG_APLICACION], [LOG_MODULO], [LOG_DOC_AFEC], [LOG_CONSULTA]) VALUES " & _
                            "(@LOG_USER_ID , @LOG_USER_CC , @LOG_HOST , @LOG_IP , @LOG_FECHA , @LOG_APLICACION , @LOG_MODULO , @LOG_DOC_AFEC , @LOG_CONSULTA) "
            cmd.Parameters.Add("@LOG_USER_ID", SqlDbType.VarChar, 40).Value = UsuarioId
            cmd.Parameters.Add("@LOG_USER_CC", SqlDbType.VarChar, 50).Value = ""
            cmd.Parameters.Add("@LOG_HOST", SqlDbType.VarChar, 50).Value = ClientHostName()
            cmd.Parameters.Add("@LOG_IP", SqlDbType.VarChar, 20).Value = ClientIpAddress()
            cmd.Parameters.Add("@LOG_FECHA", SqlDbType.DateTime).Value = Date.Now
            cmd.Parameters.Add("@LOG_APLICACION", SqlDbType.VarChar, 100).Value = AplicationName()
            cmd.Parameters.Add("@LOG_MODULO", SqlDbType.VarChar, 100).Value = NombreModulo
            cmd.Parameters.Add("@LOG_DOC_AFEC", SqlDbType.VarChar, 50).Value = Documento
            cmd.Parameters.Add("@LOG_CONSULTA", SqlDbType.VarChar, 5000).Value = CreateScript(Commando)
            cmd.Transaction = transac
            'Connection.Open()
            cmd.ExecuteNonQuery()
            'Connection.Close()
            'Connection.Dispose()
            'cmd.Dispose()
            Return True
        Catch ex As Exception
            mens = ex.Message
            Return False
        End Try
    End Function

End Class
