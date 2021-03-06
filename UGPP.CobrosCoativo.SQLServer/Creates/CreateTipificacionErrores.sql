﻿-- =============================================
-- Author:		Stefanini - yeferson alba
-- Create date: 2018-11-30
-- Description:	Crear tabla tabla [TIPIFICACION_ERRORES]
-- =============================================
/****** Object:  Table [dbo].[TIPIFICACION_ERRORES]    Script Date: 30/11/2018 8:33:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'TIPIFICACION_ERRORES' AND TABLE_SCHEMA = 'dbo')
    drop table dbo.TIPIFICACION_ERRORES;
CREATE TABLE [dbo].[TIPIFICACION_ERRORES](
	[ID_TIPIFICACION_ERROR] [int] NOT NULL,
	[VAL_TIPIFICACION_ERROR] [varchar](250) NOT NULL,
	[IND_ESTADO] [bit] NOT NULL,
 CONSTRAINT [PK_TIPIFICACION_ERRORES] PRIMARY KEY CLUSTERED 
(
	[ID_TIPIFICACION_ERROR] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
