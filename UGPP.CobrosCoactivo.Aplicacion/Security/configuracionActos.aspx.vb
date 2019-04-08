Imports System.Data.SqlClient
Partial Public Class configuracionActos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'LoadGrid("")

            'txtservicio.Attributes.Add("onChange", "Rep();")
            Page.Form.Attributes.Add("autocomplete", "off")
            If Session("ConexionServer") = Nothing Then
                Session("ConexionServer") = CadenaConexion()
            End If

            txtservicio.Text = Request.QueryString("texto")
            txtBuscar.Text = Request.QueryString("etapa")
            txtTermino.Text = Request.QueryString("termino")
            Dim tipoteras As String = Request.QueryString("tipotermino")

            If tipoteras = "DIA" Then
                OptTermino.Items(0).Value = True
            Else
                OptTermino.Items(1).Value = True
            End If

            If txtservicio.Text <> Nothing Then
                Call cargar()
                Dim tb As New DataTable
                tb = Dependencias(Val(txtservicio.Text))

                Dim Rows As DataRow = tb.Select("DEP_CONMOV='" & txtservicio.Text & "'")(0)
                If Not Rows Is Nothing Then
                    DropDownListActo1.SelectedValue = Rows("DEP_CODACTO")
                    'DropDownListActo2.SelectedValue = Rows("DEP_DEPENDENCIA")
                End If

                info.InnerHtml = "En este momento si presiona agregar se procederá a actualizar el registro… <br /> <b>Nota :</b> Si quiere crear uno nuevo presione cancelar y seleccione los actos administrativos."
            End If
        Else

        End If
    End Sub

    Private Sub cargar()
        Dim codigo As String
        codigo = Mid(Me.txtBuscar.Text.Trim(), Me.txtBuscar.Text.IndexOf(":") + 3)

        DropDownListActo1.DataSource = Actuaciones(codigo)
        DropDownListActo1.DataTextField = "nombre"
        DropDownListActo1.DataValueField = "codigo"
        DropDownListActo1.DataBind()

        'DropDownListActo2.DataSource = Actuaciones(codigo)
        'DropDownListActo2.DataTextField = "nombre"
        'DropDownListActo2.DataValueField = "codigo"
        'DropDownListActo2.DataBind()
    End Sub
    Private Sub LoadGrid(ByVal pValor As String)
        Dim cmd As String
        'Dim cmd, criterio, condicion As String
        Dim cnx As String = Funciones.CadenaConexion
        cmd = ""
        If pValor = "" Then
            cmd = "SELECT TOP 50 DEP_DEPENDENCIA, DEP_DESCRIPCION, DEP_TERMINO FROM DEPENDENCIA_ACTUACIONES WHERE DEP_CODACTO  = '001'"
        Else
            'criterio = cmbBuscarPor.SelectedValue
            'condicion = ""
            'Select Case criterio
            '    Case "NroPredial"
            cmd = "SELECT TOP 50 DEP_DEPENDENCIA, DEP_DESCRIPCION, DEP_TERMINO FROM DEPENDENCIA_ACTUACIONES " & _
                "WHERE DEP_CODACTO = '" & pValor & "'"
            '        'Columna 2                    
            '        CType(GridVPredios.Columns(1), BoundField).DataField = "PreCod"
            '        GridVPredios.Columns(1).HeaderText = "Cod. Corto"
            '        GridVPredios.Columns(1).Visible = True
            '        'Columna 3
            '        CType(GridVPredios.Columns(2), BoundField).DataField = "PreMatInm"
            '        GridVPredios.Columns(2).HeaderText = "Matrícula Inmobiliaria"
            '        GridVPredios.Columns(2).Visible = True                
            'End Select
            'cmd 
        End If

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtDependencias As New DataTable
        Adaptador.Fill(dtDependencias)

        GridDependencias.DataSource = dtDependencias
        GridDependencias.DataBind()
        If dtDependencias.Rows.Count = 0 Then
            'Validator.ErrorMessage = "No se han encontrado resultados para su búsqueda"
            'Validator.IsValid = False
        End If

        ' Poner la etiqueta de predio seleccionado en Ninguno y quitar resaltado de GridView
        'LabelPredioSel.Text = "Ninguno"
        Me.GridDependencias.SelectedIndex = -1
    End Sub
    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        Call cargar()
        LoadGrid(Me.DropDownListActo1.Text.Trim)
    End Sub

    Function Actuaciones(ByVal etapas As String) As DataTable
        Dim datata As New DataTable
        Dim myadapa As New SqlDataAdapter("SELECT * FROM ACTUACIONES WHERE IDETAPA = @ETAPA ORDER BY CODIGO", CadenaConexion)
        myadapa.SelectCommand.Parameters.Add("@ETAPA", SqlDbType.VarChar)
        myadapa.SelectCommand.Parameters("@ETAPA").Value = etapas

        myadapa.Fill(datata)
        Return datata
    End Function

    Function Dependencias(ByVal cod As Integer) As DataTable
        Dim datata As New DataTable
        Dim myadapa As New SqlDataAdapter("SELECT * FROM DEPENDENCIA_ACTUACIONES WHERE DEP_CONMOV = @cod", CadenaConexion)
        myadapa.SelectCommand.Parameters.Add("@cod", SqlDbType.Int)
        myadapa.SelectCommand.Parameters("@cod").Value = cod

        myadapa.Fill(datata)

        Return datata
    End Function


    Private Function LoadDatos(ByVal cedula As String) As DataTable
        Dim cnn As String = Session("ConexionServer").ToString
        Dim MyAdapter As New SqlDataAdapter("select * from DEPENDENCIA_ACTUACIONES WHERE DEP_CONMOV='" & cedula.Trim & "'", cnn)
        Dim Mytb As New DatasetForm.DEPENDENCIA_ACTUACIONESDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

    Private Sub cargeactos(ByVal cedula As String)
        Dim Table As DatasetForm.DEPENDENCIA_ACTUACIONESDataTable = LoadDatos(cedula)
        Me.ViewState("Datosetapa_acto") = Table
    End Sub
    Private Sub btnMas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMas.Click
        If Not IsNumeric(txtTermino.Text) Then
            txtTermino.Text = 0
        End If

        If txtBuscar.Text = Nothing Or txtTermino.Text = Nothing Or DropDownListActo1.SelectedItem.Value = Nothing Then
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & "Faltan datos para continuar." & "</font>"
            Exit Sub
        End If

        Dim boolmis As Boolean = actuamis()
        If boolmis = True Then
            Call actualiza_depmis()
            Exit Sub
        End If


        Try
            Using con As New SqlConnection(Session("ConexionServer").ToString)
                'With Me
                '    Dim da As New SqlDataAdapter("SELECT * FROM DEPENDENCIA_ACTUACIONES WHERE DEP_CODACTO= @DEP_CODACTO", con)
                '    da.SelectCommand.Parameters.Add("@DEP_CODACTO", SqlDbType.VarChar).Value = .DropDownListActo1.DataValueField
                '    cargeactos(txtservicio.Text)

                '    da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                '    ' Creamos los comandos con el CommandBuilder
                '    Dim cb As New SqlCommandBuilder(da)

                '    da.InsertCommand = cb.GetInsertCommand()
                '    da.UpdateCommand = cb.GetUpdateCommand()
                '    'da.DeleteCommand = cb.GetDeleteCommand()

                '    con.Open()
                '    Dim tran As SqlTransaction = con.BeginTransaction

                '    da.InsertCommand.Transaction = tran
                '    da.UpdateCommand.Transaction = tran
                '    'da.DeleteCommand.Transaction = tran

                '    Dim Mytb As New DatasetForm.DEPENDENCIA_ACTUACIONESDataTable
                '    Mytb = CType(.ViewState("Datosetapa_acto"), DatasetForm.DEPENDENCIA_ACTUACIONESDataTable)

                '    If Mytb Is Nothing Then
                '        Mytb = New DatasetForm.DEPENDENCIA_ACTUACIONESDataTable
                '    End If
                '    Dim etapa As String = ""
                '    etapa = Mid(Me.txtBuscar.Text.Trim(), Me.txtBuscar.Text.IndexOf(":") + 3)
                '    If (Mytb.Select("DEP_CONMOV='" & txtservicio.Text & "'").Length > 0) OrElse (Mytb.Select("DEP_CODACTO='" & DropDownListActo1.SelectedItem.Value & "' AND DEP_DEPENDENCIA = '" & DropDownListActo2.SelectedItem.Value & "' AND DEP_ETAPA = '" & etapa & "'").Length > 0) Then
                '        'En esta parate se procede a actualizar el registro si existe 
                '        Dim Row As DatasetForm.DEPENDENCIA_ACTUACIONESRow = Mytb.Select("DEP_CONMOV='" & txtservicio.Text & "'")(0)
                '        If Not Row Is Nothing Then
                '            Row.DEP_CODACTO = DropDownListActo1.SelectedItem.Value
                '            Row.DEP_NOMBREPPAL = DropDownListActo1.SelectedItem.Text
                '            Row.DEP_DEPENDENCIA = DropDownListActo2.SelectedItem.Value
                '            Row.DEP_DESCRIPCION = DropDownListActo2.SelectedItem.Text
                '            Row.DEP_TERMINO = txtTermino.Text
                '            Dim tTermino As String
                '            If OptTermino.Items(0).Selected = True Then
                '                tTermino = "DIA"
                '            Else
                '                tTermino = "MES"
                '            End If
                '            Row.DEP_TIPOTERMINO = tTermino
                '        End If
                '    Else

                '        Dim tTermino As String

                '        If OptTermino.Items(0).Selected = True Then
                '            tTermino = "DIA"
                '        Else
                '            tTermino = "MES"
                '        End If

                '        Mytb.AddDEPENDENCIA_ACTUACIONESRow(DropDownListActo1.SelectedItem.Value, DropDownListActo1.SelectedItem.Text, DropDownListActo2.SelectedItem.Value, DropDownListActo2.SelectedItem.Text, txtTermino.Text, tTermino, "001", etapa, "S", 0)
                '    End If

                '    Try
                '        'Actualizamos los datos de la tabla
                '        da.Update(Mytb)
                '        tran.Commit()
                '        'Mytb.AcceptChanges()
                '        Messenger.InnerHtml = "<font color='#FFFFFF'><b> Se han guardado los datos</b><br /> <b>Nota :</b> Si quiere actualizar los datos debe escogerlo de la tabla, si no será tomado como un registro nuevo.</font>"
                '        txtservicio.Text = ""

                '        ViewState("dato_1") = etapa
                '        ViewState("dato_2") = DropDownListActo1.SelectedItem.Value
                '        ViewState("dato_3") = DropDownListActo2.SelectedItem.Value
                '    Catch ex As Exception
                '        ' Si hay error, desahacemos lo que se haya hecho
                '        tran.Rollback()
                '        Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
                '    End Try
                '    con.Close()
                'End With
            End Using
        Catch ex As Exception
            Messenger.InnerHtml = "<font color='#8A0808'><b>Error : </b>" & ex.Message & "</font>"
        End Try
    End Sub
    Private Function actuamis() As Boolean
        'Dim etapa As String = ""
        'etapa = Mid(Me.txtBuscar.Text.Trim(), Me.txtBuscar.Text.IndexOf(":") + 3)
        'If (DropDownListActo1.SelectedItem.Value = ViewState("dato_2") And DropDownListActo2.SelectedItem.Value = ViewState("dato_3") And etapa = ViewState("dato_1")) Then
        '    Return True
        'Else
        '    Return False
        'End If
    End Function
    Private Sub actualiza_depmis()
        'Dim sql As String
        'Dim etapa As String = ""

        'Dim tTermino As String
        'If OptTermino.Items(0).Selected = True Then
        '    tTermino = "DIA"
        'Else
        '    tTermino = "MES"
        'End If

        'etapa = Mid(Me.txtBuscar.Text.Trim(), Me.txtBuscar.Text.IndexOf(":") + 3)
        'sql = "UPDATE DEPENDENCIA_ACTUACIONES SET DEP_CODACTO = @DEP_CODACTO, DEP_NOMBREPPAL = @DEP_NOMBREPPAL, DEP_DEPENDENCIA = @DEP_DEPENDENCIA, DEP_DESCRIPCION = @DEP_DESCRIPCION, DEP_TERMINO = @DEP_TERMINO, DEP_TIPOTERMINO = @DEP_TIPOTERMINO, DEP_ETAPA = @DEP_ETAPA WHERE DEP_CODACTO = @DAT_1 AND DEP_DEPENDENCIA = @DAT_2 AND DEP_ETAPA = @DAT_3"

        'Dim con As New SqlConnection(Session("ConexionServer").ToString)
        'Dim cm As New SqlCommand

        'cm.Parameters.Add("@DAT_1", SqlDbType.Char).Value = ViewState("dato_2")
        'cm.Parameters.Add("@DAT_2", SqlDbType.Char).Value = ViewState("dato_3")
        'cm.Parameters.Add("@DAT_3", SqlDbType.Char).Value = ViewState("dato_1")


        'cm.Parameters.Add("@DEP_CODACTO", SqlDbType.Char).Value = DropDownListActo1.SelectedItem.Value
        'cm.Parameters.Add("@DEP_NOMBREPPAL", SqlDbType.Char).Value = DropDownListActo1.SelectedItem.Text
        'cm.Parameters.Add("@DEP_DEPENDENCIA", SqlDbType.Char).Value = DropDownListActo2.SelectedItem.Value
        'cm.Parameters.Add("@DEP_DESCRIPCION", SqlDbType.Char).Value = DropDownListActo2.SelectedItem.Text
        'cm.Parameters.Add("@DEP_TERMINO", SqlDbType.Char).Value = txtTermino.Text
        'cm.Parameters.Add("@DEP_TIPOTERMINO", SqlDbType.Char).Value = tTermino
        'cm.Parameters.Add("@DEP_ETAPA", SqlDbType.Char).Value = etapa

        'cm.CommandText = sql
        'con.Open()
        'cm.Connection = con
        'cm.ExecuteNonQuery()
        'con.Close()
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        txtservicio.Text = ""
        info.InnerHtml = "<b>Nota : </b>Para guardar un secuencia de actos administrativos (registro  nuevo) seleccione o digite una etapa y asigne las dependencias correspondientes a la misma."
        Messenger.InnerHtml = ""
    End Sub

    Protected Sub DropDownListActo1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListActo1.SelectedIndexChanged
        LoadGrid(Me.DropDownListActo1.Text.Trim)
    End Sub
End Class
