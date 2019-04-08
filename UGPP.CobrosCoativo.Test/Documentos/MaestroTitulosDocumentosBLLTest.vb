Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class MaestroTitulosDocumentosBLLTest

    <TestMethod()> Public Sub crearMaestroTitulosDocumentos()
        Dim documentoTitulo As New Entidades.MaestroTitulosDocumentos
        documentoTitulo.ID_DOCUMENTO_TITULO = 38
        documentoTitulo.ID_MAESTRO_TITULO = 15555
        documentoTitulo.DES_RUTA_DOCUMENTO = "\DocumentosLocal\20183026113002darksiders-3---s02-1493750188165_1280w.jpg"
        documentoTitulo.TIPO_RUTA = 1 'Entidades.Enumeraciones.DominioDetalle.Local
        documentoTitulo.OBSERVA_LEGIBILIDAD = "Prueba unitaria"
        documentoTitulo.NUM_PAGINAS = 1

        Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()
        _maestroTitulosDocumentosBLL.crearMaestroTitulosDocumentos(documentoTitulo)
        Assert.IsTrue(documentoTitulo.ID_DOCUMENTO_TITULO > 0)
    End Sub

    <TestMethod()> Public Sub ActualizarMaestroTitulosDocumentos()
        Dim documentoTitulo As New Entidades.MaestroTitulosDocumentos
        documentoTitulo.ID_MAESTRO_TITULOS_DOCUMENTOS = 19
        documentoTitulo.ID_DOCUMENTO_TITULO = 38
        documentoTitulo.ID_MAESTRO_TITULO = 15555
        documentoTitulo.DES_RUTA_DOCUMENTO = "\DocumentosLocal\20183026113002darksiders-3---s02-1493750188165_1280w.jpg2"
        documentoTitulo.TIPO_RUTA = 1 'Entidades.Enumeraciones.DominioDetalle.Local
        documentoTitulo.OBSERVA_LEGIBILIDAD = "Prueba unitaria actualizacion"
        documentoTitulo.NUM_PAGINAS = 1

        Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()
        _maestroTitulosDocumentosBLL.ActualizarMaestroTitulosDocumentos(documentoTitulo)
        Assert.IsTrue(documentoTitulo.ID_DOCUMENTO_TITULO > 0)
    End Sub

    <TestMethod()> Public Sub obtenerMaestroTitulosDocumentosPorId()
        Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()
        Dim documentoTitulo = _maestroTitulosDocumentosBLL.obtenerMaestroTitulosDocumentosPorId(1)
        Assert.IsTrue(documentoTitulo.ID_DOCUMENTO_TITULO > 0)
    End Sub

    <TestMethod()> Public Sub obtenerDocumentosPorTitulo()
        Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()
        Dim documentosTitulo = _maestroTitulosDocumentosBLL.obtenerDocumentosPorTitulo(15555)
        Assert.IsTrue(documentosTitulo.Count() > 0)
    End Sub

End Class