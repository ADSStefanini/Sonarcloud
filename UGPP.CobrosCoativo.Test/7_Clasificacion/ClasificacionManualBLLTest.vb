Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class ClasificacionManualBLLTest

    <TestMethod()> Public Sub ClasificacionPorCuantia()
        Dim _clasificacionManualBLL As New ClasificacionManualBLL()
        Dim res = _clasificacionManualBLL.ClasificacionPorCuantia("81228")
        Assert.IsTrue(res)
    End Sub

    <TestMethod()> Public Sub EstadoProcesalCarteraIncobrable()
        Dim _clasificacionManualBLL As New ClasificacionManualBLL()
        Dim res = _clasificacionManualBLL.EstadoProcesalCarteraIncobrable("81228", "2")
        Assert.IsTrue(res)
    End Sub

    <TestMethod()> Public Sub EstadoProcesalDificilCobro()
        Dim _clasificacionManualBLL As New ClasificacionManualBLL()
        Dim res = _clasificacionManualBLL.EstadoProcesalDificilCobro("81228", "2")
        Assert.IsTrue(res)
    End Sub

    <TestMethod()> Public Sub ClasificacionPorTipoProcesoJuridico()
        Dim Res As Boolean = False
        Dim idExpediente = "81228"
        Dim valDdlTipoProcesoJuridica = "4"
        Dim valDdlBeneficioTributario = "1"
        Dim valueddlTipoProcesoJuridica = valDdlTipoProcesoJuridica
        Dim valueddlBeneficioTributario = valDdlBeneficioTributario
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim valEstadoProcPersuasivo = My.Resources.ValEstadoProcesoPersuasivo
        Dim valEstadoProcConcursal = My.Resources.ValEstadoProcesoConcursal
        Dim valEstadoProcesoCoactivo = My.Resources.ValEstadoProcesoCoactivo
        Dim valEtapaBeneficioTributario = Convert.ToInt32(My.Resources.ValEtapaBenTributario)
        If valueddlTipoProcesoJuridica = My.Resources.ValTiProcLiqVoluntaria And valueddlBeneficioTributario = My.Resources.ValPositivoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcPersuasivo, valEtapaBeneficioTributario)
            Res = True
        Else
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcConcursal, valEtapaBeneficioTributario)
            Res = True
        End If
        If valueddlTipoProcesoJuridica = My.Resources.ValTiProcLiqVoluntaria Then
            expedientes.ActualizarExpediente(idExpediente, valEstadoProcesoCoactivo)
            Res = True
        ElseIf valueddlTipoProcesoJuridica <> My.Resources.ValTiProcLiqVoluntaria And valueddlTipoProcesoJuridica <> My.Resources.DefaultValueDdlTipoProceso Then
            expedientes.ActualizarExpediente(idExpediente, valEstadoProcConcursal)
            Res = True
        End If
        Assert.IsTrue(Res)
    End Sub

    <TestMethod()> Public Sub clasificacionBeneficioTributario()
        Dim Res As Boolean = False
        Dim idExpediente = "81228"
        Dim valDdlBeneficioTributario = "1"
        Dim valDdlTipoProcesoJuridica = "4"
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim valueddlBeneficioTributario = valDdlBeneficioTributario
        Dim valEstadoProcPersuasivo = My.Resources.ValEstadoProcesoPersuasivo
        Dim valEstadoProcConcursal = My.Resources.ValEstadoProcesoConcursal
        Dim valEtapaBeneficioTributario = Convert.ToInt32(My.Resources.ValEtapaBenTributario)
        If valDdlTipoProcesoJuridica = My.Resources.ValTiProcLiqVoluntaria And valueddlBeneficioTributario = My.Resources.ValPositivoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcConcursal, valEtapaBeneficioTributario)
            Res = True
        ElseIf valueddlBeneficioTributario = My.Resources.ValNegativoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcPersuasivo, valEtapaBeneficioTributario)
            Res = True
        End If
        Assert.IsTrue(Res)
    End Sub

    <TestMethod()> Public Sub clasificacionDeudorRealizaPagos()
        Dim Res As Boolean = False
        Dim idExpediente = "81228"
        Dim valDdlPagosDeudor = "1"
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim valueddlPagosDeudor = valDdlPagosDeudor
        Dim valEstadoProcVerifPagos = My.Resources.ValEstadoProcesoVerifPagos
        Dim valEtapaNormalizacion = My.Resources.ValEtapaNormalizacion
        If valueddlPagosDeudor = My.Resources.ValPositivoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcVerifPagos, valEtapaNormalizacion)
            Res = True
        End If
        Assert.IsTrue(Res)
    End Sub

    <TestMethod()> Public Sub ClasificacionPorTipoProcesoNatural()
        Dim test As New ClasificacionManualBLL
        Dim Res As Boolean = False
        Dim idExpediente = "81228"
        Dim valDdlTipoProcesoNatural = "1"
        Dim valueddlTipoProcesoNatural = valDdlTipoProcesoNatural
        Dim valEstadoProcConcursal = My.Resources.ValEstadoProcesoConcursal
        If valueddlTipoProcesoNatural <> My.Resources.DefaultValueDdlTipoProceso Then
            test.EstablecerEstadoProcesal(idExpediente, valEstadoProcConcursal)
            Res = True
        End If
        Assert.IsTrue(Res)
    End Sub

    <TestMethod()> Public Sub EstablecerEstadoProcesal()

        Dim _clasificacionManualBLL As New ClasificacionManualBLL()
        Dim idExpediente = "81228"
        Dim valEstadoProcConcursal = "6"
        Dim res = _clasificacionManualBLL.EstablecerEstadoProcesal(idExpediente, valEstadoProcConcursal)
        Assert.IsTrue(res)
    End Sub
End Class