Namespace control_informe_x_impuesto
    Public MustInherit Class controlador_de_informes
        Dim _codigo_impuesto As Integer = 0
        Dim _acto_paso_administrativo As String
        Dim _tipoReporte As String

        Public Property codigo_impuesto() As Integer
            Get
                Return _codigo_impuesto
            End Get
            Set(ByVal value As Integer)
                _codigo_impuesto = value
            End Set
        End Property

        Public Property acto_paso_administrativo() As String
            Get
                Return _acto_paso_administrativo
            End Get
            Set(ByVal value As String)
                _acto_paso_administrativo = value
            End Set
        End Property

        'En el caso de reportes masivos cuyas consultas ameritan una transacción SQL más compleja en los reportes masivos.
        Public Property TipoReporte() As String
            Get
                Return _tipoReporte
            End Get
            Set(ByVal value As String)
                _tipoReporte = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public MustOverride Function Informe_retorno()
        Public MustOverride Function QueReporte() As CrystalDecisions.CrystalReports.Engine.ReportDocument

    End Class
End Namespace

