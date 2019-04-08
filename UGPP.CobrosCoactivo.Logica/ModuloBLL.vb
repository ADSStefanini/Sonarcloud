Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class ModuloBLL

    Private Property _moduloDAL As ModuloDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _moduloDAL = New ModuloDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _moduloDAL = New ModuloDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.MODULO a Entidades.Modulo
    ''' </summary>
    ''' <param name="prmObjModuloEntidadDatos">Objeto de tipo Datos.MODULO</param>
    ''' <returns>Objeto de tipo Entidades.Modulo</returns>
    Public Function ConvertirEntidadModulo(ByVal prmObjModuloEntidadDatos As Datos.MODULO) As Entidades.Modulo
        Dim m As Entidades.Modulo = New Entidades.Modulo()
        m.pk_codigo = prmObjModuloEntidadDatos.pk_codigo
        m.val_nombre = prmObjModuloEntidadDatos.val_nombre
        m.val_url = prmObjModuloEntidadDatos.val_url
        m.val_url_icono = prmObjModuloEntidadDatos.val_url_icono
        m.ind_estado = Convert.ToInt32(prmObjModuloEntidadDatos.ind_estado)
        Return m
    End Function

    ''' <summary>
    ''' Obtener todos las módulos activos en el aplicativo
    ''' </summary>
    ''' <returns>Lista de módulos</returns>
    Public Function obtenerModulos(ByVal prmStrTextoBusqueda As String) As List(Of Entidades.Modulo)
        Dim modulosConsulta = _moduloDAL.obtenerModulos(prmStrTextoBusqueda)

        Dim modulos As List(Of Entidades.Modulo) = New List(Of Entidades.Modulo)
        For Each modulo As Datos.MODULO In modulosConsulta
            modulos.Add(ConvertirEntidadModulo(modulo))
        Next

        Return modulos
    End Function

    ''' <summary>
    ''' Retorna lista de módulos activos
    ''' </summary>
    ''' <returns>Lista de objetos Entidades.Modulo</returns>
    Public Function obtenerModulosActivos() As List(Of Entidades.Modulo)
        Dim modulosConsulta = _moduloDAL.obtenerModulosActivos()

        Dim modulos As List(Of Entidades.Modulo) = New List(Of Entidades.Modulo)
        For Each modulo As Datos.MODULO In modulosConsulta
            modulos.Add(ConvertirEntidadModulo(modulo))
        Next

        Return modulos
    End Function

    ''' <summary>
    ''' Retorna un módulo dependiendo de si identificador
    ''' </summary>
    ''' <param name="prmIntIdModulo">Identificador del módulo</param>
    ''' <returns>Objeto del tipo Entidades.Modulo</returns>
    Public Function obtenerModuloPorId(ByVal prmIntIdModulo As Long) As Entidades.Modulo
        Dim moduloConsulta = _moduloDAL.obtenerModuloPorId(prmIntIdModulo)
        If IsNothing(moduloConsulta) Then
            Return New Entidades.Modulo()
        End If
        Return ConvertirEntidadModulo(moduloConsulta)
    End Function

    ''' <summary>
    ''' Crear módulo
    ''' </summary>
    ''' <param name="prmNombreModulo">Nombre del módulo</param>
    ''' <param name="prmUrlModulo">Url Principal del módulo</param>
    ''' <param name="prmUrlIconoModulo">Url del icono del módulo</param>
    ''' <param name="prmBoolEstado">Activo Inactivo</param>
    ''' <returns>Objeto del tipo Entidades.Modulo</returns>
    Public Function guardarModulo(ByVal prmNombreModulo As String, ByVal prmUrlModulo As String, ByVal prmUrlIconoModulo As String, ByVal prmBoolEstado As Boolean) As Entidades.Modulo
        Dim moduloConsulta = _moduloDAL.guardarModulo(prmNombreModulo, prmUrlModulo, prmUrlIconoModulo, prmBoolEstado)
        Return ConvertirEntidadModulo(moduloConsulta)
    End Function

    ''' <summary>
    ''' Actualizar módulo
    ''' </summary>
    ''' <param name="prmModuloId">Identificador del módulo</param>
    ''' <param name="prmNombreModulo">Nombre del módulo</param>
    ''' <param name="prmUrlModulo">Url Principal del módulo</param>
    ''' <param name="prmUrlIconoModulo">Url del icono del módulo</param>
    ''' <param name="prmBoolEstado">Activo Inactivo</param>
    ''' <returns>Objeto del tipo Entidades.Modulo</returns>
    Public Function actualizarModulo(ByVal prmModuloId As Int32, ByVal prmNombreModulo As String, ByVal prmUrlModulo As String, ByVal prmUrlIconoModulo As String, ByVal prmBoolEstado As Boolean) As Entidades.Modulo
        Dim moduloConsulta = _moduloDAL.actualizarModulo(prmModuloId, prmNombreModulo, prmUrlModulo, prmUrlIconoModulo, prmBoolEstado)
        Return ConvertirEntidadModulo(moduloConsulta)
    End Function

    ''' <summary>
    ''' Obtener los módulos asociados a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de módulos</returns>
    Public Function obtenerModulosPorPerfil(ByVal prmIntPerfil As Int32) As List(Of Entidades.Modulo)
        Dim modulosConsulta = _moduloDAL.obtenerModulosPorPerfil(prmIntPerfil)
        If IsNothing(modulosConsulta) Then
            Return New List(Of Entidades.Modulo)
        End If

        Dim modulos As List(Of Entidades.Modulo) = New List(Of Entidades.Modulo)
        For Each modulo As Datos.MODULO In modulosConsulta
            modulos.Add(ConvertirEntidadModulo(modulo))
        Next

        Return modulos
    End Function

    ''' <summary>
    ''' Actualiza el estado de acceso de un módulo a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfilId">ID del perfil</param>
    ''' <param name="prmIntModuloId">Id del módulo</param>
    ''' <param name="prmBoolEstadoAcceso">Activo - Inactivo</param>
    ''' <returns></returns>
    Public Function actualizarAccesoModulo(ByVal prmIntPerfilId As Int32, ByVal prmIntModuloId As Int32, ByVal prmBoolEstadoAcceso As Boolean) As Entidades.PerfilModulo
        Dim perfilModuloConsulta As Datos.PERFIL_MODULO = _moduloDAL.actualizarAccesoModulo(prmIntPerfilId, prmIntModuloId, prmBoolEstadoAcceso)

        Dim perfilModulo As Entidades.PerfilModulo = New Entidades.PerfilModulo
        perfilModulo.fk_perfil_id = perfilModuloConsulta.fk_perfil_id
        perfilModulo.fk_modulo_id = perfilModuloConsulta.fk_modulo_id
        perfilModulo.ind_estado = perfilModuloConsulta.ind_estado

        Return perfilModulo
    End Function
End Class
