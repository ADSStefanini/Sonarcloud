
GO

ALTER TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS] DROP CONSTRAINT [FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_MAESTRO_TITULO]
GO

ALTER TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS] DROP CONSTRAINT [FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_DOCUMENTO_TITULO]
GO

/****** Object:  Table [dbo].[MAESTRO_TITULOS_DOCUMENTOS]    Script Date: 26/11/2018 15:31:44 ******/
DROP TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS]
GO

/****** Object:  Table [dbo].[MAESTRO_TITULOS_DOCUMENTOS]    Script Date: 26/11/2018 15:31:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS](
	[ID_MAESTRO_TITULOS_DOCUMENTOS] [bigint] IDENTITY(1,1) NOT NULL,
	[ID_DOCUMENTO_TITULO] [int] NULL,
	[ID_MAESTRO_TITULO] [bigint] NULL,
	[DES_RUTA_DOCUMENTO] [varchar](200) NULL,
	[TIPO_RUTA] [int] NULL,
	[COD_GUID] [varchar](50)  NULL,
	[COD_TIPO_DOCUMENTO_AO] [varchar](50)  NULL,
	[NOM_DOC_AO] [varchar](50)  NULL,
	[OBSERVA_LEGIBILIDAD] [varchar](50)  NULL,
	[NUM_PAGINAS] [int]  NULL,
 CONSTRAINT [PK_MAESTRO_TITULOS_DOCUMENTOS] PRIMARY KEY CLUSTERED 
(
	[ID_MAESTRO_TITULOS_DOCUMENTOS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS]  WITH CHECK ADD  CONSTRAINT [FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_DOCUMENTO_TITULO] FOREIGN KEY([ID_DOCUMENTO_TITULO])
REFERENCES [dbo].[DOCUMENTO_TITULO] ([ID_DOCUMENTO_TITULO])
GO

ALTER TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS] CHECK CONSTRAINT [FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_DOCUMENTO_TITULO]
GO

ALTER TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS]  WITH CHECK ADD  CONSTRAINT [FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_MAESTRO_TITULO] FOREIGN KEY([ID_MAESTRO_TITULO])
REFERENCES [dbo].[MAESTRO_TITULOS] ([idunico])
GO

ALTER TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS] CHECK CONSTRAINT [FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_MAESTRO_TITULO]
GO


