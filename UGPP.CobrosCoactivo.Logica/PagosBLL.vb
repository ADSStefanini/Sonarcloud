Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades
Public Class PagosBLL

    Private Property _pagosDAL As PagosDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _pagosDAL = New PagosDAL()
    End Sub
    ''' <param name="auditData"></param>
    Public Sub Auditoria(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _pagosDAL = New PagosDAL(_AuditEntity)
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Datos.PAGOS a Entidades.Pagos
    ''' </summary>
    ''' <param name="prmObjPagos">Objeto de tipo Datos.PAGOS</param>
    ''' <returns>Objeto de tipo Entidades.Pagos</returns>
    Private Function ConvertirAEntidadPagos(ByVal prmObjPagos As Datos.PAGOS) As Entidades.Pagos
        Dim pagos As Entidades.Pagos
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.Pagos, Datos.PAGOS)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        pagos = IMapper.Map(Of Datos.PAGOS, Entidades.Pagos)(prmObjPagos)
        Return pagos
    End Function
    ''' <summary>
    ''' Consulta pagos por idExpediente 
    ''' </summary>
    ''' <param name="idExpediente">id expediente para consultar pagos</param>
    ''' <returns>Lista de objetos del tipo Datos.PAGOS</returns>
    Public Function consultarPagos(ByVal idExpediente As String) As List(Of Entidades.Pagos)
        Dim resultadoConsulta = _pagosDAL.consultarPagosPorIdExpediente(idExpediente)
        Dim pagos As New List(Of Entidades.Pagos)
        For Each pago As Datos.PAGOS In resultadoConsulta
            pagos.Add(ConvertirAEntidadPagos(pago))
        Next
        Return pagos
    End Function
End Class
