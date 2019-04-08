Namespace UGPPSvcDocumento
    Partial Public Class credencialesAutenticacion

        Private claveUsuarioSistemaField As String

        Private nombreUsuarioNegocioField As String

        Private nombreUsuarioSistemaField As String

        Public Property claveUsuarioSistema() As String
            Get
                Return Me.claveUsuarioSistemaField
            End Get
            Set
                Me.claveUsuarioSistemaField = Value
            End Set
        End Property

        Public Property nombreUsuarioNegocio() As String
            Get
                Return Me.nombreUsuarioNegocioField
            End Get
            Set
                Me.nombreUsuarioNegocioField = Value
            End Set
        End Property

        Public Property nombreUsuarioSistema() As String
            Get
                Return Me.nombreUsuarioSistemaField
            End Get
            Set
                Me.nombreUsuarioSistemaField = Value
            End Set
        End Property
    End Class
    Partial Public Class informacionConsultaDocumentosPorIdCarpeta
        Private credencialesField As credencialesAutenticacion

        Private documentoField As informacionDocumentoConsultar

        Private idCarpetaField As String

        '''<remarks/>
        Public Property credenciales() As credencialesAutenticacion
            Get
                Return Me.credencialesField
            End Get
            Set
                Me.credencialesField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property documento() As informacionDocumentoConsultar
            Get
                Return Me.documentoField
            End Get
            Set
                Me.documentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idCarpeta() As String
            Get
                Return Me.idCarpetaField
            End Get
            Set
                Me.idCarpetaField = Value
            End Set
        End Property
    End Class

    Partial Public Class informacionDocumentoConsultar

        Private agrupador1Field As String

        Private agrupador2Field As String

        Private agrupador3Field As String

        Private agrupador4Field As String

        Private agrupador5Field As String

        Private agrupador6Field As String

        Private agrupador7Field As String

        Private autorOriginadorField As String

        Private codigoEntidadOriginadoraField As String

        Private codigoSerieField As String

        Private codigoSubSerieField As String

        Private documentTitleField As String

        Private fechaRadicacionCorrespondenciaFinalField As String

        Private fechaRadicacionCorrespondenciaInicialField As String

        Private idExpedienteField As String

        Private naturalezaDocumentoField As String

        Private nombreSerieField As String

        Private nombreSubSerieField As String

        Private numeroRadicadoCorrespondenciaField As String

        Private observacionLegibilidadField As String

        Private origenDocumentoField As String

        Private tipoDocumentalField As String

        '''<remarks/>
        Public Property agrupador1() As String
            Get
                Return Me.agrupador1Field
            End Get
            Set
                Me.agrupador1Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador2() As String
            Get
                Return Me.agrupador2Field
            End Get
            Set
                Me.agrupador2Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador3() As String
            Get
                Return Me.agrupador3Field
            End Get
            Set
                Me.agrupador3Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador4() As String
            Get
                Return Me.agrupador4Field
            End Get
            Set
                Me.agrupador4Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador5() As String
            Get
                Return Me.agrupador5Field
            End Get
            Set
                Me.agrupador5Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador6() As String
            Get
                Return Me.agrupador6Field
            End Get
            Set
                Me.agrupador6Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador7() As String
            Get
                Return Me.agrupador7Field
            End Get
            Set
                Me.agrupador7Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property autorOriginador() As String
            Get
                Return Me.autorOriginadorField
            End Get
            Set
                Me.autorOriginadorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codigoEntidadOriginadora() As String
            Get
                Return Me.codigoEntidadOriginadoraField
            End Get
            Set
                Me.codigoEntidadOriginadoraField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codigoSerie() As String
            Get
                Return Me.codigoSerieField
            End Get
            Set
                Me.codigoSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codigoSubSerie() As String
            Get
                Return Me.codigoSubSerieField
            End Get
            Set
                Me.codigoSubSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property documentTitle() As String
            Get
                Return Me.documentTitleField
            End Get
            Set
                Me.documentTitleField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaRadicacionCorrespondenciaFinal() As String
            Get
                Return Me.fechaRadicacionCorrespondenciaFinalField
            End Get
            Set
                Me.fechaRadicacionCorrespondenciaFinalField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaRadicacionCorrespondenciaInicial() As String
            Get
                Return Me.fechaRadicacionCorrespondenciaInicialField
            End Get
            Set
                Me.fechaRadicacionCorrespondenciaInicialField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idExpediente() As String
            Get
                Return Me.idExpedienteField
            End Get
            Set
                Me.idExpedienteField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property naturalezaDocumento() As String
            Get
                Return Me.naturalezaDocumentoField
            End Get
            Set
                Me.naturalezaDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property nombreSerie() As String
            Get
                Return Me.nombreSerieField
            End Get
            Set
                Me.nombreSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property nombreSubSerie() As String
            Get
                Return Me.nombreSubSerieField
            End Get
            Set
                Me.nombreSubSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property numeroRadicadoCorrespondencia() As String
            Get
                Return Me.numeroRadicadoCorrespondenciaField
            End Get
            Set
                Me.numeroRadicadoCorrespondenciaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property observacionLegibilidad() As String
            Get
                Return Me.observacionLegibilidadField
            End Get
            Set
                Me.observacionLegibilidadField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property origenDocumento() As String
            Get
                Return Me.origenDocumentoField
            End Get
            Set
                Me.origenDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property tipoDocumental() As String
            Get
                Return Me.tipoDocumentalField
            End Get
            Set
                Me.tipoDocumentalField = Value
            End Set
        End Property
    End Class
    Partial Public Class resultadoIdentificadorDocumentos
        Private documentoField() As documentoGeneral
        Private estadoEjecucionField As estadoEjecucion

        '''<remarks/>
        Public Property documento() As documentoGeneral()
            Get
                Return Me.documentoField
            End Get
            Set
                Me.documentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property estadoEjecucion() As estadoEjecucion
            Get
                Return Me.estadoEjecucionField
            End Get
            Set
                Me.estadoEjecucionField = Value
            End Set
        End Property
    End Class

    Partial Public Class documentoGeneral

        Private codigoEntidadOriginadoraField As String

        Private contenidoField As String

        Private datosDocumentoField As informacionDocumento

        Private descripcionExpedienteField As String

        Private direccionOficinaField As String

        Private extensionField As String

        Private idField As String

        Private idExpedienteField As String

        Private medioRecepcionField As String

        Private nombreEntidadOriginadoraField As String

        Private nombreTipoDocumentalField As String

        '''<remarks/>
        Public Property codigoEntidadOriginadora() As String
            Get
                Return Me.codigoEntidadOriginadoraField
            End Get
            Set
                Me.codigoEntidadOriginadoraField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property contenido() As String
            Get
                Return Me.contenidoField
            End Get
            Set
                Me.contenidoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property datosDocumento() As informacionDocumento
            Get
                Return Me.datosDocumentoField
            End Get
            Set
                Me.datosDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property descripcionExpediente() As String
            Get
                Return Me.descripcionExpedienteField
            End Get
            Set
                Me.descripcionExpedienteField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property direccionOficina() As String
            Get
                Return Me.direccionOficinaField
            End Get
            Set
                Me.direccionOficinaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property id() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property idExpediente() As String
            Get
                Return Me.idExpedienteField
            End Get
            Set
                Me.idExpedienteField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property medioRecepcion() As String
            Get
                Return Me.medioRecepcionField
            End Get
            Set
                Me.medioRecepcionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property nombreEntidadOriginadora() As String
            Get
                Return Me.nombreEntidadOriginadoraField
            End Get
            Set
                Me.nombreEntidadOriginadoraField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property nombreTipoDocumental() As String
            Get
                Return Me.nombreTipoDocumentalField
            End Get
            Set
                Me.nombreTipoDocumentalField = Value
            End Set
        End Property
    End Class
    Partial Public Class estadoEjecucion

        Private codigoField As String

        Private descripcionField As String

        '''<remarks/>
        Public Property codigo() As String
            Get
                Return Me.codigoField
            End Get
            Set
                Me.codigoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property descripcion() As String
            Get
                Return Me.descripcionField
            End Get
            Set
                Me.descripcionField = Value
            End Set
        End Property
    End Class

    Partial Public Class informacionDocumento

        Private agrupador1Field As String

        Private agrupador2Field As String

        Private agrupador3Field As String

        Private agrupador4Field As String

        Private agrupador5Field As String

        Private agrupador6Field As String

        Private agrupador7Field As String

        Private autorOriginadorField As String

        Private codigoSerieField As String

        Private codigoSubSerieField As String

        Private documentTitleField As String

        Private fechaDocumentoField As String

        Private fechaRadicacionCorrespondenciaField As String

        Private legibleField As String

        Private naturalezaDocumentoField As String

        Private nombreSerieField As String

        Private nombreSubSerieField As String

        Private numeroPaginasDocumentoField As String

        Private numeroRadicadoCorrespondenciaField As String

        Private observacionLegibilidadField As String

        Private origenDocumentoField As String

        Private tipoDocumentalField As String

        '''<remarks/>
        Public Property agrupador1() As String
            Get
                Return Me.agrupador1Field
            End Get
            Set
                Me.agrupador1Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador2() As String
            Get
                Return Me.agrupador2Field
            End Get
            Set
                Me.agrupador2Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador3() As String
            Get
                Return Me.agrupador3Field
            End Get
            Set
                Me.agrupador3Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador4() As String
            Get
                Return Me.agrupador4Field
            End Get
            Set
                Me.agrupador4Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador5() As String
            Get
                Return Me.agrupador5Field
            End Get
            Set
                Me.agrupador5Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador6() As String
            Get
                Return Me.agrupador6Field
            End Get
            Set
                Me.agrupador6Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property agrupador7() As String
            Get
                Return Me.agrupador7Field
            End Get
            Set
                Me.agrupador7Field = Value
            End Set
        End Property

        '''<remarks/>
        Public Property autorOriginador() As String
            Get
                Return Me.autorOriginadorField
            End Get
            Set
                Me.autorOriginadorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codigoSerie() As String
            Get
                Return Me.codigoSerieField
            End Get
            Set
                Me.codigoSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property codigoSubSerie() As String
            Get
                Return Me.codigoSubSerieField
            End Get
            Set
                Me.codigoSubSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property documentTitle() As String
            Get
                Return Me.documentTitleField
            End Get
            Set
                Me.documentTitleField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaDocumento() As String
            Get
                Return Me.fechaDocumentoField
            End Get
            Set
                Me.fechaDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property fechaRadicacionCorrespondencia() As String
            Get
                Return Me.fechaRadicacionCorrespondenciaField
            End Get
            Set
                Me.fechaRadicacionCorrespondenciaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property legible() As String
            Get
                Return Me.legibleField
            End Get
            Set
                Me.legibleField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property naturalezaDocumento() As String
            Get
                Return Me.naturalezaDocumentoField
            End Get
            Set
                Me.naturalezaDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property nombreSerie() As String
            Get
                Return Me.nombreSerieField
            End Get
            Set
                Me.nombreSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property nombreSubSerie() As String
            Get
                Return Me.nombreSubSerieField
            End Get
            Set
                Me.nombreSubSerieField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property numeroPaginasDocumento() As String
            Get
                Return Me.numeroPaginasDocumentoField
            End Get
            Set
                Me.numeroPaginasDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property numeroRadicadoCorrespondencia() As String
            Get
                Return Me.numeroRadicadoCorrespondenciaField
            End Get
            Set
                Me.numeroRadicadoCorrespondenciaField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property observacionLegibilidad() As String
            Get
                Return Me.observacionLegibilidadField
            End Get
            Set
                Me.observacionLegibilidadField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property origenDocumento() As String
            Get
                Return Me.origenDocumentoField
            End Get
            Set
                Me.origenDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property tipoDocumental() As String
            Get
                Return Me.tipoDocumentalField
            End Get
            Set
                Me.tipoDocumentalField = Value
            End Set
        End Property

    End Class
End Namespace