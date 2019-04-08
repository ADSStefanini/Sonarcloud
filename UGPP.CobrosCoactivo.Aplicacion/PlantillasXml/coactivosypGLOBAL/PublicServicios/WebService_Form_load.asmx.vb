Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la siguiente línea.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WebService_Form_load
    Inherits System.Web.Services.WebService

    <WebMethod(Description:="Devuelve la conexión local o pricipal de coactivosyp", MessageName:="CONEXION_PPAL")> _
    Public Function Retorna_Conexion(ByVal cod As String, ByVal pws As String) As String
        Dim conexion As String = Funciones.CadenaConexion
        Return conexion
    End Function

    <WebMethod(Description:="Devuelve la conexión con la que esta amarado el gestor documental ", MessageName:="CONEXION_DE_UNION")> _
  Public Function Retorna_Conexion_Union(ByVal cod As String, ByVal pws As String) As String
        Dim conexion As String = Funciones.CadenaConexionUnion
        Return conexion
    End Function

End Class