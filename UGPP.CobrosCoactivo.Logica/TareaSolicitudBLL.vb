Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades
Public Class TareaSolicitudBLL

    ''' <summary>
    ''' Objeto para llamar métodos de consulta a la base de datos
    ''' </summary>
    ''' <returns></returns>
    Private Property _tareaSolicitudDAL As TareaSolicitudDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _tareaSolicitudDAL = New TareaSolicitudDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _tareaSolicitudDAL = New TareaSolicitudDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TIPOS_CAUSALES_PRIORIZACION a Entidades.TiposCausalesPriorizacion
    ''' </summary>
    ''' <param name="prmObjTareaSolicitudDatos">Objeto de tipo Datos.TIPOS_CAUSALES_PRIORIZACION</param>
    ''' <returns>Objeto de tipo Entidades.TiposCausalesPriorizacion</returns>
    Public Function ConvertirAEntidadTareaSolicitud(ByVal prmObjTareaSolicitudDatos As Datos.TAREA_SOLICITUD) As Entidades.TareaSolicitud
        Dim tareaSolicitud As Entidades.TareaSolicitud
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TareaSolicitud, Datos.TAREA_SOLICITUD)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        tareaSolicitud = IMapper.Map(Of Datos.TAREA_SOLICITUD, Entidades.TareaSolicitud)(prmObjTareaSolicitudDatos)
        Return tareaSolicitud
    End Function


    ''' <summary>
    ''' Agregar un nuevo registro a la tabla TAREA_SOLICITUD
    ''' </summary>
    ''' <param name="prmObjTareaSolicitud">Objeto del tipo Entidades.TareaSolicitud</param>
    ''' <returns>Objeto del tipo Datos.TAREA_SOLICITUD</returns>
    Public Function guardarTareaSolicitud(ByVal prmObjTareaSolicitud As Entidades.TareaSolicitud) As Entidades.TareaSolicitud
        Dim tareaSoicitud = _tareaSolicitudDAL.guardarTareaSolicitud(prmObjTareaSolicitud)
        Return ConvertirAEntidadTareaSolicitud(tareaSoicitud)
    End Function
    Public Function registrarTarea(tareaInsertar As TareaSolicitud) As TareaSolicitud
        Return _tareaSolicitudDAL.registrarTarea(tareaInsertar)
    End Function

    ''' <summary>
    ''' Busca si una tarea tiene una solicitud sin procesar del tipo tipo de solicitud pasado como parámetro  
    ''' </summary>
    ''' <param name="prmIntTareaSolicitud">ID TAREA_ASIGNADA</param>
    ''' <param name="prmIntTipoSolicitud">Identificador Tipo Solicitud {6: SolicitudSuspension; 8: SolicitudPriorizacion; 9: SolicitudResignacion}</param>
    ''' <returns>Objeto del tipo Entidades.TareaSolicitud</returns>
    Public Function obtenerTareaSolicitudPorTipoSolicitudNoProcesada(ByVal prmIntTareaSolicitud As Integer, ByVal prmIntTipoSolicitud As Int32) As Entidades.TareaSolicitud
        Return ConvertirAEntidadTareaSolicitud(_tareaSolicitudDAL.obtenerTareaSolicitudPorTipoSolicitudNoProcesada(prmIntTareaSolicitud, prmIntTipoSolicitud))
    End Function

    ''' <summary>
    ''' Obtener tarea solicitud por ID
    ''' </summary>
    ''' <param name="prmIntIdTareaSolicitud">Identificador de la tarea solicitud</param>
    ''' <returns>Objeto del tipo Datos.TAREA_SOLICITUD</returns>
    Public Function obternerTareaSolicitudPorId(ByVal prmIntIdTareaSolicitud As Int32) As Entidades.TareaSolicitud
        Return ConvertirAEntidadTareaSolicitud(_tareaSolicitudDAL.obternerTareaSolicitudPorId(prmIntIdTareaSolicitud))
    End Function

    ''' <summary>
    ''' Actualiza un registro de la una solicitud
    ''' </summary>
    ''' <param name="prmObjTareaSolicitud">Obgeto del tipo Entidades.TareaSolicitud</param>
    ''' <returns>Objeto del tipo Entidades.TareaSolicitud</returns>
    Public Function ActualizarTareaSolicitud(ByVal prmObjTareaSolicitud As Entidades.TareaSolicitud) As Entidades.TareaSolicitud
        Return ConvertirAEntidadTareaSolicitud(_tareaSolicitudDAL.ActualizarTareaSolicitud(prmObjTareaSolicitud))
    End Function

    ''' <summary>
    ''' Método que retorna el último ID registrado en la tabla
    ''' </summary>
    ''' <returns>Int => último ID</returns>
    Public Function getLastId() As Int32

    End Function
End Class
