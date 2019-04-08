Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

<TestClass()>
Public Class PruebaUnitariaValidadorBLL

    Private validadorBLL As New ValidadorBLL()

    <TestMethod()>
    Public Sub MallaValidadoraTituloEjecutivoGlobalSuccess()
#Region "Direccion ubicacion"
        Dim DireccionUbicacion As List(Of DireccionUbicacion) = New List(Of DireccionUbicacion)
        DireccionUbicacion.Add(New DireccionUbicacion() With {
        .direccionCompleta = "Calle 56 # 45 b-56",
        .deudornombre = "Carlos Pachon",
        .idDepartamento = 2,
        .idCiudad = 2,
        .telefono = "2345678",
        .celular = "3245678987",
        .email = "carlos@gmail.com",
        .fuentesDirecciones = 87,
        .otrasFuentesDirecciones = "34"})
#End Region

#Region "Deudor"

        Dim Deudor As Deudor = New Deudor()
        Deudor.tipoIdentificacion = "01"
        Deudor.numeroIdentificacion = "1026278871"
        Deudor.digitoVerificacion = "123"
        Deudor.tipoPersona = "01"
        Deudor.nombreDeudor = "JAIMART ENAMORADO"
        Deudor.TipoEnte = 1
        Deudor.PorcentajeParticipacion = 0
#End Region

#Region "direccionUbicacion"
        Dim Direccion As DireccionUbicacion = New DireccionUbicacion()
        Direccion.direccionCompleta = "Calle 56 # 45 b-56"
        Direccion.deudornombre = "Carlos Pachon"
        Direccion.numeroidentificacionDeudor = "1026278871"
        Direccion.idDepartamento = "15"
        Direccion.idCiudad = "05001"
        Direccion.telefono = "2345678"
        Direccion.celular = "3245678987"
        Direccion.email = "carlos@gmail.com"
        Direccion.fuentesDirecciones = 1
        Direccion.otrasFuentesDirecciones = "34"
#End Region

#Region "Titulo especial"
        Dim TituloEspecial As TituloEspecial = New TituloEspecial()
        TituloEspecial.numeroTitulo = "12"
        TituloEspecial.fechaTituloEjecutivo = DateTime.Now
        TituloEspecial.formaNotificacion = "01"
        TituloEspecial.fechaNotificacion = DateTime.Now
#End Region

#Region "Documentos"
        Dim DocumentoMaestroTitulo As List(Of DocumentoMaestroTitulo) = New List(Of DocumentoMaestroTitulo)
        DocumentoMaestroTitulo.Add(New DocumentoMaestroTitulo() With {.ID_DOCUMENTO_TITULO = 4,
        .DES_RUTA_DOCUMENTO = "C:\COD\UGPP\UGPP.CobrosCoactivo\UGPP.CobrosCoactivo.Aplicacion\images\1.png",
        .TIPO_RUTA = 1,
        .COD_GUID = "123",
        .COD_TIPO_DOCUMENTO_AO = "01",
        .NOM_DOC_AO = "Prueba",
        .NUM_PAGINAS = 2,
        .OBSERVA_LEGIBILIDAD = "Si"})
#End Region

