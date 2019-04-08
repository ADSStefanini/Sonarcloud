Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos

Public Class AlmacenamientoTemporalBLL
    Private Property _AlmacenamientoTemporalDAL As AlmacenamientoTemporalDAL
    Private Property _Audit As LogAuditoria

    Public Sub New()
        _AlmacenamientoTemporalDAL = New AlmacenamientoTemporalDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _Audit = auditData
        _AlmacenamientoTemporalDAL = New AlmacenamientoTemporalDAL(_Audit)
    End Sub

    Public Function consultarAlmacenamientoPorId(idAlamacenamiento As Long) As AlmacenamientoTemporal
        Return _AlmacenamientoTemporalDAL.consultarAlmacenamientoPorId(idAlamacenamiento)
    End Function

    Public Sub actualizarAlmacenamiento(prmAlmacenamientoTemporal As AlmacenamientoTemporal)
        _AlmacenamientoTemporalDAL.actualizarAlmacenamiento(prmAlmacenamientoTemporal)
    End Sub

    Public Function InsertarAlmacenamiento(prmAlmacenamientoTemporal As AlmacenamientoTemporal) As AlmacenamientoTemporal
        Return _AlmacenamientoTemporalDAL.InsertarAlmacenamiento(prmAlmacenamientoTemporal)
    End Function
End Class
