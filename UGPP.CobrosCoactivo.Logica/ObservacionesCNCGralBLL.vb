Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades
Public Class ObservacionesCNCGralBLL
    ''' <summary>
    ''' Objeto para llamar métodos de consulta a la base de datos
    ''' </summary>
    ''' <returns></returns>
    Private Property _ObservacionesCNCGralDAl As ObservacionesCNCGralDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _ObservacionesCNCGralDAl = New ObservacionesCNCGralDAL()
    End Sub
    ''' <param name="auditData"></param>
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _ObservacionesCNCGralDAl = New ObservacionesCNCGralDAL(_AuditEntity)
    End Sub

    Public Function ConvertirEntidadObservacionesCNC(ByVal prmObjModuloObservacionesCNCDatos As Datos.OBSERVACIONES_CUMPLE_NOCUMPLE) As Entidades.ObservacionesCNC
        Dim Observaciones As Entidades.ObservacionesCNC
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.ObservacionesCNC, Datos.OBSERVACIONES_CUMPLE_NOCUMPLE)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        Observaciones = IMapper.Map(Of Datos.OBSERVACIONES_CUMPLE_NOCUMPLE, Entidades.ObservacionesCNC)(prmObjModuloObservacionesCNCDatos)
        Return Observaciones
    End Function

    ''' <summary>
    ''' Obtiene las tipificaciones de cumple no cumple estudio de titulos
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Entidades.ESTADO_OPERATIVO</returns>
    Public Function obtenerObservacionesCNCGral(ByVal IdUnico As Int64) As List(Of Entidades.ObservacionesCNC)
        Dim ObservacionesCNC = _ObservacionesCNCGralDAl.obtenerObservacionCNC(IdUnico)
        Dim Observaciones As List(Of Entidades.ObservacionesCNC) = New List(Of Entidades.ObservacionesCNC)
        For Each ObservacionesL As Datos.OBSERVACIONES_CUMPLE_NOCUMPLE In ObservacionesCNC
            Observaciones.Add(ConvertirEntidadObservacionesCNC(ObservacionesL))
        Next

        Return Observaciones
    End Function



End Class