#Region "Titulo ejecutivo"
        Dim TituloEjecutivo As TituloEjecutivo = New TituloEjecutivo()
        TituloEjecutivo.tipoTitulo = "01"
        TituloEjecutivo.tipoCartera = 2
        TituloEjecutivo.numeroExpedienteOrigen = Nothing
        TituloEjecutivo.areaOrigen = Enumeraciones.AreaOrigen.SUBDIRECCION_PARAFISCALES
        TituloEjecutivo.numeroTitulo = "65841898498"
        TituloEjecutivo.fechaTituloEjecutivo = DateTime.Now
        TituloEjecutivo.valorTitulo = 12.98
        TituloEjecutivo.partidaGlobal = 89.98
        TituloEjecutivo.sancionOmision = 78.67
        TituloEjecutivo.sancionMora = 78.98
        TituloEjecutivo.TotalSancion = 87.87
        TituloEjecutivo.sancionInexactitud = 67.987
        TituloEjecutivo.totalObligacion = 76.978
        TituloEjecutivo.formaNotificacion = "01"
        TituloEjecutivo.CodTipNotificacion = 67
        TituloEjecutivo.CodTipSentencia = 87
        TituloEjecutivo.fechaNotificacion = DateTime.Now
        TituloEjecutivo.tituloEjecutoriado = 45
        TituloEjecutivo.fechaEjecutoria = DateTime.Now
        TituloEjecutivo.fechaExigibilidad = DateTime.Now
        TituloEjecutivo.fechaCaducidadPrescripcion = DateTime.Now
        TituloEjecutivo.IdunicoTitulo = 23
        TituloEjecutivo.deudor = New Deudor
        TituloEjecutivo.direccionubicacion = New DireccionUbicacion
        TituloEjecutivo.presentaRecursoReconsideracion = 87
        TituloEjecutivo.tituloEjecutivoRecursoReconsideracion = TituloEspecial
        TituloEjecutivo.presentaRecursoReposicion = 12
        TituloEjecutivo.tituloEjecutivoRecursoReposicion = TituloEspecial
        TituloEjecutivo.existeSentenciaSegundaInstancia = 12
        TituloEjecutivo.tituloEjecutivoSentenciaSegundaInstancia = TituloEspecial
        TituloEjecutivo.existeFalloCasacion = 23
        TituloEjecutivo.tituloEjecutivoFalloCasacion = TituloEspecial
        'TituloEjecutivo.documento = New Documentos
#End Region

#Region "Notificación"
        Dim Notificacion As NotificacionTitulo = New NotificacionTitulo()

#End Region


        Dim DeudorValidar As New List(Of Deudor)
        Dim DireccionValidar As New List(Of DireccionUbicacion)
        DeudorValidar.Add(Deudor)
        DireccionValidar.Add(Direccion)
        Dim idUnicoTitulo As Int32


        idUnicoTitulo = validadorBLL.MallaValidadoraTituloEjecutivoGlobal(TituloEjecutivo, DocumentoMaestroTitulo, DeudorValidar, DireccionValidar, Nothing)
        Assert.IsTrue(idUnicoTitulo <> 0)
    End Sub

    <TestMethod()>
    Public Sub MallaValidadoraTituloEjecutivoGlobalFail()
#Region "Direccion ubicacion"
        Dim DireccionUbicacion As List(Of DireccionUbicacion) = New List(Of DireccionUbicacion)
        DireccionUbicacion.Add(New DireccionUbicacion() With {
        .direccionCompleta = "Calle 56 # 45 b-56",
        .deudornombre = "Carlos Pachon",
        .idDepartamento = 2,
        .idCiudad = 2,
        .telefono = "2345678",
        .celular = "3245678987",
        .email = "carlos@gmail.com",
        .fuentesDirecciones = 87,
        .otrasFuentesDirecciones = "34"})
#End Region

#Region "Deudor"

        Dim Deudor As Deudor = New Deudor()
        Deudor.tipoPersona = "01"
        Deudor.TipoEnte = 1
        Deudor.nombreDeudor = "Francisco Ortiz"
        Deudor.tipoIdentificacion = "01"
        Deudor.numeroIdentificacion = "1234565676"
        Deudor.digitoVerificacion = "123"
        Deudor.direccionUbicacion = New DireccionUbicacion() With {
        .direccionCompleta = "Calle 56 # 45 b-56",
        .deudornombre = "Carlos Pachon",
        .idDepartamento = 2,
        .idCiudad = 2,
        .telefono = "2345678",
        .celular = "3245678987",
        .email = "carlos@gmail.com",
        .fuentesDirecciones = 87,
        .otrasFuentesDirecciones = "34"}
#End Region

#Region "Titulo especial"
        Dim TituloEspecial As TituloEspecial = New TituloEspecial()
        TituloEspecial.numeroTitulo = "12"
        TituloEspecial.fechaTituloEjecutivo = DateTime.Now
        TituloEspecial.formaNotificacion = "01"
        TituloEspecial.fechaNotificacion = DateTime.Now
#End Region

