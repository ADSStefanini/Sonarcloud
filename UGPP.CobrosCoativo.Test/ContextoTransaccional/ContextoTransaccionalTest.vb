Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo.Entidades
<TestClass()> Public Class ContextoTransaccionalTest

    <TestMethod()> Public Sub TestContextoTransaccional()
        Dim bllContext As New ContextoTransaccionalBLL
        Dim context As ContextoTransaccion
        context = bllContext.ObtenerContextoPorIdTitulo(15622)
        Assert.AreEqual(context.ID_TITULO, 15622)
    End Sub

End Class