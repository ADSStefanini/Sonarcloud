Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Public Class TareaAsignadaDAL
    Inherits AccesObject(Of TareaAsignada)

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
    ''' Metodo para consultar una tarea por id
    ''' </summary>
    ''' <param name="idTarea">id de la tarea</param>
    ''' <returns></returns>
    Public Function consultarTareaPorId(idTarea As Long) As TareaAsignada
        Return ExecuteList("SP_TAREA_ASIGNADA_POR_ID", New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", idTarea)).FirstOrDefault()
    End Function

    Public Sub eliminarTareaYalmacenamiento(idTarea As Int64)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim itemTarea As TAREA_ASIGNADA = obtenerTareaAsignadaPorId(idTarea)
        Parameters.Add(New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", idTarea))
        If itemTarea.ALMACENAMIENTO_TEMPORAL IsNot Nothing And itemTarea.ALMACENAMIENTO_TEMPORAL.Count() > 0 Then
            db.ALMACENAMIENTO_TEMPORAL.Remove(itemTarea.ALMACENAMIENTO_TEMPORAL.FirstOrDefault())
        End If
        db.TAREA_ASIGNADA.Remove(itemTarea)
        Utils.salvarDatos(db)
        Utils.ValidaLog(_auditLog, "DELETE TAREA_ASIGNADA ", Parameters.ToArray)
    End Sub

    Public Function registrarTarea(tareaInsertar As TareaAsignada) As TareaAsignada
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim parametros As SqlClient.SqlParameter() = New SqlClient.SqlParameter() {
                           New SqlClient.SqlParameter("@VAL_USUARIO_NOMBRE", tareaInsertar.VAL_USUARIO_NOMBRE),
                           New SqlClient.SqlParameter("@ID_UNICO_TITULO", tareaInsertar.ID_UNICO_TITULO),
                           New SqlClient.SqlParameter("@EFINROEXP_EXPEDIENTE", tareaInsertar.EFINROEXP_EXPEDIENTE),
                           New SqlClient.SqlParameter("@COD_TIPO_OBJ", tareaInsertar.COD_TIPO_OBJ),
                           New SqlClient.SqlParameter("@VAL_PRIORIDAD", tareaInsertar.VAL_PRIORIDAD),
                           New SqlClient.SqlParameter("@COD_ESTADO_OPERATIVO", tareaInsertar.COD_ESTADO_OPERATIVO)}
        Utils.ValidaLog(_auditLog, "EXECUTE SP_TAREA_ASIGNADA_INGRESAR", parametros)
        Return ExecuteList("SP_TAREA_ASIGNADA_INGRESAR", parametros).FirstOrDefault()

    End Function



    ''' <summary>
    ''' Retorna una tarea asiganada dependiendo de su identificador
    ''' </summary>
    ''' <param name="prmIntIdTareaAsignada">Identificador de la tarea asignada</param>
    ''' <returns>Objeto del tipo Datos.TAREA_ASIGNADA</returns>
    Public Function obtenerTareaAsignadaPorId(ByVal prmIntIdTareaAsignada As Int32) As Datos.TAREA_ASIGNADA
        Dim tareaAsignada = (From ta In db.TAREA_ASIGNADA
                             Where ta.ID_TAREA_ASIGNADA = prmIntIdTareaAsignada
                             Select ta).FirstOrDefault()
        Return tareaAsignada
    End Function

    ''' <summary>
    ''' Retorna una tarea asiganada dependiendo del idExpediente
    ''' </summary>
    ''' <param name="idExpediente">Identificador de la tarea asignada</param>
    ''' <returns>Objeto del tipo Datos.TAREA_ASIGNADA</returns>
    Public Function obtenerTareaAsignadaPorIdExpediente(ByVal idExpediente As String) As Datos.TAREA_ASIGNADA
        Dim tareaAsignada = (From ta In db.TAREA_ASIGNADA
                             Where ta.EFINROEXP_EXPEDIENTE = idExpediente
                             Select ta).FirstOrDefault()
        Return tareaAsignada
    End Function

    ''' <summary>
    ''' Actualiza el estado operativo de una tarea asiganada
    ''' </summary>
    ''' <param name="prmIntIdTareaAsignada"></param>
    ''' <param name="prmIntIdEstadoOperativo"></param>
    ''' <returns></returns>
    Public Function actualizarEstadoOperativoTareaAsignada(ByVal prmIntIdTareaAsignada As Int32, ByVal prmIntIdEstadoOperativo As Int32) As Datos.TAREA_ASIGNADA
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim tareaAsignada = obtenerTareaAsignadaPorId(prmIntIdTareaAsignada)
        Dim updateDate = DateTime.Now
        If (tareaAsignada.COD_TIPO_OBJ = 4 And prmIntIdEstadoOperativo = 13) Then
            prmIntIdEstadoOperativo = 3
        End If
        tareaAsignada.COD_ESTADO_OPERATIVO = prmIntIdEstadoOperativo
        tareaAsignada.FEC_ACTUALIZACION = updateDate
        Parameters.Add(New SqlClient.SqlParameter("@COD_ESTADO_OPERATIVO", prmIntIdEstadoOperativo))
        Parameters.Add(New SqlClient.SqlParameter("@FEC_ACTUALIZACION", updateDate))
        Utils.salvarDatos(db)
        Utils.ValidaLog(_auditLog, "UPDATE TAREA_ASIGNADA ", Parameters.ToArray)
        Return tareaAsignada
    End Function


    ''' <summary>
    ''' Actualiza el estado operativo de una tarea asiganada
    ''' </summary>
    ''' <returns></returns>
    Public Sub actualizarTareaAsignadaDevolucion(tareaAsignada As TareaAsignada)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim tareaAsignadaResult = obtenerTareaAsignadaPorId(tareaAsignada.ID_TAREA_ASIGNADA)
        tareaAsignadaResult.COD_ESTADO_OPERATIVO = tareaAsignada.COD_ESTADO_OPERATIVO
        tareaAsignadaResult.VAL_USUARIO_NOMBRE = tareaAsignada.VAL_USUARIO_NOMBRE
        tareaAsignadaResult.FEC_ACTUALIZACION = tareaAsignada.FEC_ACTUALIZACION
        tareaAsignadaResult.ID_UNICO_TITULO = tareaAsignada.ID_UNICO_TITULO
        Utils.salvarDatos(db)
        Parameters.Add(New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", tareaAsignada.ID_TAREA_ASIGNADA))
        Parameters.Add(New SqlClient.SqlParameter("@COD_ESTADO_OPERATIVO", tareaAsignada.COD_ESTADO_OPERATIVO))
        Parameters.Add(New SqlClient.SqlParameter("@VAL_USUARIO_NOMBRE", tareaAsignada.VAL_USUARIO_NOMBRE))
        Parameters.Add(New SqlClient.SqlParameter("@FEC_ACTUALIZACION", tareaAsignada.FEC_ACTUALIZACION))
        Parameters.Add(New SqlClient.SqlParameter("@ID_UNICO_TITULO", tareaAsignada.ID_UNICO_TITULO))
        Utils.ValidaLog(_auditLog, "UPDATE TareaAsignada ", Parameters.ToArray)
    End Sub

    Public Function ObtenerUsuarioDevolucion(IdtareaAsignada As Int32, ValUsuario As String, ListEstados As List(Of Int32))
        Dim tareaAsignada = (From ta In db.HISTORICO_TAREA_ASIGNADA
                             Where ta.ID_TAREA_ASIGNADA = IdtareaAsignada And ta.VAL_USUARIO_NOMBRE <> ValUsuario And ListEstados.Contains(ta.COD_ESTADO_OPERATIVO) Order By ta.ID_HISTORICO_TAREA_ASIGNADA Descending
                             Select ta).FirstOrDefault()
        Return tareaAsignada.VAL_USUARIO_NOMBRE
    End Function
    Public Function actualizarEstadoOperativoTareaAsignadaPorExpediente(ByVal NroExpendiente As Int32, ByVal prmIntIdEstadoOperativo As Int32) As Datos.TAREA_ASIGNADA
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim tareaAsignada = obtenerTareaAsignadaPorIdExpediente(NroExpendiente)
        tareaAsignada.COD_ESTADO_OPERATIVO = prmIntIdEstadoOperativo
        Utils.salvarDatos(db)
        Parameters.Add(New SqlClient.SqlParameter("@ID_TAREA_ASIGNADA", tareaAsignada.ID_TAREA_ASIGNADA))
        Parameters.Add(New SqlClient.SqlParameter("@COD_ESTADO_OPERATIVO", tareaAsignada.COD_ESTADO_OPERATIVO))
        Utils.ValidaLog(_auditLog, "UPDATE TAREA_ASIGNADA", Parameters.ToArray)
        Return tareaAsignada
    End Function

    ''' <summary>
    ''' Actualiza una tarea asignada
    ''' </summary>
    ''' <param name="prmObjTareaAsignada">Objeto del tipo Entidades.TareaAsignada</param>
    ''' <returns>Objeto del tipo Datos.TAREA_ASIGNADA</returns>
    Public Function ActualizarTareaAsignada(ByVal prmObjTareaAsignada As Entidades.TareaAsignada) As Datos.TAREA_ASIGNADA
        Dim _tareaAsiganada = obtenerTareaAsignadaPorId(prmObjTareaAsignada.ID_TAREA_ASIGNADA)
        _tareaAsiganada.VAL_USUARIO_NOMBRE = prmObjTareaAsignada.VAL_USUARIO_NOMBRE
        _tareaAsiganada.COD_TIPO_OBJ = prmObjTareaAsignada.COD_TIPO_OBJ
        _tareaAsiganada.ID_UNICO_TITULO = prmObjTareaAsignada.ID_UNICO_TITULO
        _tareaAsiganada.EFINROEXP_EXPEDIENTE = prmObjTareaAsignada.EFINROEXP_EXPEDIENTE
        _tareaAsiganada.FEC_ACTUALIZACION = prmObjTareaAsignada.FEC_ACTUALIZACION
        _tareaAsiganada.FEC_ENTREGA_GESTOR = prmObjTareaAsignada.FEC_ENTREGA_GESTOR
        _tareaAsiganada.VAL_PRIORIDAD = prmObjTareaAsignada.VAL_PRIORIDAD
        _tareaAsiganada.IND_TITULO_PRIORIZADO = prmObjTareaAsignada.IND_TITULO_PRIORIZADO
        _tareaAsiganada.COD_ESTADO_OPERATIVO = prmObjTareaAsignada.COD_ESTADO_OPERATIVO
        _tareaAsiganada.ID_TAREA_OBSERVACION = prmObjTareaAsignada.ID_TAREA_OBSERVACION
        Utils.salvarDatos(db)
        Return _tareaAsiganada
    End Function


    ''' <summary>
    ''' Retorna una tarea asiganada dependiendo del IdTitulo
    ''' </summary>
    ''' <param name="IdTitulo">Identificador del título</param>
    ''' <returns>Objeto del tipo Datos.TAREA_ASIGNADA</returns>
    Public Function obtenerTareaAsignadaPorIdTitulo(ByVal IdTitulo As Integer) As Datos.TAREA_ASIGNADA
        Dim tareaAsignada = (From ta In db.TAREA_ASIGNADA
                             Where ta.ID_UNICO_TITULO = IdTitulo
                             Select ta).FirstOrDefault()
        Return tareaAsignada
    End Function
End Class
