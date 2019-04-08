Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class PaginaBLLTest

    <TestMethod()> Public Sub textGuardarPerfilWrong()
        Dim paginaBLL As PaginaBLL = New PaginaBLL()
        'Ok
        Dim res As List(Of Entidades.Pagina) = paginaBLL.obtenerPaginasPorPerfilPuedeVer(2)
        Assert.Equals(0, res.Count())
    End Sub

    <TestMethod()> Public Sub obtenerPaginasPorPerfil()
        Dim paginaBLL As PaginaBLL = New PaginaBLL()
        Dim test = paginaBLL.obtenerPaginasPorPerfil(4, 0)
        Assert.IsTrue(test.Count > 0)
    End Sub

End Class