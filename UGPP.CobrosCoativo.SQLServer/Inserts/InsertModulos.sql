SET IDENTITY_INSERT [dbo].[MODULO] ON 
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (1, N'Auditoria', N'/Security/Maestros/LOG_AUDITORIA.aspx', N'/Security/images/icons/auditar.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (2, N'Diccionario Auditoria', N'/Security/Maestros/DICCIONARIO_AUDITORIA.aspx', N'/Security/images/icons/Blocnote.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (3, N'Seguridad', N'/Security/modulos/maestro-acceso.aspx', N'/Security/images/icons/Keys.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (4, N'Datos basicos', N'/Securiry/MenuMaestros.aspx', N'/Security/images/icons/HP-Control.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (5, N'Administrar usarios', N'/Security/menu4.aspx', N'/Security/images/icons/Keys.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (6, N'Administar expedientes', N'/Security/Maestros/EJEFISGLOBALREPARTIDOR.aspx', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (7, N'Log / Auditoria procesos', N'/Security/Maestros/LOG_AUDITORIA.aspx', N'/Security/images/icons/auditar.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (8, N'Informes', N'/Security/FrmGrupoReportes.aspx', N'/Security/images/icons/informes96x96.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (9, N'Sincronizar usuarios', N'/Security/Maestros/sincronizarusuarios.aspx', N'/Security/images/usuarios.png', 1)
GO
SET IDENTITY_INSERT [dbo].[MODULO] OFF
GO