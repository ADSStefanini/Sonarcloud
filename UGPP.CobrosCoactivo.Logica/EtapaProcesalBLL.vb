Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class EtapaProcesalBLL

    Private Property _etapaProcesal As EtapaProcesalDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _etapaProcesal = New EtapaProcesalDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _etapaProcesal = New EtapaProcesalDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.EtapaProcesalDAL a Entidades.EtapaProcesal
    ''' </summary>
    ''' <param name="prmObjEtapaProcesal">Objeto de tipo Datos.TIPOS_CAUSALES_PRIORIZACION</param>
    ''' <returns>Objeto de tipo Entidades.EtapaProcesal</returns>
    Public Function ConvertirEntidadEtapaProcesal(ByVal prmObjEtapaProcesal As Datos.ETAPA_PROCESAL) As Entidades.EtapaProcesal
        Dim etapaProcesal As Entidades.EtapaProcesal
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.EtapaProcesal, Datos.ETAPA_PROCESAL)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        etapaProcesal = IMapper.Map(Of Datos.ETAPA_PROCESAL, Entidades.EtapaProcesal)(prmObjEtapaProcesal)
        Return etapaProcesal
    End Function

    ''' <summary>
    ''' Obtener todas las etapas procesales
    ''' </summary>
    ''' <returns>Lista de etapas procesales</returns>
    Public Function ObtenerEtapaProcesal() As List(Of Entidades.EtapaProcesal)

        Dim listaEtapaProcesalConsulta = _etapaProcesal.ObtenerEtapasProcesales()
        Dim listaEtapaProcesal As List(Of Entidades.EtapaProcesal) = New List(Of Entidades.EtapaProcesal)
        For Each listaEtapasProcesales As Datos.ETAPA_PROCESAL In listaEtapaProcesalConsulta
            listaEtapaProcesal.Add(ConvertirEntidadEtapaProcesal(listaEtapasProcesales))
        Next
        Return listaEtapaProcesal
    End Function
    Public Function ObtenerEtapaProcesalPorId(ByVal Id As String) As List(Of Entidades.EtapaProcesal)

        Dim listaEtapaProcesalConsulta = _etapaProcesal.ObtenerEtapasProcesalesPorId(Id)
        Dim listaEtapaProcesal As List(Of Entidades.EtapaProcesal) = New List(Of Entidades.EtapaProcesal)
        For Each listaEtapasProcesales As Datos.ETAPA_PROCESAL In listaEtapaProcesalConsulta
            listaEtapaProcesal.Add(ConvertirEntidadEtapaProcesal(listaEtapasProcesales))
        Next
        Return listaEtapaProcesal
    End Function
End Class
