Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class CambiosEstadoBLL
    ''' <summary>
    ''' Clase de comunicaión para la conexión a la DB
    ''' </summary>
    Dim cambiosEstadoDAL As CambiosEstadoDAL
    Private Property _AuditEntity As LogAuditoria

    ''' <summary>
    ''' Control y CRUD de la entidad CAMBIOS_ESTADO
    ''' </summary>
    Public Sub New()
        cambiosEstadoDAL = New CambiosEstadoDAL()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        _AuditEntity = auditLog
        cambiosEstadoDAL = New CambiosEstadoDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.CAMBIOS_ESTADO a Entidades.CambiosEstado
    ''' </summary>
    ''' <param name="prmObjCambioEstado">Objeto de tipo Datos.CAMBIOS_ESTADO</param>
    ''' <returns>Objeto de tipo Entidades.CambiosEstado</returns>
    Private Function ConvertirAEntidadCambioEstado(ByVal prmObjCambioEstado As Datos.CAMBIOS_ESTADO) As Entidades.CambiosEstado
        Dim cambioEstado As Entidades.CambiosEstado
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.CambiosEstado, Datos.CAMBIOS_ESTADO)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        cambioEstado = IMapper.Map(Of Datos.CAMBIOS_ESTADO, Entidades.CambiosEstado)(prmObjCambioEstado)
        Return cambioEstado
    End Function


    ''' <summary>
    ''' Crear un nuevo registro Cambios Estado
    ''' </summary>
    ''' <param name="prmObjCambiosEstado">Objeto de tipo Cambios estado</param>
    ''' <returns></returns>
    Public Function guardarCambiosEstado(ByVal prmObjCambiosEstado As Entidades.CambiosEstado) As Entidades.CambiosEstado
        Return ConvertirAEntidadCambioEstado(cambiosEstadoDAL.guardarCambiosEstado(prmObjCambiosEstado))
    End Function
End Class
