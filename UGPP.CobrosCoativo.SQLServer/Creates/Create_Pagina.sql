/****** Object:  Table [dbo].[PAGINA]    Script Date: 07/11/2018 15:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAGINA](
	[pk_codigo] [int] IDENTITY(1,1) NOT NULL,
	[val_nombre] [nvarchar](100) NOT NULL,
	[val_url] [nvarchar](100) NULL,
	[fk_padre] [int] NULL,
	[ind_pagina_interna] [bit] NOT NULL,
	[ind_estado] [bit] NOT NULL,
 CONSTRAINT [PK_PAGINA] PRIMARY KEY CLUSTERED 
(
	[pk_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PAGINA] ADD  CONSTRAINT [DF_PAGINA_ind_pagina_interna]  DEFAULT ((0)) FOR [ind_pagina_interna]
GO
ALTER TABLE [dbo].[PAGINA]  WITH CHECK ADD  CONSTRAINT [FK_PAGINA_FK_PADRE] FOREIGN KEY([fk_padre])
REFERENCES [dbo].[PAGINA] ([pk_codigo])
GO
ALTER TABLE [dbo].[PAGINA] CHECK CONSTRAINT [FK_PAGINA_FK_PADRE]
GO