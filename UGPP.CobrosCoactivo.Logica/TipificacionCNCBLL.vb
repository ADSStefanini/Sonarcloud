Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos

Public Class TipificacionCNCBLL
    ''' <summary>
    ''' Objeto para llamar métodos de consulta a la base de datos
    ''' </summary>
    ''' <returns></returns>
    Private Property _TipificacionDAL As TipificacionCNCDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _TipificacionDAL = New TipificacionCNCDAL()
    End Sub

    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _TipificacionDAL = New TipificacionCNCDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TIPIFICACION_CUMPLE_NOCUMPLE a Entidades.TipificacionCNC
    ''' </summary>
    ''' <param name="prmObjModuloTipificacionCNCDatos">Objeto de tipo Datos.TIPIFICACION_CUMPLE_NOCUMPLE</param>
    ''' <returns>Objeto de tipo Entidades.TipificacionCNC</returns>
    Public Function ConvertirEntidadTipificacionCNC(ByVal prmObjModuloTipificacionCNCDatos As Datos.TIPIFICACION_CUMPLE_NOCUMPLE) As Entidades.TipificacionCNC
        Dim estadoOperativo As Entidades.TipificacionCNC
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TipificacionCNC, Datos.TIPIFICACION_CUMPLE_NOCUMPLE)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        estadoOperativo = IMapper.Map(Of Datos.TIPIFICACION_CUMPLE_NOCUMPLE, Entidades.TipificacionCNC)(prmObjModuloTipificacionCNCDatos)
        Return estadoOperativo
    End Function

    ''' <summary>
    ''' Obtiene las tipificaciones de cumple no cumple estudio de titulos
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Entidades.ESTADO_OPERATIVO</returns>
    Public Function obtenerTipificacionCNC() As List(Of Entidades.TipificacionCNC)
        Dim tipificacionCNC = _TipificacionDAL.obtenerTipificacionCNC()
        Dim tipificaciones As New List(Of Entidades.TipificacionCNC)
        For Each tipificacion As Datos.TIPIFICACION_CUMPLE_NOCUMPLE In tipificacionCNC
            tipificaciones.Add(ConvertirEntidadTipificacionCNC(tipificacion))
        Next

        Return tipificaciones
    End Function
End Class
