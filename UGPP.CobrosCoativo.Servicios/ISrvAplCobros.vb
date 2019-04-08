Imports UGPP.CobrosCoactivo.Entidades

' NOTE: You can use the "Rename" command on the context menu to change the interface name "ISrvAplCobros" in both code and config file together.
<ServiceContract()>
Public Interface ISrvAplCobros

    <OperationContract()>
    Function OpIniciarInstancia(ByVal request As ContextoTransaccionalRequest) As UGPP.CobrosCoactivo.Entidades.ResponseContract
End Interface
