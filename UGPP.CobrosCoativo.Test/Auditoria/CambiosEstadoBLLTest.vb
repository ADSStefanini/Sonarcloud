Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class CambiosEstadoBLLTest

    Function getLogAuditoria() As LogAuditoria
        Dim log As LogAuditoria = New LogAuditoria
        log.LOG_USER_ID = "Harry"
        log.LOG_USER_CC = String.Empty
        log.LOG_HOST = "Rafael"
        log.LOG_IP = "127.0.0.1"
        log.LOG_FECHA = Date.Now
        log.LOG_APLICACION = "Coactivos SyP v20140223.0831"
        log.LOG_MODULO = "Actualizacion"
        log.LOG_DOC_AFEC = "nombre"
        log.LOG_CONSULTA = "Insert"
        log.LOG_CONSE = 28
        log.LOG_NEGOCIO = Nothing
        Return log
    End Function

    <TestMethod()> Public Sub guardarCambiosEstado()
        Dim cambiosEstado As CambiosEstado = New CambiosEstado
        cambiosEstado.idunico = 1
        cambiosEstado.NroExp = "80001"
        cambiosEstado.repartidor = "0017"
        cambiosEstado.revisor = Nothing
        cambiosEstado.abogado = "0011"
        cambiosEstado.fecha = Date.Now
        cambiosEstado.estado = "02"
        cambiosEstado.estadopago = "05"
        cambiosEstado.estadooperativo = 1
        cambiosEstado.etapaprocesal = 1
        Dim resultado As CambiosEstado = New CambiosEstado
        Dim cambiosEstadoBLL As CambiosEstadoBLL = New CambiosEstadoBLL(getLogAuditoria())
        resultado = cambiosEstadoBLL.guardarCambiosEstado(cambiosEstado)
        Assert.IsNotNull(resultado)
    End Sub

End Class