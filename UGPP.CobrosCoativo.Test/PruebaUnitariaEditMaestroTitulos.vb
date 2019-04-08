Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

<TestClass()>
Public Class PruebaUnitariaEditMaestroTitulos

    Private TipoAreaOrigenDAL As TipoAreaOrigenDAL = New TipoAreaOrigenDAL()
    Private TipoCarteraDAL As TipoCarteraDAL = New TipoCarteraDAL()
    Private DocumentoTipoTituloDAL As DocumentoTipoTituloDAL = New DocumentoTipoTituloDAL()
    Private ObtenerDatosValoresDAL As ObtenDatosValoresDAL = New ObtenDatosValoresDAL()

    <TestMethod()>
    Public Sub LoadcboPROCEDENCIASuccess()
        Dim resultado As List(Of TipoAreaOrigen) = New List(Of TipoAreaOrigen)
        resultado = TipoAreaOrigenDAL.ConsultarAreaOrigen()
        Assert.IsNotNull(resultado)
    End Sub

    <TestMethod()>
    Public Sub consultarTiposCarteraSuccess()
        Dim codAreaOrigen As Int32 = 12
        Dim resultado As List(Of TipoCartera) = New List(Of TipoCartera)
        resultado = TipoCarteraDAL.consultarTiposCartera(codAreaOrigen)
        Assert.IsNotNull(resultado)
    End Sub

    <TestMethod()>
    Public Sub consultarDocumentosPorTipoSuccess()
        Dim codTipoTitulo As String = "Auto liquida las costas"
        Dim resultado As List(Of DocumentoTipoTitulo) = New List(Of DocumentoTipoTitulo)
        resultado = DocumentoTipoTituloDAL.consultarDocumentosPorTipo(codTipoTitulo)
        Assert.IsNotNull(resultado)
    End Sub

    <TestMethod()>
    Public Sub ConsultaDatValoresSuccess()
        Dim resultado As List(Of Valores)
        resultado = ObtenerDatosValoresDAL.ConsultaDatValores()
        Assert.IsNotNull(resultado)
    End Sub
End Class
