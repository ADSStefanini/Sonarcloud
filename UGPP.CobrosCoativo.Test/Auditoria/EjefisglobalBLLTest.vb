Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class EjefisglobalBLLTest

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

    Dim ejefis As EjefisglobalBLL = New EjefisglobalBLL(getLogAuditoria())

    <TestMethod()> Public Sub actualizarExpediente()
        Dim res As Boolean = ejefis.ActualizarExpediente(80001, "11")
        Assert.AreEqual(res, True)
    End Sub

    <TestMethod()> Public Sub actualizarExpedienteEstapaProcesal()
        Dim res As Boolean = ejefis.ActualizarExpedienteEtapaProcesal(80001, "11", 8)
        Assert.AreEqual(res, True)
    End Sub

End Class