SET IDENTITY_INSERT [dbo].[PAGINA] ON 
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (1, N'Seguridad', NULL, NULL, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (2, N'Páginas', N'/Security/admin/paginas/paginas.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (3, N'Agregar Página', N'/Security/admin/paginas/addPagina.aspx', 2, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (5, N'Editar Pagina', N'/Security/paginas/editPagina.aspx?idPagina=', 2, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (6, N'Perfiles', N'/Security/admin/perfiles/perfiles.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (7, N'Agregar Perfil', N'/Security/admin/perfiles/addPerfil.aspx', 6, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (8, N'Editar Perfil', N'/Security/admin/perfiles/editPerfil.aspx?idPerfil=', 6, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (10, N'Acceso por Perfil', N'/Security/admin/adminAccesoPerfiles.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (12, N'Modulos', N'/Security/admin/modulos/modulos-list.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (14, N'Paginas por perfil', N'/Security/admin/AdminAccesoPerfilesPaginas.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (18, N'Agregar modulo', N'/Security/admin/modulos/addModulo.aspx', 12, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (19, N'Editar Módulo', N'/Security/admin/modulos/editModulo.aspx?idModulo=', 12, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (20, N'Acceso Módulos por Perfil', N'Security/admin/AdminAccesoPerfilesModulos.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (21, N'Paginaprueba1 editar', N'http://172.16.11.55:8021/UGPPBase/Security/images/icons/informes96x96.png', 2, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (22, N'Bandeja', N'/Security/Area_Origen/BandejaAreaOrigen.aspx', NULL, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (23, N'Crear titulo', N'/Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx?AreaOrigenId=2', NULL, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[PAGINA] OFF
GO