#Region "Documentos"
        Dim DocumentoMaestroTitulo As List(Of DocumentoMaestroTitulo) = New List(Of DocumentoMaestroTitulo)
        DocumentoMaestroTitulo.Add(New DocumentoMaestroTitulo() With {.ID_DOCUMENTO_TITULO = 1,
        .DES_RUTA_DOCUMENTO = "C:\COD\UGPP\UGPP.CobrosCoactivo\UGPP.CobrosCoactivo.Aplicacion\images\1.png",
        .TIPO_RUTA = 1,
        .COD_GUID = "123",
        .COD_TIPO_DOCUMENTO_AO = "01",
        .NOM_DOC_AO = "Prueba",
        .NUM_PAGINAS = 2,
        .OBSERVA_LEGIBILIDAD = "Si"})
#End Region

#Region "Titulo ejecutivo"
        Dim TituloEjecutivo As TituloEjecutivo = New TituloEjecutivo()
        TituloEjecutivo.tipoTitulo = Nothing
        TituloEjecutivo.tipoCartera = 2
        TituloEjecutivo.numeroExpedienteOrigen = 12
        TituloEjecutivo.areaOrigen = Enumeraciones.AreaOrigen.SUBDIRECCION_PARAFISCALES
        TituloEjecutivo.numeroTitulo = "23"
        TituloEjecutivo.fechaTituloEjecutivo = DateTime.Now
        TituloEjecutivo.valorTitulo = 12.98
        TituloEjecutivo.partidaGlobal = 89.98
        TituloEjecutivo.sancionOmision = 78.67
        TituloEjecutivo.sancionMora = 78.98
        TituloEjecutivo.TotalSancion = 87.87
        TituloEjecutivo.sancionInexactitud = 67.987
        TituloEjecutivo.totalObligacion = 76.978
        TituloEjecutivo.formaNotificacion = "01"
        TituloEjecutivo.CodTipNotificacion = 67
        TituloEjecutivo.CodTipSentencia = 87
        TituloEjecutivo.fechaNotificacion = DateTime.Now
        TituloEjecutivo.tituloEjecutoriado = 45
        TituloEjecutivo.fechaEjecutoria = DateTime.Now
        TituloEjecutivo.fechaExigibilidad = DateTime.Now
        TituloEjecutivo.fechaCaducidadPrescripcion = DateTime.Now
        TituloEjecutivo.IdunicoTitulo = 23
        TituloEjecutivo.deudor = Nothing
        TituloEjecutivo.direccionubicacion = New DireccionUbicacion
        TituloEjecutivo.presentaRecursoReconsideracion = 87
        TituloEjecutivo.tituloEjecutivoRecursoReconsideracion = TituloEspecial
        TituloEjecutivo.presentaRecursoReposicion = 12
        TituloEjecutivo.tituloEjecutivoRecursoReposicion = TituloEspecial
        TituloEjecutivo.existeSentenciaSegundaInstancia = 12
        TituloEjecutivo.tituloEjecutivoSentenciaSegundaInstancia = TituloEspecial
        TituloEjecutivo.existeFalloCasacion = 23
        TituloEjecutivo.tituloEjecutivoFalloCasacion = TituloEspecial
        'TituloEjecutivo.documento = New Documentos
#End Region

        Dim DeudorValidar As New List(Of Deudor)
        DeudorValidar.Add(Deudor)
        Dim idUnicoTitulo As Int32

        idUnicoTitulo = validadorBLL.MallaValidadoraTituloEjecutivoGlobal(TituloEjecutivo, DocumentoMaestroTitulo, DeudorValidar, DireccionUbicacion)
        Assert.IsFalse(idUnicoTitulo = 0)
    End Sub

    <TestMethod()>
    Public Sub InsertaTituloEjecutivoSuccess()

#Region "Direccion ubicacion"
        Dim DireccionUbicacion As DireccionUbicacion = New DireccionUbicacion()
        DireccionUbicacion.direccionCompleta = "Calle 56 # 45 b-56"
        DireccionUbicacion.deudornombre = "Carlos Pachon"
        DireccionUbicacion.idDepartamento = 2
        DireccionUbicacion.idCiudad = 2
        DireccionUbicacion.telefono = "2345678"
        DireccionUbicacion.celular = "3245678987"
        DireccionUbicacion.email = "carlos@gmail.com"
        DireccionUbicacion.fuentesDirecciones = 87
        DireccionUbicacion.otrasFuentesDirecciones = "34"
