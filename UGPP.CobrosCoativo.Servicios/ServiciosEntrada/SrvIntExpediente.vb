Imports AutoMapper

Public Class SrvIntExpediente
    Inherits ServiceParent

    ''' <summary>
    ''' Crea un cliente SOAP
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function CrearClient(contextoTransaccional As UGPPSrvIntExpediente.ContextoTransaccionalTipo) As UGPPSrvIntExpediente.portSrvIntExpedienteSOAPClient
        Dim service As New UGPPSrvIntExpediente.portSrvIntExpedienteSOAPClient
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
    ''' Servicio que permite la búsqueda de un expediente
    ''' </summary>
    ''' <param name="idExpediente">Lista de ids expedientes</param>
    Public Function OpBuscarPorIdExpediente(idExpediente As String()) As CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpBuscarPorIdExpedienteRespTipo
        Dim params As New UGPPSrvIntExpediente.OpBuscarPorIdExpedienteSolTipo

        ' PARAMETROS
        params.idExpediente = idExpediente

        params.contextoTransaccional = New UGPPSrvIntExpediente.ContextoTransaccionalTipo
        Dim service As UGPPSrvIntExpediente.portSrvIntExpedienteSOAPClient = CrearClient(params.contextoTransaccional)
        Dim response As UGPPSrvIntExpediente.OpBuscarPorIdExpedienteRespTipo = service.OpBuscarPorIdExpediente(params)

        Dim configOutput As New MapperConfiguration(Function(cfg)
                                                        Return cfg.CreateMap(Of UGPPSrvIntExpediente.OpBuscarPorIdExpedienteRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpBuscarPorIdExpedienteRespTipo)()
                                                    End Function)
        Dim IMapperOutput = configOutput.CreateMapper()
        Return IMapperOutput.Map(Of UGPPSrvIntExpediente.OpBuscarPorIdExpedienteRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpBuscarPorIdExpedienteRespTipo)(response)

    End Function

    ''' <summary>
    ''' Servicio que permite la creación de un expediente de acuerdo a los criterios ingresados
    ''' </summary>
    ''' <param name="expediente"></param>
    ''' <returns></returns>
    Public Function OpCrearExpediente(expediente As CobrosCoactivo.Entidades.UGPPSrvIntExpediente.ExpedienteTipo) As CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpCrearExpedienteRespTipo
        ' LIMIPIAR PARAMETROS PARA NO ENVIARLOS
        ClearProperties(expediente)

        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of CobrosCoactivo.Entidades.UGPPSrvIntExpediente.ExpedienteTipo, UGPPSrvIntExpediente.ExpedienteTipo)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        Dim expedienteParam = IMapper.Map(Of CobrosCoactivo.Entidades.UGPPSrvIntExpediente.ExpedienteTipo, UGPPSrvIntExpediente.ExpedienteTipo)(expediente)
        Dim params As New UGPPSrvIntExpediente.OpCrearExpedienteSolTipo

        ' PARAMETROS
        params.expediente = expedienteParam

        params.contextoTransaccional = New UGPPSrvIntExpediente.ContextoTransaccionalTipo
        Dim service As UGPPSrvIntExpediente.portSrvIntExpedienteSOAPClient = CrearClient(params.contextoTransaccional)
        Dim response As UGPPSrvIntExpediente.OpCrearExpedienteRespTipo = service.OpCrearExpediente(params)

        Dim configOutput As New MapperConfiguration(Function(cfg)
                                                        Return cfg.CreateMap(Of UGPPSrvIntExpediente.OpCrearExpedienteRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpCrearExpedienteRespTipo)()
                                                    End Function)
        Dim IMapperOutput = configOutput.CreateMapper()
        Return IMapperOutput.Map(Of UGPPSrvIntExpediente.OpCrearExpedienteRespTipo, CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpCrearExpedienteRespTipo)(response)

    End Function

End Class
