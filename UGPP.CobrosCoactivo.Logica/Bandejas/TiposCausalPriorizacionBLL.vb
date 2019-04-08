Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos

Public Class TiposCausalPriorizacionBLL

    ''' <summary>
    ''' Objeto para llamar métodos de consulta a la base de datos
    ''' </summary>
    ''' <returns></returns>
    Private Property _tiposCausalPriorizacionDAL As TiposCausalesPriorizacionDAL

    Public Sub New()
        _tiposCausalPriorizacionDAL = New TiposCausalesPriorizacionDAL()
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TIPOS_CAUSALES_PRIORIZACION a Entidades.TiposCausalesPriorizacion
    ''' </summary>
    ''' <param name="prmObjTiposCausalesPriorizacionDatos">Objeto de tipo Datos.TIPOS_CAUSALES_PRIORIZACION</param>
    ''' <returns>Objeto de tipo Entidades.TiposCausalesPriorizacion</returns>
    Public Function ConvertirEntidadTiposCausalesPriorizacion(ByVal prmObjTiposCausalesPriorizacionDatos As Datos.TIPOS_CAUSALES_PRIORIZACION) As Entidades.TiposCausalesPriorizacion
        Dim cuasalPriorizacin As Entidades.TiposCausalesPriorizacion
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.TiposCausalesPriorizacion, Datos.TIPOS_CAUSALES_PRIORIZACION)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        cuasalPriorizacin = IMapper.Map(Of Datos.TIPOS_CAUSALES_PRIORIZACION, Entidades.TiposCausalesPriorizacion)(prmObjTiposCausalesPriorizacionDatos)
        Return cuasalPriorizacin
    End Function

    ''' <summary>
    ''' Obtiene la lista de causales de reasignación activos
    ''' </summary>
    ''' <returns>Lista de Objetos Entidades.TiposCausalesReasignacion</returns>
    Public Function obtenerCausalesPriorizacionActivos() As List(Of Entidades.TiposCausalesPriorizacion)
        Dim causalesPriorizacionConsulta = _tiposCausalPriorizacionDAL.obtenerCausalesPriorizacionnActivos()
        Dim causalesPriorizacion As List(Of Entidades.TiposCausalesPriorizacion) = New List(Of Entidades.TiposCausalesPriorizacion)
        For Each causalReasignacion As Datos.TIPOS_CAUSALES_PRIORIZACION In causalesPriorizacionConsulta
            causalesPriorizacion.Add(ConvertirEntidadTiposCausalesPriorizacion(causalReasignacion))
        Next
        Return causalesPriorizacion
    End Function
End Class
