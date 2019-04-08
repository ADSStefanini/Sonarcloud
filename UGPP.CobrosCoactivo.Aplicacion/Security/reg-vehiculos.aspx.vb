Public Partial Class reg_vehiculos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioValido") Is Nothing Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Protected Sub TxtPlaca_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TxtPlaca.TextChanged
        Dim TblDatos As DataTable = New SQL_Vehiculo(Session("ConexionServer").ToString).Load_Vehiculo(TxtPlaca.Text)
        TxtMarca.Text = ""
        TxtLinea.Text = ""
        TxtModelo.Text = ""
        TxtColor.Text = ""
        If TblDatos.Rows.Count = 1 Then
            TxtPropietario.Text = TblDatos.Rows(0).Item("PROPIETARIO_ID")
            TxtOrganismo.Text = TblDatos.Rows(0).Item("ORGANISMO_TRANSITO")
            TxtMarca.Text = TblDatos.Rows(0).Item("MVEH_MARCA")
            TxtLinea.Text = TblDatos.Rows(0).Item("MVEH_LINEA")
            TxtModelo.Text = TblDatos.Rows(0).Item("MVEH_MODELO")
            TxtColor.Text = TblDatos.Rows(0).Item("MVEH_COLOR")
        End If
    End Sub

    Function isValida() As Boolean
        If IsNumeric(TxtModelo.Text) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        If isValida() = False Then
            resul.InnerHtml = "Ingrese correctamente los valores"
            Exit Sub
        End If

        Dim TblDatos As DataTable = New SQL_Vehiculo(Session("ConexionServer").ToString).Load_Vehiculo(TxtPlaca.Text)
        Dim Row As DataRow
        If TblDatos.Rows.Count = 0 Then
            Row = TblDatos.NewRow
        Else
            Row = TblDatos.Rows(0)
        End If

        Row.Item("MVEH_PLACA") = TxtPlaca.Text
        Row.Item("PROPIETARIO_ID") = TxtPropietario.Text
        Row.Item("ORGANISMO_TRANSITO") = TxtOrganismo.Text
        Row.Item("MVEH_MARCA") = TxtMarca.Text
        Row.Item("MVEH_LINEA") = TxtLinea.Text
        Row.Item("MVEH_MODELO") = TxtModelo.Text
        Row.Item("MVEH_COLOR") = TxtColor.Text

        If TblDatos.Rows.Count = 0 Then
            TblDatos.Rows.Add(Row)
        End If

        If Not New SQL_Vehiculo(Session("ConexionServer").ToString).Save_Vehiculo(TblDatos) Then
            resul.InnerHtml = "Error"
        Else
            resul.InnerHtml = "Vehículo guardado satisfactoriamente."
        End If
    End Sub

    Public Class SQL_Vehiculo
        Private _Conexion As SqlClient.SqlConnection

        Public Sub New(ByVal Conexion As String)
            _Conexion = New SqlClient.SqlConnection(Conexion)
        End Sub

        Public Function Load_Vehiculo(ByVal Placa As String) As DataTable
            Dim Adapter As New SqlClient.SqlDataAdapter("SELECT * FROM MAESTRO_VEHICULOS WHERE MVEH_PLACA = @MVEH_PLACA", _Conexion)
            Adapter.SelectCommand.Parameters.AddWithValue("@MVEH_PLACA", Placa)
            Dim TblDatos As New DataTable
            Adapter.Fill(TblDatos)
            Return TblDatos
        End Function

        Public Function Save_Vehiculo(ByVal TblDatos As DataTable) As Boolean
            'Dim TblDatos As DataTable = Load_Titulo(Referencia)
            Dim Insert As New SqlClient.SqlCommand("INSERT INTO MAESTRO_VEHICULOS (MVEH_PLACA, PROPIETARIO_ID, ORGANISMO_TRANSITO, MVEH_MARCA, MVEH_LINEA, MVEH_COLOR, MVEH_MODELO) VALUES (@MVEH_PLACA, @PROPIETARIO_ID, @ORGANISMO_TRANSITO, @MVEH_MARCA, @MVEH_LINEA, @MVEH_COLOR, @MVEH_MODELO)", _Conexion)
            Dim Update As New SqlClient.SqlCommand("UPDATE MAESTRO_VEHICULOS SET PROPIETARIO_ID = @PROPIETARIO_ID, ORGANISMO_TRANSITO = @ORGANISMO_TRANSITO, MVEH_MARCA = @MVEH_MARCA, MVEH_LINEA = @MVEH_LINEA, MVEH_COLOR = @MVEH_COLOR, MVEH_MODELO = @MVEH_MODELO WHERE MVEH_PLACA = @ORIGINAL_MVEH_PLACA", _Conexion)

            Insert.Parameters.Add("@MVEH_MARCA", SqlDbType.Char, 100, "MVEH_MARCA")
            Insert.Parameters.Add("@PROPIETARIO_ID", SqlDbType.Char, 50, "PROPIETARIO_ID")
            Insert.Parameters.Add("@ORGANISMO_TRANSITO", SqlDbType.Char, 100, "ORGANISMO_TRANSITO")
            Insert.Parameters.Add("@MVEH_LINEA", SqlDbType.Char, 100, "MVEH_LINEA")
            Insert.Parameters.Add("@MVEH_COLOR", SqlDbType.Char, 100, "MVEH_COLOR")
            Insert.Parameters.Add("@MVEH_MODELO", SqlDbType.Int, 4, "MVEH_MODELO")
            Insert.Parameters.Add("@MVEH_PLACA", SqlDbType.Char, 6, "MVEH_PLACA")

            Update.Parameters.Add("@PROPIETARIO_ID", SqlDbType.Char, 50, "PROPIETARIO_ID")
            Update.Parameters.Add("@ORGANISMO_TRANSITO", SqlDbType.Char, 100, "ORGANISMO_TRANSITO")
            Update.Parameters.Add("@MVEH_MARCA", SqlDbType.Char, 100, "MVEH_MARCA")
            Update.Parameters.Add("@MVEH_LINEA", SqlDbType.Char, 100, "MVEH_LINEA")
            Update.Parameters.Add("@MVEH_COLOR", SqlDbType.Char, 100, "MVEH_COLOR")
            Update.Parameters.Add("@MVEH_MODELO", SqlDbType.Int, 4, "MVEH_MODELO")
            Update.Parameters.Add("@ORIGINAL_MVEH_PLACA", SqlDbType.Char, 10, "MVEH_PLACA")

            Dim Adapter As New SqlClient.SqlDataAdapter()
            Adapter.InsertCommand = Insert
            Adapter.UpdateCommand = Update

            Adapter.Update(TblDatos)

            Return True
        End Function
    End Class
End Class