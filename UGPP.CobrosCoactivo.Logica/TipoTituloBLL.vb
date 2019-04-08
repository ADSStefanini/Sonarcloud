Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports AutoMapper

Public Class TipoTituloBLL

    Private Property _TipoTituloDAL As TipoTituloDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _TipoTituloDAL = New TipoTituloDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _TipoTituloDAL = New TipoTituloDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TIPOS_TITULO a Entidades.TipoTitulo
    ''' </summary>
    ''' <param name="prmObjTipoTitulo">Objeto de tipo Datos.TIPOS_TITULO</param>
    ''' <returns>Objeto de tipo Entidades.TipoTitulo</returns>
    Private Function ConvertirAEntidadTipoTitulo(ByVal prmObjTipoTitulo As Datos.TIPOS_TITULO) As Entidades.TipoTitulo
        Dim tipoTitulo As Entidades.TipoTitulo
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TipoTitulo, Datos.TIPOS_TITULO)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        tipoTitulo = IMapper.Map(Of Datos.TIPOS_TITULO, Entidades.TipoTitulo)(prmObjTipoTitulo)
        Return tipoTitulo
    End Function

    Public Function consultarTipoTitulo(codTipoArea As Int32) As List(Of TipoTitulo)
        Return _TipoTituloDAL.consultarTipoTitulo(codTipoArea)
    End Function
End Class
