Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class HistoricoClasificacionManualBLL

    Private Property _HistoricoClasificacionManualDAL As HistoricoClasificacionManualDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _HistoricoClasificacionManualDAL = New HistoricoClasificacionManualDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _HistoricoClasificacionManualDAL = New HistoricoClasificacionManualDAL(_AuditEntity)
    End Sub


    ''' <summary>
    ''' Convierte un objeto del tipo Datos.HISTORICO_CLASIFICACION_MANUAL a Entidades.HistoricoClasificacionManual
    ''' </summary>
    ''' <param name="prmObjHistoricoClasificacionManual">Objeto de tipo Datos.HISTORICO_CLASIFICACION_MANUAL</param>
    ''' <returns>Objeto de tipo Entidades.HistoricoClasificacionManual</returns>
    Private Function ConvertirAEntidadHistoricoClasificacionManual(ByVal prmObjHistoricoClasificacionManual As Datos.HISTORICO_CLASIFICACION_MANUAL) As Entidades.HistoricoClasificacionManual
        Dim historicoClasificacionManual As Entidades.HistoricoClasificacionManual
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.HistoricoClasificacionManual, Datos.HISTORICO_CLASIFICACION_MANUAL)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        historicoClasificacionManual = IMapper.Map(Of Datos.HISTORICO_CLASIFICACION_MANUAL, Entidades.HistoricoClasificacionManual)(prmObjHistoricoClasificacionManual)
        Return historicoClasificacionManual
    End Function

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.HISTORICO_CLASIFICACION_MANUAL a Entidades.HistoricoClasificacionManual
    ''' </summary>
    ''' <param name="prmObjHistoricoClasificacionManual">Objeto de tipo Datos.HISTORICO_CLASIFICACION_MANUAL</param>
    ''' <returns>Objeto de tipo Entidades.HistoricoClasificacionManual</returns>
    Private Function ConvertirADatosHistoricoClasificacionManual(ByVal prmObjHistoricoClasificacionManual As Entidades.HistoricoClasificacionManual) As Datos.HISTORICO_CLASIFICACION_MANUAL
        Dim historicoClasificacionManual As Datos.HISTORICO_CLASIFICACION_MANUAL
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Datos.HISTORICO_CLASIFICACION_MANUAL, Entidades.HistoricoClasificacionManual)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        historicoClasificacionManual = IMapper.Map(Of Entidades.HistoricoClasificacionManual, Datos.HISTORICO_CLASIFICACION_MANUAL)(prmObjHistoricoClasificacionManual)
        Return historicoClasificacionManual
    End Function


    ''' <summary>
    ''' Guarda el historico de clasificacion
    ''' </summary>
    ''' <returns></returns>
    Public Function Salvar(ByVal historico As Entidades.HistoricoClasificacionManual) As Boolean
        Dim result As Boolean
        Dim data As New Datos.HistoricoClasificacionManualDAL
        'Dim dataHistorico As New Datos.HISTORICO_CLASIFICACION_MANUAL
        'dataHistorico.BENEFICIO_TRIBUTARIO = historico.BENEFICIO_TRIBUTARIO
        'dataHistorico.ID_EXPEDIENTE = historico.ID_EXPEDIENTE
        'dataHistorico.ID_USUARIO = historico.ID_USUARIO
        'dataHistorico.FECHA = historico.FECHA
        'dataHistorico.PERSONA_JURIDICA = historico.PERSONA_JURIDICA
        'dataHistorico.PERSONA_NATURAL = historico.PERSONA_NATURAL
        'dataHistorico.PERSONA_VIVA = historico.PERSONA_VIVA
        'dataHistorico.MATRICULA_MERCANTIL = historico.MATRICULA_MERCANTIL.Value
        'dataHistorico.ID_MTD_DOCUMENTO = historico.ID_MTD_DOCUMENTO.Value
        'dataHistorico.PROCESO_ESPECIAL = historico.PROCESO_ESPECIAL.Value
        'dataHistorico.TIPO_PROCESO = If(historico.TIPO_PROCESO.HasValue, historico.TIPO_PROCESO.Value, Nothing)
        'dataHistorico.BENEFICIO_TRIBUTARIO = historico.BENEFICIO_TRIBUTARIO
        'dataHistorico.PAGOS_DEUDOR = historico.PAGOS_DEUDOR.Value
        'dataHistorico.NUMERO_RADICADO = historico.NUMERO_RADICADO.Value
        'dataHistorico.OBSERVACIONES = historico.OBSERVACIONES
        'dataHistorico.VALOR_MENOR_UVT = historico.VALOR_MENOR_UVT
        result = data.Salvar(ConvertirADatosHistoricoClasificacionManual(historico))
        Return result
    End Function
    ''' <summary>
    ''' Obtiene una lista del historico de clasificacion por el id del expediente
    ''' </summary>
    ''' <param name="idExpediente">parametro de busqueda</param>
    ''' <returns>lista de tipo historicoclasificacionmanual</returns>
    Public Function ObtenerHistoricoPorIdExpediente(ByVal idExpediente As String) As List(Of Entidades.HistoricoClasificacionManual)
        Dim data As New Datos.HistoricoClasificacionManualDAL
        Dim resultado As New List(Of Entidades.HistoricoClasificacionManual)
        For Each item In data.ObtenerHistoricoPorIdExpediente(idExpediente)
            resultado.Add(New HistoricoClasificacionManual With {.BENEFICIO_TRIBUTARIO = item.BENEFICIO_TRIBUTARIO, .FECHA = item.FECHA, .ID_EXPEDIENTE = item.ID_EXPEDIENTE, .ID_MTD_DOCUMENTO = item.ID_MTD_DOCUMENTO,
            .ID_REGISTRO_CLASIFICACION_MANUAL = item.ID_REGISTRO_CLASIFICACION_MANUAL, .ID_USUARIO = item.ID_USUARIO, .MATRICULA_MERCANTIL = item.MATRICULA_MERCANTIL, .NUMERO_RADICADO = item.NUMERO_RADICADO, .OBSERVACIONES = item.OBSERVACIONES,
            .PAGOS_DEUDOR = item.PAGOS_DEUDOR, .PERSONA_JURIDICA = item.PERSONA_JURIDICA, .PERSONA_NATURAL = item.PERSONA_NATURAL, .PERSONA_VIVA = item.PERSONA_VIVA, .PROCESO_ESPECIAL = item.PROCESO_ESPECIAL, .TIPO_PROCESO = item.TIPO_PROCESO, .VALOR_MENOR_UVT = item.VALOR_MENOR_UVT})
        Next
        Return resultado
    End Function
End Class
