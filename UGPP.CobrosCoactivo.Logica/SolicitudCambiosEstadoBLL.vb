Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports AutoMapper

Public Class SolicitudCambiosEstadoBLL
    Private Property _Solictudes_CambioEstado As Solicitudes_CambioEstadoDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _Solictudes_CambioEstado = New Solicitudes_CambioEstadoDAL()
    End Sub

    Public Function consultarSolictiudesCE(Id As Int32) As Datos.SOLICITUDES_CAMBIOESTADO
        Return _Solictudes_CambioEstado.obtenerSolicitud_CambiosEstadoPorExp(Id)
    End Function

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.SOLICITUDES_CAMBIOESTADO a Entidades.Solicitudes_CambioEstado
    ''' </summary>
    ''' <param name="prmObjSolicitudCambioEstadoDatos">Objeto de tipo Datos.SOLICITUDES_CAMBIOESTADO</param>
    ''' <returns>Objeto de tipo Entidades.Solicitudes_CambioEstado</returns>
    Public Function ConvertirAEntidadSolicitudCambioEstado(ByVal prmObjSolicitudCambioEstadoDatos As Datos.SOLICITUDES_CAMBIOESTADO) As Entidades.Solicitudes_CambioEstado
        Dim solicitudesCambioEstado As Entidades.Solicitudes_CambioEstado
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.EJEFISGLOBAL, Datos.SOLICITUDES_CAMBIOESTADO)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        solicitudesCambioEstado = IMapper.Map(Of Datos.SOLICITUDES_CAMBIOESTADO, Entidades.Solicitudes_CambioEstado)(prmObjSolicitudCambioEstadoDatos)
        Return solicitudesCambioEstado
    End Function

    ''' <summary>
    ''' Actualiza el estado de una solicitud de cambio de estado procesal
    ''' </summary>
    ''' <param name="prmObjSolicitudCambioEstado">Objeto del tipo Entidades.Solicitudes_CambioEstado con los campos ejecutor(quien autotiza), aprob_revisor (SI o NO) y nota_revisor requeridos </param>
    ''' <returns>Obgeto del tipo Datos.SOLICITUDES_CAMBIOESTADO</returns>
    Public Function actualizarEstadoAprobacionCambioEstado(ByVal prmObjSolicitudCambioEstado As Entidades.Solicitudes_CambioEstado) As Entidades.Solicitudes_CambioEstado
        Return ConvertirAEntidadSolicitudCambioEstado(_Solictudes_CambioEstado.actualizarEstadoAprobacionCambioEstado(prmObjSolicitudCambioEstado))
    End Function

End Class
