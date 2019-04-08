Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class EstadoOperativoBLL
    ''' <summary>
    ''' Objeto para llamar métodos de consulta a la base de datos
    ''' </summary>
    ''' <returns></returns>
    Private Property _estadoOperativoDAL As EstadoOperativoDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _estadoOperativoDAL = New EstadoOperativoDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _estadoOperativoDAL = New EstadoOperativoDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.ESTADO_OPERATIVO a Entidades.EstadoOperativo
    ''' </summary>
    ''' <param name="prmObjModuloEstadoOperativoDatos">Objeto de tipo Datos.ESTADO_OPERATIVO</param>
    ''' <returns>Objeto de tipo Entidades.EstadoOperativo</returns>
    Public Function ConvertirEntidadEstadoOperativo(ByVal prmObjModuloEstadoOperativoDatos As Datos.ESTADO_OPERATIVO) As Entidades.EstadoOperativo
        Dim estadoOperativo As Entidades.EstadoOperativo
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.EstadoOperativo, Datos.ESTADO_OPERATIVO)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        estadoOperativo = IMapper.Map(Of Datos.ESTADO_OPERATIVO, Entidades.EstadoOperativo)(prmObjModuloEstadoOperativoDatos)
        Return estadoOperativo
    End Function

    ''' <summary>
    ''' Obtiene los estados operativos activos relacionados con los títulos
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Entidades.ESTADO_OPERATIVO</returns>
    Public Function obtenerEstadosOperativosActivosTitulos() As List(Of Entidades.EstadoOperativo)
        Dim estadosOperativosConsulta = _estadoOperativoDAL.obtenerEstadosOperativosActivosTitulos()
        Dim estadosOperativos As List(Of Entidades.EstadoOperativo) = New List(Of Entidades.EstadoOperativo)
        For Each estadoOperativo As Datos.ESTADO_OPERATIVO In estadosOperativosConsulta
            estadosOperativos.Add(ConvertirEntidadEstadoOperativo(estadoOperativo))
        Next

        Return estadosOperativos
    End Function

End Class
