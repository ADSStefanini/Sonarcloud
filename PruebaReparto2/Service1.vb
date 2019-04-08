Imports System.Configuration
Imports System.Data.SqlClient
Imports System.DirectoryServices
Imports System.Net


Public Class Service1
    Dim usuarios As New List(Of String)
    Dim fecha As Date
    Dim DiaSemana As Int32
    Dim cont As Int32
    Dim name As String
    Dim strConn As String
    Private sqlCon As SqlConnection
    Dim retorno As String
    Dim iplocal As String
    Dim _scheduleTime As DateTime
    Private Timer As System.Threading.Timer = Nothing
    Private ProcessRunning As Boolean = False
    Private Shared Function ReadConfigFile(ByVal key As String) As String
        Return ConfigurationManager.AppSettings(key)
    End Function
    Public Sub OnStart()
        ' Agregue el código aquí para iniciar el servicio. Este método debería poner
        ' en movimiento los elementos para que el servicio pueda funcionar.
        ProcessRunning = False
        Timer_Elapsed(Nothing)
        'Timer = New System.Threading.Timer(New Threading.TimerCallback(AddressOf Timer_Elapsed), Nothing, 0, 60000)

    End Sub
    Private Sub Timer_Elapsed(ByVal sender As Object)
        If Not ProcessRunning Then Execute()
    End Sub

    Private Sub Execute()
        Dim format As String = "HH:mm"
        _scheduleTime = ConfigurationManager.AppSettings("Hora_Ejecucion")
        'If DateTime.Now.ToString("HH:mm") = _scheduleTime.ToString(format) Then
        Reparto_Automatico()
            ProcessRunning = False
        'End If

    End Sub
    Public Sub Reparto_Automatico()
        fecha = Date.Now
        DiaSemana = fecha.DayOfWeek
        Dim retorno As String
        retorno = False
        If (DiaSemana <= 5) Then
            strConn = System.Configuration.ConfigurationManager.AppSettings("cnn")
            Dim sqlConnection1 As New SqlConnection(strConn)
            Dim cmd As New SqlCommand
            Dim reader As SqlDataReader
            cmd.CommandText = "SELECT * FROM TDIAS_FESTIVOS WHERE FECHA=" + fecha.ToString("yyyy-MM-dd")
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()

            If (reader.HasRows = False) Then
                retorno = Listar_Directorio_Gestores_Titulos()
                If retorno Is Nothing Then
                    Listar_Directorio_Gestores_Expedientes()
                End If
                If retorno Is Nothing Then
                    retorno = "Proceso de reparto automatico finalizo satisfactoriamente"
                End If
                Log_Auditoria("Reparto", fecha, retorno)
            Else
                Return
            End If
        Else
            Return

        End If
        Return

    End Sub
    Public Function Listar_Directorio_Gestores_Titulos() As String
        Try
            name = System.Configuration.ConfigurationManager.AppSettings("DomainLdapQueryString")
            Dim usuario As String = System.Configuration.ConfigurationManager.AppSettings("user")
            Dim password As String = System.Configuration.ConfigurationManager.AppSettings("pss")
            Dim directory As DirectoryEntry = New DirectoryEntry(name, usuario, password)
            directory.AuthenticationType = AuthenticationTypes.Secure
            Dim GroupSearcher As New DirectorySearcher
            Dim grupo As String = System.Configuration.ConfigurationManager.AppSettings("Grupo_Titulo")
            With GroupSearcher
                .SearchRoot = directory
                .Filter = "(&(ObjectClass=Group)(CN=" + grupo + "))"
            End With

            Dim Members As Object = GroupSearcher.FindOne.GetDirectoryEntry.Invoke("Members", Nothing)
            For Each Member As Object In CType(Members, IEnumerable)  '<<< loop through members
                Dim CurrentMember As New DirectoryEntry(Member) '<<< Get user
                usuarios.Add(CurrentMember.Properties("sAMAccountName").Value)  '<<< Add user’s name 
            Next

            If (usuarios.Count() > 0) Then
                strConn = System.Configuration.ConfigurationManager.AppSettings("cnn")
                sqlCon = New SqlConnection(strConn)
                Dim sqlComm As New SqlCommand
                sqlComm.Connection = sqlCon
                Try
                    sqlCon.Open()
                    sqlComm.CommandText = "ASIGNACION_GESTORES"
                    sqlComm.CommandType = CommandType.StoredProcedure
                    sqlComm.Parameters.Add("@USERS", SqlDbType.VarChar, 200)
                    For Each users As String In usuarios
                        sqlComm.Parameters("@USERS").Value = users
                        sqlComm.ExecuteNonQuery()
                    Next
                    sqlCon.Close()
                    Reparto_Titulos()
                Catch ex As Exception
                    sqlCon.Close()
                    retorno = "Se presento el siguiente error en la asignacion de gestores de titulos: " + ex.Message.ToString
                    Return retorno
                End Try
            Else
                retorno = "El Grupo del LDAP " + grupo + "no tiene usuarios asociados como gestores de titulos"
                Return retorno

            End If
        Catch ex As Exception
            retorno = "Se presento el siguiente error al consultar el LDAP " + ex.Message.ToString
        End Try
        Return retorno
    End Function
    Public Sub Reparto_Titulos()
        strConn = System.Configuration.ConfigurationManager.AppSettings("cnn")
        sqlCon = New SqlConnection(strConn)
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlCon
        sqlComm.CommandText = "REPARTO_AUTOMATICO"
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlCon.Open()
        sqlComm.ExecuteNonQuery()
        sqlCon.Close()
    End Sub
    Public Sub Log_Auditoria(user As String, fecha As DateTime, log As String)
        Dim strHost As String = Dns.GetHostName()
        Dim ip As IPHostEntry = Dns.GetHostByName(strHost)
        If ip.AddressList.Count > 1 Then
            iplocal = ip.AddressList(0).ToString
        End If
        strConn = System.Configuration.ConfigurationManager.AppSettings("cnn")
        sqlCon = New SqlConnection(strConn)
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlCon
        sqlCon.Open()
        sqlComm.CommandText = "LOG_AUDITORIA_REPARTO_AUTOMATICO"
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.Parameters.Add("@FECHA", SqlDbType.DateTime)
        sqlComm.Parameters.Add("@LOG", SqlDbType.VarChar, 5000)
        sqlComm.Parameters.Add("@HOST", SqlDbType.VarChar, 5000)
        sqlComm.Parameters.Add("@IP", SqlDbType.VarChar, 5000)
        sqlComm.Parameters("@FECHA").Value = fecha
        sqlComm.Parameters("@LOG").Value = log
        sqlComm.Parameters("@HOST").Value = strHost
        sqlComm.Parameters("@IP").Value = iplocal
        sqlComm.ExecuteNonQuery()
        sqlCon.Close()
    End Sub
    Public Function Listar_Directorio_Gestores_Expedientes()
        Try
            name = System.Configuration.ConfigurationManager.AppSettings("DomainLdapQueryString")
            Dim usuario As String = System.Configuration.ConfigurationManager.AppSettings("user")
            Dim password As String = System.Configuration.ConfigurationManager.AppSettings("pss")
            Dim directory As DirectoryEntry = New DirectoryEntry(name, usuario, password)
            directory.AuthenticationType = AuthenticationTypes.Secure
            Dim GroupSearcher As New DirectorySearcher
            Dim grupo As String = System.Configuration.ConfigurationManager.AppSettings("Grupo_Expediente")
            With GroupSearcher
                .SearchRoot = directory
                .Filter = "(&(ObjectClass=Group)(CN=" + grupo + "))"
            End With

            Dim Members As Object = GroupSearcher.FindOne.GetDirectoryEntry.Invoke("Members", Nothing)
            For Each Member As Object In CType(Members, IEnumerable)  '<<< loop through members
                Dim CurrentMember As New DirectoryEntry(Member) '<<< Get user
                usuarios.Add(CurrentMember.Properties("sAMAccountName").Value)  '<<< Add user’s name 
            Next

            If (usuarios.Count() > 0) Then
                strConn = System.Configuration.ConfigurationManager.AppSettings("cnn")
                sqlCon = New SqlConnection(strConn)
                Dim sqlComm As New SqlCommand
                sqlComm.Connection = sqlCon
                Try
                    sqlCon.Open()
                    sqlComm.CommandText = "ASIGNACION_GESTORES_EXPEDIENTES"
                    sqlComm.CommandType = CommandType.StoredProcedure
                    sqlComm.Parameters.Add("@USERS", SqlDbType.VarChar, 200)
                    For Each users As String In usuarios
                        sqlComm.Parameters("@USERS").Value = users
                        sqlComm.ExecuteNonQuery()
                    Next
                    sqlCon.Close()
                    Reparto_Expedientes()
                Catch ex As Exception
                    sqlCon.Close()
                    retorno = "Se presento el siguiente error en la asignacion de gestores de titulos: " + ex.Message.ToString
                    Return retorno
                End Try
            Else
                retorno = "El Grupo del LDAP " + grupo + "no tiene usuarios asociados como gestores de expedientes"
                Return retorno

            End If
        Catch ex As Exception
            retorno = "Se presento el siguiente error al consultar el LDAP " + ex.Message.ToString
        End Try
        Return retorno
    End Function
    Public Sub Reparto_Expedientes()
        strConn = System.Configuration.ConfigurationManager.AppSettings("cnn")
        sqlCon = New SqlConnection(strConn)
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlCon
        sqlComm.CommandText = "REPARTO_EXPEDIENTES"
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlCon.Open()
        sqlComm.ExecuteNonQuery()
        sqlCon.Close()
    End Sub
End Class
