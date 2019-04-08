Imports AutoMapper
Public Class SvcDocumento
    ''' <summary>
    ''' Función que crea un client del servicio Contratos svcDocumento
    ''' </summary>
    ''' <returns>ContratoSvcDocumentoClient</returns>
    Private Shared Function CrearClient() As UGPPSvcDocumento.ContratoSvcDocumentoClient
        Dim serviceDocumentInterface = New UGPPSvcDocumento.ContratoSvcDocumentoClient
        Return serviceDocumentInterface
    End Function

    Private Shared Function CrearCredencials() As CobrosCoactivo.Entidades.UGPPSvcDocumento.credencialesAutenticacion
        Return New CobrosCoactivo.Entidades.UGPPSvcDocumento.credencialesAutenticacion With
        {
        .claveUsuarioSistema = ConfigurationManager.AppSettings("UgppPassword"),
        .nombreUsuarioNegocio = ConfigurationManager.AppSettings("UgppUserBusiness"),
        .nombreUsuarioSistema = ConfigurationManager.AppSettings("UgppUserSystem")
        }
    End Function
    ''' <summary>
    ''' Función que realiza la busqueda por idCarpeta y consume el servicio
    ''' </summary>
    ''' <param name="documento"></param>
    ''' <returns></returns>
    Public Function OpBuscarPorCriteriosExpediente(documento As CobrosCoactivo.Entidades.UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta) As CobrosCoactivo.Entidades.UGPPSvcDocumento.resultadoIdentificadorDocumentos
        documento.credenciales = CrearCredencials()
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of CobrosCoactivo.Entidades.UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta, UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        Dim documentParam = IMapper.Map(Of CobrosCoactivo.Entidades.UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta, UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta)(documento)

        Dim serviceDocumentInterface = CrearClient()
        Dim request As New UGPPSvcDocumento.OpConsultaDocumentosPorIdCarpetaRequest
        request.InformacionConsultaDocumentosPorIdCarpeta = documentParam
        Dim result As New UGPPSvcDocumento.resultadoIdentificadorDocumentos
        result = serviceDocumentInterface.OpConsultaDocumentosPorIdCarpeta(request.InformacionConsultaDocumentosPorIdCarpeta)
        Dim configOutput As New MapperConfiguration(Function(cfg)
                                                        Return cfg.CreateMap(Of UGPPSvcDocumento.resultadoIdentificadorDocumentos, CobrosCoactivo.Entidades.UGPPSvcDocumento.resultadoIdentificadorDocumentos)()
                                                    End Function)
        Dim IMapperOutput = configOutput.CreateMapper()
        Return IMapperOutput.Map(Of UGPPSvcDocumento.resultadoIdentificadorDocumentos, CobrosCoactivo.Entidades.UGPPSvcDocumento.resultadoIdentificadorDocumentos)(result)

    End Function

End Class
