Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos

Public Class UvtBLL
    Private Property _valAnioUvt As UvtDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _valAnioUvt = New Datos.UvtDAL
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Datos.UvtDAL a Entidades.UVT
    ''' </summary>
    ''' <param name="prmObjUvt">Objeto de tipo Datos.UVT</param>
    ''' <returns>Objeto de tipo Entidades.UVT</returns>
    Public Function ConvertirValoresUvt(ByVal prmObjUvt As Datos.UVT) As Entidades.UVT
        Dim valAnioActual As Entidades.UVT
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.UVT, Datos.UVT)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        valAnioActual = IMapper.Map(Of Datos.UVT, Entidades.UVT)(prmObjUvt)
        Return valAnioActual
    End Function

    ''' <summary>
    ''' Obtener valorAnioActual
    ''' </summary>
    ''' <returns>ValorAnioActual</returns>
    Public Function ObtenerValAnioActual() As List(Of Entidades.UVT)
        Dim valorAnioActualUvt = _valAnioUvt.ObtenerUvtAnioActual()
        Dim listaValorUvt As List(Of Entidades.UVT) = New List(Of Entidades.UVT)
        For Each listaValoresUvt As Datos.UVT In valorAnioActualUvt
            listaValorUvt.Add(ConvertirValoresUvt(listaValoresUvt))
        Next
        Return listaValorUvt
    End Function
End Class
