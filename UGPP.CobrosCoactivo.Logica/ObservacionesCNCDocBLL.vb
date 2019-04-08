Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos

Public Class ObservacionesCNCDocBLL
    ''' <summary>
    ''' Objeto para llamar métodos de consulta a la base de datos
    ''' </summary>
    ''' <returns></returns>
    Private Property _ObservacionesCNCDocDAl As ObservacionesCNCDocDAL
    Private Property _AuditEntity As Entidades.LogAuditoria


    Public Sub New()
        _ObservacionesCNCDocDAl = New ObservacionesCNCDocDAL()
    End Sub


    Public Function ConvertirEntidadObservacionesCNC(ByVal prmObjModuloObservacionesCNCDatos As Datos.OBSERVACIONESDOC_CUMPLE_NOCUMPLE) As Entidades.ObservacionesCNCDoc
        Dim Observaciones As Entidades.ObservacionesCNCDoc
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.ObservacionesCNCDoc, Datos.OBSERVACIONESDOC_CUMPLE_NOCUMPLE)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        Observaciones = IMapper.Map(Of Datos.OBSERVACIONESDOC_CUMPLE_NOCUMPLE, Entidades.ObservacionesCNCDoc)(prmObjModuloObservacionesCNCDatos)
        Return Observaciones
    End Function

    ''' <summary>
    ''' Obtiene las tipificaciones de cumple no cumple estudio de titulos
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Entidades.ESTADO_OPERATIVO</returns>
    Public Function obtenerObservacionesCNCDoc(ByVal IdUnico As Int64, ByVal IdDocumento As Int64) As List(Of Entidades.ObservacionesCNCDoc)
        Dim ObservacionesCNCDoc = _ObservacionesCNCDocDAl.obtenerObservacionCNCDoc(IdUnico, IdDocumento)
        Dim Observaciones As List(Of Entidades.ObservacionesCNCDoc) = New List(Of Entidades.ObservacionesCNCDoc)
        For Each ObservacionesL As Datos.OBSERVACIONESDOC_CUMPLE_NOCUMPLE In ObservacionesCNCDoc
            Observaciones.Add(ConvertirEntidadObservacionesCNC(ObservacionesL))
        Next

        Return Observaciones
    End Function
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _ObservacionesCNCDocDAl = New ObservacionesCNCDocDAL(_AuditEntity)
    End Sub
End Class
