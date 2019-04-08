/****** Object:  Table [dbo].[MODULO]    Script Date: 07/11/2018 15:53:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MODULO](
	[pk_codigo] [int] IDENTITY(1,1) NOT NULL,
	[val_nombre] [nvarchar](150) NOT NULL,
	[val_url] [nvarchar](150) NOT NULL,
	[val_url_icono] [nvarchar](150) NULL,
	[ind_estado] [bit] NOT NULL,
 CONSTRAINT [PK_MODULO] PRIMARY KEY CLUSTERED 
(
	[pk_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO