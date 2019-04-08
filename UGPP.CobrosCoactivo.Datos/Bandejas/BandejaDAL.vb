Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports System.Data.SqlClient

Public Class BandejaDAL

    Public Property ConnectionString As String
    Private Property AuditEntity As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Sub New(ByVal audit As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        AuditEntity = audit
    End Sub

    ''' <summary>
    ''' Retorna tabla con la lista de títulos para reasignzción
    ''' </summary>
    ''' <param name="USULOG">Nombre de usuario, por default tomado de la variable de sesión ssloginusuario</param>
    ''' <param name="NROTITULO">Número del título</param>
    ''' <param name="ESTADOPROCESAL">Id estado Procesal</param>
    ''' <param name="ESTADOSOPERATIVO">Id estado operativo</param>
    ''' <param name="FCHENVIOCOBRANZADESDE">Fecha inicio </param>
    ''' <param name="FCHENVIOCOBRANZAHASTA">Fecha fin</param>
    ''' <param name="NROIDENTIFICACIONDEUDOR">Número de identificación del deudor</param>
    ''' <param name="NOMBREDEUDOR">Nombre del deudor</param>
    ''' <returns>TableList para llenar el GridView</returns>
    Public Function obtenerTitulosAprobador(ByVal USULOG As String, Optional ByVal NROTITULO As String = Nothing, Optional ByVal ESTADOPROCESAL As Int32 = Nothing, Optional ByVal ESTADOSOPERATIVO As Int32 = Nothing, Optional ByVal FCHENVIOCOBRANZADESDE As String = Nothing, Optional ByVal FCHENVIOCOBRANZAHASTA As String = Nothing, Optional ByVal NROIDENTIFICACIONDEUDOR As String = Nothing, Optional ByVal NOMBREDEUDOR As String = Nothing) As DataTable

        Dim con As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand("SP_ObtenerTitulosEstudioTitulos", con)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.Add(New SqlParameter("@USULOG", USULOG))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@NROTITULO", NROTITULO))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@ESTADOPROCESAL", ESTADOPROCESAL))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@ESTADOSOPERATIVO", ESTADOSOPERATIVO))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@FCHENVIOCOBRANZADESDE", FCHENVIOCOBRANZADESDE))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@FCHENVIOCOBRANZAHASTA", FCHENVIOCOBRANZAHASTA))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@NROIDENTIFICACIONDEUDOR", NROIDENTIFICACIONDEUDOR))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@NOMBREDEUDOR", NOMBREDEUDOR))

        Dim table As DataTable = New DataTable()
        Dim DataAdapter As SqlDataAdapter = New SqlDataAdapter()
        DataAdapter.SelectCommand = cmd
        DataAdapter.Fill(table)
        Return table

    End Function

    ''' <summary>
    ''' Llamar al procedimiento que actaliza la priorización de los títulos para el gestor de estudio de títulos
    ''' </summary>
    ''' <param name="USULOG">Usuario de consulta</param>
    Public Sub actualizarPriorizacionTitulo(ByVal USULOG As String)
        Dim con As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand("SP_ACTUALIZAR_PRIORIDAD_TITULOS", con)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.Add(New SqlParameter("@USULOG", USULOG))

        con.Open()
        cmd.ExecuteNonQuery()
    End Sub

    ''' <summary>
    ''' Obtiene la lista de solicitudes de reasignación que se han realizado dependiendo del usuario logeado
    ''' Se utitliza para llenar la bandeja de aprobaciones de reasignaciones
    ''' </summary>
    ''' <param name="USULOG">Usuario logeado</param>
    ''' <param name="prmIntTipoSolicitud">{8: Priorización; 9: Reasignación}</param>
    ''' <param name="NroExpediente">Opcional: Número de expediente</param>
    ''' <param name="IdUnicoTitulo">Opcional: ID único del título (tabla MAESTRO_TITULOS)</param>
    ''' <param name="LogUsuSolicitante">Opcional: Login del usuario que realizo la solicitud</param>
    ''' <param name="estadoSolicitud">Opcional: ID del estado de la solicitud (tabla DETALLE_DOMINIO, DOMINIO_ID = 8)</param>
    ''' <returns></returns>
    Public Function obtenerSolicitudesPorTipoSolicitud(ByVal USULOG As String, ByVal prmIntTipoSolicitud As Int32, Optional ByVal NroExpediente As String = Nothing, Optional ByVal IdUnicoTitulo As String = Nothing, Optional ByVal LogUsuSolicitante As String = Nothing, Optional ByVal estadoSolicitud As Integer = 0) As DataTable
        Dim con As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand("SP_OBTENER_SOLICITUDES_POR_TIPO_SOLICITUD", con)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.Add(New SqlParameter("@LoginUsuarioSuperior", USULOG))
        cmd.Parameters.Add(New SqlParameter("@IdTipoSolicitud", prmIntTipoSolicitud))
        cmd.Parameters.Add(New SqlParameter("@EFINROEXP", NroExpediente))
        cmd.Parameters.Add(New SqlParameter("@ID_UNICO_TITULO", IdUnicoTitulo))
        cmd.Parameters.Add(New SqlParameter("@LoginUsuarioSolicitante", LogUsuSolicitante))
        cmd.Parameters.Add(New SqlParameter("@EstadoSolicitud", estadoSolicitud))

        Dim table As New DataTable()
        Dim DataAdapter As New SqlDataAdapter()
        DataAdapter.SelectCommand = cmd
        DataAdapter.Fill(table)
        Return table
    End Function

    ''' <summary>
    ''' Retorna la lista de solictudes de cambio de estado para un repartidor
    ''' </summary>
    ''' <param name="USULOG">Login del repartidor</param>
    ''' <param name="NroExpediente"></param>
    ''' <param name="LogUsuSolicitante"></param>
    ''' <returns></returns>
    Public Function ObtenerSolicitudesCambioEstado(ByVal USULOG As String, Optional ByVal NroExpediente As String = Nothing, Optional ByVal LogUsuSolicitante As String = Nothing) As DataTable
        Dim con As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand("SP_OBTENER_SOLICITUDES_CAMBIO_ESTADO", con)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.Add(New SqlParameter("@USULOG", USULOG))
        cmd.Parameters.Add(New SqlParameter("@EFINROEXP", NroExpediente))
        cmd.Parameters.Add(New SqlParameter("@LoginUsuarioSolicitante", LogUsuSolicitante))
        'cmd.Parameters.Add(New SqlParameter("@CodUsuarioSolicitante", CodUsuSolicitante))

        Dim table As New DataTable()
        Dim DataAdapter As New SqlDataAdapter()
        DataAdapter.SelectCommand = cmd
        DataAdapter.Fill(table)
        Return table
    End Function

End Class
