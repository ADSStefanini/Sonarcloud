Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class PerfilPaginaBLLTest

    <TestMethod()> Public Sub obtenerPermisosPorPaginaGood()
        Dim PerfilPaginaBLL As PerfilPaginaBLL = New PerfilPaginaBLL()
        Dim test = PerfilPaginaBLL.obtenerPermisosPorPagina(4, 1)
        Assert.AreEqual(test.ind_puede_ver, Entidades.Enumeraciones.Estado.ACTIVO)
    End Sub

    <TestMethod()> Public Sub obtenerPermisosPorPaginaWrong()
        Dim PerfilPaginaBLL As PerfilPaginaBLL = New PerfilPaginaBLL()
        Dim test = PerfilPaginaBLL.obtenerPermisosPorPagina(4, -1)
        Assert.AreEqual(test.ind_puede_ver, Entidades.Enumeraciones.Estado.INACTIVO)
    End Sub

End Class