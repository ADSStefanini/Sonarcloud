Imports System.Configuration
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades
Public Class PerfilDAL
    Inherits AccesObject(Of UGPP.CobrosCoactivo.Entidades.Perfiles)

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
    ''' Servicio para obtener las lista de perfiles activos en la aplicación
    ''' </summary>
    ''' <returns>Lista de perfiles</returns>
    Public Function obtenerPerfiles(ByVal prmStrTextoBusqueda As String) As List(Of Datos.PERFILES)
        Dim perfiles
        If (String.IsNullOrEmpty(prmStrTextoBusqueda) = False) Then
            Dim modulosFiltrados = (From p In db.PERFILES
                                    Where p.nombre.IndexOf(prmStrTextoBusqueda) >= 0
                                    Order By p.nombre
                                    Select p).ToList()
            Return modulosFiltrados
        Else
            perfiles = (From p In db.PERFILES
                        Order By p.nombre
                        Select p).ToList()
        End If

        Return perfiles

    End Function

    ''' <summary>
    ''' Retorna lista de perfiles activos en la aplicación
    ''' </summary>
    ''' <returns>Lista de perfiles activos</returns>
    Public Function obtenerPerfilesActivos() As List(Of Datos.PERFILES)
        Dim perfiles = (From p In db.PERFILES
                        Where p.ind_estado.Equals(True)
                        Order By p.nombre
                        Select p).ToList()
        Return perfiles
    End Function

    ''' <summary>
    ''' Obtener un perfil dependiendo del ID de 
    ''' </summary>
    ''' <param name="prmIntIdPerfil">Identificador del perfil que se quiere obtener los datos</param>
    ''' <returns>Objeto del tipo UGPP.CobrosCoactivo.Datos.PERFILES</returns>
    Public Function obternetPerfilPorId(ByVal prmIntIdPerfil As Long) As Datos.PERFILES
        Dim perfil = (From p In db.PERFILES
                      Where p.codigo = prmIntIdPerfil
                      Select p).FirstOrDefault()
        Return perfil
    End Function

    ''' <summary>
    ''' Función que crea un nuevo perfil en la aplicación
    ''' </summary>
    ''' <param name="prmStrNombrePerfil">Nombre del perfil</param>
    ''' <param name="prmStrGrupoLdap">Grupo LDAP del perfil</param>
    ''' <param name="prmBoolEstado">Activo o Inactivo</param>
    ''' <returns></returns>
    Public Function guardarPerfil(ByVal prmStrNombrePerfil As String, ByVal prmStrGrupoLdap As String, ByVal prmBoolEstado As Boolean) As Boolean
        Dim perfil As UGPP.CobrosCoactivo.Datos.PERFILES = New UGPP.CobrosCoactivo.Datos.PERFILES()
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        perfil.nombre = prmStrNombrePerfil
        perfil.val_ldap_group = prmStrGrupoLdap
        perfil.ind_estado = prmBoolEstado
        Parameters.Add(New SqlClient.SqlParameter("@nombre", prmStrNombrePerfil))
        Parameters.Add(New SqlClient.SqlParameter("@val_ldap_group", prmStrGrupoLdap))
        Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstado))
        db.PERFILES.Add(perfil)
        Utils.ValidaLog(_auditLog, "INSERT PERFILES ", Parameters.ToArray)
        Return Utils.salvarDatos(db)
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
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        Dim perfil As Datos.PERFILES = obternetPerfilPorId(prmLongCodigo)
        perfil.nombre = prmStrNombrePerfil
        perfil.val_ldap_group = prmStrGrupoLdap
        perfil.ind_estado = prmBoolEstado
        Parameters.Add(New SqlClient.SqlParameter("@nombre", prmStrNombrePerfil))
        Parameters.Add(New SqlClient.SqlParameter("@val_ldap_group", prmStrGrupoLdap))
        Parameters.Add(New SqlClient.SqlParameter("@ind_estado", prmBoolEstado))
        Utils.ValidaLog(_auditLog, "UPDATE PERFILES ", Parameters.ToArray)
        Return Utils.salvarDatos(db)
    End Function

End Class
