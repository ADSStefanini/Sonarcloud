Imports AutoMapper

Public Class SrvIntDocumento
    Inherits ServiceParent

    ''' <summary>
    ''' Crea un cliente SOAP
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function CrearClient(contextoTransaccional As UGPPSrvIntDocumento.ContextoTransaccionalTipo) As UGPPSrvIntDocumento.portSrvIntDocumentoSOAPClient
        Dim service As New UGPPSrvIntDocumento.portSrvIntDocumentoSOAPClient
        service.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings("UgppUserBusiness")
        service.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings("UgppPassword")

        contextoTransaccional.fechaInicioTx = DateTime.Now
        contextoTransaccional.idUsuarioAplicacion = ConfigurationManager.AppSettings("UgppUserBusiness")
        contextoTransaccional.valClaveUsuarioAplicacion = ConfigurationManager.AppSettings("UgppPassword")
        contextoTransaccional.idUsuario = ConfigurationManager.AppSettings("UgppUserSystem")
        contextoTransaccional.idEmisor = ConfigurationManager.AppSettings("UgppUserSystem")


        ' LIMIPIAR PARAMETROS PARA NO ENVIARLOS
        contextoTransaccional.idTx = ""
        contextoTransaccional.idInstanciaProceso = ""
        contextoTransaccional.idDefinicionProceso = ""
        contextoTransaccional.valNombreDefinicionProceso = ""
        contextoTransaccional.idInstanciaActividad = ""
        contextoTransaccional.valNombreDefinicionActividad = ""
        contextoTransaccional.valTamPagina = "0"
        contextoTransaccional.valNumPagina = "0"

        Return service
    End Function

    ''' <summary>
    ''' Servicio que permite la creación de un expediente de acuerdo a los criterios ingresados
    ''' </summary>
    ''' <param name="documento"></param>
    ''' <returns></returns>
    Public Function OpIngresarDocumento(documento As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoSolTipo) As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo
        ' LIMIPIAR PARAMETROS PARA NO ENVIARLOS
        ClearProperties(documento)
        ' Documento
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoSolTipo, UGPPSrvIntDocumento.OpIngresarDocumentoSolTipo)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        Dim documentoParam = IMapper.Map(Of CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoSolTipo, UGPPSrvIntDocumento.OpIngresarDocumentoSolTipo)(documento)

        documentoParam.contextoTransaccional = New UGPPSrvIntDocumento.ContextoTransaccionalTipo
        Dim service As UGPPSrvIntDocumento.portSrvIntDocumentoSOAPClient = CrearClient(documentoParam.contextoTransaccional)
        Dim response As UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo = service.OpIngresarDocumento(documentoParam)

        Dim configOutput As New MapperConfiguration(Function(cfg)
                                                        Return cfg.CreateMap(Of UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo)()
                                                    End Function)
        Dim IMapperOutput = configOutput.CreateMapper()
        Return IMapperOutput.Map(Of UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo)(response)

    End Function

    ''' <summary>
    ''' Servicio que permite realizar la consulta de un documento dependiendo de su código GUID
    ''' </summary>
    ''' <param name="documentoConsulta"></param>
    ''' <returns></returns>
    Public Function ConsultarDcoumento(documentoConsulta As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoSolTipo) As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo
        ' LIMIPIAR PARAMETROS PARA NO ENVIARLOS
        ClearProperties(documentoConsulta)
        ' Documento
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoSolTipo, UGPPSrvIntDocumento.OpConsultarDocumentoSolTipo)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        Dim documentoParam = IMapper.Map(Of CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoSolTipo, UGPPSrvIntDocumento.OpConsultarDocumentoSolTipo)(documentoConsulta)

        documentoParam.contextoTransaccional = New UGPPSrvIntDocumento.ContextoTransaccionalTipo
        Dim service As UGPPSrvIntDocumento.portSrvIntDocumentoSOAPClient = CrearClient(documentoParam.contextoTransaccional)
        'Se crea el id de la transacción para la consulta
        documentoParam.contextoTransaccional.idTx = Guid.NewGuid().ToString()
        'Adición de datos para evitar excepción en la consulta del servicio
        documentoParam.documento.valPaginas = "0"
        documentoParam.documento.numFolios = "0"
        'Consumo del servicio
        Dim response As UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo = service.OpConsultarDocumento(documentoParam)

        Dim configOutput As New MapperConfiguration(Function(cfg)
                                                        Return cfg.CreateMap(Of UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo)()
                                                    End Function)
        Dim IMapperOutput = configOutput.CreateMapper()
        Return IMapperOutput.Map(Of UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo)(response)

    End Function

End Class
