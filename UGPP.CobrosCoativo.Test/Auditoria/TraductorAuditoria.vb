Imports System.Text
Imports coactivosyp
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
<TestClass()>
Public Class TraductorAuditoria

    Private testContextInstance As TestContext

    '''<summary>
    '''Obtiene o establece el contexto de las pruebas que proporciona
    '''información y funcionalidad para la serie de pruebas actual.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Atributos de prueba adicionales"
    '
    ' Puede usar los siguientes atributos adicionales conforme escribe las pruebas:
    '
    ' Use ClassInitialize para ejecutar el código antes de ejecutar la primera prueba en la clase
    ' <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    ' End Sub
    '
    ' Use ClassCleanup para ejecutar el código después de haberse ejecutado todas las pruebas en una clase
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
    '
    ' Usar TestInitialize para ejecutar el código antes de ejecutar cada prueba
    ' <TestInitialize()> Public Sub MyTestInitialize()
    ' End Sub
    '
    ' Use TestCleanup para ejecutar el código una vez ejecutadas todas las pruebas
    ' <TestCleanup()> Public Sub MyTestCleanup()
    ' End Sub
    '
#End Region

    <TestMethod()>
    Public Sub VerficarTraductor()
        Dim test As New coactivosyp.LogProcesos
        Dim resultado As Boolean
        resultado = test.SaveLog("oodp", "test", "11", New SqlClient.SqlCommand("UPDATE DICCIONARIO_AUDITORIA SET [VALOR_DESTINO] = A, [ACTIVO] = 1"))
        Assert.AreEqual(True, resultado)
    End Sub

    <TestMethod()>
    Public Sub SalvarDiccionario()
        Dim resultado As Boolean
        Dim diccionario As New DiccionarioAditoria
        diccionario.ACTIVO = True
        diccionario.VALOR_DESTINO = "TEST2"
        diccionario.VALOR_ORIGINAL = "TEST"
        Dim translator As New TraductorAuditoriaBLL
        resultado = translator.salvarDiccionario(diccionario)
        Assert.IsTrue(resultado, True)
    End Sub
End Class
