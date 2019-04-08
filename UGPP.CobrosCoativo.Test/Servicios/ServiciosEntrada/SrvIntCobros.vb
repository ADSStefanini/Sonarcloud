Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Entidades.UGPPSrvIntCobros
Imports UGPP.CobrosCoativo.Servicios

<TestClass()> Public Class SrvIntCobros
    ''' <summary>
    ''' Prueba unitaria exitosa servicio de cobros
    ''' </summary>
    <TestMethod()>
    Public Sub OpActualizarEstadoInstanciaSuccess()
        Dim service As New Servicios.SrvIntCobros
        Dim response As New ActualizacionEstadoProcesoCobrosResp
        Dim request As New ActualizacionEstadoProcesoCobrosReq
        request.fechaEvaluacion = DateTime.Now.ToShortDateString
        request.idTipoCartera = "1"
        request.idTituloOrigen = "687498719848"
        request.observacionesEvaluacionTitulo = "Obser 1"
        request.resultadoEvaluacion = "Exelente"
        request.usuarioEvaluador = "s-bpmpar01"
        Dim documento As New DocumentoCobros
        documento.codTipoDocumento = "1"
        documento.codDocumentic = "1"
        documento.observacionesDocumento = "Prueba"
        documento.valNombreDocumento = "Name.pdf"
        Dim documentoLista As New List(Of DocumentoCobros)
        documentoLista.Add(documento)
        request.documentos = documentoLista.ToArray
        response = service.OpActualizarEstadoInstancia(request, "63cffedc-89e4-4e34-b0ad-04cd2e3c59b8")

        Assert.IsNotNull(response)
    End Sub
    ''' <summary>
    ''' Prueba unitaria fallida servicio de cobors
    ''' </summary>
    <TestMethod()>
    Public Sub OpActualizarEstadoInstanciaFail()
        Dim service As New Servicios.SrvIntCobros
        Dim response As New ActualizacionEstadoProcesoCobrosResp
        Dim request As New ActualizacionEstadoProcesoCobrosReq

        request.fechaEvaluacion = DateTime.Now.ToShortDateString
        request.idTipoCartera = "1"
        request.idTituloOrigen = "687498719848"
        request.observacionesEvaluacionTitulo = "Obser 1"
        request.resultadoEvaluacion = "Exelente"
        request.usuarioEvaluador = "s-bpmpar01"
        Dim documento As New DocumentoCobros
        documento.codTipoDocumento = "123"
        documento.codDocumentic = "{1234}"
        documento.observacionesDocumento = "Prueba"
        documento.valNombreDocumento = "Name.pdf"
        Dim documentoLista As New List(Of DocumentoCobros)
        documentoLista.Add(documento)
        request.documentos = documentoLista.ToArray
        response = service.OpActualizarEstadoInstancia(request)

        Assert.IsNotNull(response)
    End Sub
End Class