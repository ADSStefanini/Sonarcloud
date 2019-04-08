Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class EntesDeudoresBLL

    ''' <summary>
    ''' Clase de comunicaión para la conexión a la DB
    ''' </summary>
    Dim _entesDeudoresDAL As EntesDeudoresDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        _entesDeudoresDAL = New EntesDeudoresDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        _entesDeudoresDAL = New EntesDeudoresDAL(_AuditEntity)
    End Sub

    ''' <summary>
    ''' Retorna los deudores relacionados con un título
    ''' </summary>
    ''' <param name="prmIntTitulo">Número del título</param>
    ''' <returns>Lista de objetos del tipo Entidades.DeudoresExpediente)</returns>
    Public Function obtenerDeudoresPorTitulo(ByVal prmIntTitulo As Int32) As List(Of Entidades.EntesDeudores)
        Return _entesDeudoresDAL.obtenerDeudoresPorTitulo(prmIntTitulo)
    End Function

End Class
