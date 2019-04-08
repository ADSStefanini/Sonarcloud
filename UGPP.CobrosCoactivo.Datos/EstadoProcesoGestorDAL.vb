Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports System.Data.Entity

Public Class EstadoProcesoGestorDAL
    Private Property Context As UGPPEntities
    Private Property AuditData As LogAuditoria
    ''' <summary>
    ''' Constructor
    ''' </summary>
    Sub New()
        Context = New UGPPEntities
    End Sub
    ''' <summary>
    ''' Constructor de auditoria
    ''' </summary>
    ''' <param name="auditLog"></param>
    Sub New(ByVal auditLog As LogAuditoria)
        Context = New UGPPEntities
        AuditData = auditLog
    End Sub
    ''' <summary>
    ''' Guarda un registro en la base de datos tabla estados_proceso_gestor
    ''' </summary>
    ''' <param name="estadosProceso">entidad de tipo estados_proceso_gestor</param>
    ''' <returns>true/false</returns>
    Public Function Guardar(ByVal estadosProceso As ESTADOS_PROCESO_GESTOR) As Boolean
        Context.ESTADOS_PROCESO_GESTOR.Add(estadosProceso)
        Utils.salvarDatos(Context)
        Return True
    End Function
    ''' <summary>
    ''' Actualiza un registro en la base de datos tabla estados_proceso_gestor
    ''' </summary>
    ''' <param name="estadosProceso">entidad de tipo estados_proceso_gestor</param>
    ''' <returns>true/false</returns>
    Public Function Actualizar(ByVal estadosProceso As ESTADOS_PROCESO_GESTOR) As Boolean
        Context.ESTADOS_PROCESO_GESTOR.Attach(estadosProceso)
        Context.Entry(estadosProceso).State = EntityState.Modified
        Utils.salvarDatos(Context)
        Return True
    End Function
    ''' <summary>
    ''' Retorna la lista de gestores y estaods proceso
    ''' </summary>
    ''' <returns></returns>
    Public Function ObtenerGestoresyEstados() As List(Of ESTADOS_PROCESO_GESTOR)
        Return (From m In Context.ESTADOS_PROCESO_GESTOR
                Select m).ToList()
    End Function
    ''' <summary>
    ''' Retorna un item de gestores y estaods proceso
    ''' </summary>
    ''' <param name="usuario">usuario</param>
    ''' <param name="estado">estado</param>
    ''' <returns></returns>
    Public Function ObtenerGestoresyEstadosPorLlaves(ByVal usuario As String, ByVal estado As String) As ESTADOS_PROCESO_GESTOR
        Return Context.ESTADOS_PROCESO_GESTOR.Where(Function(x) x.VAL_USUARIO = usuario AndAlso x.COD_ID_ESTADOS_PROCESOS = estado).FirstOrDefault
    End Function
End Class
