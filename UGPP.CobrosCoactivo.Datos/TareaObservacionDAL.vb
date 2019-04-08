Imports System.Configuration
Imports AutoMapper
Imports UGPP.CobrosCoactivo.Entidades
Public Class TareaObservacionDAL
    Inherits AccesObject(Of Entidades.TareaObservacion)

    Public Property ConnectionString As String

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
    ''' Convierte un objeto del tipo Entidades.TareaObservacion a Datos.TAREA_OBSERVACION
    ''' </summary>
    ''' <param name="prmObjTareaObervacionEntidad">Objeto de tipo Entidades.TareaObservacion</param>
    ''' <returns>Objeto de tipo Datos.TAREA_OBSERVACION</returns>
    Public Function ConvertirADatosTareaObervacion(ByVal prmObjTareaObervacionEntidad As Entidades.TareaObservacion) As Datos.TAREA_OBSERVACION
        Dim tareaObservacion As Datos.TAREA_OBSERVACION
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Datos.TAREA_OBSERVACION, Entidades.TareaObservacion)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        tareaObservacion = IMapper.Map(Of Entidades.TareaObservacion, Datos.TAREA_OBSERVACION)(prmObjTareaObervacionEntidad)
        Return tareaObservacion
    End Function

    ''' <summary>
    ''' Metódo que agrega un nuevo registro en la tabla TAREA_ONSERVACION
    ''' </summary>
    ''' <param name="prmObjTareaObservacionEntidad">Objeto de tipo Entidades.TareaObservacion</param>
    ''' <returns>Objeto de tipo Datos.TAREA_OBSERVACION</returns>
    Public Function crearTareaObservacion(ByVal prmObjTareaObservacionEntidad As Entidades.TareaObservacion) As Datos.TAREA_OBSERVACION
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim tareaObservacion = ConvertirADatosTareaObervacion(prmObjTareaObservacionEntidad)
        Parameters.Add(New SqlClient.SqlParameter("@COD_ID_TAREA_OBSERVACION", tareaObservacion.COD_ID_TAREA_OBSERVACION))
        Parameters.Add(New SqlClient.SqlParameter("@OBSERVACION", tareaObservacion.OBSERVACION))
        Parameters.Add(New SqlClient.SqlParameter("@IND_ESTADO", tareaObservacion.IND_ESTADO))
        Parameters.Add(New SqlClient.SqlParameter("@FEC_CREACION", tareaObservacion.FEC_CREACION))
        db.TAREA_OBSERVACION.Add(tareaObservacion)
        Utils.salvarDatos(db)
        Utils.ValidaLog(_auditLog, "INSERT TAREA_OBSERVACION", Parameters.ToArray)
        Return tareaObservacion
    End Function

    ''' <summary>
    ''' Obtiene una observacion de una tarea por su ID
    ''' </summary>
    ''' <param name="prmIntITareaObservacion">Identificador de la observación</param>
    ''' <returns>Objeto del tipo Datos.TAREA_OBSERVACION</returns>
    Public Function obtenerTareaObservacionPorId(ByVal prmIntITareaObservacion As Int32) As Datos.TAREA_OBSERVACION
        Dim tareaObservacion = (From tob In db.TAREA_OBSERVACION
                                Where tob.COD_ID_TAREA_OBSERVACION = prmIntITareaObservacion
                                Select tob).FirstOrDefault()
        Return tareaObservacion
    End Function

End Class
