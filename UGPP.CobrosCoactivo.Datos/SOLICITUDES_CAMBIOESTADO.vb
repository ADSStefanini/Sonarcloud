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

Partial Public Class SOLICITUDES_CAMBIOESTADO
    Public Property idunico As Long
    Public Property NroExp As String
    Public Property abogado As String
    Public Property fecha As Nullable(Of Date)
    Public Property estadoactual As String
    Public Property estado As String
    Public Property estadosol As Nullable(Of Integer)
    Public Property observac As String
    Public Property grupo As String
    Public Property revisor As String
    Public Property aprob_revisor As String
    Public Property nota_revisor As String
    Public Property fecha_aprob_revisor As Nullable(Of Date)
    Public Property ejecutor As String
    Public Property aprob_ejecutor As String
    Public Property nota_ejecutor As String
    Public Property fecha_aprob_ejecutor As Nullable(Of Date)
    Public Property nivel_escalamiento As Nullable(Of Integer)
    Public Property efietapaprocesal As Nullable(Of Integer)

    Public Overridable Property TAREA_SOLICITUD As ICollection(Of TAREA_SOLICITUD) = New HashSet(Of TAREA_SOLICITUD)
    Public Overridable Property TAREA_SOLICITUD1 As ICollection(Of TAREA_SOLICITUD) = New HashSet(Of TAREA_SOLICITUD)

End Class