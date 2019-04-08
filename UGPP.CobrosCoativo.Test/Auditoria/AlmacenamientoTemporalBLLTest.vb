Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class AlmacenamientoTemporalBLLTest

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

    Dim almacenamientoTemporalBLL As AlmacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(getLogAuditoria())

    <TestMethod()> Public Sub actualizarAlmacenamientoSucess()
        Dim almacenamiento As AlmacenamientoTemporal = New AlmacenamientoTemporal()
        almacenamiento.FEC_ACTUALIZACION = Date.Now
        almacenamiento.JSON_OBJ = ""
        almacenamiento.ID_TAREA_ASIGNADA = 428
        almacenamientoTemporalBLL.actualizarAlmacenamiento(almacenamiento)
    End Sub

    <TestMethod()> Public Sub insertarAlmacenamiento()
        Dim almacenamiento As AlmacenamientoTemporal = New AlmacenamientoTemporal()
        almacenamiento.ID_ALMACENAMIENTO_TEMPORAL = 344
        almacenamiento.ID_TAREA_ASIGNADA = 428
        almacenamiento.JSON_OBJ = ""
        almacenamiento.FEC_ACTUALIZACION = Date.Now
        Dim resultado As AlmacenamientoTemporal = New AlmacenamientoTemporal
        resultado = almacenamientoTemporalBLL.InsertarAlmacenamiento(almacenamiento)
        Assert.IsNotNull(resultado)
    End Sub
End Class