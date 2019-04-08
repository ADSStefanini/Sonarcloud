SET IDENTITY_INSERT [dbo].[PERFILES] ON 

UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_A_SUPERADMIN' , ind_estado = 1 WHERE [codigo] = 1
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_SUPERVISOR' , ind_estado = 1 WHERE [codigo] = 2
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_REVISOR' , ind_estado = 1 WHERE [codigo] = 3
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_GESTOR' , ind_estado = 1 WHERE [codigo] = 4
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_REPARTIDOR' , ind_estado = 1 WHERE [codigo] = 5
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_VERIFICADOROPAGOS' , ind_estado = 1 WHERE [codigo] = 6
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_CREADOROUSUARIOS' , ind_estado = 1 WHERE [codigo] = 7
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_GESTOROINFORMACION' , ind_estado = 1 WHERE [codigo] = 8

INSERT [dbo].[PERFILES] ([codigo], [nombre], [val_ldap_group], [ind_estado]) VALUES (9, N'GESTOR COBROS PERSUASIVO', N'COAC_U', 1)
INSERT [dbo].[PERFILES] ([codigo], [nombre], [val_ldap_group], [ind_estado]) VALUES (10, N'ESTUDIO DE TÍTULOS', N'COAC_U_ESTUDIOTITULOS', 1)
INSERT [dbo].[PERFILES] ([codigo], [nombre], [val_ldap_group], [ind_estado]) VALUES (11, N'AREA ORIGEN', N'COAC_U_CARGUETITULOS', 1)

SET IDENTITY_INSERT [dbo].[PERFILES] OFF
