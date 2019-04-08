Public Class PerfilPagina
    Public Property fk_perfil_id As Int32
    Public Property perfil As Perfiles
    Public Property fk_pagina_id As Int32
    Public Property pagina As Pagina
    Public Property ind_puede_ver As Enumeraciones.Estado
    Public Property ind_puede_editar As Enumeraciones.Estado
    Public Property ind_estado As Enumeraciones.Estado?
End Class
