Imports System.Configuration

Public Class TiposCausalesReasignacionDAL
    Inherits AccesObject(Of Entidades.TiposCausalesReasignacion)

    Public Property ConnectionString As String

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    ''' <summary>
    ''' Obtiene la lista de causales de reasignación activos
    ''' </summary>
    ''' <returns>Lista de Objetos Datos.TIPOS_CAUSALES_REASIGNACION</returns>
    Public Function obtenerCausalesReasignacionActivos() As List(Of Datos.TIPOS_CAUSALES_REASIGNACION)
        Dim causalesReasignacion = (From cr In db.TIPOS_CAUSALES_REASIGNACION
                                    Where cr.IND_ESTADO.Equals(True)
                                    Order By cr.VAL_TIPO_CAUSAL_REASIGNACION
                                    Select cr).ToList()
        Return causalesReasignacion
    End Function
End Class
