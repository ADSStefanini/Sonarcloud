Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class HistoricoClasificacionManualBLLTest

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

    Dim cumpleNoCumpleBLL As ObservacionCumpleBLL = New ObservacionCumpleBLL(getLogAuditoria())
    Dim moduloBLL As ModuloBLL = New ModuloBLL(getLogAuditoria())
    Dim maestroTituloBLL As MaestroTitulosDocumentosBLL = New MaestroTitulosDocumentosBLL(getLogAuditoria())

    <TestMethod()> Public Sub salvar()
        Dim historicoBLL As HistoricoClasificacionManualBLL = New HistoricoClasificacionManualBLL(getLogAuditoria())
        Dim historicoClasificacion As HistoricoClasificacionManual = New HistoricoClasificacionManual()
        historicoClasificacion.ID_REGISTRO_CLASIFICACION_MANUAL = 12
        historicoClasificacion.ID_EXPEDIENTE = "12"
        historicoClasificacion.ID_USUARIO = "12"
        historicoClasificacion.FECHA = Date.Now
        historicoClasificacion.PERSONA_JURIDICA = True
        historicoClasificacion.PERSONA_NATURAL = True
        historicoClasificacion.PERSONA_VIVA = True
        historicoClasificacion.MATRICULA_MERCANTIL = True
        historicoClasificacion.ID_MTD_DOCUMENTO = 1234
        historicoClasificacion.PROCESO_ESPECIAL = False
        historicoClasificacion.TIPO_PROCESO = 1
        historicoClasificacion.BENEFICIO_TRIBUTARIO = False
        historicoClasificacion.PAGOS_DEUDOR = True
        historicoClasificacion.NUMERO_RADICADO = 12
        historicoClasificacion.OBSERVACIONES = "hola"
        historicoClasificacion.VALOR_MENOR_UVT = True
        Dim res As Boolean
        res = historicoBLL.Salvar(historicoClasificacion)
        Assert.IsTrue(res)
    End Sub

    <TestMethod()> Public Sub InsertaJustificacionCierre()
        Dim insertaJustficacionBLL As InsertaJustificacionBLL = New InsertaJustificacionBLL(getLogAuditoria())
        insertaJustficacionBLL.InsertaJustificacionCierre(1, "sali")
    End Sub

    <TestMethod()> Public Sub asignarExpedienteATitulo()
        Dim maestroTitulo As MaestroTitulosBLL = New MaestroTitulosBLL(getLogAuditoria())
        Dim maestroTitulos As MaestroTitulos = New MaestroTitulos
        maestroTitulos = maestroTitulo.asignarExpedienteATitulo(12, 12)
        Assert.IsNotNull(maestroTitulos)
    End Sub

    <TestMethod()> Public Sub actualizarMaestroTitulosDocumentos()
        Dim maestroTitulo As MaestroTitulosDocumentos = New MaestroTitulosDocumentos
        maestroTitulo.ID_MAESTRO_TITULOS_DOCUMENTOS = 1
        maestroTitulo.ID_DOCUMENTO_TITULO = 4
        maestroTitulo.ID_MAESTRO_TITULO = 15555
        maestroTitulo.DES_RUTA_DOCUMENTO = "cod/ugpp/fiction"
        maestroTitulo.TIPO_RUTA = 123
        maestroTitulo.COD_GUID = "273782"
        maestroTitulo.COD_TIPO_DOCUMENTO_AO = "12454"
        maestroTitulo.NOM_DOC_AO = "12345"
        maestroTitulo.OBSERVA_LEGIBILIDAD = "silo"
        maestroTitulo.NUM_PAGINAS = 12
        maestroTitulo.IND_DOC_SINCRONIZADO = False
        Dim res As MaestroTitulosDocumentos = maestroTituloBLL.ActualizarMaestroTitulosDocumentos(maestroTitulo)
        Assert.IsNotNull(res)
    End Sub

    <TestMethod()> Public Sub crearMaestroTitulosDocumentos()
        Dim parametroMaestroTitulos As MaestroTitulosDocumentos = New MaestroTitulosDocumentos
        parametroMaestroTitulos.ID_MAESTRO_TITULOS_DOCUMENTOS = 1
        parametroMaestroTitulos.ID_DOCUMENTO_TITULO = 4
        parametroMaestroTitulos.ID_MAESTRO_TITULO = 15555
        parametroMaestroTitulos.DES_RUTA_DOCUMENTO = "cod/ugpp/fiction"
        parametroMaestroTitulos.TIPO_RUTA = 123
        parametroMaestroTitulos.COD_GUID = "1223344"
        parametroMaestroTitulos.COD_TIPO_DOCUMENTO_AO = "3234242"
        parametroMaestroTitulos.NOM_DOC_AO = "3433"
        parametroMaestroTitulos.OBSERVA_LEGIBILIDAD = "jkdjlsk"
        parametroMaestroTitulos.NUM_PAGINAS = 12
        parametroMaestroTitulos.IND_DOC_SINCRONIZADO = True
        Dim res As MaestroTitulosDocumentos = maestroTituloBLL.crearMaestroTitulosDocumentos(parametroMaestroTitulos)
        Assert.IsNotNull(res)
    End Sub

    <TestMethod()> Public Sub ActualizarAccesoModulo()
        Dim res As PerfilModulo = moduloBLL.actualizarAccesoModulo(12, 2, True)
        Assert.IsNotNull(res)
    End Sub

    <TestMethod()> Public Sub ActualizarModulo()
        Dim res As Modulo = moduloBLL.actualizarModulo(1, "cualquier", "hsjhjk/hgjds", "dhjsjj", False)
        Assert.IsNotNull(res)
    End Sub

    <TestMethod()> Public Sub GuardarModulo()
        Dim res As Modulo = moduloBLL.guardarModulo("seguridad", "jklfjdskl/kjhjkds", "fjdhsk/jksdfdf", False)
        Assert.IsNotNull(res)
    End Sub

    <TestMethod()> Public Sub insertaCNCCComentario()
        cumpleNoCumpleBLL.InsertaCNCComentarioC(12, "pedro", "si cumple", "marcos", True)
    End Sub

    <TestMethod()> Public Sub insertaCNNCComentarioDocC()
        cumpleNoCumpleBLL.InsertaCNCComentarioDocC(12, 12, "jorge", "pedro", True, "si cumple")
    End Sub

    <TestMethod()> Public Sub insertaCNCTificacionC()
        cumpleNoCumpleBLL.InsertaCNCTipificacionC(12, 13, "jorge")
    End Sub
End Class