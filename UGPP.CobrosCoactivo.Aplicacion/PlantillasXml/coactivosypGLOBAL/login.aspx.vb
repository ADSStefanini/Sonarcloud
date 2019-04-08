Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Security

Imports System.DirectoryServices
Imports System.DirectoryServices.Protocols
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates


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
    Protected WithEvents daEntidades As System.Data.SqlClient.SqlDataAdapter
    Protected WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents DsPensiones1 As dsPensiones
    Protected WithEvents pnlSeleccionarDatos As Global.System.Web.UI.WebControls.Panel
    Protected WithEvents btnNo As Global.System.Web.UI.WebControls.Button
    Protected WithEvents btnSi As Global.System.Web.UI.WebControls.Button
    Protected WithEvents Button1 As Global.System.Web.UI.WebControls.Button
    Protected WithEvents mpeSeleccion As Global.AjaxControlToolkit.ModalPopupExtender

    Protected WithEvents UpdatePanel1 As Global.System.Web.UI.UpdatePanel
    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager
    Protected WithEvents rfvCedulanit As Global.System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidatorCalloutExtender1 As Global.AjaxControlToolkit.ValidatorCalloutExtender
    Protected WithEvents RequiredFieldValidator1 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidatorCalloutExtender2 As Global.AjaxControlToolkit.ValidatorCalloutExtender
    Protected WithEvents DropImpuesto As System.Web.UI.WebControls.DropDownList


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

            '05/MARZO/2015
            Dim MTG2 As New MetodosGlobalesAD
            MTG2.SincronizarUsuarios()
            '------------------------------------------

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
                Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Tecno Expedientes no  puede conectar o no tiene acceso  origen de datos configurado, puede que no tenga permiso para utilizar el recurso de datos. Si el problema persiste favor comuníquese  con el <b>administrador</b> de este servidor  para comprobar si tiene permisos de acceso.<br />" _
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
        '
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

    Public Function getUserDN(ByVal username As String, ByVal ldapConn As LdapConnection, ByRef pGrupos As String) As String

        'Indicar que se va a buscar
        Dim findme As New SearchRequest()
        'findme.DistinguishedName = "OU=Usuarios,DC=ugpp,DC=pruebas"
        findme.DistinguishedName = "OU=Usuarios,DC=ugppdc,DC=local"
        'findme.DistinguishedName = "OU=Service Accounts,DC=ugppdc,DC=local"
        'findme.DistinguishedName = "CN=s-devglobal,OU=Service Accounts,DC=ugppdc,DC=local"

        '10/dic/2014. Variable para el usuario activo userAccountControl
        Dim UserActivo As String = ""


        findme.Filter = "(samaccountname=" & username & ")"
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
                    If valor1 = "distinguishedname" Then
                        distinguishedname = valor2
                    End If
                    If valor1 = "memberof" Then
                        If valor2.IndexOf("COAC_A_SUPERADMIN") >= 0 Then
                            pGrupos = "SUPERADMIN"
                        ElseIf valor2.IndexOf("COAC_U_SUPERVISOR") >= 0 Then
                            pGrupos = "SUPERVISOR"
                        ElseIf valor2.IndexOf("COAC_U_REVISOR") >= 0 Then
                            pGrupos = "REVISOR"
                        ElseIf valor2.IndexOf("COAC_U_GESTOROINFORMACION") >= 0 Then
                            pGrupos = "GESTOR DE INFORMACION"
                        ElseIf valor2.IndexOf("COAC_U_GESTOR") >= 0 Then
                            pGrupos = "GESTOR"
                        ElseIf valor2.IndexOf("COAC_U_REPARTIDOR") >= 0 Then
                            pGrupos = "REPARTIDOR"
                        ElseIf valor2.IndexOf("COAC_U_VERIFICADOROPAGOS") >= 0 Then
                            pGrupos = "VERIFICADOR DE PAGOS"
                        ElseIf valor2.IndexOf("COAC_U_CREADOROUSUARIOS") >= 0 Then
                            pGrupos = "CREADOR DE USUARIOS"
                        End If
                    End If

                    '10/DIC/2014. Verificacion de los usuarios deshabilitados
                    If valor1.ToUpper = "USERACCOUNTCONTROL" Then '"userAccountControl" Then
                        UserActivo = valor2
                    End If

                Next
            End While
        Next

        If UserActivo = "512" Then
            'todo bien
        Else
            'usuario deshabilitado
        End If

        ' si el distinguishedname esta vacio es porque el usuario no existe. Es posible que haya existido y fue eliminado, pero NO existe

        'Un ejemplo: "CN=fictipru,OU=Pruebas,OU=Usuarios,DC=ugppdc,DC=local"
        Return distinguishedname
    End Function

    Private Sub CustomValidator1_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustomValidator1.ServerValidate
        Try
            With Me
                If Trim(.TxtUserId.Text) = "" Or Trim(.TxtPwd.Text) = "" Then
                    .CustomValidator1.ErrorMessage = "Fue imposible ingresar con la información suministrada"
                    CustomValidator1.IsValid = False
                Else
                    Dim cmd As String = "SELECT * FROM USUARIOS WHERE (LOGIN = @codigo) AND CLAVE = @clave AND COBRADOR = @cobrador"
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
                        If Session("ssrutalocalexpediente") Is Nothing Or Session("ssrutaexpediente") Is Nothing Then
                            Session("ssrutalocalexpediente") = Hiddenssrutalocalexpediente.Value
                            Session("ssrutaexpediente") = Hiddenssrutaexpediente.Value
                        End If

                        If DataUser.GetValue(12) = True Then
                            Session("ssCampoClave") = HiddenssCampoClave.Value
                            Session("sscodigousuario") = .DataUser.GetValue(0).ToString
                            Session("ssnombreusuario") = .DataUser.GetValue(1).ToString
                            Session("mcobrador") = Trim(Me.DropDownList1.SelectedValue)
                            Session("mnombcobrador") = Me.DropDownList1.SelectedItem.Text
                            Session("mnivelacces") = .DataUser.GetValue(4).ToString
                            Session("mapppredial") = .DataUser.GetValue(6).ToString
                            Session("mappvehic") = .DataUser.GetValue(7).ToString
                            Session("mappcuotasp") = .DataUser.GetValue(8).ToString
                            Session("ConexionServer") = Me.SqlConnection1.ConnectionString
                            Session("ssimpuesto") = DropImpuesto.SelectedValue
                            Session("ssCodimpadm") = DropImpuesto.SelectedItem.Text
                            Session("UsuarioValido") = "si"

                            '29/ene2014
                            Session("superior") = .DataUser("superior").ToString().Trim
                            Session("ssloginusuario") = .DataUser("login").ToString().Trim

                            DataUser.Close()

                            Call Permisos(Session("sscodigousuario"))

                            'ObtenerPerfil(Session("sscodigousuario")) 'Obtengo: Session("perfil")
                            '28/ene/2014
                            Session("perfil") = Session("mnivelacces")
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
                End If
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

        If Trim(TxtUserId.Text) = "" Or Trim(TxtPwd.Text) = "" Then
            CustomValidator1.ErrorMessage = "Fue imposible ingresar con la información suministrada"
            CustomValidator1.IsValid = False
        Else
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
            Dim UsuarioConsultar As String = TxtUserId.Text.Trim
            Dim ClaveUsuarioCons As String = TxtPwd.Text.Trim

            'Consultar datos de conexion al server LDAP en la base de datos
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
            If msg <> "" Then
                'Response.Write(msg)
                CustomValidator1.ErrorMessage = msg
                CustomValidator1.IsValid = False
                Exit Sub
            Else
                'Response.Write("conexion exitosa del usuario de servicio en servidor LDAP<br />")
            End If

            'Validando existencia del usuario
            userDN = getUserDN(UsuarioConsultar, ConexionLdap, grupos)
            If userDN = "" Then
                CustomValidator1.ErrorMessage = "Usuario no existe"
                CustomValidator1.IsValid = False
                Exit Sub
            Else
                'taObs.InnerHtml += "Usuario pertenece a los siguientes grupos: " & Mid(grupos, 4, InStr(grupos, ",") - 4) & Chr(13)
            End If


            Dim MsgFinal As String = ""
            MsgFinal = connectSSL(userDN, ClaveUsuarioCons, ConexionLdap)
            If MsgFinal = "Usuario y clave correctos" Then
                'grupo = Mid(grupos, 4, InStr(grupos, ",") - 4)
                grupo = grupos '04/AGO/2014

                'COAC_A_SUPERADMIN - COAC_U_SUPERVISOR - COAC_U_REVISOR - COAC_U_GESTOR - COAC_U_REPARTIDOR - COAC_U_VERIFICADOROPAGOS
                '----------------------------------------------------------------------------------------------------------------------
                '''''grupo = "COAC_U_SUPERVISOR" ' Esta linea hay que quitarla... debe venir de la funcion connectSSL

                'If InStr(grupo, "SUPERADMIN") > 0 Then
                '    Session("mnivelacces") = 1
                'ElseIf InStr(grupo, "SUPERVISOR") > 0 Then
                '    Session("mnivelacces") = 2
                'ElseIf InStr(grupo, "REVISOR") > 0 Then
                '    Session("mnivelacces") = 3
                'ElseIf InStr(grupo, "GESTOR") > 0 Then
                '    Session("mnivelacces") = 4
                'ElseIf InStr(grupo, "REPARTIDOR") > 0 Then
                '    Session("mnivelacces") = 5
                'ElseIf InStr(grupo, "VERIFICADOR") > 0 And InStr(grupo, "PAGOS") > 0 Then
                '    Session("mnivelacces") = 6
                'End If

                If grupo = "SUPERADMIN" Then
                    Session("mnivelacces") = 1

                ElseIf grupo = "SUPERVISOR" Then
                    Session("mnivelacces") = 2

                ElseIf grupo = "REVISOR" Then
                    Session("mnivelacces") = 3

                ElseIf grupo = "GESTOR" Then
                    Session("mnivelacces") = 4

                ElseIf grupo = "REPARTIDOR" Then
                    Session("mnivelacces") = 5

                ElseIf grupo = "VERIFICADOR DE PAGOS" Then
                    Session("mnivelacces") = 6

                ElseIf grupo = "CREADOR DE USUARIOS" Then
                    Session("mnivelacces") = 7

                ElseIf grupo = "GESTOR DE INFORMACION" Then
                    Session("mnivelacces") = 8

                End If


                Dim cmd As String = "SELECT * FROM USUARIOS WHERE (LOGIN = @codigo) AND COBRADOR = @cobrador"
                Dim oSQLCmd As System.Data.SqlClient.SqlCommand

                oSQLCmd = New SqlCommand(cmd, Me.SqlConnection1)
                oSQLCmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = UCase(Trim(TxtUserId.Text))
                oSQLCmd.Parameters.Add("@cobrador", SqlDbType.VarChar).Value = Trim(Me.DropDownList1.SelectedValue)

                Me.SqlConnection1.Open()
                Me.DataUser = oSQLCmd.ExecuteReader

                If Not Me.DataUser.Read Then
                    ' 04/marzo/2015 xxxxxxxx
                    '  "Usuarios NO registrados en la base de datos" ************************

                    Dim MsgRegistro As String = ""
                    'Instanciar clase MetodosGlobalesCobro
                    Dim MTG As New MetodosGlobalesCobro
                    MsgRegistro = MTG.RegistrarUsuario(UCase(Trim(TxtUserId.Text)), Session("mnivelacces"))

                    If MsgRegistro <> "" Then
                        Me.CustomValidator1.ErrorMessage = MsgRegistro
                        CustomValidator1.IsValid = False
                        Exit Sub
                    End If


                    '- Los usuarios de nivel 3 en adelante necesitan tener asociado un superior
                    If Session("mnivelacces") >= 3 Then
                        Me.CustomValidator1.ErrorMessage = "Los usuarios de nivel " & grupo & " necesitan tener un superior asociado. Favor contactar a soporte técnico"
                        CustomValidator1.IsValid = False
                        Exit Sub
                    Else
                        ' Los usuarios de nivel 1 o 2 deben ser registrados en la base de datos                                                
                        Me.CustomValidator1.ErrorMessage = "Proceso de registro de usuario nuevo finalizado. Favor digitar nuevamente las credenciales"
                        CustomValidator1.IsValid = False
                        Exit Sub
                    End If

                Else
                    If Session("ssrutalocalexpediente") Is Nothing Or Session("ssrutaexpediente") Is Nothing Then
                        Session("ssrutalocalexpediente") = Hiddenssrutalocalexpediente.Value
                        Session("ssrutaexpediente") = Hiddenssrutaexpediente.Value
                    End If

                    If DataUser.GetValue(12) = True Then
                        Session("ssCampoClave") = HiddenssCampoClave.Value
                        Session("sscodigousuario") = DataUser.GetValue(0).ToString
                        Session("ssnombreusuario") = DataUser.GetValue(1).ToString
                        Session("mcobrador") = Trim(Me.DropDownList1.SelectedValue)
                        Session("mnombcobrador") = Me.DropDownList1.SelectedItem.Text
                        Session("mapppredial") = DataUser.GetValue(6).ToString
                        Session("mappvehic") = DataUser.GetValue(7).ToString
                        Session("mappcuotasp") = DataUser.GetValue(8).ToString
                        Session("ConexionServer") = Me.SqlConnection1.ConnectionString
                        Session("ssimpuesto") = DropImpuesto.SelectedValue
                        Session("ssCodimpadm") = DropImpuesto.SelectedItem.Text
                        Session("UsuarioValido") = "si"

                        Session("superior") = DataUser("superior").ToString().Trim
                        Session("ssloginusuario") = DataUser("login").ToString().Trim
                        DataUser.Close()

                        Call Permisos(Session("sscodigousuario"))
                        Session("perfil") = Session("mnivelacces")
                    Else
                        Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Este usuario se encuentra inhabilitado o no tiene acceso debido a protocolos de seguridad. Si el problema persiste favor comuníquese  con el <b>administrador</b> de este servidor  para comprobar si tiene permisos de acceso.<br /><br />" _
                                              & "<b>Nombre Servidor : </b>" & SqlConnection1.WorkstationId & "   <br /><hr /><h2>INICIO SESIÓN INVÁLIDO</h2>Inténtelo otra vez. <br /><br />"

                        menssageError(amsbgox)
                        CustomValidator1.ErrorMessage = "Lo sentimos el usuario está inactivo"
                        CustomValidator1.IsValid = False
                    End If
                    DataUser.Close()
                    SqlConnection1.Close()
                End If
                '------------------------------------------------------------------

            Else
                CustomValidator1.ErrorMessage = MsgFinal
                CustomValidator1.IsValid = False
                'Exit Sub
            End If


        End If

    End Sub

    Public Function connectSSL(ByVal username As String, ByVal password As String, ByVal ldapConn As LdapConnection) As String
        Dim msg As String = "Usuario y clave correctos"
        ' Opciones de conexion
        Dim ldapOptions As LdapSessionOptions = ldapConn.SessionOptions
        ldapOptions.ProtocolVersion = 3
        ldapConn.SessionOptions.VerifyServerCertificate = New VerifyServerCertificateCallback(AddressOf verifyCallback)
        ldapConn.Credential = New Net.NetworkCredential(username, password)
        ldapConn.SessionOptions.StartTransportLayerSecurity(Nothing)
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


        '27/08/2014. ojo quitar esta linea
        'TipoAutenticacion = "LOCAL"

        With Me
            If TipoAutenticacion = "LOCAL" Then
                CustomValidator1_ServerValidate(Nothing, Nothing)
            Else
                ValidarActiveDirectory()
            End If

            If .Page.IsValid Then
                If Session("sscodigousuario") <> Nothing Then
                    Session("UsuarioValido") = "si"

                    FormsAuthentication.RedirectFromLoginPage(Session("sscodigousuario"), False)
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
                            'Response.Redirect("Security/Maestros/plantilla.aspx?pag=EJEFISGLOBAL", False)
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

                        Case Else
                            Response.Redirect("Security/Maestros/EJEFISGLOBAL.aspx", False)
                    End Select
                End If
            End If
        End With
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
            menssageError("<h2 class='err'>ERROR</h2> Tecno Expedientes no  puede conectar al origen de datos. <hr /><h2 class='err'>MONITOREO #1</h2>" _
                             & "<div><strong>Nombre del host :   </strong> " & Request.ServerVariables("HTTP_HOST") & "</div>" _
                             & "<div><strong>Navegador del usuario :  </strong> " & Request.Browser.Browser & "</div>" _
                             & "<div><strong>Direccion IP Local :  </strong> " & Request.ServerVariables("LOCAL_ADDR") & "</div>" _
                             & "<div><strong>Protocolo de conexión :  </strong> " & Request.ServerVariables("SERVER_PROTOCOL") & "</div>" _
                             & "<div><strong>Nombre del servidor :  </strong> " & Request.ServerVariables("SERVER_NAME") & "</div>" _
                             & "<div><strong>Dirección IP Remota :  </strong> " & Request.ServerVariables("REMOTE_ADDR") & "</div>" _
                             & "<div><strong>Puerto HTTP :  </strong> " & Request.ServerVariables("SERVER_PORT") & "</div><hr /><h2 class='err'>MONITOREO #2</h2>" _
                             & "<div><strong>Identificación el cliente de bases de datos :   </strong> " & "</div>" _
                             & "<div>" & SqlConnection1.WorkstationId & "</div>" _
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
End Class
