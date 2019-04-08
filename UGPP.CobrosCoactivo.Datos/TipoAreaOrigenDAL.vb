Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports System.Configuration

Public Class TipoAreaOrigenDAL
    Inherits AccesObject(Of TipoAreaOrigen)

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub

    Public Function ConsultarAreaOrigen() As List(Of TipoAreaOrigen)
        Return ExecuteList("SP_TIPOS_AREA_ORIGEN")
    End Function
End Class
