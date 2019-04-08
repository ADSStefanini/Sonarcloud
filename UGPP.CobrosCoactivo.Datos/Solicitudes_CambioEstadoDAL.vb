Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Public Class Solicitudes_CambioEstadoDAL
    Inherits AccesObject(Of UGPP.CobrosCoactivo.Entidades.Solicitudes_CambioEstado)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub
    Public Function obtenerSolicitud_CambiosEstadoPorExp(ByVal prmIntExpSol As Int32) As Datos.SOLICITUDES_CAMBIOESTADO

        Dim solicitudCE = (From sol In db.SOLICITUDES_CAMBIOESTADO
                           Where sol.idunico = prmIntExpSol
                           Select sol).FirstOrDefault()
        Return solicitudCE
    End Function

    ''' <summary>
    ''' Actualiza el estado de una solicitud de cambio de estado procesal
    ''' </summary>
    ''' <param name="prmObjSolicitudCambioEstado">Objeto del tipo Entidades.Solicitudes_CambioEstado con los campos ejecutor(quien autotiza), aprob_revisor (SI o NO) y nota_revisor requeridos </param>
    ''' <returns>Obgeto del tipo Datos.SOLICITUDES_CAMBIOESTADO</returns>
    Public Function actualizarEstadoAprobacionCambioEstado(ByVal prmObjSolicitudCambioEstado As Entidades.Solicitudes_CambioEstado) As Datos.SOLICITUDES_CAMBIOESTADO
        Dim _solicitudCambioEstado = obtenerSolicitud_CambiosEstadoPorExp(prmObjSolicitudCambioEstado.idunico)
        _solicitudCambioEstado.ejecutor = prmObjSolicitudCambioEstado.ejecutor
        _solicitudCambioEstado.aprob_revisor = prmObjSolicitudCambioEstado.aprob_revisor
        _solicitudCambioEstado.fecha_aprob_revisor = DateTime.Now
        _solicitudCambioEstado.nota_revisor = prmObjSolicitudCambioEstado.nota_revisor

        If prmObjSolicitudCambioEstado.aprob_revisor = "SI" Then
            _solicitudCambioEstado.estadosol = 2
            _solicitudCambioEstado.nivel_escalamiento = 3
        Else
            _solicitudCambioEstado.estadosol = 3
        End If

        Utils.salvarDatos(db)
        Return _solicitudCambioEstado
    End Function
End Class
