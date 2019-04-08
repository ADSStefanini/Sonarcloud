Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo.Entidades

<TestClass()> Public Class HistoricoClasificacionManualTest

    <TestMethod()>
    Public Sub SalvarHistoricoTestSucess()
        Dim negocio As New HistoricoClasificacionManualBLL
        Dim dato As New HistoricoClasificacionManual
        dato.ID_EXPEDIENTE = "80001"
        dato.ID_USUARIO = "0001"
        dato.FECHA = DateTime.Now
        dato.PERSONA_JURIDICA = True
        dato.PERSONA_NATURAL = True
        dato.PERSONA_VIVA = True
        dato.MATRICULA_MERCANTIL = True
        dato.ID_MTD_DOCUMENTO = Nothing 'Se deja como Nothing para probar el eliminado de la relación de la tabla
        dato.PROCESO_ESPECIAL = True
        dato.TIPO_PROCESO = 10
        dato.BENEFICIO_TRIBUTARIO = True
        dato.PAGOS_DEUDOR = True
        dato.NUMERO_RADICADO = 121
        dato.OBSERVACIONES = "Prueba de guardado desde prueba unitaria"
        dato.VALOR_MENOR_UVT = False
        Assert.IsTrue(negocio.Salvar(dato))
    End Sub


    <TestMethod()>
    Public Sub ObtenerHistoricoTestSucess()
        Dim negocio As New HistoricoClasificacionManualBLL
        Assert.IsTrue(negocio.ObtenerHistoricoPorIdExpediente("80001").Count > 0)
    End Sub

End Class