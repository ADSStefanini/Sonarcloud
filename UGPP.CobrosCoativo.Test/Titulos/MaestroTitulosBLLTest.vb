Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class MaestroTitulosBLLTest

    <TestMethod()> Public Sub consultarTituloPorID()
        Dim _maestroTitulosBLL As New MaestroTitulosBLL()
        Dim titulo = _maestroTitulosBLL.consultarTituloPorID(1)
        Assert.IsTrue(titulo.idunico = 1)
    End Sub

    <TestMethod()> Public Sub consultarTituloPorIDs()
        Dim _maestroTitulosBLL As New MaestroTitulosBLL()
        Dim lista As New List(Of Integer)(New Integer() {2, 3, 5})
        'Dim listaTitulos = _maestroTitulosBLL.consultarTituloPorIDs(lista)
        'Assert.IsTrue(listaTitulos.Count > 0)
    End Sub

End Class