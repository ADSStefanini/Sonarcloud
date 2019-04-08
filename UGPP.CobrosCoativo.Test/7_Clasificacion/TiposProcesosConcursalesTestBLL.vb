Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class TiposProcesosConcursalesTestBLL

    <TestMethod()> Public Sub ObtenerTipoProcesoConcursalNaturalSucess()
        Dim tipoProcesosConcursales As TiposProcesosConcursalesBLL = New TiposProcesosConcursalesBLL
        Dim cod As Int32 = 6
        Dim NumProceso As Int32 = 3
        Dim lista = tipoProcesosConcursales.ObtenerTipoProcesoNatural()
    End Sub

End Class