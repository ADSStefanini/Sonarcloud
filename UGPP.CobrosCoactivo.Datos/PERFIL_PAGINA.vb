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

Partial Public Class PERFIL_PAGINA
    Public Property fk_perfil_id As Long
    Public Property fk_pagina_id As Integer
    Public Property ind_puede_ver As Boolean
    Public Property ind_puede_editar As Boolean
    Public Property ind_estado As Boolean

    Public Overridable Property PAGINA As PAGINA
    Public Overridable Property PERFILES As PERFILES

End Class