#End Region

#Region "Deudor"
        Dim Deudor As Deudor = New Deudor()
        Deudor.tipoPersona = "01"
        Deudor.nombreDeudor = "Francisco Ortiz"
        Deudor.tipoIdentificacion = "01"
        Deudor.numeroIdentificacion = "1234565676"
        Deudor.digitoVerificacion = "123"
        Deudor.direccionUbicacion = DireccionUbicacion
#End Region

#Region "Titulo especial"
        Dim TituloEspecial As TituloEspecial = New TituloEspecial()
        TituloEspecial.numeroTitulo = "12"
        TituloEspecial.fechaTituloEjecutivo = "1/1/1753 12:00:00 AM"
        TituloEspecial.formaNotificacion = "01"
        TituloEspecial.fechaNotificacion = "1/1/1753 12:00:00 AM"
#End Region

#Region "Documentos"
        Dim Documentos As Documentos = New Documentos()
        Documentos.valNombreDocumento = "C.C."
        Documentos.codDocumentic = "123"
        Documentos.codTipoDocumento = "1"
#End Region

#Region "Titulo ejecutivo"
        Dim TituloEjecutivo As TituloEjecutivo = New TituloEjecutivo()
        TituloEjecutivo.tipoTitulo = "67.87"
        TituloEjecutivo.tipoCartera = 2
        TituloEjecutivo.numeroExpedienteOrigen = 12
        TituloEjecutivo.areaOrigen = Enumeraciones.AreaOrigen.SUBDIRECCION_PARAFISCALES
        TituloEjecutivo.numeroTitulo = "23"
        TituloEjecutivo.fechaTituloEjecutivo = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.valorTitulo = 12.98
        TituloEjecutivo.partidaGlobal = 89.98
        TituloEjecutivo.sancionOmision = 78.67
        TituloEjecutivo.sancionMora = 78.98
        TituloEjecutivo.TotalSancion = 87.87
        TituloEjecutivo.sancionInexactitud = 67.987
        TituloEjecutivo.totalObligacion = 76.978
        TituloEjecutivo.formaNotificacion = "01"
        TituloEjecutivo.CodTipNotificacion = 67
        TituloEjecutivo.CodTipSentencia = 87
        TituloEjecutivo.fechaNotificacion = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.tituloEjecutoriado = 45
        TituloEjecutivo.fechaEjecutoria = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.fechaExigibilidad = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.fechaCaducidadPrescripcion = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.IdunicoTitulo = 23
        TituloEjecutivo.deudor = Deudor
        TituloEjecutivo.direccionubicacion = DireccionUbicacion
        TituloEjecutivo.presentaRecursoReconsideracion = 87
        TituloEjecutivo.tituloEjecutivoRecursoReconsideracion = TituloEspecial
        TituloEjecutivo.presentaRecursoReposicion = 12
        TituloEjecutivo.tituloEjecutivoRecursoReposicion = TituloEspecial
        TituloEjecutivo.existeSentenciaSegundaInstancia = 12
        TituloEjecutivo.tituloEjecutivoSentenciaSegundaInstancia = TituloEspecial
        TituloEjecutivo.existeFalloCasacion = 23
        TituloEjecutivo.tituloEjecutivoFalloCasacion = TituloEspecial
        'TituloEjecutivo.documento = Documentos
#End Region

        Assert.IsNotNull(validadorBLL.InsertaTituloEjecutivo(TituloEjecutivo))
    End Sub

    <TestMethod()>
    Public Sub InsertaDeudorSuccess()

#Region "Direccion ubicacion"
        Dim DireccionUbicacion As DireccionUbicacion = New DireccionUbicacion()
        DireccionUbicacion.direccionCompleta = "Calle 56 # 45 b-56"
        DireccionUbicacion.deudornombre = "Carlos Pachon"
        DireccionUbicacion.idDepartamento = 2
        DireccionUbicacion.idCiudad = 2
        DireccionUbicacion.telefono = "2345678"
        DireccionUbicacion.celular = "3245678987"
        DireccionUbicacion.email = "oidssd@gmail.com"
        DireccionUbicacion.fuentesDirecciones = 87
        DireccionUbicacion.otrasFuentesDirecciones = "34"
