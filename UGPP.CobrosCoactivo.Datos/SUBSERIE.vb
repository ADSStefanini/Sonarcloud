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

Partial Public Class SUBSERIE
    Public Property Id_Subserie As Integer
    Public Property Id_Serie As Integer
    Public Property Codigo_Subserie As String
    Public Property Nombre_Subserie As String
    Public Property Activo As Boolean

    Public Overridable Property SERIE As SERIE
    Public Overridable Property TIPO_DOCUMENTAL As ICollection(Of TIPO_DOCUMENTAL) = New HashSet(Of TIPO_DOCUMENTAL)

End Class