Imports System.Data.SqlClient
Imports System.DirectoryServices.Protocols

Public Class MetodosGlobalesAD
    ' 05/marzo/2015. Metodos para actualizar la base de datos en base al directorio activo ----------------------

    Public Sub SincronizarUsuarios()
        ' 1. Declarar variables
        Dim servidor, puerto, UsuarioServicio, ClaveUsuarioServ, msg As String
        Dim ConexionLdap As LdapConnection ' Imports System.DirectoryServices.Protocols

        '2. Iniciar variables
        servidor = ""
        puerto = ""
        UsuarioServicio = ""
        ClaveUsuarioServ = ""
        msg = ""

        '2. Conectar a la BD para extraer los datos de conexion del directorio activo
        GetDatosConexionDirectorioActivo(servidor, puerto, UsuarioServicio, ClaveUsuarioServ)

        '3. Intentar conexion con las credenciales anteriores
        ConexionLdap = ConectarLdap(servidor, puerto, UsuarioServicio, ClaveUsuarioServ, msg)

        '4. Si ocurre un error => terminar el proceso
        If msg <> "" Then
            'Response.Write(msg)
            Exit Sub
        End If

        ' 5. Conexion exitosa del usuario de servicio en servidor LDAP. Ingresar usuarios a un DataTable                
        ' Buscar todos los perfiles que esten relacionados con Coactivo SyP y meterlos en un datatable  
        Dim dt As DataTable = New DataTable("TablaUsuarios")
        dt.Columns.Add("CodigoDN") 'DistinguishedName
        dt.Columns.Add("Activo")
        dt.Columns.Add("Nivel")
        dt.Columns.Add("Login")

        getUsuariosDelGrupo("COAC_A_SUPERADMIN", ConexionLdap, dt, 1)
        getUsuariosDelGrupo("COAC_U_SUPERVISOR", ConexionLdap, dt, 2)
        getUsuariosDelGrupo("COAC_U_REVISOR", ConexionLdap, dt, 3)
        getUsuariosDelGrupo("COAC_U_GESTOR", ConexionLdap, dt, 4)
        getUsuariosDelGrupo("COAC_U_REPARTIDOR", ConexionLdap, dt, 5)
        getUsuariosDelGrupo("COAC_U_VERIFICADOROPAGOS", ConexionLdap, dt, 6)
        getUsuariosDelGrupo("COAC_U_CREADOROUSUARIOS", ConexionLdap, dt, 7)
        getUsuariosDelGrupo("COAC_U_GESTOROINFORMACION", ConexionLdap, dt, 8)

        ' Imprimir el datatable de usuarios
        'For x = 0 To dt.Rows.Count - 1            
        '    Response.Write(dt.Rows(x).Item("CodigoDN") & ". ACTIVO: (" & dt.Rows(x).Item("Activo") & "). NIVEL: " & dt.Rows(x).Item("Nivel") & ". LOGIN: " & dt.Rows(x).Item("Login") & "<br>")
        'Next

        ' 6. Recorrer el datatable de usuarios y buscar el codigo equivalente en la tabla de usuarios. 
        ' Si existe => actualizar estado, sino (es un usuario nuevo) => agregar a la tabla de usuarios
        Dim MTG As New MetodosGlobalesCobro
        Dim loginUsuario As String = ""
        Dim MsgRegistro As String = ""

        For x = 0 To dt.Rows.Count - 1
            'Response.Write(dt.Rows(x).Item("CodigoDN") & ". ACTIVO: (" & dt.Rows(x).Item("Activo") & "). NIVEL: " & dt.Rows(x).Item("Nivel") & ". LOGIN: " & dt.Rows(x).Item("Login") & "<br>")

            loginUsuario = dt.Rows(x).Item("Login").ToString.ToUpper

            If ExisteUsuarioBaseDatos(loginUsuario) Then
                'actualizar estado y/o nivel en tabla de usarios
                ActualizarUsuario(loginUsuario, dt.Rows(x).Item("Activo"), dt.Rows(x).Item("Nivel"))
            Else
                'registrar en tabla de usuarios 

                MsgRegistro = MTG.RegistrarUsuario(loginUsuario, dt.Rows(x).Item("Nivel"))
            End If
        Next

        'Response.Write("fin")
    End Sub

    Public Sub GetDatosConexionDirectorioActivo(ByRef servidor As String, ByRef puerto As String, ByRef UsuarioServicio As String, ByRef ClaveUsuarioServ As String)
        '
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM CONFIGURACIONES_ACTIVE_DIRECTORY"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            servidor = Reader("servidor").ToString().Trim
            puerto = CInt(Reader("puerto").ToString.Trim)
            UsuarioServicio = Reader("UsuarioServicio").ToString.Trim
            ClaveUsuarioServ = Reader("ClaveUsuarioServ").ToString.Trim
        End If
        Reader.Close()
        Connection.Close()
    End Sub

    Public Function ConectarLdap(ByVal pServidor As String, ByVal pPuerto As Integer, ByVal pUsuarioServicio As String, ByVal pClaveUsuarioServ As String, ByRef pMsg As String) As LdapConnection

        Dim msgError As String = ""

        '//Use the servername and port that you setup LDAP Directory Service on                
        Dim ldapDir As New LdapDirectoryIdentifier(pServidor, pPuerto)
        Dim ldapConn As New LdapConnection(ldapDir)

        ' Se pueden probar diferentes tipos de autenticaciones dependiendo de la configuración
        ldapConn.AuthType = AuthType.Basic

        ' Credenciales del usuario de servicio
        Dim myCredentials As New System.Net.NetworkCredential(pUsuarioServicio, pClaveUsuarioServ)

        ' Aqui es donde se intenta hacer la conexion
        Try
            ldapConn.Bind(myCredentials)
        Catch ex As Exception
            msgError = ex.Message
        End Try

        pMsg = msgError

        Return ldapConn

    End Function

    Public Sub getUsuariosDelGrupo(ByVal grupo As String, ByVal ldapConn As LdapConnection, ByVal dtUsuarios As DataTable, ByVal nivel As Integer)
        Dim CodLogin As String = ""

        'Indicar que se va a buscar
        Dim findme As New SearchRequest()
        findme.DistinguishedName = "CN=" & grupo & ",OU=Coactivo S&P_QA,OU=Aplicaciones,OU=Grupos,DC=ugppdc,DC=local"
        findme.Filter = "(samaccountname=" & grupo & ")"
        findme.Scope = System.DirectoryServices.Protocols.SearchScope.Subtree

        Dim results As SearchResponse = ldapConn.SendRequest(findme)
        Dim entries As SearchResultEntryCollection = results.Entries

        Dim i, ic As Integer

        Dim entry As SearchResultEntry
        Dim attribEnum As IDictionaryEnumerator
        Dim valor1 As String = ""
        Dim valor2 As String = ""
        Dim distinguishedname As String = ""

        Dim subAttrib As DirectoryAttribute
        For i = 0 To entries.Count - 1
            entry = entries(i)
            attribEnum = entry.Attributes.GetEnumerator()
            While attribEnum.MoveNext()
                subAttrib = attribEnum.Value

                For ic = 0 To subAttrib.Count - 1
                    valor1 = attribEnum.Key.ToString()
                    valor2 = subAttrib(ic).ToString()
                    'Response.Write(valor1 & " : " & valor2 & "<br>")

                    If valor1 = "member" Then
                        If Mid(valor2, 1, 3) = "CN=" Then
                            Dim dr As DataRow
                            dr = dtUsuarios.NewRow()
                            dr("CodigoDN") = Mid(valor2, 4, InStr(valor2, ",") - 4)
                            dr("Activo") = EstaActivo(valor2, ldapConn, CodLogin)
                            dr("Nivel") = nivel
                            dr("Login") = CodLogin
                            dtUsuarios.Rows.Add(dr)
                        End If
                    End If

                Next
            End While
        Next

    End Sub

    Public Function EstaActivo(ByVal username As String, ByVal ldapConn As LdapConnection, ByRef CodLogin As String) As String

        'Indicar que se va a buscar
        Dim findme As New SearchRequest()

        findme.DistinguishedName = "OU=Usuarios,DC=ugppdc,DC=local"

        '10/dic/2014. Variable para el usuario activo userAccountControl
        Dim UserActivo As String = ""


        findme.Filter = "(distinguishedname=" & username & ")"
        findme.Scope = System.DirectoryServices.Protocols.SearchScope.Subtree
        Dim results As SearchResponse = ldapConn.SendRequest(findme)

        Dim entries As SearchResultEntryCollection = results.Entries

        Dim i, ic As Integer

        Dim entry As SearchResultEntry
        Dim attribEnum As IDictionaryEnumerator
        Dim valor1 As String = ""
        Dim valor2 As String = ""
        Dim distinguishedname As String = ""

        Dim subAttrib As DirectoryAttribute
        For i = 0 To entries.Count - 1
            entry = entries(i)
            attribEnum = entry.Attributes.GetEnumerator()
            While attribEnum.MoveNext()
                subAttrib = attribEnum.Value

                For ic = 0 To subAttrib.Count - 1
                    valor1 = attribEnum.Key.ToString()
                    valor2 = subAttrib(ic).ToString()

                    'Response.Write(valor1 & ": " & valor2 & "<br>")

                    '10/DIC/2014. Verificacion de los usuarios deshabilitados
                    If valor1.ToUpper = "USERACCOUNTCONTROL" Then
                        UserActivo = valor2
                    End If

                    '05/MARZO/2015. Verificacion del LOGIN
                    If valor1.ToLower = "samaccountname" Then
                        CodLogin = valor2
                    End If

                Next
            End While
        Next

        If UserActivo = "512" Then
            'todo bien
            UserActivo = "S"
        Else
            'usuario deshabilitado
            UserActivo = "N"
        End If

        Return UserActivo

    End Function

    Public Function ExisteUsuarioBaseDatos(ByVal login As String) As Boolean
        Dim respuesta As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT login FROM USUARIOS WHERE login = @login"

        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@login", login)

        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            respuesta = True
        End If
        Reader.Close()
        Connection.Close()
        '
        Return respuesta
    End Function

    Public Sub ActualizarUsuario(ByVal login As String, ByVal activo As String, ByVal nivel As Integer)

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        'Comandos SQL
        Dim UpdateSQL As String = "UPDATE USUARIOS SET useractivo = @useractivo, nivelacces = @nivelacces WHERE login = @login"

        ' Update             
        Command = New SqlCommand(UpdateSQL, Connection)
        Command.Parameters.AddWithValue("@login", login)
        If activo.Trim = "S" Then
            Command.Parameters.AddWithValue("@useractivo", 1)
        Else
            Command.Parameters.AddWithValue("@useractivo", 0)
        End If
        Command.Parameters.AddWithValue("@nivelacces", nivel)

        Try
            Command.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Connection.Close()

    End Sub
    ' -----------------------------------------------------------------------------------------------------------

End Class
