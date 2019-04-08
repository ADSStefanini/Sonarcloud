Imports System.Configuration
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Public Class ModuloDAL
    Inherits AccesObject(Of UGPP.CobrosCoactivo.Entidades.Modulo)

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
    ''' Obtener todos las módulos activos en el aplicativo
    ''' </summary>
    ''' <param name="prmStrTextoBusqueda">Cadena de texto para filtrar por nombre</param>
    ''' <returns>Lista de objetos Datos.MODULO</returns>
    Public Function obtenerModulos(ByVal prmStrTextoBusqueda As String) As List(Of Datos.MODULO)
        Dim modulos
        If (String.IsNullOrEmpty(prmStrTextoBusqueda) = False) Then
            Dim modulosFiltrados = (From m In db.MODULO
                                    Where m.val_nombre.IndexOf(prmStrTextoBusqueda) >= 0
                                    Order By m.val_nombre
                                    Select m).ToList()
            Return modulosFiltrados
        Else
            modulos = (From m In db.MODULO
                       Order By m.val_nombre
                       Select m).ToList()
        End If

        Return modulos
    End Function

    ''' <summary>
    ''' Retorna un módulo dependiendo de si identificador
    ''' </summary>
    ''' <param name="prmIntIdModulo">Identificador del módulo</param>
    ''' <returns>Objeto del tipo Datos.MODULO</returns>
    Public Function obtenerModuloPorId(ByVal prmIntIdModulo As Long) As Datos.MODULO
        Dim modulo = (From m In db.MODULO
                      Where m.pk_codigo = prmIntIdModulo
                      Order By m.val_nombre
                      Select m).FirstOrDefault()
        Return modulo
    End Function

    ''' <summary>
    ''' Retorna lista de módulos activos
    ''' </summary>
    ''' <returns>Lista de objetos Datos.MODULO</returns>
    Public Function obtenerModulosActivos() As List(Of Datos.MODULO)
        Dim modulos = (From m In db.MODULO
                       Where m.ind_estado.Equals(True)
                       Order By m.val_nombre
                       Select m).ToList()
        Return modulos
    End Function

    ''' <summary>
    ''' Crear módulo
    ''' </summary>
    ''' <param name="prmNombreModulo">Nombre del módulo</param>
    ''' <param name="prmUrlModulo">Url Principal del módulo</param>
    ''' <param name="prmUrlIconoModulo">Url del icono del módulo</param>
    ''' <param name="prmBoolEstado">Activo Inactivo</param>
    ''' <returns>Objeto del tipo Datos.MODULO</returns>
    Public Function guardarModulo(ByVal prmNombreModulo As String, ByVal prmUrlModulo As String, ByVal prmUrlIconoModulo As String, ByVal prmBoolEstado As Boolean) As Datos.MODULO
        Dim modulo As Datos.MODULO = New Datos.MODULO()
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@val_nombre", prmNombreModulo))
        Parameters.Add(New SqlClient.SqlParameter("@val_url", prmUrlModulo))
        Parameters.Add(New SqlClient.SqlParameter("@val_url_icono", prmUrlIconoModulo))
        Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstado))
        modulo.val_nombre = prmNombreModulo
        modulo.val_url = prmUrlModulo
        modulo.val_url_icono = prmUrlIconoModulo
        modulo.ind_estado = prmBoolEstado
        db.MODULO.Add(modulo)
        Utils.salvarDatos(db)
        Utils.ValidaLog(_auditLog, "INSERT MODULO ", Parameters.ToArray)
        Return modulo
    End Function

    ''' <summary>
    ''' Actualizar módulo
    ''' </summary>
    ''' <param name="prmModuloId">Identificador del módulo</param>
    ''' <param name="prmNombreModulo">Nombre del módulo</param>
    ''' <param name="prmUrlModulo">Url Principal del módulo</param>
    ''' <param name="prmUrlIconoModulo">Url del icono del módulo</param>
    ''' <param name="prmBoolEstado">Activo Inactivo</param>
    ''' <returns>Objeto del tipo Datos.MODULO</returns>
    Public Function actualizarModulo(ByVal prmModuloId As Int32, ByVal prmNombreModulo As String, ByVal prmUrlModulo As String, ByVal prmUrlIconoModulo As String, ByVal prmBoolEstado As Boolean) As Datos.MODULO
        Dim modulo As Datos.MODULO = obtenerModuloPorId(prmModuloId)
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@val_nombre", prmNombreModulo))
        Parameters.Add(New SqlClient.SqlParameter("@val_url", prmUrlModulo))
        Parameters.Add(New SqlClient.SqlParameter("@val_url_icono", prmUrlIconoModulo))
        Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstado))
        modulo.val_nombre = prmNombreModulo
        modulo.val_url = prmUrlModulo
        modulo.val_url_icono = prmUrlIconoModulo
        modulo.ind_estado = prmBoolEstado
        Utils.salvarDatos(db)
        Utils.ValidaLog(_auditLog, "UPDATE MODULO ", Parameters.ToArray)
        Return modulo
    End Function

    ''' <summary>
    ''' Obtener los módulos asociados a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfil">ID del perfil</param>
    ''' <returns>Lista de módulos</returns>
    Public Function obtenerModulosPorPerfil(ByVal prmIntPerfil As Int32) As List(Of Datos.MODULO)
        Dim modulos = (From m In db.MODULO
                       Join pm In db.PERFIL_MODULO On m.pk_codigo Equals pm.fk_modulo_id
                       Join pf In db.PERFILES On pm.fk_perfil_id Equals pf.codigo
                       Where m.ind_estado.Equals(True)
                       Where pf.ind_estado.Equals(True)
                       Where pm.ind_estado.Equals(True)
                       Where pm.fk_perfil_id.Equals(prmIntPerfil)
                       Order By m.val_nombre
                       Select m).ToList()
        Return modulos
    End Function

    ''' <summary>
    ''' Actualiza el estado de acceso de un módulo a un perfil
    ''' </summary>
    ''' <param name="prmIntPerfilId">ID del perfil</param>
    ''' <param name="prmIntModuloId">Id del módulo</param>
    ''' <param name="prmBoolEstadoAcceso">Activo - Inactivo</param>
    ''' <returns></returns>
    Public Function actualizarAccesoModulo(ByVal prmIntPerfilId As Int32, ByVal prmIntModuloId As Int32, ByVal prmBoolEstadoAcceso As Boolean) As Datos.PERFIL_MODULO
        Dim perfilModulo = (From pm In db.PERFIL_MODULO
                            Where pm.fk_perfil_id.Equals(prmIntPerfilId)
                            Where pm.fk_modulo_id.Equals(prmIntModuloId)
                            Select pm).FirstOrDefault()
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()

        If IsNothing(perfilModulo) Then
            perfilModulo = New Datos.PERFIL_MODULO
            perfilModulo.fk_perfil_id = prmIntPerfilId
            perfilModulo.fk_modulo_id = prmIntModuloId
            perfilModulo.ind_estado = prmBoolEstadoAcceso
            db.PERFIL_MODULO.Add(perfilModulo)
            Parameters.Add(New SqlClient.SqlParameter("@fk_perfil_id", prmIntPerfilId))
            Parameters.Add(New SqlClient.SqlParameter("@fk_modulo_id", prmIntModuloId))
            Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstadoAcceso))
        Else
            perfilModulo.ind_estado = prmBoolEstadoAcceso
            Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstadoAcceso))
        End If
        Utils.ValidaLog(_auditLog, "UPDATE PERFIL_MODULO", Parameters.ToArray)
        Utils.salvarDatos(db)
        Return perfilModulo
    End Function
End Class
