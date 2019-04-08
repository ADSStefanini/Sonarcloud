Imports AutoMapper
Imports UGPP.CobrosCoactivo.Entidades.UGPPSrvIntCobros

Public Class SrvIntCobros
    Inherits ServiceParent
    ''' <summary>
    ''' Crear el contexto transaccional 
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function CrearContextoTransaccional() As UGPPSrvIntCobros.ContextoTransaccionalTipo
        Dim contextoTransaccional As New ContextoTransaccionalTipo
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
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of ContextoTransaccionalTipo, UGPPSrvIntCobros.ContextoTransaccionalTipo)()
                                              End Function)
        Dim IMapper = config.CreateMapper()

        Return IMapper.Map(Of ContextoTransaccionalTipo, UGPPSrvIntCobros.ContextoTransaccionalTipo)(contextoTransaccional)
    End Function
    ''' <summary>
    ''' Crea la instancia para llamar el servicio
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function CrearClient() As UGPPSrvIntCobros.SrvIntCobros_PortTypeClient
        Dim service As New UGPPSrvIntCobros.SrvIntCobros_PortTypeClient
        service.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings("UgppUserBusiness")
        service.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings("UgppPassword")
        Return service
    End Function
    ''' <summary>
    ''' Retorna la respuesta del servicio 
    ''' y la convierte en un objeto interno
    ''' </summary>
    ''' <param name="param"></param>
    ''' <returns></returns>
    Public Function OpActualizarEstadoInstancia(param As ActualizacionEstadoProcesoCobrosReq) As ActualizacionEstadoProcesoCobrosResp
        Dim response As UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosResp
        Dim service As UGPPSrvIntCobros.SrvIntCobros_PortTypeClient = CrearClient()
        Dim request As New UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosReq
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of ActualizacionEstadoProcesoCobrosReq, UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosReq)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        request = IMapper.Map(Of ActualizacionEstadoProcesoCobrosReq, UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosReq)(param)
        request.ContextoTransaccional = CrearContextoTransaccional()
        response = service.OpActualizarEstadoInstanciaCobros(request)

        Dim configResponse As New MapperConfiguration(Function(cfg)
                                                          Return cfg.CreateMap(Of UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosResp, ActualizacionEstadoProcesoCobrosResp)()
                                                      End Function)
        Dim IMapperResponse = configResponse.CreateMapper()
        Return IMapperResponse.Map(Of UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosResp, ActualizacionEstadoProcesoCobrosResp)(response)
    End Function

    ''' <summary>
    ''' Sobrecarga que recibe el id de la transacción
    ''' </summary>
    ''' <param name="param"></param>
    ''' <param name="idTx"></param>
    ''' <returns></returns>
    Public Function OpActualizarEstadoInstancia(param As ActualizacionEstadoProcesoCobrosReq, ByVal idTx As String) As ActualizacionEstadoProcesoCobrosResp
        Dim response As UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosResp
        Dim service As UGPPSrvIntCobros.SrvIntCobros_PortTypeClient = CrearClient()
        Dim request As New UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosReq
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of ActualizacionEstadoProcesoCobrosReq, UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosReq)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        request = IMapper.Map(Of ActualizacionEstadoProcesoCobrosReq, UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosReq)(param)
        request.ContextoTransaccional = CrearContextoTransaccional()
        request.ContextoTransaccional.idTx = idTx
        response = service.OpActualizarEstadoInstanciaCobros(request)

        Dim configResponse As New MapperConfiguration(Function(cfg)
                                                          Return cfg.CreateMap(Of UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosResp, ActualizacionEstadoProcesoCobrosResp)()
                                                      End Function)
        Dim IMapperResponse = configResponse.CreateMapper()
        Return IMapperResponse.Map(Of UGPPSrvIntCobros.ActualizacionEstadoProcesoCobrosResp, ActualizacionEstadoProcesoCobrosResp)(response)
    End Function

End Class
