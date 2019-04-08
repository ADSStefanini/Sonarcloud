Imports System.Configuration
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Public Class BandejaAreaOrigenDAL

    Inherits AccesObject(Of BandejaTitulosAreaOrigen)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _AuditLog = auditLog
    End Sub

    ''' <summary>
    ''' Funcion para el llamado de los datos del gridview de bandeja de titulos Area Origen
    ''' </summary>
    ''' <returns></returns>
    Public Function ConsultaGrillaBandejaAreaOrigen(ByVal USULOG As String, Optional ByVal NROTITULO As String = Nothing, Optional ByVal ESTADOPROCESAL As Int32 = Nothing, Optional ByVal ESTADOSOPERATIVO As Int32 = Nothing, Optional ByVal FCHENVIOCOBRANZADESDE As String = Nothing, Optional ByVal FCHENVIOCOBRANZAHASTA As String = Nothing, Optional ByVal NROIDENTIFICACIONDEUDOR As String = Nothing, Optional ByVal NOMBREDEUDOR As String = Nothing) As List(Of BandejaTitulosAreaOrigen)
        Return ExecuteList("SP_GrillaAreaOrigen",
                New SqlClient.SqlParameter("@USULOG", USULOG),
                New SqlClient.SqlParameter("@NROTITULO", NROTITULO),
                New SqlClient.SqlParameter("@ESTADOPROCESAL", ESTADOPROCESAL),
                New SqlClient.SqlParameter("@ESTADOSOPERATIVO", ESTADOSOPERATIVO),
                New SqlClient.SqlParameter("@FCHENVIOCOBRANZADESDE", FCHENVIOCOBRANZADESDE),
                New SqlClient.SqlParameter("@FCHENVIOCOBRANZAHASTA", FCHENVIOCOBRANZAHASTA),
                New SqlClient.SqlParameter("@NROIDENTIFICACIONDEUDOR", NROIDENTIFICACIONDEUDOR),
                New SqlClient.SqlParameter("@NOMBREDEUDOR", NOMBREDEUDOR)
                )
    End Function

    Public Function ConsultaBandejaAreaOrigenEnCreacion(ByVal USULOG As String) As List(Of ALMACENAMIENTO_TEMPORAL)
        Dim lsvTareas = db.TAREA_ASIGNADA.Where(Function(x) (x.COD_ESTADO_OPERATIVO = 1 And x.VAL_USUARIO_NOMBRE = USULOG)).ToList()
        If lsvTareas.Count() > 0 Then
            Dim lsvTareasIDs = lsvTareas.Select(Function(t) t.ID_TAREA_ASIGNADA)
            Return db.ALMACENAMIENTO_TEMPORAL.Where(Function(x) (lsvTareasIDs.Contains(x.ID_TAREA_ASIGNADA))).ToList()
        Else
            Return New List(Of ALMACENAMIENTO_TEMPORAL)()
        End If
    End Function
End Class
