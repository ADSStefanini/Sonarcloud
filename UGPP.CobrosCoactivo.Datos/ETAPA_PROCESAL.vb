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

Partial Public Class ETAPA_PROCESAL
    Public Property ID_ETAPA_PROCESAL As Integer
    Public Property codigo As String
    Public Property VAL_ETAPA_PROCESAL As String

    Public Overridable Property CAMBIOS_ESTADO As ICollection(Of CAMBIOS_ESTADO) = New HashSet(Of CAMBIOS_ESTADO)
    Public Overridable Property ESTADOS_PROCESO As ESTADOS_PROCESO

End Class
