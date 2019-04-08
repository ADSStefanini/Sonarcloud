Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class ContextoTransaccionalDAL
    Inherits AccesObject(Of Entidades.ContextoTransaccionalTipo)

    Dim _AuditLog As LogAuditoria
    Dim db As UGPPEntities
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities
    End Sub
    ''' <summary>
    ''' Constructor de la clase con auditoria
    ''' </summary>
    ''' <param name="auditLog"></param>
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        _AuditLog = auditLog
    End Sub
    ''' <summary>
    ''' Guarda o actualiza el contexto transaccional
    ''' </summary>
    ''' <param name="contexto"></param>
    ''' <param name="IdTitulo"></param>
    ''' <returns></returns>
    Public Function Add(ByVal contexto As ContextoTransaccionalTipo, ByVal IdTitulo As Integer) As Boolean
        Dim parametros As SqlClient.SqlParameter() = New SqlClient.SqlParameter() {
            New SqlClient.SqlParameter("@ID_TX", contexto.idTx),
            New SqlClient.SqlParameter("@FECHA_INICIO", contexto.fechaInicioTx),
            New SqlClient.SqlParameter("@ID_DEF_PROCESO", contexto.idDefinicionProceso),
            New SqlClient.SqlParameter("@NOMBRE_PROCESO", contexto.valNombreDefinicionProceso),
            New SqlClient.SqlParameter("@ID_USUARIO_APP", contexto.idUsuarioAplicacion),
            New SqlClient.SqlParameter("@ID_EMISOR", contexto.idEmisor),
            New SqlClient.SqlParameter("@ID_TITULO", IdTitulo),
            New SqlClient.SqlParameter("@COD_TX", New Guid().ToString)
        }
        ExecuteCommand("SP_CONTEXTO_TRANSACCIONAL", parametros)
        Utils.ValidaLog(_AuditLog, "EXECUTE SP_CONTEXTO_TRANSACCIONAL", parametros)
        Return True
    End Function

    ''' <summary>
    ''' Obtiene el contexto transaccional por id de titulo y ordenando por la fecha titulo
    ''' </summary>
    ''' <param name="idTitulo"></param>
    ''' <returns></returns>
    Public Function ObtenerContextoPorIdTitulo(ByVal idTitulo As Integer) As Datos.CONTEXTO_TRANSACCIONAL
        Dim contexto As Datos.CONTEXTO_TRANSACCIONAL
        contexto = (From c In db.CONTEXTO_TRANSACCIONAL
                    Where c.ID_TITULO = idTitulo
                    Order By c.FECHA_CREACION Descending
                    Select c).FirstOrDefault()
        Return contexto
    End Function
End Class
