Imports System.Web.Services
Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class seguridad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' Método web para consultar las páginas relacionadas con un perfil y las páginas hijas, utilizado para armar las pestañas
    ''' </summary>
    ''' <param name="paginasPorPerfil">Objeto del tipo Entidades/Request/PaginasPorPerfil.vb</param>
    ''' <returns>Lista de objetos Entidades.Pagina serializado en un objeto json</returns>
    <WebMethod>
    Public Shared Function getPages(ByVal paginasPorPerfil As PaginasPorPerfil) As String
        Dim paginaBLL As PaginaBLL = New PaginaBLL()
        Dim paginas As List(Of Pagina) = paginaBLL.obtenerPaginasPorPerfil(paginasPorPerfil.idPerfil, paginasPorPerfil.idPaginaPadre)
        Return JsonConvert.SerializeObject(paginas)
    End Function

    ''' <summary>
    ''' Método web para consultar los módulos relacionadas con un perfil
    ''' </summary>
    ''' <param name="prmIntPerfilId">Id del perfil </param>
    ''' <returns>Lista de objetos Entidades.Modulo serializado en un objeto json</returns>
    <WebMethod>
    Public Shared Function getModules(ByVal prmIntPerfilId As String) As String
        Dim moduloBLL As ModuloBLL = New ModuloBLL()
        Dim modulos As List(Of Entidades.Modulo) = moduloBLL.obtenerModulosPorPerfil(prmIntPerfilId)
        Return JsonConvert.SerializeObject(modulos)
    End Function

End Class
