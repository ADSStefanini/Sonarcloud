Imports System.Data.SqlClient
Public Class actuaciones
    Inherits System.Web.UI.Page

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.daActuaciones = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand
        Me.DsExpedientes1 = New dsExpedientes
        CType(Me.DsExpedientes1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'daActuaciones
        '
        Me.daActuaciones.DeleteCommand = Me.SqlDeleteCommand1
        Me.daActuaciones.InsertCommand = Me.SqlInsertCommand1
        Me.daActuaciones.SelectCommand = Me.SqlSelectCommand1
        Me.daActuaciones.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "actuaciones", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("codigo", "codigo"), New System.Data.Common.DataColumnMapping("nombre", "nombre"), New System.Data.Common.DataColumnMapping("idetapa", "idetapa"), New System.Data.Common.DataColumnMapping("manejaterm", "manejaterm"), New System.Data.Common.DataColumnMapping("termino", "termino"), New System.Data.Common.DataColumnMapping("dependen1", "dependen1"), New System.Data.Common.DataColumnMapping("dependen2", "dependen2")})})
        Me.daActuaciones.UpdateCommand = Me.SqlUpdateCommand1
        '
        'SqlDeleteCommand1
        '
        Me.SqlDeleteCommand1.CommandText = "DELETE FROM actuaciones WHERE (codigo = @mcodigo)"
        Me.SqlDeleteCommand1.Connection = Me.SqlConnection1
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@mcodigo", System.Data.SqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "codigo", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlConnection1
        '
        Me.SqlConnection1.ConnectionString = "workstation id=PORTATILRAFA;packet size=4096;user id=userpensiones;data source=po" & _
        "rtatilrafa;persist security info=True;initial catalog=pensionesweb;password=1020" & _
        "30"
        '
        'SqlInsertCommand1
        '
        Me.SqlInsertCommand1.CommandText = "INSERT INTO actuaciones (codigo, nombre, idetapa, manejaterm, termino, dependen1," & _
        " dependen2) VALUES (@codigo, @nombre, @idetapa, @manejaterm, @termino, @dependen" & _
        "1, @dependen2)"
        Me.SqlInsertCommand1.Connection = Me.SqlConnection1
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@codigo", System.Data.SqlDbType.VarChar, 3, "codigo"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@nombre", System.Data.SqlDbType.VarChar, 250, "nombre"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@idetapa", System.Data.SqlDbType.VarChar, 2, "idetapa"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@manejaterm", System.Data.SqlDbType.Int, 4, "manejaterm"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@termino", System.Data.SqlDbType.Int, 4, "termino"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@dependen1", System.Data.SqlDbType.VarChar, 3, "dependen1"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@dependen2", System.Data.SqlDbType.VarChar, 3, "dependen2"))
        '
        'SqlSelectCommand1
        '
        Me.SqlSelectCommand1.CommandText = "SELECT codigo, nombre, idetapa, manejaterm, termino, dependen1, dependen2 FROM ac" & _
        "tuaciones WHERE (RTRIM(codigo) = @mcodigo)"
        Me.SqlSelectCommand1.Connection = Me.SqlConnection1
        Me.SqlSelectCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@mcodigo", System.Data.SqlDbType.VarChar))
        '
        'SqlUpdateCommand1
        '
        Me.SqlUpdateCommand1.CommandText = "UPDATE actuaciones SET nombre = @nombre, idetapa = @idetapa, manejaterm = @maneja" & _
        "term, termino = @termino, dependen1 = @dependen1, dependen2 = @dependen2 WHERE (" & _
        "codigo = @mcodigo)"
        Me.SqlUpdateCommand1.Connection = Me.SqlConnection1
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@nombre", System.Data.SqlDbType.VarChar, 250, "nombre"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@idetapa", System.Data.SqlDbType.VarChar, 2, "idetapa"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@manejaterm", System.Data.SqlDbType.Int, 4, "manejaterm"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@termino", System.Data.SqlDbType.Int, 4, "termino"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@dependen1", System.Data.SqlDbType.VarChar, 3, "dependen1"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@dependen2", System.Data.SqlDbType.VarChar, 3, "dependen2"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@mcodigo", System.Data.SqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "codigo", System.Data.DataRowVersion.Original, Nothing))
        '
        'DsExpedientes1
        '
        Me.DsExpedientes1.DataSetName = "dsExpedientes"
        Me.DsExpedientes1.Locale = New System.Globalization.CultureInfo("es-CO")
        CType(Me.DsExpedientes1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents btnAceptar As System.Web.UI.WebControls.Button
    Protected WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents daActuaciones As System.Data.SqlClient.SqlDataAdapter
    Protected WithEvents DsExpedientes1 As dsExpedientes
    Protected WithEvents txtNombre As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents txtCodigo As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkmanejaterm As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtTermino As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents lstdependencia1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lstdependencia2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Protected WithEvents CustomValidator1 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents dtgetapa_acto As Global.System.Web.UI.WebControls.GridView
    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Function LoadDatos() As DataTable
        Dim cnn As String = Session("ConexionServer")
        Dim MyAdapter As New SqlDataAdapter("select actuaciones.codigo,actuaciones.nombre,actuaciones.idetapa,etapas.nombre as nometapa from actuaciones ,etapas where actuaciones.idetapa = etapas.codigo and actuaciones.codigo <> '000'", cnn)
        Dim Mytb As New DatasetForm.etapa_actoDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

    Private Sub cargeetapa_acto()
        Dim Table As DatasetForm.etapa_actoDataTable = LoadDatos()
        Me.ViewState("Datosetapa_acto") = Table
        dtgetapa_acto.DataSource = Table
        dtgetapa_acto.DataBind()
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not Page.IsPostBack Then
            Try
                Me.SqlConnection1.ConnectionString = Session("ConexionServer").ToString
                chkmanejaterm.Attributes.Add("onclick", "CheckVal(0)")
                Call cargeetapa_acto()

                Dim Sql As New SqlClient.SqlDataAdapter("SELECT codigo, nombre from actuaciones ORDER BY codigo", Me.SqlConnection1)
                Sql.Fill(Me.DsExpedientes1.actuaciones)
                Me.lstdependencia1.DataBind()
                Me.lstdependencia2.DataBind()
            Catch ex As Exception
                CustomValidator1.ErrorMessage = "Error : " & ex.Message & "<br />" & "Es posible que se haya perdido la conexión con el servidor, si el problema persiste intente salir y volver entrar al sistema."
                CustomValidator1.IsValid = False
            End Try
        End If
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        Try
            With Me
                Me.SqlConnection1.ConnectionString = Session("ConexionServer").ToString
                .daActuaciones.SelectCommand.Parameters("@mcodigo").Value = .txtCodigo.Text.Trim
                If .daActuaciones.Fill(.DsExpedientes1, "actuaciones") > 0 Then
                    .txtNombre.Value = .DsExpedientes1.actuaciones.Item(0).nombre
                    'Casilla de verificacion (maneja termino)
                    If .DsExpedientes1.actuaciones.Item(0).manejaterm = 1 Then
                        .chkmanejaterm.Checked = True
                        .txtTermino.Enabled = True
                        .lstdependencia1.Enabled = True
                        .lstdependencia2.Enabled = True
                    Else
                        .chkmanejaterm.Checked = False
                        .txtTermino.Enabled = False
                        .lstdependencia1.Enabled = False
                        .lstdependencia2.Enabled = False
                    End If
                    .txtTermino.Text = .DsExpedientes1.actuaciones.Item(0).termino

                    'Dependencia 1
                    'Dim x As Integer
                    'For x = 0 To .lstdependencia1.Items.Count - 1
                    '    If .lstdependencia1.Items(x).Value.Trim = .DsExpedientes1.actuaciones.Item(0).dependen1.Trim Then
                    '        .lstdependencia1.SelectedIndex = x
                    '        Exit For
                    '    End If
                    'Next

                    'Dependencia 2                
                    'For x = 0 To .lstdependencia2.Items.Count - 1
                    '    If .lstdependencia2.Items(x).Value.Trim = .DsExpedientes1.actuaciones.Item(0).dependen2.Trim Then
                    '        .lstdependencia2.SelectedIndex = x
                    '        Exit For
                    '    End If
                    'Next


                    'Leyton codigo
                    lstdependencia1.SelectedValue = valorNull(DsExpedientes1.actuaciones.Item(0).dependen1.Trim, "000")
                    lstdependencia2.SelectedValue = valorNull(DsExpedientes1.actuaciones.Item(0).dependen2.Trim, "000")
                Else
                    .txtNombre.Value = ""
                    .chkmanejaterm.Checked = False
                    .txtTermino.Text = 0
                End If
            End With
        Catch ex As Exception
            CustomValidator1.ErrorMessage = "Error : " & ex.Message & "<br />" & "Es posible que se haya perdido la conexión con el servidor, si el problema persiste intente salir y volver entrar al sistema."
            CustomValidator1.IsValid = False
        End Try
    End Sub

    Private Sub chkmanejaterm_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkmanejaterm.CheckedChanged
        With Me
            If .chkmanejaterm.Checked = True Then
                .txtTermino.Enabled = True
                .lstdependencia1.Enabled = True
                .lstdependencia2.Enabled = True                
            Else
                .txtTermino.Enabled = False
                .lstdependencia1.Enabled = False
                .lstdependencia2.Enabled = False
                .txtTermino.Text = 0
                .lstdependencia1.SelectedIndex = 0
                .lstdependencia2.SelectedIndex = 0
            End If
        End With
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            With Me
                Me.SqlConnection1.ConnectionString = Session("ConexionServer").ToString
                .daActuaciones.SelectCommand.Parameters("@mcodigo").Value = .txtCodigo.Text.Trim()
                If .daActuaciones.Fill(.DsExpedientes1, "actuaciones") Then
                    'La actuación existe y hay que editarla
                    .DsExpedientes1.actuaciones.Item(0).nombre = .txtNombre.Value.Trim
                    If .chkmanejaterm.Checked = True Then
                        .DsExpedientes1.actuaciones.Item(0).manejaterm = 1
                    Else
                        .DsExpedientes1.actuaciones.Item(0).manejaterm = 0
                    End If

                    .DsExpedientes1.actuaciones.Item(0).termino = txtTermino.Text.Trim
                    .DsExpedientes1.actuaciones.Item(0).dependen1 = .lstdependencia1.SelectedValue
                    .DsExpedientes1.actuaciones.Item(0).dependen2 = .lstdependencia2.SelectedValue
                Else
                    'Codigo de insercion (La actuación no existe
                    '.DsExpedientes1.actuaciones.AddactuacionesRow(.txtCodigo.Text.Trim, .txtNombres.Text.Trim, _
                    '    .txtApellidos.Text.Trim, .txtDireccion.Text.Trim, .txtTelefono.Text.Trim, _
                    '    .lstpaises.SelectedValue.Trim, .txtCiudad.Text.Trim)
                    .CustomValidator1.ErrorMessage = "Esta actuacion no existe"
                    .CustomValidator1.IsValid = False
                End If
                .daActuaciones.Update(.DsExpedientes1.actuaciones)
                .CustomValidator1.ErrorMessage = "Datos actualizados correctamente"
                .CustomValidator1.IsValid = False
            End With
        Catch ex As Exception
            CustomValidator1.ErrorMessage = ex.Message
            CustomValidator1.IsValid = False
        End Try
    End Sub

    Private Sub dtgetapa_acto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgetapa_acto.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.etapa_actoDataTable = CType(.ViewState("Datosetapa_acto"), DatasetForm.etapa_actoDataTable)
            With Mytb.Item(.dtgetapa_acto.SelectedIndex)
                Try
                    txtCodigo.Text= .codigo
                    txtNombre.value = .nombre
                Catch ex As Exception
                    CustomValidator1.ErrorMessage = "Error : " & ex.Message & "<br />" & "Es posible que se haya perdido la conexión con el servidor, si el problema persiste intente salir y volver entrar al sistema."
                    CustomValidator1.IsValid = False
                End Try
            End With
        End With
    End Sub
End Class
