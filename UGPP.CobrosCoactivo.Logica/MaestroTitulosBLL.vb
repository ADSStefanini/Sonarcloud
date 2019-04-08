Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos
Imports AutoMapper

Public Class MaestroTitulosBLL

    Private Property _maestroTitulosDAL As MaestroTitulosDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _maestroTitulosDAL = New MaestroTitulosDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _maestroTitulosDAL = New MaestroTitulosDAL()
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.ESTADO_OPERATIVO a Entidades.EstadoOperativo
    ''' </summary>
    ''' <param name="prmObjMaestroTituloDatos">Objeto de tipo Datos.ESTADO_OPERATIVO</param>
    ''' <returns>Objeto de tipo Entidades.EstadoOperativo</returns>
    Private Function ConvertirAEntidadMaestroTitulo(ByVal prmObjMaestroTituloDatos As Datos.MAESTRO_TITULOS) As Entidades.MaestroTitulos
        Dim maestroTitulo As Entidades.MaestroTitulos
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.MaestroTitulos, Datos.MAESTRO_TITULOS)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        maestroTitulo = IMapper.Map(Of Datos.MAESTRO_TITULOS, Entidades.MaestroTitulos)(prmObjMaestroTituloDatos)
        Return maestroTitulo
    End Function

    ''' <summary>
    ''' Consulta los datos de un título por su idunico
    ''' </summary>
    ''' <param name="prmIntIdTitulo">Id único del título que se consulta</param>
    ''' <returns>Objeto del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function consultarTituloPorID(ByVal prmIntIdTitulo As Int32) As Entidades.MaestroTitulos
        Return ConvertirAEntidadMaestroTitulo(_maestroTitulosDAL.consultarTituloPorID(prmIntIdTitulo))
    End Function

    ''' <summary>
    ''' Consulta el tipo de titulo 
    ''' </summary>
    ''' <param name="idExpediente">id expediente para consulta tipo titulo</param>
    ''' <returns>Lista de objetos del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function consultarTipoTitulo(ByVal idExpediente As String) As List(Of Entidades.MaestroTitulos)
        Dim resultadoConsulta = _maestroTitulosDAL.consultarTipoTituloPoridExpediente(idExpediente)
        Dim matestroTitulos As New List(Of Entidades.MaestroTitulos)
        For Each maestroTitulo As Datos.MAESTRO_TITULOS In resultadoConsulta
            matestroTitulos.Add(ConvertirAEntidadMaestroTitulo(maestroTitulo))
        Next
        Return matestroTitulos
    End Function

    ''' <summary>
    ''' Obtener el título mas proximo a prescribir de la lista de títulos entregada
    ''' </summary>
    ''' <param name="prmListIdsTitulos">Lista de IDs únicos a consultar</param>
    ''' <returns>Objeto del tipo Entidades.MaestroTitulos</returns>
    Public Function obtenerTituloMasCercanoPrescripcion(ByVal prmListIdsTitulos As List(Of Integer)) As Entidades.MaestroTitulos
        Return ConvertirAEntidadMaestroTitulo(_maestroTitulosDAL.obtenerTituloMasCercanoPrescripcion(prmListIdsTitulos))
    End Function

    ''' <summary>
    ''' Obtener el título mas proximo a prescribir del expediente
    ''' </summary>
    ''' <param name="prmIntIdExpediente">ID del expediente de Cobros y Coactivos</param>
    ''' <returns>Objeto del tipo Entidades.MaestroTitulos</returns>
    Public Function obtenerTituloMasCercanoPrescripcionPorExpedienteId(ByVal prmIntIdExpediente As String) As Entidades.MaestroTitulos
        Return ConvertirAEntidadMaestroTitulo(_maestroTitulosDAL.obtenerTituloMasCercanoPrescripcionPorExpedienteId(prmIntIdExpediente))
    End Function

    ''' <summary>
    ''' Aginar el expediente al que pertenece el título
    ''' </summary>
    ''' <param name="prmIntTituloId">Id único del título</param>
    ''' <param name="prmIntExpedienteId">Id único del expediente</param>
    ''' <returns>Objeto dle tipo Datos.MAESTRO_TITULOS</returns>
    Public Function asignarExpedienteATitulo(ByVal prmIntTituloId As Int32, ByVal prmIntExpedienteId As Int32) As Entidades.MaestroTitulos
        Return ConvertirAEntidadMaestroTitulo(_maestroTitulosDAL.asignarExpedienteATitulo(prmIntTituloId, prmIntExpedienteId))
    End Function

    ''' <summary>
    ''' Obtiene un título por el número de expediente relacionado a este
    ''' </summary>
    ''' <param name="prmStrNroExpediente">ID del expediente</param>
    ''' <returns>Objeto del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function obtenerTituloPorExpedienteId(ByVal prmStrNroExpediente As String) As Entidades.MaestroTitulos
        Return ConvertirAEntidadMaestroTitulo(_maestroTitulosDAL.obtenerTituloPorExpedienteId(prmStrNroExpediente))
    End Function

End Class
