Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos

Public Class TiposCausalReasignacionBLL

    ''' <summary>
    ''' Objeto para llamar métodos de consulta a la base de datos
    ''' </summary>
    ''' <returns></returns>
    Private Property _tiposCausalesReasignacionDAL As TiposCausalesReasignacionDAL

    Public Sub New()
        _tiposCausalesReasignacionDAL = New TiposCausalesReasignacionDAL()
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TIPOS_CAUSALES_PRIORIZACION a Entidades.TiposCausalesPriorizacion
    ''' </summary>
    ''' <param name="prmObjTiposCausalesReasignacionDatos">Objeto de tipo Datos.TIPOS_CAUSALES_PRIORIZACION</param>
    ''' <returns>Objeto de tipo Entidades.TiposCausalesPriorizacion</returns>
    Public Function ConvertirEntidadCausalesReasignacion(ByVal prmObjTiposCausalesReasignacionDatos As Datos.TIPOS_CAUSALES_REASIGNACION) As Entidades.TiposCausalesReasignacion
        Dim cuasalReasignacion As Entidades.TiposCausalesReasignacion
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TiposCausalesReasignacion, Datos.TIPOS_CAUSALES_REASIGNACION)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        cuasalReasignacion = IMapper.Map(Of Datos.TIPOS_CAUSALES_REASIGNACION, Entidades.TiposCausalesReasignacion)(prmObjTiposCausalesReasignacionDatos)
        Return cuasalReasignacion
    End Function

    ''' <summary>
    ''' Obtiene la lista de causales de reasignación activos
    ''' </summary>
    ''' <returns>Lista de Objetos Entidades.TiposCausalesReasignacion</returns>
    Public Function obtenerCausalesReasignacionActivos() As List(Of Entidades.TiposCausalesReasignacion)
        Dim causalesReasignacionConsulta = _tiposCausalesReasignacionDAL.obtenerCausalesReasignacionActivos()
        Dim causalesReasignacion As List(Of Entidades.TiposCausalesReasignacion) = New List(Of Entidades.TiposCausalesReasignacion)
        For Each causalReasignacion As Datos.TIPOS_CAUSALES_REASIGNACION In causalesReasignacionConsulta
            causalesReasignacion.Add(ConvertirEntidadCausalesReasignacion(causalReasignacion))
        Next
        Return causalesReasignacion
    End Function
End Class
