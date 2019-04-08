Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo

<TestClass()> Public Class DocumentoTituloTipoTituloTest

    <TestMethod()>
    Public Sub ObtenerTipos()
        Dim tipos As List(Of Entidades.DocumentoTituloTipoTitulo)
        tipos = New Logica.DocumentoTituloTipoTituloBLL().obtenerDocumentoTituloTipoTitulo()
        Assert.IsTrue(tipos.Count > 2)
    End Sub


    <TestMethod()>
    Public Sub ObtenerdocumentosTitulo()
        Dim tipos As List(Of Entidades.DocumentoTitulo)
        tipos = New Logica.DocumentoTituloTipoTituloBLL().obtenerDocumentosTitulo()
        Assert.IsTrue(tipos.Count > 2)
    End Sub

    <TestMethod()>
    Public Sub ObtenertiposTitulo()
        Dim tipos As List(Of Entidades.TipoTitulo)
        tipos = New Logica.DocumentoTituloTipoTituloBLL().obtenerTiposTitulo()
        Assert.IsTrue(tipos.Count > 2)
    End Sub


    <TestMethod()>
    Public Sub Salvar()
        Dim result As Boolean = False
        result = New Logica.DocumentoTituloTipoTituloBLL().salvar(New Entidades.DocumentoTituloTipoTitulo With {.COD_TIPO_TITULO = "02", .ID_DOCUMENTO_TITULO = 10, .VAL_ESTADO = True, .VAL_OBLIGATORIO = True})
        Assert.IsTrue(result)
    End Sub
End Class