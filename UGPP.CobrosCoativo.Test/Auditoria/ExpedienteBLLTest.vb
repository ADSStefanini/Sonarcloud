Imports coactivosyp
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class ExpedienteBLLTest

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

    <TestMethod()> Public Sub asignarExpedientePorRepartir()
        Dim expedienteBLL As ExpedienteBLL = New ExpedienteBLL(getLogAuditoria())
        expedienteBLL.asignarExpedientePorRepartir("2")
    End Sub

    <TestMethod()> Public Sub crearExpediente()
        Dim _expedientesUtils As ExpedientesUtils
        _expedientesUtils = New ExpedientesUtils(15555, "UnitTest")
        '_expedientesUtils.iniciarInstaciaExpediente()
        _expedientesUtils.SincronizarDocumentosTitulos(15555)
    End Sub

End Class