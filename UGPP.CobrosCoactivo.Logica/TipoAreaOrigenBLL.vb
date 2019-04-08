Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports System.Configuration

Public Class TipoAreaOrigenBLL

    Private Property _TipoAreaOrigenDAL As TipoAreaOrigenDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _TipoAreaOrigenDAL = New TipoAreaOrigenDAL()
    End Sub
    Public Function ConsultarAreaOrigen() As List(Of TipoAreaOrigen)
        Return _TipoAreaOrigenDAL.ConsultarAreaOrigen()
    End Function
End Class
