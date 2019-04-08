Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class EjefisglobalDAL
    Inherits AccesObject(Of UGPP.CobrosCoactivo.Entidades.EJEFISGLOBAL)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _Auditoria As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _Auditoria = auditLog
    End Sub

    ''' <summary>
    ''' Retorna un expediente dependiendo de su identificador
    ''' </summary>
    ''' <param name="ID">Identificador del expediente</param>
    ''' <returns>Objeto del tipo Datos.EJEFISGLOBAL</returns>
    Public Function ObtenerExpedientePorId(ByVal ID As String) As Datos.EJEFISGLOBAL
        Dim expediente = (From m In db.EJEFISGLOBAL
                          Where m.EFINROEXP = ID
                          Select m).FirstOrDefault()
        Return expediente
    End Function

    ''' <summary>
    ''' Retorna expedientes dependiendo de su identificador
    ''' </summary>
    ''' <param name="ID">Identificador del expediente</param>
    ''' <returns>Objeto del tipo Datos.EJEFISGLOBAL</returns>
    Public Function ObtenerExpedientesPorId(ByVal ID As String) As List(Of Datos.EJEFISGLOBAL)
        Dim expediente = (From m In db.EJEFISGLOBAL
                          Where m.EFINROEXP = ID
                          Select m).ToList()
        Return expediente
    End Function

    ''' <summary>
    ''' Actualizar estado expediente
    ''' </summary>
    ''' <param name="ID">Identificador del expediente</param>
    ''' <param name="idEstado">Estado del expediente</param>
    ''' <returns>Valor booleano que indica si se actualizo el registro en la base de datos</returns>
    Public Function ActualizarExpediente(ByVal ID As String, ByVal idEstado As String)
        Dim expediente As Datos.EJEFISGLOBAL = ObtenerExpedientePorId(ID)
        expediente.EFIESTADO = idEstado
        Utils.salvarDatos(db)
        Dim array As ArrayList = New ArrayList
        array.Add(idEstado)
        Utils.ValidaLog(_Auditoria, "UPDATE EJEFISGLOBAL (EFIESTADO) VALUES ", array)
        Return True
    End Function

    ''' <summary>
    ''' Actualizar etapa procesal expediente
    ''' </summary>
    ''' <param name="ID">Identificador del expediente</param>
    ''' <param name="idEstadoProcesal">Estado del expediente</param>
    ''' <param name="idEtapaProcesal">Estpa procesal del expediente</param>
    ''' <returns>Valor booleano que indica si se actualizo el registro en la base de datos</returns>
    Public Function ActualizarExpedienteEtapaProcesal(ByVal ID As String, ByVal idEstadoProcesal As String, ByVal idEtapaProcesal As Int32)
        Dim expediente As Datos.EJEFISGLOBAL = ObtenerExpedientePorId(ID)
        expediente.EFIESTADO = idEstadoProcesal
        expediente.EFIETAPAPROCESAL = idEtapaProcesal
        Utils.salvarDatos(db)
        Dim array As ArrayList = New ArrayList
        array.Add(idEstadoProcesal)
        array.Add(idEtapaProcesal)
        Utils.ValidaLog(_Auditoria, "UPADATE EJEFISGLOBAL (IDESTADOPROCESAL, IDETAPAPROCESAL) ", array)
        Return True
    End Function
End Class
