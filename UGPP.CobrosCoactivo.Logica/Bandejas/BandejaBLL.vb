Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class BandejaBLL
    Private Property _BandejaDAL As BandejaDAL
    Private Property Audit As LogAuditoria

#Region "Propiedades de la case, utilizadas para la priorización y reasignación"
    ''' <summary>
    ''' Identificador de la tarea asignada
    ''' </summary>
    ''' <returns></returns>
    Property codTareaAsiganada As Integer

#End Region

    Public Sub New()
        _BandejaDAL = New BandejaDAL()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        Audit = auditLog
        _BandejaDAL = New BandejaDAL(Audit)
    End Sub

    ''' <summary>
    ''' Retorna tabla con la lista de títulos asignado al gestor de estudio de títulos
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
    Public Function obtenerTitulosEstudioTitulos(ByVal USULOG As String, Optional ByVal NROTITULO As String = Nothing, Optional ByVal ESTADOPROCESAL As Int32 = Nothing, Optional ByVal ESTADOSOPERATIVO As Int32 = Nothing, Optional ByVal FCHENVIOCOBRANZADESDE As String = Nothing, Optional ByVal FCHENVIOCOBRANZAHASTA As String = Nothing, Optional ByVal NROIDENTIFICACIONDEUDOR As String = Nothing, Optional ByVal NOMBREDEUDOR As String = Nothing) As DataTable
        Return _BandejaDAL.obtenerTitulosAprobador(USULOG, NROTITULO, ESTADOPROCESAL, ESTADOSOPERATIVO, FCHENVIOCOBRANZADESDE, FCHENVIOCOBRANZAHASTA, NROIDENTIFICACIONDEUDOR, NOMBREDEUDOR)
    End Function


    ''' <summary>
    ''' Método que permite procesar la solicitud de Priorizaión, Reasignación o Suspensión de un título o expediente
    ''' </summary>
    ''' <param name="prmObjSolicitudTarea">Objeto del tipo Entidades.SolicitudTarea sobre el cual se inicializan las variables para guardar la solicitud</param>
    ''' <returns></returns>
    Public Function ProcesarSolicitudEnTarea(ByVal prmObjSolicitudTarea As SolicitudTarea) As List(Of Entidades.TareaSolicitud)
        'Se inicia el objeto de respuesta
        Dim _tareasSolicitud As New List(Of TareaSolicitud)
        'Se valida que vengan título, si no vienen se devuelve la lista vacia
        If prmObjSolicitudTarea.IdsTareasignadas.Count() = 0 Then
            Return _tareasSolicitud
        End If

        'Se inica la variable de usuario para asignación al superior
        Dim _usuarioSuperior As New Usuario
        'Si la solicitud es una diferente a suspensión se asigna un superior a la tarea
        If prmObjSolicitudTarea.TipoSolicitud <> 6 OrElse prmObjSolicitudTarea.TipoSolicitud <> 8 Then
            'Se obtiene el usuario superior del usuario solicitante al que se le va a asiganar la solicitud
            Dim usuariosBLL As New UsuariosBLL()
            Dim usuario = usuariosBLL.obtenerUsuarioPorLogin(prmObjSolicitudTarea.UsuarioSolicitante)
            'Para priorización el superior del superior
            _usuarioSuperior = usuariosBLL.obtenerUsuarioSuperior(usuario.codigo)
            'Para reasignaciones se toma el superior del superior
            If prmObjSolicitudTarea.TipoSolicitud = 9 Then
                _usuarioSuperior = usuariosBLL.obtenerUsuarioSuperior(_usuarioSuperior.codigo)
            End If
        End If

        'Se inicia la variable para la relación con la tabla TAREA_OBSERVACION
        Dim idTareaObservacion = Nothing
        'Si viene un texto de observación en el llamado al método se debe crear el registro en la tabla TAREA_OBSERVACION
        If Not IsNothing(prmObjSolicitudTarea.SolicitudTareaObservacion) Then
            Dim _tareaObservacion As New TareaObservacion()
            _tareaObservacion.OBSERVACION = prmObjSolicitudTarea.SolicitudTareaObservacion
            _tareaObservacion.IND_ESTADO = 1
            _tareaObservacion.FEC_CREACION = DateTime.Now
            Dim _tareaObservacionBLL As New TareaObservacionBLL()
            _tareaObservacion = _tareaObservacionBLL.crearTareaObservacion(_tareaObservacion)
            idTareaObservacion = _tareaObservacion.COD_ID_TAREA_OBSERVACION
        End If

        Dim _tareaSolicitud As New TareaSolicitud()
        '_tareaSolicitud.ID_TAREA_ASIGNADA = prmIntIdTareaAsignada
        _tareaSolicitud.VAL_USUARIO_SOLICITANTE = prmObjSolicitudTarea.UsuarioSolicitante
        _tareaSolicitud.VAL_USUARIO_APROBADOR = If(Not IsNothing(_usuarioSuperior.codigo), _usuarioSuperior.login, Nothing)
        _tareaSolicitud.VAL_USUARIO_DESTINO = If(Not IsNothing(prmObjSolicitudTarea.UsuarioDestino), prmObjSolicitudTarea.UsuarioDestino, Nothing)
        _tareaSolicitud.VAL_TIPO_SOLICITUD = prmObjSolicitudTarea.TipoSolicitud
        _tareaSolicitud.VAL_TIPOLOGIA = prmObjSolicitudTarea.TipologiaSolicitud
        _tareaSolicitud.ID_TAREA_OBSERVACION = idTareaObservacion
        _tareaSolicitud.FEC_SOLICITUD = DateTime.Now

        'Objeto para guardar los registros
        Dim _tareaSolicitudBLL As New TareaSolicitudBLL()
        For Each _idTareaAsignada As Integer In prmObjSolicitudTarea.IdsTareasignadas
            Dim _tareaSolicitudLoop As New TareaSolicitud()
            _tareaSolicitudLoop = _tareaSolicitud
            _tareaSolicitudLoop.ID_TAREA_ASIGNADA = _idTareaAsignada
            _tareaSolicitudLoop = _tareaSolicitudBLL.guardarTareaSolicitud(_tareaSolicitudLoop)
            _tareasSolicitud.Add(_tareaSolicitudLoop)

            'Se actualiza el estado operativo de la tarea asignada
            If Not IsNothing(prmObjSolicitudTarea.CodNuevoEstado) Then
                Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(_idTareaAsignada, prmObjSolicitudTarea.CodNuevoEstado)
            End If

        Next

        Return _tareasSolicitud
    End Function

    ''' <summary>
    ''' Solicitar reasignación de un título o expediente
    ''' </summary>
    ''' <param name="prmIntIdTareaAsignada">Id de la tarea asignada</param>
    ''' <param name="prmStrUsuarioSolicitante">Nombre de usuario que realiza la solicitud</param>
    ''' <returns>Objeto del tipo Entidades.TareaSolicitud</returns>
    Public Function solicitudTarea(ByVal prmIntIdTareaAsignada As Int32, ByVal prmStrUsuarioSolicitante As String, ByVal prmIntTipoSolicitud As Int32, ByVal prmIntTipologia As Int32, Optional ByVal prmIntTareaObservacion As Int32 = Nothing) As Entidades.TareaSolicitud
        Dim usuariosBLL As New UsuariosBLL()
        Dim usuario = usuariosBLL.obtenerUsuarioPorLogin(prmStrUsuarioSolicitante)
        'Para reasignaciones se toma el superior
        If prmIntTipoSolicitud = 9 Then

        End If
        'Para priorización el superior del superior

        Dim superior = usuariosBLL.obtenerUsuarioSuperior(usuario.codigo)

        Dim tareaSolicitud As New Entidades.TareaSolicitud()
        tareaSolicitud.ID_TAREA_ASIGNADA = prmIntIdTareaAsignada
        tareaSolicitud.VAL_USUARIO_SOLICITANTE = prmStrUsuarioSolicitante
        tareaSolicitud.VAL_USUARIO_APROBADOR = superior.login
        tareaSolicitud.VAL_TIPO_SOLICITUD = prmIntTipoSolicitud
        tareaSolicitud.VAL_TIPOLOGIA = prmIntTipologia
        tareaSolicitud.ID_TAREA_OBSERVACION = prmIntTareaObservacion

        Dim tareaSolicitudBLL As New TareaSolicitudBLL()
        Dim tareaSolicitudRes = tareaSolicitudBLL.guardarTareaSolicitud(tareaSolicitud)

        'Actualiza el estado de la tarea asignada, solo para las priorizaciones
        If (prmIntTipoSolicitud = 9) Then
            Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
            Dim tareaAsignada = _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(prmIntIdTareaAsignada, 13)
        End If
        'tareaAsignada.COD_ESTADO_OPERATIVO = 13

        Return tareaSolicitud
    End Function

    ''' <summary>
    ''' Llamar al procedimiento que actaliza la priorización de los títulos para el gestor de estudio de títulos
    ''' </summary>
    ''' <param name="USULOG">Usuario de consulta</param>
    Public Sub actualizarPriorizacionTitulo(ByVal USULOG As String)
        _BandejaDAL.actualizarPriorizacionTitulo(USULOG)
    End Sub

    ''' <summary>
    ''' Obtiene la lista de solicitudes de reasignación que se han realizado dependiendo del usuario logeado
    ''' Se utitliza para llenar la bandeja de aprobaciones de reasignaciones
    ''' </summary>
    ''' <param name="USULOG">Usuario logeado (cordinador)</param>
    ''' <param name="NroExpediente">Opcional: Número de expediente</param>
    ''' <param name="IdUnicoTitulo">Opcional: ID único del título (tabla MAESTRO_TITULOS)</param>
    ''' <param name="LogUsuSolicitante">Opcional: Login del usuario que realizo la solicitud</param>
    ''' <param name="estadoSolicitud">Opcional: ID del estado de la solicitud (tabla DETALLE_DOMINIO, DOMINIO_ID = 8)</param>
    ''' <returns></returns>
    Public Function obtenerSolicitudesPorTipoSolicitud(ByVal USULOG As String, ByVal prmIntTipoSolicitud As Int32, Optional ByVal NroExpediente As String = Nothing, Optional ByVal IdUnicoTitulo As String = Nothing, Optional ByVal LogUsuSolicitante As String = Nothing, Optional ByVal estadoSolicitud As Integer = 0) As DataTable
        Return _BandejaDAL.obtenerSolicitudesPorTipoSolicitud(USULOG, prmIntTipoSolicitud, NroExpediente, IdUnicoTitulo, LogUsuSolicitante, estadoSolicitud)
    End Function

    ''' <summary>
    ''' Retorna la lista de solictudes de cambio de estado para un repartidor
    ''' </summary>
    ''' <param name="USULOG">Login del repartidor</param>
    ''' <param name="NroExpediente"></param>
    ''' <param name="LogUsuSolicitante"></param>
    ''' <param name="CodUsuSolicitante"></param>
    ''' <returns></returns>
    Public Function ObtenerSolicitudesCambioEstado(ByVal USULOG As String, Optional ByVal NroExpediente As String = Nothing, Optional ByVal LogUsuSolicitante As String = Nothing) As DataTable
        Return _BandejaDAL.ObtenerSolicitudesCambioEstado(USULOG, NroExpediente, LogUsuSolicitante)
    End Function

End Class
