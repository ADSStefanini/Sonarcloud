Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class MaestroTitulosDAL
    Inherits AccesObject(Of MaestroTitulos)

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
    ''' Consulta los datos de un título por su idunico
    ''' </summary>
    ''' <param name="prmIntIdTitulo">Id único del título que se consulta</param>
    ''' <returns>Objeto del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function consultarTituloPorID(ByVal prmIntIdTitulo As Int32) As Datos.MAESTRO_TITULOS
        Dim titulo = (From mt In db.MAESTRO_TITULOS
                      Where mt.idunico = prmIntIdTitulo
                      Select mt).FirstOrDefault()
        Return titulo
    End Function

    ''' <summary>
    ''' Consulta los datos de los títulos relacionados con la lista de IDs únicos del parámetro
    ''' </summary>
    ''' <param name="prmListIdsTitulos">Lista de IDs únicos a consultar</param>
    ''' <returns>Lista de objetos del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function consultarTituloPorIDs(ByVal prmListIdsTitulos As List(Of Integer)) As List(Of Datos.MAESTRO_TITULOS)
        Dim titulos = (From mt In db.MAESTRO_TITULOS
                       Where prmListIdsTitulos.Contains(mt.idunico)
                       Select mt).ToList()
        Return titulos
    End Function

    ''' <summary>
    ''' Obtener el título mas proximo a prescribir de la lista de títulos entregada
    ''' </summary>
    ''' <param name="prmListIdsTitulos">Lista de IDs únicos a consultar</param>
    ''' <returns>Objeto del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function obtenerTituloMasCercanoPrescripcion(ByVal prmListIdsTitulos As List(Of Integer)) As Datos.MAESTRO_TITULOS
        Dim titulo = (From mt In db.MAESTRO_TITULOS
                      Where prmListIdsTitulos.Contains(mt.idunico)
                      Order By mt.MT_fec_cad_presc Descending
                      Select mt).FirstOrDefault()
        Return titulo
    End Function

    ''' <summary>
    ''' Obtener el título mas proximo a prescribir del expediente
    ''' </summary>
    ''' <param name="prmIntIdExpediente">ID del expediente de Cobros y Coactivos</param>
    ''' <returns>Objeto del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function obtenerTituloMasCercanoPrescripcionPorExpedienteId(ByVal prmIntIdExpediente As String) As Datos.MAESTRO_TITULOS
        Dim titulo = (From mt In db.MAESTRO_TITULOS
                      Where mt.MT_expediente = prmIntIdExpediente
                      Order By mt.MT_fec_cad_presc Descending
                      Select mt).FirstOrDefault()
        Return titulo
    End Function
    ''' <summary>
    ''' Aginar el expediente al que pertenece el título
    ''' </summary>
    ''' <param name="prmIntTituloId">Id único del título</param>
    ''' <param name="prmIntExpedienteId">Id único del expediente</param>
    ''' <returns>Objeto dle tipo Datos.MAESTRO_TITULOS</returns>
    Public Function asignarExpedienteATitulo(ByVal prmIntTituloId As Int32, ByVal prmIntExpedienteId As Int32) As Datos.MAESTRO_TITULOS
        Dim parametros As SqlClient.SqlParameter() = New SqlClient.SqlParameter() {
            New SqlClient.SqlParameter("@MT_nro_titulo", prmIntTituloId.ToString()),
            New SqlClient.SqlParameter("@MT_expediente", prmIntExpedienteId.ToString())
        }
        ExecuteCommand("SP_ASIGNAR_EXPEDIENTE_A_TITULO", parametros)
        Utils.ValidaLog(_auditLog, "EXECUTE SP_ASIGNAR_EXPEDIENTE_A_TITULO", parametros)

        Dim titulo = consultarTituloPorID(prmIntTituloId)
        Return titulo
    End Function

    ''' <summary>
    ''' Consulta El tipo de titulo para clasificación por cuantia
    ''' </summary>
    ''' <param name="idExpediente">Id unico a consultar</param>
    ''' <returns>Lista de objetos del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function consultarTipoTituloPoridExpediente(ByVal idExpediente As String) As List(Of Datos.MAESTRO_TITULOS)
        Dim titulos = (From mt In db.MAESTRO_TITULOS
                       Where mt.MT_expediente = idExpediente
                       Select mt).ToList()
        Return titulos
    End Function

    ''' <summary>
    ''' Obtiene un título por el número de expediente relacionado a este
    ''' </summary>
    ''' <param name="prmStrNroExp">ID del expediente</param>
    ''' <returns>Objeto del tipo Datos.MAESTRO_TITULOS</returns>
    Public Function obtenerTituloPorExpedienteId(ByVal prmStrNroExp As String) As Datos.MAESTRO_TITULOS
        Dim titulo = (From mt In db.MAESTRO_TITULOS
                      Where mt.MT_expediente = prmStrNroExp
                      Select mt).FirstOrDefault()
        Return titulo
    End Function
End Class
