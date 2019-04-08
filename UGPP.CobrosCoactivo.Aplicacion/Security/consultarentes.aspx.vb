Public Class consultarentes
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
    Protected WithEvents btnConsultar As System.Web.UI.WebControls.Button
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    'Protected WithEvents txtEnte As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents Validator As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents contenidogrids As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents HyperLink4 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HyperLink5 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HyperLink6 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents mpeSeleccion As Global.AjaxControlToolkit.ModalPopupExtender
    Protected WithEvents pnlSeleccionarDatos As Global.System.Web.UI.WebControls.Panel
    Protected WithEvents ListExpedientes As Global.System.Web.UI.WebControls.ListBox

    Protected WithEvents btnNo As Global.System.Web.UI.WebControls.Button
    Protected WithEvents btnSi As Global.System.Web.UI.WebControls.Button

    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager
    Protected WithEvents AutoCompleteExtender1 As Global.AjaxControlToolkit.AutoCompleteExtender
    Protected WithEvents txtEnte As Global.System.Web.UI.WebControls.TextBox
    'Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager
    'Protected WithEvents hiddenCliente As System.Web.UI.WebControls.Button

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
        Dim NomServidor, Usuario, Clave, BaseDatos As String
        NomServidor = ConfigurationManager.AppSettings("ServerName")
        Usuario = ConfigurationManager.AppSettings("BD_User")
        Clave = ConfigurationManager.AppSettings("BD_pass")
        BaseDatos = ConfigurationManager.AppSettings("BD_name")

        Me.SqlConnection1.ConnectionString = "workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
            ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave

        If Not Me.Page.IsPostBack Then
            'Dim Adap As SqlClient.SqlDataAdapter
            'Dim Table As DataTable = New DataTable
            Dim ente As String
            ente = Request.QueryString("ente")

            If ente <> Nothing Then
                txtEnte.Text = Request.QueryString("nombente") & "::" & ente
                If Request.QueryString("Expedinete") = Nothing Then
                    expediente(ente, Nothing)
                Else
                    expediente(ente, Request.QueryString("Expedinete").ToString.Trim)
                End If
            End If
        End If
    End Sub

    Private Sub expediente(ByVal idEntidad As String, ByVal expediente As String)
        Try
            Dim cmd, CodigoEtapa, NombreEtapa, TipoConexion As String
            Dim X As Integer
            TipoConexion = ConfigurationManager.AppSettings("tipoconexion") 'local o web

            'Creacion de objeto SQLCommand
            Dim oscmd As SqlClient.SqlCommand


            cmd = "SELECT codigo, nombre FROM etapas ORDER BY codigo"
            oscmd = New SqlClient.SqlCommand(cmd, Me.SqlConnection1)

            'Creacion del objeto DataAdapter
            Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
            oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, Me.SqlConnection1)
            oDtAdapterSql1.SelectCommand = oscmd

            'Creacion del DataSet
            Dim Dataset1 As DataSet = New DataSet
            oDtAdapterSql1.Fill(Dataset1, "etapas")
            Me.contenidogrids.InnerHtml = ""
            For X = 0 To Dataset1.Tables("etapas").Rows.Count - 1
                CodigoEtapa = Trim(Dataset1.Tables("etapas").Rows(X).Item("codigo"))
                NombreEtapa = CodigoEtapa & ". " & Dataset1.Tables("etapas").Rows(X).Item("nombre")
                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedortitulos'>" + NombreEtapa + "<br /></div>"

                'Seleccion de las actuaciones segun la etapa que se este procesando
                If expediente = Nothing Then
                    cmd = "SELECT actuaciones.nombre, documentos.nomarchivo, documentos.paginas, documentos.id, documentos.ruta, documentos.entidad, documentos.fecharadic,entesdbf.nombre AS nomente,etapas.codigo AS idetapa, etapas.nombre AS nometapa, documentos.idacto, " & _
                   "'                                                                                                                                                                                                                                                              ' as rutaimagen," & _
                   "'                                                                                                                                                                                                                                                              ' as rutavisor " & _
                   "FROM documentos,actuaciones,entesdbf,etapas WHERE RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit)  AND RTRIM(entidad) = '" & idEntidad & "' AND RTRIM(actuaciones.idetapa) = RTRIM(etapas.codigo) AND idetapa = '" & _
                   CodigoEtapa & "' AND RTRIM(documentos.cobrador) = '" & Session("mcobrador") & "' AND RTRIM(entesdbf.cobrador) = '" & Session("mcobrador") & "' ORDER BY actuaciones.codigo, documentos.nomarchivo"
                Else
                    cmd = "SELECT actuaciones.nombre, documentos.nomarchivo, documentos.paginas, documentos.id, documentos.ruta, documentos.entidad, documentos.fecharadic,entesdbf.nombre AS nomente,etapas.codigo AS idetapa, etapas.nombre AS nometapa, documentos.idacto, " & _
                    "'                                                                                                                                                                                                                                                              ' as rutaimagen," & _
                    "'                                                                                                                                                                                                                                                              ' as rutavisor " & _
                    "FROM documentos,actuaciones,entesdbf,etapas WHERE RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit)  AND RTRIM(entidad) = '" & idEntidad & "' AND RTRIM(actuaciones.idetapa) = RTRIM(etapas.codigo) AND idetapa = '" & _
                    CodigoEtapa & "' AND RTRIM(documentos.cobrador) = '" & Session("mcobrador") & "' AND RTRIM(entesdbf.cobrador) = '" & Session("mcobrador") & "' AND RTRIM(documentos.docexpediente) = '" & expediente & "' ORDER BY actuaciones.codigo, documentos.nomarchivo"

                    pnlSeleccionarDatos.Visible = False
                End If

                oscmd.CommandText = cmd
                oDtAdapterSql1.SelectCommand = oscmd
                oDtAdapterSql1.Fill(Dataset1, "acts")
                If Dataset1.Tables("acts").Rows.Count = 0 Then
                    Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedorregistros'>No Hay documentos</div>"
                    Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedorfecharad'> </div>"
                    'Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedorfecharad'>algo </div>"
                Else
                    Dim NomEntidad, idActo As String
                    If Trim(Dataset1.Tables("acts").Rows(X).Item("nomente")) = "" Then
                        '
                    Else
                        NomEntidad = Trim(Dataset1.Tables("acts").Rows(X).Item("nomente"))
                    End If

                    Dim Y, totalpags As Integer
                    Dim NomActo, NomArchivo, ruta, FechaRadic As String
                    NomActo = ""
                    For Y = 0 To Dataset1.Tables("acts").Rows.Count - 1
                        If NomActo = Trim(Dataset1.Tables("acts").Rows(Y).Item("nombre")) Then
                            'Saltar
                        Else
                            NomActo = Trim(Dataset1.Tables("acts").Rows(Y).Item("nombre"))
                            NomArchivo = Trim(Dataset1.Tables("acts").Rows(Y).Item("nomarchivo"))
                            NomArchivo = NomArchivo.ToLower()
                            totalpags = Trim(Dataset1.Tables("acts").Rows(Y).Item("paginas"))
                            ruta = Trim(Dataset1.Tables("acts").Rows(Y).Item("ruta"))
                            Try
                                FechaRadic = ""
                                idActo = Trim(Dataset1.Tables("acts").Rows(Y).Item("idacto"))
                                If Dataset1.Tables("acts").Rows(Y).Item("fecharadic") Is DBNull.Value Then
                                    'Validar permisos
                                    If Session("mnivelacces") = "1" Then
                                        FechaRadic = "<a href='actualizarfechas.aspx?idente=" & idEntidad & "&nomente=" & NomEntidad & "&idacto=" & idActo & "&Expedinete=" & expediente & "'>Ingresar fecha de radicación</a>"
                                    End If
                                    'Fin val permisos
                                Else
                                    FechaRadic = Left(Trim(Dataset1.Tables("acts").Rows(Y).Item("fecharadic")), 10)
                                    If FechaRadic = "01/01/1900" Then
                                        'Validar permisos
                                        If Session("mnivelacces") = "1" Then
                                            FechaRadic = "<a href='actualizarfechas.aspx?idente=" & idEntidad & "&nomente=" & NomEntidad & "&idacto=" & idActo & "&Expedinete=" & expediente & "'>Ingresar fecha de radicación</a>"
                                        End If
                                        'Fin validar permisos
                                    Else
                                        Dim slash1, slash2, PosIni, NumCars As Integer
                                        If TipoConexion = "web" Then
                                            Dim dia, mes, anio As String
                                            If Mid(FechaRadic.Trim(), 2, 1) = "/" Then
                                                mes = "0" & Left(FechaRadic.Trim(), 1)
                                            Else
                                                mes = Left(FechaRadic.Trim(), 2)
                                            End If
                                            slash1 = InStr(FechaRadic.Trim(), "/")
                                            slash2 = InStrRev(FechaRadic.Trim(), "/")
                                            PosIni = slash1 + 1
                                            NumCars = slash2 - slash1 - 1
                                            If NumCars = 1 Then
                                                dia = "0" & Mid(FechaRadic.Trim(), PosIni, NumCars)
                                            Else
                                                dia = Mid(FechaRadic.Trim(), PosIni, NumCars)
                                            End If
                                            anio = Right(FechaRadic.Trim(), 4)
                                            FechaRadic = dia & "/" & mes & "/" & anio
                                        End If
                                        'Validar permisos
                                        If Session("mnivelacces") = "1" Then
                                            FechaRadic = "<a href='actualizarfechas.aspx?idente=" & idEntidad & "&nomente=" & NomEntidad & "&idacto=" & idActo & "&Expedinete=" & expediente & "'>" & FechaRadic & "</a>"
                                        End If
                                        'Fin validar permisos
                                    End If
                                End If
                            Catch ex As Exception
                                Me.Validator.Text = ex.Message.ToString()
                                Me.Validator.IsValid = False
                            End Try

                            'Columa de la actuacion
                            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedorregistros'>"
                            ' Validar permisos
                            If Session("mnivelacces") <> "3" Then
                                'Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<a href='visorimagen.aspx?nomente=" & NomEntidad & "&idente=" & idEntidad & "&F=" & NomArchivo & "&totimg=" & totalpags & "&acto=" & NomActo & "&idacto=" & idActo & "&folder=" & ruta & "'>"
                                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<a href='TiffViewer.aspx?nomente=" & NomEntidad & "&idente=" & idEntidad & "&F=" & NomArchivo & "&totimg=" & totalpags & "&acto=" & NomActo & "&idacto=" & idActo & "&folder=" & ruta & "&Expedinete=" & expediente & " '>"
                            End If
                            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + NomActo
                            ' Validar permisos
                            If Session("mnivelacces") <> "3" Then
                                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "</a>"
                            End If
                            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "</div>"

                            'Columna de la fecha de radicacion
                            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedorfecharad'>" & FechaRadic & "</div>"
                        End If
                    Next
                End If
                Dataset1.Tables("acts").Rows.Clear()
                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<br />"
            Next
        Catch ex As Exception
            Me.Validator.ErrorMessage = ex.Message
            Me.Validator.IsValid = False
        End Try
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        ListExpedientes.Items.Clear()
        Dim idEntidad As String
        idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
        'expediente(idEntidad)
        Dim cmd As String
        Dim cnn As String = Session("ConexionServer")

        cmd = "select distinct docexpediente from documentos where RTRIM(entidad) = '" & idEntidad & "' AND RTRIM(documentos.cobrador) = '" & Session("mcobrador") & "'"
        Dim MyAdapter As New SqlClient.SqlDataAdapter(cmd, cnn)
        Dim myTable As New DataTable
        MyAdapter.Fill(myTable)

        If myTable.Rows.Count > 1 Then
            pnlSeleccionarDatos.Visible = True

            Dim xt As Integer
            For xt = 0 To myTable.Rows.Count - 1
                ListExpedientes.Items.Add(myTable.Rows(xt).Item("docexpediente"))
            Next

            'Me.mpeSeleccion.Show()
            'expediente(idEntidad, Nothing)
        Else
            expediente(idEntidad, Nothing)
        End If
    End Sub

    Private Sub BeginSearch(ByVal Page As Integer, ByVal Datos As DataSet)
        If Datos.Tables("documentos").Rows.Count = 0 Then
            'Mostrar mensaje de no hay registros 
            Me.Validator.ErrorMessage = "No hay datos que coincidan con esta búsqueda"
            Me.Validator.IsValid = False
            'Else
            'Dim X As Integer = 0
            'Dim totalpags As Integer
            'Dim rutaimg, ruta, nomarchivo, entidad, NomEntidad, Actuacion As String            
        End If
        'Me.DataGrid1.DataSource = DataSet1.Tables("documentos")        
        Me.Session("Datos") = Datos
        'Me.DataGrid1.DataBind()
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Dim Dataset1 As DataSet = CType(Me.Session("Datos"), DataSet)
        Me.BeginSearch(e.NewPageIndex, Dataset1)
    End Sub

    Protected Sub btnSi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSi.Click
        Dim idEntidad As String
        idEntidad = Mid(Me.txtEnte.Text.Trim(), Me.txtEnte.Text.IndexOf(":") + 3)
        Dim expedientet As String
        expedientet = ListExpedientes.SelectedValue
        pnlSeleccionarDatos.Visible = False
        expediente(idEntidad, expedientet)
    End Sub

    Private Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        pnlSeleccionarDatos.Visible = False
    End Sub
End Class
