Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class EjefisglobalBLL

    Private Property _ejefisglobal As EjefisglobalDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _ejefisglobal = New EjefisglobalDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _ejefisglobal = New EjefisglobalDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.EJEFISGLOBAL a Entidades.EJEFISGLOBAL
    ''' </summary>
    ''' <param name="prmObjExpediente">Objeto de tipo Datos.EJEFISGLOBAL</param>
    ''' <returns>Objeto de tipo Entidades.EJEFISGLOBAL</returns>
    Private Function ConvertirAEntidadEjefisglobal(ByVal prmObjExpediente As Datos.EJEFISGLOBAL) As Entidades.EJEFISGLOBAL
        Dim expediente As Entidades.EJEFISGLOBAL
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.EJEFISGLOBAL, Datos.EJEFISGLOBAL)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        expediente = IMapper.Map(Of Datos.EJEFISGLOBAL, Entidades.EJEFISGLOBAL)(prmObjExpediente)
        Return expediente
    End Function


    ''' <summary>
    ''' Consulta Expediente 
    ''' </summary>
    ''' <param name="idExpediente">id expediente para consultar expediente</param>
    ''' <returns>Lista de objetos del tipo Datos.EJEFISGLOBAL</returns>
    Public Function consultarExpediente(ByVal idExpediente As String) As List(Of Entidades.EJEFISGLOBAL)
        Dim resultadoConsulta = _ejefisglobal.ObtenerExpedientesPorId(idExpediente)
        Dim expedientes As New List(Of Entidades.EJEFISGLOBAL)
        For Each expediente As Datos.EJEFISGLOBAL In resultadoConsulta
            expedientes.Add(ConvertirAEntidadEjefisglobal(expediente))
        Next
        Return expedientes
    End Function

    ''' <summary>
    ''' Actualizar estado expediente
    ''' </summary>
    ''' <param name="ID">Identificador del expediente</param>
    ''' <param name="idEstado">Estado del Expediente</param>
    ''' <returns>Valor booleano que indica si se actualizo el registro en la base de datos</returns>
    Public Function ActualizarExpediente(ByVal ID As Int32, ByVal idEstado As String)
        Dim expediente = _ejefisglobal.ActualizarExpediente(ID, idEstado)
        Return True
    End Function
    ''' <summary>
    ''' Actualizar etapa procesal expediente
    ''' </summary>
    ''' <param name="ID">Identificador del expediente</param>
    ''' <param name="idEstado">Estado del módulo</param>
    ''' <param name="idEtapaProcesal">Estado Etapa procesal del expediente</param>
    ''' <returns>Valor booleano que indica si se actualizo el registro en la base de datos</returns>
    Public Function ActualizarExpedienteEtapaProcesal(ByVal ID As Int32, ByVal idEstado As String, idEtapaProcesal As Int32)
        Dim expediente = _ejefisglobal.ActualizarExpedienteEtapaProcesal(ID, idEstado, idEtapaProcesal)
        Return True
    End Function
End Class
