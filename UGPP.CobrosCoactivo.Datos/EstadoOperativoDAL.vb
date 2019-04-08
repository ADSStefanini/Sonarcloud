Imports System.Configuration

Public Class EstadoOperativoDAL
    Inherits AccesObject(Of Entidades.EstadoOperativo)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _Auditoria As Entidades.LogAuditoria

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
    ''' Obtiene los estados operativos activos relacionados con los títulos
    ''' </summary>
    ''' <returns>Lista de objetos del tipo Datos.ESTADO_OPERATIVO</returns>
    Public Function obtenerEstadosOperativosActivosTitulos() As List(Of Datos.ESTADO_OPERATIVO)
        Dim estadosOperativos = (From eo In db.ESTADO_OPERATIVO
                                 Where eo.COD_TIPO_OBJ = Entidades.Enumeraciones.DominioDetalle.Titulo
                                 Order By eo.VAL_NOMBRE
                                 Select eo).ToList()
        Return estadosOperativos
    End Function
End Class
