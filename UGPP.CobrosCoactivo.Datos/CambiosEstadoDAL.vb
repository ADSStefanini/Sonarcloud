Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class CambiosEstadoDAL
    Inherits AccesObject(Of Entidades.CambiosEstado)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _AuditLog As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _AuditLog = auditLog
    End Sub

    ''' <summary>
    ''' Agregar un nuevo registro a la tabla CAMBIOS_ESTADO
    ''' </summary>
    ''' <param name="prmObjCambiosEstado">Objeto del tipo Entidades.CambiosEstado</param>
    ''' <returns>Objeto del tipo Datos.CAMBIOS_ESTADO</returns>
    Public Function guardarCambiosEstado(ByVal prmObjCambiosEstado As Entidades.CambiosEstado) As Datos.CAMBIOS_ESTADO
        Dim cambioEstado As Datos.CAMBIOS_ESTADO = New Datos.CAMBIOS_ESTADO()
        cambioEstado.NroExp = prmObjCambiosEstado.NroExp
        cambioEstado.repartidor = prmObjCambiosEstado.repartidor
        cambioEstado.abogado = prmObjCambiosEstado.abogado
        cambioEstado.fecha = prmObjCambiosEstado.fecha
        cambioEstado.estado = prmObjCambiosEstado.estado
        cambioEstado.estadopago = prmObjCambiosEstado.estadopago
        cambioEstado.estadooperativo = prmObjCambiosEstado.estadooperativo
        cambioEstado.etapaprocesal = prmObjCambiosEstado.etapaprocesal
        db.CAMBIOS_ESTADO.Add(cambioEstado)
        Utils.salvarDatos(db)
        Dim array As ArrayList = New ArrayList()
        array.Add(prmObjCambiosEstado.NroExp.ToString)
        array.Add(prmObjCambiosEstado.repartidor.ToString)
        array.Add(prmObjCambiosEstado.abogado.ToString)
        array.Add(prmObjCambiosEstado.fecha.ToString)
        array.Add(prmObjCambiosEstado.estado.ToString)
        array.Add(prmObjCambiosEstado.estadopago.ToString)
        array.Add(prmObjCambiosEstado.estadooperativo.ToString)
        array.Add(prmObjCambiosEstado.etapaprocesal.ToString)
        Utils.ValidaLog(_AuditLog, "INSERT INTO CAMBIOS_ESTADO ", array)
        Return cambioEstado
    End Function
End Class
