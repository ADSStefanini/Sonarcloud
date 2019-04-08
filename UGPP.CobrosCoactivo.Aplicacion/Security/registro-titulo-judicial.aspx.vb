Public Partial Class registro_titulo_judicial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub


    Private Sub TxtDocumento_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtDocumento.TextChanged
        Dim TblDatos As DataTable = New SQL_Titulos(Session("ConexionServer").ToString).Load_Titulo(TxtDocumento.Text)
        TxtExpediente.Text = ""
        TxtFecha.Text = ""
        TxtValor.Text = ""
        If TblDatos.Rows.Count = 1 Then
            TxtExpediente.Text = TblDatos.Rows(0).Item("MTJ_EXPEDIENTE")
            TxtFecha.Text = TblDatos.Rows(0).Item("MTJ_FECHA")
            TxtValor.Text = TblDatos.Rows(0).Item("MTJ_VALOR")
            CmbEstado.SelectedIndex = TblDatos.Rows(0).Item("MTJ_ESTADO")
        End If
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Dim TblDatos As DataTable = New SQL_Titulos(Session("ConexionServer").ToString).Load_Titulo(TxtDocumento.Text)
        Dim Row As DataRow
        If TblDatos.Rows.Count = 0 Then
            Row = TblDatos.NewRow
        Else
            Row = TblDatos.Rows(0)
        End If

        Row.Item("MTJ_REFERENCIA") = TxtDocumento.Text
        Row.Item("MTJ_EXPEDIENTE") = TxtExpediente.Text
        Row.Item("MTJ_FECHA") = TxtFecha.Text
        Row.Item("MTJ_VALOR") = TxtValor.Text
        Row.Item("MTJ_ESTADO") = CmbEstado.SelectedValue
        If TblDatos.Rows.Count = 0 Then
            Row.Item("MTJ_LIQUIDACION") = "1"
            TblDatos.Rows.Add(Row)
        End If

        If Not New SQL_Titulos(Session("ConexionServer").ToString).Save_Titulo(TblDatos) Then
            resul.InnerHtml = "Error"
        Else
            resul.InnerHtml = "Titulo guardado satisfactoriamente."
        End If
    End Sub

    Public Class SQL_Titulos

        Private _Conexion As SqlClient.SqlConnection

        Public Sub New(ByVal Conexion As String)
            _Conexion = New SqlClient.SqlConnection(Conexion)
        End Sub

        Public Function Load_Titulo(ByVal Referencia As String) As DataTable
            Dim AdapTitulo As New SqlClient.SqlDataAdapter("SELECT * FROM MAESTRO_TITULOS_JUDICIALES WHERE MTJ_REFERENCIA = @MTJ_REFERENCIA", _Conexion)
            AdapTitulo.SelectCommand.Parameters.AddWithValue("@MTJ_REFERENCIA", Referencia)
            Dim TblTitulo As New DataTable
            AdapTitulo.Fill(TblTitulo)
            Return TblTitulo
        End Function

        Public Function Save_Titulo(ByVal TblDatos As DataTable) As Boolean
            'Dim TblDatos As DataTable = Load_Titulo(Referencia)
            Dim Insert As New SqlClient.SqlCommand("INSERT INTO MAESTRO_TITULOS_JUDICIALES (MTJ_EXPEDIENTE, MTJ_LIQUIDACION, MTJ_VALOR, MTJ_FECHA, MTJ_REFERENCIA, MTJ_ESTADO) VALUES (@MTJ_EXPEDIENTE, @MTJ_LIQUIDACION, @MTJ_VALOR, @MTJ_FECHA, @MTJ_REFERENCIA, @MTJ_ESTADO)", _Conexion)
            Dim Update As New SqlClient.SqlCommand("UPDATE MAESTRO_TITULOS_JUDICIALES SET MTJ_EXPEDIENTE = @MTJ_EXPEDIENTE, MTJ_VALOR = @MTJ_VALOR, MTJ_FECHA = @MTJ_FECHA, MTJ_ESTADO = @MTJ_ESTADO WHERE MTJ_REFERENCIA = @ORIGINAL_MTJ_REFERENCIA", _Conexion)

            Insert.Parameters.Add("@MTJ_EXPEDIENTE", SqlDbType.Char, 10, "MTJ_EXPEDIENTE")
            Insert.Parameters.Add("@MTJ_LIQUIDACION", SqlDbType.Char, 50, "MTJ_LIQUIDACION")
            Insert.Parameters.Add("@MTJ_VALOR", SqlDbType.Char, 10, "MTJ_VALOR")
            Insert.Parameters.Add("@MTJ_FECHA", SqlDbType.Char, 10, "MTJ_FECHA")
            Insert.Parameters.Add("@MTJ_REFERENCIA", SqlDbType.Char, 20, "MTJ_REFERENCIA")
            Insert.Parameters.Add("@MTJ_ESTADO", SqlDbType.Char, 1, "MTJ_ESTADO")

            Update.Parameters.Add("@MTJ_EXPEDIENTE", SqlDbType.Char, 50, "MTJ_EXPEDIENTE")
            Update.Parameters.Add("@MTJ_VALOR", SqlDbType.Decimal, 18, "MTJ_VALOR")
            Update.Parameters.Add("@MTJ_FECHA", SqlDbType.DateTime, 7, "MTJ_FECHA")
            Update.Parameters.Add("@MTJ_ESTADO", SqlDbType.Char, 1, "MTJ_ESTADO")
            Update.Parameters.Add("@ORIGINAL_MTJ_REFERENCIA", SqlDbType.Char, 20, "MTJ_REFERENCIA")

            Dim Adapter As New SqlClient.SqlDataAdapter()
            Adapter.InsertCommand = Insert
            Adapter.UpdateCommand = Update

            Adapter.Update(TblDatos)

            Return True
        End Function

    End Class

End Class