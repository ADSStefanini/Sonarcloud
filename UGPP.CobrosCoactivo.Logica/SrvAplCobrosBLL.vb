Imports AutoMapper
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos

Public Class SrvAplCobrosBLL
    Inherits ValidadorUtils

    Public erroresValidation As List(Of RespuestaMallaValidacion)

    ''' <summary>
    ''' Método que envia la infomración a la malla de validación
    ''' </summary>
    ''' <param name="request">Parametro con todos los tipos necesarios</param>
    ''' <returns></returns>
    Public Function CrearTituloAutomatico(request As ContextoTransaccionalRequest) As Entidades.ResponseContract
        Dim response As New Entidades.ResponseContract
        Dim validation As New ValidadorBLL
        Dim context As New Datos.ContextoTransaccionalDAL
        Dim resultContext As Boolean
        Dim responseTitle As List(Of RespuestaMallaValidacion)
        Dim idTitulo As Integer
        'Dim errorValidation As New List(Of String) 'TODO: Estos errores debería ser como el obgeto
        erroresValidation = New List(Of RespuestaMallaValidacion)
        Dim insertarTitulo As Boolean = True
        Dim errorMallaValidacion As New List(Of String)
        Try
            ' Mapping del tittulo
            Dim titulo As TituloEjecutivo
            Dim deudorProvisional As New DeudorContract
            deudorProvisional = request.tituloOrigen(0).deudor
            request.tituloOrigen(0).deudor = Nothing
            Dim config As New MapperConfiguration(Function(cfg)
                                                      Return cfg.CreateMap(Of CobrosCoactivo.Entidades.TituloOrigenContract, TituloEjecutivo)()
                                                  End Function)
            Dim IMapper = config.CreateMapper()
            titulo = IMapper.Map(Of TituloOrigenContract, TituloEjecutivo)(request.tituloOrigen(0))
            'Validaciones
            If titulo.presentaRecursoReposicion.HasValue And titulo.presentaRecursoReposicion.Value Then
                If titulo.tituloEjecutivoRecursoReposicion Is Nothing Then
                    erroresValidation.Add(validation.CrearRespuestaMallaValidacion("048", "No hay recurso de reposición"))
                End If
            End If

            If titulo.presentaRecursoReconsideracion.HasValue And titulo.presentaRecursoReconsideracion.Value Then
                If titulo.tituloEjecutivoRecursoReconsideracion Is Nothing Then
                    erroresValidation.Add(validation.CrearRespuestaMallaValidacion("049", "No hay recurso de reconsideración"))
                End If
            End If

            If Not IsNothing(titulo.existeFalloCasacion) AndAlso (titulo.existeFalloCasacion.HasValue And titulo.existeFalloCasacion.Value) Then
                If titulo.tituloEjecutivoFalloCasacion Is Nothing Then
                    erroresValidation.Add(validation.CrearRespuestaMallaValidacion("050", "No hay fallo casación"))
                End If
            End If

            If titulo.existeSentenciaSegundaInstancia.HasValue And titulo.existeSentenciaSegundaInstancia.Value Then
                If titulo.tituloEjecutivoSentenciaSegundaInstancia Is Nothing Then
                    erroresValidation.Add(validation.CrearRespuestaMallaValidacion("051", "No hay segunda instancia"))
                End If
            End If
            'En caso de error de validación
            If erroresValidation IsNot Nothing AndAlso erroresValidation.Count > 0 Then
                response.codigoError = "ERROR"
                insertarTitulo = False
            End If

            'Mapping de  deudores
            Dim deudor As New Deudor
            If Not IsNothing(deudorProvisional) Then
                'deudor.direccionUbicacion = Me.ObtenerDireccion(deudorProvisional.direccionesubicacion)
                deudor.nombreDeudor = deudorProvisional.valNombreDeudor
                deudor.numeroIdentificacion = deudorProvisional.valNumeroIdentificacion
                Dim _idTipoPersona = Me.ObtenerHomologacion(deudorProvisional.idTipoPersona, Entidades.Enumeraciones.Homologacion.TipoPersona)
                If IsNothing(_idTipoPersona) Then
                    erroresValidation.Add(CrearRespuestaMallaValidacion("056", FormatearMensajeHomologacion("tipo de persona", deudorProvisional.idTipoPersona)))
                End If
                deudor.tipoPersona = _idTipoPersona
                Dim _idTipoIdentificacion = Me.ObtenerHomologacion(deudorProvisional.idTipoIdentificacion, Entidades.Enumeraciones.Homologacion.TipoIdentificacion)
                If IsNothing(_idTipoIdentificacion) Then
                    erroresValidation.Add(CrearRespuestaMallaValidacion("055", FormatearMensajeHomologacion("tipo de identificación", deudorProvisional.idTipoIdentificacion)))
                End If
                deudor.tipoIdentificacion = _idTipoIdentificacion
            End If
            titulo.deudor = deudor

            Dim direccionList As New List(Of DireccionUbicacion)
            If Not IsNothing(deudorProvisional) Then
                direccionList.Add(Me.ObtenerDireccion(deudorProvisional.direccionesubicacion, deudor))
            End If

            Dim documentList As New List(Of DocumentoMaestroTitulo)
            If Not IsNothing(request.tituloOrigen(0).documentos) Then
                For Each item In request.tituloOrigen(0).documentos
                    documentList.Add(
                        New DocumentoMaestroTitulo With {
                            .NOM_DOC_AO = item.valNombreDocumento,
                            .COD_TIPO_DOCUMENTO_AO = item.codTipoDocumento,
                            .COD_GUID = item.codDocumentic,
                            .TIPO_RUTA = 2,
                            .ID_DOCUMENTO_TITULO = Nothing,
                            .DES_RUTA_DOCUMENTO = " ",
                            .NUM_PAGINAS = 0,
                            .ID_MAESTRO_TITULOS_DOCUMENTOS = 0,
                            .OBSERVA_LEGIBILIDAD = " "
                     })
                Next
            End If

            'Homologaciones
            Dim _tipoCartera As String = Me.ObtenerHomologacion(titulo.tipoCartera.ToString, Entidades.Enumeraciones.Homologacion.TipoCartera)
            If Not IsNothing(_tipoCartera) Then
                titulo.tipoCartera = Integer.Parse(_tipoCartera.TrimStart("0"c))
            Else
                erroresValidation.Add(CrearRespuestaMallaValidacion("057", FormatearMensajeHomologacion("tipo cartera", titulo.tipoCartera)))
            End If

            Dim _areaOrigen As String = Me.ObtenerHomologacion(titulo.areaOrigen.ToString, Entidades.Enumeraciones.Homologacion.AreaOrigen)
            If Not IsNothing(_areaOrigen) Then
                titulo.areaOrigen = _areaOrigen
            Else
                erroresValidation.Add(CrearRespuestaMallaValidacion("058", FormatearMensajeHomologacion("área origen", titulo.areaOrigen)))
            End If

            Dim _tipoTitulo As String = Me.ObtenerHomologacion(If(IsNothing(titulo.tipoTitulo), String.Empty, titulo.tipoTitulo.ToString), Entidades.Enumeraciones.Homologacion.TipoTituloOrigen)
            If Not IsNothing(_tipoTitulo) Then
                titulo.tipoTitulo = _tipoTitulo
            Else
                erroresValidation.Add(CrearRespuestaMallaValidacion("059", FormatearMensajeHomologacion("tipo título", If(IsNothing(titulo.tipoTitulo), String.Empty, titulo.tipoTitulo.ToString))))
            End If

            Dim _formaNotificacion As String = Me.ObtenerHomologacion(If(IsNothing(titulo.formaNotificacion), String.Empty, titulo.formaNotificacion.ToString), Entidades.Enumeraciones.Homologacion.FormaNotificacion)
            If Not IsNothing(_formaNotificacion) Then
                titulo.formaNotificacion = _tipoTitulo
            Else
                erroresValidation.Add(CrearRespuestaMallaValidacion("060", FormatearMensajeHomologacion("forma de notificación", If(IsNothing(titulo.tipoTitulo), String.Empty, titulo.tipoTitulo.ToString))))
            End If
            'Fin Homologaciones

            Dim deudorList As New List(Of Deudor)
            deudor = CompletarDeudor(deudor)
            deudorList.Add(deudor)

            titulo.Automatico = True
            If Not titulo.TotalSancion.HasValue Then
                titulo.TotalSancion = 0
            End If

            If titulo.tituloEjecutivoRecursoReposicion Is Nothing Then
                titulo.tituloEjecutivoRecursoReposicion = CompletarTtituloEspecialDefault()
            End If
            If titulo.tituloEjecutivoRecursoReconsideracion Is Nothing Then
                titulo.tituloEjecutivoRecursoReconsideracion = CompletarTtituloEspecialDefault()
            End If

            If titulo.fechaNotificacion.HasValue AndAlso titulo.fechaNotificacion = Date.MinValue Then
                titulo.fechaNotificacion = ObenerFechaDefault()
            End If

            If titulo.sancionMora Is Nothing Then
                titulo.sancionMora = 0
            End If
            If titulo.sancionInexactitud Is Nothing Then
                titulo.sancionInexactitud = 0
            End If
            If titulo.sancionOmision Is Nothing Then
                titulo.sancionOmision = 0
            End If
            'Se asigna a la fecha de exigilidad la fecha ejecutoria + 1 dia
            If titulo.fechaEjecutoria.HasValue Then
                titulo.fechaExigibilidad = titulo.fechaEjecutoria.Value.AddDays(1)
            End If

            insertarTitulo = If(erroresValidation.Count > 0, False, True)
            idTitulo = validation.MallaValidadoraTituloEjecutivoGlobal(titulo, documentList, deudorList, direccionList, Nothing, Nothing, insertarTitulo)
            responseTitle = validation.respuesta

            'Creación de contexto transaccional
            If request.contextoTransaccionalTipo IsNot Nothing Then
                resultContext = context.Add(request.contextoTransaccionalTipo, idTitulo)
            End If
            'Verificación de la respuesta
            If responseTitle.Count = 0 And erroresValidation.Count = 0 Then
                response.codigoError = "00"
                response.resultadoEjecucion = "00"
            Else
                erroresValidation.AddRange(responseTitle)
                response.codigoError = "01"
                For Each item In erroresValidation
                    errorMallaValidacion.Add(item.codigo & ":" & item.respuesta & ";")
                Next
                response.resultadoEjecucion = "01"
                response.detalleError = String.Join(",", errorMallaValidacion)
            End If
            response.contextoRespuesta = New ContextoRespuestaTipo_old With {
                .valNumPagina = 0,
                .valCantidadPaginas = 0,
                .idTx = request.contextoTransaccionalTipo.idTx,
                .fechaTx = Date.Now,
                .idInstanciaProceso = request.contextoTransaccionalTipo.idDefinicionProceso,
                .idInstanciaActividad = request.contextoTransaccionalTipo.idInstanciaActividad
            }

        Catch ex As Exception
            Throw ex
        End Try
        Return response
    End Function

    ''' <summary>
    ''' Funcion encargada de obtener la omologación
    ''' </summary>
    ''' <param name="origen"></param>
    ''' <param name="tipo"></param>
    ''' <returns></returns>
    Public Function ObtenerHomologacion(ByVal origen As String, ByVal tipo As Integer) As String
        Dim _homologaciones = New HomologacionDAL().ConsultaDatValores(origen, tipo)
        If _homologaciones.Count > 0 Then
            Return _homologaciones(0).DESTINO
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Setea valor por defecto a las fechas
    ''' </summary>
    ''' <returns></returns>
    Private Function ObenerFechaDefault() As Date
        Return Date.MinValue.AddYears(1990)
    End Function

    ''' <summary>
    ''' Completa el objeto deudor con valores por defecto
    ''' </summary>
    ''' <param name="deudor"></param>
    ''' <returns></returns>
    Private Function CompletarDeudor(ByVal deudor As Deudor) As Deudor
        Dim d As DeudorContract
        deudor.digitoVerificacion = "1"
        deudor.MatriculaMercantil = String.Empty
        deudor.EstadoPersona = "01"
        deudor.NomEstadoPersona = "ACTIVA"
        deudor.PorcentajeParticipacion = "0"
        deudor.TarjetaProfesional = String.Empty
        deudor.TipoAportante = "00"
        deudor.TipoEnte = 1
        Return deudor
    End Function

    ''' <summary>
    ''' Convierte una DireccionUbicacionContract a DireccionUbicacion
    ''' </summary>
    ''' <param name="direccion">objeto de tipo DireccionUbicacionContract</param>
    ''' <returns>retorna objeto de tipo DireccionUbicacion</returns>
    Public Function ObtenerDireccion(ByVal direccion As DireccionUbicacionContract, Optional ByVal deudor As Deudor = Nothing) As DireccionUbicacion
        Dim direccionUbicacion As New DireccionUbicacion
        direccionUbicacion.celular = direccion.valCelular
        direccionUbicacion.direccionCompleta = direccion.valDireccionCompleta
        direccionUbicacion.email = direccion.valemail
        Dim _fuentesDirecciones = Me.ObtenerHomologacion(direccion.idFuenteDireccion, Entidades.Enumeraciones.Homologacion.FuenteDireccion)
        If IsNothing(_fuentesDirecciones) Then
            erroresValidation.Add(CrearRespuestaMallaValidacion("054", FormatearMensajeHomologacion("fuente de dirección", direccion.idFuenteDireccion)))
        End If
        direccionUbicacion.fuentesDirecciones = _fuentesDirecciones
        direccionUbicacion.telefono = direccion.valTelefono
        Dim _idDepartamento = Me.ObtenerHomologacion(direccion.codDepartamento, Entidades.Enumeraciones.Homologacion.Departamento)
        If IsNothing(_idDepartamento) Then
            erroresValidation.Add(CrearRespuestaMallaValidacion("053", FormatearMensajeHomologacion("departamento", direccion.codDepartamento)))
        End If
        direccionUbicacion.idDepartamento = _idDepartamento
        Dim _idCiudad = Me.ObtenerHomologacion(direccion.codCiudad, Entidades.Enumeraciones.Homologacion.Ciudad)
        If IsNothing(_idCiudad) Then
            erroresValidation.Add(CrearRespuestaMallaValidacion("052", FormatearMensajeHomologacion("ciudad", direccion.codCiudad)))
        End If
        direccionUbicacion.idCiudad = _idCiudad
        If deudor IsNot Nothing Then
            direccionUbicacion.numeroidentificacionDeudor = deudor.numeroIdentificacion
        End If
        Return direccionUbicacion
    End Function

    ''' <summary>
    ''' Completa un titulo especial con valores default
    ''' </summary>
    ''' <returns></returns>
    Private Function CompletarTtituloEspecialDefault() As TituloEspecial
        Dim titulo As New TituloEspecial()
        titulo.numeroTitulo = ""
        titulo.formaNotificacion = ""
        titulo.fechaNotificacion = Nothing
        titulo.fechaTituloEjecutivo = ObenerFechaDefault()
        Return titulo
    End Function

    ''' <summary>
    ''' Metodo encargado de validar que los campos requeridos dentro del contexto transaccional esten definidos
    ''' </summary>
    ''' <param name="prmObjContextoTransaccionalTipo">Objeto del tipo ContextoTransaccionalTipo</param>
    ''' <returns>Objeto del tipo RespuestaMallaValidacion</returns>
    Public Function ValidarContextoTransaccional(ByVal prmObjContextoTransaccionalTipo As ContextoTransaccionalTipo) As List(Of RespuestaMallaValidacion)
        Dim respuesta As New List(Of RespuestaMallaValidacion)

        If prmObjContextoTransaccionalTipo.idTx Is Nothing Then
            respuesta.Add(CrearRespuestaMallaValidacion("042", My.Resources.idtx_ct))
        End If
        If (IsNothing(prmObjContextoTransaccionalTipo.fechaInicioTx) OrElse prmObjContextoTransaccionalTipo.fechaInicioTx < Date.Parse("1/1/1753 12:00:00 AM")) Then
            respuesta.Add(CrearRespuestaMallaValidacion("043", My.Resources.fechaInicio_ct))
        End If
        If prmObjContextoTransaccionalTipo.idDefinicionProceso Is Nothing Then
            respuesta.Add(CrearRespuestaMallaValidacion("044", My.Resources.idDefinicionProceso_ct))
        End If
        If prmObjContextoTransaccionalTipo.valNombreDefinicionProceso Is Nothing Then
            respuesta.Add(CrearRespuestaMallaValidacion("045", My.Resources.nombrePorceso_ct))
        End If
        If prmObjContextoTransaccionalTipo.idUsuarioAplicacion Is Nothing Then
            respuesta.Add(CrearRespuestaMallaValidacion("046", My.Resources.idUsuarioApp_ct))
        End If
        If prmObjContextoTransaccionalTipo.idEmisor Is Nothing Then
            respuesta.Add(CrearRespuestaMallaValidacion("047", My.Resources.idEmisor_ct))
        End If

        Return respuesta
    End Function


    Public Function FormatearRespuesta(ByVal prmListRespuestaMallaValidacion As List(Of RespuestaMallaValidacion)) As String
        Dim errorMallaValidacion As New List(Of String)
        For Each item In prmListRespuestaMallaValidacion
            errorMallaValidacion.Add(item.codigo & ":" & item.respuesta & ";")
        Next
        Return String.Join(",", errorMallaValidacion)
    End Function

    Private Function FormatearMensajeHomologacion(ByVal prmStrTipoHologacion As String, ByVal prmStrCodigo As String) As String
        Return "El codigo de " & prmStrTipoHologacion & " " & prmStrCodigo & " no es valido"
    End Function
End Class
