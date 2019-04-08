Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class UsuarioDAL
    Inherits AccesObject(Of Usuario)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub

    ''' <summary>
    ''' Retorna la lista de todos los usuarios de la tabla aplicación
    ''' </summary>
    ''' <returns>Lista de objetos Entidades.Usuario</returns>
    Public Function ConsultarUsuarios() As List(Of Usuario)
        Return ExecuteList("SP_ConsultarUsuarios")
    End Function

    ''' <summary>
    ''' Obtiene un usuario por su ID 
    ''' </summary>
    ''' <param name="prmStrUsuarioId">Id del usuario</param>
    ''' <returns>Objeto tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuarioPorId(ByVal prmStrUsuarioId As String) As Datos.USUARIOS
        Dim usuario = (From u In db.USUARIOS
                       Where u.codigo.Equals(prmStrUsuarioId)
                       Select u).FirstOrDefault()
        Return usuario
    End Function

    ''' <summary>
    ''' Obtiene datos de un usuario por su login 
    ''' </summary>
    ''' <param name="prmStrLoginUsuario">Id del usuario</param>
    ''' <returns>Objeto tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuarioPorLogin(ByVal prmStrLoginUsuario As String) As Datos.USUARIOS
        Dim usuario = (From u In db.USUARIOS
                       Where u.login.Equals(prmStrLoginUsuario)
                       Select u).FirstOrDefault()
        Return usuario
    End Function

    ''' <summary>
    ''' Retorna el usuario superior de un usuario de la aplicación
    ''' </summary>
    ''' <param name="prmStrUsuarioId">Id del usuario que se quiere su superior</param>
    ''' <returns>Usuario superior, objeto del tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuarioSuperior(ByVal prmStrUsuarioId As String) As Datos.USUARIOS
        Dim usuario = obtenerUsuarioPorId(prmStrUsuarioId)
        If (String.IsNullOrEmpty(usuario.superior)) Then
            Return New Datos.USUARIOS
        End If
        Dim usuarioSuperior = obtenerUsuarioPorId(usuario.superior)
        Return usuarioSuperior
    End Function

    ''' <summary>
    ''' Retorna la lista de usuarios que pueden gestionar estudio de títulos o expedientes
    ''' </summary>
    ''' <param name="prmIntTipoGestor">{1: Gestores estudio de títulos; 2: Gestores expedientes}</param>
    ''' <returns>Lista de objetos del tipo Datos.USUARIOS</returns>
    Public Function ObtenerUsuariosReasignacion(ByVal prmIntTipoGestor As Integer) As List(Of Datos.USUARIOS)
        Dim usuarios As New List(Of Datos.USUARIOS)
        If prmIntTipoGestor = 1 Then
            usuarios = (From u In db.USUARIOS
                        Where u.ind_gestor_estudios = True
                        Order By u.nombre
                        Select u).ToList()
        ElseIf prmIntTipoGestor = 2 Then
            usuarios = (From u In db.USUARIOS
                        Where u.ind_gestor_expedientes = True
                        Order By u.nombre
                        Select u).ToList()
        End If
        Return usuarios
    End Function

    ''' <summary>
    ''' Retorna la lista de usuarios asignados a un usuario superior
    ''' </summary>
    ''' <param name="prmStrCodUsuarioSuperior">Código del usuario superior</param>
    ''' <returns>Lista de objetos del tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuariosPorSuperior(ByVal prmStrCodUsuarioSuperior As String) As List(Of Datos.USUARIOS)
        Dim usuarios = (From u In db.USUARIOS
                        Where u.superior = prmStrCodUsuarioSuperior
                        Order By u.nombre
                        Select u).ToList()
        Return usuarios
    End Function

    ''' <summary>
    ''' Retorna la lista de usuarios asignados a un usuario superior
    ''' </summary>
    ''' <param name="prmStrCodUsuarioSuperior">Código del usuario superior del Superior</param>
    ''' <returns>Lista de objetos del tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuariosPorSuperiorDelSuperior(ByVal prmStrCodUsuarioSuperior As String) As List(Of Datos.USUARIOS)
        Dim usuariosSuperior = (From u In db.USUARIOS
                                Where u.superior = prmStrCodUsuarioSuperior
                                Order By u.nombre
                                Select u.codigo).ToList()
        Dim usuariosCodigos As String() = usuariosSuperior.ToArray()

        Dim usuarios = (From u In db.USUARIOS
                        Where (usuariosCodigos.Contains(u.superior))
                        Order By u.nombre
                        Select u).ToList()
        Return usuarios
    End Function

    ''' <summary>
    ''' Retorna la lista de todos los usuarios de la tabla aplicación
    ''' </summary>
    ''' <param name="prmStrLoginUsuario">Login del usuario que se desea verificar</param>
    ''' <returns>Lista de objetos Entidades.Usuario</returns>
    Public Function VerificarSiUsuarioPerteneceATercerNivel(ByVal prmStrLoginUsuario) As List(Of Usuario)
        Return ExecuteList("SP_VERIFICA_SI_USUARIO_ES_TERCER_NIVEL", New SqlClient.SqlParameter("@USULOG", prmStrLoginUsuario))
    End Function

    ''' <summary>
    ''' Obtiene la lista de todos los gestores de estudio de títulos y expedientes
    ''' </summary>
    ''' <returns>Lista de objetos Datos.USUARIOS</returns>
    Public Function ObtenerGestoresTodos() As List(Of Datos.USUARIOS)
        Dim usuarios = (From u In db.USUARIOS
                        Where u.ind_gestor_estudios = True
                        Where u.ind_gestor_expedientes = True
                        Order By u.nombre
                        Select u).ToList()
        Return usuarios
    End Function

    ''' <summary>
    ''' Verifica si el login que se pasa como parametro pertenece a la jerarquía que se pasa como parámetro
    ''' </summary>
    ''' <param name="prnStrUserLogin">Login del usuario</param>
    ''' <param name="prmObjIdJerarquia">Identificador de la jerarquía</param>
    ''' <returns>Objeto del tipo Entidades.Usuario</returns>
    Public Function VerificarJerarquiaUsuarioPorLogin(ByVal prnStrUserLogin As String, ByVal prmObjIdJerarquia As Enumeraciones.JerarquiaUsuario) As Usuario
        Dim parametros As SqlClient.SqlParameter() = New SqlClient.SqlParameter() {
            New SqlClient.SqlParameter("@LoginUsuario", prnStrUserLogin),
            New SqlClient.SqlParameter("@Jerarquia", prmObjIdJerarquia)
        }
        Return ExecuteList("SP_VERIFICAR_JERARQUIA_USUARIO_POR_LOGIN", parametros).FirstOrDefault()
    End Function

End Class