#End Region

#Region "Deudor"
        Dim Deudor As Deudor = New Deudor()
        Deudor.tipoPersona = "01"
        Deudor.nombreDeudor = "Francisco Ortiz"
        Deudor.tipoIdentificacion = "01"
        Deudor.numeroIdentificacion = "1234322"
        Deudor.digitoVerificacion = "678"
        Deudor.direccionUbicacion = DireccionUbicacion
#End Region

        Dim listDeudor As New List(Of Deudor)
        listDeudor.Add(Deudor)
        Dim idUnico As Int32 = 100

        validadorBLL.InsertaDeudor(listDeudor, idUnico)
    End Sub

    <TestMethod()>
    Public Sub InsertaDireccionSuccess()

#Region "Direccion ubicacion"
        Dim DireccionUbicacion As DireccionUbicacion = New DireccionUbicacion()
        DireccionUbicacion.direccionCompleta = "Calle 56 # 45 b-56"
        DireccionUbicacion.deudornombre = "Carlos Pachon"
        DireccionUbicacion.idDepartamento = 2
        DireccionUbicacion.idCiudad = 2
        DireccionUbicacion.telefono = "2345678"
        DireccionUbicacion.celular = "3245678987"
        DireccionUbicacion.email = "carlos@gmail.com"
        DireccionUbicacion.fuentesDirecciones = 87
        DireccionUbicacion.otrasFuentesDirecciones = "34"
#End Region

        Dim listDireccion As New List(Of DireccionUbicacion)
        listDireccion.Add(DireccionUbicacion)
        validadorBLL.InsertaDireccion(listDireccion)
    End Sub

    <TestMethod()>
    Public Sub InsertaDocumentosSuccess()

#Region "Documento maestro titulo"
        Dim DocumentoMaestroTitulo As DocumentoMaestroTitulo = New DocumentoMaestroTitulo()
        DocumentoMaestroTitulo.ID_MAESTRO_TITULOS_DOCUMENTOS = 678
        DocumentoMaestroTitulo.ID_DOCUMENTO_TITULO = 23
        DocumentoMaestroTitulo.DES_RUTA_DOCUMENTO = "doc/lol"
        DocumentoMaestroTitulo.TIPO_RUTA = 23
        DocumentoMaestroTitulo.COD_GUID = "1234"
        DocumentoMaestroTitulo.COD_TIPO_DOCUMENTO_AO = "123"
        DocumentoMaestroTitulo.NOM_DOC_AO = "345"
        DocumentoMaestroTitulo.OBSERVA_LEGIBILIDAD = "true"
        DocumentoMaestroTitulo.NUM_PAGINAS = 67
#End Region

        Dim listDocumentos As New List(Of DocumentoMaestroTitulo)
        listDocumentos.Add(DocumentoMaestroTitulo)
        Dim idUnico As Int32 = 98
        validadorBLL.InsertaDocumentos(listDocumentos, idUnico)
    End Sub

    <TestMethod()>
    Public Sub InsertaNotificacionSuccess()

        Dim ID_UNICO_MAESTRO_TITULOS As Int32 = 1
        Dim FEC_NOTIFICACIOn As DateTime = "1/1/1753 12:00:00 AM"
        Dim COD_FOR_NOT As String = "01"
        Dim COD_TIPO_NOTIFICACION As Int32 = "01"

        validadorBLL.InsertaNotificacion(ID_UNICO_MAESTRO_TITULOS, FEC_NOTIFICACIOn, COD_FOR_NOT, COD_TIPO_NOTIFICACION)
    End Sub

    <TestMethod()>
    Public Sub MallaValidadoraTituloEjecutivoSuccess()
