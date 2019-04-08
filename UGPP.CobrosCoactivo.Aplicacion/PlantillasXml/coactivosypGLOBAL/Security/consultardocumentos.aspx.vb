Public Class consultardocumentos
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
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Protected WithEvents Validator As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents contenidogrids As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtFechaRad As System.Web.UI.HtmlControls.HtmlInputText

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

        If Not Me.Page.IsPostBack Then
            Dim Adap As SqlClient.SqlDataAdapter
            Dim Table As DataTable = New DataTable
            Adap = New SqlClient.SqlDataAdapter("SELECT * FROM ETAPAS", Me.SqlConnection1)
            'If Adap.Fill(Table) > 0 Then
            '    Me.CmbEtapa.DataSource = Table
            '    Me.CmbEtapa.DataTextField = "NOMBRE"
            '    Me.CmbEtapa.DataValueField = "CODIGO"
            '    Me.CmbEtapa.DataBind()
            'End If
        End If

    End Sub



    Private Sub BeginSearch(ByVal Page As Integer, ByVal Datos As DataSet)
        If Datos.Tables("documentos").Rows.Count = 0 Then
            'Mostrar mensaje de no hay registros 
            Me.Validator.ErrorMessage = "No hay datos que coincidan con esta búsqueda"
            Me.Validator.IsValid = False
        Else
            Dim X As Integer = 0
            Dim totalpags As Integer = 0
            Dim rutaimg, ruta, nomarchivo, entidad, NomEntidad, Actuacion As String
            With Me
                'For X = (Page * .DataGrid1.PageSize) To Datos.Tables("documentos").Rows.Count - 1
                '    If Not Datos.Tables("documentos").Rows(X).IsNull("RUTAIMAGEN") Then
                '        If (X > 0 AndAlso X <= (.DataGrid1.PageSize * Page) + (.DataGrid1.PageSize - 1)) OrElse (X = 0 AndAlso X < .DataGrid1.PageSize) Then
                '            ruta = Trim(Datos.Tables("documentos").Rows(X).Item("ruta"))
                '            nomarchivo = Trim(Datos.Tables("documentos").Rows(X).Item("nomarchivo"))
                '            entidad = Trim(Datos.Tables("documentos").Rows(X).Item("entidad"))
                '            NomEntidad = Trim(Datos.Tables("documentos").Rows(X).Item("nomente"))
                '            Actuacion = Trim(Datos.Tables("documentos").Rows(X).Item("nombre"))
                '            totalpags = Trim(Datos.Tables("documentos").Rows(X).Item("paginas"))

                '            'rutaimg = "C:\Inetpub\wwwroot\coactivosyp\expedientes\" & ruta & "\" & nomarchivo
                '            'rutaimg = "expedientes/" & ruta & "/" & nomarchivo


                '            Datos.Tables("documentos").Rows(X).Item("rutaimagen") = nomarchivo
                '            Datos.Tables("documentos").Rows(X).Item("rutavisor") = "/coactivosyp/visorimagen.aspx" & "?nomente=" & NomEntidad & "&F=" & nomarchivo & "&totimg=" & totalpags & "&acto=" & Actuacion & "&folder=" & ruta
                '        Else
                '            Exit For
                '        End If
                '    Else
                '        Exit For
                '    End If
                'Next
                '.DataGrid1.CurrentPageIndex = Page
                '.DataGrid1.DataSource = Datos.Tables("documentos")
            End With
        End If
        'Me.DataGrid1.DataSource = DataSet1.Tables("documentos")        
        Me.Session("Datos") = Datos
        'Me.DataGrid1.DataBind()
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Dim Dataset1 As DataSet = CType(Me.Session("Datos"), DataSet)
        Me.BeginSearch(e.NewPageIndex, Dataset1)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim TipoConexion, dia, mes, anio, fechaserver, cmd, NombreEnte, NombreActuacion As String
        Dim fecharadic As Date
        Dim X As Integer
        TipoConexion = ConfigurationManager.AppSettings("tipoconexion") 'local o web

        If TipoConexion = "web" Then
            dia = Left(Me.txtFechaRad.Value.Trim(), 2)
            mes = Mid(Me.txtFechaRad.Value.Trim(), 4, 2)
            anio = Mid(Me.txtFechaRad.Value.Trim(), 7, 4)
            fechaserver = mes & "/" & dia & "/" & anio
            fecharadic = CDate(fechaserver)
        Else
            fecharadic = CDate(Me.txtFechaRad.Value)
        End If

        Me.Validator.Text = fecharadic
        Me.Validator.IsValid = False

        'Consultar los documentos q tengan fecha de radicacion igual a la consultada
        'cmd = "SELECT * FROM documentos WHERE fecharadic BETWEEN '" & fecharadic & "' AND '" & fecharadic & " 11:59:59.998 PM' ORDER BY entidad, idacto, nomarchivo"
        cmd = "SELECT documentos.entidad, documentos.idacto, documentos.ruta, documentos.nomarchivo, documentos.id," & _
            "documentos.paginas, documentos.fecharadic,entesdbf.nombre AS nomente, actuaciones.nombre AS nomactuacion " & _
            "FROM documentos, entesdbf, actuaciones " & _
            "WHERE RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit) AND " & _
            "RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND " & _
            "fecharadic BETWEEN '" & fecharadic & "' AND '" & fecharadic & " 11:59:59.998 PM' ORDER BY entidad, idacto, nomarchivo"

        'Creacion de objeto SQLCommand
        Dim oscmd As SqlClient.SqlCommand
        oscmd = New SqlClient.SqlCommand(cmd, Me.SqlConnection1)
        'Creacion del objeto DataAdapter
        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, Me.SqlConnection1)
        oDtAdapterSql1.SelectCommand = oscmd
        'Creacion del DataSet
        Dim Dataset1 As DataSet = New DataSet
        oDtAdapterSql1.Fill(Dataset1, "qDocumentos")

        'Generar el HTML para mostrar los documentos q ingresaron con la fecha de radicacion
        Me.contenidogrids.InnerHtml = ""
        For X = 0 To Dataset1.Tables("qDocumentos").Rows.Count - 1
            NombreEnte = Trim(Dataset1.Tables("qDocumentos").Rows(X).Item("nomente"))
            NombreActuacion = Trim(Dataset1.Tables("qDocumentos").Rows(X).Item("nomactuacion"))

            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedorregistros'>"

            'Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<a href='visorimagen.aspx?nomente=" & NomEntidad & "&F=" & NomArchivo & "&totimg=" & totalpags & "&acto=" & NomActo & "&folder=" & ruta & "'>"

            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + NombreEnte

            'Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "</a>"

            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "</div>"

            'Dataset1.Tables("acts").Rows.Clear()
            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<br />"
        Next

    End Sub
End Class