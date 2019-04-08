Imports System.Data.SqlClient
Imports AjaxControlToolkit.HTMLEditor
Partial Public Class predialvariable011
    Inherits System.Web.UI.Page

    'Dim conec1 As New SqlConnection(Funciones.CadenaConexionUnion)
    'Dim conec As New SqlConnection(Funciones.CadenaConexion)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Response.Redirect("~/Login.aspx")
        End If
    End Sub
    Protected Sub Link_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Link.Click
        Ejecuta_informe()
    End Sub

    Public Sub Ejecuta_informe()
        Dim tab As New DataTable("EJEFIS")
        Dim tab2 As New DataTable("Registro_excepciones")
        Dim consultaImpuestos As String = "SELECT PRENUM, EFIDIR, EFINOM, EFINIT,'' AS RESOLUCION FROM EJEFISGLOBAL WHERE EFINROEXP = @EFINROEXP"
        Dim consultaLocal As String = "SELECT * FROM REGISTRO_EXCEPCIONES WHERE NUMERO_EXPEDIENTE = @EFINROEXP AND IDACTO = '011' AND IMPUESTO = @IMPUESTO"
        Dim cmdDatos As New SqlCommand(consultaImpuestos, New SqlConnection(Funciones.CadenaConexion))
        cmdDatos.Parameters.Add("@EFINROEXP", SqlDbType.VarChar, 50).Value = Me.txtNumExp.Text
        cmdDatos.Parameters.Add("@IMPUESTO", SqlDbType.Char, 1).Value = Session("ssimpuesto")
        Dim Dts As New DataSet
        cmdDatos.CommandTimeout = 99999
        Dim adap As New SqlClient.SqlDataAdapter
        adap.SelectCommand = cmdDatos
        adap.Fill(tab)
        If tab.Rows.Count > 0 Then
            cmdDatos.Connection = New SqlConnection(Funciones.CadenaConexion)
            cmdDatos.CommandText = consultaLocal
            adap.SelectCommand = (cmdDatos)
            adap.Fill(tab2)
            If tab2.Rows.Count > 0 Then
                Dts.Tables.Add(tab)
                Dts.Tables.Add(tab2)
                saveResolucion(txtNumExp.Text, "011", Dts)
                Session("ssRegistroExcepciones") = CType(Dts, DataSet)
                Dim imp As Integer = Session("ssimpuesto")
                Select Case imp
                    Case 1
                        Response.Write("<script type='text/javascript'>window.open('predialvariable011Report.aspx');</script>")
                    Case 2
                        Response.Write("<script type='text/javascript'>window.open('predialvariable011Report_in.aspx');</script>")
                End Select
            Else
                lblres.Text = "No Se encontró este número de expediente en el sistema. <br /> Favor capturar y guardar los datos previos para continuar."
            End If
        Else
            lblres.Text = "No Se encontró este número de expediente en el sistema."
        End If
    End Sub

    Protected Sub txtNumExp_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtNumExp.TextChanged
        '1. Buscar el expediente en la tabla de documentos, sino existe => mostrar un mensaje
        Limpiar()
        Dim cons As Boolean = False
        Dim Adaptador As New SqlDataAdapter("SELECT TOP 1 DOCEXPEDIENTE FROM DOCUMENTOS WHERE DOCEXPEDIENTE = @DOCEXPEDIENTE ", New SqlConnection(Funciones.CadenaConexion))
        Adaptador.SelectCommand.Parameters.Add("@DOCEXPEDIENTE", SqlDbType.VarChar, 50).Value = txtNumExp.Text.Trim
        Dim dtDocumentos As New DataTable
        Adaptador.Fill(dtDocumentos)
        If dtDocumentos.Rows.Count = 0 Then
            lblres.Text = "El expediente digitado no existe en la base de datos o no puede ser procesado como :<b>" & Session("ssCodimpadm") & "</b>"
            Exit Sub
        Else
            cons = True
        End If

        '2. Buscar el expediente en la tabla Registro_excepciones. Si existe=> mostrar campos, sino dejarlos en blanco
        Dim Adaptador2 As New SqlDataAdapter("SELECT * FROM REGISTRO_EXCEPCIONES WHERE NUMERO_EXPEDIENTE = @NUMERO_EXPEDIENTE AND IDACTO = '011' AND IMPUESTO = @IMPUESTO", New SqlConnection(Funciones.CadenaConexion))
        Adaptador2.SelectCommand.Parameters.Add("@IMPUESTO", SqlDbType.Char, 1).Value = Session("ssimpuesto")
        Adaptador2.SelectCommand.Parameters.Add("@NUMERO_EXPEDIENTE", SqlDbType.VarChar, 50).Value = txtNumExp.Text.Trim
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)
        If dtRegistro_excepciones.Rows.Count > 0 Then
            'Mostrar campos
            txtCalendario.Text = dtRegistro_excepciones.Rows(0).Item(1)
            EditorHechos.Content = dtRegistro_excepciones.Rows(0).Item(4).ToString
            EditorConsideraciones.Content = dtRegistro_excepciones.Rows(0).Item(5).ToString
            EditorArticulos.Content = dtRegistro_excepciones.Rows(0).Item(6).ToString
        Else
            IIf(cons = True, lblres.Text = "Digite los datos para continuar", lblres.Text = "Este puede ser procesado como: <b>" & Session("ssCodimpadm") & "</b>")
        End If




    End Sub

    Private Sub Limpiar()
        lblres.Text = ""
        txtCalendario.Text = ""
        EditorHechos.Content = ""
        EditorConsideraciones.Content = ""
        EditorArticulos.Content = ""
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        'Si el registro existe en la tabla Registro_excepciones => UPDATE, sino: INSERT
        Dim Adaptador2 As New SqlDataAdapter("SELECT * FROM REGISTRO_EXCEPCIONES WHERE NUMERO_EXPEDIENTE = @NUMERO_EXPEDIENTE AND IDACTO='011'", New SqlConnection(Funciones.CadenaConexion))
        Adaptador2.SelectCommand.Parameters.Add("@NUMERO_EXPEDIENTE", SqlDbType.VarChar, 50).Value = txtNumExp.Text.Trim
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)
        Dim cmd2 As SqlCommand
        If dtRegistro_excepciones.Rows.Count > 0 Then
            'UPDATE
            cmd2 = New SqlCommand("UPDATE REGISTRO_EXCEPCIONES SET tipo_excepcion=@codigo,fecha_de_acto=@fecha,usuario=@user,Hechos=@texto,consideraciones=@texto2,ARTICULO_VARIABLE=@ARTICULO_VARIABLE where numero_expediente=@codigoExpediente and idacto='011'", New SqlConnection(Funciones.CadenaConexion))
        Else
            'INSERT
            cmd2 = New SqlCommand("INSERT INTO REGISTRO_EXCEPCIONES(tipo_excepcion,fecha_de_acto,numero_expediente,usuario,Hechos,consideraciones,ARTICULO_VARIABLE,idacto,IMPUESTO) VALUES (@codigo,@fecha,@codigoExpediente,@user,@texto,@texto2,@ARTICULO_VARIABLE,'011',@IMPUESTO)", New SqlConnection(Funciones.CadenaConexion))
        End If
        cmd2.Connection.Open()
        cmd2.Parameters.AddWithValue("@codigo", Me.DropCodigoActo.Text)
        cmd2.Parameters.AddWithValue("@texto", EditorHechos.Content)
        cmd2.Parameters.AddWithValue("@texto2", Me.EditorConsideraciones.Content)
        cmd2.Parameters.AddWithValue("@user", Session("sscodigousuario"))
        cmd2.Parameters.AddWithValue("@fecha", Me.txtCalendario.Text)
        cmd2.Parameters.AddWithValue("@codigoExpediente", Me.txtNumExp.Text)
        cmd2.Parameters.AddWithValue("@ARTICULO_VARIABLE", EditorArticulos.Content)
        cmd2.Parameters.AddWithValue("@IMPUESTO", Session("ssimpuesto"))
        cmd2.ExecuteNonQuery()
        cmd2.Connection.Close()
    End Sub

    Private Sub saveResolucion(ByVal expediente As String, ByVal acto As String, ByVal Dts As DataSet)
        Dim resolucion As DataTable = Funciones.InsertReslucion(expediente, acto)
        If resolucion.Rows.Count > 0 Then
            If Dts.Tables("EJEFIS").Rows.Count > 0 Then
                '---Cambiar en Tabla de mandaniento de pago
                Dts.Tables("EJEFIS").Rows(0).Item("RESOLUCION") = resolucion.Rows(0).Item(3)
            End If
        End If
    End Sub
End Class
