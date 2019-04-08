Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports AutoMapper

Public Class TareaAsignadaBLL
    Private Property _TareaAsignadaDAL As TareaAsignadaDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _TareaAsignadaDAL = New TareaAsignadaDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _TareaAsignadaDAL = New TareaAsignadaDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TAREA_ASIGNADA a Entidades.TareaAsignada
    ''' </summary>
    ''' <param name="prmObjTareaAsigandaDatos">Objeto de tipo Datos.TAREA_ASIGNADA</param>
    ''' <returns>Objeto de tipo Entidades.TareaAsignada</returns>
    Public Function ConvertirAEntidadTareaAsiganda(ByVal prmObjTareaAsigandaDatos As Datos.TAREA_ASIGNADA) As Entidades.TareaAsignada
        Dim tareaAsiganada As Entidades.TareaAsignada
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TareaAsignada, Datos.TAREA_ASIGNADA)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        tareaAsiganada = IMapper.Map(Of Datos.TAREA_ASIGNADA, Entidades.TareaAsignada)(prmObjTareaAsigandaDatos)
        Return tareaAsiganada
    End Function

    Public Sub eliminarTareaYalmacenamiento(idTarea As Int64)
        _TareaAsignadaDAL.eliminarTareaYalmacenamiento(idTarea)
    End Sub

    Public Function consultarTareaPorId(idTarea As Long) As TareaAsignada
        Return _TareaAsignadaDAL.consultarTareaPorId(idTarea)
    End Function

    Public Function registrarTarea(tareaInsertar As TareaAsignada) As TareaAsignada
        Return _TareaAsignadaDAL.registrarTarea(tareaInsertar)
    End Function

    ''' <summary>
    ''' Retorna una tarea asiganada dependiendo de su identificador
    ''' </summary>
    ''' <param name="prmIntIdTareaAsignada">Identificador de la tarea asignada</param>
    ''' <returns>Objeto del tipo Entidades.TareaAsignada</returns>
    Public Function obtenerTareaAsignadaPorId(ByVal prmIntIdTareaAsignada As Int32) As Entidades.TareaAsignada
        Return ConvertirAEntidadTareaAsiganda(_TareaAsignadaDAL.obtenerTareaAsignadaPorId(prmIntIdTareaAsignada))
    End Function


    ''' <summary>
    ''' Retorna una tarea asiganada dependiendo del idExpediente
    ''' </summary>
    ''' <param name="idExpediente">Identificador de la tarea asignada</param>
    ''' <returns>Objeto del tipo Entidades.TareaAsignada</returns>
    Public Function obtenerTareaAsignadaPorIdExpediente(ByVal idExpediente As String) As Entidades.TareaAsignada
        Return ConvertirAEntidadTareaAsiganda(_TareaAsignadaDAL.obtenerTareaAsignadaPorIdExpediente(idExpediente))
    End Function

    ''' <summary>
    ''' Actualiza el estado operativo de una tarea asiganada
    ''' </summary>
    ''' <param name="prmIntIdTareaAsignada">Identificador de la tarea asignada</param>
    ''' <param name="prmIntEstadoOperativo">Identificador del estado operativo</param>
    ''' <returns>Objeto del tipo Entidades.TareaAsignada</returns>
    Public Function actualizarEstadoOperativoTareaAsignada(ByVal prmIntIdTareaAsignada As Int32, ByVal prmIntEstadoOperativo As Int32) As Entidades.TareaAsignada
        Return ConvertirAEntidadTareaAsiganda(_TareaAsignadaDAL.actualizarEstadoOperativoTareaAsignada(prmIntIdTareaAsignada, prmIntEstadoOperativo))
    End Function

    Public Sub actualizarTareaAsignadaDevolucion(tareaAsignada As TareaAsignada)
        _TareaAsignadaDAL.actualizarTareaAsignadaDevolucion(tareaAsignada)
    End Sub


    Public Function ObtenerUsuarioDevolucion(IdtareaAsignada As Int32, ValUsuario As String, ListEstados As List(Of Int32)) As String
        Return _TareaAsignadaDAL.ObtenerUsuarioDevolucion(IdtareaAsignada, ValUsuario, ListEstados)
    End Function
    Public Function actualizarEstadoOperativoTareaAsignadaPorExpediente(ByVal NroExpediente As Int32, ByVal prmIntEstadoOperativo As Int32) As Entidades.TareaAsignada
        Return ConvertirAEntidadTareaAsiganda(_TareaAsignadaDAL.actualizarEstadoOperativoTareaAsignadaPorExpediente(NroExpediente, prmIntEstadoOperativo))
    End Function

    ''' <summary>
    ''' Actualiza una tarea asignada
    ''' </summary>
    ''' <param name="prmObjTareaAsignada">Objeto del tipo Entidades.TareaAsignada</param>
    ''' <returns>Objeto del tipo Entidades.TareaAsignada</returns>
    Public Function ActualizarTareaAsignada(ByVal prmObjTareaAsignada As Entidades.TareaAsignada) As Entidades.TareaAsignada
        Return ConvertirAEntidadTareaAsiganda(_TareaAsignadaDAL.ActualizarTareaAsignada(prmObjTareaAsignada))
    End Function

    ''' <summary>
    ''' Retorna una tarea asiganada dependiendo del idTitulo
    ''' </summary>
    ''' <param name="idTitulo">Identificador del título a consultar</param>
    ''' <returns>Objeto del tipo Entidades.TareaAsignada</returns>
    Public Function obtenerTareaAsignadaPorIdTitulo(ByVal idTitulo As String) As Entidades.TareaAsignada
        Return ConvertirAEntidadTareaAsiganda(_TareaAsignadaDAL.obtenerTareaAsignadaPorIdTitulo(idTitulo))
    End Function
End Class
