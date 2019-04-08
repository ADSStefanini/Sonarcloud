Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections.Specialized
Public Class actualizarfechas
    Inherits System.Web.UI.Page
    Dim idEnte, nomEnte, idacto As String
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.daDocumentos = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand
        Me.DsExpedientes1 = New dsExpedientes


        CType(Me.DsExpedientes1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'daDocumentos
        '
        Me.daDocumentos.DeleteCommand = Me.SqlDeleteCommand1
        Me.daDocumentos.InsertCommand = Me.SqlInsertCommand1
        Me.daDocumentos.SelectCommand = Me.SqlSelectCommand1
        Me.daDocumentos.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "documentos", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("entidad", "entidad"), New System.Data.Common.DataColumnMapping("idacto", "idacto"), New System.Data.Common.DataColumnMapping("ruta", "ruta"), New System.Data.Common.DataColumnMapping("nomarchivo", "nomarchivo"), New System.Data.Common.DataColumnMapping("id", "id"), New System.Data.Common.DataColumnMapping("paginas", "paginas"), New System.Data.Common.DataColumnMapping("fecharadic", "fecharadic")})})
        Me.daDocumentos.UpdateCommand = Me.SqlUpdateCommand1
        '
        'SqlDeleteCommand1
        '
        Me.SqlDeleteCommand1.CommandText = "DELETE FROM documentos WHERE (id = @Original_id) AND (entidad = @Original_entidad" & _
        ") AND (fecharadic = @Original_fecharadic OR @Original_fecharadic IS NULL AND fec" & _
        "haradic IS NULL) AND (idacto = @Original_idacto) AND (nomarchivo = @Original_nom" & _
        "archivo) AND (paginas = @Original_paginas) AND (ruta = @Original_ruta)"
        Me.SqlDeleteCommand1.Connection = Me.SqlConnection1
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_id", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(10, Byte), CType(0, Byte), "id", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_entidad", System.Data.SqlDbType.VarChar, 20, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "entidad", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_fecharadic", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "fecharadic", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_idacto", System.Data.SqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "idacto", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_nomarchivo", System.Data.SqlDbType.VarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "nomarchivo", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_paginas", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(10, Byte), CType(0, Byte), "paginas", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ruta", System.Data.SqlDbType.VarChar, 250, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ruta", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand1
        '
        Me.SqlInsertCommand1.CommandText = "INSERT INTO documentos(entidad, idacto, ruta, nomarchivo, paginas, fecharadic) VA" & _
        "LUES (@entidad, @idacto, @ruta, @nomarchivo, @paginas, @fecharadic); SELECT enti" & _
        "dad, idacto, ruta, nomarchivo, id, paginas, fecharadic FROM documentos WHERE (id" & _
        " = @@IDENTITY)"
        Me.SqlInsertCommand1.Connection = Me.SqlConnection1
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@entidad", System.Data.SqlDbType.VarChar, 20, "entidad"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@idacto", System.Data.SqlDbType.VarChar, 3, "idacto"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ruta", System.Data.SqlDbType.VarChar, 250, "ruta"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@nomarchivo", System.Data.SqlDbType.VarChar, 100, "nomarchivo"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@paginas", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(10, Byte), CType(0, Byte), "paginas", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@fecharadic", System.Data.SqlDbType.DateTime, 8, "fecharadic"))
        '
        'SqlSelectCommand1
        '
        Me.SqlSelectCommand1.CommandText = "SELECT entidad, idacto, ruta, nomarchivo, id, paginas, fecharadic FROM documentos" & _
        " WHERE (RTRIM(entidad) = @entidad) AND (RTRIM(nomarchivo) = @nomarchivo)"
        Me.SqlSelectCommand1.Connection = Me.SqlConnection1
        Me.SqlSelectCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@entidad", System.Data.SqlDbType.VarChar))
        Me.SqlSelectCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@nomarchivo", System.Data.SqlDbType.VarChar))
        '
        'SqlUpdateCommand1
        '
        Me.SqlUpdateCommand1.CommandText = "UPDATE documentos SET entidad = @entidad, idacto = @idacto, ruta = @ruta, nomarch" & _
        "ivo = @nomarchivo, paginas = @paginas, fecharadic = @fecharadic WHERE (id = @Ori" & _
        "ginal_id) AND (entidad = @Original_entidad) AND (fecharadic = @Original_fecharad" & _
        "ic OR @Original_fecharadic IS NULL AND fecharadic IS NULL) AND (idacto = @Origin" & _
        "al_idacto) AND (nomarchivo = @Original_nomarchivo) AND (paginas = @Original_pagi" & _
        "nas) AND (ruta = @Original_ruta); SELECT entidad, idacto, ruta, nomarchivo, id, " & _
        "paginas, fecharadic FROM documentos WHERE (id = @id)"
        Me.SqlUpdateCommand1.Connection = Me.SqlConnection1
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@entidad", System.Data.SqlDbType.VarChar, 20, "entidad"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@idacto", System.Data.SqlDbType.VarChar, 3, "idacto"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ruta", System.Data.SqlDbType.VarChar, 250, "ruta"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@nomarchivo", System.Data.SqlDbType.VarChar, 100, "nomarchivo"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@paginas", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(10, Byte), CType(0, Byte), "paginas", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@fecharadic", System.Data.SqlDbType.DateTime, 8, "fecharadic"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_id", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(10, Byte), CType(0, Byte), "id", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_entidad", System.Data.SqlDbType.VarChar, 20, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "entidad", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_fecharadic", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "fecharadic", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_idacto", System.Data.SqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "idacto", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_nomarchivo", System.Data.SqlDbType.VarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "nomarchivo", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_paginas", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(10, Byte), CType(0, Byte), "paginas", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ruta", System.Data.SqlDbType.VarChar, 250, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ruta", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@id", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(10, Byte), CType(0, Byte), "id", System.Data.DataRowVersion.Current, Nothing))
        '
        'DsExpedientes1
        '
        Me.DsExpedientes1.DataSetName = "dsExpedientes"
        Me.DsExpedientes1.Locale = New System.Globalization.CultureInfo("es-CO")
        CType(Me.DsExpedientes1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtEnte As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents Validator As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents daDocumentos As System.Data.SqlClient.SqlDataAdapter
    Protected WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents DsExpedientes1 As dsExpedientes
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection

    Protected WithEvents txtFechaRad As System.Web.UI.WebControls.TextBox


    Protected WithEvents HyperLink4 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HyperLink5 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HyperLink6 As System.Web.UI.WebControls.HyperLink

    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents CalendarExtender1 As Global.AjaxControlToolkit.CalendarExtender
    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager

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

        'If Not Me.Page.IsPostBack Then
        '    Dim Adap As SqlClient.SqlDataAdapter
        '    Dim Table As DataTable = New DataTable
        '    Adap = New SqlClient.SqlDataAdapter("SELECT codigo,nombre FROM actuaciones", Me.SqlConnection1)
        'End If

        Dim expediente As String
        expediente = Request.QueryString("Expedinete")

        If expediente <> Nothing Then
            Me.Validator.Text = "Expediente : " & expediente
            Me.Validator.IsValid = False
        End If

        idEnte = RTrim(Request.QueryString("idente"))
        nomEnte = RTrim(Request.QueryString("nomente"))
        idacto = RTrim(Request.QueryString("idacto"))
        Me.txtEnte.Value = nomEnte & "::" & idEnte
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim idEntidad, cmd As String
        idEntidad = Mid(Me.txtEnte.Value.Trim(), Me.txtEnte.Value.IndexOf(":") + 3)

        cmd = "SELECT actuaciones.nombre, documentos.nomarchivo, documentos.paginas, documentos.id, documentos.ruta, documentos.entidad, documentos.fechaacto,entesdbf.nombre AS nomente, " & _
            "'                                                                                                                                                                                                                                                              ' as rutaimagen," & _
            "'                                                                                                                                                                                                                                                              ' as rutavisor " & _
            "FROM documentos,actuaciones,entesdbf WHERE RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit)  AND RTRIM(entidad) = '" & idEntidad & "'"

        'Creacion de objeto SQLCommand
        Dim oscmd As SqlClient.SqlCommand
        oscmd = New SqlClient.SqlCommand(cmd, Me.SqlConnection1)

        'Creacion del objeto DataAdapter
        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, Me.SqlConnection1)
        oDtAdapterSql1.SelectCommand = oscmd

        'Creacion del DataSet
        Dim Dataset1 As DataSet = New DataSet
        oDtAdapterSql1.Fill(Dataset1, "documentos")
        'Me.BeginSearch(0, Dataset1)

        'Me.CmbEtapa.SelectedValue
    End Sub
    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Dim Dataset1 As DataSet = CType(Me.Session("Datos"), DataSet)
        'Me.BeginSearch(e.NewPageIndex, Dataset1)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            'Dim CarpetaTrabajo As String = Me.txtEnte.Value.Trim()
            Dim idEntidad As String
            Dim recs As Integer
            idEntidad = Mid(Me.txtEnte.Value.Trim(), Me.txtEnte.Value.IndexOf(":") + 3)

            'Preparar los datos que se van a insertar en la tabla de documentos
            Dim sw, dia, mes, anio, fechaserver, TipoConexion As String
            Dim fecharadic As Date
            TipoConexion = ConfigurationManager.AppSettings("tipoconexion") 'local o web

            If Me.txtFechaRad.Text = "" Then
                Me.Validator.Text = "Digite la fecha de radicación por favor"
                Me.Validator.IsValid = False
                Return
            End If

            Try
                If TipoConexion = "web" Then
                    dia = Left(Me.txtFechaRad.Text.Trim(), 2)
                    mes = Mid(Me.txtFechaRad.Text.Trim(), 4, 2)
                    anio = Mid(Me.txtFechaRad.Text.Trim(), 7, 4)
                    fechaserver = mes & "/" & dia & "/" & anio
                    fecharadic = CDate(fechaserver)
                Else
                    fecharadic = CDate(Me.txtFechaRad.Text)
                    'fecharadic = Format(CDate(Me.txtFechaRad.Value), "dd/MM/yyyy")
                End If
            Catch ex As Exception
                Me.Validator.Text = "<font color='#8A0808' >Error :" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
                Me.Validator.IsValid = False
                Return
            End Try

            'Actualizar la fecha de radicacion de los registros de la actuacion

            Dim cmd As String = ""
            Dim expediente As String
            expediente = Request.QueryString("Expedinete")

            If expediente = Nothing Then
                cmd = "UPDATE documentos SET fecharadic = '" & fecharadic & "' WHERE entidad = '" & idEnte & "' AND idacto = '" & idacto & "' AND cobrador = '" & Session("mcobrador") & "'"
            Else
                cmd = "UPDATE documentos SET fecharadic = '" & fecharadic & "' WHERE entidad = '" & idEnte & "' AND idacto = '" & idacto & "' AND cobrador = '" & Session("mcobrador") & "' and docexpediente='" & expediente & "'"
            End If

            Me.SqlConnection1.Open()
            Dim cmdupdate As SqlCommand = New SqlCommand(cmd, Me.SqlConnection1)
            cmdupdate.CommandType = CommandType.Text

            recs = cmdupdate.ExecuteNonQuery()


            Me.Validator.Text = recs & " registros actualizados con éxito"
            'Me.Validator.Text = cmd
            Me.Validator.IsValid = False
        Catch ex As Exception
            Me.Validator.Text = ex.Message
            Me.Validator.IsValid = False
        End Try
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
         Dim mIdEnte, mNomEnte As String
        mIdEnte = Request.QueryString("idente")
        mNomEnte = Request.QueryString("nomente")
        Response.Redirect("consultarentes.aspx?ente=" & mIdEnte & "&nombente=" & mNomEnte & "&Expedinete=" & Request.QueryString("Expedinete").ToString)
    End Sub
End Class
