Namespace UGPPSrvIntCobros
    Partial Public Class ActualizacionEstadoProcesoCobrosReq
        Inherits Object

        Private idTituloOrigenField As String

        Private idTipoCarteraField As String

        Private resultadoEvaluacionField As String

        Private observacionesEvaluacionTituloField As String

        Private usuarioEvaluadorField As String

        Private fechaEvaluacionField As String

        Private documentosField() As DocumentoCobros

        Private contextoTransaccionalField As ContextoTransaccionalTipo

        '''<remarks/>
        Public Property idTituloOrigen() As String
            Get
                Return Me.idTituloOrigenField
            End Get
            Set
                Me.idTituloOrigenField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idTipoCartera() As String
            Get
                Return Me.idTipoCarteraField
            End Get
            Set
                Me.idTipoCarteraField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property resultadoEvaluacion() As String
            Get
                Return Me.resultadoEvaluacionField
            End Get
            Set
                Me.resultadoEvaluacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property observacionesEvaluacionTitulo() As String
            Get
                Return Me.observacionesEvaluacionTituloField
            End Get
            Set
                Me.observacionesEvaluacionTituloField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property usuarioEvaluador() As String
            Get
                Return Me.usuarioEvaluadorField
            End Get
            Set
                Me.usuarioEvaluadorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaEvaluacion() As String
            Get
                Return Me.fechaEvaluacionField
            End Get
            Set
                Me.fechaEvaluacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property documentos() As DocumentoCobros()
            Get
                Return Me.documentosField
            End Get
            Set
                Me.documentosField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property ContextoTransaccional() As ContextoTransaccionalTipo
            Get
                Return Me.contextoTransaccionalField
            End Get
            Set
                Me.contextoTransaccionalField = Value
            End Set
        End Property


    End Class


    Partial Public Class DocumentoCobros
        Inherits Object

        Private valNombreDocumentoField As String

        Private codDocumenticField As String

        Private codTipoDocumentoField As String

        Private observacionesDocumentoField As String

        '''<remarks/>
        Public Property valNombreDocumento() As String
            Get
                Return Me.valNombreDocumentoField
            End Get
            Set
                Me.valNombreDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codDocumentic() As String
            Get
                Return Me.codDocumenticField
            End Get
            Set
                Me.codDocumenticField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codTipoDocumento() As String
            Get
                Return Me.codTipoDocumentoField
            End Get
            Set
                Me.codTipoDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property observacionesDocumento() As String
            Get
                Return Me.observacionesDocumentoField
            End Get
            Set
                Me.observacionesDocumentoField = Value
            End Set
        End Property
    End Class

    '''<remarks/>

    Partial Public Class IniciarInstanciaCobrosResp
        Inherits Object

        Private contextoRespuestaField As FalloTipo

        '''<remarks/>
        Public Property ContextoRespuesta() As FalloTipo
            Get
                Return Me.contextoRespuestaField
            End Get
            Set
                Me.contextoRespuestaField = Value
            End Set
        End Property
    End Class


    Partial Public Class FalloTipo
        Inherits Object

        Private contextoRespuestaField As ContextoRespuestaTipo

        Private erroresField() As ErrorTipo

        '''<remarks/>
        Public Property contextoRespuesta() As ContextoRespuestaTipo
            Get
                Return Me.contextoRespuestaField
            End Get
            Set
                Me.contextoRespuestaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property errores() As ErrorTipo()
            Get
                Return Me.erroresField
            End Get
            Set
                Me.erroresField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    Partial Public Class ContextoRespuestaTipo
        Inherits Object

        Private idTxField As String

        Private codEstadoTxField As String

        Private fechaTxField As Date

        Private idInstanciaProcesoField As String

        Private idInstanciaActividadField As String

        Private valCantidadPaginasField As String

        Private valNumPaginaField As String

        '''<remarks/>
        Public Property idTx() As String
            Get
                Return Me.idTxField
            End Get
            Set
                Me.idTxField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codEstadoTx() As String
            Get
                Return Me.codEstadoTxField
            End Get
            Set
                Me.codEstadoTxField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaTx() As Date
            Get
                Return Me.fechaTxField
            End Get
            Set
                Me.fechaTxField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idInstanciaProceso() As String
            Get
                Return Me.idInstanciaProcesoField
            End Get
            Set
                Me.idInstanciaProcesoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idInstanciaActividad() As String
            Get
                Return Me.idInstanciaActividadField
            End Get
            Set
                Me.idInstanciaActividadField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valCantidadPaginas() As String
            Get
                Return Me.valCantidadPaginasField
            End Get
            Set
                Me.valCantidadPaginasField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valNumPagina() As String
            Get
                Return Me.valNumPaginaField
            End Get
            Set
                Me.valNumPaginaField = Value
            End Set
        End Property
    End Class

    Partial Public Class ErrorTipo
        Inherits Object

        Private codErrorField As String

        Private valDescErrorField As String

        Private valDescErrorTecnicoField As String

        '''<remarks/>
        Public Property codError() As String
            Get
                Return Me.codErrorField
            End Get
            Set
                Me.codErrorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valDescError() As String
            Get
                Return Me.valDescErrorField
            End Get
            Set
                Me.valDescErrorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valDescErrorTecnico() As String
            Get
                Return Me.valDescErrorTecnicoField
            End Get
            Set
                Me.valDescErrorTecnicoField = Value
            End Set
        End Property
    End Class

    '''<remarks/>

    Partial Public Class ObservacionesDocumento
        Inherits Object

        Private usuarioField As String

        Private fechaField As String

        Private detalleField As String

        '''<remarks/>
        Public Property usuario() As String
            Get
                Return Me.usuarioField
            End Get
            Set
                Me.usuarioField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fecha() As String
            Get
                Return Me.fechaField
            End Get
            Set
                Me.fechaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property detalle() As String
            Get
                Return Me.detalleField
            End Get
            Set
                Me.detalleField = Value
            End Set
        End Property
    End Class

    '''<remarks/>

    Partial Public Class Documento
        Inherits Object

        Private tipoDocumentalField As String

        Private esValidoField As String

        Private observacionesField() As ObservacionesDocumento

        '''<remarks/>
        Public Property tipoDocumental() As String
            Get
                Return Me.tipoDocumentalField
            End Get
            Set
                Me.tipoDocumentalField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property esValido() As String
            Get
                Return Me.esValidoField
            End Get
            Set
                Me.esValidoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Observaciones() As ObservacionesDocumento()
            Get
                Return Me.observacionesField
            End Get
            Set
                Me.observacionesField = Value
            End Set
        End Property
    End Class

    '''<remarks/>

    Partial Public Class DireccionNotificacion
        Inherits Object

        Private valDireccionCompletaField As String

        Private codDepartamentoField As String

        Private codCiudadField As String

        Private valTelefonoField As String

        Private valTelefono1Field As String

        Private valEmailField As String

        Private idFuenteDireccionField As String

        Private idOtrasFuentesDireccionesField As String

        '''<remarks/>
        Public Property valDireccionCompleta() As String
            Get
                Return Me.valDireccionCompletaField
            End Get
            Set
                Me.valDireccionCompletaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codDepartamento() As String
            Get
                Return Me.codDepartamentoField
            End Get
            Set
                Me.codDepartamentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codCiudad() As String
            Get
                Return Me.codCiudadField
            End Get
            Set
                Me.codCiudadField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valTelefono() As String
            Get
                Return Me.valTelefonoField
            End Get
            Set
                Me.valTelefonoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valTelefono1() As String
            Get
                Return Me.valTelefono1Field
            End Get
            Set
                Me.valTelefono1Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valEmail() As String
            Get
                Return Me.valEmailField
            End Get
            Set
                Me.valEmailField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idFuenteDireccion() As String
            Get
                Return Me.idFuenteDireccionField
            End Get
            Set
                Me.idFuenteDireccionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idOtrasFuentesDirecciones() As String
            Get
                Return Me.idOtrasFuentesDireccionesField
            End Get
            Set
                Me.idOtrasFuentesDireccionesField = Value
            End Set
        End Property
    End Class

    '''<remarks/>

    Partial Public Class Deudor
        Inherits Object

        Private idTipoPersonaField As String

        Private idTipoIdentificacionField As String

        Private nombreCompletoField As String

        Private numeroIdentificacionField As String

        Private direccionNotificacionField As DireccionNotificacion

        '''<remarks/>
        Public Property idTipoPersona() As String
            Get
                Return Me.idTipoPersonaField
            End Get
            Set
                Me.idTipoPersonaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idTipoIdentificacion() As String
            Get
                Return Me.idTipoIdentificacionField
            End Get
            Set
                Me.idTipoIdentificacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property nombreCompleto() As String
            Get
                Return Me.nombreCompletoField
            End Get
            Set
                Me.nombreCompletoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property numeroIdentificacion() As String
            Get
                Return Me.numeroIdentificacionField
            End Get
            Set
                Me.numeroIdentificacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property DireccionNotificacion() As DireccionNotificacion
            Get
                Return Me.direccionNotificacionField
            End Get
            Set
                Me.direccionNotificacionField = Value
            End Set
        End Property


    End Class


    Partial Public Class TituloEjecutivo
        Inherits Object

        Private idTipoCarteraField As String

        Private idTipoObligacionField As String

        Private numeroExpedienteOrigenField As String

        Private idAreaOrigenField As String

        Private idTipoTituloField As String

        Private numeroTituloEjecutivoField As String

        Private fechaTituloEjecutivoField As String

        Private valorObligacionField As String

        Private idFormaNotificacionField As String

        Private fechaNotificacionTituloEjecutivoField As String

        Private presentaRecursoReconsideracionField As String

        Private presentaRecursoReposicionField As String

        Private existeSentenciaSegundaInstanciaField As String

        Private existeFalloCasacionField As String

        Private deudorField As Deudor

        Private documentosField() As Documento

        '''<remarks/>
        Public Property idTipoCartera() As String
            Get
                Return Me.idTipoCarteraField
            End Get
            Set
                Me.idTipoCarteraField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idTipoObligacion() As String
            Get
                Return Me.idTipoObligacionField
            End Get
            Set
                Me.idTipoObligacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property numeroExpedienteOrigen() As String
            Get
                Return Me.numeroExpedienteOrigenField
            End Get
            Set
                Me.numeroExpedienteOrigenField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idAreaOrigen() As String
            Get
                Return Me.idAreaOrigenField
            End Get
            Set
                Me.idAreaOrigenField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idTipoTitulo() As String
            Get
                Return Me.idTipoTituloField
            End Get
            Set
                Me.idTipoTituloField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property numeroTituloEjecutivo() As String
            Get
                Return Me.numeroTituloEjecutivoField
            End Get
            Set
                Me.numeroTituloEjecutivoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaTituloEjecutivo() As String
            Get
                Return Me.fechaTituloEjecutivoField
            End Get
            Set
                Me.fechaTituloEjecutivoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valorObligacion() As String
            Get
                Return Me.valorObligacionField
            End Get
            Set
                Me.valorObligacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idFormaNotificacion() As String
            Get
                Return Me.idFormaNotificacionField
            End Get
            Set
                Me.idFormaNotificacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaNotificacionTituloEjecutivo() As String
            Get
                Return Me.fechaNotificacionTituloEjecutivoField
            End Get
            Set
                Me.fechaNotificacionTituloEjecutivoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property presentaRecursoReconsideracion() As String
            Get
                Return Me.presentaRecursoReconsideracionField
            End Get
            Set
                Me.presentaRecursoReconsideracionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property presentaRecursoReposicion() As String
            Get
                Return Me.presentaRecursoReposicionField
            End Get
            Set
                Me.presentaRecursoReposicionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property existeSentenciaSegundaInstancia() As String
            Get
                Return Me.existeSentenciaSegundaInstanciaField
            End Get
            Set
                Me.existeSentenciaSegundaInstanciaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property existeFalloCasacion() As String
            Get
                Return Me.existeFalloCasacionField
            End Get
            Set
                Me.existeFalloCasacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Deudor() As Deudor
            Get
                Return Me.deudorField
            End Get
            Set
                Me.deudorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Documentos() As Documento()
            Get
                Return Me.documentosField
            End Get
            Set
                Me.documentosField = Value
            End Set
        End Property


    End Class


    Partial Public Class IniciarInstanciaCobrosReq
        Inherits Object


        Private tituloEjecutivoField As TituloEjecutivo

        Private contextoTransaccionalField As ContextoTransaccionalTipo

        '''<remarks/>
        Public Property TituloEjecutivo() As TituloEjecutivo
            Get
                Return Me.tituloEjecutivoField
            End Get
            Set
                Me.tituloEjecutivoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property ContextoTransaccional() As ContextoTransaccionalTipo
            Get
                Return Me.contextoTransaccionalField
            End Get
            Set
                Me.contextoTransaccionalField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    Partial Public Class ContextoTransaccionalTipo
        Inherits Object


        Private idTxField As String

        Private fechaInicioTxField As Date

        Private idInstanciaProcesoField As String

        Private idDefinicionProcesoField As String

        Private valNombreDefinicionProcesoField As String

        Private idInstanciaActividadField As String

        Private valNombreDefinicionActividadField As String

        Private idUsuarioAplicacionField As String

        Private valClaveUsuarioAplicacionField As String

        Private idUsuarioField As String

        Private idEmisorField As String

        Private valTamPaginaField As String

        Private valNumPaginaField As String

        Private criteriosOrdenamientoField() As CriterioOrdenamientoTipo

        '''<remarks/>
        Public Property idTx() As String
            Get
                Return Me.idTxField
            End Get
            Set
                Me.idTxField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaInicioTx() As Date
            Get
                Return Me.fechaInicioTxField
            End Get
            Set
                Me.fechaInicioTxField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idInstanciaProceso() As String
            Get
                Return Me.idInstanciaProcesoField
            End Get
            Set
                Me.idInstanciaProcesoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idDefinicionProceso() As String
            Get
                Return Me.idDefinicionProcesoField
            End Get
            Set
                Me.idDefinicionProcesoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valNombreDefinicionProceso() As String
            Get
                Return Me.valNombreDefinicionProcesoField
            End Get
            Set
                Me.valNombreDefinicionProcesoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idInstanciaActividad() As String
            Get
                Return Me.idInstanciaActividadField
            End Get
            Set
                Me.idInstanciaActividadField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valNombreDefinicionActividad() As String
            Get
                Return Me.valNombreDefinicionActividadField
            End Get
            Set
                Me.valNombreDefinicionActividadField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idUsuarioAplicacion() As String
            Get
                Return Me.idUsuarioAplicacionField
            End Get
            Set
                Me.idUsuarioAplicacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valClaveUsuarioAplicacion() As String
            Get
                Return Me.valClaveUsuarioAplicacionField
            End Get
            Set
                Me.valClaveUsuarioAplicacionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idUsuario() As String
            Get
                Return Me.idUsuarioField
            End Get
            Set
                Me.idUsuarioField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idEmisor() As String
            Get
                Return Me.idEmisorField
            End Get
            Set
                Me.idEmisorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valTamPagina() As String
            Get
                Return Me.valTamPaginaField
            End Get
            Set
                Me.valTamPaginaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valNumPagina() As String
            Get
                Return Me.valNumPaginaField
            End Get
            Set
                Me.valNumPaginaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property criteriosOrdenamiento() As CriterioOrdenamientoTipo()
            Get
                Return Me.criteriosOrdenamientoField
            End Get
            Set
                Me.criteriosOrdenamientoField = Value
            End Set
        End Property


    End Class

    '''<remarks/>
    Partial Public Class CriterioOrdenamientoTipo
        Inherits Object


        Private valOrdenField As String

        Private valNombreCampoField As String

        '''<remarks/>
        Public Property valOrden() As String
            Get
                Return Me.valOrdenField
            End Get
            Set
                Me.valOrdenField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property valNombreCampo() As String
            Get
                Return Me.valNombreCampoField
            End Get
            Set
                Me.valNombreCampoField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    Partial Public Class ActualizacionEstadoProcesoCobrosResp
        Inherits Object


        Private contextoRespuestaField As FalloTipo

        '''<remarks/>
        Public Property ContextoRespuesta() As FalloTipo
            Get
                Return Me.contextoRespuestaField
            End Get
            Set
                Me.contextoRespuestaField = Value
            End Set
        End Property

    End Class
End Namespace
