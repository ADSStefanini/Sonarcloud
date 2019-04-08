Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class UsuarioBLLTest

    <TestMethod()> Public Sub obtenerUsuarioPorId()
        Dim _usuarioBLL As UsuariosBLL = New UsuariosBLL()
        Dim codigo = "0004"
        Dim test = _usuarioBLL.obtenerUsuarioPorId(codigo)
        Assert.IsTrue(test.codigo = codigo)
    End Sub

    <TestMethod()> Public Sub obtenerUsuarioSuperior()
        Dim _usuarioBLL As UsuariosBLL = New UsuariosBLL()
        Dim codigo = "0004"
        Dim test = _usuarioBLL.obtenerUsuarioSuperior(codigo)
        Assert.IsTrue(Not String.IsNullOrWhiteSpace(test.codigo))
    End Sub

    <TestMethod()> Public Sub obtenerUsuarioSuperioraVacio()
        Dim _usuarioBLL As UsuariosBLL = New UsuariosBLL()
        Dim codigo = "0001"
        Dim test = _usuarioBLL.obtenerUsuarioSuperior(codigo)
        Assert.IsTrue(String.IsNullOrWhiteSpace(test.codigo))
    End Sub

End Class