#Region "Direccion ubicacion"
        Dim DireccionUbicacion As DireccionUbicacion = New DireccionUbicacion()
        DireccionUbicacion.direccionCompleta = "Calle 56 # 45 b-56"
        DireccionUbicacion.deudornombre = "Carlos Pachon"
        DireccionUbicacion.idDepartamento = 2
        DireccionUbicacion.idCiudad = 2
        DireccionUbicacion.telefono = "2345678"
        DireccionUbicacion.celular = "3245678987"
        DireccionUbicacion.email = "carlos@gmail.com"
        DireccionUbicacion.fuentesDirecciones = 87
        DireccionUbicacion.otrasFuentesDirecciones = "34"
#End Region

#Region "Deudor"
        Dim Deudor As Deudor = New Deudor()
        Deudor.tipoPersona = "01"
        Deudor.nombreDeudor = "Francisco Ortiz"
        Deudor.tipoIdentificacion = "01"
        Deudor.numeroIdentificacion = "1234565676"
        Deudor.digitoVerificacion = "123"
        Deudor.direccionUbicacion = DireccionUbicacion
#End Region

#Region "Titulo especial"
        Dim TituloEspecial As TituloEspecial = New TituloEspecial()
        TituloEspecial.numeroTitulo = "12"
        TituloEspecial.fechaTituloEjecutivo = DateTime.Now
        TituloEspecial.formaNotificacion = "01"
        TituloEspecial.fechaNotificacion = DateTime.Now
#End Region

#Region "Documentos"
        Dim Documentos As Documentos = New Documentos()
        Documentos.valNombreDocumento = "C.C."
        Documentos.codDocumentic = "123"
        Documentos.codTipoDocumento = "1"
#End Region

#Region "Titulo ejecutivo"
        Dim TituloEjecutivo As TituloEjecutivo = New TituloEjecutivo()
        TituloEjecutivo.tipoTitulo = "67.87"
        TituloEjecutivo.tipoCartera = 2
        TituloEjecutivo.numeroExpedienteOrigen = 12
        TituloEjecutivo.areaOrigen = Enumeraciones.AreaOrigen.SUBDIRECCION_PARAFISCALES
        TituloEjecutivo.numeroTitulo = "23"
        TituloEjecutivo.fechaTituloEjecutivo = DateTime.Now
        TituloEjecutivo.valorTitulo = 12.98
        TituloEjecutivo.partidaGlobal = 89.98
        TituloEjecutivo.sancionOmision = 78.67
        TituloEjecutivo.sancionMora = 78.98
        TituloEjecutivo.TotalSancion = 87.87
        TituloEjecutivo.sancionInexactitud = 67.987
        TituloEjecutivo.totalObligacion = 76.978
        TituloEjecutivo.formaNotificacion = "01"
        TituloEjecutivo.CodTipNotificacion = 67
        TituloEjecutivo.CodTipSentencia = 87
        TituloEjecutivo.fechaNotificacion = DateTime.Now
        TituloEjecutivo.tituloEjecutoriado = 45
        TituloEjecutivo.fechaEjecutoria = DateTime.Now
        TituloEjecutivo.fechaExigibilidad = DateTime.Now
        TituloEjecutivo.fechaCaducidadPrescripcion = DateTime.Now
        TituloEjecutivo.IdunicoTitulo = 23
        TituloEjecutivo.deudor = Deudor
        TituloEjecutivo.direccionubicacion = DireccionUbicacion
        TituloEjecutivo.presentaRecursoReconsideracion = 87
        TituloEjecutivo.tituloEjecutivoRecursoReconsideracion = TituloEspecial
        TituloEjecutivo.presentaRecursoReposicion = 12
        TituloEjecutivo.tituloEjecutivoRecursoReposicion = TituloEspecial
        TituloEjecutivo.existeSentenciaSegundaInstancia = 12
        TituloEjecutivo.tituloEjecutivoSentenciaSegundaInstancia = TituloEspecial
        TituloEjecutivo.existeFalloCasacion = 23
        TituloEjecutivo.tituloEjecutivoFalloCasacion = TituloEspecial
        'TituloEjecutivo.documento = Documentos
