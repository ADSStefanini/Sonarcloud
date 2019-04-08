ALTER TABLE [dbo].[PERFILES] DROP CONSTRAINT [PK_roles]
GO
CREATE TABLE [dbo].[TempPERFILES]
(
	[codigo] [bigint] IDENTITY (1,1) NOT NULL,
	[nombre] [varchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[val_ldap_group] [varchar] (150) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[ind_estado] [bit] NOT NULL

) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[TempPERFILES] ON
INSERT INTO [dbo].[TempPERFILES] ([codigo],[nombre],[val_ldap_group],[ind_estado]) SELECT [codigo],ISNULL([nombre],''),'',0 FROM [dbo].[PERFILES]
SET IDENTITY_INSERT [dbo].[TempPERFILES] OFF
GO

DROP TABLE [dbo].[PERFILES]
GO
EXEC sp_rename N'[dbo].[TempPERFILES]',N'PERFILES', 'OBJECT'
GO


ALTER TABLE [dbo].[PERFILES] ADD CONSTRAINT [PK_roles] PRIMARY KEY CLUSTERED
	(
		[codigo] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO