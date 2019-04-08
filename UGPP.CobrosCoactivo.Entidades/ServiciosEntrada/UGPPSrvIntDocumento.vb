Namespace UGPPSrvIntDocumento

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/GestionDocumental/SrvIntDocumento/v1")>
    Partial Public Class OpConsultarDocumentoSolTipo
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged

        Private contextoTransaccionalField As ContextoTransaccionalTipo

        Private documentoField As DocumentoTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>
        Public Property contextoTransaccional() As ContextoTransaccionalTipo
            Get
                Return Me.contextoTransaccionalField
            End Get
            Set
                Me.contextoTransaccionalField = Value
                Me.RaisePropertyChanged("contextoTransaccional")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>
        Public Property documento() As DocumentoTipo
            Get
                Return Me.documentoField
            End Get
            Set
                Me.documentoField = Value
                Me.RaisePropertyChanged("documento")
            End Set
        End Property

        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/GestionDocumental/SrvIntDocumento/v1")>
    Partial Public Class OpConsultarDocumentoRespTipo
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged

        Private contextoRespuestaField As ContextoRespuestaTipo

        Private documentoField As DocumentoTipo

        Private correspondenciaField As CorrespondenciaTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>
        Public Property contextoRespuesta() As ContextoRespuestaTipo
            Get
                Return Me.contextoRespuestaField
            End Get
            Set
                Me.contextoRespuestaField = Value
                Me.RaisePropertyChanged("contextoRespuesta")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>
        Public Property documento() As DocumentoTipo
            Get
                Return Me.documentoField
            End Get
            Set
                Me.documentoField = Value
                Me.RaisePropertyChanged("documento")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=2)>
        Public Property correspondencia() As CorrespondenciaTipo
            Get
                Return Me.correspondenciaField
            End Get
            Set
                Me.correspondenciaField = Value
                Me.RaisePropertyChanged("correspondencia")
            End Set
        End Property

        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
 System.SerializableAttribute(),
 System.Diagnostics.DebuggerStepThroughAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/GestionDocumental/SrvIntDocumento/v1")>
    Partial Public Class OpIngresarDocumentoSolTipo
        Inherits Object

        Private contextoTransaccionalField As ContextoTransaccionalTipo

        Private documentosField() As DocumentoTipo

        Private expedienteField As ExpedienteTipo

        Private correspondenciaField As CorrespondenciaTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>
        Public Property contextoTransaccional() As ContextoTransaccionalTipo
            Get
                Return Me.contextoTransaccionalField
            End Get
            Set
                Me.contextoTransaccionalField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("documentos", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>
        Public Property documentos() As DocumentoTipo()
            Get
                Return Me.documentosField
            End Get
            Set
                Me.documentosField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=2)>
        Public Property expediente() As ExpedienteTipo
            Get
                Return Me.expedienteField
            End Get
            Set
                Me.expedienteField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=3)>
        Public Property correspondencia() As CorrespondenciaTipo
            Get
                Return Me.correspondenciaField
            End Get
            Set
                Me.correspondenciaField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/ContextoTransaccionalTipo/v1")>
    Partial Public Class ContextoTransaccionalTipo
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged

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
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property idTx() As String
            Get
                Return Me.idTxField
            End Get
            Set
                Me.idTxField = Value
                Me.RaisePropertyChanged("idTx")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>
        Public Property fechaInicioTx() As Date
            Get
                Return Me.fechaInicioTxField
            End Get
            Set
                Me.fechaInicioTxField = Value
                Me.RaisePropertyChanged("fechaInicioTx")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property idInstanciaProceso() As String
            Get
                Return Me.idInstanciaProcesoField
            End Get
            Set
                Me.idInstanciaProcesoField = Value
                Me.RaisePropertyChanged("idInstanciaProceso")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property idDefinicionProceso() As String
            Get
                Return Me.idDefinicionProcesoField
            End Get
            Set
                Me.idDefinicionProcesoField = Value
                Me.RaisePropertyChanged("idDefinicionProceso")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=4)>
        Public Property valNombreDefinicionProceso() As String
            Get
                Return Me.valNombreDefinicionProcesoField
            End Get
            Set
                Me.valNombreDefinicionProcesoField = Value
                Me.RaisePropertyChanged("valNombreDefinicionProceso")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=5)>
        Public Property idInstanciaActividad() As String
            Get
                Return Me.idInstanciaActividadField
            End Get
            Set
                Me.idInstanciaActividadField = Value
                Me.RaisePropertyChanged("idInstanciaActividad")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=6)>
        Public Property valNombreDefinicionActividad() As String
            Get
                Return Me.valNombreDefinicionActividadField
            End Get
            Set
                Me.valNombreDefinicionActividadField = Value
                Me.RaisePropertyChanged("valNombreDefinicionActividad")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=7)>
        Public Property idUsuarioAplicacion() As String
            Get
                Return Me.idUsuarioAplicacionField
            End Get
            Set
                Me.idUsuarioAplicacionField = Value
                Me.RaisePropertyChanged("idUsuarioAplicacion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=8)>
        Public Property valClaveUsuarioAplicacion() As String
            Get
                Return Me.valClaveUsuarioAplicacionField
            End Get
            Set
                Me.valClaveUsuarioAplicacionField = Value
                Me.RaisePropertyChanged("valClaveUsuarioAplicacion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=9)>
        Public Property idUsuario() As String
            Get
                Return Me.idUsuarioField
            End Get
            Set
                Me.idUsuarioField = Value
                Me.RaisePropertyChanged("idUsuario")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=10)>
        Public Property idEmisor() As String
            Get
                Return Me.idEmisorField
            End Get
            Set
                Me.idEmisorField = Value
                Me.RaisePropertyChanged("idEmisor")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="integer", IsNullable:=True, Order:=11)>
        Public Property valTamPagina() As String
            Get
                Return Me.valTamPaginaField
            End Get
            Set
                Me.valTamPaginaField = Value
                Me.RaisePropertyChanged("valTamPagina")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="integer", IsNullable:=True, Order:=12)>
        Public Property valNumPagina() As String
            Get
                Return Me.valNumPaginaField
            End Get
            Set
                Me.valNumPaginaField = Value
                Me.RaisePropertyChanged("valNumPagina")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("criteriosOrdenamiento", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=13)>
        Public Property criteriosOrdenamiento() As CriterioOrdenamientoTipo()
            Get
                Return Me.criteriosOrdenamientoField
            End Get
            Set
                Me.criteriosOrdenamientoField = Value
                Me.RaisePropertyChanged("criteriosOrdenamiento")
            End Set
        End Property

        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/CriterioOrdenamientoTipo")>
    Partial Public Class CriterioOrdenamientoTipo
        Inherits Object

        Private valOrdenField As String

        Private valNombreCampoField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="integer", IsNullable:=True, Order:=0)>
        Public Property valOrden() As String
            Get
                Return Me.valOrdenField
            End Get
            Set
                Me.valOrdenField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
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
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/schema/GestionDocumental/DocumentoTipo/v1")>
    Partial Public Class DocumentoTipo
        Inherits Object

        Private idDocumentoField As String

        Private valNombreDocumentoField As String

        Private docDocumentoField As ArchivoTipo

        Private fecDocumentoField As String

        Private valPaginasField As String

        Private codTipoDocumentoField As String

        Private valAutorOriginadorField As String

        Private idRadicadoCorrespondenciaField As String

        Private fecRadicacionCorrespondenciaField As String

        Private valNaturalezaDocumentoField As String

        Private valOrigenDocumentoField As String

        Private descObservacionLegibilidadField As String

        Private esMoverField As String

        Private valNombreTipoDocumentalField As String

        Private numFoliosField As String

        Private valLegibleField As String

        Private valTipoFirmaField As String

        Private metadataDocumentoField As MetadataDocumentoTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property idDocumento() As String
            Get
                Return Me.idDocumentoField
            End Get
            Set
                Me.idDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valNombreDocumento() As String
            Get
                Return Me.valNombreDocumentoField
            End Get
            Set
                Me.valNombreDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property docDocumento() As ArchivoTipo
            Get
                Return Me.docDocumentoField
            End Get
            Set
                Me.docDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property fecDocumento() As String
            Get
                Return Me.fecDocumentoField
            End Get
            Set
                Me.fecDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="integer", IsNullable:=True, Order:=4)>
        Public Property valPaginas() As String
            Get
                Return Me.valPaginasField
            End Get
            Set
                Me.valPaginasField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=5)>
        Public Property codTipoDocumento() As String
            Get
                Return Me.codTipoDocumentoField
            End Get
            Set
                Me.codTipoDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=6)>
        Public Property valAutorOriginador() As String
            Get
                Return Me.valAutorOriginadorField
            End Get
            Set
                Me.valAutorOriginadorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=7)>
        Public Property idRadicadoCorrespondencia() As String
            Get
                Return Me.idRadicadoCorrespondenciaField
            End Get
            Set
                Me.idRadicadoCorrespondenciaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=8)>
        Public Property fecRadicacionCorrespondencia() As String
            Get
                Return Me.fecRadicacionCorrespondenciaField
            End Get
            Set
                Me.fecRadicacionCorrespondenciaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=9)>
        Public Property valNaturalezaDocumento() As String
            Get
                Return Me.valNaturalezaDocumentoField
            End Get
            Set
                Me.valNaturalezaDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=10)>
        Public Property valOrigenDocumento() As String
            Get
                Return Me.valOrigenDocumentoField
            End Get
            Set
                Me.valOrigenDocumentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=11)>
        Public Property descObservacionLegibilidad() As String
            Get
                Return Me.descObservacionLegibilidadField
            End Get
            Set
                Me.descObservacionLegibilidadField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=12)>
        Public Property esMover() As String
            Get
                Return Me.esMoverField
            End Get
            Set
                Me.esMoverField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=13)>
        Public Property valNombreTipoDocumental() As String
            Get
                Return Me.valNombreTipoDocumentalField
            End Get
            Set
                Me.valNombreTipoDocumentalField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="integer", IsNullable:=True, Order:=14)>
        Public Property numFolios() As String
            Get
                Return Me.numFoliosField
            End Get
            Set
                Me.numFoliosField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=15)>
        Public Property valLegible() As String
            Get
                Return Me.valLegibleField
            End Get
            Set
                Me.valLegibleField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=16)>
        Public Property valTipoFirma() As String
            Get
                Return Me.valTipoFirmaField
            End Get
            Set
                Me.valTipoFirmaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=17)>
        Public Property metadataDocumento() As MetadataDocumentoTipo
            Get
                Return Me.metadataDocumentoField
            End Get
            Set
                Me.metadataDocumentoField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/schema/GestionDocumental/MetadataDocumentoTipo/v1")>
    Partial Public Class MetadataDocumentoTipo
        Inherits Object

        Private serieDocumentalField As SerieDocumentalTipo

        Private valAgrupadorField() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property serieDocumental() As SerieDocumentalTipo
            Get
                Return Me.serieDocumentalField
            End Get
            Set
                Me.serieDocumentalField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("valAgrupador", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valAgrupador() As String()
            Get
                Return Me.valAgrupadorField
            End Get
            Set
                Me.valAgrupadorField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/schema/GestionDocumental/ArchivoTipo/v1")>
    Partial Public Class ArchivoTipo
        Inherits Object

        Private valNombreArchivoField As String

        Private idArchivoField As String

        Private valContenidoArchivoField() As Byte

        Private valContenidoFirmaField() As Byte

        Private codTipoMIMEArchivoField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property valNombreArchivo() As String
            Get
                Return Me.valNombreArchivoField
            End Get
            Set
                Me.valNombreArchivoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property idArchivo() As String
            Get
                Return Me.idArchivoField
            End Get
            Set
                Me.idArchivoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="base64Binary", IsNullable:=True, Order:=2)>
        Public Property valContenidoArchivo() As Byte()
            Get
                Return Me.valContenidoArchivoField
            End Get
            Set
                Me.valContenidoArchivoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="base64Binary", IsNullable:=True, Order:=3)>
        Public Property valContenidoFirma() As Byte()
            Get
                Return Me.valContenidoFirmaField
            End Get
            Set
                Me.valContenidoFirmaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=4)>
        Public Property codTipoMIMEArchivo() As String
            Get
                Return Me.codTipoMIMEArchivoField
            End Get
            Set
                Me.codTipoMIMEArchivoField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/schema/GestionDocumental/CorrespondenciaTipo/v1")>
    Partial Public Class CorrespondenciaTipo
        Inherits Object

        Private codEntidadOriginadoraField As String

        Private codEntidadOriginadoraInicialField As String

        Private valNombreEntidadOriginadoraField As String

        Private codMedioRecepcionField As String

        Private codMedioEnvioField As String

        Private valNombreEntidadCorrespondenciaField As String

        Private codPaisField As String

        Private codTipoClienteField As String

        Private codPuntoImpresionField As String

        Private codSedeField As String

        Private codTipoEnvioField As String

        Private valDescripcionAnexosField As String

        Private codDependenciaField As String

        Private codPuntoRecepcionField As String

        Private codTipoCorrespondenciaField As String

        Private valNombreProcesoField As String

        Private valIndRequiereRespuestaNotificacionField As String

        Private personaNaturalField As PersonaNaturalTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property codEntidadOriginadora() As String
            Get
                Return Me.codEntidadOriginadoraField
            End Get
            Set
                Me.codEntidadOriginadoraField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property codEntidadOriginadoraInicial() As String
            Get
                Return Me.codEntidadOriginadoraInicialField
            End Get
            Set
                Me.codEntidadOriginadoraInicialField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property valNombreEntidadOriginadora() As String
            Get
                Return Me.valNombreEntidadOriginadoraField
            End Get
            Set
                Me.valNombreEntidadOriginadoraField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property codMedioRecepcion() As String
            Get
                Return Me.codMedioRecepcionField
            End Get
            Set
                Me.codMedioRecepcionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=4)>
        Public Property codMedioEnvio() As String
            Get
                Return Me.codMedioEnvioField
            End Get
            Set
                Me.codMedioEnvioField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=5)>
        Public Property valNombreEntidadCorrespondencia() As String
            Get
                Return Me.valNombreEntidadCorrespondenciaField
            End Get
            Set
                Me.valNombreEntidadCorrespondenciaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=6)>
        Public Property codPais() As String
            Get
                Return Me.codPaisField
            End Get
            Set
                Me.codPaisField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=7)>
        Public Property codTipoCliente() As String
            Get
                Return Me.codTipoClienteField
            End Get
            Set
                Me.codTipoClienteField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=8)>
        Public Property codPuntoImpresion() As String
            Get
                Return Me.codPuntoImpresionField
            End Get
            Set
                Me.codPuntoImpresionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=9)>
        Public Property codSede() As String
            Get
                Return Me.codSedeField
            End Get
            Set
                Me.codSedeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=10)>
        Public Property codTipoEnvio() As String
            Get
                Return Me.codTipoEnvioField
            End Get
            Set
                Me.codTipoEnvioField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=11)>
        Public Property valDescripcionAnexos() As String
            Get
                Return Me.valDescripcionAnexosField
            End Get
            Set
                Me.valDescripcionAnexosField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=12)>
        Public Property codDependencia() As String
            Get
                Return Me.codDependenciaField
            End Get
            Set
                Me.codDependenciaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=13)>
        Public Property codPuntoRecepcion() As String
            Get
                Return Me.codPuntoRecepcionField
            End Get
            Set
                Me.codPuntoRecepcionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=14)>
        Public Property codTipoCorrespondencia() As String
            Get
                Return Me.codTipoCorrespondenciaField
            End Get
            Set
                Me.codTipoCorrespondenciaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=15)>
        Public Property valNombreProceso() As String
            Get
                Return Me.valNombreProcesoField
            End Get
            Set
                Me.valNombreProcesoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=16)>
        Public Property valIndRequiereRespuestaNotificacion() As String
            Get
                Return Me.valIndRequiereRespuestaNotificacionField
            End Get
            Set
                Me.valIndRequiereRespuestaNotificacionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=17)>
        Public Property personaNatural() As PersonaNaturalTipo
            Get
                Return Me.personaNaturalField
            End Get
            Set
                Me.personaNaturalField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/IdentificacionTipo/v1")>
    Partial Public Class IdentificacionTipo
        Inherits Object

        Private codTipoIdentificacionField As String

        Private valNumeroIdentificacionField As String

        Private municipioExpedicionField As MunicipioTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property codTipoIdentificacion() As String
            Get
                Return Me.codTipoIdentificacionField
            End Get
            Set
                Me.codTipoIdentificacionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valNumeroIdentificacion() As String
            Get
                Return Me.valNumeroIdentificacionField
            End Get
            Set
                Me.valNumeroIdentificacionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property municipioExpedicion() As MunicipioTipo
            Get
                Return Me.municipioExpedicionField
            End Get
            Set
                Me.municipioExpedicionField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/PersonaNaturalTipo/v1")>
    Partial Public Class PersonaNaturalTipo
        Inherits Object

        Private idPersonaField As IdentificacionTipo

        Private valNombreCompletoField As String

        Private valPrimerNombreField As String

        Private valSegundoNombreField As String

        Private valPrimerApellidoField As String

        Private valSegundoApellidoField As String

        Private codSexoField As String

        Private descSexoField As String

        Private codEstadoCivilField As String

        Private descEstadoCivilField As String

        Private codNivelEducativoField As String

        Private descNivelEducativoField As String

        Private codTipoPersonaNaturalField As String

        Private descTipoPersonaNaturalField As String

        Private contactoField As ContactoPersonaTipo

        Private idAutorizadoField As IdentificacionTipo

        Private idAbogadoField As IdentificacionTipo

        Private codFuenteField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property idPersona() As IdentificacionTipo
            Get
                Return Me.idPersonaField
            End Get
            Set
                Me.idPersonaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valNombreCompleto() As String
            Get
                Return Me.valNombreCompletoField
            End Get
            Set
                Me.valNombreCompletoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property valPrimerNombre() As String
            Get
                Return Me.valPrimerNombreField
            End Get
            Set
                Me.valPrimerNombreField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property valSegundoNombre() As String
            Get
                Return Me.valSegundoNombreField
            End Get
            Set
                Me.valSegundoNombreField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=4)>
        Public Property valPrimerApellido() As String
            Get
                Return Me.valPrimerApellidoField
            End Get
            Set
                Me.valPrimerApellidoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=5)>
        Public Property valSegundoApellido() As String
            Get
                Return Me.valSegundoApellidoField
            End Get
            Set
                Me.valSegundoApellidoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=6)>
        Public Property codSexo() As String
            Get
                Return Me.codSexoField
            End Get
            Set
                Me.codSexoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=7)>
        Public Property descSexo() As String
            Get
                Return Me.descSexoField
            End Get
            Set
                Me.descSexoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=8)>
        Public Property codEstadoCivil() As String
            Get
                Return Me.codEstadoCivilField
            End Get
            Set
                Me.codEstadoCivilField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=9)>
        Public Property descEstadoCivil() As String
            Get
                Return Me.descEstadoCivilField
            End Get
            Set
                Me.descEstadoCivilField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=10)>
        Public Property codNivelEducativo() As String
            Get
                Return Me.codNivelEducativoField
            End Get
            Set
                Me.codNivelEducativoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=11)>
        Public Property descNivelEducativo() As String
            Get
                Return Me.descNivelEducativoField
            End Get
            Set
                Me.descNivelEducativoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=12)>
        Public Property codTipoPersonaNatural() As String
            Get
                Return Me.codTipoPersonaNaturalField
            End Get
            Set
                Me.codTipoPersonaNaturalField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=13)>
        Public Property descTipoPersonaNatural() As String
            Get
                Return Me.descTipoPersonaNaturalField
            End Get
            Set
                Me.descTipoPersonaNaturalField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=14)>
        Public Property contacto() As ContactoPersonaTipo
            Get
                Return Me.contactoField
            End Get
            Set
                Me.contactoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=15)>
        Public Property idAutorizado() As IdentificacionTipo
            Get
                Return Me.idAutorizadoField
            End Get
            Set
                Me.idAutorizadoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=16)>
        Public Property idAbogado() As IdentificacionTipo
            Get
                Return Me.idAbogadoField
            End Get
            Set
                Me.idAbogadoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=17)>
        Public Property codFuente() As String
            Get
                Return Me.codFuenteField
            End Get
            Set
                Me.codFuenteField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/ContactoPersonaTipo/v1")>
    Partial Public Class ContactoPersonaTipo
        Inherits Object

        Private telefonosField() As TelefonoTipo

        Private correosElectronicosField() As CorreoElectronicoTipo

        Private esAutorizaNotificacionElectronicaField As String

        Private ubicacionesField() As UbicacionPersonaTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("telefonos", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property telefonos() As TelefonoTipo()
            Get
                Return Me.telefonosField
            End Get
            Set
                Me.telefonosField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("correosElectronicos", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property correosElectronicos() As CorreoElectronicoTipo()
            Get
                Return Me.correosElectronicosField
            End Get
            Set
                Me.correosElectronicosField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property esAutorizaNotificacionElectronica() As String
            Get
                Return Me.esAutorizaNotificacionElectronicaField
            End Get
            Set
                Me.esAutorizaNotificacionElectronicaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ubicaciones", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property ubicaciones() As UbicacionPersonaTipo()
            Get
                Return Me.ubicacionesField
            End Get
            Set
                Me.ubicacionesField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/schema/Transversales/CorreoElectronicoTipo/v1")>
    Partial Public Class CorreoElectronicoTipo
        Inherits Object

        Private valCorreoElectronicoField As String

        Private codFuenteField As String

        Private descFuenteCorreoField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property valCorreoElectronico() As String
            Get
                Return Me.valCorreoElectronicoField
            End Get
            Set
                Me.valCorreoElectronicoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property codFuente() As String
            Get
                Return Me.codFuenteField
            End Get
            Set
                Me.codFuenteField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property descFuenteCorreo() As String
            Get
                Return Me.descFuenteCorreoField
            End Get
            Set
                Me.descFuenteCorreoField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/UbicacionPersonaTipo/v1")>
    Partial Public Class UbicacionPersonaTipo
        Inherits Object

        Private municipioField As MunicipioTipo

        Private valDireccionField As String

        Private codTipoDireccionField As String

        Private descTipoDireccionField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property municipio() As MunicipioTipo
            Get
                Return Me.municipioField
            End Get
            Set
                Me.municipioField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valDireccion() As String
            Get
                Return Me.valDireccionField
            End Get
            Set
                Me.valDireccionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property codTipoDireccion() As String
            Get
                Return Me.codTipoDireccionField
            End Get
            Set
                Me.codTipoDireccionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property descTipoDireccion() As String
            Get
                Return Me.descTipoDireccionField
            End Get
            Set
                Me.descTipoDireccionField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/MunicipioTipo/v1")>
    Partial Public Class MunicipioTipo
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged

        Private codMunicipioField As String

        Private valNombreField As String

        Private departamentoField As DepartamentoTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property codMunicipio() As String
            Get
                Return Me.codMunicipioField
            End Get
            Set
                Me.codMunicipioField = Value
                Me.RaisePropertyChanged("codMunicipio")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valNombre() As String
            Get
                Return Me.valNombreField
            End Get
            Set
                Me.valNombreField = Value
                Me.RaisePropertyChanged("valNombre")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property departamento() As DepartamentoTipo
            Get
                Return Me.departamentoField
            End Get
            Set
                Me.departamentoField = Value
                Me.RaisePropertyChanged("departamento")
            End Set
        End Property

        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/DepartamentoTipo/v1")>
    Partial Public Class DepartamentoTipo
        Inherits Object

        Private codDepartamentoField As String

        Private valNombreField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property codDepartamento() As String
            Get
                Return Me.codDepartamentoField
            End Get
            Set
                Me.codDepartamentoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valNombre() As String
            Get
                Return Me.valNombreField
            End Get
            Set
                Me.valNombreField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/TelefonoTipo/v1")>
    Partial Public Class TelefonoTipo
        Inherits Object

        Private codTipoTelefonoField As String

        Private valNumeroTelefonoField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property codTipoTelefono() As String
            Get
                Return Me.codTipoTelefonoField
            End Get
            Set
                Me.codTipoTelefonoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property valNumeroTelefono() As String
            Get
                Return Me.valNumeroTelefonoField
            End Get
            Set
                Me.valNumeroTelefonoField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/schema/GestionDocumental/ExpedienteTipo/v1")>
    Partial Public Class ExpedienteTipo
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged

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

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property idNumExpediente() As String
            Get
                Return Me.idNumExpedienteField
            End Get
            Set
                Me.idNumExpedienteField = Value
                Me.RaisePropertyChanged("idNumExpediente")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property codSeccion() As String
            Get
                Return Me.codSeccionField
            End Get
            Set
                Me.codSeccionField = Value
                Me.RaisePropertyChanged("codSeccion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property valSeccion() As String
            Get
                Return Me.valSeccionField
            End Get
            Set
                Me.valSeccionField = Value
                Me.RaisePropertyChanged("valSeccion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property codSubSeccion() As String
            Get
                Return Me.codSubSeccionField
            End Get
            Set
                Me.codSubSeccionField = Value
                Me.RaisePropertyChanged("codSubSeccion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=4)>
        Public Property valSubSeccion() As String
            Get
                Return Me.valSubSeccionField
            End Get
            Set
                Me.valSubSeccionField = Value
                Me.RaisePropertyChanged("valSubSeccion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=5)>
        Public Property serieDocumental() As SerieDocumentalTipo
            Get
                Return Me.serieDocumentalField
            End Get
            Set
                Me.serieDocumentalField = Value
                Me.RaisePropertyChanged("serieDocumental")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=6)>
        Public Property valAnoApertura() As String
            Get
                Return Me.valAnoAperturaField
            End Get
            Set
                Me.valAnoAperturaField = Value
                Me.RaisePropertyChanged("valAnoApertura")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=7)>
        Public Property valFondo() As String
            Get
                Return Me.valFondoField
            End Get
            Set
                Me.valFondoField = Value
                Me.RaisePropertyChanged("valFondo")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=8)>
        Public Property valEntidadPredecesora() As String
            Get
                Return Me.valEntidadPredecesoraField
            End Get
            Set
                Me.valEntidadPredecesoraField = Value
                Me.RaisePropertyChanged("valEntidadPredecesora")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=9)>
        Public Property identificacion() As IdentificacionTipo
            Get
                Return Me.identificacionField
            End Get
            Set
                Me.identificacionField = Value
                Me.RaisePropertyChanged("identificacion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=10)>
        Public Property descDescripcion() As String
            Get
                Return Me.descDescripcionField
            End Get
            Set
                Me.descDescripcionField = Value
                Me.RaisePropertyChanged("descDescripcion")
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=11)>
        Public Property idCarpetaFileNet() As String
            Get
                Return Me.idCarpetaFileNetField
            End Get
            Set
                Me.idCarpetaFileNetField = Value
                Me.RaisePropertyChanged("idCarpetaFileNet")
            End Set
        End Property

        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/GestionDocumental/SrvIntDocumento/v1")>
    Partial Public Class OpIngresarDocumentoRespTipo
        Inherits Object

        Private contextoRespuestaField As ContextoRespuestaTipo

        Private documentosField() As DocumentoTipo

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>
        Public Property contextoRespuesta() As ContextoRespuestaTipo
            Get
                Return Me.contextoRespuestaField
            End Get
            Set
                Me.contextoRespuestaField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("documentos", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>
        Public Property documentos() As DocumentoTipo()
            Get
                Return Me.documentosField
            End Get
            Set
                Me.documentosField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/schema/GestionDocumental/SerieDocumentalTipo/v1")>
    Partial Public Class SerieDocumentalTipo
        Inherits Object

        Private valNombreSerieField As String

        Private codSerieField As String

        Private valNombreSubserieField As String

        Private codSubserieField As String

        Private codTipoDocumentalField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=0)>
        Public Property valNombreSerie() As String
            Get
                Return Me.valNombreSerieField
            End Get
            Set
                Me.valNombreSerieField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=1)>
        Public Property codSerie() As String
            Get
                Return Me.codSerieField
            End Get
            Set
                Me.codSerieField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=2)>
        Public Property valNombreSubserie() As String
            Get
                Return Me.valNombreSubserieField
            End Get
            Set
                Me.valNombreSubserieField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property codSubserie() As String
            Get
                Return Me.codSubserieField
            End Get
            Set
                Me.codSubserieField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=4)>
        Public Property codTipoDocumental() As String
            Get
                Return Me.codTipoDocumentalField
            End Get
            Set
                Me.codTipoDocumentalField = Value
            End Set
        End Property

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ugpp.gov.co/esb/schema/ContextoRespuestaTipo/v1")>
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
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>
        Public Property idTx() As String
            Get
                Return Me.idTxField
            End Get
            Set
                Me.idTxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>
        Public Property codEstadoTx() As String
            Get
                Return Me.codEstadoTxField
            End Get
            Set
                Me.codEstadoTxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=2)>
        Public Property fechaTx() As Date
            Get
                Return Me.fechaTxField
            End Get
            Set
                Me.fechaTxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=3)>
        Public Property idInstanciaProceso() As String
            Get
                Return Me.idInstanciaProcesoField
            End Get
            Set
                Me.idInstanciaProcesoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=True, Order:=4)>
        Public Property idInstanciaActividad() As String
            Get
                Return Me.idInstanciaActividadField
            End Get
            Set
                Me.idInstanciaActividadField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="integer", IsNullable:=True, Order:=5)>
        Public Property valCantidadPaginas() As String
            Get
                Return Me.valCantidadPaginasField
            End Get
            Set
                Me.valCantidadPaginasField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="integer", IsNullable:=True, Order:=6)>
        Public Property valNumPagina() As String
            Get
                Return Me.valNumPaginaField
            End Get
            Set
                Me.valNumPaginaField = Value
            End Set
        End Property

    End Class

End Namespace
