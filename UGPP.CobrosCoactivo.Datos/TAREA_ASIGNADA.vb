'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class TAREA_ASIGNADA
    Public Property ID_TAREA_ASIGNADA As Long
    Public Property VAL_USUARIO_NOMBRE As String
    Public Property COD_TIPO_OBJ As Integer
    Public Property ID_UNICO_TITULO As Nullable(Of Long)
    Public Property EFINROEXP_EXPEDIENTE As String
    Public Property FEC_ACTUALIZACION As Date
    Public Property FEC_ENTREGA_GESTOR As Nullable(Of Date)
    Public Property VAL_PRIORIDAD As Integer
    Public Property IND_TITULO_PRIORIZADO As Nullable(Of Boolean)
    Public Property COD_ESTADO_OPERATIVO As Integer
    Public Property ID_TAREA_OBSERVACION As Nullable(Of Integer)

    Public Overridable Property ALMACENAMIENTO_TEMPORAL As ICollection(Of ALMACENAMIENTO_TEMPORAL) = New HashSet(Of ALMACENAMIENTO_TEMPORAL)
    Public Overridable Property TAREA_OBSERVACION As TAREA_OBSERVACION
    Public Overridable Property TAREA_SOLICITUD As ICollection(Of TAREA_SOLICITUD) = New HashSet(Of TAREA_SOLICITUD)

End Class
