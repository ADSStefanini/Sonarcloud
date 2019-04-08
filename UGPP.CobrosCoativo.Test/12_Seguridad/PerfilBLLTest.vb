Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica

<TestClass()> Public Class PerfilBLLTest

    Dim PerfilBLL As PerfilBLL = New PerfilBLL()

    <TestMethod()> Public Sub ConvertirEntidadPerfilSuccess()
        Dim entidades As Entidades.Perfiles = New Entidades.Perfiles()
        Dim perfiles As Datos.PERFILES = New Datos.PERFILES()
        perfiles.codigo = 1234
        perfiles.codigo = 1234
        perfiles.nombre = "Prueba"
        perfiles.val_ldap_group = "Prueba"
        perfiles.ind_estado = 456
        entidades = PerfilBLL.ConvertirEntidadPerfil(perfiles)
        Assert.IsNotNull(entidades)
    End Sub
    <TestMethod()> Public Sub ConvertirEntidadPerfilFail()
        Dim entidades As Entidades.Perfiles = New Entidades.Perfiles()
        Dim perfiles As Datos.PERFILES = New Datos.PERFILES()
        perfiles.codigo = Nothing
        perfiles.codigo = Nothing
        perfiles.nombre = Nothing
        perfiles.val_ldap_group = Nothing
        perfiles.ind_estado = Nothing
        entidades = PerfilBLL.ConvertirEntidadPerfil(perfiles)
        Assert.IsTrue(entidades.codigo = Nothing)
    End Sub

    <TestMethod()> Public Sub obtenerPerfilesToodosSuccess()
        Dim texto As String = "abcd"
        Dim perfiles As List(Of Entidades.Perfiles) = New List(Of Entidades.Perfiles)
        perfiles = PerfilBLL.obtenerPerfilesToodos(texto)
        Assert.IsNotNull(perfiles)
    End Sub
    <TestMethod()> Public Sub obtenerPerfilesToodosFail()
        Dim texto As String = Nothing
        Dim perfiles As List(Of Entidades.Perfiles) = New List(Of Entidades.Perfiles)
        perfiles = PerfilBLL.obtenerPerfilesToodos(texto)
        Assert.IsNull(perfiles)
    End Sub

    <TestMethod()> Public Sub obtenerPerfilesActivosSucces()
        Dim entidades As List(Of Entidades.Perfiles) = New List(Of Entidades.Perfiles)
        entidades = PerfilBLL.obtenerPerfilesActivos()
        Assert.IsNotNull(entidades)
    End Sub
    <TestMethod()> Public Sub obtenerPerfilesActivosFail()
        Dim entidades As List(Of Entidades.Perfiles) = New List(Of Entidades.Perfiles)
        entidades = PerfilBLL.obtenerPerfilesActivos()
        Assert.IsNotNull(entidades)
    End Sub

    <TestMethod()> Public Sub obternetPerfilPorIdSuccess()
        Dim idPerfil As Long = 123456
        Dim entidad As Entidades.Perfiles = New Entidades.Perfiles()
        entidad = PerfilBLL.obternetPerfilPorId(idPerfil)
        Assert.IsNotNull(entidad)
    End Sub
    <TestMethod()> Public Sub obternetPerfilPorIdFail()
        Dim idPerfil As Long = Nothing
        Dim entidad As Entidades.Perfiles = New Entidades.Perfiles()
        entidad = PerfilBLL.obternetPerfilPorId(idPerfil)
        Assert.IsNull(entidad)
    End Sub

    <TestMethod()> Public Sub guardarPerfilSuccess()
        Dim resultado As Boolean
        Dim nombrePerfil As String = "Finanzas"
        Dim grupo As String = "Finanzas"
        Dim estado As Boolean = False
        resultado = PerfilBLL.guardarPerfil(nombrePerfil, grupo, estado)
        Assert.IsTrue(resultado = True)
    End Sub
    <TestMethod()> Public Sub guardarPerfilFail()
        Dim resultado As Boolean
        Dim nombrePerfil As String = Nothing
        Dim grupo As String = Nothing
        Dim estado As Boolean = Nothing
        resultado = PerfilBLL.guardarPerfil(nombrePerfil, grupo, estado)
        Assert.IsTrue(resultado = False)
    End Sub

    <TestMethod()> Public Sub actualizarPerfilSucces()
        Dim resultado As Boolean
        Dim codigo As Long = 12345
        Dim nombrePerfil As String = "Empresarial"
        Dim grupo As String = "Empresario"
        Dim estado As Boolean = True
        resultado = PerfilBLL.actualizarPerfil(codigo, nombrePerfil, grupo, estado)
        Assert.IsNotNull(resultado)
    End Sub
    <TestMethod()> Public Sub actualizarPerfilFail()
        Dim resultado As Boolean
        Dim codigo As Long = Nothing
        Dim nombrePerfil As String = "Empresarial"
        Dim grupo As String = "Empresario"
        Dim estado As Boolean = True
        resultado = PerfilBLL.actualizarPerfil(codigo, nombrePerfil, grupo, estado)
        Assert.IsNull(resultado)
    End Sub
End Class