Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo

<TestClass()> Public Class ModuloBLLTest
    Dim ModuloBll As ModuloBLL = New ModuloBLL()

    <TestMethod()> Public Sub ConvertirEntidadModuloSucess()
        Dim DatoModulo As Datos.MODULO = New Datos.MODULO()
        DatoModulo.pk_codigo = 1
        DatoModulo.val_nombre = "pedro"
        DatoModulo.val_url = "www.url.com"
        DatoModulo.val_url_icono = "www.urlIcono.com"
        DatoModulo.ind_estado = True
        Dim test = ModuloBll.ConvertirEntidadModulo(DatoModulo)
        Assert.IsTrue(test.pk_codigo <> 0)
    End Sub
    <TestMethod()> Public Sub ConvertirEntidadModuloFail()
        Dim DatoModulo As Datos.MODULO = New Datos.MODULO()
        DatoModulo.pk_codigo = Nothing
        DatoModulo.val_nombre = "pedro"
        DatoModulo.val_url = "www.url.com"
        DatoModulo.val_url_icono = "www.urlIcono.com"
        DatoModulo.ind_estado = True
        Dim test = ModuloBll.ConvertirEntidadModulo(DatoModulo)
        Assert.IsFalse(test.pk_codigo <> 0)
    End Sub

    <TestMethod()> Public Sub ObtenerModulosSucess()
        Dim prmStrTextoBusqueda = "Seguridad"
        Dim modulos As List(Of Entidades.Modulo) = New List(Of Entidades.Modulo)
        modulos = ModuloBll.obtenerModulos(prmStrTextoBusqueda)
        Assert.IsTrue(modulos(0).pk_codigo <> 0)
    End Sub
    <TestMethod()> Public Sub ObtenerModulosFail()
        Dim prmStrTextoBusqueda = ""
        Dim modulos As List(Of Entidades.Modulo) = New List(Of Entidades.Modulo)
        modulos = ModuloBll.obtenerModulos(prmStrTextoBusqueda)
        Assert.IsFalse(modulos(0).pk_codigo = Nothing)
    End Sub

    <TestMethod()> Public Sub ObtenerModulosActivosSucess()
        Dim modulos As List(Of Entidades.Modulo) = New List(Of Entidades.Modulo)
        modulos = ModuloBll.obtenerModulosActivos()
        Assert.IsTrue(modulos(0).pk_codigo <> 0)
    End Sub
    <TestMethod()> Public Sub ObtenerModulosActivosFail()
        Dim modulos As List(Of Entidades.Modulo) = New List(Of Entidades.Modulo)
        modulos = ModuloBll.obtenerModulosActivos()
        Assert.IsFalse(modulos(0).pk_codigo = 0)
    End Sub

    <TestMethod()> Public Sub ObtenerModuloPorIdSucess()
        Dim numero As Long = 1
        Dim moduloPorId As Entidades.Modulo = New Entidades.Modulo
        moduloPorId = ModuloBll.obtenerModuloPorId(numero)
        Assert.IsTrue(moduloPorId.pk_codigo <> 0)
    End Sub
    <TestMethod()> Public Sub ObtenerModuloPorIdFail()
        Dim numero = Nothing
        Dim moduloPorId As Entidades.Modulo = New Entidades.Modulo
        moduloPorId = ModuloBll.obtenerModuloPorId(numero)
        Assert.IsNotNull(moduloPorId)
    End Sub

    <TestMethod()> Public Sub GuardarModuloSucess()
        Dim modulo As Modulo = New Modulo()
        Dim val_nombre = "Orlando"
        Dim val_url = "www.wwwww.com"
        Dim val_url_icono = "Fileupload"
        Dim ind_estado = True
        modulo = ModuloBll.guardarModulo(val_nombre, val_url, val_url_icono, ind_estado)
        Assert.IsTrue(modulo.pk_codigo <> 0)
    End Sub
    <TestMethod()> Public Sub GuardarModuloFail()
        Dim modulo As Modulo = New Modulo()
        Dim val_nombre As String = "Maria"
        Dim val_url As String = "www.google.com"
        Dim val_url_icono As String = "iconfileupload"
        Dim ind_estado As Boolean = False
        modulo = ModuloBll.guardarModulo(val_nombre, val_url, val_url_icono, ind_estado)
        Assert.IsFalse(modulo.pk_codigo = Nothing)
    End Sub

    <TestMethod()> Public Sub ActualizarModuloSucess()
        Dim modulo As Entidades.Modulo = New Entidades.Modulo()
        Dim prmModuloId As Int32 = 1
        Dim prmNombreModulo As String = "Seguridad"
        Dim prmUrlModulo As String = "/Security/admin/adminAccesoPerfiles.aspx"
        Dim prmUrlIconoModulo As String = "/Security/images/icons/Keys.png"
        Dim prmBoolEstado As Boolean = True
        modulo = ModuloBll.actualizarModulo(prmModuloId, prmNombreModulo, prmUrlModulo, prmUrlIconoModulo, prmBoolEstado)
        Assert.IsTrue(modulo.pk_codigo <> 0)
    End Sub
    <TestMethod()> Public Sub ActualizarModuloFail()
        Dim modulo As Modulo = New Modulo()
        Dim prmModuloId As Int32 = 1
        Dim prmNombreModulo As String = "Cualquier"
        Dim prmUrlModulo As String = "www.gestion.com"
        Dim prmUrlIconoModulo As String = "gestion.ico"
        Dim prmBoolEstado As Boolean = True
        modulo = ModuloBll.actualizarModulo(prmModuloId, prmNombreModulo, prmUrlModulo, prmUrlIconoModulo, prmBoolEstado)
        Assert.IsNotNull(modulo)
    End Sub

    <TestMethod()> Public Sub ObtenerModulosPorPerfilSucess()
        Dim modulos As List(Of Modulo) = New List(Of Modulo)
        Dim prmIntPerfil As Int32 = 1
        modulos = ModuloBll.obtenerModulosPorPerfil(prmIntPerfil)
        Assert.IsTrue(modulos(0).pk_codigo <> 0)
    End Sub
    <TestMethod()> Public Sub ObtenerModulosPorPerfilFail()
        Dim modulos As List(Of Modulo) = New List(Of Modulo)
        Dim prmIntPerfil As Int32 = 123456
        modulos = ModuloBll.obtenerModulosPorPerfil(prmIntPerfil)
        Assert.IsNotNull(modulos)
    End Sub

    <TestMethod()> Public Sub ActualizarAccesoModuloSucess()
        Dim modelo As PerfilModulo = New PerfilModulo()
        Dim prmIntPerfilId As Int32 = 12
        Dim prmIntModuloId As Int32 = 45
        Dim prmBoolEstadoAcceso As Boolean = False
        modelo = ModuloBll.actualizarAccesoModulo(prmIntPerfilId, prmIntModuloId, prmBoolEstadoAcceso)
        Assert.IsTrue(modelo.fk_modulo_id <> 0)
    End Sub
    <TestMethod()> Public Sub ActualizarAccesoModuloFail()
        Dim modelo As PerfilModulo = New PerfilModulo()
        Dim prmIntPerfilId As Int32 = 12
        Dim prmIntModuloId As Int32 = 45
        Dim prmBoolEstadoAcceso As Boolean = False
        modelo = ModuloBll.actualizarAccesoModulo(prmIntPerfilId, prmIntModuloId, prmBoolEstadoAcceso)
        Assert.IsFalse(modelo.fk_modulo_id = 0)
    End Sub

End Class