#End Region

        Dim resultList As New List(Of RespuestaMallaValidacion)
        resultList = validadorBLL.MallaValidadoraTituloEjecutivo(TituloEjecutivo)
        Assert.IsNull(resultList)
    End Sub

    <TestMethod()>
    Public Sub MallaValidadorDocumentoSuccess()
#Region "Direccion ubicacion"
        Dim DireccionUbicacion As DireccionUbicacion = New DireccionUbicacion()
        DireccionUbicacion.direccionCompleta = "Calle 56 # 45 b-56"
        DireccionUbicacion.deudornombre = "Carlos Pachon"
        DireccionUbicacion.idDepartamento = 2
        DireccionUbicacion.idCiudad = 2
        DireccionUbicacion.telefono = "2345678"
        DireccionUbicacion.celular = "3245678987"
        DireccionUbicacion.email = "carlos@gmail.com"
        DireccionUbicacion.fuentesDirecciones = 87
        DireccionUbicacion.otrasFuentesDirecciones = "34"
#End Region

#Region "Deudor"
        Dim Deudor As Deudor = New Deudor()
        Deudor.tipoPersona = "01"
        Deudor.nombreDeudor = "Francisco Ortiz"
        Deudor.tipoIdentificacion = "01"
        Deudor.numeroIdentificacion = "1234565676"
        Deudor.digitoVerificacion = "123"
        Deudor.direccionUbicacion = DireccionUbicacion
#End Region

#Region "Titulo especial"
        Dim TituloEspecial As TituloEspecial = New TituloEspecial()
        TituloEspecial.numeroTitulo = "12"
        TituloEspecial.fechaTituloEjecutivo = "1/1/1753 12:00:00 AM"
        TituloEspecial.formaNotificacion = "01"
        TituloEspecial.fechaNotificacion = "1/1/1753 12:00:00 AM"
#End Region

#Region "Documentos"
        Dim Documentos As Documentos = New Documentos()
        Documentos.valNombreDocumento = "C.C."
        Documentos.codDocumentic = "123"
        Documentos.codTipoDocumento = "1"
#End Region

#Region "Titulo ejecutivo"
        Dim TituloEjecutivo As TituloEjecutivo = New TituloEjecutivo()
        TituloEjecutivo.tipoTitulo = "67.87"
        TituloEjecutivo.tipoCartera = 2
        TituloEjecutivo.numeroExpedienteOrigen = 12
        TituloEjecutivo.areaOrigen = Enumeraciones.AreaOrigen.SUBDIRECCION_NOMINA
        TituloEjecutivo.numeroTitulo = "23"
        TituloEjecutivo.fechaTituloEjecutivo = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.valorTitulo = 12.98
        TituloEjecutivo.partidaGlobal = 89.98
        TituloEjecutivo.sancionOmision = 78.67
        TituloEjecutivo.sancionMora = 78.98
        TituloEjecutivo.TotalSancion = 87.87
        TituloEjecutivo.sancionInexactitud = 67.987
        TituloEjecutivo.totalObligacion = 76.978
        TituloEjecutivo.formaNotificacion = "01"
        TituloEjecutivo.CodTipNotificacion = 67
        TituloEjecutivo.CodTipSentencia = 87
        TituloEjecutivo.fechaNotificacion = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.tituloEjecutoriado = 45
        TituloEjecutivo.fechaEjecutoria = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.fechaExigibilidad = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.fechaCaducidadPrescripcion = "1/1/1753 12:00:00 AM"
        TituloEjecutivo.IdunicoTitulo = 23
        TituloEjecutivo.deudor = Deudor
        TituloEjecutivo.direccionubicacion = DireccionUbicacion
        TituloEjecutivo.presentaRecursoReconsideracion = 87
        TituloEjecutivo.tituloEjecutivoRecursoReconsideracion = TituloEspecial
        TituloEjecutivo.presentaRecursoReposicion = 12
        TituloEjecutivo.tituloEjecutivoRecursoReposicion = TituloEspecial
        TituloEjecutivo.existeSentenciaSegundaInstancia = 12
        TituloEjecutivo.tituloEjecutivoSentenciaSegundaInstancia = TituloEspecial
        TituloEjecutivo.existeFalloCasacion = 23
        TituloEjecutivo.tituloEjecutivoFalloCasacion = TituloEspecial
        'TituloEjecutivo.documento = Documentos
