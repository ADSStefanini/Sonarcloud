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
Public Class Funciones_Server
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function ActivarUsuario(ByVal codigo As String, ByVal SiNo_Activo As Boolean) As String
        Dim activodes As String = SiNo_Activo

        Using conexcion As New SqlConnection(Session("ConexionServer").ToString)
            Using mycmd As New SqlCommand
                mycmd.CommandType = CommandType.Text
                If activodes = True Then
                    mycmd.Parameters.Add("@USERACTIVO", SqlDbType.VarChar).Value = False
                Else
                    mycmd.Parameters.Add("@USERACTIVO", SqlDbType.VarChar).Value = True
                End If

                mycmd.CommandText = "UPDATE USUARIOS SET USERACTIVO = @USERACTIVO WHERE CODIGO = @CODIGO"
                mycmd.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = ""
                mycmd.ExecuteNonQuery()

                Return "Usuario "
            End Using
        End Using
    End Function


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