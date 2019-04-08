Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Public Class TipificacionCNCDAL
    Inherits AccesObject(Of Entidades.TipificacionCNC)

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
    ''' Obtiene las tipificaciones para cumple no cumple estudio de titulos
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Datos.TIPIFICACION_CUMPLE_NOCUMPLE</returns>
    Public Function obtenerTipificacionCNC() As List(Of Datos.TIPIFICACION_CUMPLE_NOCUMPLE)
        Dim Tipificacion = (From eo In db.TIPIFICACION_CUMPLE_NOCUMPLE
                            Where eo.HABILITADO = True
                            Order By eo.ID_TIPIFICACION
                            Select eo).ToList()
        Return Tipificacion
    End Function

    ''' <summary>
    ''' Obtiene las tipificaciones para cumple no cumple estudio de titulos
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Datos.TIPIFICACION_CUMPLE_NOCUMPLE</returns>
    Public Function obtenerTipificacionCNCtitulo(idTitulo As Long) As List(Of Datos.TIPIFICACION_CNC)
        Dim LstTipificaciones = (From tt In db.TIPIFICACION_CNC
                                 Where tt.ID_UNICO_MT = idTitulo
                                 Order By tt.ID_TIPIFICACIONCNC
                                 Select tt).ToList()
        Return LstTipificaciones
    End Function
    ''' <summary>
    ''' Funcion que inserta la tipificacion del CNC Gral
    ''' </summary>
    ''' <param name="ID_TIPIFICACION"></param>
    ''' <param name="ID_UNICO_MT"></param>
    ''' <param name="USUARIO"></param>
    ''' <returns></returns>
    Public Function InsertaTipificacionCNC(ByVal ID_TIPIFICACION As Int64, ByVal ID_UNICO_MT As Int64, ByVal USUARIO As String)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_TIPIFICACION", ID_TIPIFICACION))
        Parameters.Add(New SqlClient.SqlParameter("@ID_UNICO_MT", ID_UNICO_MT))
        Parameters.Add(New SqlClient.SqlParameter("@USUARIO", USUARIO))
        Return ExecuteList("SP_InsertaTipificacionCNC",
                           New SqlClient.SqlParameter("@ID_TIPIFICACION", ID_TIPIFICACION),
                           New SqlClient.SqlParameter("@ID_UNICO_MT", ID_UNICO_MT),
                           New SqlClient.SqlParameter("@USUARIO", USUARIO))
        Utils.ValidaLog(_auditLog, "INSERT TIPIFICACION_CNC ", Parameters.ToArray)
    End Function
End Class
