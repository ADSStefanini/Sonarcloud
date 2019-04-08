Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Script.Services
Imports System.Collections.Generic
' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la siguiente línea.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Autocomplete
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Dim lista As New List(Of String)

        Return "Hola a todos"
    End Function

    <WebMethod()> _
    Public Function ObtListaEtidades(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim colEstados As New List(Of String)
        Dim con As New SqlConnection(CadenaConexion)
        Dim comando As New SqlCommand("select top 40 Rtrim(Ltrim(ED_Nombre )) + '::' + ED_Codigo_Nit  as retornar from ENTES_DEUDORES where (ED_Nombre like '%' + @param + '%' or ED_Codigo_Nit like '%' + @param + '%')", con)
        comando.Parameters.AddWithValue("param", prefixText)
        Dim dr As SqlDataReader
        comando.Connection.Open()
        dr = comando.ExecuteReader
        Dim lista As New List(Of String)
        While dr.Read
            lista.Add(Trim(dr.Item("retornar")))
        End While
        dr.Close()
        comando.Connection.Close()
        Return lista.ToArray
    End Function


    <WebMethod()> _
    Public Function ObtListaPropietario(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim colEstados As New List(Of String)
        Dim con As New SqlConnection(CadenaConexion)
        Dim comando As New SqlCommand("select top 40 Rtrim(Ltrim(nombre)) + '::' + codigo_nit as retornar from entesdbf where (nombre like '%' + @param + '%' or cedula like '%' + @param + '%')", con)
        comando.Parameters.AddWithValue("param", prefixText)
        Dim dr As SqlDataReader
        comando.Connection.Open()
        dr = comando.ExecuteReader
        Dim lista As New List(Of String)
        While dr.Read
            lista.Add(Trim(dr.Item("retornar")))
        End While
        dr.Close()
        comando.Connection.Close()
        Return lista.ToArray
    End Function

    <WebMethod()> _
   Public Function ObtListaEtapas(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim con As New SqlConnection(CadenaConexion)
        Dim comando As New SqlCommand("select Rtrim(Ltrim(nombre)) + '::' + codigo as retornar from etapas where (nombre like '%' + @param + '%' or codigo like '%' + @param + '%')", con)
        comando.Parameters.AddWithValue("param", prefixText)
        Dim dr As SqlDataReader
        comando.Connection.Open()
        dr = comando.ExecuteReader
        Dim lista As New List(Of String)
        While dr.Read
            lista.Add(Trim(dr.Item("retornar")))
        End While
        dr.Close()
        comando.Connection.Close()
        Return lista.ToArray
    End Function

    <WebMethod()> _
    Public Function ObtListaActuaciones(ByVal prefixText As String, ByVal count As Integer) As String()
        'Busqueda por actuacion
        Dim con As New SqlConnection(CadenaConexion)
        Dim comando As New SqlCommand("select Rtrim(Ltrim(nombre)) + '::' + codigo as retornar from actuaciones where (nombre like '%' + @param + '%' or codigo like '%' + @param + '%')", con)
        comando.Parameters.AddWithValue("param", prefixText)
        Dim dr As SqlDataReader
        comando.Connection.Open()
        dr = comando.ExecuteReader
        Dim lista As New List(Of String)
        While dr.Read
            lista.Add(Trim(dr.Item("retornar")))
        End While
        dr.Close()
        comando.Connection.Close()
        Return lista.ToArray
    End Function

#Region "Union de Bases de datos"
    <WebMethod()> _
   Public Function ObtListaEtidades_Est(ByVal prefixText As String, ByVal count As Integer) As String()
        'Busqueda por nombre o cedula
        Dim colEstados As New List(Of String)
        Dim con As New SqlConnection(Funciones.CadenaConexion)

        Dim comando As New SqlCommand("SELECT TOP 20 Rtrim(Efinom) + '::' + EfiNit AS retornar FROM ejefisglobal WHERE (Efinom LIKE '%' + @param + '%' OR Efinit LIKE '%' + @param + '%')", con)
        comando.Parameters.AddWithValue("param", prefixText)
        Dim dr As SqlDataReader
        comando.Connection.Open()
        dr = comando.ExecuteReader
        Dim lista As New List(Of String)
        While dr.Read
            lista.Add(Trim(dr.Item("retornar")))
        End While
        dr.Close()
        comando.Connection.Close()
        Return lista.ToArray
    End Function

    <WebMethod()> _
    Public Function ObtListaEtidades_EstPredio(ByVal prefixText As String, ByVal count As Integer) As String()
        'Busqueda por predio o placa
        Dim colEstados As New List(Of String)
        Dim con As New SqlConnection(Funciones.CadenaConexion)
        
        Dim comando As New SqlCommand("select distinct top 20  Efigen  as retornar from EJEFISGLOBAL where (EfiGen like '%' + @param + '%')", con)
        comando.Parameters.AddWithValue("param", prefixText)
        Dim dr As SqlDataReader
        comando.Connection.Open()
        dr = comando.ExecuteReader
        Dim lista As New List(Of String)
        While dr.Read
            lista.Add(Trim(dr.Item("retornar")))
        End While
        dr.Close()
        comando.Connection.Close()
        Return lista.ToArray
    End Function

    <WebMethod()> _
    Public Function ObtListaEtidades_Expediente(ByVal prefixText As String, ByVal count As Integer) As String()
        'Busqueda por expediente
        Dim colEstados As New List(Of String)
        Dim con As New SqlConnection(Funciones.CadenaConexion)
        Dim comando As New SqlCommand("SELECT DISTINCT TOP 20 EfiNroExp AS retornar FROM  EJEFISGLOBAL WHERE EfiNroExp LIKE '%' + @param + '%' ORDER BY 1", con)
        comando.Parameters.AddWithValue("param", prefixText)
        Dim dr As SqlDataReader
        comando.Connection.Open()
        dr = comando.ExecuteReader
        Dim lista As New List(Of String)
        While dr.Read
            lista.Add(Trim(dr.Item("retornar")))
        End While
        dr.Close()
        comando.Connection.Close()
        Return lista.ToArray
    End Function
#End Region
   


    'Private Function CadenaConexion2() As String
    '    Dim conexion As String = ""
    '    Dim NomServidor, Usuario, Clave, BaseDatos As String
    '    NomServidor = ConfigurationManager.AppSettings("ServerName2")
    '    Usuario = ConfigurationManager.AppSettings("BD_User2")
    '    Clave = ConfigurationManager.AppSettings("BD_pass2")
    '    BaseDatos = ConfigurationManager.AppSettings("BD_name2")


    '    conexion = "workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
    '        ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave

    '    Return conexion
    'End Function
    Private Function CadenaConexion() As String
        Dim conexion As String = ""
        Dim NomServidor, Usuario, Clave, BaseDatos As String
        NomServidor = ConfigurationManager.AppSettings("ServerName")
        Usuario = ConfigurationManager.AppSettings("BD_User")
        Clave = ConfigurationManager.AppSettings("BD_pass")
        BaseDatos = ConfigurationManager.AppSettings("BD_name")


        conexion = "workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
            ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave

        Return conexion
    End Function
End Class