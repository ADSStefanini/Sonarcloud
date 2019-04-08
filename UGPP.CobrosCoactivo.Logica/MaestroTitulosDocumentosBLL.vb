Imports AutoMapper
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class MaestroTitulosDocumentosBLL

    Private Property _maestroTitulosDocumentosDAL As MaestroTitulosDocumentosDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _maestroTitulosDocumentosDAL = New MaestroTitulosDocumentosDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _maestroTitulosDocumentosDAL = New MaestroTitulosDocumentosDAL()
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Datos.MAESTRO_TITULOS_DOCUMENTOS a Entidades.MaestroTitulosDocumentos
    ''' </summary>
    ''' <param name="prmObjMaestroTitulosDocumentosDatos">Objeto de tipo Datos.MAESTRO_TITULOS_DOCUMENTOS</param>
    ''' <returns>Objeto de tipo Entidades.MaestroTitulosDocumentos</returns>
    Public Function ConvertirAEntidadMaestroTitulosDocumentos(ByVal prmObjMaestroTitulosDocumentosDatos As Datos.MAESTRO_TITULOS_DOCUMENTOS) As Entidades.MaestroTitulosDocumentos
        Dim maestroTitulosDocumentos As Entidades.MaestroTitulosDocumentos
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Entidades.MaestroTitulosDocumentos, Datos.MAESTRO_TITULOS_DOCUMENTOS)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        maestroTitulosDocumentos = IMapper.Map(Of Datos.MAESTRO_TITULOS_DOCUMENTOS, Entidades.MaestroTitulosDocumentos)(prmObjMaestroTitulosDocumentosDatos)
        Return maestroTitulosDocumentos
    End Function

    ''' <summary>
    ''' Crear un documento relacionado con un título por id
    ''' </summary>
    ''' <param name="prmObjMaestroTitulosDocumentosEntidad">Objeto Entidades.MaestroTitulosDocumentos</param>
    ''' <returns>Objeto Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function crearMaestroTitulosDocumentos(ByVal prmObjMaestroTitulosDocumentosEntidad) As Entidades.MaestroTitulosDocumentos
        Dim maestroTitulosDocumentosConsulta = _maestroTitulosDocumentosDAL.crearMaestroTitulosDocumentos(prmObjMaestroTitulosDocumentosEntidad)
        If IsNothing(maestroTitulosDocumentosConsulta) Then
            Return New Entidades.MaestroTitulosDocumentos
        End If
        Return ConvertirAEntidadMaestroTitulosDocumentos(maestroTitulosDocumentosConsulta)
    End Function

    ''' <summary>
    ''' Actualiza un documento relacionado con un título por id
    ''' </summary>
    ''' <param name="prmObjMaestroTitulosDocumentosEntidad">Objeto Entidades.MaestroTitulosDocumentos</param>
    ''' <returns>Objeto Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function ActualizarMaestroTitulosDocumentos(ByVal prmObjMaestroTitulosDocumentosEntidad As Entidades.MaestroTitulosDocumentos) As Entidades.MaestroTitulosDocumentos
        Dim maestroTitulosDocumentosConsulta = _maestroTitulosDocumentosDAL.ActualizarMaestroTitulosDocumentos(prmObjMaestroTitulosDocumentosEntidad)
        If IsNothing(maestroTitulosDocumentosConsulta) Then
            Return New Entidades.MaestroTitulosDocumentos
        End If
        Return ConvertirAEntidadMaestroTitulosDocumentos(maestroTitulosDocumentosConsulta)
    End Function

    ''' <summary>
    ''' Obtiene un documento relacionado con un título por id
    ''' </summary>
    ''' <param name="prmIntMaestroTitulosDocumentosId">Identificador del documento relacionado con el títulos</param>
    ''' <returns>Objeto Entidades.MaestroTitulosDocumentos</returns>
    Public Function obtenerMaestroTitulosDocumentosPorId(ByVal prmIntMaestroTitulosDocumentosId As Integer) As Entidades.MaestroTitulosDocumentos
        Dim maestroTitulosDocumentosConsulta = _maestroTitulosDocumentosDAL.obtenerMaestroTitulosDocumentosPorId(prmIntMaestroTitulosDocumentosId)
        If IsNothing(maestroTitulosDocumentosConsulta) Then
            Return New Entidades.MaestroTitulosDocumentos
        End If
        Return ConvertirAEntidadMaestroTitulosDocumentos(maestroTitulosDocumentosConsulta)
    End Function

    ''' <summary>
    ''' Obtener los documentos que están relacionados con un título
    ''' </summary>
    ''' <param name="prmIntTituloId">Id único del título</param>
    ''' <returns>Lista de objetos del tipo Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function obtenerDocumentosPorTitulo(ByVal prmIntTituloId As Int32) As List(Of Entidades.MaestroTitulosDocumentos)

        Try
            Dim maestroTitulosDocumentos As New List(Of Entidades.MaestroTitulosDocumentos)
            Dim maestroTitulosDocumentosConsulta = _maestroTitulosDocumentosDAL.obtenerDocumentosPorTitulo(prmIntTituloId)
            For Each maestroTituloDocumento As Datos.MAESTRO_TITULOS_DOCUMENTOS In maestroTitulosDocumentosConsulta
                maestroTitulosDocumentos.Add(ConvertirAEntidadMaestroTitulosDocumentos(maestroTituloDocumento))
            Next
            Return maestroTitulosDocumentos
        Catch ex As Exception
            'Error, título sin documentos
            Return New List(Of Entidades.MaestroTitulosDocumentos)
        End Try

    End Function
End Class
