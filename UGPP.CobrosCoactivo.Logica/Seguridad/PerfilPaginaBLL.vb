Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos

Public Class PerfilPaginaBLL
    Private Property _perfilPaginaDAL As PerfilPaginaDAL

    Public Sub New()
        _perfilPaginaDAL = New PerfilPaginaDAL()
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.ESTADO_OPERATIVO a Entidades.EstadoOperativo
    ''' </summary>
    ''' <param name="prmObjPerfilPaginaDatos">Objeto de tipo Datos.ESTADO_OPERATIVO</param>
    ''' <returns>Objeto de tipo Entidades.EstadoOperativo</returns>
    Public Function ConvertirEntidadEstadoOperativo(ByVal prmObjPerfilPaginaDatos As Datos.PERFIL_PAGINA) As Entidades.PerfilPagina
        Dim perfilPagina As Entidades.PerfilPagina = New Entidades.PerfilPagina()
        perfilPagina.fk_pagina_id = prmObjPerfilPaginaDatos.fk_pagina_id
        perfilPagina.fk_perfil_id = prmObjPerfilPaginaDatos.fk_perfil_id
        perfilPagina.ind_puede_ver = Convert.ToInt32(prmObjPerfilPaginaDatos.ind_puede_ver)
        perfilPagina.ind_puede_editar = Convert.ToInt32(prmObjPerfilPaginaDatos.ind_puede_editar)
        perfilPagina.ind_estado = Convert.ToInt32(prmObjPerfilPaginaDatos.ind_estado)
        Return perfilPagina
    End Function


    ''' <summary>
    ''' Obtener los permisos que tiene un perfil a una página en especifico
    ''' </summary>
    ''' <param name="prmIntIdPerfil">Id del perfil</param>
    ''' <param name="prmIntIdPagina">Id de la página</param>
    ''' <returns>Objeto del tipo Entidades.PerfilPagina</returns>
    Public Function obtenerPermisosPorPagina(ByVal prmIntIdPerfil As Long, ByVal prmIntIdPagina As Integer) As Entidades.PerfilPagina
        Dim perfilPaginaConsulta = _perfilPaginaDAL.obtenerPermisosPorPagina(prmIntIdPerfil, prmIntIdPagina)

        If IsNothing(perfilPaginaConsulta) Then
            Return New Entidades.PerfilPagina()
        End If

        Return ConvertirEntidadEstadoOperativo(perfilPaginaConsulta)
    End Function
End Class
