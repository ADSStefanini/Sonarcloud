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

Partial Public Class ESTADO_OPERATIVO
    Public Property ID_ESTADO_OPERATIVOS As Integer
    Public Property COD_TIPO_OBJ As Integer
    Public Property VAL_NOMBRE As String
    Public Property DEC_ESTADO_OPERATIVO As String
    Public Property IND_ESTADO As Boolean

    Public Overridable Property CAMBIOS_ESTADO As ICollection(Of CAMBIOS_ESTADO) = New HashSet(Of CAMBIOS_ESTADO)

End Class