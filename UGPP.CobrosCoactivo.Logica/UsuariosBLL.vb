Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class UsuariosBLL
    Private Property _usuarioDAL As UsuarioDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _usuarioDAL = New UsuarioDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _usuarioDAL = New UsuarioDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TIPOS_CAUSALES_PRIORIZACION a Entidades.TiposCausalesPriorizacion
    ''' </summary>
    ''' <param name="prmObjUsuarioDatos">Objeto de tipo Datos.TIPOS_CAUSALES_PRIORIZACION</param>
    ''' <returns>Objeto de tipo Entidades.TiposCausalesPriorizacion</returns>
    Public Function ConvertirAEntidadUsuario(ByVal prmObjUsuarioDatos As Datos.USUARIOS) As Entidades.Usuario
        Dim cuasalPriorizacin As Entidades.Usuario
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.Usuario, Datos.USUARIOS)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        cuasalPriorizacin = IMapper.Map(Of Datos.USUARIOS, Entidades.Usuario)(prmObjUsuarioDatos)
        Return cuasalPriorizacin
    End Function

    ''' <summary>
    ''' Retorna la lista de todos los usuarios de la tabla aplicación
    ''' </summary>
    ''' <returns>Lista de objetos Entidades.Usuario</returns>
    Public Function ConsultarUsuarios() As List(Of Usuario)
        Return _usuarioDAL.ConsultarUsuarios()
    End Function

    ''' <summary>
    ''' Obtiene un usuario por su ID 
    ''' </summary>
    ''' <param name="prmStrUsuarioId">Id del usuario</param>
    ''' <returns>Objeto tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuarioPorId(ByVal prmStrUsuarioId As String) As Entidades.Usuario
        Dim usuarioConsulta = _usuarioDAL.obtenerUsuarioPorId(prmStrUsuarioId)
        If IsNothing(usuarioConsulta) Then
            Return New Entidades.Usuario
        End If
        Return ConvertirAEntidadUsuario(usuarioConsulta)
    End Function

    ''' <summary>
    ''' Obtiene datos de un usuario por su login 
    ''' </summary>
    ''' <param name="prmStrLoginUsuario">Login del usuario</param>
    ''' <returns>Objeto tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuarioPorLogin(ByVal prmStrLoginUsuario As String) As Entidades.Usuario
        Dim usuarioConsulta = _usuarioDAL.obtenerUsuarioPorLogin(prmStrLoginUsuario)
        If IsNothing(usuarioConsulta) Then
            Return New Entidades.Usuario
        End If
        Return ConvertirAEntidadUsuario(usuarioConsulta)
    End Function

    ''' <summary>
    ''' Retorna el usuario superior de un usuario de la aplicación
    ''' </summary>
    ''' <param name="prmStrUsuarioId">Id del usuario que se quiere su superior</param>
    ''' <returns>Usuario superior, objeto del tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuarioSuperior(ByVal prmStrUsuarioId As String) As Entidades.Usuario
        Dim usuarioConsulta = _usuarioDAL.obtenerUsuarioSuperior(prmStrUsuarioId)
        If IsNothing(usuarioConsulta.codigo) Then
            Return New Entidades.Usuario
        End If
        Return ConvertirAEntidadUsuario(usuarioConsulta)
    End Function

    ''' <summary>
    ''' Retorna la lista de usuarios que pueden gestionar estudio de títulos o expedientes
    ''' </summary>
    ''' <param name="prmIntTipoGestor">{1: Gestores estudio de títulos; 2: Gestores expedientes}</param>
    ''' <returns>Lista de objetos del tipo Datos.USUARIOS</returns>
    Public Function ObtenerUsuariosReasignacion(ByVal prmIntTipoGestor As Integer) As List(Of Entidades.Usuario)
        Dim usuarios As New List(Of Entidades.Usuario)
        Dim usuariosConsulta As List(Of Datos.USUARIOS) = _usuarioDAL.ObtenerUsuariosReasignacion(prmIntTipoGestor)
        If IsNothing(usuariosConsulta) Then
            Return usuarios
        End If
        For Each usuario As Datos.USUARIOS In usuariosConsulta
            usuarios.Add(ConvertirAEntidadUsuario(usuario))
        Next
        Return usuarios
    End Function

    ''' <summary>
    ''' Retorna la lista de usuarios asignados a un usuario superior
    ''' </summary>
    ''' <param name="prmStrCodUsuarioSuperior">Código del usuario superior</param>
    ''' <returns>Lista de objetos del tipo Entidades.Usuario</returns>
    Public Function obtenerUsuariosPorSuperior(ByVal prmStrCodUsuarioSuperior As String) As List(Of Entidades.Usuario)
        Dim usuarios As New List(Of Entidades.Usuario)
        Dim usuariosConsulta As List(Of Datos.USUARIOS) = _usuarioDAL.obtenerUsuariosPorSuperior(prmStrCodUsuarioSuperior)
        If IsNothing(usuariosConsulta) Then
            Return usuarios
        End If
        For Each usuario As Datos.USUARIOS In usuariosConsulta
            usuarios.Add(ConvertirAEntidadUsuario(usuario))
        Next
        Return usuarios
    End Function

    ''' <summary>
    ''' Retorna la lista de usuarios asignados a un usuario superior
    ''' </summary>
    ''' <param name="prmStrCodUsuarioSuperior">Código del usuario superior del Superior</param>
    ''' <returns>Lista de objetos del tipo Datos.USUARIOS</returns>
    Public Function obtenerUsuariosPorSuperiorDelSuperior(ByVal prmStrCodUsuarioSuperior As String) As List(Of Entidades.Usuario)
        Dim usuarios As New List(Of Entidades.Usuario)
        Dim usuariosConsulta As List(Of Datos.USUARIOS) = _usuarioDAL.obtenerUsuariosPorSuperiorDelSuperior(prmStrCodUsuarioSuperior)
        If IsNothing(usuariosConsulta) Then
            Return usuarios
        End If
        For Each usuario As Datos.USUARIOS In usuariosConsulta
            usuarios.Add(ConvertirAEntidadUsuario(usuario))
        Next
        Return usuarios
    End Function

    ''' <summary>
    ''' Retorna la lista de todos los usuarios de la tabla aplicación
    ''' </summary>
    ''' <param name="prmStrLoginUsuario">Login del usuario que se desea verificar</param>
    ''' <returns>Lista de objetos Entidades.Usuario</returns>
    Public Function VerificarSiUsuarioPerteneceATercerNivel(ByVal prmStrLoginUsuario) As List(Of Usuario)
        Return _usuarioDAL.VerificarSiUsuarioPerteneceATercerNivel(prmStrLoginUsuario)
    End Function

    ''' <summary>
    ''' Obtiene la lista de todos los gestores de estudio de títulos y expedientes
    ''' </summary>
    ''' <returns>Lista de objetos Entidades.Usuario</returns>
    Public Function ObtenerGestoresTodos() As List(Of Entidades.Usuario)
        Dim usuarios As New List(Of Entidades.Usuario)
        Dim usuariosConsulta As List(Of Datos.USUARIOS) = _usuarioDAL.ObtenerGestoresTodos()
        If IsNothing(usuariosConsulta) Then
            Return usuarios
        End If
        For Each usuario As Datos.USUARIOS In usuariosConsulta
            usuarios.Add(ConvertirAEntidadUsuario(usuario))
        Next
        Return usuarios
    End Function

    ''' <summary>
    ''' Verifica si el login que se pasa como parametro pertenece a la jerarquía que se pasa como parámetro
    ''' </summary>
    ''' <param name="prnStrUserLogin">Login del usuario</param>
    ''' <param name="prmObjIdJerarquia">Identificador de la jerarquía</param>
    ''' <returns>Objeto del tipo Entidades.Usuario</returns>
    Public Function VerificarJerarquiaUsuarioPorLogin(ByVal prnStrUserLogin As String, ByVal prmObjIdJerarquia As Enumeraciones.JerarquiaUsuario) As Usuario
        Return _usuarioDAL.VerificarJerarquiaUsuarioPorLogin(prnStrUserLogin, prmObjIdJerarquia)
    End Function
End Class

