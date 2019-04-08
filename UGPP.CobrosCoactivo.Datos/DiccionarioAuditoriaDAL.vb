Option Strict On
Imports System.Configuration
Imports System.Data.Entity
Imports UGPP.CobrosCoactivo.Entidades

Public Class DiccionarioAuditoriaDAL
    Inherits AccesObject(Of Entidades.DiccionarioAditoria)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Private Property _Auditoria As Entidades.LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As Entidades.LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _Auditoria = auditLog
    End Sub

    ''' <summary>
    ''' Obtiene el diccionario para la auditoria
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Public Function obtenerDiccionarioPorLlave(ByVal llave As String) As List(Of Datos.DICCIONARIO_AUDITORIA)
        Return (From m In db.DICCIONARIO_AUDITORIA
                Where m.VALOR_ORIGINAL = llave
                Select m).ToList()
    End Function

    ''' <summary>
    ''' Obtiene todas las llaves del disicionario
    ''' </summary>
    ''' <returns></returns>
    Public Function obtenerDiccionario() As List(Of Datos.DICCIONARIO_AUDITORIA)
        Return (From m In db.DICCIONARIO_AUDITORIA
                Select m).ToList()
    End Function

    ''' <summary>
    ''' Guarda un nuevo diccionario
    ''' </summary>
    ''' <param name="diccionario"></param>
    ''' <returns></returns>
    Public Function guardarDiccionario(ByVal diccionario As Datos.DICCIONARIO_AUDITORIA) As Boolean
        db.DICCIONARIO_AUDITORIA.Add(diccionario)
        Utils.salvarDatos(db)
        Dim array As ArrayList = New ArrayList
        array.Add(diccionario.ACTIVO.ToString)
        array.Add(diccionario.VALOR_DESTINO.ToString)
        array.Add(diccionario.VALOR_ORIGINAL.ToString)
        Utils.ValidaLog(_Auditoria, "INSERT INTO ", array)
        Return True
    End Function

    ''' <summary>
    ''' Actualiza el diccionario
    ''' </summary>
    ''' <param name="diccionario"></param>
    ''' <returns></returns>
    Public Function actualizarDiccionario(ByVal diccionario As Datos.DICCIONARIO_AUDITORIA) As Boolean
        db.DICCIONARIO_AUDITORIA.Attach(diccionario)
        db.Entry(diccionario).State = EntityState.Modified
        Utils.salvarDatos(db)
        Dim array As ArrayList = New ArrayList
        array.Add(diccionario.ACTIVO.ToString)
        array.Add(diccionario.VALOR_DESTINO.ToString)
        array.Add(diccionario.VALOR_ORIGINAL.ToString)
        Utils.ValidaLog(_Auditoria, "UPDATE DICCIONARIO_AUDITORIA ", array)
        Return True
    End Function

    ''' <summary>
    ''' Obtiene el diccionario de datos por un like de la llave
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Public Function obtenerDiccionarioLike(ByVal llave As String) As List(Of Datos.DICCIONARIO_AUDITORIA)
        Return (From m In db.DICCIONARIO_AUDITORIA
                Where m.VALOR_ORIGINAL.Contains(llave)
                Select m).ToList()
    End Function
End Class
