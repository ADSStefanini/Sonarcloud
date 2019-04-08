Public Class Pagina
    Public Property pk_codigo As Int32
    Public Property val_nombre As String
    Public Property val_url As String
    Public Property fk_padre As Int32?
    Public Property Padre As Pagina
    Public Property ind_pagina_interna As Boolean?
    Public Property ind_estado As Enumeraciones.Estado
    Public Property ind_nivel As Int32?
End Class
