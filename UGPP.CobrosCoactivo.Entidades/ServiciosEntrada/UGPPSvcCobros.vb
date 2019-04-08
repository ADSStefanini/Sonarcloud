Namespace UGPPSvcCobros

    Partial Public Class ActualizacionEstadoProcesoCobros
        Private idTituloOrigenField As String
        Public Property idTituloOrigen() As String
            Get
                Return idTituloOrigenField
            End Get
            Set(ByVal value As String)
                idTituloOrigenField = value
            End Set
        End Property

        Private resultadoEvaluacionField As String
        Public Property resultadoEvaluacion() As String
            Get
                Return resultadoEvaluacionField
            End Get
            Set(ByVal value As String)
                resultadoEvaluacionField = value
            End Set
        End Property

        Private observacionesEvaluacionTituloField As String
        Public Property observacionesEvaluacionTitulo() As String
            Get
                Return observacionesEvaluacionTituloField
            End Get
            Set(ByVal value As String)
                observacionesEvaluacionTituloField = value
            End Set
        End Property

        Private usuarioEvaluadorField As String
        Public Property usuarioEvaluador() As String
            Get
                Return usuarioEvaluadorField
            End Get
            Set(ByVal value As String)
                usuarioEvaluadorField = value
            End Set
        End Property

        Private fechaEvaluacionField As DateTime
        Public Property fechaEvaluacion() As DateTime
            Get
                Return fechaEvaluacionField
            End Get
            Set(ByVal value As DateTime)
                fechaEvaluacionField = value
            End Set
        End Property

        Private observacionesField As List(Of Documento)
        Public Property observacion() As List(Of Documento)
            Get
                Return observacionesField
            End Get
            Set(ByVal value As List(Of Documento))
                observacionesField = value
            End Set
        End Property

        Private observacionesDocumentoField As List(Of ObservacionDocumento)
        Public Property NewProperty() As List(Of ObservacionDocumento)
            Get
                Return observacionesDocumentoField
            End Get
            Set(ByVal value As List(Of ObservacionDocumento))
                observacionesDocumentoField = value
            End Set
        End Property

    End Class

    Partial Public Class ObservacionDocumento
        Private observacionField As String
        Public Property observacion() As String
            Get
                Return observacionField
            End Get
            Set(ByVal value As String)
                observacionField = value
            End Set
        End Property
    End Class

    Partial Public Class Documento
        Private valNombreDocumentoField As String
        Public Property valNombreDocumento() As String
            Get
                Return valNombreDocumentoField
            End Get
            Set(ByVal value As String)
                valNombreDocumentoField = value
            End Set
        End Property

        Private codDocumenticField As String
        Public Property codDocumentic() As String
            Get
                Return codDocumenticField
            End Get
            Set(ByVal value As String)
                codDocumenticField = value
            End Set
        End Property

        Private codTipoDocumentoField As String
        Public Property codTipoDocumento() As String
            Get
                Return codTipoDocumentoField
            End Get
            Set(ByVal value As String)
                codTipoDocumentoField = value
            End Set
        End Property

    End Class

    Partial Public Class RspActualizacionEstadoProcesoCobros
        Private resultadoEjecucionField As String
        Public Property resultadoEjecucion() As String
            Get
                Return resultadoEjecucionField
            End Get
            Set(ByVal value As String)
                resultadoEjecucionField = value
            End Set
        End Property

        Private codigoErrorField As String
        Public Property codigoError() As String
            Get
                Return codigoErrorField
            End Get
            Set(ByVal value As String)
                codigoErrorField = value
            End Set
        End Property

        Private detalleErrorField As String
        Public Property detalleError() As String
            Get
                Return detalleErrorField
            End Get
            Set(ByVal value As String)
                detalleErrorField = value
            End Set
        End Property

    End Class


End Namespace