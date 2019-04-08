Imports System.Configuration

Public Class TiposCausalesPriorizacionDAL
    Inherits AccesObject(Of Entidades.TiposCausalesPriorizacion)

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
    ''' Obtiene la lista de causales de priorización activos
    ''' </summary>
    ''' <returns>Lista de Objetos Datos.TIPOS_CAUSALES_PRIORIZACION</returns>
    Public Function obtenerCausalesPriorizacionnActivos() As List(Of Datos.TIPOS_CAUSALES_PRIORIZACION)
        Dim causalesPriorizacion = (From cr In db.TIPOS_CAUSALES_PRIORIZACION
                                    Where cr.IND_ESTADO.Equals(True)
                                    Order By cr.VAL_TIPO_CAUSAL_PRIORIZACION
                                    Select cr).ToList()
        Return causalesPriorizacion
    End Function
End Class
