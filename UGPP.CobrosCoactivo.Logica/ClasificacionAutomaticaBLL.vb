Imports UGPP.CobrosCoactivo.Entidades

Public Class ClasificacionAutomaticaBLL

    ''' <summary>
    ''' ID del título relacionado con el expediente
    ''' </summary>
    Property idTitulo As Int32
    ''' <summary>
    ''' ID de la tarea asignada, se toma a partir de la URL
    ''' </summary>
    Property idTask As Int32
    Private Property _AuditEntity As LogAuditoria

    Public Sub New(Optional ByVal prmStrNroExpediente As String = Nothing, Optional ByVal prmIntIdTask As Integer = Nothing)
        If Not IsNothing(prmStrNroExpediente) Then
            Try
                Dim _maestroTitulosBLL As New MaestroTitulosBLL
                Dim _titulo = _maestroTitulosBLL.obtenerTituloPorExpedienteId(prmStrNroExpediente)
                Me.idTitulo = _titulo.idunico
            Catch ex As Exception
                Me.idTitulo = 0
            End Try

        End If

        If Not IsNothing(prmIntIdTask) Then
            Me.idTask = prmIntIdTask
        End If
    End Sub

    Public Sub New(Optional ByVal prmStrNroExpediente As String = Nothing, Optional ByVal prmIntIdTask As Integer = Nothing, Optional ByVal auditLog As LogAuditoria = Nothing)
        Dim _maestroTitulosBLL As MaestroTitulosBLL
        If Not IsNothing(prmStrNroExpediente) Then
            Try
                If auditLog IsNot Nothing Then
                    Me._AuditEntity = auditLog
                    _maestroTitulosBLL = New MaestroTitulosBLL(_AuditEntity)
                Else
                    _maestroTitulosBLL = New MaestroTitulosBLL
                End If

                Dim _titulo = _maestroTitulosBLL.obtenerTituloPorExpedienteId(prmStrNroExpediente)
                Me.idTitulo = _titulo.idunico
            Catch ex As Exception
                Me.idTitulo = 0
            End Try

        End If

        If Not IsNothing(prmIntIdTask) Then
            Me.idTask = prmIntIdTask
        End If

    End Sub


    Private Function actualizarEstadosOperativos(ByVal prmStrIdExpediente As String)
        If Not IsNothing(Me.idTitulo) Then
            Dim _expedienteBLL As New ExpedienteBLL()
            _expedienteBLL.asignarExpedientePorRepartir(prmStrIdExpediente)
            Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
            _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(Me.idTask, 6)
        End If
    End Function

    ''' <summary>
    ''' Función que permite calsficiación automatica por prescripción
    ''' </summary>
    ''' <returns></returns>
    Public Function porPrescripcion(ByVal idExpediente As String, ByVal valCodUsuario As String) As Boolean
        Dim valReturn = False
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim maestroTitulo As MaestroTitulosBLL = New MaestroTitulosBLL
        Dim valEstadoProcesoCoactivo = My.Resources.ValEstadoProcesoCoactivo
        Dim valEtapaMandamientoDePago = My.Resources.ValEtapaMandamientoDePago
        Dim objExpediente = maestroTitulo.obtenerTituloMasCercanoPrescripcionPorExpedienteId(idExpediente)
        Dim valFecPrescripcion As Date = objExpediente.MT_fec_cad_presc
        Dim valFecActual As Date = Now.Date
        Dim valDifTerminoPrescripcion As Long = DateAndTime.DateDiff(DateInterval.Month, valFecPrescripcion, valFecActual)
        If Convert.ToInt32(valDifTerminoPrescripcion) <= Convert.ToInt32(My.Resources.ValMesesTerminoPrescripcion) Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcesoCoactivo, valEtapaMandamientoDePago)
            registroAutomaticoCambioEstado(idExpediente, valEtapaMandamientoDePago, valCodUsuario)
            If Not IsNothing(Me.idTitulo) AndAlso Me.idTitulo <> 0 Then
                actualizarEstadosOperativos(idExpediente)
            End If
            valReturn = True
        End If
        Return valReturn
    End Function

    ''' <summary>
    ''' Función que permite calsficiación automatica por facilidad de pago
    ''' </summary>
    ''' <returns></returns>
    Public Function porFacilidadDePago(ByVal idExpediente As String) As Boolean
        Dim valReturn As Boolean = False
        Dim titulos As MaestroTitulosBLL = New MaestroTitulosBLL
        Dim tareaAsignada As TareaAsignadaBLL = New TareaAsignadaBLL
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim valEstadoProcesal = My.Resources.ValEstadoProcesoFacilPago
        Dim objTareaAsignada = tareaAsignada.obtenerTareaAsignadaPorIdExpediente(idExpediente)
        Dim fecEntregaGestor As Date = objTareaAsignada.FEC_ENTREGA_GESTOR
        Dim fecExigibilidad = titulos.consultarTipoTitulo(idExpediente).FirstOrDefault().MT_fec_exi_liq.Value
        Dim ValFec = DateTime.Compare(fecEntregaGestor, fecExigibilidad)
        If ValFec > 0 Then
            expedientes.ActualizarExpediente(idExpediente, valEstadoProcesal)
            valReturn = True
            If Not IsNothing(Me.idTitulo) Then
                actualizarEstadosOperativos(idExpediente)
            End If
        End If
        Return valReturn
    End Function

    ''' <summary>
    ''' Función que permite calsficiación automatica por tipo de obligación
    ''' </summary>
    Public Function porTipoDeObligacion(ByVal idExpediente As String, ByVal valCodUsuario As String) As Boolean
        Dim valRet As Boolean = False
        Dim maestroTitulo As MaestroTitulosBLL = New MaestroTitulosBLL
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim ListaValTipoTitulo = maestroTitulo.consultarTipoTitulo(idExpediente)
        Dim valTipoTitulo = ListaValTipoTitulo.FirstOrDefault(Function(x) (x.MT_tipo_titulo))
        Dim tipoTitulo = valTipoTitulo.MT_tipo_titulo
        Dim valEstadoProcConcursal = My.Resources.ValEstadoProcesoConcursal
        Dim valEtapaDsiac = My.Resources.ValEtapaDSIAC
        Dim valEstadoProcesoCoactivo = My.Resources.ValEstadoProcesoCoactivo
        Dim valEstadoProcPersuasivo = My.Resources.ValEstadoProcesoPersuasivo
        If tipoTitulo = My.Resources.ValTiTituloSentJudicial Or tipoTitulo = My.Resources.ValTiTituloCuotaPartPension Or tipoTitulo = My.Resources.ValTiTituloMaValPagadosPorFraude Then
            expedientes.ActualizarExpediente(idExpediente, valEstadoProcesoCoactivo)
            If Not IsNothing(Me.idTitulo) Then
                actualizarEstadosOperativos(idExpediente)
            End If
            valRet = True
        ElseIf tipoTitulo = My.Resources.ValTiTituloReqParaDecYOCorregir Or tipoTitulo = My.Resources.ValTiTuloPliegoDeCargos Or tipoTitulo = My.Resources.ValTiTituloInformeDeFiscalizacion Then
            expedientes.ActualizarExpediente(idExpediente, valEstadoProcConcursal)
            If Not IsNothing(Me.idTitulo) Then
                actualizarEstadosOperativos(idExpediente)
            End If
            valRet = True
        Else
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcPersuasivo, valEtapaDsiac)
            registroAutomaticoCambioEstado(idExpediente, valEtapaDsiac, valCodUsuario)
            If Not IsNothing(Me.idTitulo) Then
                actualizarEstadosOperativos(idExpediente)
            End If
            valRet = True
        End If
        Return valRet
    End Function

    ''' <summary>
    ''' función que permite realizar el registro del valor de la etapa procesal en la tabla cambio_estado
    ''' </summary>
    ''' <param name="ValEtapa"></param>
    Public Sub registroAutomaticoCambioEstado(ByVal idExpediente As String, ByVal ValEtapa As String, ByVal valCodUsuario As String)
        Dim _expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim ListaExpediente = _expedientes.consultarExpediente(idExpediente)
        Dim _expediente = ListaExpediente.FirstOrDefault()
        'Objeto de cambio de estado
        Dim prmObjCambiosEstado As New CambiosEstado()
        prmObjCambiosEstado.NroExp = idExpediente
        prmObjCambiosEstado.repartidor = My.Resources.ValCodRepartidor
        prmObjCambiosEstado.abogado = valCodUsuario
        prmObjCambiosEstado.fecha = Now.Date
        prmObjCambiosEstado.estado = _expediente.EFIESTADO.ToString()
        prmObjCambiosEstado.estadopago = If(IsNothing(_expediente.EFIESTADOPAGO), "0", _expediente.EFIESTADOPAGO.ToString())
        prmObjCambiosEstado.estadooperativo = My.Resources.ValEstadoOperPorRepartir
        prmObjCambiosEstado.etapaprocesal = Convert.ToInt32(ValEtapa)
        Dim ValoresCambioEstado As New CambiosEstadoBLL(_AuditEntity)
        ValoresCambioEstado.guardarCambiosEstado(prmObjCambiosEstado)
    End Sub
End Class
