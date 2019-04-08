Public Class listaprocesos
    Inherits System.Web.UI.Page

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        '
        'SqlConnection1
        '
        Me.SqlConnection1.ConnectionString = "workstation id=PORTATILRAFA;packet size=4096;user id=sa;data source=PORTATILRAFA;" & _
        "persist security info=True;initial catalog=pensionesweb;password=0197"

    End Sub
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim NomServidor, Usuario, Clave, BaseDatos, cmd, userDE As String
        NomServidor = ConfigurationManager.AppSettings("ServerName")
        Usuario = ConfigurationManager.AppSettings("BD_User")
        Clave = ConfigurationManager.AppSettings("BD_pass")
        BaseDatos = ConfigurationManager.AppSettings("BD_name")

        Me.SqlConnection1.ConnectionString = "workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
            ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave



        'Llenar el datagrid con los asuntos pendientes del usuario actual
        cmd = "SELECT * FROM procesos"

        'Crear el objeto DataAdapter
        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, Me.SqlConnection1)

        'Crear el dataset
        Dim Dataset1 As DataSet = New DataSet

        'Dim oscmd As SqlClient.SqlCommand
        'oscmd = New SqlClient.SqlCommand(cmd, Me.SqlConnection1)
        'oDtAdapterSql1.SelectCommand = oscmd
        oDtAdapterSql1.Fill(Dataset1, "procesos")

        Me.DataGrid1.DataSource = Dataset1.Tables("procesos")
        Me.DataGrid1.DataBind()
    End Sub
End Class