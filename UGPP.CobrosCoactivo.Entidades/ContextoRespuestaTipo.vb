
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")>
<System.SerializableAttribute()>
<System.Diagnostics.DebuggerStepThroughAttribute()>
<System.ComponentModel.DesignerCategoryAttribute("code")>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")>
<System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", IsNullable:=False)>
Partial Public Class ContextoRespuestaTipo_old
    Private idTxField As String
    Private codEstadoTxField As String
    Private fechaTxField As System.DateTime
    Private idInstanciaProcesoField As String
    Private idInstanciaActividadField As String
    Private valCantidadPaginasField As String
    Private valNumPaginaField As String

    Public Property idTx As String
        Get
            Return Me.idTxField
        End Get
        Set(ByVal value As String)
            Me.idTxField = value
        End Set
    End Property

    Public Property codEstadoTx As String
        Get
            Return Me.codEstadoTxField
        End Get
        Set(ByVal value As String)
            Me.codEstadoTxField = value
        End Set
    End Property

    Public Property fechaTx As System.DateTime
        Get
            Return Me.fechaTxField
        End Get
        Set(ByVal value As System.DateTime)
            Me.fechaTxField = value
        End Set
    End Property

    Public Property idInstanciaProceso As String
        Get
            Return Me.idInstanciaProcesoField
        End Get
        Set(ByVal value As String)
            Me.idInstanciaProcesoField = value
        End Set
    End Property

    Public Property idInstanciaActividad As String
        Get
            Return Me.idInstanciaActividadField
        End Get
        Set(ByVal value As String)
            Me.idInstanciaActividadField = value
        End Set
    End Property

    <System.Xml.Serialization.XmlElementAttribute(DataType:="integer")>
    Public Property valCantidadPaginas As String
        Get
            Return Me.valCantidadPaginasField
        End Get
        Set(ByVal value As String)
            Me.valCantidadPaginasField = value
        End Set
    End Property

    <System.Xml.Serialization.XmlElementAttribute(DataType:="integer")>
    Public Property valNumPagina As String
        Get
            Return Me.valNumPaginaField
        End Get
        Set(ByVal value As String)
            Me.valNumPaginaField = value
        End Set
    End Property
End Class

