Imports System.Text
Imports coactivosyp
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class ExpedientesTest

    <TestMethod()> Public Sub CrearExpediente()
        Dim _expedientesUtils As ExpedientesUtils
        _expedientesUtils = New ExpedientesUtils(15555, "UnitTest")
        '_expedientesUtils.iniciarInstaciaExpediente()
        '_expedientesUtils.SincronizarDocumentosTitulos(15555)
    End Sub

    <TestMethod()> Public Sub FiltrarExpedientesAsignados()
        'Creación del objeto para filtro
        Dim _consultaExpedientes As New ConsultaExpedientes()
        _consultaExpedientes.StartRecord = 1
        _consultaExpedientes.StopRecord = 10
        _consultaExpedientes.SortExpression = String.Empty
        _consultaExpedientes.SortDirection = String.Empty
        _consultaExpedientes.UsuarioLogin = String.Empty
        _consultaExpedientes.NumeroExpediente = String.Empty
        _consultaExpedientes.NombreDeudor = String.Empty
        _consultaExpedientes.NumeroDocDeudor = String.Empty
        _consultaExpedientes.CodEstadoProcesal = String.Empty
        _consultaExpedientes.CodTipoTitulo = String.Empty
        _consultaExpedientes.FechaEntragaTitulo = String.Empty
        _consultaExpedientes.FechaAsignacionGestor = String.Empty
        _consultaExpedientes.EstadoOperativo = 0
        'Creación de objeto que realiza la consulta
        Dim _expedienteDAL As New ExpedienteBLL()
        Dim _expedientes = _expedienteDAL.obtenerExpedientesAsignados(_consultaExpedientes)
        Assert.IsTrue(_expedientes.Rows.Count() > 0)
    End Sub

    <TestMethod()> Public Sub SincronizarDocsTituloAutomatico()
        Dim _expedientesUtils As ExpedientesUtils
        _expedientesUtils = New ExpedientesUtils(18884, "UnitTest")
        _expedientesUtils.iniciarInstaciaExpediente()
        _expedientesUtils.SincronizarDocumentosTitulos()
    End Sub


End Class