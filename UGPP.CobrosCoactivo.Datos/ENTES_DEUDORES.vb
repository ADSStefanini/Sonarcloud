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

Partial Public Class ENTES_DEUDORES
    Public Property ED_TipoId As String
    Public Property ED_Codigo_Nit As String
    Public Property ED_DigitoVerificacion As String
    Public Property ED_TipoPersona As String
    Public Property ED_Nombre As String
    Public Property ED_EnteReal As String
    Public Property ED_idzapen As String
    Public Property ED_Excluir As Nullable(Of Boolean)
    Public Property ED_Fecha550 As Nullable(Of Date)
    Public Property ED_EstadoPersona As String
    Public Property ED_TipoAportante As String
    Public Property ED_TarjetaProf As String
    Public Property VAL_NO_MATRICULA_MERCANTIL As String

    Public Overridable Property TIPOS_APORTANTES As TIPOS_APORTANTES
    Public Overridable Property ESTADOS_PERSONA As ESTADOS_PERSONA
    Public Overridable Property EJEFISGLOBAL As ICollection(Of EJEFISGLOBAL) = New HashSet(Of EJEFISGLOBAL)
    Public Overridable Property DIRECCIONES As ICollection(Of DIRECCIONES) = New HashSet(Of DIRECCIONES)
    Public Overridable Property TIPOS_IDENTIFICACION As TIPOS_IDENTIFICACION

End Class