Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Public Class TiposProcesosConcursalesBLL
    Private Property _tiposProcesosConcursales As TiposProcesosConcursalesDAL
    Private Property _AuditEntity As Entidades.LogAuditoria
    Public Sub New()
        _tiposProcesosConcursales = New Datos.TiposProcesosConcursalesDAL
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TiposProcesosConcursalesDAL a Entidades.TipoProcesoConcursal
    ''' </summary>
    ''' <param name="prmObjTipoProcesoConcursal">Objeto de tipo Datos.TIPOS_PROCESOS_CONCURSALES</param>
    ''' <returns>Objeto de tipo Entidades.TiposProcesosConcursales</returns>
    Public Function ConvertirTiposProcesosConcursales(ByVal prmObjTipoProcesoConcursal As Datos.TIPOS_PROCESOS_CONCURSALES) As Entidades.TiposProcesosConcursales
        Dim tipoProcesoConcursal As Entidades.TiposProcesosConcursales
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TiposProcesosConcursales, Datos.TIPOS_PROCESOS_CONCURSALES)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        tipoProcesoConcursal = IMapper.Map(Of Datos.TIPOS_PROCESOS_CONCURSALES, Entidades.TiposProcesosConcursales)(prmObjTipoProcesoConcursal)
        Return tipoProcesoConcursal
    End Function

    ''' <summary>
    ''' Obtener todas las etapas procesales
    ''' </summary>
    ''' <returns>Lista de etapas procesales</returns>
    Public Function ObtenerTipoProcesoJuridica() As List(Of Entidades.TiposProcesosConcursales)
        Dim listaTipoProcesoConcursalConsulta = _tiposProcesosConcursales.ObtenerTiposProcesosJuridico()
        Dim listaTipoProcesoConcursal As List(Of Entidades.TiposProcesosConcursales) = New List(Of Entidades.TiposProcesosConcursales)
        For Each listaProcesosConcursales As Datos.TIPOS_PROCESOS_CONCURSALES In listaTipoProcesoConcursalConsulta
            listaTipoProcesoConcursal.Add(ConvertirTiposProcesosConcursales(listaProcesosConcursales))
        Next
        Return listaTipoProcesoConcursal
    End Function

    Public Function ObtenerTipoProcesoNatural() As List(Of Entidades.TiposProcesosConcursales)
        Dim listaTipoProcesoConcursalNaturalConsulta = _tiposProcesosConcursales.ObtenerTiposProcesosNatural()
        Dim listaTipoProcesoConcursalNatural As List(Of Entidades.TiposProcesosConcursales) = New List(Of Entidades.TiposProcesosConcursales)
        For Each listaProcesosConcursalesNatural As Datos.TIPOS_PROCESOS_CONCURSALES In listaTipoProcesoConcursalNaturalConsulta
            listaTipoProcesoConcursalNatural.Add(ConvertirTiposProcesosConcursales(listaProcesosConcursalesNatural))
        Next
        Return listaTipoProcesoConcursalNatural
    End Function
End Class
