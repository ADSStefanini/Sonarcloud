Imports System.Data.SqlClient

Partial Public Class predialvariable032
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Link_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Link.Click
        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        Dim tab As New DataTable("EJEFIS")
        Dim tab2 As New DataTable("Registro_excepciones")
        Dim cmd1 As New SqlCommand("select PreNum,EfiDir,EfiNom,EfiNit,''AS RESOLUCION from EJEFISGLOBAL where EfiNroExp ='" & Me.txtNumExp.Text & "'", conec)
        Dim cmd2 As New SqlCommand("SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='032'", conec)
        Dim Dts As New DataSet
        Dim adap As New SqlClient.SqlDataAdapter
        adap.SelectCommand = cmd1
        adap.Fill(tab)
        adap.SelectCommand = (cmd2)
        adap.Fill(tab2)
        Dts.Tables.Add(tab)
        Dts.Tables.Add(tab2)
        saveResolucion(txtNumExp.Text, "032", Dts)

        Session("ssRegistroExcepciones") = CType(Dts, DataSet)
        Dim imp As Integer = Session("ssimpuesto")
        Select Case imp
            Case 1
                Response.Write("<script type='text/javascript'>window.open('predialvariable032Report.aspx');</script>")
            Case 2
                Response.Write("<script type='text/javascript'>window.open('predialvariable032Report_in.aspx');</script>")
        End Select
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        'Si el registro existe en la tabla Registro_excepciones => UPDATE, sino: INSERT
        Dim cmd As String
        cmd = "SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='032'"
        Dim Adaptador2 As New SqlDataAdapter(cmd, conec)
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)

        Dim cmd2 As SqlCommand
        conec.Open()
        If dtRegistro_excepciones.Rows.Count > 0 Then
            'UPDATE
            cmd2 = New SqlCommand("UPDATE Registro_excepciones SET tipo_excepcion=@codigo,fecha_de_acto=@fecha,usuario=@user,Hechos=@texto,consideraciones=@texto2,ARTICULO_VARIABLE=@ARTICULO_VARIABLE where numero_expediente=@codigoExpediente and idacto='032'", conec)
        Else
            'INSERT
            cmd2 = New SqlCommand("INSERT INTO Registro_excepciones(tipo_excepcion,fecha_de_acto,numero_expediente,usuario,Hechos,consideraciones,idacto,ARTICULO_VARIABLE) VALUES (@codigo,@fecha,@codigoExpediente,@user,@texto,@texto2,'032',@ARTICULO_VARIABLE)", conec)
        End If
        cmd2.Parameters.AddWithValue("@codigo", Me.DropCodigoActo.Text)
        cmd2.Parameters.AddWithValue("@texto", Me.TextArea1.Value)
        cmd2.Parameters.AddWithValue("@texto2", Me.TxtArea2.Value)
        cmd2.Parameters.AddWithValue("@user", Session("sscodigousuario"))
        cmd2.Parameters.AddWithValue("@fecha", Me.txtCalendario.Text)
        cmd2.Parameters.AddWithValue("@codigoExpediente", Me.txtNumExp.Text)
        cmd2.Parameters.AddWithValue("@ARTICULO_VARIABLE", Me.TextAreaArticulo.Value)
        cmd2.ExecuteNonQuery()
        conec.Close()
    End Sub
    Sub clearcampo()
        Me.TextArea1.Value = ""
        Me.TxtArea2.Value = ""
        Me.txtCalendario.Text = ""
        Me.TextAreaArticulo.Value = ""
    End Sub

    Protected Sub txtNumExp_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtNumExp.TextChanged
        Dim conec As New SqlConnection(Funciones.CadenaConexion)
        '1. Buscar el expediente en la tabla de documentos, sino existe => mostrar un mensaje
        Dim cmd As String
        cmd = "SELECT TOP 1 docexpediente FROM documentos WHERE docexpediente = '" & txtNumExp.Text.Trim & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, conec)
        Dim dtDocumentos As New DataTable
        Adaptador.Fill(dtDocumentos)
        If dtDocumentos.Rows.Count = 0 Then
            LBLRES.Text = "El expediente digitado no existe en la base de datos"
        Else
            'CustomValidator1.Text = ""
            LBLRES.Text = ""
        End If

        '2. Buscar el expediente en la tabla Registro_excepciones. Si existe=> mostrar campos, sino dejarlos en blanco
        cmd = "SELECT * FROM Registro_excepciones WHERE numero_expediente = '" & txtNumExp.Text.Trim & "' and idacto='032'"
        Dim Adaptador2 As New SqlDataAdapter(cmd, conec)
        Dim dtRegistro_excepciones As New DataTable
        Adaptador2.Fill(dtRegistro_excepciones)
        If dtRegistro_excepciones.Rows.Count > 0 Then
            'Mostrar campos
            txtCalendario.Text = dtRegistro_excepciones.Rows(0).Item(1)
            TextArea1.Value = dtRegistro_excepciones.Rows(0).Item(4).ToString
            TxtArea2.Value = dtRegistro_excepciones.Rows(0).Item(5).ToString
            TextAreaArticulo.Value = dtRegistro_excepciones.Rows(0).Item(6).ToString
            Dim codacto As String = "032"
        Else
            'Dejar controles en blanco
            txtCalendario.Text = ""
            TextArea1.Value = ""
            TxtArea2.Value = ""
            TextAreaArticulo.Value = ""
        End If
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