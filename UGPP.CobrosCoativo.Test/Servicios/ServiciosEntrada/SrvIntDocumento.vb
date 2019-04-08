Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class SrvIntDocumento

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
    Public Sub OpIngresarDocumento()
        Dim servicio As New Servicios.SrvIntDocumento

        Dim arrayDocumentos(0) As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.DocumentoTipo
        arrayDocumentos(0) = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.DocumentoTipo
        arrayDocumentos(0).idDocumento = "" ' ENVIAR EN BLANCO
        arrayDocumentos(0).valNombreDocumento = "Documento1.xlsx"
        arrayDocumentos(0).valAutorOriginador = "usuario"
        arrayDocumentos(0).fecDocumento = DateTime.Now.ToString("dd/MM/yyyy")
        arrayDocumentos(0).valPaginas = "1"
        arrayDocumentos(0).valNaturalezaDocumento = "COPIA SIMPLE"
        arrayDocumentos(0).descObservacionLegibilidad = "LEGIBLE"
        arrayDocumentos(0).valOrigenDocumento = "FISICO"
        arrayDocumentos(0).codTipoDocumento = "357"
        arrayDocumentos(0).numFolios = "0"

        arrayDocumentos(0).docDocumento = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.ArchivoTipo
        arrayDocumentos(0).docDocumento.valNombreArchivo = "Documento1.xlsx"
        arrayDocumentos(0).docDocumento.codTipoMIMEArchivo = ".xlsx"
        arrayDocumentos(0).docDocumento.valContenidoArchivo = Util.ReadFileAsArray("Documento1.xlsx")

        Dim arrayAgrupadores(1) As String
        arrayAgrupadores(0) = "987654321"
        arrayAgrupadores(1) = "EX"
        arrayDocumentos(0).metadataDocumento = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.MetadataDocumentoTipo
        arrayDocumentos(0).metadataDocumento.valAgrupador = arrayAgrupadores

        Dim param As New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoSolTipo
        param.expediente = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.ExpedienteTipo()
        param.expediente.idNumExpediente = "20171100012000003"
        param.expediente.valFondo = "900"
        param.expediente.valEntidadPredecesora = "900"
        param.documentos = arrayDocumentos

        param.correspondencia = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.CorrespondenciaTipo
        param.correspondencia.codEntidadOriginadora = "900"

        For Each item In param.documentos
            Dim blankArray(0) As Byte
            If (item.docDocumento.valContenidoFirma Is Nothing) Then
                item.docDocumento.valContenidoFirma = blankArray
            End If
        Next

        Dim result As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo = servicio.OpIngresarDocumento(param)
        Assert.IsTrue(result IsNot Nothing)
    End Sub

    <TestMethod()>
    Public Sub OpConsultarDocumento()
        Dim servicio As New Servicios.SrvIntDocumento

        Dim param As New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoSolTipo
        param.documento = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.DocumentoTipo
        param.documento.idDocumento = "{2891A005-352D-4052-945D-1181F1E62494}"

        Dim result As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo = servicio.ConsultarDcoumento(param)
        Assert.IsTrue(result IsNot Nothing)
    End Sub

End Class
