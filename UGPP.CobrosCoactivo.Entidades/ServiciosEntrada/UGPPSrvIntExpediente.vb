Namespace UGPPSrvIntExpediente

    Partial Public Class OpBuscarPorIdExpedienteRespTipo
        Inherits Object

        Private contextoRespuestaField As ContextoRespuestaTipo

        Private expedientesField() As ExpedienteTipo

        Public Property contextoRespuesta() As ContextoRespuestaTipo
            Get
                Return Me.contextoRespuestaField
            End Get
            Set
                Me.contextoRespuestaField = Value
            End Set
        End Property

        Public Property expedientes() As ExpedienteTipo()
            Get
                Return Me.expedientesField
            End Get
            Set
                Me.expedientesField = Value
            End Set
        End Property

    End Class

    Partial Public Class DepartamentoTipo
        Inherits Object

        Private codDepartamentoField As String

        Private valNombreField As String

        Public Property codDepartamento() As String
            Get
                Return Me.codDepartamentoField
            End Get
            Set
                Me.codDepartamentoField = Value
            End Set
        End Property

        Public Property valNombre() As String
            Get
                Return Me.valNombreField
            End Get
            Set
                Me.valNombreField = Value
            End Set
        End Property

    End Class

    Partial Public Class MunicipioTipo
        Inherits Object

        Private codMunicipioField As String

        Private valNombreField As String

        Private departamentoField As DepartamentoTipo

        Public Property codMunicipio() As String
            Get
                Return Me.codMunicipioField
            End Get
            Set
                Me.codMunicipioField = Value
            End Set
        End Property

        Public Property valNombre() As String
            Get
                Return Me.valNombreField
            End Get
            Set
                Me.valNombreField = Value
            End Set
        End Property

        Public Property departamento() As DepartamentoTipo
            Get
                Return Me.departamentoField
            End Get
            Set
                Me.departamentoField = Value
            End Set
        End Property

    End Class

    Partial Public Class IdentificacionTipo
        Inherits Object

        Private codTipoIdentificacionField As String

        Private valNumeroIdentificacionField As String

        Private municipioExpedicionField As MunicipioTipo

        Public Property codTipoIdentificacion() As String
            Get
                Return Me.codTipoIdentificacionField
            End Get
            Set
                Me.codTipoIdentificacionField = Value
            End Set
        End Property

        Public Property valNumeroIdentificacion() As String
            Get
                Return Me.valNumeroIdentificacionField
            End Get
            Set
                Me.valNumeroIdentificacionField = Value
            End Set
        End Property

        Public Property municipioExpedicion() As MunicipioTipo
            Get
                Return Me.municipioExpedicionField
            End Get
            Set
                Me.municipioExpedicionField = Value
            End Set
        End Property

    End Class

    Partial Public Class SerieDocumentalTipo
        Inherits Object

        Private valNombreSerieField As String

        Private codSerieField As String

        Private valNombreSubserieField As String

        Private codSubserieField As String

        Private codTipoDocumentalField As String

        Public Property valNombreSerie() As String
            Get
                Return Me.valNombreSerieField
            End Get
            Set
                Me.valNombreSerieField = Value
            End Set
        End Property

        Public Property codSerie() As String
            Get
                Return Me.codSerieField
            End Get
            Set
                Me.codSerieField = Value
            End Set
        End Property

        Public Property valNombreSubserie() As String
            Get
                Return Me.valNombreSubserieField
            End Get
            Set
                Me.valNombreSubserieField = Value
            End Set
        End Property

        Public Property codSubserie() As String
            Get
                Return Me.codSubserieField
            End Get
            Set
                Me.codSubserieField = Value
            End Set
        End Property

        Public Property codTipoDocumental() As String
            Get
                Return Me.codTipoDocumentalField
            End Get
            Set
                Me.codTipoDocumentalField = Value
            End Set
        End Property

    End Class

    Partial Public Class ExpedienteTipo
        Inherits Object

        Private idNumExpedienteField As String

        Private codSeccionField As String

        Private valSeccionField As String

        Private codSubSeccionField As String

        Private valSubSeccionField As String

        Private serieDocumentalField As SerieDocumentalTipo

        Private valAnoAperturaField As String

        Private valFondoField As String

        Private valEntidadPredecesoraField As String

        Private identificacionField As IdentificacionTipo

        Private descDescripcionField As String

        Private idCarpetaFileNetField As String

        Public Property idNumExpediente() As String
            Get
                Return Me.idNumExpedienteField
            End Get
            Set
                Me.idNumExpedienteField = Value
            End Set
        End Property

        Public Property codSeccion() As String
            Get
                Return Me.codSeccionField
            End Get
            Set
                Me.codSeccionField = Value
            End Set
        End Property

        Public Property valSeccion() As String
            Get
                Return Me.valSeccionField
            End Get
            Set
                Me.valSeccionField = Value
            End Set
        End Property

        Public Property codSubSeccion() As String
            Get
                Return Me.codSubSeccionField
            End Get
            Set
                Me.codSubSeccionField = Value
            End Set
        End Property

        Public Property valSubSeccion() As String
            Get
                Return Me.valSubSeccionField
            End Get
            Set
                Me.valSubSeccionField = Value
            End Set
        End Property

        Public Property serieDocumental() As SerieDocumentalTipo
            Get
                Return Me.serieDocumentalField
            End Get
            Set
                Me.serieDocumentalField = Value
            End Set
        End Property

        Public Property valAnoApertura() As String
            Get
                Return Me.valAnoAperturaField
            End Get
            Set
                Me.valAnoAperturaField = Value
            End Set
        End Property

        Public Property valFondo() As String
            Get
                Return Me.valFondoField
            End Get
            Set
                Me.valFondoField = Value
            End Set
        End Property

        Public Property valEntidadPredecesora() As String
            Get
                Return Me.valEntidadPredecesoraField
            End Get
            Set
                Me.valEntidadPredecesoraField = Value
            End Set
        End Property

        Public Property identificacion() As IdentificacionTipo
            Get
                Return Me.identificacionField
            End Get
            Set
                Me.identificacionField = Value
            End Set
        End Property

        Public Property descDescripcion() As String
            Get
                Return Me.descDescripcionField
            End Get
            Set
                Me.descDescripcionField = Value
            End Set
        End Property

        Public Property idCarpetaFileNet() As String
            Get
                Return Me.idCarpetaFileNetField
            End Get
            Set
                Me.idCarpetaFileNetField = Value
            End Set
        End Property

    End Class

    Partial Public Class ContextoRespuestaTipo
        Inherits Object

        Private idTxField As String

        Private codEstadoTxField As String

        Private fechaTxField As Date

        Private idInstanciaProcesoField As String

        Private idInstanciaActividadField As String

        Private valCantidadPaginasField As String

        Private valNumPaginaField As String

        Public Property idTx() As String
            Get
                Return Me.idTxField
            End Get
            Set
                Me.idTxField = Value
            End Set
        End Property

        Public Property codEstadoTx() As String
            Get
                Return Me.codEstadoTxField
            End Get
            Set
                Me.codEstadoTxField = Value
            End Set
        End Property

        Public Property fechaTx() As Date
            Get
                Return Me.fechaTxField
            End Get
            Set
                Me.fechaTxField = Value
            End Set
        End Property

        Public Property idInstanciaProceso() As String
            Get
                Return Me.idInstanciaProcesoField
            End Get
            Set
                Me.idInstanciaProcesoField = Value
            End Set
        End Property

        Public Property idInstanciaActividad() As String
            Get
                Return Me.idInstanciaActividadField
            End Get
            Set
                Me.idInstanciaActividadField = Value
            End Set
        End Property

        Public Property valCantidadPaginas() As String
            Get
                Return Me.valCantidadPaginasField
            End Get
            Set
                Me.valCantidadPaginasField = Value
            End Set
        End Property

        Public Property valNumPagina() As String
            Get
                Return Me.valNumPaginaField
            End Get
            Set
                Me.valNumPaginaField = Value
            End Set
        End Property

    End Class

    Partial Public Class OpCrearExpedienteRespTipo
        Inherits Object

        Private contextoRespuestaField As ContextoRespuestaTipo

        Private expedienteField As ExpedienteTipo

        Public Property contextoRespuesta() As ContextoRespuestaTipo
            Get
                Return Me.contextoRespuestaField
            End Get
            Set
                Me.contextoRespuestaField = Value
            End Set
        End Property

        Public Property expediente() As ExpedienteTipo
            Get
                Return Me.expedienteField
            End Get
            Set
                Me.expedienteField = Value
            End Set
        End Property

    End Class

End Namespace