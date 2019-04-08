Imports System.Configuration
Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades
Public Class TareaSolicitudDAL
    Inherits AccesObject(Of Entidades.TareaSolicitud)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria
    Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Entidades.EJEFISGLOBAL a Datos.EJEFISGLOBAL
    ''' </summary>
    ''' <param name="prmObjTareaSolicitudEntidad">Objeto de tipo Entidades.EJEFISGLOBAL</param>
    ''' <returns>Objeto de tipo Datos.EJEFISGLOBAL</returns>
    Public Function ConvertirADatosTareaSolicitud(ByVal prmObjTareaSolicitudEntidad As Entidades.TareaSolicitud) As Datos.TAREA_SOLICITUD
        Dim tareaSolicitud As Datos.TAREA_SOLICITUD
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Datos.TAREA_SOLICITUD, Entidades.TareaSolicitud)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        tareaSolicitud = IMapper.Map(Of Entidades.TareaSolicitud, Datos.TAREA_SOLICITUD)(prmObjTareaSolicitudEntidad)
        Return tareaSolicitud
    End Function

    ''' <summary>
    ''' Agregar un nuevo registro a la tabla TAREA_SOLICITUD
    ''' </summary>
    ''' <param name="prmObjTareaSolicitud">Objeto del tipo Entidades.TareaSolicitud</param>
    ''' <returns>Objeto del tipo Datos.TAREA_SOLICITUD</returns>
    Public Function guardarTareaSolicitud(ByVal prmObjTareaSolicitud As Entidades.TareaSolicitud) As Datos.TAREA_SOLICITUD
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim tareaSolicitud = ConvertirADatosTareaSolicitud(prmObjTareaSolicitud)
        Parameters.Add(New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", tareaSolicitud.ID_TAREA_ASIGNADA))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_USUARIO_SOLICITANTE", tareaSolicitud.VAL_USUARIO_SOLICITANTE))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_USUARIO_APROBADOR", tareaSolicitud.VAL_USUARIO_APROBADOR))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_USUARIO_DESTINO", tareaSolicitud.VAL_USUARIO_DESTINO))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_TIPO_SOLICITUD", tareaSolicitud.VAL_TIPO_SOLICITUD))
        Parameters.Add(New SqlClient.SqlParameter("@COD_SOLICITUD_CAMBIO_ESTADO", tareaSolicitud.COD_SOLICITUD_CAMBIO_ESTADO))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_TIPOLOGIA", tareaSolicitud.VAL_TIPOLOGIA))
        db.TAREA_SOLICITUD.Add(tareaSolicitud)
        Utils.salvarDatos(db)
        Utils.ValidaLog(_auditLog, "INSERT TAREA_SOLICITUD ", Parameters.ToArray)
        Return tareaSolicitud
    End Function

    Public Function registrarTarea(tareaInsertar As TareaSolicitud) As TareaSolicitud
        Parameters = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", tareaInsertar.ID_TAREA_ASIGNADA))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_USUARIO_SOLICITANTE", tareaInsertar.VAL_USUARIO_SOLICITANTE))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_USUARIO_APROBADOR", tareaInsertar.VAL_USUARIO_APROBADOR))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_USUARIO_DESTINO", tareaInsertar.VAL_USUARIO_DESTINO))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_TIPO_SOLICITUD", tareaInsertar.VAL_TIPO_SOLICITUD))
        Parameters.Add(New SqlClient.SqlParameter("@COD_SOLICITUD_CAMBIO_ESTADO", tareaInsertar.COD_SOLICITUD_CAMBIO_ESTADO))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_TIPOLOGIA", tareaInsertar.VAL_TIPOLOGIA))
        Parameters.Add(New SqlClient.SqlParameter("@ID_TAREA_OBSERVACION", tareaInsertar.ID_TAREA_OBSERVACION))
        Utils.ValidaLog(_auditLog, "INSERT TareaSolicitud", Parameters.ToArray)
        Return ExecuteList("SP_TAREA_SOLICITUD_INGRESAR", Parameters.ToArray).FirstOrDefault()
    End Function

    ''' <summary>
    ''' Busca si una tarea tiene una solicitud sin procesar del tipo tipo de solicitud pasado como parámetro  
    ''' </summary>
    ''' <param name="prmIntTareaSolicitud">ID TAREA_ASIGNADA</param>
    ''' <param name="prmIntTipoSolicitud">Identificador Tipo Solicitud {6: SolicitudSuspension; 8: SolicitudPriorizacion; 9: SolicitudResignacion}</param>
    ''' <returns>Objeto del tipo Datos.TAREA_SOLICITUD</returns>
    Public Function obtenerTareaSolicitudPorTipoSolicitudNoProcesada(ByVal prmIntTareaSolicitud As Integer, ByVal prmIntTipoSolicitud As Int32) As Datos.TAREA_SOLICITUD
        Dim _tareaSolicitud = (From ts In db.TAREA_SOLICITUD
                               Where ts.ID_TAREA_ASIGNADA = prmIntTareaSolicitud
                               Where ts.VAL_TIPO_SOLICITUD = prmIntTipoSolicitud
                               Where ts.IND_SOLICITUD_PROCESADA.Equals(False)
                               Select ts).FirstOrDefault()
        Return _tareaSolicitud
    End Function

    ''' <summary>
    ''' Obtener tarea solicitud por ID
    ''' </summary>
    ''' <param name="prmIntIdTareaSolicitud">Identificador de la tarea solicitud</param>
    ''' <returns>Objeto del tipo Datos.TAREA_SOLICITUD</returns>
    Public Function obternerTareaSolicitudPorId(ByVal prmIntIdTareaSolicitud As Int32) As Datos.TAREA_SOLICITUD
        Dim _tareaSolicitud = (From ts In db.TAREA_SOLICITUD
                               Where ts.ID_TAREA_SOLICITUD = prmIntIdTareaSolicitud
                               Select ts).FirstOrDefault()
        Return _tareaSolicitud
    End Function

    ''' <summary>
    ''' Actualiza un registro de la una solicitud
    ''' </summary>
    ''' <param name="prmObjTareaSolicitud">Obgeto del tipo Entidades.TareaSolicitud</param>
    ''' <returns>Objeto del tipo Datos.TAREA_SOLICITUD</returns>
    Public Function ActualizarTareaSolicitud(ByVal prmObjTareaSolicitud As Entidades.TareaSolicitud) As Datos.TAREA_SOLICITUD
        Dim _tareaSolicitud = obternerTareaSolicitudPorId(prmObjTareaSolicitud.ID_TAREA_SOLICITUD)
        _tareaSolicitud.ID_TAREA_ASIGNADA = prmObjTareaSolicitud.ID_TAREA_ASIGNADA
        _tareaSolicitud.VAL_USUARIO_SOLICITANTE = prmObjTareaSolicitud.VAL_USUARIO_SOLICITANTE
        _tareaSolicitud.VAL_USUARIO_APROBADOR = prmObjTareaSolicitud.VAL_USUARIO_APROBADOR
        _tareaSolicitud.VAL_USUARIO_DESTINO = prmObjTareaSolicitud.VAL_USUARIO_DESTINO
        _tareaSolicitud.VAL_TIPO_SOLICITUD = prmObjTareaSolicitud.VAL_TIPO_SOLICITUD
        _tareaSolicitud.COD_SOLICITUD_CAMBIO_ESTADO = prmObjTareaSolicitud.COD_SOLICITUD_CAMBIO_ESTADO
        _tareaSolicitud.VAL_TIPOLOGIA = prmObjTareaSolicitud.VAL_TIPOLOGIA
        _tareaSolicitud.ID_TAREA_OBSERVACION = prmObjTareaSolicitud.ID_TAREA_OBSERVACION
        _tareaSolicitud.IND_SOLICITUD_PROCESADA = prmObjTareaSolicitud.IND_SOLICITUD_PROCESADA
        _tareaSolicitud.COD_ESTADO_SOLICITUD = prmObjTareaSolicitud.COD_ESTADO_SOLICITUD
        _tareaSolicitud.FEC_SOLICITUD = prmObjTareaSolicitud.FEC_SOLICITUD
        Utils.salvarDatos(db)
        Return _tareaSolicitud
    End Function

    ''' <summary>
    ''' Método que retorna el último ID registrado en la tabla
    ''' </summary>
    ''' <returns>Int => último ID</returns>
    Public Function getLastId() As Int32
        Dim intIdt As Integer? = db.TAREA_SOLICITUD.Max(Function(ts) CType(ts.ID_TAREA_SOLICITUD, Integer?))
        Return intIdt
    End Function
End Class
