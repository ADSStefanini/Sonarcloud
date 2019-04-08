Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class ExpedienteBLL

    ''' <summary>
    ''' Clase de comunicaión para la conexión a la DB
    ''' </summary>
    Dim _expedienteDAL As ExpedienteDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _expedienteDAL = New ExpedienteDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _expedienteDAL = New ExpedienteDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.TIPOS_CAUSALES_PRIORIZACION a Entidades.TiposCausalesPriorizacion
    ''' </summary>
    ''' <param name="prmObjExpedienteDatos">Objeto de tipo Datos.TIPOS_CAUSALES_PRIORIZACION</param>
    ''' <returns>Objeto de tipo Entidades.TiposCausalesPriorizacion</returns>
    Public Function ConvertirAEntidadExpediente(ByVal prmObjExpedienteDatos As Datos.EJEFISGLOBAL) As Entidades.EJEFISGLOBAL
        Dim expediente As Entidades.EJEFISGLOBAL
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.EJEFISGLOBAL, Datos.EJEFISGLOBAL)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        expediente = IMapper.Map(Of Datos.EJEFISGLOBAL, Entidades.EJEFISGLOBAL)(prmObjExpedienteDatos)
        Return expediente
    End Function

    ''' <summary>
    ''' Método que retorna el último id insertado de la tabla de expedientes
    ''' </summary>
    ''' <returns>Número máximo del campo EFINROEXP</returns>
    Public Function obtenerUltimoExpediente() As Int32
        Return _expedienteDAL.obtenerUltimoExpediente()
    End Function

    ''' <summary>
    ''' Crea un expediente a partir de la entidad de negocio Entidades.EJEFISGLOBAL
    ''' </summary>
    ''' <param name="prmObjExpedienteEntidad">Objeto del tipo Entidades.EJEFISGLOBAL</param>
    ''' <returns>Objeto del tipo Entidades.EJEFISGLOBAL</returns>
    Public Function crearExpediente(ByVal prmObjExpedienteEntidad As Entidades.EJEFISGLOBAL) As Entidades.EJEFISGLOBAL
        Return ConvertirAEntidadExpediente(_expedienteDAL.crearExpediente(prmObjExpedienteEntidad))
    End Function

    ''' <summary>
    ''' Retorna un expediente por su número ID
    ''' </summary>
    ''' <param name="prmStrExpedienteId">Número único del expediente de cobros</param>
    ''' <returns>Objeto del tipo Entidades.EJEFISGLOBAL</returns>
    Public Function obtenerExpedientePorId(ByVal prmStrExpedienteId As String) As Entidades.EJEFISGLOBAL
        Dim expedienteConsulta = _expedienteDAL.obtenerExpedientePorId(prmStrExpedienteId)
        If IsNothing(expedienteConsulta) Then
            Return Nothing
        End If
        Return ConvertirAEntidadExpediente(expedienteConsulta)
    End Function

    ''' <summary>
    ''' Actualiza el estado operatio del expediente que se creo  a Por repartir
    ''' </summary>
    ''' <param name="prmIntExpedienteId">Id del expediente</param>
    Public Sub asignarExpedientePorRepartir(ByVal prmIntExpedienteId As String)
        'Se crea el objeto de inserción
        Dim objTareaAsigana As New Entidades.TareaAsignada
        objTareaAsigana.VAL_USUARIO_NOMBRE = String.Empty 'Se deja vacio para que no presente error
        objTareaAsigana.COD_TIPO_OBJ = Entidades.Enumeraciones.DominioDetalle.Expediente
        objTareaAsigana.EFINROEXP_EXPEDIENTE = prmIntExpedienteId.ToString()
        objTareaAsigana.COD_ESTADO_OPERATIVO = 10 'Por repartir

        Dim tareaAsigada As New TareaAsignadaBLL
        Try
            tareaAsigada.registrarTarea(objTareaAsigana)
        Catch ex As Exception
            'TODO: Llamar LOG errores
        End Try
    End Sub

    ''' <summary>
    ''' Consulta que retorna los expedientes asignados a un gestor (o expedientes asignados a los gestores relacionados con un cordinador o superior) con la páginación y filtros
    ''' </summary>
    ''' <param name="prmObjFiltroExpedientes">Objeto donde se encapsula los datos necesarios para ejecutar el SP SP_OBTENER_EXPEDIENTES_ASIGNADOS</param>
    ''' <returns>Objeto del tipo DataTable con el resultado de ejecutar el SP SP_OBTENER_EXPEDIENTES_ASIGNADOS</returns>
    Public Function obtenerExpedientesAsignados(ByVal prmObjFiltroExpedientes As Entidades.ConsultaExpedientes, Optional prmObjDataTable As DataTable = Nothing) As DataTable
        Return _expedienteDAL.obtenerExpedientesAsignados(prmObjFiltroExpedientes, prmObjDataTable)
    End Function

End Class
