Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports UGPP.CobrosCoativo.Servicios
Imports UGPP.CobrosCoactivo.Entidades
<TestClass()>
Public Class ScvAplCobros

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
    Public Sub OpIniciarInstanciaFail()
        ' TODO: Agregar aquí la lógica de las pruebas
        Dim instancia As New Servicios.SrvAplCobros()
        Dim response As New UGPP.CobrosCoactivo.Entidades.ResponseContract
        Dim context As New UGPP.CobrosCoactivo.Entidades.ContextoTransaccionalRequest
        Dim tituloList As New List(Of TituloOrigenContract)
        Dim documentList As New List(Of DocumentoContract)
        documentList.Add(New DocumentoContract With {.codDocumentic = "1234t", .codTipoDocumento = "qwqeq", .valNombreDocumento = "234567"
                         })
        tituloList.Add(
        New TituloOrigenContract With {
            .areaOrigen = Enumeraciones.AreaOrigen.SUBDIRECCION_PARAFISCALES,
            .causante = New Causante With {.idTipoIdentificacion = 1, .valNumeroIdentificacion = "159340"},
            .fechaEjecutoria = DateTime.Now,
            .fechaExigibilidad = DateTime.Now,
            .fechaNotificacion = DateTime.Now,
            .fechaTituloEjecutivo = DateTime.Now,
            .existeFalloCasacion = 1,
            .valorTitulo = 10000,
            .numeroTitulo = "1000101",
            .existeSentenciaSegundaInstancia = 1,
            .tituloEjecutoriado = 0,
            .tipoCartera = Enumeraciones.TipoCartera.Parafiscales,
            .totalObligacion = 10000,
            .presentaRecursoReconsideracion = 0,
            .presentaRecursoReposicion = 0,
            .cuentasCobros = Nothing,
            .formaNotificacion = Enumeraciones.FormaNotificacion.FormaNotificacion,
            .tipoTitulo = Enumeraciones.TipoTitulo.Parafiscales,
            .sancionInexactitud = 0,
            .tituloEjecutivoRecursoReconsideracion = Nothing,
            .tituloEjecutivoRecursoReposicion = Nothing,
            .tituloEjecutivoSentenciaSegundaInstancia = Nothing,
            .sancionOmision = 0,
            .numeroExpedienteOrigen = 1,
            .partidaGlobal = 10000,
            .documentos = documentList,
            .deudor = New DeudorContract With {.idTipoIdentificacion = "122", .idTipoPersona = "Natural", .valNombreDeudor = "Prueba", .valNumeroIdentificacion = "1234567",
        .direccionesubicacion = New DireccionUbicacionContract With {.codCiudad = "1", .codDepartamento = "1", .idFuenteDireccion = "1", .valCelular = "31112222",
        .valDireccionCompleta = "dir 1 calle 1 # 1", .valemail = "prueba@prueba.com", .valTelefono = "3102342341"}}})



        context.tituloOrigen = tituloList
        response = instancia.OpIniciarInstancia(context)
        Assert.AreEqual("ERROR", response.codigoError)
    End Sub
    <TestMethod()>
    Public Sub OpIniciarInstanciaSucess()
        ' TODO: Agregar aquí la lógica de las pruebas
        Dim instancia As New Servicios.SrvAplCobros()
        Dim response As New UGPP.CobrosCoactivo.Entidades.ResponseContract
        Dim context As New UGPP.CobrosCoactivo.Entidades.ContextoTransaccionalRequest
        Dim tituloList As New List(Of TituloOrigenContract)
        Dim documentList As New List(Of DocumentoContract)
        documentList.Add(New DocumentoContract With {.codDocumentic = "1234t", .codTipoDocumento = "qwqeq", .valNombreDocumento = "234567"
                         })
        tituloList.Add(
        New TituloOrigenContract With {
            .areaOrigen = Enumeraciones.AreaOrigen.DIRECCION_PARAFISCALES,
            .causante = New Causante With {.idTipoIdentificacion = 1, .valNumeroIdentificacion = "159340"},
            .fechaEjecutoria = DateTime.Now,
            .fechaExigibilidad = DateTime.Now,
            .fechaNotificacion = Nothing,
            .fechaTituloEjecutivo = DateTime.Now,
            .existeFalloCasacion = 0,
            .valorTitulo = 10000,
            .numeroTitulo = "1000101",
            .existeSentenciaSegundaInstancia = 0,
            .tituloEjecutoriado = 0,
            .tipoCartera = Enumeraciones.TipoCartera.Parafiscales,
            .totalObligacion = 10000,
            .presentaRecursoReconsideracion = 0,
            .presentaRecursoReposicion = 0,
            .cuentasCobros = Nothing,
            .formaNotificacion = "01",
            .tipoTitulo = "01",
            .sancionInexactitud = 0,
            .tituloEjecutivoRecursoReconsideracion = Nothing,
            .tituloEjecutivoRecursoReposicion = Nothing,
            .tituloEjecutivoSentenciaSegundaInstancia = Nothing,
            .sancionOmision = 0,
            .numeroExpedienteOrigen = "1",
            .partidaGlobal = 10000,
            .documentos = documentList,
            .deudor = New DeudorContract With {.idTipoIdentificacion = "CC", .idTipoPersona = "01", .valNombreDeudor = "Prueba", .valNumeroIdentificacion = "1234567",
        .direccionesubicacion = New DireccionUbicacionContract With {.codCiudad = "05036", .codDepartamento = "00", .idFuenteDireccion = "1", .valCelular = "31112222",
        .valDireccionCompleta = "dir 1 calle 1 # 1", .valemail = "prueba@prueba.com", .valTelefono = "3102342341"}}})
        context.contextoTransaccionalTipo = New ContextoTransaccionalTipo With {.fechaInicioTx = Date.Now, .idEmisor = "1", .idDefinicionProceso = "1",
            .idInstanciaActividad = "1", .idTx = Guid.NewGuid.ToString(), .idUsuario = "prueba", .idUsuarioAplicacion = "prueba", .valClaveUsuarioAplicacion = "test",
            .valURL = "url", .valNumPagina = 0, .valTamPagina = 0, .valNombreDefinicionActividad = "prueba", .valNombreDefinicionProceso = "prueba"}


        context.tituloOrigen = tituloList
        response = instancia.OpIniciarInstancia(context)
        Assert.AreEqual("OK", response.codigoError)
    End Sub

    <TestMethod()>
    Public Sub OpIniciarInstanciaMallaValidacion()
        ' TODO: Agregar aquí la lógica de las pruebas
        Dim instancia As New Servicios.SrvAplCobros()
        Dim response As New UGPP.CobrosCoactivo.Entidades.ResponseContract
        Dim context As New UGPP.CobrosCoactivo.Entidades.ContextoTransaccionalRequest
        Dim tituloList As New List(Of TituloOrigenContract)
        Dim documentList As New List(Of DocumentoContract)

        documentList.Add(
            New DocumentoContract With {
                .codDocumentic = "1234t",
                .codTipoDocumento = "qwqeq",
                .valNombreDocumento = "234567"
            }
        )
        tituloList.Add(
            New TituloOrigenContract()
        )

        context.contextoTransaccionalTipo = New ContextoTransaccionalTipo With {
            .idTx = Guid.NewGuid.ToString(),
            .fechaInicioTx = Date.Now,
            .valNombreDefinicionProceso = "prueba",
            .idUsuario = "s-bpmpar01",
            .idUsuarioAplicacion = "s-bpmpar01",
            .valClaveUsuarioAplicacion = "RptSSo1Ug",'.valURL = "url",
            .idEmisor = "1",
            .idDefinicionProceso = "1", '.idInstanciaActividad = "1",
            .valNumPagina = 0,
            .valTamPagina = 0 ',.valNombreDefinicionActividad = "prueba",
        }

        context.tituloOrigen = tituloList
        response = instancia.OpIniciarInstancia(context)
        Assert.AreEqual("ERROR", response.codigoError)
    End Sub

    <TestMethod()>
    Public Sub ObtenerHomologacion()
        Dim logica As New UGPP.CobrosCoactivo.Logica.SrvAplCobrosBLL
        Dim result As String
        result = logica.ObtenerHomologacion("CC", Enumeraciones.Homologacion.TipoIdentificacion)
        Assert.AreEqual(result, "01")
    End Sub
End Class
