Imports System.Configuration
Imports AutoMapper
Imports UGPP.CobrosCoactivo.Entidades
Public Class MaestroTitulosDocumentosDAL
    Inherits AccesObject(Of Entidades.MaestroTitulosDocumentos)

    Public Property ConnectionString As String

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria


    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    ''' <summary>
    ''' Convierte un objeto del tipo Entidades.MaestroTitulosDocumentos a Datos.MAESTRO_TITULOS_DOCUMENTOS
    ''' </summary>
    ''' <param name="prmObjMaestroTitulosDocumentosEntidad">Objeto de tipo Entidades.MaestroTitulosDocumentos</param>
    ''' <returns>Objeto de tipo Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function ConvertirADatosMaestroTitulosDocumentos(ByVal prmObjMaestroTitulosDocumentosEntidad As Entidades.MaestroTitulosDocumentos) As Datos.MAESTRO_TITULOS_DOCUMENTOS
        Dim maestroTitulosDocumentos As Datos.MAESTRO_TITULOS_DOCUMENTOS
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Datos.MAESTRO_TITULOS_DOCUMENTOS, Entidades.MaestroTitulosDocumentos)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        maestroTitulosDocumentos = IMapper.Map(Of Entidades.MaestroTitulosDocumentos, Datos.MAESTRO_TITULOS_DOCUMENTOS)(prmObjMaestroTitulosDocumentosEntidad)
        Return maestroTitulosDocumentos
    End Function

    ''' <summary>
    ''' Crear un documento relacionado con un título por id
    ''' </summary>
    ''' <param name="prmObjMaestroTitulosDocumentosEntidad">Objeto Entidades.MaestroTitulosDocumentos</param>
    ''' <returns>Objeto Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function crearMaestroTitulosDocumentos(ByVal prmObjMaestroTitulosDocumentosEntidad As Entidades.MaestroTitulosDocumentos) As Datos.MAESTRO_TITULOS_DOCUMENTOS
        Dim maestroTitulosDocumentos = ConvertirADatosMaestroTitulosDocumentos(prmObjMaestroTitulosDocumentosEntidad)
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        db.MAESTRO_TITULOS_DOCUMENTOS.Add(maestroTitulosDocumentos)
        Parameters.Add(New SqlClient.SqlParameter("@ID_MAESTRO_TITULOS_DOCUMENTOS", maestroTitulosDocumentos.ID_MAESTRO_TITULOS_DOCUMENTOS))
        Parameters.Add(New SqlClient.SqlParameter("@ID_DOCUMENTO_TITULO", maestroTitulosDocumentos.ID_DOCUMENTO_TITULO))
        Parameters.Add(New SqlClient.SqlParameter("@ID_MAESTRO_TITULO", maestroTitulosDocumentos.ID_MAESTRO_TITULO))
        Parameters.Add(New SqlClient.SqlParameter("@DES_RUTA_DOCUMENTO", maestroTitulosDocumentos.DES_RUTA_DOCUMENTO))
        Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUTA", maestroTitulosDocumentos.TIPO_RUTA))
        Parameters.Add(New SqlClient.SqlParameter("@COD_TIPO_DOCUMENTO_AO", maestroTitulosDocumentos.COD_TIPO_DOCUMENTO_AO))
        Parameters.Add(New SqlClient.SqlParameter("@NOM_DOC_AO", maestroTitulosDocumentos.NOM_DOC_AO))
        Parameters.Add(New SqlClient.SqlParameter("@OBSERVA_LEGIBILIDAD", maestroTitulosDocumentos.OBSERVA_LEGIBILIDAD))
        Parameters.Add(New SqlClient.SqlParameter("@NUM_PAGINAS", maestroTitulosDocumentos.NUM_PAGINAS))
        Parameters.Add(New SqlClient.SqlParameter("@IND_DOC_SINCRONIZADO", maestroTitulosDocumentos.IND_DOC_SINCRONIZADO))
        Utils.ValidaLog(_auditLog, "INSERT MaestroTitulosDocumentos", Parameters.ToArray)
        Utils.salvarDatos(db)
        Return maestroTitulosDocumentos
    End Function

    ''' <summary>
    ''' Actualiza un documento relacionado con un título por id
    ''' </summary>
    ''' <param name="prmObjMaestroTitulosDocumentosEntidad">Objeto Entidades.MaestroTitulosDocumentos</param>
    ''' <returns>Objeto Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function ActualizarMaestroTitulosDocumentos(ByVal prmObjMaestroTitulosDocumentosEntidad As Entidades.MaestroTitulosDocumentos) As Datos.MAESTRO_TITULOS_DOCUMENTOS
        'Dim maestroTitulosDocumentos = ConvertirADatosMaestroTitulosDocumentos(prmObjMaestroTitulosDocumentosEntidad)
        Dim maestroTitulosDocumentosConsulta = obtenerMaestroTitulosDocumentosPorId(prmObjMaestroTitulosDocumentosEntidad.ID_MAESTRO_TITULOS_DOCUMENTOS)
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_DOCUMENTO_TITULO", prmObjMaestroTitulosDocumentosEntidad.ID_DOCUMENTO_TITULO))
        Parameters.Add(New SqlClient.SqlParameter("@ID_MAESTRO_TITULO", prmObjMaestroTitulosDocumentosEntidad.ID_MAESTRO_TITULO))
        Parameters.Add(New SqlClient.SqlParameter("@DES_RUTA_DOCUMENTO", prmObjMaestroTitulosDocumentosEntidad.DES_RUTA_DOCUMENTO))
        Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUTA", prmObjMaestroTitulosDocumentosEntidad.TIPO_RUTA))
        Parameters.Add(New SqlClient.SqlParameter("@COD_GUID", prmObjMaestroTitulosDocumentosEntidad.COD_GUID))
        Parameters.Add(New SqlClient.SqlParameter("@COD_TIPO_DOCUMENTO_AO", prmObjMaestroTitulosDocumentosEntidad.COD_TIPO_DOCUMENTO_AO))
        Parameters.Add(New SqlClient.SqlParameter("@NOM_DOC_AO", prmObjMaestroTitulosDocumentosEntidad.NOM_DOC_AO))
        Parameters.Add(New SqlClient.SqlParameter("@OBSERVA_LEGIBILIDAD", prmObjMaestroTitulosDocumentosEntidad.OBSERVA_LEGIBILIDAD))
        Parameters.Add(New SqlClient.SqlParameter("@NUM_PAGINAS", prmObjMaestroTitulosDocumentosEntidad.NUM_PAGINAS))
        Parameters.Add(New SqlClient.SqlParameter("@IND_DOC_SINCRONIZADO", prmObjMaestroTitulosDocumentosEntidad.IND_DOC_SINCRONIZADO))
        maestroTitulosDocumentosConsulta.ID_DOCUMENTO_TITULO = prmObjMaestroTitulosDocumentosEntidad.ID_DOCUMENTO_TITULO
        maestroTitulosDocumentosConsulta.ID_MAESTRO_TITULO = prmObjMaestroTitulosDocumentosEntidad.ID_MAESTRO_TITULO
        maestroTitulosDocumentosConsulta.DES_RUTA_DOCUMENTO = prmObjMaestroTitulosDocumentosEntidad.DES_RUTA_DOCUMENTO
        maestroTitulosDocumentosConsulta.TIPO_RUTA = prmObjMaestroTitulosDocumentosEntidad.TIPO_RUTA
        maestroTitulosDocumentosConsulta.COD_GUID = prmObjMaestroTitulosDocumentosEntidad.COD_GUID
        maestroTitulosDocumentosConsulta.COD_TIPO_DOCUMENTO_AO = prmObjMaestroTitulosDocumentosEntidad.COD_TIPO_DOCUMENTO_AO
        maestroTitulosDocumentosConsulta.NOM_DOC_AO = prmObjMaestroTitulosDocumentosEntidad.NOM_DOC_AO
        maestroTitulosDocumentosConsulta.OBSERVA_LEGIBILIDAD = prmObjMaestroTitulosDocumentosEntidad.OBSERVA_LEGIBILIDAD
        maestroTitulosDocumentosConsulta.NUM_PAGINAS = prmObjMaestroTitulosDocumentosEntidad.NUM_PAGINAS
        maestroTitulosDocumentosConsulta.IND_DOC_SINCRONIZADO = prmObjMaestroTitulosDocumentosEntidad.IND_DOC_SINCRONIZADO
        Utils.ValidaLog(_auditLog, "Update MaestroTitulosDocumentos", Parameters.ToArray)
        Utils.salvarDatos(db)
        Return maestroTitulosDocumentosConsulta
    End Function

    ''' <summary>
    ''' Obtiene un documento relacionado con un título por id
    ''' </summary>
    ''' <param name="prmIntMaestroTitulosDocumentosId">Identificador del documento relacionado con el títulos</param>
    ''' <returns>Objeto Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function obtenerMaestroTitulosDocumentosPorId(ByVal prmIntMaestroTitulosDocumentosId As Integer) As Datos.MAESTRO_TITULOS_DOCUMENTOS
        Dim maestroTitulosDocumentos = db.MAESTRO_TITULOS_DOCUMENTOS.Where(Function(mtd) mtd.ID_MAESTRO_TITULOS_DOCUMENTOS = prmIntMaestroTitulosDocumentosId).FirstOrDefault()
        Return maestroTitulosDocumentos
    End Function

    ''' <summary>
    ''' Obtener los documentos que están relacionados con un título
    ''' </summary>
    ''' <param name="prmIntTituloId">Id único del título</param>
    ''' <returns>Lista de objetos del tipo Datos.MAESTRO_TITULOS_DOCUMENTOS</returns>
    Public Function obtenerDocumentosPorTitulo(ByVal prmIntTituloId As Int32) As List(Of Datos.MAESTRO_TITULOS_DOCUMENTOS)
        Dim documentos = (From dmt In db.MAESTRO_TITULOS_DOCUMENTOS
                          Where dmt.ID_MAESTRO_TITULO = prmIntTituloId
                          Select dmt).ToList()
        Return documentos
    End Function

End Class
