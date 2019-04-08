Imports UGPP
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoativo

''' <summary>
''' Clase en la capa de presentación para invocar la creación de un nuevo título en documentic
''' </summary>
Public Class ExpedientesUtils

    ''' <summary>
    ''' Lista de IDs de títulos que van a ser relacionados con el expediente que se crea en la aplicación de cobros y coactivos
    ''' </summary>
    Private listIntTitulos As List(Of Int32)
    ''' <summary>
    ''' Nuevo número de expediente que se va a crear
    ''' </summary>
    Private intNuevoNumeroExpediente As Int32
    ''' <summary>
    ''' Objeto expediente que va a ser creado en la aplicación
    ''' </summary>
    Dim objExpediente As Entidades.EJEFISGLOBAL
    ''' <summary>
    ''' Número de expediente creado en documentic
    ''' </summary>
    Dim strNumeroDocumentic As String
    ''' <summary>
    ''' Usuario que realiza las operaciones, se toma del login
    ''' </summary>
    Dim strUserGenerator As String
    ''' <summary>
    ''' Errores registrados en el proceso
    ''' </summary>
    Dim errors As List(Of ErroresCreacionExpediente)
    ''' <summary>
    ''' URL base del proyecto
    ''' </summary>
    Property urlBase As String
    ''' <summary>
    ''' Project base path
    ''' </summary>
    Property basePath As String

    Public Sub New(ByVal prmIntTituloID As Int32, ByVal prmStrUserLogin As String)
        Me.listIntTitulos = New List(Of Int32)
        Me.listIntTitulos.Add(prmIntTituloID)
        Me.strUserGenerator = prmStrUserLogin
        'errors = New Dictionary(Of String, String)
        errors = New List(Of ErroresCreacionExpediente)
    End Sub

    Public Sub New(ByVal prmListIntTitulosId As List(Of Int32), ByVal prmStrUserLogin As String)
        Me.listIntTitulos = prmListIntTitulosId
        Me.strUserGenerator = prmStrUserLogin
        errors = New List(Of ErroresCreacionExpediente)
    End Sub

    Private Sub VerificarTitulos()
        Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL
        For Each titulo As Int32 In listIntTitulos
            'Datos del título
            Dim _maestroTitulos As New MaestroTitulosBLL()
            Dim tituloConsulta = _maestroTitulos.consultarTituloPorID(titulo)
        Next
    End Sub

    ''' <summary>
    ''' Genera una lista con los títulos que se desean relacionar
    ''' </summary>
    ''' <param name="prmIntTitulo"></param>
    Private Sub actualizarListaTitulos(Optional ByVal prmIntTitulo As Integer = Nothing)
        'Si viene definido un título se sincronizan solo esos documentos
        If prmIntTitulo > 0 Then
            Me.listIntTitulos = New List(Of Integer)
            Me.listIntTitulos.Add(prmIntTitulo)
        End If
    End Sub

    ''' <summary>
    ''' Obtener el último número de expediente creado en la aploicación y agregar un número
    ''' </summary>
    Private Sub obtenerNumeroExpediente()
        Dim expedienteBLL As New ExpedienteBLL
        Me.intNuevoNumeroExpediente = expedienteBLL.obtenerUltimoExpediente() + 1
    End Sub

    ''' <summary>
    ''' Disparador de consumo de servicio y creación de expediente en local 
    ''' </summary>
    ''' <returns>ID expediente cobros y coactivos</returns>
    Public Function iniciarInstaciaExpediente() As Int32
        obtenerNumeroExpediente()
        crearExpedienteServicio()
        crearExpedienteLocal()
        Return Me.intNuevoNumeroExpediente
    End Function

    ''' <summary>
    ''' Servicio que crea el expediente en el servicio
    ''' </summary>
    Private Sub crearExpedienteServicio(Optional ByVal prmIntNumExpediente As Int32 = Nothing)
        prmIntNumExpediente = If(prmIntNumExpediente > 0, prmIntNumExpediente, Me.intNuevoNumeroExpediente)
        Dim servicio As New Servicios.SrvIntExpediente
        Dim expediente As New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.ExpedienteTipo
        expediente.valFondo = "900"
        expediente.valEntidadPredecesora = "900"
        expediente.identificacion = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.IdentificacionTipo
        expediente.identificacion.codTipoIdentificacion = "EX"
        expediente.identificacion.valNumeroIdentificacion = prmIntNumExpediente
        expediente.descDescripcion = "Creación de Expediente " & prmIntNumExpediente
        expediente.codSeccion = "1530"
        expediente.serieDocumental = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.SerieDocumentalTipo
        expediente.serieDocumental.codSerie = "1530.44"

        expediente.identificacion.municipioExpedicion = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.MunicipioTipo()
        expediente.identificacion.municipioExpedicion.departamento = New CobrosCoactivo.Entidades.UGPPSrvIntExpediente.DepartamentoTipo()

        Dim result As CobrosCoactivo.Entidades.UGPPSrvIntExpediente.OpCrearExpedienteRespTipo = servicio.OpCrearExpediente(expediente)
        Me.strNumeroDocumentic = result.expediente.idNumExpediente
    End Sub

    ''' <summary>
    ''' Servicio que crea el expediente en el aplicativo de CyC
    ''' </summary>
    Private Sub crearExpedienteLocal()
        Dim _maestroTitulosBLL As New MaestroTitulosBLL()
        Dim tituloMasCercanoPrescripcion As Entidades.MaestroTitulos
        If (Me.listIntTitulos.Count > 1) Then
            tituloMasCercanoPrescripcion = _maestroTitulosBLL.obtenerTituloMasCercanoPrescripcion(Me.listIntTitulos)
        Else
            tituloMasCercanoPrescripcion = _maestroTitulosBLL.consultarTituloPorID(Me.listIntTitulos.FirstOrDefault())
        End If

        Me.objExpediente = New Entidades.EJEFISGLOBAL

        Me.objExpediente.EFINROEXP = intNuevoNumeroExpediente.ToString() 'Número del expediente que se envío al servicio de crear expediene
        Me.objExpediente.EFIFECHAEXP = tituloMasCercanoPrescripcion.MT_fec_expedicion_titulo 'Fecha recepción (creación) título ejecutivo - Fecha de creación del título - Se toma el título con la fecha de recepción mas recente si son varios títulos
        Me.objExpediente.EFINUMMEMO = String.Empty 'Numero de memorando - No Va - Se deja en blanco
        Me.objExpediente.EFIEXPORIGEN = If(IsNothing(tituloMasCercanoPrescripcion.NoExpedienteOrigen), String.Empty, tituloMasCercanoPrescripcion.NoExpedienteOrigen.ToString()) 'Expediente Origen
        Me.objExpediente.EFIEXPDOCUMENTIC = Me.strNumeroDocumentic 'Expediente documentic, se crea en el servicio
        Me.objExpediente.EFIMODCOD = 4 ' Impuesto No. 4 = "parafiscales / pensiones"
        'Me.objExpediente.EFIESTADO = "09" 'Colocar inicialmente el estado '09' = REPARTO

        Dim expedienteBLL As New ExpedienteBLL
        Try
            expedienteBLL.crearExpediente(objExpediente)
            'Una vez creado se actualiza el expediente del título - Se mueve a la finalización de la creación del expediente
            actualizarExpedienteTitulo()
            'asignarExpedientePorRepartir()
        Catch ex As Exception
            'TODO: Llamar LOG errores
        End Try

    End Sub

    ''' <summary>
    ''' Asigna el expediente al 
    ''' </summary>
    ''' <param name="prmIntTitulo"></param>
    Private Sub actualizarExpedienteTitulo(Optional ByVal prmIntTitulo As Integer = Nothing, Optional ByVal prmIntExpediente As Integer = Nothing)

        actualizarListaTitulos(prmIntTitulo)
        Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL
        For Each titulo As Int32 In listIntTitulos
            Dim _maestroTitulos As New MaestroTitulosBLL()
            _maestroTitulos.asignarExpedienteATitulo(titulo, Me.intNuevoNumeroExpediente)
        Next
    End Sub

    ''' <summary>
    ''' Actualiza el estado operatio del expediente que se creo  a Por repartir
    ''' </summary>
    ''' <param name="prmIntExpedienteId">Id del expediente</param>
    Private Sub asignarExpedientePorRepartir(Optional ByVal prmIntExpedienteId As String = Nothing)
        prmIntExpedienteId = If(Not IsNothing(prmIntExpedienteId), prmIntExpedienteId, intNuevoNumeroExpediente)
        'Se crea el objeto de inserción
        Dim objTareaAsigana As New Entidades.TareaAsignada
        objTareaAsigana.VAL_USUARIO_NOMBRE = String.Empty 'Se deja vacio para que no presente error
        objTareaAsigana.COD_TIPO_OBJ = Entidades.Enumeraciones.DominioDetalle.Expediente
        objTareaAsigana.EFINROEXP_EXPEDIENTE = intNuevoNumeroExpediente.ToString()
        objTareaAsigana.COD_ESTADO_OPERATIVO = 10 'Por repartir

        Dim tareaAsigada As New TareaAsignadaBLL
        Try
            tareaAsigada.registrarTarea(objTareaAsigana)
        Catch ex As Exception
            'TODO: Llamar LOG errores
        End Try
    End Sub

    ''' <summary>
    ''' Llamado de la sincronización de los archivos relacionados con un títulos
    ''' Se sincronizan solamente ficheros de títulos asignados a un expediente
    ''' </summary>
    ''' <param name="prmIntTitulo">Id del título a sincronizar</param>
    Public Sub SincronizarDocumentosTitulos(Optional ByVal prmIntTitulo As Integer = Nothing)
        'Si viene definido un título se sincronizan solo esos documentos
        actualizarListaTitulos(prmIntTitulo)

        Dim _maestroTitulos As New MaestroTitulosBLL()
        Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL
        Dim expedienteBLL As New ExpedienteBLL

        For Each titulo As Int32 In listIntTitulos
            'Datos del título
            Dim tituloConsulta = _maestroTitulos.consultarTituloPorID(titulo)

            If String.IsNullOrEmpty(tituloConsulta.MT_expediente) Then
                'errors.Add("05", "El título no cuenta con un expediente")
                errors.Add(New ErroresCreacionExpediente With {.CodigoError = 5, .Descripcion = "El título " & tituloConsulta.idunico & " no cuenta con un expediente"})
                Continue For
            End If

            'Si el título no es automático y no esta definida la url de la aplicación y el directorio del proyecto 
            'no se puede realizar el proceso de sincronización de los ficheros
            If (Not tituloConsulta.Automatico) And (String.IsNullOrEmpty(basePath) Or String.IsNullOrEmpty(urlBase)) Then
                errors.Add(New ErroresCreacionExpediente With {.CodigoError = 8, .Descripcion = "No está definida la URL y la ruta física para sincronizar los documentos"})
                Exit Sub
            End If

            'Documentos del título
            Dim documentosTitulo As List(Of Entidades.MaestroTitulosDocumentos) = _maestroTitulosDocumentosBLL.obtenerDocumentosPorTitulo(titulo)
            If documentosTitulo.Count = 0 Then
                errors.Add(New ErroresCreacionExpediente With {.CodigoError = 6, .Descripcion = "El título " & tituloConsulta.idunico & " no cuenta con documentos para relacionar"})
                Continue For
            End If

            'Datos del expediente
            If IsNothing(Me.objExpediente) Then
                Me.objExpediente = expedienteBLL.obtenerExpedientePorId(tituloConsulta.MT_expediente)
                If IsNothing(Me.objExpediente) Then
                    errors.Add(New ErroresCreacionExpediente With {.CodigoError = 7, .Descripcion = "El Expediente " & tituloConsulta.MT_expediente & " no es valido"})
                    Exit Sub
                End If
                Me.strNumeroDocumentic = Me.objExpediente.EFIEXPDOCUMENTIC
            End If
            'Se inicia el proceso de sincronización de los documentos
            For Each documento As Entidades.MaestroTitulosDocumentos In documentosTitulo
                'Se verifica que no este sincronizado
                If (documento.IND_DOC_SINCRONIZADO) Then
                    Continue For
                End If

                'Se inicializa el servicio para sincronizar documentos
                Dim servicio As New Servicios.SrvIntDocumento
                Dim arrayDocumentos(0) As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.DocumentoTipo
                arrayDocumentos(0) = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.DocumentoTipo

                If tituloConsulta.Automatico Then
                    'Datos que provienen desde servicio para descargar documento desde DocumenTic
                    'Se verifica que el documento tenga el código GUID
                    If IsNothing(documento.COD_GUID) Or String.IsNullOrEmpty(documento.COD_GUID) Then
                        Continue For
                    End If
                    'Se inicializa variable para consultar el documento a través del servicio
                    Dim documentoConsulta As New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoSolTipo
                    documentoConsulta.documento = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.DocumentoTipo
                    documentoConsulta.documento.idDocumento = documento.COD_GUID

                    Dim documentoConsultaResult As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpConsultarDocumentoRespTipo
                    Try
                        documentoConsultaResult = servicio.ConsultarDcoumento(documentoConsulta)
                    Catch ex As Exception
                        errors.Add(New ErroresCreacionExpediente With {.CodigoError = 11, .Descripcion = "Fallo al consultar el documento: " & documento.COD_GUID})
                        CommonsCobrosCoactivos.EscribirMensajeLogServicio("Fallo al consultar el documento: " & documento.COD_GUID & " | " & ex.Message)
                        Continue For
                    End Try
                    arrayDocumentos(0).valNombreDocumento = documentoConsultaResult.documento.valNombreDocumento
                    arrayDocumentos(0).docDocumento = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.ArchivoTipo With {
                        .valNombreArchivo = documentoConsultaResult.documento.valNombreDocumento,
                        .codTipoMIMEArchivo = documentoConsultaResult.documento.docDocumento.codTipoMIMEArchivo,
                        .valContenidoArchivo = documentoConsultaResult.documento.docDocumento.valContenidoArchivo
                    }
                Else
                    'Datos que se toman desde la carga manual que se realiza del título
                    'Se verifica que este definida la ruta del archivo
                    If String.IsNullOrEmpty(documento.DES_RUTA_DOCUMENTO) Then
                        'errors.Add("06", "Ruta invalida")
                        errors.Add(New ErroresCreacionExpediente With {.CodigoError = 7, .Descripcion = "No se encuentra la ruta del documento" & documento.DES_RUTA_DOCUMENTO & " del título " & tituloConsulta.idunico})
                        Continue For
                    End If

                    'Remplaza la URL y verifica que el archivo exista
                    documento.DES_RUTA_DOCUMENTO = documento.DES_RUTA_DOCUMENTO.Replace(urlBase, "")
                    Dim fullPath As String = basePath & documento.DES_RUTA_DOCUMENTO
                    If Not (System.IO.File.Exists(fullPath)) Then
                        errors.Add(New ErroresCreacionExpediente With {.CodigoError = 9, .Descripcion = "El archivo " & fullPath & " del título " & tituloConsulta.idunico & " no existe"})
                        Continue For
                    End If

                    fullPath = fullPath.Replace("\", "/")
                    Dim rutaDoc = documento.DES_RUTA_DOCUMENTO.Replace("\", "/")
                    'Carga de datos que dependen de la carga manual
                    arrayDocumentos(0).valNombreDocumento = documento.DES_RUTA_DOCUMENTO.Substring(rutaDoc.LastIndexOf("/") + 1)
                    arrayDocumentos(0).docDocumento = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.ArchivoTipo With {
                        .valNombreArchivo = documento.DES_RUTA_DOCUMENTO.Substring(fullPath.LastIndexOf("\") + 1),
                        .codTipoMIMEArchivo = System.IO.Path.GetExtension(fullPath),
                        .valContenidoArchivo = Util.ReadFileAsArray(fullPath)
                    }
                End If
                'Datos generales del documento
                arrayDocumentos(0).idDocumento = "" ' ENVIAR EN BLANCO
                arrayDocumentos(0).valAutorOriginador = strUserGenerator
                arrayDocumentos(0).fecDocumento = DateTime.Now.ToString("dd/MM/yyyy")
                arrayDocumentos(0).valPaginas = "1"
                arrayDocumentos(0).valNaturalezaDocumento = "Electrónico"
                arrayDocumentos(0).descObservacionLegibilidad = "LEGIBLE"
                arrayDocumentos(0).valOrigenDocumento = "Original"
                arrayDocumentos(0).codTipoDocumento = "357"
                arrayDocumentos(0).numFolios = "0"

                'Se obtienen los datos de los deudores
                Dim _entesDeudoresBLL As New EntesDeudoresBLL
                Dim deudores = _entesDeudoresBLL.obtenerDeudoresPorTitulo(titulo)
                'Se toma el primer deudor relacionado con le título
                Dim deudor = deudores.FirstOrDefault()

                Dim arrayAgrupadores(6) As String
                arrayAgrupadores(0) = deudor.ED_Codigo_Nit
                arrayAgrupadores(1) = "CC"
                arrayAgrupadores(2) = tituloConsulta.MT_nro_titulo
                arrayAgrupadores(3) = tituloConsulta.NoExpedienteOrigen.ToString()
                arrayAgrupadores(4) = "Deudor"
                arrayAgrupadores(5) = "Expediente"
                arrayAgrupadores(6) = tituloConsulta.MT_expediente
                arrayDocumentos(0).metadataDocumento = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.MetadataDocumentoTipo With {
                    .valAgrupador = arrayAgrupadores
                }

                Dim param As New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoSolTipo
                param.documentos = arrayDocumentos
                param.expediente = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.ExpedienteTipo With {
                    .idNumExpediente = Me.strNumeroDocumentic,
                    .valFondo = "900",
                    .valEntidadPredecesora = "900"
                }
                param.correspondencia = New CobrosCoactivo.Entidades.UGPPSrvIntDocumento.CorrespondenciaTipo With {
                    .codEntidadOriginadora = "900"
                }

                For Each item In param.documentos
                    Dim blankArray(0) As Byte
                    If (item.docDocumento.valContenidoFirma Is Nothing) Then
                        item.docDocumento.valContenidoFirma = blankArray
                    End If
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Inicio consumo de servicio para enviar el documento
                Dim docResult As CobrosCoactivo.Entidades.UGPPSrvIntDocumento.OpIngresarDocumentoRespTipo
                Try
                    docResult = servicio.OpIngresarDocumento(param)
                    If docResult.contextoRespuesta.codEstadoTx = "1" Then
                        documento.IND_DOC_SINCRONIZADO = 1
                        _maestroTitulosDocumentosBLL.ActualizarMaestroTitulosDocumentos(documento)
                    End If
                Catch ex As Exception
                    errors.Add(New ErroresCreacionExpediente With {.CodigoError = 10, .Descripcion = "Fallo la sincronización del  archivo: " & arrayDocumentos(0).docDocumento.valNombreArchivo})
                    CommonsCobrosCoactivos.EscribirMensajeLogServicio("Fallo la sincronización del  archivo: " & arrayDocumentos(0).docDocumento.valNombreArchivo & " " & ex.Message)
                    Continue For
                End Try
            Next
        Next
    End Sub
End Class

Public Class ErroresCreacionExpediente
    Property CodigoError As Integer
    Property Descripcion As String
End Class