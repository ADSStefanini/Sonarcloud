Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades
''' <summary>
''' Clase de negocio para el estado de proceso gestor
''' </summary>
Public Class EstadoProcesoGestorBLL
    Public Function Guardar(ByVal estadoProcesoGestor As EstadoProcesoGestor) As Boolean
        Dim estadosProceso As Datos.ESTADOS_PROCESO_GESTOR
        Dim dataAccess As New Datos.EstadoProcesoGestorDAL
        Dim resultado As Boolean = False
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.EstadoProcesoGestor, Datos.ESTADOS_PROCESO_GESTOR)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        estadosProceso = IMapper.Map(Of Entidades.EstadoProcesoGestor, Datos.ESTADOS_PROCESO_GESTOR)(estadoProcesoGestor)

        If dataAccess.ObtenerGestoresyEstadosPorLlaves(estadoProcesoGestor.VAL_USUARIO, estadoProcesoGestor.COD_ID_ESTADOS_PROCESOS) IsNot Nothing Then
            resultado = New EstadoProcesoGestorDAL().Actualizar(estadosProceso)
        Else
            resultado = dataAccess.Guardar(estadosProceso)
        End If
        Return resultado
    End Function
    ''' <summary>
    ''' Obtiene los elemantos 
    ''' </summary>
    ''' <returns></returns>
    Public Function ObtenerEstadoProcesoGestor() As List(Of EstadoProcesoGestor)
        Dim estadosProceso As List(Of EstadoProcesoGestor)
        Dim estadosProcesoData As List(Of Datos.ESTADOS_PROCESO_GESTOR)
        Dim dataAccess As New Datos.EstadoProcesoGestorDAL
        Dim dataEstados As New Datos.EstadosProcesoDAL
        estadosProcesoData = dataAccess.ObtenerGestoresyEstados()
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Datos.ESTADOS_PROCESO_GESTOR, Entidades.EstadoProcesoGestor)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        estadosProceso = IMapper.Map(Of List(Of Datos.ESTADOS_PROCESO_GESTOR), List(Of Entidades.EstadoProcesoGestor))(estadosProcesoData)
        For Each item In estadosProceso
            item.NOMBRE_ESTADOS_PROCESOS = dataEstados.obtenerEstadosProcesos().FirstOrDefault(Function(x) x.codigo = item.COD_ID_ESTADOS_PROCESOS).nombre
        Next
        Return estadosProceso
    End Function
End Class
