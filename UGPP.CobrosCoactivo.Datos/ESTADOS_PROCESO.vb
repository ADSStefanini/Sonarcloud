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

Partial Public Class ESTADOS_PROCESO
    Public Property codigo As String
    Public Property nombre As String
    Public Property termino As Nullable(Of Integer)
    Public Property max_dias_gestion_amarillo As Nullable(Of Integer)
    Public Property max_dias_gestion_rojo As Nullable(Of Integer)

    Public Overridable Property ETAPA_PROCESAL As ICollection(Of ETAPA_PROCESAL) = New HashSet(Of ETAPA_PROCESAL)
    Public Overridable Property ESTADOS_PROCESO_GESTOR As ICollection(Of ESTADOS_PROCESO_GESTOR) = New HashSet(Of ESTADOS_PROCESO_GESTOR)

End Class