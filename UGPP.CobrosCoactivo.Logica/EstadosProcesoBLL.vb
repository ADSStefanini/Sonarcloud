Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class EstadosProcesoBLL
    ''' <summary>
    ''' Clase de comunicaión para la conexión a la DB
    ''' </summary>
    Dim _estadosProcesoDAL As EstadosProcesoDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _estadosProcesoDAL = New EstadosProcesoDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _estadosProcesoDAL = New EstadosProcesoDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.ESTADO_OPERATIVO a Entidades.EstadoOperativo
    ''' </summary>
    ''' <param name="prmObjModuloEstadosProcesoDatos">Objeto de tipo Datos.ESTADO_OPERATIVO</param>
    ''' <returns>Objeto de tipo Entidades.EstadoOperativo</returns>
    Public Function ConvertirEntidadEstadosProceso(ByVal prmObjModuloEstadosProcesoDatos As Datos.ESTADOS_PROCESO) As Entidades.EstadosProceso
        Dim estadosProceso As Entidades.EstadosProceso
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.EstadosProceso, Datos.ESTADOS_PROCESO)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        estadosProceso = IMapper.Map(Of Datos.ESTADOS_PROCESO, Entidades.EstadosProceso)(prmObjModuloEstadosProcesoDatos)
        Return estadosProceso
    End Function

    ''' <summary>
    ''' Retorna la lista de los estados procesales
    ''' </summary>
    ''' <returns>Lista de objetos Datos.ESTADOS_PROCESO</returns>
    Public Function obtenerEstadosProcesos() As List(Of Entidades.EstadosProceso)
        Dim estadosProcesoConsulta = _estadosProcesoDAL.obtenerEstadosProcesos()
        Dim estadosProcesos As List(Of Entidades.EstadosProceso) = New List(Of Entidades.EstadosProceso)

        For Each estadosProceso As Datos.ESTADOS_PROCESO In estadosProcesoConsulta
            estadosProcesos.Add(ConvertirEntidadEstadosProceso(estadosProceso))
        Next

        Return estadosProcesos

    End Function
End Class
