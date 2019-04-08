Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports UGPP.CobrosCoactivo.Datos

Public Class PaginaBLL

    ''' <summary>
    ''' Clase de comunicaión para la conexión a la DB
    ''' </summary>
    Private Property paginaDAL As PaginaDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        paginaDAL = New PaginaDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        paginaDAL = New PaginaDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.PAGINA a Entidades.Pagina
    ''' </summary>
    ''' <param name="prmObjPaginaEntidadDatos">Objeto de tipo Datos.PAGINA</param>
    ''' <returns>Objeto de tipo Entidades.Pagina</returns>
    Public Function ConvertirEntidadPagina(ByVal prmObjPaginaEntidadDatos As Datos.PAGINA, Optional ByVal prmIntLevel As Int32 = -1) As Entidades.Pagina
        Dim p As Entidades.Pagina = New Entidades.Pagina()
        p.pk_codigo = prmObjPaginaEntidadDatos.pk_codigo
        p.val_nombre = prmObjPaginaEntidadDatos.val_nombre
        p.val_url = prmObjPaginaEntidadDatos.val_url
        p.fk_padre = prmObjPaginaEntidadDatos.fk_padre
        p.ind_estado = Convert.ToInt32(prmObjPaginaEntidadDatos.ind_estado)

        p.ind_pagina_interna = prmObjPaginaEntidadDatos.ind_pagina_interna
        If (prmIntLevel > 0) Then
            p.ind_nivel = prmIntLevel
        End If
        Return p
    End Function

    Private Function CaseContains(ByVal baseString As String, ByVal textToSearch As String, ByVal comparisonMode As StringComparison) As Boolean
        Return (baseString.IndexOf(textToSearch, comparisonMode) <> -1)
    End Function


    ''' <summary>
    ''' Obtener todas las páginas en el aplicativo
    ''' </summary>
    ''' <returns>Lista de Entidades.Pagina</returns>
    Public Function obtenerPaginasTodas(ByVal prmStrTextoBusqueda As String) As List(Of Entidades.Pagina)
        If Not String.IsNullOrEmpty(prmStrTextoBusqueda) Then
            prmStrTextoBusqueda = UnAccent(prmStrTextoBusqueda.ToLower())
        End If

        Dim paginasConsulta = paginaDAL.obtenerPaginasTodas()

        Dim paginas As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)
        Dim parent As Int32 = 0
        Dim currentParent As Int32 = 0
        For Each pagina As Datos.PAGINA In paginasConsulta
            If (IsNothing(pagina.fk_padre)) Then
                parent = 0
            Else

                If (pagina.fk_padre <> currentParent) Then
                    currentParent = pagina.fk_padre
                ElseIf pagina.fk_padre > currentParent Then
                    parent += 1
                ElseIf pagina.fk_padre < currentParent Then
                    parent -= 1
                End If

            End If

            'If String.IsNullOrEmpty(prmStrTextoBusqueda) OrElse CaseContains(pagina.val_nombre, prmStrTextoBusqueda, StringComparison.CurrentCultureIgnoreCase) Then
            Dim pageName As String

            If Not String.IsNullOrEmpty(prmStrTextoBusqueda) Then
                pageName = UnAccent(pagina.val_nombre.ToLower())
            End If

            If String.IsNullOrEmpty(prmStrTextoBusqueda) OrElse pageName.Contains(prmStrTextoBusqueda) Then
                paginas.Add(ConvertirEntidadPagina(pagina, parent))
            End If
        Next

        Return paginas
    End Function

    Public Function UnAccent(ByVal aString As String) As String
        Dim toReplace() As Char = "àèìòùÀÈÌÒÙ äëïöüÄËÏÖÜ âêîôûÂÊÎÔÛ áéíóúÁÉÍÓÚðÐýÝ ãñõÃÑÕšŠžŽçÇåÅøØ".ToCharArray
        Dim replaceChars() As Char = "aeiouAEIOU aeiouAEIOU aeiouAEIOU aeiouAEIOUdDyY anoANOsSzZcCaAoO".ToCharArray
        For index As Integer = 0 To toReplace.GetUpperBound(0)
            aString = aString.Replace(toReplace(index), replaceChars(index))
        Next
        Return aString
    End Function

    ''' <summary>
    ''' Obtener todas las páginas activas en el aplicativo
    ''' </summary>
    ''' <returns>Lista de Entidades.Pagina</returns>
    Public Function obtenerPaginasActivas() As List(Of Entidades.Pagina)
        Dim paginasConsulta = paginaDAL.obtenerPaginasActivas()
        Dim paginas As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)
        For Each pagina As Datos.PAGINA In paginasConsulta
            paginas.Add(ConvertirEntidadPagina(pagina))
        Next

        Return paginas
    End Function

    ''' <summary>
    ''' Obtener una página dependiendo del ID de 
    ''' </summary>
    ''' <param name="prmIntPaginaId">Identificador de la página que se quiere obtener los datos</param>
    ''' <returns>Objeto del tipo UGPP.CobrosCoactivo.Entidades.Pagina</returns>
    Public Function obtenerPaginaPorId(prmIntPaginaId) As UGPP.CobrosCoactivo.Entidades.Pagina
        Dim paginaConsulta = paginaDAL.obtenerPaginaPorId(prmIntPaginaId)
        Dim pagina As Entidades.Pagina = New Entidades.Pagina
        If IsNothing(paginaConsulta) Then
            Return pagina
        End If

        Return ConvertirEntidadPagina(paginaConsulta)
    End Function

    ''' <summary>
    ''' Función que crea una nueva página en la aplicación
    ''' </summary>
    ''' <param name="prmStrNombrePagina">Nombre de la página</param>
    ''' <param name="prmStrUrl">Url que lleva la página, opcional, vacia para las paginas que fucnionan como padres</param>
    ''' <param name="prmIntPadre">Página superior de la página que se está creando</param>
    ''' <param name="prmBoolEstado">Activo o Inactivo</param>
    ''' <returns>Objeto del tipo UGPP.CobrosCoactivo.Datos.PAGINA</returns>
    Public Function guardarPagina(ByVal prmStrNombrePagina As String, ByVal prmStrUrl As String, ByVal prmIntPadre As Nullable(Of Integer), ByVal prmBoolEstado As Boolean) As Entidades.Pagina
        Dim paginaConsulta = paginaDAL.guardarPagina(prmStrNombrePagina, prmStrUrl, prmIntPadre, prmBoolEstado)
        Dim pagina As Entidades.Pagina = New Entidades.Pagina()
        If (paginaConsulta.pk_codigo = 0) Then
            Return pagina
        End If

        Return ConvertirEntidadPagina(paginaConsulta)
    End Function

    ''' <summary>
    ''' Función que actualizar una página en la aplicación
    ''' </summary>
    ''' <param name="prmLongPaginaId">Identificador de la página</param>
    ''' <param name="prmStrNombrePagina">Nombre de la página</param>
    ''' <param name="prmStrUrl">Url que lleva la página, opcional, vacia para las paginas que fucnionan como padres</param>
    ''' <param name="prmIntPadre">Página superior de la página que se está creando</param>
    ''' <param name="prmBoolEstado">Activo o Inactivo</param>
    ''' <returns>Objeto del tipo UGPP.CobrosCoactivo.Datos.PAGINA</returns>
    Public Function actualizarPerfil(ByVal prmLongPaginaId As Long, ByVal prmStrNombrePagina As String, ByVal prmStrUrl As String, ByVal prmIntPadre As Nullable(Of Integer), ByVal prmBoolEstado As Boolean) As Entidades.Pagina
        Dim paginaConsulta = paginaDAL.actualizarPerfil(prmLongPaginaId, prmStrNombrePagina, prmStrUrl, prmIntPadre, prmBoolEstado)
        Dim pagina As Entidades.Pagina = New Entidades.Pagina()
        Return ConvertirEntidadPagina(paginaConsulta)
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfil(ByVal prmIntPerfil As Int32, ByVal prmIntPaginaPadre As Int32) As List(Of Entidades.Pagina)
        Return paginaDAL.obtenerPaginasPorPerfil(prmIntPerfil, prmIntPaginaPadre)
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfil(ByVal prmIntPerfil As Int32) As List(Of Entidades.Pagina)
        Dim paginasConsultas = paginaDAL.obtenerPaginasPorPerfil(prmIntPerfil)
        Dim paginas As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)

        For Each pagina As Datos.PAGINA In paginasConsultas
            paginas.Add(ConvertirEntidadPagina(pagina))
        Next
        Return paginas
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfilPuedeVer(ByVal prmIntPerfil As Int32) As List(Of Entidades.Pagina)
        Dim paginasConsultas = paginaDAL.obtenerPaginasPorPerfilPuedeVer(prmIntPerfil)
        Dim paginas As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)

        For Each pagina As Datos.PAGINA In paginasConsultas
            paginas.Add(ConvertirEntidadPagina(pagina))
        Next
        Return paginas
    End Function

    ''' <summary>
    ''' Obtener las páginas asociadas a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de páginas</returns>
    Public Function obtenerPaginasPorPerfilPuedeEditar(ByVal prmIntPerfil As Int32) As List(Of Entidades.Pagina)
        Dim paginasConsultas = paginaDAL.obtenerPaginasPorPerfilPuedeEditar(prmIntPerfil)
        Dim paginas As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)

        For Each pagina As Datos.PAGINA In paginasConsultas
            paginas.Add(ConvertirEntidadPagina(pagina))
        Next
        Return paginas
    End Function

    Public Function obtenerPaginasOrdenadas() As List(Of Entidades.Pagina)
        Dim padres As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)
        Dim padresConsulta = paginaDAL.obtenerPaginasPadres()
        For Each pagina As Datos.PAGINA In padresConsulta
            padres.Add(ConvertirEntidadPagina(pagina))
        Next

        Dim listaPaginas As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)

        For Each pagina As Entidades.Pagina In padres
            listaPaginas.Add(pagina)
            Dim hijas = obtenerPaginasHijas(pagina.pk_codigo, 1)
            If hijas.Count() > 0 Then
                listaPaginas.AddRange(hijas)
            End If
        Next
        Return listaPaginas
    End Function

    Private Function obtenerPaginasHijas(ByVal prmIntPageId As Int32, Optional ByVal prmIntNivel As Int32 = 0) As List(Of Entidades.Pagina)
        Dim hijas As List(Of Entidades.Pagina) = New List(Of Entidades.Pagina)
        Dim hijasConsulta = paginaDAL.obtenerPaginasHijas(prmIntPageId)
        If (hijasConsulta.Count() > 0) Then
            For Each pagina As Datos.PAGINA In hijasConsulta
                hijas.Add(ConvertirEntidadPagina(pagina, prmIntNivel))
                Dim hijasDeHijas = obtenerPaginasHijas(pagina.pk_codigo, prmIntNivel + 1)
                If hijasDeHijas.Count() > 0 Then
                    hijas.AddRange(hijasDeHijas)
                End If
            Next
        End If
        Return hijas
    End Function

    ''' <summary>
    ''' Actualiza el estado de acceso de una página de lectura y escritur a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfilId">ID del perfil</param>
    ''' <param name="prmIntPaginaId">Id de la página</param>
    ''' <param name="prmBoolPuedeVer">Puede ver Activo - Inactivo</param>
    ''' <param name="prmBoolPuedeEditar">Puede Editar Activo - Inactivo</param>
    ''' <returns></returns>
    Public Function actualizarAccesoPagina(ByVal prmIntPerfilId As Int32, ByVal prmIntPaginaId As Int32, ByVal prmBoolPuedeVer As Boolean, ByVal prmBoolPuedeEditar As Boolean) As Entidades.PerfilPagina
        Dim perfilPaginaConsulta As Datos.PERFIL_PAGINA = paginaDAL.actualizarAccesoPagina(prmIntPerfilId, prmIntPaginaId, prmBoolPuedeVer, prmBoolPuedeEditar)

        Dim perfilPagina As Entidades.PerfilPagina = New Entidades.PerfilPagina
        perfilPagina.fk_perfil_id = perfilPaginaConsulta.fk_perfil_id
        perfilPagina.fk_pagina_id = perfilPaginaConsulta.fk_pagina_id
        perfilPagina.ind_puede_ver = perfilPaginaConsulta.ind_puede_ver
        perfilPagina.ind_puede_editar = perfilPaginaConsulta.ind_puede_editar
        perfilPagina.ind_estado = 1
        Return perfilPagina
    End Function

End Class
