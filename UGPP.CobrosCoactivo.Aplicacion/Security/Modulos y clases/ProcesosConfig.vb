Imports System.Data.SqlClient
Namespace procesos_tributario
    Public MustInherit Class ProcesosConfig
        Protected Friend MyConexion As SqlConnection
        Protected Friend MyConexionME As SqlConnection

        Private varExpedinete As String
        Private varRefeCatastral As String
        Private varRepo As String
        Private _Ente As String

        Protected Friend ESCommand As SqlCommand
        Protected Friend ESAdap As SqlDataReader

        REM estructura de datos
        Private _Datos_Ente As DataTable
        Private _Datos_Informe As DataTable
        Private _impuesto As String
        Private _EnteDeudorPropietario As String
        Private _Impuesto_Literal As String
        Private _Datos_Conceptos_Deuda As DataTable
        Private _Datos_Conceptos_Deuda_Indus As DataTable
        Private _Datos_Liquidacion_credito As DataTable
        Private _Load_Acto_Previo As Boolean = False
        Private _Datos_Acto_Previo As DataTable
        Private _mnivelacces As String
        Private _sscodigousuario As String

        Public Property sscodigousuario() As String
            Get
                Return _sscodigousuario
            End Get
            Set(ByVal value As String)
                _sscodigousuario = value
            End Set
        End Property

        Public Property mnivelacces() As String
            Get
                Return _mnivelacces
            End Get
            Set(ByVal value As String)
                _mnivelacces = value
            End Set
        End Property

        Public Property Load_Acto_Previo() As Boolean
            Get
                Return _Load_Acto_Previo
            End Get
            Set(ByVal value As Boolean)
                _Load_Acto_Previo = value
            End Set
        End Property

        Public Property EnteDeudorPropietario() As String
            Get
                Return _EnteDeudorPropietario
            End Get
            Set(ByVal value As String)
                _EnteDeudorPropietario = value
            End Set
        End Property

        Public Property Impuesto_Literal() As String
            Get
                Return _Impuesto_Literal
            End Get
            Set(ByVal value As String)
                _Impuesto_Literal = value
            End Set
        End Property

        Public Property Impuesto() As String
            Get
                Return _impuesto
            End Get
            Set(ByVal value As String)
                _impuesto = value
            End Set
        End Property

        Public Property Datos_Ente() As DataTable
            Get
                Return _Datos_Ente
            End Get
            Set(ByVal value As DataTable)
                _Datos_Ente = value
            End Set
        End Property

        Public Property Datos_Informe() As DataTable
            Get
                Return _Datos_Informe
            End Get
            Set(ByVal value As DataTable)
                _Datos_Informe = value
            End Set
        End Property

        Public Property Datos_Liquidacion_credito() As DataTable
            Get
                Return _Datos_Liquidacion_credito
            End Get
            Set(ByVal value As DataTable)
                _Datos_Liquidacion_credito = value
            End Set
        End Property

        Public Property Datos_Conceptos_Deuda() As DataTable
            Get
                Return _Datos_Conceptos_Deuda
            End Get
            Set(ByVal value As DataTable)
                _Datos_Conceptos_Deuda = value
            End Set
        End Property

        Public Property Datos_Conceptos_Deuda_Indus() As DataTable
            Get
                Return _Datos_Conceptos_Deuda_Indus
            End Get
            Set(ByVal value As DataTable)
                _Datos_Conceptos_Deuda_Indus = value
            End Set
        End Property

        Public Property Datos_Acto_Previo() As DataTable
            Get
                Return _Datos_Acto_Previo
            End Get
            Set(ByVal value As DataTable)
                _Datos_Acto_Previo = value
            End Set
        End Property

        Public Property Ente() As String
            Get
                Return _Ente
            End Get
            Set(ByVal value As String)
                _Ente = value
            End Set
        End Property

        Public Property RefeCatastral() As String
            Get
                Return varRefeCatastral
            End Get
            Set(ByVal value As String)
                varRefeCatastral = value
            End Set
        End Property

        Public Property Expedinete() As String
            Get
                Return varExpedinete
            End Get
            Set(ByVal value As String)
                varExpedinete = value
            End Set
        End Property

        Public Property Repo() As String
            Get
                Return varRepo
            End Get
            Set(ByVal value As String)
                varRepo = value
            End Set
        End Property

        Public Property Conexion() As SqlConnection
            Get
                Return MyConexion
            End Get
            Set(ByVal value As SqlConnection)
                MyConexion = value
            End Set
        End Property

        Public Property ConexionME() As SqlConnection
            Get
                Return MyConexionME
            End Get
            Set(ByVal value As SqlConnection)
                MyConexionME = value
            End Set
        End Property

        Public Sub New(ByVal Conexion As SqlConnection)
            MyConexion = Conexion
        End Sub

        Public Sub New()

        End Sub

        Protected Sub NewClass()
            With Me
                .ESCommand = New SqlCommand
                .ESCommand.Connection = .MyConexionME
                .MyConexionME.Open()
            End With
        End Sub

        Protected Sub Load_Configuracion()
            NewClass()
            ESCommand.CommandText = "SELECT CODIGO AS ID_CLIENTE, NOMBRE AS NOMBRE, ENT_FOTO AS FOTO,ent_pref_exp,ent_pref_res,ent_tesorero,ent_firma,ent_foto2,ent_foto3,ent_funcionario_ejec,ent_cargo_func_ejec FROM ENTESCOBRADORES WHERE CODIGO = @COBRADOR"
            ESCommand.Parameters.Add("@COBRADOR", SqlDbType.VarChar).Value = Ente
            ESAdap = ESCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Datos_Ente.Load(ESAdap)
            ESAdap.Close() : MyConexionME.Close()
        End Sub

        Public Function addnewRows(ByVal xrow As DataRow, ByVal xDatos As DataTable) As DataRow
            Dim row As Reportes_Admistratiivos.Mandaniento_PagoRow
            row = xDatos.NewRow

            row("MAN_DEUSDOR") = xrow("MAN_DEUSDOR")
            row("MAN_IMPUESTO") = xrow("MAN_IMPUESTO")
            row("MAN_VALORMANDA") = xrow("MAN_VALORMANDA")
            row("MAN_NOMDEUDOR") = xrow("MAN_NOMDEUDOR")
            row("MAN_DIRECCION") = Nothing
            row("MAN_DIR_ESTABL") = xrow("MAN_DIR_ESTABL")
            row("MAN_REFCATRASTAL") = xrow("MAN_REFCATRASTAL")
            row("MAN_VIGENCIA") = Nothing
            row("MAN_CONCEPTOCDG") = Nothing
            row("MAN_ESTRATOCD") = Nothing
            row("MAN_DESTINOCD2") = Nothing
            row("MAN_BASEGRAVABLE") = Nothing
            row("MAN_TARIFA") = DBNull.Value
            row("MAN_CAPITAL") = DBNull.Value
            row("MAN_INTERESES") = xrow("MAN_INTERESES")
            row("MAN_TOTAL") = xrow("MAN_TOTAL")
            row("MAN_EXPEDIENTE") = xrow("MAN_EXPEDIENTE")
            row("MAN_FECHADOC") = DBNull.Value
            row("MAN_EFIPERDES") = xrow("MAN_EFIPERDES")
            row("MAN_EFIPERHAS") = xrow("MAN_EFIPERHAS")
            row("MAN_PAGOS") = xrow("MAN_PAGOS")
            row("MAN_FECHARAC") = DBNull.Value
            row("MAN_MATINMOB") = DBNull.Value

            Return row
        End Function

        Public Function QueReporte(ByVal cadenas As String) As CrystalDecisions.CrystalReports.Engine.ReportClass
            If Impuesto Is Nothing Then
                Throw New Exception("No se detecto el impuesto (Acto Administrativo)")
            End If

            If Impuesto = 1 Then ' Predial
                Dim controReport As New control_informe_x_impuesto.cobro_predial
                controReport.acto_paso_administrativo = cadenas
                Return controReport.QueReporte()
            ElseIf Impuesto = 2 Then ' Industria y comercio
                Dim controReport As New control_informe_x_impuesto.cobro_industriaComercio
                controReport.acto_paso_administrativo = cadenas
                Return controReport.QueReporte()
            ElseIf Impuesto = 3 Then ' Vehiculo 
                Dim controReport As New control_informe_x_impuesto.cobro_vehiculo
                controReport.acto_paso_administrativo = cadenas
                Return controReport.QueReporte()
            ElseIf Impuesto = 4 Then
                Dim controReport As New control_informe_x_impuesto.cobro_pensiones
                controReport.acto_paso_administrativo = cadenas
                Return controReport.QueReporte()
            Else
                Throw New Exception("No se detecto el impuesto  para la ejecucion del informe. (Acto Administrativo)")
            End If
        End Function

        Public MustOverride Function Prima_EjecucionFiscal(ByVal Impuesto As String, ByVal TipoRep As Integer) As Boolean
        Public MustOverride Function Prima_EjecucionFiscal_Masivos(ByVal Valor As Boolean, ByVal ValValor As Double, ByVal Impuesto As String, ByVal TipoRep As Integer) As Boolean
    End Class
End Namespace


