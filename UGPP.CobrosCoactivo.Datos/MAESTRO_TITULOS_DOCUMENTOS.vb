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

Partial Public Class MAESTRO_TITULOS_DOCUMENTOS
    Public Property ID_MAESTRO_TITULOS_DOCUMENTOS As Long
    Public Property ID_DOCUMENTO_TITULO As Nullable(Of Integer)
    Public Property ID_MAESTRO_TITULO As Nullable(Of Long)
    Public Property DES_RUTA_DOCUMENTO As String
    Public Property TIPO_RUTA As Nullable(Of Integer)
    Public Property COD_GUID As String
    Public Property COD_TIPO_DOCUMENTO_AO As String
    Public Property NOM_DOC_AO As String
    Public Property OBSERVA_LEGIBILIDAD As String
    Public Property NUM_PAGINAS As Nullable(Of Integer)
    Public Property IND_DOC_SINCRONIZADO As Nullable(Of Boolean)

    Public Overridable Property MAESTRO_TITULOS As MAESTRO_TITULOS

End Class
