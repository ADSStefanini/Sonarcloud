Imports System.Configuration
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Public Class PaginaDAL
    Inherits AccesObject(Of Entidades.Pagina)

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
    ''' Obtener todas las páginas en el aplicativo
    ''' </summary>
    ''' <returns>Lista de Datos.PAGINA</returns>
    Public Function obtenerPaginasTodas(Optional ByVal prmStrTextoBusqueda As String = "") As List(Of Datos.PAGINA)
        Dim paginas
        If (String.IsNullOrEmpty(prmStrTextoBusqueda) = False) Then
            Dim paginasFiltradas = (From p In db.PAGINA
                                    Where p.val_nombre.IndexOf(prmStrTextoBusqueda) >= 0
                                    Order By p.pk_codigo, p.fk_padre
                                    Select p).ToList()
            Return paginasFiltradas
        Else
            paginas = (From p In db.PAGINA
                       Order By p.pk_codigo, p.fk_padre
                       Select p).ToList()
            'Return paginas
        End If

        Return paginas
    End Function

    ''' <summary>
    ''' Obtener todas las páginas activas en el aplicativo
    ''' </summary>
    ''' <returns>Lista de Datos.PAGINA</returns>
    Public Function obtenerPaginasActivas() As List(Of Datos.PAGINA)
        Dim paginas = (From p In db.PAGINA
                       Where p.ind_estado = True
                       Select p).ToList()
        Return paginas
    End Function

    ''' <summary>
    ''' Obtener una página dependiendo del ID de 
    ''' </summary>
    ''' <param name="prmIntPaginaId">Identificador de la página que se quiere obtener los datos</param>
    ''' <returns>Objeto del tipo Datos.PAGINA</returns>
    Public Function obtenerPaginaPorId(ByVal prmIntPaginaId As Long) As Datos.PAGINA
        Dim pagina = (From p In db.PAGINA
                      Where p.pk_codigo = prmIntPaginaId
                      Select p).FirstOrDefault()
        Return pagina
    End Function

    ''' <summary>
    ''' Función que crea una nueva página en la aplicación
    ''' </summary>
    ''' <param name="prmStrNombrePagina">Nombre de la página</param>
    ''' <param name="prmStrUrl">Url que lleva la página, opcional, vacia para las paginas que fucnionan como padres</param>
    ''' <param name="prmIntPadre">Página superior de la página que se está creando</param>
    ''' <param name="prmBoolEstado">Activo o Inactivo</param>
    ''' <returns>Objeto del tipo Datos.PAGINA</returns>
    Public Function guardarPagina(ByVal prmStrNombrePagina As String, ByVal prmStrUrl As String, ByVal prmIntPadre As Nullable(Of Integer), ByVal prmBoolEstado As Boolean) As Datos.PAGINA
        Dim pagina As Datos.PAGINA = New Datos.PAGINA()
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        pagina.val_nombre = prmStrNombrePagina
        If (String.IsNullOrEmpty(prmStrUrl) = False) Then
            pagina.val_url = prmStrUrl
            Parameters.Add(New SqlClient.SqlParameter("@val_url", prmStrUrl))
        End If
        If (prmIntPadre > 0) Then
            pagina.fk_padre = prmIntPadre
            Parameters.Add(New SqlClient.SqlParameter("@fk_padre", prmIntPadre))
        End If
        pagina.ind_estado = prmBoolEstado
        Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstado))
        db.PAGINA.Add(pagina)
        Utils.ValidaLog(_auditLog, "INSERT PAGINA ", Parameters.ToArray)
        Return If(Utils.salvarDatos(db), pagina, New Datos.PAGINA())
    End Function

    ''' <summary>
    ''' Función que actualizar una página en la aplicación
    ''' </summary>
    ''' <param name="prmLongPaginaId">Identificador de la página</param>
    ''' <param name="prmStrNombrePagina">Nombre de la página</param>
    ''' <param name="prmStrUrl">Url que lleva la página, opcional, vacia para las paginas que fucnionan como padres</param>
    ''' <param name="prmIntPadre">Página superior de la página que se está creando</param>
    ''' <param name="prmBoolEstado">Activo o Inactivo</param>
    ''' <returns>Objeto del tipo Datos.PAGINA</returns>
    Public Function actualizarPerfil(ByVal prmLongPaginaId As Long, ByVal prmStrNombrePagina As String, ByVal prmStrUrl As String, ByVal prmIntPadre As Nullable(Of Integer), ByVal prmBoolEstado As Boolean) As Datos.PAGINA
        Dim pagina As Datos.PAGINA = obtenerPaginaPorId(prmLongPaginaId)
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        pagina.val_nombre = prmStrNombrePagina
        pagina.val_url = prmStrUrl
        pagina.fk_padre = If(prmIntPadre > 0, prmIntPadre, Nothing)
        pagina.ind_estado = prmBoolEstado
        Parameters.Add(New SqlClient.SqlParameter("@val_nombre", prmStrNombrePagina))
        Parameters.Add(New SqlClient.SqlParameter("@val_url", prmStrUrl))
        Parameters.Add(New SqlClient.SqlParameter("@fk_padre", prmIntPadre))
        Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstado))
        Utils.ValidaLog(_auditLog, "UPDATE PAGINA ", Parameters.ToArray)
        Return If(Utils.salvarDatos(db), pagina, New Datos.PAGINA())
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfil(ByVal prmIntPerfil As Int32, ByVal prmIntPaginaPadre As Int32) As List(Of Entidades.Pagina)
        Dim parametros As SqlParameter() = {
            New SqlParameter With {
                .ParameterName = "@ID_PERFIL",
                .DbType = DbType.Int32,
                .Value = prmIntPerfil
            },
            New SqlParameter With {
                .ParameterName = "@ID_PARENTPAGE",
                .DbType = DbType.Int32,
                .Value = prmIntPaginaPadre
            }
        }
        Return ExecuteList("SP_ObtenerPaginasPorPerfil", parametros)
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfil(ByVal prmIntPerfil As Int32) As List(Of Datos.PAGINA)
        Dim paginas = (From p In db.PAGINA
                       Where p.ind_estado.Equals(True)
                       Where p.fk_padre = prmIntPerfil
                       Select p).ToList()
        Return paginas
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfilPuedeVer(ByVal prmIntPerfil As Int32) As List(Of Datos.PAGINA)
        Dim paginas = (From p In db.PAGINA
                       Join pp In db.PERFIL_PAGINA On p.pk_codigo Equals pp.fk_pagina_id
                       Where p.ind_estado = True
                       Where pp.fk_perfil_id = prmIntPerfil
                       Where pp.ind_puede_ver = True
                       Select p).ToList()
        Return paginas
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfilPuedeEditar(ByVal prmIntPerfil As Int32) As List(Of Datos.PAGINA)
        Dim paginas = (From p In db.PAGINA
                       Join pp In db.PERFIL_PAGINA On p.pk_codigo Equals pp.fk_pagina_id
                       Where p.ind_estado = True
                       Where pp.fk_perfil_id = prmIntPerfil
                       Where pp.ind_puede_editar = True
                       Select p).ToList()
        Return paginas
    End Function

    ''' <summary>
    ''' Retorna las páginas principales de la aplicación 
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Datos.PAGINA</returns>
    Public Function obtenerPaginasPadres() As List(Of Datos.PAGINA)
        Dim paginas = (From p In db.PAGINA
                       Where p.fk_padre Is Nothing
                       Where p.ind_pagina_interna.Equals(False)
                       Where p.ind_estado.Equals(True)
                       Select p).ToList()
        Return paginas
    End Function

    ''' <summary>
    ''' Retorna las páginas hijas de la pagina padre 
    ''' </summary>
    ''' <param name="prmIntPageId">ID página padre</param>
    ''' <returns>Lista de objetos del tipo Datos.PAGINA</returns>
    Public Function obtenerPaginasHijas(ByVal prmIntPageId As Int32) As List(Of Datos.PAGINA)
        Dim paginas = (From p In db.PAGINA
                       Where p.fk_padre = prmIntPageId
                       Where p.ind_estado.Equals(True)
                       Select p).ToList()
        Return paginas
    End Function


    Public Function obtenerPaginasHijas(ByVal prmIntPaginaPadre As Int32, Optional ByVal prmBoolSoloActivas As Boolean = True) As List(Of Datos.PAGINA)
        Dim paginas As List(Of Datos.PAGINA)
        If prmBoolSoloActivas Then
            paginas = (From p In db.PAGINA
                       Where p.ind_estado.Equals(True)
                       Where p.fk_padre.Equals(prmIntPaginaPadre)
                       Select p).ToList()
            Return paginas
        End If
        paginas = (From p In db.PAGINA
                   Where p.fk_padre = prmIntPaginaPadre
                   Select p).ToList()
        Return paginas
    End Function

    ''' <summary>
    ''' Actualiza el estado de acceso de una página de lectura y escritur a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfilId">ID del perfil</param>
    ''' <param name="prmIntPaginaId">Id de la página</param>
    ''' <param name="prmBoolPuedeVer">Puede ver Activo - Inactivo</param>
    ''' <param name="prmBoolPuedeEditar">Puede Editar Activo - Inactivo</param>
    ''' <returns></returns>
    Public Function actualizarAccesoPagina(ByVal prmIntPerfilId As Int32, ByVal prmIntPaginaId As Int32, ByVal prmBoolPuedeVer As Boolean, ByVal prmBoolPuedeEditar As Boolean) As Datos.PERFIL_PAGINA
        Dim perfilPagina = (From pp In db.PERFIL_PAGINA
                            Where pp.fk_perfil_id.Equals(prmIntPerfilId)
                            Where pp.fk_pagina_id.Equals(prmIntPaginaId)
                            Select pp).FirstOrDefault()
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        If IsNothing(perfilPagina) Then
            perfilPagina = New PERFIL_PAGINA
            perfilPagina.fk_perfil_id = prmIntPerfilId
            perfilPagina.fk_pagina_id = prmIntPaginaId
            perfilPagina.ind_puede_ver = prmBoolPuedeVer
            perfilPagina.ind_puede_editar = prmBoolPuedeEditar
            db.PERFIL_PAGINA.Add(perfilPagina)
            Parameters.Add(New SqlClient.SqlParameter("@fk_perfil_id", prmIntPerfilId))
            Parameters.Add(New SqlClient.SqlParameter("@fk_pagina_id", prmIntPaginaId))
            Parameters.Add(New SqlClient.SqlParameter("@ind_puede_ver", prmBoolPuedeVer))
            Parameters.Add(New SqlClient.SqlParameter("@ind_puede_editar", prmBoolPuedeEditar))
        Else
            perfilPagina.ind_puede_ver = prmBoolPuedeVer
            perfilPagina.ind_puede_editar = prmBoolPuedeEditar
            Parameters.Add(New SqlClient.SqlParameter("@ind_puede_ver", prmBoolPuedeVer))
            Parameters.Add(New SqlClient.SqlParameter("@ind_puede_editar", prmBoolPuedeEditar))
        End If
        Utils.ValidaLog(_auditLog, "UPDATE PERFIL_PAGINA ", Parameters.ToArray)
        Utils.salvarDatos(db)
        Return perfilPagina
    End Function
End Class
