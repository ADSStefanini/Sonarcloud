Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo.Entidades

<TestClass()> Public Class EstadoProcesoGestorTest

    <TestMethod()> Public Sub ObtenerEstadosProcesoGestor()
        Dim estadoProcesoBll As New CobrosCoactivo.Logica.EstadoProcesoGestorBLL
        Assert.IsTrue(estadoProcesoBll.ObtenerEstadoProcesoGestor().Count > 0)
    End Sub
    <TestMethod()>
    Public Sub GuardarUpdateTest()
        Dim estadoProcesal As New CobrosCoactivo.Entidades.EstadoProcesoGestor
        Dim estadoProcesoBll As New CobrosCoactivo.Logica.EstadoProcesoGestorBLL
        estadoProcesal.COD_ID_ESTADOS_PROCESOS = "01"
        estadoProcesal.IND_ESTADO = False
        estadoProcesal.VAL_USUARIO = "AAVILA"
        Assert.IsTrue(estadoProcesoBll.Guardar(estadoProcesal))
    End Sub

    <TestMethod()>
    Public Sub GuardarSaveTest()
        Dim estadoProcesal As New CobrosCoactivo.Entidades.EstadoProcesoGestor
        Dim estadoProcesoBll As New CobrosCoactivo.Logica.EstadoProcesoGestorBLL
        estadoProcesal.COD_ID_ESTADOS_PROCESOS = "02"
        estadoProcesal.IND_ESTADO = False
        estadoProcesal.VAL_USUARIO = "AAVILA"
        Assert.IsTrue(estadoProcesoBll.Guardar(estadoProcesal))
    End Sub
End Class