Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos

Public Class TareaObservacionBLL
    Private Property _tareaObservacionDAL As TareaObservacionDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _tareaObservacionDAL = New TareaObservacionDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _tareaObservacionDAL = New TareaObservacionDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TAREA_OBSERVACION a Entidades.TareaObservacion
    ''' </summary>
    ''' <param name="prmObjTareaObervacionDatos">Objeto de tipo Datos.TAREA_OBSERVACION</param>
    ''' <returns>Objeto de tipo Entidades.TareaObservacion</returns>
    Public Function ConvertirAEntidadTareaObervacion(ByVal prmObjTareaObervacionDatos As Datos.TAREA_OBSERVACION) As Entidades.TareaObservacion
        Dim tareaObervacion As Entidades.TareaObservacion
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TareaObservacion, Datos.TAREA_OBSERVACION)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        tareaObervacion = IMapper.Map(Of Datos.TAREA_OBSERVACION, Entidades.TareaObservacion)(prmObjTareaObervacionDatos)
        Return tareaObervacion
    End Function

    ''' <summary>
    ''' Metódo que agrega un nuevo registro en la tabla TAREA_ONSERVACION
    ''' </summary>
    ''' <param name="prmObjTareaObservacionEntidad">Objeto de tipo Entidades.TareaObservacion</param>
    ''' <returns>Objeto de tipo Datos.TAREA_OBSERVACION</returns>
    Public Function crearTareaObservacion(ByVal prmObjTareaObservacionEntidad As Entidades.TareaObservacion) As Entidades.TareaObservacion
        Return ConvertirAEntidadTareaObervacion(_tareaObservacionDAL.crearTareaObservacion(prmObjTareaObservacionEntidad))
    End Function

    ''' <summary>
    ''' Obtiene una observacion de una tarea por su ID
    ''' </summary>
    ''' <param name="prmIntITareaObservacion">Identificador de la observación</param>
    ''' <returns>Objeto del tipo Entidades.TareaObservacion</returns>
    Public Function obtenerTareaObservacionPorId(ByVal prmIntITareaObservacion As Int32) As Entidades.TareaObservacion
        Return ConvertirAEntidadTareaObervacion(_tareaObservacionDAL.obtenerTareaObservacionPorId(prmIntITareaObservacion))
    End Function
End Class
