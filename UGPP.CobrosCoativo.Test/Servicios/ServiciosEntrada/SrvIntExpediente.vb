Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class SrvIntExpediente

    Private testContextInstance As TestContext

    '''<summary>
    '''Obtiene o establece el contexto de las pruebas que proporciona
    '''información y funcionalidad para la serie de pruebas actual.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

    <TestMethod()>
    Public Sub OpBuscarPorIdExpediente()
        Dim servicio As New Servicios.SrvIntExpediente
        Dim array(0) As String
        array(0) = "20171100012000003"
        Dim result As CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpBuscarPorIdExpedienteRespTipo = servicio.OpBuscarPorIdExpediente(array)
        Assert.IsTrue(result.expedientes.Count > 0)
    End Sub

    <TestMethod()>
    Public Sub OpCrearExpediente()
        Dim servicio As New Servicios.SrvIntExpediente
        Dim expediente As New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.ExpedienteTipo
        expediente.valFondo = "900"
        expediente.valEntidadPredecesora = "900"
        expediente.identificacion = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.IdentificacionTipo
        expediente.identificacion.codTipoIdentificacion = "EX"
        expediente.identificacion.valNumeroIdentificacion = "987654321"
        expediente.descDescripcion = "Prueba Stefanini 1"
        expediente.codSeccion = "1530"
        expediente.serieDocumental = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.SerieDocumentalTipo
        expediente.serieDocumental.codSerie = "1530.44"

        expediente.identificacion.municipioExpedicion = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.MunicipioTipo()
        expediente.identificacion.municipioExpedicion.departamento = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.DepartamentoTipo()

        Dim result As CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpCrearExpedienteRespTipo = servicio.OpCrearExpediente(expediente)
        Assert.IsTrue(result.expediente.idNumExpediente > 0)
    End Sub


End Class
