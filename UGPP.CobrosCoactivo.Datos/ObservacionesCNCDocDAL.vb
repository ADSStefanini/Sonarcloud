Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class ObservacionesCNCDocDAL
    Inherits AccesObject(Of ObservacionesCNCDoc)

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
    Public Function obtenerObservacionCNCDoc(ByVal IdUnico As Int64, ByVal IdDocumento As Int64) As List(Of Datos.OBSERVACIONESDOC_CUMPLE_NOCUMPLE)
        Dim Observacion = (From eo In db.OBSERVACIONESDOC_CUMPLE_NOCUMPLE
                           Where eo.ID_UNICO_MT = IdUnico
                           Where eo.ID_DOCUMENTO = IdDocumento
                           Order By eo.FCHENVIO
                           Select eo).ToList()
        Return Observacion
    End Function

    ''' <summary>
    '''  Función que Inserta el comentario de CNC de los Documentos
    ''' </summary>
    ''' <param name="ID_UNICO_MT"></param>
    ''' <param name="ID_DOCUMENTO"></param>
    ''' <param name="USUARIO"></param>
    ''' <param name="OBSERVACIONES"></param>
    ''' <returns></returns>
    Public Function InsertaObservacionCNCDoc(ByVal ID_UNICO_MT As Int64, ByVal ID_DOCUMENTO As Int64, ByVal USUARIO As String, ByVal CUMPLENOCUMPLE As Boolean, ByVal DESTINATARIO As String, ByVal OBSERVACIONES As String)
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_UNICO_MT", ID_UNICO_MT))
        Parameters.Add(New SqlClient.SqlParameter("@ID_DOCUMENTO", ID_DOCUMENTO))
        Parameters.Add(New SqlClient.SqlParameter("@USUARIO", USUARIO))
        Parameters.Add(New SqlClient.SqlParameter("@CUMPLENOCUMPLE", CUMPLENOCUMPLE))
        Parameters.Add(New SqlClient.SqlParameter("@DESTINATARIO", DESTINATARIO))
        Parameters.Add(New SqlClient.SqlParameter("@OBSERVACIONES", OBSERVACIONES))
        Utils.ValidaLog(_auditLog, "EXECUTE SP_InsertaObservacionCNCDoc", Parameters.ToArray)
        Return ExecuteList("SP_InsertaObservacionCNCDoc", Parameters.ToArray())
    End Function


End Class
