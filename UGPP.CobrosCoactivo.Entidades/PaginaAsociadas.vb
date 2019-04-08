Public Class PaginaAsociadas
    Public Property fk_pagina_disparadora_id As Int32
    Public Property pagina_disparadora As Pagina
    Public Property pagina_asociada As Pagina
    Public Property fk_pagina_asociada_id As Int32
    Public Property ind_estado As Enumeraciones.Estado
End Class
