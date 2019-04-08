Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades


Public Class ObservacionCumpleBLL
    Private Property observacionesCNCGralDAL As ObservacionesCNCGralDAL
    Private Property observacionesCNCDocDAL As ObservacionesCNCDocDAL
    Private Property tipificacionCNCDAL As TipificacionCNCDAL
    Private Property _AuditEntity As LogAuditoria

    Public Sub New()
        observacionesCNCGralDAL = New ObservacionesCNCGralDAL()
        observacionesCNCDocDAL = New ObservacionesCNCDocDAL()
        tipificacionCNCDAL = New TipificacionCNCDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _AuditEntity = auditData
        observacionesCNCGralDAL = New ObservacionesCNCGralDAL()
        observacionesCNCDocDAL = New ObservacionesCNCDocDAL()
        tipificacionCNCDAL = New TipificacionCNCDAL()
    End Sub

    ''' <summary>
    '''  Metodo que Inserta el comentario de CNC General
    ''' </summary>
    ''' <param name="ID_UNICO_MT"></param>
    ''' <param name="USUARIO"></param>
    ''' <param name="OBSERVACIONES"></param>
    ''' <param name="CUMPLE_NOCUMPLE"></param>
    Public Sub InsertaCNCComentarioC(ByVal ID_UNICO_MT As Int64, ByVal USUARIO As String, ByVal OBSERVACIONES As String, ByVal DESTINATARIO As String, CUMPLE_NOCUMPLE As Boolean)
        observacionesCNCGralDAL.InsertaObservacionCNC(ID_UNICO_MT, USUARIO, OBSERVACIONES, DESTINATARIO, CUMPLE_NOCUMPLE)
    End Sub
    ''' <summary>
    ''' Metodo que Inserta el comentario de CNC De los documentos
    ''' </summary>
    ''' <param name="ID_UNICO_MT"></param>
    ''' <param name="ID_DOCUMENTO"></param>
    ''' <param name="USUARIO"></param>
    ''' <param name="OBSERVACIONES"></param>

    Public Sub InsertaCNCComentarioDocC(ByVal ID_UNICO_MT As Int64, ByVal ID_DOCUMENTO As Int64, ByVal USUARIO As String, ByVal DESTINATARIO As String, ByVal CUMPLENOCUMPLE As Boolean, ByVal OBSERVACIONES As String)
        observacionesCNCDocDAL.InsertaObservacionCNCDoc(ID_UNICO_MT, ID_DOCUMENTO, USUARIO, CUMPLENOCUMPLE, DESTINATARIO, OBSERVACIONES)
    End Sub
    ''' <summary>
    ''' Metodo que Inserta la tipificacion del CNC Gral
    ''' </summary>
    ''' <param name="ID_TIPIFICACION"></param>
    ''' <param name="ID_UNICO_MT"></param>
    ''' <param name="USUARIO"></param>
    Public Sub InsertaCNCTipificacionC(ByVal ID_TIPIFICACION As Int64, ByVal ID_UNICO_MT As Int64, ByVal USUARIO As String)
        tipificacionCNCDAL.InsertaTipificacionCNC(ID_TIPIFICACION, ID_UNICO_MT, USUARIO)
    End Sub
End Class

