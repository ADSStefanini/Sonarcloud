Imports System.Configuration

Public Class PerfilPaginaDAL
    Inherits AccesObject(Of Entidades.PerfilPagina)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    ''' <summary>
    ''' Obtener los permisos que tiene un perfil a una página en especifico
    ''' </summary>
    ''' <param name="prmIntIdPerfil">Id del perfil</param>
    ''' <param name="prmIntIdPagina">Id de la página</param>
    ''' <returns>Objeto del tipo Datos.PERFIL_PAGINA</returns>
    Public Function obtenerPermisosPorPagina(ByVal prmIntIdPerfil As Int32, ByVal prmIntIdPagina As Int32) As Datos.PERFIL_PAGINA
        Dim perfilPagina = (From pg In db.PERFIL_PAGINA
                            Where pg.fk_perfil_id = prmIntIdPerfil
                            Where pg.fk_pagina_id = prmIntIdPagina
                            Select pg).FirstOrDefault()
        Return perfilPagina
    End Function

End Class
