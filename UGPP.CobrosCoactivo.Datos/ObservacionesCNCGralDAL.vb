Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class ObservacionesCNCGralDAL
    Inherits AccesObject(Of ObservacionesCNC)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria
    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    '' <summary>
    '' Obtiene las observaciones generales para cumple no cumple estudio de titulos
    '' </summary>
    '' <returns>Lista de objetos del tipo Datos.TIPIFICACION_CUMPLE_NOCUMPLE</returns>
    Public Function obtenerObservacionCNC(ByVal IdUnico As Int64) As List(Of Datos.OBSERVACIONES_CUMPLE_NOCUMPLE)
        Dim Observacion = (From eo In db.OBSERVACIONES_CUMPLE_NOCUMPLE
                           Where eo.ID_UNICO_MT = IdUnico
                           Order By eo.FCHOBSERVACIONES
                           Select eo).ToList()
        Return Observacion
    End Function

    ''' <summary>
    '''  Funcion que Inserta el comentario de CNC General
    ''' </summary>
    ''' <param name="ID_UNICO_MT"></param>
    ''' <param name="USUARIO"></param>
    ''' <param name="OBSERVACIONES"></param>
    ''' <param name="CUMPLE_NOCUMPLE"></param>
    ''' <returns></returns>
    Public Function InsertaObservacionCNC(ByVal ID_UNICO_MT As Int64, ByVal USUARIO As String, ByVal OBSERVACIONES As String, ByVal DESTINATARIO As String, ByVal CUMPLE_NOCUMPLE As Boolean)
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_UNICO_MT", ID_UNICO_MT))
        Parameters.Add(New SqlClient.SqlParameter("@USUARIO", USUARIO))
        Parameters.Add(New SqlClient.SqlParameter("@OBSERVACIONES", OBSERVACIONES))
        Parameters.Add(New SqlClient.SqlParameter("@CUMPLE_NOCUMPLE", CUMPLE_NOCUMPLE))
        Parameters.Add(New SqlClient.SqlParameter("@DESTINATARIO", DESTINATARIO))
        Utils.ValidaLog(_auditLog, "EXECUTE SP_InsertaObservacionCNC", Parameters.ToArray)
        Return ExecuteList("SP_InsertaObservacionCNC", Parameters.ToArray)
    End Function
End Class
