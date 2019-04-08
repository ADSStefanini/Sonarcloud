Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class SvcDocumento

    <TestMethod()>
    Public Sub OpConsultaDocumentosPorIdCarpetaFail()
        Dim service As New Servicios.SvcDocumento
        Dim result As CobrosCoactivo.Entidades.UGPPSvcDocumento.resultadoIdentificadorDocumentos
        result = service.OpBuscarPorCriteriosExpediente(New CobrosCoactivo.Entidades.UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta With {
        .idCarpeta = "{F4DAFE46-8FDF-4010-A6C9-B977A37F069F}"})
        Assert.IsTrue(result.estadoEjecucion.codigo = "04")
    End Sub


    <TestMethod()>
    Public Sub OpConsultaDocumentosPorIdCarpetaSuccess()
        Dim service As New Servicios.SvcDocumento
        Dim result As CobrosCoactivo.Entidades.UGPPSvcDocumento.resultadoIdentificadorDocumentos
        result = service.OpBuscarPorCriteriosExpediente(New CobrosCoactivo.Entidades.UGPPSvcDocumento.informacionConsultaDocumentosPorIdCarpeta With {
        .idCarpeta = "{0CA91493-E429-4A8B-B1FB-6555E2ED15B1}"})
        Assert.IsTrue(result.documento.Length > 0)
    End Sub
End Class