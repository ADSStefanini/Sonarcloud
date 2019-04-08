Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class PerfilBLL

    ''' <summary>
    ''' Clase de comunicaión para la conexión a la DB
    ''' </summary>
    Dim perfilDAL As PerfilDAL
    Private Property _AuditEntity As Entidades.LogAuditoria


    ''' <summary>
    ''' Control y CRUD de la entidad Perfiles
    ''' </summary>
    Public Sub New()
        perfilDAL = New PerfilDAL()
    End Sub
    ''' <param name="auditData"></param>
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        perfilDAL = New PerfilDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.PERFILES a Entidades.Perfiles
    ''' </summary>
    ''' <param name="prmObjPerfilEntidadDatos">Objeto de tipo Datos.PERFILES</param>
    ''' <returns>Objeto de tipo Entidades.Perfiles</returns>
    Public Function ConvertirEntidadPerfil(ByVal prmObjPerfilEntidadDatos As Datos.PERFILES) As Entidades.Perfiles
        Dim p As Entidades.Perfiles = New Entidades.Perfiles()
        p.codigo = prmObjPerfilEntidadDatos.codigo
        p.nombre = prmObjPerfilEntidadDatos.codigo
        p.nombre_perfil = prmObjPerfilEntidadDatos.nombre
        p.val_ldap_group = prmObjPerfilEntidadDatos.val_ldap_group
        p.ind_estado = Convert.ToInt32(prmObjPerfilEntidadDatos.ind_estado)
        'p.estado = prmObjPerfilEntidadDatos.
        Return p
    End Function

    ''' <summary>
    ''' Obtener todos los perfiles guardados en la base de datos
    ''' </summary>
    ''' <returns>Lista de objeto del tipo UGPP.CobrosCoactivo.Entidades.Perfiles</returns>
    Public Function obtenerPerfilesToodos(ByVal prmStrTextoBusqueda As String) As List(Of Entidades.Perfiles)
        Dim perfilesConsulta = perfilDAL.obtenerPerfiles(prmStrTextoBusqueda)

        Dim perfiles As List(Of Entidades.Perfiles) = New List(Of Entidades.Perfiles)
        For Each perfil As Datos.PERFILES In perfilesConsulta
            perfiles.Add(ConvertirEntidadPerfil(perfil))
        Next

        Return perfiles
    End Function

    ''' <summary>
    ''' Retorna lista de perfiles activos en la aplicación
    ''' </summary>
    ''' <returns>Lista de perfiles activos</returns>
    Public Function obtenerPerfilesActivos() As List(Of Entidades.Perfiles)
        Dim perfilesConsulta = perfilDAL.obtenerPerfilesActivos()

        Dim perfiles As List(Of Entidades.Perfiles) = New List(Of Entidades.Perfiles)
        For Each perfil As Datos.PERFILES In perfilesConsulta
            perfiles.Add(ConvertirEntidadPerfil(perfil))
        Next

        Return perfiles
    End Function

    ''' <summary>
    ''' Obtener un perfil dependiendo del ID de 
    ''' </summary>
    ''' <param name="prmIntIdPerfil">Identificador del perfil que se quiere obtener los datos</param>
    ''' <returns>Objeto del tipo UGPP.CobrosCoactivo.Entidades.Perfiles</returns>
    Public Function obternetPerfilPorId(ByVal prmIntIdPerfil As Long) As Entidades.Perfiles
        Dim perfilConsulta = perfilDAL.obternetPerfilPorId(prmIntIdPerfil)
        Dim perfil As Entidades.Perfiles = New Entidades.Perfiles()
        If IsNothing(perfilConsulta) Then
            Return perfil
        End If
        perfil.codigo = perfilConsulta.codigo
        perfil.nombre_perfil = perfilConsulta.nombre
        perfil.val_ldap_group = perfilConsulta.val_ldap_group
        perfil.ind_estado = Convert.ToInt32(perfilConsulta.ind_estado)
        Return perfil
    End Function

    ''' <summary>
    ''' Crear un nuevo perfil en la aplicación
    ''' </summary>
    ''' <param name="prmStrNombrePerfil">Nombre del perfil</param>
    ''' <param name="prmStrGrupoLdap">Grupo LDAP del perfil</param>
    ''' <param name="prmBoolEstado">Activo o Inactivo</param>
    ''' <returns></returns>
    Public Function guardarPerfil(ByVal prmStrNombrePerfil As String, ByVal prmStrGrupoLdap As String, ByVal prmBoolEstado As Boolean) As Boolean
        Return perfilDAL.guardarPerfil(prmStrNombrePerfil, prmStrGrupoLdap, prmBoolEstado)
    End Function

    ''' <summary>
    ''' Actualizar un perfil a partir de u ID
    ''' </summary>
    ''' <param name="prmLongCodigo">ID del perfil a actualizar</param>
    ''' <param name="prmStrNombrePerfil">Nuevo nombre del perfil</param>
    ''' <param name="prmStrGrupoLdap">Nuevo grupo LDAP del perfil</param>
    ''' <param name="prmBoolEstado">Activo o Inactivo</param>
    ''' <returns></returns>
    Public Function actualizarPerfil(ByVal prmLongCodigo As Long, ByVal prmStrNombrePerfil As String, ByVal prmStrGrupoLdap As String, ByVal prmBoolEstado As Boolean) As Boolean
        Return perfilDAL.actualizarPerfil(prmLongCodigo, prmStrNombrePerfil, prmStrGrupoLdap, prmBoolEstado)
    End Function
End Class
