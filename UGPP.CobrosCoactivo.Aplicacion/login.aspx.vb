Imports System.Data.SqlClient
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports System.DirectoryServices.Protocols
Imports System.Security.Cryptography.X509Certificates
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica
Imports System.Web.UI.Control
Imports coactivosyp.My.Resources

Public Class login
    Inherits System.Web.UI.Page
    Protected DataUser As SqlDataReader
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.daEntidades = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.DsPensiones1 = New dsPensiones
        CType(Me.DsPensiones1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'daEntidades
        '
        Me.daEntidades.SelectCommand = Me.SqlSelectCommand1
        Me.daEntidades.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "entescobradores", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("codigo", "codigo"), New System.Data.Common.DataColumnMapping("nombre", "nombre")})})
        '
        'SqlSelectCommand1
        '

        Me.SqlSelectCommand1.CommandText = "SELECT codigo, nombre, ent_ruta, ent_rutalocal FROM entescobradores"
        Me.SqlSelectCommand1.Connection = Me.SqlConnection1
        '
        'DsPensiones1
        '
        Me.DsPensiones1.DataSetName = "dsPensiones"
        Me.DsPensiones1.Locale = New System.Globalization.CultureInfo("es-CO")
        CType(Me.DsPensiones1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Protected WithEvents CustomValidator1 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents form1 As Global.System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents TxtUserId As Global.System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtPwd As Global.System.Web.UI.WebControls.TextBox
    'Protected WithEvents Submit1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    'Protected WithEvents btn As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents BtnAceptar As Global.System.Web.UI.WebControls.Button
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlPerfil As System.Web.UI.WebControls.DropDownList
    Protected WithEvents daEntidades As System.Data.SqlClient.SqlDataAdapter
    Protected WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents DsPensiones1 As dsPensiones
    Protected WithEvents pnlSeleccionarDatos As Global.System.Web.UI.WebControls.Panel
    Protected WithEvents btnNo As Global.System.Web.UI.WebControls.Button
    Protected WithEvents btnSi As Global.System.Web.UI.WebControls.Button
    Protected WithEvents Button1 As Global.System.Web.UI.WebControls.Button
    Protected WithEvents mpeSeleccion As Global.AjaxControlToolkit.ModalPopupExtender
    Protected WithEvents cmdAceptar As Global.System.Web.UI.WebControls.Button
    Protected WithEvents UpdatePanel1 As Global.System.Web.UI.UpdatePanel
    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager
    Protected WithEvents rfvCedulanit As Global.System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidatorCalloutExtender1 As Global.AjaxControlToolkit.ValidatorCalloutExtender
    Protected WithEvents RequiredFieldValidator1 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidatorCalloutExtender2 As Global.AjaxControlToolkit.ValidatorCalloutExtender
    Protected WithEvents DropImpuesto As System.Web.UI.WebControls.DropDownList
    Protected WithEvents mp2 As Global.AjaxControlToolkit.ModalPopupExtender
    Protected WithEvents LoginSD As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents HiddenssCampoClave As Global.System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Hiddenssrutalocalexpediente As Global.System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Hiddenssrutaexpediente As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub
#End Region

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("useractivo") = msn
        Me.CustomValidator1.IsValid = False
        mpeSeleccion.Show()
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SqlConnection1.ConnectionString = Funciones.CadenaConexion
        If Not IsPostBack Then
            ViewState("con") = 0
            Try

                If Me.daEntidades.Fill(Me.DsPensiones1, "entescobradores") Then
                    DropImpuesto.Enabled = False
                    Me.DropDownList1.DataBind()
                    Call llenar_Drop()
                End If
                If Not Request("clave") Is Nothing Then
                    FormsAuthentication.SignOut()
                End If
            Catch sqlEx As SqlException
                Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Coactivo S&P no  puede conectar o no tiene acceso  origen de datos configurado, puede que no tenga permiso para utilizar el recurso de datos. Si el problema persiste favor comuníquese  con el <b>administrador</b> de este servidor  para comprobar si tiene permisos de acceso.<br />" _
                & "<b>Nombre Servidor : </b>" & SqlConnection1.WorkstationId & "   <br /><hr /><h2>ERROR TÉCNICO</h2>"

                menssageError(amsbgox & sqlEx.Message)
                Me.CustomValidator1.IsValid = False
                LoginSD.InnerHtml = "SIN CONEXIÓN"
            Catch ex As Exception
                Me.CustomValidator1.Text = ex.Message.ToString()
                Me.CustomValidator1.IsValid = False
            End Try
        End If
    End Sub

    Private Sub Permisos(ByVal cod_user As String)
        Try
            Using myadapter As New SqlDataAdapter("SELECT * FROM USUARIO_PPALMENU WHERE me_usuario = @me_usuario ORDER BY ME_OPCIONINDEX ASC", Me.SqlConnection1)
                myadapter.SelectCommand.Parameters.Add("@me_usuario", SqlDbType.VarChar).Value = UCase(Trim(cod_user))
                Dim mytb As New DataTable
                myadapter.Fill(mytb)
                If mytb.Rows.Count > 0 Then
                    Session("ConExp") = mytb.Rows(0).Item("ME_PERMISO")
                    Session("ActExp") = mytb.Rows(1).Item("ME_PERMISO")
                    Session("ConDia") = mytb.Rows(2).Item("ME_PERMISO")
                    Session("OpPredial") = mytb.Rows(3).Item("ME_PERMISO")
                    Session("usuario") = mytb.Rows(4).Item("ME_PERMISO")
                    Session("caClave") = mytb.Rows(5).Item("ME_PERMISO")
                    Session("impActAdmin") = mytb.Rows(6).Item("ME_PERMISO")
                    Session("ejecuActuac") = mytb.Rows(7).Item("ME_PERMISO")
                    Session("salcontrires") = mytb.Rows(8).Item("ME_PERMISO")
                    Session("datosbasicos") = mytb.Rows(9).Item("ME_PERMISO")

                    Session("meregisActos") = mytb.Rows(10).Item("ME_PERMISO")
                    Session("meregisEtapa") = mytb.Rows(11).Item("ME_PERMISO")
                    Session("meconfigimprimi") = mytb.Rows(12).Item("ME_PERMISO")
                    Session("meregideu") = mytb.Rows(13).Item("ME_PERMISO")
                    Session("mesecueacto") = mytb.Rows(14).Item("ME_PERMISO")
                    Session("meregisEntes") = mytb.Rows(15).Item("ME_PERMISO")
                    Session("mefestivoadd") = mytb.Rows(16).Item("ME_PERMISO")
                    Session("meResolAcumulado") = mytb.Rows(17).Item("ME_PERMISO")
                Else
                    Session("ConExp") = False
                    Session("ActExp") = False
                    Session("ConDia") = False
                    Session("OpPredial") = False
                    Session("usuario") = False
                    Session("caClave") = False
                    Session("impActAdmin") = False
                    Session("ejecuActuac") = False
                    Session("salcontrires") = False
                    Session("datosbasicos") = False
                    Session("meregisActos") = False
                    Session("meregisEtapa") = False
                    Session("meconfigimprimi") = False
                    Session("meregideu") = False
                    Session("mesecueacto") = False
                    Session("meregisEntes") = False
                    Session("mefestivoadd") = False
                    Session("meResolAcumulado") = False
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Function GetTipoAutenticacion() As String
        Dim Tipo As String = ""
        Dim usarad As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM CONFIGURACIONES_ACTIVE_DIRECTORY"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            usarad = Reader("usarad").ToString().ToUpper
        End If
        Reader.Close()
        Connection.Close()

        If usarad = "TRUE" Then
            Tipo = "ACTIVEDIRECTORY"
        Else
            Tipo = "LOCAL"
        End If

        Return Tipo
    End Function

    Private Function ConectarLdap(ByVal pServidor As String, ByVal pPuerto As Integer, ByVal pUsuarioServicio As String, ByVal pClaveUsuarioServ As String, ByRef pMsg As String) As LdapConnection

        Dim msgError As String = ""

        '//Use the servername and port that you setup LDAP Directory Service on                
        Dim ldapDir As New LdapDirectoryIdentifier(pServidor, pPuerto)
        Dim ldapConn As New LdapConnection(ldapDir)

        '//You may need to try different types of Authentication depending on your setup        
        ldapConn.AuthType = AuthType.Basic

        '//Update the next line to include the Fully Qualified LDAP name of the user along with that user's password        
        Dim myCredentials As New System.Net.NetworkCredential(pUsuarioServicio, pClaveUsuarioServ)

        '//This is the actual Connection establishment here   
        Try
            ldapConn.Bind(myCredentials)
        Catch ex As Exception
            msgError = ex.Message
        End Try

        pMsg = msgError

        Return ldapConn

    End Function

    Public Function GetDomainGroups(ByVal userName As String, ByVal userPass As String, ByVal domain As String) As IList(Of GroupPrincipal)
        Dim result As List(Of GroupPrincipal) = New List(Of GroupPrincipal)()
        'Dim domain As String = "UGPP"
        Dim useGroup As String = "True"
        Dim groups As PrincipalSearchResult(Of Principal) = Nothing
        Dim distinguishedName As String = ConfigurationManager.AppSettings("DistinguishedName")
        Try
            Dim yourDomain As PrincipalContext = New PrincipalContext(ContextType.Domain, domain, distinguishedName, userName, userPass) 'PrincipalContext(ContextType.Domain, domain, userName, "Ugpp2015")
            Dim user As UserPrincipal = UserPrincipal.FindByIdentity(yourDomain, userName)

            If user IsNot Nothing Then

                If useGroup.Equals("true") Then
                    groups = user.GetGroups()
                Else
                    groups = user.GetAuthorizationGroups()
                End If

                For Each p As Principal In groups

                    If TypeOf p Is GroupPrincipal Then
                        result.Add(CType(p, GroupPrincipal))
                    End If
                Next
            End If

        Catch ex As Exception
            Throw
        End Try

        Return result
    End Function

    ''' <summary>
    ''' Obtiene del departamento del area origen y lo asigna a la variable 
    ''' de sesion  usrAreaOrgen
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="ldapConn"></param>
    Public Sub getDepartment(ByVal username As String, ByVal ldapConn As LdapConnection)
        Dim findme As New SearchRequest()
        findme.DistinguishedName = ConfigurationManager.AppSettings("DistinguishedName")
        findme.Filter = "(samaccountname=" & username & ")"
        findme.Attributes.Add("department")
        findme.Scope = System.DirectoryServices.Protocols.SearchScope.Subtree
        Dim results As SearchResponse = ldapConn.SendRequest(findme)

        Dim entries As SearchResultEntryCollection = results.Entries

        Dim i, ic As Integer

        Dim entry As SearchResultEntry
        Dim attribEnum As IDictionaryEnumerator
        Dim valor1 As String = ""
        Dim valor2 As String = ""

        Dim subAttrib As DirectoryAttribute

        For i = 0 To entries.Count - 1
            entry = entries(i)
            attribEnum = entry.Attributes.GetEnumerator()
            While attribEnum.MoveNext()
                subAttrib = attribEnum.Value

                For ic = 0 To subAttrib.Count - 1
                    valor1 = attribEnum.Key.ToString()
                    valor2 = subAttrib(ic).ToString()
                    If valor1 = "department" Then
                        Dim CodAreaOrigen As String = GetCodigo(subAttrib(ic).ToString())
                        Session("usrAreaOrgen") = CodAreaOrigen
                    End If
                Next
            End While
        Next
        If Session("usrAreaOrgen") = Nothing Then
            Session("usrAreaOrgen") = String.Empty
        End If
        If String.IsNullOrEmpty(Session("usrAreaOrgen").ToString()) Then
            Session("usrAreaOrgen") = ConfigurationManager.AppSettings("AREA_DEFAULT")
        End If
    End Sub

    Private Function GetCodigo(ByVal NombreDepartamento As String) As String
        Dim areaOrigen As String = "AREA_"
        Dim KeyName As String = ConfigurationManager.AppSettings(areaOrigen + NombreDepartamento.Trim().ToUpper())
        If KeyName Is Nothing Then
            KeyName = String.Empty
        End If
        Return KeyName
    End Function

    Public Function getUserDN(ByVal username As String, ByVal ldapConn As LdapConnection, ByRef pGrupos As String, ByRef grouplist As List(Of String), Optional ByRef pefilList As List(Of Entidades.Perfiles) = Nothing) As String
        'Indicar que se va a buscar
        Dim findme As New SearchRequest()
        'findme.DistinguishedName = "OU=Usuarios,DC=ugppdc,DC=local"
        'findme.DistinguishedName = "OU=Service Accounts,DC=ugppdc,DC=local"
        'findme.DistinguishedName = "CN=s-devglobal,OU=Service Accounts,DC=ugppdc,DC=local"
        'findme.DistinguishedName = "CN=s-devglobal,OU=Service Accounts,DC=ugppdc,DC=local"
        'findme.DistinguishedName = "OU=Usuarios,DC=ugpp,DC=local"
        findme.DistinguishedName = ConfigurationManager.AppSettings("DistinguishedName")
        'findme.DistinguishedName = "OU=Coactivo S&P_QA,OU=Aplicaciones,OU=Grupos,DC=UGPP,DC=PRUEBAS"
        '10/dic/2014. Variable para el usuario activo userAccountControl
        Dim UserActivo As String = ""
        Dim gropList As New List(Of String)
        Dim perfilesUsuarios As New List(Of Entidades.Perfiles)

        findme.Filter = "(samaccountname=" & username & ")"
        '  findme.Attributes.Add("department")
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
        If entries.Count > 0 Then

        End If
        If entries.Count > 0 Then
            Dim _perfilBLL As New PerfilBLL()
            Dim perfiles As List(Of Entidades.Perfiles) = _perfilBLL.obtenerPerfilesActivos()
            For i = 0 To entries.Count - 1
                entry = entries(i)
                attribEnum = entry.Attributes.GetEnumerator()
                While attribEnum.MoveNext()
                    subAttrib = attribEnum.Value

                    For ic = 0 To subAttrib.Count - 1
                        valor1 = attribEnum.Key.ToString()
                        valor2 = subAttrib(ic).ToString()
                        If valor1 = "distinguishedname" Then
                            'distinguishedname = valor2
                            distinguishedname = entry.DistinguishedName.ToString
                        End If
                        If valor1 = "memberof" Then
                            For Each perfil As Entidades.Perfiles In perfiles
                                If valor2.IndexOf(perfil.val_ldap_group) >= 0 Then
                                    pGrupos = perfil.nombre_perfil
                                    gropList.Add(perfil.nombre_perfil)
                                    perfilesUsuarios.Add(perfil)
                                End If
                            Next
                        End If

                        '10/DIC/2014. Verificacion de los usuarios deshabilitados
                        If valor1.ToUpper = "USERACCOUNTCONTROL" Then '"userAccountControl" Then
                            UserActivo = valor2
                        End If

                    Next
                End While
            Next
        End If
        'Grupos de usuarios a los que pertenece el usuario
        Session("UserGroupList") = gropList
        grouplist = gropList
        If Not IsNothing(pefilList) Then
            pefilList = perfilesUsuarios
        End If
        If UserActivo = "512" Then
            'todo bien
        Else
            'usuario deshabilitado
        End If

        ' si el distinguishedname esta vacio es porque el usuario no existe. Es posible que haya existido y fue eliminado, pero NO existe

        Return distinguishedname
    End Function

    Private Sub CustomValidator1_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustomValidator1.ServerValidate
        Dim logSessionInit As New LogProcesos
        Try
            With Me
                If Trim(.TxtUserId.Text) = "" Or Trim(.TxtPwd.Text) = "" Then
                    .CustomValidator1.ErrorMessage = "Fue imposible ingresar con la información suministrada"
                    CustomValidator1.IsValid = False
                    Exit Sub
                End If

                Dim cmd As String = "SELECT * FROM USUARIOS WHERE (LOGIN = @codigo) AND CLAVE = @clave AND COBRADOR = @cobrador"
                'Dim cmd As String = "SELECT * FROM USUARIOS WHERE (LOGIN = @codigo)  AND COBRADOR = @cobrador"
                Dim oSQLCmd As System.Data.SqlClient.SqlCommand

                oSQLCmd = New SqlCommand(cmd, Me.SqlConnection1)
                oSQLCmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = UCase(Trim(TxtUserId.Text))
                oSQLCmd.Parameters.Add("@clave", SqlDbType.VarChar).Value = Encrypt(TxtPwd.Text.Trim)
                oSQLCmd.Parameters.Add("@cobrador", SqlDbType.VarChar).Value = Trim(Me.DropDownList1.SelectedValue)

                Me.SqlConnection1.Open()
                .DataUser = oSQLCmd.ExecuteReader
                If Not .DataUser.Read Then
                    .CustomValidator1.ErrorMessage = "Usuario y/o clave erróneos"
                    CustomValidator1.IsValid = False

                    ViewState("con") = ViewState("con") + 1

                    If ViewState("con") = 3 Then
                        .DataUser.Close()
                        Dim cmdUsu As New SqlCommand("UPDATE USUARIOS SET USERACTIVO = 0 WHERE (LOGIN = @codigo)", Me.SqlConnection1)
                        cmdUsu.Parameters.AddWithValue("@codigo", TxtUserId.Text)
                        If cmdUsu.ExecuteNonQuery() > 0 Then
                            Me.SqlConnection1.Close()
                            Dim amsbgox As String = "<h2 class='err'>INFORMACIÓN</h2> EL usuario " & TxtUserId.Text.Trim & " ha sido inhabilitado por intentos erróneos contáctese con el <b>administrador</b> del sistema .<br /><br /><hr /><h2>INFORMACIÓN TÉCNICA</h2>"
                            menssageError(amsbgox)
                            Exit Sub
                        End If
                    End If
                Else
                    EstablecerVariablesDeSesion(DataUser)
                    If DataUser.GetValue(12) = True Then
                        Call Permisos(Session("sscodigousuario"))
                        logSessionInit.SaveLog(Session("ssnombreusuario"), "Inicio Session", String.Empty, "")
                        Dim mycommand As New SqlCommand("update MAESTRO_CONSECUTIVOS set @proximo_numero = con_user = con_user + 1 where CON_IDENTIFICADOR = 5", New SqlClient.SqlConnection(Funciones.CadenaConexion))
                        mycommand.Parameters.Add("@proximo_numero", SqlDbType.Int)
                        mycommand.Parameters("@proximo_numero").Direction = ParameterDirection.Output
                        '----------------
                        'Después de cada GRABAR hay que llamar al log de auditoria
                        'Dim LogProc As New LogProcesos
                        logSessionInit.SaveLog(Session("ssloginusuario"), "Actualización de consecutivos", "CON_IDENTIFICADOR = 5 ", mycommand)
                        'ObtenerPerfil(Session("sscodigousuario")) 'Obtengo: Session("perfil")
                        '28/ene/2014
                        Session("perfil") = Session("mnivelacces")
                        DataUser.Close()
                    Else
                        Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Este usuario se encuentra inhabilitado o no tiene acceso debido a protocolos de seguridad. Si el problema persiste favor comuníquese  con el <b>administrador</b> de este servidor  para comprobar si tiene permisos de acceso.<br /><br />" _
                                            & "<b>Nombre Servidor : </b>" & SqlConnection1.WorkstationId & "   <br /><hr /><h2>INICIO SESIÓN INVÁLIDO</h2>Inténtelo otra vez. <br /><br />"

                        menssageError(amsbgox)
                        CustomValidator1.ErrorMessage = "Lo sentimos el usuario está inactivo"
                        CustomValidator1.IsValid = False
                    End If
                End If
                .DataUser.Close()
                .SqlConnection1.Close()

            End With
        Catch sqlEx As SqlException
            Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Acceso invalido conexión no habilitada.<br />" _
               & "<b>Nombre Servidor : </b>" & SqlConnection1.WorkstationId & "   <br /><hr /><h2>ERROR TÉCNICO</h2>"

            menssageError(amsbgox & sqlEx.Message)
            Me.CustomValidator1.IsValid = False
        Finally
            ' DataUser.Close()
        End Try
    End Sub

    Private Sub ValidarActiveDirectory()
        'Se valida que el formulario de login contenga datos
        If Trim(TxtUserId.Text) = "" Or Trim(TxtPwd.Text) = "" Then
            CustomValidator1.ErrorMessage = "Fue imposible ingresar con la información suministrada"
            CustomValidator1.IsValid = False
            Exit Sub
        End If
        'Variables de conexion al servidor
        Dim servidor As String = ""
        Dim puerto As Integer = 0
        'Usuario de servicio
        Dim UsuarioServicio As String = ""
        Dim ClaveUsuarioServ As String = ""
        'Otras variables 
        Dim ConexionLdap As LdapConnection
        Dim msg As String = ""
        Dim userDN As String = ""
        Dim grupo As String = ""
        Dim grupos As String = ""
        Dim listadoGrupo As New List(Of String)
        Dim UsuarioConsultar As String = TxtUserId.Text.Trim
        Dim ClaveUsuarioCons As String = TxtPwd.Text.Trim
        Dim perfilesUsuarios As New List(Of Entidades.Perfiles)

        'consultar datos de conexion al server LDAP en la base de datos
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
        'Intentar conexion con las credenciales anteriores
        ConexionLdap = ConectarLdap(servidor, puerto, UsuarioServicio, ClaveUsuarioServ, msg)
        'GetUserGroupMembership(UsuarioServicio, ClaveUsuarioServ, servidor, puerto, UsuarioConsultar)
        If msg <> "" Then
            'Response.Write(msg)
            CustomValidator1.ErrorMessage = msg
            CustomValidator1.IsValid = False
            Exit Sub
        Else
            'Response.Write("conexion exitosa del usuario de servicio en servidor LDAP<br />")
        End If

        'Validando existencia del usuario
        userDN = getUserDN(UsuarioConsultar, ConexionLdap, grupos, listadoGrupo, perfilesUsuarios)
        getDepartment(UsuarioConsultar, ConexionLdap)
        If userDN = "" Then
            CustomValidator1.ErrorMessage = "Usuario no existe"
            CustomValidator1.IsValid = False
            Exit Sub
        Else
            'taObs.InnerHtml += "Usuario pertenece a los siguientes grupos: " & Mid(grupos, 4, InStr(grupos, ",") - 4) & Chr(13)
        End If

        'Se valida la contraseña del usuario contra LDAP
        Dim MsgFinal As String = ""
        MsgFinal = connectSSL(userDN, ClaveUsuarioCons, ConexionLdap)
        If MsgFinal <> "Usuario y clave correctos" Then
            CustomValidator1.ErrorMessage = MsgFinal
            CustomValidator1.IsValid = False
            Exit Sub
        End If
        'Se valida que cuente con un grupo valido relacionado en la aplicación
        If perfilesUsuarios.Count = 0 Then
            CustomValidator1.ErrorMessage = "Grupo no configurado en la aplicación"
            CustomValidator1.IsValid = False
            Exit Sub
        End If

        'Se valida la existencia del usuario en la base de datos de la aplicación
        Dim cmd As String = "SELECT * FROM USUARIOS WHERE (LOGIN = @codigo) AND COBRADOR = @cobrador"
        Dim oSQLCmd As SqlCommand = New SqlCommand(cmd, Me.SqlConnection1)
        oSQLCmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = UCase(Trim(TxtUserId.Text))
        oSQLCmd.Parameters.Add("@cobrador", SqlDbType.VarChar).Value = Trim(Me.DropDownList1.SelectedValue)
        Me.SqlConnection1.Open()
        Me.DataUser = oSQLCmd.ExecuteReader
        If Not DataUser.Read Then
            Me.CustomValidator1.ErrorMessage = "Usuario no registrado en la base de datos"
            CustomValidator1.IsValid = False
            Exit Sub
        End If
        'Se crean las variables de sesión
        Call Permisos(Session("sscodigousuario"))
        EstablecerVariablesDeSesion(Me.DataUser)
        If Me.DataUser.GetValue(12) = True Then
            Session("perfil") = Session("mnivelacces")
            DataUser.Close()
        Else
            Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Este usuario se encuentra inhabilitado o no tiene acceso debido a protocolos de seguridad. Si el problema persiste favor comuníquese  con el <b>administrador</b> de este servidor  para comprobar si tiene permisos de acceso.<br /><br />" _
                            & "<b>Nombre Servidor : </b>" & SqlConnection1.WorkstationId & "   <br /><hr /><h2>INICIO SESIÓN INVÁLIDO</h2>Inténtelo otra vez. <br /><br />"

            menssageError(amsbgox)
            CustomValidator1.ErrorMessage = "Lo sentimos el usuario está inactivo"
            CustomValidator1.IsValid = False
        End If
        DataUser.Close()
        SqlConnection1.Close()
        If perfilesUsuarios.Count > 1 Then
            'Si cuenta con mas de un perfil se muestra el dropdown para que seleccione un perfil
            For Each perfil As Entidades.Perfiles In perfilesUsuarios
                ddlPerfil.Items.Add(New ListItem(perfil.nombre_perfil, perfil.codigo))
            Next
            mp2.Show()
        Else
            'Si solo tiene un perfil se selecciona ese perfil y se redirecciona a su respectiva pantalla
            If perfilesUsuarios.FirstOrDefault().codigo = 12 Then
                'Se le asgina los permisos dentro de las paginas del revisor al coordinador 
                Session("mnivelacces") = 3
            Else
                Session("mnivelacces") = perfilesUsuarios.FirstOrDefault().codigo
            End If

            Session("perfil") = Session("mnivelacces")
            RedireccionarPorPerfil()
        End If

    End Sub

    Public Function connectSSL(ByVal username As String, ByVal password As String, ByVal ldapConn As LdapConnection) As String
        Dim msg As String = "Usuario y clave correctos"
        ' Opciones de conexion
        Dim ldapOptions As LdapSessionOptions = ldapConn.SessionOptions
        ldapOptions.ProtocolVersion = 3
        ldapConn.SessionOptions.VerifyServerCertificate = New VerifyServerCertificateCallback(AddressOf verifyCallback)
        ldapConn.Credential = New Net.NetworkCredential(username, password)
        ' ldapConn.SessionOptions.StartTransportLayerSecurity(Nothing)
        ldapConn.AuthType = AuthType.Basic
        'Intentar conexion
        Try
            ldapConn.Bind()
        Catch ex As Exception
            'msg = ex.Message
            msg = "Clave incorrecta"
        End Try

        Return msg

    End Function

    Public Function verifyCallback(ByVal connection As LdapConnection, ByVal certificate As X509Certificate) As Boolean
        'Do some stuff here to verify the server certificate, if you choose.
        Return True
    End Function

    Protected Sub BtnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAceptar.Click
        '05/marzo/2014
        ' Obtener el tipo de autenticacion que se va a usar para el acceso al software 
        Dim TipoAutenticacion As String = ""

        TipoAutenticacion = GetTipoAutenticacion()
        With Me
            If TipoAutenticacion = "LOCAL" Then
                CustomValidator1_ServerValidate(Nothing, Nothing)
                RedireccionarPorPerfil()
            Else
                ValidarActiveDirectory()
            End If
        End With
        If Session("usrAreaOrgen") = Nothing Then
            Session("usrAreaOrgen") = String.Empty
        End If
    End Sub

    Protected Sub btnSi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSi.Click
        Try
            Me.SqlConnection1.ConnectionString = Funciones.CadenaConexion

            If Me.daEntidades.Fill(Me.DsPensiones1, "entescobradores") Then
                DropImpuesto.Enabled = False
                Me.DropDownList1.DataBind()
                Call llenar_Drop()
            End If
        Catch ex As Exception
            menssageError("<h2 class='err'>ERROR</h2> Coactivo S&P no  puede conectar al origen de datos. <hr /><h2 class='err'>MONITOREO #1</h2>" _
                             & "<div><strong>Nombre del host :   </strong> " & Request.ServerVariables("HTTP_HOST") & "</div>" _
                             & "<div><strong>Navegador del usuario :  </strong> " & Request.Browser.Browser & "</div>" _
                             & "<div><strong>Direccion IP Local :  </strong> " & Request.ServerVariables("LOCAL_ADDR") & "</div>" _
                             & "<div><strong>Protocolo de conexión :  </strong> " & Request.ServerVariables("SERVER_PROTOCOL") & "</div>" _
                             & "<div><strong>Nombre del servidor :  </strong> " & Request.ServerVariables("SERVER_NAME") & "</div>" _
                             & "<div><strong>Dirección IP Remota :  </strong> " & Request.ServerVariables("REMOTE_ADDR") & "</div>" _
                             & "<div><strong>Puerto HTTP :  </strong> " & Request.ServerVariables("SERVER_PORT") & "</div><hr /><h2 class='err'>MONITOREO #2</h2>" _
                             & "<div><strong>Identificación el cliente de bases de datos :   </strong> " & "</div>" _
                             & "<div>" & SqlConnection1.WorkstationId & "</div>"
                         )
            Me.CustomValidator1.IsValid = False
        End Try
    End Sub

    Private Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Call llenar_Drop()
    End Sub

    Private Sub llenar_Drop()
        Using MyAdapter As New SqlDataAdapter("SELECT A.IMP_NOMBRE,A.IMP_VALUES,A.IMP_ENTECOBRADOR,A.IMP_ID,B.NOMBRE,A.IMP_CAMPOCLAVEID,B.ENT_RUTA,B.ENT_RUTALOCAL FROM DOCUMENTO_IMPUESTO A INNER JOIN ENTESCOBRADORES B ON A.IMP_ENTECOBRADOR = B.codigo WHERE IMP_ENTECOBRADOR = @IMP_ENTECOBRADOR", Funciones.CadenaConexion)
            MyAdapter.SelectCommand.Parameters.Add("@IMP_ENTECOBRADOR", SqlDbType.Char, 2).Value = Me.DropDownList1.SelectedValue
            Using dataForm As New DatasetForm
                MyAdapter.Fill(dataForm.DOCUMENTO_IMPUESTO)
                If dataForm.DOCUMENTO_IMPUESTO.Rows.Count > 0 Then
                    Session("ssrutalocalexpediente") = dataForm.DOCUMENTO_IMPUESTO.Rows(0).Item("ENT_RUTALOCAL")
                    Session("ssrutaexpediente") = dataForm.DOCUMENTO_IMPUESTO.Rows(0).Item("ENT_RUTA")

                    Hiddenssrutalocalexpediente.Value = valorNull(dataForm.DOCUMENTO_IMPUESTO.Rows(0).Item("ENT_RUTALOCAL"), Nothing)
                    Hiddenssrutaexpediente.Value = valorNull(dataForm.DOCUMENTO_IMPUESTO.Rows(0).Item("ENT_RUTA"), Nothing)
                    HiddenssCampoClave.Value = valorNull(dataForm.DOCUMENTO_IMPUESTO.Rows(0).Item("IMP_CAMPOCLAVEID"), Nothing)

                    DropImpuesto.Enabled = True

                    DropImpuesto.DataSource = dataForm.DOCUMENTO_IMPUESTO
                    DropImpuesto.DataValueField = "IMP_VALUES"
                    DropImpuesto.DataTextField = "IMP_NOMBRE"
                    DropImpuesto.DataBind()
                    LoginSD.InnerHtml = ""
                    'LoginSD.InnerHtml = "ENTIDAD COBRADORA CON MAS DE UN IMPUESTOS, SELECCIONE EL DE SU PREFERENCIA."
                Else
                    DropImpuesto.Enabled = False
                    LoginSD.InnerHtml = "NO SE DETECTARON IMPUESTOS ASOCIADOS A LA ENTIDAD COBRADORA."
                End If
            End Using
        End Using
    End Sub

    Private Function GetUserGroupMembership(ByVal strUser As String, ByVal userPassword As String, ByVal server As String, ByVal port As String, ByVal userQuery As String) As StringCollection
        Dim groups As StringCollection = New StringCollection()
        Dim domain As String
        domain = String.Concat("LDAP://", server)
        Try
            Dim obEntry As DirectoryEntry = New DirectoryEntry(domain, strUser, userPassword, AuthenticationTypes.None)
            Dim srch As DirectorySearcher = New DirectorySearcher(obEntry, "(sAMAccountName=" & userQuery & ")")
            Dim res As SearchResult = srch.FindOne()

            If res IsNot Nothing Then
                Dim obUser As DirectoryEntry = New DirectoryEntry(res.Path)
                Dim obGroups As Object = obUser.Invoke("Groups")

                For Each ob As Object In CType(obGroups, IEnumerable)
                    Dim obGpEntry As DirectoryEntry = New DirectoryEntry(ob)
                    groups.Add(obGpEntry.Name)
                Next
            End If

        Catch ex As Exception
            Trace.Write(ex.Message)
        End Try

        Return groups
    End Function

    Protected Sub cmdAceptar_Click(sender As Object, e As EventArgs) Handles cmdAceptar.Click
        Dim perfil = ddlPerfil.SelectedValue.ToString
        If Not IsNothing(perfil) Then
            Session("mnivelacces") = perfil
            Session("perfil") = Session("mnivelacces")
            RedireccionarPorPerfil()
        End If
        mp2.Hide()
    End Sub

    ''' <summary>
    ''' Crea variables de sesión compartidas entre el login de LDAP y base de datos local
    ''' En los dos logins el usuario debe existir en la base de datos
    ''' </summary>
    ''' <param name="prmObjDataUser">Resultado de la consulta del usuario a la base de datos local</param>
    Private Sub EstablecerVariablesDeSesion(ByVal prmObjDataUser As SqlDataReader)
        If Session("ssrutalocalexpediente") Is Nothing Or Session("ssrutaexpediente") Is Nothing Then
            Session("ssrutalocalexpediente") = Hiddenssrutalocalexpediente.Value
            Session("ssrutaexpediente") = Hiddenssrutaexpediente.Value
        End If

        If prmObjDataUser.GetValue(12) = True Then
            Session("ssCampoClave") = HiddenssCampoClave.Value
            Session("sscodigousuario") = prmObjDataUser.GetValue(0).ToString
            Session("ssnombreusuario") = prmObjDataUser.GetValue(1).ToString
            Session("mcobrador") = Trim(Me.DropDownList1.SelectedValue)
            Session("mnombcobrador") = Me.DropDownList1.SelectedItem.Text
            Session("mnivelacces") = prmObjDataUser.GetValue(4).ToString
            Session("mapppredial") = prmObjDataUser.GetValue(6).ToString
            Session("mappvehic") = prmObjDataUser.GetValue(7).ToString
            Session("mappcuotasp") = prmObjDataUser.GetValue(8).ToString
            Session("ConexionServer") = Me.SqlConnection1.ConnectionString
            Session("ssimpuesto") = DropImpuesto.SelectedValue
            Session("ssCodimpadm") = DropImpuesto.SelectedItem.Text
            Session("UsuarioValido") = "si"
            Session("usuario") = True
            '29/ene2014
            Session("superior") = prmObjDataUser("superior").ToString().Trim
            Session("ssloginusuario") = prmObjDataUser("login").ToString().Trim
        End If
    End Sub

    ''' <summary>
    ''' Método que se encarga de redireccionar al usuario dependiendo de su perfil
    ''' primero verifica si tiene módulos asignados, en caso que esto sea verdadero redirecciona al módulo de selección de módulos
    ''' </summary>
    Private Sub RedireccionarPorPerfil()
        With Me
            If .Page.IsValid Then
                If Session("sscodigousuario") <> Nothing Then
                    Session("UsuarioValido") = "si"
                    '/-----------------------------------------------------------------  
                    'ID _HU:  005 
                    'Nombre HU   : Redireccionamiento del botón Cancelar 
                    'Empresa: UT TECHNOLOGY 
                    'Autor: Jeisson Gómez 
                    'Fecha: 20-01-2017  
                    'Objetivo : Redireccionar a la página anterior en el botón cancelar, para cualquier 
                    '           perfil de usuario en la aplicación.
                    '------------------------------------------------------------------/ 
                    Session("MenuPrincipal") = False

                    FormsAuthentication.RedirectFromLoginPage(Session("sscodigousuario"), False)

                    'Valida si el perfil tiene módulos asignados
                    Dim _moduloBLL As New ModuloBLL()
                    Dim modulos As List(Of Entidades.Modulo) = _moduloBLL.obtenerModulosPorPerfil(CType(Session("perfil"), Int32))
                    If (modulos.Count > 0) Then
                        Response.Redirect("~/Security/Modulos.aspx", True)
                        Exit Sub
                    End If

                    'Response.Redirect("Security/menu.aspx", False)
                    '29/dic/2013
                    'Dependiendo del prfil muestro una pantalla inicial diferente
                    Select Case Session("perfil")
                        Case 1
                            ' SUPER ADMINISTRADOR
                            Response.Redirect("Security/menu.aspx", False)
                        Case 2
                            ' SUPERVISOR
                            Response.Redirect("Security/Maestros/EJEFISGLOBAL.aspx", False)
                        Case 3
                            ' REVISOR
                            Response.Redirect("Security/Maestros/EJEFISGLOBAL.aspx", False)
                        Case 4
                            ' GESTOR - ABOGADO
                            Response.Redirect("Security/Maestros/EJEFISGLOBAL.aspx", False)
                        Case 5
                            ' REPARTIDOR
                            Response.Redirect("Security/Maestros/EJEFISGLOBALREPARTIDOR.aspx", False)
                        Case 6
                            ' VERIFICADOR DE PAGOS
                            'Response.Redirect("Security/Maestros/PAGOS.aspx", False)
                            Response.Redirect("Security/Maestros/EJEFISGLOBAL.aspx", False)
                        Case 7
                            ' CREADOR DE USUARIOS
                            Response.Redirect("Security/menu4.aspx", False)
                        Case 8
                            ' GESTOR DE INFORMACION
                            Response.Redirect("Security/menu5.aspx", False)
                        Case 10
                            ' ESTUDIO TITULOS 
                            Response.Redirect("Security/Maestros/estudio-titulos/BandejaTitulos.aspx", False)
                        Case 11
                            'AREA ORIGEN
                            Response.Redirect("Security/Modulos/MaestroAreaOrigen.aspx", False)
                        Case Else
                            Response.Redirect("Security/Maestros/EJEFISGLOBAL.aspx", False)
                    End Select
                End If
            End If
        End With
    End Sub
End Class