#End Region

#Region "Documento maestro titulo"
        Dim DocumentoMaestroTitulo As DocumentoMaestroTitulo = New DocumentoMaestroTitulo()
        DocumentoMaestroTitulo.ID_MAESTRO_TITULOS_DOCUMENTOS = 678
        DocumentoMaestroTitulo.ID_DOCUMENTO_TITULO = 23
        DocumentoMaestroTitulo.DES_RUTA_DOCUMENTO = "doc/lol"
        DocumentoMaestroTitulo.TIPO_RUTA = 23
        DocumentoMaestroTitulo.COD_GUID = "1234"
        DocumentoMaestroTitulo.COD_TIPO_DOCUMENTO_AO = "123"
        DocumentoMaestroTitulo.NOM_DOC_AO = "345"
        DocumentoMaestroTitulo.OBSERVA_LEGIBILIDAD = "true"
        DocumentoMaestroTitulo.NUM_PAGINAS = 67
#End Region

        Dim DocumentosMaestro As New List(Of DocumentoMaestroTitulo)
        DocumentosMaestro.Add(DocumentoMaestroTitulo)
        Dim Respuesta As New List(Of RespuestaMallaValidacion)

        Respuesta = validadorBLL.MallaValidadorDocumento(DocumentosMaestro, TituloEjecutivo)

        Assert.IsNull(Respuesta)
    End Sub

    <TestMethod()>
    Public Sub MallaValidadoraDeudorSuccess()
#Region "Direccion ubicacion"
        Dim DireccionUbicacion As DireccionUbicacion = New DireccionUbicacion()
        DireccionUbicacion.direccionCompleta = "Calle 56 # 45 b-56"
        DireccionUbicacion.deudornombre = "Carlos Pachon"
        DireccionUbicacion.idDepartamento = 2
        DireccionUbicacion.idCiudad = 2
        DireccionUbicacion.telefono = "2345678"
        DireccionUbicacion.celular = "3245678987"
        DireccionUbicacion.email = "carlos@gmail.com"
        DireccionUbicacion.fuentesDirecciones = 87
        DireccionUbicacion.otrasFuentesDirecciones = "34"
#End Region

#Region "Deudor"
        Dim Deudor As Deudor = New Deudor()
        Deudor.tipoPersona = "01"
        Deudor.nombreDeudor = "Francisco Ortiz"
        Deudor.tipoIdentificacion = "01"
        Deudor.numeroIdentificacion = "1234565676"
        Deudor.digitoVerificacion = "123"
        Deudor.direccionUbicacion = DireccionUbicacion
#End Region

        Dim listDeudor As New List(Of Deudor)
        listDeudor.Add(Deudor)
        Dim respuesta As New List(Of RespuestaMallaValidacion)
        respuesta = validadorBLL.MallaValidadoraDeudor(listDeudor)
        Assert.IsNull(respuesta)
    End Sub

    <TestMethod()>
    Public Sub MallaValidadoraDireccionUbicacionSuccess()
#Region "Direccion ubicacion"
        Dim DireccionUbicacion As DireccionUbicacion = New DireccionUbicacion()
        DireccionUbicacion.direccionCompleta = "Calle 56 # 45 b-56"
        DireccionUbicacion.deudornombre = "Carlos Pachon"
        DireccionUbicacion.idDepartamento = 2
        DireccionUbicacion.idCiudad = 2
        DireccionUbicacion.telefono = "2345678"
        DireccionUbicacion.celular = "3245678987"
        DireccionUbicacion.email = "carlos@gmail.com"
        DireccionUbicacion.fuentesDirecciones = 87
        DireccionUbicacion.otrasFuentesDirecciones = "34"
#End Region

        Dim DireccionUbicacionList As New List(Of DireccionUbicacion)
        DireccionUbicacionList.Add(DireccionUbicacion)
        Dim respuesta As New List(Of RespuestaMallaValidacion)
        respuesta = validadorBLL.MallaValidadoraDireccionUbicacion(DireccionUbicacionList)
        Assert.IsNull(respuesta)
    End Sub
End Class
