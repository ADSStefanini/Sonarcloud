Imports UGPP.CobrosCoactivo.Datos
Public Class Relacion_Estado_EtapaBLL
    Private Property _RelacionEP_EPDal As RelacionEP_EPDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _RelacionEP_EPDal = New RelacionEP_EPDAL()
    End Sub
    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _RelacionEP_EPDal = New RelacionEP_EPDAL(_AuditEntity)
    End Sub
    Public Function ConsultaEstadoEtapaPorID(ByVal codigo_estado As String, ByVal codigo_etapa As Int32) As Int32
        Dim retorno As Int32 = _RelacionEP_EPDal.ConsultaEstadoEtapaPorID(codigo_estado, codigo_etapa)
        Return retorno
    End Function
    Public Sub InsertarEstadoEtapa(ByVal codigo_estado As String, ByVal codigo_etapa As Int32)
        _RelacionEP_EPDal.InsertarEstadoEtapa(codigo_estado, codigo_etapa)
    End Sub
    Public Sub BorradoEstadoEtapa(ByVal codigo_estado As String, ByVal codigo_etapa As Int32)
        _RelacionEP_EPDal.EliminarRegistro(codigo_estado, codigo_etapa)
    End Sub
End Class
