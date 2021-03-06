﻿-- =============================================
-- Author:		Stefanini - yeferson alba
-- Create date: 2018-11-30
-- Description:	Crear tabla [TIPOS_CAUSALES_REASIGNACION]
-- =============================================
/****** Object:  Table [dbo].[TIPOS_CAUSALES_REASIGNACION]    Script Date: 29/11/2018 23:02:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'TIPOS_CAUSALES_REASIGNACION' AND TABLE_SCHEMA = 'dbo')
    drop table dbo.TIPOS_CAUSALES_REASIGNACION;
CREATE TABLE [dbo].[TIPOS_CAUSALES_REASIGNACION](
	[ID_TIPO_CAUSAL_REASIGNACION] [int] NOT NULL,
	[VAL_TIPO_CAUSAL_REASIGNACION] [varchar](250) NOT NULL,
	[IND_ESTADO] [bit] NOT NULL,
 CONSTRAINT [PK_TIPOS_CAUSALES_REASIGNACION] PRIMARY KEY CLUSTERED 
(
	[ID_TIPO_CAUSAL_REASIGNACION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO