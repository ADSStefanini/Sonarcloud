Imports System.Data.SqlClient

Public Class FuncionesInformes

    Public Sub CargarInformacionDeudores()
        Using con As New SqlConnection(Funciones.CadenaConexion)
            con.Open()
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanager As New DataTable
            Dim sqlcommand As New SqlCommand("DIRECCIONESXEXPEDIENTE", con)
            sqlcommand.CommandType = CommandType.StoredProcedure
            sqlcommand.CommandTimeout = 600 ' 600 = 10 minutos
            sqlcommand.ExecuteNonQuery()
            con.Close()
        End Using
    End Sub

    Public Sub CargarCambiosEstado()
        Using con As New SqlConnection(Funciones.CadenaConexion)
            con.Open()
            Dim sqladapter As New SqlDataAdapter
            Dim sqldatamanager As New DataTable
            Dim sqlcommand As New SqlCommand("CAMBIOESTADOS", con)
            sqlcommand.CommandType = CommandType.StoredProcedure
            sqlcommand.CommandTimeout = 600 ' 600 = 10 minutos
            sqlcommand.ExecuteNonQuery()
            con.Close()
        End Using
    End Sub

    
End Class
